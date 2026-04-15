using _01_Core.Interfaces;
using _01_Core.Models;
using NModbus;
using NModbus.Device;
using NModbus.Serial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static _01_Core.Models.FunctionCodeHelper;

namespace _02_Infrastructure.Services
{
    public class ModbusMasterService : IModbusMasterService
    {
        public ModbusConnectParams ConnectParams { get; set; }
        public bool IsConnected { get; set; }

        private IModbusFactory _factory;
        private TcpClient _tcpClient;
        private IModbusMaster _master;

        private SerialPort _serialPort;
        private ILoggerService _logger;
        private int _currentReconnectAttempts = 0;
        public ModbusMasterService(ILoggerService logger)
        {
            _factory = new ModbusFactory();
            _logger = logger;
        }
        public async Task<bool> Connect(ModbusConnectParams Params)
        {
            ConnectParams = Params;
            if (IsConnected == true)
                return true;
            try
            {
                _logger.LogInformation($"{ConnectParams.ModbusConnectType} 请求连接中");
                if (ConnectParams.ModbusConnectType == ModbusConnectType.TCP)
                {
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(ConnectParams.IpAdress, ConnectParams.Port);
                    _master = _factory.CreateMaster(_tcpClient);
                }
                else if (ConnectParams.ModbusConnectType == ModbusConnectType.RTU)
                {
                    _serialPort = new SerialPort(ConnectParams.ComPort,
                        ConnectParams.BaudRate,
                        ConnectParams.ParityBit,
                        ConnectParams.DataBit,
                        ConnectParams.StopBit)
                    {
                        ReadTimeout = ConnectParams.TimeOut,
                        WriteTimeout = ConnectParams.TimeOut
                    };
                    _serialPort.Open();
                    var adapter = new SerialPortAdapter(_serialPort);
                    _master = _factory.CreateRtuMaster(adapter);
                }
                _logger.LogInformation($"{ConnectParams.ModbusConnectType} 连接成功");
                IsConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ConnectParams.ModbusConnectType} 连接失败:{ex.Message}", ex);
                await CleanAsync();
                return false;
            }
        }

        public async Task<bool> DisConnect()
        {
            if (IsConnected == false)
                return false;
            try
            {
                await CleanAsync();
                _logger.LogInformation($"{ConnectParams.ModbusConnectType} 断开连接成功");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ConnectParams.ModbusConnectType} 断开连接异常:{ex.Message}", ex);
                return true;
            }
        }
        public async Task CleanAsync()
        {
            _master = null;
            if (ConnectParams.ModbusConnectType == ModbusConnectType.TCP)
            {
                _tcpClient?.Dispose();
                _tcpClient = null;
            }
            else if (ConnectParams.ModbusConnectType == ModbusConnectType.RTU)
            {
                _serialPort?.Dispose();
                _serialPort = null;
            }
            IsConnected = false;
            await Task.CompletedTask;
        }
        private readonly object _registersLock = new object();
        private ObservableCollection<ModbusPoints> _pointsTable;
        private ModbusRequestParams _requestParams;
        private int _intercalMS;
        private Action<ModbusPoints, string> _updateCallBack;
        private CancellationTokenSource _cts;
        public void StartPolling(ModbusRequestParams requestParams, ObservableCollection<ModbusPoints> PointsTable, int IntervalMs, Action<ModbusPoints, string> UpdateCallBack)
        {
            StopPolling();
            _cts = new CancellationTokenSource();

            lock (_registersLock)
            {
                _pointsTable = PointsTable;
            }
            _requestParams = requestParams;
            _intercalMS = IntervalMs;
            _updateCallBack = UpdateCallBack;
            Task.Run(async () =>
            {
                await Polling(_cts.Token);
            });
        }
        private async Task Polling(CancellationToken token)
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (!IsConnected || _master == null)
                {
                    _currentReconnectAttempts++;
                    if (_currentReconnectAttempts > ConnectParams.RetryCount)
                    {
                        Debug.WriteLine("已超过最大重连次数");
                        StopPolling();
                        await DisConnect();
                        return;
                    }
                    Debug.WriteLine($"开始尝试第{_currentReconnectAttempts}次重连");
                    if (await Connect(ConnectParams))
                    {
                        _currentReconnectAttempts = 0;
                    }
                    else
                    {
                        await Task.Delay(ConnectParams.TimeOut,token);
                        continue;
                    }
                }
                if (_pointsTable == null || _pointsTable.Count == 0)
                {
                    await Task.Delay(_intercalMS, token);
                    continue;
                }
                try
                {
                    await GetRegiserAsync(_pointsTable,token);
                }
                catch (Exception)
                {
                    Debug.WriteLine("读取异常");
                    await DisConnect();
                }
                await Task.Delay(_intercalMS, token);
            }
        }

        private async Task GetRegiserAsync(ObservableCollection<ModbusPoints> pointsTable, CancellationToken token)
        {
            var startAddress = (ushort)_requestParams.StartAddress;
            var count = (ushort)_requestParams.Count;

            switch (_requestParams.FunctionCode)
            {
                case FunctionCode.ReadCoils: // 读线圈
                    bool[] coils = _master.ReadCoils(_requestParams.SlaveId, startAddress, count);
                    foreach (var reg in pointsTable)
                    {
                        int index = reg.Address - startAddress;
                        _updateCallBack?.Invoke(reg, (index >= 0 && index < coils.Length) ? (coils[index] ? "1" : "0") : "越界");
                    }
                    break;
                case FunctionCode.ReadDiscreteInputs: // 读离散输入
                    bool[] inputs = _master.ReadInputs(_requestParams.SlaveId, startAddress, count);
                    foreach (var reg in pointsTable)
                    {
                        int index = reg.Address - startAddress;
                        _updateCallBack?.Invoke(reg, (index >= 0 && index < inputs.Length) ? (inputs[index] ? "1" : "0") : "越界");
                    }
                    break;
                case FunctionCode.ReadHoldingRegisters: // 读保持寄存器
                    ushort[] holdings = _master.ReadHoldingRegisters(_requestParams.SlaveId, startAddress, count);
                    foreach (var reg in pointsTable)
                    {
                        int index = reg.Address - startAddress;
                        _updateCallBack?.Invoke(reg, (index >= 0 && index < holdings.Length) ? holdings[index].ToString() : "越界");
                    }
                    break;
                case FunctionCode.ReadInputRegisters: // 读输入寄存器
                    ushort[] inputRegs = _master.ReadInputRegisters(_requestParams.SlaveId, startAddress, count);
                    foreach (var reg in pointsTable)
                    {
                        int index = reg.Address - startAddress;
                        _updateCallBack?.Invoke(reg, (index >= 0 && index < inputRegs.Length) ? inputRegs[index].ToString() : "越界");
                    }
                    break;
                default:
                    foreach (var reg in pointsTable)
                        _updateCallBack?.Invoke(reg, "不支持");
                    break;
            }
        }
        public void StopPolling()
        {
            if(_cts != null)
            _cts.Cancel();
        }
    }
}

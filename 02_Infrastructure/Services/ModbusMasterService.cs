using _01_Core.Interfaces;
using _01_Core.Models;
using NModbus;
using NModbus.Device;
using NModbus.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
        public ModbusMasterService(ILoggerService logger)
        {
            _factory = new ModbusFactory();
            _logger= logger;
        }
        public async Task<bool> Connect(ModbusConnectParams Params)
        {
            ConnectParams = Params;
            if (IsConnected == true)
                return true;
            try
            {
                if (ConnectParams.ModbusConnectType == ModbusConnectType.TCP)
                {
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(ConnectParams.IpAdress, ConnectParams.Port);
                    _master = _factory.CreateMaster(_tcpClient);
                }
                else if(ConnectParams.ModbusConnectType==ModbusConnectType.RTU)
                {
                    _serialPort = new SerialPort(ConnectParams.ComPort,
                        ConnectParams.BaudRate,
                        ConnectParams.ParityBit,
                        ConnectParams.DataBit,
                        ConnectParams.StopBit)
                    {
                        ReadTimeout= ConnectParams.TimeOut,
                        WriteTimeout= ConnectParams.TimeOut
                    };
                     _serialPort.Open();
                    var adapter = new SerialPortAdapter(_serialPort);
                    _master= _factory.CreateRtuMaster(adapter);             
                }
                _logger.LogInformation($"{ConnectParams.ModbusConnectType} 连接成功");
                IsConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ConnectParams.ModbusConnectType} 连接失败:{ex.Message}",ex);
                IsConnected = false;
                _master = null;
                _serialPort?.Close();
                _serialPort = null;
                _tcpClient?.Close();
                _tcpClient = null;
                return false;
            }
        }

        public async Task<bool> DisConnect()
        {
            if (IsConnected == false)
                return false;
            try
            {
                if (ConnectParams.ModbusConnectType == ModbusConnectType.TCP)
                {
                    _tcpClient?.Close();
                    _tcpClient = null;
                }
                else if (ConnectParams.ModbusConnectType == ModbusConnectType.RTU)
                {
                    _serialPort?.Close();
                    _serialPort = null;
                }
                    _master = null;
                _logger.LogInformation($"{ConnectParams.ModbusConnectType} 断开连接");
                IsConnected = false;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ConnectParams.ModbusConnectType} 断开连接异常:{ex.Message}",ex);
                return true;
            }
        }
    }
}

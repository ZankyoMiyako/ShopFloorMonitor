using _01_Core.Events;
using _01_Core.Interfaces;
using _01_Core.Models;
using _02_Infrastructure.Services;
using _03_Modules.DebuggerModule.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _01_Core.Models.FunctionCodeHelper;
using static _03_Modules.DebuggerModule.ViewModels.DebuggerViewModel;

namespace _03_Modules.DebuggerModule.ViewModels
{
    public class DebuggerViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private IModbusMasterService _masterService;
        private IModbusPointsService _modbusPoints;
        private readonly ILoggerService _logger;
        public DebuggerViewModel(IEventAggregator eventAggregator, IModbusMasterService masterService,IModbusPointsService pointsService,LoggerFactory factory)
        {
            _eventAggregator = eventAggregator;
            _masterService = masterService;
            _modbusPoints = pointsService;
            _logger = factory.DebuggerModule;
            DrawerControl = new DelegateCommand<string>(OpenDrawer);
            IsConnectCmd = new DelegateCommand(Connect);
            CleanLogsCmd = new DelegateCommand(CleanLogs);
            GenerateTableCommand = new DelegateCommand(GenerateTable);
            _eventAggregator.GetEvent<DrawerControlEvent>().Subscribe(args =>
            {
                IsRightDrawerOpen = args;

            }, ThreadOption.UIThread);
            _eventAggregator.GetEvent<ModbusConnectParamsUpdateEvent>().Subscribe(args =>
            {
                ConnectParams = args;
            }, ThreadOption.UIThread);           
        }
        private ModbusConnectParams _connectParams = new ModbusConnectParams();
        public ModbusConnectParams ConnectParams
        {
            get { return _connectParams; }
            set
            {
                SetProperty(ref _connectParams, value);
            }
        }

        #region 侧边栏开关and内容属性
        private bool _isRightDrawerOpen;
        public bool IsRightDrawerOpen
        {
            get { return _isRightDrawerOpen; }
            set
            {
                if (SetProperty(ref _isRightDrawerOpen, value))
                {
                    if (!value)
                        UpdateButtonStatus();

                }
            }
        }
        private object _rightDrawerContent;

        public object RightDrawerContent
        {
            get { return _rightDrawerContent; }
            set
            {
                SetProperty(ref _rightDrawerContent, value);
            }
        }

        private TCPConnectView _cachedTCPConnectView;
        private RTUConnectView _cachedRTUConnectView;
        #endregion
        #region 侧边栏开关命令
        public DelegateCommand<string> DrawerControl { get; set; }
        private void OpenDrawer(string obj)
        {
            IsRightDrawerOpen = true;
            switch (obj)
            {
                case "TCP": LoadTCPConnectView(); break;
                case "RTU": LoadRTUConnectView(); break;
            }
            _eventAggregator.GetEvent<ModbusConnectParamsRequestEvent>().Publish(ConnectParams);
        }
        #endregion
        #region 侧边栏内容加载
        private void LoadRTUConnectView()
        {
            if (_cachedRTUConnectView == null)
            {
                _cachedRTUConnectView = new RTUConnectView();
            }
            RightDrawerContent = _cachedRTUConnectView;
        }

        private void LoadTCPConnectView()
        {
            if (_cachedTCPConnectView == null)
            {
                _cachedTCPConnectView = new TCPConnectView();
            }
            RightDrawerContent = _cachedTCPConnectView;
        }
        #endregion
        #region 请求配置内容
        private ModbusRequestParams _requestParams = new ModbusRequestParams();

        public ModbusRequestParams RequestParams
        {
            get { return _requestParams; }
            set
            {
                SetProperty(ref _requestParams, value);
            }
        }
        public FunctionCodeHelper FunctionCodeHelper { get; } = new FunctionCodeHelper();
        #endregion
        #region 主站连接
        private bool _isConnect;

        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                SetProperty(ref _isConnect, value);
            }
        }
        private bool _isConnecting;

        public bool IsConnecting
        {
            get => _isConnecting;
            set
            {
                if (SetProperty(ref _isConnecting, value))
                {
                    RaisePropertyChanged(nameof(CanOperate));  
                }
            }
        }

        public bool CanOperate => !IsConnecting; 
        private async void Connect()
        {
            bool ShouldConnect = IsConnect;
            if (ShouldConnect == true)
            {
                IsConnecting = true;
                IsConnect = await _masterService.Connect(ConnectParams);
                IsConnecting = false;
            }
            else
            {
                IsConnect = await _masterService.DisConnect();
            }
        }

        public DelegateCommand IsConnectCmd { get; set; }
        #endregion
        #region 协议选中逻辑优化
        private bool _TcpButtonSelected = true;

        public bool TcpButtonSelected
        {
            get { return _TcpButtonSelected; }
            set
            {
                SetProperty(ref _TcpButtonSelected, value);
            }
        }
        private bool _rtuButtonSelected;

        public bool RtuButtonSelected
        {
            get { return _rtuButtonSelected; }
            set
            {
                SetProperty(ref _rtuButtonSelected, value);
            }
        }
        public void UpdateButtonStatus()
        {
            if (ConnectParams.ModbusConnectType == ModbusConnectType.TCP)
            {
                TcpButtonSelected = true;
                RtuButtonSelected = false;
            }
            else if (ConnectParams.ModbusConnectType == ModbusConnectType.RTU)
            {
                TcpButtonSelected = false;
                RtuButtonSelected = true;
            }
        }
        #endregion
        #region 日志相关
        public ObservableCollection<string> Logs => _logger.Logs;
        public DelegateCommand CleanLogsCmd { get; set; }
        private void CleanLogs()
        {
            Logs.Clear();
        }
        #endregion
        #region 生成看板命令
        public DelegateCommand GenerateTableCommand {  get; set; }
        #endregion
        #region 测试点表生成接口
        private ObservableCollection<ModbusPoints> _pointTable;

        public ObservableCollection<ModbusPoints> PointTable
        {
            get { return _pointTable; }
            set {
                SetProperty(ref _pointTable, value);
            }
        }
        private void GenerateTable()
        {
            _modbusPoints.RequestParams = RequestParams;
            PointTable= _modbusPoints.GeneratePointsTable();
        }
        #endregion
    }
}

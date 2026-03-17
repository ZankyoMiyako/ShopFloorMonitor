using _01_Core.Events;
using _01_Core.Interfaces;
using _01_Core.Models;
using _03_Modules.DebuggerModule.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _01_Core.Models.FunctionCodeHelper;

namespace _03_Modules.DebuggerModule.ViewModels
{
    public class DebuggerViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private IModbusMasterService _masterService;
        public DebuggerViewModel(IEventAggregator eventAggregator, IModbusMasterService masterService)
        {
            _eventAggregator = eventAggregator;
            _masterService = masterService;
            DrawerControl = new DelegateCommand<string>(OpenDrawer);
            IsConnectCmd = new DelegateCommand(Connect);
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
                SetProperty(ref _isRightDrawerOpen, value);
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
        private async void Connect()
        {
            if (IsConnect == true)
            {
                IsConnect = false;
                IsConnect = await _masterService.Connect(ConnectParams);
            }
            else
            {
                IsConnect = await _masterService.DisConnect();
            }

        }
        
        public DelegateCommand IsConnectCmd { get; set; }
        #endregion
    }
}

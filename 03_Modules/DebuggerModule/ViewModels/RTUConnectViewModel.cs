using _01_Core.Events;
using _01_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Modules.DebuggerModule.ViewModels
{
    internal class RTUConnectViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        public RTUConnectViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DrawerControl = new DelegateCommand<string>(CloseDrawer);
            _eventAggregator.GetEvent<ModbusConnectParamsRequestEvent>().Subscribe(args =>
            {
                ConnectParams = args.Clone();
            }, ThreadOption.UIThread);
        }

        private void CloseDrawer(string obj)
        {
            if (obj == "确定")
            {
                ConnectParams.ModbusConnectType = ModbusConnectType.RTU;
                _eventAggregator.GetEvent<ModbusConnectParamsUpdateEvent>().Publish(ConnectParams);
            }
            _eventAggregator.GetEvent<DrawerControlEvent>().Publish(false);
        }

        public DelegateCommand<string> DrawerControl { get; set; }

        private ModbusConnectParams _connectParams;
        public ModbusConnectParams ConnectParams
        {
            get { return _connectParams; }
            set
            {
                SetProperty(ref _connectParams, value);
            }
        }
        public RTUConnectOptions RTUConnectOptions { get; } = new RTUConnectOptions();
    }
}

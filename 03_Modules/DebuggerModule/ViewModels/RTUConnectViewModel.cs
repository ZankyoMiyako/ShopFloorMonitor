using _01_Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Modules.DebuggerModule.ViewModels
{
    internal class RTUConnectViewModel
    {
        private IEventAggregator _eventAggregator;
        public RTUConnectViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DrawerControl = new DelegateCommand<string>(CloseDrawer);
        }

        private void CloseDrawer(string obj)
        {
            _eventAggregator.GetEvent<DrawerControlEvent>().Publish(false);
        }

        public DelegateCommand<string> DrawerControl { get; set; }
    }
}

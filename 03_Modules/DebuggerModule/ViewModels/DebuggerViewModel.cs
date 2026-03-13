using _03_Modules.DebuggerModule.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Modules.DebuggerModule.ViewModels
{
    public class DebuggerViewModel : BindableBase
    {
        public DebuggerViewModel()
        {
            DrawerControl = new DelegateCommand<string>(OpenDrawer);
        }
        #region 侧边栏开关and内容
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
        public DelegateCommand<string> DrawerControl { get; set; }
        private void OpenDrawer(string obj)
        {
            IsRightDrawerOpen = true;
            switch (obj)
            {
                case "TCP": LoadTCPConnectView(); break;
                case "RTU": LoadRTUConnectView(); break;
            }
        }

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

    }
}

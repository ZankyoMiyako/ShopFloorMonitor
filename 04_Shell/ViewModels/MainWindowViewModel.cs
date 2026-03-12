using _01_Core.Interfaces;
using _01_Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_Shell.ViewModels
{
    internal class MainWindowViewModel : BindableBase,IConfigureService
    {
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
        }
        public void Configure()
        {
            CreateMenuBars();
            _regionManager.Regions["MainWindowRegion"].RequestNavigate("WorkshopView");
        }
        #region 导航
        private IRegionManager _regionManager;
        public DelegateCommand<MenuBar> NavigateCommand { get; set; }
        private void Navigate(MenuBar bar)
        {
            if (bar == null || string.IsNullOrEmpty(bar.ViewName))
                return;
            _regionManager.Regions["MainWindowRegion"].RequestNavigate(bar.ViewName);
        }
        #endregion
        #region 菜单栏
        private ObservableCollection<MenuBar> _menuBars;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return _menuBars; }
            set
            {
                SetProperty(ref _menuBars, value);
            }
        }
        private void CreateMenuBars()
        {
            MenuBars = new ObservableCollection<MenuBar>();
            MenuBars.Add(new MenuBar { Icon = "Factory", MenuName = "车间视图", ViewName = "WorkshopView" });
            MenuBars.Add(new MenuBar { Icon = "Connection", MenuName = "连接视图", ViewName = "TestView" });
            MenuBars.Add(new MenuBar { Icon = "BugOutline", MenuName="调试视图",ViewName = "DebuggerView" });
            MenuBars.Add(new MenuBar { Icon = "DatabaseOutline",MenuName="数据库视图",ViewName = "1" });
            MenuBars.Add(new MenuBar { Icon = "ChartLine",MenuName="趋势视图", ViewName = "1" });
        }

        #endregion
    }
}

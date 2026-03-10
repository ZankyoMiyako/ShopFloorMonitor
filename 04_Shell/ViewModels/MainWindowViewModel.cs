using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_Shell.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            CreateMenuBars();
        }

        public class MenuBar
        {
            public string Icon { get; set; }
            public string ViewName { get; set; }
            public string MenuName { get; set; }
        }
        private ObservableCollection<MenuBar> _menuBars;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return _menuBars; }
            set
            {
                _menuBars = value;
                RaisePropertyChanged();
            }
        }
        private void CreateMenuBars()
        {
            MenuBars = new ObservableCollection<MenuBar>();
            MenuBars.Add(new MenuBar { Icon = "Factory", MenuName = "车间视图", ViewName = "1" });
            MenuBars.Add(new MenuBar { Icon = "HomeOutline", ViewName = "1" });
            MenuBars.Add(new MenuBar { Icon = "HomeOutline", ViewName = "1" });
            MenuBars.Add(new MenuBar { Icon = "HomeOutline", ViewName = "1" });
            MenuBars.Add(new MenuBar { Icon = "HomeOutline", ViewName = "1" });
            MenuBars.Add(new MenuBar { Icon = "HomeOutline", ViewName = "1" });
        }

    }
}

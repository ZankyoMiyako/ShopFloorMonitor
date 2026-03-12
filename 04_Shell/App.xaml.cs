using _01_Core.Interfaces;
using _03_Modules.DebuggerModule.ViewModels;
using _03_Modules.DebuggerModule.Views;
using _03_Modules.ViewModels;
using _03_Modules.Views;
using _04_Shell.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace _04_Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void OnInitialized()
        {
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
            {
                service.Configure();
            }
            base.OnInitialized();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<WorkshopView, WorkshopViewModel>();
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>();
            containerRegistry.RegisterForNavigation<DebuggerView, DebuggerViewModel>();
        }
    }

}

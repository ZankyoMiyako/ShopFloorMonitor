using _01_Core.Interfaces;
using _02_Infrastructure.Services;
using _03_Modules;
using _03_Modules.DebuggerModule;
using _03_Modules.DebuggerModule.ViewModels;
using _03_Modules.DebuggerModule.Views;
using _03_Modules.ViewModels;
using _03_Modules.Views;
using _03_Modules.WorkshopModule;
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
            containerRegistry.RegisterSingleton<LoggerFactory>();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<DebuggerModuleInit>();
            moduleCatalog.AddModule<WorkshopModuleInit>();
        }
    }

}

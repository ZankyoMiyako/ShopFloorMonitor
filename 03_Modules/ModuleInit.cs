using _01_Core.Interfaces;
using _02_Infrastructure.Services;
using _03_Modules.DebuggerModule.ViewModels;
using _03_Modules.DebuggerModule.Views;
using _03_Modules.ViewModels;
using _03_Modules.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Modules
{
    public class ModuleInit : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager= containerProvider.Resolve<IRegionManager>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DebuggerView,DebuggerViewModel>();
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>();
            containerRegistry.RegisterForNavigation<WorkshopView, WorkshopViewModel>();

            containerRegistry.RegisterSingleton<IModbusMasterService, ModbusMasterService>();
            containerRegistry.RegisterSingleton<LoggerFactory>();
        }
    }
}

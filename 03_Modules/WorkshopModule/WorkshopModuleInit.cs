using _01_Core.Interfaces;
using _02_Infrastructure.Services;
using _03_Modules.ViewModels;
using _03_Modules.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Modules.WorkshopModule
{
    public class WorkshopModuleInit : IModule
    {
        private ILoggerService _logger;
        public WorkshopModuleInit(LoggerFactory factory)
        {
            _logger= factory.WorkShopModule;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _logger.LogInformation("车间日志系统初始化成功");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>();
            containerRegistry.RegisterForNavigation<WorkshopView, WorkshopViewModel>();
        }
    }
}

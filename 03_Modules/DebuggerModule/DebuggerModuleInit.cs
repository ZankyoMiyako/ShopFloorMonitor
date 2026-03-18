using _01_Core.Interfaces;
using _02_Infrastructure.Services;
using _03_Modules.DebuggerModule.ViewModels;
using _03_Modules.DebuggerModule.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Modules.DebuggerModule
{
    public class DebuggerModuleInit : IModule
    {
        private readonly ILoggerService _logger;
        public DebuggerModuleInit(LoggerFactory factory)
        {
            _logger = factory.DebuggerModule;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _logger.LogInformation($"日志系统初始化成功");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DebuggerView, DebuggerViewModel>();
            containerRegistry.RegisterSingleton<IModbusMasterService, ModbusMasterService>();
            containerRegistry.RegisterSingleton<LoggerFactory>();
        }
    }
}

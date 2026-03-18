using _01_Core.Interfaces;
using _02_Infrastructure.Sink;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _02_Infrastructure.Services
{
    public class LoggerService : ILoggerService,IDisposable
    {
        public string LoggerName { get; }
        public ObservableCollection<string> Logs {  get; }
        private readonly ILogger _logger;

        public LoggerService(string loggerName)
        {
            LoggerName= loggerName;

            _logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Sink(new ObservableCollectionSink(Logs, Application.Current.Dispatcher))
                        .WriteTo.File($"logs/{LoggerName}/log-.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();
            
            _logger.Information($"{LoggerName}日志系统初始化成功");
        }

        public void LogInformation(string message)=>_logger.Information(message);

        public void LogWarning(string message)=>_logger.Warning(message);

        public void LogError(string message, Exception? ex = null)=>_logger.Error(ex, message);

        public void LogDebug(string message)=>_logger?.Debug(message);

        public void Dispose()=>(_logger as IDisposable)?.Dispose();
    }
}

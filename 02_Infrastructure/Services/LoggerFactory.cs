using _01_Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Infrastructure.Services
{
    public class LoggerFactory
    {
        private readonly ConcurrentDictionary<string, ILoggerService> _logger = new();
        public ILoggerService GetLogger(string loggerName)
        {
            return _logger.GetOrAdd(loggerName,n=>new LoggerService(loggerName));
        }
        public ILoggerService DebuggerModule => GetLogger(nameof(DebuggerModule));
    }
}

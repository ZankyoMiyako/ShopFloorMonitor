using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Interfaces
{
    public interface ILoggerService
    {
        string LoggerName { get; }
        ObservableCollection<string> Logs { get; }

        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception? ex = null);

        void LogDebug(string message);
    }
}

using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace _02_Infrastructure.Sink
{
    internal class ObservableCollectionSink : ILogEventSink
    {
        private readonly ObservableCollection<string> _logs;
        private readonly Dispatcher _dispatcher;
        public ObservableCollectionSink(ObservableCollection<string> logs,Dispatcher  dispatcher)
        {
            _logs = logs ?? throw new ArgumentNullException(nameof(logs));
            _dispatcher =dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
                return;
            var message = $"[{logEvent.Timestamp:G}]{logEvent.RenderMessage()}";
            if (_dispatcher.CheckAccess())
                _logs.Add(message);
            else
                _dispatcher.Invoke(() => _logs.Add(message));
        }
    }
}

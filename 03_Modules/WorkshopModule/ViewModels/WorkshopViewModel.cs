using _01_Core.Interfaces;
using _01_Core.Models;
using _02_Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Modules.ViewModels
{
    public class WorkshopViewModel : BindableBase
    {
        private ILoggerService _logger;
        public WorkshopViewModel(LoggerFactory factory)
        {
            CreateDevices();
            _logger = factory.WorkShopModule;
            CleanLogsCmd=new DelegateCommand(CleanLogs);
        }

        private void CreateDevices()
        {
            Devices = new ObservableCollection<Device>();
            Devices.Add(new Device { ID = "CNC-001", Row = 0, Column = 0, CurrentWorkpiece = "Al-101", Progress = 20, Status = Device.MachineStatus.Running });
            Devices.Add(new Device { ID = "CNC-002", Row = 1, Column = 0, CurrentWorkpiece = "Al-101", Progress = 60, Status = Device.MachineStatus.Running });
            Devices.Add(new Device { ID = "CNC-003", Row = 2, Column = 0, CurrentWorkpiece = "Al-101", Progress = 100, Status = Device.MachineStatus.Waiting });
            Devices.Add(new Device { ID = "CNC-004", Row = 0, Column = 1, CurrentWorkpiece = "Al-201", Progress = 0, Status = Device.MachineStatus.Offline });
            Devices.Add(new Device { ID = "CNC-005", Row = 1, Column = 1, CurrentWorkpiece = "Al-201", Progress = 100, Status = Device.MachineStatus.Waiting });
            Devices.Add(new Device { ID = "CNC-006", Row = 2, Column = 1, CurrentWorkpiece = "Al-201", Progress = 70, Status = Device.MachineStatus.Fault });
        }

        private ObservableCollection<Device> _devices;

        public ObservableCollection<Device> Devices
        {
            get { return _devices; }
            set
            {
                SetProperty(ref _devices, value);
            }
        }
        #region 日志相关
        public ObservableCollection<string> Logs => _logger.Logs;
        public DelegateCommand CleanLogsCmd { get; set; }
        private void CleanLogs()
        {
            Logs.Clear();
        }
        #endregion
    }
}

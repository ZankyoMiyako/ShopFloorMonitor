using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Models
{
    public class Device
    {
        public enum MachineStatus
        {
            Offline,//离线(灰色)
            Running,//加工中(绿色)
            Waiting,//等待中(黄色)
            Fault//故障(红色)
        }

        public string ID { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public MachineStatus Status { get; set; }
        public string CurrentWorkpiece { get; set; }
        public int Progress { get; set; }

    }
}

using _01_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Events
{
    public class ModbusConnectParamsRequestEvent : PubSubEvent<ModbusConnectParams> { }
}

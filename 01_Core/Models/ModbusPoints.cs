using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Models
{
    public class ModbusPoints
    {
            public int Index { get; set; }
            public int Address { get; set; }
            public string Name { get; set; }
            public int Value { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _01_Core.Models.FunctionCodeHelper;

namespace _01_Core.Models
{
    public class ModbusRequestParams
    {
        public byte SlaveId { get; set; } = 1;
        public FunctionCode FunctionCode { get; set; }=FunctionCode.ReadHoldingRegisters;
        public int StartAddress { get; set; } = 0;
        public int Count { get; set; } = 10;

    }

}

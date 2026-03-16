using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Models
{
    public class FunctionCodeHelper
    {
        public enum FunctionCode
        {
            [Description("0x01-读线圈")]
            ReadCoils = 0x01,
            [Description("0x02-读离散输入")]
            ReadDiscreteInputs = 0x02,
            [Description("0x03-读保持寄存器")]
            ReadHoldingRegisters = 0x03,
            [Description("0x04-读输入寄存器")]
            ReadInputRegisters = 0x04,
            [Description("0x05-写单个线圈")]
            WriteSingleCoil = 0x05,
            [Description("0x06-写单个寄存器")]
            WriteSingleRegister = 0x06,
            [Description("0x0F-写多个线圈")]
            WriteMultipleCoils = 0x0F,
            [Description("0x10-写多个寄存器")]
            WriteMultipleRegister = 0x10
        }

        public static string GetDescription(FunctionCode code)
        {
            FieldInfo field = code.GetType().GetField(code.ToString());
            DescriptionAttribute[] descriptions = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descriptions.Length > 0 ? descriptions[0].Description : code.ToString();
        }
        private readonly List<KeyValuePair<FunctionCode, string>> _functionCodeOptions;

        public FunctionCodeHelper()
        {
            _functionCodeOptions = Enum.GetValues<FunctionCode>()
                .Select(code => new KeyValuePair<FunctionCode, string>(code, GetDescription(code)))
                .ToList();
        }

        public IEnumerable<KeyValuePair<FunctionCode, string>> FunctionCodeOptions =>
            _functionCodeOptions;  
    }
}

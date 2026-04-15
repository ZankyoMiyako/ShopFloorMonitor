using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Models
{
    public class ModbusPoints : BindableBase
    {
        public int Index { get; set; }
        public int Address { get; set; }
        public string Name { get; set; }
        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }
    }
}

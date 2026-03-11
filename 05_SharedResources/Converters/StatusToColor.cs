using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static _01_Core.Models.Device;

namespace _99_SharedResources.Converters
{
    public class StatusToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                MachineStatus.Offline => "White",
                MachineStatus.Fault => "Red",
                MachineStatus.Running => "Green",
                MachineStatus.Waiting => "Yellow"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

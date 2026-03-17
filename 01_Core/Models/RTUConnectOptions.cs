using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Models
{
    public class RTUConnectOptions
    {
        public ObservableCollection<string> ComPortOptions { get; }
        public ObservableCollection<int> BaudRateOptions { get; } = new(){ 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200 };
        public ObservableCollection<int> DataBitOptions { get; } = new() { 5,6,7,8};
        public ObservableCollection<StopBits> StopBitOptions { get; } = new() {StopBits.None,StopBits.One,StopBits.Two,StopBits.OnePointFive };
        public ObservableCollection<Parity> ParityBitOptions { get; } = new() {Parity.None,Parity.Odd,Parity.Even,Parity.Mark,Parity.Space };

        public RTUConnectOptions()
        {
            var prots = SerialPort.GetPortNames();
            ComPortOptions = prots.Length > 0 ? new ObservableCollection<string>(prots) : new ObservableCollection<string> { "COM3" };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Models
{
    public class ModbusConnectParams
    {
        //===================公共属性=====================
        public ModbusConnectType ModbusConnectType { get; set; } = ModbusConnectType.TCP;
        public int TimeOut { get; set; } = 2000;
        public int RetryTime { get; set; } = 3;

        //===============TCP特有属性=======================
        public string IpAdress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 502;

        //===============RTU特有属性=========================
        public string SerialPort { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; }=StopBits.One;
        public Parity ParityBits { get; set; } = Parity.None;
    }
    public enum ModbusConnectType
    {
        TCP,
        RTU
    }
}

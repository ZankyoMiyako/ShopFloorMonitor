using _01_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Interfaces
{
    public interface IModbusMasterService
    {
        ModbusConnectParams ConnectParams { get; set; }

        bool IsConnected { get; set; }

        Task<bool> Connect(ModbusConnectParams Params);

        Task<bool> DisConnect();
    }
}

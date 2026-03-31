using _01_Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _01_Core.Models.FunctionCodeHelper;

namespace _01_Core.Interfaces
{
    public interface IModbusMasterService
    {
        ModbusConnectParams ConnectParams { get; set; }

        bool IsConnected { get; set; }

        Task<bool> Connect(ModbusConnectParams Params);

        Task<bool> DisConnect();

        void StartPolling(ModbusRequestParams requestParams, ObservableCollection<ModbusPoints> PointsTable,int IntervalMs,Action<ModbusPoints,string> UpdateCallBack);
        void StopPolling();
    }
}

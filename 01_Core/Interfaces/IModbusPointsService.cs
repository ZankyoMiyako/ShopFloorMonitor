using _01_Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Core.Interfaces
{
    public interface IModbusPointsService
    {
        ObservableCollection<ModbusPoints> GeneratePointsTable(ModbusRequestParams requestParams);
    }
}

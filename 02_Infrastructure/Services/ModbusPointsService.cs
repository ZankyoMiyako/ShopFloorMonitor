using _01_Core.Interfaces;
using _01_Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Infrastructure.Services
{
    public class ModbusPointsService : IModbusPointsService
    {
        private ModbusRequestParams _requestParams;
        private ObservableCollection<ModbusPoints> PointsTable;
        public ObservableCollection<ModbusPoints> GeneratePointsTable(ModbusRequestParams requestParams)
        {
            _requestParams= requestParams;
            PointsTable = new ObservableCollection<ModbusPoints>();
            for (int i = 0; i < _requestParams.Count; i++)
            {
                PointsTable.Add(new ModbusPoints
                {
                    Index = i + 1,
                    Address = _requestParams.StartAddress + i,
                    Name = $"寄存器{i + 1}",
                    Value = ""
                });
            }
            return PointsTable;
        }
    }
}

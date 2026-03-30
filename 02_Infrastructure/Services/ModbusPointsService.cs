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
        public ModbusRequestParams RequestParams {  get; set; }
        public ObservableCollection<ModbusPoints> PointsTable { get; set; }

        public ObservableCollection<ModbusPoints> GeneratePointsTable()
        {
            PointsTable = new ObservableCollection<ModbusPoints>();
            for (int i = 0; i < RequestParams.Count; i++)
            {
                PointsTable.Add(new ModbusPoints
                {
                    Index = i + 1,
                    Address = RequestParams.StartAddress + i,
                    Name = $"寄存器{i + 1}",
                    Value = 0
                });
            }
            return PointsTable;
        }
    }
}

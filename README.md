# ShopFloorMonitor
(刚起步,并非已经完成的项目)
一个基于WPF的CNC车间监控系统，支持Modbus TCP通信，实时显示设备状态。

## 功能特点
- 🖥️ 仿真实车间布局（两排设备，粗加工/精加工分区）
- 🎨 设备状态可视化（绿=运行，黄=待检，红=故障，灰=离线）
- 🔌 Modbus TCP通信，支持多设备并发
- 📊 实时数据展示与历史趋势（开发中）
- ⚙️ JSON配置车间布局，无需重新编译

## 技术栈
- WPF / .NET 8.0
- Prism (MVVM)
- MaterialDesignTheme
- Modbus TCP (NModbus)
- SQLite (Entity Framework Core)
- LiveCharts2

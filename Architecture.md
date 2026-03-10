# ShopFloorMonitor整体架构设计

## 一、解决方案整体结构

ShopFloorMonitor（解决方案）

```
│
├── 01_Core （核心层：接口、模型、公用枚举）
├── 02_Infrastructure （基础设施层：具体实现）
├── 03_Modules （模块层：按功能拆分的Prism模块）
│   ├── DeviceMonitor （设备调试模块）
│   ├── Dashboard （车间概览模块 - Canvas可视化）
│   ├── AlarmManagement （报警管理模块）
│   ├── TrendChart （趋势曲线模块）
│   └── Production （生产管理模块 - 可选）
├── 04_Shell （壳项目：主窗口、导航、样式）
└── 99_SharedResources （共享资源：样式、控件、转换器）
```

## 二、各项目详细职责

### 01_Core（核心层）

**类型**：.NET类库  
**职责**：定义整个系统公用的接口、模型、枚举，不依赖任何其他项目

```
01_Core/
│
├── Interfaces/
│   ├── IDeviceCommunication.cs （设备通信接口）
│   ├── IAlarmService.cs （报警服务接口）
│   ├── ILogService.cs （日志服务接口）
│   └── IDataPersistence.cs （数据持久化接口）
│
├── Models/
│   ├── Device.cs （设备模型）
│   ├── AlarmRecord.cs （报警记录）
│   ├── MachineStatus.cs （设备状态枚举）
│   └── ModbusConfig.cs （Modbus配置）
│
└── Events/ （Prism事件）
    ├── DeviceDataUpdatedEvent.cs
    ├── AlarmTriggeredEvent.cs
    └── DeviceSelectedEvent.cs
```

### 02_Infrastructure（基础设施层）

**类型**：.NET类库  
**职责**：实现Core里定义的接口，具体的技术实现都在这里

```
02_Infrastructure/
│
├── Communication/
│   ├── ModbusTcpCommunication.cs （Modbus TCP实现）
│   └── （预留 OPC UA 实现）
│
├── Persistence/
│   ├── AppDbContext.cs （EF Core + SQLite）
│   ├── AlarmRepository.cs （报警仓储）
│   └── DeviceConfigRepository.cs （设备配置仓储）
│
├── Services/
│   ├── AlarmService.cs （报警规则引擎）
│   ├── LogService.cs （日志实现 - Serilog）
│   └── ConfigurationService.cs （JSON配置读写）
│
└── Mock/
    └── MockDataProvider.cs （模拟数据，开发时用）
```

### 03_Modules（模块层）

每个模块都是一个独立的Prism模块，可以单独加载、卸载。

#### 1. DeviceMonitor（设备调试模块）

```
DeviceMonitor/
│
├── Views/
│   ├── DeviceMonitorView.xaml
│   └── Parts/ （拆分的局部控件）
│       ├── ConnectionPart.xaml
│       ├── DataGridPart.xaml
│       └── WriteTestPart.xaml
│
├── ViewModels/
│   ├── DeviceMonitorViewModel.cs
│   ├── ConnectionPartViewModel.cs
│   ├── DataGridPartViewModel.cs
│   └── WriteTestPartViewModel.cs
│
└── ModuleInit.cs （Prism模块初始化）
```

#### 2. Dashboard（车间概览模块）

```
Dashboard/
│
├── Views/
│   ├── DashboardView.xaml （主视图）
│   └── Controls/
│       └── MachineWidget.xaml （单个设备方块控件）
│
├── ViewModels/
│   ├── DashboardViewModel.cs
│   └── MachineWidgetViewModel.cs
│
├── Converters/
│   └── StatusToColorConverter.cs （状态转颜色）
│
└── ModuleInit.cs
```

#### 3. AlarmManagement（报警管理模块）

```
AlarmManagement/
│
├── Views/
│   ├── AlarmListView.xaml （报警列表）
│   └── AlarmConfigView.xaml （报警规则配置）
│
├── ViewModels/
│   ├── AlarmListViewModel.cs
│   └── AlarmConfigViewModel.cs
│
└── ModuleInit.cs
```

#### 4. TrendChart（趋势曲线模块）

```
TrendChart/
│
├── Views/
│   └── TrendChartView.xaml
│
├── ViewModels/
│   └── TrendChartViewModel.cs
│
└── ModuleInit.cs
```

### 04_Shell（壳项目）

**类型**：WPF应用程序  
**职责**：程序的入口，主窗口，导航菜单，全局样式

```
04_Shell/
│
├── Views/
│   ├── MainWindow.xaml （主窗口，带导航栏）
│   └── ShellWindow.xaml （如果有需要）
│
├── ViewModels/
│   └── MainWindowViewModel.cs
│
├── App.xaml
└── App.xaml.cs （注册模块、配置容器）
```

### 99_SharedResources（共享资源）

**类型**：.NET类库  
**职责**：存放全局样式、自定义控件、转换器，被各个模块引用

```
99_SharedResources/
│
├── Styles/
│   ├── Colors.xaml （颜色定义）
│   ├── Buttons.xaml （按钮样式）
│   └── DataGrid.xaml （表格样式）
│
├── Converters/
│   ├── BoolToVisibilityConverter.cs
│   └── InverseBoolConverter.cs
│
└── Controls/
    ├── IndustrialGauge.xaml （工业仪表盘控件）
    └── AlarmBanner.xaml （报警横幅）
```

## 三、依赖关系图

```
                    ┌────────────┐
                    │ 04_Shell   │
                    │ (启动项目)  │
                    └──────┬─────┘
                           │ 引用
                           ▼
              ┌─────────────────────────────────────────┐
              │   03_Modules (各模块)                   │
              │ ┌───────┬─────────┬──────────┬────────┐ │
              │ │Device │Dashboard│Alarm     │Trend   │ │
              │ │Monitor│         │Management│Chart   │ │
              │ └───────┴─────────┴──────────┴────────┘ │
              └───────────┬─────────────────────────────┘
                          │ 引用
                          ▼
              ┌────────────────────────┐
              │   02_Infrastructure    │
              │ (Modbus实现、数据库、    │
              │  服务实现)              │
              └───────────┬─────────────┘
                          │ 实现接口
                          ▼
              ┌──────────────────────────┐
              │       01_Core            │
              │ (接口、模型、事件 - 最底层)│
              └──────────────────────────┘
                           ▲
                           │ 引用
              ┌──────────────────────────┐
              │   99_SharedResources     │
              │ (样式、控件、转换器 -      │
              │  被所有项目引用)          │
              └──────────────────────────┘

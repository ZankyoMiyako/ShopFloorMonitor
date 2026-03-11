using _01_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _99_SharedResources.Controls
{
    /// <summary>
    /// MachineBlock.xaml 的交互逻辑
    /// </summary>
    public partial class MachineBlock : UserControl
    {


        public Device Device
        {
            get { return (Device)GetValue(DeviceProperty); }
            set { SetValue(DeviceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeviceProperty =
            DependencyProperty.Register(nameof(Device), typeof(Device), typeof(MachineBlock), new PropertyMetadata(null));

        public MachineBlock()
        {
            InitializeComponent();
        }

    }
}

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _04_Shell.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CloseBtn.Click += (e, s) => { this.Close(); };
            MaxBtn.Click += (e, s) => { this.WindowState = (this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized); };
            MinBtn.Click += (e, s) => { this.WindowState = WindowState.Minimized; };
        }
    }
}
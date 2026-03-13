using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Modules.DebuggerModule.ViewModels
{
    internal class TCPConnectViewModel:BindableBase
    {
        public TCPConnectViewModel()
        {
            Debug.WriteLine("启动成功");
        }
    }
}

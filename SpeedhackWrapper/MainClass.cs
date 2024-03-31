using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeedhackWrapper
{
    public class MainClass : EasyHook.IEntryPoint
    {
        public MainClass(EasyHook.RemoteHooking.IContext context) { }

        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(string path);

        public void Run(EasyHook.RemoteHooking.IContext context)
        {
            LoadLibrary(AppDomain.CurrentDomain.BaseDirectory + @"\file\dll\speedhack\x86\version.dll");
        }
    }
}

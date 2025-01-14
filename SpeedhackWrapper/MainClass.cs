using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SpeedhackWrapper
{
    public class MainClass : EasyHook.IEntryPoint
    {
        public MainClass(EasyHook.RemoteHooking.IContext context, bool isLoadFormSpeedhack) { }

        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(string path);
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool FreeLibrary(IntPtr hModule);

        private static IntPtr hSpeedhack = IntPtr.Zero;

        public void Run(EasyHook.RemoteHooking.IContext context, bool isLoadFormSpeedhack)
        {
            ResetSpeed();
            if (!isLoadFormSpeedhack) return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new FormChangeSpeed();
            form.Show();
            Application.Run();
        }

        public static void ResetSpeed()
        {
            if (hSpeedhack != IntPtr.Zero) FreeLibrary(hSpeedhack);
            hSpeedhack = LoadLibrary(AppDomain.CurrentDomain.BaseDirectory + @"\file\dll\speedhack\x64\version.dll");
        }
    }
}

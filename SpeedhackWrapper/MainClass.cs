using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SpeedhackWrapper
{
    public class MainClass : EasyHook.IEntryPoint
    {
        public MainClass(EasyHook.RemoteHooking.IContext context, bool isLoadFormSpeedhack) { }

        [DllImport("file\\dll\\speed\\x64\\WxSpeed.dll")]
        private extern static void HookInit();
        [DllImport("file\\dll\\speed\\x64\\WxSpeed.dll")]
        public extern static void SetGameSpeed(double speed);

        public const string SPEED_CONFIG_FILE = @"file\dll\speed\x64\speed.txt";

        public void Run(EasyHook.RemoteHooking.IContext context, bool isLoadFormSpeedhack)
        {
            try
            {
                double.TryParse(File.ReadAllText(SPEED_CONFIG_FILE), out double speed);
                SetGameSpeed(speed);
            }
            catch { }
            HookInit();
            if (!isLoadFormSpeedhack) return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new FormChangeSpeed();
            form.Show();
            Application.Run();
        }
    }
}

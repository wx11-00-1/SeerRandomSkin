using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketHack
{
    public partial class FormScreenShot : Form
    {
        private const int titleAndMenuHeight = 60;

        public FormScreenShot()
        {
            InitializeComponent();
        }

        private void FormScreenShot_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            MainClass.StartHook();
        }

        public void ScreenShot()
        {
            // 获取屏幕宽度和高度
            RECT_INFO rect = new RECT_INFO();
            GetWindowRect(MainClass.hGameWnd, ref rect);
            Width = (rect.Right - rect.Left) / 2;
            Height = (rect.Bottom - rect.Top) - titleAndMenuHeight;

            // 创建一个屏幕图像对象
            using (Bitmap bitmap = new Bitmap(Width, Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // 拷贝屏幕图像到位图中
                    graphics.CopyFromScreen(rect.Left + Width, rect.Top + titleAndMenuHeight, 0, 0, new System.Drawing.Size(Width, Height));

                    // 在本窗口中显示
                    pictureBox1.Dock = DockStyle.Fill;
                    pictureBox1.Image = Image.FromHbitmap(bitmap.GetHbitmap());
                }
            }
        }

        /// <summary>
        /// 获取指定窗口（或控件）在屏幕中的位置信息 （左边界，上边界，右边界，下边界）
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="lpRect">LPRECT矩形结构的长指针,数据存储使用struct类型</param>
        /// <returns>获取成功返回非0值,失败返回0</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT_INFO lpRect);

        const int WM_GETTEXT = 0x000D;
        const int WM_GETTEXTLENGTH = 0x000E;
        const int WM_CLOSE = 0x10;

        /// <summary>
        /// 矩形范围信息（结构体）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT_INFO
        {
            /// <summary>
            /// 当前矩形范围的最左边界
            /// </summary>
            public int Left;
            /// <summary>
            /// 当前矩形的最上边界
            /// </summary>
            public int Top;
            /// <summary>
            /// 当前矩形的最右边界
            /// </summary>
            public int Right;
            /// <summary>
            /// 当前矩形的最下边界
            /// </summary>
            public int Bottom;
        }
    }
}

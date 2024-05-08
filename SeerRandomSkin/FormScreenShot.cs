using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormScreenShot : Form
    {
        public Form1 MainForm = null;
        private const int titleAndMenuHeight = 60;

        public FormScreenShot()
        {
            InitializeComponent();
        }

        private void FormScreenShot_Load(object sender, EventArgs e)
        {

        }

        public void ScreenShot()
        {
            // 获取屏幕宽度和高度
            Width = MainForm.Size.Width / 2;
            Height = MainForm.Size.Height - titleAndMenuHeight;

            // 创建一个屏幕图像对象
            using (Bitmap bitmap = new Bitmap(Width, Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // 拷贝屏幕图像到位图中
                    graphics.CopyFromScreen(MainForm.Location.X + Width, MainForm.Location.Y + titleAndMenuHeight, 0, 0, new System.Drawing.Size(Width, Height));

                    // 在本窗口中显示
                    pictureBox1.Dock = DockStyle.Fill;
                    pictureBox1.Image = Image.FromHbitmap(bitmap.GetHbitmap());
                }
            }
        }
    }
}

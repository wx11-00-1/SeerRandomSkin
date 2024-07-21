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
            int screenWidth = MainForm.Size.Width / 2;
            int screenHeight = MainForm.Size.Height - titleAndMenuHeight;

            // 创建一个屏幕图像对象
            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // 拷贝屏幕图像到位图中
                    graphics.CopyFromScreen(MainForm.Location.X + screenWidth, MainForm.Location.Y + titleAndMenuHeight + 60, 0, 0, new System.Drawing.Size(screenWidth, screenHeight));

                    // 缩小图片
                    using (var smallPic = new Bitmap(bitmap, bitmap.Width / 2, bitmap.Height / 2))
                    {
                        // 在本窗口中显示
                        pictureBox1.Dock = DockStyle.Fill;
                        pictureBox1.Image = Image.FromHbitmap(smallPic.GetHbitmap());
                        // 更新窗口大小
                        Width = smallPic.Width;
                        Height = smallPic.Height;
                    }
                }
            }
        }
    }
}

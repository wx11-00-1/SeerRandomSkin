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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "作者：https://space.bilibili.com/435657414\n开源地址：https://github.com/wx11-00-1/SeerRandomSkin\n\n说明：\n1、本程序完全开源，免费分享。\n\n2、全屏快捷键 F11。\n\n3、“配置”中的皮肤黑名单：\n表示随机替换皮肤的时候要避免选中皮肤，可以是用户自行测试过会出问题的皮肤，也可以是不喜欢的精灵皮肤，设置完这一项后，要重新点击“获取皮肤数据”才能生效。\n\n4、变速\nFlash 端默认2倍速，可以通过修改speedhack.txt文件进行调整；\n在战斗过程中，遇到动画复杂的精灵，变速会失效，随机到简约的老精灵则一切正常；\n怀旧服也可以变速，但不会静音，也没有别的功能；\n变速功能只会在刚启动程序的时候开启一次，也就是说，切换到 H5 端后再切回 Flash 端就没有变速效果了。\n\n5、浏览数据保存在 cache 文件夹，请勿发送给他人。\n\n6、更多功能请在菜单栏体验！";
        }
    }
}

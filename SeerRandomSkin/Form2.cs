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
            richTextBox1.Text = "作者：https://space.bilibili.com/435657414\n开源地址：https://github.com/wx11-00-1/SeerRandomSkin\n\n说明：\n1、本程序完全开源，免费分享。\n\n2、全屏快捷键 F11。\n\n3、Flash 端可以通过点击商城，召唤小助手蒂朵，能借绿火、自动治疗哦！4、“配置”中的皮肤黑名单：\n表示随机替换时要避免选中的皮肤；\n有些皮肤确实太辣眼睛。。。还有些使用物攻技能，动画就会卡住，想用也用不了；1000序号以前的皮肤也有很多能用的，只是筛选起来太麻烦；\n随机替换上的皮肤 id 会在开发者工具的 console 中显示，便于识别；\n设置完这一项后，要重新点击“获取皮肤数据”才能生效。\n\n5、变速\nFlash 端默认2倍速，可以通过修改speedhack.txt文件进行调整；\n在战斗过程中，遇到动画复杂的精灵，可能加速效果不好，随机到简约的老精灵则正常加速；\n怀旧服也可以变速，但不会静音，也没有别的功能；\n变速功能会自动开启一次，如果到了开启的时间点程序还没初始化完成，可能会自动开启失败，需要在菜单栏，手动开启；\n从 Flash 切换到 H5，再回来的时候变速可能还在，也可能不在了，总之就看战斗倒计时有没有加速，没有再手动开变速。\n\n6、浏览数据保存在 cache 文件夹，请勿发送给他人。";
        }
    }
}

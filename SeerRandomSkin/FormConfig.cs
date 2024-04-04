using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormConfig : Form
    {
        public FormConfig()
        {
            InitializeComponent();
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            // 初始化配置项
            checkBox_RandomSkin.Checked = Properties.Settings.Default.IsRandomSkin;
            checkBox_h5_first.Checked = Properties.Settings.Default.IsH5First;
            richTextBox_SkinBlackList.Text = Properties.Settings.Default.SkinBlackList;
            comboBox_font.Text = Properties.Settings.Default.BrowserFont;
            // 遍历系统字体
            foreach(var f in System.Drawing.FontFamily.Families)
            {
                comboBox_font.Items.Add(f.Name);
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsRandomSkin = checkBox_RandomSkin.Checked;
            Properties.Settings.Default.IsH5First = checkBox_RandomSkin.Checked;
            // 检查黑名单字符串格式是否正确
            string pattern = "^[0-9,]*$";
            var m = Regex.Match(richTextBox_SkinBlackList.Text, pattern);
            if(m.Success)
            {
                Properties.Settings.Default.SkinBlackList = richTextBox_SkinBlackList.Text;
            }
            else
            {
                MessageBox.Show("皮肤黑名单格式错误");
            }
            Properties.Settings.Default.BrowserFont = comboBox_font.Text;
            Properties.Settings.Default.Save();
        }
    }
}

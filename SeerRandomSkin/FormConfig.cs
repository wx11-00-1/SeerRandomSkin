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
            numericUpDown_skinCeiling.Value = Properties.Settings.Default.SkinRangeCeiling;
            numericUpDown_skinFloor.Value = Properties.Settings.Default.SkinRangeFloor;
            richTextBox_SkinBlackList.Text = Properties.Settings.Default.SkinBlackList;
            comboBox_font.Text = Properties.Settings.Default.BrowserFont;
            numericUpDown_win_width.Value = Properties.Settings.Default.WinWidth;
            numericUpDown_win_height.Value = Properties.Settings.Default.WinHeight;
            checkBox_resource_ad_panel.Checked = Properties.Settings.Default.IsChangeAdPanel;
            checkBox_resource_background.Checked = Properties.Settings.Default.IsChangeBackground;
            checkBox_resource_vip_icon.Checked = Properties.Settings.Default.IsChangeVipIcon;
            // 遍历系统字体
            foreach (var f in FontFamily.Families)
            {
                comboBox_font.Items.Add(f.Name);
            }
            textBox1.Text = Properties.Settings.Default.AutoExecuteSoftwarePath1;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsRandomSkin = checkBox_RandomSkin.Checked;
            Properties.Settings.Default.IsH5First = checkBox_h5_first.Checked;
            Properties.Settings.Default.WinWidth = numericUpDown_win_width.Value;
            Properties.Settings.Default.WinHeight = numericUpDown_win_height.Value;
            Properties.Settings.Default.SkinRangeCeiling = (int)numericUpDown_skinCeiling.Value;
            Properties.Settings.Default.SkinRangeFloor = (int)numericUpDown_skinFloor.Value;
            Properties.Settings.Default.IsChangeAdPanel = checkBox_resource_ad_panel.Checked;
            Properties.Settings.Default.IsChangeBackground = checkBox_resource_background.Checked;
            Properties.Settings.Default.IsChangeVipIcon = checkBox_resource_vip_icon.Checked;
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
            Properties.Settings.Default.AutoExecuteSoftwarePath1 = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "可执行程序（.exe）|*.exe" };
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
            }
        }
    }
}

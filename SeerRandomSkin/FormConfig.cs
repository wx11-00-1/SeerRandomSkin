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
        private Properties.Settings SettingsDef = Properties.Settings.Default;

        public FormConfig()
        {
            InitializeComponent();
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            // 初始化配置项
            checkBox_RandomSkin.Checked = SettingsDef.IsRandomSkin;
            checkBox_h5_first.Checked = SettingsDef.IsH5First;
            numericUpDown_skinCeiling.Value = SettingsDef.SkinRangeCeiling;
            numericUpDown_skinFloor.Value = SettingsDef.SkinRangeFloor;
            richTextBox_SkinBlackList.Text = SettingsDef.SkinBlackList;
            comboBox_font.Text = SettingsDef.BrowserFont;
            numericUpDown_win_width.Value = SettingsDef.WinWidth;
            numericUpDown_win_height.Value = SettingsDef.WinHeight;
            checkBox_resource_ad_panel.Checked = SettingsDef.IsChangeAdPanel;
            checkBox_resource_background.Checked = SettingsDef.IsChangeBackground;
            checkBox_resource_vip_icon.Checked = SettingsDef.IsChangeVipIcon;
            checkBox_resource_bg_h5.Checked = SettingsDef.IsChangeH5LoginBg2024;
            // 遍历系统字体
            foreach (var f in FontFamily.Families)
            {
                comboBox_font.Items.Add(f.Name);
            }
            textBox1.Text = SettingsDef.AutoExecuteSoftwarePath1;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            SettingsDef.IsRandomSkin = checkBox_RandomSkin.Checked;
            SettingsDef.IsH5First = checkBox_h5_first.Checked;
            SettingsDef.WinWidth = numericUpDown_win_width.Value;
            SettingsDef.WinHeight = numericUpDown_win_height.Value;
            SettingsDef.SkinRangeCeiling = (int)numericUpDown_skinCeiling.Value;
            SettingsDef.SkinRangeFloor = (int)numericUpDown_skinFloor.Value;
            SettingsDef.IsChangeAdPanel = checkBox_resource_ad_panel.Checked;
            SettingsDef.IsChangeBackground = checkBox_resource_background.Checked;
            SettingsDef.IsChangeVipIcon = checkBox_resource_vip_icon.Checked;
            SettingsDef.IsChangeH5LoginBg2024 = checkBox_resource_bg_h5.Checked;
            // 检查黑名单字符串格式是否正确
            string pattern = "^[0-9,]*$";
            var m = Regex.Match(richTextBox_SkinBlackList.Text, pattern);
            if(m.Success)
            {
                SettingsDef.SkinBlackList = richTextBox_SkinBlackList.Text;
            }
            else
            {
                MessageBox.Show("皮肤黑名单格式错误");
            }
            SettingsDef.BrowserFont = comboBox_font.Text;
            SettingsDef.AutoExecuteSoftwarePath1 = textBox1.Text;
            SettingsDef.Save();
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

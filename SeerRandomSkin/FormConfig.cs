using EasyHook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
            Init();
        }

        private void Init()
        {
            checkBox_RandomSkin.Checked = SettingsDef.IsRandomSkin;
            checkBox_HideBattleStrategy.Checked = SettingsDef.IsHideBattleStrategy;
            numericUpDown_skinCeiling.Value = SettingsDef.SkinRangeCeiling;
            numericUpDown_skinFloor.Value = SettingsDef.SkinRangeFloor;
            richTextBox_skinList.Text = SettingsDef.SkinIds;
            richTextBox_SkinBlackList.Text = SettingsDef.SkinBlackList;
            richTextBox_SkinExc.Text = SettingsDef.RandomSkinExclusion;
            comboBox_font.Text = SettingsDef.BrowserFont;
            numericUpDown_win_width.Value = SettingsDef.WinWidth;
            numericUpDown_win_height.Value = SettingsDef.WinHeight;
            checkBox_flash_pack.Checked = SettingsDef.IsUseSocketHack;
            checkBox_fd.Checked = SettingsDef.AutoLoadFD;
            cbAutoMute.Checked = SettingsDef.AutoMute;
            // 遍历系统字体
            foreach (var f in FontFamily.Families)
            {
                comboBox_font.Items.Add(f.Name);
            }
            textBox1.Text = SettingsDef.AutoExecuteSoftwarePath1;
            textBox2.Text = SettingsDef.AutoExecuteSoftwarePath2;
            textBox3.Text = SettingsDef.AutoExecuteSoftwarePath3;
            textBox_defaultURL.Text = SettingsDef.DefaultURL;
            comboBoxZoom.Text = SettingsDef.FlashZoom;
            comboBoxZoom.Items.Clear();
            comboBoxZoom.Items.Add("0.5");
            comboBoxZoom.Items.Add("0.75");
            comboBoxZoom.Items.Add("1");
            comboBoxZoom.Items.Add("1.25");
            comboBoxZoom.Items.Add("1.5");
            comboBoxZoom.Items.Add("1.75");
            comboBoxZoom.Items.Add("2");
            textBox_speedUp.Text = File.ReadAllText(@"file\dll\speedhack\x64\speedhack.txt");
            // 加载窗口
            checkBox_flash_activities.Checked = SettingsDef.AutoLoadActivities;
            checkBox_flash_fight.Checked = SettingsDef.AutoLoadFightHandler;
            checkBox_h5_pack.Checked = SettingsDef.AutoLoadH5Pack;
            checkBox_h5_pet_bag.Checked = SettingsDef.AutoLoadPetBag;
            checkBox_screen_shot.Checked = SettingsDef.AutoLoadScreenShot;
            checkBox_flash_map.Checked = SettingsDef.AutoLoadFlashMap;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            // 检查字符串格式是否正确
            string pattern = "^[0-9,]*$";
            var m = Regex.Match(richTextBox_SkinBlackList.Text, pattern);
            if(m.Success)
            {
                SettingsDef.SkinBlackList = richTextBox_SkinBlackList.Text;
            }
            else
            {
                MessageBox.Show("皮肤黑名单格式错误");
                return;
            }
            m = Regex.Match(richTextBox_SkinExc.Text, pattern);
            if (m.Success)
            {
                SettingsDef.RandomSkinExclusion = richTextBox_SkinExc.Text;
            }
            else
            {
                MessageBox.Show("换肤排除项格式错误");
                return;
            }
            m = Regex.Match(richTextBox_skinList.Text, pattern);
            if (m.Success)
            {
                SettingsDef.SkinIds = richTextBox_skinList.Text;
            }
            else
            {
                MessageBox.Show("皮肤列表格式错误");
                return;
            }
            SettingsDef.IsRandomSkin = checkBox_RandomSkin.Checked;
            SettingsDef.IsHideBattleStrategy = checkBox_HideBattleStrategy.Checked;
            SettingsDef.WinWidth = numericUpDown_win_width.Value;
            SettingsDef.WinHeight = numericUpDown_win_height.Value;
            SettingsDef.SkinRangeCeiling = (int)numericUpDown_skinCeiling.Value;
            SettingsDef.SkinRangeFloor = (int)numericUpDown_skinFloor.Value;
            SettingsDef.IsUseSocketHack = checkBox_flash_pack.Checked;
            SettingsDef.BrowserFont = comboBox_font.Text;
            SettingsDef.AutoExecuteSoftwarePath1 = textBox1.Text;
            SettingsDef.AutoExecuteSoftwarePath2 = textBox2.Text;
            SettingsDef.AutoExecuteSoftwarePath3 = textBox3.Text;
            SettingsDef.DefaultURL = textBox_defaultURL.Text;
            SettingsDef.FlashZoom = comboBoxZoom.Text;
            SettingsDef.AutoMute = cbAutoMute.Checked;

            if (double.TryParse(textBox_speedUp.Text, out var speed) && speed >= 1)
            {
                File.WriteAllText(@"file\dll\speedhack\x64\speedhack.txt", textBox_speedUp.Text);
            }

            // 窗口
            SettingsDef.AutoLoadActivities = checkBox_flash_activities.Checked;
            SettingsDef.AutoLoadFightHandler = checkBox_flash_fight.Checked;
            SettingsDef.AutoLoadH5Pack = checkBox_h5_pack.Checked;
            SettingsDef.AutoLoadPetBag = checkBox_h5_pet_bag.Checked;
            SettingsDef.AutoLoadScreenShot = checkBox_screen_shot.Checked;
            SettingsDef.AutoLoadFlashMap = checkBox_flash_map.Checked;
            SettingsDef.AutoLoadFD = checkBox_fd.Checked;

            SettingsDef.Save();
            MessageBox.Show("保存成功");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "可执行程序（.exe）|*.exe" };
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "可执行程序（.exe）|*.exe" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "可执行程序（.exe）|*.exe" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = dialog.FileName;
            }
        }

        public static string GetUserConfigFilePath()
        {
            try
            {
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            }
            catch (ConfigurationErrorsException e)
            {
                return e.Filename;
            }
        }

        public static void DeleteUserConfig()
        {
            var path = GetUserConfigFilePath();
            if (Path.GetFileName(path) != "user.config")
            {
                MessageBox.Show("删除配置文件失败，请尝试读取正常的配置文件（不是正常的文件名）");
                return;
            }
            path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
            if (!path.EndsWith(new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().DeclaringType.Namespace))
            {
                MessageBox.Show("删除配置文件失败，请尝试读取正常的配置文件（找不到文件夹）");
                return;
            }
            Directory.Delete(path, true);
        }

        private void btnRemoveSettings_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("除了本窗口的配置外，还会删除用户自制的脚本、FD 替换等内容，确定继续吗？", "提示", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DeleteUserConfig();
                MessageBox.Show("恢复成功，请重新打开程序");
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            var path = GetUserConfigFilePath();
            if (!File.Exists(path))
            {
                MessageBox.Show("配置没有改动过，不需要备份啦");
                return;
            }
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.Copy(path, Path.Combine(dialog.SelectedPath, "SeerRandomSkin.config"), true);
            }
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var result = MessageBox.Show($"将会覆盖原有配置，是否继续？", "提示", MessageBoxButtons.OKCancel);
                if (result != DialogResult.OK) return;
                var path = GetUserConfigFilePath();
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.Copy(dialog.FileName, path, true);
                try
                {
                    SettingsDef.Reload();
                    // 过滤掉 FD 的文件替换部分
                    var fdObjs = Utils.TryJsonConvert<List<FiddleObject>>(SettingsDef.FiddleObjects);
                    if (fdObjs != null)
                    {
                        SettingsDef.FiddleObjects = JsonConvert.SerializeObject(fdObjs.Where(obj => obj.IsUrl).ToList());
                    }
                    SettingsDef.Save();
                    MessageBox.Show("读取成功，重新启动后生效");
                }
                catch (Exception)
                {
                    MessageBox.Show("文件格式错误，请恢复默认配置");
                }
            }
        }
    }
}

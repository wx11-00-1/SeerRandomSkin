using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormFiddler : Form
    {
        private string fiddleFilePath;

        public FormFiddler()
        {
            InitializeComponent();
        }

        private void FormFiddler_Load(object sender, EventArgs e)
        {
            RefreshListView(Form1.FiddleObjects);
        }

        private void RefreshListView(IEnumerable<FiddleObject> fiddleObjects)
        {
            listView1.Items.Clear();
            listView1.Items.AddRange(fiddleObjects.Select((o, index) =>
            {
                var item = new ListViewItem()
                {
                    Text = index.ToString()
                };
                item.SubItems.Add(o.From);
                item.SubItems.Add(o.To);
                item.SubItems.Add(o.Description);
                return item;
            }).ToArray());
        }

        private void SaveFiddleObjectsAndRefresh()
        {
            Properties.Settings.Default.FiddleObjects = JsonConvert.SerializeObject(Form1.FiddleObjects);
            Properties.Settings.Default.Save();
            RefreshListView(Form1.FiddleObjects);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool isUrl = true;
            if (tbTo.Text == Path.GetFileName(fiddleFilePath))
            {
                // 复制文件
                if (!Directory.Exists(Form1.FiddleFilePath))
                {
                    Directory.CreateDirectory(Form1.FiddleFilePath);
                }
                var destFilename = Path.Combine(Form1.FiddleFilePath, tbTo.Text);
                if (File.Exists(destFilename))
                {
                    MessageBox.Show("请不要用同一个文件替换多个URL");
                    return;
                }
                File.Copy(fiddleFilePath, destFilename);
                isUrl = false;
            }
            Form1.FiddleObjects.Add(new FiddleObject
            {
                From = tbFrom.Text,
                FromReg = new System.Text.RegularExpressions.Regex(tbFrom.Text),
                To = tbTo.Text,
                Description = tbDesc.Text,
                IsUrl = isUrl
            });
            SaveFiddleObjectsAndRefresh();
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fiddleFilePath = dialog.FileName;
                tbTo.Text = Path.GetFileName(fiddleFilePath);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            var dels = listView1.SelectedItems.Cast<ListViewItem>().Select(item =>
            {
                int.TryParse(item.Text, out int index);
                if (!Form1.FiddleObjects[index].IsUrl)
                {
                    File.Delete(Path.Combine(Form1.FiddleFilePath, Form1.FiddleObjects[index].To));
                }
                return index;
            }).ToHashSet();
            Form1.FiddleObjects = Form1.FiddleObjects.Where((value, index) => !dels.Contains(index)).ToList();
            SaveFiddleObjectsAndRefresh();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshListView(Form1.FiddleObjects.Where(obj => obj.Description.Contains(tbDesc.Text)).ToList());
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                tbFrom.Text = listView1.SelectedItems[0].SubItems[1].Text;
                tbTo.Text = listView1.SelectedItems[0].SubItems[2].Text;
                tbDesc.Text = listView1.SelectedItems[0].SubItems[3].Text;
            }
        }

        private void btnOpenFilePath_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Form1.FiddleFilePath);
        }
    }
}

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

        private void RefreshListView(List<FiddleObject> fiddleObjects)
        {
            listView1.Items.Clear();
            for (int i = 0; i < fiddleObjects.Count; ++i) {
                ListViewItem listItem = new ListViewItem()
                {
                    Text = i.ToString(),
                };
                listItem.SubItems.Add(fiddleObjects[i].From);
                listItem.SubItems.Add(fiddleObjects[i].To);
                listItem.SubItems.Add(fiddleObjects[i].Description);
                listView1.Items.Add(listItem);
            }
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
                File.Copy(fiddleFilePath, Path.Combine(Form1.FiddleFilePath, tbTo.Text), true);
                isUrl = false;
            }
            Form1.FiddleObjects.Add(new FiddleObject
            {
                From = tbFrom.Text,
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
            var dels = listView1.SelectedItems.Cast<ListViewItem>().Select(item => int.Parse(item.Text)).ToList();
            foreach (var del in dels)
            {
                if (Form1.FiddleObjects[del].IsUrl) continue;
                File.Delete(Path.Combine(Form1.FiddleFilePath, Form1.FiddleObjects[del].To));
            }
            Form1.FiddleObjects = Form1.FiddleObjects.Where((value, index) => !dels.Contains(index)).ToList();
            SaveFiddleObjectsAndRefresh();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshListView(Form1.FiddleObjects.Where(obj => obj.Description.Contains(tbDesc.Text)).ToList());
        }
    }
}

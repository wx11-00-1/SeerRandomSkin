using CefSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormActivityCollection : Form
    {
        private static JObject jObj_ac;

        public FormActivityCollection()
        {
            InitializeComponent();
        }

        private LinkedList<ListViewItem> GetAllListViewItems()
        {
            var items = new LinkedList<ListViewItem>();
            jObj_ac = Utils.TryGetJObject(Properties.Settings.Default.ActivityCollection);
            var properties = jObj_ac.Properties();
            foreach (var property in properties)
            {
                ListViewItem item = new ListViewItem
                {
                    Text = property.Name
                };
                item.SubItems.Add(property.Value.ToString());

                items.AddLast(item);
            }
            return items;
        }

        private void ReloadListView()
        {
            listView1.Items.Clear();
            listView1.Items.AddRange(GetAllListViewItems().ToArray());
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync(String.Format("WxFightHandler.Utils.ShowAppModule('{0}')", tbCode.Text));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "")
            {
                return;
            }
            jObj_ac[tbName.Text] = tbCode.Text;
            Properties.Settings.Default.ActivityCollection = jObj_ac.ToString();
            Properties.Settings.Default.Save();
            ReloadListView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            jObj_ac.Remove(tbName.Text);
            Properties.Settings.Default.ActivityCollection = jObj_ac.ToString();
            Properties.Settings.Default.Save();
            ReloadListView();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idxs = listView1.SelectedIndices;
            if (idxs.Count == 1)
            {
                tbName.Text = listView1.Items[idxs[0]].SubItems[0].Text;
                tbCode.Text = listView1.Items[idxs[0]].SubItems[1].Text;
            }
        }

        private void FormActivityCollection_Load(object sender, EventArgs e)
        {
            ReloadListView();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.Items.AddRange(GetAllListViewItems().Where(item => item.Text.Contains(tbName.Text)).ToArray());
        }

        private void tbCode_MouseClick(object sender, MouseEventArgs e)
        {
            tbCode.SelectAll();
        }

        private void btnWndTop_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
        }

        private void tbName_Click(object sender, EventArgs e)
        {
            tbName.SelectAll();
        }
    }
}

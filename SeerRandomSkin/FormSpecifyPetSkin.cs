using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormSpecifyPetSkin : Form
    {
        public FormSpecifyPetSkin()
        {
            InitializeComponent();
        }

        private void FormSpecifyPetSkin_Load(object sender, EventArgs e)
        {
            RefreshLv();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form1.SpecificPetSkins[(int)numericUpDown_from.Value] = (int)numericUpDown_to.Value;
            SaveSpecificPetSkinsAndRefresh();
        }

        private void SaveSpecificPetSkinsAndRefresh()
        {
            Properties.Settings.Default.SpecificPetSkins = JsonConvert.SerializeObject(Form1.SpecificPetSkins);
            Properties.Settings.Default.Save();
            RefreshLv();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            Form1.SpecificPetSkins.Remove((int)numericUpDown_from.Value);
            SaveSpecificPetSkinsAndRefresh();
        }

        private void RefreshLv()
        {
            var items = new LinkedList<ListViewItem>();
            foreach (var v in Form1.SpecificPetSkins)
            {
                ListViewItem listViewItem = new ListViewItem()
                {
                    Text = v.Key.ToString(),
                };
                listViewItem.SubItems.Add(v.Value.ToString());
                items.AddLast(listViewItem);
            }
            lvMap.Items.Clear();
            lvMap.Items.AddRange(items.OrderBy(item => { int.TryParse(item.Text, out int id); return id; }).ToArray());
        }

        private void lvMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            var skins = lvMap.SelectedItems;
            if (skins.Count == 1)
            {
                numericUpDown_from.Value = int.Parse(skins[0].SubItems[0].Text);
                numericUpDown_to.Value = int.Parse(skins[0].SubItems[1].Text);
            }
        }
    }
}

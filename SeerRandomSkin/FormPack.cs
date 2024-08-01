using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormPack : Form
    {
        private static HashSet<string> HideCmds = new HashSet<string>();

        public FormPack()
        {
            InitializeComponent();
        }

        private void FormPack_Load(object sender, EventArgs e)
        {
            checkBoxHideRecv.Checked = true;
            checkBoxHideSend.Checked = true;
        }

        private void checkBoxHideRecv_MouseClick(object sender, MouseEventArgs e)
        {
            Form1.chromiumBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("WxSeerUtil.HideRecv = !WxSeerUtil.HideRecv;");
        }

        private void checkBoxHideSend_MouseClick(object sender, MouseEventArgs e)
        {
            Form1.chromiumBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("WxSeerUtil.HideSend = !WxSeerUtil.HideSend;");
        }

        public void ShowRecvPack(string cmd, string pack)
        {
            if (HideCmds.Contains(cmd))
            {
                return;
            }
            var item = new ListViewItem();
            item.Text = "收";
            item.SubItems.Add(pack);
            item.SubItems.Add(cmd);
            listViewPack.Items.Add(item);
        }

        public void ShowSendPack(string cmd, string pack)
        {
            if (HideCmds.Contains(cmd))
            {
                return;
            }
            var item = new ListViewItem();
            item.Text = "发";
            item.SubItems.Add(pack);
            item.SubItems.Add(cmd);
            listViewPack.Items.Add(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listViewPack.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listViewPack.SelectedItems.Count > 0)
            {
                Clipboard.SetDataObject(listViewPack.SelectedItems[0].SubItems[1].Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string packHex = textBox1.Text;
            if (packHex.Length < 17 * 2) return;

            string[] pars = new string[(packHex.Length - 17 * 2) / 8];
            for (int i = 0; i < packHex.Length - 17 * 2; i += 8) { pars[i / 8] = Convert.ToInt32(packHex.Substring(17 * 2 + i, 8), 16).ToString(); }

            int cmd = Convert.ToInt32(packHex.Substring(10, 8), 16);
            string js = packHex.Length == 17 * 2 ?
                $"SocketConnection.send({cmd})" :
                $"SocketConnection.send({cmd},{string.Join(",", pars)})";
            for(int i = 0; i < numericUpDown1.Value; ++i)
            {
                Form1.chromiumBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(js);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HideCmds = new HashSet<string>(richTextBox_filter.Text.Split(','));
        }
    }
}

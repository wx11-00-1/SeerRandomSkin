using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketHack
{
    public partial class FormPack : Form
    {
        private const int titleAndMenuHeight = 60;

        public static Action<string, string> ActionShowPack;

        public static bool HideRecv = true;
        public static bool HideSend = true;

        public FormPack()
        {
            InitializeComponent();
        }

        private void FormScreenShot_Load(object sender, EventArgs e)
        {
            checkBoxHideRecv.Checked = HideRecv;
            checkBoxHideSend.Checked = HideSend;
            ActionShowPack += showPack; // static 修饰的方法不能操作窗口控件，需要借助委托
            MainClass.StartHook();
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

        private void showPack(string type,string pack)
        {
            var item = new ListViewItem();
            item.Text = type;
            item.SubItems.Add(pack);
            listViewPack.Items.Add(item);
        }

        private void checkBoxHideSend_Click(object sender, EventArgs e)
        {
            HideSend = !HideSend;
        }

        private void checkBoxHideRecv_Click(object sender, EventArgs e)
        {
            HideRecv = !HideRecv;
        }
    }
}

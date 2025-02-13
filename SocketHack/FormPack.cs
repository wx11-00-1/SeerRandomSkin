using Seer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketHack
{
    public partial class FormPack : Form
    {
        public static Action<string, string, string> ActionShowPack;
        public static Action<string> ActionShowMsg;

        public static bool HideRecv = true;
        public static bool HideSend = true;

        private static HashSet<string> HideCmds = new HashSet<string>();

        public FormPack()
        {
            InitializeComponent();
        }

        private void FormScreenShot_Load(object sender, EventArgs e)
        {
            checkBoxHideRecv.Checked = HideRecv;
            checkBoxHideSend.Checked = HideSend;
            ActionShowPack += showPack; // static 修饰的方法不能操作窗口控件，需要借助委托
            ActionShowMsg += ShowMsgInRtb;
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
                Clipboard.SetDataObject(listViewPack.SelectedItems[0].SubItems[2].Text);
            }
        }

        private void btnCleanPackSpace_Click(object sender, EventArgs e)
        {
            if (listViewPack.SelectedItems.Count > 0)
            {
                Clipboard.SetDataObject(listViewPack.SelectedItems[0].SubItems[2].Text.Replace(" ", ""));
            }
        }

        private void showPack(string type, string commandID, string pack)
        {
            if (HideCmds.Contains(commandID))
            {
                return;
            }
            var item = new ListViewItem();
            item.Text = type;
            item.SubItems.Add(commandID);
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

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMsgInRtb(string msg)
        {
            msg = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            if (rtbMsg.Text.Length == 0) { rtbMsg.Text = msg + "\n"; }
            else { rtbMsg.Text += msg + "\n"; }
            //自动滚动
            rtbMsg.SelectionStart = rtbMsg.Text.Length;
            rtbMsg.ScrollToCaret();
#if DEBUG
            // 文件日志
            using (var sw = new System.IO.StreamWriter("SeerLog.txt", true))
            {
                sw.WriteLine(msg);
            }
#endif
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            var pack = Misc.HexString2ByteArray(tbPackStr.Text);
            if (pack.Length == 0)
            {
                MessageBox.Show("请输入要发送的内容");
                return;
            }
            if (pack.Length != Misc.GetIntParam(pack, 0))
            {
                MessageBox.Show("封包长度有误");
                return;
            }
            if (pack.Length < 17)
            {
                MessageBox.Show("封包长度不能小于17");
                return;
            }
            btnSend.Enabled = !btnSend.Enabled;
            pack = Packet.encrypt(pack);
            for (int i = 0; i < numericUpDown1.Value; ++i)
            {
                MainClass.SendPack(Packet.ProcessingSendPacket(Packet.Socket, pack));
                await Task.Delay(500);
            }
            btnSend.Enabled = !btnSend.Enabled;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HideCmds = new HashSet<string>(richTextBox_filter.Text.Split(','));
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (listViewPack.Items.Count == 0)
            {
                MessageBox.Show("没有数据可以导出");
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV 文件|*.csv",
                Title = "保存为 CSV 文件"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = saveFileDialog.FileName;
                var sb = new StringBuilder();

                // 添加列标题
                for (int i = 0; i < listViewPack.Columns.Count; i++)
                {
                    sb.Append(listViewPack.Columns[i].Text);
                    if (i < listViewPack.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendLine();

                // 添加行数据
                foreach (ListViewItem item in listViewPack.Items)
                {
                    for (int i = 0; i < item.SubItems.Count; i++)
                    {
                        sb.Append(item.SubItems[i].Text);
                        if (i < item.SubItems.Count - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    sb.AppendLine();
                }

                // 写入文件
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("导出成功");
            }
        }
    }
}

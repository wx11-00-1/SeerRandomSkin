using Seer;
using System;
using System.Collections.Generic;
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

        private void button_fightCatch_Click(object sender, EventArgs e)
        {
            MainClass.SendPack(2601, new int[] { 300505,1 });
            MainClass.SendPack(2409, new int[] { 300505 });
        }

        private void button_fightExit_Click(object sender, EventArgs e)
        {
            MainClass.SendPack(2410, new int[] { });
        }

        private void button_fightSkill0_Click(object sender, EventArgs e)
        {
            MainClass.SendPack(2405, new int[] { 0 });
        }

        private void btnReadyToFight_Click(object sender, EventArgs e)
        {
            MainClass.SendPack(2404, new int[] { });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HideCmds = new HashSet<string>(richTextBox_filter.Text.Split(','));
        }

        private void btnSurrender_Click(object sender, EventArgs e)
        {
            MainClass.SendPack(2593, new int[] { });
        }
    }
}

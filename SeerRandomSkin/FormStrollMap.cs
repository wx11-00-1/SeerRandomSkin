using CefSharp;
using System;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormStrollMap : Form
    {
        public FormStrollMap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync(String.Format("document.Client.WxChangeMap({0})", numericUpDown1.Value));
        }

        private void FormStrollMap_Load(object sender, EventArgs e)
        {

        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxChangeMapRandom()");
        }
    }
}

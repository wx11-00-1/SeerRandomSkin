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
            Form1.chromiumBrowser.ExecuteScriptAsync(String.Format("WxSc.Util.ChangeMap({0})", numericUpDown1.Value));
        }

        private void FormStrollMap_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = Properties.Settings.Default.FavouriteMap;
        }

        private async void btnRandom_Click(object sender, EventArgs e)
        {
            int.TryParse((await Form1.chromiumBrowser.EvaluateScriptAsync("(()=>{ let maps=WxSc.Refl.Func('com.robot.core.config.xml.MapXMLInfo','getIDList');let r=maps[Math.floor(Math.random() * maps.length)];WxSc.Util.ChangeMap(r);return r; })()")).Result.ToString(), out int id);
            numericUpDown2.Value = id;
        }

        private void btnLike_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FavouriteMap = (int)numericUpDown1.Value;
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync(String.Format("WxSc.Util.ChangeMap({0})", numericUpDown2.Value));
        }
    }
}

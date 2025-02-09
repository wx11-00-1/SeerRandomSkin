using CefSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormPetFollow : Form
    {
        public FormPetFollow()
        {
            InitializeComponent();
        }

        private static readonly JObject jPets = Utils.TryGetJObject(Properties.Settings.Default.PetFollow);
        private const string KEY_PET1 = "p1";
        private const string KEY_PET2 = "p2";
        private const string KEY_ID = "id";
        private const string KEY_ABILITY_TYPE = "type";
        private const string KEY_LIGHT = "light";

        private void FormPetFollow_Load(object sender, EventArgs e)
        {
            try
            {
                numericUpDown_id1.Value = int.Parse(jPets[KEY_PET1][KEY_ID].ToString());
                switch (jPets[KEY_PET1][KEY_ABILITY_TYPE].ToString())
                {
                    case "1":
                        rbRed1.Checked = true;
                        break;
                    case "2":
                        rbBlue1.Checked = true;
                        break;
                    case "3":
                        rbYellow1.Checked = true;
                        break;
                    default:
                        rbNone1.Checked = true;
                        break;
                }
                cbLight1.Checked = bool.Parse(jPets[KEY_PET1][KEY_LIGHT].ToString());

                numericUpDown_id2.Value = int.Parse(jPets[KEY_PET2][KEY_ID].ToString());
                switch (jPets[KEY_PET2][KEY_ABILITY_TYPE].ToString())
                {
                    case "1":
                        rbRed2.Checked = true;
                        break;
                    case "2":
                        rbBlue2.Checked = true;
                        break;
                    case "3":
                        rbYellow2.Checked = true;
                        break;
                    default:
                        rbNone2.Checked = true;
                        break;
                }
                cbLight2.Checked = bool.Parse(jPets[KEY_PET2][KEY_LIGHT].ToString());
            }
            catch (Exception) { }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int ab1 = 0;
            if (rbRed1.Checked) ab1 = 10;
            else if (rbBlue1.Checked) ab1 = 20;
            else if (rbYellow1.Checked) ab1 = 32;
            int ab2 = 0;
            if (rbRed2.Checked) ab2 = 10;
            else if (rbBlue2.Checked) ab2 = 20;
            else if (rbYellow2.Checked) ab2 = 32;
            Form1.chromiumBrowser.ExecuteScriptAsync($"document.Client.WxPetFollow({numericUpDown_id1.Value},{ab1},{(cbLight1.Checked ? "true" : "false")},{numericUpDown_id2.Value},{ab2},{(cbLight2.Checked ? "true" : "false")})");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            jPets[KEY_PET1] = new JObject
            {
                { KEY_ID, numericUpDown_id1.Value },
                { KEY_ABILITY_TYPE, rbRed1.Checked ? 1 : rbBlue1.Checked ? 2 : rbYellow1.Checked ? 3 : 0 },
                { KEY_LIGHT, cbLight1.Checked }
            };
            jPets[KEY_PET2] = new JObject
            {
                { KEY_ID, numericUpDown_id2.Value },
                { KEY_ABILITY_TYPE, rbRed2.Checked ? 1 : rbBlue2.Checked ? 2 : rbYellow2.Checked ? 3 : 0 },
                { KEY_LIGHT, cbLight2.Checked }
            };
            Properties.Settings.Default.PetFollow = jPets.ToString();
            Properties.Settings.Default.Save();
        }

        private void btnScale1_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync($"document.Client.WxScale1({textBox1.Text})");
        }

        private void btnScale2_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync($"document.Client.WxScale2({textBox2.Text})");
        }
    }
}

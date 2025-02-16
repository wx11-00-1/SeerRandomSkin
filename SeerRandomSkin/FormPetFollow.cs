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
        private const string KEY_SCALE = "sc";

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
                textBox1.Text = jPets[KEY_PET1][KEY_SCALE].ToString();

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
                textBox2.Text = jPets[KEY_PET2][KEY_SCALE].ToString();
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
            WxPetFollow((int)numericUpDown_id1.Value, ab1, cbLight1.Checked, (int)numericUpDown_id2.Value, ab2, cbLight2.Checked);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            jPets[KEY_PET1] = new JObject
            {
                { KEY_ID, numericUpDown_id1.Value },
                { KEY_ABILITY_TYPE, rbRed1.Checked ? 1 : rbBlue1.Checked ? 2 : rbYellow1.Checked ? 3 : 0 },
                { KEY_LIGHT, cbLight1.Checked },
                { KEY_SCALE, textBox1.Text }
            };
            jPets[KEY_PET2] = new JObject
            {
                { KEY_ID, numericUpDown_id2.Value },
                { KEY_ABILITY_TYPE, rbRed2.Checked ? 1 : rbBlue2.Checked ? 2 : rbYellow2.Checked ? 3 : 0 },
                { KEY_LIGHT, cbLight2.Checked },
                { KEY_SCALE, textBox2.Text }
            };
            Properties.Settings.Default.PetFollow = jPets.ToString();
            Properties.Settings.Default.Save();
        }

        private void btnScale1_Click(object sender, EventArgs e)
        {
            WxScale1(textBox1.Text);
        }

        private void btnScale2_Click(object sender, EventArgs e)
        {
            WxScale2(textBox2.Text);
        }

        private static void WxPetFollow(int id1, int ab1, bool l1, int id2, int ab2, bool l2)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("(() => {const k = 'psi';WxFightHandler.Reflection.AddObj(k,'com.robot.core.info.pet.PetShowInfo');WxFightHandler.Reflection.SetObj(k,'petID'," + id1.ToString() + ");WxFightHandler.Reflection.SetObj(k,'abilityType'," + ab1.ToString() + ");WxFightHandler.Reflection.SetObj(k,'isBright'," + (l1 ? "true" : "false") + ");WxFightHandler.Reflection.SetObj(k,'otherPetId'," + id2.ToString() + ");WxFightHandler.Reflection.SetObj(k,'otherAbilityType'," + ab2.ToString() + ");WxFightHandler.Reflection.SetObj(k,'otherBright'," + (l2 ? "true" : "false") + ");" + "WxFightHandler.Reflection.Action(WxFightHandler.Const.MainManager,'actorModel.showPet',true,k);})()");
        }
        private static void WxScale1(string s)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync($"WxFightHandler.Reflection.Set(WxFightHandler.Const.MainManager,'actorModel.scaleX',{s});WxFightHandler.Reflection.Set(WxFightHandler.Const.MainManager,'actorModel.scaleY',{s})");
        }
        private static void WxScale2(string s)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync($"WxFightHandler.Reflection.Set(WxFightHandler.Const.MainManager,'actorModel.pet.scaleX',{s});WxFightHandler.Reflection.Set(WxFightHandler.Const.MainManager,'actorModel.pet.scaleY',{s})");
        }

        public static void WxPetFollow()
        {
            try
            {
                int ab1 = 0, ab2 = 0;
                switch (jPets[KEY_PET1][KEY_ABILITY_TYPE].ToString())
                {
                    case "1":
                        ab1 = 10;
                        break;
                    case "2":
                        ab1 = 20;
                        break;
                    case "3":
                        ab1 = 32;
                        break;
                }
                switch (jPets[KEY_PET2][KEY_ABILITY_TYPE].ToString())
                {
                    case "1":
                        ab2 = 10;
                        break;
                    case "2":
                        ab2 = 20;
                        break;
                    case "3":
                        ab2 = 32;
                        break;
                }
                WxPetFollow(int.Parse(jPets[KEY_PET1][KEY_ID].ToString()), ab1, bool.Parse(jPets[KEY_PET1][KEY_LIGHT].ToString()), int.Parse(jPets[KEY_PET2][KEY_ID].ToString()), ab2, bool.Parse(jPets[KEY_PET2][KEY_LIGHT].ToString()));
            }
            catch (Exception) { }
        }
        public static void WxScale()
        {
            try
            {
                WxScale1(jPets[KEY_PET1][KEY_SCALE].ToString());
                WxScale2(jPets[KEY_PET2][KEY_SCALE].ToString());
            }
            catch (Exception) { }
        }
    }
}

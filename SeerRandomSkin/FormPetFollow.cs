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

        private const string CLASS_ITEMXMLINFO = "com.robot.core.config.xml.ItemXMLInfo";
        private const string CLASS_SUITXMLINFO = "com.robot.core.config.xml.SuitXMLInfo";

        private async void FormPetFollow_Load(object sender, EventArgs e)
        {
            try
            {
                // 装扮
                var resp = await Form1.chromiumBrowser.EvaluateScriptAsync($"JSON.stringify(WxSc.Refl.Func('{CLASS_ITEMXMLINFO}','getAllCloth'))");
                if (!resp.Success) return;

                var heads = InitLvItem();
                var eyes = InitLvItem();
                var hands = InitLvItem();
                var waists = InitLvItem();
                var foots = InitLvItem();
                var mounts = InitLvItem();
                mounts.First.Value.Text = "0";
                var suits = InitLvItem();
                suits.First.Value.Text = "0";

                var clothes = JArray.Parse(resp.Result.ToString());
                foreach (var cloth in clothes)
                {
                    var i = new ListViewItem()
                    {
                        Text = cloth["Name"].ToString()
                    };
                    i.SubItems.Add(cloth["ID"].ToString());
                    switch (cloth["type"].ToString())
                    {
                        case "head":
                            heads.AddLast(i); break;
                        case "eye":
                            eyes.AddLast(i); break;
                        case "hand":
                            hands.AddLast(i); break;
                        case "waist":
                            waists.AddLast(i); break;
                        case "foot":
                            foots.AddLast(i); break;
                        default:
                            MessageBox.Show($"未知类型部件：{cloth["type"]}");
                            return;
                    }
                }

                lvHead.Items.AddRange(heads.ToArray());
                lvEye.Items.AddRange(eyes.ToArray());
                lvHand.Items.AddRange(hands.ToArray());
                lvWaist.Items.AddRange(waists.ToArray());
                lvFoot.Items.AddRange(foots.ToArray());

                resp = await Form1.chromiumBrowser.EvaluateScriptAsync($"JSON.stringify(WxSc.Refl.Func('{CLASS_ITEMXMLINFO}','getAllMount'))");
                if (!resp.Success) return;
                var ms = JArray.Parse(resp.Result.ToString());
                foreach (var m in ms)
                {
                    var i = new ListViewItem()
                    {
                        Text = m.ToString(),
                    };
                    mounts.AddLast(i);
                }
                lvMount.Items.AddRange(mounts.ToArray());

                resp = await Form1.chromiumBrowser.EvaluateScriptAsync($"JSON.stringify(WxSc.Refl.Func('{CLASS_SUITXMLINFO}','getAllSuitIds'))");
                if (!resp.Success) return;
                var ss = JArray.Parse(resp.Result.ToString());
                foreach (var s in ss)
                {
                    var i = new ListViewItem()
                    {
                        Text = s.ToString()
                    };
                    suits.AddLast(i);
                }
                lvSuit.Items.AddRange(suits.ToArray());

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

                cbDefPet.Checked = Properties.Settings.Default.PetFollow.Length != 0;
                if (Properties.Settings.Default.Suits.Length != 0)
                {
                    var a = JArray.Parse(Properties.Settings.Default.Suits).AsEnumerable().Select(i => i.ToString()).ToHashSet();
                    a.Remove("0");
                    SelectLvItem(lvHead, a);
                    SelectLvItem(lvEye, a);
                    SelectLvItem(lvHand, a);
                    SelectLvItem(lvWaist, a);
                    SelectLvItem(lvFoot, a);
                }
                if (Properties.Settings.Default.Mount.Length > 0)
                {
                    cbDefSuit.Checked = true;
                    foreach (ListViewItem i in lvMount.Items)
                    {
                        if (i.Text == Properties.Settings.Default.Mount)
                        {
                            i.Selected = true;
                            lvMount.TopItem = i;
                            break;
                        }
                    }
                    lvMount.ItemSelectionChanged += lvMount_ItemSelectionChanged;
                }

                AddLvEvent();

                cbScale.Checked = Properties.Settings.Default.ScaleKeep;

                tbPosX.Text = Properties.Settings.Default.PosX;
                tbPosY.Text = Properties.Settings.Default.PosY;
            }
            catch (Exception) { }
        }

        private static LinkedList<ListViewItem> InitLvItem()
        {
            var i = new ListViewItem()
            {
                Text = "无"
            };
            i.SubItems.Add("0");
            var result = new LinkedList<ListViewItem>();
            result.AddLast(i);
            return result;
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

        private void btnScale1_Click(object sender, EventArgs e)
        {
            WxScale1(textBox1.Text);
        }

        private void btnScale2_Click(object sender, EventArgs e)
        {
            WxScale2(textBox2.Text, tbPosX.Text, tbPosY.Text);
        }

        private static void WxPetFollow(int id1, int ab1, bool l1, int id2, int ab2, bool l2)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync($"(()=>{{const k = '_psi';WxSc.Dict.Add(k,'com.robot.core.info.pet.PetShowInfo');WxSc.Dict.Set(k,'petID',false,{id1});WxSc.Dict.Set(k,'abilityType',false,{ab1});WxSc.Dict.Set(k,'isBright',false,{(l1 ? "true" : "false")});WxSc.Dict.Set(k,'otherPetId',false,{id2});WxSc.Dict.Set(k,'otherAbilityType',false,{ab2});WxSc.Dict.Set(k,'otherBright',false,{(l2 ? "true" : "false")});WxSc.Refl.Func(WxSc.Const.MainManager,'actorModel.showPet',true,k)}})()");
        }
        private static void WxScale1(string s)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync($"WxSc.Refl.Set(WxSc.Const.MainManager,'actorModel.scaleX',false,{s});WxSc.Refl.Set(WxSc.Const.MainManager,'actorModel.scaleY',false,{s})");
        }
        private static void WxScale2(string s, string x, string y)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync($"WxSc.Refl.Set(WxSc.Const.MainManager,'actorModel.pet.scaleX',false,{s});WxSc.Refl.Set(WxSc.Const.MainManager,'actorModel.pet.scaleY',false,{s});WxSc.Dict.Add('_pos','flash.geom.Point',false,{x},false,{y});WxSc.Refl.Set(WxSc.Const.MainManager,'actorModel.pet.pos',true,'_pos')");
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
                WxScale2(jPets[KEY_PET2][KEY_SCALE].ToString(), Properties.Settings.Default.PosX, Properties.Settings.Default.PosY);
            }
            catch (Exception) { }
        }

        public static void ChangeCloth(string clothes)
        {
            if (clothes.Length == 0) return;
            Form1.chromiumBrowser.ExecuteScriptAsync($"WxSc.Util.ChangeCloth({clothes},false)");
        }
        private string GetSelectedItem(ListView l)
        {
            var s = l.SelectedItems;
            return s.Count == 1 ? s[0].SubItems[1].Text : "0";
        }
        private string GetClothesFromLv()
        {
            return $"[{GetSelectedItem(lvHead)},0,{GetSelectedItem(lvEye)},0,{GetSelectedItem(lvHand)},0,{GetSelectedItem(lvWaist)},0,{GetSelectedItem(lvFoot)},0]";
        }
        private void LvEvent(object s, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) return;
            ChangeCloth(GetClothesFromLv());
        }
        private void AddLvEvent()
        {
            lvHead.ItemSelectionChanged += LvEvent;
            lvEye.ItemSelectionChanged += LvEvent;
            lvHand.ItemSelectionChanged += LvEvent;
            lvWaist.ItemSelectionChanged += LvEvent;
            lvFoot.ItemSelectionChanged += LvEvent;
        }
        private void RmLvEvent()
        {
            lvHead.ItemSelectionChanged -= LvEvent;
            lvEye.ItemSelectionChanged -= LvEvent;
            lvHand.ItemSelectionChanged -= LvEvent;
            lvWaist.ItemSelectionChanged -= LvEvent;
            lvFoot.ItemSelectionChanged -= LvEvent;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchLv(lvHead, tbSearch.Text);
            SearchLv(lvEye, tbSearch.Text);
            SearchLv(lvHand, tbSearch.Text);
            SearchLv(lvWaist, tbSearch.Text);
            SearchLv(lvFoot, tbSearch.Text);
            SearchLv(lvMount, tbSearch.Text);
            SearchLv(lvSuit, tbSearch.Text);
        }
        private async void SearchLv(ListView l,string s)
        {
            foreach (ListViewItem i in l.Items)
            {
                if (i.Text.Contains(s))
                {
                    l.TopItem = i;
                    i.BackColor = Color.Aquamarine;
                    await Task.Delay(5000);
                    i.BackColor = Color.White;
                    return;
                }
            }
        }

        private async void lvSuit_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) return;
            try
            {
                var id = (sender as ListView).SelectedItems[0].Text;
                Form1.chromiumBrowser.ExecuteScriptAsync($"WxSc.Util.SimpleAlarm(WxSc.Refl.Func('{CLASS_SUITXMLINFO}','getName',false,{id}))");
                var r = await Form1.chromiumBrowser.EvaluateScriptAsync($"JSON.stringify(WxSc.Refl.Func('{CLASS_SUITXMLINFO}','getCloths',false,{id}))");
                if (!r.Success)
                {
                    MessageBox.Show("获取套装部件数组失败");
                    return;
                }
                var json = r.Result.ToString();
                Form1.chromiumBrowser.ExecuteScriptAsync($"((cs)=>{{let a=[];for(let c of cs){{a.push(c);a.push(0)}}WxSc.Util.ChangeCloth(a,false)}})({json})");
                var a = JArray.Parse(json).AsEnumerable().Select(i => i.ToString()).ToHashSet();
                RmLvEvent();
                SelectLvItem(lvHead, a);
                SelectLvItem(lvEye, a);
                SelectLvItem(lvHand, a);
                SelectLvItem(lvWaist, a);
                SelectLvItem(lvFoot, a);
                ChangeCloth(GetClothesFromLv());
                AddLvEvent();
            }
            catch (Exception) { }
        }
        private static void SelectLvItem(ListView l,HashSet<string> s)
        {
            foreach(ListViewItem i in l.Items)
            {
                if (s.Contains(i.SubItems[1].Text))
                {
                    l.TopItem = i;
                    i.Selected = true;
                    return;
                }
            }
            l.Items[0].Selected = true;
            l.EnsureVisible(0);
        }

        private void btnTrans_Click(object sender, EventArgs e)
        {
            try
            {
                var id = lvSuit.SelectedItems[0].Text;
                Form1.chromiumBrowser.ExecuteScriptAsync($"if(WxSc.Refl.Func('{CLASS_SUITXMLINFO}','getIsTransform',false,{id})){{let a=[];for(let c of WxSc.Refl.Func('{CLASS_SUITXMLINFO}','getCloths',false,{id})){{a.push(c);a.push(0)}}WxSc.Util.ChangeCloth(a,false);WxSc.Refl.Set(WxSc.Const.MainManager,'actorModel.info.changeShape',{id},false);WxSc.Dict.Add('tr','com.robot.core.skeleton.TransformSkeleton');WxSc.Refl.Set(WxSc.Const.MainManager,'actorModel.skeleton','tr',true)}}else WxSc.Util.SimpleAlarm('无法变形')");
            }
            catch (Exception) { }
        }

        private void lvMount_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                var id = lvMount.SelectedItems[0].Text;
                ShowMount(id);
            }
            catch (Exception) { }
        }
        public static void ShowMount(string id)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync($"WxSc.Refl.Set(WxSc.Const.MainManager,'actorModel.info.vip',1,false);WxSc.Refl.Func(WxSc.Const.MainManager,'actorModel.showMount',false,{id})");
        }

        private void cbDefSuit_MouseUp(object sender, MouseEventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                Properties.Settings.Default.Suits = GetClothesFromLv();
                Properties.Settings.Default.Mount = lvMount.SelectedItems.Count == 1 ? lvMount.SelectedItems[0].Text : "0";
            }
            else
            {
                Properties.Settings.Default.Mount = String.Empty;
            }
            Properties.Settings.Default.Save();
        }

        private void cbDefPet_MouseUp(object sender, MouseEventArgs e)
        {
            if ((sender as CheckBox).Checked)
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
            }
            else
            {
                Properties.Settings.Default.PetFollow = String.Empty;
            }
            Properties.Settings.Default.Save();
        }
     
        private void cbScale_MouseUp(object sender, MouseEventArgs e)
        {
            Properties.Settings.Default.ScaleKeep = cbScale.Checked;
            Properties.Settings.Default.PosX = tbPosX.Text;
            Properties.Settings.Default.PosY = tbPosY.Text;
            Properties.Settings.Default.Save();
        }
        public static void ScaleKeep()
        {
            if (Properties.Settings.Default.ScaleKeep)
            {
                Form1.chromiumBrowser.ExecuteScriptAsync($"(()=>{{WxSc.Dict.AddCall('_scale','_k',()=>{{setTimeout(()=>{{seerRandomSkinObj.petScale()}},2000)}});WxSc.Refl.Func('org.taomee.manager.EventManager','addEventListener',false,'createdMapUser',true,'_scale');}})()");
            }
        }
    }
}

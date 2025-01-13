using CefSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormFlashFightHandler : Form
    {
        public const string JS_FIGHT_ENVIRONMENT =
            "WxFightHandler = {};" +

            "WxFightHandler.Utils = {};" +

            "WxFightHandler.Private = {};" +

            "WxFightHandler.Utils.GetBagPetInfos = () => {" +
            "   return document.Client.WxGetBagPetInfos();" +
            "};" +
            "WxFightHandler.Utils.GetBag1 = () => {" +
            "   return document.Client.WxGetBag1();" +
            "};" +
            "WxFightHandler.Utils.GetBag2 = () => {" +
            "   return document.Client.WxGetBag2();" +
            "};" +
            "WxFightHandler.Utils.SetPetBag = (bag1,bag2=[]) => new Promise((resolve) => {" +
            "   WxFightHandler.Utils._as3Callback = resolve;" +
            "   document.Client.WxSetPetBag(bag1,bag2);" +
            "});" +

            "WxFightHandler.Utils.GetStoragePets = async () => await new Promise((resolve) => {" +
            "   WxFightHandler.Utils._as3Callback = resolve;" +
            "   document.Client.WxGetStoragePets();" +
            "});" +

            "WxFightHandler.Utils.GetClothes = () => {" +
            "   return document.Client.WxGetClothes();" +
            "};" +
            "WxFightHandler.Utils.ChangeCloth = (clothes) => {" +
            "   return document.Client.WxChangeCloth(clothes);" +
            "};" +

            "WxFightHandler.Utils.GetTitle = () => {" +
            "   return document.Client.WxGetTitle();" +
            "};" +
            "WxFightHandler.Utils.SetTitle = (title) => {" +
            "   return document.Client.WxSetTitle(title);" +
            "};" +

            "WxFightHandler.Utils.CopyFire = (fireType) => { document.Client.WxCopyFire(fireType); };" +

            "WxFightHandler.Utils.RoundReset = () => { WxFightHandler.Private.Round = 0; };" +
            "WxFightHandler.Utils.ShowRound = (hpPercent) => { WxFightHandler.Private.Round += 1; seerRandomSkinObj.showFightInfo(WxFightHandler.Private.Round,hpPercent); };" +

            "WxFightHandler.Utils.UseSkill = (skillID) => {" +
            "   document.Client.WxUseSkill(skillID);" +
            "};" +

            "WxFightHandler.Utils.ChangePet = (petCatchTime) => {" +
            "   document.Client.WxChangePet(petCatchTime);" +
            "};" +

            "WxFightHandler.Utils.UsePetItem = (itemID) => {" +
            "   document.Client.WxUsePetItem(itemID);" +
            "};" +

            "WxFightHandler.Utils.UsePetItem10PP = () => {" +
            "   WxFightHandler.Utils.ItemBuy(300017);" +
            "   WxFightHandler.Utils.UsePetItem(300017);" +
            "};" +

            "WxFightHandler.Utils.ItemBuy = (itemID) => {" +
            "   document.Client.WxItemBuy(itemID);" +
            "};" +

            "WxFightHandler.Utils.StopAutoFight = () => {" +
            "   WxFightHandler.OnFirstRound=()=>{};" +
            "   WxFightHandler.OnUseSkill=()=>{};" +
            "   WxFightHandler.OnChangePet=()=>{};" +
            "   WxFightHandler.OnFightOver=()=>{};" +
            "};" +

            "WxFightHandler.Utils.GetFightingPetID = () => { return document.Client.WxGetFightingPetID(); };" +
            "WxFightHandler.Utils.GetFightingPetCatchTime = () => { return document.Client.WxGetFightingPetCatchTime(); };" +
            "WxFightHandler.Utils.GetBagPetIDByCatchTime = (ct) => { return document.Client.WxGetBagPetIDByCatchTime(ct); };" +

            "WxFightHandler.Utils.ChangePetByID = (fightInfo,idArray) => {" +
            // 标记主动切换
            // 1、刚切换上一只精灵，马上又切换另一只精灵（fightInfo 的类型为 ChangePetInfo）
            // 2、在场精灵仍然存活，切换上另一只精灵（fightInfo 的类型为 AttackValue）
            "   if ('petID' in fightInfo || fightInfo.remainHP > 0) { document.Client.WxSetIsPositiveChangePet(); }" +
            "   let petArray = fightInfo.changehps;" +
            "   for (let pet of petArray) {" +
            "       if (pet.hp > 0 && -1 !== idArray.indexOf(WxFightHandler.Utils.GetBagPetIDByCatchTime(pet.id)) && pet.id !== WxFightHandler.Utils.GetFightingPetCatchTime()) {" +
            "           WxFightHandler.Utils.ChangePet(pet.id); return;" +
            "       }" +
            "   }" +
            // 没有这些 ID 的精灵，随便上一个
            "   for (let pet of petArray) {" +
            "       if (pet.hp > 0 && pet.id !== WxFightHandler.Utils.GetFightingPetCatchTime()) {" +
            "           WxFightHandler.Utils.ChangePet(pet.id); return;" +
            "       }" +
            "   }" +
            "};" +

            // 延迟函数
            "WxFightHandler.Utils.Delay = async (millisecond) => {" +
            "   return new Promise((resolve) => { setTimeout(() => { resolve(); },millisecond); });" +
            "};" +

            // 发包 并接收返回值
            "WxFightHandler.Utils.Send = (commandID, ...args) => {" +
            "   document.Client.WxSend(commandID, ...args);" +
            "};" +
            "WxFightHandler.Utils.SendAsync = async (commandID,parameterArray) => {" +
            "   return await new Promise((resolve) => {" +
            "       WxFightHandler.Utils._as3Callback = resolve;" +
            "       document.Client.WxSendWithCallback2(commandID,parameterArray);" +
            "   });" +
            "};" +

            // xml
            "WxFightHandler.Utils.GetItemNameByID = (itemID) => {" +
            "   return document.Client.WxGetItemNameByID(itemID);" +
            "};" +
            "WxFightHandler.Utils.GetAllCloth = () => {" +
            "   return document.Client.WxGetAllCloth();" +
            "};" +
            "WxFightHandler.Utils.GetPetNameByID = (petID) => {" +
            "   return document.Client.WxGetPetNameByID(petID);" +
            "};" +
            "WxFightHandler.Utils.GetSkillNameByID = (skillID) => {" +
            "   return document.Client.WxGetSkillNameByID(skillID);" +
            "};"
            ;

        public const string JS_FIGHT_DEFAULT =
            "WxFightHandler.OnFirstRound = (fightStartInfo) => {\r\n" +
            "  \r\n" +
            "};\r\n\r\n" +

            "WxFightHandler.OnUseSkill = (mySkillInfo,enemySkillInfo) => {\r\n" +
            "  if (mySkillInfo.remainHP !== 0) {\r\n" +
            "    \r\n" +
            "  }\r\n" +
            "  else {\r\n" +
            "    \r\n" +
            "  }\r\n" +
            "};\r\n\r\n" +

            "WxFightHandler.OnChangePet = (petInfo) => {\r\n" +
            "  \r\n" +
            "};\r\n\r\n" +

            "WxFightHandler.OnFightOver = (fightOverInfo) => {\r\n" +
            "  \r\n" +
            "};"
            ;

        private static JObject jFightTemplate = Utils.TryGetJObject(Properties.Settings.Default.FlashFightTemplate);

        public FormFlashFightHandler()
        {
            InitializeComponent();
        }

        private void FormFlashFightHandler_Load(object sender, EventArgs e)
        {
            richTextBox_script.Text = JS_FIGHT_DEFAULT;
            ResetLvTemplate();
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            Form1.chromiumBrowser.ExecuteScriptAsync(richTextBox_script.Text);
            await Task.Delay(1000);
            btnTest.Enabled = true;
        }

        private void lvTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var indices = lvTemplate.SelectedIndices;
            if (indices.Count == 1)
            {
                var it = lvTemplate.Items[indices[0]];
                tbTemplateName.Text = it.SubItems[0].Text;
                richTextBox_script.Text = it.SubItems[1].Text;
            }
        }

        private LinkedList<ListViewItem> GetAllListViewItems()
        {
            var items = new LinkedList<ListViewItem>();
            var properties = jFightTemplate.Properties();
            foreach (var property in properties)
            {
                var item = new ListViewItem()
                {
                    Text = property.Name,
                };
                item.SubItems.Add(property.Value.ToString());
                items.AddLast(item);
            }
            return items;
        }

        private void ResetLvTemplate()
        {
            lvTemplate.Items.Clear();
            lvTemplate.Items.AddRange(GetAllListViewItems().ToArray());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbTemplateName.Text.Length == 0)
            {
                return;
            }
            jFightTemplate[tbTemplateName.Text] = richTextBox_script.Text;
            SaveConfigTemplate();
        }

        private void SaveConfigTemplate()
        {
            Properties.Settings.Default.FlashFightTemplate = jFightTemplate.ToString();
            Properties.Settings.Default.Save();
            ResetLvTemplate();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            jFightTemplate.Remove(tbTemplateName.Text);
            SaveConfigTemplate();
        }

        public static void StopAutoFight()
        {
            Form1.chromiumBrowser.ExecuteScriptAsync(JS_FIGHT_DEFAULT);
        }

        public static bool SetFightTemplate(string name)
        {
            if (!jFightTemplate.ContainsKey(name))
            {
                return false;
            }
            Form1.chromiumBrowser.ExecuteScriptAsync(jFightTemplate[name].ToString());
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StopAutoFight();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lvTemplate.Items.Clear();
            lvTemplate.Items.AddRange(GetAllListViewItems().Where(item => item.Text.Contains(tbTemplateName.Text)).ToArray());
        }

        private void btnCure20_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxCurePet20HP();");
        }

        private void btnCureAll_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxCurePetAll();");
        }

        private void btnItem170_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxUsePetItem(300749);");
        }

        private void btnItem150_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxUsePetItem(300701);");
        }

        private void btnItem10pp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxItemBuy(300017);");
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxUsePetItem(300017);");
        }

        private void btn20pp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxUsePetItem(300018);");
        }

        private void btnItem250hp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxUsePetItem(300079);");
        }

        private void btnItem200hp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxUsePetItem(300157);");
        }

        private void btnFightPanelHide_Click(object sender, EventArgs e)
        {
            Form1.IsHideFlashFightPanel = true;
        }

        private void btnFightPanelShow_Click(object sender, EventArgs e)
        {
            Form1.IsHideFlashFightPanel = false;
        }

        private void btnAutoCureOpen_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxAutoCureStart();");
        }

        private void btnAutoCureStop_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxAutoCureStop();");
        }
    }
}

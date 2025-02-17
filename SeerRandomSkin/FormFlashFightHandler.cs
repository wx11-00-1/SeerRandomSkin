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
@"
WxFightHandler = {};
WxFightHandler.Private = {};
WxFightHandler.Utils = {};

WxFightHandler.Reflection = {};
WxFightHandler.Reflection.Get = (className,k) => document.Client.WxReflGet(className,k);
WxFightHandler.Reflection.Set = (className,k,v) => document.Client.WxReflSet(className,k,v);
WxFightHandler.Reflection.Action = (className,methodName,...args) => document.Client.WxReflAction(className,methodName,...args);
WxFightHandler.Reflection.Func = (className,methodName,...args) => document.Client.WxReflFunc(className,methodName,...args);
WxFightHandler.Reflection.AddObj = (key,className,...args) => document.Client.WxAddObj(key,className,...args);
WxFightHandler.Reflection.SetObj = (k,a,v,u) => document.Client.WxSetObj(k,a,v,u);
WxFightHandler.Reflection.ObjAction = (k,m,...args) => document.Client.WxObjAction(k,m,...args);
WxFightHandler.Reflection.ObjFunc = (k,m,...args) => document.Client.WxObjFunc(k,m,...args);

WxFightHandler.Const = {};
WxFightHandler.Const.StateKey = 'LanBaiState';
WxFightHandler.Const.DelayMs = 200;
WxFightHandler.Const.Pet = 'com.robot.core.pet.Pet';
WxFightHandler.Const.PetManager = 'com.robot.core.manager.PetManager';
WxFightHandler.Const.MainManager = 'com.robot.core.manager.MainManager';
WxFightHandler.Const.SocketConnection = 'com.robot.core.net.SocketConnection';

WxFightHandler.Utils.GetBagPetInfos = () => WxFightHandler.Reflection.Get(WxFightHandler.Const.PetManager,'allInfos');
WxFightHandler.Utils.GetBag1 = () => WxFightHandler.Reflection.Func(WxFightHandler.Const.PetManager,'getBagMap');
WxFightHandler.Utils.GetBag2 = () => WxFightHandler.Reflection.Func(WxFightHandler.Const.PetManager,'getSecondBagMap');

WxFightHandler.Private.ClearBagAsync = () => new Promise(res => { WxFightHandler.Private._as3Callback = res; document.Client.WxClearBag(); });
WxFightHandler.Private.SetBag1Async = bag1 => new Promise(res => { WxFightHandler.Private._as3Callback = res; document.Client.WxSetBag1(bag1); });
WxFightHandler.Private.SetBag2Async = bag2 => new Promise(res => { WxFightHandler.Private._as3Callback = res; document.Client.WxSetBag2(bag2); });
WxFightHandler.Utils.SetPetBagAsync = async (bag1, bag2 = []) => {
  await WxFightHandler.Private.ClearBagAsync();
  await WxFightHandler.Private.SetBag1Async(bag1);
  await WxFightHandler.Private.SetBag2Async(bag2);
  await WxFightHandler.Utils.DelayAsync(1000);
};

WxFightHandler.Utils.GetStoragePetsAsync = () => new Promise(resolve => {
  WxFightHandler.Private._as3Callback = resolve;
  document.Client.WxGetStoragePets();
});

WxFightHandler.Utils.GetClothes = () => {
  const cs = WxFightHandler.Reflection.Get(WxFightHandler.Const.MainManager,'actorInfo.clothes');
  let result = [];
  for (let c of cs) {
    result.push(c.id);
    result.push(c.level);
  }
  return result;
}
WxFightHandler.Utils.ChangeCloth = (clothes,isNet=true) => {
  const k1 = 'cl', k2 = 'it', k3 = 'be';
  WxFightHandler.Reflection.AddObj(k1,'Array');
  for (let i=0; i<clothes.length; i+=2) {
    WxFightHandler.Reflection.AddObj(k2,'com.robot.core.info.clothInfo.PeopleItemInfo',false,clothes[i],false,clothes[i+1]);
    WxFightHandler.Reflection.ObjAction(k1,'push',true,k2);
  }
  WxFightHandler.Reflection.AddObj(k3,'com.robot.core.behavior.ChangeClothBehavior',true,k1,false,isNet);
  WxFightHandler.Reflection.Action(WxFightHandler.Const.MainManager,'actorModel.execBehavior',true,k3);
}

WxFightHandler.Utils.GetTitle = () => WxFightHandler.Reflection.Get(WxFightHandler.Const.MainManager,'actorInfo.curTitle');
WxFightHandler.Utils.SetTitle = title => document.Client.WxSetTitle(title);

WxFightHandler.Private.RoundReset = () => WxFightHandler.Private.Round = 0;
WxFightHandler.Utils.GetRound = () => WxFightHandler.Private.Round;

WxFightHandler.Private.ShowRound = (hp1,hp2) => { WxFightHandler.Private.Round += 1; seerRandomSkinObj.showFightInfo(hp1,WxFightHandler.Private.Round,hp2); };

WxFightHandler.Utils.UseSkill = skillID => WxFightHandler.Utils.Send(2405,skillID);
WxFightHandler.Utils.ChangePet = petCatchTime => WxFightHandler.Reflection.Action(WxFightHandler.Const.SocketConnection,'WxChangePet',false,petCatchTime);
WxFightHandler.Utils.UsePetItem = itemID => document.Client.WxUsePetItem(itemID);
WxFightHandler.Utils.UsePetItem10PP = () => {
  WxFightHandler.Utils.ItemBuy(300017);
  WxFightHandler.Utils.UsePetItem(300017);
};
WxFightHandler.Utils.ItemBuy = (itemID,c=1) => WxFightHandler.Utils.Send(2601,itemID,c);

WxFightHandler.Utils.StopAutoFight = () => { WxFightHandler.OnFirstRound = WxFightHandler.OnUseSkill = WxFightHandler.OnChangePet = WxFightHandler.OnFightOver = () => {}; };

WxFightHandler.Utils.GetFightingPetID = () => WxFightHandler.Reflection.Get(WxFightHandler.Const.SocketConnection,'WxFightingPetID');
WxFightHandler.Utils.GetFightingPetCatchTime = () => WxFightHandler.Reflection.Get(WxFightHandler.Const.SocketConnection,'WxFightingPetCatchTime');
WxFightHandler.Utils.GetFightingPets = () => WxFightHandler.Reflection.Get(WxFightHandler.Const.SocketConnection,'WxFightingPets');
WxFightHandler.Utils.ChangePetByID = ids => document.Client.WxChangePetByID(ids);

WxFightHandler.Utils.DelayAsync = ms => new Promise(resolve => setTimeout(resolve, ms));

WxFightHandler.Utils.Send = (commandID, ...args) => {
  let ps = [];
  for (let arg of args) {
    ps.push(false); ps.push(arg);
  }
  WxFightHandler.Reflection.Action(WxFightHandler.Const.SocketConnection,'send',false,commandID, ...ps);
}
WxFightHandler.Utils.SendAsync = (commandID, parameterArray) => new Promise(resolve => {
  WxFightHandler.Private._as3Callback = resolve;
  document.Client.WxSendWithCallback2(commandID, parameterArray);
});

WxFightHandler.Utils.GetPetNameByID = petID => WxFightHandler.Reflection.Func('com.robot.core.config.xml.PetXMLInfo','getName',false,petID);
WxFightHandler.Utils.GetSkillNameByID = skillID => WxFightHandler.Reflection.Func('com.robot.core.config.xml.SkillXMLInfo','getName',false,skillID);

WxFightHandler.Utils.AutoFight = id => document.Client.WxAutoFight(id);

WxFightHandler.Utils.SetIsHidePetFight = h => WxFightHandler.Reflection.Set('com.robot.app.fight.FightManager','petFightClass',(h ? 'PetFightDLL' : 'PetFightDLL_201308'));
WxFightHandler.Utils.SetIsAutoCure = cure => WxFightHandler.Reflection.Set(WxFightHandler.Const.SocketConnection,'WxIsAutoCure',cure);
WxFightHandler.Utils.CurePet20HP = () => {
  WxFightHandler.Utils.ItemBuy(300011,6);
  WxFightHandler.Utils.ItemBuy(300017,6);
  for (let p of WxFightHandler.Utils.GetBag1()) {
    WxFightHandler.Utils.Send(2326,p.catchTime,300011);
    WxFightHandler.Utils.Send(2326,p.catchTime,300017);
  }
}
WxFightHandler.Utils.CurePetAll = () => WxFightHandler.Reflection.Action(WxFightHandler.Const.SocketConnection,'WxCurePetAll');
WxFightHandler.Utils.LowHP = () => {
  const c = ((new Date()).getUTCHours() + 8) % 24;
  WxFightHandler.Utils.Send(41129,(c < 12 || c >= 15) ? 8692 : 8694);
}
WxFightHandler.Utils.SimpleAlarm = msg => WxFightHandler.Reflection.Action('com.robot.core.ui.alert.SimpleAlarm','show',false,msg);

WxFightHandler.Utils.CopyFireAsync = async (fireType = null) => {
  // 从地图上借
  if (await new Promise((resolve) => {
    WxFightHandler.Private._as3Callback = resolve;
    document.Client.WxCopyFireFromMap(fireType);
  })) {
    WxFightHandler.Utils.SimpleAlarm('借火成功');
    return true;
  }

  // 向图鉴-新增排行榜上的活跃玩家借
  let dateToYYYYMMDDInt = date => {
    let year = date.getFullYear(); // 获取年份
    let month = date.getMonth() + 1; // 获取月份，加1是因为月份从0开始
    let day = date.getDate(); // 获取日期
    return year * 10000 + month * 100 + day; // 将年、月、日组合成YYYYMMDD格式的整数
  }

  let date = new Date();
  let day = date.getDay();
  let offset = 0;
  switch (day) {
    case 1:
      offset = 3;
      break;
    case 2:
      offset = 4;
      break;
    case 3:
      offset = 5;
      break;
    case 4:
      offset = 6;
      break;
    case 6:
      offset = 1;
      break;
    case 0:
      offset = 2;
      break;
  }
  date.setDate(date.getDate() - offset);

  let key = 157; // 图鉴-新增排行榜
  let sub_key = dateToYYYYMMDDInt(date);
  // console.log(sub_key);
  let len = await new Promise(resolve => {
    WxFightHandler.Private._as3Callback = resolve;
    document.Client.WxGetRankListLen(key, sub_key);
  });
  len -= 100;
  for (let i = 0; i <= len; i += 100) {
    WxFightHandler.Utils.SimpleAlarm(`${i}/${len}`);
    if (await new Promise(resolve => {
      WxFightHandler.Private._as3Callback = resolve;
      document.Client.WxCopyFireFromRank(key, sub_key, i, fireType);
    })) {
      WxFightHandler.Utils.SimpleAlarm('借火成功');
      return true;
    }
  }
  WxFightHandler.Utils.SimpleAlarm('借火失败');
  return false;
};

WxFightHandler.Utils.GetActivityValueAsync = (name,key) => new Promise(resolve => {
  WxFightHandler.Private._as3Callback = resolve;
  document.Client.WxGetActivityValue(name,key); 
});

WxFightHandler.Utils.ChangeMap = id => WxFightHandler.Reflection.Action('com.robot.core.manager.MapManager','changeMap',false,id);
WxFightHandler.Utils.ShowAppModule = id => WxFightHandler.Reflection.Action('com.robot.core.manager.ModuleManager','showAppModule',false,id);

WxFightHandler.Utils.StateSave = k => {
  let s = {};
  s.clothes = WxFightHandler.Utils.GetClothes();
  s.title = WxFightHandler.Utils.GetTitle();
  s.bag1 = WxFightHandler.Utils.GetBag1().map(pet => pet.catchTime);
  s.bag2 = WxFightHandler.Utils.GetBag2().map(pet => pet.catchTime);
  let st = {};
  if (localStorage.getItem(WxFightHandler.Const.StateKey)!=null) st = JSON.parse(localStorage.getItem(WxFightHandler.Const.StateKey));
  st[k] = s;
  localStorage.setItem(WxFightHandler.Const.StateKey, JSON.stringify(st));
  WxFightHandler.Utils.SimpleAlarm('ok');
}
WxFightHandler.Utils.StateLoadAsync = async k => {
  try {
    let s = JSON.parse(localStorage.getItem(WxFightHandler.Const.StateKey))[k];
    WxFightHandler.Utils.ChangeCloth(s.clothes);
    WxFightHandler.Utils.SetTitle(s.title);
    await WxFightHandler.Utils.SetPetBagAsync(s.bag1, s.bag2);
    WxFightHandler.Utils.SimpleAlarm('ok');
  } catch {alert(`${k} 不存在`)}
}

";
        public const string JS_FIGHT_DEFAULT =
            "WxFightHandler.OnFirstRound = (fightStartInfo) => {\r\n" +
            "  \r\n" +
            "};\r\n\r\n" +

            "WxFightHandler.OnUseSkill = (mySkillInfo,enemySkillInfo,isMeFirst) => {\r\n" +
            "  if (mySkillInfo.remainHP != 0) {\r\n" +
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

        private static readonly JObject jFightTemplate = Utils.TryGetJObject(Properties.Settings.Default.FlashFightTemplate);

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
            if (lvTemplate.SelectedItems.Count != 1) return;
            var item = lvTemplate.SelectedItems[0];
            tbTemplateName.Text = item.Text;
            richTextBox_script.Text = jFightTemplate[item.Text].ToString();
        }

        private IEnumerable<ListViewItem> GetAllListViewItems()
        {
            // 取所有 key
            return jFightTemplate.AsJEnumerable().Select(item => new ListViewItem(item.Path));
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
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.CurePet20HP();");
        }

        private void btnCureAll_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.CurePetAll();");
        }

        private void btnItem170_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.UsePetItem(300749);");
        }

        private void btnItem150_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.UsePetItem(300701);");
        }

        private void btnItem10pp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.UsePetItem10PP();");
        }

        private void btn20pp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.UsePetItem(300018);");
        }

        private void btnItem250hp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.UsePetItem(300079);");
        }

        private void btnItem200hp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.UsePetItem(300157);");
        }

        private void btnFightPanelHide_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.SetIsHidePetFight(true);");
        }

        private void btnFightPanelShow_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.SetIsHidePetFight(false);");
        }

        private void btnAutoCureOpen_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.SetIsAutoCure(true);");
        }

        private void btnAutoCureStop_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxFightHandler.Utils.SetIsAutoCure(false);");
        }
    }
}

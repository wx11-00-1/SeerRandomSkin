using CefSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormFlashFightHandler : Form
    {
        public const string JS_FIGHT_ENVIRONMENT =
@"
WxSc = {}
WxSc.Priv = {}
WxSc.Util = {}

WxSc.Priv._cure = true;
WxSc.Priv.ShowRound = (hp1,hp2) => { WxSc.Priv.Round += 1; seerRandomSkinObj.showFightInfo(hp1,WxSc.Priv.Round,hp2); };

WxSc.Refl = {}
WxSc.Refl.Set = (name,a,u,v) => document.Client.WxRefl(1,name,a,u,v);
WxSc.Refl.Get = (name,a) => document.Client.WxRefl(2,name,a);
WxSc.Refl.Func = (name,method,...args) => document.Client.WxRefl(3,name,method,...args);
WxSc.Refl.Tmp = (name,method,key,...args) => document.Client.WxRefl(4,name,method,key,...args);

WxSc.Dict = {}
WxSc.Dict.Add = (key,name,...args) => document.Client.WxAddObj(key,name,...args);
WxSc.Dict.Set = (key,a,u,v) => WxSc.Refl.Set(WxSc.Const.SocketConnection,`WxOs.${key}.${a}`,u,v);
WxSc.Dict.Get = (key,a=undefined) => WxSc.Refl.Get(WxSc.Const.SocketConnection,`WxOs.${key}${a ? `.${a}` : ''}`);
WxSc.Dict.Func = (key,method,...args) => WxSc.Refl.Func(WxSc.Const.SocketConnection,`WxOs.${key}.${method}`,...args);
WxSc.Dict.Tmp = (key,method,key2,...args) => WxSc.Refl.Tmp(WxSc.Const.SocketConnection,`WxOs.${key}.${method}`,key2,...args);
WxSc.Dict.TmpAttrib = (key,attrib,key2) => document.Client.WxTmpAttrib(key,attrib,key2);
WxSc.Dict.Del = (k) => document.Client.WxDelObj(k);

WxSc.Dict.AddCall = (n,r,f) => {
  WxSc.Priv[n] = f;
  document.Client.WxAddFunc(n,r);
}

WxSc.Const = {}
WxSc.Const.StateKey = 'LanBaiState';
WxSc.Const.DelayMs = 200;
WxSc.Const.PetManager = 'com.robot.core.manager.PetManager';
WxSc.Const.MainManager = 'com.robot.core.manager.MainManager';
WxSc.Const.SocketConnection = 'com.robot.core.net.SocketConnection';

WxSc.Util.GetBagPetInfos = () => WxSc.Refl.Get(WxSc.Const.PetManager,'allInfos');
WxSc.Util.GetBag1 = () => WxSc.Refl.Func(WxSc.Const.PetManager,'getBagMap');
WxSc.Util.GetBag2 = () => WxSc.Refl.Func(WxSc.Const.PetManager,'getSecondBagMap');

WxSc.Priv.ClearBagAsync = () => new Promise(res => { WxSc.Priv.res = res; document.Client.WxClearBag(); });
WxSc.Priv.SetBag1Async = bag1 => new Promise(res => { WxSc.Priv.res = res; document.Client.WxSetBag1(bag1); });
WxSc.Priv.SetBag2Async = bag2 => new Promise(res => { WxSc.Priv.res = res; document.Client.WxSetBag2(bag2); });
WxSc.Util.SetPetBagAsync = async (bag1, bag2 = []) => {
  await WxSc.Priv.ClearBagAsync();
  await WxSc.Priv.SetBag1Async(bag1);
  await WxSc.Priv.SetBag2Async(bag2);
  await WxSc.Util.DelayAsync(1000);
};

WxSc.Util.GetStoragePetsAsync = () => new Promise(resolve => {
  WxSc.Priv.res = resolve;
  document.Client.WxGetStoragePets();
});

WxSc.Util.GetClothes = () => {
  const cs = WxSc.Refl.Get(WxSc.Const.MainManager,'actorInfo.clothes');
  let result = [];
  for (let c of cs) {
    result.push(c.id);
    result.push(c.level);
  }
  return result;
}
WxSc.Util.GetClothesAsync = () => new Promise(resolve => {
  const KEY_CALLBACK = '_cbInfo', KEY_RESULT = '_kInfo';
  WxSc.Dict.AddCall(KEY_CALLBACK, KEY_RESULT, () => {
    const info = WxSc.Dict.Get(KEY_RESULT);
    let result = [];
    for (let c of info[0].clothes) {
      result.push(c.id);
      result.push(c.level);
    }
    WxSc.Dict.Del(KEY_RESULT);
    resolve(result)
  });
  WxSc.Refl.Func('com.robot.core.manager.UserInfoManager','getInfo',false,WxSc.Refl.Get(WxSc.Const.MainManager,'actorInfo.userID'),true,KEY_CALLBACK);
})
WxSc.Util.ChangeCloth = (clothes,isNet=true) => {
  const k1 = 'cl', k2 = 'it', k3 = 'be';
  WxSc.Dict.Add(k1,'Array');
  for (let i=0; i<clothes.length; i+=2) {
    if (clothes[i] === 0) continue;
    WxSc.Dict.Add(k2,'com.robot.core.info.clothInfo.PeopleItemInfo',false,clothes[i],false,clothes[i+1]);
    WxSc.Dict.Func(k1,'push',true,k2);
  }
  WxSc.Dict.Add(k3,'com.robot.core.behavior.ChangeClothBehavior',true,k1,false,isNet);
  WxSc.Refl.Func(WxSc.Const.MainManager,'actorModel.execBehavior',true,k3);
}

WxSc.Util.GetTitle = () => WxSc.Refl.Get(WxSc.Const.MainManager,'actorInfo.curTitle');
WxSc.Util.SetTitle = title => document.Client.WxSetTitle(title);

WxSc.Util.GetRound = () => WxSc.Priv.Round;

WxSc.Util.UseSkill = skillID => WxSc.Util.Send(2405,skillID);
WxSc.Util.ChangePet = ct => {
  if (WxSc.Priv._fHP > 0) WxSc.Priv._posiCh = true;
  WxSc.Util.Send(2407,ct);
}
WxSc.Util.UsePetItem = itemID => WxSc.Util.Send(2406,WxSc.Priv._fCT,itemID,0);
WxSc.Util.UsePetItem10PP = () => {
  WxSc.Util.ItemBuy(300017);
  WxSc.Util.UsePetItem(300017);
};
WxSc.Util.ItemBuy = (itemID,c=1) => WxSc.Util.Send(2601,itemID,c);

WxSc.Util.StopAutoFight = () => { WxSc.OnFirstRound = WxSc.OnUseSkill = WxSc.OnChangePet = WxSc.OnFightOver = () => {}; };

WxSc.Util.GetFightingPetID = () => WxSc.Priv._fID;
WxSc.Util.GetFightingPetCatchTime = () => WxSc.Priv._fCT;
WxSc.Util.GetFightingPets = () => WxSc.Priv._fPets;
WxSc.Util.ChangePetByID = ids => {
  if (ids.length > 0) {
    for (let id of ids) {
      for (let p of WxSc.Priv._fPets) {
        if (p.id === id && p.hp > 0 && p.catchTime != WxSc.Priv._fCT) {
          WxSc.Util.ChangePet(p.catchTime);
          return;
        }
      }
    }
  }
  for (let p of WxSc.Priv._fPets) {
    if (p.hp > 0 && p.catchTime != WxSc.Priv._fCT) {
      WxSc.Util.ChangePet(p.catchTime);
      return;
    }
  }
}

WxSc.Util.DelayAsync = ms => new Promise(resolve => setTimeout(resolve, ms));

WxSc.Util.Send = (commandID, ...args) => {
  let ps = [];
  for (let arg of args) { ps.push(false); ps.push(arg); }
  WxSc.Refl.Func(WxSc.Const.SocketConnection,'send',false,commandID, ...ps);
}
WxSc.Util.SendAsync = (commandID, parameterArray) => new Promise(resolve => {
  WxSc.Priv.res = resolve;
  document.Client.WxSendWithCallback2(commandID, parameterArray);
});

WxSc.Util.GetPetNameByID = petID => WxSc.Refl.Func('com.robot.core.config.xml.PetXMLInfo','getName',false,petID);
WxSc.Util.GetSkillNameByID = skillID => WxSc.Refl.Func('com.robot.core.config.xml.SkillXMLInfo','getName',false,skillID);

WxSc.Util.SetIsHidePetFight = h => WxSc.Refl.Set('com.robot.app.fight.FightManager','petFightClass',false,(h ? 'PetFightDLL' : 'PetFightDLL_201308'));
WxSc.Util.SetIsAutoCure = cure => WxSc.Priv._cure = cure;
WxSc.Util.CurePet20HP = () => {
  WxSc.Util.ItemBuy(300011,6);
  WxSc.Util.ItemBuy(300017,6);
  for (let p of WxSc.Util.GetBag1()) {
    WxSc.Util.Send(2326,p.catchTime,300011);
    WxSc.Util.Send(2326,p.catchTime,300017);
  }
}
WxSc.Util.CurePetAll = () => {
  const p = WxSc.Refl.Func(WxSc.Const.PetManager,'getBagMap',false,true);
  for (let i of p) WxSc.Util.Send(2310,i.catchTime);
}
WxSc.Util.LowHP = () => {
  const c = ((new Date()).getUTCHours() + 8) % 24;
  WxSc.Util.Send(41129,(c < 12 || c >= 15) ? 8692 : 8694);
}
WxSc.Util.SimpleAlarm = msg => WxSc.Refl.Func('com.robot.core.ui.alert.SimpleAlarm','show',false,msg);

WxSc.Util.CopyFireAsync = async (fireType = null) => {
  // 从地图上借
  if (await new Promise(res => {
    WxSc.Priv.res = res;
    document.Client.WxCopyFireFromMap(fireType);
  })) {
    WxSc.Util.SimpleAlarm('借火成功');
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
    WxSc.Priv.res = resolve;
    document.Client.WxGetRankListLen(key, sub_key);
  });
  len -= 100;
  for (let i = 0; i <= len; i += 100) {
    WxSc.Util.SimpleAlarm(`${i}/${len}`);
    if (await new Promise(resolve => {
      WxSc.Priv.res = resolve;
      document.Client.WxCopyFireFromRank(key, sub_key, i, fireType);
    })) {
      WxSc.Util.SimpleAlarm('借火成功');
      return true;
    }
  }
  WxSc.Util.SimpleAlarm('借火失败');
  return false;
};

WxSc.Util.ChangeMap = id => WxSc.Refl.Func('com.robot.core.manager.MapManager','changeMap',false,id);
WxSc.Util.ShowAppModule = id => WxSc.Refl.Func('com.robot.core.manager.ModuleManager','showAppModule',false,id);

WxSc.Util.StateSave = async k => {
  let s = {};
  s.clothes = await WxSc.Util.GetClothesAsync();
  s.title = WxSc.Util.GetTitle();
  s.bag1 = WxSc.Util.GetBag1().map(pet => pet.catchTime);
  s.bag2 = WxSc.Util.GetBag2().map(pet => pet.catchTime);
  let st = {};
  if (localStorage.getItem(WxSc.Const.StateKey)!=null) st = JSON.parse(localStorage.getItem(WxSc.Const.StateKey));
  st[k] = s;
  localStorage.setItem(WxSc.Const.StateKey, JSON.stringify(st));
  WxSc.Util.SimpleAlarm('ok');
}
WxSc.Util.StateLoadAsync = async k => {
  try {
    let s = JSON.parse(localStorage.getItem(WxSc.Const.StateKey))[k];
    WxSc.Util.ChangeCloth(s.clothes);
    WxSc.Util.SetTitle(s.title);
    await WxSc.Util.SetPetBagAsync(s.bag1, s.bag2);
    WxSc.Util.SimpleAlarm('ok');
  } catch {alert(`${k} 不存在`)}
}

// 对战
WxSc._in = () => {
  WxSc.Dict.AddCall('_start','_stIn',() => {
    WxSc.Priv.Round = 0;
    WxSc.Priv._posiCh = false; // 主动切换精灵
    WxSc.Priv._fPets = WxSc.Dict.Get('_rdData');
    const info = WxSc.Dict.Get('_stIn','0.data');
    WxSc.Priv._fCT = info.myInfo.catchTime;
    WxSc.Priv._fID = info.myInfo.petID;
    WxSc.Priv._fHP = info.myInfo.hp;
    WxSc.OnFirstRound(info);
  });
  WxSc.Refl.Func(WxSc.Const.SocketConnection,'addCmdListener',false,2504,true,'_start'); // NOTE_START_FIGHT
  WxSc.Dict.AddCall('_skill','_skIn',() => {
    const info = WxSc.Dict.Get('_skIn','0.data');
    let my, en;
    const isFi = (info.firstAttackInfo.userID === WxSc.Refl.Get(WxSc.Const.MainManager,'actorInfo.userID'));
    if (isFi) {
      my = info.firstAttackInfo;
      en = info.secondAttackInfo; 
    }
    else {
      my = info.secondAttackInfo;
      en = info.firstAttackInfo; 
    }
    WxSc.Priv._fHP = my.remainHP;
    for (let i=0; i<WxSc.Priv._fPets.length; ++i) {
      if (WxSc.Priv._fPets[i].catchTime === WxSc.Priv._fCT) {
        WxSc.Priv._fPets[i].hp = my.remainHP;
        break;
      }
    }
    for (let p of my.changehps) {
      for (let i=0; i<WxSc.Priv._fPets.length; ++i) {
        if (WxSc.Priv._fPets[i].catchTime === p.id) {
          WxSc.Priv._fPets[i].hp = p.hp;
          break;
        }
      }
    }
    WxSc.Priv.ShowRound((my.maxHp===0 ? 0 : (my.remainHP / my.maxHp * 100).toFixed(1)), (en.maxHp===0? 0 : (en.remainHP / en.maxHp * 100).toFixed(1)));
    if (en.remainHP === 0 && en.changehps.every(p => p.hp === 0) || my.remainHP === 0 && my.changehps.every(p => p.hp === 0)) return;
    WxSc.OnUseSkill(my,en,isFi);
  });
  WxSc.Refl.Func(WxSc.Const.SocketConnection,'addCmdListener',false,2505,true,'_skill'); // NOTE_USE_SKILL
  WxSc.Dict.AddCall('_ChPet','_chIn',() => {
    const info = WxSc.Dict.Get('_chIn','0.data');
    if (info.userID === WxSc.Refl.Get(WxSc.Const.MainManager,'actorInfo.userID')) {
      WxSc.Priv._fCT = info.catchTime;
      WxSc.Priv._fID = info.petID;
      WxSc.Priv._fHP = info.hp;
      if (WxSc.Priv._posiCh) WxSc.Priv._posiCh = false; // 主动切换
      else WxSc.OnChangePet(info); // 死亡切换
    }
  });
  WxSc.Refl.Func(WxSc.Const.SocketConnection,'addCmdListener',false,2407,true,'_ChPet'); // CHANGE_PET
  WxSc.Dict.AddCall('_over','_ovIn',() => {
    if (WxSc.Priv._cure) for (let i of WxSc.Priv._fPets) WxSc.Util.Send(2310,i.catchTime);
    WxSc.OnFightOver(WxSc.Dict.Get('_ovIn','0.data')); 
  });
  WxSc.Refl.Func(WxSc.Const.SocketConnection,'addCmdListener',false,2506,true,'_over'); // FIGHT_OVER
}

";

        private static JObject jFightTemplate = Utils.TryGetJObject(Properties.Settings.Default.FlashFightTemplate);

        private const string K_CHECK = "c", K_SCRIPT = "s";
        private const string CB_T = "1", CB_F = "";

        public static TaskCompletionSource<bool> _t;

        public FormFlashFightHandler()
        {
            InitializeComponent();
        }

        private void FormFlashFightHandler_Load(object sender, EventArgs e)
        {
            ResetLvTemplate();
        }

        private static string JsToAsync(string js)
        {
            return String.Format("(async ()=>{{try{{{0}\n;seerRandomSkinObj.resolve(true)}}catch(e){{console.error(e);seerRandomSkinObj.resolve(false)}}}})()", js);
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            _t = new TaskCompletionSource<bool>();
            Form1.chromiumBrowser.ExecuteScriptAsync(JsToAsync(richTextBox_script.Text));
            await _t.Task;
            btnTest.Enabled = true;
        }

        private async void btnMultiExec_Click(object sender, EventArgs e)
        {
            btnMultiExec.Enabled = false;
            foreach (ListViewItem i in lvTemplate.CheckedItems)
            {
                _t = new TaskCompletionSource<bool>();
                Form1.chromiumBrowser.ExecuteScriptAsync(JsToAsync(jFightTemplate[i.Text][K_SCRIPT].ToString()));
                if (await _t.Task)
                {
                    i.BackColor = Color.SpringGreen;
                }
            }
            btnMultiExec.Enabled = true;
        }

        private void lvTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTemplate.SelectedItems.Count != 1) return;
            var item = lvTemplate.SelectedItems[0];
            tbTemplateName.Text = item.Text;
            richTextBox_script.Text = jFightTemplate[item.Text][K_SCRIPT].ToString();
        }

        private IEnumerable<ListViewItem> GetAllListViewItems()
        {
            return jFightTemplate.Properties().Select(item => new ListViewItem(item.Name) { Checked = item.Value[K_CHECK].ToString().Length != 0 });
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
            UpdateItemSel();
            jFightTemplate[tbTemplateName.Text] = new JObject
            {
                { K_CHECK, CB_F },
                { K_SCRIPT, richTextBox_script.Text }
            };
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
            if (lvTemplate.CheckedItems.Count > 0 && MessageBox.Show($"确定要删除【{lvTemplate.CheckedItems[0].Text}】等 {lvTemplate.CheckedItems.Count} 个脚本吗？", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK) return;
            foreach (ListViewItem del in lvTemplate.CheckedItems)
            {
                jFightTemplate.Remove(del.Text);
            }
            SaveConfigTemplate();
        }

        public static void StopAutoFight()
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.StopAutoFight()");
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
        }

        #region 快捷方式
        private void btnCure20_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.CurePet20HP();");
        }

        private void btnCureAll_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.CurePetAll();");
        }

        private void btnItem170_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.UsePetItem(300749);");
        }

        private void btnItem150_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.UsePetItem(300701);");
        }

        private void btnItem10pp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.UsePetItem10PP();");
        }

        private void btn20pp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.UsePetItem(300018);");
        }

        private void btnItem250hp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.UsePetItem(300079);");
        }

        private void btnItem200hp_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.UsePetItem(300157);");
        }

        private void btnFightPanelHide_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.SetIsHidePetFight(true);");
        }

        private void btnFightPanelShow_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.SetIsHidePetFight(false);");
        }

        private void btnAutoCureOpen_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.SetIsAutoCure(true);");
        }

        private void btnAutoCureStop_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("WxSc.Util.SetIsAutoCure(false);");
        }
        #endregion

        private void UpdateItemSel()
        {
            foreach (ListViewItem i in lvTemplate.Items)
            {
                jFightTemplate[i.Text][K_CHECK] = i.Checked ? CB_T : CB_F;
            }
            Properties.Settings.Default.FlashFightTemplate = jFightTemplate.ToString();
            Properties.Settings.Default.Save();
        }

        private void ResetJObj()
        {
            var o = new JObject();
            foreach (ListViewItem i in lvTemplate.Items)
            {
                o[i.Text] = new JObject
                {
                    { K_CHECK, i.Checked ? CB_T : CB_F },
                    { K_SCRIPT, jFightTemplate[i.Text][K_SCRIPT] }
                };
            }
            jFightTemplate = o;
            Properties.Settings.Default.FlashFightTemplate = jFightTemplate.ToString();
            Properties.Settings.Default.Save();
        }

        private void btnItemDown_Click(object sender, EventArgs e)
        {
            if (lvTemplate.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先在列表中点击要移动项的名字");
                return;
            }
            int i = lvTemplate.SelectedIndices[0];
            if (i == lvTemplate.Items.Count - 1) return;
            (sender as Button).Enabled = false;
            (lvTemplate.Items[i + 1].Text, lvTemplate.Items[i].Text) = (lvTemplate.Items[i].Text, lvTemplate.Items[i + 1].Text);
            (lvTemplate.Items[i + 1].Checked, lvTemplate.Items[i].Checked) = (lvTemplate.Items[i].Checked, lvTemplate.Items[i + 1].Checked);
            lvTemplate.Items[i].Selected = false;
            lvTemplate.Items[i + 1].Selected = true;
            ResetJObj();
            (sender as Button).Enabled = true;
        }
        private void btnItemUp_Click(object sender, EventArgs e)
        {
            if (lvTemplate.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先在列表中点击要移动项的名字");
                return;
            }
            int i = lvTemplate.SelectedIndices[0];
            if (i == 0) return;
            (sender as Button).Enabled = false;
            (lvTemplate.Items[i - 1].Text, lvTemplate.Items[i].Text) = (lvTemplate.Items[i].Text, lvTemplate.Items[i - 1].Text);
            (lvTemplate.Items[i - 1].Checked, lvTemplate.Items[i].Checked) = (lvTemplate.Items[i].Checked, lvTemplate.Items[i - 1].Checked);
            lvTemplate.Items[i].Selected = false;
            lvTemplate.Items[i - 1].Selected = true;
            ResetJObj();
            (sender as Button).Enabled = true;
        }

        private void FormFlashFightHandler_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateItemSel();
        }
    }
}

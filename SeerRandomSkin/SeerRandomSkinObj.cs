using CefSharp;
using System;
using System.Threading.Tasks;

namespace SeerRandomSkin
{
    /// <summary>
    /// 给 js 层调用的对象
    /// </summary>
    internal class SeerRandomSkinObj
    {
        public void GetCloth(string bag)
        {
            FormPetBag.SeerCloth = bag;
        }

        public void ScreenShot()
        {
            if (Form1.childFormScreenShot == null) return;
            Form1.childFormScreenShot.ScreenShot();
        }

        public void GetRecvPackArray(string cmd, string pack)
        {
            Form1.childFormPack.ShowRecvPack(cmd, pack);
        }

        public void GetSendPackArray(string cmd, string pack)
        {
            Form1.childFormPack.ShowSendPack(cmd, pack);
        }

        public void ShowFightInfo(string hp1, int round, string hp2)
        {
            Form1.ChangeTitleAction(String.Format("{0} {1}% ({2}) {3}%", Form1.FormTitle, hp1, round, hp2));
        }

        public async void OnLogined()
        {
            FormPetFollow.WxPetFollow();
            await Task.Delay(3000);
            FormPetFollow.WxScale();
            if (Properties.Settings.Default.Mount.Length > 0)
            {
                FormPetFollow.ChangeCloth(Properties.Settings.Default.Suits);
                FormPetFollow.ShowMount(Properties.Settings.Default.Mount);
            }
            FormPetFollow.ScaleKeep();

            Form1.chromiumBrowser.ExecuteScriptAsync($"(()=>{{WxSc.Dict.AddCall('_jipai','_k',()=>{{seerRandomSkinObj.screenShot()}});WxSc.Refl.Func(WxSc.Const.SocketConnection,'addCmdListener',false,45144,true,'_jipai')}})()");
        }
    }
}

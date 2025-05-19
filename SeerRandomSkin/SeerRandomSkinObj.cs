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
        public void ScreenShot()
        {
            if (Form1.childFormScreenShot == null) return;
            Form1.childFormScreenShot.ScreenShot();
        }

        public void ShowFightInfo(string hp1, int round, string hp2)
        {
            Form1.ChangeTitleAction(String.Format("{0} {1}% ({2}) {3}%", Form1.FormTitle, hp1, round, hp2));
        }

        public async void OnLogined()
        {
            FormPetFollow.WxPetFollow();
            await Task.Delay(5000);
            PetScale();
            if (Properties.Settings.Default.Mount.Length > 0)
            {
                FormPetFollow.ChangeCloth(Properties.Settings.Default.Suits);
                if (Properties.Settings.Default.Mount != "0") FormPetFollow.ShowMount(Properties.Settings.Default.Mount);
            }
            FormPetFollow.ScaleKeep();

            Form1.chromiumBrowser.ExecuteScriptAsync($"(()=>{{WxSc.Dict.AddCall('_jipai','_k',()=>{{seerRandomSkinObj.screenShot()}});WxSc.Refl.Func(WxSc.Const.SocketConnection,'addCmdListener',false,45144,true,'_jipai')}})()");

            if (Properties.Settings.Default.Title != String.Empty) FormPetFollow.ShowTitle(Properties.Settings.Default.Title);

            int.TryParse(Properties.Settings.Default.AutoAlarmOk, out int autoAlarmOk);
            if (autoAlarmOk > 0) Form1.chromiumBrowser.ExecuteScriptAsync($"document.Client.WxAutoAlarmOk({autoAlarmOk})");
        }
        public void PetScale()
        {
            FormPetFollow.WxScale();
        }

        public void Resolve(bool r)
        {
            FormFlashFightHandler._t.TrySetResult(r);
        }
    }
}

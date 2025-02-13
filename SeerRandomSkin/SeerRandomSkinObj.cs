using CefSharp;
using System;

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

        public void ShowFightInfo(double hp1, int round, double hp2)
        {
            Form1.ChangeTitleAction(String.Format("{0} {1}% ({2}) {3}%", Form1.FormTitle, hp1, round, hp2));
        }
    }
}

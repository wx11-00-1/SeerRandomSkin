﻿using System;
using System.Windows.Forms;

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

        public void ShowFightInfo(int round, double hpPercent)
        {
            Form1.ChangeTitleAction(String.Format("{0} ({1}) [{2}%]", Form1.FormTitle, round, hpPercent));
        }

        public void SetH5SkinsJsFileName(string json)
        {
            Form1.H5SkinsJs = Utils.TryGetJObject(json);
        }

        public void Log(string s)
        {
            MessageBox.Show(s);
        }
    }
}

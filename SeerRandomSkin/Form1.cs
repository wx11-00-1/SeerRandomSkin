﻿using CefSharp;
using CefSharp.Callback;
using CefSharp.Handler;
using CefSharp.WinForms;
using EasyHook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class Form1 : Form
    {
        //游戏网址
        public const string gameAddress = "https://seer.61.com/play.shtml";
        public const string gameH5Address = "https://seerh5.61.com";

        static readonly Properties.Settings Configs = Properties.Settings.Default;

        public static ChromiumWebBrowser chromiumBrowser;
        private Form2 childForm2 = null;
        private FormConfig childFormConfig = null;
        private FormPetBag childFormPetBag = null;
        public static FormScreenShot childFormScreenShot = null;
        public static FormPack childFormPack = null;

        public bool isFullScreen = false;
        private static readonly List<int> skinIds = new List<int>();
        private static readonly List<int> skinExclusion = new List<int>();

        public static string FormTitle; // 窗口标题
        public static Action<string> ChangeTitleAction;

        private static bool isHideFlashFightPanel = false;
        public static bool IsHideFlashFightPanel
        {
            get
            {
                return isHideFlashFightPanel;
            }
            set
            {
                isHideFlashFightPanel = value;
                chromiumBrowser.ExecuteScriptAsync(value ? "document.Client.WxHiddenFightPanelStart();" : "document.Client.WxHiddenFightPanelStop();");
            }
        }

        public static Dictionary<int, int> SpecificPetSkins;

        public static List<FiddleObject> FiddleObjects;
        public const string FiddleFilePath = @"file/fd";
        private static readonly List<FiddleObject> FiddleUrl = new List<FiddleObject>();
        private static readonly List<FiddleObject> FiddleFile = new List<FiddleObject>();

        public Form1()
        {
            try
            {
                System.Diagnostics.Process.Start(Configs.AutoExecuteSoftwarePath1);
                System.Diagnostics.Process.Start(Configs.AutoExecuteSoftwarePath2);
                System.Diagnostics.Process.Start(Configs.AutoExecuteSoftwarePath3);
            }
            catch { }

            //初始化cef
            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("ppapi-flash-version", "99.0.0.999"); //显示out of date时，直接冒充一下版本
            settings.CefCommandLineArgs.Add("ppapi-flash-path", @"file/dll/pepflashplayer.dll");
            settings.CachePath = AppDomain.CurrentDomain.BaseDirectory + @"\cache";
            settings.LogSeverity = LogSeverity.Disable;//关闭记录debug.log的功能
            Cef.EnableHighDPISupport();
            Cef.Initialize(settings);
            // 自动 flash 插件
            var contx = Cef.GetGlobalRequestContext();
            // js 调用 c# 相关
            CefSharpSettings.WcfEnabled = true;
            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                contx.SetPreference("profile.default_content_setting_values.plugins", 1, out string err);
            });

            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            FormTitle = Text;
            ChangeTitleAction = (title) =>
            {
                Text = title;
            };

            Width = (int)Configs.WinWidth;
            Height = (int)Configs.WinHeight;
            CenterToScreen();

            // 初始化皮肤列表
            if (Configs.SkinIds == "")
            {
                await GetSkinData();
            }
            else
            {
                string[] strings = Configs.SkinIds.Replace(" ", "").Split(',');
                foreach (string s in strings)
                {
                    if (s == "" || !int.TryParse(s,out int id) || id < Configs.SkinRangeFloor || id > Configs.SkinRangeCeiling) continue;
                    skinIds.Add(id);
                }
            }
            // 初始化随机皮肤的排除项
            string[] exc = Configs.RandomSkinExclusion.Replace(" ", "").Split(',');
            foreach (string ex in exc)
            {
                if (ex == "" || !int.TryParse(ex, out int id)) continue; skinExclusion.Add(id);
            }

            chromiumBrowser = CreateChromium(Configs.IsH5First ? gameH5Address : gameAddress);
            Controls.Add(chromiumBrowser);
            ResizeChromiumBrowser();

            new Thread(() =>
            {
                Thread.Sleep(2000);
                if (Configs.IsUseSocketHack) FlashSocketHack();
                FlashSpeedHack();
            })
            { IsBackground = true }.Start();

            // 加载窗口
            if (Configs.AutoLoadActivities)
            {
                var f = new FormActivityCollection(); f.Show();
            }
            if (Configs.AutoLoadFightHandler)
            {
                var f = new FormFlashFightHandler(); f.Show();
            }
            if (Configs.AutoLoadH5Pack)
            {
                childFormPack = new FormPack(); childFormPack.Show();
            }
            if (Configs.AutoLoadPetBag)
            {
                var f = new FormPetBag(); f.Show();
            }
            if (Configs.AutoLoadScreenShot)
            {
                childFormScreenShot = new FormScreenShot
                {
                    MainForm = this
                };
                childFormScreenShot.Show();
                childFormScreenShot.ScreenShot();
            }
            if (Configs.AutoLoadFlashMap)
            {
                var f = new FormStrollMap(); f.Show();
            }
            if (Configs.AutoLoadFD)
            {
                var f = new FormFiddler(); f.Show();
            }

            SpecificPetSkins = Utils.TryJsonConvert<Dictionary<int, int>>(Configs.SpecificPetSkins);
            FiddleObjects = Utils.TryJsonConvert<List<FiddleObject>>(Configs.FiddleObjects);
            foreach (var fiddleObject in FiddleObjects)
            {
                fiddleObject.FromReg = new Regex(fiddleObject.From);
                if (fiddleObject.IsUrl)
                {
                    FiddleUrl.Add(fiddleObject);
                }
                else
                {
                    FiddleFile.Add(fiddleObject);
                }
            }
        }

        private ChromiumWebBrowser CreateChromium(string address)
        {
            ChromiumWebBrowser chromium = new ChromiumWebBrowser(address)
            {
                Dock = DockStyle.None,
                Location = new Point(0, 25),
                Size = new Size(960, 560),
                RequestHandler = new MyRequestHandler(),
                BrowserSettings = new BrowserSettings(),
                KeyboardHandler = new KeyBoardHandler() { mainForm = this }
            };
            chromium.BrowserSettings.StandardFontFamily =
            chromium.BrowserSettings.SansSerifFontFamily =
            chromium.BrowserSettings.SerifFontFamily =
            chromium.BrowserSettings.FantasyFontFamily =
            chromium.BrowserSettings.FixedFontFamily =
            chromium.BrowserSettings.CursiveFontFamily = Configs.BrowserFont;

            // 注册 js 类
            chromium.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            chromium.JavascriptObjectRepository.Register("seerRandomSkinObj", new SeerRandomSkinObj(), false, BindingOptions.DefaultBinder);
            
            //页面加载完毕后
            chromium.FrameLoadEnd += (sender, args) =>
            {
                if (args.Frame.IsMain)
                {
                    // 自动静音
                    args.Browser.GetHost().SetAudioMuted(true);
                    // 隐藏滚动条
                    args.Browser.MainFrame.ExecuteJavaScriptAsync("document.body.style.overflow = 'hidden'");
                    if (address == gameH5Address)
                    {
                        args.Browser.MainFrame.ExecuteJavaScriptAsync(
                            "WxSeerUtil = {};" +
                            "WxSeerUtil.AutoCurePet = true;" +
                            "WxSeerUtil.Initialized = false;" +

                            // 是否显示收发包的标志
                            "WxSeerUtil.HideRecv = true;" +
                            "WxSeerUtil.HideSend = true;" +
                            // hook 对象 prototype 函数
                            "WxSeerUtil.rsPrototypeWrapRecv = (obj, meth) => {" +
                            "   var orig = obj[meth];" +
                            "   obj[meth] = function rsPrototypeWrapper(){" +
                            "       if(!WxSeerUtil.HideRecv) {" +
                            "           let view = arguments[0].data;" +
                            "           let abLen = view.buffer.byteLength;" +
                            "           let packStr=\"\";" +
                            "           for(var i=0; i<abLen; ++i) { packStr += (view.getUint8(i)).toString(16).padStart(2, '0').toUpperCase(); }" +
                            "           seerRandomSkinObj.getRecvPackArray((view.getUint32(5)).toString(), packStr);" +
                            "       }" +
                            "       var res = orig.apply(this, arguments);" +
                            "       return res;" +
                            "   }" +
                            "};" +
                            "WxSeerUtil.rsPrototypeWrapSend = (obj, meth) => {" +
                            "   var orig = obj[meth];" +
                            "   obj[meth] = function rsPrototypeWrapper(){" +
                            "       var res = orig.apply(this, arguments);" +
                            "       if (!WxSeerUtil.HideSend) {" +
                            "           let view = res.data;" +
                            "           let abLen = view.buffer.byteLength;" +
                            "           let packStr = '';" +
                            "           for (var i = 0; i < abLen; ++i) { packStr += (view.getUint8(i)).toString(16).padStart(2, '0').toUpperCase(); }" +
                            "           seerRandomSkinObj.getSendPackArray((view.getUint32(5)).toString(), packStr);" +
                            "       }" +
                            "       return res;" +
                            "   }" +
                            "};" +

                            "Object.defineProperty(window, 'ActivityAnnouncement', {" +
                            "   get: () => {" +
                            "       if (WxSeerUtil.Initialized) return;" +
                            // 自动治疗
                            "       SocketConnection.addCmdListener(2506, () => { if (WxSeerUtil.AutoCurePet) PetManager.noAlarmCureAll(); });" +
                            // H5 巅峰记牌器
                            "       SocketConnection.addCmdListener(45144, () => { seerRandomSkinObj.screenShot(); });" +

                            // hook 接受包的数据解析函数
                            "       WxSeerUtil.rsPrototypeWrapRecv(SocketEncryptImpl.prototype, 'parseData');" +
                            // hook 发送包
                            "       WxSeerUtil.rsPrototypeWrapSend(SocketEncryptImpl.prototype, 'pack');" +

                            "       WxSeerUtil.Initialized = true;" +
                            "       return 0;" +
                            "   }," +
                            "   set: () => {}" +
                            "});");
                    }
                    else if (address == gameAddress)
                    {
                        args.Browser.MainFrame.ExecuteJavaScriptAsync(String.Format("document.body.style.zoom = {0};", Configs.FlashZoom));
                        args.Browser.MainFrame.ExecuteJavaScriptAsync(FormFlashFightHandler.JS_FIGHT_ENVIRONMENT);
                    }
                }
            };
            return chromium;
        }

        public class MyRequestHandler : RequestHandler
        {
            protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
            IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
            {
                return new MyResourceRequestHandler();
            }

            public class MyResourceRequestHandler : ResourceRequestHandler
            {
                static readonly Random random_obj = new Random();
                // https://seer.61.com/resource/fightResource/pet/swf/3788.swf
                static readonly Regex rgxPetSkin = new Regex("https:\\/\\/seer\\.61\\.com\\/resource\\/fightResource\\/pet\\/swf\\/(\\d{4,})\\.swf\\?");

                private int GetRandomSkinId()
                {
                    return skinIds[random_obj.Next(skinIds.Count)];
                }

                protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
                {
                    foreach (var fiddleObject in FiddleUrl)
                    {
                        if (fiddleObject.IsUrl && fiddleObject.FromReg.IsMatch(request.Url))
                        {
                            request.Url = fiddleObject.To;
                            return CefReturnValue.Continue;
                        }
                    }
                    // 随机替换 1000 以后的精灵皮肤
                    var ms = rgxPetSkin.Matches(request.Url);
                    if (ms.Count > 0)
                    {
                        int skin_id = int.Parse(ms[0].Groups[1].Value);
                        int rid;
                        if (SpecificPetSkins.ContainsKey(skin_id))
                        {
                            SpecificPetSkins.TryGetValue(skin_id, out rid);
                        }
                        else if (!Configs.IsRandomSkin || skinExclusion.Contains(skin_id))
                        {
                            return CefReturnValue.Continue;
                        }
                        else
                        {
                            rid = GetRandomSkinId();
                        }
                        request.Url = @"https://seer.61.com/resource/fightResource/pet/swf/" + rid + @".swf";
                        chromiumBrowser.ExecuteScriptAsync($"console.log({rid}, document.Client.WxGetPetNameByID({rid}))");
                    }

                    return CefReturnValue.Continue;
                }

                protected override IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
                {
                    string url = request.Url;
                    foreach (var fiddleObject in FiddleFile)
                    {
                        if (!fiddleObject.IsUrl && fiddleObject.FromReg.IsMatch(url))
                        {
                            return new MyResourceHandler(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FiddleFilePath, fiddleObject.To));
                        }
                    }
                    if (url.StartsWith("https://seer.61.com/dll/PetFightDLL_201308.swf?"))
                    {
                        if (IsHideFlashFightPanel)
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\PetFightDLL.swf");
                        }
                    }
                    else if (url.StartsWith("https://seer.61.com/resource/xml/battleStrategy.xml?"))
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\xml\battleStrategy.xml");
                    }
                    else if (url.StartsWith("https://seer.61.com/dll/Assets.swf?"))
                    {
                        if (Configs.IsChangeBackground)
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\Assets.swf");
                        }
                    }
                    else if (url.StartsWith("https://seer.61.com/resource/uiIcon/yearvip_icon.swf?"))
                    {
                        if (Configs.IsChangeVipIcon)
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\yearvip_icon.swf");
                        }
                    }
                    else if (url.StartsWith(@"https://seer.61.com/resource/login/ServerAdPanel1.swf?"))
                    {
                        if (Configs.IsChangeAdPanel)
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\NoAd.swf");
                        }
                    }
                    else if (url.StartsWith(@"https://seer.61.com/resource/forApp/superMarket/tip.swf?"))
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\tip.swf");
                    }
                    else if (url.StartsWith(@"https://seer.61.com/module/com/robot/module/app/SupermarketPanel.swf?"))
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\DiduoAssistant.swf");
                    }

                    return base.GetResourceHandler(chromiumWebBrowser, browser, frame, request);
                }

                public class MyResourceHandler : IResourceHandler
                {
                    private readonly string _localResourceFileName;
                    private byte[] _localResourceData;
                    private int _dataReadCount;

                    public MyResourceHandler(string localResourceFileName)
                    {
                        this._localResourceFileName = localResourceFileName;
                        this._dataReadCount = 0;
                    }

                    public void Dispose()
                    {

                    }

                    public bool Open(IRequest request, out bool handleRequest, ICallback callback)
                    {
                        handleRequest = true;
                        return true;
                    }

                    public bool ProcessRequest(IRequest request, ICallback callback)
                    {
                        throw new NotImplementedException();
                    }

                    public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
                    {
                        using (FileStream fileStream = new FileStream(this._localResourceFileName, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader binaryReader = new BinaryReader(fileStream))
                            {
                                long length = fileStream.Length;
                                this._localResourceData = new byte[length];
                                // 读取文件中的内容并保存到私有变量字节数组中
                                binaryReader.Read(this._localResourceData, 0, this._localResourceData.Length);
                            }
                        }

                        responseLength = this._localResourceData.Length;
                        redirectUrl = null;
                    }

                    public bool Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback)
                    {
                        throw new NotImplementedException();
                    }

                    public bool Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback)
                    {
                        int leftToRead = this._localResourceData.Length - this._dataReadCount;
                        if (leftToRead == 0)
                        {
                            bytesRead = 0;
                            return false;
                        }

                        int needRead = Math.Min((int)dataOut.Length, leftToRead);
                        dataOut.Write(this._localResourceData, this._dataReadCount, needRead);
                        this._dataReadCount += needRead;
                        bytesRead = needRead;
                        return true;
                    }

                    public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
                    {
                        throw new NotImplementedException();
                    }

                    public void Cancel()
                    {

                    }
                }
            }
        }

        private uint GetCefSubprocessPid(string cmd)
        {
            string wmiQuery = string.Format("select CommandLine,ProcessId,ParentProcessId from Win32_Process where (ParentProcessID={0}) and (Name='{1}')", System.Diagnostics.Process.GetCurrentProcess().Id, "CefSharp.BrowserSubprocess.exe");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery))
            using (ManagementObjectCollection retObjectCollection = searcher.Get())
                foreach (ManagementObject retObject in retObjectCollection.Cast<ManagementObject>())
                {
                    string cmdStr = (string)retObject["CommandLine"];
                    if (cmdStr.IndexOf(cmd) > 0)
                    {
                        return (uint)retObject["ProcessId"];
                    }
                }
            return 0;
        }

        private void flashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls.Remove(chromiumBrowser);
            chromiumBrowser.Dispose();
            chromiumBrowser = null;
            chromiumBrowser = CreateChromium(gameAddress);
            Controls.Add(chromiumBrowser);
            ResizeChromiumBrowser();
        }

        private void h5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls.Remove(chromiumBrowser);
            chromiumBrowser.Dispose();
            chromiumBrowser = null;
            chromiumBrowser = CreateChromium(gameH5Address);
            Controls.Add(chromiumBrowser);
            ResizeChromiumBrowser();

        }

        private void 开发者工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromiumBrowser.ShowDevTools();
        }

        private void toolStripMenuItem_CureOpen_Click(object sender, EventArgs e)
        {
            chromiumBrowser.GetMainFrame().ExecuteJavaScriptAsync("WxSeerUtil.AutoCurePet = true;");
        }

        private void toolStripMenuItem_AutoCureClose_Click(object sender, EventArgs e)
        {
            chromiumBrowser.GetMainFrame().ExecuteJavaScriptAsync("WxSeerUtil.AutoCurePet = false;");
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (childForm2 == null)
            {
                childForm2 = new Form2();
                childForm2.Show();
            }
        }

        private void 获取皮肤数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSkinData();
        }

        private static async Task GetSkinData()
        {
            skinIds.Clear();
            string version_str = await GetJsonStringAsync("https://seerh5.61.com/version/version.json");
            Match m = Regex.Match(version_str, "\"monsters\\.json\":\"(.*?\\.json)\"");
            if(!m.Success)
            {
                return;
            }
            string monsters_str = await GetJsonStringAsync("http://seerh5.61.com/resource/config/xml/" + m.Groups[1].Value);
            foreach (Match match in Regex.Matches(monsters_str, "\"ID\":(\\d+),\"DefName", RegexOptions.None))
            {
                skinIds.Add(int.Parse(match.Groups[1].Value));
            }
            FilterSkins();
            SaveConfigSkinIds();
            MessageBox.Show("获取皮肤数据完成");
        }

        private static void SaveConfigSkinIds()
        {
            string s = "";
            foreach(var skinId in skinIds)
            {
                s = s + skinId + ",";
            }
            Configs.SkinIds = s;
            Configs.Save();
        }

        private static async Task<string> GetJsonStringAsync(string url)//从url获取文件内容
        {
            var client = new HttpClient();
            byte[] content = await client.GetByteArrayAsync(url);
            return Encoding.UTF8.GetString(content, 0, content.Length);
        }

        private void 开启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromiumBrowser.GetBrowser().GetHost().SetAudioMuted(false);
        }

        private void 静音ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromiumBrowser.GetBrowser().GetHost().SetAudioMuted(true);
        }

        private static void FilterSkins()
        {
            string[] blacks = Configs.SkinBlackList.Replace(" ", "").Split(',');
            HashSet<int> set1 = new HashSet<int>();
            foreach (string s in blacks)
            {
                if (s != "" && int.TryParse(s, out int id)) set1.Add(id);
            }
            skinIds.RemoveAll(data => set1.Contains(data));
        }

        private void 配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (childFormConfig == null)
            {
                childFormConfig = new FormConfig();
                childFormConfig.Show();
            }
        }

        private void 换装ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (childFormPetBag == null)
            {
                childFormPetBag = new FormPetBag();
                childFormPetBag.Show();
            }
        }

        private class KeyBoardHandler : IKeyboardHandler
        {
            public Form1 mainForm = null;

            public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
            {
                return false;
            }

            public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
            {
                if (type != KeyType.KeyUp) return false;
                switch(windowsKeyCode)
                {
                    case 0x7A:
                        // F11 全屏切换
                        mainForm.isFullScreen = !mainForm.isFullScreen;
                        if (!mainForm.isFullScreen)
                        {
                            mainForm.menuStrip1.Visible = true;
                            mainForm.FormBorderStyle = FormBorderStyle.Sizable;
                            mainForm.WindowState = FormWindowState.Normal;
                        }
                        else
                        {
                            mainForm.menuStrip1.Visible = false;
                            mainForm.FormBorderStyle = FormBorderStyle.None;
                            mainForm.WindowState = FormWindowState.Maximized;
                        }
                        break;
                }
                return false;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeChromiumBrowser();
        }

        private void ResizeChromiumBrowser()
        {
            if (chromiumBrowser == null) return;
            chromiumBrowser.Dock = DockStyle.Fill;
            if (isFullScreen) return;
            int tmpW = chromiumBrowser.Width;
            int tmpH = chromiumBrowser.Height;
            chromiumBrowser.Dock = DockStyle.None;
            chromiumBrowser.Width = tmpW;
            chromiumBrowser.Height = tmpH - 25;
        }

        private void FlashSpeedHack()
        {
            string libPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SpeedhackWrapper.dll");
            int pid = (int)GetCefSubprocessPid("type=ppapi");
            if (pid == 0) return;
            RemoteHooking.Inject(pid, libPath, libPath, null);
        }

        private void FlashSocketHack()
        {
            string libPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SocketHack.dll");
            int pid = (int)GetCefSubprocessPid("type=network.mojom");
            if (pid == 0) return;
            RemoteHooking.Inject(pid, libPath, libPath, Handle);
        }

        private void 收发包ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            childFormPack = new FormPack(); childFormPack.Show();
        }

        private void 巅峰记牌ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (childFormScreenShot != null) return;
            childFormScreenShot = new FormScreenShot
            {
                MainForm = this
            };
            childFormScreenShot.Show();
            childFormScreenShot.ScreenShot();
        }

        private void SetFlashAutoFight(bool flag)
        {
            chromiumBrowser.ExecuteScriptAsync(flag ? "document.Client.WxAutoUseSkillStart();" : "document.Client.WxAutoUseSkillEnd();");
        }

        private void 变速ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlashSpeedHack();
        }

        private void 开始自动出招ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetFlashAutoFight(true);
        }

        private void 结束自动出招ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetFlashAutoFight(false);
        }

        private void 隐藏战斗界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsHideFlashFightPanel = !IsHideFlashFightPanel;
            MessageBox.Show(IsHideFlashFightPanel ? "战斗界面已隐藏" : "战斗界面正常显示");
        }

        private void 压血ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFlashFightHandler.SetFightTemplate("压血");
            IsHideFlashFightPanel = true;
            chromiumBrowser.ExecuteScriptAsync("document.Client.WxLowHP();");
        }

        private void 自动治疗ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromiumBrowser.ExecuteScriptAsync("document.Client.WxAutoCureSwitch();");
        }

        private void 对战助手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormFlashFightHandler();
            form.Show();
        }

        private void 活动收藏夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormActivityCollection();
            f.Show();
        }

        private void 逛地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormStrollMap();
            f.Show();
        }

        private void 指定皮肤ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormSpecifyPetSkin();
            f.Show();
        }

        private void fDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormFiddler();
            f.Show();
        }
    }
}

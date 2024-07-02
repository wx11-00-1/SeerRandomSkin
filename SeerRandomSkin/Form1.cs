using CefSharp;
using CefSharp.Callback;
using CefSharp.Handler;
using CefSharp.Lagacy;
using CefSharp.WinForms;
using EasyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static bool IsHideFlashFightPanel { get; set; } = false;

        public Form1()
        {
            try
            {
                System.Diagnostics.Process.Start(Properties.Settings.Default.AutoExecuteSoftwarePath1);
                System.Diagnostics.Process.Start(Properties.Settings.Default.AutoExecuteSoftwarePath2);
                System.Diagnostics.Process.Start(Properties.Settings.Default.AutoExecuteSoftwarePath3);
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

            Width = (int)Properties.Settings.Default.WinWidth;
            Height = (int)Properties.Settings.Default.WinHeight;
            CenterToScreen();

            // 初始化皮肤列表
            if (Properties.Settings.Default.SkinIds == "")
            {
                await GetSkinData();
            }
            else
            {
                string[] strings = Properties.Settings.Default.SkinIds.Split(',');
                foreach (string s in strings)
                {
                    if (s == "") continue;
                    int id = int.Parse(s);
                    if (id < Properties.Settings.Default.SkinRangeFloor || id > Properties.Settings.Default.SkinRangeCeiling) continue;
                    skinIds.Add(id);
                }
            }
            // 初始化随机皮肤的排除项
            string[] exc = Properties.Settings.Default.RandomSkinExclusion.Split(',');
            foreach (string ex in exc)
            {
                if (ex == "") continue; int id = int.Parse(ex); skinExclusion.Add(id);
            }

            chromiumBrowser = CreateChromium(Properties.Settings.Default.IsH5First ? gameH5Address : gameAddress);
            Controls.Add(chromiumBrowser);
            ResizeChromiumBrowser();

            new Thread(() =>
            {
                Thread.Sleep(2000);
                if (Properties.Settings.Default.IsUseSocketHack) FlashSocketHack();
                FlashSpeedHack();
            })
            { IsBackground = true }.Start();
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
            if (Properties.Settings.Default.BrowserFont != "")
            {
                chromium.BrowserSettings.StandardFontFamily =
                chromium.BrowserSettings.SansSerifFontFamily =
                chromium.BrowserSettings.SerifFontFamily =
                chromium.BrowserSettings.FantasyFontFamily =
                chromium.BrowserSettings.FixedFontFamily =
                chromium.BrowserSettings.CursiveFontFamily = Properties.Settings.Default.BrowserFont;
            }

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
                            "           seerRandomSkinObj.getRecvPackArray(packStr);" +
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
                            "           seerRandomSkinObj.getSendPackArray(packStr);" +
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
                        args.Browser.MainFrame.ExecuteJavaScriptAsync(String.Format("document.body.style.zoom = {0};", Properties.Settings.Default.FlashZoom));
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
                readonly string pattern = "https:\\/\\/seer\\.61\\.com\\/resource\\/fightResource\\/pet\\/swf\\/(\\d{4,})\\.swf\\?";

                private int GetRandomSkinId()
                {
                    return skinIds[random_obj.Next(skinIds.Count)];
                }

                protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
                {
                    if(!Properties.Settings.Default.IsRandomSkin) return CefReturnValue.Continue;

                    // 随机替换 1000 以后的精灵皮肤
                    var ms = Regex.Matches(request.Url, pattern, RegexOptions.None);
                    if (ms.Count > 0)
                    {
                        int skin_id = int.Parse(ms[0].Groups[1].Value);
                        if (!skinExclusion.Contains(skin_id))
                        {
                            int rid = GetRandomSkinId();
                            request.Url = @"https://seer.61.com/resource/fightResource/pet/swf/" + rid + @".swf";
                            browser.MainFrame.ExecuteJavaScriptAsync("console.log(" + skin_id + "+' -> '+" + rid + ");");
                        }
                    }

                    return CefReturnValue.Continue;
                }

                protected override IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
                {
                    string url = request.Url;
                    if (url.Contains("https://seer.61.com/dll/PetFightDLL_201308.swf?"))
                    {
                        if (IsHideFlashFightPanel)
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\PetFightDLL.swf");
                        }
                    }
                    else if (url.Contains("https://seer.61.com/resource/xml/battleStrategy.xml?"))
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\xml\battleStrategy.xml");
                    }
                    else if (url == @"https://seer.61.com/dll/Assets.swf?lsx13yv4")
                    {
                        if (Properties.Settings.Default.IsChangeBackground)
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\Assets.swf");
                        }
                    }
                    else if (url.Contains("https://seer.61.com/resource/uiIcon/yearvip_icon.swf?"))
                    {
                        if (Properties.Settings.Default.IsChangeVipIcon)
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\yearvip_icon.swf");
                        }
                    }
                    else if (url.Contains(@"login/ServerAdPanel1.swf"))
                    {
                        if (Properties.Settings.Default.IsChangeAdPanel)
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\NoAd.swf");
                        }
                    }
                    else if (url.Contains(@"/resource/forApp/superMarket/tip.swf?"))
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\tip.swf");
                    }
                    else if (url.Contains(@"https://seer.61.com/module/com/robot/module/app/SupermarketPanel.swf?"))
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\DiduoAssistant.swf");
                    }
                    else if(Properties.Settings.Default.IsChangeH5LoginBg2024)
                    {
                        if(url == @"https://seerh5.61.com/resource/assets/ui/login202202/outside/2024nianfeidaiji.png")
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\png\pixel.png");
                        }
                        else if (url == @"https://seerh5.61.com/resource/assets/ui/login202202/outside/2024nianfeidaijiBg.png")
                        {
                            return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\png\bg_login_h5.png");
                        }
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
            Properties.Settings.Default.SkinIds = s;
            Properties.Settings.Default.Save();
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
            string[] blacks = Properties.Settings.Default.SkinBlackList.Split(',');
            HashSet<int> set1 = new HashSet<int>();
            foreach (string s in blacks)
            {
                if (s != "") set1.Add(int.Parse(s));
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
            Form1.chromiumBrowser.ExecuteScriptAsync(flag ? "document.Client.WxAutoUseSkillStart();" : "document.Client.WxAutoUseSkillEnd();");
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
            IsHideFlashFightPanel = true;
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxLowHP();");
        }

        private void 自动治疗ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.ExecuteScriptAsync("document.Client.WxAutoCureSwitch();");
        }
    }
}

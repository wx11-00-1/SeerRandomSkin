using CefSharp;
using CefSharp.Callback;
using CefSharp.Handler;
using CefSharp.WinForms;
using EasyHook;
using System;
using System.Collections.Generic;
using System.Data;
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
        private static int[] skinIds;
        private static HashSet<int> skinExclusion;

        public static string FormTitle; // 窗口标题
        public static Action<string> ChangeTitleAction;

        public static Dictionary<int, int> SpecificPetSkins;

        public static List<FiddleObject> FiddleObjects;
        public static readonly string FiddleFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"file/fd");

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
#if x64
            settings.CefCommandLineArgs.Add("ppapi-flash-path", @"file/dll/pepflashplayer.dll");
#else
            settings.CefCommandLineArgs.Add("ppapi-flash-path", @"file/dll/pepflashplayer32_15_0_0_152.dll");
#endif
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
                if (skinIds == null || skinIds.Length == 0)
                {
                    // 获取所有皮肤失败，不换了
                    Configs.IsRandomSkin = false;
                }
            }
            else
            {
                skinIds = Configs.SkinIds.Replace(" ", "").Split(',').Select(s =>
                {
                    int.TryParse(s, out int id);
                    return id;
                }).Distinct().Where(id => id != 0 && id >= Configs.SkinRangeFloor && id <= Configs.SkinRangeCeiling).ToArray();
            }
            // 初始化随机皮肤的排除项
            skinExclusion = Configs.RandomSkinExclusion.Replace(" ", "").Split(',').Select(s =>
            {
                int.TryParse(s, out int id);
                return id;
            }).Where(id => id != 0).ToHashSet();

            chromiumBrowser = CreateChromium(Configs.DefaultURL);
            panelBrowser.Controls.Add(chromiumBrowser);

            new Thread(() =>
            {
                Thread.Sleep(3000);
#if x64
                if (Configs.IsUseSocketHack) FlashSocketHack();
#endif
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
            if (FiddleObjects == null)
            {
                FiddleObjects = new List<FiddleObject>(); // 有可能解析失败
            }
            foreach (var fiddleObject in FiddleObjects)
            {
                fiddleObject.FromReg = new Regex(fiddleObject.From);
            }
        }

        private ChromiumWebBrowser CreateChromium(string address)
        {
            ChromiumWebBrowser chromium = new ChromiumWebBrowser(address)
            {
                Dock = DockStyle.Fill,
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
                    args.Browser.GetHost().SetAudioMuted(Configs.AutoMute);
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
                return new WxResourceRequestHandler();
            }

            public class WxResourceRequestHandler : ResourceRequestHandler
            {
                static readonly Random random_obj = new Random();
                // https://seer.61.com/resource/fightResource/pet/swf/3788.swf
                static readonly Regex rgxPetSkin = new Regex("https:\\/\\/seer\\.61\\.com\\/resource\\/fightResource\\/pet\\/swf\\/(\\d{4,})\\.swf\\?");

                private int GetRandomSkinId()
                {
                    return skinIds[random_obj.Next(skinIds.Length)];
                }

                protected override IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
                {
                    string url = request.Url;
                    foreach (var fiddleObject in FiddleObjects)
                    {
                        if (fiddleObject.FromReg.IsMatch(url))
                        {
                            if (fiddleObject.IsUrl)
                            {
                                return new WxUrlResourceHandler(fiddleObject.To);
                            }
                            else
                            {
                                return new WxFileResourceHandler(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FiddleFilePath, fiddleObject.To));
                            }
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
                            return base.GetResourceHandler(chromiumWebBrowser, browser, frame, request);
                        }
                        else
                        {
                            rid = GetRandomSkinId();
                        }
                        chromiumBrowser.ExecuteScriptAsync($"console.log({rid}, document.Client.WxGetPetNameByID({rid}))");
                        return new WxUrlResourceHandler(String.Format("https://seer.61.com/resource/fightResource/pet/swf/{0}.swf", rid));
                    }
                    if (url.StartsWith("https://seer.61.com/dll/PetFightDLL.swf?"))
                    {
                        return new WxFileResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\PetFightDLL.swf");
                    }
                    else if (url.StartsWith("https://seer.61.com/resource/xml/battleStrategy.xml?"))
                    {
                        if (Configs.IsHideBattleStrategy)
                        {
                            return new WxFileResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\xml\battleStrategy.xml");
                        }
                    }
                    else if (url.StartsWith(@"https://seer.61.com/resource/forApp/superMarket/tip.swf?"))
                    {
                        return new WxFileResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\tip.swf");
                    }

                    return base.GetResourceHandler(chromiumWebBrowser, browser, frame, request);
                }

                public class WxUrlResourceHandler : IResourceHandler
                {
                    private readonly string url;

                    public WxUrlResourceHandler(string url) { this.url = url; }

                    public void Cancel()
                    {
                    }

                    public void Dispose()
                    {
                    }

                    public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
                    {
                        responseLength = 0;
                        redirectUrl = url;
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

                    public bool Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback)
                    {
                        throw new NotImplementedException();
                    }

                    public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
                    {
                        throw new NotImplementedException();
                    }

                    public bool Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback)
                    {
                        throw new NotImplementedException();
                    }
                }

                public class WxFileResourceHandler : IResourceHandler
                {
                    private readonly string _localResourceFileName;
                    private byte[] _localResourceData;
                    private int _dataReadCount;

                    public WxFileResourceHandler(string localResourceFileName)
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
            panelBrowser.Controls.Remove(chromiumBrowser);
            chromiumBrowser.Dispose();
            chromiumBrowser = null;
            chromiumBrowser = CreateChromium(gameAddress);
            panelBrowser.Controls.Add(chromiumBrowser);
        }

        private void h5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBrowser.Controls.Remove(chromiumBrowser);
            chromiumBrowser.Dispose();
            chromiumBrowser = null;
            chromiumBrowser = CreateChromium(gameH5Address);
            panelBrowser.Controls.Add(chromiumBrowser);

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
            var blackList = Configs.SkinBlackList.Replace(" ", "").Split(',').Select(s =>
            {
                int.TryParse(s, out int id);
                return id;
            }).ToHashSet();
            string monsters_str = await GetJsonStringAsync("http://seerh5.61.com/resource/config/xml/" + m.Groups[1].Value);
            skinIds = Regex.Matches(monsters_str, "\"ID\":(\\d+),\"DefName", RegexOptions.None).Cast<Match>().Select(match =>
            {
                int.TryParse(match.Groups[1].Value, out int id);
                return id;
            }).Where(id => !blackList.Contains(id)).ToArray();
            SaveConfigSkinIds();
            MessageBox.Show("获取皮肤数据完成");
        }

        private static void SaveConfigSkinIds()
        {
            Configs.SkinIds = string.Join(",", skinIds);
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
                    case (int)Keys.F11:
                        // F11 全屏切换
                        mainForm.isFullScreen = !mainForm.isFullScreen;
                        if (!mainForm.isFullScreen)
                        {
                            mainForm.menuStrip1.Visible = true;
                            mainForm.WindowState = FormWindowState.Normal;
                            mainForm.FormBorderStyle = FormBorderStyle.Sizable;
                        }
                        else
                        {
                            mainForm.menuStrip1.Visible = false;
                            mainForm.FormBorderStyle = FormBorderStyle.None;
                            mainForm.WindowState = FormWindowState.Maximized;
                        }
                        break;
                    case (int)Keys.F5:
                        chromiumBrowser.Reload();
                        break;
                }
                return false;
            }
        }

        private void FlashSpeedHack()
        {
            string libPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SpeedhackWrapper.dll");
            int pid = (int)GetCefSubprocessPid("type=ppapi");
            if (pid == 0) return;
            RemoteHooking.Inject(pid, libPath, libPath, Configs.IsLoadFormSpeedhack);
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

        private void 变速ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlashSpeedHack();
        }

        private void 压血ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!FormFlashFightHandler.SetFightTemplate("压血出招"))
            {
                MessageBox.Show("没有找到压血的出招方案，请先恢复默认配置");
                return;
            }
            chromiumBrowser.ExecuteScriptAsync("document.Client.WxLowHP();");
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

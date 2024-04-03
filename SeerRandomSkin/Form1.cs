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
        private FormPetBag chileFormPetBag = null;
        private static readonly List<int> skinIds = new List<int>();

        public Form1()
        {
            //初始化cef
            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("ppapi-flash-version", "99.0.0.999"); //显示out of date时，直接冒充一下版本
            settings.CefCommandLineArgs.Add("ppapi-flash-path", @"file/dll/pepflashplayer32_15_0_0_152.dll");
            settings.CachePath = AppDomain.CurrentDomain.BaseDirectory + @"\cache";
            settings.LogSeverity = LogSeverity.Disable;//关闭记录debug.log的功能

            Cef.Initialize(settings);
            // 自动插件
            var contx = Cef.GetGlobalRequestContext();
            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                contx.SetPreference("profile.default_content_setting_values.plugins", 1, out string err);
            });

            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
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
                    if (s != "")
                    {
                        skinIds.Add(int.Parse(s));
                    }
                }
            }

            chromiumBrowser = CreateChromium(gameAddress);
            Controls.Add(chromiumBrowser);
            new Thread(() =>
            {
                Thread.Sleep(10000);
                string libPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SpeedhackWrapper.dll");
                int pid = (int)GetCefSubprocessPid("type=ppapi");
                if (pid == 0) return;
                RemoteHooking.Inject(pid, libPath, libPath, null);
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
            };
            // 注册 js 类
            CefSharpSettings.WcfEnabled = true;
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
                            "Object.defineProperty(window, 'ActivityAnnouncement', {" +
                            "   get: () => {" +
                            "       if (WxSeerUtil.Initialized) return;" +
                            "       SocketConnection.addCmdListener(2506, () => { if (WxSeerUtil.AutoCurePet) PetManager.noAlarmCureAll(); });" +
                            "       WxSeerUtil.Initialized = true;" +
                            "       return 0;" +
                            "   }," +
                            "   set: () => {}" +
                            "});");
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
                static Random random_obj = new Random();
                // https://seer.61.com/resource/fightResource/pet/swf/3788.swf
                string pattern = "https:\\/\\/seer\\.61\\.com\\/resource\\/fightResource\\/pet\\/swf\\/(\\d{4,})\\.swf\\?";

                private int GetRandomSkinId()
                {
                    int rand_idx = 0;
                    // 452 及以前的精灵皮肤，脚本结构与之后的精灵略有不同，替换上去可能会卡
                    while (rand_idx < 453)
                    {
                        rand_idx = skinIds[random_obj.Next(skinIds.Count)];
                    }
                    return rand_idx;
                }

                protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
                {
                    if(!Properties.Settings.Default.IsRandomSkin) return CefReturnValue.Continue;

                    // 随机替换 1000 以后的精灵皮肤
                    var ms = Regex.Matches(request.Url, pattern, RegexOptions.None);
                    if (ms.Count > 0)
                    {
                        int skin_id = int.Parse(ms[0].Groups[1].Value);
                        if (skin_id != 3788 && skin_id != 290003788 && skin_id != 1400512 && skin_id != 2900512)
                        {
                            //request.Url = @"https://seer.61.com/resource/fightResource/pet/swf/" + random_obj.Next(1000, 3290) + @".swf";
                            
                            request.Url = @"https://seer.61.com/resource/fightResource/pet/swf/" + GetRandomSkinId() + @".swf";
                        }
                    }

                    return CefReturnValue.Continue;
                }

                protected override IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
                {
                    string url = request.Url;
                    if (url == @"https://seer.61.com/dll/Assets.swf?lsx13yv4")
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\Assets.swf");
                    }
                    else if (url == @"https://seer.61.com/resource/uiIcon/yearvip_icon.swf?lqp17ri0")
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\yearvip_icon.swf");
                    }
                    else if (url.Contains(@"login/ServerAdPanel1.swf"))
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\NoAd.swf");
                    }
                    else if (url.Contains(@"/resource/forApp/superMarket/tip.swf"))
                    {
                        return new MyResourceHandler(AppDomain.CurrentDomain.BaseDirectory + @"\file\swf\tip_230831_closeBattery.swf");
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
            string wmiQuery = string.Format("select CommandLine,ProcessId from Win32_Process where Name='{0}'", "CefSharp.BrowserSubprocess.exe");
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
        }

        private void h5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls.Remove(chromiumBrowser);
            chromiumBrowser.Dispose();
            chromiumBrowser = null;
            chromiumBrowser = CreateChromium(gameH5Address);
            Controls.Add(chromiumBrowser);

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
            else
            {
                try
                {
                    childForm2.Dispose();
                    childForm2 = null;
                    childForm2 = new Form2();
                    childForm2.Show();
                }
                catch(Exception ex) { }
            }
        }

        private void 获取皮肤数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSkinData();
        }

        private static async Task GetSkinData()
        {
            string monsters_str = await GetJsonStringAsync("http://seerh5.61.com/resource/config/xml/monsters_191616a2.json");
            //await Console.Out.WriteLineAsync(monsters_str.Substring(0,500));
            string pattern = "\"ID\":(\\d{3,}),\"DefName";
            foreach (Match match in Regex.Matches(monsters_str, pattern, RegexOptions.None))
            {
                skinIds.Add(int.Parse(match.Groups[1].Value));
            }
            SaveConfigSkinIds();
            MessageBox.Show("获取皮肤数据完成");
            FilterSkins();
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

        private void toolStripMenuItem_FilterSkins_Click(object sender, EventArgs e)
        {
            FilterSkins();
        }

        private static void FilterSkins()
        {
            string skin404 = File.ReadAllText(@"file\txt\Skin404.txt");
            string[] skins = skin404.Split(',');
            HashSet<int> set = new HashSet<int>();
            foreach (string s in skins)
            {
                if (s != "") set.Add(int.Parse(s));
            }
            skinIds.RemoveAll(data => set.Contains(data));

            string blackList = Properties.Settings.Default.SkinBlackList;
            string[] blacks = skin404.Split(',');
            HashSet<int> set1 = new HashSet<int>();
            foreach (string s in blacks)
            {
                if (s != "") set1.Add(int.Parse(s));
            }
            skinIds.RemoveAll(data => set1.Contains(data));

            // 452 及以前的精灵皮肤，脚本结构与之后的精灵略有不同，替换上去可能会卡
            skinIds.RemoveAll(data => data < 453);

            SaveConfigSkinIds();
            MessageBox.Show("筛选完成");
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
            if (chileFormPetBag == null)
            {
                chileFormPetBag = new FormPetBag();
                chileFormPetBag.Show();
            }
        }
    }
}

using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    public partial class FormConfig : Form
    {
        private Properties.Settings SettingsDef = Properties.Settings.Default;

        public FormConfig()
        {
            InitializeComponent();
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            // 初始化配置项
            checkBox_RandomSkin.Checked = SettingsDef.IsRandomSkin;
            numericUpDown_skinCeiling.Value = SettingsDef.SkinRangeCeiling;
            numericUpDown_skinFloor.Value = SettingsDef.SkinRangeFloor;
            richTextBox_skinList.Text = SettingsDef.SkinIds;
            richTextBox_SkinBlackList.Text = SettingsDef.SkinBlackList;
            richTextBox_SkinExc.Text = SettingsDef.RandomSkinExclusion;
            comboBox_font.Text = SettingsDef.BrowserFont;
            numericUpDown_win_width.Value = SettingsDef.WinWidth;
            numericUpDown_win_height.Value = SettingsDef.WinHeight;
            checkBox_resource_ad_panel.Checked = SettingsDef.IsChangeAdPanel;
            checkBox_resource_background.Checked = SettingsDef.IsChangeBackground;
            checkBox_resource_vip_icon.Checked = SettingsDef.IsChangeVipIcon;
            checkBox_flash_pack.Checked = SettingsDef.IsUseSocketHack;
            checkBox_fd.Checked = SettingsDef.AutoLoadFD;
            // 遍历系统字体
            foreach (var f in FontFamily.Families)
            {
                comboBox_font.Items.Add(f.Name);
            }
            textBox1.Text = SettingsDef.AutoExecuteSoftwarePath1;
            textBox2.Text = SettingsDef.AutoExecuteSoftwarePath2;
            textBox3.Text = SettingsDef.AutoExecuteSoftwarePath3;
            textBox_defaultURL.Text = SettingsDef.DefaultURL;
            comboBoxZoom.Text = SettingsDef.FlashZoom;
            comboBoxZoom.Items.Add("0.5");
            comboBoxZoom.Items.Add("0.75");
            comboBoxZoom.Items.Add("1");
            comboBoxZoom.Items.Add("1.25");
            comboBoxZoom.Items.Add("1.5");
            comboBoxZoom.Items.Add("1.75");
            comboBoxZoom.Items.Add("2");
            textBox_speedUp.Text = File.ReadAllText(@"file\dll\speedhack\x64\speedhack.txt");
            // 加载窗口
            checkBox_flash_activities.Checked = SettingsDef.AutoLoadActivities;
            checkBox_flash_fight.Checked = SettingsDef.AutoLoadFightHandler;
            checkBox_h5_pack.Checked = SettingsDef.AutoLoadH5Pack;
            checkBox_h5_pet_bag.Checked = SettingsDef.AutoLoadPetBag;
            checkBox_screen_shot.Checked = SettingsDef.AutoLoadScreenShot;
            checkBox_flash_map.Checked = SettingsDef.AutoLoadFlashMap;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            // 检查字符串格式是否正确
            string pattern = "^[0-9,]*$";
            var m = Regex.Match(richTextBox_SkinBlackList.Text, pattern);
            if(m.Success)
            {
                SettingsDef.SkinBlackList = richTextBox_SkinBlackList.Text;
            }
            else
            {
                MessageBox.Show("皮肤黑名单格式错误");
                return;
            }
            m = Regex.Match(richTextBox_SkinExc.Text, pattern);
            if (m.Success)
            {
                SettingsDef.RandomSkinExclusion = richTextBox_SkinExc.Text;
            }
            else
            {
                MessageBox.Show("换肤排除项格式错误");
                return;
            }
            m = Regex.Match(richTextBox_skinList.Text, pattern);
            if (m.Success)
            {
                SettingsDef.SkinIds = richTextBox_skinList.Text;
            }
            else
            {
                MessageBox.Show("皮肤列表格式错误");
                return;
            }
            SettingsDef.IsRandomSkin = checkBox_RandomSkin.Checked;
            SettingsDef.WinWidth = numericUpDown_win_width.Value;
            SettingsDef.WinHeight = numericUpDown_win_height.Value;
            SettingsDef.SkinRangeCeiling = (int)numericUpDown_skinCeiling.Value;
            SettingsDef.SkinRangeFloor = (int)numericUpDown_skinFloor.Value;
            SettingsDef.IsChangeAdPanel = checkBox_resource_ad_panel.Checked;
            SettingsDef.IsChangeBackground = checkBox_resource_background.Checked;
            SettingsDef.IsChangeVipIcon = checkBox_resource_vip_icon.Checked;
            SettingsDef.IsUseSocketHack = checkBox_flash_pack.Checked;
            SettingsDef.BrowserFont = comboBox_font.Text;
            SettingsDef.AutoExecuteSoftwarePath1 = textBox1.Text;
            SettingsDef.AutoExecuteSoftwarePath2 = textBox2.Text;
            SettingsDef.AutoExecuteSoftwarePath3 = textBox3.Text;
            SettingsDef.DefaultURL = textBox_defaultURL.Text;
            SettingsDef.FlashZoom = comboBoxZoom.Text;

            if (double.TryParse(textBox_speedUp.Text, out var speed) && speed >= 1)
            {
                File.WriteAllText(@"file\dll\speedhack\x64\speedhack.txt", textBox_speedUp.Text);
            }

            // 窗口
            SettingsDef.AutoLoadActivities = checkBox_flash_activities.Checked;
            SettingsDef.AutoLoadFightHandler = checkBox_flash_fight.Checked;
            SettingsDef.AutoLoadH5Pack = checkBox_h5_pack.Checked;
            SettingsDef.AutoLoadPetBag = checkBox_h5_pet_bag.Checked;
            SettingsDef.AutoLoadScreenShot = checkBox_screen_shot.Checked;
            SettingsDef.AutoLoadFlashMap = checkBox_flash_map.Checked;
            SettingsDef.AutoLoadFD = checkBox_fd.Checked;

            SettingsDef.Save();
            MessageBox.Show("保存成功");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "可执行程序（.exe）|*.exe" };
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox_skinList.Text = "1,4,8,29,50,104,164,165,166,171,172,174,181,183,192,193,195,224,227,278,306,309,347,353,438,439,447,454,455,468,469,470,490,498,501,503,508,510,511,515,518,526,530,531,532,544,569,570,584,590,601,603,612,613,614,643,661,695,697,698,729,760,780,781,782,783,784,796,798,799,804,810,811,678,820,821,834,864,504,505,875,880,881,886,682,683,905,908,923,927,928,945,950,957,960,961,962,965,974,977,987,998,999,1000,1003,1011,1012,1017,1018,1019,1020,1029,1045,1061,1086,1087,1093,1109,1111,1115,1121,1122,1155,1156,1165,1166,1167,1168,1177,1179,1187,1189,1201,1202,1204,1215,1256,1260,1287,1288,1290,1306,1327,1336,1355,1394,1412,1446,1447,1449,1456,1526,1527,1533,1534,1537,1567,1568,1569,1580,1587,1588,1630,1631,1632,1648,1651,1656,1657,1669,1678,1679,1680,1681,1665,1700,1715,1717,1730,1745,1746,1748,1756,1776,1777,1802,1806,1809,1815,1819,1825,1836,1840,1851,1864,1861,1910,1944,1945,1951,1955,1956,1960,1971,1972,2000,2001,2006,2034,2045,2048,2049,2054,2073,2074,2085,2147,2149,2156,2165,2167,2168,2172,2173,2174,2175,2178,2186,2197,2206,2218,2219,2234,2242,2247,2250,2257,2262,2283,2288,2310,2321,2325,2326,2327,2329,2333,2347,2349,2358,2363,2375,2376,2377,2387,2394,2400,2402,2407,2413,2429,2430,2437,2438,2441,2442,2447,2462,2468,2469,2471,2488,2500,2502,2503,2508,2516,2519,2520,2521,2533,2535,2545,2580,2591,2606,2607,2612,2613,2619,2620,2625,2634,2635,2636,2643,2648,2652,2655,2661,2662,2665,2673,2676,2678,2679,2681,2684,2691,2703,2708,2710,2733,2736,2744,2748,2761,2769,2780,2786,2787,2793,2794,2796,2800,2811,2813,2821,2824,2825,2829,2835,2837,2839,2840,2842,2843,2849,2850,2852,2853,2858,2859,2861,2862,2873,2875,2879,2883,2887,2888,2893,2906,2915,2918,2925,2935,2937,2951,2953,2955,2967,2972,2982,2988,2991,3001,3003,3017,3038,3042,3043,3044,3050,3057,3066,3074,3075,3081,3082,3087,3092,3094,3097,3102,3103,3104,3105,3110,3111,3112,3113,3117,3118,3122,3128,3129,3134,3137,3140,3141,3146,3147,3150,3156,3163,3164,3168,3171,3179,3190,3192,3193,3194,3196,3204,3205,3210,3214,3216,3222,3224,3227,3228,3229,3232,3233,3234,3237,3239,3240,3243,3249,3256,3261,3267,3269,3272,3273,3289,3290,3293,3294,3295,3296,3302,3303,3304,3305,3307,3308,3309,3311,3312,3315,3318,3320,3321,3322,3324,3325,3326,3327,3328,3333,3336,3340,3343,3346,3353,3354,3355,3356,3357,3358,3373,3374,3375,3378,3380,3382,3383,3388,3392,3395,3397,3402,3403,3404,3414,3418,3421,3425,3430,3435,3436,3437,3439,3440,3443,3446,3455,3456,3458,3461,3463,3465,3478,3479,3481,3486,3487,3488,3490,3491,3502,3503,3504,3506,3510,3511,3516,3525,3532,3535,3539,3545,3547,3552,3554,3567,3568,3569,3570,3571,3575,3577,3578,3580,3590,3612,3615,3623,3625,3626,3627,3631,3642,3643,3644,3645,3649,3655,3663,3664,3665,3667,3668,3669,3676,3679,3683,3685,3691,3693,3694,3701,3703,3704,3711,3717,3724,3737,3740,3741,3742,3743,3748,3753,3758,3774,3776,3778,3780,3783,3786,3790,3792,3797,3803,3807,3808,3810,3813,3816,3820,3830,3833,3841,3842,3845,3857,3858,3861,3864,3866,3867,3873,3875,3878,3879,3883,3888,3889,3891,3892,3897,3898,3899,3914,3919,3920,3926,3928,3929,3937,3939,3940,3941,3945,3949,3957,3959,3961,3964,3965,3983,3986,3987,3992,3995,3998,4004,4005,4007,4009,4015,4017,4018,4022,4024,4027,4028,4032,4035,4037,4038,4041,4042,4046,4052,4054,4055,4057,4061,4064,4065,4068,4074,4081,4086,4088,4093,4097,4101,4106,4107,4109,4110,4113,4114,4115,4123,4124,4125,4131,4132,4135,4139,4143,4146,4147,4151,4154,4156,4159,4160,4163,4165,4166,4173,4174,4177,4179,4182,4184,4186,4187,4188,4189,4193,4209,4212,4215,4219,4221,4222,4231,4233,4237,4240,4241,4242,4246,4247,4249,4250,4251,4252,4253,4254,4261,4262,4268,4272,4273,4275,4277,4280,4284,4286,4287,4288,4289,4290,4291,4295,4300,4301,4302,4303,4304,4305,4306,4307,4309,4310,4313,4318,4319,4321,4322,4324,4325,4327,4328,4329,4330,4331,4333,4334,4338,4340,4342,4345,4347,4352,4353,4356,4358,4360,4364,4365,4372,4373,4374,4385,4389,4390,4391,4392,4397,4401,4402,4407,4408,4410,4412,4413,4417,4420,4421,4423,4424,4426,4429,4436,4438,4440,4441,4442,4446,4448,4449,4451,4455,4456,4458,4459,4461,4462,4465,4466,4467,4469,4472,4477,4480,4481,4482,4483,4484,4488,4490,4491,4492,4496,4498,4499,4500,4502,4504,4505,4509,4510,4511,4514,4516,4518,4519,4522,4524,4526,4527,4529,4530,4531,4534,4535,4538,4539,4546,4547,4548,4551,4552,4553,4554,4555,4557,4559,4560,4562,4566,4567,4569,4572,4573,4574,4577,4582,4584,4585,4587,4588,4589,4591,4593,4594,4595,4597,4598,4600,4604,4605,4606,4609,4612,4613,4617,4619,4620,4622,4623,4625,4627,4628,4630,4634,4635,4636,4642,4643,4644,4646,4648,4649,4650,4651,4652,4653,4654,4655,4656,4658,4661,4662,4663,4667,4669,4671,4672,4674,4676,4677,4678,4679,4680,4681,4683,4684,4685,4686,4687,4688,4689,4691,4706,4693,4694,4695,4696,4697,4698,4699,4700,4702,4703,5002,5005,5006,5008,5009,5010,5011,5013,5014,5015,5016,5017,5018,5019,5021,5024,5025,5027,5030,5031,5032,5033,5036,5039,5042,5044,5045,5046,5047,5052,5053,5055,5056,5057,5058,5059,5060,5061,5062,5063,5064,5069,5071,5072,5073,5074,5075,5111,5357,5403,5404,5494,5498,5503,5509,5601,5605,5606,5607,5608,5693,5713,5772,5773,5774,5776,5785,5786,5787,5788,5789,5790,5796,5797,5819,5820,5821,5824,1400126,1400130,1400132,1400138,1400139,1400141,1400142,1400143,1400148,1400149,1400150,1400153,1400155,1400158,1400159,1400160,1400161,1400165,1400166,1400168,1400170,1400174,1400176,1400178,1400179,1400182,1400183,1400185,1400187,1400189,1400190,1400193,1400194,1400195,1400204,1400213,1400220,1400224,1400225,1400231,1400233,1400241,1400242,1400246,1400247,1400249,1400250,1400252,1400253,1400261,1400262,1400268,1400269,1400280,1400281,1400283,1400284,1400286,1400287,1400288,1400294,1400295,1400297,1400299,1400301,1400302,1400304,1400305,1400306,1400307,1400308,1400309,1400311,1400313,1400314,1400315,1400317,1400318,1400321,1400322,1400325,1400327,1400328,1400329,1400330,1400332,1400333,1400334,1400335,1400336,1400340,1400342,1400344,1400345,1400346,1400347,1400348,1400351,1400352,1400354,1400356,1400359,1400360,1400365,1400367,1400369,1400371,1400372,1400374,1400375,1400377,1400378,1400379,1400380,1400382,1400383,1400384,1400385,1400386,1400387,1400392,1400394,1400395,1400396,1400398,1400404,1400409,1400410,1400411,1400412,1400413,1400414,1400415,1400416,1400417,1400419,1400420,1400421,1400422,1400427,1400430,1400431,1400433,1400434,1400436,1400437,1400438,1400440,1400442,1400445,1400446,1400447,1400448,1400450,1400454,1400455,1400457,1400460,1400464,1400465,1400467,1400468,1400469,1400470,1400471,1400472,1400473,1400474,1400475,1400478,1400479,1400480,1400481,1400482,1400486,1400487,1400489,1400490,1400491,1400492,1400493,1400495,1400496,1400497,1400501,1400502,1400507,1400508,1400510,1400511,1400514,1400516,1400517,1400519,1400520,1400522,1400523,1400525,1400527,1400530,1400531,1400532,1400534,1400535,1400536,1400540,1400541,1400542,1400543,1400544,1400546,1400547,1400548,1400549,1400552,1400553,1400555,1400556,1400559,1400560,1400561,1400563,1400564,1400565,1400566,1400567,1400568,1400569,1400570,1400571,1400572,1400575,1400576,1400578,1400579,1400580,1400582,1400583,1400584,1400587,1400588,1400589,1400592,1400594,1400595,1400596,1400599,1400601,1400604,1400605,1400606,1400607,1400608,1400611,1400613,1400616,1400619,1400620,1400621,1400622,1400623,1400634,1400635,1400636,1400639,1400640,1400641,1400642,1400644,1400646,1400650,1400652,1400653,1400654,1400658,1400659,1400660,1400661,1400662,1400664,1400665,1400666,1400667,1400669,1400670,1400671,1400674,1400675,1400677,1400678,";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "可执行程序（.exe）|*.exe" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "可执行程序（.exe）|*.exe" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = dialog.FileName;
            }
        }
    }
}

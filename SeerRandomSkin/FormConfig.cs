using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            checkBox_h5_first.Checked = SettingsDef.IsH5First;
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
            checkBox_resource_bg_h5.Checked = SettingsDef.IsChangeH5LoginBg2024;
            checkBox_flash_pack.Checked = SettingsDef.IsUseSocketHack;
            // 遍历系统字体
            foreach (var f in FontFamily.Families)
            {
                comboBox_font.Items.Add(f.Name);
            }
            textBox1.Text = SettingsDef.AutoExecuteSoftwarePath1;
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
            SettingsDef.IsH5First = checkBox_h5_first.Checked;
            SettingsDef.WinWidth = numericUpDown_win_width.Value;
            SettingsDef.WinHeight = numericUpDown_win_height.Value;
            SettingsDef.SkinRangeCeiling = (int)numericUpDown_skinCeiling.Value;
            SettingsDef.SkinRangeFloor = (int)numericUpDown_skinFloor.Value;
            SettingsDef.IsChangeAdPanel = checkBox_resource_ad_panel.Checked;
            SettingsDef.IsChangeBackground = checkBox_resource_background.Checked;
            SettingsDef.IsChangeVipIcon = checkBox_resource_vip_icon.Checked;
            SettingsDef.IsChangeH5LoginBg2024 = checkBox_resource_bg_h5.Checked;
            SettingsDef.IsUseSocketHack = checkBox_flash_pack.Checked;
            SettingsDef.BrowserFont = comboBox_font.Text;
            SettingsDef.AutoExecuteSoftwarePath1 = textBox1.Text;
            SettingsDef.Save();
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
            richTextBox_skinList.Text = "1,2,3,4,5,6,7,8,9,29,50,104,125,164,165,166,169,170,171,172,173,174,175,181,182,183,191,192,193,194,195,223,224,227,278,306,309,347,351,353,438,439,445,446,447,448,449,454,455,468,469,470,488,489,490,497,498,499,500,501,503,507,508,509,510,511,512,513,514,515,516,517,518,519,520,526,527,528,529,530,531,532,533,534,535,536,537,538,544,545,546,564,565,566,568,569,570,583,584,590,591,601,602,603,612,613,614,643,644,645,658,659,660,661,694,695,696,697,698,729,760,779,780,781,782,783,784,796,797,798,799,801,802,803,804,809,810,811,677,678,679,820,821,833,834,845,846,864,865,504,505,875,878,880,881,680,681,884,886,682,683,904,905,908,686,922,923,926,927,928,944,945,946,947,950,956,957,960,961,962,963,964,965,970,973,974,977,978,979,986,987,997,998,999,1000,1002,1003,1011,1012,1017,1018,1019,1020,1021,1028,1029,1042,1043,1044,1045,1060,1061,1062,1063,1064,1086,1087,1092,1093,1108,1109,1110,1111,1114,1115,1118,1119,1120,1121,1122,1152,1153,1154,1155,1156,1157,1158,1161,1162,1163,1164,1165,1166,1167,1168,1169,1170,1171,1173,1174,1175,1176,1177,1178,1179,1180,1181,1182,1186,1187,1188,1189,1190,1191,1192,1199,1200,1201,1202,1203,1204,1211,1212,1213,1214,1215,1216,1219,1220,1222,1223,1226,1227,1230,1232,1237,1238,1239,1240,1241,1243,1244,1245,1247,1248,1249,1251,1253,1254,1255,1256,1257,1258,1259,1260,1261,1263,1264,1265,1266,1273,1274,1275,1276,1285,1286,1287,1288,1289,1290,1299,1300,1305,1306,1315,1316,1317,1320,1321,1322,1323,1326,1327,1328,1331,1332,1333,1334,1335,1336,1337,1340,1341,1346,1353,1354,1355,1356,1357,1358,1361,1372,1373,1394,1395,1400,1402,1412,1414,1419,1420,1421,1436,1437,1446,1447,1448,1449,1454,1455,1456,1460,1483,1484,1500,1501,1502,1505,1506,1518,1524,1525,1526,1527,1531,1532,1533,1534,1537,1538,1547,1548,1551,1553,1554,1555,1556,1561,1562,1565,1566,1567,1568,1569,1570,1571,1572,1579,1580,1581,1582,1587,1588,1589,1590,1597,1598,1601,1602,1603,1605,1606,1607,1608,1609,1610,1611,1612,1613,1614,1615,1616,1617,1619,1620,1621,1624,1630,1631,1632,1633,1634,1636,1637,1638,1639,1640,1641,1644,1647,1648,1649,1650,1651,1654,1655,1656,1657,1662,1667,1668,1669,1670,1671,1678,1679,1680,1681,1664,1665,1692,1693,1695,1696,1697,1698,1699,1700,1701,1706,1707,1708,1709,1710,1711,1714,1715,1716,1717,1718,1719,1722,1723,1728,1729,1730,1731,1732,1735,1736,1737,1738,1745,1746,1747,1748,1750,1751,1752,1755,1756,1759,1766,1767,1768,1772,1773,1776,1777,1780,1781,1782,1783,1784,1791,1792,1793,1794,1797,1798,1800,1801,1802,1803,1804,1806,1808,1809,1810,1811,1812,1813,1814,1815,1818,1819,1820,1821,1822,1823,1824,1825,1826,1830,1831,1832,1834,1836,1837,1840,1841,1842,1845,1846,1847,1848,1849,1850,1851,1852,1853,1854,1855,1856,1857,1860,1862,1863,1864,1865,1829,1861,1869,1870,1871,1872,1877,1878,1879,1881,1883,1884,1885,1886,1887,1888,1889,1890,1891,1892,1893,1894,1897,1902,1903,1904,1905,1906,1907,1908,1909,1910,1911,1914,1915,1916,1917,1918,1922,1923,1924,1926,1927,1928,1929,1930,1931,1932,1933,1937,1938,1939,1940,1941,1942,1943,1944,1945,1946,1948,1949,1950,1951,1954,1955,1956,1957,1958,1959,1960,1961,1963,1964,1965,1967,1968,1970,1971,1972,1973,1976,1977,1978,1979,1980,1981,1982,1983,1984,1985,1988,1989,1990,1991,1998,1999,2000,";
        }
    }
}

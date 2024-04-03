using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace SeerRandomSkin
{
    public partial class FormPetBag : Form
    {
        private static string ClothName = "";

        public static string seer_cloth = "";
        public static string SeerCloth
        {
            get { return seer_cloth; }
            set
            {
                seer_cloth = value;
                // 在配置文件中添加用户的装备信息
                jObj_clothes[ClothName] = value;
                Properties.Settings.Default.SeerCloth = jObj_clothes.ToString();
                Properties.Settings.Default.Save();
                MessageBox.Show("【" + ClothName + "】添加成功");
                //MessageBox.Show(Properties.Settings.Default.SeerCloth);
            }
        }

        private static JObject jObj_clothes;

        public FormPetBag()
        {
            InitializeComponent();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (textBox_Name.Text == "") return;

            ClothName = textBox_Name.Text;
            Form1.chromiumBrowser.GetMainFrame().ExecuteJavaScriptAsync(
                "window.seer_cloth_obj = {};" +
                // 获取背包
                "seer_cloth_obj.pet_bag1 = [];" +
                "var clear_pet_bag_tmp_arr = PetManager.getBagMap();" +
                "for (var i in clear_pet_bag_tmp_arr) { seer_cloth_obj.pet_bag1.push(clear_pet_bag_tmp_arr[i].id); }" +
                "seer_cloth_obj.pet_bag2 = [];" +
                "clear_pet_bag_tmp_arr = PetManager.getSecondBagMap();" +
                "for (var i in clear_pet_bag_tmp_arr) { seer_cloth_obj.pet_bag2.push(clear_pet_bag_tmp_arr[i].id); }" +
                // 套装
                "seer_cloth_obj.clothes = [];" +
                "for (var i in MainManager.actorInfo.clothes) { seer_cloth_obj.clothes.push(MainManager.actorInfo.clothes[i].id); }" +
                // 称号
                "seer_cloth_obj.curTitle = MainManager.actorInfo.curTitle;" +
                // 保存
                "seerRandomSkinObj.getCloth(JSON.stringify(seer_cloth_obj));"
                );
        }

        private void FormPetBag_Load(object sender, EventArgs e)
        {
            InitListView();
        }

        private void InitListView()
        {
            listView1.Items.Clear();
            jObj_clothes = JObject.Parse(Properties.Settings.Default.SeerCloth);
            var properties = jObj_clothes.Properties();
            foreach (var property in properties)
            {
                ListViewItem item = new ListViewItem
                {
                    Text = property.Name
                };
                item.SubItems.Add(property.Value.ToString());

                listView1.Items.Add(item);
            }
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            InitListView();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idxs = listView1.SelectedIndices;
            if (idxs.Count == 1)
            {
                textBox_Name.Text = listView1.Items[idxs[0]].SubItems[0].Text;
            }
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            jObj_clothes.Remove(textBox_Name.Text);
            Properties.Settings.Default.SeerCloth = jObj_clothes.ToString();
            Properties.Settings.Default.Save();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            Form1.chromiumBrowser.GetMainFrame().ExecuteJavaScriptAsync(
                "window.seer_cloth_obj = JSON.parse('" + jObj_clothes[textBox_Name.Text].ToString() + "');" +
                // 清空背包
                "var clear_pet_bag_tmp_arr = PetManager.getBagMap();" +
                "for (var i in clear_pet_bag_tmp_arr) { PetManager.bagToStorage(clear_pet_bag_tmp_arr[i].catchTime); }" +
                "clear_pet_bag_tmp_arr = PetManager.getSecondBagMap();" +
                "for (var i in clear_pet_bag_tmp_arr) { PetManager.secondBagToStorage(clear_pet_bag_tmp_arr[i].catchTime); }" +
                // 换精灵
                "for (var i in seer_cloth_obj.pet_bag1) {" +
                "   var pet_info_storage_got_by_id = PetStorage2015InfoManager.getInfoByID(seer_cloth_obj.pet_bag1[i]);" +
                "   if (pet_info_storage_got_by_id.length > 0) { SocketConnection.sendByQueue(CommandID.PET_RELEASE, [pet_info_storage_got_by_id[0].catchTime, 1]); }" +
                "}" +
                "for (var i in seer_cloth_obj.pet_bag2) {" +
                "   var pet_info_storage_got_by_id = PetStorage2015InfoManager.getInfoByID(seer_cloth_obj.pet_bag2[i]);" +
                "   if (pet_info_storage_got_by_id.length > 0) { SocketConnection.sendByQueue(CommandID.PET_RELEASE, [pet_info_storage_got_by_id[0].catchTime, 2]); }" +
                "}" +
                // 套装
                "if (seer_cloth_obj.clothes.length > 0) {" +
                "   SocketConnection.send(CommandID.CHANGE_CLOTH, seer_cloth_obj.clothes.length, seer_cloth_obj.clothes);" +
                "}" +
                // 称号
                "SocketConnection.send(3404, seer_cloth_obj.curTitle);" +
                "Alarm.show('" + textBox_Name.Text + "');"
                );
        }
    }
}

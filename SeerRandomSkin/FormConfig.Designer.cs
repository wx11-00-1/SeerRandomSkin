namespace SeerRandomSkin
{
    partial class FormConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Save = new System.Windows.Forms.Button();
            this.richTextBox_SkinBlackList = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_font = new System.Windows.Forms.ComboBox();
            this.checkBox_h5_first = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown_win_width = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_win_height = new System.Windows.Forms.NumericUpDown();
            this.checkBox_RandomSkin = new System.Windows.Forms.CheckBox();
            this.checkBox_onlyOldPet = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_resource_vip_icon = new System.Windows.Forms.CheckBox();
            this.checkBox_resource_ad_panel = new System.Windows.Forms.CheckBox();
            this.checkBox_resource_background = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_win_width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_win_height)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(536, 528);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 23);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // richTextBox_SkinBlackList
            // 
            this.richTextBox_SkinBlackList.Location = new System.Drawing.Point(53, 280);
            this.richTextBox_SkinBlackList.Name = "richTextBox_SkinBlackList";
            this.richTextBox_SkinBlackList.Size = new System.Drawing.Size(436, 271);
            this.richTextBox_SkinBlackList.TabIndex = 2;
            this.richTextBox_SkinBlackList.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "皮肤黑名单（格式如下）：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "123,124,125,";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(321, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "字体：";
            // 
            // comboBox_font
            // 
            this.comboBox_font.FormattingEnabled = true;
            this.comboBox_font.Location = new System.Drawing.Point(368, 41);
            this.comboBox_font.Name = "comboBox_font";
            this.comboBox_font.Size = new System.Drawing.Size(121, 20);
            this.comboBox_font.TabIndex = 6;
            // 
            // checkBox_h5_first
            // 
            this.checkBox_h5_first.AutoSize = true;
            this.checkBox_h5_first.Location = new System.Drawing.Point(173, 41);
            this.checkBox_h5_first.Name = "checkBox_h5_first";
            this.checkBox_h5_first.Size = new System.Drawing.Size(108, 17);
            this.checkBox_h5_first.TabIndex = 7;
            this.checkBox_h5_first.Text = "优先进入h5端";
            this.checkBox_h5_first.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(273, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "默认窗口大小：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(423, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "x";
            // 
            // numericUpDown_win_width
            // 
            this.numericUpDown_win_width.Location = new System.Drawing.Point(368, 81);
            this.numericUpDown_win_width.Maximum = new decimal(new int[] {
            40000,
            0,
            0,
            0});
            this.numericUpDown_win_width.Name = "numericUpDown_win_width";
            this.numericUpDown_win_width.Size = new System.Drawing.Size(45, 21);
            this.numericUpDown_win_width.TabIndex = 11;
            // 
            // numericUpDown_win_height
            // 
            this.numericUpDown_win_height.Location = new System.Drawing.Point(444, 81);
            this.numericUpDown_win_height.Maximum = new decimal(new int[] {
            40000,
            0,
            0,
            0});
            this.numericUpDown_win_height.Name = "numericUpDown_win_height";
            this.numericUpDown_win_height.Size = new System.Drawing.Size(45, 21);
            this.numericUpDown_win_height.TabIndex = 12;
            // 
            // checkBox_RandomSkin
            // 
            this.checkBox_RandomSkin.AutoSize = true;
            this.checkBox_RandomSkin.Location = new System.Drawing.Point(53, 41);
            this.checkBox_RandomSkin.Name = "checkBox_RandomSkin";
            this.checkBox_RandomSkin.Size = new System.Drawing.Size(107, 17);
            this.checkBox_RandomSkin.TabIndex = 0;
            this.checkBox_RandomSkin.Text = "启用随机皮肤";
            this.checkBox_RandomSkin.UseVisualStyleBackColor = true;
            // 
            // checkBox_onlyOldPet
            // 
            this.checkBox_onlyOldPet.AutoSize = true;
            this.checkBox_onlyOldPet.Location = new System.Drawing.Point(53, 79);
            this.checkBox_onlyOldPet.Name = "checkBox_onlyOldPet";
            this.checkBox_onlyOldPet.Size = new System.Drawing.Size(198, 17);
            this.checkBox_onlyOldPet.TabIndex = 13;
            this.checkBox_onlyOldPet.Text = "仅用老精灵皮肤，出招不墨迹";
            this.checkBox_onlyOldPet.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_resource_vip_icon);
            this.groupBox1.Controls.Add(this.checkBox_resource_ad_panel);
            this.groupBox1.Controls.Add(this.checkBox_resource_background);
            this.groupBox1.Location = new System.Drawing.Point(54, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 54);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "替换";
            // 
            // checkBox_resource_vip_icon
            // 
            this.checkBox_resource_vip_icon.AutoSize = true;
            this.checkBox_resource_vip_icon.Location = new System.Drawing.Point(201, 22);
            this.checkBox_resource_vip_icon.Name = "checkBox_resource_vip_icon";
            this.checkBox_resource_vip_icon.Size = new System.Drawing.Size(81, 17);
            this.checkBox_resource_vip_icon.TabIndex = 17;
            this.checkBox_resource_vip_icon.Text = "年费图标";
            this.checkBox_resource_vip_icon.UseVisualStyleBackColor = true;
            // 
            // checkBox_resource_ad_panel
            // 
            this.checkBox_resource_ad_panel.AutoSize = true;
            this.checkBox_resource_ad_panel.Location = new System.Drawing.Point(93, 22);
            this.checkBox_resource_ad_panel.Name = "checkBox_resource_ad_panel";
            this.checkBox_resource_ad_panel.Size = new System.Drawing.Size(81, 17);
            this.checkBox_resource_ad_panel.TabIndex = 16;
            this.checkBox_resource_ad_panel.Text = "选服广告";
            this.checkBox_resource_ad_panel.UseVisualStyleBackColor = true;
            // 
            // checkBox_resource_background
            // 
            this.checkBox_resource_background.AutoSize = true;
            this.checkBox_resource_background.Location = new System.Drawing.Point(22, 22);
            this.checkBox_resource_background.Name = "checkBox_resource_background";
            this.checkBox_resource_background.Size = new System.Drawing.Size(55, 17);
            this.checkBox_resource_background.TabIndex = 15;
            this.checkBox_resource_background.Text = "背景";
            this.checkBox_resource_background.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "自动打开 FD 或 YOSO：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(53, 197);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(340, 21);
            this.textBox1.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(414, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "选择文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 585);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox_onlyOldPet);
            this.Controls.Add(this.numericUpDown_win_height);
            this.Controls.Add(this.numericUpDown_win_width);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBox_h5_first);
            this.Controls.Add(this.comboBox_font);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_SkinBlackList);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.checkBox_RandomSkin);
            this.Name = "FormConfig";
            this.ShowIcon = false;
            this.Text = "配置";
            this.Load += new System.EventHandler(this.FormConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_win_width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_win_height)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.RichTextBox richTextBox_SkinBlackList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_font;
        private System.Windows.Forms.CheckBox checkBox_h5_first;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown_win_width;
        private System.Windows.Forms.NumericUpDown numericUpDown_win_height;
        private System.Windows.Forms.CheckBox checkBox_RandomSkin;
        private System.Windows.Forms.CheckBox checkBox_onlyOldPet;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_resource_vip_icon;
        private System.Windows.Forms.CheckBox checkBox_resource_ad_panel;
        private System.Windows.Forms.CheckBox checkBox_resource_background;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
    }
}
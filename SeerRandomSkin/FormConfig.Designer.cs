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
            this.checkBox_RandomSkin = new System.Windows.Forms.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_win_width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_win_height)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_RandomSkin
            // 
            this.checkBox_RandomSkin.AutoSize = true;
            this.checkBox_RandomSkin.Location = new System.Drawing.Point(53, 41);
            this.checkBox_RandomSkin.Name = "checkBox_RandomSkin";
            this.checkBox_RandomSkin.Size = new System.Drawing.Size(72, 16);
            this.checkBox_RandomSkin.TabIndex = 0;
            this.checkBox_RandomSkin.Text = "随机皮肤";
            this.checkBox_RandomSkin.UseVisualStyleBackColor = true;
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(546, 382);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 23);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // richTextBox_SkinBlackList
            // 
            this.richTextBox_SkinBlackList.Location = new System.Drawing.Point(53, 134);
            this.richTextBox_SkinBlackList.Name = "richTextBox_SkinBlackList";
            this.richTextBox_SkinBlackList.Size = new System.Drawing.Size(436, 271);
            this.richTextBox_SkinBlackList.TabIndex = 2;
            this.richTextBox_SkinBlackList.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "皮肤黑名单（格式如下）：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "123,124,125,";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(321, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
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
            this.checkBox_h5_first.Location = new System.Drawing.Point(152, 41);
            this.checkBox_h5_first.Name = "checkBox_h5_first";
            this.checkBox_h5_first.Size = new System.Drawing.Size(96, 16);
            this.checkBox_h5_first.TabIndex = 7;
            this.checkBox_h5_first.Text = "优先进入h5端";
            this.checkBox_h5_first.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(273, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "默认窗口大小：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(423, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "x";
            // 
            // numericUpDown_win_width
            // 
            this.numericUpDown_win_width.Location = new System.Drawing.Point(368, 84);
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
            this.numericUpDown_win_height.Location = new System.Drawing.Point(444, 84);
            this.numericUpDown_win_height.Maximum = new decimal(new int[] {
            40000,
            0,
            0,
            0});
            this.numericUpDown_win_height.Name = "numericUpDown_win_height";
            this.numericUpDown_win_height.Size = new System.Drawing.Size(45, 21);
            this.numericUpDown_win_height.TabIndex = 12;
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 450);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_RandomSkin;
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
    }
}
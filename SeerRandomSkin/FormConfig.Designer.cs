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
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_SkinBlackList);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.checkBox_RandomSkin);
            this.Name = "FormConfig";
            this.Text = "配置";
            this.Load += new System.EventHandler(this.FormConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_RandomSkin;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.RichTextBox richTextBox_SkinBlackList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
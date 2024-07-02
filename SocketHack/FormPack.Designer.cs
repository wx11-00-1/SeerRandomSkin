namespace SocketHack
{
    partial class FormPack
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPack));
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxHideSend = new System.Windows.Forms.CheckBox();
            this.checkBoxHideRecv = new System.Windows.Forms.CheckBox();
            this.listViewPack = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rtbMsg = new System.Windows.Forms.RichTextBox();
            this.tbPackStr = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_fightSkill0 = new System.Windows.Forms.Button();
            this.button_fightExit = new System.Windows.Forms.Button();
            this.button_fightCatch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(402, 281);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "复制";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(498, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBoxHideSend
            // 
            this.checkBoxHideSend.AutoSize = true;
            this.checkBoxHideSend.Location = new System.Drawing.Point(121, 281);
            this.checkBoxHideSend.Name = "checkBoxHideSend";
            this.checkBoxHideSend.Size = new System.Drawing.Size(72, 16);
            this.checkBoxHideSend.TabIndex = 7;
            this.checkBoxHideSend.Text = "屏蔽发包";
            this.checkBoxHideSend.UseVisualStyleBackColor = true;
            this.checkBoxHideSend.Click += new System.EventHandler(this.checkBoxHideSend_Click);
            // 
            // checkBoxHideRecv
            // 
            this.checkBoxHideRecv.AutoSize = true;
            this.checkBoxHideRecv.Location = new System.Drawing.Point(43, 281);
            this.checkBoxHideRecv.Name = "checkBoxHideRecv";
            this.checkBoxHideRecv.Size = new System.Drawing.Size(72, 16);
            this.checkBoxHideRecv.TabIndex = 6;
            this.checkBoxHideRecv.Text = "屏蔽收包";
            this.checkBoxHideRecv.UseVisualStyleBackColor = true;
            this.checkBoxHideRecv.Click += new System.EventHandler(this.checkBoxHideRecv_Click);
            // 
            // listViewPack
            // 
            this.listViewPack.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2});
            this.listViewPack.FullRowSelect = true;
            this.listViewPack.HideSelection = false;
            this.listViewPack.Location = new System.Drawing.Point(43, 34);
            this.listViewPack.MultiSelect = false;
            this.listViewPack.Name = "listViewPack";
            this.listViewPack.Size = new System.Drawing.Size(530, 220);
            this.listViewPack.TabIndex = 5;
            this.listViewPack.UseCompatibleStateImageBehavior = false;
            this.listViewPack.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "类型";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "命令号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "内容";
            this.columnHeader2.Width = 1000;
            // 
            // rtbMsg
            // 
            this.rtbMsg.Location = new System.Drawing.Point(43, 372);
            this.rtbMsg.Name = "rtbMsg";
            this.rtbMsg.Size = new System.Drawing.Size(530, 162);
            this.rtbMsg.TabIndex = 10;
            this.rtbMsg.Text = "";
            // 
            // tbPackStr
            // 
            this.tbPackStr.Location = new System.Drawing.Point(43, 317);
            this.tbPackStr.Name = "tbPackStr";
            this.tbPackStr.Size = new System.Drawing.Size(341, 21);
            this.tbPackStr.TabIndex = 11;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(498, 315);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 12;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(402, 317);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(75, 21);
            this.numericUpDown1.TabIndex = 13;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_fightSkill0);
            this.groupBox2.Controls.Add(this.button_fightExit);
            this.groupBox2.Controls.Add(this.button_fightCatch);
            this.groupBox2.Location = new System.Drawing.Point(158, 551);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(309, 55);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "对战";
            // 
            // button_fightSkill0
            // 
            this.button_fightSkill0.Location = new System.Drawing.Point(216, 20);
            this.button_fightSkill0.Name = "button_fightSkill0";
            this.button_fightSkill0.Size = new System.Drawing.Size(75, 23);
            this.button_fightSkill0.TabIndex = 15;
            this.button_fightSkill0.Text = "弃权";
            this.button_fightSkill0.UseVisualStyleBackColor = true;
            this.button_fightSkill0.Click += new System.EventHandler(this.button_fightSkill0_Click);
            // 
            // button_fightExit
            // 
            this.button_fightExit.Location = new System.Drawing.Point(118, 20);
            this.button_fightExit.Name = "button_fightExit";
            this.button_fightExit.Size = new System.Drawing.Size(75, 23);
            this.button_fightExit.TabIndex = 14;
            this.button_fightExit.Text = "撤退";
            this.button_fightExit.UseVisualStyleBackColor = true;
            this.button_fightExit.Click += new System.EventHandler(this.button_fightExit_Click);
            // 
            // button_fightCatch
            // 
            this.button_fightCatch.Location = new System.Drawing.Point(20, 20);
            this.button_fightCatch.Name = "button_fightCatch";
            this.button_fightCatch.Size = new System.Drawing.Size(75, 23);
            this.button_fightCatch.TabIndex = 13;
            this.button_fightCatch.Text = "捕捉";
            this.button_fightCatch.UseVisualStyleBackColor = true;
            this.button_fightCatch.Click += new System.EventHandler(this.button_fightCatch_Click);
            // 
            // FormPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 629);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbPackStr);
            this.Controls.Add(this.rtbMsg);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxHideSend);
            this.Controls.Add(this.checkBoxHideRecv);
            this.Controls.Add(this.listViewPack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPack";
            this.Load += new System.EventHandler(this.FormScreenShot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxHideSend;
        private System.Windows.Forms.CheckBox checkBoxHideRecv;
        private System.Windows.Forms.ListView listViewPack;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.RichTextBox rtbMsg;
        private System.Windows.Forms.TextBox tbPackStr;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_fightSkill0;
        private System.Windows.Forms.Button button_fightExit;
        private System.Windows.Forms.Button button_fightCatch;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
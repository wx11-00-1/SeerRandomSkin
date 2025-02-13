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
            this.btnCleanPackSpace = new System.Windows.Forms.Button();
            this.richTextBox_filter = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
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
            this.rtbMsg.Location = new System.Drawing.Point(43, 444);
            this.rtbMsg.Name = "rtbMsg";
            this.rtbMsg.Size = new System.Drawing.Size(530, 90);
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
            // btnCleanPackSpace
            // 
            this.btnCleanPackSpace.Location = new System.Drawing.Point(282, 281);
            this.btnCleanPackSpace.Name = "btnCleanPackSpace";
            this.btnCleanPackSpace.Size = new System.Drawing.Size(102, 23);
            this.btnCleanPackSpace.TabIndex = 16;
            this.btnCleanPackSpace.Text = "复制（无空格）";
            this.btnCleanPackSpace.UseVisualStyleBackColor = true;
            this.btnCleanPackSpace.Click += new System.EventHandler(this.btnCleanPackSpace_Click);
            // 
            // richTextBox_filter
            // 
            this.richTextBox_filter.Location = new System.Drawing.Point(43, 366);
            this.richTextBox_filter.Name = "richTextBox_filter";
            this.richTextBox_filter.Size = new System.Drawing.Size(434, 52);
            this.richTextBox_filter.TabIndex = 17;
            this.richTextBox_filter.Text = "1,2,";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(498, 366);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 52);
            this.button3.TabIndex = 18;
            this.button3.Text = "屏蔽指定命令号";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(211, 281);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(65, 23);
            this.btnExport.TabIndex = 19;
            this.btnExport.Text = "导出表格";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // FormPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 573);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.richTextBox_filter);
            this.Controls.Add(this.btnCleanPackSpace);
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
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnCleanPackSpace;
        private System.Windows.Forms.RichTextBox richTextBox_filter;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnExport;
    }
}
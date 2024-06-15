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
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            // columnHeader2
            // 
            this.columnHeader2.Text = "内容";
            this.columnHeader2.Width = 1000;
            // 
            // FormPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 346);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxHideSend);
            this.Controls.Add(this.checkBoxHideRecv);
            this.Controls.Add(this.listViewPack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPack";
            this.Load += new System.EventHandler(this.FormScreenShot_Load);
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
    }
}
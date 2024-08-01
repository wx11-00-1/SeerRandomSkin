namespace SeerRandomSkin
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
            this.listViewPack = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxHideRecv = new System.Windows.Forms.CheckBox();
            this.checkBoxHideSend = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.richTextBox_filter = new System.Windows.Forms.RichTextBox();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewPack
            // 
            this.listViewPack.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewPack.FullRowSelect = true;
            this.listViewPack.HideSelection = false;
            this.listViewPack.Location = new System.Drawing.Point(43, 33);
            this.listViewPack.MultiSelect = false;
            this.listViewPack.Name = "listViewPack";
            this.listViewPack.Size = new System.Drawing.Size(530, 220);
            this.listViewPack.TabIndex = 0;
            this.listViewPack.UseCompatibleStateImageBehavior = false;
            this.listViewPack.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "类型";
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 2;
            this.columnHeader2.Text = "内容";
            this.columnHeader2.Width = 1000;
            // 
            // checkBoxHideRecv
            // 
            this.checkBoxHideRecv.AutoSize = true;
            this.checkBoxHideRecv.Location = new System.Drawing.Point(43, 280);
            this.checkBoxHideRecv.Name = "checkBoxHideRecv";
            this.checkBoxHideRecv.Size = new System.Drawing.Size(72, 16);
            this.checkBoxHideRecv.TabIndex = 1;
            this.checkBoxHideRecv.Text = "屏蔽收包";
            this.checkBoxHideRecv.UseVisualStyleBackColor = true;
            this.checkBoxHideRecv.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkBoxHideRecv_MouseClick);
            // 
            // checkBoxHideSend
            // 
            this.checkBoxHideSend.AutoSize = true;
            this.checkBoxHideSend.Location = new System.Drawing.Point(121, 280);
            this.checkBoxHideSend.Name = "checkBoxHideSend";
            this.checkBoxHideSend.Size = new System.Drawing.Size(72, 16);
            this.checkBoxHideSend.TabIndex = 2;
            this.checkBoxHideSend.Text = "屏蔽发包";
            this.checkBoxHideSend.UseVisualStyleBackColor = true;
            this.checkBoxHideSend.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkBoxHideSend_MouseClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(498, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(402, 280);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "复制";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(43, 311);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(343, 21);
            this.textBox1.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(498, 310);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "发送";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(402, 313);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(75, 21);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(498, 355);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 52);
            this.button4.TabIndex = 20;
            this.button4.Text = "屏蔽指定命令号";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // richTextBox_filter
            // 
            this.richTextBox_filter.Location = new System.Drawing.Point(43, 355);
            this.richTextBox_filter.Name = "richTextBox_filter";
            this.richTextBox_filter.Size = new System.Drawing.Size(434, 52);
            this.richTextBox_filter.TabIndex = 19;
            this.richTextBox_filter.Text = "1,2,";
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 1;
            this.columnHeader3.Text = "命令号";
            // 
            // FormPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 440);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.richTextBox_filter);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxHideSend);
            this.Controls.Add(this.checkBoxHideRecv);
            this.Controls.Add(this.listViewPack);
            this.Name = "FormPack";
            this.Load += new System.EventHandler(this.FormPack_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewPack;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox checkBoxHideRecv;
        private System.Windows.Forms.CheckBox checkBoxHideSend;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RichTextBox richTextBox_filter;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
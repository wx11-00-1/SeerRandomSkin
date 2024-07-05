namespace SeerRandomSkin
{
    partial class FormFlashFightHandler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFlashFightHandler));
            this.richTextBox_script = new System.Windows.Forms.RichTextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.lvTemplate = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbTemplateName = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox_script
            // 
            this.richTextBox_script.Location = new System.Drawing.Point(204, 37);
            this.richTextBox_script.Name = "richTextBox_script";
            this.richTextBox_script.Size = new System.Drawing.Size(707, 451);
            this.richTextBox_script.TabIndex = 0;
            this.richTextBox_script.Text = "";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(836, 511);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "执行";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lvTemplate
            // 
            this.lvTemplate.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvTemplate.FullRowSelect = true;
            this.lvTemplate.HideSelection = false;
            this.lvTemplate.Location = new System.Drawing.Point(31, 37);
            this.lvTemplate.Name = "lvTemplate";
            this.lvTemplate.Size = new System.Drawing.Size(141, 451);
            this.lvTemplate.TabIndex = 2;
            this.lvTemplate.UseCompatibleStateImageBehavior = false;
            this.lvTemplate.View = System.Windows.Forms.View.Details;
            this.lvTemplate.SelectedIndexChanged += new System.EventHandler(this.lvTemplate_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "出招方案";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "内容";
            this.columnHeader2.Width = 267;
            // 
            // tbTemplateName
            // 
            this.tbTemplateName.Location = new System.Drawing.Point(31, 511);
            this.tbTemplateName.Name = "tbTemplateName";
            this.tbTemplateName.Size = new System.Drawing.Size(141, 21);
            this.tbTemplateName.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(204, 511);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "保存";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(328, 511);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "删除";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(714, 511);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "停止接管";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormFlashFightHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 560);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbTemplateName);
            this.Controls.Add(this.lvTemplate);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.richTextBox_script);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFlashFightHandler";
            this.Load += new System.EventHandler(this.FormFlashFightHandler_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_script;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ListView lvTemplate;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox tbTemplateName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button button1;
    }
}
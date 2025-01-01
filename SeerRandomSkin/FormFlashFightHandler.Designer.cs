﻿namespace SeerRandomSkin
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn20pp = new System.Windows.Forms.Button();
            this.btnItem150 = new System.Windows.Forms.Button();
            this.btnItem10pp = new System.Windows.Forms.Button();
            this.btnItem170 = new System.Windows.Forms.Button();
            this.btnCureAll = new System.Windows.Forms.Button();
            this.btnCure20 = new System.Windows.Forms.Button();
            this.btnFightPanelHide = new System.Windows.Forms.Button();
            this.btnFightPanelShow = new System.Windows.Forms.Button();
            this.btnAutoCureOpen = new System.Windows.Forms.Button();
            this.btnAutoCureStop = new System.Windows.Forms.Button();
            this.btnItem250hp = new System.Windows.Forms.Button();
            this.btnItem200hp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
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
            this.btnTest.Location = new System.Drawing.Point(31, 588);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(66, 23);
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
            this.btnAdd.Location = new System.Drawing.Point(31, 558);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(66, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "保存";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(106, 588);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(66, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "删除";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(31, 637);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "停止自动出招";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(106, 558);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnItem200hp);
            this.groupBox1.Controls.Add(this.btnItem250hp);
            this.groupBox1.Controls.Add(this.btn20pp);
            this.groupBox1.Controls.Add(this.btnItem150);
            this.groupBox1.Controls.Add(this.btnItem10pp);
            this.groupBox1.Controls.Add(this.btnItem170);
            this.groupBox1.Location = new System.Drawing.Point(204, 511);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 159);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "对战药剂";
            // 
            // btn20pp
            // 
            this.btn20pp.Location = new System.Drawing.Point(106, 77);
            this.btn20pp.Name = "btn20pp";
            this.btn20pp.Size = new System.Drawing.Size(66, 23);
            this.btn20pp.TabIndex = 10;
            this.btn20pp.Text = "20pp";
            this.btn20pp.UseVisualStyleBackColor = true;
            this.btn20pp.Click += new System.EventHandler(this.btn20pp_Click);
            // 
            // btnItem150
            // 
            this.btnItem150.Location = new System.Drawing.Point(106, 38);
            this.btnItem150.Name = "btnItem150";
            this.btnItem150.Size = new System.Drawing.Size(66, 23);
            this.btnItem150.TabIndex = 9;
            this.btnItem150.Text = "150+3";
            this.btnItem150.UseVisualStyleBackColor = true;
            this.btnItem150.Click += new System.EventHandler(this.btnItem150_Click);
            // 
            // btnItem10pp
            // 
            this.btnItem10pp.Location = new System.Drawing.Point(28, 77);
            this.btnItem10pp.Name = "btnItem10pp";
            this.btnItem10pp.Size = new System.Drawing.Size(66, 23);
            this.btnItem10pp.TabIndex = 8;
            this.btnItem10pp.Text = "10pp";
            this.btnItem10pp.UseVisualStyleBackColor = true;
            this.btnItem10pp.Click += new System.EventHandler(this.btnItem10pp_Click);
            // 
            // btnItem170
            // 
            this.btnItem170.Location = new System.Drawing.Point(28, 38);
            this.btnItem170.Name = "btnItem170";
            this.btnItem170.Size = new System.Drawing.Size(66, 23);
            this.btnItem170.TabIndex = 7;
            this.btnItem170.Text = "170";
            this.btnItem170.UseVisualStyleBackColor = true;
            this.btnItem170.Click += new System.EventHandler(this.btnItem170_Click);
            // 
            // btnCureAll
            // 
            this.btnCureAll.Location = new System.Drawing.Point(376, 716);
            this.btnCureAll.Name = "btnCureAll";
            this.btnCureAll.Size = new System.Drawing.Size(87, 23);
            this.btnCureAll.TabIndex = 6;
            this.btnCureAll.Text = "恢复所有";
            this.btnCureAll.UseVisualStyleBackColor = true;
            this.btnCureAll.Click += new System.EventHandler(this.btnCureAll_Click);
            // 
            // btnCure20
            // 
            this.btnCure20.Location = new System.Drawing.Point(376, 686);
            this.btnCure20.Name = "btnCure20";
            this.btnCure20.Size = new System.Drawing.Size(149, 23);
            this.btnCure20.TabIndex = 5;
            this.btnCure20.Text = "全体恢复 20hp + 10pp";
            this.btnCure20.UseVisualStyleBackColor = true;
            this.btnCure20.Click += new System.EventHandler(this.btnCure20_Click);
            // 
            // btnFightPanelHide
            // 
            this.btnFightPanelHide.Location = new System.Drawing.Point(31, 686);
            this.btnFightPanelHide.Name = "btnFightPanelHide";
            this.btnFightPanelHide.Size = new System.Drawing.Size(141, 23);
            this.btnFightPanelHide.TabIndex = 9;
            this.btnFightPanelHide.Text = "隐藏Flash对战界面";
            this.btnFightPanelHide.UseVisualStyleBackColor = true;
            this.btnFightPanelHide.Click += new System.EventHandler(this.btnFightPanelHide_Click);
            // 
            // btnFightPanelShow
            // 
            this.btnFightPanelShow.Location = new System.Drawing.Point(31, 716);
            this.btnFightPanelShow.Name = "btnFightPanelShow";
            this.btnFightPanelShow.Size = new System.Drawing.Size(141, 23);
            this.btnFightPanelShow.TabIndex = 10;
            this.btnFightPanelShow.Text = "显示Flash对战界面";
            this.btnFightPanelShow.UseVisualStyleBackColor = true;
            this.btnFightPanelShow.Click += new System.EventHandler(this.btnFightPanelShow_Click);
            // 
            // btnAutoCureOpen
            // 
            this.btnAutoCureOpen.Location = new System.Drawing.Point(204, 716);
            this.btnAutoCureOpen.Name = "btnAutoCureOpen";
            this.btnAutoCureOpen.Size = new System.Drawing.Size(141, 23);
            this.btnAutoCureOpen.TabIndex = 11;
            this.btnAutoCureOpen.Text = "开启自动回血";
            this.btnAutoCureOpen.UseVisualStyleBackColor = true;
            this.btnAutoCureOpen.Click += new System.EventHandler(this.btnAutoCureOpen_Click);
            // 
            // btnAutoCureStop
            // 
            this.btnAutoCureStop.Location = new System.Drawing.Point(204, 686);
            this.btnAutoCureStop.Name = "btnAutoCureStop";
            this.btnAutoCureStop.Size = new System.Drawing.Size(141, 23);
            this.btnAutoCureStop.TabIndex = 12;
            this.btnAutoCureStop.Text = "停止自动回血";
            this.btnAutoCureStop.UseVisualStyleBackColor = true;
            this.btnAutoCureStop.Click += new System.EventHandler(this.btnAutoCureStop_Click);
            // 
            // btnItem250hp
            // 
            this.btnItem250hp.Location = new System.Drawing.Point(28, 115);
            this.btnItem250hp.Name = "btnItem250hp";
            this.btnItem250hp.Size = new System.Drawing.Size(66, 23);
            this.btnItem250hp.TabIndex = 11;
            this.btnItem250hp.Text = "250hp";
            this.btnItem250hp.UseVisualStyleBackColor = true;
            this.btnItem250hp.Click += new System.EventHandler(this.btnItem250hp_Click);
            // 
            // btnItem200hp
            // 
            this.btnItem200hp.Location = new System.Drawing.Point(106, 115);
            this.btnItem200hp.Name = "btnItem200hp";
            this.btnItem200hp.Size = new System.Drawing.Size(66, 23);
            this.btnItem200hp.TabIndex = 12;
            this.btnItem200hp.Text = "200hp";
            this.btnItem200hp.UseVisualStyleBackColor = true;
            this.btnItem200hp.Click += new System.EventHandler(this.btnItem200hp_Click);
            // 
            // FormFlashFightHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 788);
            this.Controls.Add(this.btnAutoCureStop);
            this.Controls.Add(this.btnAutoCureOpen);
            this.Controls.Add(this.btnFightPanelShow);
            this.Controls.Add(this.btnFightPanelHide);
            this.Controls.Add(this.btnCure20);
            this.Controls.Add(this.btnCureAll);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSearch);
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
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCure20;
        private System.Windows.Forms.Button btnCureAll;
        private System.Windows.Forms.Button btnItem150;
        private System.Windows.Forms.Button btnItem10pp;
        private System.Windows.Forms.Button btnItem170;
        private System.Windows.Forms.Button btn20pp;
        private System.Windows.Forms.Button btnFightPanelHide;
        private System.Windows.Forms.Button btnFightPanelShow;
        private System.Windows.Forms.Button btnAutoCureOpen;
        private System.Windows.Forms.Button btnAutoCureStop;
        private System.Windows.Forms.Button btnItem200hp;
        private System.Windows.Forms.Button btnItem250hp;
    }
}
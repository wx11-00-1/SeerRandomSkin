namespace SeerRandomSkin
{
    partial class FormSpecifyPetSkin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSpecifyPetSkin));
            this.lvMap = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_from = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_to = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_from)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_to)).BeginInit();
            this.SuspendLayout();
            // 
            // lvMap
            // 
            this.lvMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvMap.FullRowSelect = true;
            this.lvMap.HideSelection = false;
            this.lvMap.Location = new System.Drawing.Point(36, 37);
            this.lvMap.MultiSelect = false;
            this.lvMap.Name = "lvMap";
            this.lvMap.Size = new System.Drawing.Size(160, 522);
            this.lvMap.TabIndex = 0;
            this.lvMap.UseCompatibleStateImageBehavior = false;
            this.lvMap.View = System.Windows.Forms.View.Details;
            this.lvMap.SelectedIndexChanged += new System.EventHandler(this.lvMap_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "原 ID";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "替换 ID";
            this.columnHeader2.Width = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(329, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "->";
            // 
            // numericUpDown_from
            // 
            this.numericUpDown_from.Location = new System.Drawing.Point(221, 55);
            this.numericUpDown_from.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDown_from.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_from.Name = "numericUpDown_from";
            this.numericUpDown_from.Size = new System.Drawing.Size(92, 21);
            this.numericUpDown_from.TabIndex = 13;
            this.numericUpDown_from.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDown_to
            // 
            this.numericUpDown_to.Location = new System.Drawing.Point(361, 55);
            this.numericUpDown_to.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDown_to.Name = "numericUpDown_to";
            this.numericUpDown_to.Size = new System.Drawing.Size(92, 21);
            this.numericUpDown_to.TabIndex = 14;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(238, 120);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(361, 120);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 18;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // FormSpecifyPetSkin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 598);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.numericUpDown_to);
            this.Controls.Add(this.numericUpDown_from);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvMap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSpecifyPetSkin";
            this.Load += new System.EventHandler(this.FormSpecifyPetSkin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_from)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_to)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvMap;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_from;
        private System.Windows.Forms.NumericUpDown numericUpDown_to;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
    }
}
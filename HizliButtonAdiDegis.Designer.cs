namespace GPTS
{
    partial class HizliButtonAdiDegis
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
            this.oncekibarkod = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.stokadi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.oncekibarkod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stokadi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // oncekibarkod
            // 
            this.oncekibarkod.Enabled = false;
            this.oncekibarkod.Location = new System.Drawing.Point(247, 2);
            this.oncekibarkod.Name = "oncekibarkod";
            this.oncekibarkod.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.oncekibarkod.Properties.Appearance.Options.UseBackColor = true;
            this.oncekibarkod.Size = new System.Drawing.Size(114, 20);
            this.oncekibarkod.TabIndex = 7;
            this.oncekibarkod.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(151, 91);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(82, 13);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "YENİ AD GİRİNİZ";
            this.labelControl1.Click += new System.EventHandler(this.labelControl1_Click);
            // 
            // stokadi
            // 
            this.stokadi.EditValue = "";
            this.stokadi.Location = new System.Drawing.Point(29, 110);
            this.stokadi.Name = "stokadi";
            this.stokadi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.stokadi.Properties.Appearance.Options.UseFont = true;
            this.stokadi.Properties.MaxLength = 20;
            this.stokadi.Size = new System.Drawing.Size(424, 35);
            this.stokadi.TabIndex = 0;
            this.stokadi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.barkod_KeyDown);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(29, 31);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(47, 13);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "STOK ADI";
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(102, 28);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.textEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.Size = new System.Drawing.Size(298, 20);
            this.textEdit1.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GPTS.Properties.Resources.dep_1412526_Two_men_with_Refresh_symbol2;
            this.pictureBox1.Location = new System.Drawing.Point(420, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 31);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // HizliButtonAdiDegis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 158);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.oncekibarkod);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.stokadi);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HizliButtonAdiDegis";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hızlı Button Adı Değiş";
            this.Load += new System.EventHandler(this.HizliButtonAdiDegis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.oncekibarkod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stokadi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.TextEdit oncekibarkod;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.TextEdit stokadi;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        public DevExpress.XtraEditors.TextEdit textEdit1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
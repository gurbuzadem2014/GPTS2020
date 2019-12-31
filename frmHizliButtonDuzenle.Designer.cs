namespace GPTS
{
    partial class frmHizliButtonDuzenle
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
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.barkod = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.oncekibarkod = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.stokadi = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.barkod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oncekibarkod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = global::GPTS.Properties.Resources.Find;
            this.simpleButton1.Location = new System.Drawing.Point(418, 129);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(114, 48);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "ARA [F8]";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // barkod
            // 
            this.barkod.EditValue = "";
            this.barkod.Location = new System.Drawing.Point(26, 132);
            this.barkod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barkod.Name = "barkod";
            this.barkod.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.barkod.Properties.Appearance.Options.UseFont = true;
            this.barkod.Size = new System.Drawing.Size(388, 43);
            this.barkod.TabIndex = 0;
            this.barkod.KeyDown += new System.Windows.Forms.KeyEventHandler(this.barkod_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 107);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(101, 16);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "YENİ KOD GİRİNİZ";
            // 
            // oncekibarkod
            // 
            this.oncekibarkod.Enabled = false;
            this.oncekibarkod.Location = new System.Drawing.Point(14, 15);
            this.oncekibarkod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oncekibarkod.Name = "oncekibarkod";
            this.oncekibarkod.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.oncekibarkod.Properties.Appearance.Options.UseBackColor = true;
            this.oncekibarkod.Size = new System.Drawing.Size(133, 22);
            this.oncekibarkod.TabIndex = 4;
            this.oncekibarkod.Visible = false;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton3.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.simpleButton3.Location = new System.Drawing.Point(243, 2);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(143, 59);
            this.simpleButton3.TabIndex = 5;
            this.simpleButton3.Text = "Tamam [F9]";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(10, 11);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(47, 16);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "Stok Adı";
            // 
            // stokadi
            // 
            this.stokadi.Location = new System.Drawing.Point(76, 11);
            this.stokadi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.stokadi.Name = "stokadi";
            this.stokadi.Size = new System.Drawing.Size(47, 16);
            this.stokadi.TabIndex = 7;
            this.stokadi.Text = "Stok Adı";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.stokadi);
            this.panelControl1.Location = new System.Drawing.Point(26, 199);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(490, 34);
            this.panelControl1.TabIndex = 8;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.simpleButton3);
            this.panelControl2.Controls.Add(this.simpleButton21);
            this.panelControl2.Controls.Add(this.oncekibarkod);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(544, 63);
            this.panelControl2.TabIndex = 9;
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(386, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(156, 59);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // frmHizliButtonDuzenle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 270);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.barkod);
            this.Controls.Add(this.simpleButton1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHizliButtonDuzenle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hızlı Buton Düzenle";
            this.Load += new System.EventHandler(this.frmHizliButtonDuzenle_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmHizliButtonDuzenle_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barkod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oncekibarkod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.TextEdit barkod;
        public DevExpress.XtraEditors.TextEdit oncekibarkod;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        public DevExpress.XtraEditors.LabelControl stokadi;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
    }
}
namespace GPTS
{
    partial class frmMusteriGrupKarti
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
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cbVarsayilan = new DevExpress.XtraEditors.CheckEdit();
            this.GrupAdi = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.AltGrupAdi = new DevExpress.XtraEditors.TextEdit();
            this.grupid = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cbVarsayilan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrupAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AltGrupAdi.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton5
            // 
            this.simpleButton5.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.simpleButton5.Location = new System.Drawing.Point(275, 21);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(83, 23);
            this.simpleButton5.TabIndex = 169;
            this.simpleButton5.Text = "Durumu";
            // 
            // simpleButton1
            // 
            this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.simpleButton1.Location = new System.Drawing.Point(13, 21);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(256, 23);
            this.simpleButton1.TabIndex = 166;
            this.simpleButton1.Text = "Müşteri Grup Adı";
            // 
            // cbVarsayilan
            // 
            this.cbVarsayilan.EditValue = true;
            this.cbVarsayilan.Location = new System.Drawing.Point(315, 45);
            this.cbVarsayilan.Name = "cbVarsayilan";
            this.cbVarsayilan.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.cbVarsayilan.Properties.Appearance.Options.UseFont = true;
            this.cbVarsayilan.Properties.AutoWidth = true;
            this.cbVarsayilan.Properties.Caption = "";
            this.cbVarsayilan.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.cbVarsayilan.Size = new System.Drawing.Size(26, 24);
            this.cbVarsayilan.TabIndex = 164;
            this.cbVarsayilan.CheckedChanged += new System.EventHandler(this.cbVarsayilan_CheckedChanged);
            this.cbVarsayilan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbVarsayilan_KeyDown);
            // 
            // GrupAdi
            // 
            this.GrupAdi.Location = new System.Drawing.Point(13, 43);
            this.GrupAdi.Name = "GrupAdi";
            this.GrupAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.GrupAdi.Properties.Appearance.Options.UseFont = true;
            this.GrupAdi.Size = new System.Drawing.Size(256, 26);
            this.GrupAdi.TabIndex = 91;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(499, 56);
            this.panelControl1.TabIndex = 56;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Enabled = false;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(262, 2);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(112, 52);
            this.BtnKaydet.TabIndex = 56;
            this.BtnKaydet.Text = "Tamam";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton21.Location = new System.Drawing.Point(374, 2);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(123, 52);
            this.simpleButton21.TabIndex = 89;
            this.simpleButton21.Text = "Vazgeç";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 56);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(499, 133);
            this.xtraTabControl1.TabIndex = 170;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.GrupAdi);
            this.xtraTabPage1.Controls.Add(this.simpleButton5);
            this.xtraTabPage1.Controls.Add(this.cbVarsayilan);
            this.xtraTabPage1.Controls.Add(this.simpleButton1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(493, 107);
            this.xtraTabPage1.Text = "Yeni Müşteri Grup";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.labelControl4);
            this.xtraTabPage2.Controls.Add(this.labelControl2);
            this.xtraTabPage2.Controls.Add(this.AltGrupAdi);
            this.xtraTabPage2.Controls.Add(this.grupid);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(493, 107);
            this.xtraTabPage2.Text = "Yeni Müşteri  Alt Grup";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(85, 31);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(79, 13);
            this.labelControl4.TabIndex = 97;
            this.labelControl4.Text = "Müşteri Grup Adı";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(15, 65);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(149, 13);
            this.labelControl2.TabIndex = 98;
            this.labelControl2.Text = "Yeni Müşteri Alt Grup Adı Giriniz";
            // 
            // AltGrupAdi
            // 
            this.AltGrupAdi.Location = new System.Drawing.Point(170, 57);
            this.AltGrupAdi.Name = "AltGrupAdi";
            this.AltGrupAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.AltGrupAdi.Properties.Appearance.Options.UseFont = true;
            this.AltGrupAdi.Size = new System.Drawing.Size(287, 26);
            this.AltGrupAdi.TabIndex = 95;
            // 
            // grupid
            // 
            this.grupid.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.grupid.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.grupid.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.grupid.Location = new System.Drawing.Point(170, 28);
            this.grupid.Name = "grupid";
            this.grupid.Size = new System.Drawing.Size(287, 20);
            this.grupid.TabIndex = 96;
            this.grupid.Tag = "0";
            this.grupid.Text = "labelControl3";
            // 
            // frmMusteriGrupKarti
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 189);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Name = "frmMusteriGrupKarti";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "0";
            this.Text = "Müşteri Grup Bilgisi";
            this.Load += new System.EventHandler(this.frmStokKoduverKarti_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStokKoduverKarti_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.cbVarsayilan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrupAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AltGrupAdi.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.CheckEdit cbVarsayilan;
        private DevExpress.XtraEditors.TextEdit GrupAdi;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit AltGrupAdi;
        public DevExpress.XtraEditors.LabelControl grupid;
    }
}
namespace GPTS
{
    partial class frmKasaTanimlari
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
            this.teKasaAdi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.teHesapKodu = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cbAktif = new DevExpress.XtraEditors.CheckEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.pkKasalar = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lSube = new DevExpress.XtraEditors.LabelControl();
            this.lueSubeler = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.teKasaAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teHesapKodu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAktif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pkKasalar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSubeler.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // teKasaAdi
            // 
            this.teKasaAdi.Location = new System.Drawing.Point(133, 113);
            this.teKasaAdi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.teKasaAdi.Name = "teKasaAdi";
            this.teKasaAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.teKasaAdi.Properties.Appearance.Options.UseFont = true;
            this.teKasaAdi.Size = new System.Drawing.Size(233, 23);
            this.teKasaAdi.TabIndex = 6;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(68, 117);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(49, 16);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Kasa Adı";
            // 
            // teHesapKodu
            // 
            this.teHesapKodu.Location = new System.Drawing.Point(133, 149);
            this.teHesapKodu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.teHesapKodu.Name = "teHesapKodu";
            this.teHesapKodu.Size = new System.Drawing.Size(233, 22);
            this.teHesapKodu.TabIndex = 9;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(49, 153);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(67, 16);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "Hesap Kodu";
            // 
            // cbAktif
            // 
            this.cbAktif.EditValue = true;
            this.cbAktif.Location = new System.Drawing.Point(279, 226);
            this.cbAktif.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAktif.Name = "cbAktif";
            this.cbAktif.Properties.Caption = "Aktif";
            this.cbAktif.Size = new System.Drawing.Size(87, 21);
            this.cbAktif.TabIndex = 10;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(435, 58);
            this.panelControl1.TabIndex = 11;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = global::GPTS.Properties.Resources.kasahareketiekle_48x32;
            this.simpleButton4.Location = new System.Drawing.Point(2, 2);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(132, 54);
            this.simpleButton4.TabIndex = 89;
            this.simpleButton4.Text = "Yeni Kasa \r\n[F7]";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(167, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(132, 54);
            this.BtnKaydet.TabIndex = 1;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(299, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(134, 54);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // pkKasalar
            // 
            this.pkKasalar.EditValue = "0";
            this.pkKasalar.Enabled = false;
            this.pkKasalar.Location = new System.Drawing.Point(133, 78);
            this.pkKasalar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkKasalar.Name = "pkKasalar";
            this.pkKasalar.Size = new System.Drawing.Size(72, 22);
            this.pkKasalar.TabIndex = 13;
            this.pkKasalar.Tag = "";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(100, 81);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(12, 16);
            this.labelControl3.TabIndex = 12;
            this.labelControl3.Text = "ID";
            // 
            // lSube
            // 
            this.lSube.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lSube.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.lSube.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lSube.Location = new System.Drawing.Point(66, 180);
            this.lSube.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lSube.Name = "lSube";
            this.lSube.Size = new System.Drawing.Size(55, 29);
            this.lSube.TabIndex = 146;
            this.lSube.Text = " Şubeler";
            // 
            // lueSubeler
            // 
            this.lueSubeler.Location = new System.Drawing.Point(133, 182);
            this.lueSubeler.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueSubeler.Name = "lueSubeler";
            this.lueSubeler.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lueSubeler.Properties.Appearance.Options.UseFont = true;
            this.lueSubeler.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueSubeler.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkSube", "pkSube", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sube_adi", "Şube Adı")});
            this.lueSubeler.Properties.DisplayMember = "sube_adi";
            this.lueSubeler.Properties.NullText = "Seçiniz...";
            this.lueSubeler.Properties.ShowHeader = false;
            this.lueSubeler.Properties.ValueMember = "pkSube";
            this.lueSubeler.Size = new System.Drawing.Size(233, 25);
            this.lueSubeler.TabIndex = 147;
            // 
            // frmKasaTanimlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 270);
            this.ControlBox = false;
            this.Controls.Add(this.lSube);
            this.Controls.Add(this.lueSubeler);
            this.Controls.Add(this.pkKasalar);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.cbAktif);
            this.Controls.Add(this.teHesapKodu);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.teKasaAdi);
            this.Controls.Add(this.labelControl2);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmKasaTanimlari";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kasa Tanımları";
            this.Load += new System.EventHandler(this.frmKasaTanimlari_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teKasaAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teHesapKodu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAktif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pkKasalar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSubeler.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit teKasaAdi;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit teHesapKodu;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit cbAktif;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        public DevExpress.XtraEditors.TextEdit pkKasalar;
        private DevExpress.XtraEditors.LabelControl lSube;
        private DevExpress.XtraEditors.LookUpEdit lueSubeler;
    }
}
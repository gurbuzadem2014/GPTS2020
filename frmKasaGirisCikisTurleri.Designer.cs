namespace GPTS
{
    partial class frmKasaGirisCikisTurleri
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKasaGirisCikisTurleri));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Aciklama = new DevExpress.XtraEditors.TextEdit();
            this.ceGelirMi = new DevExpress.XtraEditors.CheckEdit();
            this.ceGiderMi = new DevExpress.XtraEditors.CheckEdit();
            this.ceAktif = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lueKasaHareketGrup = new DevExpress.XtraEditors.LookUpEdit();
            this.simpleButton28 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Aciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceGelirMi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceGiderMi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceAktif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueKasaHareketGrup.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(628, 60);
            this.panelControl1.TabIndex = 57;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(352, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(131, 56);
            this.BtnKaydet.TabIndex = 56;
            this.BtnKaydet.TabStop = false;
            this.BtnKaydet.Text = "Tamam [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(483, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(143, 56);
            this.simpleButton21.TabIndex = 89;
            this.simpleButton21.TabStop = false;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(44, 149);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(184, 16);
            this.labelControl1.TabIndex = 94;
            this.labelControl1.Text = "Yeni Kasa Giriş Çikis Türü Giriniz";
            // 
            // Aciklama
            // 
            this.Aciklama.Location = new System.Drawing.Point(229, 142);
            this.Aciklama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Aciklama.Name = "Aciklama";
            this.Aciklama.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.Aciklama.Properties.Appearance.Options.UseFont = true;
            this.Aciklama.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Aciklama.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.Aciklama.Size = new System.Drawing.Size(296, 31);
            this.Aciklama.TabIndex = 0;
            // 
            // ceGelirMi
            // 
            this.ceGelirMi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ceGelirMi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ceGelirMi.Location = new System.Drawing.Point(106, 202);
            this.ceGelirMi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ceGelirMi.Name = "ceGelirMi";
            this.ceGelirMi.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ceGelirMi.Properties.Appearance.Options.UseBackColor = true;
            this.ceGelirMi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ceGelirMi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.ceGelirMi.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.ceGelirMi.Properties.Caption = "Gelir Olarak İşle";
            this.ceGelirMi.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.ceGelirMi.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ceGelirMi.Size = new System.Drawing.Size(131, 24);
            this.ceGelirMi.TabIndex = 143;
            this.ceGelirMi.TabStop = false;
            // 
            // ceGiderMi
            // 
            this.ceGiderMi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ceGiderMi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ceGiderMi.Location = new System.Drawing.Point(279, 202);
            this.ceGiderMi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ceGiderMi.Name = "ceGiderMi";
            this.ceGiderMi.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ceGiderMi.Properties.Appearance.Options.UseBackColor = true;
            this.ceGiderMi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ceGiderMi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.ceGiderMi.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.ceGiderMi.Properties.Caption = "Gider Olarak İşle";
            this.ceGiderMi.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.ceGiderMi.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ceGiderMi.Size = new System.Drawing.Size(131, 24);
            this.ceGiderMi.TabIndex = 144;
            this.ceGiderMi.TabStop = false;
            // 
            // ceAktif
            // 
            this.ceAktif.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ceAktif.EditValue = true;
            this.ceAktif.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ceAktif.Location = new System.Drawing.Point(455, 202);
            this.ceAktif.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ceAktif.Name = "ceAktif";
            this.ceAktif.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ceAktif.Properties.Appearance.Options.UseBackColor = true;
            this.ceAktif.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ceAktif.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.ceAktif.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.ceAktif.Properties.Caption = "Aktif";
            this.ceAktif.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.ceAktif.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ceAktif.Size = new System.Drawing.Size(70, 24);
            this.ceAktif.TabIndex = 145;
            this.ceAktif.TabStop = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(33, 91);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(191, 16);
            this.labelControl2.TabIndex = 146;
            this.labelControl2.Text = "Yeni Kasa Giriş Çikis Grubu Giriniz";
            // 
            // lueKasaHareketGrup
            // 
            this.lueKasaHareketGrup.Location = new System.Drawing.Point(229, 89);
            this.lueKasaHareketGrup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueKasaHareketGrup.Name = "lueKasaHareketGrup";
            this.lueKasaHareketGrup.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueKasaHareketGrup.Properties.Appearance.Options.UseFont = true;
            this.lueKasaHareketGrup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueKasaHareketGrup.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkKasaGirisCikisGruplari", "pkKasaGirisCikisGruplari", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("GrupAdi", "GrupAdi")});
            this.lueKasaHareketGrup.Properties.DisplayMember = "GrupAdi";
            this.lueKasaHareketGrup.Properties.NullText = "Seçiniz...";
            this.lueKasaHareketGrup.Properties.ShowHeader = false;
            this.lueKasaHareketGrup.Properties.ValueMember = "pkKasaGirisCikisGruplari";
            this.lueKasaHareketGrup.Size = new System.Drawing.Size(296, 31);
            this.lueKasaHareketGrup.TabIndex = 147;
            // 
            // simpleButton28
            // 
            this.simpleButton28.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton28.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton28.Image")));
            this.simpleButton28.Location = new System.Drawing.Point(530, 91);
            this.simpleButton28.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton28.Name = "simpleButton28";
            this.simpleButton28.Size = new System.Drawing.Size(34, 31);
            toolTipItem1.Text = "Yeni Grup Tanımlayabilirsiniz";
            superToolTip1.Items.Add(toolTipItem1);
            this.simpleButton28.SuperTip = superToolTip1;
            this.simpleButton28.TabIndex = 206;
            this.simpleButton28.TabStop = false;
            this.simpleButton28.Click += new System.EventHandler(this.simpleButton28_Click);
            // 
            // simpleButton8
            // 
            this.simpleButton8.Location = new System.Drawing.Point(570, 92);
            this.simpleButton8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(30, 31);
            toolTipItem2.Text = "Yeni Grup Tanımlayabilirsiniz";
            superToolTip2.Items.Add(toolTipItem2);
            this.simpleButton8.SuperTip = superToolTip2;
            this.simpleButton8.TabIndex = 207;
            this.simpleButton8.TabStop = false;
            this.simpleButton8.Text = "...";
            this.simpleButton8.Click += new System.EventHandler(this.simpleButton8_Click);
            // 
            // frmKasaGirisCikisTurleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 258);
            this.Controls.Add(this.simpleButton8);
            this.Controls.Add(this.simpleButton28);
            this.Controls.Add(this.lueKasaHareketGrup);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.ceAktif);
            this.Controls.Add(this.ceGiderMi);
            this.Controls.Add(this.ceGelirMi);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.Aciklama);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmKasaGirisCikisTurleri";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "1";
            this.Text = "Yeni Kasa Giriş Çıkış Tür Ekleme";
            this.Load += new System.EventHandler(this.frmKasaGirisCikisTurleri_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmKasaGirisCikisTurleri_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Aciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceGelirMi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceGiderMi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceAktif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueKasaHareketGrup.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit Aciklama;
        private DevExpress.XtraEditors.CheckEdit ceGelirMi;
        private DevExpress.XtraEditors.CheckEdit ceGiderMi;
        private DevExpress.XtraEditors.CheckEdit ceAktif;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit lueKasaHareketGrup;
        private DevExpress.XtraEditors.SimpleButton simpleButton28;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;

    }
}
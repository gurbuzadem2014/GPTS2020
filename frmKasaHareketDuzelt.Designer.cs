namespace GPTS
{
    partial class frmKasaHareketDuzelt
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.islemtarihi = new DevExpress.XtraEditors.DateEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.ceBorc = new DevExpress.XtraEditors.CalcEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.pkKasaHareket = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ceAlacak = new DevExpress.XtraEditors.CalcEdit();
            this.tEaciklama = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cbKasayaisle = new DevExpress.XtraEditors.CheckEdit();
            this.CariAdi = new DevExpress.XtraEditors.LabelControl();
            this.ceGelirMi = new DevExpress.XtraEditors.CheckEdit();
            this.ceGiderMi = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtMakbuzNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtOdemeSekli = new DevExpress.XtraEditors.TextEdit();
            this.lueKasalar = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.islemtarihi.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.islemtarihi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceBorc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceAlacak.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tEaciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbKasayaisle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceGelirMi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceGiderMi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMakbuzNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOdemeSekli.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueKasalar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(426, 53);
            this.panelControl1.TabIndex = 2;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(152, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(132, 49);
            this.BtnKaydet.TabIndex = 1;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(284, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(140, 49);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.edit_clear;
            this.simpleButton2.Location = new System.Drawing.Point(2, 2);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(101, 49);
            this.simpleButton2.TabIndex = 89;
            this.simpleButton2.Text = "Temizle";
            this.simpleButton2.Visible = false;
            // 
            // islemtarihi
            // 
            this.islemtarihi.EditValue = null;
            this.islemtarihi.Location = new System.Drawing.Point(178, 110);
            this.islemtarihi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.islemtarihi.Name = "islemtarihi";
            this.islemtarihi.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.islemtarihi.Properties.Appearance.Options.UseFont = true;
            this.islemtarihi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.islemtarihi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.islemtarihi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.islemtarihi.Properties.DisplayFormat.FormatString = "g";
            this.islemtarihi.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.islemtarihi.Properties.EditFormat.FormatString = "g";
            this.islemtarihi.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.islemtarihi.Properties.Mask.EditMask = "g";
            this.islemtarihi.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.islemtarihi.Size = new System.Drawing.Size(185, 24);
            this.islemtarihi.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(80, 113);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 18);
            this.label11.TabIndex = 4;
            this.label11.Text = "İşlem Tarihi ";
            // 
            // ceBorc
            // 
            this.ceBorc.Location = new System.Drawing.Point(177, 143);
            this.ceBorc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ceBorc.Name = "ceBorc";
            this.ceBorc.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ceBorc.Properties.Appearance.Options.UseFont = true;
            this.ceBorc.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ceBorc.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.ceBorc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceBorc.Properties.DisplayFormat.FormatString = "{0:#0.00####}";
            this.ceBorc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ceBorc.Size = new System.Drawing.Size(187, 35);
            this.ceBorc.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(66, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Tahsilat Tutarı";
            // 
            // pkKasaHareket
            // 
            this.pkKasaHareket.Location = new System.Drawing.Point(12, 421);
            this.pkKasaHareket.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkKasaHareket.Name = "pkKasaHareket";
            this.pkKasaHareket.Size = new System.Drawing.Size(116, 23);
            this.pkKasaHareket.TabIndex = 91;
            this.pkKasaHareket.Text = "0";
            this.pkKasaHareket.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(68, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 18);
            this.label2.TabIndex = 93;
            this.label2.Text = "Ödeme Tutarı";
            // 
            // ceAlacak
            // 
            this.ceAlacak.Location = new System.Drawing.Point(177, 186);
            this.ceAlacak.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ceAlacak.Name = "ceAlacak";
            this.ceAlacak.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ceAlacak.Properties.Appearance.Options.UseFont = true;
            this.ceAlacak.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ceAlacak.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.ceAlacak.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceAlacak.Properties.DisplayFormat.FormatString = "{0:#0.00####}";
            this.ceAlacak.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ceAlacak.Size = new System.Drawing.Size(187, 35);
            this.ceAlacak.TabIndex = 92;
            // 
            // tEaciklama
            // 
            this.tEaciklama.EditValue = "";
            this.tEaciklama.Location = new System.Drawing.Point(177, 267);
            this.tEaciklama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tEaciklama.Name = "tEaciklama";
            this.tEaciklama.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tEaciklama.Properties.Appearance.Options.UseFont = true;
            this.tEaciklama.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.tEaciklama.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tEaciklama.Size = new System.Drawing.Size(187, 25);
            this.tEaciklama.TabIndex = 95;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(117, 271);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(51, 16);
            this.labelControl3.TabIndex = 96;
            this.labelControl3.Text = "Açıklama";
            // 
            // cbKasayaisle
            // 
            this.cbKasayaisle.Location = new System.Drawing.Point(235, 378);
            this.cbKasayaisle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbKasayaisle.Name = "cbKasayaisle";
            this.cbKasayaisle.Properties.Caption = "Aktif Hesap";
            this.cbKasayaisle.Size = new System.Drawing.Size(128, 21);
            this.cbKasayaisle.TabIndex = 97;
            // 
            // CariAdi
            // 
            this.CariAdi.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.CariAdi.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CariAdi.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.CariAdi.Dock = System.Windows.Forms.DockStyle.Top;
            this.CariAdi.Location = new System.Drawing.Point(0, 53);
            this.CariAdi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CariAdi.Name = "CariAdi";
            this.CariAdi.Size = new System.Drawing.Size(426, 34);
            this.CariAdi.TabIndex = 98;
            this.CariAdi.Tag = "0";
            this.CariAdi.Text = "CariAdi";
            this.CariAdi.Visible = false;
            // 
            // ceGelirMi
            // 
            this.ceGelirMi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ceGelirMi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ceGelirMi.Location = new System.Drawing.Point(233, 431);
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
            this.ceGiderMi.Location = new System.Drawing.Point(233, 481);
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
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(104, 305);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(62, 16);
            this.labelControl1.TabIndex = 96;
            this.labelControl1.Text = "Makbuz No";
            // 
            // txtMakbuzNo
            // 
            this.txtMakbuzNo.EditValue = "";
            this.txtMakbuzNo.Location = new System.Drawing.Point(178, 300);
            this.txtMakbuzNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMakbuzNo.Name = "txtMakbuzNo";
            this.txtMakbuzNo.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtMakbuzNo.Properties.Appearance.Options.UseFont = true;
            this.txtMakbuzNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtMakbuzNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtMakbuzNo.Size = new System.Drawing.Size(187, 25);
            this.txtMakbuzNo.TabIndex = 95;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(91, 235);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 16);
            this.labelControl2.TabIndex = 96;
            this.labelControl2.Text = "Ödeme Şekli";
            // 
            // txtOdemeSekli
            // 
            this.txtOdemeSekli.EditValue = "";
            this.txtOdemeSekli.Location = new System.Drawing.Point(178, 232);
            this.txtOdemeSekli.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOdemeSekli.Name = "txtOdemeSekli";
            this.txtOdemeSekli.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtOdemeSekli.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtOdemeSekli.Properties.Appearance.Options.UseBackColor = true;
            this.txtOdemeSekli.Properties.Appearance.Options.UseFont = true;
            this.txtOdemeSekli.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtOdemeSekli.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtOdemeSekli.Properties.ReadOnly = true;
            this.txtOdemeSekli.Size = new System.Drawing.Size(187, 25);
            this.txtOdemeSekli.TabIndex = 95;
            // 
            // lueKasalar
            // 
            this.lueKasalar.Location = new System.Drawing.Point(179, 333);
            this.lueKasalar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueKasalar.Name = "lueKasalar";
            this.lueKasalar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lueKasalar.Properties.Appearance.Options.UseFont = true;
            this.lueKasalar.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lueKasalar.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.lueKasalar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueKasalar.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("KasaAdi", "Kasa Adı"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkKasalar", "pkKasalar", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.lueKasalar.Properties.DisplayMember = "KasaAdi";
            this.lueKasalar.Properties.NullText = "Seçiniz";
            this.lueKasalar.Properties.ValueMember = "pkKasalar";
            this.lueKasalar.Size = new System.Drawing.Size(185, 25);
            this.lueKasalar.TabIndex = 145;
            this.lueKasalar.Tag = "0";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(115, 337);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(49, 16);
            this.labelControl4.TabIndex = 96;
            this.labelControl4.Text = "Kasa Adı";
            // 
            // frmKasaHareketDuzelt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 584);
            this.ControlBox = false;
            this.Controls.Add(this.lueKasalar);
            this.Controls.Add(this.ceGiderMi);
            this.Controls.Add(this.ceGelirMi);
            this.Controls.Add(this.CariAdi);
            this.Controls.Add(this.cbKasayaisle);
            this.Controls.Add(this.txtMakbuzNo);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtOdemeSekli);
            this.Controls.Add(this.tEaciklama);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ceAlacak);
            this.Controls.Add(this.pkKasaHareket);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ceBorc);
            this.Controls.Add(this.islemtarihi);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmKasaHareketDuzelt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kasa Hareket Düzelt";
            this.Load += new System.EventHandler(this.frmKasaHareketDuzelt_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmKasaHareketDuzelt_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.islemtarihi.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.islemtarihi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceBorc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceAlacak.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tEaciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbKasayaisle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceGelirMi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceGiderMi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMakbuzNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOdemeSekli.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueKasalar.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.DateEdit islemtarihi;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.CalcEdit ceBorc;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox pkKasaHareket;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.CalcEdit ceAlacak;
        private DevExpress.XtraEditors.TextEdit tEaciklama;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit cbKasayaisle;
        public DevExpress.XtraEditors.LabelControl CariAdi;
        private DevExpress.XtraEditors.CheckEdit ceGelirMi;
        private DevExpress.XtraEditors.CheckEdit ceGiderMi;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtMakbuzNo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtOdemeSekli;
        private DevExpress.XtraEditors.LookUpEdit lueKasalar;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}
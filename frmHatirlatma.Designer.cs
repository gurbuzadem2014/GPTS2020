namespace GPTS
{
    partial class frmHatirlatma
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
            this.cbKategori = new DevExpress.XtraEditors.ComboBoxEdit();
            this.dtBasTar = new DevExpress.XtraEditors.DateEdit();
            this.dtBitTarih = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cbUyar = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.Aciklama = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.cbTekrarlamaSecenek = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbEposta = new DevExpress.XtraEditors.CheckEdit();
            this.cbSms = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit2 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.teFirmaid = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.lueHatirlatmaDurum = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cbArandi = new DevExpress.XtraEditors.CheckEdit();
            this.cbAnimsat = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.lueOdalar = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cbKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBasTar.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBasTar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBitTarih.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBitTarih.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbUyar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Aciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbTekrarlamaSecenek.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbEposta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSms.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teFirmaid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueHatirlatmaDurum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbArandi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAnimsat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOdalar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cbKategori
            // 
            this.cbKategori.EditValue = "Diğer";
            this.cbKategori.Location = new System.Drawing.Point(532, 325);
            this.cbKategori.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbKategori.Name = "cbKategori";
            this.cbKategori.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbKategori.Properties.Appearance.Options.UseFont = true;
            this.cbKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbKategori.Properties.Items.AddRange(new object[] {
            "Diğer",
            "Doğum Günü",
            "Kutlama / Yıldönümü",
            "Tatil",
            "Toplantı"});
            this.cbKategori.Size = new System.Drawing.Size(155, 27);
            this.cbKategori.TabIndex = 4;
            this.cbKategori.Visible = false;
            // 
            // dtBasTar
            // 
            this.dtBasTar.EditValue = new System.DateTime(2013, 10, 29, 11, 6, 30, 0);
            this.dtBasTar.Location = new System.Drawing.Point(162, 238);
            this.dtBasTar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtBasTar.Name = "dtBasTar";
            this.dtBasTar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtBasTar.Properties.Appearance.Options.UseFont = true;
            this.dtBasTar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtBasTar.Properties.DisplayFormat.FormatString = "g";
            this.dtBasTar.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtBasTar.Properties.EditFormat.FormatString = "g";
            this.dtBasTar.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtBasTar.Properties.Mask.EditMask = "g";
            this.dtBasTar.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtBasTar.Size = new System.Drawing.Size(154, 27);
            this.dtBasTar.TabIndex = 5;
            this.dtBasTar.EditValueChanged += new System.EventHandler(this.dtBasTar_EditValueChanged);
            // 
            // dtBitTarih
            // 
            this.dtBitTarih.EditValue = new System.DateTime(2013, 10, 29, 11, 6, 47, 972);
            this.dtBitTarih.Location = new System.Drawing.Point(534, 238);
            this.dtBitTarih.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtBitTarih.Name = "dtBitTarih";
            this.dtBitTarih.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtBitTarih.Properties.Appearance.Options.UseFont = true;
            this.dtBitTarih.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtBitTarih.Properties.DisplayFormat.FormatString = "g";
            this.dtBitTarih.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtBitTarih.Properties.EditFormat.FormatString = "g";
            this.dtBitTarih.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.dtBitTarih.Properties.Mask.EditMask = "g";
            this.dtBitTarih.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtBitTarih.Size = new System.Drawing.Size(154, 27);
            this.dtBitTarih.TabIndex = 6;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl3.Location = new System.Drawing.Point(27, 240);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(128, 21);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Başlangıç zamanı";
            // 
            // cbUyar
            // 
            this.cbUyar.EditValue = true;
            this.cbUyar.Location = new System.Drawing.Point(160, 393);
            this.cbUyar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbUyar.Name = "cbUyar";
            this.cbUyar.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cbUyar.Properties.Appearance.Options.UseFont = true;
            this.cbUyar.Properties.Caption = "Hatırlat";
            this.cbUyar.Size = new System.Drawing.Size(118, 25);
            this.cbUyar.TabIndex = 8;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl4.Location = new System.Drawing.Point(435, 243);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(92, 21);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "Bitiş Zamanı";
            // 
            // Aciklama
            // 
            this.Aciklama.Location = new System.Drawing.Point(162, 190);
            this.Aciklama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Aciklama.Name = "Aciklama";
            this.Aciklama.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.Aciklama.Properties.Appearance.Options.UseFont = true;
            this.Aciklama.Size = new System.Drawing.Size(526, 27);
            this.Aciklama.TabIndex = 10;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton3.Image = global::GPTS.Properties.Resources.sil_1;
            this.simpleButton3.Location = new System.Drawing.Point(2, 2);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(98, 53);
            this.simpleButton3.TabIndex = 14;
            this.simpleButton3.Text = "Sil";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl5.Location = new System.Drawing.Point(88, 193);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(67, 21);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "Açıklama";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl6.Location = new System.Drawing.Point(467, 329);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(62, 21);
            this.labelControl6.TabIndex = 18;
            this.labelControl6.Text = "Kategori";
            this.labelControl6.Visible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(730, 57);
            this.panelControl1.TabIndex = 59;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(454, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(131, 53);
            this.BtnKaydet.TabIndex = 56;
            this.BtnKaydet.Text = "Tamam [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(585, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(143, 53);
            this.simpleButton21.TabIndex = 89;
            this.simpleButton21.Text = "Vazgeç [Esc]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // labelControl22
            // 
            this.labelControl22.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl22.Location = new System.Drawing.Point(20, 284);
            this.labelControl22.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(136, 18);
            this.labelControl22.TabIndex = 1009;
            this.labelControl22.Text = "Tekrarlama Seçeneği";
            this.labelControl22.Visible = false;
            // 
            // cbTekrarlamaSecenek
            // 
            this.cbTekrarlamaSecenek.EditValue = "Tek Seferlik";
            this.cbTekrarlamaSecenek.EnterMoveNextControl = true;
            this.cbTekrarlamaSecenek.Location = new System.Drawing.Point(162, 281);
            this.cbTekrarlamaSecenek.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbTekrarlamaSecenek.Name = "cbTekrarlamaSecenek";
            this.cbTekrarlamaSecenek.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbTekrarlamaSecenek.Properties.Appearance.Options.UseFont = true;
            this.cbTekrarlamaSecenek.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbTekrarlamaSecenek.Properties.Items.AddRange(new object[] {
            "Tek Seferlik",
            "Haftada 1 Kez",
            "Ayda 1 Kez",
            "Yılda 1 Kez"});
            this.cbTekrarlamaSecenek.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbTekrarlamaSecenek.Size = new System.Drawing.Size(155, 27);
            this.cbTekrarlamaSecenek.TabIndex = 1008;
            this.cbTekrarlamaSecenek.Visible = false;
            this.cbTekrarlamaSecenek.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit3_SelectedIndexChanged);
            // 
            // cbEposta
            // 
            this.cbEposta.EditValue = true;
            this.cbEposta.Enabled = false;
            this.cbEposta.Location = new System.Drawing.Point(157, 464);
            this.cbEposta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbEposta.Name = "cbEposta";
            this.cbEposta.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cbEposta.Properties.Appearance.Options.UseFont = true;
            this.cbEposta.Properties.Caption = "E-Posta Gönder";
            this.cbEposta.Size = new System.Drawing.Size(159, 25);
            this.cbEposta.TabIndex = 1010;
            // 
            // cbSms
            // 
            this.cbSms.Enabled = false;
            this.cbSms.Location = new System.Drawing.Point(160, 428);
            this.cbSms.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbSms.Name = "cbSms";
            this.cbSms.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cbSms.Properties.Appearance.Options.UseFont = true;
            this.cbSms.Properties.Caption = "Sms Gönder";
            this.cbSms.Size = new System.Drawing.Size(118, 25);
            this.cbSms.TabIndex = 1010;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.BackColor = System.Drawing.Color.White;
            this.labelControl8.Appearance.Image = global::GPTS.Properties.Resources.info_bilgi_32x32;
            this.labelControl8.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelControl8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelControl8.Location = new System.Drawing.Point(0, 536);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(730, 46);
            this.labelControl8.TabIndex = 1011;
            this.labelControl8.Text = "Sms ücreti 1.000 sms 40.00 TL dir";
            this.labelControl8.Click += new System.EventHandler(this.labelControl8_Click);
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl7.Location = new System.Drawing.Point(484, 287);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(43, 21);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "Etiket";
            this.labelControl7.Visible = false;
            // 
            // comboBoxEdit2
            // 
            this.comboBoxEdit2.EditValue = "Hiçbiri";
            this.comboBoxEdit2.Location = new System.Drawing.Point(532, 283);
            this.comboBoxEdit2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxEdit2.Name = "comboBoxEdit2";
            this.comboBoxEdit2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.comboBoxEdit2.Properties.Appearance.Options.UseFont = true;
            this.comboBoxEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit2.Properties.Items.AddRange(new object[] {
            "Aidat Ödemesi",
            "Akaryakıt",
            "Birikim",
            "Çek / Senet",
            "Diğer",
            "Eğitim",
            "Eğlence / Hobi",
            "Elektronik",
            "Emeklilik",
            "E-Ticaret",
            "Ev / Dekorasyon",
            "Faiz / Komisyon",
            "Giyim / Aksesuar",
            "Kart Ödemesi",
            "Kira Ödemesi",
            "Kredi Ödemesi"});
            this.comboBoxEdit2.Size = new System.Drawing.Size(155, 27);
            this.comboBoxEdit2.TabIndex = 16;
            this.comboBoxEdit2.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl1.Location = new System.Drawing.Point(68, 100);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 21);
            this.labelControl1.TabIndex = 19;
            this.labelControl1.Text = "Müşteri Adı";
            this.labelControl1.Click += new System.EventHandler(this.labelControl1_Click);
            // 
            // teFirmaid
            // 
            this.teFirmaid.Location = new System.Drawing.Point(162, 97);
            this.teFirmaid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.teFirmaid.Name = "teFirmaid";
            this.teFirmaid.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.teFirmaid.Properties.Appearance.Options.UseFont = true;
            this.teFirmaid.Properties.ReadOnly = true;
            this.teFirmaid.Size = new System.Drawing.Size(383, 27);
            this.teFirmaid.TabIndex = 1012;
            this.teFirmaid.Tag = "-1";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.musteribul_32x32;
            this.simpleButton2.Location = new System.Drawing.Point(552, 87);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(164, 44);
            this.simpleButton2.TabIndex = 1013;
            this.simpleButton2.Text = "Müşteri Seç [F4]";
            this.simpleButton2.ToolTip = "Satış yapılacak müşteriyi  seçer ";
            this.simpleButton2.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // lueHatirlatmaDurum
            // 
            this.lueHatirlatmaDurum.Location = new System.Drawing.Point(161, 142);
            this.lueHatirlatmaDurum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueHatirlatmaDurum.Name = "lueHatirlatmaDurum";
            this.lueHatirlatmaDurum.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lueHatirlatmaDurum.Properties.Appearance.Options.UseFont = true;
            this.lueHatirlatmaDurum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueHatirlatmaDurum.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkHatirlatmaDurum", "pkHatirlatmaDurum", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("durumu", "durumu")});
            this.lueHatirlatmaDurum.Properties.DisplayMember = "durumu";
            this.lueHatirlatmaDurum.Properties.DropDownRows = 15;
            this.lueHatirlatmaDurum.Properties.NullText = "";
            this.lueHatirlatmaDurum.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.lueHatirlatmaDurum.Properties.ShowHeader = false;
            this.lueHatirlatmaDurum.Properties.ShowPopupShadow = false;
            this.lueHatirlatmaDurum.Properties.ValueMember = "pkHatirlatmaDurum";
            this.lueHatirlatmaDurum.Size = new System.Drawing.Size(154, 29);
            this.lueHatirlatmaDurum.TabIndex = 1015;
            this.lueHatirlatmaDurum.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl2.Location = new System.Drawing.Point(27, 146);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(128, 21);
            this.labelControl2.TabIndex = 1014;
            this.labelControl2.Text = "Randevu Durumu";
            // 
            // cbArandi
            // 
            this.cbArandi.Location = new System.Drawing.Point(160, 341);
            this.cbArandi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbArandi.Name = "cbArandi";
            this.cbArandi.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cbArandi.Properties.Appearance.Options.UseFont = true;
            this.cbArandi.Properties.Caption = "Arandı";
            this.cbArandi.Size = new System.Drawing.Size(118, 25);
            this.cbArandi.TabIndex = 8;
            // 
            // cbAnimsat
            // 
            this.cbAnimsat.Location = new System.Drawing.Point(467, 393);
            this.cbAnimsat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAnimsat.Name = "cbAnimsat";
            this.cbAnimsat.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cbAnimsat.Properties.Appearance.Options.UseFont = true;
            this.cbAnimsat.Properties.Caption = "Anımsat";
            this.cbAnimsat.Size = new System.Drawing.Size(118, 23);
            this.cbAnimsat.TabIndex = 1016;
            this.cbAnimsat.Visible = false;
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl9.Location = new System.Drawing.Point(431, 147);
            this.labelControl9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(59, 21);
            this.labelControl9.TabIndex = 1014;
            this.labelControl9.Text = "Oda Adı";
            // 
            // lueOdalar
            // 
            this.lueOdalar.EditValue = "Tümü";
            this.lueOdalar.EnterMoveNextControl = true;
            this.lueOdalar.Location = new System.Drawing.Point(498, 143);
            this.lueOdalar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueOdalar.Name = "lueOdalar";
            this.lueOdalar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lueOdalar.Properties.Appearance.Options.UseFont = true;
            this.lueOdalar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueOdalar.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkOda", "pkOda", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("oda_adi", "oda_adi")});
            this.lueOdalar.Properties.DisplayMember = "oda_adi";
            this.lueOdalar.Properties.DropDownRows = 15;
            this.lueOdalar.Properties.NullText = "Tüm Odalar";
            this.lueOdalar.Properties.ShowHeader = false;
            this.lueOdalar.Properties.ValueMember = "pkOda";
            this.lueOdalar.Size = new System.Drawing.Size(190, 29);
            this.lueOdalar.TabIndex = 1017;
            this.lueOdalar.Tag = "0";
            this.lueOdalar.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            // 
            // frmHatirlatma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 582);
            this.Controls.Add(this.lueOdalar);
            this.Controls.Add(this.cbAnimsat);
            this.Controls.Add(this.lueHatirlatmaDurum);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.teFirmaid);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.cbSms);
            this.Controls.Add(this.cbEposta);
            this.Controls.Add(this.labelControl22);
            this.Controls.Add(this.cbTekrarlamaSecenek);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.comboBoxEdit2);
            this.Controls.Add(this.Aciklama);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.cbArandi);
            this.Controls.Add(this.cbUyar);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.dtBitTarih);
            this.Controls.Add(this.dtBasTar);
            this.Controls.Add(this.cbKategori);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHatirlatma";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yeni Hatırlatma";
            this.Load += new System.EventHandler(this.frmHatirlatma_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cbKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBasTar.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBasTar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBitTarih.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBitTarih.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbUyar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Aciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbTekrarlamaSecenek.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbEposta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSms.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teFirmaid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueHatirlatmaDurum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbArandi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAnimsat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOdalar.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cbKategori;
        private DevExpress.XtraEditors.DateEdit dtBasTar;
        private DevExpress.XtraEditors.DateEdit dtBitTarih;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit cbUyar;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit Aciklama;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.ComboBoxEdit cbTekrarlamaSecenek;
        private DevExpress.XtraEditors.CheckEdit cbEposta;
        private DevExpress.XtraEditors.CheckEdit cbSms;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit teFirmaid;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LookUpEdit lueHatirlatmaDurum;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit cbArandi;
        private DevExpress.XtraEditors.CheckEdit cbAnimsat;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LookUpEdit lueOdalar;
    }
}
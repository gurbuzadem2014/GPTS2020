namespace GPTS
{
    partial class frmFisYaziciYeniRapor
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
            this.components = new System.ComponentModel.Container();
            this.GrupAdi = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnGuncelle = new DevExpress.XtraEditors.SimpleButton();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.cbOnizle = new DevExpress.XtraEditors.CheckEdit();
            this.cbAktif = new DevExpress.XtraEditors.CheckEdit();
            this.checkedListBoxControl1 = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.pkEtiketSablonlari = new System.Windows.Forms.TextBox();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.seSiraNo = new DevExpress.XtraEditors.SpinEdit();
            this.seYazdirmaAdedi = new DevExpress.XtraEditors.SpinEdit();
            this.pkSatisFisiSecimi = new System.Windows.Forms.TextBox();
            this.cbYazicilar = new System.Windows.Forms.ComboBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lueKullanicilar = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lueSatisTipi = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.lcDosyaYolu = new DevExpress.XtraEditors.LabelControl();
            this.dosyaadi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtDosyaYolu = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btnRaporYukle = new DevExpress.XtraEditors.SimpleButton();
            this.lbDurum = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.GrupAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbOnizle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAktif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seSiraNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seYazdirmaAdedi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueKullanicilar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSatisTipi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dosyaadi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDosyaYolu.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GrupAdi
            // 
            this.GrupAdi.Location = new System.Drawing.Point(176, 32);
            this.GrupAdi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrupAdi.Name = "GrupAdi";
            this.GrupAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.GrupAdi.Properties.Appearance.Options.UseFont = true;
            this.GrupAdi.Size = new System.Drawing.Size(348, 31);
            this.GrupAdi.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnGuncelle);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(877, 69);
            this.panelControl1.TabIndex = 56;
            // 
            // BtnGuncelle
            // 
            this.BtnGuncelle.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnGuncelle.Image = global::GPTS.Properties.Resources.save;
            this.BtnGuncelle.Location = new System.Drawing.Point(470, 2);
            this.BtnGuncelle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnGuncelle.Name = "BtnGuncelle";
            this.BtnGuncelle.Size = new System.Drawing.Size(131, 65);
            this.BtnGuncelle.TabIndex = 90;
            this.BtnGuncelle.TabStop = false;
            this.BtnGuncelle.Text = "Güncelle [F9]";
            this.BtnGuncelle.Click += new System.EventHandler(this.BtnGuncelle_Click);
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(601, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(131, 65);
            this.BtnKaydet.TabIndex = 56;
            this.BtnKaydet.TabStop = false;
            this.BtnKaydet.Text = "Tamam [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(732, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(143, 65);
            this.simpleButton21.TabIndex = 89;
            this.simpleButton21.TabStop = false;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 69);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(877, 450);
            this.xtraTabControl1.TabIndex = 167;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            this.xtraTabControl1.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.lbDurum);
            this.xtraTabPage1.Controls.Add(this.btnRaporYukle);
            this.xtraTabPage1.Controls.Add(this.labelControl8);
            this.xtraTabPage1.Controls.Add(this.cbOnizle);
            this.xtraTabPage1.Controls.Add(this.cbAktif);
            this.xtraTabPage1.Controls.Add(this.checkedListBoxControl1);
            this.xtraTabPage1.Controls.Add(this.pkEtiketSablonlari);
            this.xtraTabPage1.Controls.Add(this.labelControl6);
            this.xtraTabPage1.Controls.Add(this.labelControl7);
            this.xtraTabPage1.Controls.Add(this.seSiraNo);
            this.xtraTabPage1.Controls.Add(this.seYazdirmaAdedi);
            this.xtraTabPage1.Controls.Add(this.pkSatisFisiSecimi);
            this.xtraTabPage1.Controls.Add(this.cbYazicilar);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.simpleButton1);
            this.xtraTabPage1.Controls.Add(this.labelControl4);
            this.xtraTabPage1.Controls.Add(this.lueKullanicilar);
            this.xtraTabPage1.Controls.Add(this.labelControl5);
            this.xtraTabPage1.Controls.Add(this.labelControl3);
            this.xtraTabPage1.Controls.Add(this.lueSatisTipi);
            this.xtraTabPage1.Controls.Add(this.labelControl10);
            this.xtraTabPage1.Controls.Add(this.lcDosyaYolu);
            this.xtraTabPage1.Controls.Add(this.dosyaadi);
            this.xtraTabPage1.Controls.Add(this.labelControl9);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.txtDosyaYolu);
            this.xtraTabPage1.Controls.Add(this.GrupAdi);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(871, 421);
            this.xtraTabPage1.Text = "Yen Rapor Adı";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl8.Location = new System.Drawing.Point(336, 228);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(280, 16);
            this.labelControl8.TabIndex = 217;
            this.labelControl8.Text = "Varsayılna olduğunda Tüm kullanıcılarda Görünür";
            // 
            // cbOnizle
            // 
            this.cbOnizle.Location = new System.Drawing.Point(360, 300);
            this.cbOnizle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbOnizle.Name = "cbOnizle";
            this.cbOnizle.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbOnizle.Properties.Appearance.Options.UseFont = true;
            this.cbOnizle.Properties.Caption = "Ön İzle";
            this.cbOnizle.Size = new System.Drawing.Size(124, 26);
            this.cbOnizle.TabIndex = 216;
            // 
            // cbAktif
            // 
            this.cbAktif.Location = new System.Drawing.Point(174, 300);
            this.cbAktif.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAktif.Name = "cbAktif";
            this.cbAktif.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbAktif.Properties.Appearance.Options.UseFont = true;
            this.cbAktif.Properties.Caption = "Aktif";
            this.cbAktif.Size = new System.Drawing.Size(124, 26);
            this.cbAktif.TabIndex = 216;
            // 
            // checkedListBoxControl1
            // 
            this.checkedListBoxControl1.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "PC1"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "PC2"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "PC3"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "SERVER")});
            this.checkedListBoxControl1.Location = new System.Drawing.Point(731, 138);
            this.checkedListBoxControl1.Name = "checkedListBoxControl1";
            this.checkedListBoxControl1.Size = new System.Drawing.Size(98, 107);
            this.checkedListBoxControl1.TabIndex = 215;
            this.checkedListBoxControl1.Visible = false;
            // 
            // pkEtiketSablonlari
            // 
            this.pkEtiketSablonlari.Location = new System.Drawing.Point(336, 4);
            this.pkEtiketSablonlari.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkEtiketSablonlari.Name = "pkEtiketSablonlari";
            this.pkEtiketSablonlari.Size = new System.Drawing.Size(116, 23);
            this.pkEtiketSablonlari.TabIndex = 214;
            this.pkEtiketSablonlari.Text = "0";
            this.pkEtiketSablonlari.Visible = false;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl6.Location = new System.Drawing.Point(96, 262);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(50, 18);
            this.labelControl6.TabIndex = 213;
            this.labelControl6.Text = "Sıra No";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl7.Location = new System.Drawing.Point(51, 190);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(102, 18);
            this.labelControl7.TabIndex = 213;
            this.labelControl7.Text = "Yazdırma Adedi";
            // 
            // seSiraNo
            // 
            this.seSiraNo.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seSiraNo.Location = new System.Drawing.Point(176, 259);
            this.seSiraNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.seSiraNo.Name = "seSiraNo";
            this.seSiraNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.seSiraNo.Properties.Appearance.Options.UseFont = true;
            this.seSiraNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seSiraNo.Size = new System.Drawing.Size(68, 25);
            this.seSiraNo.TabIndex = 212;
            this.seSiraNo.TabStop = false;
            // 
            // seYazdirmaAdedi
            // 
            this.seYazdirmaAdedi.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seYazdirmaAdedi.Location = new System.Drawing.Point(176, 188);
            this.seYazdirmaAdedi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.seYazdirmaAdedi.Name = "seYazdirmaAdedi";
            this.seYazdirmaAdedi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.seYazdirmaAdedi.Properties.Appearance.Options.UseFont = true;
            this.seYazdirmaAdedi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seYazdirmaAdedi.Size = new System.Drawing.Size(68, 25);
            this.seYazdirmaAdedi.TabIndex = 212;
            this.seYazdirmaAdedi.TabStop = false;
            // 
            // pkSatisFisiSecimi
            // 
            this.pkSatisFisiSecimi.Location = new System.Drawing.Point(176, 2);
            this.pkSatisFisiSecimi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkSatisFisiSecimi.Name = "pkSatisFisiSecimi";
            this.pkSatisFisiSecimi.Size = new System.Drawing.Size(116, 23);
            this.pkSatisFisiSecimi.TabIndex = 101;
            this.pkSatisFisiSecimi.Text = "0";
            this.pkSatisFisiSecimi.Visible = false;
            // 
            // cbYazicilar
            // 
            this.cbYazicilar.FormattingEnabled = true;
            this.cbYazicilar.Location = new System.Drawing.Point(176, 113);
            this.cbYazicilar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbYazicilar.Name = "cbYazicilar";
            this.cbYazicilar.Size = new System.Drawing.Size(347, 24);
            this.cbYazicilar.TabIndex = 100;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl2.Location = new System.Drawing.Point(45, 117);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(109, 18);
            this.labelControl2.TabIndex = 99;
            this.labelControl2.Text = "Yazici Adı Giriniz";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(460, 71);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(36, 32);
            this.simpleButton1.TabIndex = 97;
            this.simpleButton1.TabStop = false;
            this.simpleButton1.Text = "...";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl4.Location = new System.Drawing.Point(633, 252);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(196, 16);
            this.labelControl4.TabIndex = 95;
            this.labelControl4.Text = "Bu Yazıcıyı Kullanacak Bilgisayarlar";
            this.labelControl4.Visible = false;
            // 
            // lueKullanicilar
            // 
            this.lueKullanicilar.Location = new System.Drawing.Point(176, 220);
            this.lueKullanicilar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueKullanicilar.Name = "lueKullanicilar";
            this.lueKullanicilar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lueKullanicilar.Properties.Appearance.Options.UseFont = true;
            this.lueKullanicilar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueKullanicilar.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("KullaniciAdi", "KullaniciAdi")});
            this.lueKullanicilar.Properties.DisplayMember = "KullaniciAdi";
            this.lueKullanicilar.Properties.DropDownRows = 15;
            this.lueKullanicilar.Properties.NullText = "";
            this.lueKullanicilar.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.lueKullanicilar.Properties.ShowHeader = false;
            this.lueKullanicilar.Properties.ShowPopupShadow = false;
            this.lueKullanicilar.Properties.ValueMember = "pkKullanicilar";
            this.lueKullanicilar.Size = new System.Drawing.Size(143, 31);
            this.lueKullanicilar.TabIndex = 96;
            this.lueKullanicilar.Tag = "0";
            this.lueKullanicilar.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl5.Location = new System.Drawing.Point(264, 274);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(588, 16);
            this.labelControl5.TabIndex = 95;
            this.labelControl5.Text = "HANGİ BİLGİSAYARDAN YAZDIRIYORSA BİLGİSAYAR TANIMLI YAZICI ÜZERİNDEN YAZDIRMA YAP" +
    "ILACAK";
            this.labelControl5.Visible = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl3.Location = new System.Drawing.Point(71, 227);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(78, 18);
            this.labelControl3.TabIndex = 95;
            this.labelControl3.Text = "Kullanıcı Adı";
            // 
            // lueSatisTipi
            // 
            this.lueSatisTipi.Location = new System.Drawing.Point(176, 150);
            this.lueSatisTipi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueSatisTipi.Name = "lueSatisTipi";
            this.lueSatisTipi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lueSatisTipi.Properties.Appearance.Options.UseFont = true;
            this.lueSatisTipi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueSatisTipi.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Durumu", "Durumu")});
            this.lueSatisTipi.Properties.DisplayMember = "Durumu";
            this.lueSatisTipi.Properties.DropDownRows = 15;
            this.lueSatisTipi.Properties.NullText = "";
            this.lueSatisTipi.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.lueSatisTipi.Properties.ShowHeader = false;
            this.lueSatisTipi.Properties.ShowPopupShadow = false;
            this.lueSatisTipi.Properties.ValueMember = "pkSatisDurumu";
            this.lueSatisTipi.Size = new System.Drawing.Size(143, 31);
            this.lueSatisTipi.TabIndex = 96;
            this.lueSatisTipi.Tag = "0";
            this.lueSatisTipi.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.lueSatisTipi.Visible = false;
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl10.Location = new System.Drawing.Point(92, 154);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(60, 18);
            this.labelControl10.TabIndex = 95;
            this.labelControl10.Text = "Satış Tipi";
            this.labelControl10.Visible = false;
            // 
            // lcDosyaYolu
            // 
            this.lcDosyaYolu.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lcDosyaYolu.Location = new System.Drawing.Point(44, 81);
            this.lcDosyaYolu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcDosyaYolu.Name = "lcDosyaYolu";
            this.lcDosyaYolu.Size = new System.Drawing.Size(113, 18);
            this.lcDosyaYolu.TabIndex = 94;
            this.lcDosyaYolu.Text = "Dosya Adı Giriniz";
            // 
            // dosyaadi
            // 
            this.dosyaadi.Location = new System.Drawing.Point(176, 71);
            this.dosyaadi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dosyaadi.Name = "dosyaadi";
            this.dosyaadi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.dosyaadi.Properties.Appearance.Options.UseFont = true;
            this.dosyaadi.Size = new System.Drawing.Size(308, 31);
            this.dosyaadi.TabIndex = 93;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Location = new System.Drawing.Point(15, 39);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(143, 18);
            this.labelControl1.TabIndex = 92;
            this.labelControl1.Text = "Yeni Rapor Adı Giriniz";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtDosyaYolu
            // 
            this.txtDosyaYolu.Location = new System.Drawing.Point(175, 338);
            this.txtDosyaYolu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDosyaYolu.Name = "txtDosyaYolu";
            this.txtDosyaYolu.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.txtDosyaYolu.Properties.Appearance.Options.UseFont = true;
            this.txtDosyaYolu.Properties.ReadOnly = true;
            this.txtDosyaYolu.Size = new System.Drawing.Size(499, 31);
            this.txtDosyaYolu.TabIndex = 0;
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl9.Location = new System.Drawing.Point(14, 344);
            this.labelControl9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(132, 18);
            this.labelControl9.TabIndex = 92;
            this.labelControl9.Text = "Rapor Dosyası Byte";
            // 
            // btnRaporYukle
            // 
            this.btnRaporYukle.Image = global::GPTS.Properties.Resources.Find;
            this.btnRaporYukle.Location = new System.Drawing.Point(680, 331);
            this.btnRaporYukle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRaporYukle.Name = "btnRaporYukle";
            this.btnRaporYukle.Size = new System.Drawing.Size(172, 38);
            this.btnRaporYukle.TabIndex = 218;
            this.btnRaporYukle.TabStop = false;
            this.btnRaporYukle.Text = "Rapor Dosyası Yükle";
            this.btnRaporYukle.Click += new System.EventHandler(this.btnRaporYukle_Click);
            // 
            // lbDurum
            // 
            this.lbDurum.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbDurum.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbDurum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbDurum.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.lbDurum.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbDurum.Location = new System.Drawing.Point(0, 395);
            this.lbDurum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbDurum.Name = "lbDurum";
            this.lbDurum.Size = new System.Drawing.Size(871, 26);
            this.lbDurum.TabIndex = 219;
            this.lbDurum.Text = "Durum: ";
            // 
            // frmFisYaziciYeniRapor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 519);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmFisYaziciYeniRapor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "0";
            this.Text = "Yeni / Düzenle";
            this.Load += new System.EventHandler(this.frmStokKoduverKarti_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStokKoduverKarti_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.GrupAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbOnizle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAktif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seSiraNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seYazdirmaAdedi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueKullanicilar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSatisTipi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dosyaadi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDosyaYolu.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit GrupAdi;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.LabelControl lcDosyaYolu;
        private DevExpress.XtraEditors.TextEdit dosyaadi;
        private DevExpress.XtraEditors.LookUpEdit lueSatisTipi;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton BtnGuncelle;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.ComboBox cbYazicilar;
        public System.Windows.Forms.TextBox pkSatisFisiSecimi;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SpinEdit seYazdirmaAdedi;
        public System.Windows.Forms.TextBox pkEtiketSablonlari;
        private DevExpress.XtraEditors.LookUpEdit lueKullanicilar;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControl1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SpinEdit seSiraNo;
        private DevExpress.XtraEditors.CheckEdit cbAktif;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.CheckEdit cbOnizle;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TextEdit txtDosyaYolu;
        private DevExpress.XtraEditors.SimpleButton btnRaporYukle;
        private DevExpress.XtraEditors.LabelControl lbDurum;
    }
}
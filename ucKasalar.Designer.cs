namespace GPTS
{
    partial class ucKasalar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gcKasaListesi = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.devirGirişiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.silToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sütunGörünümüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kaydetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.varsayılanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sütunSeçimiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnTrasnfer = new DevExpress.XtraEditors.SimpleButton();
            this.btnKasaTrasn = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton19 = new DevExpress.XtraEditors.SimpleButton();
            this.btnYeni = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gcKullanicilar = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gcKasaListesi)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcKullanicilar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // gcKasaListesi
            // 
            this.gcKasaListesi.ContextMenuStrip = this.contextMenuStrip1;
            this.gcKasaListesi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcKasaListesi.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcKasaListesi.Location = new System.Drawing.Point(0, 65);
            this.gcKasaListesi.MainView = this.gridView1;
            this.gcKasaListesi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcKasaListesi.Name = "gcKasaListesi";
            this.gcKasaListesi.Size = new System.Drawing.Size(1175, 198);
            this.gcKasaListesi.TabIndex = 4;
            this.gcKasaListesi.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.devirGirişiToolStripMenuItem,
            this.silToolStripMenuItem,
            this.sütunGörünümüToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(261, 118);
            // 
            // devirGirişiToolStripMenuItem
            // 
            this.devirGirişiToolStripMenuItem.Image = global::GPTS.Properties.Resources.kasatransfer_32x32;
            this.devirGirişiToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.devirGirişiToolStripMenuItem.Name = "devirGirişiToolStripMenuItem";
            this.devirGirişiToolStripMenuItem.Size = new System.Drawing.Size(260, 38);
            this.devirGirişiToolStripMenuItem.Text = "Kasa  Bakiye Düzeltme";
            this.devirGirişiToolStripMenuItem.Click += new System.EventHandler(this.devirGirişiToolStripMenuItem_Click);
            // 
            // silToolStripMenuItem
            // 
            this.silToolStripMenuItem.Image = global::GPTS.Properties.Resources.DeleteRed;
            this.silToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.silToolStripMenuItem.Name = "silToolStripMenuItem";
            this.silToolStripMenuItem.Size = new System.Drawing.Size(260, 38);
            this.silToolStripMenuItem.Text = "Kasa  Sil";
            this.silToolStripMenuItem.Click += new System.EventHandler(this.silToolStripMenuItem_Click);
            // 
            // sütunGörünümüToolStripMenuItem
            // 
            this.sütunGörünümüToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kaydetToolStripMenuItem,
            this.varsayılanToolStripMenuItem,
            this.sütunSeçimiToolStripMenuItem});
            this.sütunGörünümüToolStripMenuItem.Image = global::GPTS.Properties.Resources.grup_32x32;
            this.sütunGörünümüToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.sütunGörünümüToolStripMenuItem.Name = "sütunGörünümüToolStripMenuItem";
            this.sütunGörünümüToolStripMenuItem.Size = new System.Drawing.Size(260, 38);
            this.sütunGörünümüToolStripMenuItem.Text = "Sütun Görünümü";
            // 
            // kaydetToolStripMenuItem
            // 
            this.kaydetToolStripMenuItem.Image = global::GPTS.Properties.Resources.onay_32x32;
            this.kaydetToolStripMenuItem.Name = "kaydetToolStripMenuItem";
            this.kaydetToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.kaydetToolStripMenuItem.Text = "Kaydet";
            this.kaydetToolStripMenuItem.Click += new System.EventHandler(this.kaydetToolStripMenuItem_Click);
            // 
            // varsayılanToolStripMenuItem
            // 
            this.varsayılanToolStripMenuItem.Name = "varsayılanToolStripMenuItem";
            this.varsayılanToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.varsayılanToolStripMenuItem.Text = "Varsayılan";
            this.varsayılanToolStripMenuItem.Click += new System.EventHandler(this.varsayılanToolStripMenuItem_Click);
            // 
            // sütunSeçimiToolStripMenuItem
            // 
            this.sütunSeçimiToolStripMenuItem.Image = global::GPTS.Properties.Resources.grup2_32x32;
            this.sütunSeçimiToolStripMenuItem.Name = "sütunSeçimiToolStripMenuItem";
            this.sütunSeçimiToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.sütunSeçimiToolStripMenuItem.Text = "Sütun Seçimi";
            this.sütunSeçimiToolStripMenuItem.Click += new System.EventHandler(this.sütunSeçimiToolStripMenuItem_Click);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn14});
            this.gridView1.GridControl = this.gcKasaListesi;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowViewCaption = true;
            this.gridView1.ViewCaption = "Kasa Listesi";
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "Kasa Id";
            this.gridColumn1.FieldName = "PkKasalar";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "PkKasalar", "İşlem Sayısı=")});
            this.gridColumn1.Width = 136;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Kasa Adı";
            this.gridColumn2.FieldName = "KasaAdi";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 165;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Kasaya Giren";
            this.gridColumn4.DisplayFormat.FormatString = "{0:n}";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn4.FieldName = "Borc";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Borc", "{0:n}")});
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 165;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Kasadan Çıkan";
            this.gridColumn3.DisplayFormat.FormatString = "{0:n}";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn3.FieldName = "Alacak";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Alacak", "{0:n}")});
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 165;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "Kasadaki Para";
            this.gridColumn5.DisplayFormat.FormatString = "{0:n}";
            this.gridColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn5.FieldName = "Bakiye";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Bakiye", "{0:n}")});
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 172;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "Durumu";
            this.gridColumn6.FieldName = "Aktif";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 103;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "fkKasalar";
            this.gridColumn7.FieldName = "fkKasalar";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "SubeId";
            this.gridColumn14.FieldName = "fkSube";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 5;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnTrasnfer);
            this.panelControl1.Controls.Add(this.btnKasaTrasn);
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton19);
            this.panelControl1.Controls.Add(this.btnYeni);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1175, 65);
            this.panelControl1.TabIndex = 5;
            // 
            // btnTrasnfer
            // 
            this.btnTrasnfer.Appearance.Options.UseTextOptions = true;
            this.btnTrasnfer.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnTrasnfer.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTrasnfer.Image = global::GPTS.Properties.Resources.Refresh_32x32;
            this.btnTrasnfer.Location = new System.Drawing.Point(870, 2);
            this.btnTrasnfer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTrasnfer.Name = "btnTrasnfer";
            this.btnTrasnfer.Size = new System.Drawing.Size(120, 61);
            this.btnTrasnfer.TabIndex = 25;
            this.btnTrasnfer.Text = "Kasadan Bankaya Transfer";
            this.btnTrasnfer.Click += new System.EventHandler(this.btnTrasnfer_Click);
            // 
            // btnKasaTrasn
            // 
            this.btnKasaTrasn.Appearance.Options.UseTextOptions = true;
            this.btnKasaTrasn.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnKasaTrasn.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnKasaTrasn.Image = global::GPTS.Properties.Resources.Refresh_32x32;
            this.btnKasaTrasn.Location = new System.Drawing.Point(750, 2);
            this.btnKasaTrasn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnKasaTrasn.Name = "btnKasaTrasn";
            this.btnKasaTrasn.Size = new System.Drawing.Size(120, 61);
            this.btnKasaTrasn.TabIndex = 26;
            this.btnKasaTrasn.Text = "Kasa Transfer";
            this.btnKasaTrasn.Click += new System.EventHandler(this.btnKasaTrasn_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = global::GPTS.Properties.Resources.kasaraporu_32x48;
            this.simpleButton4.Location = new System.Drawing.Point(530, 2);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(220, 61);
            this.simpleButton4.TabIndex = 27;
            this.simpleButton4.Text = "Kasa Transfer Raporları";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton3.Image = global::GPTS.Properties.Resources.kasaraporu_32x48;
            this.simpleButton3.Location = new System.Drawing.Point(381, 2);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(149, 61);
            this.simpleButton3.TabIndex = 24;
            this.simpleButton3.Text = "Kasa Raporları";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.kasacikis_32x32;
            this.simpleButton2.Location = new System.Drawing.Point(252, 2);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(129, 61);
            this.simpleButton2.TabIndex = 23;
            this.simpleButton2.Text = "Kasa Çıkışı";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton19
            // 
            this.simpleButton19.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton19.Appearance.Options.UseFont = true;
            this.simpleButton19.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton19.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton19.Location = new System.Drawing.Point(1023, 2);
            this.simpleButton19.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton19.Name = "simpleButton19";
            this.simpleButton19.Size = new System.Drawing.Size(150, 61);
            this.simpleButton19.TabIndex = 22;
            this.simpleButton19.Text = "Kapat [ESC]";
            this.simpleButton19.Click += new System.EventHandler(this.simpleButton19_Click);
            // 
            // btnYeni
            // 
            this.btnYeni.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnYeni.Image = global::GPTS.Properties.Resources.kasagiris_32x32;
            this.btnYeni.Location = new System.Drawing.Point(123, 2);
            this.btnYeni.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnYeni.Name = "btnYeni";
            this.btnYeni.Size = new System.Drawing.Size(129, 61);
            this.btnYeni.TabIndex = 0;
            this.btnYeni.Text = "Kasa Girişi";
            this.btnYeni.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton1.Image = global::GPTS.Properties.Resources.kasaekle_32x48;
            this.simpleButton1.Location = new System.Drawing.Point(2, 2);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(121, 61);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Yeni Kasa";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click_1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gcKullanicilar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 263);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1175, 240);
            this.panel1.TabIndex = 6;
            // 
            // gcKullanicilar
            // 
            this.gcKullanicilar.Dock = System.Windows.Forms.DockStyle.Left;
            this.gcKullanicilar.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcKullanicilar.Location = new System.Drawing.Point(0, 0);
            this.gcKullanicilar.MainView = this.gridView2;
            this.gcKullanicilar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcKullanicilar.Name = "gcKullanicilar";
            this.gcKullanicilar.Size = new System.Drawing.Size(356, 240);
            this.gcKullanicilar.TabIndex = 1;
            this.gcKullanicilar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13});
            this.gridView2.GridControl = this.gcKullanicilar;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowViewCaption = true;
            this.gridView2.ViewCaption = "Kullanıcı Listesi";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "pkKullanicilar";
            this.gridColumn8.FieldName = "pkKullanicilar";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 0;
            this.gridColumn8.Width = 20;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Kullanıcı Adı";
            this.gridColumn9.FieldName = "KullaniciAdi";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            this.gridColumn9.Width = 147;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Şifre";
            this.gridColumn10.FieldName = "Sifre";
            this.gridColumn10.Name = "gridColumn10";
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "adisoyadi";
            this.gridColumn11.FieldName = "adisoyadi";
            this.gridColumn11.Name = "gridColumn11";
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "durumu";
            this.gridColumn12.FieldName = "durumu";
            this.gridColumn12.Name = "gridColumn12";
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "Cep";
            this.gridColumn13.FieldName = "Cep";
            this.gridColumn13.Name = "gridColumn13";
            // 
            // ucKasalar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcKasaListesi);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ucKasalar";
            this.Size = new System.Drawing.Size(1175, 503);
            this.Load += new System.EventHandler(this.ucKasalar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcKasaListesi)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcKullanicilar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraGrid.GridControl gcKasaListesi;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnYeni;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton19;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem devirGirişiToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private System.Windows.Forms.ToolStripMenuItem silToolStripMenuItem;
        private DevExpress.XtraEditors.SimpleButton btnTrasnfer;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gcKullanicilar;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraEditors.SimpleButton btnKasaTrasn;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private System.Windows.Forms.ToolStripMenuItem sütunGörünümüToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kaydetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem varsayılanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sütunSeçimiToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
    }
}

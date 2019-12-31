namespace GPTS
{
    partial class frmKasaGiderTablosu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKasaGiderTablosu));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.lueKasa = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.cbTarihAraligi = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.sondate = new DevExpress.XtraEditors.DateEdit();
            this.ilkdate = new DevExpress.XtraEditors.DateEdit();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.btnListele = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton19 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEPosta = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.pivotGridControl1 = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.pivotGridField2 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField3 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField1 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField4 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kasaGirişiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kasaÇıkışıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.düzeltToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kasaDevirBakiyeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.müşteriHareketleriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.müşteriKArtıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueKasa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTarihAraligi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sondate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sondate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilkdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilkdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.CaptionImage = global::GPTS.Properties.Resources.kasaraporu_32x32;
            this.groupControl1.Controls.Add(this.groupControl4);
            this.groupControl1.Controls.Add(this.panelControl4);
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1269, 100);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Gelir-Gider";
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.lueKasa);
            this.groupControl4.Controls.Add(this.labelControl6);
            this.groupControl4.Controls.Add(this.cbTarihAraligi);
            this.groupControl4.Controls.Add(this.labelControl1);
            this.groupControl4.Controls.Add(this.sondate);
            this.groupControl4.Controls.Add(this.ilkdate);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl4.Location = new System.Drawing.Point(206, 40);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.ShowCaption = false;
            this.groupControl4.Size = new System.Drawing.Size(796, 58);
            this.groupControl4.TabIndex = 35;
            // 
            // lueKasa
            // 
            this.lueKasa.Enabled = false;
            this.lueKasa.Location = new System.Drawing.Point(559, 29);
            this.lueKasa.Name = "lueKasa";
            this.lueKasa.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lueKasa.Properties.Appearance.Options.UseFont = true;
            this.lueKasa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueKasa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkKasalar", "pkKasalar", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("KasaAdi", "Kasa Adi")});
            this.lueKasa.Properties.DisplayMember = "pkKasalar";
            this.lueKasa.Properties.NullText = "Seçiniz...";
            this.lueKasa.Properties.ValueMember = "KasaAdi";
            this.lueKasa.Size = new System.Drawing.Size(110, 20);
            this.lueKasa.TabIndex = 43;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(559, 10);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(55, 13);
            this.labelControl6.TabIndex = 42;
            this.labelControl6.Text = "Kasa Listesi";
            // 
            // cbTarihAraligi
            // 
            this.cbTarihAraligi.Location = new System.Drawing.Point(19, 29);
            this.cbTarihAraligi.Name = "cbTarihAraligi";
            this.cbTarihAraligi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbTarihAraligi.Properties.Items.AddRange(new object[] {
            "Bugün",
            "Dün",
            "Geçen Hafta",
            "Bu Hafta",
            "Bu Ay",
            "Önceki Ay",
            "Bu Yıl",
            "Özel"});
            this.cbTarihAraligi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbTarihAraligi.Size = new System.Drawing.Size(110, 20);
            this.cbTarihAraligi.TabIndex = 41;
            this.cbTarihAraligi.SelectedIndexChanged += new System.EventHandler(this.cbTarihAraligi_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 11);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(56, 13);
            this.labelControl1.TabIndex = 40;
            this.labelControl1.Text = "Tarih Aralığı";
            // 
            // sondate
            // 
            this.sondate.EditValue = new System.DateTime(2012, 1, 21, 19, 19, 36, 0);
            this.sondate.Location = new System.Drawing.Point(347, 29);
            this.sondate.Name = "sondate";
            this.sondate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sondate.Properties.DisplayFormat.FormatString = "D";
            this.sondate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.sondate.Properties.EditFormat.FormatString = "D";
            this.sondate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.sondate.Properties.Mask.EditMask = "G";
            this.sondate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sondate.Size = new System.Drawing.Size(206, 20);
            this.sondate.TabIndex = 18;
            // 
            // ilkdate
            // 
            this.ilkdate.EditValue = new System.DateTime(2012, 1, 21, 19, 19, 36, 0);
            this.ilkdate.Location = new System.Drawing.Point(135, 29);
            this.ilkdate.Name = "ilkdate";
            this.ilkdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ilkdate.Properties.DisplayFormat.FormatString = "D";
            this.ilkdate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ilkdate.Properties.EditFormat.FormatString = "D";
            this.ilkdate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ilkdate.Properties.Mask.EditMask = "G";
            this.ilkdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.ilkdate.Size = new System.Drawing.Size(206, 20);
            this.ilkdate.TabIndex = 17;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.btnListele);
            this.panelControl4.Controls.Add(this.simpleButton2);
            this.panelControl4.Controls.Add(this.simpleButton3);
            this.panelControl4.Controls.Add(this.simpleButton19);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl4.Location = new System.Drawing.Point(1002, 40);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(265, 58);
            this.panelControl4.TabIndex = 39;
            // 
            // btnListele
            // 
            this.btnListele.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnListele.Image = global::GPTS.Properties.Resources.listele;
            this.btnListele.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btnListele.Location = new System.Drawing.Point(2, 2);
            this.btnListele.Name = "btnListele";
            this.btnListele.Size = new System.Drawing.Size(82, 54);
            this.btnListele.TabIndex = 1;
            this.btnListele.Text = "Listele";
            this.btnListele.Click += new System.EventHandler(this.btnListele_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.Printer_2;
            this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton2.Location = new System.Drawing.Point(84, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(63, 54);
            this.simpleButton2.TabIndex = 33;
            this.simpleButton2.Text = "Yazdır";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton3.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton3.Location = new System.Drawing.Point(147, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(27, 54);
            this.simpleButton3.TabIndex = 34;
            this.simpleButton3.Text = "...";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton19
            // 
            this.simpleButton19.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton19.Appearance.Options.UseFont = true;
            this.simpleButton19.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton19.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton19.Image")));
            this.simpleButton19.Location = new System.Drawing.Point(174, 2);
            this.simpleButton19.Name = "simpleButton19";
            this.simpleButton19.Size = new System.Drawing.Size(89, 54);
            this.simpleButton19.TabIndex = 35;
            this.simpleButton19.Text = "Kapat\r\n[ESC]";
            this.simpleButton19.Click += new System.EventHandler(this.simpleButton19_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEPosta);
            this.panel1.Controls.Add(this.simpleButton4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(2, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 58);
            this.panel1.TabIndex = 40;
            // 
            // btnEPosta
            // 
            this.btnEPosta.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnEPosta.Image = ((System.Drawing.Image)(resources.GetObject("btnEPosta.Image")));
            this.btnEPosta.Location = new System.Drawing.Point(103, 0);
            this.btnEPosta.Name = "btnEPosta";
            this.btnEPosta.Size = new System.Drawing.Size(101, 58);
            this.btnEPosta.TabIndex = 93;
            this.btnEPosta.Text = "Kasa Çıkışı";
            this.btnEPosta.Click += new System.EventHandler(this.kasaÇıkışıToolStripMenuItem_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(0, 0);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(103, 58);
            this.simpleButton4.TabIndex = 94;
            this.simpleButton4.Text = "Kasa Girişi";
            this.simpleButton4.Click += new System.EventHandler(this.kasaGirişiToolStripMenuItem_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.pivotGridControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 100);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.ShowCaption = false;
            this.groupControl2.Size = new System.Drawing.Size(1269, 309);
            this.groupControl2.TabIndex = 5;
            // 
            // pivotGridControl1
            // 
            this.pivotGridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pivotGridControl1.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.pivotGridField2,
            this.pivotGridField3,
            this.pivotGridField1,
            this.pivotGridField4});
            this.pivotGridControl1.Location = new System.Drawing.Point(2, 2);
            this.pivotGridControl1.Name = "pivotGridControl1";
            this.pivotGridControl1.OptionsChartDataSource.FieldValuesProvideMode = DevExpress.XtraPivotGrid.PivotChartFieldValuesProvideMode.DisplayText;
            this.pivotGridControl1.OptionsChartDataSource.ProvideColumnCustomTotals = false;
            this.pivotGridControl1.OptionsCustomization.AllowFilter = false;
            this.pivotGridControl1.OptionsView.ShowColumnGrandTotalHeader = false;
            this.pivotGridControl1.OptionsView.ShowFilterHeaders = false;
            this.pivotGridControl1.OptionsView.ShowFilterSeparatorBar = false;
            this.pivotGridControl1.Size = new System.Drawing.Size(757, 305);
            this.pivotGridControl1.TabIndex = 96;
            // 
            // pivotGridField2
            // 
            this.pivotGridField2.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField2.AreaIndex = 0;
            this.pivotGridField2.Caption = "Ciro";
            this.pivotGridField2.CellFormat.FormatString = "{0:c}";
            this.pivotGridField2.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.pivotGridField2.FieldName = "ToplamTutar";
            this.pivotGridField2.Name = "pivotGridField2";
            this.pivotGridField2.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Descending;
            this.pivotGridField2.Width = 117;
            // 
            // pivotGridField3
            // 
            this.pivotGridField3.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField3.AreaIndex = 0;
            this.pivotGridField3.Caption = "Tarih";
            this.pivotGridField3.FieldName = "Tarih";
            this.pivotGridField3.Name = "pivotGridField3";
            this.pivotGridField3.UnboundFieldName = "pivotGridField3";
            // 
            // pivotGridField1
            // 
            this.pivotGridField1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField1.AreaIndex = 1;
            this.pivotGridField1.Caption = "Kar Tutar";
            this.pivotGridField1.CellFormat.FormatString = "{0:c}";
            this.pivotGridField1.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.pivotGridField1.FieldName = "Kar";
            this.pivotGridField1.Name = "pivotGridField1";
            this.pivotGridField1.Width = 89;
            // 
            // pivotGridField4
            // 
            this.pivotGridField4.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField4.AreaIndex = 1;
            this.pivotGridField4.Caption = "Tür";
            this.pivotGridField4.FieldName = "tur";
            this.pivotGridField4.Name = "pivotGridField4";
            this.pivotGridField4.Width = 52;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kasaGirişiToolStripMenuItem,
            this.kasaÇıkışıToolStripMenuItem,
            this.toolStripMenuItem2,
            this.düzeltToolStripMenuItem,
            this.kasaDevirBakiyeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.müşteriHareketleriToolStripMenuItem,
            this.müşteriKArtıToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(224, 276);
            // 
            // kasaGirişiToolStripMenuItem
            // 
            this.kasaGirişiToolStripMenuItem.Image = global::GPTS.Properties.Resources.kasagiris_32x32;
            this.kasaGirişiToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.kasaGirişiToolStripMenuItem.Name = "kasaGirişiToolStripMenuItem";
            this.kasaGirişiToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.kasaGirişiToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            this.kasaGirişiToolStripMenuItem.Text = "Kasa Girişi";
            this.kasaGirişiToolStripMenuItem.Click += new System.EventHandler(this.kasaGirişiToolStripMenuItem_Click);
            // 
            // kasaÇıkışıToolStripMenuItem
            // 
            this.kasaÇıkışıToolStripMenuItem.Image = global::GPTS.Properties.Resources.kasacikis_32x32;
            this.kasaÇıkışıToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.kasaÇıkışıToolStripMenuItem.Name = "kasaÇıkışıToolStripMenuItem";
            this.kasaÇıkışıToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.kasaÇıkışıToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            this.kasaÇıkışıToolStripMenuItem.Text = "Kasa Çıkışı";
            this.kasaÇıkışıToolStripMenuItem.Click += new System.EventHandler(this.kasaÇıkışıToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(220, 6);
            // 
            // düzeltToolStripMenuItem
            // 
            this.düzeltToolStripMenuItem.Image = global::GPTS.Properties.Resources.edit;
            this.düzeltToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.düzeltToolStripMenuItem.Name = "düzeltToolStripMenuItem";
            this.düzeltToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            this.düzeltToolStripMenuItem.Text = "Düzelt";
            this.düzeltToolStripMenuItem.Click += new System.EventHandler(this.düzeltToolStripMenuItem_Click);
            // 
            // kasaDevirBakiyeToolStripMenuItem
            // 
            this.kasaDevirBakiyeToolStripMenuItem.Image = global::GPTS.Properties.Resources.kasatransfer_32x32;
            this.kasaDevirBakiyeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.kasaDevirBakiyeToolStripMenuItem.Name = "kasaDevirBakiyeToolStripMenuItem";
            this.kasaDevirBakiyeToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            this.kasaDevirBakiyeToolStripMenuItem.Text = "Kasa  Bakiye Düzeltme";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::GPTS.Properties.Resources.DeleteRed;
            this.toolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(223, 38);
            this.toolStripMenuItem1.Text = "Seçilen Kaydı Sil";
            // 
            // müşteriHareketleriToolStripMenuItem
            // 
            this.müşteriHareketleriToolStripMenuItem.Image = global::GPTS.Properties.Resources.musterihareketleri_32x32;
            this.müşteriHareketleriToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.müşteriHareketleriToolStripMenuItem.Name = "müşteriHareketleriToolStripMenuItem";
            this.müşteriHareketleriToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            this.müşteriHareketleriToolStripMenuItem.Text = "Müşteri Hareketleri";
            this.müşteriHareketleriToolStripMenuItem.Click += new System.EventHandler(this.müşteriHareketleriToolStripMenuItem_Click);
            // 
            // müşteriKArtıToolStripMenuItem
            // 
            this.müşteriKArtıToolStripMenuItem.Image = global::GPTS.Properties.Resources.musteri_32x321;
            this.müşteriKArtıToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.müşteriKArtıToolStripMenuItem.Name = "müşteriKArtıToolStripMenuItem";
            this.müşteriKArtıToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            this.müşteriKArtıToolStripMenuItem.Text = "Müşteri Kartı";
            this.müşteriKArtıToolStripMenuItem.Click += new System.EventHandler(this.müşteriKArtıToolStripMenuItem_Click);
            // 
            // frmKasaGiderTablosu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1269, 409);
            this.ControlBox = false;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.KeyPreview = true;
            this.Name = "frmKasaGiderTablosu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "1";
            this.Text = "Kasa Gider Hareketleri";
            this.Load += new System.EventHandler(this.ucKasaHareketleri_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmKasaHareketleri_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            this.groupControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueKasa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTarihAraligi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sondate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sondate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilkdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilkdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btnListele;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.DateEdit sondate;
        private DevExpress.XtraEditors.DateEdit ilkdate;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cbTarihAraligi;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ToolStripMenuItem kasaGirişiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kasaÇıkışıToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem düzeltToolStripMenuItem;
        private DevExpress.XtraEditors.SimpleButton simpleButton19;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem kasaDevirBakiyeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem müşteriHareketleriToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem müşteriKArtıToolStripMenuItem;
        private DevExpress.XtraEditors.SimpleButton btnEPosta;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.LookUpEdit lueKasa;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraPivotGrid.PivotGridControl pivotGridControl1;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField2;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField3;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField1;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField4;
    }
}

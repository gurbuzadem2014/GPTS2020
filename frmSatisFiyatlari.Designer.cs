namespace GPTS
{
    partial class frmSatisFiyatlari
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSatisFiyatlari));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.topluFiyatDeğiştirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tümünüAktifYapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tümünüPasifYapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCalcEditKdvDahil = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCalcEditKdvHaric = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.ımageList1 = new System.Windows.Forms.ImageList(this.components);
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btTamam = new DevExpress.XtraEditors.SimpleButton();
            this.kdv = new DevExpress.XtraEditors.SpinEdit();
            this.pkStokKarti = new System.Windows.Forms.TextBox();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.sbtnSil = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.baslik = new System.Windows.Forms.Label();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEditKdvDahil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEditKdvHaric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kdv.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 105);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageComboBox1,
            this.repositoryItemCalcEditKdvDahil,
            this.repositoryItemCalcEditKdvHaric,
            this.repositoryItemSpinEdit1,
            this.repositoryItemHyperLinkEdit1});
            this.gridControl1.Size = new System.Drawing.Size(692, 379);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topluFiyatDeğiştirToolStripMenuItem,
            this.toolStripMenuItem1,
            this.tümünüAktifYapToolStripMenuItem,
            this.tümünüPasifYapToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(207, 82);
            // 
            // topluFiyatDeğiştirToolStripMenuItem
            // 
            this.topluFiyatDeğiştirToolStripMenuItem.Name = "topluFiyatDeğiştirToolStripMenuItem";
            this.topluFiyatDeğiştirToolStripMenuItem.Size = new System.Drawing.Size(206, 24);
            this.topluFiyatDeğiştirToolStripMenuItem.Text = "Toplu Fiyat Değiştir";
            this.topluFiyatDeğiştirToolStripMenuItem.Click += new System.EventHandler(this.topluFiyatDeğiştirToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(203, 6);
            // 
            // tümünüAktifYapToolStripMenuItem
            // 
            this.tümünüAktifYapToolStripMenuItem.Name = "tümünüAktifYapToolStripMenuItem";
            this.tümünüAktifYapToolStripMenuItem.Size = new System.Drawing.Size(206, 24);
            this.tümünüAktifYapToolStripMenuItem.Text = "Tümünü Aktif Yap";
            this.tümünüAktifYapToolStripMenuItem.Click += new System.EventHandler(this.tümünüAktifYapToolStripMenuItem_Click);
            // 
            // tümünüPasifYapToolStripMenuItem
            // 
            this.tümünüPasifYapToolStripMenuItem.Name = "tümünüPasifYapToolStripMenuItem";
            this.tümünüPasifYapToolStripMenuItem.Size = new System.Drawing.Size(206, 24);
            this.tümünüPasifYapToolStripMenuItem.Text = "Tümünü Pasif Yap";
            this.tümünüPasifYapToolStripMenuItem.Click += new System.EventHandler(this.tümünüPasifYapToolStripMenuItem_Click);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridView1.ColumnPanelRowHeight = 40;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick_1);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "pkSatisFiyatlari";
            this.gridColumn1.FieldName = "pkSatisFiyatlari";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn2.Caption = "Satış Fiyat Grubu";
            this.gridColumn2.FieldName = "Baslik";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 351;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn3.Caption = "Satış Fiyatı Kdv Dahil";
            this.gridColumn3.ColumnEdit = this.repositoryItemCalcEditKdvDahil;
            this.gridColumn3.DisplayFormat.FormatString = "{0:#0.00####}";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn3.FieldName = "SatisFiyatiKdvli";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 283;
            // 
            // repositoryItemCalcEditKdvDahil
            // 
            this.repositoryItemCalcEditKdvDahil.AccessibleDescription = "Kdv Dahil";
            this.repositoryItemCalcEditKdvDahil.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.repositoryItemCalcEditKdvDahil.AppearanceFocused.Options.UseBackColor = true;
            this.repositoryItemCalcEditKdvDahil.AutoHeight = false;
            this.repositoryItemCalcEditKdvDahil.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEditKdvDahil.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemCalcEditKdvDahil.Name = "repositoryItemCalcEditKdvDahil";
            this.repositoryItemCalcEditKdvDahil.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repositoryItemCalcEditKdvDahil_KeyDown);
            this.repositoryItemCalcEditKdvDahil.Leave += new System.EventHandler(this.repositoryItemCalcEditKdvDahil_Leave);
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn4.Caption = "Satış Fiyatı Kdv Hariç";
            this.gridColumn4.ColumnEdit = this.repositoryItemCalcEditKdvHaric;
            this.gridColumn4.DisplayFormat.FormatString = "{0:#0.00####}";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn4.FieldName = "SatisFiyatiKdvsiz";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Width = 89;
            // 
            // repositoryItemCalcEditKdvHaric
            // 
            this.repositoryItemCalcEditKdvHaric.AccessibleDescription = "Kdv Hariç";
            this.repositoryItemCalcEditKdvHaric.AutoHeight = false;
            this.repositoryItemCalcEditKdvHaric.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEditKdvHaric.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemCalcEditKdvHaric.Name = "repositoryItemCalcEditKdvHaric";
            this.repositoryItemCalcEditKdvHaric.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repositoryItemCalcEditKdvHaric_KeyDown);
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn5.Caption = "İskonto (%)";
            this.gridColumn5.ColumnEdit = this.repositoryItemSpinEdit1;
            this.gridColumn5.FieldName = "iskontoYuzde";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Width = 67;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AccessibleDescription = "iskonto";
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            this.repositoryItemSpinEdit1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repositoryItemSpinEdit1_KeyDown);
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "pkSatisFiyatlariBaslik";
            this.gridColumn6.FieldName = "pkSatisFiyatlariBaslik";
            this.gridColumn6.Name = "gridColumn6";
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Aktif";
            this.gridColumn7.FieldName = "Aktif";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 3;
            // 
            // repositoryItemImageComboBox1
            // 
            this.repositoryItemImageComboBox1.AutoHeight = false;
            this.repositoryItemImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox1.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Aktif", true, 0),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Pasif", false, 1)});
            this.repositoryItemImageComboBox1.LargeImages = this.ımageList1;
            this.repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1";
            // 
            // ımageList1
            // 
            this.ımageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ımageList1.ImageStream")));
            this.ımageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ımageList1.Images.SetKeyName(0, "camera_test.png");
            this.ımageList1.Images.SetKeyName(1, "editdelete1.png");
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.Image = global::GPTS.Properties.Resources.db_delete;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btTamam);
            this.panelControl1.Controls.Add(this.kdv);
            this.panelControl1.Controls.Add(this.pkStokKarti);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.sbtnSil);
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(692, 68);
            this.panelControl1.TabIndex = 2;
            // 
            // btTamam
            // 
            this.btTamam.Dock = System.Windows.Forms.DockStyle.Right;
            this.btTamam.Image = global::GPTS.Properties.Resources.onay_32x32;
            this.btTamam.Location = new System.Drawing.Point(346, 2);
            this.btTamam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btTamam.Name = "btTamam";
            this.btTamam.Size = new System.Drawing.Size(100, 64);
            this.btTamam.TabIndex = 166;
            this.btTamam.Text = "Tamam";
            this.btTamam.Visible = false;
            this.btTamam.Click += new System.EventHandler(this.btTamam_Click);
            // 
            // kdv
            // 
            this.kdv.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.kdv.Location = new System.Drawing.Point(325, 36);
            this.kdv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.kdv.Name = "kdv";
            this.kdv.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.kdv.Size = new System.Drawing.Size(68, 22);
            this.kdv.TabIndex = 165;
            this.kdv.Visible = false;
            // 
            // pkStokKarti
            // 
            this.pkStokKarti.Location = new System.Drawing.Point(324, 6);
            this.pkStokKarti.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkStokKarti.Name = "pkStokKarti";
            this.pkStokKarti.Size = new System.Drawing.Size(67, 23);
            this.pkStokKarti.TabIndex = 164;
            this.pkStokKarti.Visible = false;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(446, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(120, 64);
            this.BtnKaydet.TabIndex = 25;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // sbtnSil
            // 
            this.sbtnSil.Dock = System.Windows.Forms.DockStyle.Left;
            this.sbtnSil.Image = global::GPTS.Properties.Resources.DeleteRed5;
            this.sbtnSil.Location = new System.Drawing.Point(179, 2);
            this.sbtnSil.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sbtnSil.Name = "sbtnSil";
            this.sbtnSil.Size = new System.Drawing.Size(138, 64);
            this.sbtnSil.TabIndex = 84;
            this.sbtnSil.Text = "Fiyat Grubu Sil\r\n[F5]";
            this.sbtnSil.Click += new System.EventHandler(this.sbtnSil_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = global::GPTS.Properties.Resources.new_window;
            this.simpleButton4.Location = new System.Drawing.Point(2, 2);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(177, 64);
            this.simpleButton4.TabIndex = 85;
            this.simpleButton4.Text = "Yeni Fiyat Grubu\r\n[F7]";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(566, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(124, 64);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // baslik
            // 
            this.baslik.BackColor = System.Drawing.Color.SkyBlue;
            this.baslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.baslik.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baslik.ForeColor = System.Drawing.Color.Black;
            this.baslik.Location = new System.Drawing.Point(0, 68);
            this.baslik.Name = "baslik";
            this.baslik.Size = new System.Drawing.Size(692, 37);
            this.baslik.TabIndex = 91;
            this.baslik.Text = "STOK ADI";
            this.baslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Tur";
            this.gridColumn8.FieldName = "Tur";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 1;
            this.gridColumn8.Width = 66;
            // 
            // frmSatisFiyatlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 484);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.baslik);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSatisFiyatlari";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "0";
            this.Text = "Satış Fiyatları";
            this.Load += new System.EventHandler(this.frmSatisFiyatlari_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSatisFiyatlari_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEditKdvDahil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEditKdvHaric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kdv.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        public System.Windows.Forms.TextBox pkStokKarti;
        private DevExpress.XtraEditors.SimpleButton sbtnSil;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        private System.Windows.Forms.ImageList ımageList1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEditKdvDahil;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEditKdvHaric;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.SpinEdit kdv;
        private DevExpress.XtraEditors.SimpleButton btTamam;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        private System.Windows.Forms.Label baslik;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem topluFiyatDeğiştirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tümünüAktifYapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tümünüPasifYapToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
    }
}
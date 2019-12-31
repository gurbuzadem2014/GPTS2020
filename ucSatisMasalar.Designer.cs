namespace GPTS
{
    partial class ucSatisMasalar
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lueKullanicilar = new DevExpress.XtraEditors.LookUpEdit();
            this.lcKullanici = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton19 = new DevExpress.XtraEditors.SimpleButton();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.masaTanımlarıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.masaDizaynKaydetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masalarıTemizleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masaYerleriSıfırlaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fişSilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tümMasalarıSilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xTabStokGruplari = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.xTabBirimler = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueKullanicilar.Properties)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lueKullanicilar);
            this.panelControl1.Controls.Add(this.lcKullanici);
            this.panelControl1.Controls.Add(this.simpleButton19);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1133, 62);
            this.panelControl1.TabIndex = 5;
            // 
            // lueKullanicilar
            // 
            this.lueKullanicilar.Location = new System.Drawing.Point(88, 18);
            this.lueKullanicilar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueKullanicilar.Name = "lueKullanicilar";
            this.lueKullanicilar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lueKullanicilar.Properties.Appearance.Options.UseFont = true;
            this.lueKullanicilar.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.lueKullanicilar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueKullanicilar.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkKullanicilar", "pkKullanicilar", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("KullaniciAdi", "Kullanici Adı")});
            this.lueKullanicilar.Properties.DisplayMember = "KullaniciAdi";
            this.lueKullanicilar.Properties.DropDownRows = 15;
            this.lueKullanicilar.Properties.NullText = "";
            this.lueKullanicilar.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.lueKullanicilar.Properties.ShowHeader = false;
            this.lueKullanicilar.Properties.ShowPopupShadow = false;
            this.lueKullanicilar.Properties.ValueMember = "pkKullanicilar";
            this.lueKullanicilar.Size = new System.Drawing.Size(178, 27);
            this.lueKullanicilar.TabIndex = 36;
            this.lueKullanicilar.Tag = "0";
            this.lueKullanicilar.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            // 
            // lcKullanici
            // 
            this.lcKullanici.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.lcKullanici.Location = new System.Drawing.Point(31, 24);
            this.lcKullanici.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcKullanici.Name = "lcKullanici";
            this.lcKullanici.Size = new System.Drawing.Size(49, 16);
            this.lcKullanici.TabIndex = 35;
            this.lcKullanici.Tag = "0";
            this.lcKullanici.Text = "GARSON";
            // 
            // simpleButton19
            // 
            this.simpleButton19.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton19.Appearance.Options.UseFont = true;
            this.simpleButton19.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton19.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton19.Location = new System.Drawing.Point(1001, 2);
            this.simpleButton19.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton19.Name = "simpleButton19";
            this.simpleButton19.Size = new System.Drawing.Size(130, 58);
            this.simpleButton19.TabIndex = 22;
            this.simpleButton19.Text = "Kapat [ESC]";
            this.simpleButton19.Click += new System.EventHandler(this.simpleButton19_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.masaTanımlarıToolStripMenuItem,
            this.toolStripMenuItem1,
            this.masaDizaynKaydetToolStripMenuItem,
            this.masalarıTemizleToolStripMenuItem,
            this.masaYerleriSıfırlaToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(229, 162);
            // 
            // masaTanımlarıToolStripMenuItem
            // 
            this.masaTanımlarıToolStripMenuItem.Name = "masaTanımlarıToolStripMenuItem";
            this.masaTanımlarıToolStripMenuItem.Size = new System.Drawing.Size(228, 38);
            this.masaTanımlarıToolStripMenuItem.Text = "Masa Tanımları";
            this.masaTanımlarıToolStripMenuItem.Click += new System.EventHandler(this.masaTanımlarıToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(225, 6);
            // 
            // masaDizaynKaydetToolStripMenuItem
            // 
            this.masaDizaynKaydetToolStripMenuItem.Name = "masaDizaynKaydetToolStripMenuItem";
            this.masaDizaynKaydetToolStripMenuItem.Size = new System.Drawing.Size(228, 38);
            this.masaDizaynKaydetToolStripMenuItem.Text = "Masa Dizayn Kaydet";
            this.masaDizaynKaydetToolStripMenuItem.Click += new System.EventHandler(this.masaDizaynKaydetToolStripMenuItem_Click);
            // 
            // masalarıTemizleToolStripMenuItem
            // 
            this.masalarıTemizleToolStripMenuItem.Name = "masalarıTemizleToolStripMenuItem";
            this.masalarıTemizleToolStripMenuItem.Size = new System.Drawing.Size(228, 38);
            this.masalarıTemizleToolStripMenuItem.Text = "Masaları Temizle";
            this.masalarıTemizleToolStripMenuItem.Click += new System.EventHandler(this.masalarıTemizleToolStripMenuItem_Click);
            // 
            // masaYerleriSıfırlaToolStripMenuItem
            // 
            this.masaYerleriSıfırlaToolStripMenuItem.Image = global::GPTS.Properties.Resources.delete_icon_redman_resized_600_jpg_resized_600___Kopya;
            this.masaYerleriSıfırlaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.masaYerleriSıfırlaToolStripMenuItem.Name = "masaYerleriSıfırlaToolStripMenuItem";
            this.masaYerleriSıfırlaToolStripMenuItem.Size = new System.Drawing.Size(228, 38);
            this.masaYerleriSıfırlaToolStripMenuItem.Text = "Masa  Dizayn Sıfırla";
            this.masaYerleriSıfırlaToolStripMenuItem.Click += new System.EventHandler(this.masaYerleriSıfırlaToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fişSilToolStripMenuItem,
            this.tümMasalarıSilToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(188, 52);
            // 
            // fişSilToolStripMenuItem
            // 
            this.fişSilToolStripMenuItem.Name = "fişSilToolStripMenuItem";
            this.fişSilToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
            this.fişSilToolStripMenuItem.Text = "Fiş Sil";
            this.fişSilToolStripMenuItem.Click += new System.EventHandler(this.fişSilToolStripMenuItem_Click);
            // 
            // tümMasalarıSilToolStripMenuItem
            // 
            this.tümMasalarıSilToolStripMenuItem.Name = "tümMasalarıSilToolStripMenuItem";
            this.tümMasalarıSilToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
            this.tümMasalarıSilToolStripMenuItem.Text = "Tüm Masaları Sil";
            this.tümMasalarıSilToolStripMenuItem.Click += new System.EventHandler(this.tümMasalarıSilToolStripMenuItem_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 62);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xTabStokGruplari;
            this.xtraTabControl1.Size = new System.Drawing.Size(1133, 29);
            this.xtraTabControl1.TabIndex = 9;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xTabStokGruplari,
            this.xtraTabPage2,
            this.xTabBirimler,
            this.xtraTabPage1});
            // 
            // xTabStokGruplari
            // 
            this.xTabStokGruplari.Margin = new System.Windows.Forms.Padding(4);
            this.xTabStokGruplari.Name = "xTabStokGruplari";
            this.xTabStokGruplari.Size = new System.Drawing.Size(1127, 0);
            this.xTabStokGruplari.Text = "AÇIK MASALAR";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1127, 0);
            this.xtraTabPage2.Text = "BAHÇE";
            // 
            // xTabBirimler
            // 
            this.xTabBirimler.Margin = new System.Windows.Forms.Padding(4);
            this.xTabBirimler.Name = "xTabBirimler";
            this.xTabBirimler.Size = new System.Drawing.Size(1127, 0);
            this.xTabBirimler.Text = "SALON";
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1127, 0);
            this.xtraTabPage1.Text = "TÜM MASALAR";
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl1.Location = new System.Drawing.Point(0, 91);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHyperLinkEdit1,
            this.repositoryItemHyperLinkEdit2,
            this.repositoryItemHyperLinkEdit3,
            this.repositoryItemPictureEdit1});
            this.gridControl1.Size = new System.Drawing.Size(1133, 726);
            this.gridControl1.TabIndex = 10;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 50;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 20;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowHeight = 70;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "Id";
            this.gridColumn1.FieldName = "pkMasalar";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 84;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Masa Adı";
            this.gridColumn2.ColumnEdit = this.repositoryItemPictureEdit1;
            this.gridColumn2.FieldName = "masa_adi";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 181;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Appearance.Image = global::GPTS.Properties.Resources.bos;
            this.repositoryItemPictureEdit1.Appearance.Options.UseImage = true;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.NullText = "Masa Boş";
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Müşteri Adı";
            this.gridColumn3.FieldName = "fkMasaGruplari";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 272;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.Caption = "Toplam Tutar (Ödeme Al)";
            this.gridColumn4.ColumnEdit = this.repositoryItemHyperLinkEdit3;
            this.gridColumn4.FieldName = "sira_no";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 197;
            // 
            // repositoryItemHyperLinkEdit3
            // 
            this.repositoryItemHyperLinkEdit3.AutoHeight = false;
            this.repositoryItemHyperLinkEdit3.Image = global::GPTS.Properties.Resources.odemeal_32x32;
            this.repositoryItemHyperLinkEdit3.Name = "repositoryItemHyperLinkEdit3";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Toplam Saat";
            this.gridColumn5.FieldName = "gen";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 221;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Sil";
            this.gridColumn6.ColumnEdit = this.repositoryItemHyperLinkEdit2;
            this.gridColumn6.FieldName = "yuk";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 6;
            this.gridColumn6.Width = 136;
            // 
            // repositoryItemHyperLinkEdit2
            // 
            this.repositoryItemHyperLinkEdit2.AutoHeight = false;
            this.repositoryItemHyperLinkEdit2.Image = global::GPTS.Properties.Resources.DeleteRed1;
            this.repositoryItemHyperLinkEdit2.Name = "repositoryItemHyperLinkEdit2";
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Detay";
            this.gridColumn7.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 5;
            this.gridColumn7.Width = 67;
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.Image = global::GPTS.Properties.Resources.Search_32x32;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            // 
            // ucSatisMasalar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ucSatisMasalar";
            this.Size = new System.Drawing.Size(1133, 817);
            this.Load += new System.EventHandler(this.ucSatisMasa_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueKullanicilar.Properties)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton19;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fişSilToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem masaDizaynKaydetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masalarıTemizleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masaYerleriSıfırlaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tümMasalarıSilToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masaTanımlarıToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xTabStokGruplari;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTab.XtraTabPage xTabBirimler;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        private DevExpress.XtraEditors.LookUpEdit lueKullanicilar;
        private DevExpress.XtraEditors.LabelControl lcKullanici;
    }
}

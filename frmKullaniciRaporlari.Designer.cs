namespace GPTS
{
    partial class frmKullaniciRaporlari
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
            this.gridControl5 = new DevExpress.XtraGrid.GridControl();
            this.gridView6 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemCheckEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.listBoxControl2 = new DevExpress.XtraEditors.ListBoxControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.listBoxControl1 = new DevExpress.XtraEditors.ListBoxControl();
            this.pkYetkiAlanlari = new DevExpress.XtraGrid.Columns.GridColumn();
            this.fkKullanicilar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ModulAdi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.YetkiKodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.YetkiAdi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.YetkiDurumu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Deger = new DevExpress.XtraGrid.Columns.GridColumn();
            this.KullaniciAdi = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl2)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl5
            // 
            this.gridControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl5.Location = new System.Drawing.Point(0, 245);
            this.gridControl5.MainView = this.gridView6;
            this.gridControl5.Name = "gridControl5";
            this.gridControl5.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit7,
            this.repositoryItemCheckEdit8});
            this.gridControl5.Size = new System.Drawing.Size(956, 251);
            this.gridControl5.TabIndex = 17;
            this.gridControl5.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView6});
            // 
            // gridView6
            // 
            this.gridView6.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.pkYetkiAlanlari,
            this.fkKullanicilar,
            this.KullaniciAdi,
            this.ModulAdi,
            this.YetkiKodu,
            this.YetkiAdi,
            this.YetkiDurumu,
            this.Deger});
            this.gridView6.GridControl = this.gridControl5;
            this.gridView6.GroupCount = 2;
            this.gridView6.Name = "gridView6";
            this.gridView6.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView6.OptionsSelection.MultiSelect = true;
            this.gridView6.OptionsView.ShowAutoFilterRow = true;
            this.gridView6.OptionsView.ShowFooter = true;
            this.gridView6.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.KullaniciAdi, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.ModulAdi, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // repositoryItemCheckEdit7
            // 
            this.repositoryItemCheckEdit7.AutoHeight = false;
            this.repositoryItemCheckEdit7.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.repositoryItemCheckEdit7.Name = "repositoryItemCheckEdit7";
            // 
            // repositoryItemCheckEdit8
            // 
            this.repositoryItemCheckEdit8.AutoHeight = false;
            this.repositoryItemCheckEdit8.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.repositoryItemCheckEdit8.Name = "repositoryItemCheckEdit8";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(956, 245);
            this.xtraTabControl1.TabIndex = 18;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.listBoxControl2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(950, 219);
            this.xtraTabPage1.Text = "Kullanıcılar";
            // 
            // listBoxControl2
            // 
            this.listBoxControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxControl2.Items.AddRange(new object[] {
            "GENEL",
            "YÖNETİCİ",
            "MÜDÜR",
            "SİSTEM YÖNETİCİSİ",
            "SATIŞ GÖREVLİSİ",
            "KASA GÖREVLİSİ",
            "İŞ ZEKASI RAPORU",
            "MUHASEBE",
            "SATIN ALMA SORUMLUSU",
            "SANTRAL ELEMANI",
            "SAYIM YETKİLİSİ",
            "RANDEVU ELEMANI",
            "STOK İŞLEMLERİ"});
            this.listBoxControl2.Location = new System.Drawing.Point(0, 0);
            this.listBoxControl2.Name = "listBoxControl2";
            this.listBoxControl2.Size = new System.Drawing.Size(262, 219);
            this.listBoxControl2.TabIndex = 1;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.listBoxControl1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(950, 219);
            this.xtraTabPage2.Text = "Roller";
            // 
            // listBoxControl1
            // 
            this.listBoxControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxControl1.Items.AddRange(new object[] {
            "GENEL",
            "YÖNETİCİ",
            "MÜDÜR",
            "SİSTEM YÖNETİCİSİ",
            "SATIŞ GÖREVLİSİ",
            "KASA GÖREVLİSİ",
            "İŞ ZEKASI RAPORU",
            "MUHASEBE",
            "SATIN ALMA SORUMLUSU",
            "SANTRAL ELEMANI",
            "SAYIM YETKİLİSİ",
            "RANDEVU ELEMANI",
            "STOK İŞLEMLERİ"});
            this.listBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.listBoxControl1.Name = "listBoxControl1";
            this.listBoxControl1.Size = new System.Drawing.Size(262, 219);
            this.listBoxControl1.TabIndex = 0;
            // 
            // pkYetkiAlanlari
            // 
            this.pkYetkiAlanlari.Caption = "pkYetkiAlanlari";
            this.pkYetkiAlanlari.FieldName = "pkYetkiAlanlari";
            this.pkYetkiAlanlari.Name = "pkYetkiAlanlari";
            // 
            // fkKullanicilar
            // 
            this.fkKullanicilar.Caption = "fkKullanicilar";
            this.fkKullanicilar.FieldName = "fkKullanicilar";
            this.fkKullanicilar.Name = "fkKullanicilar";
            // 
            // ModulAdi
            // 
            this.ModulAdi.Caption = "ModulAdi";
            this.ModulAdi.FieldName = "ModulAdi";
            this.ModulAdi.Name = "ModulAdi";
            this.ModulAdi.Visible = true;
            this.ModulAdi.VisibleIndex = 1;
            // 
            // YetkiKodu
            // 
            this.YetkiKodu.Caption = "YetkiKodu";
            this.YetkiKodu.FieldName = "YetkiKodu";
            this.YetkiKodu.Name = "YetkiKodu";
            this.YetkiKodu.Visible = true;
            this.YetkiKodu.VisibleIndex = 0;
            // 
            // YetkiAdi
            // 
            this.YetkiAdi.Caption = "YetkiAdi";
            this.YetkiAdi.FieldName = "YetkiAdi";
            this.YetkiAdi.Name = "YetkiAdi";
            this.YetkiAdi.Visible = true;
            this.YetkiAdi.VisibleIndex = 1;
            // 
            // YetkiDurumu
            // 
            this.YetkiDurumu.Caption = "YetkiDurumu";
            this.YetkiDurumu.FieldName = "YetkiDurumu";
            this.YetkiDurumu.Name = "YetkiDurumu";
            this.YetkiDurumu.Visible = true;
            this.YetkiDurumu.VisibleIndex = 2;
            // 
            // Deger
            // 
            this.Deger.Caption = "Deger";
            this.Deger.FieldName = "Deger";
            this.Deger.Name = "Deger";
            this.Deger.Visible = true;
            this.Deger.VisibleIndex = 3;
            // 
            // KullaniciAdi
            // 
            this.KullaniciAdi.Caption = "KullaniciAdi";
            this.KullaniciAdi.FieldName = "KullaniciAdi";
            this.KullaniciAdi.Name = "KullaniciAdi";
            this.KullaniciAdi.Visible = true;
            this.KullaniciAdi.VisibleIndex = 0;
            // 
            // frmKullaniciRaporlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 496);
            this.Controls.Add(this.gridControl5);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "frmKullaniciRaporlari";
            this.Text = "Kullanıcı Raporları";
            this.Load += new System.EventHandler(this.frmKullaniciRaporlari_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl2)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl5;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView6;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit7;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit8;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.ListBoxControl listBoxControl1;
        private DevExpress.XtraEditors.ListBoxControl listBoxControl2;
        private DevExpress.XtraGrid.Columns.GridColumn pkYetkiAlanlari;
        private DevExpress.XtraGrid.Columns.GridColumn fkKullanicilar;
        private DevExpress.XtraGrid.Columns.GridColumn ModulAdi;
        private DevExpress.XtraGrid.Columns.GridColumn YetkiKodu;
        private DevExpress.XtraGrid.Columns.GridColumn YetkiAdi;
        private DevExpress.XtraGrid.Columns.GridColumn YetkiDurumu;
        private DevExpress.XtraGrid.Columns.GridColumn Deger;
        private DevExpress.XtraGrid.Columns.GridColumn KullaniciAdi;
    }
}
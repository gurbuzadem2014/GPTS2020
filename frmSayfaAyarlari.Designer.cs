namespace GPTS
{
    partial class frmSayfaAyarlari
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
            this.gridControl5 = new DevExpress.XtraGrid.GridControl();
            this.gridView6 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn47 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn49 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn46 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gcKullanicilar = new DevExpress.XtraGrid.GridControl();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn42 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn43 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn44 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn45 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn48 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.gridColumn50 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit9 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kontrolEtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit8)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcKullanicilar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit5)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl5
            // 
            this.gridControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl5.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl5.Location = new System.Drawing.Point(0, 0);
            this.gridControl5.MainView = this.gridView6;
            this.gridControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl5.Name = "gridControl5";
            this.gridControl5.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit7,
            this.repositoryItemCheckEdit8});
            this.gridControl5.Size = new System.Drawing.Size(350, 356);
            this.gridControl5.TabIndex = 17;
            this.gridControl5.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView6});
            // 
            // gridView6
            // 
            this.gridView6.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn47,
            this.gridColumn49,
            this.gridColumn46});
            this.gridView6.GridControl = this.gridControl5;
            this.gridView6.Name = "gridView6";
            this.gridView6.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView6.OptionsSelection.MultiSelect = true;
            this.gridView6.OptionsView.ShowAutoFilterRow = true;
            this.gridView6.OptionsView.ShowFooter = true;
            this.gridView6.OptionsView.ShowGroupPanel = false;
            this.gridView6.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView6_FocusedRowChanged);
            // 
            // gridColumn47
            // 
            this.gridColumn47.Caption = "Yetki Alanları";
            this.gridColumn47.FieldName = "Aciklama50";
            this.gridColumn47.Name = "gridColumn47";
            this.gridColumn47.OptionsColumn.AllowEdit = false;
            this.gridColumn47.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn47.Visible = true;
            this.gridColumn47.VisibleIndex = 0;
            this.gridColumn47.Width = 165;
            // 
            // gridColumn49
            // 
            this.gridColumn49.Caption = "pkParametreler";
            this.gridColumn49.FieldName = "pkParametreler";
            this.gridColumn49.Name = "gridColumn49";
            // 
            // gridColumn46
            // 
            this.gridColumn46.Caption = "fkNesneTipi";
            this.gridColumn46.FieldName = "fkNesneTipi";
            this.gridColumn46.Name = "gridColumn46";
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridControl5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gcKullanicilar);
            this.splitContainer1.Size = new System.Drawing.Size(350, 599);
            this.splitContainer1.SplitterDistance = 356;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 18;
            // 
            // gcKullanicilar
            // 
            this.gcKullanicilar.ContextMenuStrip = this.contextMenuStrip1;
            this.gcKullanicilar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcKullanicilar.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcKullanicilar.Location = new System.Drawing.Point(0, 0);
            this.gcKullanicilar.MainView = this.gridView5;
            this.gcKullanicilar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcKullanicilar.Name = "gcKullanicilar";
            this.gcKullanicilar.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit5,
            this.repositoryItemCheckEdit6,
            this.repositoryItemSpinEdit6,
            this.repositoryItemCheckEdit9});
            this.gcKullanicilar.Size = new System.Drawing.Size(350, 238);
            this.gcKullanicilar.TabIndex = 16;
            this.gcKullanicilar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView5});
            // 
            // gridView5
            // 
            this.gridView5.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn42,
            this.gridColumn43,
            this.gridColumn44,
            this.gridColumn45,
            this.gridColumn48,
            this.gridColumn50});
            this.gridView5.GridControl = this.gcKullanicilar;
            this.gridView5.Name = "gridView5";
            this.gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView5.OptionsSelection.MultiSelect = true;
            this.gridView5.OptionsView.ShowFooter = true;
            this.gridView5.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn42
            // 
            this.gridColumn42.Caption = "pkKullanicilar";
            this.gridColumn42.FieldName = "pkKullanicilar";
            this.gridColumn42.Name = "gridColumn42";
            // 
            // gridColumn43
            // 
            this.gridColumn43.Caption = "Kullanıcı Adı";
            this.gridColumn43.FieldName = "KullaniciAdi";
            this.gridColumn43.Name = "gridColumn43";
            this.gridColumn43.OptionsColumn.AllowEdit = false;
            this.gridColumn43.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn43.Visible = true;
            this.gridColumn43.VisibleIndex = 0;
            this.gridColumn43.Width = 112;
            // 
            // gridColumn44
            // 
            this.gridColumn44.Caption = "Durumu";
            this.gridColumn44.ColumnEdit = this.repositoryItemCheckEdit6;
            this.gridColumn44.FieldName = "Sec";
            this.gridColumn44.Name = "gridColumn44";
            this.gridColumn44.Visible = true;
            this.gridColumn44.VisibleIndex = 1;
            this.gridColumn44.Width = 51;
            // 
            // repositoryItemCheckEdit6
            // 
            this.repositoryItemCheckEdit6.AutoHeight = false;
            this.repositoryItemCheckEdit6.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.repositoryItemCheckEdit6.Name = "repositoryItemCheckEdit6";
            this.repositoryItemCheckEdit6.CheckedChanged += new System.EventHandler(this.repositoryItemCheckEdit6_CheckedChanged);
            // 
            // gridColumn45
            // 
            this.gridColumn45.Caption = "pkYetkiAlanlari";
            this.gridColumn45.FieldName = "pkYetkiAlanlari";
            this.gridColumn45.Name = "gridColumn45";
            // 
            // gridColumn48
            // 
            this.gridColumn48.Caption = "Yazdirma Adedi";
            this.gridColumn48.ColumnEdit = this.repositoryItemSpinEdit6;
            this.gridColumn48.FieldName = "Sayi";
            this.gridColumn48.Name = "gridColumn48";
            // 
            // repositoryItemSpinEdit6
            // 
            this.repositoryItemSpinEdit6.AccessibleDescription = "parametrelersayi";
            this.repositoryItemSpinEdit6.AutoHeight = false;
            this.repositoryItemSpinEdit6.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit6.Name = "repositoryItemSpinEdit6";
            this.repositoryItemSpinEdit6.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repositoryItemSpinEdit6_KeyDown);
            // 
            // gridColumn50
            // 
            this.gridColumn50.Caption = "Balon Göster";
            this.gridColumn50.ColumnEdit = this.repositoryItemCheckEdit9;
            this.gridColumn50.FieldName = "BalonGoster";
            this.gridColumn50.Name = "gridColumn50";
            this.gridColumn50.Width = 77;
            // 
            // repositoryItemCheckEdit9
            // 
            this.repositoryItemCheckEdit9.AutoHeight = false;
            this.repositoryItemCheckEdit9.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.repositoryItemCheckEdit9.Name = "repositoryItemCheckEdit9";
            // 
            // repositoryItemCheckEdit5
            // 
            this.repositoryItemCheckEdit5.AutoHeight = false;
            this.repositoryItemCheckEdit5.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.repositoryItemCheckEdit5.Name = "repositoryItemCheckEdit5";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kontrolEtToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 58);
            // 
            // kontrolEtToolStripMenuItem
            // 
            this.kontrolEtToolStripMenuItem.Name = "kontrolEtToolStripMenuItem";
            this.kontrolEtToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.kontrolEtToolStripMenuItem.Text = "Kontrol Et";
            this.kontrolEtToolStripMenuItem.Click += new System.EventHandler(this.kontrolEtToolStripMenuItem_Click);
            // 
            // frmSayfaAyarlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 599);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSayfaAyarlari";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sayfa Ayarları";
            this.Load += new System.EventHandler(this.frmSayfaAyarlari_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit8)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcKullanicilar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit5)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl5;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn47;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn49;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn46;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit7;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit8;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraGrid.GridControl gcKullanicilar;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn42;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn43;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn44;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn45;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn48;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn50;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit9;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem kontrolEtToolStripMenuItem;
    }
}
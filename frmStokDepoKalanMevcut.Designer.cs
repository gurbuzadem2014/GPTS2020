namespace GPTS
{
    partial class frmStokDepoKalanMevcut
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.stokHareketleriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stokKartıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devirBakiyeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.düzenleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.farklıKaydetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pivotGridControl2 = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.pivotGridField7 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField8 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField9 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField11 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField12 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField13 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.gridControl3 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1322, 53);
            this.panelControl1.TabIndex = 147;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.ExportToExcel_16x16;
            this.simpleButton2.Location = new System.Drawing.Point(155, 2);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(153, 49);
            this.simpleButton2.TabIndex = 91;
            this.simpleButton2.Text = "Excel Gönder Pivod";
            this.simpleButton2.Visible = false;
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton1.Image = global::GPTS.Properties.Resources.ExportToExcel_16x16;
            this.simpleButton1.Location = new System.Drawing.Point(2, 2);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(153, 49);
            this.simpleButton1.TabIndex = 90;
            this.simpleButton1.Text = "Excel Gönder";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(1174, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(146, 49);
            this.simpleButton21.TabIndex = 89;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stokHareketleriToolStripMenuItem,
            this.stokKartıToolStripMenuItem,
            this.devirBakiyeToolStripMenuItem,
            this.düzenleToolStripMenuItem,
            this.farklıKaydetToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 152);
            // 
            // stokHareketleriToolStripMenuItem
            // 
            this.stokHareketleriToolStripMenuItem.Name = "stokHareketleriToolStripMenuItem";
            this.stokHareketleriToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.stokHareketleriToolStripMenuItem.Text = "Stok Hareketleri";
            this.stokHareketleriToolStripMenuItem.Click += new System.EventHandler(this.stokHareketleriToolStripMenuItem_Click);
            // 
            // stokKartıToolStripMenuItem
            // 
            this.stokKartıToolStripMenuItem.Name = "stokKartıToolStripMenuItem";
            this.stokKartıToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.stokKartıToolStripMenuItem.Text = "Stok Kartı";
            this.stokKartıToolStripMenuItem.Click += new System.EventHandler(this.stokKartıToolStripMenuItem_Click);
            // 
            // devirBakiyeToolStripMenuItem
            // 
            this.devirBakiyeToolStripMenuItem.Name = "devirBakiyeToolStripMenuItem";
            this.devirBakiyeToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.devirBakiyeToolStripMenuItem.Text = "Devir Bakiye";
            this.devirBakiyeToolStripMenuItem.Click += new System.EventHandler(this.devirBakiyeToolStripMenuItem_Click);
            // 
            // düzenleToolStripMenuItem
            // 
            this.düzenleToolStripMenuItem.Name = "düzenleToolStripMenuItem";
            this.düzenleToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.düzenleToolStripMenuItem.Text = "Düzenle";
            this.düzenleToolStripMenuItem.Click += new System.EventHandler(this.düzenleToolStripMenuItem_Click);
            // 
            // farklıKaydetToolStripMenuItem
            // 
            this.farklıKaydetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.excelToolStripMenuItem});
            this.farklıKaydetToolStripMenuItem.Name = "farklıKaydetToolStripMenuItem";
            this.farklıKaydetToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.farklıKaydetToolStripMenuItem.Text = "Farklı Kaydet";
            // 
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(118, 26);
            this.excelToolStripMenuItem.Text = "Excel";
            this.excelToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // pivotGridControl2
            // 
            this.pivotGridControl2.ContextMenuStrip = this.contextMenuStrip1;
            this.pivotGridControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pivotGridControl2.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.pivotGridField7,
            this.pivotGridField8,
            this.pivotGridField9,
            this.pivotGridField11,
            this.pivotGridField12,
            this.pivotGridField13});
            this.pivotGridControl2.Location = new System.Drawing.Point(0, 382);
            this.pivotGridControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pivotGridControl2.Name = "pivotGridControl2";
            this.pivotGridControl2.OptionsView.ShowColumnGrandTotalHeader = false;
            this.pivotGridControl2.OptionsView.ShowColumnGrandTotals = false;
            this.pivotGridControl2.OptionsView.ShowColumnTotals = false;
            this.pivotGridControl2.OptionsView.ShowRowGrandTotalHeader = false;
            this.pivotGridControl2.OptionsView.ShowRowGrandTotals = false;
            this.pivotGridControl2.OptionsView.ShowRowTotals = false;
            this.pivotGridControl2.Size = new System.Drawing.Size(1322, 272);
            this.pivotGridControl2.TabIndex = 150;
            this.pivotGridControl2.Visible = false;
            // 
            // pivotGridField7
            // 
            this.pivotGridField7.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField7.AreaIndex = 0;
            this.pivotGridField7.FieldName = "Stokadi";
            this.pivotGridField7.Name = "pivotGridField7";
            this.pivotGridField7.Width = 250;
            // 
            // pivotGridField8
            // 
            this.pivotGridField8.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField8.AreaIndex = 1;
            this.pivotGridField8.Caption = "Barkod";
            this.pivotGridField8.FieldName = "Barcode";
            this.pivotGridField8.Name = "pivotGridField8";
            this.pivotGridField8.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Descending;
            this.pivotGridField8.Width = 120;
            // 
            // pivotGridField9
            // 
            this.pivotGridField9.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField9.AreaIndex = 3;
            this.pivotGridField9.Caption = "Şube Adı";
            this.pivotGridField9.FieldName = "sube_adi";
            this.pivotGridField9.Name = "pivotGridField9";
            this.pivotGridField9.Width = 150;
            // 
            // pivotGridField11
            // 
            this.pivotGridField11.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField11.AreaIndex = 0;
            this.pivotGridField11.Caption = "Depo Mevcut";
            this.pivotGridField11.FieldName = "DepoMevcut";
            this.pivotGridField11.Name = "pivotGridField11";
            this.pivotGridField11.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Average;
            this.pivotGridField11.Width = 150;
            // 
            // pivotGridField12
            // 
            this.pivotGridField12.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField12.AreaIndex = 0;
            this.pivotGridField12.Caption = "Depo Adı";
            this.pivotGridField12.FieldName = "DepoAdi";
            this.pivotGridField12.Name = "pivotGridField12";
            this.pivotGridField12.Width = 150;
            // 
            // pivotGridField13
            // 
            this.pivotGridField13.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField13.AreaIndex = 2;
            this.pivotGridField13.Caption = "Toplam Mevcut";
            this.pivotGridField13.FieldName = "ToplamMevcut";
            this.pivotGridField13.Name = "pivotGridField13";
            this.pivotGridField13.Width = 150;
            // 
            // gridControl3
            // 
            this.gridControl3.ContextMenuStrip = this.contextMenuStrip2;
            this.gridControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl3.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl3.Location = new System.Drawing.Point(0, 53);
            this.gridControl3.MainView = this.gridView3;
            this.gridControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl3.Name = "gridControl3";
            this.gridControl3.Size = new System.Drawing.Size(1322, 329);
            this.gridControl3.TabIndex = 151;
            this.gridControl3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.toolStripMenuItem3});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(185, 76);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 24);
            this.toolStripMenuItem1.Text = "Stok Hareketleri";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 24);
            this.toolStripMenuItem2.Text = "Stok Kartı";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(184, 24);
            this.toolStripMenuItem3.Text = "Devir Bakiye";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // gridView3
            // 
            this.gridView3.ColumnPanelRowHeight = 40;
            this.gridView3.CustomizationFormBounds = new System.Drawing.Rectangle(808, 285, 216, 318);
            this.gridView3.GridControl = this.gridControl3;
            this.gridView3.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Adet", null, "Satılan Adet {0}")});
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsBehavior.Editable = false;
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowAutoFilterRow = true;
            this.gridView3.OptionsView.ShowFooter = true;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.ViewCaption = "Satış Listesi";
            // 
            // frmStokDepoKalanMevcut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1322, 654);
            this.Controls.Add(this.gridControl3);
            this.Controls.Add(this.pivotGridControl2);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmStokDepoKalanMevcut";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Depo Stok Durumu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmStokKartiDepo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem stokKartıToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem devirBakiyeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stokHareketleriToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem düzenleToolStripMenuItem;
        private DevExpress.XtraPivotGrid.PivotGridControl pivotGridControl2;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField7;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField8;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField9;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField11;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField12;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField13;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private System.Windows.Forms.ToolStripMenuItem farklıKaydetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private DevExpress.XtraGrid.GridControl gridControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}
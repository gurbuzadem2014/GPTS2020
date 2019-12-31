namespace GPTS
{
    partial class frmSayimDetay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSayimDetay));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.stokKartıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depoMevcutlarıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stokHareketleriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depoMevcutlarıGüncelleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCalcEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbSayilanlar = new System.Windows.Forms.CheckBox();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 49);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCalcEdit1,
            this.repositoryItemButtonEdit1});
            this.gridControl1.Size = new System.Drawing.Size(858, 387);
            this.gridControl1.TabIndex = 3;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stokKartıToolStripMenuItem,
            this.depoMevcutlarıToolStripMenuItem,
            this.stokHareketleriToolStripMenuItem,
            this.depoMevcutlarıGüncelleToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 92);
            // 
            // stokKartıToolStripMenuItem
            // 
            this.stokKartıToolStripMenuItem.Name = "stokKartıToolStripMenuItem";
            this.stokKartıToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.stokKartıToolStripMenuItem.Text = "Stok Kartı";
            this.stokKartıToolStripMenuItem.Click += new System.EventHandler(this.stokKartıToolStripMenuItem_Click);
            // 
            // depoMevcutlarıToolStripMenuItem
            // 
            this.depoMevcutlarıToolStripMenuItem.Name = "depoMevcutlarıToolStripMenuItem";
            this.depoMevcutlarıToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.depoMevcutlarıToolStripMenuItem.Text = "Depo Mevcutları";
            this.depoMevcutlarıToolStripMenuItem.Click += new System.EventHandler(this.depoMevcutlarıToolStripMenuItem_Click);
            // 
            // stokHareketleriToolStripMenuItem
            // 
            this.stokHareketleriToolStripMenuItem.Name = "stokHareketleriToolStripMenuItem";
            this.stokHareketleriToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.stokHareketleriToolStripMenuItem.Text = "Stok Hareketleri";
            this.stokHareketleriToolStripMenuItem.Click += new System.EventHandler(this.stokHareketleriToolStripMenuItem_Click);
            // 
            // depoMevcutlarıGüncelleToolStripMenuItem
            // 
            this.depoMevcutlarıGüncelleToolStripMenuItem.Name = "depoMevcutlarıGüncelleToolStripMenuItem";
            this.depoMevcutlarıGüncelleToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.depoMevcutlarıGüncelleToolStripMenuItem.Text = "Depo Mevcutları Güncelle";
            this.depoMevcutlarıGüncelleToolStripMenuItem.Click += new System.EventHandler(this.depoMevcutlarıGüncelleToolStripMenuItem_Click);
            // 
            // gridView1
            // 
            this.gridView1.AppearancePrint.HeaderPanel.BackColor = System.Drawing.Color.Blue;
            this.gridView1.AppearancePrint.HeaderPanel.BackColor2 = System.Drawing.Color.Blue;
            this.gridView1.AppearancePrint.HeaderPanel.BorderColor = System.Drawing.Color.Red;
            this.gridView1.AppearancePrint.HeaderPanel.Options.UseBackColor = true;
            this.gridView1.AppearancePrint.HeaderPanel.Options.UseBorderColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn7,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn18,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn13});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsSelection.InvertSelection = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.ViewCaption = "SAYILAN STOK LİSTESİ";
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "pkStokSayim";
            this.gridColumn11.FieldName = "pkStokSayim";
            this.gridColumn11.Name = "gridColumn11";
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "fkDepolar";
            this.gridColumn12.FieldName = "fkDepolar";
            this.gridColumn12.Name = "gridColumn12";
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.gridColumn7.AppearanceCell.Options.UseFont = true;
            this.gridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.gridColumn7.AppearanceHeader.Options.UseFont = true;
            this.gridColumn7.Caption = "pkStokSayimDetay";
            this.gridColumn7.FieldName = "pkStokSayimDetay";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "BARKOD";
            this.gridColumn1.FieldName = "Barcode";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Barcode", "Stok Çeşidi={0}")});
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 119;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "STOK ADI";
            this.gridColumn2.FieldName = "Stokadi";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 215;
            // 
            // gridColumn18
            // 
            this.gridColumn18.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.gridColumn18.AppearanceCell.Options.UseFont = true;
            this.gridColumn18.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.gridColumn18.AppearanceHeader.Options.UseFont = true;
            this.gridColumn18.Caption = "pkStokKarti";
            this.gridColumn18.FieldName = "pkStokKarti";
            this.gridColumn18.Name = "gridColumn18";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "GRUP";
            this.gridColumn5.FieldName = "StokGrup";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 163;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "ALT GRUP";
            this.gridColumn6.FieldName = "StokAltGrup";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Width = 118;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "MEVCUT MİKTAR";
            this.gridColumn8.FieldName = "MevcutMiktar";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 5;
            this.gridColumn8.Width = 95;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridColumn9.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn9.Caption = "STOK SAYIM MİKTARI";
            this.gridColumn9.ColumnEdit = this.repositoryItemCalcEdit1;
            this.gridColumn9.FieldName = "SayimSonuMiktari";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 4;
            this.gridColumn9.Width = 126;
            // 
            // repositoryItemCalcEdit1
            // 
            this.repositoryItemCalcEdit1.AccessibleDescription = "sayim miktari";
            this.repositoryItemCalcEdit1.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.repositoryItemCalcEdit1.AutoHeight = false;
            this.repositoryItemCalcEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEdit1.Name = "repositoryItemCalcEdit1";
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "FARK";
            this.gridColumn10.FieldName = "FARK";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.UnboundExpression = "[SayimSonuMiktari]-[MevcutMiktar]";
            this.gridColumn10.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 6;
            this.gridColumn10.Width = 74;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Tutar";
            this.gridColumn3.DisplayFormat.FormatString = "{0:c}";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn3.FieldName = "Tutar";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Tutar", "{0:c}")});
            this.gridColumn3.UnboundExpression = "[FARK]*[AlisFiyati]";
            this.gridColumn3.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn3.Width = 61;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Alış Fiyatı";
            this.gridColumn4.FieldName = "AlisFiyati";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Width = 46;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "Mevcut";
            this.gridColumn13.FieldName = "Mevcut";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 2;
            this.gridColumn13.Width = 49;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AccessibleDescription = "Stok Ara...";
            this.repositoryItemButtonEdit1.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.repositoryItemButtonEdit1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.repositoryItemButtonEdit1.Appearance.Options.UseBackColor = true;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cbSayilanlar);
            this.panelControl1.Controls.Add(this.simpleButton8);
            this.panelControl1.Controls.Add(this.simpleButton6);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(858, 49);
            this.panelControl1.TabIndex = 4;
            // 
            // cbSayilanlar
            // 
            this.cbSayilanlar.AutoSize = true;
            this.cbSayilanlar.Checked = true;
            this.cbSayilanlar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSayilanlar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbSayilanlar.Location = new System.Drawing.Point(130, 25);
            this.cbSayilanlar.Name = "cbSayilanlar";
            this.cbSayilanlar.Size = new System.Drawing.Size(139, 17);
            this.cbSayilanlar.TabIndex = 103;
            this.cbSayilanlar.Text = "Sayılan Tüm Stok Listesi";
            this.cbSayilanlar.UseVisualStyleBackColor = true;
            this.cbSayilanlar.Visible = false;
            this.cbSayilanlar.CheckedChanged += new System.EventHandler(this.cbSayilanlar_CheckedChanged);
            // 
            // simpleButton8
            // 
            this.simpleButton8.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton8.Image = global::GPTS.Properties.Resources.Printer;
            this.simpleButton8.Location = new System.Drawing.Point(639, 2);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(106, 45);
            this.simpleButton8.TabIndex = 89;
            this.simpleButton8.Text = "Yazdır [F11]";
            this.simpleButton8.Click += new System.EventHandler(this.simpleButton8_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton6.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton6.Image")));
            this.simpleButton6.Location = new System.Drawing.Point(2, 2);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(113, 45);
            this.simpleButton6.TabIndex = 84;
            this.simpleButton6.Text = "Stok Kartını Sil \r\n[F5]";
            this.simpleButton6.Visible = false;
            // 
            // simpleButton21
            // 
            this.simpleButton21.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(745, 2);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(111, 45);
            this.simpleButton21.TabIndex = 19;
            this.simpleButton21.Text = "Kapat\r\n[ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // frmSayimDetay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 436);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmSayimDetay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "0";
            this.Text = "Sayım Detay";
            this.Load += new System.EventHandler(this.frmSayimDetay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
        private System.Windows.Forms.CheckBox cbSayilanlar;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem depoMevcutlarıToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stokHareketleriToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stokKartıToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depoMevcutlarıGüncelleToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
    }
}
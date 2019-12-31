namespace GPTS
{
    partial class frmKodver
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tkod = new DevExpress.XtraEditors.TextEdit();
            this.sRakam = new DevExpress.XtraEditors.SpinEdit();
            this.pkKodver = new System.Windows.Forms.TextBox();
            this.sbtnSil = new DevExpress.XtraEditors.SimpleButton();
            this.kodal = new System.Windows.Forms.TextBox();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aktifSeçToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcKodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sRakam.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tkod);
            this.panelControl1.Controls.Add(this.sRakam);
            this.panelControl1.Controls.Add(this.pkKodver);
            this.panelControl1.Controls.Add(this.sbtnSil);
            this.panelControl1.Controls.Add(this.kodal);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(964, 68);
            this.panelControl1.TabIndex = 1;
            // 
            // tkod
            // 
            this.tkod.EditValue = "";
            this.tkod.Location = new System.Drawing.Point(379, 6);
            this.tkod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tkod.Name = "tkod";
            this.tkod.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.tkod.Properties.Appearance.Options.UseFont = true;
            this.tkod.Size = new System.Drawing.Size(57, 23);
            toolTipItem1.Text = "Stoklarınızı girmeden önce lütfen kendinize bir sıralama belirleyiniz. Örn:ÜLKER " +
    "KREMA BİSKÜVİ 30GR";
            superToolTip1.Items.Add(toolTipItem1);
            this.tkod.SuperTip = superToolTip1;
            this.tkod.TabIndex = 166;
            this.tkod.Visible = false;
            // 
            // sRakam
            // 
            this.sRakam.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sRakam.Location = new System.Drawing.Point(443, 34);
            this.sRakam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sRakam.Name = "sRakam";
            this.sRakam.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.sRakam.Properties.Appearance.Options.UseFont = true;
            this.sRakam.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sRakam.Size = new System.Drawing.Size(69, 23);
            this.sRakam.TabIndex = 165;
            this.sRakam.Visible = false;
            // 
            // pkKodver
            // 
            this.pkKodver.Location = new System.Drawing.Point(444, 6);
            this.pkKodver.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkKodver.Name = "pkKodver";
            this.pkKodver.Size = new System.Drawing.Size(67, 23);
            this.pkKodver.TabIndex = 164;
            this.pkKodver.Visible = false;
            // 
            // sbtnSil
            // 
            this.sbtnSil.Dock = System.Windows.Forms.DockStyle.Left;
            this.sbtnSil.Enabled = false;
            this.sbtnSil.Image = global::GPTS.Properties.Resources.DeleteRed5;
            this.sbtnSil.Location = new System.Drawing.Point(230, 2);
            this.sbtnSil.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sbtnSil.Name = "sbtnSil";
            this.sbtnSil.Size = new System.Drawing.Size(117, 64);
            this.sbtnSil.TabIndex = 84;
            this.sbtnSil.Text = "Sil [F5]";
            this.sbtnSil.Click += new System.EventHandler(this.sbtnSil_Click);
            // 
            // kodal
            // 
            this.kodal.Location = new System.Drawing.Point(379, 34);
            this.kodal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.kodal.Name = "kodal";
            this.kodal.Size = new System.Drawing.Size(56, 23);
            this.kodal.TabIndex = 90;
            this.kodal.Visible = false;
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(821, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(141, 64);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnKaydet.Enabled = false;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.duzenle_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(120, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(110, 64);
            this.BtnKaydet.TabIndex = 25;
            this.BtnKaydet.Text = "Düzenle";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = global::GPTS.Properties.Resources.new_window;
            this.simpleButton4.Location = new System.Drawing.Point(2, 2);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(118, 64);
            this.simpleButton4.TabIndex = 85;
            this.simpleButton4.Text = "Yeni [F7]";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // gridControl2
            // 
            this.gridControl2.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl2.Location = new System.Drawing.Point(0, 68);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHyperLinkEdit1,
            this.repositoryItemCheckEdit1});
            this.gridControl2.Size = new System.Drawing.Size(964, 474);
            this.gridControl2.TabIndex = 52;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aktifSeçToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 28);
            // 
            // aktifSeçToolStripMenuItem
            // 
            this.aktifSeçToolStripMenuItem.Name = "aktifSeçToolStripMenuItem";
            this.aktifSeçToolStripMenuItem.Size = new System.Drawing.Size(172, 24);
            this.aktifSeçToolStripMenuItem.Text = "Varsayılan Yap";
            this.aktifSeçToolStripMenuItem.Click += new System.EventHandler(this.aktifSeçToolStripMenuItem_Click);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gcKodu,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn2,
            this.gridColumn7,
            this.gridColumn8});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowAutoFilterRow = true;
            this.gridView2.OptionsView.ShowFooter = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.RowHeight = 30;
            this.gridView2.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridView2.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView2_RowClick);
            this.gridView2.DoubleClick += new System.EventHandler(this.gridView2_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "pkKodver";
            this.gridColumn1.FieldName = "pkKodver";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gcKodu
            // 
            this.gcKodu.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gcKodu.AppearanceHeader.Options.UseFont = true;
            this.gcKodu.AppearanceHeader.Options.UseTextOptions = true;
            this.gcKodu.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcKodu.Caption = "Kısaltma";
            this.gcKodu.FieldName = "Kodu";
            this.gcKodu.Name = "gcKodu";
            this.gcKodu.Visible = true;
            this.gcKodu.VisibleIndex = 2;
            this.gcKodu.Width = 162;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "EAN 13-Stok Kodu";
            this.gridColumn3.FieldName = "Rakam";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 136;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Açıklama";
            this.gridColumn4.FieldName = "Aciklama";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 202;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn5.AppearanceCell.Image = global::GPTS.Properties.Resources.Favorite1;
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.AppearanceCell.Options.UseImage = true;
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = " ";
            this.gridColumn5.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.gridColumn5.FieldName = "Kodversec";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 7;
            this.gridColumn5.Width = 106;
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemHyperLinkEdit1.Caption = "Kod Ver";
            this.repositoryItemHyperLinkEdit1.Image = global::GPTS.Properties.Resources.button_ok;
            this.repositoryItemHyperLinkEdit1.ImageAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.repositoryItemHyperLinkEdit1.LinkColor = System.Drawing.Color.Black;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            this.repositoryItemHyperLinkEdit1.EditValueChanged += new System.EventHandler(this.repositoryItemHyperLinkEdit1_EditValueChanged);
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "Varsayilan";
            this.gridColumn6.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn6.FieldName = "AktifSec";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 80;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.repositoryItemCheckEdit1.Appearance.Options.UseFont = true;
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Üretici Barkodu";
            this.gridColumn2.FieldName = "uretici_barkodu";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 5;
            this.gridColumn2.Width = 94;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Ülke Kodu";
            this.gridColumn7.FieldName = "ulke_kodu";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Stok Barkodu";
            this.gridColumn8.FieldName = "stok_barkodu";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 1;
            this.gridColumn8.Width = 91;
            // 
            // frmKodver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 542);
            this.Controls.Add(this.gridControl2);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmKodver";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stok Kodu Ver";
            this.Load += new System.EventHandler(this.frmKodver_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmKodver_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sRakam.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton sbtnSil;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        public System.Windows.Forms.TextBox kodal;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        public System.Windows.Forms.TextBox pkKodver;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aktifSeçToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        public DevExpress.XtraEditors.SpinEdit sRakam;
        public DevExpress.XtraEditors.TextEdit tkod;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        public DevExpress.XtraGrid.Columns.GridColumn gcKodu;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
    }
}
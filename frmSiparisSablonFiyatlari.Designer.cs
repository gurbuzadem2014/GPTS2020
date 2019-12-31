namespace GPTS
{
    partial class frmSiparisSablonFiyatlari
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
            this.baslik = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pkFirma = new System.Windows.Forms.TextBox();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.sbtnSil = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.gcFiyatlar = new DevExpress.XtraGrid.GridControl();
            this.gridView6 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn27 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn32 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcFiyatlar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).BeginInit();
            this.SuspendLayout();
            // 
            // baslik
            // 
            this.baslik.BackColor = System.Drawing.Color.SkyBlue;
            this.baslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.baslik.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baslik.ForeColor = System.Drawing.Color.Black;
            this.baslik.Location = new System.Drawing.Point(0, 55);
            this.baslik.Name = "baslik";
            this.baslik.Size = new System.Drawing.Size(549, 30);
            this.baslik.TabIndex = 1;
            this.baslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.pkFirma);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.sbtnSil);
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(549, 55);
            this.panelControl1.TabIndex = 3;
            // 
            // pkFirma
            // 
            this.pkFirma.Location = new System.Drawing.Point(278, 5);
            this.pkFirma.Name = "pkFirma";
            this.pkFirma.Size = new System.Drawing.Size(58, 21);
            this.pkFirma.TabIndex = 164;
            this.pkFirma.Visible = false;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(338, 2);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(103, 51);
            this.BtnKaydet.TabIndex = 25;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Visible = false;
            // 
            // sbtnSil
            // 
            this.sbtnSil.Dock = System.Windows.Forms.DockStyle.Left;
            this.sbtnSil.Image = global::GPTS.Properties.Resources.DeleteRed5;
            this.sbtnSil.Location = new System.Drawing.Point(154, 2);
            this.sbtnSil.Name = "sbtnSil";
            this.sbtnSil.Size = new System.Drawing.Size(118, 51);
            this.sbtnSil.TabIndex = 84;
            this.sbtnSil.Text = "Fiyat Grubu Sil\r\n[F5]";
            this.sbtnSil.Visible = false;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = global::GPTS.Properties.Resources.new_window;
            this.simpleButton4.Location = new System.Drawing.Point(2, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(152, 51);
            this.simpleButton4.TabIndex = 85;
            this.simpleButton4.Text = "Yeni Fiyat Grubu\r\n[F7]";
            this.simpleButton4.Visible = false;
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton21.Location = new System.Drawing.Point(441, 2);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(106, 51);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // gcFiyatlar
            // 
            this.gcFiyatlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcFiyatlar.Location = new System.Drawing.Point(0, 85);
            this.gcFiyatlar.MainView = this.gridView6;
            this.gcFiyatlar.Name = "gcFiyatlar";
            this.gcFiyatlar.Size = new System.Drawing.Size(549, 242);
            this.gcFiyatlar.TabIndex = 114;
            this.gcFiyatlar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView6});
            this.gcFiyatlar.Click += new System.EventHandler(this.gcFiyatlar_Click);
            // 
            // gridView6
            // 
            this.gridView6.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn27,
            this.gridColumn28,
            this.gridColumn32,
            this.gridColumn1});
            this.gridView6.GridControl = this.gcFiyatlar;
            this.gridView6.Name = "gridView6";
            this.gridView6.OptionsBehavior.Editable = false;
            this.gridView6.OptionsView.ShowFooter = true;
            this.gridView6.OptionsView.ShowGroupPanel = false;
            this.gridView6.OptionsView.ShowIndicator = false;
            this.gridView6.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn28, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gridView6.ViewCaption = "Satış Fiyatları";
            // 
            // gridColumn27
            // 
            this.gridColumn27.Caption = "Stok Adı";
            this.gridColumn27.FieldName = "Stokadi";
            this.gridColumn27.Name = "gridColumn27";
            this.gridColumn27.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn27.Visible = true;
            this.gridColumn27.VisibleIndex = 0;
            this.gridColumn27.Width = 126;
            // 
            // gridColumn28
            // 
            this.gridColumn28.Caption = "Satış Fiyatı";
            this.gridColumn28.DisplayFormat.FormatString = "{0:#0.00####}";
            this.gridColumn28.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn28.FieldName = "SatisFiyatiKdvli";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.Visible = true;
            this.gridColumn28.VisibleIndex = 1;
            this.gridColumn28.Width = 124;
            // 
            // gridColumn32
            // 
            this.gridColumn32.Caption = "Fiyat Grubu";
            this.gridColumn32.FieldName = "FiyatAdi";
            this.gridColumn32.Name = "gridColumn32";
            this.gridColumn32.Visible = true;
            this.gridColumn32.VisibleIndex = 2;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "pkSatisFiyatlari";
            this.gridColumn1.FieldName = "pkSatisFiyatlari";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 20;
            // 
            // frmSiparisSablonFiyatlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 327);
            this.Controls.Add(this.gcFiyatlar);
            this.Controls.Add(this.baslik);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmSiparisSablonFiyatlari";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Müşteri Stok Fiyatları";
            this.Load += new System.EventHandler(this.frmSiparisSablonFiyatlari_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcFiyatlar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label baslik;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        public System.Windows.Forms.TextBox pkFirma;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton sbtnSil;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraGrid.GridControl gcFiyatlar;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn27;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn32;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}
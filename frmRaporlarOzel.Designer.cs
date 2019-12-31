namespace GPTS
{
    partial class frmRaporlarOzel
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage10 = new DevExpress.XtraTab.XtraTabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cgOzelRaporlar = new DevExpress.XtraGrid.GridControl();
            this.gridView14 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.simpleButton34 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton33 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton32 = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCalistir = new DevExpress.XtraEditors.SimpleButton();
            this.rapor_adi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.meSql = new DevExpress.XtraEditors.MemoEdit();
            this.gcSql = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cgOzelRaporlar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView14)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rapor_adi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.meSql.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcSql)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage10;
            this.xtraTabControl1.Size = new System.Drawing.Size(937, 560);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage10});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPage10
            // 
            this.xtraTabPage10.Controls.Add(this.splitContainer1);
            this.xtraTabPage10.Name = "xtraTabPage10";
            this.xtraTabPage10.Size = new System.Drawing.Size(931, 534);
            this.xtraTabPage10.Text = "Özel Raporlar";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cgOzelRaporlar);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gcSql);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Size = new System.Drawing.Size(931, 534);
            this.splitContainer1.SplitterDistance = 309;
            this.splitContainer1.TabIndex = 0;
            // 
            // cgOzelRaporlar
            // 
            this.cgOzelRaporlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cgOzelRaporlar.Location = new System.Drawing.Point(0, 36);
            this.cgOzelRaporlar.MainView = this.gridView14;
            this.cgOzelRaporlar.Name = "cgOzelRaporlar";
            this.cgOzelRaporlar.Size = new System.Drawing.Size(309, 320);
            this.cgOzelRaporlar.TabIndex = 1;
            this.cgOzelRaporlar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView14});
            // 
            // gridView14
            // 
            this.gridView14.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13});
            this.gridView14.GridControl = this.cgOzelRaporlar;
            this.gridView14.Name = "gridView14";
            this.gridView14.OptionsBehavior.Editable = false;
            this.gridView14.OptionsView.ShowGroupPanel = false;
            this.gridView14.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView14_RowClick);
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Rapor Adı";
            this.gridColumn11.FieldName = "rapor_adi";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 0;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Özel Rapor Id";
            this.gridColumn12.FieldName = "rapor_id";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 1;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "rapor_sql";
            this.gridColumn13.FieldName = "rapor_sql";
            this.gridColumn13.Name = "gridColumn13";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.simpleButton34);
            this.panel1.Controls.Add(this.simpleButton33);
            this.panel1.Controls.Add(this.simpleButton32);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(309, 36);
            this.panel1.TabIndex = 0;
            // 
            // simpleButton34
            // 
            this.simpleButton34.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton34.Location = new System.Drawing.Point(202, 0);
            this.simpleButton34.Name = "simpleButton34";
            this.simpleButton34.Size = new System.Drawing.Size(101, 36);
            this.simpleButton34.TabIndex = 2;
            this.simpleButton34.Text = "Sil";
            this.simpleButton34.Click += new System.EventHandler(this.simpleButton34_Click);
            // 
            // simpleButton33
            // 
            this.simpleButton33.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton33.Location = new System.Drawing.Point(101, 0);
            this.simpleButton33.Name = "simpleButton33";
            this.simpleButton33.Size = new System.Drawing.Size(101, 36);
            this.simpleButton33.TabIndex = 1;
            this.simpleButton33.Text = "Kaydet";
            this.simpleButton33.Click += new System.EventHandler(this.simpleButton33_Click);
            // 
            // simpleButton32
            // 
            this.simpleButton32.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton32.Location = new System.Drawing.Point(0, 0);
            this.simpleButton32.Name = "simpleButton32";
            this.simpleButton32.Size = new System.Drawing.Size(101, 36);
            this.simpleButton32.TabIndex = 0;
            this.simpleButton32.Text = "Yeni";
            this.simpleButton32.Click += new System.EventHandler(this.simpleButton32_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCalistir);
            this.panel2.Controls.Add(this.rapor_adi);
            this.panel2.Controls.Add(this.labelControl1);
            this.panel2.Controls.Add(this.meSql);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 356);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(309, 178);
            this.panel2.TabIndex = 2;
            // 
            // btnCalistir
            // 
            this.btnCalistir.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCalistir.Location = new System.Drawing.Point(254, 0);
            this.btnCalistir.Name = "btnCalistir";
            this.btnCalistir.Size = new System.Drawing.Size(55, 33);
            this.btnCalistir.TabIndex = 3;
            this.btnCalistir.Text = "Çalıştır";
            this.btnCalistir.Click += new System.EventHandler(this.btnCalistir_Click);
            // 
            // rapor_adi
            // 
            this.rapor_adi.Location = new System.Drawing.Point(94, 7);
            this.rapor_adi.Name = "rapor_adi";
            this.rapor_adi.Size = new System.Drawing.Size(144, 20);
            this.rapor_adi.TabIndex = 2;
            this.rapor_adi.Tag = "0";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 11);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(47, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Rapor Adı";
            // 
            // meSql
            // 
            this.meSql.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.meSql.EditValue = "select s.pkSatislar,s.GuncellemeTarihi,s.fkFirma as s_firma,kh.fkFirma as kh_firm" +
    "a from Satislar s\r\nleft join KasaHareket kh on kh.fkSatislar=s.pkSatislar\r\nwhere" +
    " s.fkFirma<>kh.fkFirma";
            this.meSql.Location = new System.Drawing.Point(0, 33);
            this.meSql.Name = "meSql";
            this.meSql.Size = new System.Drawing.Size(309, 145);
            this.meSql.TabIndex = 0;
            // 
            // gcSql
            // 
            this.gcSql.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.gcSql.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcSql.Location = new System.Drawing.Point(0, 36);
            this.gcSql.MainView = this.gridView1;
            this.gcSql.Name = "gcSql";
            this.gcSql.Size = new System.Drawing.Size(618, 498);
            this.gcSql.TabIndex = 0;
            this.gcSql.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcSql;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.simpleButton2);
            this.panel3.Controls.Add(this.simpleButton3);
            this.panel3.Controls.Add(this.simpleButton1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(618, 36);
            this.panel3.TabIndex = 1;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton2.Location = new System.Drawing.Point(315, 0);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(101, 36);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "Yazdır";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton3.Location = new System.Drawing.Point(416, 0);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(101, 36);
            this.simpleButton3.TabIndex = 0;
            this.simpleButton3.Text = "Excel At";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton1.Location = new System.Drawing.Point(517, 0);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(101, 36);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "e-Posta Gönder";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 560);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(937, 41);
            this.label1.TabIndex = 2;
            this.label1.Text = "Grupları Sil";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmRaporlarOzel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 601);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.label1);
            this.Name = "frmRaporlarOzel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Raporlar Özel";
            this.Load += new System.EventHandler(this.frmAktarim_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage10.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cgOzelRaporlar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView14)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rapor_adi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.meSql.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcSql)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage10;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraGrid.GridControl cgOzelRaporlar;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton simpleButton33;
        private DevExpress.XtraEditors.SimpleButton simpleButton32;
        private DevExpress.XtraEditors.TextEdit rapor_adi;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit meSql;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraEditors.SimpleButton simpleButton34;
        private DevExpress.XtraEditors.SimpleButton btnCalistir;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        public DevExpress.XtraGrid.GridControl gcSql;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}
﻿namespace GPTS
{
    partial class frmKasaGirisCikisTurListesi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKasaGirisCikisTurListesi));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.yeniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.düzenleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcGider = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcGelir = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcGirisCikis = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.sbtnSil = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(481, 324);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.yeniToolStripMenuItem,
            this.düzenleToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(168, 92);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolStripMenuItem1.Size = new System.Drawing.Size(167, 22);
            this.toolStripMenuItem1.Text = "Stok Grubu Sil";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // yeniToolStripMenuItem
            // 
            this.yeniToolStripMenuItem.Name = "yeniToolStripMenuItem";
            this.yeniToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.yeniToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.yeniToolStripMenuItem.Text = "Yeni ";
            this.yeniToolStripMenuItem.Click += new System.EventHandler(this.yeniToolStripMenuItem_Click);
            // 
            // düzenleToolStripMenuItem
            // 
            this.düzenleToolStripMenuItem.Name = "düzenleToolStripMenuItem";
            this.düzenleToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.düzenleToolStripMenuItem.Text = "Düzenle";
            this.düzenleToolStripMenuItem.Click += new System.EventHandler(this.düzenleToolStripMenuItem_Click);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gcGider,
            this.gcGelir,
            this.gcGirisCikis,
            this.gridColumn3,
            this.gridColumn4});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.ViewCaption = "Sistemdeki Grupları";
            this.gridView1.DoubleClick += new System.EventHandler(this.düzenleToolStripMenuItem_Click);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.Caption = "pkKasaGirisCikisTurleri";
            this.gridColumn1.FieldName = "pkKasaGirisCikisTurleri";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Width = 49;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn2.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Acıklama";
            this.gridColumn2.FieldName = "Aciklama";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 287;
            // 
            // gcGider
            // 
            this.gcGider.Caption = "GiderOlarakisle";
            this.gcGider.FieldName = "GiderOlarakisle";
            this.gcGider.Name = "gcGider";
            // 
            // gcGelir
            // 
            this.gcGelir.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gcGelir.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gcGelir.AppearanceHeader.Options.UseFont = true;
            this.gcGelir.AppearanceHeader.Options.UseForeColor = true;
            this.gcGelir.Caption = "GelirOlarakisle";
            this.gcGelir.FieldName = "GelirOlarakisle";
            this.gcGelir.Name = "gcGelir";
            this.gcGelir.OptionsColumn.AllowEdit = false;
            this.gcGelir.Width = 101;
            // 
            // gcGirisCikis
            // 
            this.gcGirisCikis.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gcGirisCikis.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gcGirisCikis.AppearanceHeader.Options.UseFont = true;
            this.gcGirisCikis.AppearanceHeader.Options.UseForeColor = true;
            this.gcGirisCikis.AppearanceHeader.Options.UseTextOptions = true;
            this.gcGirisCikis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcGirisCikis.Caption = "GirisCikis";
            this.gcGirisCikis.FieldName = "GirisCikis";
            this.gcGirisCikis.Name = "gcGirisCikis";
            this.gcGirisCikis.OptionsColumn.AllowEdit = false;
            this.gcGirisCikis.Width = 108;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Aktif";
            this.gridColumn3.FieldName = "Aktif";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 62;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Grup Adı";
            this.gridColumn4.FieldName = "GrupAdi";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 114;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 55);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(487, 350);
            this.xtraTabControl1.TabIndex = 4;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gridControl1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(481, 324);
            this.xtraTabPage1.Text = "Kasa Giriş Cikiş Grupları";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.sbtnSil);
            this.panelControl2.Controls.Add(this.simpleButton4);
            this.panelControl2.Controls.Add(this.simpleButton21);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(487, 55);
            this.panelControl2.TabIndex = 5;
            // 
            // sbtnSil
            // 
            this.sbtnSil.Dock = System.Windows.Forms.DockStyle.Left;
            this.sbtnSil.Image = ((System.Drawing.Image)(resources.GetObject("sbtnSil.Image")));
            this.sbtnSil.Location = new System.Drawing.Point(119, 2);
            this.sbtnSil.Name = "sbtnSil";
            this.sbtnSil.Size = new System.Drawing.Size(109, 51);
            this.sbtnSil.TabIndex = 84;
            this.sbtnSil.Text = "Sil [F5]";
            this.sbtnSil.Click += new System.EventHandler(this.sbtnSil_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(2, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(117, 51);
            this.simpleButton4.TabIndex = 85;
            this.simpleButton4.Text = "Yeni [F7]";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton21.Image")));
            this.simpleButton21.Location = new System.Drawing.Point(357, 2);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(128, 51);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // frmKasaGirisCikisTurListesi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 405);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControl2);
            this.KeyPreview = true;
            this.Name = "frmKasaGirisCikisTurListesi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kasa Giriş Çikis Tür Listesi";
            this.Load += new System.EventHandler(this.frmStokGrupKarti_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStokGrupKarti_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gcGirisCikis;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton sbtnSil;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private System.Windows.Forms.ToolStripMenuItem yeniToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gcGelir;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private System.Windows.Forms.ToolStripMenuItem düzenleToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gcGider;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}
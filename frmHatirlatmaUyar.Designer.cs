namespace GPTS
{
    partial class frmHatirlatmaUyar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHatirlatmaUyar));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.açToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yeniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.animsat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cbTekrarlamaSecenek = new DevExpress.XtraEditors.ComboBoxEdit();
            this.seDk = new DevExpress.XtraEditors.SpinEdit();
            this.deHatirtama_zamani = new DevExpress.XtraEditors.DateEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btnhicbiri = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.baslik = new System.Windows.Forms.Label();
            this.lbKonu = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbTekrarlamaSecenek.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDk.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deHatirtama_zamani.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deHatirtama_zamani.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 88);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(849, 460);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.açToolStripMenuItem,
            this.yeniToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(140, 80);
            // 
            // açToolStripMenuItem
            // 
            this.açToolStripMenuItem.Name = "açToolStripMenuItem";
            this.açToolStripMenuItem.Size = new System.Drawing.Size(139, 38);
            this.açToolStripMenuItem.Text = "Aç";
            this.açToolStripMenuItem.Click += new System.EventHandler(this.açToolStripMenuItem_Click);
            // 
            // yeniToolStripMenuItem
            // 
            this.yeniToolStripMenuItem.Image = global::GPTS.Properties.Resources.randevuekle_48x32;
            this.yeniToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.yeniToolStripMenuItem.Name = "yeniToolStripMenuItem";
            this.yeniToolStripMenuItem.Size = new System.Drawing.Size(139, 38);
            this.yeniToolStripMenuItem.Text = "Yeni";
            this.yeniToolStripMenuItem.Click += new System.EventHandler(this.yeniToolStripMenuItem_Click);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn7,
            this.gridColumn1,
            this.gridColumn4,
            this.animsat,
            this.gridColumn5,
            this.gridColumn6});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.DoubleClick += new System.EventHandler(this.açToolStripMenuItem_Click);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "pkHatirlatmaAnimsat";
            this.gridColumn2.FieldName = "pkHatirlatmaAnimsat";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Width = 122;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Anımsatma Zamanı";
            this.gridColumn3.DisplayFormat.FormatString = "g";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn3.FieldName = "animsat_zamani";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 166;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Konu";
            this.gridColumn7.FieldName = "Aciklama";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 0;
            this.gridColumn7.Width = 236;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Fark Dakika";
            this.gridColumn1.FieldName = "kalan_sure";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 178;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "BitisTarihi";
            this.gridColumn4.FieldName = "BitisTarihi";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Width = 145;
            // 
            // animsat
            // 
            this.animsat.Caption = "animsat";
            this.animsat.Name = "animsat";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "fkDurumu";
            this.gridColumn5.FieldName = "fkDurumu";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Cari Adı";
            this.gridColumn6.FieldName = "firmaadi";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 1;
            this.gridColumn6.Width = 251;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cbTekrarlamaSecenek);
            this.groupControl1.Controls.Add(this.seDk);
            this.groupControl1.Controls.Add(this.deHatirtama_zamani);
            this.groupControl1.Controls.Add(this.simpleButton2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1139, 88);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Anımsatma Bilgileri";
            // 
            // cbTekrarlamaSecenek
            // 
            this.cbTekrarlamaSecenek.EditValue = "Dakika";
            this.cbTekrarlamaSecenek.EnterMoveNextControl = true;
            this.cbTekrarlamaSecenek.Location = new System.Drawing.Point(544, 35);
            this.cbTekrarlamaSecenek.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbTekrarlamaSecenek.Name = "cbTekrarlamaSecenek";
            this.cbTekrarlamaSecenek.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbTekrarlamaSecenek.Properties.Appearance.Options.UseFont = true;
            this.cbTekrarlamaSecenek.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbTekrarlamaSecenek.Properties.Items.AddRange(new object[] {
            "Dakika",
            "Saat",
            "Gün",
            "Ay",
            "Yıl"});
            this.cbTekrarlamaSecenek.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbTekrarlamaSecenek.Size = new System.Drawing.Size(161, 27);
            this.cbTekrarlamaSecenek.TabIndex = 1009;
            // 
            // seDk
            // 
            this.seDk.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.seDk.Location = new System.Drawing.Point(454, 35);
            this.seDk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.seDk.Name = "seDk";
            this.seDk.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.seDk.Properties.Appearance.Options.UseFont = true;
            this.seDk.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seDk.Size = new System.Drawing.Size(84, 27);
            this.seDk.TabIndex = 3;
            // 
            // deHatirtama_zamani
            // 
            this.deHatirtama_zamani.EditValue = null;
            this.deHatirtama_zamani.Enabled = false;
            this.deHatirtama_zamani.Location = new System.Drawing.Point(148, 40);
            this.deHatirtama_zamani.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deHatirtama_zamani.Name = "deHatirtama_zamani";
            this.deHatirtama_zamani.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deHatirtama_zamani.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deHatirtama_zamani.Size = new System.Drawing.Size(211, 22);
            this.deHatirtama_zamani.TabIndex = 2;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(711, 35);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(132, 28);
            this.simpleButton2.TabIndex = 0;
            this.simpleButton2.Text = "Sonra Anımsatma";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(34, 42);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(102, 16);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Hatırlatma Zaman";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(711, 35);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(120, 45);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "Artık Anımsatma";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.dateEdit1);
            this.groupControl2.Controls.Add(this.labelControl4);
            this.groupControl2.Controls.Add(this.btnhicbiri);
            this.groupControl2.Controls.Add(this.simpleButton1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl2.Location = new System.Drawing.Point(0, 548);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(849, 86);
            this.groupControl2.TabIndex = 2;
            // 
            // dateEdit1
            // 
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Enabled = false;
            this.dateEdit1.Location = new System.Drawing.Point(316, 44);
            this.dateEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit1.Size = new System.Drawing.Size(211, 22);
            this.dateEdit1.TabIndex = 2;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(231, 46);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(72, 16);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "Çıkış Zamanı";
            // 
            // btnhicbiri
            // 
            this.btnhicbiri.Location = new System.Drawing.Point(12, 39);
            this.btnhicbiri.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnhicbiri.Name = "btnhicbiri";
            this.btnhicbiri.Size = new System.Drawing.Size(172, 28);
            this.btnhicbiri.TabIndex = 0;
            this.btnhicbiri.Text = "Hiçbirini Anımsatma";
            this.btnhicbiri.Click += new System.EventHandler(this.btnhicbiri_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbKonu);
            this.panel1.Controls.Add(this.baslik);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(849, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 546);
            this.panel1.TabIndex = 3;
            // 
            // baslik
            // 
            this.baslik.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.baslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.baslik.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.baslik.ForeColor = System.Drawing.Color.Black;
            this.baslik.Location = new System.Drawing.Point(0, 0);
            this.baslik.Name = "baslik";
            this.baslik.Size = new System.Drawing.Size(290, 162);
            this.baslik.TabIndex = 11;
            this.baslik.Text = "Müşteri Bilgisi";
            this.baslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbKonu
            // 
            this.lbKonu.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lbKonu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbKonu.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lbKonu.ForeColor = System.Drawing.Color.Black;
            this.lbKonu.Location = new System.Drawing.Point(0, 162);
            this.lbKonu.Name = "lbKonu";
            this.lbKonu.Size = new System.Drawing.Size(290, 384);
            this.lbKonu.TabIndex = 128;
            this.lbKonu.Text = "Konu";
            this.lbKonu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmHatirlatmaUyar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 634);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHatirlatmaUyar";
            this.Text = "Anımsatıcı";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHatirlatmaUyar_FormClosed);
            this.Load += new System.EventHandler(this.frmAracTakip_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbTekrarlamaSecenek.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seDk.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deHatirtama_zamani.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deHatirtama_zamani.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SpinEdit seDk;
        private DevExpress.XtraEditors.DateEdit deHatirtama_zamani;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton btnhicbiri;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn animsat;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem açToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yeniToolStripMenuItem;
        private DevExpress.XtraEditors.ComboBoxEdit cbTekrarlamaSecenek;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private System.Windows.Forms.Label baslik;
        private System.Windows.Forms.Label lbKonu;
    }
}
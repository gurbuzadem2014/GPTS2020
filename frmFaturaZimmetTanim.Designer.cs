namespace GPTS
{
    partial class frmFaturaZimmetTanim
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
            this.musteriadi = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton19 = new DevExpress.XtraEditors.SimpleButton();
            this.lueKullanicilar = new DevExpress.XtraEditors.LookUpEdit();
            this.lcKullanici = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.seFaturaNo = new DevExpress.XtraEditors.SpinEdit();
            this.teSeriNo = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.seFaturaNo2 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.silToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueKullanicilar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seFaturaNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSeriNo.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seFaturaNo2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.musteriadi);
            this.panelControl1.Controls.Add(this.simpleButton19);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(484, 53);
            this.panelControl1.TabIndex = 7;
            // 
            // musteriadi
            // 
            this.musteriadi.Dock = System.Windows.Forms.DockStyle.Left;
            this.musteriadi.Location = new System.Drawing.Point(2, 2);
            this.musteriadi.Name = "musteriadi";
            this.musteriadi.Size = new System.Drawing.Size(0, 13);
            this.musteriadi.TabIndex = 23;
            // 
            // simpleButton19
            // 
            this.simpleButton19.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton19.Appearance.Options.UseFont = true;
            this.simpleButton19.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton19.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton19.Location = new System.Drawing.Point(369, 2);
            this.simpleButton19.Name = "simpleButton19";
            this.simpleButton19.Size = new System.Drawing.Size(113, 49);
            this.simpleButton19.TabIndex = 22;
            this.simpleButton19.Text = "Kapat [ESC]";
            this.simpleButton19.Click += new System.EventHandler(this.simpleButton19_Click);
            // 
            // lueKullanicilar
            // 
            this.lueKullanicilar.Location = new System.Drawing.Point(79, 14);
            this.lueKullanicilar.Name = "lueKullanicilar";
            this.lueKullanicilar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lueKullanicilar.Properties.Appearance.Options.UseFont = true;
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
            this.lueKullanicilar.Size = new System.Drawing.Size(253, 21);
            this.lueKullanicilar.TabIndex = 36;
            this.lueKullanicilar.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.lueKullanicilar.EditValueChanged += new System.EventHandler(this.lueKullanicilar_EditValueChanged);
            // 
            // lcKullanici
            // 
            this.lcKullanici.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.lcKullanici.Location = new System.Drawing.Point(15, 18);
            this.lcKullanici.Name = "lcKullanici";
            this.lcKullanici.Size = new System.Drawing.Size(49, 13);
            this.lcKullanici.TabIndex = 35;
            this.lcKullanici.Tag = "0";
            this.lcKullanici.Text = "Kullanıcılar";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl1.Location = new System.Drawing.Point(33, 64);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(34, 13);
            this.labelControl1.TabIndex = 37;
            this.labelControl1.Tag = "0";
            this.labelControl1.Text = "Seri No";
            this.labelControl1.Visible = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl2.Location = new System.Drawing.Point(11, 110);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 13);
            this.labelControl2.TabIndex = 38;
            this.labelControl2.Tag = "0";
            this.labelControl2.Text = "ilk Fatura No";
            // 
            // seFaturaNo
            // 
            this.seFaturaNo.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seFaturaNo.Location = new System.Drawing.Point(79, 107);
            this.seFaturaNo.Name = "seFaturaNo";
            this.seFaturaNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seFaturaNo.Size = new System.Drawing.Size(81, 20);
            this.seFaturaNo.TabIndex = 42;
            // 
            // teSeriNo
            // 
            this.teSeriNo.Location = new System.Drawing.Point(79, 60);
            this.teSeriNo.Name = "teSeriNo";
            this.teSeriNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.teSeriNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.teSeriNo.Size = new System.Drawing.Size(129, 20);
            this.teSeriNo.TabIndex = 40;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = global::GPTS.Properties.Resources.dataal_24x24;
            this.simpleButton1.Location = new System.Drawing.Point(357, 92);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(84, 35);
            this.simpleButton1.TabIndex = 24;
            this.simpleButton1.Text = "Oluştur";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.seFaturaNo2);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.simpleButton1);
            this.panel1.Controls.Add(this.lcKullanici);
            this.panel1.Controls.Add(this.seFaturaNo);
            this.panel1.Controls.Add(this.lueKullanicilar);
            this.panel1.Controls.Add(this.teSeriNo);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 139);
            this.panel1.TabIndex = 43;
            // 
            // seFaturaNo2
            // 
            this.seFaturaNo2.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.seFaturaNo2.Location = new System.Drawing.Point(251, 107);
            this.seFaturaNo2.Name = "seFaturaNo2";
            this.seFaturaNo2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seFaturaNo2.Size = new System.Drawing.Size(81, 20);
            this.seFaturaNo2.TabIndex = 44;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl3.Location = new System.Drawing.Point(183, 110);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(69, 13);
            this.labelControl3.TabIndex = 43;
            this.labelControl3.Tag = "0";
            this.labelControl3.Text = "Son Fatura No";
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 192);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(484, 170);
            this.gridControl1.TabIndex = 44;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.silToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(87, 26);
            // 
            // silToolStripMenuItem
            // 
            this.silToolStripMenuItem.Name = "silToolStripMenuItem";
            this.silToolStripMenuItem.Size = new System.Drawing.Size(86, 22);
            this.silToolStripMenuItem.Text = "Sil";
            this.silToolStripMenuItem.Click += new System.EventHandler(this.silToolStripMenuItem_Click);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Seri No";
            this.gridColumn1.FieldName = "SeriNo";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "FaturaNo";
            this.gridColumn2.FieldName = "FaturaNo";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Durumu";
            this.gridColumn4.FieldName = "Durumu";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "fkSatislar";
            this.gridColumn5.FieldName = "fkSatislar";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "fkFaturaToplu";
            this.gridColumn6.FieldName = "fkFaturaToplu";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            // 
            // frmFaturaZimmetTanim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmFaturaZimmetTanim";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seri Fatura Zimmetleme";
            this.Load += new System.EventHandler(this.frmUcGoster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueKullanicilar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seFaturaNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSeriNo.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seFaturaNo2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton19;
        public DevExpress.XtraEditors.LabelControl musteriadi;
        private DevExpress.XtraEditors.LookUpEdit lueKullanicilar;
        private DevExpress.XtraEditors.LabelControl lcKullanici;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit seFaturaNo;
        private DevExpress.XtraEditors.TextEdit teSeriNo;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.SpinEdit seFaturaNo2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem silToolStripMenuItem;
    }
}
namespace GPTS
{
    partial class frmStokFiyatGrupKarti
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.Baslik = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.seMaxid = new DevExpress.XtraEditors.SpinEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbAktif = new DevExpress.XtraEditors.CheckEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.repositoryItemCalcEditKdvDahil = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.repositoryItemCalcEditKdvHaric = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.btnPasifYap = new DevExpress.XtraEditors.SimpleButton();
            this.btnAktifYap = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.Baslik.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seMaxid.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbAktif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEditKdvDahil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEditKdvHaric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            this.SuspendLayout();
            // 
            // Baslik
            // 
            this.Baslik.EnterMoveNextControl = true;
            this.Baslik.Location = new System.Drawing.Point(158, 10);
            this.Baslik.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Baslik.Name = "Baslik";
            this.Baslik.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.Baslik.Properties.Appearance.Options.UseFont = true;
            this.Baslik.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Baslik.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.Baslik.Size = new System.Drawing.Size(238, 31);
            toolTipItem1.Text = "Stoklarınızı girmeden önce lütfen kendinize bir sıralama belirleyiniz. Örn:ÜLKER " +
    "KREMA BİSKÜVİ 30GR";
            superToolTip1.Items.Add(toolTipItem1);
            this.Baslik.SuperTip = superToolTip1;
            this.Baslik.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(616, 61);
            this.panelControl1.TabIndex = 2;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.save;
            this.BtnKaydet.Location = new System.Drawing.Point(340, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(131, 57);
            this.BtnKaydet.TabIndex = 5;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(471, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(143, 57);
            this.simpleButton21.TabIndex = 6;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // seMaxid
            // 
            this.seMaxid.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.seMaxid.EnterMoveNextControl = true;
            this.seMaxid.Location = new System.Drawing.Point(462, 9);
            this.seMaxid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.seMaxid.Name = "seMaxid";
            this.seMaxid.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.seMaxid.Properties.Appearance.Options.UseFont = true;
            this.seMaxid.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.seMaxid.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.seMaxid.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seMaxid.Size = new System.Drawing.Size(86, 31);
            toolTipTitleItem1.Text = "NAKİT FİYAT ÜZERİNDEN İSKONTO";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Nakit Fiyatı Baz Alarak Belirlediğiniz oranda Satış Fiyatını Sabitler";
            superToolTip2.Items.Add(toolTipTitleItem1);
            superToolTip2.Items.Add(toolTipItem2);
            this.seMaxid.SuperTip = superToolTip2;
            this.seMaxid.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Image = global::GPTS.Properties.Resources.info_bilgi_32x32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 355);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(616, 43);
            this.label1.TabIndex = 171;
            this.label1.Text = "Satış Fiyat Grubu Oluşturduğunuzda Tüm ürünlerin Yeni Fiyatları 1.Fiyat üzerinden" +
    " Oluşmaktar.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbAktif);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.seMaxid);
            this.panel1.Controls.Add(this.Baslik);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 61);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(616, 50);
            this.panel1.TabIndex = 0;
            // 
            // cbAktif
            // 
            this.cbAktif.EditValue = true;
            this.cbAktif.Location = new System.Drawing.Point(401, 14);
            this.cbAktif.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAktif.Name = "cbAktif";
            this.cbAktif.Properties.Caption = "Aktif";
            this.cbAktif.Size = new System.Drawing.Size(54, 21);
            this.cbAktif.TabIndex = 172;
            this.cbAktif.Visible = false;
            this.cbAktif.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbAktif_MouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(552, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 17);
            this.label3.TabIndex = 171;
            this.label3.Text = ".FİYAT";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(7, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 17);
            this.label2.TabIndex = 171;
            this.label2.Text = "Yeni Fiyat Grubu Adı";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 111);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageComboBox1,
            this.repositoryItemCalcEditKdvDahil,
            this.repositoryItemCalcEditKdvHaric,
            this.repositoryItemSpinEdit1,
            this.repositoryItemHyperLinkEdit1});
            this.gridControl1.Size = new System.Drawing.Size(616, 185);
            this.gridControl1.TabIndex = 173;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn2,
            this.gridColumn1,
            this.gridColumn3});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowColumnHeaders = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "id";
            this.gridColumn6.FieldName = "pkSatisFiyatlariBaslik";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Width = 30;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn2.Caption = "Satış Fiyat Grubu";
            this.gridColumn2.FieldName = "Baslik";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 374;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Fiyat Id";
            this.gridColumn1.FieldName = "Tur";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 194;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Aktif";
            this.gridColumn3.FieldName = "Aktif";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // repositoryItemImageComboBox1
            // 
            this.repositoryItemImageComboBox1.AutoHeight = false;
            this.repositoryItemImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox1.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Aktif", true, 0),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Pasif", false, 1)});
            this.repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1";
            // 
            // repositoryItemCalcEditKdvDahil
            // 
            this.repositoryItemCalcEditKdvDahil.AccessibleDescription = "Kdv Dahil";
            this.repositoryItemCalcEditKdvDahil.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.repositoryItemCalcEditKdvDahil.AppearanceFocused.Options.UseBackColor = true;
            this.repositoryItemCalcEditKdvDahil.AutoHeight = false;
            this.repositoryItemCalcEditKdvDahil.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEditKdvDahil.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemCalcEditKdvDahil.Name = "repositoryItemCalcEditKdvDahil";
            // 
            // repositoryItemCalcEditKdvHaric
            // 
            this.repositoryItemCalcEditKdvHaric.AccessibleDescription = "Kdv Hariç";
            this.repositoryItemCalcEditKdvHaric.AutoHeight = false;
            this.repositoryItemCalcEditKdvHaric.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEditKdvHaric.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemCalcEditKdvHaric.Name = "repositoryItemCalcEditKdvHaric";
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AccessibleDescription = "iskonto";
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.Image = global::GPTS.Properties.Resources.db_delete;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            // 
            // btnPasifYap
            // 
            this.btnPasifYap.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPasifYap.Image = global::GPTS.Properties.Resources.delete_icon_redman_resized_600_jpg_resized_600___Kopya;
            this.btnPasifYap.Location = new System.Drawing.Point(2, 2);
            this.btnPasifYap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPasifYap.Name = "btnPasifYap";
            this.btnPasifYap.Size = new System.Drawing.Size(131, 55);
            this.btnPasifYap.TabIndex = 7;
            this.btnPasifYap.Text = "Pasif Yap";
            this.btnPasifYap.Click += new System.EventHandler(this.btnPasifYap_Click);
            // 
            // btnAktifYap
            // 
            this.btnAktifYap.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAktifYap.Image = global::GPTS.Properties.Resources.onay_32x32;
            this.btnAktifYap.Location = new System.Drawing.Point(133, 2);
            this.btnAktifYap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAktifYap.Name = "btnAktifYap";
            this.btnAktifYap.Size = new System.Drawing.Size(131, 55);
            this.btnAktifYap.TabIndex = 8;
            this.btnAktifYap.Text = "Aktif Yap";
            this.btnAktifYap.Click += new System.EventHandler(this.btnAktifYap_Click);
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.btnAktifYap);
            this.panelControl5.Controls.Add(this.btnPasifYap);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl5.Location = new System.Drawing.Point(0, 296);
            this.panelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(616, 59);
            this.panelControl5.TabIndex = 195;
            // 
            // frmStokFiyatGrupKarti
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(616, 398);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmStokFiyatGrupKarti";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Satış Fiyat Grupları";
            this.Load += new System.EventHandler(this.frmStokFiyatGrupKarti_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStokFiyatGrupKarti_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Baslik.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seMaxid.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbAktif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEditKdvDahil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEditKdvHaric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit Baslik;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SpinEdit seMaxid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEditKdvDahil;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEditKdvHaric;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.CheckEdit cbAktif;
        private DevExpress.XtraEditors.SimpleButton btnPasifYap;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.SimpleButton btnAktifYap;
        private DevExpress.XtraEditors.PanelControl panelControl5;
    }
}
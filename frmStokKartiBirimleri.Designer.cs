namespace GPTS
{
    partial class frmStokKartiBirimleri
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStokKartiBirimleri));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.pkStokKartiid = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpBirim = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.SatisFiyati1 = new DevExpress.XtraEditors.CalcEdit();
            this.speicindekimiktar = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.lueBirimler = new DevExpress.XtraEditors.LookUpEdit();
            this.simpleButton25 = new DevExpress.XtraEditors.SimpleButton();
            this.teStokAdi = new DevExpress.XtraEditors.TextEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pkStokKartiid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpBirim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SatisFiyati1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speicindekimiktar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueBirimler.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStokAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Controls.Add(this.pkStokKartiid);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(722, 48);
            this.panelControl1.TabIndex = 1;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton4.Appearance.Options.UseFont = true;
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButton4.Location = new System.Drawing.Point(150, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(159, 44);
            this.simpleButton4.TabIndex = 12;
            this.simpleButton4.Text = "Listeye Ekle [F8]";
            this.simpleButton4.ToolTip = "Satılacak stokları bulur";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // pkStokKartiid
            // 
            this.pkStokKartiid.EditValue = "1";
            this.pkStokKartiid.Location = new System.Drawing.Point(344, 12);
            this.pkStokKartiid.Name = "pkStokKartiid";
            this.pkStokKartiid.Size = new System.Drawing.Size(43, 20);
            this.pkStokKartiid.TabIndex = 11;
            this.pkStokKartiid.Visible = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton1.Image = global::GPTS.Properties.Resources.sil_1;
            this.simpleButton1.Location = new System.Drawing.Point(2, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(148, 44);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "Listeden Çıkar [F2]";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.simpleButton2.Location = new System.Drawing.Point(490, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(115, 44);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "Tamam";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton3.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton3.Location = new System.Drawing.Point(605, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(115, 44);
            this.simpleButton3.TabIndex = 2;
            this.simpleButton3.Text = "Vazgeç [ESC]";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 100);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpBirim});
            this.gridControl1.Size = new System.Drawing.Size(722, 328);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "pkStokKarti";
            this.gridColumn1.FieldName = "pkStokKarti";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Width = 67;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Stok Adı";
            this.gridColumn2.FieldName = "Stokadi";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 186;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.Caption = "Barcode";
            this.gridColumn3.FieldName = "Barcode";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 95;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.Caption = "Birimi";
            this.gridColumn4.ColumnEdit = this.repositoryItemLookUpBirim;
            this.gridColumn4.FieldName = "Stoktipi";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 106;
            // 
            // repositoryItemLookUpBirim
            // 
            this.repositoryItemLookUpBirim.AutoHeight = false;
            this.repositoryItemLookUpBirim.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpBirim.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("BirimAdi", "Birim Adı")});
            this.repositoryItemLookUpBirim.DisplayMember = "BirimAdi";
            this.repositoryItemLookUpBirim.Name = "repositoryItemLookUpBirim";
            this.repositoryItemLookUpBirim.ValueMember = "pkBirimler";
            this.repositoryItemLookUpBirim.EditValueChanged += new System.EventHandler(this.repositoryItemLookUpBirim_EditValueChanged);
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.Caption = "Mevcut";
            this.gridColumn5.FieldName = "Mevcut";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            this.gridColumn5.Width = 45;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.Caption = "Satis Fiyatı";
            this.gridColumn6.FieldName = "SatisFiyati";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 73;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridColumn7.AppearanceHeader.Options.UseFont = true;
            this.gridColumn7.Caption = "pkStokKartiid";
            this.gridColumn7.FieldName = "pkStokKartiid";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Width = 45;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "İçindeki Adet/Gr";
            this.gridColumn8.FieldName = "CikisAdet";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 3;
            this.gridColumn8.Width = 87;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Alış Fiyatı";
            this.gridColumn9.FieldName = "AlisFiyati";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 5;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Fiyatları Ana Birimden Al";
            this.gridColumn10.Name = "gridColumn10";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.SatisFiyati1);
            this.panelControl2.Controls.Add(this.speicindekimiktar);
            this.panelControl2.Controls.Add(this.labelControl20);
            this.panelControl2.Controls.Add(this.lueBirimler);
            this.panelControl2.Controls.Add(this.simpleButton25);
            this.panelControl2.Controls.Add(this.teStokAdi);
            this.panelControl2.Controls.Add(this.textEdit1);
            this.panelControl2.Controls.Add(this.simpleButton6);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 48);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(722, 52);
            this.panelControl2.TabIndex = 3;
            this.panelControl2.Visible = false;
            // 
            // SatisFiyati1
            // 
            this.SatisFiyati1.EnterMoveNextControl = true;
            this.SatisFiyati1.Location = new System.Drawing.Point(474, 19);
            this.SatisFiyati1.Name = "SatisFiyati1";
            this.SatisFiyati1.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.SatisFiyati1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.SatisFiyati1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SatisFiyati1.Properties.DisplayFormat.FormatString = "{0:#0.00####}";
            this.SatisFiyati1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.SatisFiyati1.Properties.EditFormat.FormatString = "{0:#0.00####}";
            this.SatisFiyati1.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.SatisFiyati1.Size = new System.Drawing.Size(95, 20);
            this.SatisFiyati1.TabIndex = 190;
            // 
            // speicindekimiktar
            // 
            this.speicindekimiktar.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speicindekimiktar.Location = new System.Drawing.Point(414, 19);
            this.speicindekimiktar.Name = "speicindekimiktar";
            this.speicindekimiktar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speicindekimiktar.Size = new System.Drawing.Size(54, 20);
            this.speicindekimiktar.TabIndex = 189;
            this.speicindekimiktar.TabStop = false;
            // 
            // labelControl20
            // 
            this.labelControl20.Location = new System.Drawing.Point(295, 5);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(24, 13);
            toolTipItem1.Text = "İsteğinize bağlı olarak marka ve model tanımlayabilirsiniz";
            superToolTip1.Items.Add(toolTipItem1);
            this.labelControl20.SuperTip = superToolTip1;
            this.labelControl20.TabIndex = 186;
            this.labelControl20.Text = "Birimi";
            // 
            // lueBirimler
            // 
            this.lueBirimler.Location = new System.Drawing.Point(292, 19);
            this.lueBirimler.Name = "lueBirimler";
            this.lueBirimler.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lueBirimler.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.lueBirimler.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueBirimler.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkTedarikciler", "pkTedarikciler", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Firmaadi", "Firmaadi")});
            this.lueBirimler.Properties.DisplayMember = "Firmaadi";
            this.lueBirimler.Properties.NullText = "Seçiniz...";
            this.lueBirimler.Properties.ShowHeader = false;
            this.lueBirimler.Properties.ValueMember = "pkTedarikciler";
            this.lueBirimler.Size = new System.Drawing.Size(95, 20);
            this.lueBirimler.TabIndex = 185;
            // 
            // simpleButton25
            // 
            this.simpleButton25.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton25.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton25.Image")));
            this.simpleButton25.Location = new System.Drawing.Point(384, 17);
            this.simpleButton25.Name = "simpleButton25";
            this.simpleButton25.Size = new System.Drawing.Size(29, 22);
            this.simpleButton25.TabIndex = 187;
            this.simpleButton25.TabStop = false;
            // 
            // teStokAdi
            // 
            this.teStokAdi.EditValue = "";
            this.teStokAdi.Location = new System.Drawing.Point(109, 19);
            this.teStokAdi.Name = "teStokAdi";
            this.teStokAdi.Size = new System.Drawing.Size(177, 20);
            this.teStokAdi.TabIndex = 12;
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "";
            this.textEdit1.Location = new System.Drawing.Point(12, 19);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(94, 20);
            this.textEdit1.TabIndex = 11;
            // 
            // simpleButton6
            // 
            this.simpleButton6.Image = global::GPTS.Properties.Resources.insert_object;
            this.simpleButton6.Location = new System.Drawing.Point(575, 13);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(85, 33);
            this.simpleButton6.TabIndex = 2;
            this.simpleButton6.Text = "Ekle";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // frmStokKartiBirimleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 428);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Name = "frmStokKartiBirimleri";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Diğer Birimler";
            this.Load += new System.EventHandler(this.frmStokKartiBirimleri_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStokKartiBirimleri_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pkStokKartiid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpBirim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SatisFiyati1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speicindekimiktar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueBirimler.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStokAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        public DevExpress.XtraEditors.TextEdit pkStokKartiid;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpBirim;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        public DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        public DevExpress.XtraEditors.TextEdit teStokAdi;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.LookUpEdit lueBirimler;
        private DevExpress.XtraEditors.SimpleButton simpleButton25;
        private DevExpress.XtraEditors.SpinEdit speicindekimiktar;
        private DevExpress.XtraEditors.CalcEdit SatisFiyati1;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
    }
}
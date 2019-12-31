namespace GPTS
{
    partial class frmStokSatisGrafigi
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions1 = new DevExpress.XtraCharts.PointOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStokSatisGrafigi));
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.pivotGridControl1 = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.pivotGridField1 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField2 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField3 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.cbGosterimSekli = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lueStoklar = new DevExpress.XtraEditors.LookUpEdit();
            this.cbTarihAraligi = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.sontarih = new DevExpress.XtraEditors.DateEdit();
            this.ilktarih = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbGosterimSekli.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStoklar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTarihAraligi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sontarih.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sontarih.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilktarih.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilktarih.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            this.chartControl1.AppearanceName = "Chameleon";
            this.chartControl1.DataSource = this.pivotGridControl1;
            xyDiagram1.AxisX.Label.Staggered = true;
            xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
            xyDiagram1.AxisX.Range.SideMarginsEnabled = true;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
            xyDiagram1.AxisY.Range.SideMarginsEnabled = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.MaxHorizontalPercentage = 30D;
            this.chartControl1.Location = new System.Drawing.Point(0, 299);
            this.chartControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.RuntimeSelection = true;
            this.chartControl1.SeriesDataMember = "Series";
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControl1.SeriesTemplate.ArgumentDataMember = "Arguments";
            sideBySideBarSeriesLabel1.LineVisible = true;
            sideBySideBarSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.Default;
            this.chartControl1.SeriesTemplate.Label = sideBySideBarSeriesLabel1;
            pointOptions1.PointView = DevExpress.XtraCharts.PointView.Argument;
            this.chartControl1.SeriesTemplate.LegendPointOptions = pointOptions1;
            this.chartControl1.SeriesTemplate.SynchronizePointOptions = false;
            this.chartControl1.SeriesTemplate.TopNOptions.Count = 20;
            this.chartControl1.SeriesTemplate.TopNOptions.OthersArgument = "Diğerleri";
            this.chartControl1.SeriesTemplate.ValueDataMembersSerializable = "Values";
            this.chartControl1.Size = new System.Drawing.Size(1098, 368);
            this.chartControl1.TabIndex = 1;
            // 
            // pivotGridControl1
            // 
            this.pivotGridControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pivotGridControl1.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.pivotGridField1,
            this.pivotGridField2,
            this.pivotGridField3});
            this.pivotGridControl1.Location = new System.Drawing.Point(0, 53);
            this.pivotGridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pivotGridControl1.Name = "pivotGridControl1";
            this.pivotGridControl1.OptionsChartDataSource.FieldValuesProvideMode = DevExpress.XtraPivotGrid.PivotChartFieldValuesProvideMode.DisplayText;
            this.pivotGridControl1.OptionsChartDataSource.ProvideColumnCustomTotals = false;
            this.pivotGridControl1.Size = new System.Drawing.Size(1098, 246);
            this.pivotGridControl1.TabIndex = 3;
            // 
            // pivotGridField1
            // 
            this.pivotGridField1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField1.AreaIndex = 0;
            this.pivotGridField1.FieldName = "Stokadi";
            this.pivotGridField1.Name = "pivotGridField1";
            // 
            // pivotGridField2
            // 
            this.pivotGridField2.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField2.AreaIndex = 0;
            this.pivotGridField2.FieldName = "Satilan";
            this.pivotGridField2.Name = "pivotGridField2";
            this.pivotGridField2.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Descending;
            // 
            // pivotGridField3
            // 
            this.pivotGridField3.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField3.AreaIndex = 0;
            this.pivotGridField3.Caption = "Tarih";
            this.pivotGridField3.FieldName = "Tarih";
            this.pivotGridField3.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.Date;
            this.pivotGridField3.Name = "pivotGridField3";
            this.pivotGridField3.UnboundFieldName = "pivotGridField3";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.cbGosterimSekli);
            this.panelControl3.Controls.Add(this.labelControl4);
            this.panelControl3.Controls.Add(this.lueStoklar);
            this.panelControl3.Controls.Add(this.cbTarihAraligi);
            this.panelControl3.Controls.Add(this.labelControl8);
            this.panelControl3.Controls.Add(this.labelControl2);
            this.panelControl3.Controls.Add(this.labelControl7);
            this.panelControl3.Controls.Add(this.simpleButton3);
            this.panelControl3.Controls.Add(this.sontarih);
            this.panelControl3.Controls.Add(this.ilktarih);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1098, 53);
            this.panelControl3.TabIndex = 5;
            // 
            // cbGosterimSekli
            // 
            this.cbGosterimSekli.EditValue = "Günlük";
            this.cbGosterimSekli.Location = new System.Drawing.Point(296, 23);
            this.cbGosterimSekli.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbGosterimSekli.Name = "cbGosterimSekli";
            this.cbGosterimSekli.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cbGosterimSekli.Properties.Appearance.Options.UseFont = true;
            this.cbGosterimSekli.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbGosterimSekli.Properties.Items.AddRange(new object[] {
            "Günlük",
            "Haftanın Günleri",
            "Haftalık",
            "Aylık",
            "Yılık"});
            this.cbGosterimSekli.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbGosterimSekli.Size = new System.Drawing.Size(121, 25);
            this.cbGosterimSekli.TabIndex = 98;
            this.cbGosterimSekli.SelectedIndexChanged += new System.EventHandler(this.cbGosterimSekli_SelectedIndexChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(300, 2);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(74, 16);
            this.labelControl4.TabIndex = 97;
            this.labelControl4.Text = "Zaman Dilimi";
            // 
            // lueStoklar
            // 
            this.lueStoklar.EnterMoveNextControl = true;
            this.lueStoklar.Location = new System.Drawing.Point(6, 26);
            this.lueStoklar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueStoklar.Name = "lueStoklar";
            this.lueStoklar.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lueStoklar.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.lueStoklar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueStoklar.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkStokKarti", "pkStokKarti", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("StokAdi", "StokAdi")});
            this.lueStoklar.Properties.DisplayMember = "StokAdi";
            this.lueStoklar.Properties.NullText = "Tümü";
            this.lueStoklar.Properties.ShowHeader = false;
            this.lueStoklar.Properties.ValueMember = "pkStokKarti";
            this.lueStoklar.Size = new System.Drawing.Size(266, 22);
            this.lueStoklar.TabIndex = 8;
            // 
            // cbTarihAraligi
            // 
            this.cbTarihAraligi.Location = new System.Drawing.Point(439, 22);
            this.cbTarihAraligi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbTarihAraligi.Name = "cbTarihAraligi";
            this.cbTarihAraligi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cbTarihAraligi.Properties.Appearance.Options.UseFont = true;
            this.cbTarihAraligi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbTarihAraligi.Properties.DropDownRows = 11;
            this.cbTarihAraligi.Properties.Items.AddRange(new object[] {
            "Son 30 Gün",
            "Bugün",
            "Dün",
            "Yarın",
            "Geçen Hafta",
            "Bu Hafta",
            "Bu Ay",
            "Önceki Ay",
            "Bu Yıl",
            "Özel",
            "Son Ödemeden Sonra"});
            this.cbTarihAraligi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbTarihAraligi.Size = new System.Drawing.Size(206, 25);
            this.cbTarihAraligi.TabIndex = 7;
            this.cbTarihAraligi.SelectedIndexChanged += new System.EventHandler(this.cbTarihAraligi_SelectedIndexChanged);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(833, 3);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(56, 16);
            this.labelControl8.TabIndex = 6;
            this.labelControl8.Text = "Son Tarih";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(439, 6);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(70, 16);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Tarih Aralığı";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(661, 6);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(46, 16);
            this.labelControl7.TabIndex = 5;
            this.labelControl7.Text = "ilk Tarih";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(973, 6);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(120, 41);
            this.simpleButton3.TabIndex = 5;
            this.simpleButton3.Text = "Listele";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // sontarih
            // 
            this.sontarih.EditValue = new System.DateTime(2012, 3, 11, 23, 30, 10, 0);
            this.sontarih.Location = new System.Drawing.Point(833, 23);
            this.sontarih.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sontarih.Name = "sontarih";
            this.sontarih.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.sontarih.Properties.Appearance.Options.UseFont = true;
            this.sontarih.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sontarih.Properties.DisplayFormat.FormatString = "g";
            this.sontarih.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.sontarih.Properties.EditFormat.FormatString = "g";
            this.sontarih.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.sontarih.Properties.Mask.EditMask = "g";
            this.sontarih.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sontarih.Size = new System.Drawing.Size(134, 25);
            this.sontarih.TabIndex = 4;
            // 
            // ilktarih
            // 
            this.ilktarih.EditValue = new System.DateTime(2012, 3, 5, 23, 30, 3, 0);
            this.ilktarih.Location = new System.Drawing.Point(658, 23);
            this.ilktarih.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ilktarih.Name = "ilktarih";
            this.ilktarih.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ilktarih.Properties.Appearance.Options.UseFont = true;
            this.ilktarih.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ilktarih.Properties.DisplayFormat.FormatString = "g";
            this.ilktarih.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ilktarih.Properties.EditFormat.FormatString = "g";
            this.ilktarih.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ilktarih.Properties.Mask.EditMask = "g";
            this.ilktarih.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.ilktarih.Size = new System.Drawing.Size(168, 25);
            this.ilktarih.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 7);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 16);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Stoklar";
            // 
            // splitterControl1
            // 
            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterControl1.Location = new System.Drawing.Point(0, 299);
            this.splitterControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(1098, 5);
            this.splitterControl1.TabIndex = 6;
            this.splitterControl1.TabStop = false;
            // 
            // frmStokSatisGrafigi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 667);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.chartControl1);
            this.Controls.Add(this.pivotGridControl1);
            this.Controls.Add(this.panelControl3);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmStokSatisGrafigi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ucStokSatisGrafigi_Load);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbGosterimSekli.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStoklar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTarihAraligi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sontarih.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sontarih.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilktarih.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilktarih.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.XtraPivotGrid.PivotGridControl pivotGridControl1;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField1;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField2;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField3;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cbTarihAraligi;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.DateEdit sontarih;
        private DevExpress.XtraEditors.DateEdit ilktarih;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.LookUpEdit lueStoklar;
        private DevExpress.XtraEditors.ComboBoxEdit cbGosterimSekli;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}

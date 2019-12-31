namespace GPTS
{
    partial class frmFormAyarlari
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
            this.gridControl5 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn46 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn47 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.sebit = new DevExpress.XtraEditors.SpinEdit();
            this.sesaat = new DevExpress.XtraEditors.SpinEdit();
            this.sebas = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sebit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sesaat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sebas.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl5
            // 
            this.gridControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl5.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            gridLevelNode1.RelationName = "Level1";
            this.gridControl5.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl5.Location = new System.Drawing.Point(0, 278);
            this.gridControl5.MainView = this.gridView1;
            this.gridControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl5.Name = "gridControl5";
            this.gridControl5.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit7,
            this.repositoryItemCheckEdit8});
            this.gridControl5.Size = new System.Drawing.Size(350, 224);
            this.gridControl5.TabIndex = 18;
            this.gridControl5.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl5.Visible = false;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn46,
            this.gridColumn1,
            this.gridColumn47});
            this.gridView1.GridControl = this.gridControl5;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn46
            // 
            this.gridColumn46.Caption = "pkAyarlar";
            this.gridColumn46.FieldName = "pkAyarlar";
            this.gridColumn46.Name = "gridColumn46";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Açıklama";
            this.gridColumn1.FieldName = "Ayar20";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 178;
            // 
            // gridColumn47
            // 
            this.gridColumn47.Caption = "Değer";
            this.gridColumn47.FieldName = "Ayar50";
            this.gridColumn47.Name = "gridColumn47";
            this.gridColumn47.OptionsColumn.AllowEdit = false;
            this.gridColumn47.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn47.Visible = true;
            this.gridColumn47.VisibleIndex = 0;
            this.gridColumn47.Width = 154;
            // 
            // repositoryItemCheckEdit7
            // 
            this.repositoryItemCheckEdit7.AutoHeight = false;
            this.repositoryItemCheckEdit7.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.repositoryItemCheckEdit7.Name = "repositoryItemCheckEdit7";
            // 
            // repositoryItemCheckEdit8
            // 
            this.repositoryItemCheckEdit8.AutoHeight = false;
            this.repositoryItemCheckEdit8.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style4;
            this.repositoryItemCheckEdit8.Name = "repositoryItemCheckEdit8";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(350, 69);
            this.panelControl1.TabIndex = 19;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.save;
            this.BtnKaydet.Location = new System.Drawing.Point(74, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(131, 65);
            this.BtnKaydet.TabIndex = 5;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton21.Location = new System.Drawing.Point(205, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(143, 65);
            this.simpleButton21.TabIndex = 6;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.simpleButton1);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.labelControl7);
            this.panel1.Controls.Add(this.sebit);
            this.panel1.Controls.Add(this.sesaat);
            this.panel1.Controls.Add(this.sebas);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 209);
            this.panel1.TabIndex = 20;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(12, 181);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(85, 20);
            this.simpleButton1.TabIndex = 669;
            this.simpleButton1.Text = "Varsayılan";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(126, 50);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(109, 16);
            this.labelControl2.TabIndex = 667;
            this.labelControl2.Text = "Randevu Bitiş Saati";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(116, 80);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(119, 16);
            this.labelControl1.TabIndex = 667;
            this.labelControl1.Text = "Randevu Saat Aralığı";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(96, 20);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(139, 16);
            this.labelControl7.TabIndex = 667;
            this.labelControl7.Text = "Randevu Başlanğıç Saati";
            // 
            // sebit
            // 
            this.sebit.EditValue = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.sebit.EnterMoveNextControl = true;
            this.sebit.Location = new System.Drawing.Point(252, 47);
            this.sebit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sebit.Name = "sebit";
            this.sebit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sebit.Properties.MaxValue = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.sebit.Properties.MinValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.sebit.Size = new System.Drawing.Size(68, 22);
            this.sebit.TabIndex = 668;
            this.sebit.TabStop = false;
            // 
            // sesaat
            // 
            this.sesaat.EditValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.sesaat.EnterMoveNextControl = true;
            this.sesaat.Location = new System.Drawing.Point(252, 77);
            this.sesaat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sesaat.Name = "sesaat";
            this.sesaat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sesaat.Properties.MaxValue = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.sesaat.Properties.MinValue = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.sesaat.Size = new System.Drawing.Size(68, 22);
            this.sesaat.TabIndex = 668;
            this.sesaat.TabStop = false;
            // 
            // sebas
            // 
            this.sebas.EditValue = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.sebas.EnterMoveNextControl = true;
            this.sebas.Location = new System.Drawing.Point(252, 17);
            this.sebas.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sebas.Name = "sebas";
            this.sebas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sebas.Properties.MaxValue = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.sebas.Properties.MinValue = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.sebas.Size = new System.Drawing.Size(68, 22);
            this.sebas.TabIndex = 668;
            this.sebas.TabStop = false;
            // 
            // frmFormAyarlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 502);
            this.Controls.Add(this.gridControl5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmFormAyarlari";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form Ayarları";
            this.Load += new System.EventHandler(this.frmSayfaAyarlari_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sebit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sesaat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sebas.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl5;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn47;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn46;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit7;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SpinEdit sebas;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit sebit;
        private DevExpress.XtraEditors.SpinEdit sesaat;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;

    }
}
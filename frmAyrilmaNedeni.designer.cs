namespace GPTS
{
    partial class frmAyrilmaNedeni
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
            this.ayrilmanedeni = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pkpersoneller = new DevExpress.XtraEditors.TextEdit();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl28 = new DevExpress.XtraEditors.LabelControl();
            this.deAyrilisTarihi = new DevExpress.XtraEditors.DateEdit();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.label1 = new System.Windows.Forms.Label();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.isegiristarih = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ayrilmanedeni.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pkpersoneller.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deAyrilisTarihi.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deAyrilisTarihi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ayrilmanedeni
            // 
            this.ayrilmanedeni.Location = new System.Drawing.Point(173, 162);
            this.ayrilmanedeni.Margin = new System.Windows.Forms.Padding(4);
            this.ayrilmanedeni.Name = "ayrilmanedeni";
            this.ayrilmanedeni.Size = new System.Drawing.Size(300, 22);
            this.ayrilmanedeni.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 166);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(144, 16);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Ayrılma Nedenini Giriniz :";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.pkpersoneller);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(571, 56);
            this.panelControl1.TabIndex = 2;
            // 
            // pkpersoneller
            // 
            this.pkpersoneller.Location = new System.Drawing.Point(16, 22);
            this.pkpersoneller.Margin = new System.Windows.Forms.Padding(4);
            this.pkpersoneller.Name = "pkpersoneller";
            this.pkpersoneller.Properties.ReadOnly = true;
            this.pkpersoneller.Size = new System.Drawing.Size(133, 22);
            this.pkpersoneller.TabIndex = 61;
            this.pkpersoneller.Visible = false;
            this.pkpersoneller.EditValueChanged += new System.EventHandler(this.pkpersoneller_EditValueChanged);
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Location = new System.Drawing.Point(256, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(149, 52);
            this.BtnKaydet.TabIndex = 3;
            this.BtnKaydet.Text = "Tamam";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Location = new System.Drawing.Point(405, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(164, 52);
            this.simpleButton21.TabIndex = 4;
            this.simpleButton21.Text = "Vazgeç";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // labelControl28
            // 
            this.labelControl28.Location = new System.Drawing.Point(48, 130);
            this.labelControl28.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl28.Name = "labelControl28";
            this.labelControl28.Size = new System.Drawing.Size(108, 16);
            this.labelControl28.TabIndex = 59;
            this.labelControl28.Text = "İşten AyrılışTarihi :";
            // 
            // deAyrilisTarihi
            // 
            this.deAyrilisTarihi.EditValue = null;
            this.deAyrilisTarihi.Location = new System.Drawing.Point(175, 127);
            this.deAyrilisTarihi.Margin = new System.Windows.Forms.Padding(4);
            this.deAyrilisTarihi.Name = "deAyrilisTarihi";
            this.deAyrilisTarihi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deAyrilisTarihi.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deAyrilisTarihi.Size = new System.Drawing.Size(207, 22);
            this.deAyrilisTarihi.TabIndex = 1;
            // 
            // groupControl6
            // 
            this.groupControl6.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl6.AppearanceCaption.Options.UseFont = true;
            this.groupControl6.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl6.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl6.Controls.Add(this.label1);
            this.groupControl6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl6.Location = new System.Drawing.Point(0, 56);
            this.groupControl6.Margin = new System.Windows.Forms.Padding(4);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.ShowCaption = false;
            this.groupControl6.Size = new System.Drawing.Size(571, 37);
            this.groupControl6.TabIndex = 60;
            this.groupControl6.Text = "Müşteri Listesi";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold);
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(567, 33);
            this.label1.TabIndex = 23;
            this.label1.Text = "Personel Adı Soyadı";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl2.Location = new System.Drawing.Point(0, 205);
            this.gridControl2.MainView = this.gridView1;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl2.Size = new System.Drawing.Size(571, 293);
            this.gridControl2.TabIndex = 61;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn7,
            this.isegiristarih,
            this.gridColumn1});
            this.gridView1.GridControl = this.gridControl2;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridView1.OptionsView.EnableAppearanceOddRow = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "Personel No";
            this.gridColumn6.FieldName = "fkPersoneller";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Width = 53;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn7.AppearanceHeader.Options.UseFont = true;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "ayrilma_nedeni";
            this.gridColumn7.FieldName = "ayrilma_nedeni";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 220;
            // 
            // isegiristarih
            // 
            this.isegiristarih.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.isegiristarih.AppearanceHeader.Options.UseFont = true;
            this.isegiristarih.AppearanceHeader.Options.UseTextOptions = true;
            this.isegiristarih.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.isegiristarih.Caption = "İşten AyrılışTarihi";
            this.isegiristarih.FieldName = "ayrilma_tarihi";
            this.isegiristarih.Name = "isegiristarih";
            this.isegiristarih.OptionsColumn.AllowEdit = false;
            this.isegiristarih.Visible = true;
            this.isegiristarih.VisibleIndex = 1;
            this.isegiristarih.Width = 147;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.DisplayValueChecked = "True";
            this.repositoryItemCheckEdit1.DisplayValueGrayed = "false";
            this.repositoryItemCheckEdit1.DisplayValueUnchecked = "false";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit1.NullText = "false";
            this.repositoryItemCheckEdit1.ValueGrayed = "false";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "kayit_tarihi";
            this.gridColumn1.FieldName = "kayit_tarihi";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 133;
            // 
            // frmAyrilmaNedeni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 498);
            this.Controls.Add(this.gridControl2);
            this.Controls.Add(this.labelControl28);
            this.Controls.Add(this.deAyrilisTarihi);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.ayrilmanedeni);
            this.Controls.Add(this.groupControl6);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAyrilmaNedeni";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Personel İşten Ayrılma Bilgileri";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ayrilmanedeni.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pkpersoneller.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deAyrilisTarihi.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deAyrilisTarihi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        public DevExpress.XtraEditors.TextEdit ayrilmanedeni;
        private DevExpress.XtraEditors.LabelControl labelControl28;
        private DevExpress.XtraEditors.DateEdit deAyrilisTarihi;
        public DevExpress.XtraEditors.TextEdit pkpersoneller;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private System.Windows.Forms.Label label1;
        public DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn isegiristarih;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}


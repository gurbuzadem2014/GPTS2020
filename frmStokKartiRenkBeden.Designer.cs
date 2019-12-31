namespace GPTS
{
    partial class frmStokKartiRenkBeden
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip16 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem9 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem16 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip14 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem14 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip17 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem8 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem17 = new DevExpress.Utils.ToolTipItem();
            this.gridControl4 = new DevExpress.XtraGrid.GridControl();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn36 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lueRenk = new DevExpress.XtraEditors.LookUpEdit();
            this.lueBeden = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.seAdet = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl45 = new DevExpress.XtraEditors.LabelControl();
            this.btnEkle = new DevExpress.XtraEditors.SimpleButton();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueRenk.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueBeden.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seAdet.Properties)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl4
            // 
            this.gridControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl4.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.gridControl4.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl4.Location = new System.Drawing.Point(0, 46);
            this.gridControl4.MainView = this.gridView4;
            this.gridControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl4.Name = "gridControl4";
            this.gridControl4.Size = new System.Drawing.Size(964, 315);
            this.gridControl4.TabIndex = 39;
            this.gridControl4.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            // 
            // gridView4
            // 
            this.gridView4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn36,
            this.gridColumn16,
            this.gridColumn22,
            this.gridColumn10,
            this.gridColumn1,
            this.gridColumn2});
            this.gridView4.GridControl = this.gridControl4;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView4.OptionsView.ShowFooter = true;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            this.gridView4.OptionsView.ShowIndicator = false;
            this.gridView4.OptionsView.ShowViewCaption = true;
            this.gridView4.ViewCaption = "Alış Detay Renk ve Beden Adetleri";
            // 
            // gridColumn36
            // 
            this.gridColumn36.Caption = "pkAlisDetayRB";
            this.gridColumn36.FieldName = "pkAlisDetayRB";
            this.gridColumn36.Name = "gridColumn36";
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceHeader.Options.UseFont = true;
            this.gridColumn16.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.Caption = "fkRenk";
            this.gridColumn16.FieldName = "fkRenk";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.ToolTip = "Stoklarınızın daha önce kimden, ne zaman ve hangi fiyatlara alındığını gösterir";
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 1;
            this.gridColumn16.Width = 230;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "Adet";
            this.gridColumn22.FieldName = "Adet";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 3;
            this.gridColumn22.Width = 231;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "fkAlisDetay";
            this.gridColumn10.FieldName = "fkAlisDetay";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsColumn.AllowFocus = false;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 190;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "fkBeden";
            this.gridColumn1.FieldName = "fkBeden";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            this.gridColumn1.Width = 192;
            // 
            // lueRenk
            // 
            this.lueRenk.EnterMoveNextControl = true;
            this.lueRenk.Location = new System.Drawing.Point(91, 4);
            this.lueRenk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueRenk.Name = "lueRenk";
            this.lueRenk.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lueRenk.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.lueRenk.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueRenk.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkRenkGrupKodu", "pkRenkGrupKodu", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Aciklama", "Aciklama")});
            this.lueRenk.Properties.DisplayMember = "Aciklama";
            this.lueRenk.Properties.NullText = "Seçiniz...";
            this.lueRenk.Properties.ShowHeader = false;
            this.lueRenk.Properties.ValueMember = "pkRenkGrupKodu";
            this.lueRenk.Size = new System.Drawing.Size(232, 22);
            this.lueRenk.TabIndex = 40;
            // 
            // lueBeden
            // 
            this.lueBeden.EnterMoveNextControl = true;
            this.lueBeden.Location = new System.Drawing.Point(424, 4);
            this.lueBeden.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueBeden.Name = "lueBeden";
            this.lueBeden.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lueBeden.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.lueBeden.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueBeden.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkBedenGrupKodu", "pkBedenGrupKodu", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Aciklama", "Aciklama")});
            this.lueBeden.Properties.DisplayMember = "Aciklama";
            this.lueBeden.Properties.NullText = "Seçiniz...";
            this.lueBeden.Properties.ShowHeader = false;
            this.lueBeden.Properties.ValueMember = "pkBedenGrupKodu";
            this.lueBeden.Size = new System.Drawing.Size(232, 22);
            this.lueBeden.TabIndex = 41;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Location = new System.Drawing.Point(8, 6);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(35, 17);
            this.labelControl3.TabIndex = 42;
            this.labelControl3.Text = "Renk";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(341, 6);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(44, 17);
            this.labelControl1.TabIndex = 43;
            this.labelControl1.Text = "Beden";
            // 
            // seAdet
            // 
            this.seAdet.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seAdet.Location = new System.Drawing.Point(718, 4);
            this.seAdet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.seAdet.Name = "seAdet";
            this.seAdet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.seAdet.Size = new System.Drawing.Size(89, 22);
            toolTipTitleItem9.Text = "Genel Stok Durumu";
            toolTipItem16.LeftIndent = 6;
            toolTipItem16.Text = "Genel Stok Durum Mevcudu";
            superToolTip16.Items.Add(toolTipTitleItem9);
            superToolTip16.Items.Add(toolTipItem16);
            this.seAdet.SuperTip = superToolTip16;
            this.seAdet.TabIndex = 85;
            this.seAdet.Tag = "-1";
            this.seAdet.ToolTip = "Tag Önceki Stok Mevcudu";
            // 
            // labelControl45
            // 
            this.labelControl45.Location = new System.Drawing.Point(669, 5);
            this.labelControl45.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl45.Name = "labelControl45";
            this.labelControl45.Size = new System.Drawing.Size(26, 16);
            toolTipItem14.Text = "Bu ürünü kayıt yaparken elinizdeki miktarı girebilirsiniz";
            superToolTip14.Items.Add(toolTipItem14);
            this.labelControl45.SuperTip = superToolTip14;
            this.labelControl45.TabIndex = 86;
            this.labelControl45.Text = "Adet";
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(833, 4);
            this.btnEkle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(77, 27);
            toolTipTitleItem8.Text = "Stok Mevcutları";
            toolTipItem17.LeftIndent = 6;
            toolTipItem17.Text = "Stok mevcutlarını düzenlemek için kullanılır.";
            superToolTip17.Items.Add(toolTipTitleItem8);
            superToolTip17.Items.Add(toolTipItem17);
            this.btnEkle.SuperTip = superToolTip17;
            this.btnEkle.TabIndex = 2252;
            this.btnEkle.TabStop = false;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Mevcut";
            this.gridColumn2.FieldName = "Mevcut";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            this.gridColumn2.Width = 119;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnEkle);
            this.panel4.Controls.Add(this.lueRenk);
            this.panel4.Controls.Add(this.lueBeden);
            this.panel4.Controls.Add(this.seAdet);
            this.panel4.Controls.Add(this.labelControl3);
            this.panel4.Controls.Add(this.labelControl45);
            this.panel4.Controls.Add(this.labelControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(964, 46);
            this.panel4.TabIndex = 2253;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 407);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(964, 287);
            this.panel1.TabIndex = 2254;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 361);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(964, 46);
            this.panel2.TabIndex = 2255;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(964, 287);
            this.gridControl1.TabIndex = 40;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.OptionsView.ShowViewCaption = true;
            this.gridView1.ViewCaption = "Renk ve Beden Depo Mevcutlerı";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "pkAlisDetayRB";
            this.gridColumn3.FieldName = "pkAlisDetayRB";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "fkRenk";
            this.gridColumn4.FieldName = "fkRenk";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.ToolTip = "Stoklarınızın daha önce kimden, ne zaman ve hangi fiyatlara alındığını gösterir";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 230;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "depo mevcut";
            this.gridColumn5.FieldName = "depomevcut";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 231;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "pkStokKartiRBDepo";
            this.gridColumn6.FieldName = "pkStokKartiRBDepo";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
            this.gridColumn6.Width = 190;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "fkBeden";
            this.gridColumn7.FieldName = "fkBeden";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 192;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "rb Mevcut";
            this.gridColumn8.FieldName = "rbMevcut";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 4;
            this.gridColumn8.Width = 119;
            // 
            // frmStokKartiRenkBeden
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 694);
            this.Controls.Add(this.gridControl4);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmStokKartiRenkBeden";
            this.Text = "Stok Karti Renk Beden";
            this.Load += new System.EventHandler(this.frmStokKartiRenkBeden_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueRenk.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueBeden.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seAdet.Properties)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl4;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn36;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn22;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.LookUpEdit lueRenk;
        private DevExpress.XtraEditors.LookUpEdit lueBeden;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit seAdet;
        private DevExpress.XtraEditors.LabelControl labelControl45;
        private DevExpress.XtraEditors.SimpleButton btnEkle;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
    }
}
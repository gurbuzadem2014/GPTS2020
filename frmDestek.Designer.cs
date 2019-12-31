namespace GPTS
{
    partial class frmDestek
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
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.fkKullanicilar = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testEdilecekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tamamlandıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.çalışılıyorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beklemedeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.silToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.öncelikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.kullanıcılarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meMesaj = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbKonu = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbTumu = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cbBirimi = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtEposta = new DevExpress.XtraEditors.TextEdit();
            this.alisdangeldi = new DevExpress.XtraEditors.LabelControl();
            this.gcDestek = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcDestekDetay = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.meMesaj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbKonu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTumu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBirimi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEposta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDestek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDestekDetay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.mail_send;
            this.BtnKaydet.Location = new System.Drawing.Point(826, 96);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(180, 70);
            this.BtnKaydet.TabIndex = 90;
            this.BtnKaydet.Text = "Yeni Cağrı Kaydı Gönder";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // fkKullanicilar
            // 
            this.fkKullanicilar.AutoSize = true;
            this.fkKullanicilar.Location = new System.Drawing.Point(447, 28);
            this.fkKullanicilar.Name = "fkKullanicilar";
            this.fkKullanicilar.Size = new System.Drawing.Size(16, 17);
            this.fkKullanicilar.TabIndex = 91;
            this.fkKullanicilar.Text = "0";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testEdilecekToolStripMenuItem,
            this.tamamlandıToolStripMenuItem,
            this.çalışılıyorToolStripMenuItem,
            this.beklemedeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.silToolStripMenuItem,
            this.öncelikToolStripMenuItem,
            this.kullanıcılarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(164, 178);
            // 
            // testEdilecekToolStripMenuItem
            // 
            this.testEdilecekToolStripMenuItem.Name = "testEdilecekToolStripMenuItem";
            this.testEdilecekToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.testEdilecekToolStripMenuItem.Text = "Test Edilecek";
            this.testEdilecekToolStripMenuItem.Click += new System.EventHandler(this.testEdilecekToolStripMenuItem_Click);
            // 
            // tamamlandıToolStripMenuItem
            // 
            this.tamamlandıToolStripMenuItem.Name = "tamamlandıToolStripMenuItem";
            this.tamamlandıToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.tamamlandıToolStripMenuItem.Text = "Tamamlandı.";
            this.tamamlandıToolStripMenuItem.Click += new System.EventHandler(this.tamamlandıToolStripMenuItem_Click);
            // 
            // çalışılıyorToolStripMenuItem
            // 
            this.çalışılıyorToolStripMenuItem.Name = "çalışılıyorToolStripMenuItem";
            this.çalışılıyorToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.çalışılıyorToolStripMenuItem.Text = "Çalışılıyor...";
            this.çalışılıyorToolStripMenuItem.Click += new System.EventHandler(this.çalışılıyorToolStripMenuItem_Click);
            // 
            // beklemedeToolStripMenuItem
            // 
            this.beklemedeToolStripMenuItem.Name = "beklemedeToolStripMenuItem";
            this.beklemedeToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.beklemedeToolStripMenuItem.Text = "Beklemede";
            this.beklemedeToolStripMenuItem.Click += new System.EventHandler(this.beklemedeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(160, 6);
            // 
            // silToolStripMenuItem
            // 
            this.silToolStripMenuItem.Name = "silToolStripMenuItem";
            this.silToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.silToolStripMenuItem.Text = "Sil";
            this.silToolStripMenuItem.Click += new System.EventHandler(this.silToolStripMenuItem_Click);
            // 
            // öncelikToolStripMenuItem
            // 
            this.öncelikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalToolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.öncelikToolStripMenuItem.Name = "öncelikToolStripMenuItem";
            this.öncelikToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.öncelikToolStripMenuItem.Text = "Öncelik";
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(92, 26);
            this.normalToolStripMenuItem.Text = "1";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(92, 26);
            this.toolStripMenuItem3.Text = "2";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(92, 26);
            this.toolStripMenuItem4.Text = "3";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(92, 26);
            this.toolStripMenuItem5.Text = "4";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(92, 26);
            this.toolStripMenuItem6.Text = "5";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // kullanıcılarToolStripMenuItem
            // 
            this.kullanıcılarToolStripMenuItem.Name = "kullanıcılarToolStripMenuItem";
            this.kullanıcılarToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.kullanıcılarToolStripMenuItem.Text = "Kullanıcılar";
            this.kullanıcılarToolStripMenuItem.Click += new System.EventHandler(this.kullanıcılarToolStripMenuItem_Click);
            // 
            // meMesaj
            // 
            this.meMesaj.EditValue = "";
            this.meMesaj.Location = new System.Drawing.Point(135, 94);
            this.meMesaj.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.meMesaj.Name = "meMesaj";
            this.meMesaj.Size = new System.Drawing.Size(686, 80);
            this.meMesaj.TabIndex = 64;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.cbKonu);
            this.panelControl1.Controls.Add(this.cbTumu);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.meMesaj);
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.fkKullanicilar);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.cbBirimi);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.txtEposta);
            this.panelControl1.Controls.Add(this.alisdangeldi);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1035, 207);
            this.panelControl1.TabIndex = 65;
            // 
            // cbKonu
            // 
            this.cbKonu.EditValue = "";
            this.cbKonu.EnterMoveNextControl = true;
            this.cbKonu.Location = new System.Drawing.Point(140, 60);
            this.cbKonu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbKonu.Name = "cbKonu";
            this.cbKonu.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbKonu.Properties.Appearance.Options.UseFont = true;
            this.cbKonu.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cbKonu.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cbKonu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbKonu.Properties.Items.AddRange(new object[] {
            "DESTEK VERİLDİ",
            "DESTEK VERİLDİ VE GÜNCELLEME YAPILDI",
            "GÜNCELLEME YAPILDI",
            "MSSQL YENİDEN KURULDU",
            "YENİ SİSTEM KURULDU",
            "EĞİTİM VERİLDİ",
            "YENİ SİSTEM KURULDU VE EĞİTİM VERİLDİ"});
            this.cbKonu.Size = new System.Drawing.Size(300, 27);
            this.cbKonu.TabIndex = 95;
            this.cbKonu.Tag = "";
            // 
            // cbTumu
            // 
            this.cbTumu.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbTumu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbTumu.Location = new System.Drawing.Point(16, 177);
            this.cbTumu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbTumu.Name = "cbTumu";
            this.cbTumu.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.cbTumu.Properties.Appearance.Options.UseBackColor = true;
            this.cbTumu.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cbTumu.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cbTumu.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cbTumu.Properties.Caption = "Tümünü Göster";
            this.cbTumu.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.cbTumu.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbTumu.Size = new System.Drawing.Size(119, 24);
            this.cbTumu.TabIndex = 94;
            this.cbTumu.TabStop = false;
            // 
            // labelControl3
            // 
            this.labelControl3.AccessibleDescription = "";
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelControl3.Location = new System.Drawing.Point(35, 111);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(94, 51);
            this.labelControl3.TabIndex = 93;
            this.labelControl3.Text = "İstek Öneri \r\nve \r\nDestek Mesajı\r\n";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = global::GPTS.Properties.Resources.iletisim_eposta_32x32;
            this.simpleButton3.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton3.Location = new System.Drawing.Point(0, 0);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(48, 50);
            this.simpleButton3.TabIndex = 92;
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.AccessibleDescription = "alisdangeldihayir";
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelControl2.Location = new System.Drawing.Point(671, 62);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(49, 17);
            this.labelControl2.TabIndex = 28;
            this.labelControl2.Text = "Öncelik";
            this.labelControl2.Click += new System.EventHandler(this.labelControl2_Click);
            // 
            // cbBirimi
            // 
            this.cbBirimi.EditValue = "1";
            this.cbBirimi.EnterMoveNextControl = true;
            this.cbBirimi.Location = new System.Drawing.Point(726, 58);
            this.cbBirimi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbBirimi.Name = "cbBirimi";
            this.cbBirimi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cbBirimi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cbBirimi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbBirimi.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbBirimi.Size = new System.Drawing.Size(96, 22);
            this.cbBirimi.TabIndex = 27;
            this.cbBirimi.Tag = "1";
            // 
            // labelControl1
            // 
            this.labelControl1.AccessibleDescription = "alisdangeldihayir";
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.labelControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelControl1.Location = new System.Drawing.Point(96, 64);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(37, 17);
            this.labelControl1.TabIndex = 26;
            this.labelControl1.Text = "Konu";
            // 
            // txtEposta
            // 
            this.txtEposta.Location = new System.Drawing.Point(135, 22);
            this.txtEposta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEposta.Name = "txtEposta";
            this.txtEposta.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtEposta.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.txtEposta.Properties.Appearance.Options.UseBackColor = true;
            this.txtEposta.Properties.Appearance.Options.UseFont = true;
            this.txtEposta.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtEposta.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtEposta.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtEposta.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtEposta.Size = new System.Drawing.Size(304, 27);
            this.txtEposta.TabIndex = 23;
            this.txtEposta.Tag = "0";
            // 
            // alisdangeldi
            // 
            this.alisdangeldi.AccessibleDescription = "alisdangeldihayir";
            this.alisdangeldi.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.alisdangeldi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.alisdangeldi.Location = new System.Drawing.Point(78, 26);
            this.alisdangeldi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.alisdangeldi.Name = "alisdangeldi";
            this.alisdangeldi.Size = new System.Drawing.Size(54, 17);
            this.alisdangeldi.TabIndex = 24;
            this.alisdangeldi.Text = "E-Posta";
            // 
            // gcDestek
            // 
            this.gcDestek.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gcDestek.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcDestek.Location = new System.Drawing.Point(0, 214);
            this.gcDestek.MainView = this.gridView1;
            this.gcDestek.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcDestek.Name = "gcDestek";
            this.gcDestek.Size = new System.Drawing.Size(1035, 271);
            this.gcDestek.TabIndex = 66;
            this.gcDestek.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13});
            this.gridView1.GridControl = this.gcDestek;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.OptionsView.ShowViewCaption = true;
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn3, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gridView1.ViewCaption = "İstek ve Destek Talepleri";
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Destek No";
            this.gridColumn1.FieldName = "pkDestek";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 62;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Konu";
            this.gridColumn2.FieldName = "konu";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 184;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Mesaj";
            this.gridColumn4.FieldName = "mesaj";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 293;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Tarih";
            this.gridColumn3.DisplayFormat.FormatString = "g";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn3.FieldName = "tarih";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 112;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Durumu";
            this.gridColumn5.FieldName = "durumu";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Width = 43;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "Sonuç";
            this.gridColumn6.FieldName = "sonuc";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 165;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.Caption = "E-Posta";
            this.gridColumn10.FieldName = "eposta";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 5;
            this.gridColumn10.Width = 142;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Öncelik";
            this.gridColumn11.FieldName = "oncelik";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Width = 41;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.Caption = "Kullanici ID";
            this.gridColumn12.FieldName = "fkKullanicilar";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 6;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "Kiralik";
            this.gridColumn13.FieldName = "Kiralik";
            this.gridColumn13.Name = "gridColumn13";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcDestekDetay);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 485);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1035, 321);
            this.panelControl2.TabIndex = 69;
            // 
            // gcDestekDetay
            // 
            this.gcDestekDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDestekDetay.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcDestekDetay.Location = new System.Drawing.Point(2, 97);
            this.gcDestekDetay.MainView = this.gridView2;
            this.gcDestekDetay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcDestekDetay.Name = "gcDestekDetay";
            this.gcDestekDetay.Size = new System.Drawing.Size(1031, 222);
            this.gcDestekDetay.TabIndex = 67;
            this.gcDestekDetay.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
            this.gridColumn7,
            this.gridColumn8});
            this.gridView2.GridControl = this.gcDestekDetay;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowIndicator = false;
            this.gridView2.OptionsView.ShowViewCaption = true;
            this.gridView2.ViewCaption = "İstek ve Destek Talep Hareketleri";
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Tarih";
            this.gridColumn9.DisplayFormat.FormatString = "g";
            this.gridColumn9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn9.FieldName = "Tarih";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 0;
            this.gridColumn9.Width = 140;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "pkDestekDetay";
            this.gridColumn7.FieldName = "pkDestekDetay";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Mesaj";
            this.gridColumn8.FieldName = "Mesaj";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 1;
            this.gridColumn8.Width = 741;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.memoEdit1);
            this.panelControl3.Controls.Add(this.simpleButton2);
            this.panelControl3.Controls.Add(this.simpleButton1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1031, 95);
            this.panelControl3.TabIndex = 70;
            // 
            // memoEdit1
            // 
            this.memoEdit1.EditValue = "";
            this.memoEdit1.Location = new System.Drawing.Point(6, 6);
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(834, 84);
            this.memoEdit1.TabIndex = 69;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Enabled = false;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.Delete_32x32;
            this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton2.Location = new System.Drawing.Point(934, 6);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 84);
            this.simpleButton2.TabIndex = 91;
            this.simpleButton2.Text = "Seçileni Sil";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = global::GPTS.Properties.Resources.mail_send;
            this.simpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton1.Location = new System.Drawing.Point(853, 4);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 84);
            this.simpleButton1.TabIndex = 90;
            this.simpleButton1.Text = "Cevap\r\nGönder";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // frmDestek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 806);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.gcDestek);
            this.Controls.Add(this.panelControl2);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDestek";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Çağrı Merkezi Destek";
            this.Load += new System.EventHandler(this.frmDestek_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmDestek_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmDestek_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmDestek_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.meMesaj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbKonu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTumu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBirimi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEposta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDestek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDestekDetay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        public DevExpress.XtraEditors.MemoEdit meMesaj;
        public DevExpress.XtraEditors.PanelControl panelControl1;
        public DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.TextEdit txtEposta;
        public DevExpress.XtraEditors.LabelControl alisdangeldi;
        private DevExpress.XtraEditors.ComboBoxEdit cbBirimi;
        private DevExpress.XtraGrid.GridControl gcDestek;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        public DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private System.Windows.Forms.Label fkKullanicilar;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tamamlandıToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem çalışılıyorToolStripMenuItem;
        public DevExpress.XtraEditors.PanelControl panelControl2;
        public DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraGrid.GridControl gcDestekDetay;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        public DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private System.Windows.Forms.ToolStripMenuItem beklemedeToolStripMenuItem;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        public DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem silToolStripMenuItem;
        private DevExpress.XtraEditors.CheckEdit cbTumu;
        private System.Windows.Forms.ToolStripMenuItem testEdilecekToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private System.Windows.Forms.ToolStripMenuItem öncelikToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private System.Windows.Forms.ToolStripMenuItem kullanıcılarToolStripMenuItem;
        private DevExpress.XtraEditors.ComboBoxEdit cbKonu;
    }
}
namespace GPTS
{
    partial class frmbankalar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmbankalar));
            this.Aktif = new DevExpress.XtraEditors.CheckEdit();
            this.BankaAdi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.txtSube = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lSube = new DevExpress.XtraEditors.LabelControl();
            this.lueSubeler = new DevExpress.XtraEditors.LookUpEdit();
            this.cbKartTuru = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.Aktif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankaAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSube.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSubeler.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbKartTuru.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Aktif
            // 
            this.Aktif.EditValue = true;
            this.Aktif.Location = new System.Drawing.Point(382, 103);
            this.Aktif.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Aktif.Name = "Aktif";
            this.Aktif.Properties.Caption = "Aktif";
            this.Aktif.Size = new System.Drawing.Size(61, 21);
            this.Aktif.TabIndex = 40;
            // 
            // BankaAdi
            // 
            this.BankaAdi.Location = new System.Drawing.Point(143, 148);
            this.BankaAdi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BankaAdi.Name = "BankaAdi";
            this.BankaAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BankaAdi.Properties.Appearance.Options.UseFont = true;
            this.BankaAdi.Size = new System.Drawing.Size(327, 27);
            this.BankaAdi.TabIndex = 35;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(49, 151);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(77, 16);
            this.labelControl6.TabIndex = 33;
            this.labelControl6.Text = "Banka Adı    :";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(479, 69);
            this.panelControl1.TabIndex = 41;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(2, 2);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(132, 65);
            this.simpleButton4.TabIndex = 86;
            this.simpleButton4.Text = "Yeni Banka \r\n[F7]";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click_1);
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.save;
            this.BtnKaydet.Location = new System.Drawing.Point(203, 2);
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
            this.simpleButton21.Location = new System.Drawing.Point(334, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(143, 65);
            this.simpleButton21.TabIndex = 6;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // txtSube
            // 
            this.txtSube.Location = new System.Drawing.Point(143, 193);
            this.txtSube.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSube.Name = "txtSube";
            this.txtSube.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSube.Properties.Appearance.Options.UseFont = true;
            this.txtSube.Size = new System.Drawing.Size(327, 27);
            this.txtSube.TabIndex = 35;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(3, 196);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(123, 16);
            this.labelControl3.TabIndex = 33;
            this.labelControl3.Text = "Bankanın Şube Adı   :";
            // 
            // lSube
            // 
            this.lSube.Cursor = System.Windows.Forms.Cursors.Default;
            this.lSube.Location = new System.Drawing.Point(53, 102);
            this.lSube.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lSube.Name = "lSube";
            this.lSube.Size = new System.Drawing.Size(73, 16);
            this.lSube.TabIndex = 148;
            this.lSube.Text = "Bağlı  Şube :";
            // 
            // lueSubeler
            // 
            this.lueSubeler.Location = new System.Drawing.Point(143, 99);
            this.lueSubeler.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueSubeler.Name = "lueSubeler";
            this.lueSubeler.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lueSubeler.Properties.Appearance.Options.UseFont = true;
            this.lueSubeler.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueSubeler.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkSube", "pkSube", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sube_adi", "Şube Adı")});
            this.lueSubeler.Properties.DisplayMember = "sube_adi";
            this.lueSubeler.Properties.NullText = "Seçiniz...";
            this.lueSubeler.Properties.ShowHeader = false;
            this.lueSubeler.Properties.ValueMember = "pkSube";
            this.lueSubeler.Size = new System.Drawing.Size(160, 27);
            this.lueSubeler.TabIndex = 149;
            // 
            // cbKartTuru
            // 
            this.cbKartTuru.EditValue = "Kredi Kartı";
            this.cbKartTuru.Location = new System.Drawing.Point(143, 236);
            this.cbKartTuru.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbKartTuru.Name = "cbKartTuru";
            this.cbKartTuru.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbKartTuru.Properties.Appearance.Options.UseFont = true;
            this.cbKartTuru.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cbKartTuru.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cbKartTuru.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.cbKartTuru.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbKartTuru.Properties.Items.AddRange(new object[] {
            "Kredi Kartı",
            "Banka",
            "Sodexo Ticket"});
            this.cbKartTuru.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbKartTuru.Size = new System.Drawing.Size(160, 29);
            this.cbKartTuru.TabIndex = 182;
            this.cbKartTuru.TabStop = false;
            this.cbKartTuru.ToolTip = "Ödeme tipinin seçilmesini sağlar";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(47, 243);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(79, 16);
            this.labelControl2.TabIndex = 33;
            this.labelControl2.Text = "Kart Turu     :";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(312, 244);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(82, 16);
            this.labelControl1.TabIndex = 33;
            this.labelControl1.Text = "(Ödeme Şekli)";
            // 
            // frmbankalar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 308);
            this.Controls.Add(this.cbKartTuru);
            this.Controls.Add(this.lSube);
            this.Controls.Add(this.lueSubeler);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.Aktif);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.txtSube);
            this.Controls.Add(this.BankaAdi);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmbankalar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Banka Kartı";
            this.Load += new System.EventHandler(this.frmbankalar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Aktif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankaAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSube.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSubeler.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbKartTuru.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.TextEdit BankaAdi;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.CheckEdit Aktif;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.TextEdit txtSube;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl lSube;
        private DevExpress.XtraEditors.LookUpEdit lueSubeler;
        private DevExpress.XtraEditors.ComboBoxEdit cbKartTuru;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
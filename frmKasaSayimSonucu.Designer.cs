namespace GPTS
{
    partial class frmKasaSayimSonucu
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnyazdir = new DevExpress.XtraEditors.SimpleButton();
            this.btnDevirBakiye = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.button3 = new System.Windows.Forms.Button();
            this.olmasigereken = new DevExpress.XtraEditors.CalcEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.kasadaki = new DevExpress.XtraEditors.CalcEdit();
            this.button2 = new System.Windows.Forms.Button();
            this.fark = new DevExpress.XtraEditors.CalcEdit();
            this.cbAktifHesap = new DevExpress.XtraEditors.CheckEdit();
            this.txtAciklama = new DevExpress.XtraEditors.TextEdit();
            this.deTarih = new DevExpress.XtraEditors.DateEdit();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.lueKasalar = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olmasigereken.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kasadaki.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAktifHesap.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTarih.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTarih.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueKasalar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.CaptionImage = global::GPTS.Properties.Resources.randevu1;
            this.groupControl1.Controls.Add(this.btnyazdir);
            this.groupControl1.Controls.Add(this.btnDevirBakiye);
            this.groupControl1.Controls.Add(this.simpleButton21);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(449, 97);
            this.groupControl1.TabIndex = 50;
            this.groupControl1.Text = "Kasa Sayım Sonucu";
            // 
            // btnyazdir
            // 
            this.btnyazdir.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnyazdir.Appearance.Options.UseFont = true;
            this.btnyazdir.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnyazdir.Image = global::GPTS.Properties.Resources.Preview_32x32;
            this.btnyazdir.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnyazdir.Location = new System.Drawing.Point(113, 43);
            this.btnyazdir.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnyazdir.Name = "btnyazdir";
            this.btnyazdir.Size = new System.Drawing.Size(114, 52);
            this.btnyazdir.TabIndex = 90;
            this.btnyazdir.Tag = "0";
            this.btnyazdir.Text = "Ön İzleme";
            this.btnyazdir.Visible = false;
            this.btnyazdir.Click += new System.EventHandler(this.btnyazdir_Click);
            // 
            // btnDevirBakiye
            // 
            this.btnDevirBakiye.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDevirBakiye.Image = global::GPTS.Properties.Resources.save;
            this.btnDevirBakiye.Location = new System.Drawing.Point(2, 43);
            this.btnDevirBakiye.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDevirBakiye.Name = "btnDevirBakiye";
            this.btnDevirBakiye.Size = new System.Drawing.Size(111, 52);
            this.btnDevirBakiye.TabIndex = 95;
            this.btnDevirBakiye.Text = "KAYDET";
            this.btnDevirBakiye.Click += new System.EventHandler(this.btnDevirBakiye_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(326, 43);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(121, 52);
            this.simpleButton21.TabIndex = 89;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button3.Location = new System.Drawing.Point(9, 43);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(147, 26);
            this.button3.TabIndex = 24;
            this.button3.Text = "Kasa Sayım Sonucu";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // olmasigereken
            // 
            this.olmasigereken.Location = new System.Drawing.Point(163, 42);
            this.olmasigereken.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.olmasigereken.Name = "olmasigereken";
            this.olmasigereken.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.olmasigereken.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.olmasigereken.Properties.Appearance.Options.UseFont = true;
            this.olmasigereken.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.olmasigereken.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.olmasigereken.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.olmasigereken.Size = new System.Drawing.Size(219, 29);
            this.olmasigereken.TabIndex = 19;
            this.olmasigereken.ToolTip = "Fatura altı iskontoyu\r\n% olarak yapar";
            this.olmasigereken.EditValueChanged += new System.EventHandler(this.olmasigereken_EditValueChanged);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 97);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(449, 277);
            this.panelControl1.TabIndex = 8;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.lueKasalar);
            this.panelControl3.Controls.Add(this.kasadaki);
            this.panelControl3.Controls.Add(this.button2);
            this.panelControl3.Controls.Add(this.fark);
            this.panelControl3.Controls.Add(this.cbAktifHesap);
            this.panelControl3.Controls.Add(this.txtAciklama);
            this.panelControl3.Controls.Add(this.deTarih);
            this.panelControl3.Controls.Add(this.button5);
            this.panelControl3.Controls.Add(this.button4);
            this.panelControl3.Controls.Add(this.button3);
            this.panelControl3.Controls.Add(this.button1);
            this.panelControl3.Controls.Add(this.olmasigereken);
            this.panelControl3.Location = new System.Drawing.Point(23, 19);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(403, 245);
            this.panelControl3.TabIndex = 4;
            // 
            // kasadaki
            // 
            this.kasadaki.Location = new System.Drawing.Point(8, 151);
            this.kasadaki.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.kasadaki.Name = "kasadaki";
            this.kasadaki.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.kasadaki.Properties.Appearance.BackColor = System.Drawing.Color.Blue;
            this.kasadaki.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.kasadaki.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.kasadaki.Properties.Appearance.Options.UseBackColor = true;
            this.kasadaki.Properties.Appearance.Options.UseFont = true;
            this.kasadaki.Properties.Appearance.Options.UseForeColor = true;
            this.kasadaki.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.kasadaki.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.kasadaki.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.kasadaki.Properties.DisplayFormat.FormatString = "{0:n2}";
            this.kasadaki.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.kasadaki.Properties.ReadOnly = true;
            this.kasadaki.Size = new System.Drawing.Size(147, 31);
            this.kasadaki.TabIndex = 24;
            this.kasadaki.Visible = false;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(10, 79);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 26);
            this.button2.TabIndex = 101;
            this.button2.Text = "Fark";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // fark
            // 
            this.fark.Location = new System.Drawing.Point(163, 79);
            this.fark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fark.Name = "fark";
            this.fark.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.fark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.fark.Properties.Appearance.Options.UseFont = true;
            this.fark.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.fark.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.fark.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.fark.Properties.ReadOnly = true;
            this.fark.Size = new System.Drawing.Size(219, 25);
            this.fark.TabIndex = 100;
            this.fark.ToolTip = "Fatura altı iskontoyu\r\n% olarak yapar";
            this.fark.Visible = false;
            // 
            // cbAktifHesap
            // 
            this.cbAktifHesap.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbAktifHesap.EditValue = true;
            this.cbAktifHesap.Enabled = false;
            this.cbAktifHesap.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbAktifHesap.Location = new System.Drawing.Point(161, 158);
            this.cbAktifHesap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAktifHesap.Name = "cbAktifHesap";
            this.cbAktifHesap.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.cbAktifHesap.Properties.Appearance.Options.UseBackColor = true;
            this.cbAktifHesap.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cbAktifHesap.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cbAktifHesap.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cbAktifHesap.Properties.Caption = "Kasaya İşle";
            this.cbAktifHesap.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.cbAktifHesap.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbAktifHesap.Size = new System.Drawing.Size(101, 24);
            this.cbAktifHesap.TabIndex = 99;
            this.cbAktifHesap.TabStop = false;
            // 
            // txtAciklama
            // 
            this.txtAciklama.EditValue = "Günlük Kasa Durumu";
            this.txtAciklama.Location = new System.Drawing.Point(163, 112);
            this.txtAciklama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtAciklama.Properties.Appearance.Options.UseFont = true;
            this.txtAciklama.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtAciklama.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtAciklama.Size = new System.Drawing.Size(219, 24);
            this.txtAciklama.TabIndex = 98;
            // 
            // deTarih
            // 
            this.deTarih.EditValue = new System.DateTime(2014, 8, 3, 0, 0, 0, 0);
            this.deTarih.Location = new System.Drawing.Point(163, 7);
            this.deTarih.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deTarih.Name = "deTarih";
            this.deTarih.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.deTarih.Properties.Appearance.Options.UseFont = true;
            this.deTarih.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTarih.Properties.DisplayFormat.FormatString = "g";
            this.deTarih.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deTarih.Properties.EditFormat.FormatString = "g";
            this.deTarih.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deTarih.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deTarih.Size = new System.Drawing.Size(180, 27);
            this.deTarih.TabIndex = 96;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button4.Location = new System.Drawing.Point(9, 113);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(147, 30);
            this.button4.TabIndex = 22;
            this.button4.Text = "Açıklama";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(9, 7);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(147, 26);
            this.button1.TabIndex = 20;
            this.button1.Text = "İşlem Tarihi";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button5.Location = new System.Drawing.Point(10, 193);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(147, 30);
            this.button5.TabIndex = 22;
            this.button5.Text = "Kasa";
            this.button5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // lueKasalar
            // 
            this.lueKasalar.Location = new System.Drawing.Point(163, 196);
            this.lueKasalar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueKasalar.Name = "lueKasalar";
            this.lueKasalar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lueKasalar.Properties.Appearance.Options.UseFont = true;
            this.lueKasalar.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lueKasalar.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.lueKasalar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueKasalar.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("KasaAdi", "Kasa Adı"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkKasalar", "pkKasalar", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.lueKasalar.Properties.DisplayMember = "KasaAdi";
            this.lueKasalar.Properties.NullText = "Seçiniz";
            this.lueKasalar.Properties.ValueMember = "pkKasalar";
            this.lueKasalar.Size = new System.Drawing.Size(219, 25);
            this.lueKasalar.TabIndex = 110;
            this.lueKasalar.Tag = "0";
            // 
            // frmKasaSayimSonucu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 374);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmKasaSayimSonucu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kasa Sayım Sonucu";
            this.Load += new System.EventHandler(this.frmGunSonuRaporlari_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ucSatislar_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olmasigereken.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kasadaki.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAktifHesap.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTarih.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTarih.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueKasalar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton btnyazdir;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraEditors.CalcEdit olmasigereken;
        private System.Windows.Forms.Button button3;
        private DevExpress.XtraEditors.SimpleButton btnDevirBakiye;
        private DevExpress.XtraEditors.DateEdit deTarih;
        private System.Windows.Forms.Button button4;
        private DevExpress.XtraEditors.TextEdit txtAciklama;
        private DevExpress.XtraEditors.CheckEdit cbAktifHesap;
        private DevExpress.XtraEditors.CalcEdit fark;
        private System.Windows.Forms.Button button2;
        private DevExpress.XtraEditors.CalcEdit kasadaki;
        private System.Windows.Forms.Button button5;
        private DevExpress.XtraEditors.LookUpEdit lueKasalar;
    }
}

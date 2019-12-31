namespace GPTS
{
    partial class frmStokDevirDuzenle
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.islemtarihi = new DevExpress.XtraEditors.DateEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.ceDevirAdedi = new DevExpress.XtraEditors.CalcEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.pkStokDevir = new System.Windows.Forms.TextBox();
            this.StokAdi = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtAciklama = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtOncekiAdet = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lueDepolar = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.islemtarihi.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.islemtarihi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDevirAdedi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOncekiAdet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueDepolar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(399, 53);
            this.panelControl1.TabIndex = 2;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(125, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(132, 49);
            this.BtnKaydet.TabIndex = 1;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(257, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(140, 49);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.edit_clear;
            this.simpleButton2.Location = new System.Drawing.Point(2, 2);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(101, 49);
            this.simpleButton2.TabIndex = 89;
            this.simpleButton2.Text = "Temizle";
            this.simpleButton2.Visible = false;
            // 
            // islemtarihi
            // 
            this.islemtarihi.EditValue = null;
            this.islemtarihi.Location = new System.Drawing.Point(178, 110);
            this.islemtarihi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.islemtarihi.Name = "islemtarihi";
            this.islemtarihi.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.islemtarihi.Properties.Appearance.Options.UseFont = true;
            this.islemtarihi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.islemtarihi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.islemtarihi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.islemtarihi.Properties.DisplayFormat.FormatString = "g";
            this.islemtarihi.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.islemtarihi.Properties.EditFormat.FormatString = "g";
            this.islemtarihi.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.islemtarihi.Properties.Mask.EditMask = "g";
            this.islemtarihi.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.islemtarihi.Size = new System.Drawing.Size(185, 24);
            this.islemtarihi.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(120, 113);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 18);
            this.label11.TabIndex = 4;
            this.label11.Text = "Tarihi";
            // 
            // ceDevirAdedi
            // 
            this.ceDevirAdedi.Location = new System.Drawing.Point(177, 143);
            this.ceDevirAdedi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ceDevirAdedi.Name = "ceDevirAdedi";
            this.ceDevirAdedi.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ceDevirAdedi.Properties.Appearance.Options.UseFont = true;
            this.ceDevirAdedi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ceDevirAdedi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.ceDevirAdedi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceDevirAdedi.Properties.DisplayFormat.FormatString = "{0:#0.00####}";
            this.ceDevirAdedi.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ceDevirAdedi.Size = new System.Drawing.Size(187, 35);
            this.ceDevirAdedi.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(80, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Devir Adedi";
            // 
            // pkStokDevir
            // 
            this.pkStokDevir.Location = new System.Drawing.Point(12, 108);
            this.pkStokDevir.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkStokDevir.Name = "pkStokDevir";
            this.pkStokDevir.Size = new System.Drawing.Size(81, 23);
            this.pkStokDevir.TabIndex = 91;
            this.pkStokDevir.Text = "0";
            this.pkStokDevir.Visible = false;
            this.pkStokDevir.TextChanged += new System.EventHandler(this.pkStokDevir_TextChanged);
            // 
            // StokAdi
            // 
            this.StokAdi.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.StokAdi.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.StokAdi.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.StokAdi.Dock = System.Windows.Forms.DockStyle.Top;
            this.StokAdi.Location = new System.Drawing.Point(0, 53);
            this.StokAdi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StokAdi.Name = "StokAdi";
            this.StokAdi.Size = new System.Drawing.Size(399, 34);
            this.StokAdi.TabIndex = 98;
            this.StokAdi.Tag = "0";
            this.StokAdi.Text = "DevirId";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(103, 250);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(51, 16);
            this.labelControl1.TabIndex = 96;
            this.labelControl1.Text = "Açıklama";
            // 
            // txtAciklama
            // 
            this.txtAciklama.EditValue = "";
            this.txtAciklama.Location = new System.Drawing.Point(177, 245);
            this.txtAciklama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtAciklama.Properties.Appearance.Options.UseFont = true;
            this.txtAciklama.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtAciklama.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtAciklama.Size = new System.Drawing.Size(187, 25);
            this.txtAciklama.TabIndex = 95;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(89, 204);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(68, 16);
            this.labelControl2.TabIndex = 96;
            this.labelControl2.Text = "Önceki Adet";
            // 
            // txtOncekiAdet
            // 
            this.txtOncekiAdet.EditValue = "";
            this.txtOncekiAdet.Location = new System.Drawing.Point(176, 198);
            this.txtOncekiAdet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOncekiAdet.Name = "txtOncekiAdet";
            this.txtOncekiAdet.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtOncekiAdet.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtOncekiAdet.Properties.Appearance.Options.UseBackColor = true;
            this.txtOncekiAdet.Properties.Appearance.Options.UseFont = true;
            this.txtOncekiAdet.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtOncekiAdet.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtOncekiAdet.Properties.ReadOnly = true;
            this.txtOncekiAdet.Size = new System.Drawing.Size(187, 25);
            this.txtOncekiAdet.TabIndex = 95;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(112, 293);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(51, 16);
            this.labelControl4.TabIndex = 96;
            this.labelControl4.Text = "Depo Adı";
            // 
            // lueDepolar
            // 
            this.lueDepolar.Location = new System.Drawing.Point(176, 290);
            this.lueDepolar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueDepolar.Name = "lueDepolar";
            this.lueDepolar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueDepolar.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkDepolar", "pkDepolar", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DepoAdi", "Depo Adı")});
            this.lueDepolar.Properties.DisplayMember = "DepoAdi";
            this.lueDepolar.Properties.NullText = "Seçiniz...";
            this.lueDepolar.Properties.ShowHeader = false;
            this.lueDepolar.Properties.ValueMember = "pkDepolar";
            this.lueDepolar.Size = new System.Drawing.Size(185, 22);
            this.lueDepolar.TabIndex = 145;
            // 
            // frmStokDevirDuzenle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 332);
            this.ControlBox = false;
            this.Controls.Add(this.lueDepolar);
            this.Controls.Add(this.StokAdi);
            this.Controls.Add(this.txtAciklama);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtOncekiAdet);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.pkStokDevir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ceDevirAdedi);
            this.Controls.Add(this.islemtarihi);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmStokDevirDuzenle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stok Devir Duzenle";
            this.Load += new System.EventHandler(this.frmKasaHareketDuzelt_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmKasaHareketDuzelt_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.islemtarihi.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.islemtarihi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDevirAdedi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOncekiAdet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueDepolar.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.DateEdit islemtarihi;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.CalcEdit ceDevirAdedi;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox pkStokDevir;
        public DevExpress.XtraEditors.LabelControl StokAdi;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtAciklama;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtOncekiAdet;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LookUpEdit lueDepolar;
    }
}
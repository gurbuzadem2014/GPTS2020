namespace GPTS
{
    partial class frmFaturaDuzelt
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
            this.deFaturaTarihi = new DevExpress.XtraEditors.DateEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.ceFaturaTutar = new DevExpress.XtraEditors.CalcEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.tEaciklama = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.CariAdi = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtFaturaNo = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deFaturaTarihi.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deFaturaTarihi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFaturaTutar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tEaciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaturaNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(425, 55);
            this.panelControl1.TabIndex = 2;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(198, 2);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(113, 51);
            this.BtnKaydet.TabIndex = 1;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(311, 2);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(112, 51);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.edit_clear;
            this.simpleButton2.Location = new System.Drawing.Point(2, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(101, 51);
            this.simpleButton2.TabIndex = 89;
            this.simpleButton2.Text = "Temizle";
            this.simpleButton2.Visible = false;
            // 
            // deFaturaTarihi
            // 
            this.deFaturaTarihi.EditValue = null;
            this.deFaturaTarihi.Location = new System.Drawing.Point(153, 89);
            this.deFaturaTarihi.Name = "deFaturaTarihi";
            this.deFaturaTarihi.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.deFaturaTarihi.Properties.Appearance.Options.UseFont = true;
            this.deFaturaTarihi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.deFaturaTarihi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.deFaturaTarihi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deFaturaTarihi.Properties.DisplayFormat.FormatString = "g";
            this.deFaturaTarihi.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deFaturaTarihi.Properties.EditFormat.FormatString = "g";
            this.deFaturaTarihi.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deFaturaTarihi.Properties.Mask.EditMask = "g";
            this.deFaturaTarihi.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deFaturaTarihi.Size = new System.Drawing.Size(159, 21);
            this.deFaturaTarihi.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(60, 92);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 14);
            this.label11.TabIndex = 4;
            this.label11.Text = "Fatura Tarihi ";
            // 
            // ceFaturaTutar
            // 
            this.ceFaturaTutar.Enabled = false;
            this.ceFaturaTutar.Location = new System.Drawing.Point(152, 118);
            this.ceFaturaTutar.Name = "ceFaturaTutar";
            this.ceFaturaTutar.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ceFaturaTutar.Properties.Appearance.Options.UseFont = true;
            this.ceFaturaTutar.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ceFaturaTutar.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.ceFaturaTutar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceFaturaTutar.Properties.DisplayFormat.FormatString = "{0:#0.00####}";
            this.ceFaturaTutar.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ceFaturaTutar.Size = new System.Drawing.Size(160, 23);
            this.ceFaturaTutar.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(59, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "Fatura Tutarı";
            // 
            // tEaciklama
            // 
            this.tEaciklama.EditValue = "";
            this.tEaciklama.Location = new System.Drawing.Point(151, 177);
            this.tEaciklama.Name = "tEaciklama";
            this.tEaciklama.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tEaciklama.Properties.Appearance.Options.UseFont = true;
            this.tEaciklama.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.tEaciklama.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tEaciklama.Size = new System.Drawing.Size(262, 21);
            this.tEaciklama.TabIndex = 95;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(93, 180);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(41, 13);
            this.labelControl3.TabIndex = 96;
            this.labelControl3.Text = "Açıklama";
            // 
            // CariAdi
            // 
            this.CariAdi.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.CariAdi.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CariAdi.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.CariAdi.Dock = System.Windows.Forms.DockStyle.Top;
            this.CariAdi.Location = new System.Drawing.Point(0, 55);
            this.CariAdi.Name = "CariAdi";
            this.CariAdi.Size = new System.Drawing.Size(425, 28);
            this.CariAdi.TabIndex = 98;
            this.CariAdi.Tag = "0";
            this.CariAdi.Text = "CariAdi";
            this.CariAdi.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(89, 154);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 13);
            this.labelControl1.TabIndex = 96;
            this.labelControl1.Text = "Fatura No";
            // 
            // txtFaturaNo
            // 
            this.txtFaturaNo.EditValue = "";
            this.txtFaturaNo.Location = new System.Drawing.Point(153, 150);
            this.txtFaturaNo.Name = "txtFaturaNo";
            this.txtFaturaNo.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFaturaNo.Properties.Appearance.Options.UseFont = true;
            this.txtFaturaNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFaturaNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtFaturaNo.Size = new System.Drawing.Size(160, 21);
            this.txtFaturaNo.TabIndex = 95;
            // 
            // frmFaturaDuzelt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 219);
            this.ControlBox = false;
            this.Controls.Add(this.CariAdi);
            this.Controls.Add(this.txtFaturaNo);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.tEaciklama);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ceFaturaTutar);
            this.Controls.Add(this.deFaturaTarihi);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Name = "frmFaturaDuzelt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fatura Düzelt {Fiş Düzenle}";
            this.Load += new System.EventHandler(this.frmKasaHareketDuzelt_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmKasaHareketDuzelt_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.deFaturaTarihi.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deFaturaTarihi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFaturaTutar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tEaciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaturaNo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.DateEdit deFaturaTarihi;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.CalcEdit ceFaturaTutar;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit tEaciklama;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        public DevExpress.XtraEditors.LabelControl CariAdi;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtFaturaNo;
    }
}
namespace GPTS
{
    partial class frmFaturaTopluDuzelt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFaturaTopluDuzelt));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.pkFaturaToplu = new System.Windows.Forms.TextBox();
            this.pkFirma = new DevExpress.XtraEditors.TextEdit();
            this.deFaturaTarihi = new DevExpress.XtraEditors.DateEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.txtFaturaAdresi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.CariAdi = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtFaturaNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtVergiDairesi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtVerigiNo = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pkFirma.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deFaturaTarihi.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deFaturaTarihi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaturaAdresi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaturaNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVergiDairesi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerigiNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Controls.Add(this.pkFaturaToplu);
            this.panelControl1.Controls.Add(this.pkFirma);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(781, 68);
            this.panelControl1.TabIndex = 2;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(491, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(132, 64);
            this.BtnKaydet.TabIndex = 1;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton21.Image")));
            this.simpleButton21.Location = new System.Drawing.Point(623, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(156, 64);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // pkFaturaToplu
            // 
            this.pkFaturaToplu.Location = new System.Drawing.Point(221, 23);
            this.pkFaturaToplu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkFaturaToplu.Name = "pkFaturaToplu";
            this.pkFaturaToplu.Size = new System.Drawing.Size(116, 23);
            this.pkFaturaToplu.TabIndex = 91;
            this.pkFaturaToplu.Text = "0";
            this.pkFaturaToplu.Visible = false;
            // 
            // pkFirma
            // 
            this.pkFirma.EditValue = "0";
            this.pkFirma.Location = new System.Drawing.Point(359, 21);
            this.pkFirma.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pkFirma.Name = "pkFirma";
            this.pkFirma.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.pkFirma.Properties.Appearance.Options.UseFont = true;
            this.pkFirma.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.pkFirma.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.pkFirma.Size = new System.Drawing.Size(73, 25);
            this.pkFirma.TabIndex = 95;
            this.pkFirma.Visible = false;
            this.pkFirma.EditValueChanged += new System.EventHandler(this.pkFirma_EditValueChanged);
            // 
            // deFaturaTarihi
            // 
            this.deFaturaTarihi.EditValue = null;
            this.deFaturaTarihi.Location = new System.Drawing.Point(179, 150);
            this.deFaturaTarihi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.deFaturaTarihi.Size = new System.Drawing.Size(185, 24);
            this.deFaturaTarihi.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(62, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 22);
            this.label11.TabIndex = 4;
            this.label11.Text = "Fatura Tarihi ";
            // 
            // txtFaturaAdresi
            // 
            this.txtFaturaAdresi.EditValue = "";
            this.txtFaturaAdresi.Location = new System.Drawing.Point(177, 197);
            this.txtFaturaAdresi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFaturaAdresi.Name = "txtFaturaAdresi";
            this.txtFaturaAdresi.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFaturaAdresi.Properties.Appearance.Options.UseFont = true;
            this.txtFaturaAdresi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFaturaAdresi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtFaturaAdresi.Size = new System.Drawing.Size(573, 25);
            this.txtFaturaAdresi.TabIndex = 95;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Arial", 11F);
            this.labelControl3.Location = new System.Drawing.Point(67, 199);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(102, 21);
            this.labelControl3.TabIndex = 96;
            this.labelControl3.Text = "FaturaAdresi";
            // 
            // CariAdi
            // 
            this.CariAdi.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.CariAdi.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CariAdi.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.CariAdi.Dock = System.Windows.Forms.DockStyle.Top;
            this.CariAdi.Location = new System.Drawing.Point(0, 68);
            this.CariAdi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CariAdi.Name = "CariAdi";
            this.CariAdi.Size = new System.Drawing.Size(781, 34);
            this.CariAdi.TabIndex = 98;
            this.CariAdi.Tag = "0";
            this.CariAdi.Text = "CariAdi";
            this.CariAdi.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 11F);
            this.labelControl1.Location = new System.Drawing.Point(91, 112);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 21);
            this.labelControl1.TabIndex = 96;
            this.labelControl1.Text = "Fatura No";
            // 
            // txtFaturaNo
            // 
            this.txtFaturaNo.EditValue = "";
            this.txtFaturaNo.Location = new System.Drawing.Point(177, 110);
            this.txtFaturaNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFaturaNo.Name = "txtFaturaNo";
            this.txtFaturaNo.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFaturaNo.Properties.Appearance.Options.UseFont = true;
            this.txtFaturaNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFaturaNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtFaturaNo.Size = new System.Drawing.Size(187, 25);
            this.txtFaturaNo.TabIndex = 95;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Arial", 11F);
            this.labelControl2.Location = new System.Drawing.Point(65, 243);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(102, 21);
            this.labelControl2.TabIndex = 96;
            this.labelControl2.Text = "Vergi Dairesi";
            // 
            // txtVergiDairesi
            // 
            this.txtVergiDairesi.EditValue = "";
            this.txtVergiDairesi.Location = new System.Drawing.Point(176, 240);
            this.txtVergiDairesi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtVergiDairesi.Name = "txtVergiDairesi";
            this.txtVergiDairesi.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtVergiDairesi.Properties.Appearance.Options.UseFont = true;
            this.txtVergiDairesi.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtVergiDairesi.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtVergiDairesi.Size = new System.Drawing.Size(187, 25);
            this.txtVergiDairesi.TabIndex = 95;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Arial", 11F);
            this.labelControl4.Location = new System.Drawing.Point(477, 240);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(69, 21);
            this.labelControl4.TabIndex = 96;
            this.labelControl4.Text = "Vergi No";
            // 
            // txtVerigiNo
            // 
            this.txtVerigiNo.EditValue = "";
            this.txtVerigiNo.Location = new System.Drawing.Point(563, 238);
            this.txtVerigiNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtVerigiNo.Name = "txtVerigiNo";
            this.txtVerigiNo.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtVerigiNo.Properties.Appearance.Options.UseFont = true;
            this.txtVerigiNo.Properties.AppearanceFocused.BackColor = System.Drawing.Color.GreenYellow;
            this.txtVerigiNo.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtVerigiNo.Size = new System.Drawing.Size(187, 25);
            this.txtVerigiNo.TabIndex = 95;
            // 
            // frmFaturaTopluDuzelt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 305);
            this.ControlBox = false;
            this.Controls.Add(this.CariAdi);
            this.Controls.Add(this.txtVergiDairesi);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtVerigiNo);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtFaturaNo);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtFaturaAdresi);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.deFaturaTarihi);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmFaturaTopluDuzelt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fatura Toplu Duzelt";
            this.Load += new System.EventHandler(this.frmKasaHareketDuzelt_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmKasaHareketDuzelt_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pkFirma.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deFaturaTarihi.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deFaturaTarihi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaturaAdresi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaturaNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVergiDairesi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerigiNo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.DateEdit deFaturaTarihi;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.TextBox pkFaturaToplu;
        private DevExpress.XtraEditors.TextEdit txtFaturaAdresi;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        public DevExpress.XtraEditors.LabelControl CariAdi;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtFaturaNo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtVergiDairesi;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtVerigiNo;
        private DevExpress.XtraEditors.TextEdit pkFirma;
    }
}
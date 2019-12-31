namespace GPTS
{
    partial class frmYeniMarkaYeniKarti
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
            this.MarkaAdi = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.RenkAdi = new DevExpress.XtraEditors.TextEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.Beden = new DevExpress.XtraEditors.TextEdit();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MarkaAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RenkAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Beden.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // MarkaAdi
            // 
            this.MarkaAdi.EnterMoveNextControl = true;
            this.MarkaAdi.Location = new System.Drawing.Point(131, 90);
            this.MarkaAdi.Name = "MarkaAdi";
            this.MarkaAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.MarkaAdi.Properties.Appearance.Options.UseFont = true;
            this.MarkaAdi.Size = new System.Drawing.Size(298, 26);
            this.MarkaAdi.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(460, 56);
            this.panelControl1.TabIndex = 56;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(223, 2);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(112, 52);
            this.BtnKaydet.TabIndex = 55;
            this.BtnKaydet.Text = "Tamam [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton21.Location = new System.Drawing.Point(335, 2);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(123, 52);
            this.simpleButton21.TabIndex = 56;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(466, 177);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.panelControl1);
            this.xtraTabPage1.Controls.Add(this.MarkaAdi);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(460, 151);
            this.xtraTabPage1.Text = "Yeni Marka Adı";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 96);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(101, 13);
            this.labelControl1.TabIndex = 92;
            this.labelControl1.Text = "Yeni Marka Adı Giriniz";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.labelControl2);
            this.xtraTabPage2.Controls.Add(this.RenkAdi);
            this.xtraTabPage2.Controls.Add(this.panelControl2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(460, 151);
            this.xtraTabPage2.Text = "Yeni Renk Adı";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(26, 92);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 13);
            this.labelControl2.TabIndex = 94;
            this.labelControl2.Text = "Yeni Renk Adı Giriniz";
            // 
            // RenkAdi
            // 
            this.RenkAdi.EnterMoveNextControl = true;
            this.RenkAdi.Location = new System.Drawing.Point(137, 86);
            this.RenkAdi.Name = "RenkAdi";
            this.RenkAdi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.RenkAdi.Properties.Appearance.Options.UseFont = true;
            this.RenkAdi.Size = new System.Drawing.Size(298, 26);
            this.RenkAdi.TabIndex = 93;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.simpleButton1);
            this.panelControl2.Controls.Add(this.simpleButton2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(460, 56);
            this.panelControl2.TabIndex = 57;
            this.panelControl2.TabStop = true;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton1.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.simpleButton1.Location = new System.Drawing.Point(223, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(112, 52);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "Tamam [F9]";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton2.Location = new System.Drawing.Point(335, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(123, 52);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "Vazgeç [ESC]";
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.labelControl3);
            this.xtraTabPage3.Controls.Add(this.Beden);
            this.xtraTabPage3.Controls.Add(this.panelControl3);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(460, 151);
            this.xtraTabPage3.Text = "Yeni Beden && Numara";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(26, 92);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(152, 13);
            this.labelControl3.TabIndex = 94;
            this.labelControl3.Text = "Yeni Beden && Numara Adı Giriniz";
            // 
            // Beden
            // 
            this.Beden.EnterMoveNextControl = true;
            this.Beden.Location = new System.Drawing.Point(184, 86);
            this.Beden.Name = "Beden";
            this.Beden.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.Beden.Properties.Appearance.Options.UseFont = true;
            this.Beden.Size = new System.Drawing.Size(251, 26);
            this.Beden.TabIndex = 93;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.simpleButton3);
            this.panelControl3.Controls.Add(this.simpleButton4);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(460, 56);
            this.panelControl3.TabIndex = 57;
            this.panelControl3.TabStop = true;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton3.Image = global::GPTS.Properties.Resources.ActiveRents_32x32;
            this.simpleButton3.Location = new System.Drawing.Point(223, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(112, 52);
            this.simpleButton3.TabIndex = 0;
            this.simpleButton3.Text = "Tamam [F9]";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton4.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton4.Location = new System.Drawing.Point(335, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(123, 52);
            this.simpleButton4.TabIndex = 1;
            this.simpleButton4.Text = "Vazgeç [ESC]";
            // 
            // frmYeniMarkaYeniKarti
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 177);
            this.Controls.Add(this.xtraTabControl1);
            this.KeyPreview = true;
            this.Name = "frmYeniMarkaYeniKarti";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "M";
            this.Text = "Yeni Marka";
            this.Load += new System.EventHandler(this.frmStokKoduverKarti_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStokKoduverKarti_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.MarkaAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RenkAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.xtraTabPage3.ResumeLayout(false);
            this.xtraTabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Beden.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit MarkaAdi;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit RenkAdi;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit Beden;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
    }
}
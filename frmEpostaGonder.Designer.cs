namespace GPTS
{
    partial class frmEpostaGonder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEpostaGonder));
            this.meMesaj = new DevExpress.XtraEditors.MemoEdit();
            this.pFaturaTarihi = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.meMesaj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pFaturaTarihi)).BeginInit();
            this.pFaturaTarihi.SuspendLayout();
            this.SuspendLayout();
            // 
            // meMesaj
            // 
            this.meMesaj.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meMesaj.EditValue = resources.GetString("meMesaj.EditValue");
            this.meMesaj.Location = new System.Drawing.Point(0, 61);
            this.meMesaj.Name = "meMesaj";
            this.meMesaj.Size = new System.Drawing.Size(736, 408);
            this.meMesaj.TabIndex = 1;
            this.meMesaj.EditValueChanged += new System.EventHandler(this.meMesaj_EditValueChanged);
            // 
            // pFaturaTarihi
            // 
            this.pFaturaTarihi.Controls.Add(this.BtnKaydet);
            this.pFaturaTarihi.Controls.Add(this.simpleButton4);
            this.pFaturaTarihi.Dock = System.Windows.Forms.DockStyle.Top;
            this.pFaturaTarihi.Location = new System.Drawing.Point(0, 0);
            this.pFaturaTarihi.Name = "pFaturaTarihi";
            this.pFaturaTarihi.Size = new System.Drawing.Size(736, 61);
            this.pFaturaTarihi.TabIndex = 62;
            this.pFaturaTarihi.Visible = false;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.mail_send;
            this.BtnKaydet.Location = new System.Drawing.Point(2, 2);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(119, 57);
            this.BtnKaydet.TabIndex = 90;
            this.BtnKaydet.Text = "Gönder";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton4.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton4.Location = new System.Drawing.Point(600, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(134, 57);
            this.simpleButton4.TabIndex = 89;
            this.simpleButton4.Text = "Vazgeç [ESC]";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // frmEpostaGonder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 469);
            this.Controls.Add(this.meMesaj);
            this.Controls.Add(this.pFaturaTarihi);
            this.Name = "frmEpostaGonder";
            this.Text = " E-Posta Gönder";
            ((System.ComponentModel.ISupportInitialize)(this.meMesaj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pFaturaTarihi)).EndInit();
            this.pFaturaTarihi.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        public DevExpress.XtraEditors.PanelControl pFaturaTarihi;
        public DevExpress.XtraEditors.MemoEdit meMesaj;
    }
}
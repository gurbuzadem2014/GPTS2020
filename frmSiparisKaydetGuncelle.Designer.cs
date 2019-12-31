namespace GPTS
{
    partial class frmSiparisKaydetGuncelle
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
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cbTumSiparis = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lueSablonGrup = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbTumSiparis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSablonGrup.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(473, 42);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl2.Location = new System.Drawing.Point(2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(469, 38);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "Müşteri Adı";
            this.labelControl2.Visible = false;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = global::GPTS.Properties.Resources.Delete_32x32;
            this.simpleButton2.Location = new System.Drawing.Point(280, 150);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(118, 49);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Tag = "0";
            this.simpleButton2.Text = "Hayır";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // cbTumSiparis
            // 
            this.cbTumSiparis.Location = new System.Drawing.Point(52, 216);
            this.cbTumSiparis.Name = "cbTumSiparis";
            this.cbTumSiparis.Properties.Caption = "Tüm Sipariş Etkilensin";
            this.cbTumSiparis.Size = new System.Drawing.Size(159, 19);
            this.cbTumSiparis.TabIndex = 3;
            this.cbTumSiparis.Tag = "0";
            this.cbTumSiparis.Visible = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = global::GPTS.Properties.Resources.camera_test;
            this.simpleButton1.Location = new System.Drawing.Point(54, 150);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(124, 49);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Tag = "1";
            this.simpleButton1.Text = "Evet";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Location = new System.Drawing.Point(22, 60);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(427, 25);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Satış Çevirmek İstediğinize Eminmisiniz?";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(109, 104);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(101, 13);
            this.labelControl3.TabIndex = 35;
            this.labelControl3.Text = "Oluşturulacak Dönem";
            this.labelControl3.Visible = false;
            // 
            // lueSablonGrup
            // 
            this.lueSablonGrup.Location = new System.Drawing.Point(216, 100);
            this.lueSablonGrup.Name = "lueSablonGrup";
            this.lueSablonGrup.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lueSablonGrup.Properties.Appearance.Options.UseFont = true;
            this.lueSablonGrup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueSablonGrup.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Aciklama", "Aciklama"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkSablonGrup", "pkSablonGrup", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.lueSablonGrup.Properties.DisplayMember = "Aciklama";
            this.lueSablonGrup.Properties.DropDownRows = 15;
            this.lueSablonGrup.Properties.NullText = "";
            this.lueSablonGrup.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.lueSablonGrup.Properties.ShowHeader = false;
            this.lueSablonGrup.Properties.ShowPopupShadow = false;
            this.lueSablonGrup.Properties.ValueMember = "pkSablonGrup";
            this.lueSablonGrup.Size = new System.Drawing.Size(131, 21);
            this.lueSablonGrup.TabIndex = 34;
            this.lueSablonGrup.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.lueSablonGrup.Visible = false;
            // 
            // frmSiparisKaydetGuncelle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 263);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.lueSablonGrup);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.cbTumSiparis);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmSiparisKaydetGuncelle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "0";
            this.Text = "Satışa Çevir";
            this.Load += new System.EventHandler(this.frmSiparisKaydetGuncelle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbTumSiparis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSablonGrup.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        public DevExpress.XtraEditors.CheckEdit cbTumSiparis;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        public DevExpress.XtraEditors.LookUpEdit lueSablonGrup;
    }
}
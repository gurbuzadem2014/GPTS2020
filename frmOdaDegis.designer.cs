namespace GPTS
{
    partial class frmOdaDegis
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
            this.lueOdalar = new DevExpress.XtraEditors.LookUpEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.lueOdalar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lueOdalar
            // 
            this.lueOdalar.EditValue = "Tümü";
            this.lueOdalar.EnterMoveNextControl = true;
            this.lueOdalar.Location = new System.Drawing.Point(27, 95);
            this.lueOdalar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueOdalar.Name = "lueOdalar";
            this.lueOdalar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lueOdalar.Properties.Appearance.Options.UseFont = true;
            this.lueOdalar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueOdalar.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkOda", "pkOda", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("oda_adi", "oda_adi")});
            this.lueOdalar.Properties.DisplayMember = "oda_adi";
            this.lueOdalar.Properties.DropDownRows = 15;
            this.lueOdalar.Properties.NullText = "Tüm Odalar";
            this.lueOdalar.Properties.ShowHeader = false;
            this.lueOdalar.Properties.ValueMember = "pkOda";
            this.lueOdalar.Size = new System.Drawing.Size(301, 29);
            this.lueOdalar.TabIndex = 111;
            this.lueOdalar.Tag = "0";
            this.lueOdalar.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.BtnKaydet);
            this.panelControl2.Controls.Add(this.simpleButton21);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(613, 68);
            this.panelControl2.TabIndex = 112;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.onay_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(297, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(159, 64);
            this.BtnKaydet.TabIndex = 25;
            this.BtnKaydet.Text = "Tamam";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton21.Location = new System.Drawing.Point(456, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(155, 64);
            this.simpleButton21.TabIndex = 88;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // frmOdaDegis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 137);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.lueOdalar);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmOdaDegis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "0";
            this.Text = "Oda Tanımları";
            this.Load += new System.EventHandler(this.frmDepoKarti_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lueOdalar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public DevExpress.XtraEditors.LookUpEdit lueOdalar;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
    }
}
namespace GPTS
{
    partial class inputForm
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.GirilenCaption = new DevExpress.XtraEditors.LabelControl();
            this.Girilen = new System.Windows.Forms.TextBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lueKarOranlari = new DevExpress.XtraEditors.LookUpEdit();
            this.simpleButton28 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueKarOranlari.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GirilenCaption
            // 
            this.GirilenCaption.Location = new System.Drawing.Point(33, 94);
            this.GirilenCaption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GirilenCaption.Name = "GirilenCaption";
            this.GirilenCaption.Size = new System.Drawing.Size(66, 16);
            this.GirilenCaption.TabIndex = 1;
            this.GirilenCaption.Text = "Şifre Giriniz";
            // 
            // Girilen
            // 
            this.Girilen.Location = new System.Drawing.Point(33, 117);
            this.Girilen.Name = "Girilen";
            this.Girilen.Size = new System.Drawing.Size(186, 23);
            this.Girilen.TabIndex = 2;
            this.Girilen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sifre_KeyDown);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(334, 55);
            this.panelControl1.TabIndex = 61;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.onay_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(2, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(131, 51);
            this.BtnKaydet.TabIndex = 56;
            this.BtnKaydet.Text = "Tamam [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Image = global::GPTS.Properties.Resources.cancel;
            this.btnCancel.Location = new System.Drawing.Point(189, 2);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(143, 51);
            this.btnCancel.TabIndex = 89;
            this.btnCancel.Text = "Vazgeç [Esc]";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lueKarOranlari
            // 
            this.lueKarOranlari.Location = new System.Drawing.Point(257, 117);
            this.lueKarOranlari.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueKarOranlari.Name = "lueKarOranlari";
            this.lueKarOranlari.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueKarOranlari.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("iskonto_id", "iskonto_id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("iskonto_orani", "iskonto")});
            this.lueKarOranlari.Properties.DisplayMember = "iskonto_orani";
            this.lueKarOranlari.Properties.NullText = "Seçiniz...";
            this.lueKarOranlari.Properties.ShowHeader = false;
            this.lueKarOranlari.Properties.ValueMember = "iskonto_id";
            this.lueKarOranlari.Size = new System.Drawing.Size(64, 22);
            this.lueKarOranlari.TabIndex = 184;
            this.lueKarOranlari.Tag = "0";
            this.lueKarOranlari.Visible = false;
            this.lueKarOranlari.EditValueChanged += new System.EventHandler(this.lueKarOranlari_EditValueChanged);
            // 
            // simpleButton28
            // 
            this.simpleButton28.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton28.Image = global::GPTS.Properties.Resources.dep_1412526_Two_men_with_Refresh_symbol2;
            this.simpleButton28.Location = new System.Drawing.Point(214, 113);
            this.simpleButton28.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton28.Name = "simpleButton28";
            this.simpleButton28.Size = new System.Drawing.Size(40, 31);
            toolTipItem1.Text = "Yeni Grup Tanımlayabilirsiniz";
            superToolTip1.Items.Add(toolTipItem1);
            this.simpleButton28.SuperTip = superToolTip1;
            this.simpleButton28.TabIndex = 206;
            this.simpleButton28.TabStop = false;
            this.simpleButton28.Visible = false;
            this.simpleButton28.Click += new System.EventHandler(this.simpleButton28_Click);
            // 
            // inputForm
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 166);
            this.Controls.Add(this.lueKarOranlari);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.Girilen);
            this.Controls.Add(this.GirilenCaption);
            this.Controls.Add(this.simpleButton28);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "inputForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Şifre Ekranı";
            this.Load += new System.EventHandler(this.inputForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lueKarOranlari.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public DevExpress.XtraEditors.LabelControl GirilenCaption;
        public System.Windows.Forms.TextBox Girilen;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        public DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LookUpEdit lueKarOranlari;
        private DevExpress.XtraEditors.SimpleButton simpleButton28;
    }
}
namespace GPTS
{
    partial class frmStokKartiHizli
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.ceBarkod = new DevExpress.XtraEditors.CalcEdit();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            this.tEStokadi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.SatisFiyati1 = new DevExpress.XtraEditors.CalcEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ceBarkod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tEStokadi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SatisFiyati1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ceBarkod
            // 
            this.ceBarkod.Location = new System.Drawing.Point(83, 95);
            this.ceBarkod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ceBarkod.Name = "ceBarkod";
            this.ceBarkod.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ceBarkod.Properties.Appearance.Options.UseFont = true;
            this.ceBarkod.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.ceBarkod.Properties.EditFormat.FormatString = "{0}";
            this.ceBarkod.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ceBarkod.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.ceBarkod.Size = new System.Drawing.Size(178, 27);
            this.ceBarkod.TabIndex = 20;
            this.ceBarkod.TabStop = false;
            this.ceBarkod.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            // 
            // labelControl19
            // 
            this.labelControl19.Location = new System.Drawing.Point(26, 97);
            this.labelControl19.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(46, 16);
            this.labelControl19.TabIndex = 170;
            this.labelControl19.Text = "Barkodu";
            // 
            // tEStokadi
            // 
            this.tEStokadi.EnterMoveNextControl = true;
            this.tEStokadi.Location = new System.Drawing.Point(80, 132);
            this.tEStokadi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tEStokadi.Name = "tEStokadi";
            this.tEStokadi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tEStokadi.Properties.Appearance.Options.UseFont = true;
            this.tEStokadi.Size = new System.Drawing.Size(244, 27);
            toolTipItem1.Text = "Stoklarınızı girmeden önce lütfen kendinize bir sıralama belirleyiniz. Örn:ÜLKER " +
    "KREMA BİSKÜVİ 30GR";
            superToolTip1.Items.Add(toolTipItem1);
            this.tEStokadi.SuperTip = superToolTip1;
            this.tEStokadi.TabIndex = 1;
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(10, 171);
            this.labelControl17.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(62, 16);
            this.labelControl17.TabIndex = 175;
            this.labelControl17.Text = "Satış Fiyatı";
            // 
            // SatisFiyati1
            // 
            this.SatisFiyati1.EnterMoveNextControl = true;
            this.SatisFiyati1.Location = new System.Drawing.Point(80, 167);
            this.SatisFiyati1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SatisFiyati1.Name = "SatisFiyati1";
            this.SatisFiyati1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.SatisFiyati1.Properties.Appearance.Options.UseFont = true;
            this.SatisFiyati1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SatisFiyati1.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.SatisFiyati1.Size = new System.Drawing.Size(133, 27);
            this.SatisFiyati1.TabIndex = 2;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(338, 58);
            this.panelControl1.TabIndex = 176;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(2, 2);
            this.BtnKaydet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(148, 54);
            this.BtnKaydet.TabIndex = 3;
            this.BtnKaydet.Text = "Kaydet [F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(169, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(167, 54);
            this.simpleButton21.TabIndex = 4;
            this.simpleButton21.Text = "Vazgeç [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.simpleButton2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 265);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(338, 69);
            this.panelControl2.TabIndex = 177;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleButton2.Image = global::GPTS.Properties.Resources.stokekle_32x48;
            this.simpleButton2.Location = new System.Drawing.Point(2, 2);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(334, 65);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "Stok Tanıtım Kartına Geç";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton1.Location = new System.Drawing.Point(15, 132);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(58, 28);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "Stok Adı";
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(6, 220);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(325, 39);
            this.labelControl1.TabIndex = 175;
            this.labelControl1.Text = "Not:  Hızlı kaydettiğiniz stokları, stok listesinde bulup\r\neksiklerini tamamlayab" +
    "ilirsiniz.";
            // 
            // frmStokKartiHizli
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 334);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl17);
            this.Controls.Add(this.SatisFiyati1);
            this.Controls.Add(this.ceBarkod);
            this.Controls.Add(this.labelControl19);
            this.Controls.Add(this.tEStokadi);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmStokKartiHizli";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hızlı Stok Karti Girişi";
            this.Load += new System.EventHandler(this.frmStokKartiHizli_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStokKartiHizli_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ceBarkod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tEStokadi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SatisFiyati1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl19;
        private DevExpress.XtraEditors.TextEdit tEStokadi;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.CalcEdit SatisFiyati1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        public DevExpress.XtraEditors.CalcEdit ceBarkod;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
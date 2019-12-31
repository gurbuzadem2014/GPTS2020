namespace GPTS
{
    partial class frmGorusmeDetayBilgileri
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
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.pkProjeGorusme = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton10 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.GorusmeSonucu = new DevExpress.XtraEditors.TextEdit();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.GorusmeNotu = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.GorTarihi = new DevExpress.XtraEditors.DateEdit();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.GorYapKisiGorev = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.GorYapKisi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.GorRanTarihi = new DevExpress.XtraEditors.DateEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pkProjeGorusme.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorusmeSonucu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorusmeNotu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorTarihi.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorTarihi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorYapKisiGorev.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorYapKisi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorRanTarihi.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorRanTarihi.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.pkProjeGorusme);
            this.panelControl1.Controls.Add(this.simpleButton10);
            this.panelControl1.Controls.Add(this.simpleButton6);
            this.panelControl1.Controls.Add(this.BtnKaydet);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(815, 55);
            this.panelControl1.TabIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton1.Image = global::GPTS.Properties.Resources.Printer;
            this.simpleButton1.Location = new System.Drawing.Point(329, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(101, 51);
            toolTipTitleItem1.Text = "Kopyala";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Kaydettiğiniz stok kartının tamamını kopyalar ve zaman kaybınızı önler, daha az z" +
                "amanda daha çok stok işleyebilirsiniz";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.simpleButton1.SuperTip = superToolTip1;
            this.simpleButton1.TabIndex = 88;
            this.simpleButton1.Text = "Yazdır";
            // 
            // pkProjeGorusme
            // 
            this.pkProjeGorusme.EditValue = "0";
            this.pkProjeGorusme.Location = new System.Drawing.Point(479, 12);
            this.pkProjeGorusme.Name = "pkProjeGorusme";
            this.pkProjeGorusme.Size = new System.Drawing.Size(100, 20);
            this.pkProjeGorusme.TabIndex = 87;
            this.pkProjeGorusme.Visible = false;
            this.pkProjeGorusme.EditValueChanged += new System.EventHandler(this.pkProjeGorusme_EditValueChanged);
            // 
            // simpleButton10
            // 
            this.simpleButton10.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton10.Image = global::GPTS.Properties.Resources.kopyala_stock_copy;
            this.simpleButton10.Location = new System.Drawing.Point(228, 2);
            this.simpleButton10.Name = "simpleButton10";
            this.simpleButton10.Size = new System.Drawing.Size(101, 51);
            toolTipTitleItem2.Text = "Kopyala";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Kaydettiğiniz stok kartının tamamını kopyalar ve zaman kaybınızı önler, daha az z" +
                "amanda daha çok stok işleyebilirsiniz";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.simpleButton10.SuperTip = superToolTip2;
            this.simpleButton10.TabIndex = 86;
            this.simpleButton10.Text = "Kopyala";
            // 
            // simpleButton6
            // 
            this.simpleButton6.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton6.Image = global::GPTS.Properties.Resources.sil_1;
            this.simpleButton6.Location = new System.Drawing.Point(115, 2);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(113, 51);
            this.simpleButton6.TabIndex = 84;
            this.simpleButton6.Text = "Görüşme Sil \r\n[F5]";
            this.simpleButton6.Visible = false;
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnKaydet.Image = global::GPTS.Properties.Resources.Save_32x32;
            this.BtnKaydet.Location = new System.Drawing.Point(601, 2);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.Size = new System.Drawing.Size(101, 51);
            this.BtnKaydet.TabIndex = 13;
            this.BtnKaydet.Text = "Kaydet\r\n[F9]";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.kapat_32x32;
            this.simpleButton21.Location = new System.Drawing.Point(702, 2);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(111, 51);
            this.simpleButton21.TabIndex = 19;
            this.simpleButton21.Text = "Kapat\r\n[ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton4.Image = global::GPTS.Properties.Resources.new_window;
            this.simpleButton4.Location = new System.Drawing.Point(2, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(113, 51);
            this.simpleButton4.TabIndex = 85;
            this.simpleButton4.Text = "Yeni Görüşme \r\n[F7]";
            // 
            // GorusmeSonucu
            // 
            this.GorusmeSonucu.Location = new System.Drawing.Point(204, 262);
            this.GorusmeSonucu.Name = "GorusmeSonucu";
            this.GorusmeSonucu.Size = new System.Drawing.Size(246, 20);
            this.GorusmeSonucu.TabIndex = 51;
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(14, 267);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(35, 13);
            this.labelControl13.TabIndex = 50;
            this.labelControl13.Text = "SONUÇ";
            // 
            // GorusmeNotu
            // 
            this.GorusmeNotu.EditValue = "";
            this.GorusmeNotu.Location = new System.Drawing.Point(464, 86);
            this.GorusmeNotu.Name = "GorusmeNotu";
            this.GorusmeNotu.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.GorusmeNotu.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.GorusmeNotu.Size = new System.Drawing.Size(325, 194);
            this.GorusmeNotu.TabIndex = 49;
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(464, 67);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(96, 13);
            this.labelControl12.TabIndex = 48;
            this.labelControl12.Text = "GÖRÜŞME NOTLARI";
            // 
            // GorTarihi
            // 
            this.GorTarihi.EditValue = new System.DateTime(2011, 12, 18, 21, 45, 19, 0);
            this.GorTarihi.Location = new System.Drawing.Point(204, 222);
            this.GorTarihi.Name = "GorTarihi";
            this.GorTarihi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.GorTarihi.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.GorTarihi.Size = new System.Drawing.Size(103, 20);
            this.GorTarihi.TabIndex = 47;
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(14, 225);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(87, 13);
            this.labelControl11.TabIndex = 46;
            this.labelControl11.Text = "GÖRÜŞME TARİHİ";
            // 
            // GorYapKisiGorev
            // 
            this.GorYapKisiGorev.Location = new System.Drawing.Point(204, 180);
            this.GorYapKisiGorev.Name = "GorYapKisiGorev";
            this.GorYapKisiGorev.Size = new System.Drawing.Size(246, 20);
            this.GorYapKisiGorev.TabIndex = 45;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(14, 183);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(176, 13);
            this.labelControl10.TabIndex = 44;
            this.labelControl10.Text = "GÖRÜŞME YAPILAN KİŞİNİN GÖREVİ";
            // 
            // GorYapKisi
            // 
            this.GorYapKisi.Location = new System.Drawing.Point(204, 134);
            this.GorYapKisi.Name = "GorYapKisi";
            this.GorYapKisi.Size = new System.Drawing.Size(246, 20);
            this.GorYapKisi.TabIndex = 43;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(14, 137);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(117, 13);
            this.labelControl9.TabIndex = 42;
            this.labelControl9.Text = "GÖRÜŞME YAPILAN KİŞİ";
            // 
            // GorRanTarihi
            // 
            this.GorRanTarihi.EditValue = new System.DateTime(2011, 12, 18, 21, 45, 19, 0);
            this.GorRanTarihi.Location = new System.Drawing.Point(204, 92);
            this.GorRanTarihi.Name = "GorRanTarihi";
            this.GorRanTarihi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.GorRanTarihi.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.GorRanTarihi.Size = new System.Drawing.Size(103, 20);
            this.GorRanTarihi.TabIndex = 40;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(14, 95);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(137, 13);
            this.labelControl8.TabIndex = 41;
            this.labelControl8.Text = "GÖRÜŞME RANDEVU TARİHİ";
            // 
            // frmGorusmeDetayBilgileri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 314);
            this.Controls.Add(this.GorusmeSonucu);
            this.Controls.Add(this.labelControl13);
            this.Controls.Add(this.GorusmeNotu);
            this.Controls.Add(this.labelControl12);
            this.Controls.Add(this.GorTarihi);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.GorYapKisiGorev);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.GorYapKisi);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.GorRanTarihi);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmGorusmeDetayBilgileri";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmGorusmeDetayBilgileri";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pkProjeGorusme.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorusmeSonucu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorusmeNotu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorTarihi.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorTarihi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorYapKisiGorev.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorYapKisi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorRanTarihi.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GorRanTarihi.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton10;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton BtnKaydet;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.TextEdit GorusmeSonucu;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.MemoEdit GorusmeNotu;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.DateEdit GorTarihi;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.TextEdit GorYapKisiGorev;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.TextEdit GorYapKisi;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.DateEdit GorRanTarihi;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        public DevExpress.XtraEditors.TextEdit pkProjeGorusme;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}
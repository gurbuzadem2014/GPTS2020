namespace GPTS
{
    partial class frmYedekAl
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
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.button1 = new System.Windows.Forms.Button();
            this.simpleButton21 = new DevExpress.XtraEditors.SimpleButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.klasoryol = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnKlasorAc = new DevExpress.XtraEditors.SimpleButton();
            this.dateEdit2 = new DevExpress.XtraEditors.DateEdit();
            this.labelControl29 = new DevExpress.XtraEditors.LabelControl();
            this.gCDynamik = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcTedarikciler = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.klasoryol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gCDynamik)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTedarikciler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton3
            // 
            this.simpleButton3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.simpleButton3.Image = global::GPTS.Properties.Resources.ZipDrive;
            this.simpleButton3.Location = new System.Drawing.Point(302, 219);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(302, 76);
            this.simpleButton3.TabIndex = 0;
            this.simpleButton3.Text = "Yedek Al";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.button1);
            this.panelControl1.Controls.Add(this.simpleButton21);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(659, 62);
            this.panelControl1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Left;
            this.button1.Location = new System.Drawing.Point(2, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 58);
            this.button1.TabIndex = 1;
            this.button1.Text = "ftp\'ye Gönder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // simpleButton21
            // 
            this.simpleButton21.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton21.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton21.Location = new System.Drawing.Point(521, 2);
            this.simpleButton21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton21.Name = "simpleButton21";
            this.simpleButton21.Size = new System.Drawing.Size(136, 58);
            this.simpleButton21.TabIndex = 0;
            this.simpleButton21.Text = "Kapat [ESC]";
            this.simpleButton21.Click += new System.EventHandler(this.simpleButton21_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GPTS.Properties.Resources.Animation;
            this.pictureBox1.Location = new System.Drawing.Point(73, 148);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(62, 63);
            this.pictureBox1.TabIndex = 89;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // klasoryol
            // 
            this.klasoryol.EditValue = "";
            this.klasoryol.Location = new System.Drawing.Point(202, 113);
            this.klasoryol.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.klasoryol.Name = "klasoryol";
            this.klasoryol.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.klasoryol.Properties.Appearance.Options.UseFont = true;
            this.klasoryol.Size = new System.Drawing.Size(345, 29);
            this.klasoryol.TabIndex = 3;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(73, 121);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(117, 16);
            this.labelControl6.TabIndex = 13;
            this.labelControl6.Text = "Yedek Alınacak Yer :";
            this.labelControl6.Click += new System.EventHandler(this.labelControl6_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = global::GPTS.Properties.Resources.bul_find_24x24;
            this.simpleButton2.Location = new System.Drawing.Point(554, 111);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(50, 32);
            this.simpleButton2.TabIndex = 2;
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labelControl1.Location = new System.Drawing.Point(154, 166);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(373, 30);
            this.labelControl1.TabIndex = 90;
            this.labelControl1.Text = "Yedek Alınıyor Lütfen Bekleyiniz...";
            this.labelControl1.Visible = false;
            // 
            // btnKlasorAc
            // 
            this.btnKlasorAc.Enabled = false;
            this.btnKlasorAc.Image = global::GPTS.Properties.Resources.FilesFolder64;
            this.btnKlasorAc.Location = new System.Drawing.Point(73, 219);
            this.btnKlasorAc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnKlasorAc.Name = "btnKlasorAc";
            this.btnKlasorAc.Size = new System.Drawing.Size(222, 75);
            this.btnKlasorAc.TabIndex = 1;
            this.btnKlasorAc.Text = "Yedek Klasörünü Aç";
            this.btnKlasorAc.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // dateEdit2
            // 
            this.dateEdit2.EditValue = new System.DateTime(2011, 11, 27, 17, 59, 1, 0);
            this.dateEdit2.Location = new System.Drawing.Point(488, 308);
            this.dateEdit2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateEdit2.Name = "dateEdit2";
            this.dateEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Properties.DisplayFormat.FormatString = "HH:mm:ss";
            this.dateEdit2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit2.Properties.EditFormat.FormatString = "HH:mm:ss";
            this.dateEdit2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit2.Properties.Mask.EditMask = "HH:mm:ss";
            this.dateEdit2.Properties.MinValue = new System.DateTime(2011, 11, 27, 0, 0, 0, 0);
            this.dateEdit2.Properties.NullDate = new System.DateTime(2011, 11, 27, 23, 6, 9, 606);
            this.dateEdit2.Properties.NullText = "27.11.2011";
            this.dateEdit2.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit2.Size = new System.Drawing.Size(117, 22);
            this.dateEdit2.TabIndex = 92;
            // 
            // labelControl29
            // 
            this.labelControl29.Location = new System.Drawing.Point(407, 311);
            this.labelControl29.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl29.Name = "labelControl29";
            this.labelControl29.Size = new System.Drawing.Size(67, 16);
            this.labelControl29.TabIndex = 91;
            this.labelControl29.Text = "Yedek Saati";
            this.labelControl29.ToolTip = "Ürün üzerine yapıştırılan etiket üzerinde\r\ngizli fiyat yazmak için kullanılır\r\nBa" +
    "rkodun ilk 4 hanesi buradan girilir ve sabittir\r\n4. rakamı 0 olmalı (ayraç)\r\nÖr:" +
    "4320";
            // 
            // gCDynamik
            // 
            this.gCDynamik.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gCDynamik.Location = new System.Drawing.Point(14, 326);
            this.gCDynamik.MainView = this.gridView3;
            this.gCDynamik.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gCDynamik.Name = "gCDynamik";
            this.gCDynamik.Size = new System.Drawing.Size(149, 95);
            this.gCDynamik.TabIndex = 93;
            this.gCDynamik.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            this.gCDynamik.Visible = false;
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gCDynamik;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsView.ShowFooter = true;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // gcTedarikciler
            // 
            this.gcTedarikciler.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcTedarikciler.Location = new System.Drawing.Point(154, 326);
            this.gcTedarikciler.MainView = this.gridView1;
            this.gcTedarikciler.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcTedarikciler.Name = "gcTedarikciler";
            this.gcTedarikciler.Size = new System.Drawing.Size(233, 95);
            this.gcTedarikciler.TabIndex = 94;
            this.gcTedarikciler.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gcTedarikciler.Visible = false;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcTedarikciler;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioGroup1.EditValue = ((short)(0));
            this.radioGroup1.Location = new System.Drawing.Point(0, 62);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Columns = 2;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(0)), "Otomatik Yedek Yolu"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(1)), "Manuel Yedek Yolu")});
            this.radioGroup1.Size = new System.Drawing.Size(659, 41);
            this.radioGroup1.TabIndex = 95;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // frmYedekAl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 433);
            this.ControlBox = false;
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.gcTedarikciler);
            this.Controls.Add(this.gCDynamik);
            this.Controls.Add(this.dateEdit2);
            this.Controls.Add(this.labelControl29);
            this.Controls.Add(this.btnKlasorAc);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.klasoryol);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmYedekAl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yedek Al";
            this.Load += new System.EventHandler(this.frmYedekAl_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmYedekAl_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.klasoryol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gCDynamik)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTedarikciler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton21;
        private DevExpress.XtraEditors.TextEdit klasoryol;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private DevExpress.XtraEditors.SimpleButton btnKlasorAc;
        private DevExpress.XtraEditors.DateEdit dateEdit2;
        private DevExpress.XtraEditors.LabelControl labelControl29;
        private DevExpress.XtraGrid.GridControl gCDynamik;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.GridControl gcTedarikciler;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
    }
}
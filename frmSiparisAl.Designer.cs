namespace GPTS
{
    partial class frmSiparisAl
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
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.musteriadi = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.simpleButton12 = new DevExpress.XtraEditors.SimpleButton();
            this.lUEPersonel = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lUEPersonel.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = global::GPTS.Properties.Resources.Delete_32x32;
            this.simpleButton2.Location = new System.Drawing.Point(320, 287);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(138, 60);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Tag = "0";
            this.simpleButton2.Text = "Hayır";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = global::GPTS.Properties.Resources.camera_test;
            this.simpleButton1.Location = new System.Drawing.Point(56, 287);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(145, 60);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Tag = "1";
            this.simpleButton1.Text = "Evet";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // musteriadi
            // 
            this.musteriadi.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.musteriadi.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.musteriadi.Appearance.ForeColor = System.Drawing.Color.White;
            this.musteriadi.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.musteriadi.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.musteriadi.Dock = System.Windows.Forms.DockStyle.Top;
            this.musteriadi.Location = new System.Drawing.Point(0, 0);
            this.musteriadi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.musteriadi.Name = "musteriadi";
            this.musteriadi.Size = new System.Drawing.Size(545, 47);
            this.musteriadi.TabIndex = 5;
            this.musteriadi.Text = "Müşteri Adı";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Location = new System.Drawing.Point(48, 108);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(244, 31);
            this.labelControl1.TabIndex = 36;
            this.labelControl1.Text = "Teslim Alma Tarihi";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl3.Location = new System.Drawing.Point(161, 155);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(121, 31);
            this.labelControl3.TabIndex = 38;
            this.labelControl3.Text = "Açıklama";
            // 
            // memoEdit1
            // 
            this.memoEdit1.Location = new System.Drawing.Point(289, 150);
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(241, 119);
            this.memoEdit1.TabIndex = 39;
            // 
            // dateEdit1
            // 
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(289, 111);
            this.dateEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit1.Size = new System.Drawing.Size(241, 22);
            this.dateEdit1.TabIndex = 40;
            // 
            // simpleButton12
            // 
            this.simpleButton12.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton12.Appearance.Options.UseFont = true;
            this.simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton12.Image = global::GPTS.Properties.Resources.personel__32x32;
            this.simpleButton12.Location = new System.Drawing.Point(181, 64);
            this.simpleButton12.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton12.Name = "simpleButton12";
            this.simpleButton12.Size = new System.Drawing.Size(101, 32);
            this.simpleButton12.TabIndex = 42;
            this.simpleButton12.Tag = "0";
            this.simpleButton12.Text = "Personel";
            this.simpleButton12.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            // 
            // lUEPersonel
            // 
            this.lUEPersonel.Location = new System.Drawing.Point(289, 68);
            this.lUEPersonel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lUEPersonel.Name = "lUEPersonel";
            this.lUEPersonel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lUEPersonel.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pkpersoneller", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("adi", "Personel Adı")});
            this.lUEPersonel.Properties.DisplayMember = "adi";
            this.lUEPersonel.Properties.NullText = "Seçiniz";
            this.lUEPersonel.Properties.ValueMember = "pkpersoneller";
            this.lUEPersonel.Size = new System.Drawing.Size(241, 22);
            this.lUEPersonel.TabIndex = 41;
            this.lUEPersonel.Tag = "0";
            // 
            // frmSiparisAl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 362);
            this.Controls.Add(this.simpleButton12);
            this.Controls.Add(this.lUEPersonel);
            this.Controls.Add(this.dateEdit1);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.musteriadi);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSiparisAl";
            this.Tag = "0";
            this.Text = "Sipariş Al";
            this.Load += new System.EventHandler(this.frmSiparisAl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lUEPersonel.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl musteriadi;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton12;
        private DevExpress.XtraEditors.LookUpEdit lUEPersonel;
    }
}
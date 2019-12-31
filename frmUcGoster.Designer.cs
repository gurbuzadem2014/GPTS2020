namespace GPTS
{
    partial class frmUcGoster
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
            this.musteriadi = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton19 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.musteriadi);
            this.panelControl1.Controls.Add(this.simpleButton19);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1042, 65);
            this.panelControl1.TabIndex = 7;
            // 
            // musteriadi
            // 
            this.musteriadi.Dock = System.Windows.Forms.DockStyle.Left;
            this.musteriadi.Location = new System.Drawing.Point(2, 2);
            this.musteriadi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.musteriadi.Name = "musteriadi";
            this.musteriadi.Size = new System.Drawing.Size(0, 16);
            this.musteriadi.TabIndex = 23;
            // 
            // simpleButton19
            // 
            this.simpleButton19.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.simpleButton19.Appearance.Options.UseFont = true;
            this.simpleButton19.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton19.Image = global::GPTS.Properties.Resources.cancel;
            this.simpleButton19.Location = new System.Drawing.Point(908, 2);
            this.simpleButton19.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton19.Name = "simpleButton19";
            this.simpleButton19.Size = new System.Drawing.Size(132, 61);
            this.simpleButton19.TabIndex = 22;
            this.simpleButton19.Text = "Kapat [ESC]";
            this.simpleButton19.Click += new System.EventHandler(this.simpleButton19_Click);
            // 
            // frmUcGoster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 564);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmUcGoster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UcGoster";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmUcGoster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton19;
        public DevExpress.XtraEditors.LabelControl musteriadi;
    }
}
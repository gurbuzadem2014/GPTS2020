namespace GPTS
{
    partial class FormScreenKeyboard
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
            this.numpadButton1 = new GPTS.NumpadButton();
            this.numpadButton2 = new GPTS.NumpadButton();
            this.SuspendLayout();
            // 
            // numpadButton1
            // 
            this.numpadButton1.KeyValue = "1";
            this.numpadButton1.Location = new System.Drawing.Point(12, 12);
            this.numpadButton1.Name = "numpadButton1";
            this.numpadButton1.Size = new System.Drawing.Size(54, 54);
            this.numpadButton1.TabIndex = 217;
            this.numpadButton1.Text = "1";
            // 
            // numpadButton2
            // 
            this.numpadButton2.KeyValue = null;
            this.numpadButton2.Location = new System.Drawing.Point(85, 18);
            this.numpadButton2.Name = "numpadButton2";
            this.numpadButton2.Size = new System.Drawing.Size(54, 47);
            this.numpadButton2.TabIndex = 218;
            this.numpadButton2.Text = "numpadButton2";
            // 
            // FormScreenKeyboard
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 334);
            this.Controls.Add(this.numpadButton2);
            this.Controls.Add(this.numpadButton1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormScreenKeyboard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Sanal Klavye";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private NumpadButton numpadButton1;
        private NumpadButton numpadButton2;
    }
}
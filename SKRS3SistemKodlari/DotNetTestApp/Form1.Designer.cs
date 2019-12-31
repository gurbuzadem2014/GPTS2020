namespace DotNetTestApp
{
    partial class FrmSkrsKayitlari
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
            this.dgvSistemler = new System.Windows.Forms.DataGridView();
            this.colAdi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKodu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSistemKodlari = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSistemKodlariniGetir = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSistemler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSistemKodlari)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSistemler
            // 
            this.dgvSistemler.AllowUserToAddRows = false;
            this.dgvSistemler.AllowUserToDeleteRows = false;
            this.dgvSistemler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSistemler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSistemler.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAdi,
            this.colKodu});
            this.dgvSistemler.Location = new System.Drawing.Point(12, 37);
            this.dgvSistemler.MultiSelect = false;
            this.dgvSistemler.Name = "dgvSistemler";
            this.dgvSistemler.ReadOnly = true;
            this.dgvSistemler.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvSistemler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSistemler.Size = new System.Drawing.Size(855, 150);
            this.dgvSistemler.TabIndex = 0;
            this.dgvSistemler.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSistemler_CellContentClick);
            // 
            // colAdi
            // 
            this.colAdi.DataPropertyName = "adi";
            this.colAdi.HeaderText = "Adı";
            this.colAdi.Name = "colAdi";
            this.colAdi.ReadOnly = true;
            // 
            // colKodu
            // 
            this.colKodu.DataPropertyName = "kodu";
            this.colKodu.HeaderText = "Kodu";
            this.colKodu.Name = "colKodu";
            this.colKodu.ReadOnly = true;
            // 
            // dgvSistemKodlari
            // 
            this.dgvSistemKodlari.AllowUserToAddRows = false;
            this.dgvSistemKodlari.AllowUserToDeleteRows = false;
            this.dgvSistemKodlari.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSistemKodlari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSistemKodlari.Location = new System.Drawing.Point(12, 243);
            this.dgvSistemKodlari.MultiSelect = false;
            this.dgvSistemKodlari.Name = "dgvSistemKodlari";
            this.dgvSistemKodlari.ReadOnly = true;
            this.dgvSistemKodlari.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvSistemKodlari.Size = new System.Drawing.Size(855, 191);
            this.dgvSistemKodlari.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sistemler :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(13, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sistem Kodları : ";
            // 
            // btnSistemKodlariniGetir
            // 
            this.btnSistemKodlariniGetir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSistemKodlariniGetir.Location = new System.Drawing.Point(764, 193);
            this.btnSistemKodlariniGetir.Name = "btnSistemKodlariniGetir";
            this.btnSistemKodlariniGetir.Size = new System.Drawing.Size(103, 44);
            this.btnSistemKodlariniGetir.TabIndex = 4;
            this.btnSistemKodlariniGetir.Text = "Sistem Kodlarını Getir";
            this.btnSistemKodlariniGetir.UseVisualStyleBackColor = true;
            this.btnSistemKodlariniGetir.Click += new System.EventHandler(this.btnSistemKodlariniGetir_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(439, 193);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 44);
            this.button1.TabIndex = 5;
            this.button1.Text = "Excel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(123, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 29);
            this.button2.TabIndex = 6;
            this.button2.Text = "Sistemleri Getir";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(308, 193);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(125, 44);
            this.button3.TabIndex = 7;
            this.button3.Text = "Lookup Tablosuna At";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(655, 193);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(103, 44);
            this.button4.TabIndex = 8;
            this.button4.Text = "Sistem Kodlarını Getir (Sayfa)";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(546, 193);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(103, 44);
            this.button5.TabIndex = 9;
            this.button5.Text = "Sistem Kodlarını Getir (Sayfa) Excel";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // FrmSkrsKayitlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 446);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSistemKodlariniGetir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvSistemKodlari);
            this.Controls.Add(this.dgvSistemler);
            this.Name = "FrmSkrsKayitlari";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SKRS 3 Kayıtları";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmSkrsKayitlari_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSistemler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSistemKodlari)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSistemler;
        private System.Windows.Forms.DataGridView dgvSistemKodlari;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSistemKodlariniGetir;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKodu;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}


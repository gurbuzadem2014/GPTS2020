using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class frmGorevYeniKarti : DevExpress.XtraEditors.XtraForm
    {
        public frmGorevYeniKarti()
        {
            InitializeComponent();
        }

        private void frmStokKoduverKarti_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            switch (this.Tag.ToString())
            {
                case "1":
                    {
                        list.Add(new SqlParameter("@bolumadi", GrupAdi.Text));
                        DB.ExecuteSQL("INSERT INTO Bolumler (bolumadi) VALUES(@bolumadi)", list);
                        break;
                    }
            }           
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void frmStokKoduverKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this.Tag.ToString())
            {
                case "1":
                    {
                        this.Text = "Yeni Stok Grup Kartı";
                        xtraTabPage1.PageVisible = true;
                        xtraTabPage1.Text = "Yeni Grup Adı";
                        GrupAdi.Focus();
                        break;
                    }
                case "2":
                    {
                        this.Text = "Yeni Stok Alt Grup Kartı";
                        xtraTabPage1.PageVisible = false;
                        break;
                    }
                case "3":
                    {
                        this.Text = "Yeni Tedarikçi Grup Kartı";
                        xtraTabPage1.PageVisible = false;
                        break;
                    }
                case "4":
                    {
                        this.Text = "Yeni Tedarikçi Alt Grup Kartı";
                        xtraTabPage1.PageVisible = false;
                        break;
                    }
            }           
            timer1.Enabled = false;
        }
    }
}
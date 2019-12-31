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
    public partial class frmYeniMarkaYeniKarti : DevExpress.XtraEditors.XtraForm
    {
        public frmYeniMarkaYeniKarti()
        {
            InitializeComponent();
        }

        private void frmStokKoduverKarti_Load(object sender, EventArgs e)
        {
            if (this.Tag == "M") // Marka
            {
                xtraTabControl1.SelectedTabPageIndex = 0;
                MarkaAdi.Focus();
            }
            if (this.Tag == "R") // Renk
            {
                xtraTabControl1.SelectedTabPageIndex = 1;
                MarkaAdi.Focus();
            }
            if (this.Tag == "B") // Beden
            {
                xtraTabControl1.SelectedTabPageIndex = 2;
                MarkaAdi.Focus();
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
           ArrayList list = new ArrayList();
           list.Add(new SqlParameter("@Marka", MarkaAdi.Text));
           DB.ExecuteSQL("INSERT INTO Markalar (Marka) VALUES(@Marka)", list);       
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Aciklama", RenkAdi.Text));
            DB.ExecuteSQL("INSERT INTO RenkGrupKodu (Aciklama) VALUES(@Aciklama)", list);
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Aciklama", Beden.Text));
            DB.ExecuteSQL("INSERT INTO BedenGrupKodu (Aciklama) VALUES(@Aciklama)", list);
            Close();
            
        }
    }
}
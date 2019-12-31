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
    public partial class frmMusteriGrupKarti : DevExpress.XtraEditors.XtraForm
    {
        public frmMusteriGrupKarti()
        {
            InitializeComponent();
        }

        private void frmStokKoduverKarti_Load(object sender, EventArgs e)
        {
            GrupAdi.Text = "";
            BtnKaydet.Enabled = true;
            if (this.Tag.ToString() == "0")
            {
                xtraTabPage1.PageVisible=true;
                xtraTabPage2.PageVisible = false;
            }
            if (this.Tag.ToString() == "2")
            {
                xtraTabPage1.PageVisible = false;
                xtraTabPage2.PageVisible = true;
            }
        }
        //void bilgileri()
        //{
        //    DataTable dt = DB.GetData("select * from FirmaGruplari where pkFirmaGruplari=" + pkFirmaGruplari.Text);
        //    if (dt.Rows.Count > 0)
        //    {
        //        GrupAdi.Text = dt.Rows[0]["GrupAdi"].ToString();
        //    }
        //}
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
            {
                if (GrupAdi.Text == "")
                {
                    frmMesajBox mesaj = new frmMesajBox(200);
                    mesaj.label1.Text = "Müşteri Grup Adı Boş Olmaz";
                    mesaj.Show();
                    return;
                }
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@GrupAdi", GrupAdi.Text));
                DB.ExecuteSQL("INSERT INTO FirmaGruplari (GrupAdi,Aktif) VALUES(@GrupAdi,1)", list);
            }
            //alt gruplar
            if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
            {
                if (AltGrupAdi.Text == "")
                {
                    frmMesajBox mesaj = new frmMesajBox(200);
                    mesaj.label1.Text = "Müşteri Alt Grup Adı Boş Olmaz";
                    mesaj.Show();
                    return;
                }
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirmaGruplari", grupid.Tag.ToString()));
                list.Add(new SqlParameter("@FirmaAltGrupAdi", AltGrupAdi.Text));
                DB.ExecuteSQL("INSERT INTO FirmaAltGruplari (fkFirmaGruplari,FirmaAltGrupAdi,Aktif) VALUES(@fkFirmaGruplari,@FirmaAltGrupAdi,1)", list);
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
        private void sRakam_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                cbVarsayilan.Focus();
        }

        private void cbVarsayilan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.Focus();
        }

        private void cbVarsayilan_CheckedChanged(object sender, EventArgs e)
        {
            //DB.ExecuteSQL("UPDATE Kodver SET AktifSec=0");
           // DB.ExecuteSQL("UPDATE Kodver SET AktifSec=1 WHERE pkKodver=" + pkFirmaGruplari.Text);
        }
    }
}
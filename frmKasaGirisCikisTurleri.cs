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
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmKasaGirisCikisTurleri : DevExpress.XtraEditors.XtraForm
    {
        string fkKasaGirisCikisTurleri = "0";
        public frmKasaGirisCikisTurleri(string pkKasaGirisCikisTurleri)
        {
            InitializeComponent();
            fkKasaGirisCikisTurleri = pkKasaGirisCikisTurleri;
            
        }
        private void frmKasaGirisCikisTurleri_Load(object sender, EventArgs e)
        {
            if (fkKasaGirisCikisTurleri == "0")
            {
                BtnKaydet.Text = "Kaydet";
            }
            else
            {
                BtnKaydet.Text = "Güncelle";
            }
            if (this.Tag.ToString() == ((int)Degerler.GelirGider.Gelir).ToString())
            {
                ceGelirMi.Visible = true;
                ceGelirMi.Checked = true;
                ceGiderMi.Visible = false;
            }
            else
            {
                ceGelirMi.Visible = false;
                ceGiderMi.Visible = true;
                ceGiderMi.Checked = true;
            }

            DataTable dt = DB.GetData("select * from KasaGirisCikisTurleri kgct with(nolock) "+
                " where pkKasaGirisCikisTurleri=" + fkKasaGirisCikisTurleri);
            if (dt.Rows.Count > 0)
            {
                Aciklama.Text = dt.Rows[0]["Aciklama"].ToString();

                if(dt.Rows[0]["GiderOlarakisle"].ToString()=="True")
                    ceGiderMi.Checked=true;
                if (dt.Rows[0]["GelirOlarakisle"].ToString() == "True")
                    ceGelirMi.Checked = true;
                if (dt.Rows[0]["Aktif"].ToString() == "True")
                    ceAktif.Checked = true;
                else
                    ceAktif.Checked = false;
                int fkKasaGirisCikisGruplari=0;
                int.TryParse(dt.Rows[0]["fkKasaGirisCikisGruplari"].ToString(), out fkKasaGirisCikisGruplari);

                lueKasaHareketGrup.EditValue = fkKasaGirisCikisGruplari;
            }

            KasaGirisCikisGruplariGetir();
        }
        void KasaGirisCikisGruplariGetir()
        {
            string sql = "select * from KasaGirisCikisGruplari with(nolock) where Aktif=1";
            if (this.Tag.ToString() == ((int)Degerler.GelirGider.Gelir).ToString())
                sql = sql + " and Gelir=1";
            else
                sql = sql + " and Gider=1";

            lueKasaHareketGrup.Properties.DataSource = DB.GetData(sql);
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();

            list.Add(new SqlParameter("@Aciklama", Aciklama.Text));

            if (this.Tag.ToString() == "1")
                list.Add(new SqlParameter("@GirisCikis", true));
            else
                list.Add(new SqlParameter("@GirisCikis", false));

            if (ceGelirMi.Checked == true && ceGelirMi.Visible==true)
            {
                list.Add(new SqlParameter("@GelirOlarakisle", "1"));
                list.Add(new SqlParameter("@GiderOlarakisle", "0"));
            }
            else if (ceGiderMi.Checked == true && ceGiderMi.Visible == true)
            {
                list.Add(new SqlParameter("@GelirOlarakisle", "0"));
                list.Add(new SqlParameter("@GiderOlarakisle", "1"));
            }
            else
            {
                list.Add(new SqlParameter("@GelirOlarakisle", "0"));
                list.Add(new SqlParameter("@GiderOlarakisle", "0"));
            }
            list.Add(new SqlParameter("@Aktif", ceAktif.Checked));

            if (lueKasaHareketGrup.EditValue==null)
                list.Add(new SqlParameter("@fkKasaGirisCikisGruplari", DBNull.Value));
            else
                list.Add(new SqlParameter("@fkKasaGirisCikisGruplari", lueKasaHareketGrup.EditValue.ToString()));
            

            string sql = @"insert into KasaGirisCikisTurleri (Aciklama,GirisCikis,GelirOlarakisle,GiderOlarakisle,Aktif,fkKasaGirisCikisGruplari)
                         values(@Aciklama,@GirisCikis,@GelirOlarakisle,@GiderOlarakisle,@Aktif,@fkKasaGirisCikisGruplari)";

            if(fkKasaGirisCikisTurleri != "0")
                sql = @"update KasaGirisCikisTurleri set Aciklama=@Aciklama,GirisCikis=@GirisCikis,GelirOlarakisle=@GelirOlarakisle,
                GiderOlarakisle=@GiderOlarakisle,Aktif=@Aktif,fkKasaGirisCikisGruplari=@fkKasaGirisCikisGruplari where pkKasaGirisCikisTurleri=" + fkKasaGirisCikisTurleri;

            string sonuc = DB.ExecuteSQL(sql, list);
            if (sonuc == "0")
                Close();
            else
                MessageBox.Show("Hata Oluştu : " + sonuc);
        }
        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmKasaGirisCikisTurleri_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
                BtnKaydet_Click(sender, e);

        }

        private void simpleButton28_Click(object sender, EventArgs e)
        {
            frmKasaGirisCikisGruplari KasaGirisCikisGruplari = new frmKasaGirisCikisGruplari("0");
            KasaGirisCikisGruplari.Tag = this.Tag;
            KasaGirisCikisGruplari.ShowDialog();
            KasaGirisCikisGruplariGetir();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            frmKasaGirisCikisGrupListesi KasaGirisCikisGrupListesi = new frmKasaGirisCikisGrupListesi();
            KasaGirisCikisGrupListesi.Tag = this.Tag;
            KasaGirisCikisGrupListesi.ShowDialog();
            KasaGirisCikisGruplariGetir();
        }
    }
}
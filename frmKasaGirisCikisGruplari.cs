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
using GPTS.islemler;

namespace GPTS
{
    public partial class frmKasaGirisCikisGruplari : DevExpress.XtraEditors.XtraForm
    {
        string pkKasaGirisCikisGruplari = "0";
        public frmKasaGirisCikisGruplari(string fkKasaGirisCikisGruplari)
        {
            InitializeComponent();
            pkKasaGirisCikisGruplari = fkKasaGirisCikisGruplari;
        }

        private void frmStokKoduverKarti_Load(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from KasaGirisCikisGruplari where pkKasaGirisCikisGruplari="+pkKasaGirisCikisGruplari);
            
            if (dt.Rows.Count == 0) return;

            GrupAdi.Text = dt.Rows[0]["GrupAdi"].ToString();

            if (dt.Rows[0]["Aktif"].ToString() == "False")
                cbAktif.Checked = false;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@GrupAdi", GrupAdi.Text));
            list.Add(new SqlParameter("@Aktif", cbAktif.Checked));

            if (this.Tag.ToString() == "1")
            {
                list.Add(new SqlParameter("@Gelir", "1"));
                list.Add(new SqlParameter("@Gider", "0"));
            }
            else
            {
                list.Add(new SqlParameter("@Gelir", "0"));
                list.Add(new SqlParameter("@Gider", "1"));
            }
            string sonuc = "0";
            if (pkKasaGirisCikisGruplari == "0")
                sonuc = DB.ExecuteSQL("INSERT INTO KasaGirisCikisGruplari (GrupAdi,Aktif,Gelir,Gider) VALUES(@GrupAdi,1,@Gelir,@Gider)", list);
            else
            {
                list.Add(new SqlParameter("@pkKasaGirisCikisGruplari", pkKasaGirisCikisGruplari));
                sonuc = DB.ExecuteSQL("UPDATE KasaGirisCikisGruplari SET GrupAdi=@GrupAdi,Aktif=@Aktif,Gelir=@Gelir,Gider=@Gider where pkKasaGirisCikisGruplari=@pkKasaGirisCikisGruplari", list);
            }
            if (sonuc != "0")
                formislemleri.Mesajform(sonuc, "R", 200);
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
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
    public partial class frmStokDevirDuzenle : DevExpress.XtraEditors.XtraForm
    {
        public frmStokDevirDuzenle()
        {
            InitializeComponent();
        }

        private void frmKasaHareketDuzelt_Load(object sender, EventArgs e)
        {
            Depolar();

            DataTable dt = DB.GetData(@"select * from StokDevir with(nolock) where pkStokDevir=" + pkStokDevir.Text);
            if (dt.Rows.Count == 0) return;

            //string fkKullanicilar = dt.Rows[0]["fkKullanicilar"].ToString();
            string fkDepolar = dt.Rows[0]["fkDepolar"].ToString();
            string DevirAdedi = dt.Rows[0]["DevirAdedi"].ToString();
            txtAciklama.Text = dt.Rows[0]["Aciklama"].ToString();

            islemtarihi.DateTime = Convert.ToDateTime(dt.Rows[0]["Tarih"].ToString());
            ceDevirAdedi.Value = Decimal.Parse(DevirAdedi);
            txtOncekiAdet.Text = dt.Rows[0]["OncekiAdet"].ToString();
             

            int _Depolar = 0;
            int.TryParse(fkDepolar, out _Depolar);
            lueDepolar.EditValue = _Depolar;
        }

        void Depolar()
        {
            lueDepolar.Properties.DataSource = DB.GetData("select pkDepolar,DepoAdi from Depolar with(nolock) where Aktif=1");
            lueDepolar.ItemIndex = 0;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //if(lueKasalar.EditValue.ToString()=="0")
            //{
            //    MessageBox.Show("Kasa Seçiniz");
            //    lueKasalar.Focus();
            //    return;
            //}

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkStokDevir", pkStokDevir.Text));
            list.Add(new SqlParameter("@Tarih", islemtarihi.DateTime));
            list.Add(new SqlParameter("@DevirAdedi", ceDevirAdedi.Value));
            list.Add(new SqlParameter("@Aciklama", txtAciklama.Text));
            list.Add(new SqlParameter("@fkDepolar", lueDepolar.EditValue.ToString()));
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
          
            string sonuc=
            DB.ExecuteSQL(@"UPDATE StokDevir SET Tarih=@Tarih,Aciklama=@Aciklama,DevirAdedi=@DevirAdedi,
            fkDepolar=@fkDepolar,fkKullanicilar=@fkKullanicilar where pkStokDevir=@pkStokDevir", list);
            if (sonuc == "0")
                Close();
            else
                MessageBox.Show("Hata Oluştu Bilgileri Kontrol Ediniz" +  sonuc);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmKasaHareketDuzelt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void pkStokDevir_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
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
    public partial class frmKasaHareketDuzelt : DevExpress.XtraEditors.XtraForm
    {
        public frmKasaHareketDuzelt()
        {
            InitializeComponent();
        }

        private void frmKasaHareketDuzelt_Load(object sender, EventArgs e)
        {
            kasalar();

            DataTable dt = DB.GetData(@"select * from KasaHareket kh with(nolock) where pkKasaHareket=" + pkKasaHareket.Text);
            if (dt.Rows.Count == 0) return;

            string fkFirma = dt.Rows[0]["fkFirma"].ToString();
            string fkTedarikciler = dt.Rows[0]["fkTedarikciler"].ToString();
            string fkPersoneller = dt.Rows[0]["fkPersoneller"].ToString();

            string fkKasaGirisCikisTurleri = dt.Rows[0]["fkKasaGirisCikisTurleri"].ToString();
            tEaciklama.Text = dt.Rows[0]["Aciklama"].ToString();
            txtMakbuzNo.Text = dt.Rows[0]["MakbuzNo"].ToString();
            txtOdemeSekli.Text = dt.Rows[0]["OdemeSekli"].ToString();

            if (dt.Rows[0]["AktifHesap"].ToString() == "True")
                cbKasayaisle.Checked = true;
            else
                cbKasayaisle.Checked = false;

            if (dt.Rows[0]["GelirOlarakisle"].ToString() == "True")
                ceGelirMi.Checked = true;
            else
                ceGelirMi.Checked = false;

            if (dt.Rows[0]["GiderOlarakisle"].ToString() == "True")
                ceGiderMi.Checked = true;
            else
                ceGiderMi.Checked = false;

                islemtarihi.DateTime = Convert.ToDateTime(dt.Rows[0]["Tarih"].ToString());
                ceBorc.Value = Decimal.Parse(dt.Rows[0]["Borc"].ToString().Replace(".",","));
                ceAlacak.Value = Decimal.Parse(dt.Rows[0]["Alacak"].ToString().Replace(".", ","));
                        //Ödeme Girişi
                if (ceBorc.Value > 0)
                {
                    ceBorc.Enabled = true;
                    ceAlacak.Enabled = false;
                }
                else
                {
                    ceAlacak.Enabled = true;
                    ceBorc.Enabled = false;
                }

            int _kasaid = 0;
            int.TryParse(dt.Rows[0]["fkKasalar"].ToString(),out _kasaid);
            lueKasalar.EditValue = _kasaid;

            if(dt.Rows[0]["OdemeSekli"].ToString()== "Nakit")
                lueKasalar.Enabled = true;
            else
                lueKasalar.Enabled = false;
        }

        void kasalar()
        {
            lueKasalar.Properties.DataSource = DB.GetData("select 0 as pkKasalar, 'Tümü' as KasaAdi " +
                "union all select pkKasalar,KasaAdi from Kasalar with(nolock) where Aktif=1");
            lueKasalar.ItemIndex = 0;
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
            list.Add(new SqlParameter("@Borc", ceBorc.Value));
            list.Add(new SqlParameter("@Alacak", ceAlacak.Value));
            list.Add(new SqlParameter("@Tarih", islemtarihi.DateTime));
            list.Add(new SqlParameter("@pkKasaHareket", pkKasaHareket.Text));
            list.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list.Add(new SqlParameter("@MakbuzNo", txtMakbuzNo.Text));
            list.Add(new SqlParameter("@AktifHesap", cbKasayaisle.Checked));
            list.Add(new SqlParameter("@fkKasalar", lueKasalar.EditValue.ToString()));
            list.Add(new SqlParameter("@GiderOlarakisle", ceGiderMi.Checked));
            list.Add(new SqlParameter("@GelirOlarakisle", ceGelirMi.Checked));
          
            string sonuc=
            DB.ExecuteSQL(@"UPDATE KasaHareket SET Borc=@Borc,Alacak=@Alacak,Tarih=@Tarih,Aciklama=@Aciklama,
            AktifHesap=@AktifHesap,GiderOlarakisle=@GiderOlarakisle,GelirOlarakisle=@GelirOlarakisle,
            fkKasalar=@fkKasalar,MakbuzNo=@MakbuzNo,guncelleme_tarihi=getdate() where pkKasaHareket=@pkKasaHareket", list);
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
    }
}
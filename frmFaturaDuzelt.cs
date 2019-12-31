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
    public partial class frmFaturaDuzelt : DevExpress.XtraEditors.XtraForm
    {
        string _fkSatislar="0";
        public frmFaturaDuzelt(string fkSatislar)
        {
            InitializeComponent();
            _fkSatislar = fkSatislar;
        }

        private void frmKasaHareketDuzelt_Load(object sender, EventArgs e)
        {
            if (_fkSatislar == "")
            {
                MessageBox.Show("Satış Seçiniz...");
                Close();
                return;
            }

            DataTable dt = DB.GetData(@"select * from Satislar s with(nolock) where pkSatislar=" + _fkSatislar);

            CariAdi.Text = dt.Rows[0]["fkFirma"].ToString();
            string _FaturaTarihi = dt.Rows[0]["FaturaTarihi"].ToString();
            txtFaturaNo.Text = dt.Rows[0]["FaturaNo"].ToString();
            string _ToplamTutar = dt.Rows[0]["ToplamTutar"].ToString();
            if (_ToplamTutar == "") _ToplamTutar = "0";
            tEaciklama.Text = dt.Rows[0]["Aciklama"].ToString();

            if(_FaturaTarihi!="")
             deFaturaTarihi.DateTime = Convert.ToDateTime(_FaturaTarihi);

            ceFaturaTutar.Value = Decimal.Parse(_ToplamTutar.Replace(".",","));

        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ToplamTutar", ceFaturaTutar.Value));
            if(deFaturaTarihi.EditValue==null)
                list.Add(new SqlParameter("@FaturaTarihi", DBNull.Value));
            else
                list.Add(new SqlParameter("@FaturaTarihi", deFaturaTarihi.DateTime));
            list.Add(new SqlParameter("@pkSatislar", _fkSatislar));
            list.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list.Add(new SqlParameter("@FaturaNo", txtFaturaNo.Text));

            string sonuc=
            DB.ExecuteSQL(@"UPDATE Satislar SET ToplamTutar=@ToplamTutar,FaturaTarihi=@FaturaTarihi,
            Aciklama=@Aciklama,FaturaNo=@FaturaNo  where pkSatislar=@pkSatislar", list);
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
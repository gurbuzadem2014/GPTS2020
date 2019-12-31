using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Collections;
using GPTS.Include.Data;
using System.Threading;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmAktarimGonderAlKontrol : DevExpress.XtraEditors.XtraForm
    {
        public frmAktarimGonderAlKontrol()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmWebAyarlari_Load(object sender, EventArgs e)
        {
            //AyarlariGetir();
            //xtraTabC.SelectedTabPage = xTabPAyarlar;
             simpleButton10_Click(sender, e);
             simpleButton12_Click(sender, e);
        }

        private void SatislariWebeGonder()
        {
            //-- null ise insert 0 ise güncelle 1 ise gönderildi
            DataTable dt = DB.GetData("select top 100 * from Satislar with(nolock)  where GonderildiWS is null ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", Convert.ToDateTime(dt.Rows[i]["Tarih"].ToString())));
                list.Add(new SqlParameter("@fkFirma", dt.Rows[i]["fkFirma"].ToString()));
                list.Add(new SqlParameter("@fkSatisDurumu", dt.Rows[i]["fkSatisDurumu"].ToString()));
                list.Add(new SqlParameter("@Aciklama", dt.Rows[i]["Aciklama"].ToString()));

                string sql = @"insert into Satislar (Tarih,fkFirma,fkSatisDurumu,Aciklama)
                    values(@Tarih,@fkFirma,@fkSatisDurumu,@Aciklama)";

               string sonuc = DBWeb.ExecuteSQL_Web(sql, list);
                if (sonuc == "0")
                    DB.ExecuteSQL("update Satislar set GonderildiWS=1 where pkSatislar=" + dt.Rows[i]["pkSatislar"].ToString());
            }  
        }
       
        private void SatisDetayWebeGonder()
        {
            //-- null ise insert 0 ise güncelle 1 ise gönderildi
            DataTable dt = DB.GetData("select top 100 * from SatisDetay with(nolock)  where GonderildiWS is null ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkSatislar", dt.Rows[i]["fkSatislar"].ToString()));
                list.Add(new SqlParameter("@Tarih", Convert.ToDateTime(dt.Rows[i]["Tarih"].ToString())));
                list.Add(new SqlParameter("@fkStokKarti", dt.Rows[i]["fkStokKarti"].ToString()));
                list.Add(new SqlParameter("@Adet", dt.Rows[i]["Adet"].ToString()));
                list.Add(new SqlParameter("@SatisFiyati", dt.Rows[i]["SatisFiyati"].ToString().Replace(",",".")));

                string sql = @"insert into SatisDetay (Tarih,fkSatislar,fkStokKarti,Adet,SatisFiyati)
                    values(@Tarih,@fkSatislar,@fkStokKarti,@Adet,@SatisFiyati)";

                string sonuc = DBWeb.ExecuteSQL_Web(sql, list);
                if (sonuc == "0")
                    DB.ExecuteSQL("update SatisDetay set GonderildiWS=1 where pkSatisDetay=" + dt.Rows[i]["pkSatisDetay"].ToString());
            }

        }

        void SatislarUzakOnay()
        {
            gridControlOnay.DataSource = DB.GetData(@"select  s.*,f.Firmaadi,f.OzelKod from Satislar s with(nolock)
            left join Firmalar f with(nolock) on f.pkFirma=s.fkFirma where s.BilgisayarAdi='Aktarım'");
        }

        void KasaHareketUzakOnay()
        {
             gridControlTahsilat.DataSource = DB.GetData(@"select kh.*,f.Firmaadi,f.OzelKod from KasaHareket kh with(nolock)
             left join Firmalar f with(nolock) on f.pkFirma=kh.fkFirma
             where fkSatislar is null and kh.BilgisayarAdi='Aktarım'");
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xTabPOnay)
            {
                SatislarUzakOnay();
                KasaHareketUzakOnay();
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridViewOnay.DataRowCount; i++)
			{
                DataRow dr = gridViewOnay.GetDataRow(i);

                string pkSatislar = dr["pkSatislar"].ToString();
                DB.ExecuteSQL("Update Satislar set BilgisayarAdi='"+ Degerler.BilgisayarAdi +"' where pkSatislar=" + pkSatislar);
			}
            SatislarUzakOnay();

            satistahsilatgetir();
        }
        void satistahsilatgetir()
        {
            decimal aratop = 0;
            if (gridColumn16.SummaryItem.SummaryValue != null)
                aratop = decimal.Parse(gridColumn16.SummaryItem.SummaryValue.ToString());
            ceSatisTahsilat.Value = aratop;
        }
        private void simpleButton10_Click(object sender, EventArgs e)
        {
            SatislarUzakOnay();

            satistahsilatgetir();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridViewTahsilat.DataRowCount; i++)
            {
                DataRow dr = gridViewTahsilat.GetDataRow(i);

                string pkKasaHareket = dr["pkKasaHareket"].ToString();
                DB.ExecuteSQL("Update KasaHareket set BilgisayarAdi='" + Degerler.BilgisayarAdi + "' where pkKasaHareket=" + pkKasaHareket);
            }
            KasaHareketUzakOnay();

            tahsilatgetir();
        }

        void BakiyeGuncelle()
        {
            DataTable dtUzak = DBWeb.GetData_Web("select * from Firmalar with(nolock)");// where isnull(Aktarildi,0)=0");

            if (dtUzak.Rows.Count == 0) return;

            foreach (DataRow dr in dtUzak.Rows)
            {
                string pkFirma = dr["pkFirma"].ToString();
                string Devir = dr["Devir"].ToString();
                DB.ExecuteSQL("update Firmalar set Devir=" + Devir.Replace(",", ".") + " where pkFirma=" + pkFirma);
            }
            //listBoxControl1.Items.Add("Bakiyeler Güncellendi");
            formislemleri.Mesajform("Bakiyeler Güncellendi", "S", 200);
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridViewOnay.FocusedRowHandle < 0) return;
            DataRow dr = gridViewOnay.GetDataRow(gridViewOnay.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            simpleButton11_Click(sender, e);
            simpleButton13_Click(sender, e);

            tahsilatgetir();

            satistahsilatgetir();
        }

        private void gridViewOnay_DoubleClick(object sender, EventArgs e)
        {
            //gridControl çift tıkda var 
            if (gridViewOnay.FocusedRowHandle < 0) return;
            DataRow dr = gridViewOnay.GetDataRow(gridViewOnay.FocusedRowHandle);

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(true);
            FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        void tahsilatgetir()
        {
            decimal aratop = 0;
            if (gridColumn42.SummaryItem.SummaryValue != null)
                aratop = decimal.Parse(gridColumn42.SummaryItem.SummaryValue.ToString());
            ceTahsilat.Value = aratop;
        }
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            KasaHareketUzakOnay();

            tahsilatgetir();
        }

        private void ceTahsilat_EditValueChanged(object sender, EventArgs e)
        {
            ceToplam.Value = ceSatisTahsilat.Value + ceTahsilat.Value;
        }

        private void ceSatisTahsilat_EditValueChanged(object sender, EventArgs e)
        {
            ceToplam.Value = ceSatisTahsilat.Value + ceTahsilat.Value;
        }

        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridViewTahsilat.FocusedRowHandle < 0) return;

            DataRow dr = gridViewTahsilat.GetDataRow(gridViewTahsilat.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = "select * from AktarimHareketleri with(nolock)";
            gridControl1.DataSource=DB.GetData(sql);
        }
    }
}
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
    public partial class frmSatisMusteriEkrani : DevExpress.XtraEditors.XtraForm
    {
        string fis_no = "0";
        public frmSatisMusteriEkrani(string _pkSatislar)
        {
            InitializeComponent();
            fis_no = _pkSatislar;
        }

        private void frmbankalar_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string sonfis = "0";
            DataTable dt = DB.GetData("select top 1 pkSatislar from Satislar s with(nolock) where isnull(s.Siparis,0)=0 and s.fkKullanici=" + 
                DB.fkKullanicilar +" order by 1 desc");

            if (dt.Rows.Count > 0)
            sonfis = dt.Rows[0][0].ToString();

            lbFisNo.Text = "Fiş No=" + sonfis;
            gridView2.ViewCaption = "Müşteri Fiş No = " + sonfis;
            lblZaman.Text = DateTime.Now.ToString();

            string sqls = @"SELECT 

    sd.isKdvHaric,
    sd.pkSatisDetay,
    sd.fkStokKarti,

	case when sd.isKdvHaric = 1 then
      round((sk.SatisFiyatiKdvHaric * sd.iskontoyuzdetutar) / 100, 6)
	else

      round((sd.SatisFiyati * sd.iskontoyuzdetutar) / 100, 6)

    end iskontotutar,
    sd.iskontotutar as iskonto_tutar,
    sd.iskontoyuzdetutar,
    sk.pkStokKarti,
    sk.Stokadi,
    sk.Barcode,
    sk.Stoktipi,
    sk.StokKod,
    isnull(sd.KdvOrani, sk.KdvOrani) as KdvOrani,
    sd.AlisFiyati,
    sd.Adet,
    sd.NakitFiyat as NakitFiyati,
    sd.SatisFiyati,
    sk.KutuFiyat,
    0 as koliicitanefiyati,
    isnull(iade, 0) as iade,
    pkStokKartiid,
    sk.Mevcut,
    sk.KritikMiktar,
    
    case when sd.isKdvHaric = 1 then
      round(sd.Adet * ((sd.SatisFiyatiKdvHaric * sd.iskontoyuzdetutar) / 100), 6)
    else
      round(sd.Adet * ((sd.SatisFiyati * sd.iskontoyuzdetutar) / 100), 6)
    end  iskontotutaradet,
    sd.SatisFiyatiKdvHaric - round(((sd.SatisFiyatiKdvHaric * sd.iskontoyuzdetutar) / 100), 6) as satisfiyati_kdvharic_iskontolu,
    sk.SonAlisTarihi,
    
    case when sd.isKdvHaric = 1 then
      sd.Adet * ((sd.SatisFiyatiKdvHaric - round((sd.SatisFiyatiKdvHaric * sd.iskontoyuzdetutar) / 100, 6))
      +
     ((sd.SatisFiyatiKdvHaric - (sd.SatisFiyatiKdvHaric * sd.iskontoyuzdetutar) / 100) * sd.KdvOrani) / 100)
    else
     sd.Adet * (sd.SatisFiyati - ((sd.SatisFiyati * sd.iskontoyuzdetutar) / 100) - sd.Faturaiskonto)
    end Tutar,
    
    case when sd.isKdvHaric = 1 then
     ((sd.SatisFiyatiKdvHaric * sk.KdvOrani) / (100 + sk.KdvOrani))
    else
     ((sd.SatisFiyati * sk.KdvOrani) / 100) end KdvTutari,
    
    case when sd.isKdvHaric = 1 then
    sd.Adet * (((sd.SatisFiyatiKdvHaric - ((sd.SatisFiyatiKdvHaric * sd.iskontoyuzdetutar) / 100)) * sd.KdvOrani) / (100 + sd.KdvOrani))
    else
    sd.Adet * (((sd.SatisFiyatiKdvHaric - ((sd.SatisFiyatiKdvHaric * sd.iskontoyuzdetutar) / 100)) * sd.KdvOrani) / 100) end KdvToplamTutari,

    isnull(sd.Birimi, sk.Stoktipi) as Birimi,
    sd.SatisFiyatiKdvHaric,
    
    case when sd.isKdvHaric = 1 then

      sd.SatisFiyati - round((sk.SatisFiyatiKdvHaric * sd.iskontoyuzdetutar) / 100, 6) - sd.Faturaiskonto
	else

      sd.SatisFiyati - round((sd.SatisFiyati * sd.iskontoyuzdetutar) / 100, 6) - sd.Faturaiskonto

    end SatisFiyatiiskontolu,

    sd.Faturaiskonto,

    case 
    when AlisFiyati = 0 then 0 
    else 
   (((sd.SatisFiyati - sd.iskontotutar - sd.Faturaiskonto) - AlisFiyati) * 100) / AlisFiyati
   end KarYuzde,

   sd.Adet * (((sd.SatisFiyati - sd.iskontotutar - sd.Faturaiskonto) - AlisFiyati)) as Kar,
   isnull(fkParaBirimi, 1) as fkParaBirimi, isnull(kur_fiyati, 1) as kur_fiyati,
   sd.SatisFiyatiKdvHaric,
   sd.fkDepolar,
   sd.Adet * (sf.SatisFiyatiKdvli - ((sf.SatisFiyatiKdvli * sd.iskontoyuzdetutar) / 100) - sd.Faturaiskonto)
   as SatisFiyati_iki,
   isnull(SDA.aciklama_detay, '...') as aciklama_detay,
   sk.RBGKodu


FROM SatisDetay sd with(nolock)
LEFT JOIN SatisDetayAciklama SDA WITH(NOLOCK) ON SDA.fkSatisDetay = SD.pkSatisDetay
INNER JOIN(select pkStokKarti, StokKod, Stokadi, Barcode, Stoktipi, pkStokKartiid, KutuFiyat,
KdvOrani, Mevcut, SatisFiyati, SatisFiyatiKdvHaric, KritikMiktar, SonAlisTarihi,
satis_iskonto, alis_iskonto, RBGKodu from StokKarti with(nolock))sk  ON  sd.fkStokKarti = sk.pkStokKarti
left join SatisFiyatlari sf on sf.fkStokKarti = sk.pkStokKarti and sf.fkSatisFiyatlariBaslik = (select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik with(nolock) where Tur = 2)
where sd.fkSatislar=" + sonfis;

            //try
            //{
                gcSatisDetay.DataSource = DB.GetData(sqls);
            //}
            //catch (Exception exp)
            //{
            //    this.Text = "Hata Oluştur " + exp.Message;
            //    timer1.Enabled = false;
            //}
            
            toplamlar();
        }
        void toplamlar()
        {
            //decimal aratop = 0;//, istutar = 0;
            if (gridColumn5.SummaryItem.SummaryValue == null)
                Satis1Toplam.Text = "0.00";
            else
                Satis1Toplam.Text=gridColumn5.SummaryItem.SummaryValue.ToString();
             //   aratop = 0;//atis1Toplam.Text = "0,0";
             //else
             //aratop = decimal.Parse(gridColumn5.SummaryItem.SummaryValue.ToString());

                //iskonto tutar
                //if (gridColumn14.SummaryItem.SummaryValue == null)
                //    aratop = 0;//atis1Toplam.Text = "0,0";
                //else
                //    ceToplamiskonto.Value = decimal.Parse(gridColumn14.SummaryItem.SummaryValue.ToString());


                //KdvToplamTutari tutar
                //if (gcKdvToplamTutari.SummaryItem.SummaryValue == null)
                //    aratop = 0;//atis1Toplam.Text = "0,0";
                //else
                //    ceKdvToplamTutari.Value = decimal.Parse(gcKdvToplamTutari.SummaryItem.SummaryValue.ToString());

                //önce hesapla sonra bilgi göster NullTexde
                //if (ceiskontoyuzde.EditValue != null)
                //{
                //    isfiyat = decimal.Parse(ceiskontoyuzde.EditValue.ToString());
                //    istutar = isfiyat * aratop / 100;
                //}
                //if (ceiskontoTutar.EditValue != null) // && iskontoTutar.EditValue.ToString() !="0")
                //{
                //    isfiyat = decimal.Parse(ceiskontoTutar.EditValue.ToString());
                //    if (aratop > 0)
                //        isyuzde = (isfiyat * 100) / aratop;
                //    istutar = isfiyat;
                //}
            //Satis1Toplam.Text = aratop.ToString();
            //aratoplam.Value = aratop;
            //if (AcikSatisindex == 1)
            //{
            //    Satis1Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
            //    Satis1Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
            //}
            //if (AcikSatisindex == 2)
            //{
            //    Satis2Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
            //    Satis2Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
            //}
            //if (AcikSatisindex == 3)
            //{
            //    Satis3Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
            //    Satis3Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
            //}

            //if (AcikSatisindex == 4)
            //{
            //    Satis4Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
            //    Satis4Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
            //}

            //if (gridColumn28.SummaryItem.SummaryValue == null)
            //    cefark.EditValue = "0,0";
            //else
            //    cefark.EditValue = gridColumn28.SummaryItem.SummaryValue.ToString();

            //cefark.Value = aratoplam.Value - iskontoTutar.Value;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
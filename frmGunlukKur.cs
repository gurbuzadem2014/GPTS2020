using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Data.SqlClient;
using GPTS.islemler;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmGunlukKur : Form
    {
        bool _OtomatikAl = false;
        public frmGunlukKur(bool OtomatikAl)
        {
            InitializeComponent();
            _OtomatikAl = OtomatikAl;
        }

        private void DovizCek()
        {
            //XmlTextReader xmlReader = new XmlTextReader("http://www.tcmb.gov.tr/kurlar/today.xml");
            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.Load(xmlReader);
            //XmlNode topNode = xmlDocument.DocumentElement;

            //XmlNode dolarAlis = xmlDocument.SelectSingleNode("//Tarih_Date//Currency[Kod='USD']|//ForexBuying");
            //XmlNode dolarSatis = xmlDocument.SelectSingleNode("//Tarih_Date//Currency[Kod='USD']|//ForexSelling");

            //XmlNode GBPAlis = xmlDocument.SelectSingleNode("//Tarih_Date//Currency[Kod='GBP']|//ForexBuying");
            //XmlNode GBPSatis = xmlDocument.SelectSingleNode("//Tarih_Date//Currency[Kod='GBP']|//ForexSelling");

            //XmlNode euroAlis = topNode.SelectSingleNode("Currency[CurrencyName='EURO']");

            //XmlNode sterlincanAlis = topNode.SelectSingleNode("Currency[CurrencyName='POUND STERLING']");

            ////sterlinAlis = sterlincanAlis.ChildNodes[3].InnerText;
            ////sterlinSatis = sterlincanAlis.ChildNodes[4].InnerText;

            //usdAlis = dolarAlis.InnerText;
            //usdSatis = dolarSatis.InnerText;
            //euAlis = euroAlis.ChildNodes[3].InnerText;
            //euSatis = euroAlis.ChildNodes[4].InnerText;
        }

        void KayitliKurlar()
        {
            gridControl1.DataSource = DB.GetData(@"SELECT TOP 10 GK.Kod,GK.Tarih,PB.adi,GK.AlisKuru,GK.SatisKuru,PB.kodu FROM GunlukKurlar GK with(nolock)
            left join ParaBirimi PB with(nolock) ON PB.pkParaBirimi=GK.fkParaBirimi order by GK.Tarih desc");
        }

        public DataTable KurlariAl()
        {
            DataTable dt = new DataTable();
            // DataTable nesnemizi yaratıyoruz
            DataRow dr;
            // DataTable ın satırlarını tanımlıyoruz.
            dt.Columns.Add(new DataColumn("Adi", typeof(string)));
            dt.Columns.Add(new DataColumn("Kod", typeof(string)));
            dt.Columns.Add(new DataColumn("DovizAlis", typeof(string)));
            dt.Columns.Add(new DataColumn("DovizSatis", typeof(string)));
            dt.Columns.Add(new DataColumn("EfektifAlis", typeof(string)));
            dt.Columns.Add(new DataColumn("EfektifSatis", typeof(string)));
            // DataTableımıza 6 sütün ekliyoruz ve değişken tiplerini tanımlıyoruz.
            XmlTextReader rdr = new XmlTextReader("http://www.tcmb.gov.tr/kurlar/today.xml");
            // XmlTextReader nesnesini yaratıyoruz ve parametre olarak xml dokümanın urlsini veriyoruz
            // XmlTextReader urlsi belirtilen xml dokümanlarına hızlı ve forward-only giriş imkanı sağlar.
            XmlDocument myxml = new XmlDocument();
            // XmlDocument nesnesini yaratıyoruz.
            myxml.Load(rdr);
            // Load metodu ile xml yüklüyoruz
            XmlNode tarih = myxml.SelectSingleNode("/Tarih_Date/@Tarih");
            XmlNodeList mylist = myxml.SelectNodes("/Tarih_Date/Currency");
            XmlNodeList adi = myxml.SelectNodes("/Tarih_Date/Currency/Isim");
            XmlNodeList kod = myxml.SelectNodes("/Tarih_Date/Currency/@Kod");
            XmlNodeList doviz_alis = myxml.SelectNodes("/Tarih_Date/Currency/ForexBuying");
            XmlNodeList doviz_satis = myxml.SelectNodes("/Tarih_Date/Currency/ForexSelling");
            XmlNodeList efektif_alis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteBuying");
            XmlNodeList efektif_satis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteSelling");
            // XmlNodeList cinsinden her bir nodu, SelectSingleNode metoduna nodların xpathini parametre olarak
            // göndererek tanımlıyoruz.
            this.Text = tarih.InnerText.ToString() + " tarihli merkez bankası kur bilgileri";
            // datagridimin captionu ayarlıyoruz.
            int x = 19;
            /*  Burada xmlde bahsettiğim - bence-  mantık hatasından dolayı x gibi bir değişken tanımladım.
            bu x =19  DataTable a sadece 19 satır eklenmesini sağlıyor. çünkü xml dökümanında 19. node dan sonra
            güncel kur bilgileri değil Euro dönüşüm kurları var ve bu node dan sonra yapı ilk 18 node ile tutmuyor
            Bence ayrı bir xml dökümanda tutulması gerekirdi. 
            */
            for (int i = 0; i < x; i++)
            {
                dr = dt.NewRow();
                dr[0] = adi.Item(i).InnerText.ToString(); // i. adi nodunun içeriği
                // Adı isimli DataColumn un satırlarını  /Tarih_Date/Currency/Isim node ları ile dolduruyoruz.
                dr[1] = kod.Item(i).InnerText.ToString();
                // Kod satırları
                dr[2] = doviz_alis.Item(i).InnerText.ToString();
                // Döviz Alış
                dr[3] = doviz_satis.Item(i).InnerText.ToString();
                // Döviz  Satış
                dr[4] = efektif_alis.Item(i).InnerText.ToString();
                // Efektif Alış
                dr[5] = efektif_satis.Item(i).InnerText.ToString();
                // Efektif Satış.
                dt.Rows.Add(dr);
            }
            // DataTable ımızın satırlarını 18 satır olacak şekilde dolduruyoruz
            // gerçi yine web mastırın insafı devreye giriyor:).
            // yukarıda bahsettiğim sorun.
            return dt;
            // DataTable ı döndürüyoruz.
        }

        void OtomatikAl()
        {
            DataTable dtKurlar = (DataTable)gridControl2.DataSource;
            if(dtKurlar.Rows.Count==0) return;

            DataTable dt = DB.GetData("select * from ParaBirimi with(nolock) where aktif=1");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string kodu=dt.Rows[i]["kodu"].ToString();
                string pkParaBirimi = dt.Rows[i]["pkParaBirimi"].ToString();
                

                DataRow[] dr = dtKurlar.Select("Kod = '" + kodu + "'");
                //eğer yoksa ekle
                if(dr==null)
                {
                    DataTable dtGK = DB.GetData("select top 1 * from GunlukKurlar with(nolock)  where Dvz='" + kodu + "'  order by Tarih desc");
                }

                if (dr.Length == 0) continue;

                kodu = dr[0]["Kod"].ToString();
                string doviz_alis = dr[0]["DovizAlis"].ToString();
                string doviz_satis = dr[0]["DovizSatis"].ToString();

                ArrayList arr = new ArrayList();
                arr.Add(new SqlParameter("@AlisKuru", doviz_alis));
                arr.Add(new SqlParameter("@SatisKuru", doviz_satis));
                arr.Add(new SqlParameter("@fkParaBirimi", pkParaBirimi));
                arr.Add(new SqlParameter("@Dvz", kodu));

                string sql = "INSERT INTO GunlukKurlar (Tarih,Dvz,fkParaBirimi,AlisKuru,SatisKuru) VALUES(GETDATE(),@Dvz,@fkParaBirimi,@AlisKuru,@SatisKuru)";

                DB.ExecuteSQL(sql, arr);
            }
            DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama,fkKullanicilar) " +
                       " VALUES(3,'Günlük Kurlar Alındı',getdate(),1,'Günlük Kurlar Alındı'," + DB.fkKullanicilar + ")");

            KayitliKurlar();
        }
       
        private void frmGunlukKur_Load(object sender, EventArgs e)
        {
            KayitliKurlar();

            try
            {
                gridControl2.DataSource = KurlariAl();

                if (_OtomatikAl)
                   OtomatikAl();
            }
            catch(Exception hata)
            {
                string s = hata.Message;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //common_katman.CGunlukKurlar kurr = new common_katman.CGunlukKurlar();
            //kurr.Kod

            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string kodu = dr["Kod"].ToString();
            string doviz_alis = dr["DovizAlis"].ToString();
            string doviz_satis = dr["DovizSatis"].ToString();
            
            //SELECT GK.Kod,GK.Tarih,PB.adi,GK.AlisKuru,GK.SatisKuru,PB.kodu FROM GunlukKurlar GK with(nolock)
            //left join ParaBirimi PB with(nolock) ON PB.pkParaBirimi=GK.fkParaBirimi

            DataTable dtParaBirimi = DB.GetData("select * from ParaBirimi with(nolock) where kodu='"+ kodu +"'");
            if (dtParaBirimi.Rows.Count == 0)
            {
                formislemleri.Mesajform("Para Birimi Tanımlı Değil", "K", 200);
                return;
            }
            string pkParaBirimi = dtParaBirimi.Rows[0]["pkParaBirimi"].ToString();

            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@AlisKuru", doviz_alis));
            arr.Add(new SqlParameter("@SatisKuru", doviz_satis));
            arr.Add(new SqlParameter("@fkParaBirimi", pkParaBirimi));
            arr.Add(new SqlParameter("@Dvz", kodu));

            string sql = "INSERT INTO GunlukKurlar (Tarih,Dvz,fkParaBirimi,AlisKuru,SatisKuru) VALUES(GETDATE(),@Dvz,@fkParaBirimi,@AlisKuru,@SatisKuru)";

            DB.ExecuteSQL(sql, arr);

            KayitliKurlar();
        }

        private void sİLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.ExecuteSQL("delete from GunlukKurlar where Kod=" + dr["Kod"].ToString());

            KayitliKurlar();
        }

        private void satışFiyatlarınıSatışKurlarıİleÇartpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Satış Fiyatları Satış Fiyatları çarpılacak Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("exec sph_satisfiyatlari_kurla_carp");
        }
    }
}

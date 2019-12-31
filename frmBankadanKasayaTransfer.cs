using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using GPTS.islemler;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmBankadanKasayaTransfer : Form
    {
        public frmBankadanKasayaTransfer()
        {
            InitializeComponent();
        }

        private void frmKasaTransferKarti_Load(object sender, EventArgs e)
        {
            lueKasa.Properties.DataSource = DB.GetData("SELECT * FROM Kasalar with(nolock) where Aktif=1");
            lueKasa.ItemIndex = 1;

            lueBanka.Properties.DataSource = DB.GetData("SELECT * FROM Bankalar with(nolock) where Aktif=1 order by tiklamaadedi desc");

            TransferHareketleri();

            islemtarihi.DateTime = DateTime.Now;

            ceTutar.Focus();
        }

        void TransferHareketleri()
        {
            gridControl1.DataSource = DB.GetData("select * from KasaHareket with(nolock) where OdemeSekli='Transfer'");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            #region Bankaya Çıkan
            ArrayList list2 = new ArrayList();
            list2.Add(new SqlParameter("@fkKasalar", DBNull.Value));

            list2.Add(new SqlParameter("@fkBankalar", int.Parse(lueBanka.EditValue.ToString())));

            list2.Add(new SqlParameter("@Tarih", islemtarihi.DateTime));

            list2.Add(new SqlParameter("@Alacak",ceTutar.Value.ToString().Replace(",", ".")));
            list2.Add(new SqlParameter("@Borc", "0"));
            list2.Add(new SqlParameter("@Tipi", "1"));
            list2.Add(new SqlParameter("@AktifHesap", "1"));
            list2.Add(new SqlParameter("@OdemeSekli", "Transfer"));
            list2.Add(new SqlParameter("@Tutar", "0"));
            list2.Add(new SqlParameter("@fkKasaGirisCikisTurleri", "0"));
            list2.Add(new SqlParameter("@fkCek", "0"));
            list2.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list2.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            list2.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
            string sql2 = @"INSERT INTO KasaHareket (fkKasalar,fkBankalar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,OdemeSekli,Tutar,fkKasaGirisCikisTurleri,BilgisayarAdi,fkKullanicilar,GiderOlarakisle,GelirOlarakisle)
                         values(@fkKasalar,@fkBankalar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,0,@AktifHesap,@OdemeSekli,@Tutar,@fkKasaGirisCikisTurleri,@BilgisayarAdi,@fkKullanicilar,0,0)
                        SELECT IDENT_CURRENT('KasaHareket')";

            string sonuc2 = DB.ExecuteScalarSQL(sql2, list2);

            if (sonuc2.Substring(0, 1) != "H")
            {
                formislemleri.Mesajform("Banka Hareketi Eklenemiştir.", "S", 200);
            }
            else
                DB.ExecuteSQL("delete from KasaHareket where pkKasaHareket=" + sonuc2);

            #endregion 

            #region kasadan Giren
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkKasalar", int.Parse(lueKasa.EditValue.ToString())));

            list.Add(new SqlParameter("@fkBankalar", DBNull.Value));

            list.Add(new SqlParameter("@Tarih", islemtarihi.DateTime));

            list.Add(new SqlParameter("@Alacak", "0"));
            list.Add(new SqlParameter("@Borc", ceTutar.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Tipi", "2"));
            list.Add(new SqlParameter("@AktifHesap", "1"));
            list.Add(new SqlParameter("@OdemeSekli", "Transfer"));
            list.Add(new SqlParameter("@Tutar", "0"));
            list.Add(new SqlParameter("@fkKasaGirisCikisTurleri", "0"));
            list.Add(new SqlParameter("@fkCek", "0"));
            list.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

             string sql = @"INSERT INTO KasaHareket (fkKasalar,fkBankalar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,OdemeSekli,Tutar,fkKasaGirisCikisTurleri,BilgisayarAdi,fkKullanicilar,GiderOlarakisle,GelirOlarakisle)
                         values(@fkKasalar,@fkBankalar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,0,@AktifHesap,@OdemeSekli,@Tutar,@fkKasaGirisCikisTurleri,@BilgisayarAdi,@fkKullanicilar,0,0)
                        SELECT IDENT_CURRENT('KasaHareket')"; 

            string sonuc = DB.ExecuteScalarSQL(sql, list);

            if (sonuc.Substring(0, 1) != "H")
            {
                formislemleri.Mesajform("Kasa Hareket Eklenirken Hata Oluştu.", "K",200);
            }
            else
                return;

            #endregion

            

            TransferHareketleri();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

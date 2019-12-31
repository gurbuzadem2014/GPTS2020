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
    public partial class frmKasaTransfer : Form
    {
        public frmKasaTransfer()
        {
            InitializeComponent();
        }

        private void frmKasaTransferKarti_Load(object sender, EventArgs e)
        {
            lueKasa.Properties.DataSource = DB.GetData("SELECT * FROM Kasalar with(nolock) where Aktif=1");
            lueKasa.EditValue = DB.fkKullanicilar;

            lueTransferEdilecekKasa.Properties.DataSource = DB.GetData("SELECT * FROM Kasalar with(nolock) where Aktif=1");

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
            if (lueTransferEdilecekKasa.EditValue == null)
            {
                formislemleri.Mesajform("Transfer Edilecek Kasa Seçiniz", "K", 200);
                lueTransferEdilecekKasa.Focus();
                return;
            }

            if (lueKasa.EditValue == null)
            {
                formislemleri.Mesajform("Kasa Seçiniz", "K", 200);
                lueKasa.Focus();
                return;
            }

            if (lueTransferEdilecekKasa.EditValue == lueKasa.EditValue)
            {
                formislemleri.Mesajform("Aynı Kasaya Trasnfer Edilemez", "K", 200);
                lueTransferEdilecekKasa.Focus();
                return;
            }

            if (ceTutar.EditValue == null)
            {
                formislemleri.Mesajform("Tutar Giriniz", "K", 200);
                ceTutar.Focus();
                return;
            }

            #region kasadan çıkan
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkKasalar", int.Parse(lueKasa.EditValue.ToString())));

            list.Add(new SqlParameter("@fkBankalar", DBNull.Value));

            list.Add(new SqlParameter("@Tarih", islemtarihi.DateTime));

            list.Add(new SqlParameter("@Alacak", ceTutar.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Borc", "0"));
            list.Add(new SqlParameter("@Tipi", "2"));
            list.Add(new SqlParameter("@AktifHesap", "1"));
            list.Add(new SqlParameter("@OdemeSekli", "Transfer"));
            list.Add(new SqlParameter("@Tutar", "0"));
            list.Add(new SqlParameter("@fkKasaGirisCikisTurleri", "0"));
            list.Add(new SqlParameter("@fkCek", "0"));
            list.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list.Add(new SqlParameter("@BilgisayarAdi", DB.fkKullanicilar));
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

             string sql = @"INSERT INTO KasaHareket (fkKasalar,fkBankalar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,OdemeSekli,Tutar,fkKasaGirisCikisTurleri,BilgisayarAdi,fkKullanicilar,GiderOlarakisle,GelirOlarakisle)
                         values(@fkKasalar,@fkBankalar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,0,@AktifHesap,@OdemeSekli,@Tutar,@fkKasaGirisCikisTurleri,@BilgisayarAdi,@fkKullanicilar,0,0)
                        SELECT IDENT_CURRENT('KasaHareket')"; 

            string sonuc = DB.ExecuteScalarSQL(sql, list);

            if (sonuc.Substring(0, 1) != "H")
            {
                //formislemleri.Mesajform("Kasa Hareket Eklenmiştir.", "S");
            }
            else
                return;

            #endregion

            #region Kasaya Giren
            ArrayList list2 = new ArrayList();
            list2.Add(new SqlParameter("@fkKasalar", int.Parse(lueTransferEdilecekKasa.EditValue.ToString()))); 

            list2.Add(new SqlParameter("@fkBankalar", DBNull.Value));

            list2.Add(new SqlParameter("@Tarih", islemtarihi.DateTime));

            list2.Add(new SqlParameter("@Alacak", "0"));
            list2.Add(new SqlParameter("@Borc", ceTutar.Value.ToString().Replace(",", ".")));
            list2.Add(new SqlParameter("@Tipi", "1"));
            list2.Add(new SqlParameter("@AktifHesap", "1"));
            list2.Add(new SqlParameter("@OdemeSekli", "Transfer"));
            list2.Add(new SqlParameter("@Tutar", "0"));
            list2.Add(new SqlParameter("@fkKasaGirisCikisTurleri", "0"));
            list2.Add(new SqlParameter("@fkCek", "0"));
            list2.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list2.Add(new SqlParameter("@BilgisayarAdi", DB.fkKullanicilar));
            list2.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
            string sql2 = @"INSERT INTO KasaHareket (fkKasalar,fkBankalar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,OdemeSekli,Tutar,fkKasaGirisCikisTurleri,BilgisayarAdi,fkKullanicilar,GiderOlarakisle,GelirOlarakisle)
                         values(@fkKasalar,@fkBankalar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,0,@AktifHesap,@OdemeSekli,@Tutar,@fkKasaGirisCikisTurleri,@BilgisayarAdi,@fkKullanicilar,0,0)
                        SELECT IDENT_CURRENT('KasaHareket')"; 

            string sonuc2 = DB.ExecuteScalarSQL(sql2, list2);

            if (sonuc2.Substring(0, 1) != "H")
            {
                formislemleri.Mesajform("Kasa Hareketi(Giriş) Eklenmiştir.", "S",200);
            }
            else
                DB.ExecuteSQL("delete from KasaHareket where pkKasaHareket=" + sonuc);

            #endregion 

            TransferHareketleri();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sİLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            if (!formislemleri.SifreIste()) return;

            string sec = formislemleri.MesajBox("Silmek İstediğinize Eminmisiniz ?", "Kasa Hareketi Sil", 3, 0);
            if (sec != "1") return;

            //inputForm sifregir1 = new inputForm();
            //sifregir1.Text = "Silinme Nedeni";
            //sifregir1.GirilenCaption.Text = "Silme Nedenini Giriniz!";
            //sifregir1.Girilen.Text = "Hatalı Ödeme Al";
            ////sifregir.Girilen.Properties.PasswordChar = '#';
            //sifregir1.ShowDialog();

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkKasaHareket = dr["pkKasaHareket"].ToString();
            string fkCek = dr["fkCek"].ToString();
            string fkTaksitler = dr["fkTaksitler"].ToString();
            string fkFirma = dr["fkFirma"].ToString();

            string borc = dr["Borc"].ToString();
            string alacak = dr["Alacak"].ToString();

            //,odeme_sekli,,bilgisayar_adi,fkFirma 
           // DB.ExecuteSQL("INSERT INTO KasaHareketLog (fkKasaHareket,aciklama,tarih,fkKullanicilar,bilgisayar_adi,fkFirma,borc,alacak) " +
           //" VALUES(" + pkKasaHareket + ",'" + sifregir1.Girilen.Text + "',getdate()," + DB.fkKullanicilar + ",'" + Degerler.BilgisayarAdi + "'," + fkFirma + "," +
           //borc.Replace(',', '.') + "," + alacak.Replace(',', '.') + ")");


            #region trasn başlat
            DB.trans_basarili = true;
            DB.trans_hata_mesaji = "";
            if (DB.conTrans == null)
                DB.conTrans = new SqlConnection(DB.ConnectionString());
            if (DB.conTrans.State == ConnectionState.Closed)
                DB.conTrans.Open();

            DB.transaction = DB.conTrans.BeginTransaction("KasaTransaction");
            #endregion

            #region İşlemler
            string sonuc = "";
            //if (fkCek != "0")
            //{
            //    sonuc = DB.ExecuteSQLTrans("update Cekler set fkFirma=0,fkCekTuru=0 where pkCek=" + fkCek);
            //    DB.ExecuteSQL("delete from HatirlatmaAnimsat where fkCek=" + fkCek);
            //}
            //if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            //{
            //    DB.trans_hata_mesaji = "Kasa Çek " + sonuc;
            //    DB.trans_basarili = false;
            //}


            //if (fkTaksitler != "" && DB.trans_basarili)
            //{
            //    sonuc = DB.ExecuteSQLTrans(@"UPDATE Taksitler Set Odenen=0,OdendigiTarih=null,OdemeSekli='silindi'
            //            where pkTaksitler=" + fkTaksitler);

            //    if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            //    {
            //        DB.trans_hata_mesaji = "Kasa Taksit " + sonuc;
            //        DB.trans_basarili = false;
            //    }
            //}
            sonuc = DB.ExecuteSQLTrans("DELETE FROM KasaHareket WHERE pkKasaHareket=" + pkKasaHareket);

            if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            {
                DB.trans_hata_mesaji = "Kasa Trasfer " + sonuc;
                DB.trans_basarili = false;
            }

            //if (fkFirma != "")
            //{
            //    sonuc = DB.ExecuteSQLTrans("update Firmalar set Aktarildi="+Degerler.AracdaSatis+" where pkFirma=" + fkFirma);
            //    if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            //    {
            //        DB.trans_hata_mesaji = "Firmalar aktarıldı " + sonuc;
            //        DB.trans_basarili = false;
            //    }
            //}
            #endregion

            #region trasn sonu
            if (DB.trans_basarili == true)
            {
                DB.transaction.Commit();
            }
            else
            {
                DB.transaction.Rollback();
                formislemleri.Mesajform(DB.trans_hata_mesaji, "K", 200);
            }
            DB.conTrans.Close();
            #endregion

            //MusteriBorcu();

            TransferHareketleri();

            //comboBoxEdit3_SelectedIndexChanged(sender, e);

            //lueCekler.ItemIndex = 0;
        }
    }
}

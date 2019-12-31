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
using System.Data.OleDb;

namespace GPTS
{

    public partial class frmAktarim : DevExpress.XtraEditors.XtraForm
    {
        DataTable dturunler = null;
        DataTable dtsatislar = null;
        DataTable dtalislar = null;
        DataTable dtTedarikciler = null;
        DataTable dtMusteriler = null;
        public frmAktarim()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = DB.GetDatamdb("Select * from grub");
            gridControl2.DataSource = DB.GetData("Select * from StokGruplari");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("INSERT INTO StokGruplari (StokGrup,Aktif) VALUES('" + dr["grubu"].ToString() + "',1)");
            }
            gridControl2.DataSource = DB.GetData("Select * from StokGruplari");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            dturunler = DB.GetDatamdb("select * from urunler");
            gcUrunler.DataSource = dturunler;
            simpleButton3.Enabled = true;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (cbSatis2.Checked == true)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("2.Fiyat Gurubu Eklediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;
            }
            listBox1.Items.Clear();
            int aktarilan = 0;
            int hatali = 0;
            string sql = "";
            int c = dturunler.Rows.Count;
            if (c == 0) return;
            for (int i = 0; i < c; i++)
            {
                if (DB.GetData("select pkStokKarti from StokKarti with(nolock) where Barcode='" + dturunler.Rows[i]["barkod"].ToString() + "'").Rows.Count == 1) continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@StokKod", dturunler.Rows[i]["barkod"].ToString()));
                list.Add(new SqlParameter("@Barcode", dturunler.Rows[i]["barkod"].ToString()));
                string grupadi = dturunler.Rows[i]["grub"].ToString();
                string pkStokGrup = "0";
                DataTable dtgrup = DB.GetData("select pkStokGrup from StokGruplari where StokGrup='" + grupadi + "'");
                if (dtgrup.Rows.Count == 0)
                {
                    pkStokGrup = DB.ExecuteScalarSQL("INSERT INTO StokGruplari (StokGrup,WebdeGoster,Aktif,SortID,Gonderildi) VALUES('" + grupadi + "',0,1,0,0) select IDENT_CURRENT('StokGruplari')");
                    //pkStokGrup = DB.GetData("select Max(pkStokGrup) from StokGruplari").Rows[0][0].ToString();
                }
                else
                    pkStokGrup = dtgrup.Rows[0]["pkStokGrup"].ToString();
                list.Add(new SqlParameter("@fkStokGrup", pkStokGrup));
                list.Add(new SqlParameter("@Stokadi", dturunler.Rows[i]["urun"].ToString()));
                string Mevcut = "0";
                if (dturunler.Rows[i]["Stok"].ToString() == "0,0000")
                    Mevcut = "0";//  list.Add(new SqlParameter("@Mevcut", "0"));
                else
                    Mevcut = dturunler.Rows[i]["Stok"].ToString().Replace(",", "."); //list.Add(new SqlParameter("@Mevcut", Double.Parse(dturunler.Rows[i]["Stok"].ToString().Replace(",", "."))));
                if (dturunler.Rows[i]["gelis"].ToString() == "0,0000")
                {
                    list.Add(new SqlParameter("@AlisFiyati", "0"));
                    list.Add(new SqlParameter("@AlisFiyatiKdvHaric", "0"));
                }
                else
                {
                    list.Add(new SqlParameter("@AlisFiyati", dturunler.Rows[i]["gelis"].ToString().Replace(",", ".")));
                    list.Add(new SqlParameter("@AlisFiyatiKdvHaric", dturunler.Rows[i]["gelis"].ToString().Replace(",", ".")));

                }
                decimal sf = 0;
                decimal.TryParse(dturunler.Rows[i]["satis"].ToString(), out sf);
                //if (dturunler.Rows[i]["satis"].ToString() == "0,0000")
                //   list.Add(new SqlParameter("@SatisFiyati", "0"));
                //else
                list.Add(new SqlParameter("@SatisFiyati", sf.ToString().Replace(",", ".")));//dturunler.Rows[i]["satis"].ToString().Replace(",", ".")));
                list.Add(new SqlParameter("@Stoktipi", dturunler.Rows[i]["a"].ToString()));
                if (dturunler.Rows[i]["kdv"].ToString() == "")
                    list.Add(new SqlParameter("@KdvOrani", "8"));
                else
                    list.Add(new SqlParameter("@KdvOrani", dturunler.Rows[i]["kdv"].ToString()));
                //if (dturunler.Rows[i]["durumu"].ToString() == "AKTİF")
                list.Add(new SqlParameter("@Aktif", "1"));
                // else
                // list.Add(new SqlParameter("@Aktif", "0"));
                string KritikMiktar = "0";
                if (dturunler.Rows[i]["kritikstok"].ToString() == "0,0000" || dturunler.Rows[i]["kritikstok"].ToString() == "")
                    KritikMiktar = "0";//list.Add(new SqlParameter("@KritikMiktar", "0"));
                else
                    KritikMiktar = dturunler.Rows[i]["kritikstok"].ToString().Replace(",", ".");//list.Add(new SqlParameter("@KritikMiktar", dturunler.Rows[i]["kritikstok"].ToString().Replace(",", ".")));

                if (KritikMiktar == "") KritikMiktar = "0";
                if (Mevcut == "") Mevcut = "0";

                sql = "INSERT INTO StokKarti (StokKod,Barcode,fkStokGrup,Stokadi,Stoktipi,Mevcut,AlisFiyati,SatisFiyati,KdvOrani,Aktif,KritikMiktar,AlisFiyatiKdvHaric)" +
                " values(@StokKod,@Barcode,@fkStokGrup,@Stokadi,@Stoktipi," + Mevcut + ",@AlisFiyati,@SatisFiyati,@KdvOrani,@Aktif," + KritikMiktar + ",@AlisFiyatiKdvHaric) select IDENT_CURRENT('StokKarti')";
                try
                {
                    string pkStokKarti = DB.ExecuteScalarSQL(sql, list);
                    if (pkStokKarti.Substring(0, 1) == "H")
                    {
                        listBox1.Items.Add(dturunler.Rows[i]["gelis"].ToString() + pkStokKarti);
                        continue;
                    }
                    aktarilan++;
                    DB.ExecuteSQL("update  StokKarti set HizliSatisAdi=substring(StokAdi,0,19)");
                    DB.ExecuteSQL("insert into SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,Aktif)" +
                        " values(" + pkStokKarti + ",1," + dturunler.Rows[i]["satis"].ToString().Replace(",", ".") + ",0,0,1)");
                    if (cbSatis2.Checked)
                        DB.ExecuteSQL("insert into SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,Aktif)" +
                    " values(" + pkStokKarti + ",2," + dturunler.Rows[i]["satis2"].ToString().Replace(",", ".") + ",0,0,1)");
                }
                catch (Exception exp)
                {
                    hatali++;
                }
            }
            DB.ExecuteSQL("update StokKarti set SatisAdedi=1 where SatisAdedi is null");
            DB.ExecuteSQL("update StokKarti set HizliSatisAdi=rtrim(HizliSatisAdi)");
            //DB.ExecuteSQL("update StokKarti set UreticiKodu=Barcode where UreticiKodu is null");
            DB.ExecuteSQL("update StokKarti set KdvHaric=1 where KdvHaric is null");
            DB.ExecuteSQL("update StokKarti set alis_iskonto=0 where alis_iskonto is null");
            DB.ExecuteSQL("update StokKarti set satis_iskonto=0 where satis_iskonto is null");
            DB.ExecuteSQL("update Stokkarti set SatisFiyatiKdvHaric=SatisFiyati-(SatisFiyati*KdvOrani)/(100+KdvOrani)");
            DB.ExecuteSQL("update Stokkarti set AlisFiyatiKdvHaric=AlisFiyati-(AlisFiyati*KdvOraniAlis)/(100+KdvOraniAlis)");

            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            dtsatislar = null;
            dtsatislar = DB.GetDatamdb("select * from satislar order by fisno");
            gridControl3.DataSource = dtsatislar;
            simpleButton5.Enabled = true;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            int aktarilan = 0;
            string fisno = "0";
            string eskifisno = "0";
            int hatali = 0;
            string sql = "";
            float fistoplam = 0;
            int c = dtsatislar.Rows.Count;
            if (c == 0) return;
            for (int i = 0; i < c; i++)
            {
                //if (DB.GetData("select * from Satislar where EskiFis=" + dtsatislar.Rows[i]["fisno"].ToString()).Rows.Count == 1) continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", Convert.ToDateTime(dtsatislar.Rows[i]["tarih"].ToString())));
                list.Add(new SqlParameter("@EskiFis", dtsatislar.Rows[i]["fisno"].ToString()));
                string fkSatisDurumu = "2";
                string odemesi = dtsatislar.Rows[i]["odemesi"].ToString();
                if (odemesi == "İADE")
                    fkSatisDurumu = "9";
                list.Add(new SqlParameter("@fkSatisDurumu", fkSatisDurumu));
                list.Add(new SqlParameter("@Aciklama", "Ödeme=" + odemesi + "-Kullanıcı=" + dtsatislar.Rows[i]["satisiyapan"].ToString()));
                sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,fkKullanici,fkSatisDurumu,Aciklama,ToplamTutar,EskiFis,AlinanPara,iskontoFaturaTutar)" +
                " values(@Tarih,1,1,1,@fkSatisDurumu,@Aciklama,0,@EskiFis,0,0) select IDENT_CURRENT('Satislar')";
                try
                {
                    if (eskifisno != dtsatislar.Rows[i]["fisno"].ToString())
                    {
                        eskifisno = dtsatislar.Rows[i]["fisno"].ToString();
                        fisno = DB.ExecuteScalarSQL(sql, list);
                    }

                    if (fisno.Substring(0, 1) == "H") continue;
                    aktarilan++;
                    DataTable dtStokKarti = DB.GetData("select * from StokKarti where Barcode='" + dtsatislar.Rows[i]["barkod2"].ToString() + "'");
                    if (dtStokKarti.Rows.Count > 0)
                    {
                        string fkStokKarti = dtStokKarti.Rows[0][0].ToString();
                        string adet = dtsatislar.Rows[i]["adet"].ToString();
                        if (adet == "0,0000")
                            adet = "1";
                        decimal Satis = decimal.Parse(dtsatislar.Rows[i]["fiyat"].ToString());
                        decimal kar = decimal.Parse(dtsatislar.Rows[i]["kar"].ToString());
                        decimal Alis = Satis - kar;
                        sql = "insert into SatisDetay (fkSatislar,fkStokKarti,Adet,AlisFiyati,SatisFiyati,Tarih,iskontotutar)" +
                        " values(" + fisno + "," + fkStokKarti + "," + adet + "," + Alis.ToString().Replace(",", ".") + "," +
                        Satis.ToString().Replace(",", ".") + ",'" +
                        Convert.ToDateTime(dtsatislar.Rows[i]["tarih"].ToString()).ToString("yyyyMMdd") + "',0)";

                        int sonuc = DB.ExecuteSQL(sql);
                        if (sonuc != 1)
                            sonuc = 0;
                    }
                }
                catch (Exception exp)
                {
                    hatali++;
                }
            }
            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("TRUNCATE TABLE StokGruplari");
            gridControl1.DataSource = DB.GetDatamdb("Select * from grub");
            gridControl2.DataSource = DB.GetData("Select * from StokGruplari");
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            dtTedarikciler = DB.GetDatamdb("select * from toptanci");
            gridControl6.DataSource = dtTedarikciler;
            simpleButton8.Enabled = true;
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            int aktarilan = 0;
            int hatali = 0;
            string sql = "";
            int c = dtTedarikciler.Rows.Count;
            if (c == 0) return;
            for (int i = 0; i < c; i++)
            {
                if (DB.GetData("select * from Tedarikciler where Firmaadi='" + dtTedarikciler.Rows[i]["firma"].ToString() + "'").Rows.Count == 1) continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Firmaadi", dtTedarikciler.Rows[i]["firma"].ToString()));
                list.Add(new SqlParameter("@Yetkili", dtTedarikciler.Rows[i]["yetkili"].ToString()));
                list.Add(new SqlParameter("@Tel", dtTedarikciler.Rows[i]["tel1"].ToString()));
                list.Add(new SqlParameter("@OzelKod", dtTedarikciler.Rows[i]["no"].ToString()));

                //if (dturunler.Rows[i]["ToplamTutar"].ToString() == "0,0000")
                //    list.Add(new SqlParameter("@ToplamTutar", "0"));
                //else
                //    list.Add(new SqlParameter("@ToplamTutar", dturunler.Rows[i]["Stok"].ToString().Replace(",", ".")));

                sql = "INSERT INTO Tedarikciler (Firmaadi,Yetkili,Tel,OzelKod)" +
                " values(@Firmaadi,@Yetkili,@Tel,@OzelKod)";
                try
                {
                    string pktedarikci = DB.ExecuteSQL(sql, list);
                }
                catch (Exception exp)
                {
                    hatali++;
                }
            }
            DB.ExecuteSQL("UPDATE Tedarikciler SET OzelKod=pkTedarikciler where OzelKod is null");
            DB.ExecuteSQL("UPDATE Tedarikciler SET Aktif=1");
            DB.ExecuteSQL(@"update Tedarikciler set fkFirmaGruplari=1,
fkil=7,fkilce=0,fkSirket=1,LimitBorc=0,Borc=0,Alacak=0,Devir=0");
            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());

        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            dtalislar = null;
            dtalislar = DB.GetDatamdb("select * from alislar order by fisno");
            gridControl8.DataSource = dtalislar;
            simpleButton10.Enabled = true;
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            int aktarilan = 0;
            string fisno = "0";
            string eskifisno = "0";
            int hatali = 0;
            string sql = "";
            int c = dtalislar.Rows.Count;
            if (c == 0) return;
            for (int i = 0; i < c; i++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", Convert.ToDateTime(dtalislar.Rows[i]["tarih"].ToString())));
                list.Add(new SqlParameter("@EskiFis", dtalislar.Rows[i]["fisno"].ToString()));
                list.Add(new SqlParameter("@Aciklama", "Tedarikçi=" + dtalislar.Rows[i]["toptanciadi"].ToString()));
                DataTable dtt = DB.GetData("select pkTedarikciler From Tedarikciler where OzelKod=" + dtalislar.Rows[i]["toptancikodu"].ToString());
                string fkTedarikci = "1";
                if (dtt.Rows.Count == 0)
                    fkTedarikci = "1";
                else
                    fkTedarikci = dtt.Rows[0][0].ToString();

                list.Add(new SqlParameter("@fkFirma", fkTedarikci));
                sql = "INSERT INTO Alislar (Tarih,fkFirma,Siparis,fkSatisDurumu,fkKullanici,Aciklama,ToplamTutar,EskiFis,AlinanPara,iskontoFaturaTutar)" +
                " values(@Tarih,@fkFirma,1,1,1,@Aciklama,0,@EskiFis,0,0) select IDENT_CURRENT('Alislar')";
                try
                {
                    if (eskifisno != dtalislar.Rows[i]["fisno"].ToString())
                    {
                        eskifisno = dtalislar.Rows[i]["fisno"].ToString();
                        fisno = DB.ExecuteScalarSQL(sql, list);
                    }
                    if (fisno.Substring(0, 1) == "H") continue;
                    aktarilan++;
                    DataTable dtStokKarti = DB.GetData("select * from StokKarti where Barcode='" + dtalislar.Rows[i]["barkod2"].ToString() + "'");
                    if (dtStokKarti.Rows.Count > 0)
                    {
                        string fkStokKarti = dtStokKarti.Rows[0][0].ToString();
                        string adet = dtalislar.Rows[i]["gelenstok"].ToString();
                        if (adet == "0,0000")
                            adet = "1";
                        decimal SatisFiyati = decimal.Parse(dtalislar.Rows[i]["satisfiyati"].ToString());
                        decimal AlisFiyati = decimal.Parse(dtalislar.Rows[i]["alisfiyati"].ToString());
                        sql = "insert into AlisDetay (fkAlislar,fkStokKarti,Adet,AlisFiyati,SatisFiyati,Tarih,iskontotutar,iskontoyuzdetutar)" +
                        " values(" + fisno + "," + fkStokKarti + "," + adet + "," + AlisFiyati.ToString().Replace(",", ".") + "," +
                        SatisFiyati.ToString().Replace(",", ".") + ",'" +
                        Convert.ToDateTime(dtalislar.Rows[i]["tarih"].ToString()).ToString("yyyyMMdd") + "',0,0)";
                        int sonuc = DB.ExecuteSQL(sql);
                        if (sonuc != 1)
                            sonuc = 0;
                    }
                }
                catch (Exception exp)
                {
                    hatali++;
                }
                dtTedarikciler = DB.GetDatamdb("select * from toptanci");
                gridControl6.DataSource = dtTedarikciler;
                simpleButton8.Enabled = true;
            }
            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());

        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("delete from Tedarikciler where pkTedarikciler > 1");
            DB.ExecuteSQL("delete from Tedarikciler");
            DB.ExecuteSQL("INSERT INTO Tedarikciler (Firmaadi,Aktif,fkSirket,fkFirmaGruplari,fkil,fkilce,KaraListe,LimitBorc,tiklamaadedi,OzelKod,GeciciMusteri) VALUES('KAYITSIZ TEDARİKÇİ',1,1,1,41,7,0,0,0,1,1)");
        }
        DataTable dtHizliGrup = null;
        private void simpleButton15_Click(object sender, EventArgs e)
        {
            dtHizliGrup = DB.GetDatamdb("select * from hizlikategori");
            gridControl10.DataSource = dtHizliGrup;
            simpleButton14.Enabled = true;
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("truncate table HizliGruplar");
            for (int i = 0; i < dtHizliGrup.Rows.Count; i++)
            {
                DB.ExecuteSQL("INSERT INTO HizliGruplar (HizliGrupAdi,Aktif) values('" + dtHizliGrup.Rows[i][1].ToString() + "',1)");
            }
            DB.ExecuteSQL("UPDATE HizliGruplar SET SiraNo=pkHizliGruplar");
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Şirkete Ait Tüm Hareketleri Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            #region foregin keyleri sil
            //DB.ExecuteSQL(@"ALTER TABLE SatisDetay DROP CONSTRAINT FK_SatisDetay_Satislar");
            //DB.ExecuteSQL(@"ALTER TABLE AlisDetay DROP CONSTRAINT FK_AlisDetay_Alislar");
            #endregion

            DB.ExecuteSQL(@"
TRUNCATE TABLE KasaHareket
TRUNCATE TABLE SatisDetay
TRUNCATE TABLE SatisDetayRandevu
TRUNCATE TABLE FaturaToplu
TRUNCATE TABLE Taksitler
TRUNCATE TABLE AlisDetay
TRUNCATE table EtiketFisDetay
TRUNCATE table EtiketFisleri
TRUNCATE table StokFiyatGuncelleDetay
TRUNCATE table StokFiyatGuncelle
truncate table FirmaOzelNot
update Firmalar set Borc=0
update Firmalar set Alacak=0
update Tedarikciler set Borc=0
update Tedarikciler set Alacak=0
TRUNCATE table TedarikcilerOzelNot
TRUNCATE table BonusKullanilan
TRUNCATE table MusteriZiyaretGunleri
TRUNCATE table KampanyalarDetay
TRUNCATE table Kampanyalar
truncate table Sms
truncate table iskontolar
TRUNCATE TABLE AktarimHareketleri
TRUNCATE TABLE Logs
TRUNCATE TABLE StokDevir
truncate table StokKartiDepo
TRUNCATE TABLE DepoTransferDetay
TRUNCATE TABLE KasaGunluk
TRUNCATE TABLE SatislarSilinen");

            DB.ExecuteSQL("delete from Satislar");
            DB.ExecuteSQL("delete from Alislar");
            DB.ExecuteSQL("delete from Taksit");
            DB.ExecuteSQL("delete from DepoTransfer");
            
            DB.ExecuteSQL("DBCC CHECKIDENT ('[Satislar]', RESEED, 0)");
            DB.ExecuteSQL("DBCC CHECKIDENT ('[Alislar]', RESEED, 0)");
            DB.ExecuteSQL("DBCC CHECKIDENT ('[Taksit]', RESEED, 0)");
            DB.ExecuteSQL("DBCC CHECKIDENT ('[DepoTransfer]', RESEED, 0)");

            #region foreing key oluştur
//            DB.ExecuteSQL(@"ALTER TABLE [dbo].[SatisDetay]  WITH CHECK ADD  
//            CONSTRAINT [FK_SatisDetay_Satislar] FOREIGN KEY([fkSatislar])
//            REFERENCES [dbo].[Satislar] ([pkSatislar])");

//            DB.ExecuteSQL(@"ALTER TABLE [dbo].[AlisDetay]  WITH CHECK ADD  
//            CONSTRAINT [FK_AlisDetay_Alislar] FOREIGN KEY([fkAlislar])
//            REFERENCES [dbo].[Alislar] ([pkAlislar])");
            #endregion
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Şirkete Ait Tüm Kartlar Silinecektir. Onaylıyormusunuz \n(Stok Kartları Dahil)?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL(@"
TRUNCATE TABLE SiparisStoklari
TRUNCATE TABLE StokKartiBarkodlar
TRUNCATE TABLE StokKartiBagli
TRUNCATE TABLE StokDurum
TRUNCATE TABLE StokKartiAciklama
truncate table StokkartiDepo
truncate table Envanter
TRUNCATE TABLE EnvanterDetay
TRUNCATE TABLE StokHareket
TRUNCATE TABLE UrunlerSatisFiyatlari
TRUNCATE TABLE RenkGrupKodu
TRUNCATE TABLE BedenGrupKodu
TRUNCATE TABLE Markalar
update HizliStokSatis set fkStokKarti=0
TRUNCATE TABLE StokAltGruplari
TRUNCATE TABLE StokGruplari
TRUNCATE TABLE SatisFiyatlari
TRUNCATE TABLE Kontaklar
TRUNCATE TABLE FirmaOzelNot
TRUNCATE TABLE Firmalar
TRUNCATE TABLE Tedarikciler
TRUNCATE TABLE FirmaEpostalari
TRUNCATE TABLE SirketEpostalari
TRUNCATE TABLE GunlukKurlar
");
            DB.ExecuteSQL("DELETE FROM StokKarti");
            DB.ExecuteSQL("INSERT INTO Firmalar (Firmaadi,Aktif,fkSirket,fkFirmaGruplari,fkil,fkilce,KaraListe,LimitBorc,tiklamaadedi,OzelKod,GeciciMusteri,Borc,Alacak) VALUES('KAYITSIZ MÜŞTERİ',1,1,1,41,7,0,0,0,1,1,0,0)");
            DB.ExecuteSQL("INSERT INTO Tedarikciler (Firmaadi,Aktif,fkSirket,fkFirmaGruplari,fkil,fkilce,KaraListe,LimitBorc,tiklamaadedi,OzelKod,GeciciMusteri,Borc,Alacak) VALUES('KAYITSIZ TEDARİKÇİ',1,1,1,41,7,0,0,0,1,1,0,0)");
            //DB.ExecuteSQL("DELETE FROM Kullanicilar where pkKullanicilar>1");
            //DB.ExecuteSQL("delete from HizliGruplar where pkHizliGruplar>5");


        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Beden Grupları Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.No) return;

            DB.ExecuteSQL(@"TRUNCATE TABLE  BedenGrupKodu");

        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tedarikçilerin Tüm Hareketleri Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL(@"
DELETE FROM KasaHareket WHERE fkTedarikciler<>0
update Tedarikciler set Borc=0
update Tedarikciler set Alacak=0
");

        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Mevcut Bilgileri Sıfırlanacaktır. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            
            DB.ExecuteSQL(@"
update StokKarti set Mevcut=0
truncate table StokKartiDepo
truncate table DepoTransferDetay
delete from DepoTransfer");
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Puanları(Bonus) Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("update Firmalar set Bonus=0");
            DB.ExecuteSQL("truncate table BonusKullanilan");
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Caller ID Gelen Aramalar Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("truncate table Arayanlar");

        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Marka Modeller Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("TRUNCATE TABLE  Modeller");
            DB.ExecuteSQL("TRUNCATE TABLE  MARKALAR");
           
            XtraMessageBox.Show("Stok Marka Modeller Silindi");
        }

        private void simpleButton26_Click(object sender, EventArgs e)
        {
            dtMusteriler = DB.GetDatamdb("select * from musteriler");
            gridControl12.DataSource = dtMusteriler;
            simpleButton25.Enabled = true;
        }
        void borcsifirla(string pkfirma, string Bakiye)
        {
            string sql = @"INSERT INTO KasaHareket (fkKasalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli)
             values(@fkKasalar,0,getdate(),3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Aktarım')";
            ArrayList list0 = new ArrayList();
            list0.Add(new SqlParameter("@fkKasalar", "1"));//int.Parse(lueKasa.EditValue.ToString())));
            if (decimal.Parse(Bakiye) > 0)
            {
                list0.Add(new SqlParameter("@Alacak", Bakiye.Replace(",", ".").Replace("-", "")));
                list0.Add(new SqlParameter("@Borc", "0"));
            }
            else
            {
                list0.Add(new SqlParameter("@Alacak", "0"));
                list0.Add(new SqlParameter("@Borc", Bakiye.Replace(",", ".")));
            }
            list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
            list0.Add(new SqlParameter("@Aciklama", "Aktarım"));
            list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
            list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
            list0.Add(new SqlParameter("@fkFirma", pkfirma));
            list0.Add(new SqlParameter("@AktifHesap", "0"));
            string sonuc = DB.ExecuteSQL(sql, list0);
            if (sonuc != "0")
            {
                return;
            }
        }
        private void simpleButton25_Click(object sender, EventArgs e)
        {
            int aktarilan = 0;
            int hatali = 0;
            string sql = "";
            int c = dtMusteriler.Rows.Count;
            if (c == 0) return;
            for (int i = 0; i < c; i++)
            {
                //if (DB.GetData("select * from Firmalar where Firmaadi='" + dtMusteriler.Rows[i]["ad"].ToString() + "'").Rows.Count == 1) continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Firmaadi", dtMusteriler.Rows[i]["ad"].ToString()));//+ " " + dtMusteriler.Rows[i]["soyad"].ToString()));
                list.Add(new SqlParameter("@Yetkili", dtMusteriler.Rows[i]["soyad"].ToString()));
                list.Add(new SqlParameter("@Adres", dtMusteriler.Rows[i]["adres"].ToString()));
                list.Add(new SqlParameter("@Cep", dtMusteriler.Rows[i]["telefon"].ToString()));
                list.Add(new SqlParameter("@Tel", dtMusteriler.Rows[i]["telefon1"].ToString()));
                list.Add(new SqlParameter("@Tel2", dtMusteriler.Rows[i]["telefon2"].ToString()));
                list.Add(new SqlParameter("@OzelKod", dtMusteriler.Rows[i]["carikartnosu"].ToString())); //dtMusteriler.Rows[i]["no"].ToString()));
                list.Add(new SqlParameter("@VergiDairesi", dtMusteriler.Rows[i]["vergidairesi"].ToString()));
                sql = "INSERT INTO Firmalar (Firmaadi,Yetkili,Cep,OzelKod,Adres,Tel,Tel2,fkil,fkilce,VergiDairesi)" +
                " values(@Firmaadi,@Yetkili,@Cep,@OzelKod,@Adres,@Tel,@Tel2,41,0,@VergiDairesi) select IDENT_CURRENT('Firmalar')";

                try
                {
                    string pktedarikci = DB.ExecuteScalarSQL(sql, list);
                    if (dtMusteriler.Rows[i]["ilkborcu"].ToString() != "0" || dtMusteriler.Rows[i]["ilkborcu"].ToString() != "0,00")
                        borcsifirla(pktedarikci, dtMusteriler.Rows[i]["ilkborcu"].ToString());
                }
                catch (Exception exp)
                {
                    hatali++;
                }
            }
            DB.ExecuteSQL("UPDATE Firmalar SET OzelKod=pkFirma where OzelKod is null");
            DB.ExecuteSQL("UPDATE Firmalar SET Aktif=1");
            DB.ExecuteSQL("update Firmalar set Tel=replace(Tel,' ','')");
            DB.ExecuteSQL("update Firmalar set Cep=replace(Cep,' ','')");
            DB.ExecuteSQL("update Firmalar set Tel2=replace(Tel2,' ','')");
            DB.ExecuteSQL("update Firmalar set Tel='0262'+Tel where len(Tel)=7");
            DB.ExecuteSQL(@"update Firmalar set fkFirmaGruplari=1,fkFirmaAltGruplari=0,
fkil=7,fkilce=0,fkSirket=1,LimitBorc=0,Borc=0,Alacak=0,Bonus=0");
            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());

        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Müşteriler Silinecektir. Onaylıyormusunuz \n(Stok Kartları Dahil)?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL(@"TRUNCATE TABLE Firmalar");
            DB.ExecuteSQL("INSERT INTO Firmalar (Firmaadi,Aktif,fkSirket,fkFirmaGruplari,fkil,fkilce,KaraListe,LimitBorc,tiklamaadedi,OzelKod,GeciciMusteri,Borc,Alacak) VALUES('KAYITSIZ MÜŞTERİ',1,1,1,41,7,0,0,0,1,1,0,0)");
        }

        private void simpleButton27_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Kasa Hareketleri Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL(@"
TRUNCATE TABLE KasaHareketLog
TRUNCATE TABLE KasaHareket
TRUNCATE TABLE KasaGunluk
update Firmalar set Borc=0
update Firmalar set Alacak=0
");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbSatis2.Checked == true)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("2.Fiyat Gurubu Eklediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;
            }
            listBox1.Items.Clear();
            int aktarilan = 0;
            int hatali = 0;
            string sql = "";

            DataTable dturunler = DB.GetData("select * from [PRODYNA_MOBILE].[dbo].[URUNTANITIM]");
            for (int i = 0; i < dturunler.Rows.Count; i++)
            {
                string ID = dturunler.Rows[i]["ID"].ToString();

                if (DB.GetData("select pkStokKarti from StokKarti with(nolock) where StokKarti_id=" + ID).Rows.Count > 1) continue;

                string URKOD = dturunler.Rows[i]["URKOD"].ToString();
                string BARKOD_KODU = dturunler.Rows[i]["BARKOD_KODU"].ToString();
                if (BARKOD_KODU == "") BARKOD_KODU = URKOD;

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@StokKarti_id", ID));

                list.Add(new SqlParameter("@StokKod", URKOD));
                list.Add(new SqlParameter("@Barcode", BARKOD_KODU));
                list.Add(new SqlParameter("@fkStokGrup", "1"));
                list.Add(new SqlParameter("@Stokadi", dturunler.Rows[i]["URUNCINS"].ToString() + " " +
                    dturunler.Rows[i]["URUNCINS_2"].ToString() + " " + dturunler.Rows[i]["URUNCINS_3"].ToString() +
                    " " + dturunler.Rows[i]["URUNCINS_4"].ToString()));
                string Mevcut = "0";
                if (dturunler.Rows[i]["KALAN"].ToString() == "0,0000")
                    Mevcut = "0";//  list.Add(new SqlParameter("@Mevcut", "0"));
                else
                    Mevcut = dturunler.Rows[i]["KALAN"].ToString().Replace(",", "."); //list.Add(new SqlParameter("@Mevcut", Double.Parse(dturunler.Rows[i]["Stok"].ToString().Replace(",", "."))));
                if (dturunler.Rows[i]["FIYAT1"].ToString() == "0,0000")
                    list.Add(new SqlParameter("@AlisFiyati", "0"));
                else
                    list.Add(new SqlParameter("@AlisFiyati", dturunler.Rows[i]["FIYAT1"].ToString().Replace(",", ".")));
                decimal sf = 0;
                decimal.TryParse(dturunler.Rows[i]["SATIS_FIYAT1"].ToString(), out sf);
                //if (dturunler.Rows[i]["satis"].ToString() == "0,0000")
                //   list.Add(new SqlParameter("@SatisFiyati", "0"));
                //else
                list.Add(new SqlParameter("@SatisFiyati", sf.ToString().Replace(",", ".")));//dturunler.Rows[i]["satis"].ToString().Replace(",", ".")));
                list.Add(new SqlParameter("@Stoktipi", dturunler.Rows[i]["BIRIM"].ToString()));
                if (dturunler.Rows[i]["KDV"].ToString() == "")
                {
                    list.Add(new SqlParameter("@KdvOrani", "0"));//dturunler.Rows[i]["KDV"].ToString()));
                    list.Add(new SqlParameter("@KdvOraniAlis", "0"));//dturunler.Rows[i]["KDV"].ToString()));
                }
                else
                {
                    list.Add(new SqlParameter("@KdvOrani", dturunler.Rows[i]["KDV"].ToString()));
                    list.Add(new SqlParameter("@KdvOraniAlis", dturunler.Rows[i]["KDV"].ToString()));
                }
                if (dturunler.Rows[i]["AKTIF"].ToString() == "1")
                    list.Add(new SqlParameter("@Aktif", "1"));
                else
                    list.Add(new SqlParameter("@Aktif", "0"));

                string Marka = dturunler.Rows[i]["URUNCINS_2"].ToString();

                string fkMarka = "0";
                DataTable dtgrup = DB.GetData("select * from Markalar where Marka='" + Marka + "'");
                if (dtgrup.Rows.Count == 0)
                {
                    DB.ExecuteSQL("INSERT INTO Markalar (Marka) VALUES('" + Marka + "')");
                    fkMarka = DB.GetData("select Max(pkMarka) from Markalar").Rows[0][0].ToString();
                }
                else
                    fkMarka = dtgrup.Rows[0]["pkMarka"].ToString();
                list.Add(new SqlParameter("@fkMarka", fkMarka));
                //pkRenkGrupKodu başla
                string RenkGrup = dturunler.Rows[i]["URUNCINS_3"].ToString();

                string fkRenkGrupKodu = "0";
                DataTable dtRenkGrup = DB.GetData("select * from RenkGrupKodu where Aciklama='" + RenkGrup + "'");
                if (dtRenkGrup.Rows.Count == 0)
                {
                    DB.ExecuteSQL("INSERT INTO RenkGrupKodu (Aciklama) VALUES('" + RenkGrup + "')");
                    fkRenkGrupKodu = DB.GetData("select Max(pkRenkGrupKodu) from RenkGrupKodu").Rows[0][0].ToString();
                }
                else
                    fkRenkGrupKodu = dtRenkGrup.Rows[0]["pkRenkGrupKodu"].ToString();
                list.Add(new SqlParameter("@fkRenkGrupKodu", fkRenkGrupKodu));
                //pkRenkGrupKodu bitir
                //fkBedenGrupKodu başla
                string BedenGrup = dturunler.Rows[i]["URUNCINS_4"].ToString();

                string fkBedenGrupKodu = "0";
                DataTable dtBedenGrup = DB.GetData("select * from BedenGrupKodu where Aciklama='" + BedenGrup + "'");
                if (dtBedenGrup.Rows.Count == 0)
                {
                    DB.ExecuteSQL("INSERT INTO BedenGrupKodu (Aciklama) VALUES('" + BedenGrup + "')");
                    fkBedenGrupKodu = DB.GetData("select Max(pkBedenGrupKodu) from BedenGrupKodu").Rows[0][0].ToString();
                }
                else
                    fkBedenGrupKodu = dtBedenGrup.Rows[0]["pkBedenGrupKodu"].ToString();
                list.Add(new SqlParameter("@fkBedenGrupKodu", fkBedenGrupKodu));
                //fkBedenGrupKodu bitir
                sql = "INSERT INTO StokKarti (StokKod,Barcode,fkStokGrup,Stokadi,Stoktipi,Mevcut,AlisFiyati,SatisFiyati,KdvOrani,KdvOraniAlis,Aktif,KritikMiktar,fkMarka,fkRenkGrupKodu,fkBedenGrupKodu,StokKarti_id,satisadedi)" +
                " values(@StokKod,@Barcode,@fkStokGrup,@Stokadi,@Stoktipi," + Mevcut + ",@AlisFiyati,@SatisFiyati,@KdvOrani,@KdvOraniAlis,@Aktif,0,@fkMarka,@fkRenkGrupKodu,@fkBedenGrupKodu,@StokKarti_id,1) select IDENT_CURRENT('StokKarti')";
                try
                {
                    string pkStokKarti = DB.ExecuteScalarSQL(sql, list);
                    if (pkStokKarti.Substring(0, 1) == "H")
                    {
                        listBox1.Items.Add(dturunler.Rows[i]["URKOD"].ToString() + pkStokKarti);
                        continue;
                    }
                    aktarilan++;
                    DB.ExecuteSQL("update  StokKarti set HizliSatisAdi=substring(StokAdi,0,19)");
                    DB.ExecuteSQL("insert into SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,Aktif)" +
                        " values(" + pkStokKarti + ",1," + dturunler.Rows[i]["SATIS_FIYAT1"].ToString().Replace(",", ".") + ",0,0,1)");
                    if (cbSatis2.Checked)
                        DB.ExecuteSQL("insert into SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,Aktif)" +
        " values(" + pkStokKarti + ",2," + dturunler.Rows[i]["SATIS_FIYAT2"].ToString().Replace(",", ".") + ",0,0,1)");
                }
                catch (Exception exp)
                {
                    hatali++;
                }
            }
            DB.ExecuteSQL("update StokKarti set Barcode=pkStokKarti where Barcode is null");
            DB.ExecuteSQL("update StokKarti set HizliSatisAdi=rtrim(HizliSatisAdi)");

            DB.ExecuteSQL("update StokKarti set UreticiKodu=Barcode where UreticiKodu is null");
            DB.ExecuteSQL("update StokKarti set SatisFiyati=SatisFiyati/1000000");
            DB.ExecuteSQL("update StokKarti set AlisFiyati=AlisFiyati/1000000");
            DB.ExecuteSQL("update StokKarti set SatisFiyatiKdvHaric=SatisFiyati-(SatisFiyati*KdvOrani)/(100+KdvOrani)");
            DB.ExecuteSQL("update SatisFiyatlari set SatisFiyatiKdvli=SatisFiyatiKdvli/1000000");
            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());
        }

        private void simpleButton28_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Yazdırılmış Etiketler Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("truncate table EtiketBas");
            DB.ExecuteSQL("truncate table EtiketBasDetay");
        }

        private void btnStokKartiSil_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Kartları Silinecektir. Onaylıyormusunuz.?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("TRUNCATE TABLE StokKartiBarkodlar");
            DB.ExecuteSQL("TRUNCATE TABLE SatisFiyatlari");
            DB.ExecuteSQL("TRUNCATE TABLE StokKarti");
        }

        private void simpleButton29_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Grup Alt Gruplar Silinecektir. Onaylıyormusunuz.?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL(@"
TRUNCATE TABLE StokAltGruplari
TRUNCATE TABLE StokGruplari");
        }

        private void simpleButton30_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE StokKarti SET Stokadi=substring(StokAdi,0,charindex('|',StokAdi)) where Stokadi like '%|%'");
        }

        private void simpleButton31_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tedarikçiler ve Hareketleri Müşteri Olarak Aktarılacak. Onaylıyormusunuz.?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DataTable dt = DB.GetData("select * from Tedarikciler");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pkTedarikcilereski = dt.Rows[i]["pkTedarikciler"].ToString();

                string sql = @"insert into Firmalar (Firmaadi,Tel,Fax,Adres,webadresi,Eposta,Tel2,Aktif,fkil,fkilce,VergiDairesi,VergiNo,Unvani,Cep,Cep2,KaraListe,LimitBorc,tiklamaadedi,OzelKod,fkFirmaAltGruplari,Borc,Alacak,Bonus,fkPerTeslimEden,KayitTarihi,FaturaUnvani)
                            values(@Firmaadi,@Tel,@Fax,@Adres,@webadresi,@Eposta,@Tel2,@Aktif,@fkil,@fkilce,@VergiDairesi,@VergiNo,@Unvani,@Cep,@Cep2,@KaraListe,@LimitBorc,0,@pkFirma,1,0,0,0,1,getdate(),@FaturaUnvani)
                            select IDENT_CURRENT('Firmalar') ";
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Firmaadi", dt.Rows[i]["Firmaadi"].ToString()));
                list.Add(new SqlParameter("@Tel", dt.Rows[i]["Tel"].ToString()));
                list.Add(new SqlParameter("@Fax", dt.Rows[i]["Fax"].ToString()));
                list.Add(new SqlParameter("@Adres", dt.Rows[i]["Adres"].ToString()));
                list.Add(new SqlParameter("@webadresi", dt.Rows[i]["webadresi"].ToString()));
                list.Add(new SqlParameter("@Eposta", dt.Rows[i]["Eposta"].ToString()));
                list.Add(new SqlParameter("@Tel2", dt.Rows[i]["Tel2"].ToString()));
                list.Add(new SqlParameter("@Aktif", dt.Rows[i]["Aktif"].ToString()));
                list.Add(new SqlParameter("@fkil", dt.Rows[i]["fkil"].ToString()));
                list.Add(new SqlParameter("@fkilce", dt.Rows[i]["fkilce"].ToString()));
                list.Add(new SqlParameter("@VergiDairesi", dt.Rows[i]["VergiDairesi"].ToString()));
                list.Add(new SqlParameter("@VergiNo", dt.Rows[i]["VergiNo"].ToString()));
                list.Add(new SqlParameter("@Unvani", dt.Rows[i]["Unvani"].ToString()));
                list.Add(new SqlParameter("@Cep", dt.Rows[i]["Cep"].ToString()));
                list.Add(new SqlParameter("@Cep2", dt.Rows[i]["Cep2"].ToString()));
                list.Add(new SqlParameter("@KaraListe", dt.Rows[i]["KaraListe"].ToString()));
                list.Add(new SqlParameter("@LimitBorc", dt.Rows[i]["LimitBorc"].ToString().Replace(",", ".")));
                list.Add(new SqlParameter("@pkFirma", dt.Rows[i]["pkTedarikciler"].ToString()));
                //list.Add(new SqlParameter("@OzelKod", dt.Rows[i]["@OzelKod"].ToString()));
                //list.Add(new SqlParameter("@fkFirmaAltGruplari", dt.Rows[i]["@fkFirmaAltGruplari"].ToString()));
                list.Add(new SqlParameter("@Borc", "0"));//dt.Rows[i]["@Borc"].ToString().Replace(",",".")));
                list.Add(new SqlParameter("@Alacak", "0"));//dt.Rows[i]["@Alacak"].ToString().Replace(",", ".")));
                //list.Add(new SqlParameter("@Bonus", dt.Rows[i]["@Bonus"].ToString().Replace(",", ".")));
                //list.Add(new SqlParameter("@fkPerTeslimEden", dt.Rows[i]["@fkPerTeslimEden"].ToString()));
                //list.Add(new SqlParameter("@KayitTarihi", dt.Rows[i]["@KayitTarihi"].ToString()));
                //list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", dt.Rows[i]["@fkSatisFiyatlariBaslik"].ToString()));
                list.Add(new SqlParameter("@FaturaUnvani", dt.Rows[i]["Firmaadi"].ToString()));

                string pkTedarikcileryeni = DB.ExecuteScalarSQL(sql, list);
                int id = 0;
                int.TryParse(pkTedarikcileryeni, out id);
                try
                {
                    if (id > 0)
                    {
                        sql = "update Alislar set fkFirma=" + pkTedarikcileryeni + " where fkFirma=" + pkTedarikcilereski;
                        DB.ExecuteSQL(sql);
                        DB.ExecuteSQL("DELETE FROM Tedarikciler WHERE pkTedarikciler=" + pkTedarikcilereski);
                    }
                    else
                    {
                        MessageBox.Show("Hata var.");
                        continue;
                    }

                }
                catch (SqlException exp)
                {
                    MessageBox.Show("HATA OLUŞTU." + exp.Message);
                    continue;
                }
            }
        }

        private void simpleButton32_Click(object sender, EventArgs e)
        {
            rapor_adi.Tag = "0";
            rapor_adi.Focus();
        }

        private void simpleButton33_Click(object sender, EventArgs e)
        {
            string id = "0", sql;
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@rapor_adi", rapor_adi.Text));
            list.Add(new SqlParameter("@rapor_sql", meSql.Text));

            if (rapor_adi.Tag.ToString() == "0")
                sql = "insert into OzelRaporlar (rapor_adi,rapor_sql) values(@rapor_adi,@rapor_sql) select IDENT_CURRENT('OzelRaporlar')";
            else
                sql = "update OzelRaporlar set rapor_adi=@rapor_adi,rapor_sql=@rapor_sql where ozel_rapor_id=" + rapor_adi.Tag.ToString();
            id = DB.ExecuteScalarSQL(sql, list);

            rapor_adi.Tag = id;

            OzelRaporlar();
        }
        void OzelRaporlar()
        {
            cgOzelRaporlar.DataSource = DB.GetData("select * from OzelRaporlar with(nolock)");
        }
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPage10)
            {
                OzelRaporlar();
            }
        }

        private void gridView14_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView14.FocusedRowHandle < 0) return;

            gcSql.DataSource = null;
            gridView2.Columns.Clear();
            gridView2.BeginDataUpdate();
            for (int i = 0; i < gridView2.Columns.Count; i++)
            {
                gridView14.Columns.RemoveAt(i);
            }

            DataRow dr = gridView14.GetDataRow(gridView14.FocusedRowHandle);

            rapor_adi.Tag = dr["ozel_rapor_id"].ToString();
            rapor_adi.Text = dr["rapor_adi"].ToString();
            meSql.Text = dr["rapor_sql"].ToString();


            if (meSql.Text != "")
                gcSql.DataSource = DB.GetData(dr["rapor_sql"].ToString());


            gridView2.EndDataUpdate();
        }

        private void simpleButton34_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("delete from OzelRaporlar where ozel_rapor_id=" + rapor_adi.Tag.ToString());
            OzelRaporlar();
        }

        private void btnCalistir_Click(object sender, EventArgs e)
        {
            gridView15.BeginDataUpdate();
            gcSql.DataSource = null;
            gridView15.Columns.Clear();
            for (int i = 0; i < gridView15.Columns.Count; i++)
            {
                gridView15.Columns.RemoveAt(i);
            }
            gridView15.EndDataUpdate();
            gcSql.DataSource = DB.GetData(meSql.Text);
        }

        private void simpleButton35_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Kasa Giriş Çıkış Türleri ve Grupları Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL(@"TRUNCATE TABLE KasaGirisCikisGruplari");

            DB.ExecuteSQL(@"TRUNCATE TABLE KasaGirisCikisTurleri");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Açık Kalan Satışlar Temizlenecek. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL(@"delete from Satislar where pkSatislar in(select pkSatislar from Satislar s 
                    left join SatisDetay sd on sd.fkSatislar=s.pkSatislar
                    where sd.fkSatislar is null)");

            DB.ExecuteSQL(@"delete from SatisDetay where fkSatislar in(select pkSatislar from Satislar WHERE Siparis=0)");
            DB.ExecuteSQL(@"delete from Satislar WHERE Siparis=0");
        }

        private void simpleButton36_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL(@"update StokKarti set Stokadi=upper(Stokadi)");
        }

        private void simpleButton37_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL(@"update Firmalar set Firmaadi=upper(Firmaadi)");
            DB.ExecuteSQL(@"update Tedarikciler set Firmaadi=upper(Firmaadi)");
        }

        private void simpleButton38_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(deTarihi.DateTime.ToString("yyyy-MM-dd") + " Tarihinden Önceki Satışlar Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;


            int sonuc = 0;

            string sql = "";

            sql = @"delete from KasaHareket where fkSatislar in(
            select pkSatislar from Satislar with(nolock)
            where GuncellemeTarihi<'" + deTarihi.DateTime.ToString("yyyy-MM-dd") + "'";
            if (seMusteriid.Value == 1)
                sql = sql + " and fkFirma=1";

            sql = sql + ")";
            sonuc = DB.ExecuteSQL(sql);

            sql=@"delete from SatisDetay where fkSatislar in(
            select pkSatislar from Satislar with(nolock)
            where GuncellemeTarihi<'" + deTarihi.DateTime.ToString("yyyy-MM-dd") + "'";
            if (seMusteriid.Value == 1)
                sql = sql + " and fkFirma=1";

            sql = sql + ")";
            sonuc = DB.ExecuteSQL(sql);

            //if (sonuc == 0) return;

            //if (sonuc == 0) return;

            sql = "delete from Satislar where GuncellemeTarihi<'" + deTarihi.DateTime.ToString("yyyy-MM-dd") + "'";
            if (seMusteriid.Value == 1)
                sql = sql + " and fkFirma=1";

            sonuc = DB.ExecuteSQL(sql);

            MessageBox.Show(sonuc + " Veriler Silindi");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("Dış Veri Alırken Excel 2003 formatında ve ilk Sayfa Adı Sayfa1 Olmalı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();

                if (openFileDialog1.FileName == "") return;
                OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OleDb.4.0;Data Source = " +
                    openFileDialog1.FileName + " ; Extended Properties = Excel 8.0");//" ; Extended Properties = Excel 8.0");
                //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
                OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
                //MessageBox.Show((System.Exception)(con.ServerVersion).Message);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int hatali = 0,basarili=0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string MUSTERIKODU = dt.Rows[i]["MUSTERIKODU"].ToString();
                    string MUSTERIADI = dt.Rows[i]["MUSTERIADI"].ToString();
                    if (MUSTERIADI == "") continue;
                    string BAKIYE = dt.Rows[i]["BAKIYE"].ToString();
                    string GRUBU = dt.Rows[i]["GRUBU"].ToString();

                    if (BAKIYE == "") BAKIYE = "0";

                    #region Firma Gruplari  ekle

                    DataTable dtG = DB.GetData("select * from FirmaGruplari with(nolock) where GrupAdi='" + GRUBU + "'");
                    if (dtG.Rows.Count == 0)
                        GRUBU = DB.ExecuteScalarSQL("insert into FirmaGruplari (GrupAdi,Aktif) values('" + GRUBU + "',1) select IDENT_CURRENT('FirmaGruplari')");
                    else
                        GRUBU = dtG.Rows[0][0].ToString();

                    #endregion
                    if (checkBox1.Checked)
                    {
                        if (MUSTERIKODU == "") MUSTERIKODU = "-1";
                    }
                    DataTable dtFirma = DB.GetData("select * from Firmalar with(nolock) where OzelKod='" + MUSTERIKODU + "'");
                    string pkFirma = "0";
                    if (dtFirma.Rows.Count == 0)
                    {
                        ArrayList list = new ArrayList();
                        list.Add(new SqlParameter("@OzelKod", ""));
                        list.Add(new SqlParameter("@Firmaadi", MUSTERIADI));
                        list.Add(new SqlParameter("@FaturaUnvani", MUSTERIADI));
                        list.Add(new SqlParameter("@fkFirmaGruplari", GRUBU));
                        list.Add(new SqlParameter("@Devir", BAKIYE.Replace(",", ".")));

                        string sql = "INSERT INTO Firmalar (OzelKod,Firmaadi,FaturaUnvani,fkFirmaGruplari,Devir,Aktif,KayitTarihi)" +
                        " values(@OzelKod,@FaturaUnvani,@Firmaadi,@fkFirmaGruplari,@Devir,1,getdate()) select IDENT_CURRENT('Firmalar')";

                        try
                        {
                            pkFirma = DB.ExecuteScalarSQL(sql, list);

                            #region sonuç başarılı ise kasa hareketine devir ekle

                            sql = @"delete from KasaHareket where fkFirma=@fkFirma INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,BilgisayarAdi)
                            values(1,1,getdate(),3,1,@Borc,@Alacak,'Aktarım',0,1,@fkFirma,0,'Kasa Bakiye Düzeltme',@Tutar,'Aktarım')";

                            sql = sql.Replace("@fkFirma", pkFirma);
                            sql = sql.Replace("@Tutar", "0");//Devir.Replace(",", "."));

                            decimal ddevir = 0;
                            decimal.TryParse(BAKIYE.Replace(".", ","), out ddevir);//NOKTA VİRGÜL DECİMALDE FARKLI
                            if (ddevir > 0)
                            {
                                sql = sql.Replace("@Alacak", ddevir.ToString().Replace(",", ".").Replace("-", ""));
                                sql = sql.Replace("@Borc", "0");
                            }
                            else
                            {
                                sql = sql.Replace("@Alacak", "0");
                                sql = sql.Replace("@Borc", ddevir.ToString().Replace(",", ".").Replace("-", ""));
                            }

                            int sonuc1 = DB.ExecuteSQL_Sonuc_Sifir(sql);
                            if (sonuc1 != 0)
                            {
                                hatali = hatali + 1;
                                //hatalı aktardıysa sil
                                DB.ExecuteSQL("delete from Firmalar where pkFirma=" + pkFirma);
                                pkFirma = "0";
                            }
                            else
                                basarili = basarili + 1;
                            #endregion
                        }
                        catch (Exception exp)
                        {
                            hatali = hatali + 1;
                            pkFirma = "0";
                        }
                    }
                    else
                    {
                        pkFirma = dtFirma.Rows[0]["pkFirma"].ToString();
                    }
                }
                MessageBox.Show("Hatalı-Başarılı Kayıt : " + hatali.ToString() + "-" + basarili.ToString());
            }
            
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("Dış Veri Alırken Excel 2003 formatında ve ilk Sayfa Adı Sayfa1 Olmalı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();

                if (openFileDialog1.FileName == "") return;

                OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OleDb.4.0;Data Source = " +
                    openFileDialog1.FileName + " ; Extended Properties = Excel 8.0");//" ; Extended Properties = Excel 8.0");
                //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
                OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
                //MessageBox.Show((System.Exception)(con.ServerVersion).Message);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int hatali=0,basarili=0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string TEDARIKCIKODU = dt.Rows[i]["TEDARIKCIKODU"].ToString();
                    string TEDARICIADI = dt.Rows[i]["TEDARICIADI"].ToString();

                    if (TEDARICIADI == "") continue;

                    string BAKIYE = dt.Rows[i]["BAKIYE"].ToString();
                    string GRUBU = dt.Rows[i]["GRUBU"].ToString();

                    if (BAKIYE == "") BAKIYE = "0";

                    #region Firma Gruplari  ekle

                    DataTable dtG = DB.GetData("select * from TedarikcilerGruplari with(nolock) where GrupAdi='" + GRUBU + "'");
                    if (dtG.Rows.Count == 0)
                        GRUBU = DB.ExecuteScalarSQL("insert into TedarikcilerGruplari (GrupAdi,Aktif) values('" + GRUBU + "',1) select IDENT_CURRENT('FirmaGruplari')");
                    else
                        GRUBU = dtG.Rows[0][0].ToString();

                    #endregion

                    DataTable dtTedarikciler = DB.GetData("select * from Tedarikciler with(nolock) where OzelKod='" + TEDARIKCIKODU + "'");
                    string pkTedarikciler = "0";
                    if (dtTedarikciler.Rows.Count == 0)
                    {
                        ArrayList list = new ArrayList();
                        list.Add(new SqlParameter("@OzelKod", "0"));
                        list.Add(new SqlParameter("@Firmaadi", TEDARICIADI));
                        list.Add(new SqlParameter("@fkFirmaGruplari", GRUBU));
                        list.Add(new SqlParameter("@Devir", BAKIYE.Replace(",", ".")));

                        string sql = "INSERT INTO Tedarikciler (OzelKod,Firmaadi,fkFirmaGruplari,Devir,Aktif,KayitTarihi)" +
                        " values(@OzelKod,@Firmaadi,@fkFirmaGruplari,@Devir,1,getdate()) select IDENT_CURRENT('Tedarikciler')";

                        try
                        {
                            pkTedarikciler = DB.ExecuteScalarSQL(sql, list);

                            if (pkTedarikciler.Substring(0, 1) == "H")
                                hatali = hatali + 1;
                            else
                            {
                                #region sonuç başarılı ise kasa hareketine devir ekle

                                sql = @"delete from KasaHareket where fkTedarikciler=@fkTedarikciler INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,BilgisayarAdi)
                    values(1,1,getdate(),3,1,@Borc,@Alacak,'Aktarım',0,1,0,@fkTedarikciler,'Kasa Bakiye Düzeltme',@Tutar,'Aktarım')";

                                sql = sql.Replace("@fkTedarikciler", pkTedarikciler);
                                sql = sql.Replace("@Tutar", "0");

                                decimal ddevir = 0;
                                decimal.TryParse(BAKIYE.Replace(".", ","), out ddevir);
                                if (ddevir > 0)
                                {
                                    sql = sql.Replace("@Borc", ddevir.ToString().Replace(",", ".").Replace("-", ""));
                                    sql = sql.Replace("@Alacak", "0");
                                }
                                else
                                {
                                    sql = sql.Replace("@Borc", "0");
                                    sql = sql.Replace("@Alacak", ddevir.ToString().Replace(",", ".").Replace("-", ""));
                                }

                                int sonuc1 = DB.ExecuteSQL_Sonuc_Sifir(sql);
                                if (sonuc1 != 0)
                                {
                                    hatali = hatali + 1;
                                    DB.ExecuteSQL("delete from Tedarikciler where pkTedarikciler=" + pkTedarikciler);
                                    pkTedarikciler = "0";
                                }
                                else
                                    basarili = basarili + 1;
                                #endregion
                            }
                        }
                        catch (Exception exp)
                        {
                            pkTedarikciler = "0";
                        }
                    }
                    else
                    {
                        pkTedarikciler = dtTedarikciler.Rows[0]["pkTedarikciler"].ToString();
                    }
                }
                MessageBox.Show("Hatalı-Başarılı Kayıt : " + hatali.ToString() + "-" + basarili.ToString());
                
            }
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Yeni\v.xls;Extended Properties=”Excel 8.0;HDR=Yes;IMEX=1″
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

            //müşteri aktarım
            //if (cbSatis2.Checked == true)
            //{
            //    DialogResult secim;
            //    secim = DevExpress.XtraEditors.XtraMessageBox.Show("2.Fiyat Gurubu Eklediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (secim == DialogResult.No) return;
            //}
            listBox1.Items.Clear();
            int aktarilan = 0;
            int hatali = 0;
            string sql = "";

            DataTable dturunler = DB.GetData("select * from [PRODYNA_MOBILE].[dbo].[MUSTKAYIT]");
            for (int i = 0; i < dturunler.Rows.Count; i++)
            {
                string ID = dturunler.Rows[i]["ID"].ToString();
                string GURUPID = dturunler.Rows[i]["GURUP_ID"].ToString();
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Firmaadi", dturunler.Rows[i]["AD"].ToString() + ' ' + dturunler.Rows[i]["SOYAD"].ToString()));

                if (dturunler.Rows[i]["EVTEL"].ToString()=="")
                    list.Add(new SqlParameter("@Tel", ""));
                else
                    list.Add(new SqlParameter("@Tel", dturunler.Rows[i]["EVTEL"].ToString()));

                if (dturunler.Rows[i]["GSM"].ToString()=="")
                    list.Add(new SqlParameter("@Cep", ""));
                else
                    list.Add(new SqlParameter("@Cep", dturunler.Rows[i]["GSM"].ToString()));


                if (dturunler.Rows[i]["MAIL"].ToString() == "")
                    list.Add(new SqlParameter("@Eposta", ""));
                else
                    list.Add(new SqlParameter("@Eposta", dturunler.Rows[i]["MAIL"].ToString()));

                string adres = dturunler.Rows[i]["EVMH"].ToString() +
                    ' ' + dturunler.Rows[i]["EVCD"].ToString() +
                    ' ' + dturunler.Rows[i]["SOKAKNO"].ToString()+
                    ' ' + dturunler.Rows[i]["EVAPT"].ToString();

                if(adres=="")
                    list.Add(new SqlParameter("@Adres", ""));
                else
                    list.Add(new SqlParameter("@Adres", adres));

                
                //

                string eviladi = dturunler.Rows[i]["EVIL"].ToString();
                string evil_id = "0";
                if (eviladi == "")
                    list.Add(new SqlParameter("@fkil", ""));
                else
                {
                    DataTable dtil = DB.GetData("select * from Sehirler where ADI='" + eviladi+"'");

                    if (dtil.Rows.Count > 0)
                    {
                        list.Add(new SqlParameter("@fkil", dtil.Rows[0]["KODU"].ToString()));
                        evil_id = dtil.Rows[0]["KODU"].ToString();
                    }
                    else
                        list.Add(new SqlParameter("@fkil", ""));
                }

                string evilceadi = dturunler.Rows[i]["EVILCE"].ToString();
                if (evilceadi == "")
                    list.Add(new SqlParameter("@fkilce", ""));
                else
                {
                    DataTable dtilce = DB.GetData("sph_ilce_ara " + evil_id + ",'" + evilceadi + "'");
                    if (dtilce.Rows.Count==0)
                        list.Add(new SqlParameter("@fkilce", ""));
                    else
                        list.Add(new SqlParameter("@fkilce", dtilce.Rows[0]["KODU"]));
                }

                //if (GURUPID == "")
                //    list.Add(new SqlParameter("@fkilce", ""));
                //else
                //{
                //    DataTable dtilce = DB.GetData("select * from GURUPID " + evil_id + ",'" + evilceadi + "'");
                //    if (dtilce.Rows.Count==0)
                //        list.Add(new SqlParameter("@fkilce", ""));
                //    else
                //        list.Add(new SqlParameter("@fkilce", dtilce.Rows[0]["KODU"]));
                //}
                

                list.Add(new SqlParameter("@OzelKod", ID));
                string VergiDairesi = dturunler.Rows[i]["VERGIDAIRE"].ToString();
                string VergiNo = dturunler.Rows[i]["VERGIDAIRENO"].ToString();

                if (VergiDairesi=="")
                    list.Add(new SqlParameter("@VergiDairesi", ""));
                else
                    list.Add(new SqlParameter("@VergiDairesi", VergiDairesi));

                if (VergiNo == "")
                    list.Add(new SqlParameter("@VergiNo", ""));
                else
                     list.Add(new SqlParameter("@VergiNo", VergiNo));

                string devir= dturunler.Rows[i]["BAKIYE"].ToString();
                decimal tutar = 0;
                decimal.TryParse(devir.ToString(), out tutar);
                tutar = tutar / 1000000;

                if (tutar ==0)
                    list.Add(new SqlParameter("@Devir", "0"));
                else
                    list.Add(new SqlParameter("@Devir", tutar.ToString().Replace(",", ".").Replace("-", "")));

                sql = "INSERT INTO Firmalar (Firmaadi,fkFirmaGruplari,Tel,Cep,fkil,fkilce,Eposta,Adres,OzelKod,VergiNo,VergiDairesi,Aktif,Devir)" +
                    " values(@Firmaadi,1,@Tel,@Cep,@fkil,@fkilce,@Eposta,@Adres,@OzelKod,@VergiNo,@VergiDairesi,1,@Devir) select IDENT_CURRENT('Firmalar')";
                try
                {
                    string pkFirmalar = DB.ExecuteScalarSQL(sql, list);
                    if (pkFirmalar.Substring(0, 1) == "H")
                    {
                        listBox1.Items.Add(dturunler.Rows[i]["URKOD"].ToString() + pkFirmalar);
                        continue;
                    }
                    aktarilan++;

                    #region Bakiye
                    string sql2 = @"INSERT INTO KasaHareket (fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,aktarildi)
                    values(@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Bakiye Düzeltme',@Tutar," + Degerler.AracdaSatis + ")";
                    ArrayList list1 = new ArrayList();
                    list1.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

                    
                    if (tutar > 0)
                    {
                        list1.Add(new SqlParameter("@Alacak", tutar.ToString().Replace(",", ".").Replace("-", "")));
                        list1.Add(new SqlParameter("@Borc", "0"));
                    }
                    else
                    {
                        list1.Add(new SqlParameter("@Alacak", "0"));
                        list1.Add(new SqlParameter("@Borc", tutar.ToString().Replace(",", ".").Replace("-", "")));
                    }

                    list1.Add(new SqlParameter("@Tutar", tutar.ToString().Replace(",", ".")));

                    list1.Add(new SqlParameter("@Tipi", int.Parse("1")));
                    list1.Add(new SqlParameter("@Aciklama", "Aktarım"));
                    list1.Add(new SqlParameter("@donem", DateTime.Now.Month));
                    list1.Add(new SqlParameter("@yil", DateTime.Now.Year));
                    list1.Add(new SqlParameter("@fkFirma", pkFirmalar));
                    list1.Add(new SqlParameter("@AktifHesap", "0"));
                    list1.Add(new SqlParameter("@Tarih", DateTime.Today));
                    string sonuc2 = DB.ExecuteSQL(sql2, list1);
                    if (sonuc2 != "0")
                    {
                        //ceBakiye.Value = 0;
                        // MessageBox.Show("İşlem Başarılı");
                    }

                    #endregion
                 }
                catch (Exception exp)
                {
                    hatali++;
                }
            }
            //DB.ExecuteSQL("update StokKarti set SatisAdedi=1 where isnull(SatisAdedi,0)=0");
            //DB.ExecuteSQL("update StokKarti set Barcode=pkStokKarti where Barcode is null");
            //DB.ExecuteSQL("update StokKarti set HizliSatisAdi=rtrim(HizliSatisAdi)");

            //DB.ExecuteSQL("update StokKarti set UreticiKodu=Barcode where UreticiKodu is null");
            //DB.ExecuteSQL("update StokKarti set SatisFiyati=SatisFiyati/1000000");
            //DB.ExecuteSQL("update StokKarti set AlisFiyati=AlisFiyati/1000000");

            DB.ExecuteSQL("update Firmalar set Devir=Devir/1000000");

            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());
        }

        private void simpleButton39_Click(object sender, EventArgs e)
        {
            frmKullanicilar kul = new frmKullanicilar();
            kul.ShowDialog();
        }

        private void simpleButton40_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Randevular ve Hatırlatmalar Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("truncate table Hatirlatma");
            DB.ExecuteSQL("truncate table HatirlatmaAnimsat");
            DB.ExecuteSQL("TRUNCATE TABLE SatisDetayRandevu");
        }

        private void btnTaksit_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Taksitler Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("truncate table Taksitler");
            DB.ExecuteSQL("delete from Taksit");
            DB.ExecuteSQL("DBCC CHECKIDENT ('[Taksit]', RESEED, 0)");

        }

        private void simpleButton41_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm HatirlatmaAnimsat Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("truncate table HatirlatmaAnimsat");
        }

        private void simpleButton42_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Renkleri Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("TRUNCATE TABLE  RenkGrupKodu"); 

            XtraMessageBox.Show("Stok Renkleri Silindi");
        }

        private void simpleButton43_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Personeller Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("TRUNCATE TABLE PersonellerFirma");
            DB.ExecuteSQL("TRUNCATE TABLE PersonelResim");
            DB.ExecuteSQL("TRUNCATE TABLE Personeller");

            XtraMessageBox.Show("Personeller Silindi");
            
        }

        private void simpleButton44_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton45_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Grupları Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("TRUNCATE TABLE FirmaGruplari");

            XtraMessageBox.Show("Müşteri Grupları Silindi");
        }

        private void simpleButton44_Click_1(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Grupları Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("TRUNCATE TABLE StokAltGruplari");
            DB.ExecuteSQL("TRUNCATE TABLE StokGruplari");

            XtraMessageBox.Show("Stok Gruplarını Silindi");
        }

        private void frmAktarim_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton47_Click(object sender, EventArgs e)
        {
            DataTable dtDepolar=
            DB.GetData("select * from Depolar with(nolock) where Aktif=1");
            for (int i = 0; i < dtDepolar.Rows.Count; i++)
            {
                DB.ExecuteSQL("insert into StokKartiDepo" +
                " select pkStokKarti,"+ dtDepolar.Rows[i]["pkDepolar"].ToString() + ",Mevcut,0,0,0,0,getdate(),0 from StokKarti");
            }
           

        }

        private void simpleButton46_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton48_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Açıklamaları Silinecektir. Onaylıyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("TRUNCATE TABLE StokKartiAciklama");

            XtraMessageBox.Show("Stok Açıklamaları Silindi");
        }
    }
}
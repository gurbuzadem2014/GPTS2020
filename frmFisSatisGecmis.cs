using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using DevExpress.XtraReports.UI;
using System.IO;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmFisSatisGecmis : DevExpress.XtraEditors.XtraForm
    {
        bool FisDuzenle = false;
        string Dosya = DB.exeDizini + "\\FisSatisGecmisGrid.xml";
        public frmFisSatisGecmis(bool _FisDuzenle)
        {
            InitializeComponent();
            FisDuzenle = _FisDuzenle;

            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 40;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 40;
        }

        private void frmFisNoBilgisi_Load(object sender, EventArgs e)
        {
            btnFisDuzenle.Visible = FisDuzenle;

            SatislarveOdemeler();

            MusteriBakiye();

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }


            if (DB.GetData("select count(*) from Taksit with(nolock) where fkSatislar=" + fisno.Text).Rows[0][0].ToString() != "0")
            {
                lbUyari.Text = "Satışa Ait Taksit(ler) var!";
            }
            else
                lbUyari.Text = "";

            Yetkiler();

            KullaniciYetkileri();

            gridView1.Focus();
        }

        void Odemeleri()
        {
            string sql = @"select OdemeSekli,kh.Borc as Odenen,kh.Alacak as Verilen,Tarih from KasaHareket kh with(nolock)
            where fkSatislar=@fkSatislar
            union all
            select 'Açık Hesap',AcikHesap,0,Tarih from Satislar with(nolock)
            where pkSatislar=@fkSatislar";

            sql = sql.Replace("@fkSatislar", fisno.Text);
            gcOdemeler.DataSource = DB.GetData(sql);
        }

        void KullaniciYetkileri()
        {
            DataTable dt =
            DB.GetData(@"select m.pkModuller,m.Kod,m.ModulAdi,m.ana_id,m.durumu,m.Kod,my.pkModullerYetki,
            my.Yetki from Moduller m with(nolock)
            left join ModullerYetki my with(nolock) on my.Kod=m.Kod
            where m.Kod in('1.2','1.3') and my.fkKullanicilar=" + DB.fkKullanicilar);
            foreach (DataRow dr in dt.Rows)
            {
                string kod = dr["Kod"].ToString();
                string Yetki = dr["Yetki"].ToString();

                if (kod == "1.2" && Yetki=="False")
                    btnFisDuzenle.Enabled = false;
                else if (kod == "1.3" && Yetki == "False")
                    simpleButton3.Enabled = false;

            }
        }

        void Yetkiler()
        {
            string sql = @"SELECT ya.Yetki, p.Aciklama10,ya.Sayi,isnull(p.Aktif,0) as Aktif FROM  YetkiAlanlari ya with(nolock)  
            INNER JOIN Parametreler p with(nolock) ON ya.fkParametreler = p.pkParametreler
            WHERE ya.fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);

            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string aciklama = dtYetkiler.Rows[i]["Aciklama10"].ToString();
                bool yetki = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                string sayi = dtYetkiler.Rows[i]["Sayi"].ToString();
                //bool aktif = Convert.ToBoolean(dtYetkiler.Rows[i]["Aktif"]);

                if (aciklama == "AlisFiyati")
                {
                    gridColumn31.Visible = yetki;
                    gridColumn20.Visible = yetki;
                    gridColumn22.Visible = yetki;
                }
            }
        }

        void SatislarveOdemeler()
        {
            DataTable dtSatislar = DB.GetData("exec sp_Satislar " + fisno.Text);
            if (dtSatislar.Rows.Count == 0)
            {
                //Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            SatisDurumu.Text = dtSatislar.Rows[0]["SatisDurumu"].ToString();
            SatisDurumu.Tag = dtSatislar.Rows[0]["fkSatisDurumu"].ToString();
            SatisTarih.Text = dtSatislar.Rows[0]["GuncellemeTarihi"].ToString();
            txtFaturaTarihi.Text = dtSatislar.Rows[0]["FaturaTarihi"].ToString();
            txtTeklifTarihi.Text = dtSatislar.Rows[0]["TeklifTarihi"].ToString();
            KayitTarihi.Text = dtSatislar.Rows[0]["Tarih"].ToString();
            KullaniciAdiSoyadi.Tag = dtSatislar.Rows[0]["fkKullanici"].ToString();
            KullaniciAdiSoyadi.Text = dtSatislar.Rows[0]["KullaniciAdiSoyadi"].ToString();
            float Alinan = 0;
            if(dtSatislar.Rows[0]["AlinanPara"].ToString()!="")
             Alinan = float.Parse(dtSatislar.Rows[0]["AlinanPara"].ToString());
            AlinanPara.Text = Convert.ToDouble(Alinan).ToString("##0.00");
            float SatisTutar = float.Parse(dtSatislar.Rows[0]["ToplamTutar"].ToString());
            float iskontoFaturaTutar = 0;
            if(dtSatislar.Rows[0]["iskontoFaturaTutar"].ToString()!="")
               iskontoFaturaTutar = float.Parse(dtSatislar.Rows[0]["iskontoFaturaTutar"].ToString());

            if (Alinan!=0)
                ParaUstu.Text = Convert.ToDouble(Alinan - SatisTutar).ToString("##0.00");
            ceiskontoTutar.EditValue = iskontoFaturaTutar;

            gridControl1.DataSource = DB.GetData("exec sp_SatisDetay " + fisno.Text + ",0");
            //
            if (gridColumn5.SummaryItem.SummaryValue == null)
                Satis4Toplam.Text = "0,0";
            else
            {
                float aratop = float.Parse(gridColumn5.SummaryItem.SummaryValue.ToString());
                Satis4Toplam.Text = Convert.ToDouble(aratop).ToString("##0.00");
            }
            //ceiskontoTutar
            if (gridColumn23.SummaryItem.SummaryValue == null)
                ceiskontoTutar.Value = 0;
            else
            {
                float isktop = 0;
                float.TryParse(gridColumn23.SummaryItem.SummaryValue.ToString(),out isktop);
                ceiskontoTutar.EditValue = isktop;
            }


            memoEdit1.Text = dtSatislar.Rows[0]["Aciklama"].ToString();
            groupControl1.Tag = dtSatislar.Rows[0]["fkFirma"].ToString();


            groupControl1.Text = DB.GetData("select OzelKod from Firmalar where pkFirma=" + groupControl1.Tag.ToString()).Rows[0][0].ToString() 
            +"-" + dtSatislar.Rows[0]["Firmaadi"].ToString();

            decimal OncekiBakiye=0;
            decimal.TryParse(dtSatislar.Rows[0]["OncekiBakiye"].ToString(),out OncekiBakiye);
            lbSatisBakiye.Text = Convert.ToDouble(OncekiBakiye).ToString("##0.00");

            if (dtSatislar.Rows[0]["Siparis"].ToString() != "True")
            {
                simpleButton3.Enabled = false;
                btnFisDuzenle.Enabled = false;
            }

            Odemeleri();
        }

        void MusteriBakiye()
        {
            DataTable dt = DB.GetData("exec sp_MusteriBakiyesi " + groupControl1.Tag.ToString() + ",0");
            if (dt.Rows.Count == 0)
            {
                Satis1Toplam.Text = "0,00";
            }
            else
            {
                decimal bakiye = decimal.Parse(dt.Rows[0][0].ToString());
                Satis1Toplam.Text = bakiye.ToString("##0.00");//dt.Rows[0][0].ToString();
            }
        }

        void bonusdus()
        {
            string sqlbonus = "";
            sqlbonus = "update Firmalar set Bonus=Bonus-s.BonusTutar From Satislar s with(nolock) " +
                     " where  Firmalar.pkFirma=s.fkFirma and s.pkSatislar=" + fisno.Text;
            DB.ExecuteSQL(sqlbonus);

            //bonus ile ödeme alınandan sil
            sqlbonus = @"update Firmalar set Firmalar.Bonus=Firmalar.Bonus+BonusKullanilan.Bonus from BonusKullanilan
            where  Firmalar.pkFirma=BonusKullanilan.fkFirma and BonusKullanilan.fkSatislar=@pkSatislar";
            sqlbonus = sqlbonus.Replace("@pkSatislar", fisno.Text);
            DB.ExecuteSQL(sqlbonus);
            //sil
            DB.ExecuteSQL("delete from BonusKullanilan where fkSatislar=" + fisno.Text);
        }

        void MevcutSatisGeriAl()
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string Adet = dr["Adet"].ToString();
                decimal miktar = 0;
                decimal.TryParse(Adet,out miktar);
                if (miktar<0)
                    DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=isnull(Mevcut,0)-" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                    " where pkStokKarti=" + fkStokKarti);
                else
                    DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=isnull(Mevcut,0)+" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                    " where pkStokKarti=" + fkStokKarti);
            }
        }

        void MevcutDepoGeriAl()
        {
            if (Degerler.DepoKullaniyorum)
            {
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    DataRow dr = gridView1.GetDataRow(i);
                    string fkStokKarti = dr["fkStokKarti"].ToString();
                    string fkDepolar = dr["fkDepolar"].ToString();
                    string Adet = dr["Adet"].ToString();
                    decimal miktar = 0;
                    decimal.TryParse(Adet, out miktar);

                    if (miktar < 0)
                        DB.ExecuteSQL("UPDATE StokKartiDepo SET MevcutAdet=MevcutAdet-" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                        " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar);
                    else
                        DB.ExecuteSQL("UPDATE StokKartiDepo SET MevcutAdet=MevcutAdet+" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                        " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar);
                }
            }
        }

        void TaksitleriSil(string fkSatislar)
        {
            DB.ExecuteSQL("delete from Taksit where fkSatislar="+fkSatislar);
            DB.ExecuteSQL("delete from Taksitler where taksit_id=(select taksit_id from Taksit where fkSatislar=" + fkSatislar + ")");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            //string s = formislemleri.MesajBox("Satış Fişi Silinsin mi?", Degerler.mesajbaslik, 3, 2);
            //if (s == "0") return;

            DataTable dtSatislar = DB.GetData("select * from Satislar with(nolock) where pkSatislar=" + fisno.Text);
            if (dtSatislar.Rows.Count == 0)
            {
                formislemleri.Mesajform("Fiş Bulunamadı.","K",200);
                return;
            }

            string fkSatisDurumu = dtSatislar.Rows[0]["fkSatisDurumu"].ToString();
            //string fkCek = dtSatislar.Rows[0]["fkCek"].ToString();
            string fkFirma = dtSatislar.Rows[0]["fkFirma"].ToString();
            
            string mesaj = "";
            //bool taksitvarmi = false;
            //if (DB.GetData("select count(*) from Taksit with(nolock) where fkSatislar=" + fisno.Text).Rows[0][0].ToString() != "0")
            //{
            //    mesaj = "Satışa Ait Taksit(ler) var! \n";
            //    taksitvarmi = true;
            //}

            //bool faturasivar = false;
            //fatura kesildi mi?
            if (DB.GetData("select count(*) from SatisDetay with(nolock) where isnull(fkFaturaToplu,0)<>0 and fkSatislar=" + fisno.Text).Rows[0][0].ToString() != "0")
            {
                mesaj = mesaj + "Faturası Kesilmiş Hizmetler var! \n";
                //faturasivar = true;
            }
            if (DB.GetData("select count(*) from Taksit with(nolock) where fkSatislar=" + fisno.Text).Rows[0][0].ToString() != "0")
            {
                mesaj = mesaj + "Satışa Ait Taksit(ler) var! \n";
            }

            mesaj = mesaj + "Fişi Silmek İstediğinize Eminmisiniz. \n Not: Ödemeler ve Stok Adetleri Geri Alınacaktır";


            string sec = formislemleri.MesajBox(mesaj, "Satış Sil", 3, 0);
            if (sec != "1") return;

            #region çek durumunu değiştir
            //if (fkCek != "")
            //{
            //    DB.ExecuteSQL("update Cekler set fkCekTuru=0,fkFirma=0 where pkCek=" + fkCek);
            //}
            #endregion

            DB.ExecuteSQL("delete from FaturaToplu " +
             " where pkFaturaToplu in(select fkFaturaToplu from SatisDetay where fkSatislar=" + fisno.Text + ")");

            DB.ExecuteSQL("DELETE FROM SatisDetay WHERE fkSatislar=" + fisno.Text);
            DB.ExecuteSQL("DELETE FROM KasaHareket where fkSatislar=" + fisno.Text);
            DB.ExecuteSQL("DELETE FROM Satislar WHERE pkSatislar=" + fisno.Text);

            
            DB.ExecuteSQL("delete from Taksitler where taksit_id=(select taksit_id from Taksit where fkSatislar=" + fisno.Text + ")");
            DB.ExecuteSQL("delete from Taksit where fkSatislar=" + fisno.Text);

            DB.ExecuteSQL("delete from HatirlatmaAnimsat where fkSatislar=" + fisno.Text);

            //DB.ExecuteSQL("update SatisDetay set fkFaturaToplu=null where fkSatislar=" + fisno.Text);
            //if (faturasivar)
            //{
            //    DB.ExecuteSQL("delete from FaturaToplu where pkFaturaToplu in(" +
            //    " select fkFaturaToplu from SatisDetay  SD with(nolock)" +
            //    " left join FaturaToplu FT with(nolock) on FT.fkSatislar=SD.fkSatislar" +
            //    " where SD.fkSatislar=" + fisno.Text + "group by fkFaturaToplu)");
            //}  

            inputForm sifregir1 = new inputForm();
            sifregir1.Text = "Silinme Nedeni";
            sifregir1.GirilenCaption.Text = "Silme Nedenini Giriniz!";
            sifregir1.Girilen.Text = "İptal";
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir1.ShowDialog();

          

            if (SatisDurumu.Tag.ToString() != "1" && SatisDurumu.Tag.ToString() != "10" && SatisDurumu.Tag.ToString() != "11")
            {
                MevcutSatisGeriAl();
                MevcutDepoGeriAl();
                bonusdus();
            }

            DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,fkKullanicilar,HataAciklama) VALUES(2,'" +
               fisno.Text + "-Fiş Silindi Tutar="+ Satis4Toplam.Text.Replace(",",".")+"',getdate(),1," + DB.fkKullanicilar + ",'" + sifregir1.Girilen.Text + "')");

            DB.ExecuteSQL("INSERT INTO SatislarSilinen (fkSatislar,Aciklama,Tarih,fkKullanicilar,fkFirma) VALUES(" + fisno.Text + ",'" +
                sifregir1.Girilen.Text + "',getdate()," + DB.fkKullanicilar+","+groupControl1.Tag.ToString()+")");
             
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            btnFisDuzenle.Tag = "0";
            Close();
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //if (!formislemleri.SifreIste()) return;

            DataTable dtSatislar = DB.GetData("select * from Satislar with(nolock) where pkSatislar=" + fisno.Text);
            if (dtSatislar.Rows.Count == 0)
            {
                formislemleri.Mesajform("Fiş Bulunamadı.", "K", 200);
                return;
            }

            string fkSatisDurumu = dtSatislar.Rows[0]["fkSatisDurumu"].ToString();
            string fkCek = dtSatislar.Rows[0]["fkCek"].ToString();
            string fkFirma = dtSatislar.Rows[0]["fkFirma"].ToString();

            if (Degerler.fkKullaniciGruplari != "1")
            {
                if (KullaniciAdiSoyadi.Tag.ToString() != DB.fkKullanicilar)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Bu Fişi Düzenleme Yetkiniz Bulunmamaktadır.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
           
            bool faturasivar = false;
            string mesaj = "";
            if (DB.GetData("select count(*) from SatisDetay with(nolock) where isnull(fkFaturaToplu,0)>0 and fkSatislar=" + fisno.Text).Rows[0][0].ToString() != "0")
            {
                mesaj = mesaj + "Toplu Faturası Kesilmiş Hizmetler var! \n";
                //faturasivar = true;
            }
            if (DB.GetData("select count(*) from Taksit with(nolock) where fkSatislar=" + fisno.Text).Rows[0][0].ToString() != "0")
            {
                mesaj = mesaj + "Satışa Ait Taksit(ler) var! \n";
            }

            mesaj = mesaj + "Fişi Düzeltmek İstediğinize Eminmisiniz. \n Not: Ödemeler ve Stok Adetleri Geri Alınacaktır";


            string sec = formislemleri.MesajBox(mesaj, "Satış Sil", 3, 0);
            if (sec != "1") return;

            DB.ExecuteSQL("UPDATE Satislar SET duzenleme_tarihi=getdate(),Siparis=0 where pkSatislar=" + fisno.Text);

            //hesaplar ve mevcutları geri alma durumu
            if (fkSatisDurumu != "1" && fkSatisDurumu != "10" && fkSatisDurumu != "11")
            {
                //string fkFirma = groupControl1.Tag.ToString();
                //alacak
                //DB.ExecuteSQL("UPDATE Firmalar SET Alacak=Alacak-" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkFirma=" + fkFirma);
                //borç
                //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc-" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkFirma=" + fkFirma);
                //Devir
                //DB.ExecuteSQL("UPDATE Firmalar SET Devir=Devir-" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkFirma=" + fkFirma);

                bonusdus();

                MevcutSatisGeriAl();
                
                MevcutDepoGeriAl();
            }

            //kasa hareketlerini sil
             DB.ExecuteSQL("DELETE FROM KasaHareket where fkSatislar=" + fisno.Text);
            //fatura altı iskontoları sil
             DB.ExecuteSQL("update SatisDetay set Faturaiskonto=0 where fkSatislar=" + fisno.Text);
            //taksitler
             DB.ExecuteSQL("delete from Taksitler where taksit_id=(select taksit_id from Taksit where fkSatislar=" + fisno.Text + ")");
             DB.ExecuteSQL("delete from Taksit where fkSatislar=" + fisno.Text);
            DB.ExecuteSQL("delete from HatirlatmaAnimsat where fkSatislar=" + fisno.Text);

            DB.ExecuteSQL("delete from FaturaToplu " +
              " where pkFaturaToplu in(select fkFaturaToplu from SatisDetay where fkSatislar=" + fisno.Text + ")");

             DB.ExecuteSQL("update SatisDetay set fatura_durumu=null,fkFaturaToplu=null where fkSatislar=" + fisno.Text);

            #region çek durumunu değiştir

            if (fkCek != "")
            {
                DB.ExecuteSQL("update Cekler set fkCekTuru=0,fkFirma=0 where pkCek=" + fkCek);
                DB.ExecuteSQL("delete from HatirlatmaAnimsat where fkCek=" + fkCek);
            }
            //çeki verdiği için çek iade yapması gerekiyor
            //DataTable dt =
            //DB.GetData("select fkCek from Satislar with(nolock) where pkSatislar=" + fisno.Text);

            //DB.ExecuteSQL("update Cekler set fkCekTuru=10,fkFirma=0 where pkCek=" +
            //    dt.Rows[0]["fkCek"].ToString());
            #endregion

            

            FisDuzenle = true;
            this.btnFisDuzenle.Tag = "1";
            Close();
        }

        void FisYazdir(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli,Tutar from KasaHareket with(nolock) where fkSatislar=" + fisid);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                DataTable Musteri = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }

                xrCariHareket rapor = new xrCariHareket();
                //if (yazdirmaadedi>1)
                //  rapor.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);

                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = SatisFisTipi;
                rapor.Report.Name = SatisFisTipi;
                if (Disigner)
                    rapor.ShowDesigner();
                else
                {
                    //if (yazdirmaadedi < 1) yazdirmaadedi = 1;

                    //for (int i = 0; i < yazdirmaadedi; i++)
                        rapor.Print(YaziciAdi);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void FisYazdir1(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                if (Fis.Rows.Count == 0)
                {
                    MessageBox.Show("Satış Bulunamadı");
                    return;
                }
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select top 1 * from Sirketler");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                //Bakiye bilgileri
                DataTable Bakiye = DB.GetData(@"select dbo.fon_MusteriBakiyesi(fkFirma) as Bakiye,ToplamTutar as FisTutar,dbo.fon_MusteriBakiyesi(fkFirma)-ToplamTutar as OncekiBakiye from Satislar
where pkSatislar=" + pkSatislar);
                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);
                //Firma bilgileri
                DataTable Musteri = DB.GetData("select * from Firmalar where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string YaziciAdi = "", YaziciDosyasi = "";
            DataTable dtYazicilar =
            DB.GetData("SELECT * FROM SatisFisiSecimi with(nolock) where Sec=1");
            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                //19.12.2015
                if (SatisDurumu.Tag.ToString() == ((int)Degerler.SatisDurumlari.Değişim).ToString())
                    SatisDurumu.Tag = ((int)Degerler.SatisDurumlari.Satış).ToString();
                if (SatisDurumu.Tag.ToString() == ((int)Degerler.SatisDurumlari.İade).ToString())
                    SatisDurumu.Tag = ((int)Degerler.SatisDurumlari.Satış).ToString();

                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1, int.Parse(SatisDurumu.Tag.ToString()));
                YaziciAyarlari.ShowDialog();
                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;
                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }
            if (YaziciAdi != "")
            {
                FisYazdir(false, fisno.Text, YaziciDosyasi, YaziciAdi);
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            if (fisno.Text == DB.GetData("select mAX(pkSatislar) from Satislar").Rows[0][0].ToString())
            {
                MessageBox.Show("Son Kayıt");
                return;
            }
            int fno=0;
            int.TryParse(fisno.Text, out fno);
            fno = fno + 1;
            fisno.Text = fno.ToString();
            SatislarveOdemeler();
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            if (fisno.Text == "1")
            {
                MessageBox.Show("ilk Kayıt");
                return;
            }
            int fno = 0;
            int.TryParse(fisno.Text, out fno);
            fno = fno - 1;
            fisno.Text = fno.ToString();

            SatislarveOdemeler();
        }

        private void frmFisNoBilgisi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if(e.KeyCode==Keys.F11)
                simpleButton4_Click(sender, e);
        }

        private void gridView1_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;
            Rectangle rect = e.Bounds;
            ControlPaint.DrawBorder3D(e.Graphics, e.Bounds);
            Brush brush =
                e.Cache.GetGradientBrush(rect, e.Column.AppearanceHeader.BackColor,
                e.Column.AppearanceHeader.BackColor2, e.Column.AppearanceHeader.GradientMode);
            rect.Inflate(-1, -1);
            // Fill column headers with the specified colors.
            e.Graphics.FillRectangle(brush, rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            // Draw the filter and sort buttons.
            foreach (DevExpress.Utils.Drawing.DrawElementInfo info in e.Info.InnerElements)
            {
                try
                {
                    DevExpress.Utils.Drawing.ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
                }
                catch (Exception exp)
                {
                }
            }
            e.Handled = true;
        }

        private void ceiskontoyuzde_KeyDown(object sender, KeyEventArgs e)
        {
            float iskontofaturayuzde=0;
            float.TryParse(ceiskontoyuzde.EditValue.ToString(), out iskontofaturayuzde);

            //DB.ExecuteSQL();

        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void etiketBasımıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkStokKarti = dr["pkStokKarti"].ToString();
            //DataTable dt = DB.GetData("select pkStokKarti,SatisAdedi,KutuFiyat as icindekiadet from Stokkarti with(nolock) where pkStokKarti="+
            //    fkStokKarti);
            //if (dt.Rows.Count == 0) return;
            int adet = 1;
            int.TryParse(dr["Adet"].ToString(), out adet);
            formislemleri.EtiketBas(fkStokKarti, adet);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(fisno.Value.ToString());
        }

        private void yazdırDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,0);
            string pkSatislar = fisno.Value.ToString();
            DB.pkSatislar = int.Parse(pkSatislar);
            YaziciAyarlari.Tag = 1;
            YaziciAyarlari.ShowDialog();
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["fkStokKarti"].ToString();
            StokHareketleri.ShowDialog();

            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0");
            //SatisUrunBazindaDetay.Tag = "2";
            //SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            //SatisUrunBazindaDetay.ShowDialog();
        }

        private void satışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti = dr["fkStokKarti"].ToString();
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.Tag = "1";
            SatisFiyatlari.pkStokKarti.Text = fkStokKarti;
            SatisFiyatlari.ShowDialog();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            //string Dosya = DB.exeDizini + "\\FisSatisGecmisGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            //string Dosya = DB.exeDizini + "\\FisSatisGecmisGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }

        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = groupControl1.Tag.ToString();
            CariHareketMusteri.Show();
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriKarti MusteriKarti = new frmMusteriKarti(groupControl1.Tag.ToString(), "");
            MusteriKarti.ShowDialog();
        }

        private void alışFaturasıİçinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExceleGonder.datatabletoexel(fisno.Text);
        }

        private void excelGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\FisSatisGecmis" + fisno.Text + ".Xls";
            gridView1.ExportToXls(dosyaadi);
            System.Diagnostics.Process.Start(dosyaadi);
        }

        private void ePostaGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında");
        }
        short yazdirmaadedi = 1;
        private void sBtnEpostaGonder_Click(object sender, EventArgs e)
        {
            string fkfirma = "0", FisNo = "0", fkSatisDurumu = "0", eposta = "@";

            fkfirma = groupControl1.Tag.ToString();//dr["fkFirma"].ToString();
            FisNo = fisno.Text;
            //fkSatisDurumu = dr["Durumu"].ToString();
            //if (fkSatisDurumu == "Teklif")
            fkSatisDurumu = "2";
            DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkfirma);
            //DataTable dtFirma = DB.GetData("select * From Firmalar with(nolock) where pkFirma=" + fkfirma);
            eposta = Musteri.Rows[0]["eposta"].ToString();


            inputForm sifregir = new inputForm();
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir.GirilenCaption.Text = "E-Posta Adresi Giriniz";
            sifregir.Girilen.Text = eposta;

            sifregir.ShowDialog();
            eposta = sifregir.Girilen.Text;

            if (eposta.Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(eposta + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;


            #region  yazıcı Seçimi
            string YaziciAdi = "", YaziciDosyasi = "";

            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" + fkSatisDurumu); //+ lueSatisTipi.EditValue.ToString());

            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1, 2);//int.Parse(lueSatisTipi.EditValue.ToString()));

                YaziciAyarlari.ShowDialog();

                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }
            #endregion 

            if (YaziciAdi == "")
            {
                MessageBox.Show("Yazıcı Bulunamadı");
                return;
            }
            // else
            //FisYazdir(dizayner, pkSatisBarkod.Text, YaziciDosyasi, YaziciAdi);

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\" + YaziciDosyasi + ".repx");
            rapor.Name = "Teklif";
            rapor.Report.Name = "Teklif.repx";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + FisNo + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + FisNo);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli from KasaHareket with(nolock) where fkSatislar=" + FisNo);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                //DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);


                string dosyaadi = Application.StartupPath + "\\" + YaziciDosyasi + ".pdf";

                rapor.DataSource = ds;
                //rapor.DataSource = gridControl2.DataSource;
                //rapor.FilterString = "[ID]=1";
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(eposta, " Fiş Bilgisi", dosyaadi, groupControl1.Text + "Fiş No=" + FisNo);

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void groupControl3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
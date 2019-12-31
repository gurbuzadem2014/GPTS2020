using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraReports.Design;
using DevExpress.XtraTab;
using System.Threading;
using System.Management;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmAyarlar : DevExpress.XtraEditors.XtraForm
    {
        int pkSirket = 1;
        public int _form_id = 0;
        public frmAyarlar(int form_id)
        {
            InitializeComponent();
            _form_id = form_id;
        }
        private void frmKurumDonem_Load(object sender, EventArgs e)
        {
            //if (Degerler.ip_adres == "185.130.56.98")
            //    Close();

            //if (Degerler.isProf == false)
            //{
            //    inputForm sifregir = new inputForm();
            //    sifregir.ShowDialog();
            //    if (sifregir.Girilen.Text != "9999")
            //        Close();
            //}
            if (!formislemleri.SifreIste()) Close();

            vProiller();

            vProillerVarsayilan();

            Birimler();

            Sirketler();

            gridView1.Focus();
            gridView1.SelectRow(0);

            sirketgetir();

            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\logo.jpg";
            if (File.Exists(dosya))
            {
                FileStream bitmapFile = new FileStream(dosya, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Image loaded = new Bitmap(bitmapFile);
                bitmapFile.Dispose();
                pictureEdit1.Image = loaded;
            }
            //pictureEdit1.LoadImage(dosya);

            if (_form_id == 1)//giriş ise
                xTabControl.SelectedTabPage = xtraTabPage1;
            if (_form_id == 2)//giriş ise
                xTabControl.SelectedTabPage = xtisyeri;

            luzaksqlbaglimi.Text = DB.uzaksqlbaglandimi.ToString();
        }

        void Birimler()
        {
            lueBirimler.Properties.DataSource = DB.GetData("SELECT * FROM Birimler with(nolock) where Aktif=1 order by Varsayilan desc");
        }

        void sirketgetir()
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            pkSirket = int.Parse(dr["pkSirket"].ToString());

            DataTable dt = DB.GetData("select * from Sirketler with(nolock) where pkSirket=" + pkSirket.ToString());

            tESirket.Text = dt.Rows[0]["Sirket"].ToString();
            txtYetkili.Text = dt.Rows[0]["Yetkili"].ToString(); 
            klasoryol.Text = dt.Rows[0]["yedekalinacakyer"].ToString();
            BarkodNo.Text = dt.Rows[0]["BarkodNo"].ToString();
            if (dt.Rows[0]["WebKullanir"].ToString() == "True")
                WebKullanir.Checked = true;
            else
                WebKullanir.Checked = false;
            cbSektor.Text = dt.Rows[0]["Sektor"].ToString();
            //cbFaturaTipiSatış
            int ft = 0;
            int.TryParse(dt.Rows[0]["FaturaTipi"].ToString(),out ft);
            cbFaturaTipi.SelectedIndex = ft;
            //cbFaturaTipiAlis
            int fta = 0;
            int.TryParse(dt.Rows[0]["FaturaTipiAlis"].ToString(), out fta);
            cbFaturaTipiAlis.SelectedIndex = fta;

            if (dt.Rows[0]["StokTakibi"].ToString() == "True")
                cbStokTakibi.Checked = true;
            if (dt.Rows[0]["MusteriZorunluUyari"].ToString() == "True")
                cbMusteriZorunluUyari.Checked = true;
            if (dt.Rows[0]["TedarikciZorunluUyari"].ToString() == "True")
                cbTedarikciZorunluUyari.Checked = true;
            if (dt.Rows[0]["Callerid"].ToString() == "True")
                cbCallerid.Checked = true;
            cbSektor.Text = dt.Rows[0]["Sektor"].ToString();

            seTeraziBarkoduBasi.EditValue = dt.Rows[0]["TeraziBarkoduBasi"].ToString();
            seTeraziBarkoduBasi2.EditValue = dt.Rows[0]["TeraziBarkoduBasi2"].ToString();
            seTeraziBarkoduBasi3.EditValue = dt.Rows[0]["TeraziBarkoduBasi3"].ToString();

            MusteriOzelKod.EditValue = dt.Rows[0]["MusteriBasNo"].ToString();
            bonusyuzde.EditValue = dt.Rows[0]["BonusYuzde"].ToString();
            teVergiDairesi.Text = dt.Rows[0]["VergiDairesi"].ToString();
            teVergiNo.Text = dt.Rows[0]["VergiNo"].ToString();
            WebSitesi.Text = dt.Rows[0]["WebSitesi"].ToString();
            if (dt.Rows[0]["BonusTur"].ToString()!="")
            BonusTur.SelectedIndex = int.Parse(dt.Rows[0]["BonusTur"].ToString());

            teHost.Text = dt.Rows[0]["Host"].ToString();
            teGonderenEposta.Text = dt.Rows[0]["GonderenEposta"].ToString();
            //string epostasifresi = islemler.CryptoStreamSifreleme.Decrypt("Hitit999", dt.Rows[0]["GonderenEpostaSifre"].ToString());
            string epostasifresi = islemler.CryptoStreamSifreleme.md5SifreyiCoz(dt.Rows[0]["GonderenEpostaSifre"].ToString());
            teGonderenEpostaSifre.Text = epostasifresi;//dt.Rows[0]["GonderenEpostaSifre"].ToString();
            tePort.Text = dt.Rows[0]["Port"].ToString();
            teSirketAdresi.Text = dt.Rows[0]["Adres"].ToString();
            dateEdit2.EditValue = dt.Rows[0]["YedekSaati"].ToString();
            teTel.Text = dt.Rows[0]["TelefonNo"].ToString();
            teFax.Text = dt.Rows[0]["Fax"].ToString();
            teCep.Text = dt.Rows[0]["Cep"].ToString();

            teEposta.Text = dt.Rows[0]["Eposta"].ToString();
            teEpostabcc.Text = dt.Rows[0]["bccEposta"].ToString();
            teEpostacc.Text = dt.Rows[0]["ccEposta"].ToString();
            //sms
            tbKullaniciAdi.Text = dt.Rows[0]["sms_kullaniciadi"].ToString();

            string coz = DB.DecodeFrom64(dt.Rows[0]["sms_sifre"].ToString());
            tbSmsSifre.Text = coz;

            tbSmsBaslik.Text = dt.Rows[0]["sms_baslik"].ToString();

            cbKdvorani.Text = dt.Rows[0]["kdv_orani"].ToString();
            cbAlisKdv.Text = dt.Rows[0]["kdv_orani_alis"].ToString();
            
            cbBirimi.SelectedIndex = int.Parse(dt.Rows[0]["stok_birimi"].ToString());
            txtOzelSifre.Text = dt.Rows[0]["OzelSifre"].ToString();

            if ( dt.Rows[0]["satisadedi_icindekiadetdir"].ToString()=="True")
                cbeSatisadedi_icindekiadet.Checked = true;

            if (dt.Rows[0]["ikincifiyatgoster"].ToString() == "True")
                cbikincifiyat.Checked = true;
            if (dt.Rows[0]["ucuncufiyatgoster"].ToString() == "True")
                cbucuncufiyat.Checked = true;

            if (dt.Rows[0]["uruneklendisescal"].ToString() == "True")
            {
                cbUruneklendisescal.Checked = true;
                Degerler.Uruneklendisescal = true;
            }

            if (dt.Rows[0]["satiskopyala"].ToString() == "True")
            {
                cbSatiskopyala.Checked = true;
                Degerler.Satiskopyala = true;
            }

            string odemesekli = dt.Rows[0]["odemesekli"].ToString();
            if (odemesekli == "")
                comboBoxEdit4.Text = "Nakit";
            else
                comboBoxEdit4.Text = odemesekli;

            if (dt.Rows[0]["EnableSsl"].ToString() == "True")
            {
                cbEnableSsl.Checked = true;
                Degerler.EnableSsl = true;
            }

            if (dt.Rows[0]["OncekiFiyatHatirla"].ToString() == "True")
            {
                cbOncekiFiyatHatirla.Checked = true;
                Degerler.OncekiFiyatHatirla = true;
            }

            tbUreticiBarkodNo.Text = dt.Rows[0]["uretici__barkod_no"].ToString();

            if (dt.Rows[0]["tedarikciler_giderdir"].ToString() == "True")
            {
                cbTadarikcilerGiderdir.Checked = true;
            }

            if (dt.Rows[0]["is_kamp_odeme_sekli_acikhesap"].ToString() == "1")
            {
                cbKampanyaAcikHesap.Checked = true;
            }

            if (dt.Rows[0]["makbuz_yazdir"].ToString() == "True")
            {
                ceMakbuzyazdir.Checked = true;
            }
            if (dt.Rows[0]["oto_guncelle"].ToString() == "True")
            {
                cbOtoGuncelle.Checked = true;
            }
            if (dt.Rows[0]["tedarikci_OncekiFiyatHatirla"].ToString() == "True")
            {
                cbOncekiFiyatHatirla_teda.Checked = true;
                Degerler.OncekiFiyatHatirla_teda = true;
            }

            #region il ilçe varsayılan
            if (dt.Rows[0]["fkKodu"].ToString() != "")
            {
                LUESehir.EditValue = int.Parse(dt.Rows[0]["fkKodu"].ToString());
            }
            if (dt.Rows[0]["fkAltGrup"].ToString() != "")
            {
                lookUpilce.EditValue = int.Parse(dt.Rows[0]["fkAltGrup"].ToString());
            }
            //varsayılan
            if (dt.Rows[0]["fkKoduDefault"].ToString() != "")
            {
                lueil.EditValue = int.Parse(dt.Rows[0]["fkKoduDefault"].ToString());
            }
            if (dt.Rows[0]["fkAltGrupDefault"].ToString() != "")
            {
                lueilce.EditValue = int.Parse(dt.Rows[0]["fkAltGrupDefault"].ToString());
            }

            if (dt.Rows[0]["DepoKullaniyorum"].ToString() == "True")
            {
                cbDepoKullaniyorum.Checked = true;
                Degerler.DepoKullaniyorum = true;
            }
            if (dt.Rows[0]["stok_karti_dizayn"].ToString() == "True")
            {
                cbStokKartiDizayn.Checked = true;
                Degerler.StokKartiDizayn = true;
            }

            if (dt.Rows[0]["BitisTarihi"].ToString() != "")
            {
                deBitisTarihi.DateTime = Convert.ToDateTime(dt.Rows[0]["BitisTarihi"].ToString());
            }

            if (dt.Rows[0]["fkBirimler"].ToString() != "")
            {
                lueBirimler.EditValue = int.Parse(dt.Rows[0]["fkBirimler"].ToString());
            }
            #endregion
            txtEFaturaKul.Text = dt.Rows[0]["e_fatura_kul"].ToString();
            txtEFaturaSifre.Text = dt.Rows[0]["e_fatura_sifre"].ToString();
            txtEFaturaWebservis.Text=dt.Rows[0]["e_fatura_url"].ToString();
        }

        void Sirketler()
        {
            gridControl1.DataSource = DB.GetData("select * from Sirketler with(nolock)");
        }

        

        void ServerKaydet()
        {
            string sql = "update Kullanicilar set GuncellemeTarihi=getdate(),Aciklama ='" + tESirket.Text + "',HDDNo='" + teHDD.Text + "' where Eposta='" + teEposta.Text + "'";
            string cs = "Data Source=sql2012.isimtescil.net;Initial Catalog=hitityazilim_db9999;Persist Security Info=True;User ID=hitityazilim_adem;Password=Hitit9999";
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand(sql, con);

            int count = 0;
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                count = -1;
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama) values(0,'" +
                //e.Message.ToString().Replace("'", "") + "',getdate(),0,"+sql+")");
                DB.logayaz("ExecuteSQL Hatası Oluştur: " + e.Message.ToString(),sql);
                //throw e;
            }
            catch (Exception e)
            {
                //count = -1;
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                //e.Message.ToString().Replace("'","") + ",getdate(),0)");
                //throw e;
                DB.logayaz("ExecuteSQL Hatası Oluştur: " + e.Message.ToString(), sql);
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (cbFaturaTipi.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen Fatura Tipini Seçiniz");
                cbFaturaTipi.Focus();
                return;
            }
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Sektor", cbSektor.Text));
            DB.Sektor = cbSektor.Text;
            list.Add(new SqlParameter("@Sirket", tESirket.Text));
            list.Add(new SqlParameter("@yedekalinacakyer", klasoryol.Text));

            if (WebKullanir.Checked)
            list.Add(new SqlParameter("@WebKullanir", true));
            else
                list.Add(new SqlParameter("@WebKullanir", false));

            list.Add(new SqlParameter("@FaturaTipi", cbFaturaTipi.SelectedIndex));
            list.Add(new SqlParameter("@FaturaTipiAlis", cbFaturaTipiAlis.SelectedIndex));
            list.Add(new SqlParameter("@StokTakibi", cbStokTakibi.Checked));
            list.Add(new SqlParameter("@MusteriZorunluUyari", cbMusteriZorunluUyari.Checked));
            list.Add(new SqlParameter("@TedarikciZorunluUyari", cbTedarikciZorunluUyari.Checked));
            list.Add(new SqlParameter("@Callerid", cbCallerid.Checked));
            
            list.Add(new SqlParameter("@TeraziBarkoduBasi", seTeraziBarkoduBasi.EditValue.ToString().Replace(",",".")));
            list.Add(new SqlParameter("@TeraziBarkoduBasi2", seTeraziBarkoduBasi2.EditValue.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@TeraziBarkoduBasi3", seTeraziBarkoduBasi3.EditValue.ToString().Replace(",", ".")));

            list.Add(new SqlParameter("@MusteriBasNo", MusteriOzelKod.EditValue.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@BonusYuzde", bonusyuzde.Value.ToString()));
            list.Add(new SqlParameter("@VergiDairesi",teVergiDairesi.Text));
            Degerler.SirketVDaire = teVergiDairesi.Text;
            list.Add(new SqlParameter("@VergiNo", teVergiNo.Text));
            Degerler.SirketVDaire = teVergiNo.Text;
            list.Add(new SqlParameter("@WebSitesi", WebSitesi.Text));
            list.Add(new SqlParameter("@BonusTur", BonusTur.SelectedIndex));
            list.Add(new SqlParameter("@Host", teHost.Text));
            list.Add(new SqlParameter("@GonderenEposta", teGonderenEposta.Text));

            //string epostasifresisifrele = islemler.CryptoStreamSifreleme.Encrypt("Hitit999", teGonderenEpostaSifre.Text);
            string epostasifresisifrele = islemler.CryptoStreamSifreleme.md5Sifrele(teGonderenEpostaSifre.Text);
            list.Add(new SqlParameter("@GonderenEpostaSifre", epostasifresisifrele));
            list.Add(new SqlParameter("@Port", tePort.Text));
            list.Add(new SqlParameter("@Adres", teSirketAdresi.Text));
            list.Add(new SqlParameter("@YedekSaati", dateEdit2.DateTime.ToString("HH:mm:ss")));
            list.Add(new SqlParameter("@TelefonNo", teTel.Text));
            list.Add(new SqlParameter("@Fax", teFax.Text));
            list.Add(new SqlParameter("@Cep", teCep.Text));

            list.Add(new SqlParameter("@eposta", teEposta.Text));
            list.Add(new SqlParameter("@ccEposta", teEpostacc.Text));
            list.Add(new SqlParameter("@bccEposta", teEpostabcc.Text));

            Degerler.eposta= teEposta.Text;
            Degerler.epostacc= teEpostacc.Text;
            Degerler.epostabcc= teEpostabcc.Text;
            
            //sms
            list.Add(new SqlParameter("@sms_kullaniciadi", tbKullaniciAdi.Text));
            string sifrele = DB.EncodeTo64(tbSmsSifre.Text);
            list.Add(new SqlParameter("@sms_sifre", sifrele));
            list.Add(new SqlParameter("@sms_baslik", tbSmsBaslik.Text));

            list.Add(new SqlParameter("@kdv_orani", cbKdvorani.Text));
            list.Add(new SqlParameter("@kdv_orani_alis", cbAlisKdv.Text));
            
            list.Add(new SqlParameter("@stok_birimi", cbBirimi.SelectedIndex));
            list.Add(new SqlParameter("@OzelSifre", txtOzelSifre.Text));
            list.Add(new SqlParameter("@satisadedi_icindekiadetdir", cbeSatisadedi_icindekiadet.Checked));

            list.Add(new SqlParameter("@ikincifiyatgoster", cbikincifiyat.Checked));
            list.Add(new SqlParameter("@ucuncufiyatgoster", cbucuncufiyat.Checked));
            list.Add(new SqlParameter("@uruneklendisescal", cbUruneklendisescal.Checked));
            list.Add(new SqlParameter("@satiskopyala", cbSatiskopyala.Checked));

            list.Add(new SqlParameter("@odemesekli", comboBoxEdit4.Text));
            list.Add(new SqlParameter("@EnableSsl", cbEnableSsl.Checked));
            list.Add(new SqlParameter("@OncekiFiyatHatirla", cbOncekiFiyatHatirla.Checked));
            list.Add(new SqlParameter("@uretici__barkod_no", tbUreticiBarkodNo.Text));
            list.Add(new SqlParameter("@tedarikciler_giderdir", cbTadarikcilerGiderdir.Checked));
            list.Add(new SqlParameter("@is_kamp_odeme_sekli_acikhesap", cbKampanyaAcikHesap.Checked));

            list.Add(new SqlParameter("@fkKodu", LUESehir.EditValue));
            list.Add(new SqlParameter("@fkAltGrup", lookUpilce.EditValue));
            list.Add(new SqlParameter("@fkKoduDefault", lueil.EditValue));
            list.Add(new SqlParameter("@fkAltGrupDefault", lueilce.EditValue));
            list.Add(new SqlParameter("@DepoKullaniyorum", cbDepoKullaniyorum.Checked));
            list.Add(new SqlParameter("@stok_karti_dizayn", cbStokKartiDizayn.Checked));

            list.Add(new SqlParameter("@BitisTarihi", deBitisTarihi.DateTime.ToString("yyyy-MM-dd")));

            list.Add(new SqlParameter("@Yetkili", txtYetkili.Text));

            list.Add(new SqlParameter("@fkBirimler", lueBirimler.EditValue));
            list.Add(new SqlParameter("@makbuz_yazdir", ceMakbuzyazdir.Checked));
            list.Add(new SqlParameter("@oto_guncelle", cbOtoGuncelle.Checked));

            list.Add(new SqlParameter("@e_fatura_kul", txtEFaturaKul.Text));
            list.Add(new SqlParameter("@e_fatura_sifre", txtEFaturaSifre.Text));
            list.Add(new SqlParameter("@tedarikci_OncekiFiyatHatirla", cbOncekiFiyatHatirla_teda.Checked));
            list.Add(new SqlParameter("@e_fatura_url", txtEFaturaWebservis.Text));
            
            Degerler.OzelSifre = txtOzelSifre.Text;

            string sql = @"UPDATE Sirketler set Sirket=@Sirket,Yetkili=@Yetkili,yedekalinacakyer=@yedekalinacakyer,WebKullanir=@WebKullanir,
                        FaturaTipi=@FaturaTipi,FaturaTipiAlis=@FaturaTipiAlis,Sektor=@Sektor,StokTakibi=@StokTakibi,MusteriZorunluUyari=@MusteriZorunluUyari,
                        TedarikciZorunluUyari=@TedarikciZorunluUyari,VergiNo=@VergiNo,VergiDairesi=@VergiDairesi,
                        TeraziBarkoduBasi=@TeraziBarkoduBasi,MusteriBasNo=@MusteriBasNo,BonusYuzde=@BonusYuzde,WebSitesi=@WebSitesi,
                        Callerid=@Callerid,BonusTur=@BonusTur,kdv_orani=@kdv_orani,stok_birimi=@stok_birimi,
                        Host=@Host,GonderenEposta=@GonderenEposta,GonderenEpostaSifre=@GonderenEpostaSifre,Port=@Port,Adres=@Adres,
                        YedekSaati=@YedekSaati,TelefonNo=@TelefonNo,Fax=@Fax,Cep=@Cep,eposta=@eposta,
                        sms_kullaniciadi=@sms_kullaniciadi,sms_sifre=@sms_sifre,sms_baslik=@sms_baslik,OzelSifre=@OzelSifre,
                        satisadedi_icindekiadetdir=@satisadedi_icindekiadetdir,TeraziBarkoduBasi2=@TeraziBarkoduBasi2,
                        ikincifiyatgoster=@ikincifiyatgoster,ucuncufiyatgoster=@ucuncufiyatgoster,TeraziBarkoduBasi3=@TeraziBarkoduBasi3,
                        satiskopyala=@satiskopyala,odemesekli=@odemesekli,EnableSsl=@EnableSsl,OncekiFiyatHatirla=@OncekiFiyatHatirla,
                        bccEposta=@bccEposta,ccEposta=@ccEposta,uretici__barkod_no=@uretici__barkod_no,uruneklendisescal=@uruneklendisescal,
                        tedarikciler_giderdir=@tedarikciler_giderdir,is_kamp_odeme_sekli_acikhesap=@is_kamp_odeme_sekli_acikhesap,
                        fkKodu=@fkKodu,fkAltGrup=@fkAltGrup,fkKoduDefault=@fkKoduDefault,fkAltGrupDefault=@fkAltGrupDefault,
                        DepoKullaniyorum=@DepoKullaniyorum,stok_karti_dizayn=@stok_karti_dizayn,BitisTarihi=@BitisTarihi,fkBirimler=@fkBirimler,
                        makbuz_yazdir=@makbuz_yazdir,oto_guncelle=@oto_guncelle,kdv_orani_alis=@kdv_orani_alis, 
                        e_fatura_kul=@e_fatura_kul,e_fatura_sifre=@e_fatura_sifre,tedarikci_OncekiFiyatHatirla=@tedarikci_OncekiFiyatHatirla,
                        e_fatura_url=@e_fatura_url where pkSirket=" + pkSirket;
            string s = DB.ExecuteSQL(sql,list);
            if (s == "0")
            {
                sirketgetir();
                //gridControl1.DataSource = DB.GetData("select * from Sirketler");
                frmMesajBox mesaj = new frmMesajBox(100);
                mesaj.label1.Text = "Bilgiler Kaydedildi.";
                mesaj.label1.BackColor = System.Drawing.Color.YellowGreen;
                mesaj.Show();
                Degerler.YedekSaati = dateEdit2.DateTime.ToString("HH:mm:ss");
                Degerler.YedekYolu=klasoryol.Text;
                //sms ayarları
                Degerler.smskullaniciadi = tbKullaniciAdi.Text;
                //string coz = DB.DecodeFrom64(Degerler.smssifre);
                Degerler.smssifre = DB.EncodeTo64(tbSmsSifre.Text);
                Degerler.smsbaslik = tbSmsBaslik.Text;

                Degerler.kdvorani = int.Parse(cbKdvorani.Text);
                Degerler.kdvorani_alis = int.Parse(cbAlisKdv.Text);
                
                Degerler.stokbirimi = cbBirimi.Text;
                Degerler.odemesekli = comboBoxEdit4.Text;
                Degerler.Uruneklendisescal = cbUruneklendisescal.Checked;
                Degerler.Satiskopyala = cbSatiskopyala.Checked;
                Degerler.EnableSsl = cbEnableSsl.Checked;
                Degerler.OncekiFiyatHatirla = cbOncekiFiyatHatirla.Checked;

                Degerler.fkilKoduDefault = int.Parse(lueil.EditValue.ToString());
                Degerler.fkilceAltGrupDefault = int.Parse(lueilce.EditValue.ToString());
                Degerler.DepoKullaniyorum = cbDepoKullaniyorum.Checked;
                Degerler.StokKartiDizayn = cbStokKartiDizayn.Checked;

                Degerler.fkBirimler = int.Parse(lueBirimler.EditValue.ToString());
                Degerler.makbuzyazdir = ceMakbuzyazdir.Checked;
                Degerler.MusteriZorunluUyari = cbMusteriZorunluUyari.Checked;
            }
            else
                MessageBox.Show("Hata Oluştu.");

            Modulyetkilerikaydet();

            DB.TeraziBarkoduBasi1= int.Parse(seTeraziBarkoduBasi.Value.ToString());
            DB.TeraziBarkoduBasi2= int.Parse(seTeraziBarkoduBasi2.Value.ToString());
            DB.TeraziBarkoduBasi3= int.Parse(seTeraziBarkoduBasi3.Value.ToString());

            Thread thread1 = new Thread(new ThreadStart(ServerKaydet));
            thread1.Start();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() ==DialogResult.OK)
            klasoryol.Text = folderBrowserDialog1.SelectedPath;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            sirketgetir();
        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            //XtraTabControl tabControl = sender as XtraTabControl;
            //ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
            //(arg.Page as XtraTabPage).PageVisible = false;
        }

        void yetkigetir()
        {
            gcModuller.DataSource = DB.GetData(@"SELECT  pkModuller, ModulAdi, durumu, Kod,goster FROM Moduller with(nolock)");
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (e.Page == xtisyeri)
            {
                vProiller();
            }
            else if (e.Page == xtModuller)
            {
                inputForm sifregir = new inputForm();
                sifregir.Girilen.PasswordChar = '*';
                sifregir.ShowDialog();
                if (sifregir.Girilen.Text == "9999")
                {
                    simpleButton4.Enabled = true;
                    simpleButton6.Enabled = true;
                    yetkigetir();
                }
                sifregir.Dispose();
            }
            else if (e.Page == xTabDiger)
            {
                DataTable dt = DB.GetData("select * from  ayarlar with(nolock) where Ayar20='excelyol'");
                if (dt.Rows.Count > 0)
                    teExcelYol.Text = dt.Rows[0]["Ayar50"].ToString();
            }
        }

        void Modulyetkilerikaydet()
        {
            if (gvModuller.DataRowCount == 0) return;
            for (int i = 0; i < gvModuller.DataRowCount; i++)
            {
                DataRow dr = gvModuller.GetDataRow(i);
                string durumu = dr["durumu"].ToString();
                string aktif = "0";
                if (durumu == "True") aktif = "1";
                DB.ExecuteSQL("UPDATE Moduller SET durumu=" + aktif + " where pkModuller=" + dr["pkModuller"].ToString());
            }
            yetkigetir();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 1) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSirket = dr["pkSirket"].ToString();
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Sirket"].ToString() + " Şirketi Silinecek Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("DELETE FROM Sirketler WHERE pkSirket=" + pkSirket);
            gridView1.DeleteSelectedRows();
            Sirketler();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sonuc= DB.epostagonder(teGonderenEposta.Text, "Test", "", "Test Gönderim...");

            frmMesajBox mesaj = new frmMesajBox(200);
            if (sonuc=="OK")
                mesaj.label1.Text = "E-Posta Gönderildi.";
            else
                mesaj.label1.Text = sonuc;
            mesaj.Show();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE Moduller SET durumu=0,goster=0");
            DB.ExecuteSQL("UPDATE Moduller SET durumu=1,goster=1 where Kod in('1','2','3')");
            yetkigetir();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE Moduller SET durumu=1,goster=1");
            yetkigetir();
        }

        private void btnGetir_Click(object sender, EventArgs e)
        {
            HDDFizikselSeriNo();

            //string sql = "Select * from Kullanicilar with(nolock) where Eposta='" + teEposta.Text + "'";
            //string cs = "Data Source=sql2012.isimtescil.net;Initial Catalog=hitityazilim_db9999;Persist Security Info=True;User ID=hitityazilim_adem;Password=Hitit9999";

            //SqlConnection con = new SqlConnection(cs);
            //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            //adp.SelectCommand.CommandTimeout = 60;

            DataTable dt = new DataTable();
           
           // MessageBox.Show("Veritabanı Hatası Oluştu " + sexp.Message.ToString());
          
            if(dt.Rows.Count==0)
            {
                teEposta.Tag = "0";
                MessageBox.Show("Lütfen Hitit Yazılımı Arayınız! \n Tel : 0262 644 51 12 \n Cep : 0531 464 80 46");
            }
            else
            {
                teEposta.Tag = "1";
                string adi = dt.Rows[0]["KullaniciAdi"].ToString();
                teEposta.ToolTip = "Kullanıcı Adı "  + adi;
                MessageBox.Show("Kullanıcı Adı " + adi + " Olarak Görünmekteri.\n" +
                    " Tel : 0262 644 51 12   Cep : 0531 464 80 46");
            }
        }

        void HDDFizikselSeriNo()
        {
            teHDD.Text = "HDDFizikselSeriNo Bulunacak";//disk["SerialNumber"].ToString();
            //List<string> serials = new List<string>();

            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            //ManagementObjectCollection disks = searcher.Get();
            //foreach (ManagementObject disk in disks)
            //{
            //    if (disk["SerialNumber"] == null)
            //        serials.Add("");
            //    else
            //    {
            //        teHDD.Text = disk["SerialNumber"].ToString();
            //        return;
            //    }
            //}
        }

        private void btnYeniKul_Click(object sender, EventArgs e)
        {
            string kul = "";
            wshitityazilim.WebService ws = new wshitityazilim.WebService();
            //ws.Url = "http://localhost:49732/WebService.asmx";

            if (ws != null)
                kul = ws.KulEkle("", tESirket.Text, teTel.Text, teEposta.Text);

            MessageBox.Show(kul);

            return;
            //if (teEposta.Tag == "1")
            //{
            //    MessageBox.Show("Zaten Kayıtlı.\n Lütfen Hitit Yazılımı Arayınız.\n"+
            //        " Tel : 0262 644 51 12   Cep : 0531 464 80 46");
            //    return;
            //}
            string sql = @"insert into Kullanicilar (KullaniciAdi,Tarih,bitistarihi,Eposta,Aciklama,HDDNo,durumu) 
            values('" + tESirket.Text+ "',getdate(),getdate()+365,'" + teEposta.Text + "','" + tESirket.Text + "','"+ teHDD.Text+"',1)";
            string cs = "Data Source=sql2012.isimtescil.net;Initial Catalog=hitityazilim_db9999;Persist Security Info=True;User ID=hitityazilim_adem;Password=Hitit9999";
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand(sql, con);

            int count = 0;
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException sexp)
            {
                count = -1;
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama) values(0,'" +
                //e.Message.ToString().Replace("'", "") + "',getdate(),0,"+sql+")");
                DB.logayaz("ExecuteSQL Hatası Oluştur: " + sexp.Message.ToString(), sql);
                //throw e;
            }
            catch (Exception exp)
            {
                //count = -1;
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                //e.Message.ToString().Replace("'","") + ",getdate(),0)");
                //throw e;
                DB.logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
        }

        private void labelControl34_Click(object sender, EventArgs e)
        {
            HDDFizikselSeriNo();
        }

        private void labelControl35_Click(object sender, EventArgs e)
        {
            inputForm sifregir = new inputForm();
            sifregir.Girilen.PasswordChar = '*';
            sifregir.ShowDialog();
            if (sifregir.Girilen.Text != "9999*")
                return;
            DataTable dt = null;
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                ws.Url = "http://hitityazilim.com/WebService.asmx";
                //DB.InternetVarmi22

                if (ws != null)
                dt = ws.KulList();

            //string sql = "select pkKullanicilar,KullaniciAdi from Kullanicilar with(nolock)";
            //string cs = "Data Source=sql2012.isimtescil.net;Initial Catalog=hitityazilim_db9999;Persist Security Info=True;User ID=hitityazilim_adem;Password=Hitit9999";

            //SqlConnection con = new SqlConnection(cs);
            //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            //adp.SelectCommand.CommandTimeout = 60;

            //DataTable dt = new DataTable();
           
             //   adp.Fill(dt);
            }
            //catch (Exception e)
            //{
            //    frmMesaj mesaj = new frmMesaj();
            //    mesaj.Text = "Hata Oluştu" + e.Message.ToString();
            //    mesaj.Show();
            //}
            catch (SqlException sexp)
            {
                MessageBox.Show("Veritabanı Hatası Oluştu " + sexp.Message.ToString());
            }
            //finally
            //{
            //    con.Dispose();
            //    adp.Dispose();
            //}
            if(dt.Rows.Count==0)
            {
                teEposta.Tag = "0";
                MessageBox.Show("Lütfen Hitit Yazılımı Arayınız! \n Tel : 0262 644 51 12 \n Cep : 0531 464 80 46");
            }
            else
            {
                teEposta.Tag = "1";
                lueGruplar.Properties.DataSource = dt;
                //string adi = dt.Rows[0]["KullaniciAdi"].ToString();
                //teEposta.ToolTip = "Kullanıcı Adı "  + adi;
                //MessageBox.Show("Kullanıcı Adı " + adi + " Olarak Görünmekteri.\n" +
                  //  " Tel : 0262 644 51 12   Cep : 0531 464 80 46");
            }
            
        }

        private void btnKulKaydet_Click(object sender, EventArgs e)
        {
            if (lueGruplar.EditValue == null)
            {
                formislemleri.Mesajform("Referans Seçiniz veya İnternetinizi Kontrol Ediniz", "K", 200);
                return;
            }
            string sql = "update Kullanicilar set GuncellemeTarihi=getdate(),"+
                "Aciklama ='" + tESirket.Text + "',"+
                "HDDNo='" + teHDD.Text + "'," +
                "Eposta='" + teEposta.Text + "'," +
                "il='" + LUESehir.Text+ "'," +
                "ilce='" + lookUpilce.Text + "'," +
                "Grubu='" + cbSektor.Text + "'," +
                "WebAdresi='" + WebSitesi.Text + "'," +
                "Telefon='" + teTel.Text + "'," +
                "CepTel='" + teCep.Text + "'" +
                " where pkKullanicilar=" + lueGruplar.EditValue.ToString() ;

            string cs = "Data Source=sql2012.isimtescil.net;Initial Catalog=hitityazilim_db9999;Persist Security Info=True;User ID=hitityazilim_adem;Password=Hitit9999";
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand(sql, con);

            int count = 0;
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException sexp)
            {
                count = -1;
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama) values(0,'" +
                //e.Message.ToString().Replace("'", "") + "',getdate(),0,"+sql+")");
                DB.logayaz("ExecuteSQL Hatası Oluştur: " + sexp.Message.ToString(), sql);
                //throw e;
            }
            catch (Exception exp)
            {
                //count = -1;
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                //e.Message.ToString().Replace("'","") + ",getdate(),0)");
                //throw e;
                DB.logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
        }

        void vProiller()
        {
            LUESehir.Properties.DataSource = DBislemleri.ilgetir();
            LUESehir.Properties.ValueMember = "KODU";
            LUESehir.Properties.DisplayMember = "ADI";

        }

        void vProillerVarsayilan()
        {
            lueil.Properties.DataSource = DBislemleri.ilgetir();
            lueil.Properties.ValueMember = "KODU";
            lueil.Properties.DisplayMember = "ADI";
        }

        void vProilceler(string ilid)
        {
            lookUpilce.Properties.DataSource = DBislemleri.ilcelerigetir(ilid);
            lookUpilce.Properties.ValueMember = "KODU";
            lookUpilce.Properties.DisplayMember = "ADI";
        }

        void ProilcelerVarsayilan(string ilid)
        {
            lueilce.Properties.DataSource = DBislemleri.ilcelerigetir(ilid);
            lueilce.Properties.ValueMember = "KODU";
            lueilce.Properties.DisplayMember = "ADI";
        }
        private void LUESehir_EditValueChanged(object sender, EventArgs e)
        {
            vProilceler(LUESehir.EditValue.ToString());
        }

        private void cbeSatisadedi_icindekiadet_Click(object sender, EventArgs e)
        {
            string mesaj = "";
            if (cbeSatisadedi_icindekiadet.Checked == true)
            {
                mesaj = "Satış Miktarı 1 olarak Değiştirilecektir.";
            }
            else
            {
                mesaj = "Satış Miktarı, Paket içindeki Miktarı Olarak değiştirilecektir.";
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(mesaj + "\n Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            if (cbeSatisadedi_icindekiadet.Checked == true)
            {
                DB.ExecuteSQL("update StokKarti set SatisAdedi=1");
                cbeSatisadedi_icindekiadet.Checked = false;
            }
            else
            {
                DB.ExecuteSQL("update StokKarti set SatisAdedi=KutuFiyat");
                cbeSatisadedi_icindekiadet.Checked = true;
            }

        }

        private void labelControl38_Click(object sender, EventArgs e)
        {

        }

        private void lueil_EditValueChanged(object sender, EventArgs e)
        {
            ProilcelerVarsayilan(lueil.EditValue.ToString());
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            int i= DB.ExecuteSQL_Sonuc_Sifir("update ayarlar set Ayar50='"+teExcelYol.Text+"' where Ayar20='excelyol'");
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                teExcelYol.Text = folderBrowserDialog1.SelectedPath;

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {
            inputForm sifregir = new inputForm();
            sifregir.Girilen.PasswordChar = '#';
            sifregir.ShowDialog();

            if (sifregir.Girilen.Text =="9999*")
               deBitisTarihi.Enabled = true;

            sifregir.Dispose();
        }

        private void luzaksqlbaglimi_Click(object sender, EventArgs e)
        {
                SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=185.106.211.51\\MSSQLSERVER2016;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityaz_hitityazilim;Password=Hitit9999");
                string sql = "select KullaniciAdi,versiyon,Kiralik,WebAdresi,convert(nvarchar,BitisTarihi,112) as BitisTarihi from Kullanicilar with(nolock) where Eposta='" +
                teEposta.Text + "'";

                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                bool baglanti_basarili = false;
                DataTable dt = new DataTable();
                try
                {
                    adp.Fill(dt);
                    baglanti_basarili = true;
                    DB.uzaksqlbaglandimi = true;
                    //MessageBox.Show("Bağlandı");
                }
                catch (Exception exp)
                {
                    DB.logayaz("Bağlantı Hatası: Hitit Yazılımı Arayınız hata: ", exp.Message);
                    baglanti_basarili = false;
                    DB.uzaksqlbaglandimi = false;
                    MessageBox.Show("Hata Oluştur " + exp.Message.ToString());
                }
                finally
                {
                    con.Dispose();
                    adp.Dispose();
                }

                if (dt.Rows.Count == 0 && baglanti_basarili)
                {
                    XtraMessageBox.Show(teEposta.Text +"\r Lisans Bilgileriniz Hatalı. \r Lütfen Hitit Yazılımı arayınız.", "Lisanas Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    DB.kayitli = 0;
                }
                if (dt.Rows.Count > 0)
                {
                    string KullaniciAdi = dt.Rows[0]["KullaniciAdi"].ToString();
                    string vol = dt.Rows[0]["versiyon"].ToString();
                    //DB.kayitli = int.Parse(dt.Rows[0]["Aktif"].ToString());
                    string BitisTarihi = dt.Rows[0]["BitisTarihi"].ToString();
                    //string Kiralik = dt.Rows[0]["Kiralik"].ToString();
                    //string WebAdresi = dt.Rows[0]["WebAdresi"].ToString();

                MessageBox.Show("Hitit Yazılımda Kayıtlı Bitiş Tarihi " + BitisTarihi.ToString());
                    if (BitisTarihi != "")
                        DB.ExecuteSQL("update Sirketler set BitisTarihi='" + BitisTarihi + "'");

                MessageBox.Show("Bu Bilgisayarda Görünen Bitiş Tarihi " +
                DB.GetData("select BitisTarihi from Sirketler").Rows[0][0].ToString());
            }
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelControl6_Click(object sender, EventArgs e)
        {

        }

        private void btnManuel_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           string sonuc = DB.epostagonder(teEposta.Text, "Test-2", "", "Test-2 Gönderim...");

            if (sonuc == "OK")
            {
                frmMesajBox mesaj = new frmMesajBox(200);
                mesaj.label1.Text = "E-Posta Gönderildi.";
                mesaj.Show();
            }
            else
                MessageBox.Show(sonuc);
            
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://portal-test.uyumsoft.com.tr/");
        }

        private void labelControl52_Click(object sender, EventArgs e)
        {
            txtEFaturaWebservis.Text = "https://efatura.uyumsoft.com.tr/services/Integration";
        }
    }
}
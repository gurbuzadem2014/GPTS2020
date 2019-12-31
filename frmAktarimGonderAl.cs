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
    public partial class frmAktarimGonderAl : DevExpress.XtraEditors.XtraForm
    {
        public frmAktarimGonderAl()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmWebAyarlari_Load(object sender, EventArgs e)
        {
            AyarlariGetir();
            xtraTabC.SelectedTabPage = xTabPAyarlar;
            //SatislarUzak();
            //KasaHareketUzak();

            if (Degerler.AracdaSatis == "0")
                simpleButton12_Click(sender, e);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (DB.VeriTabaniAdresi == txtUzakSunucu.Text && DB.VeriTabaniAdi == txtVeritabani.Text)
            {
                formislemleri.Mesajform("VeriTabanı Sunucu ve Adı Aynı Olamaz", "K", 200);
                return;
            }
            
            string sql = "";
            DataTable dt = DB.GetData("select * from Ayarlar with(nolock)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pkAyarlar = dt.Rows[i]["pkAyarlar"].ToString();
                string ayar = dt.Rows[i]["Ayar20"].ToString();
                sql = "";
                if (ayar == "UzakSunucu")
                {
                    sql += "update Ayarlar set  Ayar50='" + txtUzakSunucu.Text + "' WHERE pkAyarlar=" + pkAyarlar;
                    DBWeb.UzakSunucu = txtUzakSunucu.Text;
                }
                else if (ayar == "Veritabani")
                {
                    sql += "update Ayarlar set  Ayar50='" + txtVeritabani.Text + "' WHERE pkAyarlar=" + pkAyarlar;
                    DBWeb.Veritabani = txtVeritabani.Text;
                }
                else if (ayar == "SqlUser")
                {
                    sql += "update Ayarlar set  Ayar50='" + txtSqlUser.Text + "' WHERE pkAyarlar=" + pkAyarlar;
                    DBWeb.SqlUser = txtSqlUser.Text;
                }
                else if (ayar == "Password")
                {
                    sql += "update Ayarlar set  Ayar50='" + txtPassword.Text + "' WHERE pkAyarlar=" + pkAyarlar;
                    DBWeb.Password = txtPassword.Text;
                }
                DB.ExecuteSQL(sql);

                formislemleri.Mesajform("Bilgiler Kaydedildi", "S", 200);

                simpleButton2.Enabled = true;
            }
            /*
            //KullaniciKontrol("gizembebe");
            if (DB.webpkKullanicilar == 0) return;
            SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=mssql02.turhost.com;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityazilim;Password=hitit9999");
            //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            SqlCommand cmd = new SqlCommand(sql, con);
            //adp.SelectCommand.CommandTimeout = 60;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
               // adp.Fill(dt);
                //gridControl1.DataSource = dt;
            }
            catch (Exception exp)
            {
                // throw e;
            }
            finally
            {
                con.Dispose();
                //adp.Dispose();
            }
             * */
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
        void SatislarBuBilgisayardaki()
        {
            gridControl1.DataSource = DB.GetData(@"select  s.*,f.Firmaadi,f.OzelKod from Satislar s with(nolock)
            left join Firmalar f with(nolock) on f.pkFirma=s.fkFirma where isnull(s.Aktarildi,0)=0" );
        }

        void KasaHareketBuBilgisayardaki()
        {
             gCPerHareketleri.DataSource = DB.GetData(@"select kh.*,f.Firmaadi,f.OzelKod from KasaHareket kh with(nolock)
             left join Firmalar f with(nolock) on f.pkFirma=kh.fkFirma
             where fkSatislar is null and isnull(kh.Aktarildi,0)=0");
            //OdemeSekli not in('Kasa Bakiye Düzeltme','Bakiye Düzeltme') and
        }

        void StoklarUzak()
        {
             gridControl2.DataSource = DBWeb.GetData_Web(@"select * from StokKarti with(nolock) where isnull(Aktarildi,0)=0");
        }
        void MusterilerUzak()
        {
             gridControl3.DataSource = DBWeb.GetData_Web(@"select * from Firmalar with(nolock) where isnull(Aktarildi,0)=0");
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xTabPAyarlar)
            {
                AyarlariGetir();
            }
            else if (e.Page == xTabPGonder)
            {
                SatislarBuBilgisayardaki();
                KasaHareketBuBilgisayardaki();
            }
            else if (e.Page == xTabAl)
            {
                StoklarUzak();
                MusterilerUzak();
            }
            
        }
        void AyarlariGetir()
        {
            DataTable dt = DB.GetData("select * from Ayarlar with(nolock)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ayar20 = dt.Rows[i]["Ayar20"].ToString();
                string ayar50 = dt.Rows[i]["Ayar50"].ToString();

                if (ayar20 == "UzakSunucu")
                    txtUzakSunucu.Text = ayar50;
                else if (ayar20 == "Veritabani")
                    txtVeritabani.Text = ayar50;
                else if (ayar20 == "SqlUser")
                    txtSqlUser.Text = ayar50;
                else if (ayar20 == "Password")
                    txtPassword.Text = ayar50;
            }
            DBWeb.UzakSunucu = txtUzakSunucu.Text;
            DBWeb.Veritabani = txtVeritabani.Text;
            DBWeb.SqlUser =  txtSqlUser.Text;
            DBWeb.Password =   txtPassword.Text;
        }

        private void sbtnSatislar_Click(object sender, EventArgs e)
        {
            if (gridView5.DataRowCount == 0) return;

            sbtnSatislar.Enabled = false;

            DataTable dt = DB.GetData(@"select * from Satislar s with(nolock)
                left join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
                where isnull(s.Aktarildi,0)=0");
            
            int hatali = 0, hatasiz = 0, c = dt.Rows.Count;

            if (c == 0) return;

            listBoxControl1.Items.Add(c.ToString()  + " adet Satış aktarılıyor...");
            //uzak trans
            if (DBWeb.conTrans == null)
                DBWeb.conTrans = new SqlConnection(DBWeb.ConnectionString());
            if (DBWeb.conTrans.State == ConnectionState.Closed)
                DBWeb.conTrans.Open();
            //local trans
            if (DB.conTrans == null)
                DB.conTrans = new SqlConnection(DB.ConnectionString());
            if (DB.conTrans.State == ConnectionState.Closed)
                DB.conTrans.Open();

            foreach (DataRow dr in dt.Rows)
            {
                Application.DoEvents();

                #region transları başlat
                DBWeb.transaction = DBWeb.conTrans.BeginTransaction("AdemTransaction");
                DB.transaction = DB.conTrans.BeginTransaction("AktarimTransaction");
                #endregion

                #region Satış Değerler
                string pkSatislar = dr["pkSatislar"].ToString();
                string Tarih = dr["Tarih"].ToString();
                string fkFirma = dr["fkFirma"].ToString();
                string Firma_id = dr["Firma_id"].ToString();//ana bilgisayardaki pkFirma=Firma_id
                string Siparis = dr["Siparis"].ToString();
                string fkKullanici = dr["fkKullanici"].ToString();
                string fkSatisDurumu = dr["fkSatisDurumu"].ToString();
                string Aciklama = dr["Aciklama"].ToString();
                string AlinanPara = dr["AlinanPara"].ToString();
                string ToplamTutar = dr["ToplamTutar"].ToString();
                string Odenen = dr["Odenen"].ToString();
                string OdenenKrediKarti = dr["OdenenKrediKarti"].ToString();
                string CekTutar = dr["CekTutar"].ToString();
                if (CekTutar == "") CekTutar = "0";
                
                string AcikHesap = dr["AcikHesap"].ToString();
                if (AcikHesap == "") AcikHesap = "0";

                string OdemeSekli = dr["OdemeSekli"].ToString();
                string GuncellemeTarihi = dr["GuncellemeTarihi"].ToString();

                string fkSatisFiyatlariBaslik = dr["fkSatisFiyatlariBaslik"].ToString();
                
                string BonusOdenen = dr["BonusOdenen"].ToString();
                if (BonusOdenen == "") BonusOdenen = "0";

                string OncekiBakiye = dr["OncekiBakiye"].ToString();
                if (OncekiBakiye == "") OncekiBakiye = "0";

                string NakitOdenen = dr["NakitOdenen"].ToString();
                if (NakitOdenen == "") NakitOdenen = "0";

                string sql = "";
                #endregion

                #region Satış Kaydet

                 //DataTable dtS = DBWeb.GetData_Web("select pkSatislar from Satislar with(nolock) where EskiFis=" + pkSatislar);
                //if (dtS.Rows.Count > 0)
                //{
                //    listBoxControl1.Items.Add("Satis Daha Önce Kaydedildi"+  pkSatislar + " Hata:");
                //    continue;
                //}

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@EskiFis", pkSatislar));
                list.Add(new SqlParameter("@Tarih", Convert.ToDateTime(Tarih)));
                list.Add(new SqlParameter("@fkFirma", Firma_id));//fkFirma)); ana bilgisayardaki pkFirma=Firma_id
                list.Add(new SqlParameter("@Siparis", Siparis));
                list.Add(new SqlParameter("@fkKullanici", fkKullanici));
                list.Add(new SqlParameter("@fkSatisDurumu", fkSatisDurumu));
                if (Aciklama=="")
                    list.Add(new SqlParameter("@Aciklama", DBNull.Value));
                else
                    list.Add(new SqlParameter("@Aciklama", Aciklama));

                list.Add(new SqlParameter("@OdenenKrediKarti", OdenenKrediKarti.Replace(",", ".")));
                list.Add(new SqlParameter("@CekTutar", CekTutar.Replace(",", ".")));
                
                if (AlinanPara == "") AlinanPara = "0";
                list.Add(new SqlParameter("@AlinanPara", AlinanPara.Replace(",",".")));

                if (ToplamTutar == "") ToplamTutar = "0";
                list.Add(new SqlParameter("@ToplamTutar", ToplamTutar.Replace(",", ".")));

                if (Odenen == "")  Odenen = "0";
                list.Add(new SqlParameter("@Odenen", Odenen.Replace(",", ".")));

                if (AcikHesap == "") AcikHesap = "0";
                list.Add(new SqlParameter("@AcikHesap", AcikHesap.Replace(",", ".")));

                list.Add(new SqlParameter("@OdemeSekli", OdemeSekli));

                if (GuncellemeTarihi == "") 
                    GuncellemeTarihi = Tarih;

                list.Add(new SqlParameter("@GuncellemeTarihi", Convert.ToDateTime(GuncellemeTarihi)));

                list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", fkSatisFiyatlariBaslik));
                if (BonusOdenen == "") BonusOdenen = "0";
                list.Add(new SqlParameter("@BonusOdenen", BonusOdenen.Replace(",", ".")));
                if (OncekiBakiye == "") OncekiBakiye = "0";
                list.Add(new SqlParameter("@OncekiBakiye", OncekiBakiye.Replace(",", ".")));
                if (NakitOdenen == "") NakitOdenen = "0";
                list.Add(new SqlParameter("@NakitOdenen", NakitOdenen.Replace(",", ".")));

                sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,fkKullanici,fkSatisDurumu,Aciklama,OdemeSekli,fkSatisFiyatlariBaslik,ToplamTutar,Odenen,AcikHesap,EskiFis,AlinanPara,BilgisayarAdi,GuncellemeTarihi,OdenenKrediKarti,CekTutar,BonusOdenen,OncekiBakiye,NakitOdenen,aktarildi)" +
               " values(@Tarih,@fkFirma,@Siparis,@fkKullanici,@fkSatisDurumu,@Aciklama,@OdemeSekli,@fkSatisFiyatlariBaslik,@ToplamTutar,@Odenen,@AcikHesap,@EskiFis,@AlinanPara,'Aktarım',@GuncellemeTarihi,@OdenenKrediKarti,@CekTutar,@BonusOdenen,@OncekiBakiye,@NakitOdenen,1) select IDENT_CURRENT('Satislar')";

                bool islembasarili=true;
                string yeni_pkSatislar = DBWeb.ExecuteScalarSQLTrans(sql, list);

                if (yeni_pkSatislar.Substring(0, 1) == "H")
                {
                    islembasarili = false;
                    listBoxControl1.Items.Add("Satislar Hata Fiş No: " + pkSatislar + " Hata:" + yeni_pkSatislar);
                }
                #endregion

                #region islembasarili ise Satış Detayları aktar
                if (islembasarili)
                {
                    DataTable dt2 = DB.GetData(@"select * from SatisDetay sd with(nolock)
                        left join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                    where fkSatislar=" + pkSatislar);
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        string fkStokKarti = dr2["fkStokKarti"].ToString();
                        string StokKarti_id = dr2["StokKarti_id"].ToString();//ana pc deki stokkarti id =pkStokKarti
                        string Adet = dr2["Adet"].ToString();
                        string Tarih2 = dr2["Tarih"].ToString();

                        string AlisFiyati = dr2["AlisFiyati"].ToString();
                        if (AlisFiyati == "") AlisFiyati = "0";

                        string SatisFiyati = dr2["SatisFiyati"].ToString();
                        if(SatisFiyati=="") SatisFiyati="0";

                        string NakitFiyat = dr2["NakitFiyat"].ToString();
                        if (NakitFiyat == "") NakitFiyat = "0";

                        string iade = dr2["iade"].ToString();
                        string KdvOrani = dr2["KdvOrani"].ToString();
                        
                        string iskontoyuzdetutar = dr2["iskontoyuzdetutar"].ToString();
                        if (iskontoyuzdetutar == "") iskontoyuzdetutar = "0";
                        
                        string iskontotutar = dr2["iskontotutar"].ToString();
                        if(iskontotutar=="") iskontotutar="0";
                        
                        string fkDepolar=dr2["fkDepolar"].ToString(); 
                        if(fkDepolar=="") fkDepolar="1";

                        ArrayList list2 = new ArrayList();
                        list2.Add(new SqlParameter("@fkSatislar", yeni_pkSatislar));
                        list2.Add(new SqlParameter("@fkStokKarti", StokKarti_id));//fkStokKarti));//ana pc deki stokkarti id =pkStokKarti
                        list2.Add(new SqlParameter("@Tarih", Convert.ToDateTime(Tarih2)));
                        list2.Add(new SqlParameter("@Adet", Adet.Replace(",",".")));
                        list2.Add(new SqlParameter("@AlisFiyati", AlisFiyati.Replace(",", ".")));
                        list2.Add(new SqlParameter("@SatisFiyati", SatisFiyati.Replace(",", ".")));
                        list2.Add(new SqlParameter("@NakitFiyat", NakitFiyat.Replace(",", ".")));
                        list2.Add(new SqlParameter("@iade", iade));
                        list2.Add(new SqlParameter("@KdvOrani", KdvOrani.Replace(",", ".")));

                        if (iskontotutar == "") iskontotutar = "0";
                        list2.Add(new SqlParameter("@iskontotutar", iskontotutar.Replace(",", ".")));

                        if (iskontoyuzdetutar == "") iskontoyuzdetutar = "0";
                        list2.Add(new SqlParameter("@iskontoyuzdetutar", iskontoyuzdetutar.Replace(",", ".")));
                        list2.Add(new SqlParameter("@fkDepolar", fkDepolar));
                        
                        sql = "INSERT INTO SatisDetay (fkSatislar,fkStokKarti,Tarih,Adet,AlisFiyati,SatisFiyati,NakitFiyat,iade,KdvOrani,iskontotutar,iskontoyuzdetutar,Faturaiskonto,isKdvHaric,GercekAdet,fkDepolar) " +
                        " values(@fkSatislar,@fkStokKarti,@Tarih,@adet,@AlisFiyati,@SatisFiyati,@NakitFiyat,@iade,@KdvOrani,@iskontotutar,@iskontoyuzdetutar,0,0,1,@fkDepolar)";

                        string sonuc = DBWeb.ExecuteSQLTrans(sql, list2);

                        if (sonuc.Substring(0, 1) == "H")
                        {
                            islembasarili = false;
                            listBoxControl1.Items.Add("SatisDetay Hata Fiş No: " + pkSatislar + " Hata:" + sonuc);
                            break;//ikinci döngünün içinden çıkması için islembasarili sonraki döngü hatasız ise hatalı olan aktarılmamış olur.
                        }
                        else
                        {
                            #region stok karti mevcut güncelle
                            //StokMevcutGuncele  satışda mevcut azalır
                            string ssql="update StokKarti set";
                            decimal adet=1;
                            decimal.TryParse(Adet, out adet);
                            
                            if(adet>0)
                                ssql=ssql+" Mevcut = Mevcut - " + adet.ToString().Replace(",",".");
                            else
                                ssql = ssql + " Mevcut = Mevcut + " + adet.ToString().Replace(",", ".").Replace("-","");

                            ssql = ssql + " where pkStokKarti=" + StokKarti_id;
                            string sm = DBWeb.ExecuteSQLTrans(ssql);

                            if (sm.Substring(0, 1) == "H")
                                islembasarili = false;
                            #endregion
                            #region Depo Mevcut
                            
                            //kendi deposundan azalt 
                            //iade ise arttır değilse azalt
                            if (adet > 0)
                                DBWeb.ExecuteSQLTrans("update StokKartiDepo set MevcutAdet=MevcutAdet-" + adet.ToString() + " where fkDepolar=" + fkDepolar + " and fkStokKarti=" + fkStokKarti);
                            else
                                DBWeb.ExecuteSQLTrans("update StokKartiDepo set MevcutAdet=MevcutAdet+" + adet.ToString().Replace("-", "") + " where fkDepolar=" + fkDepolar + " and fkStokKarti=" + fkStokKarti);
                            
                            #endregion
                        }

                    }
                }
                #endregion

                #region KasaHareketleri Aktar Satış Ödemeleri
                if (islembasarili)
                {
                    DataTable dt3 = DB.GetData("select * from KasaHareket kh with(nolock) "+
                    " left join Firmalar f with(nolock) on f.pkFirma=kh.fkFirma where f.Firma_id is not null and fkSatislar=" + pkSatislar);
                    foreach (DataRow  dr3 in dt3.Rows)
                    {
                        //if (!islembasarili) continue;
                        string pkKasaHareket = dr3["pkKasaHareket"].ToString();
                        //DataTable dtK = DBWeb.GetData_Web("select pkKasaHareket from KasaHareket with(nolock) where aktarim_fis_no=" + pkKasaHareket);
                        //if (dtK.Rows.Count > 0)
                        //{
                        //    listBoxControl1.Items.Add("Kasa Hareket Daha Önce Kaydedildi" + pkKasaHareket + " Hata:");
                        //    islembasarili = false;
                        //    //continue;
                        //}
                        string fkKasalar = dr3["fkKasalar"].ToString();
                        string Tarih2 = dr3["Tarih"].ToString();
                        string Tutar = dr3["Tutar"].ToString();
                        string Borc = dr3["Borc"].ToString();
                        string Alacak = dr3["Alacak"].ToString();
                        string Aciklama3 = dr3["Aciklama"].ToString();
                        string fkFirma3 = dr3["fkFirma"].ToString();
                        string _Firma_id = dr3["Firma_id"].ToString();
                        string fkTuru = dr3["fkTuru"].ToString();
                        string OdemeSekli3 = dr3["OdemeSekli"].ToString();
                        string fkBankalar = dr3["fkBankalar"].ToString();
                        string AktifHesap = dr3["AktifHesap"].ToString();
                        string fkTedarikciler = dr3["fkTedarikciler"].ToString();
                        string fkAlislar = dr3["fkAlislar"].ToString();
                        string fkKasaGirisCikisTurleri = dr3["fkKasaGirisCikisTurleri"].ToString();
                        string Bakiye = dr3["Bakiye"].ToString();
                        string BilgisayarAdi = dr3["BilgisayarAdi"].ToString();
                        string fkCek = dr3["fkCek"].ToString();
                        string fkKullanicilar = dr3["fkKullanicilar"].ToString();

                        ArrayList list2 = new ArrayList();

                        list2.Add(new SqlParameter("@fkSatislar", yeni_pkSatislar));
                        list2.Add(new SqlParameter("@fkKasalar", fkKasalar));
                        list2.Add(new SqlParameter("@Tarih", Convert.ToDateTime(Tarih2)));
                        list2.Add(new SqlParameter("@Tutar", Tutar.Replace(",", ".")));
                        if (Aciklama3 == "") Aciklama3 = "Aktarım";
                        list2.Add(new SqlParameter("@Aciklama", Aciklama3));
                        list2.Add(new SqlParameter("@Borc", Borc.Replace(",", ".")));
                        list2.Add(new SqlParameter("@Alacak", Alacak.Replace(",", ".")));
                        list2.Add(new SqlParameter("@fkFirma", _Firma_id));  //fkFirma3));
                        if (fkTuru=="")
                            list2.Add(new SqlParameter("@fkTuru", DBNull.Value));
                        else
                            list2.Add(new SqlParameter("@fkTuru", fkTuru));
                        list2.Add(new SqlParameter("@OdemeSekli", OdemeSekli3));
                        if (fkBankalar=="")
                            list2.Add(new SqlParameter("@fkBankalar", DBNull.Value));
                        else
                            list2.Add(new SqlParameter("@fkBankalar", fkBankalar));
                        list2.Add(new SqlParameter("@AktifHesap", AktifHesap));
                        if (fkTedarikciler=="")
                            list2.Add(new SqlParameter("@fkTedarikciler", DBNull.Value));
                        else
                            list2.Add(new SqlParameter("@fkTedarikciler", fkTedarikciler));

                        if(fkAlislar=="")
                            list2.Add(new SqlParameter("@fkAlislar", DBNull.Value));
                        else
                            list2.Add(new SqlParameter("@fkAlislar", fkAlislar));

                        if (fkKasaGirisCikisTurleri=="")
                            list2.Add(new SqlParameter("@fkKasaGirisCikisTurleri", DBNull.Value));
                        else
                            list2.Add(new SqlParameter("@fkKasaGirisCikisTurleri", fkKasaGirisCikisTurleri));

                        if (Bakiye == "") Bakiye = "0";
                        list2.Add(new SqlParameter("@Bakiye", Bakiye.Replace(",",".")));

                        list2.Add(new SqlParameter("@BilgisayarAdi", "Aktarım"));//BilgisayarAdi));
                        if (fkCek=="")
                            list2.Add(new SqlParameter("@fkCek", DBNull.Value));
                        else
                            list2.Add(new SqlParameter("@fkCek", fkCek));
                        list2.Add(new SqlParameter("@fkKullanicilar", fkKullanicilar));

                        list2.Add(new SqlParameter("@aktarim_fis_no", pkKasaHareket));
                        
                        sql = "INSERT INTO KasaHareket (fkKasalar,Tarih,Tutar,Aciklama,Borc,Alacak,fkFirma,fkTuru,OdemeSekli,fkBankalar,AktifHesap,fkTedarikciler,fkSatislar,fkAlislar,fkKasaGirisCikisTurleri,Bakiye,BilgisayarAdi,fkCek,fkKullanicilar,aktarildi,aktarim_fis_no)" +
                            " values(@fkKasalar,@Tarih,@Tutar,@Aciklama,@Borc,@Alacak,@fkFirma,@fkTuru,@OdemeSekli,@fkBankalar,@AktifHesap,@fkTedarikciler,@fkSatislar,@fkAlislar,@fkKasaGirisCikisTurleri,@Bakiye,@BilgisayarAdi,@fkCek,@fkKullanicilar,1,@aktarim_fis_no)";

                        string sonuc = DBWeb.ExecuteSQLTrans(sql, list2);

                        if (sonuc.Substring(0, 1) == "H")
                        {
                            islembasarili = false;
                            listBoxControl1.Items.Add("Kasa Hareket Hata: " + fkKasalar + " Hata:" + sonuc);
                        }
                        //else
                        //{
                        //    islembasarili = true;
                        //}
                    }
                }
                #endregion

                #region sonuç başarılı ise aktarılanı sil
                if (islembasarili)
                {
                    #region trans başlat
                    //if (DB.conTrans == null)
                    //{
                    //    DB.conTrans = new SqlConnection(DB.ConnectionString());
                    //}

                    //if (DB.conTrans.State == ConnectionState.Closed)
                    //    DB.conTrans.Open();

                    //DB.transaction = DB.conTrans.BeginTransaction("AktarimTransaction");
                    #endregion
                    
                    //localden sil  
                    //bir kayıt silindi ise 2
                    //hiçbir kayıt silindi ise 0 
                    string s,s1,s2;
                    s = DB.ExecuteSQLTrans("delete from KasaHareket where fkSatislar=" + pkSatislar);
                    if (s != "0")
                    {
                        islembasarili = false;
                        //locak sunucuyu kapat
                        //DB.transaction.Rollback();
                        //DB.conTrans.Close();
                        //uzak sunucuyu kapat
                        //DBWeb.transaction.Rollback();
                        //DBWeb.conTrans.Close();
                        //continue;
                    }
                    s1= DB.ExecuteSQLTrans("delete from SatisDetay where fkSatislar=" + pkSatislar);
                    if (s1 != "0")
                    {
                        //locak sunucuyu kapat
                        //DB.transaction.Rollback();
                        //DB.conTrans.Close();
                        //uzak sunucuyu kapat
                        //DBWeb.transaction.Rollback();
                        //DBWeb.conTrans.Close();
                        islembasarili = false;
                        //continue;
                    }
                    s2 = DB.ExecuteSQLTrans("delete from Satislar where pkSatislar=" + pkSatislar);
                    if (s2 != "0")
                    {
                        //locak sunucuyu kapat
                        //DB.transaction.Rollback();
                        //DB.conTrans.Close();
                        //uzak sunucuyu kapat
                        //DBWeb.transaction.Rollback();
                        //DBWeb.conTrans.Close();
                        //continue;
                        islembasarili = false;
                    }
                }
                #endregion

                #region trans. işlemi
                if (islembasarili)
                {
                    //local sunucu
                    DB.transaction.Commit();
                    //DB.conTrans.Close();
                    //uzak sunucuyu
                    DBWeb.transaction.Commit();
                    //DBWeb.conTrans.Close();
                    hatasiz++;
                    //listBoxControl1.Items.Add("Fiş No: " + pkSatislar + " Başarılı" );
                }
                else
                {
                    //locak sunucuyu kapat
                    //if (DB.conTrans != null && DB.conTrans.State == ConnectionState.Open)
                    DB.transaction.Rollback();
                    //DB.conTrans.Close();

                    //uzak sunucuyu kapat
                    DBWeb.transaction.Rollback();
                    //DBWeb.conTrans.Close();
                    hatali++;
                    listBoxControl1.Items.Add("Hatalı Fiş No: " + pkSatislar + " Hatalı");
                }
                #endregion

                lblAktarim.Text = "Satış Sayısı:" + c.ToString() + "-" + hatali.ToString() + " Hatalı / " + hatasiz.ToString() + " Hatasız";
            }

            DBWeb.conTrans.Close();
            DB.conTrans.Close();
            
            //MevcutlariGuncelle(); Mevcut 

            SatislarBuBilgisayardaki();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sonuc = "";
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = null;
            try
            {
                dt = DBWeb.GetData_Web("select GetDate() as Tarih");
                if (dt.Rows.Count > 0)
                    sonuc = "Uzak Ana Bilgisayara Bağlantı Sağlandı." + dt.Rows[0][0].ToString();                    

                MessageBox.Show(sonuc);
            }
            catch (Exception exp)
            {
                sonuc = sonuc + exp.Message;
                MessageBox.Show(sonuc);
            }
            Cursor.Current = Cursors.Default;

            simpleButton2.Enabled = false;

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView6.DataRowCount == 0) return;

            //simpleButton3.Enabled = false;
            //müşteriden alınan tahsilatları gönderir. satış tahsilatları hariç
            //listBoxControl1.Items.Clear();

            DataTable dt = DB.GetData(@"select * from KasaHareket kh with(nolock)
                inner join Firmalar f with(nolock) on f.pkFirma=kh.fkFirma
                where fkSatislar is null  and isnull(kh.Aktarildi,0)=0");

            //OdemeSekli not in('Kasa Bakiye Düzeltme','Bakiye Düzeltme') and 
            int hatali = 0, hatasiz = 0, c = dt.Rows.Count;

            if (c == 0) return;
            //uzak
            if (DBWeb.conTrans == null)
                DBWeb.conTrans = new SqlConnection(DBWeb.ConnectionString());

            if (DBWeb.conTrans.State == ConnectionState.Closed)
                DBWeb.conTrans.Open();

            //local
            if (DB.conTrans == null)
                DB.conTrans = new SqlConnection(DB.ConnectionString());

            if (DB.conTrans.State == ConnectionState.Closed)
                DB.conTrans.Open();

            foreach (DataRow dr3 in dt.Rows)
            {
                Application.DoEvents();

                #region trans başlat
                
                DBWeb.transaction = DBWeb.conTrans.BeginTransaction("AdemTransaction");
                DB.transaction = DB.conTrans.BeginTransaction("AktarimTransaction");

                #endregion

                string fkKasaHareket = dr3["pkKasaHareket"].ToString();
                //DataTable dtK = DBWeb.GetData_Web("select pkKasaHareket from KasaHareket with(nolock) where aktarim_fis_no=" + fkKasaHareket);
                //if (dtK.Rows.Count > 0)
                //{
                //    listBoxControl1.Items.Add("Kasa Hareket Tahsilat Daha Önce Kaydedildi" + fkKasaHareket + " Hata:");
                //    continue;
                //}
                string fkKasalar = dr3["fkKasalar"].ToString();
                string Tarih2 = dr3["Tarih"].ToString();
                string Tutar = dr3["Tutar"].ToString();
                string Borc = dr3["Borc"].ToString();
                string Alacak = dr3["Alacak"].ToString();
                string Aciklama3 = dr3["Aciklama"].ToString();
                //string fkFirma3 = dr3["fkFirma"].ToString();
                string Firma_id = dr3["Firma_id"].ToString();//ana bilgisayardaki pkFirma=firma_id
                string fkTuru = dr3["fkTuru"].ToString();
                string OdemeSekli3 = dr3["OdemeSekli"].ToString();
                string fkBankalar = dr3["fkBankalar"].ToString();
                string AktifHesap = dr3["AktifHesap"].ToString();
                string fkTedarikciler = dr3["fkTedarikciler"].ToString();
                string fkAlislar = dr3["fkAlislar"].ToString();
                string fkKasaGirisCikisTurleri = dr3["fkKasaGirisCikisTurleri"].ToString();
                
                string Bakiye = dr3["Bakiye"].ToString();
                if (Bakiye == "") Bakiye = "0";

                string BilgisayarAdi = dr3["BilgisayarAdi"].ToString();
                string fkCek = dr3["fkCek"].ToString();
                string fkKullanicilar = dr3["fkKullanicilar"].ToString();

                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@fkKasalar", fkKasalar));
                list2.Add(new SqlParameter("@Tarih", Convert.ToDateTime(Tarih2)));
                list2.Add(new SqlParameter("@Tutar", Tutar.Replace(",", ".")));
                list2.Add(new SqlParameter("@Aciklama", Aciklama3));
                list2.Add(new SqlParameter("@Borc", Borc.Replace(",", ".")));
                list2.Add(new SqlParameter("@Alacak", Alacak.Replace(",", ".")));
                list2.Add(new SqlParameter("@fkFirma", Firma_id));//fkFirma3)); //ana bilgisayardaki pkFirma=firma_id
                list2.Add(new SqlParameter("@fkTuru", fkTuru));
                list2.Add(new SqlParameter("@OdemeSekli", OdemeSekli3));
                
                if (fkBankalar=="")
                    list2.Add(new SqlParameter("@fkBankalar", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkBankalar", fkBankalar));
                
                list2.Add(new SqlParameter("@AktifHesap", AktifHesap));
                
                if (fkTedarikciler=="")
                    list2.Add(new SqlParameter("@fkTedarikciler", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkTedarikciler", fkTedarikciler));

                if (fkAlislar=="")
                    list2.Add(new SqlParameter("@fkAlislar", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkAlislar", fkAlislar));

                list2.Add(new SqlParameter("@fkKasaGirisCikisTurleri", fkKasaGirisCikisTurleri));
                list2.Add(new SqlParameter("@Bakiye", Bakiye.Replace(",",".")));
                list2.Add(new SqlParameter("@BilgisayarAdi", "Aktarım"));//BilgisayarAdi));
                list2.Add(new SqlParameter("@fkCek", fkCek));
                list2.Add(new SqlParameter("@fkKullanicilar", fkKullanicilar));
                list2.Add(new SqlParameter("@aktarim_fis_no", fkKasaHareket));
                
                string sql = "INSERT INTO KasaHareket (fkKasalar,Tarih,Tutar,Aciklama,Borc,Alacak,fkFirma,fkTuru,OdemeSekli,fkBankalar,AktifHesap,fkTedarikciler,fkAlislar,fkKasaGirisCikisTurleri,Bakiye,BilgisayarAdi,fkCek,fkKullanicilar,Aktarildi,aktarim_fis_no)" +
                    " values(@fkKasalar,@Tarih,@Tutar,@Aciklama,@Borc,@Alacak,@fkFirma,@fkTuru,@OdemeSekli,@fkBankalar,@AktifHesap,@fkTedarikciler,@fkAlislar,@fkKasaGirisCikisTurleri,@Bakiye,@BilgisayarAdi,@fkCek,@fkKullanicilar,1,@aktarim_fis_no)";

                bool islembasarili = true;
                string sonuc = DBWeb.ExecuteSQLTrans(sql, list2);
                //bool islembasarili=false;
                if (sonuc.Substring(0, 1) == "H")
                    islembasarili = false;
                    

                #region sonuç başarılı ise aktarılanı sil
                    if (islembasarili)
                    {
                      //localden sil  
                      //bir kayıt silindi ise 2
                      //hiçbir kayıt silindi ise 0 
                      string s;
                      s = DB.ExecuteSQLTrans("delete from KasaHareket where pkKasaHareket=" + fkKasaHareket);
                      if (s != "0")
                      {
                          //locak sunucuyu kapat
                          //DB.transaction.Rollback();
                          //DB.conTrans.Close();
                         //uzak sunucuyu kapat
                          //DBWeb.transaction.Rollback();
                          //DBWeb.conTrans.Close();
                         //continue;
                          islembasarili = false;
                      }
                }
                #endregion

                #region trans. işlemi
                if (islembasarili)
                {
                    //local sunucu
                    DB.transaction.Commit();
                    //DB.conTrans.Close();
                    //uzak sunucuyu
                    DBWeb.transaction.Commit();
                    hatasiz++;
                    //listBoxControl1.Items.Add("Kasa Id: " + fkKasaHareket + " Başarılı");
                }
                else
                {
                    //locak sunucuyuyu geri al
                    DB.transaction.Rollback();
                    //uzak sunucuyu geri al
                    DBWeb.transaction.Rollback();
                    hatali++;
                    listBoxControl1.Items.Add("Kasa Id: " + fkKasaHareket + " Hatalı");
                }
                #endregion

                lblTahsilat.Text = "Tahsilat Sayısı:" + c.ToString() + "-" + hatali.ToString() + " Hatalı / " + hatasiz.ToString() + " Hatasız";
            }
            DB.conTrans.Close();
            DBWeb.conTrans.Close();
            //BakiyeGuncelle(); kasa kahareketi göndermek gerekiyor
            KasaHareketBuBilgisayardaki();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            listBoxCStokMusteri.Items.Clear();
            //ana makinada al
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = DBWeb.GetData_Web("select * from Firmalar with(nolock) where isnull(Aktarildi,0)=0");
            Cursor.Current = Cursors.Default;
            int hatali = 0, hatasiz = 0, c = dt.Rows.Count;

            foreach (DataRow dr3 in dt.Rows)
            {
                Application.DoEvents();

                #region trans başlat
                //if (DBWeb.conTrans == null)
                //{
                //    DBWeb.conTrans = new SqlConnection(DBWeb.ConnectionString());
                //}

                //if (DBWeb.conTrans.State == ConnectionState.Closed)
                //{
                //    DBWeb.conTrans.Open();
                //}
                //DBWeb.transaction = DB.conTrans.BeginTransaction("AdemTransaction");

                #endregion

                string pkFirma = dr3["pkFirma"].ToString();
                string Firmaadi = dr3["Firmaadi"].ToString();
                string Tel = dr3["Tel"].ToString();
                string Fax = dr3["Fax"].ToString();
                string Adres = dr3["Adres"].ToString();
                string webadresi = dr3["webadresi"].ToString();
                string Eposta = dr3["Eposta"].ToString();
                string Tel2 = dr3["Tel2"].ToString();
                string Aktif = dr3["Aktif"].ToString();
                string Yetkili = dr3["Yetkili"].ToString();
                string fkFirmaGruplari = dr3["fkFirmaGruplari"].ToString();
                string fkil = dr3["fkil"].ToString();
                string fkilce = dr3["fkilce"].ToString();
                string VergiDairesi = dr3["VergiDairesi"].ToString();
                string VergiNo = dr3["VergiNo"].ToString();
                string Cep = dr3["Cep"].ToString();
                string Unvani = dr3["Unvani"].ToString();
                string Cep2 = dr3["Cep2"].ToString();
                string KaraListe = dr3["KaraListe"].ToString();
                string LimitBorc = dr3["LimitBorc"].ToString();
                string tiklamaadedi = dr3["tiklamaadedi"].ToString();
                string OzelKod = dr3["OzelKod"].ToString();
                string GeciciMusteri = dr3["GeciciMusteri"].ToString();
                string fkFirmaAltGruplari = dr3["fkFirmaAltGruplari"].ToString();
                string Borc = dr3["Borc"].ToString();
                string Alacak = dr3["Alacak"].ToString();
                string Bonus = dr3["Bonus"].ToString();
                string Devir = dr3["Devir"].ToString();
                string SonSatisTarihi = dr3["SonSatisTarihi"].ToString();
                string SonOdemeTarihi = dr3["SonOdemeTarihi"].ToString();
                string fkSatisFiyatlariBaslik = dr3["fkSatisFiyatlariBaslik"].ToString();
                string OdemeGunSayisi = dr3["OdemeGunSayisi"].ToString();
                string AidatTutari = dr3["AidatTutari"].ToString();
                string fkPerTeslimEden = dr3["fkPerTeslimEden"].ToString();
                string FaturaUnvani = dr3["FaturaUnvani"].ToString();
                string sec = dr3["sec"].ToString();
                string Webde = dr3["Webde"].ToString();

                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@Firmaadi", Firmaadi));
                list2.Add(new SqlParameter("@Tel", Tel));
                list2.Add(new SqlParameter("@Fax", Fax));
                list2.Add(new SqlParameter("@Adres", Adres));
                list2.Add(new SqlParameter("@webadresi", webadresi));
                list2.Add(new SqlParameter("@Eposta", Eposta));
                list2.Add(new SqlParameter("@Tel2", Tel2));
                list2.Add(new SqlParameter("@Aktif", Aktif));
                if (Yetkili == "")
                    list2.Add(new SqlParameter("@Yetkili", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@Yetkili", Yetkili));

                list2.Add(new SqlParameter("@fkFirmaGruplari", fkFirmaGruplari));

                if (fkil == "")
                    list2.Add(new SqlParameter("@fkil", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkil", fkil));

                if (fkilce == "")
                    list2.Add(new SqlParameter("@fkilce", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkilce", fkilce));

                list2.Add(new SqlParameter("@VergiDairesi", VergiDairesi));
                list2.Add(new SqlParameter("@VergiNo", VergiNo));
                list2.Add(new SqlParameter("@Cep", Cep));
                list2.Add(new SqlParameter("@Unvani", Unvani));
                list2.Add(new SqlParameter("@Cep2", Cep2));
                list2.Add(new SqlParameter("@KaraListe", KaraListe));
                list2.Add(new SqlParameter("@LimitBorc", LimitBorc.Replace(",",".")));
                list2.Add(new SqlParameter("@tiklamaadedi", tiklamaadedi));
                list2.Add(new SqlParameter("@OzelKod", OzelKod));
                list2.Add(new SqlParameter("@GeciciMusteri", GeciciMusteri));
                list2.Add(new SqlParameter("@fkFirmaAltGruplari", fkFirmaAltGruplari));
                //list2.Add(new SqlParameter("@Borc", Borc.Replace(",",".")));
                //list2.Add(new SqlParameter("@Alacak", Alacak.Replace(",",".")));
                if (Bonus == "") Bonus = "0";
                list2.Add(new SqlParameter("@Bonus", Bonus.Replace(",", ".")));
                list2.Add(new SqlParameter("@Devir", Devir.Replace(",", ".")));
                list2.Add(new SqlParameter("@fkSatisFiyatlariBaslik", fkSatisFiyatlariBaslik));
                list2.Add(new SqlParameter("@OdemeGunSayisi", OdemeGunSayisi));
                list2.Add(new SqlParameter("@FaturaUnvani", FaturaUnvani));
                list2.Add(new SqlParameter("@Firma_id", pkFirma));
                //list2.Add(new SqlParameter("@sec", sec));
                
                //list2.Add(new SqlParameter("@Tarih", Convert.ToDateTime(Tarih2)));
                //müşterileri alırken varsa güncelle
                string sql = "";
                DataTable dtFirma = DB.GetData("select Firma_id from Firmalar with(nolock) where Firma_id=" + pkFirma);
                string anabilgisayar_pkFirma="0";
                if (dtFirma.Rows.Count == 0)
                {
                   sql = "INSERT INTO Firmalar (Firmaadi,Tel,Fax,Adres,webadresi,Eposta,Tel2,Aktif,Yetkili,fkFirmaGruplari,fkil,fkilce,VergiDairesi,VergiNo," +
                   "Cep,Unvani,Cep2,KaraListe,LimitBorc,tiklamaadedi,OzelKod,GeciciMusteri,fkFirmaAltGruplari,Bonus,Devir,fkSatisFiyatlariBaslik,OdemeGunSayisi,FaturaUnvani,Firma_id)" +
                   " values(@Firmaadi,@Tel,@Fax,@Adres,@webadresi,@Eposta,@Tel2,@Aktif,@Yetkili,@fkFirmaGruplari,@fkil,@fkilce,@VergiDairesi,@VergiNo," +
                   "@Cep,@Unvani,@Cep2,@KaraListe,@LimitBorc,@tiklamaadedi,@OzelKod,@GeciciMusteri,@fkFirmaAltGruplari,@Bonus,@Devir,@fkSatisFiyatlariBaslik,@OdemeGunSayisi,@FaturaUnvani,@Firma_id) select IDENT_CURRENT('Firmalar')";

                   anabilgisayar_pkFirma = DB.ExecuteScalarSQL(sql, list2);
                }
                else
                {
                    anabilgisayar_pkFirma=dtFirma.Rows[0]["Firma_id"].ToString();

                    sql = "UPDATE Firmalar SET Firmaadi=@Firmaadi,Tel=@Tel,Fax=@Fax,Adres=@Adres,webadresi=@webadresi,Eposta=@Eposta,Tel2=@Tel2,Aktif=@Aktif,Yetkili=@Yetkili,fkFirmaGruplari=@fkFirmaGruplari," +
                    "fkil=@fkil,fkilce=@fkilce,VergiDairesi=@VergiDairesi,VergiNo=@VergiNo,Cep=@Cep,Unvani=@Unvani,Cep2=@Cep2,KaraListe=@KaraListe,LimitBorc=@LimitBorc,tiklamaadedi=@tiklamaadedi,OzelKod=@OzelKod," +
                    "GeciciMusteri=@GeciciMusteri,fkFirmaAltGruplari=@fkFirmaAltGruplari,Bonus=@Bonus,Devir=@Devir,fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik,OdemeGunSayisi=@OdemeGunSayisi,FaturaUnvani=@FaturaUnvani WHERE Firma_id=@Firma_id";

                    anabilgisayar_pkFirma = DB.ExecuteSQL(sql, list2);
                }
                

                bool islembasarili = false;
                if (anabilgisayar_pkFirma.Substring(0, 1) == "H")
                {
                    islembasarili = false;
                    listBoxCStokMusteri.Items.Add("Müşteri: " + Firmaadi + " Hata:" + anabilgisayar_pkFirma);
                    //hatali++;
                    //DBWeb.transaction.Rollback();
                    //DBWeb.conTrans.Close();
                    //break kullanıldı için rool back aşağıda sonlandırıldı.
                    //break;
                    //continue;
                }
                else
                {
                    islembasarili = true;
                    //hatasiz++;
                }
                #region sonuç başarılı ise kasa hareketine devir ekle
                if (islembasarili)
                {
                    sql = @"delete from KasaHareket where fkFirma=" + anabilgisayar_pkFirma;
                    int sonu = DB.ExecuteSQL_Sonuc_Sifir(sql);
                    if (sonu != 0)
                    {
                        islembasarili = false;
                    }
                }

                sql = @"INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,BilgisayarAdi)
                    values(1,1,getdate(),3,1,@Borc,@Alacak,'Aktarım',0,1,@fkFirma,0,'Kasa Bakiye Düzeltme',@Tutar,'Aktarım')";

                    sql = sql.Replace("@fkFirma", anabilgisayar_pkFirma);
                    sql = sql.Replace("@Tutar","0");//Devir.Replace(",", "."));

                    decimal ddevir = 0;
                    decimal.TryParse(Devir.Replace(".", ","),out ddevir);//NOKTA VİRGÜL DECİMALDE FARKLI
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

                    if (islembasarili)
                    {
                        int sonuc1 = DB.ExecuteSQL_Sonuc_Sifir(sql);
                        if (sonuc1 != 0)
                        {
                            islembasarili = false;
                        }
                    }

                #endregion

                #region sonuç başarılı ise aktarılanı sil

                #region trans başlat
                //if (DBWeb.conTrans == null)
                //{
                //    DBWeb.conTrans = new SqlConnection(DBWeb.ConnectionString());
                //}

                //if (DBWeb.conTrans.State == ConnectionState.Closed)
                //{
                //    //DB.conTrans = new SqlConnection(DB.ConnectionString());
                //    DBWeb.conTrans.Open();
                //    //transaction = conTrans.BeginTransaction("AdemTransaction");
                //}
                //DBWeb.transaction = DBWeb.conTrans.BeginTransaction("AktarimTransaction");
                #endregion

                if (islembasarili)
                {
                    //localden sil  
                    //bir kayıt silindi ise 2
                    //hiçbir kayıt silindi ise 0 
                    int s= DBWeb.ExecuteSQL("update Firmalar Set Aktarildi=1 where pkFirma=" + pkFirma);
                    if (s != 0)
                    {
                        listBoxCStokMusteri.Items.Add("Müşteri: " + Firmaadi + " Hata:" + s);
                        //locak sunucuyu kapat
                        //DBWeb.transaction.Rollback();
                        //DBWeb.conTrans.Close();
                        continue;
                    }
                }
                #endregion

                #region trans. işlemi
                if (islembasarili)
                {
                    //DBWeb.transaction.Commit();
                    //DBWeb.conTrans.Close();
                    hatasiz++;
                    //listBoxControl1.Items.Add("Kasa Id: " + fkKasaHareket + " Başarılı");
                }
                else
                {
                    //if (DBWeb.conTrans.State == ConnectionState.Open)
                    //    DBWeb.transaction.Rollback();

                    //DBWeb.conTrans.Close();
                    hatali++;
                }
                #endregion

                lblMusteriler.Text = "Müşteri Sayısı:" + c.ToString() + "-" + hatali.ToString() + " Hatalı / " + hatasiz.ToString() + " Hatasız";
            }
            MusterilerUzak();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            listBoxCStokMusteri.Items.Clear();
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = DBWeb.GetData_Web("select * from StokKarti with(nolock) where isnull(Aktarildi,0)=0");
            Cursor.Current = Cursors.Default;

            int hatali = 0, hatasiz = 0, c = dt.Rows.Count;

            if (dt.Rows.Count == 1 && dt.Columns.Count == 1)
            {
                MessageBox.Show("Hata : " + dt.Rows[0][0].ToString());
                return;
            }
            foreach (DataRow dr3 in dt.Rows)
            {
                Application.DoEvents();

                #region local trans başlat
                if (DB.conTrans == null)
                {
                    DB.conTrans = new SqlConnection(DB.ConnectionString());
                }

                if (DB.conTrans.State == ConnectionState.Closed)
                {
                    DB.conTrans.Open();
                }
                DB.transaction = DB.conTrans.BeginTransaction("AdemTransaction");

                #endregion

                #region Degerler
                string pkStokKarti = dr3["pkStokKarti"].ToString();

                string Stokadi = dr3["Stokadi"].ToString();
                string Barcode = dr3["Barcode"].ToString();
                string StokKod = dr3["StokKod"].ToString();
                string fkStokAltGruplari = dr3["fkStokAltGruplari"].ToString();
                string Stoktipi = dr3["Stoktipi"].ToString();
                string KdvOrani = dr3["KdvOrani"].ToString();
                string Aktif = dr3["Aktif"].ToString();
                string Mevcut = dr3["Mevcut"].ToString();
                string ToplamGiren = dr3["ToplamGiren"].ToString();
                string ToplamCikan = dr3["ToplamCikan"].ToString();
                string AlisFiyati = dr3["AlisFiyati"].ToString();
                string SatisFiyati = dr3["SatisFiyati"].ToString();

                string KutuFiyat = dr3["KutuFiyat"].ToString();
                string fkModel = dr3["fkModel"].ToString();
                string fkMarka = dr3["fkMarka"].ToString();
                string fkStokGrup = dr3["fkStokGrup"].ToString();
                string KritikMiktar = dr3["KritikMiktar"].ToString();
                string HizliSatisAdi = dr3["HizliSatisAdi"].ToString();
                string HizliSiraNo = dr3["HizliSiraNo"].ToString();

                string tiklamaadedi = dr3["tiklamaadedi"].ToString();
                string fkBedenGrupKodu = dr3["fkBedenGrupKodu"].ToString();
                string fkRenkGrupKodu = dr3["fkRenkGrupKodu"].ToString();
                string RBGKodu = dr3["RBGKodu"].ToString();
                string MevcutFarki = dr3["MevcutFarki"].ToString();
                string fkTedarikciler = dr3["fkTedarikciler"].ToString();
                string AlisFiyatiKdvHaric = dr3["AlisFiyatiKdvHaric"].ToString();
                string UreticiKodu = dr3["UreticiKodu"].ToString();
                string AlisFiyatiiskontolu = dr3["AlisFiyatiiskontolu"].ToString();
                string SatisFiyatiKdvHaric = dr3["SatisFiyatiKdvHaric"].ToString();
                string KdvOraniAlis = dr3["KdvOraniAlis"].ToString();
                //string MevcutFatura = dr3["MevcutFatura"].ToString();
                string SatisAdedi = dr3["SatisAdedi"].ToString();
                string fkBirimler = dr3["fkBirimler"].ToString();
                string alis_iskonto = dr3["alis_iskonto"].ToString();
                string satis_iskonto = dr3["satis_iskonto"].ToString();
                string KdvHaric = dr3["KdvHaric"].ToString();
                #endregion

                #region list
                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@Stokadi", Stokadi));
                list2.Add(new SqlParameter("@Barcode", Barcode));
                list2.Add(new SqlParameter("@StokKod", StokKod));
                list2.Add(new SqlParameter("@Stoktipi", Stoktipi));
                list2.Add(new SqlParameter("@fkStokAltGruplari", fkStokAltGruplari));
                list2.Add(new SqlParameter("@KdvOrani", KdvOrani));
                list2.Add(new SqlParameter("@Aktif", Aktif));
                list2.Add(new SqlParameter("@Mevcut", Mevcut.Replace(",", ".")));

                if (ToplamGiren == "")
                    list2.Add(new SqlParameter("@ToplamGiren", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@ToplamGiren", ToplamGiren));

                if (ToplamCikan == "")
                    list2.Add(new SqlParameter("@ToplamCikan", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@ToplamCikan", ToplamCikan));

                list2.Add(new SqlParameter("@AlisFiyati", AlisFiyati.Replace(",", ".")));
                list2.Add(new SqlParameter("@SatisFiyati", SatisFiyati.Replace(",", ".")));
                list2.Add(new SqlParameter("@KutuFiyat", KutuFiyat.Replace(",", ".")));
                list2.Add(new SqlParameter("@fkModel", fkModel));
                list2.Add(new SqlParameter("@fkMarka", fkMarka));
                list2.Add(new SqlParameter("@fkStokGrup", fkStokGrup));
                list2.Add(new SqlParameter("@KritikMiktar", KritikMiktar.Replace(",", ".")));
                list2.Add(new SqlParameter("@HizliSatisAdi", HizliSatisAdi));
                list2.Add(new SqlParameter("@HizliSiraNo", HizliSiraNo));
                list2.Add(new SqlParameter("@tiklamaadedi", tiklamaadedi));

                list2.Add(new SqlParameter("@fkBedenGrupKodu", fkBedenGrupKodu));
                list2.Add(new SqlParameter("@fkRenkGrupKodu", fkRenkGrupKodu));
                list2.Add(new SqlParameter("@RBGKodu", RBGKodu));
                list2.Add(new SqlParameter("@MevcutFarki", MevcutFarki));
                list2.Add(new SqlParameter("@fkTedarikciler", fkTedarikciler));
                if (AlisFiyatiKdvHaric=="")
                    list2.Add(new SqlParameter("@AlisFiyatiKdvHaric",DBNull.Value));
                else
                    list2.Add(new SqlParameter("@AlisFiyatiKdvHaric", AlisFiyatiKdvHaric.Replace(",",".")));

                if (UreticiKodu=="")
                    list2.Add(new SqlParameter("@UreticiKodu", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@UreticiKodu", UreticiKodu));

                if (AlisFiyatiiskontolu == "") AlisFiyatiiskontolu = "0";
                list2.Add(new SqlParameter("@AlisFiyatiiskontolu", AlisFiyatiiskontolu.Replace(",", ".")));

                if (SatisFiyatiKdvHaric == "") SatisFiyatiKdvHaric = "0";
                list2.Add(new SqlParameter("@SatisFiyatiKdvHaric", SatisFiyatiKdvHaric.Replace(",", ".")));

                if( KdvOraniAlis=="")
                    list2.Add(new SqlParameter("@KdvOraniAlis", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@KdvOraniAlis", KdvOraniAlis));

                list2.Add(new SqlParameter("@SatisAdedi", SatisAdedi.Replace(",",".")));
                
                if (fkBirimler=="")
                    list2.Add(new SqlParameter("@fkBirimler", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkBirimler", fkBirimler));

                if (alis_iskonto == "") alis_iskonto = "0";
                list2.Add(new SqlParameter("@alis_iskonto", alis_iskonto.Replace(",", ".")));
                if (satis_iskonto == "") satis_iskonto = "0";
                list2.Add(new SqlParameter("@satis_iskonto", satis_iskonto.Replace(",", ".")));
                if (KdvHaric == "") KdvHaric = "0";
                list2.Add(new SqlParameter("@KdvHaric", KdvHaric));
                list2.Add(new SqlParameter("@StokKarti_id", pkStokKarti));
                
                //list2.Add(new SqlParameter("@Tarih", Convert.ToDateTime(Tarih2)));
                #endregion

                 string sql = "";
                 string yeni_pkStokKarti_sonuc = "0";
                 DataTable dtStok = DB.GetDataTrans("select pkStokKarti,StokKarti_id from StokKarti with(nolock) where StokKarti_id=" + pkStokKarti);

                 string ana_bilgisayar_StokKarti_id = pkStokKarti, client_pkStokKarti ="0";
                 if (dtStok.Rows.Count > 0)
                 {
                     client_pkStokKarti = dtStok.Rows[0]["pkStokKarti"].ToString();
                     ana_bilgisayar_StokKarti_id = dtStok.Rows[0]["StokKarti_id"].ToString();
                 }

                if (dtStok.Rows.Count == 0)
                 {
                     sql = "INSERT INTO StokKarti (Stokadi,Barcode,StokKod,fkStokAltGruplari,Stoktipi,KdvOrani,Aktif,ToplamGiren,ToplamCikan,Mevcut,SatisFiyati,AlisFiyati,KutuFiyat,fkModel," +
                    "fkMarka,fkStokGrup,KritikMiktar,HizliSatisAdi,HizliSiraNo,tiklamaadedi,fkBedenGrupKodu,RBGKodu,MevcutFarki,fkTedarikciler,AlisFiyatiKdvHaric,UreticiKodu,AlisFiyatiiskontolu," +
                    "SatisFiyatiKdvHaric,KdvOraniAlis,SatisAdedi,fkBirimler,alis_iskonto,satis_iskonto,KdvHaric,StokKarti_id)" +
                    " values(@Stokadi,@Barcode,@StokKod,@fkStokAltGruplari,@Stoktipi,@KdvOrani,@Aktif,@ToplamGiren,@ToplamCikan,@Mevcut,@SatisFiyati,@AlisFiyati,@KutuFiyat,@fkModel," +
                    "@fkMarka,@fkStokGrup,@KritikMiktar,@HizliSatisAdi,@HizliSiraNo,@tiklamaadedi,@fkBedenGrupKodu,@RBGKodu,@MevcutFarki,@fkTedarikciler,@AlisFiyatiKdvHaric,@UreticiKodu,@AlisFiyatiiskontolu," +
                    "@SatisFiyatiKdvHaric,@KdvOraniAlis,@SatisAdedi,@fkBirimler,@alis_iskonto,@satis_iskonto,@KdvHaric,@StokKarti_id) select IDENT_CURRENT('StokKarti')";

                     yeni_pkStokKarti_sonuc = DB.ExecuteScalarSQLTrans(sql, list2);
                     client_pkStokKarti = yeni_pkStokKarti_sonuc;
                 }
                 else
                 {
                     sql = "UPDATE StokKarti SET Stokadi=@Stokadi,Barcode=@Barcode,StokKod=@StokKod,fkStokAltGruplari=@fkStokAltGruplari,Stoktipi=@Stoktipi,KdvOrani=@KdvOrani,Aktif=@Aktif,ToplamGiren=@ToplamGiren,ToplamCikan=@ToplamCikan," +
                     "Mevcut=@Mevcut,SatisFiyati=@SatisFiyati,AlisFiyati=@AlisFiyati,KutuFiyat=@KutuFiyat,fkModel=@fkModel," +
                     "fkMarka=@fkMarka,fkStokGrup=@fkStokGrup,KritikMiktar=@KritikMiktar,HizliSatisAdi=@HizliSatisAdi,HizliSiraNo=@HizliSiraNo," +
                     "tiklamaadedi=@tiklamaadedi,fkBedenGrupKodu=@fkBedenGrupKodu,RBGKodu=@RBGKodu,MevcutFarki=@MevcutFarki,fkTedarikciler=@fkTedarikciler,AlisFiyatiKdvHaric=@AlisFiyatiKdvHaric," +
                     "UreticiKodu=@UreticiKodu,AlisFiyatiiskontolu=@AlisFiyatiiskontolu,SatisFiyatiKdvHaric=@SatisFiyatiKdvHaric,KdvOraniAlis=@KdvOraniAlis,SatisAdedi=@SatisAdedi,fkBirimler=@fkBirimler," +
                     "alis_iskonto=@alis_iskonto,satis_iskonto=@satis_iskonto,KdvHaric=@KdvHaric WHERE StokKarti_id=" + pkStokKarti;

                     yeni_pkStokKarti_sonuc = DB.ExecuteSQLTrans(sql, list2);
                 }
                

                bool islembasarili = false;
                if (yeni_pkStokKarti_sonuc.Substring(0, 1) == "H")
                {
                    islembasarili = false;
                    listBoxCStokMusteri.Items.Add("Stok Adı: " + Stokadi + " Hata:" + yeni_pkStokKarti_sonuc);
                    //hatali++;
                    //DBWeb.transaction.Rollback();
                    //DBWeb.conTrans.Close();
                    //break kullanıldı için rool back aşağıda sonlandırıldı.
                    //break;
                    //continue;
                }
                else
                {
                    islembasarili = true;
                    //hatasiz++;
                }
                #region Satış Fiyatlarını Aktar
                bool satisfiyatlari_Aktarildimi = true;

                //fiyatı değişmişde olabilir yeni stok kartıda olabilir.
                //yeni stok kartı ise StokKarti_id null gelecektir.

                //client da fiyat var mı? var ise StokKarti_id al
                //DataTable dtFiyat = DB.GetData("select * from SatisFiyatlari sf with(nolock) where fkStokKarti=" + client_pkStokKarti);

                //string client_fkStokKarti ="0";
                //if (dtFiyat.Rows.Count > 0)
                //   client_fkStokKarti = dtFiyat.Rows[0]["fkStokKarti"].ToString();

                //ana bilgisayardaki fiyatları al
                DataTable dtSatisFiyatlari = DBWeb.GetData_Web("SELECT * FROM SatisFiyatlari with(nolock) where fkStokKarti=" + ana_bilgisayar_StokKarti_id);

                foreach (DataRow drFiyat in dtSatisFiyatlari.Rows)
                {
                    string fkSatisFiyatlariBaslik = drFiyat["fkSatisFiyatlariBaslik"].ToString();

                    ArrayList list3 = new ArrayList();
                    list3.Add(new SqlParameter("@fkStokKarti", client_pkStokKarti));

                    DataTable dtf = DB.GetDataTrans("SELECT * FROM SatisFiyatlari with(nolock) where fkSatisFiyatlariBaslik=" +
                        fkSatisFiyatlariBaslik+" and fkStokKarti=" + client_pkStokKarti);
                    if (dtf.Rows.Count == 0)
                    {
                        sql = "INSERT INTO SatisFiyatlari(fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,Aktif) " +
                            " VALUES(@fkStokKarti,@fkSatisFiyatlariBaslik,@SatisFiyatiKdvli,@SatisFiyatiKdvsiz,0,@Aktif)";
                    }
                    else
                    {
                        sql = "UPDATE SatisFiyatlari SET fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik,SatisFiyatiKdvli=@SatisFiyatiKdvli," +
                            "SatisFiyatiKdvsiz=@SatisFiyatiKdvsiz,iskontoYuzde=0,Aktif=@Aktif where fkStokKarti=@fkStokKarti and fkSatisFiyatlariBaslik="+
                            fkSatisFiyatlariBaslik;
                    }

                    list3.Add(new SqlParameter("@fkSatisFiyatlariBaslik", fkSatisFiyatlariBaslik));

                    if (drFiyat["SatisFiyatiKdvli"].ToString() == "")
                        list3.Add(new SqlParameter("@SatisFiyatiKdvli", DBNull.Value));
                    else
                        list3.Add(new SqlParameter("@SatisFiyatiKdvli", drFiyat["SatisFiyatiKdvli"].ToString().Replace(",", ".")));

                    if (drFiyat["SatisFiyatiKdvsiz"].ToString() == "")
                        list3.Add(new SqlParameter("@SatisFiyatiKdvsiz", DBNull.Value));
                    else
                        list3.Add(new SqlParameter("@SatisFiyatiKdvsiz", drFiyat["SatisFiyatiKdvsiz"].ToString().Replace(",", ".")));

                    if (drFiyat["Aktif"].ToString()== "")
                        list3.Add(new SqlParameter("@Aktif", "0"));
                    else
                        list3.Add(new SqlParameter("@Aktif", drFiyat["Aktif"].ToString().Replace(",", ".")));
                    string sonuc = DB.ExecuteSQLTrans(sql, list3);

                    if (sonuc.Substring(0, 1) == "H")
                    {
                        satisfiyatlari_Aktarildimi = false;
                        listBoxCStokMusteri.Items.Add("SatisFiyatlari: " + Stokadi + " Hata:" + sonuc);
                    }
                }

                islembasarili = satisfiyatlari_Aktarildimi;
                #endregion

                #region sonuç başarılı ise aktarılanı=1 yap

                #region trans başlat
                //if (DBWeb.conTrans == null)
                //{
                //    DBWeb.conTrans = new SqlConnection(DBWeb.ConnectionString());
                //}

                //if (DBWeb.conTrans.State == ConnectionState.Closed)
                //{
                //    //DB.conTrans = new SqlConnection(DB.ConnectionString());
                //    DBWeb.conTrans.Open();
                //    //transaction = conTrans.BeginTransaction("AdemTransaction");
                //}
                //DBWeb.transaction = DBWeb.conTrans.BeginTransaction("AktarimTransaction");
                #endregion

                if (islembasarili)
                {
                    //localden sil  
                    //bir kayıt silindi ise 2
                    //hiçbir kayıt silindi ise 0 
                    int s = DBWeb.ExecuteSQL("update StokKarti Set Aktarildi=1 where pkStokKarti=" + ana_bilgisayar_StokKarti_id);
                    if (s != 0)
                    {
                        //locak sunucuyu kapat
                        listBoxCStokMusteri.Items.Add("Stok Adı: " + Stokadi + " Hata:" + s);
                        DB.transaction.Rollback();
                        DB.conTrans.Close();
                        continue;
                    }
                }
                #endregion

                #region trans. işlemi
                if (islembasarili)
                {
                    DB.transaction.Commit();
                    DB.conTrans.Close();
                    hatasiz++;
                }
                else
                {
                    if (DB.conTrans.State == ConnectionState.Open)
                        DB.transaction.Rollback();

                    DB.conTrans.Close();
                    hatali++;
                }
                #endregion

                lblStoklar.Text = "Stok Sayısı:" + c.ToString() + "-" + hatali.ToString() + " Hatalı / " + hatasiz.ToString() + " Hatasız";
            }
            StoklarUzak();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (cbYedekAl.Checked)
            {
                listBoxControl1.Items.Add("Yedek Alınıyor...");

                bool b = Digerislemler.YedekAl();
                if (b == true)
                    listBoxControl1.Items.Add("Yedek Alındı.");
                else
                    listBoxControl1.Items.Add("Yedek Alınırken Hata Oluştu.");
            }

            DataTable dtToplamAlacak = 
            DBWeb.GetData_Web("select isnull(sum(Devir),0) as ToplamAlacak  From Firmalar with(nolock)");
            decimal ToplamAlacak=0;
            decimal.TryParse(dtToplamAlacak.Rows[0][0].ToString(),out ToplamAlacak);
            ceOncekiBakiye.Value = ToplamAlacak;

            decimal acik_hesap = 0;
            if (gridColumn23.SummaryItem.SummaryValue != null)
                acik_hesap = decimal.Parse(gridColumn23.SummaryItem.SummaryValue.ToString());

            decimal tahsilat = 0;
            if (gridColumn71.SummaryItem.SummaryValue != null)
                tahsilat = decimal.Parse(gridColumn71.SummaryItem.SummaryValue.ToString());

            sbtnSatislar_Click(sender, e);//satışlar 
            simpleButton3_Click(sender, e);//tahsilatlar

            DataTable dtToplamAlacakSon =
            DBWeb.GetData_Web("select isnull(sum(Devir),0) as ToplamAlacak  From Firmalar with(nolock)");
            decimal ToplamAlacakson = 0;
            decimal.TryParse(dtToplamAlacakSon.Rows[0][0].ToString(), out ToplamAlacakson);
            ceOncekiBakiyeSon.Value = ToplamAlacakson;

            ArrayList list = new ArrayList();

            string sql = "insert into AktarimHareketleri (Tarih,onceki_bakiye,acik_hesap,tahsilat,son_bakiye,aciklama,fkKullanicilar) " +
                " values(getdate(),@onceki_bakiye,@acik_hesap,@tahsilat,@son_bakiye,@aciklama,@fkKullanicilar)";

            list.Add(new SqlParameter("@onceki_bakiye", ceOncekiBakiye.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@acik_hesap", acik_hesap.ToString().Replace(",", ".")));        
            list.Add(new SqlParameter("@tahsilat", tahsilat.ToString().Replace(",", ".")));

            list.Add(new SqlParameter("@son_bakiye", ceOncekiBakiyeSon.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
            list.Add(new SqlParameter("@aciklama", "test"));
            
            DBWeb.ExecuteSQL_Web(sql, list);

        }

        private void btnAktarim_Click(object sender, EventArgs e)
        {
            frmAktarim Aktarim = new frmAktarim();
            Aktarim.ShowDialog();
        }

        private void btnMusteriAktarildi_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {

        }

        private void tümStoklarıAktarılmadıYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(DBWeb.UzakSunucu + " Bilgisayardaki, Tüm Stokları Aktarılmadı Yapılacak?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            DBWeb.ExecuteSQL("update StokKarti set Aktarildi=0");

            StoklarUzak();
        }

        private void stoklarıSilbuBilgisayardakiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(DB.VeriTabaniAdresi + " Bilgisayardaki, Tüm Stok Kartları Silinsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            int s= DB.ExecuteSQL("truncate table SatisFiyatlari");
            if (s != 0)
            {
                formislemleri.Mesajform(s.ToString(), "K", 200);
                return;
            }

            s = DB.ExecuteSQL("truncate table StokKarti");

            if (s != 0)
            {
                formislemleri.Mesajform(s.ToString(), "K", 200);
                return;
            }
        }


        private void tümMüşterileriAktarılmadıYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(DBWeb.UzakSunucu + " Bilgisayardaki, Tüm Müşteriler Aktarılmadı Yapılacak?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            DBWeb.ExecuteSQL("update Firmalar set Aktarildi=0");

            MusterilerUzak();
        }

        private void müşterileriSilBuBilgisayarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(DB.VeriTabaniAdresi + " Bilgisayardaki, Tüm Müşteriler Silinsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("truncate table Firmalar");
            //DB.ExecuteSQL("INSERT INTO Firmalar (Firmaadi,Aktif,fkSirket,fkFirmaGruplari,fkil,fkilce,KaraListe,LimitBorc,tiklamaadedi,OzelKod,GeciciMusteri,Borc,Alacak) VALUES('GEÇİCİ MÜŞTERİ',1,1,1,41,7,0,0,0,1,1,0,0)");
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;

            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);

            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            sbtnSatislar.Enabled = true;
            SatislarBuBilgisayardaki();
        }

        private void simpleButton9_Click_1(object sender, EventArgs e)
        {
            simpleButton3.Enabled = true;
            KasaHareketBuBilgisayardaki();
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            KasaHareketBuBilgisayardaki(); 
            SatislarBuBilgisayardaki();
        }

        void MevcutlariGuncelle()
        {
            DataTable dtUzak = DBWeb.GetData_Web("select * from StokKarti with(nolock)");// where isnull(Aktarildi,0)=0");

            if (dtUzak.Rows.Count == 0)  return;
            
            foreach (DataRow dr in dtUzak.Rows)
            {
                string pkStokKarti = dr["pkStokKarti"].ToString();
                string Mevcut = dr["Mevcut"].ToString();
                DB.ExecuteSQL("update StokKarti set Mevcut=" + Mevcut.Replace(",",".") + " where pkStokKarti=" + pkStokKarti);
            }
            listBoxControl1.Items.Add("Mevcutlar Güncellendi");
            formislemleri.Mesajform("Mevcutlar Güncellendi", "S", 200);
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
            listBoxControl1.Items.Add("Bakiyeler Güncellendi");

            formislemleri.Mesajform("Bakiyeler Güncellendi", "S", 200);
        }
        private void mevcutlarıGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MevcutlariGuncelle();
        }

        private void bakiyeleriGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BakiyeGuncelle();
        }

        private void simpleButton15_Click_1(object sender, EventArgs e)
        {
            StoklarUzak();
            MusterilerUzak();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            simpleButton4_Click(sender, e);//müşteri
            simpleButton5_Click(sender, e);//stok

        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            if (DB.GetData("select pkSatislar from Satislar with(nolock)").Rows.Count > 0)
            {
                formislemleri.Mesajform("önce satışları aktarınız", "K", 200);
                return;
            }


            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(DB.VeriTabaniAdresi + " Bilgisayardaki, Tüm Müşteriler Güncellensin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            //firmaları sil
            DB.ExecuteSQL("truncate table Firmalar");

            //ana bilgisayardan sil
            DBWeb.ExecuteSQL("update Firmalar set Aktarildi=0");

            MusterilerUzak();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(DB.VeriTabaniAdresi + " Bilgisayardaki, Tüm Stok Kartları Silinsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            int s = 
            DB.ExecuteSQL("truncate table SatisFiyatlari");

            s= DB.ExecuteSQL("truncate table StokKarti");

            //uzaktaki(server) stokları aktarılmadı yap
            DBWeb.ExecuteSQL("update StokKarti set Aktarildi=0");

            StoklarUzak();
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            string sql = "select * from Satislar with(nolock) where aktarildi=0";
            DataTable dtS = DB.GetData(sql);
            if (dtS.Rows.Count > 0)
            {
                MessageBox.Show("Önce Satışları Ana Bilgisayara Gönderin");
                return;
            }
            sql = "select * from KasaHareket with(nolock) where aktarildi=0";
            DataTable dtKh = DB.GetData(sql);
            if (dtKh.Rows.Count > 0)
            {
                MessageBox.Show("Önce Kasa Hareketleri Ana Bilgisayara Gönderin");
                return;
            }

            listBoxControl2.Items.Clear();

            string yedek_alinacak_yer_paylasimli = "", yer1 = "", yer2 = "", yedekalinacakyer = "c:\\hititdb\\yedek.bak";

            DataTable dtAyar = DB.GetData("select top 1  * from Sirketler with(nolock)");

            yedek_alinacak_yer_paylasimli = dtAyar.Rows[0]["yedek_alinacak_yer_paylasimli"].ToString();
            listBoxControl2.Items.Add("yedek_alinacak_yer_paylasimli = "+ yedek_alinacak_yer_paylasimli);
            //yedekalinacakyer = dtAyar.Rows[0]["yedekalinacakyer"].ToString();
            if (yedek_alinacak_yer_paylasimli == "")
            {
                MessageBox.Show("yedek_alinacak_yer_paylasimli Boş Olamaz. Lütfen Klasör Seçiniz!");
                return;
            }
            //klasör olacak
            //if (!File.Exists(yedek_alinacak_yer_paylasimli))
            //{
            //    MessageBox.Show("yedek_alinacak_yer_paylasimli Bulunamadı. Lütfen Yeni Klasör Oluşturunuz!");
            //    return;
            //}

            sql = @"BEGIN TRY
            BACKUP DATABASE [MTP2012] TO  DISK = '@yer'  
            WITH NOFORMAT, NOINIT,   
            NAME = N'Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10 
            select ('1') as sonuc
            END TRY
            BEGIN CATCH;
                SELECT ERROR_MESSAGE() AS sonuc;
            END CATCH";


            yer1 = yedek_alinacak_yer_paylasimli + "\\yedek.bak";
            string kaynak1 = "\\\\" + DBWeb.UzakSunucu + "\\hititdb\\yedek.bak";
            listBoxControl2.Items.Add("Yedek Alınıyor...");
            listBoxControl2.Items.Add("kaynak = " + kaynak1);
            if (File.Exists(kaynak1))
                File.Delete(kaynak1);

            sql = sql.Replace("@yer", yer1);

            DataTable dt1 = DBWeb.GetData_Web(sql);
            string sonuc = dt1.Rows[0][0].ToString();
            if (sonuc == "1")
            {
                listBoxControl2.Items.Add("Yedek Alındı");

                string kaynak = "\\\\" + DBWeb.UzakSunucu + "\\hititdb\\yedek.bak";
                if (File.Exists(yedekalinacakyer))
                    File.Delete(yedekalinacakyer);
                listBoxControl2.Items.Add(kaynak + " kaynak dan dosya kopyalanıyor");

                File.Copy(kaynak, yedekalinacakyer);

                listBoxControl2.Items.Add("Dosya Kopyalandı.");
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Yedek Alınırken Hata Oluştur.\nHata Mesajı: " + sonuc, Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable dtKill= 
            DB.GetData("SELECT * from master.dbo.sysprocesses where dbid = db_id('MTP2012')");
            for (int i = 0; i < dtKill.Rows.Count; i++)
            {
                DB.ExecuteSQL("KILL " + dtKill.Rows[i]["spid"].ToString());
            }
            listBoxControl2.Items.Add("kullanıcılar devre dışı bırakıldı. kill");

            sql = @"use [master]
            
            BEGIN TRY

            RESTORE DATABASE [MTP2012] from  DISK = '@yer'  
            WITH    
            file = 1, 
            NOUNLOAD, 
            REPLACE,
            STATS = 1
            select ('1') as sonuc
            END TRY
            BEGIN CATCH;
                SELECT ERROR_MESSAGE() AS sonuc;
            END CATCH";

            yer2 = yedek_alinacak_yer_paylasimli + "\\yedek.bak";

            sql = sql.Replace("@yer", yer2);
            listBoxControl2.Items.Add("Yedek Geri Yükleniyor " + yer2);
            DataTable dt2 = DB.GetData(sql);
            string sonuc2 = "1";
            if(dt2.Rows.Count>0)
               sonuc2 = dt2.Rows[0][0].ToString();

            if (sonuc2 == "1")
            {
                listBoxControl2.Items.Add("Yedek Geri Yüklendi.");
                //yedeği sil
                if (File.Exists(yer2))
                    File.Delete(yer2);

                //DB.ExecuteSQL("update Satislar set Aktarildi=1");
                //DB.ExecuteSQL("update KasaHareket set Aktarildi=1");
                //DB.ExecuteSQL("update StokKarti set StokKarti_id=pkStokKarti where StokKarti_id is null");
                //DB.ExecuteSQL("update Firmalar set Firma_id=pkFirma where Firma_id is null");

                listBoxControl2.Items.Add("İşlem Tamamlandı.");

                MessageBox.Show("İşlem Tamamlandı.");
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Yedek Geri Alınırken Hata Oluştur.\nHata Mesajı: " + sonuc, Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show("StokKarti ve Firmalar Firma_id=pkFirma ve StokKarti_id=pkStokKarti yapılsın mı?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.No) return;

            //DB.ExecuteSQL("update Satislar set Aktarildi=1");
            //DB.ExecuteSQL("update KasaHareket set Aktarildi=1");

            DB.ExecuteSQL("update StokKarti set StokKarti_id=pkStokKarti where StokKarti_id is null");
            DB.ExecuteSQL("update Firmalar set Firma_id=pkFirma where Firma_id is null");

            //MessageBox.Show("satış ve kasa Firma_id=pkFirma ve StokKarti_id=pkStokKarti  ");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            simpleButton7.Visible = true;
            sbtnSatislar.Visible = true;
            simpleButton9.Visible = true;
            simpleButton3.Visible = true;
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            //DB.ExecuteSQL("update Sirketler set yedek_alinacak_yer_paylasimli='" +txtpaylasimli_yedek_yol.Text+"'");
            //DB.ExecuteSQL("update Sirketler set yedekalinacakyer='" + txtyedekyeri.Text + "'");
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.Title="Yedek Dosyasını Seçiniz";
                openFileDialog1.InitialDirectory = "C:\\data";
                txtYedekDosyasi.Text = openFileDialog1.FileName;
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            if (gridView6.DataRowCount == 0) return;

            //simpleButton3.Enabled = false;
            //müşteriden alınan tahsilatları gönderir. satış tahsilatları hariç
            //listBoxControl1.Items.Clear();

            DataTable dt = DB.GetData(@"select * from KasaHareket kh with(nolock)
                inner join Firmalar f with(nolock) on f.pkFirma=kh.fkFirma
                where fkSatislar is null  and kh.Aktarildi=0");

            //OdemeSekli not in('Kasa Bakiye Düzeltme','Bakiye Düzeltme') and 
            int hatali = 0, hatasiz = 0, c = dt.Rows.Count;

            if (c == 0) return;
            //uzak
            //if (DBWeb.conTrans == null)
            //    DBWeb.conTrans = new SqlConnection(DBWeb.ConnectionString());

            //if (DBWeb.conTrans.State == ConnectionState.Closed)
            //    DBWeb.conTrans.Open();

            //local
            //if (DB.conTrans == null)
            //    DB.conTrans = new SqlConnection(DB.ConnectionString());

            //if (DB.conTrans.State == ConnectionState.Closed)
            //    DB.conTrans.Open();

            foreach (DataRow dr3 in dt.Rows)
            {
                Application.DoEvents();

                #region trans başlat

                //DBWeb.transaction = DBWeb.conTrans.BeginTransaction("AdemTransaction");
                //DB.transaction = DB.conTrans.BeginTransaction("AktarimTransaction");

                #endregion

                string fkKasaHareket = dr3["pkKasaHareket"].ToString();
                //DataTable dtK = DBWeb.GetData_Web("select pkKasaHareket from KasaHareket with(nolock) where aktarim_fis_no=" + fkKasaHareket);
                //if (dtK.Rows.Count > 0)
                //{
                //    listBoxControl1.Items.Add("Kasa Hareket Tahsilat Daha Önce Kaydedildi" + fkKasaHareket + " Hata:");
                //    continue;
                //}
                string fkKasalar = dr3["fkKasalar"].ToString();
                string Tarih2 = dr3["Tarih"].ToString();
                string Tutar = dr3["Tutar"].ToString();
                string Borc = dr3["Borc"].ToString();
                string Alacak = dr3["Alacak"].ToString();
                string Aciklama3 = dr3["Aciklama"].ToString();
                //string fkFirma3 = dr3["fkFirma"].ToString();
                string Firma_id = dr3["Firma_id"].ToString();//ana bilgisayardaki pkFirma=firma_id
                string fkTuru = dr3["fkTuru"].ToString();
                string OdemeSekli3 = dr3["OdemeSekli"].ToString();
                string fkBankalar = dr3["fkBankalar"].ToString();
                string AktifHesap = dr3["AktifHesap"].ToString();
                string fkTedarikciler = dr3["fkTedarikciler"].ToString();
                string fkAlislar = dr3["fkAlislar"].ToString();
                string fkKasaGirisCikisTurleri = dr3["fkKasaGirisCikisTurleri"].ToString();

                string Bakiye = dr3["Bakiye"].ToString();
                if (Bakiye == "") Bakiye = "0";

                string BilgisayarAdi = dr3["BilgisayarAdi"].ToString();
                string fkCek = dr3["fkCek"].ToString();
                string fkKullanicilar = dr3["fkKullanicilar"].ToString();

                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@fkKasalar", fkKasalar));
                list2.Add(new SqlParameter("@Tarih", Convert.ToDateTime(Tarih2)));
                list2.Add(new SqlParameter("@Tutar", Tutar.Replace(",", ".")));
                list2.Add(new SqlParameter("@Aciklama", Aciklama3));
                list2.Add(new SqlParameter("@Borc", Borc.Replace(",", ".")));
                list2.Add(new SqlParameter("@Alacak", Alacak.Replace(",", ".")));
                list2.Add(new SqlParameter("@fkFirma", Firma_id));//fkFirma3)); //ana bilgisayardaki pkFirma=firma_id
                list2.Add(new SqlParameter("@fkTuru", fkTuru));
                list2.Add(new SqlParameter("@OdemeSekli", OdemeSekli3));

                if (fkBankalar == "")
                    list2.Add(new SqlParameter("@fkBankalar", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkBankalar", fkBankalar));

                list2.Add(new SqlParameter("@AktifHesap", AktifHesap));

                if (fkTedarikciler == "")
                    list2.Add(new SqlParameter("@fkTedarikciler", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkTedarikciler", fkTedarikciler));

                if (fkAlislar == "")
                    list2.Add(new SqlParameter("@fkAlislar", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@fkAlislar", fkAlislar));

                list2.Add(new SqlParameter("@fkKasaGirisCikisTurleri", fkKasaGirisCikisTurleri));
                list2.Add(new SqlParameter("@Bakiye", Bakiye.Replace(",", ".")));
                list2.Add(new SqlParameter("@BilgisayarAdi", "Aktarım"));//BilgisayarAdi));
                list2.Add(new SqlParameter("@fkCek", fkCek));
                list2.Add(new SqlParameter("@fkKullanicilar", fkKullanicilar));
                list2.Add(new SqlParameter("@aktarim_fis_no", fkKasaHareket));

                string sql = "INSERT INTO KasaHareket (fkKasalar,Tarih,Tutar,Aciklama,Borc,Alacak,fkFirma,fkTuru,OdemeSekli,fkBankalar,AktifHesap,fkTedarikciler,fkAlislar,fkKasaGirisCikisTurleri,Bakiye,BilgisayarAdi,fkCek,fkKullanicilar,Aktarildi,aktarim_fis_no)" +
                    " values(@fkKasalar,@Tarih,@Tutar,@Aciklama,@Borc,@Alacak,@fkFirma,@fkTuru,@OdemeSekli,@fkBankalar,@AktifHesap,@fkTedarikciler,@fkAlislar,@fkKasaGirisCikisTurleri,@Bakiye,@BilgisayarAdi,@fkCek,@fkKullanicilar,1,@aktarim_fis_no)";

                bool islembasarili = true;
                //string sonuc = 
                DBWeb.ExecuteSQL_Web(sql, list2);
                //bool islembasarili=false;
                //if (sonuc.Substring(0, 1) == "H")
                //    islembasarili = false;


                #region sonuç başarılı ise aktarılanı sil
                if (islembasarili)
                {
                    //localden sil  
                    //bir kayıt silindi ise 2
                    //hiçbir kayıt silindi ise 0 
                    string s;
                    DB.ExecuteSQL("delete from KasaHareket where pkKasaHareket=" + fkKasaHareket);
                    //if (s != "0")
                    {
                        //locak sunucuyu kapat
                        //DB.transaction.Rollback();
                        //DB.conTrans.Close();
                        //uzak sunucuyu kapat
                        //DBWeb.transaction.Rollback();
                        //DBWeb.conTrans.Close();
                        //continue;
                        islembasarili = false;
                    }
                }
                #endregion

                #region trans. işlemi
                if (islembasarili)
                {
                    //local sunucu
                    //DB.transaction.Commit();
                    //DB.conTrans.Close();
                    //uzak sunucuyu
                    //DBWeb.transaction.Commit();
                    hatasiz++;
                    //listBoxControl1.Items.Add("Kasa Id: " + fkKasaHareket + " Başarılı");
                }
                else
                {
                    //locak sunucuyuyu geri al
                    //DB.transaction.Rollback();
                    //uzak sunucuyu geri al
                    //DBWeb.transaction.Rollback();
                    hatali++;
                    listBoxControl1.Items.Add("Kasa Id: " + fkKasaHareket + " Hatalı");
                }
                #endregion

                lblTahsilat.Text = "Tahsilat Sayısı:" + c.ToString() + "-" + hatali.ToString() + " Hatalı / " + hatasiz.ToString() + " Hatasız";
            }
            //DB.conTrans.Close();
            //DBWeb.conTrans.Close();
            //BakiyeGuncelle(); kasa kahareketi göndermek gerekiyor
            KasaHareketBuBilgisayardaki();
        }

        private void lblTahsilat_Click(object sender, EventArgs e)
        {
            simpleButton18.Visible = true;
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            string sql = "RESTORE DATABASE " + txtVeritanabiAdi.Text;
            sql = sql + " FROM DISK = '" + txtYedekDosyasi.Text + "'";
            sql = sql + " with move 'akincilar' to '" + txtGeriyukleYol.Text + "\\" + txtVeritanabiAdi.Text + ".mdf',";
            sql = sql + " move 'akincilar_log' to '" + txtGeriyukleYol.Text + "\\" + txtVeritanabiAdi.Text + ".ldf',";
            sql = sql + " replace";
            int sonu = DB.ExecuteSQLSa(sql);

            MessageBox.Show("sonuç: " + sonu.ToString());

            try
            {
                MessageBox.Show(DB.GetData("select Sirket From " + txtVeritanabiAdi.Text + ".dbo.Sirketler").Rows[0][0].ToString());
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
                //throw;
            }
            
        }
    }
}
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
    public partial class frmAlisOdeme : DevExpress.XtraEditors.XtraForm
    {
        string _AlisTipi = "2";
        public frmAlisOdeme(DateTime alistarihi,string AlisTipi)
        {
            InitializeComponent();
            
            deAlisTarihi.DateTime = alistarihi;
            _AlisTipi = AlisTipi;
            this.Top = 150;
            this.Left = 320;
        }

        private void frmSatisOdeme_Load(object sender, EventArgs e)
        {
            System.Drawing.Font f = new System.Drawing.Font("Arial", 26);
            
            ceNakit.Font = f;
            ceKrediKarti.Font = f;
            //DataTable dtAlislar =  DB.GetData("select * from Alislar with(nolock)where pkAlislar=" + pkAlislar.Text);
            //if (dtAlislar.Rows.Count == 0)
            //{
            //    formislemleri.Mesajform("Fiş Bulunamadı", "K");
            //    this.Tag = (int)Degerler.islemDurumu.Bulunamadi;
            //    Close();
            //}
            //if (dtAlislar.Rows[0]["Siparis"].ToString() == "True")
            //{
            //    formislemleri.Mesajform("Fiş Daha Önce Kaydedildi", "K");
            //    this.Tag = (int)Degerler.islemDurumu.Bulunamadi;
            //    Close();
            //}
            Yetkiler();

            //DataTable dts = DB.GetData("select top 1 * from Sirketler");
            //if (dts.Rows.Count > 0)
            //{
            //    if (dts.Rows[0]["BonusYuzde"].ToString() == "0")
            //    {
            //       // ceBonus.Visible = false;
            //    }
            //}

            #region tedarikçi bilgileri
            DataTable dtp = DB.GetData(@"select pkTedarikciler,OzelKod,Firmaadi,Eposta,isnull(dbo.fon_TedarikciBakiyesi(pkTedarikciler),0) as Bakiye,gider_mi from Tedarikciler with(nolock) 
                                        where pkTedarikciler=" + TedarikciAdi.Tag.ToString());
            if (dtp.Rows.Count > 0)
            {
                TedarikciAdi.Text = dtp.Rows[0]["OzelKod"].ToString() + "-" + dtp.Rows[0]["Firmaadi"].ToString();
                lcBakiye.Text = double.Parse(dtp.Rows[0]["Bakiye"].ToString()).ToString("##0.00");
                //ceBonusKalan.EditValue = dtp.Rows[0]["Bonus"].ToString();

                if (dtp.Rows[0]["gider_mi"].ToString() == "True")
                    cbGider.Checked = true;
                else
                    cbGider.Checked = false;
            }
            #endregion

            lUKasa.Properties.DataSource = DB.GetData("select * from Kasalar with(nolock) where Aktif=1");
            lueKKarti.Properties.DataSource = DB.GetData("Select * from Bankalar with(nolock) where Aktif=1");
            lUKasa.EditValue = Degerler.fkKasalar;
            
//            #region Ödemeleri ve açık hesap getir
//            ceAcikHesapOdenen.Visible = false;
//            lAcikHesapOdenen.Visible = false;
//            if (this.Tag == "1")
//            {
//                DataTable dt = DB.GetData("select 'KH' as T,pkKasaHareket as id,fkKasalar,OdemeSekli,fkBankalar,Borc as Odeme,0 as Odenen from KasaHareket" +
//" where fkSatislar=" + pkAlislar.Text + " union all" +
//" select 'S' as T,pkSatislar as id,0 as fkKasalar,' Açık Hesap' as OdemeSekli,0 as fkBankalar,AcikHesap as Odeme,AcikHesapOdenen as Odenen from Satislar" +
//" where pkAlislar=" + pkAlislar.Text);
//                for (int i = 0; i < dt.Rows.Count; i++)
//                {
//                    if (dt.Rows[i]["OdemeSekli"].ToString() == "Nakit")
//                    {
//                        ceNakit.EditValue = dt.Rows[i]["Odeme"].ToString();
//                        lUKasa.EditValue = int.Parse(dt.Rows[i]["fkKasalar"].ToString());
//                    }
//                    if (dt.Rows[i]["OdemeSekli"].ToString() == "Kredi Kartı")
//                    {
//                        ceKrediKarti.EditValue = dt.Rows[i]["Odeme"].ToString();
//                        lueKKarti.EditValue = int.Parse(dt.Rows[i]["fkBankalar"].ToString());
//                    }
//                    if (dt.Rows[i]["OdemeSekli"].ToString() == " Açık Hesap")
//                    {
//                        ceAcikHesap.EditValue = dt.Rows[i]["Odeme"].ToString();
//                        ceAcikHesapOdenen.Visible = true;
//                        lAcikHesapOdenen.Visible = true;
//                        ceAcikHesapOdenen.EditValue = dt.Rows[i]["Odenen"].ToString();
//                    }
//                }
//            }
//            #endregion
            CekListesi();

            ceNakit.Focus();

            FisBilgileriGetir();
        }
        void FisBilgileriGetir()
        {
            DataTable dtAl=DB.GetData("select * from Alislar with(nolock) where pkAlislar="+pkAlislar.Text);
            if(dtAl.Rows.Count>0)
            {
                lDuzenlemeTarihi.Text = dtAl.Rows[0]["DuzenlemeTarihi"].ToString();
                if (lDuzenlemeTarihi.Text != "")
                {
                    pcEskiler.Visible = true;

                    ceEskiNakit.Text = dtAl.Rows[0]["OdenenNakit"].ToString();
                    ceEskiKrediKarti.Text = dtAl.Rows[0]["OdenenKrediKarti"].ToString();
                    ceEskiAcikHesap.Text = dtAl.Rows[0]["AcikHesap"].ToString();
                    lOdemeSekli.Text = dtAl.Rows[0]["OdemeSekli"].ToString();
                }
            }
        }
        void Yetkiler()
        {
            string sql = @"SELECT   YetkiAlanlari.Yetki, YetkiAlanlari.Sayi, Parametreler.Aciklama10, YetkiAlanlari.BalonGoster
                FROM  YetkiAlanlari INNER JOIN Parametreler ON YetkiAlanlari.fkParametreler = Parametreler.pkParametreler
                WHERE  Parametreler.fkModul = 1  and YetkiAlanlari.fkKullanicilar =" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "OdemeYaz")
                    btyazdir.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);

                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
                //  gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);

                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "GizliAlan")
                //{
                //if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                //    dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                //else
                //        dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //}
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "SayfaYetki")
                //{
                //    if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                //    {
                //        gridControl5.DataSource = DB.GetData("select * from Parametreler where fkModul=1 order by SiraNo");
                //        dockPanel3.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                //    }
                //    else
                //        dockPanel3.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //}
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
                //{
                //    if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                //    {
                //        timer1.Enabled = true;
                //        //gridControl5.DataSource = DB.GetData("select * from Parametreler where fkModul=1 order by SiraNo");
                //        dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                //    }
                //    else
                //        dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //}
            }
        }
        void Hesapla()
        {
            decimal dcek =0;
            if(lueCekler.EditValue!=null)
                dcek = decimal.Parse(lueCekler.Text.Replace(".", ","));
            ceAcikHesap.Value = satistutari.Value - (ceKrediKarti.Value + ceNakit.Value + dcek);
        }
        private void ceKrediKarti_EditValueChanged(object sender, EventArgs e)
        {
            string kk=ceKrediKarti.EditValue.ToString().Replace(".",",");

            lueKKarti.Visible=true;
            lueKKarti.ItemIndex = 0;
             Hesapla();
        }

        private void ceNakit_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ceNakit.EditValue.ToString()) || ceNakit.EditValue.ToString() == "0")
                lUKasa.Visible = false;
            else
                lUKasa.Visible = true;

            if (ceNakit.Tag.ToString() == "1")
                Hesapla();
        }

        private bool kaydetyazidir(string kaydet_yazir)
        {
            #region Uyarılar
            if (ceNakit.Value > 0 && lUKasa.EditValue == null)
            {
                MessageBox.Show("Kasa Seçiniz!");
                lUKasa.Focus();
                return false;
            }
            if (ceKrediKarti.Value>0 && lueKKarti.EditValue == null)
            {
                MessageBox.Show("K.Kartı Seçiniz!");
                lueKKarti.Focus();
                return false;
            }
            if (ceAcikHesap.Value > 0 && TedarikciAdi.Tag.ToString() == "1")
            {
                MessageBox.Show("1 Nolu Tedarikçiye Açık Hesap Olamaz.");
                ceNakit.Focus();
                return false;
            }
            if(cbOdemeAlindi.Checked==false)
            {
               DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Ödeme Aktif Olarak Kasaya İşlenmeyecek Eminmisiniz?", "Hitit2012", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No)         
                    return false;
            }
            #endregion

            #region Eski Bakiye
            string EskiBakiye =
            DB.GetData("select isnull(dbo.fon_TedarikciBakiyesi(" + TedarikciAdi.Tag.ToString() + "),0) as OncekiBakiye").Rows[0][0].ToString();
            EskiBakiye = EskiBakiye.Replace(",", ".");
            #endregion

            #region alisbilgileri
            //DataTable dtAlis =
            //DB.GetData("select * from Alislar with(nolock) where pkAlislar="+ pkAlislar.Text);
            #endregion
            if (_AlisTipi != "1" & _AlisTipi != "9" & _AlisTipi != "12")
            {
                #region Nakit ÖDENEN
                if (ceNakit.Value != 0)
                {
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkTedarikciler", TedarikciAdi.Tag.ToString()));
                    if (ceNakit.Value > 0)
                    {
                        list.Add(new SqlParameter("@Alacak", ceNakit.Value.ToString().Replace(",", ".")));
                        list.Add(new SqlParameter("@Borc", "0"));
                    }
                    if (ceNakit.Value < 0)
                    {
                        list.Add(new SqlParameter("@Alacak", "0"));
                        list.Add(new SqlParameter("@Borc", ceNakit.Value.ToString().Replace(",", ".").Replace("-", "")));
                    }
                    if (cbOdemeAlindi.Checked == false)
                        list.Add(new SqlParameter("@fkKasalar", DBNull.Value));
                    else
                        list.Add(new SqlParameter("@fkKasalar", lUKasa.EditValue.ToString()));
                    //if (cbOdemeAlindi.Checked == false)
                    //  list.Add(new SqlParameter("@Aciklama", "Pasif Ödeme"));
                    //else
                    list.Add(new SqlParameter("@Aciklama", txtAciklama.Text));
                    list.Add(new SqlParameter("@fkAlislar", pkAlislar.Text));
                    list.Add(new SqlParameter("@Tutar", EskiBakiye.Replace(",", ".")));
                    list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

                    if (cbOdemeAlindi.Checked == false)
                        list.Add(new SqlParameter("@AktifHesap", "0"));
                    else
                        list.Add(new SqlParameter("@AktifHesap", "1"));

                    list.Add(new SqlParameter("@Bakiye", "0"));
                    list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                    list.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
                    list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                    if (cbGider.Checked == false)
                        list.Add(new SqlParameter("@GiderOlarakisle", "0"));
                    else
                        list.Add(new SqlParameter("@GiderOlarakisle", "1"));

                    list.Add(new SqlParameter("@Tarih", deAlisTarihi.DateTime));

                    string sonuc = DB.ExecuteSQLTrans("INSERT INTO KasaHareket (fkKasalar,fkTedarikciler,Tarih,Modul,Tipi,Borc,Alacak," +
                        "AktifHesap,Aciklama,OdemeSekli,fkAlislar,Tutar,fkKullanicilar,GelirOlarakisle,GiderOlarakisle,Bakiye,BilgisayarAdi," +
                        "aktarildi,fkSube)" +
                        " values(@fkKasalar,@fkTedarikciler,@Tarih,5,3,@Borc,@Alacak," +
                        "@AktifHesap,@Aciklama,'Nakit',@fkAlislar,@Tutar,@fkKullanicilar,0,@GiderOlarakisle,@Bakiye,@BilgisayarAdi," +
                        "@aktarildi,@fkSube)", list);
                    if (sonuc.Substring(0, 1) == "H")
                    {
                        //DB.ExecuteSQL("delete from Alislar where fkAlislar=" + pkAlislar.Text);
                        DB.trans_basarili = false;
                        DB.trans_hata_mesaji = DB.trans_hata_mesaji + sonuc;
                        return false;
                        //return ((int)Degerler.islemDurumu.Basarisiz).ToString();
                    }
                    //DB.ExecuteSQL("UPDATE Tedarikciler SET Alacak=Alacak+" + ceNakit.Value.ToString().Replace(",", ".") + " where pkTedarikciler=" + TedarikciAdi.Tag.ToString());
                }
                #endregion

                #region kredi kartı ödemesi
                if (ceKrediKarti.Value != 0)
                {
                    ArrayList list2 = new ArrayList();
                    list2.Add(new SqlParameter("@fkTedarikciler", TedarikciAdi.Tag.ToString()));
                    if (ceKrediKarti.Value > 0)
                    {
                        list2.Add(new SqlParameter("@Alacak", ceKrediKarti.Value.ToString().Replace(",", ".")));
                        list2.Add(new SqlParameter("@Borc", "0"));
                    }
                    if (ceKrediKarti.Value < 0)
                    {
                        list2.Add(new SqlParameter("@Alacak", "0"));
                        list2.Add(new SqlParameter("@Borc", ceKrediKarti.Value.ToString().Replace(",", ".").Replace("-", "")));
                    }
                    if (cbOdemeAlindi.Checked == false)
                        list2.Add(new SqlParameter("@fkBankalar", DBNull.Value));
                    else
                        list2.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue.ToString()));

                    //if (cbOdemeAlindi.Checked == false)
                    //    list2.Add(new SqlParameter("@Aciklama", "Pasif Ödeme"));
                    //else
                    list2.Add(new SqlParameter("@Aciklama", txtAciklama.Text));
                    list2.Add(new SqlParameter("@fkAlislar", pkAlislar.Text));
                    list2.Add(new SqlParameter("@Tutar", EskiBakiye));

                    if (cbOdemeAlindi.Checked == false)
                        list2.Add(new SqlParameter("@AktifHesap", "0"));
                    else
                        list2.Add(new SqlParameter("@AktifHesap", "1"));

                    list2.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

                    //if(lcBakiye.Text=="")
                    list2.Add(new SqlParameter("@Bakiye", "0"));
                    //else
                    //  list2.Add(new SqlParameter("@Bakiye", lcBakiye.Text.Replace(",",".")));

                    list2.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                    list2.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));

                    if (cbGider.Checked == false)
                        list2.Add(new SqlParameter("@GiderOlarakisle", "0"));
                    else
                        list2.Add(new SqlParameter("@GiderOlarakisle", "1"));

                    list2.Add(new SqlParameter("@fkSube", Degerler.fkSube));
                    list2.Add(new SqlParameter("@Tarih", deAlisTarihi.DateTime));
                    string sonuc = DB.ExecuteSQLTrans("INSERT INTO KasaHareket (fkBankalar,fkTedarikciler,Tarih,Modul,Tipi,Borc,Alacak," +
                        "Aciklama,OdemeSekli,fkAlislar,Tutar,AktifHesap,fkKullanicilar,GelirOlarakisle,GiderOlarakisle,Bakiye," +
                        "BilgisayarAdi,aktarildi,fkSube)" +
                        " values(@fkBankalar,@fkTedarikciler,@Tarih,8,1,@Borc,@Alacak," +
                        "@Aciklama,'Kredi Kartı',@fkAlislar,@Tutar,@AktifHesap,@fkKullanicilar,0,@GiderOlarakisle,@Bakiye," +
                        "@BilgisayarAdi,@aktarildi,@fkSube)", list2);

                    if (sonuc.Substring(0, 1) == "H")
                    {
                        DB.trans_basarili = false;
                        DB.trans_hata_mesaji = DB.trans_hata_mesaji + sonuc;
                        return false;
                        //DB.ExecuteSQL("delete from Alislar where fkAlislar=" + pkAlislar.Text);
                        //return ((int)Degerler.islemDurumu.Basarisiz).ToString();
                    }
                    //DB.ExecuteSQL("UPDATE Alislar SET OdenenKrediKarti=" + ceKrediKarti.Value.ToString().Replace(",", ".") + " where pkAlislar=" + pkAlislar.Text);

                    //DB.ExecuteSQLTrans("UPDATE Tedarikciler SET Alacak=Alacak+" + ceKrediKarti.Value.ToString().Replace(",", ".") + " where pkTedarikciler=" + TedarikciAdi.Tag.ToString());
                }
                #endregion

                #region çek ödemesi
                if (lueCekler.EditValue != null)
                {
                    DB.ExecuteSQLTrans("update Cekler set fkCekTuru=2,fkTedarikci=" + TedarikciAdi.Tag.ToString() + " where pkCek=" + lueCekler.EditValue.ToString());
                    //MessageBox.Show("Çek Modülü Yapım Aşamasındadır.");
                    ArrayList list2 = new ArrayList();
                    list2.Add(new SqlParameter("@fkTedarikciler", TedarikciAdi.Tag.ToString()));
                    //if (ceKrediKarti.Value > 0)
                    //{
                    list2.Add(new SqlParameter("@Alacak", lueCekler.Text.Replace(",", ".")));
                    list2.Add(new SqlParameter("@Borc", "0"));
                    // }
                    //if (ceKrediKarti.Value < 0)
                    //{
                    //    list2.Add(new SqlParameter("@Alacak", "0"));
                    //    list2.Add(new SqlParameter("@Borc", ceKrediKarti.Value.ToString().Replace(",", ".").Replace("-", "")));
                    //}

                    list2.Add(new SqlParameter("@fkBankalar", DBNull.Value));
                    //if (cbOdemeAlindi.Checked == false)
                    //    list2.Add(new SqlParameter("@Aciklama", "Pasif Ödeme"));
                    //else
                    list2.Add(new SqlParameter("@Aciklama", txtAciklama.Text));
                    list2.Add(new SqlParameter("@fkAlislar", pkAlislar.Text));
                    list2.Add(new SqlParameter("@Tutar", EskiBakiye));

                    if (cbOdemeAlindi.Checked == false)
                        list2.Add(new SqlParameter("@AktifHesap", "0"));
                    else
                        list2.Add(new SqlParameter("@AktifHesap", "1"));

                    list2.Add(new SqlParameter("@fkCek", lueCekler.EditValue.ToString()));

                    list2.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                    list2.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));

                    if (cbGider.Checked == false)
                        list2.Add(new SqlParameter("@GiderOlarakisle", "0"));
                    else
                        list2.Add(new SqlParameter("@GiderOlarakisle", "1"));

                    list2.Add(new SqlParameter("@fkSube", Degerler.fkSube));
                    list2.Add(new SqlParameter("@Tarih", deAlisTarihi.DateTime));

                    string sonuc = DB.ExecuteSQLTrans("INSERT INTO KasaHareket (fkBankalar,fkTedarikciler,Tarih,Modul,Tipi,Borc,Alacak," +
                        "Aciklama,OdemeSekli,fkAlislar,Tutar,AktifHesap,fkCek,fkKullanicilar,GelirOlarakisle,GiderOlarakisle,aktarildi,fkSube)" +
                        " values(@fkBankalar,@fkTedarikciler,@Tarih,8,1,@Borc,@Alacak," +
                        "@Aciklama,'Çek Ödemesi',@fkAlislar,@Tutar,@AktifHesap,@fkCek,@fkKullanicilar,0,@GiderOlarakisle,@aktarildi,@fkSube)", list2);

                    if (sonuc.Substring(0, 1) == "H")
                    {
                        DB.trans_basarili = false;
                        DB.trans_hata_mesaji = DB.trans_hata_mesaji + sonuc;
                        return false;
                        //DB.ExecuteSQL("delete from Alislar where fkAlislar=" + pkAlislar.Text);
                        //return ((int)Degerler.islemDurumu.Basarisiz).ToString();
                    }
                    //DB.ExecuteSQL("UPDATE Alislar SET OdenenCek=" + lueCekler.Text.Replace(",", ".") + " where pkAlislar=" + pkAlislar.Text);

                    //DB.ExecuteSQL("UPDATE Tedarikciler SET Alacak=Alacak+" + lueCekler.Text.Replace(",", ".") + " where pkTedarikciler=" + TedarikciAdi.Tag.ToString());

                }
                #endregion

                #region açık hesap ekle
                //if (ceAcikHesap.EditValue != null && ceAcikHesap.Text != "0" && ceAcikHesap.Text != "" && ceAcikHesap.Text != "0,00")
                //{
                //    ArrayList list3 = new ArrayList();
                //    list3.Add(new SqlParameter("@AcikHesap", ceAcikHesap.EditValue.ToString().Replace(",",".")));
                //    list3.Add(new SqlParameter("@fkAlislar", pkAlislar.Text));
                //    //DB.ExecuteSQL("UPDATE Alislar SET AcikHesap=@AcikHesap where pkAlislar=@fkAlislar", list3);
                //    DB.ExecuteSQLTrans("UPDATE Tedarikciler SET Borc=Borc+" + ceAcikHesap.EditValue.ToString().Replace(",", ".") + " where pkTedarikciler=" + TedarikciAdi.Tag.ToString());
                //}
                #endregion
            }
            //this.Tag = (int)Degerler.islemDurumu.Basarili;
            kaydetmiyazdirmi.Text = kaydet_yazir;

            DB.ExecuteSQLTrans("UPDATE Alislar SET OdenenNakit=" + ceNakit.Value.ToString().Replace(",", ".") +
                ",OdenenKrediKarti=" + ceKrediKarti.Value.ToString().Replace(",", ".") +
                ",OdenenCek=0" +
                ",AcikHesap=" + ceAcikHesap.Value.ToString().Replace(",", ".") +
                " where pkAlislar=" + pkAlislar.Text);

            OdemeSekliKaydet();

            DB.ExecuteSQL("update Tedarikciler set Devir=Devir+" + ceAcikHesap.Value.ToString().Replace(",", ".") + " where pkTedarikciler=" + TedarikciAdi.Tag.ToString());

            return DB.trans_basarili;
        }

        void OdemeSekliKaydet()
        {
            string OdemeSekli = "Nakit";

            if (ceNakit.Value == satistutari.Value)
                OdemeSekli = "Nakit";
            else if (ceKrediKarti.Value == satistutari.Value)
                OdemeSekli = "Kredi Kartı";
            else if (ceAcikHesap.Value == satistutari.Value)
                OdemeSekli = "Açık Hesap";
            else
                OdemeSekli = "Diğer...";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkAlislar", pkAlislar.Text));
            list.Add(new SqlParameter("@OdemeSekli", OdemeSekli));
            
            DB.ExecuteSQLTrans("UPDATE Alislar SET OdemeSekli=@OdemeSekli where pkAlislar=@fkAlislar", list);
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            DB.trans_basarili = kaydetyazidir("kaydet");
             Close();
        }

        private void ceAcikHesap_EditValueChanged(object sender, EventArgs e)
        {
            if (ceAcikHesap.Tag.ToString() == "1")
                Hesapla();
            if (ceAcikHesap.Value < 0)
                ceAcikHesap.BackColor = System.Drawing.Color.Green;
            else
                ceAcikHesap.BackColor = System.Drawing.Color.PaleVioletRed;
            //if (string.IsNullOrEmpty(ceAcikHesap.EditValue.ToString()) || ceAcikHesap.EditValue.ToString() == "0,00")
            //    islemTarihi.Visible = false;
            //else
            //{
            //    islemTarihi.Visible = true;
            //    islemTarihi.DateTime = DateTime.Now.AddDays(30);
            //}
        }

        private void ceNakit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ceKrediKarti.Focus();
        }

        private void ceKrediKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                lueCekler.Focus();
        }

        private void ceCek_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ceAcikHesap.Focus();
          //  if (e.KeyCode == Keys.Enter)
            //    ceSenet.Focus();
        }

        private void ceSenet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ceAcikHesap.Focus();
        }

        private void ceAcikHesap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnKaydet.Focus();
        }

        private void frmSatisOdeme_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                btnKapat_Click(sender, e);
            else if (e.KeyCode == Keys.F9)
                btnKaydet_Click(sender, e);
        }

        private void lUEBanka_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnKaydet.Focus();
        }

        private void islemTarihi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                btnKaydet.Focus();
        }

        void SozlesmeYazdir(bool dizayn)
        {
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Sozlesme.repx");
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string sql = "select * from Taksitler";//"exec personeldurum " + DB.pkPersoneller.ToString();
                rapor.DataSource = DB.GetData(sql);
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowPreview();
            else
                rapor.ShowDesigner();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("27");
            SayfaAyarlari.ShowDialog();
        }

        private void BtnSozlesme_Click(object sender, EventArgs e)
        {
            SozlesmeYazdir(false);
        }

        private void btyazdir_Click(object sender, EventArgs e)
        {
            //SozlesmeYazdir(true);
            if (kaydetyazidir("yazdir") == true)
                Close();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Tag = (int)Degerler.islemDurumu.Basarisiz;
            //this.TopMost = false;
           // if (bt_pkCek.Tag != "0")
             //   DB.ExecuteSQL("DELETE FROM Cekler WHERE pkCek=" + bt_pkCek.Tag.ToString());
            Close();
        }

        private void ceKrediKarti_Leave(object sender, EventArgs e)
        {
                ceKrediKarti.Tag = "0";
            
            if (ceKrediKarti.Value == 0)
            {
                lueKKarti.Visible = false;
            }
            else
            {
                lueKKarti.Visible = true;
            }
        }

        private void satistutari_EditValueChanged(object sender, EventArgs e)
        {
            ceNakit.Value = satistutari.Value;
        }
        void CekListesi()
        {
            lueCekler.Properties.DataSource = DB.GetData(@"select pkCek,isnull(Firmaadi,'Tanımsız') as Firmaadi,Vade,Tutar from Cekler c with(nolock) 
                left join Firmalar f with(nolock)  on c.fkFirma=f.pkFirma
                where fkCekTuru=1 order by pkCek desc");
        }
        private void btnCek_Click(object sender, EventArgs e)
        {
            ceNakit.Value = 0;
            ceKrediKarti.Value = 0;
            ceAcikHesap.Value = 0;

            CekListesi();
            lueCekler.ItemIndex = 0;
        }

        private void btnAcikHesap_Click(object sender, EventArgs e)
        {
            ceAcikHesap.Value = satistutari.Value;
            ceNakit.Value = 0;
            ceKrediKarti.Value = 0;
        }

        private void btnKrediKarti_Click(object sender, EventArgs e)
        {
            ceKrediKarti.Value = satistutari.Value;
            ceNakit.Value = 0;
            ceAcikHesap.Value = 0;

        }

        private void btnNakitOdenen_Click(object sender, EventArgs e)
        {
            ceNakit.Value = satistutari.Value;
            ceKrediKarti.Value=0;
            ceAcikHesap.Value = 0;
        }

        private void ceKrediKarti_Enter(object sender, EventArgs e)
        {
            ceKrediKarti.Tag="1";
        }

        private void ceAcikHesap_Leave(object sender, EventArgs e)
        {
            ceAcikHesap.Tag = "0";
        }

        private void ceAcikHesap_Enter(object sender, EventArgs e)
        {
            ceAcikHesap.Tag = "1";
        }

        private void ceNakit_Enter(object sender, EventArgs e)
        {
            ceNakit.Tag = "1";
        }

        private void ceNakit_Leave(object sender, EventArgs e)
        {
            ceNakit.Tag = "0";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmUcGoster SatisGoster = new frmUcGoster(3,"0");
            SatisGoster.ShowDialog();
        }

        private void bt_pkCek_Click(object sender, EventArgs e)
        {
            frmCekGirisi CekGirisi = new frmCekGirisi("0");//MusteriAdi.Tag.ToString());
            CekGirisi.pkCek.Text = "0";
            CekGirisi.ceCekTutari.EditValue = ceAcikHesap.EditValue;
            CekGirisi.pkFirma.Text = "0";//MusteriAdi.Tag.ToString();
            CekGirisi.ShowDialog();
            bt_pkCek.Tag = CekGirisi.pkCek.Text;

            CekListesi();

            lueCekler.ItemIndex = 0;
        }
    }
}
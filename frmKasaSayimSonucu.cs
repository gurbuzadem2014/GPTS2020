using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using System.IO;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using GPTS.islemler;
using DevExpress.XtraGrid.Views.Grid;

namespace GPTS
{
    public partial class frmKasaSayimSonucu : DevExpress.XtraEditors.XtraForm
    {
        public frmKasaSayimSonucu()
        {
            InitializeComponent();
            //this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            //this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmGunSonuRaporlari_Load(object sender, EventArgs e)
        {

            #region Yetkiler
            DataTable dtYetki = DB.GetData(@"select * from ModullerYetki with(nolock) where Kod like '3%' and fkKullanicilar=" + DB.fkKullanicilar);

            for (int i = 0; i < dtYetki.Rows.Count; i++)
            {
                string Kod = dtYetki.Rows[i]["Kod"].ToString();
                string yetki = dtYetki.Rows[i]["Yetki"].ToString();
                ////gün sonu Raporu
                if (Kod == "3.3" && yetki == "False")
                {
                    //Close();
                    //break;
                }
                else if (Kod == "3.3.1" && yetki == "False")//Gün Sonu Raporu Liste
                {
                    //btnListele.Enabled =  false;
                }
                //else if (Kod == "3.7" && yetki == "False")//Kasa Hareketi Sil
                //{
                //    tsmHareketSil.Enabled = false;
                //}
               
            }
            #endregion

            //ilktarih.Properties.DisplayFormat.FormatString = "f";
            //sontarih.Properties.EditFormat.FormatString = "f";
            //ilktarih.Properties.EditFormat.FormatString = "f";
            //sontarih.Properties.DisplayFormat.FormatString = "f";
            //ilktarih.Properties.EditMask = "f";
            //sontarih.Properties.EditMask = "f";

            sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            //ilktarih.DateTime = DateTime.Today;
            //sontarih.DateTime = DateTime.Today.AddHours(23).AddMinutes(59);

            //kasalar();
            
            deTarih.DateTime = DateTime.Now;

            //lUEKullanicilar.Properties.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where eposta is not null");
            //lUEKullanicilar.ItemIndex = 0;

            KasaBakiye();

            //DataTable dt = DB.GetData("select top 1 DATEADD(MINUTE,1,Tarih) as Tarih from KasaGunluk with(nolock) order by 1 desc");

            //if (dt.Rows.Count > 0)
            //{
            //    string tar= dt.Rows[0]["Tarih"].ToString();
            //    ilktarih.DateTime = Convert.ToDateTime(tar);
            //}

            olmasigereken.Focus();
        }

        //void kasalar()
        //{
        //    lueKasalar.Properties.DataSource = DB.GetData("select 0 as pkKasalar, 'Tümü' as KasaAdi "+
        //        "union all select pkKasalar,KasaAdi from Kasalar with(nolock) where Aktif=1");
        //    lueKasalar.ItemIndex = 0;
        //}
        void KasaBakiye()
        {
            DataTable dtKasa=
            DB.GetData("SELECT sum(Borc)-Sum(Alacak) FROM KasaHareket where isnull(fkKasalar,0)>0");
            kasadaki.Value = 0;
            if (dtKasa.Rows.Count > 0)
            {
                decimal _kasadaki = 0;
                decimal.TryParse(dtKasa.Rows[0][0].ToString(), out _kasadaki);
                kasadaki.Value = _kasadaki;
            }
        }
        //private DataTable GunSonuDB()
        //{
        //    string sql = @"exec sp_GunSonuRaporu @ilktar,@sontar,@kasaid";
        //    ArrayList list = new ArrayList();
        //    list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
        //    list.Add(new SqlParameter("@sontar", sontarih.DateTime));
        //    list.Add(new SqlParameter("@kasaid", lueKasalar.EditValue.ToString()));
        //    return DB.GetData(sql, list);
        //}



        private void sorguTarihAraligi(int g1, int m1, int y1, int h1, int min1, int sec1, Boolean p1,
               int g2, int m2, int y2, int h2, int min2, int sec2, Boolean p2)
        {

            DateTime d1 = DateTime.Now;

            d1 = d1.AddDays(g1);
            d1 = d1.AddMonths(m1);
            d1 = d1.AddYears(y1);
            d1 = d1.AddHours(h1);
            d1 = d1.AddMinutes(min1);
            //ilktarih.DateTime = d1.AddSeconds(sec1);


            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            //sontarih.DateTime = d2.AddSeconds(sec2);

        }



        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.ShowDialog();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.pkSatislar = int.Parse(dr["pkSatislar"].ToString());
        }

       
        

        void OdemeKaydet(string OdemeSekli, string pkFirma, string pkSatislar, string ToplamTutar)
        {
            if (OdemeSekli == "Nakit")
            {
                //Nakit
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", pkFirma));
                list.Add(new SqlParameter("@Borc", ToplamTutar.ToString()));
                list.Add(new SqlParameter("@Aciklama", "Nakit Ödeme"));
                list.Add(new SqlParameter("@fkSatislar", pkSatislar));
                //list.Add(new SqlParameter("@Tarih", guncellemetarihi.DateTime));
                //guncellemetarihi.DateTime = DateTime.Now;
                DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkKasalar,fkFirma,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
                    " values(0,1,@fkFirma,4,1,@Borc,0,1,@Aciklama,'Nakit',@fkSatislar)", list);
                DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + ToplamTutar.ToString() + " where pkFirma=" + pkFirma);
                //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
                //MevcutGuncelle();
                return;
            }
            //kredi kartı ile ödeme yaptıysa
            if (OdemeSekli == "Kredi Kartı")
            {
                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@fkFirma", pkFirma));
                list2.Add(new SqlParameter("@Borc", ToplamTutar.ToString()));
                list2.Add(new SqlParameter("@fkBankalar", "1"));
                list2.Add(new SqlParameter("@Aciklama", "K.Kartı Ödemesi"));
                list2.Add(new SqlParameter("@fkSatislar", pkSatislar));
                DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkFirma,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
                    " values(@fkBankalar,@fkFirma,8,1,@Borc,0,1,@Aciklama,'Kredi Kartı',@fkSatislar)", list2);
                DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + ToplamTutar.ToString() + " where pkFirma=" + pkFirma);
                //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
            }
            //açık hesap
            else //if (OdemeSekli == "Açık Hesap")
            {
                ArrayList list4 = new ArrayList();
                list4.Add(new SqlParameter("@fkSatislar", pkSatislar));
                list4.Add(new SqlParameter("@AcikHesap", ToplamTutar.ToString()));
                DB.ExecuteSQL("UPDATE Satislar SET AcikHesap=@AcikHesap where pkSatislar=@fkSatislar", list4);
                //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
            }
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            DB.pkSatislar = 0;
            Close();
        }

        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));
        }

        public virtual void InitPrintTool(PrintTool tool)
        {
            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);
        }
        PrintingSystem printingSystem = null;
        PrintingSystem Printing
        {
            get
            {
                if (printingSystem == null) printingSystem = new PrintingSystem();
                return printingSystem;
            }
        }
        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            //printableLink.Component = gcGunSonuOzet;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            yazdir();
        }

        private void ucSatislar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {

            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();
            //KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();
            //KasaHareketDuzelt.Tag = this.Tag;
            //KasaHareketDuzelt.ShowDialog();
            //KasaBakiyeDUzeltme();
        }


        private void olmasigereken_EditValueChanged(object sender, EventArgs e)
        {
            fark.Value = olmasigereken.Value -kasadaki.Value ;
        }

        private void btnyazdirfis_Click(object sender, EventArgs e)
        {
      
        }



        private void btnDevirBakiye_Click(object sender, EventArgs e)
        {
            if (olmasigereken.Value == null)
            {
                formislemleri.Mesajform("Kasadaki Parayı giriniz", "K", 200);
                olmasigereken.Focus();
                return;
            }
            
            string pkkasahareket = "0";
            decimal _borc = 0, _alacak = 0, _fark = fark.Value;

            #region kasahareketi

            if (_fark != 0)
            {
                DialogResult secim;
                //secim = DevExpress.XtraEditors.XtraMessageBox.Show(fark.Value.ToString() + " Fark Oluştu.Kasa Düzeltinsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kasa Sayım Sonucu Eklensin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.Yes)
                {
                    string sql2 = @"INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,BilgisayarAdi)
                    values(@fkKasalar,@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Kasa Bakiye Düzeltme',@Tutar,@BilgisayarAdi)  SELECT IDENT_CURRENT('KasaHareket')";

                    ArrayList listk = new ArrayList();
                    listk.Add(new SqlParameter("@fkKasalar", Degerler.fkKasalar));
                    if (_fark > 0)
                    {
                        listk.Add(new SqlParameter("@Borc", _fark.ToString().Replace(",", ".").Replace("-", "")));
                        listk.Add(new SqlParameter("@Alacak", "0"));
                    }
                    else
                    {
                        listk.Add(new SqlParameter("@Borc", "0"));
                        listk.Add(new SqlParameter("@Alacak", _fark.ToString().Replace(",", ".").Replace("-", "")));
                    }
                    listk.Add(new SqlParameter("@Tipi", int.Parse("1")));
                    listk.Add(new SqlParameter("@Aciklama", txtAciklama.Text));
                    listk.Add(new SqlParameter("@donem", deTarih.DateTime.Month));
                    listk.Add(new SqlParameter("@yil", deTarih.DateTime.Year));
                    listk.Add(new SqlParameter("@fkFirma", "0"));
                    listk.Add(new SqlParameter("@AktifHesap", cbAktifHesap.Checked));
                    listk.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                    listk.Add(new SqlParameter("@Tutar", kasadaki.Value.ToString().Replace(",", ".")));

                    listk.Add(new SqlParameter("@Tarih", deTarih.DateTime));
                    listk.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));

                    pkkasahareket = DB.ExecuteScalarSQL(sql2, listk);

                    if (pkkasahareket.Substring(0, 1) == "H")
                    {
                        formislemleri.Mesajform("Hata Oluştu " + pkkasahareket, "K", 200);
                        return;
                    }
                }
                else
                    return;
            }

            #endregion

           
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("fkKasalar", "1"));
            list.Add(new SqlParameter("fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("Tarih", deTarih.DateTime));
            list.Add(new SqlParameter("KasadakiPara", olmasigereken.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("OlmasiGereken", kasadaki.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("Aciklama", txtAciklama.Text));
            list.Add(new SqlParameter("fkKasaHareket", pkkasahareket));

            //DataTable dt = DB.GetData("select * from KasaGunluk with(nolock) where fkKasalar=1 and fkKullanici=" +
            //    DB.fkKullanicilar + " and convert(varchar(10),Tarih,112)='" + deTarih.DateTime.ToString("yyyyMMdd") + "'");
            //if (dt.Rows.Count == 0)
            //{
            string sonuc = DB.ExecuteSQL(@"insert into KasaGunluk (fkKasalar,fkKullanici,Tarih,KasadakiPara,OlmasiGereken,Aciklama,fkKasaHareket) 
            values(@fkKasalar,@fkKullanici,@Tarih,@KasadakiPara,@OlmasiGereken,@Aciklama,@fkKasaHareket)", list);
            if (sonuc.Substring(0, 1) == "H")
            {
                formislemleri.Mesajform("Hata Oluştu " + sonuc, "K", 200);
            }
            else
            {
                formislemleri.Mesajform("Kasa Sayım Sonucu Girilmiştir", "S", 100);
                Close();
            }
            //}
            //            else
            //            {
            //                list.Add(new SqlParameter("pkKasaGunluk", dt.Rows[0]["pkKasaGunluk"]));

            //                DB.ExecuteSQL(@"update KasaGunluk set KasadakiPara=@KasadakiPara,OlmasiGereken=@OlmasiGereken,Aciklama=@Aciklama
            //             where pkKasaGunluk=@pkKasaGunluk", list);
            //            }


            //olmasigereken.Value = 0;
            //fark.Value = 0;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            deTarih.DateTime = DateTime.Now;
        }
    }
}

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
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using DevExpress.XtraReports.UI;

namespace GPTS
{
    public partial class frmGenelDurumEkrani : DevExpress.XtraEditors.XtraForm
    {
        public frmGenelDurumEkrani()
        {
            InitializeComponent();
            //this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            //this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmWebAyarlari_Load(object sender, EventArgs e)
        {
            comboBoxEdit2.SelectedIndex = 0;

            Subeler();

            //xtraTabC.SelectedTabPage = xTabPAyarlar;
            //MusteriBorclariGetir();

            deIlktarih.DateTime = DateTime.Now;
            deSonTarih.DateTime = DateTime.Now.AddDays(1);

            lUEKullanicilar.Properties.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where eposta is not null");
            lUEKullanicilar.ItemIndex = 0;

            Gelirler();
            Giderler();
        }

        private void Subeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData(@"select '' as pkSube,'Tümü' as sube_adi
            union all
            select pkSube, sube_adi from Subeler with(nolock) where Subeler.aktif=1");
            lueSubeler.EditValue = Degerler.fkSube;
        }

        void Gelirler()
        {
            string sql = @"hsp_Gelirler @ilktar,@sontar,@fkSube,@acikhesap";
            
            ArrayList list = new ArrayList();

            list.Add(new SqlParameter("@ilktar", deBasGelir.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", deBitGelir.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));

            if(cbAcikhesap.Checked)
                list.Add(new SqlParameter("@acikhesap", "1"));
            else
                list.Add(new SqlParameter("@acikhesap", "0"));

            gcGelirler.DataSource = DB.GetData(sql,list);
        }

        
        void Giderler()
        {
            string sql = @"hsp_Giderler @ilktar,@sontar,@fkSube,@acikhesap";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", deBasGelir.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", deBitGelir.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", "1"));
            if (cbAcikhesap.Checked)
                list.Add(new SqlParameter("@acikhesap", "1"));
            else
                list.Add(new SqlParameter("@acikhesap", "0"));
            gcGiderler.DataSource = DB.GetData(sql, list);
        }
        //        private void SatisDetayWebeGonder()
        //        {
        //            //-- null ise insert 0 ise güncelle 1 ise gönderildi
        //            DataTable dt = DB.GetData("select top 100 * from SatisDetay with(nolock)  where GonderildiWS is null ");

        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                ArrayList list = new ArrayList();
        //                list.Add(new SqlParameter("@fkSatislar", dt.Rows[i]["fkSatislar"].ToString()));
        //                list.Add(new SqlParameter("@Tarih", Convert.ToDateTime(dt.Rows[i]["Tarih"].ToString())));
        //                list.Add(new SqlParameter("@fkStokKarti", dt.Rows[i]["fkStokKarti"].ToString()));
        //                list.Add(new SqlParameter("@Adet", dt.Rows[i]["Adet"].ToString()));
        //                list.Add(new SqlParameter("@SatisFiyati", dt.Rows[i]["SatisFiyati"].ToString().Replace(",",".")));

        //                string sql = @"insert into SatisDetay (Tarih,fkSatislar,fkStokKarti,Adet,SatisFiyati)
        //                    values(@Tarih,@fkSatislar,@fkStokKarti,@Adet,@SatisFiyati)";

        //                string sonuc = DBWeb.ExecuteSQL_Web(sql, list);
        //                if (sonuc == "0")
        //                    DB.ExecuteSQL("update SatisDetay set GonderildiWS=1 where pkSatisDetay=" + dt.Rows[i]["pkSatisDetay"].ToString());
        //            }

        //        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xTabPOnay)
            {
                MusteriBorclariGetir();
            }
        }

        void MusteriBorclariGetir()
        {
            DataTable dtBakiye = DB.GetData("select sum(Devir) as Devir from Firmalar with(nolock)");

            decimal bakiye=0;
            if (dtBakiye.Rows.Count == 0) 
                bakiye=0;
            else
                bakiye= decimal.Parse(dtBakiye.Rows[0]["Devir"].ToString());

            ceToplam.Value = bakiye;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            gridControl2.DataSource = DB.GetData(@"SELECT
convert(varchar(10), s.GuncellemeTarihi, 102) as Tarih,
s.OdemeSekli,
--sum(sd.Adet * (sd.SatisFiyati)) as Tutar,
sdu.Durumu,
sum(sd.Adet * (sd.SatisFiyati - sd.AlisFiyati - sd.iskontotutar - isnull(sd.TevkifatTutari, 0))) as Kar,
sum(sd.Adet * sd.iskontotutar) as iskontotutar,
sum(s.ToplamTutar) as ToplamTutar

FROM Satislar s with(nolock)
inner join SatisDetay sd with(nolock) ON sd.fkSatislar = s.pkSatislar
left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu = s.fkSatisDurumu
left join Kullanicilar k with(nolock) on k.pkKullanicilar = s.fkKullanici
where s.Siparis = 1 and s.fkSatisDurumu not in (1, 10, 11)
and s.GuncellemeTarihi Between '"+deIlktarih.DateTime.ToString("yyyy-MM-dd 00:00")+"'"+ "and '"+deSonTarih.DateTime.ToString("yyyy-MM-dd 23:59")+"'"+
" group by convert(varchar(10), s.GuncellemeTarihi, 102),sdu.Durumu,s.Aciklama,s.OdemeSekli order by 1");
            
            gridView2.ViewCaption = "Satış Özet Raporu "+deIlktarih.DateTime.ToString("yyyy-MM-dd 00:00") + "-" +
                deSonTarih.DateTime.ToString("yyyy-MM-dd 23:59");
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            DataTable dtMail = DB.GetData("select Sirket,eposta from Sirketler with(nolock)");
            if (dtMail.Rows.Count == 0)
            {
                formislemleri.Mesajform("Şirket Bilgilerini kontrol ediniz!", "K", 150);
                return;
            }
            string mail = "";//dtMail.Rows[0]["eposta"].ToString();
            string sirket = dtMail.Rows[0]["Sirket"].ToString();
            //string mail = formislemleri.inputbox("E-Posta Adresi Giriniz", "Gönderilecek E-Posta", "gurbuzadem@gmail.com", false);
            mail = lUEKullanicilar.EditValue.ToString();
            if (mail.Length < 5)
            {
                formislemleri.Mesajform("e-posta adresi eksik veya hatalı!", "K", 150);
                return;
            }
            if (formislemleri.MesajBox("Satış Özet Raporu Gönderilsin mi?", mail, 3, 1) == "0") return;

            string dosyaadi = Application.StartupPath + "\\OzetRapor" + deIlktarih.DateTime.ToString("yyMMddHHmmss") + ".Xls";
            //gridview.ExportToXls(dosyaadi);
            gridView2.ExportToXls(dosyaadi);


            DB.epostagonder(mail, "Özet Rapor -" +
                deIlktarih.DateTime.ToString("dd MMMM yy dddd")+"-"+ deSonTarih.DateTime.ToString("dd MMMM yy dddd")
                , dosyaadi, sirket);
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnGelirGider_Click(object sender, EventArgs e)
        {
            Gelirler();
            Giderler();

            farkbul();
        }

        void farkbul()
        {
            decimal aratopgelir = 0;
            if (gridColumn18.SummaryItem.SummaryValue == null)
                aratopgelir = 0;
            else
                aratopgelir = decimal.Parse(gridColumn18.SummaryItem.SummaryValue.ToString());

            decimal aratopgider = 0;
            if (gridColumn23.SummaryItem.SummaryValue == null)
                aratopgider = 0;
            else
                aratopgider = decimal.Parse(gridColumn23.SummaryItem.SummaryValue.ToString());
            

            ceFark.EditValue = aratopgelir- aratopgider;
        }

        private DateTime getStartOfWeek(bool useSunday)
        {
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;

            if (!useSunday)
                dayOfWeek--;

            if (dayOfWeek < 0)
            {// day of week is Sunday and we want to use Monday as the start of the week
                // Sunday is now the seventh day of the week
                dayOfWeek = 6;
            }

            return now.AddDays(-1 * (double)dayOfWeek);
        }

        private void sorguTarihAraligi(int g1, int m1, int y1, int h1, int min1, int sec1, Boolean p1,
                       int g2, int m2, int y2, int h2, int min2, int sec2, Boolean p2)
        {

            DateTime d1 = DateTime.Now;

            d1 = d1.AddDays(g1);
            d1 = d1.AddMonths(m1);
            d1 = d1.AddYears(y1);
            d1 = d1.AddHours(h1);
            d1 = d1.AddMinutes(min1);
            deBasGelir.DateTime = d1.AddSeconds(sec1);
            deBasGelir.ToolTip = deBasGelir.DateTime.ToString();

            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            deBitGelir.DateTime = d2.AddSeconds(sec2);
            deBitGelir.ToolTip = deBitGelir.DateTime.ToString();
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            deBasGelir.Properties.EditMask = "D";
            deBitGelir.Properties.EditMask = "D";

            deBasGelir.Properties.DisplayFormat.FormatString = "D";
            deBitGelir.Properties.DisplayFormat.FormatString = "D";

            deBasGelir.Properties.EditFormat.FormatString = "D";
            deBitGelir.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (comboBoxEdit2.SelectedIndex == 0)// Bu gün
            {
                deBasGelir.Properties.DisplayFormat.FormatString = "f";
                deBitGelir.Properties.EditFormat.FormatString = "f";
                deBasGelir.Properties.EditFormat.FormatString = "f";
                deBitGelir.Properties.DisplayFormat.FormatString = "f";
                deBasGelir.Properties.EditMask = "f";
                deBitGelir.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }

            if (comboBoxEdit2.SelectedIndex == 1)// dün
            {
                deBasGelir.Properties.DisplayFormat.FormatString = "f";
                deBitGelir.Properties.EditFormat.FormatString = "f";
                deBasGelir.Properties.EditFormat.FormatString = "f";
                deBitGelir.Properties.DisplayFormat.FormatString = "f";
                deBasGelir.Properties.EditMask = "f";
                deBitGelir.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (comboBoxEdit2.SelectedIndex == 2)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (comboBoxEdit2.SelectedIndex == 3)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (comboBoxEdit2.SelectedIndex == 4)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (comboBoxEdit2.SelectedIndex == 5)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day + 1), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (comboBoxEdit2.SelectedIndex == 6)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            //else if (cbTarihAraligi.SelectedIndex ==6) // Bu Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
            //                      0, 0, 0, 0, 0, 0, false);

            //}
            //else if (cbTarihAraligi.SelectedIndex == 7) // Geçen Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), -1, 0, 0, 0, false,
            //                    (-DateTime.Now.Day), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false);

            //}
            else if (cbTarihAraligi.SelectedIndex == 6)
            {
                deBasGelir.Enabled = true;
                deBitGelir.Enabled = true;
            }
        }

        private void btnEPosta_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = 0;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();

            Giderler();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = 0;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            Gelirler();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length > 10)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(lUEKullanicilar.EditValue.ToString() + " E-Posta Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;

                //if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    gridView3.ExportToXls(Application.StartupPath + "\\Gelirler.Xls");
                    DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Gelirler " + lueSubeler.Text, Application.StartupPath + "\\Gelirler.Xls", "Gelirler " + ilktarih.Text + "-" + sontarih.Text);

                    gridView4.ExportToXls(Application.StartupPath + "\\Giderler.Xls");
                    DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Giderler " + lueSubeler.Text, Application.StartupPath + "\\Giderler.Xls", "Giderler " + ilktarih.Text + "-" + sontarih.Text);
                }
                //else if (xtraTabControl1.SelectedTabPageIndex == 1)
                //{
                //    gridView3.ExportToXls(Application.StartupPath + "\\SatisRaporuUrunBazinda.Xls");
                //    DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Fiş Bazinda Satış Raporu " + lueSubeler.Text, Application.StartupPath + "\\SatisRaporuUrunBazinda.Xls", "Satış Raporu Ürün Bazında" + ilktarih.Text + "-" + sontarih.Text);

                //}
                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            else
            {
                MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
                lUEKullanicilar.Focus();
            }
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

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gcGelirler;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void btnGiderlerYazdir_Click(object sender, EventArgs e)
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gcGiderler;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void btnyazdirfis_Click(object sender, EventArgs e)
        {
            Yazdir(false);
        }

        void Yazdir(bool dizayn)
        {
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\GelirGiderTablosu.repx");
            rapor.Name = "GelirGiderTablosu";
            rapor.Report.Name = "GelirGiderTablosu";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                rapor.FindControl("label3", true).Text = deBasGelir.Text + "         " + deBitGelir.Text;

                System.Data.DataSet ds = new DataSet("Test");

                string sql = @"hsp_GelirGiderTablosu @ilktar,@sontar,@fkSube";
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@ilktar", deBasGelir.DateTime.ToString("yyyy-MM-dd")));
                list.Add(new SqlParameter("@sontar", deBitGelir.DateTime.ToString("yyyy-MM-dd 23:59")));
                list.Add(new SqlParameter("@fkSube", "1"));
                DataTable dtGelirGider = DB.GetData(sql, list);
                dtGelirGider.TableName = "GelirGiderTablosu";
                ds.Tables.Add(dtGelirGider);

                rapor.DataSource = ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }

        private void yazdırDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdir(true);
        }

        private void btnMailAt_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Gelir Gider Raporu " +
                lUEKullanicilar.EditValue.ToString() + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\GelirGider.repx");
            rapor.Name = "GelirGider";
            rapor.Report.Name = "GelirGider.repx";

            try
            {
                rapor.FindControl("label3", true).Text = ilktarih.Text + "\r" + sontarih.Text;

                string dosyaadi = Application.StartupPath + "\\GelirGider.pdf";
                System.Data.DataSet ds = new DataSet("Test");

                string sql = @"hsp_GelirGider @ilktar,@sontar,@fkSube";
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@ilktar", deBasGelir.DateTime.ToString("yyyy-MM-dd")));
                list.Add(new SqlParameter("@sontar", deBitGelir.DateTime.ToString("yyyy-MM-dd 23:59")));
                list.Add(new SqlParameter("@fkSube", "1"));
                DataTable dtGelirGider = DB.GetData(sql, list);
                dtGelirGider.TableName = "GelirGider";
                ds.Tables.Add(dtGelirGider);

                rapor.DataSource = ds;

                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Gelir Gider Raporu", dosyaadi, "Gün Sonu Raporu");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

        }

        private void cbAcikhesap_CheckedChanged(object sender, EventArgs e)
        {
            btnGelirGider_Click(sender, e);
        }

        private void btnGiderYazdir_Click(object sender, EventArgs e)
        {
            GiderYazdir(false);
        }
        void GelirYazdir(bool Disigner)
        {
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable dtliste = (DataTable)gcGelirler.DataSource;//DB.GetData(sql);
                dtliste.TableName = "dtliste";
                ds.Tables.Add(dtliste);
                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\Gelirler.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Giderler.repx Dosya Bulunamadı");
                    return;
                }
                XtraReport rapor = new XtraReport();

                rapor.LoadLayout(RaporDosyasi);
                rapor.FindControl("label3", true).Text = deBasGelir.Text + "         " + deBitGelir.Text;

                rapor.Name = "Gelirler";

                rapor.Report.Name = "Gelirler";

                rapor.DataSource = ds;

                if (Disigner)
                    rapor.ShowDesignerDialog();
                else
                    rapor.ShowPreviewDialog();
                //ds.Dispose();
                ds.Tables.Remove(dtliste);
                ds.Tables.Remove(Sirket);


                dtliste.Dispose();
                ds.Dispose();
                rapor.Dispose();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void GiderYazdir(bool Disigner)
        {
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable dtliste = (DataTable)gcGiderler.DataSource;
               
                dtliste.TableName = "dtliste";
                ds.Tables.Add(dtliste);
                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\Giderler.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Giderler.repx Dosya Bulunamadı");
                    return;
                }
                XtraReport rapor = new XtraReport();

                rapor.LoadLayout(RaporDosyasi);
                rapor.FindControl("label3", true).Text = deBasGelir.Text + "         " + deBitGelir.Text;

                rapor.Name = "Giderler";

                rapor.Report.Name = "Giderler";

                rapor.DataSource = ds;

                if (Disigner)
                    rapor.ShowDesignerDialog();
                else
                    rapor.ShowPreviewDialog();
                //ds.Dispose();
                ds.Tables.Remove(dtliste);
                ds.Tables.Remove(Sirket);


                dtliste.Dispose();
                ds.Dispose();
                rapor.Dispose();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void giderlerDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GiderYazdir(true);
        }

        private void btnGelir_Click(object sender, EventArgs e)
        {
            GelirYazdir(false);
        }

        private void gelirlerDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GelirYazdir(true);
        }

        private void btnGiderMail_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Gider Raporu " +
                lUEKullanicilar.EditValue.ToString() + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            System.Data.DataSet ds = new DataSet("Test");
            DataTable dtliste = (DataTable)gcGiderler.DataSource;

            dtliste.TableName = "dtliste";
            ds.Tables.Add(dtliste);
            //şirket bilgileri
            DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
            Sirket.TableName = "Sirket";
            ds.Tables.Add(Sirket);

            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

            string RaporDosyasi = exedizini + "\\Raporlar\\Giderler.repx";
            if (!File.Exists(RaporDosyasi))
            {
                MessageBox.Show("Giderler.repx Dosya Bulunamadı");
                return;
            }
            XtraReport rapor = new XtraReport();

            rapor.LoadLayout(RaporDosyasi);
            rapor.FindControl("label3", true).Text = deBasGelir.Text + "         " + deBitGelir.Text;

            rapor.Name = "Giderler";

            rapor.Report.Name = "Giderler";

            rapor.DataSource = ds;
            string dosyaadi = Application.StartupPath + "\\Giderler.pdf";
            rapor.ExportToPdf(dosyaadi);

            ds.Tables.Remove(dtliste);
            ds.Tables.Remove(Sirket);


            dtliste.Dispose();
            ds.Dispose();
            rapor.Dispose();

            DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Gelir Gider Raporu", dosyaadi, "Gün Sonu Raporu");

            formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}

        }

        private void sbtGelirMail_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Gider Raporu " +
                lUEKullanicilar.EditValue.ToString() + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            System.Data.DataSet ds = new DataSet("Test");
            DataTable dtliste = (DataTable)gcGelirler.DataSource;//DB.GetData(sql);
            dtliste.TableName = "dtliste";
            ds.Tables.Add(dtliste);
            //şirket bilgileri
            DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
            Sirket.TableName = "Sirket";
            ds.Tables.Add(Sirket);

            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

            string RaporDosyasi = exedizini + "\\Raporlar\\Gelirler.repx";
            if (!File.Exists(RaporDosyasi))
            {
                MessageBox.Show("Giderler.repx Dosya Bulunamadı");
                return;
            }
            XtraReport rapor = new XtraReport();

            rapor.LoadLayout(RaporDosyasi);
            rapor.FindControl("label3", true).Text = deBasGelir.Text + "         " + deBitGelir.Text;

            rapor.Name = "Gelirler";

            rapor.Report.Name = "Gelirler";

            rapor.DataSource = ds;
            string dosyaadi = Application.StartupPath + "\\Gelirler.pdf";
            rapor.ExportToPdf(dosyaadi);

            //ds.Dispose();
            ds.Tables.Remove(dtliste);
            ds.Tables.Remove(Sirket);


            dtliste.Dispose();
            ds.Dispose();
            rapor.Dispose();

            DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Gelirler Raporu", dosyaadi, "Gün Sonu Raporu");

            formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
        }
    }
}
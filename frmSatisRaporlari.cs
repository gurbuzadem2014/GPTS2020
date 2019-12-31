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
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using GPTS.islemler;
using System.Diagnostics;

namespace GPTS
{
    public partial class frmSatisRaporlari : DevExpress.XtraEditors.XtraForm
    {
        public frmSatisRaporlari()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void ucSatislar_Load(object sender, EventArgs e)
        {
            DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
            if (dtSirket.Rows[0]["BonusYuzde"].ToString() == "0")
            {
                gcBonus.Visible = false;
                gcBonusOdenen.Visible = false;
            }
            cbTarihAraligi.SelectedIndex = 0;

            Subeler();
            //simpleButton1_Click_1(sender, e);
            //dizayn
            string Dosya = DB.exeDizini + "\\SatisRaporlariGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }

            Dosya = DB.exeDizini + "\\SatisRaporlariUrunBazindaGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView3.RestoreLayoutFromXml(Dosya);
                gridView3.ActiveFilter.Clear();
            }

            lUEKullanicilar.Properties.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where eposta is not null");
            lUEKullanicilar.ItemIndex = 0;

            ModulYetkiler();
        }

        void ModulYetkiler()
        {
            //Kod,ModulAdi,Yetki
            DataTable dtYetkiler = islemler.yetkiler.ModulYetkileri(int.Parse(DB.fkKullanicilar));
            foreach (DataRow row in dtYetkiler.Rows)
            {
                string kod = row["Kod"].ToString();
                string yetki = row["Yetki"].ToString();

                //if (kod == ((int)Degerler.Moduller.Satis).ToString())
                //{
                //    if (yetki == "True")
                //        this.Enabled = true;
                //    else
                //        this.Enabled = false;
                //}
                //else 
                if (kod == "16.1") //((int)Degerler.Moduller.SatisRaporlariTumu).ToString())
                {
                    if (yetki == "True")
                        lueSubeler.Enabled = true;
                    else
                        lueSubeler.Enabled = false;
                }

            }
        }
        private void Subeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData(@"select '' as pkSube,'Tümü' as sube_adi
            union all
            select pkSube, sube_adi from Subeler with(nolock) where Subeler.aktif=1");
            lueSubeler.EditValue = Degerler.fkSube;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.ShowDialog();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.pkSatislar = int.Parse(dr["pkSatislar"].ToString());
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
        }

        void Satislar()
        {
            string sql = "sp_FisBazindaSatisRaporu @ilktar,@sontar,@fkSube";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
            gridControl1.DataSource = DB.GetData(sql, list);

            gridView1.OptionsView.ShowViewCaption = true;
            gridView1.ViewCaption = "Satış Raporu - " + ilktarih.DateTime.ToString("dd.MMMM.yyyy HH:mm")+
                " - "+ sontarih.DateTime.ToString("dd.MMMM.yyyy HH:mm");
        }

//        void OdemeSekliGetir()
//        {
//            string sql = @"select OdemeSekli,SUM(Borc-Alacak) as Tutar from KasaHareket
//where fkSatislar is not null and AktifHesap=1 and Tarih Between @ilktar and @sontar
//group by OdemeSekli
//union all
//select 'Açık Hesap' as OdemeSekli,sum(isnull(AcikHesap,0)) as Tutar from Satislar
//where Siparis=1 and GuncellemeTarihi Between @ilktar and @sontar
//union all
//SELECT 'Nakit(Bonus)' as OdemeSekli, isnull(sum(Bonus),0) as Tutar FROM BonusKullanilan
//where Tarih Between @ilktar and @sontar";
//            ArrayList list = new ArrayList();
//            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
//            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
//            gcOdemeSekli.DataSource = DB.GetData(sql, list);
//        }

//        void SatisDurumlari()
//        {
//            string sql = @"select sd.Durumu,SUM(sad.Adet*(sad.SatisFiyati-sad.iskontotutar)) as ToplamTutar,sum(isnull(AcikHesap,0)) as AcikHesap,sum(isnull(AcikHesapOdenen,0)) as AcikHesapOdenen from Satislar s with(nolock)
//inner join SatisDetay sad with(nolock) on sad.fkSatislar=s.pkSatislar
//inner join SatisDurumu  sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu
//where Siparis=1 and GuncellemeTarihi Between @ilktar and @sontar
//group by sd.Durumu";
//ArrayList list = new ArrayList();
//list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
//list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
//            gridControl2.DataSource = DB.GetData(sql, list);
//        }
        
        void Satislar_UrunBazinda()
        {
            string sql = @"exec sp_FisBazindaSatisRaporu_Ayrintili @ilktar,@sontar,@fkSube";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
            gridControl3.DataSource = DB.GetData(sql, list);

            gridView3.OptionsView.ShowViewCaption = true;
            gridView3.ViewCaption = "Satış Raporu Ürün Bazında " + ilktarih.DateTime.ToString("dd.MMMM.yyyy HH:mm")+
                " - " + sontarih.DateTime.ToString("dd.MMMM.yyyy HH:mm");
        }

        void SatislarUrunBazindeAyrintili()
        {
            string sql = @"exec HSP_FisBazindaSatisRaporuDetayli @ilktar,@sontar,@fkSube";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
            gridControl4.DataSource = DB.GetData(sql, list);

            gridView4.OptionsView.ShowViewCaption = true;
            gridView5.ViewCaption = "Satış Raporu Ürün Bazında Ayrıntı " + ilktarih.DateTime.ToString("dd.MMMM.yyyy HH:mm") +
               " - " + sontarih.DateTime.ToString("dd.MMMM.yyyy HH:mm");
        }

        void Satislar_Kullanicibazinda()
        {
            string sql = @"exec HSP_SatisRaporuKullaniciBazinda @ilktar,@sontar,@fkSube";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
            gridControl5.DataSource = DB.GetData(sql, list);
        }
        void Satislar_PlasiyerBazinda()
        {
            string sql = @"exec HSP_SatisRaporuPlasiyerBazinda @ilktar,@sontar,@fkSube";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
            gridControl6.DataSource = DB.GetData(sql, list);
        }
        void Satislar_PersonelBazinda()
        {
            string sql = @"exec HSP_SatisRaporuHizmetiYapan @ilktar,@sontar,@fkSube";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
            gridControl7.DataSource = DB.GetData(sql, list);
        }
        
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
                Satislar();
            else if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                Satislar_UrunBazinda();
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 2)
            {
                string Dosya = DB.exeDizini + "\\SatisRaporlariUrunBazindaGrid.xml";

                if (File.Exists(Dosya))
                {
                    gridView5.RestoreLayoutFromXml(Dosya);
                    gridView5.ActiveFilter.Clear();
                }

                SatislarUrunBazindeAyrintili();
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 3)
                Satislar_Kullanicibazinda();
            else if (xtraTabControl1.SelectedTabPageIndex == 4)
            {
                string sql = @"exec HSP_SatisRaporuPivotGrid @ilktar,@sontar,@fkSube";
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
                list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
                list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
                pivotGridControl1.DataSource = DB.GetData(sql, list);
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 5)
            {
                string sql = @"exec sp_SatisRaporu_DepoMevcutlari @ilktar,@sontar,@fkSube";
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
                list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
                list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
                pivotGridControl2.DataSource = DB.GetData(sql, list);
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 6)
            {
                Satislar_UrunBazindaDepo();
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 7)
            {
                Satislar_PlasiyerBazinda();
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 8)
            {
                Satislar_PersonelBazinda();
            }

        }

        void Satislar_UrunBazindaDepo()
        {
            string sql = @"exec sp_FisBazindaSatisRaporu_DepoMevcut @ilktar,@sontar,@fkSube";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
            gridControl2.DataSource = DB.GetData(sql, list);
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
            ilktarih.DateTime = d1.AddSeconds(sec1);
            ilktarih.ToolTip = ilktarih.DateTime.ToString();

            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih.DateTime = d2.AddSeconds(sec2);
            sontarih.ToolTip = sontarih.DateTime.ToString();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());

            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
        }

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex== 0)// Bu gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            if (cbTarihAraligi.SelectedIndex == 1)// dün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 2)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 3)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day+1), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);              

            }
            else if (cbTarihAraligi.SelectedIndex == 6)// bu yıl
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
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        void RaporOnizleme(bool Disigner)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSatislar = dr["pkSatislar"].ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Satislar s 
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar
inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
where s.pkSatislar =" + pkSatislar;
            Barkod.DataSource = DB.GetData(sql);

            RaporDosyasi = exedizini + "\\Raporlar\\satisfisia4.repx";
            //rapor.DataSource = gCPerHareketleri.DataSource;
            //rapor.CreateDocument();
            Barkod.LoadLayout(RaporDosyasi);
            //rapor.FindControl("xlKasaAdi", true).Text = lueKasa.Text + " " + ilkdate.DateTime.ToString("dd.MM.yyyy") +
            //    "-" + sondate.DateTime.ToString("dd.MM.yyyy");
            if (Disigner)
                Barkod.ShowDesignerDialog();
            else
                Barkod.ShowRibbonPreview();
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
            if(xtraTabControl1.SelectedTabPageIndex==0) 
               printableLink.Component = gridControl1;
            if (xtraTabControl1.SelectedTabPageIndex == 1)
                printableLink.Component = gridControl3;
            if (xtraTabControl1.SelectedTabPageIndex == 2)
                printableLink.Component = gridControl4;
            if (xtraTabControl1.SelectedTabPageIndex == 6)
                printableLink.Component = gridControl2;
            if (xtraTabControl1.SelectedTabPageIndex == 8)
                printableLink.Component = gridControl7;

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

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporOnizleme(true);
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            //FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();

            simpleButton1_Click_1(sender, e);
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmStokSatisGrafigi StokSatisGrafigi = new frmStokSatisGrafigi("0");
            StokSatisGrafigi.ShowDialog();
        }

        private void gridView3_EndSorting(object sender, EventArgs e)
        {
            if (gridView3.DataRowCount > 0)
                gridView3.FocusedRowHandle = 0;
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmSatisUrunBazindaDetay StokTekSatislari = new frmSatisUrunBazindaDetay("0", "", "");
            StokTekSatislari.Tag = "0";
            StokTekSatislari.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            StokTekSatislari.ilktarih.EditValue = ilktarih.EditValue;
            StokTekSatislari.sontarih.EditValue = sontarih.EditValue;
            StokTekSatislari.ShowDialog();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
          simpleButton1_Click_1(sender,e);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmMusteriKarlikRaporu istatistikRaporlari = new frmMusteriKarlikRaporu();
            istatistikRaporlari.Tag = 1;
            istatistikRaporlari.Show();
        }

        private void gridView5_EndSorting(object sender, EventArgs e)
        {
            if (gridView5.DataRowCount > 0)
                gridView5.FocusedRowHandle = 0;
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;
            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void stokKartıDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void müşteriyeTopluFaturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmFaturaToplu TopluFatura = new frmFaturaToplu(dr["fkFirma"].ToString());
            TopluFatura.Show();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //gridControl çift tıkda var 
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(true);
            //FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            //FisNoBilgisi.ShowDialog();
        }

        private void btnFisYazdir_Click(object sender, EventArgs e)
        {
            YazdirDizayn(false);
        }

        void YazdirDizayn(bool Yazdir)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\FisToplu.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("FisToplu.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }

            gridView1.ActiveFilterString = "Sec=1";
            string secilen_idler = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    secilen_idler = secilen_idler + dr["pkSatislar"].ToString() + ",";
                    //DB.ExecuteSQL("UPDATE Satislar Set Yazdir=1 where pkSatislar=" + dr["pkSatislar"].ToString());
                }

            }
            gridView1.ClearColumnsFilter();

            if (secilen_idler.Length == 0)
            {
                formislemleri.Mesajform("Seçim yapınız", "K", 200);
                gridView1.Focus();
                return;
            }
            secilen_idler = secilen_idler.Substring(0, secilen_idler.Length - 1);

            FisYazdir(Yazdir, DosyaYol, secilen_idler);
        }

        void FisYazdir(bool Disigner, string RaporDosyasi, string secilen_idler)
        {
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            //list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.Date));
            //list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"select s.pkSatislar,s.GuncellemeTarihi as Tarih,
                s.pkSatislar as FisNo,sk.Stokadi,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as SatisFiyati,
                sd.iskontotutar,
                (sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as Tutar,OdemeSekli,sdu.Durumu,
                f.Firmaadi,Devir as Bakiye
                from Satislar s with(nolock)
                inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
                left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu 
                inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
                where pkSatislar in(" + secilen_idler + ") order by s.pkSatislar desc");//, list);
                //and s.GuncellemeTarihi between @ilktar and @sontar and s.fkFirma=@fkFirma and s.Siparis=1 and fkSatisDurumu not in(1,10)
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                
                //DataTable Fis = DB.GetData(@"SELECT Admin");//*,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                //Fis.TableName = "Fis";
                //ds.Tables.Add(Fis);
                //kasahareketleri
                //DataTable dtkasahareketleri = DB.GetData(@"select top 1 *,Tutar-Borc as OncekiBakiye from KasaHareket with(nolock) where fkSatislar is null and fkFirma="
                //    + musteriadi.Tag.ToString() + " order by pkKasaHareket DESC");
                //dtkasahareketleri.TableName = "kasahareketleri";
                //ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.Name = "Müşteri Hesap Extresi";
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void fişTopluDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirDizayn(true);
        }

        private void fişDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
              if (gridView1.FocusedRowHandle < 0) return;

               DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

               fişDüzenleToolStripMenuItem.Tag = dr["pkSatislar"].ToString();

               Close();
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\SatisRaporlariGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\SatisRaporlariGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void sutünSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void yazdırToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;
            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
            
        }

        private void stokHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;

            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void excelGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\SatisRaporuUrunBazinda" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
            gridView5.ExportToXls(dosyaadi);
            Process.Start(dosyaadi);
        }

        private void sutünSeçimiToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (!formislemleri.SifreIste()) return;

            gridView5.ShowCustomization();
            gridView5.OptionsBehavior.AutoPopulateColumns = true;
            gridView5.OptionsCustomization.AllowColumnMoving = true;
            gridView5.OptionsCustomization.AllowColumnResizing = true;
            gridView5.OptionsCustomization.AllowQuickHideColumns = true;
            gridView5.OptionsCustomization.AllowRowSizing = true;
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\SatisRaporlariUrunBazindaGrid.xml";
            gridView5.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\SatisRaporlariUrunBazindaGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void mailGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mail = formislemleri.inputbox("E-Posta Adresi Giriniz", "Gönderilecek E-Posta", "@", false);
            if (mail.Length < 5) return;

            string dosyaadi = Application.StartupPath + "\\SatisRaporuUrunBazindaOzet" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
            gridView3.ExportToXls(dosyaadi);
            //Process.Start(dosyaadi);

            DB.epostagonder(mail, "Satis Raporu Urun Bazinda Ozet", dosyaadi, "Satis Raporu Urun Bazinda Ozet");

            formislemleri.Mesajform("E-Posta Gönderildi.", "S", 100);
        }

        private void lueSubeler_EditValueChanged(object sender, EventArgs e)
        {
            simpleButton1_Click_1(sender, e);
        }

        //private void simpleButton4_Click(object sender, EventArgs e)
        //{
        //    if (lUEKullanicilar.EditValue.ToString().Length > 10)
        //    {
        //        DialogResult secim;
        //        secim = DevExpress.XtraEditors.XtraMessageBox.Show(lUEKullanicilar.EditValue.ToString() + " E-Posta Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
        //        if (secim == DialogResult.No) return;

        //        if (xtraTabControl1.SelectedTabPageIndex == 0)
        //        {
        //            gridView1.ExportToXls(Application.StartupPath + "\\SatisRaporuFisBazinda.Xls");
        //            DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Satış Raporu Fiş Bazinda "+ lueSubeler.Text, Application.StartupPath + "\\SatisRaporuFisBazinda.Xls", "Satış Raporu Fiş Bazinda " + ilktarih.Text + "-" + sontarih.Text);

        //        }
        //        else if (xtraTabControl1.SelectedTabPageIndex == 1)
        //        {
        //            gridView3.ExportToXls(Application.StartupPath + "\\SatisRaporuUrunBazinda.Xls");
        //            DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Fiş Bazinda Satış Raporu "+ lueSubeler.Text, Application.StartupPath + "\\SatisRaporuUrunBazinda.Xls", "Satış Raporu Ürün Bazında" + ilktarih.Text + "-" + sontarih.Text);

        //        }
        //        formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
        //    }
        //    else
        //    {
        //        MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
        //        lUEKullanicilar.Focus();
        //    }
        //}

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length > 10)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(lUEKullanicilar.EditValue.ToString() + " E-Posta Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;

                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    gridView1.ExportToXls(Application.StartupPath + "\\SatisRaporuFisBazinda.Xls");
                    DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Satış Raporu Fiş Bazinda " + lueSubeler.Text, Application.StartupPath + "\\SatisRaporuFisBazinda.Xls", "Satış Raporu Fiş Bazinda " + ilktarih.Text + "-" + sontarih.Text);

                }
                else if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    gridView3.ExportToXls(Application.StartupPath + "\\SatisRaporuUrunBazinda.Xls");
                    DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Fiş Bazinda Satış Raporu " + lueSubeler.Text, Application.StartupPath + "\\SatisRaporuUrunBazinda.Xls", "Satış Raporu Ürün Bazında" + ilktarih.Text + "-" + sontarih.Text);

                }
                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            else
            {
                MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
                lUEKullanicilar.Focus();
            }
        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void depoMevcutlerıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmStokKartiDepoMevcut StokKartiDepoMevcut = new frmStokKartiDepoMevcut(int.Parse(dr["pkStokKarti"].ToString()));
            StokKartiDepoMevcut.ShowDialog();
        }

        private void stokHareketleriToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void kaydetToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //gridView7
        }

        private void personelEkÖdemeEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.pkPersoneller = 0;
            if (gridView7.FocusedRowHandle < 0) return;
            DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);

            DB.pkPersoneller = int.Parse(dr["pkPersoneller"].ToString());

            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.Tag = "1";
            KasaCikis.cbGelirMi.Checked = false;
            KasaCikis.pkPersonel.Text = dr["pkPersoneller"].ToString();
            KasaCikis.cbHareketTipi.SelectedIndex = 3;
            ///Ödeme Girişi
            KasaCikis.Tag = "1";
            KasaCikis.tEaciklama.Text = "Personel Ek Ödeme";
            KasaCikis.cbAktifHesap.Checked = false;
            KasaCikis.cbGelirMi.Checked = false;
            KasaCikis.ShowDialog();
        }

        private void btnExcelGonder_Click(object sender, EventArgs e)
        {
            string dosyaadi = "";

            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                dosyaadi = Application.StartupPath + "\\SatisRaporuFisBazinda.Xls";
                gridView1.ExportToXls(dosyaadi);
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                dosyaadi = Application.StartupPath + "\\SatisRaporuUrunBazinda" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
                gridView3.ExportToXls(dosyaadi);
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 2)
            {
                dosyaadi = Application.StartupPath + "\\SatisRaporuUrunBazindaAyrinti" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
                gridView5.ExportToXls(dosyaadi);
                
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 8)
            {
                dosyaadi = Application.StartupPath + "\\SatisRaporuPersonelHizmetSayilari" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
                gridView7.ExportToXls(dosyaadi);

            }
            Process.Start(dosyaadi);
        }

        private void fişDüzenleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmFaturaDuzelt fFaturaDuzelt = new frmFaturaDuzelt(dr["pkSatislar"].ToString());
            fFaturaDuzelt.ShowDialog();

            Satislar();
        }

        private void seçilenleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string s = formislemleri.MesajBox("Seçilen Fişler Silinsin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                if (dr["sec"].ToString() != "True") continue;

                string fisId = dr["pkSatislar"].ToString();
                string Durumu = dr["Durumu"].ToString();
                string fkSatisDurumu = dr["fkSatisDurumu"].ToString();
                string fkFirma = dr["fkFirma"].ToString();

                DataTable dtSatislar = DB.GetData("select * from Satislar with(nolock) where pkSatislar=" + fisId);
                if (dtSatislar.Rows.Count == 0)
                {
                    formislemleri.Mesajform("Fiş Bulunamadı.", "K", 200);
                    continue;
                }

                //string mesaj = "";
                //bool faturasivar = false;
                //fatura kesildi mi?
                //if (DB.GetData("select count(*) from SatisDetay with(nolock) where isnull(fkFaturaToplu,0)<>0 and fkSatislar=" + fisno.Text).Rows[0][0].ToString() != "0")
                //{
                //    mesaj = mesaj + "Faturası Kesilmiş Hizmetler var! \n";
                //    //faturasivar = true;
                //}
                //if (DB.GetData("select count(*) from Taksit with(nolock) where fkSatislar=" + fisno.Text).Rows[0][0].ToString() != "0")
                //{
                //    mesaj = mesaj + "Satışa Ait Taksit(ler) var! \n";
                //}

                //mesaj = mesaj + "Fişi Silmek İstediğinize Eminmisiniz. \n Not: Ödemeler ve Stok Adetleri Geri Alınacaktır";

                //string sec = formislemleri.MesajBox(mesaj, "Satış Sil", 3, 0);
                //if (sec != "1") return;

                #region çek durumunu değiştir
                //if (fkCek != "")
                //{
                //    DB.ExecuteSQL("update Cekler set fkCekTuru=0,fkFirma=0 where pkCek=" + fkCek);
                //}
                #endregion

                DB.ExecuteSQL("delete from FaturaToplu " +
                 " where pkFaturaToplu in(select fkFaturaToplu from SatisDetay where fkSatislar=" + fisId + ")");
                DataTable dtFisDetay=DB.GetData("select * from SatisDetay WHERE fkSatislar="+ fisId);

                for (int j = 0; j < dtFisDetay.Rows.Count; j++)
                {
                    string SatisDetayId = dtFisDetay.Rows[j]["pkSatisDetay"].ToString();
                    string fkStokKarti = dtFisDetay.Rows[j]["fkStokKarti"].ToString();
                    string Adet = dtFisDetay.Rows[j]["Adet"].ToString();
                    string fkDepolar = dtFisDetay.Rows[j]["fkDepolar"].ToString();
                    if (fkDepolar == "") fkDepolar = "1";
                    
                    decimal miktar = 0;
                    decimal.TryParse(Adet, out miktar);
                    if (miktar < 0)
                        DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=isnull(Mevcut,0)-" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                        " where pkStokKarti=" + fkStokKarti);
                    else
                        DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=isnull(Mevcut,0)+" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                        " where pkStokKarti=" + fkStokKarti);


                    if (miktar < 0)
                        DB.ExecuteSQL("UPDATE StokKartiDepo SET MevcutAdet=MevcutAdet-" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                        " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar);
                    else
                        DB.ExecuteSQL("UPDATE StokKartiDepo SET MevcutAdet=MevcutAdet+" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                        " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar);


                    DB.ExecuteSQL("DELETE FROM SatisDetay WHERE pkSatisDetay=" + SatisDetayId);
                }
                
                DB.ExecuteSQL("DELETE FROM KasaHareket where fkSatislar=" + fisId);
                DB.ExecuteSQL("DELETE FROM Satislar WHERE pkSatislar=" + fisId);


                DB.ExecuteSQL("delete from Taksitler where taksit_id=(select taksit_id from Taksit where fkSatislar=" + fisId + ")");
                DB.ExecuteSQL("delete from Taksit where fkSatislar=" + fisId);

                DB.ExecuteSQL("delete from HatirlatmaAnimsat where fkSatislar=" + fisId);

                //if (fkSatisDurumu != "1" && fkSatisDurumu != "10" && fkSatisDurumu != "11")
                //{
                    //MevcutSatisGeriAl();
                    //MevcutDepoGeriAl();
                    //bonusdus();
                //}

                //DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,fkKullanicilar,HataAciklama) VALUES(2,'" +
                //   fisno.Text + "-Fiş Silindi Tutar=" + Satis4Toplam.Text.Replace(",", ".") + "',getdate(),1," + DB.fkKullanicilar + ",'" + sifregir1.Girilen.Text + "')");

                //DB.ExecuteSQL("INSERT INTO SatislarSilinen (fkSatislar,Aciklama,Tarih,fkKullanicilar,fkFirma) VALUES(" + fisno.Text + ",'" +
                //    sifregir1.Girilen.Text + "',getdate()," + DB.fkKullanicilar + "," + groupControl1.Tag.ToString() + ")");
            }

            Satislar();

            //inputForm sifregir1 = new inputForm();
            //sifregir1.Text = "Silinme Nedeni";
            //sifregir1.GirilenCaption.Text = "Silme Nedenini Giriniz!";
            //sifregir1.Girilen.Text = "İptal";
            ////sifregir.Girilen.Properties.PasswordChar = '#';
            //sifregir1.ShowDialog();

        }

        private void eksikListesineEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string Stokadi = dr["Stokadi"].ToString();
            string fkStokKarti = dr["pkStokKarti"].ToString();
            string fark = dr["Fark"].ToString();
            if (DB.GetData("select StokAdi from EksikListesi el with(nolock) where fkStokKarti=" + fkStokKarti).Rows.Count > 0)
            {
                formislemleri.Mesajform("Daha Önce Eklendi!", "K", 200);
                return;
            }
            //inputForm sifregir = new inputForm();
            //sifregir.Text = "Miktar Giriniz";
            //sifregir.ShowDialog();

            int girilen = 0;
            int.TryParse(formislemleri.inputbox("Eksik Miktar Girişi", "Alınacak Stok Miktarını Giriniz", fark , false), out girilen);
            if (girilen == 0)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız", "K", 200);
                //yesilisikyeni();
                return;
            }
            int sonuc = DB.ExecuteSQL("INSERT INTO EksikListesi (Tarih,StokAdi,Miktar,fkStokKarti,Durumu) values(getdate(),'"
                + Stokadi + "'," + girilen.ToString().Replace(",", ".") + "," + fkStokKarti + ",'Yeni')");
            if (sonuc == -1)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız"+ sonuc, "K", 200); 
            }
            //yesilisikyeni();
        }

        private void eksikListesineEkleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;

            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);

            string Stokadi = dr["Stokadi"].ToString();
            string fkStokKarti = dr["pkStokKarti"].ToString();

            if (DB.GetData("select StokAdi from EksikListesi el with(nolock) where fkStokKarti=" + fkStokKarti).Rows.Count > 0)
            {
                formislemleri.Mesajform("Daha Önce Eklendi!", "K", 200);
                return;
            }
            //inputForm sifregir = new inputForm();
            //sifregir.Text = "Miktar Giriniz";
            //sifregir.ShowDialog();

            int girilen = 0;
            int.TryParse(formislemleri.inputbox("Eksik Miktar Girişi", "Alınacak Stok Miktarını Giriniz", "1", false), out girilen);
            if (girilen == 0)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız", "K", 200);
                //yesilisikyeni();
                return;
            }
            int sonuc = DB.ExecuteSQL("INSERT INTO EksikListesi (Tarih,StokAdi,Miktar,fkStokKarti,Durumu) values(getdate(),'"
                + Stokadi + "'," + girilen.ToString().Replace(",", ".") + "," + fkStokKarti + ",'Yeni')");
            if (sonuc == -1)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız" + sonuc, "K", 200);
            }
        }

        private void eksikListesineEkleToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string Stokadi = dr["Stokadi"].ToString();
            string fkStokKarti = dr["pkStokKarti"].ToString();

            if (DB.GetData("select StokAdi from EksikListesi el with(nolock) where fkStokKarti=" + fkStokKarti).Rows.Count > 0)
            {
                formislemleri.Mesajform("Daha Önce Eklendi!", "K", 200);
                return;
            }
            //inputForm sifregir = new inputForm();
            //sifregir.Text = "Miktar Giriniz";
            //sifregir.ShowDialog();

            int girilen = 0;
            int.TryParse(formislemleri.inputbox("Eksik Miktar Girişi", "Alınacak Stok Miktarını Giriniz", "1", false), out girilen);
            if (girilen == 0)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız", "K", 200);
                //yesilisikyeni();
                return;
            }
            int sonuc = DB.ExecuteSQL("INSERT INTO EksikListesi (Tarih,StokAdi,Miktar,fkStokKarti,Durumu) values(getdate(),'"
                + Stokadi + "'," + girilen.ToString().Replace(",", ".") + "," + fkStokKarti + ",'Yeni')");
            if (sonuc == -1)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız" + sonuc, "K", 200);
            }
        }

        private void kaydetToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\SatisRaporlariUrunBazindaGrid.xml";
            gridView3.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\SatisRaporlariUrunBazindaGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void alanListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView3.ShowCustomization();
            gridView3.OptionsBehavior.AutoPopulateColumns = true;
            gridView3.OptionsCustomization.AllowColumnMoving = true;
            gridView3.OptionsCustomization.AllowColumnResizing = true;
            gridView3.OptionsCustomization.AllowQuickHideColumns = true;
            gridView3.OptionsCustomization.AllowRowSizing = true;
        }
    }
}

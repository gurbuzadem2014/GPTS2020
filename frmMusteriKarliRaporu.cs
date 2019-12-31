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
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using System.Threading;
using GPTS.Include.Data;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmMusteriKarliRaporu : DevExpress.XtraEditors.XtraForm
    {
        public frmMusteriKarliRaporu()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        //void KasadakiPara_()
        //{
        //    decimal KasadakiPara = 0;
        //    decimal.TryParse(DB.GetData("select isnull(sum(borc),0)-isnull(sum(Alacak),0) as KasaBakiye from KasaHareket with(nolock) where fkKasalar is not null").Rows[0][0].ToString(),
        //        out KasadakiPara);
        //    ceKasadakiParaMevcut.Value = KasadakiPara;
        //}

        private void ucKasaHareketleri_Load(object sender, EventArgs e)
        {

            if (!formislemleri.SifreIste())
            {
                Close();
                return;
            }

            cbTarihAraligi.SelectedIndex = 0;

            btnListele_Click(sender, e);
            //ilkdate.DateTime = DateTime.Today;
            //sondate.DateTime = DateTime.Today;

            //Thread.Sleep(1000);
            //Thread islem = new Thread(new ThreadStart(Kasadakipara));
            //islem.Start();
            //Kasadakipara();

            //Thread.Sleep(1000);
            //System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            //Thread islem = new Thread(new ThreadStart(DevirGetir));
            //islem.Start();

            string Dosya = DB.exeDizini + "\\musterikarlikraporuGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView2.RestoreLayoutFromXml(Dosya);
                gridView2.ActiveFilter.Clear();
            }
        }
//        void DevirGetir()
//        {
//            DataTable dt = DB.GetData(@"select isnull(sum(kh.Borc-kh.Alacak),0)   from KasaHareket as kh with(nolock)
//                        where fkKasalar is not null and Tarih<='"+ilkdate.DateTime.ToString("yyyy-MM-dd HH:mm")+"'");
//            ceDevir.Value = decimal.Parse(dt.Rows[0][0].ToString());
//        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if(gridView2.FocusedRowHandle<0)return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string fkCek = dr["fkCek"].ToString();
            string fkTaksitler = dr["fkTaksitler"].ToString();

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["pkKasaHareket"].ToString() + " Kasa Hareketi Silinsin mi?", "GPTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            try
            {
                if (fkCek != "")
                    DB.ExecuteSQL("update Cekler set fkCekTuru=0 where pkCek=" + fkCek);

                if (fkTaksitler != "")
                {
                    DB.ExecuteSQL("UPDATE Taksitler Set Odenen=0" +
                        ",OdendigiTarih=null,OdemeSekli='silindi'" +
                        " where pkTaksitler=" + fkTaksitler);
                }


                string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + dr["pkKasaHareket"].ToString();
                DB.ExecuteSQL(sql);
            }
            catch (Exception exp)
            {
                btnSil.Enabled = true;
                return;
            }
            btnSil.Enabled = false;
            btnListele_Click(sender, e);
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            //btnSil.Tag = dr["pkKasaHareket"].ToString();
            //btnSil.ToolTip = dr["Aciklama"].ToString();
            //btnSil.Enabled = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            RaporOnizleme(false);
        }
        void RaporOnizleme(bool Disigner)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";

            DevExpress.XtraReports.UI.XtraReport rapor = new DevExpress.XtraReports.UI.XtraReport();
            RaporDosyasi = exedizini + "\\Raporlar\\MusteriKarlikRaporu.repx";
            rapor.DataSource = gCMusteriKarlikRaporu.DataSource;
            //rapor.CreateDocument();
            if (!File.Exists(RaporDosyasi)) { Print(); return; }
            rapor.LoadLayout(RaporDosyasi);

            //rapor.FindControl("xlKasaAdi", true).Text = lueKasa.Text +" "+ ilkdate.DateTime.ToString("dd.MM.yyyy")+
            //    "-" + sondate.DateTime.ToString("dd.MM.yyyy"); 
            if (Disigner)
                rapor.ShowDesignerDialog();
            else
                rapor.ShowRibbonPreview();
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
        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));

        }
        public virtual void InitPrintTool(PrintTool tool)
        {
            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);

        }
        public void Print()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gCMusteriKarlikRaporu;

            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            RaporOnizleme(true);
        }

        private void gridView2_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            e.TotalValue.ToString();
        }

        private void gridView2_CustomSummaryExists(object sender, DevExpress.Data.CustomSummaryExistEventArgs e)
        {
            //string s ="";
            //if(((DevExpress.XtraGrid.GridColumnSummaryItem)(e.Item)).SummaryValue==null)
            //(((DevExpress.XtraGrid.GridColumnSummaryItem)(e.Item))).SummaryValue.ToString();
            ////((((DevExpress.XtraGrid.GridColumnSummaryItem)(e.Item))).Column.SummaryItem).SummaryValue;
        }

        private void cESonrakiBakiye_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void cbKasaGrup_SelectedIndexChanged(object sender, EventArgs e)
        {
           
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
            ilkdate.DateTime = d1.AddSeconds(sec1);
            ilkdate.ToolTip = ilkdate.DateTime.ToString();

            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sondate.DateTime = d2.AddSeconds(sec2);
            sondate.ToolTip = sondate.DateTime.ToString();

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
        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilkdate.Properties.EditMask = "D";
            sondate.Properties.EditMask = "D";
            ilkdate.Properties.DisplayFormat.FormatString = "D";
            ilkdate.Properties.EditFormat.FormatString = "D";
            sondate.Properties.DisplayFormat.FormatString = "D";
            sondate.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// Bu gün
            {
                ilkdate.Properties.DisplayFormat.FormatString = "f";
                sondate.Properties.EditFormat.FormatString = "f";
                ilkdate.Properties.EditFormat.FormatString = "f";
                sondate.Properties.DisplayFormat.FormatString = "f";
                ilkdate.Properties.EditMask = "f";
                sondate.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            if (cbTarihAraligi.SelectedIndex == 1)// dün
            {
                ilkdate.Properties.DisplayFormat.FormatString = "f";
                sondate.Properties.EditFormat.FormatString = "f";
                ilkdate.Properties.EditFormat.FormatString = "f";
                sondate.Properties.DisplayFormat.FormatString = "f";
                ilkdate.Properties.EditMask = "f";
                sondate.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 2)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days-7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days-1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

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

                //sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                //                    0, 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// önceki ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
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
                sondate.Enabled = true;
                ilkdate.Enabled = true;
            }
        }

        private void ceAlacak_EditValueChanged(object sender, EventArgs e)
        {
            //ceBakiye.Value = cEBorc.Value - ceAlacak.Value;
        }

        private void cEBorc_EditValueChanged(object sender, EventArgs e)
        {
            //ceBakiye.Value = cEBorc.Value - ceAlacak.Value;
        }

        private void kasaGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            btnListele_Click(sender, e);
        }

        private void kasaÇıkışıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();

            btnListele_Click(sender, e);
        }

        private void düzeltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView2.FocusedRowHandle < 0) return;
            //DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            //frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();
            //KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();
            //KasaHareketDuzelt.Tag = this.Tag;
            //KasaHareketDuzelt.ShowDialog();
            //btnListele_Click(sender, e);
        }

        private void frmKasaHareketleri_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
                Close();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void kasaDevirBakiyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKasaBakiyeDuzeltme KasaBakiyeDuzeltme = new frmKasaBakiyeDuzeltme(0);
            //KasaBakiyeDuzeltme.pkKasalar.Text = lueKasa.EditValue.ToString();
            //KasaBakiyeDuzeltme.ceKasadakiParaMevcut.Value = ceBakiye.Value;
            KasaBakiyeDuzeltme.ShowDialog();

            btnListele_Click(sender, e);
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            string sql = @"exec Hsp_MusteriKarlikRaporu '@ilktar','@sontar'";

            sql = sql.Replace("@ilktar", ilkdate.DateTime.ToString("yyyy-MM-dd HH:mm:00"));
            sql = sql.Replace("@sontar", sondate.DateTime.ToString("yyyy-MM-dd HH:mm:59"));

            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@ilktar", ilkdate.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            //list.Add(new SqlParameter("@sontar", sondate.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            gCMusteriKarlikRaporu.DataSource = DB.GetData(sql);//, list);

            btnSil.Tag = "";
            btnSil.ToolTip = "";
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void müşteriKArtıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
        }

        private void gridView2_EndSorting(object sender, EventArgs e)
        {
            //TODO:
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gCMusteriKarlikRaporu, "A4");
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\musterikarlikraporuGrid.xml";
            gridView2.SaveLayoutToXml(Dosya); 
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\musterikarlikraporuGrid.xml";
            if (File.Exists(Dosya))
            {
                for (int i = 0; i < gridView2.Columns.Count; i++)
                    gridView2.Columns[i].Visible = true;
                File.Delete(Dosya);
                Close();
            }
        }

        private void sütünSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView2.ShowCustomization();
            gridView2.OptionsBehavior.AutoPopulateColumns = true;
            gridView2.OptionsCustomization.AllowColumnMoving = true;
            gridView2.OptionsCustomization.AllowColumnResizing = true;
            gridView2.OptionsCustomization.AllowQuickHideColumns = true;
            gridView2.OptionsCustomization.AllowRowSizing = true;
        }

        private void tedarikçiHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

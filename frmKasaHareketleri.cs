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
using DevExpress.Utils;

namespace GPTS
{
    public partial class frmKasaHareketleri : DevExpress.XtraEditors.XtraForm
    {
        public frmKasaHareketleri()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        void Kasadakipara()
        {
            decimal KasadakiPara = 0;
            decimal.TryParse(DB.GetData("select isnull(sum(borc),0)-isnull(sum(Alacak),0) as KasaBakiye from KasaHareket kh with(nolock) "+
                " where kh.fkKasalar<>0 and (kh.fksube is null or kh.fkSube="+Degerler.fkSube+")").Rows[0][0].ToString(),
                out KasadakiPara);
            ceKasadakiParaMevcut.Value = KasadakiPara;
        }

        private void ucKasaHareketleri_Load(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste())
            {
                Close();
                return;
            }

            DataTable dt = DB.GetData("SELECT * FROM Kasalar with(nolock) where Aktif=1");
            lueKasa.Properties.DataSource = dt;
            lueKasa.Properties.ValueMember = "pkKasalar";
            lueKasa.Properties.DisplayMember = "KasaAdi";
            lueKasa.EditValue = 1;

            cbTarihAraligi.SelectedIndex = 0;

            btnListele_Click(sender, e);
            //ilkdate.DateTime = DateTime.Today;
            //sondate.DateTime = DateTime.Today;

            //Thread.Sleep(1000);
            //Thread islem = new Thread(new ThreadStart(Kasadakipara));
            //islem.Start();
            Kasadakipara();

            //Thread.Sleep(1000);
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            Thread islem = new Thread(new ThreadStart(DevirGetir));
            islem.Start();

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimYukle(gridView2, "KasaHareketleriGrid");
        }
        void DevirGetir()
        {
            DataTable dt = DB.GetData(@"select isnull(sum(kh.Borc-kh.Alacak),0)   from KasaHareket kh with(nolock)
                        where AktifHesap=1 and isnull(fkKasalar,0)>0 and Tarih<='" + ilkdate.DateTime.ToString("yyyy-MM-dd HH:mm")+
                        "' and (kh.fkSube is null or kh.fkSube="+Degerler.fkSube+")");

            ceDevir.Value = decimal.Parse(dt.Rows[0][0].ToString());
        }
        void kasahareketleri()
        {
            string sql = "";
            DataTable sanal = new DataTable();
            ArrayList listDevir = new ArrayList();
            ArrayList list = new ArrayList();
            sql = @"select 0 as pkKasaHareket,'Kasa Hareketi' as Tipi,'01.01.1900' as Tarih,'Devir' as Aciklama,isnull(sum(kh.Borc),0) as Borc,isnull(sum(kh.Alacak),0) as Alacak
                            from KasaHareket kh
                            where AktifHesap=1  and fkKasalar is null and Tarih < @ilktar";
            listDevir.Add(new SqlParameter("@ilktar", ilkdate.DateTime));
            
            DataTable dtDevir = DB.GetData(sql,listDevir);
//            sql = @"select kh.pkKasaHareket,0 as PkFirma, '' as Firmaadi,kh.Tarih,kh.Aciklama,isnull(kh.Borc,0) as Borc,isnull(kh.Alacak,0) as Alacak,'NAKİT' AS OdemeSekli
//                            from KasaHareket kh
//                            where AktifHesap=1 and fkKasalar=@fkKasalar and Tarih BETWEEN @ilktar and @sontar
//                            order by Tarih";
            sql = @"select * from (
select kh.pkKasaHareket,kh.fkKasalar,kh.AktifHesap,kh.OdemeSekli,'Personel Hareketleri' as Tipi,p.adi+' '+ p.soyadi as FirmaPersonel ,kh.Tarih,kh.Aciklama,kh.Borc,kh.Alacak from KasaHareket kh
inner join Personeller p on P.pkpersoneller=kh.fkPersoneller and kh.Modul=5
union all
select kh.pkKasaHareket,kh.fkKasalar,kh.AktifHesap,kh.OdemeSekli,'Cari Hareketleri' as Tipi,p.Firmaadi as FirmaPersonel ,kh.Tarih,kh.Aciklama,kh.Borc,kh.Alacak from KasaHareket kh
inner join Firmalar p on P.PkFirma=kh.fkPersoneller and kh.Modul=6
union all
select kh.pkKasaHareket,kh.fkKasalar,kh.AktifHesap,kh.OdemeSekli,'Kasa Hareketleri' as Tipi,'Kasa Hareketi' as FirmaPersonel ,kh.Tarih,kh.Aciklama,kh.Borc,kh.Alacak from KasaHareket kh
where  kh.Modul not in(5,6)) as k
where AktifHesap=1 and fkKasalar=@fkKasalar and Tarih BETWEEN @ilktar and @sontar";
            list.Add(new SqlParameter("@ilktar", ilkdate.DateTime));
            list.Add(new SqlParameter("@sontar", sondate.DateTime.AddHours(23).AddMinutes(59).AddSeconds(59)));
            list.Add(new SqlParameter("@fkKasalar", lueKasa.EditValue.ToString()));

            DataTable dt2 = DB.GetData(sql,list);
            sanal.Columns.Add(new DataColumn("pkKasaHareket", typeof(Int32)));
            sanal.Columns.Add(new DataColumn("FirmaPersonel", typeof(string)));
            sanal.Columns.Add(new DataColumn("Tipi", typeof(string)));
            sanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
            sanal.Columns.Add(new DataColumn("Aciklama", typeof(string)));
            sanal.Columns.Add(new DataColumn("Borc", typeof(float)));
            sanal.Columns.Add(new DataColumn("Alacak", typeof(float)));
            //sanal.Columns.Add(new DataColumn("OdemeSekli", typeof(string)));
            sanal.Columns.Add(new DataColumn("BakiyeB", typeof(float)));
            sanal.Columns.Add(new DataColumn("BakiyeA", typeof(float)));
            DataRow dr;
            float Borc=0;
            float Alacak=0;
            float Bakiye=0;
            //Devir
            if(dtDevir.Rows.Count>0)
            {
                Borc = float.Parse(dtDevir.Rows[0]["Borc"].ToString());
                Alacak = float.Parse(dtDevir.Rows[0]["Alacak"].ToString());
                Bakiye = Bakiye + (Borc - Alacak);
                dr = sanal.NewRow();
                dr["pkKasaHareket"] = dtDevir.Rows[0]["pkKasaHareket"];
                //dr["PkFirma"] = dtDevir.Rows[0]["PkFirma"];
                dr["Tipi"] = dtDevir.Rows[0]["Tipi"];
                dr["Tarih"] = dtDevir.Rows[0]["Tarih"];
                dr["Aciklama"] = dtDevir.Rows[0]["Aciklama"];
                dr["Borc"] = Borc;
                dr["Alacak"] = Alacak;
                //dr["OdemeSekli"] = dtDevir.Rows[0]["OdemeSekli"];
                if (Bakiye >= 0)
                    dr["BakiyeB"] = Bakiye;
                else
                    dr["BakiyeA"] = Bakiye;
                sanal.Rows.Add(dr);
            }
            //hareketler
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                Borc=float.Parse(dt2.Rows[i]["Borc"].ToString());
                Alacak = float.Parse(dt2.Rows[i]["Alacak"].ToString());
                Bakiye = Bakiye+(Borc-Alacak);
                dr = sanal.NewRow();
                dr["pkKasaHareket"] = dt2.Rows[i]["pkKasaHareket"];
                dr["FirmaPersonel"] = dt2.Rows[i]["FirmaPersonel"];
                dr["Tipi"] = dt2.Rows[i]["Tipi"];
                dr["Tarih"] = dt2.Rows[i]["Tarih"];
                dr["Aciklama"] = dt2.Rows[i]["Aciklama"];
                dr["Borc"] = Borc;
                dr["Alacak"] = Alacak;
                //dr["OdemeSekli"] = dt2.Rows[i]["OdemeSekli"];
                if (Bakiye >=0)
                dr["BakiyeB"] = Bakiye;
                else
                dr["BakiyeA"] = Bakiye;
                sanal.Rows.Add(dr);
            }
            //sonraki dönem bakiye
            if (cbOncekiGunlerDevir.Checked)
            {
                ArrayList listDevirSonraki = new ArrayList();
                ArrayList listsonraki = new ArrayList();
                sql = @"select 0 as pkKasaHareket, '0' as FirmaPersonel,'01.01.1900' as Tarih,'Sonraki Günlerin Toplamı' as Aciklama,isnull(sum(kh.Borc),0) as Borc,isnull(sum(kh.Alacak),0) as Alacak,'NAKİT' AS OdemeSekli
                            from KasaHareket kh
                            where AktifHesap=1 and fkKasalar is null and Tarih > @sontar";
                listDevirSonraki.Add(new SqlParameter("@sontar", sondate.DateTime));
                DataTable dtDevirSonraki = DB.GetData(sql, listDevirSonraki);
                Borc = float.Parse(dtDevirSonraki.Rows[0]["Borc"].ToString());
                Alacak = float.Parse(dtDevirSonraki.Rows[0]["Alacak"].ToString());
                if (Borc!= 0 && Alacak!=0)
                {
                    //Borc = float.Parse(dtDevirSonraki.Rows[0]["Borc"].ToString());
                    //Alacak = float.Parse(dtDevirSonraki.Rows[0]["Alacak"].ToString());
                    Bakiye = Bakiye + (Borc - Alacak);
                    dr = sanal.NewRow();
                    dr["pkKasaHareket"] = dtDevirSonraki.Rows[0]["pkKasaHareket"];
                   // dr["PkFirma"] = dtDevirSonraki.Rows[0]["PkFirma"];
                    dr["FirmaPersonel"] = dtDevirSonraki.Rows[0]["FirmaPersonel"];
                    dr["Tarih"] = dtDevirSonraki.Rows[0]["Tarih"];
                    dr["Aciklama"] = dtDevirSonraki.Rows[0]["Aciklama"];
                    dr["Borc"] = Borc;
                    dr["Alacak"] = Alacak;
                    //dr["OdemeSekli"] = dtDevirSonraki.Rows[0]["OdemeSekli"];
                    if (Bakiye >= 0)
                        dr["BakiyeB"] = Bakiye;
                    else
                        dr["BakiyeA"] = Bakiye;
                    sanal.Rows.Add(dr);
                }
            }
            gCPerHareketleri.DataSource = sanal;
        }

        void kasahareket(string gruplu)
        {
            string sql = "exec HSP_KasaHareketi @ilktar,@sontar,@fkSube,@devir";
            ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@gruplu", gruplu));
            list.Add(new SqlParameter("@ilktar", ilkdate.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sondate.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));
            list.Add(new SqlParameter("@devir", cbDevir.Checked));
            
            gCPerHareketleri.DataSource = DB.GetData(sql, list);

            DevirGetir();

            Kasadakipara();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if(gridView2.FocusedRowHandle<0)return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string fkCek = dr["fkCek"].ToString();
            string fkTaksitler = dr["fkTaksitler"].ToString();
            string fkFirma = dr["fkFirma"].ToString();

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

                //aktarıldı yapmak için
                if (fkFirma!="")
                DB.ExecuteSQL("update Firmalar set Aktarildi=0 where pkFirma=" + fkFirma);
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
            xrCariHareket rapor = new xrCariHareket();
            RaporDosyasi = exedizini + "\\Raporlar\\KasaHareketRaporu.repx";
            rapor.DataSource = gCPerHareketleri.DataSource;

            if (!File.Exists(RaporDosyasi))
            { 
                Print();
                return;
            }
            rapor.LoadLayout(RaporDosyasi);

            rapor.Name = "KasaHareketRaporu";
            //rapor.Report.Name = "KasaHareketRaporu.repx";

            rapor.FindControl("xlKasaAdi", true).Text = lueKasa.Text +" "+ ilkdate.DateTime.ToString("dd.MM.yyyy HH:mm")+
                "-" + sondate.DateTime.ToString("dd.MM.yyyy  HH:mm"); 
            if (Disigner)
                rapor.ShowDesignerDialog();
            else
                rapor.ShowPreview();
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
            printableLink.Component = gCPerHareketleri;

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
            kasahareketleri();
        }

        private void cbKasaGrup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbKasaGrup.SelectedIndex==1)
                gcTarih.Visible = false;
            else
                gcTarih.Visible = true;
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
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = 0;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            btnListele_Click(sender, e);
        }

        private void kasaÇıkışıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = 0;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();

            btnListele_Click(sender, e);
        }

        private void düzeltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();
            KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();
            KasaHareketDuzelt.Tag = this.Tag;
            KasaHareketDuzelt.ShowDialog();
            btnListele_Click(sender, e);
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
            kasahareket(cbKasaGrup.SelectedIndex.ToString());

            btnSil.Tag = "";
            btnSil.ToolTip = "";
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            if (dr["fkFirma"].ToString()!="")
            {
                frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
                CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
                CariHareketMusteri.Show();
            }
            else if (dr["fkTedarikciler"].ToString() != "")
            {
                frmTedarikcilerHareketleri tedatikciHakeket = new frmTedarikcilerHareketleri();
                tedatikciHakeket.musteriadi.Tag = dr["fkTedarikciler"].ToString();
                tedatikciHakeket.Show();
            }
            else if (dr["fkPersoneller"].ToString() != "")
            {
                frmPersonelHareketleri PersonellerHakeket = new frmPersonelHareketleri(dr["fkPersoneller"].ToString());
                PersonellerHakeket.musteriadi.Tag = dr["fkPersoneller"].ToString();
                PersonellerHakeket.Show();
            }
        }

        private void müşteriKArtıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
        }

        private void gridView2_EndSorting(object sender, EventArgs e)
        {
            //TODO:
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gCPerHareketleri, "A4");
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimKaydet(gridView2, "KasaHareketleriGrid");
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimSil("KasaHareketleriGrid");
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView2.ShowCustomization();
            gridView2.OptionsBehavior.AutoPopulateColumns = true;
            gridView2.OptionsCustomization.AllowColumnMoving = true;
            gridView2.OptionsCustomization.AllowColumnResizing = true;
            gridView2.OptionsCustomization.AllowQuickHideColumns = true;
            gridView2.OptionsCustomization.AllowRowSizing = true;
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            AppearanceDefault appLightSkyBlue = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appGreen = new AppearanceDefault(Color.LightGreen);
            AppearanceDefault appRed = new AppearanceDefault(Color.IndianRed);
            AppearanceDefault appRedOrg = new AppearanceDefault(Color.OrangeRed);

            DataRow dr = gridView2.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            if (dr["GrupAdi"].ToString() == "Müşteri")
            {
                AppearanceHelper.Apply(e.Appearance, appGreen);
            }
            if (dr["GrupAdi"].ToString() == "Tedarikçi")
            {
                AppearanceHelper.Apply(e.Appearance, appRed);
            }
            if (dr["GrupAdi"].ToString() == "Personel")
            {
                AppearanceHelper.Apply(e.Appearance, appRedOrg);
            }

        }
    }
}

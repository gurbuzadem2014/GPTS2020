using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using System.Threading;
using System.IO;
using GPTS.islemler;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraScheduler.Drawing;
using DevExpress.XtraScheduler;

namespace GPTS
{
    public partial class frmHatirlatHizmetSenet : DevExpress.XtraEditors.XtraForm
    {
        public frmHatirlatHizmetSenet()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        bool ilkyukle = false;
        private void frmCikis_Load(object sender, EventArgs e)
        {
            schedulerControl1.Start = DateTime.Today;
            cbTarihAraligi.SelectedIndex = 7;//bu yıl

            //dateNavigator1.DateTime = DateTime.Today;
            //dateNavigator1.TodayButton.PerformClick();
            HatirlatmaDurum();
            HatirlatmaDurumlar();
            //dizayn
            string Dosya = DB.exeDizini + "\\HatirlatmaHizmetSenetGrid.xml";
            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
            //Thread thread1 = new Thread(new ThreadStart(epostagonder));
            //thread1.Start();
            //xtraTabControl2.SelectedTabPageIndex = 1;

            label1.Text = "Gönderilecek E-Posta :  " + Degerler.eposta;
        }

        void HatirlatmaDurum()
        {
            lueHatirlatmaDurum.Properties.DataSource = DB.GetData(@"select 0 as pkHatirlatmaDurum,'Tümü' as durumu  
union all select pkHatirlatmaDurum,durumu from HatirlatmaDurum with(nolock)");
            lueHatirlatmaDurum.EditValue = 0;
        }

        void HatirlatmaDurumlar()
        {
            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from HatirlatmaDurum with(nolock)");
            repositoryItemLookUpEdit1.ValueMember = "pkHatirlatmaDurum";
            repositoryItemLookUpEdit1.DisplayMember = "durumu";
        }

        private void MusteriRandevuListesi()
        {
            string sql = @"select h.pkHatirlatma,h.Tarih, fkFirma,fkStokKarti,sk.Stokadi ,fkDurumu from Hatirlatma h with(nolock)
left join Firmalar f on f.pkFirma=h.fkFirma
left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
where tl.Tarih>'@ilktar' and tl.Tarih<'@sontar'";
            //where h.fkFirma=" + pkFirma.Text;

            gridControl1.DataSource = DB.GetData(sql);

        }

        void GunlukHatirlatmalar()
        {
            string sql = "";

            sql = @"select h.pkHatirlatma,h.Tarih, fkFirma,f.Firmaadi,f.Adres,f.Tel,f.Tel2,f.Cep,f.Devir,fkStokKarti,sk.Stokadi,
hd.durumu,h.fkDurumu,h.Aciklama,h.sira_no,isnull(h.arandi,0) as arandi,h.BitisTarihi  from Hatirlatma h with(nolock)
left join Firmalar f on f.pkFirma=h.fkFirma
left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
left join HatirlatmaDurum hd with(nolock) on hd.pkHatirlatmaDurum=h.fkDurumu
where h.Tarih>='@ilktar' and h.Tarih<='@sontar'";

            if (lueHatirlatmaDurum.EditValue != null && lueHatirlatmaDurum.EditValue.ToString() != "0")
                sql = sql + " and h.fkDurumu=" + lueHatirlatmaDurum.EditValue.ToString();

            sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm"));
            sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm"));
            gridControl1.DataSource = DB.GetData(sql);

            TimeSpan ts = ilktarih.DateTime - sontarih.DateTime;
            if (ts.Days == 0)
            {
                gridColumn1.DisplayFormat.FormatString = "HH:mm";
                gridColumn21.DisplayFormat.FormatString = "HH:mm";
            }
            else
            {
                gridColumn1.DisplayFormat.FormatString = "g";
                gridColumn21.DisplayFormat.FormatString = "g";
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            düzenleToolStripMenuItem_Click(sender, e);
        }

        private void ePostaGönderSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilenlere E-Posta Gönderilecek! Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;

            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));

                string pkHatirlatma = dr["pkHatirlatma"].ToString();

                DB.ExecuteSQL("update Hatirlatma set EpostaGonder=1 where pkHatirlatma=" + pkHatirlatma);
            }
            GunlukHatirlatmalar();
        }

        private void ePostaGönderSeçmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilenlere E-Posta Gönderilmeyecek! Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;

            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));

                string pkHatirlatma = dr["pkHatirlatma"].ToString();

                DB.ExecuteSQL("update Hatirlatma set EpostaGonder=0 where pkHatirlatma=" + pkHatirlatma);
            }
            GunlukHatirlatmalar();
        }

        void RandevulariYenile()
        {
            if (xtraTabControl2.SelectedTabPageIndex == 1)
                GorunumluHatirlatmalariGetir();
            else
                GunlukHatirlatmalar();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            RandevulariYenile();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(i);
            string fkFirma = dr["fkFirma"].ToString();
            frmMusteriHareketleri mh = new frmMusteriHareketleri();
            mh.musteriadi.Tag = fkFirma;
            mh.ShowDialog();

            GunlukHatirlatmalar();
            gridView1.FocusedRowHandle = i;
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;


            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkFirma = dr["fkFirma"].ToString();
            frmMusteriKarti mh = new frmMusteriKarti(fkFirma, "");
            mh.ShowDialog();

            GunlukHatirlatmalar();

            gridView1.FocusedRowHandle = i;
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(i);
            //string pkTaksitler = dr["pkTaksitler"].ToString();
            string fkFirma = dr["fkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkFirma;
            // KasaGirisi.pkTaksitler.Text = pkTaksitler;
            //KasaGirisi.tEaciklama.Text = dr["Tarih"].ToString() + "-Taksit Ödemesi-" + dr["Odenecek"].ToString();
            //KasaGirisi.ceTutarNakit.EditValue = dr["Odenecek"].ToString();
            //decimal kalan = 0;
            //decimal.TryParse(dr["Kalan"].ToString(), out kalan);
            //KasaGirisi.ceTutarNakit.Value = kalan;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            GunlukHatirlatmalar();

            gridView1.FocusedRowHandle = i;
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
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// Bu gün
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
            else if (cbTarihAraligi.SelectedIndex == 2)// yarın
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }

            else if (cbTarihAraligi.SelectedIndex == 3)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    7, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 5)// Bu ay
            {
                //sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                //                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 30, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 6)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 7)// bu yıl
            {
                //int ilkayfarki = DateTime.Now.AddMonths(-1).Month*-1;
                //sorguTarihAraligi(0, ilkayfarki, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                //                  365, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                                  0, 0, 1, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
                
            }
            else if (cbTarihAraligi.SelectedIndex == 8)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }

            RandevulariYenile();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\HatirlatmaHizmetSenetGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
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

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\HatirlatmaHizmetSenetGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }


        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkfirma = dr["fkFirma"].ToString();
            string pkHatirlatma = dr["pkHatirlatma"].ToString();
            DB.pkHatirlatma = int.Parse(pkHatirlatma);
            if (fkfirma == "") return;

            frmHatirlatma h = new frmHatirlatma(DateTime.Today, DateTime.Today, int.Parse(fkfirma));
            h.ShowDialog();

            GunlukHatirlatmalar();

            gridView1.FocusedRowHandle = i;
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            frmRandevuVer RandevuVer = new frmRandevuVer();
            if (schedulerControl1.SelectedAppointments.Count > 0)
            {
                DevExpress.XtraScheduler.Appointment apt = null;
                for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
                {
                    apt = schedulerControl1.SelectedAppointments[i];
                }
                RandevuVer.deRandevuZamani.DateTime = apt.Start;
                RandevuVer.txtPkHatirlatma.Text = "0";
                RandevuVer.deTarihi.DateTime = apt.Start;
            }
            else
            {
                RandevuVer.deRandevuZamani.DateTime = schedulerControl1.Start;
                RandevuVer.txtPkHatirlatma.Text = "0";
                RandevuVer.deTarihi.DateTime = schedulerControl1.Start;
            }
            
            RandevuVer.ShowDialog();

            RandevulariYenile();
        }

        private void bekliyorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string pkHatirlatma = dr["pkHatirlatma"].ToString();
                DB.ExecuteSQL("update Hatirlatma set fkDurumu=1 where pkHatirlatma=" + pkHatirlatma);
            }

            GunlukHatirlatmalar();
        }

        private void geldiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string pkHatirlatma = dr["pkHatirlatma"].ToString();
                DB.ExecuteSQL("update Hatirlatma set fkDurumu=2 where pkHatirlatma=" + pkHatirlatma);
            }

            GunlukHatirlatmalar();
        }

        private void gelmediToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string pkHatirlatma = dr["pkHatirlatma"].ToString();
                DB.ExecuteSQL("update Hatirlatma set fkDurumu=3 where pkHatirlatma=" + pkHatirlatma);
            }

            GunlukHatirlatmalar();
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            //Degerler.Renkler.Beyaz;
            if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "1")
                e.Appearance.BackColor = System.Drawing.Color.LightYellow;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "2")
                e.Appearance.BackColor = System.Drawing.Color.LightGreen;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "3")
                e.Appearance.BackColor = System.Drawing.Color.Red;

            //if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "1")
            //  e.Appearance.BackColor = System.Drawing.Color.Aqua;

            //if(gridColumn3
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "4")
            //    e.Appearance.BackColor = System.Drawing.Color.Green;
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "5")
            //    e.Appearance.BackColor = System.Drawing.Color.Blue;
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "6")
            //    e.Appearance.BackColor = System.Drawing.Color.Black;


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
            printableLink.Component = gridControl1;

            printableLink.Landscape = true;

            printableLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //printableLink.PrintingSystem.Document.AutoFitToPagesWidth = 1;

            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            yazdir();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            // GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);

            //if (ghi.RowHandle == -999997) return;
            //if (ghi.RowHandle == -2147483647) return;

            //if (ghi.InRowCell)
            //{
            //    int rowHandle = ghi.RowHandle;
            //    if (ghi.Column.FieldName == "Tutar")
            //    {
            //    }
            //}
        }

        private void repositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.ExecuteSQL("update Hatirlatma set fkDurumu=" + value + " where pkHatirlatma=" + dr["pkHatirlatma"].ToString());

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (xtraTabControl2.SelectedTabPageIndex == 1)
            {
                DevExpress.XtraScheduler.Appointment apt = null;
                for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
                {
                    apt = schedulerControl1.SelectedAppointments[i];
                }

                if (apt == null)
                {
                    formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                    return;
                }

                string id = apt.CustomFields["pkHatirlatma"].ToString();
                DB.pkHatirlatma = int.Parse(id);
                DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

                if (dt.Rows.Count == 0) return;

                frmRandevuVer RandevuVer = new frmRandevuVer();
                RandevuVer.txtPkHatirlatma.Text = id;
                RandevuVer.pkFirma.Text = dt.Rows[0]["fkfirma"].ToString();
                RandevuVer.ShowDialog();
            }
            else if (xtraTabControl2.SelectedTabPageIndex == 0)
            {

                int i = gridView1.FocusedRowHandle;

                if (i < 0) return;

                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string id = dr["pkHatirlatma"].ToString();
                DB.pkHatirlatma = int.Parse(id);
                //if (fkfirma == "") return;

                DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

                if (dt.Rows.Count == 0) return;
                frmRandevuVer rv = new frmRandevuVer();
                rv.pkFirma.Text = dt.Rows[0]["fkfirma"].ToString();
                rv.ShowDialog();
                gridView1.FocusedRowHandle = i;
            }

            RandevulariYenile();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
                simpleButton4.Enabled = false;
            else
                simpleButton4.Enabled = true;
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //listele butonu
            simpleButton3_Click(sender, e);
        }

        void GorunumluHatirlatmalariGetir()
        {
            //schedulerControl1.Start = ilktarih.DateTime;

            schedulerStorage1.Appointments.Mappings.Start = "StartTime";
            schedulerStorage1.Appointments.Mappings.End = "EndTime";
            schedulerStorage1.Appointments.Mappings.Subject = "Subject";
            schedulerStorage1.Appointments.Mappings.Description = "Description";
            schedulerStorage1.Appointments.Mappings.Location = "Cep";
            schedulerStorage1.Appointments.Mappings.Status = "fkDurumu";
            schedulerStorage1.Appointments.Mappings.Label = "Label";

            DevExpress.XtraScheduler.AppointmentCustomFieldMapping acfmCustomMappingID = new DevExpress.XtraScheduler.AppointmentCustomFieldMapping("pkHatirlatma", "pkHatirlatma");
            schedulerStorage1.Appointments.CustomFieldMappings.Add(acfmCustomMappingID);

            Yenile();
        }

        void Yenile()
        {
            string Sql = @"select pkHatirlatma,isnull(f.Firmaadi,'...')+' - '+isnull(sk.Stokadi,'..')+' - '+ISNULL(Aciklama,'.') as [Subject],Tarih as StartTime,
            BitisTarihi as EndTime,Konu as [Description],0 as AllDay,fkfirma, arandi as fkDurumu,
            case when fkDurumu=1 then 10
            when fkDurumu=2 then 8
            when fkDurumu=3 then 5 end Label1,fkDurumu as Label,f.Cep from Hatirlatma h with(nolock)
            left join Firmalar f with(nolock) on pkFirma=h.fkFirma
            left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
            where Tarih>='@ilktarih' and Tarih<='@sontarih'";

            Sql = Sql.Replace("@ilktarih", ilktarih.DateTime.ToString("yyy-MM-dd 00:00"));
            Sql = Sql.Replace("@sontarih", sontarih.DateTime.ToString("yyy-MM-dd 23:59"));
            schedulerStorage1.Appointments.DataSource = DB.GetData(Sql);
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            schedulerControl1.Start = DateTime.Today;
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Day;
            dateNavigator1.DateTime = DateTime.Today;
            RandevulariYenile();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            schedulerControl1.Start = DateTime.Today;
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.WorkWeek;

            RandevulariYenile();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            schedulerControl1.Start = DateTime.Today;
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Week;

            RandevulariYenile();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            schedulerControl1.Start = DateTime.Today;
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Month;

            RandevulariYenile();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            schedulerControl1.Start = DateTime.Today;
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Timeline;

            RandevulariYenile();
        }

        private void schedulerControl1_EditAppointmentFormShowing(object sender, DevExpress.XtraScheduler.AppointmentFormEventArgs e)
        {
            if (e.Appointment.CustomFields["pkHatirlatma"] == null)
            {
                DB.pkHatirlatma = 0;
                frmRandevuVer RandevuVer = new frmRandevuVer();
                RandevuVer.deRandevuZamani.DateTime = e.Appointment.Start;
                RandevuVer.deTarihi.Tag = "1";
                RandevuVer.deTarihi.DateTime = e.Appointment.Start;
                RandevuVer.ShowDialog();

            }
            else
            {
                btnRandevuDegis_Click(sender, e);
            }

            e.Handled = true;
            RandevulariYenile();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            panelControl2.Visible = true;
        }

        private void schedulerStorage1_AppointmentDeleting(object sender, DevExpress.XtraScheduler.PersistentObjectCancelEventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = (DevExpress.XtraScheduler.Appointment)e.Object;
            string id = apt.CustomFields["pkHatirlatma"].ToString();

            string sonuc = formislemleri.MesajBox("Randevu Silmek İstediğinize Eminmisiniz?", "Randevu Sil", 1, 2);
            if (sonuc == "0") return;

            DB.ExecuteSQL("delete from Hatirlatma where pkHatirlatma=" + id);

            GorunumluHatirlatmalariGetir();
        }

        private void schedulerStorage1_AppointmentChanging(object sender, DevExpress.XtraScheduler.PersistentObjectCancelEventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = e.Object as DevExpress.XtraScheduler.Appointment;
            string id = apt.CustomFields["pkHatirlatma"].ToString();
            if (id != "")
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@StartTime", apt.Start));
                list.Add(new SqlParameter("@EndTime", apt.End));
                list.Add(new SqlParameter("@pkHatirlatma", id));
                list.Add(new SqlParameter("@fkDurumu", apt.LabelId));

                DB.ExecuteSQL("update Hatirlatma set fkDurumu=@fkDurumu,Tarih=@StartTime,BitisTarihi=@EndTime where pkHatirlatma=@pkHatirlatma", list);
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            string s = @"Müşteriye Yeni Randevu Verme
1.Randevu Listesi Ekranından Açılır.
2.Randevu Listesi Ekranından Sağ tarafdaki Tarih Takviminden
Müşterinin Almak istediği Tarih seçilir.
3.Müşterinin Gelmek istediği saat çift tıklanarak Randevu Verme ekranı açılır.
4.Müşteri ve Hizmet seçilerek Randevu Oluştur butonuna basılır.

Müşteri Randevusu Düzenleme
1.Randevu listesi ekranı açılır.
2.Yeni Randevu butonuna basılır.
3.Müşteri Seçilir.
4.Sağ tarafdan Müşterinin değiştirmek istediği tarih seçilir.
5.Seans Listesinden,Boş olan saat aralığına sürüklenerek randevu değiştirilir.

Müşteri Randevu Silme
1.Randevu listesi ekranı açılır.
2.Yeni Randevu butonuna basılır.
3.Müşteri Seçilir.
5.Seans Listesinden,Silmek istediğiniz randevuyu sağ tıklayarak
Randevuyu Silebilirsiniz.

Not:Randevu Listesi ekranındaki Tarih Takvimi 30 günlük randevu gelmektedir.
Sorun Olursa 1 yılda yapılabilir.fakat yavaşlık olabilir.";

            MessageBox.Show(s);
        }

        private void schedulerControl1_PopupMenuShowing(object sender, DevExpress.XtraScheduler.PopupMenuShowingEventArgs e)
        {
            if (e.Menu.Id == DevExpress.XtraScheduler.SchedulerMenuItemId.AppointmentMenu)
            {
                // Hide the "Change View To" menu item.
                //DevExpress.XtraScheduler.SchedulerPopupMenu itemChangeViewTo = e.Menu.GetPopupMenuById(DevExpress.XtraScheduler.SchedulerMenuItemId.SwitchViewMenu);
                //itemChangeViewTo.Visible = false;

                //e.Menu.RemoveMenuItem(DevExpress.XtraScheduler.SchedulerMenuItemId.NewRecurringEvent);
                e.Menu.RemoveMenuItem(DevExpress.XtraScheduler.SchedulerMenuItemId.OpenAppointment);

            }
            DevExpress.Utils.Menu.DXMenuItem itemAc = new DevExpress.Utils.Menu.DXMenuItem("Detay Gör", new EventHandler(item_Click_RandevuAc));
            e.Menu.Items.Insert(0, itemAc);

            DevExpress.Utils.Menu.DXMenuItem itemA = new DevExpress.Utils.Menu.DXMenuItem("Arandı Yap", new EventHandler(item_Click_Arandi));
            e.Menu.Items.Insert(0, itemA);

            DevExpress.Utils.Menu.DXMenuItem item = new DevExpress.Utils.Menu.DXMenuItem("Sms Gönder", new EventHandler(item_Click_sms_gonder));
            e.Menu.Items.Insert(0, item);

            DevExpress.Utils.Menu.DXMenuItem iteme = new DevExpress.Utils.Menu.DXMenuItem("E-Posta Gönder", new EventHandler(item_Click_mail_gonder));
            e.Menu.Items.Insert(0, iteme);

            DevExpress.Utils.Menu.DXMenuItem item2 = new DevExpress.Utils.Menu.DXMenuItem("Müşteri Hareketleri", new EventHandler(item_Click_musteri_hareketleri));
            e.Menu.Items.Insert(0, item2);

            DevExpress.Utils.Menu.DXMenuItem item0 = new DevExpress.Utils.Menu.DXMenuItem("Randevu Düzenle", new EventHandler(item_Click_RandevuDuzenle));
            e.Menu.Items.Insert(0, item0);

            DevExpress.Utils.Menu.DXMenuItem itemYeni = new DevExpress.Utils.Menu.DXMenuItem("Yeni Özel Randevu", new EventHandler(item_Click_OzelRandevuEkle));
            e.Menu.Items.Insert(0, itemYeni);


            DevExpress.Utils.Menu.DXMenuItem itemCopy = new DevExpress.Utils.Menu.DXMenuItem("Kopyala", new EventHandler(item_Click_Copy));
            e.Menu.Items.Insert(0, itemCopy);

            DevExpress.Utils.Menu.DXMenuItem itemCut = new DevExpress.Utils.Menu.DXMenuItem("Kes", new EventHandler(item_Click_Cut));
            e.Menu.Items.Insert(0, itemCut);

            DevExpress.Utils.Menu.DXMenuItem itemPaste = new DevExpress.Utils.Menu.DXMenuItem("Yapıştır", new EventHandler(item_Click_Paste));
            e.Menu.Items.Insert(0, itemPaste);
        }

        void item_Click_OzelRandevuEkle(object sender, EventArgs e)
        {
            DateTime secilentarih = schedulerControl1.Start;
            DB.pkHatirlatma = 0;
            frmHatirlatma Hatirlat = new frmHatirlatma(schedulerControl1.SelectedInterval.Start, schedulerControl1.SelectedInterval.End, 0);
            Hatirlat.ShowDialog();

            GorunumluHatirlatmalariGetir();

            schedulerControl1.Start = secilentarih;
        }

        void item_Click_Copy(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma_Copy = int.Parse(id);
            DB.pkHatirlatma_Cut = 0;
        }

        void item_Click_Cut(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma_Cut = int.Parse(id);
            DB.pkHatirlatma_Copy = 0;
        }

        void item_Click_Paste(object sender, EventArgs e)
        {
            //hafızada varsa 
            if (DB.pkHatirlatma_Cut > 0)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", schedulerControl1.SelectedInterval.Start));
                //list.Add(new SqlParameter("@Konu", konu));
                //list.Add(new SqlParameter("@Aciklama", "Kopya"));
                //list.Add(new SqlParameter("@Uyar", "1"));
                list.Add(new SqlParameter("@BitisTarihi", schedulerControl1.SelectedInterval.End));
                //list.Add(new SqlParameter("@fkFirma", fkFirma));
                //list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                //list.Add(new SqlParameter("@fkDurumu", "1"));
                list.Add(new SqlParameter("@pkHatirlatma", DB.pkHatirlatma_Cut));

                DB.GetData("update Hatirlatma set Tarih=@Tarih,BitisTarihi=@BitisTarihi where pkHatirlatma=@pkHatirlatma", list);

                GorunumluHatirlatmalariGetir();
                return;
            }

            if (DB.pkHatirlatma_Copy > 0)
            {
                DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + DB.pkHatirlatma_Copy);
                
                string konu = dt.Rows[0]["Konu"].ToString();
                string fkFirma = dt.Rows[0]["fkFirma"].ToString();
                string fkStokKarti = dt.Rows[0]["fkStokKarti"].ToString();

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", schedulerControl1.SelectedInterval.Start));
                list.Add(new SqlParameter("@Konu", konu));
                list.Add(new SqlParameter("@Aciklama", "Kopya"));
                list.Add(new SqlParameter("@Uyar", "1"));
                list.Add(new SqlParameter("@BitisTarihi", schedulerControl1.SelectedInterval.End));
                list.Add(new SqlParameter("@fkFirma", fkFirma));
                list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                list.Add(new SqlParameter("@fkDurumu", "1"));

                DB.GetData("insert into Hatirlatma (Tarih,Konu,Aciklama,Uyar,BitisTarihi,fkFirma,fkStokKarti,fkDurumu)" +
                    " values(@Tarih,@Konu,@Aciklama,@Uyar,@BitisTarihi,@fkFirma,@fkStokKarti,@fkDurumu)", list);

                GorunumluHatirlatmalariGetir();
            }
            

        }

        void item_Click_RandevuAc(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma = int.Parse(id);
            //DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

            DateTime secilentarih = schedulerControl1.Start;

            frmHatirlatma Hatirlat = new frmHatirlatma(secilentarih, secilentarih, 0);
            Hatirlat.ShowDialog();

            GorunumluHatirlatmalariGetir();

            schedulerControl1.Start = secilentarih;
        }

        void item_Click_RandevuDuzenle(object sender, EventArgs e)
        {
            btnRandevuDegis_Click(sender, e);

            //DevExpress.XtraScheduler.Appointment apt = null;
            //for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            //{
            //    apt = schedulerControl1.SelectedAppointments[i];
            //}

            //if (apt == null)
            //{
            //    formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
            //    return;
            //}

            //string id = apt.CustomFields["pkHatirlatma"].ToString();
            //frmRandevuVer rv = new frmRandevuVer();
            //rv.txtPkHatirlatma.Text = id;
            //rv.ShowDialog();
        }

        void item_Click_Arandi(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.ExecuteSQL("update Hatirlatma  set arandi=1 where pkHatirlatma=" + id);

            RandevulariYenile();
        }

        void item_Click_sms_gonder(object sender, EventArgs e)
        {
            //int h = 0;
            //DataTable dt = DB.GetData("select pkFirma,Cep,Cep2 from Firmalar with(nolock) where sec=1 and (len(Cep)>9 or len(Cep2)>9)");//pkFirma=" + pkFirma);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string fkFirma = dt.Rows[i]["pkFirma"].ToString();
            //    string cep = dt.Rows[i]["Cep"].ToString();
            //    string cep2 = dt.Rows[i]["Cep2"].ToString();
            //    if (cep.Length < 9) cep = cep2;

            //    if (cep.Length < 9)
            //    {
            //        h++;
            //        continue;
            //    }

            //    ArrayList list = new ArrayList();
            //    list.Add(new SqlParameter("@fkFirma", fkFirma));
            //    list.Add(new SqlParameter("@CepTel", cep));
            //    list.Add(new SqlParameter("@Mesaj", "Bakiye"));

            //    if (DB.GetData("select fkFirma from Sms where Durumu=0 and fkFirma=" + fkFirma).Rows.Count == 0)
            //    {
            //        string sonuc = DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CepTel,Durumu,Mesaj,Tarih) values(@fkFirma,@CepTel,0,@Mesaj,GetDate())", list);
            //        if (sonuc == "0")
            //            DB.ExecuteSQL("update Firmalar set sec=0 where pkFirma=" + fkFirma);
            //    }
            //    else
            //        DB.ExecuteSQL("update Firmalar set sec=0 where pkFirma=" + fkFirma);
            //}

            frmSmsGonder SmsGonder = new frmSmsGonder();
            SmsGonder.Show();
        }

        void item_Click_mail_gonder(object sender, EventArgs e)
        {
            frmEpostaGonder mailGonder = new frmEpostaGonder();
            mailGonder.Show();
        }

        void item_Click_musteri_hareketleri(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma = int.Parse(id);
            DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

            if (dt.Rows.Count == 0) return;

            string fkfirma = dt.Rows[0]["fkfirma"].ToString();
            frmMusteriHareketleri mh = new frmMusteriHareketleri();
            mh.musteriadi.Tag = fkfirma;
            mh.ShowDialog();
        }

        //Boolean selectedDateChanged = false;
        private void dateNavigator1_EditDateModified(object sender, EventArgs e)
        {
            //selectedDateChanged = true;
        }

        private void schedulerControl1_VisibleIntervalChanged(object sender, EventArgs e)
        {
            //if (selectedDateChanged)
            //{
            //    DateTime start = new DateTime(dateNavigator1.SelectionStart.Year, dateNavigator1.SelectionStart.Month, dateNavigator1.SelectionStart.Day,
            //        schedulerControl1.TimelineView.WorkTime.Start.Hours, schedulerControl1.TimelineView.WorkTime.Start.Minutes, schedulerControl1.TimelineView.WorkTime.Start.Seconds);
            //    dateNavigator1.SchedulerControl.Start = start;

            // 
            //}

            //ilktarih.DateTime = dateNavigator1.SelectionStart;
            //sontarih.DateTime = dateNavigator1.SelectionEnd;

            //Yenile();

            //selectedDateChanged = false;
        }

        private void btnRandevuDegis_Click(object sender, EventArgs e)
        {
            if (xtraTabControl2.SelectedTabPageIndex == 1)
            {
                DevExpress.XtraScheduler.Appointment apt = null;
                for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
                {
                    apt = schedulerControl1.SelectedAppointments[i];
                }

                if (apt == null)
                {
                    formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                    return;
                }

                string id = apt.CustomFields["pkHatirlatma"].ToString();
                DB.pkHatirlatma = int.Parse(id);
                DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

                //if (dt.Rows.Count == 0) return;
                frmRandevuVer rv = new frmRandevuVer();
                rv.pkFirma.Text = dt.Rows[0]["fkfirma"].ToString();
                rv.txtPkHatirlatma.Text = id;
                rv.ShowDialog();
            }
            else if (xtraTabControl2.SelectedTabPageIndex == 0)
            {

                int i = gridView1.FocusedRowHandle;

                if (i < 0) return;

                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string id = dr["pkHatirlatma"].ToString();
                DB.pkHatirlatma = int.Parse(id);
                //if (fkfirma == "") return;

                DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

                if (dt.Rows.Count == 0) return;
                frmRandevuVer rv = new frmRandevuVer();
                rv.pkFirma.Text = dt.Rows[0]["fkfirma"].ToString();
                rv.ShowDialog();
                gridView1.FocusedRowHandle = i;
            }

            RandevulariYenile();
        }

        private void schedulerControl1_CustomDrawAppointment(object sender, DevExpress.XtraScheduler.CustomDrawObjectEventArgs e)
        {
            if (((SchedulerControl)sender).ActiveView is DayView)
            {
                AppointmentViewInfo viewInfo = e.ObjectInfo as AppointmentViewInfo;
                // Get DevExpress images.
                //Image im = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/add_16x16.png");

                //Rectangle imageBounds = new Rectangle(viewInfo.InnerBounds.X, viewInfo.InnerBounds.Y, im.Width, im.Height);
                //Rectangle mainContentBounds = new Rectangle(viewInfo.InnerBounds.X, viewInfo.InnerBounds.Y + im.Width + 1,
                //viewInfo.InnerBounds.Width, viewInfo.InnerBounds.Height - im.Height - 1);
                //// Draw image in the appointment.
                //e.Cache.Graphics.DrawImage(im, imageBounds);
            }


        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using System.IO;

namespace GPTS
{
    public partial class frmStokGirisCikislari : DevExpress.XtraEditors.XtraForm
    {
        public frmStokGirisCikislari()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        
        private void frmStokGirisCikislari_Load(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 4;

            string Dosya = DB.exeDizini + "\\StokGirisCikisGrid.xml";
            if(File.Exists(Dosya))
               gridView1.RestoreLayoutFromXml(Dosya);

            Liste();
        }

        private void Liste()
        {
            string sql = @"select SK.pkStokKarti,SK.Barcode,SK.Stokadi,SK.SatisFiyati,SK.AlisFiyati,SK.KritikMiktar,
SK.Mevcut,GirisMiktar,CikisMiktar,SG.StokGrup,SAG.StokAltGrup
 from StokKarti SK with(nolock)
left join StokGruplari SG with(nolock) on SG.pkStokGrup=SK.fkStokGrup
left join StokAltGruplari SAG with(nolock) on SAG.pkStokAltGruplari=SK.fkStokAltGruplari
left join (select fkStokKarti,count(*) as GirisMiktar From SatisDetay with(nolock) where Tarih>'@ilktarih' and Tarih<'@sontarih' group by fkStokKarti) SD  on SD.fkStokKarti = SK.pkStokKarti
left join (select fkStokKarti,count(*) as CikisMiktar From AlisDetay with(nolock) where Tarih>'@ilktarih' and Tarih<'@sontarih' group by fkStokKarti) AD on AD.fkStokKarti = SK.pkStokKarti  
";

            sql = "exec sp_StokgirisCikislari '@ilktarih','@sontarih'";

            sql = sql.Replace("@ilktarih", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm"));
            sql = sql.Replace("@sontarih", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm"));

            gridControl1.DataSource = DB.GetData(sql);
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Liste();
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

                sorguTarihAraligi((-DateTime.Now.Day), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
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
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            yazdir();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;


            DataRow dr = gridView1.GetDataRow(i);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
            


            
            

           // frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            //StokHareketleri.fkStokKarti.Text = dr["StokAdi"].ToString();
            //StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            //StokHareketleri.ShowDialog();

            Liste();

            gridView1.FocusedRowHandle=i;
        }

        private void stokKartiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(i);

            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            StokKarti.ShowDialog();

            Liste();

            gridView1.FocusedRowHandle = i;
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\StokGirisCikisGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\StokAraGrid.xml";
            if (File.Exists(Dosya))
            {
                //for (int i = 0; i < gridView1.Columns.Count; i++)
                //    gridView1.Columns[i].Visible = true;
                File.Delete(Dosya);

                Close();
            }
        }
    }
}
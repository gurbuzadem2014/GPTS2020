using System;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using DevExpress.Data.Filtering;
using DevExpress.XtraScheduler.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraScheduler.Localization;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.OleDb;
using GPTS.Include;
using GPTS.Include.Data;
using System.Configuration;
using DevExpress.XtraPrinting.Links;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraCharts.Printing;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using System.IO;
using DevExpress.XtraPrinting.Control;

namespace GPTS
{
    public partial class frmPersonelMaasOdemesi : DevExpress.XtraEditors.XtraForm
    {
        //private DataTable dt;

        public frmPersonelMaasOdemesi()
        {
            InitializeComponent();
            cBEYil.Text = DateTime.Today.ToString("yyyy");
            cBDonem.Text = DateTime.Today.ToString("MM");
        }



        private void PersonelList()
        {
            try 
            {
                string sql = @"select pkpersoneller,adi+' '+soyadi as adisoyadi,maasi,agiucreti,yolucreti,isegiristarih,
sum(kh.Borc) as Borc,
sum(kh.Alacak) as Alacak, 
sum(kh.Borc) - sum(kh.Alacak) as Kalan
from Personeller p with(nolock)
left join KasaHareket kh with(nolock) on kh.fkPersoneller = p.pkpersoneller
group by  pkpersoneller,adi,soyadi,maasi,agiucreti,yolucreti,isegiristarih";
                //                @"select convert(bit,0) as sec,p.pkpersoneller,pro.ProjeAdi,p.adi+' '+p.soyadi as adisoyadi,p.isegiristarih,isnull(kh.Borc,0) as MaasTahakkuk,isnull(khavans.Borc,0) as Avans,
                //isnull(khkesinti.Borc,0) as Kesinti,isnull(khMesai.Borc,0) as Mesai,isnull(khMaasOdeme.Borc,0) as MaasOdeme,isnull(khAgi.Borc,0) as AgiOdemesi,
                //(isnull(kh.Borc,0)-isnull(khavans.Borc,0)-isnull(khkesinti.Borc,0)+isnull(khMesai.Borc,0)-isnull(khMaasOdeme.Borc,0)+isnull(khAgi.Borc,0)) as  Kalan from Personeller p
                //left join (select * from Projeler) pro on pro.pkProjeler=p.fkgrup
                //left join (select fkPersoneller,sum(Borc) as Borc from KasaHareket where Modul=5 and Tipi=3  and yil=@yil and donem=@donem Group By fkPersoneller) kh on kh.fkPersoneller=p.pkpersoneller
                //left join (select fkPersoneller,sum(Alacak) as Borc from KasaHareket where Modul=5 and Tipi=1  and yil=@yil and donem=@donem Group By fkPersoneller) khavans on khavans.fkPersoneller=p.pkpersoneller
                //left join (select fkPersoneller,sum(Alacak) as Borc from KasaHareket where Modul=5 and Tipi=9  and yil=@yil and donem=@donem Group By fkPersoneller) khkesinti on khkesinti.fkPersoneller=p.pkpersoneller
                //left join (select fkPersoneller,sum(Borc) as Borc from KasaHareket where Modul=5 and Tipi=4  and yil=@yil and donem=@donem Group By fkPersoneller) khMesai on khMesai.fkPersoneller=p.pkpersoneller
                //left join (select fkPersoneller,sum(Alacak) as Borc from KasaHareket where Modul=5 and Tipi=5  and yil=@yil and donem=@donem Group By fkPersoneller) khMaasOdeme on khMaasOdeme.fkPersoneller=p.pkpersoneller
                //left join (select fkPersoneller,sum(Borc) as Borc from KasaHareket where Modul=5 and Tipi=8  and yil=@yil and donem=@donem Group By fkPersoneller) khAgi on khAgi.fkPersoneller=p.pkpersoneller
                //where p.AyrilisTarihi is null";
               // sql = sql.Replace("@yil",cBEYil.Text);
               // sql = sql.Replace("@donem", cBDonem.Text);
//exec PersonelDonemMaasHakedisi " +cBDonem.Text+","+cBEYil.Text;
                //if(cESifir.Checked==false)
                  // sql+= " and (isnull(kh.Borc,0)-isnull(khavans.Borc,0)-isnull(khkesinti.Borc,0)+isnull(khMesai.Borc,0)-isnull(khMaasOdeme.Borc,0)+isnull(khAgi.Borc,0))<>0";
                gridControl2.DataSource = DB.GetData(sql);
                //sehirler
            }
            catch (Exception ex)
            {

            }

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string personel_id = dr["pkpersoneller"].ToString();

            frmPersonel personelkart = new frmPersonel(personel_id);
            personelkart.ShowDialog();
            //System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //pkmusteri.Text = row["pkpersoneller"].ToString();
            //personelkart.pkpersoneller = int.Parse(row["pkpersoneller"].ToString());
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            //gridView1_DoubleClick(sender, e);

            //frmPersonel personelkart = new frmPersonel();
            System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //string configConnection = ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString;
            if (row == null) return;
            DB.pkPersoneller= int.Parse(row["pkpersoneller"].ToString());
            //personelkart.pkpersoneller = int.Parse(row["pkpersoneller"].ToString());
            //personelkart.ShowDialog();
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            PersonelList(); 
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["sec"].ToString() == "True")
                {
                    string sql = @"select COUNT(*) as c from KasaHareket 
                        where Modul=5 and Tipi=5 and yil=@yil and donem=@donem and fkPersoneller=@personel";

                    sql = sql.Replace("@yil", cBEYil.EditValue.ToString());
                    sql = sql.Replace("@donem", cBDonem.EditValue.ToString());
                    sql = sql.Replace("@personel", dr["pkpersoneller"].ToString());
                    DataTable dtc = DB.GetData(sql);
                    if (dtc.Rows[0][0].ToString() != "0")
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Personel Maaş Ödemesi Daha Önce Yapılmıştır!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //DataTable dt2 = DB.GetData(@"select count(*) from [KasaHareket] where Modul=5 and Tipi=5 and fkpersoneller=" + pkpersoneller);
                    //if (dt2.Rows.Count == 0)
                    //{
                    //    return;
                    //}
                    sql = @"INSERT INTO KasaHareket (fkkasalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,aciklama,donem,yil,Odendi,AktifHesap)
                    values(@fkkasalar,@fkPersoneller,getdate(),5,5,0,@Alacak,'@aciklama',@donem,@yil,0,1)";
                    sql = sql.Replace("@fkkasalar", "1");
                    sql = sql.Replace("@fkPersoneller", dr["pkpersoneller"].ToString());
                    sql = sql.Replace("@Alacak",dr["Kalan"].ToString().Replace(",", "."));
                    sql = sql.Replace("@aciklama", "Maaş Ödemesi");
                    sql = sql.Replace("@yil", cBEYil.EditValue.ToString());
                    sql = sql.Replace("@donem", cBDonem.EditValue.ToString());
                    DB.ExecuteSQL(sql);
                }
            }

        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("sec"), checkEdit1.Checked);
            }
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
            printableLink.Component = gridControl2;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
        string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
        string RaporDosyasi = exedizini + "\\Raporlar\\PersonelMaasOdemeleri.repx";
        DevExpress.XtraReports.UI.XtraReport rapor = new DevExpress.XtraReports.UI.XtraReport();
        rapor.LoadLayout(RaporDosyasi);
        rapor.DataSource = gridControl2.DataSource;
        rapor.ShowRibbonPreview();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = exedizini + "\\Raporlar\\PersonelMaasOdemeleri.repx";
            DevExpress.XtraReports.UI.XtraReport rapor = new DevExpress.XtraReports.UI.XtraReport();
            rapor.LoadLayout(RaporDosyasi);
            rapor.DataSource = gridControl2.DataSource;
            //rapor.ShowRibbonDesigner();
            rapor.ShowDesigner();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            DB.pkPersoneller = int.Parse(dr["pkpersoneller"].ToString());
            //gridView2.ViewCaption = "Adı Soyadı :" + dr["adi"].ToString() + " " + dr["soyadi"].ToString();
        }

        private void cBDonem_SelectedIndexChanged(object sender, EventArgs e)
        {
            PersonelList(); 
        }

        private void cESifir_CheckedChanged(object sender, EventArgs e)
        {
            PersonelList(); 
        }
    }
}
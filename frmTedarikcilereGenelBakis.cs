using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using GPTS.islemler;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmTedarikcilereGenelBakis : DevExpress.XtraEditors.XtraForm
    {
        public frmTedarikcilereGenelBakis()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        private void ucStokKontrol_Load(object sender, EventArgs e)
        {
            gridyukle("");

            lUEKullanicilar.Properties.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where len(eposta)>2 and durumu=1 ");
        }
        void gridyukle(string kosul)
        {
            string sql = "exec sp_TedarikcilereGenelBakis";
            gridControl1.DataSource = DB.GetData(sql);
        }


        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            yazdir();
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
            if (xtraTabControl1.SelectedTabPageIndex==0)
            printableLink.Component = gridControl1;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmTedarikciKarti KurumKarti = new frmTedarikciKarti(dr["pkTedarikciler"].ToString());
            DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());
            KurumKarti.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            inputForm sifregir = new inputForm();
            sifregir.Girilen.PasswordChar = '#';
            sifregir.ShowDialog();

            if (sifregir.Girilen.Text != "9999") return;

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tedarikçi Bakiyeleri Sıfırlanacak, Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string bakiye = dr["Bakiye"].ToString();
                string pkTedarikciler = dr["pkTedarikciler"].ToString();
                decimal bak = 0;
                decimal.TryParse(bakiye, out bak);

                if(dr["Sec"].ToString()=="True")
                {
                    if (bak!=0)
                    {
                        string sql = @"INSERT INTO KasaHareket (Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar)
             values(getdate(),3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,0,@pkTedarikciler,'Bakiye Düzeltme',@Tutar)";
                        ArrayList list0 = new ArrayList();
                        //list0.Add(new SqlParameter("@fkKasalar", "1"));//int.Parse(lueKasa.EditValue.ToString())));
                        if (bak < 0)
                        {
                            list0.Add(new SqlParameter("@Borc", bak.ToString().Replace("-", "").Replace(",", ".")));
                            list0.Add(new SqlParameter("@Alacak", "0"));
                        }
                        else
                        {
                            list0.Add(new SqlParameter("@Borc", "0"));
                            list0.Add(new SqlParameter("@Alacak", bak));
                        }
                        list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
                        list0.Add(new SqlParameter("@Aciklama", "Bakiye Sıfırlandı."));
                        list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
                        list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
                        list0.Add(new SqlParameter("@pkTedarikciler", pkTedarikciler));
                        list0.Add(new SqlParameter("@AktifHesap", "0"));
                        list0.Add(new SqlParameter("@Tutar",  bak.ToString().Replace(",", ".")));
                        
                        string sonuc = DB.ExecuteSQL(sql, list0);
                        if (sonuc != "0")
                        {
                            //ceBakiye.Value = 0;
                            // MessageBox.Show("Hata Oluştu Tekrar deneyiniz");
                            return;
                        }
                    }
                }
            }
            gridyukle("");
            DevExpress.XtraEditors.XtraMessageBox.Show("Tedarikçi Bakiyeleri Sıfırlandı.", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Information);
             
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
                gridView1.SetRowCellValue(i, "Sec", checkEdit1.Checked);
        }

        void FisYazdir(bool Disigner)
        {
            string sql = @"exec sp_TedarikcilereGenelBakis";

            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable dtliste = DB.GetData(sql);
                //DataTable dtliste = (DataTable)gridControl1.DataSource;
                dtliste.TableName = "dtliste";
                ds.Tables.Add(dtliste);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\TedarikciBorcListesi.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                XtraReport rapor = new XtraReport();
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "TedarikciBorcListesi";
                rapor.Report.Name = "TedarikciBorcListesi";
                rapor.DataSource = ds;
                
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();
                ds.Dispose();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            FisYazdir(false);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FisYazdir(true);
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmTedarikcilerHareketleri CariHareketMusteri = new frmTedarikcilerHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkTedarikciler"].ToString();
            CariHareketMusteri.ShowDialog();
        }

        private void bakiyeDevirGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(i);

            DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());

            frmTedarikciBakiyeDuzeltme DevirBakisiSatisKaydi = new frmTedarikciBakiyeDuzeltme();
            DevirBakisiSatisKaydi.ShowDialog();

            gridyukle("");
            //gridView3.FocusedRowHandle = i;
            //gridView3.SelectRow(i);
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void sbExcelGonder_Click(object sender, EventArgs e)
        {
            gridView1.ExportToXls(Application.StartupPath + "\\BorcluTedarikci.Xls");
            Process.Start(Application.StartupPath + "\\BorcluTedarikci.Xls");
            //Process.Start(Application.StartupPath);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue == null)
            {
                lUEKullanicilar.Focus();
                return;
            }
            string dosyaadi = Application.StartupPath + "\\BorcluTedarikci" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
            if (lUEKullanicilar.EditValue.ToString().Length > 10)
            {
                gridView1.ExportToXls(Application.StartupPath + "\\BorcluTedarikci.Xls");
                DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Borçlu Tedarikçi Listesi", dosyaadi, "Borçlu Tedarikci");
                frmMesajBox mesaj = new frmMesajBox(200);
                mesaj.label1.Text = "E-Posta Gönderildi.";
                mesaj.Show();
            }
            else
            {
                MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
                lUEKullanicilar.Focus();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKullanicilar Kullanicilar = new frmKullanicilar();
            Kullanicilar.ShowDialog();
        }


        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());

            frmTedarikciKarti KurumKarti = new frmTedarikciKarti(dr["pkTedarikciler"].ToString());
            KurumKarti.ShowDialog();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(lUEKullanicilar.EditValue.ToString() + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            string RaporDosyasi = DB.exeDizini + "\\Raporlar\\TedarikciBorcListesi.repx";
            if (!File.Exists(RaporDosyasi))
            {
                MessageBox.Show("TedarikciBorcListesi.repx Dosya Bulunamadı");
                return;
            }

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(RaporDosyasi);
            rapor.Name = "TedarikciBorcListesi";
            rapor.Report.Name = "TedarikciBorcListesi.repx";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string dosyaadi = Application.StartupPath + "\\TedarikciBorcListesi.pdf";
                rapor.DataSource = DB.GetData("exec sp_TedarikcilereGenelBakis");

                //rapor.FilterString = "[ID]=1";
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Tedarikciler Borç Listesi", dosyaadi, "Tedarikciler Borç Listesi");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            string mb = formislemleri.MesajBox("Tedarikçi Bakiyeri Yenilenecektir. Eminmisiniz?", "Tedarikçi Bakiyeleri Yenile", 1, 0);
            if (mb == "0") return;

            string s = btnYenile.Text;

            DataTable dt = DB.GetData("select pkTedarikciler from Tedarikciler with(nolock)");
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                Application.DoEvents();
                btnYenile.Text = i.ToString();

                DB.ExecuteSQL("update Tedarikciler set Devir=dbo.fon_TedarikciBakiyesi(" + dr["pkTedarikciler"].ToString()
                    + ") WHERE pkTedarikciler =" + dr["pkTedarikciler"].ToString());
                i++;
            }
        }
    }
}

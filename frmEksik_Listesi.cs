using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using GPTS.islemler;
using System.IO;

namespace GPTS
{
    public partial class frmEksik_Listesi : DevExpress.XtraEditors.XtraForm
    {
        public frmEksik_Listesi()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        void gridyukle()
        {
            gridControl2.DataSource = DB.GetData(@"select isnull(BirimFiyati,AlisFiyati) as BirimFiyati,* from EksikListesi el with(nolock)
left join (
select pkStokKarti,Barcode,Stokadi,b.BirimAdi,sk.AlisFiyati as AlisFiyati,sk.SatisFiyati,sk.Mevcut,sg.StokGrup,Alis.Firmaadi as TedarikciAdi,Alis.SonAlisTarihi,sd.SonOtuzGunSatisMiktari from StokKarti sk with(nolock)
left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
left join (select Max(t.Firmaadi) as FirmaAdi,Max(a.Tarih) as SonAlisTarihi,ad.fkStokKarti from Alislar a with(nolock)
left join Tedarikciler t with(nolock) on t.pkTedarikciler=a.fkFirma
left join AlisDetay ad with(nolock) on a.pkAlislar=ad.fkAlislar
where Siparis=1
group by fkStokKarti ) as Alis on Alis.fkStokKarti=sk.pkStokKarti
left join (select fkStokKarti,sum(Adet) as SonOtuzGunSatisMiktari from SatisDetay with(nolock) where Tarih between getdate()-30 and getdate() group by fkStokKarti) sd  on sd.fkStokKarti=sk.pkStokKarti
left join Birimler b with(nolock) on b.pkBirimler=sk.fkBirimler
group by pkStokKarti,Barcode,Stokadi,b.BirimAdi,sk.AlisFiyati,sk.SatisFiyati,sk.Mevcut,sg.StokGrup,Alis.Firmaadi,Alis.SonAlisTarihi,sd.SonOtuzGunSatisMiktari) as skad on skad.pkStokKarti=el.fkStokKarti
");
        }

        void gridyukleKritik()
        {
            gridControl3.DataSource = DB.GetData(@"select pkStokKarti,Barcode,Stokadi,Stoktipi,AlisFiyati,SatisFiyati,EklemeTarihi,Mevcut,KritikMiktar  from StokKarti with(nolock)
            where Mevcut<=KritikMiktar and Aktif=1 order by Mevcut");
        }
       
        private void frmAvans_Load(object sender, EventArgs e)
        {
            gridyukle();

            lUEKullanicilar.Properties.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where eposta is not null");
            lUEKullanicilar.ItemIndex = 0;

            lueTedarikci.Properties.DataSource = DB.GetData("select * from Tedarikciler with(nolock) where Eposta is not null");
            lueTedarikci.ItemIndex = 0;

            //DataTable dtp = DB.GetData(@"select * from Personeller where pkpersoneller=" + DB.pkPersoneller.ToString());
            //pkmusteri.Text = dtp.Rows[0]["pkpersoneller"].ToString();
            //teAdi.Text = dtp.Rows[0]["adi"].ToString();
            //teSoyadi.Text = dtp.Rows[0]["soyadi"].ToString();
            //tETel.Text = dtp.Rows[0]["tel"].ToString();
            ////tEMaxavans.Text = dtp.Rows[0]["maxavans"].ToString();

            //DataTable dtkh = DB.GetData("select isnull(max(donem),MONTH(GETDATE())) as donem,YEAR(GETDATE()) as yil from KasaHareket where fkPersoneller=" + DB.pkPersoneller.ToString());
            //cBDonem.EditValue = dtkh.Rows[0][0].ToString();
            //cBYil.EditValue = dtkh.Rows[0][1].ToString();
            //simpleButton1_Click(sender,e);
            ///maasödemesi yapıldıysa sonraki döneme geç
            ///
            EkranYetkileriGetir();

            string Dosya = DB.exeDizini + "\\EksikListesiGrid.xml";
            
            if (File.Exists(Dosya))
            {
                gridView2.RestoreLayoutFromXml(Dosya);
                gridView2.ActiveFilter.Clear();
            }
        }

        void EkranYetkileriGetir()
        {
            string sql = @"SELECT ya.Yetki, p.Aciklama10,ya.Sayi,isnull(p.Aktif,0) as Aktif FROM  YetkiAlanlari ya with(nolock)  
            INNER JOIN Parametreler p with(nolock) ON ya.fkParametreler = p.pkParametreler
            WHERE p.fkModul =1 and ya.fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);

            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string aciklama = dtYetkiler.Rows[i]["Aciklama10"].ToString();
                bool yetki = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                string sayi = dtYetkiler.Rows[i]["Sayi"].ToString();
                //bool aktif = Convert.ToBoolean(dtYetkiler.Rows[i]["Aktif"]);

                if (aciklama == "AlisFiyati")
                {
                    if (yetki == false)
                    {
                        gridColumn10.Visible = yetki;
                        //AlisFiyati.Properties.PasswordChar = '*';
                        //AlisFiyatiKdvHaric.Properties.PasswordChar = '*';
                        //ceAlisFiyatiiskontolu.Properties.PasswordChar = '*';
                        //txtbirincifiyatorani.Properties.PasswordChar = '*';
                        //txikincifiyatyuzde.Properties.PasswordChar = '*';
                        //txtbirincifiyatorani.Properties.PasswordChar = '*';
                    }
                }
            }
        }

        private void xtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (e.Page.Name == "xtraTabPage1") 
                gridyukle();
             else  if (e.Page.Name == "xTabKritikStoklar") 
                gridyukleKritik();
        }

        private void frmAvans_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            else if (e.KeyCode == Keys.F2)
            {
                btnSil_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                btnTumunuSil_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F8)
            {
                urunaraekle();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            
        }

        private void gridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                btnSil_Click(sender, e);
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
        
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gridControl2;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;
            string miktar =
               ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
            
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            //miktar = dr["Miktar"].ToString();
           DB.ExecuteSQL("UPDATE EksikListesi SET Miktar=" + miktar.Replace(",",".")
            + " where pkEksikListesi=" + dr["pkEksikListesi"].ToString());
        }

        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnEkle.Focus();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        void urunaraekle()
        {
            //YeniSatisEkle();
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "0";
            StokAra.ShowDialog();
            if (StokAra.TopMost == false)
            {
                //StokAra.gridView1.FilterDisplayText("Starts with([Sec], True)";
                //for (int i = 0; i < StokAra.gridView1.DataRowCount; i++)
                //{



                 for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();
                    //if (StokAra.gridView1.GetSelectedRows()[i] >= 0)
                    //{
                        DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                        //AlisDetayEkle(dr["Barcode"].ToString());
                    //}
                
                    //DataRow dr = StokAra.gridView1.GetDataRow(i);
                    //if (dr["Sec"].ToString() == "True")
                    //{
                        if (DB.GetData("select StokAdi from EksikListesi el with(nolock) where fkStokKarti=" + dr["pkStokKarti"].ToString()).Rows.Count == 0)
                        {
                            ArrayList list = new ArrayList();
                            list.Add(new SqlParameter("@StokAdi", dr["StokAdi"].ToString()));
                            list.Add(new SqlParameter("@FirmaAdi", dr["FirmaAdi"].ToString()));
                            list.Add(new SqlParameter("@fkStokKarti", dr["pkStokKarti"].ToString()));
                            DB.ExecuteSQL(@"INSERT INTO EksikListesi (StokAdi,Miktar,FirmaAdi,fkStokKarti,Durumu,Tarih)
                        VALUES(@StokAdi,1,@FirmaAdi,@fkStokKarti,'Alınacak',getdate())", list);
                        }
                        else
                        {
                            frmMesajBox Mesaj = new frmMesajBox(200);
                            Mesaj.label1.Text = "Daha Önce Eklendi!";
                            Mesaj.Show();
                        }
                    //}
                        //SatisDetayEkle(dr["Barcode"].ToString());
                    //SatisDetayEkle(StokAra.pkurunid.Text);
                }
            }
            //SatisDetayEkle(StokAra.pkurunid.Text);
            StokAra.Dispose();
            gridyukle();
            //yesilisikyeni();
        }
        
        private void simpleButton21_Click_1(object sender, EventArgs e)
        {
            Close();
        }
        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gridControl2;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }
        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            //gridColumn8.Visible = false;
            //gridColumn9.Visible = false;
            yazdir();
            //gridColumn8.Visible = true;
            //gridColumn9.Visible = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@StokAdi", txtStokAdi.Text));
            list.Add(new SqlParameter("@Miktar", cMiktar.EditValue.ToString().Replace(",",".")));
            list.Add(new SqlParameter("@BirimFiyati", cBirimFiyat.EditValue.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Aciklama", tctAciklama.Text));

            DB.ExecuteSQL("INSERT INTO EksikListesi (StokAdi,Miktar,FirmaAdi,Aciklama,fkStokKarti,BirimFiyati,Tarih,Durumu)" +
                " values(@StokAdi,@Miktar,'',@Aciklama,0,@BirimFiyati,getdate(),'Alınacak')", list);
           
            gridyukle();

            txtStokAdi.Text = "";
            tctAciklama.Text = "";
            cMiktar.Value = 1;
            cBirimFiyat.Value = 0;
            txtStokAdi.Focus();
        }

        private void txtStokAdi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tctAciklama.Focus();
            }
        }

        private void tctAciklama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cBirimFiyat.Focus();
            }
        }

        private void cMiktar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEkle.Focus();
            }
        }

        private void cBirimFiyat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cMiktar.Focus();
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.ExecuteSQL("delete from EksikListesi where pkEksikListesi=" + dr["pkEksikListesi"].ToString());
            gridView2.DeleteSelectedRows();
        }

        private void btnTumunuSil_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)   return;
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                DB.ExecuteSQL("delete from EksikListesi where pkEksikListesi=" + dr["pkEksikListesi"].ToString());
            }
            gridyukle(); 
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (lUEKullanicilar.EditValue.ToString().Length > 10)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(lUEKullanicilar.EditValue.ToString() + " E-Posta Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;

                gridView2.ExportToXls(Application.StartupPath + "\\EksikListesi.Xls");
                DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Eksik Listesi", Application.StartupPath + "\\EksikListesi.Xls", "Eksik Listesi");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            else
            {
                MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
                lUEKullanicilar.Focus();
            }
        }

        private void stokKartınıDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (dr["pkStokKarti"].ToString() == "")
            {
                formislemleri.Mesajform(dr["StokAdi"].ToString() + " için Stok Kartı tanımlanmamıştır.", "K", 200);
                return;
            }
            frmStokKarti StokKarti = new frmStokKarti();
            StokKarti.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string fkFirma = "1";
            GridHitInfo ghi = gridView2.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            if (ghi.RowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(ghi.RowHandle);
            if (ghi.Column.FieldName == "StokAdi")
            {
                if (dr["pkStokKarti"].ToString() == "") return;
                frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay(fkFirma, "", "");
                SatisUrunBazindaDetay.Tag = "3";
                SatisUrunBazindaDetay.pkStokKarti.Text = dr["pkStokKarti"].ToString();
                SatisUrunBazindaDetay.ShowDialog();
            }
        }
        private void btnStokBul_Click(object sender, EventArgs e)
        {
            urunaraekle();
        }
        void Yazdir(bool dizayn)
        {
            xrCariHareket rapor = new xrCariHareket();
            //XtraReport rapor = new XtraReport();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\EksikListesi.repx");
            rapor.Name = "EksikListesi";
            rapor.Report.Name = "EksikListesi.repx";

            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                //string pkKasaHareket = "0";
                //if (gridView1.FocusedRowHandle >= 0)
                //{
                //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                //    pkKasaHareket = dr["pkKasaHareket"].ToString();
                //}
                // string sql = "select * from KasaHareket where pkKasaHareket=" + pkKasaHareket;
                rapor.DataSource = gridControl2.DataSource;
                //DB.GetData(sql);
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }
        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            Yazdir(false);
        }

        private void fişYazdırDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdir(true);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (lueTedarikci.EditValue.ToString().Length > 10)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(lueTedarikci.EditValue.ToString() + " E-Posta Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;

                gridView2.ExportToXls(Application.StartupPath + "\\EksikListesi.Xls");
                DB.epostagonder(lueTedarikci.EditValue.ToString(), "Eksik Listesi", Application.StartupPath + "\\EksikListesi.Xls", "Eksik Listesi");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            else
            {
                MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
                lueTedarikci.Focus();
            }
        }

        private void alındıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (dr["pkStokKarti"].ToString() == "") return;
                        
               DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());


               DB.ExecuteSQL("update EksikListesi set Durumu='Alındı' where pkEksikListesi=" + dr["pkEksikListesi"].ToString());

               gridyukle();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(lUEKullanicilar.EditValue.ToString() + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\EksikListesi.repx");
            rapor.Name = "EksikListesi";
            rapor.Report.Name = "EksikListesi.repx";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string dosyaadi = Application.StartupPath + "\\EksikListesi.pdf";
                rapor.DataSource = gridControl2.DataSource;
                //rapor.FilterString = "[ID]=1";
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Eksik Listesi", dosyaadi, "Eksik Listesi");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length < 10) return;

            string dosya = DB.exeDizini + "\\Raporlar\\EksikListesi2.repx";
            if (!File.Exists(dosya))
            {
                MessageBox.Show(dosya +" Dosya Bulunamadı");
                return;
            }
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(dosya);
            rapor.Name = "EksikListesi2";
            rapor.Report.Name = "EksikListesi2";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string dosyaadi = Application.StartupPath + "\\EksikListesi2.pdf";
                rapor.DataSource = gridControl2.DataSource;
                //rapor.FilterString = "[ID]=1";
                //rapor.ExportToPdf(dosyaadi);
                //if (dizayn)
                  //  rapor.ShowDesigner();
                //else
                    rapor.ShowPreview();
                //DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Eksik Listesi", dosyaadi, "Eksik Listesi");

                //formislemleri.Mesajform("E-Posta Gönderildi.", "S");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (lueTedarikci.EditValue.ToString().Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(lueTedarikci.EditValue.ToString() + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\EksikListesi.repx");
            rapor.Name = "EksikListesi";
            rapor.Report.Name = "EksikListesi.repx";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string dosyaadi = Application.StartupPath + "\\EksikListesi.pdf";
                rapor.DataSource = gridControl2.DataSource;
                //rapor.FilterString = "[ID]=1";
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(lueTedarikci.EditValue.ToString(), "Eksik Listesi", dosyaadi, "Eksik Listesi");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void eksikListesineEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

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
                //Showmessage("Hata Oluştu.", "K");
            }
        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            if (dr["pkStokKarti"].ToString() == "") return;

            frmStokKarti StokKarti = new frmStokKarti();
            StokKarti.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();

            gridyukleKritik();
        }

        private void gridView3_EndSorting(object sender, EventArgs e)
        {
            if (gridView3.DataRowCount > 0)
                gridView3.FocusedRowHandle = 0;
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\EksikListesiGrid.xml";
            gridView2.SaveLayoutToXml(Dosya);

            
            //EksikListesiGrid
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\EksikListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }
    }
}
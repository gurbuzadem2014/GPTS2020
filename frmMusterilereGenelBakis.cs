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
using System.Threading;
using GPTS.islemler;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmMusterilereGenelBakis : DevExpress.XtraEditors.XtraForm
    {
        //frmYukleniyor loading = null;
        //bool Aktif = false;
        //public Thread say;

        public frmMusterilereGenelBakis()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        public void StartKronometre()
        {
            //DateTime zaman = DateTime.Now;
            //while (Aktif == true)
            //{
            //    loading.Show();
            //    loading.Refresh();
            //    TimeSpan aralik = DateTime.Now - zaman;
            //    Thread.Sleep(20);
            //}
        }

        private void ucStokKontrol_Load(object sender, EventArgs e)
        {
            //loading = new frmYukleniyor();
            //Aktif = true;
            ////System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            //say = new Thread(new ThreadStart(StartKronometre));
            //say.Start();

            ////Application.DoEvents();
            ////DataTable dtf= DB.GetData("select * from Firmalar where Aktif=1");

            ////for (int i = 0; i < dtf.Rows.Count; i++)
            ////{
            ////    DB.ExecuteSQL("update Firmalar set Devir=dbo.fon_MusteriBakiyesi(" + dtf.Rows[i]["pkFirma"].ToString() + ")");
            ////    Application.DoEvents();
            ////}

            ////MusteriBakiyeleriniGuncelle();

            ////Thread.Sleep(10);
            ////Aktif = false;
            ////loading.Show();
            ////loading.Close();
            //DB.ExecuteSQL("sp_job_MusteriBakiye");

            gridyukle("");

            //Thread.Sleep(10);
            //Aktif = false;
            //loading.Show();
            //loading.Close();

            bonus();
            BonusHareketleri();

            lUEKullanicilar.Properties.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where len(eposta)>2 and durumu=1 ");


            string Dosya = DB.exeDizini + "\\MusterilereGenelBakisGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
            
        }

        void gridyukle(string kosul)
        {
            string sql = "exec sp_MusterilereGenelBakis";
            gridControl1.DataSource = DB.GetData(sql);
        }

        void bonus()
        {
            gridControl2.DataSource=
            DB.GetData(@"SELECT Firmalar.pkFirma, Firmalar.Firmaadi, Firmalar.Bonus, Firmalar.Alacak, Firmalar.Devir, Firmalar.Borc, sum(BonusKullanilan.Bonus) AS BonusKul
FROM         Firmalar with(nolock)
LEFT OUTER JOIN  BonusKullanilan ON Firmalar.pkFirma = BonusKullanilan.fkFirma
group by Firmalar.pkFirma, Firmalar.Firmaadi, Firmalar.Bonus, Firmalar.Alacak, Firmalar.Devir, Firmalar.Borc");
        }

        void BonusHareketleri()
        {
            gridControl4.DataSource =
            DB.GetData(@"SELECT   BonusKullanilan.pkBonusKullanilan, BonusKullanilan.fkFirma, BonusKullanilan.Bonus, BonusKullanilan.Tarih, BonusKullanilan.fkSatislar, 
                      Firmalar.Firmaadi, Firmalar.Borc, Firmalar.Alacak, Firmalar.Bonus AS Bonusu,Firmalar.OzelKod
FROM BonusKullanilan with(nolock) 
INNER JOIN  Firmalar with(nolock) ON BonusKullanilan.fkFirma = Firmalar.pkFirma");
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
            else if (xtraTabControl1.SelectedTabPageIndex == 1)
                printableLink.Component = gridControl2;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            gridControl3.DataSource = DB.GetData("select * from BonusKullanilan where fkFirma=" + dr["pkFirma"].ToString());
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            KurumKarti.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            inputForm sifregir = new inputForm();
            sifregir.Girilen.PasswordChar = '#';
            sifregir.ShowDialog();
            if (sifregir.Girilen.Text != "9999*") return;

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Bakiyeleri Sıfırlanacak, Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string bakiye = dr["Bakiye"].ToString();
                string pkFirma = dr["pkFirma"].ToString();
                decimal bak = 0;
                decimal.TryParse(bakiye, out bak);

                if(dr["Sec"].ToString()=="True")
                {
                    if (bak!=0)
                    {
                        string sql = @"INSERT INTO KasaHareket (Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,fkSube)
             values(getdate(),3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Bakiye Düzeltme',@Tutar,@fkSube)";
                        ArrayList list0 = new ArrayList();
                        //list0.Add(new SqlParameter("@fkKasalar", "1"));//int.Parse(lueKasa.EditValue.ToString())));
                        if (bak < 0)
                        {
                            list0.Add(new SqlParameter("@Alacak", bak.ToString().Replace("-","").Replace(",",".")));
                            list0.Add(new SqlParameter("@Borc", "0"));
                        }
                        else
                        {
                            list0.Add(new SqlParameter("@Alacak", "0"));
                            list0.Add(new SqlParameter("@Borc", bak));
                        }
                        list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
                        list0.Add(new SqlParameter("@Aciklama", "Bakiye Sıfırlandı."));
                        list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
                        list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
                        list0.Add(new SqlParameter("@fkFirma", pkFirma));
                        list0.Add(new SqlParameter("@AktifHesap", "0"));
                        list0.Add(new SqlParameter("@Tutar", bak.ToString().Replace(",",".")));
                        list0.Add(new SqlParameter("@fkSube", Degerler.fkSube));
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
            DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Bakiyeleri Sıfırlandı.", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Information);
             
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
                gridView1.SetRowCellValue(i, "Sec", checkEdit1.Checked);
        }

        void FisYazdir(bool Disigner)
        {
            string sql = "";
            //@"select convert(bit,'1') as Sec,f.pkFirma,Firmaadi,OzelKod,Tel,Cep,Adres,
                //satis.SatisTutari,isnull(kh.Borc,0),isnull(kh.Alacak,0),
                //(isnull(kh.Alacak,0)+satis.SatisTutari+f.Devir)-isnull(kh.Borc,0) as Bakiye from Firmalar f
                //inner join 
                //(select s.fkFirma,sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as SatisTutari from Satislar s
                //inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar group by s.fkFirma)  satis
                //on satis.fkFirma=f.pkFirma
                //left join (select fkFirma,sum(Borc) as Borc,SUM(Alacak) as Alacak from KasaHareket group by fkFirma) kh on kh.fkFirma=f.pkFirma
                //where isnull(kh.Borc,0)-(isnull(kh.Alacak,0)+satis.SatisTutari+f.Devir)<>0";

            sql = "exec sp_MusterilereGenelBakis";
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable dtliste = (DataTable)gridControl1.DataSource;//DB.GetData(sql);
                if (gridView1.Columns["Firmaadi"].FilterInfo.Value != null)
                {
                    string aranan = gridView1.Columns["Firmaadi"].FilterInfo.Value.ToString();
                    dtliste = dtliste.Select("Firmaadi like'%" + aranan + "%'").CopyToDataTable();
                }
                    dtliste.TableName = "dtliste";
                ds.Tables.Add(dtliste);
                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\MusteriBorcListesi.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("MusteriBorcListesi.repx Dosya Bulunamadı");
                    return;
                }
                XtraReport rapor = new XtraReport();
               
                
                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriBorcListesi";

                rapor.Report.Name = "MusteriBorcListesi";

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

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.ShowDialog();
        }

        private void bakiyeDevirGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(i);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            string pkFirma = dr["pkFirma"].ToString();

            frmMusteriBakiyeDuzeltme DevirBakisiSatisKaydi = new frmMusteriBakiyeDuzeltme(pkFirma);
            DevirBakisiSatisKaydi.ShowDialog();

            gridyukle("");
            gridView3.FocusedRowHandle = i;
            gridView3.SelectRow(i);
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void sbExcelGonder_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\BorcluMusteriler" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
            gridView1.ExportToXls(dosyaadi);
            Process.Start(dosyaadi);
            //Process.Start(Application.StartupPath);
        }

        void BorcluMusterileriEpostaileGonder()
        {
            if (lUEKullanicilar.EditValue.ToString().Length > 10)
            {
                string dosyaadi = Application.StartupPath + "\\BorcluMusteriler" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";

                gridView1.ExportToXls(dosyaadi);
                DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Borçlu Müşteri Listesi", dosyaadi, "Borçlu Müsteriler");
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
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue == null)
            {
                lUEKullanicilar.Focus();
                return;
            }

            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            
            Thread epostegonder = new Thread(new ThreadStart(BorcluMusterileriEpostaileGonder));
            epostegonder.Start();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKullanicilar Kullanicilar = new frmKullanicilar();
            Kullanicilar.ShowDialog();
        }

        void SatisFiyatGruplariGetir()
        {
            gridControl5.DataSource = DB.GetData(@"SELECT CONVERT(bit,'1') as Sec,pkFirma, FirmaAdi,sfb.Baslik as SatisFiyatGrubu,OzelKod,sfb.pkSatisFiyatlariBaslik FROM  Firmalar with(nolock)
            INNER JOIN SatisFiyatlariBaslik sfb   with(nolock) ON sfb.pkSatisFiyatlariBaslik=fkSatisFiyatlariBaslik");
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 3)
                SatisFiyatGruplariGetir();
            if (xtraTabControl1.SelectedTabPageIndex == 4)
                MusteriOdemeGunleri();
            
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;
            int i=gridView5.FocusedRowHandle;
            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());

            frmMusteriKarti MusteriKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            
            MusteriKarti.ShowDialog();
            SatisFiyatGruplariGetir();
            gridView5.FocusedRowHandle = i;
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());

            frmMusteriKarti MusteriKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            MusteriKarti.ShowDialog();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            string mb = formislemleri.MesajBox("Müşteri Bakiyeri Yenilenecektir. Eminmisiniz?", "Müşteri Bakiyeleri Yenile", 1, 0);
            if (mb == "0") return;

            string s = btnYenile.Text;
            
            DataTable dt = DB.GetData("select pkFirma from Firmalar with(nolock)");
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                Application.DoEvents();
                btnYenile.Text = i.ToString();

                DB.ExecuteSQL("update Firmalar set Devir=dbo.fon_MusteriBakiyesi(" + dr["pkFirma"].ToString() 
                    + ") WHERE pkFirma =" + dr["pkFirma"].ToString());
                i++;
            }
            btnYenile.Text = s;
            //DB.ExecuteSQL("exec sp_job_MusteriBakiye");

            //MusteriBakiyeleriniGuncelle();
            
            //for (int i = 0; i < length; i++)
            //{
                
            //}

            gridyukle("");
        }

        private void smsGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //string pkFirma = dr["pkFirma"].ToString();
            int h=0;
            DataTable dt = DB.GetData("select pkFirma,Cep,Cep2 from Firmalar with(nolock) where sec=1 and (len(Cep)>9 or len(Cep2)>9)");//pkFirma=" + pkFirma);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string fkFirma=dt.Rows[i]["pkFirma"].ToString();
                string cep = dt.Rows[i]["Cep"].ToString();
                string cep2 = dt.Rows[i]["Cep2"].ToString();
                if (cep.Length < 9) cep = cep2;

                if (cep.Length < 9)
                {
                    h++;
                    continue;
                }

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma",fkFirma));
                list.Add(new SqlParameter("@CepTel", cep));
                list.Add(new SqlParameter("@Mesaj", "Bakiye"));

                if (DB.GetData("select fkFirma from Sms where Durumu=0 and fkFirma=" + fkFirma).Rows.Count == 0)
                {
                    string sonuc = DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CepTel,Durumu,Mesaj,Tarih) values(@fkFirma,@CepTel,0,@Mesaj,GetDate())", list);
                    if (sonuc == "0")
                        DB.ExecuteSQL("update Firmalar set sec=0 where pkFirma=" + fkFirma);
                }
                else
                    DB.ExecuteSQL("update Firmalar set sec=0 where pkFirma=" + fkFirma);
            }

            //if(h>0)
            //  MessageBox.Show( h.ToString() + " Hatalı Cep No Bilgisilerini Kontrol Ediniz!");
            
            frmSmsGonder SmsGonder = new frmSmsGonder();
            SmsGonder.Show();

            gridyukle("");
        }

        private void gridView6_DoubleClick(object sender, EventArgs e)
        {
            if (gridView6.FocusedRowHandle < 0) return;
            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());

            frmMusteriKarti MusteriKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            MusteriKarti.ShowDialog();

            MusteriOdemeGunleri();
        }
        void MusteriOdemeGunleri()
        {
            gridControl6.DataSource = DB.GetData(@"SELECT pkFirma, Firmaadi,Adres,OzelKod,Tel,Cep, Devir,Aktif,
                (datediff(dd,SonOdemeTarihi,GETDATE())) as OdemeYapmadigiGun, OdemeGunSayisi,SonOdemeTarihi FROM Firmalar with(nolock)");
        }

        private void gridView6_EndSorting(object sender, EventArgs e)
        {
            if (gridView6.DataRowCount > 0)
                gridView6.FocusedRowHandle = 0;
        }

        private void repositoryItemCheckEdit1_CheckStateChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string sec = ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (sec == "False")
            {
                DB.ExecuteSQL("update Firmalar Set sec=0 where pkFirma=" + dr["pkFirma"].ToString());
            }
            else
            {
                DB.ExecuteSQL("update Firmalar Set sec=1 where pkFirma=" + dr["pkFirma"].ToString());
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(lUEKullanicilar.EditValue.ToString() + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            
            string RaporDosyasi = DB.exeDizini + "\\Raporlar\\MusteriBorcListesi.repx";
            if (!File.Exists(RaporDosyasi))
            {
                MessageBox.Show("MusteriBorcListesi.repx Dosya Bulunamadı");
                return;
            }

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(RaporDosyasi);
            rapor.Name = "MusteriBorcListesi";
            rapor.Report.Name = "MusteriBorcListesi.repx";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string dosyaadi = Application.StartupPath + "\\MusteriBorcListesi.pdf";
                rapor.DataSource = DB.GetData("exec sp_MusterilereGenelBakis");

                //rapor.FilterString = "[ID]=1";
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Müşteri Borç Listesi", dosyaadi, "Müşteri Borç Listesi");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //string pkFirma = dr["pkFirma"].ToString();
            //lblSecilenMusteri.Text = dr["Firmaadi"].ToString();

            //DataTable dtOzelNot = DB.GetData("select * from FirmaOzelNot with(nolock) where fkFirma=" + pkFirma);

            //if (dtOzelNot.Rows.Count>0)
            //    memoozelnot.Text = dtOzelNot.Rows[0]["OzelNot"].ToString();

        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();

            KasaGirisi.pkFirma.Text = dr["pkFirma"].ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            gridView1.FocusedRowHandle = i;

            gridyukle("");
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int i= e.FocusedRowHandle;

            if (i < 0) return;
            
            DataRow dr = gridView1.GetDataRow(i);

            string pkFirma = dr["pkFirma"].ToString();

            lblSecilenMusteri.Text = dr["Firmaadi"].ToString();

            DataTable dtOzelNot = DB.GetData("select * from FirmaOzelNot with(nolock) where fkFirma=" + pkFirma);

            if (dtOzelNot.Rows.Count == 0)
                memoozelnot.Text = "";
            else
                memoozelnot.Text = dtOzelNot.Rows[0]["OzelNot"].ToString();
        }

        private void tümünüSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO:filter yap

            if (tümünüSeçToolStripMenuItem.CheckState == CheckState.Unchecked)
                tümünüSeçToolStripMenuItem.Checked = true;
            else
                tümünüSeçToolStripMenuItem.Checked = false;

            for (int i = 0; i < gridView1.DataRowCount; i++)
			{
                string sec = tümünüSeçToolStripMenuItem.CheckState.ToString();

                if (sec == "Checked") sec = "1";
                else
                    sec = "0";

                DataRow dr = gridView1.GetDataRow(i);
			    DB.ExecuteSQL("update Firmalar Set Sec="+sec+" where pkFirma=" + dr["pkFirma"].ToString());
			}

            gridyukle("");
        }

        private void hatırlatmaEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i= gridView1.FocusedRowHandle;

            if (i < 0) return;
            
            DataRow dr = gridView1.GetDataRow(i);

            string pkFirma = dr["pkFirma"].ToString();

            frmHatirlatmaUzat Hatirlat = new frmHatirlatmaUzat(DateTime.Now, DateTime.Now, int.Parse(pkFirma));
            DB.pkHatirlatmaAnimsat = 0;
            Hatirlat.ShowDialog();

            gridView1.FocusedRowHandle=i;
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\MusterilereGenelBakisGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\MusterilereGenelBakisGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void gütünAlanlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }
    }
}

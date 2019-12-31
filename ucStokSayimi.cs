using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Data.OleDb;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraReports.UI;
using GPTS.Include.Data;
using DevExpress.XtraTab;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using GPTS.islemler;

namespace GPTS
{
    public partial class ucStokSayimi : DevExpress.XtraEditors.XtraUserControl
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis="";//AccessibleDescription
        int AcikSatisindex = 1;//hangi satış açık
        decimal HizliMiktar = 1;

      public ucStokSayimi()
      {
        InitializeComponent();
        DB.PkFirma = 1;
      }
      void KullaniciListesi()
      {
          lueKullanicilar.Properties.DataSource = DB.GetData("select pkKullanicilar,adisoyadi,KullaniciAdi from Kullanicilar with(nolock)");
          lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
      }
      void FisListesi()
      {
          string sql = @"select pkSayim,s.ilkTarih,s.bitistarih,s.Aciklama,k.KullaniciAdi from Sayim s
LEFT JOIN Kullanicilar k ON s.fkKullanici=k.pkKullanicilar
 order by pkSayim desc";//where s.bitistarih is not null 
            lueFis.Properties.DataSource = DB.GetData(sql);
      }
      private void ucAnaEkran_Load(object sender, EventArgs e)
      {
          Yetkiler();

          KullaniciListesi();
          
          Depolar();

          timer1.Enabled = true;

          SayimListesi(0);

          SayimDetayGetir();

          tasarimlar();

      }
        void tasarimlar()
        {
            string Dosya = DB.exeDizini + "\\Satism1Grid.xml";

            if (File.Exists(Dosya))
            {
                gridView2.RestoreLayoutFromXml(Dosya);
                gridView2.ActiveFilter.Clear();
            }

            string Dosya2 = DB.exeDizini + "\\Satism2Grid.xml";

            if (File.Exists(Dosya2))
            {
                gridView1.RestoreLayoutFromXml(Dosya2);
                gridView1.ActiveFilter.Clear();
            }
        }

        void Depolar()
        {
            lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock) where Aktif=1");
            lueDepolar.EditValue = Degerler.fkDepolar;
        }

      void Showmessage(string lmesaj,string renk)
      {
          frmMesajBox mesaj = new frmMesajBox(200);
          mesaj.label1.Text = lmesaj;
          if (renk=="K")
              mesaj.label1.BackColor = System.Drawing.Color.Red;
          else if (renk == "B")
              mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
          else
              mesaj.label1.BackColor = System.Drawing.Color.Blue;
          mesaj.Show();
      }
      void Yetkiler()
      {
          string sql = @"SELECT   YetkiAlanlari.Yetki, YetkiAlanlari.Sayi, Parametreler.Aciklama10, YetkiAlanlari.BalonGoster
FROM  YetkiAlanlari INNER JOIN Parametreler ON YetkiAlanlari.fkParametreler = Parametreler.pkParametreler
WHERE  Parametreler.fkModul = 1  and YetkiAlanlari.fkKullanicilar =" + DB.fkKullanicilar;
          DataTable dtYetkiler = DB.GetData(sql);
          for (int i = 0; i < dtYetkiler.Rows.Count; i++)
          {
              //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
              //    xtraTabControl2.Visible = true;
                  //timer1.Enabled = true;
              //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "GizliAlan")
              //{
                  //if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                  //    dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                  //else
              //        dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
              //}
          }
      }

        int x = 0;
        int y = 0;
        int p = 1;
        private void ButtonClick(object sender, EventArgs e)
        {
            //gridView1.AddNewRow();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            //gridView1.SetFocusedRowCellValue(gridColumn1, "1");
            //gridView1.SetFocusedRowCellValue(gridColumn1, "");

            if (((SimpleButton)sender).Tag != null)
            {
                SatisDetayEkle(((SimpleButton)sender).Tag.ToString());
                yesilisikyeni();
            }
                //urunekle(((SimpleButton)sender).Tag.ToString());
            //gridView1.Focus();
            //gridView1.AddNewRow();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            //gridView1.SetFocusedRowCellValue(gridColumn1, "1");
            //gridView1.SetFocusedRowCellValue(gridColumn1, "");
            //SendKeys.Send("{ENTER}");
        }
        private void ButtonClickSag(object sender, MouseEventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
                urunekle(((SimpleButton)sender).Tag.ToString());
        }
       
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if(gridView1.FocusedRowHandle<0) return;
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Sayım İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle
            string sonuc = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE StokKarti Set Sayim=0 where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }
            sonuc = DB.ExecuteScalarSQL("DELETE FROM StokSayimDetay where fkStokSayim=" + pkStokSayim.Text);
            sonuc = DB.ExecuteScalarSQL("DELETE FROM StokSayim where pkStokSayim=" + pkStokSayim.Text);

            if (sonuc != "")
                MessageBox.Show(sonuc);
            else
                pkStokSayim.Text = "0";
            SayilmayanStoklar();

            StokFisleriListesi();

            yesilisikyeni();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("DELETE FROM StokSayimDetay WHERE pkStokSayimDetay=" + dr["pkStokSayimDetay"].ToString());
            DB.ExecuteSQL("UPDATE StokKarti SET Sayim=0 where pkStokKarti=" + dr["pkStokKarti"].ToString());
            DB.ExecuteSQL("UPDATE StokKartiDepo SET IsSayim=0 where fkStokKarti=" + dr["pkStokKarti"].ToString()
                + " and fkDepolar=" + dr["fkDepolar"].ToString());

            gridView1.DeleteSelectedRows();

            SayilmayanStoklar();
            yesilisikyeni();
        }
        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
         if (e.Control && e.Shift && gridView1.DataRowCount > 0)
         {
             //üst satırı kopyala
                 DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);
                 SatisDetayEkle(dr["Barcode"].ToString());
                 //gridView1.ShowEditor();
                 yesilisikyeni();
                 gridView1.ShowEditor();
                 gridView1.ActiveEditor.SelectAll();
                 gridView1.CloseEditor();
                 //Application.DoEvents();
                 //Application.DoEvents();
                 //SendKeys.Send("{ENTER}");
                 return;
         }
            if (e.KeyCode == Keys.Enter)
            {
                string kod=
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (kod == "" && gridView1.DataRowCount == 0)
                {
                    yesilisikyeni();
                    return;
                }
                SatisDetayEkle(kod);
                yesilisikyeni();
                gridView1.FocusedRowHandle=gridView1.DataRowCount;
                gridView1.FocusedColumn = gridView1.VisibleColumns[5];
                gridView1.CloseEditor();
            }
        }
        void urunaraekle()
        {
            //YeniSatisEkle();
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "0";
            StokAra.ShowDialog();
            if (StokAra.TopMost == false) 
            {
                for (int i = 0; i < StokAra.gridView1.DataRowCount; i++)
                {
                    DataRow dr = StokAra.gridView1.GetDataRow(i);
                    if (dr["Sec"].ToString() == "True")
                        SatisDetayEkle(dr["Barcode"].ToString());
                    //SatisDetayEkle(StokAra.pkurunid.Text);
                }
            }
            //SatisDetayEkle(StokAra.pkurunid.Text);
            StokAra.Dispose();
            yesilisikyeni();
            //04.08.2013
            gridView1.FocusedRowHandle = gridView1.DataRowCount;
            gridView1.FocusedColumn = gridView1.VisibleColumns[5];
            gridView1.CloseEditor();
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle != -2147483647)
                gridView1.AddNewRow();
               urunaraekle();
        }
        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.FocusedRowHandle != -2147483647)
            gridView1.AddNewRow();
            urunaraekle();
        }
        private void urunekle(string barkod)
        {
            if (barkod == "") return;
            string ilktarih = "";
            string sontarih = "";
            string fkUrunlerNoPromosyon = "";
            int DususAdet = 1;
            DataTable dturunler = DB.GetData("select * From StokKarti WHERE Barcode='" + barkod + "'");
            if (dturunler.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                StokKartiHizli.ceBarkod.EditValue = barkod;
                StokKartiHizli.ShowDialog();
                if (StokKartiHizli.TopMost == true)
                {
                    dturunler = DB.GetData("select * From StokKarti WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
                }
                else
                {
                    StokKartiHizli.Dispose();
                    return;
                }
                StokKartiHizli.Dispose();
            }

            SatisDetayEkle(barkod);
        }
        void resimolustur(SimpleButton sb)
        {
            x = x + 25;
            y = y + 25;
            PictureBox pictureBox = new PictureBox();
            //xtraTabPage1.Controls.Add(pictureBox);
            pictureBox.Location = new System.Drawing.Point(150+x, 50+y);
            pictureBox.Name = "pictureBox"+p;
            pictureBox.Size = new System.Drawing.Size(150, 100);
            pictureBox.Image = sb.Image;//Image.FromFile("10-.jpg");//
            pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox.TabIndex = 3;
            pictureBox.TabStop = false;
            ((System.ComponentModel.ISupportInitialize)(pictureBox)).BeginInit();
            pictureBox.BringToFront();
            if (x > 200) x = 0;
            if (y > 200) y = 0;
            p++;
        }
        void yesilisikyeni()
        {
            // if (gridView3.FocusedRowHandle >= 0)
            // {
            //   DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            SayimDetayGetir();
            //}
                //gridView1.Focus();
                tedtBarkod.Focus();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            // gridView1.CloseEditor();
            //SendKeys.Send("{ENTER}");
        }
        private bool SatisVar()
        {

            if (gridView1.DataRowCount == 0)
            {
                Showmessage("Önce Satış Yapınız!", "K");
                return false;
            }
             return true;
        }
       
        void SayimKaydet(bool yazdir, bool odemekaydedildi)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string fkDepolar = dr["fkDepolar"].ToString();


            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkStokSayim", pkStokSayim.Text));
            list.Add(new SqlParameter("@Aciklama", "Sayım Fişi Kaydedildi"));//tbAciklama.Text));
            string sonuc = DB.ExecuteSQL("UPDATE StokSayim SET TarihBitis=getdate(),Aciklama=@Aciklama where pkStokSayim=@pkStokSayim", list);

            if (sonuc.Length>1 && sonuc.Substring(1, 1) == "H")
            {
                Showmessage("Hata Oluştur: " + sonuc ,"K");
                return;
            }

            pkStokSayim.Text="0";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr2 = gridView1.GetDataRow(i);

                DB.ExecuteSQL("UPDATE StokKarti SET Sayim=1 where pkStokKarti=" + dr2["pkStokKarti"].ToString());

                DB.ExecuteSQL("UPDATE StokkartiDepo SET MevcutAdet=" + dr2["SayimSonuMiktari"].ToString() +
                   " where fkStokKarti=" + dr2["pkStokKarti"].ToString() + " and fkDepolar=" + fkDepolar);

                DB.ExecuteSQL(@"update StokKarti set Mevcut=MevcutAdet from 
                (select fkStokKarti,sum(MevcutAdet) as MevcutAdet from StokKartiDepo
                group by fkStokKarti) ds
                 where StokKarti.pkStokKarti=ds.fkStokKarti and StokKarti.pkStokKarti=" + dr2["pkStokKarti"].ToString());

                //DB.ExecuteSQL("insert into ");
            }

            pkStokSayim.Text="0";
        }

        void temizle(int aciksatisno)
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = dtMusteriler.Rows[0]["OzelKod"].ToString() + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
            }
            tbAciklama.Text = "";
            gridControl1.DataSource = null;
        }
        void satiskaydet(bool timer, bool yazdir, bool odemekaydedildi)
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesajBox mesaj = new frmMesajBox(200);
                mesaj.label1.Text = "Önce Satış Yapınız";
                mesaj.Show();
                return;
            }
            SayimKaydet(yazdir, odemekaydedildi);
            SayimListesi(0);
            yesilisikyeni();
        }
        void kaydetyazdir(string btn_kaydet_yazdir)
        {
            if (btn_kaydet_yazdir == "kaydet")
                SayimKaydet(true, false);
            else if (btn_kaydet_yazdir == "yazdir")
            {
                string pkSatislar = "0";
                DB.pkSatislar = int.Parse(pkSatislar);
                string YaziciAdi = "", YaziciDosyasi = "";
                DataTable dtYazicilar =
                DB.GetData("SELECT  YaziciAdi,Dosya FROM SatisFisiSecimi with(nolock) where Sec=1");
                if (dtYazicilar.Rows.Count == 1)
                {
                    YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                    YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();
                }
                else if (dtYazicilar.Rows.Count > 1)
                {
                    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(0,0);
                    YaziciAyarlari.ShowDialog();
                    YaziciAyarlari.Tag = 0;
                    YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;
                    if (YaziciAyarlari.YaziciAdi.Tag == null)
                        YaziciAdi = "";
                    else
                        YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                    YaziciAyarlari.Dispose();
                }

                if (YaziciAdi != "")
                {
                    SayimKaydet(true, false);
                    FisYazdir(false, pkSatislar, YaziciDosyasi, YaziciAdi);
                }
                //yesilisikyeni();
            }
            yesilisikyeni();
        }
        private void simpleButton37_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Mevcut Miktarları Stok Sayım Miktarları Yapmak İstediğinize Emininmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            kaydetyazdir("kaydet");

            StokFisleriListesi();

            tedtBarkod.Focus();
        }
        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }
        void FisYazdir(bool Disigner, string pkSatislar,string SatisFisTipi,string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                //string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";

                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        void FisYazdir_eski(bool Disigner)
        {
            string fisid = pkStokSayim.Text;
            //if (lueFis.EditValue != null)
               // fisid = lueFis.EditValue.ToString();
            System.Data.DataSet ds = new DataSet("Test");
            DataTable FisDetay = DB.GetData(@"SELECT * from SatisDetay where fkSatislar=" + fisid);
            FisDetay.TableName = "FisDetay";
            ds.Tables.Add(FisDetay);
            DataTable Fis = DB.GetData(@"SELECT  * from Satislar where pkSatislar=" + fisid);
            Fis.TableName = "Fis";
            ds.Tables.Add(Fis);

            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
            XtraReport rapor = new XtraReport();
            rapor.DataSource = ds;
            rapor.LoadLayout(RaporDosyasi);
            if (Disigner)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }
        void RaporOnizleme(bool Disigner)
        {
            string fisid = pkStokSayim.Text;
            //if (lueFis.EditValue != null)
              //  fisid = lueFis.EditValue.ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Satislar s 
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar
inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
where s.pkSatislar =" + fisid;
            Barkod.DataSource = DB.GetData(sql);

            RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
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
        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;
            int i = gridView1.FocusedRowHandle;
            if (i < 0)
            {
                i = gridView1.DataRowCount;
                i--;
            }
            DataRow dr = gridView1.GetDataRow(i);
            frmStokKarti StokKarti = new frmStokKarti();
            //string pkStokKartiid = dr["pkStokKartiid"].ToString();
            //if (pkStokKartiid == "" || pkStokKartiid == "0")
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
           // else
           // DB.pkStokKarti = int.Parse(pkStokKartiid);
            StokKarti.ShowDialog();
            //xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();
            yesilisikyeni();
            //Satis1SonKayidaGit();
        }
        private void repositoryItemButtonEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                simpleButton4_Click(sender,  e);
                //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                //xtraTabPage5_Click( sender, e);
            }
            //*
            if (e.KeyValue == 223 || e.KeyValue == 106)
            {
                string kod =
   ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (kod == "")
                {
                    gridView1.SetFocusedRowCellValue("Barcode", "");
                    return;
                }
                int adetvar = kod.IndexOf("*");
                decimal badet = 1;
                //string bbarkod = barkod;
                if (adetvar > 0)
                {
                    //badet = float.Parse(kod.Substring(0, adetvar));
                    decimal.TryParse(kod.Substring(0, adetvar), out badet);
                    //bbarkod = barkod.Substring(adetvar + 1, barkod.Length - (adetvar + 1));
                    HizliMiktar = badet;
                }
                gridView1.SetFocusedRowCellValue("Adet",badet);
                gridView1.CloseEditor();
                gridView1.SetFocusedRowCellValue("Barcode", "");
                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
        }

        private void gCSatisDuzen_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 4;
        }
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            AppearanceDefault appyesil = new AppearanceDefault(Color.DarkGreen);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);
            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                //yesilisikyeni();
                return;
            }
            if (e.Column.FieldName == "SayimSonuMiktari" && int.Parse(dr["SayimSonuMiktari"].ToString()) < int.Parse(dr["MevcutMiktar"].ToString()))
            {
            //    decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
            //    decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
             //   if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                    AppearanceHelper.Apply(e.Appearance, appError);
            }
            else if (e.Column.FieldName == "SayimSonuMiktari" && int.Parse(dr["SayimSonuMiktari"].ToString()) > int.Parse(dr["MevcutMiktar"].ToString()))
            {
                //    decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                //    decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                //   if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                AppearanceHelper.Apply(e.Appearance, appyesil);
            }
            //if (e.Column.FieldName == "Adet" && dr["Adet"].ToString() == "0")
            //{
            //    AppearanceHelper.Apply(e.Appearance, appError);
            //}
        }
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                //string iade = View.GetRowCellDisplayText(e.RowHandle, View.Columns["iade"]);
                //string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
                //if (iade.Trim() != "Seçim Yok")
               // {
                 //   e.Appearance.BackColor = Color.DeepPink;
                //}
                //if (Fiyat.Trim() == "0")
                //{
                //    e.Appearance.BackColor = Color.Red;
                //}

            }
        }
        private void satışİptalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);
        }
        
        private void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;
            Rectangle rect = e.Bounds;
            ControlPaint.DrawBorder3D(e.Graphics, e.Bounds);
            Brush brush =
                e.Cache.GetGradientBrush(rect, e.Column.AppearanceHeader.BackColor,
                e.Column.AppearanceHeader.BackColor2, e.Column.AppearanceHeader.GradientMode);
            rect.Inflate(-1, -1);
            // Fill column headers with the specified colors.
            e.Graphics.FillRectangle(brush, rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            // Draw the filter and sort buttons.
            foreach (DevExpress.Utils.Drawing.DrawElementInfo info in e.Info.InnerElements)
            {
               try
               {
                 DevExpress.Utils.Drawing.ObjectPainter.DrawObject(e.Cache, info.ElementPainter,info.ElementInfo);
               }
               catch(Exception exp)
               {
               }

            }
            e.Handled = true;
        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //gridView1.Focus();
            //gridControl1.ForceInitialize();
           // gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            //gridView1.FocusedColumn = gridView1.Columns["UrunKodu"];
            //gridView1.ShowEditor();
            //SendKeys.Send("{BS}");
           if(((SimpleButton)sender).Tag!=null)
                urunekle(((SimpleButton)sender).Tag.ToString());
            //gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
           gridView1.FocusedColumn = gridView1.Columns["SayimSonuMiktari"];
           gridView1.ShowEditor();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton_MouseDown(object sender, MouseEventArgs e)
        {
            //if(e.Button==Keys.Left)
            int l = 0;
            string lef = "0";
            string barkod = "";
            if (((SimpleButton)sender).Tag != null)
            {
                barkod = ((SimpleButton)sender).Tag.ToString();
                lef = ((SimpleButton)sender).Left.ToString();
            }
            int.TryParse(lef, out l);
            frmHizliButtonDuzenle HizliButtonDuzenle = new frmHizliButtonDuzenle();
            HizliButtonDuzenle.Left = l;
            HizliButtonDuzenle.barkod.Text = barkod;
            HizliButtonDuzenle.ShowDialog();
        }

        private void ürünBulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton4_Click(sender,e);
        }

        //private void gridView1_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        //{
        //    if (e.Column.FieldName == "SatisFiyati")
        //        e.RepositoryItem = repositoryItemLookUpEdit1;
        //}
        void SayimListesi(int ipkSayim)
        {
            string sql = "";
            if (ipkSayim == 0)
                sql = @"select s.*,d.DepoAdi from Sayim s with(nolock)
                left join Depolar d with(nolock) on d.pkDepolar = s.fkDepolar
                where bitistarih is null";
            else
                sql = @"select s.*,d.DepoAdi from Sayim s with(nolock)
                left join Depolar d with(nolock) on d.pkDepolar = s.fkDepolar
                where pkSayim =" + ipkSayim;

            gcAcikSayim.DataSource = DB.GetData(sql);

            if (gridView4.DataRowCount == 0)
            {
                btnSayimBasla.Enabled= lueDepolar.Enabled = true; 
                btnSayimiBitir.Enabled = false;
                
            }
            else
            {
                DataRow dr = gridView4.GetDataRow(0);

                //pkSayim.Text = dr["pkSayim"].ToString();

                btnSayimBasla.Enabled = lueDepolar.Enabled = false;

                if (dr["fkDepolar"].ToString() != "")
                    lueDepolar.EditValue = int.Parse(dr["fkDepolar"].ToString());

                btnSayimiBitir.Enabled = true;

                StokFisleriListesi();

                //DataTable dt = DB.GetData("select top 1 pkStokSayim From StokSayim where TarihBitis is null order by pkStokSayim desc");
                //if (dt.Rows.Count == 0)
                //    pkStokSayim.Text = "0";
                //else
                //{
                //    pkStokSayim.Text = dt.Rows[0][0].ToString();
                //}
                SayilmayanStoklar();
            }
        }

        private void sb_MouseEnter(object sender, EventArgs e)
        {
            HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            HizliTop = ((SimpleButton)sender).Top;
            HizliLeft = ((SimpleButton)sender).Left;
            HizliBarkodName = ((SimpleButton)sender).Name;
            pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;
        }


        //private void repositoryItemLookUpEdit1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (gridView1.FocusedRowHandle<0) return;
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        string birimfiyat=
        //        ((DevExpress.XtraEditors.TextEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
        //        if (birimfiyat == "") return;
        //        gridView1.SetFocusedRowCellValue(gridColumn4, birimfiyat);
        //        DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
        //        decimal NakitFiyat = Convert.ToDecimal(dr["NakitFiyat"].ToString());
        //        decimal tutar = NakitFiyat -  Convert.ToDecimal(birimfiyat);

        //        decimal yuzde = (tutar * 100) / NakitFiyat;
        //        gridView1.SetFocusedRowCellValue(gridColumn33, "0");//yuzde);
        //        gridView1.SetFocusedRowCellValue(gridColumn34, "0");//tutar);
        //        //
        //        yesilisikyeni();
        //    }
        //}


        private void alinanpara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                yesilisikyeni();
            }
            if (e.KeyCode == Keys.Enter)
            {
                yesilisikyeni();
            }
        }

        private void yenidenAdlandırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            int secilen = 0;
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
              DataRow dr = gridView2.GetDataRow(i);
              if (dr["Sec"].ToString() == "True")
               {
                 DB.ExecuteSQL("UPDATE StokKarti SET Aktif=0 where pkStokKarti=" + dr["pkStokKarti"].ToString());
                 secilen++;
               }
            }
            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = secilen.ToString() +  " Adet Stok Karti Pasif Yapıldı.";
            Mesaj.Show();
            SayilmayanStoklar();
            yesilisikyeni();
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            //this.Hide();
            //string str = ActiveControl.Name;
            //this.Dispose();
        }

        private void simpleButton20_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        public bool OnceSatisYapiniz()
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.Text = "Önce Satış Yapınız!";
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.Show();
                return false;
            }
            return true;
        }
        private void btnyazdir_Click(object sender, EventArgs e)
        {
            kaydetyazdir("yazdir");
            tedtBarkod.Focus();
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (HizliBarkodName == "")
            {
                toolStripMenuItem2.Enabled = false;
                toolStripMenuItem3.Enabled = false;
            }
            else
            {
                toolStripMenuItem2.Enabled = true;
                toolStripMenuItem3.Enabled = true;
            }
        }

       

        private void iskontoTutar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        private void repositoryItemButtonEdit1_MouseEnter(object sender, EventArgs e)
        {
            //dockPanel1Gizle();
        }
        private void gridControl1_MouseEnter(object sender, EventArgs e)
        {
            //dockPanel1Gizle();
        }

        private void ceiskontoyuzde_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        void SayimDetayGetir()
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            //string pkSayim= dr["pkSayim"].ToString();
            string fkDepolar = dr["fkDepolar"].ToString();

            if (pkStokSayim.Text == "") return;

            gridControl1.DataSource = DB.GetData(@"SELECT StokSayimDetay.pkStokSayimDetay, 
StokSayimDetay.fkStokSayim, StokSayimDetay.MevcutMiktar, StokSayimDetay.SayimSonuMiktari, 
StokKarti.pkStokKarti,StokKarti.Barcode,  StokKarti.Stokadi, StokGruplari.StokGrup, 
StokAltGruplari.StokAltGrup, StokKarti.Mevcut, StokSayimDetay.fkStokKarti,
StokKarti.pkStokKarti,StokKarti.AlisFiyati,SKD.MevcutAdet,StokSayimDetay.fkDepolar FROM  StokSayimDetay with(nolock)
INNER JOIN  StokKarti with(nolock) ON StokSayimDetay.fkStokKarti = StokKarti.pkStokKarti 
LEFT JOIN StokKartiDepo SKD with(nolock) ON SKD.fkStokKarti=StokKarti.pkStokKarti 
LEFT OUTER JOIN  StokGruplari with(nolock) ON StokKarti.fkStokGrup = StokGruplari.pkStokGrup 
LEFT OUTER JOIN  StokAltGruplari with(nolock) ON StokKarti.fkStokAltGruplari = StokAltGruplari.pkStokAltGruplari
 where StokSayimDetay.fkStokSayim=" + pkStokSayim.Text+" and SKD.fkDepolar=" + fkDepolar);
           // gridView1.AddNewRow();
        }
        void SatisDetayEkle(string barkod)
        {
            float Adet = 1;
            decimal EklenenMiktar = 1;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null)
                EklenenMiktar = 1;
            else
            {
                if (dr["MevcutMiktar"].ToString() != "")
                    EklenenMiktar = decimal.Parse(dr["MevcutMiktar"].ToString());
            }
            if (EklenenMiktar == 1)
                EklenenMiktar = HizliMiktar;
            decimal f = 0;
            decimal.TryParse(barkod, out f);
            if (dr != null && dr["pkStokSayimDetay"].ToString() != "") return; 
            if (barkod == "" || f == 0) return;
            if (pkStokSayim.Text == "" || pkStokSayim.Text == "0")
                YeniSatis();
            //başında sıfır varsa
            if (barkod.Length == 3)
                barkod = (1 * decimal.Parse(barkod)).ToString();
            //gramlı ürünler
            if (barkod.Length == 13 && barkod.Substring(0, 3) == DB.TeraziBarkoduBasi1.ToString())
            {
                EklenenMiktar = decimal.Parse(barkod.Substring(7, 5)) / 1000;
                barkod = barkod.Substring(2, 5);
                barkod = (1 * decimal.Parse(barkod)).ToString();
            }
            else if (barkod.Length == 13 && barkod.Substring(0, 3) == DB.TeraziBarkoduBasi2.ToString())
            {
                EklenenMiktar = decimal.Parse(barkod.Substring(7, 5)) / 1000;
                barkod = barkod.Substring(2, 5);
                barkod = (1 * decimal.Parse(barkod)).ToString();
            }
            else if (barkod.Length == 13 && barkod.Substring(0, 3) == DB.TeraziBarkoduBasi3.ToString())
            {
                EklenenMiktar = decimal.Parse(barkod.Substring(7, 5)) / 1000;
                barkod = barkod.Substring(2, 5);
                barkod = (1 * decimal.Parse(barkod)).ToString();
            }
            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti,Mevcut FROM StokKarti where Barcode='" + barkod + "'");
            if (dtStokKarti.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                StokKartiHizli.ceBarkod.EditValue = barkod;
                StokKartiHizli.ShowDialog();
                if (StokKartiHizli.TopMost == true)
                {
                    dtStokKarti = DB.GetData("select pkStokKarti,Mevcut From StokKarti WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
                }
                else
                {
                    yesilisikyeni();
                    StokKartiHizli.Dispose();
                    return;
                }
                StokKartiHizli.Dispose();
            }
            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkStokSayim", pkStokSayim.Text));
            arr.Add(new SqlParameter("@MevcutMiktar", dtStokKarti.Rows[0]["Mevcut"].ToString()));
            arr.Add(new SqlParameter("@SayimSonuMiktari", EklenenMiktar.ToString().Replace(",", ".")));
            arr.Add(new SqlParameter("@fkStokKarti", dtStokKarti.Rows[0]["pkStokKarti"].ToString()));

            string s = DB.ExecuteScalarSQL(@"INSERT INTO StokSayimDetay (fkStokSayim,fkStokKarti,MevcutMiktar,SayimSonuMiktari)
             values(@fkStokSayim,@fkStokKarti,@MevcutMiktar,@SayimSonuMiktari) select 'Sayım Detay Eklendi'", arr);
            if (s != "Sayım Detay Eklendi")
            {
                MessageBox.Show(s);
                return;
            }
            HizliMiktar = 1;
        }
        void YeniSatis()
        {
            string sql = "";
            string fisno = "0";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("@Aciklama", tbAciklama.Text));

            sql = "INSERT INTO StokSayim (Tarih,fkKullanici,Aciklama)" +
                " VALUES(getdate(),@fkKullanici,@Aciklama) SELECT IDENT_CURRENT('StokSayim')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            pkStokSayim.Text = fisno;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Bilgileri ve Genel Durumu. Yapım Aşamasındadır...");
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(0,0);
            string pkSatislar = "0";
            DB.pkSatislar = int.Parse(pkSatislar);
            YaziciAyarlari.Tag = 1;
            YaziciAyarlari.ShowDialog();
        }

        private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmiskonto iskonto = new frmiskonto();
            iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
            iskonto.ceBirimFiyati.Value = decimal.Parse(dr["SatisFiyati"].ToString());
            iskonto.ShowDialog();
            yesilisikyeni();
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{
            //    DataRow dr = gridView1.GetDataRow(i);
            //    if (dr == null)
            //        gridView1.DeleteRow(i);
            //    else if (dr["pkStokSayimDetay"].ToString() == "")
            //        gridView1.DeleteRow(i);
            //}
        }

        private void pkSatisBarkod_Enter(object sender, EventArgs e)
        {
            pkStokSayim.Text = "";
        }

        private void repositoryItemButtonEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string iskontoyuzdetutargirilen =
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (iskontoyuzdetutargirilen == "")
                {
                    yesilisikyeni();
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
                decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
                //decimal Miktar = Convert.ToDecimal(dr["Adet"].ToString().Replace(",", "."));
                decimal iskontoyuzdetutar=0,iskontoyuzde=0;
                decimal.TryParse(iskontoyuzdetutargirilen.ToString(), out iskontoyuzde);
                iskontoyuzdetutar = (iskontoyuzde*Fiyat) /100;
                //gridView1.SetFocusedRowCellValue(gridColumn33, iskontoyuzde);
                DB.ExecuteSQL("UPDATE SatisDetay SET iskontoyuzdetutar=" + iskontoyuzdetutar.ToString().Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
                yesilisikyeni();
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !gridView1.IsEditing && gridView1.FocusedColumn.FieldName == "iskyuzdesanal")
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                frmiskonto iskonto = new frmiskonto();
                iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
                iskonto.ShowDialog();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            DataTable dt = DB.GetData("select pkStokSayim,fkKullanici from StokSayim where TarihBitis is null and fkKullanici=" + DB.fkKullanicilar);
            //select pkSatislar,fkFirma,f.OzelKod from Satislar s with(nolock) "+ 
            //"inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma where Siparis<>1 and fkKullanici="+DB.fkKullanicilar);
            int c =dt.Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string pkSatislar = dt.Rows[i]["pkStokSayim"].ToString();
                    string fkFirma = dt.Rows[i]["fkKullanici"].ToString();
                    AcikSatisindex = i+1;
                    if (i > 2) break;
                    //yesilisikyeni();
                }
                AcikSatisindex = 1;

                SayimDetayGetir();
                yesilisikyeni();

                return;
            }

            SayimDetayGetir();
            yesilisikyeni();
        }
        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (gridView1.DataRowCount == 0) e.Cancel = true;
        }

        private void satışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.pkStokKarti.Text=dr["fkStokKarti"].ToString();
            SatisFiyatlari.ShowDialog();
        }


        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        private void cmsPopYazir_Opening(object sender, CancelEventArgs e)
        {
            if (((((System.Windows.Forms.ContextMenuStrip)(sender)).SourceControl).Controls.Owner).Name == "btnyazdir")
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = true;
                müşteriSeçToolStripMenuItem.Visible = false;
                müşteriKArtıToolStripMenuItem.Visible = false;
            }
            else
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = false;
                müşteriSeçToolStripMenuItem.Visible = true;
                müşteriKArtıToolStripMenuItem.Visible = true;
            }
        }

        private void bARKOTBASToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (gridView1.FocusedRowHandle < 0) return;
           string pkEtiketBas = "0";
           DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
           pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Fiş No " + pkStokSayim.Text + "',0) SELECT IDENT_CURRENT('EtiketBas')");
           DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["fkStokKarti"].ToString() + ",1,0,getdate())");
           frmEtiketBas EtiketBas = new frmEtiketBas();
           EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
           EtiketBas.ShowDialog();
        }

        private void sTOKKARTINIDÜZENLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
           // DataTable dt =
           // DB.GetData("SELECT pkStokKarti from StokKarti where Barcode='" + HizliBarkod + "'");
           // if (dt.Rows.Count > 0)
                DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
           // else
             //   DB.pkStokKarti = 0;
            StokKarti.ShowDialog();
            //Hizlibuttonlariyukle();
        }

        private void simpleButton8_Click_2(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("59");//2.2
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            //Hizlibuttonlariyukle();
        }

        private void groupControl6_MouseClick(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
        }

        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbAciklama.Focus();//işe yaradı
            }
        }

        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            if(gridView1.FocusedRowHandle<0) return;
            //if (((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).OldEditValue == null)
           // {
           //     yesilisikyeni();
           //     return;
           // }
            //string oncekimiktar = ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).OldEditValue.ToString();
            string yenimiktar =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
            if (yenimiktar == "0") yenimiktar = "1";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("UPDATE StokSayimDetay SET SayimSonuMiktari="+yenimiktar+" where pkStokSayimDetay=" + dr["pkStokSayimDetay"].ToString() );
            //int i = gridView1.FocusedRowHandle;
            //SatisDetayGetir();
            //gridView1.FocusedRowHandle = i;
            yesilisikyeni();
        }

        void SayilanStoklar()
        {
            gridControl1.DataSource =
           DB.GetData(@"SELECT sk.Barcode, sk.Stokadi, ssd.pkStokSayimDetay, sk.Stoktipi, sk.Mevcut, sk.fkStokGrup, sk.fkStokAltGruplari
FROM  StokKarti sk with(nolock) 
LEFT OUTER JOIN StokSayimDetay ssd ON sk.pkStokKarti = ssd.fkStokKarti
WHERE ssd.fkStokKarti IS NULL");
            //gridView1.AddNewRow();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            SayilanStoklar();
        }

        private void pasifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridView1.FocusedRowHandle<0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("UPDATE StokKarti SET Aktif=0 where pkStokKarti=" +dr["pkStokKarti"].ToString());
            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = "Stok Karti Pasif Yapıldı.";
            Mesaj.Show();
            yesilisikyeni();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Listedeki Stoklar Sıfırlanacaktır Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=0"  +
                    " where pkStokKarti=" + dr["pkStokKarti"].ToString());
            }
        }

        private void gridControl3_Click(object sender, EventArgs e)
        {

        }

        private void gridView3_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //if(e.FocusedRowHandle<0) return;
            //DataRow dr = gridView3.GetDataRow(e.FocusedRowHandle);
            ////dr["pkStokSayim"].ToString();
            //pkStokSayim.Text = dr["pkStokSayim"].ToString();
            //tbAciklama.Text = dr["Aciklama"].ToString();
            //if (dr["fkKullanici"].ToString()!="")
            //  lueKullanicilar.EditValue = int.Parse(dr["fkKullanici"].ToString());
            //SatisDetayGetir(dr["pkStokSayim"].ToString());
        }

        private void tbAciklama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DB.ExecuteSQL("UPDATE StokSayim SET Aciklama='"+
                    tbAciklama.Text + "' where pkStokSayim=" + pkStokSayim.Text);
                gridView3.SetFocusedRowCellValue("Aciklama", tbAciklama.Text);
                yesilisikyeni();
            }
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmSayimDetay SayimDetay = new frmSayimDetay();
            SayimDetay.Tag = dr["pkStokSayim"].ToString();
            SayimDetay.Show();
        }

        private void btnSayimBasla_Click(object sender, EventArgs e)
        {
            string fisno = 
            DB.ExecuteScalarSQL("INSERT INTO Sayim (ilkTarih,fkKullanici,fkDepolar) values(getdate(),1,"+
            lueDepolar.EditValue.ToString()+") SELECT IDENT_CURRENT('Sayim')");

            DB.ExecuteSQL("update Sayim SET Aciklama='Sayim " + fisno + "' where pkSayim=" + fisno);
            DB.ExecuteSQL("update Sayim SET fkKullanici="+DB.fkKullanicilar+" where pkSayim=" + fisno);
            
            SayimListesi(0);

            //DB.ExecuteSQL("UPDATE StokKarti SET Sayim=0");
            DB.ExecuteSQL("UPDATE StokKartiDepo SET IsSayim=0 where fkDepolar=" + lueDepolar.EditValue.ToString());

            SayilmayanStoklar();
            
            tedtBarkod.Focus();
       }
        void SayilmayanStoklar()
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            //string pkSayim= dr["pkSayim"].ToString();
            string fkDepolar = dr["fkDepolar"].ToString();

            gcStokListesi.DataSource=
            DB.GetData(@"select Convert(bit,'0') as Sec,SK.pkStokKarti,SK.Stokadi,SK.Barcode,SG.StokGrup,SAG.StokAltGrup,SK.AlisFiyati,SK.Mevcut,SK.SatisFiyati,
SK.KdvOrani,SK.UreticiKodu,SKD.fkDepolar,SKD.MevcutAdet from StokKarti  SK with(nolock)
left join StokGruplari SG with(nolock) on SK.fkStokGrup=SG.pkStokGrup
left join StokAltGruplari SAG with(nolock) on SK.fkStokAltGruplari=SAG.pkStokAltGruplari
left join StokKartiDepo SKD with(nolock) ON SKD.fkStokKarti=SK.pkStokKarti
where SK.Aktif=1 and SKD.IsSayim=0 and SKD.fkDepolar=" + fkDepolar);
        }
        //void SayilanStoklar()
        //{
        //    gridControl1.DataSource =
        //    DB.GetData("select * from StokSayimDetay where fkStokSayim=" + pkStokSayim.Text);
        //}
        void StokFisleriListesi()
        {
            DataTable dt = DB.GetData("select top 1 pkStokSayim From StokSayim  with(nolock) where TarihBitis is null order by pkStokSayim desc");
            if (dt.Rows.Count == 0)
                pkStokSayim.Text = "0";
            else
            {
                pkStokSayim.Text = dt.Rows[0][0].ToString();
            }

            sayimfislerilistesi();
            //gcSayimFisleri.DataSource = DB.GetData("select * from StokSayim with(nolock) where fkSayim=" + pkSayim.Text);
        }
        void sayimfislerilistesi()
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string pkSayim= dr["pkSayim"].ToString();
            //string fkDepolar = dr["fkDepolar"].ToString();

            gcSayimFisleri.DataSource = DB.GetData("select * from StokSayim with(nolock) where fkSayim=" + pkSayim);
        }
        private void gridView4_RowClick(object sender, RowClickEventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            gcSayimFisleri.DataSource = DB.GetData("select * from StokSayim where fkSayim=" +dr["pkSayim"].ToString());
            if(dr["fkDepolar"].ToString() != "")
               lueDepolar.EditValue = int.Parse(dr["fkDepolar"].ToString());

            SayilmayanStoklar();
            SayimDetayGetir();
        }

        private void btnSayimiBitir_Click(object sender, EventArgs e)
        {
            if(gridView1.DataRowCount>0)
            {
                DialogResult secim1;
                secim1 = DevExpress.XtraEditors.XtraMessageBox.Show("Kaydedilmemiş Sayımları Kaydetmediniz? Sayım Kaydedilmeyecek Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim1 == DialogResult.No)
                {
                    yesilisikyeni();
                    return;
                }
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Sayımı Bitirmek istediğinize Emininmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            gcStokListesi.DataSource = null;
            DB.ExecuteSQL("UPDATE StokKarti SET Sayim=0");

            if (gridView4.FocusedRowHandle < 0) return;

             DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

             DB.ExecuteSQL("UPDATE Sayim SET bitistarih=getdate() WHERE pkSayim=" + dr["pkSayim"].ToString());
             DB.ExecuteSQL("update StokSayim set TarihBitis=getdate() where pkStokSayim=" + pkStokSayim.Text);

             pkStokSayim.Text = "0";
             gcSayimFisleri.DataSource = null;
             gridControl1.DataSource = null;

             SayimListesi(0);

             tedtBarkod.Focus();
        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
           
        }

        private void gridView2_RowClick(object sender, RowClickEventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            tedtBarkod.Text = dr["Barcode"].ToString();
            tedtBarkod.Tag = dr["pkStokKarti"].ToString();
            //cedtMiktar.EditValue = dr["Mevcut"].ToString();
            cedtMiktar.EditValue = dr["MevcutAdet"].ToString();
            lblUrunAdi.Text = dr["Stokadi"].ToString();
            //txtDepolar.Text = dr["fkDepolar"].ToString();
            
        }

        private void sayımFişiniSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Sayım Fişi Silinecektir Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle
            
            int sonuc = 0;
            string _pkStokSayim = dr["pkStokSayim"].ToString();
            sonuc = DB.ExecuteSQL_Sonuc_Sifir(@"update StokKartiDepo set IsSayim=0 where fkStokKarti in(
            select fkStokKarti From StokSayimDetay where fkStokSayim =" +_pkStokSayim+ ")");

            //sonuc = DB.ExecuteSQL_Sonuc_Sifir("update StokKarti set Sayim=0 where pkStokKarti in(" +
            //        "select fkStokKarti From StokSayimDetay where fkStokSayim=" + dr["pkStokSayim"].ToString() + ")");

            sonuc = DB.ExecuteSQL_Sonuc_Sifir("DELETE FROM StokSayimDetay where fkStokSayim=" + _pkStokSayim);
            sonuc = DB.ExecuteSQL_Sonuc_Sifir("DELETE FROM StokSayim where pkStokSayim=" +_pkStokSayim);
            
            if (sonuc != 0)
                MessageBox.Show(sonuc.ToString());

            SayilmayanStoklar();

            StokFisleriListesi();

            yesilisikyeni();
        }

        private void tedtBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.F8)
            {
                frmStokAra StokAra = new frmStokAra(tedtBarkod.Text);

                StokAra.Tag = "0";
                StokAra.ShowDialog();

                if (StokAra.TopMost == false)
                {
                    for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                    {
                        string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                        DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                        //string barko= dr["Barcode"].ToString();

                        tedtBarkod.Text = dr["Barcode"].ToString();
                        tedtBarkod.Tag = dr["pkStokKarti"].ToString();
                        cedtMiktar.EditValue = dr["Mevcut"].ToString();
                        lblUrunAdi.Text = dr["Stokadi"].ToString();

                    }
                }
                StokAra.Dispose();
                return;
            }

            if (e.KeyCode == Keys.Enter && tedtBarkod.Text.Length>0)
            {
                DataTable dtDepoMevcut = null;

                DataTable dtStok = DB.GetData("select pkStokKarti,Stokadi from StokKarti with(nolock) where Barcode = '" + tedtBarkod.Text + "'");
                string fkStokKarti = "0";

                //çoklu barkodlara bak
                if (dtStok.Rows.Count == 0)
                {
                    DataTable dtBarkod = DB.GetData("select fkStokKarti from StokKartiBarkodlar where Barkod='" + tedtBarkod.Text + "'");
                    if (dtBarkod.Rows.Count == 0)
                    {
                        formislemleri.Mesajform("Çoklu Barkod Ürünü Bulunamadı!", "K", 150);
                        // MessageBox.Show("Stok Bulunamadı");
                        tedtBarkod.Focus();

                        //cedtMiktar.SelectAll();
                        //cedtMiktar.Focus();
                        return;
                    }
                    dtStok = DB.GetData("select pkStokKarti,Stokadi from StokKarti with(nolock) where pkStokKarti = " + dtBarkod.Rows[0]["fkStokKarti"].ToString());
                    fkStokKarti = dtBarkod.Rows[0]["fkStokKarti"].ToString();
                    lblUrunAdi.Text = dtStok.Rows[0]["Stokadi"].ToString();
                }
                else
                {
                    fkStokKarti = dtStok.Rows[0]["pkStokKarti"].ToString();
                    lblUrunAdi.Text = dtStok.Rows[0]["Stokadi"].ToString();
                }

                dtDepoMevcut = DB.GetData("select MevcutAdet from StokKartiDepo with(nolock) where fkStokKarti=" + fkStokKarti +
                    " and fkDepolar=" + lueDepolar.EditValue.ToString());

                //DataTable dturunler = DB.GetData("select Stokadi,Mevcut,skd.MevcutAdet From StokKarti sk with(nolock)" +
                //    " left join StokKartiDepo skd with(nolock) on skd.fkStokKarti = sk.pkStokKarti" +
                //    " where skd.fkDepolar =" + lueDepolar.EditValue.ToString() +
                //    " and Barcode='" + tedtBarkod.Text + "'");

                if (dtDepoMevcut.Rows.Count == 0)
                {
                    formislemleri.Mesajform("Depo Ürünü Bulunamadı!", "K", 150);
                    // MessageBox.Show("Stok Bulunamadı");
                    tedtBarkod.Focus();

                    cedtMiktar.SelectAll();
                    //cedtMiktar.Focus();
                    return;
                }

                //    lblUrunAdi.Text = dtDepoMevcut.Rows[0]["Stokadi"].ToString();
                cedtMiktar.EditValue = dtDepoMevcut.Rows[0]["MevcutAdet"].ToString();


                //DataTable dturunler = DB.GetData("select Stokadi,Mevcut,skd.MevcutAdet From StokKarti sk with(nolock)" +
                //" left join StokKartiDepo skd with(nolock) on skd.fkStokKarti = sk.pkStokKarti"+
                //" where skd.fkDepolar ="+lueDepolar.EditValue.ToString()+
                //" and Barcode='" + tedtBarkod.Text + "'");

                //if (dturunler.Rows.Count == 0)
                //{
                //    formislemleri.Mesajform("Ürün Bulunamadı!","K",150);

                //    tedtBarkod.Focus();
                //}
                //else
                //{
                //    lblUrunAdi.Text = dturunler.Rows[0]["Stokadi"].ToString();
                //    cedtMiktar.EditValue = dturunler.Rows[0]["MevcutAdet"].ToString();

                    cedtMiktar.SelectAll();
                    cedtMiktar.Focus();
                //}

                kaydedilenicindemi(fkStokKarti);
            }
        }

        void kaydedilenicindemi(string fkStokKarti)
        {
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string pkSayim = dr["pkSayim"].ToString();

            DataTable dt=
            DB.GetData(@"select ss.TarihBitis,SayimSonuMiktari from StokSayim ss with(nolock)
            inner join StokSayimDetay ssd with(nolock) on ssd.fkStokSayim = ss.pkStokSayim
            where ss.TarihBitis is not null and ss.fkSayim = " + pkSayim + " and fkStokKarti = "+ fkStokKarti + " and fkDepolar = "+ lueDepolar.EditValue.ToString());

            if (dt.Rows.Count == 0)
            {
                lcKaydedilen.Visible = false;
            }
            else
                lcKaydedilen.Visible = true;

        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
            SayilmayanStoklar();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView1.FocusedRowHandle);
            string pkStokKarti = dr["pkStokKarti"].ToString();
            if (DB.GetData("select * from SatisDetay where fkStokKarti=" + pkStokKarti).Rows.Count > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Stok Kartı Hareket Gördüğü için Silemezsiniz! \n Stoğun Durumunu Pasif Ürün Olarak Seçebilrsiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;

            DB.ExecuteSQL("Delete From StokKarti where pkStokKarti=" + pkStokKarti);

            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = "Stok Kartı Bilgileri Silindi.";
            Mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
            SayilmayanStoklar();
        }

        private void cedtMiktar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEkle_Click(sender, e);
            }
        }

        private void tedtBarkod_KeyUp(object sender, KeyEventArgs e)
        {
            //*
            if (e.KeyValue == 223 || e.KeyValue == 106)
            {
                if (tedtBarkod.Text == "")
                {
                    tedtBarkod.Text = "";
                    return;
                }
                int adetvar = tedtBarkod.Text.IndexOf("*");
                decimal badet = 1;
                if (adetvar > 0)
                {
                    //badet = float.Parse(kod.Substring(0, adetvar));
                    decimal.TryParse(tedtBarkod.Text.Substring(0, adetvar), out badet);
                    HizliMiktar = badet;
                }
                cedtMiktar.Value = badet;
                tedtBarkod.Text = "";
            }
            else if (e.KeyCode == Keys.Space)
            {
                if (gridView1.FocusedRowHandle != -2147483647)
                    gridView1.AddNewRow();

                urunaraekle();
            }
        }
        private void lueFis_Enter(object sender, EventArgs e)
        {
            FisListesi();
        }

        private void lueFis_EditValueChanged(object sender, EventArgs e)
        {
            SayimListesi(int.Parse(lueFis.EditValue.ToString()));
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
        void yazdir(DevExpress.XtraGrid.GridControl gridControl)
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gridControl;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }
        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            yazdir(gridControl1);
            tedtBarkod.Focus();
        }

        private void simpleButton6_Click_1(object sender, EventArgs e)
        {
            yazdir(gcStokListesi);
            tedtBarkod.Focus();
        }

        private void cbTumunuSec_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                //DataRow dr = gridView2.GetDataRow(i);
                gridView2.SetRowCellValue(i, "Sec", cbTumunuSec.Checked);
                   // DB.ExecuteSQL("UPDATE StokKarti SET Aktif=0 where pkStokKarti=" + dr["pkStokKarti"].ToString());
            }
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void gridView2_EndSorting(object sender, EventArgs e)
        {
            if (gridView2.DataRowCount > 0)
                gridView2.FocusedRowHandle = 0;
        }

        private void sayımFişiSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Sayım İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            int sonuc = 0;
 
            sonuc = DB.ExecuteSQL_Sonuc_Sifir("DELETE FROM StokSayimDetay where fkStokSayim in (select  pkStokSayim from StokSayim where fkSayim=" + dr["pkSayim"].ToString()+")");
            sonuc = DB.ExecuteSQL("DELETE FROM StokSayim where fkSayim=" + dr["pkSayim"].ToString());
            sonuc = DB.ExecuteSQL("DELETE FROM Sayim where pkSayim=" + dr["pkSayim"].ToString());

            if (sonuc != 0)
                MessageBox.Show(sonuc.ToString());
            //SayilmayanStoklar();

            pkStokSayim.Text = "0";
            gcSayimFisleri.DataSource = null;
            gridControl1.DataSource = null;
            gcStokListesi.DataSource = null;

            //DB.ExecuteSQL("UPDATE StokKarti SET Sayim=0");

            SayimListesi(0);

            StokFisleriListesi();

            yesilisikyeni();
        }

        private void tümSayımlarıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Sayımlar Silinecektir Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            DB.ExecuteSQL("truncate table StokSayimDetay");
            DB.ExecuteSQL("truncate table StokSayim");
            DB.ExecuteSQL("truncate table Sayim");

            btnSayimBasla.Enabled = true;
            btnSayimiBitir.Enabled = false;
        }

        private void notDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i=gridView3.FocusedRowHandle;
            if (i < 0) return;

            DataRow dr = gridView3.GetDataRow(i);

            string pkStokSayim="0",AciklamaNot="";
            pkStokSayim = dr["pkStokSayim"].ToString();
            AciklamaNot = dr["AciklamaNot"].ToString();
            AciklamaNot = formislemleri.inputbox("Açklama Gir", "Bilgi Giriniz", AciklamaNot, false);
            DB.ExecuteSQL("update StokSayim set AciklamaNot='" + AciklamaNot + "' where pkStokSayim=" + pkStokSayim);

            sayimfislerilistesi();

            gridView3.FocusedRowHandle = i;
        }

        private void kaydetToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\Satism2Grid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\Satism1Grid.xml";
            gridView2.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\Satism2Grid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\Satism1Grid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void sutünSeçimiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void sutünSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView2.ShowCustomization();
            gridView2.OptionsBehavior.AutoPopulateColumns = true;
            gridView2.OptionsCustomization.AllowColumnMoving = true;
            gridView2.OptionsCustomization.AllowColumnResizing = true;
            gridView2.OptionsCustomization.AllowQuickHideColumns = true;
            gridView2.OptionsCustomization.AllowRowSizing = true;
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            tedtBarkod.Text = "";
            cedtMiktar.EditValue = null;
            tedtBarkod.Focus();
        }

        private void lueDepolar_Leave(object sender, EventArgs e)
        {
            lueDepolar.Tag = 0;
        }

        private void lueDepolar_Enter(object sender, EventArgs e)
        {
            lueDepolar.Tag = 1;
        }

        private void lueDepolar_EditValueChanged(object sender, EventArgs e)
        {
            if (lueDepolar.Tag.ToString() == "1")
            {
                SayilmayanStoklar();
                SayimDetayGetir();
            }
        }

        private void mevcutlarıSıfırlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Depo Mevcutları Sıfırlansın mı?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string fkDepolar = dr["fkDepolar"].ToString();

            DB.ExecuteSQL(@"update stokkartidepo set MevcutAdet =0 where fkDepolar ="+ fkDepolar);

            //stok kartınıda güncelle
            DB.ExecuteSQL(@"update StokKarti set Mevcut = MevcutAdet from
                  (select fkStokKarti, sum(MevcutAdet) as MevcutAdet from StokKartiDepo
                  group by fkStokKarti) ds
                 where StokKarti.pkStokKarti = ds.fkStokKarti");

            //if (gridView4.FocusedRowHandle < 0) return;

            //DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            ////string pkSayim= dr["pkSayim"].ToString();
            //string fkDepolar = dr["fkDepolar"].ToString();

            //for (int i = 0; i < gridView2.DataRowCount; i++)
            //{
            //    DataRow dr2 = gridView2.GetDataRow(i);
            //    string fkStokKarti = dr2["pkStokKarti"].ToString();
            //    DataTable dtStokDepo = DB.GetData("	select * from StokKartiDepo with(nolock) where fkDepolar=" + fkDepolar
            //    + " and fkStokkarti="+ fkStokKarti);

            //    if(dtStokDepo.Rows.Count==0)
            //    {
            //        StokKartiDepoInsert(fkStokKarti, fkDepolar, "0",true);
            //    }
            //    else
            //        StokKartiDepoUpdate(fkStokKarti, fkDepolar, "0",true);
            //}

            SayilmayanStoklar();

            //DB.ExecuteSQL("insert into StokKartiDepo" +
            //" select pkStokKarti," + lueDepolar.EditValue.ToString() + ",Mevcut,0,0,0,0,getdate(),0 from StokKarti sk"+
            //" left join StokKartiDepo skd on skd.fkStokkarti = sk.pkStokkarti"+
            //" where skd.IsSayim = 0 and skd.fkDepolar = " + lueDepolar.EditValue.ToString() );
        }
        void StokKartiDepoInsert(string fkStokKarti, string fkDepolar, string MevcutAdet,bool IsSayim)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
            list.Add(new SqlParameter("@fkDepolar", fkDepolar));
            list.Add(new SqlParameter("@MevcutAdet", MevcutAdet));
            list.Add(new SqlParameter("@IsSayim", IsSayim));

            DB.ExecuteSQL(@"insert into StokKartiDepo (fkStokKarti,fkDepolar,MevcutAdet,tarih,IsSayim)
                        values(@fkStokKarti,@fkDepolar,@MevcutAdet,getdate(),@IsSayim)", list);
        }

        void StokKartiDepoUpdate(string fkStokKarti, string fkDepolar, string MevcutAdet,bool IsSayim)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
            list.Add(new SqlParameter("@fkDepolar", fkDepolar));
            list.Add(new SqlParameter("@MevcutAdet", MevcutAdet));
            list.Add(new SqlParameter("@IsSayim", IsSayim));
            

            DB.ExecuteSQL(@"update StokKartiDepo set MevcutAdet=@MevcutAdet,tarih=getdate(),IsSayim=@IsSayim
                     where fkStokKarti=@fkStokKarti and fkDepolar=@fkDepolar", list);
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (tedtBarkod.Text == "")
            {
                tedtBarkod.Focus();
                return;
            }

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string pkSayim = dr["pkSayim"].ToString();
            string fkDepolar = dr["fkDepolar"].ToString();

            string sql = "";
            string StokSayimid = "0";

            if (pkStokSayim.Text == "0")
            {
                if (gridView4.FocusedRowHandle < 0) return;

                sql = "INSERT INTO StokSayim (fkSayim,Tarih,Aciklama,fkKullanici) values(@fkSayim,getdate(),'Sayım Yapılıyor...',1) SELECT IDENT_CURRENT('StokSayim')";
                sql = sql.Replace("@fkSayim", pkSayim);

                StokSayimid = DB.ExecuteScalarSQL(sql);

                pkStokSayim.Text = StokSayimid;

                StokFisleriListesi();
            }
            string sqli = @"INSERT INTO StokSayimDetay (fkStokSayim,fkStokKarti,MevcutMiktar,SayimSonuMiktari,fkDepolar) 
             values(@fkStokSayim,@fkStokKarti,@MevcutMiktar,@SayimSonuMiktari,@fkDepolar)";

            ArrayList list = new ArrayList();
            DataTable dtStok = DB.GetData("select pkStokKarti,Stokadi from StokKarti with(nolock) where Barcode = '" + tedtBarkod.Text + "'");
            string fkStokKarti = "0";
            //çoklu barkodlara bak
            if (dtStok.Rows.Count == 0)
            {
                DataTable dtBarkod = DB.GetData("select fkStokKarti from StokKartiBarkodlar where Barkod='" + tedtBarkod.Text + "'");
                if (dtBarkod.Rows.Count == 0)
                {
                    formislemleri.Mesajform("Çoklu Barkod Ürünü Bulunamadı!", "K", 150);
                    // MessageBox.Show("Stok Bulunamadı");
                    tedtBarkod.Focus();

                    cedtMiktar.SelectAll();
                    //cedtMiktar.Focus();
                    return;
                }
                dtStok = DB.GetData("select pkStokKarti,Stokadi from StokKarti with(nolock) where pkStokKarti = " + dtBarkod.Rows[0]["fkStokKarti"].ToString());
                fkStokKarti = dtBarkod.Rows[0]["fkStokKarti"].ToString();
                lblUrunAdi.Text = dtStok.Rows[0]["Stokadi"].ToString();
            }
            else
            {
                fkStokKarti = dtStok.Rows[0]["pkStokKarti"].ToString();
                lblUrunAdi.Text = dtStok.Rows[0]["Stokadi"].ToString();
            }
            DataTable dtDepoMevcut = DB.GetData("select MevcutAdet from StokKartiDepo with(nolock) where fkStokKarti=" + fkStokKarti +
                   " and fkDepolar=" + lueDepolar.EditValue.ToString());

            if (dtDepoMevcut.Rows.Count == 0)
            {
                formislemleri.Mesajform("Depo Ürünü Bulunamadı!", "K", 150);
                // MessageBox.Show("Stok Bulunamadı");
                tedtBarkod.Focus();

                cedtMiktar.SelectAll();
                //cedtMiktar.Focus();
                return;
            }
            string mevcutadet = dtDepoMevcut.Rows[0]["MevcutAdet"].ToString();

            //lblUrunAdi.Text = dtDepoMevcut.Rows[0]["Stokadi"].ToString();
            //cedtMiktar.EditValue = dtDepoMevcut.Rows[0]["MevcutAdet"].ToString();


            //DataTable dt = DB.GetData("select pkStokKarti,Stokadi,Mevcut,skd.MevcutAdet,skd.fkDepolar From StokKarti sk with(nolock)" +
            //    " left join StokKartiDepo skd with(nolock) on skd.fkStokKarti = sk.pkStokKarti" +
            //    " where skd.fkDepolar =" + fkDepolar +
            //    " and Barcode='" + tedtBarkod.Text + "'");

            //daha önce sayıldımı kontrol et
            DataTable dtKontrol = DB.GetData(@"select pkStokSayimDetay,pkSayim,
            pkStokSayim,fkStokKarti,ssd.SayimSonuMiktari from Sayim s with(nolock)
            inner join StokSayim  sd with(nolock) on sd.fkSayim=s.pkSayim
            inner join StokSayimDetay ssd with(nolock) on ssd.fkStokSayim=sd.pkStokSayim
			inner join StokKarti sk with(nolock) on sk.pkStokKarti=ssd.fkStokKarti --and sk.Sayim=0
            where ssd.fkStokSayim=" + pkStokSayim.Text + " and fkStokKarti=" + fkStokKarti + " and s.fkDepolar="+fkDepolar);

            if (dtKontrol.Rows.Count > 0)
            {
                string SayimSonuMiktari = dtKontrol.Rows[0]["SayimSonuMiktari"].ToString();
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Daha önce "+ SayimSonuMiktari + " Olarak Sayıldı. " +
                    cedtMiktar.Text +" Adet Üzerine Eklensin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);

                if (secim == DialogResult.No) return;

                list.Add(new SqlParameter("@pkStokSayimDetay", dtKontrol.Rows[0]["pkStokSayimDetay"].ToString()));

                sqli = @"update StokSayimDetay set fkStokSayim=@fkStokSayim,MevcutMiktar=@MevcutMiktar,SayimSonuMiktari=SayimSonuMiktari + @SayimSonuMiktari 
                where pkStokSayimDetay=@pkStokSayimDetay";
            }

            list.Add(new SqlParameter("@fkStokSayim", pkStokSayim.Text));
            list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
            list.Add(new SqlParameter("@MevcutMiktar", mevcutadet.Replace(",", ".")));//dt.Rows[0]["Mevcut"].ToString()));
            list.Add(new SqlParameter("@SayimSonuMiktari", cedtMiktar.Value.ToString()));
            list.Add(new SqlParameter("@fkDepolar", fkDepolar));
            string sonuc = DB.ExecuteSQL(sqli, list);

            if (sonuc.Substring(0, 1) == "H")
            {
                MessageBox.Show("Hata : " + sonuc);
                return;
            }
            

            tedtBarkod.Tag = "0";
            tedtBarkod.Text = "";

            DB.ExecuteSQL("UPDATE StokKarti SET Sayim=0 where pkStokKarti=" + fkStokKarti);
            DB.ExecuteSQL("UPDATE StokKartiDepo SET IsSayim=1 where fkStokKarti=" + fkStokKarti +" and fkDepolar=" + fkDepolar);

            SayilmayanStoklar();

            SayimDetayGetir();

            cedtMiktar.Value = 1;
            lblUrunAdi.Text = "";
            lcKaydedilen.Visible = false;
            gridView1.FocusedRowHandle = -2147483646;
            tedtBarkod.Focus();
        }

        private void depoDurumlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            //string pkSayim= dr["pkSayim"].ToString();
            string fkStokKarti = dr["pkStokKarti"].ToString();

            frmStokKartiDepoMevcut StokKartiDepoMevcut = new frmStokKartiDepoMevcut(int.Parse(fkStokKarti));
            StokKartiDepoMevcut.ShowDialog();
        }

        private void depoMevcutlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //string pkSayim= dr["pkSayim"].ToString();
            string fkStokKarti = dr["pkStokKarti"].ToString();

            frmStokKartiDepoMevcut StokKartiDepoMevcut = new frmStokKartiDepoMevcut(int.Parse(fkStokKarti));
            StokKartiDepoMevcut.ShowDialog();
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void stokHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void depodaOlmayanlarıEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Depoda Olmayanlar Eklensin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            //if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string fkDepolar = dr["fkDepolar"].ToString();

            DB.ExecuteSQL("insert into stokkartidepo"+
            " select ad.fkStokKarti,"+ fkDepolar + ",0,0,0,0,0,getdate(),0 from (select * from stokkartidepo with(nolock)  where fkDepolar=1) ad"+
            " left join (select * from stokkartidepo with(nolock)  where fkDepolar="+ fkDepolar + ") cd on cd.fkStokKarti=ad.fkStokKarti"+
            " where cd.pkstokkartidepo is null");

            //for (int i = 0; i < gridView2.DataRowCount; i++)
            //{
            //    DataRow dr2 = gridView2.GetDataRow(i);
            //    string fkStokKarti = dr2["pkStokKarti"].ToString();
            //    DataTable dtDepolar = DB.GetData("select pkDepolar from Depolar with(nolock) where aktif=1");
            //    for (int j = 0; j < dtDepolar.Rows.Count; j++)
            //    {
            //        string fkDepolar = dtDepolar.Rows[j]["pkDepolar"].ToString();
            //        DataTable dtStokDepo = DB.GetData("	select * from StokKartiDepo with(nolock) where fkDepolar=" + fkDepolar
            //        + " and fkStokkarti=" + fkStokKarti);

            //        if (dtStokDepo.Rows.Count == 0)
            //        {
            //            StokKartiDepoInsert(fkStokKarti, fkDepolar, "0", false);
            //        }
            //        //else
            //        //    StokKartiDepoUpdate(fkStokKarti, fkDepolar, "0", false);
            //    }
               
            //}

            SayilmayanStoklar();
        }
    }
}

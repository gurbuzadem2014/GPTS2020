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

namespace GPTS
{
    public partial class frmKampanya : DevExpress.XtraEditors.XtraForm
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis="";//AccessibleDescription
        int AcikSatisindex = 1;//hangi satış açık
        public frmKampanya()
      {
        InitializeComponent();
        DB.PkFirma = 1;
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
      void Fiyatlarigetir()
      {
          lueFiyatlar.Properties.DataSource = DB.GetData("select * from SatisFiyatlariBaslik where Aktif=1 order by pkSatisFiyatlariBaslik");
      }
      private void ucAnaEkran_Load(object sender, EventArgs e)
      {
          Fiyatlarigetir();

          lueSatisTipi.Properties.DataSource = DB.GetData(@"SELECT 1 as pkSatisDurumu, 'Kampanya' as Durumu, 1 as Aktif, 1 as SiraNo
          FROM  SatisDurumu WHERE Aktif = 1 ORDER BY SiraNo");
          lueSatisTipi.EditValue = 1;
          KampanyaListesi();
          timer1.Enabled = true;
      }
        int x = 0;
        int y = 0;
        int p = 1;
        private void ButtonClick(object sender, EventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
            {
                KampanyaDetayEkle(((SimpleButton)sender).Tag.ToString());
                yesilisikyeni();
            }
        }
        private void ButtonClickSag(object sender, MouseEventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
                urunekle(((SimpleButton)sender).Tag.ToString());
        }
        void SatisTemizle()
        {
            if (AcikSatisindex == 1)
            {
                Satis1Toplam.Tag = 0;
                Satis1Toplam.Text = "0,0";
                pkKampanyalar.Text = "0";
            }
            else if (AcikSatisindex == 2)
            {
                Satis2Toplam.Tag = 0;
                Satis2Toplam.Text = "0,0";
            }
            else if (AcikSatisindex == 3)
            {
                Satis3Toplam.Tag = 0;
                Satis3Toplam.Text = "0,0";
            }
            if (AcikSatisindex == 4)
            {
                SatisDuzenKapat();
            }
            temizle(AcikSatisindex);
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kampanya İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle
            
            string sonuc="",pkKampanyalar="0";
            if (AcikSatisindex == 1)
                pkKampanyalar = Satis1Toplam.Tag.ToString();
            if (AcikSatisindex == 2)
                pkKampanyalar = Satis2Toplam.Tag.ToString();
            if (AcikSatisindex == 3)
                pkKampanyalar = Satis3Toplam.Tag.ToString();

            DB.ExecuteSQL("delete from KampanyalarDetay where fkKampanyalar =" + pkKampanyalar);
            DB.ExecuteSQL("delete from Kampanyalar where pkKampanyalar =" + pkKampanyalar);
            SatisTemizle();
            
            yesilisikyeni();

            KampanyaListesi();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("DELETE FROM KampanyalarDetay WHERE pkKampanyalarDetay=" + dr["pkKampanyalarDetay"].ToString());            
            yesilisikyeni();
        }
        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
         if (e.Control && e.Shift && gridView1.DataRowCount > 0)
         {
                 DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);
                 KampanyaDetayEkle(dr["Barcode"].ToString());
                 yesilisikyeni();
                 gridView1.ShowEditor();
                 gridView1.ActiveEditor.SelectAll();
                 gridView1.CloseEditor();
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
                //if (kod == "" && gridView1.DataRowCount>0)
                //{
                //    //üst satırı kopyala
                //    DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);
                //    //SatisDetayEkle(dr["Barcode"].ToString());
                //    return;
                //}
                KampanyaDetayEkle(kod);
                yesilisikyeni();
            }
        }
        void urunaraekle()
        {
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = 1;
            StokAra.ShowDialog();
            KampanyaFiyati.EditValue = StokAra.KampanyaFiyati.EditValue;
            if (StokAra.TopMost == false) 
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();
                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    //DataRow dr = StokAra.gridView1.GetDataRow(i);
                    //if (dr["Sec"].ToString() == "True")
                    KampanyaDetayEkle(dr["Barcode"].ToString());
                }
            }
            StokAra.Dispose();
            yesilisikyeni();
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //gridView1.AddNewRow();
            urunaraekle();
        }
        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //gridView1.AddNewRow();
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
            KampanyaDetayEkle(barkod);
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle == -2147483647 || gridView1.FocusedRowHandle >=0)
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dr == null) return;
                    simpleButton1_Click(sender, e);
            }
            //if (gridView1.DataRowCount == 0)
            //{
                yesilisikyeni();
                return;
            //}
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
            SatisDetayGetir(Satis1Toplam.Tag.ToString());
            TutarFont(Satis1Toplam);
            gridView1.Focus();
            //yesilisikyeni();
            //return;
            //gridView1.CloseEditor();
            //gridView1.AddNewRow();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            //gridView1.SetFocusedRowCellValue(gridColumn2, "1");
            //gridView1.SetFocusedRowCellValue(gridColumn2, "");
            //Application.DoEvents();
            //gridView1.ShowEditor();
            //gridView1.ActiveEditor.SelectAll();
             gridView1.CloseEditor();
            //if (gridView1.IsNewItemRow(gridView1.FocusedRowHandle))
              //  gridView1.ShowEditor();
            
            if (AcikSatisindex == 1) 
                fontayarla(Satis1Toplam);
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
        void satiskaydet(bool yazdir, bool odemekaydedildi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkKampanyalar", pkKampanyalar.Text));
            list.Add(new SqlParameter("@KampanyaAdi", KampanyaAdi.Text));
            list.Add(new SqlParameter("@BasTarih", baslangictarihi.DateTime));
            list.Add(new SqlParameter("@BitisTarih", BitisTarihi.DateTime));
            list.Add(new SqlParameter("@Aktif", cbAktif.Checked));
            
            string sonuc = DB.ExecuteSQL("UPDATE Kampanyalar SET BasTarih=@BasTarih,BitisTarih=@BitisTarih,Aktif=@Aktif,KampanyaAdi=@KampanyaAdi where pkKampanyalar=@pkKampanyalar", list);
            if (sonuc.Length>1 && sonuc.Substring(1, 1) == "H")
            {
                Showmessage("Hata Oluştur: " + sonuc ,"K");
                return;
            }
            SatisTemizle();
            KampanyaListesi();
            yesilisikyeni();           
        }
        void GeciciMusteriDefault()
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.Lütfen Yetkili Firmayı Arayınız");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = DB.PkFirma + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
                Satis1Baslik.Text = DB.FirmaAdi;
                Satis1Firma.Tag = DB.PkFirma;
                Satis2Baslik.Text = DB.FirmaAdi;
                Satis2Firma.Tag = DB.PkFirma;
                Satis3Baslik.Text = DB.FirmaAdi;
                Satis3Firma.Tag = DB.PkFirma;
            }
        }
        void temizle(int aciksatisno)
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = DB.PkFirma + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
            }
            if (aciksatisno == 1)
            {
                Satis1Baslik.Text = DB.FirmaAdi;
                Satis1Firma.Tag = DB.PkFirma;
            }
            else if (aciksatisno == 2)
            {
                Satis2Baslik.Text = DB.FirmaAdi;
                Satis2Firma.Tag = DB.PkFirma;
            }
            else if (aciksatisno == 3)
            {
                Satis3Baslik.Text = DB.FirmaAdi;
                Satis3Firma.Tag = DB.PkFirma;
            }
            btnAciklamaGirisi.ToolTip = "";
            lueSatisTipi.EditValue = 2;
            ceiskontoTutar.EditValue = null;
            ceiskontoyuzde.EditValue = null;
            ceiskontoyuzde.Properties.NullText = "";
            gridControl1.DataSource = null;
            ceiskontoyuzde.Value = 0;
            ceiskontoTutar.Value = 0;
            lueFiyatlar.EditValue = 1;
            KampanyaAdi.Text = "";
        }
        private void simpleButton37_Click(object sender, EventArgs e)
        {
            if (baslangictarihi.EditValue == null)
            {
                baslangictarihi.Focus();
                return;
            }
            if (BitisTarihi.EditValue == null)
            {
                BitisTarihi.Focus();
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(KampanyaAdi.Text + " "+ gridView1.DataRowCount.ToString() +" Farklı Stok için \n" + 
                baslangictarihi.Text + " tarihinde başlatılmak Üzere Oluşturulacaktır, \n Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            
            
            if (OnceSatisYapiniz() == false) return;
               satiskaydet(true,false);
        }
        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }
        void FisYazdir(bool Disigner, string pkKampanyalar,string SatisFisTipi,string YaziciAdi)
        {
            try
            {
                string fisid = pkKampanyalar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Kampanyalar " + fisid);
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
                XtraReport rapor = new XtraReport();
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
            string fisid = pkKampanyalar.Text;
            //if (lueFis.EditValue != null)
               // fisid = lueFis.EditValue.ToString();
            System.Data.DataSet ds = new DataSet("Test");
            DataTable FisDetay = DB.GetData(@"SELECT * from SatisDetay where fkKampanyalar=" + fisid);
            FisDetay.TableName = "FisDetay";
            ds.Tables.Add(FisDetay);
            DataTable Fis = DB.GetData(@"SELECT  * from Kampanyalar where pkKampanyalar=" + fisid);
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
            string fisid = pkKampanyalar.Text;
            //if (lueFis.EditValue != null)
              //  fisid = lueFis.EditValue.ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Kampanyalar s 
inner join SatisDetay sd on sd.fkKampanyalar=s.pkKampanyalar
inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
where s.pkKampanyalar =" + fisid;
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
            string pkStokKartiid = dr["pkStokKartiid"].ToString();
            if (pkStokKartiid == "" || pkStokKartiid == "0")
               DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            else
            DB.pkStokKarti = int.Parse(pkStokKartiid);
            StokKarti.ShowDialog();
            Fiyatlarigetir();
            yesilisikyeni();
            //Satis1SonKayidaGit();
        }
        private void gCSatisDuzen_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 4;
        }
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //if (e.RowHandle < 0) return;
            //AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            //AppearanceDefault appError = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorRed = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorGreen = new AppearanceDefault(Color.GreenYellow);
            //AppearanceDefault appErrorYellowGreen = new AppearanceDefault(Color.YellowGreen);
            //AppearanceDefault appErrorPink = new AppearanceDefault(Color.LightSkyBlue);//, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            //object val = gridView1.GetRowCellValue(e.RowHandle, e.Column);
            //if ((e.Column.FieldName == "UnitPrice" && !(bool)validationControl1.IsTrueCondition(val)[0])
            //  || (e.Column.FieldName == "Quantity" && !(bool)validationControl2.IsTrueCondition(val)[0])
            //|| (e.Column.FieldName == "Discount" && !(bool)validationControl3.IsTrueCondition(val)[0]))

            //DataRow dr = gridView1.GetDataRow(e.RowHandle);
            //if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString()!="" && dr["AlisFiyati"].ToString() != "")
            //{
            //    decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
            //    decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
            //    if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
            //        AppearanceHelper.Apply(e.Appearance, appError);
            //}
            //if (e.Column.FieldName == "iskontotutar" && dr["iskontotutar"].ToString() != "0")
            //{
            //    AppearanceHelper.Apply(e.Appearance, appfont);
            //}
        }
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string iade = View.GetRowCellDisplayText(e.RowHandle, View.Columns["iade"]);
            //    string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
            //    if (AcikSatisindex == 1)
            //        e.Appearance.BackColor = Satis1Toplam.BackColor;
            //    if (AcikSatisindex == 2)
            //        e.Appearance.BackColor = Satis2Toplam.BackColor;
            //    if (AcikSatisindex == 3)
            //        e.Appearance.BackColor = Satis3Toplam.BackColor;
            //    if (AcikSatisindex == 4)
            //        e.Appearance.BackColor = Satis4Toplam.BackColor;
            //    //if (iade.Trim() != "Seçim Yok")
            //   // {
            //     //   e.Appearance.BackColor = Color.DeepPink;
            //    //}
            //    if (Fiyat.Trim() == "0")
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //    }

            //}
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
            //gridView1.FocusedColumn = gridView1.Columns["UrunKodu"];
            //gridView1.ShowEditor();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //if (HizliBarkod == "") return;
            frmHizliButtonDuzenle HizliButtonDuzenle = new frmHizliButtonDuzenle();
            HizliButtonDuzenle.Top = HizliTop;
            HizliButtonDuzenle.Left = HizliLeft;
            HizliButtonDuzenle.oncekibarkod.Text = HizliBarkod;
            HizliButtonDuzenle.oncekibarkod.Tag = pkHizliStokSatis;
            HizliButtonDuzenle.ShowDialog();
            //string adi = HizliButtonDuzenle.barkod.ToolTip;
            //string barkodu = HizliButtonDuzenle.barkod.Text;
            //string pk = HizliButtonDuzenle.barkod.Tag.ToString();
            //HizliBarkodName = ((SimpleButton)sender).Name;
            //TabHizliSatisGenel.Controls.Add(sb)
            //for (int i = 0; i < TabHizliSatisGenel.Controls.Count; i++)
            //{
            //    if (TabHizliSatisGenel.Controls[i].Name == HizliBarkodName)
            //    {
            //        TabHizliSatisGenel.Controls[i].Text = adi;//HizliButtonDuzenle.stokadi.Text;
            //        TabHizliSatisGenel.Controls[i].Tag = barkodu;
            //    }
            //}
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

        void KampanyaListesi()
        {
            string sql = @"SELECT *  FROM Kampanyalar with(nolock)  order by pkKampanyalar desc";
            lueFis.Properties.DataSource = DB.GetData(sql);
        }

        private void sb_MouseEnter(object sender, EventArgs e)
        {
            HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            HizliTop = ((SimpleButton)sender).Top;
            HizliLeft = ((SimpleButton)sender).Left;
            HizliBarkodName = ((SimpleButton)sender).Name;
            pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;
        }

        private void lueFis_EditValueChanged(object sender, EventArgs e)
        {
            if (lueFis.EditValue == null) return;
            //frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis();
            //FisNoBilgisi.fisno.EditValue= lueFis.EditValue.ToString();
            //FisNoBilgisi.ShowDialog();
            //if (FisNoBilgisi.TopMost == true)
            //{
               AcikSatisindex = 1;
                //gCSatisDuzen.Visible = true;
                
               Satis1Toplam.Tag = lueFis.EditValue.ToString();
               // Satis4Toplam_Click(sender, e);
                SatisGetir();
           // }
            //FisNoBilgisi.Dispose();
            lueFis.EditValue = null;
            yesilisikyeni();
        }
        void SatisGetir()
        {
            string pkKampanyalar="0";
            if (AcikSatisindex == 1)
                pkKampanyalar = Satis1Toplam.Tag.ToString();
            DataTable dtKampanyalar = DB.GetData("SELECT *  FROM Kampanyalar where pkKampanyalar=" +
                pkKampanyalar);//fiş bilgisi
            if (dtKampanyalar.Rows.Count == 0)
            {
                Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            if (dtKampanyalar.Rows[0]["Aktif"].ToString() == "True")
                cbAktif.Checked = true;
            else
                cbAktif.Checked = false;
            KampanyaAdi.Text= dtKampanyalar.Rows[0]["KampanyaAdi"].ToString();
            baslangictarihi.DateTime = Convert.ToDateTime(dtKampanyalar.Rows[0]["BasTarih"].ToString());
            BitisTarihi.DateTime = Convert.ToDateTime(dtKampanyalar.Rows[0]["BitisTarih"].ToString());    
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
        private void repositoryItemComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //iskonto yüzde
            if (e.KeyCode == Keys.Enter)
            {
                string iskontoyuzde =
                ((DevExpress.XtraEditors.ComboBox)(((DevExpress.XtraEditors.ComboBox)(sender)).Properties.OwnerEdit)).Text;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                decimal SatisFiyati = Convert.ToDecimal(dr["SatisFiyati"].ToString());
                decimal Adet = Convert.ToDecimal(dr["Adet"].ToString());
                decimal TopTutar = SatisFiyati * Adet;
                yesilisikyeni();
            }
        }

        void TutarFont(DevExpress.XtraEditors.LabelControl secilen)
        {
            Satis1Baslik.Font = buttonkucukfont.Font;
            Satis2Baslik.Font = buttonkucukfont.Font;
            Satis3Baslik.Font = buttonkucukfont.Font;
            if (AcikSatisindex == 1)
                Satis1Baslik.Font = Satis4Baslik.Font;
            else if (AcikSatisindex == 2)
                    Satis2Baslik.Font = Satis4Baslik.Font;
            else if (AcikSatisindex == 3)
                Satis3Baslik.Font = Satis4Baslik.Font;

            Satis1Toplam.Font = defaultfontkucuk.Font;
            Satis2Toplam.Font = defaultfontkucuk.Font;
            Satis3Toplam.Font = defaultfontkucuk.Font;
            Satis4Toplam.Font = defaultfontkucuk.Font;
            secilen.Font = defaultfontbuyuk.Font;
            odemepanel.BackColor = secilen.BackColor;
            gridView1.Appearance.Empty.BackColor = secilen.BackColor;
            gridView1.Appearance.Row.BackColor = secilen.BackColor;
            gridView1.Appearance.TopNewRow.BackColor = secilen.BackColor;
            gridView1.Appearance.HeaderPanel.BackColor = secilen.BackColor;
            iskontopanel.BackColor = secilen.BackColor;
        }
        private void simpleButton17_Click(object sender, EventArgs e)
        {
            frmSatisRaporlari Kampanyalar = new frmSatisRaporlari();
            Kampanyalar.ShowDialog();
            yesilisikyeni();
        }

        private void yenidenAdlandırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HizliButtonAdiDegis ButtonAdiDegis = new HizliButtonAdiDegis();
            ButtonAdiDegis.Top = HizliTop;
            ButtonAdiDegis.Left = HizliLeft;
            ButtonAdiDegis.oncekibarkod.Text = HizliBarkod;
            ButtonAdiDegis.oncekibarkod.Tag = pkHizliStokSatis;
            ButtonAdiDegis.ShowDialog();

            //for (int i = 0; i < TabHizliSatisGenel.Controls[0].Controls.Count; i++)
            //{
            //    if (TabHizliSatisGenel.Controls[0].Controls[i].Name == HizliBarkodName)
            //        TabHizliSatisGenel.Controls[0].Controls[i].Text = ButtonAdiDegis.stokadi.Text;
            //}
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            string pkSatis = "0";
            if (AcikSatisindex == 1) pkSatis = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2) pkSatis = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3) pkSatis = Satis3Toplam.Tag.ToString();
            if (pkSatis == "0")
            {
                Showmessage("Önce Satış Yapınız!", "K");
                return;
            }
            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.memoozelnot.Text = btnAciklamaGirisi.ToolTip;
            fFisAciklama.ShowDialog();
            btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;
            KampanyaAdi.Text = fFisAciklama.memoozelnot.Text;
            DB.ExecuteSQL("UPDATE Kampanyalar SET Aciklama='" + btnAciklamaGirisi.ToolTip + "' where pkKampanyalar=" + pkSatis);
            fFisAciklama.Dispose();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            string str = ActiveControl.Name;
            this.Dispose();
        }

        private void simpleButton20_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void satistipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lueSatisTipi.EditValue.ToString())
	      {
              case "1":
                  {
                      btnyazdir.Text = "Fiş Yazdır [F11]"; break;
                      btnyazdir.Tag = "0";
                  }
              case "3":
                  {
                      btnyazdir.Text = "Fatura Yazdır [F11]"; break;
                      btnyazdir.Tag = "0";
                  }
	      }
          KampanyaListesi();
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
            if(OnceSatisYapiniz()==false) return;
  
            string pkKampanyalar = "0";
            if (AcikSatisindex == 1)
                pkKampanyalar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2)
                pkKampanyalar = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3)
                pkKampanyalar = Satis3Toplam.Tag.ToString();
            DB.pkSatislar = int.Parse(pkKampanyalar);
            string YaziciAdi = "", YaziciDosyasi="";
            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya  FROM SatisFisiSecimi where Sec=1");
            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();
            }
            else if (dtYazicilar.Rows.Count >1)
            {
                 frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(23,0);
                 YaziciAyarlari.ShowDialog();
                 YaziciAyarlari.Tag=0;
                 YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;
                 if (YaziciAyarlari.YaziciAdi.Tag == null)
                     YaziciAdi = "";
                 else
                 YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                 YaziciAyarlari.Dispose();
            }
            if (YaziciAdi != "")
            {
                satiskaydet(false, true);
                FisYazdir(false, pkKampanyalar, YaziciDosyasi,YaziciAdi);
            }
            yesilisikyeni();
        }

        private void lueFiyatlar_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
			{
			    DataRow dr = gridView1.GetDataRow(i);
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string pkSatisDetay = dr["pkKampanyalarDetay"].ToString();
                DataTable dt = DB.GetData("select * from SatisFiyatlari where Aktif=1 and fkStokKarti=" + fkStokKarti +" and fkSatisFiyatlariBaslik=" + lueFiyatlar.EditValue.ToString());
                if (dt.Rows.Count==0)
                    dt = DB.GetData("select * from SatisFiyatlari where fkStokKarti=" + fkStokKarti + " and fkSatisFiyatlariBaslik=1");
                string SatisFiyat1 = dt.Rows[0]["SatisFiyatiKdvli"].ToString();
                string sql = "update SatisDetay set SatisFiyati=@SatisFiyati where pkSatisDetay=" + pkSatisDetay;
                sql = sql.Replace("@SatisFiyati", SatisFiyat1.Replace(",", "."));
                DB.ExecuteSQL(sql);
			}
            yesilisikyeni();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE HizliStokSatis Set fkStokKarti=0 where pkHizliStokSatis=" + pkHizliStokSatis);
            //for (int i = 0; i < TabHizliSatisGenel.Controls.Count; i++)
            //{
            //    if (TabHizliSatisGenel.Controls[i].Name == HizliBarkodName)
            //        TabHizliSatisGenel.Controls[i].Text = "BOŞ";
            //    ((SimpleButton)(TabHizliSatisGenel.Controls[i])).Image = null;

            //}
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
        private void Satis3Toplam_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 3;
            yesilisikyeni();
        }
        private void Satis4Toplam_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 4;
            yesilisikyeni();
        }


        void SatisDuzenKapat()
        {
            gCSatisDuzen.Visible = false;
            pkKampanyalar.EditValue = null;
            TutarFont(Satis4Toplam);
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
           SatisDuzenKapat();
        }
        private void repositoryItemCalcEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string yenimiktar =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
                MiktarDegistir(yenimiktar);
                yesilisikyeni();
            }
        }
        void MiktarDegistir(string yenimiktar)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSatisDetay = dr["pkSatisDetay"].ToString();
            DB.ExecuteSQL("UPDATE SatisDetay SET Adet=" + yenimiktar.Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
            decimal iskontoyuzde = 0;
            if (dr["iskontoyuzdetutar"].ToString() != "")
                iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
            decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());
            decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
            decimal Miktar = Convert.ToDecimal(yenimiktar);
            decimal iskontogercektutar = Convert.ToDecimal(dr["iskontotutar"].ToString());

            if (iskontogercektutar > 0)
            {
                iskontogercekyuzde = (iskontogercektutar * 100) / (Fiyat * Miktar);
            }  
        }
        private void iskontoTutar_EditValueChanged(object sender, EventArgs e)
        {
            ceiskontoyuzde.EditValue = null;
        }

        private void iskontoTutar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }
        private void pkSatisBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //fiş düzenle
            if (e.KeyCode == Keys.Enter)
            {
                
                if (pkKampanyalar.Text == "") return;
                lueFis.EditValue = int.Parse(pkKampanyalar.Text);
                if (lueFis.EditValue == null)
                    lueFis.EditValue = pkKampanyalar.Text;
                //lueFis_EditValueChanged(sender, e);
                //Satis4Toplam.Tag = pkSatisBarkod.Text;
                //SatisGetir();

                //gCSatisDuzen.Visible = true;
                //Satis4Toplam_Click(sender, e);
                
            }
        }

        private void gCSatisDuzen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DB.PkFirma = int.Parse(gCSatisDuzen.Tag.ToString());
            frmMusteriKarti kart = new frmMusteriKarti(gCSatisDuzen.Tag.ToString(), "");
            kart.ShowDialog();
        }

        private void ceiskontoyuzde_EditValueChanged(object sender, EventArgs e)
        {
            ceiskontoTutar.EditValue = null;
        }

        private void ceiskontoyuzde_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }
        void SatisDetayGetir(string fkKampanyalar)
        {
            pkKampanyalar.Text = fkKampanyalar;
            gridControl1.DataSource = DB.GetData(@"SELECT KampanyalarDetay.pkKampanyalarDetay, KampanyalarDetay.fkKampanyalar,KampanyalarDetay.fkStokKarti,
 sk.AlisFiyati,KampanyalarDetay.SatisFiyati,sk.SatisFiyati as NakitFiyati,sk.Stokadi, sk.Barcode, sk.StokKod, sk.Stoktipi,sk.pkStokKartiid,KampanyalarDetay.fkKampanyalar
FROM KampanyalarDetay with(nolock)
INNER JOIN (select pkStokKarti,StokKod,Stokadi,Barcode,Stoktipi,pkStokKartiid,AlisFiyati,SatisFiyati from StokKarti with(nolock)) sk ON  KampanyalarDetay.fkStokKarti = sk.pkStokKarti 
where KampanyalarDetay.fkKampanyalar=" + fkKampanyalar+
" order by KampanyalarDetay.pkKampanyalarDetay");
            //+ fkKampanyalar + ",0");
            gridView1.AddNewRow();
        }
        void KampanyaDetayEkle(string barkod)
        {
            float Adet = 1;
            float EklenenMiktar = 1;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null)
                EklenenMiktar = 1;
            float f = 0;
            float.TryParse(barkod, out f);
            //if (dr != null && dr["pkKampanyalarDetay"].ToString() != "") return; 
            //if (barkod == "" || f == 0) return;
            if (pkKampanyalar.Text=="0")
               YeniKampanya();

            if (barkod.Length == 3)
                barkod = (1 * float.Parse(barkod)).ToString();

            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti FROM StokKarti where Barcode='" + barkod + "'");
            if (dtStokKarti.Rows.Count == 0)
            {
                return;
                //frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                //StokKartiHizli.ceBarkod.EditValue = barkod;
                //StokKartiHizli.ShowDialog();
                //if (StokKartiHizli.TopMost == true)
                //{
                  //  dtStokKarti = DB.GetData("select pkStokKarti From StokKarti WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
                //}
                //else
                //{
                 //   yesilisikyeni();
                  //  StokKartiHizli.Dispose();
                   // return;
                //}
                //StokKartiHizli.Dispose();
            }
            string pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkKampanyalar", pkKampanyalar.Text));
            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));

            if (KampanyaFiyati.EditValue == null) return;
            arr.Add(new SqlParameter("@SatisFiyati", KampanyaFiyati.EditValue.ToString().Replace(",", ".")));

            string sql = "INSERT INTO KampanyalarDetay (fkKampanyalar,Tarih,fkStokKarti,SatisFiyati)" +
" values(@fkKampanyalar,getdate(),@fkStokKarti,@SatisFiyati) SELECT IDENT_CURRENT('KampanyalarDetay')";
            string pkKampanyalarDetay = DB.ExecuteScalarSQL(sql,arr);//"exec sp_KampanyalarDetay_Ekle @fkKampanyalar,@SatisFiyatGrubu,@Adet,@fkStokKarti,@iskontoyuzde,@iskontotutar", arr);
            if (pkKampanyalarDetay.Substring(0,1) == "H")
            {
                MessageBox.Show(pkKampanyalarDetay);
                return;
            }
           // DB.ExecuteSQL("UPDATE KampanyalarDetay SET SatisFiyati=" + KampanyaFiyati.Value +
             //   " where pkKampanyalarDetay=" + pkKampanyalarDetay);
        }
        void YeniKampanya()
        {
            string sql = "";
            string fisno = "0";
            bool yazdir = false;
            ArrayList list = new ArrayList();
            string pkFirma = "1";
            pkFirma = Satis1Firma.Tag.ToString();
            if (btnAciklamaGirisi.ToolTip == "")
                btnAciklamaGirisi.ToolTip = "Kampanya";
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            sql = "INSERT INTO Kampanyalar (Tarih,Aktif,KampanyaAdi,Aciklama)" +
                " VALUES(getdate(),0,'',@Aciklama) SELECT IDENT_CURRENT('Kampanyalar')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            pkKampanyalar.Text = fisno;
            Satis1Toplam.Tag = fisno;
            SatisDetayGetir(Satis1Toplam.Tag.ToString());
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Bilgileri ve Genel Durumu. Yapım Aşamasındadır...");
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(23,0);
            string pkKampanyalar = "0";
            if (AcikSatisindex == 1)
                pkKampanyalar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2)
                pkKampanyalar = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3)
                pkKampanyalar = Satis3Toplam.Tag.ToString();
            DB.pkSatislar = int.Parse(pkKampanyalar);
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
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr == null)
                    gridView1.DeleteRow(i);
                else if (dr["pkKampanyalarDetay"].ToString() == "")
                    gridView1.DeleteRow(i);
            }
        }

        private void sonrakiSatisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AcikSatisindex == 1)  AcikSatisindex = 2;
            else if (AcikSatisindex == 2) AcikSatisindex = 3;
            else if (AcikSatisindex ==3) AcikSatisindex = 1;
            yesilisikyeni();
            Application.DoEvents();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            yesilisikyeni();
        }

        private void repositoryItemCalcEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string g =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if(dr==null) return;
                string pkKampanyalarDetay = dr["pkKampanyalarDetay"].ToString();
                DB.ExecuteSQL("UPDATE KampanyalarDetay SET SatisFiyati=" + g.Replace(",", ".") + " where pkKampanyalarDetay=" + pkKampanyalarDetay);
                yesilisikyeni();
            }
        }

        private void pkSatisBarkod_Enter(object sender, EventArgs e)
        {
            pkKampanyalar.Text = "";
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
                DB.ExecuteSQL("UPDATE KampanyalarDetay SET iskontoyuzdetutar=" + iskontoyuzdetutar.ToString().Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
                yesilisikyeni();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lueFiyatlar.EditValue = 1;
            timer1.Enabled = false;
            DataTable dt = DB.GetData("select pkKampanyalar from Kampanyalar where Aktif<>1");
            int c =dt.Rows.Count;
            if (c > 0)
            {
                AcikSatisindex = 1;
                SatisDetayGetir(dt.Rows[0][0].ToString());
                yesilisikyeni();
                return;
            }
            SatisDetayGetir("0");
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
            int i = int.Parse(lueFiyatlar.EditValue.ToString());
            Fiyatlarigetir();
            lueFiyatlar.EditValue = i;
        }

        private void iskontoTutar_Enter(object sender, EventArgs e)
        {
            ceiskontoTutar.EditValue = null;
        }

        private void iskontoTutar_Leave(object sender, EventArgs e)
        {
            if (ceiskontoTutar.EditValue == null)
                ceiskontoTutar.EditValue = ceiskontoTutar.OldEditValue;
        }

        private void Satis1Toplam_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 1;
            yesilisikyeni();
        }


        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                yesilisikyeni();
            if (e.KeyCode == Keys.Space)
                urunaraekle();
        }

        private void Satis2Toplam_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 2;
            yesilisikyeni();
        }
        void fontayarla(LabelControl lb)
        {
            if (lb.Text.Length < 4)
                lb.Font = new Font(lb.Font.Name, 36, FontStyle.Bold);
            if (lb.Text.Length == 4)
                lb.Font = new Font(lb.Font.Name, 35, FontStyle.Bold);
            if (lb.Text.Length == 5)
                lb.Font = new Font(lb.Font.Name, 34, FontStyle.Bold);
            if (lb.Text.Length == 6)
                lb.Font = new Font(lb.Font.Name, 33, FontStyle.Bold);
            if (lb.Text.Length == 7)
                lb.Font = new Font(lb.Font.Name, 32, FontStyle.Bold);
            if (lb.Text.Length == 8)
                lb.Font = new Font(lb.Font.Name, 31, FontStyle.Bold);
            if (lb.Text.Length == 9)
                lb.Font = new Font(lb.Font.Name, 30, FontStyle.Bold);
            if (lb.Text.Length > 10)
                lb.Font = new Font(lb.Font.Name, 20, FontStyle.Bold);
        }

        void SatisDetayYenile()
        {
            if (AcikSatisindex == 1)
            {
                SatisDetayGetir(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
            }
            else if (AcikSatisindex == 2)
            {
                SatisDetayGetir(Satis2Toplam.Tag.ToString());
                TutarFont(Satis2Toplam);
            }
            else if (AcikSatisindex == 3)
            {
                SatisDetayGetir(Satis3Toplam.Tag.ToString());
                TutarFont(Satis3Toplam);
            }
            else if (AcikSatisindex == 4)
            {
                SatisDetayGetir(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
            }
        }
        private void repositoryItemCalcEdit3_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).OldEditValue == null)
            {
                yesilisikyeni();
                return;
            }
            string oncekimiktar = ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).OldEditValue.ToString();
            string yenimiktar =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
            if (yenimiktar == "0") yenimiktar = "1";
            MiktarDegistir(yenimiktar);
            int i = gridView1.FocusedRowHandle;
            SatisDetayYenile();
            gridView1.FocusedRowHandle = i;
        }

        private void repositoryItemCalcEdit3_MouseDown(object sender, MouseEventArgs e)
        {
            //güncelleme sorunu oldu
            //((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue = null;
        }

        private void müşteriKArtıToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (AcikSatisindex == 1)
                DB.PkFirma = int.Parse(Satis1Firma.Tag.ToString());
            if (AcikSatisindex == 2)
                DB.PkFirma = int.Parse(Satis2Firma.Tag.ToString());
            if (AcikSatisindex == 3)
                DB.PkFirma = int.Parse(Satis3Firma.Tag.ToString());

            frmMusteriKarti MusteriKarti = new frmMusteriKarti(DB.PkFirma.ToString(), "");
            MusteriKarti.ShowDialog();
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
           //if (gridView1.FocusedRowHandle < 0) return;
           //string pkEtiketBas = "0";
           //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
           //pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Fiş No " + pkSatisBarkod.Text + "',0) SELECT IDENT_CURRENT('EtiketBas')");
           //DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["fkStokKarti"].ToString() + ",1,0,getdate())");
           //frmEtiketBas EtiketBas = new frmEtiketBas();
           //EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
           //EtiketBas.ShowDialog();
        }

        private void sTOKKARTINIDÜZENLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStokKarti StokKarti = new frmStokKarti();
            DataTable dt =
            DB.GetData("SELECT pkStokKarti from StokKarti where Barcode='" + HizliBarkod + "'");
            if (dt.Rows.Count > 0)
                DB.pkStokKarti = int.Parse(dt.Rows[0][0].ToString());
            else
                DB.pkStokKarti = 0;
            StokKarti.ShowDialog();
        }

        private void simpleButton8_Click_2(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("1");
            SayfaAyarlari.ShowDialog();
        }

        private void Güncelle_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE KampanyalarDetay SET SatisFiyati="+KampanyaFiyati.Value+
                    " where pkKampanyalarDetay=" + dr["pkKampanyalarDetay"].ToString());
            }
            yesilisikyeni();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@Aktif", cbAktif.Checked));
            //DB.ExecuteSQL("UPDATE Kampanyalar SET Aktif=@Aktif where pkKampanyalar=" + pkKampanyalar.Text,list);
        }
    }
}

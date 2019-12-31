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

namespace GPTS
{
    public partial class frmProjeSatis : DevExpress.XtraEditors.XtraForm
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis="";//AccessibleDescription
        int AcikSatisindex = 1;//hangi satış açık
        decimal HizliMiktar = 1;
        public frmProjeSatis()
      {
        InitializeComponent();
        DB.PkFirma = 1;
      }
      private void ucAnaEkran_Load(object sender, EventArgs e)
      {
          pkProjeler.Text=DB.pkProjeler.ToString();
          Fiyatlarigetir();
          HizliSatisTablariolustur();
          //xtraTabControl3.TabPages[0].Tag = "0";
          //TabHizliSatisGenel.Tag = "0";
          //xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
          //gridView1.AddNewRow();
          //SendKeys.Send("{LEFT}");
          //gridView1.FocusedColumn = gridView1.Columns["UrunKodu"];
          //gridView1.ShowEditor();
          //gridView1.GetNextVisibleRow(0);
          //gridView1.ShowEditor();
          //gridView1.DefaultEdit.BeginInit();
          //CreateFiyatGruplari();
          //return;
          //gridView1.Focus();
          //gridControl1.ForceInitialize();
          //gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
          //gridView1.FocusedColumn = gridView1.Columns["UrunKodu"];
          //gridView1.ShowEditor();
          //GridCell gcStart = new GridCell(gridView1.FocusedRowHandle, gridView1.Columns[0]);
          //GridCell gcEnd = new GridCell(gridView1.FocusedRowHandle, gridView1.Columns[gridView1.Columns.Count - 1]);
          //gridView1.SelectCells(gcStart, gcEnd);
          //SendKeys.Send("{ENTER}");
          Yetkiler();
          //timer1.Enabled = true;
          lueSatisTipi.Properties.DataSource = DB.GetData(@"SELECT pkSatisDurumu, Durumu, Aktif, SiraNo
FROM  SatisDurumu WHERE     (Aktif = 1) ORDER BY SiraNo");
          lueSatisTipi.EditValue = 2;
          FisListesi();
          //gridView1.Focus();
          //gridView1.AddNewRow();
          //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
          //gridView1.SetFocusedRowCellValue(gridColumn1, "1");
          //gridView1.SetFocusedRowCellValue(gridColumn1, "");
          //Application.DoEvents();
          //gridView1.ShowEditor();
          //gridView1.ActiveEditor.SelectAll();
          //SendKeys.Send("{ENTER}");
          GeciciMusteriDefault();
          timer1.Enabled = true;
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
              if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
                  gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
              if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
                  xtraTabControl2.Visible = true;
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
          lueFiyatlar.EditValue = 1;
      }
      void HizliSatisTablariolustur()
      {
          xtraTabControl3.TabPages.Clear();
          DataTable dt = DB.GetData("select * from HizliGruplar where Aktif=1 order by SiraNo");
          for (int i = 0; i < dt.Rows.Count; i++)
          {
              XtraTabPage xtab = new XtraTabPage();
              xtab.Text = dt.Rows[i]["HizliGrupAdi"].ToString();
              xtab.Tag = dt.Rows[i]["pkHizliGruplar"].ToString();
              xtraTabControl3.TabPages.Add(xtab);
              xtab.PageVisible = true;
          }
          xtraTabControl3.SelectedTabPageIndex = 0;
          //Hizlibuttonlariyukle();
          //xtab.Controls.Add(uc); 
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
        void Hizlibuttonlariyukle()
        {
            //temizle
            xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();
            int to = 0;
            int lef = 0;
            DataTable dtbutton = DB.GetData(@"select pkHizliStokSatis,pkStokKarti,Barcode,HizliSatisAdi,Stokadi,sf.SatisFiyatiKdvli as SatisFiyati from HizliStokSatis h
            left join (select pkStokKarti,Stokadi,fkStokGrup,HizliSatisAdi,Barcode,HizliSiraNo 
            from StokKarti) sk on sk.pkStokKarti=h.fkStokKarti 
            left join SatisFiyatlari sf on sf.fkStokKarti=h.fkStokKarti  and sf.fkSatisFiyatlariBaslik=1
            where fkHizliGruplar=" + xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Tag.ToString() +
            " order by pkHizliStokSatis");
            int h = 80;//dockPanel1.Height / 7;
            int w = 110;//dockPanel1.Width / 5;
            for (int i = 0; i < dtbutton.Rows.Count; i++)
            {
                string pkid = dtbutton.Rows[i]["pkStokKarti"].ToString();
                string Barcode = dtbutton.Rows[i]["Barcode"].ToString();
                string HizliSatisAdi = dtbutton.Rows[i]["HizliSatisAdi"].ToString();
                string Stokadi = dtbutton.Rows[i]["Stokadi"].ToString();
                string SatisFiyati = dtbutton.Rows[i]["SatisFiyati"].ToString();
                SimpleButton sb = new SimpleButton();
                sb.AccessibleDescription = dtbutton.Rows[i]["pkHizliStokSatis"].ToString();
                sb.Name = "Btn" + (i + 1).ToString();
                sb.Text = HizliSatisAdi;
                sb.Tag =  Barcode;
                sb.ToolTip = "Satış Fiyatı=" + SatisFiyati +"\n Stok Adı:"+Stokadi;
                sb.ToolTipTitle = "Kodu: " + Barcode;
                sb.Height = h;
                sb.Width = w;
                sb.Click += new EventHandler(ButtonClick);
                sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseEnter);
                sb.Left = lef;
                sb.Top = to;
                //adı 15 karakterden büyükse
                if (HizliSatisAdi.Length>15)
                sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                else
                    sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                sb.ContextMenuStrip = contextMenuStrip1;
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string imagedosya=exeDiz + "\\HizliSatisButtonResim\\" + pkid +".png";
                if (File.Exists(imagedosya))
                {
                    Image im =new Bitmap(imagedosya);
                    sb.Image = new Bitmap(im, 45, 45);
                    sb.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                }
                if (i != 0 && (i + 1) % 7 == 0)
                {
                    to = 0;
                    lef = lef + w;
                }
                else
                {
                    to += h;
                }
                sb.Show();
                sb.SendToBack();
                //DockPanel p1 = dockManager1.AddPanel(DockingStyle.Left);
                //p1.Text = "Genel";
                //DevExpress.XtraEditors.SimpleButton btn = new DevExpress.XtraEditors.SimpleButton();
                //btn.Text = "Print...";
                xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Add(sb);
                /*
                //LABEL OLUŞTUR
                Label l = new Label();
                l.Name = "lab" + (i + 1).ToString();
                l.Text = Barcode;
                //l.Left = sb.Left;
                //l.Width = sb.Width;
                //l.Height = sb.Height-50;
                l.Top = sb.Height - 20;
                l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                l.BackColor = System.Drawing.Color.Transparent;
                l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                l.BorderStyle = System.Windows.Forms.BorderStyle.None;
                //l.Click += new EventHandler(ButtonClick);
                PButtonlar.Controls.Add(sb);
                sb.Controls.Add(l);
                l.Show();
                */
            }
            //if (((SimpleButton)sender).Tag != null)
            //    uruneklemdb(((SimpleButton)sender).Tag.ToString());
            //dockPanel1.Width = 900;
            //dockPanel1.Show();
            //xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Tag = "1";
            //for (int i = 0; i < TabHizliSatisGenel.Controls.Count; i++)
            //{
            //    //if (TabHizliSatisGenel.Controls[i].Name == HizliBarkodName)
            //    //  TabHizliSatisGenel.Controls[i].Text = "BOŞ";
            //    ((SimpleButton)(TabHizliSatisGenel.Controls[i])).Dispose();

            //}
        }
        void SatisTemizle()
        {
            if (AcikSatisindex == 1)
            {
                Satis1Toplam.Tag = 0;
                Satis1Toplam.Text = "0,0";
                pkSatisBarkod.Text = "0";
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
            cbOdemeSekli.SelectedIndex = 0;
            TaksitOdemeTarihi.Visible = false;
            lueKKarti.Visible = false;
            temizle(AcikSatisindex);
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle
            
            string sonuc="",pkSatislar="0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            if (AcikSatisindex == 2)
                pkSatislar = Satis2Toplam.Tag.ToString();
            if (AcikSatisindex == 3)
                pkSatislar = Satis3Toplam.Tag.ToString();
               sonuc = DB.ExecuteScalarSQL("EXEC spSatisSil " + pkSatislar+",1");
               if (sonuc != "Satis Silindi.")
                MessageBox.Show(sonuc);
            SatisTemizle();
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
            DB.ExecuteSQL("DELETE FROM ProjeSatisDetay WHERE pkProjeSatisDetay=" + dr["pkProjeSatisDetay"].ToString());
            //DB.ExecuteSQL("update StokKarti set Mevcut=Mevcut+" + dr["Adet"].ToString().Replace(",", ".") + "  where pkStokKarti=" + dr["fkStokKarti"].ToString());
            gridView1.DeleteSelectedRows();
            if (gridView1.DataRowCount == 0)
            {
                DB.ExecuteSQL("DELETE FROM ProjeSatislar WHERE pkProjeSatislar=" + pkSatisBarkod.Text);
                pkSatisBarkod.Text = "0";
                if (AcikSatisindex == 1)
                    Satis1Toplam.Tag = "0";
                else if (AcikSatisindex == 2)
                    Satis2Toplam.Tag = "0";
                else if (AcikSatisindex == 3)
                    Satis3Toplam.Tag = "0";
            }
            yesilisikyeni();
        }
        void YeniSatisEkle()
        {
            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
                YeniSatis();
            else if (AcikSatisindex == 2 && Satis2Toplam.Tag.ToString() == "0")
                YeniSatis();
            else if (AcikSatisindex == 3 && Satis3Toplam.Tag.ToString() == "0")
                YeniSatis();
        }
        void iskontoyagit_ctrli()
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                if (gridView1.DataRowCount > 0)
                    ceiskontoyuzde.Focus();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmiskonto iskonto = new frmiskonto();
            iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
            iskonto.ceBirimFiyati.Value = decimal.Parse(dr["SatisFiyati"].ToString());
            iskonto.ShowDialog();
            yesilisikyeni();
            return;
        }        
        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //ctrl + i iskonto
         if (e.Control && e.KeyValue == 222)
            {
                iskontoyagit_ctrli();
            }
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
        //void StokSatisFiyati(string pkStokKarti)
        //{
        //    if (pkStokKarti == "") return;
        //    string satisfiyati = "0";
        //    //************ satiş fiyatı bul
        //    string sql = "";
        //    //Kampanya var mı
        //    DataTable dt=  DB.GetData("Select * from KampanyaStokFiyat  where fkStokKarti=" + pkStokKarti);
        //    if (dt.Rows.Count==0)
        //       dt = DB.GetData("Select * FROM UrunlerSatisFiyatlari where fkStokKarti=" + pkStokKarti);
        //    if (dt.Rows.Count == 0)
        //    {
        //        MessageBox.Show("Fiyat Bilgisi Bulunamadı.");
        //        return;
        //    }
        //    DataTable dtbaslik = DB.GetDatamdb("Select * FROM UrunlerSatisBaslik");
        //    DataTable dtFiyatlar = new DataTable();
        //    dtFiyatlar.Columns.Add(new DataColumn("FiyatAdi", typeof(string)));
        //    dtFiyatlar.Columns.Add(new DataColumn("Yuzde", typeof(Int32)));
        //    dtFiyatlar.Columns.Add(new DataColumn("Fiyat", typeof(decimal)));
        //    dtFiyatlar.Columns.Add(new DataColumn("pkid", typeof(int)));
        //    for (int i = 0; i < 6; i++)
        //    {
        //        DataRow dr;
        //        dr = dtFiyatlar.NewRow();
        //        if (i == 0)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde1"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat1"].ToString();
        //            satisfiyati = dt.Rows[0]["SatisFiyat1"].ToString();
        //        }
        //        else if (i == 1)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde2"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat2"].ToString();
        //        }
        //        else if (i == 2)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde3"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat3"].ToString();
        //        }
        //        else if (i == 3)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde4"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat4"].ToString();
        //        }
        //        else if (i == 4)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde5"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat5"].ToString();
        //        }
        //        dr["pkid"] = (i + 1).ToString();
        //        dtFiyatlar.Rows.Add(dr);
        //    }
        //    repositoryItemLookUpEdit1.Properties.DataSource = dtFiyatlar;
        //    dtFiyatlar.Dispose();

        //    if (satisfiyati == "") satisfiyati = "0";
        //    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SatisFiyati", satisfiyati);
        //    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "NakitFiyat", satisfiyati);
        //}
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
            return;
            //DususAdet = int.Parse(dturunler.Rows[0]["CikisAdet"].ToString());
            //string pkStokKarti = dturunler.Rows[0]["pkStokKarti"].ToString();
            //string pkStokKartiid = dturunler.Rows[0]["pkStokKartiid"].ToString();
            //StokSatisFiyati(pkStokKarti);
            ////    satisfiyati = dtsatisfiyati.Rows[0]["SatisFiyat"].ToString();
            ////    fkUrunlerNoPromosyon = dtsatisfiyati.Rows[0]["fkUrunlerNoPromosyon"].ToString();
            ////TODO: Promosyon yapılacak iskonto ile yap tarinde varsa ekle            
            //int sr = 0;
            //sr = gridView1.FocusedRowHandle;//DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            //if (dturunler.Rows[0]["pkStokKartiid"].ToString()!="")
            //gridView1.SetRowCellValue(sr, "pkStokKartiid", dturunler.Rows[0]["pkStokKartiid"].ToString());
            //gridView1.SetRowCellValue(sr, "Barcode", dturunler.Rows[0]["Barcode"].ToString());
            //gridView1.SetRowCellValue(sr, "Stokadi", dturunler.Rows[0]["Stokadi"].ToString());
            //string iade = "";
            //if (gridView1.GetFocusedRowCellValue("iade") == null)
            //    iade = "false";
            //else
            //    iade = gridView1.GetFocusedRowCellValue("iade").ToString();
            //if (iade == "True")
            //    gridView1.SetRowCellValue(sr, "iade", "true");
            //else
            //    gridView1.SetRowCellValue(sr, "iade", "false");

            // string miktar="1";
            //if(gridView1.GetFocusedRowCellValue("Adet")==null)
            //    miktar = "1";
            //else
            //{
            //    miktar = gridView1.GetFocusedRowCellValue("Adet").ToString();
            //    if (miktar == "") miktar = "1";
            //    if (iade == "True") miktar = (int.Parse(miktar) * -1).ToString(); 
            //}
            //if (miktar == "" || miktar == "1")
            //    gridView1.SetRowCellValue(sr, "Adet", "1");
            //else
            //    gridView1.SetRowCellValue(sr, "Adet", miktar);
            
            //gridView1.SetRowCellValue(sr, "pkid", gridView1.DataRowCount + 1);
            //gridView1.SetRowCellValue(sr, "pkStokKarti", dturunler.Rows[0]["pkStokKarti"].ToString());
            //gridView1.SetFocusedRowCellValue("Stoktipi", dturunler.Rows[0]["Stoktipi"].ToString());
            //gridView1.SetFocusedRowCellValue("AlisFiyati", dturunler.Rows[0]["AlisFiyati"].ToString());
            //gridView1.SetFocusedRowCellValue("iskonto", "0");
            //gridView1.SetFocusedRowCellValue("iskontotutar", "0");
            //gridView1.SetFocusedRowCellValue("KdvOrani", dturunler.Rows[0]["KdvOrani"].ToString());
            //gridView1.SetFocusedRowCellValue("iskontogercektutar", "0");
            //gridView1.SetFocusedRowCellValue("iskontogercekyuzde", "0");
            //gridView1.AddNewRow();
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
            
            if (AcikSatisindex == 1) fontayarla(Satis1Toplam);
            if (AcikSatisindex == 2) fontayarla(Satis2Toplam);
            if (AcikSatisindex == 3) fontayarla(Satis3Toplam);
            SendKeys.Send("{ENTER}");
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
            list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.EditValue.ToString()));
            string sonuc = DB.ExecuteSQL("UPDATE ProjeSatislar SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,fkSatisDurumu=@fkSatisDurumu,OdemeSekli=@OdemeSekli where pkProjeSatislar=@pkProjeSatislar", list);
            if (sonuc.Length>1 && sonuc.Substring(1, 1) == "H")
            {
                Showmessage("Hata Oluştur: " + sonuc ,"K");
                return;
            }
            //proje için gerek var mı
            //OdemeKaydet(odemekaydedildi);
            SatisTemizle();
            FisListesi();
        }
        void OdemeKaydet(bool odemekaydedildi)
        {
            string pkSatis = "0", pkFirma = "0";
            if (AcikSatisindex == 1)
            {
                pkSatis = Satis1Toplam.Tag.ToString();
                pkFirma = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 2)
            {
                pkSatis = Satis2Toplam.Tag.ToString();
                pkFirma = Satis2Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 3)
            {
                pkSatis = Satis3Toplam.Tag.ToString();
                pkFirma = Satis3Firma.Tag.ToString();
            }
            if (odemekaydedildi == false)//ödeme alınmadı ise ctrl+d değilse
            {
                DB.ExecuteSQL("UPDATE Firmalar SET Alacak=Alacak+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
                if (cbOdemeSekli.SelectedIndex == 0)
                {
                    //Nakit
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkFirma", pkFirma));
                    list.Add(new SqlParameter("@Borc", aratoplam.Value.ToString().Replace(",", ".")));
                    list.Add(new SqlParameter("@Aciklama", "Nakit Ödeme"));
                    list.Add(new SqlParameter("@fkSatislar", pkSatis));
                    DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkKasalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
                        " values(0,1,@fkFirma,getdate(),4,1,@Borc,0,1,@Aciklama,'Nakit',@fkSatislar)", list);
                    DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
                    //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
                    return;
                }
                //kredi kartı ile ödeme yaptıysa
                if (cbOdemeSekli.SelectedIndex == 1 && lueKKarti.EditValue == null)
                {
                    MessageBox.Show("Kredi Kartı Seçiniz!");
                    lueKKarti.Focus();
                    return;
                }
                if (cbOdemeSekli.SelectedIndex == 1)
                {
                    ArrayList list2 = new ArrayList();
                    list2.Add(new SqlParameter("@fkFirma", pkFirma));
                    list2.Add(new SqlParameter("@Borc", aratoplam.Value.ToString().Replace(",", ".")));
                    list2.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue));
                    list2.Add(new SqlParameter("@Aciklama", "K.Kartı Ödemesi"));
                    list2.Add(new SqlParameter("@fkSatislar", pkSatis));
                    DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
                        " values(@fkBankalar,@fkFirma,getdate(),8,1,@Borc,0,1,@Aciklama,'Kredi Kartı',@fkSatislar)", list2);
                    DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
                    //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
                }
                //açık hesap
                if (cbOdemeSekli.SelectedIndex == 2)
                {
                    ArrayList list4 = new ArrayList();
                    list4.Add(new SqlParameter("@fkSatislar", pkSatis));
                    list4.Add(new SqlParameter("@AcikHesap", aratoplam.Value.ToString().Replace(",", ".")));
                    DB.ExecuteSQL("UPDATE Satislar SET AcikHesap=@AcikHesap where pkSatislar=@fkSatislar", list4);

                    if (TaksitOdemeTarihi.EditValue != null)
                    {
                        ArrayList list3 = new ArrayList();
                        list3.Add(new SqlParameter("@fkFirma", pkFirma));
                        list3.Add(new SqlParameter("@Tarih", TaksitOdemeTarihi.DateTime));
                        list3.Add(new SqlParameter("@Odenecek", aratoplam.Value.ToString().Replace(",", ".")));
                        list3.Add(new SqlParameter("@fkSatislar", pkSatis));
                        DB.ExecuteSQL(@"INSERT INTO dbo.Taksitler (fkFirma,Tarih,Odenecek,Odenen,SiraNo,Aciklama,OdemeSekli,fkSatislar)
                  VALUES(@fkFirma,@Tarih,@Odenecek,0,1,'Açık Hesap','Açık Hesap',@fkSatislar)", list3);
                    }
                }
            }
        }
        void GeciciMusteriDefault()
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.Lütfen Yetkili Firmayı Arayınız");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = dtMusteriler.Rows[0]["OzelKod"].ToString() + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
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
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = dtMusteriler.Rows[0]["OzelKod"].ToString() + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
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
            cbOdemeSekli.SelectedIndex = 0;
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
            satiskaydet(yazdir, odemekaydedildi);
            FisListesi();
            SatisDuzenKapat();
           // temizle(AcikSatisindex);
            yesilisikyeni();
        }
        public bool bir_nolumusteriolamaz()
        {
            if (DB.GetData("select * from Sirketler").Rows[0]["MusteriZorunluUyari"].ToString() == "True")
            {
                if ((AcikSatisindex == 1 && Satis1Firma.Tag.ToString() == "1") ||
                    (AcikSatisindex == 2 && Satis2Firma.Tag.ToString() == "1") ||
                    (AcikSatisindex == 3 && Satis3Firma.Tag.ToString() == "1"))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Müşteri Olamaz.\n (Ayarlardan Kaldırabilirsiniz.)!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    musteriara();
                    return false;
                }
            }
            return true;

        }
        void kaydetyazdir(string btn_kaydet_yazdir)
        {
            if (bir_nolumusteriolamaz() == false) return;
            if (OnceSatisYapiniz() == false) return;
            if (btn_kaydet_yazdir == "kaydet")
                satiskaydet(true, false);
            //if (btn_kaydet_yazdir == "yazdir")
            else if (btn_kaydet_yazdir == "yazdir")
            {
                //if (bir_nolumusteriolamaz() == false) return;
                //if (OnceSatisYapiniz() == false) return;

                string pkSatislar = "0";
                if (AcikSatisindex == 1)
                    pkSatislar = Satis1Toplam.Tag.ToString();
                else if (AcikSatisindex == 2)
                    pkSatislar = Satis2Toplam.Tag.ToString();
                else if (AcikSatisindex == 3)
                    pkSatislar = Satis3Toplam.Tag.ToString();
                DB.pkSatislar = int.Parse(pkSatislar);
                string YaziciAdi = "", YaziciDosyasi = "";
                DataTable dtYazicilar =
                DB.GetData("SELECT  YaziciAdi,Dosya FROM SatisFisiSecimi where Sec=1");
                if (dtYazicilar.Rows.Count == 1)
                {
                    YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                    YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();
                }
                else if (dtYazicilar.Rows.Count > 1)
                {
                    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(6,0);
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
                    satiskaydet(true, false);
                    FisYazdir(false, pkSatislar, YaziciDosyasi, YaziciAdi);
                }
                //yesilisikyeni();
            }
            yesilisikyeni();
        }
        private void simpleButton37_Click(object sender, EventArgs e)
        {
            kaydetyazdir("kaydet");
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
            string fisid = pkSatisBarkod.Text;
            //if (lueFis.EditValue != null)
               // fisid = lueFis.EditValue.ToString();
            System.Data.DataSet ds = new DataSet("Test");
            DataTable FisDetay = DB.GetData(@"SELECT * from ProjeSatisDetay where fkProjeSatislar=" + fisid);
            FisDetay.TableName = "FisDetay";
            ds.Tables.Add(FisDetay);
            DataTable Fis = DB.GetData(@"SELECT  * from ProjeSatislar where pkSatislar=" + fisid);
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
            string fisid = pkSatisBarkod.Text;
            //if (lueFis.EditValue != null)
              //  fisid = lueFis.EditValue.ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM ProjeSatislar s 
inner join ProjeSatisDetay sd on sd.fkProjeSatislar=s.pkSatislar
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
        private string musteriara()
        {
            string fkFirma = "1",ozelkod="0", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        fkFirma = Satis1Firma.Tag.ToString();
                        break;
                    }
                case 2:
                    {
                        fkFirma = Satis2Firma.Tag.ToString();
                        break;
                    }
                case 3:
                    {
                        fkFirma = Satis3Firma.Tag.ToString();
                        break;
                    }
            }
            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.fkFirma.Tag = fkFirma;
            MusteriAra.ShowDialog();
            fkFirma = MusteriAra.fkFirma.Tag.ToString();
           
            if (pkSatisBarkod.Text != "0" || pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE ProjeSatislar SET fkFirma=" + fkFirma + " where pkProjeSatislar=" + pkSatisBarkod.Text);
            }
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + fkFirma);
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString(); 
            MusteriAra.Dispose();
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        Satis1Firma.Tag = fkFirma;
                        Satis1Baslik.Text = ozelkod + "-" + firmadi;
                        Satis1Baslik.ToolTip = Satis1Baslik.Text;
                        break;
                    }
                case 2:
                    {
                        Satis2Firma.Tag = fkFirma;
                        Satis2Baslik.Text = ozelkod + "-" + firmadi;
                        Satis2Baslik.ToolTip = Satis2Baslik.Text;
                        break;
                    }
                case 3:
                    {
                        Satis3Firma.Tag = fkFirma;
                        Satis3Baslik.Text = ozelkod + "-" + firmadi;
                        Satis3Baslik.ToolTip = Satis3Baslik.Text;
                        break;
                    }
                default:
                    break;
            }
            yesilisikyeni();
            return fkFirma;
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            musteriara();
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
            xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();
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
                gridView1.SetFocusedRowCellValue(gridColumn3, "1");
                gridView1.SetFocusedRowCellValue(gridColumn3, badet);
                gridView1.CloseEditor();
                gridView1.SetFocusedRowCellValue("Barcode", "");
                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //iade
            if (e.KeyValue == 222)
            {
                string kod =
   ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (kod == "")
                {
                    gridView1.SetFocusedRowCellValue("Barcode", "");
                    return;
                }
                int adetvar = kod.IndexOf("*");
                int badet = 1;
                //string bbarkod = barkod;
                if (adetvar > 0)
                {
                    badet = int.Parse(kod.Substring(0, adetvar));
                    //bbarkod = barkod.Substring(adetvar + 1, barkod.Length - (adetvar + 1));

                }
                gridView1.SetFocusedRowCellValue("iade", "true");
                gridView1.SetFocusedRowCellValue("Barcode", "");
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //nakit
            lueKKarti.Visible = false;
            if (e.KeyValue == 78)
            {
                cbOdemeSekli.SelectedIndex = 0;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
                //kkartı
            else if (e.KeyValue == 75)
            {
                cbOdemeSekli.SelectedIndex = 1;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                lueKKarti.Properties.DataSource = DB.GetData("select * from Bankalar where Aktif=1");
                lueKKarti.Visible = true;
                lueKKarti.ItemIndex = 0;
            }
                //veresiye açık hesap
            else if (e.KeyValue == 65)
            {
                cbOdemeSekli.SelectedIndex = 2;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //DİĞER
            else if (e.KeyValue == 68)
            {
                cbOdemeSekli.SelectedIndex = 3;
                ((DevExpress.XtraEditors.TextEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
        }

        private void gCSatisDuzen_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 4;
        }
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorRed = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorGreen = new AppearanceDefault(Color.GreenYellow);
            //AppearanceDefault appErrorYellowGreen = new AppearanceDefault(Color.YellowGreen);
            //AppearanceDefault appErrorPink = new AppearanceDefault(Color.LightSkyBlue);//, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            //object val = gridView1.GetRowCellValue(e.RowHandle, e.Column);
            //if ((e.Column.FieldName == "UnitPrice" && !(bool)validationControl1.IsTrueCondition(val)[0])
            //  || (e.Column.FieldName == "Quantity" && !(bool)validationControl2.IsTrueCondition(val)[0])
            //|| (e.Column.FieldName == "Discount" && !(bool)validationControl3.IsTrueCondition(val)[0]))

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                //yesilisikyeni();
                return;
            }
            if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString()!="" && dr["AlisFiyati"].ToString() != "")
            {
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                    AppearanceHelper.Apply(e.Appearance, appError);
            }
            if (e.Column.FieldName == "iskontotutar" && dr["iskontotutar"].ToString() != "0")
            {
                AppearanceHelper.Apply(e.Appearance, appfont);
            }
            if (e.Column.FieldName == "Adet" && dr["Adet"].ToString() == "0")
            {
                AppearanceHelper.Apply(e.Appearance, appError);
            }
        }
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string iade = View.GetRowCellDisplayText(e.RowHandle, View.Columns["iade"]);
                string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
                if (AcikSatisindex == 1)
                    e.Appearance.BackColor = Satis1Toplam.BackColor;
                if (AcikSatisindex == 2)
                    e.Appearance.BackColor = Satis2Toplam.BackColor;
                if (AcikSatisindex == 3)
                    e.Appearance.BackColor = Satis3Toplam.BackColor;
                if (AcikSatisindex == 4)
                    e.Appearance.BackColor = Satis4Toplam.BackColor;
                //if (iade.Trim() != "Seçim Yok")
               // {
                 //   e.Appearance.BackColor = Color.DeepPink;
                //}
                if (Fiyat.Trim() == "0")
                {
                    e.Appearance.BackColor = Color.Red;
                }

            }
        }
        void toplamlar()
        {
            decimal aratop = 0;
            if (gridColumn5.SummaryItem.SummaryValue == null)
                aratop=0;//atis1Toplam.Text = "0,0";
            else
                aratop =  decimal.Parse(gridColumn5.SummaryItem.SummaryValue.ToString());

            decimal isyuzde = 0,isfiyat=0,istutar=0;
            //önce hesapla sonra bilgi göster NullTexde
            if (ceiskontoyuzde.EditValue != null)
            {
                isfiyat = decimal.Parse(ceiskontoyuzde.EditValue.ToString());
                istutar = isfiyat * aratop/100;
            }
            if (ceiskontoTutar.EditValue != null) // && iskontoTutar.EditValue.ToString() !="0")
            {
                isfiyat = decimal.Parse(ceiskontoTutar.EditValue.ToString());
                if (aratop>0)
                   isyuzde = (isfiyat * 100) / aratop;
                istutar = isfiyat;
            }

            aratoplam.Value = aratop - istutar;
            if(AcikSatisindex==1)
            {
                Satis1Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
                Satis1Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
            }
            if (AcikSatisindex == 2)
            {
                Satis2Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
                Satis2Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
            }
            if (AcikSatisindex == 3)
            {
                Satis3Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
                Satis3Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
            }

            if (AcikSatisindex == 4)
            {
                Satis4Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
                Satis4Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
            }

            if (aratop > 0 && ceiskontoTutar.EditValue != null)
                ceiskontoyuzde.Properties.NullText = "% " + Convert.ToDouble(((ceiskontoTutar.Value * 100) / aratop)).ToString("#0.0");
            else
                ceiskontoyuzde.Properties.NullText = "";
            if (aratop > 0 && ceiskontoyuzde.EditValue != null)
                ceiskontoTutar.Properties.NullText = Convert.ToDouble(((ceiskontoyuzde.Value * aratop) / 100)).ToString("##0.00");
            else
                ceiskontoTutar.Properties.NullText ="";
            //if (gridColumn28.SummaryItem.SummaryValue == null)
            //    cefark.EditValue = "0,0";
            //else
            //    cefark.EditValue = gridColumn28.SummaryItem.SummaryValue.ToString();

            //cefark.Value = aratoplam.Value - iskontoTutar.Value;
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
            Hizlibuttonlariyukle();
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

        void FisListesi()
        {
            string sql = @"SELECT top 20 s.pkSatislar, f.Firmaadi, sum(sd.Adet* sd.SatisFiyati) as Tutar,s.Tarih
            FROM ProjeSatislar s 
            LEFT JOIN ProjeSatisDetay sd ON s.pkProjeSatislar = sd.fkProjeSatislar 
            INNER JOIN Firmalar f ON s.fkFirma = f.PkFirma
            WHERE Siparis=1 and s.fkSatisDurumu=@fkSatisDurumu
            group by s.pkProjeSatislar, f.Firmaadi,s.Tarih
            order by s.pkProjeSatislar desc";
            sql = sql.Replace("@fkSatisDurumu", lueSatisTipi.EditValue.ToString());
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
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.EditValue= lueFis.EditValue.ToString();
            FisNoBilgisi.ShowDialog();
            if (FisNoBilgisi.TopMost == true)
            {
                AcikSatisindex = 4;
                gCSatisDuzen.Visible = true;
                
                Satis4Toplam.Tag = lueFis.EditValue.ToString();
                Satis4Toplam_Click(sender, e);
                SatisGetir();
            }
            FisNoBilgisi.Dispose();
            FisListesi();
            lueFis.EditValue = null;
            yesilisikyeni();
        }
        void SatisGetir()
        {
            string pkSatislar="0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            if (AcikSatisindex == 2)
                pkSatislar = Satis2Toplam.Tag.ToString();
            if (AcikSatisindex == 3)
                pkSatislar = Satis3Toplam.Tag.ToString();
            if (AcikSatisindex == 4)
                pkSatislar = Satis4Toplam.Tag.ToString();
            DataTable dtSatislar = DB.GetData("exec sp_Satislar " + pkSatislar);//fiş bilgisi
            if (dtSatislar.Rows.Count == 0)
            {
                Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            string fkfirma=dtSatislar.Rows[0]["fkFirma"].ToString();
            string firmaadi = dtSatislar.Rows[0]["Firmaadi"].ToString();
            if (AcikSatisindex == 1)
            {
                Satis1Firma.Tag = fkfirma;
                Satis1Firma.Text = fkfirma + "-" + firmaadi;
            }
            if (AcikSatisindex == 2)
            {
                Satis2Firma.Tag = fkfirma;
                Satis2Firma.Text = fkfirma + "-" + firmaadi;
            }
            if (AcikSatisindex == 3)
            {
                Satis3Firma.Tag = fkfirma;
                Satis3Firma.Text = fkfirma + "-" + firmaadi;
            }
            if (AcikSatisindex == 4)
            {
                gCSatisDuzen.Tag = fkfirma;
                gCSatisDuzen.Text = fkfirma + "-" + firmaadi;
            }
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
                decimal iskontotutar = (TopTutar * Convert.ToDecimal(iskontoyuzde) / 100);
                gridView1.SetFocusedRowCellValue(gridColumn33, iskontoyuzde);
                yesilisikyeni();
            }
        }
        void iskontotutarDegistir(string iskontotutar)
        {
            if (iskontotutar == "")
            {
                yesilisikyeni();
                return;
            }
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSatisDetay = dr["pkSatisDetay"].ToString();
            decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
            decimal Miktar = Convert.ToDecimal(dr["Adet"].ToString().Replace(",", "."));
            if (Miktar == 0) return;
            decimal iskontoyuzde = (Convert.ToDecimal(iskontotutar) * 100) / (Fiyat * Miktar);
            gridView1.SetFocusedRowCellValue(gridColumn33, iskontoyuzde);
            DB.ExecuteSQL("UPDATE SatisDetay SET iskontotutar=" + iskontotutar.Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
        }
        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //iskonto tutar
            if (e.KeyCode == Keys.Enter)
            {
                string iskontotutar =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
                iskontotutarDegistir(iskontotutar);
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
            frmSatisRaporlari Satislar = new frmSatisRaporlari();
            Satislar.ShowDialog();
            yesilisikyeni();
        }

        private void cariSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            musteriara();
        }

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
            Hizlibuttonlariyukle();
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
            DB.ExecuteSQL("UPDATE Satislar SET Aciklama='" + btnAciklamaGirisi.ToolTip + "' where pkSatislar=" + pkSatis);
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
            FisListesi();
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
        }
        private void lueFiyatlar_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
			{
			    DataRow dr = gridView1.GetDataRow(i);
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
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
            Hizlibuttonlariyukle();
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
        void OdemeSekli(string odemesekli)
        {
            string pkSatislar = "0";
            string musteriadi="";
            string pkFirma = "1";
            if (AcikSatisindex == 1)
            {
                pkSatislar = Satis1Toplam.Tag.ToString();
                musteriadi=Satis1Baslik.Text;
                if (Satis1Firma.Tag != null)
                    pkFirma = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 2)
            {
                pkSatislar = Satis2Toplam.Tag.ToString();
                musteriadi=Satis2Baslik.Text;
                if (Satis2Firma.Tag != null)
                    pkFirma = Satis2Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 3)
            {
                pkSatislar = Satis3Toplam.Tag.ToString();
                musteriadi=Satis3Baslik.Text;
                if (Satis3Firma.Tag != null)
                    pkFirma = Satis3Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                pkSatislar = Satis4Toplam.Tag.ToString();
                musteriadi=Satis4Baslik.Text;
            }
            DB.pkSatislar = int.Parse(pkSatislar);
            DB.PkFirma = int.Parse(pkFirma);
            lueKKarti.Visible = false;
            if (odemesekli == "Diğer...")
            {
                cbOdemeSekli.Text = odemesekli;
                if (pkFirma == "1")
                {
                    pkFirma = musteriara();
                    if (pkFirma == "1")
                    {
                        yesilisikyeni();
                        cbOdemeSekli.SelectedIndex = 0;
                        //cbOdemeSekli.EditValue = "Nakit";
                        //cbOdemeSekli.Text = "Nakit";
                        return;
                    }
                }
                if (pkFirma != "1")
                {
                    frmSatisOdeme SatisOdeme = new frmSatisOdeme("0",false,0,true);
                    SatisOdeme.MusteriAdi.Tag = pkFirma;
                    SatisOdeme.satistutari.ToolTip = odemesekli;
                    SatisOdeme.satistutari.EditValue = aratoplam.EditValue;
                    SatisOdeme.ShowDialog();
                    //kaydetyazdir(SatisOdeme.kaydetmiyazdirmi.Text);//önemli
                    SatisOdeme.Dispose();
                }
            }
            else if (odemesekli == "Kredi Kartı")
            {
                lueKKarti.Properties.DataSource = DB.GetData("Select * from Bankalar where Aktif=1");
                lueKKarti.Visible = true;
                lueKKarti.Focus();
                lueKKarti.ItemIndex = 0;
            }
            else if (odemesekli == "Açık Hesap")
            {
                TaksitOdemeTarihi.Visible = true;
                //TaksitOdemeTarihi.DateTime = DateTime.Today.AddDays(30);
                if (pkFirma == "1")
                {
                    pkFirma = musteriara();
                    //if (pkFirma == "1")
                    //{
                    //    yesilisikyeni();
                    //    cbOdemeSekli.SelectedIndex = 0;
                    //    cbOdemeSekli.EditValue = "Nakit";
                    //    return;
                    //}
                }
            }
            yesilisikyeni();
        }
        private void cbOdemeSekli_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            OdemeSekli(e.Value.ToString());
           // if(e.Value.ToString()=="Diğer...")
             //  e.Value = "Nakit";
            //cbOdemeSekli.EditValue = "Nakit";
            //cbOdemeSekli.Text = "Nakit";
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
            pkSatisBarkod.EditValue = null;
            TutarFont(Satis4Toplam);
            AcikSatisindex = 1;
            yesilisikyeni();
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
           SatisDuzenKapat();
        }

        private void odemeseklidigerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli("Diğer...");
        }

        private void odemeseklikkartiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli("Kredi Kartı");
        }
        private void odemesekliacikhesapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli("Açık Hesap...");
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
            if (lueSatisTipi.Text == "İade")
            {
                decimal miktar = decimal.Parse(yenimiktar);
                if (miktar > 0) 
                    miktar = miktar * -1;
                yenimiktar = miktar.ToString();
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
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle,gridColumn3, yenimiktar);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle,gridColumn33, iskontogercekyuzde.ToString());
            
        }
        private void iskontoTutar_EditValueChanged(object sender, EventArgs e)
        {
            ceiskontoyuzde.EditValue = null;
        }
        void dockPanel1Gizle()
        {
            if (dockPanel1.Width > 45)
            {
                dockPanel1.Width = 40;
                dockPanel1.SendToBack();
                yesilisikyeni();
            }
        }
        void dockPanel1goster()
        {
            if (dockPanel1.Width == 810)
            {
                dockPanel1.Width = 40;
                dockPanel1.SendToBack();
            }
            else
            {
                dockPanel1.Width = 810;
                dockPanel1.BringToFront();
            }
        }
        private void xtraTabPage5_Click(object sender, EventArgs e)
        {
            if (xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Count == 0)
               Hizlibuttonlariyukle();
            dockPanel1goster();
        }

        private void iskontoTutar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        private void repositoryItemButtonEdit1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
        }
        private void pkSatisBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //fiş düzenle
            if (e.KeyCode == Keys.Enter)
            {
                
                if (pkSatisBarkod.Text == "") return;
                lueFis.EditValue = int.Parse(pkSatisBarkod.Text);
                if (lueFis.EditValue == null)
                    lueFis.EditValue = pkSatisBarkod.Text;
                //lueFis_EditValueChanged(sender, e);
                //Satis4Toplam.Tag = pkSatisBarkod.Text;
                //SatisGetir();

                //gCSatisDuzen.Visible = true;
                //Satis4Toplam_Click(sender, e);
                
            }
        }

        private void gCSatisDuzen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmMusteriKarti kart = new frmMusteriKarti(gCSatisDuzen.Tag.ToString(), "");
            DB.PkFirma = int.Parse(gCSatisDuzen.Tag.ToString());
            kart.ShowDialog();
        }

        private void gridControl1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
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

        private void lueSatisTipi_EditValueChanged(object sender, EventArgs e)
        {
            if (lueSatisTipi.EditValue.ToString() == "5" || lueSatisTipi.EditValue.ToString() == "9")//iade ve değişim
                gciade.Visible = true;
            else
                gciade.Visible = false;
            if (lueSatisTipi.EditValue.ToString() == "4" || lueSatisTipi.EditValue.ToString() == "3")//fatura ve irsaliye
                gckdv.Visible = true;
            else
                gckdv.Visible = false;
            FisListesi();
            if (lueSatisTipi.Text == "İade")
               DB.ExecuteSQL("UPDATE SatisDetay SET Adet=Adet*-1 where fkSatislar=" + pkSatisBarkod.Text);
            else
                DB.ExecuteSQL("UPDATE SatisDetay SET Adet=abs(Adet) where fkSatislar=" + pkSatisBarkod.Text);
            yesilisikyeni();
        }
        void SatisDigerBilgileriGetir()
        {
            //TODO:SatisDigerBilgileriGetir
            DataTable dt = DB.GetData("select pkSatislar,fkFirma from Satislar where pkSatislar=0");
            btnAciklamaGirisi.ToolTip = dt.Rows[0]["Aciklama"].ToString();
        }
        void SatisDetayGetir(string pkSatislar)
        {
            pkSatisBarkod.Text = pkSatislar;
            gridControl1.DataSource = DB.GetData("exec sp_ProjeSatisDetay " + pkSatislar + ",0");
            toplamlar();
            gridView1.AddNewRow();
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
                if (dr["Adet"].ToString() != "")
                    EklenenMiktar = decimal.Parse(dr["Adet"].ToString());
            }
            if (EklenenMiktar == 1)
                EklenenMiktar = HizliMiktar;
            decimal f = 0;
            decimal.TryParse(barkod, out f);
            if (dr != null && dr["pkProjeSatisDetay"].ToString() != "") return; 
            if (barkod == "" || f == 0) return;
                YeniSatisEkle();
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
            //iade
            if (lueSatisTipi.Text=="İade") EklenenMiktar = EklenenMiktar * -1;
            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti FROM StokKarti where Barcode='" + barkod + "'");
            if (dtStokKarti.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                StokKartiHizli.ceBarkod.EditValue = barkod;
                StokKartiHizli.ShowDialog();
                if (StokKartiHizli.TopMost == true)
                {
                    dtStokKarti = DB.GetData("select pkStokKarti From StokKarti WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
                }
                else
                {
                    yesilisikyeni();
                    StokKartiHizli.Dispose();
                    return;
                }
                StokKartiHizli.Dispose();
            }
            string pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkSatislar", pkSatisBarkod.Text));
            arr.Add(new SqlParameter("@SatisFiyatGrubu", lueFiyatlar.EditValue.ToString()));        
            arr.Add(new SqlParameter("@Adet", EklenenMiktar.ToString().Replace(",",".")));
            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
            arr.Add(new SqlParameter("@iskontoyuzde", "0"));
            arr.Add(new SqlParameter("@iskontotutar", "0"));
            string s = DB.ExecuteScalarSQL("exec sp_ProjeSatisDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti,@iskontoyuzde,@iskontotutar", arr);
            if (s != "Satis Detay Eklendi.")
            {
                MessageBox.Show(s);
                return;
            }
            HizliMiktar = 1;
            DataTable dtKalanAdet=
            DB.GetData("SELECT  pkAlisDetay,KalanAdet FROM AlisDetay WHERE fkStokKarti = "+pkStokKarti+" and KalanAdet>0 order by Tarih");

            decimal KalanAdet = 0;
            for (int i = 0; i < dtKalanAdet.Rows.Count; i++)
			{
                KalanAdet = decimal.Parse(dtKalanAdet.Rows[i]["KalanAdet"].ToString());
                if (KalanAdet > EklenenMiktar)
                {
                    DB.ExecuteSQL("UPDATE AlisDetay SET KalanAdet=KalanAdet -" + EklenenMiktar.ToString().Replace(",", ".") +
                        " where pkAlisDetay=" + dtKalanAdet.Rows[i]["pkAlisDetay"].ToString());
                    break;
                }
                else
                {
                    //seçilen alış faturasında yoksa ikinci alış faturasından düş
                    DB.ExecuteSQL("UPDATE AlisDetay SET KalanAdet=KalanAdet -" + EklenenMiktar.ToString().Replace(",",".") +
                        " where pkAlisDetay=" + dtKalanAdet.Rows[i]["pkAlisDetay"].ToString());
                    EklenenMiktar = EklenenMiktar - KalanAdet;
                }
			}
            //yesilisikyeni();
        }
        void YeniSatis()
        {
            string sql = "";
            string fisno = "0";
            bool yazdir = false;
            ArrayList list = new ArrayList();
            string pkFirma = "1";
            if (AcikSatisindex == 1)
                pkFirma = Satis1Firma.Tag.ToString();
            if (AcikSatisindex == 2)
                pkFirma = Satis2Firma.Tag.ToString();
            if (AcikSatisindex == 3)
                pkFirma = Satis3Firma.Tag.ToString();
            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Siparis", "0"));
            list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));//iskontoTutar.Value.ToString().Replace(",", ".")));

            sql = "INSERT INTO ProjeSatislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar,0,0) SELECT IDENT_CURRENT('ProjeSatislar')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                SatisDetayGetir(Satis1Toplam.Tag.ToString());
            }
            if (AcikSatisindex == 2 && Satis2Toplam.Tag.ToString() == "0")
            {
                Satis2Toplam.Tag = fisno;
                SatisDetayGetir(Satis2Toplam.Tag.ToString());
            }
            if (AcikSatisindex == 3 && Satis3Toplam.Tag.ToString() == "0")
            {
                Satis3Toplam.Tag = fisno;
                SatisDetayGetir(Satis3Toplam.Tag.ToString());
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Bilgileri ve Genel Durumu. Yapım Aşamasındadır...");
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(6,0);
            string pkSatislar = "0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2)
                pkSatislar = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3)
                pkSatislar = Satis3Toplam.Tag.ToString();
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
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr == null)
                    gridView1.DeleteRow(i);
                else if (dr["pkProjeSatisDetay"].ToString() == "")
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
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
                DB.ExecuteSQL("UPDATE SatisDetay SET SatisFiyati=" + g.Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
                decimal iskontoyuzde = 0;
                if (dr["iskontoyuzdetutar"].ToString() != "")
                    iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
                decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());
                decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
                decimal Miktar = Convert.ToDecimal(g);
                decimal iskontogercektutar = Convert.ToDecimal(dr["iskontotutar"].ToString());

                if (iskontogercektutar > 0)
                {
                    iskontogercekyuzde = (iskontogercektutar * 100) / (Fiyat * Miktar);
                }
                gridView1.SetFocusedRowCellValue(gridColumn3, g);
                gridView1.SetFocusedRowCellValue(gridColumn33, iskontogercekyuzde.ToString());
                yesilisikyeni();
            }
        }

        private void pkSatisBarkod_Enter(object sender, EventArgs e)
        {
            pkSatisBarkod.Text = "";
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
            if (e.Control && e.KeyValue == 222)
            {
                iskontoyagit_ctrli();
            }
            //GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            //if (ghi.Column == null) return;
            //if (ghi.Column.Name == "gridColumn1")
                //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


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
            DataTable dt = DB.GetData("select pkProjeSatislar,fkFirma from ProjeSatislar where Siparis<>1 and fkKullanici=" + DB.fkKullanicilar);
            //select pkSatislar,fkFirma,f.OzelKod from Satislar s with(nolock) "+ 
            //"inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma where Siparis<>1 and fkKullanici="+DB.fkKullanicilar);
            int c =dt.Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string pkSatislar = dt.Rows[i]["pkProjeSatislar"].ToString();
                    string fkFirma=dt.Rows[i]["fkFirma"].ToString();
                    AcikSatisindex = i+1;
                    if (i == 0)
                    {
                        //Showmessage("Ekranda tamamlanmamış " + c.ToString() + " Adet işleminiz bulumnaktadır.", "K");
                        //DevExpress.XtraEditors.XtraMessageBox.Show("Ekranda tamamlanmamış " + c.ToString() + " Adet işleminiz bulumnaktadır.", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Satis1Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + fkFirma);
                        Satis1Firma.Tag = dtMusteri.Rows[0]["pkFirma"].ToString();
                        Satis1Baslik.ToolTip = dtMusteri.Rows[0]["OzelKod"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis1Baslik.Text = Satis1Baslik.ToolTip;
                    }
                    else if (i == 1)
                    {
                        Satis2Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select * from Firmalar where pkFirma=" + fkFirma);
                        Satis2Firma.Tag = dtMusteri.Rows[0]["pkFirma"].ToString();
                        Satis2Baslik.ToolTip = dtMusteri.Rows[0]["OzelKod"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis2Baslik.Text = Satis2Baslik.ToolTip;
                        //Satis2Toplam.Tag = dt.Rows[i]["pkSatislar"].ToString();
                        SatisDetayGetir(Satis2Toplam.Tag.ToString());
                    }
                    else if (i == 2)
                    {
                        Satis3Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select * from Firmalar where pkFirma=" + fkFirma);
                        Satis3Firma.Tag = dtMusteri.Rows[0]["pkFirma"].ToString();
                        Satis3Baslik.ToolTip = dtMusteri.Rows[0]["OzelKod"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis3Baslik.Text = Satis3Baslik.ToolTip;
                        //Satis3Toplam.Tag = dt.Rows[i]["pkSatislar"].ToString();
                        SatisDetayGetir(Satis3Toplam.Tag.ToString());
                    }
                    if (i > 2) break;
                    //yesilisikyeni();
                }
                AcikSatisindex = 1;
                SatisDetayGetir(Satis1Toplam.Tag.ToString());
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

        private void button1_Click(object sender, EventArgs e)
        {
            ceiskontoyuzde.Value = 0;
            yesilisikyeni();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ceiskontoTutar.Value = 0;
            yesilisikyeni();
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        private void Satis1Baslik_DoubleClick(object sender, EventArgs e)
        {
            musteriara();
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

        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;
            string iskontotutar =
               ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
            iskontotutarDegistir(iskontotutar);
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
            //özel kod veya adı için getir ve yenile
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + DB.PkFirma);
            string firmadi = dt.Rows[0]["Firmaadi"].ToString();
            string ozelkod = dt.Rows[0]["OzelKod"].ToString();
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        //Satis1Firma.Tag = DB.PkFirma;
                        Satis1Baslik.Text = ozelkod + "-" + firmadi;
                        Satis1Baslik.ToolTip = Satis1Baslik.Text;
                        break;
                    }
                case 2:
                    {
                        //Satis2Firma.Tag = DB.PkFirma;
                        Satis2Baslik.Text = ozelkod + "-" + firmadi;
                        Satis2Baslik.ToolTip = Satis2Baslik.Text;
                        break;
                    }
                case 3:
                    {
                       // Satis3Firma.Tag = DB.PkFirma;
                        Satis3Baslik.Text = ozelkod + "-" + firmadi;
                        Satis3Baslik.ToolTip = Satis3Baslik.Text;
                        break;
                    }
            }
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
           pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Fiş No " + pkSatisBarkod.Text + "',0) SELECT IDENT_CURRENT('EtiketBas')");
           DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["fkStokKarti"].ToString() + ",1,0,getdate())");
           frmEtiketBas EtiketBas = new frmEtiketBas();
           EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
           EtiketBas.ShowDialog();
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
            Hizlibuttonlariyukle();
        }

        private void simpleButton8_Click_2(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("13");//6
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            Hizlibuttonlariyukle();
        }

        private void groupControl6_MouseClick(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            frmSatisOdeme SatisOdeme = new frmSatisOdeme(pkSatisBarkod.Text,false,0,true);
            SatisOdeme.Tag = "1";
            SatisOdeme.MusteriAdi.Tag = DB.PkFirma;
            SatisOdeme.ShowDialog();
            SatisOdeme.Dispose();
        }
    }
}

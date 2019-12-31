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
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using GPTS.islemler;
using Excel = Microsoft.Office.Interop.Excel;

namespace GPTS
{
    public partial class ucAlisTrans : DevExpress.XtraEditors.XtraUserControl
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis="";//AccessibleDescription
        int AcikSatisindex = 1;//hangi satış açık
        decimal HizliMiktar = 1;
        string fkModul = "68";//alış kod 11
        string ModulKod = "11";

     public ucAlisTrans()
     {
        InitializeComponent();
        DB.pkTedarikciler = 1;
        string Dosya = DB.exeDizini + "\\AlisFaturaGrid.xml";

        if (File.Exists(Dosya))
        {
            gridView1.RestoreLayoutFromXml(Dosya);
            gridView1.ActiveFilter.Clear();
        }
     }

     //void Showmessage(string lmesaj,string renk)
      //{
      //    frmMesajBox mesaj = new frmMesajBox(200);
      //    mesaj.label1.Text = lmesaj;
      //    if (renk=="K")
      //        mesaj.label1.BackColor = System.Drawing.Color.Red;
      //    else if (renk == "B")
      //        mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
      //    else
      //        mesaj.label1.BackColor = System.Drawing.Color.Blue;
      //    mesaj.Show();
      //}

      void Yetkiler()
      {
          string sql = @"SELECT  YA.Sayi, P.Aciklama10, P.Aciklama50,YA.BalonGoster,YA.Yetki FROM  YetkiAlanlari YA with(nolock) 
INNER JOIN Parametreler P with(nolock) ON YA.fkParametreler = P.pkParametreler
LEFT JOIN Moduller M with(nolock) ON M.pkModuller=P.fkModul 
where M.Kod='11' and YA.fkKullanicilar=" + DB.fkKullanicilar;
          DataTable dtYetkiler = DB.GetData(sql);
          for (int i = 0; i < dtYetkiler.Rows.Count; i++)
          {
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
                //    gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
                    xtraTabControl2.Visible = true;
                else if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "sktedarik")
                {
                    if (dtYetkiler.Rows[i]["Yetki"].ToString() == "True")
                        cbStokKartiTedarikci.Checked = true;
                    else
                        cbStokKartiTedarikci.Checked = false;

                }//timer1.Enabled = true;
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "GizliAlan")
                //{
                //if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                //    dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                //else
                //        dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //}
            }
      }

      void Depolar()
      {
          lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock)");
          lueDepolar.EditValue = Degerler.fkDepolar;
      }

      void DepolarGrid()
      {
          repositoryItemLookUpEdit3.DataSource = DB.GetData("select * from Depolar with(nolock)");
          //repositoryItemLookUpEdit3.EditValue = 1;
      }

      private void ucAnaEkran_Load(object sender, EventArgs e)
      {
           
          DataTable dtSirketler = DB.GetData("select * from Sirketler with(nolock)");
          if (dtSirketler.Rows[0]["FaturaTipi"].ToString() != "")
          {
              Degerler.iFaturaTipiAlis=int.Parse(dtSirketler.Rows[0]["FaturaTipiAlis"].ToString());
              cbFaturaTipi.SelectedIndex = Degerler.iFaturaTipiAlis;
          }
          if (dtSirketler.Rows[0]["tedarikci_OncekiFiyatHatirla"].ToString() == "True")
          {
              //btnFisKopyala.Visible = Degerler.Satiskopyala;
              cbOncekiFiyatHatirla.Visible = Degerler.OncekiFiyatHatirla_teda;
              cbOncekiFiyatHatirla.Checked = Degerler.OncekiFiyatHatirla_teda;
          }

            Depolar();

          TabHizliSatisGenel.Tag = "0";

          Yetkiler();
          //timer1.Enabled = true;
          lueAlisTipi.Properties.DataSource = DB.GetData(@"SELECT pkAlisDurumu, Durumu, Aktif, SiraNo
                                              FROM  AlisDurumu with(nolock) WHERE     (Aktif = 1) ORDER BY SiraNo");
          lueAlisTipi.EditValue = 2;

          repositoryItemLookUpEdit2.DataSource = DB.GetData(@"select * from ParaBirimi with(nolock) where Aktif=1");

          FaturaNo.Text = "";
          deFaturaTarihi.Text = "";
          //FisListesi();
          
          GeciciTedarikciDefault();

          timer1.Enabled = true;
          //FaturaTarihi.DateTime = DateTime.Today;

          //Depolar();
          DepolarGrid();

            //yesilyak();
            yesilisikyeni();
        }

        void yesilyak_sil()
        {
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();

            //gridView1.AddNewRow();
            //gridView1.PostEditor();
            gridView1.ShowEditor();

            
        }

        int x = 0;
        int y = 0;
        int p = 1;
        private void MyShortcuts(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.M)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                //MySettings sett = new MySettings();
                //sett.Show();
            }
        } 

        private void ButtonClick(object sender, EventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
            {
                AlisDetayEkle(((SimpleButton)sender).Tag.ToString());
                yesilisikyeni();
            }
        }

        private void ButtonClickSag(object sender, MouseEventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
                urunekle(((SimpleButton)sender).Tag.ToString());
        }

        void Hizlibuttonlariyukle()
        {
            int to = 0;
            int lef = 0;
            DataTable dtbutton = DB.GetData(@"select * from HizliStokSatis h with(nolock)
            left join (select pkStokKarti,fkStokGrup,HizliSatisAdi,Barcode,HizliSiraNo,SatisFiyati 
            from StokKarti with(nolock)) sk on sk.pkStokKarti=h.fkStokKarti order by pkHizliStokSatis");
            
            int h = 80;//dockPanel1.Height / 7;
            int w = 110;//dockPanel1.Width / 5;
            for (int i = 0; i < dtbutton.Rows.Count; i++)
            {
                string pkid = dtbutton.Rows[i]["pkStokKarti"].ToString();
                string Barcode = dtbutton.Rows[i]["Barcode"].ToString();
                string HizliSatisAdi = dtbutton.Rows[i]["HizliSatisAdi"].ToString();
                string SatisFiyati = dtbutton.Rows[i]["SatisFiyati"].ToString();
                SimpleButton sb = new SimpleButton();
                sb.AccessibleDescription = dtbutton.Rows[i]["pkHizliStokSatis"].ToString();
                sb.Name = "Btn" + (i + 1).ToString();
                sb.Text = HizliSatisAdi;
                sb.Tag =  Barcode;
                sb.ToolTip = "Satış Fiyatı=" + SatisFiyati;
                sb.ToolTipTitle = "Kodu: " + Barcode;
                sb.Height = h;
                sb.Width = w;
                sb.Click += new EventHandler(ButtonClick);
                sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseEnter);
                sb.Left = lef;
                sb.Top = to;
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
                TabHizliSatisGenel.Controls.Add(sb);
            }

            TabHizliSatisGenel.Tag = "1";
        }

        void SatisTemizle()
        {
            if (AcikSatisindex == 1)
            {
                Satis1Toplam.Tag = 0;
                Satis1Toplam.Text = "0,0";
                pkAlislar.Text = "0";
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
                AcikSatisindex = 1;
            }
            FaturaNo.Text = "";
            deFaturaTarihi.EditValue = null;
            deAlisTarihi.EditValue = null;
            temizle(AcikSatisindex);

            pkAlislar.Text = "0";

            lbKalanBakiyesi.Text = "";
            lbKalanBakiyesi.Visible = false;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //satır sil
            if (gridView1.DataRowCount == 0)
            {
                GeciciTedarikciDefault();
                yesilisikyeni();
                return;
            }

            string s = formislemleri.MesajBox("Alış İptal Edilsin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Alış İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.No)
            //{
            //    yesilisikyeni();
            //    return;
            //}  
          
            string sonuc="",pkSatislar="0";

            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2)
                pkSatislar = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3)
                pkSatislar = Satis3Toplam.Tag.ToString();
            else if (AcikSatisindex == 4)
                pkSatislar = Satis4Toplam.Tag.ToString();

            sonuc = DB.ExecuteScalarSQL("EXEC spAlisSil " + pkSatislar+",1");
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

            if (gridView1.DataRowCount == 1)
            {
                string s = formislemleri.MesajBox("Ürün Kalmayacağı için, Alış İptal Edilsin mi?", Degerler.mesajbaslik, 3, 1);
                if (s == "0")
                    return;
            }

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            int sonuc = DB.ExecuteSQL("DELETE FROM AlisDetay WHERE pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            if (sonuc != 1)
            {
                MessageBox.Show("Alış Detay F2 sonuc:" + sonuc);
                return;
            }

            //yapılan iskontolarıda sil
            DB.ExecuteSQL("DELETE FROM iskontolar WHERE fkAlisDetay=" + dr["pkAlisDetay"].ToString());
            //gridView1.DeleteSelectedRows();
            if (gridView1.DataRowCount == 1)
            {
                DB.ExecuteSQL("DELETE FROM Alislar WHERE pkAlislar=" + pkAlislar.Text);
                DB.ExecuteSQL("delete from iskontolar where fkAlisDetay in(select pkAlisDetay From AlisDetay where fkAlislar="+
                    pkAlislar.Text +")");
                DB.ExecuteSQL("DELETE FROM AlisDetay WHERE fkAlislar=" + pkAlislar.Text);

                pkAlislar.Text = "0";
                if (AcikSatisindex == 1)
                    Satis1Toplam.Tag = "0";
                else if (AcikSatisindex == 2)
                    Satis2Toplam.Tag = "0";
                else if (AcikSatisindex == 3)
                    Satis3Toplam.Tag = "0";

                pkAlislar.Text = "0";

                if (AcikSatisindex == 4)
                {
                    ilkYuklemeYenile();
                    return;
                }
                else
                    GeciciTedarikciDefault();

                SatisTemizle();

            }
            yesilisikyeni();
        }

        void YeniAlisEkle()
        {
            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
                YeniAlis();
            else if (AcikSatisindex == 2 && Satis2Toplam.Tag.ToString() == "0")
                YeniAlis();
            else if (AcikSatisindex == 3 && Satis3Toplam.Tag.ToString() == "0")
                YeniAlis();
        }

        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //ctrl + i iskonto
        //if (e.Control && e.KeyValue == 222)
        //{
        //    gridView1.FocusedColumn = gridColumn33;
        //    gridView1.ShowEditor();
        //    // gridView1.CloseEditor();
        //}
        if (e.Control && e.Shift && gridView1.DataRowCount > 0)
        {
            //üst satırı kopyala
            DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);

            AlisDetayEkle(dr["Barcode"].ToString());
            //gridView1.ShowEditor();
            
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
           
            AlisDetayEkle(kod);

            yesilisikyeni();

            if(gridView1.DataRowCount>0)
                AlisBilgileriGetir(gridView1.DataRowCount - 1);
        }
        }

        void urunaraekle(string ara)
        {
            frmStokAra StokAra = new frmStokAra(ara);
            StokAra.Tag = "2";
            StokAra.ShowDialog();
            if (StokAra.TopMost == false) 
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();
                    //if (StokAra.gridView1.GetSelectedRows()[i] >= 0)
                    //{
                        DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                        AlisDetayEkle(dr["Barcode"].ToString());
                    //}
                }
                //for (int i = 0; i < StokAra.gridView1.DataRowCount; i++)
                //{
                //    DataRow dr = StokAra.gridView1.GetDataRow(i);
                //    if (dr["Sec"].ToString() == "True")
                //        AlisDetayEkle(dr["Barcode"].ToString());
                //}
            }
            StokAra.Dispose();
            yesilisikyeni();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle != -2147483647)
                gridView1.AddNewRow();

            string girilen = "";
            try
            {
                girilen =
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
            }
            catch
            {
            }
            urunaraekle(girilen);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            gridView1.AddNewRow();
            string girilen =
            ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;

            urunaraekle(girilen);
        }
        
        private void urunekle(string barkod)
        {
            if (barkod == "") return;
            string ilktarih = "";
            string sontarih = "";
            string fkUrunlerNoPromosyon = "";
            int DususAdet = 1;
            DataTable dturunler = DB.GetData("select * From StokKarti with(nolock) WHERE Barcode='" + barkod + "'");
            if (dturunler.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                Degerler.stokkartisescal = true;
                StokKartiHizli.ceBarkod.EditValue = barkod;

                StokKartiHizli.ShowDialog();

                if (StokKartiHizli.TopMost == true)
                {
                    dturunler = DB.GetData("select * From StokKarti with(nolock) WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
                }
                else
                {
                    StokKartiHizli.Dispose();
                    return;
                }
                StokKartiHizli.Dispose();
            }
            AlisDetayEkle(barkod);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

            //SATIR SİL
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            if (gridView1.DataRowCount == 1)
            {
                string s = formislemleri.MesajBox("Ürün Kalmayacağı için, Alış İptal Edilsin mi?", Degerler.mesajbaslik, 3, 1);
                if (s == "0")
                    return;
            }


            int sonuc = DB.ExecuteSQL("DELETE FROM AlisDetay WHERE pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            if (sonuc != 1)
            {
                MessageBox.Show("Alış Detay F2 sonuc:" + sonuc);
                return;
            }

            //gridView1.DeleteSelectedRows();

            // if (gridView1.DataRowCount == 0)// && AcikSatisindex!=4)
            if (DB.GetData("select * from AlisDetay with(nolock) where fkAlislar=" + pkAlislar.Text).Rows.Count == 0)
            {
                DB.ExecuteSQL("DELETE FROM Alislar WHERE pkAlislar=" + pkAlislar.Text);
                pkAlislar.Text = "0";
                if (AcikSatisindex == 1)
                    Satis1Toplam.Tag = "0";
                else if (AcikSatisindex == 2)
                    Satis2Toplam.Tag = "0";
                else if (AcikSatisindex == 3)
                    Satis3Toplam.Tag = "0";

                pkAlislar.Text = "0";
                SatisTemizle();

                deAlisTarihi.EditValue = null;
                //deAlisTarihi.Enabled = false;
                //cbOdemeSekli.Enabled = false;

                //if (AcikSatisindex == 4)
                //    Yenile();
                //else
                GeciciTedarikciDefault();
            }
            //else
            //  MessageBox.Show("Lütfen Yenile Butonuna basınız");

            yesilisikyeni();

            //lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
            //lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);

            /*
            if (gridView1.FocusedRowHandle == -2147483647 || gridView1.FocusedRowHandle >=0)
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dr == null) return;
                if (dr["Adet"].ToString() != "")
                    simpleButton1_Click(sender, e);
            }
            //if (gridView1.DataRowCount == 0)
            //{
                yesilisikyeni();
                return;
            //}
            */
        }

        void resimolustur(SimpleButton sb)
        {
            x = x + 25;
            y = y + 25;
            PictureBox pictureBox = new PictureBox();
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
            iskontoTutarGuncelle();

            if (AcikSatisindex == 1)
            {
                AlisDetayGetir(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
            }
            else if (AcikSatisindex == 2)
            {
                AlisDetayGetir(Satis2Toplam.Tag.ToString());
                TutarFont(Satis2Toplam);
            }
            else if (AcikSatisindex == 3)
            {
                AlisDetayGetir(Satis3Toplam.Tag.ToString());
                TutarFont(Satis3Toplam);
            }
            else if (AcikSatisindex == 4)
            {
                AlisDetayGetir(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
            }
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();
            
            if (AcikSatisindex == 1) fontayarla(Satis1Toplam);
            if (AcikSatisindex == 2) fontayarla(Satis2Toplam);
            if (AcikSatisindex == 3) fontayarla(Satis3Toplam);
            if (AcikSatisindex == 4) fontayarla(Satis4Toplam);

            //gridView1.ShowEditor();
            SendKeys.Send("{ENTER}");

        }

        private bool SatisVar()
        {

            if (gridView1.DataRowCount == 0)
            {
                formislemleri.Mesajform("Önce Satış Yapınız!", "K", 150);
                return false;
            }
             return true;
        }

        private bool aliskaydet(bool yazdir, bool odemekaydedildi)
        {
            //satış fiyatlarınıda güncelle aşapıda
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkAlislar", pkAlislar.Text));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@FaturaNo", FaturaNo.Text));
            list.Add(new SqlParameter("@fkSatisDurumu", lueAlisTipi.EditValue.ToString()));
            
            if (deAlisTarihi.EditValue == null || deAlisTarihi.Text == "")
                list.Add(new SqlParameter("@GuncellemeTarihi", DateTime.Now));
            else
                list.Add(new SqlParameter("@GuncellemeTarihi", deAlisTarihi.DateTime));

            if (deFaturaTarihi.EditValue == null || deFaturaTarihi.Text=="")
                list.Add(new SqlParameter("@FaturaTarihi", DBNull.Value));
            else
                list.Add(new SqlParameter("@FaturaTarihi", deFaturaTarihi.DateTime));

            string sonuc = DB.ExecuteSQLTrans(@"UPDATE Alislar SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,FaturaNo=@FaturaNo,FaturaTarihi=@FaturaTarihi,
            fkSatisDurumu=@fkSatisDurumu,GuncellemeTarihi=@GuncellemeTarihi where pkAlislar=@pkAlislar", list);
            //TEKLİF SİPERİŞ SFATURA İSE STOKLARI ETKİLEME
            if (lueAlisTipi.EditValue.ToString()=="1" || lueAlisTipi.EditValue.ToString() == "9" || lueAlisTipi.EditValue.ToString() == "12")
            {
                return true;
            }

            if (sonuc.Length>1 && sonuc.Substring(1, 1) == "H")
            {
                //Showmessage("Hata Oluştur: " + sonuc ,"K");
                //hata oluştu ise alış ödemeleri sil
                //DB.ExecuteSQL("delete from KasaHareket where fkAlislar=" + pkAlislar.Text);
                DB.trans_basarili = false;
                return false;
            }

            #region Alış,satış fiyatlarını ve depo Mevcut güncelle
            bool alisguncelle=true;
            alisguncelle = cbStokKartiniGuncelle.Checked;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string pkAlisDetay = dr["pkAlisDetay"].ToString();
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string fkDepolar = dr["fkDepolar"].ToString();
                string Adet = dr["Adet"].ToString();

                //Mevcut güncelle
                decimal satilanadet = decimal.Parse(Adet);

                string sql = "";
                #region Stok Kartı Mevcut Güncelle
                if (satilanadet > 0)
                    sql = "UPDATE StokKarti SET Aktif=1,SonAlisTarihi=getdate(), Mevcut=isnull(Mevcut,0)+" + satilanadet.ToString().Replace(",", ".")
                        + " where pkStokKarti=" + fkStokKarti;
                else
                    sql = "UPDATE StokKarti SET Aktif=1,SonAlisTarihi=getdate(), Mevcut=isnull(Mevcut,0)-" + satilanadet.ToString().Replace(",", ".").Replace("-", "")
                +" where pkStokKarti=" + fkStokKarti;

                sonuc = DB.ExecuteSQLTrans(sql);
                if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
                {
                    DB.trans_basarili = false;
                    return false;
                }
                #endregion

                #region Stok Kartı Depo Mevcut Güncelle
                if (satilanadet > 0)
                    sql = "UPDATE StokKartiDepo SET tarih=getdate(), MevcutAdet=isnull(MevcutAdet,0)+" + satilanadet.ToString().Replace(",", ".")
                        + " where fkStokKarti="+ fkStokKarti+ " and fkDepolar="+fkDepolar;
                else
                    sql = "UPDATE StokKartiDepo SET tarih=getdate(), MevcutAdet=isnull(MevcutAdet,0)-" + satilanadet.ToString().Replace(",", ".").Replace("-", "")
                         + " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar;

                sonuc = DB.ExecuteSQLTrans(sql);
                if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
                {
                    DB.trans_basarili = false;
                    return false;
                }

                #endregion

                #region Alis Satış Fiyatları Güncelle düzenleme değilse
                if (alisguncelle && AcikSatisindex<4)
                {
                    sql = "update stokkarti set AlisFiyati=" + dr["AlisFiyati"].ToString().Replace(",", ".") +
                             ",AlisFiyatiKdvHaric=" + dr["AlisFiyatiKdvHaric"].ToString().Replace(",", ".") +
                             "  where pkStokKarti=" + fkStokKarti;

                    sonuc = DB.ExecuteSQLTrans(sql);
                    if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
                    {
                        DB.trans_basarili = false;
                        return false;
                    }

                    //if (AcikSatisindex== 4)
                    //{
                    //    DataTable dt=DB.GetData("select pkAlisDetay,sk.stokadi from AlisDetay ad with(nolock) " +
                    //      " left join StokKarti sk with(nolock) on sk.pkStokKarti=ad.fkStokKarti where ad.AlisFiyati<>sk.AlisFiyati and pkAlisDetay=" + pkAlisDetay);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        DialogResult secim;
                    //        string mesaj = "";
                    //        mesaj = mesaj + dt.Rows[0]["stokadi"].ToString()+" Alış Fiyatları Değişti,\n\r Güncellemek istiyormusunuz?";
                    //        secim = DevExpress.XtraEditors.XtraMessageBox.Show(mesaj, "Hitit2012", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    //        if (secim == DialogResult.Yes)
                    //        {
                    //            sql = sql + ",AlisFiyati=" + dr["AlisFiyati"].ToString().Replace(",", ".") +
                    //           ",AlisFiyatiKdvHaric=" + dr["AlisFiyatiKdvHaric"].ToString().Replace(",", ".") +
                    //           ",alis_iskonto=" + dr["iskontoyuzdetutar"].ToString().Replace(",", ".");
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    sql = sql + ",AlisFiyati=" + dr["AlisFiyati"].ToString().Replace(",", ".") +
                    //          ",AlisFiyatiKdvHaric=" + dr["AlisFiyatiKdvHaric"].ToString().Replace(",", ".") +
                    //          ",alis_iskonto=" + dr["iskontoyuzdetutar"].ToString().Replace(",", ".");
                    //}

                    //sql = sql + ",SatisFiyati=" + dr["SatisFiyati"].ToString().Replace(",", ".") +
                    //                  ",KdvOraniAlis=" + dr["KdvOrani"].ToString().Replace(",", ".");
                }

                //sql = sql +" where pkStokKarti=" + fkStokKarti;

                //sonuc = DB.ExecuteSQLTrans(sql);
                //if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
                //{
                //    DB.trans_basarili = false;
                //    return false;
                //}
                #endregion

                #region Fiyat Başlıkları
                DataTable dtFiyatBaslik = DB.GetData("Select pkSatisFiyatlariBaslik,Tur from SatisFiyatlariBaslik with(nolock) where Aktif=1");
                for (int fi = 0; fi < dtFiyatBaslik.Rows.Count; fi++)
                {
                    string fkSatisFiyatlariBaslik =  dtFiyatBaslik.Rows[fi]["pkSatisFiyatlariBaslik"].ToString();
                    string tur=dtFiyatBaslik.Rows[fi]["Tur"].ToString();

                    //yoksa ekle varsa güncelle
                    if (DB.GetData("select * from SatisFiyatlari with(nolock) where fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik + " and fkStokKarti=" + dr["fkStokKarti"].ToString()).Rows.Count == 0)
                    {
                        DB.ExecuteSQLTrans("INSERT INTO SatisFiyatlari (fkSatisFiyatlariBaslik,SatisFiyatiKdvli,fkStokKarti)" +
                            " VALUES(" + fkSatisFiyatlariBaslik + ",0," + dr["fkStokKarti"].ToString() + ")");
                    }
                    else 
                    {
                        if (alisguncelle)
                        {
                            //nakit
                            if (tur == "1")
                                sql = "UPDATE SatisFiyatlari SET SatisFiyatiKdvli=" + dr["SatisFiyati"].ToString().Replace(",", ".") +
                                " where fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik + " and fkStokKarti=" + dr["fkStokKarti"].ToString();
                            //kredi kartı
                            else if (tur == "2")
                            {
                                if (dr["SatisFiyati2"].ToString() == "")
                                    sql = "UPDATE SatisFiyatlari SET SatisFiyatiKdvli=" + dr["SatisFiyati"].ToString().Replace(",", ".") +
                                 " where fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik + " and fkStokKarti=" + dr["fkStokKarti"].ToString();
                                else
                                    sql = "UPDATE SatisFiyatlari SET SatisFiyatiKdvli=" + dr["SatisFiyati2"].ToString().Replace(",", ".") +
                                 " where fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik + " and fkStokKarti=" + dr["fkStokKarti"].ToString();
                            }
                            else if (tur == "2")
                            {
                                //diğer fiyatlara şimdilik dokunma
                            }
                                DB.ExecuteSQLTrans(sql);
                            //stokkartı satışfiyatınıda güncelle
                            sql = "UPDATE StokKarti SET SatisFiyati=" + dr["SatisFiyati"].ToString().Replace(",", ".") +
                                " where pkStokKarti=" + dr["fkStokKarti"].ToString();

                            DB.ExecuteSQLTrans(sql);

                            sql = "UPDATE StokKarti SET SatisFiyati=sk.SatisFiyatiKdvHaric-((sk.SatisFiyati*sk.KdvOrani)/(100+sk.KdvOrani))" +
                               " where pkStokKarti=" + dr["fkStokKarti"].ToString();

                            DB.ExecuteSQLTrans(sql);
                        }
                    }
                }
                #endregion
            }//for döngü sonu

            #endregion

            #region Stok Kartındaki Tedarikçileri Güncelle
            if (cbStokKartiTedarikci.Checked)
            {
                string pkTedarikciler = "0";
                switch (AcikSatisindex)
                {
                    case 1:
                        {
                            pkTedarikciler = Satis1Firma.Tag.ToString();
                            break;
                        }
                    case 2:
                        {
                            pkTedarikciler = Satis2Firma.Tag.ToString();
                            break;
                        }
                    case 3:
                        {
                            pkTedarikciler = Satis3Firma.Tag.ToString();
                            break;
                        }
                    case 4:
                        {
                            pkTedarikciler = Satis4Firma.Tag.ToString();
                            break;
                        }
                }

                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    DataRow dr = gridView1.GetDataRow(i);
                    string sql = "UPDATE StokKarti SET  Aktarildi=0,Aktif=1,fkTedarikciler=" + pkTedarikciler +
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString();
                    DB.ExecuteSQLTrans(sql);
                }
            }
            #endregion

            return true;
        }

        void GeciciTedarikciDefault()
        {
            DataTable dtMusteriler = DB.GetData("select pkTedarikciler,Firmaadi from Tedarikciler with(nolock) where pkTedarikciler=1");
            if (dtMusteriler.Rows.Count == 0)
            {
                dtMusteriler = DB.GetData("select pkTedarikciler,Firmaadi from Tedarikciler with(nolock) where GeciciMusteri=1");
            }
            else
            {
                DB.pkTedarikciler = int.Parse(dtMusteriler.Rows[0]["pkTedarikciler"].ToString());
                DB.TedarikciAdi = DB.pkTedarikciler + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
                
                //sıfırla
                Satis1Firma.Tag = DB.pkTedarikciler;
                Satis1Baslik.ToolTip = DB.TedarikciAdi;
                Satis1Baslik.Text = DB.TedarikciAdi;

                Satis2Firma.Tag = DB.pkTedarikciler;
                Satis2Baslik.ToolTip = DB.TedarikciAdi;
                Satis2Baslik.Text = DB.TedarikciAdi;

                Satis3Firma.Tag = DB.pkTedarikciler;
                Satis3Baslik.ToolTip = DB.TedarikciAdi;
                Satis3Baslik.Text = DB.TedarikciAdi;

                Satis4Firma.Tag = DB.pkTedarikciler;
                Satis4Baslik.ToolTip = DB.TedarikciAdi;
                Satis4Baslik.Text = DB.TedarikciAdi;
            }
        }

        void temizle(int aciksatisno)
        {
            DataTable dtMusteriler = DB.GetData("select pkTedarikciler,Firmaadi from Tedarikciler with(nolock) where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
            {
                DB.ExecuteSQL("UPDATE Tedarikciler SET GeciciMusteri=1 WHERE pkTedarikciler=1");
            }
            else
            {
                DB.pkTedarikciler = int.Parse(dtMusteriler.Rows[0]["pkTedarikciler"].ToString());
                DB.TedarikciAdi = DB.pkTedarikciler + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
            }
            if (aciksatisno == 1)
            {
                Satis1Baslik.Text = DB.TedarikciAdi;
                Satis1Firma.Tag = DB.pkTedarikciler;
            }
            else if (aciksatisno == 2)
            {
                Satis2Baslik.Text = DB.TedarikciAdi;
                Satis2Firma.Tag = DB.pkTedarikciler;
            }
            else if (aciksatisno == 3)
            {
                Satis3Baslik.Text = DB.TedarikciAdi;
                Satis3Firma.Tag = DB.pkTedarikciler;
            }
            else if (aciksatisno == 4)
            {
                Satis4Baslik.Text = DB.TedarikciAdi;
                Satis4Firma.Tag = DB.pkTedarikciler;
            }

            btnAciklamaGirisi.ToolTip = "";
            lueAlisTipi.EditValue = 2;
            gridControl1.DataSource = null;
            //lbKalanBakiyesi.Visible = false;
        }

        public bool Kaydet()
        {
            if (DB.GetData("select * from Sirketler").Rows[0]["TedarikciZorunluUyari"].ToString() == "True")
            {
                if ((AcikSatisindex == 1 && Satis1Firma.Tag.ToString() == "1") ||
                    (AcikSatisindex == 2 && Satis2Firma.Tag.ToString() == "1") ||
                    (AcikSatisindex == 3 && Satis3Firma.Tag.ToString() == "1"))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Tedarikçi Olamaz.\n (Ayarlardan Kaldırabilirsiniz.)!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _TedarikciAra();
                    return false;
                }
            }
            if (OnceSatisYapiniz() == false) return false;
            DialogResult secim;
            string mesaj = "";
            mesaj = mesaj + "Alış Faturası Bilgilerini Onaylıyormusunuz?";
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(mesaj, "Hitit2012", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return false;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("update AlisDetay set KalanAdet=isnull(KalanAdet,0)+" + dr["Adet"].ToString().Replace(",", ".")
                    + " where fkStokKarti=" + dr["fkStokKarti"].ToString());
                //iskontolu alis fiyatı 04.03.2013
                decimal AlisFiyati = 0, iskontotutar = 0, AlisFiyatiiskontolu = 0;
                decimal.TryParse(dr["AlisFiyati"].ToString(), out AlisFiyati);
                decimal.TryParse(dr["iskontotutar"].ToString(), out iskontotutar);
                AlisFiyatiiskontolu = AlisFiyati - iskontotutar;
                DB.ExecuteSQL(@"update StokKarti Set AlisFiyatiiskontolu=" + AlisFiyatiiskontolu.ToString().Replace(",", ".") + " where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }
            //satış fiyatlarıda içinde
            if (aliskaydet(true, false)==false) return false;
            //kdv hariç null olanları guncelle
            DB.ExecuteSQL(@"update StokKarti Set AlisFiyatiKdvHaric=AlisFiyati-(AlisFiyati*KdvOrani)/(100+KdvOrani)
            where AlisFiyatiKdvHaric is null");
            //kdv dahil null olanları guncelle
            DB.ExecuteSQL(@"update StokKarti Set AlisFiyati=AlisFiyatiKdvHaric+(AlisFiyatiKdvHaric*KdvOrani)/100
            where AlisFiyati is null");
            btEtiketYazdir.Tag = pkAlislar.Text;
            pkAlislar.Text = "0";
            FaturaNo.Text = "0";
            deFaturaTarihi.Text = "";
            FaturaNo.Text = "";
            return true;
        }

        public string bir_nolumusteriolamaz()
         {
             if (DB.GetData("select * from Sirketler").Rows[0]["TedarikciZorunluUyari"].ToString() == "True")
             {
                 if ((AcikSatisindex == 1 && Satis1Firma.Tag.ToString() == "1") ||
                     (AcikSatisindex == 2 && Satis2Firma.Tag.ToString() == "1") ||
                     (AcikSatisindex == 3 && Satis3Firma.Tag.ToString() == "1"))
                 {
                     //DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Tedarikçi Olamaz.\n (Ayarlardan Kaldırabilirsiniz.)!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    string fkFirma=_TedarikciAra();
                    if (fkFirma == "1")
                        return "0";
                    else

                        return fkFirma;
                 }
             }
             return "1";
         }

        void iskontoTutarGuncelle()
         {
            if (pkAlislar.Text == "" || pkAlislar.Text == "0") return;
//            string sql=@"update AlisDetay set iskontotutar=
//            case when KdvDahil=1 then 
//            (AlisFiyati*isnull(iskontoyuzdetutar,0))/100
//            else
//            (AlisFiyatiKdvHaric*isnull(iskontoyuzdetutar,0))/100
//            end 
//            where fkAlislar=" + pkAlislar.Text;

            string sql = @"update AlisDetay set iskontotutar=(AlisFiyati*isnull(iskontoyuzdetutar,0))/100 where fkAlislar=" + pkAlislar.Text;
            int sonuc = DB.ExecuteSQL(sql);
         }

        private void simpleButton37_Click(object sender, EventArgs e)
        {
            #region kontroller
            if (pkAlislar.Text == "0")
            {
                yesilisikyeni();
                return;
            }

            DataTable dtAlislar = DB.GetData("select * from Alislar with(nolock) where pkAlislar=" + pkAlislar.Text);
            if (dtAlislar.Rows.Count == 0)
            {
                formislemleri.Mesajform("Fiş Bulunamadı", "K", 200);
                SatisTemizle();
                yesilisikyeni();
                return;
            }

            if (dtAlislar.Rows[0]["Siparis"].ToString() == "True")
            {
                formislemleri.Mesajform("Fiş Daha Önce Kaydedildi", "K", 200);
                SatisTemizle();
                yesilisikyeni();
                return;
            }

            //iskontoTutarGuncelle();
           

            Application.DoEvents();
            gridView1.ClearColumnsFilter();
            Application.DoEvents();

            string pkFirma = bir_nolumusteriolamaz();

            if (pkFirma == "0") return;

            

            string _pkAlislar = "0";
            string musteriadi = "";
           
            if (AcikSatisindex == 1)
            {
                _pkAlislar = Satis1Toplam.Tag.ToString();
                musteriadi = Satis1Baslik.Text;
                if (Satis1Firma.Tag != null)
                    pkFirma = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 2)
            {
                _pkAlislar = Satis2Toplam.Tag.ToString();
                musteriadi = Satis2Baslik.Text;
                if (Satis2Firma.Tag != null)
                    pkFirma = Satis2Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 3)
            {
                _pkAlislar = Satis3Toplam.Tag.ToString();
                musteriadi = Satis3Baslik.Text;
                if (Satis3Firma.Tag != null)
                    pkFirma = Satis3Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                _pkAlislar = Satis4Toplam.Tag.ToString();
                musteriadi = Satis4Baslik.Text;
                if (Satis4Firma.Tag != null)
                    pkFirma = Satis4Firma.Tag.ToString();
            }

            if (deAlisTarihi.DateTime > DateTime.Now)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Alış Zamanı Bugünden Büyüktür. Yinede Kaydetmek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);//, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;
            }

            TimeSpan ts = DateTime.Now - deAlisTarihi.DateTime;
            if (ts.Hours > 5 && deAlisTarihi.EditValue != null && AcikSatisindex < 4)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Alış Tarihi Farkı " + ts.Hours.ToString() + " Saattir. Yinede Kaydetmek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);//, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;
            }

            if (lueAlisTipi.Text== "Teklif")
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(lueAlisTipi.Text + " Bilgilerini Kaydetmek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);//, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;
            }
            else if (lueAlisTipi.Text == "Sipariş")
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(lueAlisTipi.Text  + " Bilgilerini Kaydetmek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);//, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;
            }
            else if (lueAlisTipi.Text == "SFatura")
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(lueAlisTipi.Text + " Bilgilerini Kaydetmek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);//, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;
            }
            #endregion

            #region trasn başlat
            DB.trans_basarili = true;
            DB.trans_hata_mesaji = "";
            if (DB.conTrans == null)
                DB.conTrans = new SqlConnection(DB.ConnectionString());
            if (DB.conTrans.State == ConnectionState.Closed)
                DB.conTrans.Open();

            DB.transaction = DB.conTrans.BeginTransaction("AlisTransaction");
            #endregion

            if (deAlisTarihi.EditValue == null)
                deAlisTarihi.DateTime = DateTime.Now;

            frmAlisOdeme AlisOdeme = new frmAlisOdeme(deAlisTarihi.DateTime,lueAlisTipi.EditValue.ToString());
            AlisOdeme.TedarikciAdi.Text = musteriadi;
            AlisOdeme.TedarikciAdi.Tag = pkFirma;
            AlisOdeme.satistutari.EditValue = aratoplam.EditValue;
            AlisOdeme.pkAlislar.Text = _pkAlislar;
            AlisOdeme.ShowDialog();

            //if (AlisOdeme.Tag.ToString() == ((int)Degerler.islemDurumu.Bulunamadi).ToString())
            //{
               
            //    SatisTemizle();
            //}
            //else if (AlisOdeme.Tag.ToString() == ((int)Degerler.islemDurumu.Basarisiz).ToString())
            //{
                //DB.trans_basarili = false;
                //formislemleri.Mesajform("İşlem Başarısız Lütfen Tekrar Deneyiniz", "K", 200);
                //return;
            //}
            //else if (AlisOdeme.Tag.ToString() == ((int)Degerler.islemDurumu.Basarili).ToString()) 
            //{
                //Aliskaydet(true, false, true);//önce kaydet


            if (AlisOdeme.Tag.ToString() == ((int)Degerler.islemDurumu.Basarisiz).ToString())
            {
                DB.trans_basarili = false;
                DB.trans_hata_mesaji = "İptal edildi.";
            }

            if (DB.trans_basarili == true)
            {
                DB.trans_basarili = aliskaydet(false, true);
            }           

            AlisOdeme.Dispose();

            #region trasn sonu
            if (DB.trans_basarili == true)
            {
                DB.transaction.Commit();

                #region Alis durumu kaydet

                //string sql = @"declare @iadeAdedi int,@AlisAdedi int
                //    set @iadeadedi=(select count(*) from AlisDetay with(nolock)
                //    where iade=1 and fkAlislar=@fkAlislar)
                //    set @AlisAdedi=(select count(*) from AlisDetay with(nolock)
                //    where fkAlislar=@fkAlislar)
                //    if(@AlisAdedi>@iadeadedi and @iadeadedi>0)
                //    update Alislar set fkSatisDurumu=5 where pkAlislar=@fkAlislar
                //    else if(@AlisAdedi=@iadeadedi)
                //    update Alislar set fkSatisDurumu=8 where pkAlislar=@fkAlislar
                //    else
                //    update Alislar set fkSatisDurumu=2 where pkAlislar=@fkAlislar";
                //sql = sql.Replace("@fkAlislar", _pkAlislar);

                //DB.ExecuteSQL(sql);
                #endregion

                #region Alislar Guncelleme Tarihi güncelle

                ArrayList alist = new ArrayList();
                if (deAlisTarihi.EditValue == null)
                    deAlisTarihi.DateTime = DateTime.Now;
                if (deFaturaTarihi.EditValue == null || deFaturaTarihi.EditValue == "")
                    alist.Add(new SqlParameter("@FaturaTarihi", DBNull.Value));
                else
                    alist.Add(new SqlParameter("@FaturaTarihi", deFaturaTarihi.DateTime));

                alist.Add(new SqlParameter("@GuncellemeTarihi", deAlisTarihi.DateTime));
                DB.ExecuteSQL("UPDATE Alislar set GuncellemeTarihi=@GuncellemeTarihi,FaturaTarihi=@FaturaTarihi where pkAlislar=" + pkAlislar.Text, alist);
                #endregion

                SatisTemizle();

                SatisDuzenKapat();
            }
            else
            {
                DB.transaction.Rollback();
                formislemleri.Mesajform(DB.trans_hata_mesaji, "K", 200);
            }
            DB.conTrans.Close();
            #endregion

            yesilisikyeni();      
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
                DataTable FisDetay = DB.GetData(@"exec sp_AlisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Alislar " + fisid);
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

        void RaporOnizleme(bool Disigner)
        {
            string fisid = pkAlislar.Text;
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();

            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();

            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih 
            FROM Satislar s with(nolock)
            inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
            inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
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

        private string _TedarikciAra()
        {
            //bool okcancel = false;
            string fkTedarikciler = "1", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        fkTedarikciler = Satis1Firma.Tag.ToString();
                        break;
                    }
                case 2:
                    {
                        fkTedarikciler = Satis2Firma.Tag.ToString();
                        break;
                    }
                case 3:
                    {
                        fkTedarikciler = Satis3Firma.Tag.ToString();
                        break;
                    }
                case 4:
                    {
                        fkTedarikciler = Satis4Firma.Tag.ToString();
                        break;
                    }
            }

            frmTedarikciAra TedarikciAra = new frmTedarikciAra();
            TedarikciAra.fkFirma.Tag = fkTedarikciler;

            TedarikciAra.ShowDialog();
            fkTedarikciler = TedarikciAra.fkFirma.Tag.ToString();
            DB.pkTedarikciler = int.Parse(fkTedarikciler);

            if (pkAlislar.Text != "0" || pkAlislar.Text != "")
            {
                DB.ExecuteSQL("UPDATE Alislar SET fkFirma=" + fkTedarikciler + " where pkAlislar=" + pkAlislar.Text);
            }
            DataTable dtTedarikci = DB.GetData("select Firmaadi from Tedarikciler with(nolock) where pkTedarikciler=" + fkTedarikciler);
            if (dtTedarikci.Rows.Count == 0) return "";

            firmadi = dtTedarikci.Rows[0]["Firmaadi"].ToString();
            TedarikciAra.Dispose();
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        Satis1Firma.Tag = fkTedarikciler;
                        Satis1Baslik.Text = fkTedarikciler + "-" + firmadi;
                        Satis1Baslik.ToolTip = fkTedarikciler + "-" + firmadi;
                        //lbKalanBakiyesi.Visible = true;
                        break;
                    }
                case 2:
                    {
                        Satis2Firma.Tag = fkTedarikciler;
                        Satis2Baslik.Text = fkTedarikciler + "-" + firmadi;
                        Satis2Baslik.ToolTip = fkTedarikciler + "-" + firmadi;
                        //lbKalanBakiyesi.Visible = true;
                        break;
                    }
                case 3:
                    {
                        Satis3Firma.Tag = fkTedarikciler;
                        Satis3Baslik.Text = fkTedarikciler + "-" + firmadi;
                        Satis3Baslik.ToolTip = fkTedarikciler + "-" + firmadi;
                        //lbKalanBakiyesi.Visible = true;
                        break;
                    }
                case 4:
                    {
                        Satis4Firma.Tag = fkTedarikciler;
                        Satis4Baslik.Text = fkTedarikciler + "-" + firmadi;
                        Satis4Baslik.ToolTip = fkTedarikciler + "-" + firmadi;
                        //lbKalanBakiyesi.Visible = true;
                        break;
                    }
                default:
                    break;
            }

            BakiyeGetir(fkTedarikciler);
            yesilisikyeni();
            return fkTedarikciler;
            //btnAciklamaGirisi.Focus();
            //gridView1.Focus();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            _TedarikciAra();
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
               DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            //else
               //DB.pkStokKarti = int.Parse(pkStokKartiid);
            StokKarti.ShowDialog();

            //alış fiyatlarını güncelle
            DataTable dtAlisFiyatFarki = DB.GetData(@"select ad.pkAlisDetay,sk.AlisFiyati as AlisFiyati,sk.AlisFiyatiKdvHaric  from AlisDetay ad with(nolock)
            left join StokKarti sk with(nolock) on sk.pkStokKarti=ad.fkStokKarti
            where ((ad.AlisFiyati<>sk.AlisFiyati) or (ad.AlisFiyatiKdvHaric<>sk.AlisFiyatiKdvHaric)) and ad.fkAlislar=" + pkAlislar.Text + " and ad.fkStokKarti=" + DB.pkStokKarti);

            if (dtAlisFiyatFarki.Rows.Count > 0)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Alış Fiyatlarında Değişiklik var!\n Alış Fiyatları, Stok Kartı ile Aynı Yapılsın mı?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;

                for (int j = 0; j < dtAlisFiyatFarki.Rows.Count; j++)
                {
                    DataRow dr2 = gridView1.GetDataRow(j);
                    DB.ExecuteSQL("update AlisDetay set AlisFiyati=" + dtAlisFiyatFarki.Rows[j]["AlisFiyati"].ToString().Replace(",", ".") +
                        ",AlisFiyatiKdvHaric=" + dtAlisFiyatFarki.Rows[j]["AlisFiyatiKdvHaric"].ToString().Replace(",", ".") +
                        " where pkAlisDetay=" + dtAlisFiyatFarki.Rows[j]["pkAlisDetay"].ToString());
                   //",=" + dtAlisFiyatFarki.Rows[j]["AlisFiyati"].ToString().Replace(",", ".") +
                }

                DB.ExecuteSQL("update AlisDetay set AlisFiyatiKdvHaric= AlisFiyati-((AlisFiyati*KdvOrani)/(100+KdvOrani)) where fkAlislar=" + pkAlislar.Text);
            }

            //satış fiyatlarını güncelle
            DataTable dtSatisFiyatFarki = DB.GetData(@"select ad.pkAlisDetay,sf.SatisFiyatiKdvli as SatisFiyati,isnull(sf2.SatisFiyatiKdvli,sf.SatisFiyatiKdvli) as SatisFiyatiKdvli from AlisDetay ad with(nolock)
            left join StokKarti sk with(nolock) on sk.pkStokKarti=ad.fkStokKarti
            LEFT JOIN SatisFiyatlari sf with(nolock) on sf.fkStokKarti=ad.fkStokKarti and ad.SatisFiyati<>sf.SatisFiyatiKdvli
			LEFT JOIN SatisFiyatlari sf2 with(nolock) on sf2.fkStokKarti=ad.fkStokKarti and ad.SatisFiyati2<>sf2.SatisFiyatiKdvli
            INNER JOIN SatisFiyatlariBaslik SFB WITH(NOLOCK) ON SFB.pkSatisFiyatlariBaslik=sf.fkSatisFiyatlariBaslik AND SFB.Tur=1
			LEFT JOIN SatisFiyatlariBaslik SFB2 WITH(NOLOCK) ON SFB2.pkSatisFiyatlariBaslik=sf2.fkSatisFiyatlariBaslik AND SFB2.Tur=2
            where ad.fkAlislar=" + pkAlislar.Text + 
            " and ad.fkStokKarti="+ DB.pkStokKarti);
//select ad.pkAlisDetay,sk.SatisFiyati,sf.SatisFiyatiKdvli,sk.SatisFiyatiKdvHaric from AlisDetay ad with(nolock)
//            left join StokKarti sk with(nolock) on sk.pkStokKarti=ad.fkStokKarti
//            LEFT JOIN SatisFiyatlari sf with(nolock) on sf.fkStokKarti=ad.fkStokKarti
//            INNER JOIN SatisFiyatlariBaslik SFB WITH(NOLOCK) ON SFB.pkSatisFiyatlariBaslik=SF.fkSatisFiyatlariBaslik AND SFB.Tur=1
//            where ((ad.SatisFiyati<>sk.SatisFiyati) OR(ad.SatisFiyati2<>sf.SatisFiyatiKdvli)) and ad.fkAlislar=" + pkAlislar.Text + " and ad.fkStokKarti=" + DB.pkStokKarti);

            if (dtSatisFiyatFarki.Rows.Count > 0)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Fiyatlarında Değişiklik var!\n Satış Fiyatları, Stok Kartı ile Aynı Yapılsın mı?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;

                for (int j = 0; j < dtSatisFiyatFarki.Rows.Count; j++)
                {
                    DataRow dr2 = gridView1.GetDataRow(j);
                    DB.ExecuteSQL("update AlisDetay set SatisFiyati=" + dtSatisFiyatFarki.Rows[j]["SatisFiyati"].ToString().Replace(",", ".") +
                   ",SatisFiyati2=" + dtSatisFiyatFarki.Rows[j]["SatisFiyatiKdvli"].ToString().Replace(",", ".") +
                        " where pkAlisDetay=" + dtSatisFiyatFarki.Rows[j]["pkAlisDetay"].ToString());
                }
                DB.ExecuteSQL("update AlisDetay set NakitFiyat=SatisFiyati where fkAlislar="+pkAlislar.Text);
            }

            //kdv oranlarını güncelle
            DataTable dtKdvFarki = DB.GetData(@"select ad.pkAlisDetay,sk.AlisFiyati as AlisFiyati,sk.KdvOraniAlis  from AlisDetay ad with(nolock)
            left join StokKarti sk with(nolock) on sk.pkStokKarti=ad.fkStokKarti
            where ad.KdvOrani<>sk.KdvOraniAlis and ad.fkAlislar=" + pkAlislar.Text + " and ad.fkStokKarti=" + DB.pkStokKarti);

            if (dtKdvFarki.Rows.Count > 0)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kdv Oranlarıda Değişiklik var!\n Kdv Oranları Aynı Yapılsın mı?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;

                for (int j = 0; j < dtKdvFarki.Rows.Count; j++)
                {
                    DataRow dr2 = gridView1.GetDataRow(j);
                    DB.ExecuteSQL("update AlisDetay set KdvOrani=" + dtKdvFarki.Rows[j]["KdvOraniAlis"].ToString().Replace(",", ".") +
                        " where pkAlisDetay=" + dtKdvFarki.Rows[j]["pkAlisDetay"].ToString());
                }
            }
            
            //iskontoTutarGuncelle();

            yesilisikyeni();
            
            gridView1.FocusedRowHandle = i;
        }

        private void repositoryItemButtonEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                simpleButton4_Click(sender, e);
                //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                //xtraTabPage5_Click( sender, e);
            }
           //adet * mı
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
            AppearanceDefault appBlue = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorRed = new AppearanceDefault(Color.Red);
            AppearanceDefault appErrorGreen = new AppearanceDefault(Color.GreenYellow);
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
            if (e.Column.FieldName == "AlisFiyati" && dr["Alisiskontolu"].ToString() != "" && dr["Adet"].ToString() != "" && dr["SatisFiyati"].ToString() != "")
            {
                decimal Alisiskontolu = Convert.ToDecimal(dr["Alisiskontolu"].ToString());
                decimal AlisFiyati_sk = Convert.ToDecimal(dr["AlisFiyati_sk"].ToString());
                string adet = dr["Adet"].ToString();
                decimal dadet = 0;
                decimal.TryParse(adet, out dadet);
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * dadet;
                decimal AlisTutar = Convert.ToDecimal(dr["Alisiskontolu"].ToString()) * dadet;

                string iade = dr["iade"].ToString();

                if (Alisiskontolu != AlisFiyati_sk)
                    AppearanceHelper.Apply(e.Appearance, appBlue);

                if (SatisTutar - AlisTutar <= 0 && (iade == "False" || iade == ""))
                    AppearanceHelper.Apply(e.Appearance, appError);

                //if (SatisTutar - AlisTutar <= 0 && (iade == "False" || iade == "") && (AlisFiyati != AlisFiyati_sk))
                //    AppearanceHelper.Apply(e.Appearance, appErrorGreen);
            }
            
            if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["SatisFiyati"].ToString() != "")
            {
                decimal SatisFiyati = Convert.ToDecimal(dr["SatisFiyati"].ToString());
                decimal SatisFiyati_sk = Convert.ToDecimal(dr["SatisFiyati_sk"].ToString());
                string adet = dr["Adet"].ToString();
                decimal dadet = 0;
                decimal.TryParse(adet, out dadet);
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * dadet;
                decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * dadet;

                string iade = dr["iade"].ToString();

                if (SatisFiyati != SatisFiyati_sk)
                    AppearanceHelper.Apply(e.Appearance, appBlue);
            }
            if (e.Column.FieldName == "iskontoyuzdetutar" && dr["iskontoyuzdetutar"].ToString() != "0,000000")
            {
                AppearanceHelper.Apply(e.Appearance, appBlue);
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
            if (gridColumn19.SummaryItem.SummaryValue == null)//5 di 19 yaptık kdv dahil herzaman
                aratop=0;//atis1Toplam.Text = "0,0";
            else
                aratop =  decimal.Parse(gridColumn19.SummaryItem.SummaryValue.ToString());

            decimal AraToplamiskontosuz = 0;
            if (gridColumn5.SummaryItem.SummaryValue == null)
                AraToplamiskontosuz = 0;
            else
                AraToplamiskontosuz = decimal.Parse(gridColumn5.SummaryItem.SummaryValue.ToString());

            //decimal isyuzde = 0,isfiyat=0,istutar=0;

            //if (ceiskontoTutar.EditValue != null) 
            //{
            //    isfiyat = decimal.Parse(ceiskontoTutar.EditValue.ToString());
            //    if (aratop>0)
            //       isyuzde = (isfiyat * 100) / aratop;
            //    istutar = isfiyat;
            //}

            aratoplam.Value = aratop;
            ceAraToplamiskontosuz.Value = AraToplamiskontosuz;

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

            //iskonto toplam
            if (gridColumn13.SummaryItem.SummaryValue == null)
                ceiskontoTutar.EditValue = "0";
            else
                ceiskontoTutar.EditValue = gridColumn13.SummaryItem.SummaryValue.ToString();   
         
            //ToplamTutarKdvli
            if (gridColumn8.SummaryItem.SummaryValue == null)
                ceKdvToplamTutar.EditValue = "0";
            else
            {
                ceKdvToplamTutar.EditValue = gridColumn8.SummaryItem.SummaryValue.ToString();
                //ceKdvToplamTutar.EditValue = ceKdvToplamTutar.Value.ToString("##0.00");
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
            //gridView1.FocusedColumn = gridView1.Columns["UrunKodu"];
            //gridView1.ShowEditor();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmHizliButtonDuzenle HizliButtonDuzenle = new frmHizliButtonDuzenle();
            HizliButtonDuzenle.Top = HizliTop;
            HizliButtonDuzenle.Left = HizliLeft;
            HizliButtonDuzenle.oncekibarkod.Text = HizliBarkod;
            HizliButtonDuzenle.oncekibarkod.Tag = pkHizliStokSatis;
            HizliButtonDuzenle.ShowDialog();
            for (int i = 0; i < TabHizliSatisGenel.Controls[0].Controls.Count; i++)
            {
                if (TabHizliSatisGenel.Controls[0].Controls[i].Name == HizliBarkodName)
                    TabHizliSatisGenel.Controls[0].Controls[i].Text = HizliButtonDuzenle.stokadi.Text;
            }
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

        private void sb_MouseEnter(object sender, EventArgs e)
        {
            HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            HizliTop = ((SimpleButton)sender).Top;
            HizliLeft = ((SimpleButton)sender).Left;
            HizliBarkodName = ((SimpleButton)sender).Name;
            pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;
        }

        void AlisGetir()
        {
            lbKalanBakiyesi.Text = "0,00";
            string pkSatislar="0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            if (AcikSatisindex == 2)
                pkSatislar = Satis2Toplam.Tag.ToString();
            if (AcikSatisindex == 3)
                pkSatislar = Satis3Toplam.Tag.ToString();
            if (AcikSatisindex == 4)
                pkSatislar = Satis4Toplam.Tag.ToString();

            cbFaturaTipi.SelectedIndex = Degerler.iFaturaTipiAlis;

            //FaturaNo.Text = "";

            //deFaturaTarihi.DateTime = DateTime.Today;

            if (pkSatislar == "0")
            {
                SatisTemizle();
                return;
            }
            DataTable dtAlislar = DB.GetData(@"SELECT  Alislar.pkAlislar, Alislar.Tarih, Alislar.fkFirma, Alislar.GelisNo, Alislar.Siparis, Alislar.KontokKisi, Alislar.SevkTarihi, Alislar.Aciklama, 
                    Alislar.AlinanPara, Alislar.ToplamTutar, Alislar.Yazdir, Alislar.iskontoFaturaTutar, Alislar.GonderildiWS, Tedarikciler.Firmaadi, Tedarikciler.Adres, 
                    Tedarikciler.Eposta, Tedarikciler.webadresi, Tedarikciler.Tel2, Tedarikciler.Tel,  Tedarikciler.Yetkili, FirmaGruplari.GrupAdi, Tedarikciler.VergiDairesi, Tedarikciler.VergiNo, 
                    Tedarikciler.Cep, Tedarikciler.Cep2, Tedarikciler.OzelKod, AlisDurumu.Durumu as SatisDurumu, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi as KullaniciAdiSoyadi,
                    Alislar.KdvDahil,Alislar.FaturaNo,Alislar.FaturaTarihi,Alislar.KdvDahil,Alislar.OdemeSekli,Alislar.GuncellemeTarihi,Alislar.fkSatisDurumu
                    FROM Alislar with(nolock)
                    INNER JOIN Tedarikciler with(nolock) ON Alislar.fkFirma = dbo.Tedarikciler.pkTedarikciler 
                    LEFT OUTER JOIN Kullanicilar with(nolock) ON Alislar.fkKullanici = Kullanicilar.pkKullanicilar 
                    LEFT OUTER JOIN AlisDurumu with(nolock) ON Alislar.fkSatisDurumu = AlisDurumu.pkAlisDurumu 
                    LEFT OUTER JOIN FirmaGruplari with(nolock) ON Tedarikciler.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
                    where Alislar.Siparis=0 and Alislar.pkAlislar=" + pkSatislar);
            if (dtAlislar.Rows.Count == 0)
            {
                formislemleri.Mesajform("Fiş Bulunamadı", "K", 150);
                //Showmessage("Fiş Bulunamadı", "K");
                pkAlislar.Text = "0";
                yesilisikyeni();
                return;
            }
          
            string fkfirma = dtAlislar.Rows[0]["fkFirma"].ToString();
            string firmaadi = dtAlislar.Rows[0]["Firmaadi"].ToString();
            string KdvDahil = dtAlislar.Rows[0]["KdvDahil"].ToString();
            int _fkSatisDurumu = 2;
            int.TryParse(dtAlislar.Rows[0]["fkSatisDurumu"].ToString(),out _fkSatisDurumu);
            
            FaturaNo.Text = dtAlislar.Rows[0]["FaturaNo"].ToString();
            deFaturaTarihi.Text = dtAlislar.Rows[0]["FaturaTarihi"].ToString();

            if (dtAlislar.Rows[0]["GuncellemeTarihi"].ToString()!="")
                deAlisTarihi.DateTime = Convert.ToDateTime(dtAlislar.Rows[0]["GuncellemeTarihi"].ToString());

            //deAlisTarihi.Text = dtAlislar.Rows[0]["GuncellemeTarihi"].ToString();
            string OdemeSekli = "Nakit";
            OdemeSekli = dtAlislar.Rows[0]["OdemeSekli"].ToString();
          
            if (KdvDahil == "True")
                cbFaturaTipi.SelectedIndex = 0;
            else
                cbFaturaTipi.SelectedIndex = 1;

            if (AcikSatisindex == 1)
            {  
                Satis1Firma.Tag = fkfirma;
                Satis1Firma.Text = fkfirma + "-" + firmaadi;
                Satis1Baslik.Tag = fkfirma;
                Satis1Baslik.Text = fkfirma + "-" + firmaadi;
                Satis1Baslik.ToolTip = Satis1Baslik.Text;
            }
            else if (AcikSatisindex == 2)
            {
                Satis2Firma.Tag = fkfirma;
                Satis2Firma.Text = fkfirma + "-" + firmaadi;
                Satis2Baslik.Tag = fkfirma;
                Satis2Baslik.Text = fkfirma + "-" + firmaadi;
                Satis2Baslik.ToolTip = Satis2Baslik.Text;
            }
            else if (AcikSatisindex == 3)
            {
                Satis3Firma.Tag = fkfirma;
                Satis3Firma.Text = fkfirma + "-" + firmaadi;
                Satis3Baslik.Tag = fkfirma;
                Satis3Baslik.Text = fkfirma + "-" + firmaadi;
                Satis3Baslik.ToolTip = Satis3Baslik.Text;
            }
            else if (AcikSatisindex == 4)
            {
                Satis4Firma.Tag = fkfirma;
                Satis4Firma.Text = fkfirma + "-" + firmaadi;
                Satis4Baslik.Tag = fkfirma;
                Satis4Baslik.Text = fkfirma + "-" + firmaadi;
                Satis4Baslik.ToolTip = Satis4Baslik.Text;
            }

            //if (dtSatislar.Rows[0]["iskontoFaturaTutar"].ToString()=="")
            //    ceiskontoTutar.Text = "0,00";
            //else
            //    ceiskontoTutar.Text = dtSatislar.Rows[0]["iskontoFaturaTutar"].ToString();

            lueAlisTipi.EditValue = _fkSatisDurumu;

            BakiyeGetir(fkfirma);
        }

        void BakiyeGetir(string fkTedarikciler)
        {
            lbKalanBakiyesi.Visible = true;
            DataTable dt = DB.GetData("select isnull(dbo.fon_TedarikciBakiyesi(" + fkTedarikciler + "),0)");
            if (dt.Rows.Count == 0)
            {
                lbKalanBakiyesi.Text = "0,00";
            }
            else
            {
                lbKalanBakiyesi.Text = decimal.Parse(dt.Rows[0][0].ToString()).ToString("N2");//C,C1,C2..C6
            }
            //bakiye.Value = (ceDevir.Value + satistoplam.Value) - odemetoplam.Value - kalantaksittoplam.Value;
        }

      
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

        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //iskonto tutar
            if (e.KeyCode == Keys.Enter)
            {
                string iskontotutar =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();

                 if (gridView1.FocusedRowHandle < 0)
                 {
                    yesilisikyeni();
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                try
                {
                    string pkAlisDetay = dr["pkAlisDetay"].ToString();
        
                    string alisfiyati = "0";

                    if (dr["KdvDahil"].ToString()=="True")
                        alisfiyati=dr["AlisFiyati"].ToString();
                    else
                        alisfiyati = dr["AlisFiyatiKdvHaric"].ToString();

                    decimal isyuzde = (decimal.Parse(iskontotutar) * 100) / decimal.Parse(alisfiyati);
                    //isyuzde=decimal.Round(isyuzde, 6);
                    int sonuc = DB.ExecuteSQL_Sonuc_Sifir("update AlisDetay set iskontotutar=" + iskontotutar.ToString().Replace(",",".") + ",iskontoyuzdetutar=" + isyuzde.ToString().Replace(",", ".") +
                        " where pkAlisDetay=" + pkAlisDetay);
                    if (sonuc != 0)
                    {
                        formislemleri.Mesajform("Hata Oluştu Lütfen iskonto değerlerini Kontrol Ediniz","K",200);
                    }
                }
                catch (Exception exp)
                {
                    formislemleri.Mesajform("Hata Oluştu:" + exp.Message, "K", 200);
                    //throw;
                }
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
            else if (AcikSatisindex == 4)
                Satis4Baslik.Font = Satis4Baslik.Font;
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
            //iskontopanel.BackColor = secilen.BackColor;
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Program Kurulduğundaki ilk Ayarlara Dönülecek \n Onaylıyormusunuz?", "Hitit2012", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
          
        }

        private void cariSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _TedarikciAra();
        }

        private void yenidenAdlandırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HizliButtonAdiDegis ButtonAdiDegis = new HizliButtonAdiDegis();
            ButtonAdiDegis.Top = HizliTop;
            ButtonAdiDegis.Left = HizliLeft;
            ButtonAdiDegis.oncekibarkod.Text = HizliBarkod;
            ButtonAdiDegis.oncekibarkod.Tag = pkHizliStokSatis;
            ButtonAdiDegis.ShowDialog();

            for (int i = 0; i < TabHizliSatisGenel.Controls[0].Controls.Count; i++)
            {
                if (TabHizliSatisGenel.Controls[0].Controls[i].Name == HizliBarkodName)
                    TabHizliSatisGenel.Controls[0].Controls[i].Text = ButtonAdiDegis.stokadi.Text;
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            string pkAlislar = "0";

            if (AcikSatisindex == 1) pkAlislar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2) pkAlislar = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3) pkAlislar = Satis3Toplam.Tag.ToString();
            else if (AcikSatisindex == 4) pkAlislar = Satis4Toplam.Tag.ToString();
            if (pkAlislar == "0")
            {
                formislemleri.Mesajform("Önce Satış Yapınız!", "K", 150);
                return;
            }

            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.Tag = "Alis";
            fFisAciklama.memoozelnot.Tag = pkAlislar;
                //.Text = btnAciklamaGirisi.ToolTip;
            fFisAciklama.ShowDialog();
            //btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;

            fFisAciklama.Dispose();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            //string str = ActiveControl.Name;
            //this.Dispose();
            this.Visible = false;//tetikleme anaformda
            //this.Hide();
        }

        private void simpleButton20_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void satistipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lueAlisTipi.EditValue.ToString())
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
            //FisListesi();
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

        void YazdirSatisFaturasi(bool Disigner)
        {
            string pkFirma = "1";       
            if (AcikSatisindex == 1)
                pkFirma = Satis1Firma.Tag.ToString();
            if (AcikSatisindex == 2)
                pkFirma = Satis2Firma.Tag.ToString();
            if (AcikSatisindex == 3)
                pkFirma = Satis3Firma.Tag.ToString();
            if (AcikSatisindex == 4)
                pkFirma = Satis4Firma.Tag.ToString();

            try
            {
                string fisid = pkAlislar.Text;
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(@"exec sp_AlisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Alislar " + fisid);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                ////kdv oranları
                //DataTable fiskdv = DB.GetData("select KdvOrani,SUM((SatisFiyati-iskontotutar)*Adet) as kdvtutar  from SatisDetay" +
                //" where fkSatislar= " + fisid + " group by KdvOrani");
                //fiskdv.TableName = "fiskdv";
                //ds.Tables.Add(fiskdv);
                //şirket bilgileri
                DataTable sirket = DB.GetData("select * from Sirketler with(nolock)");
                sirket.TableName = "sirket";
                ds.Tables.Add(sirket);
                //Firma bilgileri
                DataTable Musteri = DB.GetData("select * from Tedarikciler with(nolock) where pkTedarikciler=" +pkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\AlisiadeFaturasi.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Alis iade Faturasi.repx Dosya Bulunamadı");
                    return;
                }
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "AlisiadeFaturasi";
                rapor.Report.Name = "AlisiadeFaturasi";
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            //Fatura veya iade faturası ise
            if (lueAlisTipi.EditValue.ToString() == "4" || lueAlisTipi.EditValue.ToString() == "10")
            {
                DB.ExecuteSQL("update Alislar set ToplamTutar=" + aratoplam.Value.ToString().Replace(",",".") +" where pkAlislar=" + pkAlislar.Text);

                YazdirSatisFaturasi(false);//henüz tam olarak nasıl yapılacak ?TODO:
            }
            else
                yazdir();           
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE HizliStokSatis Set fkStokKarti=0 where pkHizliStokSatis=" + pkHizliStokSatis);
            for (int i = 0; i < TabHizliSatisGenel.Controls[0].Controls.Count; i++)
            {
                if (TabHizliSatisGenel.Controls[0].Controls[i].Name == HizliBarkodName)
                    TabHizliSatisGenel.Controls[0].Controls[i].Text = "BOŞ";
            }
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

            AlisGetir();

            yesilisikyeni();
        }

        private void Satis4Toplam_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 4;

            AlisGetir();

            yesilisikyeni();
        }

        void SatisDuzenKapat()
        {
            Satis4Firma.Visible = false;
            pkAlislar.EditValue = null;
            //deDuzenlemeTarihi.EditValue = null;
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
            string pkSatisDetay = dr["pkAlisDetay"].ToString();
            DB.ExecuteSQL("UPDATE AlisDetay SET Adet=" + yenimiktar.Replace(",", ".") + " where pkAlisDetay=" + pkSatisDetay);
            //decimal iskontoyuzde = 0;
            //if (dr["iskontoyuzdetutar"].ToString() != "")
            //    iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
            //decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());
            //decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
           // decimal Miktar = Convert.ToDecimal(yenimiktar);
            //decimal iskontogercektutar = Convert.ToDecimal(dr["iskontotutar"].ToString());

            //if (iskontogercektutar > 0)
            //{
            //    iskontogercekyuzde = (iskontogercektutar * 100) / (Fiyat * Miktar);
            //}
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle,gridColumn3, yenimiktar);
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle,gridColumn33, iskontogercekyuzde.ToString());
            
        }

        private void simpleButton8_Click_1(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("27");//11
            SayfaAyarlari.ShowDialog();
        }

        void dockPanel1Gizle()
        {
            if (dockPanel1.Width > 45)
            {
                dockPanel1.Width = 40;
                dockPanel1.SendToBack();
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
            if(TabHizliSatisGenel.Tag == "0")
               Hizlibuttonlariyukle();
            dockPanel1goster();
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
                
                if (pkAlislar.Text == "") return;
                if (DB.GetData("select pkAlislar From Alislar with(nolock) where pkAlislar=" + pkAlislar.Text).Rows.Count == 0)
                {
                    formislemleri.Mesajform("Fiş Bulunamadı.", "K", 150);
                    return;
                }
                FisGetir(pkAlislar.Text);
               // if (lueFis.EditValue == null)
                 //   lueFis.EditValue = pkAlislar.Text;
            }
        }

        private void gCSatisDuzen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DB.pkTedarikciler = int.Parse(Satis4Firma.Tag.ToString());
            frmMusteriKarti kart = new frmMusteriKarti(Satis4Firma.Tag.ToString(), "");
            kart.ShowDialog();
        }

        private void gridControl1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
        }
      
        void AlisDetayGetir(string pkSatislar)
        {
            pkAlislar.Text = pkSatislar;

            gridControl1.DataSource = DB.GetData("exec sp_AlisDetay " + pkSatislar+",0");

            toplamlar();

            gridView1.AddNewRow();
        }

        void AlisDetayEkle(string barkod)
        {
            if (barkod == "") return;

            //float Adet = 1;
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
            if (dr != null && dr["pkAlisDetay"].ToString() != "") return; 
            if (barkod == "" || f == 0) return;

            YeniAlisEkle();

            if (barkod.Length == 3)
                barkod = (1 * float.Parse(barkod)).ToString();

            #region gramlı ürünler
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
            #endregion

            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti,AlisFiyati,AlisFiyatiKdvHaric,isnull(SatisAdedi,0) as SatisAdedi FROM StokKarti with(nolock) where Barcode='" + barkod + "'");
            //STOK KARTINDA YOKSA
            if (dtStokKarti.Rows.Count == 0)
            {
                string Barcode="";
                dtStokKarti = DB.GetData("SELECT isnull(skb.SatisAdedi,1) as SatisAdedi,sk.Barcode FROM StokKartiBarkodlar  skb with(nolock)" +
                " inner join StokKarti sk with(nolock) on sk.pkStokKarti=skb.fkStokKarti  where skb.Barkod='" + barkod + "'");
                if (dtStokKarti.Rows.Count > 0)
                {
                    EklenenMiktar = decimal.Parse(dtStokKarti.Rows[0]["SatisAdedi"].ToString());

                    //if (EklenenMiktar == 1)
                        EklenenMiktar = HizliMiktar * EklenenMiktar;

                    Barcode = dtStokKarti.Rows[0]["Barcode"].ToString();
                }
                else
                {
                    frmStokKarti StokKartiHizli = new frmStokKarti();
                    Degerler.stokkartisescal = true;
                    DB.pkStokKarti = 0;
                    StokKartiHizli.lblBarkod.AccessibleDescription = "alisdangeldievet";
                    StokKartiHizli.Barkod.EditValue = barkod;
                    StokKartiHizli.lblBarkod.Tag = barkod;
                    StokKartiHizli.ShowDialog();
                    Barcode=StokKartiHizli.Barkod.EditValue.ToString();
                    StokKartiHizli.Dispose();
                }
                //if (StokKartiHizli.TopMost == true)
               // {
                dtStokKarti = DB.GetData("select pkStokKarti,AlisFiyati,AlisFiyatiKdvHaric,isnull(SatisAdedi,1) as SatisAdedi From StokKarti with(nolock) WHERE Barcode='" + Barcode + "'");
               // }
                //else
                //eğer stok kartı oluşturmadı ise 
                if (dtStokKarti.Rows.Count == 0)
                {
                    yesilisikyeni();
                    return;
                }
            }
            string pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            decimal satis_adedi = decimal.Parse(dtStokKarti.Rows[0]["SatisAdedi"].ToString());

            EklenenMiktar = satis_adedi * EklenenMiktar;

            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkAlislar", pkAlislar.Text));
            arr.Add(new SqlParameter("@SatisFiyatGrubu", "1"));
            arr.Add(new SqlParameter("@AlisFiyati", dtStokKarti.Rows[0]["AlisFiyati"].ToString().Replace(",", ".")));
            arr.Add(new SqlParameter("@Adet", EklenenMiktar.ToString().Replace(",", ".")));
            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
            arr.Add(new SqlParameter("@fkDepo", lueDepolar.EditValue.ToString()));
            //arr.Add(new SqlParameter("@iskontoyuzde", dtStokKarti.Rows[0]["alis_iskonto"].ToString().Replace(",", ".")));
            //arr.Add(new SqlParameter("@iskontotutar", "0"));
            string s = DB.ExecuteScalarSQL("exec sp_AlisDetay_Ekle @fkAlislar,@AlisFiyati,@SatisFiyatGrubu,@Adet,@fkStokKarti,@fkDepo", arr);

            string kdvdahilmi="0";
            if (cbFaturaTipi.SelectedIndex == 0)
                kdvdahilmi = "1";
            else
                kdvdahilmi = "0";

            DB.ExecuteSQL("UPDATE AlisDetay SET KdvDahil=" + kdvdahilmi + " WHERE fkAlislar=" + pkAlislar.Text);

            if (s != "Alis Detay Eklendi.")
            {
                MessageBox.Show(s);
                return;
            }

            if (cbOncekiFiyatHatirla.Checked)
            {
                string fkfirma = "1";
                if (AcikSatisindex == 1)
                {
                    fkfirma = Satis1Firma.Tag.ToString();
                }
                else if (AcikSatisindex == 2)
                {
                    fkfirma=Satis2Firma.Tag.ToString();
                }
                else if (AcikSatisindex == 3)
                {
                    fkfirma = Satis3Firma.Tag.ToString();
                                   }
                else if (AcikSatisindex == 4)
                {
                    fkfirma = Satis4Firma.Tag.ToString();
                    
                }
                DataTable dtad =
                DB.GetData("select pkAlisDetay from AlisDetay with(nolock) where fkAlislar=" + pkAlislar.Text +
                " and fkStokKarti=" + pkStokKarti);
                string pkAlisDetay = dtad.Rows[0][0].ToString();

                DataTable dt =
                DB.GetData("select dbo.fon_teda_son_alis_fiyati("+fkfirma+","+ pkStokKarti +","+pkAlislar.Text+ ")");
                string isk = dt.Rows[0][0].ToString();

                DB.ExecuteSQL("update alisdetay set  iskontoyuzdetutar=" + isk.Replace(",",".") + " where pkAlisdetay="+pkAlisDetay );
                DB.ExecuteSQL("update alisdetay set  iskontotutar=(AlisFiyati*iskontoyuzdetutar)/100  where pkAlisdetay=" + pkAlisDetay);
            }

            HizliMiktar = 1;

            if (Degerler.Uruneklendisescal)
                Digerislemler.UrunEkleSesCal();
        }

        void YeniAlis()
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
            if (AcikSatisindex == 4)
                pkFirma = Satis4Firma.Tag.ToString();

            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Siparis", "0"));
            list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("@fkSatisDurumu", lueAlisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            list.Add(new SqlParameter("@AlinanPara","0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));//iskontoTutar.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@OdemeSekli", "Nakit"));
            if (deFaturaTarihi.EditValue==null || deFaturaTarihi.Text == "")
                list.Add(new SqlParameter("@FaturaTarihi", DBNull.Value));
            else
                list.Add(new SqlParameter("@FaturaTarihi", deFaturaTarihi.DateTime));

            //if (deAlisTarihi.EditValue == null || deAlisTarihi.Text == "")
            //    list.Add(new SqlParameter("@GuncellemeTarihi", DateTime.Now));
            //else
            //    list.Add(new SqlParameter("@GuncellemeTarihi", deAlisTarihi.DateTime));
            

            string kdvdahilmi = "0";
            if (cbFaturaTipi.SelectedIndex == 0)//kdv dahil
                kdvdahilmi = "1";
            list.Add(new SqlParameter("@KdvDahil", kdvdahilmi));
            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

            sql = "INSERT INTO Alislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,"+
                "ToplamTutar,Yazdir,iskontoFaturaTutar,OdemeSekli,KdvDahil,FaturaTarihi,BilgisayarAdi,fkSube)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,"+
                "@ToplamTutar,@Yazdir,@iskontoFaturaTutar,@OdemeSekli,@KdvDahil,@FaturaTarihi,@BilgisayarAdi,@fkSube) SELECT IDENT_CURRENT('Alislar')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                AlisDetayGetir(Satis1Toplam.Tag.ToString());
            }
            if (AcikSatisindex == 2 && Satis2Toplam.Tag.ToString() == "0")
            {
                Satis2Toplam.Tag = fisno;
                AlisDetayGetir(Satis2Toplam.Tag.ToString());
            }
            if (AcikSatisindex == 3 && Satis3Toplam.Tag.ToString() == "0")
            {
                Satis3Toplam.Tag = fisno;
                AlisDetayGetir(Satis3Toplam.Tag.ToString());
            }
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(22,0);
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

            int secilen = gridView1.FocusedRowHandle;
            DevExpress.XtraGrid.Columns.GridColumn secilenGridColumn = gridView1.FocusedColumn;
            DataRow dr = gridView1.GetDataRow(secilen);

            repositoryItemButtonEdit2.Tag = "0";

            frmiskonto iskonto = new frmiskonto();
            //iskonto çıkışda kaydetmesini engellemek için
            //gridView1.Focus();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];

            iskonto.fkAlisDetay.Text = dr["pkAlisDetay"].ToString();
            if(cbFaturaTipi.SelectedIndex==0)
                iskonto.ceBirimFiyati.Value = decimal.Parse(dr["AlisFiyati"].ToString());
            else
                iskonto.ceBirimFiyati.Value = decimal.Parse(dr["AlisFiyatiKdvHaric"].ToString());
            iskonto.ShowDialog();

            //repositoryItemButtonEdit2.Tag = "0";
            //((DevExpress.XtraEditors.ButtonEdit)((((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue =
            //   iskonto.iskontoorani.Value;

            yesilisikyeni();
            gridView1.FocusedRowHandle = secilen;

            //gridView1.Focus();
            gridView1.FocusedColumn = secilenGridColumn;
            gridView1.CloseEditor();
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr == null)
                    gridView1.DeleteRow(i);
                else if (dr["pkAlisDetay"].ToString() == "")
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

        private void repositoryItemCalcEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
                    ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
                //kdv dahil
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyati=" + girilen.Replace(",", ".") + " where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
                DB.ExecuteSQL(@"UPDATE AlisDetay Set AlisFiyatiKdvHaric=AlisFiyati-(AlisFiyati*KdvOrani)/(100+KdvOrani)
where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
                //AlisFiyatiKdvHaricGuncelle(girilen);
                yesilisikyeni();
            }
        }

        private void pkSatisBarkod_Enter(object sender, EventArgs e)
        {
            pkAlislar.Text = "";
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
                string pkAlisDetay = dr["pkAlisDetay"].ToString();

                string sql = "UPDATE AlisDetay SET iskontoyuzdetutar=" + iskontoyuzdetutargirilen.ToString().Replace(",", ".") +" where pkAlisDetay=" + pkAlisDetay;

                int sonuc = DB.ExecuteSQL(sql);

                //iskontoTutarGuncelle();
                if (iskontoyuzdetutargirilen=="0")
                    sql = "UPDATE AlisDetay SET iskontotutar=0 where pkAlisDetay=" + pkAlisDetay;
                else
                sql = "UPDATE AlisDetay SET iskontotutar=(AlisFiyati*" + iskontoyuzdetutargirilen.ToString().Replace(",", ".") + ")/100  where pkAlisDetay=" + pkAlisDetay;

                sonuc = DB.ExecuteSQL(sql);

                yesilisikyeni();
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            //iskonto
            if (e.KeyValue == 222)
            {
                gridView1.FocusedColumn = gridColumn33;
                //gridView1.Columns["iskontoyuzdetutar"];
                gridView1.ShowEditor();
                //    iskontoyagit_ctrli();
            }
            //iade
            if (e.Control && e.KeyValue == 222)
            {
                gridView1.FocusedColumn = gciade;
                //gridView1.Columns["iskontoyuzdetutar"];
                gridView1.ShowEditor();
                //    iskontoyagit_ctrli();
            }
            //if (e.KeyCode == Keys.Enter && !gridView1.IsEditing && gridView1.FocusedColumn.FieldName == "iskyuzdesanal")
            //{
            //    if (gridView1.FocusedRowHandle < 0) return;
            //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //    frmiskonto iskonto = new frmiskonto();
            //    iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
            //    iskonto.ShowDialog();
            //}
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            GeciciTedarikciDefault();

            DataTable dt = DB.GetData("select pkAlislar,fkFirma from Alislar with(nolock) where Siparis<>1 and fkKullanici=" + DB.fkKullanicilar);

            int c =dt.Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string pkSatislar = dt.Rows[i]["pkAlislar"].ToString();
                    string fkFirma=dt.Rows[i]["fkFirma"].ToString();
                    AcikSatisindex = i+1;
                    if (i == 0)
                    {
                        //Showmessage("Ekranda tamamlanmamış " + c.ToString() + " Adet işleminiz bulumnaktadır.","K");
                        //DevExpress.XtraEditors.XtraMessageBox.Show("Ekranda tamamlanmamış " + c.ToString() + " Adet işleminiz bulumnaktadır.", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Satis1Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select * from Tedarikciler with(nolock) where pkTedarikciler=" + fkFirma);
                        if (dtMusteri.Rows.Count == 0)
                        {
                            DB.ExecuteSQL("INSERT INTO Tedarikciler (Firmaadi,OzelKod,Aktif,GeciciMusteri) VALUES('GEÇİCİ TEDARİKÇİ',1,1,1)");
                            dtMusteri = DB.GetData("select * from Tedarikciler where pkTedarikciler=" + fkFirma);
                        }
                        Satis1Firma.Tag = dtMusteri.Rows[0]["pkTedarikciler"].ToString();
                        Satis1Baslik.ToolTip = dtMusteri.Rows[0]["pkTedarikciler"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis1Baslik.Text = Satis1Baslik.ToolTip;
                    }
                    else if (i == 1)
                    {
                        Satis2Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select * from Tedarikciler with(nolock) where pkTedarikciler=" + fkFirma);
                        Satis2Firma.Tag = dtMusteri.Rows[0]["pkTedarikciler"].ToString();
                        Satis2Baslik.ToolTip = dtMusteri.Rows[0]["pkTedarikciler"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis2Baslik.Text = Satis2Baslik.ToolTip;
                        //Satis2Toplam.Tag = dt.Rows[i]["pkSatislar"].ToString();
                        AlisDetayGetir(Satis2Toplam.Tag.ToString());
                    }
                    else if (i == 2)
                    {
                        Satis3Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select * from Tedarikciler with(nolock) where pkTedarikciler=" + fkFirma);
                        Satis3Firma.Tag = dtMusteri.Rows[0]["pkTedarikciler"].ToString();
                        Satis3Baslik.ToolTip = dtMusteri.Rows[0]["pkTedarikciler"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis3Baslik.Text = Satis3Baslik.ToolTip;
                        //Satis3Toplam.Tag = dt.Rows[i]["pkSatislar"].ToString();
                        AlisDetayGetir(Satis3Toplam.Tag.ToString());
                    }
                    if (i > 2) break;
                    //yesilisikyeni();
                }
                AcikSatisindex = 1;
                AlisGetir();
                AlisDetayGetir(Satis1Toplam.Tag.ToString());
                yesilisikyeni();
                return;
            }
            AlisDetayGetir("0");
            yesilisikyeni();
        }

        void SatisFiyatlariAc(bool alismi)
        {
          if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti=dr["fkStokKarti"].ToString();
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            if (alismi==true)
                SatisFiyatlari.Tag = "1";
            SatisFiyatlari.pkStokKarti.Text=fkStokKarti;
            SatisFiyatlari.ShowDialog();
            if (alismi == true)
            {
                //YeniFiyatKdvli
                if (cbFaturaTipi.SelectedIndex==0)
                DB.ExecuteSQL(@"UPDATE AlisDetay SET SatisFiyati=SatisFiyatlari.YeniFiyatKdvli  from SatisFiyatlari
                where AlisDetay.fkStokKarti=SatisFiyatlari.fkStokKarti
                and fkSatisFiyatlariBaslik=1 and Aktif=1 and pkAlisDetay=" + dr["pkAlisDetay"].ToString());
                //YeniFiyatKdvsiz
                if (cbFaturaTipi.SelectedIndex == 1)
                    DB.ExecuteSQL(@"UPDATE AlisDetay SET SatisFiyati=SatisFiyatlari.YeniFiyatKdvsiz  from SatisFiyatlari
where AlisDetay.fkStokKarti=SatisFiyatlari.fkStokKarti
and fkSatisFiyatlariBaslik=1 and Aktif=1 and pkAlisDetay=" + dr["pkAlisDetay"].ToString());

                yesilisikyeni();
            }
        }

        private void satışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SatisFiyatlariAc(false);
        }

        private void Satis1Toplam_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 1;
            AlisGetir();
            yesilisikyeni();
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        private void Satis1Baslik_DoubleClick(object sender, EventArgs e)
        {
            _TedarikciAra();
        }

        private void Satis2Toplam_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 2;
            AlisGetir();
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

        void AlisDetayYenile()
        {
            if (AcikSatisindex == 1)
            {
                AlisDetayGetir(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
            }
            else if (AcikSatisindex == 2)
            {
                AlisDetayGetir(Satis2Toplam.Tag.ToString());
                TutarFont(Satis2Toplam);
            }
            else if (AcikSatisindex == 3)
            {
                AlisDetayGetir(Satis3Toplam.Tag.ToString());
                TutarFont(Satis3Toplam);
            }
            else if (AcikSatisindex == 4)
            {
                AlisDetayGetir(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
            }
        }
        private void repositoryItemCalcEdit3_Leave(object sender, EventArgs e)
        {
            //Miktar
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

            AlisDetayYenile();

            gridView1.FocusedRowHandle = i;
        }

        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            //iskontotutar
            if (((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;
            string iskontotutar =
               ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();

//            if (gridView1.FocusedRowHandle < 0) return;

//            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

//            string pkAlisDetay = dr["pkAlisDetay"].ToString();

//            DB.ExecuteSQL(@"update AlisDetay set iskontoyuzdetutar=
//            case 
//            when KdvDahil=1 then 
//            (iskontotutar*100)/AlisFiyati
//            else
//            (iskontotutar*100)/AlisFiyatiKdvHaric
//            end 
//            where pkAlisDetay="+ pkAlisDetay);
            
            //iskontoTutarGuncelle();
            
            int i = gridView1.FocusedRowHandle;

            AlisDetayYenile();
            
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
                DB.pkTedarikciler = int.Parse(Satis1Firma.Tag.ToString());
            else if (AcikSatisindex == 2)
                DB.pkTedarikciler = int.Parse(Satis2Firma.Tag.ToString());
            else if (AcikSatisindex == 3)
                DB.pkTedarikciler = int.Parse(Satis3Firma.Tag.ToString());
            else
                DB.pkTedarikciler = int.Parse(Satis4Firma.Tag.ToString());

            frmTedarikciKarti MusteriKarti = new frmTedarikciKarti(DB.pkTedarikciler.ToString());
           
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

        private void button3_Click(object sender, EventArgs e)
        {
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.pkStokKarti.Text = "0";
            SatisFiyatlari.ShowDialog();
        }

        private void bARKOTBASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int secilen = 0;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                if (dr["Stogaisle"].ToString() == "True")
                    secilen++;
            }
            DialogResult secim;
            if (secilen==0)
            {
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Kayıt Seçiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                yesilisikyeni();
                return;
            }
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Etiket Adedi Miktar Kadar mı olsun?", Degerler.mesajbaslik, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (secim == DialogResult.Cancel) return;
            string adetbir = "1", adet = "1";
            if (secim == DialogResult.Yes) adetbir = "0";

            string pkEtiketBas = "0"; 
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                if (dr["Stogaisle"].ToString() == "False") continue;
                
                if (i == 0)
                {
                    pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Fiş No " + pkAlislar.Text + "',0) SELECT IDENT_CURRENT('EtiketBas')");
                }

                if(adetbir=="0")
                  adet = dr["Adet"].ToString();

                if (dr["Stogaisle"].ToString() == "True")
                    DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + 
                    pkEtiketBas + "," + dr["fkStokKarti"].ToString() + ","+ adet.ToString()+ ",0,getdate())");
            }
            frmEtiketBas EtiketBas = new frmEtiketBas();
            EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
            EtiketBas.ShowDialog();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            string sec="1";

            if(checkEdit1.Checked) 
               sec="1";
            else
              sec="0";

           DB.ExecuteSQL("UPDATE AlisDetay SET Stogaisle="+ sec +" where fkAlislar=" + pkAlislar.Text);

           yesilisikyeni();
        }
        void AlisBilgileriGetir(int e)
        {
            if (e < 0)
            {
                gridControl4.DataSource = null;
                return;
            }
            DataRow dr = gridView1.GetDataRow(e);
            if (dr["fkStokKarti"].ToString() == "") return;
                DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
                string sql = @"SELECT TOP (10) Alislar.pkAlislar, Alislar.Tarih,AlisDetay.AlisFiyati,AlisDetay.AlisFiyatiKdvHaric, Tedarikciler.Firmaadi, AlisDetay.Adet
FROM  Alislar with(nolock) INNER JOIN AlisDetay with(nolock) ON Alislar.pkAlislar = AlisDetay.fkAlislar 
INNER JOIN  Tedarikciler with(nolock) ON Alislar.fkFirma = Tedarikciler.pkTedarikciler
WHERE  Alislar.Siparis=1 and AlisDetay.fkStokKarti = @fkStokKarti
GROUP BY Alislar.pkAlislar, Alislar.Tarih, AlisDetay.AlisFiyati,AlisDetay.AlisFiyatiKdvHaric, Tedarikciler.Firmaadi, AlisDetay.Adet
ORDER BY Alislar.pkAlislar DESC";
            sql = sql.Replace("@fkStokKarti", DB.pkStokKarti.ToString());
            gridControl4.DataSource = DB.GetData(sql);
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
        }

        private void repositoryItemCheckEdit2_CheckStateChanged(object sender, EventArgs e)
        {
            string sec = ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();
            if (sec == "True")
                sec = "1";
            else
                sec = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("UPDATE AlisDetay SET Stogaisle="+sec+" where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
        }

        private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            AlisBilgileriGetir(e.FocusedRowHandle);
        }

        private void repositoryItemButtonEdit3_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SatisFiyatlariAc(false);
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string sec = dr["Stogaisle"].ToString();
            if (sec == "True")
                sec = "0";
            else
                sec = "1";

            DB.ExecuteSQL("UPDATE AlisDetay SET Stogaisle=" + sec + " where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            if (sec == "1")
                gridView1.SetFocusedRowCellValue("Stogaisle", true);
            else
                gridView1.SetFocusedRowCellValue("Stogaisle", false);
        }

        bool faturatipinde = false;
        private void cbFaturaTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            int kdvdahilmi = 0;
            if (cbFaturaTipi.SelectedIndex == 0)//kdv dahil
            {
                kdvdahilmi = 1;
                gcAlisFiyatiKdvHaric.Visible = false;
                gcAlisFiyatiKdvHaric.VisibleIndex = -1;
                gridColumn32.Visible = false;
            }
            else
            {
                kdvdahilmi = 0;
                gcAlisFiyatiKdvHaric.Visible = true;
                gcAlisFiyatiKdvHaric.VisibleIndex = 4;
                gridColumn32.Visible = true;
            }

            if (faturatipinde==true)
            {
                DB.ExecuteSQL("UPDATE Alislar SET KdvDahil=" + kdvdahilmi + " WHERE pkAlislar=" + pkAlislar.Text);
                DB.ExecuteSQL("UPDATE AlisDetay SET KdvDahil=" + kdvdahilmi + " WHERE fkAlislar=" + pkAlislar.Text);
                faturatipinde = false;
               // iskontoTutarGuncelle();
                yesilisikyeni();
            }
        }

        private void repositoryItemCalcEdit4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
    ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
                //kdv hariç
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyatiKdvHaric=" + girilen.Replace(",", ".") + " where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
                DB.ExecuteSQL(@"UPDATE AlisDetay Set AlisFiyati=AlisFiyatiKdvHaric+(AlisFiyatiKdvHaric*KdvOrani)/100
            where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
                yesilisikyeni();
            }
        }

        private void ödemeYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pkAlis = "0", pkTedarikci = "0";
            if (AcikSatisindex == 1)
            {
                pkAlis = Satis1Toplam.Tag.ToString();
                pkTedarikci = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 2)
            {
                pkAlis = Satis2Toplam.Tag.ToString();
                pkTedarikci = Satis2Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 3)
            {
                pkAlis = Satis3Toplam.Tag.ToString();
                pkTedarikci = Satis3Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                pkAlis = Satis4Toplam.Tag.ToString();
                pkTedarikci = Satis4Baslik.Tag.ToString();
            }

            TedarikciHareketleri(pkTedarikci);

        }
        
        private void cbFaturaTipi_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            yesilisikyeni();
        }
        private void repositoryItemCalcEdit4_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle == gridView1.DataRowCount) return;
            string girilen =
   ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
            //kdv hariç
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyatiKdvHaric=" + girilen.Replace(",", ".") + " where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            DB.ExecuteSQL(@"UPDATE AlisDetay Set AlisFiyati=AlisFiyatiKdvHaric+(AlisFiyatiKdvHaric*KdvOrani)/100
            where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            //SatisDetayGetir(Satis1Toplam.Tag.ToString());
        }

        private void repositoryItemCalcEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle == gridView1.DataRowCount) return;
            string girilen =
                   ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
            //kdv dahil
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyati=" + girilen.Replace(",", ".") + " where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            DB.ExecuteSQL(@"UPDATE AlisDetay Set AlisFiyatiKdvHaric=AlisFiyati-(AlisFiyati*KdvOrani)/(100+KdvOrani)
            where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            //SatisDetayGetir(Satis1Toplam.Tag.ToString());
        }

        private void FaturaNo_Leave(object sender, EventArgs e)
        {
            if (FaturaNo.Text != "")
            {
                if (DB.GetData("Select count(*) From Alislar with(nolock) where FaturaNo='" + FaturaNo.Text + "' and pkAlislar<>"+pkAlislar.Text).Rows[0][0].ToString() != "0")
                {
                    MessageBox.Show("Fatura No Daha Önce Kullanıldı.Lütfen Başka Fatura No Giriniz");
                }
            }
            //else
            DB.ExecuteSQL("UPDATE Alislar SET FaturaNo='" + FaturaNo.Text + "' WHERE pkAlislar=" + pkAlislar.Text);
                //DB.ExecuteSQL("UPDATE Satislar set FaturaNo='" + txtFaturaNo.Text + "' where pkSatislar=" + pkSatisBarkod.Text);
            FaturaNo.Tag = 0;
        }

        private void FaturaTarihi_Leave(object sender, EventArgs e)
        {
            if (deFaturaTarihi.Text == "") deFaturaTarihi.DateTime = DateTime.Today;
            DB.ExecuteSQL("UPDATE Alislar SET FaturaTarihi='" + Convert.ToDateTime(deFaturaTarihi.Text).ToString("yyyy-MM-dd") + "' WHERE pkAlislar=" + pkAlislar.Text);
        }

        private void cbFaturaTipi_Enter(object sender, EventArgs e)
        {
            faturatipinde = true;
        }

        private void yeniStokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
               string barkod = "";
               frmStokKarti StokKartiHizli = new frmStokKarti();
               Degerler.stokkartisescal = false;
                DB.pkStokKarti = 0;
            //StokKartiHizli.alisdangeldi.AccessibleDescription = "alisdangeldievet";
            StokKartiHizli.Barkod.EditValue = barkod;
                StokKartiHizli.lblBarkod.Tag = barkod;
                StokKartiHizli.ShowDialog();
                Degerler.stokkartisescal = true;
                //if (StokKartiHizli.TopMost == true)
               // {
                DataTable dtStokKarti = DB.GetData("select pkStokKarti,AlisFiyati,AlisFiyatiKdvHaric From StokKarti with(nolock) WHERE Barcode='" + StokKartiHizli.Barkod.EditValue.ToString() + "'");
               // }
                //else
                //eğer stok kartı oluşturmadı ise 
                if (dtStokKarti.Rows.Count == 0)
                {
                    yesilisikyeni();
                    StokKartiHizli.Dispose();
                    return;
                }
                StokKartiHizli.Dispose();
                
            if (gridView1.DataRowCount == 0) 
                YeniAlisEkle();

            AlisDetayEkle(StokKartiHizli.Barkod.EditValue.ToString());
            //string pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            //ArrayList arr = new ArrayList();
            //arr.Add(new SqlParameter("@fkAlislar", pkAlislar.Text));
            //arr.Add(new SqlParameter("@SatisFiyatGrubu", "1"));
            //arr.Add(new SqlParameter("@AlisFiyati", dtStokKarti.Rows[0]["AlisFiyati"].ToString().Replace(",", ".")));
            //arr.Add(new SqlParameter("@Adet", "1"));//EklenenMiktar.ToString().Replace(",", ".")));
            //arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
            ////arr.Add(new SqlParameter("@iskontoyuzde", "0"));
            ////arr.Add(new SqlParameter("@iskontotutar", "0"));
            //string s = DB.ExecuteScalarSQL("exec sp_AlisDetay_Ekle @fkAlislar,@AlisFiyati,@SatisFiyatGrubu,@Adet,@fkStokKarti", arr);//,@iskontoyuzde,@iskontotutar", arr);
            //if(s!="Alis Detay Eklendi.")
            //{
            //    formislemleri.Mesajform("Hata Oluştu" + s , "K", 150);
            //}
            yesilisikyeni();
        }


        private void btEtiketYazdir_Click(object sender, EventArgs e)
        {
            formislemleri.Mesajform("Yapım Aşamasında", "K", 200);
            return;
            if (Kaydet()==false) return;

            if (btEtiketYazdir.Tag == null || btEtiketYazdir.Tag == "0" || btEtiketYazdir.Tag == "") return;

            //DataTable dt = DB.GetData("select * from AlisDetay where fkAlislar=" + btEtiketYazdir.Tag);
            string pkEtiketBas = "0";
            int etiketadet = 0;
            bool tekliadet = false;
            if (DB.GetData("select * from AlisDetay with(nolock) where Stogaisle=1 and fkAlislar=" + btEtiketYazdir.Tag).Rows.Count > 0)
            {
                DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Miktar Bir(1) Adetmi Olsun?", "Hitit2012", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.Yes) tekliadet = true;
            }
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (i == 0)
                {
                    pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Fiş No " + pkAlislar.Text + "',0) SELECT IDENT_CURRENT('EtiketBas')");
                }
                if (dr["Stogaisle"].ToString() == "True")
                {
                    if (tekliadet)
                        DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["fkStokKarti"].ToString() + ",1,0,getdate())");
                    else
                        DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["fkStokKarti"].ToString() + "," + dr["Adet"].ToString().Replace(",", ".") + ",0,getdate())");
                    etiketadet++;
                }
            }
            if (etiketadet > 0)
            {
                frmEtiketBas EtiketBas = new frmEtiketBas();
                EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
                EtiketBas.ShowDialog();
            }
            SatisTemizle();
            //FisListesi();
            yesilisikyeni();    
        }

        private void repositoryItemCalcEdit5_Leave(object sender, EventArgs e)
        {
            //taksitli tutar
            string taksitlitutar =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
            taksitlifiyatguncelle(taksitlitutar);
                //yesilisikyeni();
        }

        void taksitlifiyatguncelle(string taksitlitutar)
        {
            if (taksitlitutar == "")
            {
                yesilisikyeni();
                return;
            }
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkAlisDetay = dr["pkAlisDetay"].ToString();
            //decimal SatisFiyati2 = Convert.ToDecimal(dr["SatisFiyati2"].ToString());
            //decimal Miktar = Convert.ToDecimal(dr["Adet"].ToString().Replace(",", "."));
            //if (Miktar == 0) return;
            //decimal iskontoyuzde = (Convert.ToDecimal(taksitlitutar) * 100) / (Fiyat * Miktar);
            //gridView1.SetFocusedRowCellValue(gridColumn33, iskontoyuzde);
            DB.ExecuteSQL("UPDATE AlisDetay SET SatisFiyati2=" + taksitlitutar.Replace(",", ".") + " where pkAlisDetay=" + pkAlisDetay);
        }

        void satisfiyatguncelle(string taksitlitutar)
        {
            if (taksitlitutar == "")
            {
                yesilisikyeni();
                return;
            }
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkAlisDetay = dr["pkAlisDetay"].ToString();
            //decimal SatisFiyati2 = Convert.ToDecimal(dr["SatisFiyati2"].ToString());
            //decimal Miktar = Convert.ToDecimal(dr["Adet"].ToString().Replace(",", "."));
            //if (Miktar == 0) return;
            //decimal iskontoyuzde = (Convert.ToDecimal(taksitlitutar) * 100) / (Fiyat * Miktar);
            //gridView1.SetFocusedRowCellValue(gridColumn33, iskontoyuzde);
            
            DB.ExecuteSQL("UPDATE AlisDetay SET SatisFiyati=" + taksitlitutar.Replace(",", ".") + " where pkAlisDetay=" + pkAlisDetay);
            DB.ExecuteSQL("UPDATE AlisDetay set Stogaisle=1 where SatisFiyati<>NakitFiyat and pkAlisDetay=" + pkAlisDetay);
        }

        private void repositoryItemCalcEdit5_KeyDown(object sender, KeyEventArgs e)
        {
            //taksitli tutar
            if (e.KeyCode == Keys.Enter)
            {
                string taksitlitutar =
                    ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
                taksitlifiyatguncelle(taksitlitutar);
                yesilisikyeni();
            }
        }

        private void repositoryItemButtonEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            //taksitli tutar
            if (e.KeyCode == Keys.Enter)
            {
                if (((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;
                string satisfiyati =
                    ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
                satisfiyatguncelle(satisfiyati);
                yesilisikyeni();
            }

            //if (e.KeyCode == Keys.Enter)
            //{
            //    string girilen =
            //        ((DevExpress.XtraEditors.ButtonEdit)((((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue.ToString();
            //    if (gridView1.FocusedRowHandle < 0) return;
            //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //    if (dr == null) return;
            //    string pkSatisDetay = dr["pkAlisDetay"].ToString();
            //    //string pkStokKarti = dr["fkStokKarti"].ToString();
            //    DB.ExecuteSQL("UPDATE AlisDetay SET SatisFiyati=" + girilen.Replace(",", ".") + " where pkAlisDetay=" + pkSatisDetay);
            //    yesilisikyeni();
            //}
        }

        private void repositoryItemButtonEdit3_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;
                string taksitlitutar =
                    ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
                satisfiyatguncelle(taksitlitutar);
                //yesilisikyeni();
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            if (dr == null) return;
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["pkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (aratoplam.Value == 0)
            {
                DB.ExecuteSQL("UPDATE Alislar SET iskontoFaturaYuzde=0 where pkAlislar=" + pkAlislar.Text);
                DB.ExecuteSQL("UPDATE Alislar SET iskontoFaturaTutar=0 where pkAlislar=" + pkAlislar.Text);
                return;
            }
            decimal aratoplamkdvsiz=aratoplam.Value;
            if (cbFaturaTipi.SelectedIndex == 1)//kdv hariç
            {
                aratoplamkdvsiz = aratoplam.Value;
            }
            
            decimal yuzde = Convert.ToDecimal(((ceiskontoTutar.Value * 100) / aratoplamkdvsiz));
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE AlisDetay SET iskontotutarfatura=(((AlisFiyati-iskontotutar)* round(" + yuzde.ToString().Replace(",", ".") + ",2))/100) where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            }
            DB.ExecuteSQL("UPDATE Alislar SET iskontoFaturaYuzde=round(" + yuzde.ToString().Replace(",", ".") + ",2) where pkAlislar=" + pkAlislar.Text);
            DB.ExecuteSQL("UPDATE Alislar SET iskontoFaturaTutar=" + ceiskontoTutar.Text.Replace(",", ".") + " where pkAlislar=" + pkAlislar.Text);
        }

        private void FaturaNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void pMenuStokHareketi_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["fkStokKarti"].ToString();
            StokHareketleri.ShowDialog();

            yesilisikyeni();

            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0");
            //SatisUrunBazindaDetay.Tag="2";
            //SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            //SatisUrunBazindaDetay.ShowDialog();
            //yesilisikyeni();
        }

        private void repositoryItemCheckEdit1_CheckStateChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string sec = ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();
          //  if (sec == "True")
           //     sec = "1";
           // else
            //    sec = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
           // DB.ExecuteSQL("UPDATE AlisDetay SET iade=" + sec + " where pkAlisDetay=" + dr["pkAlisDetay"].ToString());

            if (sec == "False")
            {
                DB.ExecuteSQL("update AlisDetay Set iade=0,Adet=abs(Adet) where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
                //gridView1.SetFocusedRowCellValue("iade", "False");
            }
            else
            {
                DB.ExecuteSQL("update AlisDetay Set iade=1,Adet=Adet*-1 where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
                //gridView1.SetFocusedRowCellValue("iade", "True");
            }
            yesilisikyeni();
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            //if (ghi.Column == null) return;
            ////GridView View = sender as GridView;
            //filtir ise
            if (ghi.RowHandle == -999997) return;
            
            if (ghi.InRowCell)
            {
                int rowHandle = ghi.RowHandle;
                //DevExpress.XtraGrid.Columns.GridColumn column = ghi.Column;
                if (ghi.Column.FieldName == "Tutar")
                {
                    frmFisAciklama fFisAciklama = new frmFisAciklama();
                    fFisAciklama.panelControl2.Visible = false;
                    fFisAciklama.btnCancel.Visible = true;
                    fFisAciklama.btnTemizle.Visible = false;
                    fFisAciklama.Text = "Tutar Hesapla";
                    fFisAciklama.pcTutarHesapla.Visible=true;
                    fFisAciklama.pcTutarHesapla.BringToFront();

                    DataRow dr = gridView1.GetDataRow(ghi.RowHandle);
                    decimal tutar = decimal.Parse(dr["Tutar"].ToString());
                    decimal adet = decimal.Parse(dr["Adet"].ToString());
                    decimal kdvorani = decimal.Parse(dr["KdvOrani"].ToString());

                    fFisAciklama.seMiktar.Value = adet;
                    fFisAciklama.ceTutari.Value = tutar;
                    
                    fFisAciklama.ShowDialog();

                    if (fFisAciklama.Tag.ToString() == "0")
                    {
                        DevExpress.XtraGrid.Columns.GridColumn secilenGridColumn = gridView1.FocusedColumn;
   
                        yesilisikyeni();

                        gridView1.FocusedRowHandle = rowHandle;
                        gridView1.FocusedColumn = secilenGridColumn;
                        gridView1.CloseEditor();
                        return;
                    } 

                    decimal aliskdvdahil, aliskdvharic, satisfiyati;
                    satisfiyati=fFisAciklama.calcEdit1.Value;
                    //kdv dahil ise
                    if (cbFaturaTipi.SelectedIndex==0)
                    {
                        aliskdvdahil = satisfiyati;
                        //kdvtutar = (SatisFiyatiKdv * kdv) / (100 + kdv);
                        aliskdvharic = satisfiyati-((satisfiyati * kdvorani) / (100 + kdvorani));
                    }
                    else
                    {
                        aliskdvdahil = satisfiyati + (fFisAciklama.calcEdit1.Value * kdvorani) / 100;
                        aliskdvharic = satisfiyati;
                    }
                    DB.ExecuteSQL("update AlisDetay set AlisFiyati=" + aliskdvdahil.ToString().Replace(",", ".") +
                         ",AlisFiyatiKdvHaric=" + aliskdvharic.ToString().Replace(",", ".") +
                        " where pkAlisDetay=" + dr["pkAlisDetay"].ToString());

                    DevExpress.XtraGrid.Columns.GridColumn secilenGridColumn2 = gridView1.FocusedColumn;

                    //iskontoTutarGuncelle();

                    //gridView1.FocusedRowHandle = rowHandle;
                    gridView1.FocusedColumn = secilenGridColumn2;
                    //gridView1.CloseEditor();
                    //yesilisikyeni();

                    //gridView1.Focus();
                    //gridView1.FocusedRowHandle = gridView1.DataRowCount;
                    //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                    //gridView1.CloseEditor();

                    SendKeys.Send("{ENTER}");
                    SendKeys.Send("{ENTER}");

                    return;
                }
            }

            if (ghi.RowHandle < 0 && gridView1.CustomizationForm == null)
                yesilisikyeni();
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string fkFirma = "0";
            if (AcikSatisindex == 1)
            {
                fkFirma = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 2)
            {

                fkFirma = Satis2Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 3)
            {
                fkFirma = Satis3Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                fkFirma = Satis4Firma.Tag.ToString();
            }
            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            //GridView View = sender as GridView;
            if (ghi.RowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(ghi.RowHandle);
            if (ghi.Column.FieldName == "Stokadi")
            {
                frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay(fkFirma, "", "");
                SatisUrunBazindaDetay.Tag = "4";
                SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
                SatisUrunBazindaDetay.ShowDialog();
                yesilisikyeni();
            }
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\AlisFaturaGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);

            gridView1.OptionsBehavior.AutoPopulateColumns = false;
            gridView1.OptionsCustomization.AllowColumnMoving = false;
            gridView1.OptionsCustomization.AllowColumnResizing = false;
            gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            gridView1.OptionsCustomization.AllowRowSizing = false;
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\AlisFaturaGrid.xml";
            if (File.Exists(Dosya))
                File.Delete(Dosya);

            simpleButton19_Click(sender, e);//kapat
        }

        private void gridView1_HideCustomizationForm(object sender, EventArgs e)
        {
            kaydetToolStripMenuItem1_Click(sender, e);
        }

        private void repositoryItemCalcEdit3_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
            gridView1.CloseEditor();
        }

        private void repositoryItemButtonEdit2_Leave(object sender, EventArgs e)
        {
            if (repositoryItemButtonEdit2.Tag.ToString() == "0") return;

            if (((DevExpress.XtraEditors.ButtonEdit)((((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).OldEditValue == null)
            {
                yesilisikyeni();
                return;
            }
            
            string yeniiskonto =
                ((DevExpress.XtraEditors.ButtonEdit)((((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue.ToString();
            if (yeniiskonto == "0") yeniiskonto = "0";

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkAlisDetay = dr["pkAlisDetay"].ToString();

            DB.ExecuteSQL("UPDATE AlisDetay SET iskontoyuzdetutar=" + yeniiskonto.Replace(",", ".") +
                " where pkAlisDetay=" + pkAlisDetay);

            DB.ExecuteSQL("UPDATE AlisDetay SET iskontotutar=(AlisFiyati*iskontoyuzdetutar)/100 where pkAlisDetay=" + pkAlisDetay);

            if (yeniiskonto=="0")
                DB.ExecuteSQL("UPDATE iskontolar SET Yuzde =0  where fkAlisDetay=" + pkAlisDetay);

            int i = gridView1.FocusedRowHandle;

            //iskontoTutarGuncelle();

            AlisDetayYenile();

            gridView1.FocusedRowHandle = i;
        }

        private void repositoryItemSpinEdit1_Leave(object sender, EventArgs e)
        {
            string girilen =
  ((DevExpress.XtraEditors.SpinEdit)((((DevExpress.XtraEditors.SpinEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null) return;
            string pkSatisDetay = dr["pkAlisDetay"].ToString();

            //decimal NakitFiyati = decimal.Parse(dr["NakitFiyati"].ToString());
            //decimal iskonto = 0, iskontotutar = 0, iskontoyuzde = 0;
            //decimal.TryParse(girilen, out iskontotutar);
            //iskonto = NakitFiyati - iskontotutar;
            //iskontoyuzde = (iskonto * 100) / NakitFiyati;

            DB.ExecuteSQL("UPDATE AlisDetay SET KdvOrani=" + girilen.ToString().Replace(",", ".") +
                " where pkAlisDetay=" + pkSatisDetay);
            //yesilisikyeni();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            toplu_kdv_iskonto_degis("k");
        }

        private void repositoryItemButtonEdit2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                repositoryItemButtonEdit2_ButtonClick(sender, null);
                //simpleButton4_Click(sender, e);
                //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                //xtraTabPage5_Click( sender, e);
            }
        }

        private void repositoryItemSpinEdit1_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
            gridView1.CloseEditor();
        }
        
        void toplu_kdv_iskonto_degis(string kdv_isk)
        {
            inputForm sifregir = new inputForm();
            //sifregir.Girilen.Properties.PasswordChar = '#';
            if (kdv_isk == "k")
            {
                sifregir.Text = "Toplu Kdv Oranı";
                sifregir.GirilenCaption.Text = "Kdv Oranı Giriniz";
            }
            else
            {
                sifregir.Text = "Toplu İskonto Oranı";
                sifregir.GirilenCaption.Text = "İskonto Oranı Giriniz";
            }
            sifregir.ShowDialog();

            string girilen = sifregir.Girilen.Text;
            girilen = girilen.Replace(",",".");
            if (girilen == "")
            {
                yesilisikyeni();
                return;
            }

            if (kdv_isk == "k")
                DB.ExecuteSQL("update AlisDetay set KdvOrani=" + girilen + " where fkAlislar=" + pkAlislar.Text);
            if (kdv_isk == "i")
            {
                DB.ExecuteSQL("update AlisDetay set iskontoyuzdetutar=" + girilen + " where fkAlislar=" + pkAlislar.Text);
                //if (girilen == "0")
                  //  DB.ExecuteSQL(@"update AlisDetay set iskontotutar=0  where fkAlislar=" + pkAlislar.Text);
//                else
//                    DB.ExecuteSQL(@"update AlisDetay set iskontotutar=(AlisFiyati*isnull(iskontoyuzdetutar,0))/100
//                                where iskontoyuzdetutar>0 and fkAlislar=" + pkAlislar.Text);
                iskontoTutarGuncelle();
            }

            yesilisikyeni();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            toplu_kdv_iskonto_degis("i");
        }

        private void btnSatisRaporlari_Click(object sender, EventArgs e)
        {
            frmAlisRaporlari Satislar = new frmAlisRaporlari();
            Satislar.ShowDialog();
            yesilisikyeni();
        }
        DevExpress.XtraGrid.GridControl targetGrid = null;
        DevExpress.XtraGrid.Views.Grid.GridView gridView = null;
        private void btnGecmisFisler_Click(object sender, EventArgs e)
        {
            if (targetGrid == null)
            {
                targetGrid = new DevExpress.XtraGrid.GridControl();
                targetGrid.ContextMenuStrip = cMenuFisGecmis;
                gridView = new GridView(targetGrid);
                targetGrid.Name = "ara";
                gridView.Name = "ReportView";
                targetGrid.ViewCollection.Add(gridView);
                targetGrid.MainView = gridView;
                gridView.GridControl = targetGrid;
                this.Controls.Add(targetGrid);
                //gridView.ShowFilterPopup(gridView.Columns[0]);
                gridView.OptionsView.ShowGroupPanel = false;
                gridView.OptionsBehavior.Editable = false;
                //gridView.FocusedRowChanged += new FocusedRowChangedEventHandler(gridView_FocusedRowChanged);
                this.gridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
                gridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridView_KeyDown);
            }
            //if (tEStokadi.Text.Length > 2)
            //{
            //    if (tEStokadi.Text.IndexOf(" ") == -1)
            string sql = "";
           // if (lueSatisTipi.EditValue.ToString() == "1")
//                sql = @"select s.fkFirma,pkSatislar as FisNo,f.Firmaadi as MusteriAdi,cast(round(ToplamTutar, 3, 1) as decimal(18,2)) as ToplamTutar,s.GuncellemeTarihi as Tarih,
//s.OdemeSekli,k.KullaniciAdi,Sd.Durumu from Satislar s with(nolock)
//LEFT JOIN Firmalar f with(nolock) ON s.fkFirma = f.PkFirma
//LEFT JOIN Kullanicilar k with(nolock) ON s.fkKullanici=k.pkKullanicilar
//LEFT JOIN SatisDurumu sd with(nolock) ON sd.pkSatisDurumu=s.fkSatisDurumu
//where s.Siparis=1 and s.fkSatisDurumu=1 order by SonislemTarihi desc";
//            else if (lueSatisTipi.EditValue.ToString() == "10")
//            {
//                sql = @"select fkFirma,pkArayanlar as FisNo,f.Firmaadi as MusteriAdi, 0 as ToplamTutar,Tarih  from Arayanlar a with(nolock)
//                      LEFT JOIN Firmalar f with(nolock) ON a.fkFirma = f.PkFirma
//                      where Siparis=1";
//            }
//            else
//                sql = @"select Top 20 s.fkFirma,pkSatislar as FisNo,f.Firmaadi as MusteriAdi,cast(round(ToplamTutar, 3, 1) as decimal(18,2)) as ToplamTutar,s.GuncellemeTarihi as Tarih,
//s.OdemeSekli,k.KullaniciAdi,Sd.Durumu from Satislar s with(nolock)
//LEFT JOIN Firmalar f with(nolock) ON s.fkFirma = f.PkFirma
//LEFT JOIN Kullanicilar k with(nolock) ON s.fkKullanici=k.pkKullanicilar
//LEFT JOIN SatisDurumu sd with(nolock) ON sd.pkSatisDurumu=s.fkSatisDurumu
//where s.Siparis=1 and s.fkSatisDurumu>1 order by SonislemTarihi desc";
            sql = @"SELECT top 20 s.fkFirma,s.pkAlislar as FisNo, f.Firmaadi as TedarikciAdi, 
s.ToplamTutar as Tutar,s.GuncellemeTarihi as Tarih,s.OdemeSekli FROM Alislar s  with(nolock)
INNER JOIN Tedarikciler f  with(nolock) ON s.fkFirma = f.pkTedarikciler
WHERE Siparis=1
order by s.GuncellemeTarihi desc";
            targetGrid.DataSource = DB.GetData(sql);

            targetGrid.BringToFront();
            targetGrid.Width = 650;
            targetGrid.Height = 300;
            targetGrid.Left = groupControl6.Left+150;
                //pkAlislar.Left + lblSonAlislar.Left;
            targetGrid.Top = pkAlislar.Top + 30;
            if (gridView.Columns.Count > 0)
            {
                gridView.Columns[1].Width = 50;
                gridView.Columns[2].Width = 150;
                gridView.Columns[4].Width = 100;
                gridView.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView.Columns[4].DisplayFormat.FormatString = "g";
                //gridView.Columns[3].DisplayFormat.FormatString = "{0:n2}";
                //gridView.Columns[3].DisplayFormat.FormatType = FormatType.Numeric;
                gridView.Columns[0].Visible = false;
            }
            if (gridView.DataRowCount == 0)
                targetGrid.Visible = false;
            else
            {
                targetGrid.Visible = true;
                targetGrid.Focus();
            }
        }

        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
                FisGetir(dr["FisNo"].ToString());
                targetGrid.Visible = false;
            }
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            if (gridView.FocusedRowHandle < 0) return;

            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);

            //if (lueSatisTipi.EditValue.ToString() == "10")
            //{
            //    string pkArayanlar = dr["FisNo"].ToString();
            //    DB.ExecuteSQL("update Arayanlar Set Siparis=0 where pkArayanlar=" + pkArayanlar);
            //}
            //else
            FisGetir(dr["FisNo"].ToString());
            targetGrid.Visible = false;
        }

        void FisGetir(string Fisno)
        {
            //if (lueFis.EditValue == null) return;

            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(true);
            FisNoBilgisi.fisno.EditValue = Fisno;//lueFis.EditValue.ToString();
            FisNoBilgisi.ShowDialog();

          
            if (FisNoBilgisi.btnFisDuzenle.Tag.ToString() == "1")
            {
                #region Satış Fiyatlarını Güncelle
                if (DB.GetData(@"select count(*) From AlisDetay ad with(nolock) 
                                left join (select sf.fkStokKarti,sf.SatisFiyatiKdvli from SatisFiyatlari sf with(nolock)
                                inner join SatisFiyatlariBaslik sfb with(nolock)
                                on sf.fkSatisFiyatlariBaslik=sfb.pkSatisFiyatlariBaslik
                                where sfb.Tur=1) sf  on sf.fkStokKarti=ad.fkStokKarti
                                where ad.SatisFiyati<>sf.SatisFiyatiKdvli and 
                                ad.fkAlislar=" + FisNoBilgisi.fisno.EditValue.ToString()).Rows[0][0].ToString() != "0")
                {
                    DialogResult secim;
                    secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Fiyatlarında Değişiklik Yapılmıştır.\n\nStok Kartı Satış Fiyaları ile Aynı Yapılsın mı?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                    if (secim == DialogResult.Yes)
                        DB.ExecuteSQL(@"update AlisDetay set SatisFiyati=sf.SatisFiyatiKdvli from 
                        (select sf.fkStokKarti,sf.SatisFiyatiKdvli from SatisFiyatlari sf with(nolock)
                        inner join SatisFiyatlariBaslik sfb with(nolock)
                        on sf.fkSatisFiyatlariBaslik=sfb.pkSatisFiyatlariBaslik where sfb.Tur=1) sf 
                        where AlisDetay.SatisFiyati<>sf.SatisFiyatiKdvli and AlisDetay.fkStokKarti=sf.fkStokKarti and fkAlislar=" +
                         FisNoBilgisi.fisno.EditValue.ToString());
                }

                #endregion

                pkAlislar.Text = FisNoBilgisi.fisno.EditValue.ToString();
                //kasahareketleri sil
                DB.ExecuteSQL("delete from KasaHareket where fkAlislar=" + pkAlislar.Text);
                //Alis Sipariş çevir
                //DB.ExecuteSQL("UPDATE Alislar SET DuzenlemeTarihi=getdate(),Siparis=0 where pkAlislar=" + pkAlislar.Text);
                AcikSatisindex = 4;
                Satis4Firma.Visible = true;
                pkAlislar.Text = Fisno;//lueFis.EditValue.ToString();
                Satis4Toplam.Tag = Fisno;// lueFis.EditValue.ToString();
                //Satis4Toplam_Click(null, null);
                AlisGetir();
                //deAlisTarihi.DateTime = DateTime.Now;
            }
            FisNoBilgisi.Dispose();
            //lueFis.EditValue = null;
            //FisListesi();
            yesilisikyeni();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);

            fkfirma = dr["fkFirma"].ToString();

            TedarikciHareketleri(fkfirma);
        }
        void TedarikciHareketleri(string pkTedarikci)
        {
            frmTedarikcilerHareketleri TedarikciHareketleri = new frmTedarikcilerHareketleri();
            TedarikciHareketleri.musteriadi.Tag = pkTedarikci;
            TedarikciHareketleri.Show();
        }

        private void gridControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void tMenuitemOdemeAl_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            fkfirma = dr["fkFirma"].ToString();
            targetGrid.Visible = false;

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkTedarikci.Text = fkfirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
        }

        private void MenuitemOdemeAl_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            fkfirma = dr["fkFirma"].ToString();
            targetGrid.Visible = false;

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkTedarikci.Text = fkfirma;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();
        }

        private void ödemeYapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);

            fkfirma = dr["fkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkTedarikci.Text = fkfirma;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);

            fkfirma = dr["fkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkTedarikci.Text = fkfirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
        }

        private void ceAraToplamiskontosuz_EditValueChanged(object sender, EventArgs e)
        {
            if (ceAraToplamiskontosuz.EditValue == null) return;
            if (ceiskontoTutar.EditValue == null) return;
            ceAraToplam.EditValue = decimal.Parse(ceAraToplamiskontosuz.EditValue.ToString()) - decimal.Parse(ceiskontoTutar.EditValue.ToString());
        }

        private void repositoryItemCheckEdit3_CheckStateChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string sec = ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (sec == "False")
            {
                DB.ExecuteSQL("update AlisDetay Set Kontrol=0 where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            }
            else
            {
                DB.ExecuteSQL("update AlisDetay Set Kontrol=1 where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
            }
            //yesilisikyeni();
        }

        private void alışİadeFaturasıDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirSatisFaturasi(true);
        }

        private void ceiade_CheckedChanged(object sender, EventArgs e)
        {
            string  sql = "" ;
            if (ceiade.Checked) 
            {
                sql = "UPDATE AlisDetay SET iade=1,Adet=Adet*-1 where fkAlislar="+pkAlislar.Text;
            }
            else
            {
                sql = "UPDATE AlisDetay SET iade=0,Adet=abs(Adet) where fkAlislar=" + pkAlislar.Text;
            }
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{
              //  DataRow dr = gridView1.GetDataRow(i);
                //sql = sql + dr["pkAlisDetay"].ToString();
            DB.ExecuteSQL(sql);
            //}
            yesilisikyeni();
        }

        private void FaturaNo_EditValueChanged(object sender, EventArgs e)
        {
            if (pkAlislar.Text == "") return;

            if (FaturaNo.Tag == "1")
               DB.ExecuteSQL("UPDATE Alislar SET FaturaNo='" + FaturaNo.Text + "' WHERE pkAlislar=" + pkAlislar.Text);
        }

        private void repositoryItemCheckEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void detaylıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("Dış Veri Alırken Excel 2003 formatında ve ilk Sayfa Adı Sayfa1 Olmalı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                DataTable dtayar = DB.GetData("select * from  ayarlar with(nolock) where Ayar20='excelyol'");
                if (dtayar.Rows.Count > 0)
                    openFileDialog1.InitialDirectory = dtayar.Rows[0]["Ayar50"].ToString();
                else
                    openFileDialog1.InitialDirectory = Degerler.YedekYolu;

                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();

                if (openFileDialog1.FileName == "") return;

                OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OleDb.4.0;Data Source = " +
                    openFileDialog1.FileName + " ; Extended Properties = Excel 8.0");//" ; Extended Properties = Excel 8.0");
                //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
                OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
                //MessageBox.Show((System.Exception)(con.ServerVersion).Message);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string StokKod = dt.Rows[i]["StokKodu"].ToString();
                    string Barcode = dt.Rows[i]["Barcode"].ToString();
                    string Stokadi = dt.Rows[i]["StokAdı"].ToString();
                    string RenkKodu = dt.Rows[i]["RenkKodu"].ToString();
                    string BedenKodu = dt.Rows[i]["BedenKodu"].ToString();
                    string Miktar = "1";
                    if (dt.Rows[i]["Miktar"].ToString() != "")
                    {
                        Miktar = dt.Rows[i]["Miktar"].ToString();
                        HizliMiktar = decimal.Parse(Miktar);
                    }
                    string BirimFiyati = "0";
                    if (dt.Rows[i]["BirimFiyati"].ToString() != "")
                        BirimFiyati = dt.Rows[i]["BirimFiyati"].ToString();
                    string PSFiyati = dt.Rows[i]["PSFiyati"].ToString();

                    string TaksitliPS = "0";
                    if (dt.Rows[i]["TaksitliPS"].ToString() == "")
                        TaksitliPS = PSFiyati;
                    else
                        TaksitliPS = dt.Rows[i]["TaksitliPS"].ToString();
                    //03.06.2013  kampanyalı fiyat
                    string IlkPS = "0";
                    if (dt.Rows[i]["IlkPS"].ToString() == "")
                        IlkPS = PSFiyati;
                    else
                        IlkPS = dt.Rows[i]["IlkPS"].ToString();

                    string KDV = dt.Rows[i]["KDV"].ToString();
                    if (KDV.Contains("18")) KDV = "18";
                    else if (KDV.Contains("8")) KDV = "8";
                    else if (KDV.Contains("0")) KDV = "0";
                    else KDV = "18";
                    //IlkPS
                    //ToptanIF
                    //KDV S08
                    DataTable dtStokKarti = DB.GetData("select * from StokKarti with(nolock) where Barcode='" + Barcode + "'");
                    string pkStokKarti = "0";
                    if (dtStokKarti.Rows.Count == 0)
                    {
                        ArrayList list = new ArrayList();
                        list.Add(new SqlParameter("@StokKod", StokKod));
                        list.Add(new SqlParameter("@UreticiKodu", StokKod));
                        list.Add(new SqlParameter("@Barcode", Barcode));
                        list.Add(new SqlParameter("@Stokadi", Stokadi));
                        string fkRenkGrupKodu = "0";
                        if (RenkKodu.Length > 0)
                        {
                            DataTable dtRenk = DB.GetData("select pkRenkGrupKodu from RenkGrupKodu with(nolock) where Aciklama='" + RenkKodu + "'");
                            if (dtRenk.Rows.Count == 0)
                            {
                                fkRenkGrupKodu = DB.ExecuteScalarSQL("INSERT INTO RenkGrupKodu (Aciklama) VALUES('" + RenkKodu + "') select IDENT_CURRENT('RenkGrupKodu')");
                                list.Add(new SqlParameter("@fkRenkGrupKodu", fkRenkGrupKodu));
                            }
                            else
                                list.Add(new SqlParameter("@fkRenkGrupKodu", dtRenk.Rows[0]["pkRenkGrupKodu"].ToString()));
                        }
                        else
                            list.Add(new SqlParameter("@fkRenkGrupKodu", fkRenkGrupKodu));
                        //
                        string fkBedenGrupKodu = "0";
                        if (BedenKodu.Length > 0)
                        {
                            DataTable dtBeden = DB.GetData("select pkBedenGrupKodu from BedenGrupKodu with(nolock) where Aciklama='" + BedenKodu + "'");
                            if (dtBeden.Rows.Count == 0)
                            {
                                fkRenkGrupKodu = DB.ExecuteScalarSQL("INSERT INTO BedenGrupKodu (Aciklama) VALUES('" + BedenKodu + "') select IDENT_CURRENT('BedenGrupKodu')");
                                list.Add(new SqlParameter("@fkBedenGrupKodu", fkRenkGrupKodu));
                            }
                            else
                                list.Add(new SqlParameter("@fkBedenGrupKodu", dtBeden.Rows[0]["pkBedenGrupKodu"].ToString()));
                        }
                        else
                            list.Add(new SqlParameter("@fkBedenGrupKodu", fkBedenGrupKodu));
                        list.Add(new SqlParameter("@Stoktipi", "ADET"));
                        list.Add(new SqlParameter("@Mevcut", "0"));//HizliMiktar.ToString().Replace(",", ".")));
                        list.Add(new SqlParameter("@KdvOrani", KDV));
                        list.Add(new SqlParameter("@KdvOraniAlis", KDV));
                        list.Add(new SqlParameter("@AlisFiyati", BirimFiyati.Replace(",", ".")));
                        list.Add(new SqlParameter("@SatisFiyati", PSFiyati.Replace(",", ".")));

                        string sql = "INSERT INTO StokKarti (StokKod,Barcode,fkRenkGrupKodu,fkBedenGrupKodu,Stokadi,Stoktipi,Mevcut,AlisFiyati,SatisFiyati,KdvOrani,Aktif,KritikMiktar,UreticiKodu,SatisAdedi,SonAlisTarihi,KdvOraniAlis)" +
                        " values(@StokKod,@Barcode,@fkRenkGrupKodu,@fkBedenGrupKodu,@Stokadi,@Stoktipi,@Mevcut,@AlisFiyati,@SatisFiyati,@KdvOrani,1,0,@UreticiKodu,1,getdate(),@KdvOraniAlis) select IDENT_CURRENT('StokKarti')";

                        try
                        {
                            pkStokKarti = DB.ExecuteScalarSQL(sql, list);
                        }
                        catch (Exception exp)
                        {
                            pkStokKarti = "0";
                        }

                        //Application.DoEvents();

                        AlisDetayEkle(Barcode);
                        //TODO: KONTROL ET
                        //DataTable dtMaxlisDetay = DB.GetData("select top 1 * from AlisDetay with(nolock) order by pkAlisDetay desc");
                        //if (dtMaxlisDetay.Rows.Count > 0)
                        //{
                        //    ArrayList list2 = new ArrayList();
                        //    list2.Add(new SqlParameter("@SatisFiyati", PSFiyati.Replace(",", ".")));
                        //    list2.Add(new SqlParameter("@SatisFiyati2", TaksitliPS.Replace(",", ".")));
                        //    list2.Add(new SqlParameter("@SatisFiyati3", IlkPS.Replace(",", ".")));
                        //    list2.Add(new SqlParameter("@AlisFiyati", BirimFiyati.Replace(",", ".")));
                        //    list2.Add(new SqlParameter("@Adet", Miktar.Replace(",", ".")));
                        //    DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyati=@AlisFiyati,SatisFiyati=@SatisFiyati,SatisFiyati2=@SatisFiyati2,SatisFiyati3=@SatisFiyati3,Adet=@Adet where pkAlisDetay=" + dtMaxlisDetay.Rows[0]["pkAlisDetay"].ToString(), list2);
                        //}
                    }
                    else
                    {
                        //Stok Kartı Var ise
                        pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();

                        AlisDetayEkle(Barcode);

                        DataTable dtMaxlisDetay = DB.GetData("select top 1 * from AlisDetay with(nolock) where fkStokKarti=" + pkStokKarti + " order by pkAlisDetay desc");
                        if (dtMaxlisDetay.Rows.Count > 0)
                        {
                            ArrayList list = new ArrayList();
                            list.Add(new SqlParameter("@SatisFiyati", PSFiyati.Replace(",", ".")));
                            list.Add(new SqlParameter("@SatisFiyati2", TaksitliPS.Replace(",", ".")));
                            list.Add(new SqlParameter("@SatisFiyati3", IlkPS.Replace(",", ".")));
                            list.Add(new SqlParameter("@Adet", Miktar.Replace(",", ".")));
                            list.Add(new SqlParameter("@AlisFiyati", BirimFiyati.Replace(",", ".")));

                            DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyati=@AlisFiyati,SatisFiyati=@SatisFiyati,SatisFiyati2=@SatisFiyati2,SatisFiyati3=@SatisFiyati3,Adet=@Adet where pkAlisDetay=" + dtMaxlisDetay.Rows[0]["pkAlisDetay"].ToString(), list);
                        }
                    }
                    //TODO:SATIŞ FİYATLARI EKLE
                    DataTable dtSF = DB.GetData("Select * FROM SatisFiyatlariBaslik with(nolock) where Aktif=1");
                    for (int si = 0; si < dtSF.Rows.Count; si++)
                    {
                        string pkSatisFiyatlariBaslik = dtSF.Rows[si]["pkSatisFiyatlariBaslik"].ToString();
                        string tur = dtSF.Rows[si]["Tur"].ToString();

                        string sql = "";
                        if (tur == "1")//nakit ise
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + PSFiyati.Replace(",", ".") + "," +
                                    PSFiyati.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + PSFiyati.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    PSFiyati.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                        else if (tur == "2")//kredi kartı
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + TaksitliPS.Replace(",", ".") + "," +
                                    TaksitliPS.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + TaksitliPS.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    TaksitliPS.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                        else //diğerleri
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + TaksitliPS.Replace(",", ".") + "," +
                                    TaksitliPS.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + TaksitliPS.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    TaksitliPS.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                    }
                    HizliMiktar = 1;
                }
                //taksitli fiyatları güncell
                DB.ExecuteSQL(@"update AlisDetay set SatisFiyati2=sf.SatisFiyatiKdvli  from (
select SatisFiyatlari.SatisFiyatiKdvli,SatisFiyatlari.fkStokKarti from SatisFiyatlari left join SatisFiyatlariBaslik sfb on sfb.pkSatisFiyatlariBaslik=SatisFiyatlari.fkSatisFiyatlariBaslik where sfb.Tur=2) sf
where AlisDetay.fkAlislar=" + pkAlislar.Text + " and sf.fkStokKarti=AlisDetay.fkStokKarti");
                yesilisikyeni();
                // gridControl1.DataSource = dt;
            }
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Yeni\v.xls;Extended Properties=”Excel 8.0;HDR=Yes;IMEX=1″
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
            
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("Dış Veri Alırken Excel 2003 formatında ve ilk Sayfa Adı Sayfa1 Olmalı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                DataTable dtayar = DB.GetData("select * from  ayarlar with(nolock) where Ayar20='excelyol'");
                if (dtayar.Rows.Count > 0)
                    openFileDialog1.InitialDirectory = dtayar.Rows[0]["Ayar50"].ToString();
                else
                    openFileDialog1.InitialDirectory = Degerler.YedekYolu;

                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();

                if (openFileDialog1.FileName == "") return;


                OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+openFileDialog1.FileName+"; Extended Properties='Excel 12.0 Xml;HDR=YES'"); //excel_dosya.xlsx kısmını kendi excel dosyanızın adıyla değiştirin.
                //DataTable tablo = new DataTable();
                con.Open();
               // OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OleDb.4.0;Data Source = " +
                 //   openFileDialog1.FileName + " ; Extended Properties = Excel 8.0;");
                //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
                OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
                //MessageBox.Show((System.Exception)(con.ServerVersion).Message);
                DataTable dt = new DataTable();
                try
                {
                   // con.Open();
                    
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show("hata : " + exp.Message);
                    //throw;
                    return;
                }
                da.Fill(dt);

                int c = dt.Rows.Count, h = 0, b = 0,ayni=0;
                for (int i = 0; i < c; i++)
                {
                    string StokKod = dt.Rows[i]["STOKKODU"].ToString();
                    string Barcode = dt.Rows[i]["BARKOD"].ToString();
                    string UreticiKodu = dt.Rows[i]["URETICIKODU"].ToString();

                    if (Barcode == "") continue;
                    
                    if (StokKod == "") StokKod = Barcode;

                    string Stokadi = dt.Rows[i]["STOKADI"].ToString();
                    //string RenkKodu = dt.Rows[i]["RenkKodu"].ToString();
                    //string BedenKodu = dt.Rows[i]["BedenKodu"].ToString();
                    string Miktar = "1";
                    if (dt.Rows[i]["MEVCUT"].ToString() != "")
                    {
                        Miktar = dt.Rows[i]["MEVCUT"].ToString();
                        HizliMiktar = decimal.Parse(Miktar);
                    }
                    string BirimFiyati = "0.001";
                    if (dt.Rows[i]["ALISFIYATI"].ToString() != "")
                        BirimFiyati = dt.Rows[i]["ALISFIYATI"].ToString();

                    string PSFiyati = dt.Rows[i]["SATISFIYATI"].ToString();
                    string TaksitliPS = dt.Rows[i]["SATISFIYATI2"].ToString();
                    if(TaksitliPS=="")
                       TaksitliPS = PSFiyati;

                    string KDV = dt.Rows[i]["KDVORANI"].ToString();
                    if (KDV.Contains("18")) KDV = "18";
                    else if (KDV.Contains("8")) KDV = "8";
                    else if (KDV.Contains("0")) KDV = "0";
                    else KDV = "18";

                    #region stok grubu ve alt grubu ekle
                    string STOKGRUBU = dt.Rows[i]["STOKGRUBU"].ToString();
                    string fkStokGrup = "0";
                    DataTable dtSg = DB.GetData("select * from StokGruplari with(nolock) where StokGrup='"+STOKGRUBU+"'");
                    if (dtSg.Rows.Count == 0)
                    {
                        fkStokGrup = DB.ExecuteScalarSQL("insert into StokGruplari (StokGrup,Aktif) values('" + STOKGRUBU + "',1) select IDENT_CURRENT('StokGruplari')");                        
                    }
                    else
                        fkStokGrup = dtSg.Rows[0][0].ToString();

                    #endregion

                    #region birimi
                    string BirimAdi = dt.Rows[i]["BIRIMI"].ToString();
                    string fkBirimler = "0";
                    DataTable dtbirim = DB.GetData("select * from Birimler with(nolock) where BirimAdi='" + BirimAdi + "'");
                    if (dtbirim.Rows.Count == 0)
                    {
                        fkBirimler = DB.ExecuteScalarSQL("insert into Birimler (BirimAdi,Aktif) values('" + BirimAdi + "',1) select IDENT_CURRENT('Birimler')");
                    }
                    else
                        fkBirimler = dtbirim.Rows[0][0].ToString();

                    #endregion

                    #region Marka ekle
                    string MARKA = dt.Rows[i]["MARKA"].ToString();
                    string fkMarka = "0";
                    DataTable dtMarka = DB.GetData("select * from Markalar with(nolock) where Marka='" + MARKA + "'");
                    if (dtMarka.Rows.Count == 0)
                    {
                        fkMarka = DB.ExecuteScalarSQL("insert into Markalar (Marka) values('" + MARKA + "') select IDENT_CURRENT('Markalar')");
                    }
                    else
                        fkMarka = dtMarka.Rows[0][0].ToString();
                    #endregion

                    #region alt grubu ekle
                    string fkStokAltGrup = "0";
                    //DataTable dtSag = DB.GetData("select * from StokAltGruplari with(nolock) where StokAltGrup='" + MARKA + "' and fkStokGruplari=" +
                    //   fkStokGrup);
                    //if (dtSag.Rows.Count == 0)
                    //{
                    //    fkStokAltGrup = DB.ExecuteScalarSQL("insert into StokAltGruplari (fkStokGruplari,StokAltGrup,Aktif) values(" + fkStokGrup +
                    //        ",'" + MARKA + "',1) select IDENT_CURRENT('StokAltGruplari')");
                    //}
                    //else
                    //    fkStokAltGrup = dtSag.Rows[0][0].ToString();

                    #endregion
                    //KDV S08
                    DataTable dtStokKarti = DB.GetData("select * from StokKarti with(nolock) where Barcode='" + Barcode + "'");
                    string pkStokKarti = "0";
                    if (dtStokKarti.Rows.Count == 0)
                    {
                        ArrayList list = new ArrayList();
                        list.Add(new SqlParameter("@StokKod", StokKod));
                        list.Add(new SqlParameter("@fkStokAltGruplari", fkStokAltGrup));
                        list.Add(new SqlParameter("@Barcode", Barcode));
                        list.Add(new SqlParameter("@fkStokGrup", fkStokGrup));
                        list.Add(new SqlParameter("@UreticiKodu", UreticiKodu));
                        list.Add(new SqlParameter("@Stokadi", Stokadi));
                        list.Add(new SqlParameter("@Stoktipi", BirimAdi));
                        list.Add(new SqlParameter("@Mevcut", "0"));//Miktar.Replace(",", ".")));
                        list.Add(new SqlParameter("@KdvOrani", KDV));
                        list.Add(new SqlParameter("@KdvOraniAlis", KDV));
                        list.Add(new SqlParameter("@AlisFiyati", BirimFiyati.Replace(",", ".")));
                        list.Add(new SqlParameter("@SatisFiyati", PSFiyati.Replace(",", ".")));
                        list.Add(new SqlParameter("@fkMarka", fkMarka));
                        list.Add(new SqlParameter("@fkBirimler", fkBirimler)); 

                        string sql = "INSERT INTO StokKarti (StokKod,fkStokAltGruplari,Barcode,fkStokGrup,Stokadi,Stoktipi,Mevcut,AlisFiyati,SatisFiyati,KdvOrani,Aktif,KritikMiktar,UreticiKodu,fkMarka,SatisAdedi,SonAlisTarihi,KdvOraniAlis,Aktarildi,satis_iskonto,alis_iskonto,fkBirimler)" +
                        " values(@StokKod,@fkStokAltGruplari,@Barcode,@fkStokGrup,@Stokadi,@Stoktipi,@Mevcut,@AlisFiyati,@SatisFiyati,@KdvOrani,1,0,@UreticiKodu,@fkMarka,1,getdate(),@KdvOraniAlis,1,0,0,@fkBirimler) select IDENT_CURRENT('StokKarti')";

                        try
                        {
                            pkStokKarti = DB.ExecuteScalarSQL(sql, list);
                            //stok grubu aç
                            
                            //DB.ExecuteSQL("update ");
                            b++;

                        }
                        catch (Exception exp)
                        {
                            pkStokKarti = "0";
                            h++;
                        }

                        AlisDetayEkle(Barcode);
                       
                    }
                    else
                    {
                        ayni++;
                        //Stok Kartı Var ise
                        pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();

                        AlisDetayEkle(Barcode);

                        DataTable dtMaxlisDetay = DB.GetData("select top 1 * from AlisDetay with(nolock) where fkStokKarti=" + pkStokKarti + " order by pkAlisDetay desc");
                        if (dtMaxlisDetay.Rows.Count > 0)
                        {
                            ArrayList list = new ArrayList();
                            list.Add(new SqlParameter("@SatisFiyati", PSFiyati.Replace(",", ".")));
                            list.Add(new SqlParameter("@SatisFiyati2", TaksitliPS.Replace(",", ".")));
                            list.Add(new SqlParameter("@SatisFiyati3", TaksitliPS.Replace(",", ".")));
                            list.Add(new SqlParameter("@Adet", Miktar.Replace(",", ".")));
                            list.Add(new SqlParameter("@AlisFiyati", BirimFiyati.Replace(",", ".")));

                            DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyati=@AlisFiyati,SatisFiyati=@SatisFiyati,SatisFiyati2=@SatisFiyati2,SatisFiyati3=@SatisFiyati3,Adet=@Adet where pkAlisDetay=" + dtMaxlisDetay.Rows[0]["pkAlisDetay"].ToString(), list);
                        }
                    }
                    //TODO:SATIŞ FİYATLARI EKLE
                    DataTable dtSF = DB.GetData("Select * FROM SatisFiyatlariBaslik with(nolock) where Aktif=1");
                    for (int si = 0; si < dtSF.Rows.Count; si++)
                    {
                        string pkSatisFiyatlariBaslik = dtSF.Rows[si]["pkSatisFiyatlariBaslik"].ToString();
                        string tur = dtSF.Rows[si]["Tur"].ToString();

                        string sql = "";
                        if (tur == "1")//nakit ise
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + PSFiyati.Replace(",", ".") + "," +
                                    PSFiyati.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + PSFiyati.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    PSFiyati.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                        else if (tur == "2")//kredi kartı
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + TaksitliPS.Replace(",", ".") + "," +
                                    TaksitliPS.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + TaksitliPS.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    TaksitliPS.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                        else //diğerleri
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + TaksitliPS.Replace(",", ".") + "," +
                                    TaksitliPS.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + TaksitliPS.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    TaksitliPS.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                    }
                    HizliMiktar = 1;
                }
                //taksitli fiyatları güncell
                DB.ExecuteSQL(@"update AlisDetay set SatisFiyati2=sf.SatisFiyatiKdvli  from (
select SatisFiyatlari.SatisFiyatiKdvli,SatisFiyatlari.fkStokKarti from SatisFiyatlari left join SatisFiyatlariBaslik sfb on sfb.pkSatisFiyatlariBaslik=SatisFiyatlari.fkSatisFiyatlariBaslik where sfb.Tur=2) sf
where AlisDetay.fkAlislar=" + pkAlislar.Text + " and sf.fkStokKarti=AlisDetay.fkStokKarti");
                
                //alış kdv hariç hesaplar
                DB.ExecuteSQL("update Stokkarti set AlisFiyatiKdvHaric=(AlisFiyati-(AlisFiyati*KdvOraniAlis)/(100+KdvOraniAlis))");

                MessageBox.Show("Aktarım Tamamlandı \n \n Toplam=" + c.ToString() + "\n Aktarılan=" + b.ToString() + "\n Hatalı=" + h.ToString() + "\n Aynı Barkodlu=" + ayni.ToString());
                yesilisikyeni();
                // gridControl1.DataSource = dt;
            }
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Yeni\v.xls;Extended Properties=”Excel 8.0;HDR=Yes;IMEX=1″
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
            
        }

        private void labelControl14_Click(object sender, EventArgs e)
        {
            frmDepoKarti depo = new frmDepoKarti();
            depo.ShowDialog();

            Depolar();
        }

        private void repositoryItemLookUpEdit3_EditValueChanged(object sender, EventArgs e)
        {
            if(gridView1.FocusedRowHandle<0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.ExecuteSQL("update AlisDetay set fkDepolar=" + value+" where pkAlisDetay=" + dr["pkAlisDetay"].ToString());
        }

        private void lueDepolar_EditValueChanged(object sender, EventArgs e)
        {
            if (lueDepolar.Tag.ToString() == "1")
            {
                string s = formislemleri.MesajBox("Tüm Ürünlerin Depo Girişini Değişecektir. Eminmisiniz?", Degerler.mesajbaslik, 3, 2);
                if (s == "0")
                {
                    lueDepolar.Tag = "0";
                    lueDepolar.EditValue = lueDepolar.OldEditValue;
                    lueDepolar.Tag = "1";
                    return;
                }
                DB.ExecuteSQL("update AlisDetay set fkDepolar=" + lueDepolar.EditValue.ToString() + " where fkAlislar=" + pkAlislar.Text);
                yesilisikyeni();
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
             string fkStokKarti = dr["fkStokKarti"].ToString();

            frmStokDepoDevir StokDevir = new frmStokDepoDevir();
            StokDevir.fkStokKarti.Tag = fkStokKarti;
            StokDevir.ShowDialog();
        }

        bool sescal_acikmi = Degerler.Uruneklendisescal;
        private void tSMiSiparisFormu_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("Dış Veri Alırken Excel 2003 formatında ve ilk Sayfa Adı 0 Olmalı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                DataTable dtayar = DB.GetData("select * from  ayarlar with(nolock) where Ayar20='excelyol'");
                if (dtayar.Rows.Count > 0)
                    openFileDialog1.InitialDirectory = dtayar.Rows[0]["Ayar50"].ToString();
                else
                    openFileDialog1.InitialDirectory = Degerler.YedekYolu;

                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName == "") return;

                Degerler.Uruneklendisescal = false;

                OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OleDb.4.0;Data Source = " +
                    openFileDialog1.FileName + " ; Extended Properties = Excel 8.0");//" ; Extended Properties = Excel 8.0");
                //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
                OleDbCommand cmd = new OleDbCommand("select * from [0$]", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
                //MessageBox.Show((System.Exception)(con.ServerVersion).Message);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Barcode = dt.Rows[i][0].ToString();//SERİ NO
                    if (Barcode == "") continue;
                    string projeadi = dt.Rows[i][1].ToString();//Proje Adı

                    string projeno = dt.Rows[i][2].ToString();//Proje no
                    string uruntipi = dt.Rows[i][3].ToString();// ürün tipi
                    string Stokadi = dt.Rows[i][4].ToString();//Parça Adı
                    string RenkKodu = dt.Rows[i][5].ToString();//İç Renk 
                    string BedenKodu = dt.Rows[i][6].ToString();//Dış Renk
                    string aciklama = dt.Rows[i][7].ToString();//teslim tarihi
                    string teslimtarihi = dt.Rows[i][8].ToString();//teslim tarihi
                    string Miktar = "1";
                    string KDV = "18";

                    DataTable dtStokKarti = DB.GetData("select * from StokKarti with(nolock) where Barcode='" + Barcode + "'");
                    string pkStokKarti = "0";
                    if (dtStokKarti.Rows.Count == 0)
                    {
                        ArrayList list = new ArrayList();
                        list.Add(new SqlParameter("@StokKod", Barcode));
                        list.Add(new SqlParameter("@UreticiKodu", projeno));
                        list.Add(new SqlParameter("@Barcode", Barcode));
                        Stokadi = projeno + " & " + Stokadi;
                        list.Add(new SqlParameter("@Stokadi", Stokadi));

                        string fkStokGrup = "0";
                        if (projeadi.Length > 0)
                        {
                            DataTable dtGrup = DB.GetData("select pkStokGrup from StokGruplari with(nolock) where StokGrup='" + projeadi + "'");
                            if (dtGrup.Rows.Count == 0)
                            {
                                fkStokGrup = DB.ExecuteScalarSQL("INSERT INTO StokGruplari (StokGrup,Aktif) VALUES('" + projeadi + "',1) select IDENT_CURRENT('StokGruplari')");
                                list.Add(new SqlParameter("@fkStokGrup", fkStokGrup));
                            }
                            else
                                list.Add(new SqlParameter("@fkStokGrup", dtGrup.Rows[0]["pkStokGrup"].ToString()));
                        }
                        else
                            list.Add(new SqlParameter("@fkStokGrup", fkStokGrup)); //dtStokKarti.Rows[0]["fkMarka"].ToString()));


                        string fkMarka = "0";
                        if (uruntipi.Length > 0)
                        {
                            DataTable dtMarka = DB.GetData("select pkMarka from Markalar with(nolock) where Marka='" + uruntipi + "'");
                            if (dtMarka.Rows.Count == 0)
                            {
                                fkMarka = DB.ExecuteScalarSQL("INSERT INTO Markalar (Marka) VALUES('" + uruntipi + "') select IDENT_CURRENT('Markalar')");
                                list.Add(new SqlParameter("@fkMarka", fkMarka));
                            }
                            else
                                list.Add(new SqlParameter("@fkMarka", dtMarka.Rows[0]["pkMarka"].ToString()));
                        }
                        else
                            list.Add(new SqlParameter("@fkMarka",fkMarka)); //dtStokKarti.Rows[0]["fkMarka"].ToString()));

                        string fkRenkGrupKodu = "0";
                        if (RenkKodu.Length > 0)
                        {
                            DataTable dtRenk = DB.GetData("select pkRenkGrupKodu from RenkGrupKodu with(nolock) where Aciklama='" + RenkKodu + "'");
                            if (dtRenk.Rows.Count == 0)
                            {
                                fkRenkGrupKodu = DB.ExecuteScalarSQL("INSERT INTO RenkGrupKodu (Aciklama) VALUES('" + RenkKodu + "') select IDENT_CURRENT('RenkGrupKodu')");
                                list.Add(new SqlParameter("@fkRenkGrupKodu", fkRenkGrupKodu));
                            }
                            else
                                list.Add(new SqlParameter("@fkRenkGrupKodu", dtRenk.Rows[0]["pkRenkGrupKodu"].ToString()));
                        }
                        else
                            list.Add(new SqlParameter("@fkRenkGrupKodu", fkRenkGrupKodu));
                        //
                        string fkBedenGrupKodu = "0";
                        if (BedenKodu.Length > 0)
                        {
                            DataTable dtBeden = DB.GetData("select pkBedenGrupKodu from BedenGrupKodu with(nolock) where Aciklama='" + BedenKodu + "'");
                            if (dtBeden.Rows.Count == 0)
                            {
                                fkRenkGrupKodu = DB.ExecuteScalarSQL("INSERT INTO BedenGrupKodu (Aciklama) VALUES('" + BedenKodu + "') select IDENT_CURRENT('BedenGrupKodu')");
                                list.Add(new SqlParameter("@fkBedenGrupKodu", fkRenkGrupKodu));
                            }
                            else
                                list.Add(new SqlParameter("@fkBedenGrupKodu", dtBeden.Rows[0]["pkBedenGrupKodu"].ToString()));
                        }
                        else
                            list.Add(new SqlParameter("@fkBedenGrupKodu", fkBedenGrupKodu));

                        list.Add(new SqlParameter("@Stoktipi", "ADET"));
                        list.Add(new SqlParameter("@Mevcut", "0"));//HizliMiktar.ToString().Replace(",", ".")));
                        list.Add(new SqlParameter("@KdvOrani", KDV));
                        list.Add(new SqlParameter("@KdvOraniAlis", KDV));
                        list.Add(new SqlParameter("@AlisFiyati", "0"));
                        list.Add(new SqlParameter("@SatisFiyati", "0"));
                        list.Add(new SqlParameter("@EtiketAciklama", aciklama+"-"+teslimtarihi));

                        string sql = "INSERT INTO StokKarti (StokKod,Barcode,fkStokGrup,fkMarka,fkRenkGrupKodu,fkBedenGrupKodu,Stokadi,Stoktipi,Mevcut,AlisFiyati,SatisFiyati,KdvOrani,Aktif,KritikMiktar,UreticiKodu,SatisAdedi,SonAlisTarihi,KdvOraniAlis,EtiketAciklama)" +
                        " values(@StokKod,@Barcode,@fkStokGrup,@fkMarka,@fkRenkGrupKodu,@fkBedenGrupKodu,@Stokadi,@Stoktipi,@Mevcut,@AlisFiyati,@SatisFiyati,@KdvOrani,1,0,@UreticiKodu,1,getdate(),@KdvOraniAlis,@EtiketAciklama) select IDENT_CURRENT('StokKarti')";

                        try
                        {
                            pkStokKarti = DB.ExecuteScalarSQL(sql, list);
                        }
                        catch (Exception exp)
                        {
                            pkStokKarti = "0";
                        }

                        AlisDetayEkle(Barcode);
                    }
                    else
                    {
                        AlisDetayEkle(Barcode);
                    }

                    #region TODO:SATIŞ FİYATLARI EKLE
                    DataTable dtSF = DB.GetData("Select * FROM SatisFiyatlariBaslik with(nolock) where Aktif=1");
                    for (int si = 0; si < dtSF.Rows.Count; si++)
                    {
                        string pkSatisFiyatlariBaslik = dtSF.Rows[si]["pkSatisFiyatlariBaslik"].ToString();
                        string tur = dtSF.Rows[si]["Tur"].ToString();

                        string sql = "";
                        if (tur == "1")//nakit ise
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + ",0,0,1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=0,SatisFiyatiKdvli=0" +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                        else if (tur == "2")//kredi kartı
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + ",0,0,1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=0,SatisFiyatiKdvli=0"+
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                        else //diğerleri
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + ",0,0,1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=0,SatisFiyatiKdvli=0 WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                    }
                    #endregion

                    HizliMiktar = 1;
                }
                //taksitli fiyatları güncell
                DB.ExecuteSQL(@"update AlisDetay set SatisFiyati2=sf.SatisFiyatiKdvli  from (
                select SatisFiyatlari.SatisFiyatiKdvli,SatisFiyatlari.fkStokKarti from SatisFiyatlari left join SatisFiyatlariBaslik sfb on sfb.pkSatisFiyatlariBaslik=SatisFiyatlari.fkSatisFiyatlariBaslik where sfb.Tur=2) sf
                where AlisDetay.fkAlislar=" + pkAlislar.Text + " and sf.fkStokKarti=AlisDetay.fkStokKarti");

                yesilisikyeni();
                Degerler.Uruneklendisescal = sescal_acikmi;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            deAlisTarihi.Tag = "1";
            deAlisTarihi.DateTime = DateTime.Now;
            deAlisTarihi.Tag = "0";
        }

        private void deAlisTarihi_EditValueChanged(object sender, EventArgs e)
        {
            if (deAlisTarihi.Tag.ToString() == "1")
            {
                DB.ExecuteSQL("update Alislar set guncellemetarihi='" + deAlisTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:ss") +
                    "' where pkAlislar=" + pkAlislar.Text);
            }
        }

        private void deAlisTarihi_Enter(object sender, EventArgs e)
        {
            deAlisTarihi.Tag = "1";
        }

        private void deAlisTarihi_Leave(object sender, EventArgs e)
        {
            deAlisTarihi.Tag = "0";
        }
        void ilkYuklemeYenile()
        {
            pkAlislar.Text = "0";
            Satis1Toplam.Tag = "0";
            Satis2Toplam.Tag = "0";
            Satis3Toplam.Tag = "0";
            SatisDuzenKapat();
            timer1.Enabled = true;
            yesilisikyeni();
        }
        private void sbYenile_Click(object sender, EventArgs e)
        {
            ilkYuklemeYenile();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void labelControl3_Click(object sender, EventArgs e)
        {
            deFaturaTarihi.DateTime = DateTime.Today;
        }

        private void FaturaNo_Enter(object sender, EventArgs e)
        {
            FaturaNo.Tag = "1";
        }

        private void lueDepolar_Leave(object sender, EventArgs e)
        {
            lueDepolar.Tag = "0";
        }

        private void lueDepolar_Enter(object sender, EventArgs e)
        {
            lueDepolar.Tag = "1";
        }

        private void excelden_satis_gecmis_sablonu_yukle()
        {
            Excel.Application xlOrn = new Microsoft.Office.Interop.Excel.Application();
            if (xlOrn == null)
            {
                MessageBox.Show("Excel yüklü değil!!");
                //return;
            }
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();


            //DevExpress.XtraEditors.XtraMessageBox.Show("Dış Veri Alırken Excel 2003 formatında ve ilk Sayfa Adı Sayfa1 Olmalı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                DataTable dtayar = DB.GetData("select * from  ayarlar with(nolock) where Ayar20='excelyol'");
                if (dtayar.Rows.Count > 0)
                    openFileDialog1.InitialDirectory = dtayar.Rows[0]["Ayar50"].ToString();
                else
                    openFileDialog1.InitialDirectory = Degerler.YedekYolu;

                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();

                if (openFileDialog1.FileName == "") return;


                //OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + openFileDialog1.FileName + "; Extended Properties='Excel 12.0 Xml;HDR=YES'"); //excel_dosya.xlsx kısmını kendi excel dosyanızın adıyla değiştirin.
                ////DataTable tablo = new DataTable();
                //con.Open();
                //// OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OleDb.4.0;Data Source = " +
                ////   openFileDialog1.FileName + " ; Extended Properties = Excel 8.0;");
                ////excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
                //OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
                //OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
                ////MessageBox.Show((System.Exception)(con.ServerVersion).Message);
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);//Microsoft.CSharp dll 4.0 eklendi
                int c = xlWorkSheet.Rows.Count;
                int h = 0, b = 0, ayni = 0;
                for (int i = 1; i < c; i++)
                {
                    if(i==1) continue;
                    // DataTable dt = new DataTable();
                    //try
                    //{
                    //    // con.Open();

                    //}
                    //catch (OleDbException exp)
                    //{
                    //    MessageBox.Show("hata : " + exp.Message);
                    //    //throw;
                    //    return;
                    //}
                    //da.Fill(dt);

                    //for (int i = 0; i < c; i++)
                    //{
                    string Barcode = "";
                    if (xlWorkSheet.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 == null) break;

                    Barcode=xlWorkSheet.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();

                    string stokadi = xlWorkSheet.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString();
                    string Miktar = "1";
                    string sMiktar = "";
                    if(xlWorkSheet.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2!=null)
                        sMiktar= xlWorkSheet.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();

                    string salisfiyati = "";
                    if(xlWorkSheet.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2!=null)
                        salisfiyati = xlWorkSheet.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString();

                    string ssatisfiyati = "";
                    if(xlWorkSheet.get_Range("E" + i.ToString(), "E" + i.ToString()).Value2!=null)
                        ssatisfiyati = xlWorkSheet.get_Range("E" + i.ToString(), "E" + i.ToString()).Value2.ToString();

                    string sstokgrubu = "";
                    if(xlWorkSheet.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2!=null)
                        sstokgrubu= xlWorkSheet.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2.ToString();

                    string smarka = "";
                    if (xlWorkSheet.get_Range("G" + i.ToString(), "G" + i.ToString()).Value2!=null)
                        smarka = xlWorkSheet.get_Range("G" + i.ToString(), "G" + i.ToString()).Value2.ToString();

                    string skdvorani = "";
                    if(xlWorkSheet.get_Range("H" + i.ToString(), "H" + i.ToString()).Value2!=null)
                        skdvorani = xlWorkSheet.get_Range("H" + i.ToString(), "H" + i.ToString()).Value2.ToString();
                    string sstokkodu = "";
                    if(xlWorkSheet.get_Range("I" + i.ToString(), "I" + i.ToString()).Value2!=null)
                        sstokkodu = xlWorkSheet.get_Range("I" + i.ToString(), "I" + i.ToString()).Value2.ToString();

                    string staksitfiyati = "";
                    if(xlWorkSheet.get_Range("J" + i.ToString(), "J" + i.ToString()).Value2!=null)
                        staksitfiyati = xlWorkSheet.get_Range("J" + i.ToString(), "J" + i.ToString()).Value2.ToString();

                    string StokKod = sstokkodu; //dt.Rows[i]["STOKKODU"].ToString();
                    //string Barcode = dt.Rows[i]["BARKOD"].ToString();

                    if (Barcode == "") continue;

                    if (StokKod == "") StokKod = Barcode;

                    //string Stokadi = dt.Rows[i]["STOKADI"].ToString();
                    //string RenkKodu = dt.Rows[i]["RenkKodu"].ToString();
                    //string BedenKodu = dt.Rows[i]["BedenKodu"].ToString();
                    

                    if (sMiktar != "")
                    {
                        Miktar = sMiktar;//dt.Rows[i]["MEVCUT"].ToString();
                        HizliMiktar = decimal.Parse(Miktar);
                    }
                    string BirimFiyati = "0.001";
                    if (salisfiyati != "")
                        BirimFiyati = salisfiyati;

                    string PSFiyati = ssatisfiyati;
                    string TaksitliPS = staksitfiyati;
                    if (TaksitliPS == "")
                        TaksitliPS = PSFiyati;

                    string KDV = skdvorani;//dt.Rows[i]["KDVORANI"].ToString();
                    if (KDV.Contains("18")) KDV = "18";
                    else if (KDV.Contains("8")) KDV = "8";
                    else if (KDV.Contains("0")) KDV = "0";
                    else KDV = "18";

                    #region stok grubu ve alt grubu ekle
                    string STOKGRUBU = sstokgrubu;//dt.Rows[i]["STOKGRUBU"].ToString();
                    string fkStokGrup = "0";
                    DataTable dtSg = DB.GetData("select * from StokGruplari with(nolock) where StokGrup='" + STOKGRUBU + "'");
                    if (dtSg.Rows.Count == 0)
                    {
                        fkStokGrup = DB.ExecuteScalarSQL("insert into StokGruplari (StokGrup,Aktif) values('" + STOKGRUBU + "',1) select IDENT_CURRENT('StokGruplari')");
                    }
                    else
                        fkStokGrup = dtSg.Rows[0][0].ToString();

                    #endregion

                    #region Marka ekle
                    string MARKA = smarka;//dt.Rows[i]["MARKA"].ToString();
                    string fkMarka = "0";
                    DataTable dtMarka = DB.GetData("select * from Markalar with(nolock) where Marka='" + MARKA + "'");
                    if (dtMarka.Rows.Count == 0)
                    {
                        fkMarka = DB.ExecuteScalarSQL("insert into Markalar (Marka) values('" + MARKA + "') select IDENT_CURRENT('Markalar')");
                    }
                    else
                        fkMarka = dtMarka.Rows[0][0].ToString();
                    #endregion

                    #region alt grubu ekle
                    string fkStokAltGrup = "0";
                    DataTable dtSag = DB.GetData("select * from StokAltGruplari with(nolock) where StokAltGrup='" + MARKA + "' and fkStokGruplari=" +
                       fkStokGrup);
                    if (dtSag.Rows.Count == 0)
                    {
                        fkStokAltGrup = DB.ExecuteScalarSQL("insert into StokAltGruplari (fkStokGruplari,StokAltGrup,Aktif) values(" + fkStokGrup +
                            ",'" + MARKA + "',1) select IDENT_CURRENT('StokAltGruplari')");
                    }
                    else
                        fkStokAltGrup = dtSag.Rows[0][0].ToString();

                    #endregion
                    //KDV S08
                    DataTable dtStokKarti = DB.GetData("select * from StokKarti with(nolock) where Barcode='" + Barcode + "'");
                    string pkStokKarti = "0";
                    if (dtStokKarti.Rows.Count == 0)
                    {
                        ArrayList list = new ArrayList();
                        list.Add(new SqlParameter("@StokKod", StokKod));
                        list.Add(new SqlParameter("@fkStokAltGruplari", fkStokAltGrup));
                        list.Add(new SqlParameter("@Barcode", Barcode));
                        list.Add(new SqlParameter("@fkStokGrup", fkStokGrup));
                        list.Add(new SqlParameter("@UreticiKodu", Barcode));
                        list.Add(new SqlParameter("@Stokadi", stokadi));
                        list.Add(new SqlParameter("@Stoktipi", "ADET"));
                        list.Add(new SqlParameter("@Mevcut", "0"));//Miktar.Replace(",", ".")));
                        list.Add(new SqlParameter("@KdvOrani", KDV));
                        list.Add(new SqlParameter("@KdvOraniAlis", KDV));
                        list.Add(new SqlParameter("@AlisFiyati", BirimFiyati.Replace(",", ".")));
                        list.Add(new SqlParameter("@SatisFiyati", PSFiyati.Replace(",", ".")));
                        list.Add(new SqlParameter("@fkMarka", fkMarka));

                        string sql = "INSERT INTO StokKarti (StokKod,fkStokAltGruplari,Barcode,fkStokGrup,Stokadi,Stoktipi,Mevcut,AlisFiyati,SatisFiyati,KdvOrani,Aktif,KritikMiktar,UreticiKodu,fkMarka,SatisAdedi,SonAlisTarihi,KdvOraniAlis,Aktarildi,satis_iskonto,alis_iskonto)" +
                        " values(@StokKod,@fkStokAltGruplari,@Barcode,@fkStokGrup,@Stokadi,@Stoktipi,@Mevcut,@AlisFiyati,@SatisFiyati,@KdvOrani,1,0,@UreticiKodu,@fkMarka,1,getdate(),@KdvOraniAlis,1,0,0) select IDENT_CURRENT('StokKarti')";

                        try
                        {
                            pkStokKarti = DB.ExecuteScalarSQL(sql, list);
                            //stok grubu aç

                            //DB.ExecuteSQL("update ");
                            b++;

                        }
                        catch (Exception exp)
                        {
                            pkStokKarti = "0";
                            h++;
                        }

                        AlisDetayEkle(Barcode);

                    }
                    else
                    {
                        ayni++;
                        //Stok Kartı Var ise
                        pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();

                        AlisDetayEkle(Barcode);

                        DataTable dtMaxlisDetay = DB.GetData("select top 1 * from AlisDetay with(nolock) where fkStokKarti=" + pkStokKarti + " order by pkAlisDetay desc");
                        if (dtMaxlisDetay.Rows.Count > 0)
                        {
                            ArrayList list = new ArrayList();
                            list.Add(new SqlParameter("@SatisFiyati", PSFiyati.Replace(",", ".")));
                            list.Add(new SqlParameter("@SatisFiyati2", TaksitliPS.Replace(",", ".")));
                            list.Add(new SqlParameter("@SatisFiyati3", TaksitliPS.Replace(",", ".")));
                            list.Add(new SqlParameter("@Adet", Miktar.Replace(",", ".")));
                            list.Add(new SqlParameter("@AlisFiyati", BirimFiyati.Replace(",", ".")));

                            DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyati=@AlisFiyati,SatisFiyati=@SatisFiyati,SatisFiyati2=@SatisFiyati2,SatisFiyati3=@SatisFiyati3,Adet=@Adet where pkAlisDetay=" + dtMaxlisDetay.Rows[0]["pkAlisDetay"].ToString(), list);
                        }
                    }
                    //TODO:SATIŞ FİYATLARI EKLE
                    DataTable dtSF = DB.GetData("Select * FROM SatisFiyatlariBaslik with(nolock) where Aktif=1");
                    for (int si = 0; si < dtSF.Rows.Count; si++)
                    {
                        string pkSatisFiyatlariBaslik = dtSF.Rows[si]["pkSatisFiyatlariBaslik"].ToString();
                        string tur = dtSF.Rows[si]["Tur"].ToString();

                        string sql = "";
                        if (tur == "1")//nakit ise
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + PSFiyati.Replace(",", ".") + "," +
                                    PSFiyati.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + PSFiyati.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    PSFiyati.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                        else if (tur == "2")//kredi kartı
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + TaksitliPS.Replace(",", ".") + "," +
                                    TaksitliPS.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + TaksitliPS.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    TaksitliPS.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                        else //diğerleri
                        {
                            if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" +
                                pkStokKarti + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                                sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                                    " VALUES(" + pkStokKarti + "," + pkSatisFiyatlariBaslik + "," + TaksitliPS.Replace(",", ".") + "," +
                                    TaksitliPS.Replace(",", ".") + ",1)";
                            else
                                sql = "UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvsiz=" + TaksitliPS.Replace(",", ".") + ",SatisFiyatiKdvli=" +
                                    TaksitliPS.Replace(",", ".") +
                                     " WHERE fkStokKarti=" + pkStokKarti + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                            DB.ExecuteSQL(sql);
                        }
                    }
                    HizliMiktar = 1;
                }

                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                //taksitli fiyatları güncell
                DB.ExecuteSQL(@"update AlisDetay set SatisFiyati2=sf.SatisFiyatiKdvli  from (
select SatisFiyatlari.SatisFiyatiKdvli,SatisFiyatlari.fkStokKarti from SatisFiyatlari left join SatisFiyatlariBaslik sfb on sfb.pkSatisFiyatlariBaslik=SatisFiyatlari.fkSatisFiyatlariBaslik where sfb.Tur=2) sf
where AlisDetay.fkAlislar=" + pkAlislar.Text + " and sf.fkStokKarti=AlisDetay.fkStokKarti");

                //alış kdv hariç hesaplar
                DB.ExecuteSQL("update Stokkarti set AlisFiyatiKdvHaric=(AlisFiyati-(AlisFiyati*KdvOraniAlis)/(100+KdvOraniAlis))");

                MessageBox.Show("Aktarım Tamamlandı \n \n Toplam=" + c.ToString() + "\n Aktarılan=" + b.ToString() + "\n Hatalı=" + h.ToString() + "\n Aynı Barkodlu=" + ayni.ToString());
                yesilisikyeni();
                // gridControl1.DataSource = dt;
            }
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Yeni\v.xls;Extended Properties=”Excel 8.0;HDR=Yes;IMEX=1″
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
        }
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            excelden_satis_gecmis_sablonu_yukle();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            try
            {
                Models.Bilgiler bilgi = new Models.Bilgiler
                {
                    AdSoyad = "adem",//txtAdSoyad.Text,
                    DogumTarihi = DateTime.Now, //dtDogumTarihi.Value,
                    ID = 1,//Convert.ToInt32(txtID.Text),
                    Maas = 2500// Convert.ToDecimal(txtMaas.Text)
                };


                this.Text = JsonIslemler.JsonYaz(bilgi) ? "Json Yazıldı" : "Hata";
            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            DataTable dtayar = DB.GetData("select * from  ayarlar with(nolock) where Ayar20='excelyol'");
            if (dtayar.Rows.Count > 0)
                openFileDialog1.InitialDirectory = dtayar.Rows[0]["Ayar50"].ToString();
            else
                openFileDialog1.InitialDirectory = Degerler.YedekYolu;

            openFileDialog1.Title = "Lütfen Dosya Seçiniz";
            openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
            openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName == "") return;


            Models.Bilgiler bilgiler = JsonIslemler.JsonOku(openFileDialog1.FileName);
            MessageBox.Show(bilgiler.ID.ToString());
            MessageBox.Show(bilgiler.AdSoyad);
            MessageBox.Show(bilgiler.Maas.ToString());
            MessageBox.Show(bilgiler.DogumTarihi.ToString());
        }

        private void ucAlisTrans_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
                yesilisikyeni();
            // yesilyak();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\AlisFaturasi" + pkAlislar.Text + ".Xls";
            gridView1.ExportToXls(dosyaadi);
            System.Diagnostics.Process.Start(dosyaadi);
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            int fkAlisDetay = int.Parse(dr["pkAlisDetay"].ToString());
            int fkStokKarti = int.Parse(dr["fkStokKarti"].ToString());

            frmStokKartiRenkBeden renkBeden = new frmStokKartiRenkBeden(fkAlisDetay, fkStokKarti);
            renkBeden.ShowDialog();
        }

        private void repositoryItemButtonEdit2_Enter(object sender, EventArgs e)
        {
            repositoryItemButtonEdit2.Tag = "1";
        }

        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            yesilisikyeni();
        }
    }
}

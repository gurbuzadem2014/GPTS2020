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
    public partial class frmStokTopluDegisiklik : DevExpress.XtraEditors.XtraForm
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis="";//AccessibleDescription
        decimal HizliMiktar = 1;

      public frmStokTopluDegisiklik()
      {
        InitializeComponent();
        DB.PkFirma = 1;
        this.Width = Screen.PrimaryScreen.WorkingArea.Width - 150;
        this.Height = Screen.PrimaryScreen.WorkingArea.Height - 5;
      }
     
      void vBeklemeListesi()
        {
            vGridControl1.DataSource = DB.GetData("select pkStokFiyatGuncelle from StokFiyatGuncelle with(nolock) where Siparis=0");
        }

      void Fiyatlarigetir()
        {
            lueFiyatlar.Properties.DataSource = DB.GetData(@"select * from SatisFiyatlariBaslik with(nolock)
            where Aktif=1 order by pkSatisFiyatlariBaslik");
            lueFiyatlar.EditValue = 1;
        }

      void Gruplar()
        {
            lueGruplar.Properties.DataSource = DB.GetData("SELECT * FROM StokGruplari with(nolock)");
        }

      private void ucAnaEkran_Load(object sender, EventArgs e)
      {
          HizliSatisTablariolustur();

          Yetkiler();

          timer1.Enabled = true;

          vBeklemeListesi();

          Fiyatlarigetir();

          FisListesi();

          //KullaniciYetkileri();
      }

        void KullaniciYetkileri()
        {
            DataTable dt =
            DB.GetData(@"select m.pkModuller,m.Kod,m.ModulAdi,m.ana_id,m.durumu,m.Kod,my.pkModullerYetki,
            my.Yetki from Moduller m with(nolock)
            left join ModullerYetki my with(nolock) on my.Kod=m.Kod
            where m.Kod in('2.1','2.1') and my.fkKullanicilar=" + DB.fkKullanicilar);
            foreach (DataRow dr in dt.Rows)
            {
                string kod = dr["Kod"].ToString();
                string Yetki = dr["Yetki"].ToString();

                if (kod == "1.2" && Yetki == "False")
                    simpleButton37.Enabled = false;
                else if (kod == "1.3" && Yetki == "False")
                    simpleButton37.Enabled = false;

            }
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
          string sql = @"SELECT  YetkiAlanlari.Yetki, YetkiAlanlari.Sayi, Parametreler.Aciklama10, YetkiAlanlari.BalonGoster
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

      void HizliSatisTablariolustur()
      {
          xtraTabControl3.TabPages.Clear();
          DataTable dt = DB.GetData("select * from HizliGruplar with(nolock) where Aktif=1 order by SiraNo");
          for (int i = 0; i < dt.Rows.Count; i++)
          {
              XtraTabPage xtab = new XtraTabPage();
              xtab.Text = dt.Rows[i]["HizliGrupAdi"].ToString();
              xtab.Tag = dt.Rows[i]["pkHizliGruplar"].ToString();
              xtraTabControl3.TabPages.Add(xtab);
              xtab.PageVisible = true;
          }
          xtraTabControl3.SelectedTabPageIndex = 0;
      }

        int x = 0;
        int y = 0;
        int p = 1;
        private void ButtonClick(object sender, EventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
            {
                StokFiyatGuncelleEkle(((SimpleButton)sender).Tag.ToString());
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle
            
            string pkSatislar=pkSatisBarkod.Text;
            int sonuc = 0;
            sonuc = DB.ExecuteSQL("DELETE FROM StokFiyatGuncelleDetay WHERE fkStokFiyatGuncelle=" + pkSatislar.ToString());
            sonuc = DB.ExecuteSQL("DELETE FROM StokFiyatGuncelle WHERE pkStokFiyatGuncelle=" + pkSatislar.ToString());
               if (sonuc != 1)
                MessageBox.Show("Hata Oluştur");
               pkSatisBarkod.Text = "0";
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
            DB.ExecuteSQL("DELETE FROM StokFiyatGuncelleDetay WHERE pkStokFiyatGuncelleDetay=" + dr["pkStokFiyatGuncelleDetay"].ToString());
            gridView1.DeleteSelectedRows();
            if (gridView1.DataRowCount == 0)
            {
                DB.ExecuteSQL("DELETE FROM StokFiyatGuncelle WHERE pkStokFiyatGuncelle=" + pkSatisBarkod.Text);
            }
            yesilisikyeni();
        }

        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
         if (e.Control && e.Shift && gridView1.DataRowCount > 0)
         {
             //üst satırı kopyala
                 DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);
                 StokFiyatGuncelleEkle(dr["Barcode"].ToString());
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
                StokFiyatGuncelleEkle(kod);
                yesilisikyeni();
            }
        }

        void urunaraekle()
        {
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "0";
            StokAra.ShowDialog();
            if (StokAra.TopMost == false) 
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {

                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    StokFiyatGuncelleEkle(dr["Barcode"].ToString());
                }
            }
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

        private void urunekle(string barkod)
        {
            if (barkod == "") return;
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
            StokFiyatGuncelleEkle(barkod);
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
            SatisDetayGetir(pkSatisBarkod.Text);
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();
            SendKeys.Send("{ENTER}");
        }

        private bool SatisVar()
        {

            if (gridView1.DataRowCount == 0)
            {
                Showmessage("Önce Fiyat Listesi Yapınız!", "K");
                return false;
            }
             return true;
        }

        void satiskaydet(bool yazdir, bool odemekaydedildi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkStokFiyatGuncelle", pkSatisBarkod.Text));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@fkSatisDurumu", "2"));//lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@OdemeSekli", "Nakit"));//cbOdemeSekli.EditValue.ToString()));
            string sonuc = DB.ExecuteSQL(@"UPDATE StokFiyatGuncelle SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,
fkSatisDurumu=@fkSatisDurumu,OdemeSekli=@OdemeSekli,GuncellemeTarihi=getdate() where pkStokFiyatGuncelle=@pkStokFiyatGuncelle", list);
            if (sonuc.Length>1 && sonuc.Substring(1, 1) == "H")
            {
                Showmessage("Hata Oluştur: " + sonuc ,"K");
                return;
            }
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE SatisFiyatlari SET SatisFiyatiKdvli=" + dr["NakitFiyat"].ToString().Replace(",", ".") +
                    " where fkStokKarti=" + dr["fkStokKarti"].ToString());
            }
            FisListesi();
        }

        void MevcutGuncelle()
        {
            //for (int s = 0; s < gridView1.DataRowCount; s++)
            //{
            //    DataRow dr = gridView1.GetDataRow(s);
            //    DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=Mevcut-"+dr["Adet"].ToString()+" where pkStokKarti="+
            //        dr["fkStokKarti"].ToString());
            //    //DB.ExecuteSQL("UPDATE AlisDetay SET KalanAdet=KalanAdet-"+);
            //}
        }

        void temizle()
        {
            btnAciklamaGirisi.ToolTip = "";
            cbislemTipi.SelectedIndex = 0;
            gridControl1.DataSource = null;
            cbOdemeSekli.SelectedIndex = 0;
            SatisFiyati.Value = 0;
        }

        void satiskaydet(bool timer, bool yazdir, bool odemekaydedildi)
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesajBox mesaj = new frmMesajBox(200);
                mesaj.label1.Text = "Önce Fiyat Listesi Yapınız!";
                mesaj.Show();
                return;
            }
            satiskaydet(yazdir, odemekaydedildi);
            FisListesi();
           // temizle(AcikSatisindex);
            yesilisikyeni();
        }

        void vkaydetyazdir_sil(string btn_kaydet_yazdir)
        {
            if (OnceSatisYapiniz() == false) return;
            if (btn_kaydet_yazdir == "kaydet")
                satiskaydet(true, false);
            //if (btn_kaydet_yazdir == "yazdir")
            else if (btn_kaydet_yazdir == "yazdir")
            {
                string pkSatislar = "0";
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
                    satiskaydet(true, false);
                    //FisYazdir_(false, pkSatislar, YaziciDosyasi, YaziciAdi);
                }
            }
            yesilisikyeni();
        }

        private void simpleButton37_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex != 6)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(cbislemTipi.Text +
                    " Kaydedilecek Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;
            }

            string sql = "";
            switch (xtraTabControl1.SelectedTabPageIndex)
            {
                //Satış Fiyatları
                case 0:
                    {
                        for (int i = 0; i < gridView1.DataRowCount; i++)
                        {
                            DataRow dr = gridView1.GetDataRow(i);
                            if (lueFiyatlar.EditValue.ToString() == "1")
                            {
                                //stok kartını güncelle
                                ArrayList list = new ArrayList();
                                list.Add(new SqlParameter("@fkStokKarti", dr["fkStokKarti"].ToString()));
                                list.Add(new SqlParameter("@SatisFiyati", SatisFiyati.Text.Replace(",", ".")));
                                sql = "update StokKarti set SatisFiyati=@SatisFiyati where pkStokKarti=@fkStokKarti";
                                DB.ExecuteSQL(sql, list);
                                
                                //satış Fiyatlarınıda güncelle
                                ArrayList list2 = new ArrayList();
                                list2.Add(new SqlParameter("@fkStokKarti", dr["fkStokKarti"].ToString()));
                                list2.Add(new SqlParameter("@fkSatisFiyatlariBaslik", lueFiyatlar.EditValue.ToString()));
                                list2.Add(new SqlParameter("@SatisFiyatiKdvli", SatisFiyati.Text.Replace(",", ".")));

                                sql = "update SatisFiyatlari set Aktif=1,SatisFiyatiKdvli=@SatisFiyatiKdvli where fkStokKarti=@fkStokKarti and fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik";
                                
                                DB.ExecuteSQL(sql, list2);
                            }
                            else
                            {
                                //satış Fiyatlarınıda güncelle
                                ArrayList list2 = new ArrayList();
                                list2.Add(new SqlParameter("@fkStokKarti", dr["fkStokKarti"].ToString()));
                                list2.Add(new SqlParameter("@fkSatisFiyatlariBaslik", lueFiyatlar.EditValue.ToString()));
                                list2.Add(new SqlParameter("@SatisFiyatiKdvli", SatisFiyati.Text.Replace(",", ".")));
                                sql = "update SatisFiyatlari set Aktif=1,SatisFiyatiKdvli=@SatisFiyatiKdvli where fkStokKarti=@fkStokKarti and fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik";
                                DB.ExecuteSQL(sql, list2);
                            }

                        }
                        break;
                    }
                //Alış Fiyatı
                case 1:
                    {
                        for (int i = 0; i < gridView1.DataRowCount; i++)
                        {
                            DataRow dr = gridView1.GetDataRow(i);
                            ArrayList list = new ArrayList();
                            list.Add(new SqlParameter("@pkStokKarti", dr["fkStokKarti"].ToString()));
                            list.Add(new SqlParameter("@AlisFiyati", ceYeniAlisFiyati.Text.Replace(",", ".")));
                            sql = "update StokKarti set AlisFiyati=@AlisFiyati where pkStokKarti=@pkStokKarti";
                            DB.ExecuteSQL(sql, list);
                        }
                        //Alış Fiyatı Kdv Hariç
                        sql = "update StokKarti set AlisFiyatiKdvHaric=AlisFiyati-(AlisFiyati*KdvOrani)/(100+KdvOrani)";
                        DB.ExecuteSQL(sql);
                        break;
                    }
                //mevcut
                case 6:
                    {
                        simpleButton13_Click(sender, e);//kaydet
                        break;
                    }
                        default:
                    break;
            }
            DB.ExecuteSQL("UPDATE StokFiyatGuncelle SET Siparis=1 WHERE pkStokFiyatGuncelle=" + pkSatisBarkod.Text);
            btnAciklamaGirisi.ToolTip = "";
            //cbislemTipi.SelectedIndex = 0;
            //gridControl1.DataSource = null;
            //cbOdemeSekli.SelectedIndex = 0;
            SatisFiyati.Value = 0;
            SatisDetayGetir(pkSatisBarkod.Text);
            
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }

        void FisYazdir_sil(bool Disigner, string pkSatislar,string SatisFisTipi,string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_StokFiyatGuncelleDetay " + fisid + ",1");
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
            System.Data.DataSet ds = new DataSet("Test");
            DataTable FisDetay = DB.GetData(@"SELECT * from StokFiyatGuncelleDetay where fkSatislar=" + fisid);
            FisDetay.TableName = "FisDetay";
            ds.Tables.Add(FisDetay);
            DataTable Fis = DB.GetData(@"SELECT  * from StokFiyatGuncelle where pkStokFiyatGuncelle=" + fisid);
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
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Satislar s 
inner join StokFiyatGuncelleDetay sd on sd.fkStokFiyatGuncelle=s.pkStokFiyatGuncelle
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
            string pkStokKartiid = dr["pkStokKartiid"].ToString();
            if (pkStokKartiid == "" || pkStokKartiid == "0")
               DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            else
            DB.pkStokKarti = int.Parse(pkStokKartiid);
            StokKarti.ShowDialog();
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
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
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
                if (Fiyat.Trim() == "0")
                {
                    e.Appearance.BackColor = Color.Red;
                }

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
           if(((SimpleButton)sender).Tag!=null)
                urunekle(((SimpleButton)sender).Tag.ToString());
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmHizliButtonDuzenle HizliButtonDuzenle = new frmHizliButtonDuzenle();
            HizliButtonDuzenle.Top = HizliTop;
            HizliButtonDuzenle.Left = HizliLeft;
            HizliButtonDuzenle.oncekibarkod.Text = HizliBarkod;
            HizliButtonDuzenle.oncekibarkod.Tag = pkHizliStokSatis;
            HizliButtonDuzenle.ShowDialog();
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

        void FisListesi()
        {
            string sql = @"Select pkStokFiyatGuncelle,s.Tarih,k.KullaniciAdi,s.Aciklama from StokFiyatGuncelle s
            LEFT JOIN Kullanicilar k ON s.fkKullanici=k.pkKullanicilar
            where s.Siparis=1 order by pkStokFiyatGuncelle desc";
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
            pkSatisBarkod.Text = lueFis.EditValue.ToString();
            //SatisGetir();
            FisListesi();
            lueFis.EditValue = null;
            yesilisikyeni();
        }

        //void SatisGetir_()
        //{
        //    DataTable dtSatislar = DB.GetData("exec sp_Satislar " + pkSatisBarkod.Text);//fiş bilgisi
        //    if (dtSatislar.Rows.Count == 0)
        //    {
        //        Showmessage("Fiş Bulunamadı", "K");
        //        return;
        //    }
        //}

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            //frmSatisRaporlari Satislar = new frmSatisRaporlari();
            //Satislar.ShowDialog();
            //yesilisikyeni();
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
            string pkSatis = pkSatisBarkod.Text;
            if (pkSatis == "0")
            {
                Showmessage("Önce Fiyat Listesi Yapınız!", "K");
                return;
            }
            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.memoozelnot.Text = btnAciklamaGirisi.ToolTip;
            fFisAciklama.ShowDialog();
            btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;
            DB.ExecuteSQL("UPDATE StokFiyatGuncelle SET Aciklama='" + btnAciklamaGirisi.ToolTip + "' where pkStokFiyatGuncelle=" + pkSatis);
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
            FisListesi();
        }

        public bool OnceSatisYapiniz()
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.Text = "Önce Fiyat Listesi Yapınız!";
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.Show();
                return false;
            }
            return true;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE HizliStokSatis Set fkStokKarti=0 where pkHizliStokSatis=" + pkHizliStokSatis);
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
                
            }
        }

        private void gridControl1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
        }

        private void lueSatisTipi_EditValueChanged(object sender, EventArgs e)
        {
            FisListesi();
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
            //pkSatisBarkod.Text = pkSatislar;
            string sql = @"
SELECT sfgd.pkStokFiyatGuncelleDetay,sfgd.fkStokKarti,sfgd.iskontotutar,sfgd.iskontoyuzdetutar, 
            sk.Stokadi, sk.Barcode, sk.Stoktipi,b.BirimAdi,
            sk.StokKod,sk.AlisFiyati,sfgd.Adet,
            sf.SatisFiyatiKdvli as SatisFiyati,
            sk.SatisFiyati as NakitFiyat,
            (sfgd.Adet*(sfgd.SatisFiyati-sfgd.iskontotutar)) as Tutar,iade,pkStokKartiid,alis_iskonto,satis_iskonto,
            sg.StokGrup,sag.StokAltGrup,sk.KdvOrani,sk.KdvOraniAlis,sk.Mevcut,skd.MevcutAdet FROM 
            StokFiyatGuncelleDetay sfgd with(nolock)  
            INNER JOIN StokKarti sk with(nolock)  ON  sfgd.fkStokKarti = sk.pkStokKarti 
			LEFT JOIN StokKartiDepo skd with(nolock) ON skd.fkStokKarti=sk.pkStokKarti 
            INNER JOIN SatisFiyatlari sf with(nolock) ON sf.fkStokKarti=sfgd.fkStokKarti and sf.fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik
            left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
            left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
			left join Birimler b with(nolock) ON b.pkBirimler=sk.fkBirimler
            where sfgd.fkStokFiyatGuncelle=@fkStokFiyatGuncelle";

            sql = sql.Replace("@fkSatisFiyatlariBaslik", lueFiyatlar.EditValue.ToString());
            sql = sql.Replace("@fkStokFiyatGuncelle", pkSatislar);

            gridControl1.DataSource = DB.GetData(sql);

            gridView1.AddNewRow();
        }
        void StokFiyatGuncelleEkle(string barkod)
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
            if (dr != null && dr["pkStokFiyatGuncelleDetay"].ToString() != "") return;
            int pkStokFiyatGuncelle = 0;
            int.TryParse(pkSatisBarkod.Text, out pkStokFiyatGuncelle);
            if (pkStokFiyatGuncelle == 0) 
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
            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti FROM StokKarti with(nolock) where Barcode='" + barkod + "'");
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
            arr.Add(new SqlParameter("@SatisFiyatGrubu", "0"));        
            arr.Add(new SqlParameter("@Adet", EklenenMiktar.ToString().Replace(",",".")));
            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
            arr.Add(new SqlParameter("@iskontoyuzde", "0"));
            arr.Add(new SqlParameter("@iskontotutar", "0"));
            string s = DB.ExecuteScalarSQL("exec sp_StokFiyatGuncelleDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti,@iskontoyuzde,@iskontotutar", arr);
            if (s != "Satis Detay Eklendi.")
            {
                MessageBox.Show(s);
                return;
            }
        }

        void YeniSatis()
        {
            string sql = "";
            string fisno = "0";
            bool yazdir = false;
            ArrayList list = new ArrayList();
            string pkFirma = "1";
            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Siparis", "0"));
            list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("@fkSatisDurumu", cbislemTipi.SelectedIndex));
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));//iskontoTutar.Value.ToString().Replace(",", ".")));

            sql = "INSERT INTO StokFiyatGuncelle (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar,0,0) SELECT IDENT_CURRENT('StokFiyatGuncelle')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            pkSatisBarkod.Text = fisno;
            vBeklemeListesi();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Bilgileri ve Genel Durumu. Yapım Aşamasındadır...");
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(2,0);
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
                else if (dr["pkStokFiyatGuncelleDetay"].ToString() == "")
                    gridView1.DeleteRow(i);
            }
        }

        private void repositoryItemCalcEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if(dr==null) return;
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
                decimal SatisFiyati = decimal.Parse(dr["SatisFiyati"].ToString());
                decimal iskonto = 0, iskontotutar=0;
                decimal.TryParse(girilen, out iskontotutar);
                iskonto = SatisFiyati - iskontotutar;
                DB.ExecuteSQL("UPDATE SatisDetay SET iskontotutar=" + iskonto.ToString().Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
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
            if (e.KeyCode == Keys.Enter && !gridView1.IsEditing && gridView1.FocusedColumn.FieldName == "iskyuzdesanal")
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                frmiskonto iskonto = new frmiskonto();
                iskonto.fkSatisDetay.Text = dr["pkStokFiyatGuncelleDetay"].ToString();
                iskonto.ShowDialog();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
           
            DataTable dt = DB.GetData("select top 1 pkStokFiyatGuncelle from StokFiyatGuncelle with(nolock) where Siparis<>1");
            //and fkKullanici="  + DB.fkKullanicilar);
            if (dt.Rows.Count > 0)
            {
                pkSatisBarkod.Text = dt.Rows[0]["pkStokFiyatGuncelle"].ToString();

                SatisDetayGetir(pkSatisBarkod.Text);
            }
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
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("55");//2.1
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            Hizlibuttonlariyukle();
        }

        private void groupControl6_MouseClick(object sender, MouseEventArgs e)
        {
            pkSatisBarkod.Text = "0";
            timer1.Enabled = true;
            yesilisikyeni();
        }

        private void btnKampanya_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE StokFiyatGuncelleDetay SET NakitFiyat=" + dr["SatisFiyati"].ToString().Replace(",", ".") +
                  " where pkStokFiyatGuncelleDetay=" + dr["pkStokFiyatGuncelleDetay"].ToString());
            }
            yesilisikyeni();
        }


        private void SatisFiyati_Leave(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE StokFiyatGuncelleDetay SET NakitFiyat=" + dr["NakitFiyat"].ToString().Replace(",", ".") +
                  " where pkStokFiyatGuncelleDetay=" + dr["pkStokFiyatGuncelleDetay"].ToString());
            }
        }

        void liste()
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string sql = @"SELECT SatisFiyatlari.pkSatisFiyatlari, SatisFiyatlari.SatisFiyatiKdvli, SatisFiyatlari.SatisFiyatiKdvsiz, SatisFiyatlariBaslik.Baslik
                        ,SatisFiyatlari.Aktif,SatisFiyatlari.fkSatisFiyatlariBaslik,SatisFiyatlari.iskontoYuzde FROM  SatisFiyatlari INNER JOIN
                      SatisFiyatlariBaslik ON SatisFiyatlari.fkSatisFiyatlariBaslik = SatisFiyatlariBaslik.pkSatisFiyatlariBaslik
                        WHERE SatisFiyatlari.fkStokKarti = " + dr["fkStokKarti"].ToString()+ " ORDER BY SatisFiyatlari.fkSatisFiyatlariBaslik";
            gridControl2.DataSource = DB.GetData(sql);
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            liste();
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //xtraTabControl1.SelectedTabPageIndex = cbislemTipi.SelectedIndex;
            if (cbislemTipi.SelectedIndex == 0)
            {
                gcAlisFiyati.Visible = false;
                gcSatisFiyati.Visible = true;
            }
            else if (cbislemTipi.SelectedIndex == 1)
            {
                gcAlisFiyati.Visible = true;
                gcSatisFiyati.Visible = false;
            }
        }

        private void vGridControl1_Click(object sender, EventArgs e)
        {
            DevExpress.XtraVerticalGrid.Rows.BaseRow R;
            R = vGridControl1.GetRowByFieldName("pkStokFiyatGuncelle");
            if (vGridControl1.GetCellValue(R, vGridControl1.FocusedRecord) != null)
            {
                string s = vGridControl1.GetCellValue(R, vGridControl1.FocusedRecord).ToString();
                pkSatisBarkod.Text = s;
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            //cbislemTipi.SelectedIndex=xtraTabControl1.SelectedTabPageIndex;
            if (e.Page == xtraTabPage1)
            {
                lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock)");
                lueDepolar.EditValue = Degerler.fkDepolar;
            }
            
            else if (e.Page == xtraTabPage2)
            {
                gridColumn4.Visible = true;
                gridColumn4.Width = 200;
                gridColumn14.Visible = true;
                gridColumn14.Width = 100;
                gridColumn15.Visible = true;
                gridColumn15.Width = 100;
                gcAlisFiyati.Visible = true;
            }
            else if (e.Page == xtraTabPage3)
            {
                Gruplar();
            }
            else if (e.Page == xtraTabPage7)
            {
                gridColumn35.Visible = true;
            }
            else if (e.Page == xtraTabPage4)
            {
                lueBirimler.Properties.DataSource =
                    DB.GetData("select pkBirimler,BirimAdi from Birimler with(nolock)");
            }
            else
            {
                gridColumn4.Visible = false;
                gridColumn14.Visible = false;
                gridColumn15.Visible = false;
            }
            
        }

        private void lueFis_Enter(object sender, EventArgs e)
        {
            FisListesi();
        }

        private void btnotoiskonto_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Otomatik Alış Oranları Liste Fiyatına Göre Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //if (ceListeFiyati.EditValue == null)
            //{
            //    MessageBox.Show("Liste Fiyatını Kontrol Ediniz");
            //    return;
            //}

            for (int i = 0; i < gridView1.DataRowCount; i++)
			{
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("update StokKarti set alis_iskonto="+seAlisiskonto.EditValue.ToString().Replace(",",".") +
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString());
			}

            //DB.ExecuteSQL(@"update StokKarti set satis_iskonto=satis_iskonto)/100) where isnull(satis_iskonto,0)>0");
            //DB.ExecuteSQL(@"update StokKarti set AlisFiyati=AlisFiyati-((AlisFiyati*alis_iskonto)/100) where isnull(alis_iskonto,0)>0");

            //DB.ExecuteSQL(@"update SatisFiyatlari set SatisFiyatiKdvli=sk.SatisFiyati From StokKarti  sk
            //                where sk.pkStokKarti=SatisFiyatlari.fkStokKarti");
            //DB.ExecuteSQL(@"update SatisFiyatlari set iskontoYuzde=sk.satis_iskonto From StokKarti sk
            //            where sk.pkStokKarti=SatisFiyatlari.fkStokKarti and sk.satis_iskonto>0");
            
            SatisDetayGetir(pkSatisBarkod.Text);
        }

        private void seAlisiskonto_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Alış Kdv Oranları Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("update StokKarti set "+
                    "KdvOraniAlis=" + seKdvOraniAlis.EditValue.ToString().Replace(",", ".") +
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }

            SatisDetayGetir(pkSatisBarkod.Text);
        }
        void AltGruplar()
        {
            if (lueGruplar.EditValue == null)
            {
                lueAltGrup.EditValue = null;
                return;
            }
            lueAltGrup.Properties.DataSource =
            DB.GetData("select * from StokAltGruplari where fkStokGruplari=" + lueGruplar.EditValue.ToString());
            lueAltGrup.EditValue = int.Parse(lueAltGrup.Tag.ToString());
        }
        private void lueGruplar_EditValueChanged(object sender, EventArgs e)
        {
            AltGruplar();
        }

        private void simpleButton28_Click(object sender, EventArgs e)
        {
            frmStokGrubuYeniKarti StokGrubuYeniKarti = new frmStokGrubuYeniKarti();
            StokGrubuYeniKarti.Tag = 1;
            StokGrubuYeniKarti.ShowDialog();
            Gruplar();
            if (lueGruplar.EditValue == null)
                lueGruplar.EditValue = int.Parse(DB.GetData("SELECT MAX(pkStokGrup) FROM StokGruplari").Rows[0][0].ToString());
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            if (lueGruplar.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Stok Grubu Seçiniz", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lueGruplar.Focus();
                return;
            }
            frmStokAltGruplari StokAltGrupKarti = new frmStokAltGruplari();
            StokAltGrupKarti.lueStokGruplari.Tag = lueGruplar.EditValue.ToString();
            StokAltGrupKarti.ShowDialog();
            AltGruplar();
            if (lueAltGrup.EditValue == null)
                lueAltGrup.EditValue = int.Parse(DB.GetData("SELECT MAX(pkStokAltGruplari) FROM StokAltGruplari").Rows[0][0].ToString());

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (lueGruplar.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Stok Grubu Seçiniz", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lueGruplar.Focus();
                return;
            }
            frmStokAltGruplari StokAltGrupKarti = new frmStokAltGruplari();
            StokAltGrupKarti.lueStokGruplari.Tag = lueGruplar.EditValue.ToString();
            StokAltGrupKarti.ShowDialog();
            AltGruplar();
            if (lueAltGrup.EditValue == null)
                lueAltGrup.EditValue = int.Parse(DB.GetData("SELECT MAX(pkStokAltGruplari) FROM StokAltGruplari").Rows[0][0].ToString());

        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Gruplar Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                DB.ExecuteSQL("update StokKarti set fkStokGrup=" + lueGruplar.EditValue.ToString().Replace(",", ".") +
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }

            SatisDetayGetir(pkSatisBarkod.Text);
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Alt Gruplar Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                DB.ExecuteSQL("update StokKarti set fkStokAltGruplari=" + lueAltGrup.EditValue.ToString().Replace(",", ".") +
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }

            SatisDetayGetir(pkSatisBarkod.Text);
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Otomatik Satış Oranları Liste Fiyatına Göre Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //if (ceListeFiyati.EditValue == null)
            //{
            //    MessageBox.Show("Liste Fiyatını Kontrol Ediniz");
            //    return;
            //}

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("update StokKarti set satis_iskonto=" + seSatisiskonto.EditValue.ToString().Replace(",", ".") +
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }

            //DB.ExecuteSQL(@"update StokKarti set satis_iskonto=satis_iskonto)/100) where isnull(satis_iskonto,0)>0");
            //DB.ExecuteSQL(@"update StokKarti set AlisFiyati=AlisFiyati-((AlisFiyati*alis_iskonto)/100) where isnull(alis_iskonto,0)>0");

            //DB.ExecuteSQL(@"update SatisFiyatlari set SatisFiyatiKdvli=sk.SatisFiyati From StokKarti  sk
            //                where sk.pkStokKarti=SatisFiyatlari.fkStokKarti");
            //DB.ExecuteSQL(@"update SatisFiyatlari set iskontoYuzde=sk.satis_iskonto From StokKarti sk
            //            where sk.pkStokKarti=SatisFiyatlari.fkStokKarti and sk.satis_iskonto>0");

            SatisDetayGetir(pkSatisBarkod.Text);
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Adetleri Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                DB.ExecuteSQL("update StokKarti set SatisAdedi=" + seSatisAdedi.Value.ToString().Replace(",", ".") +
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }

            SatisDetayGetir(pkSatisBarkod.Text);
            
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Mevcutları " + ceSimdikiMevcut.Value.ToString() + " Yapılacaktır. Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            progressBar1.Maximum = gridView1.DataRowCount;
            listBox1.Items.Clear();
            btnKaydet.Enabled = false;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                try
                {
                progressBar1.Value = i;

                Application.DoEvents();

                DataRow dr = gridView1.GetDataRow(i);

                string fkStokKarti = dr["fkStokKarti"].ToString();

                DataTable dt = DB.GetData("select Mevcut,dbo.fon_StokMevcut(pkStokKarti) as StokMevcut from StokKarti with(nolock) where pkStokKarti=" + fkStokKarti);

                //decimal mevcut = decimal.Parse(dt.Rows[0]["Mevcut"].ToString());
                decimal mevcut = ceSimdikiMevcut.Value;
                decimal gercekMevcut = decimal.Parse(dt.Rows[0]["StokMevcut"].ToString());

                #region bu kısım stok kartındaki ile aynı
                string sql = @"INSERT INTO StokDevir (fkStokKarti,Tarih,Aciklama,OncekiAdet,DevirAdedi,fkKullanicilar,islemTarihi,fkDepolar)
                    values(@fkStokKarti,@Tarih,@Aciklama,@OncekiAdet,@DevirAdedi,@fkKullanicilar,getdate(),@fkDepolar)";

                ArrayList list0 = new ArrayList();
                list0.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                list0.Add(new SqlParameter("@Tarih", DateTime.Now));
                list0.Add(new SqlParameter("@Aciklama", "Stok Kartı Toplu Değişiklik"));
                list0.Add(new SqlParameter("@OncekiAdet", mevcut.ToString().Replace(",", ".")));
                list0.Add(new SqlParameter("@fkDepolar", lueDepolar.EditValue.ToString()));
                    
                decimal deviradet = 0;
                if (gercekMevcut < 0)
                    deviradet = (gercekMevcut * -1) + mevcut;
                else
                {
                    deviradet = mevcut - gercekMevcut;
                }

                list0.Add(new SqlParameter("@DevirAdedi", deviradet.ToString().Replace(",", ".")));
                list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

                string sonuc = DB.ExecuteSQL(sql, list0);
                if (sonuc == "0")
                {
                    DB.ExecuteSQL("update StokKarti set Mevcut=" + ceSimdikiMevcut.Value.ToString().Replace(",", ".") + " where pkStokKarti=" + fkStokKarti);
                    DB.ExecuteSQL("update StokKartiDepo set MevcutAdet=" + ceSimdikiMevcut.Value.ToString().Replace(",", ".") + " where fkStokKarti=" + 
                        fkStokKarti+" and fkDepolar="+lueDepolar.EditValue.ToString());
                }
                #endregion

                }
                catch (Exception exp)
                {
                    listBox1.Items.Add(exp.Message);
                   // throw;
                    continue;
                }
                i++;
            }
            btnKaydet.Enabled = true;
            SatisDetayGetir(pkSatisBarkod.Text);
        }

        private void lueFiyatlar_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pkSatisBarkod.Text))
               SatisDetayGetir(pkSatisBarkod.Text);
        }

        private void txtbirincifiyatorani_EditValueChanged(object sender, EventArgs e)
        {

            //if (txtbirincifiyatorani.Text != "")
            //{
            //    string oran = txtbirincifiyatorani.Text.Replace("%", "");

            //    decimal doran = decimal.Parse(oran);
            //    SatisFiyati1.Value = AlisFiyati.Value + (AlisFiyati.Value * doran) / 100;
            //}
            //SatisFiyati1.Focus();
            
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Kdv Oranları Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("update StokKarti set KdvOrani=" + seKdvOraniSatis.EditValue.ToString().Replace(",", ".") +
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }

            SatisDetayGetir(pkSatisBarkod.Text);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Stok Mevcutları sıfırlanacaktır. Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            //zaman aşımı oluyor
            //DataTable dt = DB.GetData("select Mevcut,dbo.fon_StokMevcut(pkStokKarti) as StokMevcut from StokKarti with(nolock) where aktif=1");
            string sql1 = "";

            if (cbSadeceAktifler.Checked)
                sql1 = "select pkStokKarti,Mevcut from StokKarti with(nolock) where aktif=1";
            else
                sql1 = "select pkStokKarti,Mevcut from StokKarti with(nolock)";

            DataTable dt = DB.GetData(sql1);

            progressBar1.Maximum = dt.Rows.Count;

            btnKaydet.Enabled = false;
            int i = 0;
            //string depoid = lueDepolar.EditValue.ToString();

            DB.ExecuteSQL("delete from StokDevir");
            DB.ExecuteSQL("delete from StokSayimDetay");
            DB.ExecuteSQL("delete from StokSayim");
            //DB.ExecuteSQL("delete from StokSayim where pkStokSayim in (select fkStokSayim from StokSayimDetay where fkDepolar=" + 
            //    depoid+ " group by fkStokSayim)");
            DB.ExecuteSQL("delete from  Sayim");// where fkDepolar=" + depoid);
            DB.ExecuteSQL("delete from DepoTransferDetay");// where fkDepolar=" + depoid);
            DB.ExecuteSQL("delete from DepoTransfer");// where fkDepolar=" + depoid);

            foreach (DataRow item in dt.Rows)
            {
                try
                {
                    progressBar1.Value = i + 1;

                    simpleButton16.Text = (i + 1).ToString();

                    Application.DoEvents();

                    string fkStokKarti = item["pkStokKarti"].ToString();
                    //decimal mevcut = decimal.Parse(dt.Rows[0]["Mevcut"].ToString());
                    decimal mevcut = ceSimdikiMevcut.Value;
                    decimal gercekMevcut = 0;//decimal.Parse(dt.Rows[0]["StokMevcut"].ToString());
                    gercekMevcut = decimal.Parse(DB.GetData("select dbo.fon_StokMevcut(" + fkStokKarti + ")").Rows[0][0].ToString());

                    #region bu kısım stok kartındaki ile aynı
                    string sql = @"INSERT INTO StokDevir (fkStokKarti,Tarih,Aciklama,OncekiAdet,DevirAdedi,fkKullanicilar,islemTarihi,fkDepolar)
                    values(@fkStokKarti,@Tarih,@Aciklama,@OncekiAdet,@DevirAdedi,@fkKullanicilar,getdate(),1)";

                    ArrayList list0 = new ArrayList();
                    list0.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                    list0.Add(new SqlParameter("@Tarih", DateTime.Now));
                    list0.Add(new SqlParameter("@Aciklama", "Genel Stok Mevcut Sıfırlama"));
                    list0.Add(new SqlParameter("@OncekiAdet", mevcut.ToString().Replace(",", ".")));

                    decimal deviradet = 0;
                    if (gercekMevcut < 0)
                        deviradet = (gercekMevcut * -1) + mevcut;
                    else
                    {
                        deviradet = mevcut - gercekMevcut;
                    }

                    list0.Add(new SqlParameter("@DevirAdedi", deviradet.ToString().Replace(",", ".")));
                    list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

                    string sonuc = DB.ExecuteSQL(sql, list0);
                    if (sonuc == "0")
                    {
                        
                        DB.ExecuteSQL("update StokKarti set Mevcut=0 where pkStokKarti=" + fkStokKarti);
                        DB.ExecuteSQL("update StokKartiDepo set MevcutAdet=0 where fkStokKarti=" + fkStokKarti);
                    }
                    else
                        DB.logayaz(sonuc, sql);
                    #endregion
                }
                catch (Exception exp)
                {
                    listBox1.Items.Add(exp.Message);
                    // throw;
                    continue;
                }
                i++;
            }
            btnKaydet.Enabled = true;
            MessageBox.Show("Stok Mevcut Sıfırlama işlemi Tamamlandı");
            SatisDetayGetir(pkSatisBarkod.Text);
        }

        private void simpleButton18_Click_1(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Birimler Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                DB.ExecuteSQL("update StokKarti set fkBirimler=" + lueBirimler.EditValue.ToString().Replace(",", ".") +
                    ",Stoktipi='" +lueBirimler.Text+"'"+
                    " where pkStokKarti=" + dr["fkStokKarti"].ToString());
            }

            SatisDetayGetir(pkSatisBarkod.Text);
        }
    }
}

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
using GPTS.islemler;

namespace GPTS
{
    public partial class frmEtiketBas : DevExpress.XtraEditors.XtraForm
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis = "";//AccessibleDescription
        int AcikSatisindex = 1;//hangi satış açık
        public frmEtiketBas()
        {
            InitializeComponent();
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 150;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 5;
            DB.PkFirma = 1;
        }
        void Showmessage(string lmesaj, string renk)
        {
            frmMesajBox mesaj = new frmMesajBox(200);
            mesaj.label1.Text = lmesaj;
            if (renk == "K")
                mesaj.label1.BackColor = System.Drawing.Color.Red;
            else if (renk == "B")
                mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
            else
                mesaj.label1.BackColor = System.Drawing.Color.Blue;
            mesaj.Show();
        }
        void Yetkiler()
        {
            string sql = @"SELECT YetkiAlanlari.Yetki, YetkiAlanlari.Sayi, Parametreler.Aciklama10, YetkiAlanlari.BalonGoster
FROM  YetkiAlanlari INNER JOIN Parametreler ON YetkiAlanlari.fkParametreler = Parametreler.pkParametreler
WHERE  Parametreler.fkModul = 1  and YetkiAlanlari.fkKullanicilar =" + DB.fkKullanicilar;
            DataTable dtYetkiler = DB.GetData(sql);
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
                //    gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
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
        void EtiketSablonlari()
        {
            TabHizliSatisGenel.Tag = "0";
            searchLookUpEdit1.Properties.DataSource = DB.GetData(@"SELECT * FROM EtiketSablonlari with(nolock) where Aktif=1 ORDER BY Varsayilan desc");
        }

        private void ucAnaEkran_Load(object sender, EventArgs e)
        {
            EtiketSablonlari();

            DataTable dt = DB.GetData(@"SELECT * FROM EtiketSablonlari with(nolock) where Aktif=1 ORDER BY Varsayilan desc");

            if (dt.Rows.Count>0)
                searchLookUpEdit1.EditValue = int.Parse(dt.Rows[0]["pkEtiketSablonlari"].ToString());

            Yetkiler();

            FisListesi();

            GeciciMusteriDefault();

            timer1.Enabled = true;

            deTarihi.DateTime = DateTime.Now;

            string Dosya = DB.exeDizini + "\\EtiketBasGrid.xml";
            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
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
                EtiketBasDetayEkle(((SimpleButton)sender).Tag.ToString());
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
            DataTable dtbutton = DB.GetData(@"select * from HizliStokSatis h
left join (select pkStokKarti,fkStokGrup,HizliSatisAdi,Barcode,HizliSiraNo,SatisFiyati 
from StokKarti) sk on sk.pkStokKarti=h.fkStokKarti order by pkHizliStokSatis");
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
                sb.Tag = Barcode;
                sb.ToolTip = "Satış Fiyatı=" + SatisFiyati;
                sb.ToolTipTitle = "Kodu: " + Barcode;
                sb.Height = h;
                sb.Width = w;
                sb.Click += new EventHandler(ButtonClick);
                //this.btnAciklamaGirisi.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnAciklamaGirisi_MouseMove);
                sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseEnter);
                sb.Left = lef;
                sb.Top = to;
                if (HizliSatisAdi.Length > 15)
                    sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                else
                    sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                sb.ContextMenuStrip = contextMenuStrip1;
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string imagedosya = exeDiz + "\\HizliSatisButtonResim\\" + pkid + ".png";
                if (File.Exists(imagedosya))
                {
                    Image im = new Bitmap(imagedosya);
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
                //DockPanel p1 = dockManager1.AddPanel(DockingStyle.Left);
                //p1.Text = "Genel";
                //DevExpress.XtraEditors.SimpleButton btn = new DevExpress.XtraEditors.SimpleButton();
                //btn.Text = "Print...";
                TabHizliSatisGenel.Controls.Add(sb);
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
            TabHizliSatisGenel.Tag = "1";
        }

        void SatisTemizle()
        {
            if (AcikSatisindex == 1)
            {
                Satis1Toplam.Tag = 0;
                Satis1Toplam.Text = "0,0";
                tepkEtiketBas.Text = "0";
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
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Etiket Basımı İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle

            string sonuc = "", pkSatislar = "0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
          
            sonuc = DB.ExecuteScalarSQL("EXEC spEtiketBasSil " + pkSatislar + ",1");
            if (sonuc != "Etiket Silindi.")
                MessageBox.Show(sonuc);

            SatisTemizle();
            FisListesi();
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
            DB.ExecuteSQL("DELETE FROM EtiketBasDetay WHERE pkEtiketBasDetay=" + dr["pkEtiketBasDetay"].ToString());
            yesilisikyeni();
        }
        void YeniSatisEkle()
        {
            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
                YeniSatis();
            
            
        }
        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && gridView1.DataRowCount > 0)
            {
                //üst satırı kopyala
                DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);
                EtiketBasDetayEkle(dr["Barcode"].ToString());

                yesilisikyeni();
                
                gridView1.ShowEditor();
                gridView1.ActiveEditor.SelectAll();
                gridView1.CloseEditor();
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                string kod =
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (kod == "" && gridView1.DataRowCount == 0)
                {
                    yesilisikyeni();
                    return;
                }
                EtiketBasDetayEkle(kod);
                yesilisikyeni();
            }
        }
        void urunaraekle()
        {
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "4";
            StokAra.ShowDialog();
            Application.DoEvents();
            if (StokAra.TopMost == false) 
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {

                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    EtiketBasDetayEkle(dr["Barcode"].ToString());
                }

            }
            StokAra.Dispose();
            yesilisikyeni();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();
            urunaraekle();
        }
        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            gridView1.AddNewRow();
            urunaraekle();
        }
        private void urunekle(string barkod)
        {
            if (barkod == "") return;

            //string ilktarih = "";
            //string sontarih = "";
            //string fkUrunlerNoPromosyon = "";
            //int DususAdet = 1;
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

            EtiketBasDetayEkle(barkod);
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle == -2147483647 || gridView1.FocusedRowHandle >= 0)
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
        }
        void resimolustur(SimpleButton sb)
        {
            x = x + 25;
            y = y + 25;
            PictureBox pictureBox = new PictureBox();
            //xtraTabPage1.Controls.Add(pictureBox);
            pictureBox.Location = new System.Drawing.Point(150 + x, 50 + y);
            pictureBox.Name = "pictureBox" + p;
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
                EtiketBasDetayGetir(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
            }
            
            else if (AcikSatisindex == 4)
            {
                EtiketBasDetayGetir(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
            }
            
            if (AcikSatisindex == 1) fontayarla(Satis1Toplam);       

            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();
            SendKeys.Send("{ENTER}");
        }
        private bool SatisVar()
        {

            if (gridView1.DataRowCount == 0)
            {
                Showmessage("Önce Etiket Listesi Yapınız!", "K");
                return false;
            }
            return true;
        }
        void GeciciMusteriDefault()
        {
            DataTable dtMusteriler = DB.GetData("select pkTedarikciler,Firmaadi from Tedarikciler where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
            {
                dtMusteriler = DB.GetData("select pkTedarikciler,Firmaadi from Tedarikciler where pkTedarikciler=1");
                //MessageBox.Show("Geçici Müşteri Bulunamadı.Lütfen Yetkili Firmayı Arayınız");
            }
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkTedarikciler"].ToString());
                DB.FirmaAdi = DB.PkFirma + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
                Satis1Baslik.Text = DB.FirmaAdi;
                Satis1Firma.Tag = DB.PkFirma;
               
               
            }
        }
        void temizle(int aciksatisno)
        {
            DataTable dtMusteriler = DB.GetData("select pkTedarikciler,Firmaadi from Tedarikciler where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
            {
                //MessageBox.Show("Geçici Tedarikçi Bulunamadı.Lütfen Geçici Tedatikçi Olrak Ayarlayınız.");
                DB.ExecuteSQL("UPDATE Tedarikciler SET GeciciMusteri=1 WHERE pkTedarikciler=1");
            }
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkTedarikciler"].ToString());
                DB.FirmaAdi = DB.PkFirma + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
            }
            if (aciksatisno == 1)
            {
                Satis1Baslik.Text = DB.FirmaAdi;
                Satis1Firma.Tag = DB.PkFirma;
            }
           
            btnAciklamaGirisi.ToolTip = "";
            gridControl1.DataSource = null;
        }
        void satiskaydet(bool timer, bool yazdir, bool odemekaydedildi)
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesajBox mesaj = new frmMesajBox(200);
                mesaj.label1.Text = "Önce Etiket Listesi Yapınız!";
                mesaj.Show();
                return;
            }
            FisListesi();
            SatisDuzenKapat();
            // temizle(AcikSatisindex);
            yesilisikyeni();
        }
        private void simpleButton37_Click(object sender, EventArgs e)
        {
            if (OnceSatisYapiniz() == false) return;

            EtiketBaskaydet(true, false);

            SatisTemizle();

            FisListesi();

            yesilisikyeni();
        }
        void EtiketBaskaydet(bool yazdir, bool odemekaydedildi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkEtiketBas", tepkEtiketBas.Text));
            if (btnAciklamaGirisi.ToolTip == "")
                list.Add(new SqlParameter("@Aciklama", tepkEtiketBas.Text + ".Etiket"));
            else
                list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));

            list.Add(new SqlParameter("@Tarih", deTarihi.DateTime));

            string sonuc = DB.ExecuteSQL("UPDATE EtiketBas SET Tarih=@Tarih,Siparis=1,Aciklama=@Aciklama where pkEtiketBas=@pkEtiketBas", list);
            if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            {
                Showmessage("Hata Oluştur: " + sonuc, "K");
                return;
            }

            //SatisTemizle();

            //FisListesi();

            //yesilisikyeni();
            //TODO: ALIŞ FATURASINA GÖRE STOKTAN DÜŞME YAPILACAK.ALIŞ FATURASINDAKİ ADET TEN DÜŞECEK.            
        }
        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }
        void FisYazdirAdi(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi,bool yazdir)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

            DataTable dtyeni = new DataTable();
            dtyeni.Columns.Add(new DataColumn("Adet", typeof(Int32)));
            dtyeni.Columns.Add(new DataColumn("fkStokKarti", typeof(Int32)));
            dtyeni.Columns.Add(new DataColumn("Barcode", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("Stokadi", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("StokGrup", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("BirimAdi", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("UreticiKodu", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("SatisFiyati", typeof(decimal)));
            dtyeni.Columns.Add(new DataColumn("SatisFiyati2", typeof(decimal)));
            dtyeni.Columns.Add(new DataColumn("SatisFiyati3", typeof(decimal)));
            dtyeni.Columns.Add(new DataColumn("koliicitanefiyati", typeof(decimal)));
            dtyeni.Columns.Add(new DataColumn("TedarikciAdi", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("GizliFiyat", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("EtiketAciklama", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("Marka", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("Beden", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("Renk", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("ParaKodu", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("Aciklama", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("urunresmi", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("KdvOrani", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("SatisFiyatiKdvHaric", typeof(decimal)));
            dtyeni.Columns.Add(new DataColumn("SatisAdedi", typeof(string)));
            dtyeni.Columns.Add(new DataColumn("icindekiadet", typeof(string)));
            
            DataRow dr;
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_EtiketBas " + fisid + ",1");//(DataTable)gridControl2.DataSource;
                for (int i = 0; i < FisDetay.Rows.Count; i++)
                {
                    int adet = int.Parse(FisDetay.Rows[i]["Adet"].ToString());
                    string fkStokKarti = FisDetay.Rows[i]["fkStokKarti"].ToString();
                    for (int j = 0; j < adet; j++)
                    {
                        dr = dtyeni.NewRow();
                        dr["Adet"] = 1;
                        dr["fkStokKarti"] = fkStokKarti;
                        dr["Stokadi"] = FisDetay.Rows[i]["Stokadi"].ToString(); ;
                        dr["StokGrup"] = FisDetay.Rows[i]["StokGrup"].ToString();
                        dr["BirimAdi"] = FisDetay.Rows[i]["BirimAdi"].ToString();
                        dr["Barcode"] = FisDetay.Rows[i]["Barcode"].ToString();
                        dr["UreticiKodu"] = FisDetay.Rows[i]["UreticiKodu"].ToString();

                        if (radioGroup1.SelectedIndex==0)
                            dr["SatisFiyati"] = FisDetay.Rows[i]["iskontolu_satisfiyati"].ToString();
                        else if (radioGroup1.SelectedIndex == 1)
                            dr["SatisFiyati"] = FisDetay.Rows[i]["SatisFiyati"].ToString();
                        else
                            dr["SatisFiyati"] = FisDetay.Rows[i]["SatisFiyati_Etiket"].ToString();

                        if (FisDetay.Rows[i]["SatisFiyati2"].ToString() == "")
                            dr["SatisFiyati2"] = "0";
                        else
                            dr["SatisFiyati2"] = FisDetay.Rows[i]["SatisFiyati2"].ToString();

                        if (FisDetay.Rows[i]["SatisFiyati3"].ToString() == "")
                            dr["SatisFiyati3"] = "0";
                        else
                            dr["SatisFiyati3"] = FisDetay.Rows[i]["SatisFiyati3"].ToString();

                        dr["koliicitanefiyati"] = FisDetay.Rows[i]["koliicitanefiyati"].ToString();
                        dr["TedarikciAdi"] = FisDetay.Rows[i]["TedarikciAdi"].ToString();
                        dr["EtiketAciklama"] = FisDetay.Rows[i]["EtiketAciklama"].ToString();
                        dr["Marka"] = FisDetay.Rows[i]["Marka"].ToString();
                        dr["Beden"] = FisDetay.Rows[i]["BedenKodu"].ToString();
                        dr["Renk"] = FisDetay.Rows[i]["RenkKodu"].ToString();
                        dr["ParaKodu"] = FisDetay.Rows[i]["ParaKodu"].ToString();
                        
                        string[] parts = FisDetay.Rows[i]["SatisFiyati"].ToString().Split(',');
                        string fi = "",dec="";
                        int k = 0;
                        foreach (string part in parts)
                        {
                            if(k==0)
                            fi = part;
                            if (k == 1)
                            {
                                if (part.Length==1)
                                    dec = part.Substring(0, 1);
                                else
                                dec = part.Substring(0, 2);
                            }
                            k++;
                        }
                        //dr["GizliFiyat"] = FisDetay.Rows[i]["UreticiKodu"].ToString() + "0" + fi + dec;

                        string sf1SatisFiyati =  FisDetay.Rows[i]["sf1SatisFiyati"].ToString();
                        if (sf1SatisFiyati.Length == 0)
                            sf1SatisFiyati = "0";
                        else if (sf1SatisFiyati.Length == 1)
                            sf1SatisFiyati = sf1SatisFiyati.Replace(",", "").Replace(".", "").Substring(0, 1);
                        else if (sf1SatisFiyati.Length == 2)
                            sf1SatisFiyati = sf1SatisFiyati.Replace(",", "").Replace(".", "").Substring(0, 2);
                        else if (sf1SatisFiyati.Length >2)
                            sf1SatisFiyati = sf1SatisFiyati.Replace(",", "").Replace(".", "").Substring(0, 2);

                        //if (fi.Length > 2)
                        //    fi = fi.Substring(0,3);
                        dr["GizliFiyat"] = fi + dec + "00" +sf1SatisFiyati;
                        //double.Parse(FisDetay.Rows[i]["SatisFiyati"].ToString().Replace(",", ".")).ToString("##0.###");

                        dr["Aciklama"] = FisDetay.Rows[i]["Aciklama"].ToString();
                        string resimyol = exedizini + "\\StokKartiResim\\"+fkStokKarti+".png";
                        dr["urunresmi"] = resimyol;
                        dr["KdvOrani"] = FisDetay.Rows[i]["KdvOrani"].ToString();

                        string satisfiyatikdvharic = FisDetay.Rows[i]["SatisFiyatiKdvHaric"].ToString();
                        if (satisfiyatikdvharic == "") satisfiyatikdvharic = "0";
                        dr["SatisFiyatiKdvHaric"] = satisfiyatikdvharic;

                        dr["SatisAdedi"] = FisDetay.Rows[i]["SatisAdedi"].ToString();
                        dr["icindekiadet"] = FisDetay.Rows[i]["icindekiadet"].ToString();
                        
                        dtyeni.Rows.Add(dr);
                    }
                }
                dtyeni.TableName = "FisDetay";
                ds.Tables.Add(dtyeni);

                DataTable Fis = DB.GetData(@"select * from EtiketBas with(nolock) where pkEtiketBas=" + tepkEtiketBas.Text);// + fisid);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");// + fisid);
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }

                //string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
                xrCariHareket rapor = new xrCariHareket();
                //XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = SatisFisTipi;
                rapor.Report.Name = SatisFisTipi;

                #region ResimEkle
                //string urunresmi = exedizini + "\\StokKartiResim\\7.jpg";
                //if (File.Exists(urunresmi))
                //{
                //    //rapor.PictureBox1.DataBindings.Add(New XRBinding("ImageUrl", ds, "Photo", ""))
                //    //XRPictureBox pictureBox = new XRPictureBox();
                //    //pictureBox.Image = Image.FromFile(urunresmi);
                //    //pictureBox.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                //    //pictureBox.WidthF = 50;//rapor.PageWidth - rapor.Margins.Left - rapor.Margins.Right;
                //    //pictureBox.HeightF = 50;// 500;
                //    //pictureBox.LeftF = rapor.PageWidth-50;
                //    //rapor.Bands[BandKind.Detail].Controls.Add(pictureBox);
                //    rapor.FindControl("pictureBox2", true).NavigateUrl= urunresmi;

                //}
                #endregion
                if (Disigner)
                    rapor.ShowDesigner();
                else
                {
                    if(yazdir)
                    rapor.Print(YaziciAdi);
                    else
                    rapor.ShowPreview();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void ProcessImageUrls(XtraReport report)
        {
            //foreach (XRPictureBox pictureBox in report.Controls<XRPictureBox>())
            {
                //if (!string.IsNullOrEmpty(pictureBox.ImageUrl))
                    //pictureBox.BeforePrint += pictureBox_BeforePrint;
            }
        }

        void pictureBox_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRPictureBox pictureBox = (XRPictureBox)sender;
            if (pictureBox.Image == null)
            {
                Uri uri = new Uri(pictureBox.ImageUrl);
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    webClient.Credentials = new System.Net.NetworkCredential("user", "password");
                    using (Stream stream = webClient.OpenRead(uri.AbsoluteUri))
                    {
                        if (stream != null)
                        {
                            using (Image image = Image.FromStream(stream))
                            {
                                if (image != null)
                                    pictureBox.Image = DevExpress.Utils.BitmapCreator.CreateBitmap(image, Color.Transparent);
                            }
                        }
                    }
                }
            }
        }

        void RaporOnizleme(bool Disigner)
        {
            string fisid = tepkEtiketBas.Text;
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
            string pkStokKartiid = dr["pkStokKartiid"].ToString();

            if (pkStokKartiid == "" || pkStokKartiid == "0")
                DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            else
                DB.pkStokKarti = int.Parse(pkStokKartiid);

            if (Degerler.StokKartiDizayn)
            {
                frmStokKartiLayout StokKarti = new frmStokKartiLayout();
                StokKarti.ShowDialog();
            }
            else
            {
                frmStokKarti StokKarti = new frmStokKarti();                             
                StokKarti.ShowDialog();
            }

            yesilisikyeni();
            //Satis1SonKayidaGit();
        }
        private void repositoryItemButtonEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                simpleButton4_Click(sender, e);
                //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                //xtraTabPage5_Click( sender, e);
            }

            //if (e.KeyCode == Keys.Space)
            //{
            //    ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            //    xtraTabPage5_Click(sender, e);
            //}
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
                float badet = 1;
                //string bbarkod = barkod;
                if (adetvar > 0)
                {
                    float.TryParse(kod.Substring(0, adetvar), out badet);
                    //bbarkod = barkod.Substring(adetvar + 1, barkod.Length - (adetvar + 1));

                }
                gridView1.SetFocusedRowCellValue("Adet", badet);
                gridView1.SetFocusedRowCellValue(gridColumn3, "1");
                gridView1.SetFocusedRowCellValue(gridColumn3, badet);
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
            if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["AlisFiyati"].ToString() != "")
            {
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                if (SatisTutar - AlisTutar <= 0 && dr["iade"].ToString() == "False")
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
                    DevExpress.Utils.Drawing.ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
                }
                catch (Exception exp)
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
            if (((SimpleButton)sender).Tag != null)
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
            simpleButton4_Click(sender, e);
        }

        //private void gridView1_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        //{
        //    if (e.Column.FieldName == "SatisFiyati")
        //        e.RepositoryItem = repositoryItemLookUpEdit1;
        //}

        void FisListesi()
        {
            string sql = @"SELECT top 50 s.pkEtiketBas,sum(sd.Adet* sd.SatisFiyati) as Tutar,s.Tarih,s.Aciklama,SUM(Adet) as Adet
            FROM EtiketBas s left JOIN EtiketBasDetay sd ON s.pkEtiketBas = sd.fkEtiketBas
            group by s.pkEtiketBas,s.Tarih,s.Aciklama
            order by s.pkEtiketBas desc";

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
            Satis1Toplam.Tag = lueFis.EditValue.ToString();
            EtiketBasDetayGetir(Satis1Toplam.Tag.ToString());
            
        }
        void SatisGetir()
        {
            string pkSatislar = "0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
           
            if (AcikSatisindex == 4)
                pkSatislar = Satis4Toplam.Tag.ToString();
            DataTable dtSatislar = DB.GetData("exec sp_Satislar " + pkSatislar);//fiş bilgisi
            if (dtSatislar.Rows.Count == 0)
            {
                Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            string fkfirma = dtSatislar.Rows[0]["fkFirma"].ToString();
            string firmaadi = dtSatislar.Rows[0]["Firmaadi"].ToString();
            if (AcikSatisindex == 1)
            {
                Satis1Firma.Tag = fkfirma;
                Satis1Firma.Text = fkfirma + "-" + firmaadi;
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

        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //iskonto tutar
            if (e.KeyCode == Keys.Enter)
            {
                string iskontotutar =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
                //iskontotutarDegistir(iskontotutar);
                yesilisikyeni();
            }
        }

        void TutarFont(DevExpress.XtraEditors.LabelControl secilen)
        {
            Satis1Baslik.Font = buttonkucukfont.Font;
          
            if (AcikSatisindex == 1)
                Satis1Baslik.Font = Satis4Baslik.Font;
           
            Satis1Toplam.Font = defaultfontkucuk.Font;
           
            Satis4Toplam.Font = defaultfontkucuk.Font;
            secilen.Font = defaultfontbuyuk.Font;
            odemepanel.BackColor = secilen.BackColor;
            gridView1.Appearance.Empty.BackColor = secilen.BackColor;
            gridView1.Appearance.Row.BackColor = secilen.BackColor;
            gridView1.Appearance.TopNewRow.BackColor = secilen.BackColor;
            gridView1.Appearance.HeaderPanel.BackColor = secilen.BackColor;
        }


        private void simpleButton8_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Program Kurulduğundaki ilk Ayarlara Dönülecek \n Onaylıyormusunuz?", "Hitit2012", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
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
            string pkSatis = "0";
            if (AcikSatisindex == 1) pkSatis = Satis1Toplam.Tag.ToString();
           
            if (pkSatis == "0")
            {
                Showmessage("Önce Etiket Listesi Yapınız!", "K");
                return;
            }
            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.memoozelnot.Text = btnAciklamaGirisi.ToolTip;
            fFisAciklama.ShowDialog();
            btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;

            DB.ExecuteSQL("UPDATE EtiketBas SET Aciklama='" + btnAciklamaGirisi.ToolTip + "' where pkEtiketBas=" + pkSatis);
            fFisAciklama.Dispose();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            //string str = ActiveControl.Name;
            //this.Dispose();
            Close();
        }


        public bool OnceSatisYapiniz()
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.Text = "Önce Etiket Listesi Yapınız!";
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.Show();
                return false;
            }
            return true;
        }
        private void btnyazdir_Click(object sender, EventArgs e)
        {
            if (OnceSatisYapiniz() == false) return;

            string pkEtiket = Satis1Toplam.Tag.ToString();

            //string pkEtiket = int.Parse(pkSatislar);

            string YaziciAdi = "", YaziciDosyasi = "";
            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,DosyaYolu  FROM EtiketSablonlari with(nolock) where pkEtiketSablonlari=" + searchLookUpEdit1.EditValue.ToString()+
            " and (fkKullanicilar=0 or fkKullanicilar="+DB.fkKullanicilar+")");
            //if (dtYazicilar.Rows.Count == 1)
            //{
            YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
            YaziciDosyasi = dtYazicilar.Rows[0]["DosyaYolu"].ToString();
            if (YaziciAdi != "")
            {
                EtiketBaskaydet(false, true);

                FisYazdirAdi(false, pkEtiket, YaziciDosyasi, YaziciAdi,true);

                SatisTemizle();

                FisListesi();

            }
            yesilisikyeni();
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
            
        }
        private void Satis4Toplam_Click(object sender, EventArgs e)
        {
            AcikSatisindex = 4;
            yesilisikyeni();
        }

        void SatisDuzenKapat()
        {
            gCSatisDuzen.Visible = false;
            tepkEtiketBas.EditValue = null;
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
            string pkSatisDetay = dr["pkEtiketBasDetay"].ToString();
            DB.ExecuteSQL("UPDATE EtiketBasDetay SET Adet=" + yenimiktar.Replace(",", ".") + " where pkEtiketBasDetay=" + pkSatisDetay);
            //decimal iskontoyuzde = 0;
            //if (dr["iskontoyuzdetutar"].ToString() != "")
            //    iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
            //decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());
            //decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
            //decimal Miktar = Convert.ToDecimal(yenimiktar);
            //decimal iskontogercektutar = Convert.ToDecimal(dr["iskontotutar"].ToString());

            //if (iskontogercektutar > 0)
            //{
            //    iskontogercekyuzde = (iskontogercektutar * 100) / (Fiyat * Miktar);
            //}
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn3, yenimiktar);
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn33, iskontogercekyuzde.ToString());

        }
        private void simpleButton8_Click_1(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("65");//20
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
            if (TabHizliSatisGenel.Tag == "0")
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

                if (tepkEtiketBas.Text == "") return;
                lueFis.EditValue = int.Parse(tepkEtiketBas.Text);
                if (lueFis.EditValue == null)
                    lueFis.EditValue = tepkEtiketBas.Text;
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

        void EtiketBasDetayGetir(string pkSatislar)
        {
            tepkEtiketBas.Text = pkSatislar;
            gridControl1.DataSource = DB.GetData("exec sp_EtiketBas " + pkSatislar + ",0");
            gridView1.AddNewRow();
            //yesilisikyeni();
        }

        void EtiketBasDetayEkle(string barkod)
        {
            //decimal Adet = 1;
            decimal EklenenMiktar = 1;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr != null && dr["pkEtiketBasDetay"].ToString()!="")
            {
                yesilisikyeni();
                return;
            }

            if (dr == null)
                EklenenMiktar = 1;
            else
            {
                if (dr["Adet"].ToString() != "")
                    EklenenMiktar = decimal.Parse(dr["Adet"].ToString());
            }

            if (dr != null && dr["fkAlisDetay"].ToString() != "") return; 

            if (barkod == "") return;

            YeniSatisEkle();

            if (barkod.Length == 3)
                barkod = (1 * float.Parse(barkod)).ToString();

            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti FROM StokKarti with(nolock) where Barcode='" + barkod + "'");
            if (dtStokKarti.Rows.Count == 0)
            {
                //string Barcode = "";
                decimal SatisAdedi = 1;
                dtStokKarti = DB.GetData("SELECT sk.pkStokKarti,skb.SatisAdedi,sk.Barcode FROM StokKartiBarkodlar  skb with(nolock)" +
                " inner join StokKarti sk with(nolock) on sk.pkStokKarti=skb.fkStokKarti  where skb.Barkod='" + barkod + "'");
                if (dtStokKarti.Rows.Count > 0)
                {
                    SatisAdedi = decimal.Parse(dtStokKarti.Rows[0]["SatisAdedi"].ToString());
                    EklenenMiktar = EklenenMiktar * SatisAdedi;
                    DB.pkStokKarti = int.Parse(dtStokKarti.Rows[0]["pkStokKarti"].ToString());
                    barkod = dtStokKarti.Rows[0]["Barcode"].ToString();
                }
                else
                {
                    frmStokKarti StokKartiHizli = new frmStokKarti();
                    Degerler.stokkartisescal = true;
                    DB.pkStokKarti = 0;
                    StokKartiHizli.lblBarkod.AccessibleDescription = "alisdangeldievet";
                    StokKartiHizli.Barkod.EditValue = barkod;
                    StokKartiHizli.ShowDialog();

                    dtStokKarti = DB.GetData("select pkStokKarti From StokKarti with(nolock) WHERE Barcode='" + StokKartiHizli.Barkod.EditValue.ToString() + "'");
                    //eğer stok kartı oluşturmadı ise 
                    if (dtStokKarti.Rows.Count == 0)
                    {
                        yesilisikyeni();
                        StokKartiHizli.Dispose();
                        return;
                    }
                    StokKartiHizli.Dispose();
                }
                
            }
            string pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkAlislar", tepkEtiketBas.Text));
            arr.Add(new SqlParameter("@SatisFiyatGrubu", "1"));
            arr.Add(new SqlParameter("@Adet", EklenenMiktar.ToString().Replace(",", ".")));
            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
            arr.Add(new SqlParameter("@iskontoyuzde", "0"));
            arr.Add(new SqlParameter("@iskontotutar", "0"));
            string s = DB.ExecuteScalarSQL("exec sp_EtiketBasDetay_Ekle @fkAlislar,@SatisFiyatGrubu,@Adet,@fkStokKarti,@iskontoyuzde,@iskontotutar", arr);
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
            if (AcikSatisindex == 1)
                pkFirma = Satis1Firma.Tag.ToString();
            
            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Siparis", "0"));
            list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("@fkSatisDurumu", "0"));
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", "0"));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));//iskontoTutar.Value.ToString().Replace(",", ".")));

            sql = "INSERT INTO EtiketBas (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar) SELECT IDENT_CURRENT('EtiketBas')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                EtiketBasDetayGetir(Satis1Toplam.Tag.ToString());
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Bilgileri ve Genel Durumu. Yapım Aşamasındadır...");
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(0,0);
            YaziciAyarlari.Tag = 2;
            YaziciAyarlari.ShowDialog();

            string s = searchLookUpEdit1.EditValue.ToString();

            EtiketSablonlari();

            searchLookUpEdit1.EditValue = int.Parse(s);
        }

        private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmiskonto iskonto = new frmiskonto();
            iskonto.fkSatisDetay.Text = dr["pkEtiketBas"].ToString();
            iskonto.ceBirimFiyati.Value = decimal.Parse(dr["AlisFiyati"].ToString());
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
                else if (dr["pkEtiketBasDetay"].ToString() == "")
                    gridView1.DeleteRow(i);
            }
        }

        private void sonrakiSatisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AcikSatisindex == 1) AcikSatisindex = 2;
            else if (AcikSatisindex == 2) AcikSatisindex = 3;
            else if (AcikSatisindex == 3) AcikSatisindex = 1;
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
                if (dr == null) return;
                string pkAlisDetay = dr["pkAlisDetay"].ToString();
                string pkStokKarti = dr["fkStokKarti"].ToString();
                DB.ExecuteSQL("UPDATE AlisDetay SET AlisFiyati=" + g.Replace(",", ".") + " where pkAlisDetay=" + pkAlisDetay);
                DB.ExecuteSQL("UPDATE StokKarti SET AlisFiyati=" + g.Replace(",", ".") + " where pkStokKarti=" + pkStokKarti);
                decimal iskontoyuzde = 0;
                if (dr["iskontoyuzdetutar"].ToString() != "")
                    iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
                decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());
                decimal Fiyat = Convert.ToDecimal(dr["AlisFiyati"].ToString());
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
            tepkEtiketBas.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            DataTable dt = DB.GetData("select pkEtiketBas,fkFirma from EtiketBas with(nolock) where Siparis<>1 order by pkEtiketBas desc");
            int c = dt.Rows.Count;
            if (c > 0)
            {
                Showmessage("Ekranda tamamlanmamış " + c.ToString() + " Adet işleminiz bulumnaktadır.", "K");
                //for (int i = 0; i < c; i++)
                //{
                string pkSatislar = dt.Rows[0]["pkEtiketBas"].ToString();

                if (alisfaturasindangelenfisno.Text == "")
                    Satis1Toplam.Tag = pkSatislar;
                else
                    Satis1Toplam.Tag = alisfaturasindangelenfisno.Text;
                AcikSatisindex = 1;

                EtiketBasDetayGetir(Satis1Toplam.Tag.ToString());

                if (Barkod.Text != "")
                    EtiketBasDetayEkle(Barkod.Text);

                yesilisikyeni();

                return;
            }
            EtiketBasDetayGetir("0");

            if (Barkod.Text != "")
                EtiketBasDetayEkle(Barkod.Text);

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
            SatisFiyatlari.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            SatisFiyatlari.ShowDialog();
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
        }

        private void Satis2Toplam_Click(object sender, EventArgs e)
        {
           
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

        void EtiketBasDetayYenile()
        {
            if (AcikSatisindex == 1)
            {
                EtiketBasDetayGetir(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
            }
           
           
            else if (AcikSatisindex == 4)
            {
                EtiketBasDetayGetir(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
            }
        }

        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;
            string iskontotutar =
               ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
            //iskontotutarDegistir(iskontotutar);
            int i = gridView1.FocusedRowHandle;

            EtiketBasDetayYenile();

            gridView1.FocusedRowHandle = i;
        }

        private void müşteriKArtıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AcikSatisindex == 1)
                DB.PkFirma = int.Parse(Satis1Firma.Tag.ToString());
           

            frmTedarikciKarti MusteriKarti = new frmTedarikciKarti(DB.PkFirma.ToString());
            
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
        private void searchLookUpEdit1View_RowClick(object sender, RowClickEventArgs e)
        {
            GridHitInfo ghi = searchLookUpEdit1View.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            string FisNo = "0";

            FisNo = Satis1Toplam.Tag.ToString();

            if (ghi.Column.Name == "gridColumn12")//dizayn
            {
                DataRow dr = searchLookUpEdit1View.GetDataRow(searchLookUpEdit1View.FocusedRowHandle);
                FisYazdirAdi(true, FisNo, dr["DosyaYolu"].ToString(), dr["YaziciAdi"].ToString(),false);
            }
            if (ghi.Column.Name == "gridColumn10")//ön izleme
            {
                DataRow dr = searchLookUpEdit1View.GetDataRow(searchLookUpEdit1View.FocusedRowHandle);
                FisYazdirAdi(false, FisNo, dr["DosyaYolu"].ToString(), dr["YaziciAdi"].ToString(),false);
            }
        }
        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            EtiketBaskaydet(true, false);
        }

        private void repositoryItemCalcEdit3_Leave(object sender, EventArgs e)
        {
            string yenimiktar =
               ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
            MiktarDegistir(yenimiktar);
           // yesilisikyeni();   
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle ;

            if (i < 0) return;
            
            DataRow dr = gridView1.GetDataRow(i);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["fkStokKarti"].ToString();
            StokHareketleri.ShowDialog();

            gridView1.FocusedRowHandle = i;
            
        }

        private void repositoryItemCalcEdit4_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;

            string _SatisFiyati_Etiket =
               ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();

            //if (gridView1.FocusedRowHandle < 0)
            //{
            //    yesilisikyeni();
            //    return;
            //}
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkEtiketBasDetay = dr["pkEtiketBasDetay"].ToString();

            DB.ExecuteSQL("UPDATE EtiketBasDetay SET SatisFiyati=" + _SatisFiyati_Etiket.Replace(",", ".") + " where pkEtiketBasDetay=" + fkEtiketBasDetay);

            int i = gridView1.FocusedRowHandle;

            EtiketBasDetayYenile();

            gridView1.FocusedRowHandle = i;
        }

        private void repositoryItemCalcEdit4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                gridView1.Focus();
                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            }
                //EtiketBasDetayYenile();
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\EtiketBasGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\EtiketBasGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void deTarihi_Leave(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkEtiketBas", tepkEtiketBas.Text));
            list.Add(new SqlParameter("@Tarih", deTarihi.DateTime));

            string sonuc = DB.ExecuteSQL("UPDATE EtiketBas SET Tarih=@Tarih where pkEtiketBas=@pkEtiketBas", list);

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {
            deTarihi.DateTime = DateTime.Now;
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkEtiketBas", tepkEtiketBas.Text));
            list.Add(new SqlParameter("@Tarih", deTarihi.DateTime));

            string sonuc = DB.ExecuteSQL("UPDATE EtiketBas SET Tarih=@Tarih where pkEtiketBas=@pkEtiketBas", list);
        }

        private void yeniStokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnYeniStokEkle_Click(sender,  e);
                return;

            string barkod = "";
            frmStokKarti StokKarti = new frmStokKarti();
            Degerler.stokkartisescal = false;
            DB.pkStokKarti = 0;
            //StokKartiHizli.alisdangeldi.AccessibleDescription = "alisdangeldievet";
            StokKarti.Barkod.EditValue = barkod;
            StokKarti.lblBarkod.Tag = barkod;
            StokKarti.ShowDialog();
            Degerler.stokkartisescal = true;
            //if (StokKartiHizli.TopMost == true)
            // {
            DataTable dtStokKarti = DB.GetData("select pkStokKarti,AlisFiyati,AlisFiyatiKdvHaric From StokKarti with(nolock) WHERE Barcode='" +
                StokKarti.Barkod.EditValue.ToString() + "'");
            // }
            //else
            //eğer stok kartı oluşturmadı ise 
            if (dtStokKarti.Rows.Count == 0)
            {
                yesilisikyeni();
                StokKarti.Dispose();
                return;
            }

            StokKarti.Dispose();

            //if (gridView1.DataRowCount == 0)
            //    YeniAlisEkle();

           
           string stok_id = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            DataTable dt = 
            DB.GetData("select * from Stokkarti with(nolock) where pkStokKarti=" + stok_id);

            if (dt.Rows.Count > 0)
                EtiketBasDetayEkle(dt.Rows[0]["barcode"].ToString());

            yesilisikyeni();
        }

        private void btnYeniStokEkle_Click(object sender, EventArgs e)
        {
            string barkod = "";
            if (Degerler.StokKartiDizayn)
            {
                frmStokKartiLayout StokKarti = new frmStokKartiLayout();
                DB.pkStokKarti = 0;
                //Degerler.StokKartiKapatEkle = true;
                Degerler.stokkartisescal = false;
                StokKarti.ShowDialog();
                //Degerler.StokKartiKapatEkle = false;
                if (StokKarti.Barkod.EditValue == null)
                {
                    yesilisikyeni();
                    StokKarti.Dispose();
                    return;
                }
                barkod = StokKarti.Barkod.EditValue.ToString();
            }
            else
            {
                frmStokKarti StokKarti = new frmStokKarti();
                DB.pkStokKarti = 0;
                Degerler.StokKartiKapatEkle = true;
                Degerler.stokkartisescal = false;
                StokKarti.lblBarkod.Tag = barkod;
                StokKarti.Barkod.EditValue = barkod;
                StokKarti.ShowDialog();

                Degerler.StokKartiKapatEkle = false;

                if (Degerler.StokKartiF7Ekle == false)
                {
                    yesilisikyeni();
                    StokKarti.Dispose();
                    return;
                }
                barkod = StokKarti.Barkod.EditValue.ToString();
                StokKarti.Dispose();
            }
        }

        private void frmEtiketBas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void lueFis_QueryCloseUp(object sender, CancelEventArgs e)
        {
            yesilisikyeni();
        }
    }
}

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
using GPTS.Include.Data;
using DevExpress.XtraTab;
using GPTS.islemler;
using System.Diagnostics;
using DevExpress.XtraPrinting.Control;

namespace GPTS
{
    public partial class frmFaturaOzel : DevExpress.XtraEditors.XtraForm
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis = "";//AccessibleDescription
        int AcikSatisindex = 1;//hangi satış açık
        decimal HizliMiktar = 1;
        bool ilkyukleme = false;

        string pkFirma = "1";
        public frmFaturaOzel(string fkFirma)
        {

            InitializeComponent();

            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;

            pkFirma = fkFirma;
        }

        private void ucAnaEkran_Load(object sender, EventArgs e)
        { 
            ilkyukleme = true;
            Fiyatlarigetir();
            HizliSatisTablariolustur();
            Yetkiler();
            lueSatisTipi.Properties.DataSource = DB.GetData(@"SELECT pkSatisDurumu, Durumu, Aktif, SiraNo FROM  SatisDurumu with(nolock) WHERE Aktif = 1 ORDER BY SiraNo");
            lueSatisTipi.EditValue = 4;
            GeciciMusteriDefault();
            musteriata(pkFirma);
            timer1.Enabled = true;
            cbOdemeSekli.SelectedIndex = 0;
            lueFiyatlar.EditValue = 1;
            ilkyukleme = false;
            KullaniciListesi();

            //string Dosya = DB.exeDizini + "\\SatisFaturaGrid.xml";

            //if (File.Exists(Dosya))
            //{
            //    gridView1.RestoreLayoutFromXml(Dosya);
            //    gridView1.ActiveFilter.Clear();
            //}
        }

        void KullaniciListesi()
        {
            lueKullanicilar.Properties.DataSource = DB.GetData("select pkKullanicilar,adisoyadi,KullaniciAdi from Kullanicilar  with(nolock) where durumu=1 ");
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
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
FROM  YetkiAlanlari with(nolock)  INNER JOIN Parametreler with(nolock)  ON YetkiAlanlari.fkParametreler = Parametreler.pkParametreler
WHERE  Parametreler.fkModul = 1  and YetkiAlanlari.fkKullanicilar =" + DB.fkKullanicilar;
            DataTable dtYetkiler = DB.GetData(sql);
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "ParaPanel")
                //    ParaPanel.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);

                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
                    gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
                    xtraTabControl3.Visible = true;
                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizButGen")
                {
                    int gen = 117;
                    int.TryParse(dtYetkiler.Rows[i]["Sayi"].ToString(),out gen);
                    dockPanel1.Tag = gen;
                    if (gen > 0)
                    {
                        dockPanel1.Width = gen;
                        dockPanel1.SendToBack();
                        lcKullanici.Tag = "1";
                    }
                }
            }
        }

        void Fiyatlarigetir()
        {
            lueFiyatlar.Properties.DataSource = DB.GetData("select * from SatisFiyatlariBaslik with(nolock) where Aktif=1 order by pkSatisFiyatlariBaslik");
            if (!ilkyukleme)
                lueFiyatlar.EditValue = 1;
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
                FaturalarDetayEkle(((SimpleButton)sender).Tag.ToString());
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
            DataTable dtbutton = DB.GetData(@"select pkHizliStokSatis,pkStokKarti,Barcode,HizliSatisAdi,Stokadi,sf.SatisFiyatiKdvli as SatisFiyati from HizliStokSatis  h with(nolock)
            left join (select pkStokKarti,Stokadi,fkStokGrup,HizliSatisAdi,Barcode,HizliSiraNo 
            from StokKarti with(nolock) ) sk on sk.pkStokKarti=h.fkStokKarti 
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
                sb.Tag = Barcode;
                double d = 0;
                double.TryParse(SatisFiyati, out d);
                sb.ToolTip = "Satış Fiyatı=" + d.ToString() + "\n Stok Adı:" + Stokadi;
                sb.ToolTipTitle = "Kodu: " + Barcode;
                sb.Height = h;
                sb.Width = w;
                sb.Click += new EventHandler(ButtonClick);
                sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseEnter);
                sb.Left = lef;
                sb.Top = to;
                //adı 15 karakterden büyükse
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
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Fatura İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle

            int sonuc;
            string pkFaturalar = "0";
            if (AcikSatisindex == 1)
                pkFaturalar = Satis1Toplam.Tag.ToString();
            if (AcikSatisindex == 4)
                pkFaturalar = Satis4Toplam.Tag.ToString();
            string sql = "delete from FaturalarDetay where fkFaturalar=" + pkFaturalar;
            sonuc = DB.ExecuteSQL(sql);
            sql = "delete from Faturalar where pkFaturalar =" + pkFaturalar;
            sonuc = DB.ExecuteSQL(sql);

            //if (sonuc != "Satis Silindi.")
              //  MessageBox.Show(sonuc);

            if (AcikSatisindex == 1)
            {
                lcBakiye1.Text = "0,00";
                lcBakiye1.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 2)
            {
                lcBakiye2.Text = "0,00";
                lcBakiye2.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 3)
            {
                lcBakiye3.Text = "0,00";
                lcBakiye3.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 4)
            {
                lcBakiye4.Text = "0,00";
                lcBakiye4.BackColor = System.Drawing.Color.Transparent;
            }
            pkSatisBarkod.Text = "0";
            SatisTemizle();
            Satis1ToplamGetir();
            deGuncellemeTarihi.EditValue = null;
            deGuncellemeTarihi.Enabled = false;
            cbOdemeSekli.Enabled = false;
            yesilisikyeni();
            lueSatisTipi.EditValue = 2;
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("DELETE FROM FaturalarDetay WHERE pkFaturalarDetay=" + dr["pkFaturalarDetay"].ToString());
            //DB.ExecuteSQL("update StokKarti set Mevcut=Mevcut+" + dr["Adet"].ToString().Replace(",", ".") + "  where pkStokKarti=" + dr["fkStokKarti"].ToString());
            gridView1.DeleteSelectedRows();
            if (gridView1.DataRowCount == 0)
            {
                DB.ExecuteSQL("DELETE FROM Faturalar WHERE pkFaturalar=" + pkSatisBarkod.Text);
                pkSatisBarkod.Text = "0";
                if (AcikSatisindex == 1)
                    Satis1Toplam.Tag = "0";

                deGuncellemeTarihi.EditValue = null;
                deGuncellemeTarihi.Enabled = false;
                cbOdemeSekli.Enabled = false;

                lueSatisTipi.EditValue = 2;
                lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
            }

            yesilisikyeni();

        }
        void YeniFaturaEkle()
        {
            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
                YeniFatura();
            else if (AcikSatisindex == 4 && Satis4Toplam.Tag.ToString() == "0")
                YeniFatura();
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
            if (e.KeyCode == Keys.Enter)
            {
                string kod =
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (kod == "" && gridView1.DataRowCount == 0)
                {
                    yesilisikyeni();
                    return;
                }
                FaturalarDetayEkle(kod);
                yesilisikyeni();
            }
            //müşteri koduna git
            else if (e.KeyValue == 77)
            {
                if (AcikSatisindex == 1) Satis1Baslik.Focus();
            }
            else if (e.KeyValue == 70)
            {
                lueFiyatlar.Focus();
                //DevExpress.XtraEditors.LookUpEdit lookUpEdit = sender as DevExpress.XtraEditors.LookUpEdit;
                //if (lookUpEdit != null)
                //BeginInvoke(new MethodInvoker(delegate { lueFiyatlar.ShowPopup(); }));
            }
            //ctrl + i iskonto
            else if (e.Control && e.KeyValue == 222)
            {
                //iskontoyagit_ctrli();
            }
            else if (e.Control && e.Shift && gridView1.DataRowCount > 0)
            {
                //üst satırı kopyala
                DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);
                FaturalarDetayEkle(dr["Barcode"].ToString());
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
                for (int i = 0; i < StokAra.gridView1.DataRowCount; i++)
                {
                    DataRow dr = StokAra.gridView1.GetDataRow(i);
                    if (dr["Sec"].ToString() == "True")
                        FaturalarDetayEkle(dr["Barcode"].ToString());
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
        private void urunekle(string barkod)
        {
            if (barkod == "") return;
            string ilktarih = "";
            string sontarih = "";
            string fkUrunlerNoPromosyon = "";
            int DususAdet = 1;
            DataTable dturunler = DB.GetData("select pkStokKarti From StokKarti with(nolock) WHERE Barcode='" + barkod + "'");
            if (dturunler.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
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
            FaturalarDetayEkle(barkod);
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
            lcBakiye1.Visible = false;
            lcBakiye2.Visible = false;
            lcBakiye3.Visible = false;
            lcBakiye4.Visible = false;
            string fkFirma = "0";
            if (AcikSatisindex == 1)
            {
                SatisDetayGetir(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
                fkFirma = Satis1Firma.Tag.ToString();
                lcBakiye1.Visible = true;
            }
            else if (AcikSatisindex == 4)
            {
                SatisDetayGetir(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
                fkFirma = gCSatisDuzen.Tag.ToString();
                lcBakiye4.Visible = true;
            }
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();

            if (AcikSatisindex == 1) fontayarla(Satis1Toplam);
            if (AcikSatisindex == 4) fontayarla(Satis4Toplam);

            if (targetGrid != null)
                targetGrid.Visible = false;

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

        void satiskaydet(bool yazdir, bool odemekaydedildi,string EskiBakiye,string SatisTipi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkFaturalar", pkSatisBarkod.Text));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.EditValue.ToString()));
            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            
            if (FaturaTeklifTarihi.EditValue == null)
                list.Add(new SqlParameter("@FaturaTarihi", DBNull.Value));
            else
                list.Add(new SqlParameter("@FaturaTarihi", FaturaTeklifTarihi.DateTime));

            if (lueSatisTipi.EditValue.ToString() == "1")
            {
                if (FaturaTeklifTarihi.EditValue == null)
                    FaturaTeklifTarihi.DateTime = DateTime.Today;
                list.Add(new SqlParameter("@TeklifTarihi", FaturaTeklifTarihi.DateTime));
            }
            else
            {
                if (FaturaTeklifTarihi.EditValue == null)
                    FaturaTeklifTarihi.DateTime = DateTime.Today;
                list.Add(new SqlParameter("@TeklifTarihi", FaturaTeklifTarihi.DateTime));
            }

            list.Add(new SqlParameter("@FaturaNo", teFaturaNo.Text));
            
            string sql = "";
            if (AcikSatisindex == 4)
                sql = @"UPDATE Faturalar SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,
                OdemeSekli=@OdemeSekli,FaturaTarihi=@FaturaTarihi,TeklifTarihi=@TeklifTarihi,SonislemTarihi=getdate(),
                BilgisayarAdi=@BilgisayarAdi,FaturaNo=@FaturaNo where pkFaturalar=@pkFaturalar";
            else
            {
                if(deGuncellemeTarihi.EditValue==null)
                    sql = @"UPDATE Faturalar SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,
                OdemeSekli=@OdemeSekli,GuncellemeTarihi=getdate(),FaturaTarihi=@FaturaTarihi,TeklifTarihi=@TeklifTarihi,
                SonislemTarihi=getdate(),BilgisayarAdi=@BilgisayarAdi,FaturaNo=@FaturaNo where pkFaturalar=@pkFaturalar";
                else
                {
                    list.Add(new SqlParameter("@GuncellemeTarihi", deGuncellemeTarihi.DateTime));
                    sql = @"UPDATE Faturalar SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,
                    OdemeSekli=@OdemeSekli,GuncellemeTarihi=@GuncellemeTarihi,FaturaTarihi=@FaturaTarihi,
                    SonislemTarihi=getdate(),BilgisayarAdi=@BilgisayarAdi where pkFaturalar=@pkFaturalar";
                }
            }
            string sonuc = DB.ExecuteSQL(sql, list);
            if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            {
                Showmessage("Hata Oluştur: " + sonuc, "K");
                return;
            }

            //Alacak pkFirma olmadığı için OdemeKaydet procedure içine alındı 
            //if (SatisTipi != "Teklif")
            //   OdemeKaydet(odemekaydedildi, EskiBakiye, SatisTipi);
            //Satış durumu kaydet
            sql = @"declare @iadeAdedi int,@SatisAdedi int
                    set @iadeadedi=(select count(*) from FaturaDetay with(nolock)
                    where iade=1 and fkFaturalar=@fkSatislar)
                    set @SatisAdedi=(select count(*) from FaturaDetay with(nolock)
                    where fkFaturalar=@fkFaturalar)
                    if(@SatisAdedi>@iadeadedi and @iadeadedi>0)
                    update Faturalar set fkSatisDurumu=5 where pkFaturalar=@fkFaturalar
                    else if(@SatisAdedi=@iadeadedi)
                    update Faturalar set fkSatisDurumu=9 where pkFaturalar=@fkFaturalar
                    else
                    update Faturalar set fkSatisDurumu=2 where pkFaturalar=@fkFaturalar";
            if (SatisTipi == "Teklif")
                sql = "update Faturalar set fkSatisDurumu=1 where pkFaturalar=@fkFaturalar";
            sql = sql.Replace("@fkFaturalar", pkSatisBarkod.Text);
            DB.ExecuteSQL(sql);
            SatisTemizle();
        }
        
        void GeciciMusteriDefault()
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar with(nolock) where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.Lütfen Yetkili Firmayı Arayınız");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = dtMusteriler.Rows[0]["OzelKod"].ToString() + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
                Satis1Baslik.Text = DB.FirmaAdi;
                Satis1Firma.Tag = DB.PkFirma;
            }
        }
        
        void temizle(int aciksatisno)
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar with(nolock) where GeciciMusteri=1");
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

            btnAciklamaGirisi.ToolTip = "";
            //lueSatisTipi.EditValue = 2;
            ceiskontoTutar.EditValue = null;
            ceiskontoyuzde.EditValue = null;
            ceiskontoyuzde.Properties.NullText = "";
            gcSatisDetay.DataSource = null;
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
            satiskaydet(yazdir, odemekaydedildi, "0", lueSatisTipi.Text);
            //FisListesi();
            SatisDuzenKapat();
            // temizle(AcikSatisindex);
            yesilisikyeni();
        }

        public bool bir_nolumusteriolamaz()
        {
            if (DB.GetData("select * from Sirketler").Rows[0]["MusteriZorunluUyari"].ToString() == "True")
            {
                if (AcikSatisindex == 1 && Satis1Firma.Tag.ToString() == "1")
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Müşteri Olamaz.\n (Ayarlardan Kaldırabilirsiniz.)!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    musteriara();
                    return false;
                }
            }
            return true;

        }

        public bool bir_acikhesapolamaz()
        {
            if (((AcikSatisindex == 1 && Satis1Firma.Tag.ToString() == "1") &&
                (AcikSatisindex == 4 && gCSatisDuzen.Tag.ToString() == "1")) && cbOdemeSekli.Text == "Açık Hesap")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Müşteri Açık Hesap Olamaz", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                musteriara();
                return false;
            }
            return true;
        }

        public bool pro_kaydetyazdir(string btn_kaydet_yazdir, string SatisTipi )
        {
            string fkFirma = "1";
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        fkFirma = Satis1Firma.Tag.ToString();
                        break;
                    }
                case 4:
                    {
                        fkFirma = gCSatisDuzen.Tag.ToString();
                        break;
                    }
            }
            if (bir_nolumusteriolamaz() == false) return false;

            string EskiBakiye=
            DB.GetData("select isnull(dbo.fon_MusteriBakiyesi(" + fkFirma + "),0) as OncekiBakiye").Rows[0][0].ToString();
            
            EskiBakiye = EskiBakiye.Replace(",", ".");
            
            if (btn_kaydet_yazdir == "kaydet")
                satiskaydet(true, false, EskiBakiye, SatisTipi);
            else if (btn_kaydet_yazdir == "yazdir")
            {
                string pkFaturalar = "0";
                if (AcikSatisindex == 1)
                    pkFaturalar = Satis1Toplam.Tag.ToString();
                else if (AcikSatisindex == 4)
                    pkFaturalar = Satis4Toplam.Tag.ToString();
                DB.pkSatislar = int.Parse(pkFaturalar);
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
                    satiskaydet(true, false, EskiBakiye, SatisTipi);
                    FisYazdir(false, pkFaturalar, YaziciDosyasi, YaziciAdi, EskiBakiye);
                }
            }
            if (AcikSatisindex == 1)
            {
                lcBakiye1.Text = "0,00";
                lcBakiye1.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 2)
            {
                lcBakiye2.Text = "0,00";
                lcBakiye2.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 3)
            {
                lcBakiye3.Text = "0,00";
                lcBakiye3.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 4)
            {
                lcBakiye4.Text = "0,00";
                lcBakiye4.BackColor = System.Drawing.Color.Transparent;
            }
            DB.ExecuteSQL("UPDATE Firmalar Set SonSatisTarihi=getdate() where pkFirma=" + fkFirma);
            return true;
        }
        void SatisKaydetveyaYazdir(string KaydetmiYazdirmi)
        {
            if (pkSatisBarkod.Text == "0")
            {
                yesilisikyeni();
                return;
            }
            if (FaturaTeklifTarihi.EditValue == null)
            {
                formislemleri.Mesajform("Lütfen Fatura Tarihih Girirniz.", "K", 200);
                FaturaTeklifTarihi.Focus();
                return;
            }
            if (teFaturaNo.Text == "")
            {
                formislemleri.Mesajform("Lütfen Fatura No Girirniz.", "K", 200);
                teFaturaNo.Focus();
                return;
            }
            
            if (pro_kaydetyazdir(KaydetmiYazdirmi, lueSatisTipi.Text))
            {
                if (AcikSatisindex == 4)
                    Satis1ToplamGetir();
                deGuncellemeTarihi.EditValue = null;
                deGuncellemeTarihi.Enabled = false;
                cbOdemeSekli.Enabled = false;

                yesilisikyeni();

                lueSatisTipi.EditValue = 2;
                lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
            }
        }

        private void simpleButton37_Click(object sender, EventArgs e)
        {
            SatisKaydetveyaYazdir("kaydet");
        }
        private void btnyazdir_Click(object sender, EventArgs e)
        {
            SatisKaydetveyaYazdir("yazdir");
        }
        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }
        void FisYazdir(bool Disigner, string pkFaturalar, string SatisFisTipi, string YaziciAdi, string EskiBakiye)
        {
            try
            {
                string fisid = pkFaturalar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler");
                Sirket.TableName = "Sirket";
                
                
                ds.Tables.Add(Sirket);
                //Bakiye bilgileri
                //DataTable Bakiye = DB.GetData("exec sp_MusteriBakiyesi " + fkFirma + ",0");
                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData("select "+EskiBakiye+ @" as OncekiBakiye,sd.ToplamTutar as FisTutar,s.ToplamTutar,
dbo.fon_MusteriBakiyesi(s.fkFirma) as Bakiye,s.OdemeSekli
 from Faturalar s with(nolock)
left join (select fkFaturalar,sum(SatisFiyati-iskontotutar*Adet) as ToplamTutar from SatisDetay  with(nolock) group by fkSatislar) sd  on sd.fkSatislar=s.pkSatislar 
left join (select fkFaturalar,sum(Borc) as Borc from KasaHareket with(nolock) group by fkSatislar) kh  on kh.fkSatislar=s.pkSatislar
where pkSatislar=" + fisid);
                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);
                //Firma bilgileri
                DataTable Musteri = DB.GetData("select * from Firmalar where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                //new xrCariHareket().DataSource = ds;
                //new xrCariHareket().LoadLayout(RaporDosyasi);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = SatisFisTipi;
                rapor.Report.Name = SatisFisTipi;
                if (Disigner)
                    //new xrCariHareket().ShowDesigner();
                    rapor.ShowDesigner();
                else
                    //new xrCariHareket().Print(YaziciAdi);//.ShowPreview();
                    rapor.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
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
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,fd.Adet,fd.SatisFiyati,fd.iskontotutar,fd.iskontoyuzde,fd.Tarih FROM Faturalar f with(nolock)
inner join FaturaDetay fd on fd.fkFaturalar=f.pkFaturalar
inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
where f.pkFaturalar =" + fisid;
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
            string fkFirma = "1", ozelkod = "0", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        fkFirma = Satis1Firma.Tag.ToString();
                        break;
                    }
                case 4:
                    {
                        fkFirma = gCSatisDuzen.Tag.ToString();
                        break;
                    }
            }
            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.fkFirma.Tag = fkFirma;
            MusteriAra.ShowDialog();
            if (MusteriAra.fkFirma.Tag.ToString() == fkFirma) 
            {
                //BakiyeGetirSecindex(fkFirma);//yavaşlatıyor
                //FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
                return fkFirma;
            }
            fkFirma = MusteriAra.fkFirma.Tag.ToString();

            if (pkSatisBarkod.Text != "0" || pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE Faturalar SET fkFirma=" + fkFirma + " where pkFaturalar=" + pkSatisBarkod.Text);
            }
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod,isnull(fkSatisFiyatlariBaslik,1) as fkSatisFiyatlariBaslik from Firmalar with(nolock) where pkFirma=" + fkFirma);
            lueFiyatlar.EditValue = int.Parse(dt.Rows[0]["fkSatisFiyatlariBaslik"].ToString());
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
                case 4:
                    {
                        Satis4Baslik.Tag = fkFirma;
                        Satis4Baslik.Text = ozelkod + "-" + firmadi;
                        Satis4Baslik.ToolTip = Satis4Baslik.Text;
                        gCSatisDuzen.Tag = fkFirma;
                        break;
                    }
                default:
                    break;
            }
            BakiyeGetirSecindex(fkFirma);
            yesilisikyeni();
            //FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
            return fkFirma;
        }
        void BakiyeGetirSecindex(string pkFirma)
        {
            DataTable dt = DB.GetData("exec sp_MusteriBakiyesi " + pkFirma + ",0");
            if (dt.Rows.Count == 0)
            {
                if (AcikSatisindex == 1)
                {
                    lcBakiye1.Text = "0,00";
                    lcBakiye1.BackColor = System.Drawing.Color.Transparent;
                }
                if (AcikSatisindex == 2)
                {
                    lcBakiye2.Text = "0,00";
                    lcBakiye2.BackColor = System.Drawing.Color.Transparent;
                }
                if (AcikSatisindex == 3)
                {
                    lcBakiye3.Text = "0,00";
                    lcBakiye3.BackColor = System.Drawing.Color.Transparent;
                }
                if (AcikSatisindex == 4)
                {
                    lcBakiye4.Text = "0,00";
                    lcBakiye4.BackColor = System.Drawing.Color.Transparent;
                }
            }
            else
            {
                decimal bakiye = decimal.Parse(dt.Rows[0][0].ToString());
                if (AcikSatisindex == 1)
                {
                    lcBakiye1.Text = bakiye.ToString("##0.00");
                    if (lcBakiye1.Text == "0,00")
                        lcBakiye1.BackColor = System.Drawing.Color.Transparent;
                    else
                        lcBakiye1.BackColor = System.Drawing.Color.Red;
                }
                else if (AcikSatisindex == 2)
                {
                    lcBakiye2.Text = bakiye.ToString("##0.00");
                    if (lcBakiye2.Text == "0,00")
                        lcBakiye3.BackColor = System.Drawing.Color.Transparent;
                    else
                        lcBakiye2.BackColor = System.Drawing.Color.Red;
                }
                else if (AcikSatisindex == 3)
                {
                    lcBakiye3.Text = bakiye.ToString("##0.00");
                    if (lcBakiye3.Text == "0,00")
                        lcBakiye3.BackColor = System.Drawing.Color.Transparent;
                    else
                        lcBakiye3.BackColor = System.Drawing.Color.Red;
                }
                else if (AcikSatisindex == 4)
                {
                    lcBakiye4.Text = bakiye.ToString("##0.00");
                    if (lcBakiye4.Text == "0,00")
                        lcBakiye4.BackColor = System.Drawing.Color.Transparent;
                    else
                        lcBakiye4.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        private string musteriata(string fkFirma)
        {
            string pkFirma = "1", ozelkod="", firmadi = "";         

            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar with(nolock) where pkFirma=" + fkFirma);
            if (dt.Rows.Count == 0)
            {
                formislemleri.Mesajform("Müşteri Bulunamadı.", "K", 200);
                Satis1Baslik.Text = "";
                Satis1Baslik.Focus();
                return "1";
            }
            pkFirma = dt.Rows[0]["pkFirma"].ToString();
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            if (pkSatisBarkod.Text != "0" || pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE Faturalar SET fkFirma=" + pkFirma + " where pkFaturalar=" + pkSatisBarkod.Text);
            }
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        Satis1Firma.Tag = pkFirma;
                        Satis1Baslik.Text = ozelkod + "-" + firmadi;
                        Satis1Baslik.ToolTip = Satis1Baslik.Text;
                        break;
                    }
                default:
                    break;
            }
            BakiyeGetirSecindex(pkFirma);
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
            //xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();
            yesilisikyeni();
            //Satis1SonKayidaGit();
        }

        private void repositoryItemButtonEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            string girilen =
   ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;

            if (e.KeyValue == 13 && girilen == "")
                return;
            if (e.KeyCode == Keys.Space)
            {
                simpleButton4_Click(sender, e);
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
                gridView1.SetFocusedRowCellValue("Adet", badet);
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
                yesilisikyeni();
            }
            //nakit
            lueKKarti.Visible = false;
            if ((e.KeyValue == 78 || e.KeyValue == 75 || e.KeyValue == 65 || e.KeyValue == 68) && cbOdemeSekli.Enabled == false)
            {
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                return;
            }
            if (e.KeyValue == 78)
            {
                cbOdemeSekli.TabStop = true;
                cbOdemeSekli.SelectedIndex = 0;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //kkartı
            else if (e.KeyValue == 75)
            {
                cbOdemeSekli.TabStop = true;
                cbOdemeSekli.SelectedIndex = 1;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                lueKKarti.Properties.DataSource = DB.GetData("select * from Bankalar where Aktif=1");
                lueKKarti.Visible = true;
                lueKKarti.ItemIndex = 0;
            }
            //veresiye açık hesap
            else if (e.KeyValue == 65)
            {
                cbOdemeSekli.TabStop = true;
                cbOdemeSekli.SelectedIndex = 2;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //DİĞER
            else if (e.KeyValue == 68 )
            {
                cbOdemeSekli.TabStop = true;
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
            AppearanceDefault appErrorRed = new AppearanceDefault(Color.OrangeRed);

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            else if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["AlisFiyati"].ToString() != "")
            {
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                    AppearanceHelper.Apply(e.Appearance, appError);
            }
            else if (e.Column.FieldName == "iskontotutar" && dr["iskontotutar"].ToString() != "0,000000")
            {
                AppearanceHelper.Apply(e.Appearance, appfont);
            }
            else if (e.Column.FieldName == "Adet" && dr["Adet"].ToString() == "0")
            {
                AppearanceHelper.Apply(e.Appearance, appError);
            }
            else if (e.Column.FieldName == "Mevcut" && dr["KritikMiktar"].ToString() != "" && dr["Mevcut"].ToString() != "")
            {
                decimal KritikMiktar = Convert.ToDecimal(dr["KritikMiktar"].ToString());
                decimal Mevcut = Convert.ToDecimal(dr["Mevcut"].ToString());
                if (KritikMiktar > Mevcut)
                    AppearanceHelper.Apply(e.Appearance, appErrorRed);
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
                aratop = 0;//atis1Toplam.Text = "0,0";
            else
                aratop = decimal.Parse(gridColumn5.SummaryItem.SummaryValue.ToString());

            decimal isyuzde = 0, isfiyat = 0, istutar = 0;
            //önce hesapla sonra bilgi göster NullTexde
            if (ceiskontoyuzde.EditValue != null)
            {
                isfiyat = decimal.Parse(ceiskontoyuzde.EditValue.ToString());
                istutar = isfiyat * aratop / 100;
            }
            //if (ceiskontoTutar.EditValue != null) // && iskontoTutar.EditValue.ToString() !="0")
            //{
            //    isfiyat = decimal.Parse(ceiskontoTutar.EditValue.ToString());
            //    if (aratop > 0)
            //        isyuzde = (isfiyat * 100) / aratop;
            //    istutar = isfiyat;
            //}

            aratoplam.Value = aratop - istutar;
            if (AcikSatisindex == 1)
            {
                Satis1Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
                Satis1Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
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
                ceiskontoTutar.Properties.NullText = "";
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
            if (((SimpleButton)sender).Tag != null)
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

        }

        private void ürünBulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton4_Click(sender, e);
        }

        private void sb_MouseEnter(object sender, EventArgs e)
        {
            HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            HizliTop = ((SimpleButton)sender).Top;
            HizliLeft = ((SimpleButton)sender).Left;
            HizliBarkodName = ((SimpleButton)sender).Name;
            pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;
        }
        void FisGetir(string pkSatis)
        {
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(true);
            FisNoBilgisi.fisno.EditValue = pkSatis;
            FisNoBilgisi.ShowDialog();
            if (FisNoBilgisi.btnFisDuzenle.Tag.ToString() == "1")
            {
                AcikSatisindex = 4;
                gCSatisDuzen.Visible = true;
                cbOdemeSekli.TabStop = false;
                gCSatisDuzen.Tag = FisNoBilgisi.groupControl1.Tag;
                DB.ExecuteSQL("update Faturalar Set Siparis=0,AcikHesap=0 where pkFaturalar=" + pkSatis);
                Satis4Toplam.Tag = pkSatis;
                fisduzenaciksatis(false);
                FaturaGetir();
                BakiyeGetirSecindex(gCSatisDuzen.Tag.ToString());
            }
            FisNoBilgisi.Dispose();
            //FisListesi();
            yesilisikyeni();
        }

        void FaturaGetir()
        {
            string pkSatislar = "0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 4)
                pkSatislar = Satis4Toplam.Tag.ToString();
            DataTable dtSatislar = DB.GetData(@"SELECT  fat.pkFaturalar, fat.Tarih, fat.fkFirma, fat.GelisNo, fat.Siparis,fat.Aciklama, 
                      fat.AlinanPara, fat.ToplamTutar, fat.Yazdir, fat.iskontoFaturaTutar, fat.GonderildiWS, f.Firmaadi, f.Adres, 
                      f.Eposta, f.webadresi, f.Tel2, f.Tel, f.Fax, f.Yetkili, fg.GrupAdi, f.VergiDairesi, f.VergiNo, 
                      f.Cep, f.Cep2, f.OzelKod, 
                      sd.Durumu as sd,fat.fkBankalar,
                      k.KullaniciAdi, k.adisoyadi as KullaniciAdiSoyadi,
                      fat.fkPerTeslimEden,fat.GuncellemeTarihi,fat.OdemeSekli,fat.fkSatisDurumu,f.fkSatisFiyatlariBaslik
FROM         Faturalar fat with(nolock) 
INNER JOIN Firmalar f with(nolock) ON fat.fkFirma = f.pkFirma 
LEFT OUTER JOIN Kullanicilar k with(nolock) ON fat.fkKullanici = k.pkKullanicilar 
LEFT OUTER JOIN SatisDurumu sd with(nolock) ON fat.fkSatisDurumu = sd.pkSatisDurumu 
LEFT OUTER JOIN FirmaGruplari fg with(nolock) ON f.fkFirmaGruplari = fg.pkFirmaGruplari
where fat.pkFaturalar=" + pkSatislar);//fiş bilgisi
            if (pkSatislar == "0")
            {
                deGuncellemeTarihi.Enabled = false;
                cbOdemeSekli.Enabled = false;
                deGuncellemeTarihi.EditValue = null;
                cbOdemeSekli.Text = "Nakit";
                return;
            }
            if (dtSatislar.Rows.Count == 0)
            {
                Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            string fkfirma = dtSatislar.Rows[0]["fkFirma"].ToString();
            gCSatisDuzen.Tag = fkfirma;
            string OzelKod = dtSatislar.Rows[0]["OzelKod"].ToString();
            string firmaadi = dtSatislar.Rows[0]["Firmaadi"].ToString();
            cbOdemeSekli.Text = dtSatislar.Rows[0]["OdemeSekli"].ToString();

         if (dtSatislar.Rows[0]["fkSatisDurumu"].ToString() != "")
             lueSatisTipi.EditValue = int.Parse(dtSatislar.Rows[0]["fkSatisDurumu"].ToString());
            //A.G.29.08.2013 00:41
         if (dtSatislar.Rows[0]["fkSatisFiyatlariBaslik"].ToString() != "")
             lueFiyatlar.EditValue = int.Parse(dtSatislar.Rows[0]["fkSatisFiyatlariBaslik"].ToString());
            //

         if (dtSatislar.Rows[0]["GuncellemeTarihi"].ToString() == "")
             deGuncellemeTarihi.EditValue = null;
         else
             deGuncellemeTarihi.DateTime = Convert.ToDateTime(dtSatislar.Rows[0]["GuncellemeTarihi"].ToString());

            if (cbOdemeSekli.Text == "Diğer...") cbOdemeSekli.Text = "Nakit";
            if (cbOdemeSekli.Text == "Kredi Kartı")
            {
                lueKKarti.Properties.DataSource = DB.GetData("select * from Bankalar with(nolock) where Aktif=1");
                if (dtSatislar.Rows[0]["fkBankalar"].ToString() == "")
                    lueKKarti.ItemIndex = 0;
                else
                    lueKKarti.EditValue = int.Parse(dtSatislar.Rows[0]["fkBankalar"].ToString());
            }
            

            if (AcikSatisindex == 1)
            {
                Satis1Firma.Tag = fkfirma;
                Satis1Firma.Text = OzelKod + "-" + firmaadi;
            }
            if (AcikSatisindex == 4)
            {
                gCSatisDuzen.Tag = fkfirma;
                gCSatisDuzen.Text = fkfirma + "-" + firmaadi;
                Satis4Baslik.Text = OzelKod + "-" + firmaadi;
            }
            deGuncellemeTarihi.Enabled = true;
            cbOdemeSekli.Enabled = true;
            //BakiyeGetirSecindex(fkfirma);
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
                if (gridView1.FocusedRowHandle < 0)
                {
                    yesilisikyeni();
                    return;
                }
                string iskontotutar =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
                iskontotutarDegistir(iskontotutar);
                
                decimal iskontoyuzde = 0;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                decimal NakitFiyati = decimal.Parse(dr["NakitFiyati"].ToString());
                iskontoyuzde = (decimal.Parse(iskontotutar) * 100) / NakitFiyati;
                DB.ExecuteSQL("UPDATE SatisDetay SET iskontoyuzdetutar=" + iskontoyuzde.ToString().Replace(",", ".") + " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
                
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

        private void yenidenAdlandırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HizliButtonAdiDegis ButtonAdiDegis = new HizliButtonAdiDegis();
            ButtonAdiDegis.Top = HizliTop;
            ButtonAdiDegis.Left = HizliLeft;
            ButtonAdiDegis.oncekibarkod.Text = HizliBarkod;
            ButtonAdiDegis.oncekibarkod.Tag = pkHizliStokSatis;
            ButtonAdiDegis.ShowDialog();

            Hizlibuttonlariyukle();
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            string pkSatis = "0";
            if (AcikSatisindex == 1) pkSatis = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 4) pkSatis = Satis4Toplam.Tag.ToString();
            if (pkSatis == "0")
            {
                Showmessage("Önce Satış Yapınız!", "K");
                return;
            }
            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.memoozelnot.Text = btnAciklamaGirisi.ToolTip;
            fFisAciklama.ShowDialog();
            btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;
            DB.ExecuteSQL("UPDATE Faturalar SET Aciklama='" + btnAciklamaGirisi.ToolTip + "' where pkSatislar=" + pkSatis);
            fFisAciklama.Dispose();
            yesilisikyeni();
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


        private void lueFiyatlar_EditValueChanged(object sender, EventArgs e)
        {
            if(lueFiyatlar.Tag.ToString() == "1")
               FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
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
        void OdemeSekli(string odemesekli)
        {
            if (ilkyukleme) return;
            string pkSatislar = "0";
            string musteriadi = "";
            string pkFirma = "1";
            if (AcikSatisindex == 1)
            {
                pkSatislar = Satis1Toplam.Tag.ToString();
                musteriadi = Satis1Baslik.Text;
                if (Satis1Firma.Tag != null)
                    pkFirma = Satis1Firma.Tag.ToString();
            }

            else if (AcikSatisindex == 4)
            {
                pkSatislar = Satis4Toplam.Tag.ToString();
                musteriadi = Satis4Baslik.Text;
                if (gCSatisDuzen.Tag != null)
                    pkFirma = gCSatisDuzen.Tag.ToString();
            }
            DB.pkSatislar = int.Parse(pkSatislar);
            DB.PkFirma = int.Parse(pkFirma);
            lueKKarti.Visible = false;
            if (odemesekli == "Diğer...")
            {
                //if (pkSatisBarkod.Text == "0")
                //{
                //    Showmessage("Lütfen Önce Ürün Ekleyiniz!", "K");
                //    cbOdemeSekli.Text = "Nakit";
                //    return;
                //}
                cbOdemeSekli.Text = odemesekli;
                if (pkFirma == "1")
                {
                    pkFirma = musteriara();
                }
                frmSatisOdeme SatisOdeme = new frmSatisOdeme(pkSatislar,false,0,true);
                SatisOdeme.MusteriAdi.Tag = pkFirma;
                SatisOdeme.satistutari.ToolTip = odemesekli;
                SatisOdeme.satistutari.EditValue = aratoplam.EditValue;
                SatisOdeme.ShowDialog();
                if (SatisOdeme.TopMost == true)
                {
                    //pro_kaydetyazdir(SatisOdeme.kaydetmiyazdirmi.Text,lueSatisTipi.Text);//önemli
                    SatisOdeme.Dispose();
                    cbOdemeSekli.SelectedIndex = 0;
                    Satis1ToplamGetir();
                    //yesilisikyeni();
                    return;
                }
                else
                {
                    SatisOdeme.Dispose();
                    cbOdemeSekli.SelectedIndex = 0;

                    return;
                }
            }
            //}
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
                }
                if (pkFirma == "1")
                {
                    cbOdemeSekli.Enabled = true;
                    cbOdemeSekli.Text = "Nakit";
                }
            }
        }

        void fisduzenaciksatis(bool sadeceindex)
        {
            AcikSatisindex = 4;
            FaturaGetir();
            MusteriBakiyeGetir();
            if (sadeceindex == true)
                yesilisikyeni();
        }
        private void Satis4Toplam_Click(object sender, EventArgs e)
        {
            fisduzenaciksatis(true);
        }

        void SatisDuzenKapat()
        {
            gCSatisDuzen.Visible = false;
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
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue.ToString();
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
            string pkSatisDetay = dr["pkFaturalarDetay"].ToString();
            DB.ExecuteSQL("UPDATE FaturalarDetay SET Adet=" + yenimiktar.Replace(",", ".") + " where pkFaturalarDetay=" + pkSatisDetay);
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
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn3, yenimiktar);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn33, iskontogercekyuzde.ToString());

        }
        private void iskontoTutar_EditValueChanged(object sender, EventArgs e)
        {
            ceiskontoyuzde.EditValue = null;
        }

        void dockPanel1Gizle()
        {
            if (dockPanel1.Width > int.Parse(dockPanel1.Tag.ToString()))// && lcKullanici.Tag.ToString()=="0")
            {
                dockPanel1.Width = int.Parse(dockPanel1.Tag.ToString());
                dockPanel1.SendToBack();
                yesilisikyeni();
            }
        }
        void dockPanel1goster()
        {
            //if (dockPanel1.Width == 860)
            //{
            //    dockPanel1.Width = int.Parse(dockPanel1.Tag.ToString());
            //    dockPanel1.SendToBack();
            //}
            //else
            {
                dockPanel1.Width = 860;
                dockPanel1.BringToFront();
            }
        }
        private void xtraTabPage5_Click(object sender, EventArgs e)
        {
            if (xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Count == 0)
                Hizlibuttonlariyukle();
            dockPanel1goster();
            lcKullanici.Tag = "0";
        }

        private void iskontoTutar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                for (int i = 0; i < gridView1.DataRowCount; i++)
			{
                DataRow dr = gridView1.GetDataRow(i);
                decimal satisfiyati = 0, iskontotutar = 0, iskontolusatisfiyati, Faturaiskonto;
                satisfiyati=decimal.Parse(dr["SatisFiyati"].ToString());
                iskontotutar=decimal.Parse(dr["iskontotutar"].ToString());
                iskontolusatisfiyati=satisfiyati-iskontotutar;
                Faturaiskonto = (iskontolusatisfiyati * ceiskontoTutar.Value) / aratoplam.Value;
                DB.ExecuteSQL("UPDATE SatisDetay Set Faturaiskonto=0" + Faturaiskonto.ToString().Replace(",", ".")
                    + " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
			}
                
                yesilisikyeni();
            }
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
                if (DB.GetData("select pkSatislar From Faturalar with(nolock) where pkFaturalar=" + pkSatisBarkod.Text).Rows.Count == 0)
                {
                    Showmessage("Fiş Bulunamadı.", "K");
                    return;
                }
                FisGetir(pkSatisBarkod.Text);
            }
        }

        private void gCSatisDuzen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DB.PkFirma = int.Parse(gCSatisDuzen.Tag.ToString());

            frmMusteriKarti kart = new frmMusteriKarti(gCSatisDuzen.Tag.ToString(), "");
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
            //if (lueSatisTipi.EditValue.ToString() == "5" || lueSatisTipi.EditValue.ToString() == "9")//iade ve değişim
            //    gciade.Visible = true;
            //else
            //    gciade.Visible = false;
            btnTopluFatura.Text = lueSatisTipi.Text +" Yazdır";
            if (lueSatisTipi.EditValue.ToString() == "4" || lueSatisTipi.EditValue.ToString() == "3"
                || lueSatisTipi.EditValue.ToString() == "1")//fatura irsaliye teklif
            {
                gckdv.Visible = true;
                btnyazdir.Visible = false;
                //lbFaturaTarihi.Visible = true;//fatura 
                //FaturaTarihi.Visible = true;//fatura 
                btnOnizleme.Visible = true;//fatura 
            }
            else
            {
                btnyazdir.Visible = true;
                gckdv.Visible = false;
                btnOnizleme.Visible = false;//fatura 
               //lbFaturaTarihi.Visible = false;//fatura 
                //FaturaTarihi.Visible = false;//fatura 
            }
            if (pkSatisBarkod.Text != "" && lueSatisTipi.Tag == "1")
            {
                if (lueSatisTipi.Text == "İade")
                    DB.ExecuteSQL("UPDATE SatisDetay SET Adet=abs(Adet)*-1 where fkSatislar=" + pkSatisBarkod.Text);
                else if (lueSatisTipi.Text == "Değişim")
                    DB.GetData("select getdate()"); //bu kısmı kaldırma iade bozuluyor
                else
                    DB.ExecuteSQL("UPDATE SatisDetay SET Adet=abs(Adet) where fkSatislar=" + pkSatisBarkod.Text);
                //Satış Tipini Kaydet
                DB.ExecuteSQL("UPDATE Satislar set fkSatisDurumu=" + lueSatisTipi.EditValue.ToString() + " where pkSatislar=" + pkSatisBarkod.Text);
            }
            if (lueSatisTipi.EditValue.ToString() == "4")
                btnTopluFatura.Visible=true;
            else
                btnTopluFatura.Visible=false;
            //if (lueSatisTipi.EditValue.ToString() == "1")
            //{
            //    lbGecmisFisler.Text = "SON TEKLİFLER";
            //    lbFaturaTeklifTarihi.Text = "TEKLİF TARİHİ";
            //    gbBaslik.Text = "Teklif";
            //}
            //else
            //{
            //    lbGecmisFisler.Text = "SON SATIŞLAR";
            //    lbFaturaTeklifTarihi.Text = "FATURA TARİHİ";
            //    gbBaslik.Text = "Satış Faturası";
            //}

            if (!ilkyukleme)
                yesilisikyeni();
        }

        void SatisDigerBilgileriGetir2()
        {
            //TODO:SatisDigerBilgileriGetir
            DataTable dt = DB.GetData("select pkSatislar,fkFirma from Satislar where pkSatislar=0");
            btnAciklamaGirisi.ToolTip = dt.Rows[0]["Aciklama"].ToString();
        }

        void SatisDetayGetir(string pkSatislar)
        {
            pkSatisBarkod.Text = pkSatislar;
            gcSatisDetay.DataSource = DB.GetData("exec sp_FaturalarDetay " + pkSatislar + ",0");
            toplamlar();
            gridView1.AddNewRow();
        }

        void FaturalarDetayEkle(string barkod)
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
            if (dr != null && dr["pkFaturalarDetay"].ToString() != "") return;
            if (barkod == "" || f == 0) return;
                YeniFaturaEkle();
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
            if (lueSatisTipi.Text == "İade") EklenenMiktar = EklenenMiktar * -1;
            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti FROM StokKarti with(nolock) where Barcode='" + barkod + "'");
            if (dtStokKarti.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                StokKartiHizli.ceBarkod.EditValue = barkod;
                StokKartiHizli.ShowDialog();
                if (StokKartiHizli.TopMost == true)
                {
                    dtStokKarti = DB.GetData("select pkStokKarti From StokKarti with(nolock) WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
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
            arr.Add(new SqlParameter("@fkFaturalar", pkSatisBarkod.Text));
            arr.Add(new SqlParameter("@SatisFiyatGrubu", lueFiyatlar.EditValue.ToString()));
            arr.Add(new SqlParameter("@Adet", EklenenMiktar.ToString().Replace(",", ".")));
            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
            arr.Add(new SqlParameter("@iskontoyuzde", "0"));
            arr.Add(new SqlParameter("@iskontotutar", "0"));
            string s = DB.ExecuteScalarSQL("exec sp_FaturalarDetay_Ekle @fkFaturalar,@SatisFiyatGrubu,@Adet,@fkStokKarti,@iskontoyuzde,@iskontotutar", arr);
            if (s != "Satis Detay Eklendi.")
            {
                MessageBox.Show(s);
                return;
            }
            HizliMiktar = 1;
        }

        void YeniFatura()
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
            list.Add(new SqlParameter("@fkKullanici", lueKullanicilar.EditValue.ToString()));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));//iskontoTutar.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.Text));


            sql = "INSERT INTO Faturalar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen,OdemeSekli,SonislemTarihi)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar,0,0,@OdemeSekli,getdate()) SELECT IDENT_CURRENT('Faturalar')";
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
            deGuncellemeTarihi.Enabled = true;
            cbOdemeSekli.Enabled = true;
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(0,0);
            string pkSatislar = "0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 4)
                pkSatislar = Satis4Toplam.Tag.ToString();
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
                else if (dr["pkFaturalarDetay"].ToString() == "")
                    gridView1.DeleteRow(i);
            }
        }

        private void sonrakiSatisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AcikSatisindex == 1) AcikSatisindex = 2;
            else if (AcikSatisindex == 2) AcikSatisindex = 3;
            else if (AcikSatisindex == 3) AcikSatisindex = 1;
            FaturaGetir();
            yesilisikyeni();
            Application.DoEvents();
        }

        private void repositoryItemCalcEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dr == null) return;
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
                decimal NakitFiyati = decimal.Parse(dr["NakitFiyati"].ToString());
                decimal iskonto = 0, iskontotutar = 0;
                decimal.TryParse(girilen, out iskontotutar);
                iskonto = NakitFiyati - iskontotutar;
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
                decimal iskontoyuzdetutar = 0, iskontoyuzde = 0;
                decimal.TryParse(iskontoyuzdetutargirilen.ToString(), out iskontoyuzde);
                iskontoyuzdetutar = (iskontoyuzde * Fiyat) / 100;
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
            DataTable dt = DB.GetData("select pkFaturalar,fkFirma from Faturalar with(nolock) where fkSatisDurumu<>10 and Siparis<>1 and fkKullanici=" + DB.fkKullanicilar);
            //select pkSatislar,fkFirma,f.OzelKod from Satislar s with(nolock) "+ 
            //"inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma where Siparis<>1 and fkKullanici="+DB.fkKullanicilar);
            int c = dt.Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string pkSatislar = dt.Rows[i]["pkFaturalar"].ToString();
                    string fkFirma = dt.Rows[i]["fkFirma"].ToString();
                    AcikSatisindex = i + 1;
                    if (i == 0)
                    {
                        //Showmessage("Ekranda tamamlanmamış " + c.ToString() + " Adet işleminiz bulumnaktadır.", "K");
                        //DevExpress.XtraEditors.XtraMessageBox.Show("Ekranda tamamlanmamış " + c.ToString() + " Adet işleminiz bulumnaktadır.", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Satis1Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar with(nolock) where pkFirma=" + fkFirma);
                        Satis1Firma.Tag = dtMusteri.Rows[0]["pkFirma"].ToString();
                        Satis1Baslik.ToolTip = dtMusteri.Rows[0]["OzelKod"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis1Baslik.Text = Satis1Baslik.ToolTip;
                    }

                    if (i > 1) break;
                    //yesilisikyeni();
                }
                AcikSatisindex = 1;
                FaturaGetir();
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
            int f = gridView1.FocusedRowHandle;
            if (f < 0)
            {
                f = gridView1.DataRowCount - 1;
            }
            DataRow dr = gridView1.GetDataRow(f);
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            SatisFiyatlari.ShowDialog();
            int i = int.Parse(lueFiyatlar.EditValue.ToString());
            Fiyatlarigetir();
            lueFiyatlar.EditValue = i;
            yesilisikyeni();
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
        
        void MusteriBakiyeGetir()
        {
            string fkfirma = "0";
            if (AcikSatisindex == 1)
            {
                fkfirma = Satis1Firma.Tag.ToString();
            }
            if (AcikSatisindex == 4)
            {
                fkfirma = gCSatisDuzen.Tag.ToString();
            }
            BakiyeGetirSecindex(fkfirma);
        }
        void Satis1ToplamGetir()
        {
            AcikSatisindex = 1;
            FaturaGetir();
            //MusteriBakiyeGetir();
            yesilisikyeni();
           // FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
        }
        private void Satis1Toplam_Click(object sender, EventArgs e)
        {
            Satis1ToplamGetir();
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
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue.ToString();
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
            if (AcikSatisindex == 4)
                DB.PkFirma = int.Parse(gCSatisDuzen.Tag.ToString());

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
            }
        }

        private void cmsPopYazir_Opening(object sender, CancelEventArgs e)
        {
            if (((((System.Windows.Forms.ContextMenuStrip)(sender)).SourceControl).Controls.Owner).Name == "btnyazdir")
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = true;
                müşteriSeçToolStripMenuItem.Visible = false;
                müşteriKArtıToolStripMenuItem.Visible = false;
                müşteriHareketleriToolStripMenuItem1.Visible = false;
            }
            else
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = false;
                müşteriSeçToolStripMenuItem.Visible = true;
                müşteriKArtıToolStripMenuItem.Visible = true;
                müşteriHareketleriToolStripMenuItem1.Visible = true;
            }
        }

        private void bARKOTBASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            formislemleri.EtiketBas(dr["fkStokKarti"].ToString(),1);
            /*
            if (gridView1.FocusedRowHandle < 0) return;
            string pkEtiketBas = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Fiş No " + pkSatisBarkod.Text + "',0) SELECT IDENT_CURRENT('EtiketBas')");
            DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["fkStokKarti"].ToString() + ",1,0,getdate())");
            frmEtiketBas EtiketBas = new frmEtiketBas();
            EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
            EtiketBas.ShowDialog();
              */
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
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("1");
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            Hizlibuttonlariyukle();
        }

        private void groupControl6_MouseClick(object sender, MouseEventArgs e)
        {
            pkSatisBarkod.Text = "0";
            Satis1Toplam.Tag = "0";
            timer1.Enabled = true;
            yesilisikyeni();
        }


        private void Satis1Baslik_DoubleClick_1(object sender, EventArgs e)
        {
            musteriara();
        }

        private void Satis1Baslik_Click(object sender, EventArgs e)
        {
            Satis1Baslik.Text = "";
            AcikSatisindex = 1;
            TutarFont(Satis1Toplam);
        }

        private void Satis1Baslik_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Satis1Baslik.Text == "")
                    musteriara();
                else
                    musteriata(Satis1Baslik.Text);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                musteriata("1");
                BakiyeGetirSecindex("1");
                yesilisikyeni();
            }
        }
        private void Satis1Baslik_Leave(object sender, EventArgs e)
        {
            if (Satis1Baslik.Text == "")
                Satis1Baslik.EditValue = Satis1Baslik.OldEditValue;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            frmFiyatGor FiyatGor = new frmFiyatGor();
            FiyatGor.ShowDialog();
            yesilisikyeni();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmHizliGruplar HizliGruplar = new frmHizliGruplar();
            HizliGruplar.ShowDialog();

        }
        void YazdirSatisFaturasi(bool Disigner)
        {
            try
            {
                string fisid = pkSatisBarkod.Text;
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                string sql = @"SELECT  s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama, 
                      sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as ToplamTutar, 
                      Firmalar.Firmaadi, Firmalar.Adres, 
                      Firmalar.Eposta, Firmalar.webadresi, Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, Firmalar.VergiDairesi, Firmalar.VergiNo, 
                      Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu as SatisDurumu, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi as KullaniciAdiSoyadi
                      ,dbo.fnc_ParayiYaziyaCevir(sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)),1) as rakamoku,s.FaturaTarihi,s.TeslimTarihi
FROM  Satislar s
INNER JOIN SatisDetay sd on sd.fkSatislar=s.pkSatislar
INNER JOIN Firmalar ON s.fkFirma = Firmalar.pkFirma 
LEFT OUTER JOIN Kullanicilar ON s.fkKullanici = Kullanicilar.pkKullanicilar 
LEFT OUTER JOIN SatisDurumu ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
LEFT OUTER JOIN FirmaGruplari ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
where s.pkSatislar=@pkSatislar
group by s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama,Firmalar.Eposta, Firmalar.webadresi,
Firmalar.Firmaadi, Firmalar.Adres, 
 Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, 
 Firmalar.VergiDairesi, Firmalar.VergiNo, 
Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu,Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi,
s.FaturaTarihi,s.TeslimTarihi
";
                sql = sql.Replace("@pkSatislar", fisid);
                DataTable Fis = DB.GetData(sql);
                //                DataTable Fis = DB.GetData(@"SELECT  s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama, 
                //                      sd.Adet*(sd.SatisFiyati-sd.iskontotutar) as ToplamTutar, 
                //                      Firmalar.Firmaadi, Firmalar.Adres, 
                //                      Firmalar.Eposta, Firmalar.webadresi, Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, Firmalar.VergiDairesi, Firmalar.VergiNo, 
                //                      Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu as SatisDurumu, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi as KullaniciAdiSoyadi
                //                      ,dbo.RakamOku(sd.Adet*(sd.SatisFiyati)) as rakamoku
                //FROM  Satislar s
                //INNER JOIN SatisDetay sd on sd.fkSatislar=s.pkSatislar
                //INNER JOIN Firmalar ON s.fkFirma = Firmalar.pkFirma 
                //LEFT OUTER JOIN Kullanicilar ON s.fkKullanici = Kullanicilar.pkKullanicilar 
                //LEFT OUTER JOIN SatisDurumu ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
                //LEFT OUTER JOIN FirmaGruplari ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
                //where s.pkSatislar= " + fisid);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                ////kdv oranları
                //DataTable fiskdv = DB.GetData("select KdvOrani,SUM((SatisFiyati-iskontotutar)*Adet) as kdvtutar  from SatisDetay" +
                //" where fkSatislar= " + fisid + " group by KdvOrani");
                //fiskdv.TableName = "fiskdv";
                //ds.Tables.Add(fiskdv);
                //şirket bilgileri
                DataTable sirket = DB.GetData("select top 1 * from Sirketler");
                sirket.TableName = "sirket";
                ds.Tables.Add(sirket);
                //Firma bilgileri
                DataTable Musteri = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + Fis.Rows[0]["fkFirma"].ToString());
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\SatisFatura.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                xrCariHareket rapor = new xrCariHareket();
                //XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "SatisFatura";
                rapor.Report.Name = "SatisFatura";
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//YaziciAdi
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void YazdirTeklif(bool Disigner)
        {
            try
            {
                string fisid = pkSatisBarkod.Text;
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                string sql = @"SELECT  s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama, 
                      sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as ToplamTutar, 
                      Firmalar.Firmaadi, Firmalar.Adres, 
                      Firmalar.Eposta, Firmalar.webadresi, Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, Firmalar.VergiDairesi, Firmalar.VergiNo, 
                      Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu as SatisDurumu, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi as KullaniciAdiSoyadi
                      ,dbo.fnc_ParayiYaziyaCevir(sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)),1) as rakamoku,s.FaturaTarihi,s.TeslimTarihi
FROM  Satislar s
INNER JOIN SatisDetay sd on sd.fkSatislar=s.pkSatislar
INNER JOIN Firmalar ON s.fkFirma = Firmalar.pkFirma 
LEFT OUTER JOIN Kullanicilar ON s.fkKullanici = Kullanicilar.pkKullanicilar 
LEFT OUTER JOIN SatisDurumu ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
LEFT OUTER JOIN FirmaGruplari ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
where s.pkSatislar=@pkSatislar
group by s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama,Firmalar.Eposta, Firmalar.webadresi,
Firmalar.Firmaadi, Firmalar.Adres, 
 Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, 
 Firmalar.VergiDairesi, Firmalar.VergiNo, 
Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu,Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi,
s.FaturaTarihi,s.TeslimTarihi
";
                sql = sql.Replace("@pkSatislar", fisid);
                DataTable Fis = DB.GetData(sql);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //şirket bilgileri
                DataTable sirket = DB.GetData("select top 1 * from Sirketler");
                sirket.TableName = "sirket";
                ds.Tables.Add(sirket);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\Teklif.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                xrCariHareket rapor = new xrCariHareket();
                //XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "Teklif.repx";
                rapor.Report.Name = "Teklif";
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//YaziciAdi
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        void Yazdirirsaliye(bool Disigner)
        {
            try
            {
                string fisid = pkSatisBarkod.Text;
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                string sql = @"SELECT  s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama, 
                      sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as ToplamTutar, 
                      Firmalar.Firmaadi, Firmalar.Adres, 
                      Firmalar.Eposta, Firmalar.webadresi, Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, Firmalar.VergiDairesi, Firmalar.VergiNo, 
                      Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu as SatisDurumu, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi as KullaniciAdiSoyadi
                      ,dbo.fnc_ParayiYaziyaCevir(sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)),1) as rakamoku,s.FaturaTarihi,s.TeslimTarihi
FROM  Satislar s
INNER JOIN SatisDetay sd on sd.fkSatislar=s.pkSatislar
INNER JOIN Firmalar ON s.fkFirma = Firmalar.pkFirma 
LEFT OUTER JOIN Kullanicilar ON s.fkKullanici = Kullanicilar.pkKullanicilar 
LEFT OUTER JOIN SatisDurumu ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
LEFT OUTER JOIN FirmaGruplari ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
where s.pkSatislar=@pkSatislar
group by s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama,Firmalar.Eposta, Firmalar.webadresi,
Firmalar.Firmaadi, Firmalar.Adres, 
 Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, 
 Firmalar.VergiDairesi, Firmalar.VergiNo, 
Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu,Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi,
s.FaturaTarihi,s.TeslimTarihi
";
                sql = sql.Replace("@pkSatislar", fisid);
                DataTable Fis = DB.GetData(sql);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //şirket bilgileri
                DataTable sirket = DB.GetData("select top 1 * from Sirketler");
                sirket.TableName = "sirket";
                ds.Tables.Add(sirket);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\irsaliye.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                xrCariHareket rapor = new xrCariHareket();
                //XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "irsaliye";
                rapor.Report.Name = "irsaliye";
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//YaziciAdi
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        private void cbOdemeSekli_EditValueChanged(object sender, EventArgs e)
        {

            if (ilkyukleme) return;
            string odemesekli = "";
            if (cbOdemeSekli.SelectedIndex == 0)
                odemesekli = "Nakit";
            else if (cbOdemeSekli.SelectedIndex == 1)
                odemesekli = "Kredi Kartı";
            else if (cbOdemeSekli.SelectedIndex == 2)
                odemesekli = "Açık Hesap";
            else if (cbOdemeSekli.SelectedIndex == 3)
                odemesekli = "Diğer...";
            string fkFirma = "0";
            if (AcikSatisindex == 1)
            {
                fkFirma = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                fkFirma = gCSatisDuzen.Tag.ToString();
            }

            if (cbOdemeSekli.TabStop == true)
            {
                OdemeSekli(odemesekli);
                //if (cbOdemeSekli.TabStop == false) return;
            }
                if (pkSatisBarkod.Text == "0" && cbOdemeSekli.Text == "Açık Hesap")
                {
                    Showmessage("Lütfen Önce Ürün Ekleyiniz!", "K");
                    cbOdemeSekli.Text = "Nakit";
                    return;
                }
                //if (fkFirma == "1" && cbOdemeSekli.Text == "Açık Hesap")
                //{
                //    Showmessage("1 Nolu Müşteriye Açık Hesap Olmaz!", "K");
                //    cbOdemeSekli.Text = "Nakit";
                //    return;
                //}
                Satis1Baslik.BackColor = Satis1Toplam.BackColor;
                switch (cbOdemeSekli.SelectedIndex)
                {
                    case 0:
                        {
                            cbOdemeSekli.BackColor = System.Drawing.Color.White;
                            break;
                        }
                    case 1:
                        {
                            lueKKarti.Visible = true;
                            lueKKarti.ItemIndex = 0;
                            cbOdemeSekli.BackColor = System.Drawing.Color.Yellow;
                            break;
                        }
                    case 3:
                        {
                            cbOdemeSekli.BackColor = System.Drawing.Color.White; break;
                        }
                    default:
                        break;
                }
                //if (cbOdemeSekli.Text == "")
                 //   cbOdemeSekli.Text = "Nakit";
                //DB.ExecuteSQL("UPDATE satislar set OdemeSekli='" + cbOdemeSekli.Text + "' where pkSatislar=" + pkSatisBarkod.Text);
            //}29.09.2013
            yesilisikyeni();
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string girilen =
                ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (girilen == "True")
                DB.ExecuteSQL("UPDATE SatisDetay SET iade=1,Adet=Adet*-1 where pkSatisDetay=" +
                dr["pkSatisDetay"].ToString());
            else
                DB.ExecuteSQL("UPDATE SatisDetay SET iade=0,Adet=abs(Adet) where pkSatisDetay=" +
           dr["pkSatisDetay"].ToString());
            yesilisikyeni();
           // SatisTipiKaydet();
        }
        void MusteriHareketleri(string fkFirma)
        {
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = fkFirma;
            CariHareketMusteri.Show();
        }
        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fkFirma = "0";
            if (AcikSatisindex == 1)
            {
                SatisDetayGetir(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
                fkFirma = Satis1Firma.Tag.ToString();
                lcBakiye1.Visible = true;
            }
            else if (AcikSatisindex == 4)
            {
                SatisDetayGetir(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
                fkFirma = gCSatisDuzen.Tag.ToString();
                lcBakiye4.Visible = true;
            }

            //DB.PkFirma = int.Parse(fkFirma);

            MusteriHareketleri(fkFirma);
        }

        private void cbOdemeSekli_Enter(object sender, EventArgs e)
        {
            cbOdemeSekli.TabStop = true;
        }

        private void cbOdemeSekli_Leave(object sender, EventArgs e)
        {
            cbOdemeSekli.TabStop = false;
        }

        private void ucSatis_VisibleChanged(object sender, EventArgs e)
        {
            //if (gridView1.DataRowCount > 0)
            //{
            //    MessageBox.Show("Açık Satışlarınız Var.");
            //}
        }

        private void ucSatis_Enter(object sender, EventArgs e)
        {
            //gridView1.Focus();
            //gridView1.AddNewRow();
            //gridView1.ShowEditor();
           // yesilisikyeni();
        }

        private void bUFİŞMALİYETİToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gcAlisTutar.Visible == true)
            {
                gcAlisTutar.Visible = false;
                gcKar.Visible = false;
            }
            else
            {
                gcAlisTutar.Visible = true;
                gcKar.Visible = true;
            }
        }

        private void tSMIEksikListesi_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string Stokadi = dr["Stokadi"].ToString();
            string fkStokKarti = dr["fkStokKarti"].ToString();

            if (DB.GetData("select StokAdi from EksikListesi el with(nolock) where fkStokKarti=" + fkStokKarti).Rows.Count > 0)
            {
                formislemleri.Mesajform("Daha Önce Eklendi!", "K", 200);
                return;
            }
            //inputForm sifregir = new inputForm();
            //sifregir.Text = "Miktar Giriniz";
            //sifregir.ShowDialog();

            int girilen=0;
            int.TryParse(formislemleri.inputbox("Eksik Miktar Girişi","Alınacak Stok Miktarını Giriniz","1",false),out girilen);
            if (girilen==0)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız", "K", 200);
                yesilisikyeni();
                return;
            }
            int sonuc = DB.ExecuteSQL("INSERT INTO EksikListesi (Tarih,StokAdi,Miktar,fkStokKarti) values(getdate(),'"
                + Stokadi + "'," + girilen.ToString().Replace(",", ".") + "," + fkStokKarti + ")");
            if (sonuc == -1)
            {
                Showmessage("Hata Oluştu.", "K");
            }
            yesilisikyeni();
        
        }

        private void lueKullanicilar_EditValueChanged(object sender, EventArgs e)
        {
            //DB.fkKullanicilar = lueKullanicilar.EditValue.ToString();
            if (pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("update Satislar set fkKullanici=" + lueKullanicilar.EditValue.ToString() +
                    " where pkSatislar=" + pkSatisBarkod.Text);
                yesilisikyeni();
            }
        }
        void FiyatGruplariDegis(string fkSatisFiyatlariBaslik)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
                DataTable dt = DB.GetData("select * from SatisFiyatlari where fkStokKarti=" + fkStokKarti +
                    " and fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik);
                if (dt.Rows.Count == 0)
                    dt = DB.GetData("select * from SatisFiyatlari where fkStokKarti=" + fkStokKarti + " and fkSatisFiyatlariBaslik=1");
                string SatisFiyat1 = dt.Rows[0]["SatisFiyatiKdvli"].ToString();
                string sql = "update SatisDetay set SatisFiyati=@SatisFiyati where pkSatisDetay=" + pkSatisDetay;
                sql = sql.Replace("@SatisFiyati", SatisFiyat1.Replace(",", "."));
                DB.ExecuteSQL(sql);
                lueFiyatlar.Tag="0";
            }
        }
        private void lueFiyatlar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (lueFiyatlar.EditValue != null)
                    FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
                yesilisikyeni();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (lueFiyatlar.EditValue != null)
                    lueFiyatlar.EditValue = lueFiyatlar.OldEditValue;
                yesilisikyeni();
            }
        }

        private void lueFiyatlar_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            //((DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit)(((DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit)(lueFiyatlar.Properties.Columns.owner)).Columns.owner)).KeyValue
            //lueFiyatlar.PopupForm.CurrentValue
            //lueFiyatlar. = 1;
            string v = e.Value.ToString();
            //lueFiyatlar.EditValue = int.Parse(e.Value.ToString());
            FiyatGruplariDegis(v);
            yesilisikyeni();
        }

        private void deGuncellemeTarihi_EditValueChanged(object sender, EventArgs e)
        {
            if (deGuncellemeTarihi.Tag == "1")
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@GuncellemeTarihi", deGuncellemeTarihi.DateTime));
                list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
                DB.ExecuteSQL("update Satislar Set GuncellemeTarihi=@GuncellemeTarihi where pkSatislar=@pkSatislar", list);
                yesilisikyeni();
            }
        }

        private void deGuncellemeTarihi_Enter(object sender, EventArgs e)
        {
            deGuncellemeTarihi.Tag = "1";
        }

        private void deGuncellemeTarihi_Leave(object sender, EventArgs e)
        {
            deGuncellemeTarihi.Tag = "0";
        }

        private void lueKKarti_EditValueChanged(object sender, EventArgs e)
        {
            if (lueKKarti.Tag == "1")
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue));
                list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
                DB.ExecuteSQL("update Satislar Set fkBankalar=@fkBankalar where pkSatislar=@pkSatislar", list);
                yesilisikyeni();
            }
        }

        private void lueKKarti_Enter(object sender, EventArgs e)
        {
            lueKKarti.Tag = "1";
        }

        private void lueKKarti_Leave(object sender, EventArgs e)
        {
            lueKKarti.Tag = "0";
        }

        private void lueSatisTipi_Enter(object sender, EventArgs e)
        {
            lueSatisTipi.Tag = "1";
        }

        private void lueSatisTipi_Leave(object sender, EventArgs e)
        {
            lueSatisTipi.Tag = "0";
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (AcikSatisindex == 1)
            {
              fkfirma=Satis1Firma.Tag.ToString();
            }
            if (AcikSatisindex == 4)
            {
                fkfirma = gCSatisDuzen.Tag.ToString();
            }
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkfirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            //Odemeler();
            BakiyeGetirSecindex(fkfirma);
        }

        private void ödemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (AcikSatisindex == 1)
            {
                fkfirma = Satis1Firma.Tag.ToString();
            }
            if (AcikSatisindex == 4)
            {
                fkfirma = gCSatisDuzen.Tag.ToString();
            }
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkfirma;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();
            //Odemeler();
            BakiyeGetirSecindex(fkfirma);
        }
        private void pmenuStokHareketleri_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0", "", "");
            SatisUrunBazindaDetay.Tag = "2";
            SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            SatisUrunBazindaDetay.ShowDialog();
            yesilisikyeni();
        }

        private void lueFiyatlar_Enter(object sender, EventArgs e)
        {
            lueFiyatlar.Tag = "1";
        }

        private void lueFiyatlar_Leave(object sender, EventArgs e)
        {
            lueFiyatlar.Tag = "0";
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

            string sql = @"select fat.fkFirma,pkFaturalar as FisNo,f.Firmaadi as MusteriAdi,ToplamTutar,fat.GuncellemeTarihi as Tarih,
                        fat.OdemeSekli,k.KullaniciAdi,Sd.Durumu from Faturalar fat
                        LEFT JOIN Firmalar f ON fat.fkFirma = f.pkFirma
                        LEFT JOIN Kullanicilar k ON fat.fkKullanici=k.pkKullanicilar
                        LEFT JOIN SatisDurumu sd ON sd.pkSatisDurumu=fat.fkSatisDurumu
                        where fat.Siparis=1 and fat.fkSatisDurumu=4 order by SonislemTarihi desc";

            targetGrid.DataSource = DB.GetData(sql);
            //,dbo.fon_MusteriBakiyesi(s.fkFirma) as Bakiye

            targetGrid.BringToFront();
            targetGrid.Width = 650;
            targetGrid.Height = 300;
            targetGrid.Left = pkSatisBarkod.Left +pGecmisFisler.Left;
            targetGrid.Top = pkSatisBarkod.Top + 60;
            if (gridView.Columns.Count > 0)
            {
                gridView.Columns[1].Width = 50;
                gridView.Columns[2].Width = 150;
                gridView.Columns[4].Width = 100;
                gridView.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView.Columns[4].DisplayFormat.FormatString ="g";
               //gridView.Columns[2].DisplayFormat.FormatString = "{0:#0.00####}";
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
            if (lueSatisTipi.EditValue.ToString() == "10")
            {
            //    AcikSatisindex = 4;
            //    gCSatisDuzen.Visible = true;
            //    cbOdemeSekli.TabStop = false;
            //    //gCSatisDuzen.Tag = FisNoBilgisi.groupControl1.Tag;
            //    DB.ExecuteSQL("update Satislar Set Siparis=0,AcikHesap=0 where pkSatislar=" + dr["FisNo"].ToString());
            //    Satis4Toplam.Tag = dr["FisNo"].ToString();
            //    fisduzenaciksatis(false);
            //    SatisGetir();
            //    BakiyeGetirSecindex(gCSatisDuzen.Tag.ToString());
            //    yesilisikyeni();
                string pkArayanlar = dr["FisNo"].ToString();
                DB.ExecuteSQL("update Arayanlar Set Siparis=0 where pkArayanlar=" + pkArayanlar);
            }
            else
              FisGetir(dr["FisNo"].ToString());
            targetGrid.Visible = false;
        }

        private void tMenuitemOdemeAl_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            fkfirma=dr["fkFirma"].ToString();
            targetGrid.Visible = false;

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkfirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            //Odemeler();
            //BakiyeGetirSecindex(fkfirma);
        }

        private void pGecmisFisler_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
               targetGrid.Visible = false;
        }

        private void gcSatisDetay_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void SolPanel_Click(object sender, EventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void SolPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void splitContainerControl1_Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void panelControl3_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            fkfirma = dr["fkFirma"].ToString();
            MusteriHareketleri(fkfirma);
        }

        private void MenuitemOdemeAl_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            fkfirma = dr["fkFirma"].ToString();
            targetGrid.Visible = false;

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkfirma;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();
        }

        private void repositoryItemCalcEdit4_KeyDown(object sender, KeyEventArgs e)
        {
            //iskonto tutar
            if (e.KeyCode == Keys.Enter)
            {
                if (gridView1.FocusedRowHandle < 0) return;
                string iskontoyuzde =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                decimal NakitFiyati = decimal.Parse(dr["NakitFiyati"].ToString());
                decimal iskontotutar = 0;
                iskontotutar = NakitFiyati * decimal.Parse(iskontoyuzde) / 100;
                iskontotutarDegistir(iskontotutar.ToString());
                DB.ExecuteSQL("UPDATE SatisDetay SET iskontoyuzdetutar=" + iskontoyuzde.Replace(",", ".") + " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
                yesilisikyeni();
            }
        }

        private void özelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xrCariHareket report = new xrCariHareket();
            report.DataSource = gcSatisDetay.DataSource;
            report.CreateDocument();
            report.ShowPreview();

            //PrintControl printControl1 = new PrintControl();
            //string dosya = Application.StartupPath + "\\temporary.csv";
            ////gridView1.ExportToXls(dosya);

            //xrCariHareket report = new xrCariHareket();

            //report.CreateDocument();
            //printControl1.PrintingSystem = report.PrintingSystem;

            //printControl1.PrintingSystem.ExportToCsv(Application.StartupPath + "\\temporary.csv", new DevExpress.XtraPrinting.CsvExportOptions(",", Encoding.Default));

            //ProcessStartInfo psi = new ProcessStartInfo();

            //psi.FileName = dosya;
            //psi.Verb = "Print";

            //Process.Start(psi);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            xrCariHareket report = new xrCariHareket();
            report.DataSource = gcSatisDetay.DataSource;
            report.CreateDocument();
            report.ShowDesigner();
        }

        private void yazdırToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            xrCariHareket report = new xrCariHareket();
            report.DataSource = gcSatisDetay.DataSource;
            report.CreateDocument();
            report.Print();
        }

        private void gcSatisDetay_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string fkFirma = "0";
            if (AcikSatisindex == 1)
            {
                fkFirma = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                fkFirma = gCSatisDuzen.Tag.ToString();
            }
            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            //GridView View = sender as GridView;
            if (ghi.RowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(ghi.RowHandle);
            if (ghi.Column.FieldName == "Stokadi")
            {
                frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay(fkFirma, "", "");
                SatisUrunBazindaDetay.Tag = "3";
                SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
                SatisUrunBazindaDetay.ShowDialog();
                yesilisikyeni();
            }
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {

            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            //if (ghi.Column == null) return;
            ////GridView View = sender as GridView;
            if (ghi.RowHandle < 0 && gridView1.CustomizationForm==null) yesilisikyeni();
            //DataRow dr = gridView1.GetDataRow(ghi.RowHandle);
            // if (ghi.Column.FieldName == "Stokadi")
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            //if (e.RowHandle < 0)
            //{
            //    yesilisikyeni();
            //    return;
            //}
        }

        private void faturaDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirSatisFaturasi(true);
        }

        private void teklifDizaynToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            YazdirTeklif(true);
        }

        private void irsaliyeDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdirirsaliye(true);
        }

        private void btnTopluFatura_Click(object sender, EventArgs e)
        {
            string fkFirma = "1";
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        fkFirma = Satis1Firma.Tag.ToString();
                        break;
                    }
                case 4:
                    {
                        fkFirma = gCSatisDuzen.Tag.ToString();
                        break;
                    }
            }
            frmFaturaToplu FaturaToplu = new frmFaturaToplu(fkFirma);
            FaturaToplu.ShowDialog();
        }

        private void btnOnizleme_Click(object sender, EventArgs e)
        {
           if (lueSatisTipi.Text=="Teklif")
               YazdirTeklif(false);
           if (lueSatisTipi.Text == "Fatura")
           {
               ArrayList list = new ArrayList();
               list.Add(new SqlParameter("@FaturaTarihi", FaturaTeklifTarihi.DateTime));
               list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
               DB.ExecuteSQL("update Satislar Set FaturaTarihi=@FaturaTarihi where pkSatislar=@pkSatislar", list);

               YazdirSatisFaturasi(false);
           }
           if (lueSatisTipi.Text == "İrsaliye")
               Yazdirirsaliye(false);
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\SatisFaturaGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);

            gridView1.OptionsBehavior.AutoPopulateColumns = false;
            gridView1.OptionsCustomization.AllowColumnMoving = false;
            gridView1.OptionsCustomization.AllowColumnResizing = false;
            gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            gridView1.OptionsCustomization.AllowRowSizing = false;
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\SatisFaturaGrid.xml";
            File.Delete(Dosya);
            //if (File.Exists(Dosya))
            //{
            //    for (int i = 0; i < gridView1.Columns.Count; i++)
            //        gridView1.Columns[i].Visible = true;
            //    File.Delete(Dosya);
            //}
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //gridView1.PopulateColumns();//tüm alanların genişliğini eşitledi.
            //gridView1.GetShowEditorMode();
           // gridView1.PopulateColumns();
            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string fisno = pkSatisBarkod.Text;
            gcSatisDetay.ExportToXls(exedizini + "/" + fisno + "Fis.xls");
            Process.Start(exedizini + "/" + fisno + "Fis.xls");
        }

        private void gridView1_HideCustomizationForm(object sender, EventArgs e)
        {
            kaydetToolStripMenuItem1_Click(sender, e);
        }

        private void xtraTabControl3_SelectedPageChanging(object sender, TabPageChangingEventArgs e)
        {
            //xtraTabControl3.TabStop = !ilkyukleme;
        }

        private void xtraTabControl3_Selecting(object sender, TabPageCancelEventArgs e)
        {
            //xtraTabControl3.TabStop = !ilkyukleme;
        }
 
        private void xtraTabControl3_Selected(object sender, TabPageEventArgs e)
        {
           // xtraTabControl3.TabStop = !ilkyukleme;
        }

        private void xtraTabControl3_Click(object sender, EventArgs e)
        {
            //if(xtraTabControl3.TabStop == false)
                dockPanel1goster();
            //xtraTabControl3.TabStop = false;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            dockPanel1goster();
        }

        private void gcSatisDetay_Click(object sender, EventArgs e)
        {

        }
    }
}
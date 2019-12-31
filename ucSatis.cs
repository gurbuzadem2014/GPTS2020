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
using GPTS.Entegrasyon;
using GPTS.EFaturaBasic;

namespace GPTS
{

    public partial class ucSatis : DevExpress.XtraEditors.XtraUserControl
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis = "";//AccessibleDescription
        int AcikSatisindex = 1;//hangi satış açık
        decimal HizliMiktar = 1;
        bool ilkyukleme = false;
        short yazdirmaadedi = 1;
        string _fisno = "0";
        public ucSatis(string fisno)
        {
            InitializeComponent();
            DB.PkFirma = 1;
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            this.Visible = true;
            _fisno = fisno;
            if(_fisno!="0")
            {
                Satis1Firma.Visible = false;
                Satis2Firma.Visible = false;
                Satis3Firma.Visible = false;
                Satis4Firma.Visible = true;
            }
        }

        private void ucAnaEkran_Load(object sender, EventArgs e)
        {
            #region Şirketler
            DataTable dtSirketler = DB.GetData("select * from Sirketler with(nolock)");

            if (dtSirketler.Rows[0]["FaturaTipi"].ToString() != "")
            {
                cbFaturaTipi.SelectedIndex = int.Parse(dtSirketler.Rows[0]["FaturaTipi"].ToString());
            }
            if (dtSirketler.Rows[0]["OncekiFiyatHatirla"].ToString() != "")
            {
                btnFisKopyala.Visible = Degerler.Satiskopyala;
                cbOncekiFiyatHatirla.Visible = Degerler.OncekiFiyatHatirla;
                cbOncekiFiyatHatirla.Checked = Degerler.OncekiFiyatHatirla;
            }
            Degerler.e_fatura_kul = dtSirketler.Rows[0]["e_fatura_kul"].ToString();
            Degerler.e_fatura_sifre = dtSirketler.Rows[0]["e_fatura_sifre"].ToString();
            Degerler.e_fatura_url = dtSirketler.Rows[0]["e_fatura_url"].ToString();
            #endregion

            ilkyukleme = true;

            Fiyatlarigetir();

            SatisTipiDoldur();

            GeciciMusteriDefault();

            timer1.Enabled = true;

            cbOdemeSekli.SelectedIndex = -1;
            BankalariDoldur();

            lueFiyatlar.ItemIndex = 0;
            ilkyukleme = false;

            KullaniciListesi();

            if (gcolbirimi.Visible == true)
                vBirimler();

            vParaBirimleri();

            //Yetkiler();aşağı alındı

            DepolarLueDoldur();
            DepolarGridComboboxDoldur();

            //if (Degerler.fkKullaniciGruplari == "1")
            //    cbGecmisFisler.Checked = true;

            string Dosya = DB.exeDizini + "\\SatisFaturaGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
            
            YetkilerGizleGoster();
            ModulYetkiler();

            HizliSatisTablariolustur();
            //System.Threading.Thread islem = new System.Threading.Thread(new System.Threading.ThreadStart(HizliSatisTablariolustur));
            //islem.Start();

            yesilyak();
        }

        void SatisTipiDoldur()
        {
            lueSatisTipi.Properties.DataSource = DB.GetData(@"SELECT pkSatisDurumu, Durumu, Aktif, SiraNo FROM  SatisDurumu with(nolock) WHERE Aktif = 1 ORDER BY SiraNo");
            lueSatisTipi.Tag = "1";
            lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
            lueSatisTipi.Tag = "0";
        }

        void YetkilerGizleGoster()
        {
            string sql = @"SELECT ya.Yetki, p.Aciklama10,ya.Sayi,isnull(p.Aktif,0) as Aktif FROM  YetkiAlanlari ya with(nolock)  
            INNER JOIN Parametreler p with(nolock) ON ya.fkParametreler = p.pkParametreler
            WHERE p.fkModul =1 and  ya.fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);

            ParaPanel.Visible = false;
            xtraTabControl3.Visible = false;
            //btnSatisRaporlari.Enabled = false;
            //lueKullanicilar.Enabled = false;
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string aciklama = dtYetkiler.Rows[i]["Aciklama10"].ToString();
                bool yetki = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                string sayi = dtYetkiler.Rows[i]["Sayi"].ToString();
                //bool aktif = Convert.ToBoolean(dtYetkiler.Rows[i]["Aktif"]);


                if (aciklama == "ParaPanel")
                    ParaPanel.Visible = yetki;

                else if (aciklama == "HizliButon")
                    xtraTabControl3.Visible = yetki;
                else if (aciklama == "HizButGen")
                {
                    int gen = 117;
                    int.TryParse(dtYetkiler.Rows[i]["Sayi"].ToString(), out gen);
                    dockPanel1.Tag = gen;
                    if (gen > 0)
                    {
                        dockPanel1.Width = gen;
                        dockPanel1.SendToBack();
                        lcKullanici.Tag = "1";
                    }
                }
                else if (aciklama == "SonSatisM")
                {
                    //gcMusteriSatis.Visible = yetki;
                }
                else if (aciklama == "AlisFiyati")
                {
                    bUFİŞMALİYETİToolStripMenuItem.Enabled = yetki;
                    //gridColumn31.Visible = yetki;
                }
                else if (aciklama == "kullanici")
                {
                    lueKullanicilar.Enabled = yetki;
                    //gridColumn31.Visible = yetki;
                }
                else if (aciklama == "FiyatGor")
                {
                    simpleButton5.Visible = yetki;
                }
                else if (aciklama == "YeniStok")
                {
                    btnYeniStokEkle.Visible = yetki;
                }
                else if (aciklama == "satstkhare")
                {
                    pmenuStokHareketleri.Enabled = yetki;
                }
                else if (aciklama == "SonFisler")
                {
                    if(yetki)
                       btnGecmisFis.Tag = 1;
                    else
                        btnGecmisFis.Tag = 0;
                }
                else if (aciklama == "OFK")
                {
                    btnOFK.Visible = yetki;
                }

            }
        }

        void ModulYetkiler()
        {
            //Kod,ModulAdi,Yetki
            DataTable dtYetkiler = islemler.yetkiler.ModulYetkileri(int.Parse(DB.fkKullanicilar));
            foreach (DataRow row in dtYetkiler.Rows)
            {
                string kod = row["Kod"].ToString();
                string yetki = row["Yetki"].ToString();

                if (kod == ((int)Degerler.Moduller.Satis).ToString())
                {
                    if (yetki == "True")
                        this.Enabled = true;
                    else
                        this.Enabled = false;
                }
                else if (kod == ((int)Degerler.Moduller.SatisRaporlari).ToString())
                {
                    if (yetki == "True")
                        btnSatisRaporlari.Enabled = true;
                    else
                        btnSatisRaporlari.Enabled = false;
                }

            }
        }

        void BankalariDoldur()
        {
            lueKKarti.Properties.DataSource = DB.GetData("select * from Bankalar with(nolock) where Aktif=1");
            lueKKarti.ItemIndex = 0;
        }

        void DepolarLueDoldur()
        {
            lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock)");
            lueDepolar.EditValue = Degerler.fkDepolar;
        }

        void DepolarGridComboboxDoldur()
        {
            repositoryItemLookUpEdit2.DataSource = DB.GetData("select * from Depolar with(nolock)");
            //repositoryItemLookUpEdit3.EditValue = 1;
        }

        void vParaBirimleri()
        {
            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from ParaBirimi with(nolock)");
        }

        void vBirimler()
        {
            repositoryItemComboBox1.Items.Clear();
            DataTable dtBirimler = DB.GetData("select pkBirimler,BirimAdi,Varsayilan from Birimler where Aktif=1");
            for (int i = 0; i < dtBirimler.Rows.Count; i++)
            {
                repositoryItemComboBox1.Items.Add(dtBirimler.Rows[i]["BirimAdi"].ToString());
            }
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

        void Fiyatlarigetir()
        {
            lueFiyatlar.Properties.DataSource = DB.GetData("select * from SatisFiyatlariBaslik with(nolock) where Aktif=1 order by pkSatisFiyatlariBaslik");
            //if (!ilkyukleme)
            lueFiyatlar.EditValue = Degerler.fkSatisFiyatlariBaslik;
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
                SatisDetayEkle(((SimpleButton)sender).Tag.ToString());
                yesilisikyeni();
            }
        }

        private void ButtonClickSag(object sender, MouseEventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
                urunekle(((SimpleButton)sender).Tag.ToString());
        }

        void Hizlibuttonlariyukle(bool bdegisiklikvar)
        {
            if (bdegisiklikvar == false && xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Count > 0)
                return;

            xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();

            int to = 0;
            int lef = 0;

            DataTable dtbutton = DB.GetData(@"select pkHizliStokSatis,pkStokKarti,Barcode,HizliSatisAdi,Stokadi,sf.SatisFiyatiKdvli as SatisFiyati from HizliStokSatis  h with(nolock)
            left join (select pkStokKarti,Stokadi,fkStokGrup,HizliSatisAdi,Barcode,HizliSiraNo 
            from StokKarti with(nolock) ) sk on sk.pkStokKarti=h.fkStokKarti 
            left join SatisFiyatlari sf on sf.fkStokKarti=h.fkStokKarti  and sf.fkSatisFiyatlariBaslik=(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik where Tur=1)
            where fkHizliGruplar=" + xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Tag.ToString() +
            " order by pkHizliStokSatis");

            int h = 80;//dockPanel1.Height / 7;
            int w = 110;//dockPanel1.Width / 5;
            try
            {


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
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu: " + exp.Message);
                throw;
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

        //satış iptal butonu
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                GeciciMusteriDefault();
                yesilisikyeni();
                return;
            }

            string s = formislemleri.MesajBox("Satış İptal Edilsin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            string sonuc = "", pkSatislar = "0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            if (AcikSatisindex == 2)
                pkSatislar = Satis2Toplam.Tag.ToString();
            if (AcikSatisindex == 3)
                pkSatislar = Satis3Toplam.Tag.ToString();
            if (AcikSatisindex == 4)
                pkSatislar = Satis4Toplam.Tag.ToString();

            sonuc = DB.ExecuteScalarSQL("EXEC spSatisSil " + pkSatislar + ","+DB.fkKullanicilar);

            if (sonuc != "Satis Silindi.")
                MessageBox.Show(sonuc);

            txtpkSatislar.Text = "0";
            SatisTemizle();

            Satis1ToplamGetir(AcikSatisindex);

            deGuncellemeTarihi.EditValue = null;
            deGuncellemeTarihi.Enabled = false;
            //cbOdemeSekli.Enabled = false;

            yesilisikyeni();

            lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
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
                string s = formislemleri.MesajBox("Ürün Kalmayacağı için, Satış İptal Edilsin mi?", Degerler.mesajbaslik, 3, 1);
                if (s == "0")
                    return;
            }


            int sonuc = DB.ExecuteSQL("DELETE FROM SatisDetay WHERE pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            if (sonuc != 1)
            {
                MessageBox.Show("Satış Detay F2 sonuc:" + sonuc);
                return;
            }

            //gridView1.DeleteSelectedRows();

            // if (gridView1.DataRowCount == 0)// && AcikSatisindex!=4)
            if (DB.GetData("select * from SatisDetay with(nolock) where fkSatislar=" + txtpkSatislar.Text).Rows.Count == 0)
            {
                DB.ExecuteSQL("DELETE FROM Satislar WHERE pkSatislar=" + txtpkSatislar.Text);
                txtpkSatislar.Text = "0";
                if (AcikSatisindex == 1)
                    Satis1Toplam.Tag = "0";
                else if (AcikSatisindex == 2)
                    Satis2Toplam.Tag = "0";
                else if (AcikSatisindex == 3)
                    Satis3Toplam.Tag = "0";

                txtpkSatislar.Text = "0";
                SatisTemizle();

                deGuncellemeTarihi.EditValue = null;
                deGuncellemeTarihi.Enabled = false;
                //cbOdemeSekli.Enabled = false;

                if (AcikSatisindex == 4)
                    Yenile();
                else
                    GeciciMusteriDefault();
            }
            //else
            //  MessageBox.Show("Lütfen Yenile Butonuna basınız");

            yesilisikyeni();

            //lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }

        void YeniSatisEkle()
        {
            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
                YeniSatis();
            else if (AcikSatisindex == 2 && Satis2Toplam.Tag.ToString() == "0")
                YeniSatis();
            else if (AcikSatisindex == 3 && Satis3Toplam.Tag.ToString() == "0")
                YeniSatis();
            else if (AcikSatisindex == 4 && Satis4Toplam.Tag.ToString() == "0")
                YeniSatis();
        }

        void iadebasildi()
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr["iade"].ToString() == "True")
            {
                DB.ExecuteSQL("update SatisDetay Set iade=0,Adet=abs(Adet) where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
                gridView1.SetFocusedRowCellValue("iade", "False");
            }
            else
            {
                DB.ExecuteSQL("update SatisDetay Set iade=1,Adet=abs(Adet)*-1 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
                gridView1.SetFocusedRowCellValue("iade", "True");
            }
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
                SatisDetayEkle(kod);
                yesilisikyeni();
            }
            //müşteri koduna git
            else if (e.KeyValue == 77)
            {
                if (AcikSatisindex == 1) Satis1Baslik.Focus();
                else if (AcikSatisindex == 2) Satis2Baslik.Focus();
                else if (AcikSatisindex == 3) Satis3Baslik.Focus();
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
                iadebasildi();
                //iskontoyagit_ctrli();
            }
            else if (e.Control && e.Shift && gridView1.DataRowCount > 0)
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
            }
            else if (e.Control && (e.KeyValue == 84 || e.KeyValue == 116))//T veya t
            {
                frmFisAciklama fFisAciklama = new frmFisAciklama();
                fFisAciklama.panelControl2.Visible = false;
                fFisAciklama.btnCancel.Visible = true;
                fFisAciklama.btnTemizle.Visible = false;
                fFisAciklama.Text = "Tutar Hesapla";
                fFisAciklama.pcTutarHesapla.Visible = true;
                fFisAciklama.pcTutarHesapla.BringToFront();

                int si = gridView1.FocusedRowHandle;
                DataRow dr = gridView1.GetDataRow(si);
                if (dr == null) return;

                decimal tutar = decimal.Parse(dr["Tutar"].ToString());
                decimal nakitfiyati = decimal.Parse(dr["NakitFiyati"].ToString());
                decimal iskontotutar, iskonyuzdetotutar = 0;
                decimal adet = decimal.Parse(dr["Adet"].ToString());
                decimal kdvorani = decimal.Parse(dr["KdvOrani"].ToString());

                fFisAciklama.seMiktar.Value = adet;
                fFisAciklama.ceTutari.Value = tutar;

                fFisAciklama.ShowDialog();
                if (fFisAciklama.Tag.ToString() == "0")
                 {
                    DevExpress.XtraGrid.Columns.GridColumn secilenGridColumn = gridView1.FocusedColumn;
                    yesilisikyeni();
                    gridView1.FocusedRowHandle = gridView1.FocusedRowHandle;
                    gridView1.FocusedColumn = secilenGridColumn;
                    gridView1.CloseEditor();
                    return;
                 }

                    decimal aliskdvdahil, aliskdvharic, satisfiyati;
                    satisfiyati = fFisAciklama.calcEdit1.Value;
                    //kdv dahil ise
                    if (cbFaturaTipi.SelectedIndex == 0)
                    {
                        aliskdvdahil = satisfiyati;
                        //kdvtutar = (SatisFiyatiKdv * kdv) / (100 + kdv);
                        aliskdvharic = satisfiyati - ((satisfiyati * kdvorani) / (100 + kdvorani));
                    }
                    else
                    {
                        aliskdvdahil = satisfiyati + (fFisAciklama.calcEdit1.Value * kdvorani) / 100;
                        aliskdvharic = satisfiyati;
                    }

                    iskontotutar = nakitfiyati - satisfiyati;
                    iskonyuzdetotutar = (iskontotutar * 100) / nakitfiyati;

                    int s = DB.ExecuteSQL("update SatisDetay set iskontotutar=" + iskontotutar.ToString().Replace(",", ".") +
                         ",iskontoyuzdetutar=" + iskonyuzdetotutar.ToString().Replace(",", ".") +
                        " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());

                    if (s != 1)
                        MessageBox.Show("Hata Oluştu: " + s.ToString());

                    SatisDetayYenile2();

                    gridView1.FocusedColumn = gridColumn1;
                    gridView1.FocusedRowHandle = si;

                    gridView1.ShowEditor();
            }
        }

        void urunaraekle(string ara)
        {
            frmStokAra StokAra = new frmStokAra(ara);

            StokAra.Tag = "0";
            StokAra.ShowDialog();

            if (StokAra.TopMost == false)
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    SatisDetayEkle(dr["Barcode"].ToString());
                }
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
                girilen = ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
            }
            catch
            {
                girilen = "";
            }
            urunaraekle(girilen);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            if (gridView1.FocusedRowHandle != -2147483647)
                gridView1.AddNewRow();

            string girilen =
            ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;


            urunaraekle(girilen);
        }

        private void urunekle(string barkod)
        {
            if (barkod == "") return;

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
            SatisDetayEkle(barkod);
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

        private bool SatisVar()
        {

            if (gridView1.DataRowCount == 0)
            {
                Showmessage("Önce Satış Yapınız!", "K");
                return false;
            }
            return true;
        }

        private void simpleButton38_Click(object sender, EventArgs e)
        {
            if (OnceSatisYapiniz() == false) return;
            alinanpara.Value = alinanpara.Value + 5;
            SatisGetir();
            yesilisikyeni();
        }

        private void simpleButton39_Click(object sender, EventArgs e)
        {
            if (OnceSatisYapiniz() == false) return;
            alinanpara.Value = alinanpara.Value + 10;
            SatisGetir();
            yesilisikyeni();
        }

        private void simpleButton40_Click(object sender, EventArgs e)
        {
            if (OnceSatisYapiniz() == false) return;
            alinanpara.Value = alinanpara.Value + 20;
            SatisGetir();
            yesilisikyeni();
        }

        private void simpleButton41_Click(object sender, EventArgs e)
        {
            if (OnceSatisYapiniz() == false) return;
            alinanpara.Value = alinanpara.Value + 50;
            SatisGetir();
            yesilisikyeni();
        }

        private void simpleButton42_Click(object sender, EventArgs e)
        {
            if (OnceSatisYapiniz() == false) return;
            alinanpara.Value = alinanpara.Value + 100;
            SatisGetir();
            yesilisikyeni();
        }

        private void simpleButton43_Click(object sender, EventArgs e)
        {
            if (OnceSatisYapiniz() == false) return;
            alinanpara.Value = alinanpara.Value + 200;
            SatisGetir();
            yesilisikyeni();
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

                //Satis1Baslik.Text = DB.FirmaAdi;
                //Satis1Firma.Tag = DB.PkFirma;
                //Satis2Baslik.Text = DB.FirmaAdi;
                //Satis2Firma.Tag = DB.PkFirma;
                //Satis3Baslik.Text = DB.FirmaAdi;
                //Satis3Firma.Tag = DB.PkFirma;

                //sıfırla
                Satis1Firma.Tag = DB.PkFirma;
                Satis1Baslik.ToolTip = DB.FirmaAdi;
                Satis1Baslik.Text = DB.FirmaAdi;

                Satis2Firma.Tag = DB.PkFirma;
                Satis2Baslik.ToolTip = DB.FirmaAdi;
                Satis2Baslik.Text = DB.FirmaAdi;

                Satis3Firma.Tag = DB.PkFirma;
                Satis3Baslik.ToolTip = DB.FirmaAdi;
                Satis3Baslik.Text = DB.FirmaAdi;

                Satis4Firma.Tag = DB.PkFirma;
                Satis4Baslik.ToolTip = DB.FirmaAdi;
                Satis4Baslik.Text = DB.FirmaAdi;

                teBakiyeDevir.Text = "0,00";
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
                Satis1Baslik.ToolTip = DB.FirmaAdi;
                Satis1Firma.Tag = DB.PkFirma;
            }
            else if (aciksatisno == 2)
            {
                Satis2Baslik.Text = DB.FirmaAdi;
                Satis2Baslik.ToolTip = DB.FirmaAdi;
                Satis2Firma.Tag = DB.PkFirma;
            }
            else if (aciksatisno == 3)
            {
                Satis3Baslik.Text = DB.FirmaAdi;
                Satis3Baslik.ToolTip = DB.FirmaAdi;
                Satis3Firma.Tag = DB.PkFirma;
            }
            else if (aciksatisno == 4)
            {
                Satis4Baslik.ToolTip = DB.FirmaAdi;
                Satis4Baslik.Text = DB.FirmaAdi;
                Satis4Firma.Tag = DB.PkFirma;
            }

            btnAciklamaGirisi.ToolTip = "";
            txtEFaturaUUID.Text = "";
            gcSatisDetay.DataSource = null;
            paraustu.Text = "0";
            lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
            lueFiyatlar.EditValue = Degerler.fkSatisFiyatlariBaslik;
            paraustu.BackColor = System.Drawing.Color.DimGray;
            //deGuncellemeTarihi.Enabled = false;
            deGuncellemeTarihi.EditValue = null;
            deFaturaTarihi.EditValue = null;
            deTeklifTarihi.EditValue = null;
            teBakiyeDevir.Text = "0,00";
            txtFaturaNo.Text = "";
            teSeriNo.Text = "";
            alinanpara.Value = 0;
        }

        public bool bir_nolumusteriolamaz()
        {
            if (DB.GetData("select * from Sirketler with(nolock)").Rows[0]["MusteriZorunluUyari"].ToString() == "True")
            {
                if ((AcikSatisindex == 1 && Satis1Firma.Tag.ToString() == "1") ||
                    (AcikSatisindex == 2 && Satis2Firma.Tag.ToString() == "1") ||
                    (AcikSatisindex == 3 && Satis3Firma.Tag.ToString() == "1"))
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Müşteri Olamaz.\n (Ayarlardan Kaldırabilirsiniz.)!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    musteriara();
                    return false;
                }
            }
            return true;
        }

        void SatisTemizle()
        {
            if (AcikSatisindex == 1)
            {
                Satis1Toplam.Tag = 0;
                Satis1Toplam.Text = "0,0";
                txtpkSatislar.Text = "0";
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
            else if (AcikSatisindex == 4)
            {
                Satis4Firma.Visible = false;
                Satis4Toplam.Tag = "0";
            }

            txtpkSatislar.Text = "0";

            cbOdemeSekli.SelectedIndex = -1;

            lueKKarti.Visible = false;
            deTeklifTarihi.EditValue = null;
            gcMusteriSatis.DataSource = null;
            deOdemeTarihi.EditValue = null;
            cbTevkifat.SelectedIndex = 0;

            temizle(AcikSatisindex);
            //fiş düzeltten sonra 1.satışa git
            if (AcikSatisindex == 4)
                Satis1ToplamGetir(1);
        }

        void KaydetYazdir(bool yazdirilsinmi,bool iskaydet)
        {
            if (txtpkSatislar.Text == "0")
            {
                yesilisikyeni();
                return;
            }

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura)
            {
                if (deFaturaTarihi.EditValue == null)
                    deFaturaTarihi.DateTime = DateTime.Now;
            }

            #region Müşteri id al
            string fkFirma = "0";
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
                case 4:
                    {
                        fkFirma = Satis4Firma.Tag.ToString();
                        break;
                    }
            }
            #endregion

            #region uyarılar ve update
            if (deGuncellemeTarihi.EditValue == null)
                deGuncellemeTarihi.DateTime = DateTime.Now;

            TimeSpan ts = DateTime.Now - deGuncellemeTarihi.DateTime;
            if (ts.Days > 0 || ts.Hours > 0 && deGuncellemeTarihi.EditValue != null)// &&  AcikSatisindex < 4)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Tarihi Farkı " + ts.Days.ToString() + " Gün " +
                    ts.Hours.ToString() + " Saattir. Devam Etmek İstiyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);

                if (secim == DialogResult.No)
                {
                    yesilisikyeni();
                    return;
                }
            }

            if (deGuncellemeTarihi.DateTime > DateTime.Now)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Zamanı Bugünden Büyüktür. Yinede Kaydetmek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);//, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (secim == DialogResult.No) return;
            }

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Teklif && fkFirma == "1")
            {
                formislemleri.Mesajform("1 Nolu Müşteriye Teklif Yapılamaz", "K", 150);

                yesilisikyeni();
                //musteriara();
                return;
            }

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Sipariş && fkFirma == "1")
            {
                formislemleri.Mesajform("1 Nolu Müşteriye Sipariş Yapılamaz", "K", 150);

                yesilisikyeni();
                //musteriara();
                return;
            }

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.İrsaliye && fkFirma == "1")
            {
                formislemleri.Mesajform("1 Nolu Müşteriye İrsaliye Kesilemez", "K", 150);

                yesilisikyeni();
                //musteriara();
                return;
            }

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.SFatura && fkFirma == "1")
            {
                formislemleri.Mesajform("1 Nolu Müşteriye Sanal Fatura Kesilemez", "K", 200);

                yesilisikyeni();
                //musteriara();
                return;
            }

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura && fkFirma == "1")
            {
                formislemleri.Mesajform("1 Nolu Müşteriye Fatura Kesilemez", "K", 200);

                yesilisikyeni();
                //musteriara();
                return;
            }

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura && deFaturaTarihi.EditValue == null)
            {
                formislemleri.Mesajform("Fatura Tarihi Giriniz", "K", 200);
                deFaturaTarihi.Focus();
                yesilisikyeni();
                return;
            }

            //if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura && txtFaturaNo.Text == "")
            //{
            //    formislemleri.Mesajform("Fatura Numarası Giriniz", "K", 200);
            //    txtFaturaNo.Focus();
            //    yesilisikyeni();
            //    return;
            //}

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Tevkifat && fkFirma == "1")
            {
                formislemleri.Mesajform("1 Nolu Müşteriye Tevkifatlı Fatura Kesilemez", "K", 200);

                yesilisikyeni();
                //musteriara();
                return;
            }
            #endregion

            #region teklif ise kasa hareketi ekleme
            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Teklif)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Teklif Bilgilerini Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                if (secim == DialogResult.No) return;

                if (deTeklifTarihi.EditValue == null)
                    deTeklifTarihi.DateTime = DateTime.Now;

                int sonuc2 = DB.ExecuteSQL_Sonuc_Sifir("Update Satislar set Siparis=1,fkSatisDurumu=1,GuncellemeTarihi='"+ deGuncellemeTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") + "'," +
                    "TeklifTarihi='" + deTeklifTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',ToplamTutar=" +
                    aratoplam.Value.ToString().Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                if (sonuc2 != 0)
                {
                    formislemleri.Mesajform("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz", "K", 200);
                    yesilisikyeni();
                    return;
                }

                SatisTemizle();

                temizle(AcikSatisindex);

                yesilisikyeni();
                return;
            }
            #endregion

            #region sipariş ise kasa hareketi ekleme
            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Sipariş)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Sipariş Bilgilerini Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                if (secim == DialogResult.No) return;

                if (deTeklifTarihi.EditValue == null)
                    deTeklifTarihi.DateTime = DateTime.Now;

                int sonuc2 = DB.ExecuteSQL_Sonuc_Sifir("Update Satislar set Siparis=1,fkSatisDurumu=10,GuncellemeTarihi=getdate()," +
                    "TeklifTarihi='" + deTeklifTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',ToplamTutar=" +
                    aratoplam.Value.ToString().Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                if (sonuc2 != 0)
                {
                    formislemleri.Mesajform("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz", "K", 200);
                    yesilisikyeni();
                    return;
                }

                SatisTemizle();

                temizle(AcikSatisindex);

                yesilisikyeni();
                return;
            }
            #endregion

            #region irsaliye ise kasa hareketi ekleme
            //if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.İrsaliye)
            //{
            //    DialogResult secim;
            //    secim = DevExpress.XtraEditors.XtraMessageBox.Show("İrsaliye Bilgilerini Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
            //    if (secim == DialogResult.No) return;

            //    if (deTeklifTarihi.EditValue == null)
            //        deTeklifTarihi.DateTime = DateTime.Now;

            //    int sonuc2 = DB.ExecuteSQL_Sonuc_Sifir("Update Satislar set Siparis=1,fkSatisDurumu=3,GuncellemeTarihi=getdate()," +
            //        "TeslimTarihi='" + deTeklifTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',ToplamTutar=" +
            //        aratoplam.Value.ToString().Replace(",", ".") +
            //    " where pkSatislar=" + txtpkSatislar.Text);

            //    if (sonuc2 != 0)
            //    {
            //        formislemleri.Mesajform("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz", "K", 200);
            //        yesilisikyeni();
            //        return;
            //    }

            //    SatisTemizle();

            //    temizle(AcikSatisindex);

            //    yesilisikyeni();
            //    return;
            //}
            #endregion

            #region Sanal Fatura

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.SFatura)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("S.Fatura Bilgilerini Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                if (secim == DialogResult.No) return;

                if (deFaturaTarihi.EditValue == null)
                    deFaturaTarihi.DateTime = DateTime.Now;

                DB.ExecuteSQL("Update Satislar set Siparis=1,fkSatisDurumu=11,GuncellemeTarihi='" + deGuncellemeTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") +"'," +
                    "FaturaTarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") + "',ToplamTutar=" +
                    aratoplam.Value.ToString().Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                SatisTemizle();

                temizle(AcikSatisindex);

                yesilisikyeni();
                return;
            }
            //şimdilik fkSatisDurumu olsun ama kod alanından gidilecek TODO:
            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Tevkifat)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("S.Fatura Bilgilerini Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                if (secim == DialogResult.No) return;

                if (deFaturaTarihi.EditValue == null)
                    deFaturaTarihi.DateTime = DateTime.Now;

                DB.ExecuteSQL("Update Satislar set Siparis=1,fkSatisDurumu=12,GuncellemeTarihi='"+ deGuncellemeTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") + "'," +
                    "FaturaTarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd") + "',ToplamTutar=" +
                    aratoplam.Value.ToString().Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                SatisTemizle();

                temizle(AcikSatisindex);

                yesilisikyeni();
                return;
            }
            #endregion

            #region Satış fatura Kontrol
            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura && fkFirma == "1")
            {
                //formislemleri.Mesajform("1 Nolu Müşteriye Fatura Kesilemez", "K", 200);
                //yesilisikyeni();
                musteriara();
                return;
            }
            #endregion

            #region Satış Kontrol
            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Satış && fkFirma == "1" && cbOdemeSekli.SelectedIndex == 2)
            {
                //formislemleri.Mesajform("1 Nolu Müşteriye Açık Hesap Yapılamaz", "K", 200);
                //yesilisikyeni();
                musteriara();
                return;
            }
            #endregion

            #region update fatura tarihi
            string sql = "";
            if (deGuncellemeTarihi.EditValue == null)
                deGuncellemeTarihi.DateTime = DateTime.Now;

            sql = "UPDATE Satislar Set GuncellemeTarihi='" +
                 deGuncellemeTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") + "',AlinanPara="
                 + alinanpara.Value.ToString().Replace(",", ".");

            if (lueKKarti.EditValue != null && cbOdemeSekli.Text !="Nakit" 
                && cbOdemeSekli.Text != "Diğer" && cbOdemeSekli.Text != "Açık Hesap")
                sql = sql + ",fkBankalar=" + lueKKarti.EditValue.ToString();
            else
                sql = sql + ",fkBankalar=0";

            if (deFaturaTarihi.EditValue == null || int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Satış)
                sql = sql + ",FaturaNo=null,FaturaTarihi=null";
            else
                sql = sql + ",FaturaNo='" + txtFaturaNo.Text + "',FaturaTarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd HH:mm").ToString() + "'";

            sql = sql + ",fkDonemler='0'";
            //aa
            sql = sql + ",ToplamTutar="+ aratoplam.Value.ToString().Replace(",",".");

            sql = sql + " where pkSatislar=" + txtpkSatislar.Text;

            int sonuc = DB.ExecuteSQL_Sonuc_Sifir(sql);

            //TODO:sonra düzenle 26.04.2016
            //sql = @"update SatisDetay set iskontotutar=0
            //where iskontoyuzdetutar=0 and iskontotutar<>0 AND fkSatislar="+ pkSatisBarkod.Text;
            //DB.ExecuteSQL_Sonuc_Sifir(sql);

            //hata varsa
            if (sonuc == -1)
            {
                MessageBox.Show("Hata Oluştu Lütfen Bağlantıyı Kontro Ediniz Sonuç=" + sonuc.ToString());
                return;
            }
            #endregion

            #region Satış Ödeme Kayıt yazdır
            frmSatisOdeme SatisOdeme = new frmSatisOdeme(txtpkSatislar.Text, yazdirilsinmi, cbOdemeSekli.SelectedIndex, iskaydet);
            SatisOdeme.MusteriAdi.Tag = fkFirma;
            SatisOdeme.ceSatisTutariGercek.EditValue = aratoplam.EditValue;
            SatisOdeme.satistutari.EditValue = aratoplam.EditValue;
            SatisOdeme.ShowDialog();
            //kaydedilecek mi?
            if (!iskaydet)
            {
                yesilisikyeni();
                return;
            }
            //kaydet veya yazdır ise işlem başarılı ise
            if (SatisOdeme.Tag.ToString() == ((int)Degerler.islemDurumu.Basarili).ToString())
            {
                //KASA HAREKETLERİ TARİHİ GÜNCELLE
                DB.ExecuteSQL("UPDATE KasaHareket Set Tarih='" + deGuncellemeTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") +
                  "' where fkSatislar=" + txtpkSatislar.Text);

                #region Toplu Fatura için Güncelle
                if (lueSatisTipi.EditValue.ToString() == ((int)Degerler.SatisDurumlari.Fatura).ToString())
                {
                    //                    DataTable dtFirma =
                    //                    DB.GetData("select Adres,VergiDairesi,VergiNo from Firmalar with(nolock) where pkFirma=" + fkFirma);
                    //                    ArrayList list = new ArrayList();
                    //                    list.Add(new SqlParameter("@FaturaNo", txtFaturaNo.Text));
                    //                    list.Add(new SqlParameter("@FaturaTarihi", DateTime.Today));
                    //                    list.Add(new SqlParameter("@Aciklama", "Fiş No=" + pkSatisBarkod.Text));//dt.Rows[0]["Firmaadi"].ToString()));

                    //                    list.Add(new SqlParameter("@FaturaAdresi", dtFirma.Rows[0]["Adres"].ToString()));
                    //                    list.Add(new SqlParameter("@VergiDairesi", dtFirma.Rows[0]["VergiDairesi"].ToString()));
                    //                    list.Add(new SqlParameter("@VergiNo", dtFirma.Rows[0]["VergiNo"].ToString()));

                    //                    list.Add(new SqlParameter("@Yazdirildi", "1"));
                    //                    list.Add(new SqlParameter("@fkFirma", fkFirma));
                    //                    list.Add(new SqlParameter("@fkSatislar", pkSatisBarkod.Text));
                    //                    list.Add(new SqlParameter("@SeriNo", teSeriNo.Text));
                    //                    list.Add(new SqlParameter("@Tutar", aratoplam.EditValue.ToString().Replace(",", ".")));
                    //                    list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                    //                    list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));

                    //                    string maxfaturaid = DB.ExecuteScalarSQL(@"insert into FaturaToplu (fkFirma,FaturaNo,Tarih,FaturaTarihi,Aciklama,FaturaAdresi,
                    //                        VergiDairesi,VergiNo,Yazdirildi,fkSatislar,SeriNo,Tutar,BilgisayarAdi,fkKullanici)
                    //                        values(@fkFirma,@FaturaNo,GETDATE(),@FaturaTarihi,@Aciklama,@FaturaAdresi,@VergiDairesi,@VergiNo,@Yazdirildi,@fkSatislar,@SeriNo,@Tutar,@BilgisayarAdi,@fkKullanici)  
                    //                        SELECT IDENT_CURRENT('FaturaToplu')", list);

                    //                    DB.ExecuteSQL("update Kullanicilar set FaturaNo=FaturaNo+1 where pkKullanicilar=" + DB.fkKullanicilar);

                    //DB.ExecuteSQL("update SatisDetay set fkFaturaDurumu=1,fkFaturaToplu=" + maxfaturaid + " where fkSatislar=" + pkSatisBarkod.Text);

                    DB.ExecuteSQL("update SatisDetay set fkFaturaDurumu=1,fkFaturaToplu=-1 where fkSatislar=" + txtpkSatislar.Text);

                    //DB.ExecuteSQL("insert into BelgeTakip (fkFaturaToplu,fkSatislar,fkKullanici,Tarih,fkBelgeDurumu,Aciklama) values(" +maxfaturaid+","+
                    //    pkSatisBarkod.Text+","+DB.fkKullanicilar+",getdate(),"+ (int)Degerler.BelgeDurumu.Yeni +",'Yeni Satış Faturası')"); 

                }
                #endregion

                deGuncellemeTarihi.EditValue = null;
                deGuncellemeTarihi.Enabled = false;

                lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
                lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);

                DB.ExecuteSQL("UPDATE Firmalar Set Aktarildi=0,SonSatisTarihi=getdate() where pkFirma=" + fkFirma);

                MevcutlariGuncelle();

                //StokDurumGuncelle();

                DepoMevcurlariGuncelle();

                #region taksit EKLE

                if (deOdemeTarihi.EditValue != null)
                {
                    //string sql = "";
                    #region taksit ekle
                    ArrayList listt = new ArrayList();
                    listt.Add(new SqlParameter("@fkFirma", fkFirma));
                    listt.Add(new SqlParameter("@aciklama", "Satış Ödeme Taksitdi"));
                    listt.Add(new SqlParameter("@kefil", ""));
                    listt.Add(new SqlParameter("@mahkeme", ""));
                    listt.Add(new SqlParameter("@fkSatislar", txtpkSatislar.Text));
                    listt.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));

                    sql = @"insert into Taksit(fkFirma,tarih,aciklama,kefil,mahkeme,fkSatislar,fkKullanici)
                    values(@fkFirma,getdate(),@aciklama,@kefil,@mahkeme,@fkSatislar,@fkKullanici) SELECT IDENT_CURRENT('Taksit')";

                    string taksit_id = DB.ExecuteScalarSQL(sql, listt);
                    //if(taksit_id=="")
                    #endregion

                    ArrayList listtt = new ArrayList();
                    //list.Add(new SqlParameter("@fkFirma", teMusteri.Tag.ToString()));
                    listtt.Add(new SqlParameter("@Tarih", deOdemeTarihi.DateTime.ToString("yyyy-MM-dd 10:00")));
                    listtt.Add(new SqlParameter("@Odenecek", aratoplam.Value.ToString().Replace(",", ".")));
                    listtt.Add(new SqlParameter("@Odenen", "0"));
                    listtt.Add(new SqlParameter("@SiraNo", "1"));
                    listtt.Add(new SqlParameter("@HesabaGecisTarih", DBNull.Value));
                    listtt.Add(new SqlParameter("@taksit_id", taksit_id));
                    listtt.Add(new SqlParameter("@fkSatislar", txtpkSatislar.Text));
                    listtt.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));

                    DB.ExecuteSQL("INSERT INTO Taksitler (Tarih,Odenecek,Odenen,SiraNo,HesabaGecisTarih,OdemeSekli,Kaydet,taksit_id,fkevraktipi,fkSatislar,kayit_tarihi,fkKullanici)" +
                        " VALUES(@Tarih,@Odenecek,@Odenen,@SiraNo,@HesabaGecisTarih,'Taksit (Senet)',1,@taksit_id,3,@fkSatislar,getdate(),@fkKullanici)", listtt);

                    DBislemleri.YeniHatirlatmaEkle("Satış Anımsatması", deOdemeTarihi.DateTime, "Açık Hesap " + aratoplam.Value.ToString().Replace(",","."), deOdemeTarihi.DateTime.AddHours(1), fkFirma, txtpkSatislar.Text, "0","0");
                }
                #endregion

                DB.ExecuteSQL(@"update Satislar set fkPerTeslimEden=Firmalar.fkPerTeslimEden from Firmalar
                where  Firmalar.pkFirma=Satislar.fkFirma and Satislar.pkSatislar=" + txtpkSatislar.Text);

                SatisTemizle();
            }
            else if (SatisOdeme.Tag.ToString() == ((int)Degerler.islemDurumu.Basarisiz).ToString())
            {
                formislemleri.Mesajform("Satış Kaydedilirken Hata Oluştu", "K", 200);
                yesilisikyeni();
                return;
                //SatisTemizle();
            }
            SatisOdeme.Dispose();

            if (cbOdemeSekli.SelectedIndex == 3)
                cbOdemeSekli.SelectedIndex = 0;
            #endregion

            yesilisikyeni();
        }

        void StokDurumGuncelle()
        {
            for (int s = 0; s < gridView1.DataRowCount; s++)
            {
                DataRow dr = gridView1.GetDataRow(s);
                string pkStokKartiid = dr["pkStokKartiid"].ToString();
                string pkStokKarti = dr["fkStokKarti"].ToString();

                decimal satilanadet = 0, mevcut = 0;

                decimal.TryParse(dr["Adet"].ToString(), out satilanadet);

                if (pkStokKartiid == "") pkStokKartiid = pkStokKarti;

                DataTable dtStokDurum =
                DB.GetData("select * from StokDurum with(nolock) where fkStokKarti=" + pkStokKartiid + " and fkDepolar=1");
                if (dtStokDurum.Rows.Count == 0)
                {
                    string sql = "insert into StokDurum (fkStokKarti,fkDepolar,guncelleme_tarihi,fkKullanicilar)" +
                    " values(" + pkStokKartiid + ",1,getdate()," + DB.fkKullanicilar + ")";
                    int sonuc = DB.ExecuteSQL(sql);
                }
                //string pkStokDurum = dtStokDurum.Rows[0]["pkStokDurum"].ToString();

                //iade değilse ana birim üzerinden takip et
                if (satilanadet > 0)
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokDurum SET miktar=isnull(miktar,0)-" + mevcut.ToString().Replace(",", ".") +
                             " where fkDepolar=1 and fkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);
                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
                else
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokDurum SET miktar=isnull(miktar,0)+" + mevcut.ToString().Replace("-", "").Replace(",", ".") +
                             " where fkDepolar=1 and fkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);
                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
            }
        }

        void MevcutlariGuncelle()
        {
            for (int s = 0; s < gridView1.DataRowCount; s++)
            {
                DataRow dr = gridView1.GetDataRow(s);
                string pkStokKartiid = dr["pkStokKartiid"].ToString();
                string pkStokKarti = dr["fkStokKarti"].ToString();

                decimal satilanadet = decimal.Parse(dr["Adet"].ToString());
                decimal mevcut = 0;

                if (pkStokKartiid == "") pkStokKartiid = pkStokKarti;

                //DB.ExecuteSQL("update StokKarti set Mevcut=dbo.fon_StokMevcut(" + pkStokKartiid + ") WHERE pkStokKarti =" + pkStokKartiid);

                //iade değilse ana birim üzerinden takip et
                if (satilanadet > 0)
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokKarti SET Aktarildi=" + Degerler.AracdaSatis + ",Aktif=1,Mevcut=isnull(Mevcut,0)-" + mevcut.ToString().Replace(",", ".") +
                             " where pkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);

                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
                else
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokKarti SET Aktarildi=" + Degerler.AracdaSatis + ",Aktif=1,Mevcut=isnull(Mevcut,0)+" + mevcut.ToString().Replace("-", "").Replace(",", ".") +
                             " where pkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);
                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }

            }
        }

        void DepoMevcurlariGuncelle()
        {
            for (int s = 0; s < gridView1.DataRowCount; s++)
            {
                DataRow dr = gridView1.GetDataRow(s);
                string pkStokKartiid = dr["pkStokKartiid"].ToString();
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string fkDepolar = dr["fkDepolar"].ToString();

                decimal satilanadet = decimal.Parse(dr["Adet"].ToString());
                //decimal mevcut = 0;

                if (pkStokKartiid == "") pkStokKartiid = fkStokKarti;

                ArrayList list_depo = new ArrayList();
                list_depo.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                list_depo.Add(new SqlParameter("@fkDepolar", fkDepolar));
                list_depo.Add(new SqlParameter("@Miktar", satilanadet));
                list_depo.Add(new SqlParameter("@AlisSatis", "2"));//--1 alış 2 satış

                string sonuc1 = DB.ExecuteSQL("HSP_StokKartiDepo_EkleGuncelle @fkStokKarti,@fkDepolar,@Miktar,@AlisSatis", list_depo);
                /*
                if (satilanadet > 0)
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokKarti SET Aktarildi=" + Degerler.AracdaSatis + ",Aktif=1,Mevcut=isnull(Mevcut,0)-" + mevcut.ToString().Replace(",", ".") +
                             " where pkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);

                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
                else
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokKarti SET Aktarildi=" + Degerler.AracdaSatis + ",Aktif=1,Mevcut=isnull(Mevcut,0)+" + mevcut.ToString().Replace("-", "").Replace(",", ".") +
                             " where pkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);
                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
                 */
            }
        }

        private void simpleButton37_Click(object sender, EventArgs e)
        {
            //sadece kaydet yazdırma yok
            KaydetYazdir(false,true);
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }

        void RaporOnizleme(bool Disigner)
        {
            string fisid = txtpkSatislar.Text;
            //if (lueFis.EditValue != null)
            //  fisid = lueFis.EditValue.ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Satislar s with(nolock)
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

        private string musteriara()
        {

            string fkFirma = "1", ozelkod = "0", firmadi = "", KaraListe = "";// MusteriAra.fkFirma.AccessibleDescription;
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
                case 4:
                    {
                        fkFirma = Satis4Firma.Tag.ToString();
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

            if (txtpkSatislar.Text != "0" && txtpkSatislar.Text != "")
            {
                DB.ExecuteSQL("UPDATE Satislar SET fkFirma=" + fkFirma + " where pkSatislar=" + txtpkSatislar.Text);
            }

            DataTable dt = null;
            try
            {
                dt = DB.GetData(@"select pkFirma,Firmaadi,OzelKod,isnull(fkSatisFiyatlariBaslik,1) as fkSatisFiyatlariBaslik,
                KaraListe,LimitBorc,SonOdemeTarihi,(datediff(dd,SonOdemeTarihi,GETDATE())) as OdemeYapmadigiGun,
                isnull(OdemeGunSayisi,0)  as OdemeGunSayisi,isnull(Devir,0) as Devir,

                case when isnull(OdemeGunSayisi,0)>0 then 				
                convert(datetime,
                convert(varchar(4),datepart(YY,GETDATE()))+'-'+
                convert(varchar(2),datepart(MM,GETDATE()))+'-'+
                convert(varchar(2),OdemeGunSayisi)) else '' end OdemeTarihi,

                case when isnull(OdemeGunSayisi,0)>0 then 
                DATEDIFF(dd,
                convert(datetime,
                convert(varchar(4),datepart(YY,GETDATE()))+'-'+
                convert(varchar(2),datepart(MM,GETDATE()))+'-'+
                convert(varchar(2),isnull(OdemeGunSayisi,0))),getdate()) else 0 end GecenGunSayisi
                 from Firmalar f with(nolock)
            where pkFirma=" + fkFirma);
            }
            catch (Exception exp)
            {
                MessageBox.Show("OdemeGunSayisi değerinde sorun oluştu Lütfen Müşteri OdemeGunSayisi değerini kontrol ediniz");

                dt = DB.GetData(@"select pkFirma,Firmaadi,OzelKod,isnull(fkSatisFiyatlariBaslik,1) as fkSatisFiyatlariBaslik,
                KaraListe,LimitBorc,SonOdemeTarihi,(datediff(dd,SonOdemeTarihi,GETDATE())) as OdemeYapmadigiGun,
                isnull(OdemeGunSayisi,0)  as OdemeGunSayisi,Devir, '' as OdemeTarihi, 0 as GecenGunSayisi
                from Firmalar f with(nolock)
                where pkFirma=" + fkFirma);
            }
            //finally
            //{

            //}
            if (dt.Rows[0]["fkSatisFiyatlariBaslik"].ToString() == "0")
                lueFiyatlar.ItemIndex = 0;
            else
                lueFiyatlar.EditValue = int.Parse(dt.Rows[0]["fkSatisFiyatlariBaslik"].ToString());

            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            KaraListe = dt.Rows[0]["KaraListe"].ToString();

            decimal BorcLimiti = 0;
            decimal.TryParse(dt.Rows[0]["LimitBorc"].ToString(), out BorcLimiti);

            if (KaraListe == "True")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Bu Müşteri Kara Listededir!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            int GecenGunSayisi = 0;
            int.TryParse(dt.Rows[0]["GecenGunSayisi"].ToString(), out GecenGunSayisi);

            decimal Devir = 0;
            decimal.TryParse(dt.Rows[0]["Devir"].ToString(), out Devir);

            if (BorcLimiti > 0 && Devir > BorcLimiti)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Müşterinin Borcu Limitini Geçmiştir!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (GecenGunSayisi > 0 && Devir > 0)
            {
                int OdemeYapmadigiGun = 0;
                int.TryParse(dt.Rows[0]["OdemeYapmadigiGun"].ToString(), out OdemeYapmadigiGun);
                //30 günden önce ödeme yapmadı ise uyar
                if (OdemeYapmadigiGun > 30)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    firmadi + " " + GecenGunSayisi.ToString() + " Gün Ödemesini Gecikmiştir."
                    , "Ödeme Gününü Kontrolü", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
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
                case 4:
                    {
                        Satis4Baslik.Tag = fkFirma;
                        Satis4Baslik.Text = ozelkod + "-" + firmadi;
                        Satis4Baslik.ToolTip = Satis4Baslik.Text;
                        Satis4Firma.Tag = fkFirma;
                        break;
                    }
                default:
                    break;
            }
            teBakiyeDevir.Text = Devir.ToString("##0.00"); ;

            yesilisikyeni();
            //FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
            return fkFirma;
        }

        private string musteriata(string fkFirma)
        {
            string pkFirma = "1", ozelkod = "0", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;            
            decimal Devir = 0;
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod,isnull(Devir,0) as Devir from Firmalar with(nolock) where OzelKod=" + fkFirma);
            if (dt.Rows.Count == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Bulunamadı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Satis1Baslik.Text = "";
                Satis1Baslik.Focus();
                return "1";
            }
            pkFirma = dt.Rows[0]["pkFirma"].ToString();
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();

            decimal.TryParse(dt.Rows[0]["Devir"].ToString(), out Devir);

            if (txtpkSatislar.Text != "0" && txtpkSatislar.Text != "")
            {
                DB.ExecuteSQL("UPDATE Satislar SET fkFirma=" + pkFirma + " where pkSatislar=" + txtpkSatislar.Text);
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
                case 2:
                    {
                        Satis2Firma.Tag = pkFirma;
                        Satis2Baslik.Text = ozelkod + "-" + firmadi;
                        Satis2Baslik.ToolTip = Satis2Baslik.Text;
                        break;
                    }
                case 3:
                    {
                        Satis3Firma.Tag = pkFirma;
                        Satis3Baslik.Text = ozelkod + "-" + firmadi;
                        Satis3Baslik.ToolTip = Satis3Baslik.Text;
                        break;
                    }
                default:
                    break;
            }

            teBakiyeDevir.Text = Devir.ToString("##0.00");

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
            if (Degerler.StokKartiDizayn)
            {
                frmStokKartiLayout StokKarti = new frmStokKartiLayout();
                DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());

                StokKarti.ShowDialog();
            }
            else
            {
                frmStokKarti StokKarti = new frmStokKarti();
                //string pkStokKartiid = dr["pkStokKartiid"].ToString();
                //if (pkStokKartiid == "" || pkStokKartiid == "0")
                DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
                //else
                //  DB.pkStokKarti = int.Parse(pkStokKartiid);
                StokKarti.ShowDialog();
            }
            FisSatisAlisFiyatlarininiDegis(DB.pkStokKarti.ToString());

            //            DB.ExecuteSQL(@"UPDATE SatisDetay SET  NakitFiyat=sk.SatisFiyati,SatisFiyati=sk.SatisFiyati From StokKarti sk 
            //            where SatisDetay.fkStokKarti=sk.pkStokKarti and  pkSatisDetay=" + dr["pkSatisDetay"].ToString());

            //Fiyatlarigetir();
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
                //gridView1.SetFocusedRowCellValue("iade", "true");
                iadebasildi();
                //gridView1.SetFocusedRowCellValue("Barcode", "");
                //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                yesilisikyeni();
            }
            //nakit
            lueKKarti.Visible = false;
            //if ((e.KeyValue == 78 || e.KeyValue == 75 || e.KeyValue == 65 || e.KeyValue == 68) && cbOdemeSekli.Enabled == false)
            //{
            //    ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            //    return;
            //}
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
                //lueKKarti.Properties.DataSource = DB.GetData("select * from Bankalar with(nolock) where Aktif=1");
                lueKKarti.Visible = true;
                //lueKKarti.ItemIndex = 0;
            }
            //veresiye açık hesap
            else if (e.KeyValue == 65)
            {
                cbOdemeSekli.TabStop = true;
                cbOdemeSekli.SelectedIndex = 2;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //DİĞER
            else if (e.KeyValue == 68)
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

            //AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            //AppearanceDefault appError = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorRed = new AppearanceDefault(Color.OrangeRed);

            //Font fontkucuk = new Font("Times New Roman", 9.0f, FontStyle.Regular);
            //Font fontnormal = new Font("Times New Roman", 11.0f,FontStyle.Underline);
            //Font fontbuyuk = new Font("Times New Roman", 12.0f,FontStyle.Bold);

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            else if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["AlisFiyati"].ToString() != "")
            {
                e.Appearance.BackColor = Color.SeaShell;
                e.Appearance.ForeColor = Color.Black;
                decimal iskontotutar = 0;
                decimal.TryParse(dr["iskontotutar"].ToString(), out iskontotutar);

                decimal SatisFiyati = 0;
                decimal.TryParse(dr["SatisFiyati"].ToString(), out SatisFiyati);
                SatisFiyati = SatisFiyati - iskontotutar;

                decimal AlisTutar = 0;
                decimal.TryParse(dr["AlisFiyati"].ToString(), out AlisTutar);

                if (SatisFiyati - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.BackColor2 = Color.White;
                }
                else
                    e.Appearance.BackColor = Color.SeaShell;
                // AppearanceHelper.Apply(e.Appearance, appError);
                if (iskontotutar == 0)
                    e.Appearance.Font = new Font("Times New Roman", 11.0f, FontStyle.Bold);

                
                decimal isk = 0;
                decimal.TryParse(dr["iskontotutar"].ToString(), out isk);
                if (isk == 0)
                    e.Appearance.Font = new Font("Times New Roman", 11.0f, FontStyle.Bold);
            }
            else if (e.Column.FieldName == "Mevcut" && dr["KritikMiktar"].ToString() != "" && dr["Mevcut"].ToString() != "")
            {
                decimal KritikMiktar = Convert.ToDecimal(dr["KritikMiktar"].ToString());
                decimal Mevcut = Convert.ToDecimal(dr["Mevcut"].ToString());
                if (KritikMiktar > Mevcut)
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.BackColor2 = Color.YellowGreen;
                    //AppearanceHelper.Apply(e.Appearance, appErrorRed);
                }
            }
            else if (e.Column.FieldName == "MevcutAdet" && dr["KritikMiktar"].ToString() != "" && dr["Mevcut"].ToString() != "")
            {
                decimal KritikMiktar = Convert.ToDecimal(dr["KritikMiktar"].ToString());
                decimal MevcutAdet = Convert.ToDecimal(dr["MevcutAdet"].ToString());
                if (KritikMiktar > MevcutAdet)
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.BackColor2 = Color.YellowGreen;
                    //AppearanceHelper.Apply(e.Appearance, appErrorRed);
                }
            }
            else if (e.Column.FieldName == "iskontotutar")
            {
                decimal isk = 0;
                decimal.TryParse(dr["iskontotutar"].ToString(), out isk);
                if (isk != 0)
                {
                    e.Appearance.BackColor = Color.DeepSkyBlue;
                    e.Appearance.BackColor2 = Color.LightCyan;
                    //AppearanceHelper.Apply(e.Appearance, appfont);
                }
               
            }
            else if ((e.Column.FieldName == "SatisFiyatiiskontolu") && dr["SatisFiyati"].ToString() != "")
            {
                e.Appearance.BackColor = Color.SeaShell;
                decimal isk = 0;
                decimal.TryParse(dr["iskontotutar"].ToString(), out isk);
                if (isk != 0)
                    e.Appearance.Font = new Font("Times New Roman", 11.0f, FontStyle.Bold);

            }          
            else if (e.Column.FieldName == "Tutar")
            {
                e.Appearance.BackColor = Color.SeaShell;
            }
            else if (e.Column.FieldName == "TutarKdvHaric")
            {
                e.Appearance.BackColor = Color.Wheat;
            }
            else if (e.Column.FieldName == "SatisFiyatiKdvHaric")
            {
                e.Appearance.BackColor = Color.Wheat;
                //e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.Black;
            }
            else if (e.Column.FieldName == "satisfiyati_kdvharic_iskontolu")
            {
                e.Appearance.BackColor = Color.Wheat;
                //e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.Black;
            }
            //if (e.Column.FieldName == "SatisFiyatiKdvHaric")
            //{
            //    e.Appearance.BackColor = Color.Wheat;
            //    //e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
            //    e.Appearance.ForeColor = Color.Black;
            //}
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
            decimal aratop = 0, istutar = 0,tutar=0;
            if (gridColumn5.SummaryItem.SummaryValue == null)
                aratop = 0;//atis1Toplam.Text = "0,0";
            else
                aratop = decimal.Parse(gridColumn5.SummaryItem.SummaryValue.ToString());

            //tutar
            if (gridColumn12.SummaryItem.SummaryValue == null)
                tutar = 0;//atis1Toplam.Text = "0,0";
            else
                tutar = decimal.Parse(gridColumn12.SummaryItem.SummaryValue.ToString());
            //iskonto tutar
            if (gridColumn14.SummaryItem.SummaryValue == null)
                aratop = 0;//atis1Toplam.Text = "0,0";
            else
                ceToplamiskonto.Value = decimal.Parse(gridColumn14.SummaryItem.SummaryValue.ToString());


            //KdvToplamTutari tutar
            if (gcKdvToplamTutari.SummaryItem.SummaryValue == null)
                aratop = 0;//atis1Toplam.Text = "0,0";
            else
                ceKdvToplamTutari.Value = decimal.Parse(gcKdvToplamTutari.SummaryItem.SummaryValue.ToString());

            //önce hesapla sonra bilgi göster NullTexde
            //if (ceiskontoyuzde.EditValue != null)
            //{
            //    isfiyat = decimal.Parse(ceiskontoyuzde.EditValue.ToString());
            //    istutar = isfiyat * aratop / 100;
            //}
            //if (ceiskontoTutar.EditValue != null) // && iskontoTutar.EditValue.ToString() !="0")
            //{
            //    isfiyat = decimal.Parse(ceiskontoTutar.EditValue.ToString());
            //    if (aratop > 0)
            //        isyuzde = (isfiyat * 100) / aratop;
            //    istutar = isfiyat;
            //}
            ceToplam.Value = tutar;
            aratoplam.Value = aratop;
            if (AcikSatisindex == 1)
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

            //if (gridColumn28.SummaryItem.SummaryValue == null)
            //    cefark.EditValue = "0,0";
            //else
            //    cefark.EditValue = gridColumn28.SummaryItem.SummaryValue.ToString();

            //cefark.Value = aratoplam.Value - iskontoTutar.Value;
        }

        private void simpleButton44_Click(object sender, EventArgs e)
        {
            paraustu.Text = "";
            alinanpara.Value = 0;
        }

        private void satışİptalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);
        }

        int getCheckedCount()
        {
            int count = 0;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                if ((bool)gridView1.GetRowCellValue(i, gridView1.Columns["iade"]) == true)
                    count++;
            }
            return count;
        }

        void CheckAll()
        {
            DB.ExecuteSQL("update SatisDetay set Adet=abs(Adet)*-1,iade=1 where fkSatislar=" + txtpkSatislar.Text);
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{
            //    gridView1.SetRowCellValue(i, gridView1.Columns["iade"], true);
            //}
        }
        void UnChekAll()
        {
            DB.ExecuteSQL("update SatisDetay set Adet=abs(Adet),iade=0 where fkSatislar=" + txtpkSatislar.Text);
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{
            //    gridView1.SetRowCellValue(i, gridView1.Columns["iade"], false);
            //}
        }

        private void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {

            //if (e.Column == (sender as GridView).Columns["iade"])
            //{
            //    e.Info.InnerElements.Clear();
            //    e.Info.Appearance.ForeColor = Color.Blue;
            //    e.Painter.DrawObject(e.Info);
            //    DrawCheckBox(e.Graphics, e.Bounds, getCheckedCount() == gridView1.DataRowCount);
            //    e.Handled = true;
            //}


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
                    string ex = exp.Message;
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
            Hizlibuttonlariyukle(true);
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
                //if (gCSatisDuzen.Visible == true)
                //{
                //    formislemleri.Mesajform("Önce Fiş Düzenlemeyi Kapatınız", "K");
                //    return;
                //}
                // if (FisNoBilgisi.SatisDurumu.Tag.ToString() == ((int)Degerler.SatisDurumlari.Teklif).ToString())

                deGuncellemeTarihi.DateTime = DateTime.Now;

                AcikSatisindex = 4;
                Satis4Firma.Visible = true;
                Satis4Firma.Tag = FisNoBilgisi.groupControl1.Tag;
                DB.ExecuteSQL("update Satislar Set Siparis=0 where pkSatislar=" + pkSatis);
                Satis4Toplam.Tag = pkSatis;

                //fisduzenaciksatis(false);

                SatisGetir();

                //BakiyeGetirSecindex(gCSatisDuzen.Tag.ToString(),0);
                //Yenile();
            }
            FisNoBilgisi.Dispose();
            //FisListesi();
            yesilisikyeni();
        }

        void SatisGetir()
        {
            string pkSatislar = "0";
            cbOdemeSekli.TabStop = false;
            cbOdemeSekli.SelectedIndex = -1;
            lueFiyatlar.ItemIndex = 0;
            cbTevkifat.SelectedIndex = 0;
            teBakiyeDevir.Text = "0,00";

            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2)
                pkSatislar = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3)
                pkSatislar = Satis3Toplam.Tag.ToString();
            else if (AcikSatisindex == 4)
                pkSatislar = Satis4Toplam.Tag.ToString();

            DataTable dtSatislar = DB.GetData("hsp_SatisBilgisi " + pkSatislar);
            if (pkSatislar == "0")
            {
                //lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
                //deGuncellemeTarihi.Enabled = false;
                //deGuncellemeTarihi.EditValue = null;
                //txtFaturaNo.Text = "";
                temizle(AcikSatisindex);
                return;
            }

            if (dtSatislar.Rows.Count == 0)
            {
                Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            string fkfirma = dtSatislar.Rows[0]["fkFirma"].ToString();
            string OzelKod = dtSatislar.Rows[0]["OzelKod"].ToString();
            string firmaadi = dtSatislar.Rows[0]["Firmaadi"].ToString();
            string OdemeSekli =  dtSatislar.Rows[0]["OdemeSekli"].ToString();

            cbOdemeSekli.EditValue = OdemeSekli;

            if (dtSatislar.Rows[0]["fkSatisDurumu"].ToString() != "")
                lueSatisTipi.EditValue = int.Parse(dtSatislar.Rows[0]["fkSatisDurumu"].ToString());

            if (dtSatislar.Rows[0]["fkSatisFiyatlariBaslik"].ToString() != "")
                lueFiyatlar.EditValue = int.Parse(dtSatislar.Rows[0]["fkSatisFiyatlariBaslik"].ToString());

            if (dtSatislar.Rows[0]["fkKullanici"].ToString() != "")
                lueKullanicilar.EditValue = int.Parse(dtSatislar.Rows[0]["fkKullanici"].ToString());

            if (dtSatislar.Rows[0]["GuncellemeTarihi"].ToString() == "")
                deGuncellemeTarihi.EditValue = null;
            else
                deGuncellemeTarihi.DateTime = Convert.ToDateTime(dtSatislar.Rows[0]["GuncellemeTarihi"].ToString());

            if (dtSatislar.Rows[0]["TeklifTarihi"].ToString() == "")
                deTeklifTarihi.EditValue = null;
            else
                deTeklifTarihi.DateTime = Convert.ToDateTime(dtSatislar.Rows[0]["TeklifTarihi"].ToString());


            decimal devir = decimal.Parse(dtSatislar.Rows[0]["Devir"].ToString());

            teBakiyeDevir.Text = devir.ToString("##0.00");

            string FaturaNo = dtSatislar.Rows[0]["FaturaNo"].ToString();

            //if (FaturaNo != "")
                txtFaturaNo.Text = FaturaNo;

            if (dtSatislar.Rows[0]["FaturaTarihi"].ToString() == "")
                deFaturaTarihi.EditValue = null;
            else
                deFaturaTarihi.DateTime = Convert.ToDateTime(dtSatislar.Rows[0]["FaturaTarihi"].ToString());

            if (OdemeSekli=="Kredi Kartı")
            {
                lueKKarti.Visible = true;
                if(dtSatislar.Rows[0]["fkBankalar"].ToString() != "")
                  lueKKarti.EditValue = int.Parse(dtSatislar.Rows[0]["fkBankalar"].ToString());
                else
                lueKKarti.ItemIndex= 0;
            }
            btnAciklamaGirisi.ToolTip = dtSatislar.Rows[0]["Aciklama"].ToString();
            txtEFaturaUUID.Text = dtSatislar.Rows[0]["uuid"].ToString();

            if (AcikSatisindex == 1)
            {
                Satis1Firma.Tag = fkfirma;
                Satis1Firma.Text = OzelKod + "-" + firmaadi;
                Satis1Baslik.Text = Satis1Firma.Text;
                Satis1Baslik.ToolTip = Satis1Firma.Text;
            }
            if (AcikSatisindex == 2)
            {
                Satis2Firma.Tag = fkfirma;
                Satis2Firma.Text = OzelKod + "-" + firmaadi;
                Satis2Baslik.Text = Satis2Firma.Text;
                Satis2Baslik.ToolTip = Satis2Firma.Text;
            }
            if (AcikSatisindex == 3)
            {
                Satis3Firma.Tag = fkfirma;
                Satis3Firma.Text = OzelKod + "-" + firmaadi;
                Satis3Baslik.Text = Satis3Firma.Text;
                Satis3Baslik.ToolTip = Satis3Firma.Text;
            }
            if (AcikSatisindex == 4)
            {
                Satis4Firma.Tag = fkfirma;
                Satis4Firma.Text = OzelKod + "-" + firmaadi;
                Satis4Baslik.Text = Satis4Firma.Text;
                Satis4Baslik.ToolTip = Satis4Firma.Text;
            }
            deGuncellemeTarihi.Enabled = true;
        }

        void TutarFont(DevExpress.XtraEditors.LabelControl secilen)
        {
            Satis1Baslik.Font = buttonkucukfont.Font;
            Satis2Baslik.Font = buttonkucukfont.Font;
            Satis3Baslik.Font = buttonkucukfont.Font;

            if (AcikSatisindex == 1)
            {
                Satis1Baslik.Font = Satis4Baslik.Font;
                Satis1Firma.Width = 250;
                Satis2Firma.Width = 100;
                Satis3Firma.Width = 100;
                Satis4Firma.Width = 100;
            }
            else if (AcikSatisindex == 2)
            {
                Satis2Baslik.Font = Satis4Baslik.Font;
                Satis1Firma.Width = 100;
                Satis2Firma.Width = 250;
                Satis3Firma.Width = 100;
                Satis4Firma.Width = 100;
            }
            else if (AcikSatisindex == 3)
            {
                Satis3Baslik.Font = Satis4Baslik.Font;
                Satis1Firma.Width = 100;
                Satis2Firma.Width = 100;
                Satis3Firma.Width = 250;
                Satis4Firma.Width = 100;
            }
            else if (AcikSatisindex == 4)
            {
                Satis4Baslik.Font = Satis4Baslik.Font;
                Satis1Firma.Width = 100;
                Satis2Firma.Width = 100;
                Satis3Firma.Width = 100;
                Satis4Firma.Width = 250;
            }
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
            if (lueSatisTipi.EditValue.ToString() == ((int)Degerler.SatisDurumlari.Teklif).ToString())
            {
                frmTeklifRaporlari Teklifler = new frmTeklifRaporlari();
                Teklifler.fişDüzenleToolStripMenuItem.Enabled = true;
                Teklifler.ShowDialog();

                string fisno = Teklifler.fişDüzenleToolStripMenuItem.Tag.ToString();
                #region fiş düzenle
                if (fisno != "0")
                {
                    //if (gCSatisDuzen.Visible == true)
                    //{
                    //    formislemleri.Mesajform("Önce Fiş Düzenlemeyi Kapatınız","K");
                    //    return;
                    //}
                    //pkSatisBarkod.Text = fisno;

                    //DataTable dt = DB.GetData("select pkSatislar,Siparis,fkKullanici From Satislar with(nolock) where pkSatislar=" + pkSatisBarkod.Text);
                    //if (dt.Rows.Count == 0)
                    //{
                    //    Showmessage("Fiş Bulunamadı.", "K");
                    //    return;
                    //}
                    //if (dt.Rows[0]["Siparis"].ToString() == "False")
                    //{
                    //    Showmessage("Fiş Açık Lütfen Yenileyiniz. Kullanıcı=" + dt.Rows[0]["fkKullanici"].ToString(), "K");
                    //    return;
                    //}
                    FisGetir(fisno);
                }
                #endregion
            }
            else
            {
                frmSatisRaporlari Satislar = new frmSatisRaporlari();
                Satislar.fişDüzenleToolStripMenuItem.Enabled = true;
                Satislar.ShowDialog();

                string fisno = Satislar.fişDüzenleToolStripMenuItem.Tag.ToString();
                #region fiş düzenle
                if (fisno != "0")
                {
                    //if (gCSatisDuzen.Visible == true)
                    //{
                    //    formislemleri.Mesajform("Önce Fiş Düzenlemeyi Kapatınız","K");
                    //    return;
                    //}
                    //pkSatisBarkod.Text = fisno;

                    //DataTable dt = DB.GetData("select pkSatislar,Siparis,fkKullanici From Satislar with(nolock) where pkSatislar=" + pkSatisBarkod.Text);
                    //if (dt.Rows.Count == 0)
                    //{
                    //    Showmessage("Fiş Bulunamadı.", "K");
                    //    return;
                    //}
                    //if (dt.Rows[0]["Siparis"].ToString() == "False")
                    //{
                    //    Showmessage("Fiş Açık Lütfen Yenileyiniz. Kullanıcı=" + dt.Rows[0]["fkKullanici"].ToString(), "K");
                    //    return;
                    //}
                    FisGetir(fisno);
                }
                #endregion
            }
            yesilisikyeni();
        }

        private void Hesapla(object sender, EventArgs e)
        {
            paraustu.BackColor = System.Drawing.Color.DimGray;
            if (alinanpara.Value > 0)
            {
                decimal pu = alinanpara.Value - aratoplam.Value;
                paraustu.Text = pu.ToString("##0.00");
                if (pu < 0) paraustu.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                alinanpara.Value = 0;
                paraustu.Text = "";
            }
        }

        private void cariSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            musteriara();
        }

        private void alinanpara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                alinanpara.Value = 0;
                paraustu.Text = "";
                paraustu.BackColor = System.Drawing.Color.DimGray;
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

            Hizlibuttonlariyukle(true);
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            string pkSatis = "0";

            if (AcikSatisindex == 1) pkSatis = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2) pkSatis = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3) pkSatis = Satis3Toplam.Tag.ToString();
            else if (AcikSatisindex == 4) pkSatis = Satis4Toplam.Tag.ToString();

            if (pkSatis == "0")
            {
                Showmessage("Önce Satış Yapınız!", "K");
                return;
            }
            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.Tag = "Satis";
            fFisAciklama.memoozelnot.Tag = pkSatis;
            //fFisAciklama.memoozelnot.Text = btnAciklamaGirisi.ToolTip;
            fFisAciklama.ShowDialog();

            btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;

            fFisAciklama.Dispose();

            yesilisikyeni();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            //string str = ActiveControl.Name;
            //this.Hide();
            //this.Dispose();
            this.Visible = false;
        }

        private void simpleButton20_Click_1(object sender, EventArgs e)
        {
            this.Dock = DockStyle.None;
            this.Width = 0;
            //this.Hide();
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

        private void paraustunegitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alinanpara.Focus();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE HizliStokSatis Set fkStokKarti=0 where pkHizliStokSatis=" + pkHizliStokSatis);

            Hizlibuttonlariyukle(true);
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
            Satis1ToplamGetir(3);
        }

        void fisduzenaciksatis(bool sadeceindex)
        {
            Satis1ToplamGetir(4);

            if (sadeceindex == true)
                yesilisikyeni();
        }

        private void Satis4Toplam_Click(object sender, EventArgs e)
        {
            fisduzenaciksatis(true);
        }

        void SatisDuzenKapat()
        {
            Satis4Firma.Visible = false;
            //Satis1ToplamGetir();
            //pkSatisBarkod.EditValue = null;
            //TutarFont(Satis4Toplam);
            //AcikSatisindex = 1;
            //yesilisikyeni();
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
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr["iade"].ToString() == "True")
            {
                decimal miktar = decimal.Parse(yenimiktar);
                if (miktar > 0)
                    miktar = miktar * -1;
                yenimiktar = miktar.ToString();
            }

            string pkSatisDetay = dr["pkSatisDetay"].ToString();
            DB.ExecuteSQL("UPDATE SatisDetay SET Adet=" + yenimiktar.Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
            decimal iskontoyuzde = 0;
            if (dr["iskontoyuzdetutar"].ToString() != "")
                iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
            decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());

            decimal Fiyat = 0;
            if (dr["SatisFiyati"].ToString() != "")
                Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());

            decimal Miktar = Convert.ToDecimal(yenimiktar);
            decimal iskontogercektutar = Convert.ToDecimal(dr["iskontotutar"].ToString());

            if (iskontogercektutar > 0)
            {
                iskontogercekyuzde = (iskontogercektutar * 100) / (Fiyat * Miktar);
            }
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn3, yenimiktar);
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn33, iskontogercekyuzde.ToString());

        }

        private void aratoplam_EditValueChanged(object sender, EventArgs e)
        {
            //alinanpara.Value = 0;
            alinanpara_EditValueChanged(sender, e);
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
                Hizlibuttonlariyukle(false);

            dockPanel1goster();
            lcKullanici.Tag = "0";
        }

        private void repositoryItemButtonEdit1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
            // string girilen=
            // ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
        }

        private void pkSatisBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //fiş düzenle
            if (e.KeyCode == Keys.Enter)
            {

                if (txtpkSatislar.Text == "") return;

                DataTable dt = DB.GetData("select pkSatislar,Siparis,fkKullanici From Satislar with(nolock) where pkSatislar=" + txtpkSatislar.Text);
                if (dt.Rows.Count == 0)
                {
                    Showmessage("Fiş Bulunamadı.", "K");
                    return;
                }
                if (dt.Rows[0]["Siparis"].ToString() == "False")
                {
                    Showmessage("Fiş Açık Lütfen Yenileyiniz. Kullanıcı=" + dt.Rows[0]["fkKullanici"].ToString(), "K");
                    return;
                }
                FisGetir(txtpkSatislar.Text);
            }
        }

        private void gCSatisDuzen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmMusteriKarti kart = new frmMusteriKarti(Satis4Firma.Tag.ToString(), "");
            DB.PkFirma = int.Parse(Satis4Firma.Tag.ToString());
            kart.ShowDialog();
        }

        private void alinanpara_EditValueChanged(object sender, EventArgs e)
        {
            if (alinanpara.Value > 0)
            {
                decimal pu = alinanpara.Value - aratoplam.Value;
                paraustu.Text = pu.ToString();
                if (pu < 0)
                    paraustu.BackColor = System.Drawing.Color.Red;
                else
                    paraustu.BackColor = System.Drawing.Color.DimGray;
            }
        }

        private void gridControl1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
        }

        private void lueSatisTipi_EditValueChanged(object sender, EventArgs e)
        {
            btnEFatura.Visible = button5.Visible = button6.Visible = false;
            xtraTabControl1.SelectedTabPage = xtabGenel;
            btnyazdir.Text = "Yazdır [F11]";

            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura
                || int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.SFatura
                || int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.İrsaliye)
            {
                //lueSatisTipi.Tag = "4";
                xtraTabControl1.SelectedTabPage = xtabFatura;
                //lbTevkifat.Visible = false;
                cbTevkifat.Visible = false;
            }
            //else if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Teklif)
            else if (lueSatisTipi.Text == "Teklif")
            {
                //lueSatisTipi.Tag = "1";
                xtraTabControl1.SelectedTabPage = xtabTeklif;
                //deFaturaTarihi.EditValue = null;
                //txtFaturaNo.Text = "";
            }
            //else if (lueSatisTipi.Text == "İade")
            //{
            //    DB.ExecuteSQL("UPDATE SatisDetay SET Adet=abs(Adet)*-1,iade=1 where fkSatislar=" + txtpkSatislar.Text);
            //}
            else
            {
                xtraTabControl1.SelectedTabPage = xtabGenel;
                //deFaturaTarihi.EditValue = null;
                //txtFaturaNo.Text = "";
            }

            //if (txtpkSatislar.Text != "" && lueSatisTipi.Tag.ToString() == "1")
            //{
            //    if (lueSatisTipi.Text == "İade")
            //        DB.ExecuteSQL("UPDATE SatisDetay SET Adet=abs(Adet)*-1,iade=1 where fkSatislar=" + txtpkSatislar.Text);
            //    else if (lueSatisTipi.Text == "Değişim")
            //        DB.GetData("select getdate()"); //bu kısmı kaldırma iade bozuluyor
            //    else
            //        DB.ExecuteSQL("UPDATE SatisDetay SET Adet=abs(Adet),iade=0 where fkSatislar=" + txtpkSatislar.Text);
            //}
            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Teklif)
            {
                btnyazdir.Text = "ÖN İZLE [F11]";
                btnGecmisFis.Text = "SON TEKLİFLER";
                //DB.ExecuteSQL("UPDATE SatisDetay SET fkSatisDurumu=1 where fkSatislar=" + pkSatisBarkod.Text);
                //lbFaturaTeklifTarihi.Text = "TEKLİF TARİHİ";
                //deTeklifTarihi
                lueSatisTipi.Tag = "1";
                gbBaslik.Text = "Teklif";
            }
            //else if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura)
            else if (lueSatisTipi.Text == "Fatura")
            {
                //lueSatisTipi.Tag = "4";
                btnyazdir.Text = "ÖN İZLE [F11]";

                xtraTabControl1.SelectedTabPage = xtabFatura;
                xtabFatura.Text = "Fatura";
                lbFaturaTevkifatTarihi.Text = "Fatura Tarihi";
                lblEFaturaUUID.Visible = txtEFaturaUUID.Visible = false;

                //if (deFaturaTarihi.EditValue == null && !ilkyukleme)
                //  deFaturaTarihi.DateTime = DateTime.Today;
                gbBaslik.Text = "Fatura";
                SonFaturaNo();
            }
            else if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.İrsaliye)
            {
                xtraTabControl1.SelectedTabPage = xtabFatura;
                //lueSatisTipi.Tag = "3";
                btnyazdir.Text = "ÖN İZLE [F11]";
                lbFaturaTevkifatTarihi.Text = "İRSALİYE TARİHİ";
                //if (deFaturaTarihi.EditValue == null && !ilkyukleme)
                //  deFaturaTarihi.DateTime = DateTime.Today;
                gbBaslik.Text = "İrsaliye";
            }
            //else if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.SFatura)
            else if (lueSatisTipi.Text == "SFatura")
            {
                //lueSatisTipi.Tag = "11";
                btnyazdir.Text = "ÖN İZLE [F11]";
                xtraTabControl1.SelectedTabPage = xtabFatura;
                xtabFatura.Text = "S-Fatura";
                lbFaturaTevkifatTarihi.Text = "FATURA TARİHİ";
                //if (deFaturaTarihi.EditValue == null && !ilkyukleme)
                //  deFaturaTarihi.DateTime = DateTime.Today;
                gbBaslik.Text = "Sanal Fatura";
                SonFaturaNo();
            }
            //else if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.EFatura)
            else if (lueSatisTipi.Text == "E-Fatura")
            {
                xtraTabControl1.SelectedTabPage = xtabFatura;
                xtabFatura.Text = "E-Fatura";
                lbFaturaTevkifatTarihi.Text = "E-Fatura Tar.";
                btnEFatura.Visible = button5.Visible = button6.Visible = lblEFaturaUUID.Visible = txtEFaturaUUID.Visible = true;
            }
            else
            {
                //lueSatisTipi.Tag = "2";
                //if (deFaturaTarihi.EditValue == null && !ilkyukleme)
                //deFaturaTarihi.DateTime = DateTime.Today;
                btnGecmisFis.Text = "Son Satış Fişler";
                gbBaslik.Text = "Satış Faturası";
            }

            #region Tevkifat
            if (lueSatisTipi.Text == "Tevkifat")//(int)Degerler.SatisDurumlari.Tevkifat)
            {
                //lueSatisTipi.Tag = "12";

                btnyazdir.Text = "ÖN İZLE [F11]";

                xtraTabControl1.SelectedTabPage = xtabFatura; //xtabTevkifat;
                lbFaturaTevkifatTarihi.Text = "TEVKİFAT TARİHİ";

                if (deFaturaTarihi.EditValue == null && !ilkyukleme)
                    deFaturaTarihi.DateTime = DateTime.Today;

                //lbTevkifat.Visible = true;
                cbTevkifat.Visible = true;
                gbBaslik.Text = "Tevkifat";
            }
            #endregion

            //if (lueSatisTipi.Tag.ToString() == "1" && pkSatisBarkod.Text != "")
            //{
            //    string sql = "update Satislar set fkSatisDurumu=" + lueSatisTipi.EditValue.ToString();

            //    if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura)
            //        sql = sql + ",FaturaTarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd") + "'";
            //        sql = sql + " where pkSatislar=" + pkSatisBarkod.Text;
            //    DB.ExecuteSQL(sql);
            //}

            if (lueSatisTipi.Tag.ToString() == "1" && lueSatisTipi.EditValue != null && txtpkSatislar.Text != "")
            {
                DB.ExecuteSQL("UPDATE Satislar SET fkSatisDurumu=" + lueSatisTipi.EditValue.ToString() + " where pkSatislar=" + txtpkSatislar.Text);
            }


            if (!ilkyukleme)
            {
                yesilisikyeni();
            }
        }

        private void SonFaturaNo()
        {
            DataTable dtKul = DB.GetData("select * from Kullanicilar with(nolock) where pkKullanicilar=" +
                   DB.fkKullanicilar);
            int faturano = 0;
            int.TryParse(dtKul.Rows[0]["FaturaNo"].ToString(), out faturano);
            teSeriNo.Text = dtKul.Rows[0]["FaturaSeriNo"].ToString();

            txtFaturaNo.Text = (faturano + 1).ToString();
        }

        void SatisDetayEkle(string barkod)
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
            if (dr != null && dr["pkSatisDetay"].ToString() != "") return;
            if (barkod == "" || f == 0) return;

            YeniSatisEkle();

            //başında sıfır varsa
            if (barkod.Length == 3)
                barkod = (1 * decimal.Parse(barkod)).ToString();

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

            if (lueSatisTipi.Text == "İade") EklenenMiktar = EklenenMiktar * -1;

            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti,isnull(SatisAdedi,1) as SatisAdedi FROM StokKarti with(nolock) where Barcode='" + barkod + "'");

            decimal SatisAdedi = 1;
            string pkStokKarti = "0";
            if (dtStokKarti.Rows.Count == 0)
            {
                //çoklu barkodlara bak
                string Barcode = "";
                dtStokKarti = DB.GetData("SELECT sk.pkStokKarti,isnull(skb.SatisAdedi,1) as SatisAdedi,sk.Barcode FROM StokKartiBarkodlar  skb with(nolock)" +
                " inner join StokKarti sk with(nolock) on sk.pkStokKarti=skb.fkStokKarti  where skb.Barkod='" + barkod + "'");
                if (dtStokKarti.Rows.Count > 0)
                {
                    SatisAdedi = decimal.Parse(dtStokKarti.Rows[0]["SatisAdedi"].ToString());
                    //EklenenMiktar = EklenenMiktar * SatisAdedi;
                    pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
                    Barcode = dtStokKarti.Rows[0]["Barcode"].ToString();
                }
                else
                {
                    frmBarkodYok BarkodYok = new frmBarkodYok(barkod);
                    BarkodYok.ShowDialog();

                    if (BarkodYok.Tag.ToString() == "1")
                    {
                        frmStokKarti StokKarti = new frmStokKarti();
                        DB.pkStokKarti = 0;
                        StokKarti.Barkod.Text = barkod;
                        StokKarti.lblBarkod.Tag = barkod;
                        StokKarti.ShowDialog();

                        dtStokKarti = DB.GetData("select pkStokKarti,isnull(SatisAdedi,1) as SatisAdedi From StokKarti with(nolock) WHERE Barcode='" + barkod + "'");
                        if (dtStokKarti.Rows.Count == 0)
                        {
                            yesilisikyeni();
                            return;
                        }
                        else
                        {
                            SatisAdedi = decimal.Parse(dtStokKarti.Rows[0]["SatisAdedi"].ToString());
                            pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
                        }
                    }
                    else
                    {
                        yesilisikyeni();
                        return;

                    }
                }
            }
            else
            {
                SatisAdedi = decimal.Parse(dtStokKarti.Rows[0]["SatisAdedi"].ToString());
                pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            }

            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkSatislar", txtpkSatislar.Text));
            arr.Add(new SqlParameter("@SatisFiyatGrubu", lueFiyatlar.EditValue.ToString()));

            decimal eklenenmik = EklenenMiktar * SatisAdedi;
            arr.Add(new SqlParameter("@Adet", eklenenmik.ToString().Replace(",", ".")));

            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
            arr.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.SelectedIndex));
            //27.06.2016
            arr.Add(new SqlParameter("@fkDepolar", lueDepolar.EditValue.ToString()));

            bool dahaonceeklendimi = false;

            if (cbOncekiFiyatHatirla.Checked && lueSatisTipi.EditValue.ToString() != "11")
            {
                if (DB.GetData("select pkSatisDetay from SatisDetay with(nolock) where fkSatislar=" +
                    txtpkSatislar.Text + " and fkStokKarti=" + pkStokKarti).Rows.Count > 0)
                    dahaonceeklendimi = true;
            }

            string s = DB.ExecuteScalarSQL("exec sp_SatisDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti,@OdemeSekli,@fkDepolar", arr);

            if (s != "Satis Detay Eklendi.")
            {
                MessageBox.Show(s);
                return;
            }

            #region OncekiFiyat kullan
            //ürün eklemeden önce yukarda kontrol ediliyor daha once eklendi mi
            if (cbOncekiFiyatHatirla.Checked && lueSatisTipi.EditValue.ToString() != "11" && dahaonceeklendimi == false)
            {
                string pkFirma = "1";
                if (AcikSatisindex == 1)
                    pkFirma = Satis1Firma.Tag.ToString();
                if (AcikSatisindex == 2)
                    pkFirma = Satis2Firma.Tag.ToString();
                if (AcikSatisindex == 3)
                    pkFirma = Satis3Firma.Tag.ToString();

                if (pkFirma != "1")
                {
                    //11-SFatura
                    string sql = @"select  top 1 sd.SatisFiyati-isnull(iskontotutar,0) as EskiSatisFiyati,sk.SatisFiyati from Satislar s with(nolock)
                    inner join SatisDetay sd on s.pkSatislar=sd.fkSatislar
                    inner join StokKarti sk with(nolock) on sk.pkStokKarti = sd.fkStokKarti
                    where s.fkSatisDurumu not in(11) and sk.satis_iskonto=0 and s.Siparis=1 and s.fkFirma=@fkFirma and  sd.fkStokKarti=@fkStokKarti
                    order by s.pkSatislar desc";

                    sql = sql.Replace("@fkFirma", pkFirma);
                    sql = sql.Replace("@fkStokKarti", pkStokKarti);

                    DataTable dtSonFiyat = DB.GetData(sql);

                    if (dtSonFiyat.Rows.Count > 0)
                    {
                        string esf = dtSonFiyat.Rows[0]["EskiSatisFiyati"].ToString();
                        string sksatisfiyati = dtSonFiyat.Rows[0]["SatisFiyati"].ToString();

                        if (esf == "") esf = "0";

                        decimal EskiSatisFiyati = decimal.Parse(esf);
                        decimal SatisFiyati = decimal.Parse(sksatisfiyati);
                        string sd = "0";

                        if (SatisFiyati == 0)
                        {
                            ArrayList list0 = new ArrayList();
                            list0.Add(new SqlParameter("@EskiSatisFiyati", EskiSatisFiyati.ToString().Replace(",", ".")));
                            list0.Add(new SqlParameter("@EskiSatisFiyati2", EskiSatisFiyati.ToString().Replace(",", ".")));
                            list0.Add(new SqlParameter("@fkSatislar", txtpkSatislar.Text));
                            list0.Add(new SqlParameter("@fkStokKarti", pkStokKarti));

                            sd = DB.ExecuteSQL(@"update SatisDetay set SatisFiyati=@EskiSatisFiyati,NakitFiyat =@EskiSatisFiyati2 
                                where fkSatislar=@fkSatislar and fkStokKarti=@fkStokKarti",list0);
                        }
                        else
                        {
                            //iskonto tutar hesapla
                            ArrayList list = new ArrayList();
                            list.Add(new SqlParameter("@EskiSatisFiyati", EskiSatisFiyati.ToString().Replace(",", ".")));
                            list.Add(new SqlParameter("@fkSatislar", txtpkSatislar.Text));
                            list.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
                            sd = DB.ExecuteSQL("update SatisDetay set iskontotutar=SatisFiyati-@EskiSatisFiyati where fkSatislar=@fkSatislar and fkStokKarti=@fkStokKarti",list);

                            //iskonto yüzde hesapla
                            //if (sd == "1")
                            ArrayList list2 = new ArrayList();
                            //list2.Add(new SqlParameter("@EskiSatisFiyati", EskiSatisFiyati.ToString().Replace(",", ".")));
                            list2.Add(new SqlParameter("@fkSatislar", txtpkSatislar.Text));
                            list2.Add(new SqlParameter("@fkStokKarti", pkStokKarti));

                            sd = DB.ExecuteSQL(@"update SatisDetay set iskontoyuzdetutar=(((SatisFiyati-(SatisFiyati-iskontotutar))*100)/SatisFiyati)
                                                     where fkSatislar=@fkSatislar and fkStokKarti=@fkStokKarti",list2);
                        }

                        ArrayList list3 = new ArrayList();
                        //list2.Add(new SqlParameter("@EskiSatisFiyati", EskiSatisFiyati.ToString().Replace(",", ".")));
                        list3.Add(new SqlParameter("@fkSatislar", txtpkSatislar.Text));
                        list3.Add(new SqlParameter("@fkStokKarti", pkStokKarti));

                        sd = DB.ExecuteSQL(@"update SatisDetay set SatisFiyatiKdvHaric=(SatisFiyati-((SatisFiyati)*KdvOrani)/(100+KdvOrani))
                        where fkSatislar=@fkSatislar and fkStokKarti=@fkStokKarti",list3);
                    }
                }
            }
            #endregion
            HizliMiktar = 1;

            if (Degerler.Uruneklendisescal)
                Digerislemler.UrunEkleSesCal();
        }

        void YeniSatis()
        {
            cbOdemeSekli.TabStop = false;
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
            list.Add(new SqlParameter("@fkKullanici", lueKullanicilar.EditValue.ToString()));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            list.Add(new SqlParameter("@AlinanPara", alinanpara.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));//iskontoTutar.Value.ToString().Replace(",", ".")));

            if (cbOdemeSekli.SelectedIndex == -1)// && cbOdemeSekli.TabStop == true)
            {
                cbOdemeSekli.SelectedIndex = 0;
                cbOdemeSekli.Text = Degerler.odemesekli;
            }
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.Text));

            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            string sf = Degerler.fkSatisFiyatlariBaslik.ToString();
            if (lueFiyatlar.EditValue != null && lueFiyatlar.EditValue.ToString() != "")
                sf = lueFiyatlar.EditValue.ToString();
            list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", sf));
            list.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
            list.Add(new SqlParameter("@fkDonemler", Degerler.fkDonemler));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

            sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara"+
                ",ToplamTutar,Yazdir,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen,OdemeSekli,SonislemTarihi,"+
                "BilgisayarAdi,fkSatisFiyatlariBaslik,aktarildi,fkDonemler,fkSube)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara"+
                ",@ToplamTutar,@Yazdir,@iskontoFaturaTutar,0,0,@OdemeSekli,getdate(),"+
                "@BilgisayarAdi,@fkSatisFiyatlariBaslik,@aktarildi,@fkDonemler,@fkSube) SELECT IDENT_CURRENT('Satislar')";
            fisno = DB.ExecuteScalarSQL(sql, list);

            if (fisno.Substring(0, 1) == "H")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }

            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
            }
            if (AcikSatisindex == 2 && Satis2Toplam.Tag.ToString() == "0")
            {
                Satis2Toplam.Tag = fisno;
                SatisDetayGetir_Yeni(Satis2Toplam.Tag.ToString());
            }
            if (AcikSatisindex == 3 && Satis3Toplam.Tag.ToString() == "0")
            {
                Satis3Toplam.Tag = fisno;
                SatisDetayGetir_Yeni(Satis3Toplam.Tag.ToString());
            }
            deGuncellemeTarihi.Enabled = true;
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1, int.Parse(lueSatisTipi.EditValue.ToString()));
            string pkSatislar = "0";
            if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();
            else if (AcikSatisindex == 2)
                pkSatislar = Satis2Toplam.Tag.ToString();
            else if (AcikSatisindex == 3)
                pkSatislar = Satis3Toplam.Tag.ToString();
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
                else if (dr["pkSatisDetay"].ToString() == "")
                    gridView1.DeleteRow(i);
            }
        }

        private void sonrakiSatisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AcikSatisindex == 1) AcikSatisindex = 2;
            else if (AcikSatisindex == 2) AcikSatisindex = 3;
            else if (AcikSatisindex == 3 && Satis4Firma.Visible == true) AcikSatisindex = 4;
            else if (AcikSatisindex == 3 && Satis4Firma.Visible == false) AcikSatisindex = 1;
            else if (AcikSatisindex == 4) AcikSatisindex = 1;

            SatisGetir();
            yesilisikyeni();
            Application.DoEvents();
        }

        void birimfiyatguncelle(string girilen)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr == null) return;

            string pkSatisDetay = dr["pkSatisDetay"].ToString();
            decimal NakitFiyati = decimal.Parse(dr["SatisFiyati"].ToString());
            //decimal.TryParse(dr["NakitFiyati"].ToString(), out NakitFiyati);
            decimal yapilan_iskonto = 0, iskontotutar = 0, iskontoyuzde = 0;
            decimal.TryParse(girilen, out iskontotutar);
            if (NakitFiyati==0)
            {
                NakitFiyati = iskontotutar;
                iskontotutar = 0;
                iskontoyuzde = 0;
                yapilan_iskonto = 0;
                int sonuc = DB.ExecuteSQL_Sonuc_Sifir("UPDATE SatisDetay SET NakitFiyat=" + NakitFiyati.ToString().Replace(",", ".") +
                ",SatisFiyati=" + NakitFiyati.ToString().Replace(",", ".") +
                " where pkSatisDetay=" + pkSatisDetay);

                if (sonuc == -1)
                    MessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz");

                sonuc = DB.ExecuteSQL("update SatisDetay set SatisFiyatiKdvHaric=(SatisFiyati-((SatisFiyati)*KdvOrani)/(100+KdvOrani))" +
           " where pkSatisDetay=" + pkSatisDetay);

                if (sonuc == -1)
                    MessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz");

            }
            else
            {
                yapilan_iskonto = NakitFiyati - iskontotutar;
                iskontoyuzde = (yapilan_iskonto * 100) / NakitFiyati;

                int sonuc = DB.ExecuteSQL_Sonuc_Sifir("UPDATE SatisDetay SET iskontotutar=" + yapilan_iskonto.ToString().Replace(",", ".") +
                ",iskontoyuzdetutar=" + iskontoyuzde.ToString().Replace(",", ".") +
                " where pkSatisDetay=" + pkSatisDetay);

                if (sonuc == -1)
                    MessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz");

                //    sonuc = DB.ExecuteSQL("update SatisDetay set SatisFiyatiKdvHaric=(SatisFiyati-((SatisFiyati-iskontotutar)*KdvOrani)/(100+KdvOrani))-iskontotutar" +
                //" where pkSatisDetay=" + pkSatisDetay);
                sonuc = DB.ExecuteSQL("update SatisDetay set SatisFiyatiKdvHaric=(SatisFiyati-((SatisFiyati)*KdvOrani)/(100+KdvOrani))" + 
                " where pkSatisDetay=" + pkSatisDetay);

                if (sonuc == -1)
                    MessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz");

            }



            //DB.ExecuteSQL("UPDATE SatisDetay SET iskontoTutar=(SatisFiyati*iskontoyuzdetutar)/100 where pkSatisDetay=" + pkSatisDetay);

            //DevExpress.XtraGrid.Columns.GridColumn gc = gridView1.FocusedColumn;

            //yesilisikyeni();

            //gridView1.Focus();
            //gridView1.FocusedColumn = gc;//gridView1.VisibleColumns[0];
            //gridView1.CloseEditor();
        }

        private void repositoryItemCalcEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                string girilen =
               ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

                birimfiyatguncelle(girilen);

                yesilisikyeni();
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {

                string girilen =
               ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

                birimfiyatguncelle(girilen);

                int i = gridView1.FocusedRowHandle;

                SatisDetayYenile();

                gridView1.FocusedRowHandle = i;
            }

        }

        private void pkSatisBarkod_Enter(object sender, EventArgs e)
        {
            txtpkSatislar.Text = "";
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            GeciciMusteriDefault();

            DataTable dt = DB.GetData("select top 3 pkSatislar,fkFirma from Satislar with(nolock) where  Siparis<>1 and fkKullanici=" + DB.fkKullanicilar);

            int c = dt.Rows.Count;
            if (c == 0)
            {
                //formislemleri.Mesajform("Açık Satış Bulunamadı","S");
            }
            else
            {
                for (int i = 0; i < c; i++)
                {
                    string pkSatislar = dt.Rows[i]["pkSatislar"].ToString();
                    string fkFirma = dt.Rows[i]["fkFirma"].ToString();
                    AcikSatisindex = i + 1;
                    if (i == 0)
                    {
                        Satis1Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar with(nolock) where pkFirma=" + fkFirma);
                        if (dtMusteri.Rows.Count > 0)
                        {
                            Satis1Firma.Tag = dtMusteri.Rows[0]["pkFirma"].ToString();
                            Satis1Baslik.ToolTip = dtMusteri.Rows[0]["OzelKod"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                            Satis1Baslik.Text = Satis1Baslik.ToolTip;
                        }
                        else
                        {
                            formislemleri.Mesajform("Müşteri Bulunamadı", "K", 200);
                            //    Satis1Firma.Tag = "0";
                            //    Satis1Baslik.ToolTip = "Müşteri Bulunamadı";
                            //    Satis1Baslik.Text = "Müşteri Bulunamadı";
                        }
                    }
                    else if (i == 1)
                    {
                        Satis2Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + fkFirma);

                        Satis2Firma.Tag = dtMusteri.Rows[0]["pkFirma"].ToString();
                        Satis2Baslik.ToolTip = dtMusteri.Rows[0]["OzelKod"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis2Baslik.Text = Satis2Baslik.ToolTip;
                        //Satis2Toplam.Tag = dt.Rows[i]["pkSatislar"].ToString();
                        SatisDetayToplamGetir(Satis2Toplam.Tag.ToString());
                    }
                    else if (i == 2)
                    {
                        Satis3Toplam.Tag = pkSatislar;
                        DataTable dtMusteri = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + fkFirma);

                        Satis3Firma.Tag = dtMusteri.Rows[0]["pkFirma"].ToString();
                        Satis3Baslik.ToolTip = dtMusteri.Rows[0]["OzelKod"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        Satis3Baslik.Text = Satis3Baslik.ToolTip;
                        //Satis3Toplam.Tag = dt.Rows[i]["pkSatislar"].ToString();
                        SatisDetayToplamGetir(Satis3Toplam.Tag.ToString());
                    }
                    if (i > 2) break;
                    //yesilisikyeni();
                }
                AcikSatisindex = 1;

                SatisGetir();

                SatisDetayToplamGetir(Satis1Toplam.Tag.ToString());

                yesilisikyeni();
                return;
            }
            //SatisDetayGetir("0");
            yesilisikyeni();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (gridView1.DataRowCount == 0) e.Cancel = true;
        }

        void Satis1ToplamGetir(int i)
        {
            AcikSatisindex = i;

            SatisGetir();
            gcMusteriSatis.DataSource = null;
            yesilisikyeni();

        }

        private void Satis1Toplam_Click(object sender, EventArgs e)
        {
            Satis1ToplamGetir(1);
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
            Satis1ToplamGetir(2);
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
                SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
            }
            else if (AcikSatisindex == 2)
            {
                SatisDetayGetir_Yeni(Satis2Toplam.Tag.ToString());
                TutarFont(Satis2Toplam);
            }
            else if (AcikSatisindex == 3)
            {
                SatisDetayGetir_Yeni(Satis3Toplam.Tag.ToString());
                TutarFont(Satis3Toplam);
            }
            else if (AcikSatisindex == 4)
            {
                SatisDetayGetir_Yeni(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
            }
        }

        void SatisDetayYenile2()
        {
            if (AcikSatisindex == 1)
            {
                SatisDetayGetir_Yeni2(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
            }
            else if (AcikSatisindex == 2)
            {
                SatisDetayGetir_Yeni2(Satis2Toplam.Tag.ToString());
                TutarFont(Satis2Toplam);
            }
            else if (AcikSatisindex == 3)
            {
                SatisDetayGetir_Yeni2(Satis3Toplam.Tag.ToString());
                TutarFont(Satis3Toplam);
            }
            else if (AcikSatisindex == 4)
            {
                SatisDetayGetir_Yeni2(Satis4Toplam.Tag.ToString());
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

        private void repositoryItemCalcEdit3_MouseDown(object sender, MouseEventArgs e)
        {
            //güncelleme sorunu oldu
            //((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue = null;
        }

        private void müşteriKArtıToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (AcikSatisindex == 1)
                DB.PkFirma = int.Parse(Satis1Firma.Tag.ToString());
            else if (AcikSatisindex == 2)
                DB.PkFirma = int.Parse(Satis2Firma.Tag.ToString());
            else if (AcikSatisindex == 3)
                DB.PkFirma = int.Parse(Satis3Firma.Tag.ToString());
            else if (AcikSatisindex == 4)
                DB.PkFirma = int.Parse(Satis4Firma.Tag.ToString());

            frmMusteriKarti MusteriKarti = new frmMusteriKarti(DB.PkFirma.ToString(), "");
            MusteriKarti.ShowDialog();
            if (DB.PkFirma == 0) DB.PkFirma = 1;

            //Müşteri özel kodu veya adı değişebilir
            //KODU DEĞİŞMEZ
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar with(nolock) where pkFirma=" + DB.PkFirma);
            string firmadi = dt.Rows[0]["Firmaadi"].ToString();
            string ozelkod = dt.Rows[0]["OzelKod"].ToString();
            switch (AcikSatisindex)
            {
                case 1:
                    {
                        Satis1Firma.Tag = DB.PkFirma;
                        Satis1Baslik.Text = ozelkod + "-" + firmadi;
                        Satis1Baslik.ToolTip = Satis1Baslik.Text;
                        break;
                    }
                case 2:
                    {
                        Satis2Firma.Tag = DB.PkFirma;
                        Satis2Baslik.Text = ozelkod + "-" + firmadi;
                        Satis2Baslik.ToolTip = Satis2Baslik.Text;
                        break;
                    }
                case 3:
                    {
                        Satis3Firma.Tag = DB.PkFirma;
                        Satis3Baslik.Text = ozelkod + "-" + firmadi;
                        Satis3Baslik.ToolTip = Satis3Baslik.Text;
                        break;
                    }
                case 4:
                    {
                        Satis4Firma.Tag = DB.PkFirma;
                        Satis4Baslik.Text = ozelkod + "-" + firmadi;
                        Satis4Baslik.ToolTip = Satis4Baslik.Text;
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

        private void sTOKKARTINIDÜZENLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStokKarti StokKarti = new frmStokKarti();
            DataTable dt =
            DB.GetData("SELECT pkStokKarti from StokKarti wiht(nolock) where Barcode='" + HizliBarkod + "'");
            if (dt.Rows.Count > 0)
                DB.pkStokKarti = int.Parse(dt.Rows[0][0].ToString());
            else
                DB.pkStokKarti = 0;
            StokKarti.ShowDialog();

            FisSatisAlisFiyatlarininiDegis(DB.pkStokKarti.ToString());

            Hizlibuttonlariyukle(true);
        }

        void FisSatisAlisFiyatlarininiDegis(string pkStokKarti)
        {
            int ikayit = DB.GetData(@"select pkSatisDetay from SatisDetay sd with(nolock)
                    inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                    where pkStokKarti=" + pkStokKarti + " and sk.SatisFiyati<>sd.SatisFiyati and fkSatislar=" + txtpkSatislar.Text.ToString()).Rows.Count;

            if (ikayit == 0) return;

            string sec = formislemleri.MesajBox("Fiyatları Güncellemek İstiyormusunuz?(İskonto Sıfırlanır)", "Fiyat Değişikliği Var", 3, 0);
            if (sec == "1")
            {
                //for (int s = 0; s < gridView1.DataRowCount; s++)
                //{
                    //DataRow dr = gridView1.GetDataRow(s);
                    //string pkSatisDetay = dr["pkSatisDetay"].ToString();

                    DB.ExecuteSQL(@"UPDATE SatisDetay SET iskontoyuzdetutar=0,iskontotutar=0,AlisFiyati= StokKarti.AlisFiyati,SatisDetay.SatisFiyati=StokKarti.SatisFiyati,
                    SatisDetay.NakitFiyat=StokKarti.SatisFiyati,SatisDetay.KdvOrani=StokKarti.KdvOrani From StokKarti 
                    where SatisDetay.fkStokKarti=StokKarti.pkStokKarti and fkStokKarti=" + pkStokKarti + " and fkSatislar="+txtpkSatislar.Text);
                //}

                DB.ExecuteSQL(@"UPDATE SatisDetay SET SatisFiyatiKdvHaric=SatisFiyati-((SatisFiyati*KdvOrani)/(100+KdvOrani))
                where fkSatislar=" + txtpkSatislar.Text+ " and fkStokKarti="+ pkStokKarti);
                
                //DB.ExecuteSQL("UPDATE SatisDetay SET TevkifatTutari=(((SatisFiyatiKdvHaric*KdvOrani)/100)*TevkifatOrani)/10 where fkSatislar=" + txtpkSatislar.Text);

                //DB.ExecuteSQL("UPDATE SatisDetay SET SatisFiyati=SatisFiyatiKdvHaric+((SatisFiyatiKdvHaric*KdvOrani)/100) where fkSatislar=" + pkSatisBarkod.Text);
            }
        }

        private void simpleButton8_Click_2(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste())  return;

            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("1");
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            Hizlibuttonlariyukle(false);
        }

        void Yenile()
        {
            txtpkSatislar.Text = "0";
            Satis1Toplam.Tag = "0";
            Satis2Toplam.Tag = "0";
            Satis3Toplam.Tag = "0";
            SatisDuzenKapat();
            timer1.Enabled = true;
            yesilisikyeni();
        }

        private void Satis3Baslik_DoubleClick(object sender, EventArgs e)
        {
            musteriara();
        }

        private void Satis3Baslik_Click(object sender, EventArgs e)
        {
            Satis3Baslik.Text = "";
            AcikSatisindex = 3;
            TutarFont(Satis3Toplam);
        }

        private void Satis3Baslik_Leave(object sender, EventArgs e)
        {
            if (Satis3Baslik.Text == "")
                Satis3Baslik.EditValue = Satis3Baslik.OldEditValue;
        }

        private void Satis3Baslik_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Satis3Baslik.Text == "")
                    musteriara();
                else
                    musteriata(Satis3Baslik.Text);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                //musteriata("1");

                yesilisikyeni();
            }
        }

        private void Satis1Baslik_DoubleClick_1(object sender, EventArgs e)
        {
            musteriara();
        }

        private void Satis2Baslik_DoubleClick(object sender, EventArgs e)
        {
            musteriara();
        }

        private void Satis2Baslik_Click(object sender, EventArgs e)
        {
            Satis2Baslik.Text = "";
            AcikSatisindex = 2;
            TutarFont(Satis2Toplam);
        }

        private void Satis1Baslik_Click(object sender, EventArgs e)
        {
            Satis1Baslik.Text = "";
            AcikSatisindex = 1;
            TutarFont(Satis1Toplam);
        }

        private void Satis2Baslik_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Satis2Baslik.Text == "")
                    musteriara();
                else
                    musteriata(Satis2Baslik.Text);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                //musteriata("1");

                yesilisikyeni();
            }
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
                //musteriata("1");

                yesilisikyeni();
            }
        }

        private void Satis2Baslik_Leave(object sender, EventArgs e)
        {
            if (Satis2Baslik.Text == "")
                Satis2Baslik.EditValue = Satis2Baslik.OldEditValue;
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

            Hizlibuttonlariyukle(true);
        }

        void YazdirSatisFaturasi(bool Disigner)
        {
            try
            {
                #region Yazici Sec
                string YaziciAdi = "", YaziciDosyasi = "";

                DataTable dtYazicilar =
                DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" + lueSatisTipi.EditValue.ToString());

                if (dtYazicilar.Rows.Count == 1)
                {
                    YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                    YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                    short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
                }
                else if (dtYazicilar.Rows.Count > 1)
                {
                    short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1, int.Parse(lueSatisTipi.EditValue.ToString()));

                    YaziciAyarlari.ShowDialog();

                    YaziciAyarlari.Tag = 0;
                    YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

                    if (YaziciAyarlari.YaziciAdi.Tag == null)
                        YaziciAdi = "";
                    else
                        YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                    YaziciAyarlari.Dispose();
                }

                if (YaziciAdi == "")
                {
                    //MessageBox.Show("Yazıcı Bulunamadı");
                    yesilisikyeni();
                    return;
                }
                #endregion

                string fisid = txtpkSatislar.Text;
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                //DataRow[] FisDr= FisDetay.Select("iade='False'");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
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
                DataTable Musteri = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + Fis.Rows[0]["fkFirma"].ToString());
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + YaziciDosyasi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "SatisFatura";
                rapor.Report.Name = "SatisFatura";

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

        void YazdirTeklif(bool Disigner)
        {
            try
            {
                string fisid = txtpkSatislar.Text;
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
FROM  Satislar s with(nolock)
INNER JOIN SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
INNER JOIN Firmalar with(nolock) ON s.fkFirma = Firmalar.pkFirma 
LEFT OUTER JOIN Kullanicilar with(nolock) ON s.fkKullanici = Kullanicilar.pkKullanicilar 
LEFT OUTER JOIN SatisDurumu with(nolock) ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
LEFT OUTER JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
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
                string fisid = txtpkSatislar.Text;
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
FROM  Satislar s with(nolock)
INNER JOIN SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
INNER JOIN Firmalar with(nolock) ON s.fkFirma = Firmalar.pkFirma 
LEFT OUTER JOIN Kullanicilar with(nolock) ON s.fkKullanici = Kullanicilar.pkKullanicilar 
LEFT OUTER JOIN SatisDurumu with(nolock) ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
LEFT OUTER JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
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

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if(DB.GetData("select pkSatislar from Satislar with(nolock) where duzenleme_tarihi is not null and pkSatislar = "+txtpkSatislar.Text).Rows.Count>0)
            {
                //DialogResult secim;
                //secim = 
                DevExpress.XtraEditors.XtraMessageBox.Show("Fiş Düzenlemede İade seçilemez!", "Dur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //if (secim == DialogResult.No) return;

                //formislemleri.MesajBox("Fiş Düzenlemede İade seçilemez!", Degerler.mesajbaslik,1, 2);
                //MessageBox.Show("Fiş Düzenlemede İade seçilemez!");
                yesilisikyeni();
                return;
            }

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
                //SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
                //TutarFont(Satis1Toplam);
                fkFirma = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 2)
            {
                //SatisDetayToplamGetir(Satis2Toplam.Tag.ToString());
                //TutarFont(Satis2Toplam);
                fkFirma = Satis2Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 3)
            {
                //SatisDetayToplamGetir(Satis3Toplam.Tag.ToString());
                //TutarFont(Satis3Toplam);
                fkFirma = Satis3Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                //SatisDetayToplamGetir(Satis4Toplam.Tag.ToString());
                //TutarFont(Satis4Toplam);
                fkFirma = Satis4Firma.Tag.ToString();
            }

            MusteriHareketleri(fkFirma);
        }

        private void bUFİŞMALİYETİToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bUFİŞMALİYETİToolStripMenuItem.Tag.ToString() == "0")
            {
                
                //görünüyorsa dokunma
                if (!gcAlisFiyatiKdvHaric.Visible)
                    gcAlisFiyatiKdvHaric.Tag = "1";
                else
                    gcAlisFiyatiKdvHaric.Tag = "0";
                gcAlisFiyatiKdvHaric.Visible = true;//alisfiyat
                

                //görünüyorsa dokunma
                if (!gridColumn31.Visible)
                    gridColumn31.Tag = "1";
                else
                    gridColumn31.Tag = "0";
                gridColumn31.Visible = true;//alisfiyat

                //görünüyorsa dokunma
                if (!gcAlisTutar.Visible)
                    gcAlisTutar.Tag = "1";
                else
                    gcAlisTutar.Tag = "0";
                gcAlisTutar.Visible = true;

                //görünüyorsa dokunma
                if (!gridColumn13.Visible)
                    gridColumn13.Tag = "1";
                else
                    gridColumn13.Tag = "0";
                gridColumn13.Visible = true;//karyuzde

                //görünüyorsa dokunma
                if (!gcKar.Visible)
                    gcKar.Tag = "1";
                else
                    gcKar.Tag = "0";

                gcKar.Visible = true;

                bUFİŞMALİYETİToolStripMenuItem.ForeColor = Color.Red;
                bUFİŞMALİYETİToolStripMenuItem.Tag = "1";
            }
            else
            {
                if (gcAlisFiyatiKdvHaric.Tag.ToString() == "1")
                    gcAlisFiyatiKdvHaric.Visible = false;

                if (gcAlisTutar.Tag.ToString() == "1")
                   gcAlisTutar.Visible = false;

                if (gcKar.Tag.ToString() == "1")
                    gcKar.Visible = false;

                if (gridColumn13.Tag.ToString() == "1")
                    gridColumn13.Visible = false;//karyuzde

                if (gridColumn31.Tag.ToString() == "1")
                    gridColumn31.Visible = false;//alisfiyat

                bUFİŞMALİYETİToolStripMenuItem.ForeColor = Color.Blue;
                bUFİŞMALİYETİToolStripMenuItem.Tag = "0";
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

            int girilen = 0;
            int.TryParse(formislemleri.inputbox("Eksik Miktar Girişi", "Alınacak Stok Miktarını Giriniz", "1", false), out girilen);
            if (girilen == 0)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız", "K", 200);
                yesilisikyeni();
                return;
            }
            int sonuc = DB.ExecuteSQL("INSERT INTO EksikListesi (Tarih,StokAdi,Miktar,fkStokKarti,Durumu) values(getdate(),'"
                + Stokadi + "'," + girilen.ToString().Replace(",", ".") + "," + fkStokKarti + ",'Yeni')");
            if (sonuc == -1)
            {
                Showmessage("Hata Oluştu.", "K");
            }
            yesilisikyeni();

        }

        private void lueKullanicilar_EditValueChanged(object sender, EventArgs e)
        {
            if (txtpkSatislar.Text != "" && lueKullanicilar.Tag == "1")
            {
                DB.ExecuteSQL("update Satislar set fkKullanici=" + lueKullanicilar.EditValue.ToString() +
                    " where pkSatislar=" + txtpkSatislar.Text);
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

                DataTable dt = DB.GetData("select * from SatisFiyatlari with(nolock) where Aktif=1 and fkStokKarti=" + fkStokKarti +
                    " and fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik);

                if (dt.Rows.Count == 0)
                    dt = DB.GetData("select * from SatisFiyatlari with(nolock) where fkStokKarti=" + fkStokKarti + " and fkSatisFiyatlariBaslik=1");

                string SatisFiyat1 = dt.Rows[0]["SatisFiyatiKdvli"].ToString();
                string sql = "update SatisDetay set SatisFiyati=@SatisFiyati,NakitFiyat=@NakitFiyat where pkSatisDetay=" + pkSatisDetay;
                sql = sql.Replace("@SatisFiyati", SatisFiyat1.Replace(",", "."));
                sql = sql.Replace("@NakitFiyat", SatisFiyat1.Replace(",", "."));
                DB.ExecuteSQL(sql);

                //DB.ExecuteSQL("UPDATE SatisDetay SET TevkifatTutari=(((SatisFiyatiKdvHaric*KdvOrani)/100)*TevkifatOrani)/10 where pkSatisDetay=" + pkSatisDetay);

                //DB.ExecuteSQL("UPDATE SatisDetay SET SatisFiyati=SatisFiyatiKdvHaric+((SatisFiyatiKdvHaric*KdvOrani)/100) where pkSatisDetay=" + pkSatisDetay);

            }
            DB.ExecuteSQL("update Satislar set fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik + " where pkSatislar=" + txtpkSatislar.Text);
            //iskonto yüzde güncelle

            if (formislemleri.MesajBox("Fiyat Grubu ve İskonto Oranları Değiştirilsin mi?", "Fiyat Grubu Değişim", 3, 2) == "1")
            {
                DataTable dtfg = DB.GetData("select Tur from SatisFiyatlariBaslik where pkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik);
                if (dtfg.Rows.Count > 0 && dtfg.Rows[0][0].ToString() == "1")
                    DB.ExecuteSQL(@"update SatisDetay set iskontoyuzdetutar=StokKarti.satis_iskonto from StokKarti 
                 where SatisDetay.fkStokKarti=StokKarti.pkStokKarti and  fkSatislar=" + txtpkSatislar.Text);
                else
                    DB.ExecuteSQL(@"update SatisDetay set iskontoyuzdetutar=StokKarti.satis_iskonto2 from StokKarti 
                 where SatisDetay.fkStokKarti=StokKarti.pkStokKarti and  fkSatislar=" + txtpkSatislar.Text);
            }
            DB.ExecuteSQL("update SatisDetay set iskontotutar=(SatisFiyati*iskontoyuzdetutar)/100 where fkSatislar=" + txtpkSatislar.Text);

            DB.ExecuteSQL("update SatisDetay set SatisFiyatiKdvHaric=SatisFiyati-(SatisFiyati*KdvOrani)/(100+KdvOrani) where fkSatislar=" + txtpkSatislar.Text);

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
            if (lueFiyatlar.Tag.ToString() == "1")
            {
                //DialogResult secim;
                //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Fiyat Grubunu Değiştirmek İstediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                //if (secim == DialogResult.Yes)
                //{
                string v = e.Value.ToString();
                FiyatGruplariDegis(v);
                //}
                //else
                //{
                //    e.AcceptValue = false;
                //    string v = lueFiyatlar.EditValue.ToString();
                //    FiyatGruplariDegis(v);
                //    lueFiyatlar.CancelPopup();
                //    //lueFiyatlar.EditValue = lueFiyatlar.OldEditValue;
                //    //lueFiyatlar.EditValue = e.Value.ToString();
                //}
            }
            yesilisikyeni();
        }

        private void deGuncellemeTarihi_EditValueChanged(object sender, EventArgs e)
        {
            if (deGuncellemeTarihi.Tag.ToString() == "1")
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@GuncellemeTarihi", deGuncellemeTarihi.DateTime));
                list.Add(new SqlParameter("@pkSatislar", txtpkSatislar.Text));
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
                fkfirma = Satis1Firma.Tag.ToString();
            }
            if (AcikSatisindex == 2)
            {
                fkfirma = Satis2Firma.Tag.ToString();
            }
            if (AcikSatisindex == 3)
            {
                fkfirma = Satis3Firma.Tag.ToString();
            }
            if (AcikSatisindex == 4)
            {
                fkfirma = Satis4Firma.Tag.ToString();
            }
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkfirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            //Odemeler();

            teBakiyeDevir.Text = MusteriBakiyeDevir(fkfirma);
        }

        private void ödemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (AcikSatisindex == 1)
            {
                fkfirma = Satis1Firma.Tag.ToString();
            }
            if (AcikSatisindex == 2)
            {
                fkfirma = Satis2Firma.Tag.ToString();
            }
            if (AcikSatisindex == 3)
            {
                fkfirma = Satis3Firma.Tag.ToString();
            }
            if (AcikSatisindex == 4)
            {
                fkfirma = Satis4Firma.Tag.ToString();
            }
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkfirma;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();
            //Odemeler();
            teBakiyeDevir.Text = MusteriBakiyeDevir(fkfirma);
        }
        public string MusteriBakiyeDevir(string fkFirma)
        {
            DataTable dt = DB.GetData("select isnull(Devir,0) From Firmalar With(nolock) where pkFirma=" + fkFirma);

            if (dt.Rows.Count == 0) return "0";

            decimal d = decimal.Parse(dt.Rows[0][0].ToString());
            return d.ToString("##0.00");
        }
        private void pmenuStokHareketleri_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
            yesilisikyeni();

            //frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0");
            //SatisUrunBazindaDetay.Tag = "2";
            //SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            //SatisUrunBazindaDetay.ShowDialog();
            //yesilisikyeni();
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
                gridView.OptionsView.ShowAutoFilterRow = true;
                gridView.OptionsFind.AlwaysVisible = true;

                //gridView.ShowFilterPopup(gridView.Columns[0]);
                gridView.OptionsView.ShowGroupPanel = false;
                gridView.OptionsBehavior.Editable = false;
                //gridView.FocusedRowChanged += new FocusedRowChangedEventHandler(gridView_FocusedRowChanged);
                this.gridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
                gridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridView_KeyDown);
            }
            else
            {
                gridView.ClearSorting();
                gridView.ClearColumnsFilter();
                gridView.ApplyFindFilter("");
                //gridView.FindFilterText.Remove;
            }
            //if (tEStokadi.Text.Length > 2)
            //{
            //    if (tEStokadi.Text.IndexOf(" ") == -1)
            string sql = "";
            //if (lueSatisTipi.EditValue.ToString() == ((int)Degerler.SatisDurumlari.Teklif).ToString())
            //{
            //    sql = @"select s.fkFirma,pkSatislar as FisNo,f.Firmaadi as MusteriAdi,cast(round(ToplamTutar, 3, 1) as decimal(18,2)) as ToplamTutar,s.GuncellemeTarihi as Tarih,
            //    s.OdemeSekli,k.KullaniciAdi,Sd.Durumu,s.Yazdir as Yazdırıldı,s.Aciklama,f.Devir as Bakiye from Satislar s with(nolock)
            //    LEFT JOIN Firmalar f with(nolock) ON s.fkFirma = f.PkFirma
            //    LEFT JOIN Kullanicilar k with(nolock) ON s.fkKullanici=k.pkKullanicilar
            //    LEFT JOIN SatisDurumu sd with(nolock) ON sd.pkSatisDurumu=s.fkSatisDurumu
            //    where s.Siparis=1 and s.fkSatisDurumu=1 order by SonislemTarihi desc";
            //}
            //else if (lueSatisTipi.EditValue.ToString() == ((int)Degerler.SatisDurumlari.İrsaliye).ToString())
            //{
            //    sql = @"select s.fkFirma,pkSatislar as FisNo,f.Firmaadi as MusteriAdi,cast(round(ToplamTutar, 3, 1) as decimal(18,2)) as ToplamTutar,
            //    s.GuncellemeTarihi as Tarih,
            //    s.OdemeSekli,k.KullaniciAdi,Sd.Durumu,s.TeslimTarihi,s.Yazdir as Yazdırıldı,s.Aciklama,f.Devir as Bakiye from Satislar s with(nolock)
            //    LEFT JOIN Firmalar f with(nolock) ON s.fkFirma = f.PkFirma
            //    LEFT JOIN Kullanicilar k with(nolock) ON s.fkKullanici=k.pkKullanicilar
            //    LEFT JOIN SatisDurumu sd with(nolock) ON sd.pkSatisDurumu=s.fkSatisDurumu
            //    where s.Siparis=1 and s.fkSatisDurumu=3 order by SonislemTarihi desc";
            //}
           if (lueSatisTipi.EditValue.ToString() == ((int)Degerler.SatisDurumlari.Sipariş).ToString())
            {
                sql = @"select  s.fkFirma,pkSatislar as FisNo,f.Firmaadi as MusteriAdi,
                cast(round(ToplamTutar, 3, 1) as decimal(18,2)) as ToplamTutar,s.GuncellemeTarihi as Tarih,
                s.OdemeSekli,k.KullaniciAdi,Sd.Durumu,s.Yazdir as Yazdırıldı,s.Aciklama,
                cast(round(f.Devir, 3, 1) as decimal(18,2)) as Bakiye from Satislar s with(nolock)
                LEFT JOIN Firmalar f with(nolock) ON s.fkFirma = f.PkFirma
                LEFT JOIN Kullanicilar k with(nolock) ON s.fkKullanici=k.pkKullanicilar
                LEFT JOIN SatisDurumu sd with(nolock) ON sd.pkSatisDurumu=s.fkSatisDurumu
                where s.Siparis=1 and s.fkSatisDurumu=10 and s.fkFirma>0";
                //Siparis (ASC), fkSatisDurumu (ASC), fkFirma (ASC)
            }
            //else if (lueSatisTipi.EditValue.ToString() == ((int)Degerler.SatisDurumlari.SFatura).ToString())
            //{
            //    sql = @"select top 20 s.fkFirma,pkSatislar as FisNo,f.Firmaadi as MusteriAdi,cast(round(ToplamTutar, 3, 1) as decimal(18,2)) as ToplamTutar,s.GuncellemeTarihi as Tarih,
            //    s.OdemeSekli,k.KullaniciAdi,Sd.Durumu,s.Yazdir as Yazdırıldı,s.Aciklama,f.Devir as Bakiye from Satislar s with(nolock)
            //    LEFT JOIN Firmalar f with(nolock) ON s.fkFirma = f.PkFirma
            //    LEFT JOIN Kullanicilar k with(nolock) ON s.fkKullanici=k.pkKullanicilar
            //    LEFT JOIN SatisDurumu sd with(nolock) ON sd.pkSatisDurumu=s.fkSatisDurumu
            //    where s.Siparis=1 and s.fkSatisDurumu=11 order by pkSatislar desc";
            //}
            else
            {
                sql = @"select Top 50 s.fkFirma,pkSatislar as FisNo,f.Firmaadi as MusteriAdi,
                cast(round(ToplamTutar, 3, 1) as decimal(18,2)) as ToplamTutar,s.GuncellemeTarihi as Tarih,
                s.OdemeSekli,k.KullaniciAdi,Sd.Durumu,s.Yazdir as Yazdırıldı,s.Aciklama,
                cast(round(f.Devir, 3, 1) as decimal(18,2)) as Bakiye from Satislar s with(nolock)
                LEFT JOIN Firmalar f with(nolock) ON s.fkFirma = f.PkFirma
                LEFT JOIN Kullanicilar k with(nolock) ON s.fkKullanici=k.pkKullanicilar
                LEFT JOIN SatisDurumu sd with(nolock) ON sd.pkSatisDurumu=s.fkSatisDurumu
                where s.Siparis=1 and s.fkSatisDurumu>0 and s.GuncellemeTarihi>getdate()-30";//s.fkSatisDurumu>1 and
                if (btnGecmisFis.Tag.ToString()=="0")
                    sql = sql + " and s.fkKullanici=" + DB.fkKullanicilar;

                sql = sql + " order by s.GuncellemeTarihi desc";

                //string Dosya3 = DB.exeDizini + "\\SonSatislarGrid.xml";

                //if (File.Exists(Dosya3))
                //{
                //    gridView.RestoreLayoutFromXml(Dosya3);
                //    gridView.ActiveFilter.Clear();
                //}
            }

            targetGrid.DataSource = DB.GetData(sql);

            targetGrid.BringToFront();
            targetGrid.Width = 750;
            targetGrid.Height = 400;
            targetGrid.Left = txtpkSatislar.Left + pGecmisFisler.Left;
            targetGrid.Top = txtpkSatislar.Top + 60;
            if (gridView.Columns.Count > 0)
            {
                gridView.Columns[1].Width = 50;
                gridView.Columns[2].Width = 150;
                gridView.Columns[4].Width = 100;
                gridView.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView.Columns[4].DisplayFormat.FormatString = "g";

                //gridView.Columns[9].DisplayFormat.FormatString = "{0:n2}";
                //gridView.Columns[9].DisplayFormat.FormatType = FormatType.Numeric;

                gridView.Columns[0].Visible = false;
            }
            if (gridView.DataRowCount == 0)
                targetGrid.Visible = false;
            else
            {
                targetGrid.Visible = true;
                targetGrid.Focus();
            }

            string Dosya2 = DB.exeDizini + "\\SonSatislarGrid.xml";

            if (File.Exists(Dosya2))
            {
                gridView.RestoreLayoutFromXml(Dosya2);
                gridView.ActiveFilter.Clear();
            }
            gridView.OptionsView.ShowAutoFilterRow = true;
            gridView.ShowFilterPopup(gridView.Columns[0]);
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

        private void tMenuitemOdemeAl_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            fkfirma = dr["fkFirma"].ToString();
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
        void iskontoguncelle(string iskontoyuzde)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            decimal SatisFiyati = decimal.Parse(dr["SatisFiyati"].ToString());
            decimal iskontotutar = 0;
            iskontotutar = SatisFiyati * decimal.Parse(iskontoyuzde) / 100;

            //iskontotutarDegistir(iskontotutar.ToString());

            int sonuc = DB.ExecuteSQL("UPDATE SatisDetay SET iskontoyuzdetutar=" + iskontoyuzde.Replace(",", ".") +
                ",iskontotutar=" + iskontotutar.ToString().Replace(",", ".") +
                " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());

            if (sonuc == -1)
                MessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz");

            //yesilisikyeni();
        }
        private void repositoryItemCalcEdit4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                string yeniiskonto =
               ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

                iskontoguncelle(yeniiskonto);

                yesilisikyeni();
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {

                string yeniiskonto =
               ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

                iskontoguncelle(yeniiskonto);

                int i = gridView1.FocusedRowHandle;

                SatisDetayYenile();

                gridView1.FocusedRowHandle = i;
            }
        }
        private void repositoryItemCalcEdit4_Leave(object sender, EventArgs e)
        {

            ////string oncekiiskonto = ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).OldEditValue.ToString();
            //string yeniiskonto =
            //    ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

            //int i = gridView1.FocusedRowHandle;

            //iskontoguncelle(yeniiskonto);

            //SatisDetayYenile();

            //gridView1.FocusedRowHandle = i;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //string pkSatisDetay = dr["pkSatisDetay"].ToString();
            //DB.ExecuteSQL("UPDATE SatisDetay SET iskontoyuzdetutar=" + yeniiskonto.Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);

            ////MiktarDegistir(yenimiktar);
            //int i = gridView1.FocusedRowHandle;
            //SatisDetayYenile();
            //gridView1.FocusedRowHandle = i;
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
                string fkSatisDetay= dr["pkSatisDetay"].ToString();
                string adet = dr["Adet"].ToString();
                frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay(fkFirma, fkSatisDetay,adet);
                SatisUrunBazindaDetay.Tag = "3";
                SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
                //SatisUrunBazindaDetay.pkStokKarti.Tag = dr["Miktar"].ToString();
                SatisUrunBazindaDetay.ShowDialog();
                yesilisikyeni();
            }
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            #region iade
            if (e.Clicks == 1 && e.Button == MouseButtons.Left)
            {
                GridHitInfo info;
                Point pt = gridView1.GridControl.PointToClient(Control.MousePosition);
                info = gridView1.CalcHitInfo(pt);

                if (info.InColumn && info.Column.FieldName == "iade")
                {
                    if (getCheckedCount() == gridView1.DataRowCount)
                        UnChekAll();
                    else
                        CheckAll();

                    yesilisikyeni();
                }

            }
            #endregion

            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            //if (ghi.Column == null) return;
            ////GridView View = sender as GridView;
            //filtir ise
            if (ghi.RowHandle == -999997) return;
            if (ghi.RowHandle == -2147483647) return;

            if (ghi.InRowCell)
            {
                int rowHandle = ghi.RowHandle;
                //DevExpress.XtraGrid.Columns.GridColumn column = ghi.Column;
                #region tutar
                if (ghi.Column.FieldName == "Tutar")
                {
                    frmFisAciklama fFisAciklama = new frmFisAciklama();
                    fFisAciklama.panelControl2.Visible = false;
                    fFisAciklama.btnCancel.Visible = true;
                    fFisAciklama.btnTemizle.Visible = false;
                    fFisAciklama.Text = "Tutar Hesapla";
                    fFisAciklama.pcTutarHesapla.Visible = true;
                    fFisAciklama.pcTutarHesapla.BringToFront();

                    DataRow dr = gridView1.GetDataRow(ghi.RowHandle);
                    decimal tutar = decimal.Parse(dr["Tutar"].ToString());
                    decimal nakitfiyati = decimal.Parse(dr["NakitFiyati"].ToString());
                    decimal iskontotutar, iskonyuzdetotutar = 0;
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
                    satisfiyati = fFisAciklama.calcEdit1.Value;
                    //kdv dahil ise
                    if (cbFaturaTipi.SelectedIndex == 0)
                    {
                        aliskdvdahil = satisfiyati;
                        //kdvtutar = (SatisFiyatiKdv * kdv) / (100 + kdv);
                        aliskdvharic = satisfiyati - ((satisfiyati * kdvorani) / (100 + kdvorani));
                    }
                    else
                    {
                        aliskdvdahil = satisfiyati + (fFisAciklama.calcEdit1.Value * kdvorani) / 100;
                        aliskdvharic = satisfiyati;
                    }

                    iskontotutar = nakitfiyati - satisfiyati;
                    iskonyuzdetotutar = (iskontotutar * 100) / nakitfiyati;

                    int s = DB.ExecuteSQL("update SatisDetay set iskontotutar=" + iskontotutar.ToString().Replace(",", ".") +
                         ",iskontoyuzdetutar=" + iskonyuzdetotutar.ToString().Replace(",", ".") +
                        " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());

                    if (s != 1)
                        MessageBox.Show("Hata Oluştu: " + s.ToString());

                    SatisDetayYenile2();

                    gridView1.FocusedColumn = gridColumn1;
                    gridView1.FocusedRowHandle = rowHandle;

                    gridView1.ShowEditor();
                    return;
                }
                #endregion
                if (ghi.Column.FieldName == "Mevcut")
                {
                    DataRow dr = gridView1.GetDataRow(ghi.RowHandle);

                    frmStokKartiDepoMevcut StokKartiDepoMevcut = new frmStokKartiDepoMevcut(int.Parse(dr["fkStokKarti"].ToString()));
                    StokKartiDepoMevcut.ShowDialog();

                    SatisDetayYenile2();
                }
                else if (ghi.Column.FieldName == "aciklama_detay")
                {
                    //gcAciklamaDetay
                    DataRow dr = gridView1.GetDataRow(ghi.RowHandle);

                    string pkSatisDetay = dr["pkSatisDetay"].ToString();

                    frmSatisDetayAciklama SatisDetayAciklama = new frmSatisDetayAciklama(pkSatisDetay);
                    SatisDetayAciklama.ShowDialog();

                    yesilisikyeni();

                    // SatisDetayYenile2();
                }
            }

            if (ghi.RowHandle < 0 && gridView1.CustomizationForm == null)
                yesilisikyeni();

        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            else
                MusteriSonSatislari(e.RowHandle);

            //gcSatisDetay_MouseDoubleClick(sender, e);
        }

        void MusteriSonSatislari(int RowHandle)
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

            DataRow dr = gridView1.GetDataRow(RowHandle);

            string sql = "";
            //and s.fkSatisDurumu in(1,2,4,5,9)  kaldırıldı 25.05.2018
            sql = @"select top 20 s.pkSatislar,sdu.Durumu,
                        s.GuncellemeTarihi as Tarih,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as Fiyati from StokKarti sk with(nolock)
                        inner join SatisDetay sd with(nolock) on sd.fkStokKarti=sk.pkStokKarti
                        inner join Satislar s with(nolock) on s.pkSatislar=sd.fkSatislar
                        left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu
                        where s.Siparis=1 and sk.pkStokKarti=@fkStokKarti and s.fkFirma=@fkFirma
                        order by s.GuncellemeTarihi desc";
            sql = sql.Replace("@fkStokKarti", dr["fkStokKarti"].ToString());
            sql = sql.Replace("@fkFirma", fkFirma);

            gcMusteriSatis.DataSource = DB.GetData(sql);

            gcMusteriSatis.Visible = true;
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

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

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
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\SatisFaturaGrid.xml";
            if (File.Exists(Dosya))
                File.Delete(Dosya);

            simpleButton19_Click(sender, e);


            //if (File.Exists(Dosya))
            //{
            //    for (int i = 0; i < gridView1.Columns.Count; i++)
            //        gridView1.Columns[i].Visible = true;
            //    File.Delete(Dosya);
            //}
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

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string fisno = txtpkSatislar.Text;
            gcSatisDetay.ExportToXls(exedizini + "/" + fisno + "Fis.xls");
            Process.Start(exedizini + "/" + fisno + "Fis.xls");
        }

        private void gridView1_HideCustomizationForm(object sender, EventArgs e)
        {
            kaydetToolStripMenuItem1_Click(sender, e);
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

        private void repositoryItemCalcEdit3_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
            gridView1.CloseEditor();
        }

        private void excelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gridView1.ExportToXls(Application.StartupPath + "Satislar.Xls");
            Process.Start(Application.StartupPath + "Satislar.Xls");
        }

        private void excelToolStripMenuItem2_Click(object sender, EventArgs e)
        {

            DataTable dt =
            DB.GetData("select Firmaadi,Eposta from Firmalar  with(nolock) where pkFirma=" + Satis1Firma.Tag.ToString());
            if (dt.Rows[0]["Eposta"].ToString().Length < 5)
            {
                MessageBox.Show(dt.Rows[0]["Firmaadi"].ToString() + "\nMüşterinin E-Posta Adresini Kontrol Ediniz!");
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dt.Rows[0]["Eposta"].ToString() + " E-Posta Gönderilecek Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            gridView1.ExportToXls(Application.StartupPath + "\\Teklif.Xls");
            DB.epostagonder(dt.Rows[0]["Eposta"].ToString(), "Teklif Listesi", Application.StartupPath + "\\Teklif.Xls", "Teklif");
            frmMesajBox mesaj = new frmMesajBox(200);
            mesaj.label1.Text = "E-Posta Gönderildi.";
            mesaj.Show();
        }


        private void repositoryItemCalcEdit3_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Up)
            //{
            //    if (gridView1.FocusedRowHandle == 0)
            //        gridView1.FocusedRowHandle = gridView1.FocusedRowHandle - 1;
            //}
        }

        private void repositoryItemSpinEdit1_Leave(object sender, EventArgs e)
        {
            string girilen =
              ((DevExpress.XtraEditors.SpinEdit)((((DevExpress.XtraEditors.SpinEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null) return;

            string pkSatisDetay = dr["pkSatisDetay"].ToString();

            string s = formislemleri.MesajBox("Kdv Oranı Değişsin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0")
            {
                //yesilisikyeni();
                return;
            }
            //decimal NakitFiyati = decimal.Parse(dr["NakitFiyati"].ToString());
            //decimal iskonto = 0, iskontotutar = 0, iskontoyuzde = 0;
            //decimal.TryParse(girilen, out iskontotutar);
            //iskonto = NakitFiyati - iskontotutar;
            //iskontoyuzde = (iskonto * 100) / NakitFiyati;

            int m= DB.ExecuteSQL("UPDATE SatisDetay SET KdvOrani=" + girilen.ToString().Replace(",", ".") +
                " where pkSatisDetay=" + pkSatisDetay);
            if(m!=1)
              formislemleri.Mesajform("Kdv Oranı Güncellenemedi.", "K", 100);
            //  formislemleri.Mesajform("Kdv Oranı Güncellendi.", "S", 50);
            //else

            yesilisikyeni();
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {
            Fiyatlarigetir();
        }

        private void cbFaturaTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFaturaTipi.Tag.ToString() == "0") return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Fiyatları " + cbFaturaTipi.Text + " Yapılacaktır Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
            if (secim == DialogResult.No) return;

            if (cbFaturaTipi.SelectedIndex == 0)
                DB.ExecuteSQL(@"update SatisDetay set SatisFiyati=SatisFiyati+((SatisFiyati*KdvOrani)/(100+KdvOrani)) From where fkSatislar=" + txtpkSatislar.Text);
            else
                DB.ExecuteSQL(@"update SatisDetay set SatisFiyati=SatisFiyati-((SatisFiyati*KdvOrani)/(100+KdvOrani)) From where fkSatislar=" + txtpkSatislar.Text);
            //}
            //else
            //{
            //    DB.ExecuteSQL("update SatisDetay set isKdvHaric=1 where fkSatislar=" + txtpkSatislar.Text);

            //    DB.ExecuteSQL(@"update SatisDetay set SatisFiyati=StokKarti.SatisFiyatiKdvHaric,AlisFiyati=StokKarti.AlisFiyatiKdvHaric From StokKarti
            //                    where SatisDetay.fkStokKarti=StokKarti.pkStokKarti and fkSatislar=" + txtpkSatislar.Text);
            //}

            yesilisikyeni();
        }

        void toplu_kdv_iskonto_degis(string ki)
        {
            inputForm sifregir = new inputForm();
            //sifregir.Girilen.Properties.PasswordChar = '#';
            if (ki == "k")
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

            if (girilen == "")
            {
                yesilisikyeni();
                return;
            }

            girilen = girilen.Replace(",", ".");
            int sonuc = 0;
            //kdv oranı
            if (ki == "k")
            {
                sonuc = DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set iskontotutar=0,iskontoyuzdetutar=0," +
                    "SatisFiyatiKdvHaric = NakitFiyat - ((NakitFiyat * " + girilen + ") / (100 + " + girilen + "))," +
                    "KdvOrani=" + girilen +
                    " where fkSatislar=" + txtpkSatislar.Text);

                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Fiyatlarıda Güncellensin mi? ", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                if (secim == DialogResult.Yes)
                {
                    sonuc = DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set iskontotutar=0,iskontoyuzdetutar=0," +
                    "SatisFiyati = NakitFiyat - ((NakitFiyat * " + girilen + ") / (100 + " + girilen + "))," +
                    "KdvOrani=" + girilen +
                    " where fkSatislar=" + txtpkSatislar.Text);
                }
            }
            
            //iskonto
            if (ki == "i")
            {
                if (girilen == "0")
                    sonuc = DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set iskontotutar=0,iskontoyuzdetutar=0" +
                        " where fkSatislar=" + txtpkSatislar.Text);
                else
                {
                    if(decimal.Parse(girilen)>0)
                      sonuc = DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set iskontoyuzdetutar=iskontoyuzdetutar+" + girilen +
                      ",iskontotutar=iskontotutar+(NakitFiyat*isnull(" + girilen + ",0))/100" +
                      " where fkSatislar=" + txtpkSatislar.Text);
                    else
                        sonuc = DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set iskontoyuzdetutar=" + girilen +
                      ",iskontotutar=NakitFiyat+(NakitFiyat*isnull(" + girilen + ",0))/100" +
                      " where fkSatislar=" + txtpkSatislar.Text);
                }
                //if (girilen=="0")
                //    DB.ExecuteSQL(@"update SatisDetay set iskontotutar=0  where fkSatislar=" + pkSatisBarkod.Text);
                //else
                //DB.ExecuteSQL("update SatisDetay set iskontotutar=(NakitFiyat*isnull(" + girilen + ",0))/100" +
                //                " where fkSatislar="+pkSatisBarkod.Text);
            }

            if (sonuc != 0)
            {
                MessageBox.Show("Hata Oluştu Lütfen Tekrar Deneyiniz");
                yesilisikyeni();
                return;
            }

            yesilisikyeni();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button4_Click_1(sender,e);
            //toplu_kdv_iskonto_degis("i");

            //yesilisikyeni();            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            toplu_kdv_iskonto_degis("k");
        }

        private void repositoryItemComboBox1_Leave(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            string girilen = ((DevExpress.XtraEditors.ComboBoxEdit)(sender)).EditValue.ToString(); ;
            //((DevExpress.XtraEditors.ComboBox)(((DevExpress.XtraEditors.ComboBox)(sender)).Properties.OwnerEdit)).Text;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("update SatisDetay set Birimi='" + girilen +
                "' where pkSatisDetay=" + dr["pkSatisDetay"].ToString());

            DataTable dtStok = DB.GetData("select Stoktipi,KutuFiyat,AlisFiyati,AlisFiyatiKdvHaric,SatisFiyati,SatisFiyatiKdvHaric from StokKarti with(nolock) where pkStokKarti = " + dr["fkStokKarti"].ToString());
            if (dtStok.Rows[0]["Stoktipi"].ToString() == girilen)
            {
                string SatisFiyati = dtStok.Rows[0]["SatisFiyati"].ToString();
                string AlisFiyati = dtStok.Rows[0]["AlisFiyati"].ToString();

                if (cbFaturaTipi.SelectedIndex == 1)
                {
                    AlisFiyati = dtStok.Rows[0]["AlisFiyatiKdvHaric"].ToString();
                    SatisFiyati = dtStok.Rows[0]["SatisFiyatiKdvHaric"].ToString();
                }

                DB.ExecuteSQL("update SatisDetay set Adet=1,SatisFiyati= " + SatisFiyati.Replace(",", ".") +
                    ",AlisFiyati=" + AlisFiyati.Replace(",", ".") +
               " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            }
            else
            {
                string SatisFiyati = dtStok.Rows[0]["SatisFiyati"].ToString();
                string AlisFiyati = dtStok.Rows[0]["AlisFiyati"].ToString();

                if (cbFaturaTipi.SelectedIndex == 1)
                {
                    AlisFiyati = dtStok.Rows[0]["AlisFiyatiKdvHaric"].ToString();
                    SatisFiyati = dtStok.Rows[0]["SatisFiyatiKdvHaric"].ToString();
                }

                DB.ExecuteSQL("update SatisDetay set Adet=" + dtStok.Rows[0]["KutuFiyat"].ToString() +
                    ",SatisFiyati=" + SatisFiyati.Replace(",", ".") + "/ " + dtStok.Rows[0]["KutuFiyat"].ToString() +
                     ",AlisFiyati=" + AlisFiyati.Replace(",", ".") + "/ " + dtStok.Rows[0]["KutuFiyat"].ToString() +
               " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            }
        }

        private void repositoryItemSpinEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
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


            DataTable dtStokKarti = DB.GetData("select pkStokKarti,Barcode From StokKarti with(nolock) WHERE Barcode='" + barkod + "'");

            //eğer stok kartı oluşturmadı ise 
            if (dtStokKarti.Rows.Count == 0)
            {
                yesilisikyeni();
                //StokKarti.Dispose();
                return;
            }

            //ürün gerçekten kaydedildi ise okutarak satış yapsın 24.08.2014
            if (gridView1.DataRowCount == 0)
                YeniSatisEkle();

            urunekle(dtStokKarti.Rows[0]["Barcode"].ToString());

            Degerler.stokkartisescal = true;
            yesilisikyeni();
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            btnYazdirKaydetmi(true);
        }

        void btnYazdirKaydetmi(bool iskaydet)
        {
            #region Müşteri Kontrol
            string fkFirma = "1";
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
            #endregion

            if (fkFirma == "1" && (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura))
            {
                //formislemleri.Mesajform("Müşteri Seçiniz", "K", 200);
                musteriara();
                yesilisikyeni();
                return;
            }
            if (fkFirma == "1" && (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.İrsaliye))
            {
                //formislemleri.Mesajform("Müşteri Seçiniz", "K", 200);
                musteriara();
                yesilisikyeni();
                return;
            }
            if (fkFirma == "1" && (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.SFatura))
            {
                //formislemleri.Mesajform("Müşteri Seçiniz", "K", 200);

                musteriara();
                yesilisikyeni();
                return;
            }
            if (fkFirma == "1" && (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Teklif))
            {
                //formislemleri.Mesajform("Müşteri Seçiniz", "K", 200);

                musteriara();
                yesilisikyeni();
                return;
            }
            if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Teklif)
            {
                if (deTeklifTarihi.EditValue == null)
                    deTeklifTarihi.DateTime = DateTime.Now;
                if (deTeslimTarihi.EditValue == null)
                    deTeslimTarihi.DateTime = DateTime.Now;

                DB.ExecuteSQL("UPDATE Satislar set Yazdir=1,ToplamTutar=" + aratoplam.Value.ToString().Replace(",", ".") +
                ",TeklifTarihi='" + deTeklifTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:ss") +
                "',TeslimTarihi='" + deTeslimTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:ss") +
                "',OncekiBakiye=" + teBakiyeDevir.Text.Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                vYazdir(1);
            }
            else if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Fatura)
            {
                if (deFaturaTarihi.EditValue == null)
                    deFaturaTarihi.DateTime = DateTime.Now;

                TimeSpan ts = DateTime.Now - deFaturaTarihi.DateTime;
                if (ts.Days > 0 || ts.Hours > 0 && deFaturaTarihi.EditValue != null && AcikSatisindex < 4)
                {
                    DialogResult secim;
                    secim = DevExpress.XtraEditors.XtraMessageBox.Show("Fatura Tarihi Farkı " + ts.Days.ToString() + " Gün " +
                        ts.Hours.ToString() + " Saattir. Devam Etmek İstiyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);

                    if (secim == DialogResult.No)
                    {
                        yesilisikyeni();
                        return;
                    }
                }

                DB.ExecuteSQL("UPDATE Satislar set Yazdir=1,ToplamTutar=" + aratoplam.Value.ToString().Replace(",", ".") +
                ",FaturaTarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") +
                "',OncekiBakiye=" + teBakiyeDevir.Text.Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                YazdirSatisFaturasi(false);
            }
            else if (lueSatisTipi.Text == "Tevkifat")//(int)Degerler.SatisDurumlari.Tevkifat)
            {
                if (deFaturaTarihi.EditValue == null)
                    deFaturaTarihi.DateTime = DateTime.Now;

                DB.ExecuteSQL("UPDATE Satislar set Yazdir=1,ToplamTutar=" + aratoplam.Value.ToString().Replace(",", ".") +
                ",FaturaTarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") +
                "',OncekiBakiye=" + teBakiyeDevir.Text.Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                YazdirSatisFaturasi(false);
            }
            else if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.İrsaliye)
            {
                if (deFaturaTarihi.EditValue == null)
                    deFaturaTarihi.DateTime = DateTime.Now;

                TimeSpan ts = DateTime.Now - deFaturaTarihi.DateTime;
                if (ts.Days > 0 || ts.Hours > 0 && deFaturaTarihi.EditValue != null && AcikSatisindex < 4)
                {
                    DialogResult secim;
                    secim = DevExpress.XtraEditors.XtraMessageBox.Show("İrsaliye Tarihi Farkı " + ts.Days.ToString() + " Gün " +
                        ts.Hours.ToString() + " Saattir. Devam Etmek İstiyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);

                    if (secim == DialogResult.No)
                    {
                        yesilisikyeni();
                        return;
                    }
                }

                DB.ExecuteSQL("UPDATE Satislar set Yazdir=1,ToplamTutar=" + aratoplam.Value.ToString().Replace(",", ".") +
                ",irsaliye_tarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") +
                "',OncekiBakiye=" + teBakiyeDevir.Text.Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                YazdirSatisFaturasi(false);
            }
            else if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.SFatura)
            {
                if (deFaturaTarihi.EditValue == null)
                    deFaturaTarihi.DateTime = DateTime.Now;

                DB.ExecuteSQL("UPDATE Satislar set Yazdir=1,ToplamTutar=" + aratoplam.Value.ToString().Replace(",", ".") +
                ",FaturaTarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") +
                "',OncekiBakiye=" + teBakiyeDevir.Text.Replace(",", ".") +
                " where pkSatislar=" + txtpkSatislar.Text);

                YazdirSatisFaturasi(false);
            }
            else
            {
                KaydetYazdir(true,iskaydet);
            }
        }
        private void vYazdir(Int16 dizayner)
        {
            string YaziciAdi = "", YaziciDosyasi = "";

            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" + lueSatisTipi.EditValue.ToString());

            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,int.Parse(lueSatisTipi.EditValue.ToString()));

                YaziciAyarlari.ShowDialog();
                
                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }

            if (YaziciAdi == "")
                yesilisikyeni();
            else
                FisYazdir(dizayner, txtpkSatislar.Text, YaziciDosyasi, YaziciAdi);
        }

        void FisYazdir(Int16 Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli from KasaHareket with(nolock) where fkSatislar=" + fisid);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }

                xrCariHareket rapor = new xrCariHareket();
                //if (yazdirmaadedi>1)
                //  rapor.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);

                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = SatisFisTipi;
                rapor.Report.Name = SatisFisTipi;
                //ön izleme
                if (Disigner == 3)
                {
                    rapor.ShowPreview();
                }
                //dizayn
                else if (Disigner == 2)
                    rapor.ShowDesigner();
                else if (Disigner == 1)
                {
                    if (yazdirmaadedi < 1) yazdirmaadedi = 1;

                    if (int.Parse(lueSatisTipi.EditValue.ToString()) == (int)Degerler.SatisDurumlari.Teklif)
                        rapor.ShowPreview();
                    else
                    {
                        for (int i = 0; i < yazdirmaadedi; i++)
                            rapor.Print(YaziciAdi);
                    }
                    DB.ExecuteSQL("update satislar set Yazdir=1 where pkSatislar=" + fisid);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void OdemeSekli(int odemesekli)
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
            else if (AcikSatisindex == 2)
            {
                pkSatislar = Satis2Toplam.Tag.ToString();
                musteriadi = Satis2Baslik.Text;
                if (Satis2Firma.Tag != null)
                    pkFirma = Satis2Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 3)
            {
                pkSatislar = Satis3Toplam.Tag.ToString();
                musteriadi = Satis3Baslik.Text;
                if (Satis3Firma.Tag != null)
                    pkFirma = Satis3Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                pkSatislar = Satis4Toplam.Tag.ToString();
                musteriadi = Satis4Baslik.Text;
                if (Satis4Firma.Tag != null)
                    pkFirma = Satis4Firma.Tag.ToString();
            }
            DB.pkSatislar = int.Parse(pkSatislar);
            DB.PkFirma = int.Parse(pkFirma);
            lueKKarti.Visible = false;

            if (odemesekli == (int)Degerler.OdemeSekli.Nakit)
            {
                //
            }
            else if (odemesekli == (int)Degerler.OdemeSekli.KrediKarti || odemesekli == (int)Degerler.OdemeSekli.Banka)
            {
                lueKKarti.Properties.DataSource = DB.GetData("Select * from Bankalar with(nolock) where Aktif=1");
                lueKKarti.Visible = true;
                //lueKKarti.Focus();
                lueKKarti.ItemIndex = 0;
            }
            else if (odemesekli == (int)Degerler.OdemeSekli.AcikHesap)
            {
                if (pkFirma == "1")
                {
                    pkFirma = musteriara();
                }
                //if (pkFirma == "1")
                //{
                //    MessageBox.Show("1 Nolu Müşteriye Açık Hesap Olamaz!");
                    //cbOdemeSekli.Enabled = true;
                    //cbOdemeSekli.Text = "Nakit";
                //}
            }
            if (odemesekli == (int)Degerler.OdemeSekli.Diger)
            {
                if (pkFirma == "1" && Degerler.MusteriZorunluUyari)
                {
                    pkFirma = musteriara();
                }
                KaydetYazdir(false,true);
            }
        }

        private void vSatisOdeme(object sender, EventArgs e)
        {
            if (ilkyukleme) return;
            if (txtpkSatislar.Text == "") return;

            cbOdemeSekli.BackColor = System.Drawing.Color.White;
            lueKKarti.Visible = false;

            if (cbOdemeSekli.TabStop == true)
            {
                OdemeSekli(cbOdemeSekli.SelectedIndex);

                DB.ExecuteSQL("UPDATE satislar set OdemeSekli='" + cbOdemeSekli.Text + "' where pkSatislar=" + txtpkSatislar.Text);
            }

            Satis1Baslik.BackColor = Satis1Toplam.BackColor;
            Satis2Baslik.BackColor = Satis2Toplam.BackColor;
            Satis3Baslik.BackColor = Satis3Toplam.BackColor;

            deOdemeTarihi.Visible = false;
            simpleButton10.Visible = false;
            simpleButton10.Text = "Ödeme Zamanı...";
            switch (cbOdemeSekli.SelectedIndex)
            {
                case (int)Degerler.OdemeSekli.Nakit:
                    {
                        cbOdemeSekli.BackColor = System.Drawing.Color.White;
                        break;
                    }
                case (int)Degerler.OdemeSekli.KrediKarti:
                    {
                        lueKKarti.Visible = true;
                        lueKKarti.ItemIndex = 0;
                        cbOdemeSekli.BackColor = System.Drawing.Color.Yellow;
                        break;
                    }
                case (int)Degerler.OdemeSekli.AcikHesap:
                    {
                        if (DB.GetData("select count(*) as kampanya from SatisDetay with(nolock) where kampanyali is not null and fkSatislar=" + txtpkSatislar.Text).Rows[0][0].ToString() != "0")
                            NormalFiyatlariUygula();

                        cbOdemeSekli.BackColor = System.Drawing.Color.Red;

                        if (AcikSatisindex == 1) Satis1Baslik.BackColor = System.Drawing.Color.Red;
                        if (AcikSatisindex == 2) Satis2Baslik.BackColor = System.Drawing.Color.Red;
                        if (AcikSatisindex == 3) Satis3Baslik.BackColor = System.Drawing.Color.Red;
                        //deOdemeTarihi.Visible = true;
                        simpleButton10.Visible = true;
                        break;
                    }
                case (int)Degerler.OdemeSekli.Diger:
                    {

                        cbOdemeSekli.BackColor = System.Drawing.Color.White;
                        //KaydetYazdir(false);
                        break;

                    }
                case (int)Degerler.OdemeSekli.Banka:
                    {
                        lueKKarti.Visible = true;
                        lueKKarti.ItemIndex = 0;
                        cbOdemeSekli.BackColor = System.Drawing.Color.LightYellow;
                        break;
                    }
                case (int)Degerler.OdemeSekli.Sodexo:
                    {
                        lueKKarti.Visible = true;
                        cbOdemeSekli.BackColor = System.Drawing.Color.YellowGreen;
                        if (lueKKarti.EditValue == null)
                        {
                            object kk = lueKKarti.Properties.DataSource;
                            lueKKarti.Properties.DataSource = DB.GetData("Select * from Bankalar with(nolock) where Aktif=1 and KartTuru='Sodexo Ticket'");
                            lueKKarti.ItemIndex = 0;
                            lueKKarti.Properties.DataSource = kk;
                        }
                        break;
                    }
                default:
                    break;
            }

            yesilisikyeni();

        }

        private void cbOdemeSekli_SelectedIndexChanged(object sender, EventArgs e)
        {
            vSatisOdeme(sender, e);
        }
        private void cbOdemeSekli_Leave(object sender, EventArgs e)
        {
            cbOdemeSekli.TabStop = false;
        }

        private void cbOdemeSekli_Enter(object sender, EventArgs e)
        {
            cbOdemeSekli.TabStop = true;
        }

        private void odemeseklikkartiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli((int)Degerler.OdemeSekli.KrediKarti);
        }

        private void odemeseklidigerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli((int)Degerler.OdemeSekli.Diger);
        }

        private void odemesekliacikhesapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli((int)Degerler.OdemeSekli.AcikHesap);
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnyazdir_Click(sender, e);
        }

        private void gridView6_DoubleClick(object sender, EventArgs e)
        {
            if (gridView6.FocusedRowHandle < 0) return;
            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            //FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //if (gcMusteriSatis.Visible == true)
            //{

            //    if (e.FocusedRowHandle < 0) return;

            //    if (e.PrevFocusedRowHandle < 0) return;

            //    if (e.PrevFocusedRowHandle  == gridView1.DataRowCount)
            //    {
            //        gcMusteriSatis.DataSource = null;
            //        return;
            //    }
            //    MusteriSonSatislari(e.FocusedRowHandle);
            //}
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

        void SatisDetayGetir_Yeni2(string pkSatislar)
        {
            txtpkSatislar.Text = pkSatislar;

            gcSatisDetay.DataSource = DB.GetData("exec sp_SatisDetay " + pkSatislar + ",0");

            toplamlar();

            //gridView1.AddNewRow();
        }

        void SatisDetayGetir_Yeni(string pkSatislar)
        {
            txtpkSatislar.Text = pkSatislar;

            gcSatisDetay.DataSource = DB.GetData("exec sp_SatisDetay " + pkSatislar + ",0");

            toplamlar();

            gridView1.AddNewRow();
        }

        void SatisDetayToplamGetir(string pkSatislar)
        {
            txtpkSatislar.Text = pkSatislar;

            gcSatisDetay.DataSource = DB.GetData("exec sp_SatisDetay " + pkSatislar + ",0");

            toplamlar();
        }

        void yesilisikyeni()
        {
            string fkFirma = "0";
            if (AcikSatisindex == 1)
            {
                SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
                TutarFont(Satis1Toplam);
                fkFirma = Satis1Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 2)
            {
                SatisDetayGetir_Yeni(Satis2Toplam.Tag.ToString());
                TutarFont(Satis2Toplam);
                fkFirma = Satis2Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 3)
            {
                SatisDetayGetir_Yeni(Satis3Toplam.Tag.ToString());
                TutarFont(Satis3Toplam);
                fkFirma = Satis3Firma.Tag.ToString();
            }
            else if (AcikSatisindex == 4)
            {
                SatisDetayGetir_Yeni(Satis4Toplam.Tag.ToString());
                TutarFont(Satis4Toplam);
                fkFirma = Satis4Firma.Tag.ToString();
            }

            if (AcikSatisindex == 1) fontayarla(Satis1Toplam);
            if (AcikSatisindex == 2) fontayarla(Satis2Toplam);
            if (AcikSatisindex == 3) fontayarla(Satis3Toplam);
            if (AcikSatisindex == 4) fontayarla(Satis4Toplam);

            if (targetGrid != null)
                targetGrid.Visible = false;

            //repositoryItemButtonEdit1.Click += new EventHandler(repositoryItemButtonEdit1_MouseEnter);
            //repositoryItemButtonEdit1.CreateEditor();

            //this.InvokeGotFocus();
            //gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            //this.InitLayout();

            //gridView1.Focus();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            //gridView1.CloseEditor();

            ////gridView1.AddNewRow();
            ////gridView1.PostEditor();
            //gridView1.ShowEditor();

            //SendKeys.Send("{ENTER}");
            gcMusteriSatis.Visible = false;
            yesilyak();
        }

        void yesilyak()
        {
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();

            //gridView1.AddNewRow();
            //gridView1.PostEditor();
            //gridView1.ShowEditor();
            SendKeys.Send("{ENTER}");
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void teBakiyeDevir_EditValueChanged(object sender, EventArgs e)
        {
            decimal d = 0;
            decimal.TryParse(teBakiyeDevir.Text, out d);
            if (d == 0)
                teBakiyeDevir.BackColor = System.Drawing.Color.Transparent;
            else if (d > 0)
                teBakiyeDevir.BackColor = System.Drawing.Color.Red;
            else if (d < 0)
                teBakiyeDevir.BackColor = System.Drawing.Color.Blue;
        }

        private void repositoryItemComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void repositoryItemSpinEdit1_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
            gridView1.CloseEditor();
        }

        void disp(object sender, EventArgs e)
        {
            MessageBox.Show("It works!");
        }

        private void labelControl10_MouseClick(object sender, MouseEventArgs e)
        {
            DataTable dt = DB.GetData("select * from SatisDurumu with(nolock)");
            cMenuSatisTipi.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cMenuSatisTipi.Items.Add(dt.Rows[i]["Durumu"].ToString(), null, disp);
            }
        }

        private void gösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = (((System.Windows.Forms.ToolStripDropDown)(((System.Windows.Forms.ToolStripDropDownItem)(sender)).DropDown)).OwnerItem).Tag.ToString();
            DB.ExecuteSQL("update SatisDurumu set Aktif=1 where pkSatisDurumu=" + id);
        }

        private void gizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = (((System.Windows.Forms.ToolStripDropDown)(((System.Windows.Forms.ToolStripDropDownItem)(sender)).DropDown)).OwnerItem).Tag.ToString();
            DB.ExecuteSQL("update SatisDurumu set Aktif=0 where pkSatisDurumu=" + id);
        }

        private void txtFaturaNo_Leave(object sender, EventArgs e)
        {
            if (DB.GetData("Select count(*) From Satislar with(nolock) where pkSatislar<>"+ txtpkSatislar.Text
                + " and FaturaNo='" + txtFaturaNo.Text + "'").Rows[0][0].ToString() != "0")
            {
                //SonFaturaNo();
                MessageBox.Show("Fatura No Daha Önce Kullanıldı.Lütfen Başka Fatura No Giriniz");
            }
            //else
            DB.ExecuteSQL("UPDATE Satislar set FaturaNo='" + txtFaturaNo.Text + "' where pkSatislar=" + txtpkSatislar.Text);
        }

        private void txtFaturaNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void kaydetToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gcSatisDetay, "A4");
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://portal-test.uyumsoft.com.tr/");
            //SonFaturaNo();
        }

        private void ePostaGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string fkfirma = "0", FisNo = "0", fkSatisDurumu = "0", eposta = "@";

            if (gridView.FocusedRowHandle < 0) return;

            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);

            fkfirma = dr["fkFirma"].ToString();
            FisNo = dr["FisNo"].ToString();
            fkSatisDurumu = dr["Durumu"].ToString();
            if (fkSatisDurumu == "Teklif") fkSatisDurumu = "1";
            DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkfirma);
            //DataTable dtFirma = DB.GetData("select * From Firmalar with(nolock) where pkFirma=" + fkfirma);
            eposta = Musteri.Rows[0]["eposta"].ToString();


            inputForm sifregir = new inputForm();
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir.GirilenCaption.Text = "E-Posta Adresi Giriniz";
            sifregir.Girilen.Text = eposta;

            sifregir.ShowDialog();
            eposta = sifregir.Girilen.Text;

            if (eposta.Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(eposta + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;


            #region  yazıcı Seçimi
            string YaziciAdi = "", YaziciDosyasi = "";

            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" + fkSatisDurumu); //+ lueSatisTipi.EditValue.ToString());

            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1, int.Parse(lueSatisTipi.EditValue.ToString()));

                YaziciAyarlari.ShowDialog();

                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }
            #endregion 

            if (YaziciAdi == "")
            {
                MessageBox.Show("Yazıcı Bulunamadı");
                return;
            }
            // else
            //FisYazdir(dizayner, pkSatisBarkod.Text, YaziciDosyasi, YaziciAdi);

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\" + YaziciDosyasi + ".repx");
            rapor.Name = "Teklif";
            rapor.Report.Name = "Teklif.repx";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + FisNo + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + FisNo);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli from KasaHareket with(nolock) where fkSatislar=" + FisNo);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                //DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);


                string dosyaadi = Application.StartupPath + "\\" + YaziciDosyasi + ".pdf";

                rapor.DataSource = ds;
                //rapor.DataSource = gridControl2.DataSource;
                //rapor.FilterString = "[ID]=1";
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(eposta, "Teklif Listesi", dosyaadi, "Teklif Listesi");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void teSeriNo_Leave(object sender, EventArgs e)
        {
            DB.ExecuteSQL("update Kullanicilar set FaturaSeriNo='" + teSeriNo.Text + "' where pkKullanicilar=" + DB.fkKullanicilar);
        }

        private void lueKKarti_Leave(object sender, EventArgs e)
        {
            if (lueKKarti.Tag.ToString() == "1" && lueKKarti.EditValue != null)
                DB.ExecuteSQL("update Satislar set fkBankalar=" +
                lueKKarti.EditValue.ToString() + " where pkSatislar=" + txtpkSatislar.Text);
        }

        private void lueKKarti_Enter(object sender, EventArgs e)
        {
            lueKKarti.Tag = "1";
        }

        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            //if (((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;
            //string iskontotutar =
            //   ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
            //iskonto tutar
            string iskontotutar =
            ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();

            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkSatisDetay = dr["pkSatisDetay"].ToString();

            string satisfiyati = "0";
            // if (dr["KdvDahil"].ToString() == "True")
            satisfiyati = dr["SatisFiyati"].ToString();
            //else
            //  satisfiyati = dr["SatisFiyatiKdvHaric"].ToString();
            decimal _satisfiyati = 0;
            decimal.TryParse(satisfiyati, out _satisfiyati);
            decimal isyuzde = 0, istut = 0;
            decimal.TryParse(iskontotutar, out istut);
            isyuzde = (istut * 100) / _satisfiyati;
            //isyuzde=decimal.Round(isyuzde, 6);
            int sonuc =
            DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set iskontoyuzdetutar=" + isyuzde.ToString().Replace(",", ".") +
                ",iskontotutar=" + iskontotutar.Replace(",", ".") +
                " where pkSatisDetay=" + pkSatisDetay);

            if (sonuc == -1)
                MessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz");

            //if (repositoryItemCalcEdit1.AccessibleDescription == "")
            //{
            int i = gridView1.FocusedRowHandle;

            SatisDetayYenile();

            gridView1.FocusedRowHandle = i;
            //}
            //else if (repositoryItemCalcEdit1.AccessibleDescription == "Enter")
            //  yesilisikyeni();
        }
        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string iskontotutar =
                  ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();

                if (gridView1.FocusedRowHandle < 0)
                {
                    yesilisikyeni();
                    return;
                }
                //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                //string pkSatisDetay = dr["pkSatisDetay"].ToString();
                //repositoryItemCalcEdit1.AccessibleDescription = e.KeyCode.ToString();
                yesilisikyeni();
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                int i = gridView1.FocusedRowHandle;

                SatisDetayToplamGetir(txtpkSatislar.Text);

                gridView1.FocusedRowHandle = i;
            }
        }
        private void repositoryItemCalcEdit1_EditValueChanged(object sender, EventArgs e)
        {
            string iskontotutar =
            ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();

            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkSatisDetay = dr["pkSatisDetay"].ToString();

            string satisfiyati = "0";

            if (dr["isKdvHaric"].ToString() == "True")
                satisfiyati = dr["SatisFiyatiKdvHaric"].ToString();
            else
                satisfiyati = dr["SatisFiyati"].ToString();
            decimal _satisfiyati = 0;
            decimal.TryParse(satisfiyati, out _satisfiyati);
            decimal isyuzde = 0, istut = 0;
            decimal.TryParse(iskontotutar, out istut);
            isyuzde = (istut * 100) / _satisfiyati;
            isyuzde = decimal.Round(isyuzde, 6);
            DB.ExecuteSQL("update SatisDetay set iskontoyuzdetutar=" + isyuzde.ToString().Replace(",", ".") +
                ",iskontotutar=" + iskontotutar.Replace(",", ".") +
                " where pkSatisDetay=" + pkSatisDetay);

        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            int adet = 1;
            int.TryParse(dr["Adet"].ToString(),out adet);
            formislemleri.EtiketBas(dr["fkStokKarti"].ToString(), adet);
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

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            BarkodYazdir(false);
        }

        void BarkodYazdir(bool dizayn)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti = dr["fkStokKarti"].ToString();

            //DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            xrCariHareket Barkod = new xrCariHareket();

            //for (int i = 0; i < 1; i++)
            //{
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "";
            //xrBarkod Barkod = new xrBarkod();
            System.Data.DataSet ds = new DataSet("Test");
            DataTable FisDetay = DB.GetData(@"select pkStokKarti as fkStokKarti,
                1 as Adet,
                case when sk.KutuFiyat=0 then
                sfn.SatisFiyatiKdvli
                else (sfn.SatisFiyatiKdvli/sk.KutuFiyat) end  koliicitanefiyati,
                sk.Stokadi,
                Barcode,
                UreticiKodu,
                sfn.SatisFiyatiKdvli as SatisFiyati,
                sfk.SatisFiyatiKdvli as SatisFiyati2,
                sfk3.SatisFiyatiKdvli as SatisFiyati3,
                m.Marka,bg.Aciklama as Beden,
                r.Aciklama as Renk,'' as TedarikciAdi,
                0 as GizliFiyat,'' as EtiketAciklama 
                
                from StokKarti sk with(nolock)
                left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka
                left join BedenGrupKodu bg with(nolock) on bg.pkBedenGrupKodu=sk.fkBedenGrupKodu
                left join RenkGrupKodu r with(nolock) on r.pkRenkGrupKodu=sk.fkRenkGrupKodu
                
                left join (select sf.* from SatisFiyatlari sf with(nolock) 
                left join SatisFiyatlariBaslik sfb with(nolock) on sfb.pkSatisFiyatlariBaslik=sf.fkSatisFiyatlariBaslik
                where sfb.Tur=1) sfn on sfn.fkStokKarti=sk.pkStokKarti
                
                left join (select sf.* from SatisFiyatlari sf with(nolock) 
                left join SatisFiyatlariBaslik sfb with(nolock) on sfb.pkSatisFiyatlariBaslik=sf.fkSatisFiyatlariBaslik
                where sfb.Tur=2) sfk on sfk.fkStokKarti=sk.pkStokKarti
                
                left join (select sf.* from SatisFiyatlari sf with(nolock) 
                left join SatisFiyatlariBaslik sfb with(nolock) on sfb.pkSatisFiyatlariBaslik=sf.fkSatisFiyatlariBaslik
                where sfb.Tur=3) sfk3 on sfk3.fkStokKarti=sk.pkStokKarti

                 where pkStokKarti=" + fkStokKarti);
            FisDetay.TableName = "FisDetay";
            ds.Tables.Add(FisDetay);

            DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");// + fisid);
            Sirket.TableName = "Sirket";
            ds.Tables.Add(Sirket);
            Barkod.DataSource = ds;
            DataTable dt = DB.GetData("SELECT * FROM EtiketSablonlari  with(nolock) where Varsayilan=1");
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Varsayılan Seçilmemiş");
                return;
            }
            try
            {
                RaporDosyasi = exedizini + "\\Raporlar\\" + dt.Rows[0]["DosyaYolu"].ToString() + ".repx";
                //rapor.DataSource = gCPerHareketleri.DataSource;
                //rapor.CreateDocument();
                Barkod.Name = "Barkod";
                Barkod.DisplayName = "Barkod";
                Barkod.LoadLayout(RaporDosyasi);
                //Barkod.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);
                if (dizayn)
                    Barkod.ShowDesigner();
                else
                {
                    if (dt.Rows[0]["YaziciAdi"].ToString() == "")
                        MessageBox.Show("Yazıcı Adı Seçiniz");
                    else
                        Barkod.Print(dt.Rows[0]["YaziciAdi"].ToString());
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Yazdırırken Hata Oluştur:" + exp.Message);
            }
            //}
        }

        private void btnFisKopyala_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
            {
                formislemleri.Mesajform("Önce Ekrandaki Satışı Kaydediniz", "K",200);
                return;
            }

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
            //MessageBox.Show(pkFirma);
            DataTable dt = DB.GetData("select MAX(pkSatislar) as pkSatislar from Satislar with(nolock) where Siparis=1 and fkfirma=" + pkFirma);
            if (dt.Rows.Count == 0)
            {
                formislemleri.Mesajform("Daha Önce Satış Yapılmamış", "K", 200);
                return;
            }
            string pkSatislar_sablon = dt.Rows[0]["pkSatislar"].ToString();
            dt = DB.GetData("select * from Satislar with(nolock) where pkSatislar=" + pkSatislar_sablon);
            string Aciklama = dt.Rows[0]["Aciklama"].ToString();

            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Siparis", "0"));
            list.Add(new SqlParameter("@fkKullanici", lueKullanicilar.EditValue.ToString()));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", Aciklama));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", "0"));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

            if (cbOdemeSekli.SelectedIndex == -1)
            {
                cbOdemeSekli.SelectedIndex = 0;
                cbOdemeSekli.Text = Degerler.odemesekli;
            }
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.Text));

            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            string sf = Degerler.fkSatisFiyatlariBaslik.ToString();
            if (lueFiyatlar.EditValue != null && lueFiyatlar.EditValue.ToString() != "")
                sf = lueFiyatlar.EditValue.ToString();
            list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", sf));

            sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen,OdemeSekli,SonislemTarihi,BilgisayarAdi,fkSatisFiyatlariBaslik,fkSube)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar,0,0,@OdemeSekli,getdate(),@BilgisayarAdi,@fkSatisFiyatlariBaslik,@fkSube) SELECT IDENT_CURRENT('Satislar')";
            fisno = DB.ExecuteScalarSQL(sql, list);

            if (fisno.Substring(0, 1) == "H")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }

            if (AcikSatisindex == 1)
                Satis1Toplam.Tag = fisno;
            if (AcikSatisindex == 2)
                Satis2Toplam.Tag = fisno;
            if (AcikSatisindex == 3)
                Satis3Toplam.Tag = fisno;
            if (AcikSatisindex == 4)
                Satis4Toplam.Tag = fisno;

            //detay ekle
            DataTable dtSatisDetay = DB.GetData(@"select pkStokKarti,sd.Adet,sk.AlisFiyati,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzdetutar,
sd.KdvOrani, sd.NakitFiyat, sd.GercekAdet, sd.Birimi, sd.SatisFiyatiKdvHaric, sd.isKdvHaric,
sd.SiraNo, sd.fkDepolar, isnull(sd.fkiskontolar, 0) as fkiskontolar, sd.fkParaBirimi, kur_fiyati
 from SatisDetay sd with(nolock)
inner join StokKarti sk with(nolock) on sk.pkStokKarti = sd.fkStokKarti
where fkSatislar = " + pkSatislar_sablon);

            for (int i = 0; i < dtSatisDetay.Rows.Count; i++)
            {
                //string fkSatislar = dtSatisDetay.Rows[i]["fkSatislar"].ToString();
                string fkStokKarti = dtSatisDetay.Rows[i]["pkStokKarti"].ToString();
                string Adet = dtSatisDetay.Rows[i]["Adet"].ToString();
                string AlisFiyati = dtSatisDetay.Rows[i]["AlisFiyati"].ToString();
                string SatisFiyati = dtSatisDetay.Rows[i]["SatisFiyati"].ToString();
                string iskontotutar = dtSatisDetay.Rows[i]["iskontotutar"].ToString();
                string iskontoyuzdetutar = dtSatisDetay.Rows[i]["iskontoyuzdetutar"].ToString();
                string KdvOrani = dtSatisDetay.Rows[i]["KdvOrani"].ToString();
                string NakitFiyat = dtSatisDetay.Rows[i]["NakitFiyat"].ToString();
                string GercekAdet = dtSatisDetay.Rows[i]["GercekAdet"].ToString();
                string Birimi = dtSatisDetay.Rows[i]["Birimi"].ToString();
                if (Birimi == "") Birimi = "Adet";
                string SatisFiyatiKdvHaric = dtSatisDetay.Rows[i]["SatisFiyatiKdvHaric"].ToString();
                if (SatisFiyatiKdvHaric == "") SatisFiyatiKdvHaric = SatisFiyati;

                string isKdvHaric = dtSatisDetay.Rows[i]["isKdvHaric"].ToString();
                if (isKdvHaric == "False") isKdvHaric = "0"; else isKdvHaric = "1";
                string SiraNo = dtSatisDetay.Rows[i]["SiraNo"].ToString();
                if (SiraNo == "") SiraNo = "0";
                string fkDepolar = dtSatisDetay.Rows[i]["fkDepolar"].ToString();
                string fkParaBirimi = dtSatisDetay.Rows[i]["fkParaBirimi"].ToString();
                

                sql = @"INSERT INTO SatisDetay (fkSatislar,fkStokKarti, Adet, AlisFiyati, SatisFiyati, Tarih, Stogaisle, iskontotutar, iskontoyuzdetutar,
                      KdvOrani, NakitFiyat,  iade, fkiskontolar, GercekAdet, fkParaBirimi,
                      Birimi, SatisFiyatiKdvHaric, isKdvHaric, Faturaiskonto, SiraNo,fkDepolar) 
                      values(@fkSatislar,@fkStokKarti,@Adet,@AlisFiyati,@Satis_Fiyati,getdate(),0,@iskontotutar,@iskontoyuzdetutar,@KdvOrani,@NakitFiyat,0,0,@GercekAdet,
                      1,'@Birimi',@SatisFiyatiKdvHaric,@isKdvHaric,0,@SiraNo,@fkDepolar)";

                sql = sql.Replace("@fkSatislar", fisno);
                sql = sql.Replace("@fkStokKarti", fkStokKarti);
                sql = sql.Replace("@Adet", Adet.Replace(",", "."));
                sql = sql.Replace("@AlisFiyati", AlisFiyati.Replace(",", "."));
                sql = sql.Replace("@Satis_Fiyati", SatisFiyati.Replace(",", "."));
                sql = sql.Replace("@iskontotutar", iskontotutar.Replace(",", "."));
                sql = sql.Replace("@iskontoyuzdetutar", iskontoyuzdetutar.Replace(",", "."));
                sql = sql.Replace("@KdvOrani", KdvOrani.Replace(",", "."));
                sql = sql.Replace("@NakitFiyat", NakitFiyat.Replace(",", "."));
                sql = sql.Replace("@GercekAdet", GercekAdet.Replace(",", "."));
                sql = sql.Replace("@Birimi", Birimi);
                sql = sql.Replace("@SatisFiyatiKdvHaric", SatisFiyatiKdvHaric.Replace(",", "."));
                sql = sql.Replace("@isKdvHaric", isKdvHaric);
                sql = sql.Replace("@SiraNo", SiraNo);
                sql = sql.Replace("@fkDepolar", fkDepolar);
                sql = sql.Replace("@fkParaBirimi", fkParaBirimi);

                DB.ExecuteSQL(sql);
            }


            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
            }

            deGuncellemeTarihi.Enabled = true;

            yesilisikyeni();
        }

        private void tSMoncekisatisikopyala_Click(object sender, EventArgs e)
        {
            if (btnFisKopyala.Visible == true)
            {
                btnFisKopyala_Click(sender, e);
            }
        }

        void NormalFiyatlariUygula()
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }

            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Normal Fiyatlar Uygulanacak Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.No)
            //{
            //    yesilisikyeni();
            //    return;
            //}

            DataTable dt = DB.GetData("select * from SatisDetay with(nolock) where fkSatislar=" + txtpkSatislar.Text);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pkSatisDetay = dt.Rows[i]["pkSatisDetay"].ToString();
                string fkStokKarti = dt.Rows[i]["fkStokKarti"].ToString();

                DataTable dtFiyat = DB.GetData(@"select SatisFiyatiKdvli from SatisFiyatlari f with(nolock) 
                left join SatisFiyatlariBaslik b on b.pkSatisFiyatlariBaslik=f.fkSatisFiyatlariBaslik
                where b.Tur=1 and fkStokKarti=" + fkStokKarti);
                if (dtFiyat.Rows.Count > 0)
                {
                    string SatisFiyatiKdvli = dtFiyat.Rows[0]["SatisFiyatiKdvli"].ToString();

                    DB.ExecuteSQL("update SatisDetay set Kampanyali=null,SatisFiyati=" + SatisFiyatiKdvli.Replace(",", ".") +
                        ",NakitFiyat=" + SatisFiyatiKdvli.Replace(",", ".") + "  where pkSatisDetay=" + pkSatisDetay);
                }
            }
            yesilisikyeni();
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkedit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        protected void DrawCheckBox(Graphics g, Rectangle r, bool Checked)
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;
            info = chkedit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = chkedit.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = Checked;

            info.Bounds = r;
            info.PaintAppearance.ForeColor = Color.Black;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();
        }

        private void cbTevkifat_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbTevkifat.SelectedIndex)
            {
                case 0:
                    {
                        DB.ExecuteSQL("update SatisDetay set TevkifatOrani=1,TevkifatTutari=0 where fkSatislar=" + txtpkSatislar.Text);
                        break;
                    }
                case 1:
                    {
                        DB.ExecuteSQL("update SatisDetay set TevkifatOrani=2 where fkSatislar=" + txtpkSatislar.Text);
                        break;
                    }
                case 2:
                    {
                        DB.ExecuteSQL("update SatisDetay set TevkifatOrani=5 where fkSatislar=" + txtpkSatislar.Text);
                        break;
                    }
                case 3:
                    {
                        DB.ExecuteSQL("update SatisDetay set TevkifatOrani=7 where fkSatislar=" + txtpkSatislar.Text);
                        break;
                    }
                case 4:
                    {
                        DB.ExecuteSQL("update SatisDetay set TevkifatOrani=9 where fkSatislar=" + txtpkSatislar.Text);
                        break;
                    }
            }

            if (cbTevkifat.SelectedIndex > 0)
            {
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    DataRow dr = gridView1.GetDataRow(i);
                    string pkSatisDetay = dr["pkSatisDetay"].ToString();
                    //kdv hariç hesapla
                    DB.ExecuteSQL(@"update SatisDetay set 
                    TevkifatTutari=(SatisFiyati-((((SatisFiyati*KdvOrani)/(100+KdvOrani))*TevkifatOrani)/10)) 
                    where pkSatisDetay=" + pkSatisDetay);

                    DB.ExecuteSQL(@"update SatisDetay set TevkifatTutari= (SatisFiyati-((SatisFiyati*iskontoyuzdetutar)/100)-isnull(Faturaiskonto,0))-TevkifatTutari
                    where pkSatisDetay=" + pkSatisDetay);


                }
            }

            yesilisikyeni();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
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

        private void repositoryItemLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.ExecuteSQL("update SatisDetay set fkDepolar=" + value + " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
        }

        private void deTeslimTarihi_EditValueChanged(object sender, EventArgs e)
        {
            if (deTeslimTarihi.EditValue != null)
                DB.ExecuteSQL("update Satislar set TeslimTarihi='" + deTeslimTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") +
                "' where pkSatislar=" + txtpkSatislar.Text);
        }

        private void lbSatisTarihi_Click(object sender, EventArgs e)
        {
            deGuncellemeTarihi.DateTime = DateTime.Now;
            DB.ExecuteSQL("update Satislar set guncellemetarihi='" + deGuncellemeTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:ss") +
                "' where pkSatislar=" + txtpkSatislar.Text);
        }

        private void sbYenile_Click(object sender, EventArgs e)
        {
            Yenile();
        }

        private void aratoplam_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                yesilisikyeni();
                return;
            }


            //decimal.TryParse(aratoplam.Tag.ToString(), out ilkara);
            if (Keys.Enter == e.KeyCode)
            {
                decimal ToplamTutar = 0;
                decimal aratop = aratoplam.Value;
                DataTable dtTutar = DB.GetData(" select sum(NakitFiyat*Adet) as ToplamTutar from SatisDetay with(nolock) where fkSatislar=" + txtpkSatislar.Text);
                if (dtTutar.Rows.Count == 0) return;
                decimal.TryParse(dtTutar.Rows[0]["ToplamTutar"].ToString(), out ToplamTutar);

                // if (aratop != ilkara)
                // {
                decimal fark = ToplamTutar - aratop;
                decimal farkyuzde = (fark * 100) / ToplamTutar;
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    DataRow dr = gridView1.GetDataRow(i);
                    decimal NakitFiyat = decimal.Parse(dr["NakitFiyati"].ToString());
                    decimal iskontotutar = (NakitFiyat * farkyuzde) / 100;
                    //DB.ExecuteSQL("update SatisDetay set Faturaiskonto=" + araiskontorow.ToString().Replace(",",".")+
                    DB.ExecuteSQL("update SatisDetay set iskontotutar=" + iskontotutar.ToString().Replace(",", ".") +
                    " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());

                }

                DB.ExecuteSQL("update SatisDetay set iskontoyuzdetutar=(iskontotutar*100)/NakitFiyat where fkSatislar=" + txtpkSatislar.Text);
                // }

                yesilisikyeni();
            }
        }

        private void aratoplam_Enter(object sender, EventArgs e)
        {
            aratoplam.Tag = aratoplam.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;

            //int secilen = gridView1.FocusedRowHandle;
            //DevExpress.XtraGrid.Columns.GridColumn secilenGridColumn = gridView1.FocusedColumn;
            //DataRow dr = gridView1.GetDataRow(secilen);

            repositoryItemButtonEdit2.Tag = "0";

            frmiskontoToplam iskonto = new frmiskontoToplam(int.Parse(txtpkSatislar.Text), 0);
            //iskonto çıkışda kaydetmesini engellemek için
            //gridView1.Focus();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            iskonto.fkAlisDetay.Text = "";
            iskonto.fkSatisDetay.Text = "";
            //if (cbFaturaTipi.SelectedIndex == 0)
            iskonto.ceBirimFiyati.Value = aratoplam.Value;//decimal.Parse(dr["SatisFiyati"].ToString());
            //else
            //    iskonto.ceBirimFiyati.Value = decimal.Parse(dr["SatisFiyatiKdvHaric"].ToString());
            iskonto.ShowDialog();

            //repositoryItemButtonEdit2.Tag = "0";
            //((DevExpress.XtraEditors.ButtonEdit)((((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue =
            //   iskonto.iskontoorani.Value;

            yesilisikyeni();
            //DB.ExecuteSQL("update Satisdetay set Faturaiskonto=0 where fkSatislar=" + txtpkSatislar.Text);
            //yesilisikyeni();
        }

        private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "AlisFiyati")
            {
                string va = "";
                //int i = 0;
                if (e.Value != null)
                {
                    DataRow dr = gridView1.GetDataRow(e.RowHandle);
                    string pkSatisDetay = dr["pkSatisDetay"].ToString();
                    va = e.Value.ToString();
                    DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set AlisFiyati=" + va.Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
                }
                //if (e.RowHandle > 0)
                //   i = e.RowHandle;
                //gridView1.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridView1_CellValueChanged);

                //gridView1.SetRowCellValue(e.RowHandle, e.Column, (int)(e.Value) + 10);

                //gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridView1_CellValueChanged);
            }

        }

        void RandevuVer()
        {
            frmRandevuVer RandevuVer = new frmRandevuVer();
            RandevuVer.pkSatislar.Text = txtpkSatislar.Text;
            RandevuVer.Show();
        }
        private void randevuVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandevuVer();
        }

        private void ucSatis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
                RandevuVer();
            else if (e.Control && (e.KeyValue == 84 || e.KeyValue == 116))//T veya t
            {
                frmFisAciklama fFisAciklama = new frmFisAciklama();
                fFisAciklama.panelControl2.Visible = false;
                fFisAciklama.btnCancel.Visible = true;
                fFisAciklama.btnTemizle.Visible = false;
                fFisAciklama.Text = "Tutar Hesapla";
                fFisAciklama.pcTutarHesapla.Visible = true;
                fFisAciklama.pcTutarHesapla.BringToFront();

                //DataRow dr = gridView1.GetDataRow(ghi.RowHandle);
                //decimal tutar = decimal.Parse(dr["Tutar"].ToString());
                //decimal nakitfiyati = decimal.Parse(dr["NakitFiyati"].ToString());
                //decimal iskontotutar, iskonyuzdetotutar = 0;
                //decimal adet = decimal.Parse(dr["Adet"].ToString());
                //decimal kdvorani = decimal.Parse(dr["KdvOrani"].ToString());

                //fFisAciklama.seMiktar.Value = adet;
                //fFisAciklama.ceTutari.Value = tutar;

                fFisAciklama.ShowDialog();

                yesilisikyeni();
            }

        }

        private void repositoryItemCalcEdit4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                //repositoryItemButtonEdit2_ButtonClick(sender, null);
            }
        }

        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("a");
            //yesilisikyeni();
        }

        private void repositoryItemLookUpEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
                yesilisikyeni();
        }

        private void cbGecmisFisler_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void lueKullanicilar_Enter(object sender, EventArgs e)
        {
            lueKullanicilar.Tag = "1";
        }

        private void lueKullanicilar_Leave(object sender, EventArgs e)
        {
            lueKullanicilar.Tag = "0";
        }

        private void deFaturaTarihi_Leave(object sender, EventArgs e)
        {
            deGuncellemeTarihi.DateTime = deFaturaTarihi.DateTime;
            DB.ExecuteSQL("UPDATE Satislar set FaturaTarihi='" + deFaturaTarihi.DateTime.ToString("yyyy-MM-dd HH:mm") + "' where pkSatislar=" + txtpkSatislar.Text);
            //deFaturaTarihi.
        }

        private void btnSatisMusteriEkrani_Click(object sender, EventArgs e)
        {
            Screen[] ekran = System.Windows.Forms.Screen.AllScreens;
            //ekran[0].Primary.ToString();
            if (Screen.AllScreens.Length > 1)
            {
                frmSatisMusteriEkrani ikinciekran = new frmSatisMusteriEkrani(txtpkSatislar.Text);
                ikinciekran.StartPosition = FormStartPosition.Manual;
                // İkinci Monitörü tanımla
                Screen screen = GetSecondaryScreen();
                // İkinci formun location tanımla
                ikinciekran.Location = screen.WorkingArea.Location;
                // fullscreen yap
                ikinciekran.Size = new Size(screen.WorkingArea.Width, screen.WorkingArea.Height);
                //f2.SetDesktopLocation.ShowFocusCues();
                ikinciekran.Show();
            }
            else
            {
                MessageBox.Show("ikinci Ekran Bulunamadı");
                frmSatisMusteriEkrani ikinciekran = new frmSatisMusteriEkrani(txtpkSatislar.Text);
                ikinciekran.Show();
            }
        }
        public Screen GetSecondaryScreen()
        {
            if (Screen.AllScreens.Length == 1)
            {
                return null;
            }
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Primary == false)
                {
                    return screen;
                }
            }
            return null;
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            frmTarihSec tarsec = new frmTarihSec();
            tarsec.ShowDialog();
            if (tarsec.Tag.ToString() == "1")
            {
                string zaman = tarsec.dateNavigator1.DateTime.ToString("dd.MM.yyyy") + " " +
                    tarsec.dtpSaat.Value.ToString("HH:mm:ss");
                deOdemeTarihi.DateTime = Convert.ToDateTime(zaman);
                simpleButton10.ToolTip = zaman;
                simpleButton10.Text = zaman;
            }
            else
                deOdemeTarihi.EditValue = null;

            tarsec.Dispose();
        }

        private void repositoryItemCalcEdit5_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
            gridView1.CloseEditor();
        }

        private void repositoryItemCalcEdit5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string girilen =
                   ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

                    if (gridView1.FocusedRowHandle < 0) return;

                    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    if (dr == null) return;

                    string pkSatisDetay = dr["pkSatisDetay"].ToString();
                    decimal KdvOrani = decimal.Parse(dr["KdvOrani"].ToString());
                    decimal NakitFiyat = decimal.Parse(dr["NakitFiyati"].ToString());
                    decimal SatisFiyatiKdvHaric = decimal.Parse(dr["SatisFiyatiKdvHaric"].ToString());
                    decimal YeniSatisFiyatiKdvHaric = decimal.Parse(girilen);
                    decimal KdvFiyat = 0;
                    decimal SatisFiyatiKdvDahil = 0;
                    decimal isk = 0, isk_yuzde = 0;
                    if (NakitFiyat == 0)
                    {
                        isk = 0;
                        isk_yuzde = 0;
                        NakitFiyat= YeniSatisFiyatiKdvHaric+((YeniSatisFiyatiKdvHaric*KdvOrani)/100);
                        
                        string sql = "UPDATE SatisDetay SET iskontotutar=0,iskontoyuzdetutar=0,NakitFiyat=" + NakitFiyat.ToString().Replace(",", ".") +
                        ",SatisFiyati=" + NakitFiyat.ToString().Replace(",", ".") +
                        ",SatisFiyatiKdvHaric="+ YeniSatisFiyatiKdvHaric.ToString().Replace(",", ".") +
                        " where pkSatisDetay=" + pkSatisDetay;

                        int sonuc = DB.ExecuteSQL_Sonuc_Sifir(sql);

                        if (sonuc != 0)
                            MessageBox.Show("Hata Oluştu Kontrol Ediniz");

                    }
                    else
                    {
                        KdvFiyat = (YeniSatisFiyatiKdvHaric * KdvOrani) / 100;
                        SatisFiyatiKdvDahil = KdvFiyat + YeniSatisFiyatiKdvHaric;
                        isk = NakitFiyat - SatisFiyatiKdvDahil;
                        isk_yuzde = ((isk * 100) / NakitFiyat);

                        string sql = "UPDATE SatisDetay SET iskontotutar=" + isk.ToString().Replace(",", ".") +
                        ",iskontoyuzdetutar=" + isk_yuzde.ToString().Replace(",", ".") +
                        " where pkSatisDetay=" + pkSatisDetay;

                        int sonuc = DB.ExecuteSQL_Sonuc_Sifir(sql);

                        if (sonuc != 0)
                            MessageBox.Show("Hata Oluştu Kontrol Ediniz");

                    }
                    yesilisikyeni();
                }
                catch (Exception exp)
                {
                    formislemleri.Mesajform(exp.Message, "K", 150);
                    yesilisikyeni();
                    //throw;
                }
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                int i = gridView1.FocusedRowHandle;

                try
                {
                    string girilen =
                   ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();

                    if (gridView1.FocusedRowHandle < 0) return;

                    DataRow dr = gridView1.GetDataRow(i);

                    if (dr == null) return;

                    string pkSatisDetay = dr["pkSatisDetay"].ToString();
                    decimal KdvOrani = decimal.Parse(dr["KdvOrani"].ToString());
                    decimal NakitFiyat = decimal.Parse(dr["NakitFiyati"].ToString());
                    decimal SatisFiyatiKdvHaric = decimal.Parse(dr["SatisFiyatiKdvHaric"].ToString());
                    decimal YeniSatisFiyatiKdvHaric = decimal.Parse(girilen);

                    decimal KdvFiyat = (YeniSatisFiyatiKdvHaric * KdvOrani) / 100;
                    decimal SatisFiyatiKdvDahil = KdvFiyat + YeniSatisFiyatiKdvHaric;

                    decimal isk = 0, isk_yuzde = 0;

                    isk = NakitFiyat - SatisFiyatiKdvDahil;

                    //if (isk < 0)
                    //    isk2 = isk * -1;
                    //else
                    //    isk2 = isk;

                    //isk_yuzde = 100 - (((NakitFiyat + isk) * 100) / NakitFiyat);
                    //iskonyuzdetotutar = (iskontotutar * 100) / nakitfiyati;
                    isk_yuzde = ((isk * 100) / NakitFiyat);
                    //100-(((NakitFiyat-" + isk.ToString().Replace(",", ".") + ")*100)/NakitFiyat)
                    string sql = "UPDATE SatisDetay SET iskontotutar=" + isk.ToString().Replace(",", ".") +
                    ",iskontoyuzdetutar=" + isk_yuzde.ToString().Replace(",", ".") +
                    " where pkSatisDetay=" + pkSatisDetay;

                    //if (isk < 0)
                    //    sql = "UPDATE SatisDetay SET iskontotutar=" + isk.ToString().Replace(",", ".") + 
                    //    ",iskontoyuzdetutar=" + isk_yuzde.ToString().Replace(",", ".")+
                    //    " where pkSatisDetay=" + pkSatisDetay;
                    //    //"iskontoyuzdetutar=100-(((NakitFiyat+" + isk2.ToString().Replace(",", ".") + ")*100)/NakitFiyat)" +
                    int sonuc = DB.ExecuteSQL_Sonuc_Sifir(sql);

                    if (sonuc != 0)
                        MessageBox.Show("Hata Oluştu Kontrol Ediniz");

                    //yesilisikyeni();
                }
                catch (Exception exp)
                {
                    formislemleri.Mesajform(exp.Message, "K", 150);
                    yesilisikyeni();
                    //throw;
                }

                SatisDetayYenile();

                gridView1.FocusedRowHandle = i;
            }

        }

        private void müşteriKartıToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            fkfirma = dr["fkFirma"].ToString();
            frmMusteriKarti frmMusteriKarti = new frmMusteriKarti(fkfirma, "");
            frmMusteriKarti.ShowDialog();
        }

        private void defaultfontkucuk_Click(object sender, EventArgs e)
        {

        }

        private void ucSatis_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                yesilyak();
        }

        private void kaydetToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\SonSatislarGrid.xml";
            gridView.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\SonSatislarGrid.xml";
            if (File.Exists(Dosya))
                File.Delete(Dosya);
        }

        private void lbFaturaTevkifatTarihi_Click(object sender, EventArgs e)
        {
            deFaturaTarihi.DateTime = DateTime.Now;
            //deGuncellemeTarihi.DateTime = deFaturaTarihi.DateTime;
            deFaturaTarihi.Focus();
            yesilyak();
        }

        private void btnKdvHaricYap_Click(object sender, EventArgs e)
        {
           
        }

        private void btnKYazdir_Click(object sender, EventArgs e)
        {
            btnYazdirKaydetmi(false);
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
            {
                formislemleri.Mesajform("Önce Ekrandaki Satışı Kaydediniz", "K", 200);
                return;
            }

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
            
            DataTable dt = DB.GetData("select MAX(pkSatislar) as pkSatislar from Satislar with(nolock) where Siparis=1 and fkKullanici=" + DB.fkKullanicilar);
            if (dt.Rows.Count == 0)
            {
                formislemleri.Mesajform("Daha Önce Satış Yapılmamış", "K", 200);
                return;
            }
            string pkSatislar_sablon = dt.Rows[0]["pkSatislar"].ToString();
            dt = DB.GetData("select * from Satislar with(nolock) where pkSatislar=" + pkSatislar_sablon);
            string Aciklama = dt.Rows[0]["Aciklama"].ToString();

            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Siparis", "0"));
            list.Add(new SqlParameter("@fkKullanici", lueKullanicilar.EditValue.ToString()));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", Aciklama));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", "0"));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

            if (cbOdemeSekli.SelectedIndex == -1)
            {
                cbOdemeSekli.SelectedIndex = 0;
                cbOdemeSekli.Text = Degerler.odemesekli;
            }
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.Text));

            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            string sf = "1";
            if (lueFiyatlar.EditValue != null && lueFiyatlar.EditValue.ToString() != "")
                sf = lueFiyatlar.EditValue.ToString();
            list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", sf));

            sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen,OdemeSekli,SonislemTarihi,BilgisayarAdi,fkSatisFiyatlariBaslik,fkSube)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar,0,0,@OdemeSekli,getdate(),@BilgisayarAdi,@fkSatisFiyatlariBaslik,@fkSube) SELECT IDENT_CURRENT('Satislar')";
            fisno = DB.ExecuteScalarSQL(sql, list);

            if (fisno.Substring(0, 1) == "H")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }

            if (AcikSatisindex == 1)
                Satis1Toplam.Tag = fisno;
            if (AcikSatisindex == 2)
                Satis2Toplam.Tag = fisno;
            if (AcikSatisindex == 3)
                Satis3Toplam.Tag = fisno;
            if (AcikSatisindex == 4)
                Satis4Toplam.Tag = fisno;

            //detay ekle
            DataTable dtSatisDetay = DB.GetData(@"select pkStokKarti,sd.Adet,sk.AlisFiyati,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzdetutar,
sd.KdvOrani, sd.NakitFiyat, sd.GercekAdet, sd.Birimi, sd.SatisFiyatiKdvHaric, sd.isKdvHaric,
sd.SiraNo, sd.fkDepolar, isnull(sd.fkiskontolar, 0) as fkiskontolar, sd.fkParaBirimi, kur_fiyati
 from SatisDetay sd with(nolock)
inner join StokKarti sk with(nolock) on sk.pkStokKarti = sd.fkStokKarti
where fkSatislar = " + pkSatislar_sablon);
            //DataTable dtSatisDetay = DB.GetData("select * from SatisDetay with(nolock) where fkSatislar=" + pkSatislar_sablon);
            for (int i = 0; i < dtSatisDetay.Rows.Count; i++)
            {
                //string fkSatislar = dtSatisDetay.Rows[i]["fkSatislar"].ToString();
                string fkStokKarti = dtSatisDetay.Rows[i]["pkStokKarti"].ToString();
                string Adet = dtSatisDetay.Rows[i]["Adet"].ToString();
                string AlisFiyati = dtSatisDetay.Rows[i]["AlisFiyati"].ToString();
                string SatisFiyati = dtSatisDetay.Rows[i]["SatisFiyati"].ToString();
                string iskontotutar = dtSatisDetay.Rows[i]["iskontotutar"].ToString();
                string iskontoyuzdetutar = dtSatisDetay.Rows[i]["iskontoyuzdetutar"].ToString();
                string KdvOrani = dtSatisDetay.Rows[i]["KdvOrani"].ToString();
                string NakitFiyat = dtSatisDetay.Rows[i]["NakitFiyat"].ToString();
                string GercekAdet = dtSatisDetay.Rows[i]["GercekAdet"].ToString();
                string Birimi = dtSatisDetay.Rows[i]["Birimi"].ToString();
                if (Birimi == "") Birimi = "Adet";
                string SatisFiyatiKdvHaric = dtSatisDetay.Rows[i]["SatisFiyatiKdvHaric"].ToString();
                if (SatisFiyatiKdvHaric == "") SatisFiyatiKdvHaric = SatisFiyati;

                string isKdvHaric = dtSatisDetay.Rows[i]["isKdvHaric"].ToString();
                if (isKdvHaric == "False") isKdvHaric = "0"; else isKdvHaric = "1";
                string SiraNo = dtSatisDetay.Rows[i]["SiraNo"].ToString();
                if (SiraNo == "") SiraNo = "0";
                string fkDepolar = dtSatisDetay.Rows[i]["fkDepolar"].ToString();

                sql = @"INSERT INTO SatisDetay (fkSatislar,fkStokKarti, Adet, AlisFiyati, SatisFiyati, Tarih, Stogaisle, iskontotutar, iskontoyuzdetutar,
                        KdvOrani, NakitFiyat,  iade, fkiskontolar, GercekAdet, 
                       Birimi, SatisFiyatiKdvHaric, isKdvHaric, Faturaiskonto, SiraNo,fkDepolar) 
                    values(@fkSatislar,@fkStokKarti,@Adet,@AlisFiyati,@Satis_Fiyati,getdate(),0,@iskontotutar,@iskontoyuzdetutar,@KdvOrani,@NakitFiyat,0,0,@GercekAdet,
                      '@Birimi',@SatisFiyatiKdvHaric,@isKdvHaric,0,@SiraNo,@fkDepolar)";

                sql = sql.Replace("@fkSatislar", fisno);
                sql = sql.Replace("@fkStokKarti", fkStokKarti);
                sql = sql.Replace("@Adet", Adet.Replace(",", "."));
                sql = sql.Replace("@AlisFiyati", AlisFiyati.Replace(",", "."));
                sql = sql.Replace("@Satis_Fiyati", SatisFiyati.Replace(",", "."));
                sql = sql.Replace("@iskontotutar", iskontotutar.Replace(",", "."));
                sql = sql.Replace("@iskontoyuzdetutar", iskontoyuzdetutar.Replace(",", "."));
                sql = sql.Replace("@KdvOrani", KdvOrani.Replace(",", "."));
                sql = sql.Replace("@NakitFiyat", NakitFiyat.Replace(",", "."));
                sql = sql.Replace("@GercekAdet", GercekAdet.Replace(",", "."));
                sql = sql.Replace("@Birimi", Birimi);
                sql = sql.Replace("@SatisFiyatiKdvHaric", SatisFiyatiKdvHaric.Replace(",", "."));
                sql = sql.Replace("@isKdvHaric", isKdvHaric);
                sql = sql.Replace("@SiraNo", SiraNo);
                sql = sql.Replace("@fkDepolar", fkDepolar);

                DB.ExecuteSQL(sql);
            }


            if (AcikSatisindex == 1 && Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
            }

            deGuncellemeTarihi.Enabled = true;

            yesilisikyeni();
        }

        private void btnKdvHaricDahilYap_Click(object sender, EventArgs e)
        {
            //daha önce basılmış mı?
            //bool dahaoncebasilmis = false;
            //DataTable dtKdv =
            //DB.GetData("select fkSatislar from SatisDetay with(nolock) where NakitFiyat=SatisFiyatiKdvHaric and fkSatislar=" + txtpkSatislar.Text);
            //if (dtKdv.Rows.Count == 0)
            //{
            //    dahaoncebasilmis = true;
            //}
            string mesaj = "Satış Fiyatları Kdv Hariç Yapılacak?";
            //if (dahaoncebasilmis)
            //{
            //    mesaj = "Satış Fiyatları Geri Alınacak";
            //}

            string s = formislemleri.MesajBox(mesaj, Degerler.mesajbaslik, 3, 2);

            if (s == "0") return;

            int sonuc = DB.ExecuteSQL_Sonuc_Sifir(@"update SatisDetay set iskontotutar=0,iskontoyuzdetutar=0,
                    SatisFiyati = NakitFiyat - ((NakitFiyat *KdvOrani) / (100 + KdvOrani)) where fkSatislar=" + txtpkSatislar.Text);
            if (sonuc != 0)
            {
                MessageBox.Show("Hata Oluştu Lütfen Tekrar Deneyiniz");
                yesilisikyeni();
                return;
            }
            /*
            DataTable dtSatisDetay = DB.GetData(@"select sd.fkStokKarti,sk.AlisFiyati,sd.NakitFiyat,sd.KdvOrani,
            CAST(sd.NakitFiyat - ((sd.NakitFiyat * sd.KdvOrani) / (100 + sd.KdvOrani)) as decimal(18, 6)) KdvHaricSatis,
            CAST(sk.AlisFiyati - ((sk.AlisFiyati * sk.KdvOraniAlis) / (100 + sk.KdvOraniAlis)) as decimal(18, 6)) as KdvHaricAlis
            from SatisDetay sd with(nolock)
            inner join StokKarti sk with(nolock) on sk.pkStokKarti = sd.fkStokKarti
            where sd.fkSatislar = " + txtpkSatislar.Text);

            for (int i = 0; i < dtSatisDetay.Rows.Count; i++)
            {
                string StokKartiid = dtSatisDetay.Rows[i]["fkStokKarti"].ToString();
                if (dahaoncebasilmis)
                {
                    int sonuc = DB.ExecuteSQL_Sonuc_Sifir(@"update SatisDetay set iskontotutar=0,
                    iskontoyuzdetutar = 0,Satisfiyati = SatisDetay.NakitFiyat,
                    AlisFiyati = sk.AlisFiyati from StokKarti sk
                    where sk.pkStokKarti = SatisDetay.fkStokKarti and fkSatislar = " + txtpkSatislar.Text + " and fkStokKarti = " + StokKartiid);
                }
                else
                {
                    int sonuc = DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set " +
                    "iskontotutar=0,iskontoyuzdetutar=0," +
                    "Satisfiyati=" + dtSatisDetay.Rows[i]["KdvHaricSatis"].ToString().Replace(",", ".") + "," +
                    "AlisFiyati=" + dtSatisDetay.Rows[i]["KdvHaricAlis"].ToString().Replace(",", ".") +
                     " where fkSatislar=" + txtpkSatislar.Text + " and fkStokKarti=" + StokKartiid);
                }

            }
            */


            yesilisikyeni();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (deGuncellemeTarihi.EditValue == null)
                deGuncellemeTarihi.DateTime = DateTime.Now;

            if (deFaturaTarihi.Text == "")
            {
                deFaturaTarihi.Focus();
                return;
            }
            #region Müşteri id al
            string fkFirma = "0";
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
                case 4:
                    {
                        fkFirma = Satis4Firma.Tag.ToString();
                        break;
                    }
            }
            #endregion
            DataTable dtFirma = DB.GetData("select * from VFaturaFirmaBilgileri with(nolock) where pkFirma=" + fkFirma);
            string vergino = dtFirma.Rows[0]["VergiNo"].ToString();
            if (vergino.Length != 11)
            {
                MessageBox.Show("Lütfen T.C. Kimlik No(Vergi No Kontrol Ediniz)");
                return;
            }


            var client = InvoiceTasks.Instance.CreateClient();

            FaturaGonder fatgonder = new FaturaGonder();
            Satislar fatura = new Satislar();
            fatura.pkSatislar = int.Parse(txtpkSatislar.Text);
            fatura.FaturaTarihi = deFaturaTarihi.DateTime;
            if (lueSatisTipi.Text == "E-Fatura") //"E-Arşiv")
               fatura.FaturaTuru = "EARSIVFATURA";//"TEMELFATURA";// "EARSIVFATURA";//"TICARIFATURA";
            else
               fatura.FaturaTuru = "TEMELFATURA";//"TEMELFATURA";// "EARSIVFATURA";//"TICARIFATURA";
            fatura.FaturaTipi = "SATIS";//"ISTISNA";
            fatura.FaturaNo = "";//txtFaturaNo.Text;
            
            //DataTable dtFirma = DB.GetData("select * from VFaturaFirmaBilgileri with(nolock) where pkFirma=" + fkFirma);
            string alici_adi_soyadi= dtFirma.Rows[0]["Firmaadi"].ToString();
            //fatura.AliciSoyad = alici_adi_soyadi;
            char ayrac = ' ';
            string[] parcalar = alici_adi_soyadi.Split(ayrac);
            if (parcalar.Length > 0)
                fatura.AliciAdi = parcalar[0];
            if (parcalar.Length > 1)
                fatura.AliciSoyad = parcalar[1];
            //if (parcalar.Length > 2)
            //fatura.AliciUnvani = fatura.AliciUnvani + parcalar[2];
            //fatura.AliciAdi = alici_adi_soyadi;

            fatura.AliciVergiNo = vergino;
            fatura.AliciVergiDairesi = dtFirma.Rows[0]["VergiDairesi"].ToString();
            fatura.Aliciil = dtFirma.Rows[0]["il_adi"].ToString();
            fatura.Aliciilce = dtFirma.Rows[0]["ilce_adi"].ToString();
            fatura.AliciCaddeSokak = dtFirma.Rows[0]["caddesokak"].ToString();

            fatura.Alici_ePosta = dtFirma.Rows[0]["Eposta"].ToString();
            fatura.Alici_Tel = dtFirma.Rows[0]["Cep"].ToString();
            fatura.Alici_Web = dtFirma.Rows[0]["webadresi"].ToString();
            fatura.Alici_Fax = dtFirma.Rows[0]["Fax"].ToString();

            fatura.Aciklama = btnAciklamaGirisi.ToolTip;
            fatura.AraToplam = aratoplam.Value;
            fatura.Toplamiskonto = ceToplamiskonto.Value;
            fatura.KdvToplamTutari = ceKdvToplamTutari.Value;

            SatisDetay[] faturadetay = new SatisDetay[gridView1.RowCount-1];
            for (int i = 0; i < gridView1.RowCount-1; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                string barkod = "", fkMarka="", fkModel="";
                DataTable dtStok = DB.GetData("select pkStokKarti,StokKod,Barcode,Stokadi,fkMarka,fkModel from StokKarti with(nolock) where pkStokKarti=" + 
                    dr["fkStokKarti"].ToString());
                if(dtStok.Rows.Count>0)
                {
                    barkod = dtStok.Rows[0]["barcode"].ToString();
                    fkMarka = dtStok.Rows[0]["fkMarka"].ToString();
                    fkModel = dtStok.Rows[0]["fkModel"].ToString();
                }
                
                faturadetay[i] = new SatisDetay();
                faturadetay[i].pkSatisDetay = int.Parse(dr["pkSatisDetay"].ToString());
                faturadetay[i].Barkod = barkod;
                faturadetay[i].UrunAdi = dr["Stokadi"].ToString();
                faturadetay[i].Aciklama = dr["aciklama_detay"].ToString();
                faturadetay[i].Birimi = dr["Birimi"].ToString();
                faturadetay[i].Marka = fkMarka;
                faturadetay[i].Model = fkModel;
                faturadetay[i].Miktar = int.Parse(dr["Adet"].ToString());
                faturadetay[i].Fiyat = decimal.Parse(dr["SatisFiyatiKdvHaric"].ToString());
                faturadetay[i].KdvOrani = decimal.Parse(dr["KdvOrani"].ToString());
                faturadetay[i].iskontoOrani = decimal.Parse(dr["iskontoyuzdetutar"].ToString());
                faturadetay[i].iskontoTutar = decimal.Parse(dr["iskontotutar"].ToString());
                decimal kdvtoptut = decimal.Parse(dr["KdvToplamTutari"].ToString());
                string formattedMoneyValue = String.Format("{0:C}", kdvtoptut);
                faturadetay[i].KdvTutar = kdvtoptut;//decimal.Parse(formattedMoneyValue.Replace(",","."));
                faturadetay[i].ToplamTutar = decimal.Parse(dr["Tutar"].ToString());
            }
            
            var invoiceInfo = fatgonder.CreateInvoice(fatura, faturadetay);

            InvoiceInfo[] invoices = new InvoiceInfo[1];
            invoices[0] = invoiceInfo;

            frmYukleniyor yukleniyor = new frmYukleniyor();
            yukleniyor.TopMost = true;
            yukleniyor.Show();
            Application.DoEvents();
            var response = client.SendInvoice(InvoiceTasks.Instance.GetUserInfo(), invoices);
            Application.DoEvents();

            yukleniyor.Show();
            yukleniyor.Close();
            //InvoiceIdentitiesResponse response = client.SendInvoice(invoices);

            if (response.IsSucceded)
            {

                MessageBox.Show(
                    string.Format("Fatura Gönderildi\n UUID:{0} \n ID:{1} \n Fatura Tipi:{2} ",
                            response.Value[0].Id.ToString(),
                            response.Value[0].Number.ToString(),
                            response.Value[0].InvoiceScenario.ToString()
                            )
                            );
                //txtSampleOutboxGuid.Text = response.Value[0].Id.ToString();
                txtEFaturaUUID.Text = response.Value[0].Id.ToString();
                DB.ExecuteSQL("update Satislar set uuid='" + response.Value[0].Id + "' where pkSatislar="+ txtpkSatislar.Text);

                txtFaturaNo.Text = response.Value[0].Number.ToString();
                DB.ExecuteSQL("update Satislar set FaturaNo='" + response.Value[0].Number.ToString() + "' where pkSatislar=" + txtpkSatislar.Text);
                // Clipboard.SetText(response.Value[0].Id.ToString());
            }
            else
            {
                MessageBox.Show(response.Message);
            }
        }

        private void lueSatisTipi_MouseClick(object sender, MouseEventArgs e)
        {
            lueSatisTipi.Tag = "1";
        }

        private void lueSatisTipi_Enter(object sender, MouseEventArgs e)
        {
            lueSatisTipi.Tag = "1";
        }

        private void lueSatisTipi_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            lueSatisTipi.Tag = "1";
        }

        private void lueSatisTipi_QueryPopUp(object sender, CancelEventArgs e)
        {
            lueSatisTipi.Tag = "1";
        }

        private void sbtnEFatura_Click(object sender, EventArgs e)
        {
            
        }

        private void repositoryItemButtonEdit2_ButtonClick_1(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
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
            iskonto.fkAlisDetay.Text = "";
            iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
            if (cbFaturaTipi.SelectedIndex == 0)
                iskonto.ceBirimFiyati.Value = decimal.Parse(dr["SatisFiyati"].ToString());
            else
                iskonto.ceBirimFiyati.Value = decimal.Parse(dr["SatisFiyatiKdvHaric"].ToString());
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

        private void ceToplamiskonto_Click(object sender, EventArgs e)
        {
           
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;

            //int secilen = gridView1.FocusedRowHandle;
            //DevExpress.XtraGrid.Columns.GridColumn secilenGridColumn = gridView1.FocusedColumn;
            //DataRow dr = gridView1.GetDataRow(secilen);

            repositoryItemButtonEdit2.Tag = "0";

            frmiskontoToplam iskonto = new frmiskontoToplam(int.Parse(txtpkSatislar.Text), 0);
            //iskonto çıkışda kaydetmesini engellemek için
            //gridView1.Focus();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            iskonto.fkAlisDetay.Text = "";
            iskonto.fkSatisDetay.Text = "";
            //if (cbFaturaTipi.SelectedIndex == 0)
            iskonto.ceBirimFiyati.Value = ceToplam.Value;//decimal.Parse(dr["SatisFiyati"].ToString());
            //else
            //    iskonto.ceBirimFiyati.Value = decimal.Parse(dr["SatisFiyatiKdvHaric"].ToString());
            iskonto.ShowDialog();

            //repositoryItemButtonEdit2.Tag = "0";
            //((DevExpress.XtraEditors.ButtonEdit)((((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue =
            //   iskonto.iskontoorani.Value;

            yesilisikyeni();
            //toplu_kdv_iskonto_degis("i");
           
        }

        private void btnGecmisFis_Click(object sender, EventArgs e)
        {
            if (btnGecmisFis.Tag.ToString() == "0" && formislemleri.SifreIste())
            {
                btnGecmisFis.Tag = "1";
                btnGecmisFis.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;

                yesilisikyeni();
            }
        }

        private void labelControl8_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var client = InvoiceTasks.Instance.CreateClient();
                var response = client.GetInboxInvoicePdf(InvoiceTasks.Instance.GetUserInfo(), txtEFaturaUUID.Text);

                if (response.IsSucceded)
                {

                    using (var stream = File.Create(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + txtEFaturaUUID.Text + ".pdf"))
                    {
                        stream.Write(response.Value.Data, 0, response.Value.Data.Length);
                        stream.Flush();
                        MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + txtEFaturaUUID.Text + ".pdf adresine kaydedildi");
                    }

                }
                else
                {
                    MessageBox.Show(response.Message);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message); ;
            }
        }

        private void labelControl7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://portal.uyumsoft.com.tr/Genel/Fatura/"+txtEFaturaUUID.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var client = InvoiceTasks.Instance.CreateClient();
            var response = client.CancelEArchiveInvoice(InvoiceTasks.Instance.GetUserInfo(), new EArchiveCancelInvoiceContext
            {
                InvoiceId = txtEFaturaUUID.Text,
                CancelDate = DateTime.Now
            });
            if (response.IsSucceded)
            {
                MessageBox.Show("Fatura başarıyla iptal edildi");
            }
            else
            {
                MessageBox.Show("Fatura İptal edilemedi: " + response.Message);
            }
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {
            //lblInvoiceStatus.Text = "";
            var client = InvoiceTasks.Instance.CreateClient();
            var response = new InvoiceStatusWithLogResponse();
            // var response = new InvoiceStatusResponse();
            var status = new InvoiceStatus();

            var statusCode = 0;
            var msg = "";
            var gibStatus = 0;

            InvoiceLogItem[] logs = null;
            StringBuilder logsb = new StringBuilder();
            var guid = new String[] { txtEFaturaUUID.Text };
            try
            {
                //response = client.QueryInboxInvoiceStatus(guid);
                //response.Value[0].

                response = client.GetOutboxInvoiceStatusWithLogs(InvoiceTasks.Instance.GetUserInfo(), guid);

                status = response.Value[0].Status;
                statusCode = response.Value[0].StatusCode;
                gibStatus = response.Value[0].EnvelopeStatusCode;
                logs = response.Value[0].Logs;


                msg = response.Value[0].Message;
                if (logs != null && logs.Length > 0)
                {
                    foreach (var item in logs)
                    {
                        logsb.AppendLine("-" + item.Message);

                    }
                }

                MessageBox.Show(string.Format("Fatura Durumu : {0} : Durum Kodu : {1} , {2} \r\n  Loglar: {3}", status.ToString(), statusCode.ToString(), msg, logsb.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");
                //lblInvoiceStatus.Text = "Durum bilgisi alınamadı";
            }
        }
    }
}
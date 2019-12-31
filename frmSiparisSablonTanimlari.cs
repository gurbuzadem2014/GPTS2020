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
    public partial class frmSiparisSablonTanimlari : DevExpress.XtraEditors.XtraForm
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis = "";//AccessibleDescription
        decimal HizliMiktar = 1;
        public frmSiparisSablonTanimlari()
        {
            InitializeComponent();
            //this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            //this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
            DB.PkFirma = 1;
        }
        void PersonelGetir()
        {
            lUEPersonel.Properties.DataSource = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller where Plasiyer=1 and AyrilisTarihi is null");
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";
        }
        void MusteriZiyaretGunleriGetir()
        {
            gridControl2.DataSource =
DB.GetData(@"SELECT  pkMusteriZiyaretGunleri, fkFirma,fkGunler, GunSonra, fkPersoneller, VarYok,fkSablonGrup,
fkSiparisSablonlari,fkSiparisSablonlari1,fkSiparisSablonlari2,fkSiparisSablonlari3,fkSablonGrup,Gunler.GunAdi 
                      FROM MusteriZiyaretGunleri inner join Gunler on pkGunler=fkGunler where fkFirma=" + Satis1Firma.Tag.ToString() +
"  order by Gunler.Kod");
        }
        void MusteriGetir()
        {
            string ozelkod = "0", firmadi = "", fkPerTeslimEden = "";// MusteriAra.fkFirma.AccessibleDescription;

            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod,fkPerTeslimEden from Firmalar where pkFirma=" + Satis1Firma.Tag.ToString());
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            fkPerTeslimEden = dt.Rows[0]["fkPerTeslimEden"].ToString();
            if (fkPerTeslimEden == "")
                lUEPersonel.EditValue = null;
            else
                lUEPersonel.EditValue = int.Parse(fkPerTeslimEden);

            Satis1Baslik.Text = ozelkod + "-" + firmadi;
            Satis1Baslik.ToolTip = Satis1Baslik.Text;
            MusteriZiyaretGunleriGetir();
            //yesilisikyeni();
        
        }
        void Fiyatlarigetir()
        {
            lueFiyatlar.Properties.DataSource = DB.GetData("select * from SatisFiyatlariBaslik where Aktif=1 order by pkSatisFiyatlariBaslik");
            lueFiyatlar.EditValue = 1;
        }
        private void ucAnaEkran_Load(object sender, EventArgs e)
        {
            HizliSatisTablariolustur();
            Yetkiler();
            lueSablonGrup.Properties.DataSource = DB.GetData(@"SELECT * FROM SablonGrup");
            PersonelGetir();
            cbmusteridurum.SelectedIndex = 0;
            MusteriGetir();
            Fiyatlarigetir();
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
            string sql = @"SELECT   YetkiAlanlari.Yetki, YetkiAlanlari.Sayi, Parametreler.Aciklama10, YetkiAlanlari.BalonGoster
FROM  YetkiAlanlari INNER JOIN Parametreler ON YetkiAlanlari.fkParametreler = Parametreler.pkParametreler
WHERE  Parametreler.fkModul = 1  and YetkiAlanlari.fkKullanicilar =" + DB.fkKullanicilar;
            DataTable dtYetkiler = DB.GetData(sql);
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "ParaPanel")
                //    ParaPanel.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);

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
                sb.Tag = Barcode;
                sb.ToolTip = "Satış Fiyatı=" + SatisFiyati + "\n Stok Adı:" + Stokadi;
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
           Satis1Toplam.Tag = 0;
           Satis1Toplam.Text = "0,0";
           pkSatisBarkod.Text = "0";
           temizle();
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }
            //DialogResult secimtum;
            //secimtum = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Siparişler İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secimtum == DialogResult.No)
            //{
            //    yesilisikyeni();
            //    return;
            //}

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Şablon İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle

            string  pkSatislar = Satis1Toplam.Tag.ToString();

            int sonuc = DB.ExecuteSQL("DELETE FROM SiparisSablonlari WHERE pkSiparisSablonlari=" + pkSatislar);
            if (sonuc != 1)
                MessageBox.Show("Hata Oluştu");
            //SatisTemizle();
            Satis1Toplam.Tag = 0;
            Satis1Toplam.Text = "0,0";
            pkSatisBarkod.Text = "0";
            simpleButton2.Enabled = true;//müşteri ara 
            //temizle(AcikSatisindex);
            gridView1.Focus();
            yesilisikyeni();
            DurumListesi();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("DELETE FROM SiparisSablonDetay WHERE pkSiparisSablonDetay=" + dr["pkSiparisSablonDetay"].ToString());
            //DB.ExecuteSQL("update StokKarti set Mevcut=Mevcut+" + dr["Adet"].ToString().Replace(",", ".") + "  where pkStokKarti=" + dr["fkStokKarti"].ToString());
            gridView1.DeleteSelectedRows();
            if (gridView1.DataRowCount == 0)
            {
                DB.ExecuteSQL("DELETE FROM SiparisSablonDetay WHERE pkSiparisSablonDetay=" + pkSatisBarkod.Text);
                pkSatisBarkod.Text = "0";
                Satis1Toplam.Tag = "0";
            }
            yesilisikyeni();
        }
        void YeniSatisEkle()
        {
            if (Satis1Toplam.Tag.ToString() == "0")
                YeniSiparisSablon();
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
            SatisDetayGetir(Satis1Toplam.Tag.ToString());
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();
            SatisGetir();
            gridView1.Focus();
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
        void GeciciMusteriDefault()
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.Lütfen Yetkili Firmayı Arayınız");
            else
            {
                MusteriBaslikGetir(dtMusteriler.Rows[0]["pkFirma"].ToString());
            }
        }
        void temizle()
        {
            MusteriBaslikGetir("1");
            lueSablonGrup.EditValue = 10;
            gridControl1.DataSource = null;
            lUEPersonel.EditValue = null;
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
            //satiskaydet(yazdir, odemekaydedildi);
            FisListesi();
            // temizle(AcikSatisindex);
            yesilisikyeni();
        }
        public bool bir_nolumusteriolamaz()
        {
            if (DB.GetData("select * from Sirketler").Rows[0]["MusteriZorunluUyari"].ToString() == "True")
            {
                if (Satis1Firma.Tag.ToString() == "1")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Müşteri Olamaz.\n (Ayarlardan Kaldırabilirsiniz.)!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MusteriAra();
                    return false;
                }
            }
            return true;

        }
        void kaydetyazdir(string btn_kaydet_yazdir)
        {
            if (bir_nolumusteriolamaz() == false) return;
            if (OnceSatisYapiniz() == false) return;
            //if (btn_kaydet_yazdir == "kaydet")
            //    satiskaydet(true, false);
            else if (btn_kaydet_yazdir == "yazdir")
            {
                string  pkSatislar = Satis1Toplam.Tag.ToString();
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
                    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(21,0);
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
                    //satiskaydet(true, false);
                    FisYazdir(false, pkSatislar, YaziciDosyasi, YaziciAdi);
                }
            }
            yesilisikyeni();
        }
        private void simpleButton37_Click(object sender, EventArgs e)
        {
            if (lUEPersonel.EditValue == null)
            {
                DataTable dt =
                DB.GetData("select fkPerTeslimEden from Firmalar where pkFirma=" + Satis1Firma.Tag.ToString());
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Personel Seçiniz");
                    lUEPersonel.Focus();
                    return;
                }
                else
                {
                    lUEPersonel.EditValue = int.Parse(dt.Rows[0]["fkPerTeslimEden"].ToString());
                }
            }

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Aciklama", DBNull.Value));
                list.Add(new SqlParameter("@pkSiparisSablonlari", pkSatisBarkod.Text));
                list.Add(new SqlParameter("@fkPersoneller", lUEPersonel.EditValue.ToString()));

                DB.ExecuteSQL("UPDATE SiparisSablonlari SET Aciklama=@Aciklama,fkPersoneller=@fkPersoneller WHERE pkSiparisSablonlari=@pkSiparisSablonlari", list);

                //if (odenennakit.Value == 0 && odenenkredikarti.Value == 0 && odenenacikhesap.Value==0 )
                //    odenennakit.Value = aratoplam.Value;

                //simpleButton37.Text = "Güncelle [F9]";
            //kaydetyazdir("kaydet");
            //simpleButton15_Click(sender, e);
           MusteriZiyaretGunleriGetir();
           btnYeni_Click(sender, e);
        }
        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }
        void FisYazdir(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
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
            string fisid = pkSatisBarkod.Text;
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
        private string MusteriAra()
        {
            string fkFirma = "1", ozelkod = "0", firmadi = "",fkPerTeslimEden="";// MusteriAra.fkFirma.AccessibleDescription;

            fkFirma = Satis1Firma.Tag.ToString();

            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.fkFirma.Tag = fkFirma;
            MusteriAra.ShowDialog();
            fkFirma = MusteriAra.fkFirma.Tag.ToString();

            if (pkSatisBarkod.Text != "0" && pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE SiparisSablonlari SET fkFirma=" + fkFirma + " where pkSiparisSablonlari=" + pkSatisBarkod.Text);
            }
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod,fkPerTeslimEden from Firmalar with(nolock) where pkFirma=" + fkFirma);
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            fkPerTeslimEden = dt.Rows[0]["fkPerTeslimEden"].ToString();
            if (fkPerTeslimEden == "")
                lUEPersonel.EditValue = null;
            else
               lUEPersonel.EditValue = int.Parse(fkPerTeslimEden);
            MusteriAra.Dispose();

            MusteriBaslikGetir(fkFirma);
            MusteriZiyaretGunleriGetir();
            return fkFirma;
        }
        private string musteriata(string fkFirma)
        {
            string pkFirma = "1", ozelkod = "0", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where OzelKod=" + fkFirma);
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
            if (pkSatisBarkod.Text != "0" || pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE Satislar SET fkFirma=" + pkFirma + " where pkSatislar=" + pkSatisBarkod.Text);
            }

            Satis1Firma.Tag = pkFirma;
            Satis1Baslik.Text = ozelkod + "-" + firmadi;
            Satis1Baslik.ToolTip = Satis1Baslik.Text;
            yesilisikyeni();
            return fkFirma;
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            MusteriAra();
            yesilisikyeni();
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
          
            //if (pkStokKartiid == "0")
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            //else
            //DB.pkStokKarti = int.Parse(pkStokKartiid);
            StokKarti.ShowDialog();
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
   //         if (e.KeyValue == 222)
   //         {
   //             string kod =
   //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
   //             if (kod == "")
   //             {
   //                 gridView1.SetFocusedRowCellValue("Barcode", "");
   //                 return;
   //             }
   //             int adetvar = kod.IndexOf("*");
   //             int badet = 1;
   //             //string bbarkod = barkod;
   //             if (adetvar > 0)
   //             {
   //                 badet = int.Parse(kod.Substring(0, adetvar));
   //                 //bbarkod = barkod.Substring(adetvar + 1, barkod.Length - (adetvar + 1));

   //             }
   //             gridView1.SetFocusedRowCellValue("iade", "true");
   //             gridView1.SetFocusedRowCellValue("Barcode", "");
   //             ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
   //         }
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
           // if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["AlisFiyati"].ToString() != "")
           // {
                //decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
               // decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                //if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                  //  AppearanceHelper.Apply(e.Appearance, appError);
           // }
            //if (e.Column.FieldName == "iskontotutar" && dr["iskontotutar"].ToString() != "0,000000")
            //{
            //    AppearanceHelper.Apply(e.Appearance, appfont);
            //}
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
                string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
                e.Appearance.BackColor = Satis1Toplam.BackColor;
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
            if (gridView1.DataRowCount == 0)
            {
                Satis1Toplam.Text = "0,00";
                return;
            }
            decimal aratop = 0;
            if (gridColumn5.SummaryItem.SummaryValue == null)
                aratop = 0;//atis1Toplam.Text = "0,0";
            else
                aratop = decimal.Parse(gridColumn5.SummaryItem.SummaryValue.ToString());

            decimal istutar = 0;
            //önce hesapla sonra bilgi göster NullTexde

           aratoplam.Value = aratop - istutar;
           Satis1Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
           Satis1Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();
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

        void FisListesi()
        {
            string sql = @"SELECT TOP 20 pkSiparisSablonlari, Aciklama, fkPersoneller, fkFirma FROM  SiparisSablonlari ORDER BY pkSiparisSablonlari DESC";
            //if (lueSatisTipi.EditValue == null) lueSatisTipi.EditValue = 10;
            //sql = sql.Replace("@pkSatisDurumu", lueSatisTipi.EditValue.ToString());
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
            FisNoBilgisi.fisno.EditValue = lueFis.EditValue.ToString();
            FisNoBilgisi.ShowDialog();
            if (FisNoBilgisi.TopMost == true)
            {
                SatisGetir();
            }
            FisNoBilgisi.Dispose();
            FisListesi();
            lueFis.EditValue = null;
            yesilisikyeni();
        }

        void SatisGetir()
        {
            string pkSatislar = Satis1Toplam.Tag.ToString();
            if (pkSatislar == "") pkSatislar = "0";

            DataTable dtSatislar = DB.GetData("select pkFirma,Firmaadi,case when fkPersoneller=0 then Firmalar.fkPerTeslimEden else fkPersoneller end fkPersoneller ,OdemeSekli from SiparisSablonlari " +
            " inner join Firmalar on Firmalar.pkFirma=SiparisSablonlari.fkFirma where pkSiparisSablonlari=" + pkSatislar);//fiş bilgisi

            if (dtSatislar.Rows.Count == 0)
            {
                //Showmessage("Şablon Silinmiş Olabilir Lütfen Kontrol Ediniz", "K");
                Satis1Toplam.Tag = "0";
                pkSatisBarkod.Text = "0";
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (DB.secilenfkSiparisSablonlari == 0)
                    DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari=null where pkMusteriZiyaretGunleri="
                        + dr["pkMusteriZiyaretGunleri"].ToString());
                return;
            }
            string fkfirma = dtSatislar.Rows[0]["pkFirma"].ToString();
            string firmaadi = dtSatislar.Rows[0]["Firmaadi"].ToString();
            if (dtSatislar.Rows[0]["fkPersoneller"].ToString() != "")
                lUEPersonel.EditValue = int.Parse(dtSatislar.Rows[0]["fkPersoneller"].ToString());
            if (dtSatislar.Rows[0]["OdemeSekli"].ToString() == "")
                cbOdemeSekli.Text = "Nakit";
            else
                cbOdemeSekli.EditValue = dtSatislar.Rows[0]["OdemeSekli"].ToString();

            Satis1Firma.Tag = fkfirma;
            Satis1Firma.Text = fkfirma + "-" + firmaadi;
            Satis1Baslik.Text = fkfirma + "-" + firmaadi;
        }

        void pkSatislarGetir(object sender, EventArgs e)
        {
            if (DB.pkSatislar > 0)
            {
                pkSatisBarkod.Text = DB.pkSatislar.ToString();
                Satis1Toplam.Tag = DB.pkSatislar;// lueFis.EditValue.ToString();
                Satis1Toplam_Click(sender, e);
                SatisGetir();
            }
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
          
            pkSatislarGetir(sender, e);
            yesilisikyeni();
        }

        private void cariSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MusteriAra();
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

            Hizlibuttonlariyukle();
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
            if (lueSablonGrup.Text == "İade")
            {
                decimal miktar = decimal.Parse(yenimiktar);
                if (miktar > 0)
                    miktar = miktar * -1;
                yenimiktar = miktar.ToString();
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSatisDetay = dr["pkSiparisSablonDetay"].ToString();
            DB.ExecuteSQL("UPDATE SiparisSablonDetay SET Adet=" + yenimiktar.Replace(",", ".") + " where pkSiparisSablonDetay=" + pkSatisDetay);
            //decimal iskontoyuzde = 0;
            //if (dr["iskontoyuzdetutar"].ToString() != "")
              //  iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
            //decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());
            decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
            decimal Miktar = Convert.ToDecimal(yenimiktar);
            //decimal iskontogercektutar = Convert.ToDecimal(dr["iskontotutar"].ToString());

            //if (iskontogercektutar > 0)
            //{
            //    iskontogercekyuzde = (iskontogercektutar * 100) / (Fiyat * Miktar);
            //}
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn3, yenimiktar);
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

        private void gridControl1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
        }
        private void ceiskontoyuzde_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void lueSatisTipi_EditValueChanged(object sender, EventArgs e)
        {
            //if (lueSatisTipi.EditValue.ToString() == "1")
            //{
            //    fkSiparisSablonlari.Visible = true;
            //    fkSiparisSablonlari1.Visible = false;
            //    fkSiparisSablonlari2.Visible = false;
            //}
            //else if (lueSatisTipi.EditValue.ToString() == "2")
            //{
            //    fkSiparisSablonlari.Visible = false;
            //    fkSiparisSablonlari1.Visible = true;
            //    fkSiparisSablonlari2.Visible = false;
            //}
            //else if (lueSatisTipi.EditValue.ToString() == "3")
            //{
            //    fkSiparisSablonlari.Visible = false;
            //    fkSiparisSablonlari1.Visible = false;
            //    fkSiparisSablonlari2.Visible = true;
            //} 
            FisListesi();
        }
        void SatisDigerBilgileriGetir()
        {
            //TODO:SatisDigerBilgileriGetir
            DataTable dt = DB.GetData("select * from SiparisSablonlari where pkSiparisSablonlari=0");
            //btnAciklamaGirisi.ToolTip = dt.Rows[0]["Aciklama"].ToString();
        }
        void SatisDetayGetir(string fkSiparisSablonlari)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string fkFirma = dr["fkFirma"].ToString();
              DataTable dt = DB.GetData("select fkSatisFiyatlariBaslik from Firmalar where pkFirma=" + fkFirma);
            if(dt.Rows.Count>0)
              lueFiyatlar.EditValue=int.Parse(dt.Rows[0][0].ToString());

            if (fkSiparisSablonlari == "") fkSiparisSablonlari = "0";
            pkSatisBarkod.Text = fkSiparisSablonlari;
            string sql = @"SELECT sk.pkStokKarti, pkSiparisSablonDetay,SiparisSablonDetay.fkSiparisSablonlari,
sk.Barcode,SiparisSablonDetay.Adet,isnull(sf.SatisFiyatiKdvli,sk.SatisFiyati) as SatisFiyati,
sk.AlisFiyati,sk.Stoktipi,sk.Stokadi
FROM SiparisSablonDetay 
INNER JOIN StokKarti sk ON SiparisSablonDetay.fkStokKarti = sk.pkStokKarti
left join SatisFiyatlari sf on sf.fkStokKarti=sk.pkStokKarti and sf.fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik
WHERE SiparisSablonDetay.fkSiparisSablonlari =@fkSiparisSablonlari";
            sql = sql.Replace("@fkSatisFiyatlariBaslik", lueFiyatlar.EditValue.ToString());
            sql = sql.Replace("@fkSiparisSablonlari", fkSiparisSablonlari);
            
            gridControl1.DataSource = DB.GetData(sql);
          
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
                if (gridView1.FocusedRowHandle < 0) 
                    EklenenMiktar = 1;
                   else
                if (dr["Adet"].ToString() != "")
                    EklenenMiktar = decimal.Parse(dr["Adet"].ToString());
            }
            if (EklenenMiktar == 1)
                EklenenMiktar = HizliMiktar;
            decimal f = 0;
            decimal.TryParse(barkod, out f);
           // if (dr != null && dr["fkSiparisSablonlari"].ToString() != "") return;
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
           // if (lueSatisTipi.Text == "İade") EklenenMiktar = EklenenMiktar * -1;
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
            arr.Add(new SqlParameter("@fkSiparisSablonlari", pkSatisBarkod.Text));
            arr.Add(new SqlParameter("@Adet", EklenenMiktar.ToString().Replace(",", ".")));
            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));

            string s = DB.ExecuteScalarSQL("INSERT INTO SiparisSablonDetay(fkSiparisSablonlari,Adet,fkStokKarti) VALUES(@fkSiparisSablonlari,@Adet,@fkStokKarti) SELECT IDENT_CURRENT('SiparisSablonDetay') ", arr);
            HizliMiktar = 1;
         //}
        }
        void YeniSiparisSablon()
        {
            string sql = "";
            string fisno = "0";
            bool yazdir = false;
            ArrayList list = new ArrayList();
            string pkFirma = "1";
            pkFirma = Satis1Firma.Tag.ToString();
            list.Add(new SqlParameter("@fkFirma", pkFirma));
            if (lUEPersonel.EditValue==null)
                list.Add(new SqlParameter("@fkPersoneller", "1"));
            else
            list.Add(new SqlParameter("@fkPersoneller", lUEPersonel.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", DBNull.Value));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));

            sql = "INSERT INTO SiparisSablonlari (Tarih,fkFirma,Aciklama,ToplamTutar,fkPersoneller)" +
                " VALUES(getdate(),@fkFirma,@Aciklama,@ToplamTutar,@fkPersoneller) SELECT IDENT_CURRENT('SiparisSablonlari')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            if (Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                //
                DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                if (gridView2.FocusedColumn.FieldName == "fkSiparisSablonlari")
                {
                    DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari=" + fisno + " where pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());
                    gridView2.SetFocusedRowCellValue("fkSiparisSablonlari", fisno);
                }
                else if (gridView2.FocusedColumn.FieldName == "fkSiparisSablonlari1")
                {
                    DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari1=" + fisno + " where pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());
                    gridView2.SetFocusedRowCellValue("fkSiparisSablonlari1", fisno);
                }
                else if (gridView2.FocusedColumn.FieldName == "fkSiparisSablonlari2")
                {
                    DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari2=" + fisno + " where pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());
                    gridView2.SetFocusedRowCellValue("fkSiparisSablonlari2", fisno);
                }
                //MusteriZiyaretGunleriGetir();
                
                SatisDetayGetir(Satis1Toplam.Tag.ToString());
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Bilgileri ve Genel Durumu. Yapım Aşamasındadır...");
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(21,0);
            string pkSatislar = "0";
            pkSatislar = Satis1Toplam.Tag.ToString();
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

        private void sonrakiSatisToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                string girilen =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dr == null) return;
                string pkSiparisSablonDetay = dr["pkSiparisSablonDetay"].ToString();
                decimal SatisFiyati = decimal.Parse(dr["SatisFiyati"].ToString());
                decimal iskonto = 0, iskontotutar = 0;
                decimal.TryParse(girilen, out iskontotutar);
                iskonto = SatisFiyati - iskontotutar;
                DB.ExecuteSQL("UPDATE SatisDetay SET iskontotutar=" + iskonto.ToString().Replace(",", ".") + " where pkSatisDetay=" + pkSiparisSablonDetay);
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
            if (e.KeyCode == Keys.Enter && !gridView1.IsEditing && gridView1.FocusedColumn.FieldName == "iskyuzdesanal")
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                frmiskonto iskonto = new frmiskonto();
                iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
                iskonto.ShowDialog();
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (gridView1.DataRowCount == 0) e.Cancel = true;
        }

        private void Satis1Toplam_Click(object sender, EventArgs e)
        {
            yesilisikyeni();
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }
        void SatisDetayYenile()
        {
            SatisDetayGetir(Satis1Toplam.Tag.ToString());
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
            DB.PkFirma = int.Parse(Satis1Firma.Tag.ToString());
            frmMusteriKarti MusteriKarti = new frmMusteriKarti(Satis1Firma.Tag.ToString(), "");
            
            MusteriKarti.ShowDialog();
            //özel kod veya adı için getir ve yenile
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + DB.PkFirma);
            string firmadi = dt.Rows[0]["Firmaadi"].ToString();
            string ozelkod = dt.Rows[0]["OzelKod"].ToString();
            Satis1Baslik.Text = ozelkod + "-" + firmadi;
            Satis1Baslik.ToolTip = Satis1Baslik.Text;
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
            müşteriSeçToolStripMenuItem.Enabled = simpleButton2.Enabled;
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
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("10");//
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            Hizlibuttonlariyukle();
        }


        private void Satis1Baslik_DoubleClick_1(object sender, EventArgs e)
        {
            MusteriAra();
        }

        private void Satis1Baslik_Click(object sender, EventArgs e)
        {
            Satis1Baslik.Text = "";
        }

        private void Satis1Baslik_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Satis1Baslik.Text == "")
                    MusteriAra();
                else
                    musteriata(Satis1Baslik.Text);

                yesilisikyeni();
            }
        }

        private void Satis1Baslik_Leave(object sender, EventArgs e)
        {
            if (Satis1Baslik.Text == "")
                Satis1Baslik.EditValue = Satis1Baslik.OldEditValue;
        }


        private void simpleButton12_Click(object sender, EventArgs e)
        {
            frmAracTakip AracTakip = new frmAracTakip();
            AracTakip.ShowDialog();
            //DevExpress.XtraEditors.XtraMessageBox.Show("Yapım Aşamasında...", Degerler.mesajbaslik, MessageBoxButtons.OK);
        }
        void olmayanlariekle()
        {
            if(Satis1Firma.Tag==null) return;
            string fkFirma=Satis1Firma.Tag.ToString();
            string fkPersoneller = "0";
            DataTable dtfirma = DB.GetData("select fkPerTeslimEden from  Firmalar where pkFirma=" + fkFirma);
            if (dtfirma.Rows.Count > 0)
            {
                if(dtfirma.Rows[0][0].ToString()!="")
                  fkPersoneller = dtfirma.Rows[0][0].ToString();
            }
            //pazartesi
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=1 and fkFirma="+fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (VarYok,fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(0," + fkFirma + ",1,7," + fkPersoneller + ")");
            //salı
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=2 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (VarYok,fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(0," + fkFirma + ",2,7," + fkPersoneller + ")");
            //c
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=3 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (VarYok,fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(0," + fkFirma + ",3,7," + fkPersoneller + ")");
            //p
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=4 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (VarYok,fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(0," + fkFirma + ",4,7," + fkPersoneller + ")");
            //cuma
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=5 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (VarYok,fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(0," + fkFirma + ",5,7," + fkPersoneller + ")");
            //ctes
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=6 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (VarYok,fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(0," + fkFirma + ",6,7," + fkPersoneller + ")");
            //p
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=7 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (VarYok,fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(0," + fkFirma + ",7,7," + fkPersoneller + ")");
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmMusteriKarti KurumKarti = new frmMusteriKarti("0", "");
            KurumKarti.ShowDialog();
        }

        void odemelerikaydetyesil()
        {
            DB.ExecuteSQL("UPDATE  SiparisSablonlari SET OdemeSekli='" + cbOdemeSekli.Text +
                    "' WHERE pkSiparisSablonlari=" + pkSatisBarkod.Text);
            yesilisikyeni();
        }
  
        private void btnYeni_Click(object sender, EventArgs e)
        {
            simpleButton37.Text = "Kaydet [F9]";
            pkSatisBarkod.Text = "0";
            Satis1Toplam.Tag = "0";
            aratoplam.EditValue = 0;
            Satis1Toplam.Text = "0,00";
            //timer1.Enabled = true;
            yesilisikyeni();
        }

        private void lUEPersonel_Enter(object sender, EventArgs e)
        {
            lUEPersonel.Tag = "1";
        }

        private void lUEPersonel_Leave(object sender, EventArgs e)
        {
            lUEPersonel.Tag = "0";
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            //AppearanceDefault appSkyBlue = new AppearanceDefault(Color.Yellow);
            AppearanceDefault appred = new AppearanceDefault(Color.Red);
            //AppearanceDefault appSilver = new AppearanceDefault(Color.Silver);
            DataRow dr = gridView2.GetDataRow(e.RowHandle);
            if (dr == null)  return;
            if (dr["VarYok"].ToString() == "True")
            {
                if (e.Column.FieldName == "fkSiparisSablonlari")
                {
                    if (dr["fkSiparisSablonlari"].ToString() == "")
                        AppearanceHelper.Apply(e.Appearance, appred);
                }
                else if (e.Column.FieldName == "fkSiparisSablonlari1")
                {
                    if (dr["fkSiparisSablonlari1"].ToString() == "")
                        AppearanceHelper.Apply(e.Appearance, appred);
                }
                else if (e.Column.FieldName == "fkSiparisSablonlari2")
                {
                    if (dr["fkSiparisSablonlari2"].ToString() == "")
                        AppearanceHelper.Apply(e.Appearance, appred);
                }
                else if (e.Column.FieldName == "fkSiparisSablonlari3")
                {
                    if (dr["fkSiparisSablonlari3"].ToString() == "")
                        AppearanceHelper.Apply(e.Appearance, appred);
                }
                //else
                //  AppearanceHelper.Apply(e.Appearance, appSilver);
            }
        }

        private void siparişŞablonlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSiparisSablonlariDetay SiparisSablonlariDetay = new frmSiparisSablonlariDetay();
            SiparisSablonlariDetay.pkFirma.Text = Satis1Firma.Tag.ToString();
            SiparisSablonlariDetay.ShowDialog();
        }

        private void sİLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
             DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkMusteriZiyaretGunleri = dr["pkMusteriZiyaretGunleri"].ToString();
            if (DB.secilenfkSiparisSablonlari == 0)
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari=null WHERE pkMusteriZiyaretGunleri=" +
                pkMusteriZiyaretGunleri);
            else if (DB.secilenfkSiparisSablonlari == 1)
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari1=null WHERE pkMusteriZiyaretGunleri=" +
                pkMusteriZiyaretGunleri);
            else if (DB.secilenfkSiparisSablonlari == 2)
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari2=null WHERE pkMusteriZiyaretGunleri=" +
                pkMusteriZiyaretGunleri);
            else if (DB.secilenfkSiparisSablonlari == 3)
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari3=null WHERE pkMusteriZiyaretGunleri=" +
                pkMusteriZiyaretGunleri);
            MusteriZiyaretGunleriGetir();
        }

        private void gridView2_RowClick(object sender, RowClickEventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            GridHitInfo ghi = gridView2.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            DataRow dr = gridView2.GetDataRow(e.RowHandle);
            string fkSiparisSablonlari = "0";
            string fkFirma = "0";
            if (ghi.Column.Name == "fkSiparisSablonlari")
            {
                fkSiparisSablonlari = dr["fkSiparisSablonlari"].ToString();
                DB.secilenfkSiparisSablonlari = 0;
            }
            else if (ghi.Column.Name == "fkSiparisSablonlari1")
            {
                fkSiparisSablonlari = dr["fkSiparisSablonlari1"].ToString();
                DB.secilenfkSiparisSablonlari = 1;
            }
            else if (ghi.Column.Name == "fkSiparisSablonlari2")
            {
                fkSiparisSablonlari = dr["fkSiparisSablonlari2"].ToString();
                DB.secilenfkSiparisSablonlari = 2;
            }
            else if (ghi.Column.Name == "fkSiparisSablonlari3")
            {
                fkSiparisSablonlari = dr["fkSiparisSablonlari3"].ToString();
                DB.secilenfkSiparisSablonlari = 3;
            }
            if (fkSiparisSablonlari == "") fkSiparisSablonlari = "0";
                Satis1Toplam.Tag = fkSiparisSablonlari;
                pkSatisBarkod.Text = fkSiparisSablonlari;
                yesilisikyeni();
        }
        void DurumListesi()
        {
            string sql = @"select distinct pkFirma,Firmaadi,CONVERT(bit, '0') AS Sec,fkPersoneller,0 as pkSiparisSablonlari from MusteriZiyaretGunleri mzg
inner join Firmalar f on f.pkFirma=mzg.fkFirma
where fkSiparisSablonlari is not null and mzg.VarYok=1";

            if (cbmusteridurum.SelectedIndex == 1)
                sql = @"select pkFirma,Firmaadi,CONVERT(bit, '0') AS Sec,isnull(fkPersoneller,f.fkPerTeslimEden) as fkPersoneller,fkSiparisSablonlari as pkSiparisSablonlari,mzg.pkMusteriZiyaretGunleri,g.GunAdi from MusteriZiyaretGunleri mzg
inner join Firmalar f on f.pkFirma=mzg.fkFirma
inner join Gunler g on g.pkGunler=mzg.fkGunler
where fkSiparisSablonlari is null and mzg.VarYok=1";

            else if (cbmusteridurum.SelectedIndex == 2)
                sql = @"select pkMusteriZiyaretGunleri, f.pkFirma,Firmaadi,CONVERT(bit, '0') AS Sec,szg.fkPersoneller,0 as pkSiparisSablonlari from MusteriZiyaretGunleri szg
inner join Firmalar f On f.pkFirma=szg.fkFirma
left join SiparisSablonlari ss on szg.fkSiparisSablonlari=ss.pkSiparisSablonlari
where szg.VarYok=1 and ss.pkSiparisSablonlari is null ";

            gcMusteriler.DataSource = DB.GetData(sql);
        }
        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbmusteridurum.SelectedIndex == 0)
                gcGunAdi.Visible = false;
            else
            {
                gcGunAdi.Visible = true;
                gcGunAdi.VisibleIndex = 2;
            }
            DurumListesi();
        }

        void OlmayanlariEkle()
        {
            string fkPersoneller = "0", fkGunler = "0";
            string fkFirma = Satis1Firma.Tag.ToString();
            DataTable dt = DB.GetData("select * from MusteriZiyaretGunleri where fkFirma=" + fkFirma);
            if (dt.Rows.Count == 0)
            {
                dt = DB.GetData("select * from Firmalar where pkFirma=" + fkFirma);
                fkPersoneller = dt.Rows[0]["fkPerTeslimEden"].ToString();
            }
            else
            {
                fkPersoneller = dt.Rows[0]["fkPersoneller"].ToString();
            }
            for (int j = 0; j < 7; j++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", fkFirma));
                list.Add(new SqlParameter("@fkGunler", (j + 1).ToString()));
                list.Add(new SqlParameter("@GunSonra", "7"));
                if (fkPersoneller == "")
                    list.Add(new SqlParameter("@fkPersoneller", DBNull.Value));
                else
                    list.Add(new SqlParameter("@fkPersoneller", fkPersoneller));
                if (DB.GetData("select * from MusteriZiyaretGunleri where fkGunler=" + (j + 1).ToString() + " and fkFirma=" + fkFirma).Rows.Count == 0)
                {
                    DB.ExecuteSQL(@"INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) 
  values(@fkFirma,@fkGunler,@GunSonra,@fkPersoneller)", list);
                }
            }
        }
        void MusteriBaslikGetir(string fkFirma)
        {
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + fkFirma);
            string firmadi = dt.Rows[0]["Firmaadi"].ToString();
            string ozelkod = dt.Rows[0]["OzelKod"].ToString();
            Satis1Firma.Tag = dt.Rows[0]["pkFirma"].ToString();
            Satis1Baslik.Text = ozelkod + "-" + firmadi;
            Satis1Baslik.ToolTip = Satis1Baslik.Text;
        }
        private void gridView3_RowClick(object sender, RowClickEventArgs e)
        {
            pkSatisBarkod.Text = "0";
            Satis1Toplam.Tag = "0";
            string fkFirma = "1";
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //Satis1Firma.Tag = dr["pkFirma"].ToString();
            fkFirma = dr["pkFirma"].ToString();
            pkSatisBarkod.Text = dr["pkSiparisSablonlari"].ToString();
            Satis1Toplam.Tag = pkSatisBarkod.Text;
            OlmayanlariEkle();
            MusteriBaslikGetir(fkFirma);
            MusteriZiyaretGunleriGetir();

            if (dr["fkPersoneller"].ToString() == "")
                lUEPersonel.EditValue = null;
            else
            lUEPersonel.EditValue = int.Parse(dr["fkPersoneller"].ToString());
            yesilisikyeni();
        }

        private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pkSatisBarkod.Text == "")
            {
                MessageBox.Show("Kopyalanacak Sipariş Bulunamadı");
                return;
            }
            DB.pkSiparisKopyala = int.Parse(pkSatisBarkod.Text);
        }

        void SatisKopyalaYapistir()
        {
             DataRow dr = null;
            if (gridView2.SelectedRowsCount > 0)
            {
            //    gridView2.GetSelectedRows(
                dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            //    string  id = dr["pkMusteriZiyaretGunleri"].ToString();
            //    //gridView1.GetSelectedRows()[0]); << other trying
            //   // windex.Text = row[0].ToString(); 
            }

            string kopyalanacakpkSatislar= DB.pkSiparisKopyala.ToString();
            DataTable dt =
            DB.GetData("select * from SiparisSablonlari where pkSiparisSablonlari=" + kopyalanacakpkSatislar);
            if(dt.Rows.Count==0) return; 
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", Satis1Firma.Tag.ToString()));
            list.Add(new SqlParameter("@fkPersoneller", dt.Rows[0]["fkPersoneller"].ToString()));
            list.Add(new SqlParameter("@OdemeSekli", dt.Rows[0]["OdemeSekli"].ToString()));

            string SiparisSablonlariYeniid = DB.ExecuteScalarSQL("INSERT INTO SiparisSablonlari (Tarih,fkFirma,fkPersoneller,OdemeSekli) " +
                " values(getdate(),@fkFirma,@fkPersoneller,@OdemeSekli) SELECT IDENT_CURRENT('SiparisSablonlari')", list);
            //şablon güncelle
            if (DB.secilenfkSiparisSablonlari == 0)
            {
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari=" +
                    SiparisSablonlariYeniid + " WHERE pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());

                    gridView2.SetFocusedRowCellValue("fkSiparisSablonlari", SiparisSablonlariYeniid);
            }
            else if (DB.secilenfkSiparisSablonlari == 1)
            {
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari1=" +
                    SiparisSablonlariYeniid + " WHERE pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());

                gridView2.SetFocusedRowCellValue("fkSiparisSablonlari1", SiparisSablonlariYeniid);
            }
            else if (DB.secilenfkSiparisSablonlari == 2)
            {
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari2=" +
                    SiparisSablonlariYeniid + " WHERE pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());

                gridView2.SetFocusedRowCellValue("fkSiparisSablonlari2", SiparisSablonlariYeniid);
            }
            else if (DB.secilenfkSiparisSablonlari == 3)
            {
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari3=" +
                    SiparisSablonlariYeniid + " WHERE pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());

                gridView2.SetFocusedRowCellValue("fkSiparisSablonlari3", SiparisSablonlariYeniid);
            }
            DataTable dtSatisDetay = DB.GetData("select * from SiparisSablonDetay where fkSiparisSablonlari=" + kopyalanacakpkSatislar);
            for (int i = 0; i < dtSatisDetay.Rows.Count; i++)
            {
                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@fkSiparisSablonlari", SiparisSablonlariYeniid));
                list2.Add(new SqlParameter("@fkStokKarti", dtSatisDetay.Rows[i]["fkStokKarti"].ToString()));
                list2.Add(new SqlParameter("@Adet", dtSatisDetay.Rows[i]["Adet"].ToString()));

                DB.ExecuteSQL("INSERT INTO SiparisSablonDetay (fkSiparisSablonlari,fkStokKarti,Adet)" +
                " values(@fkSiparisSablonlari,@fkStokKarti,@Adet)", list2);
            }
        }

        private void yapıştırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SatisKopyalaYapistir();
                //DB.ExecuteScalarSQL("INSERT INTO Satislar (Tarih,fkFirma,GelisNo,fkKullanici,)");
        }

        private void siparişGünleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSiparisSablonlari SiparisSablonlari = new frmSiparisSablonlari();
            SiparisSablonlari.ShowDialog();
            MusteriZiyaretGunleriGetir();
        }

        private void gridView2_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                DataRow dr = gridView2.GetDataRow(e.RowHandle);
                if (dr["VarYok"].ToString() == "False")
                {
                    e.Appearance.BackColor = Color.Black;

                    e.Appearance.ForeColor = Color.Black;

                }
            } 
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string VarYok = View.GetRowCellDisplayText(e.RowHandle, View.Columns["VarYok"]);
            //    //e.Appearance.BackColor = Satis1Toplam.BackColor;
            //    //if (iade.Trim() != "Seçim Yok")
            //    // {
            //    //   e.Appearance.BackColor = Color.DeepPink;
            //    //}
            //    if (VarYok.ToString() == "Seçili")
            //    {
            //        e.Appearance.BackColor = Color.Black;
            //    }
            //    else
            //        e.Appearance.BackColor = Color.DeepPink;
            //}
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmSiparisOlustur SiparisOlustur = new frmSiparisOlustur();
            SiparisOlustur.ShowDialog();
        }

        private void repositoryItemCheckEdit2_CheckStateChanged(object sender, EventArgs e)
        {
            string secilen =
                ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();
            if(gridView2.FocusedRowHandle<0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (secilen=="True")
            DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET VarYok=1 where pkMusteriZiyaretGunleri=" 
                + dr["pkMusteriZiyaretGunleri"].ToString());
            else
            DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET VarYok=0 where pkMusteriZiyaretGunleri="
               + dr["pkMusteriZiyaretGunleri"].ToString());
        }
        void SablonGrupGuncelle(string fkSablonGrup)
        {
            //lueSablonGrup.EditValue.ToString() +
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSablonGrup=" + fkSablonGrup + 
                    " where pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());
            }
        }

        private void cbOdemeSekli_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE SiparisSablonlari SET OdemeSekli='" + cbOdemeSekli.Text + "' WHERE pkSiparisSablonlari=" + pkSatisBarkod.Text);
            
        }

        private void gridView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0 ) return;
            GridHitInfo ghi = gridView2.CalcHitInfo(e.Location);
            if (ghi.RowHandle < 0) return;
            if (ghi.Column == null) return;
            DataRow dr = gridView2.GetDataRow(ghi.RowHandle);
            string fkSiparisSablonlari = "0";
            if (ghi.Column.Name == "fkSiparisSablonlari")
            {
                fkSiparisSablonlari = dr["fkSiparisSablonlari"].ToString();
                DB.secilenfkSiparisSablonlari = 0;
            }
            else if (ghi.Column.Name == "fkSiparisSablonlari1")
            {
                fkSiparisSablonlari = dr["fkSiparisSablonlari1"].ToString();
                DB.secilenfkSiparisSablonlari = 1;
            }
            else if (ghi.Column.Name == "fkSiparisSablonlari2")
            {
                fkSiparisSablonlari = dr["fkSiparisSablonlari2"].ToString();
                DB.secilenfkSiparisSablonlari = 2;
            }
            else if (ghi.Column.Name == "fkSiparisSablonlari3")
            {
                fkSiparisSablonlari = dr["fkSiparisSablonlari3"].ToString();
                DB.secilenfkSiparisSablonlari = 3;
            }
            if (fkSiparisSablonlari == "") fkSiparisSablonlari = "0";
            Satis1Toplam.Tag = fkSiparisSablonlari;
            pkSatisBarkod.Text = fkSiparisSablonlari;
            yesilisikyeni();
        }

        private void seçilenleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Şablonlar İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);
                if(dr["Sec"].ToString()=="True")
                {
                    if (cbmusteridurum.SelectedIndex == 1)
                    {

                        string pkMusteriZiyaretGunleri = dr["pkMusteriZiyaretGunleri"].ToString();
                        DB.ExecuteSQL("DELETE FROM MusteriZiyaretGunleri WHERE pkMusteriZiyaretGunleri=" + pkMusteriZiyaretGunleri);
                    }
                    else if (cbmusteridurum.SelectedIndex == 2)
                        {

                            string pkMusteriZiyaretGunleri = dr["pkMusteriZiyaretGunleri"].ToString();
                            DB.ExecuteSQL("DELETE FROM MusteriZiyaretGunleri WHERE pkMusteriZiyaretGunleri=" + pkMusteriZiyaretGunleri);
                    }
                    else
                    {
                        string pkSiparisSablonlari = dr["pkSiparisSablonlari"].ToString();
                        DB.ExecuteSQL("DELETE FROM SiparisSablonlari WHERE pkSiparisSablonlari=" + pkSiparisSablonlari);
                    }
                }
            }
            DurumListesi();
        }

        private void cbSec_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                gridView3.SetRowCellValue(i, "Sec", cbSec.Checked);
            }
        }
    }
}


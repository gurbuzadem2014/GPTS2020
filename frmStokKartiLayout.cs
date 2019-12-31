using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using System.Data.OleDb;
using GPTS.Include.Data;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using System.IO;
using System.Net;
using DevExpress.XtraEditors.Controls;
//using System.Media;
using DevExpress.XtraGrid.Views.Grid;
using System.Diagnostics;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmStokKartiLayout : DevExpress.XtraEditors.XtraForm
    {
        bool degisiklikvar = false;
        bool ilkresimyukle = false;
        bool isilkacilis = false;
        public frmStokKartiLayout()
        {
            InitializeComponent();
            //gcBarkodlar.Height = 22;
        }

        private void frmStokKarti_Load(object sender, EventArgs e)
        {
            isilkacilis = true;
            seDevirAdet.Enabled = !Degerler.DepoKullaniyorum;
            simpleButton32.Enabled = !Degerler.DepoKullaniyorum;

            #region şirket bilgileri
            DataTable dtSirketler = DB.GetData("select TOP 1 * from Sirketler with(nolock)");
            if (dtSirketler.Rows[0]["FaturaTipi"].ToString() != "")
            {
                cbFaturaTipi.SelectedIndex = int.Parse(dtSirketler.Rows[0]["FaturaTipi"].ToString());
            }

            if (dtSirketler.Rows[0]["StokTakibi"].ToString() == "True")
                cbStokTakibi.Checked = true;
            else
                cbStokTakibi.Checked = false;

            if (dtSirketler.Rows[0]["Birimi"].ToString() != "")
            {
                cbBirimi.Tag = dtSirketler.Rows[0]["Birimi"].ToString();
            }
            if (dtSirketler.Rows[0]["satisadedi_icindekiadetdir"].ToString() == "True")
                cbicindekimiktar.Checked = true;

            if (dtSirketler.Rows[0]["ikincifiyatgoster"].ToString() == "True")
            {
                lbFiyatBaslikid2.Visible = true;
                SatisFiyati2.Visible = true;
                SatisFiyati2KdvHaric.Visible = true;
                seSatisiskonto2.Visible = true;
                ceSatisFiyatiiskontolu2.Visible = true;
                txikincifiyatyuzde.Visible = true;
            }
            else
            {
                lbFiyatBaslikid2.Visible = false;
                SatisFiyati2.Visible = false;
                SatisFiyati2KdvHaric.Visible = false;
                seSatisiskonto2.Visible = false;
                ceSatisFiyatiiskontolu2.Visible = false;
                txikincifiyatyuzde.Visible = false;
            }
            #endregion

            Markalar();
            Gruplar();
            RenkGruplari();
            BedenGruplari();
            Tedarikciler();
            parabirimleri();
            
            OzelDurumlar();

            StokBilgileriGetir();

            StokKartiAciklamaGetir();
            //xtraTabPage2.PageVisible = false;

            Parametreler();

            if (this.Tag.ToString() == "2") //eğer satış faturası ekranından stok hareketleri geliyor ise
            {
                //xtraTabPage1.PageVisible = false;
                //xtraTabPage2.PageVisible = false;
                //xtraTabPage3.PageVisible = false;
            }

            //if (DB.fkKullanicilar == "1")
            //    spinEdit2.Enabled = true;
            //else
            //    spinEdit2.Enabled = false;

            //coklubarkod();

            StoklarBagli();

            EkranTasarimGetir();

            Yetkiler();

            //string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            //string dosya = exeDiz + "\\Sesler\\Yok.wav";
            //string path = "C:\\windows\\media\\ding.wav"; // Müzik adresi
            //if (File.Exists(dosya) && DB.stokkartisescal)
            if (Degerler.stokkartisescal && pkStokKarti.Text=="0")
            {
                Digerislemler.UrunYokSesCal();
                //SoundPlayer player = new SoundPlayer();
                //player.SoundLocation = dosya;// "chord.wav";
                //player.Play();
            }

            isilkacilis = false;
        }

        void EkranTasarimGetir()
        {
            string Genel = DB.Sektor;// "Genel";
            string layoutFileName = Application.StartupPath + "\\EkranTasarimlari\\" + Genel + "\\StokKarti.xml";
            if (System.IO.File.Exists(layoutFileName))
                layoutControl1.RestoreLayoutFromXml(layoutFileName);
            else
                MessageBox.Show(layoutFileName + " Dosya Bulunamadı!");
        }

        public Bitmap ScreenShot_orj()
        {
            Rectangle totalSize = Rectangle.Empty;

            foreach (Screen s in Screen.AllScreens)
                totalSize = Rectangle.Union(totalSize, s.Bounds);

            Bitmap screenShotBMP = new Bitmap(
                totalSize.Width, totalSize.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics screenShotGraphics = Graphics.FromImage(
                screenShotBMP);
            screenShotGraphics.CopyFromScreen(
                totalSize.Location.X,
                totalSize.Location.Y,
                0, 0, totalSize.Size,
                CopyPixelOperation.SourceCopy);

            return screenShotBMP;
        }

        public Bitmap ScreenShot()
        {
            Rectangle totalSize = Rectangle.Empty;

            int i =0;
            foreach (Screen s in Screen.AllScreens)
            {
                if(i==0)
                 totalSize = Rectangle.Union(totalSize, s.Bounds);

                i++;
            }

            Bitmap screenShotBMP = new Bitmap(
                200, 200,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics screenShotGraphics = Graphics.FromImage(
                screenShotBMP);
            screenShotGraphics.CopyFromScreen(
                Cursor.Position.X,
                Cursor.Position.Y,
                0, 0, totalSize.Size,
                CopyPixelOperation.SourceCopy);

            return screenShotBMP;
        }

        void Markalar()
        {
            lueMarka.Properties.DataSource = DB.GetData("SELECT * FROM Markalar with(nolock)");
            //lueMarka.Properties.ValueMember = "pkStokFiyatGruplari";
            //lueMarka.Properties.DisplayMember = "StokFiyatGrup";
            //luekurum.EditValue = 41;
        }

        void Modeller(int fkMarka)
        {
            DataTable dt = DB.GetData("SELECT * FROM Modeller with(nolock) where fkMarka=" + fkMarka);
            lueModel.Properties.DataSource = dt;
            //lueModel.Properties.ValueMember = "pkStokFiyatGruplari";
            //lueModel.Properties.DisplayMember = "StokFiyatGrup";
            //luekurum.EditValue = 41;
        }

        void Gruplar()
        {
            lueGruplar.Properties.DataSource = DB.GetData("SELECT * FROM StokGruplari with(nolock)");
        }

        void RenkGruplari()
        {
            lueRenk.Properties.DataSource = DB.GetData("SELECT * FROM RenkGrupKodu with(nolock)");
        }

        void BedenGruplari()
        {
            lueBeden.Properties.DataSource = DB.GetData("SELECT * FROM BedenGrupKodu with(nolock)");
        }

        void Tedarikciler()
        {
            lueTedarikciler.Properties.DataSource = DB.GetData("select pkTedarikciler,Firmaadi from Tedarikciler with(nolock)");
        }

        void StokBilgileriGetir()
        {
            if (DB.pkStokKarti == 0)
            {
                BtnKaydet.Text = "Kaydet \n[F9]";
            }
            else
            {
                BtnKaydet.Text = "Güncelle \n[F9]";
            }

            pkStokKarti.Text = DB.pkStokKarti.ToString();
            //select *,dbo.fon_StokMevcut(pkStokKarti) as fn_Mevcut from StokKarti
            DataTable dtStokKarti = DB.GetData("select * from StokKarti with(nolock) WHERE pkStokKarti=" + DB.pkStokKarti.ToString());
            if (dtStokKarti.Rows.Count == 0)
            {
                btnBarkodBas.Text = "Oto. Kod Ver";
                timer1.Enabled = true;
                return;
            }
            tEStokKod.Text = dtStokKarti.Rows[0]["Barcode"].ToString();
            tEStokKod.Properties.ReadOnly = true;
            tEStokadi.Text = dtStokKarti.Rows[0]["Stokadi"].ToString();
            Barkod.Text = dtStokKarti.Rows[0]["Barcode"].ToString();
            Barkod.Properties.ReadOnly = true;

            //Mevcut.Enabled = cbStokTakibi.Checked;

            Mevcut.EditValue = dtStokKarti.Rows[0]["Mevcut"].ToString();
            Mevcut.Tag = dtStokKarti.Rows[0]["Mevcut"].ToString();
            //Mevcut.EditValue = dtStokKarti.Rows[0]["fn_Mevcut"].ToString();
            //Mevcut.Tag = dtStokKarti.Rows[0]["fn_Mevcut"].ToString();            

            seMevcutFatura.EditValue = dtStokKarti.Rows[0]["MevcutFatura"].ToString();
            
            kritikmiktar.EditValue = dtStokKarti.Rows[0]["KritikMiktar"].ToString();
           
            if (dtStokKarti.Rows[0]["fkStokGrup"].ToString() != "")
                lueGruplar.EditValue = int.Parse(dtStokKarti.Rows[0]["fkStokGrup"].ToString());

            if (dtStokKarti.Rows[0]["fkStokAltGruplari"].ToString() != "")
            {
                lueAltGrup.EditValue = int.Parse(dtStokKarti.Rows[0]["fkStokAltGruplari"].ToString());
                lueAltGrup.Tag = dtStokKarti.Rows[0]["fkStokAltGruplari"].ToString();
            }
            if (dtStokKarti.Rows[0]["WebdeGoster"].ToString() == "True")
                webdegoster.Checked = true;
            if (dtStokKarti.Rows[0]["Aktif"].ToString() == "True")
                cbDurumu.SelectedIndex = 0;
            else
                cbDurumu.SelectedIndex = 1;

            RBGKodu.Text = dtStokKarti.Rows[0]["RBGKodu"].ToString();
            if (dtStokKarti.Rows[0]["fkBedenGrupKodu"].ToString() != "")
                lueBeden.EditValue = int.Parse(dtStokKarti.Rows[0]["fkBedenGrupKodu"].ToString());
            if (dtStokKarti.Rows[0]["fkRenkGrupKodu"].ToString() != "")
                lueRenk.EditValue = int.Parse(dtStokKarti.Rows[0]["fkRenkGrupKodu"].ToString());
            
            kdvorani.Text = dtStokKarti.Rows[0]["KdvOrani"].ToString();
            cbKdvOraniAlis.Text = dtStokKarti.Rows[0]["KdvOraniAlis"].ToString();
            
            if (dtStokKarti.Rows[0]["fkTedarikciler"].ToString() != "")
                lueTedarikciler.EditValue = int.Parse(dtStokKarti.Rows[0]["fkTedarikciler"].ToString());

            EtiketAciklama.Text = dtStokKarti.Rows[0]["EtiketAciklama"].ToString();
            cbBirimi.Text = dtStokKarti.Rows[0]["Stoktipi"].ToString();
            seicindekimiktar.EditValue = dtStokKarti.Rows[0]["KutuFiyat"].ToString();
            ureticikodu.Text = dtStokKarti.Rows[0]["UreticiKodu"].ToString();

            if (dtStokKarti.Rows[0]["fkMarka"] !=null && dtStokKarti.Rows[0]["fkMarka"].ToString() != "")
                lueMarka.EditValue = int.Parse(dtStokKarti.Rows[0]["fkMarka"].ToString());
            if (dtStokKarti.Rows[0]["fkModel"].ToString() != "" || dtStokKarti.Rows[0]["fkModel"].ToString() != "")
                lueModel.EditValue = int.Parse(dtStokKarti.Rows[0]["fkModel"].ToString());

            if (dtStokKarti.Rows[0]["fkBedenGrupKodu"].ToString() != "" || dtStokKarti.Rows[0]["fkBedenGrupKodu"].ToString() != "")
                lueBeden.EditValue = dtStokKarti.Rows[0]["fkBedenGrupKodu"].ToString();
            if (dtStokKarti.Rows[0]["fkRenkGrupKodu"].ToString() != "" || dtStokKarti.Rows[0]["fkRenkGrupKodu"].ToString() != "")
                lueRenk.EditValue = dtStokKarti.Rows[0]["fkRenkGrupKodu"].ToString();


            if (dtStokKarti.Rows[0]["KdvHaric"].ToString() == "True")
               cbFaturaTipi.SelectedIndex = 1;
            else
               cbFaturaTipi.SelectedIndex = 0;

            //alış fiyatı
            decimal alisfiyat = 0, alisfiyatikdvharic = 0;
            decimal.TryParse(dtStokKarti.Rows[0]["AlisFiyati"].ToString(), out alisfiyat);
            AlisFiyati.Value = alisfiyat;

            decimal.TryParse(dtStokKarti.Rows[0]["AlisFiyatiKdvHaric"].ToString(), out alisfiyatikdvharic);
            AlisFiyatiKdvHaric.Value = alisfiyatikdvharic;            

            SatisFiyatiGetir();
            
            pictureEdit1.Tag = "1";
            if (tEStokadi.Text.IndexOf("|") > -1)
            {
                tEStokadi.Text = tEStokadi.Text.Substring(0, tEStokadi.Text.IndexOf("|"));
            }
            decimal SatisAdedi = 1;
            decimal.TryParse(dtStokKarti.Rows[0]["SatisAdedi"].ToString(), out SatisAdedi);
            seSatisAdedi.Value = SatisAdedi;

            if (dtStokKarti.Rows[0]["alis_iskonto"].ToString() == "")
                seAlisiskonto.EditValue = "0";
            else
                seAlisiskonto.EditValue = dtStokKarti.Rows[0]["alis_iskonto"].ToString();

            if (dtStokKarti.Rows[0]["satis_iskonto"].ToString() == "")
                seSatisiskonto.EditValue = "0";
            else
                seSatisiskonto.EditValue = dtStokKarti.Rows[0]["satis_iskonto"].ToString();

            if (dtStokKarti.Rows[0]["satis_iskonto2"].ToString() == "")
                seSatisiskonto2.EditValue = "0";
            else
                seSatisiskonto2.EditValue = dtStokKarti.Rows[0]["satis_iskonto2"].ToString();


            if (dtStokKarti.Rows[0]["fkParaBirimi"].ToString() == "")
                lueParaBirimi.EditValue = "1";
            else
                lueParaBirimi.EditValue = int.Parse(dtStokKarti.Rows[0]["fkParaBirimi"].ToString());

            decimal SatisFiyatiDovizli = 0;
            decimal.TryParse(dtStokKarti.Rows[0]["SatisFiyatiDovizli"].ToString(), out SatisFiyatiDovizli);
            ceSatisFiyatiDovizli.Value = SatisFiyatiDovizli;

            if (dtStokKarti.Rows[0]["RafSure"].ToString() == "")
                seRafSure.EditValue = "0";
            else
                seRafSure.EditValue = dtStokKarti.Rows[0]["RafSure"].ToString();

            if (dtStokKarti.Rows[0]["fkOzelDurum"].ToString() == "")
                lueOzelDurum.EditValue = 0;
            else
                lueOzelDurum.EditValue = int.Parse(dtStokKarti.Rows[0]["fkOzelDurum"].ToString());

            

            ResimGetir();
        }

        void ResimGetir()
        {
            //pictureEdit1.EditValue = null;
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".png";
            if (File.Exists(dosya))
            {
                Image img = Image.FromFile(dosya);
                byte[] data = ByteImageConverter.ToByteArray(img, img.RawFormat);
                ilkresimyukle = true;
                pictureEdit1.EditValue = data;
                img.Dispose();
            }
            else
                pictureEdit1.EditValue = null;
        }

        void Parametreler()
        {
            DataTable dt = DB.GetData(@"SELECT pkParametreler, fkModul, Aciklama10, Aciklama50, Aktif, fkNesneTipi, SiraNo
                                        FROM Parametreler with(nolock) WHERE     (fkModul = 2)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Aciklama10"].ToString() == "lBeden" && dt.Rows[i]["Aktif"].ToString() == "True") lBeden.Checked = true;
                else if (dt.Rows[i]["Aciklama10"].ToString() == "lRenk" && dt.Rows[i]["Aktif"].ToString() == "True") lRenk.Checked = true;
                else if (dt.Rows[i]["Aciklama10"].ToString() == "lMarka" && dt.Rows[i]["Aktif"].ToString() == "True") lMarka.Checked = true;
                else if (dt.Rows[i]["Aciklama10"].ToString() == "lUretici" && dt.Rows[i]["Aktif"].ToString() == "True") lUreticiKodu.Checked = true;
                else if (dt.Rows[i]["Aciklama10"].ToString() == "lGrup" && dt.Rows[i]["Aktif"].ToString() == "True") lGrup.Checked = true;
            }
        }

        void Yetkiler()
        {
            string sql = @"SELECT ya.Yetki, p.Aciklama10,ya.Sayi,isnull(p.Aktif,0) as Aktif FROM  YetkiAlanlari ya with(nolock)  
INNER JOIN Parametreler p with(nolock) ON ya.fkParametreler = p.pkParametreler
WHERE ya.fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);

            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string aciklama = dtYetkiler.Rows[i]["Aciklama10"].ToString();
                bool yetki = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                string sayi = dtYetkiler.Rows[i]["Sayi"].ToString();
                //bool aktif = Convert.ToBoolean(dtYetkiler.Rows[i]["Aktif"]);

                if (aciklama == "AlisFiyati")
                {
                    if (yetki == false)
                    {
                        AlisFiyati.Properties.PasswordChar = '*';
                        AlisFiyatiKdvHaric.Properties.PasswordChar = '*';
                        ceAlisFiyatiiskontolu.Properties.PasswordChar = '*';
                        txtbirincifiyatorani.Properties.PasswordChar = '*';
                    }
                }
            }
        }

        void StoklarBagli()
        {
            gridControl1.DataSource = DB.GetData(@"select pkStokKartiBagli,sk.pkStokKarti,sk.Stokadi from StokKartiBagli skb with(nolock)
            inner join StokKarti  sk  with(nolock) on sk.pkStokKarti=skb.fkStokKartiBagli where skb.fkStokKarti=" + pkStokKarti.Text);
        }

        void StokKartiKaydet(int interval_saniye)
        {
            #region StokKartiKontrol
            if (tEStokadi.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ürün Adı Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tEStokadi.Focus();
                return;
            }
            if (cbKdvOraniAlis.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Alış Kdv Orani Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbKdvOraniAlis.Focus();
                return;
            }
            if (Barkod.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Barkod Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Barkod.Focus();
                return;
            }
            if (cbBirimi.Text == "PAKET" && seicindekimiktar.Value == 1)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Paket Adedi 1 Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                seicindekimiktar.Focus();
                return;
            }

            if (pkStokKarti.Text == "0" && barkodvarmi()) return;
            #endregion

            string sql="";
            ArrayList list = new ArrayList();
            if (ureticikodu.Text == "")
                ureticikodu.Text = Barkod.Text;
            list.Add(new SqlParameter("@StokKod", ureticikodu.Text));
            list.Add(new SqlParameter("@Barcode", Barkod.Text));
            string StokAdi = "";
            //24.06.2013 A.G. kaldırıldı adının önündekini sil
            if (tEStokadi.Text.IndexOf('|') == -1 && (lMarka.Checked == true || lRenk.Checked == true || lBeden.Checked == true || lUreticiKodu.Checked == true || lGrup.Checked == true))
                {
                    StokAdi = tEStokadi.Text + "|";
                    if (lMarka.Checked && lueMarka.EditValue != null) StokAdi = StokAdi + " " + lueMarka.Text;
                    if (lRenk.Checked && lueRenk.EditValue != null) StokAdi = StokAdi + " " + lueRenk.Text;
                    if (lBeden.Checked && lueBeden.EditValue != null) StokAdi = StokAdi + " " + lueBeden.Text;
                    if (lUreticiKodu.Checked && ureticikodu.EditValue != null) StokAdi = StokAdi + " " + ureticikodu.Text;
                    if (lGrup.Checked && lueGruplar.EditValue != null) StokAdi = StokAdi + " " + lueGruplar.Text;
                    tEStokadi.Text = StokAdi;
                }
            list.Add(new SqlParameter("@Stokadi", tEStokadi.Text));

            #region Stok Kartı Fiyatları Alış Satış
            if (AlisFiyati.Value == 0) AlisFiyati.EditValue = "0.001";
            if (AlisFiyati.EditValue==null)
                list.Add(new SqlParameter("@AlisFiyati", "0.001"));
            else
                list.Add(new SqlParameter("@AlisFiyati", AlisFiyati.EditValue.ToString().Replace(",",".")));

            if (AlisFiyatiKdvHaric.EditValue == null)
                list.Add(new SqlParameter("@AlisFiyatiKdvHaric", "0.001"));
            else
                list.Add(new SqlParameter("@AlisFiyatiKdvHaric", AlisFiyatiKdvHaric.EditValue.ToString().Replace(",", ".")));

            if (SatisFiyati1.EditValue == null)
                list.Add(new SqlParameter("@SatisFiyati", "0.00"));
            else
                list.Add(new SqlParameter("@SatisFiyati", SatisFiyati1.EditValue.ToString().Replace(",", ".")));
            //SatisFiyatiKdvHaric
            if (SatisFiyatikdvharic.EditValue == null)
                list.Add(new SqlParameter("@SatisFiyatiKdvHaric", "0.00"));
            else
                list.Add(new SqlParameter("@SatisFiyatiKdvHaric", SatisFiyatikdvharic.EditValue.ToString().Replace(",", ".")));

            #endregion

            if (kritikmiktar.EditValue == null)
                list.Add(new SqlParameter("@KritikMiktar", "0"));
            else
                list.Add(new SqlParameter("@KritikMiktar", kritikmiktar.Value));
            if (lueMarka.EditValue==null)
                list.Add(new SqlParameter("@fkMarka","0"));
            else
                list.Add(new SqlParameter("@fkMarka", lueMarka.EditValue));
            if (lueModel.EditValue==null)
                list.Add(new SqlParameter("@fkModel", "0"));
            else
                list.Add(new SqlParameter("@fkModel", lueModel.EditValue));
            if (lueGruplar.Text=="" || lueGruplar.Text=="Seçiniz...")
                list.Add(new SqlParameter("@fkStokGrup", "0"));
            else
                list.Add(new SqlParameter("@fkStokGrup", lueGruplar.EditValue));
            if (lueAltGrup.Text == "" || lueGruplar.Text == "Seçiniz...")
                list.Add(new SqlParameter("@fkStokAltGruplari", "0"));
            else
                list.Add(new SqlParameter("@fkStokAltGruplari", lueAltGrup.EditValue));

            if (lueBeden.EditValue==null)
                list.Add(new SqlParameter("@fkBedenGrupKodu", DBNull.Value));
            else
                list.Add(new SqlParameter("@fkBedenGrupKodu", lueBeden.EditValue));

            if(lueRenk.EditValue==null)
                list.Add(new SqlParameter("@fkRenkGrupKodu", DBNull.Value));
            else
                list.Add(new SqlParameter("@fkRenkGrupKodu", lueRenk.EditValue));
            list.Add(new SqlParameter("@Stoktipi", cbBirimi.Text));
            
            list.Add(new SqlParameter("@RBGKodu", RBGKodu.Text));
            if (cbDurumu.SelectedIndex == 0)
                list.Add(new SqlParameter("@Aktif", true));
            else
                list.Add(new SqlParameter("@Aktif", false));

            if (kdvorani.Text == "")
                kdvorani.Text = cbKdvOraniAlis.Text;
            list.Add(new SqlParameter("@KdvOrani", kdvorani.Text));
            list.Add(new SqlParameter("@KdvOraniAlis", cbKdvOraniAlis.Text));

            if (lueTedarikciler.EditValue==null)
                list.Add(new SqlParameter("@fkTedarikciler", DBNull.Value));
            else
                list.Add(new SqlParameter("@fkTedarikciler", lueTedarikciler.EditValue.ToString()));

            list.Add(new SqlParameter("@EtiketAciklama", EtiketAciklama.Text));
            
            if (seicindekimiktar.EditValue == null && (cbBirimi.Text != "PAKET" || cbBirimi.Text != "KOLİ"))
                list.Add(new SqlParameter("@KutuFiyat", "1"));
            else
                list.Add(new SqlParameter("@KutuFiyat", seicindekimiktar.Value.ToString()));

            list.Add(new SqlParameter("@Mevcut", Mevcut.Value));
            list.Add(new SqlParameter("@MevcutFatura", seMevcutFatura.Value));
            
            list.Add(new SqlParameter("@UreticiKodu", ureticikodu.Text));

            list.Add(new SqlParameter("@alis_iskonto", seAlisiskonto.Value));
            list.Add(new SqlParameter("@satis_iskonto", seSatisiskonto.Value));
            list.Add(new SqlParameter("@satis_iskonto2", seSatisiskonto2.Value));

            list.Add(new SqlParameter("@KdvHaric", cbFaturaTipi.SelectedIndex));
            if (seSatisAdedi.Value == 0)
                seSatisAdedi.Value = 1;
            list.Add(new SqlParameter("@SatisAdedi", seSatisAdedi.Value));
            //list.Add(new SqlParameter("@satisadedi_icindekiadetdir", cbicindekimiktar.Checked));

            if (lueParaBirimi.EditValue==null)
              list.Add(new SqlParameter("@fkParaBirimi", "1"));
            else
              list.Add(new SqlParameter("@fkParaBirimi", lueParaBirimi.EditValue));

            if (ceSatisFiyatiDovizli.EditValue==null)
                list.Add(new SqlParameter("@SatisFiyatiDovizli", "1"));
            else
                list.Add(new SqlParameter("@SatisFiyatiDovizli", ceSatisFiyatiDovizli.EditValue.ToString().Replace(",",".")));

            list.Add(new SqlParameter("@RafSure", seRafSure.Value));

            if (lueOzelDurum.EditValue == null)
                list.Add(new SqlParameter("@fkOzelDurum", DBNull.Value));
            else
                list.Add(new SqlParameter("@fkOzelDurum", lueOzelDurum.EditValue));
            

            string sonuc = "";
            if (pkStokKarti.Text == "0" || pkStokKarti.Text == "-1")
            {
                sql = @"INSERT INTO StokKarti (StokKod,Stoktipi,Stokadi,Barcode,
                        AlisFiyati,AlisFiyatiKdvHaric,SatisFiyati,SatisFiyatiKdvHaric,
                        fkMarka,fkModel,fkStokGrup,fkStokAltGruplari,fkBedenGrupKodu,fkRenkGrupKodu,
                        KritikMiktar,Aktif,CikisAdet,KdvOrani,KdvOraniAlis,RBGKodu,fkTedarikciler,EtiketAciklama,
                        Mevcut,EklemeTarihi,KutuFiyat,UreticiKodu,alis_iskonto,satis_iskonto,satis_iskonto2,KdvHaric,
                        SatisAdedi,MevcutFatura,fkParaBirimi,SatisFiyatiDovizli,RafSure,fkOzelDurum) 
                        VALUES(@StokKod,@Stoktipi,@Stokadi,@Barcode,
                        @AlisFiyati,@AlisFiyatiKdvHaric,@SatisFiyati,@SatisFiyatiKdvHaric,
                        @fkMarka,@fkModel,@fkStokGrup,@fkStokAltGruplari,@fkBedenGrupKodu,@fkRenkGrupKodu,
                        @KritikMiktar,@Aktif,0,@KdvOrani,@KdvOraniAlis,@RBGKodu,@fkTedarikciler,@EtiketAciklama,
                        @Mevcut,getdate(),@KutuFiyat,@UreticiKodu,@alis_iskonto,@satis_iskonto,@satis_iskonto2,
                        @KdvHaric,@SatisAdedi,@MevcutFatura,@fkParaBirimi,@SatisFiyatiDovizli,@RafSure,@fkOzelDurum)
                        SELECT IDENT_CURRENT('StokKarti')";
                sonuc = DB.ExecuteScalarSQL(sql, list);
                if (sonuc == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Boş mu hatamı", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (sonuc.Substring(0, 1) == "H")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz." + "\n" + sonuc, Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    pkStokKarti.Text = sonuc;
                    BtnKaydet.Text = "Güncelle \n[F9]";
                    DB.ExecuteSQL("update Kodver set Rakam=Rakam+1 where pkKodver=" + tEStokKod.Tag.ToString());
                    DB.ExecuteSQL("update StokKarti set Mevcut=" + seDevirAdet.Value.ToString().Replace(",", ".") + " where pkStokKarti=" + pkStokKarti.Text);
                    //ana depoyada devir mevcut ekle
                    DB.ExecuteSQL("insert into StokKartiDepo (fkStokKarti,fkDepolar,MevcutAdet) values("+
                        pkStokKarti.Text+",1," + seDevirAdet.Value.ToString().Replace(",", ".")+")");

                    simpleButton20.Enabled = false;
                    simpleButton22.Enabled = false;

                    OlmayanFiyatBasliklariniEkle();
                }
            }
            else
            {
                sql = @"UPDATE StokKarti SET HizliSatis=0,StokKod=@StokKod,Stoktipi=@Stoktipi,Stokadi=@Stokadi,Barcode=@Barcode,
                AlisFiyati=@AlisFiyati,AlisFiyatiKdvHaric=@AlisFiyatiKdvHaric,SatisFiyati=@SatisFiyati,SatisFiyatikdvharic=@SatisFiyatikdvharic,
                fkMarka=@fkMarka,fkModel=@fkModel,fkStokGrup=@fkStokGrup,fkStokAltGruplari=@fkStokAltGruplari,
                fkRenkGrupKodu=@fkRenkGrupKodu,fkBedenGrupKodu=@fkBedenGrupKodu,KritikMiktar=@KritikMiktar,Aktif=@Aktif,RBGKodu=@RBGKodu,
                KdvOrani=@KdvOrani,KdvOraniAlis=@KdvOraniAlis,fkTedarikciler=@fkTedarikciler,EtiketAciklama=@EtiketAciklama,KutuFiyat=@KutuFiyat,UreticiKodu=@UreticiKodu,
                Mevcut=@Mevcut,GuncellemeTarihi=getdate(),alis_iskonto=@alis_iskonto,satis_iskonto=@satis_iskonto,satis_iskonto2=@satis_iskonto2,
                KdvHaric=@KdvHaric,SatisAdedi=@SatisAdedi,MevcutFatura=@MevcutFatura,Aktarildi=0,fkParaBirimi=@fkParaBirimi,SatisFiyatiDovizli=@SatisFiyatiDovizli,
                RafSure=@RafSure,fkOzelDurum=@fkOzelDurum WHERE pkStokKarti=" + pkStokKarti.Text;
                sonuc = DB.ExecuteSQL(sql, list);
                if (sonuc.Substring(0, 1) == "H")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz." + "\n" + sonuc, Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (tEStokadi.OldEditValue != null)
                    DB.ExecuteSQL("UPDATE dbo.StokKarti SET HizliSatisAdi=substring(Stokadi,1,19) WHERE pkStokKarti="+ pkStokKarti.Text );
             }

            btnBarkodBas.Enabled = true;

            degisiklikvar = false;       
            if (sonuc == "0")
            {
                if (interval_saniye>0)
                  formislemleri.Mesajform("Bilgiler Güncellendi.", "S", interval_saniye);
            }
            else
            {
                formislemleri.Mesajform("Yeni Stok Kartı Kaydedildi.", "S", interval_saniye);
                DB.ExecuteSQL("UPDATE dbo.StokKarti SET HizliSatisAdi=substring(Stokadi,1,19) WHERE HizliSatisAdi is null");
            }

            SatisFiyatlariniGuncelle();

            BarkodlariKaydet();

            ResimKaydet();

            StokKartiAciklamaKaydet();

            #region Mevcut StokDevir
            //if (spinEdit2.Tag=="1")
            //{
            //    simpleButton32_Click(null, null);
            //}
            #endregion

        }

        void StokKartiAciklamaKaydet()
        {
            //if(memoEdit1.Tag.ToString()=="0") return;
            string sql = "";
            DataTable dt =
            DB.GetData("select * from StokKartiAciklama with(nolock) where fkStokKarti=" + pkStokKarti.Text);
            ArrayList list = new ArrayList();
            if (dt.Rows.Count == 0)
            {
                sql = "insert into StokKartiAciklama (fkStokKarti,Tarih,Aciklama) values(@fkStokKarti,getdate(),@Aciklama)";
            }
            else
            {
                sql = "update StokKartiAciklama set  Tarih=getdate(),Aciklama=@Aciklama where fkStokKarti=@fkStokKarti";
            }

            list.Add(new SqlParameter("@fkStokKarti", pkStokKarti.Text));
            list.Add(new SqlParameter("@Aciklama", memoEdit1.Text));
            DB.ExecuteSQL(sql, list);
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            StokKartiKaydet(200);
        }

        void StokDevirKaydet_depo_yapilinca_yapilacak()
        {
            //if (ceSimdikiMevcut.EditValue == null)
            //{
            //    ceSimdikiMevcut.Focus();
            //    return;
            //}

            //DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Devir İşlemi Yapılacak Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (secim == DialogResult.No) return;

            //önce sıfırla
            //            if (ceMevcut.Value != 0)
            //            {
            //                string sql1 = @"INSERT INTO StokDevir (fkStokKarti,Tarih,Aciklama,OncekiAdet,DevirAdedi,fkKullanicilar,islemTarihi)
            //                values(@fkStokKarti,@Tarih,@Aciklama,@OncekiAdet,@DevirAdedi,@fkKullanicilar,getdate())";

            //                ArrayList list1 = new ArrayList();
            //                list1.Add(new SqlParameter("@fkStokKarti", fkStokKarti.Tag.ToString()));
            //                list1.Add(new SqlParameter("@Tarih", deDevirTarihi.DateTime));
            //                list1.Add(new SqlParameter("@Aciklama", "Stok Mevcut Sıfırlandı"));
            //                list1.Add(new SqlParameter("@OncekiAdet", ceMevcut.Tag.ToString().Replace(",", ".")));
            //                decimal fark = ceSimdikiMevcut.Value - ceBakiyeKaydedilecek.Value;
            //                list1.Add(new SqlParameter("@DevirAdedi", fark.ToString().Replace(",", ".")));
            //                list1.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

            //                string sonuc1 = DB.ExecuteSQL(sql1, list1);
            //                if (sonuc1 != "0")
            //                {
            //                    MessageBox.Show("Hata Oluştu Sıfırlanamadı");
            //                    return;
            //                }
            //            }
            string sql = @"INSERT INTO StokDevir (fkStokKarti,Tarih,Aciklama,OncekiAdet,DevirAdedi,fkKullanicilar,islemTarihi)
                    values(@fkStokKarti,@Tarih,@Aciklama,@OncekiAdet,@DevirAdedi,@fkKullanicilar,getdate())";

            ArrayList list0 = new ArrayList();
            list0.Add(new SqlParameter("@fkStokKarti", pkStokKarti.Text));
            list0.Add(new SqlParameter("@Tarih", DateTime.Now));
            list0.Add(new SqlParameter("@Aciklama", "Stok Kartı Devir Bakiye"));
            list0.Add(new SqlParameter("@OncekiAdet", Mevcut.Tag.ToString().Replace(",", ".")));

            decimal mev = 0,deviradet=0;

            decimal.TryParse(Mevcut.Tag.ToString(), out mev);

            //if (Mevcut.Tag.ToString() == "0")
            //    deviradet = Mevcut.Value;
            //else
                deviradet = mev - Mevcut.Value;


            list0.Add(new SqlParameter("@DevirAdedi", deviradet.ToString().Replace(",", ".")));
            list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

            string sonuc = DB.ExecuteSQL(sql, list0);

            if (sonuc == "0")
                Mevcut.Tag = Mevcut.Value.ToString();
            //{
               // DB.ExecuteSQL("update StokKarti set Mevcut=" + ceSimdikiMevcut.EditValue.ToString().Replace(",", ".") +
                 //   " where pkStokKarti=" + fkStokKarti.Tag.ToString());
                //ceBakiye.Value = 0;
                // MessageBox.Show("Hata Oluştu Tekrar deneyiniz");
                //return;
            //}
            //ceSimdikiMevcut.Value = 0;
            //StokKartiGetir();
            //StokDevirGetir();

            //Close();
        }

        void SatisFiyatlariniGuncelle()
        {
            DataTable dtBaslik = DB.GetData("select * from SatisFiyatlariBaslik with(nolock) order by Tur");
            for (int i = 0; i < dtBaslik.Rows.Count; i++)
            {

                if (dtBaslik.Rows[i]["Tur"].ToString() == "1")
                {
                    DB.ExecuteSQL("UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvli=" + SatisFiyati1.Text.Replace(",", ".") +
                        " WHERE  fkSatisFiyatlariBaslik=" + dtBaslik.Rows[i]["pkSatisFiyatlariBaslik"].ToString()+
                        " and fkStokKarti=" + pkStokKarti.Text);
                }
                    //2.fiyat kredi kartı olabilir
                else if (dtBaslik.Rows[i]["Tur"].ToString() == "2")
                {
                    if (SatisFiyati2.Visible == true)
                        DB.ExecuteSQL("UPDATE SatisFiyatlari SET Aktif=1,SatisFiyatiKdvli=" + SatisFiyati2.Text.Replace(",", ".") +
                            " WHERE  fkSatisFiyatlariBaslik=" + dtBaslik.Rows[i]["pkSatisFiyatlariBaslik"].ToString() +
                            " and fkStokKarti=" + pkStokKarti.Text);
                    else
                    {
                        if (SatisFiyati2.Text!="")
                        DB.ExecuteSQL("UPDATE SatisFiyatlari SET Aktif=0,SatisFiyatiKdvli=" + SatisFiyati2.Text.Replace(",", ".") +
                        " WHERE  fkSatisFiyatlariBaslik=" + dtBaslik.Rows[i]["pkSatisFiyatlariBaslik"].ToString() +
                        " and fkStokKarti=" + pkStokKarti.Text);
                    }
                }
            }
        }

        void OlmayanFiyatBasliklariniEkle()
        {
            DataTable dt = DB.GetData("Select * FROM SatisFiyatlariBaslik with(nolock) order by Tur");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pkSatisFiyatlariBaslik = dt.Rows[i]["pkSatisFiyatlariBaslik"].ToString();
                string SatisFiyatiKdvli = SatisFiyati1.Value.ToString().Replace(",", ".");
                string SatisFiyatiKdvli2 ="0";
                if (SatisFiyati2.Visible == false)
                {
                    SatisFiyati2.Value = SatisFiyati1.Value;
                    SatisFiyatiKdvli2 = SatisFiyati2.Value.ToString().Replace(",", ".");
                }
                else
                    SatisFiyatiKdvli2 = SatisFiyati2.Value.ToString().Replace(",", ".");
                if (SatisFiyatiKdvli2 == "0")
                {
                    SatisFiyati2.Value = SatisFiyati1.Value;
                    SatisFiyatiKdvli2 = SatisFiyatiKdvli;
                }
                string tur = dt.Rows[i]["Tur"].ToString();
                string Aktif = dt.Rows[i]["Aktif"].ToString();
               
                //else if (Aktif == "True" && SatisFiyati2.Visible == true)
                  //  Aktif = "1";
                //else
                  //  Aktif = "0";
                if (Aktif == "True") Aktif = "1"; else Aktif = "0";
                if (tur != "1" && SatisFiyati2.Visible == false)
                    Aktif = "0";
                if (tur == "1")  Aktif = "1";

                string sql = "";
                if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" + pkStokKarti.Text + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                {
                    if (tur == "1")
                        sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                            " VALUES(" + pkStokKarti.Text + "," + pkSatisFiyatlariBaslik + "," + SatisFiyatiKdvli + "," + SatisFiyatiKdvli + "," + Aktif + ")";
                    else
                    {
                        sql = "INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvsiz,SatisFiyatiKdvli,Aktif)" +
                           " VALUES(" + pkStokKarti.Text + "," + pkSatisFiyatlariBaslik + "," + SatisFiyatiKdvli2 + "," + SatisFiyatiKdvli2 + "," + Aktif + ")";
                    }
                }
                else
                {
                    if (tur == "1")
                        sql = "UPDATE SatisFiyatlari SET Aktif="+Aktif+"SatisFiyatiKdvsiz=" + SatisFiyatiKdvli + ",SatisFiyatiKdvli=" + SatisFiyatiKdvli +
                         " WHERE fkStokKarti=" + pkStokKarti.Text + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                    else
                        sql = "UPDATE SatisFiyatlari SET Aktif="+Aktif+",SatisFiyatiKdvsiz=" + SatisFiyatiKdvli2 + ",SatisFiyatiKdvli=" + SatisFiyatiKdvli2 +
                         " WHERE fkStokKarti=" + pkStokKarti.Text + " AND fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik;
                }
                DB.ExecuteSQL(sql);
            }
        }
        
        void adiparcala()
        {
            string str = tEStokadi.Text;
            string sql = "";
            string[] dizi = str.Split(' ');
            for (int i = 0; i < dizi.Length; i++)
            {
                sql = "UPDATE StokKarti SET AraAdi" + (i + 1).ToString() + "='" + dizi[i].ToString() + "' where pkStokKarti=" + pkStokKarti.Text;
                DB.ExecuteSQL(sql);
                if (i > 4) break;
            }
        }

        private void tEpkStokKarti_EditValueChanged(object sender, EventArgs e)
        {
            if (pkStokKarti.Text == "0")
                BtnKaydet.Text = "Kaydet \n[F9]";
            else
                BtnKaydet.Text = "Güncelle \n[F9]";
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //alisdangeldi.AccessibleDescription = "";//"alisdangeldihayir":
            pkStokKarti.Text = "0";
            DB.pkStokKarti = 0;
            
            if (targetGrid != null)
                targetGrid.Visible = false;
            Barkod.Text = "";
            tEStokadi.Text = "";
            simpleButton20.Enabled = true;
            simpleButton22.Enabled = true;
            tEStokKod.Text = "";
            tEStokKod.Properties.ReadOnly = false;
            BtnKaydet.Text = "Kaydet[F9]";

            AlisFiyatiKdvHaric.Value = 0;
            SatisFiyati1.Value = 0;
            SatisFiyati2.Value = 0;
            SatisFiyatikdvharic.Value = 0;
            SatisFiyati2KdvHaric.Value = 0;
            txtbirincifiyatorani.EditValue = 0;
            txikincifiyatyuzde.EditValue = 0;
            ceAlisFiyatiiskontolu.EditValue = 0;
            ceSatisFiyatiiskontolu.EditValue = 0;

            AlisFiyati.Value = 0;
            cbDurumu.SelectedIndex = 0;
            //if (kdvorani.EditValue == null)
            //    kdvorani.Text = Degerler.kdvorani.ToString();
            //if (cbKdvOraniAlis.EditValue == null)

            kdvorani.Text = Degerler.kdvorani.ToString();
            cbKdvOraniAlis.Text = Degerler.kdvorani.ToString();
            //kdvorani.Text = Degerler.kdvorani.ToString();

            Mevcut.EditValue = 0;
            Mevcut.Tag = 0;
            lueGruplar.EditValue = null;
            lueAltGrup.EditValue = null;
            lueTedarikciler.EditValue = null;
            cbBirimi.SelectedIndex = 0;
            if (cbBirimi.Tag != null && cbBirimi.Tag.ToString() != "")
                 cbBirimi.Text=cbBirimi.Tag.ToString();
            EtiketAciklama.EditValue = null;
            ureticikodu.Text = "";
            lueMarka.EditValue = null;
            lueRenk.EditValue = null;
            lueBeden.EditValue = null;
            seAlisiskonto.Value = 0;
            seSatisiskonto.Value = 0;
            seRafSure.Value = 30;
            pictureEdit1.Image = null;
            seDevirAdet.Value = 0;
            
            cbBirimi.Text = Degerler.stokbirimi;

            seSatisAdedi.Value = 1;

            Barkodlar();

            //if (lblBarkod.AccessibleDescription == "alisdangeldihayir")
            //{
            //    Barkod.Properties.ReadOnly = false;
            //    Barkod.Text = lblBarkod.Tag.ToString();
            //    Barkod.Focus();
            //}
            //else
            //{
            //    lblBarkod.AccessibleDescription = "alisdangeldihayir";
            //    Barkod.Properties.ReadOnly = false;
            //    Barkod.Text = lblBarkod.Tag.ToString();
            //    tEStokadi.Focus();
            //}

            seDevirAdet.Tag = "1";
            Barkod.Focus();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmGrupTanimlari GrupTanimlari = new frmGrupTanimlari();
            GrupTanimlari.ShowDialog();
            //FiyatGruplari();
        }
 
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (pkStokKarti.Text == "0") return;
            if (DB.GetData("select * from SatisDetay with(nolock) where fkStokKarti=" + pkStokKarti.Text).Rows.Count > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Stok Kartı Hareket Gördüğü için Silemezsiniz! \n Stoğun Durumunu Pasif Ürün Olarak Seçebilrsiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            return;

            //Teboscreen.ControlPanel ekrancek = new Teboscreen.ControlPanel();
            //ekrancek.ShowDialog();

            DB.ExecuteSQL("Delete From StokKarti where pkStokKarti=" + pkStokKarti.Text);

            DB.ExecuteSQL("Delete From HizliStokSatis where fkStokKarti=" + pkStokKarti.Text);

            formislemleri.Mesajform("Ürün Bilgileri Silindi.", "S", 200);

            DB.ExecuteSQL("Delete From StokKartiBarkodlar where fkStokKarti=" + pkStokKarti.Text);

            //coklubarkod();

            Close();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            DB.pkStokKarti = 0;
            pkStokKarti.Text = "0";
            BtnKaydet.Text = "Kaydet \n[F9]";
            pkStokKarti.Text = "0";
            simpleButton20.Enabled = true;
            simpleButton22.Enabled = true;
           
            tEStokKod.Properties.ReadOnly = false;
            Barkod.Text = "";
            
           // kritikmiktar.Value = 1;
            Mevcut.Value = 0;
            Barkod.Properties.ReadOnly = false;
            
            Mevcut.Enabled = true;
            if (lueMarka.Text!="") tEStokadi.Text = tEStokadi.Text.Replace(lueMarka.Text, "");
            if (lueRenk.Text != "") tEStokadi.Text = tEStokadi.Text.Replace(lueRenk.Text, "");
            if (lueBeden.Text != "") tEStokadi.Text = tEStokadi.Text.Replace(lueBeden.Text, "");           
            if (ureticikodu.Text != "") tEStokadi.Text = tEStokadi.Text.Replace(ureticikodu.Text, "");
            tEStokadi.Text = tEStokadi.Text.Replace("|", "");
            ureticikodu.Text = "";
            tEStokKod.Text = "";
            //ureticikodu.Text = "";
            tEStokadi.Text = tEStokadi.Text.Trim();

            Barkodlar();

            Barkod.Focus();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            frmStokGrupKarti StokGrupKarti = new frmStokGrupKarti();
            StokGrupKarti.ShowDialog();
            Gruplar();
        }

        private void lueMarka_EditValueChanged(object sender, EventArgs e)
        {
            if (lueMarka.EditValue!=null)
            Modeller(int.Parse(lueMarka.EditValue.ToString()));
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (DB.pkStokKarti > 0) return;
            DataTable dtkod = DB.GetData("select Barcode from StokKarti with(nolock)");
            for (int i = 0; i < dtkod.Rows.Count; i++)
            {
                DataTable dt = DB.GetData("select * from Kodver with(nolock) where AktifSec=1");
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Varsayılan Seçiniz");
                    simpleButton22_Click(sender, e);//kodver ekranını aç
                    break;
                }
                Barkod.Text = dt.Rows[0]["Rakam"].ToString();
                Barkod.Tag = dt.Rows[0]["pkKodver"].ToString();
                if (DB.GetData("select Barcode from StokKarti with(nolock) where Barcode='" + Barkod.Text + "'").Rows.Count == 0)
                    break;
                else
                {
                    DB.ExecuteSQL("UPDATE Kodver SET Rakam=Rakam+1 where AktifSec=1");
                    continue;
                }
            }
            if (Barkod.Text == "")
            {
                DataTable dt = DB.GetData("select * from Kodver with(nolock) where AktifSec=1");
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Varsayılan Seçiniz");
                    simpleButton22_Click(sender, e);//kodver ekranını aç
                    return;
                }
                Barkod.Text = dt.Rows[0]["Rakam"].ToString();
                DB.ExecuteSQL("UPDATE Kodver SET Rakam=Rakam+1 where AktifSec=1");
               // MessageBox.Show("Tavuk Fiyat Grubunda Boş yer bulunmamaktadır!\n Lütfen tavuk fiyat grubunun başlanğıç Kodunu Değiştiriniz.");
            }
            //tEStokadi.Focus();
            ureticikodu.Focus();
        }

        void RaporOnizleme(bool Disigner)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            Barkod.DataSource = DB.GetData("select * from StokKarti where pkStokKarti="+ DB.pkStokKarti.ToString()) ;
           
            RaporDosyasi = exedizini + "\\Raporlar\\Barkod.repx";
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
       
        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));
        }

        public virtual void InitPrintTool(PrintTool tool)
        {
            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);
        }

        private void buttonEdit1_Leave(object sender, EventArgs e)
        {
            if (tEStokKod.Text == "")
                tEStokKod.Text = Barkod.Text;
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            Barkod.DataSource = DB.GetData("select * from StokKarti with(nolock) where pkStokKarti=" + DB.pkStokKarti.ToString());

            RaporDosyasi = exedizini + "\\Raporlar\\Barkod.repx";
            //rapor.DataSource = gCPerHareketleri.DataSource;
            //rapor.CreateDocument();
            Barkod.LoadLayout(RaporDosyasi);
            Barkod.PrintDialog(); //bumu
            //Barkod.Print(); //bumu
        }

        void BarkodYazdir(bool dizayn)
        {
            //DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            xrCariHareket xrBarkod = new xrCariHareket();
            for (int i = 0; i < CopyAdet.Value; i++)
            {
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
                r.Aciklama as Renk,
                0 as GizliFiyat,EtiketAciklama,
                PB.kodu,t.Firmaadi as TedarikciAdi
                
                from StokKarti sk with(nolock)
left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
                left join ParaBirimi PB with(nolock) ON PB.pkParaBirimi = sk.fkParaBirimi
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

                 where pkStokKarti=" + pkStokKarti.Text);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");// + fisid);
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                xrBarkod.DataSource = ds;
                DataTable dt = DB.GetData("SELECT top 1 * FROM EtiketSablonlari  with(nolock) where Varsayilan=1 and (fkKullanicilar=0 or fkKullanicilar="+DB.fkKullanicilar+") order by SiraNo");
                if (dt.Rows.Count == 0)
                {
                    dt = DB.GetData("SELECT top 1 * FROM EtiketSablonlari  with(nolock) where (fkKullanicilar=0 or fkKullanicilar=" + DB.fkKullanicilar + ") order by SiraNo");
                
                    MessageBox.Show("Varsayılan Seçilmemiş");
                    //return;
                }
                try
                {
                    RaporDosyasi = exedizini + "\\Raporlar\\" + dt.Rows[0]["DosyaYolu"].ToString() + ".repx";
                    //rapor.DataSource = gCPerHareketleri.DataSource;
                    //rapor.CreateDocument();
                    xrBarkod.Name = "Barkod";
                    xrBarkod.DisplayName = "Barkod";
                    xrBarkod.LoadLayout(RaporDosyasi);
                    //Barkod.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);

                    #region Ean13 mü Code128 mi Code39 mu
                    //if (!Digerislemler.BarkodEan13(Barkod.Text))
                    //{
                    //    //DevExpress.XtraReports.UI.XRBarCode barCode = new DevExpress.XtraReports.UI.XRBarCode();
                    //    //barCode.Symbology = new DevExpress.XtraPrinting.BarCode.Code128Generator();
                    //    //barCode.Name = "barCode2";
                    //    //xrBarkod.Bands[0].Controls.Add(barCode);

                    //    ((DevExpress.XtraReports.UI.XRBarCode)xrBarkod.FindControl("barCode1", false)).Symbology = new DevExpress.XtraPrinting.BarCode.Code128Generator();
                    //    ((DevExpress.XtraReports.UI.XRBarCode)xrBarkod.FindControl("barCode1", false)).Module =2;
                    //}
                    #endregion
                    if (dizayn)
                        xrBarkod.ShowDesigner();
                    else
                    {
                        if (dt.Rows[0]["YaziciAdi"].ToString() == "")
                            MessageBox.Show("Yazıcı Adı Seçiniz");
                        else
                            xrBarkod.Print(dt.Rows[0]["YaziciAdi"].ToString());
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Yazdırırken Hata Oluştur:" + exp.Message);
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            BtnKaydet_Click(sender, e);
            //threadicinYazdir();
            //BarkodYazdir(false);
            System.Threading.Thread tred = new System.Threading.Thread(new System.Threading.ThreadStart(threadicinYazdir));
            tred.Start();
        }
        private void threadicinYazdir()
        {
            BarkodYazdir(false);
        }
        
        private void SatisFiyatiGetir()
        {
            string sql = @"select b.pkSatisFiyatlariBaslik,b.Baslik,sf.SatisFiyatiKdvli,b.Tur
                            from SatisFiyatlariBaslik b with(nolock)
                            left join SatisFiyatlari sf with(nolock) on sf.fkSatisFiyatlariBaslik=b.pkSatisFiyatlariBaslik
                            where sf.Aktif=1 and sf.fkStokKarti=@fkStokKarti";
                    sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);

            DataTable dtFiyati = DB.GetData(sql);
            btnSatisFiyatlari.Text = dtFiyati.Rows.Count.ToString();
            for (int i = 0; i < dtFiyati.Rows.Count; i++)
            {
                string tur = dtFiyati.Rows[i]["Tur"].ToString();
                //1.fiyat nakit
                if (tur == "1")
                {
                    decimal s1 = 0;
                    decimal.TryParse(dtFiyati.Rows[i]["SatisFiyatiKdvli"].ToString(),out s1);
                    SatisFiyati1.EditValue = s1;
                    
                    fiyatbaslikid1.Tag = dtFiyati.Rows[i]["pkSatisFiyatlariBaslik"].ToString();
                    fiyatbaslikid1.Text = dtFiyati.Rows[i]["Baslik"].ToString();
                    fiyatbaslikid1.ToolTip = dtFiyati.Rows[i]["Baslik"].ToString();
                }
                    //2.fiyat k.kartı
                else if (tur == "2") 
                {
                    decimal s2 = 0;
                    decimal.TryParse(dtFiyati.Rows[i]["SatisFiyatiKdvli"].ToString(), out s2);
                    SatisFiyati2.EditValue = s2;

                    //SatisFiyati2.Enabled = true;
                    //SatisFiyati2KdvHaric.Enabled = true;
                    lbFiyatBaslikid2.Tag = dtFiyati.Rows[i]["pkSatisFiyatlariBaslik"].ToString();
                    lbFiyatBaslikid2.Text = dtFiyati.Rows[i]["Baslik"].ToString();
                    lbFiyatBaslikid2.ToolTip = dtFiyati.Rows[i]["Baslik"].ToString();

                    lbFiyatBaslikid2.Visible = true;
                    SatisFiyati2.Visible = true;
                    SatisFiyati2KdvHaric.Visible = true;
                }
            }
            
            lbFiyatBaslikid2.Visible = true;
            SatisFiyati2.Visible = true;
            SatisFiyati2KdvHaric.Visible = true;
            seSatisiskonto2.Visible = true;
            ceSatisFiyatiiskontolu2.Visible = true;
            txikincifiyatyuzde.Visible = true;

            if (dtFiyati.Rows.Count < 2)
            {
                lbFiyatBaslikid2.Visible = false;
                SatisFiyati2.Visible = false;
                SatisFiyati2KdvHaric.Visible = false;
                seSatisiskonto2.Visible = false;
                ceSatisFiyatiiskontolu2.Visible = false;
                txikincifiyatyuzde.Visible = false;
            }
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           toolStripMenuItem1_Click(sender,e);
        }

        private void Upload(string FtpServer, string Username, string Password, string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://" + FtpServer + "/httpdocs/Merpa/pictures/" + fileInf.Name;//Burada uplaod ediceğiniz dizini tam oalrak belirtmelisiniz.
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(Username, Password);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();

            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                MessageBox.Show("Dosya Webe Gönderildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error");
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            if (webdegoster.Checked)
            {
                // string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                //string dosya = exeDiz + "\\StokKartiResim\\" + tEpkStokKarti.Text + ".jpg";
                DB.ExecuteSQL("UPDATE StokKarti SET WebdeGoster=1 where pkStokKarti=" + pkStokKarti.Text);
                //if (File.Exists(dosya))
                //    Upload("bendeyapi.com", "gurbuzadem", "tekteksql41", dosya);
                //else
                //    MessageBox.Show("Dosya Webe Gönderilemedi. Dosya Yolunu Kontrol Ediniz.");
            }
            else
                DB.ExecuteSQL("UPDATE StokKarti SET WebdeGoster=0 where pkStokKarti=" + pkStokKarti.Text);

        }

        private void resimSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".jpg";
            if (File.Exists(dosya))
            {
                File.Delete(dosya);
                pictureEdit1.Image = null;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".jpg|*.jpg|.png|*.png|.bmp|*.bmp|.gif|*.gif";
            openFileDialog1.Title = "Stok Resmi Seçiniz";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                FileStream bitmapFile = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                Image loaded = new Bitmap(bitmapFile);
                pictureEdit1.Image = loaded;//resim.Image;
                bitmapFile.Dispose();

                //pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                //hata var
                //string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".jpg";
                if (File.Exists(dosya))
                    File.Delete(dosya);
                File.Copy(openFileDialog1.FileName, dosya);
                DB.ExecuteSQL("update StokKarti set ResimYol='" + openFileDialog1.SafeFileName + "' where pkStokKarti=" + pkStokKarti.Text);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(pictureEdit1.Image, true);
        }

        private void yapıştırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image loaded = new Bitmap(pictureEdit1.Image);
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".jpg";
            loaded.Save(dosya);
            loaded.Dispose();
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".jpg";
            if (File.Exists(dosya))
            {
                Upload("bendeyapi.com", "gurbuzadem", "tekteksql41", dosya);
                DB.ExecuteSQL("UPDATE StokKarti SET WebdeGoster=1 where pkStokKarti=" + pkStokKarti.Text);
            }
            else
                MessageBox.Show("Dosya Webe Gönderilemedi. Dosya Yolunu Kontrol Ediniz.");
        }

        private void simpleButton14_Click_1(object sender, EventArgs e)
        {
            frmStokGrupKarti StokGrupKarti = new frmStokGrupKarti();
            StokGrupKarti.ShowDialog();
            Gruplar();
        }

        private void simpleButton8_Click_1(object sender, EventArgs e)
        {
            frmStokGrupKarti StokGrupKarti = new frmStokGrupKarti();
            StokGrupKarti.Tag = "1";
            StokGrupKarti.ShowDialog();
            Gruplar();
            if(lueGruplar.EditValue==null)
                lueGruplar.EditValue = int.Parse(DB.GetData("SELECT MAX(pkStokGrup) FROM StokGruplari with(nolock)").Rows[0][0].ToString());
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from Kodver where AktifSec=1");
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Varsayılan Seçiniz");
                simpleButton22_Click(sender, e);
                return;
            }
            tEStokKod.Text = dt.Rows[0]["Kodu"].ToString() + dt.Rows[0]["Rakam"].ToString();
            tEStokKod.Tag = dt.Rows[0]["pkKodver"].ToString();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton7_Click_1(object sender, EventArgs e)
        {
            frmStokKartiBirimleri StokKartiBirimleri = new frmStokKartiBirimleri();
            StokKartiBirimleri.pkStokKartiid.Text = pkStokKarti.Text;
            StokKartiBirimleri.pkStokKartiid.ToolTip = cbBirimi.Text;
            StokKartiBirimleri.ShowDialog();
        }
        void ResimKaydet()
        {
            if (pictureEdit1.Tag == "0") return;

            try
            {
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                if (Directory.Exists(exeDiz + "\\StokKartiResim\\") == false)
                    Directory.CreateDirectory(exeDiz + "\\StokKartiResim\\");

                if (Directory.Exists(exeDiz + "\\HizliSatisButtonResim\\") == false)
                    Directory.CreateDirectory(exeDiz + "\\HizliSatisButtonResim\\");
                string dosya = "", dosya2;
                if (pkStokKarti.Text == "0")
                {
                    dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".Png";
                    dosya2 = exeDiz + "\\HizliSatisButtonResim\\" + pkStokKarti.Text + ".Png";
                }
                else
                {
                    //DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                    dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".Png";
                    dosya2 = exeDiz + "\\HizliSatisButtonResim\\" + pkStokKarti.Text + ".Png";
                }
                if (pictureEdit1.Image == null) return;
                if (File.Exists(dosya)) File.Delete(dosya);
                if (File.Exists(dosya2)) File.Delete(dosya2);
                //pictureEdit1.Image = Image.FromFile(dosya);
                //Bitmap bm = new Bitmat(pictureEdit1.Image);

                Bitmap loaded = new Bitmap(pictureEdit1.Image, int.Parse(gen.Value.ToString()), int.Parse(yuk.Value.ToString()));
                Bitmap loaded2 = new Bitmap(pictureEdit1.Image, 48, 48);// int.Parse(gen.Value.ToString()) / 2, int.Parse(yuk.Value.ToString()) / 2);
                Image img = (Image)loaded;
                Image img2 = (Image)loaded2;
                //img.Save(dosya, System.Drawing.Imaging.ImageFormat.Bmp);
                //loaded.MakeTransparent(Color.White);
                //loaded2.MakeTransparent(Color.Empty);
                img.Save(dosya, System.Drawing.Imaging.ImageFormat.Png);
                loaded.Dispose();
                img2.Save(dosya2, System.Drawing.Imaging.ImageFormat.Png);
                loaded2.Dispose();
                ilkresimyukle = true;
                pictureEdit1.Tag = "0";
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
                pictureEdit1.Tag = "0";
            }
        }
        private void pictureEdit1_EditValueChanged_1(object sender, EventArgs e)
        {
            pictureEdit1.Tag = "1";
            if (pkStokKarti.Text != "0" && ilkresimyukle == false && pictureEdit1.EditValue != null)
                ResimKaydet();

            if (pictureEdit1.EditValue == null)
            {
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".Png";
                if(File.Exists(dosya))
                   File.Delete(dosya);
                dosya = exeDiz + "\\HizliSatisButtonResim\\" + pkStokKarti.Text + ".Png";
                if (File.Exists(dosya))
                    File.Delete(dosya);
            }
        }

        private void frmStokKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();                
        }
        private void frmStokKarti_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(degisiklikvar == true)
            {
                string sec = formislemleri.MesajBox("Değişiklikleri Kaydetmek İstediğinize Eminmisiniz?", "Stok Kartında Değişiklik Yapılmıştır", 3, 0);
                if (sec != "1") return;
                //DialogResult secim;
                //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Değişiklikleri Kaydetmek İstediğinize Eminmisiniz.", "Hitit 2012", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (secim == DialogResult.Yes)
                if (sec == "1")
                    BtnKaydet_Click(sender, e);
                else
                    e.Cancel = true;

            }
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

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            frmMarkaModel MarkaModel = new frmMarkaModel();
            MarkaModel.ShowDialog();
            Markalar();
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            frmStokBirimKarti StokBirimKarti = new frmStokBirimKarti(2);
            StokBirimKarti.ShowDialog();
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            frmKodver Kodver = new frmKodver();
            Kodver.gcKodu.Visible = true;
            Kodver.ShowDialog();
            if (!tEStokKod.Properties.ReadOnly)
            {
                tEStokKod.Text = Kodver.kodal.Text;
                tEStokKod.Tag = Kodver.pkKodver.Text;
            }
            Kodver.Dispose();
        }
        private void lueGruplar_EditValueChanged(object sender, EventArgs e)
        {
            AltGruplar();
        }
        private bool barkodvarmi()
        {
            DataTable dt = DB.GetData("SELECT pkStokKarti,Barcode FROM StokKarti with(nolock) WHERE Barcode='" + Barkod.EditValue + "'");
            if (dt.Rows.Count > 0)
            {
                DB.pkStokKarti = int.Parse(dt.Rows[0]["pkStokKarti"].ToString());
                StokBilgileriGetir();
                //frmMesaj Mesaj = new frmMesaj();
                //Mesaj.label1.Text="Barkod Daha Önce Kullanıldı. \n Lütfen Başka Barkod Kullanınız";
                //Mesaj.label1.BackColor = System.Drawing.Color.Red;
                //Mesaj.Show();
                //Barkod.Focus();
                ureticikodu.Focus();
                //tEStokadi.Focus();
                return true;
            }
            else if (dt.Rows.Count == 0)
            {
                dt = DB.GetData("SELECT fkStokKarti as pkStokKarti FROM StokKartiBarkodlar with(nolock) WHERE Barkod='" + Barkod.EditValue + "'");
                if (dt.Rows.Count > 0)
                {
                    DB.pkStokKarti = int.Parse(dt.Rows[0]["pkStokKarti"].ToString());

                    StokBilgileriGetir();

                    //checkEdit1.Checked = true;

                    //coklubarkod();

                    return true;
                }
                else
                    return false;
            }

            return false;
        }

        private void Barkod_Leave(object sender, EventArgs e)
        {
            if (Barkod.Text == "")
                btnBarkodBas.Focus();
            else
            {
                if (barkodvarmi() == false)
                    tEStokadi.Focus();
            }
        }

        private void RBGKodu_Leave(object sender, EventArgs e)
        {
            if (RBGKodu.Text == "") return;
            DataTable dt = DB.GetData("select * from StokKarti with(nolock) where RBGKodu='" + RBGKodu.Text + "'");
            if(dt.Rows.Count==0) return;
            if (dt.Rows[0]["Stokadi"].ToString() != tEStokadi.Text)
                MessageBox.Show("Bu Ürün Adını Kontrol Ediniz \n Bu Grupda Daha Önceki Adı " +
                  dt.Rows[0]["Stokadi"].ToString() + " \n Kullanınız.");
        }

        private void tEStokadi_KeyDown(object sender, KeyEventArgs e)
        {
                degisiklikvar = true;
                if (e.KeyCode == Keys.Down && targetGrid != null)
                {
                    targetGrid.Visible = true;
                    targetGrid.Focus();
                }
        }

        DevExpress.XtraGrid.GridControl targetGrid = null;
        DevExpress.XtraGrid.Views.Grid.GridView gridView = null;
        private void tEStokadi_KeyUp(object sender, KeyEventArgs e)
        {
            if (targetGrid == null)
            {
                targetGrid = new DevExpress.XtraGrid.GridControl();
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
            if (tEStokadi.Text.Length > 2)
            {
                if(tEStokadi.Text.IndexOf(" ")==-1)
                    targetGrid.DataSource = DB.GetData("select Stokadi,Barcode,SatisFiyati,Stoktipi as Birimi,pkStokKarti as Id From StokKarti with(nolock) where Stokadi like '%" + tEStokadi.Text + "%'"); // populated data table
                else
                {
                    targetGrid.DataSource = DB.GetData("select Stokadi,Barcode,SatisFiyati,Stoktipi as Birimi,pkStokKarti as Id From StokKarti with(nolock) where Stokadi like '%" + tEStokadi.Text.Substring(0, tEStokadi.Text.IndexOf(" ")) + "%'" +
                       " and Stokadi like '%" + tEStokadi.Text.Substring(tEStokadi.Text.IndexOf(" ")+1, tEStokadi.Text.Length - tEStokadi.Text.IndexOf(" ")-1) + "%'"); 
                }
            }
            targetGrid.BringToFront();
            targetGrid.Width = 550;
            targetGrid.Height = 300;
            targetGrid.Left = tEStokadi.Left + 2;
            targetGrid.Top = tEStokadi.Top + 135;
            if (gridView.Columns.Count > 0)
            {
                gridView.Columns[0].Width = 250;
                gridView.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView.Columns[2].DisplayFormat.FormatString = "{0:#0.00####}";
                gridView.Columns[4].Visible = false;
            }
            if (gridView.DataRowCount == 0)
                targetGrid.Visible = false;
            else
                targetGrid.Visible = true;
        }
        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
                tEStokadi.Text = dr["Stokadi"].ToString();
                targetGrid.Visible = false;
            }
        }
        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            tEStokadi.Text = dr["Stokadi"].ToString();
            DB.pkStokKarti = int.Parse(dr["Id"].ToString());
            StokBilgileriGetir();
            if (Barkod.Text.Length!=13)
                Barkod.Text = "";
            Barkod.Properties.ReadOnly=false;
            Barkod.Focus();
            targetGrid.Visible = false;
        }
        private void lueStokAdi_QueryCloseUp(object sender, CancelEventArgs e)
        {
            tEStokadi.EditValue = lueStokAdi.EditValue;
        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            frmStokKartiRenkBeden StokKartiRenkBeden = new frmStokKartiRenkBeden(0,0);
            //StokKartiRenkBeden.RBGKodu.Text = RBGKodu.Text;
            StokKartiRenkBeden.ShowDialog();
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            DataTable dt =
            DB.GetData("select top 1 pkStokKarti from StokKarti with(nolock) where pkStokKarti>" + DB.pkStokKarti.ToString() + " order by pkStokKarti");
            if (dt.Rows.Count>0)
                DB.pkStokKarti = int.Parse(dt.Rows[0][0].ToString());
            StokBilgileriGetir();
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            tEStokKod.EditValue = null;
            DataTable dt =
             DB.GetData("select top 1 pkStokKarti from StokKarti with(nolock) where pkStokKarti<" + DB.pkStokKarti.ToString() + " order by pkStokKarti desc");
            if (dt.Rows.Count > 0)
                DB.pkStokKarti = int.Parse(dt.Rows[0][0].ToString());
            StokBilgileriGetir();
        }

        private void Barkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Barkod.Text == "")
                    btnBarkodBas.Focus();
                else
                {
                    if (barkodvarmi() == false)
                        ureticikodu.Focus();//tEStokadi.Focus();
                }
            }

            if (e.KeyCode == Keys.Right)
                btnBarkodBas.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            simpleButton4_Click(sender, e);
            timer1.Enabled = false;
        }

        private void simpleButton25_Click(object sender, EventArgs e)
        {
            frmTedarikciKarti TedarikciKarti = new frmTedarikciKarti("0");
            DB.PkFirma = 0;
            TedarikciKarti.ShowDialog();
            if (TedarikciKarti.pkkurum.EditValue.ToString() != "0")
            {
                Tedarikciler();
                lueTedarikciler.EditValue = int.Parse(TedarikciKarti.pkkurum.EditValue.ToString());
            }
        }

        private void tEStokadi_EditValueChanged(object sender, EventArgs e)
        {
            baslik.Text = tEStokadi.EditValue.ToString();
            if (tEStokadi.Text.Length > 100)
            {
                MessageBox.Show("Stok Adı 100 Karakterden Fazla Olmaz");
                tEStokadi.Text = tEStokadi.Text.ToString().Substring(0, 99);
            }
        }

        private void simpleButton26_Click(object sender, EventArgs e)
        {
            if (pkStokKarti.Text == "0") return;

            frmEtiketBas EtiketBas = new frmEtiketBas();
            EtiketBas.Barkod.Text = Barkod.Text;
            EtiketBas.ShowDialog();

            EtiketBas.Dispose();
        }
        private void cbBirimi_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cbBirimi.Text == "KOLİ" || cbBirimi.Text == "PAKET" ||
                cbBirimi.Text == "DÜZİNE" || cbBirimi.Text == "DESTE" || cbBirimi.Text == "GROSS")
            {
                //seicindekimiktar.Value = 1;
                seicindekimiktar.Visible = true;
                cbicindekimiktar.Visible = true;
            }
            else
            {
                seicindekimiktar.Value = 1;
                seicindekimiktar.Visible = false;
                cbicindekimiktar.Visible = false;
            }
            if (cbBirimi.Text == "DESTE")
                seicindekimiktar.Value = 10;
            if (cbBirimi.Text == "DÜZİNE")
                seicindekimiktar.Value = 12;
            if (cbBirimi.Text == "GROSS")
                seicindekimiktar.Value = 144;
            if (cbBirimi.Text == "DÜZİNE" || cbBirimi.Text == "DESTE" || cbBirimi.Text == "GROSS")
                  seicindekimiktar.Enabled = false;
            else
                  seicindekimiktar.Enabled = true;
        }

        private void lueTedarikciler_EditValueChanged(object sender, EventArgs e)
        {
            if (lueTedarikciler.OldEditValue!=null && (lueTedarikciler.OldEditValue != lueTedarikciler.EditValue))
                degisiklikvar = true;
        }
        void StokKartinaBagliAlisFaturasiEkle(string pkstokid)
        {
            DataTable dt = DB.GetData("select * from StokKarti where pkStokKarti="+ pkstokid);
            if (dt.Rows.Count == 0)
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.Text="Stok Karti Bulunamadı";
                Mesaj.Show();
                return;
            }
            string sql = "";
            string fisno = "0";
            bool yazdir = false;
            ArrayList list = new ArrayList();
            string Firmaid=dt.Rows[0]["fkTedarikciler"].ToString();
            if(Firmaid=="") Firmaid="1";
            list.Add(new SqlParameter("@fkFirma", Firmaid));
            list.Add(new SqlParameter("@Siparis", "1"));
            list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("@fkSatisDurumu", "1"));
            list.Add(new SqlParameter("@Aciklama", "Stok Kartından Eklendi"));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", "0"));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));

            sql = "INSERT INTO Alislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar) SELECT IDENT_CURRENT('Alislar')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            //aliş detay ekle
            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkAlislar", fisno));
            arr.Add(new SqlParameter("@SatisFiyatGrubu", "0"));
            arr.Add(new SqlParameter("@AlisFiyati", dt.Rows[0]["AlisFiyati"].ToString().Replace(",", ".")));
            arr.Add(new SqlParameter("@Adet", "1"));
            arr.Add(new SqlParameter("@fkStokKarti", pkstokid));
            arr.Add(new SqlParameter("@iskontoyuzde", "0"));
            arr.Add(new SqlParameter("@iskontotutar", "0"));
            string s = DB.ExecuteScalarSQL("exec sp_AlisDetay_Ekle @fkAlislar,@AlisFiyati,@SatisFiyatGrubu,@Adet,@fkStokKarti,@iskontoyuzde,@iskontotutar", arr);
            if (s != "Alis Detay Eklendi.")
            {
                MessageBox.Show(s);
                return;
            }
        }
        private void labelControl20_Click(object sender, EventArgs e)
        {
            //StokKartinaBagliAlisFaturasiEkle(pkStokKarti.Text);
        }

        private void labelControl20_Click_1(object sender, EventArgs e)
        {
            //StokKartinaBagliAlisFaturasiEkle(pkStokKarti.Text);
        }

        private void simpleButton9_Click_2(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Stok Kartlarına Ait Alış Faturaları Oluşturulacaktır.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            DataTable dt = DB.GetData("select * from StokKarti");
            for (int i = 0; i < dt.Rows.Count; i++)
                StokKartinaBagliAlisFaturasiEkle(dt.Rows[i]["pkStokKarti"].ToString());

            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = "Alış Faturaları Oluşturuldu";
            Mesaj.Show();
        }

        private void frmStokKarti_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Degerler.stokkartisescal = false;
        }

        private void lueTedarikciler_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.Focus();
        }

        private void tEStokadi_Leave(object sender, EventArgs e)
        {
            if (targetGrid != null)
            {
                if (ActiveControl.Name.ToString() == "ara")
                    targetGrid.Visible = true;
                else
                    targetGrid.Visible = false;
            }
        }

        void markarenkbeden(string sec)
        {
            frmMarkaKarti MarkaKarti = new frmMarkaKarti();
            MarkaKarti.Tag = sec;
            MarkaKarti.ShowDialog();
        }
        private void simpleButton15_Click_1(object sender, EventArgs e)
        {
            markarenkbeden("M");
            Markalar();
        }

        void AlisFiyatiKdvDahilHesapla()
        {
            decimal kdvtutar = 0, kdvharic = 0;
            int kdv = int.Parse(cbKdvOraniAlis.EditValue.ToString());
            kdvharic = decimal.Parse(AlisFiyatiKdvHaric.EditValue.ToString());
            kdvtutar = (kdvharic * kdv) / 100;
            AlisFiyati.EditValue = kdvharic + kdvtutar;
        }
        private void AlisFiyatiKdvHaric_Leave(object sender, EventArgs e)
        {
            AlisFiyatiKdvDahilHesapla();
        }

        void AlisFiyatiKdvHaricHesapla()
        {
            decimal kdvtutar = 0, kdvdahil = 0, AlisFiyatiKdv = 0;
            int kdv = 0;

            if (AlisFiyati.EditValue == null) return;

            AlisFiyatiKdv = decimal.Parse(AlisFiyati.EditValue.ToString());
            int.TryParse(cbKdvOraniAlis.EditValue.ToString(), out kdv);
            kdvdahil = decimal.Parse(AlisFiyati.EditValue.ToString());
            //708*18 (Kdv Oranı) /100 + 18(Kdv Oranı) 
            kdvtutar = (AlisFiyatiKdv * kdv) / (100 + kdv);
            //kdvtutar = (kdvdahil * kdv) / 100;
            AlisFiyatiKdvHaric.EditValue = kdvdahil - kdvtutar;
        }

        private void AlisFiyati_Leave(object sender, EventArgs e)
        {
            AlisFiyatiKdvHaricHesapla();
        }

        private void cbFaturaTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbFaturaTipi.SelectedIndex == 0)
            //{
            //    AlisFiyati.Enabled = true;
            //    SatisFiyati1.Enabled = true;
            //    SatisFiyati2.Enabled = true;

            //    AlisFiyatiKdvHaric.Enabled = false;
            //    SatisFiyatikdvharic.Enabled = false;
            //    SatisFiyati2KdvHaric.Enabled = false;
            //}
            //else
            //{
            //    AlisFiyati.Enabled = false;
            //    SatisFiyati1.Enabled = false;
            //    SatisFiyati2.Enabled = false;

            //    AlisFiyatiKdvHaric.Enabled = true;
            //    SatisFiyatikdvharic.Enabled = true;
            //    SatisFiyati2KdvHaric.Enabled = true;
            //}
        }
        private void AlisFiyatiKdvHaric_EditValueChanged(object sender, EventArgs e)
        {
            if (AlisFiyatiKdvHaric.OldEditValue != null && (AlisFiyatiKdvHaric.OldEditValue != AlisFiyatiKdvHaric.EditValue))
                degisiklikvar = true;
        }

        private void SatisFiyati2_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
            if (SatisFiyati2.OldEditValue != null && (SatisFiyati2.OldEditValue != SatisFiyati2.EditValue))
                degisiklikvar = true;
        }

        private void SatisFiyati1_Enter(object sender, EventArgs e)
        {
            SatisFiyati1.Tag = 1;

            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
            if (SatisFiyati1.OldEditValue != null && (SatisFiyati1.OldEditValue != SatisFiyati1.EditValue))
                degisiklikvar = true;
        }
        delegate void EditorSelectAllProc(Control c);
        void EditorSelectAll(Control c)
        {
            ((TextBox)c.Controls[0]).SelectAll();
        }
        private void AlisFiyati_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
            if (AlisFiyati.OldEditValue != null && (AlisFiyati.OldEditValue != AlisFiyati.EditValue))
               degisiklikvar = true;
        }

        private void AlisFiyatiKdvHaric_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
            if (AlisFiyatiKdvHaric.OldEditValue != null && (AlisFiyatiKdvHaric.OldEditValue != AlisFiyatiKdvHaric.EditValue))
               degisiklikvar = true;
        }

        private void SatisFiyati1_Leave(object sender, EventArgs e)
        {
            SatisFiyati1.Tag = 0;
        }

        private void SatisFiyati2_Leave(object sender, EventArgs e)
        {
            SatisFiyati2.Tag = 0;
        }

        private void SatisFiyati1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                seSatisiskonto.Focus();
                seSatisiskonto.SelectAll();
            }
        }

        private void SatisFiyati2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                seSatisiskonto2.Focus();
                seSatisiskonto2.SelectAll();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                lueMarka.Focus();
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
             markarenkbeden("R");
             RenkGruplari();
        }

        private void simpleButton27_Click(object sender, EventArgs e)
        {
             markarenkbeden("B");
             BedenGruplari();
        }

        private void lUreticiKodu_MouseUp(object sender, MouseEventArgs e)
        {
            if(lUreticiKodu.Checked)
               DB.ExecuteSQL("UPDATE Parametreler set Aktif=1 where Aciklama10='lUretici'");
            else
               DB.ExecuteSQL("UPDATE Parametreler set Aktif=0 where Aciklama10='lUretici'");
        }

        private void lMarka_MouseUp(object sender, MouseEventArgs e)
        {
            if (lMarka.Checked)
                DB.ExecuteSQL("UPDATE Parametreler set Aktif=1 where Aciklama10='lMarka'");
            else
                DB.ExecuteSQL("UPDATE Parametreler set Aktif=0 where Aciklama10='lMarka'");
        }

        private void lRenk_MouseUp(object sender, MouseEventArgs e)
        {
            if (lRenk.Checked)
                DB.ExecuteSQL("UPDATE Parametreler set Aktif=1 where Aciklama10='lRenk'");
            else
                DB.ExecuteSQL("UPDATE Parametreler set Aktif=0 where Aciklama10='lRenk'");
        }

        private void lBeden_MouseUp(object sender, MouseEventArgs e)
        {
            if (lBeden.Checked)
                DB.ExecuteSQL("UPDATE Parametreler set Aktif=1 where Aciklama10='lBeden'");
            else
                DB.ExecuteSQL("UPDATE Parametreler set Aktif=0 where Aciklama10='lBeden'");
        }

        private void lueBeden_Leave(object sender, EventArgs e)
        {
            //if (lueBeden.OldEditValue != null && (lueBeden.OldEditValue != lueBeden.EditValue))
            //{
            //    string BedenAdi=
            //    DB.GetData("SELECT Aciklama from BedenGrupKodu where pkBedenGrupKodu="+lueBeden.OldEditValue.ToString()).Rows[0]["Aciklama"].ToString();
            //    tEStokadi.Text = tEStokadi.Text.Replace(BedenAdi, lueBeden.Text);
            //    degisiklikvar = true;
            //}
        }

        private void lueRenk_Leave(object sender, EventArgs e)
        {
            //if (lueRenk.OldEditValue != null && (lueRenk.OldEditValue != lueRenk.EditValue))
            //{
            //    string RenkAdi =
            //    DB.GetData("SELECT Aciklama from RenkGrupKodu where pkRenkGrupKodu=" + lueRenk.OldEditValue.ToString()).Rows[0]["Aciklama"].ToString();
            //    tEStokadi.Text = tEStokadi.Text.Replace(RenkAdi, lueRenk.Text);
            //    degisiklikvar = true;
            //}
        }

        private void lueMarka_Leave(object sender, EventArgs e)
        {
            //if (lueMarka.OldEditValue != null && lueMarka.OldEditValue.ToString() != "0" && (lueMarka.OldEditValue != lueMarka.EditValue))
            //{
            //    string MarkaAdi =
            //    DB.GetData("SELECT Marka from Markalar where pkMarka=" + lueMarka.OldEditValue.ToString()).Rows[0]["Marka"].ToString();
            //    tEStokadi.Text = tEStokadi.Text.Replace(MarkaAdi, lueMarka.Text);
            //    degisiklikvar = true;
            //}
        }

        private void ureticikodu_Leave(object sender, EventArgs e)
        {
            if (ureticikodu.Text == "") return;

            DataTable dt = DB.GetData("select Stokadi from StokKarti with(nolock) where UreticiKodu='" + ureticikodu.Text + "' and pkStokKarti<>"+ pkStokKarti.Text); 
            if (dt.Rows.Count>0)
            {
                MessageBox.Show("Bu Üretici Kodu Daha Önce " + dt.Rows[0]["Stokadi"].ToString() + " için  Kullanıldı");
                //ureticikodu.Focus();
            }
            //if (ureticikodu.OldEditValue != null && ureticikodu.OldEditValue != "" && (ureticikodu.OldEditValue != ureticikodu.EditValue))
            //{
            //    tEStokadi.Text = tEStokadi.Text.Replace(ureticikodu.OldEditValue.ToString(), ureticikodu.Text);
            //    degisiklikvar = true;
            //}
        }

        private void speicindekimiktar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                cbBirimi.Focus();
            }
            else if (e.KeyCode == Keys.Right)
            {
                seSatisAdedi.Focus();
                seSatisAdedi.SelectAll();
            } 
                
            //if (e.KeyCode == Keys.Enter)
             //   kdvorani.Focus();
        }

        private void hızlıEtiketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarkodYazdir(true);
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

        private void ureticikodu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tEStokadi.Focus();
        }

        private void alisdangeldi_Click(object sender, EventArgs e)
        {
            Barkod.Properties.ReadOnly = false;
            Barkod.Focus();
        }

        private void simpleButton29_Click(object sender, EventArgs e)
        {
            //if (Barkod.Properties.ReadOnly == true) return;

            //if (DB.pkStokKarti > 0) return;
            //DataTable dtkod = DB.GetData("select Barcode from StokKarti");
            //for (int i = 0; i < dtkod.Rows.Count; i++)
            //{
            //    DataTable dt = DB.GetData("select * from Kodver where AktifSec=1");
            //    if (dt.Rows.Count == 0)
            //    {
            //        MessageBox.Show("Varsayılan Seçiniz");
            //        simpleButton22_Click(sender, e);//kodver ekranını aç
            //        break;
            //    }
            //    Barkod.Text = dt.Rows[0]["Rakam"].ToString();
            //    Barkod.Tag = dt.Rows[0]["pkKodver"].ToString();
            //    if (DB.GetData("select Barcode from StokKarti where Barcode='" + Barkod.Text + "'").Rows.Count == 0)
            //        break;
            //    else
            //    {
            //        DB.ExecuteSQL("UPDATE Kodver SET Rakam=Rakam+1 where AktifSec=1");
            //        continue;
            //    }
            //}
            //if (Barkod.Text == "")
           // {
                DataTable dt = DB.GetData("select * from Kodver with(nolock) where AktifSec=1");
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Varsayılan Seçiniz");
                    simpleButton22_Click(sender, e);//kodver ekranını aç
                    return;
                }
                Barkod.Text = dt.Rows[0]["Rakam"].ToString();
                DB.ExecuteSQL("UPDATE Kodver SET Rakam=Rakam+1 where AktifSec=1");
                // MessageBox.Show("Tavuk Fiyat Grubunda Boş yer bulunmamaktadır!\n Lütfen tavuk fiyat grubunun başlanğıç Kodunu Değiştiriniz.");
            //}
            //tEStokadi.Focus();
            
            Ean13Barcode2005.Ean13 barcode = new Ean13Barcode2005.Ean13();
            //bu kod barkodun ilk 2 hanesi -ülke kodu
            barcode.CountryCode = "90";

            //Bu kod üretici-imalatçı numarası -bu kısımın legal illegal gibi durumları da var
            string uretici__barkod_no="0000";
            DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
            if(dtSirket.Rows.Count>0)
            {
                if (dtSirket.Rows[0]["uretici__barkod_no"].ToString()!="")
                   uretici__barkod_no = dtSirket.Rows[0]["uretici__barkod_no"].ToString();
            }
            barcode.ManufacturerCode =uretici__barkod_no;

            //Bu kod ürün kodu -ID si falanı felanı
            barcode.ProductCode = Barkod.Text;//"000001";

            //Bu kısım boş geçilsede birşey değişmiyor EAN-13 te zaten 12 veri okuyorsunuz ,bu sayı  barkodun sonunda oluyor.
            barcode.ChecksumDigit = "5";
            Barkod.Text = barcode.ToString();
            ureticikodu.Focus();
        }

        private void Mevcut_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
        }

        private void kritikmiktar_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
        }

        private void speicindekimiktar_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
        }

        private void barkodSınıflarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputForm sifregir = new inputForm();
            sifregir.Girilen.PasswordChar = '#';
            sifregir.ShowDialog();
            if (sifregir.Girilen.Text != "9999") return;

            frmKodver Kodver = new frmKodver();
            Kodver.gcKodu.Visible = false;
            Kodver.ShowDialog();
            if (!Barkod.Properties.ReadOnly)
            {
                Barkod.Text = Kodver.sRakam.Text;
                Barkod.Tag = Kodver.pkKodver.Text;
            }
            Kodver.Dispose();
        }

        void birinci_kar_orani_hesapla()
        {
            decimal satisfiyati = 0, alisfiyati = 0, _karorani=0;
            decimal.TryParse(ceAlisFiyatiiskontolu.Value.ToString(), out alisfiyati);
            decimal.TryParse(ceSatisFiyatiiskontolu.Value.ToString(), out satisfiyati);

            if (satisfiyati > alisfiyati)
            {
                if (alisfiyati == 0)
                    _karorani = 0;
                else
                    _karorani = (satisfiyati - alisfiyati) * 100 / alisfiyati;
                txtbirincifiyatorani.EditValue = "% " + _karorani.ToString("#0.00");
            }
            else
                txtbirincifiyatorani.EditValue = "0";


            txtbirincifiyatorani.EditValue = "% " + _karorani.ToString("#0.00");

            //decimal kdvoran= 0;
            //decimal.TryParse(kdvorani.EditValue.ToString(),out kdvoran);
            //SatisFiyatikdvharic.EditValue = (satisfiyati * kdvoran) / (100 + kdvoran);
        }

        void ikinci_karorani_hesapla()
        {
            decimal satisfiyati = 0, alisfiyati = 0, karorani;
            decimal.TryParse(ceAlisFiyatiiskontolu.Value.ToString(), out alisfiyati);
            decimal.TryParse(ceSatisFiyatiiskontolu2.Value.ToString(), out satisfiyati);

            if (satisfiyati > alisfiyati)
            {
                if (alisfiyati == 0)
                    karorani = 0;
                else
                    karorani = (satisfiyati - alisfiyati) * 100 / alisfiyati;

                txikincifiyatyuzde.EditValue = "% " + karorani.ToString("#0.00");
            }
            else
                txikincifiyatyuzde.EditValue = "0"; 
        }

        private void SatisFiyati1_EditValueChanged(object sender, EventArgs e)
        {
            //if (SatisFiyati1.Tag ==null || SatisFiyati1.Tag.ToString() == "0") return; 
            if (AlisFiyati.EditValue == null) return;

                //SatisFiyatikdvharic.Value = SatisFiyati1.Value-((SatisFiyati1.Value * int.Parse(kdvorani.Text)) / (100 + SatisFiyati1.Value));

            decimal kdvtutar = 0, kdvdahil = 0, SatisFiyatiKdv = 0;
            int kdv = 0;
            SatisFiyatiKdv = decimal.Parse(SatisFiyati1.EditValue.ToString());
            int.TryParse(kdvorani.EditValue.ToString(), out kdv);
            kdvdahil = decimal.Parse(SatisFiyati1.EditValue.ToString());
            //708*18 (Kdv Oranı) /100 + 18(Kdv Oranı) 
            kdvtutar = (SatisFiyatiKdv * kdv) / (100 + kdv);
            //kdvtutar = (kdvdahil * kdv) / 100;

            SatisFiyatikdvharic.EditValue = kdvdahil - kdvtutar;

            ceSatisFiyatiiskontolu.Value = SatisFiyati1.Value - (SatisFiyati1.Value * seSatisiskonto.Value) / 100;

        }

        private void SatisFiyati2_EditValueChanged(object sender, EventArgs e)
        {
            if (AlisFiyati.EditValue == null) return;

            if (SatisFiyati2.EditValue == null) return;

            decimal kdvtutar = 0, kdvdahil = 0, SatisFiyatiKdv = 0;
            int kdv = 0;

            SatisFiyatiKdv = decimal.Parse(SatisFiyati2.EditValue.ToString());
            int.TryParse(kdvorani.EditValue.ToString(), out kdv);
            kdvdahil = decimal.Parse(SatisFiyati2.EditValue.ToString());

            //708*18 (Kdv Oranı) /100 + 18(Kdv Oranı) 
            kdvtutar = (SatisFiyatiKdv * kdv) / (100 + kdv);
            //kdvtutar = (kdvdahil * kdv) / 100;

            SatisFiyati2KdvHaric.EditValue = kdvdahil - kdvtutar;

            ceSatisFiyatiiskontolu2.Value = SatisFiyati2.Value - (SatisFiyati2.Value * seSatisiskonto2.Value) / 100;
            
        }

        private void AlisFiyati_EditValueChanged(object sender, EventArgs e)
        {
            ceAlisFiyatiiskontolu.Value = AlisFiyati.Value - (AlisFiyati.Value * seAlisiskonto.Value) / 100;
        }

        private void seAlisiskonto_EditValueChanged(object sender, EventArgs e)
        {
            //if (seAlisiskonto.Tag.ToString() == "0") return;

            ceAlisFiyatiiskontolu.Value = AlisFiyati.Value - (AlisFiyati.Value * seAlisiskonto.Value) / 100;

            //decimal Alisiskontotutar = 0,kdvtutar=0,Alistutar=0,kdvoran=0;
            //decimal.TryParse(kdvorani.Text,out kdvoran);

            //Alisiskontotutar = (AlisFiyati.Value * seAlisiskonto.Value) / 100;
            //Alistutar = AlisFiyati.Value - Alisiskontotutar;

            //kdvtutar = (Alistutar * kdvoran) / 100;

            ////AlisFiyatiKdvHaric.Value = Alistutar ;

            ////AlisFiyati.EditValue = Alistutar + kdvtutar;
            //ceListeFiyatiKdvHaric.EditValue = Alistutar + kdvtutar;
            //if(seAlisiskonto.Value>0)
            //  ceListeFiyatiKdvHaric.Visible = true;
            //else
            // ceListeFiyatiKdvHaric.Visible = false;

                birinci_kar_orani_hesapla();

                ikinci_karorani_hesapla();
         }

        private void seSatisiskonto_EditValueChanged(object sender, EventArgs e)
        {
            ceSatisFiyatiiskontolu.Value = SatisFiyati1.Value - (SatisFiyati1.Value * seSatisiskonto.Value) / 100;
            
            birinci_kar_orani_hesapla();

        }

        private void seAlisiskonto_Enter(object sender, EventArgs e)
        {
            seAlisiskonto.Tag = "1";
        }

        private void seSatisiskonto_Enter(object sender, EventArgs e)
        {
            seSatisiskonto.Tag = "1";
        }

        private void seAlisiskonto_Leave(object sender, EventArgs e)
        {
            seAlisiskonto.Tag = "0";
        }

        private void seSatisiskonto_Leave(object sender, EventArgs e)
        {
            seSatisiskonto.Tag = "0";
        }

        private void pictureEdit1_MouseClick(object sender, MouseEventArgs e)
        {
            lbResimGen.Visible = true;
            lbResimYuk.Visible = true;
            gen.Visible = true;
            yuk.Visible = true;
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            gcSatisOlcuBirimleri.DataSource = DB.GetData(@"SELECT  SKB.pkStokKartiBirimler, SKB.fkStokKarti, SKB.Aciklama, 
SKB.Satilirsa,B.BirimAdi, SKB.Sat
FROM StokKartiBirimler SKB with(nolock) 
LEFT OUTER JOIN  Birimler B with(nolock) ON SKB.fkBirimler = B.pkBirimler ");

            lueOlcuBirimi.Properties.DataSource = DB.GetData("select * from Birimler B with(nolock) where Aktif=1 order by Varsayilan");
        }

        private void kdvorani_Leave(object sender, EventArgs e)
        {
            int kdv = 0;
            int.TryParse(kdvorani.Text, out kdv);
            if (kdv > 99)
            {
                formislemleri.Mesajform("Kdv Oranını Kontrol Ediniz", "K", 200);
                kdvorani.Focus();
                return;
            }

            if (cbKdvOraniAlis.EditValue == null)
                cbKdvOraniAlis.EditValue = kdvorani.EditValue;

            if (cbKdvOraniAlis.Text=="")
                cbKdvOraniAlis.EditValue = kdvorani.EditValue;

            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Fiyatları Güncellemek İstermisinizi?.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.Yes)
            //{
            if (cbFaturaTipi.SelectedIndex == 0)
                SatisFiyati1_EditValueChanged(sender, e);
            else
            {
                SatisFiyatikdvharic.Tag = "1";
                SatisFiyatikdvharic_EditValueChanged(sender,e);
            }
            //}
        }


        private void SatisFiyatikdvharic_EditValueChanged(object sender, EventArgs e)
        {
            if(SatisFiyatikdvharic.Tag == "1")
            SatisFiyati1.Value = SatisFiyatikdvharic.Value + (SatisFiyatikdvharic.Value * int.Parse(kdvorani.Text) / 100);
        }

        private void SatisFiyatikdvharic_Enter(object sender, EventArgs e)
        {
            SatisFiyatikdvharic.Tag = 1;
        }

        private void SatisFiyatikdvharic_Leave(object sender, EventArgs e)
        {
            SatisFiyatikdvharic.Tag = 0;
            decimal kdvtutar = 0, kdvharic = 0;
            int kdv = int.Parse(kdvorani.EditValue.ToString());
            kdvharic = decimal.Parse(SatisFiyatikdvharic.EditValue.ToString());
            kdvtutar = (kdvharic * kdv) / 100;
            SatisFiyati1.EditValue = kdvharic + kdvtutar;
        }

        private void seicindekimiktar_EditValueChanged(object sender, EventArgs e)
        {
            if (cbicindekimiktar.Checked==true)
               seSatisAdedi.Value = seicindekimiktar.Value;
        }

        private void seMevcutFatura_EditValueChanged(object sender, EventArgs e)
        {
            seMevcutToplam.Value =  Mevcut.Value;
            //seMevcutToplam.Value = seMevcutFatura.Value + Mevcut.Value;
        }

        private void kdvorani_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                cbKdvOraniAlis.Focus();
                cbKdvOraniAlis.SelectAll();
            }
        }

        private void cbKdvOraniAlis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                kdvorani.Focus();
                kdvorani.SelectAll();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (AlisFiyati.Enabled)
                {
                    AlisFiyati.Focus();
                    AlisFiyati.SelectAll();
                }
                else
                {
                    AlisFiyatiKdvHaric.Focus();
                    AlisFiyatiKdvHaric.SelectAll();
                }

            }    
        }

        private void cbKdvOraniAlis_Leave(object sender, EventArgs e)
        {
            int kdv = 0;
            int.TryParse(cbKdvOraniAlis.Text, out kdv);
            if (kdv > 99)
            {
                formislemleri.Mesajform("Kdv Oranını Kontrol Ediniz", "K", 200);
                kdvorani.Focus();
                return;
            }

            if (kdvorani.EditValue == null)
                kdvorani.EditValue = cbKdvOraniAlis.EditValue;

            if (kdvorani.Text == "")
                kdvorani.EditValue = cbKdvOraniAlis.EditValue;

            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Alış Fiyatları Güncellemek İstermisinizi?.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.Yes)
            //{
                if (cbFaturaTipi.SelectedIndex == 0)
                    AlisFiyatiKdvHaricHesapla();
                else
                    AlisFiyatiKdvDahilHesapla();
            //}
        }

        private void cbKdvOraniAlis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //zaten kaydederken boş ise alış iskontoyu kaydeder.
            //if (kdvorani.Text == "")
              //  kdvorani.Text = cbKdvOraniAlis.Text;
        }

        private void spinEdit2_EditValueChanged(object sender, EventArgs e)
        {
            ceSatisFiyatiiskontolu2.Value = SatisFiyati2.Value - (SatisFiyati2.Value * seSatisiskonto2.Value) / 100;
            
            ikinci_karorani_hesapla();
        }

        private void ceAlisFiyatiiskontolu_EditValueChanged(object sender, EventArgs e)
        {
            birinci_kar_orani_hesapla();

            ikinci_karorani_hesapla();
        }

        private void ceSatisFiyatiiskontolu_EditValueChanged(object sender, EventArgs e)
        {
            birinci_kar_orani_hesapla();
        }

        private void calcEdit1_EditValueChanged(object sender, EventArgs e)
        {
            ikinci_karorani_hesapla();
        }
        void BarkodlariKaydet()
        {
            //for (int i = 0; i < gridView3.DataRowCount; i++)
            //{
            //    DataRow dr = gridView3.GetDataRow(i);
            //    if (dr["pkStokKartiBarkodlar"].ToString() == "")
            //    {
            //        string Barkod = dr["Barkod"].ToString();
            //        if (Barkod == "") continue;

            //        if (DB.GetData("Select * from StokKartiBarkodlar with(nolock) where Barkod='" + Barkod + "'").Rows.Count > 0)
            //        {
            //            formislemleri.Mesajform("Barkodlar Daha Önce Kaydedildi", "K", 200);
            //            continue;
            //        }
            //        if (DB.GetData("Select * from StokKarti with(nolock) where Barcode='" + Barkod + "'").Rows.Count > 0)
            //        {
            //            string adi = DB.GetData("Select Stokadi from StokKarti with(nolock) where Barcode='" + 
            //                Barkod + "'").Rows[0][0].ToString();

            //            formislemleri.Mesajform("Barkod Daha Önce Kaydedildi", "K", 200);
            //            continue;
            //        }
            //        DB.ExecuteSQL("INSERT INTO StokKartiBarkodlar (fkStokKarti,Barkod,SatisAdedi)" +
            //            " VALUES(" + pkStokKarti.Text + ",'" + dr["Barkod"].ToString() + "'," + dr["SatisAdedi"].ToString().Replace(",", ".") + ")");
            //    }
            //    else

            //        DB.ExecuteSQL("update StokKartiBarkodlar set Barkod='" + dr["Barkod"].ToString() +
            //            "',SatisAdedi=" + dr["SatisAdedi"].ToString().Replace(",", ".") +
            //            " where pkStokKartiBarkodlar=" + dr["pkStokKartiBarkodlar"].ToString());
            //}
            Barkodlar();
            //gcBarkodlar.DataSource = DB.GetData("select * from StokKartiBarkodlar with(nolock) where fkStokKarti=" + pkStokKarti.Text);
        }
        private void Barkodlar()
        {
            //gcBarkodlar.DataSource = DB.GetData("select * from StokKartiBarkodlar with(nolock) where fkStokKarti=" + pkStokKarti.Text);
        }

        private void repositoryItemTextEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
                    ((DevExpress.XtraEditors.TextEdit)((((DevExpress.XtraEditors.TextEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Text;
                DataTable dt =DB.GetData("Select * from StokKarti with(nolock) where Barcode='" + girilen + "'");
                if (dt.Rows.Count > 0)
                {
                    formislemleri.Mesajform(dt.Rows[0]["Stokadi"].ToString() + "\r\n Barkod Daha Önce Kaydedildi", "K", 200);

                    ((DevExpress.XtraEditors.TextEdit)((((DevExpress.XtraEditors.TextEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Focus();
                    ((DevExpress.XtraEditors.TextEdit)((((DevExpress.XtraEditors.TextEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Text = "";

                    //gridView3.Focus();
                    //gridView3.FocusedColumn = gridView3.VisibleColumns[0];
                    //gridView3.CloseEditor();

                }
            }
        }

        private void simpleButton31_Click(object sender, EventArgs e)
        {
            ilkresimyukle = false;
            pictureEdit1.Tag = "1";

            pictureEdit1.EditValue = null;

            btnEkranCek.Text = "Boşluk tuşuna basın";

            btnEkranCek.Focus();
        }

        private void btnEkranCek_Click(object sender, EventArgs e)
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti.Text + ".Png";
            if (File.Exists(dosya))
                File.Delete(dosya);
            dosya = exeDiz + "\\HizliSatisButtonResim\\" + pkStokKarti.Text + ".Png";
            if (File.Exists(dosya))
                File.Delete(dosya);

            pictureEdit1.Image = ScreenShot();
            //pictureEdit1.SizeMode = PictureBoxSizeMode.Zoom;

            pictureEdit1.Image.Save(
              dosya, System.Drawing.Imaging.ImageFormat.Png);


            ResimGetir();

            btnEkranCek.Text = "";
        }

        private void AlisFiyati_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                seAlisiskonto.Focus();
                seAlisiskonto.SelectAll();
            }
        }

        private void AlisFiyatiKdvHaric_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                seAlisiskonto.Focus();
                seAlisiskonto.SelectAll();
            }
        }

        private void SatisFiyati2KdvHaric_EditValueChanged(object sender, EventArgs e)
        {
            if (SatisFiyati2KdvHaric.Tag == "1")
                SatisFiyati2.Value = SatisFiyati2KdvHaric.Value + (SatisFiyati2KdvHaric.Value * int.Parse(kdvorani.Text) / 100);
        }

        private void SatisFiyati2KdvHaric_Enter(object sender, EventArgs e)
        {
            SatisFiyati2KdvHaric.Tag = 1;
        }

        private void SatisFiyati2KdvHaric_Leave(object sender, EventArgs e)
        {
            SatisFiyati2KdvHaric.Tag = 0;
            decimal kdvtutar = 0, kdvharic = 0;
            int kdv = int.Parse(kdvorani.EditValue.ToString());
            kdvharic = decimal.Parse(SatisFiyati2KdvHaric.EditValue.ToString());
            kdvtutar = (kdvharic * kdv) / 100;
            SatisFiyati2.EditValue = kdvharic + kdvtutar;
        }

        private void seAlisiskonto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (AlisFiyatiKdvHaric.Enabled)
                {
                    AlisFiyatiKdvHaric.Focus();
                    AlisFiyatiKdvHaric.SelectAll();
                }
                else
                {
                    AlisFiyati.Focus();
                    AlisFiyati.SelectAll();
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                seSatisiskonto.Focus();
                seSatisiskonto.SelectAll();
            }
        }

        private void seSatisiskonto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (SatisFiyatikdvharic.Enabled)
                {
                    SatisFiyatikdvharic.Focus();
                    SatisFiyatikdvharic.SelectAll();
                }
                else
                {
                    SatisFiyati1.Focus();
                    SatisFiyati1.SelectAll();
                }
            }
             else if (e.KeyCode == Keys.Enter)
            {
                if (seSatisiskonto2.Visible)
                {
                    seSatisiskonto2.Focus();
                    seSatisiskonto2.SelectAll();
                }
                else
                {
                    lueTedarikciler.Focus();
                }
            }
        }

        private void seSatisiskonto2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (SatisFiyati2KdvHaric.Enabled)
                {
                    SatisFiyati2KdvHaric.Focus();
                    SatisFiyati2KdvHaric.SelectAll();
                }
                else
                {
                    SatisFiyati2.Focus();
                    SatisFiyati2.SelectAll();
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                lueTedarikciler.Focus();
            }
        }

        private void btnwww_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.google.com.tr/search?q=" + tEStokadi.Text.Replace("\"","") + "&source=lnms&tbm=isch&sa");
        }

        private void btnBarkodBas_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Right)
            //    simpleButton29.Focus();

            //if (e.KeyCode == Keys.Left)
            //    Barkod.Focus();
        }

        private void cbBirimi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right && seicindekimiktar.Enabled==true)
            {
                seicindekimiktar.Focus();
                seicindekimiktar.SelectAll();
            }
        }

        private void cbDurumu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.Focus();
        }

        private void seSatisAdedi_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && seicindekimiktar.Enabled==true)
            {
                seicindekimiktar.Focus();
                seicindekimiktar.SelectAll();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                cbKdvOraniAlis.Focus();
                cbKdvOraniAlis.SelectAll();
            }
            
        }

        private void SatisFiyati2KdvHaric_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lueTedarikciler.Focus();
            }    
            else if (e.KeyCode == Keys.Right)
            {
                seSatisiskonto2.Focus();
                seSatisiskonto2.SelectAll();
            }
            
        }

        private void SatisFiyatikdvharic_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                seSatisiskonto.Focus();
                seSatisiskonto.SelectAll();
            }
        }

        void StokKartiMevcutGetir()
        {
            DataTable dt = DB.GetData(@"select isnull(Mevcut,0) as Mevcut From StokKarti with(nolock) where pkStokKArti=" + pkStokKarti.Text);
            string Kalan = "0";
            if (dt.Rows.Count > 0)
            {
                Kalan = dt.Rows[0]["Mevcut"].ToString();
            }

            Mevcut.EditValue = Kalan;
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView3.FocusedRowHandle < 0) return;

            //DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            //if (dr["pkStokKartiBarkodlar"].ToString() != "")
            //{
            //    DB.ExecuteSQL("delete from StokKartiBarkodlar where pkStokKartiBarkodlar=" +
            //        dr["pkStokKartiBarkodlar"].ToString());
            //}
            Barkodlar();
        }

        private void lGrup_MouseUp(object sender, MouseEventArgs e)
        {
            if (lGrup.Checked)
                DB.ExecuteSQL("UPDATE Parametreler set Aktif=1 where Aciklama10='lGrup'");
            else
                DB.ExecuteSQL("UPDATE Parametreler set Aktif=0 where Aciklama10='lGrup'");
        }

        private void txtbirincifiyatorani_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (txtbirincifiyatorani.Text != "")
                {
                    string oran = txtbirincifiyatorani.Text.Replace("%","");

                    decimal doran = decimal.Parse(oran);
                    SatisFiyati1.Value = AlisFiyati.Value + (AlisFiyati.Value * doran) / 100;
                }
                SatisFiyati1.Focus();
            }
        }

        private void txikincifiyatyuzde_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txikincifiyatyuzde.Text != "")
                {
                    string oran = txikincifiyatyuzde.Text.Replace("%", "");

                    decimal doran = decimal.Parse(oran);
                    SatisFiyati2.Value = AlisFiyati.Value + (AlisFiyati.Value * doran) / 100;
                }
                SatisFiyati2.Focus();
            }
        }

        private void txtbirincifiyatorani_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
        }

        private void txikincifiyatyuzde_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new EditorSelectAllProc(EditorSelectAll), (Control)sender);
        }

        void parabirimleri()
        {
            lueParaBirimi.Properties.DataSource = DB.GetData("select * from ParaBirimi with(nolock) where aktif=1");
            lueParaBirimi.ItemIndex = 0;
        }

        void OzelDurumlar()
        {
            lueOzelDurum.Properties.DataSource = DB.GetData("select * from OzelDurumlar with(nolock) where aktif=1");
            lueOzelDurum.ItemIndex = 0;
        }
        
        private void xTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //if (e.Page == xtraTabPage3)
            //{
            //    //parabirimleri();
            //}
            //else if (e.Page == xtraTabPage1)//xtraTabPage4

            StokKartiAciklamaGetir();
                
        }

        void StokKartiAciklamaGetir()
        {
            if (isilkacilis && pkStokKarti.Text == "0") return;

           // {
                //önce kaydet
                if (pkStokKarti.Text == "0" || pkStokKarti.Text == "")
                    StokKartiKaydet(200);

                DataTable dt =
                DB.GetData("select * from StokKartiAciklama with(nolock) where fkStokKarti=" + pkStokKarti.Text);
                if (dt.Rows.Count == 0)
                {
                    memoEdit1.Text = "";
                }
                else
                {
                    memoEdit1.Text = dt.Rows[0]["Aciklama"].ToString();
                }
                memoEdit1.Tag = "1";
           // }
        }

        private void tsmiEkle_Click(object sender, EventArgs e)
        {
            string barkod = "";
            frmStokAra stokara = new frmStokAra("");
            stokara.Tag = "0";
            stokara.ShowDialog();
            barkod = stokara.pkurunid.Text;
            stokara.Dispose();

            DataTable dtStok=DB.GetData("select * from StokKarti where Barcode='"+barkod+"'");

            if(dtStok.Rows.Count==0) return;

            string fkStokKarti = dtStok.Rows[0]["pkStokKarti"].ToString();

            DB.ExecuteSQL("insert into StokKartiBagli (fkStokKarti,fkStokKartiBagli,tarih) values("+
            pkStokKarti.Text+","+fkStokKarti+",getdate())");

            StoklarBagli();
        }

        private void tsmiSil_Click(object sender, EventArgs e)
        {
            int i = gridViewBagli.FocusedRowHandle;

            if(i<0) return;
            DataRow dr = gridViewBagli.GetDataRow(i);

            string pkStokKartiBagli = dr["pkStokKartiBagli"].ToString();
            DB.ExecuteSQL("delete from StokKartiBagli where pkStokKartiBagli=" + pkStokKartiBagli);

            //dr["fkStokKarti"].ToString();
            StoklarBagli();
        }

        private void simpleButton32_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            DataTable dt = DB.GetData("select Mevcut,dbo.fon_StokMevcut(pkStokKarti) as StokMevcut from StokKarti with(nolock) where pkStokKarti=" + pkStokKarti.Text);
            if (dt.Rows.Count == 0)
            {
                formislemleri.Mesajform("Stok Kartı Bulunamadı", "K", 200);
                return;
            }
            string s = formislemleri.MesajBox("Stok Mevcudu " + seDevirAdet.Value.ToString() + " Yapılacaktır.", "Devir", 1, 1);
            if (s == "0") return;

            decimal ceMevcut = decimal.Parse(dt.Rows[0]["StokMevcut"].ToString());
            
            #region bu kısım stok kartındaki ile aynı
            string sql = @"INSERT INTO StokDevir (fkStokKarti,Tarih,Aciklama,OncekiAdet,DevirAdedi,fkKullanicilar,islemTarihi)
                    values(@fkStokKarti,@Tarih,@Aciklama,@OncekiAdet,@DevirAdedi,@fkKullanicilar,getdate())";

            ArrayList list0 = new ArrayList();
            list0.Add(new SqlParameter("@fkStokKarti", pkStokKarti.Text));
            list0.Add(new SqlParameter("@Tarih", DateTime.Now));
            list0.Add(new SqlParameter("@Aciklama", "Stok Kartı Devir"));
            list0.Add(new SqlParameter("@OncekiAdet", ceMevcut.ToString().Replace(",", ".")));

            decimal deviradet = 0;
            //sıfırdan küçükse + yap
            if (ceMevcut < 0)
                deviradet = (ceMevcut * -1) + seDevirAdet.Value;
            else
            {
                //if (ceSimdikiMevcut.Value > ceMevcut.Value)
                deviradet = seDevirAdet.Value - ceMevcut;
                //else
                //{
                //deviradet = ceSimdikiMevcut.Value - ceMevcut.Value;
                //}
            }

            list0.Add(new SqlParameter("@DevirAdedi", deviradet.ToString().Replace(",", ".")));
            list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

            string sonuc = DB.ExecuteSQL(sql, list0);
            if (sonuc == "0")
            {
                Mevcut.Value = seDevirAdet.Value;
                DB.ExecuteSQL("update StokKarti set Mevcut=" + Mevcut.Value.ToString().Replace(",", ".") + " where pkStokKarti=" + pkStokKarti.Text);
                seDevirAdet.Tag = "0";
            }

            if (sonuc == "0" && Degerler.DepoKullaniyorum)
            {
                //if (Degerler.fkDepolar)
                DB.ExecuteSQL("update StokKartiDepo set MevcutAdet=" + Mevcut.Value.ToString().Replace(",", ".") +
                    " where fkStokKarti=" + pkStokKarti.Text + " and fkDepolar=" + Degerler.fkDepolar);
            }

            #endregion

            kritikmiktar.Focus();
        }

        private void spinEdit2_EditValueChanged_1(object sender, EventArgs e)
        {
            seDevirAdet.Tag = "1";
        }

        private void spinEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
                simpleButton32_Click(sender, e);
        }
        
        private void labelControl3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Digerislemler.BarkodEan13(Barkod.Text).ToString());
        }

        private void Barkod_EditValueChanged(object sender, EventArgs e)
        {
            //if (Digerislemler.BarkodEan13(Barkod.Text))
            //    lblBarkod.Text = "Barkodu (Ean13)";
            //else
            //    lblBarkod.Text = "Barkodu (Code128)";
        }

        private void txtbirincifiyatorani_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl45_Click(object sender, EventArgs e)
        {
            int stok_id = 0;
            stok_id = int.Parse(pkStokKarti.Text);
            frmStokKartiDepoMevcut StokKartiDepoMevcut = new frmStokKartiDepoMevcut(stok_id);
            StokKartiDepoMevcut.ShowDialog();
        }

        private void simpleButton33_Click(object sender, EventArgs e)
        {
            frmStokKartiDepoMevcut StokKartiDepoMevcut = new frmStokKartiDepoMevcut(int.Parse(pkStokKarti.Text));
            StokKartiDepoMevcut.ShowDialog();
        }

        private void simpleButton34_Click(object sender, EventArgs e)
        {
            frmStokKartiResimleri StokKartiResimleri = new frmStokKartiResimleri();
            StokKartiResimleri.fkStokKarti.Text = pkStokKarti.Text;
            StokKartiResimleri.ShowDialog();
        }

        private void memoEdit1_EditValueChanged(object sender, EventArgs e)
        {
            memoEdit1.Tag = "1";
        }

        private void btnOzelDurum_Click(object sender, EventArgs e)
        {
            frmStokOzeDurumlar stokozeldurum = new frmStokOzeDurumlar();
            stokozeldurum.ShowDialog();
            OzelDurumlar();
        }

        private void layoutControl1_HideCustomization(object sender, EventArgs e)
        {
            //kapanınca
            string Genel = DB.Sektor;// "Genel";
            layoutControl1.SaveLayoutToXml(Application.StartupPath + "\\EkranTasarimlari\\" + Genel + "\\StokKarti.xml");
        }

        private void Barkod_DoubleClick(object sender, EventArgs e)
        {
            Barkod.Properties.ReadOnly = false;
            //Barkod.Enabled = true;
        }

        private void btnSatisFiyatlari_Click(object sender, EventArgs e)
        {
            if (pkStokKarti.Text == "0")
            {
                BtnKaydet_Click(sender, e);
                //frmMesaj mesaj = new frmMesaj();
                //mesaj.label1.Text = "Lütfen Önce Kaydı Tamamlayınız!";
                //mesaj.Show();
                //return;
            }
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.pkStokKarti.Text = pkStokKarti.Text;
            SatisFiyatlari.ShowDialog();

            if (SatisFiyatlari.TopMost == true)
                SatisFiyatiGetir();

            SatisFiyatlari.Dispose();
        }

        private void btnDevir_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            frmStokDepoDevir StokDevir = new frmStokDepoDevir();
            StokDevir.fkStokKarti.Tag = pkStokKarti.Text;
            StokDevir.ShowDialog();

            StokKartiMevcutGetir();
        }

    }
}
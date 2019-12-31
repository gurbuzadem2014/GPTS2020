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
using System.Drawing.Printing;
using System.IO;

namespace GPTS
{
    public partial class frmFisYaziciYeniRapor : DevExpress.XtraEditors.XtraForm
    {
        int form_id;
        public frmFisYaziciYeniRapor(int formid)
        {
            InitializeComponent();
            form_id = formid;
        }
        void YaziciListesi()
        {
            foreach (String yazicilar in PrinterSettings.InstalledPrinters)
            {
                cbYazicilar.Items.Add(yazicilar);
            }

            PrintDocument pd = new PrintDocument();
            String defaultPrinter = pd.PrinterSettings.PrinterName;
        }
        void SatisTipleri()
        {
            lueSatisTipi.Properties.DataSource = DB.GetData(@"SELECT pkSatisDurumu, Durumu, Aktif, SiraNo FROM  SatisDurumu with(nolock) WHERE Aktif = 1 ORDER BY SiraNo");
            lueSatisTipi.EditValue = 2;   
        }

        void Kullanicilar()
        {
            lueKullanicilar.Properties.DataSource = DB.GetData("select 0 as pkKullanicilar,'Varsayılan' as KullaniciAdi union all select pkKullanicilar,KullaniciAdi from Kullanicilar with(nolock)");
        }

        private void frmStokKoduverKarti_Load(object sender, EventArgs e)
        {
            cbAktif.Checked = true;

            YaziciListesi();

            SatisTipleri();

            Kullanicilar();

            timer1.Enabled = true;

            if (form_id == 1) //satış modülünden ise
            {
                lueSatisTipi.Visible = true;
                labelControl10.Visible = true;


                //yeni ise
                if (pkSatisFisiSecimi.Text == "0")
                {
                    BtnKaydet.Visible = true;
                    BtnGuncelle.Visible = false;
                }
                else
                {
                    BtnKaydet.Visible = false;
                    BtnGuncelle.Visible = true;
                    DataTable dt =
                    DB.GetData("select * from SatisFisiSecimi with(nolock) where pkSatisFisiSecimi=" + pkSatisFisiSecimi.Text);
                    //Aciklama,YaziciAdi,Dosya,fkSatisDurumu
                    GrupAdi.Text = dt.Rows[0]["Aciklama"].ToString();
                    cbYazicilar.Text = dt.Rows[0]["YaziciAdi"].ToString();
                    dosyaadi.Text = dt.Rows[0]["Dosya"].ToString();
                    lueSatisTipi.EditValue = int.Parse(dt.Rows[0]["fkSatisDurumu"].ToString());
                    seYazdirmaAdedi.EditValue = dt.Rows[0]["YazdirmaAdedi"].ToString();
                    lueKullanicilar.EditValue = int.Parse(dt.Rows[0]["fkKullanicilar"].ToString());

                    int sirano=1;
                    int.TryParse(dt.Rows[0]["SiraNo"].ToString(),out sirano);
                    seSiraNo.Value = sirano;

                    if (dt.Rows[0]["Sec"].ToString() == "True")
                        cbAktif.Checked = true;
                    else
                        cbAktif.Checked = false;

                    if (dt.Rows[0]["onizle"].ToString() == "True")
                        cbOnizle.Checked = true;
                    else
                        cbOnizle.Checked = false;

                    dt.Dispose();
                }
            }
            else if (form_id == 2) //Etiket modülünden ise
                {
                    lueSatisTipi.Visible = false;
                    seYazdirmaAdedi.Visible = false;

                    lueSatisTipi.Visible = true;
                    labelControl10.Visible = true;


                    //yeni ise
                    if (pkEtiketSablonlari.Text == "0")
                    {
                        BtnKaydet.Visible = true;
                        BtnGuncelle.Visible = false;
                    }
                    else
                    {
                        BtnKaydet.Visible = false;
                        BtnGuncelle.Visible = true;

                        //SablonAdi,YaziciAdi,DosyaYolu,Aktif,SiraNo,Varsayilan
                        DataTable dt =
                            DB.GetData("select * from EtiketSablonlari with(nolock) where pkEtiketSablonlari=" + pkEtiketSablonlari.Text);
                        //Aciklama,YaziciAdi,Dosya,fkSatisDurumu
                        GrupAdi.Text = dt.Rows[0]["SablonAdi"].ToString();
                        cbYazicilar.Text = dt.Rows[0]["YaziciAdi"].ToString();
                        dosyaadi.Text = dt.Rows[0]["DosyaYolu"].ToString();
                        lueKullanicilar.EditValue = int.Parse(dt.Rows[0]["fkKullanicilar"].ToString());
                        //lueSatisTipi.EditValue = int.Parse(dt.Rows[0]["fkSatisDurumu"].ToString());
                        //seYazdirmaAdedi.EditValue = dt.Rows[0]["YazdirmaAdedi"].ToString();

                        dt.Dispose();
                    }
                }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (form_id == 1) //satış modülünden ise
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Aciklama", GrupAdi.Text));
                list.Add(new SqlParameter("@YaziciAdi",cbYazicilar.Text));

                if (dosyaadi.Text.IndexOf(".")==-1)
                    list.Add(new SqlParameter("@Dosya", dosyaadi.Text));
                else
                list.Add(new SqlParameter("@Dosya", dosyaadi.Text.Substring(0,dosyaadi.Text.IndexOf("."))));

                list.Add(new SqlParameter("@fkKullanicilar", lueKullanicilar.EditValue.ToString()));
                list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
                if (seYazdirmaAdedi.Value == 0) seYazdirmaAdedi.Value = 1;
                list.Add(new SqlParameter("@YazdirmaAdedi", seYazdirmaAdedi.Value));
                list.Add(new SqlParameter("@sec", cbAktif.Checked));
                list.Add(new SqlParameter("@onizle", cbOnizle.Checked));


                if (Tag.ToString()=="0")
                DB.ExecuteSQL(@"INSERT INTO SatisFisiSecimi (Aciklama,YazdirmaAdedi,ResimYol,YaziciAdi,Dosya,SiraNo,fkKullanicilar,fkSatisDurumu,sec,onizle ) 
                VALUES(@Aciklama,@YazdirmaAdedi,'C:',@YaziciAdi,@Dosya,1,@fkKullanicilar,@fkSatisDurumu,@sec,@onizle)", list);
            }
            else
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@SablonAdi", GrupAdi.Text));
                list.Add(new SqlParameter("@DosyaYolu", dosyaadi.Text));
                list.Add(new SqlParameter("@fkKullanicilar", lueKullanicilar.EditValue.ToString()));
                list.Add(new SqlParameter("@YaziciAdi", cbYazicilar.Text));
                DB.ExecuteSQL("INSERT INTO EtiketSablonlari (SablonAdi,YaziciAdi,DosyaYolu,Aktif,SiraNo,Varsayilan,fkKullanicilar)"+
                " VALUES(@SablonAdi,@YaziciAdi,@DosyaYolu,1,0,0,@fkKullanicilar)", list);
            }
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void frmStokKoduverKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string yol = Application.StartupPath.ToString();
            openFileDialog1.InitialDirectory = yol;
            //openFileDialog1.DefaultExt = yol;

            if (System.Windows.Forms.DialogResult.Cancel == openFileDialog1.ShowDialog()) return;
            dosyaadi.Text = openFileDialog1.SafeFileName;
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (seYazdirmaAdedi.Value == 0) seYazdirmaAdedi.Value = 1;

            if (form_id == 1) //satış modülünden ise
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Aciklama", GrupAdi.Text));
                list.Add(new SqlParameter("@YaziciAdi", cbYazicilar.Text));

                if (dosyaadi.Text.IndexOf(".") == -1)
                    list.Add(new SqlParameter("@Dosya", dosyaadi.Text));
                else
                    list.Add(new SqlParameter("@Dosya", dosyaadi.Text.Substring(0, dosyaadi.Text.IndexOf("."))));

                list.Add(new SqlParameter("@fkKullanicilar", lueKullanicilar.EditValue.ToString()));
                list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
                if (seYazdirmaAdedi.Value == 0) seYazdirmaAdedi.Value = 1;
                list.Add(new SqlParameter("@YazdirmaAdedi", seYazdirmaAdedi.Value));
                list.Add(new SqlParameter("@SiraNo", seSiraNo.Value));
                list.Add(new SqlParameter("@Sec", cbAktif.Checked));
                list.Add(new SqlParameter("@onizle", cbOnizle.Checked));


                if (Tag.ToString() == "0")
                    DB.ExecuteSQL(@"update SatisFisiSecimi set Aciklama=@Aciklama,YaziciAdi=@YaziciAdi,Dosya=@Dosya,
                    fkSatisDurumu=@fkSatisDurumu,YazdirmaAdedi=@YazdirmaAdedi,fkKullanicilar=@fkKullanicilar,SiraNo=@SiraNo,
                    Sec=@Sec,onizle=@onizle where pkSatisFisiSecimi=" + pkSatisFisiSecimi.Text, list);
            }
            else if (form_id == 2) //Etiket Şablonları
            {
                //SablonAdi,YaziciAdi,DosyaYolu,Aktif,SiraNo,Varsayilan
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@SablonAdi", GrupAdi.Text));

                if (dosyaadi.Text.IndexOf(".") == -1)
                    list.Add(new SqlParameter("@DosyaYolu", dosyaadi.Text));
                else
                    list.Add(new SqlParameter("@DosyaYolu", dosyaadi.Text.Substring(0, dosyaadi.Text.IndexOf("."))));

                list.Add(new SqlParameter("@YaziciAdi", cbYazicilar.Text));

                list.Add(new SqlParameter("@fkKullanicilar", lueKullanicilar.EditValue.ToString()));

                if (pkEtiketSablonlari.Text == "0")
                    DB.ExecuteSQL("INSERT INTO EtiketSablonlari (SablonAdi,YaziciAdi,DosyaYolu,Aktif,SiraNo,fkKullanicilar)"+
                        " VALUES(@SablonAdi,'Yazici Seçiniz',@DosyaYolu,1,0,@fkKullanicilar)", list);
                else
                {
                    list.Add(new SqlParameter("@pkEtiketSablonlari", pkEtiketSablonlari.Text));
                    
                    DB.ExecuteSQL("update EtiketSablonlari set SablonAdi=@SablonAdi,YaziciAdi=@YaziciAdi,DosyaYolu=@DosyaYolu,Aktif=1,SiraNo=0"+
                        ",fkKullanicilar=@fkKullanicilar where pkEtiketSablonlari=@pkEtiketSablonlari", list);
                }
            }
            Close();
        }

        private void btnRaporYukle_Click(object sender, EventArgs e)
        {
            if (pkSatisFisiSecimi.Text == "")
            {
                MessageBox.Show("Önce Raporu Kaydedin");
                return;
            }

            OpenFileDialog fdialog = new OpenFileDialog();
            fdialog.Filter = "Raporlar|*.repx";
            fdialog.InitialDirectory = "C://";

            if (DialogResult.OK == fdialog.ShowDialog())
            {
                ArrayList alist = new ArrayList();
                //alist.Add(new SqlParameter("@rapor_adi", txtRaporAdi.Text));
                //alist.Add(new SqlParameter("@aciklama", txtAciklama.Text));
                //alist.Add(new SqlParameter("@modul_kod", txtModulKod.Text));
                //alist.Add(new SqlParameter("@aktif_mi", cbAktif_mi.Checked));


                string resimYol = fdialog.FileName;
                txtDosyaYolu.Text = resimYol;

                byte[] byteResim = null;
                FileInfo fInfo = new FileInfo(resimYol);
                long sayac = fInfo.Length;
                FileStream fStream = new FileStream(resimYol, FileMode.Open, FileAccess.Read);
                BinaryReader bReader = new BinaryReader(fStream);
                byteResim = bReader.ReadBytes((int)sayac);

                alist.Add(new SqlParameter("@rapor_dosya", byteResim));

                alist.Add(new SqlParameter("@pkSatisFisiSecimi", pkSatisFisiSecimi.Text));

                string sql = @"update SatisFisiSecimi set guncelleme_tarihi=getdate(),rapor_dosya=@rapor_dosya where pkSatisFisiSecimi=@pkSatisFisiSecimi";


                string sonuc = DB.ExecuteSQL(sql, alist);
                if (sonuc == "0")
                    lbDurum.Text = "Bilgiler Güncellendi";
                else
                    lbDurum.Text = sonuc;
            }
        }
    }
}
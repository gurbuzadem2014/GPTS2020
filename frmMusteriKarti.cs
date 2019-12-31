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
using GPTS.Include.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using System.IO;
using GPTS.islemler;

namespace GPTS
{
    
    public partial class frmMusteriKarti : DevExpress.XtraEditors.XtraForm
    {
        bool ilkyukleme = false;
        string spkFirma = "0", _musteri_adi="";
        
        public frmMusteriKarti(string fkFirma,string musteri_adi)
        {
            InitializeComponent();

            ilkyukleme = true;

            int gen = Screen.PrimaryScreen.WorkingArea.Width;

            if (gen < 769)
                gen = 20;
            else
                gen = gen * 55 / 100;

            //this.Width = Screen.PrimaryScreen.WorkingArea.Width - gen;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 10;

            spkFirma = fkFirma;
            DB.PkFirma = int.Parse(fkFirma);
            pkFirma.Text = fkFirma;
            _musteri_adi = musteri_adi;
        }

        void FiyatGruplari()
        {
            lueFiyatGrubu.Properties.DataSource = DB.GetData("Select * from SatisFiyatlariBaslik with(nolock)");
            lueFiyatGrubu.ItemIndex = 0;
        }

        void vYetkiler()
        {
            DataTable dtYetkiler = DB.GetData("SELECT  pkModuller, ModulAdi, durumu, Kod FROM Moduller with(nolock) where Kod='15'");

            if (dtYetkiler.Rows.Count > 0)
            {
                if (dtYetkiler.Rows[0]["durumu"].ToString() != "True")
                {
                    MessageBox.Show("Bu Modüle Girme Yetkiniz Bulunmamaktadır.\n Lütfen Hitit Yazılımı Arayınız!");
                    Close();
                }
            }
        }

        private void frmKurumKarti_Load(object sender, EventArgs e)
        {
            //Interface Çalışmaları borcu var mı Arayüz Yap.
            /*
            List<common_katman.Firmalar> musteriler = new List<common_katman.Firmalar>();
            
            common_katman.Musteriler mus = new common_katman.Musteriler();
            mus.pkFirma = 1;
            mus.Firmaadi = "test1";
            musteriler.Add(mus);
            
            common_katman.CMusteriOdemeYap ode = new common_katman.CMusteriOdemeYap();
            //ode.OdemeYap();
            ode.pkFirma = 2;
            ode.Firmaadi = "ödemeci";
            bool b = ode.OdemeYap(1);
            double borcu = ode.OdemesiVarmi(1);
            musteriler.Add(ode);
            common_katman.GenelMusteriler gMus = new common_katman.GenelMusteriler();
            gMus.OdemeYap(1);

            foreach (var item in musteriler)
            {
                bool bo = false;
                if (item is common_katman.IOdemeYapar)
                {
                    bo= ((common_katman.IOdemeYapar)item).OdemeYap(1);
                }
                double d = 0;
                if (item is common_katman.IOdemesiVarmi)
                {
                    d= ((common_katman.IOdemesiVarmi)item).OdemesiVarmi(1);
                }
            }
            */

            layoutControlGroup2.Tag = -1;

            vYetkiler();
            proSubeler();
            proSirketler();
            proiller();
            proGruplar();
            FiyatGruplari();
            PlasiyerleriGetir();
            KanGruplari();
            OgranimDurumlari();
            Tedarikciler();

            if (DB.PkFirma == 0)
            {
                //simpleButton1_Click(sender, e);
                YeniMusteri();

                btnKaydet.Text = "Kaydet";
                lUEGrup.EditValue = 1;
                //DB.GetData("select * from Sirketler with(nolock)");
                LUESehir.EditValue = Degerler.fkilKoduDefault;
                lookUpilce.EditValue = Degerler.fkilceAltGrupDefault;
                pictureEdit1.EditValue = null;
            }
            else
            {
                TumBilgileriGetir();
            }

            EkranTasarimGetir();

            ilkyukleme = false;
        }

        void TumBilgileriGetir()
        {
            #region Firma Bilgileri
            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + DB.PkFirma);
            if (dt.Rows.Count > 0)
            {
                btnKaydet.Text = "Güncelle";

                pkFirma.Text = dt.Rows[0]["pkFirma"].ToString();
                if (pkFirma.Text == "1") txtMusteriAdi.Properties.ReadOnly =txtFaturaUnvani.Properties.ReadOnly= true;

                txtMusteriAdi.Text = dt.Rows[0]["Firmaadi"].ToString();
                telno.Text = dt.Rows[0]["Tel"].ToString();
                tel2.Text = dt.Rows[0]["Tel2"].ToString();
                belgecfax.Text = dt.Rows[0]["Fax"].ToString();
                Cep.Text = dt.Rows[0]["Cep"].ToString();
                Cep2.Text = dt.Rows[0]["Cep2"].ToString();
                web.Text = dt.Rows[0]["webadresi"].ToString();
                eposta.Text = dt.Rows[0]["eposta"].ToString();
                mAdres.Text = dt.Rows[0]["Adres"].ToString();
                tEYetkili.Text = dt.Rows[0]["Yetkili"].ToString();
                decimal LimitBorc = 0;
                decimal.TryParse(dt.Rows[0]["LimitBorc"].ToString(), out LimitBorc);
                ceLimitBorc.Value = LimitBorc;
                if (dt.Rows[0]["fkSirket"].ToString() != "")
                    lUSirket.EditValue = int.Parse(dt.Rows[0]["fkSirket"].ToString());
                if (dt.Rows[0]["fkFirmaGruplari"].ToString() == "")
                    lUEGrup.EditValue = 0;
                else
                    lUEGrup.EditValue = int.Parse(dt.Rows[0]["fkFirmaGruplari"].ToString());
                Application.DoEvents();//gruplar eklensin
                if (dt.Rows[0]["fkFirmaAltGruplari"].ToString() == "")
                    lueAltGrup.EditValue = "0";
                else
                    lueAltGrup.EditValue = int.Parse(dt.Rows[0]["fkFirmaAltGruplari"].ToString());
                int il = 0;
                int.TryParse(dt.Rows[0]["fkil"].ToString(), out il);
                LUESehir.EditValue = il;

                int ilce = 0;
                int.TryParse(dt.Rows[0]["fkilce"].ToString(), out ilce);
                lookUpilce.EditValue = ilce;

                int mahalle = 0;
                int.TryParse(dt.Rows[0]["mahalle_id"].ToString(), out mahalle);
                lueMahalle.EditValue = mahalle;

                VergiDairesi.Text = dt.Rows[0]["VergiDairesi"].ToString();
                VergiNo.Text = dt.Rows[0]["VergiNo"].ToString();
                teUnvani.Text = dt.Rows[0]["Unvani"].ToString();
                if (dt.Rows[0]["KaraListe"].ToString() == "True")
                    cbKaraliste.Checked = true;
                if (dt.Rows[0]["Aktif"].ToString() == "True")
                    cbDurumu.SelectedIndex = 0;
                else
                    cbDurumu.SelectedIndex = 1;
                MusteriKodu.Text = dt.Rows[0]["OzelKod"].ToString();
                Bonus.Text = dt.Rows[0]["Bonus"].ToString();
                string fkSatisFiyatlariBaslik = dt.Rows[0]["fkSatisFiyatlariBaslik"].ToString();
                if (fkSatisFiyatlariBaslik == "" || fkSatisFiyatlariBaslik == "0")
                    lueFiyatGrubu.ItemIndex = 0;
                else
                    lueFiyatGrubu.EditValue = int.Parse(fkSatisFiyatlariBaslik);

                txtFaturaUnvani.Text = dt.Rows[0]["FaturaUnvani"].ToString();

                if (dt.Rows[0]["AnlasmaTarihi"].ToString() != "")
                    deAnlasmaTarihi.EditValue = dt.Rows[0]["AnlasmaTarihi"].ToString();

                if (dt.Rows[0]["AnlasaBitisTarihi"].ToString() != "")
                    deAnlasmaTarihiBiris.EditValue = dt.Rows[0]["AnlasaBitisTarihi"].ToString();

                if (dt.Rows[0]["Devir"].ToString() != "")
                    ceDevir.EditValue = dt.Rows[0]["Devir"].ToString();

                if (dt.Rows[0]["OdemeGunSayisi"].ToString() != "")
                    seOdemeGunu.EditValue = dt.Rows[0]["OdemeGunSayisi"].ToString();

                string fkPerTeslimEden = dt.Rows[0]["fkPerTeslimEden"].ToString();
                if (!string.IsNullOrEmpty(fkPerTeslimEden))
                    lUEPlasiyerler.EditValue = int.Parse(fkPerTeslimEden);

                string fkSube = dt.Rows[0]["fkSube"].ToString();
                if (fkSube == "" || fkSube == "0")
                    fkSube = "1";

                    lueSubeler.EditValue = int.Parse(fkSube);

                //string kan_grubu_id = dt.Rows[0]["kan_grubu_id"].ToString();
                //if (!string.IsNullOrEmpty(kan_grubu_id))
                //    lueKanGrubu.EditValue = int.Parse(kan_grubu_id);

                //if (dt.Rows[0]["boyu"].ToString() != "")
                //    ceBoyu.Value = decimal.Parse(dt.Rows[0]["boyu"].ToString());

                //if (dt.Rows[0]["kilosu"].ToString() != "")
                //    ceKilosu.Value = decimal.Parse(dt.Rows[0]["kilosu"].ToString());

                //txtTC.Text = dt.Rows[0]["tckimlikno"].ToString();

                //string fkOgrenimDurumu = dt.Rows[0]["fkOgrenimDurumu"].ToString();
                //if (!string.IsNullOrEmpty(fkOgrenimDurumu))
                //    lueOgranimDurumu.EditValue = int.Parse(fkOgrenimDurumu);

                //cbCinsiyet.Text = dt.Rows[0]["cinsiyet"].ToString();
                //cbMedeniHali.Text = dt.Rows[0]["medeni_hali"].ToString();
            }
            #endregion

            KimlikBilgileriGetir();

            OzelNotGetir();

            ResimGetirUrl(); 
        }
        void EkranTasarimGetir()
        {
            string Genel = DB.Sektor;// "Genel";
            string layoutFileName = Application.StartupPath + "\\EkranTasarimlari\\" + Genel + "\\MusteriKarti.xml";
            if (System.IO.File.Exists(layoutFileName))
                layoutControl1.RestoreLayoutFromXml(layoutFileName);
            else
                MessageBox.Show(layoutFileName + " Dosya Bulunamadı!");
        }

        void OzelNotGetir()
        {
            DataTable dtOzelNot = DB.GetData("select * from FirmaOzelNot with(nolock) where fkFirma=" + pkFirma.Text);
            if (dtOzelNot.Rows.Count == 0)
                memoozelnot.Text = "";
            else
            {
                memoozelnot.Text = dtOzelNot.Rows[0]["OzelNot"].ToString();
                txtMusteriAdi.Focus();
            }
        }

        void ResimGetirUrl()
        {
            //string layoutFileName = Application.StartupPath + "\\EkranTasarimlari\\" + Genel + "\\MusteriKarti.xml";
            string exeyol = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya=exeyol + "\\MusteriKartiResim\\" + pkFirma.Text + ".jpg";
            //if(File.Exists(dosya))
            //    pictureEdit1.Image = Image.FromFile(dosya);

            if (!File.Exists(dosya)) return;

            Image image1 = null;

            using(FileStream stream = new FileStream(dosya, FileMode.Open))
            {
                image1 = Image.FromStream(stream);
            }

            pictureEdit1.Image = image1;

            //image1.Dispose();
        }

        void ResimGetir_()
        {
            DataTable dt =
            DB.GetData("select * from FirmaKartiResimleri with(nolock) where fkFirmalar=" +pkFirma.Text);

            if (dt.Rows.Count == 0)
            {
                pictureEdit1.Image = null;
                pictureEdit1.Tag = "0";
                return;
            }

            pictureEdit1.Tag = dt.Rows[0]["pkFirmaKartiResimleri"].ToString();

            Image UrunResim = null;
            try
            {
                byte[] resim = (byte[])dt.Rows[0]["resimvb"];   //okuyucu[0];
                MemoryStream ms = new MemoryStream(resim, 0, resim.Length);
                ms.Write(resim, 0, resim.Length);
                UrunResim = Image.FromStream(ms, true);
                pictureEdit1.Image = UrunResim;
            }
            catch (Exception)
            {
                //throw;
            }

        }

        void Tedarikciler()
        {
            lueTedarikciler.Properties.DataSource = DB.GetData("select 0 as pkTedarikciler, 'Seçiniz' as Firmaadi union all SELECT pkTedarikciler,Firmaadi FROM Tedarikciler with(nolock) where Aktif=1");
            lueTedarikciler.Properties.ValueMember = "pkTedarikciler";
            lueTedarikciler.Properties.DisplayMember = "Firmaadi";
        }

        void PlasiyerleriGetir()
        {
            lUEPlasiyerler.Properties.DataSource = DB.GetData("select 0 as pkpersoneller, 'Seçiniz' as adi union all SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller with(nolock) where Plasiyer=1 and AyrilisTarihi is null");
            lUEPlasiyerler.Properties.ValueMember = "pkpersoneller";
            lUEPlasiyerler.Properties.DisplayMember = "adi";
            //lUEPlasiyerler.EditValue = 1;
        }

        void KanGruplari()
        {
            lueKanGrubu.Properties.DataSource = DB.GetData("SELECT id,grup FROM kangrubu with(nolock)");
            lueKanGrubu.Properties.ValueMember = "id";
            lueKanGrubu.Properties.DisplayMember = "grup";
            lueKanGrubu.EditValue = 1;
            
        }

        private void OgranimDurumlari()
        {
            lueOgrenimDurumu.Properties.DataSource = DB.GetData("select * from OgrenimDurumu with(nolock)");
            lueOgrenimDurumu.Properties.ValueMember = "PkOgrenimDutrumu";
            lueOgrenimDurumu.Properties.DisplayMember = "OgrenimDurumu";
            //throw new NotImplementedException();
        }

        void YeniMusteri()
        {
            DB.PkFirma = 0;
            pkFirma.Text = "0";
            txtMusteriAdi.Text = _musteri_adi;// txtMusteriAdi.ToolTip;
            telno.Text = "0";
            tel2.Text = "0";
            mAdres.Text = "";
            web.Text = "";
            eposta.Text = "@";
            btnKaydet.Text = "Kaydet";
            tEYetkili.Text = "";
            teUnvani.Text = "";
            belgecfax.Text = "";
            memoozelnot.Text = "";
            lUEGrup.EditValue = 1;
            ceLimitBorc.Value = 0;
            cbKaraliste.Checked = false;
            cbDurumu.SelectedIndex = 0;
            if (Cep.Tag == null)
                Cep.Text = "";

            MusteriKodu.Text = "";
            lueFiyatGrubu.EditValue = 0;
            txtFaturaUnvani.Text = "";
            deAnlasmaTarihiBiris.EditValue = null;
            seOdemeGunu.Value = 0;
            deAnlasmaTarihi.EditValue = null;

            LUESehir.EditValue = Degerler.fkilKoduDefault;
            lookUpilce.EditValue = Degerler.fkilceAltGrupDefault;
            memoozelnot.Text = "";
            Bonus.Value = 0;

            VergiDairesi.Text = "";
            VergiNo.Text = "";
            lueKanGrubu.EditValue = 0;
            pictureEdit1.EditValue = null;

            KimlikBilgileriTemizle();

            txtMusteriAdi.Focus();
        }
        void KimlikBilgileriTemizle()
        {
            layoutControlGroup2.Tag = -1;
            txtTC.Text = "";
            cbMedeniHali.Text = "";
            cbCinsiyet.Text = "";
            txtAnneAdi.Text = "";
            txtBabaAdi.Text = "";
            txtCiltNo.Text = "";
            txtDini.Text = "İSLAM";
            txtAileSiraNo.Text = "";
            txtSiraNo.Text = "";
            txtisAdresi.Text = "";
            txtAcilDurumKisi.Text = "";
            txtAcilDurumTel.Text = "";
            txtisTel.Text = "";
            lueOgrenimDurumu.EditValue = 0;
            lueKanGrubu.EditValue = 0;
            deDogumTarihi.EditValue = null;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _musteri_adi = "";
            YeniMusteri();
        }

        void proSirketler()
        {
            DataTable dtb = DB.GetData("select * from Sirketler with(nolock)");
            lUSirket.Properties.DataSource = dtb;
            lUSirket.Properties.ValueMember = "pkSirket";
            lUSirket.Properties.DisplayMember = "Sirket";
            lUSirket.EditValue = 1;
        }

        void proSubeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData("select * from Subeler with(nolock)");
            lueSubeler.EditValue = 1;
        }

        void proGruplar()
        {
            lUEGrup.Properties.DataSource = DB.GetData("select * from FirmaGruplari with(nolock)");
        }


        private void frmKurumKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.F7)
            {
                YeniMusteri();
            }
            else if (e.KeyCode == Keys.F9)
            {
                btnKaydet_Click(sender, null);
            }
            else if (e.KeyCode == Keys.F11)
            {
                btnyazdir_Click(sender, null);
            }
           
        }

        private void labelControl26_Click(object sender, EventArgs e)
        {
            frmMusteriGruplari CariGrupKarti = new frmMusteriGruplari();
            CariGrupKarti.ShowDialog();
            proGruplar();
        }
        void altgrupgetir()
        {
            lueAltGrup.Properties.DataSource = DB.GetData("select * from FirmaAltGruplari where Aktif=1 and fkFirmaGruplari=" + lUEGrup.EditValue.ToString());
            lueAltGrup.EditValue = int.Parse(lueAltGrup.Tag.ToString());
        }
        private void lUEGrup_EditValueChanged(object sender, EventArgs e)
        {
            altgrupgetir();
            //if (lUEGrup.EditValue.ToString() == "4")
            //{
            //    xtraTabControl1.TabPages[2].PageVisible = false;
            //    xtraTabControl1.TabPages[1].PageVisible = true;
            //}
            //else
            //{
            //    //xtraTabControl1.TabPages[2].PageVisible = true;
            //    xtraTabControl1.TabPages[1].PageVisible = false;
            //}
        }

        private void LUESehir_EditValueChanged(object sender, EventArgs e)
        {
            proilceler(LUESehir.EditValue.ToString());
        }

        void proiller()
        {
            DataTable dt = DB.GetData(@"SELECT ADI,KODU FROM IL_ILCE_MAH with(nolock)
                                         WHERE GRUP=1  ORDER BY KODU");
            LUESehir.Properties.DataSource = dt;
            LUESehir.Properties.ValueMember = "KODU";
            LUESehir.Properties.DisplayMember = "ADI";
            LUESehir.EditValue = Degerler.fkilKoduDefault;
            lookUpilce.EditValue = Degerler.fkilceAltGrupDefault;

            lueil.Properties.DataSource = dt;
            lueil.Properties.ValueMember = "KODU";
            lueil.Properties.DisplayMember = "ADI";
            lueil.EditValue = Degerler.fkilKoduDefault;
        }

        void proilceler(string ilid)
        {
            DataTable dt = DB.GetData(@"SELECT ADI,KODU FROM IL_ILCE_MAH with(nolock)
                                         WHERE ALTGRUP=" + ilid + "  ORDER BY KODU");
            lookUpilce.Properties.DataSource = dt;
            lookUpilce.Properties.ValueMember = "KODU";
            lookUpilce.Properties.DisplayMember = "ADI";
        }

        void dogumilceler(string ilid)
        {
            DataTable dt = DB.GetData(@"SELECT ADI,KODU FROM IL_ILCE_MAH with(nolock)
                                         WHERE ALTGRUP=" + ilid + "  ORDER BY KODU");
            lueilce.Properties.DataSource = dt;
            lueilce.Properties.ValueMember = "KODU";
            lueilce.Properties.DisplayMember = "ADI";
        }
        void OzelNotKaydet()
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", pkFirma.Text));
            list.Add(new SqlParameter("@OzelNot", memoozelnot.Text));
            DataTable dt = DB.GetData("select * from FirmaOzelNot with(nolock) where fkFirma=" + pkFirma.Text);
            if (dt.Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO FirmaOzelNot (fkFirma,OzelNot) VALUES(@fkFirma,@OzelNot)", list);
            else
                DB.ExecuteSQL("UPDATE FirmaOzelNot SET OzelNot=@OzelNot WHERE fkFirma=@fkFirma", list);
            memoozelnot.Tag = 0;
        }

        private void memoozelnot_Leave(object sender, EventArgs e)
        {
            if(memoozelnot.Tag.ToString() == "1")
                OzelNotKaydet();
        }

        private void memoozelnot_EditValueChanged(object sender, EventArgs e)
        {
            memoozelnot.Tag = 1;
        }

        private void frmKurumKarti_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (memoozelnot.Tag.ToString() == "1")
                OzelNotKaydet();
        }

        private void lookUpilce_EditValueChanged(object sender, EventArgs e)
        {
            //önce ilçe kodlarını eşleştirmek gerekiyor.
//            lueMahalle.Properties.DataSource = DB.GetData(@"select m.mahalle_id,m.mahalle_adi,m.mahalle_kodu from TIlce ilce with(nolock)
//inner join TBucak b with(nolock) on b.ilce_kodu=ilce.ilce_kodu
//inner join TKoy k with(nolock) on k.bucak_kodu=b.bucak_kodu
//inner join TMahalle m with(nolock) on m.koy_kodu= k.koy_kodu
//where ilce.ilce_kodu='" + lookUpilce.EditValue.ToString()+ "'");
            if (LUESehir.EditValue == null || lookUpilce.EditValue == null) return;

            string s = "SELECT ADI, KODU,ilce_kodu FROM  dbo.IL_ILCE_MAH" +
            " WHERE  (GRUP = '2') and IL_ILCE_MAH.KODU=" + lookUpilce.EditValue.ToString() + " and ALTGRUP=" + LUESehir.EditValue.ToString();

            DataTable dt = DB.GetData(s);
            if (dt.Rows.Count > 0 && dt.Rows[0]["ilce_kodu"].ToString() != "")
            {
                lueMahalle.Properties.DataSource = DB.GetData(@"select * from VMahalleler where ilce_kodu=" + dt.Rows[0]["ilce_kodu"].ToString());
                lueMahalle.Properties.DisplayMember = "mahalle_adi";
                lueMahalle.Properties.ValueMember = "mahalle_id";
            }
            else
            {
                lueMahalle.Properties.DataSource = DB.GetData(@"select top 0 * from VMahalleler");
                lueMahalle.Properties.DisplayMember = "mahalle_adi";
                lueMahalle.Properties.ValueMember = "mahalle_id";
            }
        }
        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (txtMusteriAdi.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Adı Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMusteriAdi.Focus();
                dxErrorProvider1.SetError(txtMusteriAdi, "Müşteri Adı Boş Olamaz!");
                return;
            }
            if (lUSirket.EditValue == null)
            {
                //DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Adı Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUSirket.Focus();
                dxErrorProvider1.SetError(lUSirket, "Müşterinin Bağlı olduğu Şirketi Seçiniz!");
                //return;
                lUSirket.EditValue = 1;
            }
           
            //if (lUEGrup.EditValue == null)
            //{
            //    dxErrorProvider1.SetError(lUEGrup, "Müşteri Grubu Boş Olamaz!");
            //    //DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Grubu Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    lUEGrup.Focus();
            //    return;
            //}
            if (cbDurumu.EditValue == null)
            {
                dxErrorProvider1.SetError(cbDurumu, "Müşteri Durumu Boş Olamaz!");
                //DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Grubu Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbDurumu.Focus();
                return;
            }

            string sql = "";
            ArrayList plist = new ArrayList();
            if (pkFirma.Text == "0" | pkFirma.Text == "")
            {
                sql = @"insert into Firmalar (Firmaadi,tel,tel2,fax,OdemeIlk,OdemeSon,webadresi,eposta,adres,Aktif,KaraListe,fkSirket,Yetkili,
                KadroSayisi,LimitBorc,fkFirmaGruplari,fkil,fkilce,VergiDairesi,VergiNo,Unvani,Cep,Cep2,OzelKod,fkFirmaAltGruplari,Borc,Alacak,
                Bonus,fkSatisFiyatlariBaslik,FaturaUnvani,Devir,AnlasmaTarihi,AnlasaBitisTarihi,OdemeGunSayisi,Aktarildi,fkPerTeslimEden,
                fkTedarikciler,mahalle_id,fksube) 
                values(@Firmaadi,@tel,@tel2,@fax,@OdemeIlk,@OdemeSon,@webadresi,@eposta,@adres,@Aktif,@KaraListe,@fkSirket,@Yetkili,
                @KadroSayisi,@LimitBorc,@fkFirmaGruplari,@fkil,@fkilce,@VergiDairesi,@VergiNo,@Unvani,@Cep,@Cep2,@OzelKod,@fkFirmaAltGruplari,0,0,
                0,@fkSatisFiyatlariBaslik,@FaturaUnvani,0,@AnlasmaTarihi,@AnlasaBitisTarihi,@OdemeGunSayisi,0,@fkPerTeslimEden,
                @fkTedarikciler,@mahalle_id,@fkSube) 
                SELECT IDENT_CURRENT('Firmalar')";
            }
            else
            {
                sql = @"update Firmalar set Firmaadi=@Firmaadi,tel=@tel,tel2=@tel2,fax=@fax,fkSirket=@fkSirket,OdemeSon=@OdemeSon,
                        eposta=@eposta,webadresi=@webadresi,adres=@adres,Aktif=@Aktif,KaraListe=@KaraListe,OdemeIlk=@OdemeIlk,Yetkili=@Yetkili,
                        KadroSayisi=@KadroSayisi,LimitBorc=@LimitBorc,fkFirmaGruplari=@fkFirmaGruplari,
                        fkil=@fkil,fkilce=@fkilce,VergiDairesi=@VergiDairesi,VergiNo=@VergiNo,Unvani=@Unvani,Cep=@Cep,Cep2=@Cep2,OzelKod=@OzelKod,
                        fkFirmaAltGruplari=@fkFirmaAltGruplari,Bonus=@Bonus,fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik,FaturaUnvani=@FaturaUnvani,
                        AnlasmaTarihi=@AnlasmaTarihi,AnlasaBitisTarihi=@AnlasaBitisTarihi,OdemeGunSayisi=@OdemeGunSayisi,Aktarildi=0,
                        fkPerTeslimEden=@fkPerTeslimEden,fkTedarikciler=@fkTedarikciler,mahalle_id=@mahalle_id,fkSube=@fkSube
                        where pkFirma=@pkFirma";
            }

            plist.Add(new SqlParameter("@pkFirma", pkFirma.Text));
            plist.Add(new SqlParameter("@Firmaadi", txtMusteriAdi.Text));
            plist.Add(new SqlParameter("@tel", telno.Text));
            plist.Add(new SqlParameter("@tel2", tel2.Text));
            plist.Add(new SqlParameter("@Cep", Cep.Text));
            plist.Add(new SqlParameter("@Cep2", Cep2.Text));
            plist.Add(new SqlParameter("@OdemeIlk", "0"));//seOdemeIlk.Value));
            plist.Add(new SqlParameter("@fax", belgecfax.Text));
            plist.Add(new SqlParameter("@fkSirket", lUSirket.EditValue));
            plist.Add(new SqlParameter("@webadresi", web.Text));
            plist.Add(new SqlParameter("@eposta", eposta.Text));
            plist.Add(new SqlParameter("@adres", mAdres.Text));
            plist.Add(new SqlParameter("@OdemeSon", "0"));//seOdemeSon.Value));
            plist.Add(new SqlParameter("@Yetkili", tEYetkili.Text));
            plist.Add(new SqlParameter("@KadroSayisi", "0"));

            if (lUEGrup.EditValue == null)
                plist.Add(new SqlParameter("@fkFirmaGruplari", 0));
            else
                plist.Add(new SqlParameter("@fkFirmaGruplari", int.Parse(lUEGrup.EditValue.ToString())));
            //alt grup
            if (lueAltGrup.EditValue == null)
                plist.Add(new SqlParameter("@fkFirmaAltGruplari", 0));
            else
                plist.Add(new SqlParameter("@fkFirmaAltGruplari", int.Parse(lueAltGrup.EditValue.ToString())));
            //il
            if (LUESehir.EditValue == null)
                plist.Add(new SqlParameter("@fkil", 0));
            else
                plist.Add(new SqlParameter("@fkil", LUESehir.EditValue.ToString()));
            //ilçe
            if (lookUpilce.EditValue == null)
                plist.Add(new SqlParameter("@fkilce", 0));
            else
                plist.Add(new SqlParameter("@fkilce", lookUpilce.EditValue.ToString()));
            //mahalle
            if (lueMahalle.EditValue == null)
                plist.Add(new SqlParameter("@mahalle_id", DBNull.Value));
            else
                plist.Add(new SqlParameter("@mahalle_id", lueMahalle.EditValue.ToString()));

            plist.Add(new SqlParameter("@VergiDairesi", VergiDairesi.Text));
            plist.Add(new SqlParameter("@VergiNo", VergiNo.Text));
            plist.Add(new SqlParameter("@Unvani", teUnvani.Text));
            plist.Add(new SqlParameter("@LimitBorc", ceLimitBorc.Value.ToString().Replace(",", ".")));
            plist.Add(new SqlParameter("@KaraListe", cbKaraliste.Checked));
            if(cbDurumu.SelectedIndex==0)
               plist.Add(new SqlParameter("@Aktif", true));
            else
               plist.Add(new SqlParameter("@Aktif", false));
            //if (MusteriKodu.Text == "")
            //    plist.Add(new SqlParameter("@OzelKod", "0"));
            //else
            plist.Add(new SqlParameter("@OzelKod", MusteriKodu.Text));
            plist.Add(new SqlParameter("@Bonus", Bonus.Value.ToString().Replace(",",".")));
            if (lueFiyatGrubu.EditValue==null)
                plist.Add(new SqlParameter("@fkSatisFiyatlariBaslik", "1"));
            else
                plist.Add(new SqlParameter("@fkSatisFiyatlariBaslik", lueFiyatGrubu.EditValue.ToString()));
            
            if (txtFaturaUnvani.Text == "") txtFaturaUnvani.Text = txtMusteriAdi.Text;
            
            plist.Add(new SqlParameter("@FaturaUnvani", txtFaturaUnvani.Text));
            //sürekli güncellendiği için gerek yok
            //plist.Add(new SqlParameter("@Devir", ceDevir.Value));
            if (deAnlasmaTarihi.EditValue==null)
                plist.Add(new SqlParameter("@AnlasmaTarihi", DBNull.Value));
            else
                plist.Add(new SqlParameter("@AnlasmaTarihi", deAnlasmaTarihi.DateTime));
                
            if (deAnlasmaTarihiBiris.EditValue==null)
                plist.Add(new SqlParameter("@AnlasaBitisTarihi", DBNull.Value));
            else
                plist.Add(new SqlParameter("@AnlasaBitisTarihi", deAnlasmaTarihiBiris.DateTime));

            if (seOdemeGunu.Value > 28) seOdemeGunu.Value = 28;

            plist.Add(new SqlParameter("@OdemeGunSayisi", seOdemeGunu.Value));

            if (lUEPlasiyerler.EditValue==null)
                plist.Add(new SqlParameter("@fkPerTeslimEden",DBNull.Value));
            else
                plist.Add(new SqlParameter("@fkPerTeslimEden", lUEPlasiyerler.EditValue));

            if (lueTedarikciler.EditValue == null)
                 plist.Add(new SqlParameter("@fkTedarikciler", DBNull.Value));
            else
                 plist.Add(new SqlParameter("@fkTedarikciler", lueTedarikciler.EditValue));

            if (lueSubeler.EditValue == null)
                plist.Add(new SqlParameter("@fkSube",Degerler.fkSube));
            else
                plist.Add(new SqlParameter("@fkSube", lueSubeler.EditValue));


            //plist.Add(new SqlParameter("@kan_grubu_id", lueKanGrubu.EditValue));

            //plist.Add(new SqlParameter("@boyu", ceBoyu.Value));
            //plist.Add(new SqlParameter("@kilosu", ceKilosu.Value));
            //plist.Add(new SqlParameter("@tckimlikno", txtTC.Text));

            //if (lueOgrenimDurumu.EditValue == null)
            //    plist.Add(new SqlParameter("@fkOgrenimDurumu", 0));
            //else
            //    plist.Add(new SqlParameter("@fkOgrenimDurumu", int.Parse(lueOgrenimDurumu.EditValue.ToString())));
            //plist.Add(new SqlParameter("@cinsiyet", cbCinsiyet.Text));
            //plist.Add(new SqlParameter("@medeni_hali", cbMedeniHali.Text));
            string sonuc ="";
            if (pkFirma.Text == "0" | pkFirma.Text == "")
            {
                sonuc = DB.ExecuteScalarSQL(sql, plist);
                pkFirma.Text = sonuc;// DB.GetData("select MAX(PkFirma) as c from Firmalar").Rows[0][0].ToString();
                DB.PkFirma = int.Parse(sonuc);
                //MusteriKodu.Text = sonuc;

                //DB.ExecuteSQL("UPDATE Firmalar SET OzelKod=pkFirma where pkFirma=" + sonuc);
                if (sonuc == "1")
                {
                    DB.ExecuteSQL("UPDATE Firmalar SET GeciciMusteri=1 where pkFirmalar=1");
                }
                else
                {
                    int i = 1;
                    int.TryParse(DB.GetData("SELECT top 1 isnull(MusteriBasNo,1) from Sirketler").Rows[0][0].ToString(), out i);
                    MusteriKodu.Text = i.ToString();

                    DB.ExecuteSQL("UPDATE Firmalar SET OzelKod=" + MusteriKodu.Text + " where pkFirma=" + pkFirma.Text);

                    DB.ExecuteSQL("update Sirketler set MusteriBasNo=isnull(MusteriBasNo,0)+1");
                }
            }
            else
            {
                sonuc = DB.ExecuteSQL(sql, plist);
            }
            
            frmMesajBox mesaj = new frmMesajBox(1);
            if (sonuc.Substring(0, 1) != "H")
            {
                
                mesaj.label1.Text="Müşteri Bilgileri Kaydedildi.";
                mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;

                KimlikBilgileriKaydet();
            }
            else
            {
                mesaj.label1.Text="Müşteri Bilgilerini Kontrol Ediniz." + sonuc;
                mesaj.label1.BackColor = System.Drawing.Color.Red;
            }
            //caller id için
            Cep.Tag = null;
            //DB.ExecuteSQL("UPDATE Firmalar SET GeciciMusteri=1 WHERE pkFirma=1");
            mesaj.Show();
        }

        void KimlikBilgileriKaydet()
        {
            if (layoutControlGroup2.Tag.ToString() == "-1") return;

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma",pkFirma.Text));
            list.Add(new SqlParameter("@tckimlikno", txtTC.Text));
            list.Add(new SqlParameter("@cinsiyet", cbCinsiyet.Text));
            list.Add(new SqlParameter("@medeni_hali", cbMedeniHali.Text));
            list.Add(new SqlParameter("@boyu", ceBoyu.Value));
            list.Add(new SqlParameter("@kilosu", ceKilosu.Value));
            list.Add(new SqlParameter("@kan_grubu_id", lueKanGrubu.EditValue));

            if (lueOgrenimDurumu.EditValue==null)
                list.Add(new SqlParameter("@fkOgrenimDurumu", "0"));
            else
                list.Add(new SqlParameter("@fkOgrenimDurumu", lueOgrenimDurumu.EditValue));

            list.Add(new SqlParameter("@acil_durum_kisi", txtAcilDurumKisi.Text));
            list.Add(new SqlParameter("@acil_durum_tel", txtAcilDurumTel.Text));
            list.Add(new SqlParameter("@baba_adi", txtBabaAdi.Text));
            list.Add(new SqlParameter("@anne_adi", txtAnneAdi.Text));

            list.Add(new SqlParameter("@dini", txtDini.Text));
            list.Add(new SqlParameter("@is_adresi", txtisAdresi.Text));
            list.Add(new SqlParameter("@is_tel", txtisTel.Text));

            list.Add(new SqlParameter("@cilt_no", txtCiltNo.Text));
            list.Add(new SqlParameter("@aile_sira_no", txtAileSiraNo.Text));
            list.Add(new SqlParameter("@sira_no", txtSiraNo.Text));

            if (deDogumTarihi.EditValue==null)
                list.Add(new SqlParameter("@dogum_tarihi", DBNull.Value));
            else
            list.Add(new SqlParameter("@dogum_tarihi", deDogumTarihi.DateTime));

            list.Add(new SqlParameter("@dogum_yeri", txtDogumYeri.Text));
            list.Add(new SqlParameter("@mahalle_adi", txtMahalleAdi.Text));

            if (lueil.EditValue==null)
                list.Add(new SqlParameter("@dogum_il_id", DBNull.Value));
            else
                list.Add(new SqlParameter("@dogum_il_id",lueil.EditValue.ToString()));

            if (lueilce.EditValue == null)
                list.Add(new SqlParameter("@dogum_ilce_id", DBNull.Value));
            else
                list.Add(new SqlParameter("@dogum_ilce_id", lueilce.EditValue.ToString()));

            DataTable dtKimlik = DB.GetData("select * from KimlikBilgileri with(nolock) where fkfirma=" + pkFirma.Text);
            
            string sql = "";

            if(dtKimlik.Rows.Count==0)
                sql = @"insert into KimlikBilgileri (fkFirma,tckimlikno,cinsiyet,anne_adi,baba_adi,medeni_hali,boyu,kilosu,kan_grubu_id,
                fkOgrenimDurumu,acil_durum_kisi,acil_durum_tel,dini,is_adresi,is_tel,cilt_no,aile_sira_no,sira_no,dogum_tarihi,dogum_yeri,
                mahalle_adi,dogum_il_id,dogum_ilce_id) 
                values(@fkFirma,@tckimlikno,@cinsiyet,@anne_adi,@baba_adi,@medeni_hali,@boyu,@kilosu,@kan_grubu_id,
                @fkOgrenimDurumu,@acil_durum_kisi,@acil_durum_tel,@dini,@is_adresi,@is_tel,@cilt_no,@aile_sira_no,@sira_no,@dogum_tarihi,@dogum_yeri,
                @mahalle_adi,@dogum_il_id,@dogum_ilce_id)";
            else
                sql = @"update KimlikBilgileri set tckimlikno=@tckimlikno,cinsiyet=@cinsiyet,medeni_hali=@medeni_hali,
                boyu=@boyu,kilosu=@kilosu,kan_grubu_id=@kan_grubu_id,fkOgrenimDurumu=@fkOgrenimDurumu,anne_adi=@anne_adi,
                baba_adi=@baba_adi,acil_durum_kisi=@acil_durum_kisi,acil_durum_tel=@acil_durum_tel,dini=@dini,cilt_no=@cilt_no,
                is_adresi=@is_adresi,is_tel=@is_tel,aile_sira_no=@aile_sira_no,sira_no=@sira_no,dogum_tarihi=@dogum_tarihi,dogum_yeri=@dogum_yeri,
                mahalle_adi=@mahalle_adi,dogum_il_id=@dogum_il_id,dogum_ilce_id=@dogum_ilce_id where fkFirma=" + pkFirma.Text;

           string sonuc= DB.ExecuteSQL(sql, list);

           
           if (sonuc.Substring(0, 1) != "H")
           {
               //mesaj.label1.Text = "Müşteri Kimlik Bilgilerini Kaydedildi.";
               //mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
           }
           else
           {
               MessageBox.Show("Hata Oluştur " + sonuc);
               //frmMesajBox mesaj = new frmMesajBox(200);
               //mesaj.label1.Text = "Müşteri Kimlik Bilgilerini Kontrol Ediniz." + sonuc;
               //mesaj.label1.BackColor = System.Drawing.Color.Red;
               //mesaj.Show();
           }
           
        }

        void FirmaResimKaydet()
        {
            try
            {
                OpenFileDialog fdialog = new OpenFileDialog();
                fdialog.Filter = "Pictures|*.jpg";
                fdialog.InitialDirectory = "C://";
                if (DialogResult.OK == fdialog.ShowDialog())
                {
                    ArrayList list = new ArrayList();
                    string sql = "";
                    if (pictureEdit1.Tag.ToString() == "0")
                    {
                        sql = @"INSERT INTO FirmaKartiResimleri (fkFirmalar,aciklama,resimvb,tarih,fkKullanicilar)
                        values (@fkFirmalar,@aciklama,@resimvb,getdate(),-1)";
                        list.Add(new SqlParameter("@aciklama", txtMusteriAdi.Text));
                    }
                    else
                    {
                        sql = @"UPDATE FirmaKartiResimleri SET resimvb=@resimvb,tarih=GETDATE()
                         WHERE fkFirmalar=@fkFirmalar";
                    }
                    list.Add(new SqlParameter("@fkFirmalar", pkFirma.Text));

                    string resimYol = fdialog.FileName;
                    byte[] byteResim = null;
                    FileInfo fInfo = new FileInfo(resimYol);
                    long sayac = fInfo.Length;
                    FileStream fStream = new FileStream(resimYol, FileMode.Open, FileAccess.Read);
                    BinaryReader bReader = new BinaryReader(fStream);
                    byteResim = bReader.ReadBytes((int)sayac);

                    list.Add(new SqlParameter("@resimvb", byteResim));

                    string sonuc = DB.ExecuteSQL(sql, list);
                    if (sonuc == "0")
                    {
                        formislemleri.Mesajform("Resim Kaydedildi","S",100);
                        //MessageBox.Show("Kayıt başarıyla gerçekleşti.");
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                throw;
            }
            finally
            {
                //ResimGetir();
            }
        }

        private void padi_EditValueChanged(object sender, EventArgs e)
        {
            baslik.Text = txtMusteriAdi.Text;
        }

        private void layoutControl1_HideCustomization(object sender, EventArgs e)
        {
            //kapanınca
            string Genel = DB.Sektor;// "Genel";
            layoutControl1.SaveLayoutToXml(Application.StartupPath + "\\EkranTasarimlari\\" + Genel + "\\MusteriKarti.xml");
        }

        private void layoutControl1_ShowCustomization(object sender, EventArgs e)
        {
            //açılınca 
        }

        private void OzelKod_Leave(object sender, EventArgs e)
        {
            //if (OzelKod.OldEditValue == null)
            if (DB.PkFirma == 0)
            {
                if (DB.GetData("Select * from  Firmalar where OzelKod='" + MusteriKodu.Text + "'").Rows.Count > 0)
                {
                    MessageBox.Show("Müşteri Kodu Daha Önce Kullanıldı.");
                    MusteriKodu.Focus();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtMusteriAdi.Focus();
            timer1.Enabled = false;
        }

        private void eposta_Leave(object sender, EventArgs e)
        {
            string mesaj = eposta.Text;

            mesaj = mesaj.Replace("ş", "s");
            mesaj = mesaj.Replace("ü", "u");
            mesaj = mesaj.Replace("ğ", "g");
            mesaj = mesaj.Replace("ö", "o");
            mesaj = mesaj.Replace("ı", "i");
            mesaj = mesaj.Replace("ç", "c");
            mesaj = mesaj.Replace("â", "a");

            mesaj = mesaj.Replace("Ş", "S");
            mesaj = mesaj.Replace("Ü", "U");
            mesaj = mesaj.Replace("Ğ", "G");
            mesaj = mesaj.Replace("Ö", "O");
            mesaj = mesaj.Replace("I", "i");
            mesaj = mesaj.Replace("Ç", "C");
            mesaj = mesaj.Replace("İ", "i");
            eposta.Text = mesaj;

            if (pkFirma.Text == "0"&& eposta.Text.Length > 3)
            {
                DataTable dt =
                DB.GetData("select pkFirma,OzelKod,Firmaadi,Cep from Firmalar with(nolock) where Eposta='" + eposta.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    //DialogResult secim;
                    //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Daha Önce Kaydedilmiş" +
                    //    dt.Rows[0]["OzelKod"].ToString)+" - "+
                    //    dt.Rows[0]["Firmaadi"].ToString) +
                    //    " Müşteri Bulunmaktadır.\r\n Kayda Devam etmek istermisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                    //if (secim == DialogResult.Yes)
                    //{
                    frmMusteriKarti mk = new frmMusteriKarti(dt.Rows[0]["pkFirma"].ToString(), dt.Rows[0]["OzelKod"].ToString());
                    mk.ShowDialog();
                    //}
                }
            }
        }

        private void MusteriKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            MusteriKodu.Properties.ReadOnly = txtFaturaUnvani.Properties.ReadOnly= false;
        }
        DevExpress.XtraGrid.GridControl targetGrid = null;
        DevExpress.XtraGrid.Views.Grid.GridView gridView = null;
        private void padi_KeyUp(object sender, KeyEventArgs e)
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
            if (txtMusteriAdi.Text.Length > 2)
                targetGrid.DataSource = DB.GetData("select pkFirma,Firmaadi,Tel,Adres From Firmalar where Firmaadi like '%" + txtMusteriAdi.Text + "%'"); // populated data table
            //targetGrid.DataSource = DB.GetData("select pkFirma,Firmaadi,Tel,Adres,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye From Firmalar where Firmaadi like '%" + padi.Text + "%'"); // populated data table
            targetGrid.BringToFront();
            targetGrid.Width = 500;
            targetGrid.Height = 300;
            targetGrid.Left = txtMusteriAdi.Left + 2;
            targetGrid.Top = txtMusteriAdi.Top + 117;
            if (gridView.DataRowCount == 0)
                targetGrid.Visible = false;
            else
               targetGrid.Visible = true;
            if(txtMusteriAdi.Text.Length==0)
                targetGrid.Visible = false;
        }
        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
                txtMusteriAdi.Text = dr["Firmaadi"].ToString();
                DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
                frmKurumKarti_Load(sender, e);
                targetGrid.Visible = false;
            }
        }

        private void padi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && targetGrid != null)
            {
                targetGrid.Visible = true;
                targetGrid.Focus();
            }
        }
        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            txtMusteriAdi.Text = dr["Firmaadi"].ToString();
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmKurumKarti_Load(sender, e);
            targetGrid.Visible = false;
        }

        private void padi_Leave(object sender, EventArgs e)
        {
            if (targetGrid != null)
            {
                if (ActiveControl.Name.ToString() == "ara")
                    targetGrid.Visible = true;
                else
                    targetGrid.Visible = false;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmilceekle ilceekle = new frmilceekle();
            ilceekle.ilid.Text = LUESehir.EditValue.ToString();
            ilceekle.sehiradi.Text = LUESehir.Text;
            ilceekle.ShowDialog();
            proilceler(LUESehir.EditValue.ToString());
        }

        void FisYazdir(bool Disigner)
        {
            System.Data.DataSet ds = new DataSet("Test");
            DataTable FisDetay = DB.GetData(@"SELECT top 1 * from Sirketler");
            FisDetay.TableName = "Sirketler";
            ds.Tables.Add(FisDetay);
            DataTable Fis = DB.GetData(@"SELECT * from Firmalar where pkFirma=" + pkFirma.Text);
            Fis.TableName = "Fis";
            ds.Tables.Add(Fis);

            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = exedizini + "\\Raporlar\\etiket_musterikarti.repx";
            if (!File.Exists(RaporDosyasi))
            {
                MessageBox.Show("Müsteri Karti, Rapor Dosyası Bulunamadı");
                return;
            }
            XtraReport rapor = new XtraReport();
            rapor.DataSource = ds;
            rapor.LoadLayout(RaporDosyasi);
            rapor.Name = "etiket_musterikarti";
            if (Disigner)
                rapor.ShowDesigner();
            else
                rapor.Print();//.ShowPreview();
        }
        private void btnyazdir_Click(object sender, EventArgs e)
        {
            FisYazdir(false);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FisYazdir(true);
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            frmMusteriGruplari grup = new frmMusteriGruplari();
            grup.ShowDialog();
            proGruplar();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (lUEGrup.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Müşteri Grubunu Seçiniz", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUEGrup.Focus();
                return;
            }
            frmMusteriAltGruplari MusteriAltGruplari = new frmMusteriAltGruplari();
            MusteriAltGruplari.lueStokGruplari.Tag = lUEGrup.EditValue.ToString();
            MusteriAltGruplari.ShowDialog();
            altgrupgetir();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (pkFirma.Text == "" || pkFirma.Text == "0")
            {
                //btnKaydet_Click(sender,e);
                MessageBox.Show("Lütfen Önce Müşteriyi Kaydediniz.");
                return;
            }
            DB.PkFirma = int.Parse(pkFirma.Text);

            frmMusteriBakiyeDuzeltme DevirBakisiSatisKaydi = new frmMusteriBakiyeDuzeltme(pkFirma.Text);
            DevirBakisiSatisKaydi.ShowDialog();
        }

        private void tabbedControlGroup1_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            if (tabbedControlGroup1.SelectedTabPageIndex == 2)
            {
                DataTable dt = DB.GetData("select * from FirmaOzelNot where fkFirma=" + pkFirma.Text);
                if (dt.Rows.Count > 0)
                    memoozelnot.Text = dt.Rows[0]["OzelNot"].ToString();
            }
        }

        private void padi_DoubleClick(object sender, EventArgs e)
        {
            if (txtMusteriAdi.Properties.ReadOnly == false) return;

            if (!formislemleri.SifreIsteYazilim()) return;
            txtMusteriAdi.Properties.ReadOnly = false;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            string s = "";

            if (lueMahalle.Text != "")
                s = lueMahalle.Text + " MAH. " + mAdres.Text+" ";
            else
                s = mAdres.Text + " ";

            if (lookUpilce.Text != "")
                s = s + lookUpilce.Text + " / ";

            if (LUESehir.Text != "")
                s = s + LUESehir.Text;

            mAdres.Text = s; 
        }

        private void btnKontaklar_Click(object sender, EventArgs e)
        {
            frmKontaklar Kontaklar = new frmKontaklar(pkFirma.Text);
            Kontaklar.ShowDialog();
            
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            formislemleri.Mesajform("Yapım Aşamasında", "S", 150);
        }

        private void pictureEdit1_DoubleClick(object sender, EventArgs e)
        {
            //FirmaResimKaydet();
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (ilkyukleme) return;

            try
            {
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string klasor = exeDiz + "\\MusteriKartiResim";

                string dosya = klasor + "\\" + pkFirma.Text + ".jpg";

                if (File.Exists(dosya))
                    File.Delete(dosya);

                if (ilkyukleme || pictureEdit1.Image == null) return;

                int max_gen = pictureEdit1.Image.Width, max_yuk = pictureEdit1.Image.Height;

                if (max_gen > 700) max_gen = 700;
                if (max_yuk > 500) max_yuk = 500;

                Bitmap loaded = new Bitmap(pictureEdit1.Image, max_gen, max_yuk);


                if (!Directory.Exists(klasor))
                    Directory.CreateDirectory(klasor);

                loaded.Save(dosya);
                loaded.Dispose();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
                //pictureEdit1.Tag = "0";
            }
        }

        void KimlikBilgileriGetir()
        {
            if (pkFirma.Text == "")
            {
                btnKaydet_Click(null,  null);
                return;
            }
            DataTable dtKimlik = DB.GetData("select * from KimlikBilgileri with(nolock) where fkFirma=" + pkFirma.Text);
            if (dtKimlik.Rows.Count == 0)
            {
                lueKanGrubu.EditValue = 0;
                ceBoyu.Value = 0;
                ceKilosu.Value = 0;
                layoutControlGroup2.Tag = 0;
            }
            else
            {
                layoutControlGroup2.Tag = dtKimlik.Rows[0]["pkKimlikBilgileri"].ToString();

                string kan_grubu_id = dtKimlik.Rows[0]["kan_grubu_id"].ToString();
                if (!string.IsNullOrEmpty(kan_grubu_id))
                    lueKanGrubu.EditValue = int.Parse(kan_grubu_id);

                if (dtKimlik.Rows[0]["boyu"].ToString() != "")
                    ceBoyu.Value = decimal.Parse(dtKimlik.Rows[0]["boyu"].ToString());

                if (dtKimlik.Rows[0]["kilosu"].ToString() != "")
                    ceKilosu.Value = decimal.Parse(dtKimlik.Rows[0]["kilosu"].ToString());

                txtTC.Text = dtKimlik.Rows[0]["tckimlikno"].ToString();

                string fkOgrenimDurumu = dtKimlik.Rows[0]["fkOgrenimDurumu"].ToString();
                if (!string.IsNullOrEmpty(fkOgrenimDurumu))
                    lueOgrenimDurumu.EditValue = int.Parse(fkOgrenimDurumu);

                cbCinsiyet.Text = dtKimlik.Rows[0]["cinsiyet"].ToString();
                cbMedeniHali.Text = dtKimlik.Rows[0]["medeni_hali"].ToString();
                txtBabaAdi.Text = dtKimlik.Rows[0]["baba_adi"].ToString();
                txtAnneAdi.Text = dtKimlik.Rows[0]["anne_adi"].ToString();

                txtCiltNo.Text = dtKimlik.Rows[0]["cilt_no"].ToString();
                txtAileSiraNo.Text = dtKimlik.Rows[0]["aile_sira_no"].ToString();
                txtSiraNo.Text = dtKimlik.Rows[0]["sira_no"].ToString();
                txtDini.Text = dtKimlik.Rows[0]["dini"].ToString();
                txtisAdresi.Text = dtKimlik.Rows[0]["is_adresi"].ToString();
                txtisTel.Text = dtKimlik.Rows[0]["is_tel"].ToString();

                txtAcilDurumKisi.Text = dtKimlik.Rows[0]["acil_durum_kisi"].ToString();
                txtAcilDurumTel.Text = dtKimlik.Rows[0]["acil_durum_tel"].ToString();
                   
                string dogum_tarihi = dtKimlik.Rows[0]["dogum_tarihi"].ToString();
                if (!string.IsNullOrEmpty(dogum_tarihi))
                   deDogumTarihi.DateTime = DateTime.Parse(dogum_tarihi);

                txtAcilDurumTel.Text = dtKimlik.Rows[0]["acil_durum_tel"].ToString();

                txtDogumYeri.Text = dtKimlik.Rows[0]["dogum_yeri"].ToString();
                txtMahalleAdi.Text = dtKimlik.Rows[0]["mahalle_adi"].ToString();
                int il=0;
                int.TryParse(dtKimlik.Rows[0]["dogum_il_id"].ToString(), out il);
                lueil.EditValue = il;

                int ilce=0;
                int.TryParse(dtKimlik.Rows[0]["dogum_ilce_id"].ToString(),out ilce);
                lueilce.EditValue = ilce;
            }

        }

        private void layoutControlGroup2_Shown(object sender, EventArgs e)
        {
            //if (!ilkyukleme)
            //{
            //    //lueilce.EditValue = Degerler.fkilceAltGrupDefault;
            //    KimlikBilgileriGetir();
            //}
        }

        private void lueil_EditValueChanged(object sender, EventArgs e)
        {
            dogumilceler(lueil.EditValue.ToString());
        }

        private void layoutControlGroup2_Hidden(object sender, EventArgs e)
        {
            //KimlikBilgileriKaydet();
        }

        private void lueMahalle_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void MusteriKodu_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                DataTable dt = DB.GetData("select * from Firmalar with(nolock) where OzelKod='" + MusteriKodu .Text+ "'");
                if (dt.Rows.Count > 0)
                {
                    pkFirma.Text = dt.Rows[0][0].ToString();
                    DB.PkFirma = int.Parse(pkFirma.Text);
                    TumBilgileriGetir();
                    txtMusteriAdi.Focus();
                }
                else
                {
                    formislemleri.Mesajform("Müşteri Bulunamadı", "K", 100);
                    pkFirma.Text = "0";
                    DB.PkFirma = 0;
                    YeniMusteri();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmMahalleEkle m = new frmMahalleEkle();
            m.ShowDialog();
        }

        private void btnKontakCep_Click(object sender, EventArgs e)
        {
            frmKontaklar kontak = new frmKontaklar(pkFirma.Text);
            //Cep.Text;
            kontak.ShowDialog();
        }

        private void btnTel_Click(object sender, EventArgs e)
        {
            frmKontaklar kontak = new frmKontaklar(pkFirma.Text);
            //telno.Text;
            kontak.ShowDialog();
            
        }

        private void Cep_Leave(object sender, EventArgs e)
        {
            if(pkFirma.Text=="0" && Cep.Text != "")
            {
                DataTable dt =
                DB.GetData("select pkFirma,OzelKod,Firmaadi,Cep from Firmalar with(nolock) where replace(Cep,' ','')='" + Cep.Text.Replace(" ","")+ "'");
                if(dt.Rows.Count>0)
                {
                    //DialogResult secim;
                    //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Daha Önce Kaydedilmiş" +
                    //    dt.Rows[0]["OzelKod"].ToString)+" - "+
                    //    dt.Rows[0]["Firmaadi"].ToString) +
                    //    " Müşteri Bulunmaktadır.\r\n Kayda Devam etmek istermisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                    //if (secim == DialogResult.Yes)
                    //{
                        frmMusteriKarti mk = new frmMusteriKarti(dt.Rows[0]["pkFirma"].ToString(), dt.Rows[0]["OzelKod"].ToString());
                        mk.ShowDialog();
                    //}
                }
            }
        }

        private void VergiNo_Leave(object sender, EventArgs e)
        {
            if (pkFirma.Text == "0" && VergiNo.Text !="")
            {
                DataTable dt =
                DB.GetData("select pkFirma,OzelKod,Firmaadi,Cep from Firmalar with(nolock) where VergiNo='" + VergiNo.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    //DialogResult secim;
                    //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Daha Önce Kaydedilmiş" +
                    //    dt.Rows[0]["OzelKod"].ToString)+" - "+
                    //    dt.Rows[0]["Firmaadi"].ToString) +
                    //    " Müşteri Bulunmaktadır.\r\n Kayda Devam etmek istermisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                    //if (secim == DialogResult.Yes)
                    //{
                    frmMusteriKarti mk = new frmMusteriKarti(dt.Rows[0]["pkFirma"].ToString(), dt.Rows[0]["OzelKod"].ToString());
                    mk.ShowDialog();
                    //}
                }
            }
        }

        private void sbtnAdresler_Click(object sender, EventArgs e)
        {
            frmMusteriAdresleri adresleri = new frmMusteriAdresleri(int.Parse(pkFirma.Text));
            adresleri.ShowDialog();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using GPTS.islemler;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmPersonel : DevExpress.XtraEditors.XtraForm
    {
        public static string _personel_id = "0";
        public frmPersonel(string personel_id)
        {
            InitializeComponent();
            _personel_id = personel_id;
        }

        private void frmPersonel_Load(object sender, EventArgs e)
        {
            pkmusteri.Text = _personel_id;

            //DB.pkPersoneller.ToString();
            if (pkmusteri.Text == "0")
                BKaydet.Text = "Kaydet";
            else
                BKaydet.Text = "Güncelle";

                DataTable dt = DB.GetData("SELECT * FROM Projeler with(nolock)");
                luekurum.Properties.DataSource = dt;
                luekurum.Properties.ValueMember = "pkProjeler";
                luekurum.Properties.DisplayMember = "ProjeAdi";
                //luekurum.EditValue = 41;
            
                probankalar();
                proiller();
                proDogumiller();
                prokangruplari();

                proSirketler();
                Subeler();

                GetTahsilDurumu();
                GetBolumler();
            //PERSONEL
                DataTable dt2 = DB.GetData("SELECT * FROM personeller with(nolock) where pkpersoneller=" + pkmusteri.Text);
                if (dt2.Rows.Count > 0)
                {
                    pkmusteri.Text = dt2.Rows[0]["pkpersoneller"].ToString();
                    adi.Text = dt2.Rows[0]["adi"].ToString();
                    Soyadi.Text = dt2.Rows[0]["Soyadi"].ToString();
                    ceptel.Text = dt2.Rows[0]["ceptel"].ToString();
                    evtel.Text = dt2.Rows[0]["tel"].ToString();
                    teTCKimlik.Text = dt2.Rows[0]["tckimlikno"].ToString();
                    tEAnne.Text = dt2.Rows[0]["anneadi"].ToString();
                    tEBaba.Text = dt2.Rows[0]["babaadi"].ToString();
                    sENobetsayi.Value = int.Parse(dt2.Rows[0]["nobetsayi"].ToString());
                    isegiristarih.EditValue = Convert.ToDateTime(dt2.Rows[0]["isegiristarih"]).ToString("dd.MM.yyyy");
                    teDogumTarihi.EditValue = Convert.ToDateTime(dt2.Rows[0]["dogumtarihi"]).ToString("dd.MM.yyyy");
                    int il = 0;
                    int.TryParse(dt2.Rows[0]["fkil"].ToString(), out il);
                    LUESehir.EditValue =il;
                    int dogumil = 0;
                    int.TryParse(dt2.Rows[0]["fkDogumil"].ToString(), out dogumil);
                    lUDogumil.EditValue = dogumil;
                    if (dt2.Rows[0]["fkgrup"].ToString() == "")
                        luekurum.EditValue = null;
                    else
                        luekurum.EditValue = int.Parse(dt2.Rows[0]["fkgrup"].ToString());
                    memoozelnot.Text = dt2.Rows[0]["ozelnot"].ToString();
                    bankahesapno.Text= dt2.Rows[0]["bankahesapno"].ToString();
                    teBankaSubeKodu.Text = dt2.Rows[0]["BankaSubeKodu"].ToString();
                    ibanno.Text= dt2.Rows[0]["ibanno"].ToString();
                    sicilno.Text = dt2.Rows[0]["sicilno"].ToString();
                    dateEdit5.EditValue = dt2.Rows[0]["SSKbaslangicTarihi"].ToString();
                    dateEdit5.EditValue = Convert.ToDateTime(dateEdit5.EditValue.ToString()).ToString("dd.MM.yyyy");
                    lUBankalar.EditValue=int.Parse(dt2.Rows[0]["fkbankalar"].ToString());
                    lUKanGruplari.EditValue = int.Parse(dt2.Rows[0]["fkkangrubu"].ToString());
                    ceMaas.EditValue = dt2.Rows[0]["maasi"].ToString();
                    ceBankaMaasi.EditValue = dt2.Rows[0]["bankamaasi"].ToString();
                    cEAgi.EditValue = dt2.Rows[0]["agiucreti"].ToString();
                    cEYemek.EditValue = dt2.Rows[0]["yemekucreti"].ToString();
                    cEYol.EditValue = dt2.Rows[0]["yolucreti"].ToString();
                    if (dt2.Rows[0]["cinsiyet"].ToString() == "True")
                        cbCinsiyet.SelectedIndex = 1;
                    else
                        cbCinsiyet.SelectedIndex = 0;
                    if (dt2.Rows[0]["askerlik"].ToString()!="")
                        cbAskerlik.SelectedIndex = int.Parse(dt2.Rows[0]["askerlik"].ToString());
                    if (dt2.Rows[0]["fkmedenihali"].ToString()!="")
                        cbMedeniHali.SelectedIndex = int.Parse(dt2.Rows[0]["fkmedenihali"].ToString());
                    if (dt2.Rows[0]["Silahli"].ToString() == "True")
                        checkEdit1.Checked = true;
                    else
                        checkEdit1.Checked = false;
                    tEVardiyaKodu.Text = dt2.Rows[0]["VardiyaKodu"].ToString();
                    int sirket = 1;
                    int.TryParse(dt2.Rows[0]["fkSirket"].ToString(),out sirket);
                    lUSirket.EditValue = sirket;
                    int od = 0;
                    int.TryParse(dt2.Rows[0]["fkOgrenimDurumu"].ToString(),out od);
                    lUTahsilDurumu.EditValue = od;
                    mAdres.Text = dt2.Rows[0]["EvAdresi"].ToString();
                    textEdit4.Text = dt2.Rows[0]["eposta"].ToString();
                    textEdit5.Text = dt2.Rows[0]["TelefonNo"].ToString();
                    int ilce = 0;
                    int.TryParse(dt2.Rows[0]["fkilce"].ToString(), out ilce);
                    lookUpilce.EditValue = ilce;
                    int dogumilce = 0;
                    int.TryParse(dt2.Rows[0]["fkDogumilce"].ToString(), out dogumilce);
                    lUDogumilce.EditValue = dogumilce;
                    int si = 0;
                    int.TryParse(dt2.Rows[0]["Sigara"].ToString(), out si);
                    cBESigara.SelectedIndex = si;
                    int boy = 0;
                    int.TryParse(dt2.Rows[0]["Boy"].ToString(), out boy);
                    sEBoy.Value = boy;
                    int kilo = 0;
                    int.TryParse(dt2.Rows[0]["Kilo"].ToString(), out kilo);
                    sEKilo.Value = kilo;
                    tEDogumYeri.Text = dt2.Rows[0]["DogumYeri"].ToString();
                    tECildNo.Text = dt2.Rows[0]["CildNo"].ToString();
                    tEAileSiraNo.Text = dt2.Rows[0]["AileSiraNo"].ToString();
                    SiraNo.Text = dt2.Rows[0]["SiraNo"].ToString();
                    int gorev = 0;
                    int.TryParse(dt2.Rows[0]["fkBolumler"].ToString(),out gorev);
                    lueGorev.EditValue = gorev;
                    int ehliyet=0;
                    int.TryParse(dt2.Rows[0]["Ehliyet"].ToString(),out ehliyet);
                    cbEhliyet.SelectedIndex = ehliyet;
                    int SilahDurum=0;
                    int.TryParse(dt2.Rows[0]["SilahDurum"].ToString(),out SilahDurum);
                    cBESilah.SelectedIndex = SilahDurum;
                    tEMahalle.Text = dt2.Rows[0]["DogumMahalle"].ToString();

                if (dt2.Rows[0]["Plasiyer"].ToString() == "True")
                        cbPlasiyer.Checked = true;

                if (dt2.Rows[0]["IlkMaasTarihi"].ToString() != "")
                {
                    deilkMaasTarihi.EditValue = dt2.Rows[0]["IlkMaasTarihi"].ToString();
                    deilkMaasTarihi.DateTime = Convert.ToDateTime(deilkMaasTarihi.EditValue.ToString());
                }
                if (dt2.Rows[0]["fkSube"].ToString() == "")
                    lueSubeler.EditValue = Degerler.fkSube;
                else
                    lueSubeler.EditValue = int.Parse(dt2.Rows[0]["fkSube"].ToString());

                if (dt2.Rows[0]["maas_gunu"].ToString() == "")
                    seMaasgunu.Value = 1;
                else
                    seMaasgunu.Value = int.Parse(dt2.Rows[0]["maas_gunu"].ToString());
                

                //personel resim
                DataTable dt3 = new DataTable();
                    dt3 = DB.GetData("select resim from PersonelResim where fkpersonel=" + pkmusteri.Text);
                    if (dt3.Rows.Count>0)
                    pictureEdit1.Image = ResimOku((byte[]) dt3.Rows[0][0]);
                }
        }

        void Subeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData("select 0 pkSube,'Tümü' as sube_adi,'Tümü' as aciklama,getdate() as tarih,0 as aktif,'' as yetkili union all select * from Subeler with(nolock)");
            lueSubeler.EditValue = Degerler.fkSube;
        }

        Image ResimOku(byte[] Veri)
      {
      Image bImage = null;

      using (MemoryStream mm = new MemoryStream(Veri, 0, Veri.Length))
      {
      mm.Write(Veri, 0, Veri.Length);
      bImage = Image.FromStream(mm,true);
      }
      return bImage;
      }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            
            if (adi.Text=="")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Personel Adı Boş Olamaz!", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                adi.Focus();
                return;
            }
            if (Soyadi.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Personel Soyadı Boş Olamaz!", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Soyadi.Focus();
                return;
            }
            if (isegiristarih.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("İşe Giriş Tarihi Boş Olamaz!", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isegiristarih.Focus();
                return;
            }
            if (teDogumTarihi.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Doğum Tarihi Boş Olamaz!", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                xtraTabControl1.SelectedTabPageIndex = 0;
                teDogumTarihi.Focus();
                return;
            }
            //if (lUSirket.EditValue == null || lUSirket.EditValue.ToString() == "")
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Personelin Çalıştığı Şirketi Seçiniz!", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    lUSirket.Focus();
            //    return;
            //}
            if (cbCinsiyet.SelectedIndex == -1)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Personelin Cinsiyeti Seçiniz!", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCinsiyet.Focus();
                return;
            }
            
            string sql = "";
            if (pkmusteri.Text == "0")
            {
                sql = @"insert into Personeller (adi,soyadi,tel,ceptel,fkgrup,nobetsayi,isegiristarih,fkil,ozelnot,
                sicilno,bankahesapno,ibanno,fkbankalar,fkkangrubu,dogumtarihi,maasi,agiucreti,yolucreti,yemekucreti,
                cinsiyet,askerlik,fkmedenihali,Silahli,BankaSubeKodu,bankamaasi,tckimlikno,VardiyaKodu,fkOgrenimDurumu,fkSirket,
                SSKbaslangicTarihi,babaadi,anneadi,EvAdresi,eposta,fkilce,Sigara,Boy,Kilo,DogumYeri,DogumMahalle,CildNo,AileSiraNo,
                SiraNo,Ehliyet,fkDogumil,fkDogumilce,TelefonNo,SilahDurum,fkBolumler,Plasiyer,IlkMaasTarihi,fkSube,maas_gunu) 
                      values('@adi','@soyadi','@tel','@ceptel',@fkgrup,@nobetsayi,'@isegiristarih',@fkil,'@ozelnot',
                '@sicilno','@bankahesapno','@ibanno',@fkbankalar,@fkkangrubu,'@dogumtarihi',@maasi,@agiucreti,@yolucreti,@yemekucreti,
                @cinsiyet,@askerlik,@fkmedenihali,@Silahli,'@BankaSubeKodu', @bankamaasi,'@tckimlikno','@VardiyaKodu',@fkOgrenimDurumu,@fkSirket,
                '@SSKbaslangicTarihi','@babaadi','@anneadi','@EvAdresi','@eposta',@ilce,@Sigara,@Boy,@Kilo,'@DogumYeri','@DogumMahalle','@CildNo','@AileSiraNo','@SiraNo',
                @Ehliyet,@fkDogumil,@fkDogumi_lce,'@TelefonNo',@SilahDurum,@fkBolumler,@Plasiyer,'@IlkMaasTarihi',@fkSube,@maas_gunu)";// select IDENT_CURRENT('Personeller')";
            }
            else
            {
                sql = @"update Personeller set adi='@adi',soyadi='@soyadi',tel='@tel',ceptel='@ceptel',fkgrup=@fkgrup,
                nobetsayi=@nobetsayi,isegiristarih='@isegiristarih',fkil=@fkil,ozelnot='@ozelnot',sicilno='@sicilno',
                bankahesapno='@bankahesapno',ibanno='@ibanno',fkbankalar=@fkbankalar,fkkangrubu=@fkkangrubu,
                dogumtarihi='@dogumtarihi',maasi=@maasi,agiucreti=@agiucreti,yolucreti=@yolucreti,yemekucreti=@yemekucreti,
                cinsiyet=@cinsiyet,askerlik=@askerlik,fkmedenihali=@fkmedenihali,Silahli=@Silahli,BankaSubeKodu='@BankaSubeKodu',
                bankamaasi=@bankamaasi,tckimlikno='@tckimlikno',VardiyaKodu='@VardiyaKodu',fkOgrenimDurumu=@fkOgrenimDurumu,
                fkSirket=@fkSirket,SSKbaslangicTarihi='@SSKbaslangicTarihi',babaadi='@babaadi',anneadi='@anneadi',EvAdresi='@EvAdresi',
                eposta='@eposta',fkilce=@ilce,Sigara=@Sigara,Boy=@Boy,Kilo=@Kilo,DogumMahalle='@DogumMahalle',
                CildNo='@CildNo',AileSiraNo='@AileSiraNo',SiraNo='@SiraNo',Ehliyet=@Ehliyet,DogumYeri='@DogumYeri',
                fkDogumil=@fkDogumil,fkDogumilce=@fkDogumi_lce,TelefonNo='@TelefonNo',SilahDurum=@SilahDurum,fkBolumler=@fkBolumler,
                Plasiyer=@Plasiyer,IlkMaasTarihi='@IlkMaasTarihi',fkSube=@fkSube,maas_gunu=@maas_gunu where pkpersoneller=@pkmusteri";
                
                sql = sql.Replace("@pkmusteri", pkmusteri.Text);
            }
            sql = sql.Replace("@adi", adi.Text);
            sql = sql.Replace("@soyadi", Soyadi.Text);
            sql = sql.Replace("@tel", evtel.Text);
            sql = sql.Replace("@ceptel", ceptel.Text);
            sql = sql.Replace("@babaadi", tEBaba.Text);
            sql = sql.Replace("@anneadi", tEAnne.Text);
            if (luekurum.EditValue==null)
                sql = sql.Replace("@fkgrup", "0");
            else
                sql = sql.Replace("@fkgrup", luekurum.EditValue.ToString());
            sql = sql.Replace("@nobetsayi", sENobetsayi.Value.ToString());
            sql = sql.Replace("@isegiristarih", isegiristarih.DateTime.ToString("yyyy-MM-dd"));
            sql = sql.Replace("@dogumtarihi",teDogumTarihi.Time.ToString("yyyy-MM-dd"));

            if (dateEdit5.EditValue==null)
                sql = sql.Replace("@SSKbaslangicTarihi", "");
            else
                sql = sql.Replace("@SSKbaslangicTarihi", dateEdit5.EditValue.ToString());
            if (LUESehir.EditValue==null)
                sql = sql.Replace("@fkil", "0");
            else
                sql = sql.Replace("@fkil", LUESehir.EditValue.ToString());
            sql = sql.Replace("@ozelnot", memoozelnot.Text);
            sql = sql.Replace("@sicilno", sicilno.Text);
            sql = sql.Replace("@bankahesapno", bankahesapno.Text);
            sql = sql.Replace("@BankaSubeKodu", teBankaSubeKodu.Text);
            sql = sql.Replace("@ibanno", ibanno.Text);
            if (lUBankalar.EditValue==null)
                sql = sql.Replace("@fkbankalar", "0");
            else
                sql = sql.Replace("@fkbankalar", lUBankalar.EditValue.ToString());

            if (lUKanGruplari.EditValue == null)
                sql = sql.Replace("@fkkangrubu", "0");
            else
                sql = sql.Replace("@fkkangrubu", lUKanGruplari.EditValue.ToString());
            sql = sql.Replace("@maasi", ceMaas.Value.ToString().Replace(",","."));
            sql = sql.Replace("@bankamaasi", ceBankaMaasi.Value.ToString().Replace(",", "."));
            sql = sql.Replace("@agiucreti", cEAgi.Value.ToString().Replace(",", "."));
            sql = sql.Replace("@yolucreti", cEYol.Value.ToString().Replace(",", "."));
            sql = sql.Replace("@yemekucreti", cEYemek.Value.ToString().Replace(",", "."));
            if (cbCinsiyet.SelectedIndex == 0)
                sql = sql.Replace("@cinsiyet", "0");
            else
                sql = sql.Replace("@cinsiyet", "1");

            sql = sql.Replace("@askerlik", cbAskerlik.SelectedIndex.ToString());
            sql = sql.Replace("@fkmedenihali", cbMedeniHali.SelectedIndex.ToString());
            if (checkEdit1.Checked)
                sql = sql.Replace("@Silahli", "1");
            else
                sql = sql.Replace("@Silahli", "0");

            sql = sql.Replace("@tckimlikno", teTCKimlik.Text);
            sql = sql.Replace("@VardiyaKodu", "0");
            if (lUTahsilDurumu.EditValue==null)
                sql = sql.Replace("@fkOgrenimDurumu","0");
            else
                sql = sql.Replace("@fkOgrenimDurumu", lUTahsilDurumu.EditValue.ToString());
            if (lUSirket.EditValue == null)
                sql = sql.Replace("@fkSirket", "0");
            else
                sql = sql.Replace("@fkSirket", lUSirket.EditValue.ToString());

            sql = sql.Replace("@EvAdres", mAdres.Text);
            sql = sql.Replace("@eposta",textEdit4.Text);
            if (lookUpilce.EditValue == null)
                sql = sql.Replace("@ilce","0");
            else
                sql = sql.Replace("@ilce", lookUpilce.EditValue.ToString());
            sql = sql.Replace("@Sigara", cBESigara.SelectedIndex.ToString());
            sql = sql.Replace("@Boy", sEBoy.Value.ToString());
            sql = sql.Replace("@Kilo", sEKilo.Value.ToString());
            sql = sql.Replace("@DogumYeri", tEDogumYeri.Text);
            sql = sql.Replace("@DogumMahalle", tEMahalle.Text);
            sql = sql.Replace("@CildNo", tECildNo.Text);
            sql = sql.Replace("@AileSiraNo", tEAileSiraNo.Text);
            sql = sql.Replace("@SiraNo", SiraNo.Text);
            sql = sql.Replace("@TelefonNo", textEdit5.Text);
            
            sql = sql.Replace("@Ehliyet", cbEhliyet.SelectedIndex.ToString());
            
            if (lUDogumil.EditValue == null)
                sql = sql.Replace("@fkDogumil", "0");
            else
                sql = sql.Replace("@fkDogumil", lUDogumil.EditValue.ToString());
            if (lUDogumilce.EditValue == null)
                sql = sql.Replace("@fkDogumi_lce", "0");
            else
                sql = sql.Replace("@fkDogumi_lce", lUDogumilce.EditValue.ToString());
            sql = sql.Replace("@SilahDurum", cBESilah.SelectedIndex.ToString());
            if (lueGorev.EditValue==null)
                sql = sql.Replace("@fkBolumler", "0");
            else
                sql = sql.Replace("@fkBolumler", lueGorev.EditValue.ToString());

            //Plasiyer
            if (cbPlasiyer.Checked)
                 sql = sql.Replace("@Plasiyer", "1");
            else
                 sql = sql.Replace("@Plasiyer", "0");

            if (deilkMaasTarihi.EditValue==null)
                sql = sql.Replace("@IlkMaasTarihi", DateTime.Now.ToString("yyyy-MM-dd"));
            else
                sql = sql.Replace("@IlkMaasTarihi", deilkMaasTarihi.DateTime.ToString("yyyy-MM-dd"));

            sql = sql.Replace("@fkSube", lueSubeler.EditValue.ToString());
            sql = sql.Replace("@maas_gunu", seMaasgunu.Value.ToString());
            //string sonuc = DB.ExecuteScalarSQL(sql, list);
            int sonuc= DB.ExecuteSQL(sql);

           if (sonuc == 1)
           {

               if (BKaydet.Text == "Kaydet")
               {
                   pkmusteri.Text = DB.GetData("select MAX(pkpersoneller) as c from Personeller").Rows[0][0].ToString();

                   formislemleri.Mesajform("Yeni Personel Kaydedildi", "S", 100);
                   BKaydet.Text = "Güncelle";
               }
               else
               {
                    formislemleri.Mesajform("Personel Bilgileri Güncellendi", "S", 100);
                   //DevExpress.XtraEditors.XtraMessageBox.Show("Personel Bilgileri Güncellendi.", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               if (SiraNo.Text == "")
                   DB.ExecuteSQL("update Personeller set SiraNo=" + pkmusteri.Text + " where pkpersoneller=" + pkmusteri.Text);
           }
           else
               DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu.", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            simpleButton1.Enabled = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            pkmusteri.Text = "0";
            BKaydet.Text = "Kaydet";
            adi.Text = "";
            Soyadi.Text = "";
            luekurum.EditValue=null;
            isegiristarih.EditValue = DateTime.Today.ToShortDateString();
            lUSirket.EditValue = null;
            lUTahsilDurumu.EditValue = null;
            sEBoy.Value = 0;
            sEKilo.Value = 0;
            cBESigara.SelectedIndex = 0;
            tEBaba.Text = "";
            tEAnne.Text = "";
            lUKanGruplari.EditValue = 0;
            cbEhliyet.SelectedIndex = 0;
            cBESigara.SelectedIndex = 0;
            lUTahsilDurumu.EditValue = 0;
            teTCKimlik.Text = "";
            tEAileSiraNo.Text = "";
            textEdit5.Text = "";
            teSiraNo.Text = "";
            tECildNo.Text = "";
            tEMahalle.Text = "";
            tEDogumYeri.Text = "";
            lUDogumil.EditValue = 0;
            lUDogumilce.EditValue = 0;
            ceMaas.Value = 0;
            ceBankaMaasi.Value = 0;
            cEAgi.Value = 0;
            cEYemek.Value = 0;
            cEYol.Value = 0;
            ceptel.Text = "";
            //DevExpress.XtraEditors.XtraMessageBox.Show("Yapım Aşamasındadır.", "PBYS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            simpleButton1.Enabled = false;
            PersonelAileBilgileri();
            pictureEdit1.Image = null;
        }
        private void xtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (e.Page.Name == "xtraTabPage5")
            {
                try
                {
                    DataTable dt = DB.GetData(@"select * from KasaHareket where Modul=5 and Tipi=3 and fkpersoneller=" + pkmusteri.Text + " order by Tarih desc");
                    //DataTable dt = DB.GetData(@"select * from PersonelMaas where fkpersonel=" + pkmusteri.Text);
                    gridControl1.DataSource = dt;
                    //sehirler
                }
                catch (Exception ex)
                {

                }
            }
            if (e.Page.Name == "xtraTabPage2")
            {
                DataTable dt = DB.GetData("SELECT * FROM YAKINLIKKODLARI WHERE KOD<>'0000'");
                lUYakinlik.Properties.DataSource = dt;
                lUYakinlik.Properties.ValueMember = "KOD";
                lUYakinlik.Properties.DisplayMember = "ACIKLAMA";

                PersonelAileBilgileri();
            }
            if (e.Page.Name == "xtraTabPage9")
            {
                gridControl2.DataSource =
   DB.GetData("select * from PersonelCalismaGunleri where fkPersoneller=" + DB.pkPersoneller);
                if (gridView3.DataRowCount == 0)
                {
                    DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@Pzrts", false));
                    list.Add(new SqlParameter("@Sl", false));
                    list.Add(new SqlParameter("@Crsnb", false));
                    list.Add(new SqlParameter("@Prsnb", false));
                    list.Add(new SqlParameter("@Cm", false));
                    list.Add(new SqlParameter("@Cmrts", false));
                    list.Add(new SqlParameter("@Pzr", false));
                    list.Add(new SqlParameter("@fkPersoneller", DB.pkPersoneller));
                    DB.ExecuteSQL("INSERT INTO PersonelCalismaGunleri (fkPersoneller,Pzrts,Sl,Crsnb,Prsnb,Cm,Cmrts,Pzr)" +
                    " VALUES(@fkPersoneller,@Pzrts,@Sl,@Crsnb,@Prsnb,@Cm,@Cmrts,@Pzr)", list);
                }
                gridControl2.DataSource =
 DB.GetData("select * from PersonelCalismaGunleri where fkPersoneller=" + DB.pkPersoneller);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string sql = "";
            DB.ExecuteSQL("update personeller set maasi=" + ceMaas.EditValue.ToString().Replace(",", ".") + " where pkpersoneller=" + pkmusteri.Text);
            ArrayList plist = new ArrayList();
            if (ceMaas.EditValue == "0")
            {
                sql = @"insert into PersonelMaas (fkpersonel,maas,ilktarih,sontarih) 
                      values(@fkpersonel,@maas,@ilktarih,@sontarih)";
            }
            else
            {
                sql = @"update PersonelMaas set fkpersonel=@fkpersonel,maas=@maas,ilktarih=@ilktarih,sontarih=@sontarih
                where pkpersoneller=@pkmusteri";
               // plist.Add(new SqlParameter("@pkMaas", tEMaasi.Text));
            }
            plist.Add(new SqlParameter("@fkpersonel", pkmusteri.Text));
            plist.Add(new SqlParameter("@maas", ceMaas.Value.ToString().Replace(",",".")));
            plist.Add(new SqlParameter("@ilktarih", deilkMaasTarihi.EditValue));
           // plist.Add(new SqlParameter("@sontarih", dateEdit4.EditValue));
            //plist.Add(new SqlParameter("@fkil", LUESehir.EditValue));

            //plist.Add(new SqlParameter("@fkgrup", luekurum.EditValue.ToString()));
            DB.ExecuteSQL(sql, plist);
            //GETİR
            try
            {
                DataTable dt = DB.GetData(@"select * from PersonelMaas where fkpersonel=" + pkmusteri.Text);
                gridControl1.DataSource = dt;
                //sehirler
            }
            catch (Exception ex)
            {

            }
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Lütfen Resim Seçiniz!";
            openFileDialog1.Filter = " (*.jpg)|*.jpg|(*.png)|*.png";
            openFileDialog1.FilterIndex = 1; // varsayılan olarak jpg uzantıları göster
            //if (openFileDialog1.FileName == null) return;
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            DataTable c = DB.GetData("select count(*) from PersonelResim where fkpersonel="+pkmusteri.Text);
            int cc = int.Parse(c.Rows[0][0].ToString());
            
            ArrayList par = new ArrayList();
            byte[] img = File.ReadAllBytes(openFileDialog1.FileName);
            //plist.Add(new SqlParameter("@pkmusteri", pkmusteri.Text));
            par.Add(new SqlParameter("@Image", img));
            par.Add(new SqlParameter("@fkpersonel", pkmusteri.Text));
            if (cc == 0)
            DB.ExecuteSQL("INSERT INTO PersonelResim(fkpersonel,resim) VALUES(@fkpersonel,@Image)", par);
            else
                DB.ExecuteSQL("UPDATE PersonelResim SET resim=@Image WHERE fkpersonel=@fkpersonel", par);
            //resim göster
            DataTable dt3 = new DataTable();
            dt3 = DB.GetData("select resim from PersonelResim where fkpersonel=" + pkmusteri.Text);
            if (dt3.Rows.Count > 0)
                pictureEdit1.Image = ResimOku((byte[])dt3.Rows[0][0]);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //frmbankalar bankalar = new frmbankalar();
            //bankalar.ShowDialog();
            probankalar();
        }
        void proiller()
        {
            LUESehir.Properties.DataSource = DBislemleri.ilgetir();
            LUESehir.Properties.ValueMember = "KODU";
            LUESehir.Properties.DisplayMember = "ADI";
        }
        void proDogumiller()
        {
            lUDogumil.Properties.DataSource = DBislemleri.ilgetir();
            lUDogumil.Properties.ValueMember = "KODU";
            lUDogumil.Properties.DisplayMember = "ADI";
        }
        void proilceler(string ilid)
        {
            lookUpilce.Properties.DataSource = DBislemleri.ilcelerigetir(ilid);
            lookUpilce.Properties.ValueMember = "KODU";
            lookUpilce.Properties.DisplayMember = "ADI";
        }
        void proDogumilceler(string ilid)
        {
            DataTable dt = DB.GetData(@"SELECT [ADI] ,[KODU] ,[GRUP],[ALTGRUP] FROM IL_ILCE_MAH
                                         WHERE ALTGRUP=" + ilid + "  ORDER BY KODU");
            lUDogumilce.Properties.DataSource = dt;
            lUDogumilce.Properties.ValueMember = "KODU";
            lUDogumilce.Properties.DisplayMember = "ADI";
        }
        void probankalar()
        {
            DataTable dtb = DB.GetData("SELECT * FROM Bankalar");
            lUBankalar.Properties.DataSource = dtb;
            lUBankalar.Properties.ValueMember = "PkBankalar";
            lUBankalar.Properties.DisplayMember = "BankaAdi";
        }
        void prokangruplari()
        {
            lUKanGruplari.Properties.DataSource = DB.GetData("select * from kangrubu");
            lUKanGruplari.Properties.ValueMember = "id";
            lUKanGruplari.Properties.DisplayMember = "grup";
            lUKanGruplari.EditValue = 0;
        }

        void proSirketler()
        {
            lUSirket.Properties.DataSource = DB.GetData("select * from Sirketler with(nolock)");
            lUSirket.EditValue = 1;
        }

        void GetTahsilDurumu()
        {
            lUTahsilDurumu.Properties.DataSource = DB.GetData("select * from OgrenimDurumu");
            lUTahsilDurumu.Properties.ValueMember = "PkOgrenimDutrumu";
            lUTahsilDurumu.Properties.DisplayMember = "OgrenimDurumu";
        }
        void GetBolumler()
        {
            lueGorev.Properties.DataSource = DB.GetData("select * from Bolumler");
        }
        private void LUESehir_EditValueChanged(object sender, EventArgs e)
        {
            proilceler(LUESehir.EditValue.ToString());
        }
        void PersonelAileBilgileri()
        {
            gCAile.DataSource= DB.GetData(@"SELECT    Personel_Aile.pkPersonelAile, Personel_Aile.fkPersoneller, Personel_Aile.AdSoyad, Personel_Aile.TcKimlikNo, Personel_Aile.fkYakinlik, 
            Personel_Aile.Aciklama, Personel_Aile.DogumTarihi, Personel_Aile.DogumYeri, YAKINLIKKODLARI.ACIKLAMA AS Yakinlik
            FROM Personel_Aile LEFT OUTER JOIN  YAKINLIKKODLARI ON Personel_Aile.fkYakinlik = YAKINLIKKODLARI.KOD where fkPersoneller=" + pkmusteri.Text);
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (pkmusteri.Text == "0")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Önce Personeli Kaydediniz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (lUYakinlik.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Yakınlık Bilgisi Seçmediniz...", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUYakinlik.Focus();
                return;
            }
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkPersoneller", pkmusteri.Text));
            list.Add(new SqlParameter("@AdSoyad", tEAileAdsoyad.Text));
            list.Add(new SqlParameter("@TcKimlikNo", tEAileTC.Text));
            list.Add(new SqlParameter("@fkYakinlik", lUYakinlik.EditValue.ToString()));
            list.Add(new SqlParameter("@DogumTarihi", dEAileDTarihi.EditValue));
            DB.ExecuteSQL(@"INSERT INTO Personel_Aile (fkPersoneller,AdSoyad,TcKimlikNo,fkYakinlik,DogumTarihi) values(
                                        @fkPersoneller,@AdSoyad,@TcKimlikNo,@fkYakinlik,@DogumTarihi)", list);
            PersonelAileBilgileri();
        }

        private void gridView2_KeyUp(object sender, KeyEventArgs e)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (e.KeyCode==Keys.Delete)
            {
                DB.ExecuteSQL("delete from Personel_Aile where pkPersonelAile="+ dr["pkPersonelAile"].ToString());
                PersonelAileBilgileri();
            }
        }
        private void frmPersonel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            } 

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("delete from PersonelResim where fkpersonel=" + pkmusteri.Text);
            pictureEdit1.Image = null;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pictureEdit1_Click(sender, e);
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Personel Silinsin mi?", "Personel Takip Sistemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            if (DB.GetData("select * from KasaHareket where Modul=5 and fkPersoneller=" + pkmusteri.Text).Rows.Count == 0)
            {
                DB.ExecuteSQL("delete from Personeller where pkPersoneller=" + pkmusteri.Text);
                DB.ExecuteSQL("delete from PersonelResim where fkpersonel=" + pkmusteri.Text);
                DB.ExecuteSQL("delete from Personel_Aile where fkPersoneller=" + pkmusteri.Text);
                DevExpress.XtraEditors.XtraMessageBox.Show("Personel Bilgileri Silindi.", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Information);
                simpleButton1_Click(sender, e);
                PersonelAileBilgileri();
                pictureEdit1.Image = null;
            }
            else
                MessageBox.Show("Önce Hareketleri Siliniz!");
            Close();
        }

        private void lUDogumil_EditValueChanged(object sender, EventArgs e)
        {
            proDogumilceler(lUDogumil.EditValue.ToString());
        }
        
        private void dateEdit2_Leave(object sender, EventArgs e)
        {
           
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
              frmilceekle ilceekle = new frmilceekle();
              ilceekle.ilid.Text = LUESehir.EditValue.ToString();
              ilceekle.sehiradi.Text = LUESehir.Text;
              ilceekle.ShowDialog();
            proilceler(LUESehir.EditValue.ToString());
        }

        private void labelControl23_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void adi_EditValueChanged(object sender, EventArgs e)
        {
            label1.Text = adi.Text + " " + Soyadi.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gridControl2.DataSource=
            DB.GetData("select * from PersonelCalismaGunleri where fkPersoneller=" + DB.pkPersoneller);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (gridView3.DataRowCount > 0)
            {
                if (gridView3.FocusedRowHandle < 0) return;
                DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Pzrts", dr["Pzrts"].ToString()));
                list.Add(new SqlParameter("@Sl", dr["Sl"].ToString()));
                list.Add(new SqlParameter("@Crsnb", dr["Crsnb"].ToString()));
                list.Add(new SqlParameter("@Prsnb", dr["Prsnb"].ToString()));
                list.Add(new SqlParameter("@Cm", dr["Cm"].ToString()));
                list.Add(new SqlParameter("@Cmrts", dr["Cmrts"].ToString()));
                list.Add(new SqlParameter("@Pzr", dr["Pzr"].ToString()));
                list.Add(new SqlParameter("@fkPersoneller", DB.pkPersoneller));
                DB.ExecuteSQL("UPDATE PersonelCalismaGunleri SET Pzrts=@Pzrts,Sl=@Sl,Crsnb=@Crsnb,Prsnb=@Prsnb,Cm=@Cm,Cmrts=@Cmrts,Pzr=@Pzr where fkPersoneller=@fkPersoneller", list);
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            frmGorevKarti GorevKarti = new frmGorevKarti();
            GorevKarti.ShowDialog();
            GetBolumler();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            frmRaporGoster rg = new frmRaporGoster();
            //rg.
            rg.Show();
        }

        private void ceMaas_EditValueChanged(object sender, EventArgs e)
        {
            ceNet.Value = ceMaas.Value + cEAgi.Value + cEYemek.Value + cEYol.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt =DB.GetData("select* from PersonelIzınHakedis with(nolock)"+
" where fkPersoneller="+pkmusteri.Text+" and yil = DATEPART(year, getdate())");
            if(dt.Rows.Count==0)
            {
                DB.ExecuteSQL("insert into PersonelIzınHakedis (fkPersoneller,yil,izin_hakedis,izin_kullanilan)"+
                    " values("+ pkmusteri.Text+", DATEPART(year, getdate()),"+seizinhakki.Value.ToString()+",0)");
            }
            else
            {
                DB.ExecuteSQL("update PersonelIzınHakedis set izin_hakedis="+seizinhakki.Value.ToString()+
                    " where personel_izin_id=" + dt.Rows[0]["personel_izin_id"].ToString());
            }

        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            frmPersonelIzinleri izinler = new frmPersonelIzinleri(pkmusteri.Text);
            izinler.ShowDialog();
        }
    }
}
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
using GPTS.islemler;
using DevExpress.XtraReports.UI;

namespace GPTS
{
    public partial class frmCekGirisi : DevExpress.XtraEditors.XtraForm
    {
        string pkFirma_id="0";
        public frmCekGirisi(string fkFirma)
        {
            InitializeComponent();
            pkFirma_id = fkFirma;
        }

        void CekTurleriGetir()
        { 
            lueCekTurleri.Properties.DataSource=
            DB.GetData(@"select pkCekTurleri,CekTuru,Aciklama from CekTurleri with(nolock)
                        where Aktif=1 order by Varsayilan desc");
        }

        private void frmHatirlatma_Load(object sender, EventArgs e)
        {
            xtraTabControl2.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

                deVerildigiTarih.DateTime = DateTime.Today;
                //deVadeTarihi.DateTime = DateTime.Today.AddDays(30);

                CekTurleriGetir();

                cbEvrakTuru.SelectedIndex = 0;

                //lueCekTurleri.ItemIndex = 0;

                ceCekTutari.Focus();

                #region Çek
                this.Text = "Yeni Çek Girişi";
                xtraTabPage4.PageVisible = true;

                DataTable dt = DB.GetData("select * from Cekler with(nolock) where pkCek=" + pkCek.Text);

                if (dt.Rows.Count == 0)
                {
                    lueCekTurleri.Enabled = false;
                    return;
                }
                else
                    lueCekTurleri.Enabled = true;

                MakbuzNo.Text = dt.Rows[0]["MakbuzNo"].ToString();

                deVerildigiTarih.DateTime = Convert.ToDateTime(dt.Rows[0]["VerildigiTarih"].ToString());
                deVadeTarihi.DateTime = Convert.ToDateTime(dt.Rows[0]["Vade"].ToString());

                if (dt.Rows[0]["fkCekTuru"].ToString() != "")
                {
                    lueCekTurleri.EditValue = int.Parse(dt.Rows[0]["fkCekTuru"].ToString());// + 1);
                    //.SelectedIndex =
                }
                Aciklama.Text = dt.Rows[0]["Aciklama"].ToString();
                txtCekSahibi.Text = dt.Rows[0]["cek_sahibi"].ToString();

                decimal tutar = 0;
                decimal.TryParse(dt.Rows[0]["Tutar"].ToString(),out tutar);
                ceCekTutari.Value = tutar;

                //if (dt.Rows[0]["fkCekTuru"].ToString() != "")
                  //  cbEvrakTuru.SelectedIndex = int.Parse(dt.Rows[0]["fkCekTuru"].ToString());

                SeriNo.Text = dt.Rows[0]["SeriNo"].ToString();
                teBankaAdi.Text = dt.Rows[0]["BankaAdi"].ToString();
                teSubeAdi.Text = dt.Rows[0]["SubeAdi"].ToString();
                

                pkFirma.Text = dt.Rows[0]["fkFirma"].ToString();

                cbEvrakTuru.Text = dt.Rows[0]["EvrakTuru"].ToString();

                //seGunOnce.Value = int.Parse(dt.Rows[0]["GunSonraHatirlat"].ToString());

                //if (seGunOnce.Value >0)
                  //  cbCekHatirlat.Checked = true;
                #endregion

                deVadeTarihi.Focus();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (ceCekTutari.Value == 0)
            {
                formislemleri.Mesajform("Çek Tutarı Girini!","K",100);
                ceCekTutari.Focus();
                return;
            }
            if (deVadeTarihi.EditValue == null)
            {
                formislemleri.Mesajform("Vade Tarihini Girini!","K",100);
                deVadeTarihi.Focus();
                return;
            }
            

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Vade", deVadeTarihi.DateTime));
            string cekturu = "0";
            
            if (cbEvrakTuru.SelectedIndex == 1)
                lueCekTurleri.EditValue = "1";

            if (lueCekTurleri.EditValue != null)
                cekturu = lueCekTurleri.EditValue.ToString();

            list.Add(new SqlParameter("@fkCekTuru", cekturu));
                //cbCekDurumu.SelectedIndex));
            list.Add(new SqlParameter("@fkFirma", pkFirma.Text));
            list.Add(new SqlParameter("@Aciklama", Aciklama.Text));
            list.Add(new SqlParameter("@cek_sahibi", txtCekSahibi.Text));
            list.Add(new SqlParameter("@Tutar", ceCekTutari.Text.Replace(",",".")));
            list.Add(new SqlParameter("@SeriNo", SeriNo.Text));
            list.Add(new SqlParameter("@BankaAdi", teBankaAdi.Text));
            list.Add(new SqlParameter("@SubeAdi", teSubeAdi.Text));
            list.Add(new SqlParameter("@VerildigiTarih", deVerildigiTarih.DateTime));
            list.Add(new SqlParameter("@MakbuzNo", MakbuzNo.Text));
            list.Add(new SqlParameter("@EvrakTuru", cbEvrakTuru.Text));
            list.Add(new SqlParameter("@GunSonraHatirlat", "1"));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

            string sonuc = "";
            if (pkCek.Text == "0")
            {
                sonuc = DB.ExecuteScalarSQL(@"INSERT INTO Cekler (Vade,Tutar,fkCekTuru,SeriNo,fkFirma,Aciklama,
cek_sahibi,BankaAdi,SubeAdi,KayitTarihi,VerildigiTarih,MakbuzNo,EvrakTuru,GunSonraHatirlat,fkSube) 
                           values(@Vade,@Tutar,@fkCekTuru,@SeriNo,@fkFirma,@Aciklama,
@cek_sahibi,@BankaAdi,@SubeAdi,getdate(),@VerildigiTarih,@MakbuzNo,@EvrakTuru,@GunSonraHatirlat,@fkSube)
                           SELECT IDENT_CURRENT('Cekler')", list);
                pkCek.Text = sonuc;

                //CekKasasinaEkle();

                if (sonuc.Substring(0, 1) == "H")
                {
                    formislemleri.Mesajform("Hata Oluştur.", "K",200);
                    return;
                }
                else
                {
                    formislemleri.Mesajform("Çek Girişi Yapıldı.", "S",200);

                    //if (cbCekHatirlat.Checked)
                    //    HatirlatmeEkle(sonuc);
                }
            }
            else
            {
                 string sonuc1 = DB.ExecuteSQL(@"UPDATE Cekler SET Vade=@Vade,Tutar=@Tutar,SeriNo=@SeriNo,
                    fkCekTuru=@fkCekTuru,fkFirma=@fkFirma,Aciklama=@Aciklama,cek_sahibi=@cek_sahibi,
                    BankaAdi=@BankaAdi,SubeAdi=@SubeAdi,VerildigiTarih=@VerildigiTarih,EvrakTuru=@EvrakTuru,
                    GunSonraHatirlat=@GunSonraHatirlat WHERE pkCek=" + pkCek.Text, list);

                 if (sonuc1.Substring(0, 1) == "H")
                 {
                     formislemleri.Mesajform("Hata Oluştur.", "K", 200);
                     return;
                 }
                 else
                     formislemleri.Mesajform("Çek Girişi Yapıldı.", "S", 200);

            }

            Close();
        }

        void CekKasasinaEkle()
        {
            ArrayList list = new ArrayList();
            //nakit
                list.Add(new SqlParameter("@fkKasalar", DBNull.Value));
            //Banka
                list.Add(new SqlParameter("@fkBankalar", DBNull.Value));
            //ödeme girişi borç

               //Çek
            list.Add(new SqlParameter("@Borc", ceCekTutari.Text.Replace(",", ".")));

            list.Add(new SqlParameter("@Alacak", "0"));
            list.Add(new SqlParameter("@Tipi", "1"));

            list.Add(new SqlParameter("@Aciklama", Aciklama.Text));
            list.Add(new SqlParameter("@donem", DateTime.Today.Month));
            list.Add(new SqlParameter("@yil", DateTime.Today.Year));
            list.Add(new SqlParameter("@fkFirma", pkFirma.Text));
            
            //tedarikçi ödemesi ise
            list.Add(new SqlParameter("@fkTedarikciler", "0"));

            //personel ödemesi ise
            list.Add(new SqlParameter("@fkPersoneller", "0"));
            list.Add(new SqlParameter("@AktifHesap", "1"));
            list.Add(new SqlParameter("@OdemeSekli", "Çek"));
            //Bakiye Girişi
                list.Add(new SqlParameter("@Tutar", "0"));
            //else
            //    list.Add(new SqlParameter("@Tutar", ceBakiye.Text.Replace(",", ".")));

            list.Add(new SqlParameter("@fkKasaGirisCikisTurleri", "0"));
            list.Add(new SqlParameter("@fkTuru", "1"));
            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));

            string sql = @"INSERT INTO KasaHareket (fkKasalar,fkBankalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,fkKasaGirisCikisTurleri,fkTuru,BilgisayarAdi)
             values(@fkKasalar,@fkBankalar,@fkPersoneller,getdate(),3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,@fkTedarikciler,@OdemeSekli,@Tutar,@fkKasaGirisCikisTurleri,@fkTuru,@BilgisayarAdi)";// SELECT IDENT_CURRENT('KasaHareket')"; 

            string sonuc = DB.ExecuteSQL(sql, list);
            
            if (sonuc.Substring(0, 1) != "H")
            {
                formislemleri.Mesajform("Kasa Hareket Eklenmiştir.", "S", 200);                
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata : " + sonuc, DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            frmMusteriAra ma = new frmMusteriAra();
            ma.ShowDialog();
            pkFirma.Text = DB.PkFirma.ToString();
            //teCariAdi.Text = DB.FirmaAdi;
        }

        private void pkFirma_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + pkFirma.Text);
            if (dt.Rows.Count == 0) return;
            teCariAdi.Text = dt.Rows[0]["Firmaadi"].ToString();
            teTel.Text = dt.Rows[0]["Tel"].ToString();
            teCepTel.Text = dt.Rows[0]["Cep"].ToString();
        }


        private void labelControl8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"1.PORTFÖY: Müşterilerden aldığımız çek ve senetlerin yer kodu PORTFÖY dür. Dosyada bulunan anlamındadır.

2.CİRO: Müşterilerden aldığımız çek ve senetler ile 
Bizden Alacaklı olan Kişi ve Kurumlara ödeme yaptığımız durumlarda çek ve senedin yer kodu CİRO olur.
Ciro etmek: Müşteri çek/senedinin arkasına kaşe ve imza basıp, elimizden çıkarmaktır.

3.TAHSİL: Müşteriden aldığımız çek/senetlerin tahsil edilmek üzere bankaya gönderilmesi durumunda çek/senedin yer kodu TAHSİL olur.

4.TEMİNAT: Müşterilerden aldığımız çek/senetlerin bankaya teminata verilmesi durumunda Çek/senedin yer kodu TEMİNAT olur.
Bankadan kredi çekmek istediğimizde, müşteri çek/senedini bankaya teminat gösterip kredi kullanabiliriz.

5.İADE: Müşteri çek/senedini, müşteriye iade ettiğimizde müşteri çek/senedinin yer kodu İADE olur.");
        }

        private void btnCek_Click(object sender, EventArgs e)
        {
            frmCekTurleri CekTurleri = new frmCekTurleri();
            CekTurleri.ShowDialog();
        }

        private void btYeni_Click(object sender, EventArgs e)
        {
            pkCek.Text = "0";
        }

        private void ceCekTutari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SeriNo.Focus();
            }
        }

        private void cbCekHatirlat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnKaydet.Focus();
            }
        }

        private void deVadeTarihi_EditValueChanged(object sender, EventArgs e)
        {
            //deRandevuTarihi.DateTime = deVadeTarihi.DateTime.AddDays(int.Parse(seGunOnce.Value.ToString())).AddHours(9);
        }


        private void deVerildigiTarih_EditValueChanged(object sender, EventArgs e)
        {

        }    
    }
}
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
using DevExpress.XtraReports.UI;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmKasaGiris_Cikis : DevExpress.XtraEditors.XtraForm
    {
        public frmKasaGiris_Cikis()
        {
            InitializeComponent();            
        }

        private void frmKasaGirisi_Load(object sender, EventArgs e)
        {
            cbYazdir.Checked = Degerler.makbuzyazdir;
            #region Yetkiler
            DataTable dtYetki = DB.GetData(@"select * from ModullerYetki with(nolock) where Kod like '3%' and fkKullanicilar=" + DB.fkKullanicilar);

            for (int i = 0; i < dtYetki.Rows.Count; i++)
            {
                string Kod = dtYetki.Rows[i]["Kod"].ToString();
                string yetki = dtYetki.Rows[i]["Yetki"].ToString();
                ////kasa Girişi
                if (Kod == "3.4" &&  yetki == "False")
                {
                    BtnKaydet.Enabled = kaydetToolStripMenuItem.Enabled= false;
                    //simpleButton3.Enabled = false;
                    //ceDevirTutar.Enabled = false;
                }
                else if (Kod == "3.5" && yetki == "False")//kasa çıkışı
                {
                    BtnKaydet.Enabled = tsmHareketEdit.Enabled = kaydetToolStripMenuItem.Enabled = false;
                }
                else if (Kod == "3.7" && yetki == "False")//Kasa Hareketi Sil
                {
                    tsmHareketSil.Enabled = false;
                }
                else if (Kod == "3.8" && yetki == "False")//Kasa Harelketi Düzelt
                {
                    tsmHareketEdit.Enabled = false;
                }
            }
            #endregion
            //ödeme çıkışı
            if (this.Tag.ToString() == "1")
                BtnKaydet.Text = "Ödeme Al [F9]";
            else
                BtnKaydet.Text = "Ödeme Yap [F9]";

            System.Drawing.Font f = new System.Drawing.Font("Arial", 26);

            ceTutarNakit.Font = f;
            ceTutarKredi.Font = f;

            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            Kasalar();

            Bankalar();

            //Ödeme Girişi
            if (this.Tag.ToString() == "1")
            {
                panel1.BackColor = System.Drawing.Color.LightGreen;
                panel2.BackColor = System.Drawing.Color.LightGreen;
                panel3.BackColor = System.Drawing.Color.LightGreen;
                pnlAciklama.BackColor = System.Drawing.Color.LightGreen;
                panel6.BackColor = System.Drawing.Color.LightGreen;

                this.Text = "Ödeme Girişi";

                gcBorc.Visible = true;
                gcAlacak.Visible = false;
                cbGelirMi.Visible = true;
                cbGelirMi.Checked = true;
                ceGiderMi.Visible = false;
            }
            else
            {
                panel1.BackColor = Color.FromArgb(0xF2, 0x8F, 0x7B);
                panel2.BackColor = Color.FromArgb(0xF2, 0x8F, 0x7B);
                panel3.BackColor = Color.FromArgb(0xF2, 0x8F, 0x7B);
                pnlAciklama.BackColor = Color.FromArgb(0xF2, 0x8F, 0x7B);
                panel6.BackColor = Color.FromArgb(0xF2, 0x8F, 0x7B);

                this.Text = "Ödeme Çıkışı";

                gcBorc.Visible = false;
                gcAlacak.Visible = true;
                cbGelirMi.Visible = false;
                ceGiderMi.Visible = true;
                ceGiderMi.Checked = true;
            }

            islemtarihi.DateTime = DateTime.Now;
            if (pkFirma.Text != "0")
            {
                cbHareketTipi.SelectedIndex = 1;
                lueMusteriler.EditValue = pkFirma.Text;

                //if (pkTaksitler.Text != "0")
                //{
                //    DataTable dtTak = null;
                //    MessageBox.Show("Taksit Ödemesi");
                //    tEaciklama.Text = "Taksit";
                //}
            }
            else if (pkTedarikci.Text != "0")
            {
                cbHareketTipi.SelectedIndex = 2;
                lueMusteriler.EditValue = pkTedarikci.Text;
            }
            else if (pkPersonel.Text != "0")
            {

                cbHareketTipi.SelectedIndex = 3;
                lueMusteriler.EditValue = pkPersonel.Text;
            }
            else
                cbHareketTipi.SelectedIndex = 0;

            //cbHareketTipi.Focus();

            cbTarihAraligi.SelectedIndex = 7;
            ilktarih.DateTime = DateTime.Today.AddDays(-100);
            sontarih.DateTime = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);

            KasaHareketleriGetir();

            ceTutarNakit.Focus();

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            //Ödeme Girişi
            if (this.Tag.ToString() == "1")
                yi.GridTasarimYukle(gridView1, "KasaGirisiGrid");
            else
                yi.GridTasarimYukle(gridView1, "KasaCikisiGrid");
        }

        void Kasalar()
        {
            lueKasa.Properties.DataSource = DB.GetData("SELECT * FROM Kasalar with(nolock) where Aktif=1");
            lueKasa.EditValue = Degerler.fkKasalar;
        }

        void Bankalar()
        {
            lueBanka.Properties.DataSource = DB.GetData("SELECT * FROM Bankalar with(nolock) where Aktif=1 order by tiklamaadedi desc");
            //lueBanka.ItemIndex = 1;
        }
        //fkCekTuru giriş mi çıkış mı 1 giriş 2 çıkış
        void CekListesi(int fkCekTuru)
        {
            string sql = @"select 0 as pkCek,'Seçiniz' as Firmaadi,getdate() as Vade,0 as Tutar,'0-Seçiniz' as EvrakTuru,'Seçiniz' as Aciklama 
            union all 
            select pkCek,Firmaadi,Vade,Tutar,EvrakTuru,Aciklama from Cekler c with(nolock) 
            left join Firmalar f with(nolock)  on c.fkFirma=f.pkFirma";
            //Ödeme Girişi
            if (this.Tag.ToString() == "1")
                sql = sql + " where (fkCekTuru in(0))";
            else
                sql = sql + " where (fkCekTuru in(0,1))";

            sql = sql +" order by pkCek desc";

            lueCekler.Properties.DataSource = DB.GetData(sql);

        }

        void SenetListesi(int fkFirma)
        {
            if (lueMusteriler.EditValue == null)
            {
                cbHareketTipi.SelectedIndex = 3;
                lueMusteriler.EditValue = int.Parse(pkFirma.Text);
            }
            string sql = @"select pkTaksitler,tl.Tarih,Odenecek,Odenen,Odenecek-Odenen as kalan,tl.Aciklama,OdemeSekli from Taksit t with(nolock)
            left join Taksitler tl with(nolock) on t.taksit_id=tl.taksit_id
            where Odenecek-Odenen <> 0 and t.fkFirma=" + lueMusteriler.EditValue.ToString();

            //Ödeme Girişi
            //if (this.Tag.ToString() == "1")
            //    sql = sql + " where (fkSenetTuru in(0))";
            //else
            //    sql = sql + " where (fkSenetTuru in(0,1))";

            sql = sql + " order by Tarih";

            lueTaksitler.Properties.DataSource = DB.GetData(sql);
            if (pkTaksitler.Text != "")
                lueTaksitler.EditValue = int.Parse(pkTaksitler.Text);
        }

        void KasaHareketleriGetir()
        {
            string sql = "";
            if (this.Tag.ToString() == "1")
            {
                sql = @"SELECT TOP (100) kh.pkKasaHareket, kh.Tarih, kh.Aciklama, kh.fkFirma, kh.fkTedarikciler,kh.fkPersoneller, kh.OdemeSekli,kh.Alacak, kh.Borc, 
                      Bankalar.BankaAdi,kh.AktifHesap
                      ,case 
                      when kh.fkFirma >0 then 'Müşteri Ödemesi'
                      when kh.fkTedarikciler>0 then 'Tedarikçi Ödemesi'
                      when kh.fkPersoneller>0 then p.adi +' '+p.soyadi
                      else 'Diğer' end HareketTuru,
                      case 
                      when kh.fkFirma >0 then f.Firmaadi
                      when kh.fkTedarikciler>0 then t.Firmaadi
                      else tur.Aciklama end Adi,fkCek,isnull(fkTaksitler,0) as fkTaksitler,
                    kh.MakbuzNo,kh.olusturma_tarihi,kh.guncelleme_tarihi,kh.fkKullanicilar
                    FROM  KasaHareket  kh with(nolock)
                    LEFT JOIN Bankalar  with(nolock) ON kh.fkBankalar = Bankalar.PkBankalar 
                    LEFT JOIN Firmalar f with(nolock) ON kh.fkFirma = f.pkFirma 
                    LEFT JOIN Personeller p with(nolock)  ON kh.fkPersoneller = p.pkpersoneller 
                    LEFT JOIN Tedarikciler t with(nolock) ON kh.fkTedarikciler = t.pkTedarikciler
                    LEFT JOIN KasaGirisCikisTurleri tur with(nolock) on tur.pkKasaGirisCikisTurleri=kh.fkKasaGirisCikisTurleri
                    WHERE fkSatislar is null and kh.Borc <> 0 and OdemeSekli<>'Bakiye Düzeltme' and (kh.fkSube is null or kh.fkSube=" + Degerler.fkSube+")";
                if (!ceTumu.Checked)
                    sql = sql + " and kh.fkKullanicilar=" + DB.fkKullanicilar;
                sql = sql + " ORDER BY kh.pkKasaHareket DESC";
            }
           
            if (this.Tag.ToString() == "2")
            {
                sql = @"SELECT TOP (100) kh.pkKasaHareket, kh.Tarih, kh.Aciklama, kh.fkFirma, kh.fkTedarikciler,kh.fkPersoneller, kh.OdemeSekli,kh.Alacak, kh.Borc, 
                      Bankalar.BankaAdi,kh.AktifHesap
                      ,case 
                      when kh.fkFirma >0 then 'Müşteri Ödemesi'
                      when kh.fkTedarikciler>0 then 'Tedarikçi Ödemesi'
                      when kh.fkPersoneller>0 then 'Personel Ödemesi'
                      else 'Diğer' end HareketTuru,
                      case 
                      when kh.fkFirma >0 then f.Firmaadi
                      when kh.fkTedarikciler>0 then t.Firmaadi
					  when kh.fkPersoneller>0 then p.adi +' '+p.soyadi
                      else tur.Aciklama end Adi,fkCek,isnull(fkTaksitler,0) as fkTaksitler,
                    kh.MakbuzNo,kh.olusturma_tarihi,kh.guncelleme_tarihi
                    FROM  KasaHareket kh with(nolock)  
                    LEFT JOIN Bankalar with(nolock) ON kh.fkBankalar = Bankalar.PkBankalar 
                    LEFT JOIN Firmalar f with(nolock)  ON kh.fkFirma = f.pkFirma 
					LEFT JOIN Personeller p with(nolock)  ON kh.fkPersoneller = p.pkpersoneller 
                    LEFT JOIN Tedarikciler t with(nolock) ON kh.fkTedarikciler = t.pkTedarikciler
                    LEFT JOIN KasaGirisCikisTurleri tur with(nolock) on tur.pkKasaGirisCikisTurleri=kh.fkKasaGirisCikisTurleri
                    WHERE fkSatislar is null and kh.Alacak<> 0 and OdemeSekli<>'Bakiye Düzeltme' and (kh.fkSube is null or kh.fkSube=" + Degerler.fkSube + ")";
                if (!ceTumu.Checked)
                    sql = sql + " and kh.fkKullanicilar=" + DB.fkKullanicilar;
                sql = sql + " ORDER BY kh.pkKasaHareket DESC";
            }
                gridControl1.DataSource = DB.GetData(sql);

            //tEaciklama.Text = "";
            islemtarihi.EditValue = DateTime.Now;
            //ceTutarNakit.Value = 0;
            //tEaciklama.Focus();
        }

        private void frmKasaGirisi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (BtnKaydet.Enabled == false) return;

            string OdemeTutari = "0,00";

            #region uyarilar
            if (cbOdemeSekli.SelectedIndex == 0)
                OdemeTutari = ceTutarNakit.Text;
            else if(cbOdemeSekli.SelectedIndex==1 || cbOdemeSekli.SelectedIndex == 4)
                OdemeTutari = ceTutarKredi.Text;
            else if (cbOdemeSekli.SelectedIndex == 2 )
            {
                //çek girişi
                OdemeTutari = lueCekler.Text.Replace(",",".");
                if (OdemeTutari == "" || OdemeTutari == "Seçiniz..." || OdemeTutari == ".00")
                {
                    MessageBox.Show("Çek Listesini Kontrol Ediniz!");
                    return;
                }
            }
            else if (cbOdemeSekli.SelectedIndex == 3)
            {
                OdemeTutari = lueTaksitler.Text.Replace(",", ".");
                if (OdemeTutari == "" || OdemeTutari=="Seçiniz...")
                {
                    MessageBox.Show("Senet Listesini Kontrol Ediniz!");
                    return;
                }
            }

            if (cbOdemeSekli.SelectedIndex == 0 && lueKasa.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Kasa Seçiniz!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueKasa.Focus();
                return;
            }
            if (cbOdemeSekli.SelectedIndex == 1 && lueBanka.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Banka Seçiniz!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueBanka.Focus();
                return;
            }
            if (islemtarihi.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Tarih Giriniz!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                islemtarihi.Focus();
                return;
            }
            if (cbOdemeSekli.SelectedIndex==0 && ceTutarNakit.Value == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Tutar Sıfırdan Büyük Olmalı!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ceTutarNakit.Focus();
                return;
            }
            else if (cbOdemeSekli.SelectedIndex==1 && ceTutarKredi.Value == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Tutar Sıfırdan Büyük Olmalı!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ceTutarKredi.Focus();
                return;
            }
            else if (cbOdemeSekli.SelectedIndex == 2 && lueCekler.Text == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Tutar Sıfırdan Büyük Olmalı!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ceTutarCek.Focus();
                return;
            }
            //müşteri ödemesi
            if (cbHareketTipi.SelectedIndex == 1 && lueMusteriler.EditValue == "0")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Müşteriden Ödeme Alınamaz!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueMusteriler.Focus();
                return;
            }

            #endregion

            string Mesaj = " Ödeme Girişini Onaylıyormusunuz?";

            if (this.Tag.ToString() == "2") Mesaj = " Ödeme Çıkışını Onaylıyormusunuz?";

            //Müşteri
            if (cbHareketTipi.SelectedIndex==1) Mesaj = Mesaj + "\n\n" + lueMusteriler.Text;
            //Tedarikçi
            if (cbHareketTipi.SelectedIndex==2) Mesaj = Mesaj + "\n\n" + lueMusteriler.Text;
            //personel
            if (cbHareketTipi.SelectedIndex==3) Mesaj = Mesaj + "\n\n" + lueMusteriler.Text;


            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show(OdemeTutari + " TL " + cbOdemeSekli.Text+" " + Mesaj, DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            ArrayList list = new ArrayList();
            //nakit veya senet ise senedi nakit olarak ödeyebilir
            if ((cbOdemeSekli.SelectedIndex == 0 || cbOdemeSekli.SelectedIndex == 3) && cbAktifHesap.Checked)
                list.Add(new SqlParameter("@fkKasalar", int.Parse(lueKasa.EditValue.ToString())));
            else
                list.Add(new SqlParameter("@fkKasalar", DBNull.Value));

            //kredi kartı veya Banka
            if (cbOdemeSekli.SelectedIndex == 1 && cbAktifHesap.Checked)
                list.Add(new SqlParameter("@fkBankalar", int.Parse(lueBanka.EditValue.ToString())));
            else if (cbOdemeSekli.SelectedIndex == 4 && cbAktifHesap.Checked)
                list.Add(new SqlParameter("@fkBankalar", int.Parse(lueBanka.EditValue.ToString())));
            else
                 list.Add(new SqlParameter("@fkBankalar", DBNull.Value));

            list.Add(new SqlParameter("@Tarih", islemtarihi.DateTime));
            //ödeme girişi borç
            if (this.Tag.ToString() == "1")
            {
                //nakit
                if (cbOdemeSekli.SelectedIndex == 0)
                    list.Add(new SqlParameter("@Borc", ceTutarNakit.Value.ToString().Replace(",", ".")));
                //kredi kartı
                else if (cbOdemeSekli.SelectedIndex == 1)
                    list.Add(new SqlParameter("@Borc", ceTutarKredi.Value.ToString().Replace(",", ".")));
                //Çek
                else if (cbOdemeSekli.SelectedIndex == 2)
                    list.Add(new SqlParameter("@Borc", lueCekler.Text.ToString().Replace(",", ".").Replace(" ","")));
                //senet
                else if (cbOdemeSekli.SelectedIndex == 3)
                    list.Add(new SqlParameter("@Borc", lueTaksitler.Text.ToString().Replace(",", ".").Replace(" ", "")));
                //banka
                else if (cbOdemeSekli.SelectedIndex == 4)
                    list.Add(new SqlParameter("@Borc", ceTutarKredi.Value.ToString().Replace(",", ".")));

                list.Add(new SqlParameter("@Alacak", "0"));
                list.Add(new SqlParameter("@Tipi", "1"));
            }
                //ödeme çıkışı alacak
            else if (this.Tag.ToString() == "2")
            {
                if (cbOdemeSekli.SelectedIndex == 0)
                    list.Add(new SqlParameter("@Alacak", ceTutarNakit.Value.ToString().Replace(",", ".")));
                //kredi kartı
                else if (cbOdemeSekli.SelectedIndex == 1)
                    list.Add(new SqlParameter("@Alacak", ceTutarKredi.Value.ToString().Replace(",", ".")));
                //Çek
                else if (cbOdemeSekli.SelectedIndex == 2)
                    list.Add(new SqlParameter("@Alacak", lueCekler.Text.Replace(",", ".").Replace(" ", "")));
                //Taksit
                else if (cbOdemeSekli.SelectedIndex == 3)
                    list.Add(new SqlParameter("@Alacak", lueTaksitler.Text.Replace(",", ".").Replace(" ", "")));
                //banka
                else if (cbOdemeSekli.SelectedIndex == 4)
                    list.Add(new SqlParameter("@Alacak", ceTutarKredi.Value.ToString().Replace(",", ".")));


                list.Add(new SqlParameter("@Borc", "0"));
                list.Add(new SqlParameter("@Tipi", "2"));
            }
            list.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list.Add(new SqlParameter("@donem", islemtarihi.DateTime.Month));
            list.Add(new SqlParameter("@yil", islemtarihi.DateTime.Year));
            //müşteri ödemesi ise
            if (cbHareketTipi.SelectedIndex==1 && lueMusteriler.EditValue != null) 
                list.Add(new SqlParameter("@fkFirma", lueMusteriler.EditValue.ToString()));
            else
                list.Add(new SqlParameter("@fkFirma", DBNull.Value));
            //tedarikçi ödemesi ise
            if (cbHareketTipi.SelectedIndex == 2 && lueMusteriler.EditValue != null)
                list.Add(new SqlParameter("@fkTedarikciler", lueMusteriler.EditValue.ToString()));
            else
                list.Add(new SqlParameter("@fkTedarikciler", DBNull.Value));
                
            //personel ödemesi ise
            if (cbHareketTipi.SelectedIndex == 3 && lueMusteriler.EditValue != null)
                list.Add(new SqlParameter("@fkPersoneller", lueMusteriler.EditValue.ToString()));
            else
                list.Add(new SqlParameter("@fkPersoneller", DBNull.Value));
            //çekde aktif hesapdır
            //if (cbOdemeSekli.SelectedIndex == 2)
            //    list.Add(new SqlParameter("@AktifHesap", false));
            //else
            list.Add(new SqlParameter("@AktifHesap", cbAktifHesap.Checked));
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.Text));
            //Bakiye Girişi
            if (ceBakiye.Text=="")
                list.Add(new SqlParameter("@Tutar", "0"));
            else
            list.Add(new SqlParameter("@Tutar", ceBakiye.Text.Replace(",", ".")));
            if (cbHareketTipi.SelectedIndex == 0)
               list.Add(new SqlParameter("@fkKasaGirisCikisTurleri", lueMusteriler.EditValue.ToString()));
            else
                list.Add(new SqlParameter("@fkKasaGirisCikisTurleri","0"));
            list.Add(new SqlParameter("@fkTuru", cbHareketTipi.SelectedIndex));
            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

            if (lueCekler.EditValue==null || cbOdemeSekli.Text == "Taksit")
                list.Add(new SqlParameter("@fkCek", "0"));
            else
               list.Add(new SqlParameter("@fkCek", lueCekler.EditValue.ToString()));
            //list.Add(new SqlParameter("@fkTaksitler", pkTaksitler.Text));

            if (lueTaksitler.EditValue == null || cbOdemeSekli.Text=="Çek")
                list.Add(new SqlParameter("@fkTaksitler", pkTaksitler.Text));
            else
            {
                list.Add(new SqlParameter("@fkTaksitler", lueTaksitler.EditValue.ToString()));
                pkTaksitler.Text = lueTaksitler.EditValue.ToString();
                if (lueTaksitler.Text == "")
                    ceTutarNakit.Value = 0;
                else
                    ceTutarNakit.Value = decimal.Parse(lueTaksitler.Text);
            }
            list.Add(new SqlParameter("@MakbuzNo", txtMakbuxNo.Text));
            list.Add(new SqlParameter("@GiderOlarakisle", ceGiderMi.Checked));
            list.Add(new SqlParameter("@GelirOlarakisle", cbGelirMi.Checked));
            list.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

            string sql = @"INSERT INTO KasaHareket (fkKasalar,fkBankalar,fkPersoneller,Tarih,Modul,Tipi,"+
                "Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,"+
                "fkKasaGirisCikisTurleri,fkTuru,BilgisayarAdi,fkKullanicilar,fkCek,MakbuzNo,fkTaksitler,"+
                "GiderOlarakisle,GelirOlarakisle,aktarildi,fkSube)" +
                " values(@fkKasalar,@fkBankalar,@fkPersoneller,@Tarih,3,@Tipi,"+
                "@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,@fkTedarikciler,@OdemeSekli,@Tutar,"+
                "@fkKasaGirisCikisTurleri,@fkTuru,@BilgisayarAdi,@fkKullanicilar,@fkCek,@MakbuzNo,@fkTaksitler,"+
                "@GiderOlarakisle,@GelirOlarakisle,@aktarildi,@fkSube)";
            // SELECT IDENT_CURRENT('KasaHareket')"; 

            string sonuc = DB.ExecuteSQL(sql, list);

            if (sonuc.Substring(0, 1) != "H")
            {
                formislemleri.Mesajform("Kasa Hareket Eklenmiştir.", "S", 200);
                
                //ödeme girişi borç
                if (lueMusteriler.EditValue !=null &&  this.Tag.ToString() == "1")
                    DB.ExecuteSQL("UPDATE Firmalar Set Aktarildi=0,SonOdemeTarihi=getdate() where pkFirma=" + lueMusteriler.EditValue.ToString());
                //ödeme şekli çek ise ve müşteri ise
                if (cbOdemeSekli.SelectedIndex == 2)
                {
                    //ödeme girişi borç
                    if (this.Tag.ToString() == "1")
                    {
                        DB.ExecuteSQL("UPDATE Cekler Set fkCekTuru=1 " +
                            ",fkFirma=" + lueMusteriler.EditValue.ToString() +
                            " where pkCek=" + lueCekler.EditValue.ToString());

                        DataTable dbCek =   DB.GetData("select * from Cekler with(nolock) where pkCek=" + lueCekler.EditValue.ToString());
                        if (dbCek.Rows.Count > 0)
                        {
                            DateTime animsattar = Convert.ToDateTime(dbCek.Rows[0]["Vade"].ToString());
                            if (animsattar < DateTime.Now)
                                animsattar = DateTime.Now;

                            DBislemleri.YeniHatirlatmaEkle("Çek Hatırlatma", animsattar, "Çek Ödemesi", animsattar.AddHours(1), lueMusteriler.EditValue.ToString(), "0", lueCekler.EditValue.ToString(),"0");
                        }
                    }
                    //ödeme çıkışı ise
                    if (this.Tag.ToString() == "2")
                    {
                        DB.ExecuteSQL("UPDATE Cekler Set fkCekTuru=2 " +
                            ",fkTedarikci=" + lueMusteriler.EditValue.ToString() +
                            " where pkCek=" + lueCekler.EditValue.ToString());
                    }
                }
                if (pkTaksitler.Text != "0")
                {
                    if (ceTutarNakit.Value > 0)
                        DB.ExecuteSQL("UPDATE Taksitler Set Odenen=" + ceTutarNakit.EditValue.ToString().Replace(",", ".") +
                            ",OdendigiTarih=getdate(),OdemeSekli='Nakit'" +
                            " where pkTaksitler=" + pkTaksitler.Text);

                    if (ceTutarKredi.Value > 0)
                        DB.ExecuteSQL("UPDATE Taksitler Set Odenen=" + ceTutarKredi.EditValue.ToString().Replace(",", ".") +
                            ",OdendigiTarih=getdate(),OdemeSekli='Kredi Kartı'" +
                            " where pkTaksitler=" + pkTaksitler.Text);
                }

                comboBoxEdit3_SelectedIndexChanged(sender, e);//çekleri güncelle

                lueCekler.EditValue = null;
                BtnKaydet.Text = "Kaydet [F9]";
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata : " + sonuc, DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            KasaHareketleriGetir();
            
            islemtarihi.EditValue = DateTime.Now;
            pkFirma.Text = "0";
            tEaciklama.Text = "";
            ceTutarNakit.Value = 0;
            ceTutarKredi.Value = 0;
            pkTaksitler.Text = "0";
            pkPersonel.Text = "0";
            pkTedarikci.Text = "0";
            txtMakbuxNo.Text = "";

            MusteriBorcu();

            KasaBakiye();

            gridControl1.Focus();
            gridView1.Focus();

            makbuzbuttonaktifpasif();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        void KasaGirisCikisTurleri()
        {
            string GirisCikis = "1";

            if (this.Tag.ToString() == "2") GirisCikis = "0";

            lueMusteriler.Properties.DataSource = DB.GetData(@"select 0 as id,'Diğer' as Firmaadi,'' as GrupAdi,0 as GelirOlarakisle,0 as GiderOlarakisle
            union all 
            select pkKasaGirisCikisTurleri as id,Aciklama as Firmaadi,kgcg.GrupAdi,kgct.GelirOlarakisle,kgct.GiderOlarakisle from KasaGirisCikisTurleri kgct with(nolock) 
            left join KasaGirisCikisGruplari kgcg with(nolock) on kgcg.pkKasaGirisCikisGruplari=kgct.fkKasaGirisCikisGruplari
            where kgct.Aktif=1 and kgct.GirisCikis=" + GirisCikis);
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //gider
            if (this.Tag.ToString() == "2") 
                ceGiderMi.Visible = true;
            else
                cbGelirMi.Visible = true;

            lueMusteriler.EditValue = null;
            //Diğer
            if (cbHareketTipi.SelectedIndex == 0)
            {
                btnKasaTuru.Visible = true;
                btnKasaTuruList.Visible = true;
                lMusteriBakiyesi.Visible = false;
                ceBakiye.Visible = false;
            }
            else
            {
                btnKasaTuru.Visible = false;
                btnKasaTuruList.Visible = false;
                lMusteriBakiyesi.Visible = true;
                ceBakiye.Visible = true;
            }
            //diğer
            if (cbHareketTipi.SelectedIndex == 0)
            {
                lMusteriAdi.Text = "Diğer";
                lMusteriBakiyesi.Text = "Diğer";

                KasaGirisCikisTurleri();

                lueMusteriler.ItemIndex=0;
            }
                //Müşteri
            else if (cbHareketTipi.SelectedIndex == 1)
            {
                lMusteriAdi.Text = "Müşteri Adı";
                lMusteriBakiyesi.Text = "Müşteri Bakiyesi";
                
                lueMusteriler.Properties.DataSource = DB.GetData(@"
                select 0 as id,'Müşteri Seçiniz.' as Firmaadi 
                union all 
                select pkFirma as id,Firmaadi from Firmalar with(nolock) where pkFirma>1 and Aktif=1");
                
                if (pkFirma.Text!= "0")
                    lueMusteriler.EditValue = int.Parse(pkFirma.Text);

                ceGiderMi.Checked = false;
                cbGelirMi.Checked = true;

            }
                //tedarikçi
            else if (cbHareketTipi.SelectedIndex == 2)
            {
                lMusteriAdi.Text = "Tedarikçi Adı";
                lMusteriBakiyesi.Text = "Tedarikçi Bakiyesi";
                lueMusteriler.Properties.DataSource = DB.GetData(@"
                select 0 as id,'Tedarikçi Seçiniz.' as Firmaadi 
                union all 
                select pkTedarikciler as id,Firmaadi from Tedarikciler with(nolock) where pkTedarikciler>1 and Aktif=1");
                
                if (pkTedarikci.Text != "0")
                    lueMusteriler.EditValue = int.Parse(pkTedarikci.Text);

                //gider
                if (this.Tag.ToString() == "2")
                    ceGiderMi.Checked = false;
                else
                    cbGelirMi.Checked = false;
            }
                //personel
            else if (cbHareketTipi.SelectedIndex == 3)
            {
                lMusteriAdi.Text = "Personel Adı";
                lMusteriBakiyesi.Text = "Personel Bakiyesi";
                lueMusteriler.Properties.DataSource = DB.GetData(@"
                select 0 as id,'Personel Seçiniz.' as Firmaadi 
                union all 
                select pkPersoneller as id,Adi+' '+Soyadi as Firmaadi from Personeller with(nolock) where AyrilisTarihi is null");
                if (int.Parse(pkPersonel.Text) > 0)
                    lueMusteriler.EditValue = int.Parse(pkPersonel.Text);
                //gider
                if (this.Tag.ToString() == "2") 
                   ceGiderMi.Checked = true;
                else
                   cbGelirMi.Checked = true;
            }

            //gizle göster
            if (cbHareketTipi.SelectedIndex == 0)
            {
                if (this.Tag.ToString() == "2")
                    lueMusteriler.Properties.Columns[3].Visible = false;
                else
                    lueMusteriler.Properties.Columns[4].Visible = false;
            }
            else
            {
                lueMusteriler.Properties.Columns[0].Visible = false;
                //lueMusteriler.Properties.Columns[1].Visible = false;
                lueMusteriler.Properties.Columns[3].Visible = false;
                lueMusteriler.Properties.Columns[4].Visible = false;
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            if (!formislemleri.SifreIste()) return;

            string sec = formislemleri.MesajBox("Silmek İstediğinize Eminmisiniz ?", "Kasa Hareketi Sil", 3, 0);
            if (sec != "1") return;

            inputForm sifregir1 = new inputForm();
            sifregir1.Text = "Silinme Nedeni";
            sifregir1.GirilenCaption.Text = "Silme Nedenini Giriniz!";
            sifregir1.Girilen.Text = "Hatalı Ödeme Al";
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir1.ShowDialog();

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkKasaHareket = dr["pkKasaHareket"].ToString();
            string fkCek = dr["fkCek"].ToString();
            if (fkCek == "") fkCek = "0";
            string fkTaksitler = dr["fkTaksitler"].ToString();
            if (fkTaksitler == "") fkTaksitler = "0";
            string fkFirma = dr["fkFirma"].ToString();
            if (fkFirma == "") fkFirma = "0";

            string borc = dr["Borc"].ToString();
            string alacak = dr["Alacak"].ToString();

            //,odeme_sekli,,bilgisayar_adi,fkFirma 
            DB.ExecuteSQL("INSERT INTO KasaHareketLog (fkKasaHareket,aciklama,tarih,fkKullanicilar,bilgisayar_adi,fkFirma,borc,alacak) " +
           " VALUES(" + pkKasaHareket + ",'" + sifregir1.Girilen.Text + "',getdate()," + DB.fkKullanicilar + ",'" + Degerler.BilgisayarAdi+"',"+fkFirma + ","+
           borc.Replace(',', '.') + "," + alacak.Replace(',', '.')+")");

          
            #region trasn başlat
            DB.trans_basarili = true;
            DB.trans_hata_mesaji = "";
            if (DB.conTrans == null)
                DB.conTrans = new SqlConnection(DB.ConnectionString());
            if (DB.conTrans.State == ConnectionState.Closed)
                DB.conTrans.Open();

            DB.transaction = DB.conTrans.BeginTransaction("KasaTransaction");
            #endregion

            #region İşlemler
            string sonuc = "";
            if (fkCek != "0")
            {
                sonuc = DB.ExecuteSQLTrans("update Cekler set fkFirma=0,fkCekTuru=0 where pkCek=" + fkCek);
                DB.ExecuteSQL("delete from HatirlatmaAnimsat where fkCek=" + fkCek);
            }
            if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            {
                DB.trans_hata_mesaji = "Kasa Çek " + sonuc;
                DB.trans_basarili = false;
            }


            if (fkTaksitler != "" && DB.trans_basarili)
            {
                sonuc = DB.ExecuteSQLTrans(@"UPDATE Taksitler Set Odenen=0,OdendigiTarih=null,OdemeSekli='silindi'
                        where pkTaksitler=" + fkTaksitler);

                 if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
                 {
                     DB.trans_hata_mesaji = "Kasa Taksit " + sonuc;
                     DB.trans_basarili = false;
                 }
            }
            sonuc = DB.ExecuteSQLTrans("DELETE FROM KasaHareket WHERE pkKasaHareket=" + pkKasaHareket);
            if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            {
                DB.trans_hata_mesaji = "Kasa Hareket " + sonuc;
                DB.trans_basarili = false;
            }

            //if (fkFirma != "")
            //{
            //    sonuc = DB.ExecuteSQLTrans("update Firmalar set Aktarildi="+Degerler.AracdaSatis+" where pkFirma=" + fkFirma);
            //    if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            //    {
            //        DB.trans_hata_mesaji = "Firmalar aktarıldı " + sonuc;
            //        DB.trans_basarili = false;
            //    }
            //}
            #endregion

            #region trasn sonu
            if (DB.trans_basarili == true)
            {
                DB.transaction.Commit();
            }
            else
            {
                DB.transaction.Rollback();
                formislemleri.Mesajform(DB.trans_hata_mesaji, "K", 200);
            }
            DB.conTrans.Close();
            #endregion

            MusteriBorcu();

            KasaHareketleriGetir();

            comboBoxEdit3_SelectedIndexChanged(sender, e);

            lueCekler.ItemIndex = 0;

        }

        private void comboBoxEdit3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //tEaciklama.Text = "";
            switch (cbOdemeSekli.SelectedIndex)
            {
                case 0:{
                        //nakit
                     xtraTabControl1.SelectedTabPageIndex = 0;
                        cbAktifHesap.Text = "Kasaya İşle";
                    break;
                }
                case 1:
                    {
                        //k.kartı
                        xtraTabControl1.SelectedTabPageIndex = 1;
                        cbAktifHesap.Text = "Bankaya İşle";
                        break;
                    }
                case 2://çek
                    {
                        if (this.Tag.ToString() == "1")//çek giriş ise
                        {
                            xtraTabControl1.SelectedTabPageIndex = 2;
                            CekListesi(0);
                        }
                        else if (this.Tag.ToString() == "2")//çek çıkış ise
                        {
                            xtraTabControl1.SelectedTabPageIndex = 2;
                            CekListesi(2);
                        }
                        cbAktifHesap.Text = "Kasaya İşle";
                        break;
                    }
                case 3://senet
                    {
                        if (this.Tag.ToString() == "1")//senet giriş ise
                        {
                            xtraTabControl1.SelectedTabPage = xTabSenet;
                            SenetListesi(0);
                        }
                        else if (this.Tag.ToString() == "2")//senet çıkış ise
                        {
                            MessageBox.Show("Yapım Aşamasında");
                            cbOdemeSekli.SelectedIndex = 2;
                            //xtraTabControl1.SelectedTabPageIndex = 2;
                            //xtraTabControl1.SelectedTabPageIndex = 3;
                            //SenetListesi(2);
                        }
                        cbAktifHesap.Text = "Kasaya İşle";
                        break;
                    }
                case 4:
                    {
                        //Banka
                        //MessageBox.Show("Yapım Aşamasında");
                        xtraTabControl1.SelectedTabPageIndex = 1;
                        //cbOdemeSekli.SelectedIndex = 1;
                        if(tEaciklama.Text=="")
                           tEaciklama.Text = "Havale veya EFT";

                        cbAktifHesap.Text = "Bankaya İşle";
                        break;
                    }
                    //default:
                    //    break;
            }
           
            ceTutarNakit.Value = 0;
            ceTutarKredi.Value = 0;
        }

        void MusteriBorcu()
        {
            if (cbHareketTipi.SelectedIndex == 0) return;

            if (lueMusteriler.EditValue == null) return;

            ceBakiye.Text = "0,00";

            string sql = "";
            if (cbHareketTipi.SelectedIndex == 1)//müşteri listesi
                sql = "exec sp_MusteriBakiyesi " + lueMusteriler.EditValue.ToString() + ",0";
            else if (cbHareketTipi.SelectedIndex == 2)//müşteri listesi
                sql = "exec sp_TedarikciBakiyesi " + lueMusteriler.EditValue.ToString() + ",0";
            else if (cbHareketTipi.SelectedIndex == 3)//Personel listesi
                sql = "Select 0 as Bakiye ";// +lueMusteriler.EditValue.ToString() + ",0";

            DataTable dt = DB.GetData(sql);

            if (dt.Rows.Count == 0)
            {
                ceBakiye.Text = "0,00";
            }
            else
            {
                ceBakiye.Value = decimal.Parse(dt.Rows[0][0].ToString());
            }
        }

        private void lueMusteriler_EditValueChanged(object sender, EventArgs e)
        {
              MusteriBorcu();

              if (cbHareketTipi.SelectedIndex == 0 && lueMusteriler.EditValue != null)
              {
                  DataTable dt = DB.GetData("select GelirOlarakisle,GiderOlarakisle from KasaGirisCikisTurleri with(nolock) where pkKasaGirisCikisTurleri=" +
                      lueMusteriler.EditValue.ToString());
                  if (dt.Rows.Count > 0)
                  {
                      if (dt.Rows[0]["GelirOlarakisle"].ToString() == "True")
                          cbGelirMi.Checked = true;
                      else
                          cbGelirMi.Checked = false;

                      if (dt.Rows[0]["GiderOlarakisle"].ToString() == "True")
                          ceGiderMi.Checked = true;
                      else
                          ceGiderMi.Checked = false;
                  }
                pkFirma.Text = lueMusteriler.EditValue.ToString();
            }
              else
               {
                pkFirma.Text = "0";
               }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            cbHareketTipi.SelectedIndex = 0;
            islemtarihi.EditValue = DateTime.Today;
            tEaciklama.Text = "";
            ceTutarNakit.Value = 0;
            ceTutarKredi.Value = 0;
        }

        private void islemtarihi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(cbOdemeSekli.SelectedIndex==0) ceTutarNakit.Focus();
                if (cbOdemeSekli.SelectedIndex == 1) ceTutarKredi.Focus();
                if (cbOdemeSekli.SelectedIndex == 2) lueCekler.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (this.Tag.ToString() == "1")
              // ceTutarNakit.EditValue=ceBakiye.EditValue;
            //if (this.Tag.ToString() == "2")
            //{
                ceTutarNakit.EditValue = ceBakiye.EditValue;
            if(ceTutarNakit.Value<0)
                ceTutarNakit.Value = ceBakiye.Value*-1;
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ceTutarKredi.EditValue = ceBakiye.EditValue;

            if (ceTutarKredi.Value < 0)
                ceTutarKredi.Value = ceBakiye.Value * -1;
        }

        private void ceTutarNakit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.Focus();
        }

        private void ceTutarKredi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.Focus();
        }

        private void ceBakiye_EditValueChanged(object sender, EventArgs e)
        {
            if (ceBakiye.Value > 0) ceBakiye.BackColor = System.Drawing.Color.Red;
            else if (ceBakiye.Value < 0) ceBakiye.BackColor = System.Drawing.Color.Blue;
            else if (ceBakiye.Value == 0) ceBakiye.BackColor = System.Drawing.Color.White;
            
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
                lueBanka.ItemIndex = 0;
            //else if (xtraTabControl1.SelectedTabPageIndex == 2)
            //    lueCekler.Properties.DataSource = DB.GetData("select * from Cekler where fkTedarikci is null");

        }
        
        void Yazdir(bool dizayn)
        {
            XtraReport rapor = new XtraReport();
            
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string pkKasaHareket = "0",fkFirma="0";
                System.Data.DataSet ds = new DataSet("Test");
                if (gridView1.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    pkKasaHareket = dr["pkKasaHareket"].ToString();
                    fkFirma = dr["fkFirma"].ToString();
                    if (fkFirma == "") fkFirma = "0";
                }
                string sql = "select * from KasaHareket with(nolock) where pkKasaHareket=" +pkKasaHareket;
                //rapor.DataSource = DB.GetData(sql);

                DataTable FisDetay = DB.GetData(sql);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Makbuz.repx");
                rapor.Name = "Makbuz";
                rapor.Report.Name = "Makbuz";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
            {
                if (cbYazdir.Checked)
                    rapor.Print();
                else
                    rapor.ShowPreview();
            }
        }

        void YazdirTaksit(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string pkKasaHareket = "0", fkFirma = "0";

                System.Data.DataSet ds = new DataSet("Test");
                if (gridView1.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    pkKasaHareket = dr["pkKasaHareket"].ToString();
                    fkFirma = dr["fkFirma"].ToString();
                    if (fkFirma == "") fkFirma = "0";
                }
               
                //Taksitler
                string sql = @"select *,dbo.fnc_ParayiYaziyaCevir(Odenecek,2) as rakamoku,Odenecek-Odenen as Kalan  
                            from Taksit T with(nolock)
                   left join Taksitler TL with(nolock) on T.taksit_id=TL.taksit_id
                   where Kaydet=0";
                //if(checkEdit1.Checked)
                sql = sql + " and Odenecek<>Odenen";

                sql = sql + " and T.fkFirma=" +  fkFirma;

                DataTable Taksitler = DB.GetData(sql);
                Taksitler.TableName = "Taksitler";
                ds.Tables.Add(Taksitler);
                //
                //Fiş Detay
                sql = "select * from KasaHareket with(nolock) where pkKasaHareket=" + pkKasaHareket;
                DataTable FisDetay = DB.GetData(sql);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                //
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\MakbuzTaksitler.repx");
                rapor.Name = "MakbuzTaksitler";
                rapor.Report.Name = "MakbuzTaksitler";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
            {
                if (cbYazdir.Checked)
                    rapor.Print();
                else
                    rapor.ShowPreview();
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            frmKasaGirisCikisTurleri fKasaGirisCikisTurleri = new frmKasaGirisCikisTurleri("0");
            fKasaGirisCikisTurleri.Tag = this.Tag;
            fKasaGirisCikisTurleri.ShowDialog();
            KasaGirisCikisTurleri();
        }

        private void VadeTarihi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.Focus();
        }

        private void btnKasaTuruList_Click(object sender, EventArgs e)
        {
            switch (cbHareketTipi.SelectedIndex)
            {
                case 0:
                    {
                        frmKasaGirisCikisTurListesi KasaGirisCikisTurListesi = new frmKasaGirisCikisTurListesi();
                        KasaGirisCikisTurListesi.Tag = this.Tag;
                        KasaGirisCikisTurListesi.ShowDialog();
                        KasaGirisCikisTurleri();
                        break;
                    }
                case 1:
                    {
                        string fkFirma = "1";
                        frmMusteriAra MusteriAra = new frmMusteriAra();
                        MusteriAra.ShowDialog();
                        fkFirma = MusteriAra.fkFirma.Tag.ToString();
                        lueMusteriler.EditValue = int.Parse(fkFirma);
                        MusteriAra.Dispose();
                        break;
                    }
                case 2:
                    {
                        string fkFirma = "1";
                        frmTedarikciAra TedarikciAra = new frmTedarikciAra();
                        TedarikciAra.ShowDialog();
                        fkFirma = TedarikciAra.fkFirma.Tag.ToString();
                        lueMusteriler.EditValue = int.Parse(fkFirma);
                        TedarikciAra.Dispose();
                        break;
                    }
                //default:
            }
         }

        private void düzeltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tsmHareketEdit.Enabled==false) return;

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();
            KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();
            KasaHareketDuzelt.Tag = this.Tag;
            KasaHareketDuzelt.ShowDialog();

            KasaHareketleriGetir();
        }

        void KasaBakiye()
        {
            string sql = @"SELECT Kasalar.PkKasalar, Kasalar.KasaAdi,  isnull(sum(KasaHareket.Borc),0) as Borc, isnull(sum(KasaHareket.Alacak),0) as Alacak,
            isnull(sum(KasaHareket.Borc-KasaHareket.Alacak),0) as Bakiye
            FROM Kasalar LEFT OUTER JOIN (select * from KasaHareket where AktifHesap=1) KasaHareket  ON Kasalar.PkKasalar = KasaHareket.fkKasalar
            where Kasalar.PkKasalar=1
            Group by Kasalar.PkKasalar, Kasalar.KasaAdi";
            DataTable dt = DB.GetData(sql);
            if (dt.Rows.Count == 0) return;
            //cEBorc.Value = decimal.Parse(dt.Rows[0]["Borc"].ToString());
            //ceAlacak.Value = decimal.Parse(dt.Rows[0]["Alacak"].ToString());
            ceKasadakiPara.Value = decimal.Parse(dt.Rows[0]["Bakiye"].ToString());
        }

        private void btnCekCikisi_Click(object sender, EventArgs e)
        {
            string firma_id="0";
            if (lueMusteriler.EditValue != null)
                firma_id = "0";//lueMusteriler.EditValue.ToString();

            frmCekGirisi CekGirisi = new frmCekGirisi(firma_id);
            if (lueCekler.EditValue ==null || lueCekler.EditValue.ToString() == "")
                CekGirisi.pkCek.Text = "0";
            else
                CekGirisi.pkCek.Text = lueCekler.EditValue.ToString();
            //CekGirisi.tutar.EditValue = ceAcikHesap.EditValue;
            CekGirisi.pkFirma.Text = firma_id;
            CekGirisi.ShowDialog();

            comboBoxEdit3_SelectedIndexChanged(sender, e);

            lueCekler.ItemIndex = 0;
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmUcGoster SatisGoster = new frmUcGoster(3, "0");
            SatisGoster.ShowDialog();

            comboBoxEdit3_SelectedIndexChanged(sender, e);

            lueCekler.ItemIndex = 0;
        }

        private void btnKasaHareket_Click(object sender, EventArgs e)
        {
            frmKasaHareketleri KasaHareketleri = new frmKasaHareketleri();
            KasaHareketleri.ShowDialog();

            //Close();
        }

        private void sorguTarihAraligi(int g1, int m1, int y1, int h1, int min1, int sec1, Boolean p1,
       int g2, int m2, int y2, int h2, int min2, int sec2, Boolean p2)
        {

            DateTime d1 = DateTime.Now;

            d1 = d1.AddDays(g1);
            d1 = d1.AddMonths(m1);
            d1 = d1.AddYears(y1);
            d1 = d1.AddHours(h1);
            d1 = d1.AddMinutes(min1);
            ilktarih.DateTime = d1.AddSeconds(sec1);


            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih.DateTime = d2.AddSeconds(sec2);

        }
        private DateTime getStartOfWeek(bool useSunday)
        {
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;

            if (!useSunday)
                dayOfWeek--;

            if (dayOfWeek < 0)
            {// day of week is Sunday and we want to use Monday as the start of the week
                // Sunday is now the seventh day of the week
                dayOfWeek = 6;
            }

            return now.AddDays(-1 * (double)dayOfWeek);
        }

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "g";
            sontarih.Properties.EditMask = "g";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "g";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "g";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// Bu gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            if (cbTarihAraligi.SelectedIndex == 1)// dün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 2)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 3)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 6)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);




            }
            //else if (cbTarihAraligi.SelectedIndex ==6) // Bu Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
            //                      0, 0, 0, 0, 0, 0, false);

            //}
            //else if (cbTarihAraligi.SelectedIndex == 7) // Geçen Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), -1, 0, 0, 0, false,
            //                    (-DateTime.Now.Day), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false);

            //}
            else if (cbTarihAraligi.SelectedIndex == 6)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            string sql = "";
            if (this.Tag.ToString() == "1")
            {
                sql = @"SELECT kh.pkKasaHareket, kh.Tarih, kh.Aciklama, kh.fkFirma, kh.fkTedarikciler, kh.OdemeSekli,kh.Alacak, kh.Borc, 
                      Bankalar.BankaAdi,kh.AktifHesap
                      ,case 
                      when kh.fkFirma >0 then 'Müşteri Ödemesi'
                      when kh.fkTedarikciler>0 then 'Tedarikçi Ödemesi'
                      else 'Diğer' end HareketTuru,
                      case 
                      when kh.fkFirma >0 then f.Firmaadi
                      when kh.fkTedarikciler>0 then t.Firmaadi
                      else tur.Aciklama end Adi,fkCek,isnull(fkTaksitler,0) as fkTaksitler,kh.MakbuzNo,kh.fkKullanicilar
                    FROM  KasaHareket  kh with(nolock)
                    LEFT JOIN Bankalar  with(nolock) ON kh.fkBankalar = Bankalar.PkBankalar 
                    LEFT JOIN Firmalar f with(nolock) ON kh.fkFirma = f.pkFirma 
                    LEFT JOIN Tedarikciler t with(nolock) ON kh.fkTedarikciler = t.pkTedarikciler
                    LEFT JOIN KasaGirisCikisTurleri tur with(nolock) on tur.pkKasaGirisCikisTurleri=kh.fkKasaGirisCikisTurleri
                    WHERE fkSatislar is null and Tarih Between @ilktar and @sontar";
                if (!ceTumu.Checked)
                    sql = sql + " and kh.fkKullanicilar=" + DB.fkKullanicilar;
                sql = sql + " ORDER BY kh.pkKasaHareket DESC";
            }
            if (this.Tag.ToString() == "2")
            {
                sql = @"SELECT kh.pkKasaHareket, kh.Tarih, kh.Aciklama, kh.fkFirma, kh.fkTedarikciler, kh.OdemeSekli,kh.Alacak, kh.Borc, 
                      Bankalar.BankaAdi,kh.AktifHesap
                      ,case 
                      when kh.fkFirma >0 then 'Müşteri Ödemesi'
                      when kh.fkTedarikciler>0 then 'Tedarikçi Ödemesi'
                      else 'Diğer' end HareketTuru,
                      case 
                      when kh.fkFirma >0 then f.Firmaadi
                      when kh.fkTedarikciler>0 then t.Firmaadi
                      else tur.Aciklama end Adi,fkCek,isnull(fkTaksitler,0) as fkTaksitler,kh.MakbuzNo,kh.fkKullanicilar
                    FROM  KasaHareket kh with(nolock)  
                    LEFT JOIN Bankalar with(nolock) ON kh.fkBankalar = Bankalar.PkBankalar 
                    LEFT JOIN Firmalar f with(nolock)  ON kh.fkFirma = f.pkFirma 
                    LEFT JOIN Tedarikciler t with(nolock) ON kh.fkTedarikciler = t.pkTedarikciler
                    LEFT JOIN KasaGirisCikisTurleri tur with(nolock) on tur.pkKasaGirisCikisTurleri=kh.fkKasaGirisCikisTurleri
                    WHERE fkSatislar is null and kh.Alacak> 0 and Tarih Between @ilktar and @sontar";
                if (!ceTumu.Checked)
                    sql = sql + " and kh.fkKullanicilar=" + DB.fkKullanicilar;
                sql = sql + " ORDER BY kh.pkKasaHareket DESC";
            }
            //string sql = "exec sp_KasaHareketleri @gruplu,@ilktar,@sontar";
            ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@gruplu", gruplu));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            gridControl1.DataSource = DB.GetData(sql, list);
        }

        void makbuzbuttonaktifpasif()
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkFirma = dr["fkFirma"].ToString();
            string fkTaksitler = dr["fkTaksitler"].ToString();
            if (fkFirma == "" || fkFirma == "0")
                btnMakbuz.Enabled = false;
            else
            {
                btnMakbuz.Enabled = true;
                if (fkTaksitler == "0" || fkTaksitler == "")
                    btnTaksitMakbuz.Enabled = false;
                else
                    btnTaksitMakbuz.Enabled = true;
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            makbuzbuttonaktifpasif();
        }

        private void cbOdemeSekli_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ceTutarNakit.Focus();
            }
        }

        private void cbTarihAraligi_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void cbTarihPanel_CheckedChanged(object sender, EventArgs e)
        {
            panel7.Visible = cbTarihPanel.Checked;
        }

        private void btnMakbuz_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

                Yazdir(false);
        }

        private void btnTaksitMakbuz_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

                YazdirTaksit(false);
        }

        private void tahsilatMakbuzuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

                Yazdir(true);
        }

        private void taksitÖdemesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

                YazdirTaksit(true);
        }

        private void tedarikçiHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmTedarikcilerHareketleri CariHareketMusteri = new frmTedarikcilerHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkTedarikciler"].ToString();
            CariHareketMusteri.ShowDialog();
        }

        private void personelHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmPersonelHareketleri CariHareketMusteri = new frmPersonelHareketleri(dr["fkPersoneller"].ToString());
            CariHareketMusteri.musteriadi.Tag = dr["fkPersoneller"].ToString();
            CariHareketMusteri.ShowDialog();
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (lueTaksitler.EditValue == null) return;

            DataTable dtTaksit = DB.GetData("select pkTaksitler,Tarih,convert(money,Odenecek) as Odenecek  from Taksitler with(nolock) where pkTaksitler=" + lueTaksitler.EditValue.ToString());
            if (dtTaksit.Rows.Count > 0)
                tEaciklama.Text = dtTaksit.Rows[0]["Tarih"].ToString() + " Tarihli Taksit Ödemesi-" + dtTaksit.Rows[0]["Odenecek"].ToString();
        }

        private void lueCekler_EditValueChanged(object sender, EventArgs e)
        {
            if (lueCekler.EditValue == null) return;

            DataTable dtCek = DB.GetData("select pkCek,CONVERT(VARCHAR(10), Vade, 104) as  Vade,convert(money,Tutar) as  Tutar from Cekler with(nolock) where pkCek=" + lueCekler.EditValue.ToString());
            if (dtCek.Rows.Count > 0)
            {
                if(dtCek.Rows[0]["Vade"].ToString()!="")
                    tEaciklama.Text = dtCek.Rows[0]["Vade"].ToString() + " Tarihli Çek Girişi-"
                    + dtCek.Rows[0]["Tutar"].ToString();
            }
        }

        private void tEaciklama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.Focus();
        }

        private void pkFirma_TextChanged(object sender, EventArgs e)
        {
            DataTable dt =
            DB.GetData(@"select sum(Odenecek-Odenen) as Kalan from Taksit t with(nolock)
left join Taksitler tl with(nolock) on tl.taksit_id = t.taksit_id
where t.fkFirma = " + pkFirma.Text);
            if(dt.Rows.Count==0)
            {
                ceTaksitBakiyesi.Value = 0;
            }
            else
            {
                decimal kalan = 0;
                decimal.TryParse(dt.Rows[0]["kalan"].ToString(), out kalan);

                ceTaksitBakiyesi.Value = kalan;
            }

        }

        private void cbTarihAraligi_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// Bu gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            if (cbTarihAraligi.SelectedIndex == 1)// dün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 2)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 3)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day + 1), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 6)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            //else if (cbTarihAraligi.SelectedIndex ==6) // Bu Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
            //                      0, 0, 0, 0, 0, 0, false);

            //}
            //else if (cbTarihAraligi.SelectedIndex == 7) // Geçen Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), -1, 0, 0, 0, false,
            //                    (-DateTime.Now.Day), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false);

            //}
            else if (cbTarihAraligi.SelectedIndex == 6)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            if (this.Tag.ToString() == "1")
                yi.GridTasarimKaydet(gridView1,"KasaGirisiGrid");
            else
                yi.GridTasarimKaydet(gridView1,"KasaCikisiGrid");
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            if (this.Tag.ToString() == "1")
                yi.GridTasarimSil("KasaGirisiGrid");
            else
                yi.GridTasarimSil("KasaCikisiGrid");
        }

        private void sütunListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void ceTumu_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTarihPanel.Checked)
                KasaHareketleriGetir();
            else
                btnListele_Click(sender, e);
        }
    }
}
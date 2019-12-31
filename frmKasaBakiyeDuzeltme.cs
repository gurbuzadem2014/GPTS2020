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

namespace GPTS
{
    public partial class frmKasaBakiyeDuzeltme : DevExpress.XtraEditors.XtraForm
    {
        public static int _fkKasaid;
        public frmKasaBakiyeDuzeltme(int fkKasaid)
        {
            InitializeComponent();
            _fkKasaid = fkKasaid;

            if (_fkKasaid == 0)
                _fkKasaid = Degerler.fkKasalar;

            
        }

        private void frmDevirBakiye_Load(object sender, EventArgs e)
        {
            borclandigitarih.DateTime = DateTime.Now;
            //BakiyeGetir();
            Kasalar();

            DevirBakiyeler();

            KasadakiParaGetir();
            //Kasadakipara();
            ceKasadakiParaYeni.Focus();
        }

        private void Kasalar()
        {
            lueKasa.Properties.DataSource = DB.GetData("SELECT * FROM Kasalar with(nolock) where Aktif=1");

            if (_fkKasaid == 0)
                _fkKasaid = Degerler.fkKasalar;

            lueKasa.EditValue = _fkKasaid;
        }

        void KasadakiParaGetir()
        {
            decimal KasadakiPara = 0;
            DataTable dtKasa =
            DB.GetData("select isnull(sum(borc),0)-isnull(sum(Alacak),0) as KasaBakiye from KasaHareket with(nolock) where fkKasalar=" +
                lueKasa.EditValue.ToString());

            decimal.TryParse(dtKasa.Rows[0][0].ToString(), out KasadakiPara);

            ceKasadakiParaMevcut.Value = KasadakiPara;
        }

        void DevirBakiyeler()
        {
            gridControl1.DataSource=
            DB.GetData(@"select * from KasaHareket with(nolock)
                where OdemeSekli='Kasa Bakiye Düzeltme' and fkKasalar=" + lueKasa.EditValue.ToString()+
                " and (fkSube is null or fkSube="+Degerler.fkSube+")");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (borclandigitarih.EditValue==null)
            {
                MessageBox.Show("Borçlandığı Tarih Alanı Boş Olamaz");
                borclandigitarih.Focus();
                return;
            }
            //if (pkKasalar.Text== "0")
           //{
            //   MessageBox.Show("Lütfen Kasa Seçiniz");
                //mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
                //mesaj.label1.Text = "Lütfen Müşteri Seçiniz";
                //mesaj.Show();
             //   return;
           // }
            //else
            //{
                DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Değişiklikleri Kaydetmek istediğinize, Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;
                string yeni_kasa_hareket_id = "0";

                #region bakiye sıfırlama
                if (ceBakiyeKaydedilecek.Value != 0)
                {
                    string sql = @"INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,"+
                    "fkFirma,fkTedarikciler,OdemeSekli,Tutar,BilgisayarAdi,aktarildi,fkSube)" +
                    " values(@fkKasalar,@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,"+
                        "@fkFirma,0,'Kasa Bakiye Düzeltme',@Tutar,@BilgisayarAdi,@aktarildi,@fkSube) select IDENT_CURRENT('KasaHareket')";
                    
                    ArrayList list0 = new ArrayList();
                    list0.Add(new SqlParameter("@fkKasalar", lueKasa.EditValue.ToString()));
                    if (ceBakiyeKaydedilecek.Value > 0)
                    {
                        list0.Add(new SqlParameter("@Alacak", ceBakiyeKaydedilecek.Text.Replace(",", ".").Replace("-", "")));
                        list0.Add(new SqlParameter("@Borc", "0"));
                    }
                    else
                    {
                        list0.Add(new SqlParameter("@Alacak", "0"));
                        list0.Add(new SqlParameter("@Borc", ceBakiyeKaydedilecek.Text.Replace(",", ".").Replace("-", "")));
                    }
                    list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
                    list0.Add(new SqlParameter("@Aciklama", "Bakiye Sıfırlandı."));
                    list0.Add(new SqlParameter("@donem", borclandigitarih.DateTime.Month));
                    list0.Add(new SqlParameter("@yil", borclandigitarih.DateTime.Year));
                    list0.Add(new SqlParameter("@fkFirma", "0"));
                    list0.Add(new SqlParameter("@AktifHesap", cbAktifHesap.Checked));
                    list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                    list0.Add(new SqlParameter("@Tutar", ceKasadakiParaMevcut.Value.ToString().Replace(",",".")));

                    list0.Add(new SqlParameter("@Tarih", borclandigitarih.DateTime));
                    list0.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                    list0.Add(new SqlParameter("@aktarildi",Degerler.AracdaSatis));
                    list0.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                yeni_kasa_hareket_id = DB.ExecuteScalarSQL(sql, list0);
                    if (yeni_kasa_hareket_id.Substring(0, 1) == "H")
                    {
                        MessageBox.Show("Hata Oluştu Tekrar deneyiniz" + yeni_kasa_hareket_id);
                        return;
                    }
                }
                #endregion

                #region borç sıfırlama
                if (ceKasadakiParaYeni.Value != 0)
                {
                    string sql2 = @"INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,"+
                    "fkFirma,fkTedarikciler,OdemeSekli,Tutar,BilgisayarAdi,aktarildi,fkSube)" +
                    " values(@fkKasalar,@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap"+
                    ",@fkFirma,0,'Kasa Bakiye Düzeltme',@Tutar,@BilgisayarAdi,@aktarildi,@fkSube)";

                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkKasalar", lueKasa.EditValue.ToString()));
                    if (ceKasadakiParaYeni.Value > 0)
                    {
                        list.Add(new SqlParameter("@Borc", ceKasadakiParaYeni.Text.Replace(",", ".").Replace("-", "")));
                        list.Add(new SqlParameter("@Alacak", "0"));
                    }
                    else
                    {
                        list.Add(new SqlParameter("@Borc", "0"));
                        list.Add(new SqlParameter("@Alacak", ceKasadakiParaYeni.Text.Replace(",", ".").Replace("-", "")));
                    }
                    list.Add(new SqlParameter("@Tipi", int.Parse("1")));
                    list.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
                    list.Add(new SqlParameter("@donem", borclandigitarih.DateTime.Month));
                    list.Add(new SqlParameter("@yil", borclandigitarih.DateTime.Year));
                    list.Add(new SqlParameter("@fkFirma", "0"));
                    list.Add(new SqlParameter("@AktifHesap", cbAktifHesap.Checked));
                    list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                    list.Add(new SqlParameter("@Tutar", ceKasadakiParaMevcut.Value.ToString().Replace(",", ".")));

                    list.Add(new SqlParameter("@Tarih", borclandigitarih.DateTime));
                    list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                list.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
                list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                string sonuc2 = DB.ExecuteSQL(sql2, list);
                    if (sonuc2 != "0")
                    {
                        //ceBakiye.Value = 0;
                       // MessageBox.Show("İşlem Başarılı");
                    }
                }
                #endregion

                #region KasaGunluk Ekle
            //    ArrayList list2 = new ArrayList();
            //    list2.Add(new SqlParameter("fkKasalar", lueKasa.EditValue.ToString()));
            //    list2.Add(new SqlParameter("fkKullanici", DB.fkKullanicilar));
            //    list2.Add(new SqlParameter("Tarih", borclandigitarih.DateTime));
                
            //    decimal cikar = 0;
            //    if(ceKasadakiParaYeni.Value<0)
            //    cikar = ceKasadakiParaMevcut.Value+ceKasadakiParaYeni.Value;
            //    else
            //    cikar = ceKasadakiParaMevcut.Value-ceKasadakiParaYeni.Value;

            //    list2.Add(new SqlParameter("KasadakiPara", cikar.ToString().Replace(",", ".")));
            //    list2.Add(new SqlParameter("OlmasiGereken", ceKasadakiParaMevcut.Value.ToString().Replace(",", ".")));
            //    list2.Add(new SqlParameter("Aciklama", tEaciklama.Text));
            //    //list2.Add(new SqlParameter("Aciklama", tEaciklama.Text));
            //    list2.Add(new SqlParameter("fkKasaHareket", yeni_kasa_hareket_id));
                
            //    DataTable dt = DB.GetData("select * from KasaGunluk with(nolock) where fkKasalar=1 and fkKullanici=" +
            //        DB.fkKullanicilar + " and Tarih='" + borclandigitarih.DateTime.ToString("yyyy-MM-dd HH:mm") + "'");
            //    if (dt.Rows.Count == 0)
            //    {
            //        DB.ExecuteSQL(@"insert into KasaGunluk (fkKasalar,fkKullanici,Tarih,KasadakiPara,OlmasiGereken,Aciklama,fkKasaHareket) 
            //values(@fkKasalar,@fkKullanici,@Tarih,@KasadakiPara,@OlmasiGereken,@Aciklama,@fkKasaHareket)", list2);
            //    }
            //    else
            //    {
            //        list2.Add(new SqlParameter("pkKasaGunluk", dt.Rows[0]["pkKasaGunluk"]));

            //        DB.ExecuteSQL(@"update KasaGunluk set KasadakiPara=@KasadakiPara,OlmasiGereken=@OlmasiGereken,Aciklama=@Aciklama,fkKasaHareket=@fkKasaHareket
            //        where pkKasaGunluk=@pkKasaGunluk", list2);
            //    }

                #endregion

                Temizle();
           // }
           
            simpleButton2.Enabled = false;
            frmMesajBox mesajj = new frmMesajBox(200);
            mesajj.label1.BackColor = System.Drawing.Color.GreenYellow;
            mesajj.label1.Text = "Bilgiler Kaydedildi.";
            mesajj.Show();
            Close();
        }
        void Temizle()
        {
            ceKasadakiParaYeni.Value = 0;
            tEaciklama.Text = "Kasa Bakiye Düzeltme";
            ceBakiyeKaydedilecek.Value = 0;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Bakiyesi Sıfırlanacak, Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (secim == DialogResult.No) return;
            ceBakiyeKaydedilecek.Value = ceKasadakiParaMevcut.Value;
            ceKasadakiParaMevcut.Value = 0;
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            simpleButton2.Enabled = true;
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            simpleButton2.Enabled = true;
        }

        private void ceDevirTutar_EditValueChanged(object sender, EventArgs e)
        {
            simpleButton2.Enabled = true;
        }

        private void frmDevirBakisiSatisKaydi_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            simpleButton4_Click(sender,e);
            if (e.KeyCode == Keys.F9)
                simpleButton2_Click(sender, e);
        }

        private void ceDevirTutar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                simpleButton2.Focus();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + dr["pkKasaHareket"].ToString();
            DB.ExecuteSQL(sql);
            DevirBakiyeler();
        }

        private void düzeltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();
            KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();
            //KasaHareketDuzelt.Tag = this.Tag;
            KasaHareketDuzelt.ShowDialog();

            DevirBakiyeler();
        }

        private void lueKasa_EditValueChanged(object sender, EventArgs e)
        {
            KasadakiParaGetir();
        }
    }
}
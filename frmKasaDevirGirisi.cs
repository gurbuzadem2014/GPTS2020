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
    public partial class frmKasaDevirGirisi : DevExpress.XtraEditors.XtraForm
    {
        public frmKasaDevirGirisi()
        {
            InitializeComponent();
        }
        void Kasadakipara()
        {

            decimal KasadakiPara = 0;
            decimal.TryParse(DB.GetData("select isnull(sum(borc),0)-isnull(sum(Alacak),0) as KasaBakiye from KasaHareket with(nolock) where fkKasalar is not null").Rows[0][0].ToString(),
                out KasadakiPara);
            ceKasadakiParaMevcut.Value = KasadakiPara;
        }
        void DevirBakiyelerGetir()
        {
            gridControl1.DataSource=
            DB.GetData(@"select top 10 * from KasaHareket with(nolock)  
            where OdemeSekli='Kasa Devir Girişi' order by pkKasaHareket desc");// and fkKasalar=" + pkKasalar.Text);
        }

        private void frmDevirBakiye_Load(object sender, EventArgs e)
        {
            borclandigitarih.DateTime=DateTime.Now;

            Kasalar();

            DevirBakiyelerGetir();

            Kasadakipara();
            //simpleButton3_Click(sender,e);//sıfırla
            ceKasadakiParaYeni.Focus();
        }

        private void Kasalar()
        {
            lueKasa.Properties.DataSource = DB.GetData("SELECT * FROM Kasalar with(nolock) where Aktif=1");
            lueKasa.EditValue = Degerler.fkKasalar;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (borclandigitarih.EditValue==null)
            {
                MessageBox.Show("Borçlandığı Tarih Alanı Boş Olamaz");
                borclandigitarih.Focus();
                return;
            }

            //if (pkKasalar.Text == "0")
            //{
              //  MessageBox.Show("Lütfen Kasa Seçiniz");
                //mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
                //mesaj.label1.Text = "Lütfen Müşteri Seçiniz";
                //mesaj.Show();
               // return;
            //}

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Değişiklikleri Kaydetmek istediğinize, Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            string yeni_kasahareket_id = "0";
            #region bakiye sıfırlama
            if (ceKasadakiParaMevcut.Value != 0)
            {
                string sql = @"INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar)
                values(@fkKasalar,@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Kasa Devir Girişi',@Tutar) SELECT IDENT_CURRENT('KasaHareket')";

                ArrayList list0 = new ArrayList();
                list0.Add(new SqlParameter("@fkKasalar", lueKasa.EditValue.ToString()));

                //if (ceBakiyeKaydedilecek.Value > 0)
                // {
                list0.Add(new SqlParameter("@Alacak", ceKasadakiParaMevcut.Value.ToString().Replace(",", ".").Replace("-", "")));
                //decimal fark = ceKasadakiParaMevcut.Value - ceKasadakiParaYeni.Value;
                list0.Add(new SqlParameter("@Borc", ceKasadakiParaYeni.Value.ToString().Replace(",", ".")));
                //}
                //else
                //{
                //   list0.Add(new SqlParameter("@Alacak", "0"));
                //  list0.Add(new SqlParameter("@Borc", ceBakiyeKaydedilecek.Text.Replace(",", ".").Replace("-", "")));
                //}
                list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
                list0.Add(new SqlParameter("@Aciklama", "Kasadaki Para"));
                list0.Add(new SqlParameter("@donem", borclandigitarih.DateTime.Month));
                list0.Add(new SqlParameter("@yil", borclandigitarih.DateTime.Year));
                list0.Add(new SqlParameter("@fkFirma", "0"));
                list0.Add(new SqlParameter("@AktifHesap", cbAktifHesap.Checked));
                list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                list0.Add(new SqlParameter("@Tutar", ceBakiyeKaydedilecek.Value.ToString().Replace(",", ".")));

                list0.Add(new SqlParameter("@Tarih", borclandigitarih.DateTime));

                yeni_kasahareket_id = DB.ExecuteScalarSQL(sql, list0);

                if (yeni_kasahareket_id.Substring(0, 1) == "H")
                {
                    //ceBakiye.Value = 0;
                    MessageBox.Show("Hata Oluştu :" + yeni_kasahareket_id);
                    return;
                }
            }
            #endregion

            #region borç sıfırlama
//                if (ceKasadakiParaYeni.Value != 0)
//                {
//                    string sql2 = @"INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar)
//             values(@fkKasalar,@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Kasa Devir Girişi',@Tutar)";
//                    ArrayList list = new ArrayList();
//                    list.Add(new SqlParameter("@fkKasalar", pkKasalar.Text));
//                    if (ceKasadakiParaYeni.Value > 0)
//                    {
//                        list.Add(new SqlParameter("@Borc", ceKasadakiParaYeni.Text.Replace(",", ".").Replace("-", "")));
//                        list.Add(new SqlParameter("@Alacak", "0"));
//                    }
//                    else
//                    {
//                        list.Add(new SqlParameter("@Borc", "0"));
//                        list.Add(new SqlParameter("@Alacak", ceKasadakiParaYeni.Text.Replace(",", ".").Replace("-", "")));
//                    }
//                    list.Add(new SqlParameter("@Tipi", int.Parse("1")));
//                    list.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
//                    list.Add(new SqlParameter("@donem", borclandigitarih.DateTime.Month));
//                    list.Add(new SqlParameter("@yil", borclandigitarih.DateTime.Year));
//                    list.Add(new SqlParameter("@fkFirma", "0"));
//                    list.Add(new SqlParameter("@AktifHesap", cbAktifHesap.Checked));
//                    list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
//                    list.Add(new SqlParameter("@Tutar", ceBakiyeKaydedilecek.Value.ToString().Replace(",", ".")));

//                    list.Add(new SqlParameter("@Tarih", borclandigitarih.DateTime));

//                    string sonuc2 = DB.ExecuteSQL(sql2, list);
//                    if (sonuc2 != "0")
//                    {
//                        //ceBakiye.Value = 0;
//                       // MessageBox.Show("İşlem Başarılı");
//                    }
//                }
//            }
            #endregion

            simpleButton2.Enabled = false;

            #region KasaGunluk Ekle
            ArrayList list2 = new ArrayList();
            list2.Add(new SqlParameter("@fkKasalar", lueKasa.EditValue));
            list2.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list2.Add(new SqlParameter("@Tarih", borclandigitarih.DateTime));
            list2.Add(new SqlParameter("@KasadakiPara", ceKasadakiParaYeni.Value.ToString().Replace(",", ".")));
            list2.Add(new SqlParameter("@OlmasiGereken", ceKasadakiParaMevcut.Value.ToString().Replace(",", ".")));
            list2.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list2.Add(new SqlParameter("@fkKasaHareket", yeni_kasahareket_id));
            
            DataTable dt = DB.GetData("select * from KasaGunluk with(nolock) where fkKasalar=1 and fkKullanici=" +
                DB.fkKullanicilar + " and Tarih='" + borclandigitarih.DateTime.ToString("yyyy-MM-dd HH:mm") + "'");
            if (dt.Rows.Count == 0)
            {
                DB.ExecuteSQL(@"insert into KasaGunluk (fkKasalar,fkKullanici,Tarih,KasadakiPara,OlmasiGereken,Aciklama,fkKasaHareket) 
            values(@fkKasalar,@fkKullanici,@Tarih,@KasadakiPara,@OlmasiGereken,@Aciklama,@fkKasaHareket)", list2);
            }
            else
            {
                list2.Add(new SqlParameter("pkKasaGunluk", dt.Rows[0]["pkKasaGunluk"]));

                DB.ExecuteSQL(@"update KasaGunluk set KasadakiPara=@KasadakiPara,OlmasiGereken=@OlmasiGereken,Aciklama=@Aciklama,
                fkKasaHareket=@fkKasaHareket where pkKasaGunluk=@pkKasaGunluk", list2);
            }

            #endregion

            frmMesajBox mesajj = new frmMesajBox(200);
            mesajj.label1.BackColor = System.Drawing.Color.GreenYellow;
            mesajj.label1.Text = "Bilgiler Kaydedildi.";
            mesajj.Show();

            Temizle();

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
            ceBakiyeKaydedilecek.Value = ceKasadakiParaMevcut.Value;
            ceKasadakiParaMevcut.Value = 0;
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
            
            DevirBakiyelerGetir();
        }

        private void düzeltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();
            KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();
            KasaHareketDuzelt.ShowDialog();

            DevirBakiyelerGetir();
        }
    }
}
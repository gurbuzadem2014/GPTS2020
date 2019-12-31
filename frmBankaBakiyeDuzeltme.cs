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
    public partial class frmBankaBakiyeDuzeltme : DevExpress.XtraEditors.XtraForm
    {
        public frmBankaBakiyeDuzeltme()
        {
            InitializeComponent();
        }

        void DevirBakiyeler()
        {
            gridControl1.DataSource=
            DB.GetData(@"select * from KasaHareket
                where OdemeSekli='Banka Bakiye Düzeltme' and fkBankalar=" + pkKasalar.Text);
        }

        void Kasadakipara()
        {
            decimal KasadakiPara = 0;
            decimal.TryParse( DB.GetData("select isnull(sum(borc),0)-isnull(sum(Alacak),0) as KasaBakiye from KasaHareket").Rows[0][0].ToString(),
                out KasadakiPara);
            ceKasadakiParaMevcut.Value = KasadakiPara;
        }
        private void frmDevirBakiye_Load(object sender, EventArgs e)
        {
            borclandigitarih.DateTime=DateTime.Now;

            DevirBakiyeler();
            ceKasadakiParaYeni.Focus();
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (borclandigitarih.EditValue==null)
            {
                MessageBox.Show("Borçlandığı Tarih Alanı Boş Olamaz");
                borclandigitarih.Focus();
                return;
            }
            if (pkKasalar.Text== "0")
           {
               MessageBox.Show("Lütfen Banka Seçiniz");
                //mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
                //mesaj.label1.Text = "Lütfen Müşteri Seçiniz";
                //mesaj.Show();
                return;
            }
            else
            {
                DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Değişiklikleri Kaydetmek istediğinize, Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;
                //bakiye sıfırlama
                if (ceBakiyeKaydedilecek.Value != 0)
                {
                    string sql = @"INSERT INTO KasaHareket (fkBankalar,Tutar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkKullanicilar,OdemeSekli,fkSube)
             values(@fkBankalar,@Tutar,getdate(),3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkKullanicilar,'Banka Bakiye Düzeltme',@fkSube)";
                    ArrayList list0 = new ArrayList();
                    list0.Add(new SqlParameter("@fkBankalar", pkKasalar.Text));
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
                    list0.Add(new SqlParameter("@Tutar", ceBakiyeKaydedilecek.Text.Replace(",", ".")));
                    
                    list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
                    list0.Add(new SqlParameter("@Aciklama", "Bakiye Sıfırlandı."));
                    list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
                    list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
                    list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                    list0.Add(new SqlParameter("@AktifHesap", "1"));
                    list0.Add(new SqlParameter("@fkSube", Degerler.fkSube));
                    
                    string sonuc = DB.ExecuteSQL(sql, list0);
                    if (sonuc != "0")
                    {
                        //ceBakiye.Value = 0;
                       // MessageBox.Show("Hata Oluştu Tekrar deneyiniz");
                        return;
                    }
                }
                //borç sıfırlama
                if (ceKasadakiParaYeni.Value != 0)
                {
                    string sql2 = @"INSERT INTO KasaHareket (fkBankalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli)
             values(@fkBankalar,0,getdate(),3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Banka Bakiye Düzeltme')";
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkBankalar", pkKasalar.Text));
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
                    list.Add(new SqlParameter("@donem", DateTime.Now.Month));
                    list.Add(new SqlParameter("@yil", DateTime.Now.Year));
                    list.Add(new SqlParameter("@fkFirma", "0"));
                    list.Add(new SqlParameter("@AktifHesap", "1"));
                    //list.Add(new SqlParameter("@Tarih", borclandigitarih.DateTime));
                    string sonuc2 = DB.ExecuteSQL(sql2, list);
                    if (sonuc2 != "0")
                    {
                        //ceBakiye.Value = 0;
                       // MessageBox.Show("İşlem Başarılı");
                    }
                }
                Temizle();
            }
           
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
            tEaciklama.Text = "Banka Bakiye Düzeltme";
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
    }
}
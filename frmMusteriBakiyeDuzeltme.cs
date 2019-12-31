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
    public partial class frmMusteriBakiyeDuzeltme : DevExpress.XtraEditors.XtraForm
    {
        string fkFirma;
        public frmMusteriBakiyeDuzeltme(string pkFirma)
        {
            InitializeComponent();
            fkFirma = pkFirma;
        }
        void Getir()
        {
            DataTable dt = DB.GetData("select * From Firmalar with(nolock) where pkFirma=" + fkFirma);
            if (dt.Rows.Count == 0)
            {
                labelControl5.Text = "Bulunamadı.";
                ceDevirTutar.EditValue = 0;
            }
            else
            {
                labelControl5.Text = dt.Rows[0]["OzelKod"].ToString() + "-" +
                dt.Rows[0]["Firmaadi"].ToString();
                //ceDevirTutar.EditValue = dt.Rows[0]["Devir"].ToString();
            }
        }

        void BakiyeGetir()
        {
            DataTable dt = DB.GetData("exec sp_MusteriBakiyesi " + fkFirma + ",0");
            if (dt.Rows.Count == 0)
            {
                ceBakiye.Value = 0;
            }
            else
            {
                ceBakiye.Value = decimal.Parse(dt.Rows[0][0].ToString());
            }
            ceBakiye.Tag = ceBakiye.Value;
        }
        void DevirBakiyeler()
        {
            gridControl1.DataSource=
            DB.GetData(@"select * from KasaHareket with(nolock)
                        where OdemeSekli='Bakiye Düzeltme' and fkFirma=" + fkFirma);
        }

        private void frmDevirBakiye_Load(object sender, EventArgs e)
        {
            //Müşteri Devir Ekranı	1	15.1	15	1
            DataTable dtYetki  = DB.GetData(@"select * from ModullerYetki with(nolock)
            where Kod='15.1' and fkKullanicilar="+DB.fkKullanicilar);
            if (dtYetki.Rows.Count > 0)
            {
                string yetki= dtYetki.Rows[0]["Yetki"].ToString();
                if (yetki == "False")
                {
                    simpleButton2.Enabled = false;
                    simpleButton3.Enabled = false;
                    ceDevirTutar.Enabled = false;
                }
            }

            borclandigitarih.DateTime=DateTime.Now;

            Getir();

            BakiyeGetir();

            DevirBakiyeler();

            ceDevirTutar.Focus();
        }
        void musteriara()
        {
            //frmMusteriAra MusteriAra = new frmMusteriAra();
            //MusteriAra.ShowDialog();

            //ceDevirTutar.Value = 0;
            //tEaciklama.EditValue = null;
            //DB.PkFirma = int.Parse(MusteriAra.fkFirma.Tag.ToString());
            //Getir();
            //BakiyeGetir();
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (borclandigitarih.EditValue==null)
            {
                MessageBox.Show("Borçlandığı Tarih Alanı Boş Olamaz");
                borclandigitarih.Focus();
                return;
            }
            if (fkFirma == "0")
           {
               MessageBox.Show("Lütfen Müşteri Seçiniz");
                //mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
                //mesaj.label1.Text = "Lütfen Müşteri Seçiniz";
                //mesaj.Show();
                return;
            }
            else
            {
                string mesaj = "";
                if (ceBakiyeKaydedilecek.Value != 0)
                    mesaj = "Bakiye Sıfırlandı ve ";

                mesaj = mesaj + "Bakiye Eklenecek Eminmisiniz?";
                
                DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show(mesaj, DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;
                //bakiye sıfırlama
                if (ceBakiyeKaydedilecek.Value != 0)
                {
                    string sql = @"INSERT INTO KasaHareket (fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,aktarildi,fkSube)
                    values(@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Bakiye Düzeltme',@Tutar,@aktarildi,@fkSube)";

                    ArrayList list0 = new ArrayList();
                    list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

                    if (ceBakiyeKaydedilecek.Value < 0)
                    {
                        list0.Add(new SqlParameter("@Alacak", ceBakiyeKaydedilecek.Text.Replace(",", ".").Replace("-", "")));
                        list0.Add(new SqlParameter("@Borc", "0"));
                    }
                    else
                    {
                        list0.Add(new SqlParameter("@Alacak", "0"));
                        list0.Add(new SqlParameter("@Borc", ceBakiyeKaydedilecek.Text.Replace(",", ".")));
                    }
                    list0.Add(new SqlParameter("@Tutar", ceBakiye.Tag.ToString().Replace(",",".")));
                    
                    list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
                    list0.Add(new SqlParameter("@Aciklama", "Bakiye Sıfırlandı."));
                    list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
                    list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
                    list0.Add(new SqlParameter("@fkFirma", fkFirma));
                    list0.Add(new SqlParameter("@AktifHesap", cbAktifHesap.Checked));
                    list0.Add(new SqlParameter("@Tarih", borclandigitarih.DateTime));
                    list0.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
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
                if (ceDevirTutar.Value != 0)
                {
                    string sql2 = @"INSERT INTO KasaHareket (fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,aktarildi,fkSube)
                    values(@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Bakiye Düzeltme',@Tutar,1,@fkSube)";
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                    
                    if (ceDevirTutar.Value > 0)
                    {
                        list.Add(new SqlParameter("@Alacak", ceDevirTutar.Text.Replace(",", ".").Replace("-", "")));
                        list.Add(new SqlParameter("@Borc", "0"));
                    }
                    else
                    {
                        list.Add(new SqlParameter("@Alacak", "0"));
                        list.Add(new SqlParameter("@Borc", ceDevirTutar.Text.Replace(",", ".").Replace("-", "")));
                    }
                    decimal tutar = 0;
                    //decimal.TryParse(ceBakiye.Tag.ToString(), out tutar);
                   // tutar = tutar + ceDevirTutar.Value;
                    list.Add(new SqlParameter("@Tutar", tutar.ToString().Replace(",", ".")));

                    list.Add(new SqlParameter("@Tipi", int.Parse("1")));
                    list.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
                    list.Add(new SqlParameter("@donem", DateTime.Now.Month));
                    list.Add(new SqlParameter("@yil", DateTime.Now.Year));
                    list.Add(new SqlParameter("@fkFirma", fkFirma));
                    list.Add(new SqlParameter("@AktifHesap", "0"));
                    list.Add(new SqlParameter("@Tarih", borclandigitarih.DateTime));
                    list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                    string sonuc2 = DB.ExecuteSQL(sql2, list);
                    if (sonuc2 != "0")
                    {
                        //ceBakiye.Value = 0;
                       // MessageBox.Show("İşlem Başarılı");
                    }
                }
//                if (OdemeTarihi.EditValue != null)
//                {
//                    ArrayList list3 = new ArrayList();
//                    list3.Add(new SqlParameter("@fkFirma", DB.PkFirma));
//                    list3.Add(new SqlParameter("@Tarih", OdemeTarihi.DateTime));
//                    list3.Add(new SqlParameter("@Odenecek", ceDevirTutar.Text.ToString().Replace(",", ".")));
//                    list3.Add(new SqlParameter("@fkSatislar", "0"));
//                    DB.ExecuteSQL(@"INSERT INTO dbo.Taksitler (fkFirma,Tarih,Odenecek,Odenen,SiraNo,Aciklama,OdemeSekli,fkSatislar)
//                  VALUES(@fkFirma,@Tarih,@Odenecek,0,1,'Açık Hesap','Açık Hesap',@fkSatislar)", list3);
//                }
                Temizle();
                
            }
           
            simpleButton2.Enabled = false;
            BakiyeGetir();
            frmMesajBox mesajj = new frmMesajBox(200);
            mesajj.label1.BackColor = System.Drawing.Color.GreenYellow;
            mesajj.label1.Text = "Bilgiler Kaydedildi.";
            mesajj.Show();
            DB.ExecuteSQL("UPDATE Firmalar Set SonSatisTarihi=getdate() where pkFirma=" + fkFirma);
            Close();
        }
        void Temizle()
        {
            ceDevirTutar.Value = 0;
            tEaciklama.Text = "Bakiye Düzeltme";
            labelControl5.Text = "";
            ceBakiyeKaydedilecek.Value = 0;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
           // musteriara();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Bakiyesi Sıfırlanacak, Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (secim == DialogResult.No) return;
            ceBakiyeKaydedilecek.Value = ceBakiye.Value;
            ceBakiye.Value = 0;
            simpleButton3.Enabled = false;
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
            if (e.KeyCode == Keys.F4)
                simpleButton1_Click(sender, e);
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

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();
            KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();
            KasaHareketDuzelt.Tag = this.Tag;
            KasaHareketDuzelt.ShowDialog();

            DevirBakiyeler();
        }
    }
}
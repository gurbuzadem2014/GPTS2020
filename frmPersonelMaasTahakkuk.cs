using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using GPTS.islemler;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmPersonelMaasTahakkuk : DevExpress.XtraEditors.XtraForm
    {
        public frmPersonelMaasTahakkuk()
        {
            InitializeComponent();
        }
        void Onceki_Maas_Tahakkuklar()
        {
            try
            {
                DataTable dt = DB.GetData(@"select * from KasaHareket with(nolock) where Modul=5 and Tipi=3 and fkpersoneller=" + DB.pkPersoneller.ToString() + " order by Tarih desc");
                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata" + ex.Message);
            }
        }
        private void frmMaasTahakkuk_Load(object sender, EventArgs e)
        {
            DataTable dtp = DB.GetData(@"select * from Personeller with(nolock) where pkpersoneller=" + DB.pkPersoneller.ToString());
            
            pkPersoneller.Text = dtp.Rows[0]["pkpersoneller"].ToString();
            teAdi.Text = dtp.Rows[0]["adi"].ToString();
            teSoyadi.Text = dtp.Rows[0]["soyadi"].ToString();
            ceMaas.EditValue = dtp.Rows[0]["maasi"].ToString();
            cEAgi.EditValue = dtp.Rows[0]["agiucreti"].ToString();
            cEYemek.EditValue = dtp.Rows[0]["yemekucreti"].ToString();
            cEYol.EditValue = dtp.Rows[0]["yolucreti"].ToString();
            txtMaasGunu.Text = dtp.Rows[0]["maas_gunu"].ToString();
            //string IlkMaasTarihi = dtp.Rows[0]["IlkMaasTarihi"].ToString();
            //if (IlkMaasTarihi=="")
            //    deilkmaastarihi.DateTime = Convert.ToDateTime(IlkMaasTarihi);

            string isegiris = dtp.Rows[0]["isegiristarih"].ToString();
            if (isegiris !="")
                isegiristarihi.DateTime = Convert.ToDateTime(isegiris);


            string sonmaastarihi = dtp.Rows[0]["SonMaasTarihi"].ToString();
            if (sonmaastarihi == "")
                desonmaastarihi.DateTime = isegiristarihi.DateTime;
            else
                desonmaastarihi.DateTime = Convert.ToDateTime(sonmaastarihi);

            deMaasTarihi.DateTime = DateTime.Today;

            //deMaasTarihi.Focus();

            cBYil.Text= DateTime.Now.Year.ToString();
            cBDonem.Text = DateTime.Now.Month.ToString();

            KullandigiizinGetir();

            SonAvanslari();
        }

        void SonAvanslari()
        {
            string sql = "select top 10 pkKasaHareket,Tarih,Aciklama,Borc,Alacak,AktifHesap from KasaHareket KH with(nolock)"+
            " where fkPersoneller = "+pkPersoneller.Text+" order by pkKasaHareket desc";

            gCPerHareketleri.DataSource = DB.GetData(sql);
        }
        void KullandigiizinGetir()
        {
            string kalan = "0", izinhakedis="0", izinkullanilan="0";
            DataTable izinDb = DB.GetData(@"select top 1 *,(izin_hakedis-izin_kullanilan) as kalan from PersonelIzınHakedis 
            where fkPersoneller=" + pkPersoneller.Text+" order by yil desc");

            if (izinDb.Rows.Count > 0)
            {
                kalan = izinDb.Rows[0]["kalan"].ToString();
                izinhakedis = izinDb.Rows[0]["izin_hakedis"].ToString();
                izinkullanilan = izinDb.Rows[0]["izin_kullanilan"].ToString();
            }
            lcKullandigiIzin.Text = "Kalan İzin = " + kalan + " (İzin Hakediş="+ izinhakedis + " / İzin Kullanılan="+ izinkullanilan+")";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (deMaasTarihi.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Tarih Seçmediniz!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string s = formislemleri.MesajBox(teAdi.Text+" "+ teSoyadi.Text + "\r" + cEGenelToplam.Text + "\r Maaş Hakediş Eklensin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;
            

            string sql = @"INSERT INTO KasaHareket (fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,AktifHesap,yil,donem)
             values(@fkPersoneller,'@tarih',5,3,@Borc,0,'@Aciklama',0,@yil,@donem)";
            sql = sql.Replace("@fkPersoneller", pkPersoneller.Text);
            sql = sql.Replace("@tarih", deMaasTarihi.DateTime.ToString("yyyy-MM-dd HH:mm"));
            sql = sql.Replace("@Borc", cEGenelToplam.Value.ToString().Replace(",", "."));
            sql = sql.Replace("@Aciklama", tEaciklama.Text);
            sql = sql.Replace("@yil",cBYil.Text);
            sql = sql.Replace("@donem",cBDonem.Text);
            int sonuc = DB.ExecuteSQL(sql);
            //chkEAgi
            //if (chkEAgi.Checked == false && cEAgi.Value>0)
            //{
            //    sql = @"INSERT INTO KasaHareket (fkkasalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,AktifHesap,yil,donem)
            //            values(1,@fkPersoneller,'@tarih',5,8,@Borc,0,'@Aciklama',0,@yil,@donem)";
            //    sql = sql.Replace("@fkPersoneller", pkPersoneller.Text);
            //    sql = sql.Replace("@tarih", deMaasTarihi.DateTime.ToString("yyyy-MM-dd HH:mm"));
            //    sql = sql.Replace("@Borc", cEAgi.Value.ToString().Replace(",","."));
            //    sql = sql.Replace("@Aciklama", "A.G.İ. Ödemesi");
            //    sql = sql.Replace("@yil", cBYil.Text);
            //    sql = sql.Replace("@donem", cBDonem.Text);
            //    DB.ExecuteSQL(sql);
            //}

            DB.ExecuteSQL("update Personeller set SonMaasTarihi='" + deMaasTarihi.DateTime.ToString("yyyy-MM-dd") + "' where pkPersoneller=" + pkPersoneller.Text);
           
             if (sonuc == 1)
            {
                formislemleri.Mesajform("Personel Maaş Tahakkuk Edilmiştir Yapılmıştır", "S", 200);
            }
            else
                formislemleri.Mesajform("Hata Oluştu" + sonuc, "K", 200);

            //DevExpress.XtraEditors.XtraMessageBox.Show("Personel Maaş Tahakkuk Edilmiştir Yapılmıştır.", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            deMaasTarihi.EditValue = DateTime.Today.ToString("dd.MM.yyyy");
            ceMaas.Value = 0;
            tEaciklama.Text = "";
            ceMaas.Focus();
        }

        private void ceMaas_EditValueChanged(object sender, EventArgs e)
        {
            ceGunlukMaas.Value = ceMaas.Value / 30;
        }

        private void xtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (e.Page == xtraTabPage2)
            {
                Onceki_Maas_Tahakkuklar();
            }
        }

        private void frmMaasTahakkuk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            } 
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = formislemleri.MesajBox(teAdi.Text + " " + teSoyadi.Text + " Maaş Hakediş Silinsin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkKasaHareket = dr["pkKasaHareket"].ToString();

            string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + pkKasaHareket;
            int sonuc = DB.ExecuteSQL(sql);

            DB.ExecuteSQL("update Personeller set SonMaasTarihi=null where pkPersoneller=" + pkPersoneller.Text);

            if (sonuc == 1)
            {
                formislemleri.Mesajform("Personel Maaş Hakediş Silinmiştir", "S", 200);
            }
            else
                formislemleri.Mesajform("Hata Oluştu" + sonuc, "K", 200);


            Onceki_Maas_Tahakkuklar();

        }

        private void ceMaasHakedis_EditValueChanged(object sender, EventArgs e)
        {
            cEGenelToplam.Value = 0;
            tEaciklama.Text = "Maaş";
            cEGenelToplam.Value = ceMaasHakedis.Value;
            if (chkEAgi.Checked)
            {
                cEGenelToplam.Value += cEAgi.Value;
                tEaciklama.Text += "+A.G.İ. ";
            }
            if (chkEYemek.Checked)
            {
                cEGenelToplam.Value += cEYemek.Value;
                tEaciklama.Text += "+Yemek";
            }
            if (chkEYol.Checked)
            {
                cEGenelToplam.Value += cEYol.Value;
                tEaciklama.Text += "+Yol";
            }
        }

        private void ceGunlukMaas_EditValueChanged(object sender, EventArgs e)
        {
            ceMaasHakedis.Value = ceGunlukMaas.Value * ceCalistigiGun.Value;
        }

        private void pkPersoneller_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void deMaasTarihi_EditValueChanged(object sender, EventArgs e)
        {
            //string gun = deMaasTarihi.DateTime.ToString("d");
            //string maasgunu=desonmaastarihi.DateTime.ToString("d");
            //int gecen_gun = int.Parse(gun) - int.Parse(maasgunu);

            TimeSpan kalangun = deMaasTarihi.DateTime - desonmaastarihi.DateTime;//Sonucu zaman olarak döndürür
            double toplamGun = kalangun.TotalDays;// kalanGun den TotalDays ile sadece toplam gun değerini çekiyoruz. 

            lcGecenGun.Text = "Çalıştığı Gün="+toplamGun.ToString();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
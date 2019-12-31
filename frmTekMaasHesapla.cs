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
    public partial class frmTekMaasHesapla : DevExpress.XtraEditors.XtraForm
    {
        public frmTekMaasHesapla()
        {
            InitializeComponent();
        }
        private void frmTekMaasHesapla_Load(object sender, EventArgs e)
        {
            DataTable dtkh = DB.GetData("select isnull(max(donem),MONTH(GETDATE())) as donem,YEAR(GETDATE()) as yil from KasaHareket with(nolock) where fkPersoneller=" + DB.pkPersoneller.ToString());

            cBDonem.EditValue = dtkh.Rows[0][0].ToString();
            cBYil.EditValue = dtkh.Rows[0][1].ToString();

            DataTable dtp = DB.GetData(@"select * from Personeller with(nolock) where pkpersoneller=" + DB.pkPersoneller.ToString());

            pkmusteri.Text = dtp.Rows[0]["pkpersoneller"].ToString();
            teAdi.Text = dtp.Rows[0]["adi"].ToString();
            teSoyadi.Text = dtp.Rows[0]["soyadi"].ToString();
            tETel.Text = dtp.Rows[0]["tel"].ToString();
            //maaş hakediş
            try
            {
                DataTable dt3 = DB.GetData(@"select SUM(isnull(Borc,0)-isnull(Alacak,0)) as Tutar from KasaHareket with(nolock) where fkpersoneller=" + DB.pkPersoneller.ToString());
                decimal k = 0;
                decimal.TryParse(dt3.Rows[0][0].ToString(),out k);
                cEMaasHakedis.Value=k;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata" + ex.Message);
            }

            MaasOdemeleri();
            teAciklama.Text = DateTime.Today.ToString("MMMM") + " Maaş Ödemesi";
        }
        void MaasOdemeleri()
        {
            try
            {
                gridControl2.DataSource = DB.GetData(@"select * from KasaHareket with(nolock) where Modul=5 and fkpersoneller=" + DB.pkPersoneller.ToString());
            }
            catch (Exception ex)
            {

            }
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = "";
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Personel Maaş Ödemesi Yapılsın mı?", "GPTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            sql = @"INSERT INTO KasaHareket (fkkasalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,AktifHesap,GiderOlarakisle,OdemeSekli)
                    values(@fkkasalar,@fkPersoneller,getdate(),5,5,0,@Alacak,'@Aciklama',@donem,@yil,1,1,'Nakit')";
            sql = sql.Replace("@fkkasalar", "1");
            sql = sql.Replace("@fkPersoneller", pkmusteri.Text);//DB.pkPersoneller.ToString());
            sql = sql.Replace("@Alacak", cEMaasHakedis.Value.ToString().Replace(",","."));
            sql = sql.Replace("@Aciklama", teAciklama.Text);
            sql = sql.Replace("@yil", cBYil.EditValue.ToString());
            sql = sql.Replace("@donem", cBDonem.EditValue.ToString());

            int sonuc = DB.ExecuteSQL(sql);

            DB.ExecuteSQL("update Personeller set SonMaasTarihi=getdate() where pkPersoneller=" + pkmusteri.Text);

            DevExpress.XtraEditors.XtraMessageBox.Show("Personel Maaş Ödemesi Yapılmıştır.", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            MaasOdemeleri();

            cEMaasHakedis.Value = 0;

            //BirSonrakiMaasTahakkukEkle();
        }

        void BirSonrakiMaasTahakkukEkle()
        {
            string s = formislemleri.MesajBox("Birsonraki Maaş Tahakuk Eklensin mi?", "Maaş Tahakkuk", 1, 1);
            if (s == "0") return;

            DataTable dtp = DB.GetData(@"select maasi+agiucreti+yolucreti+yemekucreti as net_maas from Personeller with(nolock) where pkpersoneller=" + DB.pkPersoneller.ToString());
           
            decimal maas = decimal.Parse(dtp.Rows[0]["net_maas"].ToString());


            string sql = @"INSERT INTO KasaHareket (fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,AktifHesap,yil,donem)
             values(@fkPersoneller,'@tarih',5,3,@Borc,0,'@Aciklama',0,@yil,@donem)";
            sql = sql.Replace("@fkPersoneller", pkmusteri.Text);
            sql = sql.Replace("@tarih", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            sql = sql.Replace("@Borc", maas.ToString().Replace(",", "."));
            sql = sql.Replace("@Aciklama", "Birsonraki Maaş Tahakkuk");
            sql = sql.Replace("@yil", cBYil.Text);
            sql = sql.Replace("@donem", cBDonem.Text);
            DB.ExecuteSQL(sql);

            
            DevExpress.XtraEditors.XtraMessageBox.Show("Personel Maaş Tahakkuk Edilmiştir Yapılmıştır.", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void xtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (e.Page.Name == "xtraTabPage2")
            {
                try
                {
                    DataTable dt2 = DB.GetData(@"select * from KasaHareket with(nolock) where Modul=5 and Tipi=5 and fkpersoneller=" + DB.pkPersoneller.ToString() + " order by Tarih desc");
                    gridControl1.DataSource = dt2;
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB.pkPersoneller = int.Parse(pkmusteri.Text);

            frmPersonelMaasTahakkuk MaasTahakkuk = new frmPersonelMaasTahakkuk();
            MaasTahakkuk.ShowDialog();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = formislemleri.MesajBox(teAdi.Text + " " + teSoyadi.Text + " Maaş Silinsin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string pkKasaHareket = dr["pkKasaHareket"].ToString();

            string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + pkKasaHareket;

            int sonuc = DB.ExecuteSQL(sql);

            DB.ExecuteSQL("update Personeller set SonMaasTarihi=null where pkPersoneller=" + pkmusteri.Text);

            if (sonuc > 1)
            {
                formislemleri.Mesajform("Personel Maaş Silinmiştir", "S", 200);
            }
            else
                formislemleri.Mesajform("Hata Oluştu" + sonuc, "K", 200);

            MaasOdemeleri();
  
        }
    }
}
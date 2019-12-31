using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using System.IO;
using GPTS.islemler;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class frmEnvanter : DevExpress.XtraEditors.XtraForm
    {
        string Dosya = DB.exeDizini + "\\FisSatisGecmisGrid.xml";

        public frmEnvanter()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmEnvanter_Load(object sender, EventArgs e)
        {
            ilkdate.DateTime = DateTime.Today;

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }


            gridyukle();
        }
        void gridyukle()
        {
            string sql = @"SELECT 
sk.pkStokKarti, 
sk.StokKod, 
sk.Barcode, 
StokGruplari.StokGrup, 
sk.fkStokAltGruplari, 
sk.Stokadi, 
sk.fkStokGrup, 
sk.KdvOrani, 
sk.Stoktipi, 
sk.KdvOrani,
sk.fkTedarikciler, 
sk.MuhasebeKodu,
sk.MasrafMerkezi,
case when sk.Aktif=1 then 'Aktif'
else 'Pasif' end Aktif,
sk.ToplamGiren,
sk.ToplamCikan,
sk.Mevcut, 
sk.AlisFiyati,

case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,
sk.SatisFiyati, 

case when sk.satis_iskonto>0 then 
(sk.SatisFiyati-((sk.SatisFiyati*satis_iskonto)/100))
else
sk.SatisFiyati end SatisFiyatiiskontolu,

sk.KutuFiyat, 
sk.fkStokFiyatGruplari,
sk.fkMarka, 
sk.fkModel,
sk.fkBedenGrupKodu,
sk.fkBedenGrupKodu,
StokGruplari.StokGrup,

case when sk.alis_iskonto>0 then 
sk.Mevcut*(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.Mevcut*sk.AlisFiyati end AlisiskEnvanter,

case when sk.satis_iskonto>0 then 
sk.Mevcut*(sk.SatisFiyati-((sk.SatisFiyati*satis_iskonto)/100))
else
sk.Mevcut*sk.SatisFiyati end SatisiskEnvanter,

sk.KritikMiktar,
sk.EklemeTarihi,
t.Firmaadi,
sag.StokAltGrup
 FROM  StokKarti sk with(nolock)
LEFT JOIN StokGruplari with(nolock) ON sk.fkStokGrup = StokGruplari.pkStokGrup
LEFT JOIN StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
 where 1=1";

            if (cbMevcut.Checked)
                sql = sql + " and sk.Mevcut>0";

            if (checkEdit1.Checked)
                sql = sql + " and sk.Aktif=1";

            //else if (checkEdit5.Checked)
            //    sql = sql + " and sk.Mevcut<0";

            //if (checkEdit3.Checked)
            //    sql = sql + " and KritikMiktar<Mevcut"; //En büyük, en yüksek, en çok, maksimum.
            //else if (checkEdit2.Checked)
            //    sql = sql + " and Mevcut<Asgari"; //En küçük,  en az, minumum

            //if (checkEdit10.Checked)
            //    sql = sql + " and len(Barcode)<7";

            gridControl1.DataSource = DB.GetData(sql);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            int secilen = gridView1.FocusedRowHandle;
            if (secilen < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();

            gridyukle();

            gridView1.FocusedRowHandle = secilen;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string guid = Guid.NewGuid().ToString();
            gridControl1.ExportToXls(exedizini + "/" + guid + ".xls");
            Process.Start(exedizini + "/" + guid + ".xls");
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbMevcut_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void cbMevcut_CheckedChanged_1(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //formislemleri.Mesajform("OK", "S");
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl1, "A4");
        }

        private void frmEnvanter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("SELECT * FROM Envanter with(nolock) where convert(varchar(10),Donem,112)='" +
               ilkdate.DateTime.ToString("yyyyMMdd") + "'");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Daha Önce " + ilkdate.DateTime.ToString("dd.MM.yyyy") + " Olarak Kaydedildi. \r Lütfen Başka Bir Dönem Seçiniz!");
                ilkdate.Focus();
                return;
            }
            try 
            {
                if (DB.conTrans == null)
                {
                    DB.conTrans = new SqlConnection(DB.ConnectionString());
                    //DB.conTrans.Open();
                    //transaction = conTrans.BeginTransaction("AdemTransaction");
                }
                if (DB.conTrans.State == ConnectionState.Closed)
                {
                    //DB.conTrans = new SqlConnection(DB.ConnectionString());
                    DB.conTrans.Open();
                    //transaction = conTrans.BeginTransaction("AdemTransaction");
                }
                //DB.conTrans.BeginTransaction();
                DB.transaction = DB.conTrans.BeginTransaction("AdemTransaction");

                string yeni_id =
                DB.ExecuteScalarSQLTrans("INSERT INTO Envanter (Donem,Tarih,Aciklama) values('" + ilkdate.DateTime.ToString("yyyy-MM-dd") + "',getdate(),'" + tEaciklama.Text + "') select IDENT_CURRENT('Envanter');");
                int hatali = 0, say = 0;

                frmYukleniyor yukleniyor = new frmYukleniyor();
                yukleniyor.labelControl1.Text = "Envanter Oluşturuluyor Lütfen Bekleyiniz...";
                yukleniyor.TopMost = true;
                yukleniyor.Show();

                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    DataRow dr = gridView1.GetDataRow(i);
                    //TODO:trans kullanılacak
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkEnvanter", yeni_id));
                    list.Add(new SqlParameter("@fkStokKarti", dr["pkStokKarti"].ToString().Replace(",", ".")));
                    list.Add(new SqlParameter("@Adet", dr["Mevcut"].ToString().Replace(",", ".")));
                    list.Add(new SqlParameter("@AlisFiyati", dr["AlisiskEnvanter"].ToString().Replace(",", ".")));
                    list.Add(new SqlParameter("@SatisFiyati", dr["SatisiskEnvanter"].ToString().Replace(",", ".")));

                    string sonuc = DB.ExecuteSQLTrans(@"INSERT INTO EnvanterDetay (fkEnvanter,fkStokKarti,Adet,AlisFiyati,SatisFiyati) 
                    values(@fkEnvanter,@fkStokKarti,@Adet,@AlisFiyati,@SatisFiyati)", list);
                    if (sonuc == "-1")
                        hatali++;
                    else
                        say++;
                }

                yukleniyor.Close();
                if(hatali==0)
                    MessageBox.Show("Envanter Kaydedildi");
                else
                   MessageBox.Show(hatali.ToString() + " hatalı");
            }
            catch (Exception exp)
            {
                try
                {
                    DB.transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    DB.conTrans.Close();

                    MessageBox.Show("Rollback Exception Type: {0}", ex2.GetType().ToString());
                    MessageBox.Show("  Message: {0}", ex2.Message);
                }
                //DB.conTrans.Close();
                //return;
            }
            finally
            {
                DB.transaction.Commit();
                //DB.conTrans.Close();
            }
            DB.conTrans.Close();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if(e.Page==xtraTabPage1)
                gridyukle();
            else if (e.Page == xtraTabPage2)
                gridControl2.DataSource = DB.GetData(@"select pkEnvanter,Tarih,Aciklama,Donem,
SUM(Adet) as ToplamMiktar,
SUM(AlisFiyati) as ToplamTutarAlis,
SUM(SatisFiyati) as ToplamTutar  from Envanter E with(nolock)
left join EnvanterDetay ED with(nolock) on ED.fkEnvanter=E.pkEnvanter
group by pkEnvanter,Tarih,Aciklama,Donem");
            else if (e.Page == xtraTabPage3)
                pivotGridControl1.DataSource = DB.GetData(@"select * from Envanter E with(nolock)
            left join EnvanterDetay ED with(nolock) on ED.fkEnvanter=E.pkEnvanter");
        }

        private void ilkdate_EditValueChanged(object sender, EventArgs e)
        {
            tEaciklama.Text = ilkdate.DateTime.ToString("dd MMMM")+ " Envanteri";
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridView2.FocusedRowHandle<0) return;

            DataRow dr=gridView2.GetDataRow(gridView2.FocusedRowHandle);

            DB.ExecuteSQL("delete from Envanter where pkEnvanter=" + dr["pkEnvanter"].ToString());
            DB.ExecuteSQL("delete from EnvanterDetay where fkEnvanter=" + dr["pkEnvanter"].ToString());

            gridControl2.DataSource = DB.GetData("select * from Envanter with(nolock)");
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.SaveLayoutToXml(Dosya);
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            //string Dosya = DB.exeDizini + "\\FisSatisGecmisGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void satışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
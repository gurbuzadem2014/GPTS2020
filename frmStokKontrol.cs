using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using GPTS.Include.Data;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmStokKontrol : DevExpress.XtraEditors.XtraForm
    {
        public frmStokKontrol()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        private void ucStokKontrol_Load(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\StokKontrolGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
            gridyukle();
        }
        void gridyukle()
        {
            string sql = @"SELECT sk.pkStokKarti, sk.StokKod, sk.Barcode, StokGruplari.StokGrup, sk.fkStokAltGruplari, sk.Stokadi, 
                      sk.fkStokGrup, sk.KdvOrani, sk.Stoktipi, sk.KdvOrani,sk.fkTedarikciler, 
                      sk.MuhasebeKodu, sk.MasrafMerkezi, sk.RafId, sk.RafSure, sk.KalmaSure,sk.Aktif,
                      sk.ToplamGiren, sk.ToplamCikan, sk.Mevcut, 
case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,sk.AlisFiyati,
sk.SatisFiyati,sk.KutuFiyat, sk.MiadKontrol, sk.TahminiTeminSure, 
sk.fkStokFiyatGruplari, sk.fkMarka, sk.fkModel, sk.fkBedenGrupKodu,
 sk.fkBedenGrupKodu, StokGruplari.StokGrup,
 (isnull(sk.Mevcut,0)*isnull(sk.AlisFiyati,0)) as Envanter
,sk.KritikMiktar,sk.EklemeTarihi,T.Firmaadi,
sk.alis_iskonto,satis_iskonto,
sk.AlisFiyati-(sk.AlisFiyati*isnull(sk.alis_iskonto,0))/100 as iskontolu_alis,
sk.SatisFiyati-(sk.SatisFiyati*isnull(sk.satis_iskonto,0))/100 as iskontolu_satis,
(sk.SatisFiyati-(sk.SatisFiyati*isnull(sk.satis_iskonto,0))/100)-
(sk.AlisFiyati-(sk.AlisFiyati*isnull(sk.alis_iskonto,0))/100) as Kar,
sk.SatisAdedi,sf.SatisFiyatiKdvli,skd.MevcutAdet as depo_adet FROM  StokKarti sk with(nolock)
LEFT JOIN StokGruplari with(nolock) ON sk.fkStokGrup = StokGruplari.pkStokGrup
LEFT JOIN Tedarikciler T with(nolock) ON T.pkTedarikciler=sk.fkTedarikciler 
left join SatisFiyatlari sf on sf.fkStokKarti=sk.pkStokKarti and sf.fkSatisFiyatlariBaslik =(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik with(nolock) where Tur=2)
left join StokKartiDepo skd with(nolock) on skd.fkStokKarti=sk.pkStokKarti";
//and skd.fkDepolar=" + Degerler.fkSube;

            sql = sql + " where 1=1";

            if (cbMevcutSifirdanBuyuk.Checked)
                sql = sql + " and sk.Mevcut>0";

            else if (cbSifir.Checked)
                sql = sql + " and sk.Mevcut=0";
            
            else if (checkEdit5.Checked)
                sql = sql + " and sk.Mevcut<0";

            if (checkEdit3.Checked)
               sql = sql + " and KritikMiktar<Mevcut"; //En büyük, en yüksek, en çok, maksimum.
            else if (checkEdit2.Checked)
                sql = sql + " and Mevcut<Asgari"; //En küçük,  en az, minumum

            if (checkEdit10.Checked)
               sql = sql + " and len(Barcode)<7";

            if (!cbTumu.Checked)
               sql = sql + " and sk.aktif=1";
            
            gridControl1.DataSource = DB.GetData(sql);
        }

        void gridyukleHareketGormeyen()
        {
            string sql = @"SELECT sk.pkStokKarti, sk.StokKod, sk.Barcode, StokGruplari.StokGrup, sk.Stokadi, 
                      sk.fkStokGrup, sk.KdvOrani,
                      sk.Stoktipi, sk.KdvOrani, sk.fkTedarikciler, 
                      sk.MuhasebeKodu, sk.MasrafMerkezi, sk.RafId, sk.RafSure, sk.KalmaSure, sk.Aktif,
                      sk.ToplamGiren, sk.ToplamCikan, sk.Mevcut, sk.AlisFiyati, sk.AlisPara, sk.SatisFiyati, 
                      sk.KutuFiyat, sk.MiadKontrol, sk.TahminiTeminSure, 
 sk.fkStokFiyatGruplari, sk.fkMarka, sk.fkModel, sk.fkBedenGrupKodu,
 sk.fkBedenGrupKodu, StokGruplari.StokGrup,(isnull(sk.Mevcut,0)*isnull(sk.AlisFiyati,0)) as Envanter
,sk.KritikMiktar,sk.EklemeTarihi,sd.Tarih,DATEDIFF(dd,sd.Tarih,getdate()) as Fark  FROM  StokKarti sk with(nolock)
LEFT OUTER JOIN StokGruplari with(nolock) ON sk.fkStokGrup = StokGruplari.pkStokGrup
LEFT JOIN (select fkStokKarti,Max(Tarih) as Tarih from SatisDetay with(nolock) group by fkStokKarti) sd  on sd.fkStokKarti=sk.pkStokKarti";

                if (icbDurumu.SelectedIndex == 1)
                    sql = sql + " where sk.Aktif=1";
                else if (icbDurumu.SelectedIndex == 2)
                   sql = sql + " where sk.Aktif=0";

            gridControl2.DataSource = DB.GetData(sql);
        }

        static void OpenMicrosoftExcel(string f)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "WINEXCEL.EXE";
            startInfo.Arguments = f;
            Process.Start(startInfo);
        }
        static void OpenMicrosoftWord(string f)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "WINWORD.EXE";
            startInfo.Arguments = f;
            Process.Start(startInfo);
        }


        private void checkEdit10_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
           gridyukle();
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int secilen=gridView1.FocusedRowHandle;
            if (secilen < 0) return;
            
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();

            gridyukle();

            gridView1.FocusedRowHandle = secilen;
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string guid = Guid.NewGuid().ToString();
            gridControl1.ExportToXls(exedizini +"/"+guid+".xls");
            Process.Start(exedizini+"/"+guid+".xls");

            //OpenMicrosoftExcel(exedizini + "/" + guid + ".xls");
        }

        private void checkEdit7_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void checkEdit6_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void pasifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Stok Kartları Pasif Yapılacak Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            if (xtraTab.SelectedTabPage == xtGenel)
            {
                int[] nRows1 = gridView1.GetSelectedRows();

                int secilen = nRows1.Length;
                for (int i = 0; i < secilen; i++)
                {
                    string id = nRows1[i].ToString();
                    DataRow dr = gridView1.GetDataRow(int.Parse(id));
                    DB.ExecuteSQL("UPDATE StokKarti set Aktif=0,GuncellemeTarihi=getdate() where pkStokKarti=" + dr["pkStokKarti"].ToString());
                }
                gridyukle();
            }
            if (xtraTab.SelectedTabPage == xtHareketGormeyen)
            {
                int[] nRows1 = gridView2.GetSelectedRows();

                int secilen = nRows1.Length;
                for (int i = 0; i < secilen; i++)
                {
                    string id = nRows1[i].ToString();
                    DataRow dr = gridView2.GetDataRow(int.Parse(id));
                    DB.ExecuteSQL("UPDATE StokKarti set Aktif=0,GuncellemeTarihi=getdate() where pkStokKarti=" + dr["pkStokKarti"].ToString());
                }
                gridyukleHareketGormeyen();
            }
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();

            //if (xtraTab.SelectedTabPage == xtGenel)
            //{
            //    if (gridView1.FocusedRowHandle < 0) return;
            //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //    frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0");
            //    SatisUrunBazindaDetay.Tag = "2";
            //    SatisUrunBazindaDetay.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            //    SatisUrunBazindaDetay.ShowDialog();
            //}
            //if (xtraTab.SelectedTabPage == xtHareketGormeyen)
            //{
            //    if (gridView2.FocusedRowHandle < 0) return;
            //    DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            //    frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0");
            //    SatisUrunBazindaDetay.Tag = "2";
            //    SatisUrunBazindaDetay.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            //    SatisUrunBazindaDetay.ShowDialog();
            //}
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtGenel)
                gridyukle();

            else if (e.Page == xtHareketGormeyen)
                gridyukleHareketGormeyen();

            else if (e.Page == xtraTabPage1)
                DigerBarkodlar();

        }
        void DigerBarkodlar()
        {
          string sql = @"select sk.pkStokKarti,b.pkStokKartiBarkodlar,sk.Barcode,isnull(b.Barkod,'Tanımlanmamış') as Barkod,
sk.Stokadi,sg.StokGrup,sag.StokAltGrup,sk.AlisFiyati,sk.SatisFiyati,
b.SatisAdedi,sk.SatisAdedi as AnaStokSatisAdedi from StokKarti sk with(nolock) 
left join StokKartiBarkodlar b with(nolock) on b.fkStokKarti=sk.pkStokKarti
left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
order by b.pkStokKartiBarkodlar desc";

            gcDigerBarkodlar.DataSource = DB.GetData(sql);

        }

        private void icbDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridyukleHareketGormeyen();
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void gridView2_EndSorting(object sender, EventArgs e)
        {
            if (gridView2.DataRowCount > 0)
                gridView2.FocusedRowHandle = 0;
        }

        private void ürünSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Stok Kartları Silinecek Eminmisiniz? \n Not:Hareket Görmeyen StokKartları Silinecektir.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            if (xtraTab.SelectedTabPage == xtGenel)
            {
                int[] nRows1 = gridView1.GetSelectedRows();

                int secilen = nRows1.Length;
                for (int i = 0; i < secilen; i++)
                {
                    string id = nRows1[i].ToString();
                    DataRow dr = gridView1.GetDataRow(int.Parse(id));
                    string pkStokKarti =  dr["pkStokKarti"].ToString();
                    int sha = 0,aha=0;

                    sha = int.Parse(DB.GetData("select count(*) from SatisDetay with(nolock) where fkStokKarti=" + pkStokKarti).Rows[0][0].ToString());
                    aha = int.Parse(DB.GetData("select count(*) from AlisDetay with(nolock) where fkStokKarti=" + pkStokKarti).Rows[0][0].ToString());
                    
                    if(sha+sha==0)
                       DB.ExecuteSQL("DELETE FROM StokKarti where pkStokKarti=" + pkStokKarti);
                }
                gridyukle();
            }
            if (xtraTab.SelectedTabPage == xtHareketGormeyen)
            {
                int[] nRows1 = gridView2.GetSelectedRows();

                int secilen = nRows1.Length;
                for (int i = 0; i < secilen; i++)
                {
                    string id = nRows1[i].ToString();
                    DataRow dr = gridView2.GetDataRow(int.Parse(id));
                    string pkStokKarti = dr["pkStokKarti"].ToString();
                    int sha = 0, aha=0;

                    sha = int.Parse(DB.GetData("select count(*) from SatisDetay with(nolock) where fkStokKarti=" + pkStokKarti).Rows[0][0].ToString());
                    aha = int.Parse(DB.GetData("select count(*) from AlisDetay with(nolock) where fkStokKarti=" + pkStokKarti).Rows[0][0].ToString());

                    if (sha + sha == 0)
                        DB.ExecuteSQL("DELETE FROM StokKarti where pkStokKarti=" + pkStokKarti);
                }
                gridyukleHareketGormeyen();
            }
        }

        private void aktifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Stok Kartları Aktif Yapılacak Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            if (xtraTab.SelectedTabPage == xtGenel)
            {
                int[] nRows1 = gridView1.GetSelectedRows();

                int secilen = nRows1.Length;
                for (int i = 0; i < secilen; i++)
                {
                    string id = nRows1[i].ToString();
                    DataRow dr = gridView1.GetDataRow(int.Parse(id));
                    DB.ExecuteSQL("UPDATE StokKarti set Aktif=1,GuncellemeTarihi=getdate() where pkStokKarti=" + dr["pkStokKarti"].ToString());
                }
                gridyukle();
            }
            if (xtraTab.SelectedTabPage == xtHareketGormeyen)
            {
                int[] nRows1 = gridView2.GetSelectedRows();

                int secilen = nRows1.Length;
                for (int i = 0; i < secilen; i++)
                {
                    string id = nRows1[i].ToString();
                    DataRow dr = gridView2.GetDataRow(int.Parse(id));
                    DB.ExecuteSQL("UPDATE StokKarti set Aktif=1,GuncellemeTarihi=getdate() where pkStokKarti=" + dr["pkStokKarti"].ToString());
                }
                gridyukleHareketGormeyen();
            }
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
            gridyukleHareketGormeyen();
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
            gridyukle();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Çoklu Stok Barkodu Silinecek Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("DELETE FROM StokKartiBarkodlar where pkStokKartiBarkodlar=" +

            dr["pkStokKartiBarkodlar"].ToString());

            DigerBarkodlar();
        }

        private void cbTumu_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\StokKontrolGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);

            gridView1.OptionsBehavior.AutoPopulateColumns = false;
            gridView1.OptionsCustomization.AllowColumnMoving = false;
            gridView1.OptionsCustomization.AllowColumnResizing = false;
            gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            gridView1.OptionsCustomization.AllowRowSizing = false;
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\StokKontrolGrid.xml";
            File.Delete(Dosya);
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
    }
}

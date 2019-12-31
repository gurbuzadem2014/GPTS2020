using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GPTS.islemler;
using System.IO;
using DevExpress.Utils;
using GPTS.Include.Data;
using DevExpress.XtraReports.UI;

namespace GPTS
{
    public partial class frmTedarikciAra : Form
    {
        public frmTedarikciAra()
        {
            InitializeComponent();
        }
        
        private void frmMusteriAra_Load(object sender, EventArgs e)
        {
            TedarikciAra();

            seMaxKayit.Value = Degerler.select_top_tedarikci;

            string Dosya = DB.exeDizini + "\\TedarikciListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            fkFirma_Click(sender, e);
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
            {
                fkFirma_Click(sender, e);
            }
        }
       
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMusteriAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            else if (e.KeyCode == Keys.F7)
                btnYeni_Click(sender, e);
            else if (e.KeyCode == Keys.F9)
                fkFirma_Click(sender, e);
        }

        void TedarikciAra()
        {
            Satis1Toplam.Text = "";
            //if (AraBarkodtan2.Text.Length == 0 && adiara.Text.Length < 2) return;
            string tumu = "";
            if (cbTumu.SelectedIndex == 1) tumu = "1";
            else if (cbTumu.SelectedIndex==2) tumu = "0"; 

            string sql = "exec Tedarikciara_sp '" + AraBarkodtan2.Text + "','" + adiara.Text + "','"+tumu+"',"+DB.fkKullanicilar;

            gridControl1.DataSource = DB.GetData(sql);
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkTedarikciler =dr["pkTedarikciler"].ToString();

            DataTable dt = DB.GetData("exec sp_TedarikciBakiyesi " + pkTedarikciler + ",0");
            if (dt.Rows.Count == 0)
            {
                Satis1Toplam.Text = "0,00";
            }
            else
            {
                decimal bakiye = decimal.Parse(dt.Rows[0][0].ToString());
                Satis1Toplam.Text = bakiye.ToString("##0.00");//dt.Rows[0][0].ToString();
            }
            dt.Dispose();

            DataTable dtNot = 
            DB.GetData(@"SELECT pkTedarikcilerOzelNot ,OzelNot  FROM TedarikcilerOzelNot with(nolock)
                         where fkTedarikciler=" + pkTedarikciler);

            if (dtNot.Rows.Count == 0)
            {
                memoozelnot.Tag = "0";
                memoozelnot.Text = "";
            }
            else
            {
                memoozelnot.Tag = dtNot.Rows[0]["pkTedarikcilerOzelNot"].ToString();
                memoozelnot.Text = dtNot.Rows[0]["OzelNot"].ToString();
            }



            dtNot.Dispose();

        }

        private void fkFirma_Click(object sender, EventArgs e)
        {
            //fkFirma_Click(sender, e);
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());

            DB.FirmaAdi = dr["Firmaadi"].ToString();

           // DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            fkFirma.AccessibleDescription = dr["Firmaadi"].ToString();
            fkFirma.Tag = dr["pkTedarikciler"].ToString();
            DB.ExecuteSQL("update Tedarikciler set tiklamaadedi=tiklamaadedi+1 where pkTedarikciler=" + dr["pkTedarikciler"].ToString());
            Close();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmTedarikciKarti TedarikciKarti = new frmTedarikciKarti("0");
            DB.pkTedarikciler = 0;
            TedarikciKarti.ShowDialog();

            TedarikciAra();
        }

        private void tedarikciHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmTedarikcilerHareketleri CariHareket = new frmTedarikcilerHareketleri();
            CariHareket.musteriadi.Tag = dr["pkTedarikciler"].ToString();
            CariHareket.ShowDialog();

            TedarikciAra();
        }

        private void adiara_KeyUp(object sender, KeyEventArgs e)
        {
            //TedarikciAra();
        }

        private void adiara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            AraBarkodtan2.Text = "";
            if (e.KeyCode == Keys.Left && adiara.Text == "")
            {
                AraBarkodtan2.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
            if (e.KeyCode == Keys.Enter && gridView1.DataRowCount==1)
            {
                gridView1.Focus();
                fkFirma_Click(sender, e);
                //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                //DB.FirmaAdi = dr["Firmaadi"].ToString();
                //DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());
                Close();
            }
        }
        
        void OdemeAl()
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.pkKasaHareket = 0;
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkTedarikci.Text = dr["pkTedarikciler"].ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            TedarikciAra();
            //gridrowgetir(gridView3.FocusedRowHandle);
        }
        
        void OdemeYap()
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.pkKasaHareket = 0;
            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.pkTedarikci.Text = dr["pkTedarikciler"].ToString();
            KasaCikis.Tag = "2";
            KasaCikis.ShowDialog();

            TedarikciAra();
            //gridrowgetir(gridView3.FocusedRowHandle);
        }
        
        private void ödemeYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeYap();
        }

        private void AraBarkodtan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            adiara.Text = "";
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
            //if (e.KeyCode == Keys.Enter)
            //{
            //    fkFirma_Click(sender, e);
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());
            //DB.FirmaAdi = dr["Firmaadi"].ToString();
            ////DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //fkFirma.AccessibleDescription = dr["Firmaadi"].ToString();
            //fkFirma.Tag = dr["pkTedarikciler"].ToString();
            //DB.ExecuteSQL("update Tedarikciler set tiklamaadedi=tiklamaadedi+1 where pkTedarikciler=" + dr["pkTedarikciler"].ToString());
            //Close();
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.FirmaAdi = dr["Firmaadi"].ToString();
            //DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());
            //Close();
            // }
            if (e.KeyCode == Keys.Right)
                adiara.Focus();
        }

        private void AraBarkodtan2_KeyUp(object sender, KeyEventArgs e)
        {
            TedarikciAra();
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeAl();
        }

        private void tedarikçiKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());
            frmTedarikciKarti TedarikciKarti  = new frmTedarikciKarti(dr["pkTedarikciler"].ToString());
            TedarikciKarti.ShowDialog();
        }

        private void pasifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = DBislemleri.
                TedarikciyiPasifYap(dr["pkTedarikciler"].ToString());
            Mesaj.Show();
        }

        private void devirBakiyerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                  
            frmTedarikciBakiyeDuzeltme DevirBakisiSatisKaydi = new frmTedarikciBakiyeDuzeltme();
            DB.pkTedarikciler=int.Parse(dr["pkTedarikciler"].ToString());
            DevirBakisiSatisKaydi.ShowDialog();

            TedarikciAra();
        }

        private void memoozelnot_Leave(object sender, EventArgs e)
        {
            btnAciklamaKaydet.PerformClick();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;
            string Dosya = DB.exeDizini + "\\TedarikciListesiGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\TedarikciListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void sütünSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            //AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }

            //if (e.Column.FieldName == "iskontotutar" && dr["iskontotutar"].ToString() != "0,000000")
            //{
            //    AppearanceHelper.Apply(e.Appearance, appfont);
            //}
            if (e.Column.FieldName == "KaraListe" && dr["KaraListe"].ToString() == "True")
            {
                AppearanceHelper.Apply(e.Appearance, appError);
            }
        }

        private void cbTumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            TedarikciAra();
        }

        private void btnAciklamaKaydet_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (memoozelnot.Tag.ToString() == "0")
            {
                string sonuc = DB.ExecuteScalarSQL("INSERT INTO  TedarikcilerOzelNot (fkTedarikciler,OzelNot)" +
                    " VALUES(" + dr["pkTedarikciler"].ToString() + ",'" + memoozelnot.Text + "') select IDENT_CURRENT('TedarikcilerOzelNot')");

                memoozelnot.Tag = sonuc;
            }
            else
            {
                DB.ExecuteSQL("UPDATE TedarikcilerOzelNot SET OzelNot='" + memoozelnot.Text + "'" +
                " WHERE pkTedarikcilerOzelNot=" + memoozelnot.Tag.ToString());
            }
        }

        private void seMaxKayit_EditValueChanged(object sender, EventArgs e)
        {
            if (adiara.Text == "" && AraBarkodtan2.Text == "")
                TedarikciAra();
        }

        private void seMaxKayit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DB.ExecuteSQL("update Kullanicilar set select_top_tedarikci=" + seMaxKayit.Value.ToString() +
                    " where pkKullanicilar=" + DB.fkKullanicilar);

                Degerler.select_top_tedarikci = int.Parse(seMaxKayit.Value.ToString());
                adiara.Focus();

                formislemleri.Mesajform("Gösterilecek Kayıt Değiştirildi", "S", 100);
            }
        }

        private void adiara_EditValueChanged(object sender, EventArgs e)
        {
            TedarikciAra();
        }

        private void tasarlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdir(true);
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdir(false);
        }

        void Yazdir(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {

                System.Data.DataSet ds = new DataSet("Test");

                int i = gridView1.FocusedRowHandle;
                DataRow dr = gridView1.GetDataRow(i);
                string _fkFirma = dr["pkTedarikciler"].ToString();

                DataTable Cari = DB.GetData("select *,dbo.fon_TedarikciBakiyesi(pkTedarikciler) as Bakiye from Tedarikciler with(nolock) where pkTedarikciler=" + _fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\TedarikciBakiyesi.repx");
                rapor.Name = "TedarikciBakiyesi";
                rapor.Report.Name = "TedarikciBakiyesi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.Print();
        }
    }
}

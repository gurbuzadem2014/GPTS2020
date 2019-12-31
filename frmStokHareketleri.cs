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
using System.IO;
using DevExpress.Utils;

namespace GPTS
{
    public partial class frmStokHareketleri : DevExpress.XtraEditors.XtraForm
    {
        public frmStokHareketleri()
        {
            InitializeComponent();
        }

        private void frmStokHareketleri_Load(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 0;

            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from Depolar with(nolock)");
            StokHareket();

            string Dosya = DB.exeDizini + "\\StokHareketleriGrid.xml";
            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\StokHareketleri.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("StokHareketleri.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            //string id = fkStokKarti.Tag.ToString();
            
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //fkStokKarti.Tag = dr["fkStokKarti"].ToString();

            FisYazdir(false, DosyaYol);

            //fkStokKarti.Tag = id;
        }

        void FisYazdir(bool Disigner, string RaporDosyasi)
        {
            try
            {
                System.Data.DataSet ds = new DataSet("Test");

                string sql = "exec sp_stokHareketleri '@ilktarih','@sontarih','@fkstokkarti'";
                sql = sql.Replace("@ilktarih", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                sql = sql.Replace("@sontarih", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                sql = sql.Replace("@fkstokkarti", fkStokKarti.Tag.ToString());
                DataTable dt = DB.GetData(sql);

                DataTable dtSanal = new DataTable();
                dtSanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
                dtSanal.Columns.Add(new DataColumn("Tipi", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("id", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("GirisAdet", typeof(decimal)));
                dtSanal.Columns.Add(new DataColumn("CikisAdet", typeof(decimal)));
                dtSanal.Columns.Add(new DataColumn("Fark", typeof(decimal)));
                decimal bakiye = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (dt.Rows[i]["Tarih"].ToString() == "") continue;

                    decimal borc = 0, alacak = 0;
                    if (dt.Rows[i]["GirisAdet"].ToString() != "")
                    {
                        borc = decimal.Parse(dt.Rows[i]["GirisAdet"].ToString());
                        bakiye = bakiye + borc;
                    }
                    if (dt.Rows[i]["CikisAdet"].ToString() != "")
                    {
                        alacak = decimal.Parse(dt.Rows[i]["CikisAdet"].ToString());
                        bakiye = bakiye - alacak;
                    }
                    DataRow dr;
                    dr = dtSanal.NewRow();
                    dr["Tarih"] = dt.Rows[i]["Tarih"];
                    dr["Tipi"] = dt.Rows[i]["Tipi"].ToString();
                    dr["id"] = dt.Rows[i]["id"];
                    dr["GirisAdet"] = dt.Rows[i]["GirisAdet"];
                    dr["CikisAdet"] = dt.Rows[i]["CikisAdet"];
                    dr["Fark"] = bakiye;
                    dtSanal.Rows.Add(dr);
                }
                dtSanal.TableName = "FisDetay";

                DataTable StokDetay = dtSanal;//DB.GetData(sql);
                    //DB.GetData(sql, list);
                //if (StokDetay.Rows.Count == 0)
                //{
                //    MessageBox.Show("Kayıt Yok");
                //    return;
                //}
                //DataTable dtSanal = new DataTable();
                //dtSanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
                //dtSanal.Columns.Add(new DataColumn("Hareket", typeof(string)));
                //dtSanal.Columns.Add(new DataColumn("Aciklama", typeof(string)));
                //dtSanal.Columns.Add(new DataColumn("EskiDeger", typeof(decimal)));
                //dtSanal.Columns.Add(new DataColumn("YeniDeger", typeof(decimal)));
                //dtSanal.Columns.Add(new DataColumn("Fark", typeof(decimal)));
                //decimal bakiye = 0;
                //for (int i = 0; i < StokDetay.Rows.Count; i++)
                //{
                //    decimal borc = 0, alacak = 0;
                //    if (StokDetay.Rows[i]["EskiDeger"].ToString() != "0,00")
                //    {
                //        borc = decimal.Parse(StokDetay.Rows[i]["EskiDeger"].ToString());
                //        bakiye = bakiye - borc;
                //    }
                //    if (StokDetay.Rows[i]["YeniDeger"].ToString() != "0,00")
                //    {
                //        alacak = decimal.Parse(StokDetay.Rows[i]["YeniDeger"].ToString());
                //        bakiye = bakiye + alacak;
                //    }
                //    DataRow dr;
                //    dr = dtSanal.NewRow();
                //    dr["Tarih"] = StokDetay.Rows[i]["Tarih"];
                //    dr["Hareket"] = StokDetay.Rows[i]["Hareket"].ToString();
                //    dr["Aciklama"] = StokDetay.Rows[i]["Aciklama"];
                //    dr["EskiDeger"] = StokDetay.Rows[i]["EskiDeger"];
                //    dr["YeniDeger"] = StokDetay.Rows[i]["YeniDeger"].ToString();
                //    dr["Fark"] = bakiye;
                //    dtSanal.Rows.Add(dr);
                //}
                //dtSanal.TableName = "FisDetay";
                //ds.Tables.Add(dtSanal);
                StokDetay.TableName = "StokDetay";
                ds.Tables.Add(StokDetay);

                //Stok Bilgileri
                DataTable StokKart = DB.GetData(@"SELECT * FROM StokKarti with(nolock) WHERE pkStokKarti=" + fkStokKarti.Tag.ToString());
                StokKart.TableName = "StokKart";
                ds.Tables.Add(StokKart);
                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + ilktarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "-" + sontarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;

                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "StokHareketleri";
                rapor.Report.Name = "StokHareketleri";

                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            StokHareket();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmStokDepoDevir StokDepoDevir = new frmStokDepoDevir();
            StokDepoDevir.fkStokKarti.Tag = fkStokKarti.Tag.ToString();
            StokDepoDevir.ShowDialog();

            StokHareket();
        }

        private void StokHareket()
        {
            string sql = "exec sp_stokHareketleri '@ilktarih','@sontarih','@fkstokkarti'";
            sql = sql.Replace("@ilktarih", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            sql = sql.Replace("@sontarih", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            sql = sql.Replace("@fkstokkarti", fkStokKarti.Tag.ToString());
            DataTable dt = DB.GetData(sql);

            DataTable dtSanal = new DataTable();
            dtSanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
            dtSanal.Columns.Add(new DataColumn("Tipi", typeof(string)));
            dtSanal.Columns.Add(new DataColumn("Durumu", typeof(string)));
            dtSanal.Columns.Add(new DataColumn("id", typeof(string)));
            dtSanal.Columns.Add(new DataColumn("GirisAdet", typeof(decimal)));
            dtSanal.Columns.Add(new DataColumn("CikisAdet", typeof(decimal)));
            dtSanal.Columns.Add(new DataColumn("Fark", typeof(decimal)));
            dtSanal.Columns.Add(new DataColumn("fkDepolar", typeof(Int32)));
            dtSanal.Columns.Add(new DataColumn("Firmaadi", typeof(string)));

            decimal bakiye = 0;
   

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal borc = 0, alacak = 0;
                if (dt.Rows[i]["GirisAdet"].ToString() != "")
                {
                    borc = decimal.Parse(dt.Rows[i]["GirisAdet"].ToString());
                    bakiye = bakiye + borc;
                }
                if (dt.Rows[i]["CikisAdet"].ToString() != "")
                {
                    alacak = decimal.Parse(dt.Rows[i]["CikisAdet"].ToString());
                    bakiye = bakiye - alacak;
                }
                DataRow dr;
                dr = dtSanal.NewRow();
                dr["Tarih"] = dt.Rows[i]["Tarih"];
                dr["Tipi"] = dt.Rows[i]["Tipi"].ToString();
                dr["Durumu"] = dt.Rows[i]["Durumu"].ToString();
                dr["id"] = dt.Rows[i]["id"];
                dr["GirisAdet"] = dt.Rows[i]["GirisAdet"];
                dr["CikisAdet"] = dt.Rows[i]["CikisAdet"];
                dr["Fark"] = bakiye;
                dr["fkDepolar"] = dt.Rows[i]["fkDepolar"];
                dr["Firmaadi"] = dt.Rows[i]["Firmaadi"];
                dtSanal.Rows.Add(dr);
            }
            gridControl1.DataSource = dtSanal;
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

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// son 30 gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-30, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
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

        private void dizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\StokHareketleri.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("StokHareketleri.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir(true, DosyaYol);
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            AppearanceDefault appLightSkyBlue = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appGreen = new AppearanceDefault(Color.LightGreen);
            AppearanceDefault appRed = new AppearanceDefault(Color.IndianRed);

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            if (dr["Tipi"].ToString() == "StokDevir")
            {
                AppearanceHelper.Apply(e.Appearance, appLightSkyBlue);
            }
            if (dr["Tipi"].ToString() == "Satış")
            {
                AppearanceHelper.Apply(e.Appearance, appGreen);
            }
            if (dr["Tipi"].ToString() == "Alış")
            {
                AppearanceHelper.Apply(e.Appearance, appRed);
            }

            //else if (e.Column.FieldName == "GirisAdet" && dr["GirisAdet"].ToString() != "0")
            //{
            //    AppearanceHelper.Apply(e.Appearance, appfont);
            //}
            //else if (e.Column.FieldName == "CikisAdet" && dr["CikisAdet"].ToString() != "0")
            //{
            //    AppearanceHelper.Apply(e.Appearance, appError);
            //}
            //else if (e.Column.FieldName == "Fark" && dr["Fark"].ToString() != "0")
            //{
            //    AppearanceHelper.Apply(e.Appearance, appErrorRed);
            //}
        }

        private void fişBilgisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr["Tipi"].ToString() == "Satış")
            {
                frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
                FisNoBilgisi.fisno.Text = dr["id"].ToString();
                FisNoBilgisi.ShowDialog();
            }
            if (dr["Tipi"].ToString() == "Alış")
            {
                frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
                FisNoBilgisi.fisno.Text = dr["id"].ToString();
                FisNoBilgisi.ShowDialog();
            }
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\StokHareketleriGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\StokHareketleriGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }
    }
}
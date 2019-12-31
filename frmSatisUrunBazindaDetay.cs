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
using GPTS.islemler;
using System.IO;

namespace GPTS
{
    public partial class frmSatisUrunBazindaDetay : DevExpress.XtraEditors.XtraForm
    {
        string pkfirma = "";
        string _fkSatisDetay="0", _adet = "0";
        public frmSatisUrunBazindaDetay(string fkfirma, string fkSatisDetay, string adet)
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
            pkfirma = fkfirma;
            _fkSatisDetay = fkSatisDetay;
            _adet = adet;
        }
        private void frmStokTekSatislari_Load(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 0;
            Tasarimlar();
            KartGetir();

            if (this.Tag.ToString() == "0")//satış raporları
            {
                xtraTabControl1.SelectedTabPageIndex = 0;
                SatisGetir();
            }
            else if (this.Tag.ToString() == "1")//Alış raporları
            {
                xtraTabControl1.SelectedTabPageIndex = 1;
                AlisGetir();
            }
            else if (this.Tag.ToString() == "2")//satış Alış raporları
            {
                xtraTabControl1.SelectedTabPageIndex = 2;
                StokHareketleriAlisSatis();
            }
            else if (this.Tag.ToString() == "3")//Satış Faturası ekranından geliyor ise
            {
                xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                panelControl3.Visible = groupControl2.Visible =false;
                //this.Height = Screen.PrimaryScreen.WorkingArea.Height / 2+ 225;
                this.Width = Screen.PrimaryScreen.WorkingArea.Width / 3;
                this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                xtraTabControl1.SelectedTabPageIndex = 3;

                string sql = "";
                sql = @"select top 5 s.pkSatislar as FisNo,f.Firmaadi,
                        s.GuncellemeTarihi as Tarih,
						sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as Fiyati,
						sdu.Durumu as HareketTuru 
						from StokKarti sk with(nolock)
                        inner join  SatisDetay sd with(nolock) on sd.fkStokKarti=sk.pkStokKarti
                        inner join Satislar s with(nolock) on s.pkSatislar=sd.fkSatislar
                        inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
						left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu
                        where s.Siparis=1 and s.fkSatisDurumu in(2,4,5,9) and sk.pkStokKarti=@fkStokKarti
                        order by s.GuncellemeTarihi desc";
                sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);
                gcMusteriSatislari.DataSource = DB.GetData(sql);

                sql = @"select top 5 a.pkAlislar as FisNo,f.Firmaadi,
a.GuncellemeTarihi as Tarih,ad.Adet,(ad.AlisFiyati-ad.iskontotutar) as Fiyati from StokKarti sk with(nolock)
inner join AlisDetay ad with(nolock) on ad.fkStokKarti=sk.pkStokKarti
inner join Alislar a with(nolock) on a.pkAlislar=ad.fkAlislar
inner join Tedarikciler f with(nolock) on f.pkTedarikciler=a.fkFirma
where a.Siparis=1 and  sk.pkStokKarti=@fkStokKarti
order by a.GuncellemeTarihi desc";
                sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);
                gcMusteriAlislari.DataSource = DB.GetData(sql);

                sql = @"select top 15 s.pkSatislar as FisNo,f.Firmaadi,
                        s.GuncellemeTarihi as Tarih,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as Fiyati,
                        sdu.Durumu as HareketTuru from StokKarti sk with(nolock)
                        inner join  SatisDetay sd with(nolock) on sd.fkStokKarti=sk.pkStokKarti
                        inner join Satislar s with(nolock) on s.pkSatislar=sd.fkSatislar
                        inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
                        left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu
                        where s.Siparis=1 and s.fkSatisDurumu in(1,2,4,5,9) and s.fkFirma=@fkFirma and sk.pkStokKarti=@fkStokKarti
                        order by s.GuncellemeTarihi desc";
                sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);
                sql = sql.Replace("@fkFirma", pkfirma);
                gcMusteriSatisAlislari.DataSource = DB.GetData(sql);
                //adet bazında
                sql = @"select top 50 sk.pkStokKarti,sk.Stokadi,s.pkSatislar as FisNo,f.Firmaadi,sd.iskontotutar,sd.iskontoyuzdetutar,
                        s.GuncellemeTarihi as Tarih,sd.Adet,sd.SatisFiyati,(sd.SatisFiyati-sd.iskontotutar) as Fiyati,
                        sdu.Durumu as HareketTuru from StokKarti sk with(nolock)
                        inner join SatisDetay sd with(nolock) on sd.fkStokKarti=sk.pkStokKarti
                        inner join Satislar s with(nolock) on s.pkSatislar=sd.fkSatislar
                        inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
                        left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu
                        where s.Siparis=1 and s.fkSatisDurumu in(1,2,4,5,9) and s.fkFirma=@fkFirma and sk.pkStokKarti=@fkStokKarti
                        order by s.GuncellemeTarihi desc";

                sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);
                sql = sql.Replace("@fkFirma", pkfirma);
                gcAdetBazinda.DataSource = DB.GetData(sql);
                gridView7.ActiveFilterString = "[Adet] ='" + _adet + "'";
                //gridView7.ActiveFilter.EndUpdate();
            }
            else if (this.Tag.ToString() == "4")//Alış Faturası ekranından geliyor ise
            {
                xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                //this.Height = Screen.PrimaryScreen.WorkingArea.Height / 2+ 225;
                this.Width = Screen.PrimaryScreen.WorkingArea.Width / 3;
                this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                xtraTabControl1.SelectedTabPageIndex = 3;

                string sql = "";
                sql = @"select top 5 s.pkSatislar as FisNo,f.Firmaadi,
                        s.GuncellemeTarihi as Tarih,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as Fiyati from StokKarti sk with(nolock)
                        inner join  SatisDetay sd with(nolock) on sd.fkStokKarti=sk.pkStokKarti
                        inner join Satislar s with(nolock) on s.pkSatislar=sd.fkSatislar
                        inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
                        where s.Siparis=1 and s.fkSatisDurumu in(2,4,5,9) and sk.pkStokKarti=@fkStokKarti
                        order by s.GuncellemeTarihi desc";
                sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);
                gcMusteriSatislari.DataSource = DB.GetData(sql);

                sql = @"select top 5 a.pkAlislar as FisNo,f.Firmaadi,
a.GuncellemeTarihi as Tarih,ad.Adet,(ad.AlisFiyati-ad.iskontotutar) as Fiyati from StokKarti sk with(nolock)
inner join AlisDetay ad with(nolock) on ad.fkStokKarti=sk.pkStokKarti
inner join Alislar a with(nolock) on a.pkAlislar=ad.fkAlislar
inner join Tedarikciler f with(nolock) on f.pkTedarikciler=a.fkFirma
where a.Siparis=1 and  sk.pkStokKarti=@fkStokKarti
order by a.GuncellemeTarihi desc";
                sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);
                gcMusteriAlislari.DataSource = DB.GetData(sql);

                sql = @"select top 5 a.pkAlislar as FisNo,t.Firmaadi,
                        a.GuncellemeTarihi as Tarih,ad.Adet,(ad.SatisFiyati-ad.iskontotutar) as Fiyati from StokKarti sk with(nolock)
                        inner join AlisDetay ad with(nolock) on ad.fkStokKarti=sk.pkStokKarti
                        inner join Alislar a with(nolock) on a.pkAlislar=ad.fkAlislar
                        inner join Tedarikciler t with(nolock) on t.pkTedarikciler=a.fkFirma
                        where a.Siparis=1 and a.fkSatisDurumu in(2,4,5) and a.fkFirma=@fkFirma and sk.pkStokKarti=@fkStokKarti
                        order by a.GuncellemeTarihi desc";
                sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);
                sql = sql.Replace("@fkFirma", pkfirma);
                gcMusteriSatisAlislari.DataSource = DB.GetData(sql);
            }
            simpleButton21.Focus();
            //Getir();

            
        }

        void Tasarimlar()
        {
            string Dosya = DB.exeDizini + "\\SatisFiyatlariAdetBazindaGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView7.RestoreLayoutFromXml(Dosya);
                //gridView7.ActiveFilter.Clear();
            }
        }
        void KartGetir()
        {
            DataTable dt = DB.GetData(@"SELECT StokKarti.StokKod, StokKarti.Barcode, StokKarti.Stokadi, StokKarti.Stoktipi, 
StokAltGruplari.StokAltGrup, StokGruplari.StokGrup,StokKarti.Mevcut
FROM         StokKarti LEFT JOIN
                      StokAltGruplari ON StokKarti.fkStokAltGruplari = StokAltGruplari.pkStokAltGruplari LEFT JOIN
                      StokGruplari ON StokAltGruplari.fkStokGruplari = StokGruplari.pkStokGrup 
where pkStokKarti=" + pkStokKarti.Text);
            if (dt.Rows.Count == 0) return;
            //stokkodu.Text = dt.Rows[0]["StokKod"].ToString();
            Stokadi.Text = dt.Rows[0]["Stokadi"].ToString();
            Barcode.Text = dt.Rows[0]["Barcode"].ToString();
            //Stoktipi.Text = dt.Rows[0]["Stoktipi"].ToString();
            //StokAltGrup.Text = dt.Rows[0]["StokAltGrup"].ToString();
            //StokGrup.Text = dt.Rows[0]["StokGrup"].ToString();
            teMevcut.Text = dt.Rows[0]["Mevcut"].ToString();
        }

        void SatisGetir()
        {
            string sql = @"SELECT sd.Tarih,s.pkSatislar as FisNo,s.OdemeSekli,sk.pkStokKarti, 
                sk.Barcode, sk.Stokadi, StokGruplari.StokGrup, StokAltGruplari.StokAltGrup, sk.Stoktipi, sd.SatisFiyati, 
                      sd.AlisFiyati, sd.iskontotutar, sd.Adet, 
                      sd.Adet * (sd.SatisFiyati - sd.iskontotutar) AS Tutar,
                      sd.Adet * ((sd.SatisFiyati-sd.AlisFiyati) - sd.iskontotutar) AS Kar, 
                      f.Firmaadi, 
                      Kullanicilar.KullaniciAdi
                    FROM Satislar s with(nolock)
                    INNER JOIN SatisDetay sd with(nolock) ON s.pkSatislar = sd.fkSatislar 
                    INNER JOIN StokKarti sk with(nolock) ON sd.fkStokKarti = sk.pkStokKarti 
                    INNER JOIN Firmalar f with(nolock) ON s.fkFirma = f.pkFirma 
                    INNER JOIN Kullanicilar ON s.fkKullanici = Kullanicilar.pkKullanicilar 
                    LEFT JOIN StokAltGruplari ON sk.fkStokAltGruplari = StokAltGruplari.pkStokAltGruplari 
                    LEFT JOIN StokGruplari ON sk.fkStokGrup = StokGruplari.pkStokGrup
                    WHERE s.Siparis = 1 and sk.pkStokKarti=@pkStokKarti";
                   
            ArrayList list = new ArrayList();
            if (ilktarih.EditValue != null && sontarih.EditValue != null)
            {
                sql = sql + " AND s.GuncellemeTarihi>=@ilktar AND s.GuncellemeTarihi<=@sontar";
                list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
                list.Add(new SqlParameter("@sontar", sontarih.DateTime));

            }

            sql=sql + " order by s.pkSatislar desc";

            list.Add(new SqlParameter("@pkStokKarti", pkStokKarti.Text));
            gridControl3.DataSource = DB.GetData(sql, list);
        }

        void AlisGetir()
        {
            string sql = @"SELECT AlisDetay.Tarih,Alislar.pkAlislar as FisNo,StokKarti.pkStokKarti, StokKarti.Barcode, StokKarti.Stokadi,
 StokGruplari.StokGrup, StokAltGruplari.StokAltGrup, StokKarti.Stoktipi, AlisDetay.SatisFiyati, 
                      AlisDetay.AlisFiyati, AlisDetay.iskontotutar, AlisDetay.Adet,
                      AlisDetay.Adet * AlisDetay.AlisFiyati as Tutar,AlisDetay.iskontotutar,
                      Tedarikciler.Firmaadi, 
                      Kullanicilar.KullaniciAdi
FROM Alislar 
INNER JOIN AlisDetay ON Alislar.pkAlislar = AlisDetay.fkAlislar 
INNER JOIN StokKarti ON AlisDetay.fkStokKarti = StokKarti.pkStokKarti 
INNER JOIN Tedarikciler ON Alislar.fkFirma = Tedarikciler.pkTedarikciler
INNER JOIN Kullanicilar ON Alislar.fkKullanici = Kullanicilar.pkKullanicilar 
LEFT JOIN StokAltGruplari ON StokKarti.fkStokAltGruplari = StokAltGruplari.pkStokAltGruplari 
LEFT JOIN StokGruplari ON StokKarti.fkStokGrup = StokGruplari.pkStokGrup
WHERE Alislar.Siparis = 1 and StokKarti.pkStokKarti=@pkStokKarti";

            ArrayList list = new ArrayList();
            if (ilktarih.EditValue != null && sontarih.EditValue != null)
            {
                sql = sql + " AND Alislar.GuncellemeTarihi>=@ilktar AND Alislar.GuncellemeTarihi<=@sontar";
                list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
                list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            }
            
            list.Add(new SqlParameter("@pkStokKarti", pkStokKarti.Text));
            gridControl1.DataSource = DB.GetData(sql, list);
        }

        void StokHareketleriAlisSatis()
        {
            string sql = "exec sp_StokHareketleri @ilktar,@sontar,@pkStokKarti";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            list.Add(new SqlParameter("@pkStokKarti", pkStokKarti.Text));

            gridControl2.DataSource = DB.GetData(sql, list);

        }

        void SatisAlisGetirGrafik()
        {
            string sql = "exec sp_StokHareketleri @ilktar,@sontar,@pkStokKarti";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            list.Add(new SqlParameter("@pkStokKarti", pkStokKarti.Text));

            pivotGridControl1.DataSource = DB.GetData(sql, list);

            pivotGridControl1.Cells.Selection = new Rectangle(0, 0, pivotGridControl1.Cells.ColumnCount - 1, pivotGridControl1.Cells.RowCount);  
        }

        private void sontarih_EditValueChanged(object sender, EventArgs e)
        {
            gridView3.ViewCaption = ilktarih.Text + " ile " + sontarih.Text + " arası satışlar";
            SatisGetir();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmStokTekSatislari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            simpleButton1_Click( sender,  e);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
                SatisGetir();
            if (xtraTabControl1.SelectedTabPageIndex == 1)
                AlisGetir();
            if (xtraTabControl1.SelectedTabPageIndex == 2)
                StokHareketleriAlisSatis();
            if(xtraTabControl1.SelectedTabPageIndex==4)
                SatisAlisGetirGrafik();
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
          if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["FisNo"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void gridView6_DoubleClick(object sender, EventArgs e)
        {
            if (gridView6.FocusedRowHandle < 0) return;
            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["FisNo"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;
            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["FisNo"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            simpleButton1_Click(sender, e);
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
            ilktarih.ToolTip = ilktarih.DateTime.ToString();

            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih.DateTime = d2.AddSeconds(sec2);
            sontarih.ToolTip = sontarih.DateTime.ToString();
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
            if (cbTarihAraligi.SelectedIndex == 0)// Bu gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
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

        private void stokKArtıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView7.FocusedRowHandle < 0) return;
            DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\SatisFiyatlariAdetBazindaGrid.xml";
            gridView7.SaveLayoutToXml(Dosya);

            gridView7.OptionsBehavior.AutoPopulateColumns = false;
            gridView7.OptionsCustomization.AllowColumnMoving = false;
            gridView7.OptionsCustomization.AllowColumnResizing = false;
            gridView7.OptionsCustomization.AllowQuickHideColumns = false;
            gridView7.OptionsCustomization.AllowRowSizing = false;
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView7.FocusedRowHandle < 0) return;

            DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void gridView7_DoubleClick(object sender, EventArgs e)
        {
            if (gridView7.FocusedRowHandle < 0) return;

            DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);

            DataTable dtSatisDetay = DB.GetData("select * from SatisDetay with(nolock) where pkSatisDetay="+_fkSatisDetay);
            if (dtSatisDetay.Rows.Count == 0) return;

            decimal SatisFiyati = Convert.ToDecimal(dtSatisDetay.Rows[0]["SatisFiyati"].ToString());
            decimal Fiyati = Convert.ToDecimal(dr["Fiyati"].ToString());
            decimal fark = 0;
            //if(SatisFiyati>Fiyati)
            //{
            //    fark = SatisFiyati - Fiyati;
            //}
            //else
                fark = SatisFiyati - Fiyati;
            decimal iskontotutar = fark; //Convert.ToDecimal(dr["iskontotutar"].ToString());

            decimal iskontoyuzdetutar = (fark*100)/ SatisFiyati;// Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@iskontotutar", iskontotutar));
            list.Add(new SqlParameter("@iskontoyuzdetutar", iskontoyuzdetutar));
            list.Add(new SqlParameter("@pkSatisDetay", _fkSatisDetay));
            
            DB.ExecuteSQL("update SatisDetay set iskontotutar=@iskontotutar,iskontoyuzdetutar=@iskontoyuzdetutar  where pkSatisDetay=@pkSatisDetay",list);

            Close();
            
        }
    }
}
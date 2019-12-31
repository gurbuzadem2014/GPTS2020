using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmHataliKayitlar : DevExpress.XtraEditors.XtraForm
    {
        public frmHataliKayitlar()
        {
            InitializeComponent();
        }

        private void frmLogs_Load(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 0;

           simpleButton1_Click(sender, e);

        }

        void Liste()
        {
            string sql = @"select kh.pkKasaHareket,kh.Tarih,kh.fkFirma,f.Firmaadi,kh.fkTedarikciler,kh.fkSatislar,s.pkSatislar,kh.Borc,kh.Alacak,kh.BilgisayarAdi from KasaHareket kh with(nolock)
left join  Satislar s with(nolock) on s.pkSatislar=kh.fkSatislar
left join Firmalar f with(nolock) on f.pkFirma=kh.fkFirma
where s.pkSatislar is null and kh.fkSatislar is not null";

            gridControl1.DataSource = DB.GetData(sql);
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            xtraTabControl1_SelectedPageChanged(sender, null);
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

            else if (cbTarihAraligi.SelectedIndex == 1)// Bu gün
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
            else if (cbTarihAraligi.SelectedIndex == 2)// dün
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
            else if (cbTarihAraligi.SelectedIndex == 3)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 5)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 6)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 7)// bu yıl
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
            else if (cbTarihAraligi.SelectedIndex == 8)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            Close();
        }
        void SatisVarSatisDetayYok_List()
        {
            gridControl2.DataSource = DB.GetData(@"select s.fkFirma,s.tarih,s.pkSatislar,f.firmaadi,s.ToplamTutar,s.OdemeSekli,s.eskifis,sd.fkStokKarti from Satislar  s with(nolock)
left join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
left join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
where sd.fkSatislar is null
and Siparis=1");
        }

        void AlislarVarAlisDetayYok_List()
        {
            gridControl3.DataSource = DB.GetData(@"select * from Alislar  a
left join Tedarikciler t on t.pkTedarikciler=a.fkFirma
left join AlisDetay ad on ad.fkAlislar=a.pkAlislar
left join KasaHareket kh on kh.fkAlislar=a.pkAlislar
where a.Siparis=1 and kh.fkAlislar is null");
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e == null) return;

            if (e.Page == xtraTabPage2)
                SatisVarSatisDetayYok_List();
            else if (e.Page == xtraTabPage3)
                AlislarVarAlisDetayYok_List();
            else if (e.Page == xtraTabPage4)
                iskontotutar_yuzde();
            else if (e.Page == xtraTabPage5)
                hatalibarkodlar();
            else if (e.Page == xTabMevcut)
                DepoMevcut();
            else if (e.Page == xtraTabPage6)
                iskontotutar_yuzdeAlis();
            else if (e.Page == xtraTabPage7)
                gridControl8.DataSource =
                DB.GetData("select pkFirma, Firmaadi, dbo.fon_MusteriBakiyesi(pkFirma) as HesaplananBakiye,Devir from Firmalar with(nolock)");
            else if (e.Page == xtraTabPage8)
                gridControl9.DataSource =
                DB.GetData(@"select sk.pkStokKarti,sk.Stokadi,Mevcut,skd.MevcutAdetDepo,isnull(ad.toplamAlis,0) as toplamAlis,isnull(sd.toplamsatis,0) as toplamsatis,isnull(ad.toplamAlis,0)-isnull(sd.toplamsatis,0) as fark from StokKarti sk
left join (select fkStokKarti,isnull(sum(Adet),0) as toplamAlis from AlisDetay group by fkStokKarti) ad on ad.fkStokKarti=sk.pkStokKarti
left join (select fkStokKarti,isnull(sum(Adet),0) as toplamsatis from SatisDetay group by fkStokKarti) sd on sd.fkStokKarti=sk.pkStokKarti
left join (select fkStokKarti,isnull(sum(MevcutAdet),0) as MevcutAdetDepo from StokKartiDepo where fkDepolar=1 group by fkStokKarti) skd on skd.fkStokKarti=sk.pkStokKarti");
            else if (e.Page == xtraTabPage9)
                gridControl10.DataSource = DB.GetData(@"select s.pkSatislar,f.pkFirma,f.Firmaadi,
convert(decimal(18, 2), sum((SatisFiyati - sd.iskontotutar) * Adet)) as HesaplananTutar,
convert(decimal(18, 2), max(s.ToplamTutar)) as ToplamTutar from SatisDetay sd
left join Satislar s on s.pkSatislar = sd.fkSatislar
left join Firmalar f on f.pkFirma = s.fkFirma
 where s.fkFirma > 1 and s.Siparis=1
 group by s.pkSatislar, f.pkFirma, f.Firmaadi
having(convert(decimal(18, 2), sum((SatisFiyati - sd.iskontotutar) * Adet)) - convert(decimal(18, 2), max(s.ToplamTutar))) > 0.10");
            else

                Liste();
        }
        void DepoMevcut()
        {
            gridControl6.DataSource = DB.GetData(@"select pkStokKarti,Barcode,Stokadi,Mevcut,dbo.fon_StokMevcut(pkStokKarti) as StokMevcut,Mevcut-[dbo].[fon_StokMevcut](pkStokKarti) as fark from StokKarti
            where Mevcut<>[dbo].[fon_StokMevcut](pkStokKarti)");
        }

        void hatalibarkodlar()
        {
            gridControl5.DataSource = DB.GetData("select * from StokKarti with(nolock) where len(Barcode)>13 or ISNUMERIC(Barcode)=0");
        }

        void iskontotutar_yuzde()
        {
            string sql = @"select pkSatisDetay,f.pkFirma,f.Firmaadi,SatisDetay.Tarih,fkSatislar,NakitFiyat,SatisFiyati,
iskontoyuzdetutar,
            cast(iskontotutar as money) as iskontotutar,
			cast(((SatisFiyati*iskontoyuzdetutar)/100) as money)  as isk_tutar,
			(cast(iskontotutar as money))-(cast(((SatisFiyati*iskontoyuzdetutar)/100) as money)) as fark,
			adet
			from SatisDetay with(nolock)
			left join Satislar s with(nolock) on s.pkSatislar=SatisDetay.fkSatislar
			left join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
            where 	cast(iskontotutar as money)<>cast(((SatisFiyati*iskontoyuzdetutar)/100) as money)";

            gridControl4.DataSource=  DB.GetData(sql);

        }

        void iskontotutar_yuzdeAlis()
        {
            string sql = @"select pkAlisDetay,t.pkTedarikciler,t.Firmaadi,ad.Tarih,ad.fkAlislar,AlisFiyati,NakitFiyat,SatisFiyati,
iskontoyuzdetutar,
            cast(iskontotutar as money) as iskontotutar,
			cast(((AlisFiyati*iskontoyuzdetutar)/100) as money)  as isk_tutar,
			(cast(iskontotutar as money))-(cast(((AlisFiyati*iskontoyuzdetutar)/100) as money)) as fark,
			adet
			from AlisDetay  ad with(nolock)
			left join Alislar a with(nolock) on a.pkAlislar=ad.fkAlislar
			left join Tedarikciler t with(nolock) on t.pkTedarikciler=a.fkFirma
            where 	cast(iskontotutar as money)<>cast(((AlisFiyati*iskontoyuzdetutar)/100) as money)";

            gridControl7.DataSource = DB.GetData(sql);

        }
        
        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i= gridView1.FocusedRowHandle;
            
            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(i);

            DB.ExecuteSQL("delete from KasaHareket where pkKasahareket=" + dr["pkKasaHareket"].ToString());

            Liste();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (i < 0) return;

            string fkfirma = "0";

            DataRow dr = gridView1.GetDataRow(i);
            fkfirma = dr["fkFirma"].ToString();

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = fkfirma.ToString();
            CariHareketMusteri.Show();
        }

        private void silToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;

            if (i < 0) return;

            string pkSatislar = "0";

            DataRow dr = gridView2.GetDataRow(i);
            pkSatislar = dr["pkSatislar"].ToString();

            DB.ExecuteSQL("delete from Satislar where pkSatislar=" + pkSatislar);

            SatisVarSatisDetayYok_List();
        }

        private void alışSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView3.FocusedRowHandle;

            if (i < 0) return;

            string pkAlislar = "0";

            DataRow dr = gridView3.GetDataRow(i);
            pkAlislar = dr["pkAlislar"].ToString();

            DB.ExecuteSQL("delete from Alislar where pkAlislar=" + pkAlislar);

            AlislarVarAlisDetayYok_List();
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            FisNoBilgisi.fisno.Text = dr["pkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();

            AlislarVarAlisDetayYok_List();
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.Text = dr["fkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();

            iskontotutar_yuzde();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView6.FocusedRowHandle < 0) return;
            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);
            frmStokKarti sk = new frmStokKarti();
            sk.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            DB.pkStokKarti = int.Parse( dr["pkStokKarti"].ToString());
            sk.ShowDialog();
            
            DepoMevcut();
            
        }

        private void yüzdeİskontoGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = formislemleri.MesajBox("iskonto yüzdeye göre, iskonto tutar güncellensin mi?", "iskonto tutar güncelle", 3, 1);
            if (s == "0") return;

            for (int i = 0; i < gridView4.SelectedRowsCount; i++)
            {
                string v = gridView4.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView4.GetDataRow(int.Parse(v));
                string pkSatisDetay = dr["pkSatisDetay"].ToString();

                DB.ExecuteSQL("update SatisDetay set iskontotutar = (SatisFiyati * iskontoyuzdetutar) / 100 where pkSatisDetay =" + pkSatisDetay);
            }

            iskontotutar_yuzde();
        }

        private void fişDetayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            //FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["fkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void iskontolarıDüzeltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView7.SelectedRowsCount; i++)
            {
                string v = gridView7.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView7.GetDataRow(int.Parse(v));
                string pkAlisDetay = dr["pkAlisDetay"].ToString();

                DB.ExecuteSQL("update AlisDetay set iskontotutar = (AlisFiyati * iskontoyuzdetutar) / 100 where pkAlisDetay =" + pkAlisDetay);
            }

            iskontotutar_yuzdeAlis();
        }

        private void tedarikçiHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView7.FocusedRowHandle < 0) return;
            DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);

            frmTedarikcilerHareketleri CariHareketMusteri = new frmTedarikcilerHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkTedarikciler"].ToString();
            CariHareketMusteri.Show();
        }

        private void fişDetayToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView7.FocusedRowHandle < 0) return;
            DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            //FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["fkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void iskontoTutarSıfırlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = formislemleri.MesajBox("iskonto yüzde sıfırlansın mı?", "iskonto yüzde sıfırla", 1, 3);
            if (s == "0") return;

            for (int i = 0; i < gridView4.SelectedRowsCount; i++)
            {
                string v = gridView4.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView4.GetDataRow(int.Parse(v));
                string pkSatisDetay = dr["pkSatisDetay"].ToString();

                DB.ExecuteSQL("update SatisDetay set iskontoyuzdetutar=0,iskontotutar=0 where pkSatisDetay =" + pkSatisDetay);
            }

            iskontotutar_yuzde();
        }

        private void iskontoYüzdeSıfırTutarVarİseYüzdeGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = formislemleri.MesajBox("iskonto yüzde sıfır olan kayıtlar, tutara göre güncellensin mi?", "iskonto yüzde günceller", 1, 3);
            if (s == "0") return;

            //for (int i = 0; i < gridView4.SelectedRowsCount; i++)
            //{
            //    string v = gridView4.GetSelectedRows().GetValue(i).ToString();

            //    DataRow dr = gridView4.GetDataRow(int.Parse(v));
            //    string pkSatisDetay = dr["pkSatisDetay"].ToString();

            //    DB.ExecuteSQL("update SatisDetay set iskontoyuzdetutar=0,iskontotutar=0 where pkSatisDetay =" + pkSatisDetay);
            //}

            // iskontotutar_yuzde();
            DB.ExecuteSQL(@"update SatisDetay set iskontoyuzdetutar=(iskontotutar*100)/SatisFiyati
where iskontoyuzdetutar = 0 and iskontotutar <> 0
and iskontotutar > 0");
            DB.ExecuteSQL(@"update SatisDetay set iskontoyuzdetutar=(iskontotutar*100)/SatisFiyati
where iskontoyuzdetutar = 0 and iskontotutar <> 0 and SatisFiyati > 0
and iskontotutar < 0");

            iskontotutar_yuzde();
        }

        private void silToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //if (gridView7.FocusedRowHandle < 0) return;

            //string s = formislemleri.MesajBox("Alış Silinsin mi?", "Alış Detay Boş ise Sil", 1, 3);
            //if (s == "0") return;
            //DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);
            //string pkAlislar = dr["pkAlislar"].ToString();

            
            //if(DB.GetData("select * from AlisDetay with(nolock) where fkAlislar=" + pkAlislar).Rows.Count==0)
            //{
            //    DB.ExecuteSQL("delete from Alislar where pkAlislar=" + pkAlislar);
            //}

            //iskontotutar_yuzdeAlis();
//            delete from Alislar where pkAlislar in(
//select pkAlislar from Alislar a
//left join AlisDetay ad on ad.fkAlislar = a.pkAlislar
//where ad.fkAlislar is null and a.ToplamTutar = 0)
        }

        private void müşteriHareketleriToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int i = gridView10.FocusedRowHandle;

            if (i < 0) return;

            string fkfirma = "0";

            DataRow dr = gridView10.GetDataRow(i);
            fkfirma = dr["pkFirma"].ToString();

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = fkfirma.ToString();
            CariHareketMusteri.Show();
        }

        private void fişDetayToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView10.FocusedRowHandle < 0) return;
            DataRow dr = gridView10.GetDataRow(gridView10.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            //FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void tutarDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView10.FocusedRowHandle < 0) return;
            DataRow dr = gridView10.GetDataRow(gridView10.FocusedRowHandle);

            DB.ExecuteSQL(@"update Satislar set ToplamTutar=sd.tutar  from 
(select fkSatislar, convert(decimal(18, 2), sum((sd.SatisFiyati - sd.iskontotutar) * Adet)) as tutar from SatisDetay sd
group by fkSatislar) sd
where  Satislar.pkSatislar = sd.fkSatislar
and pkSatislar = "+ dr["pkSatislar"].ToString());
            simpleButton1.PerformClick();
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView6.FocusedRowHandle < 0) return;
            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();

            DepoMevcut();
        }

        private void eşitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView6.FocusedRowHandle < 0) return;
            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);

            string s = formislemleri.MesajBox(dr["Stokadi"].ToString()+" Görünen Stok Adet ile Hesaplanan Mevcut Eşitlensin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;          

            string id = dr["pkStokKarti"].ToString();

            DB.ExecuteSQL(@"update StokKarti set Mevcut=dbo.fon_StokMevcut(pkStokKarti)
            where Mevcut<>[dbo].[fon_StokMevcut](pkStokKarti) and pkStokKarti ="+id);
            DB.ExecuteSQL(@"update StokKartiDepo set MevcutAdet=dbo.fon_StokMevcut(fkStokKarti)
            where MevcutAdet<>[dbo].[fon_StokMevcut](fkStokKarti) and fkStokKarti="+id);
            DepoMevcut();
        }

        private void tümünüEşitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = formislemleri.MesajBox(gridView6.DataRowCount.ToString() + "Adet Görünen Stok Adet ile Hesaplanan Mevcut Eşitlensin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            for (int i = 0; i < gridView6.DataRowCount; i++)
            {
                DataRow dr = gridView6.GetDataRow(i);

                string id = dr["pkStokKarti"].ToString();

                DB.ExecuteSQL(@"update StokKarti set Mevcut=dbo.fon_StokMevcut(pkStokKarti)
                where Mevcut<>[dbo].[fon_StokMevcut](pkStokKarti) and pkStokKarti =" + id);
                DB.ExecuteSQL(@"update StokKartiDepo set MevcutAdet=dbo.fon_StokMevcut(fkStokKarti)
                where MevcutAdet<>[dbo].[fon_StokMevcut](fkStokKarti) and fkStokKarti=" + id);
            }
            DepoMevcut();
        }
    }
}
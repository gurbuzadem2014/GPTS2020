using System;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using System.Drawing.Drawing2D;
using System.Drawing;
using DevExpress.XtraEditors.Controls;
using DevExpress.Xpo;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.OleDb;
using System.Configuration;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using System.IO;
using GPTS.islemler;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class ucPersonelListesi : DevExpress.XtraEditors.XtraUserControl
    {
        public ucPersonelListesi()
        {
            InitializeComponent();  
        }
        
        private void ucPersonelListesi_Load(object sender, EventArgs e)
        {
            Subeler();

            //PersonelList();
            cbTarihAraligi.SelectedIndex = 5;

            string Dosya2 = DB.exeDizini + "\\PersonelListesiGrid.xml";

            if (File.Exists(Dosya2))
            {
                gridView1.RestoreLayoutFromXml(Dosya2);
                gridView1.ActiveFilter.Clear();
            }

            string Dosya = DB.exeDizini + "\\PersonelListesiHareketGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
        }

        private void PersonelList()
        {
            //int fksube = Degerler.fkSube;
            //int.TryParse(lueSubeler.EditValue.ToString(), out fksube);
            try 
            {
                string sql = @"select P.pkPersoneller,P.adi,P.soyadi,P.ceptel,P.tel,P.eposta,P.maasi,P.yemekucreti,P.yolucreti,P.agiucreti,
                P.isegiristarih,P.AyrilisTarihi,B.bolumadi,P.SonMaasTarihi,
                0 as Borc, 0 as Alacak, P.Bakiye,dbo.fon_PersonellerBakiyesi(P.pkPersoneller) as Kalan,fkSube,maas_gunu from Personeller P with(nolock)
                left join Bolumler B with(nolock) on B.pkBolumler=P.fkBolumler";

                if (lueSubeler.EditValue.ToString()=="0")
                    sql = sql + " where 1=1";
                else
                    sql = sql + " where isnull(p.fkSube,1)="+ lueSubeler.EditValue.ToString();

                if (Durumu.SelectedIndex == 0)
                    sql += " and P.AyrilisTarihi is null";
                if (Durumu.SelectedIndex == 1)
                    sql += " and P.AyrilisTarihi is not null";

                //sql = sql + " group by P.pkPersoneller,P.adi,P.soyadi,P.ceptel,P.tel,P.eposta,P.maasi,P.isegiristarih,P.AyrilisTarihi,B.bolumadi";

                gridControl2.DataSource = DB.GetData(sql);
            }
            catch (Exception ex)
            {

            }
        }

        private void PersonelDetay(int personelid)
        {
            try
            {
                string sql = "HSP_PersonelHareketleri '" + ilktarih.DateTime.ToString("yyyy-MM-dd 00:00:00") +
                    "','" + sontarih.DateTime.ToString("yyyy-MM-dd 23:59:59")+"',"+personelid;

                gCPerHareketleri.DataSource = DB.GetData(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int si =gridView1.FocusedRowHandle;
            if (si < 0) return;

            DataRow dr = gridView1.GetDataRow(si);

            string personel_id = dr["pkpersoneller"].ToString();

            frmPersonel personelkart = new frmPersonel(personel_id);
            personelkart.ShowDialog();

            PersonelList();

            gridView1.FocusedRowHandle = si;
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.PersonellerBaslik = DB.pkPersoneller.ToString() + " " +
                row["adi"].ToString() + " " + row["soyadi"].ToString();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(e.RowHandle);

            PersonelDetay(int.Parse(dr["pkpersoneller"].ToString()));

            DB.pkPersoneller = int.Parse(dr["pkpersoneller"].ToString());

            gridView2.ViewCaption = "Seçilen Personel :"+dr["adi"].ToString() +" "+ dr["soyadi"].ToString();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            if (!formislemleri.SifreIste()) return;

            string s = formislemleri.MesajBox("Personel Hareketi Silinsin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            int PkKasaHareket  = int.Parse(dr["pkKasaHareket"].ToString());

            DB.ExecuteSQL("DELETE FROM KasaHareket where PkKasaHareket="+ PkKasaHareket.ToString());

            PersonelList();

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr2 = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string personel_id = dr2["pkpersoneller"].ToString();

            PersonelDetay(int.Parse(personel_id));
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında!");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            gridControl2.ExportToXls("personellistesi.xls");
            System.Diagnostics.Process.Start("personellistesi.xls");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string personel_id = dr["pkpersoneller"].ToString();

            frmPersonel personelkart = new frmPersonel(personel_id);
            personelkart.ShowDialog();

            PersonelList();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            //this.Dispose();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmPersonel personelkart = new frmPersonel("0");
            DB.pkPersoneller = 0;
            personelkart.ShowDialog();

            PersonelList();
        }


        private void Durumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            PersonelList();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int si = gridView1.FocusedRowHandle;
            if ( si< 0) return;

            DataRow dr = gridView1.GetDataRow(si);

            string personel_id = dr["pkpersoneller"].ToString();

            frmPersonel personelkart = new frmPersonel(personel_id);
            personelkart.ShowDialog();

            PersonelList();

            gridView1.FocusedRowHandle=si;
        }

        private void btnAvans_Click(object sender, EventArgs e)
        {
            DB.pkPersoneller = 0;
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.pkPersoneller = int.Parse(dr["pkPersoneller"].ToString());

            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.Tag= "2";
            KasaCikis.pkPersonel.Text = dr["pkPersoneller"].ToString();
            KasaCikis.cbHareketTipi.SelectedIndex = 3;
            ///Ödeme Çıkışı
            KasaCikis.Tag = "2";
            KasaCikis.tEaciklama.Text = "Personel Avansı";
            KasaCikis.ShowDialog();

            PersonelList();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            DB.pkPersoneller = 0;

            if (gridView1.FocusedRowHandle < 0) return;
            
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.pkPersoneller = int.Parse(dr["pkPersoneller"].ToString());

            frmPersonelMaasTahakkuk MaasTahakkuk = new frmPersonelMaasTahakkuk();
            MaasTahakkuk.ShowDialog();

            PersonelList();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmTekMaasHesapla MaasHesapla = new frmTekMaasHesapla();
            MaasHesapla.ShowDialog();

            PersonelList();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmPersonelMaasOdemesi mo = new frmPersonelMaasOdemesi();
            mo.ShowDialog();

            PersonelList();
        }

        private void btnGenelBakis_Click(object sender, EventArgs e)
        {
            DB.pkPersoneller = 0;
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.pkPersoneller = int.Parse(dr["pkPersoneller"].ToString());

            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.Tag = "2";
            KasaCikis.pkPersonel.Text = dr["pkPersoneller"].ToString();
            KasaCikis.cbHareketTipi.SelectedIndex = 3;
            ///Ödeme Çıkışı
            KasaCikis.Tag = "2";
            KasaCikis.tEaciklama.Text = "Personel Kesintisi";
            KasaCikis.cbAktifHesap.Checked = false;
            KasaCikis.ShowDialog();

            PersonelList();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            DB.pkPersoneller = 0;
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.pkPersoneller = int.Parse(dr["pkPersoneller"].ToString());

            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.Tag = "1";
            KasaCikis.cbGelirMi.Checked = false;
            KasaCikis.pkPersonel.Text = dr["pkPersoneller"].ToString();
            KasaCikis.cbHareketTipi.SelectedIndex = 3;
            ///Ödeme Girişi
            KasaCikis.Tag = "1";
            KasaCikis.tEaciklama.Text = "Personel Ek Ödeme";
            KasaCikis.cbAktifHesap.Checked = false;
            KasaCikis.cbGelirMi.Checked = false;
            KasaCikis.ShowDialog();

            PersonelList();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında....");
            //return;
            DB.pkPersoneller = 0;

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.pkPersoneller = int.Parse(dr["pkPersoneller"].ToString());

            frmPersonelHareketleri PersonelHareketleri = new frmPersonelHareketleri(dr["pkPersoneller"].ToString());
            PersonelHareketleri.Show();
        }

        private void vardiyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVardiyaCizelgesi vardiya = new frmVardiyaCizelgesi();
            vardiya.Show();
        }

        private void araçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAracTakip AracTakip = new frmAracTakip();
            AracTakip.ShowDialog();
        }

        private void personelBakiyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmPersonelBakiyeDuzeltme PersonelBakiyeDuzeltme = new frmPersonelBakiyeDuzeltme(dr["pkPersoneller"].ToString());
            PersonelBakiyeDuzeltme.Show();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;
            string Dosya = DB.exeDizini + "\\PersonelListesiHareketGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\PersonelListesiHareketGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void iştenAyrılmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmAyrilmaNedeni AyrilmaNedeni = new frmAyrilmaNedeni();
            AyrilmaNedeni.pkpersoneller.Text = dr["pkpersoneller"].ToString();
            AyrilmaNedeni.ShowDialog();
            PersonelList();
        }

        private void tekrarİşeGirişToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void durumuAktifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            string secilen = "0";

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            secilen = dr["pkPersoneller"].ToString();

            string sec = formislemleri.MesajBox(dr["adi"].ToString() + " " + dr["soyadi"].ToString() + " Personel Aktif Yapılsın mı?", "Personel Durumu", 3, 2);
            if (sec == "0") return;


            DB.ExecuteSQL("UPDATE  Personeller SET AyrilisTarihi=NULL where pkPersoneller=" + secilen);

            formislemleri.Mesajform("Personel Aktif Yapıldı", "S", 100);

            PersonelList();
        }

        private void personelSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string secilen = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            secilen = dr["pkPersoneller"].ToString();
            if (DB.GetData("select fkPersoneller from KasaHareket with(nolock) where fkPersoneller = " + secilen).Rows.Count > 0)
            {
                formislemleri.Mesajform("Personel Hareket Gördüğü için Silinemez", "K", 100);
                return;
            }

            string sec = formislemleri.MesajBox(dr["adi"].ToString() + " " + dr["soyadi"].ToString() + " Personel Silinsin mi?", "Sil", 3, 2);
            if (sec == "0") return;

            DB.ExecuteSQL("delete from PersonelResim where fkPersonel=" + secilen);
            
            DB.ExecuteSQL("delete from Personeller where pkPersoneller=" + secilen);

            formislemleri.Mesajform("Personel Silindi", "S", 100);

            PersonelList();
        }

        private void lSube_Click(object sender, EventArgs e)
        {
            frmSubeler subeler = new frmSubeler();
            subeler.ShowDialog();

            Subeler();
        }

        void Subeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData("select 0 pkSube,'Tümü' as sube_adi,'Tümü' as aciklama,getdate() as tarih,0 as aktif,'' as yetkili " +
                "union all select * from Subeler with(nolock)");
            lueSubeler.EditValue = Degerler.fkSube;
        }

        private void lueSubeler_EditValueChanged(object sender, EventArgs e)
        {
            PersonelList();
        }

        private void sutunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView2.ShowCustomization();
            gridView2.OptionsBehavior.AutoPopulateColumns = true;
            gridView2.OptionsCustomization.AllowColumnMoving = true;
            gridView2.OptionsCustomization.AllowColumnResizing = true;
            gridView2.OptionsCustomization.AllowQuickHideColumns = true;
            gridView2.OptionsCustomization.AllowRowSizing = true;
        }

        private void simpleButton8_Click_1(object sender, EventArgs e)
        {
            frmPersonelIzinleri izinler = new frmPersonelIzinleri("0");
            izinler.ShowDialog();
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimKaydet(gridView1, "PersonelListesiGrid");
        }

        private void varsayılanToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimSil("PersonelListesiGrid");
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

        private void tekrarİşeGirişiniYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string secilen = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            secilen = dr["pkPersoneller"].ToString();

            PersonelDetay(int.Parse(secilen));
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

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            //if (cbTarihAraligi.SelectedIndex == 0)// dün
            //{
            //    ilktarih.Properties.DisplayFormat.FormatString = "f";
            //    sontarih.Properties.EditFormat.FormatString = "f";
            //    ilktarih.Properties.EditFormat.FormatString = "f";
            //    sontarih.Properties.DisplayFormat.FormatString = "f";
            //    ilktarih.Properties.EditMask = "f";
            //    sontarih.Properties.EditMask = "f";

            //    sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
            //                      -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            //}
            if (cbTarihAraligi.SelectedIndex == 0)// dün
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
            else if (cbTarihAraligi.SelectedIndex == 2)// yarın
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


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
                                  0, 0, 1, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }

            else if (cbTarihAraligi.SelectedIndex == 8)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }

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

    }
}
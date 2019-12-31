using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using GPTS.islemler;
using System.Threading;
using DevExpress.XtraReports.UI;
using System.IO;
using GPTS.Include.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class frmMusteriAra : Form
    {
        public frmMusteriAra()
        {
            InitializeComponent();
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 100;

            pOzelNot.Width = memoozelnot.Width = this.Width / 4;
        }

        private void frmMusteriAra_Load(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select top 1 * from Sirketler with(nolock)");
            if (dt.Rows.Count>0)
            {
                if (dt.Rows[0]["BonusYuzde"].ToString()=="0")
                gcBonus.Visible = false;
            }
            seMaxKayit.Value = Degerler.select_top_firma;
            //boskengetir();

            string Dosya = DB.exeDizini + "\\MusteriAraGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
             simpleButton2_Click(sender, e);
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton2_Click(sender, e);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            DB.FirmaAdi = dr["Firmaadi"].ToString();
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            fkFirma.AccessibleDescription = dr["Firmaadi"].ToString();
            fkFirma.Tag = dr["pkFirma"].ToString();

            this.Tag = "1";

            Close();
            DB.ExecuteSQL("update Firmalar set tiklamaadedi=tiklamaadedi+1 where pkFirma=" + dr["pkFirma"].ToString());
            //this.Dispose();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Tag = "0";
            this.TopMost = true;
            Close();
        }


        void MusteriBakiyeGetir()
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null) return;
            decimal bakiye = 0;
            decimal.TryParse(dr["Bakiye"].ToString(),out bakiye);
            Satis1Toplam.Text = bakiye.ToString("##0.00");

            //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());

           // DataTable dt = DB.GetData("exec sp_MusteriBakiyesi " + DB.PkFirma.ToString() + ",0");
            //if (dt.Rows.Count == 0)
            //{
                //Satis1Toplam.Text = "0,00";
            //}
            //else
           // {
               //decimal bakiye = decimal.Parse(dt.Rows[0][0].ToString());
               //Satis1Toplam.Text = bakiye.ToString("##0.00");//dt.Rows[0][0].ToString();
            //}
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //OzelNot_Getir();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            adiara.Properties.EditValueChangedDelay = 10;

            frmMusteriKarti MusteriKarti = new frmMusteriKarti("0", adiara.Text);
            DB.PkFirma = 0;
            MusteriKarti.ShowDialog();

            
            if (MusteriKarti.txtMusteriAdi.Text != "")
            {
                adiara.Text = "ara";//delay süresi için
                adiara.Text = MusteriKarti.txtMusteriAdi.Text;
                adiara.Focus();
                gridView1.Focus();
            }
            else
            {
                adiara_EditValueChanged(sender, e);
                adiara.Focus();
            }
            adiara.Properties.EditValueChangedDelay = 400;
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string KaraListe = View.GetRowCellDisplayText(e.RowHandle, View.Columns["KaraListe"]).ToString();
            //    if (KaraListe.ToString() != "Seçim Yok")
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //    }

            //}
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
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

        private void adiara_EditValueChanged(object sender, EventArgs e)
        {
            if (barkodara.Text == "" && adiara.Text == "")
                boskengetir();
            else 
                adindanara();
        }

        private void barkodara_EditValueChanged(object sender, EventArgs e)
        {
            if (barkodara.Text == "" && adiara.Text == "")
                boskengetir();
            else 
                barkoddanara();
        }

        void boskengetir()
        {
            string sql = "SELECT top "+ seMaxKayit.Value.ToString()+ @" CONVERT(bit, '0') AS Sec, Firmalar.pkFirma, Firmalar.Firmaadi,
                    Firmalar.Yetkili, Firmalar.LimitBorc, Firmalar.KaraListe, 
                    FirmaGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi,
                    convert(int,Firmalar.OzelKod) as OzelKod ,Bonus,FirmaAltGruplari.FirmaAltGrupAdi,
                    --,dbo.fon_MusteriBakiyesi(Firmalar.pkFirma)
                    Devir as Bakiye,Tel as TelefonNo,Cep,Firmalar.Adres,Firmalar.SonSatisTarihi,Firmalar.KayitTarihi,
                    Firmalar.AnlasmaTarihi,Firmalar.AnlasaBitisTarihi
                    FROM  Firmalar with(nolock) 
                    LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '1') AS il ON Firmalar.fkil = il.KODU 
                    LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '2') AS ilce ON Firmalar.fkil = ilce.ALTGRUP AND Firmalar.fkilce = ilce.KODU 
                    LEFT JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari 
                    LEFT JOIN FirmaAltGruplari with(nolock) ON Firmalar.fkFirmaAltGruplari = FirmaAltGruplari.pkFirmaAltGruplari
                    where Firmalar.Aktif=1
                    ORDER BY Firmalar.tiklamaadedi DESC";
            gridControl1.DataSource = DB.GetData(sql);

            MusteriBakiyeGetir();
        }

        void barkoddanara()
        {
            string sql = @"SELECT CONVERT(bit, '0') AS Sec, Firmalar.pkFirma, Firmalar.Firmaadi,Firmalar.Yetkili, Firmalar.LimitBorc, Firmalar.KaraListe, 
                    FirmaGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi,
                    convert(int,Firmalar.OzelKod) as OzelKod ,Bonus,FirmaAltGruplari.FirmaAltGrupAdi,
                    --,dbo.fon_MusteriBakiyesi(Firmalar.pkFirma)
                    Firmalar.Devir as  Bakiye,Tel as TelefonNo,Cep,Firmalar.Adres,Firmalar.SonSatisTarihi
                    FROM  Firmalar with(nolock)
                    LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '1') AS il ON Firmalar.fkil = il.KODU 
                    LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '2') AS ilce ON Firmalar.fkil = ilce.ALTGRUP AND Firmalar.fkilce = ilce.KODU 
                    LEFT JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari 
                    LEFT JOIN FirmaAltGruplari with(nolock) ON Firmalar.fkFirmaAltGruplari = FirmaAltGruplari.pkFirmaAltGruplari
                    where Firmalar.Aktif=1";

            if (barkodara.Text !="" && barkodara.Text.IndexOf(" ") == 0)
                    sql = sql + " and OzelKod like '%" + barkodara.Text.Substring(1, barkodara.Text.Length - 1) + "'";
            else if (barkodara.Text.Length>0 && barkodara.Text.IndexOf(" ") == barkodara.Text.Length - 1)
                sql = sql + " and OzelKod like '" + barkodara.Text.Substring(0, barkodara.Text.Length - 1) + "%'";
            else if(barkodara.Text.Length>0)
                sql = sql + " and OzelKod='" + barkodara.Text + "'";

            gridControl1.DataSource = DB.GetData(sql);

            MusteriBakiyeGetir();
        }

        void adindanara()
        {
            //if (barkodara.Text.Length == 0 && adiara.Text.Length < 2) return;

            string s1 = "", s2 = "", s3 = "";
            string ara = adiara.Text;
            string[] dizi = ara.Split(' ');
            for (int i = 0; i < dizi.Length; i++)
            {
                if (i == 0) s1 = dizi[0];
                if (i == 1) s2 = dizi[1];
                if (i == 2) s3 = dizi[2];
            }
            string sql = @"SELECT CONVERT(bit, '0') AS Sec, Firmalar.pkFirma, Firmalar.Firmaadi,Firmalar.Yetkili, Firmalar.LimitBorc, Firmalar.KaraListe, 
                   FirmaGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi,
                   convert(int,Firmalar.OzelKod) as OzelKod ,Bonus,FirmaAltGruplari.FirmaAltGrupAdi,
                   --,dbo.fon_MusteriBakiyesi(Firmalar.pkFirma)
                   Firmalar.Devir as Bakiye,Tel as TelefonNo,Cep,Firmalar.Adres,Firmalar.SonSatisTarihi,Firmalar.AnlasmaTarihi,
                   Firmalar.AnlasaBitisTarihi FROM  Firmalar with(nolock)
                   LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH with(nolock)  WHERE GRUP = '1') AS il ON Firmalar.fkil = il.KODU 
                   LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '2') AS ilce ON Firmalar.fkil = ilce.ALTGRUP AND Firmalar.fkilce = ilce.KODU 
                   LEFT JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari 
                   LEFT JOIN FirmaAltGruplari with(nolock) ON Firmalar.fkFirmaAltGruplari = FirmaAltGruplari.pkFirmaAltGruplari
                   where Firmalar.Aktif=1";
            if (s1.Length > 0)
                sql = sql + " and Firmalar.Firmaadi like '%" + s1 + "%'";
            if (s2.Length > 0)
                sql = sql + " and Firmalar.Firmaadi like '%" + s2 + "%'";
            if (s3.Length > 0)
                sql = sql + " and Firmalar.Firmaadi like '%" + s3 + "%'";
            gridControl1.DataSource = DB.GetData(sql);


            MusteriBakiyeGetir();
        }

        private void barkodara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            adiara.Text = "";
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                DB.FirmaAdi = dr["Firmaadi"].ToString();
                DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
                simpleButton2_Click(sender, e);
                Close();
            }
            if (e.KeyCode == Keys.Right)
                adiara.Focus();

        }

        private void adiara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            barkodara.Text = "";
            if (e.KeyCode == Keys.Left && adiara.Text == "")
            {
                barkodara.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
                gridView1.FocusedRowHandle = 1;
            }
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton2_Click(sender, e);

                if(gridView1.DataRowCount==0)
                {
                    //YENİ MÜŞTERİ AÇ
                    btnYeni_Click(sender,  e);
                    adindanara();
                    //simpleButton2_Click(sender, e);
                }
            }
        }

        private void frmMusteriAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            //if (e.KeyCode == Keys.F7)
            //{
            //    btnYeni.Focus();
            //    btnYeni_Click(sender, e);
            //}
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());

            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i=gridView1.FocusedRowHandle ;
            if (i< 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = dr["pkFirma"].ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            adindanara();

            gridView1.FocusedRowHandle = i;

            MusteriBakiyeGetir();
        }

        private void ödemeVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = dr["pkFirma"].ToString();
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();

            adindanara();

            gridView1.FocusedRowHandle = i;

            MusteriBakiyeGetir();
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
                MusteriBakiyeGetir();
        }

        private void müşteriyiPasifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string s = formislemleri.MesajBox("Müşteri Pasif Yapılsın mı?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string so = DBislemleri.MusteriPasifYap(dr["pkFirma"].ToString());
            formislemleri.Mesajform(so, "S", 150);
        }

        private void devirBakiyeGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int i = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(i);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            string pkFirma = dr["pkFirma"].ToString();

            frmMusteriBakiyeDuzeltme DevirBakisiSatisKaydi = new frmMusteriBakiyeDuzeltme(pkFirma);
            DevirBakisiSatisKaydi.ShowDialog();

            adindanara();

            gridView1.FocusedRowHandle = i;

            MusteriBakiyeGetir();
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
                string _fkFirma = dr["pkFirma"].ToString();

                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + _fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\MusteriBakiyesi.repx");
                rapor.Name = "MusteriBakiyesi";
                rapor.Report.Name = "MusteriBakiyesi";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.Print();
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdir(false);
        }

        private void dizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdir(true);
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(e.FocusedRowHandle);
            string firmaid = "0";
            if (dr == null)
            {
                gcKontaklar.DataSource = null;
                gcSatislar.DataSource = null;
            }
            else
                firmaid = dr["pkFirma"].ToString();

            OzelNotGetir(firmaid);
            MusteriBakiyeGetir();
        }

        void KontaklarGetir(string FirmaId)
        {
            //gcKontaklar.Visible = false;
            gcKontaklar.DataSource = DB.GetData("select * from Kontaklar with(nolock) where fkFirma=" + FirmaId);
            if (gridView2.DataRowCount > 0)
                gcKontaklar.Visible = true;
        }

        void TaksitlerGetir(string firmaid)
        {
            //gcTaksitler.Visible = false;
            gcTaksitler.DataSource = DB.GetData(@"select tl.pkTaksitler,tl.SiraNo,tl.Tarih,tl.Odenecek-tl.Odenen as Kalan,tl.Aciklama,t.aciklama from Taksit t
            left join Taksitler tl on tl.taksit_id = t.taksit_id
            where tl.Odenecek-tl.Odenen>0 and t.fkFirma = " + firmaid);
            if (gridView3.DataRowCount > 0)
                gcTaksitler.Visible = true;
        }

        private void memoozelnot_Leave(object sender, EventArgs e)
        {
          if (gridView1.FocusedRowHandle < 0) return;

          DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

          if (memoozelnot.Tag.ToString() == "0")
          {
            string sonuc = DB.ExecuteScalarSQL("INSERT INTO  FirmaOzelNot (fkFirma,OzelNot)" +
                " VALUES(" + dr["pkFirma"].ToString() + ",'" + memoozelnot.Text + "') select IDENT_CURRENT('FirmaOzelNot')");

            memoozelnot.Tag = sonuc;
          }
          else
          {
            DB.ExecuteSQL("UPDATE FirmaOzelNot SET OzelNot='" + memoozelnot.Text + "'" +
            " WHERE pkFirmaOzelNot=" + memoozelnot.Tag.ToString());
          }
        }

        private void alanlarıKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriAraGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void alanlarıSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void seMaxKayit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DB.ExecuteSQL("update Kullanicilar set select_top_firma=" + seMaxKayit.Value.ToString()+
                    " where pkKullanicilar=" + DB.fkKullanicilar);

                Degerler.select_top_firma = int.Parse(seMaxKayit.Value.ToString());

                adiara.Focus();

                formislemleri.Mesajform("Gösterilecek Kayıt Değiştirildi", "S",100);
            }
        }

        private void seMaxKayit_EditValueChanged(object sender, EventArgs e)
        {
            if (adiara.Text=="" && barkodara.Text=="")
                boskengetir();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            btnYeniKontak_Click(sender, e);
        }

        private void ödemeHatırlatmaEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmHatirlatmaUzat uyar = new frmHatirlatmaUzat(DateTime.Now,DateTime.Now.AddHours(1),int.Parse(dr["pkFirma"].ToString()));
            DB.pkHatirlatmaAnimsat = 0;
            uyar.ShowDialog();
        }

        private void taksitÖdeToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            string pkTaksitler = dr["pkTaksitler"].ToString();


            if (gridView1.FocusedRowHandle < 0) return;

            DataRow drMusteri = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma = drMusteri["pkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = pkFirma;
            KasaGirisi.pkTaksitler.Text = pkTaksitler;
            KasaGirisi.tEaciklama.Text = dr["Tarih"].ToString() + "-Taksit Ödemesi-" + dr["Kalan"].ToString();

            decimal kalan = 0;
            decimal.TryParse(dr["Kalan"].ToString(), out kalan);
            KasaGirisi.ceTutarNakit.Value = kalan;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            
            
            TaksitlerGetir(pkFirma);
            
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView3.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            ftmTaksitKarti taksit = new ftmTaksitKarti(dr["pkTaksitler"].ToString());
            taksit.Text = dr["SiraNo"].ToString() + ". Taksit Bilgisi";
            taksit.ShowDialog();

            TaksitlerGetir(dr["pkFirma"].ToString());

            gridView3.FocusedRowHandle = i;
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            //memoozelnot.Text = "";
            //gridControl1.DataSource = null;
            //gridControl4.DataSource = null;
            gcSatislar.DataSource = null;
            gcTaksitler.DataSource = null;
            //gcTaksitler.Visible = false;
            gcKontaklar.DataSource = null;
            //gcKontaklar.Visible = false;


            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            //if (ghi.Column == null) return;
            ////GridView View = sender as GridView;
            //filtir ise
            if (ghi.RowHandle == -999997) return;

            if (ghi.InRowCell)
            {
                int rowHandle = ghi.RowHandle;
                //DevExpress.XtraGrid.Columns.GridColumn column = ghi.Column;
                if (ghi.Column.FieldName == "pkFirma")
                {
                    DataRow dr = gridView1.GetDataRow(rowHandle);
                    string _pkFirma = dr["pkFirma"].ToString();


                    SatisBilgileriGetir(_pkFirma);

                    KontaklarGetir(_pkFirma);
                    TaksitlerGetir(_pkFirma);
                    OzelNotGetir(_pkFirma);
                    //ResimGetir(_pkStokKarti);

                    //AciklamaGetir(_pkStokKarti);
                }
            }
        }

        void SatisBilgileriGetir(string Firmaid)
        {
            string sql = @"SELECT TOP (30) s.pkSatislar, s.GuncellemeTarihi, s.ToplamTutar,
            sd.durumu FROM  Satislar s with(nolock)
            inner join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu
            WHERE  s.fkSatisDurumu not in(10) and Siparis=1 and s.fkFirma =" + Firmaid;

            gcSatislar.DataSource = DB.GetData(sql);
        }

        private void btnYeniKontak_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmKontaklar Kontaklar = new frmKontaklar(dr["pkFirma"].ToString());
            Kontaklar.ShowDialog();
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            //FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", dr["pkFirma"].ToString()));
                list.Add(new SqlParameter("@OzelNot", memoozelnot.Text));

                DataTable dt = DB.GetData("select * from FirmaOzelNot with(nolock) where fkFirma=" + dr["pkFirma"].ToString());
                if (dt.Rows.Count == 0)
                    DB.ExecuteSQL("INSERT INTO FirmaOzelNot (fkFirma,OzelNot) VALUES(@fkFirma,@OzelNot)", list);
                else
                    DB.ExecuteSQL("UPDATE FirmaOzelNot SET OzelNot=@OzelNot WHERE fkFirma=@fkFirma", list);
                memoozelnot.Tag = 0;
        }

        void OzelNotGetir(string firmaid)
        {
            DataTable dtNot =
            DB.GetData(@"SELECT pkFirmaOzelNot ,OzelNot  FROM FirmaOzelNot with(nolock)
                         where fkFirma=" + firmaid);

            if (dtNot.Rows.Count == 0)
            {
                memoozelnot.Tag = "0";
                memoozelnot.Text = "";
            }
            else
            {
                memoozelnot.Tag = dtNot.Rows[0]["pkFirmaOzelNot"].ToString();
                memoozelnot.Text = dtNot.Rows[0]["OzelNot"].ToString();
            }

            dtNot.Dispose();
        }
    }
}

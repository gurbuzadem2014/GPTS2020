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

namespace GPTS
{
    public partial class frmMusteriAraDokunmatik : Form
    {
        public frmMusteriAraDokunmatik()
        {
            InitializeComponent();
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 100;
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
            Close();
            DB.ExecuteSQL("update Firmalar set tiklamaadedi=tiklamaadedi+1 where pkFirma=" + dr["pkFirma"].ToString());
            //this.Dispose();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
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

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DataTable dtNot =
            DB.GetData(@"SELECT pkFirmaOzelNot ,OzelNot  FROM FirmaOzelNot with(nolock)
                         where fkFirma=" + dr["pkFirma"].ToString());

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

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmMusteriKarti MusteriKarti = new frmMusteriKarti("0", "");
            DB.PkFirma = 0;
            MusteriKarti.ShowDialog();
            boskengetir();
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
            string sql = "SELECT top "+ seMaxKayit.Value.ToString()+ @" CONVERT(bit, '0') AS Sec, Firmalar.pkFirma, Firmalar.Firmaadi,Firmalar.Yetkili, Firmalar.LimitBorc, Firmalar.KaraListe, 
                    FirmaGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi,
                    convert(int,Firmalar.OzelKod) as OzelKod ,Bonus,FirmaAltGruplari.FirmaAltGrupAdi,
                    --,dbo.fon_MusteriBakiyesi(Firmalar.pkFirma)
                    Devir as Bakiye,Tel as TelefonNo,Cep,Firmalar.Adres,Firmalar.SonSatisTarihi,Firmalar.KayitTarihi
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

            if (barkodara.Text.IndexOf(" ") == 0) sql = sql + " and OzelKod like '%" + barkodara.Text.Substring(1, barkodara.Text.Length - 1) + "'";
            else if (barkodara.Text.IndexOf(" ") == barkodara.Text.Length - 1) sql = sql + " and OzelKod like '" + barkodara.Text.Substring(0, barkodara.Text.Length - 1) + "%'";
            else
                sql = sql + " and OzelKod='" + barkodara.Text + "'";

            gridControl1.DataSource = DB.GetData(sql);

            MusteriBakiyeGetir();
        }

        void adindanara()
        {
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
                   Firmalar.Devir as Bakiye,Tel as TelefonNo,Cep,Firmalar.Adres,Firmalar.SonSatisTarihi
                   FROM  Firmalar with(nolock)
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
            }
        }

        private void frmMusteriAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.F7)
                btnYeni_Click(sender, e);
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
         if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text= DBislemleri.MusteriPasifYap(dr["pkFirma"].ToString());
            Mesaj.Show();
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
            MusteriBakiyeGetir();

            //sllep konulursa sorun çözülebilir
            KontaklarGetir();
            //Thread th = new Thread(new ThreadStart(KontaklarGetir));
            //th.Start();
        }
        void KontaklarGetir()
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            gcKontaklar.DataSource = DB.GetData("select * from Kontaklar with(nolock) where fkFirma=" + dr["pkFirma"].ToString());
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

                formislemleri.Mesajform("Gösterilecek Kayıt Kayıt Değiştirildi", "S",100);
            }
        }

        private void seMaxKayit_EditValueChanged(object sender, EventArgs e)
        {
            if (adiara.Text=="" && barkodara.Text=="")
                boskengetir();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmKontaklar Kontaklar = new frmKontaklar(dr["pkFirma"].ToString());
            Kontaklar.ShowDialog();
        }

        private void ödemeHatırlatmaEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmHatirlatmaUzat uyar = new frmHatirlatmaUzat(DateTime.Now,DateTime.Now.AddHours(1),int.Parse(dr["pkFirma"].ToString()));
            DB.pkHatirlatmaAnimsat = 0;
            uyar.ShowDialog();
        }

        private void keyboardcontrol1_UserKeyPressed(object sender, KeyboardClassLibrary.KeyboardEventArgs e)
        {
            adiara.Focus();
            SendKeys.Send(e.KeyboardKeyPressed);

        }

        private void adiara_Enter(object sender, EventArgs e)
        {
            keyboardcontrol1.Visible = true;
        }

        private void adiara_Leave(object sender, EventArgs e)
        {
            keyboardcontrol1.Visible = false;
        }
    }
}

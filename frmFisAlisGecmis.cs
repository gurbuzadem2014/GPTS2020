using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using GPTS.islemler;
using DevExpress.Utils;

namespace GPTS
{
    public partial class frmFisAlisGecmis : DevExpress.XtraEditors.XtraForm
    {
        bool FisDuzelt = false;
        public frmFisAlisGecmis(bool _FisDuzelt)
        {
            InitializeComponent();
            FisDuzelt = _FisDuzelt;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
        }

        void Odemeleri()
        {
            string sql = @"select Aciklama,OdemeSekli,SUM(Alacak) as Odenen from KasaHareket with(nolock)
                   where fkAlislar=@fkAlislar group by Aciklama,OdemeSekli
                   union all
                   select 'Açık Hesap' as Aciklama,'' as OdemeSekli,Odenecek from Taksitler with(nolock)
                   where fkSatislar=@fkAlislar";
                   sql = sql.Replace("@fkAlislar", fisno.Text);
                   gcOdemeler.DataSource = DB.GetData(sql);
        }

        private void frmFisNoBilgisi_Load(object sender, EventArgs e)
        {
            if (FisDuzelt == true)
                btnFisDuzenle.Visible = true;
            else
                btnFisDuzenle.Visible = false;

            AlislarveOdemeler();

            string sql = @"select 'Açık Hesap' as OdemeSekli,isnull(AcikHesap,0) as Odenen,Tarih,KdvDahil from Alislar with(nolock) where pkAlislar=@fkAlislar";
            sql = sql.Replace("@fkAlislar", fisno.Text);
            gcAcikHesap.DataSource = DB.GetData(sql);

            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            if (dr["KdvDahil"].ToString() == "False")
            {
                //gridColumn21.Visible = false;
                gridColumn22.Visible = true;
                gridColumn5.Visible = true;
            }
            else
            {
               // gridColumn21.Visible = false;
                gridColumn22.Visible = false;
                gridColumn5.Visible = false;
            }

            string Dosya = DB.exeDizini + "\\FisAlisGecmisGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }

            Yetkiler();
        }
        void Yetkiler()
        {
            string sql = @"SELECT ya.Yetki, p.Aciklama10,ya.Sayi,isnull(p.Aktif,0) as Aktif FROM  YetkiAlanlari ya with(nolock)  
            INNER JOIN Parametreler p with(nolock) ON ya.fkParametreler = p.pkParametreler
            WHERE p.fkModul =1 and  ya.fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);

            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string aciklama = dtYetkiler.Rows[i]["Aciklama10"].ToString();
                bool yetki = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                //string sayi = dtYetkiler.Rows[i]["Sayi"].ToString();

                if (aciklama == "AlisFiyati")
                {
                    gcAlisFiyatiKdvHaric.Visible = yetki;
                    gridColumn5.Visible = yetki;
                    gridColumn36.Visible = yetki;
                }
            }
        }

        void AlislarveOdemeler()
        {
            DataTable dtAlislar = DB.GetData("exec sp_Alislar " + fisno.Text);
            if (dtAlislar.Rows.Count == 0)
            {
                //Showmessage("Fiş Bulunamadı", "K");
                return;
            }

            SatisDurumu.Text = dtAlislar.Rows[0]["SatisDurumu"].ToString();
            //SatisDurumu.Tag = dtAlislar.Rows[0]["fkAlisDurumu"].ToString();
            SatisTarih.Text = dtAlislar.Rows[0]["Tarih"].ToString();
            txtGuncellemeTarihi.Text = dtAlislar.Rows[0]["GuncellemeTarihi"].ToString();
            KullaniciAdiSoyadi.Tag = dtAlislar.Rows[0]["fkKullanici"].ToString();
            KullaniciAdiSoyadi.Text = dtAlislar.Rows[0]["KullaniciAdiSoyadi"].ToString();

            

            //float Alinan = float.Parse(dtSatislar.Rows[0]["AlinanPara"].ToString());
            //FaturaNo.Text = Convert.ToDouble(Alinan).ToString("##0.00");
            //float SatisTutar = float.Parse(dtSatislar.Rows[0]["ToplamTutar"].ToString());
            float iskontoFaturaTutar = float.Parse(dtAlislar.Rows[0]["iskontoFaturaTutar"].ToString());
            //if (Alinan!=0)
            //    FaturaTarihi.Text = Convert.ToDouble(Alinan - SatisTutar).ToString("##0.00");
            ceiskontoTutar.EditValue = iskontoFaturaTutar;
            gridControl1.DataSource = DB.GetData("exec sp_AlisDetay " + fisno.Text + ",0");
            //
            if (gridColumn19.SummaryItem.SummaryValue == null)
                Satis4Toplam.Text = "0,0";
            else
            {
                float aratop = float.Parse(gridColumn19.SummaryItem.SummaryValue.ToString());
                Satis4Toplam.Text = Convert.ToDouble(aratop).ToString("##0.00");
            }

            memoEdit1.Text = dtAlislar.Rows[0]["Aciklama"].ToString();
            groupControl1.Tag = dtAlislar.Rows[0]["fkFirma"].ToString();
            groupControl1.Text = groupControl1.Tag.ToString() + "-" + dtAlislar.Rows[0]["Firmaadi"].ToString();
            FaturaNo.Text = dtAlislar.Rows[0]["FaturaNo"].ToString();
            if (dtAlislar.Rows[0]["FaturaTarihi"].ToString() != "")
                FaturaTarihi.Text = Convert.ToDateTime(dtAlislar.Rows[0]["FaturaTarihi"].ToString()).ToString("dd.MM.yyyy");

            if (dtAlislar.Rows[0]["Siparis"].ToString() != "True")
            {
                simpleButton3.Enabled = false;
                btnFisDuzenle.Enabled = false;
            }
            
            Odemeleri();
        }


        private void simpleButton21_Click(object sender, EventArgs e)
        {
            FisDuzelt = false;
            this.btnFisDuzenle.Tag = "0";
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //if (!formislemleri.SifreIste()) return;

            DataTable dtSatislar = DB.GetData("select * from Alislar with(nolock) where pkAlislar=" + fisno.Text);
            if (dtSatislar.Rows.Count == 0)
            {
                formislemleri.Mesajform("Fiş Bulunamadı.", "K", 200);
                return;
            }

            string fkSatisDurumu = dtSatislar.Rows[0]["fkSatisDurumu"].ToString();
            //string fkCek = dtSatislar.Rows[0]["fkCek"].ToString();
            string fkFirma = dtSatislar.Rows[0]["fkFirma"].ToString();

            if (DB.fkKullanicilar != "1")
            {
                if (KullaniciAdiSoyadi.Tag.ToString() != DB.fkKullanicilar)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Bu Fişi Düzenleme Yetkiniz Bulunmamaktadır.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Fişi Düzeltmek İstediğinize Eminmisiniz. \n Not:Fişin Ödemeleri Silienecektir.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            //hesapları geri al
            //string fkFirma = groupControl1.Tag.ToString();
            
            //alacak
            //DB.ExecuteSQL("UPDATE Tedarikciler SET Alacak=Alacak+" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkTedarikciler=" + fkFirma);
            
            //borç
            //DB.ExecuteSQL("UPDATE Tedarikciler SET Borc=Borc+" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkTedarikciler=" + fkFirma);
            
            //kasa hareketlerini sil
            int sonuc = 
            DB.ExecuteSQL_Sonuc_Sifir("DELETE FROM KasaHareket where fkAlislar=" + fisno.Text);
            if (sonuc != 0)
            {
                formislemleri.Mesajform("Hata Oluştu Alış Faturası Silinemedi","K",200);
                return;
            }

            sonuc = DB.ExecuteSQL_Sonuc_Sifir("UPDATE Alislar SET Siparis=0,ToplamTutar=0, DuzenlemeTarihi=getdate() where pkAlislar=" + fisno.Text);
            if (sonuc != 0)
            {
                formislemleri.Mesajform("Hata Oluştu", "K", 200);
                return;
            }

            if (fkSatisDurumu == "1" || fkSatisDurumu == "9" || fkSatisDurumu == "12")
            {
                FisDuzelt = true;
                this.btnFisDuzenle.Tag = "1";
                Close();
                return;
            }
            MevcutAlisGeriAl();
            MevcutDepoAlisGeriAl();

            #region Alış detaydaki Satış Fiyatlarını Güncelle
            string sql = @"update AlisDetay set SatisFiyati=sf.SatisFiyatiKdvli,satis_fiyati_sk=sf.SatisFiyatiKdvli  From SatisFiyatlari sf
            where AlisDetay.fkStokKarti=sf.fkStokKarti and sf.fkSatisFiyatlariBaslik=1 and AlisDetay.fkAlislar=@fkAlislar

            update AlisDetay set SatisFiyati2=sf.SatisFiyatiKdvli,satis_fiyati_sk=sf.SatisFiyatiKdvli  From SatisFiyatlari sf
            where AlisDetay.fkStokKarti=sf.fkStokKarti and sf.fkSatisFiyatlariBaslik=(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik with(nolock) where Tur=1) and AlisDetay.fkAlislar=@fkAlislar

            update AlisDetay set SatisFiyati2=sf.SatisFiyatiKdvli From SatisFiyatlari sf
            where AlisDetay.fkStokKarti=sf.fkStokKarti and sf.fkSatisFiyatlariBaslik=(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik with(nolock) where Tur=2) and AlisDetay.fkAlislar=@fkAlislar

            update AlisDetay set SatisFiyati3=sf.SatisFiyatiKdvli From SatisFiyatlari sf
            where AlisDetay.fkStokKarti=sf.fkStokKarti and sf.fkSatisFiyatlariBaslik=(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik with(nolock) where Tur=3) and AlisDetay.fkAlislar=@fkAlislar";

            sql = sql.Replace("@fkAlislar",fisno.Text);

            DB.ExecuteSQL(sql);
            #endregion


            FisDuzelt = true;
            this.btnFisDuzenle.Tag = "1";


            DB.ExecuteSQL("update Tedarikciler set Devir=Devir-a.AcikHesap from Alislar a where pkTedarikciler=" +
                groupControl1.Tag.ToString());

            Close();
        }

        void FisYazdir(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_AlisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Alislar " + fisid);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                //string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
                XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        void vYazdır(bool dizayn)
        {
            string YaziciAdi = "", YaziciDosyasi = "";
            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya  FROM SatisFisiSecimi where Sec=1");
            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(22,0);
                YaziciAyarlari.ShowDialog();
                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;
                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }
            if (YaziciAdi != "")
            {
                //satiskaydet(false, true);
                FisYazdir(dizayn, fisno.Text, YaziciDosyasi, YaziciAdi);
            }
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            vYazdır(false);
        }
        private void simpleButton18_Click(object sender, EventArgs e)
        {
            if (fisno.Text == DB.GetData("select mAX(pkAlislar) from Alislar").Rows[0][0].ToString())
            {
                MessageBox.Show("Son Kayıt");
                return;
            }
            int fno=0;
            int.TryParse(fisno.Text, out fno);
            fno = fno + 1;
            fisno.Text = fno.ToString();
            AlislarveOdemeler();
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            if (fisno.Text == "1")
            {
                MessageBox.Show("ilk Kayıt");
                return;
            }
            int fno = 0;
            int.TryParse(fisno.Text, out fno);
            fno = fno - 1;
            fisno.Text = fno.ToString();

            AlislarveOdemeler();
        }

        private void frmFisNoBilgisi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void gridView1_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;
            Rectangle rect = e.Bounds;
            ControlPaint.DrawBorder3D(e.Graphics, e.Bounds);
            Brush brush =
                e.Cache.GetGradientBrush(rect, e.Column.AppearanceHeader.BackColor,
                e.Column.AppearanceHeader.BackColor2, e.Column.AppearanceHeader.GradientMode);
            rect.Inflate(-1, -1);
            // Fill column headers with the specified colors.
            e.Graphics.FillRectangle(brush, rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            // Draw the filter and sort buttons.
            foreach (DevExpress.Utils.Drawing.DrawElementInfo info in e.Info.InnerElements)
            {
                try
                {
                    DevExpress.Utils.Drawing.ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
                }
                catch (Exception exp)
                {
                }
            }
            e.Handled = true;
        }

        private void ceiskontoyuzde_KeyDown(object sender, KeyEventArgs e)
        {
            float iskontofaturayuzde=0;
            float.TryParse(ceiskontoyuzde.EditValue.ToString(), out iskontofaturayuzde);

            //DB.ExecuteSQL();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string fkFirma = groupControl1.Tag.ToString();
            frmTedarikciAra TedarikciAra = new frmTedarikciAra();
            TedarikciAra.fkFirma.Tag = fkFirma;
            TedarikciAra.ShowDialog();
            //hatalı müşteriden al
            //alacak
            DB.ExecuteSQL("UPDATE Tedarikciler SET Alacak=Alacak-" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkTedarikciler=" + fkFirma);
            //borç TODO:para ödedimi ki?
            //DB.ExecuteSQL("UPDATE Tedarikciler SET Borc=Borc-" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkTedarikciler=" + fkFirma);
            fkFirma = TedarikciAra.fkFirma.Tag.ToString();
            //yeni müşteriye ekle
            DB.ExecuteSQL("UPDATE Tedarikciler SET Alacak=Alacak+" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkTedarikciler=" + fkFirma);
            //borç TODO:para ödedimi ki?
            //DB.ExecuteSQL("UPDATE Tedarikciler SET Borc=Borc+" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkTedarikciler=" + fkFirma);

            DB.ExecuteSQL("UPDATE Alislar SET fkFirma=" + fkFirma + " where pkAlislar=" + fisno.Text);
            DataTable dt = DB.GetData("select pkTedarikciler,Firmaadi,OzelKod from Tedarikciler where pkTedarikciler=" + fkFirma);
            groupControl1.Tag = dt.Rows[0]["pkTedarikciler"].ToString();
            groupControl1.Text = dt.Rows[0]["OzelKod"].ToString() + "-" + dt.Rows[0]["Firmaadi"].ToString();
            //kasa hareketlerini güncelle
            DB.ExecuteSQL("UPDATE KasaHareket SET fkFirma=" + fkFirma + " where fkAlislar=" + fisno.Text);
            TedarikciAra.Dispose();
        }

        void MevcutAlisGeriAl()
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                string fkStokKarti = dr["fkStokKarti"].ToString();
                string Mevcut = dr["Adet"].ToString().Replace(",",".");

                if (decimal.Parse(Mevcut)>0)
                   DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=isnull(Mevcut,0)-" + Mevcut +" where pkStokKarti=" +fkStokKarti);
                else
                    DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=isnull(Mevcut,0)+" + Mevcut.Replace("-","") + " where pkStokKarti=" + fkStokKarti);


                //stok kartını en son alış fiyatını güncelle
                DB.ExecuteSQL("UPDATE StokKarti SET SonAlisTarihi=AD.Tarih  From (select fkStokKarti,Max(Tarih) as Tarih "+
                " from AlisDetay with(nolock) group by fkStokKarti) AD "+
                 " where StokKarti.pkStokKarti=AD.fkStokKarti and pkStokKarti=" +fkStokKarti);
               //log ekle
                //DB.ExecuteSQL("INSERT INTO Logs (fkStokKarti,fkKullanicilar,Tarih,EskiDeger,YeniDeger,fkHareketTipi) "+
                //" VALUES("+fkStokKarti+"," + DB.fkKullanicilar + ",getdate(),1,"+Mevcut+","+Mevcut+",2)");
                //Hareket Ekle
                DB.ExecuteSQL("INSERT INTO StokHareket (fkStokKarti,fkKullanicilar,Tarih,EskiDeger,YeniDeger,fkHareketTipi) "+
                " VALUES("+fkStokKarti+"," + DB.fkKullanicilar + ",getdate(),"+Mevcut+","+Mevcut+",2)");
            }
        }

        void MevcutDepoAlisGeriAl()
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                string fkStokKarti = dr["fkStokKarti"].ToString();
                string fkDepolar = dr["fkDepolar"].ToString();
                decimal miktar = 0;
                string Adet = dr["Adet"].ToString().Replace(",", ".");

                decimal.TryParse(Adet, out miktar);

                if (miktar < 0)
                    DB.ExecuteSQL("UPDATE StokKartiDepo SET MevcutAdet=isnull(MevcutAdet,0)+" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                    " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar);
                else
                    DB.ExecuteSQL("UPDATE StokKartiDepo SET MevcutAdet=isnull(MevcutAdet,0)-" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                    " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar);


                //stok kartını en son alış fiyatını güncelle
                DB.ExecuteSQL("UPDATE StokKarti SET SonAlisTarihi=AD.Tarih  From (select fkStokKarti,Max(Tarih) as Tarih "+
                " from AlisDetay with(nolock) group by fkStokKarti) AD "+
                 " where StokKarti.pkStokKarti=AD.fkStokKarti and pkStokKarti=" +fkStokKarti);
               //log ekle
                //DB.ExecuteSQL("INSERT INTO Logs (fkStokKarti,fkKullanicilar,Tarih,EskiDeger,YeniDeger,fkHareketTipi) "+
                //" VALUES("+fkStokKarti+"," + DB.fkKullanicilar + ",getdate(),1,"+Mevcut+","+Mevcut+",2)");
                //Hareket Ekle
                //DB.ExecuteSQL("INSERT INTO StokHareket (fkStokKarti,fkKullanicilar,Tarih,EskiDeger,YeniDeger,fkHareketTipi) "+
                //" VALUES("+fkStokKarti+"," + DB.fkKullanicilar + ",getdate(),"+Mevcut+","+Mevcut+",2)");
            }
        }

        
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Fiş Silmek İstediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            DataTable dtSatislar = DB.GetData("select * from Alislar with(nolock) where pkAlislar=" + fisno.Text);
            if (dtSatislar.Rows.Count == 0)
            {
                formislemleri.Mesajform("Fiş Bulunamadı.", "K", 200);
                return;
            }

            string fkSatisDurumu = dtSatislar.Rows[0]["fkSatisDurumu"].ToString();

            if (fkSatisDurumu != "1" & fkSatisDurumu != "9" & fkSatisDurumu != "12")
            {
                MevcutAlisGeriAl();
                MevcutDepoAlisGeriAl();
            }

           

            int sonuc = -1;
            sonuc = DB.ExecuteSQL("DELETE FROM AlisDetay where fkAlislar=" + fisno.Text);
            sonuc = DB.ExecuteSQL("DELETE FROM KasaHareket where fkAlislar=" + fisno.Text);
            sonuc = DB.ExecuteSQL("DELETE FROM Alislar where pkAlislar=" + fisno.Text);

            DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama) "+
                " VALUES(2,'" + fisno.Text + "-Alış Fişi Silindi-Tutar="+ Satis4Toplam.Text.Replace(",",".")+ "',getdate(),1,'Alış Faturası Silindi.')");

            DB.ExecuteSQL("delete from iskontolar where fkAlisDetay in(select pkAlisDetay From AlisDetay where fkAlislar=" +
            fisno.Text + ")");

            FisDuzelt = false;
            Close();
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void etiketBasımıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Etiket Adedi Miktar Kadar mı olsun?", Degerler.mesajbaslik, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (secim == DialogResult.Cancel) return;
            string adetbir = "1", adet="1";
            if (secim == DialogResult.Yes) adetbir="0";
            
            string pkEtiketBas = "0";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["Stogaisle"].ToString() == "True")
                {
                    if (pkEtiketBas=="0")
                        pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'',0) SELECT IDENT_CURRENT('EtiketBas')");

                    if (adetbir == "0")
                        adet = dr["Adet"].ToString();
                    else
                        adet = "1";

                    DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," +
                        dr["fkStokKarti"].ToString() + "," + adet.ToString().Replace(",", ".") + "," +
                        dr["SatisFiyati"].ToString().Replace(",", ".") + ",getdate())");

                    //formislemleri.EtiketBas(dr["fkStokKarti"].ToString());
                }
            }

            if (pkEtiketBas != "0")
            {
                frmEtiketBas EtiketBas = new frmEtiketBas();
                EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
                EtiketBas.ShowDialog();
            }
            
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            AlisBilgileriGetir(e.FocusedRowHandle);
        }

        void AlisBilgileriGetir(int e)
        {
            if (e < 0)
            {
                gridControl4.DataSource = null;
                return;
            }
            DataRow dr = gridView1.GetDataRow(e);
            if (dr["fkStokKarti"].ToString() == "") return;
            DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            string sql = @"SELECT TOP (10) Alislar.pkAlislar, Alislar.Tarih,AlisDetay.AlisFiyati, Tedarikciler.Firmaadi, AlisDetay.Adet
FROM  Alislar with(nolock) INNER JOIN AlisDetay with(nolock) ON Alislar.pkAlislar = AlisDetay.fkAlislar 
INNER JOIN  Tedarikciler with(nolock) ON Alislar.fkFirma = Tedarikciler.pkTedarikciler
WHERE  Alislar.Siparis=1 and AlisDetay.fkStokKarti = @fkStokKarti
GROUP BY Alislar.pkAlislar, Alislar.Tarih, AlisDetay.AlisFiyati, Tedarikciler.Firmaadi, AlisDetay.Adet
ORDER BY Alislar.pkAlislar DESC";
            sql = sql.Replace("@fkStokKarti", DB.pkStokKarti.ToString());
            gridControl4.DataSource = DB.GetData(sql);
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["fkStokKarti"].ToString();
            StokHareketleri.ShowDialog();

            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0");
            //SatisUrunBazindaDetay.Tag = "2";
            //SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            //SatisUrunBazindaDetay.ShowDialog();
        }

        private void satışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti = dr["fkStokKarti"].ToString();
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.Tag = "1";
            SatisFiyatlari.pkStokKarti.Text = fkStokKarti;
            SatisFiyatlari.ShowDialog();
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vYazdır(true);
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\FisAlisGecmisGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\FisAlisGecmisGrid.xml";

             if (File.Exists(Dosya))
             {
                 File.Delete(Dosya);
             }
        }


        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string iade = View.GetRowCellDisplayText(e.RowHandle, View.Columns["iade"]);
            //    string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
            //    if (AcikSatisindex == 1)
            //        e.Appearance.BackColor = Satis1Toplam.BackColor;
            //    if (AcikSatisindex == 2)
            //        e.Appearance.BackColor = Satis2Toplam.BackColor;
            //    if (AcikSatisindex == 3)
            //        e.Appearance.BackColor = Satis3Toplam.BackColor;
            //    if (AcikSatisindex == 4)
            //        e.Appearance.BackColor = Satis4Toplam.BackColor;
            //    //if (iade.Trim() != "Seçim Yok")
            //    // {
            //    //   e.Appearance.BackColor = Color.DeepPink;
            //    //}
            //    if (Fiyat.Trim() == "0")
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //    }

            //}
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            AppearanceDefault appBlue = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorRed = new AppearanceDefault(Color.Red);
            AppearanceDefault appErrorGreen = new AppearanceDefault(Color.GreenYellow);
            //AppearanceDefault appErrorYellowGreen = new AppearanceDefault(Color.YellowGreen);
            //AppearanceDefault appErrorPink = new AppearanceDefault(Color.LightSkyBlue);//, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            //object val = gridView1.GetRowCellValue(e.RowHandle, e.Column);
            //if ((e.Column.FieldName == "UnitPrice" && !(bool)validationControl1.IsTrueCondition(val)[0])
            //  || (e.Column.FieldName == "Quantity" && !(bool)validationControl2.IsTrueCondition(val)[0])
            //|| (e.Column.FieldName == "Discount" && !(bool)validationControl3.IsTrueCondition(val)[0]))

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                //yesilisikyeni();
                return;
            }
            if (e.Column.FieldName == "AlisFiyati" && dr["Alisiskontolu"].ToString() != "" && dr["Adet"].ToString() != "" && dr["SatisFiyati"].ToString() != "")
            {
                decimal Alisiskontolu = Convert.ToDecimal(dr["Alisiskontolu"].ToString());
                decimal AlisFiyati_sk = Convert.ToDecimal(dr["AlisFiyati_sk"].ToString());
                
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                decimal AlisTutar = Convert.ToDecimal(dr["Alisiskontolu"].ToString()) * decimal.Parse(dr["Adet"].ToString());

                string iade = dr["iade"].ToString();

                if (Alisiskontolu != AlisFiyati_sk)
                    AppearanceHelper.Apply(e.Appearance, appBlue);

                if (SatisTutar - AlisTutar <= 0 && (iade == "False" || iade == ""))
                    AppearanceHelper.Apply(e.Appearance, appError);

                //if (SatisTutar - AlisTutar <= 0 && (iade == "False" || iade == "") && (AlisFiyati != AlisFiyati_sk))
                //    AppearanceHelper.Apply(e.Appearance, appErrorGreen);
            }
            
            if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["SatisFiyati"].ToString() != "")
            {
                decimal SatisFiyati = Convert.ToDecimal(dr["SatisFiyati"].ToString());
                decimal SatisFiyati_sk = Convert.ToDecimal(dr["SatisFiyati_sk"].ToString());

                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());

                string iade = dr["iade"].ToString();

                if (SatisFiyati != SatisFiyati_sk)
                    AppearanceHelper.Apply(e.Appearance, appBlue);
            }
            if (e.Column.FieldName == "iskontoyuzdetutar" && dr["iskontoyuzdetutar"].ToString() != "0,000000")
            {
                AppearanceHelper.Apply(e.Appearance, appBlue);
            }
            if (e.Column.FieldName == "Adet" && dr["Adet"].ToString() == "0")
            {
                AppearanceHelper.Apply(e.Appearance, appError);
            }           
        }

        private void excelGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\FisAlisGecmis" + fisno.Text + ".Xls";
            gridView1.ExportToXls(dosyaadi);
            System.Diagnostics.Process.Start(dosyaadi);
        }
    }
}
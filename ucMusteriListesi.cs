using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using System.IO;
using GPTS.Include.Data;
using DevExpress.Utils;
using System.Collections;
using System.Data.SqlClient;
using GPTS.islemler;
using System.Threading;
using System.Data.OleDb;

namespace GPTS
{
    public partial class ucMusteriListesi : DevExpress.XtraEditors.XtraUserControl
    {
        public ucMusteriListesi()
        {
            InitializeComponent();
            DB.FirmaAdi = "";
        }
        private void ucCariKartlar_Load(object sender, EventArgs e)
        {
            GosterilenAdet.Tag = DB.GetData("select count(*) from Firmalar with(nolock)").Rows[0][0].ToString();
            DataTable dtSirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
            if(dtSirket.Rows[0]["BonusYuzde"].ToString()=="0")
               gcBonus.Visible = false;

            //musteriara("0");
            boskengetir();
            adiara.Focus();


            string Dosya = DB.exeDizini + "\\MusteriListesiGrid.xml";
            if(File.Exists(Dosya))
                gridView3.RestoreLayoutFromXml(Dosya);

            gridView3.ClearColumnsFilter();
        }

        private void sMSGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            gridView3.ActiveFilterString = "Sec=1";
            int say = 0;
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);

               // if (dr["Sec"].ToString() == "False") continue;

                string pkFirma = dr["PkFirma"].ToString();
                DataTable dt = DB.GetData("select pkFirma,Cep from Firmalar with(nolock) where len(Cep)>9 and pkFirma=" + pkFirma);
                if (dt.Rows.Count > 0)
                {
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkFirma", pkFirma));
                    list.Add(new SqlParameter("@CepTel", dt.Rows[0]["Cep"].ToString()));
                    list.Add(new SqlParameter("@Mesaj", "Mesaj"));
                    if (DB.GetData("select * from Sms with(nolock) where Durumu=0 and fkFirma=" + pkFirma).Rows.Count == 0)
                        DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CepTel,Durumu,Mesaj,Tarih) values(@fkFirma,@CepTel,0,@Mesaj,GetDate())", list);
                }
                else
                    say++;
            }
            MessageBox.Show("");
            gridView3.ClearColumnsFilter();

            frmSmsGonder SmsGonder = new frmSmsGonder();
            SmsGonder.Show();
        }

        void SonAlisverisleri(string PkFirma)
        {
            string sql = @"SELECT  top 10 pkSatislar, Tarih, ToplamTutar AS Tutar
FROM Satislar with(nolock) where Siparis=1 and fkSatisDurumu<>10 and fkFirma=@fkFirma order by Satislar.Tarih desc";

            sql=sql.Replace("@fkFirma", PkFirma);
gcSonAlisVeris.DataSource= DB.GetData(sql);
        }

        private void hareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        void OdemeAl()
        {
            int i = gridView3.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            DB.pkKasaHareket = 0;
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = dr["pkFirma"].ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            gridrowgetir(gridView3.FocusedRowHandle);
        }
        void OdemeYap()
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            DB.pkKasaHareket = 0;
            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.pkFirma.Text = dr["pkFirma"].ToString();
            KasaCikis.Tag = "2";
            KasaCikis.ShowDialog();

            gridrowgetir(gridView3.FocusedRowHandle);
        }
        private void btnEPosta_Click(object sender, EventArgs e)
        {
            int i = gridView3.FocusedRowHandle;
            OdemeYap();

            if (barkodara.Text == "")
                adindanara();
            else
                barkoddanara();

            gridView3.FocusedRowHandle=i;

        }
        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));
        }
        public virtual void InitPrintTool(PrintTool tool)
        {
            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);
        }
        PrintingSystem printingSystem = null;
        PrintingSystem Printing
        {
            get
            {
                if (printingSystem == null) printingSystem = new PrintingSystem();
                return printingSystem;
            }
        }
        
        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gridControl1;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }
        
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            yazdir();
        }
        
        private void btnYeni_Click(object sender, EventArgs e)
        {
            DB.PkFirma = 0;
            frmMusteriKarti kart = new frmMusteriKarti("0", "");
            DB.pkStokKarti = 0;
            kart.ShowDialog();
            //int secilen = gridView3.FocusedRowHandle; 
            gridrowgetir(gridView3.FocusedRowHandle);

            adindanara();
            //gridView3.FocusedRowHandle = secilen;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int i = gridView3.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView3.GetDataRow(i);

            //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());

            frmMusteriKarti MusteriKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            MusteriKarti.pkFirma.Text = DB.PkFirma.ToString();
            MusteriKarti.ShowDialog();
           
            gridrowgetir(i);
            
            adindanara();
            gridView3.SelectRow(i);
            gridView3.FocusedRowHandle = i;
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmMusterilereGenelBakis musteriraporlari = new frmMusterilereGenelBakis();
            musteriraporlari.Show();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("gridView3.xml"))
                    File.Delete("gridView3.xml");

                    gridView3.SaveLayoutToXml("gridView3.xml");
            }
            catch
            {
            }
            this.Dispose();
            //int c = Application.OpenForms.Count;
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        void boskengetir()
        {
            string sql = @"SELECT sec as Sec, Firmalar.pkFirma, Firmalar.Firmaadi,Firmalar.Yetkili, Firmalar.LimitBorc, Firmalar.KaraListe, 
                      FirmaGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi
                      ,convert(int,Firmalar.OzelKod) as OzelKod ,Bonus,FirmaAltGruplari.FirmaAltGrupAdi,
0 as Bakiye,Firmalar.Tel,Firmalar.Cep,Firmalar.Adres,Firmalar.SonSatisTarihi,AnlasmaTarihi,AnlasaBitisTarihi,AnlasmaTutari,
datediff(day,isnull(Firmalar.SonSatisTarihi,getdate()),getdate()) as Gun,Firmalar.Devir,
Firmalar.SonOdemeTarihi,Firmalar.KayitTarihi,datediff(dd,SonOdemeTarihi,GETDATE()) as OdemeYapmadigiGun,Firmalar.Eposta
FROM  Firmalar with(nolock)
LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH with(nolock)  WHERE GRUP = '1') AS il ON Firmalar.fkil = il.KODU 
LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH with(nolock)  WHERE GRUP = '2') AS ilce ON Firmalar.fkil = ilce.ALTGRUP AND Firmalar.fkilce = ilce.KODU 
LEFT JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari 
LEFT JOIN FirmaAltGruplari with(nolock) ON Firmalar.fkFirmaAltGruplari = FirmaAltGruplari.pkFirmaAltGruplari";

            if (icbDurumu.SelectedIndex == 0) sql = sql + " where Firmalar.Aktif=1";
            if (icbDurumu.SelectedIndex == 1) sql = sql + " where Firmalar.Aktif=0"; 
            gridControl1.DataSource= DB.GetData(sql);
        }

        void barkoddanara()
        {
            string sql = @"SELECT sec as Sec, Firmalar.pkFirma, Firmalar.Firmaadi,Firmalar.Yetkili, Firmalar.LimitBorc, Firmalar.KaraListe, 
                      FirmaGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi
                      ,convert(int,Firmalar.OzelKod) as OzelKod ,Bonus,FirmaAltGruplari.FirmaAltGrupAdi,
0 as Bakiye,Firmalar.Tel,Firmalar.Cep,Firmalar.Adres,Firmalar.SonSatisTarihi,
datediff(day,isnull(Firmalar.SonSatisTarihi,getdate()),getdate()) as Gun,
Firmalar.Devir,Firmalar.SonOdemeTarihi,datediff(dd,SonOdemeTarihi,GETDATE()) as OdemeYapmadigiGun,Firmalar.Eposta
FROM  Firmalar with(nolock)
LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '1') AS il ON Firmalar.fkil = il.KODU 
LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '2') AS ilce ON Firmalar.fkil = ilce.ALTGRUP AND Firmalar.fkilce = ilce.KODU 
LEFT JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari 
LEFT JOIN FirmaAltGruplari with(nolock) ON Firmalar.fkFirmaAltGruplari = FirmaAltGruplari.pkFirmaAltGruplari
 where Firmalar.Aktif=1";

            if (barkodara.Text.IndexOf(" ") == 0) sql = sql + " and OzelKod like '%" + barkodara.Text.Substring(1,barkodara.Text.Length-1)+"'";
            else if (barkodara.Text.IndexOf(" ") == barkodara.Text.Length-1) sql = sql + " and OzelKod like '" + barkodara.Text.Substring(0, barkodara.Text.Length - 1) + "%'";
            else
                sql = sql + " and OzelKod='" + barkodara.Text + "'";
            gridControl1.DataSource = DB.GetData(sql);
        }

        void adindanara()
        {
            string s1="", s2="", s3="";
            string ara = adiara.Text;
            string[] dizi = ara.Split(' ');
            for (int i = 0; i < dizi.Length; i++)
            {
                if (i == 0) s1 = dizi[0];
                if (i == 1) s2 = dizi[1];
                if (i == 2) s3 = dizi[2];
            }
            string sql = @"SELECT sec as Sec, Firmalar.pkFirma, Firmalar.Firmaadi,Firmalar.Yetkili, Firmalar.LimitBorc, Firmalar.KaraListe, 
                      FirmaGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi
                      ,convert(int,Firmalar.OzelKod) as OzelKod ,Bonus,FirmaAltGruplari.FirmaAltGrupAdi,
0 as Bakiye,Firmalar.Tel,Firmalar.Cep,Firmalar.Adres,Firmalar.SonSatisTarihi,AnlasmaTarihi,AnlasaBitisTarihi,AnlasmaTutari,Firmalar.Devir,
datediff(day,isnull(Firmalar.SonSatisTarihi,getdate()),getdate()) as Gun,Firmalar.SonOdemeTarihi,Firmalar.KayitTarihi,
datediff(dd,SonOdemeTarihi,GETDATE()) as OdemeYapmadigiGun,Firmalar.Eposta
FROM  Firmalar with(nolock)
LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '1') AS il ON Firmalar.fkil = il.KODU 
LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH with(nolock) WHERE GRUP = '2') AS ilce ON Firmalar.fkil = ilce.ALTGRUP AND Firmalar.fkilce = ilce.KODU 
LEFT JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari 
LEFT JOIN FirmaAltGruplari with(nolock) ON Firmalar.fkFirmaAltGruplari = FirmaAltGruplari.pkFirmaAltGruplari";
            if (icbDurumu.SelectedIndex == 0) sql = sql + " where Firmalar.Aktif=1";
            if (icbDurumu.SelectedIndex == 1) sql = sql + " where Firmalar.Aktif=0";

            if (s1.Length > 0)
                sql = sql + " and Firmalar.Firmaadi like '%"+s1+"%'";
            if (s2.Length > 0)
                sql = sql + " and Firmalar.Firmaadi like '%" + s2 + "%'";
            if (s3.Length > 0)
                sql = sql + " and Firmalar.Firmaadi like '%" + s3 + "%'";
            gridControl1.DataSource = DB.GetData(sql);
        }

        private void cbTumu_CheckedChanged(object sender, EventArgs e)
        {
            string sql = @"SELECT CONVERT(bit, '0') AS Sec, Firmalar.pkFirma, Firmalar.Firmaadi,Firmalar.Yetkili, Firmalar.LimitBorc, Firmalar.KaraListe, 
                      FirmaGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi
                      ,convert(int,Firmalar.OzelKod) as OzelKod ,Bonus,FirmaAltGruplari.FirmaAltGrupAdi,
0 as Bakiye,Firmalar.Tel,Firmalar.Cep,Firmalar.Adres,Firmalar.SonSatisTarihi,
datediff(day,isnull(Firmalar.SonSatisTarihi,getdate()),getdate()) as Gun,Firmalar.Devir,Firmalar.SonOdemeTarihi,
datediff(dd,SonOdemeTarihi,GETDATE()) as OdemeYapmadigiGun,Firmalar.Eposta
FROM  Firmalar with(nolock)
LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH  WHERE GRUP = '1') AS il ON Firmalar.fkil = il.KODU 
LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH  WHERE GRUP = '2') AS ilce ON Firmalar.fkil = ilce.ALTGRUP AND Firmalar.fkilce = ilce.KODU 
LEFT JOIN FirmaGruplari ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari 
LEFT JOIN FirmaAltGruplari ON Firmalar.fkFirmaAltGruplari = FirmaAltGruplari.pkFirmaAltGruplari ";
            if (cbTumu.Checked)
                gridControl1.DataSource = DB.GetData(sql);
            //musteriara("1");
            //else
            //musteriara("0");
        }

        void tumunusec(bool sec)
        {
            int i = 0;
            while (gridView3.DataRowCount > i)
            {
                gridView3.SetRowCellValue(i, "Sec", sec);
                i++;
            }
        }
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
                gridView3.SelectAll();
            else
            {
                //gridView3.CancelSelection();
                gridView3.ClearSelection();
                //tumunusec(checkEdit1.Checked);
            }
        }
        int secilen = 0;
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            secilen = gridView3.FocusedRowHandle;
            
            OdemeAl();

            if (barkodara.Text == "")
                adindanara();
            else
                barkoddanara();

            gridView3.FocusedRowHandle = secilen;            
        }
        private void gridView3_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //if (e.RowHandle >= 0)
            //{
            //    DataRow dr = gridView3.GetDataRow(e.RowHandle);
            //    if (dr["Sec"].ToString() == "True")
            //    {
            //        e.Appearance.BackColor = Color.SlateBlue;

            //        e.Appearance.ForeColor = Color.White;

            //    }
            //} 
        }
        void gridrowgetir(int RowHandle)
        {
            if (RowHandle < 0) return;
            
            DataRow dr = gridView3.GetDataRow(RowHandle);
            string fkFirma = dr["pkFirma"].ToString();

            SonAlisverisleri(fkFirma);

            DataTable dt = DB.GetData(@"select isnull(OzelNot,'') as OzelNot from FirmaOzelNot with(nolock) where fkFirma=" + fkFirma);
            if (dt.Rows.Count == 0)
                memoEdit1.Text = "";
            else
                memoEdit1.Text = dt.Rows[0]["OzelNot"].ToString();
            
            decimal bakiye = 0;
            if(dr["Devir"].ToString()!="")
             bakiye = decimal.Parse(dr["Devir"].ToString());

            Satis1Toplam.Text = bakiye.ToString("##0.00");

            gridView3.FocusedRowHandle = RowHandle;
        }
        private void gridView3_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridrowgetir(e.FocusedRowHandle);
        }
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmMusteriTopluDegisiklik toplu  = new frmMusteriTopluDegisiklik();
            toplu.Show();
            //frmTaksitOdemeleri TaksitOdemeleri = new frmTaksitOdemeleri();
            //TaksitOdemeleri.ShowDialog();
            //gridrowgetir(e.FocusedRowHandle);
            //boskengetir();
            //musteriara("0");
        }
        private void gridView3_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            //for (int i = 0; i < gridView3.DataRowCount; i++)
            //{
            //    gridView3.SetRowCellValue(i, "Sec", false);
            //}

            //if (gridView3.SelectedRowsCount == 1)
            //{
            //    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "Sec", true);
            //    return;
            //}
            //for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            //{
            //    int si = gridView3.GetSelectedRows()[i];
            //    DataRow dr = gridView3.GetDataRow(si);
            //    //gridView3.SetFocusedRowCellValue("Sec", !Convert.ToBoolean(dr["Sec"].ToString()));
            //    gridView3.SetRowCellValue(si, "Sec", true);
            //}            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int i = gridView3.FocusedRowHandle;

            DataRow dr = gridView3.GetDataRow(i);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            string pkFirma = dr["pkFirma"].ToString();

            frmMusteriBakiyeDuzeltme DevirBakisiSatisKaydi = new frmMusteriBakiyeDuzeltme(pkFirma);
            DevirBakisiSatisKaydi.ShowDialog();

            if (barkodara.Text == "")
                adindanara();
            else
                barkoddanara();
            
            //gridrowgetir(gridView3.FocusedRowHandle);
            
            gridView3.FocusedRowHandle=i;

        }
        private void gridView3_EndSorting(object sender, EventArgs e)
        {
            if (gridView3.DataRowCount > 0)
                gridView3.FocusedRowHandle = 0;
        }

        private void gridView3_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //if (e.RowHandle < 0) return;
            ////AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);


            DataRow dr = gridView3.GetDataRow(e.RowHandle);
            if (dr == null)
                return;
            if (e.Column.FieldName == "KaraListe" && dr["KaraListe"].ToString() == "True")
                    AppearanceHelper.Apply(e.Appearance, appError);
            //if (e.Column.FieldName == "Bakiye" && dr["Bakiye"].ToString() != "0")
            //    AppearanceHelper.Apply(e.Appearance, appError);
        }

        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView3.FocusedRowHandle==0)
               gridrowgetir(gridView3.FocusedRowHandle);

            //if (gridView3.FocusedRowHandle < 0) return;
            //DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //string fkFirma = dr["pkFirma"].ToString();

            //SonAlisverisleri(fkFirma);
        }

        private void icbDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            boskengetir();
        }

        private void gridView3_RowCountChanged(object sender, EventArgs e)
        {
            GosterilenAdet.Text = "Gösterilen Kayıt=" + GosterilenAdet.Tag + "/" + gridView3.DataRowCount.ToString();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmFaturaToplu FaturaToplu = new frmFaturaToplu(DB.PkFirma.ToString());
            FaturaToplu.ShowDialog();
        }
        private void barkodara_EditValueChanged(object sender, EventArgs e)
        {
            if (barkodara.Text == "" && adiara.Text == "")
                boskengetir();
            else //if (AraBarkodtan.Text == "" && AraAdindan.Text != "")
                barkoddanara();
        }

        private void barkodara_KeyDown(object sender, KeyEventArgs e)
        {
            adiara.Text = "";
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (barkodara.Text == "" && adiara.Text == "")
                boskengetir();
            else //if (AraBarkodtan.Text == "" && AraAdindan.Text != "")
                adindanara();
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            barkodara.Text = "";
            if (e.KeyCode == Keys.Left && adiara.Text == "")
            {
                barkodara.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
            if (e.KeyCode == Keys.F7)
                btnYeni_Click(sender, e);
        }

        private void müşteriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView3.FocusedRowHandle < 0) return;
            //DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //string pkFirma = dr["pkFirma"].ToString();

            //if (pkFirma == "1")
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Müşteri Kartını Silinemez!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;
            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                string v = gridView3.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView3.GetDataRow(int.Parse(v));
                string pkFirma = dr["pkFirma"].ToString();

                if (pkFirma == "1") continue;

                if (DB.GetData("select * from Satislar with(nolock) where fkFirma=" + pkFirma).Rows.Count > 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Kartı Hareket Gördüğü için Silemezsiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DB.ExecuteSQL("Delete From Firmalar where pkFirma=" + pkFirma);
            }
            formislemleri.Mesajform("Müşteri Bilgileri Silindi.", "S", 200);

            boskengetir();
        }

        private void ePostaGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEpostaGonder EpostaGonder = new frmEpostaGonder();
            EpostaGonder.pFaturaTarihi.Visible = true;
            EpostaGonder.meMesaj.Text = "";
            EpostaGonder.ShowDialog();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            frmSatisFiyatlariMusteriBazli SatisFiyatlariMusteriBazli = new frmSatisFiyatlariMusteriBazli();
            SatisFiyatlariMusteriBazli.ShowDialog();
        }
        void FisYazdir(bool Disigner)
        {
            //string sql = "";
            //@"select convert(bit,'1') as Sec,f.pkFirma,Firmaadi,OzelKod,Tel,Cep,Adres,
            //satis.SatisTutari,isnull(kh.Borc,0),isnull(kh.Alacak,0),
            //(isnull(kh.Alacak,0)+satis.SatisTutari+f.Devir)-isnull(kh.Borc,0) as Bakiye from Firmalar f
            //inner join 
            //(select s.fkFirma,sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as SatisTutari from Satislar s
            //inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar group by s.fkFirma)  satis
            //on satis.fkFirma=f.pkFirma
            //left join (select fkFirma,sum(Borc) as Borc,SUM(Alacak) as Alacak from KasaHareket group by fkFirma) kh on kh.fkFirma=f.pkFirma
            //where isnull(kh.Borc,0)-(isnull(kh.Alacak,0)+satis.SatisTutari+f.Devir)<>0";

            //sql = "exec sp_MusterilereGenelBakis";
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                //DataTable dtliste =  DB.GetData(sql);
                DataTable dtMusteri = (DataTable)gridControl1.DataSource;

                dtMusteri.TableName = "Musteri";
                ds.Tables.Add(dtMusteri);
               
                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\MusteriListesi.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("MusteriListesi.repx Dosya Bulunamadı");
                    return;
                }
                DevExpress.XtraReports.UI.XtraReport rapor = new DevExpress.XtraReports.UI.XtraReport();


                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriListesi";

                rapor.Report.Name = "MusteriListesi";

                rapor.DataSource = ds;

                if (Disigner)
                    rapor.ShowDesignerDialog();
                else
                    rapor.ShowPreviewDialog();

                ds.Tables.Remove(dtMusteri);
                ds.Tables.Remove(Sirket);

                dtMusteri.Dispose();
                ds.Dispose();
                rapor.Dispose();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            FisYazdir(false);
        }

        frmYukleniyor loading = null;
        bool Aktif = false;
        public Thread say;
        public void StartKronometre()
        {
            DateTime zaman = DateTime.Now;
            while (Aktif == true)
            {
                loading.Show();
                loading.Refresh();
                TimeSpan aralik = DateTime.Now - zaman;
                Thread.Sleep(20);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            loading = new frmYukleniyor();
            Aktif = true;
            //System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            say = new Thread(new ThreadStart(StartKronometre));
            say.Start();

            DB.ExecuteSQL("sp_job_MusteriBakiye");

            //DB.ExecuteSQL("update Firmalar set Devir=dbo.fon_MusteriBakiyesi(pkFirma)");
            formislemleri.Mesajform("Müşteri Bakiyeleri Güncellenedi", "S", 200);

            try
            {
                Thread.Sleep(10);
                Aktif = false;
                loading.Show();
                loading.Close();
            }
            catch
            {
            }

            boskengetir();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\MusteriListesiGrid.xml";
            gridView3.SaveLayoutToXml(Dosya);

            gridView3.OptionsBehavior.AutoPopulateColumns = false;
            //gridView1.OptionsCustomization.AllowColumnMoving = false;
            //gridView1.OptionsCustomization.AllowColumnResizing = false;
            //gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            //gridView1.OptionsCustomization.AllowRowSizing = false;
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\MusteriListesiGrid.xml";
            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
                simpleButton19_Click(sender, e);//kapat
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            gridView3.ShowCustomization();
            gridView3.OptionsBehavior.AutoPopulateColumns = true;
            //gridView1.OptionsCustomization.AllowColumnMoving = true;
            //gridView1.OptionsCustomization.AllowColumnResizing = true;
            //gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            //gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("Dış Veri Alırken Excel 2003 formatında ve ilk Sayfa Adı Sayfa1 Olmalı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();

                if (openFileDialog1.FileName == "") return;
                //OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OleDb.4.0;Data Source = " +
                //  openFileDialog1.FileName + " ; Extended Properties = Excel 8.0");//" ; Extended Properties = Excel 8.0");
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + openFileDialog1.FileName + "; Extended Properties='Excel 12.0 Xml;HDR=YES'"); //excel_dosya.xlsx kısmını kendi excel dosyanızın adıyla değiştirin.
                //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
                OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
                //MessageBox.Show((System.Exception)(con.ServerVersion).Message);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int hatali = 0, basarili = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string MUSTERIKODU = dt.Rows[i]["MUSTERIKODU"].ToString();
                    string MUSTERIADI = dt.Rows[i]["MUSTERIADI"].ToString();
                    if (MUSTERIADI == "") continue;
                    string BAKIYE = dt.Rows[i]["BAKIYE"].ToString();
                    string GRUBU = dt.Rows[i]["GRUBU"].ToString();
                    string ADRES = dt.Rows[i]["ADRES"].ToString();
                    string VERGIDAIRESI = dt.Rows[i]["VERGIDAIRESI"].ToString();
                    string VERGINO = dt.Rows[i]["VERGINO"].ToString();
                    string TELEFON = dt.Rows[i]["TELEFON"].ToString();
                    string CEPTEL = dt.Rows[i]["CEPTEL"].ToString();
                    string FAX = dt.Rows[i]["FAX"].ToString();
                    string ANLASMABASTARIH = dt.Rows[i]["ANLASMABASTARIH"].ToString();
                    string ANLASMABITISTARIH = dt.Rows[i]["ANLASMABITISTARIH"].ToString();
                    string FATURAUNVANI = dt.Rows[i]["FATURAUNVANI"].ToString();
                    string YETKILI = dt.Rows[i]["YETKILI"].ToString();
                    string eposta = dt.Rows[i]["EPOSTA"].ToString();
                    if (BAKIYE == "") BAKIYE = "0";
                    if (GRUBU == "") GRUBU = "1";
                    

                    #region Firma Gruplari  ekle

                    //DataTable dtG = DB.GetData("select * from FirmaGruplari with(nolock) where GrupAdi='" + GRUBU + "'");
                    //if (dtG.Rows.Count == 0)
                    //    GRUBU = DB.ExecuteScalarSQL("insert into FirmaGruplari (GrupAdi,Aktif) values('" + GRUBU + "',1) select IDENT_CURRENT('FirmaGruplari')");
                    //else
                    //    GRUBU = dtG.Rows[0][0].ToString();

                    #endregion
                    //if (checkBox1.Checked)
                    //{
                        //if (MUSTERIKODU == "") MUSTERIKODU = "-1";
                    //}
                    //DataTable dtFirma = DB.GetData("select * from Firmalar with(nolock) where OzelKod='" + MUSTERIKODU + "'");
                    string pkFirma = "0";
                    //if (dtFirma.Rows.Count == 0)
                    //{
                        ArrayList list = new ArrayList();
                        list.Add(new SqlParameter("@OzelKod", MUSTERIKODU));
                        list.Add(new SqlParameter("@Firmaadi", MUSTERIADI));

                        if (FATURAUNVANI == "") FATURAUNVANI = MUSTERIADI;

                        list.Add(new SqlParameter("@FaturaUnvani", FATURAUNVANI));
                        list.Add(new SqlParameter("@fkFirmaGruplari", GRUBU));
                        list.Add(new SqlParameter("@Devir", BAKIYE.Replace(",", ".")));
                        list.Add(new SqlParameter("@Adres", ADRES));
                        list.Add(new SqlParameter("@VergiDairesi", VERGIDAIRESI));
                        list.Add(new SqlParameter("@VergiNo", VERGINO));
                        list.Add(new SqlParameter("@Tel", TELEFON));
                        list.Add(new SqlParameter("@Cep", CEPTEL));
                        list.Add(new SqlParameter("@Fax", FAX));
                        list.Add(new SqlParameter("@Yetkili", YETKILI));
                 
                    DateTime dtBasTar = Convert.ToDateTime("01.06.1980");
                    DateTime.TryParse(ANLASMABASTARIH, out dtBasTar);

                    DateTime dtBitTar = Convert.ToDateTime("01.06.1980");
                    DateTime.TryParse(ANLASMABITISTARIH, out dtBitTar);

                    if (dtBasTar == Convert.ToDateTime("01.01.0001"))
                        list.Add(new SqlParameter("@AnlasmaTarihi", DBNull.Value));
                    else
                    list.Add(new SqlParameter("@AnlasmaTarihi", dtBasTar));
                    if (dtBitTar == Convert.ToDateTime("01.01.0001"))
                        list.Add(new SqlParameter("@AnlasaBitisTarihi", DBNull.Value));
                    else
                        list.Add(new SqlParameter("@AnlasaBitisTarihi", dtBitTar.ToString("yyyy-MM-dd")));

                    list.Add(new SqlParameter("@Eposta", eposta));
                    
                    string sql = "INSERT INTO Firmalar (OzelKod,Firmaadi,Adres,Yetkili,FaturaUnvani,fkFirmaGruplari,Devir," +
                        "Aktif,KayitTarihi,VergiDairesi,VergiNo,Tel,Cep,Fax,AnlasmaTarihi,AnlasaBitisTarihi,Eposta)" +
                        " values(@OzelKod,@Firmaadi,@Adres,@Yetkili,@FaturaUnvani,@fkFirmaGruplari,@Devir," +
                        "1,getdate(),@VergiDairesi,@VergiNo,@Tel,@Cep,@Fax,@AnlasmaTarihi,@AnlasaBitisTarihi,@Eposta) select IDENT_CURRENT('Firmalar')";

                        try
                        {
                            pkFirma = DB.ExecuteScalarSQL(sql, list);

                            #region sonuç başarılı ise kasa hareketine devir ekle
                            if (pkFirma.Substring(0,1) != "H")
                            {
                                //sıfırla
                                DB.ExecuteSQL("delete from KasaHareket where fkFirma=" + pkFirma);

                                //bakiye ekle
                                sql = @"INSERT INTO KasaHareket (fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,fkFirma,OdemeSekli,Tutar,BilgisayarAdi,aktarildi,Bakiye)
                            values(" + DB.fkKullanicilar + ",getdate(),3,1,@Borc,@Alacak,'Müşteri Aktarım',0,0,@fkFirma,'Bakiye Düzeltme',@Tutar,'Aktarım',1,0)";

                            sql = sql.Replace("@fkFirma", pkFirma);
                            sql = sql.Replace("@Tutar", "0");//BAKIYE.Replace(",", "."));

                            decimal ddevir = 0;
                            decimal.TryParse(BAKIYE.Replace(".", ","), out ddevir);//NOKTA VİRGÜL DECİMALDE FARKLI
                            if (ddevir > 0)
                            {
                                sql = sql.Replace("@Alacak", ddevir.ToString().Replace(",", ".").Replace("-", ""));
                                sql = sql.Replace("@Borc", "0");
                            }
                            else
                            {
                                sql = sql.Replace("@Alacak", "0");
                                sql = sql.Replace("@Borc", ddevir.ToString().Replace(",", ".").Replace("-", ""));
                            }

                            int sonuc1 = 0;
                            if (ddevir!=0)
                                sonuc1= DB.ExecuteSQL_Sonuc_Sifir(sql);

                             if (sonuc1 != 0)
                             {
                                 hatali = hatali + 1;
                                 //hatalı aktardıysa sil
                                 //DB.ExecuteSQL("delete from Firmalar where pkFirma=" + pkFirma);
                                 //pkFirma = "0";
                             }
                             else
                             {
                                 basarili = basarili + 1;
                             }
                            }
                            #endregion
                        }
                        catch (Exception exp)
                        {
                            hatali = hatali + 1;
                            pkFirma = "0";
                        }
                }
                MessageBox.Show("Hatalı-Başarılı Kayıt : " + hatali.ToString() + "-" + basarili.ToString());
            }
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Yeni\v.xls;Extended Properties=”Excel 8.0;HDR=Yes;IMEX=1″
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
            DB.ExecuteSQL("update Firmalar set OzelKod=pkFirma where OzelKod is null");
            DB.ExecuteSQL("update Firmalar set OzelKod=pkFirma where OzelKod=''");
            adindanara();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            //ShowDesigner
            DataTable sanal = new DataTable();
            sanal.Columns.Add(new DataColumn("pkFirma", typeof(Int32)));
            sanal.Columns.Add(new DataColumn("firmaadi", typeof(string)));
            sanal.Columns.Add(new DataColumn("adres", typeof(string)));

            //sanal.Columns.Add(new DataColumn("Tipi", typeof(string)));
            //sanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
            //sanal.Columns.Add(new DataColumn("Aciklama", typeof(string)));
            //sanal.Columns.Add(new DataColumn("Borc", typeof(float)));

            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                string v = gridView3.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView3.GetDataRow(int.Parse(v));

                DataRow dryeni = sanal.NewRow();

                dryeni["pkFirma"] = dr["pkFirma"];
                dryeni["Firmaadi"] = dr["firmaadi"];
                dryeni["Adres"] = dr["adres"];
                //dr["Cep"] = sanal.Rows[0]["Cep"];

                sanal.Rows.Add(dryeni);
                //SatisDetayEkle(dr["Barcode"].ToString());

                //if (StokAra.gridView1.GetSelectedRows()[i] >= 0)
                //{
                //    DataRow dr = StokAra.gridView1.GetDataRow(i);
                //    SatisDetayEkle(dr["Barcode"].ToString());
                //}
            }
            xrCariHareket rapor = new xrCariHareket();
            try
            {
                System.Data.DataSet ds = new DataSet("Test");

                //int i = gridView1.FocusedRowHandle;
                //DataRow dr = gridView1.GetDataRow(i);
                //string _fkFirma = dr["pkFirma"].ToString();

                //DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + _fkFirma);
                sanal.TableName = "Musteri";
                ds.Tables.Add(sanal);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\MusteriAdresEtiket.repx");
                rapor.Name = "MusteriAdresEtiket";
                rapor.Report.Name = "MusteriAdresEtiket";
            }
            catch (Exception ex)
            {

            }
            //if (dizayn)
            rapor.ShowPreview();
            //else
            //rapor.Print();

        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            //ShowDesigner
            DataTable sanal = new DataTable();
            sanal.Columns.Add(new DataColumn("pkFirma", typeof(Int32)));
            sanal.Columns.Add(new DataColumn("firmaadi", typeof(string)));
            sanal.Columns.Add(new DataColumn("adres", typeof(string)));

            //sanal.Columns.Add(new DataColumn("Tipi", typeof(string)));
            //sanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
            //sanal.Columns.Add(new DataColumn("Aciklama", typeof(string)));
            //sanal.Columns.Add(new DataColumn("Borc", typeof(float)));

            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                string v = gridView3.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView3.GetDataRow(int.Parse(v));

                DataRow dryeni = sanal.NewRow();

                dryeni["pkFirma"] = dr["pkFirma"];
                dryeni["Firmaadi"] = dr["firmaadi"];
                dryeni["Adres"] = dr["adres"];
                //dr["Cep"] = sanal.Rows[0]["Cep"];

                sanal.Rows.Add(dryeni);
                //SatisDetayEkle(dr["Barcode"].ToString());

                //if (StokAra.gridView1.GetSelectedRows()[i] >= 0)
                //{
                //    DataRow dr = StokAra.gridView1.GetDataRow(i);
                //    SatisDetayEkle(dr["Barcode"].ToString());
                //}
            }
            xrCariHareket rapor = new xrCariHareket();
            try
            {
                System.Data.DataSet ds = new DataSet("Test");

                //int i = gridView1.FocusedRowHandle;
                //DataRow dr = gridView1.GetDataRow(i);
                //string _fkFirma = dr["pkFirma"].ToString();

                //DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + _fkFirma);
                sanal.TableName = "Musteri";
                ds.Tables.Add(sanal);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\MusteriAdresEtiket.repx");
                rapor.Name = "MusteriAdresEtiket";
                rapor.Report.Name = "MusteriAdresEtiket";
            }
            catch (Exception ex)
            {

            }
            //if (dizayn)
            rapor.ShowDesigner();
            //else
              //rapor.Print();

        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if(gridView3.FocusedRowHandle<0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            string pkFirma = dr["pkFirma"].ToString();
            frmKontaklar Kontaklar = new frmKontaklar(pkFirma);
            Kontaklar.Show();
        }

        private void sbtnExcelGonder_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\Musteriler" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
            gridView3.ExportToXls(dosyaadi);
            System.Diagnostics.Process.Start(dosyaadi);
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında.");

            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            string pkFirma = dr["pkFirma"].ToString();
         

            frmCariHareketleri cari = new frmCariHareketleri();
            cari.musteriadi.Tag = pkFirma;
            cari.Show();
        }

        private void btnOzelNot_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            string pkFirma = dr["pkFirma"].ToString();
            if (DB.GetData("select * from FirmaOzelNot where fkfirma="+pkFirma).Rows.Count>0)
                DB.ExecuteSQL("update FirmaOzelNot set OzelNot='" + memoEdit1.Text + "' where fkFirma=" + pkFirma);
            else
                DB.ExecuteSQL("insert into FirmaOzelNot (OzelNot,fkFirma)" +
            "values('" + memoEdit1.Text + "',"+pkFirma+")");

            btnOzelNot.Enabled = false;
        }

        private void memoEdit1_EditValueChanged(object sender, EventArgs e)
        {
            btnOzelNot.Enabled = true;
        }

        private void yazdırDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FisYazdir(true);
        }

        private void aidatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriOtoTaksitAta MusteriOtoTaksitAta = new frmMusteriOtoTaksitAta();
            MusteriOtoTaksitAta.Show();
        }

        private void seçilenleriAktifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                string v = gridView3.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView3.GetDataRow(int.Parse(v));
                string Firma_id = dr["pkFirma"].ToString();
            //string borc = dr["Devir"].ToString();
            //if (borc == "0,0")
                DB.ExecuteSQL("update Firmalar set Aktif=1 where pkFirma=" + Firma_id);
            }

            if (barkodara.Text == "")
                adindanara();
            else
                barkoddanara();
        }

        private void seçilenleriPasifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string s = formislemleri.MesajBox("Müşteri(ler) Pasif Yapılsın mı?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                string v = gridView3.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView3.GetDataRow(int.Parse(v));
                string Firma_id = dr["pkFirma"].ToString();
                //string borc = dr["Devir"].ToString();
                //if (borc== "0,000000")
                DB.ExecuteSQL("update Firmalar set Aktif=0 where pkFirma=" + Firma_id);
            }

            if (barkodara.Text == "")
                adindanara();
            else
                barkoddanara();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
           
        }
    }
}

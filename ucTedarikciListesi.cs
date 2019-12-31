using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using System.IO;
using GPTS.Include.Data;
using GPTS.islemler;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class ucTedarikciListesi : DevExpress.XtraEditors.XtraUserControl
    {
        public ucTedarikciListesi()
        {
            InitializeComponent();
            DB.FirmaAdi = "";
        }

        private void ucCariKartlar_Load(object sender, EventArgs e)
        {
            //if (File.Exists("gridView3.xml"))
              //  gridView3.RestoreLayoutFromXml("gridView3.xml");
            boskengetir();
            adiara.Focus();

            string Dosya = DB.exeDizini + "\\TedarikciListesiGrid.xml";
            if (File.Exists(Dosya))
                gridView3.RestoreLayoutFromXml(Dosya);

            gridView3.ClearColumnsFilter();
        }

        private void sMSGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSmsGonder SmsGonder = new frmSmsGonder();
            SmsGonder.ShowDialog();
        }

        void SonAlisverisleri(string PkFirma)
        {
            string sql = @"SELECT top 10 A.pkAlislar, A.Tarih, sum(AD.Adet * (AD.AlisFiyati-AD.iskontotutar)) AS Tutar
            FROM Alislar A with(nolock)
            INNER JOIN AlisDetay AD with(nolock) ON A.pkAlislar = AD.fkAlislar 
            where A.Siparis=1 and fkFirma=@fkFirma
            group by A.pkAlislar, A.Tarih order by A.pkAlislar desc";
            sql=sql.Replace("@fkFirma", PkFirma);

            gcSonAlisVeris.DataSource= DB.GetData(sql);
        }

        private void hareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmTedarikcilerHareketleri CariHareket = new frmTedarikcilerHareketleri();
            CariHareket.musteriadi.Tag = dr["pkTedarikciler"].ToString();
            CariHareket.Show();
        }

        private void yeniHareketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeAl();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl1,"A4");
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmTedarikciKarti kart = new frmTedarikciKarti("0");
            DB.pkTedarikciler = 0;
            kart.ShowDialog();
            boskengetir();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            gridView3.SaveLayoutToXml("gridView3.xml");
            this.Dispose();
            int c = Application.OpenForms.Count;
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        //void musteriara()
        //{
        //    string tumu="0";
        //    if (cbTumu.Checked) tumu = "1";
        //    string sql = "exec tedarikciara_sp '" + AraBarkodtan.Text + "','" + AraAdindan.Text + "'," + tumu;
        //    gridControl1.DataSource = DB.GetData(sql);
        //}


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

        private void cbTumu_CheckedChanged(object sender, EventArgs e)
        {
            boskengetir();
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

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            BtnDuzenle_Click(sender, e);
        }

        private void gridView3_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (e.FocusedRowHandle < 0) return;
            //DataRow dr = gridView3.GetDataRow(e.FocusedRowHandle);
            //DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());
            //SonAlisverisleri(DB.pkTedarikciler.ToString());
            //gcMusteriEkBilgileri.DataSource = DB.GetData("select * from Tedarikciler where pkTedarikciler=" + DB.pkTedarikciler.ToString());

            gridrowgetir(e.FocusedRowHandle);

            OzelNotGetir(e.FocusedRowHandle);
        }
        void OzelNotGetir(int RowHandle)
        {
            if (RowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(RowHandle);
            string pkTedarikciler = dr["pkTedarikciler"].ToString();

            memoEdit1.Text = "";
            DataTable dt =  DB.GetData("select * from TedarikcilerOzelNot with(nolock) where fkTedarikciler="+ pkTedarikciler);
            if (dt.Rows.Count>0)
               memoEdit1.Text = dt.Rows[0]["OzelNot"].ToString();

        }

        void gridrowgetir(int RowHandle)
        {
            if (RowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(RowHandle);

            string pkTedarikciler = dr["pkTedarikciler"].ToString();

            SonAlisverisleri(pkTedarikciler);

            //gcMusteriEkBilgileri.DataSource = DB.GetData("select * from Tedarikciler with(nolock) where pkTedarikciler=" + pkTedarikciler);

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
            //string s = Satis1Toplam.Text.ToString().Replace(",",".");

            // gridView3.SetRowCellValue(RowHandle, "Bakiye", Satis1Toplam);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["pkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmTaksitOdemeleri TaksitOdemeleri = new frmTaksitOdemeleri();
            TaksitOdemeleri.ShowDialog();
 
        }

        private void gridView3_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                gridView3.SetRowCellValue(i, "Sec", false);
            }

            if (gridView3.SelectedRowsCount == 1)
            {
                gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "Sec", true);
                return;
            }
            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                int si = gridView3.GetSelectedRows()[i];
                DataRow dr = gridView3.GetDataRow(si);
                //gridView3.SetFocusedRowCellValue("Sec", !Convert.ToBoolean(dr["Sec"].ToString()));
                gridView3.SetRowCellValue(si, "Sec", true);
            }            
        }

        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());
            frmTedarikciKarti KurumKarti = new frmTedarikciKarti(dr["pkTedarikciler"].ToString());
            KurumKarti.ShowDialog();

            gridrowgetir(gridView3.FocusedRowHandle);
        }

        void OdemeAl()
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            DB.pkKasaHareket = 0;
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkTedarikci.Text = dr["pkTedarikciler"].ToString();
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
            KasaCikis.pkTedarikci.Text = dr["pkTedarikciler"].ToString();
            KasaCikis.Tag = "2";
            KasaCikis.ShowDialog();
            gridrowgetir(gridView3.FocusedRowHandle);
        }

        private void ödemeYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeYap();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
                  
            frmTedarikciBakiyeDuzeltme DevirBakisiSatisKaydi = new frmTedarikciBakiyeDuzeltme();
            DB.pkTedarikciler = int.Parse(dr["pkTedarikciler"].ToString());
            DevirBakisiSatisKaydi.ShowDialog();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmEpostaGonder EpostaGonder = new frmEpostaGonder();
            EpostaGonder.ShowDialog();
        }

        private void tedarikçiSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string pkTedarikciler = dr["pkTedarikciler"].ToString();

            if (pkTedarikciler == "1")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Tedarikçi Kartını Silinemez!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DB.GetData("select * from Alislar where fkFirma=" + pkTedarikciler).Rows.Count > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Tedarikçi Kartı Hareket Gördüğü için Silemezsiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;
            DB.ExecuteSQL("Delete From Tedarikciler where pkTedarikciler=" + pkTedarikciler);
            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = "Tedarikçi Bilgileri Silindi.";
            Mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
            Mesaj.Show();
            boskengetir();
        }

        private void adiara_EditValueChanged(object sender, EventArgs e)
        {
            if (barkodara.Text == "" && adiara.Text == "")
                boskengetir();
            else //if (AraBarkodtan.Text == "" && AraAdindan.Text != "")
                adindanara();
        }

        private void adiara_KeyDown(object sender, KeyEventArgs e)
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
        void boskengetir()
        {
            string sql = @"SELECT top 1000 CONVERT(bit, '0') AS Sec, T.pkTedarikciler, T.Firmaadi,T.Yetkili, T.LimitBorc, T.KaraListe, 
                      TedarikcilerGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi
                      ,convert(int,T.OzelKod) as OzelKod ,TAG.TedarikcilerAltGrup as FirmaAltGrupAdi,
                       dbo.fon_TedarikciBakiyesi(T.pkTedarikciler) as Bakiye,T.Tel,T.Cep,T.Adres,T.SonAlisTarihi,T.Aktif,
                    datediff(day,isnull(T.SonAlisTarihi,getdate()),getdate()) as Gun
                    FROM  Tedarikciler T with(nolock)
                    LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH  with(nolock)  WHERE GRUP = '1') AS il ON T.fkil = il.KODU 
                    LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH  with(nolock) WHERE GRUP = '2') AS ilce ON T.fkil = ilce.ALTGRUP AND T.fkilce = ilce.KODU 
                    LEFT JOIN TedarikcilerGruplari  with(nolock) ON T.fkFirmaGruplari = TedarikcilerGruplari.pkTedarikcilerGruplari 
                    LEFT JOIN TedarikcilerAltGruplari TAG with(nolock) ON T.fkTedarikcilerAltGruplari = TAG.pkTedarikcilerAltGruplari";

            if (icbDurumu.SelectedIndex == 0) sql = sql + " where T.Aktif=1";
            if (icbDurumu.SelectedIndex == 1) sql = sql + " where T.Aktif=0";
            sql = sql + " ORDER BY T.tiklamaadedi DESC";

            gridControl1.DataSource = DB.GetData(sql);
        }

        void barkoddanara()
        {
            string sql = @"SELECT top 1000 CONVERT(bit, '0') AS Sec, T.pkTedarikciler, T.Firmaadi,T.Yetkili, T.LimitBorc, T.KaraListe, 
                      TedarikcilerGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi
                      ,convert(int,T.OzelKod) as OzelKod ,TAG.TedarikcilerAltGrup as FirmaAltGrupAdi,
                       dbo.fon_TedarikciBakiyesi(T.pkTedarikciler) as Bakiye,T.Tel,T.Cep,T.Adres,T.SonAlisTarihi,T.Aktif,
                    datediff(day,isnull(T.SonAlisTarihi,getdate()),getdate()) as Gun
                    FROM  Tedarikciler T with(nolock)
                    LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH  with(nolock)  WHERE GRUP = '1') AS il ON T.fkil = il.KODU 
                    LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH  with(nolock) WHERE GRUP = '2') AS ilce ON T.fkil = ilce.ALTGRUP AND T.fkilce = ilce.KODU 
                    LEFT JOIN TedarikcilerGruplari  with(nolock) ON T.fkFirmaGruplari = TedarikcilerGruplari.pkTedarikcilerGruplari 
                    LEFT JOIN TedarikcilerAltGruplari TAG with(nolock) ON T.fkTedarikcilerAltGruplari = TAG.pkTedarikcilerAltGruplari
                    where T.Aktif=1";
            if (barkodara.Text.IndexOf(" ") == 0) sql = sql + " and OzelKod like '%" + barkodara.Text.Substring(1, barkodara.Text.Length - 1) + "'";
            else if (barkodara.Text.IndexOf(" ") == barkodara.Text.Length - 1) sql = sql + " and OzelKod like '" + barkodara.Text.Substring(0, barkodara.Text.Length - 1) + "%'";
            else
                sql = sql + " and OzelKod='" + barkodara.Text + "'";
            gridControl1.DataSource = DB.GetData(sql);
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
            string sql = @"SELECT top 1000 CONVERT(bit, '0') AS Sec, T.pkTedarikciler, T.Firmaadi,T.Yetkili, T.LimitBorc, T.KaraListe, 
                      TedarikcilerGruplari.GrupAdi, il.ADI AS iladi, ilce.ADI ilceadi
                      ,convert(int,T.OzelKod) as OzelKod ,TAG.TedarikcilerAltGrup as FirmaAltGrupAdi,
                      Devir as Bakiye,T.Tel,T.Cep,T.Adres,T.SonAlisTarihi,T.Aktif,
                    datediff(day,isnull(T.SonAlisTarihi,getdate()),getdate()) as Gun
                    FROM  Tedarikciler T with(nolock)
                    LEFT JOIN(SELECT ADI, KODU FROM  IL_ILCE_MAH  with(nolock)  WHERE GRUP = '1') AS il ON T.fkil = il.KODU 
                    LEFT JOIN (SELECT ADI, KODU,ALTGRUP FROM  IL_ILCE_MAH  with(nolock) WHERE GRUP = '2') AS ilce ON T.fkil = ilce.ALTGRUP AND T.fkilce = ilce.KODU 
                    LEFT JOIN TedarikcilerGruplari  with(nolock) ON T.fkFirmaGruplari = TedarikcilerGruplari.pkTedarikcilerGruplari 
                    LEFT JOIN TedarikcilerAltGruplari TAG with(nolock) ON T.fkTedarikcilerAltGruplari = TAG.pkTedarikcilerAltGruplari";
            if (icbDurumu.SelectedIndex == 1)
                sql += " where T.Aktif=1";
            else if (icbDurumu.SelectedIndex == 2)
                sql += " where T.Aktif=0";
            else
                sql += " where 1=1";

            if (s1.Length > 0)
                sql += " and T.Firmaadi like '%" + s1 + "%'";
            if (s2.Length > 0)
                sql += " and T.Firmaadi like '%" + s2 + "%'";
            if (s3.Length > 0)
                sql += " and T.Firmaadi like '%" + s3 + "%'";



            gridControl1.DataSource = DB.GetData(sql);
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

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmTedarikcilereGenelBakis TedarikcilereGenelBakis = new frmTedarikcilereGenelBakis();
            TedarikcilereGenelBakis.Show();
        }


        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            gridrowgetir(e.RowHandle);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmTedarikcilerHareketleri CariHareketMusteri = new frmTedarikcilerHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkTedarikciler"].ToString();
            CariHareketMusteri.Show();
        }

        private void dışVeriAlExcelToolStripMenuItem_Click(object sender, EventArgs e)
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

                OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OleDb.4.0;Data Source = " +
                    openFileDialog1.FileName + " ; Extended Properties = Excel 8.0");//" ; Extended Properties = Excel 8.0");
                //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
                OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
                //MessageBox.Show((System.Exception)(con.ServerVersion).Message);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int hatali = 0, basarili = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string TEDARIKCIKODU = dt.Rows[i]["TEDARIKCIKODU"].ToString();
                    string TEDARICIADI = dt.Rows[i]["TEDARICIADI"].ToString();

                    if (TEDARICIADI == "") continue;

                    string BAKIYE = dt.Rows[i]["BAKIYE"].ToString();
                    string GRUBU = dt.Rows[i]["GRUBU"].ToString();
                    string ADRES = dt.Rows[i]["ADRES"].ToString();
                    string VERGIDAIRESI = dt.Rows[i]["VERGIDAIRESI"].ToString();
                    string VERGINO = dt.Rows[i]["VERGINO"].ToString();
                    string TELEFON = dt.Rows[i]["TELEFON"].ToString();
                    string CEPTEL = dt.Rows[i]["CEPTEL"].ToString();
                    string FAX = dt.Rows[i]["FAX"].ToString();

                    if (BAKIYE == "") BAKIYE = "0";

                    #region Firma Gruplari  ekle

                    DataTable dtG = DB.GetData("select * from TedarikcilerGruplari with(nolock) where GrupAdi='" + GRUBU + "'");
                    if (dtG.Rows.Count == 0)
                        GRUBU = DB.ExecuteScalarSQL("insert into TedarikcilerGruplari (GrupAdi,Aktif) values('" + GRUBU + "',1) select IDENT_CURRENT('FirmaGruplari')");
                    else
                        GRUBU = dtG.Rows[0][0].ToString();

                    #endregion

                    DataTable dtTedarikciler = DB.GetData("select * from Tedarikciler with(nolock) where OzelKod='" + TEDARIKCIKODU + "'");
                    string pkTedarikciler = "0";
                    if (dtTedarikciler.Rows.Count == 0)
                    {
                        ArrayList list = new ArrayList();
                        list.Add(new SqlParameter("@OzelKod", "0"));
                        list.Add(new SqlParameter("@Firmaadi", TEDARICIADI));
                        list.Add(new SqlParameter("@fkFirmaGruplari", GRUBU));
                        list.Add(new SqlParameter("@Devir", BAKIYE.Replace(",", ".")));
                        list.Add(new SqlParameter("@Adres", ADRES));
                        list.Add(new SqlParameter("@VergiDairesi", VERGIDAIRESI));
                        list.Add(new SqlParameter("@VergiNo", VERGINO));
                        list.Add(new SqlParameter("@Tel", TELEFON));
                        list.Add(new SqlParameter("@Cep", CEPTEL));
                        list.Add(new SqlParameter("@Fax", FAX));

                        string sql = "INSERT INTO Tedarikciler (OzelKod,Firmaadi,fkFirmaGruplari,Devir,Aktif,KayitTarihi,Adres,VergiDairesi,VergiNo,Tel,Cep,Fax)" +
                        " values(@OzelKod,@Firmaadi,@fkFirmaGruplari,@Devir,1,getdate(),@Adres,@VergiDairesi,@VergiNo,@Tel,@Cep,@Fax) select IDENT_CURRENT('Tedarikciler')";

                        try
                        {
                            pkTedarikciler = DB.ExecuteScalarSQL(sql, list);

                            if (pkTedarikciler.Substring(0, 1) == "H")
                                hatali = hatali + 1;
                            else
                            {
                                #region sonuç başarılı ise kasa hareketine devir ekle

                                sql = @"delete from KasaHareket where fkTedarikciler=@fkTedarikciler INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,BilgisayarAdi)
                    values(1,1,getdate(),3,1,@Borc,@Alacak,'Aktarım',0,1,0,@fkTedarikciler,'Kasa Bakiye Düzeltme',@Tutar,'Aktarım')";

                                sql = sql.Replace("@fkTedarikciler", pkTedarikciler);
                                sql = sql.Replace("@Tutar", "0");

                                decimal ddevir = 0;
                                decimal.TryParse(BAKIYE.Replace(".", ","), out ddevir);
                                if (ddevir > 0)
                                {
                                    sql = sql.Replace("@Borc", ddevir.ToString().Replace(",", ".").Replace("-", ""));
                                    sql = sql.Replace("@Alacak", "0");
                                }
                                else
                                {
                                    sql = sql.Replace("@Borc", "0");
                                    sql = sql.Replace("@Alacak", ddevir.ToString().Replace(",", ".").Replace("-", ""));
                                }

                                int sonuc1 = DB.ExecuteSQL_Sonuc_Sifir(sql);
                                if (sonuc1 != 0)
                                {
                                    hatali = hatali + 1;
                                    DB.ExecuteSQL("delete from Tedarikciler where pkTedarikciler=" + pkTedarikciler);
                                    pkTedarikciler = "0";
                                }
                                else
                                    basarili = basarili + 1;
                                #endregion
                            }
                        }
                        catch (Exception exp)
                        {
                            pkTedarikciler = "0";
                        }
                    }
                    else
                    {
                        pkTedarikciler = dtTedarikciler.Rows[0]["pkTedarikciler"].ToString();
                    }
                }
                MessageBox.Show("Hatalı-Başarılı Kayıt : " + hatali.ToString() + "-" + basarili.ToString());

            }
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Yeni\v.xls;Extended Properties=”Excel 8.0;HDR=Yes;IMEX=1″
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }


        }

        private void btnOzelNot_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            string pkTedarikciler = dr["pkTedarikciler"].ToString();
            if (DB.GetData("select * from TedarikcilerOzelNot where fkTedarikciler=" + pkTedarikciler).Rows.Count > 0)
                DB.ExecuteSQL("update TedarikcilerOzelNot set OzelNot='" + memoEdit1.Text + "' where fkTedarikciler=" + pkTedarikciler);
            else
                DB.ExecuteSQL("insert into TedarikcilerOzelNot (OzelNot,fkTedarikciler)" +
            "values('" + memoEdit1.Text + "'," + pkTedarikciler + ")");

            btnOzelNot.Enabled = false;
        }

        private void memoEdit1_EditValueChanged(object sender, EventArgs e)
        {
            btnOzelNot.Enabled = true;
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\TedarikciListesiGrid.xml";
            gridView3.SaveLayoutToXml(Dosya);

            gridView3.OptionsBehavior.AutoPopulateColumns = false;
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\TedarikciListesiGrid.xml";
            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
                simpleButton19_Click(sender, e);//kapat
            }
        }
    }
}

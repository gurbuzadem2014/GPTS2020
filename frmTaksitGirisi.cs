using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System.Collections;
using System.Data.SqlClient;
using GPTS.islemler;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmTaksitGirisi : DevExpress.XtraEditors.XtraForm
    {
        string pkFirma_id = "0";
        public frmTaksitGirisi(string fkFirma)
        {
            InitializeComponent();
            pkFirma_id = fkFirma;
        }

        private void btnExtreYazdir_Click(object sender, EventArgs e)
        {
            YazdirTaksitlerExtra(false);
        }

        private void frmTaksitGirisi_Load(object sender, EventArgs e)
        {
            TilkTaksitTarihi.DateTime = DateTime.Now.AddMonths(1);

            #region Taksit
            this.Text = "Yeni Taksit Girişi";
            TAdet.Value = 1;
            Taksitler();
            ToplamTutar.Focus();
            #endregion
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("delete from Taksitler where taksit_id=" + taksit_id.Text);
            DB.ExecuteSQL("delete from Taksit where taksit_id=" + taksit_id.Text);
            Close();
        }

        private void TaksitlereBol()
        {
            #region Taksit ekle 
            //taksit başlık bilgisi
            if (taksit_id.Text == "0")
            {
                ArrayList listt = new ArrayList();
                listt.Add(new SqlParameter("@fkFirma", pkFirma_id));
                listt.Add(new SqlParameter("@aciklama", teAciklama.Text));
                listt.Add(new SqlParameter("@kefil", teKefil.Text));
                listt.Add(new SqlParameter("@mahkeme", teMahkeme.Text));
                listt.Add(new SqlParameter("@fkSatislar", fkSatislar.Text));
                listt.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
                listt.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                string sql = @"insert into Taksit(fkFirma,tarih,aciklama,kefil,mahkeme,fkSatislar,fkKullanici,fkSube)
                    values(@fkFirma,getdate(),@aciklama,@kefil,@mahkeme,@fkSatislar,@fkKullanici,@fkSube) SELECT IDENT_CURRENT('Taksit')";

                string sonuc = DB.ExecuteScalarSQL(sql, listt);
                taksit_id.Text = sonuc;
            }
            
            #endregion

            decimal ToplamOdenen = 0;

            DateTime HesabaGecisTarih = TilkTaksitTarihi.DateTime;
            //DateTime gruplandir = DateTime.Now;
            for (int i = 0; i < TAdet.Value; i++)
            {

                ArrayList list = new ArrayList();
                //list.Add(new SqlParameter("@fkFirma", teMusteri.Tag.ToString()));
                list.Add(new SqlParameter("@Tarih", HesabaGecisTarih));

                if (checkEdit1.Checked && i == TAdet.Value - 1)
                {
                    list.Add(new SqlParameter("@Odenecek", (ToplamTutar.Value - ToplamOdenen).ToString().Replace(",", ".")));
                }
                else
                    list.Add(new SqlParameter("@Odenecek", ceTaksitTutari.Value.ToString().Replace(",", ".")));

                list.Add(new SqlParameter("@Odenen", "0"));
                list.Add(new SqlParameter("@SiraNo", (i + 1).ToString()));
                list.Add(new SqlParameter("@HesabaGecisTarih", HesabaGecisTarih));
                list.Add(new SqlParameter("@taksit_id", taksit_id.Text));
                list.Add(new SqlParameter("@Aciklama", teAciklama.Text));
                list.Add(new SqlParameter("@Kefil", teKefil.Text));
                list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));

                DB.ExecuteSQL(" INSERT INTO Taksitler (Tarih,Odenecek,Odenen,SiraNo,HesabaGecisTarih,OdemeSekli,Aciklama,Kefil,fkKullanici,Kaydet,taksit_id,kayit_tarihi)" +
                    " VALUES(@Tarih,@Odenecek,@Odenen,@SiraNo,@HesabaGecisTarih,'Taksit (Senet)',@Aciklama,@Kefil,@fkKullanici,0,@taksit_id,getdate())", list);

                HesabaGecisTarih = HesabaGecisTarih.AddMonths(1);
                ToplamOdenen = ToplamOdenen + ceTaksitTutari.Value;
            }

            Taksitler();
            //taksit_id.Text = "0";
        }

        void Taksitler()
        {
            string sql = "";
            sql = "Select *,Odenecek-Odenen as Kalan from Taksitler with(nolock) where Kaydet=0 and taksit_id=@taksit_id";
            sql = sql.Replace("@taksit_id", taksit_id.Text);
            gCTaksitler.DataSource = DB.GetData(sql);
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (teMusteri.Tag == null || teMusteri.Tag.ToString() == "0")
            {
                teMusteri.Focus();
                return;
            }

            if (ceTaksitTutari.Value == 0)
            {
                ceTaksitTutari.Focus();
                return;
            }

            //DB.ExecuteSQL("delete from Taksitler where taksit_id=" + taksit_id.Text);
            //DB.ExecuteSQL("delete from Taksit where taksit_id=" + taksit_id.Text);

            //taksit_id.Text = "0";

            TaksitlereBol();

            ToplamTutar.Value = 0;
            ceTaksitTutari.Value = 0;
            teAciklama.Text = "";
            TAdet.Value = 1;
            teKefil.Text = "";
            teMahkeme.Text = "";
            simpleButton2.Enabled = true;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            frmMusteriAra MusteriAra = new frmMusteriAra();
            //MusteriAra.fkFirma.Tag = fkFirma;
            MusteriAra.ShowDialog();
            //if (MusteriAra.fkFirma.Tag.ToString() == fkFirma)
            // {
            //BakiyeGetirSecindex(fkFirma);//yavaşlatıyor
            //FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
            //  return fkFirma;
            //}
            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + pkFirma_id);
            if (dt.Rows.Count > 0)
            {
                teMusteri.Tag = pkFirma_id;
                teMusteri.Text = dt.Rows[0]["Firmaadi"].ToString();
            }
            dt.Dispose();
            teMusteri.Tag = DB.PkFirma;
            teMusteri.Text = DB.FirmaAdi;

            //string fkFirma = MusteriAra.fkFirma.Tag.ToString();
            //teMusteri.Tag = fkFirma;
            MusteriAra.Dispose();
        }

        private void TAdet_EditValueChanged(object sender, EventArgs e)
        {
            if(TAdet.Value>0)
               ceTaksitTutari.Value = ToplamTutar.Value / TAdet.Value;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);
                DB.ExecuteSQL("Update Taksitler set Kaydet=1 where pkTaksitler=" + dr["pkTaksitler"].ToString());

                DBislemleri.YeniHatirlatmaEkle("Taksit Hatırlatma", Convert.ToDateTime(dr["Tarih"].ToString()), "Taksit Ödemesi", Convert.ToDateTime(dr["Tarih"].ToString()).AddHours(1), pkFirma_id, "0","0", dr["pkTaksitler"].ToString());
            }

            Close();
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            ftmTaksitKarti taksit = new ftmTaksitKarti(dr["pkTaksitler"].ToString());
            taksit.Text = dr["SiraNo"].ToString() + ". Taksit Bilgisi";
            taksit.ShowDialog();

            Taksitler();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            DB.ExecuteSQL("delete from Taksitler where pkTaksitler=" + dr["pkTaksitler"].ToString());

            Taksitler();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            YazdirTaksitler(false);
        }
        void YazdirTaksitler(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                string sql = @"select *,dbo.fnc_ParayiYaziyaCevir(Odenecek,2) as rakamoku,Odenecek-Odenen as Kalan  
                   from Taksit T with(nolock)
                   left join Taksitler TL with(nolock) on T.taksit_id=TL.taksit_id
                   where Kaydet=0";

                //if (!cbTumTaksitler.Checked)
                //    sql = sql + " and Odenecek<>Odenen";

                sql = sql + " and T.fkFirma=" + pkFirma_id;

                DataTable dtTaksitler = DB.GetData(sql);

                dtTaksitler.TableName = "Taksitler";
                ds.Tables.Add(dtTaksitler);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + pkFirma_id);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Taksitler.repx");
                rapor.Name = "Taksitler";
                rapor.Report.Name = "Taksitler";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }
        void YazdirTaksitler_(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            try
            {
                string fkFirma = teMusteri.Tag.ToString();
                System.Data.DataSet ds = new DataSet("Test");
                DataTable dtTaksitler = DB.GetData("select *,dbo.fnc_ParayiYaziyaCevir(Odenecek,2) as rakamoku from Taksitler with(nolock) where Odenecek<>Odenen and isnull(Kaydet,0)=0 and fkFirma=" + fkFirma);

                //DataTable FisDetay = DB.GetData(sql);
                dtTaksitler.TableName = "Taksitler";
                ds.Tables.Add(dtTaksitler);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Taksitler.repx");
                rapor.Name = "Taksitler";
                rapor.Report.Name = "Taksitler";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }

        private void senetYazdırDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirTaksitler(true);
        }

        private void taksitExtreDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirTaksitlerExtra(true);
        }

        void YazdirTaksitlerExtra(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                string sql = @"select *,dbo.fnc_ParayiYaziyaCevir(Odenecek,2) as rakamoku,Odenecek-Odenen as Kalan  
                            from Taksit T with(nolock)
                   left join Taksitler TL with(nolock) on T.taksit_id=TL.taksit_id
                   where Kaydet=0";
                //if(checkEdit1.Checked)
                   sql = sql+" and Odenecek<>Odenen"; 

                sql = sql + " and T.fkFirma=" + pkFirma_id;

                DataTable dtTaksitler = DB.GetData(sql);

                dtTaksitler.TableName = "Taksitler";
                ds.Tables.Add(dtTaksitler);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + pkFirma_id);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\TaksitlerExtra.repx");
                rapor.Name = "TaksitlerExtra";
                rapor.Report.Name = "TaksitlerExtra";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }

        private void ToplamTutar_EditValueChanged(object sender, EventArgs e)
        {
            if (TAdet.Value>0)
               ceTaksitTutari.Value = ToplamTutar.Value / TAdet.Value;
        }



        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
                Taksitler();
        }

        private void taksit_id_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = 
            DB.GetData("select * from Taksit T with(nolock) where taksit_id=" + taksit_id.Text);

            if (dt.Rows.Count > 0)
            {
                //pkFirma_id = dt.Rows[0]["fkFirma"].ToString();
                //teMusteri.Tag = pkFirma_id;
                //teMusteri.Text = dt.Rows[0]["Firmaadi"].ToString();
                //teMusteri.Tag = DB.PkFirma;
                //teMusteri.Text = DB.FirmaAdi;

                //teMusteri.Text = dt.Rows[0]["Firmaadi"].ToString();
                teAciklama.Text = dt.Rows[0]["aciklama"].ToString();
                teKefil.Text = dt.Rows[0]["kefil"].ToString();
                teMahkeme.Text = dt.Rows[0]["mahkeme"].ToString();
                fkSatislar.Text = dt.Rows[0]["fkSatislar"].ToString();

                Taksitler();
            }
        }

        private void ceTaksitTutari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //decimal d = ToplamTutar.Value / ceTaksitTutari.Value;
                //d = decimal.Round(d,0);
                //TAdet.Value = d;

                TAdet.Focus();
            }
        }

        private void frmTaksitGirisi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
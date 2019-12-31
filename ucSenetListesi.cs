using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
//using DevExpress.XtraPrinting;
//using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using System.Collections;
using System.Data.SqlClient;
using GPTS.islemler;
using DevExpress.XtraGrid.Views.Grid;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class ucSenetListesi : DevExpress.XtraEditors.XtraUserControl
    {
        public ucSenetListesi()
        {
            InitializeComponent();
        }
       
        private void ucBankaListesi_Load(object sender, EventArgs e)
        {
            taksitlistesi();

            if (this.Tag!=null)
            {
                if (this.Tag.ToString() != "")
                {
                    DevExpress.XtraGrid.Views.Base.ColumnView view = gridView1;
                    view.ActiveFilter.Add(view.Columns["Firmaadi"],
                     new DevExpress.XtraGrid.Columns.ColumnFilterInfo("[Firmaadi] Like '" + this.Tag.ToString() + "'", ""));
                }
            }

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimYukle(gridView1, "SenetlerGrid");

            gridView1.ExpandAllGroups();
            gridView1.ExpandGroupRow(0);
               //gridView1.ActiveFilterString = this.Tag.ToString();
        }

        void taksitlistesi()
        {
            string sql = @"SELECT T.taksit_id,pkTaksitler,T.fkFirma,pkFirma,F.Firmaadi,TL.Odenecek,TL.Odenen,TL.Aciklama,TL.Tarih,
            
            case when (TL.Odenecek - TL.Odenen)=0 then '0'
            when (TL.Odenecek - TL.Odenen)<0 then '1'
            when (TL.Odenecek - TL.Odenen)>0 and TL.Tarih>GETDATE() then '2'
            when (TL.Odenecek - TL.Odenen)>0 and TL.Tarih<GETDATE() then '3' 
            else '4' end as Durumu,T.fkSube,

            TL.OdendigiTarih,TL.SiraNo,TL.OdemeSekli,T.fkSatislar,TL.Odenecek-TL.Odenen as Kalan
            FROM Taksit T with(nolock)
            left join Taksitler TL with(nolock) on TL.taksit_id=T.taksit_id
            left join Firmalar F with(nolock) on T.fkFirma=F.pkFirma
where 1=1";

            if (cbTarihAraligi.SelectedIndex == 0)
                sql = sql + " and TL.Tarih<getdate() and Odenecek<>Odenen";
            else if (cbTarihAraligi.SelectedIndex == 1)
                sql = sql + " and TL.Tarih<getdate()+30 and Odenecek<>Odenen";
            //else if (cbTarihAraligi.SelectedIndex == 2)
            //    sql = sql + " where TL.Odenecek<>TL.Odenen";

            sql = sql + " and (isnull(T.fkSube,0)=0 or T.fkSube=" + Degerler.fkSube+")";

            gridControl1.DataSource = DB.GetData(sql);
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmTaksitOdemeleri h = new frmTaksitOdemeleri();
            h.ShowDialog();
            taksitlistesi();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmCekGirisi SenetGirisi = new frmCekGirisi("0");
            SenetGirisi.Tag = "1";
            SenetGirisi.ShowDialog();
            taksitlistesi();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmCekGirisi SenetGirisi = new frmCekGirisi("0");
            SenetGirisi.Tag = "1";
            if (SenetGirisi.Tag.ToString()=="1")
                SenetGirisi.pkCek.Text = dr["pkTaksitler"].ToString();
            else
            SenetGirisi.pkCek.Text = dr["pkCek"].ToString();
            SenetGirisi.ShowDialog();
            taksitlistesi();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            taksitlistesi();

            gridView1.ExpandAllGroups();
            gridView1.ExpandGroupRow(0);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl1, "A4");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkTaksitler = dr["pkTaksitler"].ToString();
            string fkFirma = dr["fkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkFirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.pkTaksitler.Text = pkTaksitler;
            KasaGirisi.cbOdemeSekli.SelectedIndex = 3;
            
            KasaGirisi.tEaciklama.Text = dr["Tarih"].ToString() + "-Taksit Ödemesi-" + dr["Odenecek"].ToString();
            KasaGirisi.ceTutarNakit.EditValue = dr["Odenecek"].ToString().Replace(",",".");
            
            KasaGirisi.ShowDialog();

            taksitlistesi();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkFirma = dr["PkFirma"].ToString();
            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + pkFirma);//len(Cep)>9 and 

            if (dt.Rows.Count > 0)
            {
                string ceptel = dt.Rows[0]["Cep"].ToString();
                string ceptel2 = dt.Rows[0]["Cep2"].ToString();
                if (ceptel.Length < 9) ceptel = ceptel2;
                if (ceptel.Length < 9)
                {
                    formislemleri.Mesajform("Cep Telefon Numarası Hatalı.", "K", 200);
                    return;
                }

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", pkFirma));
                list.Add(new SqlParameter("@CepTel", dt.Rows[0]["Cep"].ToString()));
                list.Add(new SqlParameter("@Mesaj", "Taksit"));
                if (DB.GetData("select * from Sms with(nolock) where Durumu=0 and fkFirma=" + pkFirma).Rows.Count == 0)
                    DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CepTel,Durumu,Mesaj,Tarih) values(@fkFirma,@CepTel,0,@Mesaj,GetDate())", list);
            }

            frmSmsGonder SmsGonder = new frmSmsGonder();
            SmsGonder.ShowDialog();
        }

        private void fişBilgileriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.Text = dr["fkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());

            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
           GridView View = sender as GridView;
           if (e.RowHandle >= 0)
           {
               string okunma = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Durumu"]);

               string odenecek = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Odenecek"]);
               string odenen = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Odenen"]);

               if (okunma.Trim() == "0")
               {
                   e.Appearance.BackColor = Color.GreenYellow;
                   //e.Appearance.BackColor2 = Color.Blue;
               }
               else if (okunma.Trim() == "1")
               {
                   e.Appearance.BackColor = Color.Blue;
                   // e.Appearance.BackColor2 = Color.Red;
               }
               else if (okunma.Trim() == "2")
               {
                   e.Appearance.BackColor = Color.Aqua;
                   // e.Appearance.BackColor2 = Color.Red;
               }
               else if (okunma.Trim() == "3")
               {
                   e.Appearance.BackColor = Color.Red;
                   // e.Appearance.BackColor2 = Color.Red;
               }
               else
                   e.Appearance.BackColor = Color.Aqua;

               if (odenecek != odenen && odenen != "0,00 TL")
                   e.Appearance.BackColor = Color.PaleVioletRed;
           }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPage2)
                pivotGridControl1.DataSource = DB.GetData("select Tarih,Odenecek,Odenen,(Odenecek -Odenen) AS Kalan from Taksitler with(nolock)");
        }

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            ftmTaksitKarti taksit = new ftmTaksitKarti(dr["pkTaksitler"].ToString());
            taksit.Text = dr["SiraNo"].ToString() + ". Taksit Bilgisi";
            taksit.ShowDialog();

            taksitlistesi();

            gridView1.FocusedRowHandle = i;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkTaksitler = dr["pkTaksitler"].ToString();
            string taksit_id = dr["taksit_id"].ToString();

            if (DB.GetData("select * from KasaHareket with(nolock) where fkTaksitler=" + pkTaksitler).Rows.Count > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Önce, Taksit Ödemesini Siliniz!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Taksit Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("delete from Taksitler where pkTaksitler=" + pkTaksitler);

            //if (DB.GetData("select count(*) from Taksitler with(nolock) where taksit_id=" + taksit_id).Rows[0][0].ToString() == "0")
            //    DB.ExecuteSQL("delete from Taksit where taksit_id=" + taksit_id);
            DataTable dtTak = DB.GetData("select count(*) from Taksitler with(nolock) taksit_id=" + taksit_id);
            if (dtTak.Rows.Count > 0 && dtTak.Rows[0][0].ToString() == "0")
                DB.ExecuteSQL("delete from Taksit where taksit_id=" + taksit_id);
            taksitlistesi();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimKaydet(gridView1, "SenetlerGrid");
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

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimSil("SenetlerGrid");
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //ilk
            //this.CurrentPageIndex = 1;
            //this.dataGridView1.DataSource = GetCurrentRecords(this.CurrentPageIndex, con);
        }

        //SqlConnection con;
        //private SqlCommand cmd1;
        //private SqlCommand cmd2;
        //private SqlDataAdapter adp1;
        //DataSet ds;

        //private int PageSize = 10;
        //private int CurrentPageIndex = 1;
        //private int TotalPage = 0;
        //private DataTable GetCurrentRecords(int page, SqlConnection con)
        //{
        //    con.ConnectionString= DB.ConnectionString();
        //    DataTable dt = new DataTable();

        //    if (page == 1)
        //    {
        //        cmd2 = new SqlCommand("Select TOP " + PageSize + " * from Firmalar ORDER BY pkFirma", con);
        //    }
        //    else
        //    {
        //        int PreviouspageLimit = (page - 1) * PageSize;

        //        cmd2 = new SqlCommand("Select TOP " + PageSize +
        //            " * from Firmalar " +
        //            "WHERE pkFirma NOT IN " +
        //        "(Select TOP " + PreviouspageLimit + " pkFirma from Firmalar ORDER BY pkFirma) ", con); // +
        //        //"order by customerid", con);
        //    }
        //    try
        //    {
        //        // con.Open();
        //        this.adp1.SelectCommand = cmd2;
        //        this.adp1.Fill(dt);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return dt;
        //}
    }
}

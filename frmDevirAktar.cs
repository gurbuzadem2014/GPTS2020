using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.islemler;
using System.Data.SqlClient;
using System.Collections;
using GPTS.Include.Data;
using System.Diagnostics;

namespace GPTS
{
    public partial class frmDevirAktar : DevExpress.XtraEditors.XtraForm
    {
        public frmDevirAktar()
        {
            InitializeComponent();
        }
        public  static string SqlDS = ".", SqlDB="MTP2012";

        private void frmDevirAktar_Load(object sender, EventArgs e)
        {
            txtSqlServerKaynak.Text = DB.VeriTabaniAdresi;
            txtSqlServerHedef.Text = DB.VeriTabaniAdresi;

            SqlDS = txtSqlServerKaynak.Text;
            SqlDB = txtHedefDB.Text;

            MusteriListesi();
        }

        private static string ConnectionString()
        {
            string cs = "Data Source=" + SqlDS + ";Initial Catalog=" + SqlDB + ";Persist Security Info=True;User ID=hitityazilim;Password=hitit9999";
            return cs;
        }

        public static SqlConnection conTrans = null;
        public static SqlTransaction transaction;
        #region uzak server db

        public static DataTable GetData_Web_sil(string sql)
        {
            ////SqlConnection con = new SqlConnection(ConnectionString());
            //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            //adp.SelectCommand.CommandTimeout = 60;

            DataTable dt = new DataTable();
            //try
            //{

            //    adp.Fill(dt);
            //}
            //catch (SqlException e)
            //{
            //    MessageBox.Show("Veritabanı Hatası Oluştu " + e.Message.ToString());
            //}
            //finally
            //{
            //    con.Dispose();
            //    adp.Dispose();
            //}
            return dt;
        }

        public static string ExecuteSQL_Web(string sql, ArrayList par)
        {
            string r = "0";
            SqlCommand cmd = new SqlCommand(sql, conTrans);
            cmd.Transaction = transaction;

            foreach (SqlParameter p in par)
                cmd.Parameters.Add(p);
            try
            {
                //con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu Execute trans: " + exp.Message.ToString();
            }
            cmd.Dispose();
            return r;
        }
        #endregion

        private void BtnKaydet_Click(object sender, EventArgs e)
        {

            SqlDS = txtSqlServerKaynak.Text;
            SqlDB = txtHedefDB.Text;

//            ArrayList list = new ArrayList();
//            list.Add(new SqlParameter("@fkKullanicilar", fkKullanicilar.Text));
//            list.Add(new SqlParameter("@eposta", txtEposta.Text));
//            list.Add(new SqlParameter("@oncelik", cbBirimi.SelectedIndex));
//            list.Add(new SqlParameter("@konu", txtKonu.Text));
//            list.Add(new SqlParameter("@mesaj", meMesaj.Text));

//            string sql = @"insert into Destek (fkKullanicilar,eposta,oncelik,konu,mesaj,tarih,durumu,sonuc) 
//                        values(@fkKullanicilar,@eposta,@oncelik,@konu,@mesaj,getdate(),1,'İşleminiz İletildi...')";
//            ExecuteSQL_Web(sql,list);

            
//            bool b = DB.epostagonder("destek@hitityazilim.com", meMesaj.Text, "", txtKonu.Text);
//            if (b == true)
//            {
//                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
//                meMesaj.Text = "";
//                txtKonu.Text = "";
//            }
//            else
//            {
//                formislemleri.Mesajform("E-Posta Gönderilemedi.", "K", 200);
//            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //MusteriDestekDetayTalepleri(dr["pkDestek"].ToString());
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.hitityazilim.com/destek.aspx");
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            //string mesaj = dr["mesaj"].ToString();
            //MessageBox.Show(mesaj);
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        void MusteriListesi()
        {
            gridControl3.DataSource = DB.GetData("select * from Firmalar with(nolock)");
        }

        void TedarikcilerListesi()
        {
            gridControl1.DataSource = DB.GetData("select * from Tedarikciler with(nolock)");
        }
        
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPage1)
            {
                MusteriListesi();
            }
            else if (e.Page == xtraTabPage2)
            {
                TedarikcilerListesi();
            }
            else if (e.Page == xtraTabPage3)
            {

            }

        }

        private void btnMusDevir_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kaynak Veritabanı: " + txtKaynakDB.Text + " Hedef Veritabanı: " + txtHedefDB.Text + " Firma Bakiyeleri Devir(Kasa Hareketi) Olarak Aktarılacaktır. Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DataTable dtMus= DB.GetData("select * from Firmalar with(nolock)");
            if (dtMus.Rows.Count == 0)
            {
                MessageBox.Show("Müşteri Bulunamadı.");
                return;
            }

            #region trans başlat
            if (conTrans == null)
                conTrans = new SqlConnection(ConnectionString());

            if (conTrans.State == ConnectionState.Closed)
                conTrans.Open();
            #endregion

            #region transları başlat
            transaction = conTrans.BeginTransaction("DevirTransaction");
            #endregion

            foreach (DataRow  dr in dtMus.Rows)
        	{
                string pkFirma = dr["pkFirma"].ToString();
                decimal devir = decimal.Parse(dr["Devir"].ToString());


                string sql = @"INSERT INTO KasaHareket (fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,aktarildi)
                    values(@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Bakiye Düzeltme',@Tutar,@aktarildi)";

                ArrayList list0 = new ArrayList();
                list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

                if (devir > 0)
                {
                    list0.Add(new SqlParameter("@Alacak", devir.ToString().Replace(",", ".")));
                    list0.Add(new SqlParameter("@Borc", "0"));
                }
                else
                {
                    list0.Add(new SqlParameter("@Alacak", "0"));
                    list0.Add(new SqlParameter("@Borc", devir.ToString().Replace(",", ".").Replace("-", "")));
                }
                list0.Add(new SqlParameter("@Tutar", "0"));

                list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
                list0.Add(new SqlParameter("@Aciklama", "Dönem Devir"));
                list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
                list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
                list0.Add(new SqlParameter("@fkFirma", pkFirma));
                list0.Add(new SqlParameter("@AktifHesap", "0"));
                list0.Add(new SqlParameter("@Tarih", DateTime.Now));
                list0.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
                string sonuc = ExecuteSQL_Web(sql, list0);
                if (sonuc.Substring(0,1) == "H")
                {
                    
                    transaction.Rollback();
                    conTrans.Close();
                    MessageBox.Show(sonuc);
                    return;
                }
        	}
            transaction.Commit();
            conTrans.Close();

            conTrans = null;
            MessageBox.Show("Devir İşlemi Yapıldı");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kasa Hareket Silinecektir. Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            #region trans başlat
            if (conTrans == null)
                conTrans = new SqlConnection(ConnectionString());

            if (conTrans.State == ConnectionState.Closed)
                conTrans.Open();
            #endregion

            #region transları başlat
            transaction = conTrans.BeginTransaction("SilTransaction");
            #endregion

            try
            {
                string sql = "truncate table KasaHareket";
                ArrayList list = new ArrayList();
                ExecuteSQL_Web(sql, list);
            }
            catch (Exception exp)
            {
                MessageBox.Show("KasaHareket silinerken hata oluştu " + exp.Message);
                transaction.Rollback();
            }
            finally
            {
                transaction.Commit();
            }
            conTrans.Close();
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Hareket Silinecektir. Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            #region trans başlat
            if (conTrans == null)
                conTrans = new SqlConnection(ConnectionString());

            if (conTrans.State == ConnectionState.Closed)
                conTrans.Open();
            #endregion

            #region transları başlat
            transaction = conTrans.BeginTransaction("SilTransaction");
            #endregion

            try
            {
                string sql = "truncate table SatisDetay";
                ArrayList list = new ArrayList();
                ExecuteSQL_Web(sql, list);

                sql = "truncate table Satislar";
                ArrayList list2 = new ArrayList();
                ExecuteSQL_Web(sql, list2);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Satışlar silinerken hata oluştu " + exp.Message);
                transaction.Rollback();
            }
            finally
            {
                transaction.Commit();
            }
            conTrans.Close();
        }
    }
}
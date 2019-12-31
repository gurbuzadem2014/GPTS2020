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
    public partial class frmDestek : DevExpress.XtraEditors.XtraForm
    {
        public frmDestek()
        {
            InitializeComponent();
        }
        public static string ConnectionString()
        {
            string cs = "Data Source=sql2012.isimtescil.net;Initial Catalog=hitityazilim_db9999;Persist Security Info=True;User ID=hitityazilim_adem;Password=Hitit9999";
            return cs;
        }

        #region uzak server db

        public static DataTable GetData_Web(string sql)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            adp.SelectCommand.CommandTimeout = 60;

            DataTable dt = new DataTable();
            try
            {

                adp.Fill(dt);
            }
            catch (SqlException e)
            {
                MessageBox.Show("Veritabanı Hatası Oluştu " + e.Message.ToString());
            }
            finally
            {
                con.Dispose();
                adp.Dispose();
            }
            return dt;
        }

        public static string ExecuteSQL_Web(string sql, ArrayList par)
        {
            string r = "0";
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, con);

            foreach (SqlParameter p in par)
                cmd.Parameters.Add(p);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu: " + exp.Message.ToString();
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                               // exp.Message.ToString().Replace("'", "") + ",getdate(),0)");
                //logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
            }

            con.Close();
            con.Dispose();
            cmd.Dispose();
            return r;
        }
        #endregion

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (meMesaj.Text == "")
            {
                meMesaj.Text = cbKonu.Text;
            //    islemler.formislemleri.Mesajform("Mesaj Giriniz", "K",200);
            //    meMesaj.Focus();
            //    return;
            }
            if (cbKonu.Text == "")
            {
                islemler.formislemleri.Mesajform("Konu Giriniz", "K", 200);
                cbKonu.Focus();
                return;
            }



            DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
            if (dtSirket.Rows.Count == 0)
            {
                return;
            }

            string Sirket = dtSirket.Rows[0]["Sirket"].ToString();
            string eposta = dtSirket.Rows[0]["eposta"].ToString();
            string TelefonNo = dtSirket.Rows[0]["TelefonNo"].ToString();
            string kul = "";
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                //ws.Url = "http://localhost:49732/WebService.asmx";
                //DB.InternetVarmi22
                if (ws != null)
                    kul = ws.DestekEkle(fkKullanicilar.Text, eposta, cbKonu.Text, meMesaj.Text);
            }
            catch (Exception exp)
            {
                DB.logayaz("internet bağlantısı yok", "lisans bilgisi bulunamadı = " + eposta);
                //throw;
            }
            finally
            {

            }
            //insertdestek(string eposta, string konu, string aciklama, string fkKullanici)

            #region espota gönder
            string b = DB.epostagonder("destek@hitityazilim.com", meMesaj.Text, "", cbKonu.Text);
            if (b == "OK")
            {
                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
                meMesaj.Text = "";
                cbKonu.Text = "";
            }
            else
            {
                formislemleri.Mesajform(b, "K", 200);
            }
            #endregion

            Desteklistesi(fkKullanicilar.Text);
            //MusteriDestekTalepleri(fkKullanicilar.Text);
        }

        void MusteriDestekTalepleri_sil(string kulid)
        {
            wshitityazilim.WebService ws = new wshitityazilim.WebService();
            //ws.Url = "http://localhost:49732/WebService.asmx";
            //DB.InternetVarmi22
            DataTable dt=null;
            if (ws != null)
                dt = ws.DestekList(kulid);

            gcDestek.DataSource = dt;//GetData_Web(sql);
        }

        void MusteriDestekDetayTalepleri(string fkDestek)
        {
            //Desteklistesi();
            DestekDetaylistesi(fkDestek);
            //gcDestekDetay.DataSource = 
            //GetData_Web("select pkDestekDetay,Mesaj,Tarih,fkDestek from DestekDetay where fkDestek=" + fkDestek);
        }

        private void frmDestek_Load(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from Sirketler with(nolock)");

            txtEposta.Text = dt.Rows[0]["eposta"].ToString();

            if (txtEposta.Text == "")
            {
                MessageBox.Show("Lüten Ayarlardan E-Posta Adresini Giriniz");
                return;
            }

            //epostadadan şirket id getir

            wshitityazilim.WebService ws = new wshitityazilim.WebService();
            //ws.Url = "http://localhost:49732/WebService.asmx";
            //DB.InternetVarmi22
            DataTable dt2 = null;
            if (ws != null)
                dt2 = ws.EpostadanKulId(txtEposta.Text);

            if (dt2.Rows.Count > 0)
                fkKullanicilar.Text = dt2.Rows[0][0].ToString();
            //string sql =@"select d.pkDestek,d.konu,d.mesaj,d.sonuc,d.durumu,d.oncelik,k.KullaniciAdi,d.tarih,k.Aciklama,d.eposta,k.Kiralik from Destek d with(nolock)
            //left join Kullanicilar k on k.pkKullanicilar=d.fkKullanicilar where 1=1";

            

            //DataTable dbWebKul =
            //GetData_Web("select * from Kullanicilar with(nolock) where Eposta='"+
            //txtEposta.Text+"'");

            //if (dbWebKul.Rows.Count == 0)
            //{
            //    MessageBox.Show("Kullanıcı Bulunamadı.\n Lütfen Ayarlardan Lisans İşlemleri Yapınız");
            //    return;
            //}
            //fkKullanicilar.Text = dbWebKul.Rows[0]["pkKullanicilar"].ToString();

            Desteklistesi(fkKullanicilar.Text);
            //MusteriDestekTalepleri();
        }

        private void tamamlandıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();
            string sonuc = "";
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                //ws.Url = "http://localhost:49732/WebService.asmx";
                if (ws != null)
                    sonuc = ws.DestekGuncelle(pkDestek, "Tamamlandı");
            }
            catch (Exception exp)
            {
                DB.logayaz("internet bağlantısı yok", "lisans bilgisi bulunamadı");
                //throw;
            }
            finally
            {

            }
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@pkDestek", pkDestek));

            //string sql = @"update Destek set sonuc='Tamamlandı' where pkDestek=@pkDestek";
            //ExecuteSQL_Web(sql,list);

        }

        private void çalışılıyorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();
            string mesaj = dr["mesaj"].ToString();
            string sonuc = "";
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                //ws.Url = "http://localhost:49732/WebService.asmx";
                if (ws != null)
                    sonuc = ws.DestekGuncelle(pkDestek, "Çalışılıyor...");
            }
            catch (Exception exp)
            {
                DB.logayaz("internet bağlantısı yok", "lisans bilgisi bulunamadı");
                //throw;
            }
            finally
            {

            }
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@pkDestek", pkDestek));

            //string sql = @"update Destek set sonuc='Çalışılıyor...' where pkDestek=@pkDestek";
            //ExecuteSQL_Web(sql, list);

            string  b = DB.epostagonder("destek@hitityazilim.com", mesaj, "", pkDestek + " Çalışılıyor...");
            if (b == "OK")
            {
                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
                //meMesaj.Text = "";
                //txtKonu.Text = "";
            }
            else
            {
                formislemleri.Mesajform(b, "K", 200);
            }
        }

        void Desteklistesi(string kulid)
        {
            wshitityazilim.WebService ws = new wshitityazilim.WebService();
            //ws.Url = "http://localhost:49732/WebService.asmx";
            //DB.InternetVarmi22
            DataTable dt = null;
            if (ws != null)
                dt = ws.DestekList(kulid);

            //string sql =@"select d.pkDestek,d.konu,d.mesaj,d.sonuc,d.durumu,d.oncelik,k.KullaniciAdi,d.tarih,k.Aciklama,d.eposta,k.Kiralik from Destek d with(nolock)
            //left join Kullanicilar k on k.pkKullanicilar=d.fkKullanicilar where 1=1";

            //if (cbTumu.Checked == false)
            //    sql = sql + " and sonuc<>'Tamamlandı'";

            //sql = sql + " and d.fkKullanicilar=" + fkKullanicilar.Text;

            gcDestek.DataSource = dt;//GetData_Web(sql);
            //string sql = @"select d.pkDestek,d.konu,d.mesaj,d.sonuc,d.durumu,d.oncelik,k.KullaniciAdi,d.tarih,k.Aciklama,d.eposta,k.Kiralik from Destek d with(nolock)
            //left join Kullanicilar k on k.pkKullanicilar=d.fkKullanicilar";

            //if (cbTumu.Checked == false)
            //    sql = sql + " where sonuc<>'Tamamlandı'";

            //gcDestek.DataSource = GetData_Web(sql);
        }

        void DestekDetaylistesi(string destekid)
        {
            wshitityazilim.WebService ws = new wshitityazilim.WebService();
            //ws.Url = "http://localhost:49732/WebService.asmx";
            //DB.InternetVarmi22
            DataTable dt = null;
            if (ws != null)
            {
                try
                {
                    dt = ws.DestekDetayList(destekid);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Hata Oluştu : " + exp.Message);
                    //throw;
                }
                
            }
            //string sql =@"select d.pkDestek,d.konu,d.mesaj,d.sonuc,d.durumu,d.oncelik,k.KullaniciAdi,d.tarih,k.Aciklama,d.eposta,k.Kiralik from Destek d with(nolock)
            //left join Kullanicilar k on k.pkKullanicilar=d.fkKullanicilar where 1=1";

            //if (cbTumu.Checked == false)
            //    sql = sql + " and sonuc<>'Tamamlandı'";

            //sql = sql + " and d.fkKullanicilar=" + fkKullanicilar.Text;

            gcDestekDetay.DataSource = dt;//GetData_Web(sql);
            //string sql = @"select d.pkDestek,d.konu,d.mesaj,d.sonuc,d.durumu,d.oncelik,k.KullaniciAdi,d.tarih,k.Aciklama,d.eposta,k.Kiralik from Destek d with(nolock)
            //left join Kullanicilar k on k.pkKullanicilar=d.fkKullanicilar";

            //if (cbTumu.Checked == false)
            //    sql = sql + " where sonuc<>'Tamamlandı'";

            //gcDestek.DataSource = GetData_Web(sql);
        }


        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkDestek = dr["pkDestek"].ToString();

            DestekDetaylistesi(pkDestek);

            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //MusteriDestekDetayTalepleri(dr["pkDestek"].ToString());
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkDestek = dr["pkDestek"].ToString();
            string konu = dr["konu"].ToString();
            //string tarih = dr["Tarih"].ToString();
            //string gidecekeposta = dr["eposta"].ToString();


            DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
            if (dtSirket.Rows.Count == 0)
            {
                return;
            }

            string Sirket = dtSirket.Rows[0]["Sirket"].ToString();
            string eposta = dtSirket.Rows[0]["eposta"].ToString();
            string sonuc = "";
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                //ws.Url = "http://localhost:49732/WebService.asmx";
                //DB.InternetVarmi22
                if (ws != null)
                    sonuc = ws.DestekDetayEkle(pkDestek, memoEdit1.Text);
            }
            catch (Exception exp)
            {
                DB.logayaz("internet bağlantısı yok", "lisans bilgisi bulunamadı = " + sonuc);
                //throw;
            }
            finally
            {

            }


            string  b = DB.epostagonder("destek@hitityazilim.com", memoEdit1.Text, "", "Destek No:" +pkDestek +" konu="+ konu );
            //b = DB.epostagonder(gidecekeposta, memoEdit1.Text, "", "Destek No:" + pkDestek + " konu=" + konu);
            
            if (b == "OK")
            {
                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
                meMesaj.Text = "";
                cbKonu.Text = "";
            }
            else
            {
                formislemleri.Mesajform(b, "K", 200);
            }


            MusteriDestekDetayTalepleri(pkDestek);

            memoEdit1.Text = "";
        }

        private void beklemedeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();

            string sonuc = "";
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                //ws.Url = "http://localhost:49732/WebService.asmx";
                if (ws != null)
                    sonuc = ws.DestekGuncelle(pkDestek, "Beklemede.");
            }
            catch (Exception exp)
            {
                DB.logayaz("internet bağlantısı yok", "lisans bilgisi bulunamadı");
                //throw;
            }
            finally
            {

            }
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@pkDestek", pkDestek));

            //string sql = @"update Destek set sonuc='Beklemede.' where pkDestek=@pkDestek";
            //ExecuteSQL_Web(sql, list);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Mesaj Hareketini Silmek istediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkDestekDetay = dr["pkDestekDetay"].ToString();
            string fkDestek = dr["fkDestek"].ToString();
            
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkDestekDetay", pkDestekDetay));
            string sql = @"delete From DestekDetay where pkDestekDetay=@pkDestekDetay";
            ExecuteSQL_Web(sql,list);

            MusteriDestekDetayTalepleri(fkDestek);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.hitityazilim.com/destek.aspx");
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();
            string sonuc = "";
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                //ws.Url = "http://localhost:49732/WebService.asmx";
                if (ws != null)
                    sonuc = ws.DestekGuncelle(pkDestek, "Silindi.");
            }
            catch (Exception exp)
            {
                DB.logayaz("internet bağlantısı yok", "lisans bilgisi bulunamadı");
                //throw;
            }
            finally
            {

            }
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@pkDestek", pkDestek));

            //string sql = @"delete from Destek where pkDestek=@pkDestek";
            //ExecuteSQL_Web(sql, list);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string mesaj = dr["mesaj"].ToString();
            MessageBox.Show(mesaj);
        }

        private void testEdilecekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();
            string mesaj = dr["mesaj"].ToString();
            string sonuc = "";
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                //ws.Url = "http://localhost:49732/WebService.asmx";
                if (ws != null)
                    sonuc = ws.DestekGuncelle(pkDestek, "Test Edilecek");
            }
            catch (Exception exp)
            {
                DB.logayaz("internet bağlantısı yok", "lisans bilgisi bulunamadı ");
                //throw;
            }
            finally
            {

            }
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@pkDestek", pkDestek));

            //string sql = @"update Destek set sonuc='Test Edilecek' where pkDestek=@pkDestek";
            //ExecuteSQL_Web(sql, list);

            //eposta gönde
            string b = DB.epostagonder("destek@hitityazilim.com", mesaj, "", pkDestek + " Test Edilecek");
            if (b == "OK")
            {
                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
                //meMesaj.Text = "";
                //txtKonu.Text = "";
            }
            else
            {
                formislemleri.Mesajform(b, "K", 200);
            }
           
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkDestek", pkDestek));

            string sql = @"update Destek set oncelik='1' where pkDestek=@pkDestek";
            ExecuteSQL_Web(sql, list);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkDestek", pkDestek));

            string sql = @"update Destek set oncelik='2' where pkDestek=@pkDestek";
            ExecuteSQL_Web(sql, list);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkDestek", pkDestek));

            string sql = @"update Destek set oncelik='3' where pkDestek=@pkDestek";
            ExecuteSQL_Web(sql, list);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkDestek", pkDestek));

            string sql = @"update Destek set oncelik='4' where pkDestek=@pkDestek";
            ExecuteSQL_Web(sql, list);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            string pkDestek = dr["pkDestek"].ToString();

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkDestek", pkDestek));

            string sql = @"update Destek set oncelik='5' where pkDestek=@pkDestek";
            ExecuteSQL_Web(sql, list);
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {
            Desteklistesi("0");
        }

        private void kullanıcılarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string s =  formislemleri.inputbox("Şifre", "Şifre Giriniz", "", true);
            //if (s == "hitit9999")
            //{
            //    frmDestekKullanici DestekKullanici = new frmDestekKullanici();
            //    DestekKullanici.Show();
            //}

        }
        bool dragging;
        Point offset;
        private void frmDestek_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            offset = e.Location;
        }

        private void frmDestek_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void frmDestek_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new
                Point(currentScreenPos.X - offset.X,
                currentScreenPos.Y - offset.Y);
            }
        }
    }
}
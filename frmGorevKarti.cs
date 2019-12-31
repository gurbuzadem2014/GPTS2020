using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmGorevKarti : Form
    {
        public frmGorevKarti()
        {
            InitializeComponent();
        }
        void gridyukle()
        {
            DataTable dt = DB.GetData("select top 1 WebKullanir from dbo.Sirketler");
            gridControl1.DataSource = DB.GetData("Select * from Bolumler with(nolock)");
        }
        private void frmStokGrupKarti_Load(object sender, EventArgs e)
        {
            gridyukle();
        }
        void KullaniciKontrol(string KullaniciAdi)
        {
            try
            {
                SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=mssql02.turhost.com;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityazilim;Password=hitit9999");
                string sql = "select * from kullanicilar where KullaniciAdi='"+KullaniciAdi+"'";// where username='@username' and password='@password'";
                //sql = sql.Replace("@username", username);
                //sql = sql.Replace("@password", password);
                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                adp.SelectCommand.CommandTimeout = 60;

                DataTable dt = new DataTable();
                try
                {
                    adp.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        DB.webpkKullanicilar = 0;
                        XtraMessageBox.Show("Lisans için Lütfen 0262 644 51 12 arayınız.", "Hitit Yazılım", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    DB.webpkKullanicilar=int.Parse(dt.Rows[0]["pkKullanicilar"].ToString());
                    //Degerler.userid = dt.Rows[0]["sirano"].ToString();
                    //Degerler.username = dt.Rows[0]["username"].ToString()
                    //Degerler.password = dt.Rows[0]["password"].ToString();
                    //Degerler.smsadet = dt.Rows[0]["smsadet"].ToString();
                    //Degerler.smskullanilan = dt.Rows[0]["kullanilan"].ToString();
                    //Degerler.yetkili = dt.Rows[0]["yetkili"].ToString();
                    //Degerler.merkezadi = dt.Rows[0]["hastaneadi"].ToString();
                    //Degerler.ilce = dt.Rows[0]["ilce"].ToString();
                    //Degerler.mumessil = dt.Rows[0]["mumessil"].ToString();
                    //Degerler.sehir = dt.Rows[0]["sehir"].ToString();
                    //Degerler.vol = dt.Rows[0]["versiyon"].ToString();
                    //Degerler.aktif = dt.Rows[0]["aktif"].ToString();
                    //Degerler.smsuser = dt.Rows[0]["smsuser"].ToString();
                    //Degerler.smspass = dt.Rows[0]["smspass"].ToString();
                    //Degerler.smsbayi = dt.Rows[0]["smsbayikodu"].ToString();
                    //Degerler.smssender = dt.Rows[0]["smssender"].ToString();
                }
                catch (Exception exp)
                {
                    return;
                }
                finally
                {
                    con.Dispose();
                    adp.Dispose();
                }
                if (dt.Rows.Count == 0)
                {
                    //Degerler.kayitli = 0;
                    return;
                }
                //else
                    //Degerler.kayitli = dt.Rows.Count;
                //
            }
            catch (Exception exp)
            {
                XtraMessageBox.Show("Lisans için Lütfen Yazılım firmasını arayınız.", "Termo Takip", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
                // Application.Exit();
            }
            finally
            {
                //con.Dispose();
                //adp.Dispose();
            }
            return;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokGrup = dr["pkStokGrup"].ToString();
            if (DB.GetData("select pkStokKarti from StokKarti where fkStokGrup=" + fkStokGrup).Rows.Count > 0)
            {
                MessageBox.Show("Bu Görev Daha Önce Kullanıldı.Lütfen Önce Personel Görev listesinden Değiştiriniz.");
                return;
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["StokGrup"].ToString() + " Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No)
                return;

            DB.ExecuteSQL("DELETE FROM Bolumler WHERE pkBolumler=" + dr["pkBolumler"].ToString());
            gridView1.DeleteSelectedRows();
            //gridyukle();
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            kAYDETToolStripMenuItem_Click(sender,e);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            yeniToolStripMenuItem_Click(sender, e);
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(sender, e);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            gridColumn2.OptionsColumn.AllowEdit = true;
        }

        void kaydet()
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE Bolumler SET bolumadi='" + dr["bolumadi"].ToString() + "' WHERE pkBolumler=" + dr["pkBolumler"].ToString());
            }
        }

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kaydet();
            frmGorevYeniKarti GorevYeniKarti = new frmGorevYeniKarti();
            GorevYeniKarti.Tag = 1;
            GorevYeniKarti.ShowDialog();
            gridyukle();
        }

        private void kAYDETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kaydet();
            Close();
        }

        private void frmStokGrupKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}

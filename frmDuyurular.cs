using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class frmDuyurular : DevExpress.XtraEditors.XtraForm
    {
        public frmDuyurular()
        {
            InitializeComponent();
        }

        void Duyurular()
        {
            SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=sql2012.isimtescil.net;Initial Catalog=hitityazilim_db9999;Persist Security Info=True;User ID=hitityazilim_adem;Password=Hitit9999");
            string sql = "select * from Duyurular with(nolock)";
            //sql = sql.Replace("@KullaniciAdi", username);
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);

            DataTable dt = new DataTable();
            try
            {
                adp.Fill(dt);
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

            gridControl1.DataSource = dt;
            //if (dt.Rows.Count == 0)
            //{
            //    XtraMessageBox.Show("Lisans için Lütfen Yazılım firmasını arayınız.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    DB.kayitli = 0;
            //    return;
            //}
        }

        private void frmDuyurular_Load(object sender, EventArgs e)
        {
            lLisans.Text = "Lisans Bitiş Tarihi: " + Degerler.LisansBitisTarih.ToString();

            Duyurular();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hitityazilim.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hitityazilim.com");
        }

        private void linkLabel3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hitityazilim.com");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hitityazilim.com/iletisim.aspx");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
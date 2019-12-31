using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GPTS
{
    public partial class frmCikis : DevExpress.XtraEditors.XtraForm
    {
        public frmCikis()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmCikis_Load(object sender, EventArgs e)
        {
            xtraTabControl2.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            if(this.Tag=="0")
              gridControl1.DataSource = DB.GetData("select top 100 * from Logs with(nolock) order by Tarih desc");
           
            else if (this.Tag == "1")
                xtraTabControl2.SelectedTabPageIndex = 2;

        }
        void YedekAl()
        {
            try
            {
                DB.ExecuteSQL("DBCC SHRINKDATABASE (MTP2012, 10);");
            }
            catch (Exception)
            {
                //a
            }

            try
            {
                //string yedekalinacakyer = "c:\\yedek\\PersonelTS.bak";
                DataTable dt = DB.GetData("exec sp_backup");
                if (dt.Rows[0][0].ToString() == "1")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alındı.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Process.Start(DB.GetData("select top 1 yedekalinacakyer from Sirketler").Rows[0][0].ToString());
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alınırken Hata Oluştu.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception exp)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alınırken Hata Oluştu." + exp.Message.ToString(), "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) VALUES(3,convert(varchar(20),getdate(),120)+' Yedek Alındı',getdate(),1)");
            //DB.epostagonder("musteri@hitityazilim.com", sirketadi.Text + " Yedek Aldı", "", ServerAdi.Text);
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            YedekAl();
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hitityazilim.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hitityazilim.com");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
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
    }
}
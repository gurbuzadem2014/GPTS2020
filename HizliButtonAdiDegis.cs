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
    public partial class HizliButtonAdiDegis : DevExpress.XtraEditors.XtraForm
    {
        public HizliButtonAdiDegis()
        {
            InitializeComponent();
        }

        private void barkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string sql = "Update StokKarti SET HizliSatisAdi='" + stokadi.Text + "' where pkStokKarti=" + stokadi.Tag.ToString();
                DB.ExecuteSQL(sql);
                Close();
            }
            if (e.KeyCode == Keys.Escape)
            {
                stokadi.Text = "";
                Close();
            }
        }

        private void HizliButtonAdiDegis_Load(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from StokKarti where Barcode='" + oncekibarkod.Text + "'");
            if(dt.Rows.Count==0) return;
            stokadi.Tag = dt.Rows[0]["pkStokKarti"].ToString();
            stokadi.Text = dt.Rows[0]["HizliSatisAdi"].ToString();
            textEdit1.Text = dt.Rows[0]["StokAdi"].ToString();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            stokadi.Text = textEdit1.Text;
        }
    }
}
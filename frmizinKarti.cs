using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;

namespace GPTS
{
    public partial class frmizinKarti : DevExpress.XtraEditors.XtraForm
    {
        public frmizinKarti()
        {
            InitializeComponent();
        }

        private void frmizinKarti_Load(object sender, EventArgs e)
        {
            if (DB.pkPersoneller == 0) return;
            DataTable dtp = DB.GetData(@"select * from Personeller where pkpersoneller=" + DB.pkPersoneller);
            pkmusteri.Text = dtp.Rows[0]["pkpersoneller"].ToString();
            teAdi.Text = dtp.Rows[0]["adi"].ToString() + " " +dtp.Rows[0]["soyadi"].ToString();
            teGBaslamaTarihi.Text = dtp.Rows[0]["isegiristarih"].ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class frmCekTurleri : DevExpress.XtraEditors.XtraForm
    {
        public frmCekTurleri()
        {
            InitializeComponent();
        }
        void CekTurleriGetir()
        {
            gridControl1.DataSource=
            DB.GetData("select * from CekTurleri with(nolock)");
        }
        private void frmCekTurleri_Load(object sender, EventArgs e)
        {
            CekTurleriGetir();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
			{
                DataRow dr = gridView1.GetDataRow(i);
             
                ArrayList list = new ArrayList();
                //list.Add(new SqlParameter("@Aktif",dr["Aktif"].ToString()));
                list.Add(new SqlParameter("@CekTuru", dr["CekTuru"].ToString()));
                list.Add(new SqlParameter("@Aciklama", dr["Aciklama"].ToString()));
                list.Add(new SqlParameter("@pkCekTurleri", dr["pkCekTurleri"].ToString()));

                DB.ExecuteSQL("update CekTurleri set CekTuru=@CekTuru,Aciklama=@Aciklama where pkCekTurleri=@pkCekTurleri", list);
            }

            CekTurleriGetir();
        }
    }
}
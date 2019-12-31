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
    public partial class frmAracTakip : DevExpress.XtraEditors.XtraForm
    {
        public frmAracTakip()
        {
            InitializeComponent();
        }
        void getir()
        {
            gridControl1.DataSource = DB.GetData("select * from AracTakip");
        }
        private void frmAracTakip_Load(object sender, EventArgs e)
        {
            getir();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkPersonel", "1"));
            list.Add(new SqlParameter("@GidisZaman", gidiszamani.DateTime));
            list.Add(new SqlParameter("@GidisKm", cikiskm.Value));
            list.Add(new SqlParameter("@Aciklama", "Denem"));
            DB.ExecuteSQL(@"INSERT INTO AracTakip (fkPersonel,GidisZaman,GidisKm,Aciklama)
                values(@fkPersonel,@GidisZaman,@GidisKm,@Aciklama)",list);
            getir();
        }
    }
}
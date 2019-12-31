using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GPTS
{
    public partial class ucTaksitOdemeleri : DevExpress.XtraEditors.XtraUserControl
    {
        public ucTaksitOdemeleri()
        {
            InitializeComponent();
            gridControl1.DataSource = DB.GetData("SELECT  * FROM Taksitler");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void schedulerControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {

        }
    }
}

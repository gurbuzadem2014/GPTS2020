using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPTS
{
    public partial class frmAlisFaturasi : Form
    {
        public frmAlisFaturasi()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void Cari_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }

        private void CariNo_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (panelControl6.Visible == false)
                panelControl6.Visible = true;
            else
                panelControl6.Visible = false;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //kaydet dediğinde stoklara işlenecek, stok işle butonu işareti kaldırılırsa
            //stoğa işlemeyecek
        }

        private void Cari_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

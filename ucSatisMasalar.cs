using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using DevExpress.XtraTab;
using System.IO;

namespace GPTS
{
    public partial class ucSatisMasalar : DevExpress.XtraEditors.XtraUserControl
    {
        
        public ucSatisMasalar()
        {
            InitializeComponent();
        }
        
        private void ucSatisMasa_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = DB.GetData("select * from masalar ");
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
           
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
           //MasalariYukle(false);
        }


            

      
      

        private void fişSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void masaDizaynKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void masalarıTemizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void masaYerleriSıfırlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("update Masalar set soldan=null,ustden=null");
            
        }

        private void tümMasalarıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("update Masalar set fkSatislar=0");
            
        }

        private void masaTanımlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasalar masalar = new frmMasalar();
            masalar.ShowDialog();
        }

        private void layoutView1_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}

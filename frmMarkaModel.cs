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
    public partial class frmMarkaModel : DevExpress.XtraEditors.XtraForm
    {
        public frmMarkaModel()
        {
            InitializeComponent();
        }

        private void frmMarkaModel_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = DB.GetData("select * from Markalar");
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            gridControl2.DataSource = DB.GetData("select * from Modeller where fkMarka=" + dr["pkMarka"].ToString());
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("INSERT INTO Modeller (Model,fkMarka) values('" + ModelAdi.Text + "'," + dr["pkMarka"].ToString()+ ")");
            gridControl2.DataSource = DB.GetData("select * from Modeller where fkMarka=" + dr["pkMarka"].ToString());
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("INSERT INTO Markalar (Marka) values('" + MarkaAdi.Text + "')");
            gridControl1.DataSource = DB.GetData("select * from Markalar");
            gridControl2.DataSource = null;
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Delete)
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                DB.ExecuteSQL("DELETE FROM Markalar where pkMarka=" + dr["pkMarka"].ToString());
                gridControl1.DataSource = DB.GetData("select * from Markalar");
                gridControl2.DataSource = null;
            }
        }

        private void gridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                DB.ExecuteSQL("DELETE FROM Modeller where pkModel=" + dr["pkModel"].ToString());
                gridControl2.DataSource = DB.GetData("select * from Modeller where fkMarka=" + dr["fkMarka"].ToString());
            }
        }
    }
}
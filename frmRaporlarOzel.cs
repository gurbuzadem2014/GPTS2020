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
using GPTS.Include.Data;
using System.Data.OleDb;

namespace GPTS
{

    public partial class frmRaporlarOzel : DevExpress.XtraEditors.XtraForm
    {
        public frmRaporlarOzel()
        {
            InitializeComponent();
        }
        private void frmAktarim_Load(object sender, EventArgs e)
        {
            OzelRaporlar();
        }
        private void simpleButton34_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("delete from OzelRaporlar where ozel_rapor_id=" + rapor_adi.Tag.ToString());
        }

        private void simpleButton33_Click(object sender, EventArgs e)
        {
            string id = "0", sql;
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@rapor_adi", rapor_adi.Text));
            list.Add(new SqlParameter("@rapor_sql", meSql.Text));
            if (rapor_adi.Tag.ToString() == "0")
                sql = "insert into OzelRaporlar (rapor_adi,rapor_sql) values(@rapor_adi,@rapor_sql) select IDENT_CURRENT('OzelRaporlar')";
            else
                sql = "update OzelRaporlar set rapor_adi=@rapor_adi,rapor_sql=@rapor_sql where ozel_rapor_id=" + rapor_adi.Tag.ToString();
            id = DB.ExecuteScalarSQL(sql, list);

            rapor_adi.Tag = id;

            OzelRaporlar();
        }
        void OzelRaporlar()
        {
            cgOzelRaporlar.DataSource = DB.GetData("select * from OzelRaporlar with(nolock)");
        }
        private void simpleButton32_Click(object sender, EventArgs e)
        {
            rapor_adi.Tag = "0";
            rapor_adi.Focus();
        }
        private void btnCalistir_Click(object sender, EventArgs e)
        {
            gridView1.BeginDataUpdate();
            gcSql.DataSource = null;
            gridView1.Columns.Clear();
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                gridView1.Columns.RemoveAt(i);
            }
            gridView1.EndDataUpdate();
            gcSql.DataSource = DB.GetData(meSql.Text);
        }
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPage10)
            {
                OzelRaporlar();
            }
        }
        private void gridView14_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView14.FocusedRowHandle < 0) return;
            gcSql.DataSource = null;
            gridView1.Columns.Clear();
            gridView1.BeginDataUpdate();
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                gridView1.Columns.RemoveAt(i);
            }
            DataRow dr = gridView14.GetDataRow(gridView14.FocusedRowHandle);

            rapor_adi.Tag = dr["ozel_rapor_id"].ToString();
            rapor_adi.Text = dr["rapor_adi"].ToString();
            meSql.Text = dr["rapor_sql"].ToString();


            if (meSql.Text != "")
                gcSql.DataSource = DB.GetData(dr["rapor_sql"].ToString());


            gridView1.EndDataUpdate();
        }










       
        

    }
}
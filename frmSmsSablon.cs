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
    public partial class frmSmsSablon : DevExpress.XtraEditors.XtraForm
    {
        public frmSmsSablon()
        {
            InitializeComponent();
        }

        void Sablonlar()
        {
            gridControl1.DataSource = DB.GetData("select * from SmsSablon with(nolock)");
        }
        private void frmSmsSablon_Load(object sender, EventArgs e)
        {
            Sablonlar();
        }
        void temizle()
        {
            teSablonAdi.Tag = "0";
            teSablonAdi.Text = "";
            meMesaj.Text = "";
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            temizle();
            teSablonAdi.Focus();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            string sql = "";

            if(teSablonAdi.Tag == "0")
               sql = "insert into SmsSablon (sablon_adi,sablon) values(@sablon_adi,@sablon)";
            else
                sql = "update SmsSablon set sablon_adi=@sablon_adi,sablon=@sablon where sms_sablon_id=" + teSablonAdi.Tag.ToString();
            
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@sablon_adi", teSablonAdi.Text));
            list.Add(new SqlParameter("@sablon", meMesaj.Text));
            DB.ExecuteSQL(sql,list);


            temizle();
            Sablonlar();

            BtnKaydet.Text = "Kaydet";
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            teSablonAdi.Tag = dr["sms_sablon_id"].ToString();
            teSablonAdi.Text = dr["sablon_adi"].ToString();
            meMesaj.Text = dr["sablon"].ToString();
            BtnKaydet.Text = "Güncelle";
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string sms_sablon_id = dr["sms_sablon_id"].ToString();

            DB.ExecuteSQL("delete from SmsSablon where sms_sablon_id=" + sms_sablon_id);

            Sablonlar();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            meMesaj.Text = meMesaj.Text + "{isim}";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            meMesaj.Text = meMesaj.Text + "{Tutar}";
        }

    }
}
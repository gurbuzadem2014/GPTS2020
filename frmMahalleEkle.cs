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
using GPTS.islemler;

namespace GPTS
{
    public partial class frmMahalleEkle : DevExpress.XtraEditors.XtraForm
    {
        public frmMahalleEkle()
        {
            InitializeComponent();
        }
        void gridyukle()
        {
            gridControl1.DataSource = DB.GetData(@"SELECT * FROM IL_ILCE_MAH with(nolock) WHERE ALTGRUP=" + ilid.Text + "  ORDER BY KODU");
        }
        private void frmilceekle_Load(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (ilceadi.Text == "") return;

            DataRow dr = gridView1.GetDataRow(0);
            
            ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@ADI", ilceadi.Text));
            if (dr == null)
                list.Add(new SqlParameter("@GRUP", "2"));
            else
                list.Add(new SqlParameter("@GRUP", dr["GRUP"].ToString()));

            if (dr == null)
                list.Add(new SqlParameter("@ALTGRUP", ilid.Text));
            else
                list.Add(new SqlParameter("@ALTGRUP", dr["ALTGRUP"].ToString()));
            
            string maxkod =
                DB.GetData("Select MAX(KODU)+1 from IL_ILCE_MAH where GRUP=@GRUP and ALTGRUP=@ALTGRUP", list).Rows[0][0].ToString();
            ArrayList list2 = new ArrayList();
            list2.Add(new SqlParameter("@ADI", ilceadi.Text));
            //list2.Add(new SqlParameter("@KODU", maxkod));

            if (dr == null)
                list2.Add(new SqlParameter("@GRUP", "2"));
            else
                list2.Add(new SqlParameter("@GRUP", dr["GRUP"].ToString()));

            if (dr == null)
                list2.Add(new SqlParameter("@ALTGRUP", ilid.Text));
            else
                list2.Add(new SqlParameter("@ALTGRUP", dr["ALTGRUP"].ToString()));


            if (DB.GetData("Select * from IL_ILCE_MAH where ADI=@ADI and GRUP=@GRUP and ALTGRUP=@ALTGRUP", list2).Rows.Count > 0)
            {
                MessageBox.Show("İlçe Zaten Kayıtlı.");
                return;
            }
            ArrayList list3 = new ArrayList();
            list3.Add(new SqlParameter("@ADI", ilceadi.Text));
            list3.Add(new SqlParameter("@KODU", maxkod));
            if (dr == null)
                list3.Add(new SqlParameter("@GRUP", "2"));
            else
                list3.Add(new SqlParameter("@GRUP", dr["GRUP"].ToString()));

            if (dr == null)
                list3.Add(new SqlParameter("@ALTGRUP", ilid.Text));
            else
                list3.Add(new SqlParameter("@ALTGRUP", dr["ALTGRUP"].ToString()));
            string sonuc = DB.ExecuteSQL("INSERT INTO IL_ILCE_MAH (ADI,KODU,GRUP,ALTGRUP) VALUES(@ADI,@KODU,@GRUP,@ALTGRUP)",list3);

            if (sonuc.Substring(0, 1).ToString() == "H")
            {
                formislemleri.Mesajform("sonuc:" + sonuc,"K",200);
                //return;
            }
            
            frmilceekle_Load(sender, e);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("delete from IL_ILCE_MAH where sirano=" +
            dr["sirano"].ToString());
            gridyukle();
        }
    }
}
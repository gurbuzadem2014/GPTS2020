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
    public partial class frmVardiyaSablon : DevExpress.XtraEditors.XtraForm
    {
        public frmVardiyaSablon()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@SablonAdi", tESablonAdi.Text));
            list.Add(new SqlParameter("@Aciklama", mEAciklama.Text));

            if (DB.pkVardiyaSablon == 0)
            {
                DB.ExecuteSQL("INSERT INTO VardiyaSablon (SablonAdi,Aciklama) values(@SablonAdi,@Aciklama)", list);
                //şablon detay oluştur.
                DataTable dt = DB.GetData("select * from VardiyaDetay where fkVardiya="+DB.pkVardiyalar.ToString());
             if (dt.Rows.Count == 0) return;
                string sql="";
                string maxvs = DB.GetData("select  MAX(pkVardiyaSablon) FROM VardiyaSablon").Rows[0][0].ToString();
             for (int i = 0; i < dt.Rows.Count-1; i++)
             {
                 sql += " INSERT INTO VardiyaSablonDetay (fkVardiyaSablon,Gunduz,Gece,izin) values(@fkVardiyaSablon,'@Gunduz','@Gece','@izin')";
                 sql = sql.Replace("@fkVardiyaSablon", maxvs.ToString());
                 sql = sql.Replace("@Gunduz", dt.Rows[0]["Gunduz"].ToString());
                 sql = sql.Replace("@Gece", dt.Rows[0]["Gece"].ToString());
                 sql = sql.Replace("@izin", dt.Rows[0]["izin"].ToString());
             }
             DB.ExecuteSQL(sql);
            }
            else
            {
                list.Add(new SqlParameter("@pkVardiyaSablon", DB.pkVardiyaSablon));

                DB.ExecuteSQL("UPDATE VardiyaSablon SET SablonAdi=@SablonAdi,Aciklama=@Aciklama where pkVardiyaSablon=@pkVardiyaSablon",list);
            }
            Close();
        }

        private void frmVardiyaSablon_Load(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from VardiyaSablon where pkVardiyaSablon=" + DB.pkVardiyaSablon.ToString());
            if (dt.Rows.Count > 0)
            {
                tESablonAdi.Text = dt.Rows[0]["SablonAdi"].ToString();
                mEAciklama.Text = dt.Rows[0]["Aciklama"].ToString();
                simpleButton1.Text = "Güncelle";
            }
        }
    }
}
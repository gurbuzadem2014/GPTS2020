using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;

namespace GPTS
{
    public partial class frmucretliizin : DevExpress.XtraEditors.XtraForm
    {
        string pkpersoneller = ConfigurationSettings.AppSettings["selectperid"].ToString();
        public frmucretliizin()
        {
            InitializeComponent();
        }

        private void frmucretliizin_Load(object sender, EventArgs e)
        {
            DataTable dtp = DB.GetData(@"select * from Personeller where pkpersoneller=" + pkpersoneller);
            pkmusteri.Text = dtp.Rows[0]["pkpersoneller"].ToString();
            adi.Text = dtp.Rows[0]["adi"].ToString();
            soyadi.Text = dtp.Rows[0]["soyadi"].ToString();
            try
            {
                DataTable dt = DB.GetData(@"select * from Personel_Ucretliizin where fkpersoneller=" + pkpersoneller + " order by tarih desc");
                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = "";
            sql = @"INSERT INTO Personel_Ucretliizin (fkpersoneller,bastarih,bittarih,aciklama,tutar)
            values (@fkpersoneller,'@bastarih','@bittarih','@aciklama',@tutar)";
            sql = sql.Replace("@fkpersoneller", pkpersoneller);
            sql = sql.Replace("@bastarih", debastarih.EditValue.ToString());
            sql = sql.Replace("@bittarih", deBitistarihi.EditValue.ToString());
            sql = sql.Replace("@aciklama", teaciklama.Text);
            sql = sql.Replace("@tutar", cEtutar.Value.ToString().Replace(",","."));
            DB.ExecuteSQL(sql);
            //Kasa Ekle
            sql = @"INSERT INTO KasaHareket (fkkasalar,fkPersoneller,Tarih,Modul,Tipi,Tutar,aciklama)
             values(1,@fkPersoneller,Getdate(),5,6,@tutar,'@aciklama')";
            sql = sql.Replace("@fkPersoneller", pkmusteri.Text);
            //sql = sql.Replace("@tarih", dateEdit1.EditValue.ToString());
            sql = sql.Replace("@tutar", cEtutar.Value.ToString());
            sql = sql.Replace("@aciklama", teaciklama.Text);
            DB.ExecuteSQL(sql);
        }
    }
}
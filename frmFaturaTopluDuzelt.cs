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
    public partial class frmFaturaTopluDuzelt : DevExpress.XtraEditors.XtraForm
    {
        public frmFaturaTopluDuzelt()
        {
            InitializeComponent();
        }

        private void frmKasaHareketDuzelt_Load(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData(@"select * from FaturaToplu ft with(nolock) where pkFaturaToplu=" + pkFaturaToplu.Text);

            pkFirma.Text = dt.Rows[0]["fkFirma"].ToString();
            txtFaturaNo.Text = dt.Rows[0]["FaturaNo"].ToString();
            string FaturaTarihi = dt.Rows[0]["FaturaTarihi"].ToString();
            txtFaturaAdresi.Text = dt.Rows[0]["FaturaAdresi"].ToString();
            txtVergiDairesi.Text = dt.Rows[0]["VergiDairesi"].ToString();
            txtVerigiNo.Text = dt.Rows[0]["VergiNo"].ToString();

            deFaturaTarihi.DateTime = Convert.ToDateTime(FaturaTarihi);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (deFaturaTarihi.EditValue == null)
                return;

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@FaturaNo", txtFaturaNo.Text));
            list.Add(new SqlParameter("@FaturaTarihi", deFaturaTarihi.DateTime));
            list.Add(new SqlParameter("@FaturaAdresi", txtFaturaAdresi.Text));
            list.Add(new SqlParameter("@VergiDairesi", txtVergiDairesi.Text));
            list.Add(new SqlParameter("@VergiNo", txtVerigiNo.Text));
            list.Add(new SqlParameter("@pkFaturaToplu", pkFaturaToplu.Text));
         
            string sonuc=
            DB.ExecuteSQL(@"UPDATE FaturaToplu SET FaturaNo=@FaturaNo,FaturaTarihi=@FaturaTarihi,FaturaAdresi=@FaturaAdresi,
            VergiDairesi=@VergiDairesi,VergiNo=@VergiNo
            where pkFaturaToplu=@pkFaturaToplu", list);
            if (sonuc == "0")
                Close();
            else
                MessageBox.Show("Hata Oluştu Bilgileri Kontrol Ediniz" +  sonuc);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmKasaHareketDuzelt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void pkFirma_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData(@"select * from Firmalar  with(nolock) where pkFirma=" + pkFirma.Text);

            CariAdi.Text = dt.Rows[0]["Firmaadi"].ToString();
            
        }
    }
}
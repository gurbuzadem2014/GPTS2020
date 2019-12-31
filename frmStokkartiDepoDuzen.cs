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
    public partial class frmStokkartiDepoDuzen : DevExpress.XtraEditors.XtraForm
    {
        string _pkStokKartiDepo = "0",_fkDepolar="0", _fkStokKarti="0";
        public frmStokkartiDepoDuzen(string pkStokKartiDepo,string fkDepolar,string fkStokKarti)
        {
            InitializeComponent();
            _pkStokKartiDepo = pkStokKartiDepo;
        }

        private void frmKasaTanimlari_Load(object sender, EventArgs e)
        {
            DataTable dt=
            DB.GetData("select isnull(KritikAdet,1) as KritikAdet from StokKartiDepo with(nolock) where pkStokKartiDepo=" + _pkStokKartiDepo);
            if (dt.Rows.Count == 0)
                _pkStokKartiDepo = "0";
            else
            {
                decimal kritik = 1;
                decimal.TryParse(dt.Rows[0]["KritikAdet"].ToString(), out kritik);
                sKritikAdet.Value = kritik;
            }

        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@KritikAdet", sKritikAdet.Value));
            list.Add(new SqlParameter("@fkDepolar", _fkDepolar));
            list.Add(new SqlParameter("@fkStokKarti", _fkStokKarti));

            string sql = "INSERT INTO Kasalar (fkDepolar,fkStokKarti,KritikAdet) values(@fkDepolar,@fkStokKarti,@KritikAdet)";

            if (_pkStokKartiDepo != "0")
                sql = "UPDATE StokKartiDepo SET KritikAdet=@KritikAdet WHERE pkStokKartiDepo=" + _pkStokKartiDepo;

                DB.ExecuteSQL(sql,list);

            Close();
        }

        private void sKritikAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.PerformClick();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {          
            BtnKaydet.Text="Kaydet[F9]";
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
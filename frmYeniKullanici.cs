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
    public partial class frmYeniKullanici : DevExpress.XtraEditors.XtraForm
    {
        public frmYeniKullanici()
        {
            InitializeComponent();
        }
        private void frmAracTakip_Load(object sender, EventArgs e)
        {
            DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
            if (dtSirket.Rows.Count == 0)
            {
                return;
            }

            txtSirket.Text= dtSirket.Rows[0]["Sirket"].ToString();
            txtEposta.Text = dtSirket.Rows[0]["eposta"].ToString();
            txtTel.Text = dtSirket.Rows[0]["TelefonNo"].ToString();
            txtYetkili.Text = dtSirket.Rows[0]["Yetkili"].ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if(txtEposta.Text.Length<6)
            {
                formislemleri.Mesajform("E-Posta Adresini Kontrol Ediniz", "K", 150);
                txtEposta.Focus();
                return;
            }
            if (txtEposta.Text.IndexOf('@')==-1)
            {
                formislemleri.Mesajform("E-Posta Adresini Kontrol Ediniz", "K", 150);
                txtEposta.Focus();
                return;
            }

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@eposta", txtEposta.Text));
            list.Add(new SqlParameter("@Sirket", txtSirket.Text));
            list.Add(new SqlParameter("@Yetkili", txtYetkili.Text));
            list.Add(new SqlParameter("@TelefonNo", txtTel.Text));
            DB.ExecuteSQL(@"UPDATE Sirketler SET eposta=@eposta,Sirket=@Sirket,Yetkili=@Yetkili,TelefonNo=@TelefonNo", list);

            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
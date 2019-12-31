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

namespace GPTS
{
    public partial class frmbankalar : DevExpress.XtraEditors.XtraForm
    {
        int _banka_id=0;
        public frmbankalar(int banka_id)
        {
            InitializeComponent();
            _banka_id = banka_id;
        }

        private void frmbankalar_Load(object sender, EventArgs e)
        {
            Subeler();

            BankaBilgileri();

            BankaAdi.Focus();
        }

        void Subeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData("select * from Subeler with(nolock)");
            lueSubeler.EditValue = 1;
        }
        void BankaBilgileri()
        {
            DataTable dt = DB.GetData("select * from Bankalar where PkBankalar=" + _banka_id);

            if (dt.Rows.Count == 0) return;

            //pkBankalar.Text = dt.Rows[0]["pkBankalar"].ToString();
            BankaAdi.Text = dt.Rows[0]["BankaAdi"].ToString();
            txtSube.Text = dt.Rows[0]["Sube"].ToString();
            cbKartTuru.EditValue = dt.Rows[0]["KartTuru"].ToString();
            

            if (dt.Rows[0]["Aktif"].ToString() == "True")
                Aktif.Checked = true;
            else
                Aktif.Checked = false;

            if(dt.Rows[0]["fkSube"].ToString()!="")
                lueSubeler.EditValue = int.Parse(dt.Rows[0]["fkSube"].ToString());
            
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            string sql = "";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@BankaAdi", BankaAdi.Text));
            list.Add(new SqlParameter("@Sube", txtSube.Text));
            list.Add(new SqlParameter("@Aktif", Aktif.Checked));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));
            list.Add(new SqlParameter("@KartTuru", cbKartTuru.Text));

            string mesaj = "";
            if (_banka_id == 0)
            {
                sql = "INSERT INTO Bankalar (BankaAdi,Sube,Aktif,fkSube,KartTuru) values(@BankaAdi,@Sube,@Aktif,@fkSube,@KartTuru)";
                mesaj = "Banka Eklendi";
            }
            else
            {
                list.Add(new SqlParameter("@PkBankalar",_banka_id));
                sql = "UPDATE Bankalar SET BankaAdi=@BankaAdi,Sube=@Sube,Aktif=@Aktif,fkSube=@fkSube,KartTuru=@KartTuru where PkBankalar=@PkBankalar";
                mesaj = "Banka Güncellendi";
            }

            string sonuc = DB.ExecuteSQL(sql, list);

            if (sonuc.Substring(0,1)=="H")
                GPTS.islemler.formislemleri.Mesajform(sonuc, "K", 200);
            else
                GPTS.islemler.formislemleri.Mesajform(mesaj, "S", 200);
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            lueSubeler.EditValue = Degerler.fkSube;
            BankaAdi.Text = "";
            Aktif.Checked = true;

            BankaAdi.Focus();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
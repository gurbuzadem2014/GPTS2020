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
    public partial class frmKasaTanimlari : DevExpress.XtraEditors.XtraForm
    {
        string _fkKasalar = "1";
        public frmKasaTanimlari(string fkKasalar)
        {
            InitializeComponent();
            _fkKasalar = fkKasalar;
        }

        private void frmKasaTanimlari_Load(object sender, EventArgs e)
        {
            Subeler();
            gridyukle();
        }

        void gridyukle()
        {
            string sql = "select * from Kasalar with(nolock) where  pkKasalar=" + _fkKasalar;
            DataTable dt = DB.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                pkKasalar.Text = dt.Rows[0]["pkKasalar"].ToString();
                teKasaAdi.Text=dt.Rows[0]["KasaAdi"].ToString();
                teHesapKodu.Text = dt.Rows[0]["HesapKodu"].ToString();
                if (dt.Rows[0]["Aktif"].ToString() == "True")
                    cbAktif.Checked = true;
                else
                    cbAktif.Checked = false;
                BtnKaydet.Text = "Güncelle";
            }
            else
            {
                teKasaAdi.Text = "";
                teHesapKodu.Text = "";
                BtnKaydet.Text = "Kaydet";
                cbAktif.Checked = true;
            }
        }

        void Subeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData("select * from Subeler with(nolock)");
            lueSubeler.EditValue = 1;
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@KasaAdi", teKasaAdi.Text));
            list.Add(new SqlParameter("@HesapKodu", teHesapKodu.Text));
            list.Add(new SqlParameter("@Aktif", cbAktif.Checked));
            list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue.ToString()));

            string sql = "INSERT INTO Kasalar (KasaAdi,HesapKodu,Aktif,fkSube) values(@KasaAdi,@HesapKodu,@Aktif,@fkSube)";

            if (pkKasalar.Text!="0")
               sql = "UPDATE Kasalar SET HesapKodu=@HesapKodu,KasaAdi=@KasaAdi,Aktif=@Aktif,fkSube=@fkSube WHERE pkKasalar=" + pkKasalar.Text;

                DB.ExecuteSQL(sql,list);

            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            pkKasalar.Text = "0";
            teKasaAdi.Text = "";
            teHesapKodu.Text = "";
            cbAktif.Checked = true;
            BtnKaydet.Text="Kaydet[F9]";
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
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
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmStokFiyatGrupKarti : DevExpress.XtraEditors.XtraForm
    {
        string pkSatisFiyatlariBaslik="0";
        public frmStokFiyatGrupKarti(string fkSatisFiyatlariBaslik)
        {
            InitializeComponent();
            pkSatisFiyatlariBaslik = fkSatisFiyatlariBaslik;
        }
        void GridGetir()
        {
            gridControl1.DataSource = DB.GetData("select * from SatisFiyatlariBaslik with(nolock) Order by Tur");
        }
        private void frmStokFiyatGrupKarti_Load(object sender, EventArgs e)
        {
            if (pkSatisFiyatlariBaslik == "0")
            {
                Baslik.Text = "";

                seMaxid.Value =
                DB.GetData("SELECT * FROM SatisFiyatlariBaslik with(nolock)").Rows.Count + 1;
                Baslik.Focus();
            }
            else
            {
                DataTable dt=
                DB.GetData("select * from SatisFiyatlariBaslik wtih(nolock) where pkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik);
                Baslik.Text = dt.Rows[0]["Baslik"].ToString();
                
                decimal tur = 0;
                decimal.TryParse(dt.Rows[0]["Tur"].ToString(), out tur);
                seMaxid.Value = tur;

                Baslik.Focus();
                Baslik.SelectAll();
            }
            GridGetir();

            if (pkSatisFiyatlariBaslik == "0")
            {
                panel1.Focus();
                Baslik.Focus();
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (Baslik.Text == "")
            {
                Baslik.Focus();
                return;
            }
            if (pkSatisFiyatlariBaslik == "0" && cbAktif.Checked)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("1.Nakit Fiyatları, Yeni Fiyat Grubu Fiyatları Olarak Açılacaktır. Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;               
            }
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Baslik", Baslik.Text));
            list.Add(new SqlParameter("@Tur", seMaxid.Value));
            list.Add(new SqlParameter("@Aktif", cbAktif.Checked));

            if (pkSatisFiyatlariBaslik == "0")
            {
                string yeniid = "0";
                yeniid = DB.ExecuteScalarSQL("INSERT INTO SatisFiyatlariBaslik (Baslik,Aktif,Tur) VALUES(@Baslik,1,@Tur) select IDENT_CURRENT('SatisFiyatlariBaslik')", list);
                if (yeniid != "0")
                {
                    string Aktif = "0";
                    if (cbAktif.Checked) Aktif = "1";

                    DB.ExecuteSQL("INSERT INTO SatisFiyatlari" +
                    " SELECT fkStokKarti," + yeniid + ",SatisFiyatiKdvli,SatisFiyatiKdvsiz,0,"+Aktif+",null,null FROM SatisFiyatlari" +
                    " where fkSatisFiyatlariBaslik=1");

                    formislemleri.Mesajform("Yeni Fiyat Grubu Oluşturuldu", "S",200);
                }
            }
            else
            {
                list.Add(new SqlParameter("@pkSatisFiyatlariBaslik", pkSatisFiyatlariBaslik));
                DB.ExecuteSQL("UPDATE SatisFiyatlariBaslik SET Baslik=@Baslik,Tur=@Tur,Aktif=@Aktif WHERE pkSatisFiyatlariBaslik=@pkSatisFiyatlariBaslik", list);

                formislemleri.Mesajform("Bilgiler GÜncellendi", "S", 200);
            }
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmStokFiyatGrupKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.F9)
                BtnKaydet_Click(sender, e);
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            
            DataTable dt =
                DB.GetData("select * from SatisFiyatlariBaslik wtih(nolock) where pkSatisFiyatlariBaslik=" + dr["pkSatisFiyatlariBaslik"].ToString());

            Baslik.Text = dt.Rows[0]["Baslik"].ToString();

            decimal tur = 0;
            decimal.TryParse(dt.Rows[0]["Tur"].ToString(), out tur);
            seMaxid.Value = tur;

            if (dt.Rows[0]["Aktif"].ToString() == "True")
                cbAktif.Checked = true;
            else
                cbAktif.Checked = false;
        }

        private void cbAktif_MouseClick(object sender, MouseEventArgs e)
        {
            if (pkSatisFiyatlariBaslik == "0" && cbAktif.Checked == true)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Fiyat Grubunu Pasif Yaparsanı, Tüm Ürünler Pasif Olacaktır. Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;
            }
        }

        private void btnPasifYap_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(pkSatisFiyatlariBaslik +".Fiyat Grubunu Pasif Yapılacaktır. Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("update SatisFiyatlariBaslik set Aktif = 0 where pkSatisFiyatlariBaslik = " + pkSatisFiyatlariBaslik);
            DB.ExecuteSQL("update SatisFiyatlari set Aktif = 0 where fkSatisFiyatlariBaslik ="+ pkSatisFiyatlariBaslik);
            GridGetir();
        }

        private void btnAktifYap_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(pkSatisFiyatlariBaslik + ".Fiyat Grubunu Aktif Yapılacaktır. Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("update SatisFiyatlariBaslik set Aktif=1 where pkSatisFiyatlariBaslik = " + pkSatisFiyatlariBaslik);
            DB.ExecuteSQL("update SatisFiyatlari set Aktif=1 where fkSatisFiyatlariBaslik =" + pkSatisFiyatlariBaslik);
            GridGetir();
        }
    }
}
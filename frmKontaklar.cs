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
using GPTS.islemler;

namespace GPTS
{
    public partial class frmKontaklar : DevExpress.XtraEditors.XtraForm
    {
        public string firma_id="0";
        public frmKontaklar(string fkfirma)
        {
            InitializeComponent();
            firma_id = fkfirma;
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        void Liste()
        {
            gridControl1.DataSource = DB.GetData("select * from Kontaklar with(nolock) where fkFirma=" + firma_id);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@kontak_adi", txtAdi.Text));
            list.Add(new SqlParameter("@tel", txtTel.Text));
            list.Add(new SqlParameter("@cep", txtCep.Text));
            list.Add(new SqlParameter("@eposta", txtEPosta.Text));
            list.Add(new SqlParameter("@fkFirma", firma_id));
            list.Add(new SqlParameter("@fkTedarikciler", "0"));
            list.Add(new SqlParameter("@gorev_id", "0"));
            list.Add(new SqlParameter("@aciklama", txtAciklama.Text));

            if (txt_id.Text == "0")
                DB.ExecuteSQL("INSERT INTO Kontaklar (kontak_adi,tel,cep,eposta,fkFirma,fkTedarikciler,aciklama,gorev_id)" +
                " values(@kontak_adi,@tel,@cep,@eposta,@fkFirma,@fkTedarikciler,@aciklama,@gorev_id)", list);
            else
            {
                DB.ExecuteSQL("UPDATE Kontaklar SET kontak_adi=@kontak_adi,tel=@tel,cep=@cep,eposta=@eposta,"+
                    "aciklama=@aciklama WHERE kontaklar_id=" + txt_id.Text, list);
            }

            yeni();

            Liste();
        }

        private void frmKontaklar_Load(object sender, EventArgs e)
        {
            Liste();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kontak Bilgisini Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string kontaklar_id = dr["kontaklar_id"].ToString();


                DB.ExecuteSQL("Delete From Kontaklar where kontaklar_id=" + kontaklar_id);
            }
            formislemleri.Mesajform("Kontak Silindi.", "S", 200);

            Liste();
        }
        void yeni()
        {
            txtAdi.Text = "";
            txtTel.Text = "";
            txtCep.Text = "";
            txtAciklama.Text = "";
            txtEPosta.Text = "";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            yeni();
        }

        private void düzeltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int si = gridView1.FocusedRowHandle;
            DataRow dr =gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string id = dr["kontaklar_id"].ToString();

            txt_id.Text = id;
            KontaklarBilgileriGetir(id);
            Liste();

            gridView1.FocusedRowHandle = si;
        }

        void KontaklarBilgileriGetir(string kontak_id)
        {
            DataTable dtKontak=
            DB.GetData("select * from Kontaklar with(nolock) where kontaklar_id=" + kontak_id);
            if(dtKontak.Rows.Count==0)
            {
                formislemleri.Mesajform("Kontak Bulunamadı","K",100);
                return;
            }

            txtAdi.Text = dtKontak.Rows[0]["kontak_adi"].ToString();
            txtTel.Text = dtKontak.Rows[0]["tel"].ToString();
            txtCep.Text = dtKontak.Rows[0]["cep"].ToString();
            txtEPosta.Text = dtKontak.Rows[0]["eposta"].ToString();
            txtAciklama.Text = dtKontak.Rows[0]["aciklama"].ToString();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int si = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string id = dr["kontaklar_id"].ToString();

            txt_id.Text = id;
            KontaklarBilgileriGetir(id);
            Liste();

            gridView1.FocusedRowHandle = si;
        }

        private void btnKasaHareket_Click(object sender, EventArgs e)
        {

        }
    }
}
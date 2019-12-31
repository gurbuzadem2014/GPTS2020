using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmMasalar : DevExpress.XtraEditors.XtraForm
    {
        public frmMasalar()
        {
            InitializeComponent();
        }
        void Masalar()
        {
            gridControl1.DataSource = DB.GetData("select * from Masalar with(nolock)");
        }

        private void frmDepoKarti_Load(object sender, EventArgs e)
        {
            MasaGruplariGetir();
            Masalar();
        }

        void MasaGruplariGetir()
        {
            lueMasaGruplari.Properties.DataSource = DB.GetData("select * from MasaGruplari with(nolock)");
        }
        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if(lueMasaGruplari.EditValue==null)
            {
                lueMasaGruplari.Focus();
                return;
            }

            //string pkDepolar = "0";
            string sonuc = "0", ak = "1";

            if (MasaAdi.Tag.ToString() == "0")
            {
                if (!cbAktif.Checked)
                    ak = "0";
                sonuc = DB.ExecuteScalarSQL("INSERT INTO Masalar (masa_adi,fkSatislar,aktif,fkMasaGruplari,masa_aciklama,gen,yuk,sira_no)" +
                    " values('" + MasaAdi.Text + "',0,"+ak+ ","+lueMasaGruplari.EditValue.ToString()+",'"+ txtAciklama.Text + "',"+ seGen.Value.ToString().Replace(",",".") +","+
                    seYuk.Value.ToString().Replace(",", ".")+","+ seSiraNo.Value.ToString().Replace(",",".")+ ")" +
                    " select IDENT_CURRENT('Masalar')");
                MasaAdi.Tag = sonuc;
            }
            else
            {
                if (!cbAktif.Checked)
                    ak = "0";
                DB.ExecuteSQL("UPDATE Masalar SET aktif=" + ak + ",masa_adi='" + MasaAdi.Text + 
                    "',fkMasaGruplari="+lueMasaGruplari.EditValue.ToString()+
                    ",masa_aciklama='" + txtAciklama.Text + "'" +
                    ",gen=" + seGen.Value.ToString().Replace(",", ".") +
                    ",yuk=" + seYuk.Value.ToString().Replace(",", ".") +
                    ",sira_no="+seSiraNo.Value.ToString().Replace(", ",".") +
                    " WHERE pkMasalar=" + MasaAdi.Tag.ToString());
            }
            //pkDepolar= DB.GetData("select MAX(pkDepolar) from Depolar").Rows[0][0].ToString();
            //string sql = "INSERT INTO DepoHareketleri" +
            //    " SELECT " + pkDepolar + ", pkStokKarti,0,0,0 FROM StokKarti";
            // DB.ExecuteSQL(sql);

            temizle();
            Masalar();
        }

        void MasaSil()
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Masayı Silmek istediğinize Edilsin mi?", "hitit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (secim == DialogResult.No) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkMasalar = dr["pkMasalar"].ToString();

            //if (DB.GetData("select * from DepoHareketleri where Mevcut>0 and fkDepolar=" + fkDepolar).Rows.Count > 0)
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Önce Depo Transfer Kullanarak Silmek istediğiniz Depodaki Stokları Aktarınız!", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //select * from StokKartiDepo where fkDepolar=1 and fkStokKarti=1
            //DB.ExecuteSQL("DELETE FROM StokKartiDepo WHERE fkDepolar=" + fkDepolar);
            //DB.ExecuteSQL("DELETE FROM DepoHareketleri WHERE fkDepolar=" + fkDepolar);
            DB.ExecuteSQL("DELETE FROM Masalar WHERE pkMasalar=" + fkMasalar);

            //formislemleri.Mesajform("Masa Silindi.", "S", 200);
            //DevExpress.XtraEditors.XtraMessageBox.Show("Depo Silindi.", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //string pkMasalar = dr["pkMasalar"].ToString();

            //if (DB.GetData("select * from Masalar with(nolock) where pkMasalar = " + pkMasalar).Rows.Count>0)
            //{
            //    formislemleri.Mesajform("Hareket Gördüğü için silemezsiniz", "K", 150);
            //    return;
            //}
            MasaSil();
            Masalar();
        }
        void temizle()
        {
            MasaAdi.Text = "";
            MasaAdi.Tag = "0";
            MasaAdi.Focus();

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            MasaAdi.Text = dr["masa_adi"].ToString();
            MasaAdi.Tag = dr["pkMasalar"].ToString();

            if (dr["aktif"].ToString() == "True")
                cbAktif.Checked = true;
            else
                cbAktif.Checked = false;

            if (dr["fkMasaGruplari"].ToString() != "")
                lueMasaGruplari.EditValue = int.Parse(dr["fkMasaGruplari"].ToString());

            if (dr["sira_no"].ToString() != "")
                seSiraNo.EditValue = int.Parse(dr["sira_no"].ToString());
            
        }

        private void simpleButton28_Click(object sender, EventArgs e)
        {
            frmMasaGruplari MasaGruplari = new frmMasaGruplari();
            MasaGruplari.ShowDialog();

            MasaGruplariGetir();
        }

        private void genişlikDeğişToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputForm sifregir1 = new inputForm();
            sifregir1.Text = "Genişlik";
            sifregir1.GirilenCaption.Text = "Genişlik Giriniz";
            sifregir1.Girilen.Text = "200";
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir1.ShowDialog();

            DB.ExecuteSQL("update Masalar set gen='" + sifregir1.Girilen.Text + "'");

            Masalar();
        }

        private void yükseklikDeğişToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputForm sifregir1 = new inputForm();
            sifregir1.Text = "Yükseklik";
            sifregir1.GirilenCaption.Text = "Yükseklik Giriniz";
            sifregir1.Girilen.Text = "200";
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir1.ShowDialog();

            DB.ExecuteSQL("update Masalar set yuk='" + sifregir1.Girilen.Text + "'");

            Masalar();
        }
    }
}

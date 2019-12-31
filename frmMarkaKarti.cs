using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmMarkaKarti : DevExpress.XtraEditors.XtraForm
    {
        public frmMarkaKarti()
        {
            InitializeComponent();
        }
        void gridyukle()
        {
            if(xtraTabControl1.SelectedTabPageIndex==0)
               gridControl1.DataSource = DB.GetData("select * from Markalar");
            if (xtraTabControl1.SelectedTabPageIndex == 1)
                gridControl2.DataSource = DB.GetData("select * from RenkGrupKodu");
            if (xtraTabControl1.SelectedTabPageIndex == 2)
                gridControl3.DataSource = DB.GetData("select * from BedenGrupKodu");
        }
        private void frmStokGrupKarti_Load(object sender, EventArgs e)
        {
            gridyukle();
            if (this.Tag == "M")
            {
                xtraTabControl1.SelectedTabPageIndex = 0;
                this.Text = "Marka Tanımları";
            }
            if (this.Tag == "R")
            {
                xtraTabControl1.SelectedTabPageIndex = 1;
                this.Text = "RenkTanımları";
            }
            if (this.Tag == "B")
            {
                xtraTabControl1.SelectedTabPageIndex = 2;
                this.Text = "Beden && Numara";
            }
        }
        void KullaniciKontrol(string KullaniciAdi)
        {
            try
            {
                SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=mssql02.turhost.com;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityazilim;Password=hitit9999");
                string sql = "select * from kullanicilar where KullaniciAdi='"+KullaniciAdi+"'";// where username='@username' and password='@password'";
                //sql = sql.Replace("@username", username);
                //sql = sql.Replace("@password", password);
                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                adp.SelectCommand.CommandTimeout = 60;

                DataTable dt = new DataTable();
                try
                {
                    adp.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        DB.webpkKullanicilar = 0;
                        XtraMessageBox.Show("Lisans için Lütfen 0262 644 51 12 arayınız.", "Hitit Yazılım", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    DB.webpkKullanicilar=int.Parse(dt.Rows[0]["pkKullanicilar"].ToString());
                    //Degerler.userid = dt.Rows[0]["sirano"].ToString();
                    //Degerler.username = dt.Rows[0]["username"].ToString()
                    //Degerler.password = dt.Rows[0]["password"].ToString();
                    //Degerler.smsadet = dt.Rows[0]["smsadet"].ToString();
                    //Degerler.smskullanilan = dt.Rows[0]["kullanilan"].ToString();
                    //Degerler.yetkili = dt.Rows[0]["yetkili"].ToString();
                    //Degerler.merkezadi = dt.Rows[0]["hastaneadi"].ToString();
                    //Degerler.ilce = dt.Rows[0]["ilce"].ToString();
                    //Degerler.mumessil = dt.Rows[0]["mumessil"].ToString();
                    //Degerler.sehir = dt.Rows[0]["sehir"].ToString();
                    //Degerler.vol = dt.Rows[0]["versiyon"].ToString();
                    //Degerler.aktif = dt.Rows[0]["aktif"].ToString();
                    //Degerler.smsuser = dt.Rows[0]["smsuser"].ToString();
                    //Degerler.smspass = dt.Rows[0]["smspass"].ToString();
                    //Degerler.smsbayi = dt.Rows[0]["smsbayikodu"].ToString();
                    //Degerler.smssender = dt.Rows[0]["smssender"].ToString();
                }
                catch (Exception exp)
                {
                    return;
                }
                finally
                {
                    con.Dispose();
                    adp.Dispose();
                }
                if (dt.Rows.Count == 0)
                {
                    //Degerler.kayitli = 0;
                    return;
                }
                //else
                    //Degerler.kayitli = dt.Rows.Count;
                //
            }
            catch (Exception exp)
            {
                XtraMessageBox.Show("Lisans için Lütfen Yazılım firmasını arayınız.", "Termo Takip", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
                // Application.Exit();
            }
            finally
            {
                //con.Dispose();
                //adp.Dispose();
            }
            return;
        }
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //DataRow dr = gridView1.GetDataRow(e.RowHandle);
        }
        void WebeEkleGuncelle(string Menu_Adi, string SortID, string fkStokGruplari)
        {
            KullaniciKontrol("gizembebe");
            if (DB.webpkKullanicilar == 0) return;
            SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=mssql02.turhost.com;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityazilim;Password=hitit9999");
            string sql = "IF NOT EXISTS (SELECT * FROM Menukategori WHERE fkKullanicilar=@fkKullanicilar and fkStokGruplari=@fkStokGruplari ) BEGIN " +
                                             "insert into Menukategori (Menu_Adi,SortID,fkKullanicilar,fkStokGruplari) " +
                                             "values('@Menu_Adi',@SortID,@fkKullanicilar,@fkStokGruplari) END";
            //TODO: sql += " else Be";
            sql = sql.Replace("@Menu_Adi", Menu_Adi);
            sql = sql.Replace("@SortID", SortID);
            sql = sql.Replace("@fkKullanicilar", DB.webpkKullanicilar.ToString());
            sql = sql.Replace("@fkStokGruplari", fkStokGruplari);
            SqlCommand cmd = new SqlCommand(sql,con);
            //SqlDataAdapter adp = new SqlDataAdapter(sql,con);
            //adp.SelectCommand.CommandTimeout = 60;

           // DataTable dt = new DataTable();
            try
            {
                //adp.Fill(dt);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                // throw e;
            }
            finally
            {
                con.Dispose();
                //adp.Dispose();
            }
        }

        void GrupSil(string MarkaRenkBeden)
        {
            if (MarkaRenkBeden == "M")
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string pkMarka = dr["pkMarka"].ToString();
                if (DB.GetData("select pkStokKarti from StokKarti where fkMarka=" + pkMarka).Rows.Count > 0)
                {
                    MessageBox.Show("Bu Gruba ait Marka Tanımlaması Yaptığınız için Silemezsiniz.\nLütfen Önce Ürünün Markasını değiştiriniz.");
                    return;
                }

                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Marka"].ToString() + " Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No)
                    return;

                DB.ExecuteSQL("DELETE FROM Markalar WHERE pkMarka=" + pkMarka);
                gridView1.DeleteSelectedRows();
            }
            if (MarkaRenkBeden == "R")
            {
                if (gridView2.FocusedRowHandle < 0) return;
                DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                string pkRenkGrupKodu = dr["pkRenkGrupKodu"].ToString();
                if (DB.GetData("select pkStokKarti from StokKarti where fkRenkGrupKodu=" + pkRenkGrupKodu).Rows.Count > 0)
                {
                    MessageBox.Show("Bu Gruba ait Renk Tanımlaması Yaptığınız için Silemezsiniz.\nLütfen Önce Ürünün Markasını değiştiriniz.");
                    return;
                }

                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Aciklama"].ToString() + " Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No)
                    return;

                DB.ExecuteSQL("DELETE FROM RenkGrupKodu WHERE pkRenkGrupKodu=" + pkRenkGrupKodu);
                gridView2.DeleteSelectedRows();
            }
            if (MarkaRenkBeden == "B")
            {
                if (gridView3.FocusedRowHandle < 0) return;
                DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
                string pkBedenGrupKodu = dr["pkBedenGrupKodu"].ToString();
                if (DB.GetData("select pkStokKarti from StokKarti where fkBedenGrupKodu=" + pkBedenGrupKodu).Rows.Count > 0)
                {
                    MessageBox.Show("Bu Gruba ait Beden Tanımlaması Yaptığınız için Silemezsiniz.\nLütfen Önce Ürünün Markasını değiştiriniz.");
                    return;
                }

                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Aciklama"].ToString() + " Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No)
                    return;

                DB.ExecuteSQL("DELETE FROM BedenGrupKodu WHERE pkBedenGrupKodu=" + pkBedenGrupKodu);
                gridView3.DeleteSelectedRows();
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            kAYDETToolStripMenuItem_Click(sender,e);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            YeniMarkaRenkBeden("M");
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            GrupSil("M");
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            gridColumn2.OptionsColumn.AllowEdit = true;
        }

        void kaydet()
        {
            if (gridColumn2.OptionsColumn.AllowEdit==false) return;
                
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                if (dr == null) continue;

                DB.ExecuteSQL("UPDATE Markalar SET Marka='" + dr["Marka"].ToString() + "' WHERE pkMarka=" + dr["pkMarka"].ToString());
            }
        }
        void YeniMarkaRenkBeden(string MarkaRenkBeden)
        {
            //kaydet();
            frmYeniMarkaYeniKarti MarkaYeniKarti = new frmYeniMarkaYeniKarti();
            MarkaYeniKarti.Tag = MarkaRenkBeden;
            MarkaYeniKarti.ShowDialog();

            gridyukle();
        }
        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YeniMarkaRenkBeden("M");
        }

        private void kAYDETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kaydet();
            Close();
        }

        private void frmStokGrupKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            gridyukle();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            YeniMarkaRenkBeden("R");
            gridyukle();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            YeniMarkaRenkBeden("B");
            gridyukle();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GrupSil("R");
            gridyukle();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
             GrupSil("B");
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);
                DB.ExecuteSQL("UPDATE BedenGrupKodu SET Aciklama='" + dr["Aciklama"].ToString() + "' WHERE pkBedenGrupKodu=" + dr["pkBedenGrupKodu"].ToString());
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                DB.ExecuteSQL("UPDATE RenkGrupKodu SET Aciklama='" + dr["Aciklama"].ToString() + "' WHERE pkRenkGrupKodu=" + dr["pkRenkGrupKodu"].ToString());
            }
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            gridColumn4.OptionsColumn.AllowEdit = true;
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            gridColumn7.OptionsColumn.AllowEdit = true;
        }
    }
}

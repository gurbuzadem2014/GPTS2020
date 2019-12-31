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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmSatisFiyatlari : DevExpress.XtraEditors.XtraForm
    {
        public frmSatisFiyatlari()
        {
            InitializeComponent();
        }
        private void frmSatisFiyatlari_Load(object sender, EventArgs e)
        {
            //alış fiyatlarından geliyorsa
            if (this.Tag.ToString() == "1")
            {
                BtnKaydet.Visible = false;
                btTamam.Visible = true;
            }
            //Stok Kartından geliyorsa
            if (this.Tag.ToString() == "2")
            {
                BtnKaydet.Visible = false;
                btTamam.Visible = true;
            }
            if (pkStokKarti.Text == "0")
            {
                gridColumn3.OptionsColumn.AllowEdit= false;// DevExpress.XtraGrid.Columns.GridColumn.ReadOnly;
                gridColumn4.OptionsColumn.AllowEdit= false;
                gridColumn5.OptionsColumn.AllowEdit = false;
            }

            OlmayanFiyatBasliklariniEkle();

            SatisFiyatlariGetir();

             DataTable dt = DB.GetData("SELECT * FROM StokKarti with(nolock) where pkStokKarti=" + pkStokKarti.Text);
             if (dt.Rows.Count > 0)
                 kdv.Value = decimal.Parse(dt.Rows[0]["KdvOrani"].ToString());
             else
                 kdv.Value = 0;
             baslik.Text = dt.Rows[0]["Stokadi"].ToString();
        }
        void OlmayanFiyatBasliklariniEkle()
        {
            DataTable dt = DB.GetData("Select * FROM SatisFiyatlariBaslik with(nolock)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pkSatisFiyatlariBaslik=dt.Rows[i]["pkSatisFiyatlariBaslik"].ToString();
                if (DB.GetData("Select * FROM SatisFiyatlari with(nolock) WHERE fkStokKarti=" + pkStokKarti.Text + " and fkSatisFiyatlariBaslik=" + pkSatisFiyatlariBaslik).Rows.Count == 0)
                {
                    DB.ExecuteSQL("INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,Aktif)" +
                        " VALUES(" + pkStokKarti.Text + "," + pkSatisFiyatlariBaslik + ",0)");

                    string sql = @"UPDATE SatisFiyatlari SET SatisFiyatiKdvli=(SELECT SatisFiyatiKdvli FROM  SatisFiyatlari with(nolock) WHERE fkSatisFiyatlariBaslik=1 and fkStokKarti=@fkStokKarti)
                    ,SatisFiyatiKdvsiz=(SELECT SatisFiyatiKdvsiz FROM  SatisFiyatlari with(nolock) WHERE fkSatisFiyatlariBaslik=1 and fkStokKarti=@fkStokKarti)
                                   where fkStokKarti=@fkStokKarti and SatisFiyatiKdvli is null";
                    
                    sql = sql.Replace("@fkStokKarti", pkStokKarti.Text);
                    int  sonuc = DB.ExecuteSQL(sql);

                    if (sonuc == -1)
                        formislemleri.Mesajform("Hata Oluştu :" + sonuc.ToString(), "K", 200);
                }

            }
        }
        void SatisFiyatlariGetir()
        {
            string sql = @"select pkSatisFiyatlariBaslik,pkSatisFiyatlari,Baslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,sf.Aktif,sfb.Tur 
                    from SatisFiyatlariBaslik sfb with(nolock)
                    left join SatisFiyatlari sf on sf.fkSatisFiyatlariBaslik=sfb.pkSatisFiyatlariBaslik
                    WHERE sf.fkStokKarti = " + pkStokKarti.Text + " order by sfb.Tur";
            gridControl1.DataSource = DB.GetData(sql);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                ArrayList list = new ArrayList();
                DB.ExecuteSQL("UPDATE SatisFiyatlari  SET YeniFiyatKdvli=null WHERE pkSatisFiyatlari=" + dr["pkSatisFiyatlari"].ToString(), list);
            }
            Close();
        }

        private void frmSatisFiyatlari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Cursor cur = Cursors.WaitCursor;
            this.Cursor = cur;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                ArrayList list = new ArrayList();
                //string SatisFiyatiKdvli=;
                list.Add(new SqlParameter("@SatisFiyatiKdvli", dr["SatisFiyatiKdvli"].ToString().Replace(",", ".")));
                //if (i == 0)
                //{
                //    pkStokKarti.AccessibleDescription = SatisFiyatiKdvli;
                //    DB.ExecuteSQL("UPDATE StokKarti SET SatisFiyati='" + SatisFiyatiKdvli + "' where pkStokKarti=" + pkStokKarti.Text);
                //}
                if(dr["SatisFiyatiKdvsiz"].ToString().Replace(",", ".")=="")
                   list.Add(new SqlParameter("@SatisFiyatiKdvsiz", "0"));
                else
                   list.Add(new SqlParameter("@SatisFiyatiKdvsiz", dr["SatisFiyatiKdvsiz"].ToString().Replace(",", ".")));
                list.Add(new SqlParameter("@iskontoYuzde", dr["iskontoYuzde"].ToString().Replace(",", ".")));
                if (dr["Aktif"].ToString() == "True" || dr["Tur"].ToString()=="1")
                    list.Add(new SqlParameter("@Aktif","1"));
                else
                    list.Add(new SqlParameter("@Aktif","0"));

                DB.ExecuteSQL("UPDATE SatisFiyatlari  SET SatisFiyatiKdvli=@SatisFiyatiKdvli,SatisFiyatiKdvsiz=@SatisFiyatiKdvsiz,iskontoYuzde=@iskontoYuzde,Aktif=@Aktif WHERE pkSatisFiyatlari=" + dr["pkSatisFiyatlari"].ToString(), list);

               // if (gridColumn2.OptionsColumn.AllowEdit == true)
                //    DB.ExecuteSQL("UPDATE SatisFiyatlariBaslik SET Baslik='" + dr["Baslik"].ToString() + "' where pkSatisFiyatlariBaslik=" + dr["fkSatisFiyatlariBaslik"].ToString());
                //if (dr["Aktif"].ToString() == "True")
                 //   DB.ExecuteSQL("UPDATE SatisFiyatlariBaslik SET Aktif=1 where pkSatisFiyatlariBaslik=" + dr["fkSatisFiyatlariBaslik"].ToString());
            }
            //FİYAT GRUP ADI KAYDET
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //string fkSatisFiyatlariBaslik = dr["fkSatisFiyatlariBaslik"].ToString();
            cur = Cursors.Default;
            this.Cursor = cur;

            this.TopMost = true;
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmStokFiyatGrupKarti StokFiyatGrupKarti = new frmStokFiyatGrupKarti("0");
            StokFiyatGrupKarti.ShowDialog();
            SatisFiyatlariGetir();
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkSatisFiyatlariBaslik = dr["pkSatisFiyatlariBaslik"].ToString();

            if (fkSatisFiyatlariBaslik == "1")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(dr["Baslik"].ToString() + " Fiyat Grubunu Silemezsini!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            #region Şifre Gir
            if (Degerler.OzelSifre != "")
            {
                inputForm sifregir = new inputForm();
                sifregir.Girilen.PasswordChar = '#';
                sifregir.ShowDialog();
                if (sifregir.Girilen.Text != Degerler.OzelSifre) return;
            }
            #endregion

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Baslik"].ToString() +" Fiyat Grubunu Silmek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("Delete from  SatisFiyatlari WHERE fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik);
            DB.ExecuteSQL("Delete from SatisFiyatlariBaslik WHERE pkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik);
            SatisFiyatlariGetir();
        }

        private void repositoryItemCalcEditKdvHaric_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).Text;
                if (girilen == "") return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                decimal GirilenSatisFiyatiKdvli = 0;
                decimal.TryParse(girilen, out GirilenSatisFiyatiKdvli);
                decimal kdvharic = 0;
                kdvharic = GirilenSatisFiyatiKdvli + (GirilenSatisFiyatiKdvli * kdv.Value) / 100;
                gridView1.SetFocusedRowCellValue("SatisFiyatiKdvli", kdvharic);
                gridView1.SetFocusedRowCellValue("SatisFiyatiKdvsiz", GirilenSatisFiyatiKdvli);
                //satış fiyatlarını yeni fiyat alanında tut
                DB.ExecuteSQL("UPDATE SatisFiyatlari SET YeniFiyatKdvli=" 
                + GirilenSatisFiyatiKdvli.ToString().Replace(",", ".") +
                ",YeniFiyatKdvsiz=" + kdvharic.ToString().Replace(",",".")
                + " where pkSatisFiyatlari=" + dr["pkSatisFiyatlari"].ToString());
            }
        }

        private void repositoryItemSpinEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string iskonto =
                ((DevExpress.XtraEditors.SpinEdit)(((DevExpress.XtraEditors.SpinEdit)(sender)).Properties.OwnerEdit)).Text;
                iskonto = iskonto.Replace(",",".");
                decimal isk = 0;
                decimal.TryParse(iskonto, out isk);
                DataRow dr = gridView1.GetDataRow(0);//nakit satış fiyatları
                decimal kdvharic = 0, KdvDahil=0;
                decimal.TryParse(dr["SatisFiyatiKdvsiz"].ToString(), out kdvharic);
                decimal.TryParse(dr["SatisFiyatiKdvli"].ToString(), out KdvDahil);
                //KdvDahil = KdvDahil - (KdvDahil * isk) / 100;
                kdvharic = KdvDahil - (KdvDahil * isk) / 100;
                gridView1.SetFocusedRowCellValue("SatisFiyatiKdvli", KdvDahil);
                gridView1.SetFocusedRowCellValue("SatisFiyatiKdvsiz", kdvharic);
                //gridView1.SetFocusedRowCellValue("iskontoYuzde", "0");
            }
        }

        void SatisFiyatiKdvliKaydet(string girilen)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            decimal GirilenSatisFiyatiKdvli = 0;
            decimal.TryParse(girilen, out GirilenSatisFiyatiKdvli);
            decimal kdvharic = 0;
            kdvharic = GirilenSatisFiyatiKdvli - (GirilenSatisFiyatiKdvli * kdv.Value) / 100;
           // bandedGridView1.SetFocusedRowCellValue("SatisFiyatiKdvli", GirilenSatisFiyatiKdvli);
           // bandedGridView1.SetFocusedRowCellValue("SatisFiyatiKdvsiz", kdvharic);
            //bandedGridView1.SetFocusedRowCellValue("iskontoYuzde", "0");
            //satış fiyatlarını yeni fiyat alanında tut
            DB.ExecuteSQL("UPDATE SatisFiyatlari SET SatisFiyatiKdvli=" + GirilenSatisFiyatiKdvli.ToString().Replace(",", ".") +
           ",SatisFiyatiKdvsiz=" + kdvharic.ToString().Replace(",", ".")
           + " where pkSatisFiyatlari=" + dr["pkSatisFiyatlari"].ToString());
        }

        private void repositoryItemCalcEditKdvDahil_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).Text;
                if (girilen == "") return;
                SatisFiyatiKdvliKaydet(girilen);
            }
        }

        private void btTamam_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                ArrayList list = new ArrayList();
                string SatisFiyatiKdvli = dr["SatisFiyatiKdvli"].ToString().Replace(",", ".");
                list.Add(new SqlParameter("@YeniFiyatKdvli", SatisFiyatiKdvli));
                DB.ExecuteSQL("UPDATE SatisFiyatlari  SET YeniFiyatKdvli=@YeniFiyatKdvli WHERE pkSatisFiyatlari=" + dr["pkSatisFiyatlari"].ToString(), list);
            }
            Close();
        }

        private void repositoryItemCalcEditKdvDahil_Leave(object sender, EventArgs e)
        {
            string girilen =
        ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).Text;
            if (girilen == "") return;
            SatisFiyatiKdvliKaydet(girilen);
        }

        private void gridView1_DoubleClick_1(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmStokFiyatGrupKarti StokFiyatGrupKarti = new frmStokFiyatGrupKarti(dr["pkSatisFiyatlariBaslik"].ToString());
            StokFiyatGrupKarti.ShowDialog();
            SatisFiyatlariGetir();
        }

        private void topluFiyatDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SatisFiyatiKdvli = formislemleri.inputbox("Yeni Fiyat ", "Yeni Fiyat Giriniz", "0", false);

            DB.ExecuteSQL("update SatisFiyatlari SET SatisFiyatiKdvli=" + SatisFiyatiKdvli.Replace(",", ".") +
                " where fkStokKarti=" + pkStokKarti.Text);

            SatisFiyatlariGetir();
        }

        private void tümünüAktifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("update SatisFiyatlari SET Aktif=1 where fkStokKarti=" + pkStokKarti.Text);

            SatisFiyatlariGetir();
        }

        private void tümünüPasifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("update SatisFiyatlari SET Aktif=0 where fkStokKarti=" + pkStokKarti.Text);
            DB.ExecuteSQL("update SatisFiyatlari SET Aktif=1 where fkStokKarti=" + pkStokKarti.Text+
                " and fkSatisFiyatlariBaslik=(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik with(nolock) where Tur=1)");
            SatisFiyatlariGetir();
        }
    }
}
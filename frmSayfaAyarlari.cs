using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GPTS
{
    public partial class frmSayfaAyarlari : DevExpress.XtraEditors.XtraForm
    {
        string _fkModul = "0";
        public frmSayfaAyarlari(string fkModul)
        {
            InitializeComponent();
            _fkModul = fkModul;
        }

        private void frmSayfaAyarlari_Load(object sender, EventArgs e)
        {
            string sql = "";
            //if (_fkModul==1)
                sql= "select * from Parametreler with(nolock) where fkModul="+ _fkModul +" order by SiraNo";
            //else if (_fkModul == 27)
              //  sql = "select * from Parametreler with(nolock) where fkModul=1 order by SiraNo";
            gridControl5.DataSource = DB.GetData(sql);
        }

        private void gridView6_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ListeGetir();
        }

        void ListeGetir()
        {
            if (gridView6.FocusedRowHandle < 0) return;

            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);
            if (dr["fkNesneTipi"].ToString() == "7")
            {
                gridColumn44.Visible = false;
                gridColumn48.Visible = true;
                gridColumn48.Caption = "Adedi";
            }
            else
            {
                gridColumn44.Visible = true;
                gridColumn48.Visible = false;
            }

            gcKullanicilar.DataSource = DB.GetData(@"SELECT pkYetkiAlanlari,Kullanicilar.pkKullanicilar, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi, Kullanicilar.Yetkilikodu, isnull(YetkiAlanlari.Yetki,0) as Sec,
            YetkiAlanlari.Sayi,YetkiAlanlari.BalonGoster,YetkiAlanlari.fkParametreler FROM Kullanicilar with(nolock) 
            INNER JOIN  YetkiAlanlari with(nolock)  ON Kullanicilar.pkKullanicilar = YetkiAlanlari.fkKullanicilar
            WHERE Kullanicilar.durumu=1 and YetkiAlanlari.fkParametreler = " + dr["pkParametreler"].ToString());

        }

        private void repositoryItemCheckEdit6_CheckedChanged(object sender, EventArgs e)
        {
            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
            string g =
                ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

            if (g == "True")
                DB.ExecuteSQL("UPDATE YetkiAlanlari SET Yetki=1 where pkYetkiAlanlari= " + dr["pkYetkiAlanlari"].ToString());
            else
                DB.ExecuteSQL("UPDATE YetkiAlanlari SET Yetki=0 where pkYetkiAlanlari= " + dr["pkYetkiAlanlari"].ToString());

        }

        private void repositoryItemSpinEdit6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
                string deger =
                ((DevExpress.XtraEditors.SpinEdit)((((DevExpress.XtraEditors.SpinEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue.ToString();
                DB.ExecuteSQL("UPDATE YetkiAlanlari SET Sayi=" + deger.Replace(",", ".") 
                    + " where fkParametreler=" + dr["fkParametreler"].ToString()+
                    " and fkKullanicilar=" + dr["pkKullanicilar"].ToString());
            }
        }

        private void kontrolEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView6.FocusedRowHandle < 0) return;

            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);
            string  pkParametreler = dr["pkParametreler"].ToString();
            DataTable dtPar = DB.GetData("select * from  Parametreler p with(nolock) where pkParametreler ="+ pkParametreler);

            if (dtPar.Rows.Count == 0) return;

            string Aciklama10 = dtPar.Rows[0]["Aciklama10"].ToString();

            DataTable dtKul = DB.GetData("select * from  Kullanicilar k with(nolock)");

            for (int i = 0; i < dtKul.Rows.Count; i++)
            {
                DB.ExecuteSQL("insert into YetkiAlanlari"+
            " (fkParametreler, fkKullanicilar, Yetki, Sayi, Aciklama10)"+
            " values("+ pkParametreler+","+ dtKul.Rows[i][0].ToString() + ", 1, 1, '"+ Aciklama10 + "') ");
            }

            ListeGetir();
        }
    }
}
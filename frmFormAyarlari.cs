using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmFormAyarlari : DevExpress.XtraEditors.XtraForm
    {
        public frmFormAyarlari()
        {
            InitializeComponent();
        }

        private void frmSayfaAyarlari_Load(object sender, EventArgs e)
        {
            gridControl5.DataSource = DB.GetData("select * from Ayarlar with(nolock)");
            RandevuAyarlari();
        }

        string RandevuBasSaat = "8", RandevuBitSaat = "21", RandevuSaatAraligi = "30";
        private  void RandevuAyarlari()//out string RandevuBasSaat, out string RandevuBitSaat, out string RandevuSaatAraligi)
        {
            DataTable dt = null;

            dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuBasSaat'");
            if (dt.Rows.Count > 0)
            {
                RandevuBasSaat = dt.Rows[0]["Ayar50"].ToString();
                sebas.Value = int.Parse(RandevuBasSaat);
            }

            dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuBitSaat'");
            if (dt.Rows.Count > 0)
            {
                RandevuBitSaat = dt.Rows[0]["Ayar50"].ToString();
                sebit.Value = int.Parse(RandevuBitSaat);
            }

            dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuSaatAraligi'");
            if (dt.Rows.Count > 0)
            {
                RandevuSaatAraligi = dt.Rows[0]["Ayar50"].ToString();
                sesaat.Value = int.Parse(RandevuSaatAraligi);
            }



        }

//        private void gridView6_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
//        {
//            if (gridView1.FocusedRowHandle < 0) return;

//            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
//            if (dr["fkNesneTipi"].ToString() == "7")
//            {
//                gridColumn44.Visible = false;
//                gridColumn48.Visible = true;
//                gridColumn48.Caption = "Adedi";
//            }
//            else
//            {
//                gridColumn44.Visible = true;
//                gridColumn48.Visible = false;
//            }

//            gcKullanicilar.DataSource = DB.GetData(@"SELECT pkYetkiAlanlari,Kullanicilar.pkKullanicilar, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi, Kullanicilar.Yetkilikodu, isnull(YetkiAlanlari.Yetki,0) as Sec,
//            YetkiAlanlari.Sayi,YetkiAlanlari.BalonGoster,YetkiAlanlari.fkParametreler FROM Kullanicilar with(nolock) 
//            INNER JOIN  YetkiAlanlari with(nolock)  ON Kullanicilar.pkKullanicilar = YetkiAlanlari.fkKullanicilar
//            WHERE Kullanicilar.durumu=1 and YetkiAlanlari.fkParametreler = " + dr["pkParametreler"].ToString());
        
//        }

        //private void repositoryItemCheckEdit6_CheckedChanged(object sender, EventArgs e)
        //{
        //    DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
        //    string g =
        //        ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

        //    if (g == "True")
        //        DB.ExecuteSQL("UPDATE YetkiAlanlari SET Yetki=1 where pkYetkiAlanlari= " + dr["pkYetkiAlanlari"].ToString());
        //    else
        //        DB.ExecuteSQL("UPDATE YetkiAlanlari SET Yetki=0 where pkYetkiAlanlari= " + dr["pkYetkiAlanlari"].ToString());

        //}

        //private void repositoryItemSpinEdit6_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
        //        string deger =
        //        ((DevExpress.XtraEditors.SpinEdit)((((DevExpress.XtraEditors.SpinEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue.ToString();
        //        DB.ExecuteSQL("UPDATE YetkiAlanlari SET Sayi=" + deger.Replace(",", ".") 
        //            + " where fkParametreler=" + dr["fkParametreler"].ToString()+
        //            " and fkKullanicilar=" + dr["pkKullanicilar"].ToString());
        //    }
        //}

        private void BtnKaydet_Click(object sender, EventArgs e)
        {

            DB.ExecuteSQL("UPDATE Ayarlar set  Ayar50='"+sebas.Value.ToString()+"' where Ayar20='RandevuBasSaat'");
            DB.ExecuteSQL("UPDATE Ayarlar set  Ayar50='" + sebit.Value.ToString() + "' where Ayar20='RandevuBitSaat'");
            DB.ExecuteSQL("UPDATE Ayarlar set  Ayar50='" + sesaat.Value.ToString() + "' where Ayar20='RandevuSaatAraligi'");
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{
            //    DB.ExecuteSQL("UPDATE ");
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sonuc = formislemleri.MesajBox("Varsayılan Ayarlar Yapılacak?", "Varsayılan", 1, 2);
            if (sonuc == "0") return;
            sebas.Value = 8;
            sebit.Value = 21;
            sesaat.Value = 30;
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
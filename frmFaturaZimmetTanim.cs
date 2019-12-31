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
    public partial class frmFaturaZimmetTanim : DevExpress.XtraEditors.XtraForm
    {
        int pkKullanici = 0;
        public frmFaturaZimmetTanim(int fkKullanici)
        {
            InitializeComponent();
            pkKullanici = fkKullanici;
        }

        private void frmUcGoster_Load(object sender, EventArgs e)
        {
            KullaniciListesi();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            Close();
        }
        void KullaniciListesi()
        {
            lueKullanicilar.Properties.DataSource = DB.GetData("select pkKullanicilar,adisoyadi,KullaniciAdi from Kullanicilar  with(nolock) where durumu=1 ");
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }
        void Getir()
        {
             string sql = @"SELECT FaturaSeriNoZimmet.pkFaturaSeriNoZimmet, FaturaSeriNoZimmet.fkKullanicilar, FaturaSeriNoZimmet.SeriNo, FaturaSeriNoZimmet.FaturaNo, 
                      FaturaSeriNoZimmet.Tarih, FaturaSeriNoZimmet.Durumu, FaturaSeriNoZimmet.fkSatislar, FaturaSeriNoZimmet.fkFaturaToplu, FaturaToplu.FaturaNo AS Expr1, 
                      FaturaToplu.FaturaTarihi, Firmalar.Firmaadi
FROM  Firmalar  with(nolock)
INNER JOIN Satislar with(nolock) ON Firmalar.pkFirma = Satislar.fkFirma 
RIGHT  JOIN FaturaSeriNoZimmet with(nolock) ON Satislar.pkSatislar = FaturaSeriNoZimmet.fkSatislar 
LEFT  JOIN FaturaToplu with(nolock) ON FaturaSeriNoZimmet.fkFaturaToplu = FaturaToplu.pkFaturaToplu
 where FaturaSeriNoZimmet.fkKullanicilar=" + lueKullanicilar.EditValue.ToString();
            gridControl1.DataSource = DB.GetData(sql);
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            decimal fark=seFaturaNo2.Value-seFaturaNo.Value;
            decimal say=seFaturaNo.Value;
            for (int i = 0; i <= fark; i++)
			{
                
			    DB.ExecuteSQL("insert into FaturaSeriNoZimmet (fkKullanicilar,SeriNo,FaturaNo,Tarih,Durumu)"+
                " values("+lueKullanicilar.EditValue.ToString()+",'"+teSeriNo.Text+"',"+say.ToString().Replace(",",".")+",getdate(),'Atanmadı')");
                say = say + 1;
			}
            

            
           Getir();
        }

        private void lueKullanicilar_EditValueChanged(object sender, EventArgs e)
        {
            Getir();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridView1.FocusedRowHandle<0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("delete from FaturaSeriNoZimmet where pkFaturaSeriNoZimmet=" + dr["pkFaturaSeriNoZimmet"].ToString());

            Getir();
        }
    }
}
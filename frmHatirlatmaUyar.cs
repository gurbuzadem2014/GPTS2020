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
    public partial class frmHatirlatmaUyar : DevExpress.XtraEditors.XtraForm
    {
        public frmHatirlatmaUyar()
        {
            InitializeComponent();
            Degerler.isHatirlatmaUyarEkran = false;//açıkken tekrar uyarma
        }
        void GunlukHatirlatmalar()
        {
            gridControl1.DataSource =  DB.GetData(@"SELECT  ha.*,f.firmaadi,f.tel,f.cep,
DATEDIFF(MINUTE,animsat_zamani,getdate()) as kalan_sure FROM  HatirlatmaAnimsat ha with(nolock) 
left join Firmalar f on f.pkFirma=ha.fkFirma
where animsat=1 and animsat_zamani<getdate()");

            Temizle();
        }

        private void frmAracTakip_Load(object sender, EventArgs e)
        {
            GunlukHatirlatmalar();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();

            DB.ExecuteSQL(@"UPDATE HatirlatmaAnimsat SET animsat=0 WHERE pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);

            GunlukHatirlatmalar();

            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkPersonel", "1"));
            //list.Add(new SqlParameter("@GidisZaman", deHatirtama_zamani.DateTime));
            //list.Add(new SqlParameter("@GidisKm", seDk.Value));
            //list.Add(new SqlParameter("@Aciklama", "Denem"));
            //DB.ExecuteSQL(@"INSERT INTO AracTakip (fkPersonel,GidisZaman,GidisKm,Aciklama)
            //    values(@fkPersonel,@GidisZaman,@GidisKm,@Aciklama)",list);
            //getir();
        }

        private void frmHatirlatmaUyar_FormClosed(object sender, FormClosedEventArgs e)
        {
            Degerler.isHatirlatmaUyarEkran = true;//açıkken tekrar uyarma
            Degerler.AnimsatmaSaniyeInterval = 1000;
        }

        private void açToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmHatirlatmaUzat Hatirlat = new frmHatirlatmaUzat(DateTime.Now, DateTime.Now, 0);
            string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();
            DB.pkHatirlatma = int.Parse(pkHatirlatmaAnimsat);
            Hatirlat.ShowDialog();

            GunlukHatirlatmalar();
        }

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHatirlatmaUzat Hatirlat = new frmHatirlatmaUzat(DateTime.Now, DateTime.Now, 0);
            DB.pkHatirlatma = 0;
            Hatirlat.ShowDialog();

            GunlukHatirlatmalar();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();

            DB.ExecuteSQL(@"UPDATE HatirlatmaAnimsat SET animsat=1,animsat_zamani=DATEADD(minute," +
                seDk.Value.ToString() + ",getdate())" +
                 " WHERE pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);

            GunlukHatirlatmalar();
        }

        void Temizle()
        {
            baslik.Text = "";
            lbKonu.Text = "";
        }

        private void btnhicbiri_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();

                DB.ExecuteSQL(@"UPDATE HatirlatmaAnimsat SET animsat=0 WHERE pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);

            }

            GunlukHatirlatmalar();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            baslik.Text = dr["firmaadi"].ToString();

            lbKonu.Text = dr["Aciklama"].ToString();

        }
    }
}
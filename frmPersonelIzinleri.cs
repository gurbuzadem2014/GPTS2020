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

namespace GPTS
{
    public partial class frmPersonelIzinleri : DevExpress.XtraEditors.XtraForm
    {
        string _persone_id = "0";
        public frmPersonelIzinleri(string persone_id)
        {
            InitializeComponent();
            _persone_id = persone_id;
        }

        private void frmPersonelEvrak_Load(object sender, EventArgs e)
        {
            ilktarihi.DateTime = DateTime.Today;
            bitistarihi.DateTime = DateTime.Today;

            kalanizinler();

            Personeller();
        }

        void kalanizinler()
        {
            gridControl1.DataSource = DB.GetData("select * from PersonelIzınHakedis with(nolock)");
        }

        void Personeller()
        {
            luePersonel.Properties.DataSource = DB.GetData("select * from Personeller with(nolock)");
            luePersonel.EditValue = int.Parse(_persone_id);
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = @"insert into Personelizinleri (fkPersoneller,baslangic_tarihi,bitis_tarihi,izin_sayisi,kayit_tarihi) " +
                " values(@fkPersoneller,@baslangic_tarihi,@bitis_tarihi,@izin_sayisi,getdate())";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkPersoneller", luePersonel.EditValue.ToString()));
            list.Add(new SqlParameter("@baslangic_tarihi", ilktarihi.DateTime));
            list.Add(new SqlParameter("@bitis_tarihi", bitistarihi.DateTime));
            list.Add(new SqlParameter("@izin_sayisi", seizinsayisi.Value));

            DB.ExecuteSQL(sql,list);

            PersonelIzinleriListesi();
            

            //kalan izinler kalanı güncelle
            DB.ExecuteSQL("update PersonelIzınHakedis set izin_kullanilan=izin_kullanilan+" + seizinsayisi.Value.ToString()+
                " where yil=2017 and fkPersoneller=" + luePersonel.EditValue.ToString());

            kalanizinler();

        }

        void PersonelIzinleriListesi()
        {
            gridControl2.DataSource = DB.GetData("select * from Personelizinleri with(nolock) where fkPersoneller=" + luePersonel.EditValue.ToString());
        }
        private void luePersonel_EditValueChanged(object sender, EventArgs e)
        {
            PersonelIzinleriListesi();
        }

        private void bitistarihi_EditValueChanged(object sender, EventArgs e)
        {
            TimeSpan ts = bitistarihi.DateTime- ilktarihi.DateTime;
            seizinsayisi.Value = ts.Days;
        }
    }
}  
    
    
    
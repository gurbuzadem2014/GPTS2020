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
    public partial class frmSiparisAl : DevExpress.XtraEditors.XtraForm
    {
        public frmSiparisAl()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (DB.PkFirma == 0)
            {
                MessageBox.Show("Müşteri Bulunamadı");
                return;
            }
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", DB.PkFirma));
            list.Add(new SqlParameter("@fkPerTeslimEden", lUEPersonel.EditValue.ToString()));//dtSablon.Rows[0]["fkPersoneller"].ToString()));
            list.Add(new SqlParameter("@TeslimTarihi", dateEdit1.DateTime));
            list.Add(new SqlParameter("@Aciklama", memoEdit1.Text));


            string pkSatislarYeniid = DB.ExecuteScalarSQL("INSERT INTO Satislar (Tarih,fkFirma,GelisNo,Siparis,fkKullanici,fkSatisDurumu,GuncellemeTarihi,fkPerTeslimEden,TeslimTarihi,Aciklama) " +
                " values(getdate(),@fkFirma,0,0,1,10,getdate(),@fkPerTeslimEden,@TeslimTarihi,@Aciklama) SELECT IDENT_CURRENT('Satislar')", list);
            this.Tag = "1";
                //ArrayList list2 = new ArrayList();
                //list2.Add(new SqlParameter("@pkSatislarYeniid", pkSatislarYeniid));
                //list2.Add(new SqlParameter("@fkStokKarti","1"));
                //list2.Add(new SqlParameter("@Adet", "1"));
                //DataTable dtStokkarti = DB.GetData("select pkStokKarti,AlisFiyati,SatisFiyati,KdvOrani from StokKarti where pkStokKarti=1" );
                //string alisfiyati = "0", SatisFiyati = "0",KdvOrani = "0";
                //alisfiyati =  dtStokkarti.Rows[0]["AlisFiyati"].ToString();
                //SatisFiyati = dtStokkarti.Rows[0]["SatisFiyati"].ToString();
                //KdvOrani = dtStokkarti.Rows[0]["KdvOrani"].ToString();
                
                //list2.Add(new SqlParameter("@AlisFiyati", alisfiyati.Replace(",",".")));
                //list2.Add(new SqlParameter("@SatisFiyati", SatisFiyati.Replace(",", ".")));
                //list2.Add(new SqlParameter("@KdvOrani", KdvOrani));
                //list2.Add(new SqlParameter("@NakitFiyat", SatisFiyati.Replace(",", ".")));
                //DB.ExecuteSQL("INSERT INTO SatisDetay (fkSatislar,fkStokKarti,Adet,AlisFiyati,SatisFiyati,Tarih,Stogaisle,iskontotutar,iskontoyuzdetutar,KdvOrani,NakitFiyat,fkAlisDetay)" +
                //" values(@pkSatislarYeniid,@fkStokKarti,@Adet,@AlisFiyati,@SatisFiyati,getdate(),0,0,0,@KdvOrani,@NakitFiyat,0)", list2);
                Close();
        }
        void PersonelGetir()
        {
            lUEPersonel.Properties.DataSource = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller where Plasiyer=1 and AyrilisTarihi is null");
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";
            lUEPersonel.EditValue = 1;
        }
        private void frmSiparisAl_Load(object sender, EventArgs e)
        {
               DataTable dt = DB.GetData("select * from Firmalar where pkFirma=" + DB.PkFirma);
               if (dt.Rows.Count > 0)
               {
                   //pkkurum.Text = dt.Rows[0]["pkFirma"].ToString();
                   musteriadi.Text = dt.Rows[0]["Firmaadi"].ToString();
               }
            dateEdit1.DateTime = DateTime.Today;
            PersonelGetir();
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Tag = "0";
            Close();
        }
    }
}
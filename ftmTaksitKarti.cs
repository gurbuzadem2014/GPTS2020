using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.islemler;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class ftmTaksitKarti : DevExpress.XtraEditors.XtraForm
    {
        string fkTaksitler;
        public ftmTaksitKarti(string pkTaksitler)
        {
            InitializeComponent();
            fkTaksitler= pkTaksitler;
        }

        void TaksitLog()
        {
            gCTaksitler.DataSource = DB.GetData("select * from TaksitLog with(nolock) where fkTaksitler=" + fkTaksitler);
        }

        private void ftmTaksitKarti_Load(object sender, EventArgs e)
        {
            DataTable dt =  DB.GetData(@"select * from Taksitler TL with(nolock)
            left join Taksit T with(nolock) on T.taksit_id=TL.taksit_id
            where pkTaksitler=" + fkTaksitler);

            if (dt.Rows.Count == 0)
            {
                formislemleri.Mesajform("Taksit Bilgisi Bulunamadı","K",200);
                Close();
            }

            seSiraNo.EditValue = dt.Rows[0]["SiraNo"].ToString();
            DateTime tt = Convert.ToDateTime(dt.Rows[0]["Tarih"].ToString());
            TilkTaksitTarihi.DateTime = tt;
            
            decimal odenecek = 0;
            decimal.TryParse(dt.Rows[0]["Odenecek"].ToString(), out odenecek);
            ceTaksitTutari.Value = odenecek;

            if (dt.Rows[0]["OdendigiTarih"].ToString()!="")
                dateEdit1.EditValue = dt.Rows[0]["OdendigiTarih"].ToString();

            decimal odenen = 0;
            decimal.TryParse(dt.Rows[0]["Odenen"].ToString(), out odenen);
            calcEdit1.Value = odenen;

            TAciklama.Text = dt.Rows[0]["Aciklama"].ToString();
            TaksitLog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Tarih", TilkTaksitTarihi.DateTime));
                //.ToString("yyyy-MM-dd HH:mm")));
            list.Add(new SqlParameter("@Odenecek", ceTaksitTutari.EditValue.ToString().Replace(",",".")));

            if (dateEdit1.EditValue==null)
               list.Add(new SqlParameter("@OdendigiTarih", DBNull.Value));
            else
               list.Add(new SqlParameter("@OdendigiTarih", dateEdit1.DateTime));

            list.Add(new SqlParameter("@Odenen", calcEdit1.EditValue.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Aciklama", TAciklama.Text));
            list.Add(new SqlParameter("@pkTaksitler", fkTaksitler));
            list.Add(new SqlParameter("@SiraNo", seSiraNo.Value.ToString().Replace(",",".")));

            string sonuc = DB.ExecuteSQL(@"update Taksitler set Tarih=@Tarih,Odenecek=@Odenecek,SiraNo=@SiraNo,
            OdendigiTarih=@OdendigiTarih,Odenen=@Odenen,Aciklama=@Aciklama where pkTaksitler=@pkTaksitler", list);

            if (sonuc == "0")
            {
                formislemleri.Mesajform("Taksit Bilgisi Kaydedildi", "S",200);

                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@Tarih", TilkTaksitTarihi.DateTime));
                //.ToString("yyyy-MM-dd HH:mm")));
                list2.Add(new SqlParameter("@Odenecek", ceTaksitTutari.EditValue.ToString().Replace(",", ".")));

                if (dateEdit1.EditValue == null)
                    list2.Add(new SqlParameter("@OdendigiTarih", DBNull.Value));
                else
                    list2.Add(new SqlParameter("@OdendigiTarih", dateEdit1.DateTime));

                list2.Add(new SqlParameter("@Odenen", calcEdit1.EditValue.ToString().Replace(",", ".")));

                list2.Add(new SqlParameter("@Aciklama", TAciklama.Text));

                list2.Add(new SqlParameter("@fkTaksitler", fkTaksitler));
                list2.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));

                DB.ExecuteSQL(@"insert into TaksitLog (fkTaksitler,fkKullanici,Tarih,Odenecek,Odenen,Aciklama,OdendigiTarih,LogTarihi) 
                values(@fkTaksitler,@fkKullanici,@Tarih,@Odenecek,@Odenen,@Aciklama,@OdendigiTarih,getdate())", list2);
                
                //anımsatmayı güncelle
                ArrayList list3 = new ArrayList();
                list3.Add(new SqlParameter("@Tarih", TilkTaksitTarihi.DateTime));
                list3.Add(new SqlParameter("@BitisTarihi", TilkTaksitTarihi.DateTime.AddHours(1)));
                list3.Add(new SqlParameter("@fkTaksitler", fkTaksitler));
                list3.Add(new SqlParameter("@animsat_zamani", TilkTaksitTarihi.DateTime));
                DB.ExecuteSQL(@"update HatirlatmaAnimsat set Tarih =@Tarih, BitisTarihi=@BitisTarihi,animsat_zamani=@animsat_zamani where fkTaksitler=@fkTaksitler", list3);

                Close();
            }
            else
                formislemleri.Mesajform("Hata Oluştu" + sonuc, "K", 200);

            //TaksitLog();
           
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
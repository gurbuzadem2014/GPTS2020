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
using GPTS.islemler;

namespace GPTS
{
    public partial class frmHatirlatmaUzat : DevExpress.XtraEditors.XtraForm
    {
       // DateTime BasTar, BitisTarihi;
        int pkFirma = -1;//müşteri bağımsız olduğu için
        public frmHatirlatmaUzat(DateTime Start, DateTime End, int fkFirma)
        {
            InitializeComponent();

            if (Start == null)
                dateEdit1.DateTime = DateTime.Now;
            else
                dateEdit1.DateTime = Start;
            //    BasTar = DateTime.Now;
            //else
            //    BasTar = Start;

                //if (End == null)
                //    BitisTarihi = DateTime.Now;
                //else

                //BitisTarihi = End;

            pkFirma = fkFirma;
            teFirmaid.Tag = pkFirma;
            if (pkFirma >0)
            {
                MusteriBilgileri(pkFirma.ToString());
            }
        }

        private void frmHatirlatma_Load(object sender, EventArgs e)
        {
            HatirlatmaAnimsatDurum();

            if (DB.pkHatirlatmaAnimsat == 0)
            {
                BtnKaydet.Text = "Kaydet";
                lueHatirlatmaDurum.EditValue = 1;
                //dateEdit1.DateTime = BasTar;
                //dtBitTarih.DateTime = BitisTarihi;
                Aciklama.Focus();
            }
            else
            {
                BtnKaydet.Text = "Güncelle";
                HatirlatmaGetir();
            }  
        }

        void HatirlatmaGetir()
        {
            DataTable dt = DB.GetData(@"select * from HatirlatmaAnimsat h with(nolock) 
            left join Firmalar f with(nolock) on f.pkFirma=h.fkFirma
            where pkHatirlatmaAnimsat=" + DB.pkHatirlatmaAnimsat.ToString());

            if (dt.Rows.Count == 0) return;

            cbKategori.Text = dt.Rows[0]["Konu"].ToString();
            dtBasTar.DateTime = Convert.ToDateTime(dt.Rows[0]["Tarih"]);
            dateEdit1.DateTime = Convert.ToDateTime(dt.Rows[0]["animsat_zamani"]);
            dtBitTarih.DateTime = Convert.ToDateTime(dt.Rows[0]["BitisTarihi"]);
            Aciklama.Text = dt.Rows[0]["Aciklama"].ToString();
            if (dt.Rows[0]["Uyar"].ToString() == "True")
                cbUyar.Checked = true;
            else
                cbUyar.Checked = false;

            if (dt.Rows[0]["EpostaGonder"].ToString() == "True")
                cbEposta.Checked = true;
            else
                cbEposta.Checked = false;

            if (dt.Rows[0]["SmsGonder"].ToString() == "True")
                cbEposta.Checked = true;
            else
                cbEposta.Checked = false;

            teFirmaid.Text = dt.Rows[0]["firmaadi"].ToString();
            teFirmaid.Tag = dt.Rows[0]["fkFirma"].ToString();

            string fkDurumu = dt.Rows[0]["fkDurumu"].ToString();
            if (fkDurumu != "")
                lueHatirlatmaDurum.EditValue = int.Parse(fkDurumu);

            if (dt.Rows[0]["arandi"].ToString() == "True")
                cbArandi.Checked = true;
            else
                cbArandi.Checked = false;

            if (dt.Rows[0]["animsat"].ToString() == "True")
                cbAnimsat.Checked = true;
            else
                cbAnimsat.Checked = false;

            string gunsonra = dt.Rows[0]["gun_sonra"].ToString();
            if (gunsonra == "") gunsonra = "0";

            seGunSonra.Value = int.Parse(gunsonra);
        }

        void HatirlatmaAnimsatDurum()
        {
            lueHatirlatmaDurum.Properties.DataSource = DB.GetData(@"select 0 as pkHatirlatmaAnimsatDurum,'Seçiniz...' as durumu  
            union all select pkHatirlatmaAnimsatDurum,durumu from HatirlatmaAnimsatDurum with(nolock)");
            lueHatirlatmaDurum.EditValue = 5;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string s =formislemleri.MesajBox("Hatırlatma Silinsin mi?", "Randevu Sil", 1, 1);
            
            if (s=="0") return;

            DB.ExecuteSQL("delete from HatirlatmaAnimsat where pkHatirlatmaAnimsat=" + DB.pkHatirlatmaAnimsat.ToString());
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            dateEdit1.DateTime = ZamanKontrol(dateEdit1.DateTime, 22);

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Konu", cbKategori.Text));
            list.Add(new SqlParameter("@Tarih", dtBasTar.DateTime));
            list.Add(new SqlParameter("@BitisTarihi", dtBitTarih.DateTime));
            list.Add(new SqlParameter("@Kategori", cbKategori.Text));
            list.Add(new SqlParameter("@Aciklama", Aciklama.Text));
            list.Add(new SqlParameter("@fkfirma", teFirmaid.Tag.ToString()));
            int uyar = 0;
            if (cbUyar.Checked) uyar = 1;
            list.Add(new SqlParameter("@Uyar", uyar));

            list.Add(new SqlParameter("@EpostaGonder", cbEposta.Checked));
            list.Add(new SqlParameter("@SmsGonder", cbSms.Checked));
            list.Add(new SqlParameter("@fkDurumu", lueHatirlatmaDurum.EditValue));
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
            list.Add(new SqlParameter("@arandi", cbArandi.Checked));
            list.Add(new SqlParameter("@animsat", cbAnimsat.Checked));
            list.Add(new SqlParameter("@animsat_zamani", dateEdit1.DateTime));
            list.Add(new SqlParameter("@gun_sonra", seGunSonra.Value));
           
            
            if (DB.pkHatirlatmaAnimsat == 0)
                DB.ExecuteSQL(@"INSERT INTO HatirlatmaAnimsat (Konu,Tarih,BitisTarihi,Aciklama,Uyar,EpostaGonder,SmsGonder,
                    Kategori,fkFirma,fkDurumu,fkKullanicilar,arandi,animsat,animsat_zamani,gun_sonra) " +
                            " values(@Konu,@Tarih,@BitisTarihi,@Aciklama,@Uyar,@EpostaGonder,@SmsGonder,"+
                            "@Kategori,@fkFirma,@fkDurumu,@fkKullanicilar,@arandi,@animsat, @animsat_zamani,@gun_sonra)", list);
            else
                DB.ExecuteSQL("UPDATE HatirlatmaAnimsat SET Konu=@Konu,Tarih=@Tarih,BitisTarihi=@BitisTarihi," +
                    "Aciklama=@Aciklama,"+
                    "Uyar=@Uyar,EpostaGonder=@EpostaGonder,Kategori=@Kategori,fkFirma=@fkFirma,"+
                    "fkDurumu=@fkDurumu,arandi=@arandi,"+
                    "animsat=@animsat,animsat_zamani=@animsat_zamani,gun_sonra=@gun_sonra where pkHatirlatmaAnimsat=" + 
                    
            DB.pkHatirlatmaAnimsat.ToString(), list);

            Close();
        }

        
        private void comboBoxEdit3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTekrarlamaSecenek.SelectedIndex == 0)
            {
                seGunSonra.Value = 0;
                labelControl4.Visible = false;
                dtBitTarih.Visible = false;
            }
            else if (cbTekrarlamaSecenek.SelectedIndex == 1)
            {
                seGunSonra.Value = 1;
            }
            else if (cbTekrarlamaSecenek.SelectedIndex == 2)
            {
                seGunSonra.Value = 7;
            }
            else if (cbTekrarlamaSecenek.SelectedIndex == 3)
            {
                seGunSonra.Value = 30;
            }
            else if (cbTekrarlamaSecenek.SelectedIndex == 4)
            {
                seGunSonra.Value = 365;
            }
            else
            {
                labelControl4.Visible = true;
                dtBitTarih.Visible = true;
            }
        }

            private void labelControl8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hitityazilim.com");
        }

        void MusteriBilgileri(string id)
        {
            //teFirmaid.Tag = MusteriAra.fkFirma.Tag.ToString();

            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + id);

            if (dt.Rows.Count == 0) return;

            teFirmaid.Text = dt.Rows[0]["Firmaadi"].ToString();
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.ShowDialog();

            teFirmaid.Tag = MusteriAra.fkFirma.Tag.ToString();

            MusteriBilgileri(teFirmaid.Tag.ToString());
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            teFirmaid.Text = "";
            teFirmaid.Tag = "0";
        }

        private void dtBitTarih_EditValueChanged(object sender, EventArgs e)
        {
            //if (dateEdit1.EditValue == null)
              //  dateEdit1.DateTime = dtBitTarih.DateTime;
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            //if(dateEdit1.DateTime)
            dtBasTar.DateTime = dateEdit1.DateTime;
            dtBitTarih.DateTime = dateEdit1.DateTime.AddHours(1);
        }

        private void frmHatirlatmaUzat_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Escape == e.KeyCode)
                Close();
        }

        private void dtBasTar_EditValueChanged(object sender, EventArgs e)
        {
            //dtBitTarih.DateTime = dtBasTar.DateTime.AddMinutes(30);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            dateEdit1.DateTime = DateTime.Now;
        }

        private void dateEdit1_Leave(object sender, EventArgs e)
        {
            dateEdit1.DateTime=ZamanKontrol(dateEdit1.DateTime,22);
        }

        public DateTime ZamanKontrol(DateTime zaman)
        {
            if (zaman.Hour >= 22)
            {
                DateTime D_T = new DateTime(zaman.Year, zaman.Month, zaman.Day, 22, 0, 0);
                zaman = D_T;
            }
            return zaman;
        }

        public DateTime ZamanKontrol(DateTime zaman,int saatdenbuyuk)
        {
            if (zaman.Hour >= saatdenbuyuk)
            {
                DateTime D_T = new DateTime(zaman.Year, zaman.Month, zaman.Day, 22, 0, 0);
                zaman = D_T;
            }
            return zaman;
        }
    }
}
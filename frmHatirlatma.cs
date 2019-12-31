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
    public partial class frmHatirlatma : DevExpress.XtraEditors.XtraForm
    {
        DateTime BasTar, BitisTarihi;
        int pkFirma = 0;
        public frmHatirlatma(DateTime Start, DateTime End, int fkFirma)
        {
            InitializeComponent();

            if (Start == null)
                BasTar = DateTime.Now;
            else
                BasTar = Start;

            if (End == null)
                BitisTarihi = DateTime.Now;
            else

            BitisTarihi = End;

            pkFirma = fkFirma;
        }

        private void frmHatirlatma_Load(object sender, EventArgs e)
        {
            HatirlatmaDurum();
            OdalarGetir();

            if (DB.pkHatirlatma == 0)
            {
                BtnKaydet.Text = "Kaydet";
                dtBasTar.DateTime = BasTar;
                //dtBitTarih.DateTime = BitisTarihi;
            }
            else
            {
                BtnKaydet.Text = "Güncelle";
                HatirlatmaGetir();
            }  

        }
        void OdalarGetir()
        {
            lueOdalar.Properties.DataSource = DB.GetData(@"select 0 as pkOda,0 as fkKat,'Tüm Odalar' as oda_adi
union all
select pkOda,fkKat,oda_adi from Odalar with(nolock) where aktif = 1");
            lueOdalar.EditValue = 0;
        }
        void HatirlatmaGetir()
        {
            DataTable dt = DB.GetData(@"select * from Hatirlatma h with(nolock) 
            left join Firmalar f with(nolock) on f.pkFirma=h.fkFirma
            where pkHatirlatma=" + DB.pkHatirlatma.ToString());

            if (dt.Rows.Count == 0) return;

            cbKategori.Text = dt.Rows[0]["Konu"].ToString();
            dtBasTar.DateTime = Convert.ToDateTime(dt.Rows[0]["Tarih"]);
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

            if (dt.Rows[0]["fkOda"].ToString() != "")
            {
                int oda_id = 0;
                int.TryParse(dt.Rows[0]["fkOda"].ToString(), out oda_id);
                lueOdalar.EditValue = oda_id;
            }
        }

        void HatirlatmaDurum()
        {
            lueHatirlatmaDurum.Properties.DataSource = DB.GetData(@"select 0 as pkHatirlatmaDurum,'Seçiniz...' as durumu  
            union all select pkHatirlatmaDurum,durumu from HatirlatmaDurum with(nolock)");
            lueHatirlatmaDurum.EditValue = 5;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string s =formislemleri.MesajBox("Hatırlatma Silinsin mi?", "Randevu Sil", 1, 1);
            
            if (s=="0") return;

            DB.ExecuteSQL("delete from Hatirlatma where pkHatirlatma="+ DB.pkHatirlatma.ToString());
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
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
            list.Add(new SqlParameter("@arandi", cbArandi.Checked));
            list.Add(new SqlParameter("@animsat", cbAnimsat.Checked));
            list.Add(new SqlParameter("@fkOda", lueOdalar.EditValue.ToString()));


            if (DB.pkHatirlatma == 0)
                DB.ExecuteSQL("INSERT INTO Hatirlatma (Konu,Tarih,BitisTarihi,Aciklama,Uyar,EpostaGonder,SmsGonder,Kategori,fkFirma,fkDurumu,arandi,animsat,fkOda) " +
                            " values(@Konu,@Tarih,@BitisTarihi,@Aciklama,@Uyar,@EpostaGonder,@SmsGonder,@Kategori,@fkFirma,@fkDurumu,@arandi,@animsat,@fkOda)", list);
            else
                DB.ExecuteSQL("UPDATE Hatirlatma SET Konu=@Konu,Tarih=@Tarih,BitisTarihi=@BitisTarihi,Aciklama=@Aciklama,"+
                        "Uyar=@Uyar,EpostaGonder=@EpostaGonder,Kategori=@Kategori,fkFirma=@fkFirma,fkDurumu=@fkDurumu,arandi=@arandi,"+
                        "animsat=@animsat,fkOda=@fkOda where pkHatirlatma=" + 

                    DB.pkHatirlatma.ToString(), list);
            Close();
        }

        
        private void comboBoxEdit3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTekrarlamaSecenek.SelectedIndex == 0)
            {
                labelControl4.Visible = false;
                dtBitTarih.Visible = false;
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmMusteriAra MusteriAra = new frmMusteriAra();
            //MusteriAra.fkFirma.Tag = fkFirma;
            MusteriAra.ShowDialog();
            teFirmaid.Tag = MusteriAra.fkFirma.Tag.ToString();

            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + teFirmaid.Tag.ToString());

            if (dt.Rows.Count == 0) return;
            teFirmaid.Text = dt.Rows[0]["Firmaadi"].ToString();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            teFirmaid.Text = "";
            teFirmaid.Tag = "0";
        }

        private void dtBasTar_EditValueChanged(object sender, EventArgs e)
        {
            dtBitTarih.DateTime = dtBasTar.DateTime.AddMinutes(30);
        }
    }
}
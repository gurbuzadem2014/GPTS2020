using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class frmTransfer : DevExpress.XtraEditors.XtraForm
    {
        string fkpersoneller = ConfigurationSettings.AppSettings["selectperid"].ToString();
        public frmTransfer()
        {
            InitializeComponent();
        }

        void PersonelGetir(string fkPersoneller)
        {
            gridControl2.DataSource = DB.GetData(@"SELECT *  FROM   ProjeTransfer  WHERE fkPersonel =" + fkPersoneller + " order by TransferTarihi desc");
        }
        private void lUEPersonel_EditValueChanged(object sender, EventArgs e)
        {
            string fkPersoneller = lUEPersonel.EditValue.ToString();
            try
            {
                DataTable dt = DB.GetData("select PkFirma,Firmaadi,maasi,isegiristarih from Firmalar F inner join Personeller P on P.fkgrup=F.PkFirma where P.pkpersoneller=" +
                fkPersoneller);
                tECalProje.Tag = dt.Rows[0]["PkFirma"].ToString();
                tECalProje.Text = dt.Rows[0]["Firmaadi"].ToString();
                decimal maasi = 0;
                decimal.TryParse(dt.Rows[0]["maasi"].ToString(), out maasi);
                cEMaasi.EditValue = maasi.ToString();
                yeniMaasi.Value = maasi;
                deisegirisTarihi.EditValue = dt.Rows[0]["isegiristarih"].ToString();
                DateTime t1 = deisegirisTarihi.DateTime, t2;
                t2 = DateTime.Now;
                TimeSpan ts = t2 - t1;
                sEGun.Value = ts.Days;
                cEGunluk.Value = cEMaasi.Value / 30;
                cEHakedis.Value = cEGunluk.Value * sEGun.Value;
                PersonelGetir(fkPersoneller);
            }
            catch (Exception exp)
            {

            }
        }

        private void frmTransfer_Load(object sender, EventArgs e)
        {
            TrsTarih.DateTime = DateTime.Today;
            DataTable dt = DB.GetData("SELECT * FROM  Projeler");
            luekurum.Properties.DataSource = dt;
            luekurum.Properties.ValueMember = "pkProjeler";
            luekurum.Properties.DisplayMember = "ProjeAdi";
            //luekurum.Text = "Proje Seçiniz.";
            dt.Dispose();
            dt = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller");
            lUEPersonel.Properties.DataSource = dt;
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";

            lUEPersonel.EditValue = int.Parse(fkpersoneller);
            PersonelGetir(fkpersoneller.ToString());
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (luekurum.Text == "Seçiniz...") 
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Transfer Edilecek Kurumu Seçiniz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                luekurum.Focus();
                return;
            }
            //maaş hesaplanacak hareketler atılacak kaçgün çalıştıysa.
            //yeni projede kaç gün çalıştıysa maaşını aya bölecek maaş hakedişine atacak maaş hekediş hesaplanırken.
            //yeni maaşını da kaydet personel kartına.
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkProje", luekurum.EditValue));
            list.Add(new SqlParameter("@fkPersonel", lUEPersonel.EditValue.ToString()));
            list.Add(new SqlParameter("@TrasnferNedeni", TrsNedeni.Text));
            list.Add(new SqlParameter("@TransferTarihi", TrsTarih.EditValue.ToString()));
            if (cEMaasi.Value==null)
                list.Add(new SqlParameter("@EskiMaasi", "0"));
            else
            list.Add(new SqlParameter("@EskiMaasi", cEMaasi.Value.ToString().Replace(",",".")));

            if (yeniMaasi.Value==null)
                list.Add(new SqlParameter("@YeniMaasi", "0"));
            else
            list.Add(new SqlParameter("@YeniMaasi", yeniMaasi.Value.ToString().Replace(",", ".")));

            list.Add(new SqlParameter("@OncekiProje", tECalProje.Tag));
            DB.ExecuteSQL(@"INSERT INTO ProjeTransfer ( fkPersonel, fkProje, TrasnferNedeni, TransferTarihi, EskiMaasi, YeniMaasi, OncekiProje) 
            values(@fkPersonel,@fkProje,@TrasnferNedeni,@TransferTarihi,@EskiMaasi,@YeniMaasi,@OncekiProje)",list);
            list.Clear();
            list.Add(new SqlParameter("@fkProjeler", luekurum.EditValue));
            list.Add(new SqlParameter("@fkPersonel", lUEPersonel.EditValue.ToString()));
            DB.ExecuteSQL("UPDATE ProjePersonel set GorevYeri='Transfer Edildi.',fkProjeler=@fkProjeler where fkPersonel=@fkPersonel", list);
            list.Clear();
            list.Add(new SqlParameter("@fkgrup", luekurum.EditValue));
            list.Add(new SqlParameter("@pkpersoneller", lUEPersonel.EditValue.ToString()));
            list.Add(new SqlParameter("@maasi", yeniMaasi.Value.ToString()));
            DB.ExecuteSQL("UPDATE Personeller set fkgrup=@fkgrup,maasi=@maasi where pkpersoneller=@pkpersoneller", list);
            
            //kasa hareket
            string sql = @"INSERT INTO KasaHareket (fkkasalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacakaciklama,donem,yil)
                    values(@fkkasalar,@fkPersoneller,getdate(),5,5,@Borc,0,'@aciklama',@donem,@yil)";
            sql = sql.Replace("@fkkasalar", "1");
            sql = sql.Replace("@fkPersoneller", lUEPersonel.EditValue.ToString());
            sql = sql.Replace("@Borc", cEHakedis.Value.ToString().Replace(",", "."));
            sql = sql.Replace("@aciklama", TrsNedeni.Text);
            sql = sql.Replace("@yil", cBYil.EditValue.ToString());
            sql = sql.Replace("@donem", cBDonem.EditValue.ToString());
                    int sonuc = DB.ExecuteSQL(sql);
            if (sonuc == 1)
               {
                   DevExpress.XtraEditors.XtraMessageBox.Show("Personel Transfer Edildi.", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   PersonelGetir(fkpersoneller.ToString());
               }
            else
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu.", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void sEGun_EditValueChanged(object sender, EventArgs e)
        {
            cEHakedis.Value = Math.Round(cEGunluk.Value * sEGun.Value, 2);
        }
    }
}
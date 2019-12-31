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
    public partial class frmProjeKarti : DevExpress.XtraEditors.XtraForm
    {
        public frmProjeKarti()
        {
            InitializeComponent();
            ProSirketler();
            //ProjePersoneller2();
        }
        void ProjePersoneller2()
        {
            DataTable dtp = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as Adi FROM personeller where fkGrup=0");
            repositoryItemLookUpEdit1.DataSource = dtp;
            repositoryItemLookUpEdit1.ValueMember = "pkpersoneller";
            repositoryItemLookUpEdit1.DisplayMember = "Adi";
        }
        void projepersonellerigetir(int projeid)
        {
            gCPersoneller.DataSource = DB.GetData(@"SELECT ProjePersonel.fkPersonel,ProjePersonel.pkProjePersonel, ProjePersonel.fkProjeler, ProjePersonel.fkPersonel, ProjePersonel.Aktif, Personeller.adi, Personeller.soyadi, Personeller.isegiristarih,
            Personeller.maasi, Personeller.agiucreti, Personeller.bankamaasi,ProjePersonel.GorevYeri,
            (Personeller.maasi+ Personeller.agiucreti+ Personeller.bankamaasi) as Toplam
            FROM ProjePersonel LEFT OUTER JOIN  Personeller ON ProjePersonel.fkPersonel = Personeller.pkpersoneller
            WHERE ProjePersonel.fkProjeler =" + projeid.ToString());
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            btnKaydetProje_Click(sender,e);
            gCProje.Text = " Proje Bilgileri "+ DB.pkProjeler.ToString()+". "+tEProjeAdi.Text;
            
            string sql="";
            for (int i = 0; i < sEPersonelAdet.Value; i++)
            {
                sql += "INSERT INTO ProjePersonel (fkProjeler,fkPersonel,GorevYeri,SiraNo) values(" + DB.pkProjeler.ToString() + ",0,'Tanımlanmamış'," + (i + 1).ToString() + ")";
            }
            DB.ExecuteSQL(sql);
            DevExpress.XtraEditors.XtraMessageBox.Show("Proje Bilgileri Kaydedildi.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            projepersonellerigetir(DB.pkProjeler);
        }
        private void luekurum_EditValueChanged(object sender, EventArgs e)
        {
            tEProjeAdi.Text = luekurum.Text;
        }
        void PersonelGetir()
        {
            DataTable dtp = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller where fkGrup=0 and AyrilisTarihi is null");
            lUEPersonel.Properties.DataSource = dtp;
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";
        }
        void tabgetir()
        {
            switch (xtraTabControl1.SelectedTabPageIndex)
            {
                case 0: ProjeGetir(); break;
                case 1: ProjeGorusmeGetir(); break;
                case 2: PersonelGetir(); break;
                case 3: projepersonellerigetir(DB.pkProjeler); break;
                default:
                    break;
            }
        }
        private void frmProjeKarti_Load(object sender, EventArgs e)
        {
            if (DB.pkProjeler == 0)
                simpleButton1.Text = "Kaydet [F9]";
            else
                simpleButton1.Text = "Güncelle [F9]";
            tabgetir();
        }
        void ProjeGetir()
        {
            bsTarih.EditValue = DateTime.Today.ToString("dd.MM.yyyy");
            //btTarih.EditValue = DateTime.Today.ToString("dd.MM.yyyy");
            luekurum.Properties.DataSource = DB.GetData("SELECT * FROM Firmalar where Aktif=1");
            DataTable dt2 = DB.GetData("SELECT * FROM Projeler where pkProjeler=" + DB.pkProjeler.ToString());
            if (dt2.Rows.Count > 0)
            {
                lUSirket.EditValue = int.Parse(dt2.Rows[0]["fkSirket"].ToString());
                luekurum.EditValue = int.Parse(dt2.Rows[0]["fkFirmalar"].ToString());
                tEProjeAdi.Text = dt2.Rows[0]["ProjeAdi"].ToString();
                ceAnlasmaTutari.EditValue = dt2.Rows[0]["AnlasmaTutari"].ToString();
                sEPersonelAdet.EditValue = dt2.Rows[0]["PersonelAdet"].ToString();
                bsTarih.EditValue = dt2.Rows[0]["BaslangicTarihi"].ToString();
                if (dt2.Rows[0]["BitisTarihi"].ToString() != "")
                    btTarih.EditValue = dt2.Rows[0]["BitisTarihi"].ToString();
                gCProje.Text = "Proje Bilgileri " + DB.pkProjeler.ToString() + "." + tEProjeAdi.Text;
            }
        }
        void ProjeGorusmeGetir()
        {
            gcGorusmeler.DataSource = DB.GetData("select * from ProjeGorusme where fkProjeler="+DB.pkProjeler);
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (lUEPersonel.EditValue.ToString() == "0")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Personel Seçiniz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUEPersonel.Focus();
                return;
            }
            if (lUEPersonel.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Personel Seçiniz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUEPersonel.Focus();
                return;
            }
            if (DB.pkProjeler == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Proje Seçiniz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUEPersonel.Focus();
                return;
            }

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkPersonel", lUEPersonel.EditValue));
            //SELECT IDENT_CURRENT('ProjePersonel')            
            //string pkProjePersonel = DB.ExecuteSQL("INSERT INTO ProjePersonel (fkProjeler,fkPersonel,GorevYeri,SiraNo) values(@fkProjeler,@fkPersoneller,'GorevYeri',0)", list);
            if (simpleButton2.Tag.ToString() == "0")
            {
                list.Add(new SqlParameter("@fkProjeler", DB.pkProjeler));
                //DB.GetData("select max() as c from ProjePersonel where pkProjePersonel=" + simpleButton2.Tag);
                DB.ExecuteSQL("INSERT INTO ProjePersonel (fkProjeler,fkPersonel,GorevYeri,SiraNo,Aktif) values(@fkProjeler,@fkPersonel,'Proje Eklendi',0,1)", list);
            }
            else
            {
                list.Add(new SqlParameter("@pkProjePersonel", simpleButton2.Tag));
                DB.ExecuteSQL("UPDATE ProjePersonel SET GorevYeri='Projeye Eklendi.',fkPersonel=@fkPersonel WHERE pkProjePersonel =@pkProjePersonel", list);
            }
            
                //DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu.", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            simpleButton2.Text = "Projeye Ekle";
            simpleButton2.Tag = 0;
            //
            DB.ExecuteSQL("UPDATE Personeller SET fkgrup=" + DB.pkProjeler.ToString()+
            " WHERE pkpersoneller="+lUEPersonel.EditValue.ToString());
            DataRow dr2 = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("UPDATE Personeller SET fkgrup=0 WHERE pkpersoneller=" + int.Parse(dr2["fkPersonel"].ToString()));
            projepersonellerigetir(DB.pkProjeler);
            PersonelGetir();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            int fkPersonel = 0;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            fkPersonel = int.Parse(dr["fkPersonel"].ToString());
            simpleButton2.Tag = dr["pkProjePersonel"].ToString();

            lUEPersonel.EditValue = fkPersonel;

            simpleButton2.Text = "Güncelle";

            if (fkPersonel==0)
                btnSil.Enabled = true;
            else
                btnSil.Enabled = false;
        }
        void ProSirketler()
        {
            DataTable dtb = DB.GetData("select * from Sirketler with(nolock)");
            lUSirket.Properties.DataSource = dtb;
            lUSirket.Properties.ValueMember = "pkSirket";
            lUSirket.Properties.DisplayMember = "Sirket";
        }

        private void frmProjeKarti_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (gridView1.DataRowCount != sEPersonelAdet.Value)
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Kadro Sayısı İle Kayıtlı Personel Adeti Farklı \n Lütfen Kontrol Ediniz.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    e.Cancel = true;
            //}
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            lUEPersonel.Properties.NullText = "Seçiniz...";
            DB.ExecuteSQL("update ProjePersonel set GorevYeri='Projeden Çıkartıldı.',fkPersonel=0 where pkProjePersonel=" + simpleButton2.Tag);
            projepersonellerigetir(DB.pkProjeler);
            DevExpress.XtraEditors.XtraMessageBox.Show("Projeden  Personel Çıkartıldı.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DB.ExecuteSQL("UPDATE Personeller SET fkgrup=0 WHERE pkpersoneller=" + lUEPersonel.EditValue.ToString());
            PersonelGetir();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("DELETE FROM ProjePersonel where pkProjePersonel=" + simpleButton2.Tag);
            projepersonellerigetir(DB.pkProjeler);
            DevExpress.XtraEditors.XtraMessageBox.Show("Kayıt Silindi.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);                
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string sql = "";
            //for (int i = 0; i < sEPersonelAdet.Value; i++)
           // {
                sql = "INSERT INTO ProjePersonel (fkProjeler,fkPersonel,GorevYeri,SiraNo) values(" + DB.pkProjeler.ToString() + ",0,'Tanımlanmamış',0)";
           // }
            DB.ExecuteSQL(sql);
            projepersonellerigetir(DB.pkProjeler);
        }

        private void frmProjeKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            } 
        }

        private void tEProjeAdi_EditValueChanged(object sender, EventArgs e)
        {
            projeadi.Text = tEProjeAdi.EditValue.ToString();
        }

        private void btnKaydetProje_Click(object sender, EventArgs e)
        {
            if (bsTarih.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Başlanğıç Tarihi Boş Olamaz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bsTarih.Focus();
                return;
            }
            if (lUSirket.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Şirket Seçmediniz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUSirket.Focus();
                return;
            }
            if (luekurum.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Cari Seçmediniz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                luekurum.Focus();
                return;
            }
            if (tEProjeAdi.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Proje Adı Boş olamaz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tEProjeAdi.Focus();
                return;
            }
            //if (sEPersonelAdet.Value == 0)
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Kadro Sayısı Sıfırdan dan büyük olmalıdır!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    sEPersonelAdet.Focus();
            //    return;
            //}
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkSirket", lUSirket.EditValue));
            list.Add(new SqlParameter("@fkFirmalar", luekurum.EditValue));
            list.Add(new SqlParameter("@BaslangicTarihi", bsTarih.DateTime));
            if (btTarih.EditValue == null)
                list.Add(new SqlParameter("@BitisTarihi", DBNull.Value));
            else
                list.Add(new SqlParameter("@BitisTarihi", btTarih.DateTime));
            list.Add(new SqlParameter("@AnlasmaTutari", ceAnlasmaTutari.Value));
            list.Add(new SqlParameter("@PersonelAdet", sEPersonelAdet.Value));
            list.Add(new SqlParameter("@ProjeAdi", tEProjeAdi.Text));
            string pkProjeler;
            if (DB.pkProjeler == 0)
            {
                pkProjeler = DB.ExecuteScalarSQL("INSERT INTO Projeler (fkFirmalar,BaslangicTarihi,BitisTarihi,AnlasmaTutari,PersonelAdet,ProjeAdi,fkSirket)" +
                 " values(@fkFirmalar,@BaslangicTarihi,@BitisTarihi,@AnlasmaTutari,@PersonelAdet,@ProjeAdi,@fkSirket) SELECT IDENT_CURRENT('Projeler')", list);
                DB.pkProjeler = int.Parse(pkProjeler);
            }
            else
            {
                list.Add(new SqlParameter("@pkProjeler", DB.pkProjeler));
                DB.ExecuteSQL(@"UPDATE Projeler SET fkFirmalar=@fkFirmalar,BaslangicTarihi=@BaslangicTarihi,
                BitisTarihi=@BitisTarihi,AnlasmaTutari=@AnlasmaTutari,PersonelAdet=@PersonelAdet,ProjeAdi=@ProjeAdi,
                fkSirket=@fkSirket where pkProjeler=@pkProjeler", list);
                DevExpress.XtraEditors.XtraMessageBox.Show("Proje Bilgileri Güncellendi.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            tabgetir();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            frmGorusmeDetayBilgileri GorusmeDetayBilgileri = new frmGorusmeDetayBilgileri();
            GorusmeDetayBilgileri.ShowDialog();
            ProjeGorusmeGetir();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmGorusmeDetayBilgileri GorusmeDetayBilgileri = new frmGorusmeDetayBilgileri();
            GorusmeDetayBilgileri.pkProjeGorusme.Text = dr["pkProjeGorusme"].ToString();
            GorusmeDetayBilgileri.ShowDialog();
            ProjeGorusmeGetir();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            lUSirket.EditValue = null;
            luekurum.EditValue = null;
            tEProjeAdi.Text    = "";
            bsTarih.DateTime = DateTime.Today;
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
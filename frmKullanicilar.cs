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
using GPTS.islemler;

namespace GPTS
{
    public partial class frmKullanicilar : DevExpress.XtraEditors.XtraForm
    {
        public frmKullanicilar()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        void vKullanicilar()
        {
            try
            {
                string sql = "select * from Kullanicilar with(nolock)";
                if (radioGroup1.SelectedIndex==0)
                    sql = sql + " where durumu=1";
                else
                    sql = sql + " where isnull(durumu,0)=0";
                DataTable dt = DB.GetData(sql);

                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {

            }
        }

        void SatisDurumlariGetir()
        {
            lueSatisTipi.Properties.DataSource = DB.GetData(@"SELECT pkSatisDurumu, Durumu, Aktif, SiraNo FROM  SatisDurumu with(nolock) WHERE Aktif = 1 ORDER BY SiraNo");
            lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
        }

        private void frmKullanicilar_Load(object sender, EventArgs e)
        {
            if (Degerler.ip_adres == "185.130.56.98" && DB.VeriTabaniAdi == "MTP2012")
                teSifre.Enabled = false;
         
            SatisDurumlariGetir();

            Subeler();

            Depolar();

            Kasalar();

            Personeller();

            KullaniciGruplari();

            SatisFiyatGrubu();

            vKullanicilar();

            //gvizinVerilenYetkiler.ExpandAllGroups();

            //gvizinVerilenYetkiler.ExpandGroupRow(0, true);

            //KullaniciYetkileri seçileni getirin içinde
            secilenigetir(0);


            //roller
            DataTable dtRol = DB.GetData("select * from Roller with(nolock)");
            for (int i = 0; i < dtRol.Rows.Count; i++)
			{
                DevExpress.XtraEditors.Controls.CheckedListBoxItem item = new DevExpress.XtraEditors.Controls.CheckedListBoxItem();
                item.Value = dtRol.Rows[i]["pkRoller"].ToString();
                item.Description = dtRol.Rows[i]["RolAdi"].ToString();
                item.CheckState = CheckState.Checked;
                clbRoller.Items.Add(item);
			} 
        }

        void Depolar()
        {
            lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock)");
            lueDepolar.EditValue = 1;
        }

        void Subeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData("select * from Subeler with(nolock)");
            lueSubeler.EditValue = 1;
        }
        
        void Kasalar()
        {
            lueKasalar.Properties.DataSource = DB.GetData("select * from Kasalar with(nolock)");
            lueKasalar.EditValue = 1;
        }

        void Personeller()
        {
            luePersonel.Properties.DataSource = DB.GetData("select * from Personeller with(nolock)");
            luePersonel.EditValue = 1;
        }

        void KullaniciGruplari()
        {
            lueKullaniciGruplari.Properties.DataSource = DB.GetData("select * from KullaniciGruplari with(nolock)");
            lueKullaniciGruplari.EditValue = 1;
        }

        void SatisFiyatGrubu()
        {
            lueSatisFiyatGrubu.Properties.DataSource = DB.GetData("select * from SatisFiyatlariBaslik with(nolock) where Aktif=1");
            lueSatisFiyatGrubu.EditValue = 1;
        }

        void secilenigetir(int fi)
        {
            if (fi < 0) return;
            int PkKullanicilar = 0;

            System.Data.DataRow row = gridView1.GetDataRow(fi);
            
            fkKullanicilar.Text = row["pkKullanicilar"].ToString();
            int.TryParse(row["PkKullanicilar"].ToString(), out PkKullanicilar);
            teKullanici.Text = row["KullaniciAdi"].ToString();
            teAdiSoyadi.Text = row["adisoyadi"].ToString();
            txtCep.Text = row["Cep"].ToString();
            teSifre.Text = row["Sifre"].ToString();//islemler.CryptoStreamSifreleme.md5SifreyiCoz(row["Sifre"].ToString());
            //teSifre.Text = row["Sifre"].ToString();

            eposta.Text = row["eposta"].ToString();

            if (row["durumu"].ToString() == "False")
                rgDurumu.SelectedIndex = 1;
            else
                rgDurumu.SelectedIndex = 0;

            if (row["AnaBilgisayar"].ToString() == "True")
                ceAnaBilgisayar.Checked = true;
            else
                ceAnaBilgisayar.Checked = false;

            if (row["acilista_hatirlatma_ekrani"].ToString() == "True")
                cbHatirlatmaEkrani.Checked = true;
            else
                cbHatirlatmaEkrani.Checked = false;

            if (row["acilista_caller_id"].ToString() == "True")
                ceCalleridAc.Checked = true;
            else
                ceCalleridAc.Checked = false;


            if (row["hatirlatma_uyar"].ToString() == "True")
                cbHatirlatmaUyar.Checked = true;
            else
                cbHatirlatmaUyar.Checked = false;
            

            int iAktifForm = 0;
            int.TryParse(row["AktifForm"].ToString(), out iAktifForm);
            cbAktifForm.SelectedIndex = iAktifForm;

            int ifaturano = 0;
            int.TryParse(row["FaturaNo"].ToString(), out ifaturano);
            seFaturaNo.Value = ifaturano;

            int fkSatisDurumu = 2;
            int.TryParse(row["fkSatisDurumu"].ToString(), out fkSatisDurumu);
            lueSatisTipi.EditValue = fkSatisDurumu;

            int fkDepolar = 2;
            int.TryParse(row["fkDepolar"].ToString(), out fkDepolar);
            lueDepolar.EditValue = fkDepolar;

            int fkKasalar = 1;
            int.TryParse(row["fkKasalar"].ToString(), out fkKasalar);
            lueKasalar.EditValue = fkKasalar;

            if (!string.IsNullOrEmpty(row["fkPersoneller"].ToString()))
                luePersonel.EditValue = int.Parse(row["fkPersoneller"].ToString());

            if (!string.IsNullOrEmpty(row["fkKullaniciGruplari"].ToString()))
                lueKullaniciGruplari.EditValue = int.Parse(row["fkKullaniciGruplari"].ToString());

            teSeriNo.Text = row["FaturaSeriNo"].ToString();


            int fkSube = 2;
            int.TryParse(row["fkSube"].ToString(), out fkSube);
            lueSubeler.EditValue = fkSube;

            txtYedekYol2.Text = row["yedek_yeri_yol"].ToString();

            if (!string.IsNullOrEmpty(row["fkSatisFiyatlariBaslik"].ToString()))
                lueSatisFiyatGrubu.EditValue = int.Parse(row["fkSatisFiyatlariBaslik"].ToString());

            simpleButton5.Text = "Güncelle";
            simpleButton3.Enabled = true;

            KullaniciYetkileri();
        }

        void KullaniciYetkileri()
        {
            string sql = @"select m.pkModuller,m.Kod,m.ModulAdi,m.ana_id,m.durumu,m.Kod,my.pkModullerYetki,my.Yetki from Moduller m with(nolock)
            left join ModullerYetki my with(nolock) on my.Kod=m.Kod
            where my.fkKullanicilar=" + fkKullanicilar.Text;
            gcizinVerilenYetkiler.DataSource = DB.GetData(sql);
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            secilenigetir(e.RowHandle);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            teKullanici.Text = "";
            teAdiSoyadi.Text = "";
            teSifre.Text = "";
            txtCep.Text = "";
            eposta.Text = "";
            fkKullanicilar.Text = "0";
            rgDurumu.SelectedIndex = 0;
            simpleButton5.Text = "Kaydet";

            teKullanici.Focus();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if(teSifre.Text=="")
            {
                formislemleri.Mesajform("Şifre Boş Olamaz","K",150);
                teSifre.Focus();
                return;
            }
            string sql = "";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@KullaniciAdi",teKullanici.Text));
            list.Add(new SqlParameter("@adisoyadi", teAdiSoyadi.Text));
            list.Add(new SqlParameter("@Sifre", teSifre.Text));
            list.Add(new SqlParameter("@eposta", eposta.Text));
            list.Add(new SqlParameter("@AktifForm",cbAktifForm.SelectedIndex));
            list.Add(new SqlParameter("@durumu", rgDurumu.EditValue.ToString()));
            list.Add(new SqlParameter("@Cep", txtCep.Text.ToString()));
            list.Add(new SqlParameter("@FaturaNo", seFaturaNo.Value));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue));
            list.Add(new SqlParameter("@FaturaSeriNo", teSeriNo.Text));
            //list.Add(new SqlParameter("@Sifreli", islemler.CryptoStreamSifreleme.Encrypt("Hitit999", teSifre.Text)));
            list.Add(new SqlParameter("@Sifreli", teSifre.Text));// islemler.CryptoStreamSifreleme.md5Sifrele(teSifre.Text)));
            list.Add(new SqlParameter("@AnaBilgisayar", ceAnaBilgisayar.Checked));
            list.Add(new SqlParameter("@acilista_hatirlatma_ekrani", cbHatirlatmaEkrani.Checked));
            list.Add(new SqlParameter("@acilista_caller_id", ceCalleridAc.Checked));
            list.Add(new SqlParameter("@fkDepolar", lueDepolar.EditValue));
            list.Add(new SqlParameter("@fkKasalar", lueKasalar.EditValue));
            if(luePersonel.EditValue==null)
                list.Add(new SqlParameter("@fkPersoneller", DBNull.Value));
            else
                list.Add(new SqlParameter("@fkPersoneller", luePersonel.EditValue));

           if(luePersonel.EditValue==null)
              list.Add(new SqlParameter("@fkKullaniciGruplari", DBNull.Value));
            else
              list.Add(new SqlParameter("@fkKullaniciGruplari", lueKullaniciGruplari.EditValue));

            list.Add(new SqlParameter("@hatirlatma_uyar", cbHatirlatmaUyar.Checked));

            if (lueSubeler.EditValue == null)
                list.Add(new SqlParameter("@fkSube", DBNull.Value));
            else
                list.Add(new SqlParameter("@fkSube", lueSubeler.EditValue));

            list.Add(new SqlParameter("@yedek_yeri_yol", txtYedekYol2.Text));

            if (lueSatisFiyatGrubu.EditValue == null)
                list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", DBNull.Value));
            else
                list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", lueSatisFiyatGrubu.EditValue));
            

            string sonuc = "";
            if (fkKullanicilar.Text == "0" || fkKullanicilar.Text == "")
            {
                sql = @"INSERT INTO Kullanicilar (KullaniciAdi,adisoyadi,Sifre,eposta,AktifForm,durumu,KayitTarihi,Cep,FaturaNo,fkSatisDurumu,FaturaSeriNo,Sifreli,
                AnaBilgisayar,acilista_hatirlatma_ekrani,fkDepolar,fkKasalar,fkPersoneller,fkKullaniciGruplari,acilista_caller_id,hatirlatma_uyar,fkSube,
                yedek_yeri_yol,fkSatisFiyatlariBaslik) 
                VALUES(@KullaniciAdi,@adisoyadi,@Sifre,@eposta,@AktifForm,@durumu,getdate(),@Cep,@FaturaNo,@fkSatisDurumu,@FaturaSeriNo,@Sifreli,
                @AnaBilgisayar,@acilista_hatirlatma_ekrani,@fkDepolar,@fkKasalar,@fkPersoneller,@fkKullaniciGruplari,@acilista_caller_id,@hatirlatma_uyar,@fkSube,
                @yedek_yeri_yol,@fkSatisFiyatlariBaslik)
                SELECT IDENT_CURRENT('Kullanicilar')";

                string yeniid = DB.ExecuteScalarSQL(sql, list);

                fkKullanicilar.Text = yeniid;
                //yetkileri ekle
                if (yeniid.Substring(0, 1) != "H")
                {
                    ModullerYetki(yeniid);
                    int s = DB.ExecuteSQL_Sonuc_Sifir("insert into ModullerYetki select Kod," + yeniid + ",1 from Moduller");
                    s = DB.ExecuteSQL_Sonuc_Sifir("insert into YetkiAlanlari select fkParametreler," + yeniid + ",1,1,1 from YetkiAlanlari where fkKullanicilar=1");

                     YetkiAlanlariEkle();
                }
                sonuc = "0";
            }
            else
            {
                sql = @"UPDATE Kullanicilar SET 
                KullaniciAdi=@KullaniciAdi,adisoyadi=@adisoyadi,Sifre=@Sifre,eposta=@eposta,
                AktifForm=@AktifForm,durumu=@durumu,Cep=@Cep,FaturaNo=@FaturaNo,fkSatisDurumu=@fkSatisDurumu,
                FaturaSeriNo=@FaturaSeriNo,Sifreli=@Sifreli,AnaBilgisayar=@AnaBilgisayar,acilista_hatirlatma_ekrani=@acilista_hatirlatma_ekrani,
                fkDepolar=@fkDepolar,fkKasalar=@fkKasalar,fkPersoneller=@fkPersoneller,fkKullaniciGruplari=@fkKullaniciGruplari,
                acilista_caller_id=@acilista_caller_id,hatirlatma_uyar=@hatirlatma_uyar,fkSube=@fkSube,
                yedek_yeri_yol=@yedek_yeri_yol,fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik
                where PkKullanicilar=" + fkKullanicilar.Text;

                sonuc=DB.ExecuteSQL(sql, list);
            }

            if (sonuc == "0")
            {
                formislemleri.Mesajform("Bilgiler Kaydedildi", "S", 200);
            }
            else
                formislemleri.Mesajform("Hata Oluştu" + sonuc, "K", 200);

            //Degerler.fkSatisDurumu = int.Parse(lueSatisTipi.EditValue.ToString()); giriş yaparken olmalı
            int i = gridView1.FocusedRowHandle;

            vKullanicilar();

            gridView1.FocusedRowHandle = i;
            //yetkileri ekle

            ModullerYetki_Kaydet();

            //vParametrelerKontrol();

            Degerler.AnaBilgisayar = ceAnaBilgisayar.Checked;
            int depo=1;
            int.TryParse(lueDepolar.EditValue.ToString(), out depo);
            Degerler.fkDepolar = depo;

            Degerler.fkKullaniciGruplari = lueKullaniciGruplari.EditValue.ToString();
            //Degerler.acilista_hatirlatma_ekrani = cbHatirlatmaEkrani.Checked;
            //if(lueDepolar.EditValue != null)
            //Degerler.fkDepolar = lueDepolar.EditValue.ToString();
            Degerler.isHatirlatmaUyar = cbHatirlatmaUyar.Checked;

            Degerler.fkKasalar = int.Parse(lueKasalar.EditValue.ToString());
            Degerler.fkSatisFiyatlariBaslik = int.Parse(lueSatisFiyatGrubu.EditValue.ToString());
        }

        void ModullerYetki_Kaydet()
        {
            //foreach (DataRow dr in gvizinVerilenYetkiler.DataRowCount)
            //{
            //    string pkModullerYetki = dr["pkModullerYetki"].ToString();
            //}
            for (int i = 0; i < gvizinVerilenYetkiler.DataRowCount; i++)
			{
                DataRow dr = gvizinVerilenYetkiler.GetDataRow(i);
                int yetki = 0;
                if (dr["Yetki"].ToString() == "True")
                    yetki = 1;

                DB.ExecuteSQL("update ModullerYetki set Yetki=" + yetki.ToString() + " WHERE pkModullerYetki=" + dr["pkModullerYetki"].ToString());

			}
        }

        void vParametrelerKontrol()
        {
            DataTable dtParametre =  DB.GetData("SELECT * FROM Parametreler with(nolock)");
            for (int i = 0; i < dtParametre.Rows.Count; i++)
			{
                string pkKullanicilar = fkKullanicilar.Text;
                string fkParametreler= dtParametre.Rows[i]["pkParametreler"].ToString();
                if (DB.GetData("select * from YetkiAlanlari with(nolock) where fkKullanicilar=" + pkKullanicilar 
                    + " and fkParametreler=" + fkParametreler).Rows.Count == 0)
               {
                string sql = "";
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkKullanicilar", pkKullanicilar));
                list.Add(new SqlParameter("@fkParametreler", fkParametreler));
                    //yetki 1 olmaz ise hızlı butonlar gelmiyor
                list.Add(new SqlParameter("@Yetki", "1"));
                list.Add(new SqlParameter("@Sayi", "1"));
                sql = "INSERT INTO YetkiAlanlari (fkKullanicilar,fkParametreler,Yetki,Sayi) VALUES(@fkKullanicilar,@fkParametreler,@Yetki,@Sayi)";
                DB.ExecuteSQL(sql, list);
               }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (fkKullanicilar.Text == "1") 
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Bu Kullanıcı Silinemez!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (DB.GetData("select * from Satislar with(nolock) where fkKullanici=" + fkKullanicilar.Text).Rows.Count > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Bu Kullanıcı Satış Yapığı için Silinemez!", "GPTS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kullanıcı Silinsin mi?", "GPTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            try
            {
                DB.ExecuteSQL("Delete From ModullerYetki where fkKullanicilar=" + fkKullanicilar.Text);
                DB.ExecuteSQL("Delete From Kullanicilar where PkKullanicilar=" + fkKullanicilar.Text);
            }
            catch (Exception exp)
            {
                simpleButton3.Enabled = true;
                return;
            }
            simpleButton3.Enabled = false;
            gridView1.DeleteSelectedRows();
            secilenigetir(gridView1.FocusedRowHandle);
        }
        
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            vKullanicilar();
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            //int secilenrow = gvizinVerilenYetkiler.FocusedRowHandle;

            //if (secilenrow < 0) return;

            //string girilen =
            //    ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

            //gvizinVerilenYetkiler.BeginUpdate();

            //DataRow dr = gvizinVerilenYetkiler.GetDataRow(secilenrow);

            //if (girilen == "True")
            //    DB.ExecuteSQL("update YetkiAlanlari set Yetki=1 WHERE pkYetkiAlanlari=" + dr["pkYetkiAlanlari"].ToString());
            //else
            //    DB.ExecuteSQL("update YetkiAlanlari set Yetki=0 WHERE pkYetkiAlanlari=" + dr["pkYetkiAlanlari"].ToString());

            //gvizinVerilenYetkiler.EndUpdate();

        }

        private void btnRoller_Click(object sender, EventArgs e)
        {
            frmKullaniciRaporlari roller = new frmKullaniciRaporlari();
            roller.ShowDialog();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmFaturaZimmetTanim FaturaZimmetTanim = new frmFaturaZimmetTanim(int.Parse(dr["pkKullanicilar"].ToString()));
            FaturaZimmetTanim.Show();
        }


        private void repositoryItemTextEdit1_EditValueChanged(object sender, EventArgs e)
        {
            //int secilenrow = gvizinVerilenYetkiler.FocusedRowHandle;

            //if (secilenrow < 0) return;

            //string girilen =
            //    ((DevExpress.XtraEditors.TextEdit)((((DevExpress.XtraEditors.TextEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Text.ToString();

            //gvizinVerilenYetkiler.BeginUpdate();

            //DataRow dr = gvizinVerilenYetkiler.GetDataRow(secilenrow);

            ////if (girilen == "True")
            //  //  DB.ExecuteSQL("update YetkiAlanlari set Yetki=1 WHERE pkYetkiAlanlari=" + dr["pkYetkiAlanlari"].ToString());
            ////else
            //DB.ExecuteSQL("update YetkiAlanlari set Sayi=" + girilen+ " WHERE pkYetkiAlanlari=" + dr["pkYetkiAlanlari"].ToString());

            //gvizinVerilenYetkiler.EndUpdate();
        }

        private void repositoryItemTextEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //e.KeyCode
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
           DB.ExecuteSQL("update ModullerYetki set Yetki=1 where fkKullanicilar=" + fkKullanicilar.Text);
           KullaniciYetkileri();
        }

        private void tümYetkileriKaldırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("update ModullerYetki set Yetki=0 where fkKullanicilar=" + fkKullanicilar.Text);
            KullaniciYetkileri();
        }

        private void yetkileriOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModullerYetki(fkKullanicilar.Text);
            //DB.ExecuteScalarSQL("insert into ModullerYetki select Kod," + fkKullanicilar.Text + ",0 from Moduller");
            KullaniciYetkileri();
        }
        private void ModullerYetki(string fkKullanicilar)
        {
            DataTable dt = DB.GetData("select Kod from Moduller with(nolock)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DB.ExecuteSQL_Sonuc_Sifir("insert into ModullerYetki (Kod,fkKullanicilar,yetki) values('" + dt.Rows[i]["Kod"].ToString() +
                    "'," + fkKullanicilar + ",0)");
            }
        }
        private void buGrubaYetkiVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvizinVerilenYetkiler.FocusedRowHandle < 0) return;

            DataRow dr = gvizinVerilenYetkiler.GetDataRow(gvizinVerilenYetkiler.FocusedRowHandle);
            string kod = dr["Kod"].ToString();
            string[] aranan = kod.Split('.');
            foreach (string item in aranan)
            {
                kod=item.ToString();
                break;

            }
            DB.ExecuteSQL("update ModullerYetki set Yetki=1 where fkKullanicilar=" + fkKullanicilar.Text+
                " and Kod like '" + kod + "%'");
            KullaniciYetkileri();
        }

        private void buGrubunYetkileriniKaldırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvizinVerilenYetkiler.FocusedRowHandle < 0) return;

            DataRow dr = gvizinVerilenYetkiler.GetDataRow(gvizinVerilenYetkiler.FocusedRowHandle);
            string kod = dr["Kod"].ToString();
            string[] aranan = kod.Split('.');
            foreach (string item in aranan)
            {
                kod = item.ToString();
                break;

            }
            DB.ExecuteSQL("update ModullerYetki set Yetki=0 where fkKullanicilar=" + fkKullanicilar.Text +
                " and Kod like '" + kod + "%'");
            KullaniciYetkileri();
        }

        void YetkiAlanlariEkle()
        {
            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            DB.ExecuteSQL("insert into YetkiAlanlari" +
            " select fkParametreler," + fkKullanicilar.Text+ ",Yetki,1,Sayi,Aciklama10 from YetkiAlanlari where fkKullanicilar=1");
        }

        private void hızlıButonlarıOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YetkiAlanlariEkle();
        }

        private void lSube_Click(object sender, EventArgs e)
        {
            frmSubeler subeler = new frmSubeler();
            subeler.ShowDialog();

            Subeler();
        }

        private void labelControl14_Click(object sender, EventArgs e)
        {
            frmDepoKarti depolar = new frmDepoKarti();
            depolar.ShowDialog();

            Depolar();
        }

        private void lueSubeler_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnManuel_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtYedekYol2.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
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
    public partial class frmSiparisOlustur : DevExpress.XtraEditors.XtraForm
    {
        bool ilkyukleme = false;
        public frmSiparisOlustur()
        {
            InitializeComponent();
        }
        private void Getir()
        {
            string sql = @"SELECT CONVERT(bit,'1') as Sec,'' as Durumu,mzg.pkMusteriZiyaretGunleri, 
mzg.fkSablonGrup, mzg.fkGunler, mzg.GunSonra, mzg.fkPersoneller, mzg.fkSiparisSablonlari, 
mzg.fkSiparisSablonlari1, mzg.fkSiparisSablonlari2, mzg.fkSiparisSablonlari3,f.pkFirma, f.FirmaAdi, 
g.GunAdi, g.Kod,p.PersonelAdi,mzg.Durumu as DurumuMesaj,sfb.Baslik as SatisFiyatGrubu,f.fkSatisFiyatlariBaslik
FROM  MusteriZiyaretGunleri mzg with(nolock)
INNER JOIN (select pkFirma,FirmaAdi,fkSatisFiyatlariBaslik from Firmalar with(nolock)) f  ON mzg.fkFirma = f.pkFirma 
INNER JOIN Gunler g  with(nolock) ON mzg.fkGunler = g.pkGunler
INNER JOIN (select pkpersoneller,Adi+' '+Soyadi  as PersonelAdi from Personeller with(nolock)) p  ON  mzg.fkPersoneller=p.pkPersoneller
LEFT JOIN SatisFiyatlariBaslik sfb   with(nolock) ON sfb.pkSatisFiyatlariBaslik=f.fkSatisFiyatlariBaslik
WHERE mzg.VarYok = 1
and fkGunler=@fkGunler";// and fkSablonGrup=@fkSablonGrup";
            sql = sql.Replace("@fkSablonGrup", lueSablonGrup.EditValue.ToString());

            string fkGunler = TeslimTarihi.DateTime.DayOfWeek.ToString();
            if (fkGunler == "Friday") fkGunler = "6";
            if (fkGunler == "Saturday") fkGunler = "7";
            if (fkGunler == "Sunday") fkGunler = "1";
            if (fkGunler == "Monday") fkGunler = "2";
            if (fkGunler == "Tuesday") fkGunler = "3";
            if (fkGunler == "Wednesday") fkGunler = "4";
            if (fkGunler == "Thursday") fkGunler = "5";

            sql = sql.Replace("@fkGunler", fkGunler);
            if (lueSablonGrup.EditValue.ToString() == "1")//genel ise
            {
                fkSiparisSablonlari.Visible = true;
                fkSiparisSablonlari1.Visible = false;
                fkSiparisSablonlari2.Visible = false;
                fkSiparisSablonlari3.Visible = false;
                sql = sql + " and fkSiparisSablonlari is not null and fkSiparisSablonlari>0";
            }
            else if (lueSablonGrup.EditValue.ToString() == "2")//15 tatil
            {
                fkSiparisSablonlari.Visible = false;
                fkSiparisSablonlari1.Visible = true;
                fkSiparisSablonlari2.Visible = false;
                fkSiparisSablonlari3.Visible = false;
                sql = sql + " and fkSiparisSablonlari1 is not null and fkSiparisSablonlari1>0";
            }
            else if (lueSablonGrup.EditValue.ToString() == "3")//genel ise
            {
                fkSiparisSablonlari.Visible = false;
                fkSiparisSablonlari1.Visible = false;
                fkSiparisSablonlari2.Visible = true;
                fkSiparisSablonlari3.Visible = false;
                sql = sql + " and fkSiparisSablonlari2 is not null and fkSiparisSablonlari2>0";
            }
            else if (lueSablonGrup.EditValue.ToString() == "4")//15 tatil
            {
                fkSiparisSablonlari.Visible = false;
                fkSiparisSablonlari1.Visible = false;
                fkSiparisSablonlari2.Visible = false;
                fkSiparisSablonlari3.Visible = true;
                sql = sql + " and fkSiparisSablonlari3 is not null and fkSiparisSablonlari3>0";
            }

            gridControl3.DataSource= DB.GetData(sql);
            if (gridView3.DataRowCount == 0)
            {
                MessageBox.Show("Lütfen " + lueSablonGrup.Text+" Şablonu Tanımlayınız.");
            }
        }
        private void frmSiparisOlustur_Load(object sender, EventArgs e)
        {
            TeslimTarihi.DateTime = DateTime.Today.AddDays(1);
            lueSablonGrup.Properties.DataSource = DB.GetData("select * from SablonGrup");
            lueSablonGrup.EditValue = 1;
            ilkyukleme = true;
            Getir();
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult secimtum;
            secimtum = DevExpress.XtraEditors.XtraMessageBox.Show("Teslim Tarihi: "+ TeslimTarihi.Text + "\n"+
                "Dönem: "+ lueSablonGrup.Text + "\n\nSipariş Oluşturulacak Edilsin mi?", "Degerler.mesajbaslik", MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secimtum == DialogResult.No) return;
            progressBarControl1.Position = 0;
            progressBarControl1.Properties.Maximum = gridView3.DataRowCount;
            //int haftaningunu = (int)teslimtarihi.DayOfWeek;
            int olsi = 0;
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                Application.DoEvents();
                progressBarControl1.Position = i+1;
                DataRow dr = gridView3.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    string SiparisSablonlari = "0";
                    string fkSablonGrup = lueSablonGrup.EditValue.ToString();//dr["fkSablonGrup"].ToString();
                    if (fkSablonGrup == "1")
                        SiparisSablonlari = dr["fkSiparisSablonlari"].ToString();
                    else if (fkSablonGrup == "2")
                        SiparisSablonlari = dr["fkSiparisSablonlari1"].ToString();
                    else if (fkSablonGrup == "3")
                        SiparisSablonlari = dr["fkSiparisSablonlari2"].ToString();
                    else if (fkSablonGrup == "3")
                        SiparisSablonlari = dr["fkSiparisSablonlari3"].ToString();

                    if (SiparisSablonlari == "") SiparisSablonlari = "0";
                    DataTable dtSablon = DB.GetData("select fkFirma,fkPersoneller,OdemeSekli from SiparisSablonlari with(nolock) where pkSiparisSablonlari=" + SiparisSablonlari);
                    if (dtSablon.Rows.Count == 0)
                    {
                        DB.ExecuteSQL("update MusteriZiyaretGunleri SET Durumu='Lütfen Şablon Tanımlayınız' where pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());
                        continue;
                    }
                    if (dtSablon.Rows[0]["OdemeSekli"].ToString() == "")
                    {
                        DB.ExecuteSQL("update MusteriZiyaretGunleri SET Durumu='Odeme Şekli Boş Olamaz' where pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());
                        continue;
                    }

                    if (DB.GetData("select * from Satislar with(nolock) where fkFirma=" + dtSablon.Rows[0]["fkFirma"].ToString() + " and Convert(varchar(10),TeslimTarihi,112)='" + TeslimTarihi.DateTime.ToString("yyyyMMdd") + "'").Rows.Count > 0)
                    {
                        DB.ExecuteSQL("update MusteriZiyaretGunleri SET Durumu='Daha Önce Oluşturuldu' where pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());
                        continue;
                    }
                    //hata yoksa boşalt
                    DB.ExecuteSQL("update MusteriZiyaretGunleri SET Durumu='' where pkMusteriZiyaretGunleri=" + dr["pkMusteriZiyaretGunleri"].ToString());
                   
                    ArrayList list = new ArrayList();
                    string fkFirma=dtSablon.Rows[0]["fkFirma"].ToString();
                    list.Add(new SqlParameter("@fkFirma", fkFirma));
                    list.Add(new SqlParameter("@fkPerTeslimEden", dtSablon.Rows[0]["fkPersoneller"].ToString()));
                    list.Add(new SqlParameter("@TeslimTarihi", TeslimTarihi.DateTime.ToString("yyyy-MM-dd")));
                    list.Add(new SqlParameter("@OdemeSekli", dtSablon.Rows[0]["OdemeSekli"].ToString()));
                    list.Add(new SqlParameter("@Odenen", "0"));
                    list.Add(new SqlParameter("@AcikHesap", "0"));
                    list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", dr["fkSatisFiyatlariBaslik"].ToString()));
                    string pkSatislarYeniid = DB.ExecuteScalarSQL("INSERT INTO Satislar (Tarih,fkFirma,GelisNo,Siparis,fkKullanici,fkSatisDurumu,GuncellemeTarihi,fkPerTeslimEden,TeslimTarihi,OdemeSekli,Odenen,AcikHesap,fkSatisFiyatlariBaslik) " +
                        " values(getdate(),@fkFirma,0,0,1,10,getdate(),@fkPerTeslimEden,@TeslimTarihi,@OdemeSekli,@Odenen,@AcikHesap,@fkSatisFiyatlariBaslik) SELECT IDENT_CURRENT('Satislar')", list);
                    gridView3.SetRowCellValue(i, "Durum", "Oluşturuldu");
                    DataTable dtSablonDetay = DB.GetData("select * from SiparisSablonDetay where fkSiparisSablonlari=" + SiparisSablonlari);
                    for (int j = 0; j < dtSablonDetay.Rows.Count; j++)
                    {
                        string fkStokKarti = dtSablonDetay.Rows[j]["fkStokKarti"].ToString();
                        ArrayList list2 = new ArrayList();
                        list2.Add(new SqlParameter("@pkSatislarYeniid", pkSatislarYeniid));
                        list2.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                        list2.Add(new SqlParameter("@Adet", dtSablonDetay.Rows[j]["Adet"].ToString()));
                        DataTable dtStokkarti = DB.GetData("select pkStokKarti,AlisFiyati,SatisFiyati,KdvOrani from StokKarti where pkStokKarti=" + fkStokKarti);
                        string alisfiyati = "0", SatisFiyati = "0", KdvOrani = "0";
                        alisfiyati = dtStokkarti.Rows[0]["AlisFiyati"].ToString();
                        //DataTable dtSatisFiyati = DB.GetData("select dbo.fon_MusteriFiyatGrubuSec("+fkFirma+"," + fkStokKarti+")");
                        DataTable dtSatisFiyati = DB.GetData("select dbo.fon_SatisFiyatlariMusteriBazli(" + fkFirma + "," + fkStokKarti + ")");
                        SatisFiyati = dtSatisFiyati.Rows[0][0].ToString();
                        KdvOrani = dtStokkarti.Rows[0]["KdvOrani"].ToString();
                        list2.Add(new SqlParameter("@AlisFiyati", alisfiyati.Replace(",", ".")));
                        if (SatisFiyati == "0") SatisFiyati = "0";
                        list2.Add(new SqlParameter("@SatisFiyati", SatisFiyati.Replace(",", ".")));
                        list2.Add(new SqlParameter("@KdvOrani", KdvOrani));
                        list2.Add(new SqlParameter("@NakitFiyat", SatisFiyati.Replace(",", ".")));
                        DB.ExecuteSQL("INSERT INTO SatisDetay (fkSatislar,fkStokKarti,Adet,AlisFiyati,SatisFiyati,Tarih,Stogaisle,iskontotutar,iskontoyuzdetutar,KdvOrani,NakitFiyat,fkAlisDetay)" +
                        " values(@pkSatislarYeniid,@fkStokKarti,@Adet,@AlisFiyati,@SatisFiyati,getdate(),0,0,0,@KdvOrani,@NakitFiyat,0)", list2);
                    }
                    olsi++;
                }
            }
            //Nakit Güncelle
            DB.ExecuteSQL(@"update Satislar Set Odenen=SatisDetay.sf from
(select fkSatislar,sum(SatisDetay.Adet*SatisDetay.SatisFiyati-iskontotutar) as sf from SatisDetay group by fkSatislar) as SatisDetay
where Satislar.pkSatislar=SatisDetay.fkSatislar
and OdemeSekli='Nakit'");
            //Açık Hesap Güncelle
            DB.ExecuteSQL(@"update Satislar Set AcikHesap=SatisDetay.sf from
(select fkSatislar,sum(SatisDetay.Adet*SatisDetay.SatisFiyati-iskontotutar) as sf from SatisDetay group by fkSatislar) as SatisDetay
where Satislar.pkSatislar=SatisDetay.fkSatislar
and OdemeSekli='Açık Hesap'");
            MessageBox.Show(olsi.ToString() +" Sipariş Oluşturuldu.");
            Getir();
        }

        private void lueSablonGrup_EditValueChanged(object sender, EventArgs e)
        {
            if(ilkyukleme)
               Getir();
        }

        private void TeslimTarihi_EditValueChanged(object sender, EventArgs e)
        {
            if (ilkyukleme)
                Getir();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                gridView3.SetRowCellValue(i, "Sec", checkEdit1.Checked);
            }
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i=gridView3.FocusedRowHandle;
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
            Getir();
            gridView3.FocusedRowHandle = i;
        }

        private void müşteriBazlıSatışFişleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSatisFiyatlariMusteriBazli SatisFiyatlariMusteriBazli = new frmSatisFiyatlariMusteriBazli();
            SatisFiyatlariMusteriBazli.ShowDialog();
        }

        private void müşteriSiparişGünleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmSiparisSablonTanimlari SiparisSablonTanimlari = new frmSiparisSablonTanimlari();
            //SiparisSablonTanimlari.Satis1Firma.Tag = dr["pkFirma"].ToString()
            SiparisSablonTanimlari.ShowDialog();
        }

        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if(gridView3.FocusedRowHandle<0) return;
            DataRow dr =gridView3.GetDataRow(gridView3.FocusedRowHandle);
            gridControl1.DataSource=
                DB.GetData(@"select pkSatislar,fkFirma,Tarih,TeslimTarihi,fkSatisDurumu,
                Odenen,OdemeSekli,AcikHesap,GuncellemeTarihi,fkPerTeslimEden,
                NakitOdenen,OdenenKrediKarti from Satislar
                where Siparis=0 and fkFirma=" +dr["pkFirma"].ToString());
        }
        void SecilenSiparisiSil()
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DialogResult secimtum;
            secimtum = DevExpress.XtraEditors.XtraMessageBox.Show(dr["TeslimTarihi"].ToString() + " Sipariş Silinsin mi?", "Degerler.mesajbaslik", MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secimtum == DialogResult.No) return;

            DB.ExecuteSQL("delete from SatisDetay where fkSatislar=" + dr["pkSatislar"].ToString());
            DB.ExecuteSQL("delete from Satislar where pkSatislar=" + dr["pkSatislar"].ToString());
            gridView1.DeleteSelectedRows();
        }
        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                SecilenSiparisiSil();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            SecilenSiparisiSil();
        }

        private void btnTumunuSil_Click(object sender, EventArgs e)
        {
            
        }
    }
}
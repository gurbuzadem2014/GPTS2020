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
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmMusteriOtoTaksitAta : DevExpress.XtraEditors.XtraForm
    {
        public frmMusteriOtoTaksitAta()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        void MusteriGetir()
        {
            gridControl3.DataSource = DB.GetData("select convert(bit,1) as Sec,* from Firmalar with(nolock) where Aktif=1 and pkFirma>1");
        }

        private void frmMusteriOtoTaksitAta_Load(object sender, EventArgs e)
        {
            TeslimTarihi.DateTime = DateTime.Today;
            MusteriGetir();
            lueStoklar.Properties.DataSource = DB.GetData("select * from StokKarti with(nolock) where Aktif=1");
        }

        void TaksitveTaksitDetayEkle()
        {
            int say = 0, hatali=0;
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", dr["pkFirma"].ToString()));
                list.Add(new SqlParameter("@Tarih", TeslimTarihi.DateTime));

                decimal aidat_tutari = 0;
                decimal.TryParse(dr["AidatTutari"].ToString(), out aidat_tutari);

                list.Add(new SqlParameter("@Odenecek", aidat_tutari.ToString().Replace(",", ".")));
                list.Add(new SqlParameter("@Odenen", "0.0"));
                list.Add(new SqlParameter("@SiraNo", (i + 1).ToString()));
                list.Add(new SqlParameter("@HesabaGecisTarih", TeslimTarihi.DateTime));
                list.Add(new SqlParameter("@fkSatislar", "0"));
                list.Add(new SqlParameter("@Aciklama", txtAciklama.Text));

                string sonuc = DB.ExecuteSQL(" INSERT INTO Taksitler (fkFirma,Tarih,Odenecek,Odenen,SiraNo,HesabaGecisTarih,OdemeSekli,fkSatislar,Aciklama)" +
                    " VALUES(@fkFirma,@Tarih,@Odenecek,@Odenen,@SiraNo,@HesabaGecisTarih,'Taksit (Senet)',@fkSatislar,@Aciklama)", list);
                if (sonuc == "1")
                    say++;
                else
                    hatali++;
            }
            formislemleri.Mesajform(say.ToString()+" Adet Aidat(Taksit) Oluşturuldu", "K",200);
        }

        void KasaHareketiEkle()
        {
            int say = 0, hatali=0;
            DateTime HesabaGecisTarih = TeslimTarihi.DateTime;

            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);

                string sql = @"INSERT INTO KasaHareket (fkKasalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar)
                    values(@fkKasalar,0,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Aiadat Ödeme',@Tutar)";

                ArrayList list0 = new ArrayList();
                list0.Add(new SqlParameter("@fkKasalar", "1"));//int.Parse(lueKasa.EditValue.ToString())));

                decimal aidat_tutari = 0;
                decimal.TryParse(dr["AidatTutari"].ToString(), out aidat_tutari);

                string fkFirma = dr["pkFirma"].ToString();

                list0.Add(new SqlParameter("@Alacak", aidat_tutari.ToString().Replace(",", ".")));
                list0.Add(new SqlParameter("@Borc", "0"));
                list0.Add(new SqlParameter("@Tutar", "0"));

                list0.Add(new SqlParameter("@Tipi", (int)Degerler.KasaTipi.AidatGirisi));
                list0.Add(new SqlParameter("@Aciklama", txtAciklama.Text));
                list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
                list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
                list0.Add(new SqlParameter("@fkFirma", fkFirma));
                list0.Add(new SqlParameter("@AktifHesap", "0"));
                list0.Add(new SqlParameter("@Tarih", TeslimTarihi.DateTime));

                string sonuc = DB.ExecuteSQL(sql, list0);
                if (sonuc != "0")
                {
                    hatali++;
                    continue;
                }
                else
                    say++;
            }
            formislemleri.Mesajform(say.ToString() + " Adet Aidat(Kasa.H.) Oluşturuldu", "K",200);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(TeslimTarihi.DateTime.ToString("MMMM yyyy") +  " Dönemi \n Müşterilere Aiadat Tutarlarını Borçlandırmak İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;

            SatisFisiOlustur();
        }

        void SatisFisiOlustur()
        {
            if (gridView3.SelectedRowsCount == 0) return;
            if (lueStoklar.EditValue == null) return;

            int hatali = 0, hatasiz=0;

            string fkStokKarti= lueStoklar.EditValue.ToString();
            string sql = "";

            //local trans
            if (DB.conTrans == null)
                DB.conTrans = new SqlConnection(DB.ConnectionString());
            if (DB.conTrans.State == ConnectionState.Closed)
                DB.conTrans.Open();

            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                #region transları başlat
                //DBWeb.transaction = DBWeb.conTrans.BeginTransaction("AdemTransaction");
                DB.transaction = DB.conTrans.BeginTransaction("AidatTransaction");
                #endregion

                string v = gridView3.GetSelectedRows().GetValue(i).ToString();
                DataRow dr = gridView3.GetDataRow(int.Parse(v));
                //DataRow dr = gridView3.GetDataRow(i);
                string fkFirma = dr["pkFirma"].ToString();
                decimal aidat_tutari = 0;
                decimal.TryParse(dr["AidatTutari"].ToString(), out aidat_tutari);

                #region Fiş Oluştur
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@EskiFis", "0"));
                list.Add(new SqlParameter("@Tarih", TeslimTarihi.DateTime));
                list.Add(new SqlParameter("@fkFirma", fkFirma));//fkFirma)); ana bilgisayardaki pkFirma=Firma_id
                list.Add(new SqlParameter("@Siparis", "1"));
                list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
                list.Add(new SqlParameter("@fkSatisDurumu", "2"));
                if (txtAciklama.Text == "")
                    list.Add(new SqlParameter("@Aciklama", DBNull.Value));
                else
                    list.Add(new SqlParameter("@Aciklama", txtAciklama.Text));

                list.Add(new SqlParameter("@OdenenKrediKarti", "0"));
                list.Add(new SqlParameter("@CekTutar", "0"));
                list.Add(new SqlParameter("@AlinanPara","0"));
                list.Add(new SqlParameter("@ToplamTutar", aidat_tutari.ToString().Replace(",",".")));
                list.Add(new SqlParameter("@Odenen", "0"));
                list.Add(new SqlParameter("@AcikHesap", aidat_tutari.ToString().Replace(",", ".")));
                list.Add(new SqlParameter("@OdemeSekli", "Açık Hesap"));
                list.Add(new SqlParameter("@GuncellemeTarihi", TeslimTarihi.DateTime));
                list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", "1"));
                list.Add(new SqlParameter("@BonusOdenen", "0"));                
                list.Add(new SqlParameter("@OncekiBakiye", "0"));
                list.Add(new SqlParameter("@NakitOdenen", "0"));

                sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,fkKullanici,fkSatisDurumu,Aciklama,OdemeSekli,fkSatisFiyatlariBaslik,ToplamTutar,Odenen,AcikHesap,EskiFis,AlinanPara,BilgisayarAdi,GuncellemeTarihi,OdenenKrediKarti,CekTutar,BonusOdenen,OncekiBakiye,NakitOdenen,aktarildi)" +
               " values(@Tarih,@fkFirma,@Siparis,@fkKullanici,@fkSatisDurumu,@Aciklama,@OdemeSekli,@fkSatisFiyatlariBaslik,@ToplamTutar,@Odenen,@AcikHesap,@EskiFis,@AlinanPara,'Aktarım',@GuncellemeTarihi,@OdenenKrediKarti,@CekTutar,@BonusOdenen,@OncekiBakiye,@NakitOdenen,1) select IDENT_CURRENT('Satislar')";

                bool islembasarili = true;
                string yeni_pkSatislar = DB.ExecuteScalarSQLTrans(sql, list);

                if (yeni_pkSatislar.Substring(0, 1) == "H")
                {
                    islembasarili = false;
                    //listBoxControl1.Items.Add("Satislar Hata Fiş No: " + pkSatislar + " Hata:" + yeni_pkSatislar);
                }
                #endregion

                #region islembasarili ise Satış Detayları aktar
                if (islembasarili)
                {
                        ArrayList list2 = new ArrayList();
                        list2.Add(new SqlParameter("@fkSatislar", yeni_pkSatislar));
                        list2.Add(new SqlParameter("@fkStokKarti", fkStokKarti));//fkStokKarti));//ana pc deki stokkarti id =pkStokKarti
                        list2.Add(new SqlParameter("@Tarih", TeslimTarihi.DateTime));
                        list2.Add(new SqlParameter("@Adet","1"));
                        list2.Add(new SqlParameter("@AlisFiyati", "0"));
                        list2.Add(new SqlParameter("@SatisFiyati", aidat_tutari.ToString().Replace(",",".")));
                        list2.Add(new SqlParameter("@NakitFiyat", aidat_tutari.ToString().Replace(",", ".")));
                        list2.Add(new SqlParameter("@iade", "0"));
                        list2.Add(new SqlParameter("@KdvOrani", "0"));
                        //if (iskontotutar == "") iskontotutar = "0";
                        list2.Add(new SqlParameter("@iskontotutar", "0"));
                        //if (iskontoyuzdetutar == "") iskontoyuzdetutar = "0";
                        list2.Add(new SqlParameter("@iskontoyuzdetutar","0"));

                        sql = "INSERT INTO SatisDetay (fkSatislar,fkStokKarti,Tarih,Adet,AlisFiyati,SatisFiyati,NakitFiyat,iade,KdvOrani,iskontotutar,iskontoyuzdetutar,Faturaiskonto,isKdvHaric,GercekAdet) " +
                        " values(@fkSatislar,@fkStokKarti,@Tarih,@adet,@AlisFiyati,@SatisFiyati,@NakitFiyat,@iade,@KdvOrani,@iskontotutar,@iskontoyuzdetutar,0,0,1)";

                        string sonuc = DB.ExecuteSQLTrans(sql, list2);

                        if (sonuc.Substring(0, 1) == "H")
                        {
                            islembasarili = false;
                            //listBoxControl1.Items.Add("SatisDetay Hata Fiş No: " + pkSatislar + " Hata:" + sonuc);
                            //break;//ikinci döngünün içinden çıkması için islembasarili sonraki döngü hatasız ise hatalı olan aktarılmamış olur.
                        }

                    //}
                }
                #endregion

                #region Taksitler
                //private void TaksitlereBol()
            //{
            #region Taksit ekle 
            //taksit başlık bilgisi
            //if (taksit_id.Text == "0")
            //{
                ArrayList listt = new ArrayList();
                listt.Add(new SqlParameter("@fkFirma", fkFirma));
                listt.Add(new SqlParameter("@aciklama", txtAciklama.Text));
                listt.Add(new SqlParameter("@kefil", ""));
                listt.Add(new SqlParameter("@mahkeme", ""));
                listt.Add(new SqlParameter("@fkSatislar", yeni_pkSatislar));
                listt.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));

             sql = @"insert into Taksit(fkFirma,tarih,aciklama,kefil,mahkeme,fkSatislar,fkKullanici)
                    values(@fkFirma,getdate(),@aciklama,@kefil,@mahkeme,@fkSatislar,@fkKullanici) SELECT IDENT_CURRENT('Taksit')";

             string taksit_id = DB.ExecuteScalarSQLTrans(sql, listt);
                //taksit_id.Text = sonuc;
            //}
            
            #endregion

            //decimal ToplamOdenen = 0;

            //DateTime HesabaGecisTarih = TilkTaksitTarihi.DateTime;
            //DateTime gruplandir = DateTime.Now;
            //for (int i = 0; i < TAdet.Value; i++)
            {

                ArrayList list2 = new ArrayList();
                //list.Add(new SqlParameter("@fkFirma", teMusteri.Tag.ToString()));
                list2.Add(new SqlParameter("@Tarih", TeslimTarihi.DateTime));

                //if (checkEdit1.Checked && i == TAdet.Value - 1)
                //{
                    //list.Add(new SqlParameter("@Odenecek", (ToplamTutar.Value - ToplamOdenen).ToString().Replace(",", ".")));
                //}
                //else
                list2.Add(new SqlParameter("@Odenecek", aidat_tutari.ToString().Replace(",", ".")));

                list2.Add(new SqlParameter("@Odenen", "0"));
                list2.Add(new SqlParameter("@SiraNo", (i + 1).ToString()));
                list2.Add(new SqlParameter("@HesabaGecisTarih", TeslimTarihi.DateTime));
                list2.Add(new SqlParameter("@taksit_id", taksit_id));

                DB.ExecuteSQLTrans(" INSERT INTO Taksitler (Tarih,Odenecek,Odenen,SiraNo,HesabaGecisTarih,OdemeSekli,Kaydet,taksit_id)" +
                    " VALUES(@Tarih,@Odenecek,@Odenen,@SiraNo,@HesabaGecisTarih,'Taksit (Senet)',0,@taksit_id)", list2);

                //HesabaGecisTarih = HesabaGecisTarih.AddMonths(1);
                //ToplamOdenen = ToplamOdenen + ceTaksitTutari.Value;
            }

            //Taksitler();
            //taksit_id.Text = "0";
        //}
                #endregion

                #region trans. işlemi
                if (islembasarili)
                {
                    //local sunucu
                    DB.transaction.Commit();
                    hatasiz++;
                    //listBoxControl1.Items.Add("Fiş No: " + pkSatislar + " Başarılı" );
                }
                else
                {
                    //locak sunucuyu kapat
                    //if (DB.conTrans != null && DB.conTrans.State == ConnectionState.Open)
                    DB.transaction.Rollback();
                    hatali++;
                    //listBoxControl1.Items.Add("Hatalı Fiş No: " + pkSatislar + " Hatalı");
                }
                #endregion
            }
            DB.conTrans.Close();

            //formislemleri.Mesajform(say.ToString() + " Adet Aidat Fişi Oluşturuldu", "S", 200);

            MusteriGetir();
        }

        void Taksitler(string fkFirma)
        {
            string sql = @"SELECT convert(bit,'1') as Sec,fkSatislar,pkTaksitler, Tarih, Odenecek, Odenen, SiraNo, Aciklama, Odenecek - Odenen AS Kalan,
            OdemeSekli,HesabaGecisTarih,
            case when (Odenecek - Odenen)=0 then '0'
            when (Odenecek - Odenen)<0 then '1'
            when (Odenecek - Odenen)>0 and Tarih>GETDATE() then '2'
            when (Odenecek - Odenen)>0 and Tarih<GETDATE() then '3' 
            else '4' end as Durumu
            FROM  Taksitler with(nolock)";

            if(cbTumu.Checked==false)
               sql= sql+ " WHERE  fkFirma = " + fkFirma;

            gcTaksitler.DataSource = DB.GetData(sql);
        }

        void KasaHareketleri(string fkFirma)
        {
            string sql = @"SELECT convert(bit,'1') as Sec,kh.pkKasaHareket, kh.Tarih, kh.Aciklama, kh.fkFirma, kh.OdemeSekli,
                    kh.Alacak, kh.Borc, tur.Aciklama
                    FROM  KasaHareket kh with(nolock)
					LEFT JOIN KasaGirisCikisTurleri tur with(nolock) on tur.pkKasaGirisCikisTurleri=kh.fkKasaGirisCikisTurleri";
            if (checkEdit2.Checked == false)
					sql = sql+" where fkFirma=" + fkFirma;
            gcKasaHareketleri.DataSource = DB.GetData(sql);
        }

        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            Taksitler(dr["pkFirma"].ToString());
            KasaHareketleri(dr["pkFirma"].ToString());
            //gridControl1.DataSource=DB.GetData("select * from Taksitler with(nolock) where fkFirma=" + 
            //dr["pkFirma"].ToString());
        }
        
        private void aidatTutarıDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputForm sifregir = new inputForm();
            sifregir.Text = "Aidat Tutarı Düzenle";
            sifregir.GirilenCaption.Text = "Aiadat Tutarı Giriniz";
            sifregir.ShowDialog();
            decimal at=0;
            decimal.TryParse(sifregir.Girilen.Text, out at);
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);

                if (dr["Sec"].ToString()=="True")
                DB.ExecuteSQL("UPDATE Firmalar SET AidatTutari=" + at.ToString().Replace(",", ".") +
                    " WHERE pkFirma=" + dr["pkFirma"].ToString());    
            }            
            

            MusteriGetir();
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();

            MusteriGetir();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                gridView3.SetRowCellValue(i, "Sec", checkEdit1.Checked);
            }
        }

        private void cbTumu_CheckedChanged(object sender, EventArgs e)
        {
            Taksitler("0");
        }

        private void tümTaksitleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Taksit Bilgilerini Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                if (dr["Sec"].ToString() == "True")
                    DB.ExecuteSQL("delete from Taksitler where pkTaksitler=" + dr["pkTaksitler"].ToString());
            }
            Taksitler("0");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);


            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = dr["pkFirma"].ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            KasaHareketleri(dr["pkFirma"].ToString());
        }

        private void hareketiSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + dr["pkKasaHareket"].ToString();
                if (dr["Sec"].ToString() == "True")
                    DB.ExecuteSQL(sql);
            }
            DataRow drm = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            KasaHareketleri(drm["pkFirma"].ToString());
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            KasaHareketleri("0");
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            gcKasaHareketleri.Visible = checkEdit3.Checked;
        }

        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            gcTaksitler.Visible = checkEdit4.Checked;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (checkEdit4.Checked)
                checkEdit4.Checked = false;
            else
                checkEdit4.Checked = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (checkEdit3.Checked)
                checkEdit3.Checked = false;
            else
                checkEdit3.Checked = true;
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string fkFirma = dr["pkFirma"].ToString();

            frmMusteriHareketleri KurumKarti = new frmMusteriHareketleri();
            KurumKarti.musteriadi.Tag = fkFirma;
            KurumKarti.ShowDialog();

            MusteriGetir();
        }
    }
}
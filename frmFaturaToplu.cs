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
using System.IO;
using GPTS.Include.Data;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmFaturaToplu : DevExpress.XtraEditors.XtraForm
    {
        string pkFirma = "0";
        public frmFaturaToplu(string fkFirma)
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
            pkFirma = fkFirma;
        }

        private void frmFaturaToplu_Load(object sender, EventArgs e)
        {
            Temizle2();

            FirmaBilgileri2();

            MusteriSatisDetayGetir();

            FaturaKesilecekListe();

            SonFaturaNo();
            //btnFaturaNo_Click(sender,e);
        }

        private void FirmaBilgileri2()
        {
            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + pkFirma);
            if (dt.Rows.Count > 0)
            {
                lbMusteri.Text = dt.Rows[0]["OzelKod"].ToString() + "-" + dt.Rows[0]["Firmaadi"].ToString();
                lbMusteri.ToolTip = dt.Rows[0]["Tel"].ToString() +
                " Tel =" + dt.Rows[0]["Tel2"].ToString() + dt.Rows[0]["Cep"].ToString() +
                "Yetkili" + dt.Rows[0]["Yetkili"].ToString();

                memoEdit1.Text = dt.Rows[0]["Adres"].ToString();

                //decimal LimitBorc = 0;
                //decimal.TryParse(dt.Rows[0]["LimitBorc"].ToString(), out LimitBorc);

                textEdit4.Text = dt.Rows[0]["VergiDairesi"].ToString();
                textEdit3.Text = dt.Rows[0]["VergiNo"].ToString();
                textEdit2.Text = dt.Rows[0]["FaturaUnvani"].ToString();
            }

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }


        void FisYazdir3(bool Disigner, string RaporDosyasi, string fkFaturaToplu)
        {
            try
            {
                System.Data.DataSet ds = new DataSet("Test");

                string sql = "";
                sql = @"SELECT sd.fkStokKarti,
case when sd.isKdvHaric=1 then 
 ((sk.SatisFiyatiKdvHaric*sd.iskontoyuzdetutar)/100)
else
 ((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
end iskontotutar,

sd.iskontoyuzdetutar,

sk.pkStokKarti,sk.Stokadi,
sk.Barcode,
sk.Stoktipi,sk.StokKod,
sd.KdvOrani,
sk.Stoktipi as Birimi,
sum(sd.Adet) as Adet,
sd.NakitFiyat,
sd.SatisFiyati,
0 as koliicitanefiyati,

case when sd.isKdvHaric=1 then 
sum(sd.Adet*((sd.SatisFiyati*sd.iskontoyuzdetutar)/100))
else
sum(sd.Adet*((sd.SatisFiyati*sd.iskontoyuzdetutar)/100))
end  iskontotutaradet,

case when sd.isKdvHaric=1 then 
sum(sd.Adet*((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
+
((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)*sd.KdvOrani)/100))
else
sum(sd.Adet* (sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto))
end Tutar,

case when sd.isKdvHaric=1 then 
sum(((sd.SatisFiyati*sd.KdvOrani)/(100+sd.KdvOrani)))
else
sum(((sd.SatisFiyati-((sd.SatisFiyati*sd.KdvOrani)/(100+sd.KdvOrani)))*sd.KdvOrani)/100)
end KdvTutari,

case when sd.isKdvHaric=1 then 
sum(sd.Adet*(((sk.SatisFiyati-((sk.SatisFiyati*sd.iskontoyuzdetutar)/100))*sd.KdvOrani)/(100+sd.KdvOrani)))
else
sum(sd.Adet*((((sd.SatisFiyati-sd.iskontotutar)-(((sd.SatisFiyati-sd.iskontotutar)*sd.KdvOrani)/(100+sd.KdvOrani)))*sd.KdvOrani)/100))
 end KdvToplamTutari,
case when sd.isKdvHaric=1 then 
sd.SatisFiyati
else
sd.SatisFiyati-((sd.SatisFiyati*sd.KdvOrani)/(100+sd.KdvOrani))
end SatisFiyatiKdvHaric,

--sum((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto))/SUM(sd.Adet) as SatisFiyatiiskontolutane,
sum((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)) as SatisFiyatiiskontolu,
sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)) as FatuaToplami,
sum(sd.Faturaiskonto) as Faturaiskonto,
--kdvli iskontolu düşmüş tutar
sum(sd.Adet*((sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto)-
--iskontolu kdv düşmüş tutar
((sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto)*sd.KdvOrani)/(100+sd.KdvOrani)))
--*sd.KdvOrani)/(100+sd.KdvOrani))
as  toplamtutar_iskontolu,
-- satış fiyatı kdv haric
sum(sd.Adet*
((sd.SatisFiyati-((sd.SatisFiyati*sd.KdvOrani)/(100+sd.KdvOrani)))*sd.iskontoyuzdetutar)/100)--+sd.iskontoyuzdetutar)
as toplamiskonto_kdvsiz

FROM SatisDetay sd with(nolock)  
INNER JOIN (select pkStokKarti,StokKod,Stokadi,Barcode,Stoktipi,KdvOrani,Mevcut,SatisFiyati,SatisFiyatiKdvHaric from StokKarti sk with(nolock))sk  ON  sd.fkStokKarti = sk.pkStokKarti 
where sd.fkFaturaDurumu=1 and sd.fkFaturaToplu=@fkFaturaToplu 
group by sd.fkStokKarti,
sd.iskontoyuzdetutar,
sk.pkStokKarti,
sk.Stokadi,
sk.Barcode,
sk.Stoktipi,
sk.StokKod,
sd.isKdvHaric,
sd.KdvOrani,
sd.NakitFiyat,
sd.SatisFiyati,sk.SatisFiyatiKdvHaric";
//having sum(Adet)>0";

                sql = sql.Replace("@fkFaturaToplu", fkFaturaToplu); 

                 DataTable dtSatisDetay = DB.GetData(sql);

                 if (dtSatisDetay.Rows.Count==0)
                 {
                     MessageBox.Show("Yazdırılacak Stok Bulunamadı.");
                     return;
                 }

                 dtSatisDetay.TableName = "FisDetay";
                 ds.Tables.Add(dtSatisDetay);
                
//Firma Bilgileri
string sqlFis = @"SELECT 
ft.Tarih,
Tutar,
FaturaAdresi as Adres,
FaturaTarihi,
VergiDairesi,
VergiNo,
FaturaUnvani,
dbo.fon_MusteriBakiyesi(ft.fkFirma) as Bakiye,

case when sd.isKdvHaric=1 then 
sum(sd.Adet*((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
+
((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)*sd.KdvOrani)/100))
else
sum(sd.Adet* (sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto))
end Tutar,

case when sd.isKdvHaric=1 then 
dbo.fnc_ParayiYaziyaCevir(sum(sd.Adet*((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
+
((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)*sd.KdvOrani)/100)),2)
else
dbo.fnc_ParayiYaziyaCevir(sum(sd.Adet* (sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto)),2)
end rakamoku

FROM FaturaToplu  ft with(nolock) 
left join SatisDetay sd with(nolock) on sd.fkFaturaToplu=ft.pkFaturaToplu
WHERE pkFaturaToplu=@pkFaturaToplu
group by ft.fkFirma,ft.Tarih,Tutar,FaturaAdresi,FaturaTarihi,VergiDairesi,VergiNo,
FaturaUnvani,sd.isKdvHaric";

                sqlFis = sqlFis.Replace("@pkFaturaToplu", fkFaturaToplu);

                DataTable Fis = DB.GetData(sqlFis);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
               
                //Fatura Bilgileri
                DataTable FaturaBaslik = DB.GetData(@"SELECT * from  FaturaToplu ft with(nolock) where pkFaturaToplu=" + fkFaturaToplu);
                FaturaBaslik.TableName = "FaturaBaslik";
                ds.Tables.Add(FaturaBaslik);

                //Fatura Bilgileri
                DataTable FaturaFirma = DB.GetData(@"SELECT * from  Firmalar with(nolock) where pkFirma=" + pkFirma);
                FaturaFirma.TableName = "FaturaFirma";
                ds.Tables.Add(FaturaFirma);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "FaturaToplu";
                rapor.Report.Name = "FaturaToplu";

                if (Disigner)
                    rapor.ShowDesignerDialog();
                else
                    rapor.ShowPreviewDialog();//.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }


        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xTabFaturaiptal)
            {
                FaturaListesi();
            }
            else if (e.Page == xtraTabPage1)
            {
                FirmaBilgileri2();
                MusteriSatisDetayGetir();
            }
        }

        private void FaturaListesi()
        {
            string sql = "select CONVERT(bit,'1') as Sec,* from FaturaToplu with(nolock) where fkFirma=" + lbMusteri.Tag.ToString();
            //if (!cbTumunuGoster.Checked)
            //    sql = sql + " where Yazdirildi=0";
            //else
              //  sql = sql + " where Yazdirildi=1";

            gridControl3.DataSource = DB.GetData(sql);

            //if (lbMusteri.Tag != null)
            //{
            //    DevExpress.XtraGrid.Views.Base.ColumnView view = gridView4;
            //    view.ActiveFilter.Add(view.Columns["fkFirma"],
            //      new DevExpress.XtraGrid.Columns.ColumnFilterInfo("[fkFirma] Like '" + lbMusteri.Tag.ToString() + "'", ""));
            //}
        }

        void FaturaBilgileriniGetir2(string pkFaturaToplu, string FaturaNo)
        {
            string sql = "";
            if (pkFaturaToplu == "")
                sql = "select * from FaturaToplu where FaturaNo='" + FaturaNo + "'";
            else
                sql = "select * from FaturaToplu where pkFaturaToplu=" + pkFaturaToplu;

            DataTable dt = DB.GetData(sql);
            if (dt.Rows.Count == 0)
            {
                islemler.formislemleri.Mesajform("Fatura Bulunamadı", "K", 200);
                return;
            }
            textEdit2.Text = dt.Rows[0]["Aciklama"].ToString();
            memoEdit1.Text = dt.Rows[0]["FaturaAdresi"].ToString();
            deFaturaTarihi.Text = dt.Rows[0]["FaturaTarihi"].ToString();
            textEdit3.Text = dt.Rows[0]["VergiNo"].ToString();
            textEdit4.Text = dt.Rows[0]["VergiDairesi"].ToString();
            txtFaturaToplu_id.Text = dt.Rows[0]["FaturaNo"].ToString();
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            faturayıSilToolStripMenuItem_Click(sender, e);

            //if (gridView4.FocusedRowHandle < 0) return;

            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Faturayı Düzenlemek istediğinize eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.No) return;


            //DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            //txtFaturaToplu_id.Text = dr["pkFaturaToplu"].ToString();
            ////DB.ExecuteSQL("update SatisDetay set fkFaturaToplu=null,fkFaturaDurumu=0 where fkFaturaToplu" + txtFaturaNo.Tag.ToString());
            //FaturaBilgileriniGetir2(dr["pkFaturaToplu"].ToString(), "");

            //MusteriSatisDetayGetir();

            xtraTabControl1.SelectedTabPage=xtraTabPage1;
        }

        public bool KaydetGuncelle(string faturaToplu_id)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@FaturaNo", txtFaturaNo.Text));
            list.Add(new SqlParameter("@SeriNo", teSeriNo.Text));
            list.Add(new SqlParameter("@FaturaTarihi", deFaturaTarihi.DateTime));
            list.Add(new SqlParameter("@Aciklama", textEdit2.Text));
            list.Add(new SqlParameter("@FaturaAdresi", memoEdit1.Text));
            list.Add(new SqlParameter("@VergiDairesi", textEdit4.Text));
            list.Add(new SqlParameter("@VergiNo", textEdit3.Text));
            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            

            decimal aratop = 0;
            if (gridColumn82.SummaryItem.SummaryValue == null)
                aratop = 0;
            else
                aratop = decimal.Parse(gridColumn82.SummaryItem.SummaryValue.ToString());

            list.Add(new SqlParameter("@Tutar", aratop));
            list.Add(new SqlParameter("@Yazdirildi", "0"));
            list.Add(new SqlParameter("@fkFirma", lbMusteri.Tag.ToString()));

            if (faturaToplu_id == "0")
            {
                string yeni_pkFaturaToplu = DB.ExecuteScalarSQL(@"insert into FaturaToplu (fkFirma,SeriNo,FaturaNo,Tarih,FaturaTarihi,Aciklama,
                FaturaAdresi,VergiDairesi,VergiNo,Yazdirildi,Tutar,BilgisayarAdi)
                values(@fkFirma,@SeriNo,@FaturaNo,GETDATE(),@FaturaTarihi,@Aciklama,
                @FaturaAdresi,@VergiDairesi,@VergiNo,@Yazdirildi,@Tutar,@BilgisayarAdi)
                SELECT IDENT_CURRENT('FaturaToplu')", list);

                txtFaturaToplu_id.Text = yeni_pkFaturaToplu;

                DB.ExecuteSQL("update Kullanicilar set FaturaNo=FaturaNo+1 where pkKullanicilar=" + DB.fkKullanicilar);

                if (yeni_pkFaturaToplu == "")
                    return false;
            }
            else
            {
                list.Add(new SqlParameter("@pkFaturaToplu", faturaToplu_id));

                string sonuc = DB.ExecuteSQL(@"UPDATE FaturaToplu SET  fkFirma=@fkFirma,FaturaNo=@FaturaNo,FaturaTarihi=@FaturaTarihi,
                Aciklama=@Aciklama,FaturaAdresi=@FaturaAdresi,VergiDairesi=@VergiDairesi,VergiNo=@VergiNo,Yazdirildi=@Yazdirildi,
                Tutar=@Tutar where pkFaturaToplu=@pkFaturaToplu", list);

                if (sonuc != "0")
                    return false;
                //    islemler.formislemleri.Mesajform("Fatura Kaydedilirken SQL Hatası Oluştu.", "K");
            }

            return true;
        }

        private void btnYeniFatura_Click(object sender, EventArgs e)
        {
            Temizle2();

            //if (txtFaturaNo.Tag.ToString() == "0")
            //{
            //    DataTable dtKul = DB.GetData("select * from Kullanicilar with(nolock) where pkKullanicilar=" + 
            //        DB.fkKullanicilar);
            //    int faturano=0;
            //    int.TryParse(dtKul.Rows[0]["FaturaNo"].ToString(),out faturano);

            //    txtFaturaNo.Text = (faturano+1).ToString();

            //    deFaturaTarihi.DateTime = DateTime.Today;
            //    //teUnvani.Text = lbMusteri.Text;
            //    teFaturaAdresi.Text = "";
            //    teVergiDairesi.Text = "";
            //    teVergiNo.Text = "";

            //    FirmaBilgileri();
                
            //    //KaydetGuncelle();
            //}
            //gcSatisDetaylar.Focus();
        }

        //private void simpleButton2_Click(object sender, EventArgs e)
        //{
        //    if (!KaydetGuncelle(txtFaturaToplu_id.Text))
        //    {
        //        formislemleri.Mesajform("Hata Oluştur", "K", 150);
        //        return;
        //    }

        //    MusteriSatisDetayGetir();

        //    xtraTabControl1.SelectedTabPageIndex = 1;
        //}

        void Temizle2()
        {
            txtFaturaToplu_id.Text = "0";
            //textEdit1.Text = "";
            SonFaturaNo();
            deFaturaTarihi.DateTime = DateTime.Today;
            textEdit2.Text = "";
            memoEdit1.Text = "";
            textEdit4.Text = "";
            textEdit3.Text = "";
        }

        private void faturayıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Fatura İptal Edilecek Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                return;
            }

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string pkFaturaToplu = dr["pkFaturaToplu"].ToString();

            #region trans başlat
            if (DB.conTrans == null)
                DB.conTrans = new SqlConnection(DB.ConnectionString());
            if (DB.conTrans.State == ConnectionState.Closed)
                DB.conTrans.Open();
            DB.transaction = DB.conTrans.BeginTransaction("DepoTransferTransaction");
            bool islembasarili = true;
            #endregion

            string sonuc = DB.ExecuteSQLTrans("update satislar set Yazdir=0 where pkSatislar in(select fkSatislar from SatisDetay with(nolock) where fkFaturaToplu=" +
             pkFaturaToplu + ")");

            if (sonuc.Substring(0, 1) == "H")
                islembasarili = false;
            if (islembasarili)
                sonuc = DB.ExecuteSQLTrans("update SatisDetay set fkFaturaDurumu=0,fkFaturaToplu=null,fatura_durumu=null where fkFaturaToplu=" + pkFaturaToplu);

            if (islembasarili)
                sonuc = DB.ExecuteSQLTrans("Delete From FaturaToplu where pkFaturaToplu=" + pkFaturaToplu);

            #region trans. işlemi
            if (islembasarili)
            {
                DB.transaction.Commit();
            }
            else
            {
                DB.transaction.Rollback();
                formislemleri.Mesajform("Hata Oluştu : " + sonuc, "K", 150);
            }
            #endregion

            DB.conTrans.Close();

            FaturaListesi();

            //DB.ExecuteSQL("insert into BelgeTakip (fkFaturaToplu,fkKullanici,Tarih,fkBelgeDurumu,Aciklama) values(" + pkFaturaToplu + "," +
            //   DB.fkKullanicilar + ",getdate()," + (int)Degerler.BelgeDurumu.iptal + ",'Fatura Silindi')"); 

            gridControl1.DataSource = null;
        }

        private void lbMusteri_Click(object sender, EventArgs e)
        {
            DB.PkFirma = int.Parse(pkFirma);
            frmMusteriKarti KurumKarti = new frmMusteriKarti(pkFirma,"");
            KurumKarti.ShowDialog();

            FirmaBilgileri2();
        }

        void KesilenSatislar(string fkFaturaToplu)
        {
            string sql = @"select CONVERT(bit,isnull(fkFaturaDurumu,0)) as Sec,
            pkSatisDetay,
            s.pkSatislar,
            sd.Tarih,
            sk.Stokadi,sd.Adet,
            sd.SatisFiyati,
            sd.KdvOrani,
            sk.SatisFiyatiKdvHaric,
case when sd.isKdvHaric=1 then 
	  (sk.SatisFiyatiKdvHaric*sd.iskontoyuzdetutar)/100
	else
	  (sd.SatisFiyati*sd.iskontoyuzdetutar)/100
	end iskontotutar,

case when sd.isKdvHaric=1 then 
      sd.Adet*((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
    else
      sd.Adet*((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
    end  iskontotutaradet,

case when sd.isKdvHaric=1 then 
      sd.Adet*((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
      +
     ((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)*sd.KdvOrani)/100)
    else
     sd.Adet* (sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto)
    end Tutar,

case when sd.isKdvHaric=1 then 
     ((sd.SatisFiyati*sk.KdvOrani)/(100+sk.KdvOrani))
    else
     ((sd.SatisFiyati*sk.KdvOrani)/100) end KdvTutari,

case when sd.isKdvHaric=1 then 
    sd.Adet*(((sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100))*sd.KdvOrani)/(100+sd.KdvOrani))
    else
    sd.Adet*(((sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100))*sd.KdvOrani)/100) end KdvToplamTutari,

            sd.fkFaturaToplu from Satislar s with(nolock) 
            left join SatisDetay sd with(nolock) on s.pkSatislar=sd.fkSatislar
            left join StokKarti sk with(nolock) on sk.pkstokkarti=sd.fkStokKarti
            where s.Siparis=1 and sd.fkFaturaToplu=" + fkFaturaToplu;

//            string sql = @"select CONVERT(bit,stogaisle) as Sec,pkSatislar,fkFaturaToplu,
//            pkSatisDetay,sd.Adet,sk.Barcode,sk.Stokadi,sk.Mevcut,sk.MevcutFarki as FaturaliMevcut,
//            sd.SatisFiyati,sd.SatisFiyatiKdvHaric,isnull(sd.KdvOrani,sk.KdvOrani) as KdvOrani,
//            isnull(sdf.FaturaMiktari,sd.Adet) as FaturaMiktari,
//            isnull(sdf.FaturaFiyati,sd.SatisFiyati) as FaturaFiyati,
//            isnull(sdf.ToplamTutar,sd.SatisFiyati*sd.Adet) as ToplamTutar from Satislar s 
//            inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar 
//            inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
//            left join SatisDetayFatura sdf on sdf.fkSatisDetay=sd.pkSatisDetay
//            where fkFaturaToplu=@fkFaturaToplu";
            
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFaturaToplu", fkFaturaToplu));

            gridControl1.DataSource = DB.GetData(sql);//, list);
        }

        private void gridView4_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
           
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            KesilenSatislar(dr["pkFaturaToplu"].ToString());
        }

        private void faturaDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            string DosyaYol = DB.exeDizini + "\\Raporlar\\FaturaToplu.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("FaturaToplu.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir3(true, DosyaYol, dr["pkFaturaToplu"].ToString());

        }

        private void MusteriSatisDetayGetir()
        {
            string sql = @"select CONVERT(bit,isnull(fkFaturaDurumu,0)) as Sec,
            pkSatisDetay,
            s.pkSatislar,
            sd.Tarih,
            sk.StokAdi,
            sd.Adet,
            sd.fkStokKarti,
            sd.SatisFiyati,
            sd.KdvOrani,
            sk.SatisFiyatiKdvHaric,
case when sd.isKdvHaric=1 then 
	  (sk.SatisFiyatiKdvHaric*sd.iskontoyuzdetutar)/100
	else
	  (sd.SatisFiyati*sd.iskontoyuzdetutar)/100
	end iskontotutar,

case when sd.isKdvHaric=1 then 
      sd.Adet*((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
    else
      sd.Adet*((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
    end  iskontotutaradet,

case when sd.isKdvHaric=1 then 
      sd.Adet*((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
      +
     ((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)*sd.KdvOrani)/100)
    else
     sd.Adet* (sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto)
    end Tutar,

case when sd.isKdvHaric=1 then 
     ((sd.SatisFiyati*sk.KdvOrani)/(100+sk.KdvOrani))
    else
     ((sd.SatisFiyati*sk.KdvOrani)/100) end KdvTutari,

case when sd.isKdvHaric=1 then 
    sd.Adet*(((sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100))*sd.KdvOrani)/(100+sd.KdvOrani))
    else
    sd.Adet*(((sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100))*sd.KdvOrani)/100) end KdvToplamTutari,

            sd.fkFaturaToplu from Satislar s with(nolock) 
            left join SatisDetay sd with(nolock) on s.pkSatislar=sd.fkSatislar
            left join StokKarti sk with(nolock) on sk.pkstokkarti=sd.fkStokKarti
            where s.fkSatisDurumu not in(10,1,11) and s.Siparis=1 and s.fkFirma=" + lbMusteri.Tag.ToString();

            if (cbfaturatumu.Checked == false)
                sql = sql + " and isnull(sd.fkFaturaToplu,0)=0";

            gridControl2.DataSource = DB.GetData(sql);
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            MusteriSatisDetayGetir();
        }

        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            MusteriSatisDetayGetir();
            gcFaturaid.Visible = cbfaturatumu.Checked;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (gridView5.DataRowCount == 0)
            {
                MessageBox.Show("Yazdırılacak Stok Bulunamadı.");
                return;
            }
            //Önemli
            if (!KaydetGuncelle(txtFaturaToplu_id.Text))
            {
                formislemleri.Mesajform("Hata Oluştur", "K", 150);
                return;
            }

            string DosyaYol = DB.exeDizini + "\\Raporlar\\FaturaToplu.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("FaturaToplu.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }

            //SatisDetay fkFaturaDurumu ve fkFaturaToplu 
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);

                if (dr["Sec"].ToString() == "True")
                    DB.ExecuteSQL("update SatisDetay Set fatura_durumu=1,fkFaturaDurumu=1,fkFaturaToplu=" + txtFaturaToplu_id.Text + " where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            }

            FisYazdir3(false, DosyaYol, txtFaturaToplu_id.Text);

            string m = formislemleri.MesajBox("Fatura Yazdırıldı mı?", "Fatura Yazdırma Durumu", 3, 1);
            if (m == "0")
            {
                DB.ExecuteSQL("delete from FaturaToplu where pkFaturaToplu=" + txtFaturaToplu_id.Text);
                DB.ExecuteSQL("update SatisDetay Set fatura_durumu=null,fkFaturaToplu=null where fkFaturaToplu=" + txtFaturaToplu_id.Text);

                txtFaturaToplu_id.Text = "0";
                checkEdit3.Checked = false;

                MusteriSatisDetayGetir();
                FaturaKesilecekListe();
                return;
            }



            DB.ExecuteSQL("UPDATE FaturaToplu Set FaturaNo='" + txtFaturaNo.Text + "',Yazdirildi=1 where pkFaturaToplu=" + txtFaturaToplu_id.Text);

            MusteriSatisDetayGetir();

            FaturaKesilecekListe();

            txtFaturaToplu_id.Text = "0";

            SonFaturaNo();

            ceSatirSayisi.Checked = false;
        }

        private void YazdirSatisFaturasi(bool Disigner)
        {
            try
            {
                //#region Yazici Sec
                //string YaziciAdi = "", YaziciDosyasi = "";

                //DataTable dtYazicilar =
                //DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" + lueSatisTipi.EditValue.ToString());

                //if (dtYazicilar.Rows.Count == 1)
                //{
                //    YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                //    YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                //    short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
                //}
                //else if (dtYazicilar.Rows.Count > 1)
                //{
                //    short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                //    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1, int.Parse(lueSatisTipi.EditValue.ToString()));

                //    YaziciAyarlari.ShowDialog();

                //    YaziciAyarlari.Tag = 0;
                //    YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

                //    if (YaziciAyarlari.YaziciAdi.Tag == null)
                //        YaziciAdi = "";
                //    else
                //        YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                //    YaziciAyarlari.Dispose();
                //}

                //if (YaziciAdi == "")
                //{
                //    MessageBox.Show("Yazıcı Bulunamadı");
                //    return;
                //}
                //#endregion


                //string fisid = pkSatisBarkod.Text;
                //System.Data.DataSet ds = new DataSet("Fatura");
                //DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                //FisDetay.TableName = "FisDetay";
                //ds.Tables.Add(FisDetay);
                
                //DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                //Fis.TableName = "Fis";
                //ds.Tables.Add(Fis);
               
                //DataTable sirket = DB.GetData("select * from Sirketler with(nolock)");
                //sirket.TableName = "sirket";
                //ds.Tables.Add(sirket);

                ////Firma bilgileri
                //DataTable Musteri = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + Fis.Rows[0]["fkFirma"].ToString());
                //Musteri.TableName = "Musteri";
                //ds.Tables.Add(Musteri);
                //string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                //string RaporDosyasi = exedizini + "\\Raporlar\\" + YaziciDosyasi + ".repx";
                //if (!File.Exists(RaporDosyasi))
                //{
                //    MessageBox.Show("Dosya Bulunamadı");
                //    return;
                //}
                //xrCariHareket rapor = new xrCariHareket();
                //rapor.DataSource = ds;
                //rapor.LoadLayout(RaporDosyasi);
                //rapor.Name = "SatisFatura";
                //rapor.Report.Name = "SatisFatura";
                //if (Disigner)
                //    rapor.ShowDesigner();
                //else
                //    rapor.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void FaturaKesilecekListe()
        {
            string sql = @"SELECT CONVERT(bit,isnull(sd.fkFaturaDurumu,0)) as Sec,sd.fkStokKarti,
case when sd.isKdvHaric=1 then 
 sum((sk.SatisFiyatiKdvHaric*sd.iskontoyuzdetutar)/100)
else
 sum((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
end iskontotutar,

sd.iskontoyuzdetutar,

sk.pkStokKarti,sk.Stokadi,
sk.Barcode,
sk.Stoktipi,sk.StokKod,
sd.KdvOrani,
sk.Stoktipi as Birimi,
sum(sd.Adet) as Adet,
sd.SatisFiyati,

0 as koliicitanefiyati,

case when sd.isKdvHaric=1 then 
sum(sd.Adet*((sd.SatisFiyati*sd.iskontoyuzdetutar)/100))
else
sum(sd.Adet*((sd.SatisFiyati*sd.iskontoyuzdetutar)/100))
end  iskontotutaradet,

case when sd.isKdvHaric=1 then 
sum(sd.Adet*((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)
+
((sd.SatisFiyati- (sd.SatisFiyati*sd.iskontoyuzdetutar)/100)*sd.KdvOrani)/100))
else
sum(sd.Adet* (sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto))
end Tutar,

case when sd.isKdvHaric=1 then 
sum(((sd.SatisFiyati*sd.KdvOrani)/(100+sd.KdvOrani)))
else
sum(((sd.SatisFiyati-((sd.SatisFiyati*sd.KdvOrani)/(100+sd.KdvOrani)))*sd.KdvOrani)/100)
end KdvTutari,

case when sd.isKdvHaric=1 then 
sum(sd.Adet*(((sk.SatisFiyati-((sk.SatisFiyati*sd.iskontoyuzdetutar)/100))*sd.KdvOrani)/(100+sd.KdvOrani)))
else
sum(sd.Adet*((((sd.SatisFiyati-sd.iskontotutar)-(((sd.SatisFiyati-sd.iskontotutar)*sd.KdvOrani)/(100+sd.KdvOrani)))*sd.KdvOrani)/100))
 end KdvToplamTutari,
case when sd.isKdvHaric=1 then 
sum(sd.SatisFiyati)
else
sum(sd.SatisFiyati-((sd.SatisFiyati*sd.KdvOrani)/(100+sd.KdvOrani)))
end SatisFiyatiKdvHaric,

sum((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)) as SatisFiyatiiskontolu,

sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)) as FatuaToplami,

sum(sd.Faturaiskonto) as Faturaiskonto,
--kdvli iskontolu düşmüş tutar
sum(sd.Adet*((sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto)-
--iskontolu kdv düşmüş tutar
((sd.SatisFiyati-((sd.SatisFiyati*sd.iskontoyuzdetutar)/100)-sd.Faturaiskonto)*sd.KdvOrani)/(100+sd.KdvOrani)))
--*sd.KdvOrani)/(100+sd.KdvOrani))
as  toplamtutar_iskontolu,
-- satış fiyatı kdv haric
sum(sd.Adet*
((sd.SatisFiyati-((sd.SatisFiyati*sd.KdvOrani)/(100+sd.KdvOrani)))*sd.iskontoyuzdetutar)/100)--+sd.iskontoyuzdetutar)
as toplamiskonto_kdvsiz

FROM SatisDetay sd with(nolock)  
INNER JOIN (select pkStokKarti,StokKod,Stokadi,Barcode,Stoktipi,pkStokKartiid,KutuFiyat,KdvOrani,Mevcut,SatisFiyati,SatisFiyatiKdvHaric,KritikMiktar,sk.SonAlisTarihi,satis_iskonto,alis_iskonto from StokKarti sk with(nolock))sk  ON  sd.fkStokKarti = sk.pkStokKarti 
left join Satislar s with(nolock) on s.pkSatislar=sd.fkSatislar
where s.Siparis=1 and s.fkSatisDurumu not in(10,1,11) and sd.fkFaturaDurumu=1 and sd.fkFaturaToplu is null and s.fkFirma=@fkFirma
group by CONVERT(bit,isnull(fkFaturaDurumu,0)),
sd.fkStokKarti,sd.iskontoyuzdetutar,
sk.pkStokKarti,sk.Stokadi,
sk.Barcode,sk.Stoktipi,
sk.StokKod,
sd.isKdvHaric,sd.KdvOrani,sk.Stoktipi,sd.SatisFiyati";
//having sum(Adet)>0";

            sql = sql.Replace("@fkFirma", lbMusteri.Tag.ToString());

            gcFaturaOnizleme.DataSource = DB.GetData(sql);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            FaturaKesilecekListe();
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);

                if (checkEdit3.Checked == true)
                    DB.ExecuteSQL("update SatisDetay Set fkFaturaDurumu=1 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
                else
                    DB.ExecuteSQL("update SatisDetay Set fkFaturaDurumu=0 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            }

            MusteriSatisDetayGetir();

            FaturaKesilecekListe();
        }

        private void repositoryItemCheckEdit5_CheckedChanged(object sender, EventArgs e)
        {
            int secilenrow = gridView2.FocusedRowHandle;
            if (secilenrow < 0) return;

            string girilen =
                ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

            gridView2.BeginUpdate();

            DataRow dr = gridView2.GetDataRow(secilenrow);

            if (girilen == "True")
                DB.ExecuteSQL("UPDATE SatisDetay SET fkFaturaDurumu=1 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            else
                DB.ExecuteSQL("UPDATE SatisDetay SET fkFaturaDurumu=0 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());

            gridView2.EndUpdate();

            MusteriSatisDetayGetir();

            gridView2.FocusedRowHandle = secilenrow;

            FaturaKesilecekListe();

        }

        private void SonFaturaNo()
        {
            DataTable dtKul = DB.GetData("select * from Kullanicilar with(nolock) where pkKullanicilar=" +
                   DB.fkKullanicilar);

            teSeriNo.Text = dtKul.Rows[0]["FaturaSeriNo"].ToString();
            int faturano = 0;
            int.TryParse(dtKul.Rows[0]["FaturaNo"].ToString(), out faturano);

            txtFaturaNo.Text = (faturano + 1).ToString();
        }

        private void btnFaturaNo_Click(object sender, EventArgs e)
        {
            SonFaturaNo();
        }

        private void faturalanmadıYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int se = 0;
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                    se++;
            }
            if(se==0)
            {
            formislemleri.Mesajform("Stok Seçiniz", "K", 150);
                return;
            }

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                   DB.ExecuteSQL("update SatisDetay Set fkFaturaToplu=null where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            }
            MusteriSatisDetayGetir();
        }

        private void faturalandıYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int se = 0;
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                    se++;
            }
            if (se == 0)
            {
                formislemleri.Mesajform("Stok Seçiniz", "K", 150);
                return;
            }

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                   DB.ExecuteSQL("update SatisDetay Set fkFaturaToplu=-1 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            }
            MusteriSatisDetayGetir();
        }

        private void gridView2_EndSorting(object sender, EventArgs e)
        {
            if (gridView2.DataRowCount > 0)
                gridView2.FocusedRowHandle = 0;
        }

        private void buFişdekiÜrünleriSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;

            if (i < 0) return;
            
            DataRow dr = gridView2.GetDataRow(i);

            DB.ExecuteSQL("update SatisDetay Set fkFaturaDurumu=1 where fkFaturaToplu is null and fkSatislar=" + dr["pkSatislar"].ToString());

            MusteriSatisDetayGetir();

            FaturaKesilecekListe();

            gridView2.FocusedRowHandle = i;
        }

        private void ceSatirSayisi_CheckedChanged(object sender, EventArgs e)
        {
            int s = int.Parse(seSatirSayisi.Value.ToString());
            if(gridView2.DataRowCount<s)
                s = gridView2.DataRowCount;

            for (int i = 0; i < s; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);

                if (ceSatirSayisi.Checked == true)
                    DB.ExecuteSQL("update SatisDetay Set fkFaturaDurumu=1 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
                else
                    DB.ExecuteSQL("update SatisDetay Set fkFaturaDurumu=0 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            }

            MusteriSatisDetayGetir();

            FaturaKesilecekListe();
        }

        private void faturaÖnİzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            string DosyaYol = DB.exeDizini + "\\Raporlar\\FaturaToplu.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("FaturaToplu.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir3(false, DosyaYol, dr["pkFaturaToplu"].ToString());
        }

        private void fişBilgisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView2.GetDataRow(i);
            string pkSatislar = dr["pkSatislar"].ToString();

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(true);
            FisNoBilgisi.fisno.EditValue = pkSatislar;
            FisNoBilgisi.ShowDialog();

            gridView2.FocusedRowHandle = i;
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUrunBazinda.Checked)
                gridColumn22.Group();
            else
                gridColumn22.UnGroup();
        }

        //private void gridView5_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        //{
        //    if (gridColumn3.SummaryItem.SummaryValue != null)
        //         textEdit5.Text = gridColumn3.SummaryItem.SummaryValue.ToString();

        //    if (gridColumn78.SummaryItem.SummaryValue != null)
        //        textEdit6.Text = gridColumn78.SummaryItem.SummaryValue.ToString();
        //}

        private void gridView5_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (gridColumn3.SummaryItem.SummaryValue != null)
                textEdit5.Text = gridColumn3.SummaryText;
                    //.SummaryItem.SummaryValue.ToString();

            if (gridColumn78.SummaryItem.SummaryValue != null)
                textEdit6.Text = gridColumn78.SummaryText;
                    //.SummaryItem.SummaryValue.ToString();
        }

        private void buÜrünleriSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView2.GetDataRow(i);

            DB.ExecuteSQL("update SatisDetay Set fkFaturaDurumu=1 where fkFaturaToplu is null and fkStokKarti=" + dr["fkStokKarti"].ToString());

            MusteriSatisDetayGetir();

            FaturaKesilecekListe();

            gridView2.FocusedRowHandle = i;
        }

        private void faturaBilgileriDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string pkFaturaToplu = dr["pkFaturaToplu"].ToString();

            frmFaturaTopluDuzelt FaturaTopluDuzelt = new frmFaturaTopluDuzelt();
            FaturaTopluDuzelt.pkFaturaToplu.Text = pkFaturaToplu;
            FaturaTopluDuzelt.ShowDialog();

            FaturaListesi();
        }
    }
}
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
using System.IO;
using GPTS.islemler;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;

namespace GPTS
{
    public partial class frmSiparisHizli : DevExpress.XtraEditors.XtraForm
    {
        public frmSiparisHizli()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmKontaklar_Load(object sender, EventArgs e)
        {
            gridView1.OptionsNavigation.EnterMoveNextColumn = true;
            TeslimTarihi.DateTime = DateTime.Today;

            AcikSiparisleriGetir();
            PersonelGetir();
            SiparisGetir();

            //SiparisStoklariBaslik();
            SiparisStoklari();

            SablonlarGetir();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        void SiparisGetir()
        {
            if (lueAcikSiparisler.EditValue == null) lueAcikSiparisler.EditValue = 0;
            if (lueAcikSiparisler.Text == "") lueAcikSiparisler.EditValue = 0;

            string sql = @"select * from SiparisDetay sd with(nolock)
            left join Firmalar f with(nolock) on f.pkFirma=sd.fkFirma
            where sd.fkSiparis=" + lueAcikSiparisler.EditValue.ToString();

            gridControl1.DataSource = DB.GetData(sql);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            string sec = formislemleri.MesajBox("Siparişler Satışa Çevrilecektir. Eminmisiniz?", "Satışa Çevir", 3, 0);
            if (sec != "1") return;

            if(lueAcikSiparisler.EditValue==null) return;

            string pkSiparis= lueAcikSiparisler.EditValue.ToString();

            DataTable dtSiparis = DB.GetData("select * from Siparis with(nolock) where pkSiparis=" + pkSiparis);

            string fkPerTeslimEden = dtSiparis.Rows[0]["fkPersoneller"].ToString();
            DateTime TeklifTarihi = Convert.ToDateTime(dtSiparis.Rows[0]["siparis_tarihi"].ToString());

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string pkFirma = dr["fkFirma"].ToString();
                string fkStokKarti1 = dr["fkStokKarti1"].ToString();
                string fkStokKarti2 = dr["fkStokKarti2"].ToString();
                string fkStokKarti3 = dr["fkStokKarti3"].ToString();
                string fkStokKarti4 = dr["fkStokKarti4"].ToString();
                string fkStokKarti5 = dr["fkStokKarti5"].ToString();
                string Adet1 = dr["Adet1"].ToString();
                string Adet2 = dr["Adet2"].ToString();
                string Adet3 = dr["Adet3"].ToString();
                string Adet4 = dr["Adet4"].ToString();
                string Adet5 = dr["Adet5"].ToString();
                decimal devir = 0;
                decimal.TryParse(dr["Devir"].ToString(),out devir);
                decimal alinan_para = 0;
                decimal.TryParse(dr["alinan_para"].ToString(),out alinan_para);

                if (Adet1 == "" && Adet2 == "" && Adet3 == "" && Adet4 == "" && Adet5 == "" && alinan_para == 0) continue;

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", pkFirma));
                list.Add(new SqlParameter("@Siparis", "1"));
                list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
                list.Add(new SqlParameter("@fkSatisDurumu", "2"));
                list.Add(new SqlParameter("@Aciklama", "Hızlı Sipariş"));
                list.Add(new SqlParameter("@AlinanPara", "0"));
                list.Add(new SqlParameter("@ToplamTutar", "0"));
                list.Add(new SqlParameter("@fkPerTeslimEden", fkPerTeslimEden));
                list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));
                list.Add(new SqlParameter("@OdemeSekli", "Açık Hesap"));
                list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                string sf = "1";
                list.Add(new SqlParameter("@fkSatisFiyatlariBaslik", sf));
                list.Add(new SqlParameter("@TeklifTarihi", TeklifTarihi));
                list.Add(new SqlParameter("@GuncellemeTarihi", TeklifTarihi));
                list.Add(new SqlParameter("@EskiFis", pkSiparis));
                
                string sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,fkPerTeslimEden,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen,OdemeSekli,SonislemTarihi,BilgisayarAdi,fkSatisFiyatlariBaslik,TeklifTarihi,GuncellemeTarihi,EskiFis)" +
                    " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@fkPerTeslimEden,@iskontoFaturaTutar,0,0,@OdemeSekli,getdate(),@BilgisayarAdi,@fkSatisFiyatlariBaslik,@TeklifTarihi,@GuncellemeTarihi,@EskiFis) SELECT IDENT_CURRENT('Satislar')";


                string fisno = "";
                //if (!string.IsNullOrEmpty(fkStokKarti1) && !string.IsNullOrEmpty(Adet1) && !string.IsNullOrEmpty(Adet2)
                //    && !string.IsNullOrEmpty(Adet3) && !string.IsNullOrEmpty(Adet4) && !string.IsNullOrEmpty(Adet5))
                //{
                //    formislemleri.Mesajform("Sipariş Bulunamadı","K");
                //    return;
                //}
                
                fisno = DB.ExecuteScalarSQL(sql, list);

                if (fisno.Substring(0, 1) == "H")
                {
                    MessageBox.Show("Hata oluştu " + fisno);
                    return;
                }
                #region Satış Detay
                DataTable dtFirma = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + pkFirma);
                string fkSatisFiyatlariBaslik = dtFirma.Rows[0]["fkSatisFiyatlariBaslik"].ToString();

                if (!string.IsNullOrEmpty(fkStokKarti1) && !string.IsNullOrEmpty(Adet1))
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(new SqlParameter("@fkSatislar", fisno));
                    arr.Add(new SqlParameter("@SatisFiyatGrubu", fkSatisFiyatlariBaslik));
                    arr.Add(new SqlParameter("@Adet", Adet1.Replace(",", ".")));
                    arr.Add(new SqlParameter("@fkStokKarti", fkStokKarti1));
                    string s = DB.ExecuteScalarSQL("exec sp_SatisDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti", arr);
                    if (s != "Satis Detay Eklendi.")
                    {
                        MessageBox.Show(s);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(fkStokKarti2) && !string.IsNullOrEmpty(Adet2))
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(new SqlParameter("@fkSatislar", fisno));
                    arr.Add(new SqlParameter("@SatisFiyatGrubu", fkSatisFiyatlariBaslik));
                    arr.Add(new SqlParameter("@Adet", Adet2.Replace(",", ".")));
                    arr.Add(new SqlParameter("@fkStokKarti", fkStokKarti2));
                    string s = DB.ExecuteScalarSQL("exec sp_SatisDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti", arr);
                    if (s != "Satis Detay Eklendi.")
                    {
                        MessageBox.Show(s);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(fkStokKarti3) && !string.IsNullOrEmpty(Adet3))
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(new SqlParameter("@fkSatislar", fisno));
                    arr.Add(new SqlParameter("@SatisFiyatGrubu", fkSatisFiyatlariBaslik));
                    arr.Add(new SqlParameter("@Adet", Adet3.Replace(",", ".")));
                    arr.Add(new SqlParameter("@fkStokKarti", fkStokKarti3));
                    string s = DB.ExecuteScalarSQL("exec sp_SatisDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti", arr);
                    if (s != "Satis Detay Eklendi.")
                    {
                        MessageBox.Show(s);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(fkStokKarti4) && !string.IsNullOrEmpty(Adet4))
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(new SqlParameter("@fkSatislar", fisno));
                    arr.Add(new SqlParameter("@SatisFiyatGrubu", fkSatisFiyatlariBaslik));
                    arr.Add(new SqlParameter("@Adet", Adet4.Replace(",", ".")));
                    arr.Add(new SqlParameter("@fkStokKarti", fkStokKarti4));
                    string s = DB.ExecuteScalarSQL("exec sp_SatisDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti", arr);
                    if (s != "Satis Detay Eklendi.")
                    {
                        MessageBox.Show(s);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(fkStokKarti5) && !string.IsNullOrEmpty(Adet5))
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(new SqlParameter("@fkSatislar", fisno));
                    arr.Add(new SqlParameter("@SatisFiyatGrubu", fkSatisFiyatlariBaslik));
                    arr.Add(new SqlParameter("@Adet", Adet5.Replace(",", ".")));
                    arr.Add(new SqlParameter("@fkStokKarti", fkStokKarti5));
                    string s = DB.ExecuteScalarSQL("exec sp_SatisDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti", arr);
                    if (s != "Satis Detay Eklendi.")
                    {
                        MessageBox.Show(s);
                        return;
                    }
                }
                #endregion

                #region KasaHareket
                if (alinan_para>0)
                {
                    string sql_kasa = "INSERT INTO KasaHareket (fkKasalar,Tarih,fkFirma,Aciklama,AktifHesap,OdemeSekli,Borc,Tutar)" +
                        " values(1,getdate(),@fkFirma,'Sipariş Ödemesi',1,'Nakit',@Borc,@Tutar)";
                    sql_kasa = sql_kasa.Replace("@fkFirma", pkFirma);
                    //sql_kasa = sql_kasa.Replace("@fkSatislar", fisno);
                    sql_kasa = sql_kasa.Replace("@Borc", alinan_para.ToString().Replace(",", "."));
                    sql_kasa = sql_kasa.Replace("@Tutar", devir.ToString().Replace(",", "."));

                    int sonuc = DB.ExecuteSQL(sql_kasa);
                }
                #endregion

                sql= @"update Satislar set ToplamTutar=
(select SUM(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as Alacak from Satislar s with(nolock)
inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
where s.pkSatislar=@pkSatislar) 
where pkSatislar=@pkSatislar";
                sql = sql.Replace("@pkSatislar", fisno);
                DB.ExecuteSQL(sql);

                DB.ExecuteSQL("update Siparis set kaydedildi=1 where pkSiparis=" + pkSiparis);
            }


            formislemleri.Mesajform("Satışa Çevrildi", "S", 200);

            AcikSiparisleriGetir();
            SiparisGetir();

            SablonlarGetir();
        }

        void AcikSiparisleriGetir()
        {
            lueAcikSiparisler.Properties.DataSource = DB.GetData(@"select s.pkSiparis,p.adi,p.soyadi,s.siparis_tarihi from Siparis s with(nolock)
            left join Personeller p with(nolock) on p.pkpersoneller=s.fkpersoneller
            where isnull(s.kaydedildi,0)=0");

            lueAcikSiparisler.Properties.ValueMember = "pkSiparis";
            lueAcikSiparisler.Properties.DisplayMember = "adi";
            lueAcikSiparisler.ItemIndex = 1;
        }

        void SablonlarGetir()
        {
            lueSablonlar.Properties.DataSource = DB.GetData(@"select s.pkSiparis,p.adi,p.soyadi,s.siparis_tarihi,s.aciklama from Siparis s with(nolock)
            left join Personeller p with(nolock) on p.pkpersoneller=s.fkpersoneller
            where isnull(s.sablon,0)=1");

            lueSablonlar.Properties.ValueMember = "pkSiparis";
            lueSablonlar.Properties.DisplayMember = "aciklama";
            //lookUpEdit1.EditValue = 1;
        }
        void PersonelGetir()
        {
            lUEPersonel.Properties.DataSource = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller where Plasiyer=1 and AyrilisTarihi is null");
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";
            lUEPersonel.EditValue = 1;
        }

        void SiparisStoklariBaslik()
        {
            DataTable dt = DB.GetData(@"select ss.pkSiparisStoklari,ss.fkStokKarti,ss.Aktif,sk.Stokadi from SiparisStoklari ss with(nolock)
            left join StokKarti sk with(nolock) on ss.fkStokKarti=sk.pkStokKarti order by ss.Sira");

            //gcStokKartı1.Visible = false;
            //gcStokKartı2.Visible = false;
            //gcStokKartı3.Visible = false;
            //gcStokKartı4.Visible = false;
            //gcStokKartı5.Visible = false;
            //gcStokKartı6.Visible = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    gcStokKartı1.Caption = dt.Rows[i]["Stokadi"].ToString();
                    gcStokKartı1.Tag = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı1.ToolTip = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı1.Visible = true;
                    gcStokKartı1.VisibleIndex = 2;
                }
                else if (i == 1)
                {
                    gcStokKartı2.Caption = dt.Rows[i]["Stokadi"].ToString();
                    gcStokKartı2.Tag = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı2.ToolTip = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı2.Visible = true;
                    gcStokKartı2.VisibleIndex = 3;
                }
                else if (i == 2)
                {
                    gcStokKartı3.Caption = dt.Rows[i]["Stokadi"].ToString();
                    gcStokKartı3.Tag = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı3.ToolTip = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı3.Visible = true;
                    gcStokKartı3.VisibleIndex = 4;
                }
                else if (i == 3)
                {
                    gcStokKartı4.Caption = dt.Rows[i]["Stokadi"].ToString();
                    gcStokKartı4.Tag = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı4.ToolTip = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı4.Visible = true;
                    gcStokKartı4.VisibleIndex = 5;
                }
                else if (i == 4)
                {
                    gcStokKartı5.Caption = dt.Rows[i]["Stokadi"].ToString();
                    gcStokKartı5.Tag = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı5.ToolTip = dt.Rows[i]["fkStokKarti"].ToString();
                    gcStokKartı5.Visible = true;
                    gcStokKartı5.VisibleIndex = 6;

                }
                //else if (i == 5)
                //{
                //    gcStokKartı6.Caption = dt.Rows[i]["Stokadi"].ToString();
                //    gcStokKartı6.Tag = dt.Rows[i]["fkStokKarti"].ToString();
                //    gcStokKartı6.ToolTip = dt.Rows[i]["fkStokKarti"].ToString();
                //    gcStokKartı1.Visible = true;
                //}
             }
        }

        void SiparisStoklari()
        {
            gridControl2.DataSource = DB.GetData(@"select ss.pkSiparisStoklari,ss.fkStokKarti,ss.Aktif,sk.Stokadi,ss.Sira from SiparisStoklari ss with(nolock)
            left join StokKarti sk with(nolock) on ss.fkStokKarti=sk.pkStokKarti");
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma=dr["pkFirma"].ToString();

            frmMusteriKarti KurumKarti = new frmMusteriKarti(pkFirma, "");
            KurumKarti.ShowDialog();

            //int secilen = gridView1.FocusedRowHandle;

            //gridrowgetir(secilen);

            SiparisGetir();

            //gridView1.FocusedRowHandle = secilen;
        }

        private void sbtnOlustur_Click(object sender, EventArgs e)
        {
            frmYukleniyor yukleniyor = new frmYukleniyor();
            yukleniyor.Text = "Siparişler Oluşturuluyor";
            yukleniyor.labelControl1.Text = "Lütfen Bekleyiniz...";
            yukleniyor.TopMost = true;
            yukleniyor.Show();

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
            list.Add(new SqlParameter("@siparis_tarihi", TeslimTarihi.DateTime));
            list.Add(new SqlParameter("@fkpersoneller", lUEPersonel.EditValue.ToString()));

            string  pkSiparis=
            DB.ExecuteScalarSQL("INSERT INTO Siparis (tarih,fkKullanicilar,fkpersoneller,siparis_tarihi,kaydedildi,olusturuldu)" +
                " values(getdate(),@fkKullanicilar,@fkpersoneller,@siparis_tarihi,0,0) select IDENT_CURRENT('Siparis')", list);

            #region Satisdetay ekle
            DataTable dtStok =  DB.GetData("select * from SiparisStoklari with(nolock) where Aktif=1 order by sira");
            if (dtStok.Rows.Count == 0)
            {
                MessageBox.Show("SiparisStoklari Bulunamadı");
                return;

            }
            string fkStokKarti1 = "0", fkStokKarti2 = "0", fkStokKarti3 = "0", fkStokKarti4 = "0", fkStokKarti5 = "0";

            for (int i = 0; i < dtStok.Rows.Count; i++)
			{
                if (i == 0)
                {
                    fkStokKarti1 = dtStok.Rows[i]["fkStokKarti"].ToString();
                }
                if (i == 1)
                {
                    fkStokKarti2 = dtStok.Rows[i]["fkStokKarti"].ToString();
                }
                if (i == 2)
                {
                    fkStokKarti3 = dtStok.Rows[i]["fkStokKarti"].ToString();
                }
                if (i == 3)
                {
                    fkStokKarti4 = dtStok.Rows[i]["fkStokKarti"].ToString();
                }
                if (i == 4)
                {
                    fkStokKarti5 = dtStok.Rows[i]["fkStokKarti"].ToString();
                }
			}

            DataTable dtFirma = DB.GetData("select * from Firmalar with(nolock) where Aktif=1 and fkPerTeslimEden=" + lUEPersonel.EditValue.ToString());
            //string pkSiparis = lookUpEdit1.EditValue.ToString();
            for (int i = 0; i < dtFirma.Rows.Count; i++)
            {
                string fkFirma = dtFirma.Rows[i]["pkFirma"].ToString();
                string sql = "insert into SiparisDetay (fkSiparis,fkFirma,fkStokKarti1,fkStokKarti2,fkStokKarti3,fkStokKarti4,fkStokKarti5)  values("
                    + pkSiparis + "," + fkFirma + "," + fkStokKarti1 + "," + fkStokKarti2 + "," + fkStokKarti3 + "," + fkStokKarti4 + "," + fkStokKarti5 + ")";
                DB.ExecuteSQL(sql);    
            }
            

            #endregion

            SiparisStoklariBaslik();

            AcikSiparisleriGetir();

            yukleniyor.Close();

            formislemleri.Mesajform("Siparişler Oluşturuldu.", "S", 200);

            gridView1.Focus();
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            SiparisGetir();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);

                DB.ExecuteSQL("update SiparisStoklari set Sira=" + dr["Sira"].ToString() +
                      " where pkSiparisStoklari=" + dr["pkSiparisStoklari"].ToString());
                
            }
            SiparisStoklari();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                string pkSiparisDetay = dr["pkSiparisDetay"].ToString();

                string pkFirma = dr["fkFirma"].ToString();

                string fkStokKarti1 = dr["fkStokKarti1"].ToString();
                string fkStokKarti2 = dr["fkStokKarti2"].ToString();
                string fkStokKarti3 = dr["fkStokKarti3"].ToString();
                string fkStokKarti4 = dr["fkStokKarti4"].ToString();
                string fkStokKarti5 = dr["fkStokKarti5"].ToString();
                string Adet1 = dr["Adet1"].ToString();
                string Adet2 = dr["Adet2"].ToString();
                string Adet3 = dr["Adet3"].ToString();
                string Adet4 = dr["Adet4"].ToString();
                string Adet5 = dr["Adet5"].ToString();
                string alinan_para = dr["alinan_para"].ToString();
                if (alinan_para == "") alinan_para = "0";

                string sql = "update SiparisDetay set ";
                if (!string.IsNullOrEmpty(Adet1))
                    sql= sql+ "Adet1=" + Adet1.Replace(",", ".") + "," ;
                else
                    sql = sql + "Adet1=null,";
                if (!string.IsNullOrEmpty(Adet2))
                    sql = sql + "Adet2=" + Adet2.Replace(",", ".") + ",";
                else
                    sql = sql + "Adet2=null,";
                if (!string.IsNullOrEmpty(Adet3))
                    sql = sql + "Adet3=" + Adet3.Replace(",", ".") + ",";
                else
                    sql = sql + "Adet3=null,";
                if (!string.IsNullOrEmpty(Adet4))
                    sql = sql + "Adet4=" + Adet4.Replace(",", ".") + ",";
                else
                    sql = sql + "Adet4=null,";

                if (!string.IsNullOrEmpty(Adet5))
                    sql = sql + "Adet5=" + Adet5.Replace(",", ".") + ",";
                else
                    sql = sql + "Adet5=null,";

                if (!string.IsNullOrEmpty(alinan_para))
                    sql = sql + "alinan_para=" + alinan_para.Replace(",", ".");
                else
                    sql = sql + "alinan_para=null,";

                sql = sql + " where pkSiparisDetay=" + pkSiparisDetay;

                int sonuc = DB.ExecuteSQL(sql);
            }

            string sablonmu = "0";

            if (checkEdit1.Checked) sablonmu = "1";
              DB.ExecuteSQL("update Siparis set sablon="+sablonmu+",aciklama='" + teSablonAdi.Text+ "' where pkSiparis="+lueAcikSiparisler.EditValue.ToString());

              SiparisGetir();

              formislemleri.Mesajform("Siparişler Kaydedildi", "S", 200);

              simpleButton2.Enabled = false;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            simpleButton2_Click(sender, e);

            string DosyaYol = DB.exeDizini + "\\Raporlar\\SiparisExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("SiparisExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            SiparisYazdir(false, DosyaYol);
        }
        void SiparisYazdir(bool Disigner, string RaporDosyasi)
        {
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = @"select 
s.pkSiparis,
s.siparis_tarihi,
p.adi,p.soyadi,
f.Firmaadi,
F.Devir,
sd.Adet1,
sd.Adet2,
sd.Adet3,
sd.Adet4,
sd.Adet5,
sd.Adet6,
sd.alinan_para

 from Siparis s with(nolock) 
left join SiparisDetay sd with(nolock) on sd.fkSiparis=s.pkSiparis 
left join Firmalar f with(nolock) on f.pkFirma=sd.fkFirma
left join Personeller p on p.pkpersoneller=s.fkPersoneller
where s.pkSiparis=" + lueAcikSiparisler.EditValue.ToString();
            
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(sql);//, list);
                if (FisDetay.Rows.Count == 0)
                {
                    MessageBox.Show("Kayıt Yok");
                    return;
                }
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + lueAcikSiparisler.Text + "' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);
                ////kasahareketleri
                //DataTable dtkasahareketleri = DB.GetData(@"select Tarih,OdemeSekli,Borc,Alacak,Tutar  from KasaHareket WHERE fkFirma=" + musteriadi.Tag.ToString());
                //dtkasahareketleri.TableName = "kasahareketleri";
                //ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;

                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "SiparisExtresi";
                rapor.Report.Name = "SiparisExtresi";

                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void siparisExtresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\SiparisExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("SiparisExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            SiparisYazdir(true, DosyaYol);
        }
        void MusteriPlasiyer()
        {
            gcFirmaPersonel.DataSource = DB.GetData("select f.pkFirma,P.pkPersoneller,f.Firmaadi,p.Adi+' '+p.Soyadi as Personel,f.Aktif from Firmalar f with(nolock) "+
            "left join Personeller P with(nolock) on P.pkPersoneller=f.fkPerTeslimEden where f.Aktif=1");
        }
        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if(e.Page==xtraTabPage1)
                gridControl3.DataSource = DB.GetData("select * from siparis with(nolock) where kaydedildi=1");
            if(e.Page==xtraTabPage2)
                MusteriPlasiyer();
        }

        private void btnSiparisSil_Click_1(object sender, EventArgs e)
        {
            if (lueAcikSiparisler.EditValue==null) return;

            string sec = formislemleri.MesajBox("Sipariş İptal Edilecektir. Eminmisiniz?", "Sipariş İptal", 3, 0);
            if (sec != "1") return;

            

            string pkSiparis = lueAcikSiparisler.EditValue.ToString();
            DB.ExecuteSQL("delete  from SiparisDetay where fkSiparis=" + pkSiparis);
            DB.ExecuteSQL("delete  from Siparis where pkSiparis=" + pkSiparis);


            SiparisGetir();

            AcikSiparisleriGetir();
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = pkFirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
        }

        private void ödemeYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = pkFirma;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();
        }

        private void müşteriKartıToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();

            frmMusteriKarti KurumKarti = new frmMusteriKarti(pkFirma, "");
            KurumKarti.ShowDialog();

            //int secilen = gridView1.FocusedRowHandle;

            //gridrowgetir(secilen);

            //SiparisGetir();
            MusteriPlasiyer();
        }

        private void personelKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string personel_id = dr["pkPersoneller"].ToString();
            if (personel_id == "")
            {
                formislemleri.Mesajform("Plasiyer Tanımlanmamış", "K", 200);
                return;
            }
            frmPersonel personelkart = new frmPersonel(personel_id);
            personelkart.ShowDialog();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            SiparisGetir();
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            yazdir();
        }

        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            //if (xtraTabControl1.SelectedTabPageIndex == 0)
                printableLink.Component = gridControl1;
            //else if (xtraTabControl1.SelectedTabPageIndex == 1)
             //   printableLink.Component = gridControl2;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));
        }
        public virtual void InitPrintTool(PrintTool tool)
        {
            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);
        }
        PrintingSystem printingSystem = null;
        PrintingSystem Printing
        {
            get
            {
                if (printingSystem == null) printingSystem = new PrintingSystem();
                return printingSystem;
            }
        }

        private void btnEPosta_Click(object sender, EventArgs e)
        {

        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string pkSiparis = dr["pkSiparis"].ToString();

            #region trans başlat
            if (DB.conTrans == null)
            {
                DB.conTrans = new SqlConnection(DB.ConnectionString());
            }

            if (DB.conTrans.State == ConnectionState.Closed)
            {
                //DB.conTrans = new SqlConnection(DB.ConnectionString());
                DB.conTrans.Open();
                //transaction = conTrans.BeginTransaction("AdemTransaction");
            }

            DB.transaction = DB.conTrans.BeginTransaction("AdemTransaction");
            #endregion
            bool islembasarili=false;

            string sonuc = DB.ExecuteSQLTrans("Delete From SiparisDetay where fkSiparis=" + pkSiparis);
            if (sonuc == "0")
            {
                sonuc = DB.ExecuteSQLTrans("Delete From Siparis where pkSiparis=" + pkSiparis);
                islembasarili = true;
            }

            #region trans. işlemi
            if (islembasarili)
            {
                //local sunucu
                DB.transaction.Commit();
                DB.conTrans.Close();
                //hatasiz++;
                //listBoxControl1.Items.Add("Fiş No: " + pkSatislar + " Başarılı");
                formislemleri.Mesajform("Siparis Silindi" + sonuc, "S", 200);
                gridControl3.DataSource = DB.GetData("select * from siparis with(nolock) where kaydedildi=1");
            }
            else
            {
                //locak sunucuyu kapat
                if (DB.conTrans.State == ConnectionState.Open)
                    DB.transaction.Rollback();
                DB.conTrans.Close();
                //hatali++;
                //listBoxControl1.Items.Add("Fiş No: " + pkSatislar + " Hatalı");
                //continue;
                formislemleri.Mesajform("Hata Oluştu" + sonuc, "K", 200);
            }
            #endregion

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriFiyatlari.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriFiyatlari.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            YazdirFiyat(false, DosyaYol);
        }

        void YazdirFiyat(bool Disigner, string RaporDosyasi)
        {
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = @" select f.pkFirma,f.Firmaadi,sf.SatisFiyatiKdvli as SatisFiyati  from Firmalar f with(nolock)
inner join  SatisFiyatlari sf with(nolock) on sf.fkSatisFiyatlariBaslik=f.fkSatisFiyatlariBaslik
where f.fkPerTeslimEden is not null and  sf.fkStokKarti=1";

            sql=@"select f.pkFirma,f.Firmaadi,sk.Stokadi,ss.Sira,sf.SatisFiyatiKdvli as SatisFiyati,sf.fkStokKarti from Firmalar f with(nolock)
inner join  SatisFiyatlari sf with(nolock) on sf.fkSatisFiyatlariBaslik=f.fkSatisFiyatlariBaslik
inner join SiparisStoklari ss with(nolock) on ss.fkStokKarti=sf.fkStokKarti
inner join StokKarti sk on sk.pkStokKarti=sf.fkStokKarti
where f.fkPerTeslimEden is not null 
order by f.pkFirma,ss.Sira";
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable Fiyatlar = DB.GetData(sql);//, list);
                if (Fiyatlar.Rows.Count == 0)
                {
                    MessageBox.Show("Kayıt Yok");
                    return;
                }
                Fiyatlar.TableName = "Fiyatlar";
                ds.Tables.Add(Fiyatlar);

                //Firma Bilgileri
                //DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                //Fis.TableName = "Fis";
                //ds.Tables.Add(Fis);
                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT 'Müşteri Fiyatları' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);
                ////kasahareketleri
                //DataTable dtkasahareketleri = DB.GetData(@"select Tarih,OdemeSekli,Borc,Alacak,Tutar  from KasaHareket WHERE fkFirma=" + musteriadi.Tag.ToString());
                //dtkasahareketleri.TableName = "kasahareketleri";
                //ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;

                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriFiyatlari";
                rapor.Report.Name = "MusteriFiyatlari";

                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void musterİFiyatlariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriFiyatlari.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriFiyatlari.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            YazdirFiyat(true, DosyaYol);
        }

        private void değiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridView2.FocusedRowHandle<0) return;

            DataRow drss = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            
            frmStokAra StokAra = new frmStokAra("");

            StokAra.Tag = "0";
            StokAra.ShowDialog();

            if (StokAra.TopMost == false)
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    string pkStokKarti=dr["pkStokKarti"].ToString();
                    //string Barkod=dr["Barcode"].ToString();
                    DB.ExecuteSQL("update SiparisStoklari set fkStokKarti="+pkStokKarti+
                        " where pkSiparisStoklari="+drss["pkSiparisStoklari"].ToString());
                }
            }
            StokAra.Dispose();
            
            SiparisStoklari();     


        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string fkStokKarti = dr["fkStokKarti"].ToString();

            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(fkStokKarti);
            StokKarti.ShowDialog();

            SiparisStoklari(); 
        }

        private void silToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            
            DB.ExecuteSQL("delete from SiparisStoklari  where pkSiparisStoklari=" + dr["pkSiparisStoklari"].ToString());

            SiparisStoklari();
        }

        private void ekleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmStokAra StokAra = new frmStokAra("");

            StokAra.Tag = "0";
            StokAra.ShowDialog();

            if (StokAra.TopMost == false)
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    string pkStokKarti = dr["pkStokKarti"].ToString();
                    
                    DB.ExecuteSQL("insert into SiparisStoklari (fkStokKarti,Aktif,Sira) values("+pkStokKarti+",1,"+(gridView2.DataRowCount+1).ToString()+")");
                }

                SiparisStoklari();
            }
            StokAra.Dispose();

                 

        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            simpleButton2.Enabled = true;
        }

        private void frmSiparisHizli_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (simpleButton2.Enabled && gridView1.DataRowCount>0)
            {
                string m=formislemleri.MesajBox("Değişiklikler Kaydedilsin mi?", "Değişiklik var ", 3, 0);
                if (m == "0")
                {
                    e.Cancel = true;
                }
                else
                    simpleButton2_Click(sender, e);


            }
        }

        private void stokSatışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int f = gridView2.FocusedRowHandle;

            if (f < 0)
            {
                f = gridView2.DataRowCount - 1;
            }
            DataRow dr = gridView2.GetDataRow(f);
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            SatisFiyatlari.ShowDialog();

            gridView2.FocusedRowHandle=f;
        }

        private void yeniMüşteriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriKarti musterikarti = new frmMusteriKarti("0", "");
            musterikarti.ShowDialog();

            MusteriPlasiyer();
        }
    }
}
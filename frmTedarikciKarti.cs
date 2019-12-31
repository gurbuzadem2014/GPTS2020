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
    public partial class frmTedarikciKarti : DevExpress.XtraEditors.XtraForm
    {
        string _fkTedarikci = "1";
        public frmTedarikciKarti(string fkTedarikci)
        {
            InitializeComponent();

            _fkTedarikci = fkTedarikci;
            int gen = Screen.PrimaryScreen.WorkingArea.Width;
            if (gen < 800)
                gen = 20;
            else
                gen = gen * 55 / 100;
           // this.Width = Screen.PrimaryScreen.WorkingArea.Width - gen;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 10;
        }
        
        private void frmKurumKarti_Load(object sender, EventArgs e)
        {
            Sirketler();

            iller();

            TedarikciGrupKartlari();

            if (DB.pkTedarikciler == 0)
            {
                simpleButton1_Click(sender, e);

                btnKaydet.Text = "Kaydet [F9]";
                lUETedarikciGrup.ItemIndex = 0;
                lUETedarikciAltGrup.ItemIndex = -1;
                LUESehir.EditValue = Degerler.fkilKoduDefault;
                lookUpilce.EditValue = Degerler.fkilceAltGrupDefault;
            }
            else
            {
                TedarikciBilgileriGetir();
            }

            string Genel = DB.Sektor;// "Genel";
            string layoutFileName = Application.StartupPath + "\\EkranTasarimlari\\" + Genel + "\\TedarikciKarti.xml";
            if (System.IO.File.Exists(layoutFileName))
                layoutControl1.RestoreLayoutFromXml(layoutFileName);
            else
                MessageBox.Show(layoutFileName +" Dosya Bulunamadı!");

            padi.Focus();
        }

        void TedarikciBilgileriGetir()
        {
            DataTable dt = DB.GetData("select * from Tedarikciler where pkTedarikciler=" + DB.pkTedarikciler);
            if (dt.Rows.Count > 0)
            {
                btnKaydet.Text = "Güncelle [F9]";

                pkkurum.Text = dt.Rows[0]["pkTedarikciler"].ToString();
                if (pkkurum.Text == "1") padi.Properties.ReadOnly = true;

                padi.Text = dt.Rows[0]["Firmaadi"].ToString();
                telno.Text = dt.Rows[0]["Tel"].ToString();
                tel2.Text = dt.Rows[0]["Tel2"].ToString();
                belgecfax.Text = dt.Rows[0]["Fax"].ToString();
                Cep.Text = dt.Rows[0]["Cep"].ToString();
                Cep2.Text = dt.Rows[0]["Cep2"].ToString();
                web.Text = dt.Rows[0]["webadresi"].ToString();
                eposta.Text = dt.Rows[0]["eposta"].ToString();
                mAdres.Text = dt.Rows[0]["Adres"].ToString();
                tEYetkili.Text = dt.Rows[0]["Yetkili"].ToString();
                lUETedarikciAltGrup.Tag = dt.Rows[0]["fkTedarikcilerAltGruplari"].ToString();
                if (lUETedarikciAltGrup.Tag.ToString() == "")
                    lUETedarikciAltGrup.Tag = "0";

                decimal LimitBorc = 0;
                decimal.TryParse(dt.Rows[0]["LimitBorc"].ToString(), out LimitBorc);
                ceLimitBorc.Value = LimitBorc;
                if (dt.Rows[0]["fkSirket"].ToString() != "")
                    lUSirket.EditValue = int.Parse(dt.Rows[0]["fkSirket"].ToString());
                if (dt.Rows[0]["fkFirmaGruplari"].ToString() == "")
                    lUETedarikciGrup.EditValue = 0;
                else
                    lUETedarikciGrup.EditValue = int.Parse(dt.Rows[0]["fkFirmaGruplari"].ToString());

                int il = 0;
                int.TryParse(dt.Rows[0]["fkil"].ToString(), out il);
                LUESehir.EditValue = il;
                int ilce = 0;
                int.TryParse(dt.Rows[0]["fkilce"].ToString(), out ilce);
                lookUpilce.EditValue = ilce;

                VergiDairesi.Text = dt.Rows[0]["VergiDairesi"].ToString();
                VergiNo.Text = dt.Rows[0]["VergiNo"].ToString();
                teUnvani.Text = dt.Rows[0]["Yetkili"].ToString();

                if (dt.Rows[0]["KaraListe"].ToString() == "True")
                    cbKaraliste.Checked = true;
                else
                    cbKaraliste.Checked = false;

                if (dt.Rows[0]["Aktif"].ToString() == "True")
                    cbDurumu.SelectedIndex = 0;
                else
                    cbDurumu.SelectedIndex = 1;

                MusteriKodu.Text = dt.Rows[0]["OzelKod"].ToString();

                if (dt.Rows[0]["gider_mi"].ToString() == "True")
                    ceGider.Checked = true;
                else
                    ceGider.Checked = false;

            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DB.pkTedarikciler = 0;
            pkkurum.Text = "0";
            padi.Text = "";
            telno.Text = "";
            tel2.Text = "";
            mAdres.Text = "";
            web.Text = "www.";
            eposta.Text = "@";
            btnKaydet.Text = "Kaydet  [F9]";
            tEYetkili.Text = "";
            teUnvani.Text = "";
            belgecfax.Text = "";
            memoozelnot.Text = "";
            lUETedarikciGrup.EditValue = 1;
            lUETedarikciAltGrup.EditValue = null;
            ceLimitBorc.Value=0;
            cbKaraliste.Checked = false;
            cbDurumu.SelectedIndex = 0;

            LUESehir.EditValue = Degerler.fkilKoduDefault;
            lookUpilce.EditValue = Degerler.fkilceAltGrupDefault;

            int i = 1;
            int.TryParse(DB.GetData("SELECT Max(pkTedarikciler) from Tedarikciler").Rows[0][0].ToString(), out i);
            MusteriKodu.Text = (i + 1).ToString();

            #region Şirket Ayarları
            DataTable dt = DB.GetData("select * from Sirketler with(nolock)");
            if (dt.Rows[0]["tedarikciler_giderdir"].ToString() == "True")
            {
                ceGider.Checked = true;
            }
            #endregion
        }

        void Sirketler()
        {
            DataTable dtb = DB.GetData("select * from Sirketler with(nolock)");
            lUSirket.Properties.DataSource = dtb;
            lUSirket.Properties.ValueMember = "pkSirket";
            lUSirket.Properties.DisplayMember = "Sirket";
        }

        private void frmKurumKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.F9)
            {
                btnKaydet_Click(sender, null);
            }
            else if (e.KeyCode == Keys.F7)
            {
                simpleButton1_Click(sender, null);
            }
        }

        private void LUESehir_EditValueChanged(object sender, EventArgs e)
        {
            ilceler(LUESehir.EditValue.ToString());
        }
        void iller()
        {
            DataTable dt = DB.GetData(@"SELECT [ADI] ,[KODU] ,[GRUP],[ALTGRUP] FROM IL_ILCE_MAH with(nolock)
                                         WHERE GRUP=1  ORDER BY KODU");
            LUESehir.Properties.DataSource = dt;
            LUESehir.Properties.ValueMember = "KODU";
            LUESehir.Properties.DisplayMember = "ADI";
            //LUESehir.EditValue = Degerler.fkAltGrupDefault;
            //lookUpilce.EditValue = Degerler.fkKoduDefault;
        }
        void ilceler(string ilid)
        {
            DataTable dt = DB.GetData(@"SELECT [ADI] ,[KODU] ,[GRUP],[ALTGRUP] FROM IL_ILCE_MAH with(nolock)
                                         WHERE ALTGRUP=" + ilid + "  ORDER BY KODU");
            lookUpilce.Properties.DataSource = dt;
            lookUpilce.Properties.ValueMember = "KODU";
            lookUpilce.Properties.DisplayMember = "ADI";
        }

        private void xtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            //if(e.Page==xtraTabPage4)
            //{
            //    DataTable dt = DB.GetData("select * from FirmaOzelNot where fkFirma=" + pkkurum.Text);
            //    if(dt.Rows.Count>0)
            //      memoozelnot.Text = dt.Rows[0]["OzelNot"].ToString();

            //}
        }

        void OzelNotKaydet1()
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkTedarikciler", pkkurum.Text));
            list.Add(new SqlParameter("@OzelNot", memoozelnot.Text));
            DataTable dt = DB.GetData("select * from TedarikcilerOzelNot where fkTedarikciler=" + pkkurum.Text);
            if (dt.Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO TedarikcilerOzelNot (fkTedarikciler,OzelNot) VALUES(@fkTedarikciler,@OzelNot)", list);
            else
                DB.ExecuteSQL("UPDATE TedarikcilerOzelNot SET OzelNot=@OzelNot WHERE fkTedarikciler=@fkTedarikciler", list);
            memoozelnot.Tag = 0;
        }
        private void memoozelnot_Leave(object sender, EventArgs e)
        {
            if(memoozelnot.Tag.ToString() == "1")
                OzelNotKaydet1();
        }

        private void memoozelnot_EditValueChanged(object sender, EventArgs e)
        {
            memoozelnot.Tag = 1;
        }

        private void frmKurumKarti_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (memoozelnot.Tag.ToString() == "1")
                OzelNotKaydet1();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (padi.Text == "")
            {
                //DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Adı Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                padi.Focus();
                dxErrorProvider1.SetError(padi, "Müşteri Adı Boş Olamaz!");
                return;
            }
            if (lUSirket.EditValue == null)
            {
                //DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Adı Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUSirket.Focus();
                dxErrorProvider1.SetError(lUSirket, "Müşterinin Bağlı olduğu Şirketi Seçiniz!");
                //return;
                lUSirket.EditValue = 1;
            }
           
            //if (lUEGrup.EditValue == null)
            //{
            //    dxErrorProvider1.SetError(lUEGrup, "Müşteri Grubu Boş Olamaz!");
            //    //DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Grubu Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    lUEGrup.Focus();
            //    return;
            //}
            if (cbDurumu.EditValue == null)
            {
                dxErrorProvider1.SetError(cbDurumu, "Müşteri Durumu Boş Olamaz!");
                //DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Grubu Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbDurumu.Focus();
                return;
            }
            string sql = "";
            ArrayList plist = new ArrayList();
            if (pkkurum.Text == "0" | pkkurum.Text == "")
            {
                sql = @"insert into Tedarikciler (Firmaadi,tel,tel2,fax,OdemeIlk,OdemeSon,webadresi,eposta,adres,Aktif,KaraListe,fkSirket,Yetkili,KadroSayisi,LimitBorc,fkFirmaGruplari,fkil,fkilce,
                      VergiDairesi,VergiNo,Unvani,Cep,Cep2,OzelKod,fkTedarikcilerAltGruplari,Borc,Alacak,gider_mi) 
                      values(@Firmaadi,@tel,@tel2,@fax,@OdemeIlk,@OdemeSon,@webadresi,@eposta,@adres,@Aktif,@KaraListe,@fkSirket,@Yetkili,@KadroSayisi,@LimitBorc,@fkFirmaGruplari,@fkil,@fkilce,
                      @VergiDairesi,@VergiNo,@Unvani,@Cep,@Cep2,@OzelKod,@fkTedarikcilerAltGruplari,0,0,@gider_mi) SELECT IDENT_CURRENT('Tedarikciler')";
            }
            else
            {
                sql = @"update Tedarikciler set Firmaadi=@Firmaadi,tel=@tel,tel2=@tel2,fax=@fax,fkSirket=@fkSirket,OdemeSon=@OdemeSon,
                        eposta=@eposta,webadresi=@webadresi,adres=@adres,Aktif=@Aktif,KaraListe=@KaraListe,OdemeIlk=@OdemeIlk,Yetkili=@Yetkili,
                        KadroSayisi=@KadroSayisi,LimitBorc=@LimitBorc,fkFirmaGruplari=@fkFirmaGruplari,
                        fkil=@fkil,fkilce=@fkilce,VergiDairesi=@VergiDairesi,VergiNo=@VergiNo,Unvani=@Unvani,Cep=@Cep,Cep2=@Cep2,OzelKod=@OzelKod,
                        fkTedarikcilerAltGruplari=@fkTedarikcilerAltGruplari,gider_mi=@gider_mi where pkTedarikciler=@pkTedarikciler";
            }
            plist.Add(new SqlParameter("@pkTedarikciler", pkkurum.Text));
            plist.Add(new SqlParameter("@Firmaadi", padi.Text));
            plist.Add(new SqlParameter("@tel", telno.Text));
            plist.Add(new SqlParameter("@tel2", tel2.Text));
            plist.Add(new SqlParameter("@Cep", Cep.Text));
            plist.Add(new SqlParameter("@Cep2", Cep2.Text));
            plist.Add(new SqlParameter("@OdemeIlk", "0"));//seOdemeIlk.Value));
            plist.Add(new SqlParameter("@fax", belgecfax.Text));
            plist.Add(new SqlParameter("@fkSirket", lUSirket.EditValue));
            plist.Add(new SqlParameter("@webadresi", web.Text));
            plist.Add(new SqlParameter("@eposta", eposta.Text));
            plist.Add(new SqlParameter("@adres", mAdres.Text));
            plist.Add(new SqlParameter("@OdemeSon", "0"));//seOdemeSon.Value));
            plist.Add(new SqlParameter("@Yetkili", tEYetkili.Text));
            plist.Add(new SqlParameter("@KadroSayisi", "0"));
            if (lUETedarikciGrup.EditValue == null)
                plist.Add(new SqlParameter("@fkFirmaGruplari", 0));
            else
                plist.Add(new SqlParameter("@fkFirmaGruplari", int.Parse(lUETedarikciGrup.EditValue.ToString())));
            //il
            if (LUESehir.EditValue == null)
                plist.Add(new SqlParameter("@fkil", 0));
            else
                plist.Add(new SqlParameter("@fkil", LUESehir.EditValue.ToString()));
            //ilçe
            if (lookUpilce.EditValue == null)
                plist.Add(new SqlParameter("@fkilce", 0));
            else
                plist.Add(new SqlParameter("@fkilce", lookUpilce.EditValue.ToString()));

            plist.Add(new SqlParameter("@VergiDairesi", VergiDairesi.Text));
            plist.Add(new SqlParameter("@VergiNo", VergiNo.Text));
            plist.Add(new SqlParameter("@Unvani", teUnvani.Text));
            plist.Add(new SqlParameter("@LimitBorc", ceLimitBorc.Value.ToString().Replace(",", ".")));

            plist.Add(new SqlParameter("@KaraListe", cbKaraliste.Checked));

            if(cbDurumu.SelectedIndex==0)
               plist.Add(new SqlParameter("@Aktif", true));
            else
               plist.Add(new SqlParameter("@Aktif", false));
            if (MusteriKodu.Text == "")
                plist.Add(new SqlParameter("@OzelKod", "0"));
            else
                plist.Add(new SqlParameter("@OzelKod", MusteriKodu.Text));
            if (lUETedarikciAltGrup.EditValue == null)
                plist.Add(new SqlParameter("@fkTedarikcilerAltGruplari", DBNull.Value));
            else
                plist.Add(new SqlParameter("@fkTedarikcilerAltGruplari", lUETedarikciAltGrup.EditValue.ToString()));

            plist.Add(new SqlParameter("@gider_mi", ceGider.Checked));

            string sonuc ="";
            if (pkkurum.Text == "0" | pkkurum.Text == "")
            {
                sonuc = DB.ExecuteScalarSQL(sql, plist);
                pkkurum.Text = sonuc;// DB.GetData("select MAX(PkFirma) as c from Firmalar").Rows[0][0].ToString();

                MusteriKodu.Text = sonuc;
                DB.pkTedarikciler = int.Parse(sonuc);
                DB.ExecuteSQL("UPDATE Tedarikciler SET OzelKod=pkTedarikciler where pkTedarikciler=" + sonuc);
                if(sonuc=="1")
                    DB.ExecuteSQL("UPDATE Tedarikciler SET GeciciMusteri=1 where pkTedarikciler=1");
            }
            else
                sonuc = DB.ExecuteSQL(sql, plist);

            if(sonuc.Substring(0,1)!="H")    
                formislemleri.Mesajform("Tedarikçi Bilgileri Kaydedildi.", "S",200);
            else
                formislemleri.Mesajform("Tedarikçi Bilgilerini Kontrol Ediniz." + sonuc, "K", 200);
        }

        private void padi_EditValueChanged(object sender, EventArgs e)
        {
            baslik.Text = padi.Text;
        }

        private void layoutControl1_HideCustomization(object sender, EventArgs e)
        {
            //kapanınca
            layoutControl1.SaveLayoutToXml(Application.StartupPath + "\\EkranTasarimlari\\" +  DB.Sektor + "\\TedarikciKarti.xml");
        }

        private void OzelKod_Leave(object sender, EventArgs e)
        {
            //if (OzelKod.OldEditValue == null)
            if (DB.pkTedarikciler == 0)
            {
                if (DB.GetData("Select * from  Tedarikciler where OzelKod='" + MusteriKodu.Text + "'").Rows.Count > 0)
                {
                    MessageBox.Show("Tedarikçi Kodu Daha Önce Kullanıldı.");
                    MusteriKodu.Focus();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            padi.Focus();
            timer1.Enabled = false;
        }

        private void eposta_Leave(object sender, EventArgs e)
        {
            //bool kontrol = DB.EmailKontrol(eposta.Text);
            //if(!kontrol)
            //   MessageBox.Show("E-Posta Adresini Kontrol Ediniz!");
        }

        private void MusteriKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            MusteriKodu.Properties.ReadOnly = false;
        }
        void TedarikciGrupKartlari()
        {
            lUETedarikciGrup.Properties.DataSource = DB.GetData("select * from TedarikcilerGruplari where Aktif=1");
        }
        void TedarikciAltGrupKartlari(string fkTedarikcilerGruplari)
        {
            lUETedarikciAltGrup.Properties.DataSource = DB.GetData("select * from TedarikcilerAltGruplari where Aktif=1 and fkTedarikcilerGruplari="
                + fkTedarikcilerGruplari);
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmTedarikciGrupKarti TedarikciGrupKarti = new frmTedarikciGrupKarti();
            TedarikciGrupKarti.ShowDialog();
            TedarikciGrupKartlari();
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            if (lUETedarikciGrup.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Tedarikçi Grubunu Seçiniz", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lUETedarikciGrup.Focus();
                return;
            }
            frmTedarikciAltGruplari TedarikciAltG = new frmTedarikciAltGruplari();
            TedarikciAltG.lueStokGruplari.EditValue = lUETedarikciGrup.EditValue;
            TedarikciAltG.lueStokGruplari.Tag = lUETedarikciGrup.EditValue;
            TedarikciAltG.ShowDialog();
            TedarikciAltGrupKartlari(lUETedarikciGrup.EditValue.ToString());
        }

        private void lUETedarikciGrup_EditValueChanged(object sender, EventArgs e)
        {
            TedarikciAltGrupKartlari(lUETedarikciGrup.EditValue.ToString());
            int i = 0;
            int.TryParse(lUETedarikciAltGrup.Tag.ToString(),out i);
            lUETedarikciAltGrup.EditValue = i;
        }

        private void eposta_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void tabbedControlGroup1_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            if (e.Page.Text =="Özel Not" && DB.pkTedarikciler.ToString() != "0")
            {
                DataTable dt =DB.GetData("select OzelNot from TedarikcilerOzelNot where fkTedarikciler=" + DB.pkTedarikciler);
                if(dt.Rows.Count>0)
                  memoozelnot.Text = dt.Rows[0][0].ToString();
            }
        }

        private void padi_DoubleClick(object sender, EventArgs e)
        {
            if (padi.Properties.ReadOnly == false) return;
            if (!formislemleri.SifreIsteYazilim()) return;
            padi.Properties.ReadOnly = false;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            mAdres.Text = mAdres.Text + " " + lookUpilce.Text + " / " + LUESehir.Text;
        }

        private void btnilceekle_Click(object sender, EventArgs e)
        {
            frmilceekle ilceekle = new frmilceekle();
            ilceekle.ilid.Text = LUESehir.EditValue.ToString();
            ilceekle.sehiradi.Text = LUESehir.Text;
            ilceekle.ShowDialog();
            ilceler(LUESehir.EditValue.ToString());
        }

        private void MusteriKodu_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                DataTable dt = DB.GetData("select * from Tedarikciler with(nolock) where OzelKod='" + MusteriKodu.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    MusteriKodu.Text = dt.Rows[0][0].ToString();
                    DB.pkTedarikciler = int.Parse(MusteriKodu.Text);
                    TedarikciBilgileriGetir();
                    padi.Focus();
                }
                else
                {
                    formislemleri.Mesajform("Tedarikçi Bulunamadı", "K", 100);
                    MusteriKodu.Text = "0";
                    DB.pkTedarikciler = 0;
                    simpleButton1_Click(sender, e);
                }
            }
        }
    }
}
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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using System.IO;
using GPTS.Include.Data;
using DevExpress.XtraScheduler.Native;

namespace GPTS
{
    public partial class ucRandevuVerYeni : DevExpress.XtraEditors.XtraUserControl
    {
        //bool ilkyukleme = false;
        protected string firma_id = "0";
        string RandevuBasSaat = "8", RandevuBitSaat = "21", RandevuSaatAraligi = "30";
        private void RandevuAyarlari(out string RandevuBasSaat, out string RandevuBitSaat, out string RandevuSaatAraligi)
        {
            RandevuBasSaat = "8";
            RandevuBitSaat = "21";
            RandevuSaatAraligi = "30";

            DataTable dt = null;

            dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuBasSaat'");
            if (dt.Rows.Count > 0)
                RandevuBasSaat = dt.Rows[0]["Ayar50"].ToString();

            dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuBitSaat'");
            if (dt.Rows.Count > 0)
                RandevuBitSaat = dt.Rows[0]["Ayar50"].ToString();

            dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuSaatAraligi'");
            if (dt.Rows.Count > 0)
                RandevuSaatAraligi = dt.Rows[0]["Ayar50"].ToString();

            TimeSpan ts1 = TimeSpan.FromHours(int.Parse(RandevuBasSaat));
            schedulerControl1.Views.DayView.VisibleTime.Start = ts1;

            TimeSpan ts2 = TimeSpan.FromHours(int.Parse(RandevuBitSaat));
            schedulerControl1.Views.DayView.VisibleTime.End = ts2;

            TimeSpan ts3 = TimeSpan.FromMinutes(int.Parse(RandevuSaatAraligi));
            schedulerControl1.Views.DayView.TimeScale = ts3;

        }

        public ucRandevuVerYeni()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 200;

            //dateNavigator1.DateTime = DateTime.Today.AddDays(90);
            //this.dateNavigator1.VisibleChanged += new EventHandler(dateNavigator1_VisibleChangedChanged);
            //xtraTabControl2.SelectedTabPageIndex = 0;
            xtraTabControl2.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            RandevuAyarlari(out RandevuBasSaat, out RandevuBitSaat, out RandevuSaatAraligi);
            RandevuGorunumAyarla();
            //this.schedulerControl1.VisibleIntervalChanged += new EventHandler(schedulerControl1_VisibleIntervalChanged);
        }

        private void frmRandevuVer_Load(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = DateTime.Today.AddDays(+360);

            HatirlatmaDurumlar();

            Hizmetler();

            //if (deTarihi.Tag == null)
            //if (txtPkHatirlatma.Text == "" || txtPkHatirlatma.Text == "0")
            //{
                //deTarihi.DateTime = deRandevuZamani.DateTime;
                //ilkSeans();

                //deRandevuZamani.DateTime = deTarihi.DateTime;
            //}
            //else
            //    deTarihi.DateTime = deRandevuZamani.DateTime;
            //lueStoklar.EditValue = 1;
            //ilkyukleme = true;

            
            deRandevuZamani.DateTime = DateTime.Now;
            dateNavigator1.DateTime = DateTime.Today;
            schedulerControl1.GoToToday();

            //xtraTabControl2.SelectedTabPage = xtabRandevuGorunum;

            string Dosya = DB.exeDizini + "\\RandevuVerGrid.xml";
            if (File.Exists(Dosya))
            {
                gridView2.RestoreLayoutFromXml(Dosya);
                gridView2.ActiveFilter.Clear();
            }
            string Dosya2 = DB.exeDizini + "\\RandevuVerMusteriGrid.xml";

            if (File.Exists(Dosya2))
            {
                gridView1.RestoreLayoutFromXml(Dosya2);
                gridView1.ActiveFilter.Clear();
            }
            OdalarGetir();

            //odaları yanyana göster
            OdalarGruplu();
        }

        void OdalarGruplu()
        {
            schedulerControl1.GroupType = SchedulerGroupType.Resource;
            DataTable dt =
            DB.GetData(@"select 0 as pkOda,'Tanımsız' as oda_adi union all select  pkOda,oda_adi from Odalar with(nolock) where aktif = 1");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pkOda = dt.Rows[i]["pkOda"].ToString();
                string oda_adi = dt.Rows[i]["oda_adi"].ToString();
                //resources.Add(new Resource(resID, resName));
                Resource resource = schedulerStorage1.CreateResource(i);
                //resource.SetValue(schedulerStorage1,"Id", pkOda);
                //object oid = new object();
                //resource.Id = new object();
                //resource.Id = pkOda;
                //resource.CustomFields[i] = pkOda;
                //renkler
                //if (i==0)
                //    resource.Color = Color.Red;
                //else if (i == 1)
                //    resource.Color = Color.RoyalBlue;
                //else if (i == 2)
                //    resource.Color = Color.SeaGreen;

                resource.Caption = oda_adi;// oda_adi;//string.Format("Resource{0}", i);
                int s = schedulerStorage1.Resources.Add(resource);
            }
        }

        void OdalarGetir()
        {
            lueOdalar.Properties.DataSource = DB.GetData(@"select 0 as pkOda,0 as fkKat,'Tüm Odalar' as oda_adi
            union all
            select pkOda,fkKat,oda_adi from Odalar with(nolock) where aktif = 1");

            lueOdalar.EditValue = 0;
        }

        private void ShowInfo(string txt)
        {
            //if (this.txtInfo.Text.Length > 800)
            //    this.txtInfo.Text = "";
            //this.txtInfo.Text += "\r\n" + (++this.counter) + " " + txt;
            //this.txtInfo.SelectionStart = this.txtInfo.Text.Length;
            //this.txtInfo.ScrollToCaret();
        }
        //private void schedulerControl1_VisibleIntervalChanged(object sender, EventArgs e)
        //{
        //    this.ShowInfo("### Visible interval changed");
        //}
        //void dateNavigator1_VisibleChangedChanged(object sender, EventArgs e)
        //{
//            this.ShowInfo("### ViewIntervalChanged");
            //DateTime dt = dateNavigator1.DateTime;
            //((DateNavigatorTest.VenusDateNavigator)sender).EndVisibleDate;
        //}


        void ilkSeans_()
        {
            string sql= @"select top 1 s.pkSeanslar,h.pkHatirlatma,
            convert(varchar(10),s.tarih,108) as saat,h.Tarih,h.sira_no from Seanslar s
            left join Hatirlatma h on convert(varchar(10),s.tarih,108)=convert(varchar(10),h.Tarih,108) and convert(varchar,h.Tarih,112)=convert(varchar,getdate(),112)
            where h.fkFirma is null
            order by saat";

            DataTable dt = DB.GetData(sql);
            if (dt.Rows.Count > 0)
                deRandevuZamani.DateTime = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd") +" "+ dt.Rows[0]["saat"].ToString());
        }

        void Hizmetler()
        {
            if (lueStoklar.Tag.ToString()=="0")
            {
                lueStoklar.Properties.DataSource = DB.GetData("select 0 as pkStokKarti,'Seçiniz...'   as Stokadi union all  select pkStokKarti,Stokadi from StokKarti with(nolock) where Aktif=1");
                lueStoklar.EditValue = 0;
                lueStoklar.Tag = "1";
            }
        }

        void HatirlatmaDurumlar()
        {
            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from HatirlatmaDurum with(nolock)");
            repositoryItemLookUpEdit1.ValueMember = "pkHatirlatmaDurum";
            repositoryItemLookUpEdit1.DisplayMember = "durumu";

            repositoryItemLookUpEdit2.DataSource = repositoryItemLookUpEdit1.DataSource;
            repositoryItemLookUpEdit2.ValueMember = "pkHatirlatmaDurum";
            repositoryItemLookUpEdit2.DisplayMember = "durumu";

        }

        private void RandevuListesi()
        {
            //string sql = "hsp_SeansRandevuListesi '"+deTarihi.DateTime.ToString("yyyy-MM-dd")+"'";
            //if (cbBosOlanlar.Checked)
            //    sql = sql + ",1";
            //else
            //    sql = sql + ",null";

            string sql = "";
            sql = @"select h.pkHatirlatma,h.Tarih,f.pkFirma,fkFirma,f.Firmaadi,f.Adres,f.Tel,f.Tel2,f.Cep,f.Devir,fkStokKarti,sk.Stokadi,
hd.durumu,h.fkDurumu,h.Aciklama,h.sira_no,isnull(h.arandi,0) as arandi,
h.BitisTarihi,O.oda_adi,k.KullaniciAdi,h.fkOda  from Hatirlatma h with(nolock)
left join Firmalar f on f.pkFirma=h.fkFirma
left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
left join HatirlatmaDurum hd with(nolock) on hd.pkHatirlatmaDurum=h.fkDurumu
left join Odalar O with(nolock) on O.pkOda=h.fkOda
left join Kullanicilar k with(nolock) on k.pkKullanicilar=h.fkKullanicilar
where h.Tarih>='@ilktar' and h.Tarih<='@sontar'";

            if (lueOdalar.EditValue.ToString() != "0")
                sql = sql + " and h.fkOda=" + lueOdalar.EditValue.ToString();

            //if (lueHatirlatmaDurum.EditValue != null && lueHatirlatmaDurum.EditValue.ToString() != "0")
            //    sql = sql + " and h.fkDurumu=" + lueHatirlatmaDurum.EditValue.ToString();

            sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd 00:00"));
            sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd 23:59"));

            gridControl2.DataSource = DB.GetData(sql);
        }

        private void MusteriRandevuListesi()
        {
            string sql = @"select h.pkHatirlatma,h.Tarih,h.fkFirma,h.fkDurumu,fkStokKarti,sk.Stokadi ,h.Aciklama,h.sira_no,h.BitisTarihi,
            h.fkOda from Hatirlatma h with(nolock)
            left join Firmalar f on f.pkFirma=h.fkFirma
            left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
            where  h.fkFirma=" + pkFirma.Text;

            if(!cbTumunuGoster.Checked)
                sql = sql +" and h.fkDurumu=1";

            gridControl1.DataSource = DB.GetData(sql);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            //this.Dispose();
            //Close();
        }

        void RandevuSekmeListeGetir()
        {
            if (xtraTabControl2.SelectedTabPageIndex == 0)
            {
                if(cbTarihAraligi.SelectedIndex == 1)
                    RandevuListesi();
                else 
                    cbTarihAraligi.SelectedIndex = 1;
            }
            else if (xtraTabControl2.SelectedTabPageIndex == 1)
            {
                //schedulerControl1.Start = deTarihi.DateTime;
                //try
                //{
                //schedulerControl1.Start = DateTime.Today;
                //schedulerStorage1.BeginInit();//renklerde sorun oluyor
                //RandevuGorunumAyarla();
                GorunumVerileriGetir();
                //}
                //finally
                //{
                //    //schedulerStorage1.EndInit();
                //}
            }
            //deRandevuZamani.DateTime = deTarihi.DateTime;
        }

        private void TeslimTarihi_EditValueChanged(object sender, EventArgs e)
        {
            //RandevuSekmeListeGetir();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                gridView2.SetRowCellValue(i, "Sec", cbBosOlanlar.Checked);
            }
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (dr["pkFirma"].ToString() == "") return;

            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
            //Getir();
            gridView2.FocusedRowHandle = i;

            //MusteriRandevuListesi();
            RandevuSekmeListeGetir();
        }

        void zamansec_sil()
        {
            //if (gridView2.FocusedRowHandle < 0) return;
            //DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            //string tarih = deTarihi.DateTime.ToString("yyyy-MM-dd");
            //string saat = dr["saat"].ToString(); //ToString("HH:mm");
            //string zaman = tarih + " " + saat;
            //deRandevuZamani.DateTime = Convert.ToDateTime(zaman);
        }

        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //zamansec();
        }

        void SecilenRandevuyuSil()
        {
            if (gridView1.FocusedRowHandle < 0) return;

            string s = formislemleri.MesajBox("Randevu Silinsin mi?", "Randevu Sil", 3, 2);

            if (s == "0") return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkHatirlatma = dr["pkHatirlatma"].ToString();
            DB.ExecuteSQL("Delete From Hatirlatma where pkHatirlatma=" + pkHatirlatma);

            RandevuSekmeListeGetir();
            MusteriRandevuListesi();
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                SecilenRandevuyuSil();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (i< 0) return;

            DataRow dr = gridView1.GetDataRow(i);

            string fkHatirlatma = dr["pkHatirlatma"].ToString();
            txtPkHatirlatma.Text = fkHatirlatma;

            if (xtraTabControl2.SelectedTabPage ==xtabRandevuGorunum)
            {
                DateTime secilentarih = Convert.ToDateTime(dr["Tarih"].ToString());
                //deTarihi.DateTime = secilentarih;

                schedulerControl1.GoToDate(secilentarih);

                for (int j = 0; j < schedulerStorage1.Appointments.Count; j++)
                {
                    if (schedulerStorage1.Appointments[j].CustomFields[0].ToString() == fkHatirlatma)
                    {
                        schedulerControl1.SelectedAppointments.Add(schedulerStorage1.Appointments[j]);
                        return;
                    }
                }
            }
            else
            {
                düzenleToolStripMenuItem_Click(sender, e);
            }

            
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.ShowDialog();

            if (MusteriAra.Tag.ToString() == "0") return;

            pkFirma.Text = MusteriAra.fkFirma.Tag.ToString();//DB.PkFirma.ToString();

            MusteriAra.Dispose();
        }
        void MusteriBorcuGetir()
        {
            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + pkFirma.Text);
            if (dt.Rows.Count >0)
            {
                //decimal borc = 0;
                
                decimal borc=decimal.Parse(dt.Rows[0]["Devir"].ToString());
                lbBakiye.Text = Math.Round(borc,2).ToString();
            }            
        }
        void vazgec()
        {
            txtPkHatirlatma.Text = "0";
            simpleButton3.Text = "Randevu Oluştur";
            tbAciklama.Text = "";
            lueStoklar.EditValue = 0;
            pkSatislar.Text = "0";
            //simpleButton13.Enabled = false;
        }
        private void pkFirma_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + pkFirma.Text);
            if (dt.Rows.Count == 0)
            {
                teCariAdi.Text = "";
                lbBakiye.Text = "0.0";
                MusteriRandevuListesi();

                return;
            }

            teCariAdi.Text = dt.Rows[0]["Firmaadi"].ToString();
            //lbBakiye.Text = dt.Rows[0]["Devir"].ToString();
            MusteriRandevuListesi();
            MusteriBorcuGetir();
        }

        void TarihKontrol()
        {
            string sql=@"select * from Hatirlatma with(nolock)
    where fkFirma=@fkFirma and convert(varchar,Tarih,112)='@tarih' and convert(varchar,Tarih,108)='@saat'";
            sql=sql.Replace("@fkFirma",pkFirma.Text);
            sql = sql.Replace("@tarih", deRandevuZamani.DateTime.ToString("yyyyMMdd"));
            sql = sql.Replace("@saat", deRandevuZamani.DateTime.ToString("HH:mm:ss"));

            //20160618
            //08:48:18
            DataTable dt = DB.GetData(sql);
            if (dt.Rows.Count > 0)
                formislemleri.MesajBox("Aynı Tarihde Randevu Bulunmaktadır", "Tarih Kontrol", 1, 0);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            #region Uyarılar
            if (pkFirma.Text == "0" || pkFirma.Text=="")
            {
                formislemleri.Mesajform("Müşteri Seçiniz!", "K", 100);
                return;
            }
            if (lueStoklar.EditValue == null || lueStoklar.EditValue.ToString() == "0")
            {
                formislemleri.Mesajform("Hizmet Seçiniz!","K",100);
                return;
            }
            //TarihKontrol();
            #endregion

            string mesaj="";
            if (txtPkHatirlatma.Text == "0" || txtPkHatirlatma.Text == "")
                mesaj = teCariAdi.Text + "-" + deRandevuZamani.Text + "-" + lueStoklar.Text + " Randevu Oluşturulacak Eminmisiniz";
            else
                mesaj = teCariAdi.Text + "-" + deRandevuZamani.Text + "-" + lueStoklar.Text + " Randevu Güncellenecektir Eminmisiniz";

            string sonuc = formislemleri.MesajBox(mesaj, "Randevu Oluştur", 3, 1);
            if (sonuc == "0") return;

            int maxseans = 0;
            DataTable dtSeansVarmi = DB.GetData("select isnull(sira_no,1) as sira_no from Hatirlatma with(nolock) where fkDurumu=1 and fkFirma=" +
                pkFirma.Text + " and fkStokKarti=" + lueStoklar.EditValue.ToString() + " order by sira_no desc");
            if (dtSeansVarmi.Rows.Count > 0)
            {
                maxseans = int.Parse(dtSeansVarmi.Rows[0][0].ToString());
            }
            maxseans = maxseans + 1;

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Tarih", deRandevuZamani.DateTime));
            list.Add(new SqlParameter("@fkFirma",pkFirma.Text));
            list.Add(new SqlParameter("@fkStokKarti", lueStoklar.EditValue));
            list.Add(new SqlParameter("@fkDurumu", "1"));//1-Bekliyor,2-Geldi,3-Gelmedi
            list.Add(new SqlParameter("@BitisTarihi", deBitisZamani.DateTime));
            list.Add(new SqlParameter("@Aciklama", tbAciklama.Text));
            list.Add(new SqlParameter("@Konu", tbAciklama.Text));
            list.Add(new SqlParameter("@sira_no", maxseans));
            list.Add(new SqlParameter("@fkOda", lueOdalar.EditValue.ToString()));
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
            string s_sonuc = "";
            if (txtPkHatirlatma.Text == "0" || txtPkHatirlatma.Text == "")
                s_sonuc = DB.ExecuteSQL("insert into Hatirlatma (Tarih,fkFirma,fkStokKarti,fkDurumu,BitisTarihi,Aciklama,Konu,sira_no,Uyar,fkOda,fkKullanicilar)" +
                    " values(@Tarih,@fkFirma,@fkStokKarti,@fkDurumu,@BitisTarihi,@Aciklama,@Konu,@sira_no,1,@fkOda,@fkKullanicilar)", list);
            else
            {
                list.Add(new SqlParameter("@pkHatirlatma", txtPkHatirlatma.Text));

                s_sonuc = DB.ExecuteSQL("update Hatirlatma set Tarih=@Tarih,fkFirma=@fkFirma,fkStokKarti=@fkStokKarti,BitisTarihi=@BitisTarihi,Aciklama=@Aciklama,Konu=@Konu where pkHatirlatma=@pkHatirlatma", list);
            }

            if (s_sonuc.Substring(0, 1) == "H")
            {
                formislemleri.Mesajform(s_sonuc, "K", 150);
            }

            //RandevuListesi();
            Degerler.isYenile = true;
            MusteriRandevuListesi();
            RandevuSekmeListeGetir();
          
            //simpleButton14_Click(sender,e);

            tbAciklama.Text = "";
            lueStoklar.EditValue = 0;
            txtPkHatirlatma.Text = "0";
            simpleButton3.Text = "Randevu Oluştur";
            //ilkSeans();
        }

        private void lueStoklar_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from StokKarti with(nolock) where pkStokKarti=" + lueStoklar.EditValue.ToString());
            if (dt.Rows.Count > 0)
            {
                string RafSure = dt.Rows[0]["RafSure"].ToString();
                if (RafSure == "")
                    seRafSure.Value = 30;
                else
                    seRafSure.Value = int.Parse(RafSure);
            }
            else
                seRafSure.Value = 30;
            //deBitisZamani.DateTime = deRandevuZamani.DateTime.AddMinutes(int.Parse(RafSure));
            seRafSure.Enabled = true;
        }

        private void seanslarıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;
            if (gridView2.FocusedRowHandle < 0) return;

            string sonuc = formislemleri.MesajBox("Randevu Silmek İstediğinize Eminmisiniz?", "Randevu Sil", 1, 2);
            if (sonuc == "0") return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkHatirlatma =dr["pkHatirlatma"].ToString();

            DB.ExecuteSQL("delete from Hatirlatma where pkHatirlatma=" + pkHatirlatma);

            MusteriRandevuListesi();

            RandevuListesi();

            //RandevuSekmeListeGetir();
        }

        private void randevuSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Degerler.isYenile = true;
            SecilenRandevuyuSil();   
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            Yazdir(false);
        }

        void Yazdir(bool dizayn)
        {
            //XtraReport rapor= new XtraReport();
            xrCariHareket rapor = new xrCariHareket();
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string pkHatirlatma = "0", fkFirma = "0";
                System.Data.DataSet ds = new DataSet("Test");
                if (gridView1.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    pkHatirlatma = dr["pkHatirlatma"].ToString();
                    fkFirma = dr["fkFirma"].ToString();
                    if (fkFirma == "") fkFirma = "0";
                }
                string sql = "select * from Hatirlatma with(nolock) where pkHatirlatma=" + pkHatirlatma;

                sql = @"select h.pkHatirlatma,h.Tarih, fkFirma,fkStokKarti,sk.Stokadi ,hd.durumu,h.Aciklama from Hatirlatma h with(nolock)
left join Firmalar f on f.pkFirma=h.fkFirma
left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
left join HatirlatmaDurum hd with(nolock) on hd.pkHatirlatmaDurum=h.fkDurumu
where  h.fkFirma=" + pkFirma.Text;

                if (!cbTumunuGoster.Checked)
                    sql = sql + " and h.fkDurumu=1";

                sql = sql + " order by h.Tarih";
                //rapor.DataSource = DB.GetData(sql);

                DataTable FisDetay = DB.GetData(sql);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Hatirlatma.repx");
                rapor.Name = "Hatirlatma";
                rapor.Report.Name = "Hatirlatma";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
            {
                if (cbYazdir.Checked)
                    rapor.Print();
                else
                    rapor.ShowPreview();
            }
        }

        void YazdirGunluk(bool dizayn)
        {
            //XtraReport rapor= new XtraReport();
            xrCariHareket rapor = new xrCariHareket();
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string pkHatirlatma = "0", fkFirma = "0";
                System.Data.DataSet ds = new DataSet("Test");
                if (gridView1.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    pkHatirlatma = dr["pkHatirlatma"].ToString();
                    fkFirma = dr["fkFirma"].ToString();
                    if (fkFirma == "") fkFirma = "0";
                }

                string sql = "hsp_SeansRandevuListesi '" + ilktarih.DateTime.ToString("yyyy-MM-dd") + "'";
                if (cbBosOlanlar.Checked)
                    sql = sql + ",1";
                else
                    sql = sql + ",null";

                DataTable FisDetay = DB.GetData(sql);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //
                //DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                //Cari.TableName = "Cari";
                //ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\HatirlatmaGunluk.repx");
                rapor.Name = "HatirlatmaGunluk";
                rapor.Report.Name = "HatirlatmaGunluk";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
            {
                if (checkEdit1.Checked)
                    rapor.Print();
                else
                    rapor.ShowPreview();
            }
        }

        private void yazdırTasarımToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            Yazdir(true);
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
          seanslarıSilToolStripMenuItem_Click( sender,  e);
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            YazdirGunluk(false);
        }

        private void günlükYazdırTasarımToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirGunluk(true);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            SecilenRandevuyuSil();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (dr["pkFirma"].ToString() == "") return;

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.ShowDialog();
        }

        private void durumunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
             string pkFirma = dr["pkFirma"].ToString();

             if (pkFirma == "") return;

             DB.pkHatirlatma = int.Parse(dr["pkHatirlatma"].ToString());
            frmHatirlatma Hatirlatma = new frmHatirlatma(DateTime.Today,DateTime.Today,int.Parse(pkFirma));
            Hatirlatma.Show();
        }

        private void pkSatislar_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt =  DB.GetData(@"select fkFirma,fkStokKarti from Satislar s with(nolock) 
            left join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar where s.pkSatislar=" + pkSatislar.Text);
            if (dt.Rows.Count > 0)
            {
                pkFirma.Text = dt.Rows[0]["fkFirma"].ToString();
                if (dt.Rows[0]["fkStokKarti"].ToString()!="")
                   lueStoklar.EditValue = int.Parse(dt.Rows[0]["fkStokKarti"].ToString());
            }
        }

        private void bekliyorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkHatirlatma = dr["pkHatirlatma"].ToString();

            DB.ExecuteSQL("update Hatirlatma set fkDurumu=1 where pkHatirlatma=" + pkHatirlatma);

            MusteriRandevuListesi();
            RandevuSekmeListeGetir();
        }

        private void geldiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkHatirlatma = dr["pkHatirlatma"].ToString();
            
            DB.ExecuteSQL("update Hatirlatma set fkDurumu=2 where pkHatirlatma=" + pkHatirlatma);

            MusteriRandevuListesi();
            RandevuSekmeListeGetir();
        }

        private void gelmediToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkHatirlatma = dr["pkHatirlatma"].ToString();

            DB.ExecuteSQL("update Hatirlatma set fkDurumu=3 where pkHatirlatma=" + pkHatirlatma);

            MusteriRandevuListesi();
            RandevuSekmeListeGetir();
        }

        private void cbBosOlanlar_CheckedChanged(object sender, EventArgs e)
        {
            RandevuListesi();
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            MusteriRandevuListesi();
        }

        private void simpleButton8_Click_1(object sender, EventArgs e)
        {
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "0";
            StokAra.ShowDialog();
            lueStoklar.Tag = "0";

            int secilen = 0;
            if (lueStoklar.EditValue != null)
                int.TryParse(lueStoklar.EditValue.ToString(), out secilen);
            Hizmetler();
            lueStoklar.EditValue = secilen;

            if (StokAra.TopMost == false)
            {
                

                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    string pkStokKarti = dr["pkStokKarti"].ToString();
                    int sk=0;
                    int.TryParse(pkStokKarti,out sk);
                    lueStoklar.EditValue = sk;
                }
            }
            StokAra.Dispose();
        }

        private void deRandevuZamani_EditValueChanged(object sender, EventArgs e)
        {
            string rafsure = seRafSure.Value.ToString();
            deBitisZamani.DateTime = deRandevuZamani.DateTime.AddMinutes(int.Parse(rafsure));
        }

        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr["fkFirma"].ToString() == "") return;

            MusteriHareketleriAc(dr["fkFirma"].ToString());
        }

        void MusteriHareketleriAc(string musteri_id)
        {
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = musteri_id;
            CariHareketMusteri.ShowDialog();

            MusteriBorcuGetir();
        }
        private void gridView3_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo ghi = gridView2.CalcHitInfo(e.Location);

            if (ghi.RowHandle == -999997) return;
            if (ghi.RowHandle == -2147483647) return;

            if (ghi.InRowCell)
            {
                int rowHandle = ghi.RowHandle;
                if (ghi.Column.FieldName == "pkHatirlatma")
                {
                    //zamansec();
                    simpleButton3_Click(sender, e);//oluştur
                }
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i= gridView1.FocusedRowHandle;
            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(i);

            string pkFirma = dr["fkFirma"].ToString();
            if (pkFirma == "") return;
            DB.pkHatirlatma = int.Parse(dr["pkHatirlatma"].ToString());
            frmHatirlatma Hatirlatma = new frmHatirlatma(DateTime.Today, DateTime.Today, int.Parse(pkFirma));
            Hatirlatma.ShowDialog();
            Degerler.isYenile = true;
            RandevuSekmeListeGetir();
            MusteriRandevuListesi();

            gridView1.FocusedRowHandle = i;
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            string secilen = "0";
            if (lueStoklar.EditValue != null)
            {
                secilen = lueStoklar.EditValue.ToString();
            }

            if (Degerler.StokKartiDizayn)
            {
                frmStokKartiLayout sk = new frmStokKartiLayout();
                DB.pkStokKarti = int.Parse(lueStoklar.EditValue.ToString());
                sk.pkStokKarti.Text = lueStoklar.EditValue.ToString();
                sk.ShowDialog();
            }
            else
            {
                frmStokKarti sk = new frmStokKarti();

                if (lueStoklar.EditValue == null)
                {
                    DB.pkStokKarti = 0;
                    sk.pkStokKarti.Text = "0";
                }
                else
                {
                    DB.pkStokKarti = int.Parse(lueStoklar.EditValue.ToString());
                    sk.pkStokKarti.Text = lueStoklar.EditValue.ToString();
                    secilen = sk.pkStokKarti.Text;
                }
                sk.ShowDialog();
            }
            lueStoklar.Tag = "0";

            Hizmetler();

            lueStoklar.EditValue = int.Parse(secilen);
        }

        private void gridView3_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            //Degerler.Renkler.Beyaz;
            if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "1")
                e.Appearance.BackColor = System.Drawing.Color.LightYellow;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "2")
                e.Appearance.BackColor = System.Drawing.Color.LightGreen;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "3")
                e.Appearance.BackColor = System.Drawing.Color.Red;

            //DataRow dr = gridView3.GetDataRow(e.RowHandle);
            //if (dr == null)
            //{
            //    return;
            //}
            //else if (e.Column.FieldName == "durumu" && dr["fkDurumu"].ToString() == "1")
            //    e.Appearance.BackColor = System.Drawing.Color.Yellow;
            //else if (e.Column.FieldName == "durumu" && dr["fkDurumu"].ToString() == "2")
            //    e.Appearance.BackColor = System.Drawing.Color.LightGreen;
            //else if (e.Column.FieldName == "durumu" && dr["fkDurumu"].ToString() == "3")
            //    e.Appearance.BackColor = System.Drawing.Color.Red;
        }

        void RandevuGorunumAyarla()
        {
            schedulerStorage1.Appointments.Mappings.Start = "StartTime";
            schedulerStorage1.Appointments.Mappings.End = "EndTime";
            schedulerStorage1.Appointments.Mappings.Subject = "Subject";
            schedulerStorage1.Appointments.Mappings.Description = "Description";
            schedulerStorage1.Appointments.Mappings.Location = "Cep";
            schedulerStorage1.Appointments.Mappings.Status = "fkDurumu";
            schedulerStorage1.Appointments.Mappings.Label = "Label";

            schedulerStorage1.Appointments.Mappings.ResourceId = "ResourceId";

            //05.05.2018 21:29
            //schedulerStorage1.Resources.Mappings.Id = "ResourceId";//"ResourceID";
            //schedulerStorage1.Resources.Mappings.Caption = "ResourceId";//"ResourceName";


            DevExpress.XtraScheduler.AppointmentCustomFieldMapping acfmCustomMappingID = new DevExpress.XtraScheduler.AppointmentCustomFieldMapping("pkHatirlatma", "pkHatirlatma");
            schedulerStorage1.Appointments.CustomFieldMappings.Add(acfmCustomMappingID);

            //schedulerStorage1.Appointments.CustomFieldMappings.Add(new AppointmentCustomFieldMapping("ApptImage1", "Icon1", FieldValueType.Object));
            DevExpress.XtraScheduler.AppointmentCustomFieldMapping acfmCustomMappingArandi = new DevExpress.XtraScheduler.AppointmentCustomFieldMapping("arandi", "arandi");
            schedulerStorage1.Appointments.CustomFieldMappings.Add(acfmCustomMappingArandi);

        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //Degerler.isYenile = true;
            RandevuSekmeListeGetir();
        }
        
        void GorunumVerileriGetir()
        {
            //string s =  schedulerStorage1.Appointments.DataMember;
            //object o = schedulerStorage1.Appointments;
            //schedulerControl1.SelectedAppointments.Add(schedulerStorage1.Appointments)
            string Sql = "";
//            @"select pkHatirlatma,f.Firmaadi+' - '+sk.Stokadi+' - '+ISNULL(Aciklama,'.') as [Subject],Tarih as StartTime,
//            BitisTarihi as EndTime,Konu as [Description],0 as AllDay,fkfirma,fkDurumu,
//case when fkDurumu=1 then 10
//when fkDurumu=2 then 8
//when fkDurumu=3 then 5 end Label1,fkDurumu as Label,f.Cep,h.arandi,fkOda as ResourceId from Hatirlatma h with(nolock)
//            left join Firmalar f with(nolock) on pkFirma=h.fkFirma
//            left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
//            where Tarih>=DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE()),0)";

            //05.05.2018 23:59 kapattım
            //if (lueOdalar.EditValue!=null || lueOdalar.EditValue.ToString() != "0")
            //    Sql = Sql + " and H.fkOda=" + lueOdalar.EditValue.ToString();


            //MessageBox.Show("randevuları görünümlü");
            //this.Text = dateNavigator1.SelectionStart.ToString() + "-" + dateNavigator1.SelectionEnd.ToString();

            //Sql = Sql + " where Tarih>='" + dateNavigator1.SelectionStart.ToString("yyyy-MM-dd")+"'";
            //Sql = Sql + " and  Tarih<='" + dateNavigator1.SelectionEnd.ToString("yyyy-MM-dd") + "'";


            //where Tarih>='@ilktarih' --and Tarih<='@sontarih'";

            //Sql = Sql.Replace("@ilktarih", deTarihi.DateTime.AddDays(-30).ToString("yyy-MM-dd 00:00");
            //Sql = Sql.Replace("@sontarih", deTarihi.DateTime.ToString("yyy-MM-dd 23:59"));

            IDateNavigatorControllerOwner control = (IDateNavigatorControllerOwner)dateNavigator1;
            
            if (ilktarih.DateTime != control.StartDate || Degerler.isYenile) 
            {
                Degerler.isYenile = false;
                //ilktarih.DateTime = control.StartDate;
                //sontarih.DateTime = control.EndDate;

                //if (lueOdalar.EditValue != null || lueOdalar.EditValue.ToString() != "0")
                //    Sql = "HSP_HatirlatmaListesiGorunum '" + control.StartDate.ToString("yyyy-MM-dd 00:00") + "','" + 
                //        control.EndDate.ToString("yyyy-MM-dd 23:59") + "'," + lueOdalar.EditValue.ToString();
                //else

                //MessageBox.Show(Sql);
                //btnYenile.ToolTip = Sql;
                try
                {
                    Sql = "HSP_HatirlatmaListesiGorunum '" + control.StartDate.ToString("yyyy-MM-dd 00:00") + "','" + control.EndDate.ToString("yyyy-MM-dd 23:59") + "',null";
                    schedulerStorage1.Appointments.DataSource = DB.GetData(Sql);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Hata=" + exp.Message);
                    //throw;
                }
                
                //DataTable dt = DB.GetData(Sql);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                    
                //    Appointment app = schedulerStorage1.CreateAppointment(AppointmentType.Normal);
                //    app.Start = DateTime.Now.AddHours(i-2).AddMinutes(-30);
                //    app.End = DateTime.Now.AddHours(i-2);
                //    app.Subject = dt.Rows[i]["ResourceId"].ToString();//string.Format("Appointment{0}", i);
                //    app.ResourceId = 0;//dt.Rows[i]["ResourceId"].ToString();
                //    schedulerStorage1.Appointments.Add(app);
                //}
                //Degerler.isYenile = false;
               // seRafSure.Value = seRafSure.Value + 1;
            }
            //MessageBox.Show("veri");
            
            //for (int i = 0; i < schedulerStorage1.Appointments.Count; i++)
            //{
            //    if (schedulerStorage1.Appointments[i].CustomFields[0].ToString() == txtPkHatirlatma.Text)
            //    {
            //        schedulerControl1.SelectedAppointments.Add(schedulerStorage1.Appointments[i]);
            //        txtPkHatirlatma.Tag = "0";
            //        return;
            //    }
            //}


            //Appointment apt = schedulerStorage1.Appointments[int.Parse(txtPkHatirlatma.Tag.ToString())];
            //schedulerControl1.SelectedAppointments.Add(apt);
            //txtPkHatirlatma.Tag = "0";
        }

        private void repositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            DB.ExecuteSQL("update Hatirlatma set fkDurumu=" + value + " where pkHatirlatma=" + dr["pkHatirlatma"].ToString());

        }

        private void seRafSure_EditValueChanged(object sender, EventArgs e)
        {
            string rafsure = seRafSure.Value.ToString();
            deBitisZamani.DateTime = deRandevuZamani.DateTime.AddMinutes(int.Parse(rafsure));
            simpleButton10.Visible = true;
        }

        private void schedulerStorage1_AppointmentChanging(object sender, DevExpress.XtraScheduler.PersistentObjectCancelEventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = e.Object as DevExpress.XtraScheduler.Appointment;

            if (apt.CustomFields["pkHatirlatma"].ToString() != "")
            {
                //çift tık tetikle
                //DevExpress.XtraScheduler.AppointmentFormEventArgs e
                //schedulerControl1_EditAppointmentFormShowing(sender, ?);

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@StartTime", apt.Start));
                list.Add(new SqlParameter("@EndTime", apt.End));
                list.Add(new SqlParameter("@pkHatirlatma", apt.CustomFields["pkHatirlatma"].ToString()));
                list.Add(new SqlParameter("@fkDurumu", apt.LabelId));
                DB.ExecuteSQL("update Hatirlatma set fkDurumu=@fkDurumu,Tarih=@StartTime,BitisTarihi=@EndTime where pkHatirlatma=@pkHatirlatma", list);
                Degerler.isYenile = true;

                RandevuBilgileriGetir(apt.CustomFields["pkHatirlatma"].ToString());
                MusteriRandevuListesi();

            }
        }

        private void schedulerControl1_EditAppointmentFormShowing(object sender, DevExpress.XtraScheduler.AppointmentFormEventArgs e)
        {
            if (e.Appointment.CustomFields["pkHatirlatma"] == null)
            {
                DB.pkHatirlatma = 0;
                deRandevuZamani.DateTime = e.Appointment.Start;
                //deBitisZamani.DateTime = e.Appointment.End;
                string oda = e.Appointment.ResourceId.ToString();
                if (oda == "") oda = "0";
                lueOdalar.EditValue = int.Parse(oda);
                string sure = seRafSure.Value.ToString();
                deBitisZamani.DateTime = deRandevuZamani.DateTime.AddMinutes(int.Parse(sure));
                //oluştur
                simpleButton3_Click(sender, e);
            }
            else
            {
                string id = e.Appointment.CustomFields["pkHatirlatma"].ToString();
                DB.pkHatirlatma = int.Parse(id);
                RandevuBilgileriGetir(id);

                RandevuSec(id);
            }

            e.Handled = true;
        }

        void RandevuBilgileriGetir(string hatirlatma_id)
        {
            DataTable dtRandevu = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + hatirlatma_id);
            if (dtRandevu.Rows.Count > 0)
            {
                txtPkHatirlatma.Text = hatirlatma_id;
                pkFirma.Text = "0";
                string firma_id = dtRandevu.Rows[0]["fkFirma"].ToString();
                //string Stok_id = dtRandevu.Rows[0]["fkStokKarti"].ToString();
                //string tarih = dtRandevu.Rows[0]["Tarih"].ToString();
                //string bitis_tarihi = dtRandevu.Rows[0]["BitisTarihi"].ToString();

                pkFirma.Text = firma_id;
                //if (Stok_id == "") Stok_id = "0";
                //lueStoklar.EditValue = int.Parse(Stok_id);

                //if (tarih == "")
                //    deRandevuZamani.DateTime = DateTime.Now;
                //else
                //    deRandevuZamani.DateTime = Convert.ToDateTime(tarih);

                //if (tarih == "")
                //    deBitisZamani.DateTime = DateTime.Now;
                //else
                //    deBitisZamani.DateTime = Convert.ToDateTime(bitis_tarihi);


                //tbAciklama.Text = dtRandevu.Rows[0]["Aciklama"].ToString();

                //simpleButton3.Text = "Randevu Güncelle";
            }
        }

        void RandevuSec(string hatirlatma_id)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                if (gridView1.GetDataRow(i)["pkHatirlatma"].ToString() == hatirlatma_id)
                {
                    gridView1.FocusedRowHandle = i;
                    gridView1.SelectCell(i,gridColumn6);
                }
            }
        }

        private void schedulerStorage1_AppointmentDeleting(object sender, DevExpress.XtraScheduler.PersistentObjectCancelEventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = (DevExpress.XtraScheduler.Appointment)e.Object;
            //string id = apt.Location.ToString();
            string id = apt.CustomFields["pkHatirlatma"].ToString();
            string sonuc = formislemleri.MesajBox("Randevu Silmek İstediğinize Eminmisiniz?", "Randevu Sil", 3, 2);
            if (sonuc == "0")
            {
                e.Cancel = true;
                return;
            }
            DB.ExecuteSQL("delete from Hatirlatma where pkHatirlatma=" + id);

            e.Cancel = false;
            Degerler.isYenile = true;
            RandevuSekmeListeGetir();
            MusteriRandevuListesi();
        }

        private void labelControl18_Click(object sender, EventArgs e)
        {
            string fkFirma=pkFirma.Text;
            frmMusteriKarti mk = new frmMusteriKarti(fkFirma, "");
            mk.ShowDialog();
            pkFirma.Text = "0";
            pkFirma.Text = fkFirma;
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (lueStoklar.EditValue == null) return;

            DB.ExecuteSQL("update stokkarti set RafSure=" + seRafSure.Value.ToString() +
               " where pkStokKarti=" + lueStoklar.EditValue.ToString());

            simpleButton10.Visible = false;
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            düzenleToolStripMenuItem_Click(sender, e);
        }
        GridHitInfo downHitInfo;
        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            downHitInfo = null;

            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None)
                return;

            if (e.Button == MouseButtons.Left && hitInfo.InRow && hitInfo.HitTest != GridHitTest.RowIndicator)
                downHitInfo = hitInfo;
        }

        private void gridView1_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    view.GridControl.DoDragDrop(GetDragData(view), DragDropEffects.All);
                    downHitInfo = null;
                }
            }
        }

        SchedulerDragData GetDragData(GridView view)
        {
            int[] selection = view.GetSelectedRows();
            if (selection == null)
                return null;

            AppointmentBaseCollection appointments = new AppointmentBaseCollection();
            int count = selection.Length;
            for (int i = 0; i < count; i++)
            {
                int rowIndex = selection[i];
                Appointment apt = schedulerStorage1.CreateAppointment(AppointmentType.Normal);
                apt.Subject = view.GetRowCellValue(rowIndex, "pkHatirlatma").ToString();
                apt.LabelId = (int)view.GetRowCellValue(rowIndex, "fkFirma");
                //apt.StatusId = (int)view.GetRowCellValue(rowIndex, "durumu");
                apt.CustomFields["pkHatirlatma"] = view.GetRowCellValue(rowIndex, "pkHatirlatma").ToString();
                apt.Location = view.GetRowCellValue(rowIndex, "fkStokKarti").ToString();
                //apt.Duration = TimeSpan.FromHours((int)view.GetRowCellValue(rowIndex, "fkStokKarti"));
                apt.Description = (string)view.GetRowCellValue(rowIndex, "Aciklama");
                appointments.Add(apt);
            }

            return new SchedulerDragData(appointments, 0);
        }

        private void schedulerControl1_AppointmentDrop(object sender, AppointmentDragEventArgs e)
        {
            //string sonuc = formislemleri.MesajBox("Randevu Silmek İstediğinize Eminmisiniz?", "Randevu Sil", 3, 2);
            //if (sonuc == "0")
            //{
            //    //e.Cancel = true;
            //    return;
            //}

            //string createEventMsg = "Randevu Zamanı  {0} - {1}.";
            //string moveEventMsg = "Yeni Randevu Zamanı {0} - {1} dan {2} - {3}.";

            DateTime srcStart = e.SourceAppointment.Start;
            DateTime newStart = e.EditedAppointment.Start;

            TimeSpan ts = newStart-srcStart;
            //int saat = ts.Hours;
            double dakika = ts.TotalMinutes;
            if (dakika < 0) dakika = dakika * -1;
            
            //string msg = (srcStart == DateTime.MinValue) ? String.Format(createEventMsg, newStart.ToShortTimeString(), newStart.ToShortDateString()) :
            //   String.Format(moveEventMsg, srcStart.ToShortTimeString(), srcStart.ToShortDateString(), newStart.ToShortTimeString(), newStart.ToShortDateString());
            
            string msg = newStart.ToString("dd MMMM yyyy HH:mm");
            if (dakika >= int.Parse(RandevuSaatAraligi))
            {
                if (XtraMessageBox.Show(msg + " olarak değiştirilsin mi?", "Randevu Zamanı Değiştirilsin mi?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    e.Allow = false;
                    //e.Handled = true;
                    if (e.EditedAppointment != null)
                    {
                        DB.ExecuteSQL("Update Hatirlatma set Tarih='" + e.EditedAppointment.Start.ToString("yyyy-MM-dd HH:mm") +
                            "',fkOda="+ e.EditedAppointment.ResourceId.ToString() +
                            ",BitisTarihi='" + e.EditedAppointment.End.ToString("yyyy-MM-dd HH:mm") + "' where pkHatirlatma=" +
                            e.SourceAppointment.CustomFields["pkHatirlatma"].ToString());
                    }
                    Degerler.isYenile = true;
                    MusteriRandevuListesi();
                    //RandevuSekmeListeGetir();
                }
                else
                {
                    e.Allow = false;
                    e.Handled = true;
                    //schedulerControl1.RefreshData();
                    //RandevuSekmeListeGetir();
                }
            }
            else
            {
                //e.Allow = false;
                e.Handled = true;
                if (e.EditedAppointment != null)
                {
                    DB.ExecuteSQL("Update Hatirlatma set Tarih='" + e.EditedAppointment.Start.ToString("yyyy-MM-dd HH:mm") +
                        "',BitisTarihi='" + e.EditedAppointment.End.ToString("yyyy-MM-dd HH:mm") + "' where pkHatirlatma=" +
                        e.SourceAppointment.CustomFields["pkHatirlatma"].ToString());
                }

                if (e.EditedAppointment.IsRecurring)
                    e.Allow = DropRecurringAppointment(e.SourceAppointment.RecurrencePattern, e.EditedAppointment.Start);
                else
                    e.Allow = DropNormalAppointment(e.EditedAppointment, e.EditedAppointment.Start, e.SourceAppointment.Start);

                Degerler.isYenile = true;
                MusteriRandevuListesi();
                RandevuSekmeListeGetir();
            }
        }

        private bool DropNormalAppointment(Appointment appointment, DateTime newStart, DateTime srcStart)
        {
            string createEventMsg = "Creating an event on {0:D} at {1:t}.";
            string moveEventMsg = "Moving the event \r\nscheduled on {0:D} at {1:T}\r\nto {2:dddd, dd MMM yyyy HH:mm:ss }.";
            string msg = (srcStart == DateTime.MinValue) ? String.Format(createEventMsg, newStart.Date, newStart.TimeOfDay) :
                String.Format(moveEventMsg, srcStart.Date, srcStart.TimeOfDay, newStart);
            if (MessageBox.Show(msg + " Proceed?", "Confirm the action", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                appointment.Subject += "\r\ndatetime modified";
                return true;
            }
            return false;
        }
        private bool DropRecurringAppointment(Appointment pattern, DateTime newStart)
        {
            DialogResult result = MessageBox.Show("Should the entire series follow the appointment?", "Confirm the action", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                pattern.DeleteExceptions();
                pattern.RecurrenceInfo.Start = newStart;
            }
            else
                if (result == DialogResult.No)
                return true;

            return false;
        }

        private void frmRandevuVer_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
                //Close();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
           RandevuSekmeListeGetir();
        }

        void GetirRandevu()
        {
            DataTable dtRandevu= 
            DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + txtPkHatirlatma.Text);
            if (dtRandevu.Rows.Count == 0)
            {
                //formislemleri.Mesajform("Hatırlatma Bulunamadı","K",150);
                return;
            }

            tbAciklama.Text = dtRandevu.Rows[0]["Aciklama"].ToString();

            if (dtRandevu.Rows[0]["Tarih"].ToString()!="")
               deRandevuZamani.DateTime=  Convert.ToDateTime(dtRandevu.Rows[0]["Tarih"].ToString());

            if (dtRandevu.Rows[0]["BitisTarihi"].ToString() != "")
                deBitisZamani.DateTime = Convert.ToDateTime(dtRandevu.Rows[0]["BitisTarihi"].ToString());

            if (dtRandevu.Rows[0]["fkStokKarti"].ToString() != "")
            {
                Hizmetler();
                lueStoklar.EditValue = int.Parse(dtRandevu.Rows[0]["fkStokKarti"].ToString());
            }
            //firma bilgilerini Getir
            if (dtRandevu.Rows[0]["fkFirma"].ToString() != "")
            {
                pkFirma.Text = dtRandevu.Rows[0]["fkFirma"].ToString();
                //DB.PkFirma = pkFirma.Text;
                
                //FirmaBilgileriGetir(dtRandevu.Rows[0]["fkFirma"].ToString());
            }
        }

        void FirmaBilgileriGetir(string fkFirma)
        {
            DataTable dtFirma = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + fkFirma);
            if (dtFirma.Rows.Count == 0)
            {
                formislemleri.Mesajform("Firma Bulunamadı", "K", 150);
                return;
            }

              teCariAdi.Text=dtFirma.Rows[0]["Firmaadi"].ToString();
        }

        private void txtPkHatirlatma_EditValueChanged(object sender, EventArgs e)
        {
            GetirRandevu();

            if (txtPkHatirlatma.Text == "0" || txtPkHatirlatma.Text == "")
                simpleButton3.Text = "Randevu Oluştur";
            else
                simpleButton3.Text = "Güncelle";

            txtPkHatirlatma.Tag = "0";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["pkHatirlatma"].ToString() == txtPkHatirlatma.Text)
                {
                    gridView1.FocusedRowHandle = i;
                    txtPkHatirlatma.Tag = i;
                    return;
                }
            }
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {

            #region Uyarılar
            if (pkFirma.Text == "0" || pkFirma.Text == "")
            {
                formislemleri.Mesajform("Müşteri Seçiniz!", "K", 100);
                return;
            }
            if (lueStoklar.EditValue == null)
            {
                formislemleri.Mesajform("Hizmet Seçiniz!", "K", 100);
                return;
            }
            //TarihKontrol();
            #endregion

            //string sonuc = formislemleri.MesajBox(teCariAdi.Text + "-" + deRandevuZamani.Text + "-" + lueStoklar.Text + " Randevu Oluşturulacak Eminmisiniz", "Randevu Oluştur", 3, 1);
            //if (sonuc == "0") return;

            //int maxseans = 0;
            //DataTable dtSeansVarmi = DB.GetData("select isnull(sira_no,1) as sira_no from Hatirlatma with(nolock) where fkDurumu=1 and fkFirma=" +
              //  pkFirma.Text + " and fkStokKarti=" + lueStoklar.EditValue.ToString() + " order by sira_no desc");
            //if (dtSeansVarmi.Rows.Count > 0)
            //{
            //    maxseans = int.Parse(dtSeansVarmi.Rows[0][0].ToString());
            //}
            //maxseans = maxseans + 1;

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Tarih", deRandevuZamani.DateTime));
            list.Add(new SqlParameter("@fkFirma", pkFirma.Text));
            list.Add(new SqlParameter("@fkStokKarti", lueStoklar.EditValue));
            list.Add(new SqlParameter("@fkDurumu", "1"));//1-Bekliyor,2-Geldi,3-Gelmedi
            list.Add(new SqlParameter("@BitisTarihi", deBitisZamani.DateTime));
            list.Add(new SqlParameter("@Aciklama", tbAciklama.Text));
            list.Add(new SqlParameter("@Konu", "Randevu Seans"));
            list.Add(new SqlParameter("@pkHatirlatma", txtPkHatirlatma.Text));

            string s_sonuc = DB.ExecuteSQL("update Hatirlatma set Tarih=@Tarih,fkFirma=@fkFirma,fkStokKarti=@fkStokKarti,fkDurumu=@fkDurumu,BitisTarihi=@BitisTarihi,Aciklama=@Aciklama,Konu=@Konu where pkHatirlatma=@pkHatirlatma", list);
            if (s_sonuc.Substring(0, 1) == "H")
            {
                formislemleri.Mesajform(s_sonuc, "K", 150);
            }
            
            MusteriRandevuListesi();
            RandevuSekmeListeGetir();

            tbAciklama.Text = "";
            //fkHatirlatma.Text = "0";
            //ilkSeans();
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            simpleButton3.Text = "Randevu Oluştur";
            txtPkHatirlatma.Text = "0";
            pkFirma.Text = "0";
            tbAciklama.Text = "";
            lueStoklar.EditValue = 0;
            teCariAdi.Text = "";
            pkSatislar.Text = "0";
            lbBakiye.Text = "0.00";
            teCariAdi.Focus();
            deRandevuZamani.DateTime = DateTime.Now;
            //btnCari_Click(sender, e);
            //ilkSeans();
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
           
        }

        private void deBitisZamani_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = dateNavigator1.DateTime.AddDays(1);
            //deTarihi.DateTime = dateNavigator1.DateTime;
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = DateTime.Today;
            //deTarihi.DateTime = dateNavigator1.DateTime;
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = dateNavigator1.DateTime.AddDays(-1);
            //deTarihi.DateTime = dateNavigator1.DateTime;
        }

        private void schedulerControl1_CustomDrawAppointment(object sender, CustomDrawObjectEventArgs e)
        {
            if (((SchedulerControl)sender).ActiveView is DayView)
            {
                AppointmentViewInfo viewInfo = e.ObjectInfo as AppointmentViewInfo;
                // Get DevExpress images.
                //Image im = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/add_16x16.png");

                //Rectangle imageBounds = new Rectangle(viewInfo.InnerBounds.X, viewInfo.InnerBounds.Y, im.Width, im.Height);
                //Rectangle mainContentBounds = new Rectangle(viewInfo.InnerBounds.X, viewInfo.InnerBounds.Y + im.Width + 1,
                //viewInfo.InnerBounds.Width, viewInfo.InnerBounds.Height - im.Height - 1);
                //// Draw image in the appointment.
                //e.Cache.Graphics.DrawImage(im, imageBounds);
            }
        }

        private void schedulerControl1_PopupMenuShowing(object sender, DevExpress.XtraScheduler.PopupMenuShowingEventArgs e)
        {
            if (e.Menu.Id == DevExpress.XtraScheduler.SchedulerMenuItemId.AppointmentMenu)
            {
                // Hide the "Change View To" menu item.
                //DevExpress.XtraScheduler.SchedulerPopupMenu itemChangeViewTo = e.Menu.GetPopupMenuById(DevExpress.XtraScheduler.SchedulerMenuItemId.SwitchViewMenu);
                //itemChangeViewTo.Visible = false;

                //e.Menu.RemoveMenuItem(DevExpress.XtraScheduler.SchedulerMenuItemId.NewRecurringEvent);
                e.Menu.RemoveMenuItem(DevExpress.XtraScheduler.SchedulerMenuItemId.OpenAppointment);

            }
            
            DevExpress.Utils.Menu.DXMenuItem item = new DevExpress.Utils.Menu.DXMenuItem("Sms Gönder", new EventHandler(item_Click_sms_gonder));
            e.Menu.Items.Insert(0, item);

            //DevExpress.Utils.Menu.DXMenuItem iteme = new DevExpress.Utils.Menu.DXMenuItem("E-Posta Gönder", new EventHandler(item_Click_mail_gonder));
            //e.Menu.Items.Insert(0, iteme);

            DevExpress.Utils.Menu.DXMenuItem itemmk = new DevExpress.Utils.Menu.DXMenuItem("Müşteri Kartı", new EventHandler(item_Click_musteri_karti));
            e.Menu.Items.Insert(0, itemmk);

            DevExpress.Utils.Menu.DXMenuItem item2 = new DevExpress.Utils.Menu.DXMenuItem("Müşteri Hareketleri", new EventHandler(item_Click_musteri_hareketleri));
            e.Menu.Items.Insert(0, item2);

            //DevExpress.Utils.Menu.DXMenuItem item0 = new DevExpress.Utils.Menu.DXMenuItem("Randevu Düzenle", new EventHandler(item_Click_RandevuDuzenle));
            //System.Drawing.Bitmap bitmap = GPTS.Properties.Resources.edit16x16;
            //item0.Image = bitmap;
            //e.Menu.Items.Insert(0, item0);




            DevExpress.Utils.Menu.DXMenuItem itemCopy = new DevExpress.Utils.Menu.DXMenuItem("Kopyala", new EventHandler(item_Click_Copy));
            System.Drawing.Bitmap bitmap1 = GPTS.Properties.Resources.Copy16x16;
            itemCopy.Image = bitmap1;
            //itemCopy.Shortcut = new Shortcut();
            //itemCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlK;
            e.Menu.Items.Insert(0, itemCopy);

            DevExpress.Utils.Menu.DXMenuItem itemCut = new DevExpress.Utils.Menu.DXMenuItem("Kes", new EventHandler(item_Click_Cut));
            System.Drawing.Bitmap bitmap2 = GPTS.Properties.Resources.cut;
            itemCut.Image = bitmap2;
            e.Menu.Items.Insert(0, itemCut);

            DevExpress.Utils.Menu.DXMenuItem itemPaste = new DevExpress.Utils.Menu.DXMenuItem("Yapıştır", new EventHandler(item_Click_Paste));
            System.Drawing.Bitmap bitmappas = GPTS.Properties.Resources.Paste16x16;
            itemPaste.Image = bitmappas;
            e.Menu.Items.Insert(0, itemPaste);

            DevExpress.Utils.Menu.DXMenuItem itemYeni = new DevExpress.Utils.Menu.DXMenuItem("Yeni Özel Randevu", new EventHandler(item_Click_OzelRandevuEkle));
            System.Drawing.Bitmap bitmapy = GPTS.Properties.Resources.New___2;
            itemYeni.Image = bitmapy;
            e.Menu.Items.Insert(0, itemYeni);

            DevExpress.Utils.Menu.DXMenuItem itemA = new DevExpress.Utils.Menu.DXMenuItem("Arandı Yap", new EventHandler(item_Click_Arandi));
            System.Drawing.Bitmap bitmapa = GPTS.Properties.Resources.Black_pin;
            itemA.Image = bitmapa;
            e.Menu.Items.Insert(0, itemA);

            DevExpress.Utils.Menu.DXMenuItem itemAc = new DevExpress.Utils.Menu.DXMenuItem("Randevu Düzenle", new EventHandler(item_Click_RandevuAc));
            System.Drawing.Bitmap bitmapdg = GPTS.Properties.Resources.edit;
            itemAc.Image = bitmapdg;
            e.Menu.Items.Insert(0, itemAc);
        }

        void item_Click_OzelRandevuEkle(object sender, EventArgs e)
        {
            DateTime secilentarih = schedulerControl1.Start;
            DB.pkHatirlatma = 0;
            frmHatirlatma Hatirlat = new frmHatirlatma(schedulerControl1.SelectedInterval.Start, schedulerControl1.SelectedInterval.End, 0);
            Hatirlat.ShowDialog();
            Degerler.isYenile = true;
            RandevuSekmeListeGetir();

            schedulerControl1.Start = secilentarih;
        }

        void item_Click_Copy(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }
            formislemleri.Mesajform("Randevu Kopyalandı", "S", 50);
            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma_Copy = int.Parse(id);
            DB.pkHatirlatma_Cut = 0;
        }

        void item_Click_Cut(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }
            formislemleri.Mesajform("Randevu Kesildi", "M", 50);
            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma_Cut = int.Parse(id);
            DB.pkHatirlatma_Copy = 0;
        }

        void item_Click_Paste(object sender, EventArgs e)
        {
            //hafızada varsa 
            if (DB.pkHatirlatma_Cut > 0)
            {
                DataTable dt = DB.GetData("select *,DATEDIFF(MINUTE,Tarih,BitisTarihi) as Saat from Hatirlatma with(nolock) where pkHatirlatma=" + DB.pkHatirlatma_Cut);

                string sonuc = formislemleri.MesajBox("Randevu Taşımak istediğinize eminmisiniz?", "Randevu Değiştir", 3, 0);
                if (sonuc == "0" || dt.Rows.Count==0) return;

                //string konu = dt.Rows[0]["Konu"].ToString();
                //string fkFirma = dt.Rows[0]["fkFirma"].ToString();
                //string fkStokKarti = dt.Rows[0]["fkStokKarti"].ToString();
                string Saat = dt.Rows[0]["Saat"].ToString();
                if (Saat == "") Saat = "30";
                if (Saat == "0") Saat = "30";

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", schedulerControl1.SelectedInterval.Start));
                //list.Add(new SqlParameter("@Konu", konu));
                //list.Add(new SqlParameter("@Aciklama", "Kopya"));
                //list.Add(new SqlParameter("@Uyar", "1"));
                list.Add(new SqlParameter("@BitisTarihi", schedulerControl1.SelectedInterval.Start.AddMinutes(int.Parse(Saat))));
                //list.Add(new SqlParameter("@fkFirma", fkFirma));
                //list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                //list.Add(new SqlParameter("@fkDurumu", "1"));
                list.Add(new SqlParameter("@pkHatirlatma", DB.pkHatirlatma_Cut));

                DB.GetData("update Hatirlatma set Tarih=@Tarih,BitisTarihi=@BitisTarihi where pkHatirlatma=@pkHatirlatma", list);
                Degerler.isYenile = true;
                RandevuSekmeListeGetir();
                MusteriRandevuListesi();
                return;
            }

            if (DB.pkHatirlatma_Copy > 0)
            {
                DataTable dt = DB.GetData("select *,DATEDIFF(MINUTE,Tarih,BitisTarihi) as sure from Hatirlatma with(nolock) where pkHatirlatma=" + DB.pkHatirlatma_Copy);
                
                string sonuc = formislemleri.MesajBox("Yeni Randevu Yapıştırılsın mı?", "Randevu Değiştir", 3, 0);
                if (sonuc == "0" || dt.Rows.Count == 0) return;

                string konu = dt.Rows[0]["Konu"].ToString();
                string fkFirma = dt.Rows[0]["fkFirma"].ToString();
                string fkStokKarti = dt.Rows[0]["fkStokKarti"].ToString();
                string Aciklama = dt.Rows[0]["Aciklama"].ToString();

                string fkOda = dt.Rows[0]["fkOda"].ToString();

                //if(lueOdalar.EditValue != null)
                fkOda = lueOdalar.EditValue.ToString();

                string sure = dt.Rows[0]["sure"].ToString();
                int isure = 30;
                int.TryParse(sure, out isure);

                if (Aciklama == "") Aciklama = ".";

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", schedulerControl1.SelectedInterval.Start));
                list.Add(new SqlParameter("@Konu", konu));
                list.Add(new SqlParameter("@Aciklama", Aciklama));
                list.Add(new SqlParameter("@Uyar", "1"));
                list.Add(new SqlParameter("@BitisTarihi", schedulerControl1.SelectedInterval.Start.AddMinutes(isure)));//schedulerControl1.SelectedInterval.End));
                list.Add(new SqlParameter("@fkFirma", fkFirma));
                list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                list.Add(new SqlParameter("@fkDurumu", "1"));
                list.Add(new SqlParameter("@fkOda", fkOda));

                DB.GetData("insert into Hatirlatma (Tarih,Konu,Aciklama,Uyar,BitisTarihi,fkFirma,fkStokKarti,fkDurumu,fkOda)" +
                    " values(@Tarih,@Konu,@Aciklama,@Uyar,@BitisTarihi,@fkFirma,@fkStokKarti,@fkDurumu,@fkOda)", list);

                Degerler.isYenile = true;
                RandevuSekmeListeGetir();
                MusteriRandevuListesi();
            }


        }

        void item_Click_RandevuAc(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma = int.Parse(id);
            //DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

            DateTime secilentarih = schedulerControl1.Start;

            frmHatirlatma Hatirlat = new frmHatirlatma(secilentarih, secilentarih, 0);
            Hatirlat.ShowDialog();
            Degerler.isYenile = true;
            RandevuSekmeListeGetir();

            schedulerControl1.Start = secilentarih;
        }

        void item_Click_RandevuDuzenle(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);

            //DevExpress.XtraScheduler.Appointment apt = null;
            //for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            //{
            //    apt = schedulerControl1.SelectedAppointments[i];
            //}

            //if (apt == null)
            //{
            //    formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
            //    return;
            //}

            //string id = apt.CustomFields["pkHatirlatma"].ToString();
            //frmRandevuVer rv = new frmRandevuVer();
            //rv.txtPkHatirlatma.Text = id;
            //rv.ShowDialog();
        }

        void item_Click_Arandi(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            if(DB.GetData("select pkHatirlatma from Hatirlatma with(nolock) where arandi=1 and pkHatirlatma=" + id).Rows.Count==0)
               DB.ExecuteSQL("update Hatirlatma  set arandi=1 where pkHatirlatma=" + id);
            else
                DB.ExecuteSQL("update Hatirlatma  set arandi=0 where pkHatirlatma=" + id);

            Degerler.isYenile = true;
            RandevuSekmeListeGetir();

        }

        void item_Click_sms_gonder(object sender, EventArgs e)
        {
            //int h = 0;
            //DataTable dt = DB.GetData("select pkFirma,Cep,Cep2 from Firmalar with(nolock) where sec=1 and (len(Cep)>9 or len(Cep2)>9)");//pkFirma=" + pkFirma);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string fkFirma = dt.Rows[i]["pkFirma"].ToString();
            //    string cep = dt.Rows[i]["Cep"].ToString();
            //    string cep2 = dt.Rows[i]["Cep2"].ToString();
            //    if (cep.Length < 9) cep = cep2;

            //    if (cep.Length < 9)
            //    {
            //        h++;
            //        continue;
            //    }

            //    ArrayList list = new ArrayList();
            //    list.Add(new SqlParameter("@fkFirma", fkFirma));
            //    list.Add(new SqlParameter("@CepTel", cep));
            //    list.Add(new SqlParameter("@Mesaj", "Bakiye"));

            //    if (DB.GetData("select fkFirma from Sms where Durumu=0 and fkFirma=" + fkFirma).Rows.Count == 0)
            //    {
            //        string sonuc = DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CepTel,Durumu,Mesaj,Tarih) values(@fkFirma,@CepTel,0,@Mesaj,GetDate())", list);
            //        if (sonuc == "0")
            //            DB.ExecuteSQL("update Firmalar set sec=0 where pkFirma=" + fkFirma);
            //    }
            //    else
            //        DB.ExecuteSQL("update Firmalar set sec=0 where pkFirma=" + fkFirma);
            //}

            frmSmsGonder SmsGonder = new frmSmsGonder();
            SmsGonder.Show();
        }

        void item_Click_mail_gonder(object sender, EventArgs e)
        {
            frmEpostaGonder mailGonder = new frmEpostaGonder();
            mailGonder.Show();
        }

        void item_Click_musteri_karti(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma = int.Parse(id);
            DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

            if (dt.Rows.Count == 0) return;

            string fkfirma = dt.Rows[0]["fkfirma"].ToString();
            frmMusteriKarti mh = new frmMusteriKarti(fkfirma, "");
            mh.ShowDialog();

            //MusteriRandevuListesi();
            RandevuSekmeListeGetir();
        }

        void item_Click_musteri_hareketleri(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatma"].ToString();
            DB.pkHatirlatma = int.Parse(id);
            DataTable dt = DB.GetData("select * from Hatirlatma with(nolock) where pkHatirlatma=" + id);

            if (dt.Rows.Count == 0) return;

            string fkfirma = dt.Rows[0]["fkfirma"].ToString();
            frmMusteriHareketleri mh = new frmMusteriHareketleri();
            mh.musteriadi.Tag = fkfirma;
            mh.ShowDialog();
        }

        private void simpleButton18_Click_1(object sender, EventArgs e)
        {
            //DateTime date = (dateNavigator1 as IDateNavigatorControllerOwner).StartDate;
            //if (date != DateTime.Today)
            {
                dateNavigator1.DateTime = DateTime.Today.AddDays(+360);
                //MessageBox.Show(date.ToString());
                dateNavigator1.DateTime = DateTime.Today;
            }
            schedulerControl1.GoToToday();
            
            //dateNavigator1.BoldAppointmentDates = false;
        }

        private void simpleButton16_Click_1(object sender, EventArgs e)
        {
            //dateNavigator1.BoldAppointmentDates = true;
            // schedulerControl1.GoToDate(dateNavigator1.DateTime.AddDays(1));
            //dateNavigator1.DateTime = dateNavigator1.DateTime.AddDays(1);
            schedulerControl1.Start = schedulerControl1.Start.AddDays(1);
        }

        private void simpleButton17_Click_1(object sender, EventArgs e)
        {
            //schedulerControl1.GoToDate(dateNavigator1.DateTime.AddDays(-1));
            //dateNavigator1.DateTime = dateNavigator1.DateTime.AddDays(-1);
            //schedulerControl1.GoToDate(dateNavigator1.DateTime);
            schedulerControl1.Start = schedulerControl1.Start.AddDays(-1);
        }

        private void frmRandevuVer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                btnCari_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                simpleButton14_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F8)
            {
                simpleButton8_Click_1(sender, e);
            }
        }

        private void sorguTarihAraligi(int g1, int m1, int y1, int h1, int min1, int sec1, Boolean p1,
               int g2, int m2, int y2, int h2, int min2, int sec2, Boolean p2)
        {

            DateTime d1 = DateTime.Now;

            d1 = d1.AddDays(g1);
            d1 = d1.AddMonths(m1);
            d1 = d1.AddYears(y1);
            d1 = d1.AddHours(h1);
            d1 = d1.AddMinutes(min1);
            ilktarih.DateTime = d1.AddSeconds(sec1);
            ilktarih.ToolTip = ilktarih.DateTime.ToString();

            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih.DateTime = d2.AddSeconds(sec2);
            sontarih.ToolTip = sontarih.DateTime.ToString();
        }


        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// dün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 1)// Bu gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 2)// yarın
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 3)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 5)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 6)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 7)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 1, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);




            }
            //else if (cbTarihAraligi.SelectedIndex ==6) // Bu Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
            //                      0, 0, 0, 0, 0, 0, false);

            //}
            //else if (cbTarihAraligi.SelectedIndex == 7) // Geçen Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), -1, 0, 0, 0, false,
            //                    (-DateTime.Now.Day), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false);

            //}
            else if (cbTarihAraligi.SelectedIndex == 8)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }

            RandevuListesi();
        }

        private DateTime getStartOfWeek(bool useSunday)
        {
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;

            if (!useSunday)
                dayOfWeek--;

            if (dayOfWeek < 0)
            {// day of week is Sunday and we want to use Monday as the start of the week
                // Sunday is now the seventh day of the week
                dayOfWeek = 6;
            }

            return now.AddDays(-1 * (double)dayOfWeek);
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            RandevuListesi();
        }

        private void bekliyorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string pkHatirlatma = dr["pkHatirlatma"].ToString();
                DB.ExecuteSQL("update Hatirlatma set fkDurumu=1 where pkHatirlatma=" + pkHatirlatma);
            }

            RandevuListesi();
        }

        private void geldiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string pkHatirlatma = dr["pkHatirlatma"].ToString();
                DB.ExecuteSQL("update Hatirlatma set fkDurumu=2 where pkHatirlatma=" + pkHatirlatma);
            }

            RandevuListesi();
        }

        private void gelmediToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string pkHatirlatma = dr["pkHatirlatma"].ToString();
                DB.ExecuteSQL("update Hatirlatma set fkDurumu=3 where pkHatirlatma=" + pkHatirlatma);
            }

            RandevuListesi();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\RandevuVerGrid.xml";
            gridView2.SaveLayoutToXml(Dosya);            
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\RandevuVerGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView2.ShowCustomization();
            gridView2.OptionsBehavior.AutoPopulateColumns = true;
            gridView2.OptionsCustomization.AllowColumnMoving = true;
            gridView2.OptionsCustomization.AllowColumnResizing = true;
            gridView2.OptionsCustomization.AllowQuickHideColumns = true;
            gridView2.OptionsCustomization.AllowRowSizing = true;
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            //Degerler.Renkler.Beyaz;
            if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "1")
                e.Appearance.BackColor = System.Drawing.Color.LightYellow;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "2")
                e.Appearance.BackColor = System.Drawing.Color.LightGreen;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "3")
                e.Appearance.BackColor = System.Drawing.Color.Red;

            //if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "1")
            //  e.Appearance.BackColor = System.Drawing.Color.Aqua;

            //if(gridColumn3
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "4")
            //    e.Appearance.BackColor = System.Drawing.Color.Green;
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "5")
            //    e.Appearance.BackColor = System.Drawing.Color.Blue;
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "6")
            //    e.Appearance.BackColor = System.Drawing.Color.Black;

        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;

            if (i < 0) return;
            DataRow dr = gridView2.GetDataRow(i);
            string fkFirma = dr["pkFirma"].ToString();

            pkFirma.Text = fkFirma;
            //zamansec();
            //simpleButton3_Click(sender,e);//oluştur

            gridView2.FocusedRowHandle = i;
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;
            if (i < 0) return;

            DataRow dr = gridView2.GetDataRow(i);
            //string pkTaksitler = dr["pkTaksitler"].ToString();
            string fkFirma = dr["fkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkFirma;
            // KasaGirisi.pkTaksitler.Text = pkTaksitler;
            //KasaGirisi.tEaciklama.Text = dr["Tarih"].ToString() + "-Taksit Ödemesi-" + dr["Odenecek"].ToString();
            //KasaGirisi.ceTutarNakit.EditValue = dr["Odenecek"].ToString();
            //decimal kalan = 0;
            //decimal.TryParse(dr["Kalan"].ToString(), out kalan);
            //KasaGirisi.ceTutarNakit.Value = kalan;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            RandevuListesi();
            MusteriBorcuGetir();
            gridView1.FocusedRowHandle = i;
        }

        DevExpress.XtraGrid.GridControl targetGrid = null;
        DevExpress.XtraGrid.Views.Grid.GridView gridView = null;
        private void teCariAdi_KeyDown(object sender, KeyEventArgs e)
        {
            //degisiklikvar = true;
            if (e.KeyCode == Keys.Down && targetGrid != null)
            {
                targetGrid.Visible = true;
                targetGrid.Focus();
            }
            if (e.KeyCode == Keys.Enter && teCariAdi.Text.Length>2 && gridView.DataRowCount==0)
            {
                targetGrid.Visible = false;

               // string fkFirma = pkFirma.Text;
                frmMusteriKarti MusteriKarti = new frmMusteriKarti("0", "");
                MusteriKarti.txtMusteriAdi.Text = teCariAdi.Text;
                MusteriKarti.txtMusteriAdi.ToolTip = teCariAdi.Text;
                MusteriKarti.ShowDialog();

                pkFirma.Text = MusteriKarti.pkFirma.Text;

                lueStoklar.Focus();
            }

            if (e.KeyCode == Keys.Enter && teCariAdi.Text.Length > 2 && gridView.DataRowCount > 0)
            {
                targetGrid.Visible = false;

                //frmMusteriKarti MusteriKarti = new frmMusteriKarti("0");
                //MusteriKarti.padi.Text = teCariAdi.Text;
                //MusteriKarti.padi.ToolTip = teCariAdi.Text;
                //MusteriKarti.ShowDialog();

                //pkFirma.Text = MusteriKarti.pkFirma.Text;

                //lueStoklar.Focus();

                targetGrid.Visible = true;
                targetGrid.Focus();
            }
        }

        private void teCariAdi_KeyUp(object sender, KeyEventArgs e)
        {
            if (targetGrid == null)
            {
                targetGrid = new DevExpress.XtraGrid.GridControl();
                gridView = new GridView(targetGrid);
                targetGrid.Name = "ara";
                gridView.Name = "ReportView";
                targetGrid.ViewCollection.Add(gridView);
                targetGrid.MainView = gridView;
                gridView.GridControl = targetGrid;
                this.Controls.Add(targetGrid);
                //gridView.ShowFilterPopup(gridView.Columns[0]);
                gridView.OptionsView.ShowGroupPanel = false;
                gridView.OptionsBehavior.Editable = false;
                //gridView.FocusedRowChanged += new FocusedRowChangedEventHandler(gridView_FocusedRowChanged);
                this.gridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
                gridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridView_KeyDown);
            }
            if (teCariAdi.Text.Length > 2)
            {
                if (teCariAdi.Text.IndexOf(" ") == -1)
                    targetGrid.DataSource = DB.GetData("select pkFirma,Firmaadi,pkFirma as Id From Firmalar with(nolock) where Firmaadi like '%" + teCariAdi.Text + "%'"); // populated data table
                else
                {
                    targetGrid.DataSource = DB.GetData("select pkFirma,Firmaadi,pkFirma as Id From Firmalar with(nolock) where Firmaadi like '%" + teCariAdi.Text.Substring(0, teCariAdi.Text.IndexOf(" ")) + "%'" +
                       " and Firmaadi like '%" + teCariAdi.Text.Substring(teCariAdi.Text.IndexOf(" ") + 1, teCariAdi.Text.Length - teCariAdi.Text.IndexOf(" ") - 1) + "%'");
                }
            }

            targetGrid.BringToFront();
            targetGrid.Width = 550;
            targetGrid.Height = 300;
            targetGrid.Left = teCariAdi.Left;
            targetGrid.Top = teCariAdi.Top + 30;
            if (gridView.Columns.Count > 0)
            {
                gridView.Columns[0].Visible = false;
                gridView.Columns[2].Width = 20;
                //gridView.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //gridView.Columns[2].DisplayFormat.FormatString = "{0:#0.00####}";
            }
            if (gridView.DataRowCount == 0)
                targetGrid.Visible = false;
            else
                targetGrid.Visible = true;
        }

        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
                teCariAdi.Text = dr["Firmaadi"].ToString();
                pkFirma.Text = dr["pkFirma"].ToString();
                txtPkHatirlatma.Text = "0";
                targetGrid.Visible = false;
                lueStoklar.Focus();
            }
        }
        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            teCariAdi.Text = dr["Firmaadi"].ToString();
            pkFirma.Text = dr["pkFirma"].ToString();
            txtPkHatirlatma.Text = "0";
            targetGrid.Visible = false;
            lueStoklar.Focus();
        }

        private void btnListe_Click(object sender, EventArgs e)
        {
            xtraTabControl2.SelectedTabPage = xtabRandevuGrid;
        }

        private void btnGorunum_Click(object sender, EventArgs e)
        {
            xtraTabControl2.SelectedTabPage = xtabRandevuGorunum;
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            frmFormAyarlari fa = new frmFormAyarlari();
            fa.ShowDialog();
            RandevuAyarlari(out RandevuBasSaat, out RandevuBitSaat, out RandevuSaatAraligi);
        }

        private void schedulerControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.C && e.Modifiers == Keys.Control) || (e.KeyCode == Keys.K && e.Modifiers == Keys.Control))
            {
                item_Click_Copy(sender, e);
            }

            if (e.KeyCode == Keys.X && e.Modifiers == Keys.Control)
            {
                item_Click_Cut(sender, e);
            }

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                item_Click_Paste(sender, e);
            }


        }

        private void lbBakiye_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = pkFirma.Text;
            // KasaGirisi.pkTaksitler.Text = pkTaksitler;
            //KasaGirisi.tEaciklama.Text = dr["Tarih"].ToString() + "-Taksit Ödemesi-" + dr["Odenecek"].ToString();
            //KasaGirisi.ceTutarNakit.EditValue = dr["Odenecek"].ToString();
            //decimal kalan = 0;
            //decimal.TryParse(dr["Kalan"].ToString(), out kalan);
            //KasaGirisi.ceTutarNakit.Value = kalan;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            MusteriBorcuGetir();

        }

        private void schedulerControl1_InitAppointmentImages(object sender, AppointmentImagesEventArgs e)
        {

            //if (e.Appointment.IsException)
            if (e.Appointment != null && e.Appointment.CustomFields["arandi"]!= null &&  e.Appointment.CustomFields["arandi"].ToString() == "True")
            {
                AppointmentImageInfo info = new AppointmentImageInfo();
                //info.Image = SystemIcons.Warning.ToBitmap();
                info.Image = GPTS.Properties.Resources.arandi;
                e.ImageInfoList.Add(info);
            }

            //if (e.Appointment.CustomFields["ApptImage1"] != null)
            //{
            //    byte[] imageBytes = (byte[])e.Appointment.CustomFields["ApptImage1"];
            //    if (imageBytes != null)
            //    {
            //        AppointmentImageInfo info = new AppointmentImageInfo();
            //        using (MemoryStream ms = new MemoryStream(imageBytes))
            //        {
            //            info.Image = Image.FromStream(ms);
            //            e.ImageInfoList.Add(info);
            //        }
            //    }
            //}
        }

        private void dateNavigator1_CustomDrawDayNumberCell(object sender, DevExpress.XtraEditors.Calendar.CustomDrawDayNumberCellEventArgs e)
        {
        //    dateEdit1.DateTime = e.Date;
        //    //if (e.Selected)
        //    //{
        //    //    labelControl2.Text = e.Date.ToString();

        //    //}
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("say");
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //if (xtraTabControl1.SelectedTabPage == xtabHizmetler)
            //    gcHizmetler.DataSource = DB.GetData("select pkStokKarti,Stokadi from StokKarti with(nolock) where aktif=1");
        }

        private void dateNavigator1_SizeChanged(object sender, EventArgs e)
        {
            //if(xtraTabControl2.SelectedTabPage== xtabRandevuGorunum)
            //{
                //RandevuSekmeListeGetir();
            //}
        }

        private void schedulerControl1_VisibleIntervalChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("e");
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RandevuSekmeListeGetir();
            timer1.Enabled = false;
        }

        private void arandıToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string pkHatirlatma = dr["pkHatirlatma"].ToString();

                if (dr["arandi"].ToString() == "True")
                    DB.ExecuteSQL("update Hatirlatma set arandi=0 where pkHatirlatma=" + pkHatirlatma);
                else
                    DB.ExecuteSQL("update Hatirlatma  set arandi=1 where pkHatirlatma=" + pkHatirlatma);
            }

            RandevuListesi();

            //if (gridView2.FocusedRowHandle < 0) return;
            //DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            //string id = dr["pkHatirlatma"].ToString();
            ////MessageBox.Show(dr["pkHatirlatma"].ToString());
            //if (dr["arandi"].ToString() == "True")
            //    DB.ExecuteSQL("update Hatirlatma  set arandi=0 where pkHatirlatma=" + id);
            //else
            //    DB.ExecuteSQL("update Hatirlatma  set arandi=1 where pkHatirlatma=" + id);

            //RandevuListesi();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            //string id = dr["pkHatirlatma"].ToString();
            if(dr["arandi"].ToString()=="True")
                arandıToolStripMenuItem1.Text = "Aranmadı Yap";
            else
                arandıToolStripMenuItem1.Text = "Arandı";
        }

        //private void schedulerControl1_CustomDrawResourceHeader(object sender, CustomDrawObjectEventArgs e)
        //{
            //ResourceHeader header = (ResourceHeader)e.ObjectInfo;
            //// Get the resource information from custom fields.
            //string postcode = (header.Resource.CustomFields["PostCode"] != null) ? header.Resource.CustomFields["PostCode"].ToString() : String.Empty;
            //string address = (header.Resource.CustomFields["Address"] != null) ? header.Resource.CustomFields["Address"].ToString() : String.Empty;
            //// Specify the header caption and appearance.
            //header.Appearance.HeaderCaption.ForeColor = Color.Blue;
            //header.Caption = header.Resource.Caption + System.Environment.NewLine + address + System.Environment.NewLine + postcode;
            //header.Appearance.HeaderCaption.Font = e.Cache.GetFont(header.Appearance.HeaderCaption.Font, FontStyle.Bold);
            //// Draw the header using default methods.
            //e.DrawDefault();
            //e.Handled = true;
       // }

        private void btnMusteriHareketleri_Click(object sender, EventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string firma_id = pkFirma.Text;
            if (firma_id == "") return;

            MusteriHareketleriAc(firma_id);

        }

        private void arandıToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cbOda_Click(object sender, EventArgs e)
        {
            frmOdalar odalar = new frmOdalar();
            odalar.ShowDialog();

            OdalarGetir();
        }

        private void lueOdalar_EditValueChanged(object sender, EventArgs e)
        {
            if (lueOdalar.Tag.ToString() == "1")
            {
                Degerler.isYenile = true;
                RandevuSekmeListeGetir();
            }
        }

        private void lueOdalar_Leave(object sender, EventArgs e)
        {
            lueOdalar.Tag = "0";
        }

        private void lueOdalar_Enter(object sender, EventArgs e)
        {
            lueOdalar.Tag = "1";
        }

        private void odaDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOdaDegis degis = new frmOdaDegis();
            degis.ShowDialog();
            if (degis.Tag.ToString() == "0") return;

            string yeni_oda_id = degis.lueOdalar.EditValue.ToString();

            string s = formislemleri.MesajBox(degis.lueOdalar.Text+" Olarak Değiştirilecek. Eminmisiniz?", "Oda Değiştir.", 3, 1);
            if (s == "0") return;

            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string pkHatirlatma = dr["pkHatirlatma"].ToString();
                DB.ExecuteSQL("update Hatirlatma set fkOda="+yeni_oda_id+" where pkHatirlatma=" + pkHatirlatma);
            }

            RandevuListesi();
        }

        private void cbOda_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void schedulerControl1_Click(object sender, EventArgs e)
        {

        }

        private void repositoryItemLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            
            DB.ExecuteSQL("update Hatirlatma set fkDurumu=" + value + " where pkHatirlatma=" + dr["pkHatirlatma"].ToString());

        }

        private void satışFişiOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView2.FocusedRowHandle < 0)
                return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            int fkfirma = 1;
            if (dr["pkFirma"].ToString() != "")
                fkfirma = int.Parse(dr["pkFirma"].ToString());

            frmSatis Satis = new frmSatis(fkfirma ,0,"","0");
            Satis.gbBaslik.Tag = 0;
            Satis.gbBaslik.AccessibleDescription = "0";
            Satis.gbBaslik.AccessibleName ="0";
            Satis.txtpkSatislar.Text = "0";
            Satis.Satis1Toplam.Tag = 0;
            Satis.ShowDialog();
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\RandevuVerMusteriGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\RandevuVerMusteriGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void labelControl5_Click(object sender, EventArgs e)
        {

        }

        //private void schedulerControl1_InitAppointmentDisplayText(object sender, AppointmentDisplayTextEventArgs e)
        //{
        //    //e.Text = "test";
        //}

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            //Degerler.Renkler.Beyaz;
            if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "1")
                e.Appearance.BackColor = System.Drawing.Color.LightYellow;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "2")
                e.Appearance.BackColor = System.Drawing.Color.LightGreen;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "3")
                e.Appearance.BackColor = System.Drawing.Color.Red;
        }

        private void dateNavigator1_EditDateModified(object sender, EventArgs e)
        {
            //seçilen adeti getirir
            //this.Text = ((DevExpress.XtraScheduler.DateNavigator)(sender)).Selection.Count.ToString();

            //this.Text = ((DevExpress.XtraScheduler.DateNavigator)(sender)).SelectionEnd.ToString();
        }

        //private void dateNavigator1_ChangeUICues(object sender, UICuesEventArgs e)
        //{

        //}

        //private void schedulerStorage1_FetchAppointments(object sender, FetchAppointmentsEventArgs e)
        //{
        //    TimeIntervalCollection tic = schedulerControl1.ActiveView.GetVisibleIntervals();
        //    DateTime start = tic.Start;
        //    DateTime end = tic.End;

        //    //DateTime start = e.Interval.Start;
        //    //DateTime end = e.Interval.End;
        //}


        //seçilen tarihlerin baş ve son
        //private void schedulerStorage1_FetchAppointments(object sender, FetchAppointmentsEventArgs e)
        //{
        //    DateTime dt = 
        //    e.Interval.Start;

        //    DateTime dt1 =
        //   e.Interval.End;
        //}
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Media;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.Utils;
using GPTS.islemler;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using GPTS.Include.Data;
using DevExpress.XtraReports.UI;
using DevExpress.XtraTab;

namespace GPTS
{
   
    public partial class frmCallerID : DevExpress.XtraEditors.XtraForm
    {
        
        //calform.axCIDv51 _axCIDv51 = new calform.axCIDv51();
        //public cidv5callerid.CIDv5 axCIDv51
        //{
        //    get
        //    {
        //        if (_axCIDv51 == null)
        //        {
        //            try
        //            {
        //                _axCIDv51 = new cidv5callerid.CIDv5();
        //            }
        //            catch (Exception exp)
        //            {
        //                MessageBox.Show("Caller Id Hatası " + exp.Message);
        //                //throw;
        //                return null;
        //            }
        //            finally
        //            {
        //                if (_axCIDv51.Active)
        //                    axCIDv51.OnCallerID += AxCIDv51_OnCallerID;
        //            }

        //        }
        //        return _axCIDv51;
        //    }
        //    set
        //    {
        //        _axCIDv51 = value;
        //    }
        //}

        string _arayan = "";
        public frmCallerID()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;// - 30;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 50;
            //ilkgenislikyukseklik();
        }

        private void frmCallerID_Load(object sender, EventArgs e)
        {
            //WindowsFormsApplication1.Form1 calform = new WindowsFormsApplication1.Form1();
            //calform.ShowDialog();
            KullaniciListesi();

            lbMusteriBilgisi.Text = "";
            lbAdres.Text = "";
            //try
            //{
            //if(axCIDv51!=null)
            axCIDv51.Start();
            //MessageBox.Show(axCIDv51.Active.ToString());
            //object sender;
            //callerform.axCIDv51 _axCIDv51 = new callerform.axCIDv51();

            //calform.axCIDv51_OnCallerID.ICIDv5Events_OnCallerIDEvent e;
            //calform.axCIDv51_OnCallerID(sender,e)

            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show("hata" + exp.Message);
            //    //throw;
            //}

            ilktarih.Properties.DisplayFormat.FormatString = "f";
            sontarih.Properties.EditFormat.FormatString = "f";
            ilktarih.Properties.EditFormat.FormatString = "f";
            sontarih.Properties.DisplayFormat.FormatString = "f";
            ilktarih.Properties.EditMask = "f";
            sontarih.Properties.EditMask = "f";

            ilktarih.DateTime = DateTime.Today.AddDays(-1).AddHours(0).AddMinutes(0);
            sontarih.DateTime = DateTime.Today.AddHours(23).AddMinutes(59);
            //ilkgenislikyukseklik();
            // this.axCIDv52 = new Axcidv5callerid.AxCIDv5();
            //net elektronik sistemler

            //GeçmisAramalarBakildi();
            ArayanlariGetirBakilmadi();

            if (File.Exists("callerid.xml"))
                gridView1.RestoreLayoutFromXml("callerid.xml");

            timer1.Enabled = true;
        }
        void KullaniciListesi()
        {
            lueKullanicilar.Properties.DataSource = DB.GetData(@"select 0 as pkKullanicilar,'Seçiniz.' as  adisoyadi,'Seçiniz.' as KullaniciAdi 
            union all
            select pkKullanicilar, adisoyadi, KullaniciAdi from Kullanicilar with(nolock) where durumu = 1");
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }
        //private void axCIDv51_OnCallerID(object sender, Axcidv5callerid.ICIDv5Events_OnCallerIDEvent e)
        private void AxCIDv51_OnCallerID(string DeviceID, string Line, string PhoneNumber, string DateTime, string OtherText)
        {
            _arayan = PhoneNumber;
            Caliyormu(_arayan, "0");//e.line);
        //    //lArayan.Text = _arayan;
        //    //throw new NotImplementedException();
        }
        void ilkgenislikyukseklik()
        {
            this.Left = 10;
            this.Top = 5;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 30;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 100;
        }        
       
        void sescal()
        {
            try
            {
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string dosya = exeDiz + "\\Sesler\\siparis.wav";

                //string path = "C:\\windows\\media\\ding.wav"; // Müzik adresi
                if (File.Exists(dosya))
                {
                    SoundPlayer player = new SoundPlayer();
                    player.SoundLocation = dosya;// "chord.wav";
                    player.Play();
                }
            }
            catch(Exception exp)
            {
               
            }
        }

        void ArayanlariGetirBakilmadi()
        {
            gridControl1.DataSource = DB.GetData(@"SELECT top 50 pkArayanlar,Tarih,
            case when fkFirma=0 then 'çift tıkla kaydet' else f.Firmaadi end as Arayan,
			isnull(fkFirma,0) as fkFirma,Telefon,Port,
            Durumu,f.Adres,f.KaraListe,f.OzelKod,
			dbo.fon_MusteriBakiyesi(fkFirma) as Bakiye,
			a.Siparis,a.aciklama  FROM Arayanlar a with(nolock)
            left join Firmalar f with(nolock) on f.pkFirma=a.fkFirma
            where Durumu='Cevapsız' order by Tarih desc");
        }

        void GeçmisAramalarBakildi()
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            
            gridControl2.DataSource = DB.GetData(@"SELECT 

pkArayanlar,
Tarih,
bakildi,
isnull(f.Firmaadi,a.Arayan) as Arayan,
fkFirma,
Telefon,
f.Tel,
f.Tel2,
f.Cep,
Port,
Durumu,
aciklama 

FROM Arayanlar a with(nolock)
left join Firmalar f with(nolock) on f.pkFirma=a.fkFirma
where Durumu<>'Cevapsız'
and BakilmaTarihi between @ilktar and @sontar
order by Tarih desc
			", list);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lbMusteriBilgisi.Text == "") return;
            if(lbMusteriBilgisi.ForeColor == System.Drawing.Color.White)
                lbMusteriBilgisi.ForeColor = System.Drawing.Color.Red;
            else
                lbMusteriBilgisi.ForeColor = System.Drawing.Color.White;
            //try
            //{
            //    //label1.Text = "Cihaz Bilgisi : " + axCIDv51.Command("Devicemodel") + "     " + axCIDv51.Command("Serial");
            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //    timer1.Enabled = false;
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (simpleButton2.Tag.ToString() == "0")
            {
                this.Width = 150;
                this.Height = 100;
                simpleButton2.Tag = "1";
                //this.TopMost = true;
            }
            else
            {
                simpleButton2.Tag = "0";
                ilkgenislikyukseklik();
                //this.TopMost = false;
            }
        }
        
        void MusteriKayit()
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            string pkArayanlar = dr["pkArayanlar"].ToString();
            string telefon = dr["Telefon"].ToString();
            //this.TopMost = false;

            //if (dr["fkFirma"].ToString() == "0")
            //{
            //    musteriata();
            //    return;
            //}

            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.Cep.Tag = "1";
            KurumKarti.Cep.Text = telefon;
            KurumKarti.ShowDialog();

            //this.TopMost = true;
            if (KurumKarti.pkFirma.Text == "") return;

            DB.PkFirma = int.Parse(KurumKarti.pkFirma.Text);

            string padi = KurumKarti.txtMusteriAdi.Text;

            DB.ExecuteSQL("UPDATE Arayanlar SET Arayan='" + padi + "',fkFirma=" + DB.PkFirma.ToString() +
                " WHERE pkArayanlar=" + pkArayanlar);

        }
        
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MusteriKayit();
            ArayanlariGetirBakilmadi();
        }
        
        void Caliyormu(string telefon,string Port)
        {
            string pkFirma = "0";
            this.Show();
            sescal();
            string arayan = "Kayıtsız Müşteri";
            //listBox1.Items.Add("Tel" + e.line + " - " + e.dateTime + " - " + telefon);
            string sql = "select pkFirma,Firmaadi,Adres, dbo.fon_MusteriBakiyesi(Firmalar.pkFirma) as Bakiye from Firmalar with(nolock)"+
            " where Firmalar.pkFirma>1 and Tel2='" +
                telefon +
                "' or Tel='" + telefon +
                "' or Cep='" + telefon +
                "' or Cep2='" + telefon +
                "' or Fax='" + telefon +
                "' or Tel='" + telefon.Substring(1, telefon.Length - 1) +
                "' or Cep='" + telefon.Substring(1, telefon.Length - 1) +
                "' or Cep2='" + telefon.Substring(1, telefon.Length - 1) +
                "' or Fax='" + telefon.Substring(1, telefon.Length - 1) +
                "' or Tel2='" + telefon.Substring(1, telefon.Length - 1) + "'";
            DataTable dt = DB.GetData(sql);

            ArrayList list = new ArrayList();
            if (dt.Rows.Count == 0)
            {
                arayan = "Bilinmiyor.";
            }
            else
            {
                arayan = dt.Rows[0]["Firmaadi"].ToString();
                pkFirma = dt.Rows[0]["pkFirma"].ToString();
            }

            list.Add(new SqlParameter("@Arayan", arayan));
            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Telefon", telefon));
            list.Add(new SqlParameter("@Port", Port));

            //if(DB.GetData("select count(*) from Arayanlar where Durumu='' and fkFirma=" + pkFirma).Rows[0][0].ToString()=="0")
            DB.ExecuteSQL("INSERT INTO Arayanlar (Tarih,Arayan,Telefon,fkFirma,Port,Durumu,Siparis) values(getdate(),@Arayan,@Telefon,@fkFirma,@Port,'Cevapsız',0)", list);
            //else
            //  DB.ExecuteSQL("UPDATE Arayanlar SET (Tarih,Arayan,Telefon,fkFirma,Port,Durumu) values(getdate(),@Arayan,@Telefon,@fkFirma,@Port,'Cevapsız')", list);
            
            //ArayanlariGetirBakildi();
            ArayanlariGetirBakilmadi();

            ArayanBilgileriGetir();

            memoEdit1.Focus();
            //  e.dateTime Telefon santrali tarafından gönderilen zaman bilgisidir. Saat ve dakika olarak gelir saniye gelmez.
            //  PC zamanından bağımsızdır. Gerekirse PC zamanı da kullanılabilir.
            //  Nesne .start  ile başlatılmışsa ve numara algılanmışsa, bu olay tetiklenir.
        }
        
        //private void axCIDv51_OnCallerID_1(object sender, Axcidv5callerid.ICIDv5Events_OnCallerIDEvent e)
        //{
        //    Caliyormu(e.phoneNumber,e.line);
        //}

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            //this.Dispose();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
           
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            string pkArayanlar = dr["pkArayanlar"].ToString();
            string telefon = dr["Telefon"].ToString();
            //if (DB.PkFirma.ToString() == "" || DB.PkFirma.ToString() == "0") return;
            this.TopMost = false;
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.Cep.Text = telefon;
            KurumKarti.ShowDialog();
            this.TopMost = true;
            DB.PkFirma = int.Parse(KurumKarti.pkFirma.Text);
            string padi = KurumKarti.txtMusteriAdi.Text;
            DB.ExecuteSQL("UPDATE Arayanlar SET Arayan='" + padi + "',fkFirma=" + DB.PkFirma.ToString() + " WHERE pkArayanlar=" +
                pkArayanlar);

            ArayanlariGetirBakilmadi();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmMusterilereGenelBakis musteriraporlari = new frmMusterilereGenelBakis();
            musteriraporlari.ShowDialog();
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                if (gridView2.FocusedRowHandle < 0) return;
                DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            }
            else
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            }
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.ShowDialog();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            GeçmisAramalarBakildi();
            ArayanlariGetirBakilmadi();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            string secim = formislemleri.MesajBox("Silmek İstediğinize Eminmisiniz?", Degerler.mesajbaslik, 1, 0);
            if (secim == "0") return;

            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkArayanlar = dr["pkArayanlar"].ToString();
            DB.ExecuteSQL("delete from Arayanlar where pkArayanlar=" + pkArayanlar);
            gridView2.DeleteSelectedRows();
            //ArayanlariGetir2();
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);
            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            if (e.Column.FieldName == "KaraListe" && dr["KaraListe"].ToString() == "True")
            {
                AppearanceHelper.Apply(e.Appearance, appError);
            }
        }

        void MusteriSiparisleri2()
        {
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //if (dr == null) return;

            gridControl3.DataSource = DB.GetData(@"select s.pkSatislar,s.Tarih,s.Aciklama,f.Firmaadi,s.fkFirma,
k.KullaniciAdi,sum(sd.Adet*sd.SatisFiyati-sd.iskontotutar) as ToplamTutar,
m.masa_adi from Satislar s with(nolock)
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar
inner join Firmalar f with(nolock) on f.pkFirma = s.fkFirma
left join Kullanicilar k with(nolock) on k.pkKullanicilar=s.fkKullanici
left join Masalar m with(nolock) on m.fkSatislar=s.pkSatislar
where s.Siparis = 0
group by s.pkSatislar,s.Tarih,s.Aciklama,f.Firmaadi,s.fkFirma,
k.KullaniciAdi,s.ToplamTutar,m.masa_adi");// and s.fkFirma = " + dr["fkFirma"].ToString());
        }

        void ArayanBilgileriGetir()
        {
            string fkFirma = "0";
            lbMusteriBilgisi.Text = "";
            lbAdres.Text = "";
            memoEdit1.Text = "";

            if (gridView1.FocusedRowHandle > -1)
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dr!=null && dr["fkFirma"].ToString() != "")
                {
                    fkFirma = dr["fkFirma"].ToString();
                    lbMusteriBilgisi.Text = dr["Arayan"].ToString();
                    lbAdres.Text= dr["Adres"].ToString();

                    memoEdit1.Text = dr["aciklama"].ToString();

                    DB.PkFirma = int.Parse(fkFirma);
                }
            }

            MusteriSiparisleri2();
        }
        
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            ArayanBilgileriGetir();
            MusteriSiparisleri2();
        }
       
        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            this.TopMost = false;
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
            this.TopMost = true;
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ArayanBilgileriGetir();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            frmSiparisAl SiparisAl = new frmSiparisAl();
            SiparisAl.ShowDialog();
            if (SiparisAl.Tag.ToString()=="1")
                 btnBakildi_Click(sender, e);
            this.TopMost = true;
            //MusteriSiparisler(fkFirma);
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            frmUcGoster SatisGoster = new frmUcGoster(1, "0");
            SatisGoster.ShowDialog();
            this.TopMost = true;
        }

        private void müşteriKayıtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridView1_DoubleClick(sender,e);
        }

        private void müşteriyeAtaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            musteriata();
        }

        void musteriata()
        {
            int secilen = gridView1.FocusedRowHandle;
            //this.TopMost = false;
            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.ShowDialog();

            //if (MusteriAra.TopMost == true) return;

            string fkFirma = MusteriAra.fkFirma.Tag.ToString();
            if (fkFirma != "0")
            {
                DataRow dr = gridView1.GetDataRow(secilen);

                DB.ExecuteSQL("UPDATE Arayanlar SET fkFirma=" + fkFirma + " WHERE pkArayanlar=" + dr["pkArayanlar"].ToString());
                DB.ExecuteSQL("UPDATE Firmalar SET Tel2=Tel,Tel='" + dr["Telefon"].ToString() + "' where pkFirma=" + fkFirma);
                ArayanlariGetirBakilmadi();
                gridView1.FocusedRowHandle = secilen;
            }
            //this.TopMost = true;
        }
        private void simpleButton12_Click(object sender, EventArgs e)
        {
           
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            string secim = formislemleri.MesajBox("Tümünü Silmek İstediğinize Eminmisiniz?", Degerler.mesajbaslik, 1, 0);
            if (secim == "0") return;

            for (int i = 0; i < gridView2.RowCount; i++)
            {
              DataRow dr = gridView2.GetDataRow(i);
              string pkArayanlar = dr["pkArayanlar"].ToString();
              DB.ExecuteSQL("delete from Arayanlar where pkArayanlar=" + pkArayanlar);
            }
            GeçmisAramalarBakildi();
        }

        private void btnSipariseGec_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkFirma = dr["fkFirma"].ToString();
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", fkFirma));
            list.Add(new SqlParameter("@fkPerTeslimEden", "1"));
            //list.Add(new SqlParameter("@TeslimTarihi", dateEdit1.DateTime));
            //list.Add(new SqlParameter("@Aciklama", memoEdit1.Text));


            string pkSatislarYeniid = DB.ExecuteScalarSQL("INSERT INTO Satislar (Tarih,fkFirma,GelisNo,Siparis,fkKullanici,fkSatisDurumu,GuncellemeTarihi,fkPerTeslimEden,TeslimTarihi) " +
                " values(getdate(),@fkFirma,1,0,1,10,getdate(),1,getdate()) SELECT IDENT_CURRENT('Satislar')", list);
            formislemleri.Mesajform("Sipariş Oluşturuldu.", "Y", 200);

            btnBakildi_Click(sender, e);

            simpleButton1_Click(sender, e);//ekranı gizle
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            yazdir();
            this.TopMost = true;
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
        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gridControl2;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {

            if (gridView1.FocusedRowHandle < 0) return;
            string girilen =
                ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (girilen == "True")
                DB.ExecuteSQL("UPDATE Arayanlar SET  Siparis=1 where pkArayanlar=" +
                dr["pkArayanlar"].ToString());
            else
                DB.ExecuteSQL("UPDATE SatisDetay SET Siparis=0 where pkArayanlar=" +
           dr["pkArayanlar"].ToString());
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridView1.SaveLayoutToXml("callerid.xml");
        }

        private void btnBakildi_Click(object sender, EventArgs e)
        {
            int si = gridView1.FocusedRowHandle;
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));

                string pkArayanlar = dr["pkArayanlar"].ToString();

                DB.ExecuteSQL("UPDATE Arayanlar SET BakilmaTarihi=getdate(),Durumu='Cevaplandı'"+
                    ",aciklama='"+memoEdit1.Text+"' where pkArayanlar=" + pkArayanlar);
                //button1_Click(sender, e);//açıklama kaydet
            }

            ArayanlariGetirBakilmadi();
            
            //ArayanBilgileriGetir();

            Application.DoEvents();

            memoEdit1.Text = "";
            //gridView1.Focus();
            //DataRow dr = gridView1.GetDataRow(si);
            if (gridView1.DataRowCount==si)
                si = si-1;
            try
            {
                gridView1.SelectRange(si, si);
                gridView1.FocusedRowHandle = si;
            }
            catch (Exception)
            {

               // throw;
            }
            //gridView1.FocusedColumn.FieldName = "Arayan";
            //gridView1.FocusedRowHandle = si;

        }

        private void btnBakildilar_Click(object sender, EventArgs e)
        {
            GeçmisAramalarBakildi();
        }

        private void frmCallerID_FormClosed(object sender, FormClosedEventArgs e)
        {
            //axCIDv51.Dispose();
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());

            this.TopMost = false;
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
            this.TopMost = true;

            GeçmisAramalarBakildi();
        }

        private void baslik_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show(); 
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();

            frmSatis fSatis = new frmSatis(int.Parse(dr["fkFirma"].ToString()), 0,"Caller Id","0");
            fSatis.ShowDialog();
            //MessageBox.Show("Yapım Aşamasında");
        }

        private void simpleButton6_Click_1(object sender, EventArgs e)
        {
            button1_Click(sender, e);//açıklama kaydet

            FisYazdir(false);
        }

        void FisYazdir(bool Disigner)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
           string firma_id = dr["fkFirma"].ToString();
            string arayanlar_id = dr["pkArayanlar"].ToString();
            System.Data.DataSet ds = new DataSet("Test");

            DataTable sirket = DB.GetData(@"SELECT top 1 * from Sirketler");
            sirket.TableName = "sirket";
            ds.Tables.Add(sirket);

            DataTable musteri = DB.GetData(@"SELECT * from Firmalar where pkFirma=" + firma_id);
            musteri.TableName = "musteri";
            ds.Tables.Add(musteri);

            DataTable arayan = DB.GetData(@"SELECT * from Arayanlar a with(nolock) 
            left join Kullanicilar k with(nolock) on k.pkKullanicilar=a.fkKullanicilar
            where pkArayanlar=" + arayanlar_id);

            arayan.TableName = "arayan";
            ds.Tables.Add(arayan);

            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = exedizini + "\\Raporlar\\arayan_callerid.repx";
            if (!File.Exists(RaporDosyasi))
            {
                MessageBox.Show("Arayanlar Caller Id, Rapor Dosyası Bulunamadı");
                return;
            }
            XtraReport rapor = new XtraReport();
            rapor.DataSource = ds;
            rapor.LoadLayout(RaporDosyasi);
            rapor.Name = "arayan_callerid";
            if (Disigner)
                rapor.ShowDesigner();
            else
                rapor.Print();//.ShowPreview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int si = gridView1.FocusedRowHandle;

            if (si < 0) return;

            DataRow dr = gridView1.GetDataRow(si);
            string arayanlar_id = dr["pkArayanlar"].ToString();

            DB.ExecuteSQL("update Arayanlar set aciklama='" + memoEdit1.Text +
                 "' where pkArayanlar=" + arayanlar_id);

            ArayanlariGetirBakilmadi();

            gridView1.FocusedRowHandle = si;
        }

        private void tasarımToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FisYazdir(true);
        }

        private void açıklamaDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnY_Click(object sender, EventArgs e)
        {
            MusteriSiparisleri2();
        }

        private void simpleButton9_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSipariseGec_Click_1(object sender, EventArgs e)
        {

        }

        private void gridView3_DoubleClick_1(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //this.TopMost = false;
            string _fisno = dr["pkSatislar"].ToString();
            string _Firmaid = dr["fkFirma"].ToString();

            frmSatis frmSatis = new frmSatis(int.Parse(_Firmaid), int.Parse(_fisno),"Caller Id","0");
            frmSatis.ShowDialog();
            //yenile
            MusteriSiparisleri2();
        }

        private void btnY_Click_1(object sender, EventArgs e)
        {
            MusteriSiparisleri2();
        }

        private void simpleButton4_Click_2(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle, Firmaid = 1;

            //if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(i);
            if (dr != null)
            {
                if (dr != null && dr["fkFirma"].ToString() != "")
                    Firmaid = int.Parse(dr["fkFirma"].ToString());
            }
            if (Firmaid==0) Firmaid = 1;

            frmSatis frmSatis = new frmSatis(Firmaid, 0,"Caller Id","0");
            frmSatis.ShowDialog();

            MusteriSiparisleri2();

            ArayanlariGetirBakilmadi();
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            //FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }
        
        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl2.SelectedTabPageIndex == 1)
                gridControl4.DataSource = DB.GetData(@"select top 10  s.pkSatislar,s.Tarih,s.Aciklama,f.Firmaadi,s.fkFirma,
s.GuncellemeTarihi,s.ToplamTutar,s.Aciklama from Satislar s with(nolock)
inner join Firmalar f with(nolock) on f.pkFirma = s.fkFirma
where s.Siparis = 1 order by s.pkSatislar desc");
            else if (xtraTabControl2.SelectedTabPageIndex == 2)
                GeçmisAramalarBakildi();
            else if (xtraTabControl2.SelectedTabPageIndex == 3)
            {
                TabMasaGruplari();
            }
        }

        void TabMasaGruplari()
        {

            if (xtraTabControl3.Tag.ToString() == "1") return;

            xtraTabControl3.TabPages.Clear();
            DataTable dt = DB.GetData("select * from MasaGruplari with(nolock) where aktif=1 order by sira_no");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XtraTabPage xtab = new XtraTabPage();
                xtab.Text = dt.Rows[i]["masa_grup_adi"].ToString();
                xtab.Tag = dt.Rows[i]["pkMasaGruplari"].ToString();
                xtraTabControl3.TabPages.Add(xtab);
                xtab.PageVisible = true;
            }
            xtraTabControl3.SelectedTabPageIndex = 0;

            xtraTabControl3.Tag = "1";
        }

        private void btnTumunuSil_Click(object sender, EventArgs e)
        {
            string secim = formislemleri.MesajBox("Tümünü Silmek İstediğinize Eminmisiniz?", Degerler.mesajbaslik, 1, 0);
            if (secim == "0") return;

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string pkArayanlar = dr["pkArayanlar"].ToString();
                DB.ExecuteSQL("delete from Arayanlar where pkArayanlar=" + pkArayanlar);
            }

            ArayanlariGetirBakilmadi();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string secim = formislemleri.MesajBox("Silmek İstediğinize Eminmisiniz?", Degerler.mesajbaslik, 1, 0);
            if (secim == "0") return;


            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkArayanlar = dr["pkArayanlar"].ToString();
            DB.ExecuteSQL("delete from Arayanlar where pkArayanlar=" + pkArayanlar);

            ArayanlariGetirBakilmadi();
        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            frmSatis frmSatis = new frmSatis(1, 0, "Caller Id","0");
            frmSatis.ShowDialog();

            MusteriSiparisleri2();

            ArayanlariGetirBakilmadi();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(i);

            frmMusteriHareketleri MusteriHareketleri = new frmMusteriHareketleri();
            MusteriHareketleri.musteriadi.Tag = dr["fkFirma"].ToString();
            MusteriHareketleri.Show();
        }

        private void smsGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSmsGonder smsgonder = new frmSmsGonder();
            smsgonder.ShowDialog();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            MasalariYukle(false);
        }

        void MasalariYukle(bool bdegisiklikvar)
        {
            if (bdegisiklikvar == false && xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Count > 0)
                return;

            xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();

            int to = 0;
            int lef = 0;

            DataTable dtbutton = DB.GetData(@"select * from Masalar with(nolock) where aktif=1 and fkMasaGruplari=" +
                xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Tag.ToString() +
            " order by sira_no");

            // int h = 80;//dockPanel1.Height / 7;
            // int w = 110;//dockPanel1.Width / 5;
            try
            {
                for (int i = 0; i < dtbutton.Rows.Count; i++)
                {
                    string pkid = dtbutton.Rows[i]["pkMasalar"].ToString();
                    string gen = "100";//dtbutton.Rows[i]["gen"].ToString();
                    if (gen == "") gen = "100";
                    string yuk = "100";//dtbutton.Rows[i]["yuk"].ToString();
                    if (yuk == "") yuk = "100";
                    string masa_adi = dtbutton.Rows[i]["masa_adi"].ToString();
                    string masa_aciklama = dtbutton.Rows[i]["masa_aciklama"].ToString();
                    string fkSatislar = dtbutton.Rows[i]["fkSatislar"].ToString();

                    SimpleButton sb = new SimpleButton();
                    System.Drawing.Font fb = new System.Drawing.Font("Arial", 22);
                    System.Drawing.Font fk = new System.Drawing.Font("Arial", 16);

                    sb.AccessibleName = fkSatislar;
                    if (fkSatislar == "0" || fkSatislar == "")
                    {
                        sb.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                        sb.Font = fk;
                    }
                    else
                    {
                        sb.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                        sb.Font = fb;
                    }

                    sb.AccessibleDescription = pkid;
                    sb.Name = "Btn" + (i + 1).ToString();
                    sb.Text = masa_adi;
                    sb.Tag = pkid;
                    //double d = 0;
                    //double.TryParse(SatisFiyati, out d);
                    sb.ToolTip = pkid + "-" + masa_aciklama;//"Satış Fiyatı=" + d.ToString() + "\n Stok Adı:" + Stokadi;
                    sb.ToolTipTitle = pkid + "-" + masa_aciklama;//"Kodu: " + Barcode;
                    sb.Height = int.Parse(yuk);
                    sb.Width = int.Parse(gen);
                    sb.Click += new EventHandler(ButtonClick);
                    //sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseEnter);
                    sb.MouseDown += new System.Windows.Forms.MouseEventHandler(sb_MouseDown);
                    sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseMove);
                    sb.MouseUp += new System.Windows.Forms.MouseEventHandler(sb_MouseUp);

                    //if (ceOzelDizayn.Checked)
                    //{
                    //    string soldan = dtbutton.Rows[i]["soldan"].ToString();
                    //    if (soldan == "") soldan = lef.ToString();
                    //    sb.Left = int.Parse(soldan);

                    //    string ustden = dtbutton.Rows[i]["ustden"].ToString();
                    //    if (ustden == "") ustden = to.ToString();
                    //    sb.Top = int.Parse(ustden);
                    //}
                    //else
                    {
                        sb.Left = lef;
                        sb.Top = to;
                    }
                    //adı 15 karakterden büyükse
                    if (masa_adi.Length > 15)
                        sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    else
                        sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                    sb.ContextMenuStrip = contextMenuStrip1;
                    string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                    string imagedosya = exeDiz + "\\MasaResim\\" + pkid + ".png";
                    if (File.Exists(imagedosya))
                    {
                        Image im = new Bitmap(imagedosya);
                        sb.Image = new Bitmap(im, 45, 45);
                        sb.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                    }
                    if (i != 0 && (i + 1) % 3 == 0)
                    {
                        to = 0;
                        lef = lef + int.Parse(gen) + 5;
                    }
                    else
                    {
                        to += int.Parse(yuk) + 5;
                    }

                    //sb.Show();
                    //sb.SendToBack();
                    //DockPanel p1 = dockManager1.AddPanel(DockingStyle.Left);
                    //p1.Text = "Genel";
                    //DevExpress.XtraEditors.SimpleButton btn = new DevExpress.XtraEditors.SimpleButton();
                    //btn.Text = "Print...";

                    //Label l = new Label();
                    //l.Name = "lab" + (i + 1).ToString();
                    //l.Text = masa_aciklama;
                    //l.Left = sb.Left;
                    //l.Width = sb.Width;
                    //l.Height = 20;//sb.Height-50;
                    //l.Top = 0;//sb.Height - l.Height;
                    //l.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                    ////l.BackColor = System.Drawing.Color.Transparent;
                    ////l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    ////l.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    ////l.Click += new EventHandler(ButtonClick);
                    ////PButtonlar.Controls.Add(sb);
                    //sb.Controls.Add(l);

                    //Button odeme = new Button();
                    //odeme.Name = "bo" + (i + 1).ToString();
                    ////odeme.Text = "0,0 TL";
                    //odeme.Left = sb.Left;
                    //odeme.Width = sb.Width;
                    //odeme.Height = 30;//sb.Height-50;
                    //odeme.Top = sb.Height - l.Height-10;
                    //odeme.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                    //odeme.Click += new EventHandler(ButtonClickOdeme);
                    ////l.BackColor = System.Drawing.Color.Transparent;
                    ////l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    ////l.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    ////l.Click += new EventHandler(ButtonClick);
                    ////PButtonlar.Controls.Add(sb);
                    //sb.Controls.Add(odeme);

                    //l.Show();

                    xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Add(sb);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu: " + exp.Message);
                throw;
            }
            //if (((SimpleButton)sender).Tag != null)
            //    uruneklemdb(((SimpleButton)sender).Tag.ToString());
            //dockPanel1.Width = 900;
            //dockPanel1.Show();
            //xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Tag = "1";
            //for (int i = 0; i < TabHizliSatisGenel.Controls.Count; i++)
            //{
            //    //if (TabHizliSatisGenel.Controls[i].Name == HizliBarkodName)
            //    //  TabHizliSatisGenel.Controls[i].Text = "BOŞ";
            //    ((SimpleButton)(TabHizliSatisGenel.Controls[i])).Dispose();

            //}
        }
        private void sb_MouseEnter(object sender, EventArgs e)
        {
            //HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            //HizliTop = ((SimpleButton)sender).Top;
            //HizliLeft = ((SimpleButton)sender).Left;
            //HizliBarkodName = ((SimpleButton)sender).Name;
            //pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;
        }
        //bool suruklenmedurumu = false; //Class seviyesinde bir field(değişken) tanımlıyoruz ki,MouseDown da biz bunu true yapacağız,MouseUpta false değerini alacak ve MouseMove eventında true ise hareket edecek.     
        //Point ilkkonum; //Global bir değişken tanımlıyoruz ki, ilk tıkladığımız andaki konumunu çıkarmadığımızda buton mouse imlecinden daha aşağıdan hareket edecektir.
        private void sb_MouseDown(object sender, MouseEventArgs e)
        {
            //if (!cbDizayn.Checked) return;

            //suruklenmedurumu = true; //işlemi burada true diyerek başlatıyoruz.
            ((SimpleButton)sender).Cursor = Cursors.SizeAll; //SizeAll yapmamımızın amacı taşırken hoş görüntü vermek için
            //ilkkonum = e.Location; //İlk konuma gördüğünüz gibi değerimizi atıyoruz.
        }
        private void sb_MouseMove(object sender, MouseEventArgs e)
        {
            //if (suruklenmedurumu) // suruklenmedurumu==true ile aynı.
            //{
            //    ((SimpleButton)sender).Left = e.X + ((SimpleButton)sender).Left - (ilkkonum.X);
            //    // button.left ile soldan uzaklığını ayarlıyoruz. Yani e.X dediğimizde buton üzerinde mouseun hareket ettiği pixeli alacağız + butonun soldan uzaklığını ekliyoruz son olarakta ilk mouseın tıklandığı alanı çıkarıyoruz yoksa butonun en solunda olur mouse imleci. Alttakide aynı şekilde Y koordinati için geçerli
            //    ((SimpleButton)sender).Top = e.Y + ((SimpleButton)sender).Top - (ilkkonum.Y);
            //}
        }
        private void sb_MouseUp(object sender, EventArgs e)
        {
            //suruklenmedurumu = false; //Sol tuştan elimizi çektik artık yani sürükle işlemi bitti.
            ((SimpleButton)sender).Cursor = Cursors.Default; //İmlecimiz(Cursor) default değerini alıyor.
        }
        private void ButtonClick(object sender, EventArgs e)
        {
            //if (cbDizayn.Checked) return;

            if (((SimpleButton)sender).Tag != null)
            {
                string satisid = ((SimpleButton)sender).AccessibleName;
                string masaadi = ((SimpleButton)sender).Text;
                //((SimpleButton)sender).Enabled = false;
                frmSatis Satis = new frmSatis(0, int.Parse(satisid), masaadi, ((SimpleButton)sender).Tag.ToString());
                Satis.gbBaslik.Tag = ((SimpleButton)sender).Tag;
                Satis.gbBaslik.AccessibleDescription = ((SimpleButton)sender).AccessibleDescription;
                Satis.gbBaslik.AccessibleName = ((SimpleButton)sender).AccessibleName;
                Satis.txtpkSatislar.Text = ((SimpleButton)sender).AccessibleName;
                Satis.Satis1Toplam.Tag = ((SimpleButton)sender).AccessibleName;
                Satis.ShowDialog();

                if (Satis.txtpkSatislar.Text == "0")
                {
                    ((SimpleButton)sender).ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;

                    //((SimpleButton)sender).Tag = ((SimpleButton)sender).Tag.ToString();
                    //((SimpleButton)sender).Text = "1.Masa Aç";
                    ((SimpleButton)sender).AccessibleName = "0";
                    ((SimpleButton)sender).ToolTip = "Fiş No = 0";
                }
                else
                {
                    //((SimpleButton)sender).Text = "1.Masa Açık";
                    ((SimpleButton)sender).ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                    //satış ekranında kaydetin altına konuldu
                    //DB.ExecuteSQL("update Masalar set fkSatislar=" + Satis.txtpkSatislar.Text + " where pkMasalar=" + ((SimpleButton)sender).Tag.ToString());
                    ((SimpleButton)sender).AccessibleName = Satis.txtpkSatislar.Text;
                    ((SimpleButton)sender).ToolTip = "Fiş No =" + Satis.txtpkSatislar.Text;
                }
                DB.ExecuteSQL("update Masalar set fkSatislar=" + Satis.txtpkSatislar.Text + " where pkMasalar=" + ((SimpleButton)sender).Tag.ToString());

                ((SimpleButton)sender).Focus();
                Satis.Dispose();

                //((SimpleButton)sender).Enabled = true;
                //MessageBox.Show(((SimpleButton)sender).Tag.ToString());
                //SatisDetayEkle(((SimpleButton)sender).Tag.ToString());
                //yesilisikyeni();

                //KONTROL
                //DB.GetData();
            }
        }

        private void simpleButton9_Click_2(object sender, EventArgs e)
        {
           
        }

        private void lueKullanicilar_EditValueChanged(object sender, EventArgs e)
        {
            int si = gridView1.FocusedRowHandle;

            if (si < 0) return;

            DataRow dr = gridView1.GetDataRow(si);
            string arayanlar_id = dr["pkArayanlar"].ToString();

            
            DB.ExecuteSQL("update Arayanlar set fkKullanicilar="+lueKullanicilar.EditValue.ToString() +
                 " where pkArayanlar=" + arayanlar_id);

            //ArayanlariGetirBakilmadi();

            //gridView1.FocusedRowHandle = si;
        }

        private void müşteriKartıToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            int i = gridView3.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView3.GetDataRow(i);

            frmMusteriKarti kart = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            kart.ShowDialog();

            gridView3.FocusedRowHandle = i;
        }

        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int i = gridView3.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView3.GetDataRow(i);


            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();

            gridView3.FocusedRowHandle = i;
        }

        private void axCIDv51_OnCallerID(object sender, Axcidv5callerid.ICIDv5Events_OnCallerIDEvent e)
        {
            Caliyormu(e.phoneNumber, "0");//e.line);
        }

        private void simpleButton9_Click_3(object sender, EventArgs e)
        {
            Close();
        }

        //private void axCIDv51_OnCallerID(object sender, Axcidv5callerid.ICIDv5Events_OnCallerIDEvent e)
        //{
        //    Caliyormu(e.phoneNumber, "0");//e.line);
        //}
    }
}
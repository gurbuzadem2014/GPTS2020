using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Data.OleDb;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using GPTS.Include.Data;
using DevExpress.XtraTab;
using GPTS.islemler;
using System.Diagnostics;
using System.Xml;

namespace GPTS
{
    
    public partial class ucDepoTransfer : DevExpress.XtraEditors.XtraUserControl
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis = "";//AccessibleDescription
        decimal HizliMiktar = 1;
        bool ilkyukleme = false;
        short yazdirmaadedi = 1;

        public ucDepoTransfer()
        {
            InitializeComponent();
        }

        private void ucAnaEkran_Load(object sender, EventArgs e)
        {
            
            ilkyukleme = true;

            HizliSatisTablariolustur();

            timer1.Enabled = true;

            ilkyukleme = false;
            
            KullaniciListesi();

            Yetkiler();

            string Dosya = DB.exeDizini + "\\DepoTransfer.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }

            Depolar();
            HedefDepolar();

            DepolarGrid();
            HedefDepolarGrid();

            ModulYetkiler();
        }

        void ModulYetkiler()
        {
            //Kod,ModulAdi,Yetki
            DataTable dtYetkiler =  islemler.yetkiler.ModulYetkileri(int.Parse(DB.fkKullanicilar));
            foreach (DataRow row in dtYetkiler.Rows)
	        {
                string kod = row["Kod"].ToString();
                string yetki = row["Yetki"].ToString();

                if (kod == ((int)Degerler.Moduller.Satis).ToString())
                {
                    if(yetki == "True")
                      this.Enabled = true;
                else
                      this.Enabled = false;
                }
                else if (kod == ((int)Degerler.Moduller.SatisRaporlari).ToString())
                {
                    if (yetki == "True")
                      btnSatisRaporlari.Enabled = true;
                    else
                      btnSatisRaporlari.Enabled = false;
                }

        	}
        }

        void Depolar()
        {
            lueGonderenDepo.Properties.DataSource = DB.GetData("select * from Depolar with(nolock) --where fkDepoTipi=1");
            lueGonderenDepo.EditValue = 1;
        }

        void HedefDepolar()
        {
            lueHedefDepo.Properties.DataSource = DB.GetData("select * from Depolar with(nolock) --where fkDepoTipi=1");
            lueHedefDepo.EditValue = Degerler.fkDepolar;
        }
        
        void DepolarGrid()
        {
            repositoryItemLookUpEdit2.DataSource = DB.GetData("select * from Depolar with(nolock)");
            //repositoryItemLookUpEdit3.EditValue = 1;
        }

        void HedefDepolarGrid()
        {
            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from Depolar with(nolock)");
            //repositoryItemLookUpEdit3.EditValue = 1;
        }

        void KullaniciListesi()
        {
            lueKullanicilar.Properties.DataSource = DB.GetData("select pkKullanicilar,adisoyadi,KullaniciAdi from Kullanicilar  with(nolock) where durumu=1 ");
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }

        void Yetkiler()
        {
            string sql = @"SELECT ya.Yetki, p.Aciklama10,ya.Sayi,isnull(p.Aktif,0) as Aktif FROM  YetkiAlanlari ya with(nolock)  
            INNER JOIN Parametreler p with(nolock) ON ya.fkParametreler = p.pkParametreler
            WHERE ya.fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);
            
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string aciklama=dtYetkiler.Rows[i]["Aciklama10"].ToString();
                bool yetki = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                string sayi = dtYetkiler.Rows[i]["Sayi"].ToString();
                //bool aktif = Convert.ToBoolean(dtYetkiler.Rows[i]["Aktif"]);


                if (aciklama == "HizliButon")
                    xtraTabControl3.Visible = yetki;
                else if (aciklama == "HizButGen")
                {
                    int gen = 117;
                    int.TryParse(dtYetkiler.Rows[i]["Sayi"].ToString(),out gen);
                    dockPanel1.Tag = gen;
                    if (gen > 0)
                    {
                        dockPanel1.Width = gen;
                        dockPanel1.SendToBack();
                        lcKullanici.Tag = "1";
                    }
                }
                else if (aciklama == "SonSatisM")
                {
                    gcMusteriSatis.Visible = yetki;
                }
                else if (aciklama == "AlisFiyati")
                {
                    gridColumn31.Visible = yetki;
                }
            }
        }

        void HizliSatisTablariolustur()
        {
            xtraTabControl3.TabPages.Clear();
            DataTable dt = DB.GetData("select * from HizliGruplar with(nolock) where Aktif=1 order by SiraNo");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XtraTabPage xtab = new XtraTabPage();
                xtab.Text = dt.Rows[i]["HizliGrupAdi"].ToString();
                xtab.Tag = dt.Rows[i]["pkHizliGruplar"].ToString();
                xtraTabControl3.TabPages.Add(xtab);
                xtab.PageVisible = true;
            }
            xtraTabControl3.SelectedTabPageIndex = 0;
        }

        int x = 0;
        int y = 0;
        int p = 1;
        private void ButtonClick(object sender, EventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
            {
                SatisDetayEkle(((SimpleButton)sender).Tag.ToString());
                yesilisikyeni();
            }
        }

        private void ButtonClickSag(object sender, MouseEventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
                urunekle(((SimpleButton)sender).Tag.ToString());
        }

        void Hizlibuttonlariyukle2(bool bdegisiklikvar)
        {
            if (bdegisiklikvar == false && xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Count > 0) 
                    return;

            xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();

            int to = 0;
            int lef = 0;

            DataTable dtbutton = DB.GetData(@"select pkHizliStokSatis,pkStokKarti,Barcode,HizliSatisAdi,Stokadi,sf.SatisFiyatiKdvli as SatisFiyati from HizliStokSatis  h with(nolock)
            left join (select pkStokKarti,Stokadi,fkStokGrup,HizliSatisAdi,Barcode,HizliSiraNo 
            from StokKarti with(nolock) ) sk on sk.pkStokKarti=h.fkStokKarti 
            left join SatisFiyatlari sf on sf.fkStokKarti=h.fkStokKarti  and sf.fkSatisFiyatlariBaslik=(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik where Tur=1)
            where fkHizliGruplar=" + xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Tag.ToString() +
            " order by pkHizliStokSatis");

            int h = 80;//dockPanel1.Height / 7;
            int w = 110;//dockPanel1.Width / 5;
            for (int i = 0; i < dtbutton.Rows.Count; i++)
            {
                string pkid = dtbutton.Rows[i]["pkStokKarti"].ToString();
                string Barcode = dtbutton.Rows[i]["Barcode"].ToString();
                string HizliSatisAdi = dtbutton.Rows[i]["HizliSatisAdi"].ToString();
                string Stokadi = dtbutton.Rows[i]["Stokadi"].ToString();
                string SatisFiyati = dtbutton.Rows[i]["SatisFiyati"].ToString();

                SimpleButton sb = new SimpleButton();
                sb.AccessibleDescription = dtbutton.Rows[i]["pkHizliStokSatis"].ToString();
                sb.Name = "Btn" + (i + 1).ToString();
                sb.Text = HizliSatisAdi;
                sb.Tag = Barcode;
                double d = 0;
                double.TryParse(SatisFiyati, out d);
                sb.ToolTip = "Satış Fiyatı=" + d.ToString() + "\n Stok Adı:" + Stokadi;
                sb.ToolTipTitle = "Kodu: " + Barcode;
                sb.Height = h;
                sb.Width = w;
                sb.Click += new EventHandler(ButtonClick);
                sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseEnter);
                sb.Left = lef;
                sb.Top = to;
                //adı 15 karakterden büyükse
                if (HizliSatisAdi.Length > 15)
                    sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                else
                    sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                sb.ContextMenuStrip = contextMenuStrip1;
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string imagedosya = exeDiz + "\\HizliSatisButtonResim\\" + pkid + ".png";
                if (File.Exists(imagedosya))
                {
                    Image im = new Bitmap(imagedosya);
                    sb.Image = new Bitmap(im, 45, 45);
                    sb.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                }
                if (i != 0 && (i + 1) % 7 == 0)
                {
                    to = 0;
                    lef = lef + w;
                }
                else
                {
                    to += h;
                }

                sb.Show();
                sb.SendToBack();
                //DockPanel p1 = dockManager1.AddPanel(DockingStyle.Left);
                //p1.Text = "Genel";
                //DevExpress.XtraEditors.SimpleButton btn = new DevExpress.XtraEditors.SimpleButton();
                //btn.Text = "Print...";
                xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Add(sb);
                /*
                //LABEL OLUŞTUR
                Label l = new Label();
                l.Name = "lab" + (i + 1).ToString();
                l.Text = Barcode;
                //l.Left = sb.Left;
                //l.Width = sb.Width;
                //l.Height = sb.Height-50;
                l.Top = sb.Height - 20;
                l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                l.BackColor = System.Drawing.Color.Transparent;
                l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                l.BorderStyle = System.Windows.Forms.BorderStyle.None;
                //l.Click += new EventHandler(ButtonClick);
                PButtonlar.Controls.Add(sb);
                sb.Controls.Add(l);
                l.Show();
                */
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

        //satış iptal butonu
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }

            string s = formislemleri.MesajBox("Transfer İptal Edilsin mi?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            int sonuc = 0;
            string pkSatislar = Satis1Toplam.Tag.ToString();

            sonuc = DB.ExecuteSQL("Delete from DepoTransferDetay where fkDepoTransfer=" + pkSatislar);

            if (sonuc == 0)
                sonuc = DB.ExecuteSQL("Delete from DepoTransfer where pkDepoTransfer=" + pkSatislar);
            else
            {
                yesilisikyeni();
                return;
            }
            

            //if (sonuc != "Transfer Silindi.")
              //  MessageBox.Show(sonuc);

            pkDepoTransfer.Text = "0";

            SatisTemizle();

            Satis1ToplamGetir(1);

            deGuncellemeTarihi.EditValue = null;
            deGuncellemeTarihi.Enabled = false;

            yesilisikyeni();

            //lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //SATIR SİL
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }

            #region trans başlat
            if (DB.conTrans == null)
                DB.conTrans = new SqlConnection(DB.ConnectionString());

            if (DB.conTrans.State == ConnectionState.Closed)
                DB.conTrans.Open();
            DB.transaction = DB.conTrans.BeginTransaction("DepoTransferTransaction");
            bool islembasarili = true;
            #endregion

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string sonuc = DB.ExecuteSQLTrans("DELETE FROM DepoTransferDetay WHERE pkDepoTransferDetay=" + dr["pkDepoTransferDetay"].ToString());
            if (sonuc.Substring(0, 1) == "H")
                islembasarili = false;

            //gridView1.DeleteSelectedRows();
            if (islembasarili & gridView1.DataRowCount == 1)
            {
                sonuc = DB.ExecuteSQLTrans("DELETE FROM DepoTransfer WHERE pkDepoTransfer=" + pkDepoTransfer.Text);
                if (sonuc.Substring(0, 1) == "H")
                {
                    //formislemleri.Mesajform("Hata Oluştu", "K", 200);
                    islembasarili=false;
                }
               
            }

            #region trans. işlemi
            if (islembasarili)
            {
                //local sunucu
                DB.transaction.Commit();

                if (gridView1.DataRowCount == 1)
                {
                    pkDepoTransfer.Text = "0";
                    Satis1Toplam.Tag = "0";
                    deGuncellemeTarihi.EditValue = null;
                    deGuncellemeTarihi.Enabled = false;

                    lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
                }
            }
            else
            {
                //locak sunucuyuyu geri al
                DB.transaction.Rollback();
                //
            }
            #endregion
            
            DB.conTrans.Close();

            

            yesilisikyeni();
        }

        void YeniSatisEkle()
        {
            if (Satis1Toplam.Tag.ToString() == "0")
                YeniDepoTransfer();
        }

        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string kod =
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (kod == "" && gridView1.DataRowCount == 0)
                {
                    yesilisikyeni();
                    return;
                }
                SatisDetayEkle(kod);
                yesilisikyeni();
            }
            //müşteri koduna git
            else if (e.KeyValue == 70)
            {
                //DevExpress.XtraEditors.LookUpEdit lookUpEdit = sender as DevExpress.XtraEditors.LookUpEdit;
                //if (lookUpEdit != null)
                //BeginInvoke(new MethodInvoker(delegate { lueFiyatlar.ShowPopup(); }));
            }
            //ctrl + i iskonto
            else if (e.Control && e.KeyValue == 222)
            {
                //iskontoyagit_ctrli();
            }
            else if (e.Control && e.Shift && gridView1.DataRowCount > 0)
            {
                //üst satırı kopyala
                DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);
                SatisDetayEkle(dr["Barcode"].ToString());
                //gridView1.ShowEditor();
                yesilisikyeni();
                gridView1.ShowEditor();
                gridView1.ActiveEditor.SelectAll();
                gridView1.CloseEditor();
                //Application.DoEvents();
                //Application.DoEvents();
                //SendKeys.Send("{ENTER}");
            }
        }

        void urunaraekle(string ara)
        {
            frmStokAra StokAra = new frmStokAra(ara);
            
            StokAra.Tag = "0";
            StokAra.ShowDialog();

            if (StokAra.TopMost == false)
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    SatisDetayEkle(dr["Barcode"].ToString());

                    //if (StokAra.gridView1.GetSelectedRows()[i] >= 0)
                    //{
                    //    DataRow dr = StokAra.gridView1.GetDataRow(i);
                    //    SatisDetayEkle(dr["Barcode"].ToString());
                    //}
                }
            }
            StokAra.Dispose();
            yesilisikyeni();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle != -2147483647)
                gridView1.AddNewRow();


            string girilen ="";
            try
            {
                girilen =
                            ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
            }
            catch
            {
                girilen = "";
            }
            urunaraekle(girilen);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            if (gridView1.FocusedRowHandle != -2147483647)
                gridView1.AddNewRow();

            string girilen =
            ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;


            urunaraekle(girilen);
        }
        
        private void urunekle(string barkod)
        {
            if (barkod == "") return;

            if (lueGonderenDepo.EditValue == lueHedefDepo.EditValue)
            {
                formislemleri.Mesajform("Aynı Depo Olamaz", "K", 150);
                return;
            }

            DataTable dturunler = DB.GetData("select pkStokKarti From StokKarti with(nolock) WHERE Barcode='" + barkod + "'");
            if (dturunler.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                StokKartiHizli.ceBarkod.EditValue = barkod;
                StokKartiHizli.ShowDialog();

                if (StokKartiHizli.TopMost == true)
                {
                    dturunler = DB.GetData("select * From StokKarti with(nolock) WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
                }
                else
                {
                    StokKartiHizli.Dispose();
                    return;
                }
                StokKartiHizli.Dispose();
            }
            SatisDetayEkle(barkod);
        }

        void resimolustur(SimpleButton sb)
        {
            x = x + 25;
            y = y + 25;
            PictureBox pictureBox = new PictureBox();
            //xtraTabPage1.Controls.Add(pictureBox);
            pictureBox.Location = new System.Drawing.Point(150 + x, 50 + y);
            pictureBox.Name = "pictureBox" + p;
            pictureBox.Size = new System.Drawing.Size(150, 100);
            pictureBox.Image = sb.Image;//Image.FromFile("10-.jpg");//
            pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox.TabIndex = 3;
            pictureBox.TabStop = false;
            ((System.ComponentModel.ISupportInitialize)(pictureBox)).BeginInit();
            pictureBox.BringToFront();
            if (x > 200) x = 0;
            if (y > 200) y = 0;
            p++;
        }

        private bool SatisVar()
        {

            if (gridView1.DataRowCount == 0)
            {
                //Showmessage("Önce Satış Yapınız!", "K");
                return false;
            }
            return true;
        }

        void SatisTemizle()
        {
            //if (AcikSatisindex == 1)
            {
                Satis1Toplam.Tag = 0;
                Satis1Toplam.Text = "0,0";
                pkDepoTransfer.Text = "0";
            }

            pkDepoTransfer.Text = "0";

            gcMusteriSatis.DataSource = null;

            btnAciklamaGirisi.ToolTip = ""; 
            //fiş düzeltten sonra 1.satışa git
            //if(AcikSatisindex==4)
                Satis1ToplamGetir(1);
        }

        void KaydetYazdir(bool yazdirilsinmi)
        {
            if (pkDepoTransfer.Text == "0")
            {
                yesilisikyeni();
                return;
            }

            #region Müşteri id al
            //string fkFirma = "0";
            //switch (AcikSatisindex)
            //{
            //    case 1:
            //        {
            //            fkFirma = Satis1Firma.Tag.ToString();
            //            break;
            //        }
            //    case 4:
            //        {
            //            fkFirma = gCSatisDuzen.Tag.ToString();
            //            break;
            //        }
            //}
            #endregion

            #region uyarılar ve update
            //TimeSpan ts =DateTime.Now - deGuncellemeTarihi.DateTime;
            //if (ts.Hours > 1 && deGuncellemeTarihi.EditValue != null)// &&  AcikSatisindex < 4)
            //{
            //    DialogResult secim;
            //    secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Tarihi Farkı "+ ts.Days.ToString()+ " Gün " + 
            //        ts.Hours.ToString()+" Saattir. Devam Etmek İstiyormusunuz ?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,System.Windows.Forms.MessageBoxDefaultButton.Button2);
               
            //    if(secim == DialogResult.No)
            //    {
            //        yesilisikyeni();
            //        return;
            //    }
            //}
            //if (deGuncellemeTarihi.DateTime>DateTime.Now)
            //{
            //    DialogResult secim;
            //    secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış Zamanı Bugünden Büyüktür. Yinede Kaydetmek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);//, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //    if (secim == DialogResult.No) return;
            //}

            #endregion

            yesilisikyeni();
        }

        void StokDurumGuncelle()
        {
            for (int s = 0; s < gridView1.DataRowCount; s++)
            {
                DataRow dr = gridView1.GetDataRow(s);
                string pkStokKartiid = dr["pkStokKartiid"].ToString();
                string pkStokKarti = dr["fkStokKarti"].ToString();

                decimal satilanadet = 0, mevcut = 0;

                decimal.TryParse(dr["Adet"].ToString(), out satilanadet);

                if (pkStokKartiid == "") pkStokKartiid = pkStokKarti;

                DataTable dtStokDurum =
                DB.GetData("select * from StokDurum with(nolock) where fkStokKarti=" + pkStokKartiid + " and fkDepolar=1");
                if (dtStokDurum.Rows.Count == 0)
                {
                    string sql = "insert into StokDurum (fkStokKarti,fkDepolar,guncelleme_tarihi,fkKullanicilar)" +
                    " values("+pkStokKartiid+",1,getdate(),"+DB.fkKullanicilar+")";
                    int sonuc = DB.ExecuteSQL(sql);
                }
                //string pkStokDurum = dtStokDurum.Rows[0]["pkStokDurum"].ToString();

                //iade değilse ana birim üzerinden takip et
                if (satilanadet > 0)
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokDurum SET miktar=isnull(miktar,0)-" + mevcut.ToString().Replace(",", ".") +
                             " where fkDepolar=1 and fkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);
                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
                else
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokDurum SET miktar=isnull(miktar,0)+" + mevcut.ToString().Replace("-", "").Replace(",", ".") +
                             " where fkDepolar=1 and fkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);
                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
            }
        }

        void MevcutlariGuncelle()
        {
            for (int s = 0; s < gridView1.DataRowCount; s++)
            {
                DataRow dr = gridView1.GetDataRow(s);
                string pkStokKartiid = dr["pkStokKartiid"].ToString();
                string pkStokKarti = dr["fkStokKarti"].ToString();

                decimal satilanadet = decimal.Parse(dr["Adet"].ToString());
                decimal mevcut = 0;

                if (pkStokKartiid == "") pkStokKartiid = pkStokKarti;

                //DB.ExecuteSQL("update StokKarti set Mevcut=dbo.fon_StokMevcut(" + pkStokKartiid + ") WHERE pkStokKarti =" + pkStokKartiid);

                //iade değilse ana birim üzerinden takip et
                if (satilanadet > 0)
                {
                    mevcut =  satilanadet;
                    string sql="UPDATE StokKarti SET Aktarildi=" + Degerler.AracdaSatis+",Aktif=1,Mevcut=isnull(Mevcut,0)-" + mevcut.ToString().Replace(",", ".") +
                             " where pkStokKarti=" + pkStokKartiid;
                    int sonuc =  DB.ExecuteSQL(sql);

                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " +sonuc, sql);
                    }
                }
                else
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokKarti SET Aktarildi=" + Degerler.AracdaSatis + ",Aktif=1,Mevcut=isnull(Mevcut,0)+" + mevcut.ToString().Replace("-", "").Replace(",", ".") +
                             " where pkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);
                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
            
            }
        }

        void DepoMevcurlariGuncelle()
        {
            for (int s = 0; s < gridView1.DataRowCount; s++)
            {
                DataRow dr = gridView1.GetDataRow(s);
                string pkStokKartiid = dr["pkStokKartiid"].ToString();
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string fkDepolar = dr["fkDepolar"].ToString();

                decimal satilanadet = decimal.Parse(dr["Adet"].ToString());
                //decimal mevcut = 0;

                if (pkStokKartiid == "") pkStokKartiid = fkStokKarti;

                ArrayList list_depo = new ArrayList();
                list_depo.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                list_depo.Add(new SqlParameter("@fkDepolar", fkDepolar));
                list_depo.Add(new SqlParameter("@Miktar", satilanadet));
                list_depo.Add(new SqlParameter("@AlisSatis", "2"));

                string sonuc1 = DB.ExecuteSQL("HSP_StokKartiDepo_EkleGuncelle @fkStokKarti,@fkDepolar,@Miktar,@AlisSatis", list_depo);
                /*
                if (satilanadet > 0)
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokKarti SET Aktarildi=" + Degerler.AracdaSatis + ",Aktif=1,Mevcut=isnull(Mevcut,0)-" + mevcut.ToString().Replace(",", ".") +
                             " where pkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);

                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
                else
                {
                    mevcut = satilanadet;
                    string sql = "UPDATE StokKarti SET Aktarildi=" + Degerler.AracdaSatis + ",Aktif=1,Mevcut=isnull(Mevcut,0)+" + mevcut.ToString().Replace("-", "").Replace(",", ".") +
                             " where pkStokKarti=" + pkStokKartiid;
                    int sonuc = DB.ExecuteSQL(sql);
                    if (sonuc == -1)
                    {
                        formislemleri.Mesajform("Mevcut Güncellenirken Hata Oluştu", "K", 200);
                        DB.logayaz("Mevcut Güncellenirken Hata Oluştu " + sonuc, sql);
                    }
                }
                 */ 
            }
        }

        private void simpleButton37_Click(object sender, EventArgs e)
        {
            //depo transfer işlemi
            //kaynak depodan hedef depoya transfer
            if (pkDepoTransfer.Text == "0")
            {
                yesilisikyeni();
                return;
            }

            if (DB.GetData("select COUNT(0) aynidepokaydi from DepoTransferDetay where fkDepoTransfer=" + 
                pkDepoTransfer.Text + " and fkDepolar=fkDepolar_alici").Rows[0][0].ToString() != "0")
            {
                formislemleri.Mesajform("Aynı Depoya Transfer İşlemi Yapılamaz!","K",150);
                return;
            }

            string s = formislemleri.MesajBox("Transfer İşlemini Onaylıyormusunuz?", Degerler.mesajbaslik, 3, 2);
            if (s == "0") return;

            if (deGuncellemeTarihi.EditValue == null)
                deGuncellemeTarihi.DateTime = DateTime.Now;

            int sonuc=0;
            #region trans başlat
            #endregion
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string fkDepolar = dr["fkDepolar"].ToString();
                string hedef_depo = dr["fkDepolar_alici"].ToString();
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string Adet = dr["Adet"].ToString().Replace(",",".");
                string sql="";
                //HSP_StokKartiDepo_EkleGuncelle 1,1,52,0
                //StokKartiDepo
                //1-alış  2-satış 
                //önce kaynak depodan azalacak satış
                sql = "HSP_StokKartiDepo_EkleGuncelle " + fkStokKarti + "," + fkDepolar + "," + Adet + ",2";
                sonuc = DB.ExecuteSQL(sql);

                //alış gibi hedef depo artacak
                sql = "HSP_StokKartiDepo_EkleGuncelle " + fkStokKarti + "," + hedef_depo + "," + Adet + ",1";
                sonuc = DB.ExecuteSQL(sql);
            }

            DB.ExecuteSQL("update DepoTransfer set transfer_tarihi='"+deGuncellemeTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:dd")+"' where pkDepoTransfer=" + pkDepoTransfer.Text);

            #region trans başlat
            #endregion

            DB.ExecuteSQL("update DepoTransfer set siparis=1 where pkDepoTransfer=" + pkDepoTransfer.Text);

            SatisTemizle();

            pkDepoTransfer.Text = "";
            
            yesilisikyeni();
            //sadece kaydet yazdırma yok
            //KaydetYazdir(false);
        }
       
        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }

        void RaporOnizleme(bool Disigner)
        {
            string fisid = pkDepoTransfer.Text;
            //if (lueFis.EditValue != null)
            //  fisid = lueFis.EditValue.ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Satislar s with(nolock)
            inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
            inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
            where s.pkSatislar =" + fisid;
            Barkod.DataSource = DB.GetData(sql);

            RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
            //rapor.DataSource = gCPerHareketleri.DataSource;
            //rapor.CreateDocument();
            Barkod.LoadLayout(RaporDosyasi);
            //rapor.FindControl("xlKasaAdi", true).Text = lueKasa.Text + " " + ilkdate.DateTime.ToString("dd.MM.yyyy") +
            //    "-" + sondate.DateTime.ToString("dd.MM.yyyy");
            if (Disigner)
                Barkod.ShowDesignerDialog();
            else
                Barkod.ShowRibbonPreview();
        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;
            int i = gridView1.FocusedRowHandle;
            if (i < 0)
            {
                i = gridView1.DataRowCount;
                i--;
            }
            DataRow dr = gridView1.GetDataRow(i);
            frmStokKarti StokKarti = new frmStokKarti();
            //string pkStokKartiid = dr["pkStokKartiid"].ToString();
            //if (pkStokKartiid == "" || pkStokKartiid == "0")
                DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            //else
              //  DB.pkStokKarti = int.Parse(pkStokKartiid);
            StokKarti.ShowDialog();

            FisSatisAlisFiyatlarininiDegis(DB.pkStokKarti.ToString());

//            DB.ExecuteSQL(@"UPDATE SatisDetay SET  NakitFiyat=sk.SatisFiyati,SatisFiyati=sk.SatisFiyati From StokKarti sk 
//            where SatisDetay.fkStokKarti=sk.pkStokKarti and  pkSatisDetay=" + dr["pkSatisDetay"].ToString());

            //Fiyatlarigetir();
            //xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();
            yesilisikyeni();
            //Satis1SonKayidaGit();
        }

        private void repositoryItemButtonEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            string girilen =
            ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;

            if (e.KeyValue == 13 && girilen == "")
                return;

            if (e.KeyCode == Keys.Space)
            {
                simpleButton4_Click(sender, e);
                //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                //xtraTabPage5_Click( sender, e);
            }
            //*
            if (e.KeyValue == 223 || e.KeyValue == 106)
            {
                string kod =
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (kod == "")
                {
                    gridView1.SetFocusedRowCellValue("Barcode", "");
                    return;
                }
                int adetvar = kod.IndexOf("*");
                decimal badet = 1;
                //string bbarkod = barkod;
                if (adetvar > 0)
                {
                    //badet = float.Parse(kod.Substring(0, adetvar));
                    decimal.TryParse(kod.Substring(0, adetvar), out badet);
                    //bbarkod = barkod.Substring(adetvar + 1, barkod.Length - (adetvar + 1));
                    HizliMiktar = badet;
                }
                gridView1.SetFocusedRowCellValue("Adet", badet);
                gridView1.SetFocusedRowCellValue(gridColumn3, "1");
                gridView1.SetFocusedRowCellValue(gridColumn3, badet);
                gridView1.CloseEditor();
                gridView1.SetFocusedRowCellValue("Barcode", "");
                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //iade
            if (e.KeyValue == 222)
            {
                string kod =
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (kod == "")
                {
                    gridView1.SetFocusedRowCellValue("Barcode", "");
                    return;
                }
                int adetvar = kod.IndexOf("*");
                int badet = 1;
                //string bbarkod = barkod;
                if (adetvar > 0)
                {
                    badet = int.Parse(kod.Substring(0, adetvar));
                    //bbarkod = barkod.Substring(adetvar + 1, barkod.Length - (adetvar + 1));

                }
                //gridView1.SetFocusedRowCellValue("iade", "true");
                //gridView1.SetFocusedRowCellValue("Barcode", "");
                //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                yesilisikyeni();
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            //AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            //AppearanceDefault appError = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorRed = new AppearanceDefault(Color.OrangeRed);

            //Font fontkucuk = new Font("Times New Roman", 9.0f, FontStyle.Regular);
            //Font fontnormal = new Font("Times New Roman", 11.0f,FontStyle.Underline);
            //Font fontbuyuk = new Font("Times New Roman", 12.0f,FontStyle.Bold);

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            //else if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["AlisFiyati"].ToString() != "")
            //{
            //    decimal iskontotutar = 0;
            //    decimal.TryParse(dr["iskontotutar"].ToString(),out iskontotutar);

            //    decimal SatisFiyati = 0;
            //    decimal.TryParse(dr["SatisFiyati"].ToString(), out SatisFiyati);
            //    SatisFiyati = SatisFiyati - iskontotutar;

            //    decimal AlisTutar = 0;
            //    decimal.TryParse(dr["AlisFiyati"].ToString(),out AlisTutar);

            //    if (SatisFiyati - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //        e.Appearance.BackColor2 = Color.White;
            //    }
            //       // AppearanceHelper.Apply(e.Appearance, appError);
            //    if(iskontotutar==0)
            //        e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
            //}
           
            //else if (e.Column.FieldName == "Adet" && dr["Adet"].ToString() == "0")
            //{
            //    e.Appearance.BackColor = Color.Red;
            //    e.Appearance.BackColor2 = Color.LightCyan;
            //    //AppearanceHelper.Apply(e.Appearance, appError);
            //}
            //else if (e.Column.FieldName == "Mevcut" && dr["KritikMiktar"].ToString() != "" && dr["Mevcut"].ToString() != "")
            //{
            //    decimal KritikMiktar = Convert.ToDecimal(dr["KritikMiktar"].ToString());
            //    decimal Mevcut = Convert.ToDecimal(dr["Mevcut"].ToString());
            //    if (KritikMiktar > Mevcut)
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //        e.Appearance.BackColor2 = Color.YellowGreen;
            //        //AppearanceHelper.Apply(e.Appearance, appErrorRed);
            //    }
            //}
            //else if (e.Column.FieldName == "SatisFiyati")
            //{
            //    decimal isk = 0;
            //    decimal.TryParse(dr["iskontotutar"].ToString(), out isk);

            //    if (isk > 0)
            //        gridColumn4.AppearanceCell.Font = fontbuyuk;
            //}

        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string iade = View.GetRowCellDisplayText(e.RowHandle, View.Columns["iade"]);
                string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
               // if (AcikSatisindex == 1)
                 //   e.Appearance.BackColor = Satis1Toplam.BackColor;

                if (Fiyat.Trim() == "0")
                {
                    e.Appearance.BackColor = Color.Red;
                }

            }
        }

        private void satışİptalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);
        }

        int getCheckedCount()
        {
            int count = 0;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                if ((bool)gridView1.GetRowCellValue(i, gridView1.Columns["iade"]) == true)
                    count++;
            }
            return count;
        }

        void CheckAll()
        {
            DB.ExecuteSQL("update SatisDetay set Adet=abs(Adet)*-1,iade=1 where fkSatislar=" + pkDepoTransfer.Text);
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{
            //    gridView1.SetRowCellValue(i, gridView1.Columns["iade"], true);
            //}
        }
        void UnChekAll()
        {
            DB.ExecuteSQL("update SatisDetay set Adet=abs(Adet),iade=0 where fkSatislar=" + pkDepoTransfer.Text);
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{
            //    gridView1.SetRowCellValue(i, gridView1.Columns["iade"], false);
            //}
        }

        private void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {

            if (e.Column == (sender as GridView).Columns["iade"])
            {
                e.Info.InnerElements.Clear();
                e.Info.Appearance.ForeColor = Color.Blue;
                e.Painter.DrawObject(e.Info);
                DrawCheckBox(e.Graphics, e.Bounds, getCheckedCount() == gridView1.DataRowCount);
                e.Handled = true;
            }


            if (e.Column == null) return;
            Rectangle rect = e.Bounds;
            ControlPaint.DrawBorder3D(e.Graphics, e.Bounds);
            Brush brush =
                e.Cache.GetGradientBrush(rect, e.Column.AppearanceHeader.BackColor,
                e.Column.AppearanceHeader.BackColor2, e.Column.AppearanceHeader.GradientMode);
            rect.Inflate(-1, -1);
            // Fill column headers with the specified colors.
            e.Graphics.FillRectangle(brush, rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            // Draw the filter and sort buttons.
            foreach (DevExpress.Utils.Drawing.DrawElementInfo info in e.Info.InnerElements)
            {
                try
                {
                    DevExpress.Utils.Drawing.ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
                }
                catch //(Exception exp)
                {
                }

            }
            e.Handled = true;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
                urunekle(((SimpleButton)sender).Tag.ToString());
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmHizliButtonDuzenle HizliButtonDuzenle = new frmHizliButtonDuzenle();
            HizliButtonDuzenle.Top = HizliTop;
            HizliButtonDuzenle.Left = HizliLeft;
            HizliButtonDuzenle.oncekibarkod.Text = HizliBarkod;
            HizliButtonDuzenle.oncekibarkod.Tag = pkHizliStokSatis;
            HizliButtonDuzenle.ShowDialog();
            Hizlibuttonlariyukle2(true);
        }

        private void ürünBulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton4_Click(sender, e);
        }

        private void sb_MouseEnter(object sender, EventArgs e)
        {
            HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            HizliTop = ((SimpleButton)sender).Top;
            HizliLeft = ((SimpleButton)sender).Left;
            HizliBarkodName = ((SimpleButton)sender).Name;
            pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;
        }

        void FisGetir(string fkDepoTransfer)
        {
            
            frmFisTrasferGecmis FisNoBilgisi = new frmFisTrasferGecmis(true);
            FisNoBilgisi.fisno.EditValue = fkDepoTransfer;
            FisNoBilgisi.ShowDialog();

            if (FisNoBilgisi.btnFisDuzenle.Tag.ToString() == "1")
            {
                pkDepoTransfer.Text = fkDepoTransfer;
                Satis1Toplam.Tag = fkDepoTransfer;

                //if (gCSatisDuzen.Visible == true)
                //{
                //    formislemleri.Mesajform("Önce Fiş Düzenlemeyi Kapatınız", "K");
                //    return;
                //}
                // if (FisNoBilgisi.SatisDurumu.Tag.ToString() == ((int)Degerler.SatisDurumlari.Teklif).ToString())

                //deGuncellemeTarihi.DateTime = DateTime.Now;

                //DB.ExecuteSQL("update Satislar Set Siparis=0 where pkSatislar=" + pkSatis);


                //fisduzenaciksatis(false);

                DepoTransferGetir();

                //BakiyeGetirSecindex(gCSatisDuzen.Tag.ToString(),0);
                //Yenile();
            }
            FisNoBilgisi.Dispose();
            //FisListesi();
             
            yesilisikyeni();
        }

        void DepoTransferGetir()
        {
            string pkDepoTransfer = "0";

            //if (AcikSatisindex == 1)
                pkDepoTransfer = Satis1Toplam.Tag.ToString();

            DataTable dtSatislar = DB.GetData("select * from  DepoTransfer with(nolock) where pkDepoTransfer=" + pkDepoTransfer);
            if (pkDepoTransfer == "0")
            {
                //lueSatisTipi.EditValue = Degerler.fkSatisDurumu;
                deGuncellemeTarihi.Enabled = false;
                deGuncellemeTarihi.EditValue = null;
                return;
            }

            if (dtSatislar.Rows.Count == 0)
            {
                //Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            string fkDepolar = dtSatislar.Rows[0]["fkDepolar"].ToString();
            string transfer_tarihi = dtSatislar.Rows[0]["transfer_tarihi"].ToString();
            string firmaadi = dtSatislar.Rows[0]["fkDepolar"].ToString();

            deGuncellemeTarihi.Enabled = true;
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            frmStokDepoMevcut StokDepoMevcut = new frmStokDepoMevcut();
            StokDepoMevcut.ShowDialog();
                //frmSatisRaporlari Satislar = new frmSatisRaporlari();
                //Satislar.fişDüzenleToolStripMenuItem.Enabled = true;
                //Satislar.ShowDialog();

                //string fisno = Satislar.fişDüzenleToolStripMenuItem.Tag.ToString();
                #region fiş düzenle
                //if (fisno != "0")
                {
                    //if (gCSatisDuzen.Visible == true)
                    //{
                    //    formislemleri.Mesajform("Önce Fiş Düzenlemeyi Kapatınız","K");
                    //    return;
                    //}
                    //pkSatisBarkod.Text = fisno;

                    //DataTable dt = DB.GetData("select pkSatislar,Siparis,fkKullanici From Satislar with(nolock) where pkSatislar=" + pkSatisBarkod.Text);
                    //if (dt.Rows.Count == 0)
                    //{
                    //    Showmessage("Fiş Bulunamadı.", "K");
                    //    return;
                    //}
                    //if (dt.Rows[0]["Siparis"].ToString() == "False")
                    //{
                    //    Showmessage("Fiş Açık Lütfen Yenileyiniz. Kullanıcı=" + dt.Rows[0]["fkKullanici"].ToString(), "K");
                    //    return;
                    //}
                    //FisGetir(fisno);
                }
                #endregion
            yesilisikyeni();
        }

        private void cariSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //musteriara();
        }

        private void yenidenAdlandırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HizliButtonAdiDegis ButtonAdiDegis = new HizliButtonAdiDegis();
            ButtonAdiDegis.Top = HizliTop;
            ButtonAdiDegis.Left = HizliLeft;
            ButtonAdiDegis.oncekibarkod.Text = HizliBarkod;
            ButtonAdiDegis.oncekibarkod.Tag = pkHizliStokSatis;
            ButtonAdiDegis.ShowDialog();

            Hizlibuttonlariyukle2(true);
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            string pkSatis = pkDepoTransfer.Text;
            
            if (pkSatis == "0")
            {
                //Showmessage("Önce Satış Yapınız!", "K");
                return;
            }
            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.Tag = "Satis";
            fFisAciklama.memoozelnot.Tag = pkSatis;
            //fFisAciklama.memoozelnot.Text = btnAciklamaGirisi.ToolTip;
            fFisAciklama.ShowDialog();

            //btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;

            fFisAciklama.Dispose();

            yesilisikyeni();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            string str = ActiveControl.Name;
            this.Dispose();
        }

        private void simpleButton20_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        public bool OnceSatisYapiniz()
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.Text = "Önce Satış Yapınız!";
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.Show();
                return false;
            }
            return true;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE HizliStokSatis Set fkStokKarti=0 where pkHizliStokSatis=" + pkHizliStokSatis);

            Hizlibuttonlariyukle2(true);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (HizliBarkodName == "")
            {
                toolStripMenuItem2.Enabled = false;
                toolStripMenuItem3.Enabled = false;
            }
            else
            {
                toolStripMenuItem2.Enabled = true;
                toolStripMenuItem3.Enabled = true;
            }
        }

        private void Satis3Toplam_Click(object sender, EventArgs e)
        {
            Satis1ToplamGetir(3);
        }

        void fisduzenaciksatis(bool sadeceindex)
        {
            Satis1ToplamGetir(4);

            if (sadeceindex == true)
                yesilisikyeni();
        }

        private void Satis4Toplam_Click(object sender, EventArgs e)
        {
            fisduzenaciksatis(true);
        }

        private void repositoryItemCalcEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string yenimiktar =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue.ToString();
                MiktarDegistir(yenimiktar);
                yesilisikyeni();
            }
        }

        void MiktarDegistir(string yenimiktar)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //if (dr["iade"].ToString() == "True")
            //{
            //    decimal miktar = decimal.Parse(yenimiktar);
            //    if (miktar > 0)
            //        miktar = miktar * -1;
            //    yenimiktar = miktar.ToString();
            //}

            string pkDepoTransferDetay = dr["pkDepoTransferDetay"].ToString();
            DB.ExecuteSQL("UPDATE DepoTransferDetay SET transfer_miktar=" + yenimiktar.Replace(",", ".") + " where pkDepoTransferDetay=" + pkDepoTransferDetay);
            //decimal iskontoyuzde = 0;
            //if (dr["iskontoyuzdetutar"].ToString() != "")
            //    iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
            //decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());
            
            //decimal Fiyat = 0;
            //if(dr["SatisFiyati"].ToString()!="")
            //  Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());

            //decimal Miktar = Convert.ToDecimal(yenimiktar);
            //decimal iskontogercektutar = Convert.ToDecimal(dr["iskontotutar"].ToString());

            //if (iskontogercektutar > 0)
            //{
            //    iskontogercekyuzde = (iskontogercektutar * 100) / (Fiyat * Miktar);
            //}
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn3, yenimiktar);
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn33, iskontogercekyuzde.ToString());

        }

        void dockPanel1Gizle()
        {
            if (dockPanel1.Width > int.Parse(dockPanel1.Tag.ToString()))// && lcKullanici.Tag.ToString()=="0")
            {
                dockPanel1.Width = int.Parse(dockPanel1.Tag.ToString());
                dockPanel1.SendToBack();
                yesilisikyeni();
            }
        }

        void dockPanel1goster()
        {
            //if (dockPanel1.Width == 860)
            //{
            //    dockPanel1.Width = int.Parse(dockPanel1.Tag.ToString());
            //    dockPanel1.SendToBack();
            //}
            //else
            {
                dockPanel1.Width = 860;
                dockPanel1.BringToFront();
            }
        }

        private void xtraTabPage5_Click(object sender, EventArgs e)
        {
            if (xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Count == 0)
                Hizlibuttonlariyukle2(false);

            dockPanel1goster();
            lcKullanici.Tag = "0";
        }

        private void repositoryItemButtonEdit1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
           // string girilen=
           // ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
        }

        private void pkSatisBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //fiş düzenle
            if (e.KeyCode == Keys.Enter)
            {

                if (pkDepoTransfer.Text == "") return;

                DataTable dt = DB.GetData("select pkSatislar,Siparis,fkKullanici From Satislar with(nolock) where pkSatislar=" + pkDepoTransfer.Text);
                if (dt.Rows.Count == 0)
                {
                    //Showmessage("Fiş Bulunamadı.", "K");
                    return;
                }
                if(dt.Rows[0]["Siparis"].ToString()=="False")
                {
                    //Showmessage("Fiş Açık Lütfen Yenileyiniz. Kullanıcı=" + dt.Rows[0]["fkKullanici"].ToString(), "K");
                    return;
                }
                FisGetir(pkDepoTransfer.Text);
            }
        }


        private void gridControl1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
        }

        void SatisDetayEkle(string barkod)
        {
            if (barkod == "") return;
            float Adet = 1;
            decimal EklenenMiktar = 1;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr == null)
                EklenenMiktar = 1;
            else
            {
                if (dr["Adet"].ToString() != "")
                    EklenenMiktar = decimal.Parse(dr["Adet"].ToString());
            }
            if (EklenenMiktar == 1)
                EklenenMiktar = HizliMiktar;
            decimal f = 0;
            decimal.TryParse(barkod, out f);
            if (dr != null && dr["pkDepoTransferDetay"].ToString() != "") return;
            //if (barkod == "" || f == 0) return;
                YeniSatisEkle();
            //başında sıfır varsa
            if (barkod.Length == 3)
                barkod = (1 * decimal.Parse(barkod)).ToString();
            //gramlı ürünler
            if (barkod.Length == 13 && barkod.Substring(0, 3) == DB.TeraziBarkoduBasi1.ToString())
            {
                EklenenMiktar = decimal.Parse(barkod.Substring(7, 5)) / 1000;
                barkod = barkod.Substring(2, 5);
                barkod = (1 * decimal.Parse(barkod)).ToString();
            }
            else if (barkod.Length == 13 && barkod.Substring(0, 3) == DB.TeraziBarkoduBasi2.ToString())
            {
                EklenenMiktar = decimal.Parse(barkod.Substring(7, 5)) / 1000;
                barkod = barkod.Substring(2, 5);
                barkod = (1 * decimal.Parse(barkod)).ToString();
            }
            else if (barkod.Length == 13 && barkod.Substring(0, 3) == DB.TeraziBarkoduBasi3.ToString())
            {
                EklenenMiktar = decimal.Parse(barkod.Substring(7, 5)) / 1000;
                barkod = barkod.Substring(2, 5);
                barkod = (1 * decimal.Parse(barkod)).ToString();
            }

            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti,SatisAdedi FROM StokKarti with(nolock) where Barcode='" + barkod + "'");

            decimal SatisAdedi = 1;
            string pkStokKarti = "0";
            if (dtStokKarti.Rows.Count == 0)
            {
                string Barcode = "";
                dtStokKarti = DB.GetData("SELECT sk.pkStokKarti,skb.SatisAdedi,sk.Barcode FROM StokKartiBarkodlar  skb with(nolock)" +
                " inner join StokKarti sk with(nolock) on sk.pkStokKarti=skb.fkStokKarti  where skb.Barkod='" + barkod + "'");
                if (dtStokKarti.Rows.Count > 0)
                {
                    SatisAdedi = decimal.Parse(dtStokKarti.Rows[0]["SatisAdedi"].ToString());
                    //EklenenMiktar = EklenenMiktar * SatisAdedi;
                    pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
                    Barcode = dtStokKarti.Rows[0]["Barcode"].ToString();
                }
                else
                {
                    frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                    StokKartiHizli.ceBarkod.EditValue = barkod;
                    StokKartiHizli.ShowDialog();

                    if (StokKartiHizli.TopMost == true)
                    {
                        dtStokKarti = DB.GetData("select pkStokKarti,SatisAdedi From StokKarti with(nolock) WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
                    }
                    else
                    {
                        yesilisikyeni();
                        StokKartiHizli.Dispose();
                        return;
                    }
                    StokKartiHizli.Dispose();
                }
            }
            else
            {
                SatisAdedi = decimal.Parse(dtStokKarti.Rows[0]["SatisAdedi"].ToString());
                pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            }
            
            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkDepoTransfer", pkDepoTransfer.Text));
            arr.Add(new SqlParameter("@fkDepolar", lueGonderenDepo.EditValue.ToString()));
            arr.Add(new SqlParameter("@fkDepolar_alici", lueHedefDepo.EditValue.ToString()));
            
            decimal eklenenmik = EklenenMiktar * SatisAdedi;
            arr.Add(new SqlParameter("@Adet", eklenenmik.ToString().Replace(",", ".")));

            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));

            string s = DB.ExecuteScalarSQL("exec hsp_DepoTransferDetay_Ekle @fkDepoTransfer,@fkDepolar,@fkDepolar_alici,@Adet,@fkStokKarti", arr);
            if (s != "Depo Transfer Eklendi.")
            {
                MessageBox.Show(s);
                return;
            }
            
            HizliMiktar = 1;

            if (Degerler.Uruneklendisescal)
                Digerislemler.UrunEkleSesCal();
        }

        void YeniDepoTransfer()
        {
            string sql = "";
            string fisno = "0";
            //bool yazdir = false;
            ArrayList list = new ArrayList();
            //string pkFirma = Satis1Firma.Tag.ToString();

            list.Add(new SqlParameter("@fkDepolar", lueGonderenDepo.EditValue.ToString()));
            list.Add(new SqlParameter("@siparis", "0"));
            list.Add(new SqlParameter("@fkKullanicilar", lueKullanicilar.EditValue.ToString()));
            list.Add(new SqlParameter("@aciklama", btnAciklamaGirisi.ToolTip));
            //list.Add(new SqlParameter("@transfer_tarihi", ));//iskontoTutar.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@bilgisayar_adi", Degerler.BilgisayarAdi));
            list.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));


            sql = "INSERT INTO DepoTransfer (fkDepolar,siparis,fkKullanicilar,aciklama,bilgisayar_adi,kayit_tarihi,aktarildi)" +
                " VALUES(@fkDepolar,@siparis,@fkKullanicilar,@aciklama,@bilgisayar_adi,getdate(),@aktarildi) SELECT IDENT_CURRENT('DepoTransfer')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            
            if (fisno.Substring(0,1)== "H")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }

            if (Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
            }

            deGuncellemeTarihi.Enabled = true;
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,0);
            string pkSatislar = "0";
            //if (AcikSatisindex == 1)
                pkSatislar = Satis1Toplam.Tag.ToString();

            DB.pkSatislar = int.Parse(pkSatislar);
            YaziciAyarlari.Tag = 1;
            YaziciAyarlari.ShowDialog();
        }


        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr == null)
                    gridView1.DeleteRow(i);
                else if (dr["pkDepoTransferDetay"].ToString() == "")
                    gridView1.DeleteRow(i);
            }
        }

        private void pkSatisBarkod_Enter(object sender, EventArgs e)
        {
            pkDepoTransfer.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            DataTable dt = DB.GetData("select pkDepoTransfer,fkDepolar from DepoTransfer with(nolock) where siparis<>1 and fkKullanicilar=" + DB.fkKullanicilar);

            int c = dt.Rows.Count;
            if (c == 0)
            {
                //formislemleri.Mesajform("Açık Satış Bulunamadı","S");
            }
            else
            {
                for (int i = 0; i < c; i++)
                {
                    string pkDepoTransfer = dt.Rows[i]["pkDepoTransfer"].ToString();
                    string fkDepolar = dt.Rows[i]["fkDepolar"].ToString();
                    //AcikSatisindex = i + 1;
                    if (i == 0)
                    {
                        Satis1Toplam.Tag = pkDepoTransfer;
                        //DataTable dtMusteri = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar with(nolock) where pkFirma=" + fkFirma);
                        //if (dtMusteri.Rows.Count > 0)
                        //{
                        //    Satis1Firma.Tag = dtMusteri.Rows[0]["pkFirma"].ToString();
                        //    Satis1Baslik.ToolTip = dtMusteri.Rows[0]["OzelKod"].ToString() + "-" + dtMusteri.Rows[0]["Firmaadi"].ToString();
                        //    Satis1Baslik.Text = Satis1Baslik.ToolTip;
                        //}
                        //else
                        //{
                        //    formislemleri.Mesajform("Müşteri Bulunamadı","K",200);
                        //    Satis1Firma.Tag = "0";
                        //    Satis1Baslik.ToolTip = "Müşteri Bulunamadı";
                        //    Satis1Baslik.Text = "Müşteri Bulunamadı";
                        //}
                    }
                   
                  
                    if (i > 2) break;
                    //yesilisikyeni();
                }

                DepoTransferGetir();

                //SatisDetayToplamGetir(Satis1Toplam.Tag.ToString());

                yesilisikyeni();
                return;
            }
            //SatisDetayGetir("0");
            yesilisikyeni();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (gridView1.DataRowCount == 0) e.Cancel = true;
        }

        void Satis1ToplamGetir(int i)
        {
            //AcikSatisindex = i;

            DepoTransferGetir();

            yesilisikyeni();

        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        void SatisDetayYenile()
        {
            //if (AcikSatisindex == 1)
            {
                SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
            }
        }

        private void repositoryItemCalcEdit3_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).OldEditValue == null)
            {
                yesilisikyeni();
                return;
            }
            string oncekimiktar = ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).OldEditValue.ToString();
            string yenimiktar =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).EditValue.ToString();
            if (yenimiktar == "0") yenimiktar = "1";

            MiktarDegistir(yenimiktar);

            int i = gridView1.FocusedRowHandle;
            SatisDetayYenile();
            gridView1.FocusedRowHandle = i;
        }

        private void cmsPopYazir_Opening(object sender, CancelEventArgs e)
        {
            if (((((System.Windows.Forms.ContextMenuStrip)(sender)).SourceControl).Controls.Owner).Name == "btnyazdir")
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = true;
                müşteriSeçToolStripMenuItem.Visible = false;
            }
            else
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = false;
                müşteriSeçToolStripMenuItem.Visible = true;
            }
        }

        private void sTOKKARTINIDÜZENLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStokKarti StokKarti = new frmStokKarti();
            DataTable dt =
            DB.GetData("SELECT pkStokKarti from StokKarti wiht(nolock) where Barcode='" + HizliBarkod + "'");
            if (dt.Rows.Count > 0)
                DB.pkStokKarti = int.Parse(dt.Rows[0][0].ToString());
            else
                DB.pkStokKarti = 0;
            StokKarti.ShowDialog();

            FisSatisAlisFiyatlarininiDegis(DB.pkStokKarti.ToString());

            Hizlibuttonlariyukle2(true);
        }

        void FisSatisAlisFiyatlarininiDegis(string pkStokKarti)
        {
            //for (int s = 0; s < gridView1.DataRowCount; s++)
            //{
            //    DataRow dr = gridView1.GetDataRow(s);
            //    string pkSatisDetay = dr["pkSatisDetay"].ToString();

                //if (lueFiyatlar.Tag.ToString() == "1")
                  //  FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
            
            int s = DB.GetData(@"select pkSatisDetay from SatisDetay sd with(nolock)
                    inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                    where pkStokKarti="+pkStokKarti+" and sk.SatisFiyati<>sd.SatisFiyati and fkSatislar=" + pkDepoTransfer.Text.ToString()).Rows.Count;

            if (s == 0) return;

            string sec =  formislemleri.MesajBox("Fiyatları Güncellemek İstiyormusunuz?","Fiyat Değişikliği Var",3,0);
            if(sec=="1")

            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Fiyatları Güncellemek İstiyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);//, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.Yes)
            {
                DB.ExecuteSQL(@"UPDATE SatisDetay SET AlisFiyati= StokKarti.AlisFiyati,SatisDetay.SatisFiyati=StokKarti.SatisFiyati,
                SatisDetay.NakitFiyat=StokKarti.SatisFiyati,SatisDetay.KdvOrani=StokKarti.KdvOrani From StokKarti 
                where SatisDetay.fkStokKarti=StokKarti.pkStokKarti and fkSatislar=" + pkDepoTransfer.Text);

                DB.ExecuteSQL(@"UPDATE SatisDetay SET SatisFiyatiKdvHaric=SatisFiyati-((SatisFiyati*KdvOrani)/(100+KdvOrani))
                where fkSatislar=" + pkDepoTransfer.Text);

                DB.ExecuteSQL("UPDATE SatisDetay SET TevkifatTutari=(((SatisFiyatiKdvHaric*KdvOrani)/100)*TevkifatOrani)/10 where fkSatislar=" + pkDepoTransfer.Text);

                //DB.ExecuteSQL("UPDATE SatisDetay SET SatisFiyati=SatisFiyatiKdvHaric+((SatisFiyatiKdvHaric*KdvOrani)/100) where fkSatislar=" + pkSatisBarkod.Text);
            }
        }

        private void simpleButton8_Click_2(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("41");//19
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            Hizlibuttonlariyukle2(false);
        }

        void Yenile()
        {
            pkDepoTransfer.Text = "0";
            timer1.Enabled = true;
            yesilisikyeni();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            frmFiyatGor FiyatGor = new frmFiyatGor();
            FiyatGor.ShowDialog();
            yesilisikyeni();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmHizliGruplar HizliGruplar = new frmHizliGruplar();
            HizliGruplar.ShowDialog();

            Hizlibuttonlariyukle2(true);
        }

        void YazdirSatisFaturasi(bool Disigner)
        {
            try
            {
                #region Yazici Sec
                string YaziciAdi = "", YaziciDosyasi = "";

                DataTable dtYazicilar =
                DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=2");

                if (dtYazicilar.Rows.Count == 1)
                {
                    YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                    YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                    short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
                }
                else if (dtYazicilar.Rows.Count > 1)
                {
                    short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,0);

                    YaziciAyarlari.ShowDialog();

                    YaziciAyarlari.Tag = 0;
                    YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

                    if (YaziciAyarlari.YaziciAdi.Tag == null)
                        YaziciAdi = "";
                    else
                        YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                    YaziciAyarlari.Dispose();
                }

                if (YaziciAdi == "")
                {
                    MessageBox.Show("Yazıcı Bulunamadı");
                    return;
                }
                #endregion


                string fisid = pkDepoTransfer.Text;
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                //DataRow[] FisDr= FisDetay.Select("iade='False'");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                ////kdv oranları
                //DataTable fiskdv = DB.GetData("select KdvOrani,SUM((SatisFiyati-iskontotutar)*Adet) as kdvtutar  from SatisDetay" +
                //" where fkSatislar= " + fisid + " group by KdvOrani");
                //fiskdv.TableName = "fiskdv";
                //ds.Tables.Add(fiskdv);
                //şirket bilgileri
                DataTable sirket = DB.GetData("select * from Sirketler with(nolock)");
                sirket.TableName = "sirket";
                ds.Tables.Add(sirket);
                //Firma bilgileri
                DataTable Musteri = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + Fis.Rows[0]["fkFirma"].ToString());
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\"+YaziciDosyasi+".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "SatisFatura";
                rapor.Report.Name = "SatisFatura";
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }


        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string girilen =
                ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (girilen == "True")
                DB.ExecuteSQL("UPDATE SatisDetay SET iade=1,Adet=Adet*-1 where pkSatisDetay=" +
                dr["pkSatisDetay"].ToString());
            else
                DB.ExecuteSQL("UPDATE SatisDetay SET iade=0,Adet=abs(Adet) where pkSatisDetay=" +
           dr["pkSatisDetay"].ToString());
            yesilisikyeni();
           // SatisTipiKaydet();
        }

        private void tSMIEksikListesi_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //string Stokadi = dr["Stokadi"].ToString();
            string fkStokKarti = dr["fkStokKarti"].ToString();

            int stok_id = int.Parse(fkStokKarti);
            frmStokKartiDepoMevcut StokKartiDepoMevcut = new frmStokKartiDepoMevcut(stok_id);
            StokKartiDepoMevcut.ShowDialog();
        
        }

        private void lueKullanicilar_EditValueChanged(object sender, EventArgs e)
        {
            //DB.fkKullanicilar = lueKullanicilar.EditValue.ToString();
            if (pkDepoTransfer.Text != "")
            {
                DB.ExecuteSQL("update Satislar set fkKullanici=" + lueKullanicilar.EditValue.ToString() +
                    " where pkSatislar=" + pkDepoTransfer.Text);
                yesilisikyeni();
            }
        }

        private void deGuncellemeTarihi_EditValueChanged(object sender, EventArgs e)
        {
            if (deGuncellemeTarihi.Tag.ToString() == "1")
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@transfer_tarihi", deGuncellemeTarihi.DateTime));
                list.Add(new SqlParameter("@pkDepoTransfer", pkDepoTransfer.Text));
                DB.ExecuteSQL("update DepoTransfer Set transfer_tarihi=@transfer_tarihi where pkDepoTransfer=@pkDepoTransfer", list);
                yesilisikyeni();
            }
        }

        private void deGuncellemeTarihi_Enter(object sender, EventArgs e)
        {
            deGuncellemeTarihi.Tag = "1";
        }

        private void deGuncellemeTarihi_Leave(object sender, EventArgs e)
        {
            deGuncellemeTarihi.Tag = "0";
        }

        private void pmenuStokHareketleri_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
            yesilisikyeni();
        }

        DevExpress.XtraGrid.GridControl targetGrid = null;
        DevExpress.XtraGrid.Views.Grid.GridView gridView = null;
        private void btnGecmisFisler_Click(object sender, EventArgs e)
        {
            if (targetGrid == null)
            {
                targetGrid = new DevExpress.XtraGrid.GridControl();
                targetGrid.ContextMenuStrip = cMenuFisGecmis;
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

            string  sql = @"select top 20 pkDepoTransfer,dt.fkDepolar,transfer_tarihi as Tarih,aciklama as Aciklama,k.adisoyadi as KullaniciAdi
            FROM DepoTransfer dt with(nolock)
            LEFT JOIN Kullanicilar k with(nolock) ON dt.fkKullanicilar = k.pkKullanicilar
            where siparis = 1 order by pkDepoTransfer desc";

            targetGrid.DataSource = DB.GetData(sql);
         
            targetGrid.BringToFront();
            targetGrid.Width = 650;
            targetGrid.Height = 300;
            targetGrid.Left = pkDepoTransfer.Left +pGecmisFisler.Left;
            targetGrid.Top = pkDepoTransfer.Top + 60;
            if (gridView.Columns.Count > 0)
            {
                gridView.Columns[1].Width = 50;
                gridView.Columns[2].Width = 100;
                gridView.Columns[3].Width = 50;
                //gridView.Columns[4].Width = 100;
                //gridView.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                //gridView.Columns[4].DisplayFormat.FormatString ="g";
                //gridView.Columns[3].DisplayFormat.FormatString = "{0:n2}";
                //gridView.Columns[3].DisplayFormat.FormatType = FormatType.Numeric;
                //gridView.Columns[0].Visible = false;
            }
            if (gridView.DataRowCount == 0)
                targetGrid.Visible = false;
            else
            {
                targetGrid.Visible = true;
                targetGrid.Focus();
            }
        }
        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
                FisGetir(dr["pkDepoTransfer"].ToString());
                targetGrid.Visible = false;
            }
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            if (gridView.FocusedRowHandle < 0) return;

            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            
            FisGetir(dr["pkDepoTransfer"].ToString());

            targetGrid.Visible = false;
        }

        private void tMenuitemOdemeAl_Click(object sender, EventArgs e)
        {
            string fkfirma = "0";
            if (gridView.FocusedRowHandle < 0) return;
            DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);
            fkfirma=dr["fkFirma"].ToString();
            targetGrid.Visible = false;

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkfirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            //Odemeler();
            //BakiyeGetirSecindex(fkfirma);
        }

        private void pGecmisFisler_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
               targetGrid.Visible = false;
        }

        private void gcSatisDetay_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void SolPanel_Click(object sender, EventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void SolPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void splitContainerControl1_Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void panelControl3_MouseClick(object sender, MouseEventArgs e)
        {
            if (targetGrid != null)
                targetGrid.Visible = false;
        }

        private void özelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xrCariHareket report = new xrCariHareket();
            report.DataSource = gcSatisDetay.DataSource;
            report.CreateDocument();
            report.ShowPreview();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            xrCariHareket report = new xrCariHareket();
            report.DataSource = gcSatisDetay.DataSource;
            report.CreateDocument();
            report.ShowDesigner();
        }

        private void yazdırToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            xrCariHareket report = new xrCariHareket();
            report.DataSource = gcSatisDetay.DataSource;
            report.CreateDocument();
            report.Print();
        }

        private void gcSatisDetay_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            //GridView View = sender as GridView;
            if (ghi.RowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(ghi.RowHandle);
            if (ghi.Column.FieldName == "Stokadi")
            {
                //frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay(fkFirma);
                //SatisUrunBazindaDetay.Tag = "3";
                //SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
                //SatisUrunBazindaDetay.ShowDialog();
                yesilisikyeni();
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            else
                MusteriSonSatislari(e.RowHandle);
        }

        void MusteriSonSatislari(int RowHandle)
        {
            DataRow dr = gridView1.GetDataRow(RowHandle);   

            string sql = "";

            sql = "select pkStokKartiDepo,DepoAdi,skd.fkDepolar,skd.MevcutAdet from StokKartiDepo skd with(nolock) left join Depolar d with(nolock)  on d.pkDepolar=skd.fkDepolar where fkStokKarti=" + dr["fkStokKarti"].ToString();

            gcMusteriSatis.DataSource = DB.GetData(sql);
        }


        private void faturaDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirSatisFaturasi(true);
        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\DepoTransfer.xml";
            gridView1.SaveLayoutToXml(Dosya);

            gridView1.OptionsBehavior.AutoPopulateColumns = false;
            gridView1.OptionsCustomization.AllowColumnMoving = false;
            gridView1.OptionsCustomization.AllowColumnResizing = false;
            gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            gridView1.OptionsCustomization.AllowRowSizing = false;
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\DepoTransfer.xml";
            File.Delete(Dosya);

            simpleButton19_Click(sender,e);

        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string fisno = pkDepoTransfer.Text;
            gcSatisDetay.ExportToXls(exedizini + "/" + fisno + "Fis.xls");
            Process.Start(exedizini + "/" + fisno + "Fis.xls");
        }

        private void gridView1_HideCustomizationForm(object sender, EventArgs e)
        {
            kaydetToolStripMenuItem1_Click(sender, e);
        }
 
        private void xtraTabControl3_Click(object sender, EventArgs e)
        {
            //if(xtraTabControl3.TabStop == false)
                dockPanel1goster();
            //xtraTabControl3.TabStop = false;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            dockPanel1goster();
        }

        private void repositoryItemCalcEdit3_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
            gridView1.CloseEditor();
        }

        private void excelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gridView1.ExportToXls(Application.StartupPath + "Satislar.Xls");
            Process.Start(Application.StartupPath + "Satislar.Xls");
        }

        private void excelToolStripMenuItem2_Click(object sender, EventArgs e)
        {

            DataTable dt =
            DB.GetData("select Firmaadi,Eposta from Firmalar  with(nolock) where pkFirma=" + Satis1Firma.Tag.ToString());
            if (dt.Rows[0]["Eposta"].ToString().Length < 5)
            {
                MessageBox.Show(dt.Rows[0]["Firmaadi"].ToString() + "\nMüşterinin E-Posta Adresini Kontrol Ediniz!");
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dt.Rows[0]["Eposta"].ToString() + " E-Posta Gönderilecek Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            gridView1.ExportToXls(Application.StartupPath + "\\Teklif.Xls");
            DB.epostagonder(dt.Rows[0]["Eposta"].ToString(), "Teklif Listesi", Application.StartupPath + "\\Teklif.Xls", "Teklif");
            frmMesajBox mesaj = new frmMesajBox(200);
            mesaj.label1.Text = "E-Posta Gönderildi.";
            mesaj.Show();
        }


        private void repositoryItemCalcEdit3_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Up)
            //{
            //    if (gridView1.FocusedRowHandle == 0)
            //        gridView1.FocusedRowHandle = gridView1.FocusedRowHandle - 1;
            //}
        }

        private void repositoryItemSpinEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            string barkod = "";
            frmStokKarti StokKartiHizli = new frmStokKarti();
            DB.pkStokKarti = 0;
            Degerler.stokkartisescal = false;
            StokKartiHizli.Barkod.EditValue = barkod;
            StokKartiHizli.ShowDialog();
            Degerler.stokkartisescal = true;

            DataTable dtStokKarti = DB.GetData("select pkStokKarti,Barcode From StokKarti with(nolock) WHERE Barcode='" + StokKartiHizli.Barkod.EditValue.ToString() + "'");

            //eğer stok kartı oluşturmadı ise 
            if (dtStokKarti.Rows.Count == 0)
            {
                yesilisikyeni();
                StokKartiHizli.Dispose();
                return;
            }
            StokKartiHizli.Dispose();

            //ürün gerçekten kaydedildi ise okutarak satış yapsın 24.08.2014
            //if (gridView1.DataRowCount == 0)
            //    YeniSatisEkle();

            //urunekle(dtStokKarti.Rows[0]["Barcode"].ToString());
            
            yesilisikyeni();
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
           KaydetYazdir(true);
        }

        private void vYazdir(Int16 dizayner)
        {
            string YaziciAdi = "", YaziciDosyasi = "";

            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=2");

            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,0);

                YaziciAyarlari.ShowDialog();

                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }

            if (YaziciAdi == "")
                MessageBox.Show("Yazıcı Bulunamadı");
            else
                FisYazdir(dizayner, pkDepoTransfer.Text, YaziciDosyasi, YaziciAdi);
        }

        void FisYazdir(Int16 Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli from KasaHareket with(nolock) where fkSatislar=" + fisid);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }

                xrCariHareket rapor = new xrCariHareket();
                //if (yazdirmaadedi>1)
                //  rapor.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);

                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = SatisFisTipi;
                rapor.Report.Name = SatisFisTipi;
                //ön izleme
                if (Disigner == 3)
                {
                    rapor.ShowPreview();
                }
                //dizayn
                else if (Disigner == 2)
                    rapor.ShowDesigner();
                else if (Disigner == 1)
                {
                    if (yazdirmaadedi < 1) yazdirmaadedi = 1;

                    
                        //rapor.ShowPreview();
                        for (int i = 0; i < yazdirmaadedi; i++)
                            rapor.Print(YaziciAdi);
                    
                    //DB.ExecuteSQL("update satislar set Yazdir=1 where pkSatislar=" + lueSatisTipi.EditValue.ToString());
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }


        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnyazdir_Click(sender,e);
        }

        private void gridView6_DoubleClick(object sender, EventArgs e)
        {
            //if (gridView6.FocusedRowHandle < 0) return;
            //DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);
            //frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            ////FisNoBilgisi.TopMost = true;
            //FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            //FisNoBilgisi.ShowDialog();
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter && !gridView1.IsEditing && gridView1.FocusedColumn.FieldName == "iskyuzdesanal")
            //{
            //    if (gridView1.FocusedRowHandle < 0) return;
            //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //    frmiskonto iskonto = new frmiskonto();
            //    iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
            //    iskonto.ShowDialog();
            //}
        }

        void SatisDetayGetir_Yeni(string pkSatislar)
        {
            pkDepoTransfer.Text = pkSatislar;

            gcSatisDetay.DataSource = DB.GetData("hsp_DepoTransferDetay " + pkSatislar + ",0");


            gridView1.AddNewRow();
        }

        //void SatisDetayToplamGetir1(string pkSatislar)
        //{
        //    pkDepoTransfer.Text = pkSatislar;

        //    gcSatisDetay.DataSource = DB.GetData("exec sp_SatisDetay " + pkSatislar + ",0");
        //}

        void yesilisikyeni()
        {
            {
                SatisDetayGetir_Yeni(Satis1Toplam.Tag.ToString());
                //fkFirma = Satis1Firma.Tag.ToString();
            }


            if (targetGrid != null)
                targetGrid.Visible = false;

            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            //gridView1.CloseEditor();

            SendKeys.Send("{ENTER}");
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void repositoryItemComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void repositoryItemSpinEdit1_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
            gridView1.CloseEditor();
        }

        void disp(object sender, EventArgs e)
        {
            MessageBox.Show("It works!");
        }

        private void kaydetToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gcSatisDetay, "A4");
        }


        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            //iskonto tutar
            string iskontotutar =
            ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();

            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkSatisDetay = dr["pkSatisDetay"].ToString();

            string satisfiyati = "0";
           // if (dr["KdvDahil"].ToString() == "True")
            satisfiyati = dr["SatisFiyati"].ToString();
            //else
              //  satisfiyati = dr["SatisFiyatiKdvHaric"].ToString();
            decimal _satisfiyati = 0;
            decimal.TryParse(satisfiyati, out _satisfiyati);
            decimal isyuzde = 0, istut = 0;
            decimal.TryParse(iskontotutar, out istut);
            isyuzde = (istut * 100) / _satisfiyati;
            //isyuzde=decimal.Round(isyuzde, 6);
            int sonuc =
            DB.ExecuteSQL_Sonuc_Sifir("update SatisDetay set iskontoyuzdetutar=" + isyuzde.ToString().Replace(",", ".") +
                ",iskontotutar="+iskontotutar.Replace(",",".")+
                " where pkSatisDetay=" + pkSatisDetay);

            if(sonuc==-1)
                MessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz");

            //if (repositoryItemCalcEdit1.AccessibleDescription == "")
            //{
                int i = gridView1.FocusedRowHandle;

                SatisDetayYenile();

                gridView1.FocusedRowHandle = i;
            //}
            //else if (repositoryItemCalcEdit1.AccessibleDescription == "Enter")
              //  yesilisikyeni();
        }
        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string iskontotutar =
                  ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();

                if (gridView1.FocusedRowHandle < 0)
                {
                    yesilisikyeni();
                    return;
                }
                //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                //string pkSatisDetay = dr["pkSatisDetay"].ToString();
                //repositoryItemCalcEdit1.AccessibleDescription = e.KeyCode.ToString();
                yesilisikyeni();
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                int i = gridView1.FocusedRowHandle;

                //SatisDetayToplamGetir(pkDepoTransfer.Text);
                
                gridView1.FocusedRowHandle = i;
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            formislemleri.EtiketBas(dr["fkStokKarti"].ToString(),1);
            /*
            if (gridView1.FocusedRowHandle < 0) return;
            string pkEtiketBas = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Fiş No " + pkSatisBarkod.Text + "',0) SELECT IDENT_CURRENT('EtiketBas')");
            DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["fkStokKarti"].ToString() + ",1,0,getdate())");
            frmEtiketBas EtiketBas = new frmEtiketBas();
            EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
            EtiketBas.ShowDialog();
              */
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            BarkodYazdir(false);
        }

        void BarkodYazdir(bool dizayn)
        {
             if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti = dr["fkStokKarti"].ToString();

            //DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            xrCariHareket Barkod = new xrCariHareket();

            //for (int i = 0; i < 1; i++)
            //{
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                string RaporDosyasi = "";
                //xrBarkod Barkod = new xrBarkod();
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"select pkStokKarti as fkStokKarti,
                1 as Adet,
                case when sk.KutuFiyat=0 then
                sfn.SatisFiyatiKdvli
                else (sfn.SatisFiyatiKdvli/sk.KutuFiyat) end  koliicitanefiyati,
                sk.Stokadi,
                Barcode,
                UreticiKodu,
                sfn.SatisFiyatiKdvli as SatisFiyati,
                sfk.SatisFiyatiKdvli as SatisFiyati2,
                sfk3.SatisFiyatiKdvli as SatisFiyati3,
                m.Marka,bg.Aciklama as Beden,
                r.Aciklama as Renk,'' as TedarikciAdi,
                0 as GizliFiyat,'' as EtiketAciklama 
                
                from StokKarti sk with(nolock)
                left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka
                left join BedenGrupKodu bg with(nolock) on bg.pkBedenGrupKodu=sk.fkBedenGrupKodu
                left join RenkGrupKodu r with(nolock) on r.pkRenkGrupKodu=sk.fkRenkGrupKodu
                
                left join (select sf.* from SatisFiyatlari sf with(nolock) 
                left join SatisFiyatlariBaslik sfb with(nolock) on sfb.pkSatisFiyatlariBaslik=sf.fkSatisFiyatlariBaslik
                where sfb.Tur=1) sfn on sfn.fkStokKarti=sk.pkStokKarti
                
                left join (select sf.* from SatisFiyatlari sf with(nolock) 
                left join SatisFiyatlariBaslik sfb with(nolock) on sfb.pkSatisFiyatlariBaslik=sf.fkSatisFiyatlariBaslik
                where sfb.Tur=2) sfk on sfk.fkStokKarti=sk.pkStokKarti
                
                left join (select sf.* from SatisFiyatlari sf with(nolock) 
                left join SatisFiyatlariBaslik sfb with(nolock) on sfb.pkSatisFiyatlariBaslik=sf.fkSatisFiyatlariBaslik
                where sfb.Tur=3) sfk3 on sfk3.fkStokKarti=sk.pkStokKarti

                 where pkStokKarti=" + fkStokKarti);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");// + fisid);
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                Barkod.DataSource = ds;
                DataTable dt = DB.GetData("SELECT * FROM EtiketSablonlari  with(nolock) where Varsayilan=1");
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Varsayılan Seçilmemiş");
                    return;
                }
                try
                {
                    RaporDosyasi = exedizini + "\\Raporlar\\" + dt.Rows[0]["DosyaYolu"].ToString() + ".repx";
                    //rapor.DataSource = gCPerHareketleri.DataSource;
                    //rapor.CreateDocument();
                    Barkod.Name = "Barkod";
                    Barkod.DisplayName = "Barkod";
                    Barkod.LoadLayout(RaporDosyasi);
                    //Barkod.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);
                    if (dizayn)
                        Barkod.ShowDesigner();
                    else
                    {
                        if (dt.Rows[0]["YaziciAdi"].ToString() == "")
                            MessageBox.Show("Yazıcı Adı Seçiniz");
                        else
                            Barkod.Print(dt.Rows[0]["YaziciAdi"].ToString());
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Yazdırırken Hata Oluştur:" + exp.Message);
                }
            //}
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkedit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        protected void DrawCheckBox(Graphics g, Rectangle r, bool Checked)
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;
            info = chkedit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = chkedit.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = Checked;

            info.Bounds = r;
            info.PaintAppearance.ForeColor = Color.Black;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti = dr["fkStokKarti"].ToString();

            frmStokDepoDevir StokDevir = new frmStokDepoDevir();
            StokDevir.fkStokKarti.Tag = fkStokKarti;
            StokDevir.ShowDialog();
        }

        private void repositoryItemLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.ExecuteSQL("update DepoTransferDetay set fkDepolar=" + value + " where pkDepoTransferDetay=" + dr["pkDepoTransferDetay"].ToString());
        }

        private void lbSatisTarihi_Click(object sender, EventArgs e)
        {
            deGuncellemeTarihi.DateTime = DateTime.Now;
            DB.ExecuteSQL("update Satislar set guncellemetarihi='"+deGuncellemeTarihi.DateTime.ToString("yyyy-MM-dd HH:mm:ss")+
                "' where pkSatislar=" + pkDepoTransfer.Text);
        }

        private void sbYenile_Click(object sender, EventArgs e)
        {
            Yenile();
        }


        private void repositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            int s = DB.ExecuteSQL("update DepoTransferDetay set fkDepolar_alici=" + value + " where pkDepoTransferDetay=" + dr["pkDepoTransferDetay"].ToString());
            if (s == 0)
                formislemleri.Mesajform("Hata","K",200);
        }

        private void lueGonderenDepo_EditValueChanged(object sender, EventArgs e)
        {
            if (pkDepoTransfer.Text == "") return;

            if (lueGonderenDepo.EditValue != null)
            {
                //Satis1Toplam.Text = lueGonderenDepo.Text;

                DB.ExecuteSQL("update DepoTransferDetay set fkDepolar=" +
                lueGonderenDepo.EditValue.ToString() + " where fkDepoTransfer=" + pkDepoTransfer.Text);

                yesilisikyeni();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void lueHedefDepo_EditValueChanged(object sender, EventArgs e)
        {
            if (pkDepoTransfer.Text == "") return;

            DB.ExecuteSQL("update DepoTransferDetay set fkDepolar_alici="+
                lueHedefDepo.EditValue.ToString() + " where fkDepoTransfer=" + pkDepoTransfer.Text);

            yesilisikyeni();
        }

        private void btntransrapor_Click(object sender, EventArgs e)
        {
            frmTransferRaporlari TransferRaporlari = new frmTransferRaporlari();
            TransferRaporlari.Show();
        }

        private void depoMevcutlarıİleEşitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = @"update DepoTransferDetay set  transfer_miktar=skd.MevcutAdet  from StokKartiDepo skd
where skd.fkStokKarti=DepoTransferDetay.fkStokKarti
and DepoTransferDetay.fkDepoTransfer=" + pkDepoTransfer.Text;

            DB.ExecuteSQL(sql);
            yesilisikyeni();
        }

        private void xmlDosyasıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData(@"select sk.pkStokKarti,sk.Barcode,sk.StokKod,sk.Stoktipi,
sk.Stokadi,sk.KdvOrani,
sk.SatisFiyati,m.Marka,dtd.transfer_miktar
 from DepoTransferDetay dtd with(nolock)
left join stokkarti sk with(nolock) on sk.pkStokKarti=dtd.fkStokKarti
left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka
where dtd.fkDepoTransfer="+pkDepoTransfer.Text);
//            @"<products>
//    <product>
//        <barcode>9999999999</barcode>
//        <code>A-123</code>
//        <name><![CDATA[ Ürün Adı ]]></name>
//        <description><![CDATA[ Ürün Adı ]]></description>
//        <category>Kategori 1 > Kategori 2 > Kategori 3</category>
//        <tax>18</tax>
//        <price>19.9999</price>
//        <brand>19.9999</brand>
//        <quantity>19.9999</quantity>
//        <image>https://www.piyersoft.com/wp-content/themes/piyersoft/images/logo.png</image>
//    </product>
//</products>";
            
                
                XmlTextWriter yaz = new XmlTextWriter("weburunler.xml", System.Text.UTF8Encoding.UTF8);
                //Daha önce bu isimle oluşturulan bir XML dosyası var ise, eski dosya silinir.
                yaz.Formatting = Formatting.Indented;
            // Dosya yapısını hiyerarşik olarak oluşturarak okunabilirliği arttırır.
            try
            {
                yaz.WriteStartDocument(); //Xml dökümanına ait declaration satırını oluşturur. Kısaca yazmaya başlar.
                yaz.WriteStartElement("products");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //DataRow dr = gridView1.GetDataRow(i);

                yaz.WriteStartElement("product");
                    //okul ve ogretmen etiketleri oluşturuldu.
                yaz.WriteAttributeString("barcode", dt.Rows[i]["Barcode"].ToString());
                yaz.WriteElementString("code", dt.Rows[i]["StokKod"].ToString());//StokKod pkStokKarti
                yaz.WriteElementString("name", dt.Rows[i]["Stokadi"].ToString());
                yaz.WriteElementString("description", "Birimi=" + dt.Rows[i]["Stoktipi"].ToString());
                yaz.WriteElementString("tax", dt.Rows[i]["KdvOrani"].ToString());//vergi
                string at = dt.Rows[i]["SatisFiyati"].ToString().Replace(",", ".");
                yaz.WriteElementString("price", at);//fiyat 
                yaz.WriteElementString("brand", dt.Rows[i]["Marka"].ToString());//marka
                yaz.WriteElementString("quantity", dt.Rows[i]["transfer_miktar"].ToString());//miktar 
                          //İçerik isim-değer çiftleri şeklinde ogretmen etiketinin içerisine element türünde eklendi.
                yaz.WriteEndElement();

            
                }
                    yaz.WriteEndElement();
                    //okul ve ogretmen etiketleri sonlandırıldı.
                    yaz.Close();
                    //XML akışı sonlandırıldı.
                    MessageBox.Show("XML dosyası oluşturuldu ve veriler eklendi.");
                }
                catch (Exception ex)
                {
                 MessageBox.Show("Hata Oluştu:" + ex.Message);
                }

            Process.Start(Application.StartupPath);
        }
    }
}
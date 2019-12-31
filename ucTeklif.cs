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
//using DevExpress.XtraReports.UI;
using GPTS.Include.Data;
using DevExpress.XtraTab;

namespace GPTS
{
    public partial class ucTeklif : DevExpress.XtraEditors.XtraUserControl
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis = "";//AccessibleDescription
        int AcikSatisindex = 1;//hangi satış açık
        decimal HizliMiktar = 1;
        bool ilkyukleme = false, digersorunu = false;
        //XtraReport rapor = null;
        public ucTeklif()
        {
            InitializeComponent();
            DB.PkFirma = 1;
        }
        private void ucAnaEkran_Load(object sender, EventArgs e)
        { 
            ilkyukleme = true;
            Fiyatlarigetir();
            HizliSatisTablariolustur();
            Yetkiler();
            lueSatisTipi.Properties.DataSource = DB.GetData(@"SELECT pkSatisDurumu, Durumu, Aktif, SiraNo FROM  SatisDurumu WHERE Aktif = 1 ORDER BY SiraNo");
            lueSatisTipi.EditValue = 1;
            FisListesi();
            GeciciMusteriDefault();
            timer1.Enabled = true;
            cbOdemeSekli.SelectedIndex = 0;
            lueFiyatlar.EditValue = 1;
            ilkyukleme = false;
            KullaniciListesi();
            //XtraReport rapor = new XtraReport();
        }
        void KullaniciListesi()
        {
            lueKullanicilar.Properties.DataSource = DB.GetData("select pkKullanicilar,adisoyadi,KullaniciAdi from Kullanicilar");
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }
        void Showmessage(string lmesaj, string renk)
        {
            frmMesaj mesaj = new frmMesaj();
            mesaj.label1.Text = lmesaj;
            if (renk == "K")
                mesaj.label1.BackColor = System.Drawing.Color.Red;
            else if (renk == "B")
                mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
            else
                mesaj.label1.BackColor = System.Drawing.Color.Blue;
            mesaj.Show();
        }
        void Yetkiler()
        {
            string sql = @"SELECT   YetkiAlanlari.Yetki, YetkiAlanlari.Sayi, Parametreler.Aciklama10, YetkiAlanlari.BalonGoster
FROM  YetkiAlanlari INNER JOIN Parametreler ON YetkiAlanlari.fkParametreler = Parametreler.pkParametreler
WHERE  Parametreler.fkModul = 1  and YetkiAlanlari.fkKullanicilar =" + DB.fkKullanicilar;
            DataTable dtYetkiler = DB.GetData(sql);
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
               // if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "ParaPanel")
                 //   ParaPanel.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);

                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
                    gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
                    xtraTabControl2.Visible = true;
            }
        }
        void Fiyatlarigetir()
        {
            lueFiyatlar.Properties.DataSource = DB.GetData("select * from SatisFiyatlariBaslik where Aktif=1 order by pkSatisFiyatlariBaslik");
            if (!ilkyukleme)
                lueFiyatlar.EditValue = 1;
        }
        void HizliSatisTablariolustur()
        {
            xtraTabControl3.TabPages.Clear();
            DataTable dt = DB.GetData("select * from HizliGruplar where Aktif=1 order by SiraNo");
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
        void Hizlibuttonlariyukle()
        {
            //temizle
            xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();
            int to = 0;
            int lef = 0;
            DataTable dtbutton = DB.GetData(@"select pkHizliStokSatis,pkStokKarti,Barcode,HizliSatisAdi,Stokadi,sf.SatisFiyatiKdvli as SatisFiyati from HizliStokSatis h
            left join (select pkStokKarti,Stokadi,fkStokGrup,HizliSatisAdi,Barcode,HizliSiraNo 
            from StokKarti) sk on sk.pkStokKarti=h.fkStokKarti 
            left join SatisFiyatlari sf on sf.fkStokKarti=h.fkStokKarti  and sf.fkSatisFiyatlariBaslik=1
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
        void SatisTemizle()
        {
            gcOncekiSatisFiyatlari.DataSource = null;
            cbOdemeSekli.SelectedIndex = 0;
            TaksitOdemeTarihi.Visible = false;
            btnKampanya.Visible = false;
            lueKKarti.Visible = false;
            temizle(AcikSatisindex);
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Satış İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle

            string sonuc = "", pkSatislar = "0";
            sonuc = DB.ExecuteScalarSQL("EXEC spSatisSil " + pkSatislar + ",1");
            if (sonuc != "Satis Silindi.")
                MessageBox.Show(sonuc);

            if (AcikSatisindex == 1)
            {
                lcBakiye1.Text = "0,00";
                lcBakiye1.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 2)
            {
                lcBakiye2.Text = "0,00";
                lcBakiye2.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 3)
            {
                lcBakiye3.Text = "0,00";
                lcBakiye3.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 4)
            {
                lcBakiye4.Text = "0,00";
                lcBakiye4.BackColor = System.Drawing.Color.Transparent;
            }
            SatisTemizle();
            yesilisikyeni();
            lueSatisTipi.EditValue = 2;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("DELETE FROM SatisDetay WHERE pkSatisDetay=" + dr["pkSatisDetay"].ToString());
            //DB.ExecuteSQL("update StokKarti set Mevcut=Mevcut+" + dr["Adet"].ToString().Replace(",", ".") + "  where pkStokKarti=" + dr["fkStokKarti"].ToString());
            gridView1.DeleteSelectedRows();
            if (gridView1.DataRowCount == 0)
            {
                DB.ExecuteSQL("DELETE FROM Satislar WHERE pkSatislar=" + pkSatisBarkod.Text);
                pkSatisBarkod.Text = "0";
            }
            yesilisikyeni();
        }
        void YeniSatisEkle()
        {
                YeniSatis();
        }
        void iskontoyagit_ctrli()
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                if (gridView1.DataRowCount > 0)
                    ceiskontoyuzde.Focus();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmiskonto iskonto = new frmiskonto();
            iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
            iskonto.ceBirimFiyati.Value = decimal.Parse(dr["SatisFiyati"].ToString());
            iskonto.seMiktar.Value = decimal.Parse(dr["Adet"].ToString());
            iskonto.ShowDialog();
            yesilisikyeni();
            return;
        }
        void iadebasildi()
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr["iade"].ToString() == "True")
            {
                DB.ExecuteSQL("update SatisDetay Set iade=0,Adet=abs(Adet) where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
                gridView1.SetFocusedRowCellValue("iade", "False");
            }
            else
            {
                DB.ExecuteSQL("update SatisDetay Set iade=1,Adet=Adet*-1 where pkSatisDetay=" + dr["pkSatisDetay"].ToString());
                gridView1.SetFocusedRowCellValue("iade", "True");
            }
        }
        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {

            //ctrl + i iskonto
            if (e.Control && e.KeyValue == 222)
            {
                iadebasildi();
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
                return;
            }
            else if (e.KeyCode == Keys.Enter)
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
        }
        void urunaraekle()
        {
            //YeniSatisEkle();
            frmStokAra StokAra = new frmStokAra();
            StokAra.Tag = "0";
            StokAra.ShowDialog();
            if (StokAra.TopMost == false)
            {
                //StokAra.gridView1.FilterDisplayText("Starts with([Sec], True)";
                for (int i = 0; i < StokAra.gridView1.DataRowCount; i++)
                {
                    DataRow dr = StokAra.gridView1.GetDataRow(i);
                    if (dr["Sec"].ToString() == "True")
                        SatisDetayEkle(dr["Barcode"].ToString());
                    //SatisDetayEkle(StokAra.pkurunid.Text);
                }
            }
            //SatisDetayEkle(StokAra.pkurunid.Text);
            StokAra.Dispose();
            yesilisikyeni();
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle != -2147483647)
                gridView1.AddNewRow();
            urunaraekle();
        }
        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.FocusedRowHandle != -2147483647)
                gridView1.AddNewRow();
            urunaraekle();
        }
        private void urunekle(string barkod)
        {
            if (barkod == "") return;
            string ilktarih = "";
            string sontarih = "";
            string fkUrunlerNoPromosyon = "";
            int DususAdet = 1;
            DataTable dturunler = DB.GetData("select pkStokKarti From StokKarti WHERE Barcode='" + barkod + "'");
            if (dturunler.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                StokKartiHizli.ceBarkod.EditValue = barkod;
                StokKartiHizli.ShowDialog();
                if (StokKartiHizli.TopMost == true)
                {
                    dturunler = DB.GetData("select * From StokKarti WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
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
        void yesilisikyeni()
        {
            lcBakiye1.Visible = false;
            lcBakiye2.Visible = false;
            lcBakiye3.Visible = false;
            lcBakiye4.Visible = false;
            string fkFirma = "0";
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();
            SendKeys.Send("{ENTER}");

        }
        private bool SatisVar()
        {

            if (gridView1.DataRowCount == 0)
            {
                Showmessage("Önce Satış Yapınız!", "K");
                return false;
            }
            return true;
        }
      
        void satiskaydet(bool yazdir, bool odemekaydedildi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.EditValue.ToString()));
            if (FaturaTarihi.EditValue == null)
                list.Add(new SqlParameter("@FaturaTarihi", DBNull.Value));
            else
                list.Add(new SqlParameter("@FaturaTarihi", FaturaTarihi.DateTime));

            string sql = "";
            if (AcikSatisindex == 4)
                sql = @"UPDATE Satislar SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,
fkSatisDurumu=@fkSatisDurumu,OdemeSekli=@OdemeSekli,FaturaTarihi=@FaturaTarihi,SonislemTarihi=getdate() where pkSatislar=@pkSatislar";
            else
            {
                if(deGuncellemeTarihi.EditValue==null)
                sql = @"UPDATE Satislar SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,
fkSatisDurumu=@fkSatisDurumu,OdemeSekli=@OdemeSekli,GuncellemeTarihi=getdate(),FaturaTarihi=@FaturaTarihi,
SonislemTarihi=getdate() where pkSatislar=@pkSatislar";
                else
                {
                    list.Add(new SqlParameter("@GuncellemeTarihi", deGuncellemeTarihi.DateTime));
                    sql = @"UPDATE Satislar SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,
fkSatisDurumu=@fkSatisDurumu,OdemeSekli=@OdemeSekli,GuncellemeTarihi=@GuncellemeTarihi,FaturaTarihi=@FaturaTarihi,
SonislemTarihi=getdate() where pkSatislar=@pkSatislar";
                }
            }
            string sonuc = DB.ExecuteSQL(sql, list);
            if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            {
                Showmessage("Hata Oluştur: " + sonuc, "K");
                return;
            }

            #region Bonus Ekle
            //satıştan bonus eklendi
            if (lueSatisTipi.EditValue.ToString() == "2" || lueSatisTipi.EditValue.ToString() == "9")
            {
                DataTable dtSirket = DB.GetData("select top 1 isnull(BonusYuzde,0) as BonusYuzde,isnull(BonusTur,0) as BonusTur from Sirketler");
                string BonusYuzde = dtSirket.Rows[0]["BonusYuzde"].ToString();
                string BonusTur = dtSirket.Rows[0]["BonusTur"].ToString();

                if (BonusYuzde != "0")
                {
                    string sqlbonus = "";
                    if (BonusTur == "0") //Kardan bonus ver
                        sqlbonus = @"update Firmalar set Bonus=isnull(Bonus,0)+kartablo.kar  from (select s.fkFirma,(@BonusYuzde * sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar-sd.AlisFiyati)))/100 as kar from Satislar s
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar 
where s.pkSatislar=@pkSatislar
group by s.fkFirma) kartablo
where Firmalar.pkFirma=kartablo.fkFirma";
                    if (BonusTur == "1") //Satistan bonus ver
                        sqlbonus = @"update Firmalar set Bonus=isnull(Bonus,0)+kartablo.kar  from (select s.fkFirma,(@BonusYuzde * sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)))/100 as kar from Satislar s
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar 
where s.pkSatislar=@pkSatislar
group by s.fkFirma) kartablo
where Firmalar.pkFirma=kartablo.fkFirma";
                    sqlbonus = sqlbonus.Replace("@BonusYuzde", BonusYuzde);
                    sqlbonus = sqlbonus.Replace("@pkSatislar", pkSatisBarkod.Text);

                    DB.ExecuteSQL(sqlbonus);
                    //satışlara kaydet
                    if (BonusTur == "0") //Kardan bonus ver
                        sqlbonus = @"update Satislar set BonusTutar=isnull(Bonus,0)+kartablo.kar  from (
select (5 * sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar-sd.AlisFiyati)))/100 as kar from Satislar s
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar 
where s.pkSatislar=@pkSatislar
group by s.fkFirma) kartablo
where Satislar.pkSatislar=@pkSatislar";
                    if (BonusTur == "1") //Satistan bonus ver
                        sqlbonus = @"update Satislar set BonusTutar=isnull(Bonus,0)+kartablo.kar  from (select s.fkFirma,(@BonusYuzde * sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)))/100 as kar from Satislar s
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar 
where s.pkSatislar=@pkSatislar
group by s.fkFirma) kartablo
where Satislar.pkSatislar=@pkSatislar";
                    sqlbonus = sqlbonus.Replace("@BonusYuzde", BonusYuzde);
                    sqlbonus = sqlbonus.Replace("@pkSatislar", pkSatisBarkod.Text);

                    DB.ExecuteSQL(sqlbonus);
                }
            }
            #endregion

            //Alacak pkFirma olmadığı için OdemeKaydet procedure içine alındı 
            OdemeKaydet(odemekaydedildi);
            SatisTemizle();
            FisListesi();
        }
        void OdemeKaydet(bool odemekaydedildi)
        {
            string pkSatis = "0", pkFirma = "0";
          
            if (odemekaydedildi == false)//ödeme alınmadı ise ctrl+d değilse
            {
                //DB.ExecuteSQL("UPDATE Satislar SET AcikHesap=0,fkFirma="+pkFirma+" where pkSatislar=" + pkSatis);
                //DB.ExecuteSQL("UPDATE Firmalar SET Alacak=Alacak+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
                if (cbOdemeSekli.SelectedIndex == 0)
                {
                    //Nakit
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkFirma", pkFirma));
                    if (aratoplam.Value < 0)
                    {
                        list.Add(new SqlParameter("@Alacak", aratoplam.Value.ToString().Replace(",", ".").Replace("-", "")));
                        list.Add(new SqlParameter("@Borc", "0"));
                    }
                    else
                    {
                        list.Add(new SqlParameter("@Borc", aratoplam.Value.ToString().Replace(",", ".").Replace("-", "")));
                        list.Add(new SqlParameter("@Alacak", "0"));
                    }
                    list.Add(new SqlParameter("@Aciklama", "Nakit Ödeme"));
                    list.Add(new SqlParameter("@fkSatislar", pkSatis));

                    if (deGuncellemeTarihi.EditValue == null)
                        DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkKasalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
    " values(0,1,@fkFirma,getdate(),4,1,@Borc,@Alacak,1,@Aciklama,'Nakit',@fkSatislar)", list);
                    else
                    {
                        list.Add(new SqlParameter("@Tarihi", deGuncellemeTarihi.DateTime));
                        DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkKasalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
    " values(0,1,@fkFirma,@Tarihi,4,1,@Borc,@Alacak,1,@Aciklama,'Nakit',@fkSatislar)", list);
                    }
                   

                   // DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkKasalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
                    //    " values(0,1,@fkFirma,@FaturaTarihi,4,1,@Borc,@Alacak,1,@Aciklama,'Nakit',@fkSatislar)", list);
                    //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
                    //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
                    MevcutGuncelle();
                    return;
                }
                //kredi kartı ile ödeme yaptıysa
                else if (cbOdemeSekli.SelectedIndex == 1 && lueKKarti.EditValue == null)
                {
                    MessageBox.Show("Kredi Kartı Seçiniz!");
                    lueKKarti.Focus();
                    return;
                }
                else if (cbOdemeSekli.SelectedIndex == 1)
                {
                    ArrayList list2 = new ArrayList();
                    list2.Add(new SqlParameter("@fkFirma", pkFirma));
                    if (aratoplam.Value < 0)
                    {
                        list2.Add(new SqlParameter("@Borc", "0"));
                        list2.Add(new SqlParameter("@Alacak", aratoplam.Value.ToString().Replace(",", ".").Replace("-", "")));
                    }
                    else
                    {
                        list2.Add(new SqlParameter("@Borc", aratoplam.Value.ToString().Replace(",", ".").Replace("-", "")));
                        list2.Add(new SqlParameter("@Alacak", "0"));
                    }
                    list2.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue));
                    list2.Add(new SqlParameter("@Aciklama", "K.Kartı Ödemesi"));
                    list2.Add(new SqlParameter("@fkSatislar", pkSatis));
                    DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
                        " values(@fkBankalar,@fkFirma,getdate(),8,1,@Borc,0,1,@Aciklama,'Kredi Kartı',@fkSatislar)", list2);
                    //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
                    //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
                }
                //açık hesap
                else if (cbOdemeSekli.SelectedIndex == 2)
                {
                    ArrayList list4 = new ArrayList();
                    list4.Add(new SqlParameter("@fkSatislar", pkSatis));
                    list4.Add(new SqlParameter("@AcikHesap", aratoplam.Value.ToString().Replace(",", ".")));
                    DB.ExecuteSQL("UPDATE Satislar SET AcikHesap=@AcikHesap where pkSatislar=@fkSatislar", list4);

                    if (TaksitOdemeTarihi.EditValue != null)
                    {
                        ArrayList list3 = new ArrayList();
                        list3.Add(new SqlParameter("@fkFirma", pkFirma));
                        list3.Add(new SqlParameter("@Tarih", TaksitOdemeTarihi.DateTime));
                        list3.Add(new SqlParameter("@Odenecek", aratoplam.Value.ToString().Replace(",", ".")));
                        list3.Add(new SqlParameter("@fkSatislar", pkSatis));
                        DB.ExecuteSQL(@"INSERT INTO dbo.Taksitler (fkFirma,Tarih,Odenecek,Odenen,SiraNo,Aciklama,OdemeSekli,fkSatislar)
                  VALUES(@fkFirma,@Tarih,@Odenecek,0,1,'Açık Hesap','Açık Hesap',@fkSatislar)", list3);
                    }
                }

                //MevcutGuncelle
                MevcutGuncelle();
            }
        }
        void MevcutGuncelle()
        {
            for (int s = 0; s < gridView1.DataRowCount; s++)
            {
                DataRow dr = gridView1.GetDataRow(s);
                if (lueSatisTipi.EditValue.ToString() == "2")//satış
                    DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=Mevcut-" + dr["Adet"].ToString() + " where pkStokKarti=" +
                        dr["fkStokKarti"].ToString());
                else if (lueSatisTipi.EditValue.ToString() == "9")//iade
                    DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=Mevcut+" + dr["Adet"].ToString().Replace("-", "") + " where pkStokKarti=" +
    dr["fkStokKarti"].ToString());
                //DB.ExecuteSQL("UPDATE AlisDetay SET KalanAdet=KalanAdet-"+);
            }
        }
        void GeciciMusteriDefault()
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.Lütfen Yetkili Firmayı Arayınız");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = dtMusteriler.Rows[0]["OzelKod"].ToString() + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
            }
        }
        void temizle(int aciksatisno)
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = dtMusteriler.Rows[0]["OzelKod"].ToString() + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
            }
            btnAciklamaGirisi.ToolTip = "";
            //lueSatisTipi.EditValue = 2;
            ceiskontoTutar.EditValue = null;
            ceiskontoyuzde.EditValue = null;
            ceiskontoyuzde.Properties.NullText = "";
            gridControl1.DataSource = null;
            ceiskontoyuzde.Value = 0;
            ceiskontoTutar.Value = 0;
            lueFiyatlar.EditValue = 1;
            cbOdemeSekli.SelectedIndex = 0;
        }
        void satiskaydet(bool timer, bool yazdir, bool odemekaydedildi)
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesaj mesaj = new frmMesaj();
                mesaj.label1.Text = "Önce Satış Yapınız";
                mesaj.Show();
                return;
            }
            satiskaydet(yazdir, odemekaydedildi);
            FisListesi();
            // temizle(AcikSatisindex);
            yesilisikyeni();
        }
        public bool bir_nolumusteriolamaz()
        {
            return true;
        }
        void pro_kaydetyazdir(string btn_kaydet_yazdir)
        {
            string fkFirma = "1";
            
            if (cbOdemeSekli.Text == "Açık Hesap" && fkFirma == "1")
            {
                MessageBox.Show("1 Nolu Müşteri'ye Açık Hesap Yapılamaz.");
                cbOdemeSekli.Focus();
                return;
            }
            //if (cbOdemeSekli.Text == "Açık Hesap")
            //{
            //    DialogResult secim;
            //    secim = DevExpress.XtraEditors.XtraMessageBox.Show("Ödeme Şeklini Açık Hesap Seçtiniz Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //    if (secim == DialogResult.No)
            //    {
            //        yesilisikyeni();
            //        return;
            //    }
            //}
            if (bir_nolumusteriolamaz() == false) return;
            //if (OnceSatisYapiniz() == false) return;

            //if (bir_acikhesapolamaz() == false) return;
            if (btn_kaydet_yazdir == "kaydet")
                satiskaydet(true, false);
            else if (btn_kaydet_yazdir == "yazdir")
            {
                string pkSatislar = "0";
                DB.pkSatislar = int.Parse(pkSatislar);
                string YaziciAdi = "", YaziciDosyasi = "";
                DataTable dtYazicilar =
                DB.GetData("SELECT  YaziciAdi,Dosya FROM SatisFisiSecimi where Sec=1");
                if (dtYazicilar.Rows.Count == 1)
                {
                    YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                    YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();
                }
                else if (dtYazicilar.Rows.Count > 1)
                {
                    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari();
                    YaziciAyarlari.ShowDialog();
                    YaziciAyarlari.Tag = 0;
                    YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;
                    if (YaziciAyarlari.YaziciAdi.Tag == null)
                        YaziciAdi = "";
                    else
                        YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                    YaziciAyarlari.Dispose();
                }

                if (YaziciAdi != "")
                {
                    satiskaydet(true, false);
                    FisYazdir(false, pkSatislar, YaziciDosyasi, YaziciAdi);
                }
            }
            if (AcikSatisindex == 1)
            {
                lcBakiye1.Text = "0,00";
                lcBakiye1.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 2)
            {
                lcBakiye2.Text = "0,00";
                lcBakiye2.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 3)
            {
                lcBakiye3.Text = "0,00";
                lcBakiye3.BackColor = System.Drawing.Color.Transparent;
            }
            else if (AcikSatisindex == 4)
            {
                lcBakiye4.Text = "0,00";
                lcBakiye4.BackColor = System.Drawing.Color.Transparent;
            }
            //yesilisikyeni();
            //lueSatisTipi.EditValue = 2;
            DB.ExecuteSQL("UPDATE Firmalar Set SonSatisTarihi=getdate() where pkFirma=" + fkFirma);
            //lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }
        void SatisKaydetveyaYazdir(string KaydetmiYazdirmi)
        {
            if (pkSatisBarkod.Text == "0")
            {
                yesilisikyeni();
                return;
            }
            pro_kaydetyazdir(KaydetmiYazdirmi);
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            deGuncellemeTarihi.EditValue = null;
            deGuncellemeTarihi.Enabled = false;
            cbOdemeSekli.Enabled = false;
            yesilisikyeni();
            lueSatisTipi.EditValue = 2;
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }
        private void simpleButton37_Click(object sender, EventArgs e)
        {
            SatisKaydetveyaYazdir("kaydet");
        }
        private void btnyazdir_Click(object sender, EventArgs e)
        {
            SatisKaydetveyaYazdir("yazdir");
        }
        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }
        void FisYazdir(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
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
                DataTable Sirket = DB.GetData(@"select * from Sirketler");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                //Bakiye bilgileri
                //DataTable Bakiye = DB.GetData("exec sp_MusteriBakiyesi " + fkFirma + ",0");
                DataTable Bakiye = DB.GetData(@"select dbo.fon_MusteriBakiyesi(fkFirma) as Bakiye,ToplamTutar as FisTutar,dbo.fon_MusteriBakiyesi(fkFirma)-ToplamTutar as OncekiBakiye from Satislar
where pkSatislar=" + fisid);
                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);
                //Firma bilgileri
                DataTable Musteri = DB.GetData("select * from Firmalar where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                //new xrCariHareket().DataSource = ds;
                //new xrCariHareket().LoadLayout(RaporDosyasi);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    //new xrCariHareket().ShowDesigner();
                    rapor.ShowDesigner();
                else
                    //new xrCariHareket().Print(YaziciAdi);//.ShowPreview();
                    rapor.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        void RaporOnizleme(bool Disigner)
        {
            string fisid = pkSatisBarkod.Text;
            //if (lueFis.EditValue != null)
            //  fisid = lueFis.EditValue.ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Satislar s 
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar
inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
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
        private string musteriara()
        {
            string fkFirma = "1", ozelkod = "0", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;
            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.fkFirma.Tag = fkFirma;
            MusteriAra.ShowDialog();
            if (MusteriAra.fkFirma.Tag.ToString() == fkFirma) return fkFirma;
            fkFirma = MusteriAra.fkFirma.Tag.ToString();

            if (pkSatisBarkod.Text != "0" || pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE Satislar SET fkFirma=" + fkFirma + " where pkSatislar=" + pkSatisBarkod.Text);
            }
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + fkFirma);
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            MusteriAra.Dispose();
           
            BakiyeGetirSecindex(fkFirma);
            yesilisikyeni();
            return fkFirma;
        }
        void BakiyeGetirSecindex(string pkFirma)
        {
            DataTable dt = DB.GetData("exec sp_MusteriBakiyesi " + pkFirma + ",0");
            if (dt.Rows.Count == 0)
            {
                if (AcikSatisindex == 1)
                {
                    lcBakiye1.Text = "0,00";
                    lcBakiye1.BackColor = System.Drawing.Color.Transparent;
                }
                if (AcikSatisindex == 2)
                {
                    lcBakiye2.Text = "0,00";
                    lcBakiye2.BackColor = System.Drawing.Color.Transparent;
                }
                if (AcikSatisindex == 3)
                {
                    lcBakiye3.Text = "0,00";
                    lcBakiye3.BackColor = System.Drawing.Color.Transparent;
                }
                if (AcikSatisindex == 4)
                {
                    lcBakiye4.Text = "0,00";
                    lcBakiye4.BackColor = System.Drawing.Color.Transparent;
                }
            }
            else
            {
                decimal bakiye = decimal.Parse(dt.Rows[0][0].ToString());
                if (AcikSatisindex == 1)
                {
                    lcBakiye1.Text = bakiye.ToString("##0.00");
                    if (lcBakiye1.Text == "0,00")
                        lcBakiye1.BackColor = System.Drawing.Color.Transparent;
                    else
                        lcBakiye1.BackColor = System.Drawing.Color.Red;
                }
                else if (AcikSatisindex == 2)
                {
                    lcBakiye2.Text = bakiye.ToString("##0.00");
                    if (lcBakiye2.Text == "0,00")
                        lcBakiye3.BackColor = System.Drawing.Color.Transparent;
                    else
                        lcBakiye2.BackColor = System.Drawing.Color.Red;
                }
                else if (AcikSatisindex == 3)
                {
                    lcBakiye3.Text = bakiye.ToString("##0.00");
                    if (lcBakiye3.Text == "0,00")
                        lcBakiye3.BackColor = System.Drawing.Color.Transparent;
                    else
                        lcBakiye3.BackColor = System.Drawing.Color.Red;
                }
                else if (AcikSatisindex == 4)
                {
                    lcBakiye4.Text = bakiye.ToString("##0.00");
                    if (lcBakiye4.Text == "0,00")
                        lcBakiye4.BackColor = System.Drawing.Color.Transparent;
                    else
                        lcBakiye4.BackColor = System.Drawing.Color.Red;
                }
            }
        }
        private string musteriata(string fkFirma)
        {
            string pkFirma = "1", ozelkod = "0", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;            

            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where OzelKod=" + fkFirma);
            if (dt.Rows.Count == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Bulunamadı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "1";
            }
            pkFirma = dt.Rows[0]["pkFirma"].ToString();
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            if (pkSatisBarkod.Text != "0" || pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE Satislar SET fkFirma=" + pkFirma + " where pkSatislar=" + pkSatisBarkod.Text);
            }
            BakiyeGetirSecindex(pkFirma);
            yesilisikyeni();
            return fkFirma;
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            musteriara();
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
            string pkStokKartiid = dr["pkStokKartiid"].ToString();
            if (pkStokKartiid == "" || pkStokKartiid == "0")
                DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            else
                DB.pkStokKarti = int.Parse(pkStokKartiid);
            StokKarti.ShowDialog();
            Fiyatlarigetir();
            xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Clear();
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
                iadebasildi();
                //gridView1.SetFocusedRowCellValue("Barcode", "");
                //((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                yesilisikyeni();
                SatisTipiKaydet();
            }
            //nakit
            lueKKarti.Visible = false;
            if ((e.KeyValue == 78 || e.KeyValue == 75 || e.KeyValue == 65 || e.KeyValue == 68) && cbOdemeSekli.Enabled == false)
            {
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                return;
            }
            if (e.KeyValue == 78)
            {
                cbOdemeSekli.TabStop = true;
                cbOdemeSekli.SelectedIndex = 0;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //kkartı
            else if (e.KeyValue == 75)
            {
                cbOdemeSekli.TabStop = true;
                cbOdemeSekli.SelectedIndex = 1;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                lueKKarti.Properties.DataSource = DB.GetData("select * from Bankalar where Aktif=1");
                lueKKarti.Visible = true;
                lueKKarti.ItemIndex = 0;
            }
            //veresiye açık hesap
            else if (e.KeyValue == 65)
            {
                cbOdemeSekli.TabStop = true;
                cbOdemeSekli.SelectedIndex = 2;
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //DİĞER
            else if (e.KeyValue == 68 )
            {
                cbOdemeSekli.TabStop = true;
                cbOdemeSekli.SelectedIndex = 3;
                ((DevExpress.XtraEditors.TextEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
        }
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);
            AppearanceDefault appErrorRed = new AppearanceDefault(Color.OrangeRed);

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            else if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["AlisFiyati"].ToString() != "")
            {
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                    AppearanceHelper.Apply(e.Appearance, appError);
            }
            else if (e.Column.FieldName == "iskontotutar" && dr["iskontotutar"].ToString() != "0,000000")
            {
                AppearanceHelper.Apply(e.Appearance, appfont);
            }
            else if (e.Column.FieldName == "Adet" && dr["Adet"].ToString() == "0")
            {
                AppearanceHelper.Apply(e.Appearance, appError);
            }
            else if (e.Column.FieldName == "Mevcut" && dr["KritikMiktar"].ToString() != "" && dr["Mevcut"].ToString() != "")
            {
                decimal KritikMiktar = Convert.ToDecimal(dr["KritikMiktar"].ToString());
                decimal Mevcut = Convert.ToDecimal(dr["Mevcut"].ToString());
                if (KritikMiktar > Mevcut)
                    AppearanceHelper.Apply(e.Appearance, appErrorRed);
            }
        }
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string iade = View.GetRowCellDisplayText(e.RowHandle, View.Columns["iade"]);
                string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
                //if (iade.Trim() != "Seçim Yok")
                // {
                //   e.Appearance.BackColor = Color.DeepPink;
                //}
                if (Fiyat.Trim() == "0")
                {
                    e.Appearance.BackColor = Color.Red;
                }

            }
        }
        void toplamlar()
        {
            decimal aratop = 0;
            if (gridColumn5.SummaryItem.SummaryValue == null)
                aratop = 0;//atis1Toplam.Text = "0,0";
            else
                aratop = decimal.Parse(gridColumn5.SummaryItem.SummaryValue.ToString());

            decimal isyuzde = 0, isfiyat = 0, istutar = 0;
            //önce hesapla sonra bilgi göster NullTexde
            if (ceiskontoyuzde.EditValue != null)
            {
                isfiyat = decimal.Parse(ceiskontoyuzde.EditValue.ToString());
                istutar = isfiyat * aratop / 100;
            }
            if (ceiskontoTutar.EditValue != null) // && iskontoTutar.EditValue.ToString() !="0")
            {
                isfiyat = decimal.Parse(ceiskontoTutar.EditValue.ToString());
                if (aratop > 0)
                    isyuzde = (isfiyat * 100) / aratop;
                istutar = isfiyat;
            }

            aratoplam.Value = aratop - istutar;
            
            if (aratop > 0 && ceiskontoTutar.EditValue != null)
                ceiskontoyuzde.Properties.NullText = "% " + Convert.ToDouble(((ceiskontoTutar.Value * 100) / aratop)).ToString("#0.0");
            else
                ceiskontoyuzde.Properties.NullText = "";
            if (aratop > 0 && ceiskontoyuzde.EditValue != null)
                ceiskontoTutar.Properties.NullText = Convert.ToDouble(((ceiskontoyuzde.Value * aratop) / 100)).ToString("##0.00");
            else
                ceiskontoTutar.Properties.NullText = "";
            //if (gridColumn28.SummaryItem.SummaryValue == null)
            //    cefark.EditValue = "0,0";
            //else
            //    cefark.EditValue = gridColumn28.SummaryItem.SummaryValue.ToString();

            //cefark.Value = aratoplam.Value - iskontoTutar.Value;
        }

        private void satışİptalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);
        }

        private void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
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
                catch (Exception exp)
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
            Hizlibuttonlariyukle();
        }

        private void simpleButton_MouseDown(object sender, MouseEventArgs e)
        {
            //if(e.Button==Keys.Left)
            int l = 0;
            string lef = "0";
            string barkod = "";
            if (((SimpleButton)sender).Tag != null)
            {
                barkod = ((SimpleButton)sender).Tag.ToString();
                lef = ((SimpleButton)sender).Left.ToString();
            }
            int.TryParse(lef, out l);
            frmHizliButtonDuzenle HizliButtonDuzenle = new frmHizliButtonDuzenle();
            HizliButtonDuzenle.Left = l;
            HizliButtonDuzenle.barkod.Text = barkod;
            HizliButtonDuzenle.ShowDialog();
        }

        private void ürünBulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton4_Click(sender, e);
        }

        void FisListesi()
        {
            string sql = @"select  Top 20 pkSatislar,f.Firmaadi,ToplamTutar,s.GuncellemeTarihi as Tarih,s.OdemeSekli,k.KullaniciAdi,Sd.Durumu from Satislar s
LEFT JOIN Firmalar f ON s.fkFirma = f.PkFirma
LEFT JOIN Kullanicilar k ON s.fkKullanici=k.pkKullanicilar
LEFT JOIN SatisDurumu sd ON sd.pkSatisDurumu=s.fkSatisDurumu
where s.Siparis=1 and s.fkSatisDurumu<>10 order by SonislemTarihi desc";
            lueFis.Properties.DataSource = DB.GetData(sql);
        }

        private void sb_MouseEnter(object sender, EventArgs e)
        {
            HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            HizliTop = ((SimpleButton)sender).Top;
            HizliLeft = ((SimpleButton)sender).Left;
            HizliBarkodName = ((SimpleButton)sender).Name;
            pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;
        }

        private void lueFis_EditValueChanged(object sender, EventArgs e)
        {
            if (lueFis.EditValue == null) return;
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis();
            FisNoBilgisi.fisno.EditValue = lueFis.EditValue.ToString();
            FisNoBilgisi.ShowDialog();
            if (FisNoBilgisi.TopMost == true)
            {
                AcikSatisindex = 4;
                string fisno = lueFis.EditValue.ToString();
                DB.ExecuteSQL("update Satislar Set Siparis=0,AcikHesap=0 where pkSatislar=" + fisno);
                DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
                SatisGetir(dr["pkSatislar"].ToString());
            }
            FisNoBilgisi.Dispose();
            FisListesi();
            lueFis.EditValue = null;
            yesilisikyeni();
        }

        void SatisGetir(string pkSatislar)
        {
            DataTable dtSatislar = DB.GetData("exec sp_Satislar " + pkSatislar);//fiş bilgisi
            if (pkSatislar == "0")
            {
                deGuncellemeTarihi.Enabled = false;
                cbOdemeSekli.Enabled = false;
                deGuncellemeTarihi.EditValue = null;
                cbOdemeSekli.Text = "Nakit";
                return;
            }
            if (dtSatislar.Rows.Count == 0)
            {
                Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            string fkfirma = dtSatislar.Rows[0]["fkFirma"].ToString();
            string OzelKod = dtSatislar.Rows[0]["OzelKod"].ToString();
            string firmaadi = dtSatislar.Rows[0]["Firmaadi"].ToString();
            cbOdemeSekli.Text = dtSatislar.Rows[0]["OdemeSekli"].ToString();

         if (dtSatislar.Rows[0]["fkSatisDurumu"].ToString() != "")
             lueSatisTipi.EditValue = int.Parse(dtSatislar.Rows[0]["fkSatisDurumu"].ToString());

         if (dtSatislar.Rows[0]["GuncellemeTarihi"].ToString() == "")
             deGuncellemeTarihi.EditValue = null;
         else
             deGuncellemeTarihi.DateTime = Convert.ToDateTime(dtSatislar.Rows[0]["GuncellemeTarihi"].ToString());

            if (cbOdemeSekli.Text == "Diğer...") cbOdemeSekli.Text = "Nakit";
            if (cbOdemeSekli.Text == "Kredi Kartı")
            {
                lueKKarti.Properties.DataSource = DB.GetData("select * from Bankalar where Aktif=1");
                if (dtSatislar.Rows[0]["fkBankalar"].ToString() == "")
                    lueKKarti.ItemIndex = 0;
                else
                    lueKKarti.EditValue = int.Parse(dtSatislar.Rows[0]["fkBankalar"].ToString());
            }
            
            deGuncellemeTarihi.Enabled = true;
            cbOdemeSekli.Enabled = true;
            //BakiyeGetirSecindex(fkfirma);
        }

        private void repositoryItemComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //iskonto yüzde
            if (e.KeyCode == Keys.Enter)
            {
                string iskontoyuzde =
                ((DevExpress.XtraEditors.ComboBox)(((DevExpress.XtraEditors.ComboBox)(sender)).Properties.OwnerEdit)).Text;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                decimal SatisFiyati = Convert.ToDecimal(dr["SatisFiyati"].ToString());
                decimal Adet = Convert.ToDecimal(dr["Adet"].ToString());
                decimal TopTutar = SatisFiyati * Adet;
                decimal iskontotutar = (TopTutar * Convert.ToDecimal(iskontoyuzde) / 100);
                gridView1.SetFocusedRowCellValue(gridColumn33, iskontoyuzde);
                yesilisikyeni();
            }
        }

        void iskontotutarDegistir(string iskontotutar)
        {
            if (iskontotutar == "")
            {
                yesilisikyeni();
                return;
            }
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSatisDetay = dr["pkSatisDetay"].ToString();
            decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
            decimal Miktar = Convert.ToDecimal(dr["Adet"].ToString().Replace(",", "."));
            if (Miktar == 0) return;
            decimal iskontoyuzde = (Convert.ToDecimal(iskontotutar) * 100) / (Fiyat * Miktar);
            gridView1.SetFocusedRowCellValue(gridColumn33, iskontoyuzde);
            DB.ExecuteSQL("UPDATE SatisDetay SET iskontotutar=" + iskontotutar.Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
        }

        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //iskonto tutar
            if (e.KeyCode == Keys.Enter)
            {
                string iskontotutar =
                ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
                iskontotutarDegistir(iskontotutar);
                yesilisikyeni();
            }
        }


        private void simpleButton17_Click(object sender, EventArgs e)
        {
            frmSatisRaporlari Satislar = new frmSatisRaporlari();
            Satislar.ShowDialog();
            yesilisikyeni();
        }

        private void cariSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            musteriara();
        }

        private void yenidenAdlandırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HizliButtonAdiDegis ButtonAdiDegis = new HizliButtonAdiDegis();
            ButtonAdiDegis.Top = HizliTop;
            ButtonAdiDegis.Left = HizliLeft;
            ButtonAdiDegis.oncekibarkod.Text = HizliBarkod;
            ButtonAdiDegis.oncekibarkod.Tag = pkHizliStokSatis;
            ButtonAdiDegis.ShowDialog();

            Hizlibuttonlariyukle();
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            string pkSatis = "0";
            if (pkSatis == "0")
            {
                Showmessage("Önce Satış Yapınız!", "K");
                return;
            }
            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.memoozelnot.Text = btnAciklamaGirisi.ToolTip;
            fFisAciklama.ShowDialog();
            btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;
            DB.ExecuteSQL("UPDATE Satislar SET Aciklama='" + btnAciklamaGirisi.ToolTip + "' where pkSatislar=" + pkSatis);
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

        private void satistipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lueSatisTipi.EditValue.ToString())
            {
                case "1":
                    {
                        btnyazdir.Text = "Fiş Yazdır [F11]"; break;
                        btnyazdir.Tag = "0";
                    }
                case "3":
                    {
                        btnyazdir.Text = "Fatura Yazdır [F11]"; break;
                        btnyazdir.Tag = "0";
                    }
            }
            FisListesi();
        }

        public bool OnceSatisYapiniz()
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesaj Mesaj = new frmMesaj();
                Mesaj.label1.Text = "Önce Satış Yapınız!";
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.Show();
                return false;
            }
            return true;
        }

        private void lueFiyatlar_EditValueChanged(object sender, EventArgs e)
        {
            FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
            //if(!ilkyukleme)
            //  yesilisikyeni();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE HizliStokSatis Set fkStokKarti=0 where pkHizliStokSatis=" + pkHizliStokSatis);

            Hizlibuttonlariyukle();
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
        void OdemeSekli(string odemesekli)
        {
            if (ilkyukleme) return;
            string pkSatislar = "0";
            string musteriadi = "";
            string pkFirma = "1";
            DB.pkSatislar = int.Parse(pkSatislar);
            DB.PkFirma = int.Parse(pkFirma);
            lueKKarti.Visible = false;
            if (odemesekli == "Diğer...")
            {
                //if (pkSatisBarkod.Text == "0")
                //{
                //    Showmessage("Lütfen Önce Ürün Ekleyiniz!", "K");
                //    cbOdemeSekli.Text = "Nakit";
                //    return;
                //}
                cbOdemeSekli.Text = odemesekli;
                digersorunu = false;
                if (pkFirma == "1")
                {
                    pkFirma = musteriara();
                }
                frmSatisOdeme SatisOdeme = new frmSatisOdeme();

                SatisOdeme.MusteriAdi.Tag = pkFirma;
                SatisOdeme.satistutari.ToolTip = odemesekli;
                SatisOdeme.satistutari.EditValue = aratoplam.EditValue;
                SatisOdeme.ShowDialog();
                if (SatisOdeme.TopMost == true)
                {
                    pro_kaydetyazdir(SatisOdeme.kaydetmiyazdirmi.Text);//önemli
                    SatisOdeme.Dispose();
                    cbOdemeSekli.SelectedIndex = 0;
                    //yesilisikyeni();
                    return;
                }
                else
                {
                    SatisOdeme.Dispose();
                    cbOdemeSekli.SelectedIndex = 0;
                    digersorunu = true;
                    return;
                }
            }
            //}
            else if (odemesekli == "Kredi Kartı")
            {
                //if (pkSatisBarkod.Text == "0")
                //{
                //    Showmessage("Lütfen Önce Ürün Ekleyiniz!", "K");
                //    cbOdemeSekli.Text = "Nakit";
                //    return;
                //}
                lueKKarti.Properties.DataSource = DB.GetData("Select * from Bankalar where Aktif=1");
                lueKKarti.Visible = true;
                lueKKarti.Focus();
                lueKKarti.ItemIndex = 0;
            }
            else if (odemesekli == "Açık Hesap")
            {

                //if (pkSatisBarkod.Text == "0")
                //{
                //    Showmessage("Lütfen Önce Ürün Ekleyiniz!", "K");
                //    cbOdemeSekli.Text = "Nakit";
                //    return;
                //}

                TaksitOdemeTarihi.Visible = true;
                btnKampanya.Visible = true;
                //TaksitOdemeTarihi.DateTime = DateTime.Today.AddDays(30);
                if (pkFirma == "1")
                {
                    pkFirma = musteriara();
                }
            }
        }
        private void cbOdemeSekli_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            // MessageBox.Show(cbOdemeSekli.SelectedIndex.ToString());
            // return;
            // if (e.Value.ToString() == "Diğer...")
            //     cbOdemeSekli.SelectedIndex = 3;
            // OdemeSekli(e.Value.ToString());
            //if (e.Value.ToString() == "Diğer...")
            // {
            //     cbOdemeSekli.SelectedIndex = 3;
            //     e.Value = "Nakit";
            // }
            // yesilisikyeni();
        }

        private void odemeseklidigerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli("Diğer...");
        }

        private void odemeseklikkartiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli("Kredi Kartı");
        }
        private void odemesekliacikhesapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdemeSekli("Açık Hesap...");
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
            if (lueSatisTipi.Text == "İade")
            {
                decimal miktar = decimal.Parse(yenimiktar);
                if (miktar > 0)
                    miktar = miktar * -1;
                yenimiktar = miktar.ToString();
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSatisDetay = dr["pkSatisDetay"].ToString();
            DB.ExecuteSQL("UPDATE SatisDetay SET Adet=" + yenimiktar.Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
            decimal iskontoyuzde = 0;
            if (dr["iskontoyuzdetutar"].ToString() != "")
                iskontoyuzde = Convert.ToDecimal(dr["iskontoyuzdetutar"].ToString());
            decimal iskontogercekyuzde = iskontoyuzde;// Convert.ToDecimal(dr["iskontoyuzde"].ToString());
            decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
            decimal Miktar = Convert.ToDecimal(yenimiktar);
            decimal iskontogercektutar = Convert.ToDecimal(dr["iskontotutar"].ToString());

            if (iskontogercektutar > 0)
            {
                iskontogercekyuzde = (iskontogercektutar * 100) / (Fiyat * Miktar);
            }
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn3, yenimiktar);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridColumn33, iskontogercekyuzde.ToString());

        }
        private void iskontoTutar_EditValueChanged(object sender, EventArgs e)
        {
            ceiskontoyuzde.EditValue = null;
        }
        void dockPanel1Gizle()
        {
            if (dockPanel1.Width > 45)
            {
                dockPanel1.Width = 40;
                dockPanel1.SendToBack();
                yesilisikyeni();
            }
        }
        void dockPanel1goster()
        {
            if (dockPanel1.Width == 810)
            {
                dockPanel1.Width = 40;
                dockPanel1.SendToBack();
            }
            else
            {
                dockPanel1.Width = 810;
                dockPanel1.BringToFront();
            }
        }
        private void xtraTabPage5_Click(object sender, EventArgs e)
        {
            if (xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Controls.Count == 0)
                Hizlibuttonlariyukle();
            dockPanel1goster();
        }

        private void iskontoTutar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        private void repositoryItemButtonEdit1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
        }
        private void pkSatisBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //fiş düzenle
            if (e.KeyCode == Keys.Enter)
            {

                if (pkSatisBarkod.Text == "") return;
                if (DB.GetData("select pkSatislar From Satislar where pkSatislar=" + pkSatisBarkod.Text).Rows.Count == 0)
                {
                    Showmessage("Fiş Bulunamadı.", "K");
                    return;
                }
                lueFis.EditValue = int.Parse(pkSatisBarkod.Text);
                if (lueFis.EditValue == null)
                    lueFis.EditValue = pkSatisBarkod.Text;
                //lueFis_EditValueChanged(sender, e);
                //Satis4Toplam.Tag = pkSatisBarkod.Text;
                //SatisGetir();

                //gCSatisDuzen.Visible = true;
                //Satis4Toplam_Click(sender, e);

            }
        }



        private void gridControl1_MouseEnter(object sender, EventArgs e)
        {
            dockPanel1Gizle();
        }

        private void ceiskontoyuzde_EditValueChanged(object sender, EventArgs e)
        {
            ceiskontoTutar.EditValue = null;
        }

        private void ceiskontoyuzde_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        private void lueSatisTipi_EditValueChanged(object sender, EventArgs e)
        {
            //if (lueSatisTipi.EditValue.ToString() == "5" || lueSatisTipi.EditValue.ToString() == "9")//iade ve değişim
            //    gciade.Visible = true;
            //else
            //    gciade.Visible = false;
            simpleButton10.Visible = false;
            if (lueSatisTipi.EditValue.ToString() == "4" || lueSatisTipi.EditValue.ToString() == "3")//fatura ve irsaliye
            {
                gckdv.Visible = true;
                btnyazdir.Visible = false;
                pFaturaTarihi.Visible = true;//fatura 
                //lbFaturaTarihi.Visible = true;//fatura 
                //FaturaTarihi.Visible = true;//fatura 
                simpleButton9.Visible = true;//fatura 
                simpleButton10.Visible = true;//özel fatura
            }
            else if (lueSatisTipi.EditValue.ToString() == "1")//teklif
            {
                simpleButton10.Visible=true;
            }
            else
            {
                btnyazdir.Visible = true;
                gckdv.Visible = false;
                simpleButton9.Visible = false;//fatura 
                simpleButton10.Visible = false;//özel fatura
                //lbFaturaTarihi.Visible = false;//fatura 
                //FaturaTarihi.Visible = false;//fatura 
                pFaturaTarihi.Visible = false;//fatura 
            }

            FisListesi();
            if (lueSatisTipi.Text == "İade")
                DB.ExecuteSQL("UPDATE SatisDetay SET Adet=abs(Adet)*-1 where fkSatislar=" + pkSatisBarkod.Text);
            else if (lueSatisTipi.Text == "Değişim")
                DB.GetData("select getdate()");
            else
                DB.ExecuteSQL("UPDATE SatisDetay SET Adet=abs(Adet) where fkSatislar=" + pkSatisBarkod.Text);
            //Satış Tipini Kaydet
            DB.ExecuteSQL("UPDATE satislar set fkSatisDurumu=" + lueSatisTipi.EditValue.ToString() + " where pkSatislar=" + pkSatisBarkod.Text);
            if (!ilkyukleme)
                yesilisikyeni();
        }

        void SatisDigerBilgileriGetir()
        {
            //TODO:SatisDigerBilgileriGetir
            DataTable dt = DB.GetData("select pkSatislar,fkFirma from Satislar where pkSatislar=0");
            btnAciklamaGirisi.ToolTip = dt.Rows[0]["Aciklama"].ToString();
        }

        void SatisDetayGetir(string pkSatislar)
        {
            gridControl1.DataSource = DB.GetData("exec sp_SatisDetay " + pkSatislar + ",0");
            toplamlar();
            gridView1.AddNewRow();
        }

        void SatisDetayEkle(string barkod)
        {
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
            if (dr != null && dr["pkSatisDetay"].ToString() != "") return;
            if (barkod == "" || f == 0) return;
            YeniSatisEkle();
            //başında sıfır varsa
            if (barkod.Length == 3)
                barkod = (1 * decimal.Parse(barkod)).ToString();
            //gramlı ürünler
            if (barkod.Length == 13 && barkod.Substring(0, 3) == DB.TeraziBarkoduBasi.ToString())
            {
                EklenenMiktar = decimal.Parse(barkod.Substring(7, 5)) / 1000;
                barkod = barkod.Substring(2, 5);
                barkod = (1 * decimal.Parse(barkod)).ToString();
            }
            //iade
            if (lueSatisTipi.Text == "İade") EklenenMiktar = EklenenMiktar * -1;
            DataTable dtStokKarti = DB.GetData("SELECT pkStokKarti FROM StokKarti where Barcode='" + barkod + "'");
            if (dtStokKarti.Rows.Count == 0)
            {
                frmStokKartiHizli StokKartiHizli = new frmStokKartiHizli();
                StokKartiHizli.ceBarkod.EditValue = barkod;
                StokKartiHizli.ShowDialog();
                if (StokKartiHizli.TopMost == true)
                {
                    dtStokKarti = DB.GetData("select pkStokKarti From StokKarti WHERE Barcode='" + StokKartiHizli.ceBarkod.EditValue.ToString() + "'");
                }
                else
                {
                    yesilisikyeni();
                    StokKartiHizli.Dispose();
                    return;
                }
                StokKartiHizli.Dispose();
            }
            string pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();
            ArrayList arr = new ArrayList();
            arr.Add(new SqlParameter("@fkSatislar", pkSatisBarkod.Text));
            arr.Add(new SqlParameter("@SatisFiyatGrubu", lueFiyatlar.EditValue.ToString()));
            arr.Add(new SqlParameter("@Adet", EklenenMiktar.ToString().Replace(",", ".")));
            arr.Add(new SqlParameter("@fkStokKarti", pkStokKarti));
            arr.Add(new SqlParameter("@iskontoyuzde", "0"));
            arr.Add(new SqlParameter("@iskontotutar", "0"));
            string s = DB.ExecuteScalarSQL("exec sp_SatisDetay_Ekle @fkSatislar,@SatisFiyatGrubu,@Adet,@fkStokKarti,@iskontoyuzde,@iskontotutar", arr);
            if (s != "Satis Detay Eklendi.")
            {
                MessageBox.Show(s);
                return;
            }
            HizliMiktar = 1;
        }

        void YeniSatis()
        {
            string sql = "";
            string fisno = "0";
            bool yazdir = false;
            ArrayList list = new ArrayList();
            string pkFirma = "1";
            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Siparis", "0"));
            list.Add(new SqlParameter("@fkKullanici", lueKullanicilar.EditValue.ToString()));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));//iskontoTutar.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.Text));


            sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen,OdemeSekli,SonislemTarihi)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar,0,0,@OdemeSekli,getdate()) SELECT IDENT_CURRENT('Satislar')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            deGuncellemeTarihi.Enabled = true;
            cbOdemeSekli.Enabled = true;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Bilgileri ve Genel Durumu. Yapım Aşamasındadır...");
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari();
            string pkSatislar = "0";
            DB.pkSatislar = int.Parse(pkSatislar);
            YaziciAyarlari.Tag = 1;
            YaziciAyarlari.ShowDialog();
        }

        private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmiskonto iskonto = new frmiskonto();
            iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
            iskonto.ceBirimFiyati.Value = decimal.Parse(dr["SatisFiyati"].ToString());
            iskonto.seMiktar.Value = decimal.Parse(dr["Adet"].ToString());
            iskonto.ShowDialog();
            yesilisikyeni();
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr == null)
                    gridView1.DeleteRow(i);
                else if (dr["pkSatisDetay"].ToString() == "")
                    gridView1.DeleteRow(i);
            }
        }


        private void gridControl1_Click(object sender, EventArgs e)
        {
            yesilisikyeni();
        }

        private void repositoryItemCalcEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dr == null) return;
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
                decimal NakitFiyati = decimal.Parse(dr["NakitFiyati"].ToString());
                decimal iskonto = 0, iskontotutar = 0;
                decimal.TryParse(girilen, out iskontotutar);
                iskonto = NakitFiyati - iskontotutar;
                DB.ExecuteSQL("UPDATE SatisDetay SET iskontotutar=" + iskonto.ToString().Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
                yesilisikyeni();
            }
        }

        private void pkSatisBarkod_Enter(object sender, EventArgs e)
        {
            pkSatisBarkod.Text = "";
        }

        private void repositoryItemButtonEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string iskontoyuzdetutargirilen =
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
                if (iskontoyuzdetutargirilen == "")
                {
                    yesilisikyeni();
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
                decimal Fiyat = Convert.ToDecimal(dr["SatisFiyati"].ToString());
                //decimal Miktar = Convert.ToDecimal(dr["Adet"].ToString().Replace(",", "."));
                decimal iskontoyuzdetutar = 0, iskontoyuzde = 0;
                decimal.TryParse(iskontoyuzdetutargirilen.ToString(), out iskontoyuzde);
                iskontoyuzdetutar = (iskontoyuzde * Fiyat) / 100;
                //gridView1.SetFocusedRowCellValue(gridColumn33, iskontoyuzde);
                DB.ExecuteSQL("UPDATE SatisDetay SET iskontoyuzdetutar=" + iskontoyuzdetutar.ToString().Replace(",", ".") + " where pkSatisDetay=" + pkSatisDetay);
                yesilisikyeni();
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyValue == 222)
            {
                iskontoyagit_ctrli();
            }
            //GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            //if (ghi.Column == null) return;
            //if (ghi.Column.Name == "gridColumn1")
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);


            if (e.KeyCode == Keys.Enter && !gridView1.IsEditing && gridView1.FocusedColumn.FieldName == "iskyuzdesanal")
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                frmiskonto iskonto = new frmiskonto();
                iskonto.fkSatisDetay.Text = dr["pkSatisDetay"].ToString();
                iskonto.ShowDialog();
            }
        }


        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (gridView1.DataRowCount == 0) e.Cancel = true;
        }

        private void satışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int f = gridView1.FocusedRowHandle;
            if (f < 0)
            {
                f = gridView1.DataRowCount - 1;
            }
            DataRow dr = gridView1.GetDataRow(f);
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            SatisFiyatlari.ShowDialog();
            int i = int.Parse(lueFiyatlar.EditValue.ToString());
            Fiyatlarigetir();
            lueFiyatlar.EditValue = i;
            yesilisikyeni();
        }

        private void iskontoTutar_Enter(object sender, EventArgs e)
        {
            ceiskontoTutar.EditValue = null;
        }

        private void iskontoTutar_Leave(object sender, EventArgs e)
        {
            if (ceiskontoTutar.EditValue == null)
                ceiskontoTutar.EditValue = ceiskontoTutar.OldEditValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ceiskontoyuzde.Value = 0;
            yesilisikyeni();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ceiskontoTutar.Value = 0;
            yesilisikyeni();
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        void fontayarla(LabelControl lb)
        {
            if (lb.Text.Length < 4)
                lb.Font = new Font(lb.Font.Name, 36, FontStyle.Bold);
            if (lb.Text.Length == 4)
                lb.Font = new Font(lb.Font.Name, 35, FontStyle.Bold);
            if (lb.Text.Length == 5)
                lb.Font = new Font(lb.Font.Name, 34, FontStyle.Bold);
            if (lb.Text.Length == 6)
                lb.Font = new Font(lb.Font.Name, 33, FontStyle.Bold);
            if (lb.Text.Length == 7)
                lb.Font = new Font(lb.Font.Name, 32, FontStyle.Bold);
            if (lb.Text.Length == 8)
                lb.Font = new Font(lb.Font.Name, 31, FontStyle.Bold);
            if (lb.Text.Length == 9)
                lb.Font = new Font(lb.Font.Name, 30, FontStyle.Bold);
            if (lb.Text.Length > 10)
                lb.Font = new Font(lb.Font.Name, 20, FontStyle.Bold);
        }

        void SatisDetayYenile()
        {

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

        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue == null) return;
            string iskontotutar =
               ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue.ToString();
            iskontotutarDegistir(iskontotutar);
            int i = gridView1.FocusedRowHandle;
            SatisDetayYenile();
            gridView1.FocusedRowHandle = i;
        }

        private void repositoryItemCalcEdit3_MouseDown(object sender, MouseEventArgs e)
        {
            //güncelleme sorunu oldu
            //((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).EditValue = null;
        }

        private void müşteriKArtıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriKarti MusteriKarti = new frmMusteriKarti();
            MusteriKarti.ShowDialog();
            //özel kod veya adı için getir ve yenile
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + DB.PkFirma);
            string firmadi = dt.Rows[0]["Firmaadi"].ToString();
            string ozelkod = dt.Rows[0]["OzelKod"].ToString();
        }

        private void cmsPopYazir_Opening(object sender, CancelEventArgs e)
        {
            if (((((System.Windows.Forms.ContextMenuStrip)(sender)).SourceControl).Controls.Owner).Name == "btnyazdir")
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = true;
                müşteriSeçToolStripMenuItem.Visible = false;
                müşteriKArtıToolStripMenuItem.Visible = false;
                müşteriHareketleriToolStripMenuItem1.Visible = false;
            }
            else
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = false;
                müşteriSeçToolStripMenuItem.Visible = true;
                müşteriKArtıToolStripMenuItem.Visible = true;
                müşteriHareketleriToolStripMenuItem1.Visible = true;
            }
        }

        private void bARKOTBASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string pkEtiketBas = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Fiş No " + pkSatisBarkod.Text + "',0) SELECT IDENT_CURRENT('EtiketBas')");
            DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["fkStokKarti"].ToString() + ",1,0,getdate())");
            frmEtiketBas EtiketBas = new frmEtiketBas();
            EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
            EtiketBas.ShowDialog();
        }

        private void sTOKKARTINIDÜZENLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStokKarti StokKarti = new frmStokKarti();
            DataTable dt =
            DB.GetData("SELECT pkStokKarti from StokKarti where Barcode='" + HizliBarkod + "'");
            if (dt.Rows.Count > 0)
                DB.pkStokKarti = int.Parse(dt.Rows[0][0].ToString());
            else
                DB.pkStokKarti = 0;
            StokKarti.ShowDialog();
            Hizlibuttonlariyukle();
        }

        private void simpleButton8_Click_2(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari();
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            Hizlibuttonlariyukle();
        }

        private void cbOdemeSekli_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            if (pkSatisBarkod.Text == "0" && cbOdemeSekli.Text == "Açık Hesap")
            {
                Showmessage("Lütfen Önce Ürün Ekleyiniz!", "K");
                cbOdemeSekli.Text = "Nakit";
                return;
            }
            btnKampanya.Visible = false;
            //if (cbOdemeSekli.TabStop == true)
            // {
            if (cbOdemeSekli.Text == "")
                cbOdemeSekli.Text = "Nakit";
            DB.ExecuteSQL("UPDATE satislar set OdemeSekli='" + cbOdemeSekli.Text + "' where pkSatislar=" + pkSatisBarkod.Text);
            // }
        }

        private void btnKampanya_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE SatisDetay SET SatisFiyati=NakitFiyat WHERE fkSatislar=" + pkSatisBarkod.Text);
            yesilisikyeni();
        }

        private void Satis3Baslik_DoubleClick(object sender, EventArgs e)
        {
            musteriara();
        }


      

        private void Satis1Baslik_DoubleClick_1(object sender, EventArgs e)
        {
            musteriara();
        }

        private void Satis2Baslik_DoubleClick(object sender, EventArgs e)
        {
            musteriara();
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

        }
        void YazdirSatisFaturasi(bool Disigner)
        {
            try
            {
                string fisid = pkSatisBarkod.Text;
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                string sql = @"SELECT  s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama, 
                      sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as ToplamTutar, 
                      Firmalar.Firmaadi, Firmalar.Adres, 
                      Firmalar.Eposta, Firmalar.webadresi, Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, Firmalar.VergiDairesi, Firmalar.VergiNo, 
                      Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu as SatisDurumu, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi as KullaniciAdiSoyadi
                      ,dbo.fnc_ParayiYaziyaCevir(sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)),1) as rakamoku,s.FaturaTarihi,s.TeslimTarihi
FROM  Satislar s
INNER JOIN SatisDetay sd on sd.fkSatislar=s.pkSatislar
INNER JOIN Firmalar ON s.fkFirma = Firmalar.pkFirma 
LEFT OUTER JOIN Kullanicilar ON s.fkKullanici = Kullanicilar.pkKullanicilar 
LEFT OUTER JOIN SatisDurumu ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
LEFT OUTER JOIN FirmaGruplari ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
where s.pkSatislar=@pkSatislar
group by s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama,Firmalar.Eposta, Firmalar.webadresi,
Firmalar.Firmaadi, Firmalar.Adres, 
 Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, 
 Firmalar.VergiDairesi, Firmalar.VergiNo, 
Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu,Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi,
s.FaturaTarihi,s.TeslimTarihi
";
                sql = sql.Replace("@pkSatislar", fisid);
                DataTable Fis = DB.GetData(sql);
                //                DataTable Fis = DB.GetData(@"SELECT  s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama, 
                //                      sd.Adet*(sd.SatisFiyati-sd.iskontotutar) as ToplamTutar, 
                //                      Firmalar.Firmaadi, Firmalar.Adres, 
                //                      Firmalar.Eposta, Firmalar.webadresi, Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, Firmalar.VergiDairesi, Firmalar.VergiNo, 
                //                      Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu as SatisDurumu, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi as KullaniciAdiSoyadi
                //                      ,dbo.RakamOku(sd.Adet*(sd.SatisFiyati)) as rakamoku
                //FROM  Satislar s
                //INNER JOIN SatisDetay sd on sd.fkSatislar=s.pkSatislar
                //INNER JOIN Firmalar ON s.fkFirma = Firmalar.pkFirma 
                //LEFT OUTER JOIN Kullanicilar ON s.fkKullanici = Kullanicilar.pkKullanicilar 
                //LEFT OUTER JOIN SatisDurumu ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
                //LEFT OUTER JOIN FirmaGruplari ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
                //where s.pkSatislar= " + fisid);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                ////kdv oranları
                //DataTable fiskdv = DB.GetData("select KdvOrani,SUM((SatisFiyati-iskontotutar)*Adet) as kdvtutar  from SatisDetay" +
                //" where fkSatislar= " + fisid + " group by KdvOrani");
                //fiskdv.TableName = "fiskdv";
                //ds.Tables.Add(fiskdv);
                //şirket bilgileri
                DataTable sirket = DB.GetData("select top 1 * from Sirketler");
                sirket.TableName = "sirket";
                ds.Tables.Add(sirket);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\SatisFatura.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                xrCariHareket rapor = new xrCariHareket();
                //XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//YaziciAdi
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            YazdirSatisFaturasi(false);
        }

        private void faturaYazdirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirSatisFaturasi(true);
        }

        void YazdirTeklif(bool Disigner)
        {
            try
            {
                string fisid = pkSatisBarkod.Text;
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                string sql = @"SELECT  s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama, 
                      sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as ToplamTutar, 
                      Firmalar.Firmaadi, Firmalar.Adres, 
                      Firmalar.Eposta, Firmalar.webadresi, Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, Firmalar.VergiDairesi, Firmalar.VergiNo, 
                      Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu as SatisDurumu, Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi as KullaniciAdiSoyadi
                      ,dbo.fnc_ParayiYaziyaCevir(sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)),1) as rakamoku,s.FaturaTarihi,s.TeslimTarihi
FROM  Satislar s
INNER JOIN SatisDetay sd on sd.fkSatislar=s.pkSatislar
INNER JOIN Firmalar ON s.fkFirma = Firmalar.pkFirma 
LEFT OUTER JOIN Kullanicilar ON s.fkKullanici = Kullanicilar.pkKullanicilar 
LEFT OUTER JOIN SatisDurumu ON s.fkSatisDurumu = SatisDurumu.pkSatisDurumu 
LEFT OUTER JOIN FirmaGruplari ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari
where s.pkSatislar=@pkSatislar
group by s.pkSatislar, s.Tarih, s.fkFirma, s.Aciklama,Firmalar.Eposta, Firmalar.webadresi,
Firmalar.Firmaadi, Firmalar.Adres, 
 Firmalar.Tel2, Firmalar.Tel, Firmalar.Fax, Firmalar.Yetkili, FirmaGruplari.GrupAdi, 
 Firmalar.VergiDairesi, Firmalar.VergiNo, 
Firmalar.Cep, Firmalar.Cep2, Firmalar.OzelKod, SatisDurumu.Durumu,Kullanicilar.KullaniciAdi, Kullanicilar.adisoyadi,
s.FaturaTarihi,s.TeslimTarihi
";
                sql = sql.Replace("@pkSatislar", fisid);
                DataTable Fis = DB.GetData(sql);
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //şirket bilgileri
                DataTable sirket = DB.GetData("select top 1 * from Sirketler");
                sirket.TableName = "sirket";
                ds.Tables.Add(sirket);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\Teklif.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                xrCariHareket rapor = new xrCariHareket();
                //XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//YaziciAdi
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        private void simpleButton10_Click(object sender, EventArgs e)
        {
            YazdirTeklif(false);
        }

        private void cbOdemeSekli_EditValueChanged(object sender, EventArgs e)
        {
            if (ilkyukleme) return;
            string odemesekli = "";
            if (cbOdemeSekli.SelectedIndex == 0)
                odemesekli = "Nakit";
            if (cbOdemeSekli.SelectedIndex == 1)
                odemesekli = "Kredi Kartı";
            if (cbOdemeSekli.SelectedIndex == 2)
                odemesekli = "Açık Hesap";
            if (cbOdemeSekli.SelectedIndex == 3)
                odemesekli = "Diğer...";
            if (cbOdemeSekli.TabStop == true)
                OdemeSekli(odemesekli);
            yesilisikyeni();
        }
        void SatisTipiKaydet()
        {

            int iadeadedi = 0;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["iade"].ToString() == "True")
                    iadeadedi++;
            }
            if (gridView1.DataRowCount == iadeadedi)
                lueSatisTipi.EditValue = 9;//İade
            else if (iadeadedi == 0)
                lueSatisTipi.EditValue = 2;//Satış
            else if (iadeadedi != 0 || iadeadedi > 0)
                lueSatisTipi.EditValue = 5;//Değişim
            DB.ExecuteSQL(" update Satislar set fkSatisDurumu=" + lueSatisTipi.EditValue.ToString() + " where pkSatislar=" + pkSatisBarkod.Text);
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
            SatisTipiKaydet();
        }

        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fkFirma = "0";
           

            //DB.PkFirma = int.Parse(fkFirma);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = fkFirma;
            CariHareketMusteri.Show();
        }

        private void cbOdemeSekli_Enter(object sender, EventArgs e)
        {
            cbOdemeSekli.TabStop = true;
        }

        private void cbOdemeSekli_Leave(object sender, EventArgs e)
        {
            cbOdemeSekli.TabStop = false;
        }

        private void ucSatis_VisibleChanged(object sender, EventArgs e)
        {
            //if (gridView1.DataRowCount > 0)
            //{
            //    MessageBox.Show("Açık Satışlarınız Var.");
            //}
        }

        private void ucSatis_Enter(object sender, EventArgs e)
        {
            //gridView1.Focus();
            //gridView1.AddNewRow();
            //gridView1.ShowEditor();
           // yesilisikyeni();
        }

        private void bUFİŞMALİYETİToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gcAlisTutar.Visible == true)
                gcAlisTutar.Visible = false;
            else
                gcAlisTutar.Visible = true;
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            //GridView View = sender as GridView;
            if (ghi.RowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(ghi.RowHandle);
            string fkFirma = "0";
            

            string sql = @"select top 10 s.pkSatislar,s.GuncellemeTarihi as Tarih,(sd.SatisFiyati-sd.iskontotutar) as SatisFiyati,Adet from Satislar s
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar
where  s.Siparis=1 and sd.fkStokKarti=@fkStokKarti and s.fkFirma=@fkFirma order by s.GuncellemeTarihi desc";
            sql = sql.Replace("@fkStokKarti", dr["fkStokKarti"].ToString());
            sql = sql.Replace("@fkFirma", fkFirma);
            gcOncekiSatisFiyatlari.DataSource = DB.GetData(sql);
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis();
            FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void tSMIEksikListesi_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string Stokadi = dr["Stokadi"].ToString();
            DB.ExecuteSQL("INSERT INTO EksikListesi (StokAdi,Miktar,FirmaAdi,Aciklama) values('" + Stokadi + "',1,'','')");
        }

        private void lueKullanicilar_EditValueChanged(object sender, EventArgs e)
        {
            //DB.fkKullanicilar = lueKullanicilar.EditValue.ToString();
            DB.ExecuteSQL("update Satislar set fkKullanici=" + lueKullanicilar.EditValue.ToString() +
                " where pkSatislar=" + pkSatisBarkod.Text);
            yesilisikyeni();
        }
        void FiyatGruplariDegis(string fkSatisFiyatlariBaslik)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string fkStokKarti = dr["fkStokKarti"].ToString();
                string pkSatisDetay = dr["pkSatisDetay"].ToString();
                DataTable dt = DB.GetData("select * from SatisFiyatlari where fkStokKarti=" + fkStokKarti + " and fkSatisFiyatlariBaslik=" + fkSatisFiyatlariBaslik);
                if (dt.Rows.Count == 0)
                    dt = DB.GetData("select * from SatisFiyatlari where fkStokKarti=" + fkStokKarti + " and fkSatisFiyatlariBaslik=1");
                string SatisFiyat1 = dt.Rows[0]["SatisFiyatiKdvli"].ToString();
                string sql = "update SatisDetay set SatisFiyati=@SatisFiyati where pkSatisDetay=" + pkSatisDetay;
                sql = sql.Replace("@SatisFiyati", SatisFiyat1.Replace(",", "."));
                DB.ExecuteSQL(sql);
            }
        }
        private void lueFiyatlar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (lueFiyatlar.EditValue != null)
                    FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
                yesilisikyeni();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (lueFiyatlar.EditValue != null)
                    lueFiyatlar.EditValue = lueFiyatlar.OldEditValue;
                yesilisikyeni();
            }
        }

        private void lueFiyatlar_Properties_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            //string v =  e.Value.ToString();
            //lueFiyatlar.EditValue = int.Parse(e.Value.ToString());
            //FiyatGruplariDegis(v);
            //yesilisikyeni();
        }

        private void lueFiyatlar_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            //((DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit)(((DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit)(lueFiyatlar.Properties.Columns.owner)).Columns.owner)).KeyValue
            //lueFiyatlar.PopupForm.CurrentValue
            //lueFiyatlar. = 1;
            string v = e.Value.ToString();
            //lueFiyatlar.EditValue = int.Parse(e.Value.ToString());
            FiyatGruplariDegis(v);
            yesilisikyeni();
        }
        private void lueFiyatlar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    FiyatGruplariDegis(lueFiyatlar.EditValue.ToString());
            //    yesilisikyeni();
            //}
        }

        private void pkSatisBarkod_EditValueChanged(object sender, EventArgs e)
        {
            //Fiş Ararken sorun oldu enter vb... olduğun da arayabilirsin 
            //SatisGetir();
        }

        private void deGuncellemeTarihi_EditValueChanged(object sender, EventArgs e)
        {
            if (deGuncellemeTarihi.Tag == "1")
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@GuncellemeTarihi", deGuncellemeTarihi.DateTime));
                list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
                DB.ExecuteSQL("update Satislar Set GuncellemeTarihi=@GuncellemeTarihi where pkSatislar=@pkSatislar", list);
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

        private void lueKKarti_EditValueChanged(object sender, EventArgs e)
        {
            if (lueKKarti.Tag == "1")
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue));
                list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
                DB.ExecuteSQL("update Satislar Set fkBankalar=@fkBankalar where pkSatislar=@pkSatislar", list);
                yesilisikyeni();
            }
        }

        private void lueKKarti_Enter(object sender, EventArgs e)
        {
            lueKKarti.Tag = "1";
        }

        private void lueKKarti_Leave(object sender, EventArgs e)
        {
            lueKKarti.Tag = "0";
        }

        private void teklifDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirTeklif(true);
        }

        private void lueSatisTipi_Leave(object sender, EventArgs e)
        {
            lueSatisTipi.Tag="0";
        }

        private void lueSatisTipi_Enter(object sender, EventArgs e)
        {
            lueSatisTipi.Tag="1";
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            gridControl3.DataSource = DB.GetData("select * from Satislar where fkSatisDurumu=1");
        }

        private void gridView4_RowClick(object sender, RowClickEventArgs e)
        {
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            SatisGetir(dr["pkSatislar"].ToString());
            SatisDetayGetir(dr["pkSatislar"].ToString());
        }
    }
}
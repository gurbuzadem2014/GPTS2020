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
using DevExpress.XtraReports.UI;
using GPTS.Include.Data;
using DevExpress.XtraTab;

namespace GPTS
{
    public partial class ucSiparis : DevExpress.XtraEditors.XtraUserControl
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis = "";//AccessibleDescription
        decimal HizliMiktar = 1;
        public ucSiparis()
        {
            InitializeComponent();
            DB.PkFirma = 1;
        }
        void PersonelGetir()
        {
            lUEPersonel.Properties.DataSource = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller where Plasiyer=1 and AyrilisTarihi is null");
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";
            lUEPersonel.ItemIndex = 0;
        }
        void Kasalar()
        {
            luekasa.Properties.DataSource = DB.GetData("select * from Kasalar where Aktif=1");
            //luekasa.EditValue = 1;
            luekasa.ItemIndex = 0;
        }
        void Bankalar()
        {
            DataTable dt = DB.GetData("select * from Bankalar where Aktif=1 order by tiklamaadedi desc");
            lueKKarti.Properties.DataSource = dt;
            //lueKKarti.ItemIndex = 0;
            if (dt.Rows[0]["tiklamaadedi"].ToString()!="0")
                lueKKarti.EditValue = int.Parse(dt.Rows[0]["PkBankalar"].ToString());
        }
        private void ucAnaEkran_Load(object sender, EventArgs e)
        {
            Fiyatlarigetir();
            HizliSatisTablariolustur();
            Yetkiler();
            FisListesi();
            GeciciMusteriDefault();
            //timer1.Enabled = true;
            lueSatisTipi.Properties.DataSource = DB.GetData(@"SELECT pkSatisDurumu, Durumu, Aktif, SiraNo
            FROM  SatisDurumu WHERE Aktif = 1 ORDER BY SiraNo");
            lueSatisTipi.EditValue = 10;
            PersonelGetir();
            TeslimTarihi.EditValue = DateTime.Now;
            Kasalar();
            Bankalar();
            cbTarihAraligi.SelectedIndex = 1;
            //Siparisler();
            if (gridView3.DataRowCount == 0)
            {
                cbTarihAraligi.SelectedIndex = 2;
                //Siparisler();
            }
            if (gridView3.DataRowCount == 0)
            {
                cbTarihAraligi.SelectedIndex = 3;
                //Siparisler();
            }
        }
        void Showmessage(string lmesaj, string renk)
        {
            frmMesajBox mesaj = new frmMesajBox(200);
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
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "ParaPanel")
                //    ParaPanel.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);

                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
                    gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
                    xtraTabControl2.Visible = true;
                //timer1.Enabled = true;
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "GizliAlan")
                //{
                //if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                //    dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                //else
                //        dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //}
            }
        }
        void Fiyatlarigetir()
        {
            lueFiyatlar.Properties.DataSource = DB.GetData("select * from SatisFiyatlariBaslik where Aktif=1 order by pkSatisFiyatlariBaslik");
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
            //Hizlibuttonlariyukle();
            //xtab.Controls.Add(uc); 
        }

        int x = 0;
        int y = 0;
        int p = 1;
        private void ButtonClick(object sender, EventArgs e)
        {
            //gridView1.AddNewRow();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            //gridView1.SetFocusedRowCellValue(gridColumn1, "1");
            //gridView1.SetFocusedRowCellValue(gridColumn1, "");

            if (((SimpleButton)sender).Tag != null)
            {
                SatisDetayEkle(((SimpleButton)sender).Tag.ToString());
                yesilisikyeni();
            }
            //urunekle(((SimpleButton)sender).Tag.ToString());
            //gridView1.Focus();
            //gridView1.AddNewRow();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            //gridView1.SetFocusedRowCellValue(gridColumn1, "1");
            //gridView1.SetFocusedRowCellValue(gridColumn1, "");
            //SendKeys.Send("{ENTER}");
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
                sb.ToolTip = "Satış Fiyatı=" + SatisFiyati + "\n Stok Adı:" + Stokadi;
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

                Satis1Toplam.Tag = 0;
                Satis1Toplam.Text = "0,0";
                pkSatisBarkod.Text = "0";

            TeslimTarihi.EditValue = null;
            temizle();
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }
            //DialogResult secimtum;
            //secimtum = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Siparişler İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secimtum == DialogResult.No)
            //{
            //    yesilisikyeni();
            //    return;
            //}

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Sipariş İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle

            string sonuc = "", pkSatislar = Satis1Toplam.Tag.ToString();

            //if (secimtum == DialogResult.No)
            //{
            //    string sql = "exec sp_SatisKopyala " + pkSatislar;
            //    DB.ExecuteSQL(sql);
            //}

            sonuc = DB.ExecuteScalarSQL("EXEC spSatisSil " + pkSatislar + ",1");
            if (sonuc != "Satis Silindi.")
                MessageBox.Show(sonuc);
            //SatisTemizle();
            Satis1Toplam.Tag = 0;
            Satis1Toplam.Text = "0,0";
            pkSatisBarkod.Text = "0";
            simpleButton2.Enabled = true;//müşteri ara 
            TeslimTarihi.EditValue = null;
            //temizle(AcikSatisindex);
            Siparisler();
            yesilisikyeni();
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
                Satis1Toplam.Tag = "0";
            }
            yesilisikyeni();
        }
        void YeniSatisEkle()
        {
            if (Satis1Toplam.Tag.ToString() == "0")
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
            iskonto.ShowDialog();
            yesilisikyeni();
            return;
        }
        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //ctrl + i iskonto
            if (e.Control && e.KeyValue == 222)
            {
                iskontoyagit_ctrli();
            }
            if (e.Control && e.Shift && gridView1.DataRowCount > 0)
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
        }
        void urunaraekle()
        {
            //YeniSatisEkle();
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "0";
            StokAra.ShowDialog();
            if (StokAra.TopMost == false)
            {
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
        //void StokSatisFiyati(string pkStokKarti)
        //{
        //    if (pkStokKarti == "") return;
        //    string satisfiyati = "0";
        //    //************ satiş fiyatı bul
        //    string sql = "";
        //    //Kampanya var mı
        //    DataTable dt=  DB.GetData("Select * from KampanyaStokFiyat  where fkStokKarti=" + pkStokKarti);
        //    if (dt.Rows.Count==0)
        //       dt = DB.GetData("Select * FROM UrunlerSatisFiyatlari where fkStokKarti=" + pkStokKarti);
        //    if (dt.Rows.Count == 0)
        //    {
        //        MessageBox.Show("Fiyat Bilgisi Bulunamadı.");
        //        return;
        //    }
        //    DataTable dtbaslik = DB.GetDatamdb("Select * FROM UrunlerSatisBaslik");
        //    DataTable dtFiyatlar = new DataTable();
        //    dtFiyatlar.Columns.Add(new DataColumn("FiyatAdi", typeof(string)));
        //    dtFiyatlar.Columns.Add(new DataColumn("Yuzde", typeof(Int32)));
        //    dtFiyatlar.Columns.Add(new DataColumn("Fiyat", typeof(decimal)));
        //    dtFiyatlar.Columns.Add(new DataColumn("pkid", typeof(int)));
        //    for (int i = 0; i < 6; i++)
        //    {
        //        DataRow dr;
        //        dr = dtFiyatlar.NewRow();
        //        if (i == 0)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde1"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat1"].ToString();
        //            satisfiyati = dt.Rows[0]["SatisFiyat1"].ToString();
        //        }
        //        else if (i == 1)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde2"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat2"].ToString();
        //        }
        //        else if (i == 2)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde3"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat3"].ToString();
        //        }
        //        else if (i == 3)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde4"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat4"].ToString();
        //        }
        //        else if (i == 4)
        //        {
        //            dr["FiyatAdi"] = dtbaslik.Rows[i]["Aciklama"].ToString();
        //            dr["Yuzde"] = dt.Rows[0]["SatisYuzde5"].ToString();
        //            dr["Fiyat"] = dt.Rows[0]["SatisFiyat5"].ToString();
        //        }
        //        dr["pkid"] = (i + 1).ToString();
        //        dtFiyatlar.Rows.Add(dr);
        //    }
        //    repositoryItemLookUpEdit1.Properties.DataSource = dtFiyatlar;
        //    dtFiyatlar.Dispose();

        //    if (satisfiyati == "") satisfiyati = "0";
        //    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SatisFiyati", satisfiyati);
        //    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "NakitFiyat", satisfiyati);
        //}
        private void urunekle(string barkod)
        {
            if (barkod == "") return;
            //string ilktarih = "";
            //string sontarih = "";
            //string fkUrunlerNoPromosyon = "";
            //int DususAdet = 1;
            DataTable dturunler = DB.GetData("select * From StokKarti WHERE Barcode='" + barkod + "'");
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
            return;
            //DususAdet = int.Parse(dturunler.Rows[0]["CikisAdet"].ToString());
            //string pkStokKarti = dturunler.Rows[0]["pkStokKarti"].ToString();
            //string pkStokKartiid = dturunler.Rows[0]["pkStokKartiid"].ToString();
            //StokSatisFiyati(pkStokKarti);
            ////    satisfiyati = dtsatisfiyati.Rows[0]["SatisFiyat"].ToString();
            ////    fkUrunlerNoPromosyon = dtsatisfiyati.Rows[0]["fkUrunlerNoPromosyon"].ToString();
            ////TODO: Promosyon yapılacak iskonto ile yap tarinde varsa ekle            
            //int sr = 0;
            //sr = gridView1.FocusedRowHandle;//DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            //if (dturunler.Rows[0]["pkStokKartiid"].ToString()!="")
            //gridView1.SetRowCellValue(sr, "pkStokKartiid", dturunler.Rows[0]["pkStokKartiid"].ToString());
            //gridView1.SetRowCellValue(sr, "Barcode", dturunler.Rows[0]["Barcode"].ToString());
            //gridView1.SetRowCellValue(sr, "Stokadi", dturunler.Rows[0]["Stokadi"].ToString());
            //string iade = "";
            //if (gridView1.GetFocusedRowCellValue("iade") == null)
            //    iade = "false";
            //else
            //    iade = gridView1.GetFocusedRowCellValue("iade").ToString();
            //if (iade == "True")
            //    gridView1.SetRowCellValue(sr, "iade", "true");
            //else
            //    gridView1.SetRowCellValue(sr, "iade", "false");

            // string miktar="1";
            //if(gridView1.GetFocusedRowCellValue("Adet")==null)
            //    miktar = "1";
            //else
            //{
            //    miktar = gridView1.GetFocusedRowCellValue("Adet").ToString();
            //    if (miktar == "") miktar = "1";
            //    if (iade == "True") miktar = (int.Parse(miktar) * -1).ToString(); 
            //}
            //if (miktar == "" || miktar == "1")
            //    gridView1.SetRowCellValue(sr, "Adet", "1");
            //else
            //    gridView1.SetRowCellValue(sr, "Adet", miktar);

            //gridView1.SetRowCellValue(sr, "pkid", gridView1.DataRowCount + 1);
            //gridView1.SetRowCellValue(sr, "pkStokKarti", dturunler.Rows[0]["pkStokKarti"].ToString());
            //gridView1.SetFocusedRowCellValue("Stoktipi", dturunler.Rows[0]["Stoktipi"].ToString());
            //gridView1.SetFocusedRowCellValue("AlisFiyati", dturunler.Rows[0]["AlisFiyati"].ToString());
            //gridView1.SetFocusedRowCellValue("iskonto", "0");
            //gridView1.SetFocusedRowCellValue("iskontotutar", "0");
            //gridView1.SetFocusedRowCellValue("KdvOrani", dturunler.Rows[0]["KdvOrani"].ToString());
            //gridView1.SetFocusedRowCellValue("iskontogercektutar", "0");
            //gridView1.SetFocusedRowCellValue("iskontogercekyuzde", "0");
            //gridView1.AddNewRow();
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
            SatisDetayGetir(Satis1Toplam.Tag.ToString());
            TutarFont(Satis1Toplam);
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();
            SatisGetir();
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
        private string OdemeSekli()
        {
            if(odenennakit.Value!= 0 && odenenkredikarti.Value == 0 && odenenacikhesap.Value==0)
                return "Nakit";
            else if(odenennakit.Value== 0 && odenenkredikarti.Value != 0 && odenenacikhesap.Value==0)
                return "Kredi Kartı";
            else if(odenennakit.Value== 0 && odenenkredikarti.Value == 0 && odenenacikhesap.Value!=0)
                return "Açık Hesap";
            else
            return "Diğer";
        }
        void satiskaydet(bool yazdir, bool odemekaydedildi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            //NAKİT
            list.Add(new SqlParameter("@Odenen", odenennakit.EditValue.ToString().Replace(",", ".")));
            //KREDİKARTI
            list.Add(new SqlParameter("@OdenenKrediKarti", odenenkredikarti.EditValue.ToString().Replace(",", ".")));
            if (lueKKarti.EditValue==null)
                list.Add(new SqlParameter("@fkBankalar", DBNull.Value));
            else
            list.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue.ToString()));
            //AÇIK HESAP
            list.Add(new SqlParameter("@AcikHesap", odenenacikhesap.EditValue.ToString().Replace(",", ".")));

            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@OdemeSekli", OdemeSekli()));
            list.Add(new SqlParameter("@fkPerTeslimEden", int.Parse(lUEPersonel.EditValue.ToString())));
            list.Add(new SqlParameter("@TeslimTarihi", TeslimTarihi.DateTime));

            string sonuc = DB.ExecuteSQL(@"UPDATE Satislar SET GuncellemeTarihi=GETDATE(),ToplamTutar=@ToplamTutar,
            fkSatisDurumu=@fkSatisDurumu,OdemeSekli=@OdemeSekli,fkPerTeslimEden=@fkPerTeslimEden,TeslimTarihi=@TeslimTarihi,
            Odenen=@Odenen,OdenenKrediKarti=@OdenenKrediKarti,AcikHesap=@AcikHesap,fkBankalar=@fkBankalar
             where pkSatislar=@pkSatislar", list);
            if (sonuc.Length > 1 && sonuc.Substring(1, 1) == "H")
            {
                Showmessage("Hata Oluştur: " + sonuc, "K");
                return;
            }
           // DB.ExecuteSQL("UPDATE Satislar SET Odenen=@Odenen,OdenenKrediKarti=@OdenenKrediKarti,AcikHesap=@AcikHesap where pkSatislar=" + pkSatisBarkod.Text);
            FisListesi();
        }

        void MevcutAdetGuncelle()
        {
            for (int s = 0; s < gridView1.DataRowCount; s++)
            {
                DataRow dr = gridView1.GetDataRow(s);
                DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=Mevcut-" + dr["Adet"].ToString() + " where pkStokKarti=" +
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
                Satis1Baslik.Text = DB.FirmaAdi;
                Satis1Firma.Tag = DB.PkFirma;
            }
        }
        void temizle()
        {
            DataTable dtMusteriler = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where GeciciMusteri=1");
            if (dtMusteriler.Rows.Count == 0)
                MessageBox.Show("Geçici Müşteri Bulunamadı.");
            else
            {
                DB.PkFirma = int.Parse(dtMusteriler.Rows[0]["pkFirma"].ToString());
                DB.FirmaAdi = dtMusteriler.Rows[0]["OzelKod"].ToString() + "-" + dtMusteriler.Rows[0]["Firmaadi"].ToString();
            }

            Satis1Baslik.Text = DB.FirmaAdi;
            Satis1Firma.Tag = DB.PkFirma;
            btnAciklamaGirisi.ToolTip = "";
            lueSatisTipi.EditValue = 10;
            ceiskontoTutar.EditValue = null;
            ceiskontoyuzde.EditValue = null;
            ceiskontoyuzde.Properties.NullText = "";
            gridControl1.DataSource = null;
            ceiskontoyuzde.Value = 0;
            ceiskontoTutar.Value = 0;
            lueFiyatlar.EditValue = 1;
            lUEPersonel.EditValue = null;
        }
        void satiskaydet(bool timer, bool yazdir, bool odemekaydedildi)
        {
            if (gridView1.DataRowCount == 0)
            {
                frmMesajBox mesaj = new frmMesajBox(200);
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
            if (DB.GetData("select * from Sirketler").Rows[0]["MusteriZorunluUyari"].ToString() == "True")
            {
                if (Satis1Firma.Tag.ToString() == "1")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Müşteri Olamaz.\n (Ayarlardan Kaldırabilirsiniz.)!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    musteriara();
                    return false;
                }
            }
            return true;

        }
        void kaydetyazdir(string btn_kaydet_yazdir)
        {
            if (bir_nolumusteriolamaz() == false) return;
            if (OnceSatisYapiniz() == false) return;
            if (btn_kaydet_yazdir == "kaydet")
                satiskaydet(true, false);
            else if (btn_kaydet_yazdir == "yazdir")
            {
                string  pkSatislar = Satis1Toplam.Tag.ToString();
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
                    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(21,10);
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
            yesilisikyeni();
        }
        private bool siparisdegidislikvarmi()
        {
            string fkSiparisSablonlari = "0";
            string fkGunler = TeslimTarihi.DateTime.DayOfWeek.ToString();
            if (fkGunler == "Friday") fkGunler = "6";
            if (fkGunler == "Saturday") fkGunler = "7";
            if (fkGunler == "Sunday") fkGunler = "1";
            if (fkGunler == "Monday") fkGunler = "2";
            if (fkGunler == "Tuesday") fkGunler = "3";
            if (fkGunler == "Wednesday") fkGunler = "4";
            if (fkGunler == "Thursday") fkGunler = "5";
            //int d = ((((int)TeslimTarihi.DateTime.DayOfWeek - 1) + 7) % 7) + 1; 
            DataTable dtg=
            DB.GetData(@"select fkSiparisSablonlari from MusteriZiyaretGunleri where fkGunler=" + fkGunler + " and fkFirma=" + Satis1Firma.Tag.ToString());
            if (dtg.Rows.Count == 0)
            {
                MessageBox.Show("Şablon Bulunamadı");
                return false;
            }
            else
            {
                fkSiparisSablonlari = dtg.Rows[0][0].ToString();

                if (fkSiparisSablonlari == "")
                {
                    MessageBox.Show("Şablon Bulunamadı");
                    return true;
                }
            //şablon karşılaştır
//            DataTable dt = DB.GetData(@"SELECT SiparisSablonDetay.fkStokKarti, SiparisSablonDetay.Adet
//FROM         SiparisSablonlari INNER JOIN
//                      SiparisSablonDetay ON SiparisSablonlari.pkSiparisSablonlari = SiparisSablonDetay.fkSiparisSablonlari
//WHERE SiparisSablonlari.pkSiparisSablonlari = " + fkSiparisSablonlari);
            //bool siparisdegisiklikvarmi = false;
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    DataRow dr = gridView1.GetDataRow(i);
                    if (DB.GetData("select * from SiparisSablonDetay where fkSiparisSablonlari=" + fkSiparisSablonlari + " and fkStokKarti=" + dr["fkStokKarti"].ToString()).Rows.Count == 0)
                        return true;
                }
                return false;
            }
        }
        private void simpleButton37_Click(object sender, EventArgs e)
        {
            if (lUEPersonel.EditValue == null)
            {
                DataTable dt =
                DB.GetData("SELECT fkPerTeslimEden FROM Firmalar WHERE pkFirma=" + Satis1Firma.Tag.ToString());
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Personel Seçiniz");
                    lUEPersonel.Focus();
                    return;
                }
                else
                {
                    if (dt.Rows[0][0].ToString() == "")
                        lUEPersonel.EditValue = 1;
                    else
                    lUEPersonel.EditValue = int.Parse(dt.Rows[0][0].ToString());
                }
            }

            if (TeslimTarihi.EditValue == null)
            {
                MessageBox.Show("Teslim Tarihi Seçiniz");
                TeslimTarihi.Focus();
                return;
            }

#region sipariş fiyat güncellemeye gerek kalmadı müşteri kendi güncellesin 
            //if (siparisdegidislikvarmi() == true)
            //{
            //    frmSiparisKaydetGuncelle SiparisKaydetGuncelle = new frmSiparisKaydetGuncelle();
            //    SiparisKaydetGuncelle.ShowDialog();
            //    if (SiparisKaydetGuncelle.cbTumSiparis.Tag == "0") return;
            //    bool TumSiparismi = SiparisKaydetGuncelle.cbTumSiparis.Checked;
            //    //tüm siparişleri etkile
            //}
            //if (DB.GetData("select count(*) from Satislar where Kaydedildi=0 and pkSatislar=" + pkSatisBarkod.Text).Rows[0][0].ToString() != "0")
            //{
            //    frmSiparisKaydetGuncelle SiparisKaydetGuncelle = new frmSiparisKaydetGuncelle();
            //    SiparisKaydetGuncelle.ShowDialog();
            //    if (SiparisKaydetGuncelle.cbTumSiparis.Tag == "0") return;
            //    bool TumSiparismi = SiparisKaydetGuncelle.cbTumSiparis.Checked;
            //}
            //DB.ExecuteSQL("update SatisDetay set Kaydedildi=1 where fkSatislar=" + pkSatisBarkod.Text);
#endregion

            if (odenennakit.Value == 0 && odenenkredikarti.Value == 0 && odenenacikhesap.Value==0 )
                odenennakit.Value = aratoplam.Value;
            kaydetyazdir("kaydet");
            simpleButton9.Visible = true;
            simpleButton2.Enabled = true;//müşteri ara 
            TeslimTarihi.EditValue = null;
            Siparisler();
            btnYeni_Click(sender, e);
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
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                //string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
                XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        void FisYazdir_eski(bool Disigner)
        {
            string fisid = pkSatisBarkod.Text;
            //if (lueFis.EditValue != null)
            // fisid = lueFis.EditValue.ToString();
            System.Data.DataSet ds = new DataSet("Test");
            DataTable FisDetay = DB.GetData(@"SELECT * from SatisDetay where fkSatislar=" + fisid);
            FisDetay.TableName = "FisDetay";
            ds.Tables.Add(FisDetay);
            DataTable Fis = DB.GetData(@"SELECT  * from Satislar where pkSatislar=" + fisid);
            Fis.TableName = "Fis";
            ds.Tables.Add(Fis);

            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
            XtraReport rapor = new XtraReport();
            rapor.DataSource = ds;
            rapor.LoadLayout(RaporDosyasi);
            if (Disigner)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
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
            string fkFirma = "1", ozelkod = "0", firmadi = "",fkPerTeslimEden="";// MusteriAra.fkFirma.AccessibleDescription;

            fkFirma = Satis1Firma.Tag.ToString();

            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.fkFirma.Tag = fkFirma;
            MusteriAra.ShowDialog();
            fkFirma = MusteriAra.fkFirma.Tag.ToString();
            MusteriAra.Dispose();

            if (pkSatisBarkod.Text != "0" || pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE Satislar SET fkFirma=" + fkFirma + " where pkSatislar=" + pkSatisBarkod.Text);
            }
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod,fkPerTeslimEden,fkSatisFiyatlariBaslik from Firmalar where pkFirma=" + fkFirma);
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            fkPerTeslimEden = dt.Rows[0]["fkPerTeslimEden"].ToString();
            if (fkPerTeslimEden == "")
                lUEPersonel.EditValue = null;
            else
               lUEPersonel.EditValue = int.Parse(fkPerTeslimEden);

            if (dt.Rows[0]["fkSatisFiyatlariBaslik"].ToString() == "")
                lueFiyatlar.EditValue = 1;
            else
                lueFiyatlar.EditValue = int.Parse(dt.Rows[0]["fkSatisFiyatlariBaslik"].ToString());

            Satis1Firma.Tag = fkFirma;
            Satis1Baslik.Text = ozelkod + "-" + firmadi;
            Satis1Baslik.ToolTip = Satis1Baslik.Text;
            yesilisikyeni();
            return fkFirma;
        }
        private string musteriata(string fkFirma)
        {
            string pkFirma = "1", ozelkod = "0", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where OzelKod=" + fkFirma);
            if (dt.Rows.Count == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Bulunamadı.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Satis1Baslik.Text = "";
                Satis1Baslik.Focus();
                return "1";
            }
            pkFirma = dt.Rows[0]["pkFirma"].ToString();
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            if (pkSatisBarkod.Text != "0" || pkSatisBarkod.Text != "")
            {
                DB.ExecuteSQL("UPDATE Satislar SET fkFirma=" + pkFirma + " where pkSatislar=" + pkSatisBarkod.Text);
            }

            Satis1Firma.Tag = pkFirma;
            Satis1Baslik.Text = ozelkod + "-" + firmadi;
            Satis1Baslik.ToolTip = Satis1Baslik.Text;
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
                gridView1.SetFocusedRowCellValue("iade", "true");
                gridView1.SetFocusedRowCellValue("Barcode", "");
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
            }
            //nakit
            if (e.KeyValue == 78)
            {
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                odenennakit.Value=aratoplam.Value;
                odenenkredikarti.Value = 0;
                odenenacikhesap.Value = 0;
                odenennakit.BackColor = System.Drawing.Color.Yellow;
                odenenkredikarti.BackColor = System.Drawing.Color.White;
                odenenacikhesap.BackColor = System.Drawing.Color.White;
                cbOdemeSekli.SelectedIndex = 1;
            }
            //kkartı
            else if (e.KeyValue == 75)
            {
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                odenennakit.Value = 0;
                odenenacikhesap.Value = 0;
                odenenkredikarti.Value = aratoplam.Value;
                odenenkredikarti.BackColor = System.Drawing.Color.Yellow;
                odenennakit.BackColor = System.Drawing.Color.White;
                odenenacikhesap.BackColor = System.Drawing.Color.White;
                cbOdemeSekli.SelectedIndex = 2;
            }
            //veresiye açık hesap
            else if (e.KeyValue == 65)
            {
                ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text = "";
                odenenacikhesap.Value = aratoplam.Value;
                odenennakit.Value = 0;
                odenenkredikarti.Value = 0;
                odenenacikhesap.BackColor = System.Drawing.Color.Yellow;
                odenennakit.BackColor = System.Drawing.Color.White;
                odenenkredikarti.BackColor = System.Drawing.Color.White;
                cbOdemeSekli.SelectedIndex = 3;
            }
        }
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorRed = new AppearanceDefault(Color.Red);
            //AppearanceDefault appErrorGreen = new AppearanceDefault(Color.GreenYellow);
            //AppearanceDefault appErrorYellowGreen = new AppearanceDefault(Color.YellowGreen);
            //AppearanceDefault appErrorPink = new AppearanceDefault(Color.LightSkyBlue);//, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            //object val = gridView1.GetRowCellValue(e.RowHandle, e.Column);
            //if ((e.Column.FieldName == "UnitPrice" && !(bool)validationControl1.IsTrueCondition(val)[0])
            //  || (e.Column.FieldName == "Quantity" && !(bool)validationControl2.IsTrueCondition(val)[0])
            //|| (e.Column.FieldName == "Discount" && !(bool)validationControl3.IsTrueCondition(val)[0]))

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                //yesilisikyeni();
                return;
            }
            if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString() != "" && dr["AlisFiyati"].ToString() != "")
            {
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                    AppearanceHelper.Apply(e.Appearance, appError);
            }
            if (e.Column.FieldName == "iskontotutar" && dr["iskontotutar"].ToString() != "0,000000")
            {
                AppearanceHelper.Apply(e.Appearance, appfont);
            }
            if (e.Column.FieldName == "Adet" && dr["Adet"].ToString() == "0")
            {
                AppearanceHelper.Apply(e.Appearance, appError);
            }
        }
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string iade = View.GetRowCellDisplayText(e.RowHandle, View.Columns["iade"]);
                string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
                e.Appearance.BackColor = Satis1Toplam.BackColor;
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
            if (gridView1.DataRowCount == 0) return;
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
           Satis1Toplam.Text = Convert.ToDouble(aratoplam.Value).ToString("##0.00");
           Satis1Toplam.ToolTip = Convert.ToDouble(aratoplam.Value).ToString();

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

            //10.03.2013 A.G.
            if (cbOdemeSekli.SelectedIndex == 0)
            {
                DB.ExecuteSQL("UPDATE Satislar SET OdenenKrediKarti=0,AcikHesap=0,Odenen=" + aratoplam.Text.ToString().Replace(",", ".") +
                    " where pkSatislar=" + pkSatisBarkod.Text);
            }
            else if (cbOdemeSekli.SelectedIndex == 1)
            {
                DB.ExecuteSQL("UPDATE Satislar SET AcikHesap=0,Odenen=0,OdenenKrediKarti=" + aratoplam.Text.ToString().Replace(",", ".") +
                    " where pkSatislar=" + pkSatisBarkod.Text);
            }
            else if (cbOdemeSekli.SelectedIndex == 2)
            {
                DB.ExecuteSQL("UPDATE Satislar SET Odenen=0,OdenenKrediKarti=0,AcikHesap=" + aratoplam.Text.ToString().Replace(",", ".") +
                    " where pkSatislar=" + pkSatisBarkod.Text);
            }
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
                catch //(Exception exp)
                {
                }

            }
            e.Handled = true;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //gridView1.Focus();
            //gridControl1.ForceInitialize();
            // gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            //gridView1.FocusedColumn = gridView1.Columns["UrunKodu"];
            //gridView1.ShowEditor();
            //SendKeys.Send("{BS}");
            if (((SimpleButton)sender).Tag != null)
                urunekle(((SimpleButton)sender).Tag.ToString());
            //gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            //gridView1.FocusedColumn = gridView1.Columns["UrunKodu"];
            //gridView1.ShowEditor();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //if (HizliBarkod == "") return;
            frmHizliButtonDuzenle HizliButtonDuzenle = new frmHizliButtonDuzenle();
            HizliButtonDuzenle.Top = HizliTop;
            HizliButtonDuzenle.Left = HizliLeft;
            HizliButtonDuzenle.oncekibarkod.Text = HizliBarkod;
            HizliButtonDuzenle.oncekibarkod.Tag = pkHizliStokSatis;
            HizliButtonDuzenle.ShowDialog();
            //string adi = HizliButtonDuzenle.barkod.ToolTip;
            //string barkodu = HizliButtonDuzenle.barkod.Text;
            //string pk = HizliButtonDuzenle.barkod.Tag.ToString();
            //HizliBarkodName = ((SimpleButton)sender).Name;
            //TabHizliSatisGenel.Controls.Add(sb)
            //for (int i = 0; i < TabHizliSatisGenel.Controls.Count; i++)
            //{
            //    if (TabHizliSatisGenel.Controls[i].Name == HizliBarkodName)
            //    {
            //        TabHizliSatisGenel.Controls[i].Text = adi;//HizliButtonDuzenle.stokadi.Text;
            //        TabHizliSatisGenel.Controls[i].Tag = barkodu;
            //    }
            //}
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

        //private void gridView1_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        //{
        //    if (e.Column.FieldName == "SatisFiyati")
        //        e.RepositoryItem = repositoryItemLookUpEdit1;
        //}

        void FisListesi()
        {
            string sql = @"select  Top 20 pkSatislar,f.Firmaadi,ToplamTutar,s.Tarih,s.OdemeSekli,k.KullaniciAdi,Sd.Durumu from Satislar s
LEFT JOIN Firmalar f ON s.fkFirma = f.PkFirma
LEFT JOIN Kullanicilar k ON s.fkKullanici=k.pkKullanicilar
LEFT JOIN SatisDurumu sd ON sd.pkSatisDurumu=s.fkSatisDurumu
where s.Siparis=0 and sd.pkSatisDurumu=@pkSatisDurumu
order by pkSatislar desc";
            if (lueSatisTipi.EditValue == null) lueSatisTipi.EditValue = 10;
            sql = sql.Replace("@pkSatisDurumu", lueSatisTipi.EditValue.ToString());
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
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(true);
            FisNoBilgisi.fisno.EditValue = lueFis.EditValue.ToString();
            FisNoBilgisi.ShowDialog();
            if (FisNoBilgisi.TopMost == true)
            {
                SatisGetir();
            }
            FisNoBilgisi.Dispose();
            FisListesi();
            lueFis.EditValue = null;
            yesilisikyeni();
        }
        void SatisGetir()
        {
            
            odenennakit.Value = 0;
            odenenkredikarti.Value = 0;
            odenenacikhesap.Value = 0;
            string pkSatislar = pkSatislar = Satis1Toplam.Tag.ToString();
            DataTable dtSatislar = DB.GetData("exec sp_Siparisler " + pkSatislar);//fiş bilgisi

            if (dtSatislar.Rows.Count == 0)
            {
                //Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            string fkfirma = dtSatislar.Rows[0]["fkFirma"].ToString();
            string firmaadi = dtSatislar.Rows[0]["Firmaadi"].ToString();
            string OdemeSekli = dtSatislar.Rows[0]["OdemeSekli"].ToString();
            cbOdemeSekli.Text = OdemeSekli;
            if (dtSatislar.Rows[0]["fkPerTeslimEden"].ToString() != "")
                lUEPersonel.EditValue = int.Parse(dtSatislar.Rows[0]["fkPerTeslimEden"].ToString());
            if (dtSatislar.Rows[0]["TeslimTarihi"].ToString() != "")
                TeslimTarihi.DateTime = DateTime.Parse(dtSatislar.Rows[0]["TeslimTarihi"].ToString());
            //
            if (dtSatislar.Rows[0]["Odenen"].ToString() == "")
                odenennakit.EditValue = 0;
            else
                odenennakit.EditValue = dtSatislar.Rows[0]["Odenen"].ToString();
            //banka
            if (dtSatislar.Rows[0]["OdenenKrediKarti"].ToString() == "")
                odenenkredikarti.EditValue = 0;
            else
                odenenkredikarti.EditValue = dtSatislar.Rows[0]["OdenenKrediKarti"].ToString();
            //hangi banka
            if (dtSatislar.Rows[0]["fkBankalar"].ToString() == "")
                lueKKarti.EditValue = null;
            else
                lueKKarti.EditValue = int.Parse(dtSatislar.Rows[0]["fkBankalar"].ToString());
            //açık hesap
            if (dtSatislar.Rows[0]["AcikHesap"].ToString() == "")
                odenenacikhesap.EditValue = 0;
            else
                odenenacikhesap.EditValue = dtSatislar.Rows[0]["AcikHesap"].ToString();
            
            btnAciklamaGirisi.ToolTip= dtSatislar.Rows[0]["Aciklama"].ToString();
            
            Satis1Firma.Tag = fkfirma;
            Satis1Firma.Text = fkfirma + "-" + firmaadi;
            Satis1Baslik.Text = fkfirma + "-" + firmaadi;
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

        void TutarFont(DevExpress.XtraEditors.LabelControl secilen)
        {
            odemepanel.BackColor = secilen.BackColor;
            gridView1.Appearance.Empty.BackColor = secilen.BackColor;
            gridView1.Appearance.Row.BackColor = secilen.BackColor;
            gridView1.Appearance.TopNewRow.BackColor = secilen.BackColor;
            gridView1.Appearance.HeaderPanel.BackColor = secilen.BackColor;
            iskontopanel.BackColor = secilen.BackColor;
        }

        void pkSatislarGetir(object sender, EventArgs e)
        {
            if (DB.pkSatislar > 0)
            {
                pkSatisBarkod.Text = DB.pkSatislar.ToString();
                Satis1Toplam.Tag = DB.pkSatislar;// lueFis.EditValue.ToString();
                Satis1Toplam_Click(sender, e);
                SatisGetir();
            }
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            //frmSiparisRaporlari Siparis = new frmSiparisRaporlari();
            //Siparis.ShowDialog();
            pkSatislarGetir(sender, e);
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

            //for (int i = 0; i < TabHizliSatisGenel.Controls[0].Controls.Count; i++)
            //{
            //    if (TabHizliSatisGenel.Controls[0].Controls[i].Name == HizliBarkodName)
            //        TabHizliSatisGenel.Controls[0].Controls[i].Text = ButtonAdiDegis.stokadi.Text;
            //}
            Hizlibuttonlariyukle();
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            string pkSatis = pkSatisBarkod.Text;
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

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            kaydetyazdir("yazdir");
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

        private void repositoryItemCalcEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string yenimiktar =
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
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
            FisListesi();
        }

        void SatisDigerBilgileriGetir()
        {
            //TODO:SatisDigerBilgileriGetir
            DataTable dt = DB.GetData("select pkSatislar,fkFirma from Satislar where pkSatislar=0");
            btnAciklamaGirisi.ToolTip = dt.Rows[0]["Aciklama"].ToString();
        }

        void SatisDetayGetir(string pkSatislar)
        {
            pkSatisBarkod.Text = pkSatislar;
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
            DataTable dtKalanAdet =
            DB.GetData("SELECT  pkAlisDetay,KalanAdet FROM AlisDetay WHERE fkStokKarti = " + pkStokKarti + " and KalanAdet>0 order by Tarih");

            decimal KalanAdet = 0;
            for (int i = 0; i < dtKalanAdet.Rows.Count; i++)
            {
                KalanAdet = decimal.Parse(dtKalanAdet.Rows[i]["KalanAdet"].ToString());
                if (KalanAdet > EklenenMiktar)
                {
                    DB.ExecuteSQL("UPDATE AlisDetay SET KalanAdet=KalanAdet -" + EklenenMiktar.ToString().Replace(",", ".") +
                        " where pkAlisDetay=" + dtKalanAdet.Rows[i]["pkAlisDetay"].ToString());
                    break;
                }
                else
                {
                    //seçilen alış faturasında yoksa ikinci alış faturasından düş
                    DB.ExecuteSQL("UPDATE AlisDetay SET KalanAdet=KalanAdet -" + EklenenMiktar.ToString().Replace(",", ".") +
                        " where pkAlisDetay=" + dtKalanAdet.Rows[i]["pkAlisDetay"].ToString());
                    EklenenMiktar = EklenenMiktar - KalanAdet;
                }
            }
            //yesilisikyeni();
        }

        void YeniSatis()
        {
            string sql = "";
            string fisno = "0";
            bool yazdir = false;
            ArrayList list = new ArrayList();
            string pkFirma = "1";
            pkFirma = Satis1Firma.Tag.ToString();
            list.Add(new SqlParameter("@fkFirma", pkFirma));
            list.Add(new SqlParameter("@Siparis", "0"));
            list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("@fkSatisDurumu", lueSatisTipi.EditValue.ToString()));
            list.Add(new SqlParameter("@Aciklama", btnAciklamaGirisi.ToolTip));
            list.Add(new SqlParameter("@AlinanPara", "0"));
            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("@Yazdir", yazdir));
            list.Add(new SqlParameter("@iskontoFaturaTutar", "0"));//iskontoTutar.Value.ToString().Replace(",", ".")));

            sql = "INSERT INTO Satislar (Tarih,fkFirma,Siparis,GelisNo,fkKullanici,fkSatisDurumu,Aciklama,AlinanPara,ToplamTutar,Yazdir,iskontoFaturaTutar,AcikHesap,AcikHesapOdenen,GuncellemeTarihi)" +
                " VALUES(getdate(),@fkFirma,@Siparis,1,@fkKullanici,@fkSatisDurumu,@Aciklama,@AlinanPara,@ToplamTutar,@Yazdir,@iskontoFaturaTutar,0,0,getdate()) SELECT IDENT_CURRENT('Satislar')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            if (Satis1Toplam.Tag.ToString() == "0")
            {
                Satis1Toplam.Tag = fisno;
                SatisDetayGetir(Satis1Toplam.Tag.ToString());
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Bilgileri ve Genel Durumu. Yapım Aşamasındadır...");
        }

        private void yazıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(21,0);
            string pkSatislar = "0";
            pkSatislar = Satis1Toplam.Tag.ToString();
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

        private void sonrakiSatisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yesilisikyeni();
            Application.DoEvents();
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
                decimal SatisFiyati = decimal.Parse(dr["SatisFiyati"].ToString());
                decimal iskonto = 0, iskontotutar = 0;
                decimal.TryParse(girilen, out iskontotutar);
                iskonto = SatisFiyati - iskontotutar;
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
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            SatisFiyatlari.ShowDialog();
            int i = int.Parse(lueFiyatlar.EditValue.ToString());
            Fiyatlarigetir();
            lueFiyatlar.EditValue = i;
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

        private void Satis1Toplam_Click(object sender, EventArgs e)
        {
            yesilisikyeni();
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

        void SatisDetayYenile()
        {
            SatisDetayGetir(Satis1Toplam.Tag.ToString());
            TutarFont(Satis1Toplam);
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
                ((DevExpress.XtraEditors.CalcEdit)((((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Value.ToString();
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

        private void müşteriKArtıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriKarti MusteriKarti = new frmMusteriKarti(Satis1Firma.Tag.ToString(), "");
            DB.PkFirma = int.Parse(Satis1Firma.Tag.ToString());
            MusteriKarti.ShowDialog();
            //özel kod veya adı için getir ve yenile
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + DB.PkFirma);
            string firmadi = dt.Rows[0]["Firmaadi"].ToString();
            string ozelkod = dt.Rows[0]["OzelKod"].ToString();
            Satis1Baslik.Text = ozelkod + "-" + firmadi;
            Satis1Baslik.ToolTip = Satis1Baslik.Text;
        }

        private void cmsPopYazir_Opening(object sender, CancelEventArgs e)
        {
            if (((((System.Windows.Forms.ContextMenuStrip)(sender)).SourceControl).Controls.Owner).Name == "btnyazdir")
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = true;
                müşteriSeçToolStripMenuItem.Visible = false;
                müşteriKArtıToolStripMenuItem.Visible = false;
            }
            else
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = false;
                müşteriSeçToolStripMenuItem.Visible = true;
                müşteriKArtıToolStripMenuItem.Visible = true;
            }
            müşteriSeçToolStripMenuItem.Enabled = simpleButton2.Enabled;
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
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("26");//10
            SayfaAyarlari.ShowDialog();
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            Hizlibuttonlariyukle();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            frmSatisOdeme SatisOdeme = new frmSatisOdeme(pkSatisBarkod.Text,false,0,true);
            SatisOdeme.Tag = "1";
            SatisOdeme.MusteriAdi.Tag = DB.PkFirma;
            SatisOdeme.ShowDialog();
            SatisOdeme.Dispose();
        }

        private void Satis1Baslik_DoubleClick_1(object sender, EventArgs e)
        {
            musteriara();
        }

        private void Satis1Baslik_Click(object sender, EventArgs e)
        {
            Satis1Baslik.Text = "";
            TutarFont(Satis1Toplam);
        }

        private void Satis1Baslik_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Satis1Baslik.Text == "")
                    musteriara();
                else
                    musteriata(Satis1Baslik.Text);
            }
        }

        private void Satis1Baslik_Leave(object sender, EventArgs e)
        {
            if (Satis1Baslik.Text == "")
                Satis1Baslik.EditValue = Satis1Baslik.OldEditValue;
        }

        private string SatisSablonKopyala(string fkGunler, string fkSablonGrup)
        {
            string SiparisSablonlari="0";

            DataTable dt =
            DB.GetData("select fkSiparisSablonlari,fkSiparisSablonlari1,fkSiparisSablonlari2,fkSiparisSablonlari3,fkPersoneller from MusteriZiyaretGunleri where fkGunler=" + fkGunler + " and fkFirma=" + Satis1Firma.Tag.ToString());
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Şablon Tanımlaması Bulunamadı.\n Lütfen Şablon Tanımlaması Yapınız");
                return "0";
            }
            else
            {
                if (fkSablonGrup=="1")
                    SiparisSablonlari = dt.Rows[0]["fkSiparisSablonlari"].ToString();
                else if (fkSablonGrup == "2")
                    SiparisSablonlari = dt.Rows[0]["fkSiparisSablonlari1"].ToString();
                else if (fkSablonGrup == "3")
                    SiparisSablonlari = dt.Rows[0]["fkSiparisSablonlari2"].ToString();
                else if (fkSablonGrup == "3")
                    SiparisSablonlari = dt.Rows[0]["fkSiparisSablonlari3"].ToString();
            }
            if (SiparisSablonlari == "") SiparisSablonlari = "0";
            DataTable dtSablon = DB.GetData("select fkFirma,fkPersoneller from SiparisSablonlari where pkSiparisSablonlari=" + SiparisSablonlari);
            if (dtSablon.Rows.Count == 0)
            {
                MessageBox.Show("Lütfen Şablon Tanımlayınız");
                return "0";
            }

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", dtSablon.Rows[0]["fkFirma"]));
            list.Add(new SqlParameter("@fkPerTeslimEden", lUEPersonel.EditValue.ToString()));//dtSablon.Rows[0]["fkPersoneller"].ToString()));
            list.Add(new SqlParameter("@TeslimTarihi", TeslimTarihi.DateTime.AddDays(7)));

            string pkSatislarYeniid = DB.ExecuteScalarSQL("INSERT INTO Satislar (Tarih,fkFirma,GelisNo,Siparis,fkKullanici,fkSatisDurumu,GuncellemeTarihi,fkPerTeslimEden,TeslimTarihi) " +
                " values(getdate(),@fkFirma,0,0,1,10,getdate(),@fkPerTeslimEden,@TeslimTarihi) SELECT IDENT_CURRENT('Satislar')", list);

            DataTable dtSablonDetay = DB.GetData("select * from SiparisSablonDetay where fkSiparisSablonlari=" + SiparisSablonlari);
            for (int i = 0; i < dtSablonDetay.Rows.Count; i++)
            {
                string fkStokKarti= dtSablonDetay.Rows[i]["fkStokKarti"].ToString();
                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@pkSatislarYeniid", pkSatislarYeniid));
                list2.Add(new SqlParameter("@fkStokKarti",fkStokKarti));
                list2.Add(new SqlParameter("@Adet", dtSablonDetay.Rows[i]["Adet"].ToString()));
                DataTable dtStokkarti = DB.GetData("select pkStokKarti,AlisFiyati,SatisFiyati,KdvOrani from StokKarti where pkStokKarti=" + fkStokKarti);
                string alisfiyati = "0", SatisFiyati = "0",KdvOrani = "0";
                alisfiyati =  dtStokkarti.Rows[0]["AlisFiyati"].ToString();
                SatisFiyati = dtStokkarti.Rows[0]["SatisFiyati"].ToString();
                KdvOrani = dtStokkarti.Rows[0]["KdvOrani"].ToString();
                
                list2.Add(new SqlParameter("@AlisFiyati", alisfiyati.Replace(",",".")));
                list2.Add(new SqlParameter("@SatisFiyati", SatisFiyati.Replace(",", ".")));
                list2.Add(new SqlParameter("@KdvOrani", KdvOrani));
                list2.Add(new SqlParameter("@NakitFiyat", SatisFiyati.Replace(",", ".")));
                DB.ExecuteSQL("INSERT INTO SatisDetay (fkSatislar,fkStokKarti,Adet,AlisFiyati,SatisFiyati,Tarih,Stogaisle,iskontotutar,iskontoyuzdetutar,KdvOrani,NakitFiyat,fkAlisDetay)" +
                " values(@pkSatislarYeniid,@fkStokKarti,@Adet,@AlisFiyati,@SatisFiyati,getdate(),0,0,0,@KdvOrani,@NakitFiyat,0)", list2);
            }
            return "1";
        }
        void SatisaCevir()
        {
            int say = 0;
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);
                if(dr["Sec"].ToString()=="True")
                {
                    string pkSatislar = dr["pkSatislar"].ToString();
                    int sonuc = DB.ExecuteSQL("UPDATE Satislar SET Siparis=1,fkSatisDurumu=2,GuncellemeTarihi=TeslimTarihi where pkSatislar=" + pkSatislar);
                    if (sonuc != 1)
                    {
                        Showmessage("Hata Oluştur: " + sonuc, "K");
                        continue;
                    }
                    //Toplam Tutar Güncelle
                    DB.ExecuteSQL(@"update Satislar Set ToplamTutar=SatisDetay.sf from
    (select fkSatislar,sum(SatisDetay.Adet*SatisDetay.SatisFiyati-iskontotutar) as sf from SatisDetay group by fkSatislar) as SatisDetay
    where Satislar.pkSatislar=SatisDetay.fkSatislar and Satislar.pkSatislar=" + pkSatislar);
                    //kasa hereketiekle    
                    SatisCevirKasaHareketleriEkle(pkSatislar);
                    say++;
                }
            }
           
            pkSatisBarkod.Text = "0";
            Satis1Toplam.Tag = "0";
            aratoplam.Value = 0;
            //timer1.Enabled = true;
            simpleButton9.Visible = true;
            simpleButton2.Enabled = true;//müşteri ara 
            Satis1Toplam.Text = "0,00";
            TeslimTarihi.EditValue = null;
            Siparisler();
            DevExpress.XtraEditors.XtraMessageBox.Show( say.ToString() + " Sipariş Satış Çevrildi", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            yesilisikyeni();
        }
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Siparişler Satış Çevrilecek Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            //frmSiparisKaydetGuncelle SiparisKaydetGuncelle = new frmSiparisKaydetGuncelle();
            //SiparisKaydetGuncelle.ShowDialog();
            //if (SiparisKaydetGuncelle.Tag == "0") return;
                SatisaCevir();
        }

        void SatisCevirKasaHareketleriEkle(string pkSatisid)
        {
             string  pkFirma = "0";
            //pkSatis = Satis1Toplam.Tag.ToString();
             DataTable dt = DB.GetData("select fkFirma,ToplamTutar,isnull(Odenen,0) as OdenenNakit,isnull(OdenenKrediKarti,0) as OdenenKrediKarti from Satislar with(nolock) where pkSatislar=" + pkSatisid);
            pkFirma = dt.Rows[0]["fkFirma"].ToString();//Satis1Firma.Tag.ToString();
            decimal fistutar = decimal.Parse(dt.Rows[0]["ToplamTutar"].ToString());
            decimal OdenenNakit = decimal.Parse(dt.Rows[0]["OdenenNakit"].ToString());
            decimal OdenenKrediKarti = decimal.Parse(dt.Rows[0]["OdenenKrediKarti"].ToString());
            
            //DB.ExecuteSQL("UPDATE Firmalar SET Alacak=Alacak+" + fistutar.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
            if (OdenenNakit != 0)
                {
                    //Nakit
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkFirma", pkFirma));
                    list.Add(new SqlParameter("@Borc", OdenenNakit.ToString().Replace(",", ".")));
                    list.Add(new SqlParameter("@Aciklama", "Nakit Ödeme"));
                    list.Add(new SqlParameter("@fkSatislar", pkSatisid));//pkSatis));
                    list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                    //personel
                    DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkKasalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar,BilgisayarAdi)" +
                        " values(0,1,@fkFirma,getdate(),4,1,@Borc,0,1,@Aciklama,'Nakit',@fkSatislar,@BilgisayarAdi)", list);
                    //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + OdenenNakit.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
                    //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
                }
                //KREDİ KARTI
            if (OdenenKrediKarti != 0)
                {
                    ArrayList list2 = new ArrayList();
                    list2.Add(new SqlParameter("@fkFirma", pkFirma));
                    list2.Add(new SqlParameter("@Borc", OdenenKrediKarti.ToString().Replace(",", ".")));
                    if (lueKKarti.EditValue==null)
                        list2.Add(new SqlParameter("@fkBankalar", "1"));
                    else
                        list2.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue));
                    list2.Add(new SqlParameter("@Aciklama", "K.Kartı Ödemesi"));
                    list2.Add(new SqlParameter("@fkSatislar", pkSatisid));//pkSatis));
                    list2.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                    DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar,BilgisayarAdi)" +
                        " values(@fkBankalar,@fkFirma,getdate(),8,1,@Borc,0,1,@Aciklama,'Kredi Kartı',@fkSatislar,@BilgisayarAdi)", list2);
                    //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + lueKKarti.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
                    //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
                }
               MevcutAdetGuncelle();
        }

        private void MusteriSipariGetir(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData(@"select * from Satislar
where fkSatisDurumu=10 and Convert(varchar(10),TeslimTarih,112)='" + TeslimTarihi.DateTime.ToString("yyyyMMdd") + "' and fkFirma=" + Satis1Firma.Tag.ToString());
            if (dt.Rows.Count > 0)
            {
                DB.pkSatislar = int.Parse(dt.Rows[0]["pkSatislar"].ToString());
                if (DB.pkSatislar > 0)
                {
                    pkSatisBarkod.Text = DB.pkSatislar.ToString();
                    Satis1Toplam.Tag = DB.pkSatislar;// lueFis.EditValue.ToString();
                    Satis1Toplam_Click(sender, e);
                    SatisGetir();
                }
            }
        }

        private void Gun4_Click(object sender, EventArgs e)
        {
            TeslimTarihi.DateTime = DateTime.Today;
            for (int i = 0; i < 7; i++)
            {
                if (TeslimTarihi.DateTime.DayOfWeek.ToString() == "Sunday")
                    TeslimTarihi.DateTime = TeslimTarihi.DateTime.AddDays(1);
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            frmSiparisSablonTanimlari SiparisSablonTanimlari = new frmSiparisSablonTanimlari();
            SiparisSablonTanimlari.Satis1Firma.Tag = Satis1Firma.Tag;
            SiparisSablonTanimlari.ShowDialog();
        }

        private void TeslimTarihi_Leave(object sender, EventArgs e)
        {
            if (TeslimTarihi.EditValue == null) return;
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@TeslimTarihi", TeslimTarihi.DateTime));
            list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
            DB.ExecuteSQL("UPDATE Satislar SET TeslimTarihi=@TeslimTarihi where pkSatislar=@pkSatislar", list);
        }

        private void TeslimTarihi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            frmAracTakip AracTakip = new frmAracTakip();
            AracTakip.ShowDialog();
        }
        private void gridView3_RowClick(object sender, RowClickEventArgs e)
        {
            if (gridView3.SelectedRowsCount > 1)
            {
                pkSatisBarkod.Text = "0";
                Satis1Toplam.Text = "";
                gridControl1.DataSource = null;
                //pkSatislarGetir(sender, e);
                temizle();
                //yesilisikyeni();
                return;
            }
            if (gridView3.FocusedRowHandle < 0) return;
            odenennakit.Text = "0";
            odenenkredikarti.Text = "0";
            odenenacikhesap.Text = "0";

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string pkSatislar = dr["pkSatislar"].ToString();
            string fkFirma = dr["fkFirma"].ToString();
            Satis1Firma.Tag = fkFirma;
            DB.pkSatislar = int.Parse(pkSatislar);
            //detay
            simpleButton37.Text = "Güncelle [F9]";
            if (dr["TeslimTarihi"].ToString() != "")
                TeslimTarihi.DateTime = Convert.ToDateTime(dr["TeslimTarihi"].ToString());
            if (dr["fkpersoneller"].ToString() != "")
                lUEPersonel.EditValue = int.Parse(dr["fkpersoneller"].ToString());

             simpleButton9.Visible = true;//SATIŞA ÇEVİR
             simpleButton2.Enabled = false;//müşteri ara 
             pkSatislarGetir(sender, e);

             if (dr["fkSatisFiyatlariBaslik"].ToString() != "")
                lueFiyatlar.EditValue = int.Parse(dr["fkSatisFiyatlariBaslik"].ToString());
             yesilisikyeni();
        }

        void AcikSiparislers()
        {
            string sql = "";
            sql = @"SELECT Convert(Bit,'0') as Sec,s.pkSatislar, s.Tarih, s.Aciklama, s.ToplamTutar, p.adi+' '+p.soyadi as AdiSoyadi, 
f.Firmaadi, f.Tel, f.Adres, s.fkFirma,
f.Borc, f.Alacak, s.GuncellemeTarihi,s.fkPerTeslimEden,datename(dw,s.TeslimTarihi) as Gun,s.TeslimTarihi
,k.KullaniciAdi
FROM  Satislar s
INNER JOIN Firmalar f ON s.fkFirma = f.pkFirma 
LEFT OUTER JOIN Personeller p ON s.fkPerTeslimEden = p.pkpersoneller
LEFT OUTER JOIN Kullanicilar k ON s.fkKullanici = k.pkKullanicilar
WHERE s.Siparis = 0 and fkSatisDurumu=10 ";
            sql = sql + " order by TeslimTarihi desc";
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm")));
            //list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm")));
            gridControl3.DataSource = DB.GetData(sql);//, list);
        }

        void olmayanlariekle()
        {
            if(Satis1Firma.Tag==null) return;
            string fkFirma=Satis1Firma.Tag.ToString();
            string fkPersoneller = "0";
            DataTable dtfirma = DB.GetData("select fkPerTeslimEden from  Firmalar where pkFirma=" + fkFirma);
            if (dtfirma.Rows.Count > 0)
            {
                if(dtfirma.Rows[0][0].ToString()!="")
                  fkPersoneller = dtfirma.Rows[0][0].ToString();
            }
            //pazartesi
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=1 and fkFirma="+fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(" + fkFirma + ",1,7," + fkPersoneller +")");
            //salı
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=2 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(" + fkFirma + ",2,7," + fkPersoneller + ")");
            //c
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=3 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(" + fkFirma + ",3,7," + fkPersoneller + ")");
            //p
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=4 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(" + fkFirma + ",4,7," + fkPersoneller + ")");
            //cuma
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=5 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(" + fkFirma + ",5,7," + fkPersoneller + ")");
            //ctes
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=6 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(" + fkFirma + ",6,7," + fkPersoneller + ")");
            //p
            if (DB.GetData("select * from  MusteriZiyaretGunleri where fkGunler=7 and fkFirma=" + fkFirma).Rows.Count == 0)
                DB.ExecuteSQL("INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) VALUES(" + fkFirma + ",7,7," + fkPersoneller + ")");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
        }

        //private void toolStripMenuItem5_Click(object sender, EventArgs e)
        //{
        //    //DialogResult secim;
        //    //secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Siparişler Satış Çevrilecek Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    //if (secim == DialogResult.No) return;

        //    //for (int i = 0; i < gridView3.DataRowCount; i++)
        //    //{
        //    //    DataRow dr = gridView3.GetDataRow(i);
        //    //    if (dr["Sec"].ToString() == "True")
        //    //    {
        //    //        string pkSatislar = dr["pkSatislar"].ToString();
        //    //        int sonuc = DB.ExecuteSQL("UPDATE Satislar Set Siparis=1,fkSatisDurumu=2,GuncellemeTarihi=GETDATE() where pkSatislar=" + pkSatislar);
        //    //        if (sonuc == 0)
        //    //        {
        //    //            DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //        }
        //    //        else
        //    //        {
        //    //            //DevExpress.XtraEditors.XtraMessageBox.Show("Sipariş Olarak Çevrildi.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    //            string sql = "exec sp_SatisKopyala " + pkSatislar;
        //    //            DB.ExecuteSQL(sql);
        //    //            SatisCevirKasaHareketleriEkle(pkSatislar);
        //    //        }
        //    //    }
        //    //}
        //    //Siparisler();
        //}

        private void seçilenleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilenleri Silmek istediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            int sonuc = 0, basarilisay = 0;
            bool hatavar = false;
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                DataRow dr = gridView3.GetDataRow(i);
                DB.pkSatislar = int.Parse(dr["pkSatislar"].ToString());
                if (dr["Sec"].ToString() == "True")
                {
                    sonuc = DB.ExecuteSQL("DELETE FROM SatisDetay where fkSatislar=" + DB.pkSatislar.ToString());
                    sonuc = DB.ExecuteSQL("DELETE FROM Satislar where pkSatislar=" + DB.pkSatislar.ToString());
                    if (sonuc == 0)
                        hatavar = true;
                    basarilisay++;
                }
            }
            if (hatavar)
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (basarilisay > 0)
                DevExpress.XtraEditors.XtraMessageBox.Show(basarilisay.ToString() + " Sipariş Silindi.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);

            Satis1Toplam.Tag = 0;
            Satis1Toplam.Text = "0,0";
            pkSatisBarkod.Text = "0";
            simpleButton2.Enabled = true;//müşteri ara 
            TeslimTarihi.EditValue = null;
           
            
            Satis1Firma.Tag = "1";
            Satis1Baslik.Text = "1"+ "-" + "firmadi";
            Satis1Baslik.ToolTip = Satis1Baslik.Text;
            Siparisler();
            yesilisikyeni();
            
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


            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih.DateTime = d2.AddSeconds(sec2);

        }

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTarihAraligi.SelectedIndex > 1)
            {
                ilktarih.Enabled = true;
                sontarih.Enabled = true;
            }
            else
            {
                ilktarih.Enabled = false;
                sontarih.Enabled = false;
            }
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";
            

            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 2)// Bu gün
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
            else if (cbTarihAraligi.SelectedIndex == 3)// Dün
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
            else if (cbTarihAraligi.SelectedIndex == 4)// yarın
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
            else if (cbTarihAraligi.SelectedIndex == 5)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, 0, 0, 0, false,
                                   0, 0, 0, 0, 0, 0, false);


            }
            else if (cbTarihAraligi.SelectedIndex == 6)// Bu hafta
            {

                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);

                //sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                //                    0, 0, 0, 0, 0, 0, false);

            }
            else if (cbTarihAraligi.SelectedIndex == 7)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi.SelectedIndex == 8)// önceki ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), -1, 0, 0, 0, 0, false,
                                 (-DateTime.Now.Day), 0, 0, 0, 0, 0, false);



            }
            else if (cbTarihAraligi.SelectedIndex == 9)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
                                  0, 0, 0, 0, 0, 0, false);




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
            else if (cbTarihAraligi.SelectedIndex == 9)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
            Siparisler();
        }

        private void ilktarih_Leave(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 10;
        }

        private void sontarih_Leave(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 10;
        }

        void Siparisler()
        {
            string sql = "";
            sql = @"SELECT Convert(Bit,'0') as Sec,s.pkSatislar, s.Tarih, s.Aciklama, s.ToplamTutar, p.adi+' '+p.soyadi as AdiSoyadi, 
f.Firmaadi, f.Tel, f.Adres, s.fkFirma,
f.Borc, f.Alacak, s.GuncellemeTarihi,p.pkpersoneller as fkpersoneller,datename(dw,s.TeslimTarihi) as Gun,s.TeslimTarihi
,k.KullaniciAdi,s.fkSatisFiyatlariBaslik
FROM  Satislar s
INNER JOIN Firmalar f ON s.fkFirma = f.pkFirma 
LEFT OUTER JOIN Personeller p ON s.fkPerTeslimEden = p.pkpersoneller
LEFT OUTER JOIN Kullanicilar k ON s.fkKullanici = k.pkKullanicilar
WHERE s.Siparis = 0 and s.fkSatisDurumu=10 ";
            if (cbTarihAraligi.SelectedIndex >1 )//tüm siparişler değilse
                sql = sql + "and s.TeslimTarihi Between @ilktar and @sontar";//sp_FisBazindaSatisRaporu @ilktar,@sontar";
            if (cbTarihAraligi.SelectedIndex == 1)//geçmiş siparişler
                sql = sql + "and convert(varchar(10),s.TeslimTarihi,112)<convert(varchar(10),getdate(),112)";
            sql = sql + " order by TeslimTarihi desc";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm")));
            gridControl3.DataSource = DB.GetData(sql, list);
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            Siparisler();
        }

        private void gridView3_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                DataRow dr = gridView3.GetDataRow(e.RowHandle);
                if (dr["Sec"].ToString() == "True")
                {
                    e.Appearance.BackColor = Color.SlateBlue;

                    e.Appearance.ForeColor = Color.White;

                }
            } 
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Ödeme Şekli Nakit Olarak Değiştirilecek Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            cbOdemeSekli.SelectedIndex = 0;
            odenennakit.Value = aratoplam.Value;
            odenenkredikarti.Value = 0;
            odenenacikhesap.Value = 0;
            odemelerikaydetyesil();
        }

        private void labelControl6_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Ödeme Şekli Kredi Kartı Olarak Değiştirilecek Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            cbOdemeSekli.SelectedIndex = 1;
            odenenkredikarti.Value = aratoplam.Value;
            lueKKarti.EditValue = 1;
            odenennakit.Value = 0;
            odenenacikhesap.Value = 0;
            odemelerikaydetyesil();
        }

        void odemelerikaydetyesil()
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Odenen", decimal.Parse(odenennakit.Text)));
            list.Add(new SqlParameter("@OdenenKrediKarti", decimal.Parse(odenenkredikarti.Text)));
            list.Add(new SqlParameter("@AcikHesap", decimal.Parse(odenenacikhesap.Text)));
            list.Add(new SqlParameter("@OdemeSekli", cbOdemeSekli.Text));
            list.Add(new SqlParameter("@pkSatislar", pkSatisBarkod.Text));
            string sql =@"UPDATE  Satislar SET Odenen=@Odenen ,OdenenKrediKarti=@OdenenKrediKarti
                    ,AcikHesap=@AcikHesap ,OdemeSekli=@OdemeSekli WHERE pkSatislar=@pkSatislar";
            DB.ExecuteSQL(sql,list);
            yesilisikyeni();
        }

        private void labelControl7_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Ödeme Şekli Açık Hesap Olarak Değiştirilecek Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            cbOdemeSekli.SelectedIndex = 2;
            odenenacikhesap.Value = aratoplam.Value;
            odenenkredikarti.Value = 0;
            odenennakit.Value = 0;
            odemelerikaydetyesil();
        }

        private void lueKKarti_Leave(object sender, EventArgs e)
        {
            if (lueKKarti.EditValue == null)
                Bankalar();
            else
                DB.ExecuteSQL("update Bankalar set tiklamaadedi=tiklamaadedi+1 where PkBankalar=" +lueKKarti.EditValue.ToString());
        }

        private void odenennakit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        private void odenenkredikarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                yesilisikyeni();
        }

        //private void odenennakit_Leave(object sender, EventArgs e)
        //{
        //    if (odenennakit.EditValue!=null)
        //    DB.ExecuteSQL("UPDATE  Satislar SET Odenen=" + odenennakit.Text.ToString().Replace(",", ".") +
        //        ",OdenenKrediKarti=" + odenenkredikarti.Text.ToString().Replace(",", ".") +
        //        ",AcikHesap=" + odenenacikhesap.Text.ToString().Replace(",", ".") +
        //        " WHERE pkSatislar=" + pkSatisBarkod.Text);
 
        //}

        //private void odenenkredikarti_Leave(object sender, EventArgs e)
        //{
        //    if (odenenkredikarti.EditValue != null)
        //        DB.ExecuteSQL("UPDATE  Satislar SET OdenenKrediKarti=" + odenenkredikarti.Text.ToString().Replace(",", ".") +
        //            ",Odenen=" + odenennakit.Text.ToString().Replace(",", ".") +
        //            ",AcikHesap=" + odenenacikhesap.Text.ToString().Replace(",", ".") +
        //            " WHERE pkSatislar=" + pkSatisBarkod.Text);
        //}

        private void cbSec_CheckedChanged(object sender, EventArgs e)
        {
            btnYeni_Click(sender, e);
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                //DataRow dr = gridView3.GetDataRow();
                gridView3.SetRowCellValue(i,"Sec",cbSec.Checked);
            }
        }

        private void odenennakit_ValueChanged(object sender, EventArgs e)
        {
            if (odenennakit.Value == 0)
            {
                odenennakit.BackColor = System.Drawing.Color.White;
                lcNakit.BackColor = System.Drawing.Color.White;
            }
            else
            {
                odenennakit.BackColor = System.Drawing.Color.Yellow;
                lcNakit.BackColor = System.Drawing.Color.Yellow;
            }
        }

        private void odenenkredikarti_ValueChanged(object sender, EventArgs e)
        {
            if (odenenkredikarti.Value == 0)
            {
                lcKrediKarti.BackColor = System.Drawing.Color.White;
                odenenkredikarti.BackColor = System.Drawing.Color.White;
            }
            else
            {
                lcKrediKarti.BackColor = System.Drawing.Color.Yellow;
                odenenkredikarti.BackColor = System.Drawing.Color.Yellow;
            }
        }

        private void odenenacikhesap_ValueChanged(object sender, EventArgs e)
        {
            if (odenenacikhesap.Value == 0)
            {
                lcAcikHesap.BackColor = System.Drawing.Color.White;
                odenenacikhesap.BackColor = System.Drawing.Color.White;
            }
            else
            {
                lcAcikHesap.BackColor = System.Drawing.Color.Yellow;
                odenenacikhesap.BackColor = System.Drawing.Color.Yellow;
            }
        }

        private void odenennakit_EditValueChanged(object sender, EventArgs e)
        {
            odenenacikhesap.EditValue = "0";
            decimal ara=0;
            decimal.TryParse(aratoplam.Text,out ara);
            decimal n = 0;
            decimal.TryParse(odenennakit.Text, out n);
            decimal k = 0;
            decimal.TryParse(odenenkredikarti.Text, out k);
            odenenacikhesap.EditValue = ara - n - k;
        }

        private void odenenkredikarti_EditValueChanged(object sender, EventArgs e)
        {
            odenenacikhesap.EditValue = "0";
            decimal ara = 0;
            decimal.TryParse(aratoplam.Text, out ara);
            decimal n = 0;
            decimal.TryParse(odenennakit.Text, out n);
            decimal k = 0;
            decimal.TryParse(odenenkredikarti.Text, out k);
            odenenacikhesap.EditValue = ara - n - k;
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            simpleButton37.Text = "Kaydet [F9]";
            pkSatisBarkod.Text = "0";
            Satis1Toplam.Tag = "0";
            aratoplam.EditValue = 0;
            Satis1Toplam.Text = "0,00";
            //timer1.Enabled = true;
            simpleButton9.Visible = false;
            simpleButton2.Enabled = true;//müşteri ara 
            TeslimTarihi.EditValue = DateTime.Now;
            odenennakit.Value = 0;
            odenenkredikarti.Value = 0;
            odenenacikhesap.Value = 0;
            odenenacikhesap.BackColor = System.Drawing.Color.White;
            odenennakit.BackColor = System.Drawing.Color.White;
            odenenkredikarti.BackColor = System.Drawing.Color.White;
            Bankalar();
            yesilisikyeni();
        }

        private void lUEPersonel_Enter(object sender, EventArgs e)
        {
            lUEPersonel.Tag = "1";
        }

        private void lUEPersonel_Leave(object sender, EventArgs e)
        {
            lUEPersonel.Tag = "0";
        }

        void OlmayanlariEkle()
        {
            string fkPersoneller = "0", fkGunler="0";
            string fkFirma = Satis1Firma.Tag.ToString();
            DataTable dt = DB.GetData("select * from MusteriZiyaretGunleri where fkFirma=" + fkFirma);
            if (dt.Rows.Count == 0)
            {
                dt = DB.GetData("select * from Firmalar where pkFirma=" + fkFirma);
                fkPersoneller = dt.Rows[0]["fkPerTeslimEden"].ToString();
            }
            else
            {
                fkPersoneller = dt.Rows[0]["fkPersoneller"].ToString();
            }
                for (int j = 0; j < 7; j++)
                {
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkFirma", fkFirma));
                    list.Add(new SqlParameter("@fkGunler", (j + 1).ToString()));
                    list.Add(new SqlParameter("@GunSonra", "7"));
                    if (fkPersoneller == "")
                        list.Add(new SqlParameter("@fkPersoneller", DBNull.Value));
                    else
                        list.Add(new SqlParameter("@fkPersoneller", fkPersoneller));
                    if (DB.GetData("select * from MusteriZiyaretGunleri where fkGunler=" + (j + 1).ToString() + " and fkFirma=" + fkFirma).Rows.Count == 0)
                    {
                        DB.ExecuteSQL(@"INSERT INTO MusteriZiyaretGunleri (fkFirma,fkGunler,GunSonra,fkPersoneller) 
  values(@fkFirma,@fkGunler,@GunSonra,@fkPersoneller)", list);
                    }
                }
        }

        private void siparişGünleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSiparisSablonlari SiparisSablonlari = new frmSiparisSablonlari();
            SiparisSablonlari.ShowDialog();
            yesilisikyeni();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            frmSiparisSablonlari SiparisSablonlari = new frmSiparisSablonlari();
            SiparisSablonlari.ShowDialog();
        }

        private void labelControl3_Click_1(object sender, EventArgs e)
        {
            string sql =@"SELECT     MusteriZiyaretGunleri.pkMusteriZiyaretGunleri, MusteriZiyaretGunleri.fkSablonGrup, MusteriZiyaretGunleri.fkGunler, MusteriZiyaretGunleri.GunSonra, 
                      MusteriZiyaretGunleri.fkPersoneller, MusteriZiyaretGunleri.fkSiparisSablonlari, MusteriZiyaretGunleri.fkSiparisSablonlari1, 
                      MusteriZiyaretGunleri.fkSiparisSablonlari2, MusteriZiyaretGunleri.fkSiparisSablonlari3, Gunler.GunAdi
FROM         MusteriZiyaretGunleri INNER JOIN
                      Gunler ON MusteriZiyaretGunleri.fkGunler = Gunler.pkGunler
WHERE     (MusteriZiyaretGunleri.fkFirma = @fkFirma) AND (MusteriZiyaretGunleri.VarYok = 1)
ORDER BY MusteriZiyaretGunleri.fkGunler";
            sql = sql.Replace("@fkFirma", Satis1Firma.Tag.ToString());
            lueSablonlar.Properties.DataSource = DB.GetData(sql);
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmSiparisOlustur SiparisOlustur = new frmSiparisOlustur();
            SiparisOlustur.ShowDialog();
        }

        private void gridView3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridView3.DataRowCount < 1000)
            {
                for (int i = 0; i < gridView3.DataRowCount; i++)
                {
                    gridView3.SetRowCellValue(i, "Sec", false);
                }
            }
            if (gridView3.SelectedRowsCount == 1)
            {
                DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
                if (dr["Sec"].ToString() == "True")
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "Sec", false);
                else
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "Sec", true);
                return;
            }
            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                int si = gridView3.GetSelectedRows()[i];
                DataRow dr = gridView3.GetDataRow(si);
                gridView3.SetRowCellValue(si, "Sec", true);
            }
        }

        void odemedegisti()
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Değişiklik Kaydedilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //cbOdemeSekli.SelectedIndex = 0;
            //odenennakit.Value = aratoplam.Value;
            //odenenkredikarti.Value = 0;
            //odenenacikhesap.Value = 0;
            odemelerikaydetyesil();
        }
        private void odenennakit_Leave(object sender, EventArgs e)
        {
            if (odenennakit.EditValue != odenennakit.OldEditValue)
                odemedegisti();
        }

        private void odenenkredikarti_Leave(object sender, EventArgs e)
        {
            if(odenenkredikarti.EditValue!=odenenkredikarti.OldEditValue)
               odemedegisti();
        }

        private void aratoplam_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void TeslimTarihi_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void tümünüSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cbSec.Checked)
                cbSec.Checked = false;
            else
            cbSec.Checked = true;
        }

        private void müşteriStokFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSiparisSablonFiyatlari SiparisSablonFiyatlari = new frmSiparisSablonFiyatlari();
            SiparisSablonFiyatlari.pkFirma.Text = Satis1Firma.Tag.ToString();
            SiparisSablonFiyatlari.ShowDialog();
        }

        private void müşteriBazlıSatışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSatisFiyatlariMusteriBazli SatisFiyatlariMusteriBazli = new frmSatisFiyatlariMusteriBazli();
            SatisFiyatlariMusteriBazli.ShowDialog();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridView3.FocusedRowHandle<0) return;
              DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();
        }
    }
}


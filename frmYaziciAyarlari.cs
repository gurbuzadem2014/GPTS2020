using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Drawing.Printing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraReports.UI;
using System.IO;
using GPTS.Include.Data;
using GPTS.islemler;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraReports.UserDesigner;

namespace GPTS
{
    public partial class frmYaziciAyarlari : DevExpress.XtraEditors.XtraForm
    {
        int form_id, pkSatisDurumu;
        public frmYaziciAyarlari(int formid,int fkSatisDurumu)
        {
            form_id = formid;
            pkSatisDurumu = fkSatisDurumu;
            InitializeComponent();
        }

        void YaziciListesi()
        {
            foreach (String yazicilar in PrinterSettings.InstalledPrinters)
            {
                repositoryItemComboBox1.Items.Add(yazicilar);
                repositoryItemComboBox2.Items.Add(yazicilar);
            }
            
            //PrintDocument pd = new PrintDocument();
            //String defaultPrinter = pd.PrinterSettings.PrinterName;
        }

        void EtiketSablonlari()
        {
            gridControl2.DataSource = DB.GetData(@"SELECT * FROM EtiketSablonlari with(nolock) ORDER BY Varsayilan desc");
        }

        void FisiSecimiGetir()
        {
            gridControl1.DataSource = DB.GetData(@"select * from SatisFisiSecimi fs with(nolock)  
                            left join SatisDurumu sd on sd.pkSatisDurumu=fs.fkSatisDurumu");
        }

        private void frmYaziciAyarlari_Load(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage1;

            //simpleButton1.Focus();

            YaziciListesi();

            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            xtraTabControl1.SelectedTabPageIndex=int.Parse(this.Tag.ToString());

            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                FisiSecimiGetir();
                BtnKaydet.Visible = true;
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 2)
            {
                BtnKaydet.Visible = true;
            }
            if (xtraTabControl1.SelectedTabPageIndex == 2)//Etiket Bas
                EtiketSablonlari();
            else
            {
                aktifyazicilarButonYap();

                flowLayoutPanel1.Focus();

                for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                {
                    flowLayoutPanel1.Controls[0].Focus();
                    //flowLayoutPanel1.Controls[0].GotFocus();
                    flowLayoutPanel1.Controls[0].Select();
                    //flowLayoutPanel1.Controls[0].SelectNextControl.Select();
                    break;
                }
                //((SimpleButton)sender).Focus();
                //flowLayoutPanel1.Focus();
                //flowLayoutPanel1.Container.Components
                //flowLayoutPanel1.Select();
                //simpleButton1.Focus();
                //return;
                //DataTable dt = DB.GetData("select * from SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" +
                //    pkSatisDurumu + " and (fkKullanicilar=0 or fkKullanicilar=" + DB.fkKullanicilar + 
                //    ") order by SiraNo");

                //if (dt.Rows.Count == 0)
                //{
                //    dt = DB.GetData("select * from SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" +
                //    pkSatisDurumu + " order by SiraNo desc");
                //}
                //if (this.Tag.ToString() == "0")
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        if (i == 0)
                //        {
                //            simpleButton1.Text = dt.Rows[i]["Aciklama"].ToString() + "\n \n \n (" + dt.Rows[i]["YaziciAdi"].ToString() + ")";
                //            if (dt.Rows[i]["Sec"].ToString() == "True")
                //                simpleButton1.Enabled = true;
                //            simpleButton1.Tag = dt.Rows[i]["Dosya"].ToString();
                //            simpleButton1.ToolTip = dt.Rows[i]["YaziciAdi"].ToString();
                //        }
                //        else if (i == 1)
                //        {
                //            simpleButton2.Text = dt.Rows[i]["Aciklama"].ToString() + "\n \n \n (" + dt.Rows[i]["YaziciAdi"].ToString() + ")";
                //            if (dt.Rows[i]["Sec"].ToString() == "True")
                //                simpleButton2.Enabled = true;
                //            simpleButton2.Tag = dt.Rows[i]["Dosya"].ToString();
                //            simpleButton2.ToolTip = dt.Rows[i]["YaziciAdi"].ToString();
                //        }
                //        else if (i == 2)
                //        {
                //            simpleButton3.Text = dt.Rows[i]["Aciklama"].ToString() + "\n \n \n (" + dt.Rows[i]["YaziciAdi"].ToString() + ")";
                //            if (dt.Rows[i]["Sec"].ToString() == "True")
                //                simpleButton3.Enabled = true;
                //            simpleButton3.Tag = dt.Rows[i]["Dosya"].ToString();
                //            simpleButton3.ToolTip = dt.Rows[i]["YaziciAdi"].ToString();
                //        }
                //    }
                //}
                //else
                //    FisiSecimiGetir();
                //simpleButton1.Focus();
            }
        }

        void aktifyazicilarButonYap()
        {
            DataTable dtbutton = DB.GetData(@"select * from SatisFisiSecimi with(nolock) 
where Sec=1 and 
fkSatisDurumu="+ pkSatisDurumu + " and (fkKullanicilar=0 or fkKullanicilar="+DB.fkKullanicilar+") order by SiraNo");

            int h = 120;//dockPanel1.Height / 7;
            int w = 170;//dockPanel1.Width / 5;
            try
            {


                for (int i = 0; i < dtbutton.Rows.Count; i++)
                {
                    string pkid = dtbutton.Rows[i]["pkSatisFisiSecimi"].ToString();
                    string YaziciAdi = dtbutton.Rows[i]["YaziciAdi"].ToString();
                    string Aciklama = dtbutton.Rows[i]["Aciklama"].ToString();
                    string Dosya = dtbutton.Rows[i]["Dosya"].ToString();
                    string YazdirmaAdedi = dtbutton.Rows[i]["YazdirmaAdedi"].ToString();
                    string onizle = dtbutton.Rows[i]["onizle"].ToString();
                    if (YazdirmaAdedi == "") YazdirmaAdedi = "1";

                    SimpleButton sb = new SimpleButton();
                    sb.AccessibleDescription = Dosya;//pkid;
                    sb.Name = pkid;//"Btn" + (i + 1).ToString();
                    sb.Text = Aciklama+ "\n\n Kopya Sayısı=" + YazdirmaAdedi+
                        "\n"+ YaziciAdi;
                    sb.Tag = YazdirmaAdedi;
                    sb.TabIndex = i;
                    sb.TabStop = true;
                    //double d = 0;
                    //double.TryParse(SatisFiyati, out d);
                    sb.ToolTip =  YaziciAdi;
                    sb.ToolTipTitle = "YaziciAdi: " + YaziciAdi;
                    sb.Height = h;
                    sb.Width = w;
                    if(onizle=="True")
                      sb.UseWaitCursor = true;
                    sb.Font = btnSablon.Font;
                    sb.Click += new EventHandler(ButtonClick);
                    sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseEnter);
                    //sb.Left = lef;
                    //sb.Top = to;
                    //adı 15 karakterden büyükse
                    //if (HizliSatisAdi.Length > 15)
                    //    sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    //else
                    //    sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                    sb.ContextMenuStrip = contextMenuStrip1;
                    //string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                   // string imagedosya = exeDiz + "\\HizliSatisButtonResim\\" + pkid + ".png";
                    //if (File.Exists(imagedosya))
                    //{
                    //    Image im = new Bitmap(imagedosya);
                    //    sb.Image = new Bitmap(im, 45, 45);
                    //    sb.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                    //}
                    //if (i != 0 && (i + 1) % 7 == 0)
                    //{
                    //    to = 0;
                    //    lef = lef + w;
                    //}
                    //else
                    //{
                    //    to += h;
                    //}

                    sb.Show();
                    //sb.SendToBack();
                    flowLayoutPanel1.Controls.Add(sb);
                   
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu: " + exp.Message);
                throw;
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            int ya = 1;
            int.TryParse(((SimpleButton)sender).Tag.ToString(),out ya);
            YaziciAdi.TabIndex = ya;
            Degerler.CopyAdetYazdirmaAdedi = short.Parse(ya.ToString());//CopyAdet.Value.ToString());
            //yazditma_adet = ya;
            if (seYazdirmaAdedi.Value > 0)
                Degerler.CopyAdetYazdirmaAdedi = short.Parse(seYazdirmaAdedi.Value.ToString());

            YaziciAdi.Text = ((SimpleButton)sender).AccessibleDescription.ToString();
            YaziciAdi.Tag = ((SimpleButton)sender).ToolTip.ToString();
            YaziciAdi.UseWaitCursor = ((SimpleButton)sender).UseWaitCursor;//önizle
            Close();

            //if (((SimpleButton)sender).Tag != null)
            //{
            //    MessageBox.Show(((SimpleButton)sender).Tag.ToString());
            //}
        }

        string HizliBarkod = "1", HizliTop="", HizliBarkodName="", pkHizliStokSatis="";
        private void sb_MouseEnter(object sender, EventArgs e)
        {
            HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            //HizliTop = ((SimpleButton)sender).Top;
            //HizliLeft = ((SimpleButton)sender).Left;
            HizliBarkodName = ((SimpleButton)sender).Name;
            pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;

            if (HizliBarkod == "") HizliBarkod = "1";

            //CopyAdet.Value = int.Parse(HizliBarkod);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Degerler.CopyAdetYazdirmaAdedi = short.Parse(CopyAdet.Value.ToString());
            
            //YaziciAdi.Text = simpleButton1.Tag.ToString(); //"satisfisirulo";//simpleButton1.Text;
            //YaziciAdi.Tag = simpleButton1.ToolTip; 
            //Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Degerler.CopyAdetYazdirmaAdedi = short.Parse(CopyAdet.Value.ToString());

            //YaziciAdi.Text = simpleButton2.Tag.ToString(); //"satisfisia5";//simpleButton2.Text;
            //YaziciAdi.Tag = simpleButton2.ToolTip; 
            //Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //Degerler.CopyAdetYazdirmaAdedi = short.Parse(CopyAdet.Value.ToString());

            //YaziciAdi.Text = simpleButton3.Tag.ToString(); //"satisfisia4";// simpleButton3.Text;
            //YaziciAdi.Tag = simpleButton3.ToolTip; 
            //Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            YaziciAdi.Text = "";
            Close();
        }

        private void frmYaziciAyarlari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                simpleButton21_Click(sender, e);
        }
        
        void SatisFisleriKaydet()
        {
            int sonuc = -1;
            DataTable dt = DB.GetData("select * from SatisFisiSecimi with(nolock)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                if (dr == null) continue;

                string sec = "0";
                if (dr["Sec"].ToString() == "True")
                    sec = "1";

                sonuc = DB.ExecuteSQL("UPDATE SatisFisiSecimi SET Sec=" + sec + ",YaziciAdi='" + dr["YaziciAdi"].ToString() +
                    "',Aciklama='" + dr["Aciklama"].ToString() +
                    "' where pkSatisFisiSecimi=" + dr["pkSatisFisiSecimi"].ToString());
            }
            if (sonuc == 1)
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.Text = "Bilgiler Kaydedildi";
                Mesaj.Show();
                Application.DoEvents();
                timer1.Enabled = true;
            }
            else
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.label1.Text = "Bilgiler Kaydedildi";
                Mesaj.Show();
                return;
            }
        }
        
        void FisEtiketKaydet()
        {
            int sonuc = -1;
            DataTable dt = DB.GetData("select * from EtiketSablonlari");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                string Aktif = "0", Varsayilan="0";
                if (dr["Aktif"].ToString() == "True")
                    Aktif = "1";
                if (dr["Varsayilan"].ToString() == "True")
                    Varsayilan = "1";
                sonuc = DB.ExecuteSQL("UPDATE EtiketSablonlari SET Aktif=" + Aktif + ",Varsayilan=" + Varsayilan + ",YaziciAdi='" + dr["YaziciAdi"].ToString() +
                    "' where pkEtiketSablonlari=" + dr["pkEtiketSablonlari"].ToString());
            }
            if (sonuc == 1)
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.Text = "Bilgiler Kaydedildi";
                Mesaj.Show();
                Application.DoEvents();
                timer1.Enabled = true;
            }
            else
            {
                frmMesajBox Mesaj = new frmMesajBox(200);
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.label1.Text = "Bilgiler Kaydedildi";
                Mesaj.Show();
                return;
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (this.Tag.ToString() == "2")
                FisEtiketKaydet();
            else
                SatisFisleriKaydet();
           
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            string FisNo="0", fkSatisFisiSecimi="0";
            if (DB.pkSatislar != 0)
                FisNo = DB.pkSatislar.ToString();

            //tasarla
            if (ghi.Column.Name == "gridColumn1")
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                fkSatisFisiSecimi = dr["pkSatisFisiSecimi"].ToString();
                FisYazdir(true, FisNo, dr["Dosya"].ToString(), fkSatisFisiSecimi);
            }
            //yazdır
            if (ghi.Column.Name == "gridColumn5")
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                fkSatisFisiSecimi = dr["pkSatisFisiSecimi"].ToString();
                FisYazdir(false, FisNo, dr["Dosya"].ToString(), fkSatisFisiSecimi);
            }
        }
        
        void FisYazdir(bool Disigner, string pkSatislar, string SatisFisTipi, string fkSatisFisiSecimi)
        {
            System.Data.DataSet ds = new DataSet("Test");
            DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + pkSatislar + ",1");
            FisDetay.TableName = "FisDetay";
            ds.Tables.Add(FisDetay);
            DataTable Fis = DB.GetData(@"exec sp_Satislar " + pkSatislar);
            if (Fis.Rows.Count == 0)
            {
                MessageBox.Show("Satış Bulunamadı");
                return;
            }
            string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
            Fis.TableName = "Fis";
            ds.Tables.Add(Fis);

            //şirket bilgileri
            DataTable Sirket = DB.GetData(@"select top 1 * from Sirketler with(nolock)");
            Sirket.TableName = "Sirket";
            ds.Tables.Add(Sirket);

            //Bakiye bilgileri
            DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli from KasaHareket with(nolock)
                    where fkSatislar=" + pkSatislar);
            Bakiye.TableName = "Bakiye";
            ds.Tables.Add(Bakiye);

            //Firma bilgileri
            //DataTable Musteri = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
            DataTable Musteri = DB.GetData("select * from VM_Musteriler where pkFirma=" + fkFirma);
            Musteri.TableName = "Musteri";
            ds.Tables.Add(Musteri);

            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

            string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
            //string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
            //XtraReport rapor = new XtraReport();
            xrCariHareket rapor = new xrCariHareket();
            //rapor.AfterPrint
            //rapor.Disposed
            //rapor.EndUpdate
            //rapor.IsDisposed
            //rapor.IsSingleChild

            //rapor.SaveComponents
            //rapor.SaveComponents += new
            //EventHandler<SaveComponentsEventArgs>(report_SaveComponents);
            rapor.DataSource = ds;

            if (File.Exists(RaporDosyasi))
            {
                rapor.LoadLayout(RaporDosyasi);
            }
            else
            {
                MemoryStream ms = RaporUtil.GetMemStr(fkSatisFisiSecimi, pkSatislar);
                if (ms == null)
                {
                    MessageBox.Show("Dosya Bulunamadı");
                }
                else
                    rapor.LoadLayout(ms);
            }

            rapor.Name = SatisFisTipi;
            rapor.Report.Name = SatisFisTipi;

            if (Disigner)
            {
                /*
                // Create a Design Tool and assign the report to it. 
                ReportDesignTool designTool = new ReportDesignTool(rapor);
                designTool.Report.DrawGrid = true;
                // Access the Designer form's properties. 
                designTool.DesignForm.SetWindowVisibility(DesignDockPanelType.FieldList |
                DesignDockPanelType.PropertyGrid, false);

                //designTool.DesignForm.OpenReport()
                //rapor.DesignerLoaded += report_DesignerLoaded;
                designTool.ShowDesignerDialog();
                 */
                rapor.ShowDesignerDialog();


                string filePath = @"XtraReport1.repx";

                rapor.SaveLayout(filePath);

                FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                long sayac = fStream.Length;
                BinaryReader bReader = new BinaryReader(fStream);
                byte[] byteResim = bReader.ReadBytes((int)sayac);
                string sql = "";
                ArrayList alist = new ArrayList();
                alist.Add(new SqlParameter("@pkSatisFisiSecimi", fkSatisFisiSecimi));
                alist.Add(new SqlParameter("@rapor_dosya", byteResim));

                sql = @"update SatisFisiSecimi set rapor_dosya=@rapor_dosya,guncelleme_tarihi=getdate() where pkSatisFisiSecimi=@pkSatisFisiSecimi";
                string sonuc = DB.ExecuteSQL(sql, alist);

            }
            else
                rapor.ShowPreview();
        }

        void report_DesignerLoaded(object sender, DevExpress.XtraReports.UserDesigner.DesignerLoadedEventArgs e)
        {
            // Access the Toolbox service. 
            System.Drawing.Design.IToolboxService toolboxService =
                (System.Drawing.Design.IToolboxService)e.DesignerHost.GetService(typeof(System.Drawing.Design.IToolboxService));

            // Add a custom control to the default category. 
            toolboxService.AddToolboxItem(new System.Drawing.Design.ToolboxItem(typeof(XRZipCode)));

            // Add a custom control to a new category. 
            // toolboxService.AddToolboxItem(new ToolboxItem(typeof(XRZipCode)), "New Category"); 
        }

        void report_SaveComponents(object sender, SaveComponentsEventArgs e)
        {
            int tableAdapterIdx = GetItemIndex(e.Components, typeof(Component));
            if (tableAdapterIdx >= 0)
                e.Components.RemoveAt(tableAdapterIdx);
            int dsIdx = GetItemIndex(e.Components, typeof(DataSet));
            if (dsIdx >= 0)
                e.Components.RemoveAt(dsIdx);
        }

        private static int GetItemIndex(IList components, Type targetType)
        {
            int idx = -1;
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].GetType().BaseType == targetType)
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            simpleButton1.Focus();
            timer2.Enabled = false;
        }

        private void simpleButton1_Enter(object sender, EventArgs e)
        {
            simpleButton1.Image = btnSablon.Image;
            simpleButton2.Image = null;
            simpleButton3.Image = null;
        }

        private void simpleButton2_Enter(object sender, EventArgs e)
        {
            simpleButton1.Image = null;
            simpleButton2.Image = btnSablon.Image; 
            simpleButton3.Image = null;

        }

        private void simpleButton3_Enter(object sender, EventArgs e)
        {
            simpleButton1.Image = null;
            simpleButton2.Image = null;
            simpleButton3.Image = btnSablon.Image; 
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridHitInfo ghi = gridView2.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            string FisNo = "0";
            if (DB.pkSatislar != 0)
                FisNo = DB.pkSatislar.ToString();

            if (ghi.Column.Name == "gridColumn6")
            {
                DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

                FisYazdir(true, FisNo, dr["DosyaYolu"].ToString(), dr["pkSatisFisiSecimi"].ToString());
            }
            if (ghi.Column.Name == "gridColumn10")
            {
                DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                FisYazdir(false, FisNo, dr["DosyaYolu"].ToString(), dr["pkSatisFisiSecimi"].ToString());
            }
        }

        private void repositoryItemCheckEdit3_CheckStateChanged(object sender, EventArgs e)
        {
            string sec = ((DevExpress.XtraEditors.CheckEdit)((((DevExpress.XtraEditors.CheckEdit)(sender)).Properties.OwnerEdit.Properties).OwnerEdit)).Checked.ToString();
            if (sec == "True")
                sec = "1";
            else
                sec = "0";

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            //DB.ExecuteSQL("UPDATE EtiketSablonlari SET Varsayilan=0");
            DB.ExecuteSQL("UPDATE EtiketSablonlari SET Aktif=1,Varsayilan=" +sec+ 
            " where pkEtiketSablonlari=" + dr["pkEtiketSablonlari"].ToString());

            EtiketSablonlari();
        }


        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                frmFisYaziciYeniRapor FisYaziciYeniRapor = new frmFisYaziciYeniRapor(form_id);
                FisYaziciYeniRapor.ShowDialog();
                FisiSecimiGetir();
            }
            if (xtraTabControl1.SelectedTabPageIndex == 2)
            {
                frmFisYaziciYeniRapor FisYaziciYeniEtiket = new frmFisYaziciYeniRapor(2);
                FisYaziciYeniEtiket.ShowDialog();
                EtiketSablonlari();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisYaziciYeniRapor FisYaziciYeniRapor = new frmFisYaziciYeniRapor(form_id);
            FisYaziciYeniRapor.pkSatisFisiSecimi.Text = dr["pkSatisFisiSecimi"].ToString();
            FisYaziciYeniRapor.ShowDialog();
            FisiSecimiGetir();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridView1.FocusedRowHandle<0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Aciklama"].ToString() + " Silinecek Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("delete from SatisFisiSecimi where pkSatisFisiSecimi=" + dr["pkSatisFisiSecimi"].ToString());

            FisiSecimiGetir();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmFisYaziciYeniRapor FisYaziciYeniRapor = new frmFisYaziciYeniRapor(2);
            FisYaziciYeniRapor.pkEtiketSablonlari.Text = dr["pkEtiketSablonlari"].ToString();
            FisYaziciYeniRapor.ShowDialog();

            EtiketSablonlari();
        }

        private void silToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["SablonAdi"].ToString() + " Silinecek Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("delete from EtiketSablonlari where pkEtiketSablonlari=" +
            dr["pkEtiketSablonlari"].ToString());
            
            EtiketSablonlari();
        }
    }
}
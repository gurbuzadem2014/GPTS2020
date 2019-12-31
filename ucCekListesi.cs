using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using System.Collections;
using System.Data.SqlClient;
using GPTS.islemler;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace GPTS
{
    public partial class ucCekListesi : DevExpress.XtraEditors.XtraUserControl
    {
        public ucCekListesi()
        {
            InitializeComponent();
        }
        void MenuOlustur()
        {
            DXPopupMenu dxPopupMenu = new DXPopupMenu();
            dxPopupMenu.Items.Add(new DXMenuItem("Firma Çeki", new EventHandler(OnItemsClick)));
            dxPopupMenu.Items.Add(new DXMenuItem("Müşteri Çeki", new EventHandler(OnItemsClick)));

            DXMenuItem newDXMenuItem = new DXMenuItem("Diğer...", new EventHandler(OnItemsClick));
            newDXMenuItem.BeginGroup = true;
            dxPopupMenu.Items.Add(newDXMenuItem);

            dropDownButton1.DropDownControl = dxPopupMenu;
        }

        private void OnItemsClick(object sender, EventArgs e)
        {
            DXMenuItem menuItem = (DXMenuItem)sender;
            string t="0";

            if (menuItem.Caption == "Firma Çeki")
                t = "0";
            else if(menuItem.Caption == "Müşteri Çeki")
                t = "1";
            else
                t = "2";

            frmCekGirisi CekGirisi = new frmCekGirisi(t);
            CekGirisi.ShowDialog();

            ceklistesi();

            //Debug.WriteLine(menuItem.Caption + " clicked");
        }

        private void ucCekListesi_Load(object sender, EventArgs e)
        {
            MenuOlustur();
            ceklistesi();

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimYukle(gridView1,"CeklerGrid");
            //string Dosya = DB.exeDizini + "\\CeklerGrid.xml";

            //if (File.Exists(Dosya))
            //{
            //    gridView1.RestoreLayoutFromXml(Dosya);
            //    gridView1.ActiveFilter.Clear();
            //}
        }

        void ceklistesi()
        {
            string sql = @"SELECT  c.pkCek, c.Vade, c.Tutar, c.fkParaBirimi, c.fkCekTuru, c.fkFirma, c.KayitTarihi,
                        c.PortfoyNo, c.SeriNo, c.fkBanka,c.fkTedarikci,
                       c.Aciklama,c.cek_sahibi, CekTurleri.CekTuru,f.Firmaadi,t.Firmaadi as Tedarikciadi,
                        c.VerildigiTarih,c.BankaAdi,c.MakbuzNo,c.EvrakTuru,c.Gosterme,c.fkSube
                      FROM  Cekler c with(nolock)
                      LEFT JOIN CekTurleri with(nolock) ON c.fkCekTuru = CekTurleri.pkCekTurleri
                      LEFT JOIN Firmalar f with(nolock) ON f.pkFirma= c.fkFirma
                      LEFT JOIN Tedarikciler t with(nolock) ON t.pkTedarikciler= c.fkTedarikci where 1=1";

            if (!ceKasadaki.Checked)
                sql = sql + " and isnull(c.Gosterme,0)=0";
                //sql = sql + " and (isnull(c.fkCekTuru,1)<>2 or c.EvrakTuru='2-Kendi Çekim')";

            if (cbGunuGecenler.Checked)
                sql = sql + " and c.Vade>=getdate()+1";

            sql = sql + " and isnull(c.fkSube,1)=" + Degerler.fkSube;

            //sql = sql + " order by c.Vade";

            gridControl1.DataSource = DB.GetData(sql);
        }
        private void ucBankaListesi_Load(object sender, EventArgs e)
        {
            ceklistesi();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmBankaHareketleri h = new frmBankaHareketleri();
            h.ShowDialog();
            ceklistesi();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmCekGirisi CekGirisi = new frmCekGirisi("0");
            CekGirisi.pkCek.Text = dr["pkCek"].ToString();
            CekGirisi.ShowDialog();
            ceklistesi();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show( dr["Firmaadi"].ToString() +  " Çek Silmek istediğinize eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                return;
            }
           
            DB.ExecuteSQL("Delete From Cekler where pkCek=" + dr["pkCek"].ToString());
            ceklistesi();
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = 0;
            KasaGirisi.Tag = "2";
            KasaGirisi.cbOdemeSekli.SelectedIndex = 2;
            KasaGirisi.ShowDialog();
            //frmCekGirisi CekGirisi = new frmCekGirisi("0");
            //CekGirisi.ShowDialog();

            ceklistesi();
        }


        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            ceklistesi();
        }

        void CekHareketleri()
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            gcCekHareketleri.DataSource = DB.GetData(@"select pkKasaHareket,Tarih,fkCek,kh.Borc,kh.Aciklama,fkSatislar,c.fkCekTuru,kh.fkFirma,
            kh.fkTedarikciler,f.Firmaadi as MusteriAdi,t.Firmaadi as TedarikciAdi
            from KasaHareket kh  with(nolock) 
            left join Cekler c  with(nolock)  on c.pkCek=kh.fkCek
            left join Firmalar f  with(nolock)  on f.pkFirma=kh.fkFirma
            left join Tedarikciler t  with(nolock)  on t.pkTedarikciler=kh.fkTedarikciler
            where fkCek=" + dr["pkCek"].ToString());
        }
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            CekHareketleri();
        }
        private void çekToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string okunma = View.GetRowCellDisplayText(e.RowHandle, View.Columns["EvrakTuru"]);
                if (okunma.Trim() == "1-Müşteri Çeki")
                {
                    e.Appearance.BackColor = Color.GreenYellow;
                    //e.Appearance.BackColor2 = Color.Blue;
                }
                //else if (okunma.Trim() == "1")
                //{
                //    e.Appearance.BackColor = Color.Blue;
                //    // e.Appearance.BackColor2 = Color.Red;
                //}
                //else if (okunma.Trim() == "2")
                //{
                //    e.Appearance.BackColor = Color.Aqua;
                //    // e.Appearance.BackColor2 = Color.Red;
                //}
                //else if (okunma.Trim() == "3")
                //{
                //    e.Appearance.BackColor = Color.Red;
                //    // e.Appearance.BackColor2 = Color.Red;
                //}
                else
                    e.Appearance.BackColor = Color.Aqua;
            }
        }

        private void portföydeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında");
            return;

            if (gridView1.FocusedRowHandle < 0) return;

            string secilenTag = ((System.Windows.Forms.ToolStripItem)(sender)).Tag.ToString();
            string secilentext = ((System.Windows.Forms.ToolStripItem)(sender)).Text;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            ArrayList list = new ArrayList();

            //nakit
            list.Add(new SqlParameter("@fkKasalar", DBNull.Value));
            //Banka
            list.Add(new SqlParameter("@fkBankalar", DBNull.Value));

            decimal borc = 0;
            decimal.TryParse(dr["Tutar"].ToString(), out borc);

            //eğer müşteriye iade
            if (dr["fkFirma"].ToString() != "0")
            {
                list.Add(new SqlParameter("@Borc", "0"));

                list.Add(new SqlParameter("@Alacak", borc.ToString().Replace(",", ".")));
            }
            //tedarikçiye iade ise
            else if (dr["fkFirma"].ToString() != "0")
            {
                list.Add(new SqlParameter("@Borc", borc.ToString().Replace(",", ".")));

                list.Add(new SqlParameter("@Alacak", "0"));
            }

            list.Add(new SqlParameter("@Tipi", "1"));

            list.Add(new SqlParameter("@Aciklama", "Çek İade Edildi"));
            list.Add(new SqlParameter("@donem", DateTime.Today.Month));
            list.Add(new SqlParameter("@yil", DateTime.Today.Year));

            //Müşteri Ödemesi ise
            if (dr["fkFirma"].ToString() != "0")
            {
                list.Add(new SqlParameter("@fkFirma", dr["fkFirma"].ToString()));
                list.Add(new SqlParameter("@fkTedarikciler", "0"));
            }
            else
            {
                //tedarikçi ödemesi ise
                list.Add(new SqlParameter("@fkFirma", "0"));
                list.Add(new SqlParameter("@fkTedarikciler", dr["fkTedarikciler"].ToString()));

            }

            //personel ödemesi ise
            list.Add(new SqlParameter("@fkPersoneller", "0"));

            list.Add(new SqlParameter("@AktifHesap", "1"));
            list.Add(new SqlParameter("@OdemeSekli", "Çek"));
            //Bakiye Girişi
            list.Add(new SqlParameter("@Tutar", "0"));
            //else
            //    list.Add(new SqlParameter("@Tutar", ceBakiye.Text.Replace(",", ".")));

            list.Add(new SqlParameter("@fkKasaGirisCikisTurleri", "0"));
            list.Add(new SqlParameter("@fkTuru", "1"));
            list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
            list.Add(new SqlParameter("@fkCek", dr["pkCek"].ToString()));

            string sql = @"INSERT INTO KasaHareket (fkKasalar,fkBankalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,fkKasaGirisCikisTurleri,fkTuru,BilgisayarAdi,fkCek)
             values(@fkKasalar,@fkBankalar,@fkPersoneller,getdate(),3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,@fkTedarikciler,@OdemeSekli,@Tutar,@fkKasaGirisCikisTurleri,@fkTuru,@BilgisayarAdi,@fkCek)";// SELECT IDENT_CURRENT('KasaHareket')"; 

            string sonuc = DB.ExecuteSQL(sql, list);

            if (sonuc.Substring(0, 1) != "H")
            {
                formislemleri.Mesajform(secilentext + " Hareket Eklenmiştir.", "S", 200);
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata : " + sonuc, DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void çekÇıkışıYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.pkKasaHareket = 0;
            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            //KasaCikis.pkTedarikci.Text = "1";
            KasaCikis.Tag = "2";
            KasaCikis.ShowDialog();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void dropDownButton1_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = 0;
            KasaGirisi.Tag = "1";
            KasaGirisi.cbOdemeSekli.SelectedIndex = 2;
            KasaGirisi.ShowDialog();
        }

        private void tamamlandıListedeGösterilmezToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["pkCek"].ToString() + " Tamamlandı Yapmak istediğinize eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.No)
            //{
            //    return;
            //}

            //DB.ExecuteSQL("UPDATE Cekler SET Gosterme=1 where pkCek=" + dr["pkCek"].ToString());
            //ceklistesi();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl1, "");
        }

        private void sütunGörünümToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimSil("CeklerGrid");
            //string Dosya = DB.exeDizini + "\\CeklerGrid.xml";

            //if (File.Exists(Dosya))
            //{
            //    File.Delete(Dosya);
            //}
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if (!formislemleri.SifreIste()) return;

             Yazdirma_islemleri yi = new Yazdirma_islemleri();
             yi.GridTasarimKaydet(gridView1, "CeklerGrid");

             //string Dosya = DB.exeDizini + "\\CeklerGrid.xml";
             //gridView1.SaveLayoutToXml(Dosya);
            
        }

        private void ödendiKendiÇekimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["pkCek"].ToString() + " Tamamlandı Yapmak istediğinize eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                return;
            }

            DB.ExecuteSQL("UPDATE Cekler SET CekTuru=11 where pkCek=" + dr["pkCek"].ToString());
            ceklistesi();
        }

        private void tedarikçiHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmTedarikcilerHareketleri TedarikciHareketi = new frmTedarikcilerHareketleri();
            TedarikciHareketi.musteriadi.Tag = dr["fkTedarikci"].ToString();
            TedarikciHareketi.Show();
        }
    }
}

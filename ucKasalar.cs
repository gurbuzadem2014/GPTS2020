using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using System.IO;
using GPTS.islemler;

namespace GPTS
{
    public partial class ucKasalar : DevExpress.XtraEditors.XtraUserControl
    {
        
        public ucKasalar()
        {
            InitializeComponent();
        }

        void KasaListesi()
        {
            string sql  = @"SELECT PkKasalar,kh.fkKasalar, k.KasaAdi,k.Aktif,  sum(kh.Borc) as Borc, sum(kh.Alacak) as Alacak,
            sum(isnull(kh.Borc,0)-isnull(kh.Alacak,0)) as Bakiye,k.fkSube FROM Kasalar k with(nolock) 
            LEFT JOIN KasaHareket kh with(nolock)  ON k.PkKasalar = kh.fkKasalar and  kh.fkKasalar is not null 
            Group by PkKasalar,kh.fkKasalar, k.KasaAdi,k.Aktif,k.fkSube";

            gcKasaListesi.DataSource = DB.GetData(sql);
        }
        private void ucKasalar_Load(object sender, EventArgs e)
        {
            KasaListesi();
            string Dosya = DB.exeDizini + "\\KasaListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.pkKasalar = int.Parse(dr["PkKasalar"].ToString());
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmKasaTanimlari KasaTanimlari = new frmKasaTanimlari(dr["PkKasalar"].ToString());
            KasaTanimlari.ShowDialog();

            KasaListesi();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            KasaListesi();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            frmKasaTanimlari KasaTanimlari = new frmKasaTanimlari("0");
            KasaTanimlari.ShowDialog();

            KasaListesi();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();

            KasaListesi();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmKasaHareketleri KasaHareketleri = new frmKasaHareketleri();
            KasaHareketleri.ShowDialog();
        }

        private void devirGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            DataRow dr = gridView1.GetDataRow(i);
            int fkKasalar = int.Parse(dr["PkKasalar"].ToString());
            frmKasaBakiyeDuzeltme KasaBakiyeDuzeltme = new frmKasaBakiyeDuzeltme(fkKasalar);
            //KasaBakiyeDuzeltme.pkKasalar.Text = dr["PkKasalar"].ToString();
            KasaBakiyeDuzeltme.ceKasadakiParaMevcut.Value = Decimal.Parse(dr["Bakiye"].ToString());
            KasaBakiyeDuzeltme.ShowDialog();

            KasaListesi();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string PkKasalar = dr["PkKasalar"].ToString();

            if (PkKasalar == "1")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("1 Nolu Kasa Silemez!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int aha = 0;

            aha = int.Parse(DB.GetData("select count(*) from KasaHareket with(nolock) where fkKasalar=" + PkKasalar).Rows[0][0].ToString());

            if (aha > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Kasa Kartı Hareket Gördüğü için Silemezsiniz! \n Kasa Durumunu Pasif Ürün Olarak Seçebilrsiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;


            if (aha == 0)
                DB.ExecuteSQL("DELETE FROM Kasalar where PkKasalar=" + PkKasalar);

            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = "Kas Kartı Bilgileri Silindi.";
            Mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
            Mesaj.Show();

            KasaListesi();
        }

        private void btnTrasnfer_Click(object sender, EventArgs e)
        {
            frmKasaTransferKarti trasn = new frmKasaTransferKarti();
            trasn.ShowDialog();

            KasaListesi();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string PkKasalar = dr["PkKasalar"].ToString();
            gcKullanicilar.DataSource = DB.GetData("select pkKullanicilar,KullaniciAdi from Kullanicilar with(nolock) where fkKasalar = " + PkKasalar);
        }

        private void btnKasaTrasn_Click(object sender, EventArgs e)
        {
            frmKasaTransfer trasn = new frmKasaTransfer();
            trasn.ShowDialog();

            KasaListesi();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmTransferRaporu vr = new frmTransferRaporu();
            vr.Show();

            KasaListesi();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\KasaListesiGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\KasaListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using GPTS.islemler;
using System.IO;

namespace GPTS
{
    public partial class ucBankaListesi : DevExpress.XtraEditors.XtraUserControl
    {
        public ucBankaListesi()
        {
            InitializeComponent();
        }
        void bankalarigetir()
        {
            string sql = @"SELECT B.pkBankalar, B.BankaAdi, B.TelefonNo,B.Aktif,B.Sube,B.fkSube,
isnull(sum(KH.Borc),0) as Borc, isnull(sum(KH.Alacak),0) as Alacak,isnull(sum(KH.Borc-KH.Alacak),0) as Bakiye,
B.KartTuru
FROM Bankalar B with(nolock)
LEFT JOIN KasaHareket KH with(nolock) ON B.PkBankalar = KH.fkBankalar
group by B.PkBankalar, B.BankaAdi, B.TelefonNo,B.Aktif,B.Sube,B.fkSube,B.KartTuru";

            gridControl1.DataSource = DB.GetData(sql);
        }
        private void ucBankaListesi_Load(object sender, EventArgs e)
        {
            bankalarigetir();

            string Dosya = DB.exeDizini + "\\BankaListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmBankaHareketleri h = new frmBankaHareketleri();
            h.ShowDialog();
            bankalarigetir();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmbankalar bankalar = new frmbankalar(0);
            bankalar.ShowDialog();

            bankalarigetir();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            int _banka_id= int.Parse(dr["pkBankalar"].ToString());

            frmbankalar bankalar = new frmbankalar(_banka_id);
            bankalar.ShowDialog();

            bankalarigetir();
        }

        private void devirGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(i);
            frmBankaBakiyeDuzeltme KasaBakiyeDuzeltme = new frmBankaBakiyeDuzeltme();
            KasaBakiyeDuzeltme.pkKasalar.Text = dr["pkBankalar"].ToString();
            KasaBakiyeDuzeltme.ceKasadakiParaMevcut.Value = Decimal.Parse(dr["Bakiye"].ToString());
            KasaBakiyeDuzeltme.ShowDialog();
            bankalarigetir();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
           
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            simpleButton2_Click(sender, e);
        }

        private void btnTrasnfer_Click(object sender, EventArgs e)
        {
            frmBankadanKasayaTransfer trans = new frmBankadanKasayaTransfer();
            trans.ShowDialog();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\BankaListesiGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\BankaListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkBankalar = dr["pkBankalar"].ToString();
            if (pkBankalar == "1")
            {
                formislemleri.Mesajform("1 Nolu Banka Silinemez!", "K", 200);
                return;
            }

            int aha = 0;

            aha = int.Parse(DB.GetData("select count(*) from KasaHareket with(nolock) where fkBankalar=" + pkBankalar).Rows[0][0].ToString());

            if (aha > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Banka Kartı Hareket Gördüğü için Silemezsiniz! \n Bankanın Durumunu Pasif Ürün Olarak Seçebilrsiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;


            if (aha == 0)
                DB.ExecuteSQL("DELETE FROM Bankalar where pkBankalar=" + pkBankalar);

            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = "Banka Kartı Bilgileri Silindi.";
            Mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
            Mesaj.Show();

            bankalarigetir();
        }
    }
}

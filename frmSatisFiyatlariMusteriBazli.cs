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
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmSatisFiyatlariMusteriBazli : DevExpress.XtraEditors.XtraForm
    {
        public frmSatisFiyatlariMusteriBazli()
        {
            InitializeComponent();
        }
        void StokListesi()
        {
            gridControl1.DataSource = DB.GetData(@"select sk.pkStokKarti,sk.Barcode,sk.Stokadi,sg.StokGrup,sk.SatisFiyati,sk.AlisFiyati,sag.StokAltGrup from StokKarti sk with(nolock)
left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
where sk.Aktif=1");
        }
        private void frmSatisFiyatlariMusteriBazli_Load(object sender, EventArgs e)
        {
            StokListesi();
        }
    
        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            SatisFiyatlariMusteriBazliGetir();
           // if (gridView1.FocusedRowHandle < 0) return;
           // DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
           // gridControl2.DataSource =
           // DB.GetData(@"select * from SatisFiyatlariMusteriBazli where fkStokKarti=" + dr["pkStokKarti"].ToString());
        }


        private string musteriara()
        {
            string fkFirma = "1", ozelkod = "0", firmadi = "";// MusteriAra.fkFirma.AccessibleDescription;

            frmMusteriAra MusteriAra = new frmMusteriAra();
            MusteriAra.fkFirma.Tag = fkFirma;
            MusteriAra.ShowDialog();

            fkFirma = MusteriAra.fkFirma.Tag.ToString();

            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + fkFirma);
            firmadi = dt.Rows[0]["Firmaadi"].ToString();
            ozelkod = dt.Rows[0]["OzelKod"].ToString();
            MusteriAra.Dispose();

            //Satis1Firma.Tag = fkFirma;
            //Satis1Baslik.Text = ozelkod + "-" + firmadi;
            //Satis1Baslik.ToolTip = Satis1Baslik.Text;
            return fkFirma;
        }
        private void btnMusteriBul_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                frmMesajBox mesaj = new frmMesajBox(200);
                mesaj.label1.Text = "Stok Seçiniz";
                mesaj.Show();
                return;
            }
            DataRow drs = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma = "0";
            frmMusteriAraSiparis MusteriAra = new frmMusteriAraSiparis();
            MusteriAra.checkEdit1.Checked = true;
            MusteriAra.ShowDialog();
            if (MusteriAra.Tag.ToString() == "0") return;
            for (int i = 0; i < MusteriAra.gridView1.DataRowCount; i++)
            {
                DataRow dr = MusteriAra.gridView1.GetDataRow(i);
                pkFirma = dr["pkFirma"].ToString();
                if (dr["Sec"].ToString() != "True") continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkStokKarti", drs["pkStokKarti"].ToString()));
                list.Add(new SqlParameter("@fkFirma", pkFirma));
                list.Add(new SqlParameter("@OzelSatisFiyati", drs["SatisFiyati"].ToString().Replace(",", ".")));
                list.Add(new SqlParameter("@YeniSatisFiyati", drs["SatisFiyati"].ToString().Replace(",", ".")));
                DB.ExecuteSQL(@"INSERT INTO SatisFiyatlariMusteriBazli (fkStokKarti,fkFirma,OzelSatisFiyati,YeniSatisFiyati,Aktif) 
            VALUES(@fkStokKarti,@fkFirma,@OzelSatisFiyati,@YeniSatisFiyati,1)", list);
                //list.Add(new SqlParameter("@fkpersoneller", lUEPersonel.EditValue.ToString()));
                //list.Add(new SqlParameter("@fkFirma", pkFirma));
                //DB.ExecuteSQL("UPDATE Firmalar SET fkPerTeslimEden=@fkpersoneller where pkFirma=@fkFirma", list);
                //DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET  Aktif=1 where fkFirma=" + pkFirma);
                //OlmayanlariEkle(pkFirma);
            }

            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //string fkFirma = musteriara();
            //ArrayList list = new ArrayList();
//            list.Add(new SqlParameter("@fkStokKarti", dr["pkStokKarti"].ToString()));
//            list.Add(new SqlParameter("@fkFirma", fkFirma));
//            list.Add(new SqlParameter("@OzelSatisFiyati", dr["SatisFiyati"].ToString().Replace(",", ".")));
//            list.Add(new SqlParameter("@YeniSatisFiyati", dr["SatisFiyati"].ToString().Replace(",", ".")));
//            DB.ExecuteSQL(@"INSERT INTO SatisFiyatlariMusteriBazli (fkStokKarti,fkFirma,OzelSatisFiyati,YeniSatisFiyati,Aktif) 
//            VALUES(@fkStokKarti,@fkFirma,@OzelSatisFiyati,@YeniSatisFiyati,1)", list);
            SatisFiyatlariMusteriBazliGetir();
        }
        void SatisFiyatlariMusteriBazliGetir()
        {
         if (gridView1.FocusedRowHandle < 0) return;
             DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
             gridControl2.DataSource = DB.GetData(@"select ROW_NUMBER() OVER (ORDER BY f.pkFirma) SiraNo,
Convert(bit,'1') as Sec,f.pkFirma,f.OzelKod,f.Firmaadi,sfmb.OzelSatisFiyati,sfmb.YeniSatisFiyati,sk.SatisFiyati,
sk.AlisFiyati,sfmb.Aktif,sfmb.fkStokKarti,sfmb.fkFirma,fg.GrupAdi,sk.AlisFiyati 
from SatisFiyatlariMusteriBazli sfmb with(nolock)
left join Firmalar f with(nolock) on f.pkFirma=sfmb.fkFirma
left join FirmaGruplari fg with(nolock) on f.fkFirmaGruplari=fg.pkFirmaGruplari
left join StokKarti sk with(nolock) on sk.pkStokKarti=sfmb.fkStokKarti
where f.Aktif=1 and sfmb.Aktif=1 and sfmb.fkStokKarti=" + dr["pkStokKarti"].ToString());
            // cedtYeniFiyat.EditValue = dr["SatisFiyati"].ToString();
        }

        private void btnMusteriCikar_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Özel Satış Fiyatlarını Silmek istediğinize Eminmisiniz mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.ExecuteSQL(@"delete from SatisFiyatlariMusteriBazli 
            where fkStokKarti="+dr["fkStokKarti"].ToString()+" and fkFirma=" +
            dr["fkFirma"].ToString());
            SatisFiyatlariMusteriBazliGetir();
        }

        private void repositoryItemCalcEdit1_Leave(object sender, EventArgs e)
        {
            string girilen =
((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).Text;

            fiyatguncelle(girilen);
        }

        private void repositoryItemCalcEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string girilen =
        ((DevExpress.XtraEditors.CalcEdit)(((DevExpress.XtraEditors.CalcEdit)(sender)).Properties.OwnerEdit)).Text;

                fiyatguncelle(girilen);
            }
        }
        void fiyatguncelle(string girilen)
        {
            if (girilen == "") return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            decimal GirilenSatisFiyatiKdvli = 0;
            decimal.TryParse(girilen, out GirilenSatisFiyatiKdvli);
            gridView2.SetFocusedRowCellValue("YeniSatisFiyati", GirilenSatisFiyatiKdvli);
            //satış fiyatlarını yeni fiyat alanında tut
            DB.ExecuteSQL("UPDATE SatisFiyatlariMusteriBazli SET OzelSatisFiyati="
            + GirilenSatisFiyatiKdvli.ToString().Replace(",", ".") +
            " where fkStokKarti=" + dr["fkStokKarti"].ToString()+
            " and fkFirma=" + dr["fkFirma"].ToString());
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);

                DB.ExecuteSQL("UPDATE SatisFiyatlariMusteriBazli SET OzelSatisFiyati=" +//YeniSatisFiyati" +
                    cedtYeniFiyat.Value.ToString().Replace(",",".") + 
                    " WHERE fkStokKarti=" + dr["fkStokKarti"].ToString() + " AND fkFirma=" +
                    dr["fkFirma"].ToString());

            }
            SatisFiyatlariMusteriBazliGetir();
           // for (int i = 0; i < gridView2.DataRowCount; i++)
           // {
           //     DataRow dr = gridView2.GetDataRow(i);

           //     DB.ExecuteSQL("UPDATE SatisFiyatlariMusteriBazli SET YeniSatisFiyati=" +
           //         cedtYeniFiyat.Value.ToString().Replace(",",".") +
           //         " WHERE fkStokKarti=" + dr["fkStokKarti"].ToString() + " AND fkFirma=" +
           //         dr["fkFirma"].ToString());

           // }
           //SatisFiyatlariMusteriBazliGetir();
        }

        private void tümMüşteriSatisFiyatlarınıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Müşteri Özel Satış Fiyatlarını Silmek istediğinize Eminmisiniz mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("truncate table SatisFiyatlariMusteriBazli");
        }

        private void frmSatisFiyatlariMusteriBazli_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.F4)
               btnMusteriBul_Click(sender,e);
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.F9)
                btnKaydet2_Click(sender, e);
        }

        private void tümünüÇıkarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Müşterileri çıkarmak istediğinize Eminmisiniz mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                DB.ExecuteSQL("Delete From SatisFiyatlariMusteriBazli where fkFirma="+dr["fkFirma"].ToString()
                    +" and fkStokKarti=" + dr["fkStokKarti"].ToString()); 
            }
            SatisFiyatlariMusteriBazliGetir();
        }

        private void btnKaydet2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);

                DB.ExecuteSQL("UPDATE SatisFiyatlariMusteriBazli SET OzelSatisFiyati=OzelSatisFiyati" +
                    " WHERE fkStokKarti=" + dr["fkStokKarti"].ToString() + " AND fkFirma=" +
                    dr["fkFirma"].ToString());

            }
            SatisFiyatlariMusteriBazliGetir();
        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            int i = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
            StokListesi();
            gridView1.FocusedRowHandle=i;
        }

        private void müşteriKartıDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            int i = gridView2.FocusedRowHandle;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
            SatisFiyatlariMusteriBazliGetir();
            gridView2.FocusedRowHandle=i;
            //gridView1.GetSelectedRows();
            gridView2.SelectRow(i);
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
        void yazdir(GridControl gc)
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gc;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            yazdir(gridControl2);
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            yazdir(gridControl1);
        }

        private void müşteriyiPasifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            int i = gridView2.FocusedRowHandle;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string sonuc=DBislemleri.MusteriPasifYap(dr["pkFirma"].ToString());
            string renk="S";
            if(sonuc=="işlem başarılı.") renk="S"; else renk="K";

            formislemleri.Mesajform(sonuc, renk, 200);

            SatisFiyatlariMusteriBazliGetir();
            gridView2.FocusedRowHandle = i;
        }
    }
}
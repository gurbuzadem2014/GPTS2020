using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;

namespace GPTS
{
    public partial class frmSiparisSablonlari : DevExpress.XtraEditors.XtraForm
    {
        public frmSiparisSablonlari()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmSiparisSablonlari_Load(object sender, EventArgs e)
        {
            PersonelGetir();
            PersoneleBagliMusteriListesiGetir();
        }
        void PersonelGetir()
        {
            DataTable dtp = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller where Plasiyer=1 and AyrilisTarihi is null");
            lUEPersonel.Properties.DataSource = dtp;
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";
            lUEPersonel.EditValue = 1;
        }
        void PersoneleBagliMusteriListesiGetir()
        {
//            string sql = @"SELECT convert(bit,'0') as Sec,pkFirma,fkPerTeslimEden,  Firmalar.Firmaadi, Firmalar.Tel, 
//Firmalar.Tel2 as Cep,Firmalar.Adres, Firmalar.Borc, Firmalar.Alacak, Firmalar.OzelKod,
//CASE when fkGunler=1 and VarYok=1 then 1 else 0 end Pzt, 
//CASE when fkGunler=2 and VarYok=1 then 1 else 0 end Sali, 
//CASE when fkGunler=3 and VarYok=1 then 1 else 0 end Car, 
//CASE when fkGunler=4 and VarYok=1 then 1 else 0 end Per, 
//CASE when fkGunler=5 and VarYok=1 then 1 else 0 end Cuma, 
//CASE when fkGunler=6 and VarYok=1 then 1 else 0 end Ctesi, 
//CASE when fkGunler=7 and VarYok=1 then 1 else 0 end Pzar
//FROM  Firmalar 
//LEFT OUTER JOIN  MusteriZiyaretGunleri ON Firmalar.pkFirma = MusteriZiyaretGunleri.fkFirma";
            string sql = @"SELECT convert(bit,'0') as Sec,pkFirma,fkPerTeslimEden,  Firmaadi, Tel, 
            Tel2 as Cep,Adres, Borc, Alacak, OzelKod  FROM  Firmalar";
            if (lUEPersonel.EditValue != null)
                sql = sql + " where fkPerTeslimEden=" + lUEPersonel.EditValue.ToString();
            gridControl2.DataSource = DB.GetData(sql);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            PersoneleBagliMusteriListesiGetir();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string pkFirma = "0";
            frmMusteriAraSiparis MusteriAra = new frmMusteriAraSiparis();
            MusteriAra.ShowDialog();
            if (MusteriAra.Tag.ToString() == "0") return;
            for (int i = 0; i < MusteriAra.gridView1.DataRowCount; i++)
            {
                DataRow dr = MusteriAra.gridView1.GetDataRow(i);
                pkFirma = dr["pkFirma"].ToString();
                if (dr["Sec"].ToString() != "True") continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkpersoneller", lUEPersonel.EditValue.ToString()));
                list.Add(new SqlParameter("@fkFirma", pkFirma));
                DB.ExecuteSQL("UPDATE Firmalar SET fkPerTeslimEden=@fkpersoneller where pkFirma=@fkFirma", list);
                DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET  Aktif=1 where fkFirma=" + pkFirma);
                OlmayanlariEkle(pkFirma);
            }
            PersoneleBagliMusteriListesiGetir();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Müşteri(ler) Çıkartılsın mı?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET  Aktif=0 where fkFirma=" + dr["pkFirma"].ToString());
                    DB.ExecuteSQL("update Firmalar set fkPerTeslimEden=null  where pkFirma="+ dr["pkFirma"].ToString());
                }
            }
            PersoneleBagliMusteriListesiGetir();
        }
        void siparisgunlerigetireski()
        {
            if (gridView2.FocusedRowHandle < 0) return;
            checkEdit1.Checked = false;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();
            DataTable dt = DB.GetData(@"SELECT pkMusteriZiyaretGunleri,fkFirma, Pzt, Sali, Car, Per, Cuma, Ctesi, Pzar
, PztGunSonra, SaliGunSonra, CarGunSonra, PerGunSonra, CumaGunSonra,CtesiGunSonra, PzarGunSonra
FROM  MusteriZiyaretGunleri WHERE  fkFirma = " + pkFirma);
            if (dt.Rows.Count == 0)
            {
                DB.ExecuteSQL(@"INSERT INTO MusteriZiyaretGunleri (Pzt, Sali, Car, Per, Cuma, Ctesi, Pzar,fkFirma)
                VALUES(0,0,0,0,0,0,0,"+pkFirma+")");

                dt = DB.GetData(@"SELECT pkMusteriZiyaretGunleri,fkFirma, Pzt, Sali, Car, Per, Cuma, Ctesi, Pzar
, PztGunSonra, SaliGunSonra, CarGunSonra, PerGunSonra, CumaGunSonra,CtesiGunSonra, PzarGunSonra
                           FROM  MusteriZiyaretGunleri WHERE  fkFirma = " + pkFirma);
            }          

        }
        void OlmayanlariEkle(string fkFirma)
        {
            string fkPersoneller = "0", fkGunler = "0";
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
                    DB.ExecuteSQL(@"INSERT INTO MusteriZiyaretGunleri (VarYok,fkFirma,fkGunler,GunSonra,fkPersoneller,fkSablonGrup) 
  values(0,@fkFirma,@fkGunler,@GunSonra,@fkPersoneller,1)", list);
                }
            }
        }
        void SiparisGunleriGetir(string pkFirma)
        {
            OlmayanlariEkle(pkFirma);
            string sql = @"select pkMusteriZiyaretGunleri,g.GunAdi,mzg.fkGunler,mzg.GunSonra,mzg.fkPersoneller,mzg.VarYok,
fkSiparisSablonlari ,mzg.fkSatislarEnson,
case 
when mzg.VarYok is null then 'Belirtilmemiş'
when mzg.VarYok=0 then 'Yok'
when mzg.VarYok=1 then 'Tanımlı'
end as Durumu,ss.Aciklama
from MusteriZiyaretGunleri  mzg
inner join Gunler g on g.pkGunler=mzg.fkGunler
left join SiparisSablonlari ss on ss.pkSiparisSablonlari=fkSiparisSablonlari
where mzg.fkFirma=" + pkFirma + " order by g.Kod";
            gcZiyaret.DataSource = DB.GetData(sql);
        }
        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            checkEdit1.Checked = false;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();
            SiparisGunleriGetir(pkFirma);
        }

        private void simpleButton37_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Bilgileri Kaydetmek İstediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
           // for (int m = 0; m < gridView2.DataRowCount; m++)
            //{
                //DataRow drf = gridView2.GetDataRow(m);
                //string pkFirma = drf["pkFirma"].ToString();
                //if (drf["Sec"].ToString() == "True")
                //{
                    //SiparisGunleriGetir(pkFirma);
                    for (int i = 0; i < gridView1.DataRowCount; i++)
                    {
                        DataRow dr = gridView1.GetDataRow(i);
                        string pkMusteriZiyaretGunleri = dr["pkMusteriZiyaretGunleri"].ToString();
                        string fkSiparisSablonlari = dr["fkSiparisSablonlari"].ToString();
                        string VarYok = "0";
                        if (dr["VarYok"].ToString() == "True")
                            VarYok = "1";
                        string sql = "";
                        if (fkSiparisSablonlari == "")
                            sql = "UPDATE  MusteriZiyaretGunleri SET VarYok= " + VarYok + ",fkPersoneller=" + lUEPersonel.EditValue.ToString() +
                            " WHERE pkMusteriZiyaretGunleri=" + pkMusteriZiyaretGunleri;
                        else
                            sql = "UPDATE MusteriZiyaretGunleri SET VarYok=" + VarYok + ",fkPersoneller=" + lUEPersonel.EditValue.ToString() +
                            ",fkSiparisSablonlari=" + fkSiparisSablonlari +
                            " WHERE pkMusteriZiyaretGunleri=" + pkMusteriZiyaretGunleri;

                        DB.ExecuteSQL(sql);
                        DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkPersoneller=" + lUEPersonel.EditValue.ToString() +
                            " where pkMusteriZiyaretGunleri=" + pkMusteriZiyaretGunleri);
                    }
               // }
            //}
            //if (gridView2.) bir kayıt seçili ise 
           
                 //Getir();


//            string s="";
//            bool hatavar = false;
//            int j = gridView2.FocusedRowHandle;
//            for (int i = 0; i < gridView2.DataRowCount; i++)
//            {
//                DataRow dr = gridView2.GetDataRow(i);
//                string Sec = dr["Sec"].ToString();
//                if (Sec == "True")
//                {
//                    string pkFirma = dr["pkFirma"].ToString();
//                    DataTable dt = DB.GetData(@"SELECT pkMusteriZiyaretGunleri,fkFirma, Pzt, Sali, Car, Per, Cuma, Ctesi, Pzar,
// PztGunSonra, SaliGunSonra, CarGunSonra, PerGunSonra, CumaGunSonra,CtesiGunSonra, PzarGunSonra
//                           FROM  MusteriZiyaretGunleri WHERE  fkFirma = " + pkFirma);
//                    string sql = "";
//                    ArrayList list = new ArrayList();
//                    list.Add(new SqlParameter("@Pzt", PAS.Checked));
//                    list.Add(new SqlParameter("@Sali", SAS.Checked));
//                    list.Add(new SqlParameter("@Car", CAS.Checked));
//                    list.Add(new SqlParameter("@Per", PES.Checked));
//                    list.Add(new SqlParameter("@Cuma", CUS.Checked));
//                    list.Add(new SqlParameter("@Ctesi", CTS.Checked));
//                    list.Add(new SqlParameter("@Pzar", PZS.Checked));
//                    list.Add(new SqlParameter("@fkFirma", pkFirma));
//                    list.Add(new SqlParameter("@PztGunSonra", luePts.EditValue.ToString()));
//                    list.Add(new SqlParameter("@SaliGunSonra", lueS.EditValue.ToString()));
//                    list.Add(new SqlParameter("@CarGunSonra", lueC.EditValue.ToString()));
//                    list.Add(new SqlParameter("@PerGunSonra", luePer.EditValue.ToString()));
//                    list.Add(new SqlParameter("@CumaGunSonra", lueCuma.EditValue.ToString()));
//                    list.Add(new SqlParameter("@CtesiGunSonra", lueCtesi.EditValue.ToString()));
//                    list.Add(new SqlParameter("@PzarGunSonra", luePazar.EditValue.ToString()));
//                    if (dt.Rows.Count == 0)
//                    {
//                        sql = @"INSERT INTO MusteriZiyaretGunleri (fkFirma, Pzt, Sali, Car, Per, Cuma, Ctesi, Pzar,
//PztGunSonra, SaliGunSonra, CarGunSonra, PerGunSonra, CumaGunSonra,CtesiGunSonra, PzarGunSonra)
//                    VALUES(@fkFirma,@Pzt,@Sali,@Car,@Per,@Cuma,@Ctesi,@Pzar,
//@PztGunSonra,@SaliGunSonra,@CarGunSonra,@PerGunSonra,@CumaGunSonra,@CtesiGunSonra,@PzarGunSonra)";
//                    }
//                    else
//                    {
//                        sql = @"UPDATE MusteriZiyaretGunleri SET Pzt=@Pzt, Sali=@Sali,Car=@Car,Per=@Per,Cuma=@Cuma,Ctesi=@Ctesi,Pzar=@Pzar,
//PztGunSonra=@PztGunSonra, SaliGunSonra=@SaliGunSonra, CarGunSonra=@CarGunSonra, PerGunSonra=@PerGunSonra, CumaGunSonra=@CumaGunSonra,
//CtesiGunSonra=@CtesiGunSonra, PzarGunSonra=@PzarGunSonra  WHERE fkFirma=@fkFirma";
//                    }
//                     s = DB.ExecuteSQL(sql, list);
//                     if (s != "0") hatavar = true;
//                }
//            }
            
//            frmMesaj mesaj = new frmMesaj();
//            if (hatavar ==false)
//            {
//                mesaj.label1.Text = "Bilgiler Kaydedildi.";
//                mesaj.label1.BackColor = System.Drawing.Color.YellowGreen;
//            }
//            else
//            {
//                mesaj.label1.Text = "Hata Oluştur";
//                mesaj.label1.BackColor = System.Drawing.Color.Red;
//            }
//            mesaj.Show();
//            PersoneleBagliMusteriListesiGetir();
//            gridView2.FocusedRowHandle=j;
//            secilenlerikaldir();
//            checkEdit1.Checked = false;
      }

//        private void lUEPersonel_EditValueChanged(object sender, EventArgs e)
//        {
//            PersoneleBagliMusteriListesiGetir();
//        }
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
        private void btnyazdir_Click(object sender, EventArgs e)
        {
            yazdir();
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            frmPersonel personelkart = new frmPersonel("0");
            personelkart.ShowDialog();
            PersonelGetir();
        }

        private void frmSiparisSablonlari_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
                Close();
            if(e.KeyCode==Keys.F4)
                simpleButton5_Click(sender,e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());

            frmMusteriKarti MusteriKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            MusteriKarti.ShowDialog();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            if(panelziyaretgunleri.Visible==false)
            {
                panelziyaretgunleri.Visible = true;
                simpleButton1.Text = ">";
            }
            else
            {
                panelziyaretgunleri.Visible = false;
                simpleButton1.Text = "<";
            }
        }
        private void gridView2_EndSorting(object sender, EventArgs e)
        {
            //if (gridView2.DataRowCount > 0)
            //{
            //    gridView2.FocusedRowHandle = 0;
            //    gridView2.SetRowCellValue(0, "Sec", true);
            //}
        }

        private void gridView2_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView2.DataRowCount < 1000)
            {
                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    gridView2.SetRowCellValue(i, "Sec", false);
                }
            }
            if (gridView2.SelectedRowsCount == 1)
            {
                DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                if (dr["Sec"].ToString() == "True")
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "Sec", false);
                else
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "Sec", true);
                return;
            }
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                int si = gridView2.GetSelectedRows()[i];
                DataRow dr = gridView2.GetDataRow(si);
                gridView2.SetRowCellValue(si, "Sec", true);
            }
        }

        private void gridView2_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                DataRow dr = gridView2.GetDataRow(e.RowHandle);
                if (dr["Sec"].ToString() == "True")
                {
                    e.Appearance.BackColor = Color.SlateBlue;

                    e.Appearance.ForeColor = Color.White;

                }
            } 
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
			{
                gridView2.SetRowCellValue(i,"Sec", checkEdit2.Checked);
			}
            
        }

        private void lUEPersonel_EditValueChanged(object sender, EventArgs e)
        {
            PersoneleBagliMusteriListesiGetir();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                gridView1.SetRowCellValue(i, "VarYok", checkEdit1.Checked);   
            }
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0) return;
            checkEdit1.Checked = false;
            DataRow dr = gridView2.GetDataRow(e.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();
            SiparisGunleriGetir(pkFirma);
        }
    }
}
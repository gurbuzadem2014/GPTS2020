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
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;

namespace GPTS
{
    public partial class frmVardiyaCizelgesi : DevExpress.XtraEditors.XtraForm
    {
        bool Degisiklikvar = false;
        public frmVardiyaCizelgesi()
        {
            InitializeComponent();
        }
        private void frmVardiyaCizelgesi_Load(object sender, EventArgs e)
        {
            vSirketler();
            vProjeler();
            vVardiyaSablon();
            if (DB.pkVardiyalar == 0)
            {
                simpleButton1.Text = "Oluştur";
                bastarih.DateTime = DateTime.Today;
                bitistarih.DateTime = DateTime.Today;
                luekurum.Properties.ReadOnly = false;
                lUSirket.Properties.ReadOnly = false;
                bastarih.Properties.ReadOnly = false;
                bitistarih.Properties.ReadOnly = false;
            }
            else
            {
                simpleButton1.Text = "Güncelle";
                DataTable dt = DB.GetData("select * from Vardiyalar where pkVardiya=" + DB.pkVardiyalar.ToString());
                if (dt.Rows.Count == 0) return;
                bastarih.EditValue = dt.Rows[0]["BasTarih"].ToString();
                bitistarih.EditValue = dt.Rows[0]["BitTarih"].ToString();
                luekurum.EditValue = int.Parse(dt.Rows[0]["fkProjeler"].ToString());
                lUSirket.EditValue = int.Parse(dt.Rows[0]["fkSirket"].ToString());
                luekurum.Properties.ReadOnly = true;
                lUSirket.Properties.ReadOnly = true;
                bastarih.Properties.ReadOnly = true;
                bitistarih.Properties.ReadOnly = true;
            }
            gridyukle();
        }
        void gridyukle()
        {
            gcVardiya.DataSource = DB.GetData(@"SELECT vd.pkVardiyaDetay, vd.fkVardiya, 
            vd.Tarih, vd.Gunduz, vd.Gece, vd.izin, vd.Personel, vd.ADANZ, vd.fkPersoneller, P.adi + '  ' + P.soyadi AS Personeladisoyadi
            FROM  VardiyaDetay AS vd LEFT OUTER JOIN
                      Personeller AS P ON P.pkpersoneller = vd.fkPersoneller
            WHERE vd.fkVardiya =" + DB.pkVardiyalar.ToString());
        }
        void vSirketler()
        {
            DataTable dtb = DB.GetData("select * from Sirketler");
            lUSirket.Properties.DataSource = dtb;
            lUSirket.Properties.ValueMember = "pkSirket";
            lUSirket.Properties.DisplayMember = "Sirket";
        }
        void vProjeler()
        {
            DataTable dt = DB.GetData("SELECT * FROM Projeler");
            luekurum.Properties.DataSource = dt;
            luekurum.Properties.ValueMember = "pkProjeler";
            luekurum.Properties.DisplayMember = "ProjeAdi";
        }
        void ProjePersoneller(int Projeid)
        {
            string sql = "SELECT pkpersoneller,(adi+' '+Soyadi) as Adi FROM personeller";
            if (Projeid > 0)
                sql += " where fkGrup=" + Projeid;
            DataTable dtp = DB.GetData(sql);
            sEPersAdet.Value = dtp.Rows.Count;
            repositoryItemLookUpEdit1.DataSource = dtp;
            repositoryItemLookUpEdit1.ValueMember = "pkpersoneller";
            repositoryItemLookUpEdit1.DisplayMember = "Adi";
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (DB.pkVardiyalar != 0)
            {
                simpleButton2_Click(sender, e);
                return;
            }
            string sql = "";
            sql = "INSERT INTO Vardiyalar (fkProjeler, fkSirket, BasTarih, BitTarih) values(@fkProjeler,@fkSirket,@BasTarih,@BitTarih) SELECT IDENT_CURRENT('Vardiyalar')";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkProjeler", luekurum.EditValue.ToString()));
            list.Add(new SqlParameter("@fkSirket", lUSirket.EditValue.ToString()));
            list.Add(new SqlParameter("@BasTarih", bastarih.DateTime.ToString()));
            list.Add(new SqlParameter("@BitTarih", bitistarih.DateTime.ToString()));
            string IID = DB.ExecuteScalarSQL(sql, list);
            DB.pkVardiyalar = int.Parse(IID);
            //Detay
            sql = "";
            DateTime dtim = new DateTime();
            dtim = bastarih.DateTime;
            TimeSpan fark = bitistarih.DateTime - bastarih.DateTime;
            int r = ((System.Data.DataTable)(((DevExpress.XtraEditors.Repository.RepositoryItemLookUpEditBase)(repositoryItemLookUpEdit1)).DataSource)).Rows.Count;

            for (int i = 0; i < fark.Days+1; i++)
            {
                list.Clear();
                list.Add(new SqlParameter("@fkVardiya", DB.pkVardiyalar));
                list.Add(new SqlParameter("@Gunduz", ""));
                list.Add(new SqlParameter("@Gece", ""));
                list.Add(new SqlParameter("@Vardiya3", ""));
                list.Add(new SqlParameter("@izin", ""));
                string val="0";
                if (r > i)
                {
                    val = ((System.Data.DataTable)(((DevExpress.XtraEditors.Repository.RepositoryItemLookUpEditBase)(repositoryItemLookUpEdit1)).DataSource)).Rows[i]["pkpersoneller"].ToString();
                    list.Add(new SqlParameter("@fkPersoneller", val));
                }
                else
                    list.Add(new SqlParameter("@fkPersoneller", val));
                list.Add(new SqlParameter("@Tarih", dtim.ToString()));
                char c = (char)(i+65);
                if (i < r)
                list.Add(new SqlParameter("@ADANZ", c.ToString()));
                else
                list.Add(new SqlParameter("@ADANZ",""));
                sql = "INSERT INTO VardiyaDetay (fkVardiya, Tarih, Gunduz, Gece,Vardiya3, izin, fkPersoneller,ADANZ) VALUES(@fkVardiya,@Tarih,@Gunduz,@Gece,@Vardiya3,@izin,@fkPersoneller,@ADANZ)";
                DB.ExecuteSQL(sql, list);
                dtim = dtim.AddDays(1);
            }
            //DB.pkVardiyalar = int.Parse(DB.GetData("SELECT MAX(pkVardiyaDetay) FROM VardiyaDetay").Rows[0][0].ToString());
            gridyukle();
            simpleButton1.Text = "Güncelle";
            DevExpress.XtraEditors.XtraMessageBox.Show("Vardiya Çizelgesi Oluşturuldu.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Degisiklikvar = false;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                sql = "UPDATE VardiyaDetay SET Gunduz=@Gunduz,Gece=@Gece,Vardiya3=@Vardiya3,izin=@izin,ADANZ=@ADANZ,fkPersoneller=@fkPersoneller where pkVardiyaDetay=@pkVardiyaDetay";
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Gunduz", dr["Gunduz"].ToString()));
                list.Add(new SqlParameter("@Gece", dr["Gece"].ToString()));
                list.Add(new SqlParameter("@Vardiya3", dr["Vardiya3"].ToString()));
                list.Add(new SqlParameter("@izin", dr["izin"].ToString()));
                list.Add(new SqlParameter("@ADANZ", dr["ADANZ"].ToString()));
                list.Add(new SqlParameter("@fkPersoneller", dr["fkPersoneller"].ToString()));
                list.Add(new SqlParameter("@pkVardiyaDetay", dr["pkVardiyaDetay"].ToString()));
                DB.ExecuteSQL(sql, list);
            }
            DevExpress.XtraEditors.XtraMessageBox.Show("Vardiya Çizelgesi Güncellendi.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridyukle();
            Degisiklikvar = false;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (Degisiklikvar == true)
            {
                simpleButton1_Click(sender, e);//Kaydet
                gridyukle();//Listele
            }
            RaporOnizleme(false);
        }
        void RaporOnizleme(bool Disigner)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";

           
            DevExpress.XtraReports.UI.XtraReport rapor = new DevExpress.XtraReports.UI.XtraReport();
            RaporDosyasi = exedizini + "\\Raporlar\\VardiyaListesi.repx";
            rapor.DataSource = gcVardiya.DataSource;
            //rapor.CreateDocument();
            if (!File.Exists(RaporDosyasi)) { Print(); return; }
            rapor.LoadLayout(RaporDosyasi);
            rapor.FindControl("label15", true).Text = lUSirket.Text;
            rapor.FindControl("label16", true).Text = luekurum.Text;
            if (Disigner)
               rapor.ShowDesignerDialog();
            else
               rapor.ShowRibbonPreview();
        }
        public void Print()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gcVardiya;

            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
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
        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));

        }
        public virtual void InitPrintTool(PrintTool tool)
        {
            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            RaporOnizleme(true);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void luekurum_EditValueChanged(object sender, EventArgs e)
        {
            if (luekurum.EditValue==null || luekurum.EditValue.ToString()=="")
                ProjePersoneller(0);
            else
                ProjePersoneller(int.Parse(luekurum.EditValue.ToString()));
        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            Degisiklikvar = true;
        }

        private void frmVardiyaCizelgesi_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Degisiklikvar == true)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Değişiklikler Kaydedilsin mi?", "Personel Takip Sistemi", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (secim == DialogResult.No) e.Cancel = false;
                if (secim == DialogResult.Cancel) e.Cancel = true;
                if (secim == DialogResult.Yes)
                {
                    simpleButton1_Click(sender, e);//Kaydet
                    e.Cancel = false;
                }
            }
        }

        private void frmVardiyaCizelgesi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            } 
        }

        private void spinEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (sEVardiyaSayisi.Value == 1)
            {
                gC1Vardiya.Visible = true;
                gC2Vardiya.Visible = false;
                gC3Vardiya.Visible = false;
            }
            else if (sEVardiyaSayisi.Value == 2)
            {
                gC1Vardiya.Visible = true;
                gC2Vardiya.Visible = true;
                gC3Vardiya.Visible = false;
            }
            else if (sEVardiyaSayisi.Value == 3)
            {
                gC1Vardiya.Visible = true;
                gC2Vardiya.Visible = true;
                gC3Vardiya.Visible = true;
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            int gh1 = 0;  
            int gh2 = 0;  
            int gh3 = 0;  
            int gh4 = 0;  
            if (sEVardiyaSayisi.EditValue.ToString() == "2")
            {
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    gh1++;
                    gh2++;
                    gh3++;
                    gh4++;
                    if (gh1 % g1.Value == 0)
                    {
                        if ((comboBox1.Items.Count - 1) == comboBox1.SelectedIndex)
                            comboBox1.SelectedIndex=0;
                        else
                            comboBox1.SelectedIndex++;

                        comboBox4.SelectedIndex = 0;
                    }

                    if (gh2 % g2.Value == 0)
                    {
                        if ((comboBox2.Items.Count - 1) == comboBox2.SelectedIndex)
                            comboBox2.SelectedIndex=0;
                        else
                            comboBox2.SelectedIndex++;
                    }

                    if (gh3 % g3.Value == 0)
                    {
                        if ((comboBox3.Items.Count - 1) == comboBox3.SelectedIndex)
                            comboBox3.SelectedIndex=0;
                        else
                            comboBox3.SelectedIndex++;
                    }

                    if (gh4 % g4.Value == 0)
                    {
                        if ((comboBox4.Items.Count - 1) == comboBox4.SelectedIndex)
                            comboBox4.SelectedIndex=0;
                        else
                            comboBox4.SelectedIndex++;
                    }
                    gridView1.SetRowCellValue(i, "Gunduz", comboBox1.Text);
                    gridView1.SetRowCellValue(i, "Gece", comboBox2.Text);
                    gridView1.SetRowCellValue(i, "izin", comboBox4.Text);
                }
            }


            //if (sEVardiyaSayisi.EditValue.ToString() == "3")
            //{
            //    gridView1.SetRowCellValue(0, "Gunduz", comboBox1.Items[0].ToString());
            //    gridView1.SetRowCellValue(0, "Gece", comboBox1.Items[1].ToString());
            //    gridView1.SetRowCellValue(0, "Vardiya3", comboBox1.Items[2].ToString());
            //    gridView1.SetRowCellValue(0, "izin", comboBox1.Items[3].ToString());
            //}
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Vardiya Sayısını Kontrol Ediniz!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            frmVardiyaSablon VardiyaSablon = new frmVardiyaSablon();
            VardiyaSablon.ShowDialog();
            vVardiyaSablon();
        }
        void vVardiyaSablon()
        {
            DataTable dt = DB.GetData("SELECT * FROM VardiyaSablon");
            lUEVardiyaSablon.Properties.DataSource = dt;
            lUEVardiyaSablon.Properties.ValueMember = "pkVardiyaSablon";
            lUEVardiyaSablon.Properties.DisplayMember = "SablonAdi";
            //lUEVardiyaSablon.Properties.too = "SablonAdi";
        }

        private void labelControl8_Click(object sender, EventArgs e)
        {
            frmVardiyaSablon VardiyaSablon = new frmVardiyaSablon();
            DB.pkVardiyaSablon = int.Parse(lUEVardiyaSablon.EditValue.ToString());
            VardiyaSablon.ShowDialog();
            vVardiyaSablon();
        }

        private void lUEVardiyaSablon_EditValueChanged(object sender, EventArgs e)
        {
            string ToolTip = DB.GetData("SELECT * FROM VardiyaSablon where pkVardiyaSablon=" + lUEVardiyaSablon.EditValue.ToString()).Rows[0]["Aciklama"].ToString();
            lUEVardiyaSablon.ToolTip = ToolTip;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kayıt Silinsin mi?", "GPTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            try
            {
                string sql = "DELETE FROM VardiyaDetay WHERE pkVardiyaDetay=" + dr["pkVardiyaDetay"].ToString();
                DB.ExecuteSQL(sql);
            }
            catch (Exception exp)
            {
                btnSil.Enabled = true;
                return;
            }
            btnSil.Enabled = false;
            gridView1.DeleteSelectedRows();
            //simpleButton1_Click(sender, e);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Kayıtlar Silinsin mi?", "GPTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            try
            {
                string sql = "DELETE FROM VardiyaDetay WHERE fkVardiya=" + DB.pkVardiyalar.ToString();
                DB.ExecuteSQL(sql);
                gridyukle();
            }
            catch (Exception exp)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(exp.Message.ToString(), "GPTS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            btnSil.Enabled = true;
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from VardiyaSablonDetay where fkVardiyaSablon=" + lUEVardiyaSablon.EditValue.ToString());
            for (int i = 0; i < dt.Rows.Count-1; i++)
            {
                gridView1.SetRowCellValue(i, "Gunduz", dt.Rows[i]["Gunduz"].ToString());
                gridView1.SetRowCellValue(i, "Gece", dt.Rows[i]["Gece"].ToString());
                gridView1.SetRowCellValue(i, "izin", dt.Rows[i]["izin"].ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
           int k = gridView1.DataRowCount;
           int r = ((System.Data.DataTable)(((DevExpress.XtraEditors.Repository.RepositoryItemLookUpEditBase)(repositoryItemLookUpEdit1)).DataSource)).Rows.Count;
           string ilk = "";
           for (int i = 0; i < r; i++)
           {
               DataRow dr = gridView1.GetDataRow(i);
               if ((i + 1) % 2 == 0)
               {
                   comboBox1.Items.Add(ilk + "-" + dr["ADANZ"].ToString());
                   comboBox2.Items.Add(ilk + "-" + dr["ADANZ"].ToString());
                   comboBox3.Items.Add(ilk + "-" + dr["ADANZ"].ToString());
                   comboBox4.Items.Add(ilk + "-" + dr["ADANZ"].ToString());
               }
               else
                   ilk = dr["ADANZ"].ToString();
               if ((i + 1) == r && (i + 1) % 2 == 1)
               {
                   comboBox1.Items.Add(dr["ADANZ"].ToString());
                   comboBox2.Items.Add(dr["ADANZ"].ToString());
                   comboBox3.Items.Add(dr["ADANZ"].ToString());
                   comboBox4.Items.Add(dr["ADANZ"].ToString());
               }
           }
           comboBox1.SelectedIndex = 0;
           comboBox2.SelectedIndex = 1;
           comboBox3.SelectedIndex = -1;
           comboBox4.SelectedIndex = 2;
        }

        private void lUSirket_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
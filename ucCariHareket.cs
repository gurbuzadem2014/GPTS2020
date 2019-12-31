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

namespace GPTS
{
    public partial class ucCariHareket : DevExpress.XtraEditors.XtraUserControl
    {
        public ucCariHareket()
        {
            InitializeComponent();
            ilkTarih.EditValue = DateTime.Today;
            sonTarih.EditValue = DateTime.Today;
            //DB.PkFirma = 0;
        }
        private void ucCariHareket_Load(object sender, EventArgs e)
        {
            string sql="select 0 as PkFirma,'Seçiniz...' as Firmaadi union all SELECT PkFirma,Firmaadi FROM Firmalar where Aktif=1";
            DataTable dtf = DB.GetData(sql);
            lueCari.Properties.DataSource = dtf;
            lueCari.Properties.ValueMember = "PkFirma";
            lueCari.Properties.DisplayMember = "Firmaadi";
            lueCari.EditValue =  DB.PkFirma;
        }
        void temizle()
        {
            //lUTuru.EditValue = 0;
            //islemTarih.DateTime = DateTime.Today;
            //teAciklama.Text = "";
            //cEBorc.Value = 0;
            //cEAlacak.Value = 0;

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //gridyukle();
            btnSil.Tag = "";
            btnSil.ToolTip = "";
            BorcAlacakDurumu();
            button1_Click(sender, e);
        }
        void BorcAlacakDurumu()
        {
            if (lueCari.EditValue != null)
                DB.PkFirma = int.Parse(lueCari.EditValue.ToString());
            //gridyukle();

            DataTable dt = new DataTable();
            string sql = "Select isnull(SUM(Borc),0) as Borc,isnull(sum(Alacak),0) as Alacak from KasaHareket where Modul=6";
            if (DB.PkFirma > 0)
                sql += " and fkPersoneller=" + DB.PkFirma.ToString();
            dt = DB.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                cEBorc.Value = Decimal.Parse(dt.Rows[0]["Borc"].ToString());
                ceAlacak.Value = Decimal.Parse(dt.Rows[0]["Alacak"].ToString());
                ceBakiye.Value = cEBorc.Value - ceAlacak.Value;
            }
        }
        private void luekurum_EditValueChanged(object sender, EventArgs e)
        {
            //BorcAlacakDurumu();
        }
        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.pkKasaHareket = int.Parse(dr["pkKasaHareket"].ToString());
            //frmCariHareket CariHareket = new frmCariHareket();
            //CariHareket.ShowDialog();
            //gridyukle();
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["PkFirma"].ToString());
            btnSil.Tag = dr["pkKasaHareket"].ToString();
            btnSil.ToolTip = dr["Aciklama"].ToString();
            btnSil.Enabled = true;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(btnSil.ToolTip + " Kasa Hareketi Silinsin mi?", "GPTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            try
            {
                string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + btnSil.Tag.ToString();
                DB.ExecuteSQL(sql);
            }
            catch (Exception exp)
            {
                btnSil.Enabled = true;
                return;
            }
            btnSil.Enabled = false;
            gridView2.DeleteSelectedRows();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\CariHareketRaporu.repx");
            rapor.FindControl("xlCariAdi", false).Text = lueCari.Text;//DB.FirmaAdi;
            rapor.FindControl("xlTarihAraligi",false).Text = ilkTarih.Text + " - " + sonTarih.Text;
            rapor.DataSource = gCPerHareketleri.DataSource;//DB.GetData("select * from KasaHareket where Modul = 6 and fkPersoneller=" + lueCari.EditValue.ToString());
            rapor.ShowRibbonPreview();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\CariHareketRaporu.repx");
            //rapor.xlCariAdi.Text = lueCari.Text;//DB.FirmaAdi;
            rapor.DataSource = gCPerHareketleri.DataSource;//DB.GetData("select * from KasaHareket where Modul = 6 and fkPersoneller=" + lueCari.EditValue.ToString());
            rapor.ShowDesigner();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lueCari.EditValue.ToString() == "0")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Cari Kart Seçiniz!", "GPTS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lueCari.Focus();
                return;
            }
            string sql = "";
            DataTable sanal = new DataTable();
            ArrayList listDevir = new ArrayList();
            sql = @"select 0 as pkKasaHareket,f.PkFirma, f.Firmaadi,'01.01.1900' as Tarih,'Devir' as Aciklama,sum(kh.Borc) as Borc,sum(kh.Alacak) as Alacak,'NAKİT' AS OdemeSekli
                            from KasaHareket kh
                            inner join dbo.Firmalar f on kh.fkPersoneller=f.PkFirma
                            where Modul=6  and Tarih < @ilktar and kh.fkPersoneller=@fkPersoneller
                            GROUP BY f.PkFirma,f.Firmaadi";
            listDevir.Add(new SqlParameter("@ilktar", ilkTarih.DateTime));
            listDevir.Add(new SqlParameter("@fkPersoneller", lueCari.EditValue.ToString()));
            DataTable dtDevir = DB.GetData(sql,listDevir);
            sql = @"select kh.pkKasaHareket,f.PkFirma,f.Firmaadi,kh.Tarih,kh.Aciklama,isnull(kh.Borc,0) as Borc,isnull(kh.Alacak,0) as Alacak,'NAKİT' AS OdemeSekli
                            from KasaHareket kh
                            inner join dbo.Firmalar f on kh.fkPersoneller=f.PkFirma
                            where Modul=6  and kh.fkPersoneller=@fkPersoneller and Tarih BETWEEN @ilktar and @sontar
                            order by Tarih";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilkTarih.DateTime));
            list.Add(new SqlParameter("@sontar", sonTarih.DateTime));
            list.Add(new SqlParameter("@fkPersoneller", lueCari.EditValue.ToString()));

            DataTable dt2 = DB.GetData(sql,list);
            sanal.Columns.Add(new DataColumn("pkKasaHareket", typeof(Int32)));
            sanal.Columns.Add(new DataColumn("PkFirma", typeof(Int32)));
            sanal.Columns.Add(new DataColumn("Firmaadi", typeof(string)));
            sanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
            sanal.Columns.Add(new DataColumn("Aciklama", typeof(string)));
            sanal.Columns.Add(new DataColumn("Borc", typeof(float)));
            sanal.Columns.Add(new DataColumn("Alacak", typeof(float)));
            sanal.Columns.Add(new DataColumn("OdemeSekli", typeof(string)));
            sanal.Columns.Add(new DataColumn("BakiyeB", typeof(float)));
            sanal.Columns.Add(new DataColumn("BakiyeA", typeof(float)));
            DataRow dr;
            float Borc=0;
            float Alacak=0;
            float Bakiye=0;
            //Devir
            if(dtDevir.Rows.Count>0)
            {
                Borc = float.Parse(dtDevir.Rows[0]["Borc"].ToString());
                Alacak = float.Parse(dtDevir.Rows[0]["Alacak"].ToString());
                Bakiye = Bakiye + (Borc - Alacak);
                dr = sanal.NewRow();
                dr["pkKasaHareket"] = dtDevir.Rows[0]["pkKasaHareket"];
                dr["PkFirma"] = dtDevir.Rows[0]["PkFirma"];
                dr["Firmaadi"] = dtDevir.Rows[0]["Firmaadi"];
                dr["Tarih"] = dtDevir.Rows[0]["Tarih"];
                dr["Aciklama"] = dtDevir.Rows[0]["Aciklama"];
                dr["Borc"] = Borc;
                dr["Alacak"] = Alacak;
                dr["OdemeSekli"] = dtDevir.Rows[0]["OdemeSekli"];
                if (Bakiye >= 0)
                    dr["BakiyeB"] = Bakiye;
                else
                    dr["BakiyeA"] = Bakiye;
                sanal.Rows.Add(dr);
            }
            //hareketler
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                Borc=float.Parse(dt2.Rows[i]["Borc"].ToString());
                Alacak = float.Parse(dt2.Rows[i]["Alacak"].ToString());
                Bakiye = Bakiye+(Borc-Alacak);
                dr = sanal.NewRow();
                dr["pkKasaHareket"] = dt2.Rows[i]["pkKasaHareket"];
                dr["PkFirma"] = dt2.Rows[i]["PkFirma"];
                dr["Firmaadi"] = dt2.Rows[i]["Firmaadi"];
                dr["Tarih"] = dt2.Rows[i]["Tarih"];
                dr["Aciklama"] = dt2.Rows[i]["Aciklama"];
                dr["Borc"] = Borc;
                dr["Alacak"] = Alacak;
                dr["OdemeSekli"] = dt2.Rows[i]["OdemeSekli"];
                if (Bakiye >=0)
                dr["BakiyeB"] = Bakiye;
                else
                dr["BakiyeA"] = Bakiye;
                sanal.Rows.Add(dr);
            }

            //sonraki dönem bakiye
            if (cESonrakiBakiye.Checked)
            {
                ArrayList listDevirSonraki = new ArrayList();
                ArrayList listsonraki = new ArrayList();
                sql = @"select 0 as pkKasaHareket,f.PkFirma, f.Firmaadi,'01.01.1900' as Tarih,'Sonraki Günleri Toplamı' as Aciklama,isnull(sum(kh.Borc),0) as Borc,isnull(sum(kh.Alacak),0) as Alacak,'NAKİT' AS OdemeSekli
                            from KasaHareket kh
                            inner join dbo.Firmalar f on kh.fkPersoneller=f.PkFirma
                            where Modul=6  and Tarih > @sontar and fkPersoneller=@fkPersoneller
                            GROUP BY f.PkFirma,f.Firmaadi";
                listDevirSonraki.Add(new SqlParameter("@sontar", sonTarih.DateTime));
                listDevirSonraki.Add(new SqlParameter("@fkPersoneller", lueCari.EditValue.ToString()));
                DataTable dtDevirSonraki = DB.GetData(sql, listDevirSonraki);
                if (dtDevirSonraki.Rows.Count > 0)
                {
                    Borc = float.Parse(dtDevirSonraki.Rows[0]["Borc"].ToString());
                    Alacak = float.Parse(dtDevirSonraki.Rows[0]["Alacak"].ToString());
                    Bakiye = Bakiye + (Borc - Alacak);
                    dr = sanal.NewRow();
                    dr["pkKasaHareket"] = dtDevirSonraki.Rows[0]["pkKasaHareket"];
                    dr["PkFirma"] = dtDevirSonraki.Rows[0]["PkFirma"];
                    dr["Firmaadi"] = dtDevirSonraki.Rows[0]["Firmaadi"];
                    dr["Tarih"] = dtDevirSonraki.Rows[0]["Tarih"];
                    dr["Aciklama"] = dtDevirSonraki.Rows[0]["Aciklama"];
                    dr["Borc"] = Borc;
                    dr["Alacak"] = Alacak;
                    dr["OdemeSekli"] = dtDevirSonraki.Rows[0]["OdemeSekli"];
                    if (Bakiye >= 0)
                        dr["BakiyeB"] = Bakiye;
                    else
                        dr["BakiyeA"] = Bakiye;
                    sanal.Rows.Add(dr);
                }
            }
            gCPerHareketleri.DataSource = sanal;
        }

        private void cESonrakiBakiye_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}

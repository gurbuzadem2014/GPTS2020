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
using GPTS.islemler;

namespace GPTS
{
    public partial class frmStokDepoDevir : DevExpress.XtraEditors.XtraForm
    {
        public frmStokDepoDevir()
        {
            InitializeComponent();
        }


        private void frmStokDevir_Load(object sender, EventArgs e)
        {
            deDevirTarihi.DateTime = DateTime.Now;

            StokKartiGetir();

            repositoryItemLookUpEdit1.Properties.DataSource = DB.GetData("select * from Depolar with(nolock)");

            StokDevirGetir();

            Depolar();

            ceSimdikiMevcut.Focus();
            ceSimdikiMevcut.SelectAll();
        }

        void StokDevirGetir()
        {
            gridControl1.DataSource = DB.GetData("select * from StokDevir with(nolock) where fkStokKarti=" + fkStokKarti.Tag.ToString());
        }

        void StokKartiGetir()
        {
            DataTable dtStokKarti = DB.GetData("select *,dbo.fon_StokMevcut(pkStokKarti) as FnMevcut from StokKarti with(nolock) WHERE pkStokKarti=" + fkStokKarti.Tag.ToString());
            if (dtStokKarti.Rows.Count == 0)
            {
                MessageBox.Show("Stok Bulunamadı");
                return;
            }
            fkStokKarti.Text = dtStokKarti.Rows[0]["Stokadi"].ToString();

            //            string sql = @"declare @girisadettoplam float,@cikisadettoplam float,@stokdeviradet float
            //            set @girisadettoplam=(select sum(Adet) from Alislar A with(nolock) inner join AlisDetay AD with(nolock)  on A.pkAlislar=AD.fkAlislar where A.Siparis=1 and fkStokKarti=@fkStokKarti)
            //            set @cikisadettoplam=(select sum(Adet) from Satislar S with(nolock) inner join SatisDetay SD with(nolock) on S.pkSatislar=SD.fkSatislar where S.Siparis=1 and S.fkSatisDurumu not in(1,10,11) and fkStokKarti=@fkStokKarti)
            //            set @stokdeviradet=(select sum(DevirAdedi) from StokDevir D with(nolock) where fkStokKarti=@fkStokKarti)
            //
            //            select isnull(@girisadettoplam,0)-isnull(@cikisadettoplam,0)-isnull(@stokdeviradet,0) as kalan";

            //            sql = sql.Replace("@fkStokKarti",fkStokKarti.Tag.ToString());

            //            DataTable dt =  DB.GetData(sql);

            string m = dtStokKarti.Rows[0]["FnMevcut"].ToString();
            decimal d = 0;
            decimal.TryParse(m, out d);
            ceMevcut.Value = d;
            ceMevcut.Tag = d.ToString();

            lblToplamMevcut.Text = "Toplam Mevcut =" + dtStokKarti.Rows[0]["Mevcut"].ToString();
        }

        void Depolar()
        {
            lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock) where Aktif=1");
            lueDepolar.EditValue = Degerler.fkDepolar;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (ceSimdikiMevcut.EditValue == null)
            {
                ceSimdikiMevcut.Focus();
                return;
            }

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Stok Devir İşlemi Yapılacak Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            #region bu kısım stok kartındaki ile aynı
            string sql = @"INSERT INTO StokDevir (fkStokKarti,Tarih,Aciklama,OncekiAdet,DevirAdedi,fkKullanicilar,islemTarihi,fkDepolar)
                    values(@fkStokKarti,@Tarih,@Aciklama,@OncekiAdet,@DevirAdedi,@fkKullanicilar,getdate(),@fkDepolar)";

            ArrayList list0 = new ArrayList();
            list0.Add(new SqlParameter("@fkStokKarti", fkStokKarti.Tag.ToString()));
            list0.Add(new SqlParameter("@Tarih", deDevirTarihi.DateTime));
            list0.Add(new SqlParameter("@Aciklama", tEaciklama.Text));
            list0.Add(new SqlParameter("@OncekiAdet", ceMevcut.Tag.ToString().Replace(",", ".")));
 
            if (lueDepolar.EditValue==null)
                list0.Add(new SqlParameter("@fkDepolar", DBNull.Value));
            else
                list0.Add(new SqlParameter("@fkDepolar", lueDepolar.EditValue.ToString()));

            decimal deviradet = 0;
            //sıfırdan küçükse + yap
            if (ceMevcut.Value < 0)
                deviradet = (ceMevcut.Value * -1) + ceSimdikiMevcut.Value;
            else
            {
                //if (ceSimdikiMevcut.Value > ceMevcut.Value)
                    deviradet = ceSimdikiMevcut.Value - ceMevcut.Value;
                //else
                //{
                    //deviradet = ceSimdikiMevcut.Value - ceMevcut.Value;
                //}
            }

            list0.Add(new SqlParameter("@DevirAdedi", deviradet.ToString().Replace(",", ".")));
            list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

            string sonuc = DB.ExecuteSQL(sql, list0); 
            #endregion
            if (sonuc == "0")
            {
                 if (lueDepolar.EditValue!=null)
                    DB.ExecuteSQL("update StokKartiDepo set MevcutAdet=" + ceSimdikiMevcut.EditValue.ToString().Replace(",", ".") +
                    " where fkStokKarti=" + fkStokKarti.Tag.ToString() + " and fkDepolar=" +lueDepolar.EditValue.ToString());
            }

            if (sonuc == "0")
            {
                    DB.ExecuteSQL(@"update Stokkarti set Mevcut=(select isnull(sum(MevcutAdet),0) from StokKartiDepo
                    where fkStokKarti="+fkStokKarti.Tag.ToString()+") where pkStokKarti=" + fkStokKarti.Tag.ToString());
            }

            ceSimdikiMevcut.Value = 0;
            StokKartiGetir();
            StokDevirGetir();

            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
           //ceBakiyeKaydedilecek.Value = ceMevcut.Value;
           DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Depo Stok Mevcutları Sıfırlanacak. Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
           if (secim == DialogResult.No) return;

           string sql1 ="select pkStokKarti,Mevcut from StokKarti with(nolock) where pkStokKarti="+fkStokKarti.Tag.ToString();

           DataTable dt = DB.GetData(sql1);
           simpleButton3.Enabled = false;
           string depoid = lueDepolar.EditValue.ToString();

           DB.ExecuteSQL("delete from StokDevir where fkStokKarti="+ fkStokKarti.Tag.ToString());
           DB.ExecuteSQL("delete from StokSayimDetay where fkStokKarti=" + fkStokKarti.Tag.ToString());
           //DB.ExecuteSQL("delete from StokSayim where pkStokSayim in (select fkStokSayim from StokSayimDetay where fkDepolar=" +
           // depoid + " group by fkStokSayim)");
           //DB.ExecuteSQL("delete from  Sayim where fkDepolar=" + depoid);
           DB.ExecuteSQL("delete from DepoTransferDetay where fkDepolar=" + depoid + " and fkStokKarti=" + fkStokKarti.Tag.ToString());
           //DB.ExecuteSQL("delete from DepoTransfer where fkDepolar=" + depoid);

           decimal mevcut = ceSimdikiMevcut.Value;
           decimal gercekMevcut = 0;//decimal.Parse(dt.Rows[0]["StokMevcut"].ToString());
           gercekMevcut = decimal.Parse(DB.GetData("select dbo.fon_StokMevcut(" + fkStokKarti.Tag.ToString() + ")").Rows[0][0].ToString());

           #region bu kısım stok kartındaki ile aynı
           string sql = @"INSERT INTO StokDevir (fkStokKarti,Tarih,Aciklama,OncekiAdet,DevirAdedi,fkKullanicilar,islemTarihi,fkDepolar)
           values(@fkStokKarti,@Tarih,@Aciklama,@OncekiAdet,@DevirAdedi,@fkKullanicilar,getdate(),1)";

           ArrayList list0 = new ArrayList();
           list0.Add(new SqlParameter("@fkStokKarti", fkStokKarti.Tag.ToString()));
           list0.Add(new SqlParameter("@Tarih", DateTime.Now));
           list0.Add(new SqlParameter("@Aciklama", "Stok Kartı Toplu Değişiklik"));
           list0.Add(new SqlParameter("@OncekiAdet", mevcut.ToString().Replace(",", ".")));

           decimal deviradet = 0;
           if (gercekMevcut < 0)
               deviradet = (gercekMevcut * -1) + mevcut;
           else
           {
               deviradet = mevcut - gercekMevcut;
           }

           list0.Add(new SqlParameter("@DevirAdedi", deviradet.ToString().Replace(",", ".")));
           list0.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

           string sonuc = DB.ExecuteSQL(sql, list0);
           if (sonuc == "0")
           {
                DB.ExecuteSQL("update StokKarti set Mevcut=0 where pkStokKarti=" + fkStokKarti.Tag.ToString());
                // + ceSimdikiMevcut.Value.ToString().Replace(",", ".") 
                DB.ExecuteSQL("update StokKartiDepo set MevcutAdet=0" +
                " where fkStokKarti=" + fkStokKarti.Tag.ToString());
           }
           else
               DB.logayaz(sonuc, sql);
            #endregion

            //Close();

            StokKartiGetir();
            StokDevirGetir();

            ceSimdikiMevcut.Focus();
            ceSimdikiMevcut.SelectAll();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            string s = formislemleri.MesajBox("Silmek istediğinize Eminmisiniz?", Degerler.mesajbaslik, 3, 2);

            if (s == "0") return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkStokDevir = dr["pkStokDevir"].ToString();
            string fkDepolar = dr["fkDepolar"].ToString();
            string deviradet = dr["DevirAdedi"].ToString();

            #region geri al

            if (int.Parse(deviradet) < 0)
            {
                DB.ExecuteSQL("update StokKartiDepo set MevcutAdet=MevcutAdet+" + deviradet.Replace("-", " ") +
                    " where fkDepolar=" + fkDepolar +
                    " and fkStokKarti=" + fkStokKarti.Tag.ToString());

                DB.ExecuteSQL("update StokKarti set Mevcut=Mevcut+" + deviradet.Replace("-", " ") +
                    " where pkStokKarti=" + fkStokKarti.Tag.ToString());
            }
            else
            {
                DB.ExecuteSQL("update StokKartiDepo set MevcutAdet=MevcutAdet-" + deviradet +
                    " where fkDepolar=" + fkDepolar +
                    " and fkStokKarti=" + fkStokKarti.Tag.ToString());

                DB.ExecuteSQL("update StokKarti set Mevcut=Mevcut-" + deviradet.Replace("-", " ") +
                   " where pkStokKarti=" + fkStokKarti.Tag.ToString());
            }
            #endregion

            DB.ExecuteSQL("delete from StokDevir where pkStokDevir=" + pkStokDevir);

            StokKartiGetir();
            StokDevirGetir();
            DepoMevcutGetir();
        }

        void DepoMevcutGetir()
        {
            if (lueDepolar.EditValue == null) return;

            DataTable dt = DB.GetData("select * from StokKartiDepo with(nolock) where fkDepolar=" + lueDepolar.EditValue.ToString()
               + " and fkStokKarti=" + fkStokKarti.Tag.ToString());
            if (dt.Rows.Count == 0)
                ceMevcut.Value = 0;
            else
                ceMevcut.Value = decimal.Parse(dt.Rows[0]["MevcutAdet"].ToString());
        }
        private void lueDepolar_EditValueChanged(object sender, EventArgs e)
        {
            if (lueDepolar.EditValue == null) return;

            string depoid = lueDepolar.EditValue.ToString();
            string stokid = fkStokKarti.Tag.ToString();
            DataTable dt = DB.GetData("select * from StokKartiDepo with(nolock) where fkDepolar=" + depoid
               + " and fkStokKarti=" + stokid);
            if (dt.Rows.Count == 0)
            {
                DB.ExecuteSQL("insert into StokKartiDepo (fkDepolar,fkStokKarti,MevcutAdet) values(" + depoid + "," + stokid + ",0)");
                ceMevcut.Value = 0;
            }
            else
            {
                string m = dt.Rows[0]["MevcutAdet"].ToString();
                decimal d = 0;
                decimal.TryParse(m, out d);
                ceMevcut.Value = d;
                ceMevcut.Tag = d.ToString();
                //ceMevcut.Value = int.Parse(dt.Rows[0]["MevcutAdet"].ToString());
            }
        }

        private void ceMevcut_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void ceSimdikiMevcut_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
                simpleButton2.Focus();
        }

        private void frmStokDepoDevir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            //string s = formislemleri.MesajBox("Silmek istediğinize Eminmisiniz?", Degerler.mesajbaslik, 3, 2);

            //if (s == "0") return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkStokDevir = dr["pkStokDevir"].ToString();

            frmStokDevirDuzenle sdd = new frmStokDevirDuzenle();
            sdd.pkStokDevir.Text = pkStokDevir;
            sdd.ShowDialog();

            StokDevirGetir();
        }
    }
}
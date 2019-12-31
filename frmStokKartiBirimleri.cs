using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using GPTS.Include.Data;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Base;

namespace GPTS
{
    public partial class frmStokKartiBirimleri : DevExpress.XtraEditors.XtraForm
    {
        public frmStokKartiBirimleri()
        {
            InitializeComponent();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        void gridyukle()
        {
            string sql = "select * from StokKarti with(nolock) where pkStokKartiid=" + pkStokKartiid.Text;
            gridControl1.DataSource = DB.GetData(sql);
        }

        public string  varsayilangetir()
        {
            string sql = "select * from StokKarti with(nolock) where pkStokKarti=" + pkStokKartiid.Text;
            DataTable dtStokKarti = DB.GetData(sql);
            if (dtStokKarti.Rows.Count == 0)
            {
                MessageBox.Show("Stok Kartı Bulunamadı");
                return "0";
            }
            else
            {
                teStokAdi.Text =
                dtStokKarti.Rows[0]["Stokadi"].ToString();
            }
            return "1";
        }

        private void frmStokKartiBirimleri_Load(object sender, EventArgs e)
        {
            repositoryItemLookUpBirim.DataSource=
            DB.GetData(@"select * from Birimler B with(nolock) where Aktif=1 order by Varsayilan");

            //lueBirimler.Properties.DataSource = repositoryItemLookUpBirim.DataSource;

            gridyukle();

            //varsayilangetir();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkStokKarti=dr["pkStokKarti"].ToString();
            if (pkStokKarti == pkStokKartiid.Text)
            {
                MessageBox.Show("Ana Birimdir!");
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Stokadi"].ToString() + " Kartını Listeden Çıkarmak İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No)
                return;

            DB.ExecuteSQL("update StokKarti set pkStokKartiid=null where pkStokKarti=" + pkStokKarti);
            //DevExpress.XtraEditors.XtraMessageBox.Show("Ürün Bilgileri Silindi.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridyukle();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
//               DataRow dr = gridView1.GetDataRow(i);
//               string pkStokKarti = dr["pkStokKarti"].ToString();
//               string Barcode = dr["Barcode"].ToString();
//               string Stokadi = dr["Stokadi"].ToString();
//               string SatisFiyati = dr["SatisFiyati"].ToString();
//               string AlisFiyati = dr["AlisFiyati"].ToString();
//               string Stoktipi = dr["Stoktipi"].ToString();
//               string fkStokGrup = dr["fkStokGrup"].ToString();

//                string sql = "";
//                string maxid = "";
//                ArrayList list = new ArrayList();
//                list.Add(new SqlParameter("@pkStokKartiid", pkStokKartiid.Text));
//                list.Add(new SqlParameter("@StokKod", Barcode));
//                list.Add(new SqlParameter("@Barcode", Barcode));
//                list.Add(new SqlParameter("@Stokadi", Stokadi));
//                list.Add(new SqlParameter("@AlisFiyati", "0.00"));
//                if (SatisFiyati == "")
//                    list.Add(new SqlParameter("@SatisFiyati", "0.00"));
//                else
//                    list.Add(new SqlParameter("@SatisFiyati", SatisFiyati.ToString().Replace(",", ".")));
//                list.Add(new SqlParameter("@Mevcut", "0"));
//                list.Add(new SqlParameter("@Asgari", "0"));
//                list.Add(new SqlParameter("@Azami", "0"));
//                list.Add(new SqlParameter("@fkMarka", "0"));
//                list.Add(new SqlParameter("@fkModel", "0"));
//                list.Add(new SqlParameter("@fkStokGrup", fkStokGrup));
//                list.Add(new SqlParameter("@BedenKodu", DBNull.Value));
//                list.Add(new SqlParameter("@RenkKodu", DBNull.Value));
//                list.Add(new SqlParameter("@Stoktipi", Stoktipi));
//                list.Add(new SqlParameter("@CikisAdet", "0"));

//                //if (pkStokKarti.ToString() == "")
//                //{
//                    sql = @"INSERT INTO StokKarti (StokKod,Stokadi,Stoktipi,Barcode,AlisFiyati,SatisFiyati,fkMarka,fkModel,fkStokGrup,Mevcut,Asgari,pkStokKartiid,CikisAdet,Aktif) 
//            VALUES(@StokKod,@Stokadi,@Stoktipi,@Barcode,@AlisFiyati,@SatisFiyati,@fkMarka,@fkModel,@fkStokGrup,@Mevcut,@Asgari,@pkStokKartiid,@CikisAdet,1) SELECT IDENT_CURRENT('StokKarti')";
//                    maxid = DB.ExecuteScalarSQL(sql, list);
//                    if (maxid == "") return;
//                    if (maxid.Substring(0, 1) == "H")
//                    {
//                        DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz." + "\n" + maxid, Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        return;
//                    }

//                    ArrayList list2 = new ArrayList();
//                    list2.Add(new SqlParameter("@SatisFiyatiKdvli", SatisFiyati.ToString().Replace(",", ".")));
//                    list2.Add(new SqlParameter("@fkStokKarti", maxid));
                    
//                    sql = "INSERT INTO SatisFiyatlari (fkStokKarti, fkSatisFiyatlariBaslik,SatisFiyatiKdvli)";
//                    sql += " VALUES(@fkStokKarti,1,@SatisFiyatiKdvli)";
                   
//                    maxid = DB.ExecuteScalarSQL(sql, list2);
                //}
//                else
//                {
//                    sql = @"UPDATE StokKarti SET StokKod=@StokKod,Stokadi=@Stokadi,Stoktipi=@Stoktipi,Barcode=@Barcode,
//            AlisFiyati=@AlisFiyati,SatisFiyati=@SatisFiyati,fkMarka=@fkMarka,fkModel=@fkModel,fkStokGrup=@fkStokGrup,
//            Mevcut=@Mevcut,Asgari=@Asgari,CikisAdet=@CikisAdet  WHERE pkStokKarti=" + pkStokKarti;
//                    DB.ExecuteSQL(sql, list);
//                    //fiyat güncelle
//                   // sql ="UPDATE SatisFiyatlari SET SatisFiyatiKdvli=@SatisFiyatiKdvli where fkStokKarti=" + pkStokKarti;
//                   // maxid = DB.ExecuteScalarSQL(sql, list2);
//                }
        }

        private void frmStokKartiBirimleri_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.F2)
                simpleButton1_Click(sender, e);
            else if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void repositoryItemLookUpBirim_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            int sb = ((DevExpress.XtraEditors.LookUpEdit)(sender)).ItemIndex+1;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.ExecuteSQL("update StokKarti set fkBirimler=" + sb + " where pkStokKarti=" + dr["pkStokKarti"].ToString());

            //string girilen = ((DevExpress.XtraEditors.LookUpEdit)(sender)).Text;
            
        }

        public bool Yeniekle()
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("",""));
            list.Add(new SqlParameter("", ""));
            list.Add(new SqlParameter("", ""));

            DB.ExecuteSQL("INSERT INTO ",list);

            return true;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (!Kontrol()) return;

            if(Yeniekle())
               varsayilangetir();

            gridyukle();
        }

        public bool Kontrol()
        {
            if (teStokAdi.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ürün Adı Boş Olamaz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                teStokAdi.Focus();
                return false;
            }
            return true;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            urunaraekle();
            //gridView1.AddNewRow();
            gridyukle();
        }

        void urunaraekle()
        {
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "2";
            StokAra.ShowDialog();
            if (StokAra.TopMost == false)
            {
                for (int i = 0; i < StokAra.gridView1.DataRowCount; i++)
                {
                    DataRow dr = StokAra.gridView1.GetDataRow(i);
                    if (dr["Sec"].ToString() == "True")
                        ekle(dr["pkStokKarti"].ToString());
                }
            }
            StokAra.Dispose();
        }
        void ekle(string pkStokKarti)
        {
            DB.ExecuteSQL("update stokKarti set pkStokKartiid="+pkStokKartiid.Text+" where pkStokKarti="  + pkStokKarti );
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }
    }
}
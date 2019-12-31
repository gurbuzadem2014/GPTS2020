using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.IO;

namespace GPTS
{
    public partial class frmStokKartiResimleri : DevExpress.XtraEditors.XtraForm
    {
        public frmStokKartiResimleri()
        {
            InitializeComponent();
        }

        private void frmucretliizin_Load(object sender, EventArgs e)
        {
            DataTable dtp = DB.GetData(@"select * from StokKarti with(nolock) where pkStokKarti=" + fkStokKarti.Text);
            this.Text = this.Text + dtp.Rows[0]["Stokadi"].ToString();

            Resimler();
        }
        void Resimler()
        {
            gridControl1.DataSource = DB.GetData("select * from StokKartiResimleri with(nolock) where fkStokKArti=" + fkStokKarti.Text);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fdialog = new OpenFileDialog();
                fdialog.Filter = "Pictures|*.jpg";
                fdialog.InitialDirectory = "C://";
                if (DialogResult.OK == fdialog.ShowDialog())
                {
                    ArrayList list = new ArrayList();
                    string sql = "";
                    sql = @"INSERT INTO StokKartiResimleri (fkStokKarti,aciklama,resimvb,tarih,fkKullanicilar)
                        values (@fkStokKarti,@aciklama,@resimvb,getdate(),-1)";

                    list.Add(new SqlParameter("@fkStokKarti", fkStokKarti.Text));
                    list.Add(new SqlParameter("@aciklama", teAciklama.Text));

                    string resimYol = fdialog.FileName;
                    
                    byte[] byteResim = null;
                    FileInfo fInfo = new FileInfo(resimYol);
                    long sayac = fInfo.Length;
                    FileStream fStream = new FileStream(resimYol, FileMode.Open, FileAccess.Read);
                    BinaryReader bReader = new BinaryReader(fStream);
                    byteResim = bReader.ReadBytes((int)sayac);

                    list.Add(new SqlParameter("@resimvb", byteResim));

                    string sonuc = DB.ExecuteSQL(sql, list);
                    if (sonuc == "0")
                        MessageBox.Show("Kayıt başarıyla gerçekleşti.");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                throw;
            }
            finally
           {
                Resimler();
           }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            UyeResimGetir(e.RowHandle);
        }

        void UyeResimGetir(int i)
        {
            //int i = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(i);
            DataTable dt=
            DB.GetData("select * from StokKartiResimleri with(nolock) where pkStokKartiResimleri=" +
            dr["pkStokKartiResimleri"].ToString());
           
            Image UrunResim = null;
            try
            {
                byte[] resim = (byte[])dt.Rows[0]["resimvb"];   //okuyucu[0];
                MemoryStream ms = new MemoryStream(resim, 0, resim.Length);
                ms.Write(resim, 0, resim.Length);
                UrunResim = Image.FromStream(ms, true);
                pictureEdit1.Image = UrunResim;
            }
            catch (Exception)
            {
                //throw;
            }
           
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.ExecuteSQL("delete from StokKartiResimleri  where pkStokKartiResimleri=" +
            dr["pkStokKartiResimleri"].ToString());

            Resimler();
        }
    }
}
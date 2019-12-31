using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmAktar : Form
    {
        DataTable dt = null;
        int clmcounter = 0;
        public frmAktar()
        {
            InitializeComponent();
        }

        private void frmAktar_Load(object sender, EventArgs e)
        {

        }

        void ExcelDosyaOku()
        {
            if (dt != null)
                dt.Dispose();
            OleDbConnection con = new OleDbConnection(@"Provider = Microsoft.Jet.OleDb.4.0 ; Data Source = " + textBox3.Text + " ; Extended Properties = Excel 8.0");
            //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
            OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
            dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
            //return dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Lütfen Dozya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName == "") return;
                textBox1.Text = openFileDialog1.FileName;
                ExcelDosyaOku();
                gridControl1.DataSource = dt;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName == "") return;
                textBox3.Text = openFileDialog1.FileName;
                ExcelDosyaOku();
                gridControl1.DataSource = dt;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
        }

        private Excel.Worksheet wrksheetData(Excel.Workbook wb, string name)
        {
            Excel.Worksheet ws = null;
            for (int x = 1; x <= wb.Sheets.Count; x++)
            {
                ws = (Excel.Worksheet)wb.Sheets[x];
                if (ws.Name.ToLower().Replace(" ", "").Contains(name.ToLower().Replace(" ", "")))
                {
                    return ws;
                }
            }
            clmcounter = 0;
            return null;
        }

        private DataTable ExcelOku(string path, string name, Boolean type)
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();

                if (xlApp == null)
                    throw new Exception("Excel Başlatılamadı. Excel Kurulu Olmaya Bilir.");


                xlApp.Visible = false;
                xlApp.DisplayAlerts = false;

                Excel.Workbook wb = xlApp.Workbooks.Open(path,
                                                            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                            Missing.Value, Missing.Value);

                int sht = wb.Sheets.Count;
                Excel.Worksheet ws = wrksheetData(wb, name);

                Excel.Range satirSayisiIcin = ws.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Missing.Value);
                ws.Cells.EntireColumn.AutoFit();

                DataTable dt = new DataTable();
                dt.TableName = ws.Name;
                Excel.Range excelRange = ws.UsedRange;
                object[,] valueArray = (object[,])excelRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                if (type)
                {
                    clmcounter = excelRange.Columns.Count;
                    for (int L = 1; L <= excelRange.Columns.Count; L++)
                    {
                        if (valueArray[1, L] == null)
                            valueArray[1, L] = "";
                        dt.Columns.Add(valueArray[1, L].ToString());
                    }

                    for (int i = 2; i <= excelRange.Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        int counter = 0;
                        for (int L = 1; L <= excelRange.Columns.Count; L++)
                        {
                            if (valueArray[i, L] == null)
                                valueArray[i, L] = "";
                            dr[counter] = valueArray[i, L].ToString();
                            counter++;
                        }
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    clmcounter = excelRange.Rows.Count;
                    for (int L = 1; L <= excelRange.Rows.Count; L++)
                    {
                        if (valueArray[L, 1] == null)
                            valueArray[L, 1] = "";
                        dt.Columns.Add(valueArray[L, 1].ToString());
                    }
                    for (int i = 2; i <= excelRange.Columns.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        int counter = 0;
                        for (int L = 1; L <= excelRange.Rows.Count; L++)
                        {
                            dr[counter] = valueArray[L, i].ToString();
                            counter++;
                        }
                        dt.Rows.Add(dr);
                    }
                }
                wb.Close();
                xlApp.Quit();
                killExcel();
                return dt;
            }
            catch //(Exception ex)
            {
                killExcel();
                clmcounter = 0;
                return null;
            }
        }

        private void killExcel()
        {
            try
            {
                Process[] pProcess;
                pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                pProcess[0].Kill();
            }
            catch (Exception ex)
            {
            }

            try
            {
                Process[] pProcess;
                pProcess = System.Diagnostics.Process.GetProcessesByName("EXCEL");
                pProcess[0].Kill();
            }
            catch (Exception ex)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cbSatis2.Checked == true)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("2.Fiyat Gurubu Eklediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;
            }
            DataTable dturunler =(DataTable) gridControl1.DataSource;
            listBox1.Items.Clear();
            int aktarilan = 0;
            int hatali = 0;
            string sql = "";
            int c = dturunler.Rows.Count;
            if (c == 0) return;
            for (int i = 0; i < c; i++)
            {
                if (DB.GetData("select pkStokKarti from StokKarti where Barcode='" + dturunler.Rows[i]["Barkod"].ToString() + "'").Rows.Count == 1) continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@StokKod", dturunler.Rows[i]["Barkod"].ToString()));
                list.Add(new SqlParameter("@Barcode", dturunler.Rows[i]["Barkod"].ToString()));
                string grupadi = "1";// dturunler.Rows[i]["grub"].ToString();
                string pkStokGrup = "0";
                //DataTable dtgrup = DB.GetData("select pkStokGrup from StokGruplari where StokGrup='" + grupadi + "'");
                //if (dtgrup.Rows.Count == 0)
               // {
               //     DB.ExecuteSQL("INSERT INTO StokGruplari (StokGrup,WebdeGoster,Aktif,SortID,Gonderildi) VALUES('" + grupadi + "',0,1,0,0)");
               //     pkStokGrup = DB.GetData("select Max(pkStokGrup) from StokGruplari").Rows[0][0].ToString();
               // }
               // else
                 //   pkStokGrup = dtgrup.Rows[0]["pkStokGrup"].ToString();
                list.Add(new SqlParameter("@fkStokGrup", pkStokGrup));
                list.Add(new SqlParameter("@Stokadi", dturunler.Rows[i]["Adi"].ToString()));
                string Mevcut = "0";
                //if (dturunler.Rows[i]["Nakit"].ToString() == "0,0000")
                    Mevcut = "0";//  list.Add(new SqlParameter("@Mevcut", "0"));
               // else
                   // Mevcut = dturunler.Rows[i]["Nakit"].ToString().Replace(",", "."); //list.Add(new SqlParameter("@Mevcut", Double.Parse(dturunler.Rows[i]["Stok"].ToString().Replace(",", "."))));
                if (dturunler.Rows[i]["Alis"].ToString() == "0,0000")
                    list.Add(new SqlParameter("@AlisFiyati", "0"));
                else
                    list.Add(new SqlParameter("@AlisFiyati", dturunler.Rows[i]["Alis"].ToString().Replace(",", ".")));
                decimal sf = 0;
                decimal.TryParse(dturunler.Rows[i]["Satis"].ToString(), out sf);
                //if (dturunler.Rows[i]["satis"].ToString() == "0,0000")
                //   list.Add(new SqlParameter("@SatisFiyati", "0"));
                //else
                list.Add(new SqlParameter("@SatisFiyati", sf.ToString().Replace(",", ".")));//dturunler.Rows[i]["satis"].ToString().Replace(",", ".")));
                list.Add(new SqlParameter("@Stoktipi", ""));
                list.Add(new SqlParameter("@KdvOrani", dturunler.Rows[i]["Kdv"].ToString()));
                //if (dturunler.Rows[i]["durumu"].ToString() == "AKTİF")
                //    list.Add(new SqlParameter("@Aktif", "1"));
                //else
                    list.Add(new SqlParameter("@Aktif", "1"));
                string KritikMiktar = "0";
               // if (dturunler.Rows[i]["kritikstok"].ToString() == "0,0000")
                    KritikMiktar = "0";//list.Add(new SqlParameter("@KritikMiktar", "0"));
                //else
                //    KritikMiktar = dturunler.Rows[i]["kritikstok"].ToString().Replace(",", ".");//list.Add(new SqlParameter("@KritikMiktar", dturunler.Rows[i]["kritikstok"].ToString().Replace(",", ".")));

                if (KritikMiktar == "") KritikMiktar = "0";
                sql = "INSERT INTO StokKarti (StokKod,Barcode,fkStokGrup,Stokadi,Stoktipi,Mevcut,AlisFiyati,SatisFiyati,KdvOrani,Aktif,KritikMiktar)" +
                " values(@StokKod,@Barcode,@fkStokGrup,@Stokadi,@Stoktipi," + Mevcut + ",@AlisFiyati,@SatisFiyati,@KdvOrani,@Aktif," + KritikMiktar + ") select IDENT_CURRENT('StokKarti')";
                try
                {
                    string pkStokKarti = DB.ExecuteScalarSQL(sql, list);
                    if (pkStokKarti.Substring(0, 1) == "H")
                    {
                        //listBox1.Items.Add(dturunler.Rows[i]["gelis"].ToString() + pkStokKarti);
                        continue;
                    }
                    aktarilan++;
                    DB.ExecuteSQL("update StokKarti set HizliSatisAdi=substring(StokAdi,0,19)");
                    DB.ExecuteSQL("insert into SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,Aktif)" +
                        " values(" + pkStokKarti + ",1," + dturunler.Rows[i]["Satis"].ToString().Replace(",", ".") + ",0,0,1)");
                    if (cbSatis2.Checked)
                        DB.ExecuteSQL("insert into SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,Aktif)" +
        " values(" + pkStokKarti + ",2," + dturunler.Rows[i]["Kredi"].ToString().Replace(",", ".") + ",0,0,1)");
                }
                catch (Exception exp)
                {
                    hatali++;
                }
            }
            DB.ExecuteSQL("update StokKarti set HizliSatisAdi=rtrim(HizliSatisAdi)");
            DB.ExecuteSQL("update StokKarti set UreticiKodu=Barcode where UreticiKodu is null");
            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());
        }

        void ExcelDosyaOku2()
        {
            if (dt != null)
                dt.Dispose();
            OleDbConnection con = new OleDbConnection(@"Provider = Microsoft.Jet.OleDb.4.0 ; Data Source = " + textBox5.Text + " ; Extended Properties = Excel 8.0");
            //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
            OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
            dt = new DataTable();
            da.Fill(dt);
            gridControl2.DataSource = dt;
            //return dt;
        }
        void ExcelDosyaOku3()
        {
            if (dt != null)
                dt.Dispose();
            OleDbConnection con = new OleDbConnection(@"Provider = Microsoft.Jet.OleDb.4.0 ; Data Source = " + textBox3.Text + " ; Extended Properties = Excel 8.0");
            //excel dosyasını oluşturup tüm alanları seçtikten sonra vermiş olduğumuz isimi yani Calisanlar bilgisini burada sorgumuzda belirtiyoruz.
            OleDbCommand cmd = new OleDbCommand("select * from [Sayfa1$]", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con.ConnectionString);
            dt = new DataTable();
            da.Fill(dt);
            gridControl3.DataSource = dt;
            //return dt;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName == "") return;
                textBox5.Text = openFileDialog1.FileName;
                ExcelDosyaOku2();
                gridControl2.DataSource = dt;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
        }

        private void BakiyeYaz(string pkFirma,decimal borc)
        {
            string sql = @"INSERT INTO KasaHareket (fkKasalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli)
             values(@fkKasalar,0,getdate(),3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Bakiye Düzeltme')";
            ArrayList list0 = new ArrayList();
            list0.Add(new SqlParameter("@fkKasalar", "1"));//int.Parse(lueKasa.EditValue.ToString())));
            if (borc < 0)
            {
                list0.Add(new SqlParameter("@Alacak", borc.ToString().Replace(",", ".").Replace("-", "")));
                list0.Add(new SqlParameter("@Borc", "0"));
            }
            else
            {
                list0.Add(new SqlParameter("@Alacak", "0"));
                list0.Add(new SqlParameter("@Borc", borc.ToString().Replace(",", ".")));
            }
            list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
            list0.Add(new SqlParameter("@Aciklama", "Bakiye Sıfırlandı.Aktarım"));
            list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
            list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
            list0.Add(new SqlParameter("@fkFirma", pkFirma));
            list0.Add(new SqlParameter("@AktifHesap", "0"));
            string sonuc = DB.ExecuteSQL(sql, list0);
            if (sonuc != "0")
            {
                //ceBakiye.Value = 0;
                // MessageBox.Show("Hata Oluştu Tekrar deneyiniz");
                return;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DataTable dtexcel = (DataTable)gridControl2.DataSource;
            int aktarilan = 0;
            int hatali = 0;
            string sql = "";
            int c = dtexcel.Rows.Count;
            if (c == 0) return;

            for (int i = 0; i < c; i++)
			{
                if (DB.GetData("select * from Firmalar where Firmaadi='" + dtexcel.Rows[i]["Ad"].ToString() + "'").Rows.Count == 1) continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Firmaadi", dtexcel.Rows[i]["Ad"].ToString()));
                list.Add(new SqlParameter("@Yetkili", "''"));
                list.Add(new SqlParameter("@Adres",  dtexcel.Rows[i]["Mahalle"].ToString() + " " + dtexcel.Rows[i]["Cadde"].ToString()
                    + " " + dtexcel.Rows[i]["Sokak"].ToString() + " " +
                    dtexcel.Rows[i]["KapiNo"].ToString()+" "+
                    dtexcel.Rows[i]["ilce"].ToString()));
                list.Add(new SqlParameter("@Cep", dtexcel.Rows[i]["Cep"].ToString()));
                list.Add(new SqlParameter("@Cep2", dtexcel.Rows[i]["Cep2"].ToString()));
                list.Add(new SqlParameter("@Tel", dtexcel.Rows[i]["Tel"].ToString()));
                list.Add(new SqlParameter("@Tel2", dtexcel.Rows[i]["Tel2"].ToString()));
                list.Add(new SqlParameter("@OzelKod", dtexcel.Rows[i]["OzelKod"].ToString()));// i + 100));
                //list.Add(new SqlParameter("@Devir", dtexcel.Rows[i]["toplamborcu"].ToString().Replace(",", ".")));
                sql = "INSERT INTO Firmalar (Firmaadi,Yetkili,Cep,Cep2,OzelKod,Adres,Tel,Tel2,fkil,fkilce)" +
                " values(@Firmaadi,@Yetkili,@Cep,@Cep2,@OzelKod,@Adres,@Tel,@Tel2,41,1) SELECT IDENT_CURRENT('Firmalar')"; 
                try
                {
                    decimal bakiye = 0;
                    decimal.TryParse(dtexcel.Rows[i]["Bakiye"].ToString(),out bakiye);
                    string pkFirma = DB.ExecuteScalarSQL(sql, list);
                    if (bakiye!=0)
                        BakiyeYaz(pkFirma, bakiye);
                    
                }
                catch (Exception exp)
                {
                    hatali++;
                }
			}
            DB.ExecuteSQL("UPDATE Firmalar SET OzelKod=pkFirma where OzelKod is null");
            DB.ExecuteSQL("UPDATE Firmalar SET Aktif=1");
            DB.ExecuteSQL("update Firmalar set Tel=replace(Tel,' ','')");
            DB.ExecuteSQL("update Firmalar set Cep=replace(Cep,' ','')");
            DB.ExecuteSQL("update Firmalar set Tel2=replace(Tel2,' ','')");
            DB.ExecuteSQL("update Firmalar set Tel='0262'+Tel where len(Tel)=7");
            DB.ExecuteSQL(@"update Firmalar set fkFirmaGruplari=1,fkFirmaAltGruplari=0,
fkil=7,fkilce=0,fkSirket=1,LimitBorc=0,Borc=0,Alacak=0,Bonus=0");
            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Ürünler Aktarılmadı.)=" + hatali.ToString());

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Lütfen Dosya Seçiniz";
                openFileDialog1.Filter = " (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1; // varsayılan olarak xls uzantıları göster
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName == "") return;
                textBox3.Text = openFileDialog1.FileName;
                ExcelDosyaOku3();
                gridControl3.DataSource = dt;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu:" + exp.Message.ToString());
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DataTable dtexcel = (DataTable)gridControl3.DataSource;
            int aktarilan = 0;
            int hatali = 0;
            string sql = "";
            int c = dtexcel.Rows.Count;
            if (c == 0) return;

            for (int i = 0; i < c; i++)
            {
                if (DB.GetData("select * from Tedarikciler where Firmaadi='" + dtexcel.Rows[i]["Ad"].ToString() + "'").Rows.Count == 1) continue;
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Firmaadi", dtexcel.Rows[i]["Ad"].ToString()));
                list.Add(new SqlParameter("@Yetkili", "''"));
                list.Add(new SqlParameter("@Adres", dtexcel.Rows[i]["Mahalle"].ToString() + " " + dtexcel.Rows[i]["Cadde"].ToString()
                    + " " + dtexcel.Rows[i]["Sokak"].ToString() + " " +
                    dtexcel.Rows[i]["KapiNo"].ToString() + " " +
                    dtexcel.Rows[i]["ilce"].ToString()));
                list.Add(new SqlParameter("@Cep", dtexcel.Rows[i]["Cep"].ToString()));
                list.Add(new SqlParameter("@Cep2", dtexcel.Rows[i]["Cep2"].ToString()));
                list.Add(new SqlParameter("@Tel", dtexcel.Rows[i]["Tel"].ToString()));
                list.Add(new SqlParameter("@Tel2", dtexcel.Rows[i]["Tel2"].ToString()));
                list.Add(new SqlParameter("@OzelKod", dtexcel.Rows[i]["OzelKod"].ToString()));// i + 100));
                //list.Add(new SqlParameter("@Devir", dtexcel.Rows[i]["toplamborcu"].ToString().Replace(",", ".")));
                sql = "INSERT INTO Tedarikciler (Firmaadi,Yetkili,Cep,Cep2,OzelKod,Adres,Tel,Tel2,fkil,fkilce)" +
                " values(@Firmaadi,@Yetkili,@Cep,@Cep2,@OzelKod,@Adres,@Tel,@Tel2,41,1) SELECT IDENT_CURRENT('Tedarikciler')";
                try
                {
                    //decimal bakiye = 0;
                    //decimal.TryParse(dtexcel.Rows[i]["Bakiye"].ToString(), out bakiye);
                    string pkFirma = DB.ExecuteScalarSQL(sql, list);
                    //if (bakiye != 0)
                    //    BakiyeYaz(pkFirma, bakiye);

                }
                catch (Exception exp)
                {
                    hatali++;
                }
            }
            DB.ExecuteSQL("UPDATE Tedarikciler SET OzelKod=pkFirma where OzelKod is null");
            DB.ExecuteSQL("UPDATE Tedarikciler SET Aktif=1");
            DB.ExecuteSQL("update Tedarikciler set Tel=replace(Tel,' ','')");
            DB.ExecuteSQL("update Tedarikciler set Cep=replace(Cep,' ','')");
            DB.ExecuteSQL("update Tedarikciler set Tel2=replace(Tel2,' ','')");
            DB.ExecuteSQL("update Tedarikciler set Tel='0262'+Tel where len(Tel)=7");
            DB.ExecuteSQL(@"update Tedarikciler set fkFirmaGruplari=1,
fkil=7,fkilce=0,fkSirket=1,LimitBorc=0,Borc=0,Alacak=0");
            MessageBox.Show("Aktarılan Kayıt=" + aktarilan.ToString() + " Hatalı Kayıt (Aynı Tedarikçi Aktarılmadı.)=" + hatali.ToString());

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data; 

namespace GPTS.islemler
{
    public class ExceleGonder
    {
        public static void datatabletoexel(string satis_id)
        {
            System.Data.DataTable dt = DB.GetData(@"select sk.Barcode,sk.Stokadi,sd.Adet,sk.AlisFiyati,sd.SatisFiyati,sg.StokGrup,m.Marka,
sk.KdvOrani,pkStokKarti,isnull(sf2.SatisFiyatiKdvli,sd.SatisFiyati) as SatisFiyatiKdvli
 from SatisDetay sd with(nolock)
            left join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
            left join SatisFiyatlari sf2 with(nolock) on sf2.fkStokKarti=sd.fkStokKarti and  sf2.fkSatisFiyatlariBaslik=(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik with(nolock) where Tur=2)
            left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
            left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka
            where fkSatislar=" + satis_id); 

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 0;
            int j = 0;

            xlWorkSheet.Cells[1, 1] = "BARKOD";
            xlWorkSheet.Cells[1, 2] = "STOKADI";
            xlWorkSheet.Cells[1, 3] = "MEVCUT";
            xlWorkSheet.Cells[1, 4] = "ALISFIYATI";
            xlWorkSheet.Cells[1, 5] = "SATISFIYATI";
            xlWorkSheet.Cells[1, 6] = "STOKGRUBU";
            xlWorkSheet.Cells[1, 7] = "MARKA";
            xlWorkSheet.Cells[1, 8] = "KDVORANI";
            xlWorkSheet.Cells[1, 9] = "STOKKODU";
            xlWorkSheet.Cells[1, 10] = "SATISFIYATI2";
            
            for (i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    //DataGridViewCell cell = dataGridView1[j, i];
                    //xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
                    xlWorkSheet.Cells[i + 2, j + 1] = dt.Rows[i][j];
                }
            }
            string ExcelYol = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            DataTable dtYol = DB.GetData("select * from  ayarlar where Ayar20='excelyol'");
            if (dtYol.Rows.Count > 0)
                ExcelYol = dtYol.Rows[0]["Ayar50"].ToString();

            if(!Directory.Exists(ExcelYol))
            {
                formislemleri.Mesajform("Dosya yolu bulunamadı" + ExcelYol, "K", 200);
                return;
            }

            // TextReader ini = File.OpenText(exeyol + "\\baglanti.ini");
            try
            {
                xlWorkBook.SaveAs(ExcelYol + "\\satisfisi" + satis_id + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            }
            catch (Exception exp)
            {
                formislemleri.Mesajform("Excel Dosyası Oluşturulamadı "+ exp.Message, "K", 150);
                //throw;
                return;
            }
           
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            System.Windows.Forms.MessageBox.Show(ExcelYol + "\\satisfisi" + satis_id + ".xls Excel Dosyası Oluşturuldu");
        }

        public static void datatableStoktoexel()
        {
            System.Data.DataTable dt = DB.GetData(@"select sk.Barcode,sk.Stokadi,sk.Mevcut,sk.AlisFiyati,sk.SatisFiyati,sg.StokGrup,m.Marka,
sk.KdvOrani,pkStokKarti,
case 
when sf2.SatisFiyatiKdvli is null then sk.SatisFiyati
when sf2.SatisFiyatiKdvli=0 then sk.SatisFiyati
else 
sf2.SatisFiyatiKdvli end SatisFiyatiKdvli
 from StokKarti sk with(nolock) 
            left join SatisFiyatlari sf2 with(nolock) on sf2.fkStokKarti=sk.pkStokKarti and  sf2.fkSatisFiyatlariBaslik=(select pkSatisFiyatlariBaslik from SatisFiyatlariBaslik with(nolock) where Tur=2)
            left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
            left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka
            where 1=1 and sk.Mevcut>0");

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 0;
            int j = 0;

            xlWorkSheet.Cells[1, 1] = "BARKOD";
            xlWorkSheet.Cells[1, 2] = "STOKADI";
            xlWorkSheet.Cells[1, 3] = "MEVCUT";
            xlWorkSheet.Cells[1, 4] = "ALISFIYATI";
            xlWorkSheet.Cells[1, 5] = "SATISFIYATI";
            xlWorkSheet.Cells[1, 6] = "STOKGRUBU";
            xlWorkSheet.Cells[1, 7] = "MARKA";
            xlWorkSheet.Cells[1, 8] = "KDVORANI";
            xlWorkSheet.Cells[1, 9] = "STOKKODU";
            xlWorkSheet.Cells[1, 10] = "SATISFIYATI2";

            for (i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    //DataGridViewCell cell = dataGridView1[j, i];
                    //xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
                    xlWorkSheet.Cells[i + 2, j + 1] = dt.Rows[i][j];
                }
            }
            string ExcelYol = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            DataTable dtYol = DB.GetData("select * from  ayarlar where Ayar20='excelyol'");
            if (dtYol.Rows.Count > 0)
                ExcelYol = dtYol.Rows[0]["Ayar50"].ToString();

            if (!Directory.Exists(ExcelYol))
            {
                formislemleri.Mesajform("Dosya yolu bulunamadı" + ExcelYol, "K", 200);
                return;
            }

            // TextReader ini = File.OpenText(exeyol + "\\baglanti.ini");
            try
            {
                xlWorkBook.SaveAs(ExcelYol + "\\stoklar.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            }
            catch (Exception exp)
            {
                formislemleri.Mesajform("Excel Dosyası Oluşturulamadı"+ exp.Message, "K", 150);
                //throw;
                return;
            }

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            System.Windows.Forms.MessageBox.Show(ExcelYol + "\\stoklar.xls Excel Dosyası Oluşturuldu");
        }
        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                System.Windows.Forms.MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}

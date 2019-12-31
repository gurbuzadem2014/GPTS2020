using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace GPTS.islemler
{
    public class Yazdirma_islemleri
    {
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
        public void GridYazdir(GridControl gridControl1, string kagit_turu)
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gridControl1;
            printableLink.Landscape = true;

            if (kagit_turu=="A4")
               printableLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
           // else
            //printableLink.PaperKind = System.Drawing.Printing.PaperKind.A4Extra;

            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        public void GridTasarimYukle(GridView gridview, string dosyaadi)
        {
            string Dosya = DB.exeDizini + "\\" + dosyaadi + ".xml";

            if (File.Exists(Dosya))
            {
                gridview.RestoreLayoutFromXml(Dosya);
                gridview.ActiveFilter.Clear();
            }
        }
        public void GridTasarimKaydet(GridView gridview, string dosyaadi)
        {
            string Dosya = DB.exeDizini + "\\" + dosyaadi + ".xml";

            //if (File.Exists(Dosya))
            //{
                gridview.SaveLayoutToXml(Dosya);
                //gridview.ActiveFilter.Clear();
            //}
        }

        public void GridTasarimSil(string dosyaadi)
        {
            string Dosya = DB.exeDizini + "\\" + dosyaadi + ".xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
                //gridview.SaveLayoutToXml(Dosya);
                //gridview.ActiveFilter.Clear();
            }
        }
    }
}

using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking2010.Views;
using HTT.WinForm.Tanimlar;

namespace HTT.WinForm
{
    public partial class RibbonAnaForm : DevExpress.XtraBars.Ribbon.RibbonForm, 
        IDocumentsHostWindow
    {
        DocumentManager manager;
        public RibbonAnaForm()
        {
            InitializeComponent();
            manager = new DocumentManager();            
            manager.MdiParent = this;
            
            manager.View.FloatingDocumentContainer = FloatingDocumentContainer.DocumentsHost;
            manager.View.CustomDocumentsHostWindow += View_CustomDocumentsHostWindow;
        }

        private void View_CustomDocumentsHostWindow(object sender, CustomDocumentsHostWindowEventArgs e) {
            e.Constructor = new DocumentsHostWindowConstructor(CreateMyHost);
        }
        private RibbonAnaForm CreateMyHost()
        {
            return new RibbonAnaForm();
        }

        private void barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            CreateForms(e.Item.Caption);
        }

        public bool DestroyOnRemovingChildren {
            get { return true; }
        }

        public DocumentManager DocumentManager {
            get { return manager; }
        }

       
        void CreateForms(string category) {
            //RemovePreviousForms();
            new FormChild() { MdiParent = this, Text = category  }.Show();
            //for (int i = 1; i <= 5; i++)
            //    new FormChild() { MdiParent = this, Text = category + " - " + i}.Show();
        }

        void CreateForms2(string category)
        {
            //RemovePreviousForms();
            new frmSirketler() { MdiParent = this, Text = category }.Show();
            //for (int i = 1; i <= 5; i++)
            //    new FormChild() { MdiParent = this, Text = category + " - " + i}.Show();
        }
        private void RemovePreviousForms() {
            for (int i = DocumentManager.View.Documents.Count - 1; i >= 0; i--)
                DocumentManager.View.Documents[i].Form.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateForms2("Şirket");// e.Item.Caption);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                manager.Dispose();
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GPTS
{
    public partial class frmProjeGorevGirisi : DevExpress.XtraEditors.XtraForm
    {
        public frmProjeGorevGirisi()
        {
            InitializeComponent();
        }

        private void frmPersonelEvrak_Load(object sender, EventArgs e)
        {
            lUProjeler.Properties.DataSource = DB.GetData("select * from Projeler with(nolock)");

            gridControl1.DataSource = DB.GetData(@"
            SELECT GP.pkGorevliPersoneller,GP.fkProjeGorev,P.pkpersoneller,P.adi,P.soyadi FROM GorevliPersoneller GP WITH(NOLOCK)
LEFT JOIN Personeller P WITH(NOLOCK) ON P.pkpersoneller=GP.fkPersoneller");
        }

        private void lUProjeler_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("INSERT INTO ");
        }
    }
}
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
    public partial class frmSiparisSablonFiyatlari : DevExpress.XtraEditors.XtraForm
    {
        public frmSiparisSablonFiyatlari()
        {
            InitializeComponent();
        }

        private void gcFiyatlar_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmSiparisSablonFiyatlari_Load(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where pkFirma=" + pkFirma.Text);
            string firmadi = dt.Rows[0]["Firmaadi"].ToString();
            string ozelkod = dt.Rows[0]["OzelKod"].ToString();
            baslik.Text = ozelkod + "-" + firmadi;

            gcFiyatlar.DataSource=
            DB.GetData(@"select sf.pkSatisFiyatlari,Baslik as FiyatAdi,sf.SatisFiyatiKdvli,sk.Stokadi from Firmalar f
left join SatisFiyatlariBaslik sfb on sfb.pkSatisFiyatlariBaslik=f.fkSatisFiyatlariBaslik
left join SatisFiyatlari sf on sf.fkSatisFiyatlariBaslik=f.fkSatisFiyatlariBaslik
left join StokKarti sk on sk.pkStokKarti=sf.fkStokKarti
where pkFirma=" + pkFirma.Text);


        }
    }
}
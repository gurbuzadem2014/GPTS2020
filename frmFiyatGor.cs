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
    public partial class frmFiyatGor : DevExpress.XtraEditors.XtraForm
    {
        public frmFiyatGor()
        {
            InitializeComponent();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }
        void SatisFiyatlari(string pkStokKarti)
        {
            string sql = @"SELECT sf.pkSatisFiyatlari, sf.SatisFiyatiKdvli, 
sf.SatisFiyatiKdvsiz, sfb.Baslik,sf.Aktif,sf.fkSatisFiyatlariBaslik,sf.iskontoYuzde,
            sf.YeniFiyatKdvli,sf.YeniFiyatKdvsiz,
			sf.SatisFiyatiKdvli-((sf.SatisFiyatiKdvli*sk.KdvOrani)/(100+sk.KdvOrani)) as kdvharic,
sf.SatisFiyatiKdvli-((sf.SatisFiyatiKdvli*sk.satis_iskonto)/100) as iskontolu
			 FROM  SatisFiyatlari sf with(nolock)
			INNER JOIN  SatisFiyatlariBaslik sfb with(nolock) ON sf.fkSatisFiyatlariBaslik = sfb.pkSatisFiyatlariBaslik
			INNER JOIN  StokKarti sk with(nolock) ON sf.fkStokKarti = sk.pkStokKarti
            WHERE sf.fkStokKarti = " + pkStokKarti + " ORDER BY sf.fkSatisFiyatlariBaslik";
            gridControl1.DataSource = DB.GetData(sql);
        
            bandedGridView1.FocusedColumn = bandedGridView1.Columns["SatisFiyatiKdvli"];
        }
        private void frmFiyatGor_Load(object sender, EventArgs e)
        {
            okunanbarkod.Text = "";
            adi.Text = "";
            fiyati.Text = "0,00";
        }

        private void BARKOD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
            DataTable dt;
            if (e.KeyCode == Keys.Enter)
            {
                 dt = DB.GetData(@"SELECT sk.pkStokKarti,sf.SatisFiyatiKdvli, sk.Barcode, sk.Stokadi,sfb.Baslik,
sf.SatisFiyatiKdvli-((sf.SatisFiyatiKdvli*sk.satis_iskonto)/100) as iskontolusatisfiyati
FROM   StokKarti sk with(nolock) 
LEFT JOIN SatisFiyatlari sf with(nolock) ON sk.pkStokKarti = sf.fkStokKarti
LEFT JOIN SatisFiyatlariBaslik sfb with(nolock) ON sfb.pkSatisFiyatlariBaslik = sf.fkSatisFiyatlariBaslik
                        WHERE sk.Barcode = '" + BARKOD.Text + "' ");
                //çoklu barkodlarada bak
                if (dt.Rows.Count == 0)
                {
                    dt = DB.GetData(@"SELECT sk.pkStokKarti, sf.SatisFiyatiKdvli, sk.Barcode, sk.Stokadi,sfb.Baslik,
sf.SatisFiyatiKdvli-((sf.SatisFiyatiKdvli*sk.satis_iskonto)/100) as iskontolusatisfiyati
 FROM StokKartiBarkodlar skb with(nolock)
left join StokKarti sk with(nolock) on sk.pkStokKarti=skb.fkStokKarti
LEFT JOIN SatisFiyatlari sf with(nolock) ON sk.pkStokKarti = sf.fkStokKarti
LEFT JOIN SatisFiyatlariBaslik sfb with(nolock) ON sfb.pkSatisFiyatlariBaslik = sf.fkSatisFiyatlariBaslik
                        WHERE skb.Barkod = '" + BARKOD.Text + "' ");
                }

                if (dt.Rows.Count == 0)
                {
                    frmMesajBox mesaj = new frmMesajBox(200);
                    mesaj.label1.Text = "Stok Bulunamadı";
                    mesaj.Show();
                    BARKOD.Focus();
                    BARKOD.Text = "";
                    BARKOD.Focus();
                }
                else
                {
                    string pkStokKarti = dt.Rows[0]["pkStokKarti"].ToString();
                    fiyati.Text = dt.Rows[0]["SatisFiyatiKdvli"].ToString();
                    adi.Text = dt.Rows[0]["Stokadi"].ToString();
                    okunanbarkod.Text = dt.Rows[0]["Barcode"].ToString();
                    SatisFiyatlari(pkStokKarti);
                    //fiyati.Text = Convert.ToDouble(fiyati.Text).ToString("##0.00");
                    BARKOD.Text = "";
                    BARKOD.Focus();
                }
            }
        }
    }
}
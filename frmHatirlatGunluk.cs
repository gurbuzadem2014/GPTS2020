using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using System.Threading;

namespace GPTS
{
    public partial class frmHatirlatGunluk : DevExpress.XtraEditors.XtraForm
    {
        public frmHatirlatGunluk()
        {
            InitializeComponent();
        }


        private void frmCikis_Load(object sender, EventArgs e)
        {
            GunlukHatirlatmalar();

            label1.Text = "Gönderilecek E-Posta :  "+Degerler.eposta;

            Thread thread1 = new Thread(new ThreadStart(epostagonder));
            thread1.Start();
        }

        void GunlukHatirlatmalar()
        {
            string sql = "";
            //@"select pkHatirlatma,Konu,Tarih,Aciklama,K.KullaniciAdi,K.adisoyadi,H.Kategori,H.Uyar,H.EpostaGonder,
            //F.Firmaadi,H.fkFirma from Hatirlatma H with(nolock)
            //left join Kullanicilar K ON K.pkKullanicilar=H.fkKullanicilar 
            //left join Firmalar F ON F.pkFirma=H.fkFirma 
            //where Tarih<getdate()+1 and H.Uyar=1";

            sql= @"select pkHatirlatma,
f.Firmaadi+'-'+Aciklama as [Subject],Tarih as StartTime,
BitisTarihi as EndTime,Konu as [Description],0 as AllDay,fkfirma,h.eposta_mesaj,h.EpostaGonder,'H' as HT  from Hatirlatma h with(nolock)
left join Firmalar f with(nolock) on pkFirma=h.fkFirma
where H.Uyar=1  and Tarih<getdate()+1
union all
select tl.pkTaksitler,f.Firmaadi+'-'+t.aciklama as [Subject],tl.Tarih as StartTime,
DATEADD(MINUTE,+30,tl.Tarih) as EndTime,'Taksit Ödemesi' as [Description],0 as AllDay,t.fkfirma,'Taksit',0,'T' from Taksit t with(nolock)
left join Taksitler tl with(nolock) on tl.taksit_id=t.taksit_id
left join Firmalar f with(nolock) on f.pkFirma=t.fkFirma
where tl.Tarih<getdate()+1 and tl.Odenecek<>tl.Odenen";

            gridControl1.DataSource = DB.GetData(sql);

            
        }
        void epostagonder()
        {
            
            for (int i = 0; i < gridView1.DataRowCount; i++)
			{
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["EpostaGonder"].ToString() == "True")
                {
                    DB.epostagonder(Degerler.eposta, dr["Description"].ToString(), "", dr["Subject"].ToString());
                    //hatırlatmamamı taksit mi?
                    if (dr["HT"].ToString() == "H")
                      DB.ExecuteSQL("update Hatirlatma set EpostaGonder=0,eposta_mesaj='E-Posta Gönderildi' where pkHatirlatma=" + dr["pkHatirlatma"].ToString());
                }
			}
            
        }
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string HT = dr["HT"].ToString();
            if (HT == "H")
            {
                frmHatirlatma Hatirlat = new frmHatirlatma(DateTime.Now, DateTime.Now, 0);
                string pkHatirlatma = dr["pkHatirlatma"].ToString();
                DB.pkHatirlatma = int.Parse(pkHatirlatma);
                Hatirlat.ShowDialog();
            }
            else if (HT == "T")
            {
                frmUcGoster SatisGoster = new frmUcGoster(2,"0");
                SatisGoster.ShowDialog();  
                //string pkHatirlatma = dr["pkHatirlatma"].ToString();
                //DB.pkHatirlatma = int.Parse(pkHatirlatma);
            }

            //DB.ExecuteSQL("update Hatirlatma set Uyar=1 where pkHatirlatma=" + pkHatirlatma);
            GunlukHatirlatmalar();
        }

        private void yeniHatırlatmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHatirlatma Hatirlat = new frmHatirlatma(DateTime.Now, DateTime.Now, 0);
            DB.pkHatirlatma = 0;
            Hatirlat.ShowDialog();

            GunlukHatirlatmalar();
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmHatirlatma Hatirlat = new frmHatirlatma(DateTime.Now, DateTime.Now, 0);
            string pkHatirlatma = dr["pkHatirlatma"].ToString();
            DB.pkHatirlatma = int.Parse(pkHatirlatma);
            Hatirlat.ShowDialog();
            //DB.ExecuteSQL("update Hatirlatma set Uyar=1 where pkHatirlatma=" + pkHatirlatma);
            GunlukHatirlatmalar();
        }

        private void hatırlatmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkHatirlatma = dr["pkHatirlatma"].ToString();

            DB.ExecuteSQL("update Hatirlatma set Uyar=0 where pkHatirlatma=" + pkHatirlatma);

            GunlukHatirlatmalar();
        }

        private void ePostaGönderSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilenlere E-Posta Gönderilecek! Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;

            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));

                string pkHatirlatma = dr["pkHatirlatma"].ToString();

                DB.ExecuteSQL("update Hatirlatma set EpostaGonder=1 where pkHatirlatma=" + pkHatirlatma);
            }
            GunlukHatirlatmalar();
        }

        private void ePostaGönderSeçmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilenlere E-Posta Gönderilmeyecek! Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;

            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));

                string pkHatirlatma = dr["pkHatirlatma"].ToString();

                DB.ExecuteSQL("update Hatirlatma set EpostaGonder=0 where pkHatirlatma=" + pkHatirlatma);
            }
            GunlukHatirlatmalar();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            GunlukHatirlatmalar();
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            frmRandevuVer RandevuVer = new frmRandevuVer();
            RandevuVer.Show();
        }

    }
}
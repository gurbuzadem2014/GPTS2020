using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class frmTransferRaporu : DevExpress.XtraEditors.XtraForm
    {
        public frmTransferRaporu()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            string sql = "exec sp_VezneRaporlari '" + ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00") +
                "','"+sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")+"'";

            gridControl1.DataSource = DB.GetData(sql);
        }

        private void frmVezneRaporu_Load(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 0;
            //gcKul.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where durumu=1");
            gridControl2.DataSource = DB.GetData(@"select pkKasaHareket,KH.fkKasalar,Tarih,Tutar as OlmasiGereken,0 as KasadakiPara,0 as Fark,Aciklama,Borc,Alacak,fkKullanicilar,K.KullaniciAdi from KasaHareket KH with(nolock)
left join Kullanicilar K with(nolock) on K.pkKullanicilar = KH.fkKullanicilar
where OdemeSekli = 'Transfer' or OdemeSekli = 'Kasa Bakiye Düzeltme'");

            //gridControl2.DataSource = DB.GetData("select top 10 * from KasaHareket with(nolock) order by 1 desc");
        }

        private DateTime getStartOfWeek(bool useSunday)
        {
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;

            if (!useSunday)
                dayOfWeek--;

            if (dayOfWeek < 0)
            {// day of week is Sunday and we want to use Monday as the start of the week
                // Sunday is now the seventh day of the week
                dayOfWeek = 6;
            }

            return now.AddDays(-1 * (double)dayOfWeek);
        }

        private void sorguTarihAraligi(int g1, int m1, int y1, int h1, int min1, int sec1, Boolean p1,
                       int g2, int m2, int y2, int h2, int min2, int sec2, Boolean p2)
        {

            DateTime d1 = DateTime.Now;

            d1 = d1.AddDays(g1);
            d1 = d1.AddMonths(m1);
            d1 = d1.AddYears(y1);
            d1 = d1.AddHours(h1);
            d1 = d1.AddMinutes(min1);
            ilktarih.DateTime = d1.AddSeconds(sec1);
            ilktarih.ToolTip = ilktarih.DateTime.ToString();

            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih.DateTime = d2.AddSeconds(sec2);
            sontarih.ToolTip = sontarih.DateTime.ToString();
        }
        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// Bu gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            if (cbTarihAraligi.SelectedIndex == 1)// dün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 2)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 3)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 6)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);




            }
            //else if (cbTarihAraligi.SelectedIndex ==6) // Bu Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
            //                      0, 0, 0, 0, 0, 0, false);

            //}
            //else if (cbTarihAraligi.SelectedIndex == 7) // Geçen Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), -1, 0, 0, 0, false,
            //                    (-DateTime.Now.Day), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false);

            //}
            else if (cbTarihAraligi.SelectedIndex == 6)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
        }
        
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            KullaniciKasaGunluk();
        }
        void KullaniciKasaGunluk()
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            gridControl2.DataSource = DB.GetData("select top 31 * from KasaGunluk with(nolock) " +
                " where fkKullanici=" + dr["fkKullanicilar"].ToString() +
                " order by pkKasaGunluk desc");
        }

        private void btnDevirBakiye_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("fkKasalar", "1"));
            list.Add(new SqlParameter("fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("Tarih", deTarih.DateTime));
            list.Add(new SqlParameter("KasadakiPara", olmasigereken.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("OlmasiGereken", kasadaki.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("Aciklama", txtAciklama.Text));

            DataTable dt = DB.GetData("select * from KasaGunluk with(nolock) where fkKasalar=1 and fkKullanici=" +
                DB.fkKullanicilar + " and Tarih='" + deTarih.DateTime.ToString("yyyyMMdd") + "'");
            if (dt.Rows.Count == 0)
            {
                DB.ExecuteSQL(@"insert into KasaGunluk (fkKasalar,fkKullanici,Tarih,KasadakiPara,OlmasiGereken,Aciklama) 
            values(@fkKasalar,@fkKullanici,@Tarih,@KasadakiPara,@OlmasiGereken,@Aciklama)", list);
            }
            else
            {
                list.Add(new SqlParameter("pkKasaGunluk", dt.Rows[0]["pkKasaGunluk"]));
                DB.ExecuteSQL(@"update KasaGunluk set KasadakiPara=@KasadakiPara,OlmasiGereken=@OlmasiGereken,Aciklama=@Aciklama
             where pkKasaGunluk=@pkKasaGunluk", list);
            }

            KullaniciKasaGunluk();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmKasaBakiyeDuzeltme KasaBakiyeDuzeltme = new frmKasaBakiyeDuzeltme(0);
            //KasaBakiyeDuzeltme.pkKasalar.Text = "1";
            KasaBakiyeDuzeltme.ceKasadakiParaYeni.Value = fark.Value;
            KasaBakiyeDuzeltme.ShowDialog();

            KullaniciKasaGunluk();
        }
    }
}
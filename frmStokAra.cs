using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using GPTS.islemler;
using System.IO;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using GPTS.Include.Data;

namespace GPTS
{

    public partial class frmStokAra : DevExpress.XtraEditors.XtraForm
    {
        string barkod = "";
        public frmStokAra(string _barkod)
        {
            InitializeComponent();
            this.Width = Screen.PrimaryScreen.WorkingArea.Width-50;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height-50;
           
            barkod = _barkod;

            if (barkod == " " || barkod == "")
            {
                AraAdindan.TabIndex = 0;
                AraBarkodtan.TabIndex = 1;
            }
            else
            {
                AraAdindan.TabIndex = 1;
                AraBarkodtan.TabIndex = 0;
            }
        }
        private void frmStokAra_Load(object sender, EventArgs e)
        {
            KullaniciAyarlari();

            Application.DoEvents();

            gcDepoMevcut.Visible  = cbDepoBazindaAra.Checked = Degerler.DepoKullaniyorum;

            if (Degerler.DepoKullaniyorum)
            {
                Depolar();
            }


            if (barkod == "")
                stokara("");
            else
                AraBarkodtan.Text = barkod;

            //kampanya değilse
            if (this.Tag.ToString() == "1")
                pnlKampanya2.Visible = true;
            else
                pnlKampanya2.Visible = false;

            string Dosya = DB.exeDizini + "\\StokAraGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
            gridColumn11.Visible = true;

            if (gridColumn33.Visible == true)
            {
                OzelDurumGetir();
                OzelDurumMenuGetir();
            }

            Yetkiler();

            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
            gridView1.UnselectRow(0);
        }
        void stokara(string tumu)
        {
            string ara1 = string.Empty, ara2 = string.Empty, ara3 = string.Empty;
            string str = AraAdindan.Text;
            string sql = "";
            string[] dizi = str.Split(' ');
            int diziuzunlugu = dizi.Length;

            if (diziuzunlugu > 3) diziuzunlugu = 3;

            for (int i = 0; i < diziuzunlugu; i++)
            {
                if (i == 0) ara1 = dizi[i].ToString();
                if (i == 1)
                {
                    ara2 = dizi[i].ToString();
                    if (ara2 == "") ara2 = " ";
                }
                if (i == 2)
                {
                    ara3 = dizi[i].ToString();
                    if (ara3 == "") ara3 = " ";
                }
                if (i > 2) break;
            }

            if (AraBarkodtan.Text == "")
            {
                if (diziuzunlugu == 1)
                {
                    if (tumu.Length > 0) ara1 = " ";
                    if (ara1.Length == 0)
                    {
                        sql = @"select top  " + seTop.Value.ToString();
                        sql = sql + @" CONVERT(bit, '0') AS Sec,
sk.pkStokKarti,sk.Stokadi,sk.Stoktipi,sk.StokKod,sk.UreticiKodu,
sk.Barcode,
case when sk.alis_iskonto>0 and sk.AlisFiyati>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,
sk.AlisFiyati,
sk.AlisFiyatiKdvHaric,
sk.KdvOrani,sk.SatisFiyati,sk.Mevcut,
                                sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
                                case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,
                                t.Firmaadi,
sk.AlisFiyatiiskontolu,renk.Aciklama as Renk,beden.Aciklama as Beden,
sk.UreticiKodu,sk.SonAlisTarihi,
case 
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
(((((sk.SatisFiyati*sk.satis_iskonto)/100)-sk.SatisFiyati)
-(((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati))/((((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati)))*100
--satis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
((((sk.SatisFiyati-((sk.SatisFiyati*sk.satis_iskonto)/100))-sk.AlisFiyati)*100)/sk.AlisFiyati)
--alis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and (sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))>0 and sk.AlisFiyati>0 then
--iskonto tutar = ((sk.AlisFiyati*sk.alis_iskonto)/100)
--iskontolu alış fiyatı = sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)
--exec sp_stokkarti_ara '',''
--select ((100-63.0)*100)/63.0
((sk.SatisFiyati-(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/
(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))
--(((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)-AlisFiyati)*100)/AlisFiyati 
--sk.alis_iskonto+(((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati)
--(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))-((sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)*sk.alis_iskonto)/100)
--sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)

--(((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))
--((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati))*100)/(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati)
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.AlisFiyati>0 and sk.AlisFiyati>0 then
((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati 
else
0
end KarYuzde,sk.EtiketAciklama,sk.fkOzelDurum,
sk.SatisFiyati-(sk.SatisFiyati*sk.satis_iskonto)/100 as isk_satis_fiyati,
m.Marka,sk.satis_iskonto,alis_iskonto";

                        sql = sql + @" from StokKarti sk with(nolock)
                                left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                                left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                                left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
                                left join RenkGrupKodu renk with(nolock) on renk.pkRenkGrupKodu=sk.fkRenkGrupKodu
                                left join BedenGrupKodu beden with(nolock) on beden.pkBedenGrupKodu=sk.fkBedenGrupKodu
                                left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka";

                        if (cbDepoBazindaAra.Checked)
                            sql = sql + " left join StokKartiDepo skd  with(nolock) on skd.fkStokKarti=sk.pkStokKarti";

                        if (cbDurumu.SelectedIndex == 1) sql = sql + " where sk.Aktif=1";
                        if (cbDurumu.SelectedIndex == 2) sql = sql + " where sk.Aktif=0";

                        if (cbDepoBazindaAra.Checked) sql = sql + " and skd.fkDepolar=" + lueDepolar.EditValue.ToString();

                        sql = sql + " order by sk.tiklamaadedi desc";
                    }
                    else//ara1 varsa
                    {
                        if (ara1 == " ")
                        {
                            sql = @"select  CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.StokKod,sk.UreticiKodu,sk.Barcode,

case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,
sk.AlisFiyati,
sk.AlisFiyatiKdvHaric,
sk.KdvOrani,sk.SatisFiyati,sk.Mevcut,
sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,
t.Firmaadi,sk.AlisFiyatiiskontolu,renk.Aciklama as Renk,beden.Aciklama as Beden,
sk.UreticiKodu,sk.SonAlisTarihi,
case 
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
(((((sk.SatisFiyati*sk.satis_iskonto)/100)-sk.SatisFiyati)
-(((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati))/((((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati)))*100
--satis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
((((sk.SatisFiyati-((sk.SatisFiyati*sk.satis_iskonto)/100))-sk.AlisFiyati)*100)/sk.AlisFiyati)
--alis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and (sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))>0 and sk.AlisFiyati>0 then
--iskonto tutar = ((sk.AlisFiyati*sk.alis_iskonto)/100)
--iskontolu alış fiyatı = sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)
--exec sp_stokkarti_ara '',''
--select ((100-63.0)*100)/63.0
((sk.SatisFiyati-(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/
(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))
--(((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)-AlisFiyati)*100)/AlisFiyati 
--sk.alis_iskonto+(((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati)
--(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))-((sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)*sk.alis_iskonto)/100)
--sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)

--(((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))
--((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati))*100)/(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati)
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.AlisFiyati>0 then
((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati 
else
0
end KarYuzde,sk.EtiketAciklama,sk.fkOzelDurum,
sk.SatisFiyati-(sk.SatisFiyati*sk.satis_iskonto)/100 as isk_satis_fiyati,
m.Marka,sk.satis_iskonto,alis_iskonto";
                            if (cbDepoBazindaAra.Checked)
                                sql = sql + ",skd.MevcutAdet";

                            sql = sql + @" from StokKarti sk with(nolock) 
                                    left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                                    left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                                    left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
                                    left join RenkGrupKodu renk with(nolock) on renk.pkRenkGrupKodu=sk.fkRenkGrupKodu
                                    left join BedenGrupKodu beden with(nolock) on beden.pkBedenGrupKodu=sk.fkBedenGrupKodu
                                    left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka";

                            if (cbDepoBazindaAra.Checked)
                                sql = sql + " left join StokKartiDepo skd  with(nolock) on skd.fkStokKarti=sk.pkStokKarti";

                            if (cbDurumu.SelectedIndex == 1) sql = sql + " where sk.Aktif=1";
                            if (cbDurumu.SelectedIndex == 2) sql = sql + " where sk.Aktif=0";

                            if (cbDepoBazindaAra.Checked) sql = sql + " and skd.fkDepolar=" + lueDepolar.EditValue.ToString();
                        }
                        else
                        {
                            sql = @"select CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.Stoktipi,sk.StokKod,sk.UreticiKodu,sk.Barcode,
case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,
sk.AlisFiyati,
sk.AlisFiyatiKdvHaric,
sk.KdvOrani,
sk.SatisFiyati,
sk.Mevcut,
sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,
t.Firmaadi,sk.AlisFiyatiiskontolu,renk.Aciklama as Renk,beden.Aciklama as Beden,
sk.UreticiKodu,sk.SonAlisTarihi,
case 
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
(((((sk.SatisFiyati*sk.satis_iskonto)/100)-sk.SatisFiyati)
-(((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati))/((((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati)))*100
--satis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
((((sk.SatisFiyati-((sk.SatisFiyati*sk.satis_iskonto)/100))-sk.AlisFiyati)*100)/sk.AlisFiyati)
--alis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and (sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))>0 and sk.AlisFiyati>0 then
--iskonto tutar = ((sk.AlisFiyati*sk.alis_iskonto)/100)
--iskontolu alış fiyatı = sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)
--exec sp_stokkarti_ara '',''
--select ((100-63.0)*100)/63.0
((sk.SatisFiyati-(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/
(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))
--(((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)-AlisFiyati)*100)/AlisFiyati 
--sk.alis_iskonto+(((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati)
--(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))-((sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)*sk.alis_iskonto)/100)
--sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)

--(((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))
--((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati))*100)/(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati)
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.AlisFiyati>0 then
((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati 
else
0
end KarYuzde,sk.EtiketAciklama,sk.fkOzelDurum,
sk.SatisFiyati-(sk.SatisFiyati*sk.satis_iskonto)/100 as isk_satis_fiyati,
m.Marka,sk.satis_iskonto,alis_iskonto";
                            if (cbDepoBazindaAra.Checked)
                                sql = sql + ",skd.MevcutAdet";

                            sql = sql + @" from StokKarti sk with(nolock)
                                    left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                                    left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                                    left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler 
                                    left join RenkGrupKodu renk with(nolock) on renk.pkRenkGrupKodu=sk.fkRenkGrupKodu
                                    left join BedenGrupKodu beden with(nolock) on beden.pkBedenGrupKodu=sk.fkBedenGrupKodu
                                    left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka";

                     if (cbDepoBazindaAra.Checked)
                      sql = sql + " left join StokKartiDepo skd  with(nolock) on skd.fkStokKarti=sk.pkStokKarti";

                            sql = sql + " where Stokadi like '%" + ara1 + "%'";

                            if (cbDurumu.SelectedIndex == 1) sql = sql + " and sk.Aktif=1";
                            if (cbDurumu.SelectedIndex == 2) sql = sql + " and sk.Aktif=0";

                            if (cbDepoBazindaAra.Checked) sql = sql + " and skd.fkDepolar=" + lueDepolar.EditValue.ToString();
                        }
                    }
                }
                else if (diziuzunlugu == 2)
                {
                    sql = @"select CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.StokKod,sk.Stoktipi,sk.UreticiKodu,sk.Barcode,
case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,
sk.AlisFiyati,
sk.AlisFiyatiKdvHaric,
sk.KdvOrani,
sk.SatisFiyati,
sk.Mevcut,
sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,
t.Firmaadi,sk.AlisFiyatiiskontolu,renk.Aciklama as Renk,beden.Aciklama as Beden,sk.UreticiKodu,
sk.SonAlisTarihi,
case 
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
(((((sk.SatisFiyati*sk.satis_iskonto)/100)-sk.SatisFiyati)
-(((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati))/((((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati)))*100
--satis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
((((sk.SatisFiyati-((sk.SatisFiyati*sk.satis_iskonto)/100))-sk.AlisFiyati)*100)/sk.AlisFiyati)
--alis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and (sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))>0 and sk.AlisFiyati>0 then
--iskonto tutar = ((sk.AlisFiyati*sk.alis_iskonto)/100)
--iskontolu alış fiyatı = sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)
--exec sp_stokkarti_ara '',''
--select ((100-63.0)*100)/63.0
((sk.SatisFiyati-(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/
(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))
--(((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)-AlisFiyati)*100)/AlisFiyati 
--sk.alis_iskonto+(((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati)
--(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))-((sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)*sk.alis_iskonto)/100)
--sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)

--(((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))
--((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati))*100)/(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati)
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.AlisFiyati>0 then
((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati 
else
0
end KarYuzde,sk.EtiketAciklama,sk.fkOzelDurum,
sk.SatisFiyati-(sk.SatisFiyati*sk.satis_iskonto)/100 as isk_satis_fiyati,
m.Marka,sk.satis_iskonto,alis_iskonto";
                    if (cbDepoBazindaAra.Checked)
                        sql = sql + ",skd.MevcutAdet";

                    sql = sql + @" from StokKarti sk with(nolock)
                            left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                            left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                            left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
                            left join RenkGrupKodu renk with(nolock) on renk.pkRenkGrupKodu=sk.fkRenkGrupKodu
                            left join BedenGrupKodu beden with(nolock) on beden.pkBedenGrupKodu=sk.fkBedenGrupKodu
left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka";

                    if (cbDepoBazindaAra.Checked)
                        sql = sql + " left join StokKartiDepo skd  with(nolock) on skd.fkStokKarti=sk.pkStokKarti";

                    sql = sql + " where Stokadi like '%" + ara1 + "%' and Stokadi like '%" + ara2 + "%'";

                    if (cbDurumu.SelectedIndex == 1) sql = sql + " and sk.Aktif=1";
                    if (cbDurumu.SelectedIndex == 2) sql = sql + " and sk.Aktif=0";

                    if (cbDepoBazindaAra.Checked) sql = sql + " and skd.fkDepolar=" + lueDepolar.EditValue.ToString();
                }
                else if (diziuzunlugu == 3)
                {
                    sql = @"select CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.StokKod,sk.UreticiKodu,sk.Barcode,
case when sk.alis_iskonto>0 and sk.AlisFiyati>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,
sk.AlisFiyati,
sk.AlisFiyatiKdvHaric,
sk.KdvOrani,
sk.SatisFiyati,
sk.Mevcut,
sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,
t.Firmaadi,renk.Aciklama as Renk,beden.Aciklama as Beden,sk.UreticiKodu,sk.SonAlisTarihi,
case 
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
(((((sk.SatisFiyati*sk.satis_iskonto)/100)-sk.SatisFiyati)
-(((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati))/((((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati)))*100
--satis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
((((sk.SatisFiyati-((sk.SatisFiyati*sk.satis_iskonto)/100))-sk.AlisFiyati)*100)/sk.AlisFiyati)
--alis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and (sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))>0 and sk.AlisFiyati>0 then
--iskonto tutar = ((sk.AlisFiyati*sk.alis_iskonto)/100)
--iskontolu alış fiyatı = sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)
--exec sp_stokkarti_ara '',''
--select ((100-63.0)*100)/63.0
((sk.SatisFiyati-(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/
(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))
--(((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)-AlisFiyati)*100)/AlisFiyati 
--sk.alis_iskonto+(((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati)
--(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))-((sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)*sk.alis_iskonto)/100)
--sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)
--(((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))
--((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati))*100)/(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati)
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.AlisFiyati>0 then
((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati 
else
0
end KarYuzde,sk.EtiketAciklama,sk.fkOzelDurum,
sk.SatisFiyati-(sk.SatisFiyati*sk.satis_iskonto)/100 as isk_satis_fiyati,
m.Marka,sk.satis_iskonto,alis_iskonto";

                    if (cbDepoBazindaAra.Checked)
                        sql = sql + ",skd.MevcutAdet";

                    sql = sql + @" from StokKarti sk with(nolock)
                            left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                            left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                            left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
                            left join RenkGrupKodu renk with(nolock) on renk.pkRenkGrupKodu=sk.fkRenkGrupKodu
                            left join BedenGrupKodu beden with(nolock) on beden.pkBedenGrupKodu=sk.fkBedenGrupKodu
left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka";

                    if (cbDepoBazindaAra.Checked)
                        sql = sql + " left join StokKartiDepo skd  with(nolock) on skd.fkStokKarti=sk.pkStokKarti";

                    sql = sql + " where Stokadi like '%" + ara1 + "%' and Stokadi like '%" + ara2 + "%' and Stokadi like '%" + ara3 + "%'";
                    if (cbDurumu.SelectedIndex == 1) sql = sql + " and sk.Aktif=1";
                    if (cbDurumu.SelectedIndex == 2) sql = sql + " and sk.Aktif=0";

                    if (cbDepoBazindaAra.Checked) sql = sql + " and skd.fkDepolar=" + lueDepolar.EditValue.ToString();

                } 
                gridControl1.DataSource = DB.GetData(sql);

            }
            else//AraBarkodtan boş değilse sonu
            {
                if (AraBarkodtan.Text != "" && AraAdindan.Text == "")
                {
                    if (AraBarkodtan.Text == " ")
                    {
                        sql = @"select top  " + seTop.Value.ToString();
                        sql = sql + @" CONVERT(bit, '0') AS Sec,
sk.pkStokKarti,sk.Stokadi,sk.Stoktipi,sk.StokKod,sk.UreticiKodu,
sk.Barcode,
case when sk.alis_iskonto>0 and sk.AlisFiyati>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,
sk.AlisFiyati,
sk.AlisFiyatiKdvHaric,
sk.KdvOrani,sk.SatisFiyati,sk.Mevcut,
                                sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
                                case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,
                                t.Firmaadi,
sk.AlisFiyatiiskontolu,renk.Aciklama as Renk,beden.Aciklama as Beden,
sk.UreticiKodu,sk.SonAlisTarihi,
case 
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
(((((sk.SatisFiyati*sk.satis_iskonto)/100)-sk.SatisFiyati)
-(((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati))/((((sk.AlisFiyati*sk.alis_iskonto)/100)-sk.AlisFiyati)))*100
--satis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.satis_iskonto>0 and sk.AlisFiyati>0 then
((((sk.SatisFiyati-((sk.SatisFiyati*sk.satis_iskonto)/100))-sk.AlisFiyati)*100)/sk.AlisFiyati)
--alis_iskonto
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.alis_iskonto>0 and (sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))>0 and sk.AlisFiyati>0 then
--iskonto tutar = ((sk.AlisFiyati*sk.alis_iskonto)/100)
--iskontolu alış fiyatı = sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)
((sk.SatisFiyati-(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/
(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))
--(((sd.SatisFiyati-sd.iskontotutar-sd.Faturaiskonto)-AlisFiyati)*100)/AlisFiyati 
--sk.alis_iskonto+(((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati)
--(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100))-((sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)*sk.alis_iskonto)/100)
--sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)

--(((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)))*100)/(sk.AlisFiyati-((sk.AlisFiyati*sk.alis_iskonto)/100)))
--((sk.SatisFiyati-(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati))*100)/(((sk.AlisFiyati*sk.alis_iskonto)/100)/sk.AlisFiyati)
when (sk.SatisFiyati-sk.AlisFiyati)>0 and sk.AlisFiyati>0 and sk.AlisFiyati>0 then
((sk.SatisFiyati-sk.AlisFiyati)*100)/sk.AlisFiyati 
else
0
end KarYuzde,sk.EtiketAciklama,sk.fkOzelDurum,
sk.SatisFiyati-(sk.SatisFiyati*sk.satis_iskonto)/100 as isk_satis_fiyati,
m.Marka,sk.satis_iskonto";
                        if (cbDepoBazindaAra.Checked)
                            sql = sql + ",skd.MevcutAdet";

                        sql = sql + @" from StokKarti sk with(nolock)
                                left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                                left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                                left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
                                left join RenkGrupKodu renk with(nolock) on renk.pkRenkGrupKodu=sk.fkRenkGrupKodu
                                left join BedenGrupKodu beden with(nolock) on beden.pkBedenGrupKodu=sk.fkBedenGrupKodu
left join Markalar m with(nolock) on m.pkMarka=sk.fkMarka";

                        if (cbDepoBazindaAra.Checked)
                            sql = sql + " left join StokKartiDepo skd  with(nolock) on skd.fkStokKarti=sk.pkStokKarti";

                        if (cbDurumu.SelectedIndex == 1) sql = sql + " where sk.Aktif=1";
                        if (cbDurumu.SelectedIndex == 2) sql = sql + " where sk.Aktif=0";

                        if (cbDepoBazindaAra.Checked) sql = sql + " and skd.fkDepolar=" + lueDepolar.EditValue.ToString();

                        sql = sql + " order by sk.tiklamaadedi desc";
                    }
                }

                if (sql == "")
                {
                    string caktif = "1";
                    if (cbDurumu.SelectedIndex == 1) caktif = "1";
                    else
                        caktif = "2";

                    sql = @"exec sp_stokkarti_ara '','" + AraBarkodtan.Text + "'," + lueDepolar.EditValue.ToString() + "," + caktif;
                }
                    gridControl1.DataSource = DB.GetData(sql);
            }
        }

        

        void Yetkiler()
        {
            string sql = @"SELECT ya.Yetki, p.Aciklama10,ya.Sayi,isnull(p.Aktif,0) as Aktif FROM  YetkiAlanlari ya with(nolock)  
INNER JOIN Parametreler p with(nolock) ON ya.fkParametreler = p.pkParametreler
WHERE ya.fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string aciklama = dtYetkiler.Rows[i]["Aciklama10"].ToString();
                bool yetki = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                string sayi = dtYetkiler.Rows[i]["Sayi"].ToString();
                //bool aktif = Convert.ToBoolean(dtYetkiler.Rows[i]["Aktif"]);

                if (aciklama == "AlisFiyati")
                {
                    //bUFİŞMALİYETİToolStripMenuItem.Enabled = yetki;

                    gridColumn5.Visible = gridColumn37.Visible = gridColumn18.Visible= gridColumn10.Visible = gridColumn15.Visible = gridColumn20.Visible = yetki;
                    
                }
                else if (aciklama == "karoran")
                {
                    //bUFİŞMALİYETİToolStripMenuItem.Enabled = yetki;

                    gridColumn16.Visible = gridColumn35.Visible = yetki;

                }
                //else if (aciklama == "AlisFiyati")
                //{
                //    bUFİŞMALİYETİToolStripMenuItem.Enabled = yetki;
                //    gridColumn31.Visible = yetki;
                //}


            }
        }

        void Depolar()
        {
            lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock)");
            lueDepolar.EditValue = Degerler.fkDepolar;
        }

        void OzelDurumMenuGetir()
        {
            DataTable dtOzel = DB.GetData("select * from OzelDurumlar with(nolock)");
            for (int i = 0; i < dtOzel.Rows.Count; i++)
            {
                //DropDownButton ddbOzel = new DropDownButton();
                //ddbOzel.Text = "Click here";
                //ddbOzel.DropDownControl = CreateDXPopupMenu();
                //DevExpress.Utils.Menu.DXPopupMenu menu = new DevExpress.Utils.Menu.DXPopupMenu();
                ///menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Item", OnItemClick));
                //menu.Items.Add(new DevExpress.Utils.Menu.DXMenuCheckItem("CheckItem", false, null, OnItemClick));
                
                //DXSubMenuItem subMenu = new DXSubMenuItem("SubMenu");
                //subMenu.Items.Add(new DXMenuItem("SubItem", OnItemClick));
                //menu.Items.Add(subMenu);

                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Tag = dtOzel.Rows[i][0].ToString();
                item.Text = dtOzel.Rows[i][1].ToString();
                
                item.Click += new EventHandler(OnItemClick);
                özelDurumlarToolStripMenuItem.DropDownItems.Add(item);
                //özelDurumlarToolStripMenuItem.DropDownItems.Add(DropDownButton, OzelDurum_click("0"));    
            }
            

        }

        private void OnItemClick(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag != null)
            {
                OzelDurumGuncelle(((ToolStripMenuItem)sender).Tag.ToString());
            }
        }

        void KullaniciAyarlari()
        {
            DataTable dtKul = DB.GetData("select * from Kullanicilar where pkKullanicilar=" + DB.fkKullanicilar);

            if (dtKul.Rows.Count > 0)
            {
                string selecttop=  dtKul.Rows[0]["selecttop"].ToString();
                if (selecttop != "")
                    seTop.Value = decimal.Parse(selecttop);
            }

        }

        void OzelDurumGetir()
        {
            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from OzelDurumlar with(nolock)");
        }
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            simpleButton2_Click(sender, e);              
        }

        private void frmStokAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                simpleButton3_Click(sender,e);
            if (e.KeyCode == Keys.F7)
                btnYeni_Click(sender, e);
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int r = e.RowHandle;

            if (r < 0) return;

            DataRow dr = gridView1.GetDataRow(r);

            string pkStokKarti = dr["pkStokKarti"].ToString();

            //AlisFiyatiOrtalama.Text = DB.GetData("select dbo.fn_alisfiyatihesapla(" + pkStokKarti + ")").Rows[0][0].ToString();

            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //if (dr["Sec"].ToString() == "False")
            //    gridView1.SetFocusedRowCellValue("Sec", "true");
            //else
            //    gridView1.SetFocusedRowCellValue("Sec", "false");

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //if (dr["Sec"].ToString() == "False")
            //    gridView1.SetFocusedRowCellValue("Sec", true);
            //else
            //    gridView1.SetFocusedRowCellValue("Sec", false);
            //int s = gridView1.FocusedRowHandle;
            //gridView1.FocusedRowHandle = -1;
            //gridView1.FocusedRowHandle = s;

            //SatisFiyatlari(pkStokKarti);

            //AlisBilgileriGetir(pkStokKarti);

            //SatisBilgileriGetir(pkStokKarti);

            //ResimGetir(pkStokKarti);
        }

        void ResimGetir(string pkStokKarti)
        {
            //pictureEdit1.EditValue = null;
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\StokKartiResim\\" + pkStokKarti + ".png";
            if (File.Exists(dosya))
            {
                Image img = Image.FromFile(dosya);
                byte[] data = DevExpress.XtraEditors.Controls.ByteImageConverter.ToByteArray(img, img.RawFormat);

                pictureEdit1.EditValue = data;
                img.Dispose();
            }
            else
                pictureEdit1.EditValue = null;
        }

        void SatisBilgileriGetir(string pkStokKarti)
        {
            string sql = @"SELECT TOP (30) s.pkSatislar, s.GuncellemeTarihi, 
            (sd.SatisFiyati-sd.iskontotutar) as SatisFiyati, f.Firmaadi, sd.Adet,sdu.durumu
            FROM  Satislar s with(nolock)
            INNER JOIN SatisDetay sd with(nolock) ON s.pkSatislar = sd.fkSatislar
            INNER JOIN Firmalar f with(nolock) ON s.fkFirma = f.pkFirma
            INNER JOIN SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu            
            WHERE  s.fkSatisDurumu not in(10) and Siparis=1 and sd.fkStokKarti =@fkStokKarti
            GROUP BY s.pkSatislar, s.GuncellemeTarihi, (sd.SatisFiyati-sd.iskontotutar), f.Firmaadi, sd.Adet,sdu.durumu
            ORDER BY s.GuncellemeTarihi DESC";
            sql = sql.Replace("@fkStokKarti", pkStokKarti);

            gcSatislar.DataSource = DB.GetData(sql);
        }

        void AlisBilgileriGetir(string pkStokKarti)
        {
            string sql = @"SELECT TOP (30) A.pkAlislar, A.GuncellemeTarihi, AD.AlisFiyati,AD.AlisFiyatiKdvHaric,
            T.Firmaadi, AD.Adet,adu.durumu
            FROM  Alislar A with(nolock)
            INNER JOIN AlisDetay AD with(nolock) ON A.pkAlislar = AD.fkAlislar 
            INNER JOIN Tedarikciler T with(nolock) ON A.fkFirma = T.pkTedarikciler
			INNER JOIN AlisDurumu adu with(nolock) on adu.pkAlisDurumu=A.fkSatisDurumu
            WHERE  Siparis=1 and AD.fkStokKarti = @fkStokKarti
            GROUP BY A.pkAlislar, A.GuncellemeTarihi,AD.AlisFiyati,AD.AlisFiyatiKdvHaric,T.Firmaadi,AD.Adet,adu.durumu
            ORDER BY A.GuncellemeTarihi DESC";
            sql = sql.Replace("@fkStokKarti", pkStokKarti);
            gridControl4.DataSource = DB.GetData(sql);
        }

        void SatisFiyatlari(string fkStokKarti)
        {
            string sql = @"SELECT SatisFiyatlari.pkSatisFiyatlari, SatisFiyatlari.SatisFiyatiKdvli, SatisFiyatlari.SatisFiyatiKdvsiz, SatisFiyatlariBaslik.Baslik
            ,SatisFiyatlari.Aktif,SatisFiyatlari.fkSatisFiyatlariBaslik,SatisFiyatlari.iskontoYuzde FROM  SatisFiyatlari with(nolock) 
            INNER JOIN SatisFiyatlariBaslik with(nolock) ON SatisFiyatlari.fkSatisFiyatlariBaslik = SatisFiyatlariBaslik.pkSatisFiyatlariBaslik
            WHERE SatisFiyatlari.fkStokKarti = " + fkStokKarti + " order by SatisFiyatlariBaslik.Tur";

            gcFiyatlar.DataSource = DB.GetData(sql);
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton2_Click(sender, e);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            pkurunid.Text = "";
            this.TopMost = true;
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.Tag.ToString() =="1"  && KampanyaFiyati.EditValue == null)
            {
                MessageBox.Show("Kampanya Fiyati Giriniz");
                KampanyaFiyati.Focus();
                return;
            }
            if (gridView1.FocusedRowHandle < 0 && gridView1.FocusedRowHandle !=- 999997) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                pkurunid.Text = dr["Barcode"].ToString();
                DB.ExecuteSQL("update StokKarti set tiklamaadedi=tiklamaadedi+1 where pkStokKarti=" + dr["pkStokKarti"].ToString());
            }
            Close();
        }

        void tumunusec_sil(bool sec)
        {
            int i = 0;
            while (gridView1.DataRowCount > i)
            {
                gridView1.SetRowCellValue(i, "Sec", sec);
                i++;
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked==true)
               gridView1.SelectAll();
            else
                gridView1.ClearSelection();
            //tumunusec(checkEdit1.Checked);
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //if (e.RowHandle >= 0)
            //{
            //    DataRow dr = gridView1.GetDataRow(e.RowHandle);
            //    if (dr["Sec"].ToString() == "True")
            //    {
            //        e.Appearance.BackColor = Color.SlateBlue;

            //        e.Appearance.ForeColor = Color.White;

            //    }
            //} 
        }

        private void repositoryItemCheckEdit1_CheckStateChanged(object sender, EventArgs e)
        {
            //int s = gridView1.FocusedRowHandle;
            //gridView1.FocusedRowHandle = -1;
            //gridView1.FocusedRowHandle = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                if (gridView1.DataRowCount == 0)
                    break;
                gridView1.DeleteRow(0);

            }
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            //if (gridView1.DataRowCount < 1000)
            //{
            //    for (int i = 0; i < gridView1.DataRowCount; i++)
            //    {
            //        gridView1.SetRowCellValue(i, "Sec", false);
            //    }
            //}
            //if (gridView1.SelectedRowsCount == 1)
            //{
            //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //    if(dr["Sec"].ToString()=="True")
            //       gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Sec", false);
            //    else
            //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Sec", true);
            //    return;
            //}
            //for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            //{
            //    int si = gridView1.GetSelectedRows()[i];
            //    DataRow dr = gridView1.GetDataRow(si);
            //    gridView1.SetRowCellValue(si, "Sec", true);
            //}
        }

        private void renkBedenGruplarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokKartiRenkBeden StokKartiRenkBeden = new frmStokKartiRenkBeden(0, int.Parse(dr["pkStokKarti"].ToString()));
            //StokKartiRenkBeden.RBGKodu.Text = dr["RBGKodu"].ToString();
            StokKartiRenkBeden.ShowDialog();
        }

        private void gridView1_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (AraAdindan.Text == "")
                stokara("tumu");
               // AraAdindan.Text = "%%%";
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            AraBarkodtan.Text = "";
            if (e.KeyCode == Keys.Left && AraAdindan.Text == "")
            {
                AraBarkodtan.Focus();
                return;
            }
            if (e.KeyCode == Keys.Down)
            {
                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
                if (gridColumn1.Visible==true)
                  gridView1.FocusedColumn = gridColumn1;
                else if (gridColumn3.Visible == true)
                    gridView1.FocusedColumn = gridColumn3;
                else if (gridColumn2.Visible == true)
                    gridView1.FocusedColumn = gridColumn2;
                else
                    gridView1.FocusedColumn = gridColumn12;
                gridView1.ShowEditor();
                //gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
                //gridControl1.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                //gridView1.FocusedRowHandle = 0;
                simpleButton2.PerformClick();
            }
        }

        private void textEdit1_KeyDown_1(object sender, KeyEventArgs e)
        {
            AraAdindan.Text = "";
            if (e.KeyCode == Keys.Right)
            {
                AraAdindan.Focus();
                return;
            }
            if (e.KeyCode == Keys.Down)
                gridControl1.Focus();

            if (e.KeyCode == Keys.Enter)
            {
                if (AraBarkodtan.Text == "")
                    AraBarkodtan.Focus();
                else
                {
                    //gridView1.FocusedRowHandle = 0;
                    simpleButton2.PerformClick();
                    //simpleButton2_Click(sender, e);
                }
            }
        }

        private void AraAdindan_EditValueChanged(object sender, EventArgs e)
        {
            //gridView1.FocusedRowHandle = -999997;
            
            stokara("");
        }

        private void AraBarkodtan_EditValueChanged(object sender, EventArgs e)
        {
                stokara("");
        }

        private void KampanyaFiyati_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
                 simpleButton2_Click(sender,  e);
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            //tumunusec(false);
            if (gridView1.DataRowCount > 0)
            {
                gridView1.FocusedRowHandle = 0;
                gridView1.SetRowCellValue(0, "Sec", true);
            }
        }

        private void checkEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
                simpleButton2_Click(sender, e);
        }

        private void icbDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            stokara("");
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            if (Degerler.StokKartiDizayn)
            {
                frmStokKartiLayout StokKarti = new frmStokKartiLayout();
                DB.pkStokKarti = 0;
                StokKarti.ShowDialog();
            }
            else
            {
                frmStokKarti StokKarti = new frmStokKarti();
                DB.pkStokKarti = 0;
                StokKarti.ShowDialog();
            }
            stokara("");
        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());

            if (Degerler.StokKartiDizayn)
            {
                frmStokKartiLayout StokKarti = new frmStokKartiLayout();
                StokKarti.ShowDialog();
            }
            else
            {
                frmStokKarti StokKarti = new frmStokKarti();
                StokKarti.ShowDialog();
            }
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0");
            //SatisUrunBazindaDetay.Tag = "2";
            //SatisUrunBazindaDetay.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            //SatisUrunBazindaDetay.ShowDialog();
            //stokara("");
        }

        private void etiketBasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string pkEtiketBas = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'Stok Kartı',0) SELECT IDENT_CURRENT('EtiketBas')");
            DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["pkStokKarti"].ToString() + ",1,0,getdate())");
            frmEtiketBas EtiketBas = new frmEtiketBas();
            EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
            EtiketBas.ShowDialog();
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            this.Text = "Stok Ara " + gridView1.DataRowCount;
        }

        private void eksikListesineEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string Stokadi = dr["Stokadi"].ToString();
            string fkStokKarti = dr["pkStokKarti"].ToString();
            if (DB.GetData("select StokAdi from EksikListesi el with(nolock) where fkStokKarti=" + fkStokKarti).Rows.Count > 0)
            {
                formislemleri.Mesajform("Daha Önce Eklendi!", "K", 200);
                return;
            }
            //inputForm sifregir = new inputForm();
            //sifregir.Text = "Miktar Giriniz";
            //sifregir.ShowDialog();

            int girilen = 0;
            int.TryParse(formislemleri.inputbox("Miktar Girişi", "Miktar Giriniz", "1", false), out girilen);
            if (girilen == 0)
            {
                formislemleri.Mesajform("Hatalı Giriş Yaptınız", "K", 200);
                return;
            }
            DB.ExecuteSQL("INSERT INTO EksikListesi (Tarih,StokAdi,Miktar,fkStokKarti) values(getdate(),'"
                + Stokadi + "'," + girilen.ToString().Replace(",", ".") + "," + fkStokKarti + ")");
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            FisNoBilgisi.fisno.EditValue = dr["pkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void varsayılanYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\StokAraGrid.xml";
            if (File.Exists(Dosya))
            {
                for (int i = 0; i < gridView1.Columns.Count; i++)
                  gridView1.Columns[i].Visible = true; 
                File.Delete(Dosya);
                Close();
            }
        }

        private void alanlarıKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\StokAraGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle); 

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(true);
            FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            simpleButton2_Click(sender, e);
        }

        private void stokHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
        }
        void AciklamaGetir(string pkStokKarti)
        {
            DataTable dt =
            DB.GetData("select * from StokKartiAciklama with(nolock) where fkStokKarti=" + pkStokKarti);
            if (dt.Rows.Count == 0)
                memoEdit1.Text = "";
            else
                memoEdit1.Text = dt.Rows[0]["Aciklama"].ToString();


        }
        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            memoEdit1.Text = "";
            //gridControl1.DataSource = null;
            gridControl4.DataSource = null;
            gcSatislar.DataSource = null;
            gcFiyatlar.DataSource = null;

           GridHitInfo ghi = gridView1.CalcHitInfo(e.Location);
            //if (ghi.Column == null) return;
            ////GridView View = sender as GridView;
            //filtir ise
            if (ghi.RowHandle == -999997) return;

            if (ghi.InRowCell)
            {
                int rowHandle = ghi.RowHandle;
                //DevExpress.XtraGrid.Columns.GridColumn column = ghi.Column;
                if (ghi.Column.FieldName == "pkStokKarti")
                {
                    DataRow dr = gridView1.GetDataRow(rowHandle);
                    string _pkStokKarti = dr["pkStokKarti"].ToString();

                    SatisFiyatlari(_pkStokKarti);

                    AlisBilgileriGetir(_pkStokKarti);

                    SatisBilgileriGetir(_pkStokKarti);

                    ResimGetir(_pkStokKarti);

                    AciklamaGetir(_pkStokKarti);

                    DepoMevcutlari(_pkStokKarti);
                }
            }
        }
        void DepoMevcutlari(string _pkStokKarti)
        {
            if (Degerler.DepoKullaniyorum)
            {
                gcDepoMevcut.DataSource = DB.GetData(@"select skd.pkStokKartiDepo,fkDepolar,MevcutAdet,DepoAdi from StokKartiDepo  skd with(nolock) 
                left join Depolar d with(nolock) on d.pkDepolar=skd.fkDepolar where fkStokKarti=" + _pkStokKarti);
            }
        }

        private void memoEdit1_EditValueChanged(object sender, EventArgs e)
        {
            string m = memoEdit1.Text;
        }

        private void memoEdit1_Leave(object sender, EventArgs e)
        {
            
        }

        private void memoEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    gridView1.Focus();
            //}
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void repositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.ExecuteSQL("update StokKarti set fkOzelDurum=" + value + " where pkStokKarti=" + dr["pkStokKarti"].ToString());

        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            //Degerler.Renkler.Beyaz;
            if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "1")
                e.Appearance.BackColor = System.Drawing.Color.White;
            else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "2")
                e.Appearance.BackColor = System.Drawing.Color.Red;
            else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "3")
                e.Appearance.BackColor = System.Drawing.Color.Yellow;
            else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "4")
                e.Appearance.BackColor = System.Drawing.Color.Green;
            else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "5")
                e.Appearance.BackColor = System.Drawing.Color.Blue;
            else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "6")
                e.Appearance.BackColor = System.Drawing.Color.Black;
            //else
              //  e.Appearance.BackColor = System.Drawing.Color.White;
        }

        private void aktifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string pkStokKarti = dr["pkStokKarti"].ToString();
                //fkOzelDurum
                DB.ExecuteSQL("update StokKarti set Aktif=1 where pkStokKarti=" + pkStokKarti);
            }

            stokara("");
        }

        private void paifYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string pkStokKarti = dr["pkStokKarti"].ToString();
                //fkOzelDurum
                DB.ExecuteSQL("update StokKarti set Aktif=0 where pkStokKarti=" + pkStokKarti);
            }

            stokara("");
        }

        void OzelDurumGuncelle(string fkOzelDurum)
        {
            string OzelDurum = "";
            if (fkOzelDurum == "0")
                OzelDurum = (string)Degerler.Renkler.Yok.ToString();
            else if (fkOzelDurum=="1")
                OzelDurum = (string)Degerler.Renkler.Beyaz.ToString();
            else if (fkOzelDurum == "2")
                OzelDurum = (string)Degerler.Renkler.Kırmızı.ToString();
            else if (fkOzelDurum == "3")
                OzelDurum = (string)Degerler.Renkler.Sarı.ToString();
            else if (fkOzelDurum == "4")
                OzelDurum = (string)Degerler.Renkler.Yeşil.ToString();
            else if (fkOzelDurum == "5")
                OzelDurum = (string)Degerler.Renkler.Mavi.ToString();
            else if (fkOzelDurum == "6")
                OzelDurum = (string)Degerler.Renkler.Siyah.ToString();


            string s = formislemleri.MesajBox("Özel Durum " + OzelDurum + " Olarak Değiştirilsin mi?", "Özel Durum Değiştir", 1, 1);
            if (s == "0") return;

            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string pkStokKarti = dr["pkStokKarti"].ToString();

                DB.ExecuteSQL("update StokKarti set fkOzelDurum=" + fkOzelDurum + " where pkStokKarti=" + pkStokKarti);
            }

            stokara("");
        }
        private void OzelDurum_click(string id)
        {
            OzelDurumGuncelle(id);
        }

        private void tanımlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStokOzeDurumlar stokozeldurum = new frmStokOzeDurumlar();
            stokozeldurum.ShowDialog();
            OzelDurumGetir();
            stokara("");
        }

        private void seTop_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                string s = formislemleri.MesajBox("Bundan Sonra Açılış Satır Sayısı " + seTop.Value.ToString() + " olsun mu?", "Satır Sayısı", 1,0);
                if (s == "1")
                {
                    DB.ExecuteSQL("update Kullanicilar set selecttop=" + seTop.Value.ToString().Replace(",",".")+
                        " where pkKullanicilar=" + DB.fkKullanicilar);
                }

                AraAdindan.Focus();
            }
        }

        private void btnOzelNot_Click(object sender, EventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //string pkStokKarti = dr["pkStokKarti"].ToString();
            //if (DB.GetData("select * from StokKartiAciklama where fkStokKarti=" + pkStokKarti).Rows.Count > 0)
            //    DB.ExecuteSQL("update StokKartiAciklama set Aciklama='" + memoEdit1.Text + "' where fkStokKarti=" + pkStokKarti);
            //else
            //    DB.ExecuteSQL("insert into StokKartiAciklama (Aciklama,fkStokKarti)" +
            //"values('" + memoEdit1.Text + "'," + pkStokKarti + ")");
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkStokKarti = dr["pkStokKarti"].ToString();

            string m = memoEdit1.Text;

            DataTable dt =
           DB.GetData("select * from StokKartiAciklama with(nolock) where fkStokKarti=" + fkStokKarti);
            int sonuc = 0;
            if (dt.Rows.Count == 0)
                sonuc = DB.ExecuteSQL("INSERT INTO StokKartiAciklama (fkStokKarti,Aciklama,Tarih)" +
                " values(" + fkStokKarti + ",'" + m + "',getdate())");
            else
                sonuc = DB.ExecuteSQL("update StokKartiAciklama set Aciklama='" + m +
                "' where fkStokKarti=" + fkStokKarti);
            if (sonuc != 1)
                formislemleri.Mesajform("Hata Oluştu.", "K", 200);
            //btnOzelNot.Enabled = false;
        }

        private void devirVeyaDüzenlemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                //yesilisikyeni();
                return;
            }

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti = dr["pkStokKarti"].ToString();

            frmStokDepoDevir StokDevir = new frmStokDepoDevir();
            StokDevir.fkStokKarti.Tag = fkStokKarti;
            StokDevir.ShowDialog();
        }

        private void satışGrafiğiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti = dr["pkStokKarti"].ToString();

            frmStokSatisGrafigi StokSatisGrafigi = new frmStokSatisGrafigi(fkStokKarti);
            StokSatisGrafigi.Show();
        }
    }
}
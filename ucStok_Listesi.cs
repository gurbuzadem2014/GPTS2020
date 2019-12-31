using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using GPTS.Include.Data;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using GPTS.islemler;
using System.IO;

namespace GPTS
{
    public partial class ucStok_Listesi : DevExpress.XtraEditors.XtraUserControl
    {
        public ucStok_Listesi()
        {
            InitializeComponent();
        }

        private void ucStok_Listesi_Load(object sender, EventArgs e)
        {
            icbDurumu.SelectedIndex = 1;
            
            gridControl2.Visible = Degerler.DepoKullaniyorum;
            //cbMevcutlu.Checked = Degerler.DepoKullaniyorum;
            GosterilenAdet.Tag = DB.GetData("select count(*) from StokKarti").Rows[0][0].ToString();
            
            //stokara("tumu");

            AraAdindan.Focus();

            string Dosya = DB.exeDizini + "\\StokListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }

            Yetkiler();
            
            KullaniciYetkileri();

            KullaniciAyarlari();

            Depolar();

            if (gridColumn45.Visible==true)
               OzelDurumGetir();
        }

        void KullaniciAyarlari()
        {
            DataTable dtKul = DB.GetData("select * from Kullanicilar where pkKullanicilar=" + DB.fkKullanicilar);

            if (dtKul.Rows.Count > 0)
            {
                string selecttop = dtKul.Rows[0]["selecttop"].ToString();
                if (selecttop != "")
                    seTop.Value = decimal.Parse(selecttop);
            }

        }

        void KullaniciYetkileri()
        {
            DataTable dt =
            DB.GetData(@"select m.pkModuller,m.Kod,m.ModulAdi,m.ana_id,m.durumu,m.Kod,my.pkModullerYetki,
            my.Yetki from Moduller m with(nolock)
            left join ModullerYetki my with(nolock) on my.Kod=m.Kod
            where m.Kod in('2.1','2.2') and my.fkKullanicilar=" + DB.fkKullanicilar);
            foreach (DataRow dr in dt.Rows)
            {
                string kod = dr["Kod"].ToString();
                string Yetki = dr["Yetki"].ToString();

                if (kod == "2.1" && Yetki == "False")
                    simpleButton6.Enabled = false;
                else if (kod == "2.2" && Yetki == "False")
                    simpleButton7.Enabled = false;

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

                    gridColumn43.Visible = gridColumn10 .Visible = gridColumn42.Visible= yetki;
                }
                //else if (aciklama == "AlisFiyati")
                //{
                //    bUFİŞMALİYETİToolStripMenuItem.Enabled = yetki;
                //    gridColumn31.Visible = yetki;
                //}


            }
        }

        void OzelDurumGetir()
        {
            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from OzelDurumlar with(nolock)");
        }

        void Depolar()
        {
            if (Degerler.DepoKullaniyorum)
            {
                lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock)");
                lueDepolar.EditValue = Degerler.fkDepolar;
            }
        }

        void DepolardakiDurum()
        {
            if (DB.pkStokKarti == 0 || Degerler.DepoKullaniyorum==false) return;

            //DataRow dr = gridView1.GetDataRow(DB.pkStokKarti);
            //DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            gridControl2.DataSource = DB.GetData(@"select DepoAdi,MevcutAdet as Mevcut from StokKartiDepo skd
            inner join Depolar d on skd.fkDepolar=d.pkDepolar
            left join DepoHareketleri dh on dh.fkStokKarti=skd.fkStokKarti
            where skd.fkStokKarti = " + DB.pkStokKarti.ToString());
        }

        private void Birimleri()
        {
            if (DB.pkStokKarti == 0) return;
            gridControl3.DataSource = DB.GetData(@"SELECT pkStokKarti,StokKod,Barcode,Stokadi,SatisFiyati,Mevcut,CikisAdet,Stoktipi FROM StokKarti WHERE pkStokKartiid = " + DB.pkStokKarti.ToString());
            //throw new NotImplementedException();
        }

        void SatisFiyatlari(string fkStokKarti)
        {
            string sql = @"SELECT SatisFiyatlari.pkSatisFiyatlari, SatisFiyatlari.SatisFiyatiKdvli, SatisFiyatlari.SatisFiyatiKdvsiz, SatisFiyatlariBaslik.Baslik,
            SatisFiyatlari.Aktif,SatisFiyatlari.fkSatisFiyatlariBaslik,SatisFiyatlari.iskontoYuzde FROM  SatisFiyatlari with(nolock) 
            INNER JOIN SatisFiyatlariBaslik ON SatisFiyatlari.fkSatisFiyatlariBaslik = SatisFiyatlariBaslik.pkSatisFiyatlariBaslik
            WHERE SatisFiyatlari.fkStokKarti = " + fkStokKarti + " ORDER BY SatisFiyatlari.fkSatisFiyatlariBaslik";
            gcFiyatlar.DataSource = DB.GetData(sql);
        }

        void SatisBilgileriGetir(int e)
        {
            if (e < 0) return;
            DataRow dr = gridView1.GetDataRow(e);
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            SatisFiyatlari(DB.pkStokKarti.ToString());
            string sql = @"SELECT  TOP (10) Satislar.pkSatislar, Satislar.Tarih, SatisDetay.SatisFiyati, Firmalar.Firmaadi, SatisDetay.Adet
FROM         Satislar  with(nolock) INNER JOIN SatisDetay  with(nolock) ON Satislar.pkSatislar = SatisDetay.fkSatislar INNER JOIN
                      Firmalar with(nolock) ON Satislar.fkFirma = Firmalar.pkFirma
WHERE Siparis=1 and SatisDetay.fkStokKarti = @fkStokKarti
GROUP BY Satislar.pkSatislar, Satislar.Tarih, SatisDetay.SatisFiyati, Firmalar.Firmaadi, SatisDetay.Adet
ORDER BY Satislar.pkSatislar DESC";
            sql = sql.Replace("@fkStokKarti", DB.pkStokKarti.ToString());
            gridControl5.DataSource = DB.GetData(sql);
        }

        void AlisBilgileriGetir(int e)
        {
            if (e < 0) return;
            DataRow dr = gridView1.GetDataRow(e);
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            string sql = @"SELECT TOP (10) Alislar.pkAlislar, Alislar.Tarih, AlisDetay.AlisFiyati, Tedarikciler.Firmaadi, AlisDetay.Adet
FROM  Alislar INNER JOIN AlisDetay ON Alislar.pkAlislar = AlisDetay.fkAlislar INNER JOIN
                      Tedarikciler ON Alislar.fkFirma = Tedarikciler.pkTedarikciler
WHERE  Siparis=1 and AlisDetay.fkStokKarti = @fkStokKarti
GROUP BY Alislar.pkAlislar, Alislar.Tarih, AlisDetay.AlisFiyati, Tedarikciler.Firmaadi, AlisDetay.Adet
ORDER BY Alislar.pkAlislar DESC";
            sql = sql.Replace("@fkStokKarti", DB.pkStokKarti.ToString());
            gridControl4.DataSource = DB.GetData(sql);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
            stokara("tumu");
            SatisFiyatlari(DB.pkStokKarti.ToString());
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmStokKartiBirimleri StokKartiBirimleri = new frmStokKartiBirimleri();
            StokKartiBirimleri.pkStokKartiid.Text = dr["pkStokKarti"].ToString();
            StokKartiBirimleri.ShowDialog();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = 0;
            StokKarti.ShowDialog();
            stokara("tumu");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmStokGirisCikislari StokGirisCikislari = new frmStokGirisCikislari();
            StokGirisCikislari.Show();

            //frmDepoKarti DepoKarti = new frmDepoKarti();
            //DepoKarti.ShowDialog();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            frmStokKontrol StokKontrol = new frmStokKontrol();
            StokKontrol.Show();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            //sayfayukle(StokSayimi);
            //ucStokSayimi StokSayim = new ucStokSayimi();
            //StokSayim.Show();
        }
        
        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            frmAktar DisveriAl = new frmAktar();
            DisveriAl.ShowDialog();
        }

        private void AraBarkodtan_TextChanged(object sender, EventArgs e)
        {

        }

        private void AraAdindan_KeyDown(object sender, KeyEventArgs e)
        {
            AraBarkodtan.Text = "";
            if (e.KeyCode == Keys.Left && AraAdindan.Text == "")
            {
                AraBarkodtan.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
        }
        void stokara(string tumu)
        {
            //if (AraBarkodtan.Text.Length == 0 || AraAdindan.Text.Length < 2) return;

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
                        if (icbDurumu.SelectedIndex == 0)
                            sql = "select ";
                        else
                            sql= @"select top " + seTop.Value.ToString();

                            sql += @"CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.StokKod,sk.Barcode,

case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,sk.AlisFiyati,

sk.KdvOrani,sk.SatisFiyati,sk.Mevcut,
                                sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
                                case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,sk.Stoktipi,
                                t.Firmaadi,sk.AlisFiyatiiskontolu,sk.UreticiKodu,
                                sk.SonAlisTarihi,sk.GuncellemeTarihi,sk.EklemeTarihi,sk.SatisAdedi,sk.fkOzelDurum,M.Marka,
								B.Aciklama as Beden,R.Aciklama as Renk,skd.MevcutAdet
                         from StokKarti sk with(nolock)
left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
left join Markalar M with(nolock) ON M.pkMarka=sk.fkMarka
left join BedenGrupKodu B with(nolock) ON B.pkBedenGrupKodu=sk.fkBedenGrupKodu
left join RenkGrupKodu R with(nolock) ON R.pkRenkGrupKodu=sk.fkRenkGrupKodu
left join StokKartiDepo skd with(nolock) on skd.fkStokKarti=sk.pkStokKarti where 1=1";

                        if(lueDepolar.EditValue== null)
                            sql += " and skd.fkDepolar=" + Degerler.fkDepolar;
                        else
                            sql += " and skd.fkDepolar=" + lueDepolar.EditValue.ToString();

                        //if(cbMevcutlu.Checked)
                        //{
                        //     sql += " and skd.MevcutAdet>0";
                        //}

                        if (icbDurumu.SelectedIndex == 1) sql += " and sk.Aktif=1";
                        if (icbDurumu.SelectedIndex == 2) sql += " and sk.Aktif=0";
                        sql = sql + " order by sk.tiklamaadedi desc";
                    }
                    else//ara1 varsa
                    {
                        if (ara1 == " ")
                        {
                           sql = @"select  CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.StokKod,sk.Barcode,
case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,sk.AlisFiyati,

sk.KdvOrani,sk.SatisFiyati,sk.Mevcut,
                                    sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
                                    case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,sk.Stoktipi,
                                    t.Firmaadi,sk.AlisFiyatiiskontolu,sk.UreticiKodu,
                                    sk.SonAlisTarihi,sk.GuncellemeTarihi,sk.EklemeTarihi,sk.SatisAdedi,sk.fkOzelDurum,
M.Marka,B.Aciklama as Beden,R.Aciklama as Renk
                             from StokKarti sk with(nolock)
                                    left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                                    left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                                    left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
left join Markalar M with(nolock) ON M.pkMarka=sk.fkMarka
left join BedenGrupKodu B with(nolock) ON B.pkBedenGrupKodu=sk.fkBedenGrupKodu
left join RenkGrupKodu R with(nolock) ON R.pkRenkGrupKodu=sk.fkRenkGrupKodu";
                            if (icbDurumu.SelectedIndex == 1) sql = sql + " where sk.Aktif=1";
                            if (icbDurumu.SelectedIndex == 2) sql = sql + " where sk.Aktif=0";
                        }
                        else
                        {
                            sql = @"select CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.StokKod,sk.Barcode,
case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,sk.AlisFiyati,
sk.KdvOrani,sk.SatisFiyati,sk.Mevcut,
                                    sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
                                    case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,sk.Stoktipi,
                                    t.Firmaadi,sk.AlisFiyatiiskontolu,sk.UreticiKodu,
                                    sk.SonAlisTarihi,sk.GuncellemeTarihi,sk.EklemeTarihi,sk.SatisAdedi,sk.fkOzelDurum,
                                    M.Marka,B.Aciklama as Beden,R.Aciklama as Renk
                             from StokKarti sk with(nolock)
                                    left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                                    left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                                    left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler 
left join RenkGrupKodu R with(nolock) on R.pkRenkGrupKodu=sk.fkRenkGrupKodu
left join Markalar M with(nolock) ON M.pkMarka=sk.fkMarka
left join BedenGrupKodu B with(nolock) ON B.pkBedenGrupKodu=sk.fkBedenGrupKodu
                                    where Stokadi like '%" + ara1 + "%'";
                            if (icbDurumu.SelectedIndex == 1) sql = sql + " and sk.Aktif=1";
                            if (icbDurumu.SelectedIndex == 2) sql = sql + " and sk.Aktif=0";
                        }
                    }
                }
                else if (diziuzunlugu == 2)
                {
                    sql = @"select CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.StokKod,sk.Barcode,
case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,sk.AlisFiyati,

sk.KdvOrani,sk.SatisFiyati,sk.Mevcut,
                            sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
                            case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,sk.Stoktipi,
                            t.Firmaadi,sk.AlisFiyatiiskontolu,sk.UreticiKodu,
                            sk.SonAlisTarihi,sk.GuncellemeTarihi,sk.EklemeTarihi,sk.SatisAdedi,sk.fkOzelDurum,
M.Marka,B.Aciklama as Beden,R.Aciklama as Renk
                     from StokKarti sk with(nolock)
                            left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                            left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                            left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
left join RenkGrupKodu R with(nolock) on R.pkRenkGrupKodu=sk.fkRenkGrupKodu
left join Markalar M with(nolock) ON M.pkMarka=sk.fkMarka
left join BedenGrupKodu B with(nolock) ON B.pkBedenGrupKodu=sk.fkBedenGrupKodu
                            where Stokadi like '%" + ara1 + "%' and Stokadi like '%" + ara2 + "%'";
                    if (icbDurumu.SelectedIndex == 1) sql = sql + " and sk.Aktif=1";
                    if (icbDurumu.SelectedIndex == 2) sql = sql + " and sk.Aktif=0";
                }
                else if (diziuzunlugu == 3)
                {
                    sql = @"select CONVERT(bit, '0') AS Sec,sk.pkStokKarti,sk.Stokadi,sk.StokKod,sk.Barcode,
case when sk.alis_iskonto>0 then 
(sk.AlisFiyati-((sk.AlisFiyati*alis_iskonto)/100))
else
sk.AlisFiyati end AlisFiyatiiskontolu,sk.AlisFiyati,
sk.KdvOrani,sk.SatisFiyati,sk.Mevcut,
                            sk.RBGKodu,sg.StokGrup,sk.SatisFiyati-(sk.KdvOrani*sk.SatisFiyati)/(sk.KdvOrani+100) as SatisFiyatiKdvsiz,
                            case when sk.Aktif='1' then 'Aktif' else 'Pasif' end as Aktif,sag.StokAltGrup,sk.Stoktipi,
                            t.Firmaadi,sk.AlisFiyatiiskontolu,sk.UreticiKodu,
                            sk.SonAlisTarihi,sk.GuncellemeTarihi,sk.EklemeTarihi,sk.SatisAdedi,sk.fkOzelDurum,
M.Marka,B.Aciklama as Beden,R.Aciklama as Renk
                     from StokKarti sk with(nolock)
                            left join StokGruplari sg with(nolock) on sg.pkStokGrup=sk.fkStokGrup
                            left join StokAltGruplari sag with(nolock) on sag.pkStokAltGruplari=sk.fkStokAltGruplari
                            left join Tedarikciler t with(nolock) on t.pkTedarikciler=sk.fkTedarikciler
left join RenkGrupKodu R with(nolock) on R.pkRenkGrupKodu=sk.fkRenkGrupKodu
left join Markalar M with(nolock) ON M.pkMarka=sk.fkMarka
left join BedenGrupKodu B with(nolock) ON B.pkBedenGrupKodu=sk.fkBedenGrupKodu
                            where Stokadi like '%" + ara1 + "%' and Stokadi like '%" + ara2 + "%' and Stokadi like '%" + ara3 + "%'";

                    if (icbDurumu.SelectedIndex == 1) sql += " and sk.Aktif=1";
                    if (icbDurumu.SelectedIndex == 2) sql += " and sk.Aktif=0";
                }
                gridControl1.DataSource = DB.GetData(sql);
            }
            else
            {
                if (AraBarkodtan.Text != "" && AraAdindan.Text == "")
                {
                    string aktif,depo;
                    if (icbDurumu.SelectedIndex == 1) aktif = "1";
                    else if (icbDurumu.SelectedIndex == 2) aktif = "0";
                    else aktif = "null";
                    if (lueDepolar.EditValue.ToString() == "0")
                        depo = "null";
                    else
                        depo = lueDepolar.EditValue.ToString();
                    sql = @"exec sp_stokkarti_ara '','" + AraBarkodtan.Text + "'," + depo + "," +aktif;
                }
                    if (sql != "")
                    gridControl1.DataSource = DB.GetData(sql);
            }
        }
        private void AraBarkodtan_KeyDown(object sender, KeyEventArgs e)
        {
            AraAdindan.Text = "";
            if (e.KeyCode == Keys.Right)
                AraAdindan.Focus();
            if (e.KeyCode == Keys.Down)
                gridControl1.Focus();

            if (e.KeyCode == Keys.Enter)
            {
                if (AraBarkodtan.Text == "")
                    AraBarkodtan.Focus();
                else
                {
                    gridView1.FocusedRowHandle = 0;
                }
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource=DB.GetData("SELECT * FROM StokKarti where HizliSatis=1");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            SatisBilgileriGetir(e.FocusedRowHandle);
            AlisBilgileriGetir(e.FocusedRowHandle);
            DepolardakiDurum();
            Birimleri();
        }

        private void AraAdindan_EditValueChanged(object sender, EventArgs e)
        {
                stokara("");
        }

        private void AraBarkodtan_EditValueChanged(object sender, EventArgs e)
        {
                stokara("");
        }

        private void AraAdindan_KeyDown_1(object sender, KeyEventArgs e)
        {
            AraBarkodtan.Text = "";
            if (e.KeyCode == Keys.Left && AraAdindan.Text == "")
            {
                AraBarkodtan.Focus();
                return;
            }
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                gridView1.FocusedRowHandle = 0;
                simpleButton2.PerformClick();
            }
        }

        private void AraBarkodtan_KeyDown_1(object sender, KeyEventArgs e)
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
                    gridView1.FocusedRowHandle = 0;
                    //simpleButton2_Click(sender, e);
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridView1_DoubleClick(sender,  e);
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkStokKarti = dr["pkStokKarti"].ToString();

            frmStokSatisGrafigi StokSatisGrafigi = new frmStokSatisGrafigi(pkStokKarti);
            StokSatisGrafigi.ShowDialog();//
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
            FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void gridView1_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (AraAdindan.Text == "")
                stokara("tumu");
                //AraAdindan.Text = "%%%";
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            FisNoBilgisi.fisno.EditValue = dr["pkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkStokKarti = dr["pkStokKarti"].ToString();
            int sha = 0, aha = 0;

            sha = int.Parse(DB.GetData("select count(*) from SatisDetay with(nolock) where fkStokKarti=" + pkStokKarti).Rows[0][0].ToString());
            aha = int.Parse(DB.GetData("select count(*) from AlisDetay with(nolock) where fkStokKarti=" + pkStokKarti).Rows[0][0].ToString());

            if (sha + aha >0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Stok Kartı Hareket Gördüğü için Silemezsiniz! \n Stoğun Durumunu Pasif Ürün Olarak Seçebilrsiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;


            if (sha + sha == 0)
            {
                DB.ExecuteSQL("DELETE FROM StokKartiAciklama where fkStokKarti=" + pkStokKarti);
                DB.ExecuteSQL("DELETE FROM StokKartiBarkodlar where fkStokKarti=" + pkStokKarti);
                DB.ExecuteSQL("DELETE FROM StokKartiDepo where fkStokKarti=" + pkStokKarti);
                DB.ExecuteSQL("DELETE FROM StokKarti where pkStokKarti=" + pkStokKarti);
            }

            frmMesajBox Mesaj = new frmMesajBox(200);
            Mesaj.label1.Text = "Stok Kartı Bilgileri Silindi.";
            Mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
            Mesaj.Show();
            stokara("tumu");
        }

        private void etiketBasımıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            formislemleri.EtiketBas(dr["pkStokKarti"].ToString(),1);

            //if (gridView1.FocusedRowHandle < 0) return;
            //string pkEtiketBas = "0";
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'',0) SELECT IDENT_CURRENT('EtiketBas')");
            //DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) VALUES(" + pkEtiketBas + "," + dr["pkStokKarti"].ToString() + ",1,0,getdate())");
            //frmEtiketBas EtiketBas = new frmEtiketBas();
            //EtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
            //EtiketBas.ShowDialog();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.DataRowCount == 1)
            {
                SatisBilgileriGetir(gridView1.FocusedRowHandle);
                AlisBilgileriGetir(gridView1.FocusedRowHandle);
                DepolardakiDurum();
                Birimleri();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ucStokSayimi StokSayimi = new ucStokSayimi();
            StokSayimi.Show();
        }

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            frmStokTopluDegisiklik FiyatGuncelle = new frmStokTopluDegisiklik();
            FiyatGuncelle.ShowDialog();
        }

        private void icbDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(icbDurumu.Tag.ToString()=="1")
               stokara("");
        }
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

        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gridControl1;

            printableLink.Landscape = true;

            printableLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //printableLink.PrintingSystem.Document.AutoFitToPagesWidth = 1;

            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            yazdir();

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl1,"A4");            
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            GosterilenAdet.Text = "Gösterilen Kayıt=" + GosterilenAdet.Tag+"/"+gridView1.DataRowCount.ToString();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            int i = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(i);
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            SatisFiyatlari.ShowDialog();
            gridView1.FocusedRowHandle = i;
        }

        private void satışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton13_Click(sender, e);
        }

        private void webServiseGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExceleGonder.datatableStoktoexel();
            //DataTable dt = (DataTable)gridControl1.DataSource;
            //for (int i = 0; i < length; i++)
            //{

            //}
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //yenistokkartiws.Service1Client yenistokkarti = new yenistokkartiws.Service1Client();
            //string sonuc = yenistokkarti.GetData("3", "deneme", "12.5");
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
                gridView1_DoubleClick(sender, e);
        }

        private void satisFiyatFarklariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.ShowDialog();
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
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmSatisFiyatlariMusteriBazli SatisFiyatlariMusteriBazli = new frmSatisFiyatlariMusteriBazli();
            SatisFiyatlariMusteriBazli.ShowDialog();
        }

        private void repositoryItemPopupContainerEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PopupContainerEdit edit = (PopupContainerEdit)sender;
                edit.ShowPopup();
            }
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            frmDepoKarti DepoKarti = new frmDepoKarti();
            DepoKarti.Show();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\StokListesiGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\StokListesiGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
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

        private void labelControl14_Click(object sender, EventArgs e)
        {
            frmDepoKarti depokart = new frmDepoKarti();
            depokart.Show();
        }

        private void lueDepolar_EditValueChanged(object sender, EventArgs e)
        {
            stokara("");
        }

        private void cbMevcutlu_CheckedChanged(object sender, EventArgs e)
        {
            if(cbMevcutlu.Tag.ToString() == "1")
              stokara("");
        }

        private void icbDurumu_Leave(object sender, EventArgs e)
        {
            icbDurumu.Tag = "0";
        }

        private void icbDurumu_Enter(object sender, EventArgs e)
        {
            icbDurumu.Tag = "1";
        }

        private void cbMevcutlu_Enter(object sender, EventArgs e)
        {
            cbMevcutlu.Tag = "1";
        }

        private void cbMevcutlu_Leave(object sender, EventArgs e)
        {
            cbMevcutlu.Tag = "0";
        }

        private void seTop_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string s = formislemleri.MesajBox("Bundan Sonra Açılış Satır Sayısı " + seTop.Value.ToString() + " olsun mu?", "Satır Sayısı", 1, 0);
                if (s == "1")
                {
                    DB.ExecuteSQL("update Kullanicilar set selecttop=" + seTop.Value.ToString().Replace(",", ".") +
                        " where pkKullanicilar=" + DB.fkKullanicilar);
                }

                AraAdindan.Focus();
            }
        }

        private void seTop_EditValueChanged(object sender, EventArgs e)
        {
            if(seTop.Tag.ToString()=="1")
                stokara("");
        }

        private void seTop_Enter(object sender, EventArgs e)
        {
            seTop.Tag= "1";
        }

        private void seTop_Leave(object sender, EventArgs e)
        {
            seTop.Tag = "0";
        }

        private void stokMevcutDevirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokDepoDevir StokDepoDevir = new frmStokDepoDevir();
            StokDepoDevir.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokDepoDevir.ShowDialog();
        }
    }
}

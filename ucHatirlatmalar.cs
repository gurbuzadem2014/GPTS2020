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
using GPTS.islemler;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using System.IO;
using GPTS.Include.Data;
using DevExpress.XtraScheduler.Native;

namespace GPTS
{
    public partial class ucHatirlatmalar : DevExpress.XtraEditors.XtraUserControl
    {
        //bool ilkyukleme = false;
        protected string firma_id = "0";
        string RandevuBasSaat = "8", RandevuBitSaat = "22", RandevuSaatAraligi = "30";
        private void RandevuAyarlari(out string RandevuBasSaat, out string RandevuBitSaat, out string RandevuSaatAraligi)
        {
            RandevuBasSaat = "8";
            RandevuBitSaat = "23";
            RandevuSaatAraligi = "30";

            //DataTable dt = null;

            //dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuBasSaat'");
            //if (dt.Rows.Count > 0)
            //    RandevuBasSaat = dt.Rows[0]["Ayar50"].ToString();

            //dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuBitSaat'");
            //if (dt.Rows.Count > 0)
            //    RandevuBitSaat = dt.Rows[0]["Ayar50"].ToString();

            //dt = DB.GetData("select * from Ayarlar with(nolock) where Ayar20='RandevuSaatAraligi'");
            //if (dt.Rows.Count > 0)
            //    RandevuSaatAraligi = dt.Rows[0]["Ayar50"].ToString();

            //TimeSpan ts1 = TimeSpan.FromHours(int.Parse(RandevuBasSaat));
            //schedulerControl1.Views.DayView.VisibleTime.Start = ts1;

            //TimeSpan ts2 = TimeSpan.FromHours(int.Parse(RandevuBitSaat));
            //schedulerControl1.Views.DayView.VisibleTime.End = ts2;

            //TimeSpan ts3 = TimeSpan.FromMinutes(int.Parse(RandevuSaatAraligi));
            //schedulerControl1.Views.DayView.TimeScale = ts3;

        }

        public ucHatirlatmalar()
        {
            InitializeComponent();
            //this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            //this.Width = Screen.PrimaryScreen.WorkingArea.Width - 200;

            //dateNavigator1.DateTime = DateTime.Today.AddDays(90);
            //this.dateNavigator1.VisibleChanged += new EventHandler(dateNavigator1_VisibleChangedChanged);
            //xtraTabControl2.SelectedTabPageIndex = 0;
            xtraTabControl2.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            RandevuAyarlari(out RandevuBasSaat, out RandevuBitSaat, out RandevuSaatAraligi);
            RandevuGorunumAyarla();
            
            //this.schedulerControl1.VisibleIntervalChanged += new EventHandler(schedulerControl1_VisibleIntervalChanged);
        }

        private void frmRandevuVer_Load(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = DateTime.Today.AddDays(+360);

            HatirlatmaAnimsatDurumlar();

            dateNavigator1.DateTime = DateTime.Today;
            schedulerControl1.GoToToday();

            string Dosya = DB.exeDizini + "\\HatirlatmaAnimsatGrid.xml";
            if (File.Exists(Dosya))
            {
                gridView2.RestoreLayoutFromXml(Dosya);
                gridView2.ActiveFilter.Clear();
            }

            string Dosya2 = DB.exeDizini + "\\HatirlatmaAnimsatGrid2.xml";
            if (File.Exists(Dosya2))
            {
                gridView1.RestoreLayoutFromXml(Dosya2);
                gridView1.ActiveFilter.Clear();
            }
        }

        private void ShowInfo(string txt)
        {
            //if (this.txtInfo.Text.Length > 800)
            //    this.txtInfo.Text = "";
            //this.txtInfo.Text += "\r\n" + (++this.counter) + " " + txt;
            //this.txtInfo.SelectionStart = this.txtInfo.Text.Length;
            //this.txtInfo.ScrollToCaret();
        }
        //private void schedulerControl1_VisibleIntervalChanged(object sender, EventArgs e)
        //{
        //    this.ShowInfo("### Visible interval changed");
        //}
        //void dateNavigator1_VisibleChangedChanged(object sender, EventArgs e)
        //{
//            this.ShowInfo("### ViewIntervalChanged");
            //DateTime dt = dateNavigator1.DateTime;
            //((DateNavigatorTest.VenusDateNavigator)sender).EndVisibleDate;
        //}

        void HatirlatmaAnimsatDurumlar()
        {
            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from HatirlatmaAnimsatDurum with(nolock)");
            repositoryItemLookUpEdit1.ValueMember = "pkHatirlatmaAnimsatDurum";
            repositoryItemLookUpEdit1.DisplayMember = "durumu";

            repositoryItemLookUpEdit2.DataSource = DB.GetData("select * from HatirlatmaAnimsatDurum with(nolock)");
            repositoryItemLookUpEdit2.ValueMember = "pkHatirlatmaAnimsatDurum";
            repositoryItemLookUpEdit2.DisplayMember = "durumu";
        }

        private void RandevuListesi()
        {
            string sql = "";
            sql = @"select case when h.fkSatislar >0 then 
'Ödeme Hatırlatması' when h.fkCek >0 then 'Çek Hatirlatması' else 'Özel Randevu' end Turu,h.pkHatirlatmaAnimsat,h.animsat_zamani,h.Tarih,f.pkFirma,fkFirma,f.Firmaadi,f.Adres,f.Tel,f.Tel2,f.Cep,f.Devir,fkStokKarti,sk.Stokadi,
had.durumu,h.fkDurumu,h.Aciklama,h.sira_no,isnull(h.arandi,0) as arandi,h.BitisTarihi,h.animsat,h.gun_sonra  from HatirlatmaAnimsat h with(nolock)
left join Firmalar f on f.pkFirma=h.fkFirma
left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
left join HatirlatmaAnimsatDurum had with(nolock) on had.pkHatirlatmaAnimsatDurum=h.fkDurumu
where h.fkDurumu=1 and h.animsat_zamani<=getdate()";

            gridGecmisler.DataSource = DB.GetData(sql);


            sql = @"select case when h.fkSatislar >0 then 
'Ödeme Hatırlatması' when h.fkCek >0 then 'Çek Hatirlatması' else 'Özel Randevu' end Turu,h.pkHatirlatmaAnimsat,h.animsat_zamani,h.Tarih,f.pkFirma,fkFirma,f.Firmaadi,f.Adres,f.Tel,f.Tel2,f.Cep,f.Devir,fkStokKarti,sk.Stokadi,
had.durumu,h.fkDurumu,h.Aciklama,h.sira_no,isnull(h.arandi,0) as arandi,h.BitisTarihi,h.animsat,h.gun_sonra  from HatirlatmaAnimsat h with(nolock)
left join Firmalar f on f.pkFirma=h.fkFirma
left join StokKarti sk with(nolock) on sk.pkStokKarti=h.fkStokKarti
left join HatirlatmaAnimsatDurum had with(nolock) on had.pkHatirlatmaAnimsatDurum=h.fkDurumu
where h.animsat_zamani>='@ilktar' and h.animsat_zamani<='@sontar'";

            sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd 00:00"));
            sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd 23:59"));

            gridControl1.DataSource = DB.GetData(sql);
            //uyarı mesajını yenilemek ve hatırlatmayı yeniler
            Degerler.AnimsatmaSaniyeInterval = 500;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Degerler.isHatirlatmaAcikmi = false;
            this.Dispose();
            //Close();
        }

        void RandevuSekmeListeGetir()
        {
            if (xtraTabControl2.SelectedTabPageIndex == 0)
            {
                cbTarihAraligi.SelectedIndex = 5;
            }
            else if (xtraTabControl2.SelectedTabPageIndex == 1)
            {
                GorunumVerileriGetir();
            }
        }

        private void TeslimTarihi_EditValueChanged(object sender, EventArgs e)
        {
            //RandevuSekmeListeGetir();
        }

        //private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        //{
            //for (int i = 0; i < gridView2.DataRowCount; i++)
            //{
            //    gridView2.SetRowCellValue(i, "Sec", cbAnimsat.Checked);
            //}
        //}

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (dr["pkFirma"].ToString() == "") return;

            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
            //Getir();
            gridView2.FocusedRowHandle = i;

            //MusteriRandevuListesi();
            RandevuSekmeListeGetir();
        }

        void zamansec_sil()
        {
            //if (gridView2.FocusedRowHandle < 0) return;
            //DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            //string tarih = deTarihi.DateTime.ToString("yyyy-MM-dd");
            //string saat = dr["saat"].ToString(); //ToString("HH:mm");
            //string zaman = tarih + " " + saat;
            //deRandevuZamani.DateTime = Convert.ToDateTime(zaman);
        }

        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //zamansec();
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
           
        }


        private void pkFirma_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        void TarihKontrol()
        {
           

            
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
           
        }

        private void lueStoklar_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void seanslarıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.SelectedRowsCount == 0) return;

            string sonuc = formislemleri.MesajBox("Randevu(ları) Silmek İstediğinize Eminmisiniz?", "Randevu Sil", 1, 2);
            if (sonuc == "0") return;

            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string id = dr["pkHatirlatmaAnimsat"].ToString();

                DB.ExecuteSQL("delete from HatirlatmaAnimsat where pkHatirlatmaAnimsat=" + id);
            }
            //uyarı mesajını yenilemek ve hatırlatmayı yeniler
            //Degerler.AnimsatmaSaniyeInterval = 500;
            RandevuListesi();
        }

        private void randevuSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Degerler.isYenile = true; 
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void yazdırTasarımToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
          seanslarıSilToolStripMenuItem_Click( sender,  e);
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            YazdirGunluk(false);
        }
        void YazdirGunluk(bool dizayn)
        {
            //XtraReport rapor= new XtraReport();
            xrCariHareket rapor = new xrCariHareket();
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string pkHatirlatma = "0", fkFirma = "0";
                System.Data.DataSet ds = new DataSet("Test");
                if (gridView1.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    pkHatirlatma = dr["pkHatirlatma"].ToString();
                    fkFirma = dr["fkFirma"].ToString();
                    if (fkFirma == "") fkFirma = "0";
                }

                //string sql = "hsp_SeansRandevuListesi '" + ilktarih.DateTime.ToString("yyyy-MM-dd") + "'";
                //if (cbBosOlanlar.Checked)
                //    sql = sql + ",1";
                //else
                //    sql = sql + ",null";

                DataTable FisDetay = (DataTable)gridControl1.DataSource; //DB.GetData(sql);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //
                //DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                //Cari.TableName = "Cari";
                //ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);

                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\HatirlatmaGunluk.repx");
                rapor.Name = "HatirlatmaGunluk";
                rapor.Report.Name = "HatirlatmaGunluk";

                if (dizayn)
                    rapor.ShowDesigner();
                else
                {
                    if (checkEdit1.Checked)
                        rapor.Print();
                    else
                        rapor.ShowPreview();
                }

               // FisDetay.();
            }
            catch (Exception ex)
            {

            }


            
        }
        private void günlükYazdırTasarımToolStripMenuItem_Click(object sender, EventArgs e)
        {
  
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
 
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (dr["pkFirma"].ToString() == "") return;

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.ShowDialog();
        }

        private void durumunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
             string pkFirma = dr["pkFirma"].ToString();

             if (pkFirma == "") return;

             DB.pkHatirlatmaAnimsat = int.Parse(dr["pkHatirlatmaAnimsat"].ToString());
            frmHatirlatmaUzat Hatirlatma = new frmHatirlatmaUzat(DateTime.Today,DateTime.Today,int.Parse(pkFirma));
            Hatirlatma.ShowDialog();

            RandevuListesi();
        }

        private void pkSatislar_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt =  DB.GetData(@"select fkFirma,fkStokKarti from Satislar s with(nolock) 
            left join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar where s.pkSatislar=" + pkSatislar.Text);
            if (dt.Rows.Count > 0)
            {
                pkFirma.Text = dt.Rows[0]["fkFirma"].ToString();
            }
        }

        private void bekliyorToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void geldiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void gelmediToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void cbBosOlanlar_CheckedChanged(object sender, EventArgs e)
        {
            RandevuListesi();
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
      
        }

        private void simpleButton8_Click_1(object sender, EventArgs e)
        {
          
        }

        private void deRandevuZamani_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
        }

        void MusteriHareketleriAc(string musteri_id)
        {
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = musteri_id;
            CariHareketMusteri.ShowDialog();
        }
        private void gridView3_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo ghi = gridView2.CalcHitInfo(e.Location);

            if (ghi.RowHandle == -999997) return;
            if (ghi.RowHandle == -2147483647) return;

            if (ghi.InRowCell)
            {
                int rowHandle = ghi.RowHandle;
                if (ghi.Column.FieldName == "pkHatirlatmaAnimsat")
                {
                    //zamansec();
                    simpleButton3_Click(sender, e);//oluştur
                }
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
           
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
           
            
        }


        void RandevuGorunumAyarla()
        {
            schedulerStorage1.Appointments.Mappings.Start = "StartTime";
            schedulerStorage1.Appointments.Mappings.End = "EndTime";
            schedulerStorage1.Appointments.Mappings.Subject = "Subject";
            schedulerStorage1.Appointments.Mappings.Description = "Description";
            schedulerStorage1.Appointments.Mappings.Location = "Cep";
            schedulerStorage1.Appointments.Mappings.Status = "fkDurumu";
            schedulerStorage1.Appointments.Mappings.Label = "Label";

            DevExpress.XtraScheduler.AppointmentCustomFieldMapping acfmCustomMappingID = new DevExpress.XtraScheduler.AppointmentCustomFieldMapping("pkHatirlatmaAnimsat", "pkHatirlatmaAnimsat");
            schedulerStorage1.Appointments.CustomFieldMappings.Add(acfmCustomMappingID);

            //schedulerStorage1.Appointments.CustomFieldMappings.Add(new AppointmentCustomFieldMapping("ApptImage1", "Icon1", FieldValueType.Object));
            DevExpress.XtraScheduler.AppointmentCustomFieldMapping acfmCustomMappingArandi = new DevExpress.XtraScheduler.AppointmentCustomFieldMapping("arandi", "arandi");
            schedulerStorage1.Appointments.CustomFieldMappings.Add(acfmCustomMappingArandi);
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            RandevuSekmeListeGetir();
        }
        
        void GorunumVerileriGetir()
        {
            string Sql = @"select pkHatirlatmaAnimsat,f.Firmaadi+' - '+ISNULL(Aciklama,'.') as [Subject],Tarih as StartTime,
            BitisTarihi as EndTime,Konu as [Description],0 as AllDay,fkfirma,fkDurumu,
case when fkDurumu=1 then 10
when fkDurumu=2 then 8
when fkDurumu=3 then 5 end Label1,fkDurumu as Label,f.Cep,h.arandi from HatirlatmaAnimsat h with(nolock)
            left join Firmalar f with(nolock) on pkFirma=h.fkFirma
            where Tarih>=DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE()),0)";
            //MessageBox.Show("randevuları görünümlü");
            //this.Text = dateNavigator1.SelectionStart.ToString() + "-" + dateNavigator1.SelectionEnd.ToString();

            //Sql = Sql + " where Tarih>='" + dateNavigator1.SelectionStart.ToString("yyyy-MM-dd")+"'";
            //Sql = Sql + " and  Tarih<='" + dateNavigator1.SelectionEnd.ToString("yyyy-MM-dd") + "'";


            //where Tarih>='@ilktarih' --and Tarih<='@sontarih'";

            //Sql = Sql.Replace("@ilktarih", deTarihi.DateTime.AddDays(-30).ToString("yyy-MM-dd 00:00");
            //Sql = Sql.Replace("@sontarih", deTarihi.DateTime.ToString("yyy-MM-dd 23:59"));

            IDateNavigatorControllerOwner control = (IDateNavigatorControllerOwner)dateNavigator1;
            
            if (ilktarih.DateTime != control.StartDate || Degerler.isYenile) 
            {
                //ilktarih.DateTime = control.StartDate;
                //sontarih.DateTime = control.EndDate;

                Sql = "HSP_HatirlatmaAnimsatListesiGorunum '" + control.StartDate.ToString("yyyy-MM-dd 00:00") + "','" + control.EndDate.ToString("yyyy-MM-dd 23:59") + "'";
                //MessageBox.Show(Sql);
                btnYenile.ToolTip = Sql;
                schedulerStorage1.Appointments.DataSource = DB.GetData(Sql);
                Degerler.isYenile = false;
               // seRafSure.Value = seRafSure.Value + 1;
            }
        }

        private void repositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            object value = (sender as LookUpEdit).EditValue;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=" + value + " where pkHatirlatmaAnimsat=" + dr["pkHatirlatmaAnimsat"].ToString());

        }


        private void schedulerStorage1_AppointmentChanging(object sender, DevExpress.XtraScheduler.PersistentObjectCancelEventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = e.Object as DevExpress.XtraScheduler.Appointment;

            if (apt.CustomFields["pkHatirlatmaAnimsat"].ToString() != "")
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@StartTime", apt.Start));
                list.Add(new SqlParameter("@EndTime", apt.End));
                list.Add(new SqlParameter("@pkHatirlatmaAnimsat", apt.CustomFields["pkHatirlatmaAnimsat"].ToString()));
                list.Add(new SqlParameter("@fkDurumu", apt.LabelId));
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=@fkDurumu,Tarih=@StartTime,BitisTarihi=@EndTime where pkHatirlatmaAnimsat=@pkHatirlatmaAnimsat", list);
                Degerler.isYenile = true;

                //MessageBox.Show("müşteri randevuları");
            }
        }

        private void schedulerControl1_EditAppointmentFormShowing(object sender, DevExpress.XtraScheduler.AppointmentFormEventArgs e)
        {
            if (e.Appointment.CustomFields["pkHatirlatmaAnimsat"] == null)
            {
                //DB.pkHatirlatma = 0;
                //deRandevuZamani.DateTime = e.Appointment.Start;
                ////deBitisZamani.DateTime = e.Appointment.End;
                //string sure = seRafSure.Value.ToString();
                //deBitisZamani.DateTime = deRandevuZamani.DateTime.AddMinutes(int.Parse(sure));
                ////oluştur
                //simpleButton3_Click(sender, e);

                //string id = "0";//e.Appointment.CustomFields["pkHatirlatma"].ToString();

                DB.pkHatirlatmaAnimsat = 0;//int.Parse(id);
                frmHatirlatmaUzat uzat = new frmHatirlatmaUzat(e.Appointment.Start, e.Appointment.End, 0);
                uzat.ShowDialog();
                Degerler.isYenile = true;
                RandevuSekmeListeGetir();
            }
            else
            {
                string id = e.Appointment.CustomFields["pkHatirlatmaAnimsat"].ToString();

                DB.pkHatirlatmaAnimsat = int.Parse(id);

                frmHatirlatmaUzat uzat = new frmHatirlatmaUzat(DateTime.Today,DateTime.Today,0);
                uzat.ShowDialog();
                //RandevuBilgileriGetir(id);

                //RandevuSec(id);
                Degerler.isYenile = true;
                RandevuSekmeListeGetir();
            }

            e.Handled = true;
        }

        void RandevuBilgileriGetir(string hatirlatma_id)
        {
            DataTable dtRandevu = DB.GetData("select * from HatirlatmaAnimsat with(nolock) where pkHatirlatmaAnimsat=" + hatirlatma_id);
            if (dtRandevu.Rows.Count > 0)
            {
                //txtPkHatirlatma.Text = hatirlatma_id;
                pkFirma.Text = "0";
                string firma_id = dtRandevu.Rows[0]["fkFirma"].ToString();

                pkFirma.Text = firma_id;
                //if (Stok_id == "") Stok_id = "0";
                //lueStoklar.EditValue = int.Parse(Stok_id);

                //if (tarih == "")
                //    deRandevuZamani.DateTime = DateTime.Now;
                //else
                //    deRandevuZamani.DateTime = Convert.ToDateTime(tarih);

                //if (tarih == "")
                //    deBitisZamani.DateTime = DateTime.Now;
                //else
                //    deBitisZamani.DateTime = Convert.ToDateTime(bitis_tarihi);


                //tbAciklama.Text = dtRandevu.Rows[0]["Aciklama"].ToString();

                //simpleButton3.Text = "Randevu Güncelle";
            }
        }


        private void schedulerStorage1_AppointmentDeleting(object sender, DevExpress.XtraScheduler.PersistentObjectCancelEventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = (DevExpress.XtraScheduler.Appointment)e.Object;
            //string id = apt.Location.ToString();
            string id = apt.CustomFields["pkHatirlatmaAnimsat"].ToString();
            string sonuc = formislemleri.MesajBox("Randevu Silmek İstediğinize Eminmisiniz?", "Randevu Sil", 3, 2);
            if (sonuc == "0")
            {
                e.Cancel = true;
                return;
            }
            DB.ExecuteSQL("delete from HatirlatmaAnimsat where pkHatirlatmaAnimsat=" + id);

            e.Cancel = false;
            Degerler.isYenile = true;
            RandevuSekmeListeGetir();

        }

        private void labelControl18_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
           
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {

        }
        GridHitInfo downHitInfo;
        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            downHitInfo = null;

            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None)
                return;

            if (e.Button == MouseButtons.Left && hitInfo.InRow && hitInfo.HitTest != GridHitTest.RowIndicator)
                downHitInfo = hitInfo;
        }

        private void gridView1_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    view.GridControl.DoDragDrop(GetDragData(view), DragDropEffects.All);
                    downHitInfo = null;
                }
            }
        }

        SchedulerDragData GetDragData(GridView view)
        {
            int[] selection = view.GetSelectedRows();
            if (selection == null)
                return null;

            AppointmentBaseCollection appointments = new AppointmentBaseCollection();
            int count = selection.Length;
            for (int i = 0; i < count; i++)
            {
                int rowIndex = selection[i];
                Appointment apt = schedulerStorage1.CreateAppointment(AppointmentType.Normal);
                apt.Subject = view.GetRowCellValue(rowIndex, "pkHatirlatmaAnimsat").ToString();
                apt.LabelId = (int)view.GetRowCellValue(rowIndex, "fkFirma");
                //apt.StatusId = (int)view.GetRowCellValue(rowIndex, "durumu");
                apt.CustomFields["pkHatirlatmaAnimsat"] = view.GetRowCellValue(rowIndex, "pkHatirlatmaAnimsat").ToString();
                apt.Location = view.GetRowCellValue(rowIndex, "fkStokKarti").ToString();
                //apt.Duration = TimeSpan.FromHours((int)view.GetRowCellValue(rowIndex, "fkStokKarti"));
                apt.Description = (string)view.GetRowCellValue(rowIndex, "Aciklama");
                appointments.Add(apt);
            }

            return new SchedulerDragData(appointments, 0);
        }

        private void schedulerControl1_AppointmentDrop(object sender, AppointmentDragEventArgs e)
        {
            //string sonuc = formislemleri.MesajBox("Randevu Silmek İstediğinize Eminmisiniz?", "Randevu Sil", 3, 2);
            //if (sonuc == "0")
            //{
            //    //e.Cancel = true;
            //    return;
            //}

            //string createEventMsg = "Randevu Zamanı  {0} - {1}.";
            //string moveEventMsg = "Yeni Randevu Zamanı {0} - {1} dan {2} - {3}.";

            DateTime srcStart = e.SourceAppointment.Start;
            DateTime newStart = e.EditedAppointment.Start;

            TimeSpan ts = newStart-srcStart;
            //int saat = ts.Hours;
            double dakika = ts.TotalMinutes;
            if (dakika < 0) dakika = dakika * -1;
            
            //string msg = (srcStart == DateTime.MinValue) ? String.Format(createEventMsg, newStart.ToShortTimeString(), newStart.ToShortDateString()) :
            //   String.Format(moveEventMsg, srcStart.ToShortTimeString(), srcStart.ToShortDateString(), newStart.ToShortTimeString(), newStart.ToShortDateString());
            
            string msg = newStart.ToString("dd MMMM yyyy HH:mm");
            if (dakika >= int.Parse(RandevuSaatAraligi))
            {
                if (XtraMessageBox.Show(msg + " olarak değiştirilsin mi?", "Randevu Zamanı Değiştirilsin mi?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    e.Allow = false;
                    e.Handled = true;
                    if (e.EditedAppointment != null)
                    {
                        DB.ExecuteSQL("Update HatirlatmaAnimsat set animsat_zamani='"+ e.EditedAppointment.Start.ToString("yyyy-MM-dd HH:mm") + "',Tarih='" + e.EditedAppointment.Start.ToString("yyyy-MM-dd HH:mm") +
                            "',BitisTarihi='" + e.EditedAppointment.End.ToString("yyyy-MM-dd HH:mm") + "' where pkHatirlatmaAnimsat=" +
                            e.SourceAppointment.CustomFields["pkHatirlatmaAnimsat"].ToString());
                    }
                    Degerler.isYenile = true;

                    RandevuSekmeListeGetir();
                }
                else
                {
                    e.Allow = false;
                    e.Handled = true;
                    //schedulerControl1.RefreshData();
                    RandevuSekmeListeGetir();
                }
            }
            else
            {
                e.Allow = false;
                e.Handled = true;
                if (e.EditedAppointment != null)
                {
                    DB.ExecuteSQL("Update HatirlatmaAnimsat set Tarih='" + e.EditedAppointment.Start.ToString("yyyy-MM-dd HH:mm") +
                        "',BitisTarihi='" + e.EditedAppointment.End.ToString("yyyy-MM-dd HH:mm") + "' where pkHatirlatmaAnimsat=" +
                        e.SourceAppointment.CustomFields["pkHatirlatmaAnimsat"].ToString());
                }
                Degerler.isYenile = true;

                RandevuSekmeListeGetir();
            }
        }

        private void frmRandevuVer_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            Degerler.isYenile = true;
            RandevuSekmeListeGetir();
        }



        private void txtPkHatirlatma_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            
        }

        private void deBitisZamani_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = dateNavigator1.DateTime.AddDays(1);
            //deTarihi.DateTime = dateNavigator1.DateTime;
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = DateTime.Today;
            //deTarihi.DateTime = dateNavigator1.DateTime;
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = dateNavigator1.DateTime.AddDays(-1);
            //deTarihi.DateTime = dateNavigator1.DateTime;
        }

        private void schedulerControl1_CustomDrawAppointment(object sender, CustomDrawObjectEventArgs e)
        {
            if (((SchedulerControl)sender).ActiveView is DayView)
            {
                AppointmentViewInfo viewInfo = e.ObjectInfo as AppointmentViewInfo;
                // Get DevExpress images.
                //Image im = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/add_16x16.png");

                //Rectangle imageBounds = new Rectangle(viewInfo.InnerBounds.X, viewInfo.InnerBounds.Y, im.Width, im.Height);
                //Rectangle mainContentBounds = new Rectangle(viewInfo.InnerBounds.X, viewInfo.InnerBounds.Y + im.Width + 1,
                //viewInfo.InnerBounds.Width, viewInfo.InnerBounds.Height - im.Height - 1);
                //// Draw image in the appointment.
                //e.Cache.Graphics.DrawImage(im, imageBounds);
            }
        }

        private void schedulerControl1_PopupMenuShowing(object sender, DevExpress.XtraScheduler.PopupMenuShowingEventArgs e)
        {
            if (e.Menu.Id == DevExpress.XtraScheduler.SchedulerMenuItemId.AppointmentMenu)
            {
                // Hide the "Change View To" menu item.
                //DevExpress.XtraScheduler.SchedulerPopupMenu itemChangeViewTo = e.Menu.GetPopupMenuById(DevExpress.XtraScheduler.SchedulerMenuItemId.SwitchViewMenu);
                //itemChangeViewTo.Visible = false;

                //e.Menu.RemoveMenuItem(DevExpress.XtraScheduler.SchedulerMenuItemId.NewRecurringEvent);
                e.Menu.RemoveMenuItem(DevExpress.XtraScheduler.SchedulerMenuItemId.OpenAppointment);

            }
            
            DevExpress.Utils.Menu.DXMenuItem item = new DevExpress.Utils.Menu.DXMenuItem("Sms Gönder", new EventHandler(item_Click_sms_gonder));
            e.Menu.Items.Insert(0, item);

            //DevExpress.Utils.Menu.DXMenuItem iteme = new DevExpress.Utils.Menu.DXMenuItem("E-Posta Gönder", new EventHandler(item_Click_mail_gonder));
            //e.Menu.Items.Insert(0, iteme);

            DevExpress.Utils.Menu.DXMenuItem itemmk = new DevExpress.Utils.Menu.DXMenuItem("Müşteri Kartı", new EventHandler(item_Click_musteri_karti));
            e.Menu.Items.Insert(0, itemmk);

            DevExpress.Utils.Menu.DXMenuItem item2 = new DevExpress.Utils.Menu.DXMenuItem("Müşteri Hareketleri", new EventHandler(item_Click_musteri_hareketleri));
            e.Menu.Items.Insert(0, item2);

            //DevExpress.Utils.Menu.DXMenuItem item0 = new DevExpress.Utils.Menu.DXMenuItem("Randevu Düzenle", new EventHandler(item_Click_RandevuDuzenle));
            //System.Drawing.Bitmap bitmap = GPTS.Properties.Resources.edit16x16;
            //item0.Image = bitmap;
            //e.Menu.Items.Insert(0, item0);




            DevExpress.Utils.Menu.DXMenuItem itemCopy = new DevExpress.Utils.Menu.DXMenuItem("Kopyala", new EventHandler(item_Click_Copy));
            System.Drawing.Bitmap bitmap1 = GPTS.Properties.Resources.Copy16x16;
            itemCopy.Image = bitmap1;
            //itemCopy.Shortcut = new Shortcut();
            //itemCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlK;
            e.Menu.Items.Insert(0, itemCopy);

            DevExpress.Utils.Menu.DXMenuItem itemCut = new DevExpress.Utils.Menu.DXMenuItem("Kes", new EventHandler(item_Click_Cut));
            System.Drawing.Bitmap bitmap2 = GPTS.Properties.Resources.cut;
            itemCut.Image = bitmap2;
            e.Menu.Items.Insert(0, itemCut);

            DevExpress.Utils.Menu.DXMenuItem itemPaste = new DevExpress.Utils.Menu.DXMenuItem("Yapıştır", new EventHandler(item_Click_Paste));
            System.Drawing.Bitmap bitmappas = GPTS.Properties.Resources.Paste16x16;
            itemPaste.Image = bitmappas;
            e.Menu.Items.Insert(0, itemPaste);

            DevExpress.Utils.Menu.DXMenuItem itemYeni = new DevExpress.Utils.Menu.DXMenuItem("Yeni Özel Randevu", new EventHandler(item_Click_OzelRandevuEkle));
            System.Drawing.Bitmap bitmapy = GPTS.Properties.Resources.New___2;
            itemYeni.Image = bitmapy;
            e.Menu.Items.Insert(0, itemYeni);

            DevExpress.Utils.Menu.DXMenuItem itemA = new DevExpress.Utils.Menu.DXMenuItem("Arandı Yap", new EventHandler(item_Click_Arandi));
            System.Drawing.Bitmap bitmapa = GPTS.Properties.Resources.Black_pin;
            itemA.Image = bitmapa;
            e.Menu.Items.Insert(0, itemA);

            DevExpress.Utils.Menu.DXMenuItem itemAc = new DevExpress.Utils.Menu.DXMenuItem("Randevu Düzenle", new EventHandler(item_Click_RandevuAc));
            System.Drawing.Bitmap bitmapdg = GPTS.Properties.Resources.edit;
            itemAc.Image = bitmapdg;
            e.Menu.Items.Insert(0, itemAc);
        }

        void item_Click_OzelRandevuEkle(object sender, EventArgs e)
        {
            DateTime secilentarih = schedulerControl1.Start;
            DB.pkHatirlatmaAnimsat = 0;
            frmHatirlatmaUzat Hatirlat = new frmHatirlatmaUzat(schedulerControl1.SelectedInterval.Start, schedulerControl1.SelectedInterval.End, 0);
            Hatirlat.ShowDialog();
            Degerler.isYenile = true;
            RandevuSekmeListeGetir();

            schedulerControl1.Start = secilentarih;
        }

        void item_Click_Copy(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }
            formislemleri.Mesajform("Randevu Kopyalandı", "S", 50);
            string id = apt.CustomFields["pkHatirlatmaAnimsat"].ToString();
            DB.pkHatirlatma_Copy = int.Parse(id);
            DB.pkHatirlatma_Cut = 0;
        }

        void item_Click_Cut(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }
            formislemleri.Mesajform("Randevu Kesildi", "M", 50);
            string id = apt.CustomFields["pkHatirlatmaAnimsat"].ToString();
            DB.pkHatirlatma_Cut = int.Parse(id);
            DB.pkHatirlatma_Copy = 0;
        }

        void item_Click_Paste(object sender, EventArgs e)
        {
            //hafızada varsa 
            if (DB.pkHatirlatma_Cut > 0)
            {
                DataTable dt = DB.GetData("select *,DATEDIFF(MINUTE,Tarih,BitisTarihi) as Saat from HatirlatmaAnimsat with(nolock) where pkHatirlatmaAnimsat=" + DB.pkHatirlatma_Cut);

                string sonuc = formislemleri.MesajBox("Randevu Taşımak istediğinize eminmisiniz?", "Randevu Değiştir", 3, 0);
                if (sonuc == "0" || dt.Rows.Count==0) return;

                //string konu = dt.Rows[0]["Konu"].ToString();
                //string fkFirma = dt.Rows[0]["fkFirma"].ToString();
                //string fkStokKarti = dt.Rows[0]["fkStokKarti"].ToString();
                string Saat = dt.Rows[0]["Saat"].ToString();
                if (Saat == "") Saat = "30";
                if (Saat == "0") Saat = "30";

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", schedulerControl1.SelectedInterval.Start));
                //list.Add(new SqlParameter("@Konu", konu));
                //list.Add(new SqlParameter("@Aciklama", "Kopya"));
                //list.Add(new SqlParameter("@Uyar", "1"));
                list.Add(new SqlParameter("@BitisTarihi", schedulerControl1.SelectedInterval.Start.AddMinutes(int.Parse(Saat))));
                //list.Add(new SqlParameter("@fkFirma", fkFirma));
                //list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                //list.Add(new SqlParameter("@fkDurumu", "1"));
                list.Add(new SqlParameter("@pkHatirlatmaAnimsat", DB.pkHatirlatma_Cut));

                DB.GetData("update HatirlatmaAnimsat set Tarih=@Tarih,BitisTarihi=@BitisTarihi where pkHatirlatmaAnimsat=@pkHatirlatmaAnimsat", list);
                Degerler.isYenile = true;
                RandevuSekmeListeGetir();

                return;
            }

            if (DB.pkHatirlatma_Copy > 0)
            {
                DataTable dt = DB.GetData("select *,DATEDIFF(MINUTE,Tarih,BitisTarihi) as sure from HatirlatmaAnimsat with(nolock) where pkHatirlatmaAnimsat=" + DB.pkHatirlatma_Copy);
                
                string sonuc = formislemleri.MesajBox("Yeni Randevu Yapıştırılsın mı?", "Randevu Değiştir", 3, 0);
                if (sonuc == "0" || dt.Rows.Count == 0) return;

                string konu = dt.Rows[0]["Konu"].ToString();
                string fkFirma = dt.Rows[0]["fkFirma"].ToString();
                string fkStokKarti = dt.Rows[0]["fkStokKarti"].ToString();
                string Aciklama = dt.Rows[0]["Aciklama"].ToString();
                
                string sure = dt.Rows[0]["sure"].ToString();
                int isure = 30;
                int.TryParse(sure, out isure);

                if (Aciklama == "") Aciklama = ".";

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", schedulerControl1.SelectedInterval.Start));
                list.Add(new SqlParameter("@Konu", konu));
                list.Add(new SqlParameter("@Aciklama", Aciklama));
                list.Add(new SqlParameter("@Uyar", "1"));
                list.Add(new SqlParameter("@BitisTarihi", schedulerControl1.SelectedInterval.Start.AddMinutes(isure)));//schedulerControl1.SelectedInterval.End));
                list.Add(new SqlParameter("@fkFirma", fkFirma));
                list.Add(new SqlParameter("@fkStokKarti", fkStokKarti));
                list.Add(new SqlParameter("@fkDurumu", "1"));

                DB.GetData("insert into HatirlatmaAnimsat (Tarih,Konu,Aciklama,Uyar,BitisTarihi,fkFirma,fkStokKarti,fkDurumu)" +
                    " values(@Tarih,@Konu,@Aciklama,@Uyar,@BitisTarihi,@fkFirma,@fkStokKarti,@fkDurumu)", list);

                Degerler.isYenile = true;
                RandevuSekmeListeGetir();
            }


        }

        void item_Click_RandevuAc(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatmaAnimsat"].ToString();
            DB.pkHatirlatmaAnimsat = int.Parse(id);
            //DataTable dt = DB.GetData("select * from HatirlatmaAnimsat with(nolock) where pkHatirlatmaAnimsat=" + id);

            DateTime secilentarih = schedulerControl1.Start;

            frmHatirlatmaUzat Hatirlat = new frmHatirlatmaUzat(secilentarih, secilentarih, 0);
            Hatirlat.ShowDialog();
            Degerler.isYenile = true;
            RandevuSekmeListeGetir();

            schedulerControl1.Start = secilentarih;
        }

        void item_Click_RandevuDuzenle(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);
        }

        void item_Click_Arandi(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatmaAnimsat"].ToString();
            if(DB.GetData("select pkHatirlatmaAnimsat from Hatirlatma with(nolock) where arandi=1 and pkHatirlatmaAnimsat=" + id).Rows.Count==0)
               DB.ExecuteSQL("update HatirlatmaAnimsat  set arandi=1 where pkHatirlatmaAnimsat=" + id);
            else
                DB.ExecuteSQL("update HatirlatmaAnimsat  set arandi=0 where pkHatirlatmaAnimsat=" + id);

            Degerler.isYenile = true;
            RandevuSekmeListeGetir();

        }

        void item_Click_sms_gonder(object sender, EventArgs e)
        {
            //int h = 0;
            //DataTable dt = DB.GetData("select pkFirma,Cep,Cep2 from Firmalar with(nolock) where sec=1 and (len(Cep)>9 or len(Cep2)>9)");//pkFirma=" + pkFirma);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string fkFirma = dt.Rows[i]["pkFirma"].ToString();
            //    string cep = dt.Rows[i]["Cep"].ToString();
            //    string cep2 = dt.Rows[i]["Cep2"].ToString();
            //    if (cep.Length < 9) cep = cep2;

            //    if (cep.Length < 9)
            //    {
            //        h++;
            //        continue;
            //    }

            //    ArrayList list = new ArrayList();
            //    list.Add(new SqlParameter("@fkFirma", fkFirma));
            //    list.Add(new SqlParameter("@CepTel", cep));
            //    list.Add(new SqlParameter("@Mesaj", "Bakiye"));

            //    if (DB.GetData("select fkFirma from Sms where Durumu=0 and fkFirma=" + fkFirma).Rows.Count == 0)
            //    {
            //        string sonuc = DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CepTel,Durumu,Mesaj,Tarih) values(@fkFirma,@CepTel,0,@Mesaj,GetDate())", list);
            //        if (sonuc == "0")
            //            DB.ExecuteSQL("update Firmalar set sec=0 where pkFirma=" + fkFirma);
            //    }
            //    else
            //        DB.ExecuteSQL("update Firmalar set sec=0 where pkFirma=" + fkFirma);
            //}

            frmSmsGonder SmsGonder = new frmSmsGonder();
            SmsGonder.Show();
        }

        void item_Click_mail_gonder(object sender, EventArgs e)
        {
            frmEpostaGonder mailGonder = new frmEpostaGonder();
            mailGonder.Show();
        }

        void item_Click_musteri_karti(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatmaAnimsat"].ToString();
            DB.pkHatirlatmaAnimsat = int.Parse(id);
            DataTable dt = DB.GetData("select * from HatirlatmaAnimsat with(nolock) where pkHatirlatmaAnimsat=" + id);

            if (dt.Rows.Count == 0) return;

            string fkfirma = dt.Rows[0]["fkfirma"].ToString();
            frmMusteriKarti mh = new frmMusteriKarti(fkfirma, "");
            mh.ShowDialog();

            //MusteriRandevuListesi();
            RandevuSekmeListeGetir();
        }

        void item_Click_musteri_hareketleri(object sender, EventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = null;
            for (int i = 0; i < schedulerControl1.SelectedAppointments.Count; i++)
            {
                apt = schedulerControl1.SelectedAppointments[i];
            }

            if (apt == null)
            {
                formislemleri.Mesajform("Randevu Seçiniz", "K", 150);
                return;
            }

            string id = apt.CustomFields["pkHatirlatmaAnimsat"].ToString();
            DB.pkHatirlatmaAnimsat = int.Parse(id);
            DataTable dt = DB.GetData("select * from HatirlatmaAnimsat with(nolock) where pkHatirlatmaAnimsat=" + id);

            if (dt.Rows.Count == 0) return;

            string fkfirma = dt.Rows[0]["fkfirma"].ToString();
            frmMusteriHareketleri mh = new frmMusteriHareketleri();
            mh.musteriadi.Tag = fkfirma;
            mh.ShowDialog();
        }

        private void simpleButton18_Click_1(object sender, EventArgs e)
        {
            //DateTime date = (dateNavigator1 as IDateNavigatorControllerOwner).StartDate;
            //if (date != DateTime.Today)
            {
                dateNavigator1.DateTime = DateTime.Today.AddDays(+360);
                //MessageBox.Show(date.ToString());
                dateNavigator1.DateTime = DateTime.Today;
            }
            schedulerControl1.GoToToday();
           
        }

        private void simpleButton16_Click_1(object sender, EventArgs e)
        {
            schedulerControl1.Start = schedulerControl1.Start.AddDays(1);
        }

        private void simpleButton17_Click_1(object sender, EventArgs e)
        {
            schedulerControl1.Start = schedulerControl1.Start.AddDays(-1);
        }

        private void frmRandevuVer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                btnCari_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                simpleButton14_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F8)
            {
                simpleButton8_Click_1(sender, e);
            }
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
            //if (cbTarihAraligi.SelectedIndex == 0)// dün
            //{
            //    ilktarih.Properties.DisplayFormat.FormatString = "f";
            //    sontarih.Properties.EditFormat.FormatString = "f";
            //    ilktarih.Properties.EditFormat.FormatString = "f";
            //    sontarih.Properties.DisplayFormat.FormatString = "f";
            //    ilktarih.Properties.EditMask = "f";
            //    sontarih.Properties.EditMask = "f";

            //    sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
            //                      -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            //}
            if (cbTarihAraligi.SelectedIndex == 0)// dün
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
            else if (cbTarihAraligi.SelectedIndex == 1)// Bu gün
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
            else if (cbTarihAraligi.SelectedIndex == 2)// yarın
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 3)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 5)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 1, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 6)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day+1), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 7)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 1, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }

            else if (cbTarihAraligi.SelectedIndex == 8)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }

            RandevuListesi();
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

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            RandevuListesi();
        }

        private void bekliyorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string id = dr["pkHatirlatmaAnimsat"].ToString();
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=1 where pkHatirlatmaAnimsat=" + id);
            }

            RandevuListesi();
        }

        private void geldiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string id = dr["pkHatirlatmaAnimsat"].ToString();
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=2 where pkHatirlatmaAnimsat=" + id);
            }

            RandevuListesi();
        }

        private void gelmediToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=4 where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            }

            RandevuListesi();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\HatirlatmaAnimsatGrid.xml";
            gridView2.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\HatirlatmaAnimsatGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView2.ShowCustomization();
            gridView2.OptionsBehavior.AutoPopulateColumns = true;
            gridView2.OptionsCustomization.AllowColumnMoving = true;
            gridView2.OptionsCustomization.AllowColumnResizing = true;
            gridView2.OptionsCustomization.AllowQuickHideColumns = true;
            gridView2.OptionsCustomization.AllowRowSizing = true;
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            //Degerler.Renkler.Beyaz;
            if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "1")//Bekliyor
                e.Appearance.BackColor = System.Drawing.Color.LightSkyBlue;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "2")//Tamamlandı
                e.Appearance.BackColor = System.Drawing.Color.LightYellow;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "3")//Ertelendi
                e.Appearance.BackColor = System.Drawing.Color.LightGreen;
            else if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "4")//İptal Edildi
                e.Appearance.BackColor = System.Drawing.Color.Red;

            //if (e.Column.FieldName == "fkDurumu" && e.CellValue.ToString() == "1")
            //  e.Appearance.BackColor = System.Drawing.Color.Aqua;

            //if(gridColumn3
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "4")
            //    e.Appearance.BackColor = System.Drawing.Color.Green;
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "5")
            //    e.Appearance.BackColor = System.Drawing.Color.Blue;
            //else if (e.Column.FieldName == "fkOzelDurum" && e.CellValue.ToString() == "6")
            //    e.Appearance.BackColor = System.Drawing.Color.Black;

        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;

            if (i < 0) return;
            DataRow dr = gridView2.GetDataRow(i);
            string id = dr["pkHatirlatmaAnimsat"].ToString();

            //pkFirma.Text = fkFirma;
            DB.pkHatirlatmaAnimsat = int.Parse(id);
            frmHatirlatmaUzat Hatirlatma = new frmHatirlatmaUzat(DateTime.Now, DateTime.Now, -1);
            Hatirlatma.ShowDialog();

            //RandevuSekmeListeGetir();
            RandevuListesi();

            gridView2.FocusedRowHandle = i;


        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView2.FocusedRowHandle;
            if (i < 0) return;

            DataRow dr = gridView2.GetDataRow(i);
            //string pkTaksitler = dr["pkTaksitler"].ToString();
            string fkFirma = dr["fkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = fkFirma;
            // KasaGirisi.pkTaksitler.Text = pkTaksitler;
            //KasaGirisi.tEaciklama.Text = dr["Tarih"].ToString() + "-Taksit Ödemesi-" + dr["Odenecek"].ToString();
            //KasaGirisi.ceTutarNakit.EditValue = dr["Odenecek"].ToString();
            //decimal kalan = 0;
            //decimal.TryParse(dr["Kalan"].ToString(), out kalan);
            //KasaGirisi.ceTutarNakit.Value = kalan;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            RandevuListesi();

        }

        private void teCariAdi_KeyDown(object sender, KeyEventArgs e)
        {
            

        }

        private void teCariAdi_KeyUp(object sender, KeyEventArgs e)
        {
           
           
        }

        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
           
        }
        private void gridView_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void btnListe_Click(object sender, EventArgs e)
        {
            xtraTabControl2.SelectedTabPage = xtabRandevuGrid;
        }

        private void btnGorunum_Click(object sender, EventArgs e)
        {
            xtraTabControl2.SelectedTabPage = xtabRandevuGorunum;
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            frmFormAyarlari fa = new frmFormAyarlari();
            fa.ShowDialog();
            RandevuAyarlari(out RandevuBasSaat, out RandevuBitSaat, out RandevuSaatAraligi);
        }

        private void schedulerControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.C && e.Modifiers == Keys.Control) || (e.KeyCode == Keys.K && e.Modifiers == Keys.Control))
            {
                item_Click_Copy(sender, e);
            }

            if (e.KeyCode == Keys.X && e.Modifiers == Keys.Control)
            {
                item_Click_Cut(sender, e);
            }

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                item_Click_Paste(sender, e);
            }
        }

        private void lbBakiye_Click(object sender, EventArgs e)
        {
           
        }

        private void schedulerControl1_InitAppointmentImages(object sender, AppointmentImagesEventArgs e)
        {
            //if (e.Appointment.IsException)
            if (e.Appointment != null && e.Appointment.CustomFields["arandi"]!= null &&  e.Appointment.CustomFields["arandi"].ToString() == "True")
            {
                AppointmentImageInfo info = new AppointmentImageInfo();
                //info.Image = SystemIcons.Warning.ToBitmap();
                info.Image = GPTS.Properties.Resources.arandi;
                e.ImageInfoList.Add(info);
            }
        }

        private void dateNavigator1_CustomDrawDayNumberCell(object sender, DevExpress.XtraEditors.Calendar.CustomDrawDayNumberCellEventArgs e)
        {

        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("say");
        }
        private void dateNavigator1_SizeChanged(object sender, EventArgs e)
        {

        }

        private void schedulerControl1_VisibleIntervalChanged(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RandevuSekmeListeGetir();
            timer1.Enabled = false;
        }

        private void arandıToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string id = dr["pkHatirlatmaAnimsat"].ToString();
            //MessageBox.Show(dr["pkHatirlatma"].ToString());
            if (dr["arandi"].ToString() == "True")
                DB.ExecuteSQL("update HatirlatmaAnimsat  set arandi=0 where pkHatirlatmaAnimsat=" + id);
            else
                DB.ExecuteSQL("update HatirlatmaAnimsat  set arandi=1 where pkHatirlatmaAnimsat=" + id);

            RandevuListesi();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            //string id = dr["pkHatirlatma"].ToString();
            if(dr["arandi"].ToString()=="True")
                arandıToolStripMenuItem1.Text = "Aranmadı Yap";
            else
                arandıToolStripMenuItem1.Text = "Arandı";
        }

        private void btnMusteriHareketleri_Click(object sender, EventArgs e)
        {

        }

        private void diğerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();

            frmTarihSec ts = new frmTarihSec();
            ts.ShowDialog();
            if (ts.Tag.ToString() == "1")
            {
                string z = ts.dateNavigator1.DateTime.ToString("yyyy-MM-dd") + " " + ts.dtpSaat.Value.ToString("HH:mm");
                DB.ExecuteSQL("update HatirlatmaAnimsat set animsat_zamani='" + z + "' where pkHatirlatmaAnimsat = " + pkHatirlatmaAnimsat);
                //DATEADD(minute," + dk + ",getdate()) where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            }
            ts.Dispose();
            //frmHatirlatmaUzat uzat = new frmHatirlatmaUzat(DateTime.Now,DateTime.Now.AddMinutes(10),0);

            // uzat.ShowDialog();

            RandevuListesi();
        }

        private void btnSonraAnimsat_Click(object sender, EventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();

            //DB.ExecuteSQL(@"UPDATE HatirlatmaAnimsat SET animsat=1,animsat_zamani=DATEADD(minute," +
            //    seDk.Value.ToString() + ",getdate())" +
            //     " WHERE pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);

            //GunlukHatirlatmalar();
        }

        private void arandıToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            DB.pkHatirlatmaAnimsat = 0;
            frmHatirlatmaUzat Hatirlatma = new frmHatirlatmaUzat(DateTime.Now, DateTime.Now, -1);
            Hatirlatma.ShowDialog();
            //uyarı mesajını yenilemek ve hatırlatmayı yeniler
            //Degerler.AnimsatmaSaniyeInterval = 500;
            RandevuListesi();
        }

        private void ertelendiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=3 where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            }

            RandevuListesi();
        }

        private void dakikaSonraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonraAnimsat(10, Degerler.ZamanDilimi.Dakika);
        }
        void sonraAnimsat(int dk, Degerler.ZamanDilimi zd)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();
            if (Degerler.ZamanDilimi.Dakika == zd)
                DB.ExecuteSQL("update HatirlatmaAnimsat set animsat_zamani=DATEADD(minute," + dk + ",getdate()) where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            else if (Degerler.ZamanDilimi.Saat == zd)
                DB.ExecuteSQL("update HatirlatmaAnimsat set animsat_zamani=DATEADD(hour," + dk + ",getdate()) where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            else if (Degerler.ZamanDilimi.Gun == zd)
                DB.ExecuteSQL("update HatirlatmaAnimsat set animsat_zamani=DATEADD(day," + dk + ",getdate()) where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);

            RandevuListesi();
        }

        private void dakikaSonraToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sonraAnimsat(30, Degerler.ZamanDilimi.Dakika);
        }

        private void saatSonraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonraAnimsat(1, Degerler.ZamanDilimi.Saat);
        }

        private void saatSonraToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sonraAnimsat(2, Degerler.ZamanDilimi.Saat);
        }

        private void saatSonraToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            sonraAnimsat(5, Degerler.ZamanDilimi.Saat);
        }

        private void günSonraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonraAnimsat(1, Degerler.ZamanDilimi.Gun);
        }

        private void dakikaSonraToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            sonraAnimsat(5, Degerler.ZamanDilimi.Dakika);
        }

        private void sontarih_Enter(object sender, EventArgs e)
        {
            sontarih.Tag = "1";
        }

        private void sontarih_EditValueChanged(object sender, EventArgs e)
        {
            if (sontarih.Tag.ToString() == "1")
                cbTarihAraligi.SelectedIndex = 9;
        }

        private void ilktarih_EditValueChanged(object sender, EventArgs e)
        {
            if (ilktarih.Tag.ToString() == "1")
                cbTarihAraligi.SelectedIndex = 9;
        }

        private void ilktarih_Enter(object sender, EventArgs e)
        {
            sontarih.Tag = "1";
        }

        private void ilktarih_Leave(object sender, EventArgs e)
        {
            ilktarih.Tag = "0";
        }

        private void sontarih_Leave(object sender, EventArgs e)
        {
            sontarih.Tag = "0";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string id = dr["pkHatirlatmaAnimsat"].ToString();
            //MessageBox.Show(dr["pkHatirlatma"].ToString());
            // if (dr["animsat"].ToString() == "True")
            DB.ExecuteSQL("update HatirlatmaAnimsat  set fkDurumu=2 where pkHatirlatmaAnimsat=" + id);
            // else
            //  DB.ExecuteSQL("update HatirlatmaAnimsat  set animsat=1 where pkHatirlatmaAnimsat=" + id);

            RandevuListesi();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();

            if (pkFirma == "") return;

            DB.pkHatirlatmaAnimsat = int.Parse(dr["pkHatirlatmaAnimsat"].ToString());
            frmHatirlatmaUzat Hatirlatma = new frmHatirlatmaUzat(DateTime.Today, DateTime.Today, int.Parse(pkFirma));
            Hatirlatma.ShowDialog();

            RandevuListesi();
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr["pkFirma"].ToString() == "") return;

            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
            //Getir();
            gridView1.FocusedRowHandle = i;

            //MusteriRandevuListesi();

            RandevuSekmeListeGetir();
        }

        private void gridView1_DoubleClick_1(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (i < 0) return;
            DataRow dr = gridView1.GetDataRow(i);
            string id = dr["pkHatirlatmaAnimsat"].ToString();

            //pkFirma.Text = fkFirma;
            DB.pkHatirlatmaAnimsat = int.Parse(id);
            frmHatirlatmaUzat Hatirlatma = new frmHatirlatmaUzat(DateTime.Now, DateTime.Now, -1);
            Hatirlatma.ShowDialog();

            //RandevuSekmeListeGetir();
            RandevuListesi();

            gridView1.FocusedRowHandle = i;
        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount==0) return;

            string sonuc = formislemleri.MesajBox("Randevu(ları) Silmek İstediğinize Eminmisiniz?", "Randevu Sil", 1, 2);
            if (sonuc == "0") return;

            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string id = dr["pkHatirlatmaAnimsat"].ToString();
               
                DB.ExecuteSQL("delete from HatirlatmaAnimsat where pkHatirlatmaAnimsat=" + id);
            }

            RandevuListesi();
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr["pkFirma"].ToString() == "") return;

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.ShowDialog();
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string id = dr["pkHatirlatmaAnimsat"].ToString();
            //MessageBox.Show(dr["pkHatirlatma"].ToString());
            if (dr["arandi"].ToString() == "True")
                DB.ExecuteSQL("update HatirlatmaAnimsat  set arandi=0 where pkHatirlatmaAnimsat=" + id);
            else
                DB.ExecuteSQL("update HatirlatmaAnimsat  set arandi=1 where pkHatirlatmaAnimsat=" + id);

            RandevuListesi();
        }

        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string id = dr["pkHatirlatmaAnimsat"].ToString();
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=1 where pkHatirlatmaAnimsat=" + id);
            }

            RandevuListesi();
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string id = dr["pkHatirlatmaAnimsat"].ToString();
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=2 where pkHatirlatmaAnimsat=" + id);
            }

            RandevuListesi();
        }

        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=4 where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            }

            RandevuListesi();
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                string v = gridView1.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView1.GetDataRow(int.Parse(v));
                string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();
                DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=3 where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            }

            RandevuListesi();
        }

        private void toolStripMenuItem27_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\HatirlatmaAnimsatGrid2.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void toolStripMenuItem28_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\HatirlatmaAnimsatGrid2.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void toolStripMenuItem29_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void tamamlandıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string id = dr["pkHatirlatmaAnimsat"].ToString();
            //MessageBox.Show(dr["pkHatirlatma"].ToString());
           // if (dr["animsat"].ToString() == "True")
                DB.ExecuteSQL("update HatirlatmaAnimsat  set fkDurumu=2 where pkHatirlatmaAnimsat=" + id);
            // else
            //  DB.ExecuteSQL("update HatirlatmaAnimsat  set animsat=1 where pkHatirlatmaAnimsat=" + id);
            yenihatirlatmaekle(id);
            RandevuListesi();
        }

        private bool yenihatirlatmaekle(string pkHatirlatmaAnimsat)
        {
            DataTable dt =
            DB.GetData("select * from HatirlatmaAnimsat with(nolock) where pkHatirlatmaAnimsat=" +
                pkHatirlatmaAnimsat);

            if (dt.Rows.Count > 0)
            {
                string gunsonra = dt.Rows[0]["gun_sonra"].ToString();

                if (gunsonra == "") gunsonra = "0";

                if (gunsonra != "0")
                {
                    DB.ExecuteSQL("UPDATE HatirlatmaAnimsat SET " +
                   "fkDurumu=1,arandi=0,animsat=1,animsat_zamani=DATEADD(day,gun_sonra,animsat_zamani)," +
                   "Tarih=DATEADD(day,gun_sonra,Tarih)," +
                   "BitisTarihi=DATEADD(day,gun_sonra,BitisTarihi) where pkHatirlatmaAnimsat=" +
                    pkHatirlatmaAnimsat);

                    formislemleri.Mesajform(gunsonra + " Gün Sonraya Randevu Eklendi", "S", 100);
                }
            }

            return true;
        }
        //private void schedulerControl1_InitAppointmentDisplayText(object sender, AppointmentDisplayTextEventArgs e)
        //{
        //    //e.Text = "test";
        //}


        private void dateNavigator1_EditDateModified(object sender, EventArgs e)
        {

        }
    }
}
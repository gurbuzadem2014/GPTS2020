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
    public partial class frmStokBirimYeniKart : DevExpress.XtraEditors.XtraForm
    {
        public frmStokBirimYeniKart()
        {
            InitializeComponent();
        }

        private void frmStokKoduverKarti_Load(object sender, EventArgs e)
        {
            //switch (this.Tag.ToString())
            //{
            //    case "1":
            //        {
            //            this.Text="Yeni Stok Grup Kartı";
            //            xtraTabPage1.PageVisible = true;
            //            xtraTabPage2.PageVisible = false;
            //            xTabTedatikciGrup.PageVisible = false;
            //            xTabTedatikciAltGrup.PageVisible = false;
            //            xtraTabPage1.Text = "Yeni Grup Adı";
            //            labelControl1.Text = "Yeni Grup Adı Giriniz";
            //            break;
            //        }
            //    case "2":
            //        {
            //            this.Text = "Yeni Stok Alt Grup Kartı";
            //            xtraTabPage1.PageVisible = false;
            //            xtraTabPage2.PageVisible = true;
            //            xTabTedatikciGrup.PageVisible = false;
            //            xTabTedatikciAltGrup.PageVisible = false;
            //            break;
            //        }
            //    case "3":
            //        {
            //            this.Text = "Yeni Tedarikçi Grup Kartı";
            //            xtraTabPage1.PageVisible = false;
            //            xtraTabPage2.PageVisible = false;
            //            xTabTedatikciGrup.PageVisible = true;
            //            xTabTedatikciAltGrup.PageVisible = false;
            //            break;
            //        }
            //    case "4":
            //        {
            //            this.Text = "Yeni Tedarikçi Alt Grup Kartı";
            //            xtraTabPage1.PageVisible = false;
            //            xtraTabPage2.PageVisible = false;
            //            xTabTedatikciGrup.PageVisible = false;
            //            xTabTedatikciAltGrup.PageVisible = true;
            //            break;
            //        }
            //}           
           
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            //switch (this.Tag.ToString())
            //{
            //    case "1":
            //        {
                        list.Add(new SqlParameter("@BirimAdi", txtBirimAdi.Text));
                        DB.ExecuteSQL("INSERT INTO Birimler (BirimAdi,Aktif,Varsayilan) VALUES(@BirimAdi,1,0)", list);
            //            break;
            //        }
            //    case "2":
            //        {
            //            list.Add(new SqlParameter("@fkStokGruplari", grupid.Tag.ToString()));
            //            list.Add(new SqlParameter("@StokAltGrup", AltGrupAdi.Text));
            //            DB.ExecuteSQL("INSERT INTO StokAltGruplari (fkStokGruplari,StokAltGrup,Aktif,SiraNo) VALUES(@fkStokGruplari,@StokAltGrup,1,0)", list);
            //            break;
            //        }
            //    case "3":
            //        {
            //            list.Add(new SqlParameter("@GrupAdi", TGrupAdi.Text));
            //            DB.ExecuteSQL("INSERT INTO TedarikcilerGruplari (GrupAdi,Aktif) VALUES(@GrupAdi,1)", list);
            //            break;
            //        }
            //    case "4":
            //        {
            //            list.Add(new SqlParameter("@fkTedarikcilerGruplari", Tgrupid.Tag.ToString()));
            //            list.Add(new SqlParameter("@TedarikcilerAltGrup", TAltGrupAdi.Text));
            //            DB.ExecuteSQL("INSERT INTO TedarikcilerAltGruplari (fkTedarikcilerGruplari,TedarikcilerAltGrup,Aktif,SiraNo) VALUES(@fkTedarikcilerGruplari,@TedarikcilerAltGrup,1,0)", list);
            //            break;
            //        }
            //}           
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void frmStokKoduverKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }
    }
}
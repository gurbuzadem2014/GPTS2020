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
    public partial class frmStokOzelDurumYeni : DevExpress.XtraEditors.XtraForm
    {
        public frmStokOzelDurumYeni()
        {
            InitializeComponent();
        }

        private void frmStokKoduverKarti_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
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
            switch (this.Tag.ToString())
            {
                case "1":
                    {
                        list.Add(new SqlParameter("@ozel_durum_adi", GrupAdi.Text));
                        list.Add(new SqlParameter("@aciklama", GrupAdi.Text));
                        DataTable dtMaxid = DB.GetData("select max(pk_ozel_durum) from OzelDurumlar with(nolock)");
                        string sonid =dtMaxid.Rows[0][0].ToString();
                        list.Add(new SqlParameter("@pk_ozel_durum", int.Parse(sonid)+1));
                        string sonuc = DB.ExecuteSQL("INSERT INTO OzelDurumlar (pk_ozel_durum,ozel_durum_adi,renk,aktif,aciklama)" +
                            " VALUES(@pk_ozel_durum,@ozel_durum_adi,'#',1,@aciklama)", list);

                        if (sonuc.Substring(0,1)=="H")
                        {
                            ArrayList list2 = new ArrayList();
                            list2.Add(new SqlParameter("@ozel_durum_adi", GrupAdi.Text));
                            list2.Add(new SqlParameter("@aciklama", GrupAdi.Text));
                            sonuc = DB.ExecuteSQL("INSERT INTO OzelDurumlar (ozel_durum_adi,renk,aktif,aciklama)" +
                                " VALUES(@ozel_durum_adi,'#',1,@aciklama)", list2);

                        }
                        break;
                    }
                case "2":
                    {
                        //list.Add(new SqlParameter("@fkStokGruplari", grupid.Tag.ToString()));
                        //list.Add(new SqlParameter("@StokAltGrup", AltGrupAdi.Text));
                        //DB.ExecuteSQL("INSERT INTO StokAltGruplari (fkStokGruplari,StokAltGrup,Aktif,SiraNo) VALUES(@fkStokGruplari,@StokAltGrup,1,0)", list);
                        break;
                    }
                case "3":
                    {
                        //list.Add(new SqlParameter("@GrupAdi", TGrupAdi.Text));
                        //DB.ExecuteSQL("INSERT INTO TedarikcilerGruplari (GrupAdi,Aktif) VALUES(@GrupAdi,1)", list);
                        break;
                    }
                case "4":
                    {
                        //list.Add(new SqlParameter("@fkTedarikcilerGruplari", Tgrupid.Tag.ToString()));
                        //list.Add(new SqlParameter("@TedarikcilerAltGrup", TAltGrupAdi.Text));
                        //DB.ExecuteSQL("INSERT INTO TedarikcilerAltGruplari (fkTedarikcilerGruplari,TedarikcilerAltGrup,Aktif,SiraNo) VALUES(@fkTedarikcilerGruplari,@TedarikcilerAltGrup,1,0)", list);
                        break;
                    }
            }           
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
            switch (this.Tag.ToString())
            {
                case "1":
                    {
                        this.Text = "Yeni Özel Durumlar Kartı";
                        xtraTabPage1.PageVisible = true;
                        xtraTabPage2.PageVisible = false;
                        xTabTedatikciGrup.PageVisible = false;
                        xTabTedatikciAltGrup.PageVisible = false;
                        xtraTabPage1.Text = "Yeni Özel Durumlar";
                        labelControl1.Text = "Yeni Özel Durumlar Giriniz";
                        GrupAdi.Focus();
                        break;
                    }
                case "2":
                    {
                        //this.Text = "Yeni Stok Alt Grup Kartı";
                        //xtraTabPage1.PageVisible = false;
                        //xtraTabPage2.PageVisible = true;
                        //xTabTedatikciGrup.PageVisible = false;
                        //xTabTedatikciAltGrup.PageVisible = false;
                        //AltGrupAdi.Focus();
                        break;
                    }
                case "3":
                    {
                        //this.Text = "Yeni Tedarikçi Grup Kartı";
                        //xtraTabPage1.PageVisible = false;
                        //xtraTabPage2.PageVisible = false;
                        //xTabTedatikciGrup.PageVisible = true;
                        //xTabTedatikciAltGrup.PageVisible = false;
                        break;
                    }
                case "4":
                    {
                        //this.Text = "Yeni Tedarikçi Alt Grup Kartı";
                        //xtraTabPage1.PageVisible = false;
                        //xtraTabPage2.PageVisible = false;
                        //xTabTedatikciGrup.PageVisible = false;
                        //xTabTedatikciAltGrup.PageVisible = true;
                        break;
                    }
            }           
            timer1.Enabled = false;
        }
    }
}
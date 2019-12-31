using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SKRS3SistemKodlari.skrsServis;
using System.Linq;
using System.Configuration;

namespace DotNetTestApp
{
    public partial class FrmSkrsKayitlari : Form
    {
        public FrmSkrsKayitlari()
        {
            InitializeComponent();
        }

       
        
        private void btnSistemKodlariniGetir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            SistemKodlariniGetir();
        }

       
        

        private void SistemleriGetir()
        {
           
        }

        private void SistemKodlariniGetir()
        {

            try
            {

                 WSSKRSSistemlerClient client = new WSSKRSSistemlerClient();

                 client.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["KullaniciAdi"].ToString();
                 client.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["Sifre"].ToString();

                client.Open();

                DataGridViewRow dataGridViewRow = dgvSistemler.SelectedRows[0];

                responseSistemler sistem =  dataGridViewRow.DataBoundItem as responseSistemler;


                kodDegerleriResponse kodDegerleriResponse = new kodDegerleriResponse();

                dgvSistemKodlari.DataSource = null;
                dgvSistemKodlari.Columns.Clear();

             

                kodDegerleriResponse = client.SistemKodlari(sistem.kodu, DateTime.Now.AddDays(-100000));
                    
                if (kodDegerleriResponse.kodDegerleri != null )
                {
                    TabloyuOlustur(kodDegerleriResponse.kodDegerleri);

               
                }
             
                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void SistemKodlariSayfaGetir()
        {

            try
            {

                WSSKRSSistemlerClient client = new WSSKRSSistemlerClient();

                client.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["KullaniciAdi"].ToString();
                client.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["Sifre"].ToString();

                client.Open();

                DataGridViewRow dataGridViewRow = dgvSistemler.SelectedRows[0];

                responseSistemler sistem = dataGridViewRow.DataBoundItem as responseSistemler;


                kodDegerleriResponse kodDegerleriResponse = new kodDegerleriResponse();

                long deger = 0;

                dgvSistemKodlari.DataSource = null;
                dgvSistemKodlari.Columns.Clear();

                burasi:

                kodDegerleriResponse = client.SistemKodlariSayfaGetir(sistem.kodu, DateTime.Now.AddDays(-100000), Convert.ToInt32(deger));

                if (kodDegerleriResponse.kodDegerleri != null)
                {
                    TabloyuOlustur(kodDegerleriResponse.kodDegerleri);

                    deger = deger + 1000;

                    goto burasi;
                }



            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void TabloyuOlustur(kodDegeriKolonu[][] kodDegerleri)
        {

            if (dgvSistemKodlari.RowCount == 0)
            {

                for (int index = 0; index < kodDegerleri[0].Length; index++)
                {
                    kodDegeriKolonu kodDegeriKolonu = kodDegerleri[0][index];
                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                    dataGridViewTextBoxColumn.HeaderText = kodDegeriKolonu.kolonAdi;

                    dgvSistemKodlari.Columns.Add(dataGridViewTextBoxColumn);
                }
            }
            
            foreach (kodDegeriKolonu[] kodDegeriKolonus in kodDegerleri)
            {
                List<String> icerikler = new List<String>();

                foreach (kodDegeriKolonu kodDegeriKolonu in kodDegeriKolonus)
                {
                    icerikler.Add(kodDegeriKolonu.kolonIcerigi.Value);
                }

                dgvSistemKodlari.Rows.Add(icerikler.ToArray());
            }

            dgvSistemKodlari.AutoResizeColumns();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            CheckForIllegalCrossThreadCalls = false;

            btnSistemKodlariniGetir.Enabled = false;
            string tmpText = btnSistemKodlariniGetir.Text;

            btnSistemKodlariniGetir.Text = "Sistemler Getiriliyor...";

            

            btnSistemKodlariniGetir.Enabled = true;
            btnSistemKodlariniGetir.Text = tmpText;

            CheckForIllegalCrossThreadCalls = true;
        }

        private void dgvSistemler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SistemKodlariniGetir();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (dgvSistemKodlari.Rows.Count > 0)
            {
               // Microsoft.Office.Interop.Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();

                //XcelApp.Application.Workbooks.Add(Type.Missing);

                //for (int i = 1; i < dgvSistemKodlari.Columns.Count + 1; i++)
                //{
                //    XcelApp.Cells[1, i] = dgvSistemKodlari.Columns[i - 1].HeaderText;
                //}

                //for (int i = 0; i < dgvSistemKodlari.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dgvSistemKodlari.Columns.Count; j++)
                //    {
                //        if (dgvSistemKodlari.Rows[i].Cells[j].Value != null)
                //        {
                //            XcelApp.Cells[i + 2, j + 1] = dgvSistemKodlari.Rows[i].Cells[j].Value.ToString();
                //        }
                //    }
                //}
                //XcelApp.Columns.AutoFit();
                //XcelApp.Visible = true;
            }
        }

        private void FrmSkrsKayitlari_Load(object sender, EventArgs e)
        {
            SistemleriGetir();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;

                WSSKRSSistemlerClient client = new WSSKRSSistemlerClient();

                client.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["KullaniciAdi"].ToString();

                client.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["Sifre"].ToString();

                client.Open();


                wsskrsSistemlerResponse wsskrsSistemlerResponse = client.Sistemler();

                List<responseSistemler> testList = new List<responseSistemler>(wsskrsSistemlerResponse.sistemler);

                dgvSistemler.DataSource = testList.OrderBy(z => z.adi).ToList();


                dgvSistemler.AutoResizeColumns();


                

                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DB db = new DB();


            try
            {

                double ll_max_seq;
                string ls_insert_sql; 
                string ls_update_sql;
                double li_count;



                ll_max_seq = db.ExecSqlOneRowInteger("SELECT max(lookup_grup_seq) FROM HBYS.LOOKUP", null);

                ll_max_seq++;

                for (int i = 0; i < dgvSistemKodlari.Rows.Count; i++)
                {
                    DataGridViewRow sistem = dgvSistemKodlari.Rows[i];

                    li_count = db.ExecSqlOneRowInteger("select lookup_grup_seq from hbys.lookup where lookup_seq = " + sistem.Cells[3].Value.ToString() + "' and DESCRIPTION = '" + sistem.Cells[0].Value.ToString() + "'", null);

                    if (li_count > 0)
                    {
                        ls_update_sql = @"update hbys.lookup set display_value = '" + sistem.Cells[2].Value.ToString() + "', AKTIF = " + sistem.Cells[1].Value.ToString() + " where lookup_grup_seq = " + li_count.ToString() + " and lookup_seq = " + sistem.Cells[3].Value.ToString();

                        li_count = db.ExecSql(ls_update_sql);
                    }
                    else
                    {
                        ls_insert_sql = @"insert into hbys.lookup (lookup_grup_seq,lookup_seq,display_value,DESCRIPTION,AKTIF) 
                                    values(" + ll_max_seq.ToString() + ","
                                             + sistem.Cells[3].Value.ToString() + ","
                                             + "'" + sistem.Cells[2].Value.ToString() + "',"
                                             + "'" + sistem.Cells[0].Value.ToString() + "',"
                                             + sistem.Cells[1].Value.ToString() + ")";
                       
                        li_count = db.ExecSql(ls_insert_sql);
                    }

              }

                MessageBox.Show("İşlem Tamamlandı", "Bilgi");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            SistemKodlariSayfaGetir();
        }

        private void button5_Click(object sender, EventArgs e)
        {


            try
            {

                WSSKRSSistemlerClient client = new WSSKRSSistemlerClient();

                client.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["KullaniciAdi"].ToString();
                client.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["Sifre"].ToString();

                client.Open();

                DataGridViewRow dataGridViewRow = dgvSistemler.SelectedRows[0];

                responseSistemler sistem = dataGridViewRow.DataBoundItem as responseSistemler;


                kodDegerleriResponse kodDegerleriResponse = new kodDegerleriResponse();

                long deger = 0;

           
                int sayac = 0;
                int sayac2 = 0;

                //Microsoft.Office.Interop.Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();

                //XcelApp.Application.Workbooks.Add(Type.Missing);

                burasi:

                kodDegerleriResponse = client.SistemKodlariSayfaGetir(sistem.kodu, DateTime.Now.AddDays(-100000), Convert.ToInt32(deger));

            //    kodDegerleriResponse = client.SistemKodlariSayfaGetir("c3eade04-4f91-5dab-e043-14031b0ac9f9", DateTime.Now.AddDays(-100000), Convert.ToInt32(deger));

                

                if (kodDegerleriResponse.kodDegerleri != null)
                {
                    sayac++;

                    if (sayac == 1)
                    {
                 
                            //for (int index = 0; index < kodDegerleriResponse.kodDegerleri[0].Length ; index++)
                            //{

                         
                            //    XcelApp.Cells[1, index + 1] = kodDegerleriResponse.kodDegerleri[0][index].kolonAdi;

                            //}

                            //XcelApp.Visible = true;
                            //XcelApp.Columns.AutoFit();

                    }

                 
                    foreach (kodDegeriKolonu[] kodDegeriKolonus in kodDegerleriResponse.kodDegerleri)
                    {
                        //int sayac3 = 0;
                        //sayac2++;

                        //foreach (kodDegeriKolonu kodDegeriKolonu in kodDegeriKolonus)
                        //{
                        //    sayac3++;
                        //    if (kodDegeriKolonu.kolonIcerigi.Value != null)
                        //    {
                        //        XcelApp.Cells[sayac2 + 1, sayac3] = kodDegeriKolonu.kolonIcerigi.Value.ToString();
                             
                        //    }
                        //}

                     }

                    //XcelApp.Columns.AutoFit();

                    deger = deger + 1000;

                    goto burasi;
                }



            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }


        }
    }
}
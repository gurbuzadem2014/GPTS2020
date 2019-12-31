using HTT.Data;
using HTT.VData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HTT.WinForm.Tanimlar
{
    public partial class frmSirketler : Form
    {
        public frmSirketler()
        {
            InitializeComponent();
        }

        private void Sirketler_Load(object sender, EventArgs e)
        {
            SirketListesi();
        }

        void SirketListesi()
        {
            dataGridView1.DataSource = Get();
        }

        public List<VSirketler> Get()
        {
            using (MTPEntities entities = new MTPEntities())
            {
                return entities.Sirketler.Select(s => new VSirketler()
                {
                    pkSirket = s.pkSirket,
                    Sirket = s.Sirket,
                    Yetkili = s.Yetkili
                }).Take(10).ToList();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Sirketler yenisirket = new Sirketler();
            yenisirket.Sirket = txtSirketAdi.Text;

            using (MTPEntities entities = new MTPEntities())
            {
                //trasn başlat
                DbContextTransaction transaction = entities.Database.BeginTransaction();
                try
                {
                    entities.Sirketler.Add(yenisirket);
                    entities.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception exp)
                {
                    transaction.Rollback();
                    this.Text = exp.Message;
                    //throw;
                }

                this.Text = "Yeni Şirket Eklendi";

                SirketListesi();


                //return entities.Sirketler.Select(s => new VSirketler()
                //{
                //    pkSirket = s.pkSirket,
                //    Sirket = s.Sirket,
                //    Yetkili = s.Yetkili
                //}).Take(10).ToList();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Sirketler yenisirket = new Sirketler();
            yenisirket.Sirket = txtSirketAdi.Text;

            using (MTPEntities entities = new MTPEntities())
            {
                //trasn başlat
                DbContextTransaction transaction = entities.Database.BeginTransaction();
                try
                {
                    entities.Sirketler.Add(yenisirket);
                    entities.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception exp)
                {
                    transaction.Rollback();
                    this.Text = exp.Message;
                    //throw;
                }

                this.Text = "Yeni Şirket Eklendi";

                SirketListesi();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sirketler sirketbul = new Sirketler();
            sirketbul.pkSirket = 1;


            using (MTPEntities entities = new MTPEntities())
            {
                //trasn başlat
                DbContextTransaction transaction = entities.Database.BeginTransaction();
                try
                {
                    //bul
                    //List< Sirketler>  sirketbullist = entities.Sirketler.Where(x => x.pkSirket == sirketbul.pkSirket).ToList();
                    sirketbul = entities.Sirketler.FirstOrDefault(x => x.pkSirket== sirketbul.pkSirket);
                    sirketbul.Sirket = txtSirketAdi.Text;
                    //entities.Sirketler.Add(sirketbul);
                    entities.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception exp)
                {
                    transaction.Rollback();
                    this.Text = exp.Message;
                    //throw;
                }

                this.Text = "Şirket güncellendi";

                SirketListesi();
            }

            //using (MTPEntities entities = new MTPEntities())
            //{
            //    this.Text = entities.Sirketler.Count().ToString();
            //}
        }
    }
}

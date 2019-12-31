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
    public partial class frmMesajKutusu : DevExpress.XtraEditors.XtraForm
    {
        int mesajtipi = 0,aktifbuton=0;
        string mesaj="",baslikmesaj = "Başlık";
        public frmMesajKutusu(string _mesaj, string baslik_mesaj, int mesaj_tipi, int aktif_buton)
        {
            InitializeComponent();

            mesajtipi = mesaj_tipi;
            aktifbuton = aktif_buton;
            baslikmesaj = baslik_mesaj;
            mesaj = _mesaj;

            if (_mesaj.Length > 80)
            {
                panel1.Height = 150;
                panel1.Width = 600;
                this.Height = 250;
            }
        }

        private void frmMesajKutusu_Load(object sender, EventArgs e)
        {
            label1.Focus();
            this.Text= baslikmesaj;
            label1.Text = mesaj;
            #region mesaj tipi
            //uyarı
            if (mesajtipi == 1)
            {
                pictureBox1.Image = Properties.Resources.uyari64;
            }
                //hata-
            else if (mesajtipi == 2)
            {
                pictureBox1.Image = Properties.Resources.Error64;
            }
                //soru
            else if (mesajtipi == 3)
            {
                pictureBox1.Image = Properties.Resources.soru64;
                btnEvet.Visible = true;
                btnHayir.Visible = true;
            }
                //ok
            else
                pictureBox1.Image = Properties.Resources.ok_64;
            #endregion

            //#region aktif buton
            //if (aktifbuton == 1)
            //    btnEvet.Focus();
            //else if (aktifbuton == 2)
            //    btnHayir.Focus();
            //else
            //    button1.Focus();
            //#endregion
            timer2.Enabled = true;
        }

        int say = 10;
        private void timer1_Tick(object sender, EventArgs e)
        {
            btnHayir.Text = "Hayır " + say.ToString();
            
            //this.Opacity = this.Opacity - 0.07;
            //if (this.Opacity == 0) Close();

            if (say < 1) Close();

            say--;
        }

        private void btnEvet_Click(object sender, EventArgs e)
        {
            this.Tag = btnEvet.Tag;
            Close();
        }

        private void frmMesajKutusu_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
                Close();
        }

        private void btnHayir_Click(object sender, EventArgs e)
        {
            this.Tag = btnHayir.Tag;
            Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (aktifbuton == 1)
                btnEvet.Focus();
            else if (aktifbuton == 2)
                btnHayir.Focus();
            else
                button1.Focus();

            timer2.Enabled = false;
        }
    }
}
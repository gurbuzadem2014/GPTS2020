using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using GPTS.islemler;
using System.IO;
using System.Media;

namespace GPTS
{
    public partial class frmBarkodYok : DevExpress.XtraEditors.XtraForm
    {
        string _barkod;
        public frmBarkodYok(string barkod)
        {
            InitializeComponent();
            _barkod = barkod;
        }

        private void frmBaBsFormlari_Load(object sender, EventArgs e)
        {
            txtBarkod.Text = _barkod;
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\Sesler\\Yok.wav";

            //string path = "C:\\windows\\media\\ding.wav"; // Müzik adresi
            if (File.Exists(dosya))
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = dosya;// "chord.wav";
                player.Play();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Tag = "1";
            Close();
        }

        private void frmBarkodYok_KeyUp(object sender, KeyEventArgs e)
        {
            if(Keys.Escape==e.KeyCode)
            {
                Close();
            }

            if (e.KeyCode == Keys.F7)
                simpleButton1.PerformClick();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;

namespace GPTS
{
    public partial class frmMesajBox : DevExpress.XtraEditors.XtraForm
    {
        public frmMesajBox(int interval)
        {
            InitializeComponent();
            timer1.Interval = interval;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width-550;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height/2;
            this.Height = 200;//Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = 500;//Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Thread thread1 = new Thread(new ThreadStart(gizlen));
            //thread1.Start();
            this.Opacity = this.Opacity - 0.07;
            if (this.Opacity == 0) Close();
        }

        void gizlen()
        {
            this.Opacity = this.Opacity - 0.07;
            if (this.Opacity == 0) Close();
        }
    }
}
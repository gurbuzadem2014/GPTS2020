using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPTS
{
    public partial class frmYukleniyor : Form
    {
        
        public frmYukleniyor()
        {
            InitializeComponent();
        }
       
        private void frmYukleniyor_Load(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            labelControl1.Visible = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Runtime.InteropServices;

namespace GPTS
{
    public partial class FormScreenKeyboard : DevExpress.XtraEditors.XtraForm
    {
        public FormScreenKeyboard()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        private extern static IntPtr SetActiveWindow(IntPtr handle);
        private const int WM_ACTIVATE = 6;
        private const int WA_INACTIVE = 0;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_ACTIVATE)
            {
                if (((int)m.WParam & 0xFFFF) != WA_INACTIVE)
                {
                    if (m.LParam != IntPtr.Zero)
                    {
                        SetActiveWindow(m.LParam);
                    }
                    else
                    {
                        SetActiveWindow(IntPtr.Zero);
                    }
                }
            }
            base.WndProc(ref m);
        }
        
    }
}
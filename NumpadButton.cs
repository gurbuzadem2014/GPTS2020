using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DevExpress.XtraEditors;
using System.ComponentModel;

namespace GPTS
{
    public class NumpadButton: SimpleButton
    {
        [Browsable(true)]
        public string KeyValue { get; set; }

        private int WM_KEYDOWN = 0x202;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                SendKeys.Send(this.KeyValue);
            }
            else
                base.WndProc(ref m);
        }
    }
}

using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GPTS
{
    public class MyButton:SimpleButton
    {
        [Browsable(true)]
        public string Tutar { get; set; }
        public string Aciklama { get; set; }
        public string cariid { get; set; }
        public string fisno { get; set; }
        public string masaid { get; set; }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            LabelControl lkTutar = new LabelControl();
            lkTutar.Dock = System.Windows.Forms.DockStyle.Bottom;
            lkTutar.Text = Tutar;

            LabelControl lkAciklama = new LabelControl();
            lkAciklama.Dock = System.Windows.Forms.DockStyle.Top;
            lkAciklama.Text = Aciklama;

            this.Controls.Add(lkTutar);//new LabelControl() { Text = Tutar });
            this.Controls.Add(lkAciklama);
        }
    }
}

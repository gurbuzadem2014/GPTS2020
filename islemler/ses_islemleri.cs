using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace GPTS.islemler
{
    public class ses_islemleri
    {
        public void sescal()
        {
            try
            {
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string dosya = exeDiz + "\\Sesler\\siparis.wav";

                //string path = "C:\\windows\\media\\ding.wav"; // Müzik adresi
                if (File.Exists(dosya))
                {
                    SoundPlayer player = new SoundPlayer();
                    player.SoundLocation = dosya;// "chord.wav";
                    player.Play();
                }
            }
            catch (Exception exp)
            {
                formislemleri.Mesajform(exp.Message, "K", 200);
            }
        }
    }
}

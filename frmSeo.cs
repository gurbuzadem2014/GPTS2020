using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace GPTS
{
    public partial class frmSeo : Form
    {
        public frmSeo()
        {
            InitializeComponent();
        }
        int tekrarla = 0;
        string google, facebook, html;


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            html = ((WebBrowser)sender).Document.Body.InnerHtml;

            if (google == "google")
            {
                if (tekrarla == 0)
                {
                    Application.DoEvents();
                    webBrowser1.Document.GetElementById("q").SetAttribute("value", textBox1.Text);
                    webBrowser1.Document.GetElementById("btnG").InvokeMember("click");
                    tekrarla++;
                }
            }
            else if (facebook == "facebook")
            {
                if (tekrarla == 0)
                {
                    Application.DoEvents();
                    webBrowser1.Document.GetElementById("email").SetAttribute("value", textBox2.Text);
                    webBrowser1.Document.GetElementById("pass").SetAttribute("value", textBox3.Text);
                    webBrowser1.Document.Forms[0].InvokeMember("submit");
                    tekrarla++;
                }
            }

            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tekrarla = 0;
            facebook = "facebook";
            google = "";
            webBrowser1.Navigate("http://www.facebook.com");
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tekrarla = 0;
            google = "google";
            facebook = "";
            webBrowser1.Navigate("http://www.google.com");
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "txt dosyası (*.txt)|*.txt";
            sf.ShowDialog();
            if (sf.FileName != "")
            {
                StreamWriter dosyaYaz = new StreamWriter(sf.FileName);
                dosyaYaz.Write(html);
                dosyaYaz.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "html dosyası (*.html)|*.html";
            sf.ShowDialog();
            if (sf.FileName != "")
            {
                File.WriteAllText(sf.FileName, webBrowser1.DocumentText, Encoding.UTF8);
            }
        }

        private void frmSeo_Load(object sender, EventArgs e)
        {

        }

        private void frmSeo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                pbDinazor.Top = pbDinazor.Top - 50;
                Application.DoEvents();
                Thread.Sleep(1000);
                Application.DoEvents();
                pbDinazor.Top = pbDinazor.Top + 50;
            }
        }
    }
}

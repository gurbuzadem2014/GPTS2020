using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using DevExpress.XtraTab;
using System.IO;

namespace GPTS
{
    public partial class ucAnaEkran : DevExpress.XtraEditors.XtraUserControl
    {
        
        public ucAnaEkran()
        {
            InitializeComponent();
        }
        
        private void ucSatisMasa_Load(object sender, EventArgs e)
        {
            ButonlariYukle(false);
        }

       

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
           
        }

        void ButonlariYukle(bool bdegisiklikvar)
        {
            //if (bdegisiklikvar == false)  return;
            int to = 0;
            int lef = 0;

            DataTable dtbutton = DB.GetData(@"select * from Masalar with(nolock) where aktif=1 order by sira_no");

           // int h = 80;//dockPanel1.Height / 7;
           // int w = 110;//dockPanel1.Width / 5;
            try
            {
                for (int i = 0; i < dtbutton.Rows.Count; i++)
                {
                    string pkid = dtbutton.Rows[i]["pkMasalar"].ToString();
                    string gen = dtbutton.Rows[i]["gen"].ToString();
                    if (gen == "") gen = "200";
                    string yuk = dtbutton.Rows[i]["yuk"].ToString();
                    if (yuk == "") yuk = "200";
                    string masa_adi = dtbutton.Rows[i]["masa_adi"].ToString();
                    string masa_aciklama = dtbutton.Rows[i]["masa_aciklama"].ToString();
                    string fkSatislar = dtbutton.Rows[i]["fkSatislar"].ToString();

                    SimpleButton sb = new SimpleButton();
                    System.Drawing.Font fb = new System.Drawing.Font("Arial", 22);
                    System.Drawing.Font fk = new System.Drawing.Font("Arial", 16);

                    sb.AccessibleName = fkSatislar;
                    if (fkSatislar == "0" || fkSatislar == "")
                    {
                        sb.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                        sb.Font = fk;
                    }
                    else
                    {
                        sb.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                        sb.Font = fb;
                    }

                    sb.AccessibleDescription = pkid;
                    sb.Name = "Btn" + (i + 1).ToString();
                    sb.Text = masa_adi;
                    sb.Tag = pkid;
                    //double d = 0;
                    //double.TryParse(SatisFiyati, out d);
                    sb.ToolTip = pkid+"-"+masa_aciklama;//"Satış Fiyatı=" + d.ToString() + "\n Stok Adı:" + Stokadi;
                    sb.ToolTipTitle = pkid+"-"+masa_aciklama;//"Kodu: " + Barcode;
                    sb.Height = int.Parse(yuk);
                    sb.Width = int.Parse(gen);
                    sb.Click += new EventHandler(ButtonClick);
                    //sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseEnter);
                    sb.MouseDown += new System.Windows.Forms.MouseEventHandler(sb_MouseDown);
                    sb.MouseMove += new System.Windows.Forms.MouseEventHandler(sb_MouseMove);
                    sb.MouseUp += new System.Windows.Forms.MouseEventHandler(sb_MouseUp);
                    
                    if (ceOzelDizayn.Checked)
                    {
                        string soldan = dtbutton.Rows[i]["soldan"].ToString();
                        if (soldan == "") soldan = lef.ToString();
                        sb.Left = int.Parse(soldan);

                        string ustden = dtbutton.Rows[i]["ustden"].ToString();
                        if (ustden == "") ustden = to.ToString();
                        sb.Top = int.Parse(ustden);
                    }
                    else
                    {
                        sb.Left = lef;
                        sb.Top = to;
                    }
                    //adı 15 karakterden büyükse
                    if (masa_adi.Length > 15)
                        sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    else
                        sb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                    sb.ContextMenuStrip = contextMenuStrip1;
                    string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                    string imagedosya = exeDiz + "\\MasaResim\\" + pkid + ".png";
                    if (File.Exists(imagedosya))
                    {
                        Image im = new Bitmap(imagedosya);
                        sb.Image = new Bitmap(im, 45, 45);
                        sb.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                    }
                    if (i != 0 && (i + 1) % 3 == 0)
                    {
                        to = 0;
                        lef = lef + int.Parse(gen)+5;
                    }
                    else
                    {
                        to += int.Parse(yuk)+5;
                    }

                    //sb.Show();
                    //sb.SendToBack();
                    //DockPanel p1 = dockManager1.AddPanel(DockingStyle.Left);
                    //p1.Text = "Genel";
                    //DevExpress.XtraEditors.SimpleButton btn = new DevExpress.XtraEditors.SimpleButton();
                    //btn.Text = "Print...";

                    //Label l = new Label();
                    //l.Name = "lab" + (i + 1).ToString();
                    //l.Text = masa_aciklama;
                    //l.Left = sb.Left;
                    //l.Width = sb.Width;
                    //l.Height = 20;//sb.Height-50;
                    //l.Top = 0;//sb.Height - l.Height;
                    //l.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                    ////l.BackColor = System.Drawing.Color.Transparent;
                    ////l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    ////l.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    ////l.Click += new EventHandler(ButtonClick);
                    ////PButtonlar.Controls.Add(sb);
                    //sb.Controls.Add(l);

                    //Button odeme = new Button();
                    //odeme.Name = "bo" + (i + 1).ToString();
                    ////odeme.Text = "0,0 TL";
                    //odeme.Left = sb.Left;
                    //odeme.Width = sb.Width;
                    //odeme.Height = 30;//sb.Height-50;
                    //odeme.Top = sb.Height - l.Height-10;
                    //odeme.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                    //odeme.Click += new EventHandler(ButtonClickOdeme);
                    ////l.BackColor = System.Drawing.Color.Transparent;
                    ////l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    ////l.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    ////l.Click += new EventHandler(ButtonClick);
                    ////PButtonlar.Controls.Add(sb);
                    //sb.Controls.Add(odeme);

                    //l.Show();

                    this.Controls.Add(sb);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu: " + exp.Message);
                throw;
            }
            //if (((SimpleButton)sender).Tag != null)
            //    uruneklemdb(((SimpleButton)sender).Tag.ToString());
            //dockPanel1.Width = 900;
            //dockPanel1.Show();
            //xtraTabControl3.TabPages[xtraTabControl3.SelectedTabPageIndex].Tag = "1";
            //for (int i = 0; i < TabHizliSatisGenel.Controls.Count; i++)
            //{
            //    //if (TabHizliSatisGenel.Controls[i].Name == HizliBarkodName)
            //    //  TabHizliSatisGenel.Controls[i].Text = "BOŞ";
            //    ((SimpleButton)(TabHizliSatisGenel.Controls[i])).Dispose();

            //}
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (cbDizayn.Checked) return;

            if (((SimpleButton)sender).Tag != null)
            {
                string satisid = ((SimpleButton)sender).AccessibleName;
                string masaadi = ((SimpleButton)sender).Text;
                //((SimpleButton)sender).Enabled = false;
                frmSatis Satis = new frmSatis(0,int.Parse(satisid),masaadi,"0");
                Satis.gbBaslik.Tag = ((SimpleButton)sender).Tag;
                Satis.gbBaslik.AccessibleDescription = ((SimpleButton)sender).AccessibleDescription;
                Satis.gbBaslik.AccessibleName = ((SimpleButton)sender).AccessibleName;
                Satis.txtpkSatislar.Text = ((SimpleButton)sender).AccessibleName;
                Satis.Satis1Toplam.Tag = ((SimpleButton)sender).AccessibleName;
                Satis.ShowDialog();
  
                if (Satis.txtpkSatislar.Text == "0")
                {
                    ((SimpleButton)sender).ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;

                    //((SimpleButton)sender).Tag = ((SimpleButton)sender).Tag.ToString();
                    //((SimpleButton)sender).Text = "1.Masa Aç";
                    ((SimpleButton)sender).AccessibleName = "0";
                    ((SimpleButton)sender).ToolTip = "Fiş No = 0";
                }
                else
                {
                    //((SimpleButton)sender).Text = "1.Masa Açık";
                    ((SimpleButton)sender).ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                    //satış ekranında kaydetin altına konuldu
                    //DB.ExecuteSQL("update Masalar set fkSatislar=" + Satis.txtpkSatislar.Text + " where pkMasalar=" + ((SimpleButton)sender).Tag.ToString());
                    ((SimpleButton)sender).AccessibleName = Satis.txtpkSatislar.Text;
                    ((SimpleButton)sender).ToolTip = "Fiş No =" + Satis.txtpkSatislar.Text;
                }
                DB.ExecuteSQL("update Masalar set fkSatislar=" + Satis.txtpkSatislar.Text + " where pkMasalar=" + ((SimpleButton)sender).Tag.ToString());

                ((SimpleButton)sender).Focus();
                Satis.Dispose();

                //((SimpleButton)sender).Enabled = true;
                //MessageBox.Show(((SimpleButton)sender).Tag.ToString());
                //SatisDetayEkle(((SimpleButton)sender).Tag.ToString());
                //yesilisikyeni();

                //KONTROL
                //DB.GetData();
            }
        }

        private void ButtonClickOdeme(object sender, EventArgs e)
        {
            //if (cbDizayn.Checked) return;

            //if (((SimpleButton)sender).Tag != null)
            //{
                frmKasaGiris_Cikis OdemeAl = new frmKasaGiris_Cikis();
                OdemeAl.Tag = "1";
                //OdemeAl.gbBaslik.Tag = ((SimpleButton)sender).Tag;
                //OdemeAl.gbBaslik.AccessibleDescription = ((SimpleButton)sender).AccessibleDescription;
                //OdemeAl.gbBaslik.AccessibleName = ((SimpleButton)sender).AccessibleName;
                //OdemeAl.txtpkSatislar.Text = ((SimpleButton)sender).AccessibleName;
                //OdemeAl.Satis1Toplam.Tag = ((SimpleButton)sender).AccessibleName;
                OdemeAl.ShowDialog();


                //if (OdemeAl.txtpkSatislar.Text == "0")
                //{
                //    ((SimpleButton)sender).ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;

                //    //((SimpleButton)sender).Tag = ((SimpleButton)sender).Tag.ToString();
                //    //((SimpleButton)sender).Text = "1.Masa Aç";
                //    ((SimpleButton)sender).AccessibleName = "0";
                //    ((SimpleButton)sender).ToolTip = "Fiş No = 0";
                //}
                //else
                //{
                    ////((SimpleButton)sender).Text = "1.Masa Açık";
                    //((SimpleButton)sender).ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                    ////satış ekranında kaydetin altına konuldu
                    ////DB.ExecuteSQL("update Masalar set fkSatislar=" + Satis.txtpkSatislar.Text + " where pkMasalar=" + ((SimpleButton)sender).Tag.ToString());
                    //((SimpleButton)sender).AccessibleName = OdemeAl.txtpkSatislar.Text;
                    //((SimpleButton)sender).ToolTip = "Fiş No =" + OdemeAl.txtpkSatislar.Text;
                //}
                //DB.ExecuteSQL("update Masalar set fkSatislar=" + OdemeAl.txtpkSatislar.Text + " where pkMasalar=" + ((SimpleButton)sender).Tag.ToString());
                OdemeAl.Dispose();
                //MessageBox.Show(((SimpleButton)sender).Tag.ToString());
                //SatisDetayEkle(((SimpleButton)sender).Tag.ToString());
                //yesilisikyeni();

                //KONTROL
                //DB.GetData();
            //}
        }

        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis = "";//AccessibleDescription
        //int AcikSatisindex = 1;//hangi satış açık
        //decimal HizliMiktar = 1;
        //bool ilkyukleme = false;
        //short yazdirmaadedi = 1;

        private void sb_MouseEnter(object sender, EventArgs e)
        {
            HizliBarkod = ((SimpleButton)sender).Tag.ToString();
            HizliTop = ((SimpleButton)sender).Top;
            HizliLeft = ((SimpleButton)sender).Left;
            HizliBarkodName = ((SimpleButton)sender).Name;
            pkHizliStokSatis = ((SimpleButton)sender).AccessibleDescription;
        }
        bool suruklenmedurumu = false; //Class seviyesinde bir field(değişken) tanımlıyoruz ki,MouseDown da biz bunu true yapacağız,MouseUpta false değerini alacak ve MouseMove eventında true ise hareket edecek.     
        Point ilkkonum; //Global bir değişken tanımlıyoruz ki, ilk tıkladığımız andaki konumunu çıkarmadığımızda buton mouse imlecinden daha aşağıdan hareket edecektir.
        private void sb_MouseDown(object sender, MouseEventArgs e)
        {
            if (!cbDizayn.Checked) return;

            suruklenmedurumu = true; //işlemi burada true diyerek başlatıyoruz.
            ((SimpleButton)sender).Cursor = Cursors.SizeAll; //SizeAll yapmamımızın amacı taşırken hoş görüntü vermek için
            ilkkonum = e.Location; //İlk konuma gördüğünüz gibi değerimizi atıyoruz.
        }
        private void sb_MouseMove(object sender, MouseEventArgs e)
        {
            if (suruklenmedurumu) // suruklenmedurumu==true ile aynı.
            {
                ((SimpleButton)sender).Left = e.X + ((SimpleButton)sender).Left - (ilkkonum.X);
                // button.left ile soldan uzaklığını ayarlıyoruz. Yani e.X dediğimizde buton üzerinde mouseun hareket ettiği pixeli alacağız + butonun soldan uzaklığını ekliyoruz son olarakta ilk mouseın tıklandığı alanı çıkarıyoruz yoksa butonun en solunda olur mouse imleci. Alttakide aynı şekilde Y koordinati için geçerli
                ((SimpleButton)sender).Top = e.Y + ((SimpleButton)sender).Top - (ilkkonum.Y);
            }
        }
        private void sb_MouseUp(object sender, EventArgs e)
        {
            suruklenmedurumu = false; //Sol tuştan elimizi çektik artık yani sürükle işlemi bitti.
            ((SimpleButton)sender).Cursor = Cursors.Default; //İmlecimiz(Cursor) default değerini alıyor.
        }

        private void fişSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            //MessageBox.Show(HizliBarkod);
            //DB.ExecuteSQL("update Masalar set fkSatislar=0 where pkMasalar=" + HizliBarkod);
            //MasalariYukle(true);

        }

        private void masaDizaynKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string c =xtraTabControl2.CompanyName;
            //for (int i = 0; i < xtraTabControl2.Controls.Count; i++)
            //{
            //    //tab içindeki nesneler
            //    // xtraTabControl2.Controls[0].Controls[0].Left    613 int
            //    for (int j = 0; j < xtraTabControl2.Controls[i].Controls.Count; j++)
            //    {
            //        string id = xtraTabControl2.Controls[i].Controls[j].Tag.ToString();

            //        int l = xtraTabControl2.Controls[i].Controls[j].Left;
            //        int t = xtraTabControl2.Controls[i].Controls[j].Top;
            //        DB.ExecuteSQL("update Masalar set soldan=" + l +
            //            ",ustden=" + t +
            //            " where  pkMasalar=" + id);
            //    }

            //}
        }

        private void masalarıTemizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MasalariTemizle();
        }

        private void masaYerleriSıfırlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DB.ExecuteSQL("update Masalar set soldan=null,ustden=null");
            //MasalariTemizle();
            //MasalariYukle(true);
        }

        private void tümMasalarıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DB.ExecuteSQL("update Masalar set fkSatislar=0");
            //MasalariYukle(true);
        }

        private void masaTanımlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasalar masalar = new frmMasalar();
            masalar.ShowDialog();
        }
    }
}

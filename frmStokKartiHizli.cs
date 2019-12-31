using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using System.Media;
using System.IO;

namespace GPTS
{
    public partial class frmStokKartiHizli : DevExpress.XtraEditors.XtraForm
    {
        public frmStokKartiHizli()
        {
            InitializeComponent();
        }

        private void frmStokKartiHizli_Load(object sender, EventArgs e)
        {
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

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            frmMesajBox Mesaj = new frmMesajBox(200);
            if (tEStokadi.Text == "")
            {
                Mesaj.label1.Text = "Lütfen Stok Adı Giriniz";
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.Show();
                //tEStokadi.Focus();
                return;
            }
            if (SatisFiyati1.Text == "")
            {
                Mesaj.label1.Text = "Lütfen Satış Fiyatını Giriniz";
                Mesaj.label1.BackColor = System.Drawing.Color.Red;
                Mesaj.Show();
                //SatisFiyati1.Focus();
                return;
            }
            Mesaj.Dispose();
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@StokKod", ceBarkod.Text));
            list.Add(new SqlParameter("@Barcode", ceBarkod.Text));
            list.Add(new SqlParameter("@Stokadi", tEStokadi.Text));
            list.Add(new SqlParameter("@AlisFiyati", "0.00"));
            list.Add(new SqlParameter("@SatisFiyati", SatisFiyati1.Value));
            list.Add(new SqlParameter("@Mevcut", "0"));
            list.Add(new SqlParameter("@fkMarka","0"));
            list.Add(new SqlParameter("@fkModel", "0"));
            list.Add(new SqlParameter("@fkStokGrup", "0"));
            list.Add(new SqlParameter("@fkBedenGrupKodu", DBNull.Value));
            list.Add(new SqlParameter("@fkRenkGrupKodu", DBNull.Value));
            string sql = @"INSERT INTO StokKarti (StokKod,Stoktipi,Stokadi,Barcode,AlisFiyati,SatisFiyati,fkMarka,fkModel,fkStokGrup,fkRenkGrupKodu,fkBedenGrupKodu,Mevcut,KritikMiktar,Aktif,CikisAdet,KdvOrani,HizliSatis,satisadedi) 
                VALUES(@StokKod,'ADET',@Stokadi,@Barcode,@AlisFiyati,@SatisFiyati,@fkMarka,@fkModel,@fkStokGrup,@fkRenkGrupKodu,@fkBedenGrupKodu,@Mevcut,0,1,0,0,1,1) SELECT IDENT_CURRENT('StokKarti')";
                string maxid = DB.ExecuteScalarSQL(sql, list);

             if (maxid == "")
                {
                    return;
                }

             if (maxid.Substring(0, 1) == "H")
                 DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz." + "\n" + maxid, Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
             else
             {
                 ArrayList flist = new ArrayList();
                 flist.Add(new SqlParameter("@fkStokKarti", maxid));
                 flist.Add(new SqlParameter("@SatisFiyatiKdvli", SatisFiyati1.Value.ToString().Replace(",", ".")));
                 //TODO:KDV siz hesapla
                 flist.Add(new SqlParameter("@SatisFiyatiKdvsiz", SatisFiyati1.Value.ToString().Replace(",", ".")));
                 DB.ExecuteSQL(@"INSERT INTO SatisFiyatlari (fkStokKarti,fkSatisFiyatlariBaslik,SatisFiyatiKdvli,SatisFiyatiKdvsiz,iskontoYuzde,Aktif) 
values(@fkStokKarti,1,@SatisFiyatiKdvli,@SatisFiyatiKdvsiz,0,1)", flist);
                 this.TopMost = true;
             }
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmStokKarti StokKarti = new frmStokKarti();
            StokKarti.Barkod.Text = ceBarkod.Text;
            DB.pkStokKarti = 0;
            StokKarti.ShowDialog();
            Close();
        }

        private void frmStokKartiHizli_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
           // if (e.KeyCode == Keys.Enter)
             //   SendKeys.SendWait("{TAB}");

        }
    }
}
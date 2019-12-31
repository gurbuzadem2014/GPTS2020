using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Data.OleDb;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraReports.UI;
using GPTS.Include.Data;
using DevExpress.XtraTab;

namespace GPTS
{
    public partial class frmMusteriTopluDegisiklik : DevExpress.XtraEditors.XtraForm
    {
        string HizliBarkod = "0";
        int HizliTop = 0;
        int HizliLeft = 0;
        string HizliBarkodName = "";
        string pkHizliStokSatis="";//AccessibleDescription
        decimal HizliMiktar = 1;

      public frmMusteriTopluDegisiklik()
      {
        InitializeComponent();
        DB.PkFirma = 1;
        this.Width = Screen.PrimaryScreen.WorkingArea.Width - 150;
        this.Height = Screen.PrimaryScreen.WorkingArea.Height - 5;
      }

      private void frmMusteriTopluDegisiklik_Load(object sender, EventArgs e)
      {
          PersonelGetir();

          Yetkiler();

          timer1.Enabled = true;

          vBeklemeListesi();

          FisListesi();
      }

      void vBeklemeListesi()
      {
          vGridControl1.DataSource = DB.GetData("select * from FirmaGuncelle with(nolock) where Siparis=0");
      }

      void FisListesi()
      {
          string sql = @"Select pkFirmaGuncelle,fg.Tarih,k.KullaniciAdi,fg.Aciklama from FirmaGuncelle fg
            LEFT JOIN Kullanicilar k ON fg.fkKullanici=k.pkKullanicilar
            where fg.Siparis=1  order by pkFirmaGuncelle desc";

          lueFis.Properties.DataSource = DB.GetData(sql);
      }

      void Showmessage(string lmesaj,string renk)
      {
          frmMesajBox mesaj = new frmMesajBox(200);
          mesaj.label1.Text = lmesaj;
          if (renk=="K")
              mesaj.label1.BackColor = System.Drawing.Color.Red;
          else if (renk == "B")
              mesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
          else
              mesaj.label1.BackColor = System.Drawing.Color.Blue;
          mesaj.Show();
      }

      void Yetkiler()
      {
//          string sql = @"SELECT  YetkiAlanlari.Yetki, YetkiAlanlari.Sayi, Parametreler.Aciklama10, YetkiAlanlari.BalonGoster
//          FROM  YetkiAlanlari INNER JOIN Parametreler ON YetkiAlanlari.fkParametreler = Parametreler.pkParametreler
//           WHERE  Parametreler.fkModul = 1  and YetkiAlanlari.fkKullanicilar =" + DB.fkKullanicilar;
//          DataTable dtYetkiler = DB.GetData(sql);
//          for (int i = 0; i < dtYetkiler.Rows.Count; i++)
//          {
//              if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
//                  gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
//              if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
//                  xtraTabControl2.Visible = true;
               
//          }
      }

        int x = 0;
        int y = 0;
        int p = 1;
        private void ButtonClick(object sender, EventArgs e)
        {
            if (((SimpleButton)sender).Tag != null)
            {
                SatisDetayEkle(((SimpleButton)sender).Tag.ToString());
                yesilisikyeni();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }
            //gridviv temizle
            
            int sonuc = 0;
            sonuc = DB.ExecuteSQL("DELETE FROM FirmaGuncelleDetay WHERE fkFirmaGuncelle=" + pkFirmaGuncelle.Text);
            sonuc = DB.ExecuteSQL("DELETE FROM FirmaGuncelle WHERE pkFirmaGuncelle=" + pkFirmaGuncelle.Text);
            if (sonuc != 1)
              MessageBox.Show("Hata Oluştur");
            pkFirmaGuncelle.Text = "0";
            
            timer1.Enabled = true;
            
            yesilisikyeni();
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                yesilisikyeni();
                return;
            }
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.ExecuteSQL("DELETE FROM FirmaGuncelleDetay WHERE pkFirmaGuncelleDetay=" + dr["pkFirmaGuncelleDetay"].ToString());

            gridView1.DeleteSelectedRows();

            if (gridView1.DataRowCount == 0)
            {
                DB.ExecuteSQL("DELETE FROM FirmaGuncelle WHERE pkFirmaGuncelle=" + pkFirmaGuncelle.Text);
            }

            yesilisikyeni();
        }

        private void repositoryItemButtonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
         if (e.Control && e.Shift && gridView1.DataRowCount > 0)
         {
                 DataRow dr = gridView1.GetDataRow(gridView1.DataRowCount - 1);

                 SatisDetayEkle(dr["Barcode"].ToString());

                 yesilisikyeni();

                 gridView1.ShowEditor();
                 gridView1.ActiveEditor.SelectAll();
                 gridView1.CloseEditor();
                 return;
         }
        if (e.KeyCode == Keys.Enter)
        {
            string kod=
            ((DevExpress.XtraEditors.ButtonEdit)(((DevExpress.XtraEditors.ButtonEdit)(sender)).Properties.OwnerEdit)).Text;
            if (kod == "" && gridView1.DataRowCount == 0)
            {
                yesilisikyeni();
                return;
            }

            SatisDetayEkle(kod);
            yesilisikyeni();
        }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string pkFirma = "0";
            frmMusteriAraSiparis MusteriAra = new frmMusteriAraSiparis();
            MusteriAra.ShowDialog();
            if (MusteriAra.Tag.ToString() == "0") 
                return;

            for (int i = 0; i < MusteriAra.gridView1.DataRowCount; i++)
            {
                DataRow dr = MusteriAra.gridView1.GetDataRow(i);

                if (dr["Sec"].ToString() != "True") continue;

                pkFirma = dr["pkFirma"].ToString();

                if (pkFirmaGuncelle.Text == "0")
                {
                    YeniFirmaGuncelle();
                }
                if (DB.GetData("select * from FirmaGuncelleDetay with(nolock) where fkFirmaGuncelle="+
                    pkFirmaGuncelle.Text + " and fkFirma=" + pkFirma).Rows.Count == 0)
                {

                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkFirma", pkFirma));
                    list.Add(new SqlParameter("@fkFirmaGuncelle", pkFirmaGuncelle.Text));   
                    //list.Add(new SqlParameter("@Mesaj", mesaj));
                    DB.ExecuteSQL("INSERT INTO FirmaGuncelleDetay (fkFirma,fkFirmaGuncelle,Durumu,OncekiFiyatdanSatDurumu)"+
                        "values(@fkFirma,@fkFirmaGuncelle,0,0)", list);
                }
            }
            yesilisikyeni();
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.FocusedRowHandle != -2147483647)
            gridView1.AddNewRow();
        }

        void yesilisikyeni()
        {
            SatisDetayGetir(pkFirmaGuncelle.Text);

            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.CloseEditor();
            SendKeys.Send("{ENTER}");
        }

        void satiskaydet(bool yazdir, bool odemekaydedildi)
        {
//            ArrayList list = new ArrayList();
//            list.Add(new SqlParameter("@pkStokFiyatGuncelle", pkFirmaGuncelle.Text));
//            list.Add(new SqlParameter("@AlinanPara", "0"));
//            list.Add(new SqlParameter("@ToplamTutar", aratoplam.Value.ToString().Replace(",", ".")));
//            list.Add(new SqlParameter("@fkSatisDurumu", "2"));//lueSatisTipi.EditValue.ToString()));
//            list.Add(new SqlParameter("@OdemeSekli", "Nakit"));//cbOdemeSekli.EditValue.ToString()));
//            string sonuc = DB.ExecuteSQL(@"UPDATE StokFiyatGuncelle SET Siparis=1,AlinanPara=@AlinanPara,ToplamTutar=@ToplamTutar,
//fkSatisDurumu=@fkSatisDurumu,OdemeSekli=@OdemeSekli,GuncellemeTarihi=getdate() where pkStokFiyatGuncelle=@pkStokFiyatGuncelle", list);
//            if (sonuc.Length>1 && sonuc.Substring(1, 1) == "H")
//            {
//                Showmessage("Hata Oluştur: " + sonuc ,"K");
//                return;
//            }
//            for (int i = 0; i < gridView1.DataRowCount; i++)
//            {
//                DataRow dr = gridView1.GetDataRow(i);
//                DB.ExecuteSQL("UPDATE SatisFiyatlari SET SatisFiyatiKdvli=" + dr["NakitFiyat"].ToString().Replace(",", ".") +
//                    " where fkStokKarti=" + dr["fkStokKarti"].ToString());
//            }
//            FisListesi();
        }


        void temizle()
        {
            gridControl1.DataSource = null;
        }

        private void simpleButton37_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("update FirmaGuncelle set Siparis=1 where pkFirmaGuncelle=" + pkFirmaGuncelle.Text);

            pkFirmaGuncelle.Text = "0";
            yesilisikyeni();
            FisListesi();

            //DialogResult secim;
            //secim = DevExpress.XtraEditors.XtraMessageBox.Show(cbislemTipi.Text + 
            //    " Kaydedilecek Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            //if (secim == DialogResult.No) return;
            //string sql = "";
            //switch (xtraTabControl1.SelectedTabPageIndex)
            //{
            //    //Satış Fiyatları
            //    case 0:
            //        {
            //            for (int i = 0; i < gridView1.DataRowCount; i++)
            //            {
            //                DataRow dr = gridView1.GetDataRow(i);
            //                if (lueFiyatlar.EditValue.ToString() == "1")
            //                {
            //                    //stok kartını güncelle
            //                    ArrayList list = new ArrayList();
            //                    list.Add(new SqlParameter("@fkStokKarti", dr["fkStokKarti"].ToString()));
            //                    list.Add(new SqlParameter("@SatisFiyati", SatisFiyati.Text.Replace(",", ".")));
            //                    sql = "update StokKarti set SatisFiyati=@SatisFiyati where pkStokKarti=@fkStokKarti";
            //                    DB.ExecuteSQL(sql, list);
                                
            //                    //satış Fiyatlarınıda güncelle
            //                    ArrayList list2 = new ArrayList();
            //                    list2.Add(new SqlParameter("@fkStokKarti", dr["fkStokKarti"].ToString()));
            //                    list2.Add(new SqlParameter("@fkSatisFiyatlariBaslik", lueFiyatlar.EditValue.ToString()));
            //                    list2.Add(new SqlParameter("@SatisFiyatiKdvli", SatisFiyati.Text.Replace(",", ".")));

            //                    sql="update SatisFiyatlari set SatisFiyatiKdvli=@SatisFiyatiKdvli where fkStokKarti=@fkStokKarti and fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik";
                                
            //                    DB.ExecuteSQL(sql, list2);
            //                }
            //                else
            //                {
            //                    //satış Fiyatlarınıda güncelle
            //                    ArrayList list2 = new ArrayList();
            //                    list2.Add(new SqlParameter("@fkStokKarti", dr["fkStokKarti"].ToString()));
            //                    list2.Add(new SqlParameter("@fkSatisFiyatlariBaslik", lueFiyatlar.EditValue.ToString()));
            //                    list2.Add(new SqlParameter("@SatisFiyatiKdvli", SatisFiyati.Text.Replace(",", ".")));
            //                    sql = "update SatisFiyatlari set SatisFiyatiKdvli=@SatisFiyatiKdvli where fkStokKarti=@fkStokKarti and fkSatisFiyatlariBaslik=@fkSatisFiyatlariBaslik";
            //                    DB.ExecuteSQL(sql, list2);
            //                }

            //            }
            //            break;
            //        }
            //    //Alış Fiyatı
            //    case 1:
            //        {
            //            for (int i = 0; i < gridView1.DataRowCount; i++)
            //            {
            //                DataRow dr = gridView1.GetDataRow(i);
            //                ArrayList list = new ArrayList();
            //                list.Add(new SqlParameter("@pkStokKarti", dr["fkStokKarti"].ToString()));
            //                list.Add(new SqlParameter("@AlisFiyati", ceYeniAlisFiyati.Text.Replace(",", ".")));
            //                sql = "update StokKarti set AlisFiyati=@AlisFiyati where pkStokKarti=@pkStokKarti";
            //                DB.ExecuteSQL(sql, list);
            //            }
            //            //Alış Fiyatı Kdv Hariç
            //            sql = "update StokKarti set AlisFiyatiKdvHaric=AlisFiyati-(AlisFiyati*KdvOrani)/(100+KdvOrani)";
            //            DB.ExecuteSQL(sql);
            //            break;
            //        }
            //    default:
            //        break;
            //}
            //DB.ExecuteSQL("UPDATE StokFiyatGuncelle SET Siparis=1 WHERE pkStokFiyatGuncelle=" + pkSatisBarkod.Text);
            //btnAciklamaGirisi.ToolTip = "";
            ////cbislemTipi.SelectedIndex = 0;
            ////gridControl1.DataSource = null;
            ////cbOdemeSekli.SelectedIndex = 0;
            //SatisFiyati.Value = 0;
            //SatisDetayGetir(pkSatisBarkod.Text);
            
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton37_Click(sender, e);
        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;
            int i = gridView1.FocusedRowHandle;
            if (i < 0)
            {
                i = gridView1.DataRowCount;
                i--;
            }
            DataRow dr = gridView1.GetDataRow(i);
            string fkFirma = dr["pkFirma"].ToString();
            frmMusteriKarti MusteriKarti = new frmMusteriKarti(fkFirma, "");
            MusteriKarti.ShowDialog();
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            AppearanceDefault appfont = new AppearanceDefault(Color.LightSkyBlue);
            AppearanceDefault appError = new AppearanceDefault(Color.Red);

            DataRow dr = gridView1.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            if (e.Column.FieldName == "SatisFiyati" && dr["SatisFiyati"].ToString() != "" && dr["Adet"].ToString()!="" && dr["AlisFiyati"].ToString() != "")
            {
                decimal SatisTutar = Convert.ToDecimal(dr["SatisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                decimal AlisTutar = Convert.ToDecimal(dr["AlisFiyati"].ToString()) * decimal.Parse(dr["Adet"].ToString());
                if (SatisTutar - AlisTutar <= 0 && (dr["iade"].ToString() == "False" || dr["iade"].ToString() == ""))
                    AppearanceHelper.Apply(e.Appearance, appError);
            }
            if (e.Column.FieldName == "iskontotutar" && dr["iskontotutar"].ToString() != "0")
            {
                AppearanceHelper.Apply(e.Appearance, appfont);
            }
            if (e.Column.FieldName == "Adet" && dr["Adet"].ToString() == "0")
            {
                AppearanceHelper.Apply(e.Appearance, appError);
            }
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string iade = View.GetRowCellDisplayText(e.RowHandle, View.Columns["iade"]);
            //    string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
            //    if (Fiyat.Trim() == "0")
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //    }

            //}
        }

        private void satışİptalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);
        }
        
        private void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            //if (e.Column == null) return;
            //Rectangle rect = e.Bounds;
            //ControlPaint.DrawBorder3D(e.Graphics, e.Bounds);
            //Brush brush =
            //    e.Cache.GetGradientBrush(rect, e.Column.AppearanceHeader.BackColor,
            //    e.Column.AppearanceHeader.BackColor2, e.Column.AppearanceHeader.GradientMode);
            //rect.Inflate(-1, -1);
            //// Fill column headers with the specified colors.
            //e.Graphics.FillRectangle(brush, rect);
            //e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            //// Draw the filter and sort buttons.
            //foreach (DevExpress.Utils.Drawing.DrawElementInfo info in e.Info.InnerElements)
            //{
            //   try
            //   {
            //     DevExpress.Utils.Drawing.ObjectPainter.DrawObject(e.Cache, info.ElementPainter,info.ElementInfo);
            //   }
            //   catch(Exception exp)
            //   {
            //   }

            //}
            //e.Handled = true;
        }

        private void ürünBulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleButton4_Click(sender,e);
        }

        private void lueFis_EditValueChanged(object sender, EventArgs e)
        {
            if (lueFis.EditValue == null) return;
            pkFirmaGuncelle.Text = lueFis.EditValue.ToString();

            FisListesi();

            lueFis.EditValue = null;
            yesilisikyeni();
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            string str = ActiveControl.Name;
            this.Dispose();
        }

        private void simpleButton20_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void satistipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            FisListesi();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (HizliBarkodName == "")
            {
                toolStripMenuItem2.Enabled = false;
                toolStripMenuItem3.Enabled = false;
            }
            else
            {
                toolStripMenuItem2.Enabled = true;
                toolStripMenuItem3.Enabled = true;
            }
        }

        private void pkSatisBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //fiş düzenle
            if (e.KeyCode == Keys.Enter)
            {
                if (pkFirmaGuncelle.Text == "") return;
                lueFis.EditValue = int.Parse(pkFirmaGuncelle.Text);
                if (lueFis.EditValue == null)
                    lueFis.EditValue = pkFirmaGuncelle.Text;
                
            }
        }

        private void lueSatisTipi_EditValueChanged(object sender, EventArgs e)
        {
            FisListesi();
            yesilisikyeni();
        }

        void SatisDetayGetir(string pkSatislar)
        {
            string sql = "";


            sql = @"SELECT  fgd.pkFirmaGuncelleDetay, fgd.fkFirmaGuncelle, fgd.fkFirma, fgd.Durumu, 
                    fgd.OncekiFiyatdanSatDurumu, f.OncekiFiyatdanSat,f.Firmaadi FROM FirmaGuncelleDetay  fgd  with(nolock)
                    LEFT  JOIN Firmalar  f  with(nolock) ON fgd.fkFirma = f.pkFirma
                    where fgd.fkFirmaGuncelle=" + pkFirmaGuncelle.Text;

            gridControl1.DataSource = DB.GetData(sql);

            gridView1.AddNewRow();
        }
        void SatisDetayEkle(string fkFirma)
        {

            if (pkFirmaGuncelle.Text == "0" || pkFirmaGuncelle.Text=="") 
                YeniFirmaGuncelle();

            DataTable dtStokKarti = DB.GetData("SELECT pkFirmaGuncelleDetay FROM FirmaGuncelleDetay with(nolock) where fkFirma='" + fkFirma + "'");
            if (dtStokKarti.Rows.Count == 0)
            {

                string pkStokKarti = dtStokKarti.Rows[0]["pkStokKarti"].ToString();

                ArrayList arr = new ArrayList();
                arr.Add(new SqlParameter("@fkFirmaGuncelle", pkFirmaGuncelle.Text));
                arr.Add(new SqlParameter("@fkFirma", fkFirma));

                string s = DB.ExecuteScalarSQL("insert into FirmaGuncelleDetay (fkFirmaGuncelle,fkFirma,Durumu,OncekiFiyatdanSatDurumu)" +
                " values(@fkSatislar,@fkFirma,0,0", arr);
                if (s != "0")
                {
                    MessageBox.Show(s);
                    return;
                }
            }
        }

        void YeniFirmaGuncelle()
        {
            string sql = "";
            string fisno = "0";
            ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@Tarih", DateTime.Now));
            list.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("@Aciklama", "Firma Güncelle" + DateTime.Now.ToString("yyMMddHHmm")));

            sql = "INSERT INTO FirmaGuncelle (Tarih,fkKullanici,Aciklama)" +
                " VALUES(getdate(),@fkKullanici,@Aciklama) SELECT IDENT_CURRENT('FirmaGuncelle')";
            fisno = DB.ExecuteScalarSQL(sql, list);
            if (fisno == "0")
            {
                MessageBox.Show("Hata oluştu " + fisno);
                return;
            }
            pkFirmaGuncelle.Text = fisno;
            vBeklemeListesi();
        }

        private void pkSatisBarkod_Enter(object sender, EventArgs e)
        {
            pkFirmaGuncelle.Text = "";
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !gridView1.IsEditing && gridView1.FocusedColumn.FieldName == "iskyuzdesanal")
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            DataTable dt = DB.GetData("select top 1 pkFirmaGuncelle from FirmaGuncelle with(nolock) where isnull(Siparis,0)=0");

            //and fkKullanici="  + DB.fkKullanicilar);
            if (dt.Rows.Count > 0)
            {
                pkFirmaGuncelle.Text = dt.Rows[0]["pkFirmaGuncelle"].ToString();

                SatisDetayGetir(pkFirmaGuncelle.Text);
            }
            yesilisikyeni();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (gridView1.DataRowCount == 0) e.Cancel = true;
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                yesilisikyeni();
        }

        private void cmsPopYazir_Opening(object sender, CancelEventArgs e)
        {
            if (((((System.Windows.Forms.ContextMenuStrip)(sender)).SourceControl).Controls.Owner).Name == "btnyazdir")
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = true;
                müşteriSeçToolStripMenuItem.Visible = false;
                müşteriKArtıToolStripMenuItem.Visible = false;
            }
            else
            {
                yazıcıAyarlarıToolStripMenuItem.Visible = false;
                müşteriSeçToolStripMenuItem.Visible = true;
                müşteriKArtıToolStripMenuItem.Visible = true;
            }
        }

        private void sTOKKARTINIDÜZENLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            string pkFirma = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            pkFirma = dr["pkFirma"].ToString();

            frmMusteriKarti MusteriKarti = new frmMusteriKarti(pkFirma, "");
            MusteriKarti.ShowDialog();
        }

        private void simpleButton8_Click_2(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("32");
            SayfaAyarlari.ShowDialog();
        }


        private void groupControl6_MouseClick(object sender, MouseEventArgs e)
        {
            pkFirmaGuncelle.Text = "0";
            timer1.Enabled = true;
            yesilisikyeni();
        }

        private void btnKampanya_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE StokFiyatGuncelleDetay SET NakitFiyat=" + dr["SatisFiyati"].ToString().Replace(",", ".") +
                  " where pkStokFiyatGuncelleDetay=" + dr["pkStokFiyatGuncelleDetay"].ToString());
            }
            yesilisikyeni();
        }

        private void vGridControl1_Click(object sender, EventArgs e)
        {
            DevExpress.XtraVerticalGrid.Rows.BaseRow R;
            R = vGridControl1.GetRowByFieldName("pkStokFiyatGuncelle");
            if (vGridControl1.GetCellValue(R, vGridControl1.FocusedRecord) != null)
            {
                string s = vGridControl1.GetCellValue(R, vGridControl1.FocusedRecord).ToString();
                pkFirmaGuncelle.Text = s;
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPage2)
                gcPlasiyer.Visible = true;
            else
                gcPlasiyer.Visible = false;
        }

        private void lueFis_Enter(object sender, EventArgs e)
        {
            FisListesi();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Liste, Önceki Fiyatdan Sat Durumu Güncellenecektir.Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                int OncekiFiyatdanSat = 0;
                if (checkEdit1.Checked)
                    OncekiFiyatdanSat = 1;

                DB.ExecuteSQL("update Firmalar set OncekiFiyatdanSat=" + OncekiFiyatdanSat.ToString() + " where pkFirma=" + dr["fkFirma"].ToString());
            }

            SatisDetayGetir(pkFirmaGuncelle.Text);
            
        }

     

        private void lueFiyatlar_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pkFirmaGuncelle.Text))
               SatisDetayGetir(pkFirmaGuncelle.Text);
        }

        void PersonelGetir()
        {
            lUEPersonel.Properties.DataSource = DB.GetData("SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller where Plasiyer=1 and AyrilisTarihi is null");
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";
            lUEPersonel.ItemIndex = 0;
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Müşterilere, Plasiyer Atanacaktır. Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                yesilisikyeni();
                return;
            }

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);

                DB.ExecuteSQL("update Firmalar set fkPerTeslimEden=" + lUEPersonel.EditValue.ToString() + " where pkFirma=" + dr["fkFirma"].ToString());
            }

            SatisDetayGetir(pkFirmaGuncelle.Text);
        }

    
       
    }
}

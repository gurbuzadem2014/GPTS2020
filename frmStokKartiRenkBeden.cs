using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GPTS
{
    public partial class frmStokKartiRenkBeden : DevExpress.XtraEditors.XtraForm
    {
        int _fkAlisDetay,_fkStokKarti;
        public frmStokKartiRenkBeden(int fkAlisDetay,int fkStokKarti)
        {
            InitializeComponent();
            _fkAlisDetay = fkAlisDetay;
            _fkStokKarti = fkStokKarti;
        }

        private void frmStokKartiRenkBeden_Load(object sender, EventArgs e)
        {
            RenkGruplari();
            BedenGruplari();
            Getir();

            RBDepoMevcutlari();
        }

        void Getir()
        {
            gridControl4.DataSource = DB.GetData(@"select * from AlisDetayRB with(nolock) where fkAlisDetay=" + _fkAlisDetay);
        }

        void RBDepoMevcutlari()
        {
            gridControl1.DataSource = DB.GetData(@"select skrd.pkStokKartiRBDepo,skd.MevcutAdet as depomevcut,skrd.Mevcut as rbMevcut,skrd.fkRenk,skrd.fkBeden from StokKartiDepo skd
left join StokKartiRBDepo skrd on skrd.fkStokKartiDepo=skd.pkStokKartiDepo
--where skd.fkStokKarti="+ _fkStokKarti);
        }

        void RenkGruplari()
        {
            lueRenk.Properties.DataSource = DB.GetData("SELECT * FROM RenkGrupKodu with(nolock)");
        }

        void BedenGruplari()
        {
            lueBeden.Properties.DataSource = DB.GetData("SELECT * FROM BedenGrupKodu with(nolock)");
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkAlisDetay", _fkAlisDetay));
            list.Add(new SqlParameter("@fkRenk", lueRenk.EditValue.ToString()));
            list.Add(new SqlParameter("@fkBeden", lueBeden.EditValue.ToString()));
            list.Add(new SqlParameter("@Adet", seAdet.Value.ToString().Replace(",",".")));

            DB.ExecuteSQL("insert into AlisDetayRB (fkAlisDetay,fkRenk,fkBeden,Adet) " +
                " values(@fkAlisDetay,@fkRenk,@fkBeden,@Adet)",list);

            Getir();
        }
    }
}
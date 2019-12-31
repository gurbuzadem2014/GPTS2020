using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GPTS
{
    public partial class frmiskonto : DevExpress.XtraEditors.XtraForm
    {
        public frmiskonto()
        {
            InitializeComponent();
        }
        void Createiskontolar_()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("pkiskontolar", typeof(string)));
            dt.Columns.Add(new DataColumn("Yuzde", typeof(Int32)));
            dt.Columns.Add(new DataColumn("iskontutari", typeof(decimal)));
            
            DataRow dr; 

            for (int i = 1; i < 11; i++)
            {
                dr = dt.NewRow();
                dr["pkiskontolar"] = "0";//"iskonto " + i.ToString();
                dr["Yuzde"] = "0";
                dr["iskontutari"] = "0";
                dt.Rows.Add(dr);
            }
            // gridControl1.DataSource = dt;
        }
        void GridYukle()
        {
            DataTable dt;
            if (fkSatisDetay.Text=="")
                 dt  = DB.GetData(@"SELECT i.pkiskontolar,isnull(i.Yuzde,0) as Yuzde,i.fkSatisDetay,i.fkAlisDetay,ad.AlisFiyati,ad.Adet FROM  iskontolar i with(nolock) 
                                      INNER JOIN AlisDetay ad with(nolock) ON i.fkAlisDetay = ad.pkAlisDetay
                                      WHERE ad.pkAlisDetay =" + fkAlisDetay.Text + " order by i.pkiskontolar");
            else
                dt = DB.GetData(@"SELECT i.pkiskontolar,isnull(i.Yuzde,0) as Yuzde,i.fkSatisDetay,i.fkAlisDetay,sd.AlisFiyati,sd.Adet FROM  iskontolar i with(nolock) 
                                      INNER JOIN SatisDetay sd with(nolock) ON i.fkSatisDetay = sd.pkSatisDetay
                                      WHERE sd.pkSatisDetay =" + fkSatisDetay.Text + " order by i.pkiskontolar");

            if (dt.Rows.Count == 0) return;
            for (int i = 0; i < dt.Rows.Count+1; i++)
            {
                if (i == 0)
                {
                    isk1.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk1.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 1)
                {
                    isk2.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk2.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 2)
                {
                    isk3.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk3.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 3)
                {
                    isk4.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk4.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 4)
                {
                    isk5.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk5.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 5)
                {
                    isk6.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk6.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 6)
                {
                    isk7.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk7.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 7)
                {
                    isk8.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk8.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 8)
                {
                    isk9.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk9.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
                else if (i == 9)
                {
                    isk10.Tag = dt.Rows[i]["pkiskontolar"].ToString();
                    isk10.Value = int.Parse(dt.Rows[i]["Yuzde"].ToString());
                }
            }
        }
        private void frmiskonto_Load(object sender, EventArgs e)
        {
            GridYukle();

            isk1.Focus();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (fkSatisDetay.Text == "")
            {
                for (int i = 0; i < 11; i++)
                {
                    if (i == 0)
                    {
                        if (isk1.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk1.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk1.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk1.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk1.Tag.ToString());
                    }
                    else if (i == 1)
                    {
                        if (isk2.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk2.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk1.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk2.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk2.Tag.ToString());
                    }
                    else if (i == 2)
                    {
                        if (isk3.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk3.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk3.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk3.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk3.Tag.ToString());
                    }
                    else if (i == 3)
                    {
                        if (isk4.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk4.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk4.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk4.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk4.Tag.ToString());
                    }
                    else if (i == 4)
                    {
                        if (isk5.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk5.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk5.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk5.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk5.Tag.ToString());
                    }
                    else if (i == 5)
                    {
                        if (isk6.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk6.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk6.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk6.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk6.Tag.ToString());
                    }
                    else if (i == 6)
                    {
                        if (isk7.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk7.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk7.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk7.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk7.Tag.ToString());
                    }
                    else if (i == 7)
                    {
                        if (isk8.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk8.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk8.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk8.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk8.Tag.ToString());
                    }
                    else if (i == 8)
                    {
                        if (isk9.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk9.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk9.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk9.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk9.Tag.ToString());
                    }
                    else if (i == 9)
                    {
                        if (isk10.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkAlisDetay,Yuzde) VALUES(" +
                                fkAlisDetay.Text + "," + isk10.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk10.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk10.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk10.Tag.ToString());
                    }
                }
            }
            else
            {
                for (int i = 0; i < 11; i++)
                {
                    if (i == 0)
                    {
                        if (isk1.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk1.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk1.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk1.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk1.Tag.ToString());
                    }
                    else if (i == 1)
                    {
                        if (isk2.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk2.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk1.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk2.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk2.Tag.ToString());
                    }
                    else if (i == 2)
                    {
                        if (isk3.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk3.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk3.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk3.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk3.Tag.ToString());
                    }
                    else if (i == 3)
                    {
                        if (isk4.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk4.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk4.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk4.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk4.Tag.ToString());
                    }
                    else if (i == 4)
                    {
                        if (isk5.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk5.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk5.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk5.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk5.Tag.ToString());
                    }
                    else if (i == 5)
                    {
                        if (isk6.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk6.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk6.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk6.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk6.Tag.ToString());
                    }
                    else if (i == 6)
                    {
                        if (isk7.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk7.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk7.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk7.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk7.Tag.ToString());
                    }
                    else if (i == 7)
                    {
                        if (isk8.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk8.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk8.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk8.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk8.Tag.ToString());
                    }
                    else if (i == 8)
                    {
                        if (isk9.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk9.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk9.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk9.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk9.Tag.ToString());
                    }
                    else if (i == 9)
                    {
                        if (isk10.Tag.ToString() == "0")
                        {
                            string sonuc = DB.ExecuteScalarSQL("INSERT INTO iskontolar (fkSatisDetay,Yuzde) VALUES(" +
                                fkSatisDetay.Text + "," + isk10.Value.ToString().Replace(",", ".") + ") select IDENT_CURRENT('iskontolar')");

                            isk10.Tag = sonuc;
                        }
                        else
                            DB.ExecuteSQL("UPDATE iskontolar set Yuzde=" + isk10.Value.ToString().Replace(",", ".") +
                                " WHERE pkiskontolar=" + isk10.Tag.ToString());
                    }
                }
            }
            //GridYukle();
            if (fkSatisDetay.Text == "")
                DB.ExecuteSQL("UPDATE AlisDetay SET iskontoyuzdetutar=" +
                iskontoorani.Value.ToString().Replace(",",".") +
                ",iskontotutar=" + iskontoorani.Value.ToString().Replace(",", ".") + 
                " WHERE pkAlisDetay=" + fkAlisDetay.Text);
            else
                DB.ExecuteSQL("UPDATE SatisDetay SET iskontoyuzdetutar=" +
                iskontoorani.Value.ToString().Replace(",", ".") +
                ",iskontotutar=" + iskontoorani.Value.ToString().Replace(",", ".") +
                " WHERE pkSatisDetay=" + fkSatisDetay.Text);

            Close();
        }

        private void frmiskonto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void iat1_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati2.Value = iat1.Value;
        }

        private void iat2_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati3.Value = iat2.Value;
        }

        private void iat3_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati4.Value = iat3.Value;
        }

        private void iat4_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati5.Value = iat4.Value;
        }

        private void iat5_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati6.Value = iat5.Value;
        }
        private void isk1_EditValueChanged(object sender, EventArgs e)
        {
            iat1.Value = ceBirimFiyati.Value - ((ceBirimFiyati.Value * isk1.Value) / 100);
            if (isk1.Value>0)
              isk2.Enabled = true;
            else
              isk2.Enabled = false;
        }

        private void isk2_EditValueChanged(object sender, EventArgs e)
        {
            iat2.Value = ceBirimFiyati2.Value - ((ceBirimFiyati2.Value * isk2.Value) / 100);
            
            if (isk2.Value > 0)
              isk3.Enabled = true;
            else
              isk3.Enabled = false;
        }

        private void isk3_EditValueChanged(object sender, EventArgs e)
        {
            iat3.Value = ceBirimFiyati3.Value - ((ceBirimFiyati3.Value * isk3.Value) / 100);


            if (isk3.Value > 0)
                isk4.Enabled = true;
            else
                isk4.Enabled = false;
        }

        private void isk4_EditValueChanged(object sender, EventArgs e)
        {
            iat4.Value = ceBirimFiyati4.Value - ((ceBirimFiyati4.Value * isk4.Value) / 100);


            if (isk4.Value > 0)
                isk5.Enabled = true;
            else
                isk5.Enabled = false;
        }

        private void isk5_EditValueChanged(object sender, EventArgs e)
        {
            iat5.Value = ceBirimFiyati5.Value - ((ceBirimFiyati5.Value * isk5.Value) / 100);

            if (isk5.Value > 0)
                isk6.Enabled = true;
            else
                isk6.Enabled = false;
        }

        private void isk6_EditValueChanged(object sender, EventArgs e)
        {
            iat6.Value = ceBirimFiyati6.Value - ((ceBirimFiyati6.Value * isk6.Value) / 100);
            
            if (isk6.Value > 0)
                isk7.Enabled = true;
            else
                isk7.Enabled = false;
        }

        private void isk7_EditValueChanged(object sender, EventArgs e)
        {
            iat7.Value = ceBirimFiyati7.Value - ((ceBirimFiyati7.Value * isk7.Value) / 100);

            if (isk7.Value > 0)
                isk8.Enabled = true;
            else
                isk8.Enabled = false;
        }

        private void isk8_EditValueChanged(object sender, EventArgs e)
        {
            iat8.Value = ceBirimFiyati8.Value - ((ceBirimFiyati8.Value * isk8.Value) / 100);

            if (isk8.Value > 0)
                isk9.Enabled = true;
            else
                isk9.Enabled = false;
        }

        private void isk9_EditValueChanged(object sender, EventArgs e)
        {
            iat9.Value = ceBirimFiyati9.Value - ((ceBirimFiyati9.Value * isk9.Value) / 100);

            if (isk9.Value > 0)
                isk10.Enabled = true;
            else
                isk10.Enabled = false;
        }

        private void isk10_EditValueChanged(object sender, EventArgs e)
        {
            iat10.Value = ceBirimFiyati10.Value - ((ceBirimFiyati10.Value * isk10.Value) / 100);
        }

        private void iat6_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati7.Value = iat6.Value;
        }

        private void iat7_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati8.Value = iat7.Value;
        }

        private void iat8_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati9.Value = iat8.Value;
        }

        private void iat9_EditValueChanged(object sender, EventArgs e)
        {
            ceBirimFiyati10.Value = iat9.Value;
        }

        private void iat10_EditValueChanged(object sender, EventArgs e)
        {
            decimal toplam = 0;
            toplam = (iat10.Value * 100) / ceBirimFiyati.Value;
            iskontoorani.Value = 100 - toplam;

            ceToplamiskonto.Value = ceBirimFiyati.Value - iat10.Value;
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            isk1.Value = 0;
            isk2.Value = 0;
            isk3.Value = 0;
            isk4.Value = 0;
            isk5.Value = 0;
            isk6.Value = 0;
            isk7.Value = 0;
            isk8.Value = 0;
            isk9.Value = 0;
            isk10.Value = 0;

            isk1.Focus();
        }
    }
}
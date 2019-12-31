using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;

namespace GPTS.Include.Data
{
    public class DBWeb
    {
        public static string UzakSunucu = "sql2012.isimtescil.net";
        public static string Veritabani = "hitityazilim_db9999";
        public static string SqlUser = "hitityazilim_adem";
        public static string Password = "Hitit9999";


        public static string ConnectionString()
        {
            //string cs = "Data Source=sql2012.isimtescil.net;Initial Catalog=ikizsepet;Persist Security Info=True;User ID=ikizsepet;Password=Hitit9999";
            string cs = "Data Source=" + UzakSunucu + ";Initial Catalog=" + Veritabani + ";Persist Security Info=True;User ID=" + SqlUser + ";Password="+Password;
            return cs;
        }
        public static DataTable GetData_Web(string sql)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            adp.SelectCommand.CommandTimeout = 60;

            DataTable dt = new DataTable();
            try
            {

                adp.Fill(dt);
            }
            //catch (Exception e)
            //{
            //    frmMesaj mesaj = new frmMesaj();
            //    mesaj.Text = "Hata Oluştu" + e.Message.ToString();
            //    mesaj.Show();
            //}
            catch (SqlException e)
            {
                 //DataTable dtSanal = new DataTable();
                //dtSanal.Columns.Add(new DataColumn("FisNo", typeof(int)));
                //dt.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("sonuc", typeof(string)));
                DataRow dr = dt.NewRow();
                //dr["FisNo"] = FisDetay.Rows[i]["FisNo"];
                dr["sonuc"] = e.Message.ToString();
                dt.Rows.Add(dr);
                //MessageBox.Show("Veritabanı Hatası Oluştu " + e.Message.ToString());
            }
            finally
            {
                con.Close();
                con.Dispose();
                adp.Dispose();
            }
            return dt;
        }

        public static int ExecuteSQL(string sql)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, con);

            int count = 0;
            try
            {
                con.Open();
                //count = 
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                count = -1;
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama) values(0,'" +
                //e.Message.ToString().Replace("'", "") + "',getdate(),0,'" + sql + "')");
                //logayaz("ExecuteSQL Hatası Oluştur: " + e.Message.ToString(), sql);
                //throw e;
            }
            catch (Exception e)
            {
                count = -1;
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                //e.Message.ToString().Replace("'", "") + ",getdate(),0)");
                //throw e;
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
            return count;
        }

        public static string ExecuteSQL_Web(string sql, ArrayList par)
        {
            string r = "0";
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, con);

            foreach (SqlParameter p in par)
                cmd.Parameters.Add(p);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu: " + exp.Message.ToString();
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                // exp.Message.ToString().Replace("'", "") + ",getdate(),0)");
                //logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
            }

            con.Close();
            con.Dispose();
            cmd.Dispose();
            return r;
        }

        #region Transaction
        public static SqlConnection conTrans = null;
        public static SqlTransaction transaction;
        public static string ExecuteSQLTrans(string sql)
        {
            string r = "0";
            SqlCommand cmd = new SqlCommand(sql, conTrans);
            cmd.Transaction = transaction;
            // Start a local transaction.
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu-ExecuteNonQuery: " + exp.Message.ToString();
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                //                exp.Message.ToString().Replace("'", "") + ",getdate(),0)");
                //logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
            }

            cmd.Dispose();
            return r;
        }

        public static string ExecuteSQLTrans(string sql, ArrayList par)
        {
            string r = "0";
            //if (conTrans == null)
            //{
            //    conTrans = new SqlConnection(ConnectionString());
            //    //transaction = conTrans.BeginTransaction("AdemTransaction");
            //}
            SqlCommand cmd = new SqlCommand(sql, conTrans);
            cmd.Transaction = transaction;
            // Start a local transaction.


            foreach (SqlParameter p in par)
                cmd.Parameters.Add(p);
            try
            {
                //conTrans.Open();
                cmd.ExecuteNonQuery();
                // Attempt to commit the transaction.
                //transaction.Commit();
            }
            catch (SqlException exp)
            {
                //transaction.Rollback();
                r = "Hata Oluştu-ExecuteNonQuery: " + exp.Message.ToString();
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                //                exp.Message.ToString().Replace("'", "") + ",getdate(),0)");
                //logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
            }

            //conTrans.Close();
            //conTrans.Dispose();
            cmd.Dispose();
            return r;
        }

        public static string ExecuteScalarSQLTrans(string sql)
        {
            //if (conTrans==null)
            //conTrans = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, conTrans);
            cmd.Transaction = transaction;
            string r = "";
            try
            {
                //conTrans.Open();
                r = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu-ExecuteScalar: " + exp.Message.ToString();
                //ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                //                exp.Message.ToString().Replace("'", "") + ",getdate(),0)");
                //logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
            }
            finally
            {
                //conTrans.Close();
                //conTrans.Dispose();
                cmd.Dispose();
            }
            return r;
        }

        public static string ExecuteScalarSQLTrans(string sql, ArrayList par)
        {
            //conTrans = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, conTrans);
            cmd.Transaction = transaction;

            foreach (SqlParameter p in par)
                cmd.Parameters.Add(p);
            string r = "H";
            try
            {
                //conTrans.Open();
                r = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu ExecuteScalar trans: " + exp.Message.ToString();
            }
            finally
            {
                //con.Close();
                //con.Dispose();
                cmd.Dispose();
            }
            return r;
        }
        #endregion
    }
}

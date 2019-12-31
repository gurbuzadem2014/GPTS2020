
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Text;

public class DB
{
    public class DbSql
    {

        public string Sql { get; set; }
        public List<object> Parametre { get; set; }

        public DbSql(string sql, List<object> parametreler)
        {
            this.Sql = sql;
            this.Parametre = parametreler;
        }

        public DbSql(string sql)
        {
            this.Sql = sql;
            this.Parametre = null;
        }

        public DbSql(string sql, object parametre)
        {
            this.Sql = sql;
            this.Parametre = new List<object>() { parametre };
        }
    }

    public string GetConnectionString()
    {

        return ConfigurationManager.AppSettings["Baglanti"].ToString();

   }

    public string GetConnectionString(string tns, string userName, string password)
    {
        return "Data Source=" + tns + "; User Id=" + userName + "; Password=" + password + ";";
    }

    public Int64 ExecSql(string sql)
    {
        int sonuc = 0;
        Fonksiyonlar f = new Fonksiyonlar();
        OracleConnection conn = new OracleConnection(this.GetConnectionString());

        try
        {
            conn.Open();
            OracleCommand cmd = new OracleCommand(sql, conn);
            sonuc = cmd.ExecuteNonQuery();
        }
        catch (OracleException oex)
        {
            f.LogYazTxt("ExecSql() Sql: " + sql, oex.StackTrace, oex.Message);
        }
        catch (Exception ex)
        {
            f.LogYazTxt("ExecSql() Sql: " + sql, ex.StackTrace, ex.Message);
        }
        finally
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return sonuc;
    }

    public Int64 ExecSql(string sql, List<object> parametreDegerleri)
    {
        OracleCommand cmd = new OracleCommand(sql);

        if (parametreDegerleri != null)
        {
            for (int paramNo = 0; paramNo < parametreDegerleri.Count; paramNo++)
            {
                cmd.Parameters.AddWithValue("param" + (paramNo + 1).ToString(), parametreDegerleri[paramNo]);
            }
        }

        return this.ExecCmd(cmd);
    }

    public Int64 ExecSql(DbSql dbSql)
    {
        OracleCommand cmd = new OracleCommand(dbSql.Sql);

        if (dbSql.Parametre != null)
        {
            for (int paramNo = 0; paramNo < dbSql.Parametre.Count; paramNo++)
            {
                cmd.Parameters.AddWithValue("param" + (paramNo + 1).ToString(), dbSql.Parametre[paramNo]);
            }
        }

        return this.ExecCmd(cmd);
    }

    public bool ExecSqls(string[] sqls, List<object>[] parametreDegerleri, bool transacted)
    {
        /*  Örnek Kullanım:
            Sqllerde parametre isimleri param1, param2 .. şeklinde tanımlanmalıdır
                 
            DB db = new DB();          

            string sql1 = "INSERT INTO TABLO_ADI_1 VALUES(:param1, 'birşey', :param2, 'başkabirşey', :param3)";
            string sql2 = "INSERT INTO TABLO_ADI_2 VALUES(:param1, :param2)";
            string sql3 = "INSERT INTO TABLO_ADI_3 VALUES('bir şey','başka bir şey')";
            string sql4 = "DELETE TABLO_ADI_4 WHERE ALAN_ADI = :param1";
            string sql5 = "UPDATE TABLO_ADI_5 SET ALAN_ADI_1 = :param1 WHERE ALAN_ADI_2 < :param2 ";
            string[] sqls = {sql1,sql2,sql3,sql4,sql5};

            List<object>[] parameters = new List<object>[5];
            parameters[0] = new List<object>(){"DEGER1","DEGER2", 100};
            parameters[1] = new List<object>(){"DEGER1",DateTime.Now};
            parameters[2] = null;
            parameters[3] = new List<object>() { "DEGER1" };
            parameters[4] = new List<object>() { "DEGER1", 30 };

            bool basari = db.ExecSqls(sqls, parameters, true);                  
        */

        bool basari = true;

        if (parametreDegerleri == null)
        {
            Fonksiyonlar f = new Fonksiyonlar();
            OracleConnection conn = new OracleConnection(this.GetConnectionString());

            if (transacted)
            {
                OracleTransaction transaction = null;

                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    foreach (string sql in sqls)
                    {
                        OracleCommand cmd = new OracleCommand(sql, conn, transaction);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (OracleException oex)
                {
                    basari = false;
                    f.LogYazTxt("ExecSqls()", oex.StackTrace, oex.Message);
                    transaction.Rollback();
                }
                catch (Exception ex)
                {
                    basari = false;
                    f.LogYazTxt("ExecSqls()", ex.StackTrace, ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                try
                {
                    conn.Open();

                    foreach (string sql in sqls)
                    {
                        OracleCommand cmd = new OracleCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OracleException oex)
                {
                    basari = false;
                    f.LogYazTxt("ExecSqls()", oex.StackTrace, oex.Message);
                }
                catch (Exception ex)
                {
                    basari = false;
                    f.LogYazTxt("ExecSqls()", ex.StackTrace, ex.Message);
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }
        else
        {
            OracleCommand[] cmds = new OracleCommand[sqls.Length];
            for (int sqlNo = 0; sqlNo < sqls.Length; sqlNo++)
            {
                cmds[sqlNo] = new OracleCommand(sqls[sqlNo]);
                if (parametreDegerleri[sqlNo] != null)
                {
                    for (int paramNo = 0; paramNo < parametreDegerleri[sqlNo].Count; paramNo++)
                    {
                        cmds[sqlNo].Parameters.AddWithValue("param" + (paramNo + 1).ToString(), parametreDegerleri[sqlNo][paramNo]);
                    }
                }
            }

            basari = this.ExecCmds(cmds, transacted);
        }

        return basari;
    }

    public bool ExecSqls(List<string> sqls, List<List<object>> parametreDegerleri, bool transacted)
    {
        /*  Örnek Kullanım:
            Sqllerde parametre isimleri param1, param2 .. şeklinde tanımlanmalıdır
                 
            DB db = new DB();
              
            string sql1 = "INSERT INTO TABLO_ADI_1 VALUES(:param1, 'birşey', :param2, 'başkabirşey', :param3)";
            string sql2 = "INSERT INTO TABLO_ADI_2 VALUES(:param1, :param2)";
            string sql3 = "INSERT INTO TABLO_ADI_3 VALUES('bir şey','başka bir şey')";
            string sql4 = "DELETE TABLO_ADI_4 WHERE ALAN_ADI = :param1";
            string sql5 = "UPDATE TABLO_ADI_5 SET ALAN_ADI_1 = :param1 WHERE ALAN_ADI_2 < :param2 ";
             
            List<string> sqls = new List<string>();
            sqls.Add(sql1);
            sqls.Add(sql2);
            sqls.Add(sql3);
            sqls.Add(sql4);
            sqls.Add(sql5);

            List<List<object>> parameters = new List<List<object>>();
            parameters.Add(new List<object>() { "DEGER1", "DEGER2", 100 });
            parameters.Add(new List<object>() { "DEGER1", DateTime.Now });
            parameters.Add(null);
            parameters.Add(new List<object>() { "DEGER1" });
            parameters.Add(new List<object>() { "DEGER1", 30 });

            bool basari = db.ExecSqls(sqls, parameters, true);       
        */

        bool basari = true;

        if (parametreDegerleri == null)
        {
            Fonksiyonlar f = new Fonksiyonlar();
            OracleConnection conn = new OracleConnection(this.GetConnectionString());

            if (transacted)
            {
                OracleTransaction transaction = null;

                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    foreach (string sql in sqls)
                    {
                        OracleCommand cmd = new OracleCommand(sql, conn, transaction);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (OracleException oex)
                {
                    basari = false;
                    f.LogYazTxt("ExecSqls()", oex.StackTrace, oex.Message);
                    transaction.Rollback();
                }
                catch (Exception ex)
                {
                    basari = false;
                    f.LogYazTxt("ExecSqls()", ex.StackTrace, ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                try
                {
                    conn.Open();

                    foreach (string sql in sqls)
                    {
                        OracleCommand cmd = new OracleCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OracleException oex)
                {
                    basari = false;
                    f.LogYazTxt("ExecSqls()", oex.StackTrace, oex.Message);
                }
                catch (Exception ex)
                {
                    basari = false;
                    f.LogYazTxt("ExecSqls()", ex.StackTrace, ex.Message);
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }
        else
        {
            List<OracleCommand> cmds = new List<OracleCommand>();
            for (int sqlNo = 0; sqlNo < sqls.Count; sqlNo++)
            {
                cmds.Add(new OracleCommand(sqls[sqlNo]));
                if (parametreDegerleri[sqlNo] != null)
                {
                    for (int paramNo = 0; paramNo < parametreDegerleri[sqlNo].Count; paramNo++)
                    {
                        cmds[sqlNo].Parameters.AddWithValue("param" + (paramNo + 1).ToString(), parametreDegerleri[sqlNo][paramNo]);
                    }
                }
            }

            basari = this.ExecCmds(cmds, transacted);
        }

        return basari;
    }

    public bool ExecSqls(List<DbSql> dbSqls, bool transacted)
    {
        /* Örnek kullanım
        DB db = new DB();
        List<DB.DbSql> dbSqls = new List<DB.DbSql>();
            
        string sql1 = "UPDATE TABLO1 SET ALAN1 = :param1 WHERE ALAN2 = :param2";
        dbSqls.Add(new DB.DbSql(sql1, new List<object>(){"deger1",deger2}));
        string sql2 = "UPDATE TABLO2 SET ALAN1 = 'deger1' WHERE ALAN2 = deger2";
        dbSqls.Add(new DB.DbSql(sql2));
        bool sonuc = db.ExecSqls(dbSqls, true);
        */

        List<OracleCommand> cmds = new List<OracleCommand>();
        for (int sqlNo = 0; sqlNo < dbSqls.Count; sqlNo++)
        {
            cmds.Add(new OracleCommand(dbSqls[sqlNo].Sql));
            if (dbSqls[sqlNo].Parametre != null)
            {
                for (int paramNo = 0; paramNo < dbSqls[sqlNo].Parametre.Count; paramNo++)
                {
                    cmds[sqlNo].Parameters.AddWithValue("param" + (paramNo + 1).ToString(), dbSqls[sqlNo].Parametre[paramNo]);
                }
            }
        }

        return this.ExecCmds(cmds, transacted);
    }

    public Int64 ExecCmd(OracleCommand cmd)
    {
        int sonuc = 0;
        Fonksiyonlar f = new Fonksiyonlar();
        OracleConnection conn = new OracleConnection(this.GetConnectionString());

        try
        {
            conn.Open();
            cmd.Connection = conn;
            sonuc = cmd.ExecuteNonQuery();
        }
        catch (OracleException oex)
        {
            f.LogYazTxt("ExecCmd() Sql: " + cmd.CommandText, oex.StackTrace, oex.Message);
        }
        catch (Exception ex)
        {
            f.LogYazTxt("ExecCmd() Sql: " + cmd.CommandText, ex.StackTrace, ex.Message);
        }
        finally
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return sonuc;
    }

    public bool ExecCmds(OracleCommand[] cmds, bool transacted)
    {
        bool basari = true;
        Fonksiyonlar f = new Fonksiyonlar();
        OracleConnection conn = new OracleConnection(this.GetConnectionString());

        if (transacted)
        {
            OracleTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                foreach (OracleCommand cmd in cmds)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (OracleException oex)
            {
                basari = false;
                f.LogYazTxt("ExecCmds()", oex.StackTrace, oex.Message);
                transaction.Rollback();
            }
            catch (Exception ex)
            {
                basari = false;
                f.LogYazTxt("ExecCmds()", ex.StackTrace, ex.Message);
                transaction.Rollback();
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        else
        {
            try
            {
                conn.Open();

                foreach (OracleCommand cmd in cmds)
                {
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException oex)
            {
                basari = false;
                f.LogYazTxt("ExecCmds()", oex.StackTrace, oex.Message);
            }
            catch (Exception ex)
            {
                basari = false;
                f.LogYazTxt("ExecCmds()", ex.StackTrace, ex.Message);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        return basari;
    }

    public bool ExecCmds(List<OracleCommand> cmds, bool transacted)
    {
        bool basari = true;
        Fonksiyonlar f = new Fonksiyonlar();
        OracleConnection conn = new OracleConnection(this.GetConnectionString());

        if (transacted)
        {
            OracleTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                foreach (OracleCommand cmd in cmds)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (OracleException oex)
            {
                basari = false;
                f.LogYazTxt("ExecCmds()", oex.StackTrace, oex.Message);
                transaction.Rollback();
            }
            catch (Exception ex)
            {
                basari = false;
                f.LogYazTxt("ExecCmds()", ex.StackTrace, ex.Message);
                transaction.Rollback();
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        else
        {
            try
            {
                conn.Open();

                foreach (OracleCommand cmd in cmds)
                {
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException oex)
            {
                basari = false;
                f.LogYazTxt("ExecCmds()", oex.StackTrace, oex.Message);
            }
            catch (Exception ex)
            {
                basari = false;
                f.LogYazTxt("ExecCmds()", ex.StackTrace, ex.Message);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        return basari;
    }

    public DataTable ExecSqlDataTable(string sql)
    {
        Fonksiyonlar f = new Fonksiyonlar();
        DataTable dt = new DataTable();
        OracleDataAdapter da = new OracleDataAdapter(sql, new OracleConnection(this.GetConnectionString()));

        try
        {
            da.Fill(dt);
        }
        catch (OracleException oex)
        {
            f.LogYazTxt("ExecSqlDataTable() Sql: " + sql, oex.StackTrace, oex.Message);
        }
        catch (Exception ex)
        {
            f.LogYazTxt("ExecSqlDataTable() Sql: " + sql, ex.StackTrace, ex.Message);
        }

        return dt;
    }

    public DataTable ExecSqlDataTable(string sql, List<object> parametreDegerleri)
    {
        OracleCommand cmd = new OracleCommand(sql);

        if (parametreDegerleri != null)
        {
            for (int paramNo = 0; paramNo < parametreDegerleri.Count; paramNo++)
            {
                cmd.Parameters.AddWithValue("param" + (paramNo + 1).ToString(), parametreDegerleri[paramNo]);
            }
        }

        return this.ExecCmdDataTable(cmd);
    }

    public DataTable ExecSqlDataTable(DbSql dbSql)
    {
        OracleCommand cmd = new OracleCommand(dbSql.Sql);

        if (dbSql.Parametre != null)
        {
            for (int paramNo = 0; paramNo < dbSql.Parametre.Count; paramNo++)
            {
                cmd.Parameters.AddWithValue("param" + (paramNo + 1).ToString(), dbSql.Parametre[paramNo]);
            }
        }

        return this.ExecCmdDataTable(cmd);
    }

    public DataTable ExecCmdDataTable(OracleCommand cmd)
    {
        Fonksiyonlar f = new Fonksiyonlar();
        DataTable dt = new DataTable();
        cmd.Connection = new OracleConnection(this.GetConnectionString());
        OracleDataAdapter da = new OracleDataAdapter(cmd);

        try
        {
            da.Fill(dt);
        }
        catch (OracleException oex)
        {
            f.LogYazTxt("ExecCmdDataTable() Sql: " + cmd.CommandText, oex.StackTrace, oex.Message);
        }
        catch (Exception ex)
        {
            f.LogYazTxt("ExecCmdDataTable() Sql: " + cmd.CommandText, ex.StackTrace, ex.Message);
        }

        return dt;
    }

    public object ExecSqlOneRow(string sql, List<object> parametreDegerleri)
    {
        object sonuc = null;
        Fonksiyonlar f = new Fonksiyonlar();

        OracleConnection conn = new OracleConnection(this.GetConnectionString());
        OracleCommand cmd = new OracleCommand(sql, conn);

        if (parametreDegerleri != null)
        {
            for (int paramNo = 0; paramNo < parametreDegerleri.Count; paramNo++)
            {
                cmd.Parameters.AddWithValue("param" + (paramNo + 1).ToString(), parametreDegerleri[paramNo]);
            }
        }

        try
        {
            conn.Open();

            //sonuc = cmd.ExecuteOracleScalar();                           
            OracleDataReader dr = cmd.ExecuteReader((CommandBehavior)40); //CommandBehavior.CloseConnection(32) & CommandBehavior.SingleRow (8) 
            while (dr.Read())
            {
                sonuc = dr[0];
            }

            dr.Close();
        }
        catch (OracleException oex)
        {
            f.LogYazTxt("ExecSqlOneRow() Sql: " + sql, oex.StackTrace, oex.Message);
        }
        catch (Exception ex)
        {
            f.LogYazTxt("ExecSqlOneRow() Sql: " + sql, ex.StackTrace, ex.Message);
        }
        finally
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return sonuc;
    }

    public Int64 ExecSqlOneRowInteger(string sql, List<object> parametreDegerleri)
    {
        Int64 sonuc = -999;
        object sonucObj = ExecSqlOneRow(sql, parametreDegerleri);

        if (sonucObj != null)
            Int64.TryParse(sonucObj.ToString(), out sonuc);

        return sonuc;
    }

    public string ExecSqlOneRowString(string sql, List<object> parametreDegerleri)
    {
        string sonuc = "";
        object sonucObj = ExecSqlOneRow(sql, parametreDegerleri);

        if (sonucObj != null)
            sonuc = sonucObj.ToString();

        return sonuc;
    }

    public int ExecSp(string spName, Dictionary<string, object> inParams, ref Dictionary<string, object> outParams)
    {
        //http://www.sqlteam.com/article/stored-procedures-returning-data
        int sonuc = 0;
        Fonksiyonlar f = new Fonksiyonlar();

        OracleConnection conn = new OracleConnection(this.GetConnectionString());
        OracleCommand cmd = new OracleCommand(spName, conn);
        cmd.CommandType = CommandType.StoredProcedure;

        try
        {
            foreach (string inParamName in inParams.Keys)
            {
                OracleParameter paramIn = new OracleParameter(inParamName, inParams[inParamName]);
                paramIn.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(paramIn);
            }

            //foreach (string outParamName in outParams.Keys)
            //{
            //    OracleParameter paramOut = new OracleParameter(outParamName);
            //    paramOut.Direction = ParameterDirection.Output;
            //    cmd.Parameters.Add(paramOut);
            //}

            conn.Open();
            sonuc = cmd.ExecuteNonQuery();
        }
        catch (OracleException oex)
        {
            f.LogYazTxt("ExecSp()", oex.StackTrace, oex.Message);
        }
        catch (Exception ex)
        {
            f.LogYazTxt("ExecSp()", ex.StackTrace, ex.Message);
        }
        finally
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return sonuc;
    }

    //tek kolon donduren bir sql'in sonucunu string listesine atar
    public List<string> ListeOlustur(string sql)
    {
        List<string> liste = null;
        DB db = new DB();

        DataTable dt = db.ExecSqlDataTable(sql);

        if (dt != null && dt.Rows.Count > 0)
        {
            liste = new List<string>();

            for (int elemanNo = 0; elemanNo < dt.Rows.Count; elemanNo++)
            {
                liste.Add(dt.Rows[elemanNo][0].ToString());
            }
        }

        return liste;
    }

    //tek kolon donduren bir sql'in sonucunu string listesine atar
    public List<string> ListeOlustur(string sql, List<object> parametreDegerleri)
    {
        List<string> liste = null;
        DB db = new DB();

        DataTable dt = db.ExecSqlDataTable(sql, parametreDegerleri);

        if (dt != null && dt.Rows.Count > 0)
        {
            liste = new List<string>();

            for (int elemanNo = 0; elemanNo < dt.Rows.Count; elemanNo++)
            {
                liste.Add(dt.Rows[elemanNo][0].ToString());
            }
        }

        return liste;
    }
}

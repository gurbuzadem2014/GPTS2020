using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using GPTS.Include.Data;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using GPTS.islemler;

namespace GPTS
{
    public class DB
    {
        public static string Yetkilikodu;
        public static string sifre;
        public static string kul;
        public static string kullaniciadi;
        public static string fkKullanicilar="1";
        public static string uzakipadresi;
        public static string uzakdbsifre="hitit9999";
        public static bool uzaksqlbaglandimi;
        public static string Dil = "Türkçe";
        public static int firmagiriscikis = 1;
        public static int pkProjeler = 0;
        public static int PkFirma = 0;
        public static int pkTedarikciler = 0;
        public static int pkStokKarti = 0;
        public static string ProjeAdi = "Hitit Prof 2012";
        public static string FirmaAdi = "Firma Seçilmedi";
        public static string TedarikciAdi = "Tedarikci Seçilmedi";
        public static int pkPersoneller = 0;
        public static string PersonellerBaslik = "Personel Seçiniz.";
        public static string vol = "12";
        public static int kayitli = 0;
        public static bool direkcik = false;
        public static int pkVardiyalar = 0;
        public static string exeDizini ="";
        public static int pkKasaHareket = 0;
        public static int yil = 0;
        public static int ay = 0;
        public static int pkVardiyaSablon = 0;
        public static int pkSatislar = 0;
        public static int pkSiparisKopyala = 0;
        public static int secilenfkSiparisSablonlari = 0;
        public static int pkAlislar = 0;
        public static int pkHatirlatma = 0;
        public static int pkHatirlatmaAnimsat = 0;
        public static int pkHatirlatma_Copy = 0;
        public static int pkHatirlatma_Cut = 0;
        public static int webpkKullanicilar=0;
        public static string Sektor = "";
        
        public static int TeraziBarkoduBasi1 = 270;
        public static int TeraziBarkoduBasi2 = 280;
        public static int TeraziBarkoduBasi3 = 290;

        public static string VeriTabaniAdresi= "(localdb)\\mssqllocaldb";
        public static string VeriTabaniAdi = "MTP2012";
        public static string VeriTabaniKul = "hitityazilim";
        public static string VeriTabaniSifre = "vxYhtNcm7YU=";

        public static bool girisbasarili = false;

        public DB()
        {
        }
        //public static string musteriac(string pkFirma)
        //{
        //     ucMusteriKontrol musterihareketleri = new ucMusteriKontrol();
        //     DB.PkFirma = int.Parse(pkFirma);
        //     musterihareketleri.ShowDialog();
        //     return "0";
        //}
        public static string ConnectionString()
        {
            exeDizini = Path.GetDirectoryName(Application.ExecutablePath);

            
            //string password = islemler.CryptoStreamSifreleme.Decrypt("Hitit999", VeriTabaniSifre);
            string password = islemler.CryptoStreamSifreleme.md5SifreyiCoz(VeriTabaniSifre);

            string con = "";
            //demo için
            //string win = Environment.GetFolderPath(Environment.SpecialFolder.System);
            //string masa = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string room = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string mdf = exeDizini + "\\Data\\MTP2012.mdf";
            mdf= room + "\\MTP2012.mdf";
            con = "Server=(LocalDB)\\MSSQLLocalDB; Integrated Security=true ;AttachDbFileName=" + mdf;//D:\Data\MyDB1.mdf+";

            //gerçek müşteri
            con ="Data Source=" + DB.VeriTabaniAdresi + ";Initial Catalog=" + VeriTabaniAdi + ";Persist Security Info=True;User ID=" + VeriTabaniKul + ";Password=" + password;
            return con;
        }
        //public OleDbConnection bag = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;Data Source=hitit.mdb");
        //public OleDbCommand kmt = new OleDbCommand();
        //public OleDbDataAdapter adtr = new OleDbDataAdapter();
        //public DataSet dtst = new DataSet();
        public static DataTable GetDatamdb(string sql)
        {
            OleDbConnection bag = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;Data Source=hitit.mdb");
            OleDbDataAdapter adtr = new OleDbDataAdapter(sql,bag);
            //SqlDataAdapter adp = new SqlDataAdapter(sql, bag);
            adtr.SelectCommand.CommandTimeout = 60;

            DataTable dt = new DataTable();
            try
            {
                adtr.Fill(dt);
            }
            catch (Exception e)
            {
                // throw e;
            }
            finally
            {
                bag.Dispose();
                adtr.Dispose();
            }
            return dt;
        }

        public static string mdb()
        {
            OleDbConnection bag = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;Data Source=hitit.mdb");
            OleDbCommand kmt = new OleDbCommand();
            //OleDbDataAdapter adtr = new OleDbDataAdapter();
            DataSet dtst = new DataSet();
            if (bag.State.ToString() == "Open")
                bag.Close();
            bag.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select * From urunler", bag);
            dtst.Clear();
            adtr.Fill(dtst, "urunler");
            string str = dtst.Tables["urunler"].Rows[0]["barkod"].ToString();

            return str;
        }

        //public static string csmdb2()
        //{
//          if (bag.State.ToString() == "Open")
//                bag.Close();
//            tesisadi.Text = Degerler.yetkili;
//            bag.Open();
//            OleDbDataAdapter adtr = new OleDbDataAdapter("select * From ayarlar", bag);
//            dtst.Clear();
//            adtr.Fill(dtst, "ayarlar");
//            ipadresi = dtst.Tables["ayarlar"].Rows[0]["ip"].ToString();
//            if (dtst.Tables["ayarlar"].Rows[0]["localsave"].ToString() != "")
//                localsave = int.Parse(dtst.Tables["ayarlar"].Rows[0]["localsave"].ToString());
//            if (dtst.Tables["ayarlar"].Rows[0]["port"].ToString() != "")
//                comport = int.Parse(dtst.Tables["ayarlar"].Rows[0]["port"].ToString());
            
//                Degerler.smsmesaj = dtst.Tables["ayarlar"].Rows[0]["smsmesaj"].ToString();
                
//            if (dtst.Tables["ayarlar"].Rows[0]["s1altdeger"].ToString() != "")
//                s1altdeger = int.Parse(dtst.Tables["ayarlar"].Rows[0]["s1altdeger"].ToString());
//            if (dtst.Tables["ayarlar"].Rows[0]["s1ustdeger"].ToString() != "")
//                s1ustdeger = int.Parse(dtst.Tables["ayarlar"].Rows[0]["s1ustdeger"].ToString());

//            if (dtst.Tables["ayarlar"].Rows[0]["s2altdeger"].ToString() != "")
//                s2altdeger = int.Parse(dtst.Tables["ayarlar"].Rows[0]["s2altdeger"].ToString());
//            if (dtst.Tables["ayarlar"].Rows[0]["s2ustdeger"].ToString() != "")
//                s2ustdeger = int.Parse(dtst.Tables["ayarlar"].Rows[0]["s2ustdeger"].ToString());
//            if (dtst.Tables["ayarlar"].Rows[0]["s1aktif"].ToString() == "True")
//               Degerler.s1aktif = "1";
//            else
//                Degerler.s1aktif = "0";
//            if (dtst.Tables["ayarlar"].Rows[0]["s2aktif"].ToString() == "True")
//                Degerler.s2aktif = "1";
//            else
//                Degerler.s2aktif = "0";
////hassas
//            int.TryParse(dtst.Tables["ayarlar"].Rows[0]["hassas"].ToString(), out Degerler.hassasdeger);
//            s1altdeger = s1altdeger - Degerler.hassasdeger;
//            s1ustdeger = s1ustdeger + Degerler.hassasdeger;
//            s2altdeger = s2altdeger - Degerler.hassasdeger;
//            s2ustdeger = s2ustdeger + Degerler.hassasdeger;

//            timer1.Interval = 60000 * localsave;
//            adtr.Dispose();
//            //kullanicilar
//            dtst.Clear();
//            if (bag.State.ToString() == "Open")
//                bag.Close();
//            dtst2.Clear();
//            OleDbDataAdapter adtr2 = new OleDbDataAdapter("select * From kullanicilar", bag);
//            adtr2.Fill(dtst2, "ayarlar2");
//            epostaadresi = dtst2.Tables["ayarlar2"].Rows[0]["eposta"].ToString();
//            ceptelno = dtst2.Tables["ayarlar2"].Rows[0]["ceptelno"].ToString();
//            adtr2.Dispose();
//            bag.Close();

//       }

        public static DataTable GetData(string sql)
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
                MessageBox.Show("Veritabanı Hatası Oluştu " + e.Message.ToString());
                logayaz("GetData Hatası Oluştur: " + e.Message.ToString(), sql);
            }
            finally
            {
                con.Close();
                con.Dispose();
                adp.Dispose();
            }
            return dt;
        }

        public static DataTable GetData(string sql, ArrayList par)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            foreach (SqlParameter p in par)
                adp.SelectCommand.Parameters.Add(p);

            DataTable dt = new DataTable();
            try
            {
                adp.Fill(dt);
                //con.Dispose();
                adp.Dispose();
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata Oluştu " + e.Message);
                //throw e;
            }
            return dt;
        }

        public static bool update(string sql)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            con.Open();
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            adp.SelectCommand.CommandTimeout = 60;
            SqlCommand sqlcom = new SqlCommand(sql, con);
            try
            {
                sqlcom.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int ExecuteSQLmdb(string sql)
        {
            OleDbConnection bag = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;Data Source=hitit.mdb");

            //SqlConnection con = new SqlConnection(ConnectionString());
            //SqlCommand cmd = new SqlCommand(sql, con);

            int count = 0;
            try
            {
            if (bag.State.ToString() == "Open")
                bag.Close();
                bag.Open();
                OleDbCommand cm = new OleDbCommand(sql,bag);
                cm.ExecuteNonQuery();
                //OleDbDataAdapter adtr = new OleDbDataAdapter(sql, bag);
                //con.Open();
                //count = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //throw e;
            }
            finally
            {
                bag.Close();
                bag.Dispose();
            }
            return count;
        }

        public static int ExecuteSQL(string sql)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, con);

            int count = 0;
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                count = -1;
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama) values(0,'" +
                e.Message.ToString().Replace("'", "") + "',getdate(),0,'"+sql+"')");
                logayaz("ExecuteSQL Hatası Oluştur: " + e.Message.ToString(),sql);
                MessageBox.Show("Hata: " + e.Message);
            }
            catch (Exception e)
            {
                count = -1;
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                e.Message.ToString().Replace("'","") + ",getdate(),0)");
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

        public static int ExecuteSQL_Sonuc_Sifir(string sql)
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
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama) values(0,'" +
                e.Message.ToString().Replace("'", "") + "',getdate(),0,'" + sql + "')");
                logayaz("ExecuteSQL Hatası Oluştur: " + e.Message.ToString(), sql);
                //throw e;
            }
            catch (Exception e)
            {
                count = -1;
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                e.Message.ToString().Replace("'", "") + ",getdate(),0)");
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

        public static int ExecuteSQLLog(string sql)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, con);

            int count = 0;
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                count = -1;
                logayaz("ExecuteSQLLog :Log Hatası Oluştur" + e.Message.ToString(), sql);
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

        public static string ExecuteSQL(string sql, ArrayList par)
        {
            string sonuc = "0";
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
                sonuc = "Hata Oluştu-ExecuteNonQuery: " + exp.Message.ToString();
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                                exp.Message.ToString().Replace("'", "") + ",getdate(),0)");
                MessageBox.Show("Hata: " + exp.Message);
                logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(),sql);

            }

            con.Close();
            con.Dispose();
            cmd.Dispose();
            return sonuc;
        }

        public static string ExecuteScalarSQL(string sql)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, con);
            string r = "";
            try
            {
                con.Open();
                r = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu-ExecuteScalar: " + exp.Message.ToString();
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                                exp.Message.ToString().Replace("'", "") + ",getdate(),0)");
                logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
            return r;
        }

        #region Transaction 

        public static SqlConnection conTrans = null;
        public static SqlTransaction transaction;
        public static bool trans_basarili=true;
        public static string trans_hata_mesaji ="";

        public static DataTable GetDataTrans(string sql)
        {
            //SqlCommand cmd = new SqlCommand(sql, conTrans);
            //cmd.Transaction = transaction;

            //SqlConnection con = new SqlConnection(ConnectionString());
            SqlDataAdapter adp = new SqlDataAdapter(sql, conTrans);
            adp.SelectCommand.Transaction = transaction;
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
                logayaz("ExecuteSQL Hatası Oluştur: " + e.Message.ToString(), sql);
                //MessageBox.Show("Veritabanı Hatası Oluştu " + e.Message.ToString());
            }
            finally
            {
                //con.Dispose();
                adp.Dispose();
            }
            return dt;
        }

        public static string ExecuteSQLTrans(string sql)
        {
            //string r = "0";
            //if (conTrans == null)
            //{
            //    conTrans = new SqlConnection(ConnectionString());
            //    //transaction = conTrans.BeginTransaction("AdemTransaction");
            //}
            SqlCommand cmd = new SqlCommand(sql, conTrans);
            cmd.Transaction = transaction;
            // Start a local transaction.
            int count = 0;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException exp)
            {
                count = -1;

                logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);

                //r = "Hata Oluştu-ExecuteNonQuery: " + exp.Message.ToString();
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,HataAciklama,Tarih,Sonuc,fkKullanicilar) values(6,'ExecuteSQLTrans','" +
                                exp.Message.ToString().Replace("'", "") + "',getdate(),0" + DB.fkKullanicilar + ")");
            }

            cmd.Dispose();
            return count.ToString();
        }

        public static string ExecuteSQLTrans(string sql, ArrayList par)
        {
            string r = "0";
           
            SqlCommand cmd = new SqlCommand(sql, conTrans);
            cmd.Transaction = transaction;
            // Start a local transaction.
            foreach (SqlParameter p in par)
                cmd.Parameters.Add(p);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu-ExecuteNonQuery: " + exp.Message.ToString();

                logayaz("ExecuteSQLTrans : " + r, sql);

                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,HataAciklama,Tarih,Sonuc,fkKullanicilar) values(6,'ExecuteSQLTrans','" +
                                exp.Message.ToString().Replace("'", "") + "',getdate(),0" + DB.fkKullanicilar + ")");
                
            }

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
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                                exp.Message.ToString().Replace("'", "") + ",getdate(),0)");
                logayaz("ExecuteSQL Hatası Oluştur: " + exp.Message.ToString(), sql);
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
            SqlCommand cmd = new SqlCommand(sql, conTrans);
            cmd.Transaction = transaction;

            foreach (SqlParameter p in par)
                cmd.Parameters.Add(p);

            string r = "H";
            try
            {
                //con.Open();
                r = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException exp)
            {
                r = "Hata Oluştu: " + exp.Message.ToString();
            }
            finally
            {
                cmd.Dispose();
            }
            return r;
        }

        #endregion

        public static string ExecuteScalarSQL(string sql, ArrayList par)
        {
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand(sql, con);
            foreach (SqlParameter p in par)
                cmd.Parameters.Add(p);
            string r = "H";
            try
            {
                con.Open();
                r = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException exp) 
            { 
                r = "Hata Oluştu: "+ exp.Message.ToString(); 
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
            return r;
        }

        public static int ExecuteSQLSa(string sql)
        {
            string scon = "Data Source=" + DB.VeriTabaniAdresi + ";Integrated Security=true;";
            SqlConnection con = new SqlConnection(scon);
            SqlCommand cmd = new SqlCommand(sql, con);

            int count = 0;
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                count = -2;
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama) values(0,'" +
                e.Message.ToString().Replace("'", "") + "',getdate(),0,'" + sql + "')");
                logayaz("ExecuteSQL Hatası Oluştur: " + e.Message.ToString(), sql);
                //throw e;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                count = -1;
                ExecuteSQLLog("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) values(0," +
                e.Message.ToString().Replace("'", "") + ",getdate(),0)");
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

        public static DataTable GetDataSa(string sql)
        {
            string scon = "Data Source=" + DB.VeriTabaniAdresi + ";Integrated Security=true;";
            SqlConnection con = new SqlConnection(scon);
            
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
                //MessageBox.Show("Veritabanı Hatası Oluştu " + e.Message.ToString());
                logayaz("GetData Hatası Oluştur: " + e.Message.ToString(), sql);
            }
            finally
            {
                con.Dispose();
                adp.Dispose();
            }
            return dt;
        }

        public static bool InternetVarmi_()
        {
          try
           {
                System.Net.Sockets.TcpClient kontrol_client = new System.Net.Sockets.TcpClient("www.google.com", 80);
                kontrol_client.Close();
                return true;
           }
          catch
           {
                return  false;
           }
        }
        //public static bool InternetVarmi22()
        //{
        //    cinternetvarmi iv = new cinternetvarmi();
        //    return iv.Asyivarmi();
        //}

        public static bool InternetVarmi11()
        {
            cinternetvarmi iv = new cinternetvarmi();
            return iv.InternetVarmi2();
        }

        public static bool InternetVarmi3()
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply pingDurumu = ping.Send("www.google.com");//IPAddress.Parse("64.15.112.45"));

                if (pingDurumu.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return true;
                    //Console.WriteLine("İnternet bağlantısı var");
                }
                else
                {
                    return false;
                    //Console.WriteLine("İnternet bağlantısı yok");
                }
                //Console.ReadKey();
                //return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool EmailKontrol(string email)
        {
            string pattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
           + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
           + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
           + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            if (String.IsNullOrEmpty(email)) return Regex.IsMatch(email, pattern);
            else return false;
        }
        public static string epostagonder(string kime, string mesaj, string dosya, string Subject)
        {
            if (mesaj == "") return "Lütfen Mesaj Giriniz";

            string sirket = "",
                Host = "mail.hitityazilim.com",
                GonderenEposta = "destek@hitityazilim.com",
                GonderenEpostaSifre = "TEKsql@3653",
                Port = "587", ccEposta = "", bccEposta = "";

            DataTable dt = DB.GetData("select top 1 * from Sirketler with(nolock)");
            if (dt.Rows.Count > 0)
            {
                sirket = dt.Rows[0]["Sirket"].ToString();
                Host = dt.Rows[0]["Host"].ToString();
                GonderenEposta = dt.Rows[0]["GonderenEposta"].ToString();
                //string epostasifresi = islemler.CryptoStreamSifreleme.Decrypt("Hitit999", dt.Rows[0]["GonderenEpostaSifre"].ToString());
                string epostasifresi = islemler.CryptoStreamSifreleme.md5SifreyiCoz(dt.Rows[0]["GonderenEpostaSifre"].ToString());
                GonderenEpostaSifre = epostasifresi;//dt.Rows[0]["GonderenEpostaSifre"].ToString();
                Port = dt.Rows[0]["Port"].ToString();

                ccEposta = dt.Rows[0]["ccEposta"].ToString();
                bccEposta = dt.Rows[0]["bccEposta"].ToString();
            }

            try
            {
                SmtpClient smtpclient = new SmtpClient();
                smtpclient.Port = int.Parse(Port);   //Smtp Portu (Sunucuya Göre Değişir)25
                smtpclient.Host = Host;//"smtp.gmail.com";  ;Smtp Hostu (Gmail smtp adresi bu şekilde)
                smtpclient.EnableSsl = Degerler.EnableSsl;   //Sunucunun SSL kullanıp kullanmadıgı
                smtpclient.Credentials = new NetworkCredential(GonderenEposta, GonderenEpostaSifre);

                MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("gurbuzadem@gmail.com", "www.hitityazilim.com"); //Gidecek Mail Adresi ve Görünüm Adınız
                mail.From = new MailAddress(GonderenEposta, Host.Replace("mail", "www")); //Gidecek Mail Adresi ve Görünüm Adınız

                if (ccEposta.Length > 5)
                    mail.CC.Add(ccEposta);//bilgi
                if (bccEposta.Length > 5)
                    mail.Bcc.Add(bccEposta);//gizli

                mail.To.Add(kime); //Kime Göndereceğiniz
                                   //mail.To.Add("info@hitityazilim.com");

                if (Subject == "") Subject = mesaj.Substring(0, 5) + "...";
                mail.Subject = Subject;// "Kayıt İşlemi";    //Emailin Konusu
                mail.Body = sirket + "<br>" + mesaj;
                mail.IsBodyHtml = true;           //Mesajınızın Gövdesinde HTML destegininin olup olmadıgı
                if (dosya != "")
                {
                    Attachment data = new Attachment(dosya, MediaTypeNames.Application.Octet);
                    // Add time stamp information for the file.
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(dosya);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(dosya);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(dosya);
                    // Add the file attachment to this e-mail message.
                    mail.Attachments.Add(data);
                }
                smtpclient.Send(mail);
                mail.Dispose();
                //smtpclient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                return "OK";
            }
            catch (Exception exp)
            {
                logayaz("e-posta", exp.Message);
                return "Hata Oluştu Lütfen E-Posta Bilgileri Kontrol Ediniz!" + exp.Message.ToString();
                //showmessage("Hata Oluştu Lütfen E-Posta Bilgileri Kontrol Ediniz!" + exp.Message.ToString());
            }
        }

        public static bool epostagonder_eski(string kime, string mesaj, string dosya, string Subject)
        {
            if (mesaj == "") return false;
            
            string sirket = "", 
                Host = "mail.hitityazilim.com", 
                GonderenEposta = "destek@hitityazilim.com", 
                GonderenEpostaSifre = "TEKsql@3653",
                Port = "587", ccEposta = "", bccEposta = "";

            DataTable dt =  DB.GetData("select top 1 * from Sirketler with(nolock)");
            if (dt.Rows.Count > 0)
            {
                sirket = dt.Rows[0]["Sirket"].ToString();
                Host = dt.Rows[0]["Host"].ToString();
                GonderenEposta = dt.Rows[0]["GonderenEposta"].ToString();
//                string epostasifresi = islemler.CryptoStreamSifreleme.Decrypt("Hitit999", dt.Rows[0]["GonderenEpostaSifre"].ToString());
                string epostasifresi = islemler.CryptoStreamSifreleme.md5SifreyiCoz(dt.Rows[0]["GonderenEpostaSifre"].ToString());
                GonderenEpostaSifre = epostasifresi;//dt.Rows[0]["GonderenEpostaSifre"].ToString();
                Port = dt.Rows[0]["Port"].ToString();

                ccEposta = dt.Rows[0]["ccEposta"].ToString();
                bccEposta = dt.Rows[0]["bccEposta"].ToString();
            }

            try
            {
                SmtpClient smtpclient = new SmtpClient();
                smtpclient.Port = int.Parse(Port);   //Smtp Portu (Sunucuya Göre Değişir)25
                smtpclient.Host = Host;//"smtp.gmail.com";  ;Smtp Hostu (Gmail smtp adresi bu şekilde)
                smtpclient.EnableSsl = Degerler.EnableSsl;   //Sunucunun SSL kullanıp kullanmadıgı
                smtpclient.Credentials = new NetworkCredential(GonderenEposta, GonderenEpostaSifre); 

                MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("gurbuzadem@gmail.com", "www.hitityazilim.com"); //Gidecek Mail Adresi ve Görünüm Adınız
                mail.From = new MailAddress(GonderenEposta,Host.Replace("mail","www")); //Gidecek Mail Adresi ve Görünüm Adınız

                if (ccEposta.Length>5)
                   mail.CC.Add(ccEposta);//bilgi
                if (bccEposta.Length > 5)
                   mail.Bcc.Add(bccEposta);//gizli

                mail.To.Add(kime); //Kime Göndereceğiniz
                //mail.To.Add("info@hitityazilim.com");
                
                if (Subject == "") Subject = mesaj.Substring(0,5)+"...";
                mail.Subject = Subject;// "Kayıt İşlemi";    //Emailin Konusu
                mail.Body = sirket + "<br>" +mesaj;
                mail.IsBodyHtml = true;           //Mesajınızın Gövdesinde HTML destegininin olup olmadıgı
                if (dosya != "")
                {
                    Attachment data = new Attachment(dosya, MediaTypeNames.Application.Octet);
                    // Add time stamp information for the file.
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(dosya);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(dosya);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(dosya);
                    // Add the file attachment to this e-mail message.
                    mail.Attachments.Add(data);
                }
                smtpclient.Send(mail);
                mail.Dispose();
                //smtpclient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                return true;
            }
            catch (Exception exp)
            {
                logayaz("e-posta", exp.Message);
                return false;
                //showmessage("Hata Oluştu Lütfen Bilgileri Kontrol Ediniz!" + exp.Message.ToString());
            }
        }

        public static string logayaz(string logmetin, string sql)
        {
            //exeDizini = Path.GetDirectoryName(Application.ExecutablePath);
            try
            {
                if (!Directory.Exists(exeDizini + "\\SqlHatalar"))
                    Directory.CreateDirectory(exeDizini + "\\SqlHatalar");

                StreamWriter sw = new StreamWriter(exeDizini + "\\SqlHatalar\\SqlHatalar"+DateTime.Today.ToString("yyMMdd")+".log", true);
                sw.WriteLine(Environment.NewLine + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " - " + sql);
                sw.WriteLine(Environment.NewLine + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " - " + logmetin);
                
                sw.Close();
                return "1";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return "0";
            }
        }

        public static string MD5Convert(string str)
        {

            MD5CryptoServiceProvider pwd = new MD5CryptoServiceProvider();

            return Encrypt(str, pwd);

        }

        private static string Encrypt(string str, HashAlgorithm alg)
        {

            byte[] byteDegeri = System.Text.Encoding.UTF8.GetBytes(str);

            byte[] encryptedByte = alg.ComputeHash(byteDegeri);

            return Convert.ToBase64String(encryptedByte);

        }

        public static string MD5(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        //Base64 Kod Çözücü (Decode) Metod :?
        static public string DecodeFrom64(string encodedData)
         {
         byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
         string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
         return returnValue;
         }

        public static string MD5Sifrele(string metin) 
        { 
            // MD5CryptoServiceProvider nesnenin yeni bir instance'sını oluşturalım. 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider(); 
            //Girilen veriyi bir byte dizisine dönüştürelim ve hash hesaplamasını yapalım. 
            byte[] btr = Encoding.UTF8.GetBytes(metin); btr = md5.ComputeHash(btr); 
            //byte'ları biriktirmek için yeni bir StringBuilder ve string oluşturalım. 
            StringBuilder sb = new StringBuilder(); 
            //hash yapılmış her bir byte'ı dizi içinden alalım ve her birini hexadecimal string olarak formatlayalım. 
            foreach (byte ba in btr) 
            { 
                sb.Append(ba.ToString("x2").ToLower()); 
            } 
            //hexadecimal(onaltılık) stringi geri döndürelim. 
            return sb.ToString(); 
        } 
    }
}
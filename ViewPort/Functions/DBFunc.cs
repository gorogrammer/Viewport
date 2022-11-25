using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ViewPort.Functions
{
    
    class DBFunc
    {
        MySqlConnection conn;
        private string ConnectionString =
            "server= 116.127.242.207;" +
            "Port=3306;" +
            "uid =root;" +
            "pwd=zkfmffh0125!;" +
            "convert zero datetime=True;" +
            "CharSet=utf8";

        private string ConnectionString_Stemco =
    "server= 16.100.29.75;" +
    "Port=3306;" +
    "uid = root;" +
    "pwd=;" +
    "convert zero datetime=True;" +
    "CharSet=utf8";





        public bool DBConnection(int id, string pw)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    string Query = "SELECT * FROM carloDB.User WHERE idChar=" + id;
                    MySqlCommand command = new MySqlCommand(Query, conn);
                    MySqlDataReader rdr = command.ExecuteReader();
                    if (rdr.Read())
                    {
                        if (rdr["Password"].ToString() == pw)
                        {
                            return true;

                        }
                        else
                        {
                            return false;
                        }


                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool DB_DL_UpDate(List<string> DL_Sever, string LotName)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();
                foreach (string dl_List in DL_Sever)
                {
                    string[] dl_split = dl_List.Split(',');
                    string ResultString = dl_split[0] + "." + dl_split[1];
                    string BadLimit = dl_split[2];
                    string Alarm = dl_split[3];

                    string UPDateQuery = @"UPDATE carloDB.BadResultLimit SET ResultString ='" + ResultString
                        + "',BadLimit ='" + BadLimit
                        + "', Alarm ='" + Alarm
                        + "' WHERE ResultString LIKE'" + dl_split[0]
                        + "%'";
                    MySqlCommand command = new MySqlCommand(UPDateQuery, conn);
                    if (command.ExecuteNonQuery() == -1)
                    {
                        return false;
                    }
                    command.Dispose();


                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DB_DL_UpLoad(List<string> DL,List<string>DL_Sever)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();
                DL.RemoveAt(0);
                DL.RemoveAt(0);

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    // List<string> Sever_DL = new List<string>();
                    
                            
                     foreach (string dl_list in DL)
                     {
                        if (dl_list.Split(',').Count() == 2)
                        {

                        }
                        else
                        {
                            string[] dl_text = dl_list.Split(',');
                            string CntQuery = "SELECT EXISTS(SELECT * FROM carloDB.BadResultLimit WHERE  ResultString LIKE'" + dl_text[0] + "%') as cnt;";
                            MySqlCommand command = new MySqlCommand(CntQuery, conn);
                            MySqlDataReader rdr = command.ExecuteReader();
                            if (rdr.Read())
                            {

                                if (rdr["cnt"].ToString() != "0")
                                {

                                }
                                else
                                {

                                    rdr.Dispose();
                                    //string[] dl_text = dl_list.Split(',');
                                    string[] col_2_3 = dl_text[1].Split(':');
                                    string ResultString = dl_text[0] + "." + col_2_3[0];
                                    dl_text[2] = dl_text[2].Replace("%", "");
                                    dl_text[3] = dl_text[3].Replace("%", "");
                                    string Alarm = col_2_3[0] + "Limit!!";
                                    //dt.Rows.Add(dl_text[0], col_2_3[0], col_2_3[1], dl_text[2], dl_text[3]);
                                    string istQuery = "INSERT INTO carloDB.BadResultLimit(ResultString,BadLimit,Alarm) VALUES('','" + ResultString + "', '" + dl_text[3] + "','" + Alarm + "')";
                                    MySqlCommand istcommand = new MySqlCommand(istQuery, conn);
                                    istcommand.ExecuteNonQuery();
                                    istcommand.Dispose();
                                }
                                if (!rdr.IsClosed)
                                    rdr.Dispose();
                                string SDIPString = string.Empty;
                                string DataQuery = "SELECT * FROM carloDB.BadResultLimit WHERE ResultString LIKE'" + dl_text[0] + "%'";
                                MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
                                MySqlDataReader drdr = DataCommand.ExecuteReader();

                                if (drdr.Read())
                                {
                                    string ResultString = drdr["ResultString"].ToString();
                                    string BadLimit = drdr["BadLimit"].ToString();
                                    string Alarm = drdr["Alarm"].ToString();



                                    DL_Sever.Add(ResultString + "," + BadLimit + "," + Alarm);
                                    drdr.Dispose();
                                }
                            }
                        }
                     }
                            
                       
                    
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DBRegister(int ID,string Name,string pwd)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                {


                    string insertQuery = "INSERT INTO User(id,idChar,Authorization,Password) VALUES(" + ID + ", '" + Name + "','user', '" + pwd + "')";
                    MySqlCommand command = new MySqlCommand(insertQuery, conn);

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
    

}

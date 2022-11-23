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
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();

            if (conn.State == System.Data.ConnectionState.Open)
            {
                string Query = "SELECT * FROM carloDB.User WHERE id=" + id;
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
        public bool DB_DL_UpDate(List<string> DL_Sever, string LotName)
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();
            foreach (string dl_List in DL_Sever) 
            {
                string[] dl_split =  dl_List.Split(',');
                string SDIPName = dl_split[0];
                string AutoLimitCnt = dl_split[1];
                string AutoLimitP = dl_split[2];
                string LimitP = dl_split[3];
                string LimitStandard = dl_split[4];
                string Alarm = dl_split[5];

                string UPDateQuery = @"UPDATE carloDB.MF_LIMIT SET AutoLimitCnt =" + AutoLimitCnt 
                    + ",AutoLimitP =" + AutoLimitP 
                    + ", LimitP =" + LimitP 
                    + ", LimitStandard ='" + LimitStandard 
                    + "', Alarm ='" + Alarm 
                    + "' WHERE LotName ='" + LotName 
                    + "'AND SDIPName ='" + SDIPName 
                    + "'";
                MySqlCommand command = new MySqlCommand(UPDateQuery, conn);
                if(command.ExecuteNonQuery() == -1)
                {
                    return false;
                }
                command.Dispose();

               
            }
            
            return true;
        }
        public bool DB_DL_UpLoad(string LotName,List<string> DL,List<string>DL_Sever)
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
                    string CntQuery = "SELECT EXISTS(SELECT * FROM carloDB.MF_LIMIT WHERE  LotName='" + LotName + "') as cnt;";
                    MySqlCommand command = new MySqlCommand(CntQuery, conn);
                    MySqlDataReader rdr = command.ExecuteReader();
                    if (rdr.Read())
                    {
                        if (rdr["cnt"].ToString() == "0")
                        {
                            command.Dispose();
                            rdr.Dispose();
                            foreach (string dl_list in DL)
                            {

                                if (dl_list.Split(',').Length == 2)
                                {

                                }
                                else
                                {

                                    string[] dl_text = dl_list.Split(',');
                                    string[] col_2_3 = dl_text[1].Split(':');
                                    dl_text[2] = dl_text[2].Replace("%", "");
                                    dl_text[3] = dl_text[3].Replace("%", "");
                                    //dt.Rows.Add(dl_text[0], col_2_3[0], col_2_3[1], dl_text[2], dl_text[3]);
                                    string istQuery = "INSERT INTO carloDB.MF_LIMIT(ID,LotName,SDIPName,AutoLimitCnt,AutoLimitP,LimitP,LimitStandard,Alarm) VALUES('','" + LotName + "', '" + col_2_3[0] + "','" + col_2_3[1] + "','" + dl_text[2] + "'," + dl_text[3] + ",0,'NULL')";
                                    MySqlCommand istcommand = new MySqlCommand(istQuery, conn);
                                    istcommand.ExecuteNonQuery();
                                    //  DL_Sever.Add(LotName + "," + col_2_3[0] + "," + col_2_3[1] + "," + dl_text[2] + "," + dl_text[3] + ",");
                                    //  istcommand.Dispose();
                                }

                                // dataGridView1.DataSource = dt;
                            }
                        }
                        else
                        {
                            command.Dispose();
                            rdr.Dispose();
                            string DataQuery = "SELECT * FROM carloDB.MF_LIMIT WHERE LotName ='" + LotName + "'";
                            MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
                            MySqlDataReader drdr = DataCommand.ExecuteReader();

                            while (drdr.Read())
                            {
                                string SDIPName = drdr["SDIPName"].ToString();
                                string AutoLimitCnt = drdr["AutoLimitCnt"].ToString();
                                string AutoLimitP = drdr["AutoLimitP"].ToString();
                                string LimitP = drdr["LimitP"].ToString();
                                string LimitStandard = drdr["LimitStandard"].ToString();
                                string Alarm = drdr["Alarm"].ToString();



                                DL_Sever.Add(SDIPName + "," + AutoLimitCnt + "," + AutoLimitP + "," + LimitP + "," + LimitStandard + "," + Alarm);
                            }
                            drdr.Dispose();


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

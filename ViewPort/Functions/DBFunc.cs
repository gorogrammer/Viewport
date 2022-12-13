using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ViewPort.Functions
{

   public class DBFunc
    {
        MySqlConnection conn;
        private string ConnectionString_Stemco =
            "server= 116.127.242.207;" +
            "Port=3306;" +
            "uid =root;" +
            "pwd=zkfmffh0125!;" +
            "convert zero datetime=True;" +
            "CharSet=utf8";

        private string ConnectionString =
    "server= 16.100.29.75;" +
    "Port=3306;" +
    "uid = root;" +
    "pwd=;" +
    "convert zero datetime=True;" +
    "CharSet=utf8";

        private string inforamtion = string.Empty;
        private string authorization = string.Empty;
        private string workTime = string.Empty;
        private string lotWorker = string.Empty;
        private string endWorker = string.Empty;
        private string endTime = string.Empty;
        private string timeTaken = string.Empty;
        private string lotImageCnt = string.Empty;
        private string workImageCnt = string.Empty;
        private string idleWork = string.Empty;
        private string isfinallyworker = string.Empty;
        private string deletePath = string.Empty;
        public string DeletePath { get => deletePath; set => deletePath = value; }
        public string Information { get => inforamtion; set => inforamtion = value; }
        public string Authorization { get => authorization; set => authorization = value; }
        public string WorkTime { get => workTime; set => workTime = value; }
        public string LotWorke { get => lotWorker; set => lotWorker = value; }
        public string EndWorke { get => endWorker; set => endWorker = value; }
        public string EndTime { get => endTime; set => endTime = value; }
        public string TimeTaken { get => timeTaken; set => timeTaken = value; }
        public string LotImageCnt { get => lotImageCnt; set => lotImageCnt = value; }
        public string WorkImageCnt { get => workImageCnt; set => workImageCnt = value; }
        public string IdleWork { get => idleWork; set => idleWork = value; }
        public string IsFinallyWorker { get => isfinallyworker; set => isfinallyworker = value; }
        public bool DBConnection(string id, string pw)
        {
            //ㅇㅇㅇㅇㅇㅇㅇㅇㅁㄴㅇ
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                {

                    string Query = "SELECT * FROM carloDB.User WHERE idChar='" + id + "'";
                    MySqlCommand command = new MySqlCommand(Query, conn);
                    MySqlDataReader rdr = command.ExecuteReader();
                    if (rdr.Read())
                    {
                        if (rdr["Password"].ToString() == pw)
                        {
                            Authorization = rdr["Authorization"].ToString();
                            if (Authorization != string.Empty)
                            {
                                Information = id;
                                rdr.Dispose();
                                return true;
                            }
                            else
                            {
                                rdr.Dispose();
                                return true;
                            }

                        }
                        else
                        {
                            rdr.Dispose();
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
        public bool DB_DL_UpLoad(List<string> DL, List<string> DL_Sever)
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
        public bool LimitSetting(List<string> EngDL)
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();
            string DataQuery = "SELECT * FROM carloDB.BadResultLimit";
            MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
            MySqlDataReader drdr = DataCommand.ExecuteReader();

            while (drdr.Read())
            {
                string ResultString = drdr["ResultString"].ToString();
                string BadLimit = drdr["BadLimit"].ToString();
                string Alarm = drdr["Alarm"].ToString();
                EngDL.Add(ResultString + "," + BadLimit + "," + Alarm);
                
            }
            drdr.Dispose();
            return true;
        }

        public bool DBRegister(string num, string pwd,string name)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                {


                    string insertQuery = "INSERT INTO carloDB.User(idChar,Authorization,Password,Name) VALUES(" + "'" + num + "','', '" + pwd + "','" + name + "')";
                    MySqlCommand command = new MySqlCommand(insertQuery, conn);

                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DB_MF_LOT_UpLoad(string LotName,string lotImageCnt,string workImageCnt)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();

                string CntQuery = "SELECT EXISTS(SELECT * FROM carloDB.MF_LOT WHERE  LotName='" + LotName + "') as cnt;";
                MySqlCommand command = new MySqlCommand(CntQuery, conn);
                MySqlDataReader rdr = command.ExecuteReader();
                if (rdr.Read())
                {
                    
                        if (rdr["cnt"].ToString() == "0")
                        {
                            rdr.Dispose();
                            string istQuery = "INSERT INTO carloDB.MF_LOT(LotName,LotImageCnt,WorkImageCnt) VALUES('" +
                                LotName + "', '" +                               
                                lotImageCnt + "', '" +
                                workImageCnt + "')";                           
                            MySqlCommand istcommand = new MySqlCommand(istQuery, conn);
                            istcommand.ExecuteNonQuery();
                            istcommand.Dispose();
                            return true;
                        }
                        else
                        {

                            rdr.Dispose();
                            string DataQuery = "SELECT * FROM carloDB.MF_LOT WHERE LotName ='" + LotName + "'";
                            MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
                            MySqlDataReader drdr = DataCommand.ExecuteReader();
                            if (drdr.Read())
                            {                               
                                LotImageCnt = drdr["LotImageCnt"].ToString();
                                WorkImageCnt = drdr["WorkImageCnt"].ToString();
                                
                            }
                        drdr.Dispose();
                        }
                    }
                                
                return true;

            }
            catch
            {
                return false;
                //System.Windows.Forms.MessageBox.Show(");
            }
        }
        public bool DBLogUpLoad(bool End, string LotName, string lotWorker, string endTime, string timeTaken, string workTime, string idleWork,string isFunallyWorker)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();

               // string CntQuery = "SELECT EXISTS(SELECT * FROM carloDB.MF_LOG WHERE  LotName='" + LotName + "'AND LotWorker ='" +lotWorker +"') as cnt;";
               // MySqlCommand command = new MySqlCommand(CntQuery, conn);
               // MySqlDataReader rdr = command.ExecuteReader();
                
                    if (!End)
                    {
                        
                           
                            string istQuery = "INSERT INTO carloDB.MF_LOG(LotName,LotWorker,WorkTime,EndTime,TimeTaken,IdleWork,IsFinallyWorker) VALUES('" +
                                LotName + "', '" +
                                lotWorker + "','" +
                                workTime + "', '" +                            
                                endTime + "', '" +
                                timeTaken + "', '" +                              
                                idleWork + "', '" +
                                isFunallyWorker + "')";
                            MySqlCommand istcommand = new MySqlCommand(istQuery, conn);
                            istcommand.ExecuteNonQuery();
                            istcommand.Dispose();

                                                  
                            string DataQuery = "SELECT * FROM carloDB.MF_LOG WHERE LotWorker ='" + lotWorker + "' AND LotName ='" + LotName + "' ORDER BY WorkTime DESC LIMIT 1";
                            MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
                            MySqlDataReader drdr = DataCommand.ExecuteReader();
                            if (drdr.Read())
                            {
                                LotWorke = drdr["LotWorker"].ToString();
                                WorkTime = drdr["WorkTime"].ToString();
                                //EndWorke = drdr["EndWorker"].ToString();
                                EndTime = drdr["EndTime"].ToString();
                                TimeTaken = drdr["TimeTaken"].ToString();                              
                                IdleWork = drdr["IdleWork"].ToString();
                                IsFinallyWorker = drdr["IsFinallyWorker"].ToString();
                            }
                    drdr.Dispose();
                    return true;
                    }
                    else if (End)
                    {

                    }
                
                return true;
            }
            catch
            {
                return false;
                //System.Windows.Forms.MessageBox.Show(");
            }
        }
        public bool DBLogUpDate(UseInfomation user, string LotName)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString_Stemco);
                conn.Open();

                string UPDateQuery = @"UPDATE carloDB.MF_LOG SET"
                           + " EndTime ='" + user.EndTime
                           + "', TimeTaken ='" + user.TimeTaken
                           + "', IdleWork ='" + user.IdleWork
                           + "', isFinallyWorker ='" + user.IsFinallyWorker
                           + "' WHERE  LotName ='" + LotName + "' AND LotWorker ='" + user.Name + "' AND WorkTime ='" + user.WorkTime
                           + "'";
                MySqlCommand command = new MySqlCommand(UPDateQuery, conn);
                if (command.ExecuteNonQuery() == -1)
                {
                    return false;
                }
                command.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteUplaod(int delete,string LotName)
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();

            string UPDateQuery = @"UPDATE carloDB.MF_LOT SET"
                        + " WorkImageCnt ='" + delete
                        + "' WHERE  LotName ='" + LotName
                        + "'";
            MySqlCommand command = new MySqlCommand(UPDateQuery, conn);
            if (command.ExecuteNonQuery() == -1)
            {
                return false;
            }
            command.Dispose();
            return true;
        }
        public bool GetDeletePathData(string Machine)
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();           
            string DataQuery = "SELECT * FROM carloDB.MF_DeletePath WHERE MachineType = '" + Machine + "'";
            MySqlCommand istcommand = new MySqlCommand(DataQuery, conn);
            MySqlDataReader drdr = istcommand.ExecuteReader();
            if (drdr.Read())
            {
                DeletePath=drdr["Path"].ToString();
            }
            drdr.Dispose();

            return true;
        }
        public bool InsertDeletePath(List<string> data)
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();

            string PathName = data[0];
            string MachineType = data[1];
            string WorkType = data[2];
            string Path = data[3];

            string istQuery = "INSERT INTO carloDB.MF_DeletePath(PathName,MachineType,WorkType,Path) VALUES('" +
                                PathName + "','" +
                                MachineType + "', '" +
                                WorkType + "', '" +
                                Path + "')";
            MySqlCommand istcommand = new MySqlCommand(istQuery, conn);
            istcommand.ExecuteNonQuery();
            istcommand.Dispose();

            return true;
        }
        public bool UplaodDeletePath(List<string> data,string before)
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();

            string PathName = data[0];
            string MachineType = data[1];
            int WorkType = (int)Enum.Parse(typeof(Enums.WORKTYPE), data[2]);
            string Path = data[3];

            string UPDateQuery = @"UPDATE carloDB.MF_DeletePath SET"
                        + " PathName ='" + PathName
                        + "', MachineType ='" + MachineType
                        + "', WorkType ='" + WorkType.ToString()
                        + "', Path ='" + Path
                        + "' WHERE  PathName ='" + before
                        + "'";
            MySqlCommand command = new MySqlCommand(UPDateQuery, conn);
            if (command.ExecuteNonQuery() == -1)
            {
                return false;
            }
            command.Dispose();
            return true;
        }
        public DataTable GetDeletePath()
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();
            DataTable dt = new DataTable();

            dt.Columns.Add("PathName");
            dt.Columns.Add("MachineType");
            dt.Columns.Add("WorkType");
            
            dt.Columns.Add("Path");

            dt.Columns["WorkType"].DataType = typeof(Enums.WORKTYPE);

            string DataQuery = "SELECT * FROM carloDB.MF_DeletePath";
            MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
            MySqlDataReader drdr = DataCommand.ExecuteReader();
            while (drdr.Read())
            {
                string PathName = drdr["PathName"].ToString();
                string MachineType = drdr["MachineType"].ToString();
                string work = drdr["WorkType"].ToString();
                Enums.WORKTYPE WorkType = (Enums.WORKTYPE)Enum.Parse(typeof(Enums.WORKTYPE), drdr["WorkType"].ToString());
                string Path = drdr["Path"].ToString();                
                dt.Rows.Add(PathName,MachineType,WorkType,Path);
            }
             drdr.Dispose();

                return dt;
        }
        public DataTable GetLog()
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();
            DataTable dt = new DataTable();

            dt.Columns.Add("LotName");
            dt.Columns.Add("LotWorker");
            dt.Columns.Add("WorkTime");
            dt.Columns.Add("EndTime");
            dt.Columns.Add("TimeTaken");
            dt.Columns.Add("IdleWork");
            dt.Columns.Add("IsFinallyWorker");

            string DataQuery = "SELECT * FROM carloDB.MF_LOG";
            MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
            MySqlDataReader drdr = DataCommand.ExecuteReader();
            while (drdr.Read())
            {
                string LotName = drdr["LotName"].ToString();
                string LotWorker = drdr["LotWorker"].ToString();
                string WorkTime = drdr["WorkTime"].ToString();
                string EndTime = drdr["EndTime"].ToString();
                string TimeTaken = drdr["TimeTaken"].ToString();
                string IdleWork = drdr["IdleWork"].ToString();
                string IsFinallyWorker = drdr["IsFinallyWorker"].ToString();

                dt.Rows.Add(LotName, LotWorker, WorkTime, EndTime, TimeTaken, IdleWork, IsFinallyWorker);
            }
            drdr.Dispose();
            return dt;
        }
        public DataTable GetLot()
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();
            DataTable dt = new DataTable();

            dt.Columns.Add("LotName");
            dt.Columns.Add("LotImageCnt");
            dt.Columns.Add("WorkImageCnt");

            //dt.Columns["WorkType"].DataType = typeof(Enum);

            string DataQuery = "SELECT * FROM carloDB.MF_LOT";
            MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
            MySqlDataReader drdr = DataCommand.ExecuteReader();
            while (drdr.Read())
            {
                string LotName = drdr["LotName"].ToString();
                string LotImageCnt = drdr["LotImageCnt"].ToString();
                string WorkImageCnt = drdr["WorkImageCnt"].ToString();
                //string Path = drdr["Path"].ToString();

                dt.Rows.Add(LotName, LotImageCnt, WorkImageCnt);
            }
            drdr.Dispose();
            return dt;
        }
        public DataTable GetUser()
        {
            conn = new MySqlConnection(ConnectionString_Stemco);
            conn.Open();
            DataTable dt = new DataTable();

            dt.Columns.Add("idChar");
            dt.Columns.Add("Authorization");
            dt.Columns.Add("Password");
            dt.Columns.Add("Name");

            dt.Columns["Authorization"].DataType = typeof(Enums.PERMISSION);

            string DataQuery = "SELECT * FROM carloDB.User";
            MySqlCommand DataCommand = new MySqlCommand(DataQuery, conn);
            MySqlDataReader drdr = DataCommand.ExecuteReader();
            while (drdr.Read())
            {
                string idChar = drdr["idChar"].ToString();
                Enums.PERMISSION Authorization =(Enums.PERMISSION)Enum.Parse(typeof(Enums.PERMISSION),drdr["Authorization"].ToString());
                string Password = drdr["Password"].ToString();
                string Name = drdr["Name"].ToString();
                //string Path = drdr["Path"].ToString();

                dt.Rows.Add(idChar, Authorization, Password, Name);
            }
            drdr.Dispose();
            return dt;
        }
    }
}

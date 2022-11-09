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
            "uid=root;" +
            "pwd=zkfmffh0125!;" +
            "database=carloDB;";





        public bool DBConnection(int id, string pw)
        {
            conn = new MySqlConnection(ConnectionString);
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

        
        public bool DBRegister(int ID,string Name,string pwd)
        {
            try
            {
                conn = new MySqlConnection(ConnectionString);
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

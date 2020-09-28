using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViewPortNetwork
{
    public class MYSQL_STR
    {
        public const string CONNECTION_CARLO = "server=192.168.50.55;port=3306;Database=carloDB;Uid=root;pwd=zkfmffh0125!;convert zero datetime=True;CharSet=utf8";
        public const string CONNECTION_STEMCO = "server=16.100.29.75;port=3306;Database=carloDB;uid=root;pwd=;convert zero datetime=True;CharSet=utf8";
    }

    public class NET_DEF
    {
        public const string STEMCO_DB_IP = "16.100.29.75";
        public const string STEMCO_NAS_IP = "16.100.29.91";
        public const string STEMCO_NAS_PATH = @"\\16.100.29.91\Public\Viewport";

        public const string CARLO_DB_IP = "192.168.50.55";
        public const string CARLO_NAS_IP = "192.168.50.5";
        public const string CARLO_NAS_PATH = @"\\192.168.50.5\Carlo FTP Server\ViewPort";

        public const string VIEWPORT_EXE_NAME = "ViewPort.exe";
    }

    public class NetworkFunc
    {
        public static string GetLocalIP()
        {
            string IP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    IP = ip.ToString();

            return IP;
        }

        public static bool Connect(string ip)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;

                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetLastViewPortVersion(string ConnectionString)
        {
            string LastVer = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand Cmd = new MySqlCommand("SELECT * FROM ViewportVer", conn);

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(Cmd))
                {
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    //이름  SDIP상태

                    if (dataSet.Tables[0].Rows.Count < 1)
                        return LastVer;

                    LastVer = dataSet.Tables[0].Rows[dataSet.Tables[0].Rows.Count - 1]["Ver"].ToString();
                }
            }

            return LastVer;
        }

        public static bool SaveVersionToStemcoDB(string Ver, string Description, string Option)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MYSQL_STR.CONNECTION_STEMCO))
                {
                    MySqlCommand Cmd = new MySqlCommand(
                        "INSERT INTO `ViewportVer`(`Ver`, `Description`, `Option`)" + "VALUES(@version, @description, @option)", conn);

                    Cmd.Parameters.AddWithValue("@version", Ver);
                    Cmd.Parameters.AddWithValue("@description", Description);
                    Cmd.Parameters.AddWithValue("@option", Option);
                    Cmd.Connection.Open();
                    Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static bool SaveFilesToStemcoNas(string folderPath, out string Version)
        {
            Version = AssemblyName.GetAssemblyName(Path.Combine(folderPath, NET_DEF.VIEWPORT_EXE_NAME)).Version.ToString();
            string SaveVersionPath = Path.Combine(NET_DEF.STEMCO_NAS_PATH, Version);
            Directory.CreateDirectory(SaveVersionPath);
            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                string FileName = Path.GetFileName(filePath);
                File.Copy(filePath, Path.Combine(SaveVersionPath, FileName), true);
            }
            return true;
        }

    }
}

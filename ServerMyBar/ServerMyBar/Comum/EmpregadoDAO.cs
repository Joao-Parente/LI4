using System;
using MySql.Data.MySqlClient;

namespace ServerMyBar.comum
{
    public class EmpregadoDAO
    {             
        public static bool getInfoEmpregado(string e, string p)
        {
            MySqlConnection conn;
            string myConnectionString;
            myConnectionString = @"server=127.0.0.1;uid=root;" +
                                    "pwd=password;database=LI_Database";
            try
            {
                conn = new MySqlConnection(myConnectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                string query = "SELECT * FROM LI_Database.Empregado where email='" + e + "' and password='" + p + "';";
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                 
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Authentication Failed");
                    return false;
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
                return false;
            }
            return false;
        }
    }
}
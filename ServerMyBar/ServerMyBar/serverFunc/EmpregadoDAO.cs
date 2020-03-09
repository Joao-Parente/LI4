using System;
using MySql.Data.MySqlClient;

namespace ServerMyBar
{
    public class EmpregadoDAO
    {
        public static Empregado getEmpregado(int id)
        {
            return new Empregado();
        }

        public static bool addEmpregado(Empregado e)
        {
            return true;
        }

        public static bool editEmpregado(int id, Empregado e)
        {
            return true;
        }

        public static Empregado getInfoEmpregado(string e, string p)
        {
           
        MySqlConnection conn;
        string myConnectionString;

        myConnectionString = @"server=127.0.0.1;uid=root;" +
        "pwd=password;database=LI_Database";

        try{
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
                    Console.WriteLine( reader.GetInt32(0)+ "| "+ reader.GetString(1) +"| "+ reader.GetString(2) +"| "+ reader.GetString(3));
                    return new Empregado(reader.GetInt32(0),reader.GetString(1),reader.GetString(2),reader.GetString(3));


                }
            }
            else
            {
                Console.WriteLine(" !!no rows found.!!");
            }
        }

        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine("Exception "+ex.Message);
        }

        return null;
    }

    }
}
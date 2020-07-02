using MySql.Data.MySqlClient;
using System;

namespace ServerMyBar.comum
{
    public class ReclamacaoDAO
    {
        public static bool addReclamacao(int idPedido, string motivo, string reclamacao)
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
                cmd.CommandText = "INSERT INTO reclamacao values(" + idPedido + ",'" + motivo + "','" + reclamacao + "',now());";
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception" + ex.Message);
                return false;
            }
        }

    }
}
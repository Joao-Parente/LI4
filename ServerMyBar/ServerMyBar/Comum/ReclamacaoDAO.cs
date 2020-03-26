using MySql.Data.MySqlClient;
using System;

namespace ServerMyBar.comum
{
    public class ReclamacaoDAO
    {

        public static Reclamacao getReclamacao(int idPedido)
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
                //pode haver problema no reclamacao comecar por letra minuscula
                string query = "SELECT * FROM reclamacao WHERE idPedido='" + idPedido+"';";

                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Reclamacao ret=new Reclamacao((int)reader.GetInt64(0), reader.GetString(1), reader.GetString(2),reader.GetDateTime(3));
                        return ret;
                    }
                }
                else
                {
                    Console.WriteLine(" !!no rows reclamacaoDAO found.!!");
                }

                return null;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
                return null;
            }          
        }


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
                cmd.Connection = conn; //pode haver problema no reclamacao comecar por letra minuscula
                cmd.CommandText = "INSERT INTO reclamacao values("+idPedido+",'" + motivo + "','" + reclamacao + "',now());";
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
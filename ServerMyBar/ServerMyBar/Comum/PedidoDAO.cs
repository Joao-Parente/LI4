using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ServerMyBar.comum
{
    public class ProdutoDAO
    {
        public void avaliar(string idCliente, int idPedido, int aval)
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

                string query = "UPDATE LI_Database.Pedido SET avaliacao=" + aval + " WHERE idPedido=" + idPedido + " AND idCliente='" + idCliente + "';";

                cmd.CommandText = query;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
            }
            return null;
        }

        public List<Pedido> anteriores(string idCliente)
        {
            List<Pedido> lp = new List<Pedido>();
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

                string query = "SELECT LI_Database.Pedido WHERE idCliente='" + idCliente + "';";

                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //agrupar os pedidos pondo os produtos no mesmo...
                    }
                    return dic;
                }
                else
                {
                    Console.WriteLine(" !!no rows found.!!");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
            }
            return null;
            return lp;
        }
    }
}


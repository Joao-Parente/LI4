using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ServerMyBar.comum
{
    public class PedidoDAO
    {
        public static void avaliar(string idCliente, int idPedido, int aval)
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
        }

        public static Dictionary<int,Pedido> anteriores(string idCliente)
        {
            Dictionary<int,Pedido> dp = new Dictionary<int,Pedido>();
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
                        Produto pr = ProdutoDAO.getProduto((int) reader.GetInt64(1));
                        if (dp.ContainsKey((int) reader.GetInt64(0))){
                            Pedido pe = new Pedido();
                            dp.TryGetValue(int.Parse(reader.GetString(0)), out pe);
                            pe.addProduto(pr);
                            dp.Add((int) reader.GetInt64(0),pe);
                        }
                        else{
                            List<Produto> lp = new List<Produto>();
                            lp.Add(pr);
                            Pedido pe = new Pedido((int) reader.GetInt64(0),reader.GetString(3),"",(int)reader.GetInt64(4),(DateTime)reader.GetDateTime(6),lp);
                            dp.Add((int) reader.GetInt64(0),pe);
                        }
                    }
                    return dp;
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
        }
    }
}


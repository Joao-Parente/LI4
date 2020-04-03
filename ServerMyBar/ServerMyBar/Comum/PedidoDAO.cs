using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerMyBar.comum
{
    public class AnterioresAuxiliar
    {
        public int idPedido { get; set; }
        public string idCliente { get; set; }
        public string idEmpregado { get; set; }
        public DateTime datahora { get; set; }

        public AnterioresAuxiliar(int i,string ic, string ie, DateTime da)
        {
            idPedido = i;
            idCliente = ic;
            idEmpregado = ie;
            datahora = da;
        }
    }

    public class PedidoDAO
    {

        public static List<Pedido> consultaEstatisticas(DateTime inicio,DateTime fim)
        {
            List<Pedido> ret = new List<Pedido>();
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

                //string query; = "SELECT * FROM pedido WHERE data_hora between '" + inicio + "' and '" + fim +"';";
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pedido WHERE data_hora between '")
                .Append(inicio.Year).Append('-').Append(inicio.Month).Append('-').Append(inicio.Day).Append(' ').Append(inicio.Hour).Append(':').Append(inicio.Minute).Append(':').Append(inicio.Second).Append("' and '")
                .Append(fim.Year).Append('-').Append(fim.Month).Append('-').Append(fim.Day).Append(' ').Append(fim.Hour).Append(':').Append(fim.Minute).Append(':').Append(fim.Second).Append("';");

                string query = sb.ToString();
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    //E necessario ler tudo o que o reader tem colocar numa lista de AnterioresAuxiliar e depois ler um a um, e fazer a query um a um de ir ver os produtos
                    List<AnterioresAuxiliar> auxy = new List<AnterioresAuxiliar>();
                    while (reader.Read())
                    {
                        auxy.Add(new AnterioresAuxiliar(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));
                    }

                    reader.Close();

                    for (int i = 0; i < auxy.Count; i++)
                    {
                        query = "SELECT * FROM listapedidos WHERE idPedido=" + auxy[i].idPedido + ";";
                        cmd.CommandText = query;
                        cmd.Prepare();
                        reader = cmd.ExecuteReader();

                        List<ProdutoPedido> ListaProdutos = new List<ProdutoPedido>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int idProduto = reader.GetInt32(1);
                                int quantidade = reader.GetInt32(2);
                                Produto produto = ProdutoDAO.getProduto(idProduto);
                                ListaProdutos.Add(new ProdutoPedido(produto, quantidade));
                            }
                            reader.Close();
                        }
                        else
                        {
                            Console.WriteLine("Um pedido sem produtos hum..................");
                        }

                        ret.Add(new Pedido(auxy[i].idPedido, auxy[i].idCliente, auxy[i].idEmpregado, "null", auxy[i].datahora, ListaProdutos));
                    }

                    return ret;
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
            return ret;
        }

        public static List<Pedido> anteriores(string idCliente)
        {
            List<Pedido> ret = new List<Pedido>();
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

                string query = "SELECT * FROM pedido WHERE idCliente='" + idCliente + "';";

                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    //E necessario ler tudo o que o reader tem colocar numa lista de AnterioresAuxiliar e depois ler um a um, e fazer a query um a um de ir ver os produtos
                    List<AnterioresAuxiliar> auxy = new List<AnterioresAuxiliar>();
                    while (reader.Read())
                    {

                        auxy.Add(new AnterioresAuxiliar(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));

                    }

                    reader.Close();

                    for (int i = 0; i < auxy.Count; i++)
                    {
                        query = "SELECT * FROM listapedidos WHERE idPedido=" + auxy[i].idPedido + ";";
                        cmd.CommandText = query;
                        cmd.Prepare();
                        reader = cmd.ExecuteReader();

                        List<ProdutoPedido> ListaProdutos = new List<ProdutoPedido>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int idProduto = reader.GetInt32(1);
                                int quantidade = reader.GetInt32(2);
                                Produto produto = ProdutoDAO.getProduto(idProduto);
                                ListaProdutos.Add(new ProdutoPedido(produto,quantidade));
                            }
                            reader.Close();
                        }
                        else
                        {
                            Console.WriteLine("Um pedido sem produtos hum..................");
                        }

                        ret.Add(new Pedido(auxy[i].idPedido, idCliente,auxy[i].idEmpregado, "null", auxy[i].datahora, ListaProdutos));
                    }

                    return ret;
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
            return ret;
        }
    }
}


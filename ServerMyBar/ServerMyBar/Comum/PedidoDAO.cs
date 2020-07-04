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

        public AnterioresAuxiliar(int i, string ic, string ie, DateTime da)
        {
            idPedido = i;
            idCliente = ic;
            idEmpregado = ie;
            datahora = da;
        }
    }

    public class PedidoDAO
    {
        public static int registaPedido(Pedido p)
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

                StringBuilder sb = new StringBuilder();
                sb.Append(p.data_hora.Year).Append('-').Append(p.data_hora.Month).Append('-').Append(p.data_hora.Day).Append(' ').Append(p.data_hora.Hour).Append(':').Append(p.data_hora.Minute).Append(':').Append(p.data_hora.Second);
                cmd.CommandText = "INSERT INTO Pedido (idCliente,idEmpregado,data_hora) values('" + p.idCliente + "','" + p.idEmpregado + "','" + sb.ToString() + "');";
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                int id = (int) cmd.LastInsertedId;

                List<ProdutoPedido> aaa = p.produtos;
                for (int i = 0; i < aaa.Count; i++)
                {
                    ProdutoPedido pp = aaa[i];
                    cmd.CommandText = "INSERT INTO listapedidos values(" + id + ",'" + pp.p.id + "'," + pp.quantidades + ");";
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception" + ex.Message);
            }
            return 0;
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
                                ListaProdutos.Add(new ProdutoPedido(produto, quantidade));
                            }                           
                        }
                        else
                        {
                            Console.WriteLine("Um pedido sem produtos hum..................");
                        }
                        reader.Close();

                        ret.Add(new Pedido(auxy[i].idPedido, idCliente, auxy[i].idEmpregado, "null", auxy[i].datahora, ListaProdutos,null));
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

        public static int getidPedido(string idCliente, DateTime dt)
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

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pedido WHERE idCliente = '" + idCliente + "' and data_hora = '")
                .Append(dt.Year).Append('-').Append(dt.Month).Append('-').Append(dt.Day).Append(' ').Append(dt.Hour).Append(':').Append(dt.Minute).Append(':').Append(dt.Second).Append("';");
                string query = sb.ToString();

                
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                int idPedido = -1;
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        idPedido = reader.GetInt32(0);
                        break;
                    }

                    reader.Close();
                    return idPedido;
                }
                else
                {
                    return idPedido;
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }
    }
}


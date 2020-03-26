using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ServerMyBar.comum
{
    public class AnterioresAuxiliar
    {
        public int idPedido { get; set; }
        public string idEmpregado { get; set; }
        public DateTime datahora { get; set; }

        public AnterioresAuxiliar(int i, string ie, DateTime da)
        {
            idPedido = i;
            idEmpregado = ie;
            datahora = da;
        }
    }

    public class PedidoDAO
    {
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
                {/*
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
                    return dp;*/


                    //E necessario ler tudo o que o reader tem colocar numa lista de AnterioresAuxiliar e depois ler um a um, e fazer a query um a um de ir ver os produtos
                    List<AnterioresAuxiliar> auxy = new List<AnterioresAuxiliar>();
                    while (reader.Read())
                    {

                        auxy.Add(new AnterioresAuxiliar(reader.GetInt32(0), reader.GetString(2), reader.GetDateTime(3)));


                        /*
                        int idPedido = reader.GetInt32(0);
                        string idEmpregado = reader.GetString(2);
                        DateTime datahora = reader.GetDateTime(3);

                        MySqlConnection connProdutos;
                        string myConnectionStringProdutos;
                        myConnectionStringProdutos = @"server=127.0.0.1;uid=root;" +
                                             "pwd=password;database=LI_Database";

                        connProdutos = new MySqlConnection(myConnectionStringProdutos);
                        connProdutos.Open();

                        MySqlCommand cmdProdutos = new MySqlCommand();
                        cmdProdutos.Connection = conn;

                        string lista_produtos = "SELECT * FROM listapedidos WHERE idPedido='" + idPedido + "';";
                        cmdProdutos.CommandText = lista_produtos;
                        cmdProdutos.Prepare();

                        MySqlDataReader readerProdutos = cmdProdutos.ExecuteReader();
                        List<Produto> ListaProdutos = new List<Produto>();
                        if (readerProdutos.HasRows)
                        {
                            while (readerProdutos.Read())
                            {
                                int idProduto = readerProdutos.GetInt32(1);
                                int quantidade = readerProdutos.GetInt32(2);
                                ListaProdutos.Add(ProdutoDAO.getProduto(idProduto));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Um pedido sem produtos hum..................");
                        }

                        ret.Add(new Pedido(idPedido, idCliente, "null", 0, datahora, ListaProdutos));*/
                    }

                    reader.Close();

                    for (int i = 0; i < auxy.Count; i++)
                    {
                        query = "SELECT * FROM listapedidos WHERE idPedido=" + auxy[i].idPedido + ";";
                        cmd.CommandText = query;
                        cmd.Prepare();
                        reader = cmd.ExecuteReader();

                        List<Produto> ListaProdutos = new List<Produto>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int idProduto = reader.GetInt32(1);
                                int quantidade = reader.GetInt32(2);
                                ListaProdutos.Add(ProdutoDAO.getProduto(idProduto));
                            }
                            reader.Close();
                        }
                        else
                        {
                            Console.WriteLine("Um pedido sem produtos hum..................");
                        }

                        ret.Add(new Pedido(auxy[i].idPedido, idCliente, "null", 0, auxy[i].datahora, ListaProdutos));
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
            return null;
        }
    }
}


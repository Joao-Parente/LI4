using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerMyBar.comum
{
    public class ProdutoDAO
    {
        public static int registaProduto(string tipo, string nome, string detalhes, int disponibilidade, float preco)
        {
            Console.WriteLine("Nome no dao " + nome);
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
                cmd.CommandText = "INSERT INTO Produto (tipo,nome,detalhes,disponibilidade,preco) values('" + tipo + "','" + nome + "','" + detalhes + "'," + disponibilidade + "," + preco + ")";
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                long id = cmd.LastInsertedId; 
                return Convert.ToInt32(id);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception" + ex.Message);
            }
            return 0;
        }

        public static bool editProduto(int idProduto, Produto p)
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
                sb.Append("UPDATE produto SET tipo = '").Append(p.tipo)
                    .Append("', nome = '").Append(p.nome).Append("', detalhes = '").Append(p.detalhes)
                    .Append("', disponibilidade = ").Append(p.disponibilidade).Append(", preco = ")
                    .Append(p.preco.ToString().Replace(',', '.')).Append(", imagem = null WHERE(idProduto = ").Append(p.id).Append(");");
                string query = sb.ToString();
                cmd.CommandText = query;

                cmd.Prepare();

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
                return false;
            }

        }

        public static Produto getProduto(int idProduto)
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

                string query = "SELECT * FROM LI_Database.Produto WHERE idProduto = '" + idProduto + "';";
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        long size = reader.GetBytes(6, 0, null, 0, 0);
                        byte[] result = new byte[size];

                        reader.GetBytes(6, 0, result, 0, (int)size);

                        Produto prod = new Produto(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetFloat(5),result);
                        return prod;
                    }

                }
                else
                {
                    Console.WriteLine(" !!no rows found!!");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
            }
            return null;
        }

        public static bool removeProdutoFavorito(int idProduto,string idCliente)
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

                string query = "delete from produtosfavoritos where idCliente = '" + idCliente + "' and idProduto = '" + idProduto + "';";
                cmd.CommandText = query;
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                return true;

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
                return false;
            }
        }

        public static bool addProdutoFavorito(int idProduto, string idCliente)
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

                string query = "insert into produtosfavoritos values ('" + idCliente + "'," + idProduto + ");";
                cmd.CommandText = query;
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                return true;

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
                return false;
            }
        }

        public static Dictionary<string, List<Produto>> getAllProdutosFunc()
        {
            Dictionary<string, List<Produto>> dic = new Dictionary<string, List<Produto>>();
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

                string query = "SELECT * FROM LI_Database.Produto;";
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Produto p = new Produto((int)reader.GetInt64(0), (string)reader.GetString(1), (string)reader.GetString(2), (string)reader.GetString(3), reader.GetInt32(4), reader.GetFloat(5), null);
                        if (dic.ContainsKey(reader.GetString(1)))
                        {
                            List<Produto> lps = dic[reader.GetString(1)];
                            lps.Add(p);
                        }
                        else
                        {
                            List<Produto> lp = new List<Produto>();
                            lp.Add(p);
                            dic.Add(reader.GetString(1), lp);
                        }
                    }
                    return dic;
                }
                else
                {
                    Console.WriteLine(" !!no rows found!!");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("MySqlException: " + ex.Message);
            }
            return null;
        }

        public static Dictionary<string, List<Produto>> getAllProdutos()
        {
            Dictionary<string, List<Produto>> dic = new Dictionary<string, List<Produto>>();
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

                string query = "SELECT * FROM LI_Database.Produto;";
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        long size = reader.GetBytes(6, 0, null, 0, 0);
                        byte[] result = new byte[size];
                        
                        reader.GetBytes(6, 0, result, 0, (int) size);

                        Produto p = new Produto((int)reader.GetInt64(0), (string)reader.GetString(1), (string)reader.GetString(2), (string)reader.GetString(3), reader.GetInt32(4), reader.GetFloat(5),result);
                        if (dic.ContainsKey(reader.GetString(1)))
                        {
                            List<Produto> lps = dic[reader.GetString(1)];
                            lps.Add(p);
                        }
                        else
                        {
                            List<Produto> lp = new List<Produto>();
                            lp.Add(p);
                            dic.Add(reader.GetString(1), lp);
                        }
                    }
                    return dic;
                }
                else
                {
                    Console.WriteLine(" !!no rows found!!");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("MySqlException: " + ex.Message);
            }
            return null;
        }

        public static bool removeProduto(int id)
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

                cmd.CommandText = "DELETE FROM LI_Database.Produto where LI_Database.Produto.idProduto='" + id + "'";
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.CommandText = "DELETE FROM LI_Database.produtosfavoritos where LI_Database.produtosfavoritos.idProduto='" + id + "'";
                cmd2.Prepare();

                cmd2.ExecuteNonQuery();




            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySqlException: " + ex.Message);
                return false;
            }
            return true;
        }

        //Devolve uma lista com os ids dos produtos, para a aplicacao (que tem os produtos todos) possa gerar a lista
        public static List<int> getProdutosFavoritos(string idCliente)
        {
            MySqlConnection conn;
            string myConnectionString;
            myConnectionString = @"server=127.0.0.1;uid=root;" +
                                    "pwd=password;database=LI_Database";

            List<int> idsProdutos = new List<int>();
            try
            {
                conn = new MySqlConnection(myConnectionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                string query = "SELECT * FROM LI_Database.produtosfavoritos WHERE idCliente = '" + idCliente + "';";
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        idsProdutos.Add(reader.GetInt32(1));
                    }

                    return idsProdutos;

                }
                else
                {
                    Console.WriteLine(" !!no rows found!!");
                    return null;
                }
              
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySqlException: " + ex.Message);
                return null;
            }
            
        }




    }
}
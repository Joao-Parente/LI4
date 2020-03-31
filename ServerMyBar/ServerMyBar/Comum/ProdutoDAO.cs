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

                long id = cmd.LastInsertedId; //A Testar...
                return Convert.ToInt32(id);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception" + ex.Message);
            }
            return 0;
        }

        public static bool editProduto(int idProduto,Produto p)
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
                    .Append(p.preco.ToString().Replace(',','.')).Append(", imagem = null WHERE(idProduto = ").Append(p.id).Append(");");
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
                        Produto prod = new Produto(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetFloat(5));
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
                        Produto p = new Produto((int)reader.GetInt64(0), (string)reader.GetString(1), (string)reader.GetString(2), (string)reader.GetString(3), reader.GetInt32(4), reader.GetFloat(5));
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
    }
}
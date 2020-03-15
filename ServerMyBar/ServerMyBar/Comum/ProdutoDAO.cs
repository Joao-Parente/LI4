using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ServerMyBar.comum
{
    public class ProdutoDAO
    {

        // +getProduto(id : int) : Produto


        public static Dictionary<string, List<Produto>> VerTodos()
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
                        Produto p = new Produto((int) reader.GetInt64(0), (string) reader.GetString(2), (string) reader.GetString(3), (int) reader.GetUInt64(4), (float) reader.GetFloat(5));
                        if (dic.ContainsKey(reader.GetString(1)))
                        {
                            List<Produto> lp = new List<Produto>();
                            dic.TryGetValue(reader.GetString(1), out lp);
                            lp.Add(p);
                            dic.Add(reader.GetString(1), lp);
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
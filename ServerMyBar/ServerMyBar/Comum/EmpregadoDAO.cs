using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace ServerMyBar.comum
{
    public class EmpregadoDAO
    {
        public static bool addEmpregado(Empregado e)
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
                sb.Append("insert into empregado (email,password,nome,eGestor) values ('")
                  .Append(e.email).Append("', '").Append(e.password).Append("', '").Append(e.nome).Append("', 0);");
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

        public static bool editEmpregado(string email, Empregado e)
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
                    sb.Append("UPDATE Empregado SET email = '").Append(e.email)
                        .Append("', password = '").Append(e.password).Append("', nome = '").Append(e.nome)
                        .Append("', eGestor = ").Append(e.egestor)
                        .Append("' WHERE(email = ").Append(email).Append(");");
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

        public static bool autenticaGestor(string email, string pw)
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

                string query = "SELECT * FROM LI_Database.Empregado where email='" + email + "' and password='" + pw + "' and eGestor=1;";
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine(" !!no rows found.!!");
                    return false;
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception " + ex.Message);
                return false;
            }
        }

        public static Empregado getEmpregado(string idEmpregado)
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

                string query = "SELECT * FROM LI_Database.Empregado where email='" + idEmpregado + "';";
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //Console.WriteLine(reader.GetString(0) + "| " + reader.GetString(1) + "| " + reader.GetString(2) + "| " + reader.GetBoolean(3));
                        return new Empregado(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3));
                    }
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

        public static Empregado getInfoEmpregado(string e, string p)
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

                string query = "SELECT * FROM LI_Database.Empregado where email='" + e + "' and password='" + p + "';";
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0) + "| " + reader.GetString(1) + "| " + reader.GetString(2) + "| " + reader.GetBoolean(3));
                        return new Empregado(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3));
                    }
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

        public static bool RemoveEmpregado(string email)
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

                cmd.CommandText = "DELETE FROM LI_Database.Empregado where LI_Database.Empregado.email='" + email + "'";
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySqlException: " + ex.Message);
                return false;
            }
            return true;
        }

    }
}
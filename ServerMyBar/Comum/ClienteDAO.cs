
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;


namespace ServerMyBar.comum
{
    public class ClienteDAO
    {
        public static Cliente getInfoCliente(string e, string p)
        {
           
            MySqlConnection conn;
            string myConnectionString;

            myConnectionString = @"server=127.0.0.1;uid=root;" +
                                 "pwd=password;database=LI_Database";

            try{
                conn = new MySqlConnection(myConnectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
               // cmd.CommandText = "SELECT * from Cliente where email='" + email + "'";//" AND password='"+password+"';" ;
               //string query="SELECT * from Cliente where email ='"+email+ "' and password='"+password+"';";
               string query = "SELECT * FROM LI_Database.Cliente where email='" + e + "' and password='" + p + "';";
               cmd.CommandText = query;
                cmd.Prepare();


                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine( reader.GetString(0)+ "| "+ reader.GetString(1) );
                        return new Cliente(reader.GetString(0),reader.GetString(1),reader.GetString(2));


                    }
                }
                else
                {
                    Console.WriteLine(" !!no rows found.!!");
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception "+ex.Message);
            }

            return null;
        }
        
        
        
        
        
        
        public static Cliente registaCliente(string email, string password, string nome)
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
                cmd.CommandText = "INSERT INTO Cliente VALUES('"   +  email+"','"+password+"','"+nome+"')";
                cmd.Prepare();
           
                //cmd.Parameters.AddWithValue("@Name", "Trygve Gulbranssen");
                cmd.ExecuteNonQuery();
              
              
              
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Exception"+ex.Message);
            }
            return null;
        }
        
        
        
    }
}
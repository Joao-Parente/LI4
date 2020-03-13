using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace AppGestor
{
    public class AppGestor
    {
        

                private static Socket master;

                static void Main(string[] args)
                {
                
                    
                    // 1 VisualizarPedido
                    // 2 MudarEstadoPedido
                    // 3 AlternarEstadoSistema
                    // 4 NotificarCliente
                    // 5 AdicionarProduto
                    // 6 EditarProduto
                    // 7 ConsultasEstatisticas
                    // 8 VisualizarFeedback
                    // 9 AlterarInfoEmpresa
                    // 10 IniciarSessao
                    // 11 TerminarSessao

                    
                    master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12344);

                   try
                    {
                        master.Connect(ipe);
                        
                    //testes  //  master.Send(BitConverter.GetBytes(1));
                       // enviaPedido(new Pedido());
                        
                        int input;
                        byte[] msg= new byte[4];
                        bool flag = true;
                        
                        
                        while (flag)
                        {
            
                            Console.WriteLine("Insira: \n 1 para Fazer um pedido \n 2 para Fazer login");
                            input = Convert.ToInt32(Console.ReadLine());

                            switch (input)
                            {
                                case 1: //Novo_Pedido

                                    msg = BitConverter.GetBytes(input); // diz o comando 
                                    master.Send(msg);
                                    
                                    enviaPedido(new Pedido());
                                    break;

                                case 2: // Login
                                    
                                    Console.WriteLine("Starting authentication");
                                    
                                    Console.WriteLine("Insira email");
                                    string email = Console.ReadLine();
                                    Console.WriteLine("Insira pw");
                                    string pw = Console.ReadLine();

                                    msg= new byte[100];
                                    msg = BitConverter.GetBytes(input);
                                    master.Send(msg);
                                    
                                    msg= new byte[512]; //senao nao tinha acerteza que o server lia o email antes deste eviar a pw tambem, senoa dps o server lia o email e pw tudo junto.,
                                    email = email + "|" + pw +"|";
                                    msg = Encoding.ASCII.GetBytes(email);
                                    master.Send(msg);
                            
                                    byte [] log= new byte[30];
                                    master.Receive(log);
                                    bool login =BitConverter.ToBoolean(log, 0);
                                    
                                    if (login) Console.WriteLine("i'm in you crazy bastard");
                                    else Console.WriteLine("we will get em next time");
                                    break;
                                
                                
                                default:
                                    flag = false;
                                    break;
                            }
                            
                            msg= new byte[100];
                        }

                    }
                    catch(Exception e) {Console.WriteLine("Exception: "+e.Message);}

                    master.Close();
             

                }
                
                public static void enviaPedido (Pedido p)
                {
                    byte[] pedido = p.SavetoBytes();

                    master.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
                    master.Send(pedido);
                    
                }
                
    }
}
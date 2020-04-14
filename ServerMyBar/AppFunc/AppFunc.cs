using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace AppFunc
{
    public class AppFunc
    {
        private static Socket master;


        static void Main(string[] args)
        {

            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            try
            {
                master.Connect(ipe);

                LN ln = new LN(master);

                //testes  //  master.Send(BitConverter.GetBytes(1));
                // enviaPedido(new Pedido());

                int input;
                byte[] msg = new byte[4];
                bool flag = true;


                while (flag)
                {
                    // 1 Visualizar Pedido  
                    // 2 MudarEstadoPedido
                    // 3 AlternarEstadodoSistema
                    // 4 NotificarClientes
                    // 5 IniciarSessao
                    // 6 TerminarSessao

                    Console.WriteLine("Insira: \n 1 Start/Off clientServer \n 2 para Fazer login \n 4 para notificar");
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
                            /*
                                    msg= new byte[100];
                                    msg = BitConverter.GetBytes(input);
                                    master.Send(msg);
                                    msg= new byte[512]; //senao nao tinha acerteza que o server lia o email antes deste eviar a pw tambem, senoa dps o server lia o email e pw tudo junto.,
                                    email = email + "|" + pw +"|";
                                    msg = Encoding.ASCII.GetBytes(email);
                                    master.Send(msg);
                                    byte [] log= new byte[30];
                                    master.Receive(log);*/

                            bool login = ln.iniciarSessao(email, pw);
                            if (login) Console.WriteLine("i'm in you crazy bastard");
                            else Console.WriteLine("we will get em next time");
                            break;
                        case 4: //Notificar Clientes
                            Console.WriteLine("Insira o id do cliente");
                            string idCliente = Console.ReadLine();
                            Console.WriteLine("Insira a mensagem");
                            string mensagem = Console.ReadLine();
                            ln.notificarCliente(idCliente, mensagem);
                            Console.WriteLine("Sucesso!!!");
                            break;
                        case 6:
                            flag = ln.TerminarSessao();
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    msg = new byte[100];
                }
            }
            catch (Exception e) { Console.WriteLine("Exception: " + e.Message); }
            master.Close();
        }


        public static void enviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            master.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
            master.Send(pedido);
        }
    }
}
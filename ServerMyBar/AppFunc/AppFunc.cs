using System;
using System.Net;
using System.Net.Sockets;


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

                int input;
                byte[] msg = new byte[4];
                bool flag = true;


                // 1 LOGIN
                // 2 ALTERAR ESTADO DO SISTEMA
                // 3 VISUALIZAR PEDIDO
                // 4 MUDAR ESTADO DO PEDIDO
                // 5 NOTIFICAR CLIENTE
                // 6 LOGOUT

                while (flag)
                {
                    Console.WriteLine("INSERT: \n 1 LOGIN \n 2 ALTERAR ESTADO DO SISTEMA \n 3 VISUALIZAR PEDIDO  \n 4 MUDAR ESTADO DO PEDIDO \n 5 NOTIFICAR CLIENTE \n 6 LOGOUT");
                    input = Convert.ToInt32(Console.ReadLine());

                    switch (input)
                    {
                        case 1: // LOGIN
                            Console.WriteLine("Starting authentication...");

                            Console.WriteLine("Insert email:");
                            string email = Console.ReadLine();

                            Console.WriteLine("Insert password:");
                            string pw = Console.ReadLine();

                            bool login = ln.iniciarSessao(email, pw);

                            if (login) Console.WriteLine("Are you in...");
                            else Console.WriteLine("Try again...");

                            break;

                        case 2: // ALTERAR ESTADO DO SISTEMA
                            bool estado = ln.alternarEstadoSistema();

                            if (estado) Console.WriteLine("Can serve customers...");
                            else Console.WriteLine("Go home is closed...");

                            break;

                        case 3: // VISUALIZAR PEDIDO
                            Console.WriteLine("Enter the order id:");
                            int id = int.Parse(Console.ReadLine());

                            Pedido pe = ln.visualizarPedido(id);

                            pe.imprimePedido();

                            break;

                        case 4: // MUDAR ESTADO DO PEDIDO
                            break;

                        case 5: // NOTIFICAR CLIENTE
                            Console.WriteLine("Enter the id of client of who wants to upset: ");
                            string idCliente = Console.ReadLine();
                            Console.WriteLine("Insert the message: ");
                            string mensagem = Console.ReadLine();

                            ln.notificarCliente(idCliente, mensagem);

                            Console.WriteLine("Success!!!");
                            break;

                        case 6: // LOGOUT                
                            flag = ln.TerminarSessao();
                            break;

                        default:
                            flag = false;
                            break;
                    }
                    msg = new byte[100];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UPS...Exception: " + e.Message);
            }
            master.Close();
        }


        public static void enviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            master.Send(BitConverter.GetBytes(pedido.Length));
            master.Send(pedido);
        }
    }
}
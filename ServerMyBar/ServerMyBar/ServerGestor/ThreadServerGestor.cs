using System;
using System.Net.Sockets;

namespace ServerMyBar.ServerGestor
{
    public class ThreadServerGestor
    {


        private Gestor gestor;
        private Socket socket;
        private StarterClient start_client;

        public ThreadServerGestor(Gestor g, Socket s,  StarterClient sa)
        {
            gestor = g;
            socket = s;
            start_client = sa;
        }

        public void run()
        {


            Console.WriteLine("Server Thread a Correr!");

            byte[] data = new byte[512];
            bool flag = true;
            int msg;


            while (flag)
            {

                socket.Receive(data, 4, SocketFlags.None);

                msg = BitConverter.ToInt32(data, 0);
                Console.WriteLine("Comando pedido foi o de id : " + msg);



                switch (msg)
                {
                    case 2: // Login

                        Console.WriteLine("Starting authentication" + msg);

                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string email_pw = System.Text.Encoding.UTF8.GetString(data);
                        string[] credenciais = email_pw.Split('|');

                        Console.WriteLine("Credencias : '" + credenciais[0] + "'  --- '" + credenciais[1] + "'");

                        if (gestor.loginGestor(credenciais[0], credenciais[1]))
                        {
                            Console.WriteLine("Authentication succeed");
                            socket.Send(BitConverter.GetBytes(true));
                        }
                        else
                        {
                            Console.WriteLine("Authentication failed");
                            socket.Send(BitConverter.GetBytes(false));
                        }



                        break;

                    default:
                        flag = false;
                        break;


                }

                msg = -1;
                data = new byte[512];

            }


            Console.WriteLine("Thread: Terminei o comunicação com o cliente, a desligar.");
        }



        public Pedido RecebePedido()
        {


            int posicao = 0;
            int size = 100;
            byte[] data = new byte[size];

            int readBytes = -1;
            socket.Receive(data, 0, 4, SocketFlags.None); // 4bytes ->1 int que é o tamanho de bytes a recebr
            int numero_total = BitConverter.ToInt32(data, 0);

            while (readBytes != 0 && numero_total - 1 > posicao)
            {
                readBytes = socket.Receive(data, posicao, size - posicao, SocketFlags.None);
                posicao += readBytes;

                if (posicao >= size - 1)
                {
                    System.Array.Resize(ref data, size * 2);
                    size *= 2;
                }

            }


            return Pedido.loadFromBytes(data);
        }







    }

}
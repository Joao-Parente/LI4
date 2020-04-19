using System;
using System.Net.Sockets;
using System.Threading;
using ServerMyBar.comum;
using ServerMyBar.serverCliente;


namespace ServerMyBar.serverFunc
{
    public class ServerFunc
    {
        private StarterClient start_client;
        private Gestor gestor;


        public ServerFunc(Gestor g, StarterClient sc)
        {
            this.gestor = g;
            this.start_client = sc;
        }


        public void run()
        {
            TcpListener server = new TcpListener(12345);
            server.Start();

            Socket socket;

            while (true)
            {
                Console.WriteLine("ServerFunc waiting...\n");
                socket = server.AcceptSocket();

                Console.WriteLine("They called me, creating a thread to take care of the employee...\n");

                ThreadServerFunc obj = new ThreadServerFunc(gestor, socket, start_client);
                Thread a = new Thread(obj.run);
                a.Start();
            }
        }
    }
}
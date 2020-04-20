using ServerMyBar.comum;
using ServerMyBar.serverCliente;
using System;
using System.Net.Sockets;
using System.Threading;


namespace ServerMyBar.serverGestor
{
    public class ServerGestor
    {
        private Gestor gestor;
        private StarterClient start_client;


        public ServerGestor(Gestor g, StarterClient s)
        {
            gestor = g;
            start_client = s;
        }


        public void run()
        {
            TcpListener server = new TcpListener(12346);
            server.Start();

            Socket socket;

            while (true)
            {
                Console.WriteLine("ServerGestor waiting...\n");
                socket = server.AcceptSocket();

                Console.WriteLine("They called me, creating a thread to take care of the manager...\n");

                ThreadServerGestor obj = new ThreadServerGestor(gestor, socket, start_client);
                Thread a = new Thread(obj.run);
                a.Start();
            }
        }
    }
}
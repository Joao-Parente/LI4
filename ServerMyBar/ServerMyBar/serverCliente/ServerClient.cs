using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using ServerMyBar.comum;

namespace ServerMyBar.serverCliente
{
    public class ServerClient
    {
        private Gestor gestor;
        private List<ThreadServerClient> clientes;
        private Socket socket;
        private TcpListener server;
        public bool flag;

        public ServerClient(Gestor g)
        {
            gestor = g;
            server = null;
            clientes = new List<ThreadServerClient>();
            flag = true;
        }

        public void off()
        {
            if (clientes.Count > 0)
            {
                if (server != null)
                {
                    server.Stop();
                }
                socket.Close();
                foreach (ThreadServerClient x in clientes)
                {
                    x.close();
                }
                flag = false;
            }
            else
            {
                if (server != null)
                {
                    server.Stop();
                }               
            }        
        }

        public void run()
        {
            try
            {
                server = new TcpListener(12344);
                server.Start();

                while (flag)
                {
                    Console.WriteLine("ServerClient á espera de chamadas!\n ");
                    socket = server.AcceptSocket();

                    Console.WriteLine("Ligaram-me, a criar uma thread para tratar do cliente. \n ");

                    ThreadServerClient obj = new ThreadServerClient(gestor, socket);
                    this.clientes.Add(obj);
                    Thread a = new Thread(obj.run);
                    a.Start();
                }
            }catch(Exception e) { Console.WriteLine("Morri"); }

        }
    }
}
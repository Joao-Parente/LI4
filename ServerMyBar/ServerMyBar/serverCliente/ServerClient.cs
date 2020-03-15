using System;
using System.Collections.Generic;
using System.IO;
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


        public ServerClient(Gestor g)
        {
            gestor = g;
        }


        public void off()
        {
            socket.Close();
            foreach (ThreadServerClient x in clientes)
            {
                x.close();
            }
        }
        
        
        public void run() 
        {
            TcpListener server = new TcpListener(12344);
            server.Start();

            while(true)
            {                    
                Console.WriteLine("ServerClient á espera de chamadas!\n ");
                socket = server.AcceptSocket();
                
                Console.WriteLine("Ligaram-me, a criar uma thread para tratar do cliente. \n ");
                
                ThreadServerClient obj= new ThreadServerClient(gestor,socket);
                Thread a = new Thread(obj.run); 
                a.Start();            
            }
        }
    }
}
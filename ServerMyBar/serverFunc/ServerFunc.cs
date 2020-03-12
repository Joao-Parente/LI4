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

        public ServerFunc(Gestor g,StarterClient s)
        {
            gestor = g;
            start_client = s;
        }

         public void run() 
         {
            TcpListener server = new TcpListener(12345);
            server.Start();

            Socket socket;
            
            
            while(true)
            {    
                
                Console.WriteLine("ServerFunc á espera de chamadas!\n ");
                socket = server.AcceptSocket();
                
                Console.WriteLine("Ligaram-me, a criar uma thread para tratar do cliente. \n ");
                
                ThreadServerFunc obj= new ThreadServerFunc(gestor,socket,start_client);
                Thread a = new Thread(obj.run); 
                a.Start();
                            
            }

         }
    }
}
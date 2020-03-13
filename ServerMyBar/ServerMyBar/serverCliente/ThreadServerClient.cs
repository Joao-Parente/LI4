using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ServerMyBar.comum;


namespace ServerMyBar.serverCliente
{
    public class ThreadServerClient
    {
 
        private Gestor gestor;
        private Socket socket;

        public ThreadServerClient(Gestor g, Socket s)
        {
            gestor = g;
            socket = s;
        }


        public void close()
        {
            socket.Close();
        }
        

        public void run()
        {

            
            // 1 Ver Produtos
            // 2 Pedidos Anteriores
            // 3 Alterar Pedido
            // 4 Efetuar Pedido
            // 5 NoUlitmoPedido
            // 6 NovoProdutoFavorito
            // 7 InfoEmpresa
            // 8 AvaliarProduto
            // 9 IniciarSessao
            // 10 TerminarSessao
            // 11 RegistarNovoCliente
            // 12 Reclamacao

            Console.WriteLine("Server Thread a Correr!");

            byte[] data = new byte[512];
            bool flag = true;
            int msg;
            
            
            while (flag)
            {

                socket.Receive(data,4,SocketFlags.None);

                msg = BitConverter.ToInt32(data, 0);
                Console.WriteLine("Comando pedido foi o de id : "+msg);
    
                
                
                switch (msg)
                {
                     case 4: //Novo pedido
                         
                         Pedido x = RecebePedido();

                         x.imprimePedido();
                         break; 
                     
                     
                     case 5: // NoUlitmoPedido

                         List<int> r = gestor.NoUltimoPedido(); 
                         socket.Send(BitConverter.GetBytes(r[0]));
                         socket.Send(BitConverter.GetBytes(r[1]));
                         break;

                    
                     case 9: // Login
                         
                         Console.WriteLine("Starting authentication" + msg);

                         socket.Receive(data,0,512,SocketFlags.None); 
                         string email_pw = System.Text.Encoding.UTF8.GetString(data);
                         Console.WriteLine("heloooooooooooooooooooo " +email_pw+" heeeelooooo");
                         string[] credenciais = email_pw.Split('|');
                         
                         Console.WriteLine("Credencias : '" + credenciais[0] + "'  --- '" + credenciais[1]+ "'");

                         if (gestor.loginCliente(credenciais[0], credenciais[1])) {Console.WriteLine("Authentication succeed");socket.Send(BitConverter.GetBytes(true));}
                         else {Console.WriteLine("Authentication failed");socket.Send(BitConverter.GetBytes(false));}
                        
                        
                         
                         break;
                       
                     case 11: // Registo
                         
                         Console.WriteLine("Starting Register" + msg);

                         socket.Receive(data,0,512,SocketFlags.None); 
                         string email_pw_nome = System.Text.Encoding.UTF8.GetString(data);
                         Console.WriteLine("heloooooooooooooooooooo " +email_pw_nome+" heeeelooooo");
                         string[] cred = email_pw_nome.Split('|');
                         
                         Console.WriteLine("Credencias : '" + cred[0] + "'  --- '" + cred[1]+ "'"+ "'  --- '" + cred[2]+ "'");

                         if (gestor.registarCliente(cred[0], cred[1],cred[2])) {Console.WriteLine("Registation succeed");socket.Send(BitConverter.GetBytes(true));}
                         else {Console.WriteLine("Registation failed");socket.Send(BitConverter.GetBytes(false));}
                        
                        
                         
                         break;

                     
         
                     
                     default:
                         flag = false;
                         break;
                
  
                 }

                msg = -1;
                data=new byte[512];

            }


            Console.WriteLine("Thread: Terminei o comunicação com o cliente, a desligar.");
        }

        
        
        public Pedido RecebePedido()
        {

           
            int posicao = 0;
            int size = 100;
            byte[] data=new byte[size];
            
            int readBytes = -1;
            socket.Receive(data,0,4,SocketFlags.None); // 4bytes ->1 int que é o tamanho de bytes a recebr
            int numero_total=BitConverter.ToInt32(data, 0);
            
            while (readBytes != 0 && numero_total-1>posicao)
            {
                readBytes = socket.Receive(data,posicao,size-posicao,SocketFlags.None);
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


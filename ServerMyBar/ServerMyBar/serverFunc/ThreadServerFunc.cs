using System;
using System.Net.Sockets;
using System.Text;
using ServerMyBar.comum;
using ServerMyBar.serverCliente;


namespace ServerMyBar.serverFunc
{
    public class ThreadServerFunc
    {
        private Gestor gestor;
        private Socket socket;
        private StarterClient start_client;


        public ThreadServerFunc(Gestor g, Socket s, StarterClient sa)
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

                // 1 Visualizar Pedido
                // 2 MudarEstadoPedido
                // 3 AlternarEstadodoSistema
                // 4 NotificarClientes
                // 5 consultasEstatisticas
                // 10 IniciarSessao
                // 11 TerminarSessao

                switch (msg)
                {
                    case 1: // Start/Off CLiente Server
                        if (start_client.estado == false) start_client.onCliente();
                        else
                        {
                            Console.WriteLine("HELLLLO"); start_client.offCliente();
                        }
                        break;
                    case 2: // Login
                        Console.WriteLine("Starting authentication" + msg);
                        /*
                         socket.Receive(data,0,512,SocketFlags.None); 
                         string email_pw = System.Text.Encoding.UTF8.GetString(data);
                         string[] credenciais = email_pw.Split('|');
                         Console.WriteLine("Credencias : '" + credenciais[0] + "'  --- '" + credenciais[1]+ "'");
                         if (gestor.loginFunc(credenciais[0], credenciais[1])) {Console.WriteLine("Authentication succeed");socket.Send(BitConverter.GetBytes(true));}
                         else {Console.WriteLine("Authentication failed");socket.Send(BitConverter.GetBytes(false));}*/
                        byte[] numL = new byte[4], msgL;

                        //recebe tamanho email
                        socket.Receive(numL, 0, 4, SocketFlags.None);
                        int sizeL = BitConverter.ToInt32(numL, 0);
                        //recebe email
                        msgL = new byte[sizeL];
                        socket.Receive(msgL, sizeL, SocketFlags.None);
                        string emaiL = Encoding.UTF8.GetString(msgL);

                        //recebe tamanho password
                        socket.Receive(numL, 0, 4, SocketFlags.None);
                        sizeL = BitConverter.ToInt32(numL, 0);
                        //recebe password
                        msgL = new byte[sizeL];
                        socket.Receive(msgL, sizeL, SocketFlags.None);
                        string passL = Encoding.UTF8.GetString(msgL);

                        if (gestor.loginFunc(emaiL, passL) == true)
                        {
                            numL = BitConverter.GetBytes(1);
                            socket.Send(numL);
                        }
                        else
                        {
                            numL = BitConverter.GetBytes(0);
                            socket.Send(numL);
                        }


                        break;

                    case 3:  // 3 AlternarEstadoSistema


                        bool res10 = false;
                        byte[] resultado10 = new byte[30];


                        if (this.start_client.estado == true)
                        { this.start_client.offCliente(); }


                        else
                        {
                            res10 = true;
                            this.start_client.onCliente();

                        }

                        socket.Send(resultado10, 30, SocketFlags.None); // true ligou false desligou


                        break; 
                    case 4: // Notificar Clientes                       
                        byte[] numNC = new byte[4], msgNC;

                        //recebe tamanho idcliente
                        socket.Receive(numNC, 0, 4, SocketFlags.None);
                        int sizeS = BitConverter.ToInt32(numNC, 0);
                        //recebe idcliente
                        msgNC = new byte[sizeS];
                        socket.Receive(msgNC, sizeS, SocketFlags.None);
                        string idc = Encoding.UTF8.GetString(msgNC);

                        //recebe tamanho mensagem
                        socket.Receive(numNC, 0, 4, SocketFlags.None);
                        sizeS = BitConverter.ToInt32(numNC, 0);
                        //recebe mensagem
                        msgNC = new byte[sizeS];
                        socket.Receive(msgNC, sizeS, SocketFlags.None);
                        string mens = Encoding.UTF8.GetString(msgNC);

                        gestor.notificarCliente(idc, mens);
                        break;
                    case 10:
                        flag = false;
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

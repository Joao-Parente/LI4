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

        public ThreadServerFunc(Gestor g, Socket s, StarterClient sc)
        {
            this.gestor = g;
            this.socket = s;
            this.start_client = sc;
        }

        public void run()
        {
            Console.WriteLine("ThreadServerFunc running...");

            byte[] data = new byte[512];
            bool flag = true;
            int msg;

            while (flag)
            {
                //Receber o ID da operacao
                socket.Receive(data, 4, SocketFlags.None);
                msg = BitConverter.ToInt32(data, 0);
                Console.WriteLine("Command requested was ID : " + msg);

                // 1 LOGIN
                // 2 ALTERAR ESTADO DO SISTEMA
                // 3 VISUALIZAR PEDIDO
                // 4 MUDAR ESTADO DO PEDIDO
                // 5 NOTIFICAR CLIENTE
                // 6 LOGOUT

                switch (msg)
                {
                    case 1: //LOGIN
                        Console.WriteLine("Starting authentication...");
                        byte[] numL = new byte[4], msgL;

                        // tamanho do campo email
                        socket.Receive(numL, 0, 4, SocketFlags.None);
                        int sizeL = BitConverter.ToInt32(numL, 0);

                        //campo email
                        msgL = new byte[sizeL];
                        socket.Receive(msgL, sizeL, SocketFlags.None);
                        string emaiL = Encoding.UTF8.GetString(msgL);

                        // tamanho do campo password
                        socket.Receive(numL, 0, 4, SocketFlags.None);
                        sizeL = BitConverter.ToInt32(numL, 0);

                        // campo password 
                        msgL = new byte[sizeL];
                        socket.Receive(msgL, sizeL, SocketFlags.None);
                        string passL = Encoding.UTF8.GetString(msgL);

                        // login
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

                    case 2:  // ALTERAR ESTADO DO SISTEMA
                        bool res10 = false;
                        byte[] resultado10 = new byte[30];

                        if (this.start_client.estado == true)
                        {
                            this.start_client.offCliente();
                        }
                        else
                        {
                            res10 = true;
                            this.start_client.onCliente();

                        }

                        socket.Send(resultado10, 30, SocketFlags.None);
                        break;

                    case 3: // VISUALIZAR PEDIDO
                        socket.Receive(data, 4, SocketFlags.None);
                        msg = BitConverter.ToInt32(data, 0);

                        Pedido aux = gestor.getPedido(msg);

                        if (aux != null) enviaPedido(aux);

                        break;

                    case 4: // MUDAR ESTADO DO PEDIDO
                        byte[] numP = new byte[4];
                        socket.Receive(numP, 4, SocketFlags.None);

                        int idPedido = BitConverter.ToInt32(numP, 0);
                        bool b = gestor.proximoEstado(idPedido);

                        byte[] res = new byte[1];
                        res = BitConverter.GetBytes(b);

                        socket.Send(res, 1, SocketFlags.None);

                        break;

                    case 5: // NOTIFICAR CLIENTE
                        byte[] numNC = new byte[4], msgNC;

                        // tamanho IDCliente
                        socket.Receive(numNC, 0, 4, SocketFlags.None);
                        int sizeS = BitConverter.ToInt32(numNC, 0);

                        // IDCliente
                        msgNC = new byte[sizeS];
                        socket.Receive(msgNC, sizeS, SocketFlags.None);
                        string idc = Encoding.UTF8.GetString(msgNC);

                        // tamanho da Mensagem
                        socket.Receive(numNC, 0, 4, SocketFlags.None);
                        sizeS = BitConverter.ToInt32(numNC, 0);

                        // Mensagem
                        msgNC = new byte[sizeS];
                        socket.Receive(msgNC, sizeS, SocketFlags.None);
                        string mens = Encoding.UTF8.GetString(msgNC);

                        gestor.notificarCliente(idc, mens);
                        break;

                    case 6: // LOGOUT
                        flag = false;
                        break;

                    default:
                        flag = false;
                        break;
                }
                msg = -1;
                data = new byte[512];
            }
            Console.WriteLine("Thread: I ended the communication with the FUNC, disconnecting...");
        }


        public Pedido RecebePedido()
        {
            int posicao = 0;
            int size = 100;
            byte[] data = new byte[size];
            int readBytes = -1;
            socket.Receive(data, 0, 4, SocketFlags.None);
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


        public void enviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            socket.Send(BitConverter.GetBytes(pedido.Length));
            socket.Send(pedido);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ServerMyBar.comum;
using ServerMyBar.serverCliente;


namespace ServerMyBar.serverFunc
{
    public class ThreadServerFunc
    {
        private Gestor gestor;
        private Socket socket;
        private StarterClient start_client;

        private bool flag_pedidos;

        public ThreadServerFunc(Gestor g, Socket s, StarterClient sc)
        {
            this.gestor = g;
            this.socket = s;
            this.start_client = sc;
            flag_pedidos = false;
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

                    case 7: //PRODUTOS TODOS

                        Dictionary<string, List<Produto>> map = gestor.VerProdutosFunc();

                        //envia o numero de chaves
                        byte[] tamT = new byte[4];
                        tamT = BitConverter.GetBytes(map.Count);
                        socket.Send(tamT);

                        foreach (string name in map.Keys)
                        {
                            //envia o tamanho em bytes da chave
                            tamT = BitConverter.GetBytes(name.Length);
                            socket.Send(tamT);

                            //envia os bytes da string
                            byte[] nome = new byte[name.Length];
                            nome = Encoding.ASCII.GetBytes(name);
                            socket.Send(nome);

                            List<Produto> lp = new List<Produto>();
                            map.TryGetValue(name, out lp);

                            byte[] tamN = new byte[4];
                            tamN = BitConverter.GetBytes(lp.Count);
                            socket.Send(tamN);

                            for (int j = 0; j < lp.Count; j++)
                            {
                                EnviaProdutoManualFunc(lp[j]);
                            }
                        }

                        break;

                    case 8: //inicia a thread para os pedidos
                        if (flag_pedidos == false)
                        {
                            TcpListener server = new TcpListener(12350);
                            server.Start();
                            socket.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);
                            gestor.inicializaSocketPedidos(server.AcceptSocket());
                            flag_pedidos = true;
                        }

                        break;

                    case 9: //pedido em preparacao



                        break;

                    case 10: //pedido preparacao



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

        public void EnviaProdutoManualFunc(Produto p)
        {
            //envia o id do produto
            byte[] dataNum = new byte[4];
            
            socket.Send(BitConverter.GetBytes(p.id), 4, SocketFlags.None);

            byte[] dataString;
            //envia o num de bytes do tipo
            dataString = Encoding.UTF8.GetBytes(p.tipo);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do tipo            
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia o num de bytes do nome
            dataString = Encoding.UTF8.GetBytes(p.nome);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do nome
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia o num de bytes do detalhes
            dataString = Encoding.UTF8.GetBytes(p.detalhes);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do detalhes
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia a disponibilidade
            dataNum = BitConverter.GetBytes(p.disponibilidade);
            socket.Send(dataNum, 4, SocketFlags.None);

            //envia o preco
            byte[] dataFloat = BitConverter.GetBytes(p.preco);
            dataNum = BitConverter.GetBytes(dataFloat.Length);
            socket.Send(dataNum, 4, SocketFlags.None);//envia num bytes
            socket.Send(dataFloat, dataFloat.Length, SocketFlags.None);//envia os bytes

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

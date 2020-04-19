using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ServerMyBar.comum;
using ServerMyBar.serverCliente;


namespace ServerMyBar.serverGestor
{
    public class ThreadServerGestor
    {
        private Gestor gestor;
        private Socket socket;
        private StarterClient start_client;


        public ThreadServerGestor(Gestor g, Socket s, StarterClient sc)
        {
            this.gestor = g;
            this.socket = s;
            this.start_client = sc;
        }


        public void run()
        {
            Console.WriteLine("ThreadServerGestor running...");

            byte[] data = new byte[512];
            bool flag = true;
            int msg;

            while (flag)
            {
                socket.Receive(data, 4, SocketFlags.None);
                msg = BitConverter.ToInt32(data, 0);
                Console.WriteLine("Command requested was ID : " + msg);

                // 1 LOGIN
                // 2 ALTERAR ESTADO DO SISTEMA
                // 3 VISUALIZAR PEDIDO
                // 4 MUDAR ESTADO DO PEDIDO
                // 5 NOTIFICAR CLIENTE
                // 6 ADICIONAR PRODUTO
                // 7 EDITAR    PRODUTO
                // 8 REMOVER   PRODUTO
                // 9  ADICIONAR EMPREGADO
                // 10 EDITAR    EMPREGADO
                // 11 REMOVER   EMPREGADO
                // 12 ALTERAR INFO EMPRESA
                // 13 CONSULTAS ESTATISTICAS
                // 14 LOGOUT

                switch (msg)
                {
                    case 1: // LOGIN
                        Console.WriteLine("Starting authentication" + msg);
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

                        if (gestor.loginGestor(emaiL, passL) == true)
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

                    case 6: // ADICIONAR PRODUTO
                        Produto p = RecebeProduto();

                        int idP = gestor.addProduto(p);

                        byte[] id = new byte[4];
                        id = BitConverter.GetBytes(idP);
                        socket.Send(id);

                        break;

                    case 7: // EDITAR PRODUTO
                        byte[] numEP = new byte[4];

                        Produto p2 = RecebeProdutoManual();

                        if (gestor.editarProduto(p2.id, p2) == true)
                        {
                            numEP = BitConverter.GetBytes(1);
                            socket.Send(numEP);
                        }
                        else
                        {
                            numEP = BitConverter.GetBytes(0);
                            socket.Send(numEP);
                        }

                        break;

                    case 8: //REMOVER PRODUTO
                        socket.Receive(data, 4, SocketFlags.None);
                        msg = BitConverter.ToInt32(data, 0);

                        bool res3 = gestor.removeProduct(msg);

                        byte[] resultado3 = new byte[30];
                        resultado3 = BitConverter.GetBytes(res3);
                        socket.Send(resultado3, 30, SocketFlags.None);

                        break;

                    case 9: // ADICIONAR EMPREGADO
                        Empregado em = RecebeEmpregado();

                        if (gestor.addEmpregado(em) == true)
                        {
                            numEP = BitConverter.GetBytes(1);
                            socket.Send(numEP);
                        }
                        else
                        {
                            numEP = BitConverter.GetBytes(0);
                            socket.Send(numEP);
                        }

                        break;

                    case 10: // EDITAR EMPREGADO
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string email1 = System.Text.Encoding.UTF8.GetString(data);

                        Empregado e = RecebeEmpregado();

                        bool res1 = gestor.editEmpregado(email1, e);

                        byte[] resultado1 = new byte[30];
                        resultado1 = BitConverter.GetBytes(res1);
                        socket.Send(resultado1, 30, SocketFlags.None);

                        break;

                    case 11: // REMOVER EMPREGADO
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string email2 = System.Text.Encoding.UTF8.GetString(data);

                        bool res2 = gestor.removeEmpregado(email2);

                        byte[] resultado2 = new byte[30];
                        resultado2 = BitConverter.GetBytes(res2);
                        socket.Send(resultado2, 30, SocketFlags.None);

                        break;

                    case 12: // ALTERAR INFO EMPRESA
                        break;

                    case 13: // CONSULTAS ESTATISTICAS
                        byte[] num7 = new byte[4], msg7;

                        msg7 = new byte[8];
                        socket.Receive(msg7, 8, SocketFlags.None);
                        DateTime datainicio = new DateTime(BitConverter.ToInt64(msg7, 0));

                        socket.Receive(msg7, 8, SocketFlags.None);
                        DateTime datafinal = new DateTime(BitConverter.ToInt64(msg7, 0));

                        List<Pedido> pedidos = gestor.consultasEstatisticas(datainicio, datafinal);

                        //id = new byte[4];
                        id = BitConverter.GetBytes(pedidos.Count);
                        socket.Send(id);

                        //envia os pedidos todos
                        for (int i = 0; i < pedidos.Count; i++)
                        {
                            //envia a datahora
                            id = BitConverter.GetBytes(pedidos[i].data_hora.ToBinary());
                            socket.Send(id);

                            //envia o id
                            id = BitConverter.GetBytes(pedidos[i].id);
                            socket.Send(id);

                            //envia idEmpregado
                            id = BitConverter.GetBytes(pedidos[i].idEmpregado.Length);
                            socket.Send(id);
                            id = Encoding.UTF8.GetBytes(pedidos[i].idEmpregado);
                            socket.Send(id, pedidos[i].idEmpregado.Length, SocketFlags.None);

                            //envia idCliente
                            id = BitConverter.GetBytes(pedidos[i].idCliente.Length);
                            socket.Send(id);
                            id = Encoding.UTF8.GetBytes(pedidos[i].idCliente);
                            socket.Send(id, pedidos[i].idCliente.Length, SocketFlags.None);

                            //envia num pedidos
                            id = BitConverter.GetBytes(pedidos[i].produtos.Count);
                            socket.Send(id);

                            for (int j = 0; j < pedidos[i].produtos.Count; j++)
                            {
                                byte[] hello = pedidos[i].produtos[j].p.SavetoBytes();
                                socket.Send(BitConverter.GetBytes(hello.Length));
                                socket.Send(hello);

                                socket.Send(BitConverter.GetBytes(pedidos[i].produtos[j].quantidades));
                            }

                        }
                        Console.WriteLine("Enviei os pedidos todos");
                        break;

                    case 14: // LOGOUT
                        flag = false;
                        break;

                    default:
                        flag = false;
                        break;
                }
                msg = -1;
                data = new byte[512];

            }
            Console.WriteLine("Thread: I ended the communication with the GESTOR, disconnecting...");
        }

        public void EnviaProdutoManual(Produto p)
        {
            //envia o id do produto
            byte[] dataNum = new byte[4];
            dataNum = BitConverter.GetBytes(p.id);
            socket.Send(dataNum, 4, SocketFlags.None);

            byte[] dataString;
            //envia o num de bytes do tipo
            dataNum = BitConverter.GetBytes(p.tipo.Length);
            socket.Send(dataNum, 4, SocketFlags.None);
            //envia os bytes do tipo
            dataString = Encoding.UTF8.GetBytes(p.tipo);
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia o num de bytes do nome
            dataNum = BitConverter.GetBytes(p.nome.Length);
            socket.Send(dataNum, 4, SocketFlags.None);
            //envia os bytes do nome
            dataString = Encoding.UTF8.GetBytes(p.nome);
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia o num de bytes do detalhes
            dataNum = BitConverter.GetBytes(p.detalhes.Length);
            socket.Send(dataNum, 4, SocketFlags.None);
            //envia os bytes do detalhes
            dataString = Encoding.UTF8.GetBytes(p.detalhes);
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia a disponibilidade
            dataNum = BitConverter.GetBytes(p.disponibilidade);
            socket.Send(dataNum, 4, SocketFlags.None);

            //envia o preco
            byte[] dataFloat = BitConverter.GetBytes(p.preco);
            dataNum = BitConverter.GetBytes(dataFloat.Length);
            socket.Send(dataNum, 4, SocketFlags.None);//envia num bytes
            socket.Send(dataFloat, dataFloat.Length, SocketFlags.None);//envia os bytes

            //envia imagem --- por completar

        }

        public Produto RecebeProdutoManual()
        {
            int sizeS;

            //recebe o id do produto
            byte[] dataNum = new byte[4];
            socket.Receive(dataNum, 4, SocketFlags.None);
            int id = BitConverter.ToInt32(dataNum, 0);

            byte[] dataString;
            //recebe o tipo
            socket.Receive(dataNum, 4, SocketFlags.None);
            sizeS = BitConverter.ToInt32(dataNum, 0);
            dataString = new byte[sizeS];
            socket.Receive(dataString, sizeS, SocketFlags.None);
            string tipo = Encoding.UTF8.GetString(dataString);

            //recebe o nome
            socket.Receive(dataNum, 4, SocketFlags.None);
            sizeS = BitConverter.ToInt32(dataNum, 0);
            dataString = new byte[sizeS];
            socket.Receive(dataString, sizeS, SocketFlags.None);
            string nome = Encoding.UTF8.GetString(dataString);

            //recebe os detalhes
            socket.Receive(dataNum, 4, SocketFlags.None);
            sizeS = BitConverter.ToInt32(dataNum, 0);
            dataString = new byte[sizeS];
            socket.Receive(dataString, sizeS, SocketFlags.None);
            string detalhes = Encoding.UTF8.GetString(dataString);

            //recebe a disponibilidade
            socket.Receive(dataNum, 4, SocketFlags.None);
            int disponibilidade = BitConverter.ToInt32(dataNum, 0);

            //envia o preco
            socket.Receive(dataNum, 4, SocketFlags.None);
            sizeS = BitConverter.ToInt32(dataNum, 0);
            dataString = new byte[sizeS];
            socket.Receive(dataString, sizeS, SocketFlags.None);
            float preco = BitConverter.ToSingle(dataString, 0);

            //envia imagem --- por completar

            return new Produto(id, tipo, nome, detalhes, disponibilidade, preco);

        }

        public Empregado RecebeEmpregado()
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
            return Empregado.loadFromBytes(data);
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


        public void enviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            socket.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
            socket.Send(pedido);
        }


        public Produto RecebeProduto()
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
            return Produto.loadFromBytes(data);
        }
    }
}
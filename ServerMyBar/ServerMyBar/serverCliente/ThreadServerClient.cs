using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
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
                socket.Receive(data, 4, SocketFlags.None);
                msg = BitConverter.ToInt32(data, 0);
                Console.WriteLine("Comando pedido foi o de id : " + msg);

                switch (msg)
                {
                    case 1: //VerProdutos
                        Dictionary<string, List<Produto>> map = new Dictionary<string, List<Produto>>();
                        map = gestor.VerProdutos();

                        byte[] tamT = new byte[4];
                        tamT = BitConverter.GetBytes(map.Count);
                        socket.Send(tamT);

                        foreach (string name in map.Keys)
                        {
                            byte[] nome = new byte[512];
                            nome = Encoding.ASCII.GetBytes(name);
                            socket.Send(nome);

                            List<Produto> lp = new List<Produto>();
                            map.TryGetValue(name, out lp);

                            byte[] tamN = new byte[4];
                            tamN = BitConverter.GetBytes(lp.Count);
                            socket.Send(tamN);

                            for (int j = 0; j < lp.Count; j++)
                            {
                                byte[] produto = lp[j].SavetoBytes();
                                socket.Send(BitConverter.GetBytes(produto.Length));
                                socket.Send(produto);
                            }
                        }

                        break;
                    case 2: //Pedidos Anteriores

                        //recebe id do cliente
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string idCliente = System.Text.Encoding.UTF8.GetString(data);

                        //vai a base de dados tirar os pedidos todos
                        List<Pedido> pedidos = gestor.pedidosAnteriores(idCliente.Replace("\0", string.Empty));

                        //envia o numero de pedidos da lista
                        byte[] id = new byte[4];
                        id = BitConverter.GetBytes(pedidos.Count);
                        socket.Send(id);

                        //envia os pedidos todos
                        for (int i = 0; i < pedidos.Count; i++)
                        {
                            byte[] pedido = pedidos[i].SavetoBytes();
                            socket.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
                            socket.Send(pedido);
                        }

                        Console.WriteLine("Enviei os pedidos todos");

                        break;
                    case 3: //Alterar Pedido
                        socket.Receive(data, 0, 4, SocketFlags.None);
                        int idPedido = BitConverter.ToInt32(data, 0);

                        socket.Receive(data, 0, 4, SocketFlags.None);
                        int a = BitConverter.ToInt32(data, 0);

                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string produtos = System.Text.Encoding.UTF8.GetString(data);

                        String[] listProdutos = produtos.Split(" ", StringSplitOptions.None);
                        if (a == 0)
                        {
                            Console.WriteLine("A adicionar produtos ao pedido");
                            gestor.adicionarProdutos(idPedido, listProdutos);
                        }
                        else if (a == 1)
                        {
                            Console.WriteLine("A remover produtos ao pedido");
                            gestor.removerProdutos(idPedido, listProdutos);
                        }
                        break;
                    case 4: //Novo pedido
                        Pedido x = RecebePedido();
                        x.imprimePedido();
                        break;
                    case 5: // NoUlitmoPedido
                        List<int> r = gestor.NoUltimoPedido();
                        socket.Send(BitConverter.GetBytes(r[0]));
                        socket.Send(BitConverter.GetBytes(r[1]));
                        break;
                    case 6: //Adicionar Produto aos favoritos

                        //recebe o id do produto
                        socket.Receive(data, 0, 4, SocketFlags.None);
                        int idProduto = BitConverter.ToInt32(data, 0);

                        //recebe o email do cliente
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string idCliente2 = System.Text.Encoding.UTF8.GetString(data);

                        byte[] idaddProdFav = new byte[4];
                        if (gestor.addProdutoFavoritos(idProduto, idCliente2.Replace("\0", string.Empty)) == true)
                        {
                            idaddProdFav = BitConverter.GetBytes(1);
                            socket.Send(idaddProdFav);
                        }
                        else
                        {
                            idaddProdFav = BitConverter.GetBytes(0);
                            socket.Send(idaddProdFav);
                        }

                        break;
                    case 9: // Login
                        Console.WriteLine("Starting authentication" + msg);
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string email_pw = System.Text.Encoding.UTF8.GetString(data);
                        Console.WriteLine("heloooooooooooooooooooo " + email_pw + " heeeelooooo");
                        string[] credenciais = email_pw.Split('|');
                        Console.WriteLine("Credencias : '" + credenciais[0] + "'  --- '" + credenciais[1] + "'");
                        if (gestor.loginCliente(credenciais[0], credenciais[1])) { Console.WriteLine("Authentication succeed"); socket.Send(BitConverter.GetBytes(true)); }
                        else { Console.WriteLine("Authentication failed"); socket.Send(BitConverter.GetBytes(false)); }
                        break;
                    case 10:
                        flag = false;
                        break;
                    case 11: // Registo
                        Console.WriteLine("Starting Register" + msg);
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string email_pw_nome = System.Text.Encoding.UTF8.GetString(data);
                        Console.WriteLine("heloooooooooooooooooooo " + email_pw_nome + " heeeelooooo");
                        string[] cred = email_pw_nome.Split('|');
                        Console.WriteLine("Credencias : '" + cred[0] + "'  --- '" + cred[1] + "'" + "'  --- '" + cred[2] + "'");
                        if (gestor.registarCliente(cred[0], cred[1], cred[2])) { Console.WriteLine("Registation succeed"); socket.Send(BitConverter.GetBytes(true)); }
                        else { Console.WriteLine("Registation failed"); socket.Send(BitConverter.GetBytes(false)); }
                        break;
                    case 12: //adicionar Reclamacao

                        //recebe id produto
                        socket.Receive(data, 0, 4, SocketFlags.None);
                        int idPedido2 = BitConverter.ToInt32(data, 0);

                        //recebe tamanho motivo
                        socket.Receive(data, 0, 4, SocketFlags.None);
                        int tamanhoString = BitConverter.ToInt32(data, 0);
                        //Recebe motivo
                        byte[] dados = new byte[tamanhoString];
                        socket.Receive(dados, tamanhoString, SocketFlags.None);
                        string motivo = System.Text.Encoding.UTF8.GetString(dados);

                        //recebe tamanho reclamacao
                        socket.Receive(data, 0, 4, SocketFlags.None);
                        tamanhoString = BitConverter.ToInt32(data, 0);
                        //recebe reclamacao
                        dados = new byte[tamanhoString];
                        socket.Receive(dados, tamanhoString, SocketFlags.None);
                        string reclamacao = System.Text.Encoding.UTF8.GetString(dados);

                        //envia para o cliente se a reclamacao foi adicionada com sucesso ou nao
                        byte[] idreclamacao = new byte[4];
                        if (gestor.AddReclamacao(idPedido2, motivo, reclamacao) == true)
                        {
                            idreclamacao = BitConverter.GetBytes(1);
                            socket.Send(idreclamacao);
                        }
                        else
                        {
                            idreclamacao = BitConverter.GetBytes(0);
                            socket.Send(idreclamacao);
                        }
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


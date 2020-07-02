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
            socket.Blocking = true;
        }
        /**
        * Função utilizada para fechar a conexao de um cliente com o servidor
        */
        public void close()
        {
            socket.Close();
        }

        /**
        * Função em loop que recebe identificadores de comandos do cliente e responde a eles
        * \n 1 : VerProdutos
        * \n 2 : Pedidos Anteriores
        * \n 4 : Novo pedido
        * \n 5 : NoUlitmoPedido
        * \n 6 : Adicionar Produto aos favoritos
        * \n 9 : Login
        * \n 10 : Terminar Sessão
        * \n 11 : Registo
        * \n 12 : Adicionar Reclamacao
        * \n 13 : Produtos favoritos
        * \n 14 : Nome do Cliente
        * \n 15 : Remove um Produto dos Favoritos
        * \n 16 : Ver um estado de pedido
        * \n 17 : Retornar um id de um pedido que foi feito a uma dado dia e hora
        */
        public void run()
        {
            try
            {
                Console.WriteLine("Server Thread a Correr!");
                byte[] data = new byte[512];
                bool flag = true;
                int msg;

                while (flag)
                {
                    socket.Receive(data, 4, SocketFlags.None);
                    msg = BitConverter.ToInt32(data);
                    Console.WriteLine("Comando pedido foi o de id : " + msg);

                    switch (msg)
                    {
                        case 1: //VerProdutos
                            Dictionary<string, List<Produto>> map = gestor.VerProdutos();

                            //envia o numero de chaves
                            byte[] tamT = new byte[4];
                            tamT = BitConverter.GetBytes(map.Count);
                            socket.Send(tamT);

                            foreach (string name in map.Keys)
                            {
                                //envia o tamanho em bytes da chave
                                socket.Send(BitConverter.GetBytes(name.Length));
                                //envia os bytes da string
                                socket.Send(Encoding.UTF8.GetBytes(name));

                                List<Produto> lp = new List<Produto>();
                                map.TryGetValue(name, out lp);

                                byte[] tamN = new byte[4];
                                tamN = BitConverter.GetBytes(lp.Count);
                                socket.Send(tamN);

                                for (int j = 0; j < lp.Count; j++)
                                {
                                    socket.Receive(data, 4, SocketFlags.None);
                                    EnviaProdutoManual(lp[j]);
                                }
                            }

                            break;
                        case 2: //Pedidos Anteriores

                            //vai a base de dados tirar os pedidos todos
                            byte[] dadospa; int size_dadospa;

                            //recebe email
                            dadospa = new byte[4];
                            socket.Receive(dadospa, 4, SocketFlags.None);
                            size_dadospa = BitConverter.ToInt32(dadospa, 0);
                            dadospa = new byte[size_dadospa];
                            socket.Receive(dadospa, size_dadospa, SocketFlags.None);
                            List<Pedido> pedidos = gestor.pedidosAnteriores(Encoding.UTF8.GetString(dadospa));

                            //envia o numero de pedidos da lista
                            byte[] id;
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

                                //envia num pedidos
                                id = BitConverter.GetBytes(pedidos[i].produtos.Count);
                                socket.Send(id);

                                for (int j = 0; j < pedidos[i].produtos.Count; j++)
                                {
                                    socket.Send(BitConverter.GetBytes(pedidos[i].produtos[j].p.id), 4, SocketFlags.None);
                                    socket.Send(BitConverter.GetBytes(pedidos[i].produtos[j].quantidades));
                                }
                            }
                            Console.WriteLine("Ja foram os pedidos todos");
                            break;
                        case 4: //Novo pedido
                            Pedido x = RecebePedidoManual();
                            int idp = gestor.addPedido(x);
                            socket.Send(BitConverter.GetBytes(idp));
                            break;
                        case 5: // NoUlitmoPedido
                            List<int> r = gestor.NoUltimoPedido();
                            socket.Send(BitConverter.GetBytes(r[0]), 4, SocketFlags.None);
                            socket.Send(BitConverter.GetBytes(r[1]), 4, SocketFlags.None);
                            break;
                        case 6: //Adicionar Produto aos favoritos

                            //recebe o id do produto
                            socket.Receive(data, 0, 4, SocketFlags.None);
                            int idProduto = BitConverter.ToInt32(data, 0);

                            //recebe o email do cliente
                            byte[] dadospf; int size_dadospf;

                            //recebe email
                            dadospf = new byte[4];
                            socket.Receive(dadospf, 4, SocketFlags.None);
                            size_dadospf = BitConverter.ToInt32(dadospf, 0);
                            dadospf = new byte[size_dadospf];
                            socket.Receive(dadospf, size_dadospf, SocketFlags.None);

                            byte[] idaddProdFav = new byte[4];
                            if (gestor.addProdutoFavorito(idProduto, Encoding.UTF8.GetString(dadospf)) == true)
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

                            byte[] dadosLogin; int size_dadosLogin;

                            //recebe email
                            dadosLogin = new byte[4];
                            socket.Receive(dadosLogin, 4, SocketFlags.None);
                            size_dadosLogin = BitConverter.ToInt32(dadosLogin, 0);
                            dadosLogin = new byte[size_dadosLogin];
                            socket.Receive(dadosLogin, size_dadosLogin, SocketFlags.None);
                            string emailLogin = Encoding.UTF8.GetString(dadosLogin);

                            //recebe password
                            dadosLogin = new byte[4];
                            socket.Receive(dadosLogin, 4, SocketFlags.None);
                            size_dadosLogin = BitConverter.ToInt32(dadosLogin, 0);
                            dadosLogin = new byte[size_dadosLogin];
                            socket.Receive(dadosLogin, size_dadosLogin, SocketFlags.None);
                            string passwordLogin = Encoding.UTF8.GetString(dadosLogin);

                            dadosLogin = new byte[1];
                            if (gestor.loginCliente(emailLogin, passwordLogin))
                            {
                                dadosLogin[0] = 1;
                                socket.Send(dadosLogin, 1, SocketFlags.None);
                            }
                            else
                            {
                                dadosLogin[0] = 0;
                                socket.Send(dadosLogin, 1, SocketFlags.None);
                            }

                            break;
                        case 10: //Terminar Sessao
                            flag = false;
                            break;
                        case 11: // Registo

                            byte[] dadosRegisto; int size_dadosRegisto;

                            //recebe email
                            dadosRegisto = new byte[4];
                            socket.Receive(dadosRegisto, 4, SocketFlags.None);
                            size_dadosRegisto = BitConverter.ToInt32(dadosRegisto, 0);
                            dadosRegisto = new byte[size_dadosRegisto];
                            socket.Receive(dadosRegisto, size_dadosRegisto, SocketFlags.None);
                            string emailRegisto = Encoding.UTF8.GetString(dadosRegisto);

                            //recebe password
                            dadosRegisto = new byte[4];
                            socket.Receive(dadosRegisto, 4, SocketFlags.None);
                            size_dadosRegisto = BitConverter.ToInt32(dadosRegisto, 0);
                            dadosRegisto = new byte[size_dadosRegisto];
                            socket.Receive(dadosRegisto, size_dadosRegisto, SocketFlags.None);
                            string passwordRegisto = Encoding.UTF8.GetString(dadosRegisto);

                            //recebe nome
                            dadosRegisto = new byte[4];
                            socket.Receive(dadosRegisto, 4, SocketFlags.None);
                            size_dadosRegisto = BitConverter.ToInt32(dadosRegisto, 0);
                            dadosRegisto = new byte[size_dadosRegisto];
                            socket.Receive(dadosRegisto, size_dadosRegisto, SocketFlags.None);
                            string nomeRegisto = Encoding.UTF8.GetString(dadosRegisto);

                            dadosRegisto = new byte[1];
                            if (gestor.registarCliente(emailRegisto, passwordRegisto, nomeRegisto))
                            {
                                dadosRegisto[0] = 1;
                                socket.Send(dadosRegisto,1,SocketFlags.None);
                            }
                            else
                            {
                                dadosRegisto[0] = 0;
                                socket.Send(dadosRegisto, 1, SocketFlags.None);
                            }
                        break;
                        case 12: //adicionar Reclamacao

                            //recebe id pedido
                            socket.Receive(data, 0, 4, SocketFlags.None);
                            int idPedido2 = BitConverter.ToInt32(data, 0);

                            //recebe tamanho reclamacao
                            socket.Receive(data, 0, 4, SocketFlags.None);
                            int tamanhoString = BitConverter.ToInt32(data, 0);
                            //recebe reclamacao
                            byte[] dados = new byte[tamanhoString];
                            socket.Receive(dados, tamanhoString, SocketFlags.None);
                            string reclamacao = System.Text.Encoding.UTF8.GetString(dados);

                            //envia para o cliente se a reclamacao foi adicionada com sucesso ou nao
                            byte[] idreclamacao = new byte[4];
                            if (gestor.AddReclamacao(idPedido2, "null", reclamacao) == true)
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
                        case 13: //produtos favoritos
                            //recebe tamanho idCLiente
                            socket.Receive(data, 0, 4, SocketFlags.None);
                            int tamanhoStringPF = BitConverter.ToInt32(data, 0);
                            //Recebe idCliente
                            byte[] dadosPF = new byte[tamanhoStringPF];
                            socket.Receive(dadosPF, tamanhoStringPF, SocketFlags.None);
                            string idClientePF = System.Text.Encoding.UTF8.GetString(dadosPF);

                            List<int> prods = gestor.produtosFavoritos(idClientePF);
                            if (prods == null)
                            {
                                socket.Send(BitConverter.GetBytes(0));
                            }
                            else
                            {
                                //envia o tamanho da lista
                                socket.Send(BitConverter.GetBytes(prods.Count));
                                for (int iPF = 0; iPF < prods.Count; iPF++)
                                {
                                    //envia os ids
                                    socket.Send(BitConverter.GetBytes(prods[iPF]));
                                }
                            }
                            break;
                        case 14: //nome do cliente

                            //recebe email
                            socket.Receive(data, 4, SocketFlags.None);
                            int tamanho = BitConverter.ToInt32(data, 0);
                            byte[] emailb = new byte[tamanho];
                            socket.Receive(emailb, tamanho, SocketFlags.None);

                            //envia nome
                            emailb = Encoding.UTF8.GetBytes(gestor.getNomeUtilizador(Encoding.UTF8.GetString(emailb)));
                            socket.Send(BitConverter.GetBytes(emailb.Length), 4, SocketFlags.None);
                            socket.Send(emailb, emailb.Length, SocketFlags.None);

                            break;
                        case 15: //removerProdutoFavorito

                            //recebe id produto
                            socket.Receive(data, 4, SocketFlags.None);
                            int idprodutorpf = BitConverter.ToInt32(data, 0);

                            //recebe email
                            socket.Receive(data, 4, SocketFlags.None);
                            int tamanhorpf = BitConverter.ToInt32(data, 0);
                            byte[] emailrpf = new byte[tamanhorpf];
                            socket.Receive(emailrpf, tamanhorpf, SocketFlags.None);
                            string idClienterpf = Encoding.UTF8.GetString(emailrpf);

                            byte[] foiremovido = new byte[4];
                            if (gestor.removerFavoritos(idprodutorpf, idClienterpf) == true)
                            {
                                foiremovido = BitConverter.GetBytes(1);
                                socket.Send(foiremovido);
                            }
                            else
                            {
                                foiremovido = BitConverter.GetBytes(0);
                                socket.Send(foiremovido);
                            }
                            break;
                        case 16: //estado pedido
                            //recebe email
                            socket.Receive(data, 4, SocketFlags.None);
                            int tamanhoep = BitConverter.ToInt32(data, 0);
                            byte[] emailep = new byte[tamanhoep];
                            socket.Receive(emailep, tamanhoep, SocketFlags.None);
                            string idClienteep = Encoding.UTF8.GetString(emailep);

                            //recebe data
                            byte[] longgy = new byte[8];
                            socket.Receive(longgy, 8, SocketFlags.None);
                            DateTime datapedido = DateTime.FromBinary(BitConverter.ToInt64(longgy, 0));

                            int esta = gestor.estadoPedido(idClienteep, datapedido);
                            socket.Send(BitConverter.GetBytes(esta));
                            break;
                        case 17: //get id pedido

                            //recebe email
                            socket.Receive(data, 4, SocketFlags.None);
                            int tamanhoip = BitConverter.ToInt32(data, 0);
                            byte[] emailip = new byte[tamanhoip];
                            socket.Receive(emailip, tamanhoip, SocketFlags.None);
                            string idClienteip = Encoding.UTF8.GetString(emailip);

                            //recebe data
                            byte[] longgyip = new byte[8];
                            socket.Receive(longgyip, 8, SocketFlags.None);
                            DateTime datapedidoip = DateTime.FromBinary(BitConverter.ToInt64(longgyip, 0));

                            socket.Send(BitConverter.GetBytes(gestor.getIdPedido(idClienteip, datapedidoip)),4,SocketFlags.None);

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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /**
        * Envia para o cliente  um produto
        */
        public void EnviaProdutoManual(Produto p)
        {
            //envia o id do produto
            byte[] dataNum = new byte[4];

            socket.Receive(dataNum, 4, SocketFlags.None);

            socket.Send(BitConverter.GetBytes(p.id), 4, SocketFlags.None);

            socket.Receive(dataNum, 4, SocketFlags.None);

            byte[] dataString;
            //envia o num de bytes do tipo
            dataString = Encoding.UTF8.GetBytes(p.tipo);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do tipo            
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            socket.Receive(dataNum, 4, SocketFlags.None);

            //envia o num de bytes do nome
            dataString = Encoding.UTF8.GetBytes(p.nome);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do nome
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            socket.Receive(dataNum, 4, SocketFlags.None);

            //envia o num de bytes do detalhes
            dataString = Encoding.UTF8.GetBytes(p.detalhes);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do detalhes
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            socket.Receive(dataNum, 4, SocketFlags.None);

            //envia a disponibilidade
            dataNum = BitConverter.GetBytes(p.disponibilidade);
            socket.Send(dataNum, 4, SocketFlags.None);

            socket.Receive(dataNum, 4, SocketFlags.None);

            //envia o preco
            byte[] dataFloat = BitConverter.GetBytes(p.preco);
            dataNum = BitConverter.GetBytes(dataFloat.Length);
            socket.Send(dataNum, 4, SocketFlags.None);//envia num bytes
            socket.Send(dataFloat, dataFloat.Length, SocketFlags.None);//envia os bytes

            socket.Receive(dataNum, 4, SocketFlags.None);

            //envia imagem 
            int tamanho = p.imagem.Length;
            dataNum = BitConverter.GetBytes(tamanho);
            socket.Send(dataNum, 4, SocketFlags.None);

            int enviados = 0;
            while(enviados < p.imagem.Length)
            {
                enviados += socket.Send(p.imagem, enviados, p.imagem.Length - enviados, SocketFlags.None);
            }
            
            socket.Receive(dataNum, 4, SocketFlags.None);

        }

        /**
        * Recebe do cliente um pedido
        */
        public Pedido RecebePedidoManual()
        {
            byte[] num = new byte[4],str,lon=new byte[8];
            int tamanho;

            //recebe idCliente_notfs
            socket.Receive(num, 4, SocketFlags.None);
            tamanho = BitConverter.ToInt32(num, 0);
            str = new byte[tamanho];
            socket.Receive(str, tamanho, SocketFlags.None);
            string idPlayer = Encoding.UTF8.GetString(str);

            //recebe o id cliente
            socket.Receive(num, 4, SocketFlags.None);
            tamanho = BitConverter.ToInt32(num, 0);
            str = new byte[tamanho];
            socket.Receive(str, tamanho, SocketFlags.None);
            string idCliente = Encoding.UTF8.GetString(str);

            //recebe detalhes
            socket.Receive(num, 4, SocketFlags.None);
            tamanho = BitConverter.ToInt32(num, 0);
            str = new byte[tamanho];
            socket.Receive(str, tamanho, SocketFlags.None);
            string detalhes = Encoding.UTF8.GetString(str);

            //recebe data
            socket.Receive(lon, 8, SocketFlags.None);
            DateTime dt = DateTime.FromBinary(BitConverter.ToInt64(lon));

            //recebe num produtos
            socket.Receive(num, 4, SocketFlags.None);
            int numProdutos = BitConverter.ToInt32(num, 0);
            List<ProdutoPedido> lpp = new List<ProdutoPedido>();
            for(int i = 0; i < numProdutos; i++)
            {
                socket.Receive(num, 4, SocketFlags.None);
                Produto p = ProdutoDAO.getProduto(BitConverter.ToInt32(num, 0));

                socket.Receive(num, 4, SocketFlags.None);

                lpp.Add(new ProdutoPedido(p, BitConverter.ToInt32(num, 0)));
            }

            return new Pedido(0, idCliente, "", detalhes, dt, lpp, idPlayer);
        }
    }
}


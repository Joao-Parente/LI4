using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AppCliente
{
    /** 
    * @brief Classe utilizada para o AppCliente comunicar com o servidor. 
    */
    public class LN
    {
        private Dictionary<string, List<Produto>> produtos;
        private Dictionary<int, Produto> produtosMAP;
        private ObservableCollection<Categoria> categorias;
        private Socket master;

        private string email_idCliente;
        private string nome_cliente;

        private ObservableCollection<ProdutoInfo> favoritosInfo;

        private ObservableCollection<Pedido> pedidosAnterioes;
        private ObservableCollection<PedidoInfo> pedidosAnterioresInfo;
        private Dictionary<int, Pedido> pedidosMAP;

        private Dictionary<int, Produto> carrinhoMAP;
        private Dictionary<int, ProdutoInfo> carrinhoInfoMAP;
        private ObservableCollection<ProdutoInfo> carrinho;
        private float precoCarrinho;

        private ObservableCollection<PedidoInfo> pedidosEmProcessamentoInfo;
        private ObservableCollection<Pedido> pedidosEmProcessamento;

        private string playerID;

        /** 
        * Construtor por defeito, estabelece a comunicação com o servidor e cria as estruturas 
        */
        public LN()
        {
            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("2.83.152.80"),12344);
            try
            {
                master.Connect(ipe);
                master.Blocking = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            produtosMAP = new Dictionary<int, Produto>();
            precoCarrinho = 0;
            carrinhoMAP = new Dictionary<int, Produto>();
            carrinhoInfoMAP = new Dictionary<int, ProdutoInfo>();
            carrinho = new ObservableCollection<ProdutoInfo>();
            favoritosInfo = new ObservableCollection<ProdutoInfo>();
            pedidosEmProcessamentoInfo = new ObservableCollection<PedidoInfo>();
            pedidosEmProcessamento = new ObservableCollection<Pedido>();
        }

        /**
        * Atualiza o id do cliente atual
        @param playerID
        */
        public void setIdsNotfs(string playerID)
        {
            this.playerID = playerID;
        }

        /**
        * Receve do servidor o identificador do pedido que foi feito num determinado dia e hora
        @param dt
        */
        public int getIdPedido(DateTime dt)
        {
            byte[] ped = new byte[4];
            lock (this)
            {
                //envia o id da operacao
                master.Send(BitConverter.GetBytes(17));

                //envia o id do cliente que fez o pedido
                byte[] idCliente = Encoding.UTF8.GetBytes(email_idCliente);
                master.Send(BitConverter.GetBytes(idCliente.Length), 4, SocketFlags.None);
                master.Send(idCliente, idCliente.Length, SocketFlags.None);

                //envia a data de quando foi feito
                long dataa = dt.ToBinary();
                master.Send(BitConverter.GetBytes(dataa), 8, SocketFlags.None);
               
                master.Receive(ped, 4, SocketFlags.None);               
            }
            return BitConverter.ToInt32(ped, 0);
        }

        /**
        * Recebe do servidor o nome do cliente enviando o email do cliente conectado
        */
        public void setNomeCliente()
        {
            lock (this)
            {
                //id operacao
                master.Send(BitConverter.GetBytes(14));

                //envia email
                byte[] email = Encoding.UTF8.GetBytes(email_idCliente);
                master.Send(BitConverter.GetBytes(email.Length), 4, SocketFlags.None);
                master.Send(email, email.Length, SocketFlags.None);

                //recebe nome
                byte[] num = new byte[4];
                master.Receive(num, 4, SocketFlags.None);
                int size = BitConverter.ToInt32(num, 0);
                byte[] nome = new byte[size];
                master.Receive(nome, nome.Length, SocketFlags.None);

                nome_cliente = "Olá " + Encoding.UTF8.GetString(nome);
            }
        }

        /**
        * Retorna o nome do cliente com a sessão inicializada
        */
        public string getNomeCliente()
        {
            return this.nome_cliente;
        }

        /**
        * Retorna o email do cliente com a sessão inicializada
        */
        public string getEmailIdCliente()
        {
            return this.email_idCliente;
        }

        /**
        * Inicializa os produtos 
        */
        public void inicializaProdutosMAP()
        {
            for(int i = 0; i < categorias.Count; i++)
            {
                List<Produto> ps = produtos[categorias[i].Nome];
                for(int j = 0; j < ps.Count; j++)
                {
                    produtosMAP.Add(ps[j].id, ps[j]);
                }
            }
        }

        /**
        * Retorna um produto pelo seu identificador
        @param id
        */
        public Produto getProduto(int id)
        {
            return produtosMAP[id];
        }

        /**
        * Retorna os produtos que estão disponíveis
        */
        public Dictionary<string, List<Produto>> getProdutos()
        {
            return this.produtos;
        }

        /**
        * Coloca os produtos que recebe como os produtos disponíveis 
        */
        public void setProdutos(Dictionary<string, List<Produto>> x)
        {
            this.produtos = x;
        }

        /**
        * Obtêm todos os produtos no carrinho
        */
        public ObservableCollection<ProdutoInfo> getCarrinho()
        {
            return this.carrinho;
        }

        /**
        * Adiciona um produto ao carrinho
        @param p produto a adicionar
        */
        public void adicionaProdutoCarrinho(Produto p)
        {
            if (carrinhoInfoMAP.ContainsKey(p.id))
            {
                carrinhoInfoMAP[p.id].Quantidades++;
                float preice = this.getPrecoTotal() + p.preco;
                setPrecoTotal((float) Math.Round(preice, 2));
            }
            else
            {
                ProdutoInfo pi = new ProdutoInfo(p.id, p.nome, "" + p.preco + "€", 1,p.imagem);
                carrinhoMAP.Add(p.id, p);
                carrinhoInfoMAP.Add(pi.Id, pi);
                carrinho.Add(pi);
                float preice2 = this.getPrecoTotal() + p.preco;
                setPrecoTotal((float) Math.Round(preice2, 2));
            }
        }

        /**
        * Remove um produto do carrinho
        @param p produto a remover
        */
        public void removeProdutoCarrinho(ProdutoInfo p)
        {
            if (carrinhoInfoMAP.ContainsKey(p.Id))
            {
                carrinho.Remove(p);
                carrinhoInfoMAP.Remove(p.Id);
                carrinhoMAP.Remove(p.Id);
                float price = this.getPrecoTotal() - produtosMAP[p.Id].preco * p.Quantidades;
                setPrecoTotal((float) Math.Round(price, 2));
            }
        }

        /**
        * Atualiza o preço do carrinho ao adicionar um produto
        @param p produto a adicionar
        */
        public void atualizaPrecoTotalMais(ProdutoInfo p)
        {
            if (carrinhoInfoMAP.ContainsKey(p.Id))
            {
                float price = this.getPrecoTotal() + carrinhoMAP[p.Id].preco;
                setPrecoTotal((float) Math.Round(price, 2));
            }
        }

        /**
        * Atualiza o preço do carrinho ao remover um produto
        @param p produto a remover
        */
        public void atualizaPrecoTotalMenos(ProdutoInfo p)
        {
            if (carrinhoInfoMAP.ContainsKey(p.Id))
            {
                float price = this.getPrecoTotal() - carrinhoMAP[p.Id].preco;
                setPrecoTotal((float) Math.Round(price, 2));
            }
        }

        /**
        * Obtêm o preço do carrinho 
        */
        public float getPrecoTotal()
        {
            lock (this)
            {
                return this.precoCarrinho;
            }            
        }

        /**
        * Atualiza o preço do carrinho 
        @param a novo preco do carrinho
        */
        public void setPrecoTotal(float a)
        {
            lock (this)
            {
                this.precoCarrinho = a;
            }
        }

        /**
        * Coloca todos os produtos nas respetivas categorias
        */
        public void preencheCategorias()
        {
            categorias = new ObservableCollection<Categoria>();
            string[] a = new string[produtos.Keys.Count];
            produtos.Keys.CopyTo(a, 0);
            for(int i = 0; i < produtos.Keys.Count; i++)
            {
                categorias.Add(new Categoria(a[i], produtos[a[i]][0].imagem,""+produtos[a[i]].Count+" Produtos"));
            }
        }

        /**
        * Obtêm todas as categorias
        */
        public ObservableCollection<Categoria> GetCategorias()
        {
            return this.categorias;
        }

        /**
        * Envia para o servidor um emal,password e um nome para o servidor registar o cliente se possível retornando true senão o servidor retorna false para indicar que não foi possível registar
        * @param email
        * @param password
        * @param nome
        */
        public bool RegistaUtilizador(string email, string password, string nome)
        {
            lock (this)
            {
                byte[] aux;

                //envia id operacao
                byte[] dados = BitConverter.GetBytes(11);
                master.Send(dados, 4, SocketFlags.None);

                //envia email
                aux = Encoding.UTF8.GetBytes(email);
                dados = BitConverter.GetBytes(aux.Length);
                master.Send(dados, 4, SocketFlags.None);
                dados = aux;
                master.Send(dados, dados.Length, SocketFlags.None);

                //envia password
                aux = Encoding.UTF8.GetBytes(password);
                dados = BitConverter.GetBytes(aux.Length);
                master.Send(dados, 4, SocketFlags.None);
                dados = aux;
                master.Send(dados, dados.Length, SocketFlags.None);

                //envia nome
                aux = Encoding.UTF8.GetBytes(nome);
                dados = BitConverter.GetBytes(aux.Length);
                master.Send(dados, 4, SocketFlags.None);
                dados = aux;
                master.Send(dados, dados.Length, SocketFlags.None);

                //recebe resposta
                dados = new byte[1];
                master.Receive(dados, 1, SocketFlags.None);

                if (dados[0] == 1)
                {
                    email_idCliente = email;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /**
        * Recebe um produto do servidor
        */
        public Produto RecebeProdutoManual()
        {
            byte[] data = new byte[4];

            master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

            //recebe id
            master.Receive(data, 4, SocketFlags.None);
            int id = BitConverter.ToInt32(data, 0);

            master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

            //recebe tipo
            master.Receive(data, 4, SocketFlags.None);
            int numbytes = BitConverter.ToInt32(data, 0);
            byte[] str = new byte[numbytes];
            master.Receive(str, numbytes, SocketFlags.None);
            string tipo = Encoding.UTF8.GetString(str);

            master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

            //recebe nome
            master.Receive(data, 4, SocketFlags.None);
            numbytes = BitConverter.ToInt32(data, 0);
            str = new byte[numbytes];
            master.Receive(str, numbytes, SocketFlags.None);
            string nome = Encoding.UTF8.GetString(str);

            master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

            //recebe detalhes
            master.Receive(data, 4, SocketFlags.None);
            numbytes = BitConverter.ToInt32(data, 0);
            str = new byte[numbytes];
            master.Receive(str, numbytes, SocketFlags.None);
            string detalhes = Encoding.UTF8.GetString(str);

            master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

            //recebe disponibilidade
            master.Receive(data, 4, SocketFlags.None);
            int disponibilidade = BitConverter.ToInt32(data, 0);

            master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

            //recebe preco
            master.Receive(data, 4, SocketFlags.None);
            int tamanhofloat = BitConverter.ToInt32(data, 0);
            data = new byte[tamanhofloat];
            master.Receive(data, tamanhofloat, SocketFlags.None);
            float preco = BitConverter.ToSingle(data, 0);

            master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

            data = new byte[4];
            master.Receive(data, 4, SocketFlags.None);
            int tamanhoimagem = BitConverter.ToInt32(data, 0);
            byte[] bites = new byte[tamanhoimagem];

            int lidos = 0;
            while (lidos < tamanhoimagem)
            {
                lidos += master.Receive(bites, lidos, tamanhoimagem - lidos, SocketFlags.None);
            }

            master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

            return new Produto(id, tipo, nome, detalhes, disponibilidade, preco, bites);

        }

        /**
        * Recebe do servidor todos os produtos disponíveis
        */
        public Dictionary<string, List<Produto>> verProdutos()
        {
            lock (this)
            {
                Dictionary<string, List<Produto>> dic = new Dictionary<string, List<Produto>>();

                //envia o id da operacao
                byte[] id = new byte[4];
                id = BitConverter.GetBytes(1);
                master.Send(id);


                byte[] tamT = new byte[4];
                master.Receive(tamT, 0, 4, SocketFlags.None);
                int numTipos = BitConverter.ToInt32(tamT, 0);

                for (int i = 0; i < numTipos; i++)
                {

                    //recebe o tamanho da string categoria
                    master.Receive(tamT, 4, SocketFlags.None);
                    int tamanho = BitConverter.ToInt32(tamT, 0);

                    //recebe os bytes da string
                    byte[] nome = new byte[tamanho];
                    master.Receive(nome, tamanho, SocketFlags.None);
                    string nomeCategoria = Encoding.UTF8.GetString(nome);

                    byte[] tamN = new byte[4];
                    master.Receive(tamN, 0, 4, SocketFlags.None);
                    int numNom = BitConverter.ToInt32(tamN, 0);

                    for (int j = 0; j < numNom; j++)
                    {
                        master.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);
                        if (dic.ContainsKey(nomeCategoria))
                        {
                            Produto p = RecebeProdutoManual();
                            List<Produto> lps = dic[nomeCategoria];
                            lps.Add(p);
                        }
                        else
                        {
                            Produto p = RecebeProdutoManual();
                            List<Produto> lp = new List<Produto>();
                            lp.Add(p);
                            dic.Add(nomeCategoria, lp);
                        }
                    }

                }
                return dic;
            }
        }

        /**
        * Envia um pedido ao servidor 
        @param p Pedido a enviar
        */
        public void EnviaPedidoManual(Pedido p)
        {
            byte[] str;

            //envia idcliente notfs
            str = Encoding.UTF8.GetBytes(this.playerID);
            master.Send(BitConverter.GetBytes(str.Length), 4, SocketFlags.None);
            master.Send(str, str.Length, SocketFlags.None);

            //envia idCliente
            str = Encoding.UTF8.GetBytes(p.idCliente);
            master.Send(BitConverter.GetBytes(str.Length), 4, SocketFlags.None);
            master.Send(str, str.Length, SocketFlags.None);

            //envia detalhes
            str = Encoding.UTF8.GetBytes(p.detalhes);
            master.Send(BitConverter.GetBytes(str.Length), 4, SocketFlags.None);
            master.Send(str, str.Length, SocketFlags.None);

            //envia data
            str = BitConverter.GetBytes(p.data_hora.ToBinary());
            master.Send(str, 8, SocketFlags.None);

            //envia o numero de produtospedido
            master.Send(BitConverter.GetBytes(p.produtos.Count), 4, SocketFlags.None);

            for (int i = 0; i < p.produtos.Count; i++)
            {
                //envia o id do produto
                master.Send(BitConverter.GetBytes(p.produtos[i].p.id), 4, SocketFlags.None);

                //envia a quantidade
                master.Send(BitConverter.GetBytes(p.produtos[i].quantidades), 4, SocketFlags.None);
            }
        }

        /**
        * Recebe todos os pedidos de um cliente
        @param idCliente identificador do cliente 
        */
        public void RecebePedidosManual(string idCliente)
        {
            byte[] num = new byte[4], str;

            master.Receive(num, 4, SocketFlags.None);
            int numPedidos = BitConverter.ToInt32(num, 0);

            pedidosAnterioes = new ObservableCollection<Pedido>();
            pedidosAnterioresInfo = new ObservableCollection<PedidoInfo>();
            pedidosMAP = new Dictionary<int, Pedido>();

            for (int i = 0; i < numPedidos; i++)
            {
                str = new byte[8];
                master.Receive(str, 8, SocketFlags.None);
                DateTime data = new DateTime(BitConverter.ToInt64(str, 0));

                master.Receive(num, 4, SocketFlags.None);
                int id = BitConverter.ToInt32(num, 0);

                master.Receive(num, 4, SocketFlags.None);
                int tamEmpregado = BitConverter.ToInt32(num, 0);
                str = new byte[tamEmpregado];
                master.Receive(str, tamEmpregado, SocketFlags.None);
                string idEmpregado = Encoding.UTF8.GetString(str);

                master.Receive(num, 4, SocketFlags.None);
                int numProdutos = BitConverter.ToInt32(num, 0);

                List<ProdutoPedido> apr = new List<ProdutoPedido>(numProdutos);
                float preco = 0;
                for (int j = 0; j < numProdutos; j++)
                {
                    master.Receive(num, 4, SocketFlags.None);
                    int idProduto = BitConverter.ToInt32(num, 0);

                    master.Receive(num, 4, SocketFlags.None);
                    int quantidades = BitConverter.ToInt32(num, 0);

                    preco += quantidades * produtosMAP[idProduto].preco;

                    apr.Add(new ProdutoPedido(produtosMAP[idProduto], quantidades));
                }

                pedidosAnterioresInfo.Add(new PedidoInfo(id, data, preco, numProdutos));
                Pedido ped = new Pedido(id, idCliente, idEmpregado, "null", data, apr);
                pedidosAnterioes.Add(ped);
                pedidosMAP.Add(ped.id, ped);
            }
        }

        /**
        * Recebe um pedido
        @param idPedido identificador do pedido 
        */
        public Pedido GetPedido(int idPedido)
        {
            return pedidosMAP[idPedido];
        }

        /**
        * Recebe todos os pedidos que um cliente fez anteriormente
        */
        public ObservableCollection<PedidoInfo> GetPedidosInfo()
        {
            return this.pedidosAnterioresInfo;
        }

        /**
        * Recebe todos os pedidos que um cliente fez anteriormente
        @param idCliente identificador do cliente 
        */
        public void PedidosAnteriores(string idCliente)
        {
            lock (this)
            {
                List<Pedido> anteriores = new List<Pedido>();

                //id operacao
                byte[] id = new byte[4];
                id = BitConverter.GetBytes(2);
                master.Send(id);

                //id do cliente
                byte[] aux = Encoding.UTF8.GetBytes(idCliente);
                byte[] dados = BitConverter.GetBytes(aux.Length);
                master.Send(dados, 4, SocketFlags.None);
                dados = aux;
                master.Send(dados, dados.Length, SocketFlags.None);

                RecebePedidosManual(idCliente);
            }               
        }

        /**
        * Recebe do servidor o número do último ticket processado e o número do ticket que alguém teria se efetua-se um pedido 
        */
        public List<int> NoUltimoPedido()
        {           
            List<int> r = new List<int>();
            int x, y;
            lock (this)
            {
                //envia id operacao
                master.Send(BitConverter.GetBytes(5));

                byte[] num = new byte[4];
                master.Receive(num, 4, SocketFlags.None);
                x = BitConverter.ToInt32(num, 0);

                num = new byte[4];
                master.Receive(num, 4, SocketFlags.None);
                y = BitConverter.ToInt32(num, 0);
            }    
            r.Add(x);
            r.Add(y);

            return r;
        }

        /**
        * Adiciona um produto aos favoritos do cliente em questão
        @param idProduto identificador do produto 
        */
        public bool NovoProdutoFavorito(int idProduto)
        {
            //envia id operacao
            byte[] id = new byte[4];

            lock (this)
            {
                id = BitConverter.GetBytes(6);
                master.Send(id);

                //envia idProduto
                id = BitConverter.GetBytes(idProduto);
                master.Send(id);

                //id do cliente
                byte[] aux = Encoding.UTF8.GetBytes(this.email_idCliente);
                byte[] dados = BitConverter.GetBytes(aux.Length);
                master.Send(dados, 4, SocketFlags.None);
                dados = aux;
                master.Send(dados, dados.Length, SocketFlags.None);

                //recebe confirmacao se o produto foi adicionado com sucesso ou nao
                master.Receive(id, 0, 4, SocketFlags.None);
            }

            int confirmacao = BitConverter.ToInt32(id, 0);

            if (confirmacao == 1)
            {
                this.favoritosInfo.Add(new ProdutoInfo(produtosMAP[idProduto].id, produtosMAP[idProduto].nome,
                                    "" + produtosMAP[idProduto].preco + "€", 0, produtosMAP[idProduto].imagem));
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
        * Remove um produto dos favoritos do cliente que tem a sessão ativa
        @param idProduto identificador do produto 
        */
        public bool removerProdFavorito(int idProduto)
        {
            byte[] id = new byte[4];
            lock (this)
            {
                id = BitConverter.GetBytes(15);
                master.Send(id, 4, SocketFlags.None);

                //envia idProduto
                id = BitConverter.GetBytes(idProduto);
                master.Send(id, 4, SocketFlags.None);

                //envia idCliente
                byte[] msg = Encoding.UTF8.GetBytes(this.email_idCliente);
                master.Send(BitConverter.GetBytes(msg.Length), 4, SocketFlags.None);
                master.Send(msg, msg.Length, SocketFlags.None);

                //recebe confirmacao se o produto foi adicionado com sucesso ou nao
                master.Receive(id, 0, 4, SocketFlags.None);
            }

            int confirmacao = BitConverter.ToInt32(id, 0);

            if (confirmacao == 1)
            {
                for(int i = 0; i < favoritosInfo.Count; i++)
                {
                    if (favoritosInfo[i].Id == idProduto)
                    {
                        favoritosInfo.RemoveAt(i);
                        break;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /**
        * Envia o pedido p para o servidor
        @param p
        */
        public int EfetuarPedido(Pedido p)
        {
            byte[] quatro_bites = new byte[4];
            lock (this)
            {
                master.Send(BitConverter.GetBytes(4));
                EnviaPedidoManual(p);
                this.pedidosEmProcessamento.Add(p);
                int numero = 0;
                float preco = 0;
                for (int i = 0; i < p.produtos.Count; i++)
                {
                    preco += p.produtos[i].p.preco * p.produtos[i].quantidades;
                    numero += p.produtos[i].quantidades;
                }
                this.pedidosEmProcessamentoInfo.Add(new PedidoInfo(p.id, p.data_hora, preco, numero));

                master.Receive(quatro_bites, 4, SocketFlags.None);
            }

            return BitConverter.ToInt32(quatro_bites, 0);
        }

        /**
        * Inicia a sessão
        @param email 
        @param password
        */
        public bool IniciarSessao(string email, string password)
        {
            byte[] aux,dados;

            lock (this)
            {
                //envia id operacao
                dados = BitConverter.GetBytes(9);
                master.Send(dados, 4, SocketFlags.None);

                //envia email
                aux = Encoding.UTF8.GetBytes(email);
                dados = BitConverter.GetBytes(aux.Length);
                master.Send(dados, 4, SocketFlags.None);
                dados = aux;
                master.Send(dados, dados.Length, SocketFlags.None);

                //envia password
                aux = Encoding.UTF8.GetBytes(password);
                dados = BitConverter.GetBytes(aux.Length);
                master.Send(dados, 4, SocketFlags.None);
                dados = aux;
                master.Send(dados, dados.Length, SocketFlags.None);

                //recebe resposta
                dados = new byte[1];
                master.Receive(dados, 1, SocketFlags.None);
            }

            if (dados[0] == 1)
            {
                email_idCliente = email;
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
        * Termina a sessão do utilizador
        */
        public bool TerminarSessao()
        {
            lock (this)
            {
                byte[] id = new byte[4];
                id = BitConverter.GetBytes(10);
                master.Send(id);
                return false;
            }
        }

        /**
        * Regista uma Reclamação
        @param idPedido identificador do pedido 
        @param motivo tópico da reclamacao
        @param reclamacao, a reclamação em si
        */
        public bool reclamacao(int idPedido, string motivo, string reclamacao)
        {
            //envia id operacao
            byte[] id = new byte[4];
            lock (this)
            {
                id = BitConverter.GetBytes(12);
                master.Send(id);

                //envia id pedido
                id = new byte[4];
                id = BitConverter.GetBytes(idPedido);
                master.Send(id);

                //envia tamanho reclamacao
                int tamanhoString = reclamacao.Length;
                id = BitConverter.GetBytes(tamanhoString);
                master.Send(id);
                //envia reclamacao
                byte[] msg = new byte[tamanhoString];
                msg = Encoding.UTF8.GetBytes(reclamacao);
                master.Send(msg);

                //recebe confirmacao se a reclamacao foi feita com sucesso ou nao
                master.Receive(id, 0, 4, SocketFlags.None);
            }
            int confirmacao = BitConverter.ToInt32(id, 0);

            if (confirmacao == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /**
        * Inicializa os produtos favoritos com os produtos favoritos do cliente
        @param idCliente email do cliente em questão
        */
        public void prodsFavoritos(string idCliente)
        {
            lock (this)
            {
                //envia id operacao
                master.Send(BitConverter.GetBytes(13));

                //envia o idCliente
                master.Send(BitConverter.GetBytes(idCliente.Length));
                master.Send(Encoding.UTF8.GetBytes(idCliente));

                //recebe a resposta
                byte[] inte = new byte[4];
                master.Receive(inte, 4, SocketFlags.None);
                int size = BitConverter.ToInt32(inte, 0);

                List<int> ret = new List<int>();
                if (size > 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        master.Receive(inte, 4, SocketFlags.None);
                        int idProd = BitConverter.ToInt32(inte, 0);
                        this.favoritosInfo.Add(new ProdutoInfo(produtosMAP[idProd].id, produtosMAP[idProd].nome,
                                        "" + produtosMAP[idProd].preco + "€", 0, produtosMAP[idProd].imagem));
                    }
                }
            }
        }

        /**
        * Retorna os produtos favoritos
        */
        public ObservableCollection<ProdutoInfo> getProdsFavoritos()
        {
            return this.favoritosInfo;
        }

        /**
        * Retorna os pedidos que estão por preparar (em formato Pedido)
        */
        public ObservableCollection<Pedido> getPedidosPorPreparar()
        {
            return this.pedidosEmProcessamento;
        }

        /**
        * Retorna os pedidos que estão por preparar (em formato PedidoInfo)
        */
        public ObservableCollection<PedidoInfo> getPedidosPorPrepararInfo()
        {
            return this.pedidosEmProcessamentoInfo;
        }

        /**
        * Atualiza o estado dos pedidos
        */
        public void atualizaEstadoPedidos()
        {
            lock (this)
            {
                List<Pedido> pedidos_a_remover = new List<Pedido>();
                List<PedidoInfo> pedidosinfo_a_remover = new List<PedidoInfo>();
                for (int i = 0; i < pedidosEmProcessamento.Count; i++)
                {
                    string estado = getEstado(pedidosEmProcessamento[i]);
                    if (estado == "null")
                    {
                        pedidos_a_remover.Add(pedidosEmProcessamento[i]);
                        pedidosinfo_a_remover.Add(pedidosEmProcessamentoInfo[i]);
                    }
                    else
                    {
                        pedidosEmProcessamentoInfo[i].setEstado(estado);
                    }
                }
                if (pedidos_a_remover.Count > 0)
                {
                    for (int i = 0; i < pedidos_a_remover.Count; i++)
                    {
                        pedidosEmProcessamento.Remove(pedidos_a_remover[i]);
                        pedidosEmProcessamentoInfo.Remove(pedidosinfo_a_remover[i]);
                    }
                }
            }
        }

        /**
        * Obter estado de um pedido (Por preparar, Em Preparação , Pronto)
        @param p pedido que se pretende obter o estado
        */
        public string getEstado(Pedido p)
        {            
            int felix = master.Available;
            if(felix > 0)
            {
                master.Receive(new byte[felix], felix, SocketFlags.None);
            }
            master.Send(BitConverter.GetBytes(16), 4, SocketFlags.None);

            //envia o id do cliente que fez o pedido
            byte[] idCliente = Encoding.UTF8.GetBytes(email_idCliente);
            master.Send(BitConverter.GetBytes(idCliente.Length),4,SocketFlags.None);
            master.Send(idCliente, idCliente.Length, SocketFlags.None);

            //envia a data de quando foi feito
            long dataa = p.data_hora.ToBinary();
            master.Send(BitConverter.GetBytes(dataa), 8, SocketFlags.None);

            byte[] resposta = new byte[4];
            int quantos_Leu = master.Receive(resposta, 4, SocketFlags.None);
            int r = BitConverter.ToInt32(resposta, 0);

            if (r == 1)
            {
                return "Por Preparar";
            }
            else if(r == 2)
            {
                return "Em Preparação";
            }
            else if (r == 3)
            {
                return "Pronto a Levantar";
            }
            return "null";
        }
        

    }
}
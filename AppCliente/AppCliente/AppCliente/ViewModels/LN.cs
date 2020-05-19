using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AppCliente
{
    public class LN
    {
        private Dictionary<string, List<Produto>> produtos;
        private Dictionary<int, Produto> produtosMAP;
        private ObservableCollection<Categoria> categorias;
        private List<Produto> favoritos;
        private List<Pedido> historico;
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


        public LN()
        {
            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("192.168.1.69"),12344);
            try
            {
                master.Connect(ipe);
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

        public void setNomeCliente()
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

        public string getNomeCliente()
        {
            return this.nome_cliente;
        }

        public string getEmailIdCliente()
        {
            return this.email_idCliente;
        }

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

        public Produto getProduto(int id)
        {
            return produtosMAP[id];
        }

        public Dictionary<string, List<Produto>> getProdutos()
        {
            return this.produtos;
        }

        public void setProdutos(Dictionary<string, List<Produto>> x)
        {
            this.produtos = x;
        }

        public ObservableCollection<ProdutoInfo> getCarrinho()
        {
            return this.carrinho;
        }

        public void adicionaProdutoCarrinho(Produto p)
        {
            if (carrinhoInfoMAP.ContainsKey(p.id))
            {
                carrinhoInfoMAP[p.id].Quantidades++;
                setPrecoTotal(this.getPrecoTotal() + p.preco);
            }
            else
            {
                ProdutoInfo pi = new ProdutoInfo(p.id, p.nome, "" + p.preco + "€", 1,p.imagem);
                carrinhoMAP.Add(p.id, p);
                carrinhoInfoMAP.Add(pi.Id, pi);
                carrinho.Add(pi);
                setPrecoTotal(this.getPrecoTotal() + p.preco);
            }
        }

        public void removeProdutoCarrinho(ProdutoInfo p)
        {
            if (carrinhoInfoMAP.ContainsKey(p.Id))
            {
                carrinho.Remove(p);
                carrinhoInfoMAP.Remove(p.Id);
                carrinhoMAP.Remove(p.Id);
                setPrecoTotal(this.getPrecoTotal() - produtosMAP[p.Id].preco * p.Quantidades);
            }
        }

        public void atualizaPrecoTotalMais(ProdutoInfo p)
        {
            if (carrinhoInfoMAP.ContainsKey(p.Id))
            {
                //this.precoCarrinho += carrinhoMAP[p.Id].preco;
                setPrecoTotal(this.getPrecoTotal() + carrinhoMAP[p.Id].preco);
            }
        }

        public void atualizaPrecoTotalMenos(ProdutoInfo p)
        {
            if (carrinhoInfoMAP.ContainsKey(p.Id))
            {
                //this.precoCarrinho -= carrinhoMAP[p.Id].preco;
                setPrecoTotal(this.getPrecoTotal() - carrinhoMAP[p.Id].preco);
            }
        }

        public float getPrecoTotal()
        {
            lock (this)
            {
                return this.precoCarrinho;
            }            
        }

        public void setPrecoTotal(float a)
        {
            lock (this)
            {
                this.precoCarrinho = a;
            }
        }

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

        public ObservableCollection<Categoria> GetCategorias()
        {
            return this.categorias;
        }

        public Boolean RegistaUtilizador(string email, string password, string nome)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(11);
            master.Send(id);

            byte[] msg = new byte[512];
            string email_pw_nome = email + "|" + password + "|" + nome + "|";
            msg = Encoding.ASCII.GetBytes(email_pw_nome);
            master.Send(msg);

            byte[] log = new byte[30];
            master.Receive(log);
            bool login = BitConverter.ToBoolean(log, 0);

            if (login)
            {
                Console.WriteLine("Sign me in");
                email_idCliente = email;
            }
            else Console.WriteLine("Dont mess with the system");

            return login;
        }


        public Produto RecebeProduto()
        {
            byte[] data = new byte[4];
            master.Receive(data, 0, 4, SocketFlags.None);
            int numero_total = BitConverter.ToInt32(data, 0);

            data = new byte[numero_total];
            master.Receive(data, numero_total, SocketFlags.None);
            
            return Produto.loadFromBytes(data);
        }

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
            
            return new Produto(id,tipo,nome,detalhes,disponibilidade,preco,bites);
        }

        public Dictionary<string, List<Produto>> verProdutos()
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

        public Pedido RecebePedido()
        {
            int size = 100;
            byte[] data = new byte[size];

            master.Receive(data, 0, 4, SocketFlags.None); // 4bytes ->1 int que � o tamanho de bytes a recebr
            int numero_total = BitConverter.ToInt32(data, 0);

            data = new byte[numero_total];
            master.Receive(data, numero_total, SocketFlags.None);

            return Pedido.loadFromBytes(data);
        }

        public void EnviaPedidoManual(Pedido p)
        {
            byte[] str;
            //envia id n e preciso
            //master.Send(BitConverter.GetBytes(p.id), 4, SocketFlags.None);

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

                pedidosAnterioresInfo.Add(new PedidoInfo(id,data,preco,numProdutos));
                Pedido ped = new Pedido(id, idCliente, idEmpregado, "null", data, apr);
                pedidosAnterioes.Add(ped);
                pedidosMAP.Add(ped.id, ped);
            }
            

        }

        public Pedido GetPedido(int idPedido)
        {
            return pedidosMAP[idPedido];
        }

        public ObservableCollection<PedidoInfo> GetPedidosInfo()
        {
            return this.pedidosAnterioresInfo;
        }

        public void PedidosAnteriores(string idCliente)
        {
            List<Pedido> anteriores = new List<Pedido>();

            //id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(2);
            master.Send(id);

            //id do cliente
            byte[] msg = new byte[512];
            msg = Encoding.ASCII.GetBytes(idCliente);
            master.Send(msg);

            RecebePedidosManual(idCliente);
                
        }

        public Boolean alterarPedido(int a, int idPedido, String produtos)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(3);
            master.Send(id);

            //envia id do pedido
            byte[] idPed = new byte[8];
            idPed = BitConverter.GetBytes(idPedido);
            master.Send(idPed);

            //envia 0 ou 1 para adicionar ou remover produtos respetivamente
            byte[] opcao = new byte[4];
            opcao = BitConverter.GetBytes(a);
            master.Send(opcao);

            //envia string com todos os produtos
            byte[] msg = new byte[512];
            msg = Encoding.ASCII.GetBytes(produtos);
            master.Send(msg);

            return true;
        }

        public List<int> NoUltimoPedido()
        {
            List<int> r = new List<int>();

            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(5);
            master.Send(id);

            byte[] num = new byte[4];
            master.Receive(num, 4, SocketFlags.None);
            int x = BitConverter.ToInt32(num,0);

            num = new byte[4];
            master.Receive(num, 4, SocketFlags.None);
            int y = BitConverter.ToInt32(num,0);

            r.Add(x);
            r.Add(y);

            Console.WriteLine("Numero ultimo pedido" + x + "e o outro " + y);
            return r;
        }

        public bool NovoProdutoFavorito(int idProduto)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(6);
            master.Send(id);

            //envia idProduto
            id = BitConverter.GetBytes(idProduto);
            master.Send(id);

            //envia idCliente
            byte[] msg = new byte[512]; //falta por o email dele aqui
            msg = Encoding.ASCII.GetBytes(this.email_idCliente);
            master.Send(msg);

            //recebe confirmacao se o produto foi adicionado com sucesso ou nao
            master.Receive(id, 0, 4, SocketFlags.None);
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

        public bool removerProdFavorito(int idProduto)
        {
            byte[] id = new byte[4];

            id = BitConverter.GetBytes(15);
            master.Send(id, 4, SocketFlags.None);

            //envia idProduto
            id = BitConverter.GetBytes(idProduto);
            master.Send(id,4,SocketFlags.None);

            //envia idCliente
            byte[] msg = Encoding.UTF8.GetBytes(this.email_idCliente);
            master.Send(BitConverter.GetBytes(msg.Length), 4, SocketFlags.None);
            master.Send(msg,msg.Length,SocketFlags.None);

            //recebe confirmacao se o produto foi adicionado com sucesso ou nao
            master.Receive(id, 0, 4, SocketFlags.None);
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

        public int EfetuarPedido(Pedido p)
        {
            /*
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(4);
            master.Send(id);
            enviaPedido(p);
            return 1;*/
            master.Send(BitConverter.GetBytes(4));
            EnviaPedidoManual(p);
            this.pedidosEmProcessamento.Add(p);
            int numero = 0;
            float preco = 0;
            for(int i = 0; i < p.produtos.Count; i++)
            {
                preco += p.produtos[i].p.preco * p.produtos[i].quantidades;
                numero += p.produtos[i].quantidades;
            }
            this.pedidosEmProcessamentoInfo.Add(new PedidoInfo(p.id, p.data_hora, preco, numero));
            return 1;
        }


        public String[] InfoEmpresa()
        {
            string[] info = null;

            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(7);
            master.Send(id);

            byte[] infoBytes = new byte[128];

            for (int i = 0; i < 5; i++)
            {
                master.Receive(infoBytes);
                info[i] = BitConverter.ToString(infoBytes);
            }

            return info;
        }


        //+AvaliarProduto(idProduto : int, idCliente : int, nota : int) : void


        public bool IniciarSessao(string email, string password)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(9);
            master.Send(id);

            byte[] msg = new byte[512];
            string email_pw = email + "|" + password + "|";
            msg = Encoding.ASCII.GetBytes(email_pw);
            master.Send(msg);

            byte[] log = new byte[30];
            master.Receive(log);
            bool login = BitConverter.ToBoolean(log, 0);

            if (login)
            {
                Console.WriteLine("i'm in you crazy bastard");
                email_idCliente = email;
                return login;
            }
            else
            {
                Console.WriteLine("we will get em next time");
                return login;
            }
        }


        public bool TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(10);
            master.Send(id);
            return false;
        }


        public bool reclamacao(int idPedido, string motivo, string reclamacao)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(12);
            master.Send(id);

            //envia id pedido
            id = new byte[4];
            id = BitConverter.GetBytes(idPedido);
            master.Send(id);

            //envia tamanho motivo
            int tamanhoString = motivo.Length;
            id = BitConverter.GetBytes(tamanhoString);
            master.Send(id);
            //envia motivo
            byte[] msg = new byte[tamanhoString];
            msg = Encoding.ASCII.GetBytes(motivo);
            master.Send(msg);

            //envia tamanho reclamacao
            tamanhoString = reclamacao.Length;
            id = BitConverter.GetBytes(tamanhoString);
            master.Send(id);
            //envia reclamacao
            msg = new byte[tamanhoString];
            msg = Encoding.ASCII.GetBytes(reclamacao);
            master.Send(msg);

            //recebe confirmacao se a reclamacao foi feita com sucesso ou nao
            master.Receive(id, 0, 4, SocketFlags.None);
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

        public void enviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            master.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
            master.Send(pedido);
        }

        //esta funcao vai ser chamada para inicializar os produtos favoritos
        public void prodsFavoritos(string idCliente)
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
                                    ""+produtosMAP[idProd].preco+"€",0, produtosMAP[idProd].imagem));
                }               
            }
        }

        public ObservableCollection<ProdutoInfo> getProdsFavoritos()
        {
            return this.favoritosInfo;
        }

        public ObservableCollection<Pedido> getPedidosPorPreparar()
        {
            return this.pedidosEmProcessamento;
        }

        public ObservableCollection<PedidoInfo> getPedidosPorPrepararInfo()
        {
            return this.pedidosEmProcessamentoInfo;
        }

        public void atualizaEstadoPedidos()
        {
            for(int i = 0; i < pedidosEmProcessamento.Count; i++)
            {
                pedidosEmProcessamentoInfo[i].setEstado(getEstado(pedidosEmProcessamento[i]));
            }
        }

        public string getEstado(Pedido p)
        {
            //int li_estes = master.Receive(new byte[100], 100, SocketFlags.None);
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
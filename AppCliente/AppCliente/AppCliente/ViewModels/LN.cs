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

        private Dictionary<int, Produto> carrinhoMAP;
        private Dictionary<int, ProdutoInfo> carrinhoInfoMAP;
        private ObservableCollection<ProdutoInfo> carrinho;
        private float precoCarrinho;

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
        }

        public string getEmailIdCliente()
        {
            return this.email_idCliente;
        }

        public void inicializaCarrinho()
        {
            Dictionary<string, List<Produto>> ps = this.getProdutos();

            List<Produto> sss = ps["baguete"];

            for (int i = 0; i < sss.Count; i++)
            {
                this.adicionaProdutoCarrinho(sss[i]);
            }
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
                //this.precoCarrinho += p.preco;
                setPrecoTotal(this.getPrecoTotal() + p.preco);
            }
            else
            {
                ProdutoInfo pi = new ProdutoInfo(p.id, p.nome, "" + p.preco + "€", 1);
                carrinhoMAP.Add(p.id, p);
                carrinhoInfoMAP.Add(pi.Id, pi);
                carrinho.Add(pi);
                //this.precoCarrinho += p.preco;
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
                categorias.Add(new Categoria(a[i], null /*produtos[a[i]][0].Imagem*/,""+produtos[a[i]].Count+" Produtos"));
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
            int size = 100;
            byte[] data = new byte[size];


            master.Receive(data, 0, 4, SocketFlags.None);
            int numero_total = BitConverter.ToInt32(data, 0);

            data = new byte[numero_total];
            master.Receive(data, numero_total, SocketFlags.None);

            return Produto.loadFromBytes(data);
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
                    if (dic.ContainsKey(nomeCategoria))
                    {
                        Produto p = RecebeProduto();
                        List<Produto> lps = dic[nomeCategoria];
                        lps.Add(p);
                    }
                    else
                    {
                        Produto p = RecebeProduto();
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

        public List<Pedido> RecebePedidosManual(string idCliente)
        {
            byte[] num = new byte[4], str;

            master.Receive(num, 4, SocketFlags.None);
            int numPedidos = BitConverter.ToInt32(num, 0);

            List<Pedido> ret = new List<Pedido>(numPedidos);

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
                for (int j = 0; j < numProdutos; j++)
                {
                    master.Receive(num, 4, SocketFlags.None);
                    int tamanhoProduto = BitConverter.ToInt32(num, 0);
                    byte[] aux = new byte[tamanhoProduto];
                    master.Receive(aux, tamanhoProduto, SocketFlags.None);

                    master.Receive(num, 4, SocketFlags.None);
                    int quantidades = BitConverter.ToInt32(num, 0);

                    apr.Add(new ProdutoPedido(Produto.loadFromBytes(aux), quantidades));
                }

                ret.Add(new Pedido(id, idCliente, idEmpregado, "null", data, apr));
            }

            return ret;

        }

        public List<Pedido> PedidosAnteriores(string idCliente)
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

            return RecebePedidosManual(idCliente);
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
                return true;
            }
            else
            {
                return false;
            }
        }

        public int EfetuarPedido(Pedido p)
        {

            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(4);
            master.Send(id);
            enviaPedido(p);
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
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AppCliente
{
    public class AppClienteLN
    {
        private Dictionary<string, List<Produto>> Produtos;
        private Dictionary<int, Produto> IdsProdutos;
        private List<Produto> Favoritos;
        private List<Pedido> Historico;
        private List<Produto> Carrinho;
        private ObservableCollection<ProdutoCell> CarrinhoOC;
        private Dictionary<int, Produto> IdsProdutosCarrinho;
        private Socket Master;

        private string EmailCliente;

        public AppClienteLN()
        {
            Carrinho = new List<Produto>();
            CarrinhoOC = new ObservableCollection<ProdutoCell>();
            IdsProdutosCarrinho = new Dictionary<int, Produto>();
            Master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("192.168.1.69"), 12344);

            try
            {
                Master.Connect(ipe);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

        }

        public void RefreshProdutos()
        {
            this.Produtos = VerProdutos();
            IdsProdutos = new Dictionary<int, Produto>();
            
            foreach(List<Produto> lps in Produtos.Values)
            {
                for(int i = 0; i < lps.Count; i++)
                {
                    IdsProdutos.Add(lps[i].id, lps[i]);
                }
            }

        }

        public Dictionary<string, List<Produto>> GetProdutos()
        {
            return this.Produtos;
        }

        public Produto GetProduto(ProdutoCell p)
        {
            return IdsProdutos[p.id];
        }

        public bool ProdutoCarrinho(Produto p)
        {
            if (IdsProdutosCarrinho.ContainsKey(p.id))
            {
                return false;
            }
            else
            {
                IdsProdutosCarrinho.Add(p.id, p);
                Carrinho.Add(p);
                CarrinhoOC.Add(new ProdutoCell(p.id, p.nome, ""+p.preco+"€"));
                return true;
            }
        }

        public List<Produto> GetCarrinho()
        {
            return this.Carrinho;
        }

        public ObservableCollection<ProdutoCell> GetCarrinhoOC()
        {
            return this.CarrinhoOC;
        }

        public ObservableCollection<ProdutoCell> GetCarrinhoCell()
        {
            ObservableCollection<ProdutoCell> ret = new ObservableCollection<ProdutoCell>();
            for(int i = 0; i < Carrinho.Count; i++)
            {
                ret.Add(new ProdutoCell(Carrinho[i].id, Carrinho[i].nome, "" + Carrinho[i].preco + "€"));
            }
            return ret;
        }

        public void RemoveCarrinho(ProdutoCell p)
        {
            CarrinhoOC.Remove(p);
            IdsProdutosCarrinho.Remove(p.id);
        }

        public Boolean RegistaUtilizador(string email, string password, string nome)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(11);
            Master.Send(id);

            byte[] msg = new byte[512];
            string email_pw_nome = email + "|" + password + "|" + nome + "|";
            msg = Encoding.ASCII.GetBytes(email_pw_nome);
            Master.Send(msg);

            byte[] log = new byte[30];
            Master.Receive(log);
            bool login = BitConverter.ToBoolean(log, 0);

            if (login)
            {
                Console.WriteLine("Sign me in");
                EmailCliente = email;
            }
            else Console.WriteLine("Dont mess with the system");

            return login;
        }


        public Produto RecebeProduto()
        {
            byte[] data = new byte[4];

            Master.Receive(data, 0, 4, SocketFlags.None);
            int numero_total = BitConverter.ToInt32(data, 0);

            data = new byte[numero_total];
            Master.Receive(data, numero_total, SocketFlags.None);

            return Produto.loadFromBytes(data);
        }

        public Produto RecebeProduto2()
        {
            int id, disponibilidade;
            string tipo, nome, detalhes;
            float preco;

            byte[] dataNumerica = new byte[4];
            byte[] dataString;
            byte[] dataFloat;
            int tamanhoString,tamanhoFloat;

            //recebe id
            Master.Receive(dataNumerica, 4, SocketFlags.None);
            id = BitConverter.ToInt32(dataNumerica, 0);

            //recebe tipo
            Master.Receive(dataNumerica, 4, SocketFlags.None);
            tamanhoString = BitConverter.ToInt32(dataNumerica, 0);
            dataString = new byte[tamanhoString];
            Master.Receive(dataString, tamanhoString, SocketFlags.None);
            tipo = Encoding.UTF8.GetString(dataString);

            //recebe nome
            Master.Receive(dataNumerica, 4, SocketFlags.None);
            tamanhoString = BitConverter.ToInt32(dataNumerica, 0);
            dataString = new byte[tamanhoString];
            Master.Receive(dataString, tamanhoString, SocketFlags.None);
            nome = Encoding.UTF8.GetString(dataString);

            //recebe detalhes
            Master.Receive(dataNumerica, 4, SocketFlags.None);
            tamanhoString = BitConverter.ToInt32(dataNumerica, 0);
            dataString = new byte[tamanhoString];
            Master.Receive(dataString, tamanhoString, SocketFlags.None);
            detalhes = Encoding.UTF8.GetString(dataString);

            //recebe disponibilidade
            Master.Receive(dataNumerica, 4, SocketFlags.None);
            disponibilidade = BitConverter.ToInt32(dataNumerica, 0);

            //recebe preco
            Master.Receive(dataNumerica, 4, SocketFlags.None);
            tamanhoFloat = BitConverter.ToInt32(dataNumerica, 0);
            dataFloat = new byte[tamanhoFloat];
            Master.Receive(dataFloat, tamanhoFloat, SocketFlags.None);
            preco = BitConverter.ToSingle(dataFloat, 0);

            //recebe imagem --- por completar

            return new Produto(id,tipo,nome,detalhes,disponibilidade,preco);
        }

        public Dictionary<string, List<Produto>> VerProdutos()
        {
            Dictionary<string, List<Produto>> dic = new Dictionary<string, List<Produto>>();

            //envia o id da operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(1);
            Master.Send(id);


            byte[] tamT = new byte[4];
            Master.Receive(tamT, 0, 4, SocketFlags.None);
            int numTipos = BitConverter.ToInt32(tamT, 0);

            for (int i = 0; i < numTipos; i++)
            {

                //recebe o tamanho da string categoria
                Master.Receive(tamT, 4, SocketFlags.None);
                int tamanho = BitConverter.ToInt32(tamT, 0);

                //recebe os bytes da string
                byte[] nome = new byte[tamanho];
                Master.Receive(nome,tamanho,SocketFlags.None);
                string nomeCategoria = Encoding.UTF8.GetString(nome);

                byte[] tamN = new byte[4];
                Master.Receive(tamN, 0, 4, SocketFlags.None);
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

            Master.Receive(data, 0, 4, SocketFlags.None); // 4bytes ->1 int que � o tamanho de bytes a recebr
            int numero_total = BitConverter.ToInt32(data, 0);
            /*
            while (readBytes != 0 && numero_total - 1 > posicao)
            {
                readBytes = Master.Receive(data, posicao, size - posicao, SocketFlags.None);
                posicao += readBytes;
                if (posicao >= size - 1)
                {
                    System.Array.Resize(ref data, size * 2);
                    size *= 2;
                }

            }*/
            data = new byte[numero_total];
            Master.Receive(data, numero_total, SocketFlags.None);

            return Pedido.loadFromBytes(data);
        }

        public List<Pedido> PedidosAnteriores(string idCliente)
        {
            List<Pedido> anteriores = new List<Pedido>();

            //id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(2);
            Master.Send(id);

            //id do cliente
            byte[] msg = new byte[512];
            msg = Encoding.ASCII.GetBytes(idCliente);
            Master.Send(msg);

            //recebe o numero de pedidos da lista
            Master.Receive(id, 0, 4, SocketFlags.None);
            int numPedidos = BitConverter.ToInt32(id, 0);

            //recebe os pedidos todos
            for (int i = 0; i < numPedidos; i++)
            {
                anteriores.Add(RecebePedido());
            }

            return anteriores;
        }

        public Boolean AlterarPedido(int a, int idPedido, String produtos)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(3);
            Master.Send(id);

            //envia id do pedido
            byte[] idPed = new byte[8];
            idPed = BitConverter.GetBytes(idPedido);
            Master.Send(idPed);

            //envia 0 ou 1 para adicionar ou remover produtos respetivamente
            byte[] opcao = new byte[4];
            opcao = BitConverter.GetBytes(a);
            Master.Send(opcao);

            //envia string com todos os produtos
            byte[] msg = new byte[512];
            msg = Encoding.ASCII.GetBytes(produtos);
            Master.Send(msg);

            return true;
        }

        public List<int> NoUltimoPedido()
        {
            List<int> r = new List<int>();

            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(5);
            Master.Send(id);

            byte[] num = new byte[4];
            Master.Receive(num, 4, SocketFlags.None);
            int x = BitConverter.ToInt32(num,0);

            num = new byte[4];
            Master.Receive(num, 4, SocketFlags.None);
            int y = BitConverter.ToInt32(num,0);

            r.Add(x);
            r.Add(y);

            Console.WriteLine("Numero ultimo pedido" + x + "e o outro " + y);
            return r;
        }

        public bool AdicionarAosFavoritos(int idProduto)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(6);
            Master.Send(id);

            //envia idProduto
            id = BitConverter.GetBytes(idProduto);
            Master.Send(id);

            //envia idCliente
            byte[] msg = new byte[512]; //falta por o email dele aqui
            msg = Encoding.ASCII.GetBytes("aaa");
            Master.Send(msg);

            //recebe confirmacao se o produto foi adicionado com sucesso ou nao
            Master.Receive(id, 0, 4, SocketFlags.None);
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
            Master.Send(id);
            EnviaPedido(p);
            return 1;
        }


        //+InfoEmpresa() : Lista String


        //+AvaliarProduto(idProduto : int, idCliente : int, nota : int) : void


        public bool IniciarSessao(string email, string password)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(9);
            Master.Send(id);

            byte[] msg = new byte[512];
            string email_pw = email + "|" + password + "|";
            msg = Encoding.ASCII.GetBytes(email_pw);
            Master.Send(msg);

            byte[] log = new byte[30];
            Master.Receive(log);
            bool login = BitConverter.ToBoolean(log, 0);

            if (login)
            {
                Console.WriteLine("i'm in you crazy bastard");
                EmailCliente = email;
                return true;
            }
            else
            {
                Console.WriteLine("we will get em next time");
                return false;
            }
        }


        public void TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(10);
            Master.Send(id);
        }


        public bool Reclamacao(int idPedido, string motivo, string reclamacao)
        {
            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(12);
            Master.Send(id);

            //envia id pedido
            id = new byte[4];
            id = BitConverter.GetBytes(idPedido);
            Master.Send(id);

            //envia tamanho motivo
            int tamanhoString = motivo.Length;
            id = BitConverter.GetBytes(tamanhoString);
            Master.Send(id);
            //envia motivo
            byte[] msg = new byte[tamanhoString];
            msg = Encoding.ASCII.GetBytes(motivo);
            Master.Send(msg);

            //envia tamanho reclamacao
            tamanhoString = reclamacao.Length;
            id = BitConverter.GetBytes(tamanhoString);
            Master.Send(id);
            //envia reclamacao
            msg = new byte[tamanhoString];
            msg = Encoding.ASCII.GetBytes(reclamacao);
            Master.Send(msg);

            //recebe confirmacao se a reclamacao foi feita com sucesso ou nao
            Master.Receive(id, 0, 4, SocketFlags.None);
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

        public void EnviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            Master.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
            Master.Send(pedido);
        }

    }
}

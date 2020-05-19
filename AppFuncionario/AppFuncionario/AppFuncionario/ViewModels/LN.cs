using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AppFuncionario
{
    public class LN
    {
        private List<Pedido> preparados;
        private List<Pedido> em_preparacao;
        private List<Pedido> por_preparar;

        private Socket master;
        private Socket socket_thread_pedidos;

        private string idFuncionario;

        private Dictionary<string, List<Produto>> produtos;
        private Dictionary<int, Produto> produtosMAP;
        private ObservableCollection<Categoria> categorias;

        private Dictionary<int, Pedido> pedidosMAP;
        private ObservableCollection<PedidoInfo> peidos;


        public LN()
        {
            socket_thread_pedidos = null;
            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("192.168.1.69"), 12345);
            
            try
            {
                master.Connect(ipe);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            pedidosMAP = new Dictionary<int, Pedido>();
            peidos = new ObservableCollection<PedidoInfo>();
        }


        public bool iniciarSessao(string email, string password)
        {
            byte[] num = new byte[4], msg;

            num = BitConverter.GetBytes(1);
            master.Send(num);

            //envia numero bytes email
            num = BitConverter.GetBytes(email.Length);
            master.Send(num);
            //envia o email
            msg = new byte[email.Length];
            msg = Encoding.UTF8.GetBytes(email);
            master.Send(msg, email.Length, SocketFlags.None);

            //envia numero bytes password
            num = BitConverter.GetBytes(password.Length);
            master.Send(num);
            //envia a password
            msg = new byte[password.Length];
            msg = Encoding.UTF8.GetBytes(password);
            master.Send(msg, password.Length, SocketFlags.None);

            master.Receive(num, 4, SocketFlags.None);
            int ret = BitConverter.ToInt32(num, 0);

            if (ret == 1)
            {
                this.idFuncionario = email;
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool alternarEstadoSistema()
        {
            byte[] num = new byte[4];

            num = BitConverter.GetBytes(2);
            master.Send(num);

            byte[] log = new byte[30];
            master.Receive(log);
            bool val = BitConverter.ToBoolean(log, 0);

            return val;
        }

        public Pedido RecebePedido()
        {
            int posicao = 0;
            int size = 100;
            byte[] data = new byte[size];
            int readBytes = -1;

            master.Receive(data, 0, 4, SocketFlags.None);
            int numero_total = BitConverter.ToInt32(data, 0);

            while (readBytes != 0 && numero_total - 1 > posicao)
            {
                readBytes = master.Receive(data, posicao, size - posicao, SocketFlags.None);
                posicao += readBytes;
                if (posicao >= size - 1)
                {
                    System.Array.Resize(ref data, size * 2);
                    size *= 2;
                }
            }

            return Pedido.loadFromBytes(data);
        }


        public Pedido visualizarPedido(int idPedido)
        {
            byte[] num = new byte[4];

            num = BitConverter.GetBytes(3);
            master.Send(num);

            num = BitConverter.GetBytes(idPedido);
            master.Send(num);

            return RecebePedido();
        }


        public bool mudarEstadoPedido(int idPedido)
        {
            byte[] num = new byte[4];

            num = BitConverter.GetBytes(4);
            master.Send(num);

            num = BitConverter.GetBytes(idPedido);
            master.Send(num);

            byte[] res = new byte[1];
            master.Receive(res, 1, SocketFlags.None);
            bool b = BitConverter.ToBoolean(res, 0);

            return b;
        }


        public void notificarCliente(string idCliente, string mensagem)
        {
            byte[] num = new byte[4], msg;

            num = BitConverter.GetBytes(5);
            master.Send(num);

            //envia numero bytes id cliente
            num = BitConverter.GetBytes(idCliente.Length);
            master.Send(num);
            //envia a string
            msg = new byte[idCliente.Length];
            msg = Encoding.UTF8.GetBytes(idCliente);
            master.Send(msg, idCliente.Length, SocketFlags.None);

            //envia numero bytes mensagem
            num = BitConverter.GetBytes(mensagem.Length);
            master.Send(num);
            //envia a string
            msg = new byte[mensagem.Length];
            msg = Encoding.UTF8.GetBytes(mensagem);
            master.Send(msg, mensagem.Length, SocketFlags.None);
        }

        public bool TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(6);
            master.Send(id);
            return false;
        }

        public void carregaProdutos()
        {
            produtos = new Dictionary<string, List<Produto>>();
            produtosMAP = new Dictionary<int, Produto>();

            //envia o id da operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(7);
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
                    if (produtos.ContainsKey(nomeCategoria))
                    {
                        Produto p = RecebeProdutoManual();
                        List<Produto> lps = produtos[nomeCategoria];
                        lps.Add(p);
                        produtosMAP.Add(p.id, p);
                    }
                    else
                    {
                        Produto p = RecebeProdutoManual();
                        List<Produto> lp = new List<Produto>();
                        lp.Add(p);
                        produtos.Add(nomeCategoria, lp);
                        produtosMAP.Add(p.id, p);
                    }
                }

            }
            
        }

        public Produto RecebeProdutoManual()
        {
            byte[] data = new byte[4];

            //recebe id
            master.Receive(data, 4, SocketFlags.None);
            int id = BitConverter.ToInt32(data, 0);

            //recebe tipo
            master.Receive(data, 4, SocketFlags.None);
            int numbytes = BitConverter.ToInt32(data, 0);
            byte[] str = new byte[numbytes];
            master.Receive(str, numbytes, SocketFlags.None);
            string tipo = Encoding.UTF8.GetString(str);

            //recebe nome
            master.Receive(data, 4, SocketFlags.None);
            numbytes = BitConverter.ToInt32(data, 0);
            str = new byte[numbytes];
            master.Receive(str, numbytes, SocketFlags.None);
            string nome = Encoding.UTF8.GetString(str);

            //recebe detalhes
            master.Receive(data, 4, SocketFlags.None);
            numbytes = BitConverter.ToInt32(data, 0);
            str = new byte[numbytes];
            master.Receive(str, numbytes, SocketFlags.None);
            string detalhes = Encoding.UTF8.GetString(str);

            //recebe disponibilidade
            master.Receive(data, 4, SocketFlags.None);
            int disponibilidade = BitConverter.ToInt32(data, 0);

            //recebe preco
            master.Receive(data, 4, SocketFlags.None);
            int tamanhofloat = BitConverter.ToInt32(data, 0);
            data = new byte[tamanhofloat];
            master.Receive(data, tamanhofloat, SocketFlags.None);
            float preco = BitConverter.ToSingle(data, 0);

            return new Produto(id, tipo, nome, detalhes, disponibilidade, preco, null);
        }

        public void preencheCategorias()
        {
            categorias = new ObservableCollection<Categoria>();
            string[] a = new string[produtos.Keys.Count];
            produtos.Keys.CopyTo(a, 0);
            for (int i = 0; i < produtos.Keys.Count; i++)
            {
                categorias.Add(new Categoria(a[i], produtos[a[i]][0].imagem, "" + produtos[a[i]].Count + " Produtos"));
            }
        }

        public Produto getProduto(int id)
        {
            return produtosMAP[id];
        }

        public void inicializaSocketandThreadPedidos()
        {
            master.Send(BitConverter.GetBytes(8), 4, SocketFlags.None);

            byte[] numero = new byte[4];
            master.Receive(numero, 4, SocketFlags.None);
            BitConverter.ToInt32(numero, 0);

            Socket ret = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("192.168.1.69"), 12350);
            try
            {
                ret.Connect(ipe);                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            ThreadPedidosHandlers hello = new ThreadPedidosHandlers(ret, produtosMAP, pedidosMAP, categorias, peidos);
            Thread goodbie = new Thread(hello.run);
            goodbie.Start();

            this.socket_thread_pedidos = ret;
        }

        public void muda_estado_pedido(PedidoInfo pi)
        {
            if(this.socket_thread_pedidos != null)
            {

                this.socket_thread_pedidos.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

                switch (pi.estado)
                {
                    case "Por Preparar":
                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(0), 4, SocketFlags.None);

                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(pi.idPedido), 4, SocketFlags.None);

                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(this.pedidosMAP[pi.idPedido].data_hora.ToBinary()), 8, SocketFlags.None);
                        pi.estado = "Em Preparação";
                        break;

                    case "Em Preparação":
                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(pi.idPedido), 4, SocketFlags.None);

                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(this.pedidosMAP[pi.idPedido].data_hora.ToBinary()), 8, SocketFlags.None);
                        pi.estado = "Pronto a Levantar";
                        break;
                    
                }
            }
        }

        public Pedido GetPedido(int id)
        {
            return this.pedidosMAP[id];
        }

        public ObservableCollection<PedidoInfo> getObCPedidos() 
        {
            return this.peidos;
        }

        public ObservableCollection<ProdutoInfo> getProdutos_Pedido(PedidoInfo p)
        {
            List<ProdutoPedido> produtoPedidos = pedidosMAP[p.idPedido].produtos;
            ObservableCollection<ProdutoInfo> ret = new ObservableCollection<ProdutoInfo>();
            for(int i = 0; i < produtoPedidos.Count; i++)
            {
                ret.Add(new ProdutoInfo(produtoPedidos[i].p.id, produtoPedidos[i].p.nome, "" + produtoPedidos[i].p.preco + "€", produtoPedidos[i].quantidades, null));
            }
            return ret;
        }
    }
}

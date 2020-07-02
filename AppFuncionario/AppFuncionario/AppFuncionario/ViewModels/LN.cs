using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace AppFuncionario
{
    /** 
    * @brief Classe utilizada para a AppFuncionario comunicar com o servidor. 
    */
    public class LN
    {
        private Socket master;
        private Socket socket_thread_pedidos;
        private ThreadPedidosHandlers tph;

        private string idFuncionario;

        private Dictionary<string, List<Produto>> produtos;
        private Dictionary<int, Produto> produtosMAP;
        private ObservableCollection<Categoria> categorias;

        private Dictionary<int, Pedido> pedidosMAP;
        private ObservableCollection<PedidoInfo> peidos;

        /** 
        * Construtor por defeito, estabelece a comunicação com o servidor e cria as estruturas 
        */
        public LN()
        {
            socket_thread_pedidos = null;
            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("2.83.152.80"), 12345);
            
            try
            {
                master.Connect(ipe);
                master.Blocking = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            pedidosMAP = new Dictionary<int, Pedido>();
            peidos = new ObservableCollection<PedidoInfo>();
        }

        /** 
        * Função utilizada para o funcionário iniciar sessão 
        * @param email introduzido a ser validado pelo servidor
        * @param password introduzido a ser validado pelo servidor
        */
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

        /** 
        * Função utilizada para o funcionário alterar o estado do sistema (iniciar/desligar) 
        */
        public bool alternarEstadoSistema()
        {
            master.Send(BitConverter.GetBytes(2));

            byte[] log = new byte[1];
            master.Receive(log,1,SocketFlags.None);

            if (log[0] == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /** 
        * Função utilizada para o funcionário terminar sessão 
        */
        public bool TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(6);
            master.Send(id);
            return false;
        }

        /** 
        * Função utilizada pela aplicação para armazenar nas estruturas criadas os produtos registados 
        */
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

        /** 
        * Função utilizada para receber um determinado produto do servidor garantido que a informação encontra-se correcta e ordenada 
        */
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

        /** 
        * Função utilizada para preencher a estrutura dinamica com as categorias 
        */
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

        /** 
        * Função utilizada para obter um produto da estrutura que possui todos os produtos 
        */
        public Produto getProduto(int id)
        {
            return produtosMAP[id];
        }

        /** 
        * Função utilizada para lançar a thread que recebe os pedidos 
        */
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
            this.tph = hello;
        }

        /** 
        * Função utilizada para estabelecer o estado de um pedido 
        * @param l Label a alterar 
        */
        public void label_para_num_pedido(Label l)
        {
            this.tph.setLabel(l);
        }

        /** 
        * Função utilizada para alterar um estado (Por Preparar > Em Preparação > Pronto a entregar > Pedido Entregue) de um determinado pedido 
        * @param pi Pedido(Info) que queremos alterar o estado
        */
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

                        master.Send(BitConverter.GetBytes(11));

                        byte[] str = Encoding.UTF8.GetBytes(pi.idClienteNotfs);
                        master.Send(BitConverter.GetBytes(str.Length), 4, SocketFlags.None);
                        master.Send(str, str.Length, SocketFlags.None);

                        master.Send(BitConverter.GetBytes(pi.idPedido), 4, SocketFlags.None);

                        break;
                    case "Pronto a Levantar":
                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(2), 4, SocketFlags.None);
                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(pi.idPedido), 4, SocketFlags.None);
                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(this.pedidosMAP[pi.idPedido].data_hora.ToBinary()), 8, SocketFlags.None);

                        byte[] msg = Encoding.UTF8.GetBytes(idFuncionario);
                        this.socket_thread_pedidos.Send(BitConverter.GetBytes(msg.Length),4,SocketFlags.None);
                        this.socket_thread_pedidos.Send(msg, msg.Length, SocketFlags.None);

                        pi.estado = "Pedido Entregue";

                        break;
                    
                }
            }
        }

        /** 
        * Função utilizada para remover um pedido da lista de pedidos
        * @param p Pedido associado com a informação a remover
        */
        public void removePedido(PedidoInfo p)
        {
            pedidosMAP.Remove(p.idPedido);
            peidos.Remove(p);
        }

        /** 
        * Função utilizada para obter um pedido da lista de pedidos
        * @param id identificador do pedido a obter
        */
        public Pedido GetPedido(int id)
        {
            return this.pedidosMAP[id];
        }

        /** 
        * Função utilizada para obter a estrutura dinámica de pedidos 
        */
        public ObservableCollection<PedidoInfo> getObCPedidos() 
        {
            return this.peidos;
        }

        /** 
        * Função utilizada para obter a estrutura dinámica de produtos com informação
        * @param p Pedido com a sua informação para obter os produtos que fazem parte dele 
        */
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

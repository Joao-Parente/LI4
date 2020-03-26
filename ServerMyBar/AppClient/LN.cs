using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AppClient
{
    public class LN
    {
        private Dictionary<string, List<Produto>> produtos;
        private List<Produto> favoritos;
        private List<Pedido> historico;
        private Socket master;
        private string email_idCliente;

        public LN()
        {
            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12344);
            try
            {
                master.Connect(ipe);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
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

            byte[] id = new byte[4];
            id = BitConverter.GetBytes(1);
            master.Send(id);

            byte[] tamT = new byte[4];
            master.Receive(tamT, 0, 4, SocketFlags.None);
            int numTipos = BitConverter.ToInt32(tamT, 0);

            for (int i = 0; i < numTipos; i++)
            {
                byte[] nome = new byte[512];
                master.Receive(nome, 0, 512, SocketFlags.None);
                string nom = BitConverter.ToString(nome, 0);

                byte[] tamN = new byte[4];
                master.Receive(tamN, 0, 4, SocketFlags.None);
                int numNom = BitConverter.ToInt32(tamN, 0);

                for (int j = 0; j < numNom; j++)
                {
                    if (dic.ContainsKey(nom))
                    {
                        Produto p = RecebeProduto();
                        List<Produto> lp = new List<Produto>();
                        dic.TryGetValue(nom, out lp);
                        lp.Add(p);
                        dic.Add(nom, lp);
                    }
                    else
                    {
                        Produto p = RecebeProduto();
                        List<Produto> lp = new List<Produto>();
                        lp.Add(p);
                        dic.Add(nom, lp);
                    }
                }

            }
            return dic;
        }

        public Pedido RecebePedido()
        {
            int size = 100;
            byte[] data = new byte[size];

            master.Receive(data, 0, 4, SocketFlags.None); // 4bytes ->1 int que é o tamanho de bytes a recebr
            int numero_total = BitConverter.ToInt32(data, 0);
            /*
            while (readBytes != 0 && numero_total - 1 > posicao)
            {
                readBytes = master.Receive(data, posicao, size - posicao, SocketFlags.None);
                posicao += readBytes;
                if (posicao >= size - 1)
                {
                    System.Array.Resize(ref data, size * 2);
                    size *= 2;
                }

            }*/
            data = new byte[numero_total];
            master.Receive(data, numero_total, SocketFlags.None);

            return Pedido.loadFromBytes(data);
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

            //recebe o numero de pedidos da lista
            master.Receive(id, 0, 4, SocketFlags.None);
            int numPedidos = BitConverter.ToInt32(id, 0);

            //recebe os pedidos todos
            for (int i = 0; i < numPedidos; i++)
            {
                anteriores.Add(RecebePedido());
            }

            return anteriores;
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
            int x = BitConverter.ToInt32(num);

            num = new byte[4];
            master.Receive(num, 4, SocketFlags.None);
            int y = BitConverter.ToInt32(num);

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
            master.Send(id);

            //envia idProduto
            id = BitConverter.GetBytes(idProduto);
            master.Send(id);

            //envia idCliente
            byte[] msg = new byte[512]; //falta por o email dele aqui
            msg = Encoding.ASCII.GetBytes("aaa");
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


        //+InfoEmpresa() : Lista String


        //+AvaliarProduto(idProduto : int, idCliente : int, nota : int) : void


        public void IniciarSessao(string email, string password)
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
            }
            else Console.WriteLine("we will get em next time");
        }


        public void TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(10);
            master.Send(id);
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
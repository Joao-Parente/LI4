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

            if (login) Console.WriteLine("Sign me in");
            else Console.WriteLine("Dont mess with the system");


            return login;
        }

        public Dictionary<string,List<Produto>> verProdutos()
        {
            Dictionary<string, List<Produto>> dic = new Dictionary<string, List<Produto>>();
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(1);
            master.Send(id);



            return dic;
        }

        // +VerPedidosAnteriores() : Lista Pedidos

        // +AlterarPedido(idPedido : int, produtos : Lista Produtos) : boolean
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

            r.Add(x); r.Add(y);

            Console.WriteLine("Numero ultimo pedido" + x + "e o outro " + y);


            return r;
        }


        //+AdicionarAosFavoritos(idProduto : int) : void


        public int EfetuarPedido(Pedido p)
        {

            //envia id operacao
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(4);
            master.Send(id);

            enviaPedido(p);
            return 1;
        }


        // +InfoEmpresa() : Lista String

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

            if (login) Console.WriteLine("i'm in you crazy bastard");
            else Console.WriteLine("we will get em next time");

        }


        public void TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(10);
            master.Send(id);
            
        }

        //+reclamacao(idCliente : int, idPedido : int, comentario : string) : boolean






        public void enviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();

            master.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
            master.Send(pedido);

        }

    }

}
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AppCliente
{
    public class AppClienteLN
    {
        private Dictionary<string, List<Produto>> Produtos;
        private List<Produto> Favoritos;
        private List<Pedido> Historico;
        private Socket Master;

        public AppClienteLN()
        {
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

            if (login) Console.WriteLine("Sign me in");
            else Console.WriteLine("Dont mess with the system");

            return login;
        }

        public Dictionary<string, List<Produto>> VerProdutos()
        {
            Dictionary<string, List<Produto>> dic = new Dictionary<string, List<Produto>>();
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(1);
            Master.Send(id);

            return dic;
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

            r.Add(x); r.Add(y);

            Console.WriteLine("Numero ultimo pedido" + x + "e o outro " + y);

            return r;
        }

        public void EnviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            Master.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
            Master.Send(pedido);
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

        public Boolean IniciarSessao(string email, string password)
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

            if (login) Console.WriteLine("i'm in you crazy bastard");
            else Console.WriteLine("we will get em next time");

            return login;
        }


        public void TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(10);
            Master.Send(id);
        }

    }
}

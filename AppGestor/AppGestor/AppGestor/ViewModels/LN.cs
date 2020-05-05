using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AppGestor
{
    public class LN
    {
        private Dictionary<int, Empregado> empregados;
        private Dictionary<int, Reclamacao> reclamacoes;
        private List<Pedido> preparados;
        private List<Pedido> em_preparacao;
        private List<Pedido> por_preparar;

        private Socket master;

        private string idGestor;

        private Dictionary<string, List<Produto>> produtos;
        private Dictionary<int, Produto> produtosMAP;
        private ObservableCollection<Categoria> categorias;

        public LN()
        {
            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("192.168.1.69"), 12346);

            try
            {
                master.Connect(ipe);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
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
                this.idGestor = email;
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

        public int adicionarProduto(Produto p)
        {
            byte[] id = new byte[4];

            id = BitConverter.GetBytes(6);
            master.Send(id);

            byte[] emp = p.SavetoBytes();
            master.Send(BitConverter.GetBytes(emp.Length));
            master.Send(emp);

            byte[] data = new byte[4];
            master.Receive(data, 4, SocketFlags.None);
            int idP = BitConverter.ToInt32(data, 0);

            return idP;
        }


        public bool editarProduto(Produto p)
        {
            byte[] num = new byte[4];

            num = BitConverter.GetBytes(7);
            master.Send(num);

            EnviaProdutoManual(p);

            master.Receive(num, 4, SocketFlags.None);
            int res = BitConverter.ToInt32(num, 0);

            if (res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool removerProduto(int idr)
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(8);
            master.Send(id);

            id = BitConverter.GetBytes(idr);
            master.Send(id);

            byte[] log = new byte[30];
            master.Receive(log);
            bool val = BitConverter.ToBoolean(log, 0);

            return val;
        }


        public bool adicionarEmpregado(Empregado e)
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(9);
            master.Send(id);

            byte[] emp = e.SavetoBytes();
            master.Send(BitConverter.GetBytes(emp.Length));
            master.Send(emp);

            byte[] num = new byte[4];
            master.Receive(num, 4, SocketFlags.None);
            int res = BitConverter.ToInt32(num, 0);

            if (res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool editarEmpregado(string email, Empregado e)
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(10);
            master.Send(id);

            byte[] msg = new byte[512];
            msg = Encoding.ASCII.GetBytes(email);
            master.Send(msg);

            byte[] emp = e.SavetoBytes();
            master.Send(BitConverter.GetBytes(emp.Length));
            master.Send(emp);

            byte[] log = new byte[30];
            master.Receive(log);
            bool val = BitConverter.ToBoolean(log, 0);

            return val;
        }


        public bool removerEmpregado(string email)
        {
            byte[] id = new byte[4];

            id = BitConverter.GetBytes(11);
            master.Send(id);

            byte[] msg = new byte[512];
            msg = Encoding.ASCII.GetBytes(email);
            master.Send(msg);

            byte[] log = new byte[30];
            master.Receive(log);
            bool val = BitConverter.ToBoolean(log, 0);

            return val;
        }


        // +alterarInfoEmpresa(novaInfo : lista string) : void


        public List<Pedido> consultasEstatisticas(DateTime i, DateTime f)
        {
            byte[] num = new byte[4], msg;

            num = BitConverter.GetBytes(13);
            master.Send(num);

            msg = BitConverter.GetBytes(i.ToBinary());
            master.Send(msg);

            msg = BitConverter.GetBytes(f.ToBinary());
            master.Send(msg);


            return RecebePedidosManual();
        }

        public bool TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(14);
            master.Send(id);
            return false;
        }


        public List<Pedido> RecebePedidosManual()
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
                tamEmpregado = BitConverter.ToInt32(num, 0);
                str = new byte[tamEmpregado];
                master.Receive(str, tamEmpregado, SocketFlags.None);
                string idCliente = Encoding.UTF8.GetString(str);

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


        public void EnviaProdutoManual(Produto p)
        {
            //envia o id do produto
            byte[] dataNum = new byte[4];
            dataNum = BitConverter.GetBytes(p.id);
            master.Send(dataNum, 4, SocketFlags.None);

            byte[] dataString;
            //envia o num de bytes do tipo
            dataNum = BitConverter.GetBytes(p.tipo.Length);
            master.Send(dataNum, 4, SocketFlags.None);
            //envia os bytes do tipo
            dataString = Encoding.UTF8.GetBytes(p.tipo);
            master.Send(dataString, dataString.Length, SocketFlags.None);

            //envia o num de bytes do nome
            dataNum = BitConverter.GetBytes(p.nome.Length);
            master.Send(dataNum, 4, SocketFlags.None);
            //envia os bytes do nome
            dataString = Encoding.UTF8.GetBytes(p.nome);
            master.Send(dataString, dataString.Length, SocketFlags.None);

            //envia o num de bytes do detalhes
            dataNum = BitConverter.GetBytes(p.detalhes.Length);
            master.Send(dataNum, 4, SocketFlags.None);
            //envia os bytes do detalhes
            dataString = Encoding.UTF8.GetBytes(p.detalhes);
            master.Send(dataString, dataString.Length, SocketFlags.None);

            //envia a disponibilidade
            dataNum = BitConverter.GetBytes(p.disponibilidade);
            master.Send(dataNum, 4, SocketFlags.None);

            //envia o preco
            byte[] dataFloat = BitConverter.GetBytes(p.preco);
            dataNum = BitConverter.GetBytes(dataFloat.Length);
            master.Send(dataNum, 4, SocketFlags.None);//envia num bytes
            master.Send(dataFloat, dataFloat.Length, SocketFlags.None);//envia os bytes

            //envia imagem --- por completar
        }


        public void enviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            master.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
            master.Send(pedido);
        }


        public Pedido RecebePedido()
        {
            int posicao = 0;
            int size = 100;
            byte[] data = new byte[size];
            int readBytes = -1;
            master.Receive(data, 0, 4, SocketFlags.None); // 4bytes ->1 int que é o tamanho de bytes a recebr
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
    }
}
using System;
using System.Collections.Generic;
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

        //+visualizarPedido(idPedido : int) : Pedido

        //+alternarEstadoSistema() : void

        //+notificarClientes(idCliente : int, mensagem : string) : void

        //+mudarEstadoPedido(idPedido : int) : void

        public int adicionarProduto(Produto p)
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(5);
            master.Send(id);

            byte[] emp = p.SavetoBytes();
            master.Send(BitConverter.GetBytes(emp.Length));
            master.Send(emp);

            byte[] data = new byte[4];
            master.Receive(data, 4, SocketFlags.None);
            int idP = BitConverter.ToInt32(data, 0);

            return idP;
        }

        //+editarProduto(idProduto : int, novoProduto : Produto) : void

        //+consultasEstatisticas() : lista string

        //+alterarInfoEmpresa(novaInfo : lista string) : void

        //+adicionarEmpregado(idEmpregado : int) : void

        //+removerEmpregado(idEmpregado : int) : void

        //+IniciarSessao(email : string, password : string) : void

        public bool TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(10);
            master.Send(id);
            return false;
        }

        public bool editarEmpregado(string email, Empregado e)
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(11);
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
            id = BitConverter.GetBytes(12);
            master.Send(id);

            byte[] msg = new byte[512];
            msg = Encoding.ASCII.GetBytes(email);
            master.Send(msg);

            byte[] log = new byte[30];
            master.Receive(log);
            bool val = BitConverter.ToBoolean(log, 0);

            return val;
        }
    }
}
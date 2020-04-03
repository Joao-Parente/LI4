using System;
using System.Collections.Generic;
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

        public LN(Socket s)
        {
            master = s;
        }


        //+visualizarPedido(idPedido : int) : Pedido

        //+alternarEstadoSistema() : void

        //+mudarEstadoPedido(idPedido : int) : void

        //+adicionarProduto(produto : Produto) : int

        //+editarProduto(idProduto : int, novoProduto : Produto) : void

        public bool editarProduto(Produto p)
        {
            byte[] num = new byte[4];
            //envia id operacao
            num = BitConverter.GetBytes(6);
            master.Send(num);

            EnviaProdutoManual(p);

            master.Receive(num,4,SocketFlags.None);
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

        //+consultasEstatisticas() : lista string
        public List<Pedido> consultasEstatisticas(DateTime i,DateTime f)
        {
            byte[] num = new byte[4], msg;
            //envia id operacao
            num = BitConverter.GetBytes(7);
            master.Send(num);

            msg = BitConverter.GetBytes(i.ToBinary());
            master.Send(msg);

            msg = BitConverter.GetBytes(f.ToBinary());
            master.Send(msg);


            return RecebePedidosManual();            
        }

        //+alterarInfoEmpresa(novaInfo : lista string) : void

        //+adicionarEmpregado(idEmpregado : int) : void

        //+removerEmpregado(idEmpregado : int) : void

        //+IniciarSessao(email : string, password : string) : void

        public bool iniciarSessao(string email, string password)
        {
            byte[] num = new byte[4], msg;
            //envia id operacao
            num = BitConverter.GetBytes(9);
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
                return true;
            }
            else
            {
                return false;
            }
        }

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

        //+TerminarSessao()

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

    }

}
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace AppFunc
{
    public class LN
    {
        private List<Pedido> preparados;

        private List<Pedido> em_preparacao;

        private List<Pedido> por_preparar;

        private Socket master;


        public LN(Socket s)
        {
            this.master = s;
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
    }
}
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
            master = s;
        }



        //+visualizarPedido(idPedido : int) : Diagrama Classes Funcionario.Pedido

        //+mudarEstadoPedido(idPedido : int) : void

        //+alternarEstadoDoSistema() : void

        //+notificarClientes(idCliente : int, mensagem : string)

        public void notificarCliente(string idCliente, string mensagem) //goncalo, eu nuno, fiz este porque era suposto fazer o notificar clientes da parte do gestor, e depois vi que aquilo nao fazia sentido nenhum
        {                                                               // ok, eu ggg,
            byte[] num = new byte[4], msg;
            //envia id operacao
            num = BitConverter.GetBytes(4);
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

        public bool iniciarSessao(string email, string password)
        {
            byte[] num = new byte[4], msg;
            //envia id operacao
            num = BitConverter.GetBytes(2);
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

        public bool TerminarSessao()
        {
            byte[] id = new byte[4];
            id = BitConverter.GetBytes(10);
            master.Send(id);
            return false;
        }
    }
}
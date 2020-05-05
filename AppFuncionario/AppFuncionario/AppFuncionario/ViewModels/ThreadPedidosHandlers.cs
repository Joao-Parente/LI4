using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;

namespace AppFuncionario
{
    public class ThreadPedidosHandlers
    {
        private Socket socket;

        private ObservableCollection<PedidoInfo> pdsObsC;
        private Dictionary<int, Pedido> pedidosMAP;

        private Dictionary<int, Produto> produtosMAP;
        private ObservableCollection<Categoria> categorias;

        public ThreadPedidosHandlers(Socket s, Dictionary<int, Produto> prds, Dictionary<int, Pedido> pdsMAP, ObservableCollection<Categoria> cats,ObservableCollection<PedidoInfo> obscPeds)
        {
            socket = s;
            produtosMAP = prds;
            pdsObsC = obscPeds;
            pedidosMAP = pdsMAP;
            categorias = cats;
        }

        public void run()
        {
            byte[] data = new byte[512];
            bool flag = true;

            while (flag) 
            {
                socket.Receive(data, 4, SocketFlags.None);
                int numero = BitConverter.ToInt32(data, 0);

                switch (numero)
                {
                    case 1: //recebe um pedido
                        byte[] num=new byte[4], str;
                        int size;

                        //recebe o id
                        socket.Receive(num, 4, SocketFlags.None);
                        int id = BitConverter.ToInt32(num, 0);

                        //recebe o id cliente
                        socket.Receive(num, 4, SocketFlags.None);
                        size = BitConverter.ToInt32(num, 0);
                        str = new byte[size];
                        socket.Receive(str, size, SocketFlags.None);
                        string idCliente = Encoding.UTF8.GetString(str);

                        //recebe detalhes
                        socket.Receive(num, 4, SocketFlags.None);
                        size = BitConverter.ToInt32(num, 0);
                        str = new byte[size];
                        socket.Receive(str, size, SocketFlags.None);
                        string detalhes = Encoding.UTF8.GetString(str);

                        //recebe datahora
                        str = new byte[8];
                        socket.Receive(str, 8, SocketFlags.None);
                        DateTime dt = DateTime.FromBinary(BitConverter.ToInt64(str,0));

                        //recebe num produtos
                        socket.Receive(num, 4, SocketFlags.None);
                        int numprodutos = BitConverter.ToInt32(num, 0);

                        List<ProdutoPedido> lpp = new List<ProdutoPedido>(numprodutos);
                        for(int i = 0; i < numprodutos; i++)
                        {
                            //recebe id produto
                            socket.Receive(num, 4, SocketFlags.None);
                            Produto p = produtosMAP[BitConverter.ToInt32(num, 0)];

                            //recebe quantidade
                            socket.Receive(num, 4, SocketFlags.None);

                            lpp.Add(new ProdutoPedido(p, BitConverter.ToInt32(num, 0)));
                        }

                        Pedido ret = new Pedido(id, idCliente, null, detalhes, dt, lpp);
                        pedidosMAP.Add(id, ret);
                        pdsObsC.Add(new PedidoInfo(ret));

                        break;
                    default:
                        flag = false;
                        break;
                }
            }

        }
    }
}

using System;
using System.Net.Sockets;
using ServerMyBar.comum;
using ServerMyBar.serverCliente;


namespace ServerMyBar.serverGestor
{
    public class ThreadServerGestor
    {
        private Gestor gestor;
        private Socket socket;
        private StarterClient start_client;


        public ThreadServerGestor(Gestor g, Socket s, StarterClient sa)
        {
            gestor = g;
            socket = s;
            start_client = sa;
        }


        public void run()
        {
            Console.WriteLine("Server Thread a Correr!");

            byte[] data = new byte[512];
            bool flag = true;
            int msg;

            while (flag)
            {
                socket.Receive(data, 4, SocketFlags.None);
                msg = BitConverter.ToInt32(data, 0);
                Console.WriteLine("Comando pedido foi o de id : " + msg);

                // 1 VisualizarPedido
                // 2 MudarEstadoPedido
                // 3 AlternarEstadoSistema
                // 4 NotificarCliente
                // 5 AdicionarProduto
                // 6 EditarProduto
                // 7 ConsultasEstatisticas
                // 8 AlterarInfoEmpresa
                // 9 IniciarSessao
                // 10 TerminarSessao

                switch (msg)
                {
                    case 2: // Login
                        Console.WriteLine("Starting authentication" + msg);
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string email_pw = System.Text.Encoding.UTF8.GetString(data);
                        string[] credenciais = email_pw.Split('|');
                        Console.WriteLine("Credencias : '" + credenciais[0] + "'  --- '" + credenciais[1] + "'");
                        if (gestor.loginGestor(credenciais[0], credenciais[1]))
                        {
                            Console.WriteLine("Authentication succeed");
                            socket.Send(BitConverter.GetBytes(true));
                        }
                        else
                        {
                            Console.WriteLine("Authentication failed");
                            socket.Send(BitConverter.GetBytes(false));
                        }
                        break;
                    case 5:
                        Produto p = RecebeProduto();

                        int idP = gestor.addProduto(p);

                        byte[] id = new byte[4];
                        id = BitConverter.GetBytes(idP);
                        socket.Send(id);

                        break;
                    case 10: //TerminarSessao
                        flag = false;
                        break;
                    case 11: //editarEmpregado
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string email1 = System.Text.Encoding.UTF8.GetString(data);

                        Empregado e = RecebeEmpregado();

                        bool res1 = gestor.editEmpregado(email1, e);

                        byte[] resultado1 = new byte[30];
                        resultado1 = BitConverter.GetBytes(res1);
                        socket.Send(resultado1, 30, SocketFlags.None);

                        break;

                    case 12: //removerEmpregado
                        socket.Receive(data, 0, 512, SocketFlags.None);
                        string email2 = System.Text.Encoding.UTF8.GetString(data);

                        bool res2 = gestor.removeEmpregado(email2);

                        byte[] resultado2 = new byte[30];
                        resultado2 = BitConverter.GetBytes(res2);
                        socket.Send(resultado2, 30, SocketFlags.None);

                        break;
                    default:
                        flag = false;
                        break;
                }
                msg = -1;
                data = new byte[512];

            }
            Console.WriteLine("Thread: Terminei o comunicação com o cliente, a desligar.");
        }


        public Pedido RecebePedido()
        {
            int posicao = 0;
            int size = 100;
            byte[] data = new byte[size];
            int readBytes = -1;
            socket.Receive(data, 0, 4, SocketFlags.None); // 4bytes ->1 int que é o tamanho de bytes a recebr
            int numero_total = BitConverter.ToInt32(data, 0);
            while (readBytes != 0 && numero_total - 1 > posicao)
            {
                readBytes = socket.Receive(data, posicao, size - posicao, SocketFlags.None);
                posicao += readBytes;
                if (posicao >= size - 1)
                {
                    System.Array.Resize(ref data, size * 2);
                    size *= 2;
                }
            }
            return Pedido.loadFromBytes(data);
        }

        public Empregado RecebeEmpregado()
        {
            int posicao = 0;
            int size = 100;
            byte[] data = new byte[size];
            int readBytes = -1;
            socket.Receive(data, 0, 4, SocketFlags.None);
            int numero_total = BitConverter.ToInt32(data, 0);
            while (readBytes != 0 && numero_total - 1 > posicao)
            {
                readBytes = socket.Receive(data, posicao, size - posicao, SocketFlags.None);
                posicao += readBytes;
                if (posicao >= size - 1)
                {
                    System.Array.Resize(ref data, size * 2);
                    size *= 2;
                }
            }
            return Empregado.loadFromBytes(data);
        }

        public Produto RecebeProduto()
        {
            int posicao = 0;
            int size = 100;
            byte[] data = new byte[size];
            int readBytes = -1;
            socket.Receive(data, 0, 4, SocketFlags.None);
            int numero_total = BitConverter.ToInt32(data, 0);
            while (readBytes != 0 && numero_total - 1 > posicao)
            {
                readBytes = socket.Receive(data, posicao, size - posicao, SocketFlags.None);
                posicao += readBytes;
                if (posicao >= size - 1)
                {
                    System.Array.Resize(ref data, size * 2);
                    size *= 2;
                }
            }
            return Produto.loadFromBytes(data);
        }
    }
}
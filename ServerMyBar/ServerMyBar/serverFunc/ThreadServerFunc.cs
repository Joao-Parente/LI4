using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerMyBar.comum;
using ServerMyBar.serverCliente;

namespace ServerMyBar.serverFunc
{
    public class ThreadServerFunc
    {
        private Gestor gestor;
        private Socket socket;
        private StarterClient start_client;
        private bool flag_pedidos;
        private TcpListener server;

        public ThreadServerFunc(Gestor g, Socket s, StarterClient sc)
        {
            this.gestor = g;
            this.socket = s;
            socket.Blocking = true;
            this.start_client = sc;
            flag_pedidos = false;
            server = null;
        }

        /**
        * Função em loop que recebe identificadores de comandos do funcionário e responde a eles
        * \n 1 : Login
        * \n 2 : Alterar Estado do Sistema
        * \n 6 : Logout
        * \n 7 : VerProdutos
        * \n 8 : Inicializa servidor dos clientes
        * \n 11 : Notifica um cliente que o seu pedido está pronto
        */
        public void run()
        {
            Console.WriteLine("ThreadServerFunc running...");

            byte[] data = new byte[512];
            bool flag = true;
            int msg;

            while (flag)
            {
                //Receber o ID da operacao
                socket.Receive(data, 4, SocketFlags.None);
                msg = BitConverter.ToInt32(data, 0);
                Console.WriteLine("Command requested was ID : " + msg);

                switch (msg)
                {
                    case 1: //LOGIN                        
                        byte[] numL = new byte[4], msgL;

                        // tamanho do campo email
                        socket.Receive(numL, 0, 4, SocketFlags.None);
                        int sizeL = BitConverter.ToInt32(numL, 0);

                        //campo email
                        msgL = new byte[sizeL];
                        socket.Receive(msgL, sizeL, SocketFlags.None);
                        string emaiL = Encoding.UTF8.GetString(msgL);

                        // tamanho do campo password
                        socket.Receive(numL, 0, 4, SocketFlags.None);
                        sizeL = BitConverter.ToInt32(numL, 0);

                        // campo password 
                        msgL = new byte[sizeL];
                        socket.Receive(msgL, sizeL, SocketFlags.None);
                        string passL = Encoding.UTF8.GetString(msgL);

                        // login
                        if (gestor.loginFunc(emaiL, passL) == true)
                        {
                            numL = BitConverter.GetBytes(1);
                            socket.Send(numL);
                        }
                        else
                        {
                            numL = BitConverter.GetBytes(0);
                            socket.Send(numL);
                        }
                        break;

                    case 2:  // ALTERAR ESTADO DO SISTEMA
                        byte[] resultado10 = new byte[1];

                        if (this.start_client.estado == true)
                        {
                            this.start_client.offCliente();
                            resultado10[0] = 0;
                        }
                        else
                        {
                            this.start_client.onCliente();
                            resultado10[0] = 1;
                        }

                        socket.Send(resultado10, 1, SocketFlags.None);
                        break;

                    case 6: // LOGOUT
                        if (server != null)
                        {
                            server.Stop();
                        }
                        flag = false;
                        break;

                    case 7: //PRODUTOS TODOS

                        Dictionary<string, List<Produto>> map = gestor.VerProdutosFunc();

                        //envia o numero de chaves
                        byte[] tamT = new byte[4];
                        tamT = BitConverter.GetBytes(map.Count);
                        socket.Send(tamT);

                        foreach (string name in map.Keys)
                        {
                            //envia o tamanho em bytes da chave
                            tamT = BitConverter.GetBytes(name.Length);
                            socket.Send(tamT);

                            //envia os bytes da string
                            byte[] nome = new byte[name.Length];
                            nome = Encoding.UTF8.GetBytes(name);
                            socket.Send(nome);

                            List<Produto> lp = new List<Produto>();
                            map.TryGetValue(name, out lp);

                            byte[] tamN = new byte[4];
                            tamN = BitConverter.GetBytes(lp.Count);
                            socket.Send(tamN);

                            for (int j = 0; j < lp.Count; j++)
                            {
                                EnviaProdutoManualFunc(lp[j]);
                            }
                        }

                        break;

                    case 8: //inicia a thread para os pedidos
                        if (flag_pedidos == false)
                        {
                            if (server == null)
                            {
                                server = new TcpListener(12350);
                                server.Start();
                                socket.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);
                                gestor.inicializaSocketPedidos(server.AcceptSocket());
                                flag_pedidos = true;
                            }
                        }

                        break;

                    case 11:
                        //recebe o id para a notificacao
                        byte[] dadddd = new byte[4];
                        socket.Receive(dadddd, 4, SocketFlags.None);
                        int sizenotfs = BitConverter.ToInt32(dadddd, 0);
                        dadddd = new byte[sizenotfs];
                        socket.Receive(dadddd, sizenotfs, SocketFlags.None);
                        string idPlayerNotf = Encoding.UTF8.GetString(dadddd);

                        dadddd = new byte[4];
                        //recebe id do pedido
                        socket.Receive(dadddd, 4, SocketFlags.None);
                        int idPedidoNotfs = BitConverter.ToInt32(dadddd, 0);

                        var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

                        request.KeepAlive = true;
                        request.Method = "POST";
                        request.ContentType = "application/json; charset=utf-8";

                        byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                                + "\"app_id\": \"56439dce-6b9f-4419-9f88-6ef9d32ed4ea\","
                                                                + "\"contents\": {\"en\": \"O seu Pedido com o ID: "+idPedidoNotfs+" esta pronto a ser levantado!\"},"
                                                                + "\"include_player_ids\": [\""+idPlayerNotf+"\"]" +
                                                                  "}");

                        string responseContent = null;

                        try
                        {
                            using (var writer = request.GetRequestStream())
                            {
                                writer.Write(byteArray, 0, byteArray.Length);
                            }

                            using (var response = request.GetResponse() as HttpWebResponse)
                            {
                                using (var reader = new StreamReader(response.GetResponseStream()))
                                {
                                    responseContent = reader.ReadToEnd();
                                }
                            }
                        }
                        catch (WebException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                        }

                        break;

                    default:
                        flag = false;
                        break;
                }
                msg = -1;
                data = new byte[512];
            }
            Console.WriteLine("Thread: I ended the communication with the FUNC, disconnecting...");
        }

        /**
        * Envia para a aplicação do funcionário um produto
        */
        public void EnviaProdutoManualFunc(Produto p)
        {
            //envia o id do produto
            byte[] dataNum = new byte[4];
            
            socket.Send(BitConverter.GetBytes(p.id), 4, SocketFlags.None);

            byte[] dataString;
            //envia o num de bytes do tipo
            dataString = Encoding.UTF8.GetBytes(p.tipo);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do tipo            
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia o num de bytes do nome
            dataString = Encoding.UTF8.GetBytes(p.nome);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do nome
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia o num de bytes do detalhes
            dataString = Encoding.UTF8.GetBytes(p.detalhes);
            socket.Send(BitConverter.GetBytes(dataString.Length), 4, SocketFlags.None);
            //envia os bytes do detalhes
            socket.Send(dataString, dataString.Length, SocketFlags.None);

            //envia a disponibilidade
            dataNum = BitConverter.GetBytes(p.disponibilidade);
            socket.Send(dataNum, 4, SocketFlags.None);

            //envia o preco
            byte[] dataFloat = BitConverter.GetBytes(p.preco);
            dataNum = BitConverter.GetBytes(dataFloat.Length);
            socket.Send(dataNum, 4, SocketFlags.None);//envia num bytes
            socket.Send(dataFloat, dataFloat.Length, SocketFlags.None);//envia os bytes
        }

    }
}

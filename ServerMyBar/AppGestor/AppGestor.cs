using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace AppGestor
{
    public class AppGestor
    {
        private static Socket master;


        static void Main(string[] args)
        {
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
            // 11 EditarEmpregado
            // 12 RemoverProduto

            int input;
            byte[] msg = new byte[4];
            bool flag = true;


            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12346);

            try
            {
                master.Connect(ipe);
                LN ln = new LN(master);
                // testes  
                // master.Send(BitConverter.GetBytes(1));
                // enviaPedido(new Pedido());

                while (flag)
                {
                    Console.WriteLine("Insira: \n 1 para Fazer um pedido \n 2 para Fazer login \n 5 addProduto \n 6 editarproduto \n 7 consultas estatisticas");
                    input = Convert.ToInt32(Console.ReadLine());
                    switch (input)
                    {
                        case 1: //Novo_Pedido
                            msg = BitConverter.GetBytes(input); // diz o comando 
                            master.Send(msg);
                            enviaPedido(new Pedido());
                            break;
                        case 2: // Login
                            Console.WriteLine("Starting authentication");
                            Console.WriteLine("Insira email");
                            string email = Console.ReadLine();
                            Console.WriteLine("Insira pw");
                            string pw = Console.ReadLine();
                            bool login = ln.iniciarSessao(email, pw);
                            if (login) Console.WriteLine("i'm in you crazy bastard");
                            else Console.WriteLine("we will get em next time");
                            break;

                        case 5: //addProduto
                            Console.WriteLine("# Starting adicionarProduto #");
                            Console.WriteLine("Insira o tipo:");
                            string tipo = Console.ReadLine();
                            Console.WriteLine("Insira o nome:");
                            string nome = Console.ReadLine();
                            Console.WriteLine("Insira detalhes:");
                            string detalhes = Console.ReadLine();
                            Console.WriteLine("Insira a disponibilidade:");
                            int disp = int.Parse(Console.ReadLine());
                            Console.WriteLine("Insira o preco:");
                            float preco = float.Parse(Console.ReadLine());
                            Console.WriteLine("Insira a imagem:");
                            string imagem = Console.ReadLine();
                            Produto p = new Produto(0, tipo, nome, detalhes, disp, preco);
                            int idP = ln.adicionarProduto(p);
                            Console.WriteLine("ID = " + idP);
                            break;
                        case 6: //editar produto

                            Console.WriteLine("Id do produto please:");
                            int id6 = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Tipo do produto please:");
                            string tipo6 = Console.ReadLine();

                            Console.WriteLine("Nome do produto please:");
                            string nome6 = Console.ReadLine();

                            Console.WriteLine("Detalhes do produto please:");
                            string detalhes6 = Console.ReadLine();

                            Console.WriteLine("Disponibilidade do produto please:");
                            int disponibilidade6 = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Preco do produto please");
                            float preco6 = Convert.ToSingle(Console.ReadLine());

                            Produto p6 = new Produto(id6, tipo6, nome6, detalhes6, disponibilidade6, preco6);

                            if (ln.editarProduto(p6) == true)
                            {
                                Console.WriteLine("FIXE");
                            }
                            else
                            {
                                Console.WriteLine("FOXE");
                            }
                            break;
                        case 7: //consultas estatisticas
                            List<Pedido> a = ln.consultasEstatisticas(new DateTime(2019, 7, 21, 14, 47, 25), new DateTime(2020, 2, 1, 14, 47, 25));
                            int kk = 99;
                            break;
                        case 10:
                            flag = ln.TerminarSessao();
                            break;
                        case 11:
                            Console.WriteLine("# Starting editarEmpregado #");
                            Console.WriteLine("Insira o email do empregado a alterar");
                            string actualmail = Console.ReadLine();
                            Console.WriteLine("Insira a nova password");
                            string newpass = Console.ReadLine();
                            Console.WriteLine("Insira o novo email:");
                            string newname = Console.ReadLine();
                            Console.WriteLine("Vai ser gestor? (true/false)");
                            string ismanager = Console.ReadLine();
                            bool val = false;
                            if (ismanager.Equals("true")) val = true;
                            Empregado e = new Empregado(actualmail, newpass, newname, val);
                            bool op = ln.editarEmpregado(actualmail, e);
                            Console.WriteLine(op);
                            break;
                        case 12: //removerEmpregado
                            Console.WriteLine("# Starting removerEmpregado #");
                            Console.WriteLine("Insira o email do empregado a remover");
                            string removemail = Console.ReadLine();
                            bool res = ln.removerEmpregado(removemail);
                            Console.WriteLine(res);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    msg = new byte[100];
                }
            }
            catch (Exception e) { Console.WriteLine("Exception: " + e.Message); }
            master.Close();
        }


        public static void enviaPedido(Pedido p)
        {
            byte[] pedido = p.SavetoBytes();
            master.Send(BitConverter.GetBytes(pedido.Length)); // envia numero bytes    
            master.Send(pedido);
        }
    }
}
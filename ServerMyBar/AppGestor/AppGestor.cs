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
            // 1 LOGIN
            // 2 ALTERAR ESTADO DO SISTEMA
            // 3 VISUALIZAR PEDIDO
            // 4 MUDAR ESTADO DO PEDIDO
            // 5 NOTIFICAR CLIENTE
            // 6 ADICIONAR PRODUTO
            // 7 EDITAR    PRODUTO
            // 8 REMOVER   PRODUTO
            // 9  ADICIONAR EMPREGADO
            // 10 EDITAR    EMPREGADO
            // 11 REMOVER   EMPREGADO
            // 12 ALTERAR INFO EMPRESA
            // 13 CONSULTAS ESTATISTICAS
            // 14 LOGOUT

            int input;
            byte[] msg = new byte[4];
            bool flag = true;

            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12346);

            try
            {
                master.Connect(ipe);
                LN ln = new LN(master);

                while (flag)
                {
                    Console.WriteLine("INSERT...");
                    input = Convert.ToInt32(Console.ReadLine());

                    switch (input)
                    {
                        case 1: // LOGIN
                            Console.WriteLine("Starting authentication...");

                            Console.WriteLine("Insert email:");
                            string email = Console.ReadLine();

                            Console.WriteLine("Insert password:");
                            string pw = Console.ReadLine();

                            bool login = ln.iniciarSessao(email, pw);

                            if (login) Console.WriteLine("Are you in...");
                            else Console.WriteLine("Try again...");

                            break;

                        case 2: // ALTERAR ESTADO DO SISTEMA
                            bool estado = ln.alternarEstadoSistema();

                            if (estado) Console.WriteLine("Can serve customers...");
                            else Console.WriteLine("Go home is closed...");

                            break;

                        case 3: // VISUALIZAR PEDIDO
                            Console.WriteLine("Enter the order id:");
                            int id = int.Parse(Console.ReadLine());

                            Pedido pe = ln.visualizarPedido(id);

                            pe.imprimePedido();

                            break;

                        case 4: // MUDAR ESTADO DO PEDIDO
                            Console.WriteLine("Insira o id do pedido que pretende alterar:");
                            int idPedido = int.Parse(Console.ReadLine());

                            bool b = ln.mudarEstadoPedido(idPedido);
                            if (b) Console.WriteLine("Estado de pedido mudado!");
                            else Console.WriteLine("Estado do pedido não alterado");

                            break;

                        case 5: // NOTIFICAR CLIENTE
                            Console.WriteLine("Enter the id of client of who wants to upset: ");
                            string idCliente = Console.ReadLine();
                            Console.WriteLine("Insert the message: ");
                            string mensagem = Console.ReadLine();

                            ln.notificarCliente(idCliente, mensagem);

                            Console.WriteLine("Success!!!");
                            break;

                        case 6: // ADICIONAR PRODUTO
                            Console.WriteLine("# ADD PRODUCT #");
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

                        case 7: // EDITAR PRODUTO
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

                        case 8: //REMOVER PRODUTO
                            Console.WriteLine("# DELETE PRODUCT #");
                            Console.WriteLine("Insira o id do produto a remover");
                            int idr = int.Parse(Console.ReadLine());

                            bool resp = ln.removerProduto(idr);
                            Console.WriteLine(resp);

                            break;

                        case 9: // ADICIONAR EMPREGADO
                            Console.WriteLine("EMAIL: ");
                            string mail = Console.ReadLine();
                            Console.WriteLine("PASSWORD:");
                            string pass = Console.ReadLine();
                            Console.WriteLine("NOME:");
                            string name = Console.ReadLine();
                            Console.WriteLine("Vai ser gestor? (true/false)");
                            string isman = Console.ReadLine();
                            bool v = false;
                            if (isman.Equals("true")) v = true;

                            Empregado emp = new Empregado(mail, pass, name, v);
                            bool r = ln.adicionarEmpregado(emp);
                            Console.WriteLine(r);
                            break;

                        case 10: // EDITAR EMPREGADO
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

                        case 11: // REMOVER EMPREGADO
                            Console.WriteLine("# Starting removerEmpregado #");
                            Console.WriteLine("Insira o email do empregado a remover");
                            string removemail = Console.ReadLine();
                            bool res = ln.removerEmpregado(removemail);
                            Console.WriteLine(res);
                            break;

                        case 12: // ALTERAR INFO EMPRESA
                            break;

                        case 13: // CONSULTAS ESTATISTICAS
                            List<Pedido> a = ln.consultasEstatisticas(new DateTime(2019, 7, 21, 14, 47, 25), new DateTime(2020, 2, 1, 14, 47, 25));
                            int kk = 99;
                            break;

                        case 14: // LOGOUT
                            flag = ln.TerminarSessao();
                            break;

                        default:
                            flag = false;
                            break;
                    }
                    msg = new byte[100];
                }
            }
            catch (Exception e) { Console.WriteLine("UPS...Exception: " + e.Message); }
            master.Close();
        }
    }
}
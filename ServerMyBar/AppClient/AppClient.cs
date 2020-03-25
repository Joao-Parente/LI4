using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AppClient{
    public class AppClient{

        private static Socket master;


        static void Main(string[] args){
            int input;
            byte[] msg = new byte[4];
            bool flag = true;
            LN ln = new LN();

            while (flag){
                // 1 Ver Produtos
                // 2 Pedidos Anteriores
                // 3 Alterar Pedido
                // 4 Efetuar Pedido <
                // 5 NoUlitmoPedido
                // 6 NovoProdutoFavorito
                // 7 InfoEmpresa
                // 8 AvaliarProduto
                // 9 IniciarSessao  <
                // 10 TerminarSessao
                // 11 RegistarNovoCliente
                // 12 Reclamacao

                Console.WriteLine("Insira: \n 1 para Ver Produtos \n 2 para ver os Pedidos Anteriores \n 3 para Alterar um pedido \n 4 para Fazer um pedido \n 5 para NoUlitmoPedido \n 6 para adicionar um novo produto aos favoritos \n 9 para Fazer login  \n 11 para Registar");
                input = Convert.ToInt32(Console.ReadLine());

                switch (input){
                    case 1: //VerProdutos
                        ln.verProdutos();
                        break;
                    case 2: //pedidos anterioresString produtos;
                        Console.WriteLine("Insira o email do cliente");
                        string idCliente = Console.ReadLine();
                        List<Pedido> pAnteriores = ln.PedidosAnteriores(idCliente);

                        break;
                    case 3: //Alterar Pedido
                        String produtos;
                        Console.WriteLine("Insira o ID do pedido");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Pretende adicionar produtos? (S/N)");
                        string res = Console.ReadLine();
                        if (res.Equals("S"))
                        {
                            Console.WriteLine("Indique os produtos que pretende adicionar (separados por um espaço)");
                            produtos = Console.ReadLine();
                            ln.alterarPedido(0, id, produtos);
                        }

                        Console.WriteLine("Pretende remover produtos? (S/N)");
                        res = Console.ReadLine();
                        if (res.Equals("S"))
                        {
                            Console.WriteLine("Indique os produtos que pretende remover (separados por um espaço)");
                            produtos = Console.ReadLine();
                            ln.alterarPedido(1, id, produtos);
                        }
                        break;
                    case 4: //Novo_Pedido
                        ln.EfetuarPedido(new Pedido());
                        break;
                    case 5: // NoUlitmoPedido
                        List<int> numeros = ln.NoUltimoPedido();
                        break;
                    case 6: //Adicionar um produto aos favoritos
                        Console.WriteLine("Insira o id do produto");
                        int idProduto = Convert.ToInt32(Console.ReadLine());
                        ln.AdicionarAosFavoritos(idProduto);
                        break;
                    case 9: // Login
                        Console.WriteLine("Starting authentication");
                        //Parse email e password
                        Console.WriteLine("Insira email");
                        string email = Console.ReadLine();
                        Console.WriteLine("Insira pw");
                        string pw = Console.ReadLine();
                        ln.IniciarSessao(email, pw);
                        break;
                    case 10: //Logout
                        ln.TerminarSessao();
                        break;
                    case 11: //RegistarCliente
                        //Parse email e password
                        Console.WriteLine("Insira email");
                        string e = Console.ReadLine();
                        Console.WriteLine("Insira pw");
                        string p = Console.ReadLine();
                        Console.WriteLine("Insira nome");
                        string n = Console.ReadLine();
                        ln.RegistaUtilizador(e, p, n);
                        break;
                    case 12: //fazer reclamacao

                        //recebe o id do pedido
                        Console.WriteLine("Insira o id do pedido");
                        int idPedidoReclamacao = Convert.ToInt32(Console.ReadLine());

                        //recebe o motivo da reclamacao
                        Console.WriteLine("Insira o motivo da reclamacao");
                        string motivo = Console.ReadLine();

                        //recebe a reclamacao propriamente dita
                        Console.WriteLine("Insira a reclamacao que deseja fazer");
                        String reclamacao = Console.ReadLine();

                        bool f = ln.reclamacao(idPedidoReclamacao, motivo, reclamacao);
                        if (f == true)
                        {
                            Console.WriteLine("CORREU TUDO BEM!");
                        }
                        else
                        {
                            Console.WriteLine("CORREU TUDO MAL!");
                        }
                        break;
                    default:
                        flag = false;
                        break;
                }
                msg = new byte[100];
            }
        }
    }
}


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

                Console.WriteLine("Insira: \n 4 para Fazer um pedido \n 5 para NoUlitmoPedido \n 9 para Fazer login  \n 11 para Registar");
                input = Convert.ToInt32(Console.ReadLine());

                switch (input){
                    case 1: //VerProdutos
                        ln.verProdutos();
                        break;
                    case 4: //Novo_Pedido
                        ln.EfetuarPedido(new Pedido());
                        break;
                    case 5: // NoUlitmoPedido
                        List<int> numeros = ln.NoUltimoPedido();
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
                    default:
                        flag = false;
                        break;
                }
                msg = new byte[100];
            }
        }
    }
}


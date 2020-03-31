using System;
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
            LN ln = new LN();


            try
            {
                // testes  
                // master.Send(BitConverter.GetBytes(1));
                // enviaPedido(new Pedido());

                while (flag)
                {
                    Console.WriteLine("Insira: \n 1 para Fazer um pedido \n 2 para Fazer login");
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
                            msg = new byte[100];
                            msg = BitConverter.GetBytes(input);
                            master.Send(msg);
                            msg = new byte[512]; //senao nao tinha acerteza que o server lia o email antes deste eviar a pw tambem, senoa dps o server lia o email e pw tudo junto.,
                            email = email + "|" + pw + "|";
                            msg = Encoding.ASCII.GetBytes(email);
                            master.Send(msg);
                            byte[] log = new byte[30];
                            master.Receive(log);
                            bool login = BitConverter.ToBoolean(log, 0);
                            if (login) Console.WriteLine("i'm in you crazy bastard");
                            else Console.WriteLine("we will get em next time");
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
                            bool op = ln.editarEmpregado(actualmail,e);
                            Console.WriteLine(op);
                            break;
                        case 12:
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
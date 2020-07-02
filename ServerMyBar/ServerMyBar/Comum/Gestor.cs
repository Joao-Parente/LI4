using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerMyBar.comum
{
    /** 
    * @brief Classe utilizada para gerir todos os comandos de todas as threads conectadas ao servidor. 
    */
    public class Gestor
    {
        private List<Pedido> por_preparar;
        private List<Pedido> em_preparacao;
        private List<Pedido> preparado;
        private int counter;
        private int ticket;

        private Socket pedidos;

        /** 
        * Construtor por defeito, cria as estruturas e inicia os contadores 
        */
        public Gestor()
        {
            por_preparar = new List<Pedido>();
            em_preparacao = new List<Pedido>();
            preparado = new List<Pedido>();
            counter = 0;
            ticket = 0;
        }

        /** 
        * Função que efectua a autenticação do cliente utilizando para isso o acesso controlado a base de dados para validar se o cliente encontra-se registados
        * @param email fornecido pelo cliente a validar
        * @param pw password fornecida pelo cliente a validar
        */
        public bool loginCliente(string email, string pw)
        {
            bool r = false;
            lock (this)
            {
                if (ClienteDAO.getInfoCliente(email, pw) != null) r = true;
            }
            return r;

        }

        /** Função que efectua a autenticação do funcionario utilizando para isso o acesso controlado a base de dados para validar se o funcionário encontra-se registados
        * @param email fornecido pelo cliente a validar
        * @param pw password fornecida pelo cliente a validar
        */
        public bool loginFunc(string email, string pw)
        {
            lock (this)
            {
                return EmpregadoDAO.getInfoEmpregado(email, pw);
            }
        }

        /** Função que obtém o nome do utilizador através do email
        * @param email fornecido pelo cliente 
        */
        public string getNomeUtilizador(string email)
        {
            lock (this)
            {
                return ClienteDAO.getNomeCliente(email);
            }
        }

        /**
        * Dado um identificador de um produto e o email de um cliente adiciona esse produto aos favoritos de um cliente
        * @param idProduto
        * @param idCliente
        */
        public bool addProdutoFavorito(int idProduto, string idCliente)
        {
            lock (this)
            {
                return ProdutoDAO.addProdutoFavorito(idProduto, idCliente);
            }
        }

        /**
        * Inicializa o socket pedidos com a socket fornecida
        * @param Socket s
        */
        public void inicializaSocketPedidos(Socket s)
        {
            pedidos = s;
            pedidos.Blocking = true;
            new Thread(run).Start();
        }

        /**
        * Função em loop que a pedido da class ThreadServerFunc alterar o estado de um pedido 
        */
        public void run()
        {
            byte[] data = new byte[512];
            bool flag = true;

            while (flag)
            {
                pedidos.Receive(data, 4, SocketFlags.None);
                int numero = BitConverter.ToInt32(data, 0);
                switch (numero)
                {
                    case 1: //mudar estado pedido

                        pedidos.Receive(data, 4, SocketFlags.None);
                        int estado_atual = BitConverter.ToInt32(data, 0);

                        pedidos.Receive(data, 4, SocketFlags.None);
                        int id = BitConverter.ToInt32(data, 0);

                        pedidos.Receive(data, 8, SocketFlags.None);
                        DateTime dt = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

                        switch (estado_atual)
                        {
                            case 0://Por preparar -> Em Preparacao
                                
                                for(int i = 0; i < por_preparar.Count; i++)
                                {
                                    if(por_preparar[i].id == id && por_preparar[i].data_hora.CompareTo(dt) == 0)
                                    {
                                        em_preparacao.Add(por_preparar[i]);
                                        counter = por_preparar[i].id;
                                        por_preparar.RemoveAt(i);
                                        break;
                                    }
                                }

                                break;
                            case 1://Em Preparacao -> Pronto a Levantar

                                for (int i = 0; i < em_preparacao.Count; i++)
                                {
                                    if (em_preparacao[i].id == id && em_preparacao[i].data_hora.CompareTo(dt) == 0)
                                    {
                                        preparado.Add(em_preparacao[i]);
                                        em_preparacao.RemoveAt(i);
                                        break;
                                    }
                                }

                                break;
                            case 2://Pronto a Levantar -> Levantado
                                pedidos.Receive(data, 4, SocketFlags.None);
                                int size_id = BitConverter.ToInt32(data, 0);
                                byte[] id_func = new byte[size_id];
                                pedidos.Receive(id_func, size_id, SocketFlags.None);
                                string id_funcionario = Encoding.UTF8.GetString(id_func);

                                for (int i = 0; i < preparado.Count; i++)
                                {
                                    if (preparado[i].id == id && preparado[i].data_hora.CompareTo(dt) == 0)
                                    {
                                        preparado[i].idEmpregado = id_funcionario;
                                        PedidoDAO.registaPedido(preparado[i]);
                                        preparado.RemoveAt(i);
                                        break;
                                    }
                                }

                                break;

                            default:
                                break;
                        }

                        break;
                    default:
                        flag = false;
                        break;
                }
            }
        }

        /**
        * Adiciona um pedido ao sistema e retorna o número do ticket que esse pedido recebeu
        * @param Pedido p
        */
        public int addPedido(Pedido p)
        {
            lock (this)
            {
                int r;
                p.id = ticket;
                r = ticket;
                if (ticket + 1 >= 100)
                {
                    ticket = 0;
                }else
                {
                    ticket++;
                }
                por_preparar.Add(p);

                byte[] str;
                pedidos.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

                //envia o idCliente_notfs
                str = Encoding.UTF8.GetBytes(p.idCliente_notfs);
                pedidos.Send(BitConverter.GetBytes(str.Length), 4, SocketFlags.None);
                pedidos.Send(str, str.Length, SocketFlags.None);

                //envia o id
                pedidos.Send(BitConverter.GetBytes(p.id), 4, SocketFlags.None);

                //envia o idCliente
                str = Encoding.UTF8.GetBytes(p.idCliente);
                pedidos.Send(BitConverter.GetBytes(str.Length), 4, SocketFlags.None);
                pedidos.Send(str, str.Length, SocketFlags.None);

                //envia os detalhes
                str = Encoding.UTF8.GetBytes(p.detalhes);
                pedidos.Send(BitConverter.GetBytes(str.Length), 4, SocketFlags.None);
                pedidos.Send(str, str.Length, SocketFlags.None);

                //envia datahora
                pedidos.Send(BitConverter.GetBytes(p.data_hora.ToBinary()), 8, SocketFlags.None);

                //envia numero de produtos
                pedidos.Send(BitConverter.GetBytes(p.produtos.Count), 4, SocketFlags.None);

                for(int index = 0; index < p.produtos.Count; index++)
                {
                    //envia o id do produto
                    pedidos.Send(BitConverter.GetBytes(p.produtos[index].p.id), 4, SocketFlags.None);

                    //envia a quantidade
                    pedidos.Send(BitConverter.GetBytes(p.produtos[index].quantidades), 4, SocketFlags.None);
                }
            
                return r;
            }
        }

        /**
        * Dado um identificador de um pedido , um motivo e a reclamação em si , adiciona uma reclamação
        * @param int idPedido
        * @param string motivo
        * @param string reclamacao
        */
        public bool AddReclamacao(int idPedido, string motivo, string reclamacao)
        {
            lock (this)
            {
                return ReclamacaoDAO.addReclamacao(idPedido, motivo, reclamacao);
            }

        }

        /**
        * Retorna um id de um pedido dado um identificador de um cliente e um dia e hora
        * @param string idCliente
        * @param DateTime data_hora
        */
        public int getIdPedido(string idCliente,DateTime data_hora)
        {
            lock (this)
            {
                return PedidoDAO.getidPedido(idCliente, data_hora);
            }
        }

        /**
        * Retorna todos os produtos que existem no sistema, mas não envia as imagens dos mesmos
        */
        public Dictionary<string, List<Produto>> VerProdutosFunc()
        {
            lock (this)
            {
                return ProdutoDAO.getAllProdutosFunc();
            }
        }

        /**
        * Retorna todos os produtos que existem no sistema
        */
        public Dictionary<string, List<Produto>> VerProdutos()
        {
            lock (this)
            {
                return ProdutoDAO.getAllProdutos();
            }
        }

        /**
        * Retorna uma lista com os pedidos que o cliente fez anteriormente
        * @param string idCliente
        */
        public List<Pedido> pedidosAnteriores(string idCliente)
        {
            lock (this)
            {
                return PedidoDAO.anteriores(idCliente);
            }
        }

        /**
        * Retorna o número do ultimo pedido processado e o número do contador de tickets
        */
        public List<int> NoUltimoPedido()
        {
            lock (this)
            {
                List<int> r = new List<int>();
                r.Add(counter);
                r.Add(ticket);
                return r;
            }
        }

        /**
        *  Regista um cliente 
        * @param email
        * @param password
        * @param nome
        */
        public bool registarCliente(string email, string password, string nome)
        {
            lock (this)
            {
                return ClienteDAO.registaCliente(email, password, nome);
            }
        }

        /**
        * Dado um identificador do cliente e um identificador de um produto, retira esse produto dos favoritos do cliente
        * @param idProduto
        * @param idCliente
        */
        public bool removerFavoritos(int idProduto, string idCliente)
        {
            lock (this)
            {
                return ProdutoDAO.removeProdutoFavorito(idProduto, idCliente);
            }
        }

        /**
        * Dado um idCliente devolve todos os produtos favoritos do cliente
        * @param idCliente
        */
        public List<int> produtosFavoritos(string idCliente)
        {
            lock (this)
            {
                return ProdutoDAO.getProdutosFavoritos(idCliente);
            }
        }

        /**
        * Dado um identificador do cliente e uma data e hora , devolve o estado do pedido que que foi feito por esse cliente nessa data e hora
        * @param idCliente
        * @param DateTime dt
        */
        public int estadoPedido(string idCliente,DateTime dt)
        {
            for(int i = 0; i < por_preparar.Count; i++)
            {
                if(por_preparar[i].idCliente.Equals(idCliente) && por_preparar[i].data_hora.Equals(dt))
                {
                    return 1;
                }
            }

            for (int i = 0; i < em_preparacao.Count; i++)
            {
                if (em_preparacao[i].idCliente.Equals(idCliente) && em_preparacao[i].data_hora.Equals(dt))
                {
                    return 2;
                }
            }

            for (int i = 0; i < preparado.Count; i++)
            {
                if (preparado[i].idCliente.Equals(idCliente) && preparado[i].data_hora.Equals(dt))
                {
                    return 3;
                }
            }
            return 0;
        }

    }


}
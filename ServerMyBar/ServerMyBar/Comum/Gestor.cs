using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ServerMyBar.comum
{
    public class Gestor
    {
        private List<Pedido> por_preparar;
        private List<Pedido> em_preparacao;
        private List<Pedido> preparado;
        private int counter;
        private int ticket;

       
        private Socket pedidos;


        public Gestor()
        {
            por_preparar = new List<Pedido>();
            em_preparacao = new List<Pedido>();
            preparado = new List<Pedido>();
            counter = 0;
            ticket = 0;
        }


        public bool loginCliente(string email, string pw)
        {
            bool r = false;
            lock (this)
            {
                if (ClienteDAO.getInfoCliente(email, pw) != null) r = true;
            }
            return r;

        }


        public bool loginGestor(string email, string pw)
        {
            lock (this)
            {
                return EmpregadoDAO.autenticaGestor(email, pw);
            }
        }


        public bool loginFunc(string email, string pw)
        {
            bool r = false;
            lock (this)
            {
                if (EmpregadoDAO.getInfoEmpregado(email, pw) != null) r = true;
            }
            return r;
        }


        public Produto getProduto(int id)
        {
            lock (this)
            {
                return ProdutoDAO.getProduto(id);
            }
        }

        public string getNomeUtilizador(string email)
        {
            lock (this)
            {
                return ClienteDAO.getNomeCliente(email);
            }
        }


        public int addProduto(Produto p)
        {
            int val = 0;
            lock (this)
            {
                val = ProdutoDAO.registaProduto(p.tipo, p.nome, p.detalhes, p.disponibilidade, p.preco);
            }
            return val;
        }


        public void adicionarProdutos(int idPedido, string[] produtos)
        {
            lock (this)
            {
                foreach (Pedido x in por_preparar)
                {
                    if (x.id == idPedido)
                    {
                        for (int i = 0; i < produtos.Length; i++)
                            x.adicionarProduto(produtos[i]);
                    }
                }
            }
        }


        public Boolean removeProduct(int id)
        {

            bool res = false;
            lock (this)
            {
                if (ProdutoDAO.removeProduto(id) != false) res = true;
            }
            return res;
        }


        public void removerProdutos(int idPedido, string[] produtos)
        {
            lock (this)
            {
                foreach (Pedido x in por_preparar)
                {
                    if (x.id == idPedido)
                    {
                        for (int i = 0; i < produtos.Length; i++)
                            x.removerProduto(produtos[i]);
                    }
                }
            }
        }

        public bool addProdutoFavorito(int idProduto, string idCliente)
        {
            lock (this)
            {
                return ProdutoDAO.addProdutoFavorito(idProduto, idCliente);
            }
        }

        public void inicializaSocketPedidos(Socket s)
        {
            pedidos = s;
        }

        public int addPedido(Pedido p)
        {
            lock (this)
            {
                int r;
                p.id = ticket;
                r = ticket;
                ticket++;
                por_preparar.Add(p);

                byte[] str;
                pedidos.Send(BitConverter.GetBytes(1), 4, SocketFlags.None);

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
            
                Console.WriteLine("VALOR " + p.id);
                //PedidoDAO.registaPedido(p);
                return r;
            }
        }

        public Pedido getPedido(int idPedido)
        {
            lock (this)
            {
                foreach (Pedido x in por_preparar)
                {
                    if (x.id == idPedido)
                    {
                        return x;
                    }
                }
                foreach (Pedido x in em_preparacao)
                {
                    if (x.id == idPedido)
                    {
                        return x;
                    }
                }
                foreach (Pedido x in preparado)
                {
                    if (x.id == idPedido)
                    {
                        return x;
                    }
                }

                return PedidoDAO.getPedido(idPedido);

            }
        }


        public bool changePedido(int idPedido, Pedido np)
        {
            lock (this)
            {
                foreach (Pedido x in por_preparar)
                {
                    if (x.id == idPedido)
                    {
                        int i = por_preparar.IndexOf(x);
                        por_preparar.Remove(x);
                        np.id = idPedido;
                        por_preparar.Insert(i, np);
                        return true;
                    }
                }
                return false;
            }
        }


        //+estatisticas(mes : int, ano : int)


        //+feedback(idPedido : int) : string 


        //+editProduct(id : in, p : Server.Produto) : bool


        public bool addEmpregado(Empregado e)
        {
            lock (this)
            {
                return EmpregadoDAO.addEmpregado(e);
            }
        }

        public bool editEmpregado(string email, Empregado e)
        {
            bool res = false;
            lock (this)
            {
                if (EmpregadoDAO.editEmpregado(email, e) != false) res = true;
            }
            return res;
        }

        public bool removeEmpregado(string email)
        {
            bool res = false;
            lock (this)
            {
                if (EmpregadoDAO.RemoveEmpregado(email) != false) res = true;
            }
            return res;
        }


        //+getEmpregado(id : int) : Server.Empregado

        public Empregado getEmpregado(string idEmpregado)
        {
            lock (this)
            {
                return EmpregadoDAO.getEmpregado(idEmpregado);
            }
        }


        //+getReclamacao(int id) : Server.Reclamacao


        public Reclamacao GetReclamacao(int idPedido)
        {
            lock (this)
            {
                return ReclamacaoDAO.getReclamacao(idPedido);
            }
        }

        public bool proximoEstado(int idPedido)
        {
            lock (this)
            {
                foreach (Pedido p in por_preparar)
                {
                    if (p.id == idPedido)
                    {
                        por_preparar.Remove(p);
                        em_preparacao.Add(p);
                        return true;
                    }
                }

                foreach (Pedido p in em_preparacao)
                {
                    if (p.id == idPedido)
                    {
                        em_preparacao.Remove(p);
                        preparado.Add(p);
                        return true;
                    }
                }
                return false;
            }
        }

        public bool AddReclamacao(int idPedido, string motivo, string reclamacao)
        {
            lock (this)
            {
                return ReclamacaoDAO.addReclamacao(idPedido, motivo, reclamacao);
            }

        }

        public Dictionary<string, List<Produto>> VerProdutosFunc()
        {
            lock (this)
            {
                return ProdutoDAO.getAllProdutos();
            }
        }

        public Dictionary<string, List<Produto>> VerProdutos()
        {
            lock (this)
            {
                return ProdutoDAO.getAllProdutos();
            }
        }

        public List<Pedido> pedidosAnteriores(string idCliente)
        {
            lock (this)
            {
                return PedidoDAO.anteriores(idCliente);
            }
        }

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


        //+AddFavoritoProduto(idProduto : int, idCliente : int) : bool


        //+verInfoEmpresa() : List sring


        public bool registarCliente(string email, string password, string nome)
        {
            lock (this)
            {
                return ClienteDAO.registaCliente(email, password, nome);
            }
        }

        public void notificarCliente(string idCliente, string mensagem)
        {
            lock (this)
            {
                //fazer alguma coisa que nao sei (nuno & goncalo)
            }
        }

        public bool editarProduto(int idProduto, Produto p)
        {
            lock (this)
            {
                return ProdutoDAO.editProduto(idProduto, p);
            }
        }

        public List<Pedido> consultasEstatisticas(DateTime inicio, DateTime fim)
        {
            lock (this)
            {
                return PedidoDAO.consultaEstatisticas(inicio, fim);
            }
        }

        public List<int> produtosFavoritos(string idCliente)
        {
            lock (this)
            {
                return ProdutoDAO.getProdutosFavoritos(idCliente);
            }
        }
    }


}
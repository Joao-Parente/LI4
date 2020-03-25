using System;
using System.Collections.Generic;

namespace ServerMyBar.comum
{
    public class Gestor
    {
        private List<Pedido> por_preparar;
        private List<Pedido> em_preparacao;
        private List<Pedido> preparado;
        private int counter;
        private int ticket;


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
            bool r = false;
            lock (this)
            {
                if (ClienteDAO.getInfoCliente(email, pw) != null) r = true;
            }
            return r;
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


        //+getProduto(id : int) : Produto


        //+addProduct(p : Produto) : int


        public void adicionarProdutos(int idPedido, String[] produtos)
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


        //+removeProduct(id : int)


        public void removerProdutos(int idPedido, String[] produtos)
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

        public void addProdutoFavoritos(int idProduto, string idCliente)
        {
            lock (this)
            {
                //Mando para onde? idk
                return;
            }
        }


        public int addPedido(Pedido p)
        {
            lock (this)
            {
                int r;
                lock (this)
                {
                    p.id = ticket;
                    r = ticket;
                    ticket++;
                    por_preparar.Add(p);
                    Console.WriteLine("VALOR " + p.id);
                }
                return r;
            }
        }

        //+getPedido(id : int) : Server.Pedido


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


        //+addEmpregado(emp : Server.Empregado) : int


        //+editEmpregado(id : int, emp : Emp) : bool


        //+removeEmpregado(id : int) : bool


        //+getEmpregado(id : int) : Server.Empregado


        //+getReclamacao(int id) : Server.Reclamacao


        public List<Reclamacao> GetReclamacoes(int idPedido)
        {
            lock (this)
            {
                return ReclamacaoDAO.getReclamacoes(idPedido);
            }
        }
        
        public bool AddReclamacao(int idPedido,string motivo,string reclamacao)
        {
            lock (this)
            {
                return ReclamacaoDAO.addReclamacao(idPedido, motivo, reclamacao);
            }
            
        }


        public Dictionary<string, List<Produto>> VerProdutos()
        {
            lock (this)
            {
                Dictionary<string, List<Produto>> map = new Dictionary<string, List<Produto>>();
                map = ProdutoDAO.getAllProdutos();
                return map;
            }
        }

        public List<Pedido> pedidosAnteriores (string idCliente)
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
                r.Add(42);
                r.Add(36);
                return r;
            }
        }


        //+AddFavoritoProduto(idProduto : int, idCliente : int) : bool


        //+verInfoEmpresa() : List sring


        //+avaliarProduto(idCliente : int, idProduto : int, avaliacao : int)

        public void avaliarProduto(string idCliente, int idProduto, int avaliacao)
        {
            lock (this)
            {
                PedidoDAO.avaliar(idCliente, idProduto, avaliacao);
            }
        }

        public bool registarCliente(string email, string password, string nome)
        {
            lock (this)
            {
                return ClienteDAO.registaCliente(email, password, nome);
            }
        }
    }


}
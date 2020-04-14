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


        public Produto getProduto(int id )
        {

                lock (this)
                {
                    return ProdutoDAO.getProduto(id);
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

        public Pedido getPedido(int idPedido) {
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


        //+addEmpregado(emp : Server.Empregado) : int


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

        public bool AddReclamacao(int idPedido, string motivo, string reclamacao)
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
    }


}
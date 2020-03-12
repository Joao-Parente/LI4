using System;
using System.Collections.Generic;

namespace ServerMyBar.comum
{


        public class Gestor



        {

            private List<Pedido> por_preparar;
            private List<Pedido> em_preparacao;
            private List<Pedido> preparado;




            public Gestor()
            {
                por_preparar= new List<Pedido>();
                em_preparacao= new List<Pedido>();
                preparado= new List<Pedido>();
                
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
            
            //+removeProduct(id : int)
            
            //+addPedido(p : Server.Pedido) : int
            
            
            //+getPedido(id : int) : Server.Pedido
            

            //+changePedido(idPedido : int  mudanca Pedido) : bool
            
            //+estatisticas(mes : int, ano : int)
            
            //+feedback(idPedido : int) : string
            
            //+editProduct(id : in, p : Server.Produto) : bool
            
            //+addEmpregado(emp : Server.Empregado) : int
            
            //+editEmpregado(id : int, emp : Emp) : bool
            
            //+removeEmpregado(id : int) : bool
            
            //+addReclamacao(idPedido int, motivo : string, reclamacao : string) : bool
            
            //+getEmpregado(id : int) : Server.Empregado
            
            //+getReclamacao(int id) : Server.Reclamacao
            
            //+verProdutos() : List Produto
            
            //+PedidosAnteriores(idCLiente : int) : List Pedido
            
            //+NoUltimoPedido() : int
            
            //+AddFavoritoProduto(idProduto : int, idCliente : int) : bool

            //+verInfoEmpresa() : List sring
            
            //+avaliarProduto(idCliente : int, idProduto : int, avaliacao : int)
            
            //+registarCliente(nome : string, password : string) : bool
            
            
        }
    }

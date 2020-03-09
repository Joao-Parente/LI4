using System;
using System.Collections.Generic;

namespace ServerMyBar
{
    namespace ServerGestor
    {


        public class Gestor



        {

            private List<Pedido> por_preparar;
            private List<Pedido> em_preparacao;
            private List<Pedido> preparado;


            private int contador_bilhete;
            private int ultimo_bilhere;



            public Gestor()
            {
                por_preparar= new List<Pedido>();
                em_preparacao= new List<Pedido>();
                preparado= new List<Pedido>();
                
                por_preparar.Add(new Pedido());
            }

            public int addPedido(Pedido p) // cliente faz um pedido
            {
                lock (this)
                {
                    
               

                p.id = ultimo_bilhere;
                por_preparar.Add(p);
                
                ultimo_bilhere++;
                }
                return p.id;
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
            
            
            
            
            
        }
    }
}
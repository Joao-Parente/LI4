using System;
using System.Collections.Generic;
using System.Text;

namespace AppCliente
{
    public class PedidoInfo
    {
        public int idPedido { get; set; }
        public DateTime data_hora { get; set; }
        public string preco { get; set; } 
        public string numProdutos { get; set; }
        public string estado { get; set; }

        public PedidoInfo(int id,DateTime d,float p,int num)
        {
            idPedido = id;
            data_hora = d;
            preco = ""+p+"€";
            numProdutos = "Numero de Produtos = "+num;
        }

        public void setEstado(string estado)
        {
            this.estado = estado;
        }
      
    }
}

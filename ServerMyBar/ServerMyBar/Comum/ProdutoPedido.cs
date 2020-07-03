using System;
using System.Collections.Generic;
using System.Text;

namespace ServerMyBar.comum
{
    public class ProdutoPedido
    {
        public Produto p { get; set; }
        public int quantidades { get; set; }

        public ProdutoPedido(Produto pr, int qua)
        {
            p = pr;
            quantidades = qua;        
        }

    }
}

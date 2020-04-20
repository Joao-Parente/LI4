using System;
using System.Collections.Generic;
using System.Text;

namespace AppFunc
{
    public class ProdutoPedido
    {
        public Produto prod { get; set; }
        public int quantidade { get; set; }

        public ProdutoPedido()
        {
            this.prod = new Produto();
            this.quantidade = 0;
        }

        public ProdutoPedido(Produto p, int q)
        {
            this.prod = p;
            this.quantidade = q;
        }
    }
}

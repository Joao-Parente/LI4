using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace AppCliente
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

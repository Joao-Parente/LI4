using System;
using System.Collections.Generic;
using System.Text;

namespace AppCliente
{
    public class Categoria
    {
        public string Nome { get; set; }
        public byte[] Imagem { get; set; }
        public string NumeroProdutos { get; set; }

        public Categoria(string n,byte[] i,string np)
        {
            this.Nome = n;
            this.Imagem = i;
            this.NumeroProdutos = np;
        }
    }
}

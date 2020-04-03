using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppCliente
{
    public class ProdutoCell
    {
        public int id { get; set; }
        public string Nome { get; set; }
        public string Preco { get; set; }
        public int Quantidades { get; set; }

        public ProdutoCell()
        {
            this.id = 0;
            this.Nome = "hello";
            this.Preco = "69.9€";
            this.Quantidades = 1;
        }

        public ProdutoCell(int i,string n,string p)
        {
            this.id = i;
            this.Nome = n;
            this.Preco = p;
            this.Quantidades = 1;
        }

    }

}

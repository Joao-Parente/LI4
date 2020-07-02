using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AppCliente
{
    public class Categoria
    {
        public string Nome { get; set; }
        public ImageSource Imagem { get; set; }
        public string NumeroProdutos { get; set; }

        public Categoria(string n,byte[] i,string np)
        {
            this.Nome = n;
            this.Imagem = ImageSource.FromStream(() => new MemoryStream(i));
            this.NumeroProdutos = np;
        }
    }
}

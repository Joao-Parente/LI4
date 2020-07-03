using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AppFuncionario
{
    public class Categoria
    {
        public string Nome { get; set; }
        public ImageSource Imagem { get; set; }
        public string NumeroProdutos { get; set; }

        public Categoria(string n,byte[] i,string np)
        {
            this.Nome = n;
            this.Imagem = null;
            this.NumeroProdutos = np;
        }
    }
}

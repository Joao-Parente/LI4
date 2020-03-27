using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppCliente
{
    public class ProdutoCell : ViewCell
    {
        public string Nome { get; set; }

        //private Image Imagem { get; set; }

        public float Preco { get; set; }


        public ProdutoCell()
        {
            this.Nome = "hello";
            this.Preco = (float) 69.9;
        }

        public ProdutoCell(String n/*,Image i*/,float p)
        {
            this.Nome = n;
            //this.Imagem = i;
            this.Preco = p;
        }

    }

}

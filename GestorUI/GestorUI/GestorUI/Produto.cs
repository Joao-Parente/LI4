using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GestorUI
{
    class Produto : ViewCell
    {
        public string Nome { get; set; }

        //private Image Imagem { get; set; }

        public float Preco { get; set; }

        public Produto(String n/*,Image i*/,float p)
        {
            this.Nome = n;
            //this.Imagem = i;
            this.Preco = p;
        }

    }
}

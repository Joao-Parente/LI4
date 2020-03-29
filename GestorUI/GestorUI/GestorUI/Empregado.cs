using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GestorUI
{
    class Empregado : ViewCell
    {
        public string Nome { get; set; }


        public Empregado(String n)
        {
            this.Nome = n;

        }

    }
}

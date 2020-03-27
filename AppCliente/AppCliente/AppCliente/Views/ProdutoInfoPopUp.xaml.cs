using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProdutoInfoPopUp
    {
        public ProdutoInfoPopUp(ProdutoCell p)
        {
            InitializeComponent();
            NomeProduto.Text = p.Nome;
            Preco.Text = ""+p.Preco + "€";
        }
    }
}
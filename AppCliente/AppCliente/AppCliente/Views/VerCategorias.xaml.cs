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
    public partial class VerCategorias : ContentPage
    {
        public VerCategorias()
        {
            InitializeComponent();
        }

        private void BaguetesBotao(object sender, EventArgs e)
        {
            List<Produto> ret = new List<Produto>();

            ret.Add(new Produto("Baguete de Atum", (float)0.9));
            ret.Add(new Produto("Baguete de Frango", (float)0.95));
            ret.Add(new Produto("Baguete de Panados", (float)0.95));
            ret.Add(new Produto("Baguete de Delicias", (float)0.95));

            Navigation.PushAsync(new VerProdutosPage(ret,"Baguetes"));
        }

        private void FolhadosBotao(object sender, EventArgs e)
        {
            List<Produto> ret = new List<Produto>();

            ret.Add(new Produto("Folhado Misto", (float)0.8));
            ret.Add(new Produto("Folhado Carne", (float)0.8));
            ret.Add(new Produto("Folhado Chaves", (float)0.8));
            ret.Add(new Produto("Folhado Frango", (float)0.8));

            Navigation.PushAsync(new VerProdutosPage(ret, "Folhados"));
        }

        private void HamburgueresBotao(object sender, EventArgs e)
        {
            List<Produto> ret = new List<Produto>();

            ret.Add(new Produto("Hamburguer Normal", (float)1.7));
            ret.Add(new Produto("Hamburguer Especial c/ Batatas Fritas", (float)2.5));
            ret.Add(new Produto("Hamburguer Vegetariano", (float)1.9));

            Navigation.PushAsync(new VerProdutosPage(ret, "Hamburgueres"));
        }

        private void CachorrosBotao(object sender, EventArgs e)
        {
            List<Produto> ret = new List<Produto>();

            ret.Add(new Produto("Cachorro Normal", (float)1.7));
            ret.Add(new Produto("Cachorro Especial c/ Batatas Fritas", (float)2.5));
            ret.Add(new Produto("Cachorro Vegetariano", (float)1.9));

            Navigation.PushAsync(new VerProdutosPage(ret, "Cachorros"));
        }
    }
}
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

        private AppClienteLN LN;

        public VerCategorias(AppClienteLN l)
        {
            this.LN = l;
            InitializeComponent();
        }

        private void BaguetesBotao(object sender, EventArgs e)
        {
            List<ProdutoCell> ret = new List<ProdutoCell>();

            ret.Add(new ProdutoCell("Baguete de Atum", (float)0.9));
            ret.Add(new ProdutoCell("Baguete de Frango", (float)0.95));
            ret.Add(new ProdutoCell("Baguete de Panados", (float)0.95));
            ret.Add(new ProdutoCell("Baguete de Delicias", (float)0.95));

            Navigation.PushAsync(new VerProdutosPage(ret,"Baguetes"));
        }

        private void FolhadosBotao(object sender, EventArgs e)
        {
            List<ProdutoCell> ret = new List<ProdutoCell>();

            ret.Add(new ProdutoCell("Folhado Misto", (float)0.8));
            ret.Add(new ProdutoCell("Folhado Carne", (float)0.8));
            ret.Add(new ProdutoCell("Folhado Chaves", (float)0.8));
            ret.Add(new ProdutoCell("Folhado Frango", (float)0.8));

            Navigation.PushAsync(new VerProdutosPage(ret, "Folhados"));
        }

        private void HamburgueresBotao(object sender, EventArgs e)
        {
            List<ProdutoCell> ret = new List<ProdutoCell>();

            ret.Add(new ProdutoCell("Hamburguer Normal", (float)1.7));
            ret.Add(new ProdutoCell("Hamburguer Especial c/ Batatas Fritas", (float)2.5));
            ret.Add(new ProdutoCell("Hamburguer Vegetariano", (float)1.9));

            Navigation.PushAsync(new VerProdutosPage(ret, "Hamburgueres"));
        }

        private void CachorrosBotao(object sender, EventArgs e)
        {
            List<ProdutoCell> ret = new List<ProdutoCell>();

            ret.Add(new ProdutoCell("Cachorro Normal", (float)1.7));
            ret.Add(new ProdutoCell("Cachorro Especial c/ Batatas Fritas", (float)2.5));
            ret.Add(new ProdutoCell("Cachorro Vegetariano", (float)1.9));

            Navigation.PushAsync(new VerProdutosPage(ret, "Cachorros"));
        }
    }
}
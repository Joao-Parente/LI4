using Rg.Plugins.Popup.Services;
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
        Dictionary<string, List<Produto>> Produtos;

        public VerCategorias(AppClienteLN l)
        {
            this.LN = l;
            Produtos = this.LN.GetProdutos(); 
            InitializeComponent();
            VerCategoriasPage.Title = "Categorias";
        }

        private void BaguetesBotao(object sender, EventArgs e)
        {
            if (Produtos.ContainsKey("baguete"))
            {
                List<Produto> pds = Produtos["baguete"];
                List<ProdutoCell> ret = new List<ProdutoCell>();

                for (int i = 0; i < pds.Count; i++)
                {
                    ret.Add(new ProdutoCell(pds[i].id, pds[i].nome, "" + pds[i].preco + "€"));
                }

                if (ret.Count == 0)
                {
                    DisplayAlert("Erro", "Não há nenhum produto da categoria Baguetes", "OK");
                }
                else
                {
                    Navigation.PushAsync(new VerProdutosPage(LN, ret, "Baguetes"));
                }
            }
            else
            {
                DisplayAlert("Erro", "Não há nenhum produto da categoria Baguetes", "OK");
            }
        }

        private void FolhadosBotao(object sender, EventArgs e)
        {
            if (Produtos.ContainsKey("folhado"))
            {
                List<Produto> pds = Produtos["folhado"];
                List<ProdutoCell> ret = new List<ProdutoCell>();

                for (int i = 0; i < pds.Count; i++)
                {
                    ret.Add(new ProdutoCell(pds[i].id, pds[i].nome, "" + pds[i].preco + "€"));
                }

                if (ret.Count == 0)
                {
                    DisplayAlert("Erro", "Não há nenhum produto da categoria Folhados", "OK");
                }
                else
                {
                    Navigation.PushAsync(new VerProdutosPage(LN, ret, "Folhados"));
                }
            }
            else
            {
                DisplayAlert("Erro", "Não há nenhum produto da categoria Folhados", "OK");
            }
        }

        private void HamburgueresBotao(object sender, EventArgs e)
        {
            if (Produtos.ContainsKey("hamburguer"))
            {
                List<Produto> pds = Produtos["hamburguer"];
                List<ProdutoCell> ret = new List<ProdutoCell>();

                for (int i = 0; i < pds.Count; i++)
                {
                    ret.Add(new ProdutoCell(pds[i].id, pds[i].nome, ""+pds[i].preco+"€"));
                }

                if (ret.Count == 0)
                {
                    DisplayAlert("Erro", "Não há nenhum produto da categoria Hamburgueres", "OK");
                }
                else
                {
                    Navigation.PushAsync(new VerProdutosPage(LN, ret, "Hamburgueres"));
                }
            }
            else 
            {
                DisplayAlert("Erro", "Não há nenhum produto da categoria Hamburgueres", "OK");
            }
        }

        private void CachorrosBotao(object sender, EventArgs e)
        {
            if (Produtos.ContainsKey("cachorro"))
            {
                List<Produto> pds = Produtos["cachorro"];
                List<ProdutoCell> ret = new List<ProdutoCell>();

                for (int i = 0; i < pds.Count; i++)
                {
                    ret.Add(new ProdutoCell(pds[i].id, pds[i].nome, "" + pds[i].preco + "€"));
                }

                if (ret.Count == 0)
                {
                    DisplayAlert("Erro", "Não há nenhum produto da categoria Cachorros", "OK");
                }
                else
                {
                    Navigation.PushAsync(new VerProdutosPage(LN, ret, "Cachorros"));
                }
            }
            else
            {
                DisplayAlert("Erro", "Não há nenhum produto da categoria Cachorros", "OK");
            }
        }

        private void BebidasBotao(object sender, EventArgs e)
        {
            if (Produtos.ContainsKey("bebidas"))
            {
                List<Produto> pds = Produtos["bebidas"];
                List<ProdutoCell> ret = new List<ProdutoCell>();

                for (int i = 0; i < pds.Count; i++)
                {
                    ret.Add(new ProdutoCell(pds[i].id, pds[i].nome, "" + pds[i].preco + "€"));
                }

                if (ret.Count == 0)
                {
                    DisplayAlert("Erro", "Não há nenhum produto da categoria Bebidas", "OK");
                }
                else
                {
                    Navigation.PushAsync(new VerProdutosPage(LN, ret, "Bebidas"));
                }
            }
            else
            {
                DisplayAlert("Erro", "Não há nenhum produto da categoria Bebidas", "OK");
            }
        }


        private void CarrinhoComprasBotao(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new CarrinhoCompras(this.LN));
            //PopupNavigation.Instance.PushAsync(new CarrinhoCompras(new CarrinhoComprasViewModel { LN = this.LN }));
        }

    }
}
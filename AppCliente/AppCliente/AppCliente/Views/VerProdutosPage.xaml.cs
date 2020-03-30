using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Net;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerProdutosPage : ContentPage
    {
        List<ProdutoCell> Produtos { get; set; }

        AppClienteLN LN { get; set; }

        public VerProdutosPage(AppClienteLN ln,List<ProdutoCell> Lista,string NomeCategoria)
        {
            InitializeComponent();
            this.LN = ln;
            this.Produtos = Lista;
            ViewProdutos.ItemsSource = Produtos;
            this.ContentPageTeste.Title = NomeCategoria;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //var p = ((ListView)sender).SelectedItem;

            ProdutoCell PSelecionado = (ProdutoCell) e.Item;

            PopupNavigation.Instance.PushAsync(new ProdutoInfoPopUp(this.LN,PSelecionado));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void CarrinhoComprasBotao(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new CarrinhoCompras(this.LN));
            //PopupNavigation.Instance.PushAsync(new CarrinhoCompras(new CarrinhoComprasViewModel { LN=this.LN }));
        }

        private void AdicionaCarrinhoBotao(object sender, EventArgs e)
        {
            ProdutoCell p = (ProdutoCell) ((Button)sender).BindingContext;

            if (this.LN.ProdutoCarrinho(LN.GetProduto(p)) == true)
            {
                DisplayAlert("Sucesso", "Produto Adicionado com Sucesso!", "OK");
            }
            else
            {
                DisplayAlert("Erro", "Já têm esse Produto no seu carrinho", "OK");
            }
        }
    }
}
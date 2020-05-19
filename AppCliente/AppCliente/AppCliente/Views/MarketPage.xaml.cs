using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Xamarin.Forms;

namespace AppCliente
{
    public partial class MarketPage : ContentPage
    {
        private LN ln;
        public ObservableCollection<ProdutoInfo> carrinho { get; set; }
        public ObservableCollection<ProdutoInfo> prods { get; set; }
        public ObservableCollection<Categoria> cats { get; set; }
        public string nome { get; set; }

        public MarketPage(LN l)
        {
            ln = l;

            cats = ln.GetCategorias();

            prods = ln.getProdsFavoritos();

            carrinho = ln.getCarrinho();

            nome = ln.getNomeCliente();

            InitializeComponent();
           
            Thread precoHandler = new Thread(atualizaPreco);
            precoHandler.Start();

            this.BindingContext = this;
        }

        public void atualizaPreco()
        {
            float plocal = ln.getPrecoTotal();
            Device.BeginInvokeOnMainThread(() =>
            {
                this.labelTotal.Text = plocal + "€";
            });
            while (true)
            {
                float atual = ln.getPrecoTotal();
                if(plocal != atual)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.labelTotal.Text = atual + "€";
                    });
                    plocal = atual;
                }                
                Thread.Sleep(2000);
            }
        }

        private async void carrinhoCompras(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CarrinhoCompras(ln));
        }

        private void ButtonHamburguer_Clicked(object sender, EventArgs e)
        {
            picker.Focus();
            if (picker.IsVisible == true)
            {
                picker.IsVisible = false;
            }
            else
            {
                picker.IsVisible = true;
            }            
        }

        private void plusButton(object sender, EventArgs e)
        {
            ProdutoInfo p = (ProdutoInfo)((Button)sender).BindingContext;

            ln.adicionaProdutoCarrinho(ln.getProduto(p.Id));
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            ((ListView)sender).SelectedItem = null;

            Categoria c = (Categoria) e.Item;

            await Navigation.PushModalAsync(new CategoriaProdutos(c.Nome, ln));
           
        }

        private void InfoButton_Clicked(object sender, EventArgs e)
        {

        }

        private async void HistoricoButton_Clicked(object sender, EventArgs e)
        {            
            await Navigation.PushModalAsync(new HistoricoPage(ln));
        }

        private async void EstadoPedidosButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EstadoPedidos(ln));
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection == null) return;

            

            List<Object> prod = (List<Object>) e.CurrentSelection;
            ProdutoInfo pi;

            ((CollectionView)sender).SelectedItem = null;
            if (prod.Count > 0)
            {
                pi = (ProdutoInfo)prod[0];
                PopupNavigation.Instance.PushAsync(new ProdutoPageFav(ln, ln.getProduto(pi.Id)));
            }

            
            
        }
    }

}

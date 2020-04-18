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

        public MarketPage(LN l)
        {
            ln = l;
            Dictionary<string, List<Produto>> ps = ln.getProdutos();
            cats = ln.GetCategorias();
            List<Produto> sss = ps["baguete"];

            prods = new ObservableCollection<ProdutoInfo>();
            for(int i = 0; i < 3; i++)
            {
                prods.Add(new ProdutoInfo(ps[cats[i].Nome][i].id, ps[cats[i].Nome][i].nome, ""+ps[cats[i].Nome][i].preco+"€",0));
            }

            carrinho = ln.getCarrinho();

            InitializeComponent();

            this.Hello.Text = "Olá Nuno";

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
    }

}

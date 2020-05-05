using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarrinhoCompras : ContentPage
    {
        public ObservableCollection<ProdutoInfo> carrinho { get; set; }
        private LN ln;
        public CarrinhoCompras(LN l)
        {
            this.ln = l;
            InitializeComponent();
            this.BindingContext = this;
            listaCarrinho.ItemsSource = ln.getCarrinho();
            this.totalLabel.Text = "Total = " + ln.getPrecoTotal() + "€";
            
        }

        private void ImageButtonPlus_Clicked(object sender, EventArgs e)
        {
            ProdutoInfo pi = (ProdutoInfo) ((ImageButton)sender).BindingContext;

            pi.Quantidades = pi.Quantidades + 1;
            ln.atualizaPrecoTotalMais(pi);

            this.totalLabel.Text = "Total = " + ln.getPrecoTotal() + "€";
        }

        private void ButtonMinus_Clicked(object sender, EventArgs e)
        {
            ProdutoInfo pi = (ProdutoInfo)((Button)sender).BindingContext;

            pi.Quantidades = pi.Quantidades - 1;
            ln.atualizaPrecoTotalMenos(pi);

            if (0 >= pi.Quantidades)
            {
                ln.removeProdutoCarrinho(pi);
            }

            this.totalLabel.Text = "Total = " + ln.getPrecoTotal() + "€";
        }

        private void listaCarrinho_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            ((ListView)sender).SelectedItem = null;
        }

        private void Pagar_ItemTapped(object sender, EventArgs e)
        {
            ObservableCollection<ProdutoInfo> hello = new ObservableCollection<ProdutoInfo>();
            ObservableCollection<ProdutoInfo> carrinho = ln.getCarrinho();

            if (carrinho.Count > 0)
            {
                List<ProdutoPedido> rrr = new List<ProdutoPedido>();
                for (int i = 0; i < carrinho.Count; i++)
                {
                    hello.Add(carrinho[i]);
                    rrr.Add(new ProdutoPedido(ln.getProduto(carrinho[i].Id), carrinho[i].Quantidades));
                }
                Pedido pedido = new Pedido(0, ln.getEmailIdCliente(), "zulmira@work.pt", "", DateTime.Now, rrr);
                ln.EfetuarPedido(pedido);

                for (int i = 0; i < hello.Count; i++)
                {
                    ln.removeProdutoCarrinho(hello[i]);
                }

                this.totalLabel.Text = "Total = " + ln.getPrecoTotal() + "€";
            }
            else
            {
                DisplayAlert("Erro", "O seu carrinho está vazio", "OK");
            }


            //PopupNavigation.Instance.PushAsync(new DetalhesPedido());
        }

        private void DetalhesButton_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new DetalhesPedido(this.ln,this.totalLabel));
        }
    }
}
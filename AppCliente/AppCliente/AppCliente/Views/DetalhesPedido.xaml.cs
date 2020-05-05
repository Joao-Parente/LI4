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
    public partial class DetalhesPedido
    {
        private LN ln;
        private Label totalLabel;
        public DetalhesPedido(LN l,Label x)
        {
            ln = l;
            totalLabel = x;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
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
                Pedido pedido = new Pedido(0, ln.getEmailIdCliente(),"", detailsEntry.Text, DateTime.Now, rrr);
                ln.EfetuarPedido(pedido);

                detailsEntry.Text = "";
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


            PopupNavigation.Instance.PopAsync(true);
        }
    }
}
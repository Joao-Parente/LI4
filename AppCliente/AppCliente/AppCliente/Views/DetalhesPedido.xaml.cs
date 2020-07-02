using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
            int num_pedido;
            if (carrinho.Count > 0)
            {
                List<ProdutoPedido> rrr = new List<ProdutoPedido>();
                for (int i = 0; i < carrinho.Count; i++)
                {
                    hello.Add(carrinho[i]);
                    rrr.Add(new ProdutoPedido(ln.getProduto(carrinho[i].Id), carrinho[i].Quantidades));
                }
                Pedido pedido = new Pedido(0, ln.getEmailIdCliente(),"", detailsEntry.Text, DateTime.Now, rrr);

                num_pedido=ln.EfetuarPedido(pedido);
                DisplayAlert("Sucesso", "O seu pedido foi efetuado, e ficou com o número: " + num_pedido, "OK");

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
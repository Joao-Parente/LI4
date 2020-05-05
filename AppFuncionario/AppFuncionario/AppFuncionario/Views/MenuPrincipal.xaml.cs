using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFuncionario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPrincipal : ContentPage
    {
        private LN ln;
        public ObservableCollection<PedidoInfo> Pedidos { get; set; }

        public MenuPrincipal(LN l)
        {
            ln = l;
            Pedidos = ln.getObCPedidos();            
            /*
            Pedidos = new ObservableCollection<Pedido>();
            List<ProdutoPedido> produtoPedidos = new List<ProdutoPedido>
            {
                new ProdutoPedido(ln.getProduto(10), 2),
                new ProdutoPedido(ln.getProduto(11), 2)
            };
            Pedidos.Add(new Pedido(1, "felix", "zuzu", "Sem Alface", DateTime.Now, produtoPedidos));
            Pedidos.Add(new Pedido(2, "filex", "uzuz", "Sem pão", DateTime.Now, produtoPedidos));*/


            InitializeComponent();

            this.BindingContext = this;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //var p = ((ListView)sender).SelectedItem;

            //Produto PSelecionado = (Produto)e.Item;

            //PopupNavigation.Instance.PushAsync(new ProdutoInfoPopUp(PSelecionado));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void CarregueiBotaoMagico(object sender, EventArgs e)
        {
                       
            BotaoMagico.IsVisible = false;
        }
    }
}
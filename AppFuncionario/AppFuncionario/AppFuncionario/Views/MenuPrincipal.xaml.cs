using Rg.Plugins.Popup.Services;
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

            InitializeComponent();

            this.BindingContext = this;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            PedidoInfo pi = (PedidoInfo)e.Item;


            PopupNavigation.Instance.PushAsync(new InfoPedido(ln, pi, ln.getProdutos_Pedido(pi)));
            
            ((ListView)sender).SelectedItem = null;
        }

        private void MudarEstadoButton_Clicked(object sender, EventArgs e)
        {
            PedidoInfo pi = (PedidoInfo)((Button)sender).BindingContext;
            ln.muda_estado_pedido(pi);
        }
    }
}
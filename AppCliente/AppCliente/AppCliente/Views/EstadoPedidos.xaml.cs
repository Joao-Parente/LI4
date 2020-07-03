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
    public partial class EstadoPedidos : ContentPage
    {
        private LN ln;
        public ObservableCollection<PedidoInfo> pedidos { get; set; }
        public EstadoPedidos(LN l)
        {
            ln = l;
            //ln.PedidosAnteriores(ln.getEmailIdCliente());
            ln.atualizaEstadoPedidos();
            pedidos = ln.getPedidosPorPrepararInfo();

            InitializeComponent();
            this.BindingContext = this;
        }

        private void listapedidos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            ((ListView)sender).SelectedItem = null;

            PedidoInfo pi = (PedidoInfo)e.Item;

            //await Navigation.PushModalAsync(new PedidosPage(ln, ln.GetPedido(pi.idPedido)));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ln.atualizaEstadoPedidos();
            pedidos = ln.getPedidosPorPrepararInfo();
            this.BindingContext = this;
        }
    }
}
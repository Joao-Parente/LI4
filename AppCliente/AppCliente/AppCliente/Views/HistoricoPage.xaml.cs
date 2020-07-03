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
    public partial class HistoricoPage : ContentPage
    {
        private LN ln;
        public ObservableCollection<PedidoInfo> pedidos { get; set; }
        public HistoricoPage(LN l)
        {
            ln = l;

            ln.PedidosAnteriores(ln.getEmailIdCliente());

            pedidos = ln.GetPedidosInfo();
            
            InitializeComponent();
            this.BindingContext = this;
        }

        private async void listapedidos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            ((ListView)sender).SelectedItem = null;

            PedidoInfo pi = (PedidoInfo) e.Item;

            await Navigation.PushModalAsync(new PedidosPage(ln, ln.GetPedido(pi.idPedido)));
        }
    }
}
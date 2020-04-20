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
        public ObservableCollection<Pedido> hist { get; set; }
        public ObservableCollection<PedidoInfo> pedidos { get; set; }
        public HistoricoPage(LN l)
        {
            ln = l;
            pedidos = new ObservableCollection<PedidoInfo>();
            hist = new ObservableCollection<Pedido>(ln.PedidosAnteriores(ln.getEmailIdCliente()));
            
            for(int i = 0; i < hist.Count; i++)
            {
                float precoTotal = 0;
                List<ProdutoPedido> ppp = hist[i].produtos;
                for(int j = 0; j < ppp.Count; j++)
                {
                    precoTotal += ppp[j].p.preco*ppp[j].quantidades;
                }
                pedidos.Add(new PedidoInfo(hist[i].id, hist[i].data_hora, precoTotal, ppp.Count));
            }
            
            InitializeComponent();
            this.BindingContext = this;
        }

        private void listapedidos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            ((ListView)sender).SelectedItem = null;
        }
    }
}
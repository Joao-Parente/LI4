using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFuncionario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPedido
    {
        private LN ln;
        private PedidoInfo pi;
        public string detalhes { get; set; }
        public string estado { get; set; }
        public ObservableCollection<ProdutoInfo> Produtos { get; set; }
        public InfoPedido(LN l,PedidoInfo pi,ObservableCollection<ProdutoInfo> ps)
        {
            this.ln = l;
            this.pi = pi;
            this.detalhes = pi.detalhes;
            this.estado = pi.estado;
            this.Produtos = ps;

            InitializeComponent();

            this.BindingContext = this;
        }

        private void listaPedidos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;


            ((ListView)sender).SelectedItem = null;
        }

    }
}
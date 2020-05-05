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
    public partial class PedidosPage : ContentPage
    {
        private LN ln;
        public DateTime data { get; set; }
        public ObservableCollection<ProdutoInfoPedidos> prods { get; set; }
        public string preco { get; set; }

        public PedidosPage(LN l,Pedido p)
        {
            ln = l;
            data = p.data_hora;
            float pr = 0;
            prods = new ObservableCollection<ProdutoInfoPedidos>();
            for(int i = 0; i < p.produtos.Count; i++)
            {
                prods.Add(new ProdutoInfoPedidos(p.produtos[i].p.id, p.produtos[i].p.nome,
                    "" + p.produtos[i].p.preco + "€", p.produtos[i].quantidades, p.produtos[i].p.imagem));

                pr += p.produtos[i].p.preco * p.produtos[i].quantidades;
            }
            preco = "Total = " + pr+"€";

            
            InitializeComponent();

            this.BindingContext = this;
        }

        private void listaprodutos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            ((ListView)sender).SelectedItem = null;
        }
    }
}
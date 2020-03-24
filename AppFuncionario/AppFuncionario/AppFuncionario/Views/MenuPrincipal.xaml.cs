using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFuncionario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPrincipal : ContentPage
    {

        public List<Pedido> Pedidos { get; set; }

        public MenuPrincipal()
        {
            InitializeComponent();
            Produto p = new Produto("Baguete de Atum", (float)0.9);
            Produto p2 = new Produto("Folhado Misto", (float)0.8);
            List<Produto> ps = new List<Produto>();
            ps.Add(p);ps.Add(p2);
            Pedidos = new List<Pedido>();
            Pedidos.Add(new Pedido(1, 4, "zulmiramail", "Gluten Free", DateTime.Now, ps));
            Pedidos.Add(new Pedido(1, 4, "acaciomail", "Vegan", DateTime.Now, ps));
            ViewPedido.ItemsSource = Pedidos;
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
            
        }
    }
}
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppGestor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GerirEmpregados : ContentPage
    {
        private LN ln;
        public ObservableCollection<Empregado> Empregados { get; set; }
        public GerirEmpregados(LN l)
        {
            ln = l;
            InitializeComponent();

            Empregados = new ObservableCollection<Empregado>();
            Empregados.Add(new Empregado("ola", "adeus", "felix", false));
            Empregados.Add(new Empregado("ola", "adeus", "felix", false));
            Empregados.Add(new Empregado("ola", "adeus", "felix", false));

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

        private void AddEmpregadoButton_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AdicionarEmpregado(ln));
        }
    }
    
}
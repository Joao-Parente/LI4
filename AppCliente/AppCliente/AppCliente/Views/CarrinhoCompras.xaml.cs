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
    public partial class CarrinhoCompras : ContentPage
    {
        public ObservableCollection<ProdutoInfo> carrinho { get; set; }
        private LN ln;
        public CarrinhoCompras(LN l)
        {
            this.ln = l;
            /*
            Dictionary<string, List<Produto>> ps = ln.getProdutos();

            List<Produto> sss = ps["baguete"];

            carrinho = new ObservableCollection<ProdutoInfo>();
            for (int i = 0; i < sss.Count; i++)
            {
                ln.adicionaProdutoCarrinho(sss[i]);
            }*/
            InitializeComponent();
            this.BindingContext = this;
            listaCarrinho.ItemsSource = ln.getCarrinho();
            this.totalLabel.Text = "Total = " + ln.getPrecoTotal() + "€";
            
        }

        private void ImageButtonPlus_Clicked(object sender, EventArgs e)
        {
            ProdutoInfo pi = (ProdutoInfo) ((ImageButton)sender).BindingContext;

            pi.Quantidades = pi.Quantidades + 1;
            ln.atualizaPrecoTotalMais(pi);

            this.totalLabel.Text = "Total = " + ln.getPrecoTotal() + "€";
        }

        private void ButtonMinus_Clicked(object sender, EventArgs e)
        {
            ProdutoInfo pi = (ProdutoInfo)((Button)sender).BindingContext;

            pi.Quantidades = pi.Quantidades - 1;
            ln.atualizaPrecoTotalMenos(pi);

            if (0 >= pi.Quantidades)
            {
                ln.removeProdutoCarrinho(pi);
            }

            this.totalLabel.Text = "Total = " + ln.getPrecoTotal() + "€";
        }

        private void listaCarrinho_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            ((ListView)sender).SelectedItem = null;

        }
    }
}
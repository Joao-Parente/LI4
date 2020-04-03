using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarrinhoCompras
    {
        AppClienteLN LN { get; set; }
        ObservableCollection<CarrinhoComprasViewModel> produtos { get; set; }
        float pTotal;
        
        public CarrinhoCompras(AppClienteLN l)
        {
            LN = l;
            InitializeComponent();

            produtos = new ObservableCollection<CarrinhoComprasViewModel>();

            ObservableCollection<ProdutoCell> produtos2 = LN.GetCarrinhoOC();
            
            

            ThreadPedidosTracker tpt = new ThreadPedidosTracker(ultimoPedido, meuPedido);
            Thread a = new Thread(tpt.run);
            a.Start();

                        
            float total = 0;
            for(int i = 0; i<produtos2.Count; i++)
            {
                total += (LN.GetProduto(produtos2[i]).preco * produtos2[i].Quantidades);
                produtos.Add(new CarrinhoComprasViewModel(produtos2[i]));
            }
            pTotal = total;
            quantiaTotal.Text = "Total = "+pTotal+"€";

            ProdutosCarrinho.ItemsSource = produtos;
        }
        
        private void MaisQuantidadeBotao(object sender, EventArgs e)
        {
            CarrinhoComprasViewModel p = (CarrinhoComprasViewModel)((Button)sender).BindingContext;
            //ProdutoCell p = (ProdutoCell)((Button)sender).BindingContext;

            int q = p.Quantidades;
            p.Quantidades = q + 1;
            pTotal += LN.GetProduto(p.GetProdutoCell()).preco;
            quantiaTotal.Text = "Total = " + pTotal + "€";
            //PropertyChanged(this, new PropertyChangedEventArgs("MaisQuantidade"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("heelo"));
            //NotifyPropertyChanged("Quantidades");

        }

        private void MenosQuantidadeBotao(object sender, EventArgs e)
        {
            //ProdutoCell p = (ProdutoCell)((Button)sender).BindingContext;
            CarrinhoComprasViewModel p = (CarrinhoComprasViewModel)((Button)sender).BindingContext;

            p.Quantidades--;
            pTotal -= LN.GetProduto(p.GetProdutoCell()).preco;
            quantiaTotal.Text = "Total = " + pTotal + "€";
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("heelominus"));
            if (p.Quantidades <= 0)
            {
                LN.RemoveCarrinho(p.GetProdutoCell());
                produtos.Remove(p);
            }
        }

    }
}
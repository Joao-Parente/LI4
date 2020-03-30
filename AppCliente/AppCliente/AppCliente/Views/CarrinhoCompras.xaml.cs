using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<ProdutoCell> produtos { get; set; }
        
        public CarrinhoCompras(AppClienteLN l)
        {
            LN = l;
            InitializeComponent();
            
            produtos = LN.GetCarrinhoOC();

            ThreadPedidosTracker tpt = new ThreadPedidosTracker(ultimoPedido, meuPedido);
            Thread a = new Thread(tpt.run);
            a.Start();

            ProdutosCarrinho.ItemsSource = produtos;
            
            float total = 0;
            for(int i = 0; i<produtos.Count; i++)
            {
                total += LN.GetProduto(produtos[i]).preco;
            }

            quantiaTotal.Text = "Total = "+total+"€";
        }
        
        private void MaisQuantidadeBotao(object sender, EventArgs e)
        {
            ProdutoCell p = (ProdutoCell)((Button)sender).BindingContext;
                      
            p.Quantidades++;
        }

        private void MenosQuantidadeBotao(object sender, EventArgs e)
        {
            ProdutoCell p = (ProdutoCell)((Button)sender).BindingContext;

            p.Quantidades--;
            if (p.Quantidades <= 0)
            {
                LN.RemoveCarrinho(p);
            }
        }
    }
}
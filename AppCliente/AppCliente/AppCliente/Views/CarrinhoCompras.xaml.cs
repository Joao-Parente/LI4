using System;
using System.Collections.Generic;
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
        List<ProdutoCell> produtos { get; set; }

        public CarrinhoCompras()
        {
            InitializeComponent();
            ThreadPedidosTracker tpt = new ThreadPedidosTracker(ultimoPedido, meuPedido);
            Thread a = new Thread(tpt.run);
            a.Start();

            ProdutoCell p = new ProdutoCell("Baguete de Atum", (float)0.9);
            ProdutoCell p2 = new ProdutoCell("Baguete de Frango", (float)0.95);
            ProdutoCell p3 = new ProdutoCell("Hamburguer", (float)1.5);
            ProdutoCell p4 = new ProdutoCell("Cachorro", (float)1.2);
            ProdutoCell p5 = new ProdutoCell("Folhado Misto", (float)0.8);
            ProdutoCell p6 = new ProdutoCell("Folhado Carne", (float)0.8);
            ProdutoCell p7 = new ProdutoCell("Folhado Chaves", (float)0.8);
            ProdutoCell p8 = new ProdutoCell("Folhado Frango", (float)0.8);
            ProdutoCell p9 = new ProdutoCell("Baguete de Atum", (float)0.9);
            ProdutoCell p10 = new ProdutoCell("Baguete de Frango", (float)0.95);
            ProdutoCell p11 = new ProdutoCell("Hamburguer", (float)1.5);
            ProdutoCell p12 = new ProdutoCell("Cachorro", (float)1.2);
            ProdutoCell p13 = new ProdutoCell("Folhado Misto", (float)0.8);
            ProdutoCell p14 = new ProdutoCell("Folhado Carne", (float)0.8);
            ProdutoCell p15 = new ProdutoCell("Folhado Chaves", (float)0.8);
            ProdutoCell p16 = new ProdutoCell("Folhado Frango", (float)0.8);
            ProdutoCell p17 = new ProdutoCell("Baguete de Atum", (float)0.9);
            ProdutoCell p18 = new ProdutoCell("Baguete de Frango", (float)0.95);
            ProdutoCell p19 = new ProdutoCell("Hamburguer", (float)1.5);
            ProdutoCell p20 = new ProdutoCell("Cachorro", (float)1.2);
            ProdutoCell p21 = new ProdutoCell("Folhado Misto", (float)0.8);
            ProdutoCell p22 = new ProdutoCell("Folhado Carne", (float)0.8);
            ProdutoCell p23 = new ProdutoCell("Folhado Chaves", (float)0.8);
            ProdutoCell p24 = new ProdutoCell("Folhado Frango", (float)0.8);
            produtos = new List<ProdutoCell>();
            produtos.Add(p);
            produtos.Add(p2);
            produtos.Add(p3); produtos.Add(p4); produtos.Add(p5); produtos.Add(p6); produtos.Add(p7); produtos.Add(p8); produtos.Add(p9); produtos.Add(p10);
            produtos.Add(p11); produtos.Add(p12); produtos.Add(p13); produtos.Add(p14); produtos.Add(p15); produtos.Add(p16); produtos.Add(p17); produtos.Add(p18);
            produtos.Add(p19); produtos.Add(p20); produtos.Add(p21); produtos.Add(p22); produtos.Add(p23); produtos.Add(p24);

            ProdutosCarrinho.ItemsSource = produtos;
            float total = 0;
            for(int i = 0; i<produtos.Count; i++)
            {
                total += produtos[i].Preco;
            }

            quantiaTotal.Text = "Total = " + total + "€";
        }
    }
}
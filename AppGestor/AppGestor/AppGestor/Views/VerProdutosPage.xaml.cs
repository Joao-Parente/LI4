using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Net;

namespace AppGestor

{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerProdutosPage : ContentPage
    {
        List<Produto> produtos { get; set; }
        public VerProdutosPage()
        {
            InitializeComponent();

            Produto p = new Produto();
            Produto p2 = new Produto();
            Produto p3 = new Produto();
            Produto p4 = new Produto();
            Produto p5 = new Produto();
            produtos = new List<Produto>();
            produtos.Add(p); produtos.Add(p2); produtos.Add(p3); produtos.Add(p4); produtos.Add(p5); 
            ViewProdutos.ItemsSource = produtos;
        }

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Net;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerProdutosPage : ContentPage
    {
        List<Produto> Produtos { get; set; }

        public VerProdutosPage(List<Produto> Lista,string NomeCategoria)
        {
            InitializeComponent();
            
            this.Produtos = Lista;
            ViewProdutos.ItemsSource = Produtos;
            this.ContentPageTeste.Title = NomeCategoria;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //var p = ((ListView)sender).SelectedItem;

            Produto PSelecionado = (Produto) e.Item;

            PopupNavigation.Instance.PushAsync(new ProdutoInfoPopUp(PSelecionado));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void CarrinhoComprasBotao(object sender, EventArgs e)
        {
            
            /*
            //Socket master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            //master.Connect(ipe);
            try
            {
                //TcpClient client = new TcpClient("127.0.0.1", 12345);
                //client.Connect("127.0.0.1",12345);
                //client.Connect("10.0.2.2", 12345);
                Socket master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("10.0.2.2"), 12345);
                master.Connect(ipe);
                NetworkStream s = new NetworkStream(master);
                StreamReader input = new StreamReader(s, Encoding.UTF8);
                StreamWriter output = new StreamWriter(s);

                output.WriteLine("GetListaProdutos");
                output.Flush();
                Thread.Sleep(500);
                string prods = input.ReadLine();
                int a = 567;
                String[] partes = prods.Split('|');
                List<Produto> hello = new List<Produto>();
                for(int i = 0; i < partes.Length; i++)
                {
                    String[] pr = partes[i].Split('_');
                    //Produto pro = new Produto(pr[0], float.Parse(pr[1]));
                    hello.Add(new Produto(pr[0], float.Parse(pr[1])));
                }
                ViewProdutos.ItemsSource = hello;
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            
            NetworkStream s = new NetworkStream(master);
            StreamReader input = new StreamReader(s, Encoding.UTF8);
            StreamWriter output = new StreamWriter(s);

            output.WriteLine("GetListaProdutos");
            output.Flush();
            Thread.Sleep(500);
            string prods = input.ReadLine();
            int a = 567;*/

            PopupNavigation.Instance.PushAsync(new CarrinhoCompras());
        }
    }
}
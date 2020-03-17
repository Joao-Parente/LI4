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
        List<Produto> produtos { get; set; }

        public VerProdutosPage()
        {
            InitializeComponent();

            Produto p = new Produto("Baguete de Atum", (float)0.9);
            Produto p2 = new Produto("Baguete de Frango", (float)0.95);
            Produto p3 = new Produto("Hamburguer", (float)1.5);
            Produto p4 = new Produto("Cachorro", (float)1.2);
            Produto p5 = new Produto("Folhado Misto", (float)0.8);
            Produto p6 = new Produto("Folhado Carne", (float)0.8);
            Produto p7 = new Produto("Folhado Chaves", (float)0.8);
            Produto p8 = new Produto("Folhado Frango", (float)0.8);
            Produto p9 = new Produto("Baguete de Atum", (float)0.9);
            Produto p10 = new Produto("Baguete de Frango", (float)0.95);
            Produto p11 = new Produto("Hamburguer", (float)1.5);
            Produto p12 = new Produto("Cachorro", (float)1.2);
            Produto p13 = new Produto("Folhado Misto", (float)0.8);
            Produto p14 = new Produto("Folhado Carne", (float)0.8);
            Produto p15 = new Produto("Folhado Chaves", (float)0.8);
            Produto p16 = new Produto("Folhado Frango", (float)0.8);
            Produto p17 = new Produto("Baguete de Atum", (float)0.9);
            Produto p18 = new Produto("Baguete de Frango", (float)0.95);
            Produto p19 = new Produto("Hamburguer", (float)1.5);
            Produto p20 = new Produto("Cachorro", (float)1.2);
            Produto p21 = new Produto("Folhado Misto", (float)0.8);
            Produto p22 = new Produto("Folhado Carne", (float)0.8);
            Produto p23 = new Produto("Folhado Chaves", (float)0.8);
            Produto p24 = new Produto("Folhado Frango", (float)0.8);
            produtos = new List<Produto>();
            produtos.Add(p);
            produtos.Add(p2);
            produtos.Add(p3); produtos.Add(p4); produtos.Add(p5); produtos.Add(p6); produtos.Add(p7); produtos.Add(p8); produtos.Add(p9); produtos.Add(p10);
            produtos.Add(p11); produtos.Add(p12); produtos.Add(p13); produtos.Add(p14); produtos.Add(p15); produtos.Add(p16); produtos.Add(p17); produtos.Add(p18);
            produtos.Add(p19); produtos.Add(p20); produtos.Add(p21); produtos.Add(p22); produtos.Add(p23); produtos.Add(p24);

            ViewProdutos.ItemsSource = produtos;
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //var p = ((ListView)sender).SelectedItem;

            Produto asasa = (Produto)e.Item;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void ServerFetch(object sender, EventArgs e)
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
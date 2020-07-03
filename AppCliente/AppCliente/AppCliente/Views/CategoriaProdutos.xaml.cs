using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriaProdutos : ContentPage
    {
        private LN ln;
        public string nomeCategoria { get; set; }
        public List<ProdutoInfo> produtos { get; set; }

        public CategoriaProdutos(string categoria, LN l)
        {
            this.ln = l;
            this.nomeCategoria = categoria;
            this.produtos = new List<ProdutoInfo>();
            List<Produto> p = this.ln.getProdutos()[categoria];
            for(int i = 0; i < p.Count; i++)
            {
                this.produtos.Add(new ProdutoInfo(p[i].id, p[i].nome, "" + p[i].preco + "€", 1,p[i].imagem));
            }
            InitializeComponent();
            this.BindingContext = this;
        }

        private void listaprodutos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            ((ListView)sender).SelectedItem = null;

            ProdutoInfo pi = (ProdutoInfo) e.Item;

            PopupNavigation.Instance.PushAsync(new ProdutoPage(ln,ln.getProduto(pi.Id)));
        }

        private void PlusButton_Clicked(object sender, EventArgs e)
        {
            ProdutoInfo pi = (ProdutoInfo)((Button)sender).BindingContext;

            ln.adicionaProdutoCarrinho(ln.getProduto(pi.Id));
            DisplayAlert("Sucesso", "Produto Adicionado com Sucesso!", "OK");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProdutoPage
    {
        public string NomeProduto { get; set; }
        public byte[] imagem { get; set; }
        public string Detalhes { get; set; }
        public string Preco { get; set; }

        private Produto p;
        private LN ln;

        public ProdutoPage(LN l,Produto pr)
        {
            this.ln = l;
            this.p = pr;
            InitializeComponent();
            NomeProduto = p.nome;
            imagem = null;// p.imagem;
            Detalhes = p.detalhes;
            Preco = "Preço: " + p.preco + "€";
            this.BindingContext = this;
        }

        private void AdicionarCarrinho_Clicked(object sender, EventArgs e)
        {
            ln.adicionaProdutoCarrinho(this.p);
            DisplayAlert("Sucesso", "Produto Adicionado com Sucesso!", "OK");
        }

        private void AdicionarFavoritos_Clicked(object sender, EventArgs e)
        {
            if (ln.NovoProdutoFavorito(this.p.id) == true)
            {
                DisplayAlert("Sucesso", "Produto Adicionado aos Favoritos com Sucesso!", "OK");
            }
            else
            {
                DisplayAlert("Erro", "Erro ao adicionar o produto aos favoritos", "OK");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProdutoPageFav
    {
        public string NomeProduto { get; set; }
        public ImageSource Imagem { get; set; }
        public string Detalhes { get; set; }
        public string Preco { get; set; }

        private LN ln;
        private Produto p;
        public ProdutoPageFav(LN l, Produto pr)
        {
            this.ln = l;
            this.p = pr;

            InitializeComponent();

            NomeProduto = p.nome;
            Imagem = ImageSource.FromStream(() => new MemoryStream(p.imagem));
            Detalhes = p.detalhes;
            Preco = "Preço: " + p.preco + "€";
            this.BindingContext = this;
        }

        private void RemoveFavButton_Clicked(object sender, EventArgs e)
        {

        }

        private void AdicionarCarrinho_Clicked(object sender, EventArgs e)
        {
            ln.adicionaProdutoCarrinho(this.p);
            DisplayAlert("Sucesso", "Produto Adicionado com Sucesso!", "OK");
        }
    }
}
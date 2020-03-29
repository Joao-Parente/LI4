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
    public partial class ProdutoInfoPopUp
    {
        private AppClienteLN LN { get; set; }
        private Produto ProdutoSelecionado { get; set; }
        public ProdutoInfoPopUp(AppClienteLN l,ProdutoCell p)
        {
            this.LN = l;
            ProdutoSelecionado = this.LN.GetProduto(p);
            InitializeComponent();
            NomeProduto.Text = p.Nome;
            Preco.Text = p.Preco;
        }

        private void AdicionaCarrinhoBotao(object sender, EventArgs e)
        {
            if (this.LN.ProdutoCarrinho(ProdutoSelecionado) == true)
            {
                DisplayAlert("Sucesso", "Produto Adicionado com Sucesso!", "OK");
            }
            else
            {
                DisplayAlert("Erro", "Já têm esse Produto no seu carrinho", "OK");
            }
        }
    }
}
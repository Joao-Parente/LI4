using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProdutoPage : INotifyPropertyChanged
    {
        private string _nomeproduto;
        private ImageSource _imagem;
        private string _detalhes;
        private string _preco;

        public string NomeProduto
        {
            get
            {
                return _nomeproduto;
            }
            set
            {
                _nomeproduto = value;
                NotifyPropertyChanged("nome");
            }
        }
        
        public ImageSource Imagem
        {
            get
            {
                return _imagem;
            }
            set
            {
                _imagem = value;
                NotifyPropertyChanged("imagem");
            }
        }

        public string Detalhes
        {
            get
            {
                return _detalhes;
            }
            set
            {
                _detalhes = value;
                NotifyPropertyChanged("detalhes");
            }
        }

        public string Preco
        {
            get
            {
                return _preco;
            }
            set
            {
                _preco = value;
                NotifyPropertyChanged("preco");
            }
        }

        private Produto p;
        private LN ln;

        public ProdutoPage(LN l,Produto pr)
        {
            this.ln = l;
            this.p = pr;
            InitializeComponent();
            _imagem = ImageSource.FromStream(() => new MemoryStream(p.imagem));
            _nomeproduto = p.nome;
            _detalhes = p.detalhes;
            _preco = "Preço: " + p.preco + "€";
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
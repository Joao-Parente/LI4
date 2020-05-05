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
    public partial class AutenticationPage : ContentPage
    {
        private LN ln;

        public AutenticationPage(LN l)
        {
            ln = l;
            InitializeComponent();
        }

        private async void criarcontaBotao(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CriarConta(ln));
        }

        private async void botaoConfirmar(object sender, EventArgs e)
        {

            if (ln.IniciarSessao(this.editorEmail.Text, this.editorPassword.Text))
            {
                ln.prodsFavoritos(this.editorEmail.Text);
                ln.setNomeCliente();
                App.Current.MainPage = new MarketPage(ln);
            }
            else
            {
                await DisplayAlert("Erro", "Email/Password errados, tente outra vez", "OK");
                this.editorEmail.Text = "";
                this.editorPassword.Text = "";
            }
        }

    }
}
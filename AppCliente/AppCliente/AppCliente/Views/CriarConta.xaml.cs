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
    public partial class CriarConta : ContentPage
    {
        LN ln;
        public CriarConta(LN l)
        {
            ln = l;
            InitializeComponent();
        }

        private async void botaoConfirmar(object sender, EventArgs e)
        {

            if (ln.RegistaUtilizador(this.editorEmail.Text, this.editorPassword.Text, this.editorNome.Text))
            {
                App.Current.MainPage = new MarketPage(ln);
            }
            else
            {
                await DisplayAlert("Erro", "Já existe um utilzador com o mesmo Email, por favor tente outra vez", "OK");
                this.editorEmail.Text = "";
                this.editorPassword.Text = "";
                this.editorNome.Text = "";
            }
        }
    }
}
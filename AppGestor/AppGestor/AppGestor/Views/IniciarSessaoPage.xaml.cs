using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppGestor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IniciarSessaoPage : ContentPage
    {
        private string email, password;
        private LN ln;
        public IniciarSessaoPage(LN l)
        {
            ln = l;
            InitializeComponent();
        }

        private void BotaoConfirmar(object sender, EventArgs e)
        {
            if(ln.iniciarSessao(emailEntry.Text, passwordEntry.Text) == true)
            {
                Application.Current.MainPage = new MenuCliente(this.ln);
            }
            else
            {
                DisplayAlert("Erro", "Email ou Password errado", "Ok");
            }

            // se verificar entao abre o novo
            
        }

    }
}
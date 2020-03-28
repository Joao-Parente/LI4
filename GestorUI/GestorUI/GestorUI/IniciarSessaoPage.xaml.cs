using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestorUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IniciarSessaoPage : ContentPage
    {
        private string email, password;
        public IniciarSessaoPage()
        {
            InitializeComponent();
        }

        private void BotaoConfirmar(object sender, EventArgs e)
        {
            this.email = emailEntry.Text;
            this.password = passwordEntry.Text;

            // se verificar entao abre o novo
            Application.Current.MainPage = new MenuCliente();
        }

        private void BotaoVoltar(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new MainPage());
            Navigation.PushModalAsync(new MainPage());
            
        }



    }
}
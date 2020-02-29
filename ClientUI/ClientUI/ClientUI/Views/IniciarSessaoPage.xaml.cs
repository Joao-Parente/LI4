using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientUI
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
        }

        private async void BotaoVoltar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }



    }
}
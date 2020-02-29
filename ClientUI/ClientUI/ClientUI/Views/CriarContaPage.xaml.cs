using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarContaPage : ContentPage
    {
        private string email, password;
        public CriarContaPage()
        {
            InitializeComponent();
        }

        private async void BotaoConfirmar(object sender, EventArgs e)
        {
            this.email = emailEntry.Text;
            this.password = passwordEntry.Text;
            string passwordConfirmation = passwordConfirmationEntry.Text;
            if (string.Compare(password,passwordConfirmation) != 0)
            {
                await Navigation.PushModalAsync(new MainPage());
            }
        }

        private async void BotaoVoltar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}
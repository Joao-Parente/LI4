using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarContaPage : ContentPage
    {
        private string Nome,Email,Password;
        private AppClienteLN LN;

        public CriarContaPage(AppClienteLN l)
        {
            this.LN = l;
            InitializeComponent();
        }

        private async void BotaoConfirmar(object sender, EventArgs e)
        {
            this.Nome = nomeEntry.Text;
            this.Email = emailEntry.Text;
            this.Password = passwordEntry.Text;
            if(this.LN.RegistaUtilizador(this.Email, this.Password, this.Nome) == true)
            {
                Application.Current.MainPage = new MenuCliente(this.LN);
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível efetuar o registo", "OK");
            }
        }

        private async void BotaoVoltar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}
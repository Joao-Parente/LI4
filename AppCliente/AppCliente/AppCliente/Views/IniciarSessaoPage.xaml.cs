using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IniciarSessaoPage : ContentPage
    {
        private string Email, Password;
        private AppClienteLN LN;
        public IniciarSessaoPage(AppClienteLN l)
        {
            this.LN = l;
            InitializeComponent();
        }

        private async void BotaoConfirmar(object sender, EventArgs e)
        {
            this.Email = emailEntry.Text;
            this.Password = passwordEntry.Text;
            if(LN.IniciarSessao(this.Email,this.Password) == true)
            {
                Application.Current.MainPage = new MenuCliente(this.LN);
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível iniciar sessão", "OK");
            }
        }

        private void BotaoVoltar(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new MainPage());
            Application.Current.MainPage = new MenuCliente(this.LN);
        }



    }
}
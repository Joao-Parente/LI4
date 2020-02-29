using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace ClientUI
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void CarregueiBotaoIniciarSessao(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new IniciarSessaoPage());
        }

        private async void CarregueiBotaoRegistar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CriarContaPage());
        }
    }
}

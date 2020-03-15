using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCliente
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private AppClienteLN ln;
        public MainPage()
        {
            InitializeComponent();
            this.ln = new AppClienteLN();
        }

        private void CarregueiBotaoIniciarSessao(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new IniciarSessaoPage(this.ln));
        }

        private void CarregueiBotaoRegistar(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CriarContaPage(this.ln));
        }
    }
}

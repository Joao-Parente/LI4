using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            bool isEmail = Regex.IsMatch(this.editorEmail.Text, @"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+.)+[a-z]{2,5}$", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                if (ln.RegistaUtilizador(this.editorEmail.Text, this.editorPassword.Text, this.editorNome.Text))
                {
                    ln.prodsFavoritos(this.editorEmail.Text);
                    ln.setNomeCliente();
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
            else
            {
                await DisplayAlert("Erro", "Não inseriu um Email válido, por favor tente outra vez", "OK");
                this.editorEmail.Text = "";
                this.editorPassword.Text = "";
                this.editorNome.Text = "";
            }
        }
    }
}
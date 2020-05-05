using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFuncionario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthenticationPage : ContentPage
    {
        private LN ln;
        public AuthenticationPage(LN l)
        {
            ln = l;
            InitializeComponent();
        }

        private void CarregueiBotaoIniciarSessao(object sender, EventArgs e)
        {
            if (ln.iniciarSessao(EntryEmail.Text, EntryPassword.Text) == true)
            {
                Navigation.PushModalAsync(new MenuPrincipal(ln));
            }
            else
            {
                EntryEmail.Text = "";
                EntryPassword.Text = "";
                DisplayAlert("Erro", "Email ou Password incorreto, por favor, tente outra vez!", "Ok");
            }
        }
    }
}
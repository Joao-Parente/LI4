using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppGestor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdicionarEmpregado
    {
        private LN ln;
        public AdicionarEmpregado(LN l)
        {
            ln = l;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string email = entryEmail.Text;
            string nome = entryNome.Text;
            string password = entryPassword.Text;

            if(ln.adicionarEmpregado(new Empregado(email, password, nome, false)) == true)
            {
                DisplayAlert("Sucesso", "Um novo empregado foi adicionado ao sistema", "Ok");
                PopupNavigation.Instance.PopAsync(true);
            }
            else
            {
                entryEmail.Text = "";
                entryNome.Text = "";
                entryPassword.Text = "";
            }

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}
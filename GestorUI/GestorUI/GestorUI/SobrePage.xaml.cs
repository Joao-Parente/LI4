using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestorUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SobrePage : ContentPage
    {
        public SobrePage()
        {
            InitializeComponent();
        }

        private void BotaoEditar(object sender, EventArgs e)
        {
    
            Navigation.PushModalAsync(new EditarSobre(nome.Text, horariosemanal.Text, horariofimsemana.Text, endereco.Text, website.Text, contactos.Text));
        }
    }
}
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
    public partial class EditarSobre : ContentPage
    {
        private Page p;

        public EditarSobre(string nome, string horariosemanal, string horariofimsemana, string endereco, string website, string contactos)
        {
            
            InitializeComponent();
            nomeEntry.Text = nome;
            horariosemanalEntry.Text = horariosemanal;
            horariofimsemanaEntry.Text = horariofimsemana;
            enderecoEntry.Text = endereco;
            websiteEntry.Text = website;
            contactosEntry.Text = contactos;


        }
         private void BotaoCancelar(object sender, EventArgs e)
          {


            //  await Navigation.PopModalAsync();
            Navigation.PopModalAsync();

        }

         private void BotaoGuardar(object sender, EventArgs e)
          {



            Navigation.PopModalAsync();


          }



    }
}
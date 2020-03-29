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
    public partial class GerirEmpregados : ContentPage
    {
        public GerirEmpregados()
        {
            InitializeComponent();
            List<Empregado>  em= new List<Empregado>();
            em.Add(new Empregado("augusto"));
            em.Add(new Empregado("manuel"));
            em.Add(new Empregado("joaquim"));


            ViewEmpregados.ItemsSource = em;
        }
    }
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarraLateral : Shell
    {
        LN ln;
        public BarraLateral(LN l)
        {
            ln = l;
            InitializeComponent();

            pagPrincipal.Content = new MarketPage(ln);
            pant.Content = new HistoricoPage(ln);
            ppen.Content = new EstadoPedidos(ln);
            about.Content = new MarketPage(ln);

        }
    }
}
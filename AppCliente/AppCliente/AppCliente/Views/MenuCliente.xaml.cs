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
    public partial class MenuCliente : Shell
    {

        private AppClienteLN LN;

        public MenuCliente(AppClienteLN ln)
        {
            this.LN = ln;

            InitializeComponent();

            VerCategorias.Content = new VerCategorias(this.LN);
            VerProdutosFavoritos.Content = new ListaDeFavoritos();
            VerHistoricoPedidos.Content = new HistoricoPage();
            VerAvaliacoes.Content = new AvaliacaoPage();
            VerSobre.Content = new SobrePage();
        }

    }
}
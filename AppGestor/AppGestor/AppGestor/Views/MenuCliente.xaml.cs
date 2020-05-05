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
    public partial class MenuCliente : Shell
    {
        private LN ln;
        public MenuCliente(LN l)
        {
            this.ln = l;
            InitializeComponent();
            verProdutos.Content = new VerProdutosPage();
            adicionarProds.Content = new AdicionarProdutos();
            gerirEmpregs.Content = new GerirEmpregados(ln);
            stats.Content = new Estatisticas();
            feedB.Content = new Feedback();
            infoP.Content = new SobrePage();
        }

    }
}
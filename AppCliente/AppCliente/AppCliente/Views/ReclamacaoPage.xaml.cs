using Rg.Plugins.Popup.Services;
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
    public partial class ReclamacaoPage
    {
        private LN ln;
        private DateTime dt;
        public ReclamacaoPage(LN l, DateTime t)
        {
            ln = l;
            dt = t;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //enviar para o servidor
            string rec = detailsEntry.Text;
            ln.reclamacao(ln.getIdPedido(this.dt), "null", rec);
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}
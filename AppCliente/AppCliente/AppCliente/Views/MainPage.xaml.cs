using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCliente
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        LN ln;
        string idP;
        public MainPage(string idPlayer)
        {
            InitializeComponent();
            this.idP = idPlayer;
            Thread t = new Thread(carregaDados);
            t.Start();
        }

        public void carregaDados()
        {
            //Thread.Sleep(1500);
            ln = new LN();
            ln.setIdsNotfs(idP);
            ln.setProdutos(ln.verProdutos());
            ln.preencheCategorias();
            ln.inicializaProdutosMAP();           
            Device.BeginInvokeOnMainThread(() =>
            {
                App.Current.MainPage = new AutenticationPage(this.ln);
            });
            
        }

    }

}

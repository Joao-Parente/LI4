using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppFuncionario
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private LN ln;
        public MainPage()
        {            
            InitializeComponent();
            Thread t = new Thread(carregaDados);
            t.Start();
        }

        public void carregaDados()
        {
            Thread.Sleep(1500);
            ln = new LN();
            ln.carregaProdutos();
            ln.preencheCategorias();
            ln.inicializaSocketandThreadPedidos();
            Device.BeginInvokeOnMainThread(() =>
            {
                App.Current.MainPage = new AuthenticationPage(ln);
            });

        }
    }
}

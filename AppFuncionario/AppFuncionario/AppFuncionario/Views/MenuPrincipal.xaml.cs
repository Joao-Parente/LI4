using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFuncionario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPrincipal : ContentPage
    {
        private LN ln;
        public ObservableCollection<PedidoInfo> Pedidos { get; set; }

        public MenuPrincipal(LN l)
        {
            ln = l;
            Pedidos = ln.getObCPedidos();            

            InitializeComponent();

            ln.label_para_num_pedido(this.PedidoAtualLabel);

            this.BindingContext = this;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            PedidoInfo pi = (PedidoInfo)e.Item;

            PopupNavigation.Instance.PushAsync(new InfoPedido(ln, pi, ln.getProdutos_Pedido(pi)));
            
            ((ListView)sender).SelectedItem = null;
        }

        private void MudarEstadoButton_Clicked(object sender, EventArgs e)
        {
            Button s = (Button)sender;
            PedidoInfo pi = (PedidoInfo)((Button)sender).BindingContext;
            ln.muda_estado_pedido(pi);
            //s.Text = pi.estado;
            switch (pi.estado)
            {
                case "Em Preparação":
                    s.Text = "Pronto a Levantar";
                    break;
                case "Pronto a Levantar":
                    s.Text = "Pedido Entregue";
                    break;
                //ln.removePedido(pi);
                case "Pedido Entregue":
                    ln.removePedido(pi);
                    break;
                
            }
        }

        private void botao_sistema_Clicked(object sender, EventArgs e)
        {
            if (this.ln.alternarEstadoSistema() == true)
            {
                botao_sistema.Text = "Desligar Sistema";
            }
            else
            {
                botao_sistema.Text = "Inicializar Sistema";
            }
        }

        private async void voltar_a_tras(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var exit = await this.DisplayAlert("Sair", "Têm a certeza que deseja terminar sessão?", "Sim", "Não").ConfigureAwait(false);
                if (exit)
                {
                    //App.Current.MainPage = new AuthenticationPage(ln);
                    Navigation.PopModalAsync(true);
                }
            });
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var exit = await this.DisplayAlert("Sair", "Têm a certeza que deseja sair da aplicação?", "Sim", "Não").ConfigureAwait(false);

                if (exit)
                {                    
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                }

            });
            return true;
        }
    }
}
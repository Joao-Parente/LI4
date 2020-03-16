using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;


/*
 * Isto é só uma classe de teste, para ver se conseguia mudar o numero do ultimo pedido e o meu pedido automaticamente
 * Sempre que se fizer alguma alteração no UI, isto é, nos Labels/Botões/Imagens etc, é preciso chamar
 */

namespace AppCliente
{
    public class ThreadPedidosTracker
    {
        Label ultimoPedido, meuPedido;
        int u, m;

        public ThreadPedidosTracker(Label a,Label b)
        {
            ultimoPedido = a;
            meuPedido = b;
            u = 0;
            m = 0;
        }

        public void run()
        {
            while (true)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ultimoPedido.Text = "" + u;
                    meuPedido.Text = "" + m;
                });
                u++; m++;
                Thread.Sleep(2000);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Collections.ObjectModel;

namespace AppFuncionario
{
    public class ThreadNovosPedidos
    {
        public ListView listView { get; set; }
        public ObservableCollection<Pedido> pds { get; set; }

        public ThreadNovosPedidos(ListView v, ObservableCollection<Pedido> p)
        {
            this.listView = v;
            this.pds = p;
        }

        public void run()
        {
            while (true)
            {
                //List<Pedido> ret = new List<Pedido>();
                Random r = new Random();
                List<Produto> produtos = new List<Produto>();
                produtos.Add(new Produto("" + r.Next(100) + "hello", (float)r.NextDouble()));
                produtos.Add(new Produto("" + r.Next(350) + "fromotherside", (float)r.NextDouble()));
                pds.Add(new Pedido(1,DateTime.Now.ToString(),"sem atuna"+ r.Next(100),DateTime.Now,produtos));
                pds.Add(new Pedido(1, DateTime.Now.ToString(), "sem atuna" + r.Next(100), DateTime.Now, produtos));/*
                pds.Add(new Pedido(1, DateTime.Now.ToString(), "sem atuna" + r.Next(100), DateTime.Now, produtos));
                pds.Add(new Pedido(1, DateTime.Now.ToString(), "sem atuna" + r.Next(100), DateTime.Now, produtos));
                ret.Add(new Pedido(1, DateTime.Now.ToString(), "sem atuna" + r.Next(100), DateTime.Now, produtos));
                ret.Add(new Pedido(1, DateTime.Now.ToString(), "sem atuna" + r.Next(100), DateTime.Now, produtos));
                ret.Add(new Pedido(1, DateTime.Now.ToString(), "sem atuna" + r.Next(100), DateTime.Now, produtos));*/
                /*
                Device.BeginInvokeOnMainThread(() =>
                {
                    //listView.ItemsSource = null;
                    listView.ItemsSource = ret;
                    //listView.BeginRefresh();
                });*/

                Thread.Sleep(10000);
            }
        }
    }
}

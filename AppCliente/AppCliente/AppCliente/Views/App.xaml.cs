using Com.OneSignal;
using Com.OneSignal.Abstractions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCliente
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string pl="", a="";

            OneSignal.Current.StartInit("56439dce-6b9f-4419-9f88-6ef9d32ed4ea").EndInit();
            
            OneSignal.Current.IdsAvailable(new Com.OneSignal.Abstractions.IdsAvailableCallback((playerID, pushToken) =>
            {
                pl = playerID;
                a = pushToken;
            }));

            MainPage = new MainPage(pl);

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

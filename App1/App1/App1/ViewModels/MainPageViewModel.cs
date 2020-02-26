using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class MainPageViewModel : ContentPage
    {
        Button b;
        public MainPageViewModel()
        {
            
            FelixCommand = new Command(() =>
            {
                b = new Button
                {
                    Text = "Felixxx",
                    TextColor = Color.White,
                    BackgroundColor = Color.Blue,
                    HorizontalOptions = LayoutOptions.Start
                    //Grid.SetColumn="5"
                };

            });
        }

        //public event ProgressChangedEventHandler PropertyChanged;
        public Command FelixCommand
        {
            get;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Bond1.Droid
{
    public class Client : ContentPage
    {
        public Client()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Welcome to Xamarin Forms!" }
                }
            };
        }
    }
}
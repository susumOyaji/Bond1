using System;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Bond1
{
    public partial class App : Application
    {
        public App()
        {
            // The root page of your application
            var content = new ContentPage
            {
                Title = "Bond",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms!"

                        }

                    }
                }

        };




            MainPage = new SelectDisp();
            //MainPage = new NavigationPage(new SelectDisp());
        }

       


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

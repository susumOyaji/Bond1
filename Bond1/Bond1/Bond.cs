﻿using System;
using System.Threading;
using System.Threading.Tasks;


using Xamarin.Forms;

namespace Bond1
{
    public class App : Application
    {
        public App()
        {
            //DependencyService.Get<TcpIpSocket1>().ClientConnect();

            DependencyService.Get<ITcpSocket1>().ServerConnect();
            Task<string> Anser = DependencyService.Get<ITcpSocket1>().ClientConnect();

           
            Button AnserButton = new Button
            {
                Text = "Ans= " + Anser
            };




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

                        },AnserButton

                    }
                }
            };




            //MainPage = new Seting();
            MainPage = new NavigationPage(content);
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

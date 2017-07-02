using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bond1
{
    public partial class SelectDisp : ContentPage
    {

        public SelectDisp()
        {
            //Padding = new Thickness(5, Device.OnPlatform(30, 10, 10), 10, 10);
            double top;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    top = 20;
                    break;
                case Device.Android:
                case Device.WinPhone:
                case Device.Windows:
                default:
                    top = 10;
                    break;
            }
            /*layout.Margin*/
            Padding = new Thickness(5, top, 5, 0);
            BackgroundColor = Color.Aquamarine;

           
            var menu1 = new ToolbarItem { Text = "Client" };
            menu1.Clicked += async (s, a) =>
            {     // <-1
                var Ans = await DisplayAlert("Selected", ((ToolbarItem)s).Text, "NO", "OK");
                if (!Ans)
                {
                    Application.Current.MainPage = new Client();
                }
            };

            ToolbarItems.Add(menu1);
              var menu2 = new ToolbarItem { Text = "Server" };
            menu2.Clicked += async (s, a) =>
            {     // <-1
                var Ans = await DisplayAlert("Selected", ((ToolbarItem)s).Text, "NO", "OK");
                if (!Ans)
                {
                    Application.Current.MainPage = new Server();
                }
            };
            ToolbarItems.Add(menu2);



            DateTime time = DateTime.Now;//new System.DateTime("yyyy", 1, 1, 0, 0, 0, 0);

            // Imageビューの生成
            var image = new Image { Aspect = Aspect.AspectFit };
            image.Source = ImageSource.FromFile("Menu2.png");
            
            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            image.Source = "Menu2.png";



            Label CommentDisp = new Label()
            {
                Text = "\nClient Mode or Server Mode Select\nServer to priSetup to Go",
                FontSize = 20,
                //BackgroundColor = Color.White,
                TextColor = Color.Black,
                //WidthRequest = 180,
                //HeightRequest = 40,
            };


            StackLayout SelectDisp = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Vertical,
                //BackgroundColor =Color.Gray,
                Children = {
                        image,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Orientation = StackOrientation.Vertical,
                        //BackgroundColor =Color.Gray,
                        Children = {
                            CommentDisp,
                            //IpaEntry,SendIpa
                        }
                    }
                }
            };
            Content = SelectDisp;

                     
        }

    }
}
      






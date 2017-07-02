using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Bond1
{
    public partial class Client : ContentPage
    {
        public  Client()
        {
            ClientReturn();
        }


        public string ClientReturn()
        {
            // Imageビューの生成
            var image = new Image { Aspect = Aspect.AspectFit };
            image.Source = ImageSource.FromFile("Menu.png");

            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            image.Source = "Menu.png";



            var ipAdr = DependencyService.Get<ITcpSocket1>();
            string ip = ipAdr.getIPAddress();

            Label IpaDisp = new Label
            {
                Text = ip,
                FontSize = 30,
                //Font = Font.SystemFontOfSize(NamedSize.Large).WithAttributes(FontAttributes.Bold),

                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40,

            };


            Entry IpaEntry = new Entry
            {
                Text = null,
                //Placeholder = "Server",
                Keyboard = Keyboard.Plain,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40
            };


            Button SendIpa = new Button
            {
                Text = "接続",
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40

            };
            SendIpa.Clicked += SendIpaButton_Clicked;




            StackLayout ClientDisp = new StackLayout()
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
                            IpaDisp,
                            IpaEntry,SendIpa
                        }
                    }
                }
            };
            Content = ClientDisp;





            void SendIpaButton_Clicked(object sender, EventArgs e)
            {
                
                var network = DependencyService.Get<ITcpSocket1>();
                Task<string> ClientAns = network.ClientConnect();

                bool speak = DependencyService.Get<ITcpSocket1>().IsConnected;//: ? "You are Connected" : "You are Not Connected";
            }
            return "Client Mode ";// + ClientAns + speak;
        }

    }
}
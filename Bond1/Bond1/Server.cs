using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bond1
{
    public class Server : ContentPage
    {
        public Server()
        {
            ServerReturn();
        }


        public void ServerReturn()
        {
            // Imageビューの生成
            var image = new Image { Aspect = Aspect.AspectFit };
            image.Source = ImageSource.FromFile("Menu.png");

            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            image.Source = "Menu.png";




            var ipAdr = DependencyService.Get<ITcpSocket1>();
            string ip = ipAdr.getIPAddress();

            Label ServerIpaDisp = new Label
            {
                Text = ip,
                FontSize = 15,
                //Font = Font.SystemFontOfSize(NamedSize.Large).WithAttributes(FontAttributes.Bold),

                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40,

            };


            Label ServerState = new Label
            {
                Text = "接続待機中",
                //Placeholder = "接続待機中",
                //Keyboard = Keyboard.Plain,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40
            };



            StackLayout ServerDisp = new StackLayout()
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
                            ServerIpaDisp,
                            ServerState,
                        }
                    }
                }
            };
            Content = ServerDisp;
           


            var network = DependencyService.Get<ITcpSocket1>();
           
            Task<string> networkAnser = network.ClientConnect1();
            //DependencyService.Get<ITcpSocket1>().ServerConnect1();//:? "You are Connected" : "You are Not Connected";
            
            //return "Server Mode";
        }
    }
}
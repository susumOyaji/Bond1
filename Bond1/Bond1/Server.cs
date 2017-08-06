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
        Label labelLatLon;// = new Label();
        Label ServerState;
        string Model;

        public Server()
        {
            BackgroundColor = Color.LightSeaGreen;
            ServerReturn();
        }


        public void ServerReturn()
        {
            // Imageビューの生成
            var Serverimage = new Image { Aspect = Aspect.AspectFit };
            Serverimage.Source = ImageSource.FromFile("Server.png");

            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            Serverimage.Source = "Server.png";




            var ipAdr = DependencyService.Get<ITcpSocket1>();
            string ip = ipAdr.getIPAddress();

            Label ServerIpaDisp = new Label
            {
                Text = ip,
                FontSize = 15,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40,

            };


            ServerState = new Label
            {
                Text = "接続待機中",
                //Placeholder = "接続待機中",
                //Keyboard = Keyboard.Plain,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40
            };


            labelLatLon = new Label()
            {
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40

            };


           
            AbsoluteLayout absoluteLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            // ラベルを配置
            absoluteLayout.Children.Add(Serverimage);
            absoluteLayout.Children.Add(ServerIpaDisp);
            absoluteLayout.Children.Add(ServerState); // 中心
            absoluteLayout.Children.Add(labelLatLon);
           

            // 中心
            AbsoluteLayout.SetLayoutFlags(Serverimage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(Serverimage, new Rectangle(0.5, 0.1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(ServerIpaDisp, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(ServerIpaDisp, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(ServerState, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(ServerState, new Rectangle(0.5, 0.6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(labelLatLon, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(labelLatLon, new Rectangle(0.5, 0.7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

          
            // absoluteLayoutを配置
            Content = absoluteLayout;
            //Content = SelectDisp;
            //Content = ClientDisp;


            //string network = DependencyService.Get<ITcpSocket1>().SeverToReceive();
            //ServerState.Text = network;

            SeverWaitTimer();
            //GpsOn();
            //Task<string> networkAnser = network.ClientConnect();
            //DependencyService.Get<ITcpSocket1>().ServerConnect1();//:? "You are Connected" : "You are Not Connected";

            //return "Server Mode";
        }



        // 追加ここから ---
        void GpsOn()
        {
            var geoLocator = DependencyService.Get<IGeolocator>();

            geoLocator.LocationReceived += (_, args) =>
            {
                labelLatLon.Text = String.Format("{0:0.00}/{1:0.00}", args.Latitude, args.Longitude);
            };

            geoLocator.StartGps();
        }
        // --- 追加ここまで

        void SeverWaitTimer()
        {
            int count = 0;
            Device.StartTimer(TimeSpan.FromSeconds(1.0), () =>
            {
                    // Do something
                    Model = DependencyService.Get<ITcpSocket1>().SeverToReceive();
                if (Model == null ^ Model == "Non Anser!")
                {
                    labelLatLon.Text = "受信待機中!  " + Model + count++;
                    return true;
                }
                else
                {
                    labelLatLon.Text = "受信完了   " + "\n" + Model;
                    return false; // True = Repeat again, False = Stop the timer 
                    }

            });

        }

    }

   





}
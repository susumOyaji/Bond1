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
                    top = 0;
                    break;
            }
            /*layout.Margin*/
            Padding = new Thickness(5, top, 5, 0);
            BackgroundColor = Color.DarkRed;

            DateTime time = DateTime.Now;//new System.DateTime("yyyy", 1, 1, 0, 0, 0, 0);

            // Imageビューの生成
            var image = new Image { Aspect = Aspect.AspectFit };
            image.Source = ImageSource.FromFile("Menu.png");
            
            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            image.Source = "Menu.png";



            //var image = new Image
            //{
            //    // 画像を読み込んでSourceプロパティに設定
            //    Source = ImageSource.FromResource("Manu.png"),
            //};

            var ipAdr = DependencyService.Get<ITcpSocket1>();
            string ip = ipAdr.getIPAddress();

            Label IpaDisp = new Label
            {
                Text = ip,
                FontSize = 15,
                //Font = Font.SystemFontOfSize(NamedSize.Large).WithAttributes(FontAttributes.Bold),
               
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40,
                
            };



            //void ClientButton_Clicked(object sender, EventArgs e)
            //{
            //    Client td = new Client(); //オブジェクト作成
            //    string Ans = td.ClientReturn();
            //    ClientButton.Text = Ans;
            //}

            // ホスト名を取得する
            //string hostname = Dns.GetHostName();

            // ホスト名からIPアドレスを取得する
            //IPAddress[] adrList = Dns.GetHostAddresses(hostname);
           



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
                Text = "送信",
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40

            };
            SendIpa.Clicked += SendIpaButton_Clicked;



            void SendIpaButton_Clicked(object sender, EventArgs e)
            {
                if (IpaEntry.Text == null)
                {
                    //to Client mode()
                    Client cl = new Client();
                    IpaEntry.Text = cl.ClientReturn();
                }
                else
                {
                    //to Server mode()
                    Server se = new Server();
                    IpaEntry.Text =se.ServerReturn();
                }


                Client td = new Client(); //オブジェクト作成
            }






            StackLayout Date = new StackLayout()
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
            Content = Date;
        }

    }
}
      






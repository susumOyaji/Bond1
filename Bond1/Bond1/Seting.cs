using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Bond1
{
    public class Seting : ContentPage
    {
       public Seting()
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
                    top = 30;
                    break;
            }
            /*layout.Margin*/Padding = new Thickness(5, top, 5, 0);


            DateTime time = DateTime.Now;//new System.DateTime("yyyy", 1, 1, 0, 0, 0, 0);

            BackgroundColor = Color.DarkRed;

            var getClient = DependencyService.Get<ClientSocket>();
            //var Guest = DependencyService.Get<GuestSocket>();
            var Tcp = DependencyService.Get<TcpIpSocket>();
           
            //Client.createReceiveUdpSocket();//①ホスト：ゲストから発信されるブロードキャストを受信できる(受信待ち受け)状態にする。
            Task <string> Ipa = getClient.WaitToGuestConnect();//②ゲスト：ホスト探索開始。ゲスト端末のIPアドレスを発信する。
            string a = Ipa.ToString();
         





            Button Client = new Button
            { Text = a, TextColor = Color.Black, WidthRequest = 80, HeightRequest = 40 };

            Button Guest = new Button
            { Text = "Guest", TextColor = Color.Black, WidthRequest = 80, HeightRequest = 40 };


            StackLayout Date = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                //BackgroundColor =Color.Gray,
                Children = {
                    new Label
                    {
                        Text = time.ToString("yyyy年MM月dd日    HH時mm分ss秒"),
                        TextColor = Color.White,  //Reloadtime(1);
                    },
                    new StackLayout()
                    {

                        Children = {
                            Client,
                            Guest
                        }
                    }
                }
            };
            Content = Date;
        }
                    
    }
      
}





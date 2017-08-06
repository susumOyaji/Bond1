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
        //string Model;

        public Client(string result)
        {
            BackgroundColor = Color.Aqua;
            ClientReturn(result);
        }


        public string ClientReturn(string result)
        {
            // Imageビューの生成
            var Clientimage = new Image { Aspect = Aspect.AspectFit};
            Clientimage.Source = ImageSource.FromFile("Client.png");

            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            Clientimage.Source = "Client.png";



            Label MyIp = new Label()
            {
                Margin = new Thickness(10, 0, 10, 0),//left, top, right, bottom
                Text = "This IP",
                FontSize = 15,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                WidthRequest = 110,
                HeightRequest = 30,
                BackgroundColor = Color.DarkGray,    
            };
           

            var ipAdr = DependencyService.Get<ITcpSocket1>();
            //string ip = ipAdr.getIPAddress();
            Label IpaDisp = new Label
            {
                Margin = new Thickness(10, 0, 10, 0),
                Text = ipAdr.getIPAddress(),
                FontSize = 15,
                BackgroundColor = Color.DarkGray,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //WidthRequest = 180,
                HeightRequest = 30,

            };

            //StartAndExpand： 縦並びではコントロールの下に（横並びでは右に）間隔を開ける
            //CenterAndExpand： 縦並びではコントロールの上下に（横並びでは左右に）間隔を開ける
            //EndAndExpand： 縦並びではコントロールの上に（横並びでは左に）間隔を開ける
            //FillAndExpand： 縦並びではコントロールを上下に（横並びでは左右に）拡張する




            Entry IpaEntry = new Entry
            {
                Margin = new Thickness(10, 0, 10, 0),
                Text = result,
                FontSize = 15,
                //Placeholder = "Server",
                Keyboard = Keyboard.Url,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = 180,
                HeightRequest = 30

            };



           


            Label GetInfo = new Label
            {
                Text = null,
                FontSize = 15,
              
                //BackgroundColor = Color.White,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                //WidthRequest = 50,
                //HeightRequest = 100
            };

            Button SendIpa = new Button
            {
                Margin = new Thickness(10, 0, 10, 0),
                Text = "接続",
                TextColor = Color.Black,
                BackgroundColor = Color.Gray,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                //WidthRequest = 100,
                HeightRequest = 50

            };
            SendIpa.Clicked += SendIpaButton_Clicked;





            AbsoluteLayout absoluteLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            // ラベルを配置
            absoluteLayout.Children.Add(Clientimage);
            absoluteLayout.Children.Add(MyIp);
            absoluteLayout.Children.Add(IpaDisp); // 中心
            absoluteLayout.Children.Add(IpaEntry);
            absoluteLayout.Children.Add(GetInfo);
            absoluteLayout.Children.Add(SendIpa);



            // 中心
            AbsoluteLayout.SetLayoutFlags(Clientimage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(Clientimage, new Rectangle(0.5, 0.1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(MyIp, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(MyIp, new Rectangle(0.1, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(IpaDisp, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(IpaDisp, new Rectangle(0.8, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(IpaEntry, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(IpaEntry, new Rectangle(0.1, 0.6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(SendIpa, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(SendIpa, new Rectangle(0.9, 0.6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(GetInfo, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(GetInfo, new Rectangle(0.1, 0.9, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // absoluteLayoutを配置
            Content = absoluteLayout;
            //Content = SelectDisp;
            //Content = ClientDisp;



          




            void SendIpaButton_Clicked(object sender, EventArgs e)
            {
                //Server IpAdressをセットして送る 
                string sendIp = "182.22.112.107";//IpaEntry.Text;
                if (sendIp != null)
                {
                    DependencyService.Get<ITcpSocket1>().SeverIpadressSet(sendIp,110);//SendIpaAdress(sendIp);
                    GetInfo.Text = sendIp;
                    WaitConect();

                }
                else
                {
                    GetInfo.Text = "Ipaデータが不正です！";
                    sendIp = null;
                    //var Ans = DependencyService.Get<ITcpSocket1>().ClientConnect();
                }
            }


            void WaitConect()
            {
                Device.StartTimer(TimeSpan.FromSeconds(1.0), () =>
                {
                    // Do something
                    var  Model = DependencyService.Get<ITcpSocket1>().SeverToConnect();
                    string ModelOk = Model.Substring(0, 3);

                    if (ModelOk.ToString() != "+OK")
                    {
                        GetInfo.Text = "サーバに接続中   ";
                        return true;
                    }
                    else
                    {
                        GetInfo.Text = "サーバに接続されました！ ";
                        string[] a = DependencyService.Get<ITcpSocket1>().GetConectIp();
                        GetInfo.Text = Model + "\n" + a[0] + "\n" + a[1] + "\n" + a[2] + "\n" + a[3];
                        return false; // True = Repeat again, False = Stop the timer 
                    }

                });

            }



            return "Client Mode ";// + ClientAns + speak;
        }

    }
}
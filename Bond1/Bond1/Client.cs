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

        public Client()
        {
            BackgroundColor = Color.BlueViolet;
            ClientReturn();
        }


        public string ClientReturn()
        {
            // Imageビューの生成
            var image = new Image { Aspect = Aspect.AspectFit};
            image.Source = ImageSource.FromFile("Menu2.png");

            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            image.Source = "Menu2.png";



            Label MyIp = new Label()
            {
                Margin = new Thickness(10, 0, 10, 0),//left, top, right, bottom
                Text = "THIS IP",
                FontSize = 15,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //WidthRequest = 110,
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
                Text = null,
                FontSize = 15,
                //Placeholder = "Server",
                Keyboard = Keyboard.Url,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                //WidthRequest = 180,
                HeightRequest = 30

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



            Label GetInfo = new Label
            {
                Text = null,
                FontSize = 15,
              
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                //WidthRequest = 50,
                //HeightRequest = 100
            };


            ///
            AbsoluteLayout absoluteLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // Labelを生成する
            Label label = new Label
            {
                Text = "中心(0.5,0.5)",
                FontSize = 22,
                TextColor = Color.Yellow,
            };

            // Labelを生成する
            Label label1 = new Label
            {
                Text = "左上(0,0)",
                FontSize = 24,
                TextColor = Color.Yellow,
            };

            // Labelを生成する
            Label label2 = new Label
            {
                Text = "右上(1,0)",
                FontSize = 24,
                TextColor = Color.Yellow,
            };

            // Labelを生成する
            Label label3 = new Label
            {
                Text = "左下(0,1)",
                FontSize = 24,
                TextColor = Color.Yellow,
            };

            // Labelを生成する
            Label label4 = new Label
            {
                Text = "右下(1,1)",
                FontSize = 24,
                TextColor = Color.Yellow,
            };

            // Labelを生成する
            Label label01 = new Label
            {
                Text = "上(0.5,0)",
                FontSize = 24,
                TextColor = Color.Yellow,
            };

            // Labelを生成する
            Label label02 = new Label
            {
                Text = "左中心(0,0.5)",
                FontSize = 22,
                TextColor = Color.Yellow,
            };

            // Labelを生成する
            Label label03 = new Label
            {
                Text = "右中心(1,0.5)",
                FontSize = 22,
                TextColor = Color.Yellow,
            };

            // Labelを生成する
            Label label04 = new Label
            {
                Text = "下(0.5,1)",
                FontSize = 24,
                TextColor = Color.Yellow,
            };

            // ラベルを配置
            absoluteLayout.Children.Add(image/* label*/); // 中心
            absoluteLayout.Children.Add(label1); // 左上
            absoluteLayout.Children.Add(label2); // 右上
            absoluteLayout.Children.Add(label3); // 左下
            absoluteLayout.Children.Add(label4); // 右下

            absoluteLayout.Children.Add(label01); // 上
            //absoluteLayout.Children.Add(ServerButton/*label02*/); // 左中心
            //absoluteLayout.Children.Add(ClientButton/*label03*/); // 右中心
            absoluteLayout.Children.Add(label04); // 下

            // 左上
            AbsoluteLayout.SetLayoutFlags(label1, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(label1, new Rectangle(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 上
            AbsoluteLayout.SetLayoutFlags(label01, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(label01, new Rectangle(0.5, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 右上
            AbsoluteLayout.SetLayoutFlags(label2, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(label2, new Rectangle(1, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 左中心
            //AbsoluteLayout.SetLayoutFlags(ServerButton/*label02*/, AbsoluteLayoutFlags.PositionProportional);
            //AbsoluteLayout.SetLayoutBounds(ServerButton/* label02*/, new Rectangle(0, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 中心
            AbsoluteLayout.SetLayoutFlags(image/* label*/, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(image/* label*/, new Rectangle(0.5, 0.0, 100/*AbsoluteLayout.AutoSize*/, 100/*AbsoluteLayout.AutoSize*/));

            // 右中心
            //AbsoluteLayout.SetLayoutFlags(ClientButton/*label03*/, AbsoluteLayoutFlags.PositionProportional);
            //AbsoluteLayout.SetLayoutBounds(ClientButton/*label03*/, new Rectangle(1.0, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 左下
            AbsoluteLayout.SetLayoutFlags(label3, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(label3, new Rectangle(0, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 下
            AbsoluteLayout.SetLayoutFlags(label04, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(label04, new Rectangle(0.5, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 右下
            AbsoluteLayout.SetLayoutFlags(label4, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(label4, new Rectangle(1, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // absoluteLayoutを配置
            //Content = absoluteLayout;
            //Content = SelectDisp;








       



            StackLayout ClientDisp = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                BackgroundColor =Color.White,
                Children = {
                        image,
                        //new StackLayout{
                        //HorizontalOptions = LayoutOptions.FillAndExpand,
                        //    VerticalOptions = LayoutOptions.Center,
                        //    Orientation = StackOrientation.Horizontal,
                        //    //BackgroundColor =Color.Gray,
                        //    Children = {
                        //       MyIp,IpaDisp,
                        //     }
                        //},
                        //new StackLayout{
                        //HorizontalOptions = LayoutOptions.FillAndExpand,
                        //    VerticalOptions = LayoutOptions.Center,
                        //    Orientation = StackOrientation.Horizontal,
                        //    Children = {
                        //        IpaEntry,SendIpa
                        //    }
                        //},GetInfo
                }

              };
          
            


            //Content = ClientDisp;
            Content = absoluteLayout;
            //Content = secondgrid;




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
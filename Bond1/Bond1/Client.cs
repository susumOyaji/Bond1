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
        string Model;

        public Client()
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
                FontSize = 15,
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
                //Server IpAdressをセットして送る 
                string sendIp = IpaEntry.Text;
                if (sendIp != null)
                {
                    //SendIpaAdress(sendIp);
                    WaitTimer();
                }
                else
                {
                    IpaEntry.Text = "Ipaデータが不正です！";
                    sendIp = null;
                }
            }


            void WaitTimer()
            {
                Device.StartTimer(TimeSpan.FromSeconds(1.0), () =>
                {
                // Do something
                    Model = DependencyService.Get<ITcpSocket1>().SeverToConnect();
                    if (Model == null ^ Model == "Non Anser!")
                    {
                        IpaEntry.Text = "サーバに接続中   " + Model;
                        return true;
                    }
                    else
                    {
                        IpaEntry.Text = "サーバに接続されました！   "+"\n" + Model;
                        return false; // True = Repeat again, False = Stop the timer 
                    }

                });

            }



            return "Client Mode ";// + ClientAns + speak;
        }

    }
}
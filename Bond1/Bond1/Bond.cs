﻿using System;
using System.Net;

using Xamarin.Forms;

namespace Bond1
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            var content = new ContentPage
            {
                

                Title = "UdpClient",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms!"
                        }
                    }
                }
            };
            var UdpReceive = DependencyService.Get<IUdpReceiveSocket>();
            var TcpReceive = DependencyService.Get<ITcpSocket>();

            UdpReceive.createReceiveUdpSocket();//①ホスト：ゲストから発信されるブロードキャストを受信できる(受信待ち受け)状態にする。
            UdpReceive.sendBroadcast();//②ゲスト：ホスト探索開始。ゲスト端末のIPアドレスを発信する。
            TcpReceive.connect();//②
            UdpReceive.returnIpAdress("IpAdress");//③ホスト：ゲストから受信したIPアドレスに対してホスト端末のIPアドレスを送り返す。
            TcpReceive.ConnectEnable();//④ゲスト：ホストから端末情報を受け取る
            //TcpReceive.Connect(String Adp);//⑤ゲスト：ホストのIPアドレスが判明して、TCP通信を開始する。


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

﻿using System;
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
            ClientReturn();
        }


        public string ClientReturn()
        {
            // Imageビューの生成
            var image = new Image { Aspect = Aspect.AspectFit };
            image.Source = ImageSource.FromFile("Menu.png");

            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            image.Source = "Menu.png";



            Label MyIp = new Label()
            {
                Text = "THIS IP",
                FontSize = 15,
                //BackgroundColor = Color.Black,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                //WidthRequest = 100,
                HeightRequest = 30
            };

            var ipAdr = DependencyService.Get<ITcpSocket1>();
            //string ip = ipAdr.getIPAddress();
            Label IpaDisp = new Label
            {
                Text = ipAdr.getIPAddress(),
                FontSize = 15,
                BackgroundColor = Color.Yellow,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                //WidthRequest = 180,
                HeightRequest = 30,

            };


            Entry IpaEntry = new Entry
            {
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
                //Placeholder = "Server",
                //Keyboard = Keyboard.Plain,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                //WidthRequest = 50,
                HeightRequest = 100
            };



            SendIpa.Clicked += SendIpaButton_Clicked;

            StackLayout ClientDisp = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Vertical,
                BackgroundColor =Color.Black,
                Children = {
                        //image,
                        new StackLayout{
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.Center,
                            Orientation = StackOrientation.Horizontal,
                            //BackgroundColor =Color.Gray,
                            Children = {
                                MyIp,IpaDisp,
                            }
                        },
                        new StackLayout{
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                            //VerticalOptions = LayoutOptions.Center,
                            Orientation = StackOrientation.Horizontal,
                            Children = {
                                IpaEntry,SendIpa
                            }
                        },GetInfo
                }

              };

            /// <summary>
            /// </summary>
            /// <param name="sender">Sender.</param>
            /// <param name="e">E.</param>

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                RowDefinitions =
            {
                new RowDefinition() { Height = GridLength.Auto }
            },
                ColumnDefinitions =
            {
                new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) },
            }
            };

            grid.Children.Add(new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 50,
                Text = "This My Ip",
                TextColor = Color.Black,
                BackgroundColor = Color.Aqua,
            }, 0, 0);

            grid.Children.Add(new Button
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "two two",
                TextColor = Color.Black,
                BackgroundColor = Color.Fuchsia,
            }, 1, 0);

            grid.Children.Add(new Button
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "three three three",
                TextColor = Color.Black,
                BackgroundColor = Color.Lime,
            }, 2, 0);








            //Content = ClientDisp;
            Content = grid;




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
                    IpaEntry.Text = "Ipaデータが不正です！";
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
                        IpaEntry.Text = "サーバに接続中   ";
                        return true;
                    }
                    else
                    {
                        IpaEntry.Text = "サーバに接続されました！ ";
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
using System;
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
            //image.Source = "６.png";



            //var image = new Image
            //{
            //    // 画像を読み込んでSourceプロパティに設定
            //    Source = ImageSource.FromResource("Manu.png"),
            //};


            Button ClientButton = new Button
            {
                Text = "Client ",
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40,
                
            };
            ClientButton.Clicked += ClientButton_Clicked;


            void ClientButton_Clicked(object sender, EventArgs e)
            {
                Client td = new Client(); //オブジェクト作成
                string Ans = td.ClientReturn();
                ClientButton.Text = Ans;
            }


            Button ServerButton = new Button
            {
                Text = "Server",
                TextColor = Color.Black,
                WidthRequest = 180,
                HeightRequest = 40
            };
            ServerButton.Clicked += ServerButton_Clicked;

            void ServerButton_Clicked(object sender, EventArgs e)
            {
                Client td = new Client(); //オブジェクト作成
            }






            StackLayout Date = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.End,
                Orientation = StackOrientation.Vertical,
                //BackgroundColor =Color.Gray,
                Children = {
                        //image,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        Orientation = StackOrientation.Vertical,
                        //BackgroundColor =Color.Gray,
                        Children = {
                            ClientButton,
                            ServerButton
                        }
                    }
                }
            };
            Content = Date;
        }

    }
}
      






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
            BackgroundColor = Color.OrangeRed;

           
            var menu1 = new ToolbarItem { Text = "Client" };
            menu1.Clicked += async (s, a) =>
            {     // <-1
                var Ans = await DisplayAlert("Selected", ((ToolbarItem)s).Text, "NO", "OK");
                if (!Ans)
                {
                    Application.Current.MainPage = new Client();
                }
            };

            ToolbarItems.Add(menu1);
              var menu2 = new ToolbarItem { Text = "Server" };
            menu2.Clicked += async (s, a) =>
            {     // <-1
                var Ans = await DisplayAlert("Selected", ((ToolbarItem)s).Text, "NO", "OK");
                if (!Ans)
                {
                    Application.Current.MainPage = new Server();
                }
            };
            ToolbarItems.Add(menu2);



            DateTime time = DateTime.Now;//new System.DateTime("yyyy", 1, 1, 0, 0, 0, 0);

            // Imageビューの生成
            var image = new Image { Aspect = Aspect.AspectFit };
            image.Source = ImageSource.FromFile("Menu2.png");
            
            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            image.Source = "Menu2.png";


            Button ServerButton = new Button()
            {
                Text = "Mode to Server",
                WidthRequest = 180,
            };
            ServerButton.Clicked +=  ServerButton_Clicked;


            Button ClientButton = new Button()
            {
                Text = "Mode tp Client",
                WidthRequest = 180,
            };
            ClientButton.Clicked += ClientButton_Clicked;



            Label CommentDisp = new Label()
            {
                Text = "\nClient Mode or Server Mode Select\nServer to priSetup to Go",
                FontSize = 20,
                //BackgroundColor = Color.White,
                TextColor = Color.Black,
                //WidthRequest = 180,
                //HeightRequest = 40,
            };

            async void ServerButton_Clicked(object sender, EventArgs e)
            {     // <-1
                var Anser = await DisplayAlert("Selected", "Server Mode", "NO", "OK");
                if (!Anser)
                {
                    Application.Current.MainPage = new Server();
                }    
           };
           
            async void ClientButton_Clicked(object sender, EventArgs e) 
           {     // <-1
                var Ans = await DisplayAlert("Selected", "Client Mode", "NO", "OK"); 
                if (!Ans) 
                {
                    Application.Current.MainPage = new Client();
               }
           };

            


            StackLayout SelectDisp = new StackLayout()
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
                            new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Start,
                                Orientation = StackOrientation.Horizontal,
                                Children ={
                                    ServerButton,
                                    ClientButton,
                                    //layout
                                }
                               
                            },
                             CommentDisp//IpaEntry,SendIpa
                        }
                    }
                }
            };





            AbsoluteLayout absoluteLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand//.FillAndExpand
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
            absoluteLayout.Children.Add(ServerButton/*label02*/); // 左中心
            absoluteLayout.Children.Add(ClientButton/*label03*/); // 右中心
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
            AbsoluteLayout.SetLayoutFlags(ServerButton/*label02*/, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(ServerButton/* label02*/, new Rectangle(0, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 中心
            AbsoluteLayout.SetLayoutFlags(image/* label*/, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(image/* label*/, new Rectangle(0.5, 0.0, 350/*AbsoluteLayout.AutoSize*/, 350/*AbsoluteLayout.AutoSize*/));

            // 右中心
            AbsoluteLayout.SetLayoutFlags(ClientButton/*label03*/, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(ClientButton/*label03*/, new Rectangle(1.0, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

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
            Content = absoluteLayout;
            //Content = SelectDisp;

        }
    }

                         
 }

    
      






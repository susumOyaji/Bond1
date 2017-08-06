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

           
          


            DateTime time = DateTime.Now;//new System.DateTime("yyyy", 1, 1, 0, 0, 0, 0);

            // Imageビューの生成
            var Serverimage = new Image { Aspect = Aspect.AspectFit };
            var ORimage = new Image { Aspect = Aspect.AspectFill };
            var Clientimage = new Image { Aspect = Aspect.AspectFit };

            Serverimage.Source = ImageSource.FromFile("Server.png");
            ORimage.Source = ImageSource.FromFile("OR.png");
            Clientimage.Source = ImageSource.FromFile("Client.png");
            
            ////さらに良いことに、暗黙の変換があるので、この行も機能します：
            Serverimage.Source = "Server.png";
            ORimage.Source = "OR.png";
            Clientimage.Source = "Client.png";

            var tgrserver = new TapGestureRecognizer();
            tgrserver.Tapped += (sender, e) => OnServerimageClicked(sender,e);
            Serverimage.GestureRecognizers.Add(tgrserver);

            var tgrclient = new TapGestureRecognizer();
            tgrclient.Tapped += (sender, e) => OnClientimageClicked(sender, e);
            Clientimage.GestureRecognizers.Add(tgrclient);
          

           
           
            //Button button = new Button(this.getActivity());
            //button.setText(test.getLabel());
            //button.setAllCaps(false);
            //layout.addView(button);


          

            Label CommentDisp = new Label()
            {
                Text = "\nTap either Client Mode or Server Mode\n",
                FontSize = 20,
               TextColor = Color.Black,
                //WidthRequest = 180,
                //HeightRequest = 40,
            };



            void OnServerimageClicked(object sender, EventArgs e)
            {
                //var result = await DependencyService.Get<IEntryAlertService>().Show("Server Mode!", "Please enter Server IpAdress", "OK", "Cancel", false);
                Application.Current.MainPage = new Server();
            }

            async void OnClientimageClicked(object sender, EventArgs e)
            {
               var result = await DependencyService.Get<IEntryAlertService>().Show("Client Mode!", "Please enter Server IpAdress", "OK", "Cancel", false);
               Application.Current.MainPage = new Client(result.Text);
            }




            AbsoluteLayout absoluteLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };

       
            // ラベルを配置
            absoluteLayout.Children.Add(Serverimage/* label*/);
            absoluteLayout.Children.Add(ORimage/* label*/); // 中心
            absoluteLayout.Children.Add(Clientimage/* label*/);
            absoluteLayout.Children.Add(CommentDisp);

           

            // 中心
            AbsoluteLayout.SetLayoutFlags(Serverimage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(Serverimage, new Rectangle(0.1, 0.3, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 中心
            AbsoluteLayout.SetLayoutFlags(ORimage/* label*/, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(ORimage/* label*/, new Rectangle(0.5, 0.3, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 中心
            AbsoluteLayout.SetLayoutFlags(Clientimage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(Clientimage, new Rectangle(0.9, 0.3, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            // 下
            AbsoluteLayout.SetLayoutFlags(CommentDisp, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(CommentDisp, new Rectangle(0.1, 0.8, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


            // absoluteLayoutを配置
            Content = absoluteLayout;
           
        }

       
    }
}


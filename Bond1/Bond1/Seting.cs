using System;
using Xamarin.Forms;


namespace Bond1
{
    public class Seting : ContentPage
    {
        private ContentPage content;

        public Seting()
        {

            Padding = new Thickness(5, Device.OnPlatform(30, 10, 10), 10, 10);
            DateTime time = DateTime.Now;//new System.DateTime("yyyy", 1, 1, 0, 0, 0, 0);

            BackgroundColor = Color.Black;


            StackLayout Date = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                //BackgroundColor =Color.Gray,
                Children = {
                    new Label
                    {
                        Text = time.ToString("yyyy年MM月dd日    HH時mm分ss秒"),
                        TextColor = Color.White,  //Reloadtime(1);
                    }
                }
            };
            Content = Date;
        }

        public Seting(ContentPage content)
        {
            this.content = content;
        }
    }
}

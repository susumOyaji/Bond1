using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bond1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Page1()
        {
           InitializeComponent();
        }

        private void ClientButton_Clicked(object sender, EventArgs e)
        {

        }

        private void SendIpa_Clicked(object sender, EventArgs e)
        {
          
        }

    }
}
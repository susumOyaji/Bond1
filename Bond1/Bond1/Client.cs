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
        public  Client()
        {
           
        }


        public string ClientReturn()
        {
            var network = DependencyService.Get<ITcpSocket1>();
            Task<string> ClientAns = network.ClientConnect();

            bool speak = DependencyService.Get<ITcpSocket1>().IsConnected;//: ? "You are Connected" : "You are Not Connected";

            return "Client Ans+ " + ClientAns + speak;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            //DependencyService.Get<TcpIpSocket1>().ClientConnect();

            var network = DependencyService.Get<ITcpSocket1>();
            network.ClientConnect();
            //Task<string> networkAnser = network.ClientConnect();
            bool speak = DependencyService.Get<ITcpSocket1>().IsConnected;// ? "You are Connected" : "You are Not Connected";
            return "Client Ans";
        }




    }
}
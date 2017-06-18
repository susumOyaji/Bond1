using System;
using System.Threading.Tasks;


namespace Bond1
{
    public interface IUdpReceiveSocket
    {
        Task createReceiveUdpSocket();
        void sendBroadcast();
        void returnIpAdress(string address);
        //void createReceiveUdpSocket();
        //void createReceiveTcpSocket();
    }

    public interface ITcpSocket
    {
        void connect();
        void ConnectEnable();
        //void inputDeviceNameAndIp(Socket socket);
        //void Connect(string remoteIpAddress);

        Task createReceiveTcpSocket();

       
    }


    public interface IUdpSocket
    {


    }


}

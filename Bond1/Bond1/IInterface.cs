using System;
using System.Threading.Tasks;


namespace Bond1
{
    public interface ClientSocket
    {
        Task createReceiveUdpSocket();
        Task WaitToGuestConnect();
        void returnIpAdress(String address);

      
    }

    public interface GuestSocket
    {
        void sendBroadcast();
        void receivedHostIp();
        //void inputDeviceNameAndIp(Socket socket);
        void HostToConnect(String remoteIpAddress);
            

       
    }


    public interface IUdpSocket
    {


    }


}

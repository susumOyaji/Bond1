using System;
using System.Threading.Tasks;


namespace Bond1
{
    public interface ClientSocket
    {
        Task createReceiveUdpSocket();
        Task<string> WaitToGuestConnect();
        void returnIpAdress(String address);

      
    }

    public interface GuestSocket
    {
        void sendBroadcast();
        void receivedHostIp();
        //void inputDeviceNameAndIp(Socket socket);
        void HostToConnect(String remoteIpAddress);
            

       
    }


    public interface TcpIpSocket
    {
        //①Host TCP通信待ち受け状態を作る。
        Task WaitToGuestConnect();

        //③ブロードキャスト発信者(ゲスト)にIPアドレスと端末名を返す
        void returnIpAdress(String address);
        
        
        //②ホストからTCPでIPアドレスが返ってきたときに受け取るメソッド 
        void receivedHostIp();

       

        //④端末名とIPアドレスのセットを受け取る
        //void inputDeviceNameAndIp(Socket socket);

        //⑤IPアドレスが判明したホストに対して接続を行う
        void HostToConnect(String remoteIpAddress);

        //IPAdress取得
        string GetIPAddress();
        string getIPAddress();
        string GetLocalIPAddress();
    }


}

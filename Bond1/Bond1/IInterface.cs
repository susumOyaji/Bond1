using System;
using System.Threading.Tasks;


namespace Bond1
{
   public interface TcpIpSocket1
    {
        //TcpServer TS = new TcpServer();

        Task<string> ClientConnect();
        string getstring();

        void ServerConnect();
    }

    public interface ITcpSocket1
    {
        Task ServerConnect();
        string SeverToConnect();
        //Task<string> ClientConnect();
        //Task<string> ClientConnect1();
        string SeverToReceive();
        bool IsConnected { get; }
        string getIPAddress();
        string OsVersion { get; }
    }


    // イベントのパラメーター
    public class LocationEventArgs : EventArgs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    // 位置を受信した際のイベントハンドラー
    public delegate void LocationEventHandler(object sender, LocationEventArgs args);

    // GPS を利用するための、共通なインターフェース
    public interface IGeolocator
    {
        void StartGps();
        event LocationEventHandler LocationReceived;
    }



}

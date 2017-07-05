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
        void ServerConnect();
        //void ServerConnect1();
        //Task<string> ClientConnect1();
        Task<string> ClientConnect1();
        bool IsConnected { get; }
        string getIPAddress();
    }



}

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
        Task<string> ClientConnect();
        bool IsConnected { get; }

    }



}

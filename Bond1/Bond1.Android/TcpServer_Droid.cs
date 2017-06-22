using System;
using System.Net;
using System.Net.Sockets;



namespace Bond1.Droid
{
    public class TcpServer_Droid
    {
        int TcpPort = 3333;

        public TcpServer_Droid()
        {
            
            Socket ssoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            EndPoint point = new IPEndPoint(IPAddress.Any, TcpPort);
            ssoc.Bind(point);


            while (true)//受付
            {
                Socket soc = ssoc.Accept();
                //通信処理
            }

            //TCPクライアント接続
            IPAddress addr = IPAddress.Parse("IPアドレス");
            /*EndPoint*/ point = new IPEndPoint(addr, TcpPort);
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            soc.Connect(point);

        
            //ホスト名
            IPAddress addr = Dns.Resolve("ホスト").AddressList[0];
        
        
        
        
        
        
        
        
        }





    }
}

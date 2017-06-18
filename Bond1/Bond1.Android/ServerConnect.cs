
using System;
using System.Net;
using System.Threading;

using Android.App;
using Android.Content;
using Java.IO;
using Java.Net;
using Android.Net.Wifi;
using Android.Net;

using Bond1.Droid;

//using Android.Net.Wifi.WifiInfo;
//using Android.Net.Wifi.WifiManager;

//ゲスト側
//②ゲスト：ホスト探索開始。ゲスト端末のIPアドレスを発信する。
//ホストが待ち受け態勢を作ったところで、次はゲスト側からブロードキャスト送信を行います。ここでは送信回数を10回、５秒間隔で送るように制限しています。
//ブロードキャスト送信を行うとき、Wi-Fi設定が有効で、かつ、ネットワークに接続されていないとExceptionが発生しますので、対策が必要です。

//④ゲスト：ホストから端末情報を受け取る
//ゲストからTCP通信が行われると待ち状態になっていた処理が再開し、②で準備していたinputDeviceNameAndIp()まで処理が進みます。
//socket = serverSocket.accept();    
//↓③で使用  
//inputDeviceNameAndIp(socket);   
//このメソッドはIPアドレスと端末名を取得して保持するために用意しています。

//⑤ゲスト：ホストのIPアドレスが判明して、TCP通信を開始する。
//ゲスト端末でホストのIPアドレスを入手することが出来たので、あとはTCP通信を試みるだけです。

[assembly: Xamarin.Forms.Dependency(typeof(ServerConnect))]
// Client
namespace Bond1.Droid
{
    //[Activity(Label = "WaitReceiv")]
    public class ServerConnect: ITcpSocket
    {
        bool waiting;
        int udpPort = 9999;//ホスト、ゲストで統一  
        int tcpPort = 3333;

        
        /// <summary>Udp
        /// 同一Wi-fiに接続している全端末に対してブロードキャスト送信を行う
        /// </summary>
        void sendBroadcast()
        {
            string myIpAddress = getIpAddress();
            int count = 0;
            //送信回数を10回に制限する  
            while (count < 10)
            {
                try
                {
                    DatagramSocket udpSocket = new DatagramSocket(udpPort);
                    udpSocket.Broadcast = true;

                    //public DatagramPacket(byte[] buf, int length, InetAddress address, int port);

                    byte[] IpAddressdata = System.Text.Encoding.UTF8.GetBytes(myIpAddress);

                    DatagramPacket packet = new DatagramPacket(IpAddressdata/*myIpAddress.getBytes()*/, myIpAddress.Length, getBroadcastAddress(), udpPort);
                    udpSocket.Send(packet);
                    udpSocket.Close();
                }
                catch (SocketException e)
                {
                    e.PrintStackTrace();
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                }
                //5秒待って再送信を行う  
                try
                {
                    Thread.Sleep(5000);
                    count++;
                }
                catch (InterruptedIOException e)
                {
                    e.PrintStackTrace();
                }
            }
        }
        //getIpAdress()、getBroadcastAdress()は端末のIPアドレスとブロードキャストアドレスを算出するメソッドを下記のように用意しています。 
        //また、これらを取得可能にするために、あらかじめサービスのハンドルを取得しています。



        WifiManager wifi;
        //WifiManager manager;
        string ipAddress = null;

        ///*コンストラクタ*/
        public void Sample_WifiConnection(Context context)
        {
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);
            //    //manager = (WifiManager)context.GetSystemService(Context.WifiService);
        }





        
        /// <summary>
        /// IPアドレスの取得
        /// </summary>
        /// <returns></returns>
        public string getIpAddress()
        {

            int ipAddress_int = wifi.ConnectionInfo.IpAddress;

            if (ipAddress_int == 0)
            {
                ipAddress = null;
            }
            else
            {
                ipAddress = (ipAddress_int & 0xFF) + "." + (ipAddress_int >> 8 & 0xFF) + "." + (ipAddress_int >> 16 & 0xFF) + "." + (ipAddress_int >> 24 & 0xFF);
            }
            return ipAddress;
        }


        
        /// <summary>Udp
        /// ブロードキャストアドレスの取得
        /// </summary>
        /// <returns></returns>
        public InetAddress getBroadcastAddress()
        {
            DhcpInfo dhcpInfo = wifi.DhcpInfo;
            int broadcast = (dhcpInfo.IpAddress & dhcpInfo.Netmask) | ~dhcpInfo.Netmask;
            byte[] quads = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                quads[i] = (byte)((broadcast >> i * 8) & 0xFF);
            }
            try
            {
                return InetAddress.GetByAddress(quads);
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
                return null;
            }
        }




      
        
      


        /// <summary>Udp
        /// 
        /// </summary>
        /// <returns></returns>
        private int EnableWifi()
        {
            string networkSSID = "bbox-xxx";
            string networkPass = "mypass";

            WifiConfiguration wifiConfig = new WifiConfiguration();
            wifiConfig.Ssid = string.Format("\"{0}\"", networkSSID);
            wifiConfig.PreSharedKey = string.Format("\"{0}\"", networkPass);

            WifiManager wifiManager = (WifiManager)Application.Context.GetSystemService(Context.WifiService);
            WifiInfo wifiinfo = (WifiInfo)Application.Context.GetSystemService(Context.WifiService);
            // Use ID
            int netId = wifiManager.AddNetwork(wifiConfig);
            wifiManager.Disconnect();
            wifiManager.EnableNetwork(netId, true);
            wifiManager.Reconnect();
            int ipAddress = wifiinfo.IpAddress;
            return ipAddress;

        }








       






        //次に、ホストからTCP通信でIPアドレスが返されてくるので、受け取るための待ち受け状態を作っておきます。
        //ホストからTCPでIPアドレスが返ってきたときに受け取るメソッド
        ///Tcp  
        void receivedHostIp()
        {
            ServerSocket serverSocket = null;
            Socket socket = null;

                while (waiting)
                {
                    try
                    {
                        if (serverSocket == null)
                        {
                            serverSocket = new ServerSocket(tcpPort);
                        }
                        socket = serverSocket.Accept();
                        //↓③で使用  
                        inputDeviceNameAndIp(socket);
                        if (serverSocket != null)
                        {
                            serverSocket.Close();
                            serverSocket = null;
                        }
                        if (socket != null)
                        {
                            socket.Close();
                            socket = null;
                        }
                    }
                    catch (IOException e)
                    {
                        waiting = false;
                        e.PrintStackTrace();
                    }
                }
        }
      
    





       

        //ゲストからTCP通信が行われると待ち状態になっていた処理が再開し、②で準備していたinputDeviceNameAndIp()まで処理が進みます。

        //socket = serverSocket.accept();    
        //↓③で使用  
        //inputDeviceNameAndIp(socket);
        //このメソッドはIPアドレスと端末名を取得して保持するために用意しています。

        //端末名とIPアドレスのセットを受け取る    Tcp
        void inputDeviceNameAndIp(Socket socket)
        {
            try
            {
                BufferedReader bufferedReader = new BufferedReader(
                    new InputStreamReader(socket.InputStream)
                );
                int infoCounter = 0;
                string remoteDeviceInfo;
                //ホスト端末情報(端末名とIPアドレス)を保持するためのクラスオブジェクト 
                //※このクラスは別途作成しているもの  
                SampleDevice hostDevice = new SampleDevice();
                while ((remoteDeviceInfo = bufferedReader.ReadLine()) != null && !remoteDeviceInfo.Equals("outputFinish"))
                {
                    switch (infoCounter)
                    {
                        case 0:
                            //1行目、端末名の格納 
                            hostDevice.setDeviceName(remoteDeviceInfo);
                            infoCounter++;
                            break;
                        case 1:
                            //2行目、IPアドレスの取得    
                            hostDevice.setDeviceIpAddress(remoteDeviceInfo);
                            infoCounter++;
                            return;
                        default:
                            return;
                    }
                }
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
        }

        //ゲストが端末情報を受け取るとき、本来はreturnしなくていいのですが、どうにも読み込みが完了してくれなかったので無理やり処理を完了させるためreturnさせています。もっとスマートなやり方があるはず…。





        // ⑤ゲスト：ホストのIPアドレスが判明して、TCP通信を開始する。
        //ゲスト端末でホストのIPアドレスを入手することが出来たので、あとはTCP通信を試みるだけです。
        //IPアドレスが判明したホストに対して接続を行う       
        void connect(String remoteIpAddress) Tcp
        {
            waiting = false;
            Socket socket = null;

            try
            {
                if (socket == null)
                {
                    socket = new Socket(remoteIpAddress, tcpPort);
                    //この後はホストに対してInputStreamやOutputStreamを用いて入出力を行ったりするが、ここでは割愛        
                }
            }
            catch (UnknownHostException e)
            {
                e.PrintStackTrace();
            }
            catch (ConnectException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
        }
    }



    public class SampleDevice
    {
        public void setDeviceName(string a)
        {
        }

        public void  setDeviceIpAddress(string b)
        {

        }


    }
}


  
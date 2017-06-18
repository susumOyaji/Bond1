
using System;
using System.Threading.Tasks;
using System.Threading;

using Android.OS;
using Java.IO;
using Java.Net;
using Android.Net.Wifi;

using Android.App;
using Android.Content;
using Android.Net;

using Bond1.Droid;



[assembly: Xamarin.Forms.Dependency(typeof(UdpConnect))]
//Server 
namespace Bond1.Droid
{
    public class UdpConnect : IUdpReceiveSocket
    {
        //public ClientConnect() { }
        DatagramSocket receiveUdpSocket;  
        bool waiting;
        int udpPort = 9999;//ホスト、ゲストで統一  
        int tcpPort = 3333;

        /// <summary>Udp
        /// ①ブロードキャスト受信用ソケットの生成,ブロードキャスト受信待ち状態を作る  
        /// </summary>
        public async Task createReceiveUdpSocket()
        {
            waiting = true;
            string address = null;


            try
            {
                //waiting = trueの間、ブロードキャストを受け取る
                while (waiting)
                {
                    //受信用ソケット
                    DatagramSocket receiveUdpSocket = new DatagramSocket(udpPort);
                    byte[] buf = new byte[256];
                    DatagramPacket packet = new DatagramPacket(buf, buf.Length);

                    //await Task.Run(
                    //ゲスト端末からのブロードキャストを受け取る  
                    //受け取るまでは待ち状態になる   
                    await Task.Run(() => receiveUdpSocket.Receive(packet));

                    //受信バイト数取得 
                    //int length = packet.getLength();
                    int length = packet.Length;
                    //受け取ったパケットを文字列にする 
                    //address = new String(buf, 0, length);
                    address = System.Text.Encoding.UTF8.GetString(buf);

                    //↓③で使用  
                    returnIpAdress(address);
                    receiveUdpSocket.Close();
                    //);
                }
            }
            catch (SocketException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }

        }


        //また、UDP通信とは別にTCP通信も待ち受け状態を作っておきます。
        //これをしないと、④でゲストから通信しようとしても通信先のホストが見つからず、Exceptionが発生しますのでご注意ください。
    
        /*
        ②ゲスト：ホスト探索開始。ゲスト端末のIPアドレスを発信する。
            ホストが待ち受け態勢を作ったところで、次はゲスト側からブロードキャスト送信を行います。ここでは送信回数を10回、５秒間隔で送るように制限しています。
            ブロードキャスト送信を行うとき、Wi-Fi設定が有効で、かつ、ネットワークに接続されていないとExceptionが発生しますので、対策が必要です。
        */

        /// <summary>Udp
        /// 同一Wi-fiに接続している全端末に対してブロードキャスト送信を行う
        /// </summary>
        public void sendBroadcast()
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



        /// <summary> Tcp
        /// ブロードキャスト発信者(ゲスト)にIPアドレスと端末名を返す
        /// </summary>
        /// <param name="address"></param>
        public void returnIpAdress(string address)
        {
            Socket returnSocket = null;

            try
            {
                if (returnSocket != null)
                {
                    returnSocket.Close();
                    returnSocket = null;
                }
                if (returnSocket == null)
                {
                    returnSocket = new Socket(address, tcpPort);
                }

                //端末情報をゲストに送り返す  
                outputDeviceNameAndIp(returnSocket, getDeviceName(), getIpAddress());
            }
            catch (UnknownHostException e)
            {
                e.PrintStackTrace();
            }
            catch (ConnectException e)
            {
                e.PrintStackTrace();
                try
                {
                    if (returnSocket != null)
                    {
                        returnSocket.Close();
                        returnSocket = null;
                    }
                }
                catch (IOException e1)
                {
                    e1.PrintStackTrace();
                }
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }

        }

        WifiManager wifi;
        //WifiManager manager;
        string ipAddress = null;

        ///*コンストラクタ*/
        public void Sample_WifiConnection(Context context)
        {
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);
            //    //manager = (WifiManager)context.GetSystemService(Context.WifiService);
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
        
        
        
        
        
        
        
        DatagramSocket receiveUdpSocket;


        //bool waiting;
        //int udpPort = 9999;//ホスト、ゲストで統一  

     

       
        /// <summary>Tcp
        /// Gets the name of the device.
        /// </summary>
        /// <returns>The device name.</returns>
        public static string getDeviceName()
        {
            string manufacturer = Build.Manufacturer;
            string model = Build.Model;

            return model;
        }


        /// <summary>Tcp
        /// Gets the ip address.
        /// </summary>
        /// <returns>The ip address.</returns>
        //public string getIpAddress()
        //{
        //    string IpAddress = "1234";
        //    return IpAddress;
        //}

        /// <summary>Tcp
        /// Outputs the device name and ip.
        /// 端末名とIPアドレスのセットをゲストに送る  
        /// </summary>
        /// <param name="outputSocket">Output socket.</param>
        /// <param name="deviceName">Device name.</param>
        /// <param name="deviceAddress">Device address.</param>
        void outputDeviceNameAndIp(Socket outputSocket, String deviceName, String deviceAddress)
        {
            BufferedWriter bufferedWriter;
            try
            {
                bufferedWriter = new BufferedWriter(
                    new OutputStreamWriter(outputSocket.OutputStream)
                );

                //(3)FileOutputStreamオブジェクトの生成
                // OutputStream xyz = new OutputStream("xyz.txt");
                //(5)OutputStreamWriterオブジェクトの生成
                //bufferedWriter = new BufferedWriter(
                //     new OutputStreamWriter(xyz, "Shift_JIS")
                //);



                //デバイス名を書き込む    
                bufferedWriter.Write(deviceName);
                bufferedWriter.NewLine();

                //IPアドレスを書き込む    
                bufferedWriter.Write(deviceAddress);
                bufferedWriter.NewLine();

                //出力終了の文字列を書き込む  
                bufferedWriter.Write("outputFinish");

                //出力する 
                bufferedWriter.Flush();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
        }


        //次に、ホストからTCP通信でIPアドレスが返されてくるので、受け取るための待ち受け状態を作っておきます。
        //ホストからTCPでIPアドレスが返ってきたときに受け取るメソッド







    }
}
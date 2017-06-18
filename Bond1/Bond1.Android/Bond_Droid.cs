using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Java.IO;
using Java.Net;
using Android.Net.Wifi;
using Android.Net;
using Bond1.Droid;
/*
---------------------------------------------      ①UDP - Host Receive    ---------------------------
①ホスト：ゲストから発信されるブロードキャストを受信できる(受信待ち受け)状態にする。
ゲストからいくらブロードキャスト送信をしていても、ホスト側で受け取り態勢が整っていなければ受け取ることが出来ません。
まずはゲストが送信を開始する前に待ち受け状態にします。
*/

[assembly: Xamarin.Forms.Dependency(typeof(SocketLib_Droid))]
namespace Bond1.Droid
{
    public class SocketLib_Droid : IUdpReceiveSocket
    {
        DatagramSocket receiveUdpSocket;
        bool waiting = false;
        int udpPort = 9999;//ホスト、ゲストで統一  

        //ブロードキャスト受信用ソケットの生成   
        //ブロードキャスト受信待ち状態を作る  
        public async Task createReceiveUdpSocket()
        {
            waiting = true;
            String address = null;

            try
            {
                //waiting = trueの間、ブロードキャストを受け取る
                while (waiting)
                {
                    //受信用ソケット
                    DatagramSocket receiveUdpSocket = new DatagramSocket(udpPort);
                    byte[] buf = new byte[256];
                    DatagramPacket packet = new DatagramPacket(buf, buf.Length);

                    //ゲスト端末からのブロードキャストを受け取る  
                    //受け取るまでは待ち状態になる   
                    await Task.Run(() => receiveUdpSocket.Receive(packet));

                    //受信バイト数取得 
                    int length = packet.Length;
                    //受け取ったパケットを文字列にする 
                    address = System.Text.Encoding.UTF8.GetString(buf); //new String(buf, 0, length);
                    //↓③で使用  
                    returnIpAdress(address);
                    receiveUdpSocket.Close();
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






        /*
        -------------------------------------------------    ①TCP - Host Receive   ----------------------------
        また、UDP通信とは別にTCP通信も待ち受け状態を作っておきます。
        これをしないと、④でゲストから通信しようとしても通信先のホストが見つからず、Exceptionが発生しますのでご注意ください。
        */


        ServerSocket serverSocket;
        Socket connectedSocket;
        int tcpPort = 3333;//ホスト、ゲストで統一  

        //ゲストからの接続を待つ処理  
        void WaitToGuestConnect()
        {
            try
            {
                //ServerSocketを生成する
                serverSocket = new ServerSocket(tcpPort);
                //ゲストからの接続が完了するまで待って処理を進める 
                connectedSocket = serverSocket.Accept();
                //この後はconnectedSocketに対してInputStreamやOutputStreamを用いて入出力を行ったりするが、ここでは割愛      
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





        /*
        --------------------------      ①UDP - Guest      --------------------------------- 
        ②ゲスト：ホスト探索開始。ゲスト端末のIPアドレスを発信する。

        ホストが待ち受け態勢を作ったところで、次はゲスト側からブロードキャスト送信を行います。ここでは送信回数を10回、５秒間隔で送るように制限しています。
        ブロードキャスト送信を行うとき、Wi-Fi設定が有効で、かつ、ネットワークに接続されていないとExceptionが発生しますので、対策が必要です。
        */
        //bool waiting;
        //int udpPort = 9999;//ホスト、ゲストで統一  

        //同一Wi-fiに接続している全端末に対してブロードキャスト送信を行う 
        void sendBroadcast()
        {
            String myIpAddress = getIpAddress();
            waiting = true;

            int count = 0;
            //送信回数を10回に制限する  
            while (count < 10)
            {
                try
                {
                    DatagramSocket udpSocket = new DatagramSocket(udpPort);
                    udpSocket.Broadcast = true;

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



        /*
        getIpAdress()、getBroadcastAdress()は端末のIPアドレスとブロードキャストアドレスを算出するメソッドを下記のように用意しています。 
        また、これらを取得可能にするために、あらかじめサービスのハンドルを取得しています。
        */



        WifiManager wifi;
        string ipAddress = null;
        /*コンストラクタ*/
        public void Sample_WifiConnection(Context context)
        {
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);
        }

        //IPアドレスの取得  
        String getIpAddress()
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

        //ブロードキャストアドレスの取得    
        InetAddress getBroadcastAddress()
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


        /*
        ------------------------------------    TCP - Guest Receive    --------------------------
        次に、ホストからTCP通信でIPアドレスが返されてくるので、受け取るための待ち受け状態を作っておきます。
        */
        //ホストからTCPでIPアドレスが返ってきたときに受け取るメソッド  
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





        /*
        --------------------------   TCP - Host   -------------------------   
        ③ホスト：ゲストから受信したIPアドレスに対してホスト端末のIPアドレスを送り返す。

        ①でIPアドレスを受け取ると
        receiveUdpSocket.receive(packet);  
        ここで止まっていた処理が再開します。
        受信データ(IPアドレス)を文字列に変換して、IPアドレスを返すために用意したメソッドに受け取ったアドレスを引数に渡します。

        returnIpAdress(address);   
        処理はこんな感じです。
        */


        Socket returnSocket;
        //ブロードキャスト発信者(ゲスト)にIPアドレスと端末名を返す   
        void returnIpAdress(String address)
        {

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
                outputDeviceNameAndIp(returnSocket, getdeviceName(), getIpAddress());
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

        private string getdeviceName()
        {
            throw new NotImplementedException();
        }



        //端末名とIPアドレスのセットを送る  
        void outputDeviceNameAndIp(Socket outputSocket, String deviceName, String deviceAddress)
        {

            BufferedWriter bufferedWriter;
            try
            {
                bufferedWriter = new BufferedWriter(
                        new OutputStreamWriter(outputSocket.OutputStream)
                );
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







        /*
        ---------------------------        TCP - Guest   --------------------------------------  
        ④ゲスト：ホストから端末情報を受け取る

        ゲストからTCP通信が行われると待ち状態になっていた処理が再開し、②で準備していたinputDeviceNameAndIp()まで処理が進みます。

        socket = serverSocket.accept();    
        //↓③で使用  
        inputDeviceNameAndIp(socket);   
        このメソッドはIPアドレスと端末名を取得して保持するために用意しています。
        */
        //端末名とIPアドレスのセットを受け取る   
        void inputDeviceNameAndIp(Socket socket)
        {
            try
            {
                BufferedReader bufferedReader = new BufferedReader(
                    new InputStreamReader(socket.InputStream)
                );
                int infoCounter = 0;
                String remoteDeviceInfo;
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
        /*    
        ゲストが端末情報を受け取るとき、本来はreturnしなくていいのですが、どうにも読み込みが完了してくれなかったので無理やり処理を完了させるためreturnさせています。
        もっとスマートなやり方があるはず…。
        */




        /*
        -----------------------------    TCP - Guest   ---------------------------------------------
        ⑤ゲスト：ホストのIPアドレスが判明して、TCP通信を開始する。

        ゲスト端末でホストのIPアドレスを入手することが出来たので、あとはTCP通信を試みるだけです。
        */
        //IPアドレスが判明したホストに対して接続を行う   
        void HostToConnect(String remoteIpAddress)
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

        public class SampleDevice
        {
            public void setDeviceName(string a)
            {
            }

            public void setDeviceIpAddress(string b)
            {

            }


        }


    }
}

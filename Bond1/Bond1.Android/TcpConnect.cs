
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Java.IO;
using Java.Net;
using Android.Net.Wifi;
using Android.Net;

      
using Android.OS;

//using Android.OS;



using Bond1.Droid;

//using Android.Net.Wifi.WifiInfo;
//using Android.Net.Wifi.WifiManager;



[assembly: Xamarin.Forms.Dependency(typeof(TcpConnect))]
namespace Bond1.Droid
{
    //また、UDP通信とは別にTCP通信も待ち受け状態を作っておきます。
    // これをしないと、④でゲストから通信しようとしても通信先のホストが見つからず、Exceptionが発生しますのでご注意ください。
    public class TcpConnect: ITcpSocket
    {
        ServerSocket serverSocket;
        Socket connectedSocket;
        int tcpPort = 3333;//ホスト、ゲストで統一 
        bool waiting;


        //ゲストからの接続を待つ処理  
        public void connect()
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



       


        /// <summary>Tcp
        /// ①ゲストからの接続を待つ処理 
        /// </summary>
        public async Task createReceiveTcpSocket()
        {
            try
            {
                //ServerSocketを生成する
                serverSocket = new ServerSocket(tcpPort);

                //ゲストからの接続が完了するまで待って処理を進める 
                connectedSocket = await Task.Run(() => serverSocket.Accept());
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

        //③ホスト：ゲストから受信したIPアドレスに対してホスト端末のIPアドレスを送り返す。
        //①でIPアドレスを受け取るとreceiveUdpSocket.receive(packet);ここで止まっていた処理が再開します。

        ///  
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

        //端末名とIPアドレスのセットを受け取る 
        /// <summary>
        /// Inputs the device name and ip.
        /// </summary>
        /// <param name="socket">Socket.</param>
        public void inputDeviceNameAndIp(Socket socket)
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
        /// <summary>
        /// Connect the specified remoteIpAddress.
        /// </summary>
        /// <returns>The connect.</returns>
        /// <param name="remoteIpAddress">Remote ip address.</param>
        public void Connect(String remoteIpAddress)
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


        public void ConnectEnable()
        {
            inputDeviceNameAndIp(Socket socket);
            Connect(String remoteIpAddress);
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


  
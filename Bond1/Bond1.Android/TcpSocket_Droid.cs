using System;
using System.Net;
//using System.Net.Sockets;
//using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Java.IO;
using Java.Net;
using Android.Runtime;
using Android.Net.Wifi;
using Android.Net;

using Bond1.Droid;
using Android.Widget;



[assembly: Xamarin.Forms.Dependency(typeof(TcpSocket))]
namespace Bond1.Droid
{
    //---------------------------------------------------------------------------------
    //[Activity(Label = Bond1.Droid, MainLauncher = true)]
    public class TcpSocket : TcpIpSocket1
    {
        string message;
        string Anser;

               
        public TcpSocket()
        {
            
        }

    


        public string getstring()
        {
            Anser = "Android";
            return Anser;
        }


        public async Task<string> ClientConnect()
        {
            Java.Net.Socket connection = null;
            BufferedReader reader = null;


            try
            {
                // サーバーへ接続
                connection = await Task.Run(() => new Java.Net.Socket("pop.mail.yahoo.co.jp", 110));

                // メッセージ取得オブジェクトのインスタンス化
                reader = new BufferedReader(new InputStreamReader(connection.InputStream));

                // サーバーからのメッセージを受信
                message = await Task.Run(() =>reader.ReadLine());
               


                // 接続確認
                if (message == "")
                {
                    //tv.SetText("サーバーとの接続に失敗しました:" + message, TextView.BufferType.Spannable);
                    //Toast.MakeText(this, "サーバーとの接続に失敗しました。", ToastLength.Long).Show();
                    return "サーバーとの接続に失敗しました。";
                }
                else
                {
                    //tv.SetText("サーバーからのメッセージ：" + message, TextView.BufferType.Spannable);
                    //Toast.MakeText(this, "サーバーとの接続に成功しました。", ToastLength.Long).Show();
                    return "サーバーとの接続に成功しました。";
                    //return this.ClientConnect().Result;
                }

            }
            catch (UnknownHostException e)
            {
                e.PrintStackTrace();
                //tv.SetText("エラー内容：" + e.ToString(), TextView.BufferType.Spannable);
                //Toast.MakeText(this, "サーバーとの接続に失敗しました。", ToastLength.Long).Show();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
                //tv.SetText("エラー内容：" + e.ToString(), TextView.BufferType.Spannable);
                //Toast.MakeText(this, "サーバーとの接続に失敗しました。", ToastLength.Short).Show();
            }
            finally
            {
                try
                {
                    // 接続終了処理
                    reader.Close();
                    connection.Close();
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                    //tv.SetText("エラー内容：" + e.ToString(), TextView.BufferType.Spannable);
                    //Toast.MakeText(this, "サーバーとの接続に失敗しました。", ToastLength.Short).Show();
                }
            }
            return "サーバーとの接続に成功しました。";
        }


        /*
       ------------------------------------    TCP - Guest Receive    --------------------------
       次に、ホストからTCP通信でIPアドレスが返されてくるので、受け取るための待ち受け状態を作っておきます。
       */
        //ホストからTCPでIPアドレスが返ってきたときに受け取るメソッド  
        public async void ServerConnect()
        {
            ServerSocket serverSocket = null;
            Socket socket = null;
            bool waiting = true;
            int tcpPort = 3333;

            while (waiting)
            {
                try
                {
                    if (serverSocket == null)
                    {
                        serverSocket = new ServerSocket(tcpPort);
                    }
                    socket = await Task.Run(() => serverSocket.Accept());
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



        //端末名とIPアドレスのセットを受け取る   
        public void inputDeviceNameAndIp(Socket socket)
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



        public class SampleDevice
        {
            public void setDeviceName(string a)
            {
            }

            public void setDeviceIpAddress(string b)
            {

            }


        }












        public string getIPAddress()
        {
            string ipaddress = "";

            IPHostEntry ipentry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in ipentry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipaddress = ip.ToString();
                    break;
                }
            }
            return ipaddress;
        }

    }




























}














    //public class TcpServer_Droid
    //{
    //    int TcpPort = 3333;

    //    public TcpServer_Droid()
    //    {

    //        Socket ssoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //        EndPoint point = new IPEndPoint(IPAddress.Any, TcpPort);
    //        ssoc.Bind(point);


    //        while (true)//受付
    //        {
    //            Socket soc = ssoc.Accept();
    //            //通信処理
    //        }

    //        //TCPクライアント接続
    //        IPAddress addr = IPAddress.Parse("IPアドレス");
    //        /*EndPoint*/
    //        point = new IPEndPoint(addr, TcpPort);
    //        Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        soc.Connect(point);


    //        //ホスト名
    //        IPAddress addr = Dns.Resolve("ホスト").AddressList[0];

    //    }

    //}



//}
////24行目ではYahooのメールサーバーのホスト名とポート番号を使用して接続しています。

//27、28行目では接続が確立したSocketクラスの入力ストリームを使用してBufferedReaderクラスのオブジェクトを作成しています。

//31行目ではBufferedReaderクラスのreadLine()メソッドを使用してサーバーからのメッセージを取得しています。

//34～42行目はYahooメールサーバーから接続完了した旨のメッセージが返ってきているかを判定しています。今回接続しているYahooメールサーバーはPOP3サーバーなので、要求が正しく実行されるとメッセージの先頭に”+OK”が付加されます。

//57、58行目ではサーバーとの接続を切断しています。不要になった接続は不要になった段階で切断しておかないと、他の通信処理に悪影響がでる可能性があるので、忘れないように適時切断して下さい。














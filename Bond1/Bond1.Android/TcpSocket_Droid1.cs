
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Java.Net;
using Java.IO;
using Java.Lang;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

using Xamarin.Forms;
using Xamarin.Android;
using Bond1.Droid;



[assembly: Xamarin.Forms.Dependency(typeof(TcpSocket_Droid1))]
namespace Bond1.Droid
{
    [Activity(Label = "TcpSocket_Droid1")]
    public partial class TcpSocket_Droid1 : ITcpSocket1
    {
        static int PORT = 110;
        string message;

       

        //クライアント側のプログラムは以下のようになる。
        //ClientLesson.java

        //static string HOST = "127.0.0.1";
        static string HOST = "pop.mail.yahoo.co.jp";//"187.22.112.107";//Yahho Mail
        //static int PORT = 110; Yahoo Mail Port


        public async Task<string> ClientConnect()
        {

            Socket socket = null;
            //Java.Net.Socket connection = null;
            BufferedReader reader = null;
            string count = "000000";


            try
            {

                // サーバーへ接続
                socket = await Task.Run(() => new Java.Net.Socket(HOST, PORT));
               // PrintWriter pw = new PrintWriter(socket.OutputStream, true);
               // pw.Println("Hello world");//サーバーに送出するコメント

                // メッセージ取得オブジェクトのインスタンス化
                reader = new BufferedReader(new InputStreamReader(socket.InputStream));

                // サーバーからのメッセージを受信
                message = await Task.Run(() => (string)(reader.ReadLine()));
                IsConnected = true;// message;
                //return count;

                string manufacturer = Android.OS.Build.Manufacturer;
                string model = Android.OS.Build.Model;
                return $"{manufacturer} {model}";


            }
            catch (UnknownHostException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }


            if (socket != null)
            {
                try
                {
                    socket.Close();
                    socket = null;
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                }
            }
            return "message1";
        }



        public bool IsConnected { get; set; }




        //        このプログラムを実行すると
        //「start wait…」と表示され、ポート10000でクライアントから接続があるまで待機する。

        //サーバーのプログラムが待機状態ならば、クライアントのプログラムを実行するとサーバー側に文字列が渡るだろう。
        //サーバー側のコンソールに
        //「Hello world」と表示される。

        //※この例では、サーバーは”exit”という文字列を受け取らないと停止しない。

        public async void ServerConnect()
        {

            ServerSocket serverSocket = null;

            try
            {
                serverSocket = new ServerSocket(PORT);

                bool runFlag = true;

                while (runFlag)
                {

                    //System.out.println("start wait...");
                    System.Console.WriteLine("start wait...");

                    // 接続があるまでブロック
                    Socket socket = await Task.Run(() => serverSocket.Accept());

                    BufferedReader br =
                        new BufferedReader(new InputStreamReader(socket.InputStream));

                    string str = await Task.Run(() => br.ReadLine());

                    while ((str) != null)
                    {
                        //System.out.println(str);
                        System.Console.WriteLine(str);

                        // exitという文字列を受け取ったら終了する
                        if ("exit".Equals(str))
                        {
                            runFlag = false;
                        }
                    }

                    if (socket != null)
                    {
                        socket.Close();
                        socket = null;
                    }
                }

            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }


            if (serverSocket != null)
            {
                try
                {
                    serverSocket.Close();
                    serverSocket = null;
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                }
            }
        }

    }
        
 }



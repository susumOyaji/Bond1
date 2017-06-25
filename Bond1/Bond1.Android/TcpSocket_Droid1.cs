
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class TcpSocket_Droid1 : ITcpSocket1
    {
        public class TcpServer
        {

            static int PORT = 10000;

            /**
             * @param args
             */
            public static void main(String[] args)
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
                        Socket socket = serverSocket.Accept();

                        BufferedReader br =
                            new BufferedReader(
                                    new InputStreamReader(socket.getInputStream()));

                        String str;
                        while ((str = br.ReadLine()) != null)
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
        //        このプログラムを実行すると
        //「start wait…」と表示され、ポート10000でクライアントから接続があるまで待機する。





        //次に、クライアント側のプログラムは以下のようになる。

        //ClientLesson.java

        public class TcpClient
        {

            static String HOST = "127.0.0.1";
            static int PORT = 10000;

            /**
             * @param args
             */
            public static void main(String[] args)
            {

                Socket socket = null;

                try
                {
                    socket = new Socket(HOST, PORT);
                    PrintWriter pw = new PrintWriter(socket.getOutputStream(), true);
                    pw.Println("Hello world");

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
            }
        }

        //サーバーのプログラムが待機状態ならば、クライアントのプログラムを実行するとサーバー側に文字列が渡るだろう。
        //サーバー側のコンソールに
        //「Hello world」と表示される。

        //※この例では、サーバーは”exit”という文字列を受け取らないと停止しない。

    }
}

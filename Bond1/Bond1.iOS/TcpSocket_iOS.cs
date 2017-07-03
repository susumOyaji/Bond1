using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Xamarin.Forms;
using UIKit;


using Bond1.iOS;


[assembly: Xamarin.Forms.Dependency(typeof(TcpSocket_iOS))]
namespace Bond1.iOS
{
        //[Activity(Label = "TcpSocket_iOS")]
        public partial class TcpSocket_iOS : ITcpSocket1
        {
            static int PORT = 3333;
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
            //string count = "000000";
                var streamReader = new StreamReader(filePath);
                string line;
                while ((line = streamReader.ReadLine()) != null)




                try
                {

                    // サーバーへ接続
                    socket = await Task.Run(() => new Socket(HOST, PORT));
                    PrintWriter pw = new PrintWriter(socket.OutputStream, true);
                    pw.Println("Hello world");//サーバーに送出するコメント

                    // メッセージ取得オブジェクトのインスタンス化
                    reader = new BufferedReader(new InputStreamReader(socket.InputStream));

                    // サーバーからのメッセージを受信
                    message = await Task.Run(() => (string)(reader.ReadLine()));
                    IsConnected = true;// message;
                                       //return count;

                   
                    return $"{manufacturer} {model}";


                }
                catch (Foundation.NSErrorException e)
                {
                    //works
                    System.Diagnostics.Debug.WriteLine(e + "Caught ns Error exception"); 
                    
                }
                catch (IOException e)
                {
                    System.Diagnostics.Debug.WriteLine(e + "Caught ns exception");
                    
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
                        System.Diagnostics.Debug.WriteLine(e + "Caught ns exception"); ;
                    }
                }
                return "message1";
            }



            public bool IsConnected { get; set; }


            //Getting the IP Address of the device fro Android.


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


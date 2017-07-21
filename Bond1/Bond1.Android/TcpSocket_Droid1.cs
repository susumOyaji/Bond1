using System.Threading.Tasks;
using System.Net;


using Java.Net;
using Java.IO;
using Android.App;
using Bond1.Droid;
using Android.OS;

[assembly: Xamarin.Forms.Dependency(typeof(TcpSocket_Droid1))]
namespace Bond1.Droid
{
    [Activity(Label = "TcpSocket_Droid1")]
    public partial class TcpSocket_Droid1 : ITcpSocket1
    {
        static int PORT = 110;//3333;
        //static string HOST = "127.0.0.1";
        static string HOST = "pop.mail.yahoo.co.jp";//"187.22.112.107";//Yahho Mail
        //static int PORT = 110; Yahoo Mail Port
        //サーバーのIPアドレス（または、ホスト名）とポート番号
        string ipOrHost = "pop.mail.yahoo.co.jp";//"127.0.0.1";
        //string ipOrHost = "localhost";
        int port = 110;//2001;
        string[] a;
        string Ans = "Non Anser!";


        public TcpSocket_Droid1(){ }

        public string SeverToConnect()
        {
            var Task = ClientConnect();
            return Ans;
        }



        //private TaskCompletionSource<string> taskCompletionSource;
        public async Task ClientConnect1()
        {
            
            Socket socket = null;
            //Java.Net.Socket connection = null;
            BufferedReader reader = null;
            //string count = "000000";


            try
            {

                // サーバーへ接続
                socket = await Task.Run(() => new Socket(HOST, PORT));

                //PrintWriter pw = new PrintWriter(socket.OutputStream, true);
                //pw.Println("Hello world");//サーバーに送出するコメント
                //socket.Wait();
                // メッセージ取得オブジェクトのインスタンス化
                reader = new BufferedReader(new InputStreamReader(socket.InputStream));

                // サーバーからのメッセージを受信
                Ans = await Task.Run(() => (reader.ReadLine()));
                //message = reader.ReadLine();
                IsConnected = true;// message;

                //taskCompletionSource = new TaskCompletionSource<string>();
                //return message;

            }
            catch (UnknownHostException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }


            //if (socket != null)
            //{
            //    try
            //    {
            //        socket.Close();
            //        socket = null;
            //    }
            //    catch (IOException e)
            //    {
            //        e.PrintStackTrace();
            //    }
            //}
            //return reader.ReadLine();
            //return await taskCompletionSource.Task;
            //return Ans;
        }



        string ITcpSocket1.OsVersion
        {
            get
            {
                return "message;"; //Android.OS.Build.VERSION.Release;
            }
        }


        public bool IsConnected { get; set; }


        //Getting the IP Address of the device fro Android.

        public string GetIPAddress()
        {
            IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());

            if (adresses != null && adresses[0] != null)
            {
                return adresses[0].ToString();
            }
            else
            {
                return null;
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




        //        このプログラムを実行すると
        //「start wait…」と表示され、ポート10000でクライアントから接続があるまで待機する。

        //サーバーのプログラムが待機状態ならば、クライアントのプログラムを実行するとサーバー側に文字列が渡るだろう。
        //サーバー側のコンソールに
        //「Hello world」と表示される。

        //※この例では、サーバーは”exit”という文字列を受け取らないと停止しない。

        public string SeverToReceive()
        {
            var Task = ServerConnect1();
            return Ans;
        }






        public async Task ServerConnect1()
        {
            
            ServerSocket serverSocket = null;

            try
            {
                serverSocket = await Task.Run(() => new Java.Net.ServerSocket(0));

                bool runFlag = true;

                while (runFlag)
                {

                    //System.out.println("start wait...");
                    System.Console.WriteLine("start wait...");

                    // 接続があるまでブロック
                    Socket socket = await Task.Run(() => serverSocket.Accept());

                    BufferedReader br = new BufferedReader(new InputStreamReader(socket.InputStream));

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
                Ans = e.ToString();
            }


            //if (serverSocket != null)
            //{
            //    try
            //    {
            //        serverSocket.Close();
            //        serverSocket = null;
            //    }
            //    catch (IOException e)
            //    {
            //        e.PrintStackTrace();
            //    }
            //}
        }










        public string[] GetConectIp()
        {
            return a;

        }









        public async Task ClientConnect()
        {
            


            //サーバーに送信するデータを入力してもらう
            //System.Console.WriteLine("文字列を入力し、Enterキーを押してください。");
            string sendMsg = System.Console.ReadLine();
            //何も入力されなかった時は終了
            //if (sendMsg == null || sendMsg.Length == 0)
            //{
            //    return "";
            //}


            //try
            //{
            //TcpClientを作成し、サーバーと接続する
            System.Net.Sockets.TcpClient tcp = await Task.Run(() => new System.Net.Sockets.TcpClient(ipOrHost, port));
            System.Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
            ((System.Net.IPEndPoint)tcp.Client.RemoteEndPoint).Address,
            ((System.Net.IPEndPoint)tcp.Client.RemoteEndPoint).Port,
            ((System.Net.IPEndPoint)tcp.Client.LocalEndPoint).Address,
            ((System.Net.IPEndPoint)tcp.Client.LocalEndPoint).Port);


           
            a = new string[] { ((System.Net.IPEndPoint)tcp.Client.RemoteEndPoint).Address.ToString(), ((System.Net.IPEndPoint)tcp.Client.RemoteEndPoint).Port.ToString(),
                    ((System.Net.IPEndPoint)tcp.Client.LocalEndPoint).Address.ToString(),((System.Net.IPEndPoint)tcp.Client.LocalEndPoint).Port.ToString()};

                       
            //}
            //catch (IOException e)
            //{
            //    System.Console.WriteLine("文字列を入力し、Enterキーを押してください。" + e); 
            //}

            //catch (IOException e)
            //{
            //    System.Console.WriteLine("文字列を入力し、Enterキーを押してください。"+e); 
            //}



            //NetworkStreamを取得する
            System.Net.Sockets.NetworkStream ns = tcp.GetStream();

            //読み取り、書き込みのタイムアウトを10秒にする
            //デフォルトはInfiniteで、タイムアウトしない
            //(.NET Framework 2.0以上が必要)
            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;

            //サーバーにデータを送信する
            //文字列をByte型配列に変換
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            byte[] sendBytes = enc.GetBytes(sendMsg + '\n');
            //データを送信する
            ns.Write(sendBytes, 0, sendBytes.Length);
            System.Console.WriteLine(sendMsg);

            //サーバーから送られたデータを受信する
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            byte[] resBytes = new byte[256];
            int resSize = 0;
            do
            {
                //データの一部を受信する
                resSize = ns.Read(resBytes, 0, resBytes.Length);
                //Readが0を返した時はサーバーが切断したと判断
                if (resSize == 0)
                {
                    System.Console.WriteLine("サーバーが切断しました。");
                    break;
                }
                //受信したデータを蓄積する
                ms.Write(resBytes, 0, resSize);
                //まだ読み取れるデータがあるか、データの最後が\nでない時は、
                // 受信を続ける
            } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');
            //受信したデータを文字列に変換
            string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Close();
            //末尾の\nを削除
            resMsg = resMsg.TrimEnd('\n');
            System.Console.WriteLine(resMsg);

            //閉じる
            ns.Close();
            tcp.Close();
            System.Console.WriteLine("切断しました。");

            System.Console.ReadLine();
            //return resMsg.ToString();
            Ans = resMsg;
        }



        //public bool IsConnected { get; set; }


        ////Getting the IP Address of the device fro Android.


        //public string getIPAddress()
        //{
        //    string ipaddress = "";

        //    IPHostEntry ipentry = Dns.GetHostEntry(Dns.GetHostName());

        //    foreach (IPAddress ip in ipentry.AddressList)
        //    {
        //        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //        {
        //            ipaddress = ip.ToString();
        //            break;
        //        }
        //    }
        //    return ipaddress;
        //}





        public async Task ServerConnect()
        {
            int port = 3333;
            //ListenするIPアドレス
            string ipString = "127.0.0.1";
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ipString);

            //ホスト名からIPアドレスを取得する時は、次のようにする
            //string host = "localhost";
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.GetHostEntry(host).AddressList[0];
            //.NET Framework 1.1以前では、以下のようにする
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.Resolve(host).AddressList[0];

            //Listenするポート番号
            //int port = 2001;

            //TcpListenerオブジェクトを作成する
            System.Net.Sockets.TcpListener listener = await Task.Run(() => new System.Net.Sockets.TcpListener(ipAdd, port));

            //Listenを開始する
            listener.Start();
            System.Console.WriteLine("Listenを開始しました({0}:{1})。", ((System.Net.IPEndPoint)listener.LocalEndpoint).Address, ((System.Net.IPEndPoint)listener.LocalEndpoint).Port);

            //接続要求があったら受け入れる
            System.Net.Sockets.TcpClient client = listener.AcceptTcpClient(); System.Console.WriteLine("クライアント({0}:{1})と接続しました。",
                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address,
                 ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Port);

            //NetworkStreamを取得
            System.Net.Sockets.NetworkStream ns = client.GetStream();

            //読み取り、書き込みのタイムアウトを10秒にする
            //デフォルトはInfiniteで、タイムアウトしない
            //(.NET Framework 2.0以上が必要)
            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;

            //クライアントから送られたデータを受信する
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            bool disconnected = false;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            byte[] resBytes = new byte[256];
            int resSize = 0;
            do
            {
                //データの一部を受信する
                resSize = ns.Read(resBytes, 0, resBytes.Length);
                //Readが0を返した時はクライアントが切断したと判断
                if (resSize == 0)
                {
                    disconnected = true;
                    System.Console.WriteLine("クライアントが切断しました。");
                    break;
                }
                //受信したデータを蓄積する
                ms.Write(resBytes, 0, resSize);
                //まだ読み取れるデータがあるか、データの最後が\nでない時は、
                // 受信を続ける
            } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');
            //受信したデータを文字列に変換
            string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Close();
            //末尾の\nを削除
            resMsg = resMsg.TrimEnd('\n');
            System.Console.WriteLine(resMsg);

            if (!disconnected)
            {
                //クライアントにデータを送信する
                //クライアントに送信する文字列を作成
                string sendMsg = resMsg.Length.ToString();
                //文字列をByte型配列に変換
                byte[] sendBytes = enc.GetBytes(sendMsg + '\n');
                //データを送信する
                ns.Write(sendBytes, 0, sendBytes.Length);
                System.Console.WriteLine(sendMsg);
            }

            //閉じる
            ns.Close();
            client.Close();
            System.Console.WriteLine("クライアントとの接続を閉じました。");

            //リスナを閉じる
            listener.Stop();
            System.Console.WriteLine("Listenerを閉じました。");

            System.Console.ReadLine();
        }






















    }

 }



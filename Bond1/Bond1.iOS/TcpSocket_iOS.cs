using System.Threading.Tasks;
using System.Net;

using Bond1.iOS;
using System.Drawing;

using System.Net.Sockets;
using System.IO;
using System;

using Foundation;
using UIKit;






[assembly: Xamarin.Forms.Dependency(typeof(TcpSocket_iOS))]
namespace Bond1.iOS
{
    public partial class MyNameSpace002ViewController : UIViewController
    {
       private string dataString;
       int currentSliderValue;

            IPAddress ipadd = IPAddress.Parse("1.2.3.4");
            TcpClient sock = new TcpClient();
            int portNumber = 2000;
        private object slider01;///


        public MyNameSpace002ViewController() : base("MyNameSpace002ViewController", null)
            {
            }

            public override void DidReceiveMemoryWarning()
            {
                // Releases the view if it doesn't have a superview.
                base.DidReceiveMemoryWarning();

                // Release any cached data, images, etc that aren't in use.
            }

            public override void ViewDidLoad()
            {
                base.ViewDidLoad();

                this.slider01.MinValue = 0;
                this.slider01.MaxValue = 255;

            }

            public override bool ShouldAutorotateToInterfaceOrientation(UIKit.UIInterfaceOrientation toInterfaceOrientation)
            {
                // Return true for supported orientations
                return true;
            }

            /// <summary>
            /// This is our common action handler. Two buttons call this via an action method.
            /// </summary>
            partial void slider_changed(Foundation.NSObject sender)
            {

                currentSliderValue = Convert.ToInt32(((UISlider)sender).Value);
                this.lb1.Text = currentSliderValue.ToString();

                buildString();
            }


            /// <summary>
            /// Builds the string. Starts with a dollar as that is our start character.
            /// This is set up for up to 4 sliders to be used but at the moment only has 
            /// one dynamic value and the other 3 are set to zeros.
            /// </summary>
            private void buildString()
            {
                string sl1 = currentSliderValue.ToString();
                string sl2 = "0";
                string sl3 = "0";
                string sl4 = "0";


                if (sl1.Length == 1) { sl1 = string.Concat("00", sl1); }
                if (sl2.Length == 1) { sl2 = string.Concat("00", sl2); }
                if (sl3.Length == 1) { sl3 = string.Concat("00", sl3); }
                if (sl4.Length == 1) { sl4 = string.Concat("00", sl4); }

                if (sl1.Length == 2) { sl1 = string.Concat("0", sl1); }
                if (sl2.Length == 2) { sl2 = string.Concat("0", sl2); }
                if (sl3.Length == 2) { sl3 = string.Concat("0", sl3); }
                if (sl4.Length == 2) { sl4 = string.Concat("0", sl4); }


                dataString = string.Concat(
                    "$",
                    sl1,
                    ",",
                    sl2,
                    ",",
                    sl3,
                    ",",
                    sl4);

                this.lbStatus.Text = dataString;

                sendPacket();

            }



            /// <summary>
            /// Makes a tcp connection and returns true or false
            /// </summary>
            /// <returns></returns>
            protected bool tcpConnect()
            {
                try
                {
                    sock.Connect(ipadd, portNumber);
                }
                catch (Exception ex)
                {
                    this.lbStatus.Text = "1. " + ex.Message;
                    return true;
                }

                return false;
            }


            /// <summary>
            /// Connects if there isn't one already and then sends the packet.
            /// </summary>
            void sendPacket()
            {
                if (!sock.Connected)
                {
                    tcpConnect();
                }

                //once connection made, do this
                if (sock.Connected)
                {
                    StreamWriter w = new StreamWriter(sock.GetStream());

                    w.WriteLine(dataString);
                    w.Flush();
                }
                else
                {
                    this.lbStatus.Text = "Not connected so cannot send.";
                }

            }


        }




























    //[Activity(Label = "TcpSocket_iOS")]
    public partial class TcpSocket_iOS : ITcpSocket1
    {
        //サーバーのIPアドレス（または、ホスト名）とポート番号
        string ipOrHost = "127.0.0.1";
        //string ipOrHost = "187.22.112.107";//yahoo
        //string ipOrHost = "localhost";
        int port = 2001;

        public TcpSocket_iOS() { }

        public async Task<string> ClientConnect()
        {
            //サーバーに送信するデータを入力してもらう
            //System.Console.WriteLine("文字列を入力し、Enterキーを押してください。");
            string sendMsg = "heliiw";//System.Console.ReadLine();
            //何も入力されなかった時は終了
            //if (sendMsg == null || sendMsg.Length == 0)
            //{
            //    return  "";
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
            return "message1";
        }



        public bool IsConnected { get; set; }


        //Getting the IP Address of the device fro Android.

        string ITcpSocket1.OsVersion
        {
            get
            {
                return "message"; //Android.OS.Build.VERSION.Release;
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





        public void ServerConnect()
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
            System.Net.Sockets.TcpListener listener = new System.Net.Sockets.TcpListener(ipAdd, port);

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

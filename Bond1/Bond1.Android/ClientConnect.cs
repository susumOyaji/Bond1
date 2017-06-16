
using System;

using Android.OS;
using Java.IO;
using Java.Net;
//using Android.OS;





//ホスト側

//①ホスト：ゲストから発信されるブロードキャストを受信できる(受信待ち受け)状態にする。
//ゲストからいくらブロードキャスト送信をしていても、ホスト側で受け取り態勢が整っていなければ受け取ることが出来ません。
//まずはゲストが送信を開始する前に待ち受け状態にします。

/*    
③ホスト：ゲストから受信したIPアドレスに対してホスト端末のIPアドレスを送り返す。

①でIPアドレスを受け取ると
receiveUdpSocket.receive(packet);  
ここで止まっていた処理が再開します。
受信データ(IPアドレス)を文字列に変換して、 
IPアドレスを返すために用意したメソッドに受け取ったアドレスを引数に渡します。
returnIpAdress(address);   

処理はこんな感じです。
*/


//Server 
namespace Bond1.Droid
{
    public class ClientConnect :TcpSocket
    {
        DatagramSocket receiveUdpSocket;
        bool waiting;
        int udpPort = 9999;//ホスト、ゲストで統一  

     

        /// <summary>
        /// ブロードキャスト受信用ソケットの生成,ブロードキャスト受信待ち状態を作る  
        /// </summary>
        async void createReceiveUdpSocket()
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

                    //ゲスト端末からのブロードキャストを受け取る  
                    //受け取るまでは待ち状態になる   
                    receiveUdpSocket.Receive(packet);

                    //受信バイト数取得 
                    //int length = packet.getLength();
                    int length = packet.Length;
                    //受け取ったパケットを文字列にする 
                    //address = new String(buf, 0, length);
                    address = System.Text.Encoding.UTF8.GetString(buf); 

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


        //*************************************** TCP Server ******************************************************************
        //また、UDP通信とは別にTCP通信も待ち受け状態を作っておきます。
        // これをしないと、④でゲストから通信しようとしても通信先のホストが見つからず、Exceptionが発生しますのでご注意ください。

        ServerSocket serverSocket;
        Socket connectedSocket;
        int tcpPort = 3333;//ホスト、ゲストで統一  

        
        /// <summary>
        /// ゲストからの接続を待つ処理 
        /// </summary>
        async void connect()
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

        //③ホスト：ゲストから受信したIPアドレスに対してホスト端末のIPアドレスを送り返す。
        //①でIPアドレスを受け取るとreceiveUdpSocket.receive(packet);ここで止まっていた処理が再開します。
        //受信データ(IPアドレス)を文字列に変換して、IPアドレスを返すために用意したメソッドに受け取ったアドレスを引数に渡します。

        //returnIpAdress(address);   

        //処理はこんな感じです。




        
        /// <summary>
        /// ブロードキャスト発信者(ゲスト)にIPアドレスと端末名を返す
        /// </summary>
        /// <param name="address"></param>
        async void returnIpAdress(string address)
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

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <returns>The device name.</returns>
        public static string getDeviceName()
        {
            string manufacturer = Build.Manufacturer;
            string model = Build.Model;

            return model;
        }


        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <returns>The ip address.</returns>
        public string getIpAddress()
        {
            string IpAddress = "1234";
            return IpAddress;
        }

        /// <summary>
        /// Outputs the device name and ip.
        /// 端末名とIPアドレスのセットをゲストに送る  
        /// </summary>
        /// <param name="outputSocket">Output socket.</param>
        /// <param name="deviceName">Device name.</param>
        /// <param name="deviceAddress">Device address.</param>
        async void outputDeviceNameAndIp(Socket outputSocket, String deviceName, String deviceAddress)
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
    }
}
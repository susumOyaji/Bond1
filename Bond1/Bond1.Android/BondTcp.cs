using System;

using System.Net;
using System.Threading;
using System.Threading.Tasks;
//using System.Diagnostics;

using Android.App;
using Android.Content;
using Android.OS;
using Java.IO;
using Java.Net;
using Android.Net.Wifi;
using Android.Net;
using Bond1.Droid;



namespace Bond1.Droid
{
    public class BondTcp
    {
        bool waiting;
        int tcpPort = 3333;//ホスト、ゲストで統一  
        string thisIpAddress;



        public BondTcp()
        {
            thisIpAddress = getIpAddress();
        }


        /// <summary>
        /// Sends the ip adress.
        /// </summary>
        /// <param name="address">Address.</param>
        //発信者(ゲスト)にIPアドレスと端末名を送る  
        public void sendIpAdress(string address)//*** Host1 ***
        {
            Socket sendSocket = null;

            try
            {
                if (sendSocket != null)
                {
                    sendSocket.Close();
                    sendSocket = null;
                }
                if (sendSocket == null)
                {
                    sendSocket = new Socket(address, tcpPort);
                }
                //端末情報をホスト／ゲストに送る  
                outputDeviceNameAndIp(sendSocket, getdeviceName(), getIpAddress());
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
                    if (sendSocket != null)
                    {
                        sendSocket.Close();
                        sendSocket = null;
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
        /// Outputs the device name and ip.
        /// </summary>
        /// <param name="outputSocket">Output socket.</param>
        /// <param name="deviceName">Device name.</param>
        /// <param name="deviceAddress">Device address.</param>
        //端末名とIPアドレスのセットを送る  
        void outputDeviceNameAndIp(Socket outputSocket, String deviceName, String deviceAddress)//*** Host And Guest ***
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

       

        /// <summary>
        /// Getdevices the name.
        /// </summary>
        /// <returns>The name.</returns>
        private string getdeviceName()//*** Host And Guest ***
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets the IPA ddress.
        /// </summary>
        /// <returns>The IPA ddress.</returns>
        public string getIpAddress()//*** Host And Guest ***
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







        /// <summary>
        /// Inputs the device name and ip.
        /// </summary>
        /// <param name="socket">Socket.</param>
        //端末名とIPアドレスのセットを受け取る   
        public void inputDeviceNameAndIp(Socket socket)//*** Host And Guest ***//
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


       /// <summary>
       /// Sample device.
       /// </summary>
        public class SampleDevice//*** Host And Guest ***//
        {
            public void setDeviceName(string a)
            {
            }

            public void setDeviceIpAddress(string b)
            {

            }


        }

        /*    
        ゲストが端末情報を受け取るとき、本来はreturnしなくていいのですが、どうにも読み込みが完了してくれなかったので無理やり処理を完了させるためreturnさせています。
        もっとスマートなやり方があるはず…。
        */


        //------------------------------------ Guest ----------------------------------------

        /// <summary>
        /// Receiveds the host ip.
        /// </summary>
        //ホストからTCP通信でIPアドレスが送られてくるので、受け取るための待ち受け状態を作っておきます。
        //ホストからTCPでIPアドレスが返ってきたときに受け取るメソッド  
        public void receivedHostIp()//**** Guest ****
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

                    inputDeviceNameAndIp(socket);//端末名とIPアドレスのセットを受け取る  
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






        /// <summary>
        /// Hosts to connect.
        /// </summary>
        /// <param name="remoteIpAddress">Remote ip address.</param>
        //IPアドレスが判明したホストに対して接続を行う   
        public void HostToConnect(String remoteIpAddress)//*** Guest ***
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






    }
    }


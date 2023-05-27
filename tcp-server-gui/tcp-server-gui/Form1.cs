using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace tcp_server_gui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static List<SocketListener> listSocket = new List<SocketListener>();
        
        int port = 11111; 
         

        private void Form1_Load(object sender, EventArgs e)
        {

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);
            var ips = IPHelper.GetInterfaceIPAddress();
            Console.WriteLine(ipHost.HostName + " " + ipAddr.ToString());

            // Creation TCP/IP Socket using
            // Socket Class Constructor
            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Using Bind() method we associate a
                // network address to the Server Socket
                // All client that will connect to this
                // Server Socket must know this network
                // Address....
                listener.Bind(localEndPoint);

                // Using Listen() method we create
                // the Client list that will want
                // to connect to Server
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection ... ");
                    // Suspend while waiting for
                    // incoming connection Using
                    // Accept() method the server
                    // will accept connection of client
                    Socket clientSocket = listener.Accept();

                    string[] pembanding = clientSocket.RemoteEndPoint.ToString().Split(':');
                    int port = Int32.Parse(pembanding[1]);
                    SocketListener obj = new SocketListener(clientSocket, pembanding[0], port);

                    Form1.listSocket.Add(obj);
                    Console.WriteLine("---LIST CLIENT---");
                    for (int i = 0; i < Form1.listSocket.Count; i++)
                    {
                        SocketListener o = Form1.listSocket[i];

                        Console.WriteLine($" ({i+1}) USERNAME - IP - PORT : {o.username} - {o.lockIp} - {o.port}");

                    }
                    Console.WriteLine("--- END LIST CLIENT---");

                    Thread tr = new Thread(new ThreadStart(obj.newClient)); 
                    tr.Start();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }
    }
    public class SocketListener
    {
        public Socket clientSocket;
        public string lockIp;
        public int port;
        public string username = "";

        public SocketListener(Socket clientSocket, string ipParam,int portParam)
        {
            this.clientSocket = clientSocket;
            this.lockIp = ipParam;
            this.port = portParam;
        }

        public void newClient()
        {

            // Data buffer
            byte[] bytes = new Byte[1024];
            string data = null;

            while (true)
            { 
                EndPoint sender = clientSocket.RemoteEndPoint;

                int numByte = clientSocket.Receive(bytes);

                data += Encoding.ASCII.GetString(bytes, 0, numByte);

                //if (data.IndexOf("\n") > -1)
                //{
                //    Console.WriteLine("From " + (((IPEndPoint)sender).Address.ToString() ?? "-") + " : " + data);
                //    //clientSocket.Send(Encoding.ASCII.GetBytes("\nPong : " + data + "\n"));
                //    data = "";
                //}
                 
                string ipsender = ((IPEndPoint)sender).Address.ToString();

                if (data.IndexOf("<EOF>") > -1)
                { 
                    string originalMsg = data.Substring(0, data.IndexOf("<EOF>")); 

                    string[] arr = originalMsg.Split('|');
                    string action = arr[0]; //SEND
                    string usernameSender = arr[1];
                    string msg = arr[2];

                    string ipReceive = arr[3];
                    string[] ipReceiveOriginArr = ipReceive.Split(':');

                    string ipReceiveOrigin = ipReceiveOriginArr[0];
                    string portReceiveOrigin = ipReceiveOriginArr[1];

                    //Console.WriteLine("------------");
                    //Console.WriteLine($"ACTION : {action}");
                    //Console.WriteLine(usernameSender);
                    //Console.WriteLine(ipsender);
                    //Console.WriteLine(ipReceive);
                    //Console.WriteLine(msg);  
                    //Console.WriteLine("------------");

                    Console.WriteLine($"FROM {ipsender} to {ipReceiveOrigin} : {msg} ");

                    //send to received ip
                     
                    int idx = -1;   
                    for (int i = 0; i < Form1.listSocket.Count; i++)
                    {
                        SocketListener o = Form1.listSocket[i];
                        Console.WriteLine(ipReceiveOrigin + " - " + o.lockIp.ToString());

                        //if (o.lockIp == ipReceiveOrigin && o.port.ToString()==portReceiveOrigin)
                        //{
                        //    idx = i;
                        //} 
                        if (o.lockIp == ipReceiveOrigin)
                        {
                            idx = i;
                        }
                    }
                    if (idx != -1)
                    {
                        IPAddress rcvIp = IPAddress.Parse(ipReceiveOrigin);
                        IPEndPoint responsetarget = new IPEndPoint(rcvIp, Int32.Parse(portReceiveOrigin)); 

                        Form1.listSocket[idx].clientSocket.SendTo(IPHelper.MsgToByte($"{usernameSender} :"+msg), responsetarget);
                    }
                    else
                    {
                        clientSocket.Send(IPHelper.MsgToByte("Target not found :(("));
                    }

                    if (action == "BYE")
                    {
                        for (int i = 0; i < Form1.listSocket.Count; i++)
                        {
                            SocketListener o = Form1.listSocket[i]; 

                            if (o.lockIp == ipsender)
                            {
                                idx = i;
                            }
                        }
                        Form1.listSocket.RemoveAt(idx); 
                        break;
                    } 
                    data = null;
                }
                else
                {
                    //resiko kalo ngechat dari pc server :")

                    string originalMsg = data; 

                    string[] arr = originalMsg.Split('|');
                    string action = arr[0]; //SEND
                    string usernameSender = arr[1];
                    string msg = arr[2];

                    int idxCariUsername = -1;
                    for (int i = 0; i < Form1.listSocket.Count; i++)
                    {
                        SocketListener o = Form1.listSocket[i];

                        if (o.lockIp == ipsender)
                        {
                            idxCariUsername = i;
                        }
                    }

                    Form1.listSocket[idxCariUsername].username = usernameSender; 
                }
            }
             

            // Send a message to Client
            // using Send() method
            //clientSocket.Send(message);

            // Close client Socket using the
            // Close() method. After closing,
            // we can use the closed Socket
            // for a new Client Connection
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

    }
    public struct PeerDetail
    {
        public PeerDetail(string ip, string username)
        {
            this.ipAddress = ip;
            this.username = username;
        }

        public readonly string ipAddress;
        public readonly string username;

        public IPAddress GetIPAddress()
        {
            return IPAddress.Parse(this.ipAddress);
        }
    }
    public struct ChatRow
    {
        public PeerDetail from;
        public PeerDetail to;
        public string message;
    }
}

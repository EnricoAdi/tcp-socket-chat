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

namespace tcp_client_gui.NewServer
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        public static List<SocketListener> listSocket = new List<SocketListener>();

        int port = 11111;

        string name;
        private void Server_Load(object sender, EventArgs e)
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

                    Console.WriteLine($"INI PORT : {Server.listSocket.Count + 1} : " + pembanding[1]);

                    Server.listSocket.Add(obj);

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
        private Socket clientSocket;
        private string lockIp;
        private int port;

        public SocketListener(Socket clientSocket, string ipParam, int portParam)
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
                Console.WriteLine("baru");
                EndPoint sender = clientSocket.RemoteEndPoint;

                int numByte = clientSocket.Receive(bytes);

                data += Encoding.ASCII.GetString(bytes, 0, numByte);

                //if (data.IndexOf("\n") > -1)
                //{
                //    Console.WriteLine("From " + (((IPEndPoint)sender).Address.ToString() ?? "-") + " : " + data);
                //    //clientSocket.Send(Encoding.ASCII.GetBytes("\nPong : " + data + "\n"));
                //    data = "";
                //}

                if (data.IndexOf("<EOF>") > -1)
                {
                    string originalMsg = data.Substring(0, data.IndexOf("<EOF>"));

                    //example  <USERNAMESENDER>asd<MESSAGE>asd<EOF>10.10.3.18 
                    //string username = originalMsg.Substring(16,originalMsg.Length-data.IndexOf("<MESSAGE>"));
                    //string ipsender = ((IPEndPoint)sender).Address.ToString();
                    //string ipReceive = data.Substring(data.IndexOf("<EOF>")+5);
                    //string msg = originalMsg.Substring(originalMsg.IndexOf("<MESSAGE>") + 9, originalMsg.Length - (originalMsg.IndexOf("<MESSAGE>") + 9));

                    string[] arr = originalMsg.Split('|');
                    string action = arr[0]; //SEND
                    string usernameSender = arr[1];
                    string msg = arr[2];

                    string ipReceive = arr[3];
                    string[] ipReceiveOriginArr = ipReceive.Split(':');
                    string ipReceiveOrigin = ipReceiveOriginArr[0];
                    string portReceiveOrigin = ipReceiveOriginArr[1];
                    string ipsender = ((IPEndPoint)sender).Address.ToString();

                    Console.WriteLine("------------");
                    Console.WriteLine($"ACTION : {action}");
                    Console.WriteLine(usernameSender);
                    Console.WriteLine(ipsender);
                    Console.WriteLine(ipReceive);
                    Console.WriteLine(msg);

                    Console.WriteLine("------------");

                    //Console.WriteLine("From " + (((IPEndPoint)sender).Address.ToString() ?? "-")  + " : " + data);
                    //clientSocket.Send(Encoding.ASCII.GetBytes("\nPong : " + data + "\n"));

                    //send to received ip
                     
                    int idx = -1;
                    for (int i = 0; i < Server.listSocket.Count; i++)
                    {
                        SocketListener o = Server.listSocket[i];
                        Console.WriteLine(portReceiveOrigin + " - " + o.port.ToString());

                        if (o.lockIp == ipReceiveOrigin && o.port.ToString() == portReceiveOrigin)
                        {
                            idx = i;
                        }
                    }
                    if (idx != -1)
                    {
                        IPAddress rcvIp = IPAddress.Parse(ipReceiveOrigin);
                        IPEndPoint responsetarget = new IPEndPoint(rcvIp, Int32.Parse(portReceiveOrigin));
                        clientSocket.SendTo(IPHelper.MsgToByte(msg), responsetarget);
                        Server.listSocket[idx].clientSocket.SendTo(IPHelper.MsgToByte(msg), responsetarget);
                    }
                    else
                    {
                        clientSocket.Send(IPHelper.MsgToByte("Target not found :(("));
                    }

                    if (action == "BYE")
                    {
                        break;
                    }

                    data = null;
                }
            }

            //Console.WriteLine("Text received -> {0} ", data);
            //byte[] message = Encoding.ASCII.GetBytes("\nTest Server");

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

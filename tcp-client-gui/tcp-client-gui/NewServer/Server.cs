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

        private void Server_Load(object sender, EventArgs e)
        {

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);
            var ips = IPHelper.GetInterfaceIPAddress();
            Console.WriteLine(ipHost.HostName + " " + ipAddr.ToString());

            // Creation TCP/IP Socket using 
            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            try
            {
                 
                listener.Bind(localEndPoint);

                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection ... ");

                    Socket clientSocket = listener.Accept();

                    string[] pembanding = clientSocket.RemoteEndPoint.ToString().Split(':');
                    int port = Int32.Parse(pembanding[1]);

                    SocketListener obj = new SocketListener(clientSocket, pembanding[0], port);

                    Server.listSocket.Add(obj);

                    Console.WriteLine("---LIST CLIENT---");
                     

                    for (int i = 0; i < Server.listSocket.Count; i++)
                    {
                        SocketListener o = Server.listSocket[i];

                        Console.WriteLine($" ({i + 1}) USERNAME - IP - PORT : {o.username} - {o.lockIp} - {o.port}");
                          
                    }
                    Console.WriteLine("--- END LIST CLIENT---");

                    
                    //ngirim list username
                    Thread tr = new Thread(new ThreadStart(obj.newClient));
                     

                    tr.Start();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        public static void printListSocket()
        {
            for (int i = 0; i < Server.listSocket.Count; i++)
            {
                SocketListener o = Server.listSocket[i];

                Console.WriteLine($" ({i + 1}) USERNAME - IP - PORT : {o.username} - {o.lockIp} - {o.port}"); 

            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }

    public class SocketListener
    {
        public Socket clientSocket;
        public string lockIp;
        public int port;
        public string username = "";

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
                EndPoint sender = clientSocket.RemoteEndPoint;

                int numByte = clientSocket.Receive(bytes);

                data += Encoding.ASCII.GetString(bytes, 0, numByte);
                 

                string ipsender = ((IPEndPoint)sender).Address.ToString();

                if (data.IndexOf("<EOF>") > -1)
                {
                    string originalMsg = data.Substring(0, data.IndexOf("<EOF>"));

                    string[] arr = originalMsg.Split('|');
                    string action = arr[0]; //SEND
                    string usernameSender = arr[1];
                    string msg = arr[2];

                    string usernameReceive = arr[3];
                    //string usernameReceive = 
                    //string[] ipReceiveOriginArr = ipReceive.Split(':');

                    //string ipReceiveOrigin = ipReceiveOriginArr[0];
                    //string portReceiveOrigin = ipReceiveOriginArr[1];


                    if (action == "ASK")
                    {
                        string listUsername = "";

                        for (int i = 0; i < Server.listSocket.Count; i++)
                        {
                            SocketListener o = Server.listSocket[i];
                            listUsername += $"{o.username}-";
                        }
                        clientSocket.Send(IPHelper.MsgToByte(listUsername)); 
                    }
                    if (action == "SEND")
                    {   
                        Console.WriteLine($"FROM {ipsender} to {usernameReceive} : {msg} ");

                        //send to received ip

                        int idx = -1;
                        for (int i = 0; i < Server.listSocket.Count; i++)
                        {
                            SocketListener o = Server.listSocket[i]; 
                            if (o.username == usernameReceive)
                            {
                                idx = i;
                            }
                        }
                        if (idx != -1)
                        {
                            IPAddress rcvIp = IPAddress.Parse(Server.listSocket[idx].lockIp);
                            IPEndPoint responsetarget = new IPEndPoint(rcvIp, Server.listSocket[idx].port);

                            Server.listSocket[idx].clientSocket.SendTo(IPHelper.MsgToByte($"{usernameSender} :" + msg), responsetarget);
                        }
                        else
                        {
                            clientSocket.Send(IPHelper.MsgToByte("Target not found :(("));
                        }
                    }

                    if (action == "BYE")
                    {
                        int idx = -1;
                        for (int i = 0; i < Server.listSocket.Count; i++)
                        {
                            SocketListener o = Server.listSocket[i];

                            if (o.lockIp == ipsender)
                            {
                                idx = i;
                            }
                        }
                        Server.listSocket.RemoveAt(idx);
                        break;
                    }

                    if (action == "HELLO")
                    {  
                        if (action == "HELLO")
                        {  
                            int idxCariUsername = -1;
                            for (int i = 0; i < Server.listSocket.Count; i++)
                            {
                                SocketListener o = Server.listSocket[i];

                                if (o.lockIp == ipsender)
                                {
                                    idxCariUsername = i;
                                }
                            }

                            Server.listSocket[idxCariUsername].username = usernameSender;

                            Server.printListSocket();
                        }
                    }

                    data = null;
                }
                else
                {
                    //resiko kalo ngechat dari pc server :")
                }
            }

             
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

    }
     
}

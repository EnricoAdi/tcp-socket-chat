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

        private void Server_Load(object sender, EventArgs e)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);
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

                    SocketListener obj = new SocketListener(clientSocket);
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

        public SocketListener(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
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

                if (data.IndexOf("\n") > -1)
                {
                    Console.WriteLine("From " + (((IPEndPoint)sender).Address.ToString() ?? "-") + " : " + data);
                    clientSocket.Send(Encoding.ASCII.GetBytes("\nPong : " + data + "\n"));
                    data = "";
                }

                if (data.IndexOf("<EOF>") > -1)
                {
                    string msg = data.Substring(0,data.Length - data.IndexOf("<EOF>")-5);
                    Console.WriteLine(msg);
                    Console.WriteLine("From " + (((IPEndPoint)sender).Address.ToString() ?? "-")
                        + " : " + data);

                    //clientSocket.Send(Encoding.ASCII.GetBytes("\nPong : " + data + "\n"));
                    break;
                }
            }

            Console.WriteLine("Text received -> {0} ", data);
            byte[] message = Encoding.ASCII.GetBytes("\nTest Server");

            // Send a message to Client
            // using Send() method
            clientSocket.Send(message);

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

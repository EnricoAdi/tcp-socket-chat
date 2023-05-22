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
namespace tcp_client_gui
{
    public partial class FormChatServer : Form
    {
        public FormChatServer(string nameParam)
        {
            InitializeComponent();
            name = nameParam;
        }

        Socket sck;
        Socket acc;
        int port = 11111;
        IPAddress ip;
        Thread rec;

        string name;
        private void FormChatServer_Load(object sender, EventArgs e)
        { 
            try
            { 
                Console.WriteLine("Waiting for connection");
                rec = new Thread(recV);
                 
                lblIP.Text = "IP : " + GetIp();
                lblTitle.Text = name;

                ip = IPAddress.Parse(GetIp());
                sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sck.Bind(new IPEndPoint(ip, port));

                sck.Listen(0);
                acc = sck.Accept();
                rec.Start();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
        }

        private void FormChatServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainMenuServer m = new MainMenuServer();
            m.Show();
        }

        string GetIp()
        {
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();
        }

        private void recV()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(500);
                    byte[] Buffer = new byte[255];
                    int rec = acc.Receive(Buffer, 0, Buffer.Length, 0);
                    Array.Resize(ref Buffer, rec);

                    string msgGet = Encoding.Default.GetString(Buffer);

                    this.Invoke(new Action(() => this.addChat(msgGet)));


                    string[] ipSenderArr = acc.RemoteEndPoint.ToString().Split(':');
                    string ipSender = ipSenderArr[0];
                    Console.WriteLine(ipSender);
                }
            }
            catch (Exception)
            {
                rec.Abort();
                MessageBox.Show("Disconnected");
            }

        }
        void sendChat(string message)
        {
            try
            {
                //byte[] sdata = Encoding.Default.GetBytes("<" + name + ">" + message);
                byte[] sdata = Encoding.Default.GetBytes(name + ": " + message);

                acc.Send(sdata, 0, sdata.Length, 0);
            }
            catch (Exception)
            {
                MessageBox.Show("Disconnected");
                this.Close();
            }
        }
        public void addChat(string msg)
        {
            listchat.Items.Add(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string message = txtMsg.Text;
            txtMsg.Text = "";

            addChat("You : " + message);

            sendChat(message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace tcp_client_gui
{
    public partial class FormChatClient : Form
    {
        public FormChatClient()
        {
            InitializeComponent(); 
            //name = uname;
            //try
            //{
            //    ipServer = IPAddress.Parse(ipChoose); 
            //    port = portChoose;
            //}
            //catch (Exception)
            //{ 
            //    ipServer = IPAddress.Parse(defaultIP);
            //    ipTujuan = defaultIP; 
            //    port = 11111;
            //}
        }

        string defaultIP = "192.168.1.5"; 
        int port = 11111;
        IPAddress ipServer;

        string ipTujuan;

        Thread rec;
        string name = ""; 

        private void button1_Click(object sender, EventArgs e)
        { 
            string message = txtMsg.Text; 
            txtMsg.Text = "";
             
            addChat("You : " + message);

            sendChat(message); 
        }
        public void addChat(string msg)
        {
            if (msg != "")
            { 
                listchat.Items.Insert(0, msg);
            }
        }

        void sendChat(string message)
        { 
            byte[] sdata = Encoding.Default.GetBytes($"SEND|{name}|{message}|{SocketClient.usernameTujuan}<EOF>");
            SocketClient.socket.Send(sdata, 0, sdata.Length, 0);
        }
        void endChat()
        { 
            byte[] sdata = Encoding.Default.GetBytes($"BYE|{name}|END|{SocketClient.usernameTujuan}<EOF>");
            SocketClient.socket.Send(sdata, 0, sdata.Length, 0);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Hello, "+name; 
            rec = new Thread(recV);  
            try
            {  
                rec.Start();  
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to connect");
                this.Close();
                MainMenuClientChooseIP m = new MainMenuClientChooseIP();
                m.Show();
            }
             
        } 

        private void recV()
        {
            try
            { 
                while (true)
                {
                    Thread.Sleep(500);
                    byte[] Buffer = new byte[255];
                    int rec = SocketClient.socket.Receive(Buffer, 0, Buffer.Length, 0);
                    Array.Resize(ref Buffer, rec);

                    string msgGet = Encoding.Default.GetString(Buffer);
                    if (msgGet != "")
                    {
                        if (msgGet.IndexOf("****") < 0)
                        {
                            //buat akses form 
                            this.Invoke(new Action(() => this.addChat(msgGet))); 

                        }
                    } 
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Disconnected"); 
            }
        }

        private void FormChatClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            endChat();
            MainMenuClientChooseIP m = new MainMenuClientChooseIP();
            m.Show();
        }
    }
}

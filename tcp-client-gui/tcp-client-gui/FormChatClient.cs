﻿using System;
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
        public FormChatClient(string uname, string ipChoose, int portChoose, string ipTo)
        {
            InitializeComponent(); 
            name = uname;
            try
            {
                ipServer = IPAddress.Parse(ipChoose);
                ipTujuan = ipTo;
                txtTo.Text = ipTo;
                port = portChoose;
            }
            catch (Exception)
            {

                ipServer = IPAddress.Parse(defaultIP);
                ipTujuan = defaultIP;
                txtTo.Text = ipTo;
                port = 11111;
            }
        }

        string defaultIP = "192.168.1.5";
        Socket sck;
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
            //byte[] sdata = Encoding.Default.GetBytes("<" + name + ">" + message);
            //byte[] sdata = Encoding.Default.GetBytes($"<USERNAMESENDER>{name}<MESSAGE>{message}<EOF>{ipTujuan}");
            byte[] sdata = Encoding.Default.GetBytes($"SEND|{name}|{message}|{txtTo.Text}<EOF>");
            sck.Send(sdata, 0, sdata.Length, 0);
        }
        void endChat(string message)
        { 
            byte[] sdata = Encoding.Default.GetBytes($"BYE|{name}|{message}|{txtTo.Text}<EOF>");
            sck.Send(sdata, 0, sdata.Length, 0);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Hello, "+name; 
            rec = new Thread(recV);
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {

            sck.Connect(new IPEndPoint(ipServer, port));

            rec.Start();

            //byte[] conmsg = Encoding.Default.GetBytes("<" + name + ">" + "Connected");
            //byte[] conmsg = MsgToByte( name + " Connected");
            //sck.Send(conmsg, 0, conmsg.Length, 0);

            }
            catch (Exception)
            {
                MessageBox.Show("Failed to connect");
                this.Close();
                MainMenuClient m = new MainMenuClient();
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
                    int rec = sck.Receive(Buffer, 0, Buffer.Length, 0);
                    Array.Resize(ref Buffer, rec);

                    string msgGet = Encoding.Default.GetString(Buffer);
                    if (msgGet != "")
                    {
                        Console.WriteLine(msgGet); 

                        //buat akses form 
                        this.Invoke(new Action(() => this.addChat(msgGet))); 
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Disconnected");

                //this.Close(); 
            }
        }

        private void FormChatClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            endChat("end");
            MainMenuClient m = new MainMenuClient();
            m.Show();
        }
    }
}

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
    public partial class MainMenuClientChooseTujuan : Form
    {
        public MainMenuClientChooseTujuan()
        {
            InitializeComponent();
        }
        private void recV()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1000); 
                    byte[] msg = IPHelper.MsgToByte($"ASK|{SocketClient.username}|hola|192.168.10.1:11111<EOF>");

                    SocketClient.socket.Send(msg, 0, msg.Length, 0);

                    byte[] Buffer = new byte[255];
                    int rec = SocketClient.socket.Receive(Buffer, 0, Buffer.Length, 0);
                    Array.Resize(ref Buffer, rec);

                    string msgGet = Encoding.Default.GetString(Buffer);
                    if (msgGet != "")
                    {
                        Console.WriteLine(msgGet);

                        //buat akses form 
                        //this.Invoke(new Action(() => this.addChat(msgGet)));
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Disconnected");
            }
        }
        private void MainMenuClientChooseTujuan_Load(object sender, EventArgs e)
        {
            try
            { 
                Thread rec;
                rec = new Thread(recV);
                rec.Start();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
        }


        public void addListFriend(string msg)
        {
            if (msg != "")
            {
                //listchat.Items.Insert(0, msg);
            }
        }
    }
}

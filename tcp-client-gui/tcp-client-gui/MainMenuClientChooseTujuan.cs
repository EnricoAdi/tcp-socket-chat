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
                    Thread.Sleep(2000); 
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
                        this.Invoke(new Action(() => this.addListFriend(msgGet)));
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Disconnected "+exc.Message.ToString());
            }
        }
        Thread rec;
        private void MainMenuClientChooseTujuan_Load(object sender, EventArgs e)
        {
            try
            { 
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
                listFriend.Items.Clear();
                string[] listUsername = msg.Split('-'); 
                if(listUsername.Length > 0 && listUsername[0] == "****")
                { 
                    for (int i = 0; i < listUsername.Length; i++)
                    {
                        if (listUsername[i] != "****" && listUsername[i]!=SocketClient.username)
                        {
                            listFriend.Items.Insert(0, listUsername[i]); 
                        }
                    }
                }
                 
            }
        }

        public void pindah(string username)
        {
            SocketClient.usernameTujuan = username;

            rec.Suspend();
            FormChatClient f = new FormChatClient();
            f.Show();
            this.Hide();

        }
        private void listFriend_DoubleClick(object sender, EventArgs e)
        {
            //string usernameTerpilih = listFriend.SelectedItem.ToString();

            //pindah(usernameTerpilih);
        }

        private void MainMenuClientChooseTujuan_FormClosed(object sender, FormClosedEventArgs e)
        { 
            byte[] sdata = Encoding.Default.GetBytes($"BYE|{SocketClient.username}|END|192.168.10.1:11111<EOF>");
            SocketClient.socket.Send(sdata, 0, sdata.Length, 0);
            rec.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //connect

            string usernameTerpilih = txtUsername.Text;

            if (usernameTerpilih != "")
            {
                pindah(usernameTerpilih);  
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //disconnect
        }

        private void listFriend_Click(object sender, EventArgs e)
        { 
            string usernameTerpilih = listFriend.SelectedItem.ToString();
            txtUsername.Text = usernameTerpilih;
        }
    }
}

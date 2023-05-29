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

            byte[] msg = IPHelper.MsgToByte($"SEND|{SocketClient.username}|hola|192.168.10.1<EOF>");

            SocketClient.socket.Send(msg, 0, msg.Length, 0);
        }

        private void MainMenuClientChooseTujuan_Load(object sender, EventArgs e)
        {

        }
    }
}

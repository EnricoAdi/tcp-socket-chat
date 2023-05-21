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

namespace tcp_client_gui
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string choosePartner = listPartner.SelectedItem.ToString();
            string username = txtUsername.Text;
            int port = 11111;
            if (username == "" || choosePartner=="")
            {
                MessageBox.Show("Username dan partner harus diisi");
                return;
            }
             
            Form1 f = new Form1(username,choosePartner,port);
            f.Show();
            this.Hide();
            

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            for (int i = 0; i < ipHost.AddressList.Length; i++)
            {
                listPartner.Items.Add(ipHost.AddressList[i]);
            }
        }
    }
}

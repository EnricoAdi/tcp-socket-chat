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
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Timers;

namespace tcp_client_gui
{
    public partial class MainMenuClient : Form
    {
        public MainMenuClient()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string choosePartner = txtIP.Text.ToString();
            string username = txtUsername.Text;
            int port = 11111;
            if (username == "" || choosePartner=="")
            {
                MessageBox.Show("Username dan partner harus diisi");
                return;
            }

            FormChatClient f = new FormChatClient(username, choosePartner, port);
            f.Show();
            this.Hide();
            

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName()); 

            var nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var nic in nics)
            {
                var ipProps = nic.GetIPProperties();

                // We're only interested in IPv4 addresses for this example.
                var ipv4Addrs = ipProps.UnicastAddresses
                    .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Where(addr => addr.Address.ToString() != "127.0.0.1") // Exclude localhost
                    .Where(addr => addr.Address.ToString().Substring(0, 7) != "169.254"); // Exclude DHCP IP, Windows based

                foreach (var addr in ipv4Addrs)
                {
                    var network = IPHelper.CalculateNetwork(addr);
                    var broadcast = IPHelper.GetBroadcastAddress(addr);

                    if (network != null)
                    { 
                            Console.WriteLine("Addr: {0}   Mask: {1}  Network: {2} Broadcast : {3}", addr.Address, addr.IPv4Mask, network, broadcast);

                            listPartner.Items.Add(addr.Address);
                    }
                }
            }
        }

        private void MainMenuClient_FormClosed(object sender, FormClosedEventArgs e)
        { 
            MainMenu m = new MainMenu();
            m.Show();
        }

        private void listPartner_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIP.Text = listPartner.SelectedItem.ToString();
        }

 
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tcp_client_gui
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            cbbRole.Items.Add("Server");
            cbbRole.Items.Add("Client");
            cbbRole.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbbRole.SelectedIndex == 0)
            {
                //server
                MainMenuServer m = new MainMenuServer();
                m.Show();
                this.Hide();
            }
            else
            {
                //client
                MainMenuClient m = new MainMenuClient();
                m.Show();
                this.Hide();
            }
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

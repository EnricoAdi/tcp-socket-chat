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
    public partial class MainMenuServer : Form
    {
        public MainMenuServer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtUsername.Text;
            if (name == "")
            {
                MessageBox.Show("Nama harus diisi");
                return;
            }
            FormChatServer f = new FormChatServer(name);
            f.Show();
            this.Hide();
        }

        private void MainMenuServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainMenu m = new MainMenu();
            m.Show();
        }
    }
}

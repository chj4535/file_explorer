using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace file_explorer
{
    public partial class Main_form : Form
    {
        string userId;
        clientsocketHandler client_socket = new clientsocketHandler();

        public Main_form(string loginformUserId)
        {
            InitializeComponent();
            userId = loginformUserId;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user_name_label.Text = userId;
        }
        
    }
}

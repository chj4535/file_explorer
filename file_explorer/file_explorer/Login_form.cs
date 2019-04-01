using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using file_explorer;
namespace file_explorer
{
    public partial class Login_form : Form
    {
        clientsocketHandler client_socket = new clientsocketHandler();
        private readonly System.Threading.EventWaitHandle waitHandle = new System.Threading.AutoResetEvent(false);
        public Login_form()
        {
            InitializeComponent();
        }

        private void check_button_Click(object sender, EventArgs e)
        {
            if (IdPwcheck()) // id , pw  모두 입력 됐으면
            {
                string user_id = user_id_textbox.Text;
                string user_pw = user_pw_textbox.Text;
                string[] client_info = new string[2];
                client_info[0] = user_id;
                client_info[1] = user_pw;
                client_socket.set_socket_evnet(login_success);
                client_socket.OnSendData("login"+";"+client_info[0] + ";" + client_info[1],null);
                waitHandle.WaitOne();
                MessageBox.Show("로그인 성공!","확인", MessageBoxButtons.OK, MessageBoxIcon.None);
                Main_form main_form = new Main_form();
                this.Hide();
                main_form.ShowDialog();
                if (main_form.DialogResult != DialogResult.OK)
                {
                    this.Dispose();
                }
            }
        }

        private bool IdPwcheck()
        {
            if (String.IsNullOrEmpty(user_id_textbox.Text))
            {
                MessageBox.Show("아이디를 입력해 주세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                user_id_textbox.Focus();
                return false;
            }
            else if (String.IsNullOrEmpty(user_pw_textbox.Text))
            {
                MessageBox.Show("비밀번호를 입력해 주세요!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                user_pw_textbox.Focus();
                return false;
            }
            return true;
        }
        private void cancel_button_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void login_success(string name)
        {
            waitHandle.Set();
        }
    }
}

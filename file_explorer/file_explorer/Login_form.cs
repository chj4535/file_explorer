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

        bool loginstate = false;
        string userId;
        LoginCheck loginCheck = new LoginCheck();
        ClientSocketHandler clientSocket = new ClientSocketHandler();
        private readonly System.Threading.EventWaitHandle waitHandle = new System.Threading.AutoResetEvent(false);
        public Login_form()
        {
            loginCheck.SetLoginEvnet(LoginCheck);
            InitializeComponent();
        }

        private void check_button_Click(object sender, EventArgs e)
        {
            if (IdPwcheck()) // id , pw  모두 입력 됐으면
            {
                userId = user_id_textbox.Text;
                string userpw = user_pw_textbox.Text;
                string[] client_info = new string[2];
                client_info[0] = userId;
                client_info[1] = userpw;
                clientSocket.OnSendData("login"+"|"+client_info[0] + "/" + client_info[1]+"|",null);
                waitHandle.WaitOne(); //로그인 결과가 올때까지 대기 상태가 되어야 한다.
                if (loginstate)
                {


                    MessageBox.Show("로그인 성공!", "확인", MessageBoxButtons.OK, MessageBoxIcon.None);
                    Main_form main_form = new Main_form(userId);
                    this.Invoke(new MethodInvoker(this.Hide)); // 크로스 스레드 해결
                    main_form.ShowDialog();
                    if (main_form.DialogResult != DialogResult.OK)
                    {
                        this.Invoke(new MethodInvoker(this.Dispose));  // 크로스 스레드 해결
                    }
                }
                else
                {
                    MessageBox.Show("로그인 실패!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoginCheck(string result)
        {
            if (result.Equals("success"))
            {
                loginstate = true;
            }
            else
            {
                loginstate = false;
            }
            waitHandle.Set();//로그인 결과 대기 해제
        }

        private void user_pw_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                check_button_Click(sender, null);
            }
        }
    }
}

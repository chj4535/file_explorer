using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_explorer
{
    public delegate void LoginEventHandler(string data);
    class LoginCheck
    {

        static event LoginEventHandler loginEvent;

        public void SetLoginEvnet(LoginEventHandler add_event)
        {
            loginEvent += new LoginEventHandler(add_event);
        }

        public void LoginResult(string loginResult)
        {
            if (loginResult.Equals("success"))
            {
                Console.WriteLine("소켓 수신 : 로그인 성공");
                loginEvent("success");
            }
            else if (loginResult.Equals("fail"))
            {
                Console.WriteLine("소켓 수신 : 로그인 실패");
                loginEvent("fail");
            }
        }
    }
}

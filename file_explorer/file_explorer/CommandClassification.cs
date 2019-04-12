using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_explorer
{
    class CommandClassification
    {
        LoginCheck loginCheck = new LoginCheck();
        MakeData makeStatedata = new MakeData();
        public void CmdClassification(string msg)
        {
            string[] msgs = msg.Split('|');
            string msgCount = msgs[0];
            string itemCount = msgs[1];
            Console.WriteLine(msgs[1]);
            switch (msgs[1])
            {
                case "login":
                    loginCheck.LoginResult(msgs[2]);
                    break;
                case "file":
                   // makeStatedata.SetFile(msgCount,msgs[2], msgs[3]);
                    break;
                case "dir":
                    //makeStatedata.SetDir(msgCount,msgs[2], msgs[2]);
                    break;
                case "drive":
                    //makeStatedata.SetDrive(msgCount,msgs[1],msgs[2]);
                    break;
            }
        }
    }
}

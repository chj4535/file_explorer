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
        LoadDriveInfo loadDriveinfo = new LoadDriveInfo();
        LoadDirSubItemsInfo loadDirsubitemsinfo = new LoadDirSubItemsInfo();
        public void CmdClassification(int msgCount, string msg)
        {
            string[] msgs = msg.Split('|');
            Console.WriteLine(msgs[1]);
            switch (msgs[0])
            {
                case "login":
                    loginCheck.LoginResult(msgs[1]);
                    break;
                case "rootload":
                    loadDriveinfo.LoadDriverInfoResult(msgCount,msgs[1]);
                    break;
                case "dirload":
                    loadDirsubitemsinfo.LoadDirSubItemsInfoResult(msgCount, msgs[1], msgs[2]);
                    break;
            }
        }
    }
}

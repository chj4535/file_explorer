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
        MakeStateData makeStatedata = new MakeStateData();
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
                    makeStatedata.LoadDriverInfoResult(msgCount,msgs[1]);
                    break;
                case "dirload":
                    makeStatedata.LoadDirSubItemsInfoResult(msgCount, msgs[1], msgs[2],msgs[3]);
                    break;
                case "MoveItemToDir":
                    //loadDirsubitemsinfo.LoadDirSubItemsInfoResult(msgCount, msgs[1], msgs[2], msgs[3]);
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace socket_server
{
    class CommandClassification
    {
        Logincheck loginCheck = new Logincheck();
        Driveinfo drinInfo = new Driveinfo();
        GetDirSubItems getDrisubitems = new GetDirSubItems();
        MoveFilesAndDirs moveFilesAndDirs = new MoveFilesAndDirs();
        public byte[] CmdClassification(string msg)
        {
            string[] msgs = msg.Split('|');
            byte[] cmd = new byte[0];
            byte[] cmdResult = new byte[0];
            //Console.WriteLine("msgs[0] : " + msgs[0]);
            //Console.WriteLine("msgs[1] : " + msgs[1]);
            //Console.WriteLine("msgs[2] : " + msgs[2]);
            switch (msgs[0]) {
                case "login":
                    cmd = Encoding.UTF8.GetBytes("login"+'|');
                    cmdResult = loginCheck.GetloginInfo(msgs[1]);
                    break;
                case "rootload":
                    cmd = Encoding.UTF8.GetBytes("rootload" + '|');
                    cmdResult = drinInfo.GetDriveInfo();
                    break;
                case "dirload":
                    cmd = Encoding.UTF8.GetBytes("dirload" + '|');
                    cmdResult = getDrisubitems.GetFilesDirs(msgs[1]);
                    break;
                case "MoveItemToDir":
                    cmd = Encoding.UTF8.GetBytes("MoveItemToDir" + '|');
                    cmdResult = moveFilesAndDirs.MvoeItems(msgs[1],msgs[2],msgs[3],msgs[4]);
                    break;
            }

            byte[] data = new byte[cmd.Length + cmdResult.Length];
            cmd.CopyTo(data, 0);
            cmdResult.CopyTo(data, cmd.Length);
            return data;
        }

        public bool IdentifySendAll(string msg)
        {
            string[] msgs = msg.Split('|');

            bool sendAll = false;
            switch (msgs[0])
            {
                case "login":
                    sendAll = false;
                    break;
                case "rootload":
                    sendAll = false;
                    break;
                case "dirload":
                    sendAll = false;
                    break;
                case "MoveItemToDir":
                    sendAll = true;
                    break;
            }
            return sendAll;
        }
    }
}

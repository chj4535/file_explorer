using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace socket_server
{
    class CommandClassification
    {
        MakeData makeData = new MakeData();
        Logincheck loginCheck = new Logincheck();
        Driveinfo drinInfo = new Driveinfo();
        GetDirSubItems getDrisubitems = new GetDirSubItems();
        MoveFilesAndDirs moveFilesAndDirs = new MoveFilesAndDirs();
        public void CmdClassification(Socket clientSocket,bool isSendall,string msg,int msgCount)
        {
            string[] msgs = msg.Split('|');
            byte[] cmd = new byte[0];
            byte[] cmdResult = new byte[0];
            //Console.WriteLine("msgs[0] : " + msgs[0]);
            //Console.WriteLine("msgs[1] : " + msgs[1]);
            //Console.WriteLine("msgs[2] : " + msgs[2]);
            switch (msgs[0]) {
                case "login":
                    makeData.GetloginInfo(clientSocket, msgCount,msgs[1]);
                    break;
                case "rootload": //target listview
                    makeData.GetDriveInfo(clientSocket, msgCount,"listView");
                    break;
                case "subitemload": //target listview
                    makeData.GetFilesDirs(clientSocket, msgCount, "listView",msgs[1]);
                    break;
                case "subdriveload": //target treeview
                    makeData.GetDriveInfo(clientSocket, msgCount, "treeView");
                    break;
                case "subdirload": //target treeview
                    makeData.GetDirs(clientSocket, msgCount, "treeView", msgs[1]);
                    break;
                case "MoveItemToDir":
                    makeData.GetDirs(clientSocket, msgCount, "all", msgs[1]);
                    break;
            }
        }

        public bool IdentifySendAll(string msg)
        {
            string[] msgs = msg.Split('|');

            bool sendAll = false;
            switch (msgs[0])
            {
                case "MoveItemToDir":
                    sendAll = true;
                    break;
            }
            return sendAll;
        }
    }
}

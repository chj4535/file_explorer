using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace file_explorer
{
    class SendServerEventHandler
    {
        static ClientSocketHandler clientSocket = new ClientSocketHandler();
        public SendServerEventHandler()
        {
        }
        public void MoveDir(string path,string sendType) // 폴더 이동 부분 
        {
            if (path.Equals("root"))
                clientSocket.OnSendData("rootload" + "|", null);
            else
            {
                clientSocket.OnSendData("subitemload" + "|" + path, null);
            }
        }
        public void LoadSubDir(string path, string sendType) // 폴더 이동 부분 
        {
            if (path.Equals("root"))
                clientSocket.OnSendData("subdriveload" + "|", null);
            else
            {
                clientSocket.OnSendData("subdirload" + "|" + path, null);
            }
        }
        public void reload(string path)
        {
            if (path.Equals("root"))
            {
                clientSocket.OnSendData("rootload" + "|", null);
            }
            else
            {
                clientSocket.OnSendData("subitemload" + "|" + path, null);
            }
        }
        public void MoveItemsToDir(string targetPath,string dragStaticpath,string dragItems,string sendType)
        {
            string sendData = "MoveItemToDir" + "|";
            sendData += targetPath + "/";
            sendData += dragStaticpath + "/";
            sendData += dragItems + "/";
            clientSocket.OnSendData(sendData, null);
        }
        public void RenameFileDir(string type,string staticPath, string preName,string nowName)
        {
            string sendData = "RenameFileDir" + "|";
            sendData += type + "/";
            sendData += staticPath + "/";
            sendData += preName + "/";
            sendData += nowName + "/";
            clientSocket.OnSendData(sendData, null);
        }
        public void DeleteFileDir(string itemName)
        {
            clientSocket.OnSendData("DeleteFileDir" + '|' + itemName,null);
        }
    }
}

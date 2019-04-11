using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.IO;

namespace socket_server
{
    public delegate void SendDataEventHandler(bool isSendall, Socket clientSocket,int msgCount,int Count,string type, string state, string info);// 전체인가?/보낸사람/보낼내용
    class MakeData
    {
        static event SendDataEventHandler sendDataevent;
        public void SetSendDataEventHandler(SendDataEventHandler addEvent)
        {
            sendDataevent += new SendDataEventHandler(addEvent);
        }

        string[][] userChart = new string[][] { new string[] { "admin", "1234" }, new string[] { "admin2", "1q2w3e4r" }, new string[] { "choi", "112233" } };
        public void GetloginInfo(Socket clientSocket, int msgCount,string msg)
        {
            string[] msgs = msg.Split('/');
            if (CompareInfo(msgs[0], msgs[1]))
            {
                sendDataevent(false, clientSocket,msgCount,1, "login", "success", null);
                return;
                //return Encoding.UTF8.GetBytes("success" + '|');
            }
            sendDataevent(false, clientSocket, msgCount,1, "login", "fail", null);
            //return Encoding.UTF8.GetBytes("fail" + '|');
        }

        private bool CompareInfo(string id, string pw)
        {
            foreach (string[] user in userChart)
            {
                if (id.Equals(user[0]) && pw.Equals(user[1]))
                {
                    return true;
                }
            }
            return false;
        }

        public void GetDriveInfo(Socket clientSocket,int msgCount)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            string driveInfo = "";
            int driveCount = allDrives.Length;
            //sendDataevent(false, clientSocket, "drive", "check", "root" + '/' + allDrives.Length.ToString() + '/');
            foreach (DriveInfo d in allDrives)
            {
                driveInfo = "";//초기화
                //driveInfo = driveCount.ToString() + "/";
                driveInfo = "root" + "/";
                driveInfo += d.Name + "/"; //드라이브 이름
                driveInfo += d.VolumeLabel + "/"; //드라이브 라벨
                driveInfo += d.DriveType + "/"; //드라이브 타입
                driveInfo += d.TotalSize.ToString() + "/";//드라이브 전체 크기
                driveInfo += d.TotalFreeSpace.ToString() + "/";//드라이브 여분 크기
                sendDataevent(false, clientSocket, msgCount, driveCount, "drive", "exist", driveInfo);
            }
        }

        public void GetFilesDirs(Socket clientSocket,int msgCount, string targetDirectory)
        {
            string[] result;
            string[] files = Directory.GetFiles(targetDirectory);
            string[] dirs = Directory.GetDirectories(targetDirectory);
            int itemCount = files.Length + dirs.Length;
            result = GetFeilsInfo(clientSocket, msgCount, itemCount, targetDirectory, files);
            //sendDataevent(false, clientSocket, "file", "check", targetDirectory+'/'+ files.Length.ToString()+'/'+ files.Length.ToString()+'/');
            if (result[0].Equals("fail"))
            {
                sendDataevent(false, clientSocket, msgCount,1,"file", "err", result[1]);
            }
            else
            {
                GetDirsInfo(clientSocket, msgCount,itemCount, targetDirectory,dirs);
            }
        }

        public static string[] GetFeilsInfo(Socket clientSocket,int msgCount,int itemCount, string targetDirectory,string[] files)//에러처리해야됨
        {
            string fileInfo = "";
            try
            {
                //string[] files = Directory.GetFiles(targetDirectory);
                foreach (string filePath in files)
                {
                    FileInfo file = new FileInfo(filePath);
                    //fileInfo = itemCount.ToString() + "/";
                    fileInfo = targetDirectory + "/";
                    fileInfo += file.Name + "/";
                    fileInfo += file.Extension + "/";
                    fileInfo += file.LastWriteTime.ToString() + "/";
                    fileInfo += file.Length.ToString() + "/";
                    sendDataevent(false, clientSocket, msgCount, itemCount, "file", "exist", fileInfo);
                }

                return new string[] { "success" };
            }
            catch (Exception e)
            {
                return new string[] { "fail", e.Message };
            }
        }

        public static string[] GetDirsInfo(Socket clientSocket, int msgCount, int itemCount, string targetDirectory,string[] dirs)
        {
            //string[] dirs = Directory.GetDirectories(targetDirectory);
            string dirInfo = "";
            try
            {
                foreach (string dirPath in dirs)
                {
                    DirectoryInfo dir = new DirectoryInfo(dirPath);
                    //dirInfo = itemCount.ToString() + "/";
                    dirInfo = targetDirectory + "/";
                    dirInfo += dir.Name + "/";
                    dirInfo += dir.Attributes + "/";
                    dirInfo += dir.LastWriteTime.ToString() + "/";
                    sendDataevent(false, clientSocket, msgCount, itemCount, "dir", "exist", dirInfo);
                }
                return new string[] { "success" };
            }
            catch (Exception e)
            {
                return new string[] { "fail",e.Message};
            }
        }
    }
}

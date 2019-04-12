using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.IO;

namespace socket_server
{
    public delegate void SendDataEventHandler(bool isSendall, Socket clientSocket, int msgCount, int itemCount, string target, string type, string state, string info);// 전체인가?/보낸사람/보낼내용
    class MakeData
    {
        static event SendDataEventHandler sendDataevent;
        public void SetSendDataEventHandler(SendDataEventHandler addEvent)
        {
            sendDataevent += new SendDataEventHandler(addEvent);
        }

        string[][] userChart = new string[][] { new string[] { "admin", "1234" }, new string[] { "admin2", "1q2w3e4r" }, new string[] { "choi", "112233" } };
        public void GetloginInfo(Socket clientSocket, int msgCount, string msg)
        {
            string[] msgs = msg.Split('/');
            if (CompareInfo(msgs[0], msgs[1]))
            {
                sendDataevent(false, clientSocket, msgCount, 1, "loginForm", "login", "success", null);
                return;
                //return Encoding.UTF8.GetBytes("success" + '|');
            }
            sendDataevent(false, clientSocket, msgCount, 1, "loginForm", "login", "fail", null);
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

        public void GetDriveInfo(Socket clientSocket, int msgCount, string target)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            string driveInfo = "";
            int driveCount = allDrives.Length;
            //sendDataevent(false, clientSocket, "drive", "check", "root" + '/' + allDrives.Length.ToString() + '/');
            foreach (DriveInfo d in allDrives)
            {
                driveInfo = "";//초기화
                driveInfo = "root" + "/";
                driveInfo += d.Name + "/"; //드라이브 이름
                driveInfo += d.VolumeLabel + "/"; //드라이브 라벨
                if (target.Equals("listView"))
                {
                    driveInfo += d.DriveType + "/"; //드라이브 타입
                    driveInfo += d.TotalSize.ToString() + "/";//드라이브 전체 크기
                    driveInfo += d.TotalFreeSpace.ToString() + "/";//드라이브 여분 크기
                    sendDataevent(false, clientSocket, msgCount, driveCount, target, "drive", "exist", driveInfo);
                }
                else//트리뷰
                {
                    string[] subdirs = Directory.GetDirectories(d.Name);

                    if (subdirs.Length > 0) //하위 폴더가 존재함 => 펼치기 가능해야됨
                    {
                        driveInfo += "have" + "/";
                        sendDataevent(false, clientSocket, msgCount, driveCount, target, "drive", "exist", driveInfo);
                    }
                    else
                    {
                        driveInfo += "none" + "/";
                        sendDataevent(false, clientSocket, msgCount, driveCount, target, "drive", "exist", driveInfo);
                    }
                }

            }
        }

        public void GetFilesDirs(Socket clientSocket, int msgCount, string target, string targetDirectory)
        {
            string[] result;
            string[] files = Directory.GetFiles(targetDirectory);
            string[] dirs = Directory.GetDirectories(targetDirectory);
            int itemCount = files.Length + dirs.Length;
            GetFeilsInfo(clientSocket, msgCount, itemCount, target, targetDirectory, files);
            GetDirsInfo(clientSocket, msgCount, itemCount, target, targetDirectory, dirs);
            //sendDataevent(false, clientSocket, "file", "check", targetDirectory+'/'+ files.Length.ToString()+'/'+ files.Length.ToString()+'/');
            /*
            if (result[0].Equals("fail"))
            {
                sendDataevent(false, clientSocket, msgCount, 1, target, "file", "err", result[1]);
            }
            else
            {
                GetDirsInfo(clientSocket, msgCount, itemCount, target, targetDirectory, dirs);
            }*/
        }
        public void GetDirs(Socket clientSocket, int msgCount, string target, string targetDirectory)
        {
            string[] result;
            string[] dirs = Directory.GetDirectories(targetDirectory);
            int itemCount = dirs.Length;
            GetDirsInfo(clientSocket, msgCount, itemCount, target, targetDirectory, dirs);
            /*//sendDataevent(false, clientSocket, "file", "check", targetDirectory+'/'+ files.Length.ToString()+'/'+ files.Length.ToString()+'/');
            if (result[0].Equals("fail"))
            {
                sendDataevent(false, clientSocket, msgCount, 1, target, "file", "err", result[1]);
            }*/
        }
        public static void GetFeilsInfo(Socket clientSocket, int msgCount, int itemCount, string target, string targetDirectory, string[] files)//에러처리해야됨
        {
            string fileInfo = "";
            //string[] files = Directory.GetFiles(targetDirectory);
            foreach (string filePath in files)
            {
                FileInfo file = new FileInfo(filePath);
                //fileInfo = itemCount.ToString() + "/";
                fileInfo = targetDirectory + "/";
                fileInfo += file.Name + "/";
                fileInfo += file.Extension + "/";
                if (file.Name.Contains("bootmgr") || file.Name.Contains("BOOTNXT") || file.Extension.Contains("sys") || file.Extension.Contains("MARKER") || file.Extension.Contains("BAK"))
                {
                    sendDataevent(false, clientSocket, msgCount, itemCount, target, "file", "disable", fileInfo);
                }
                else
                {
                    fileInfo += file.LastWriteTime.ToString() + "/";
                    fileInfo += file.Length.ToString() + "/";
                    sendDataevent(false, clientSocket, msgCount, itemCount, target, "file", "exist", fileInfo);
                }
            }
        }

        public static void GetDirsInfo(Socket clientSocket, int msgCount, int itemCount, string target, string targetDirectory, string[] dirs)
        {
            //string[] dirs = Directory.GetDirectories(targetDirectory);
            string dirInfo = "";
            foreach (string dirPath in dirs)
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                //dirInfo = itemCount.ToString() + "/";
                dirInfo = targetDirectory + "/";
                dirInfo += dir.Name + "/";
                string attribute = "";
                attribute += dir.Attributes;
                if (attribute.Contains("Hidden") || attribute.Contains("System"))
                {
                    sendDataevent(false, clientSocket, msgCount, itemCount, target, "dir", "disable", dirInfo);
                }
                else {
                    if (target.Equals("listView")) //리스트뷰용
                    {
                        dirInfo += dir.Attributes + "/";
                        dirInfo += dir.LastWriteTime.ToString() + "/";
                        sendDataevent(false, clientSocket, msgCount, itemCount, target, "dir", "exist", dirInfo);
                    }
                    else //트리뷰용
                    {
                        try {
                            string[] subdirs = Directory.GetDirectories(targetDirectory + dir.Name);//접근 가능한지
                            if (subdirs.Length > 0) //하위 폴더가 존재함 => 펼치기 가능해야됨
                            {
                                dirInfo += "have" + "/";
                                sendDataevent(false, clientSocket, msgCount, itemCount, target, "dir", "exist", dirInfo);
                            }
                            else
                            {
                                dirInfo += "none" + "/";
                                sendDataevent(false, clientSocket, msgCount, itemCount, target, "dir", "exist", dirInfo);
                            }
                        }
                        catch
                        {
                            sendDataevent(false, clientSocket, msgCount, itemCount, target, "dir", "disable", dirInfo);
                        }//접근불가능한 폴더
                    }
                }
            }
        }

        public byte[] MvoeItems(string targetPath, string dragStaticpath, string itemsLength, string item)
        {
            string[] items = item.Split('/');
            int count = 0;
            int successCount = 0;
            int failCount = 0;
            string succesResult = "";
            string failResult = "";
            try
            {
                for (int i = 0; i < Int32.Parse(itemsLength); i++)
                {
                    string itemType = items[count++];
                    string itemName = items[count++];
                    string movetargetPath = targetPath + '\\' + itemName;
                    string moveSourcePath = dragStaticpath + '\\' + itemName;
                    if (itemType.Equals("file"))
                    {
                        if (!Movefile(moveSourcePath, movetargetPath))
                        {
                            failCount++;
                            failResult += itemType + "/" + itemName + "/";
                        }
                        else
                        {
                            successCount++;
                            succesResult += itemType + "/" + itemName + "/";
                        }
                    }
                    else
                    {
                        if (!Movedir(moveSourcePath, movetargetPath))
                        {
                            failCount++;
                            failResult += itemType + "/" + itemName + "/";
                        }
                        else
                        {
                            successCount++;
                            succesResult += itemType + "/" + itemName + "/";
                        }
                    }
                }
                return Encoding.UTF8.GetBytes(successCount + "|" + succesResult + "|" + failCount + "|" + failResult + "|");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Encoding.UTF8.GetBytes("error|" + e.Message + "|");
            }
        }
        public bool Movedir(string sourceDir, string destinationDir)
        {
            try
            {
                Directory.Move(sourceDir, destinationDir);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Movefile(string sourceFile, string destinationFile)
        {
            try
            {
                File.Move(sourceFile, destinationFile);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
    }
}

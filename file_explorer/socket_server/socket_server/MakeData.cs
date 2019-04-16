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
    class MakeData : SocketList
    {
        string[][] userChart = new string[][] { new string[] { "admin", "1234" }, new string[] { "admin2", "1q2w3e4r" }, new string[] { "choi", "112233" } };
        public void GetloginInfo(Socket clientSocket, int msgCount, string msg)
        {
            string[] msgs = msg.Split('/');
            if (CompareInfo(msgs[0], msgs[1]))
            {
                OnSendData(false, clientSocket, msgCount, 1, "loginForm", "login", "success", null);
                return;
                //return Encoding.UTF8.GetBytes("success" + '|');
            }
            OnSendData(false, clientSocket, msgCount, 1, "loginForm", "login", "fail", null);
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
                    OnSendData(false, clientSocket, msgCount, driveCount, target, "drive", "exist", driveInfo);
                }
                else//트리뷰
                {
                    string[] subdirs = Directory.GetDirectories(d.Name);

                    if (subdirs.Length > 0) //하위 폴더가 존재함 => 펼치기 가능해야됨
                    {
                        driveInfo += "have" + "/";
                        OnSendData(false, clientSocket, msgCount, driveCount, target, "drive", "exist", driveInfo);
                    }
                    else
                    {
                        driveInfo += "none" + "/";
                        OnSendData(false, clientSocket, msgCount, driveCount, target, "drive", "exist", driveInfo);
                    }
                }

            }
        }

        public void GetFilesDirs(Socket clientSocket, int msgCount, string target, string targetDirectory)
        {
            string[] result;
            try
            {
                string[] files = Directory.GetFiles(targetDirectory);
                string[] dirs = Directory.GetDirectories(targetDirectory);
                int itemCount = files.Length + dirs.Length;
                if (itemCount == 0)//빈폴더 요청
                {
                    OnSendData(false, clientSocket, msgCount, 1, target, "file", "disable", targetDirectory + "/");
                }
                else
                {
                    GetFeilsInfo(clientSocket, msgCount, itemCount, target, targetDirectory, files);
                    GetDirsInfo(clientSocket, msgCount, itemCount, target, targetDirectory, dirs);
                }
            }
            catch (Exception e)
            {
                OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                Console.WriteLine(e.Message);
            }
        }
        public void GetDirs(Socket clientSocket, int msgCount, string target, string targetDirectory)
        {
            string[] result;
            string[] dirs = Directory.GetDirectories(targetDirectory);
            int itemCount = dirs.Length;
            GetDirsInfo(clientSocket, msgCount, itemCount, target, targetDirectory, dirs);
        }
        public void GetFeilsInfo(Socket clientSocket, int msgCount, int itemCount, string target, string targetDirectory, string[] files)//에러처리해야됨
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
                if (file.Name.Contains("bootmgr") || file.Name.Contains("BOOTNXT") || file.Extension.Contains("sys") || file.Extension.Contains("dat") || file.Extension.Contains("MARKER") || file.Extension.Contains("BAK"))
                {
                    OnSendData(false, clientSocket, msgCount, itemCount, target, "file", "disable", fileInfo);
                }
                else
                {
                    fileInfo += file.LastWriteTime.ToString() + "/";
                    fileInfo += file.Length.ToString() + "/";
                    OnSendData(false, clientSocket, msgCount, itemCount, target, "file", "exist", fileInfo);
                }
            }
        }

        public void GetDirsInfo(Socket clientSocket, int msgCount, int itemCount, string target, string targetDirectory, string[] dirs)
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
                    OnSendData(false, clientSocket, msgCount, itemCount, target, "dir", "disable", dirInfo);
                }
                else
                {
                    if (target.Equals("listView")) //리스트뷰용
                    {
                        dirInfo += dir.Attributes + "/";
                        dirInfo += dir.LastWriteTime.ToString() + "/";
                        OnSendData(false, clientSocket, msgCount, itemCount, target, "dir", "exist", dirInfo);
                    }
                    else //트리뷰용
                    {
                        OnSendData(false, clientSocket, msgCount, itemCount, target, "dir", "exist", dirInfo);
                        /*
                        try
                        {
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
                            sendDataevent(false, clientSocket, msgCount, itemCount, target, "dir", "exist", dirInfo);
                        }
                        catch
                        {
                            sendDataevent(false, clientSocket, msgCount, itemCount, target, "dir", "disable", dirInfo);
                        }//접근불가능한 폴더
                        */
                    }
                }
            }
        }

        public void MvoeItems(Socket clientSocket, int msgCount, string target, string msg)
        {
            /*
            string[] items = item.Split('/');
            int count = 0;
            int successCount = 0;
            int failCount = 0;
            string succesResult = "";
            string failResult = "";
            */
            string[] msgs = msg.Split('/');
            string targetPath = msgs[0];
            string dragStaticpath = msgs[1];
            string itemType = msgs[2];
            string itemName = msgs[3];

            if (itemType.Equals("file"))
            {
                Movefile(clientSocket, msgCount, target, targetPath, dragStaticpath, itemName);
            }
            else
            {
                Movedir(clientSocket, msgCount, target, targetPath, dragStaticpath, itemName);
            }
        }

        public void CopyFileDir(Socket clientSocket, int msgCount, string target, string msg)
        {
            string[] msgs = msg.Split('/');
            string itemType = msgs[0];
            string sourcePath = msgs[1];
            string targetPath = msgs[2];
            string itemName = msgs[3];
            if (itemType.Equals("file"))
            {
                CopyFile(clientSocket, msgCount, target, sourcePath, targetPath, itemName);
            }
            else
            {
                CopyDir(clientSocket, msgCount, target, sourcePath, targetPath, itemName);
            }
        }

        public void CopyDir(Socket clientSocket, int msgCount, string target, string sourcePath, string targetPath, string itemName)
        {
            try
            {
                string sourceFileName = sourcePath + '\\' + itemName;
                string destFileName = targetPath + itemName;
                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(sourceFileName, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourceFileName, destFileName));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(sourceFileName, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(sourceFileName, destFileName), true);
                string dirInfo = "";
                DirectoryInfo dir = new DirectoryInfo(destFileName);
                //dirInfo = itemCount.ToString() + "/";
                dirInfo = targetPath + "/";
                dirInfo += dir.Name + "/";
                string attribute = "";
                attribute += dir.Attributes;
                dirInfo += dir.Attributes + "/";
                dirInfo += dir.LastWriteTime.ToString() + "/";
                OnSendData(true, clientSocket, msgCount + 20000, 1, target, "dir", "add", dirInfo);
            }
            catch (Exception e)
            {
                OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                Console.WriteLine(e.Message);
            }

        }

        public void CopyFile(Socket clientSocket, int msgCount, string target, string sourcePath, string targetPath,string itemName)
        {
            string sourceFileName = sourcePath + '\\' + itemName;
            string destFileName = targetPath + itemName;
            try
            {
                File.Copy(sourceFileName, destFileName, true);
                string fileInfo = "";
                //targetPath 데이터 추가
                FileInfo file = new FileInfo(destFileName);
                fileInfo = targetPath  + "/";
                fileInfo += file.Name + "/";
                fileInfo += file.Extension + "/";

                fileInfo += file.LastWriteTime.ToString() + "/";
                fileInfo += file.Length.ToString() + "/";
                OnSendData(true, clientSocket, msgCount + 20000, 1, target, "file", "add", fileInfo);
            }
            catch (Exception e)
            {
                OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteFileDir(Socket clientSocket, int msgCount, string target, string msg)
        {
            string[] msgs = msg.Split('/');
            string itemType = msgs[0];
            string itemFullpath = msgs[1];
            string itemPath = itemFullpath.Substring(0, itemFullpath.LastIndexOf('\\'));
            string itemName = itemFullpath.Split('\\').Last();
            if (itemType.Equals("file"))
            {
                try
                {
                    File.Delete(itemFullpath);
                    OnSendData(true, clientSocket, msgCount+10000, 1, target, "file", "delete", itemPath + '\\' + '/' + itemName + '/');
                }
                catch (Exception e)
                {
                    OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Directory.Delete(itemFullpath,true);
                    OnSendData(true, clientSocket, msgCount + 10000, 1, target, "dir", "delete", itemPath + '\\' + '/' + itemName + '/');
                }
                catch (Exception e)
                {
                    OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                    Console.WriteLine(e.Message);
                }
            }
            
            //sendDataevent(true, clientSocket, msgCount, 1, target, "dir", "delete", dragStaticpath + '\\' + '/' + itemName + '/');
        }

        public void Movedir(Socket clientSocket, int msgCount, string target, string targetPath, string dragStaticpath, string itemName)
        {
            string destinationDir = targetPath + '\\' + itemName; 
            string sourceDir = dragStaticpath + '\\' + itemName;
            try
            {
                Directory.Move(sourceDir, destinationDir);

                OnSendData(true, clientSocket, msgCount+10000, 1, target, "dir", "delete", dragStaticpath + '\\' + '/' + itemName + '/');
                string dirInfo = "";
                DirectoryInfo dir = new DirectoryInfo(sourceDir);
                //dirInfo = itemCount.ToString() + "/";
                dirInfo = targetPath + '\\' + "/";
                dirInfo += dir.Name + "/";
                string attribute = "";
                attribute += dir.Attributes;
                dirInfo += dir.Attributes + "/";
                dirInfo += dir.LastWriteTime.ToString() + "/";
                OnSendData(true, clientSocket, msgCount+20000, 1, target, "dir", "add", dirInfo);

            }
            catch (Exception e)
            {
                OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                Console.WriteLine(e.Message);
            }
        }

        public void Movefile(Socket clientSocket, int msgCount, string target, string targetPath, string dragStaticpath, string itemName)
        {
            string destinationFile = targetPath + '\\' + itemName;
            string sourceFile = dragStaticpath + '\\' + itemName;
            try
            {
                File.Move(sourceFile, destinationFile);

                //dragStaticpath 데이터 삭제
                OnSendData(true, clientSocket, msgCount+ 10000, 1, target, "file", "delete", dragStaticpath + '\\' + '/' + itemName + '/');
                string fileInfo = "";
                //targetPath 데이터 추가
                FileInfo file = new FileInfo(destinationFile);
                fileInfo = targetPath + '\\' + "/";
                fileInfo += file.Name + "/";
                fileInfo += file.Extension + "/";

                fileInfo += file.LastWriteTime.ToString() + "/";
                fileInfo += file.Length.ToString() + "/";
                OnSendData(true, clientSocket, msgCount+20000, 1, target, "file", "add", fileInfo);
            }
            catch (Exception e)
            {
                OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                Console.WriteLine(e.Message);
            }
        }

        public void RenameFileDir(Socket clientSocket, int msgCount, string target, string msg)
        {
            /*
             * sendData += type + "/";
            sendData += staticPath + "/";
            sendData += preName + "/";
            sendData += nowName + "/";
             */
            string[] msgs = msg.Split('/');
            string type = msgs[0];
            string path = msgs[1]; // 마지막 \\ 포함
            string preName = msgs[2];
            string nowName = msgs[3];

            if (type.Equals("file"))
            {
                RenameFile(clientSocket, msgCount, target, path, preName, nowName);
            }
            else
            {
                RenameDir(clientSocket, msgCount, target, path, preName, nowName);
            }
        }

        public void RenameDir(Socket clientSocket, int msgCount, string target, string staticPath, string preName, string nowName)
        {
            string destinationDir = staticPath + nowName;
            string sourceDir = staticPath + preName;
            try
            {
                Directory.Move(sourceDir, destinationDir);

                OnSendData(true, clientSocket, msgCount+ 10000, 1, target, "dir", "delete", staticPath + '/' + preName + '/');
                string dirInfo = "";
                DirectoryInfo dir = new DirectoryInfo(destinationDir);
                //dirInfo = itemCount.ToString() + "/";
                dirInfo = staticPath + "/";
                dirInfo += dir.Name + "/";
                string attribute = "";
                attribute += dir.Attributes;
                dirInfo += dir.Attributes + "/";
                dirInfo += dir.LastWriteTime.ToString() + "/";
                OnSendData(true, clientSocket, msgCount, 1, target, "dir", "add", dirInfo);

            }
            catch (Exception e)
            {
                OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                Console.WriteLine(e.Message);
            }
        }

        public void RenameFile(Socket clientSocket, int msgCount, string target, string staticPath, string preName, string nowName)
        {
            string destinationFile = staticPath + nowName;
            string sourceFile = staticPath + preName;
            try
            {
                File.Move(sourceFile, destinationFile);

                //dragStaticpath 데이터 삭제
                OnSendData(true, clientSocket, msgCount + 10000, 1, target, "file", "delete", staticPath + '/' + preName + '/');
                string fileInfo = "";
                //targetPath 데이터 추가
                FileInfo file = new FileInfo(destinationFile);
                fileInfo = staticPath + "/";
                fileInfo += file.Name + "/";
                fileInfo += file.Extension + "/";

                fileInfo += file.LastWriteTime.ToString() + "/";
                fileInfo += file.Length.ToString() + "/";
                OnSendData(true, clientSocket, msgCount, 1, target, "file", "add", fileInfo);
            }
            catch (Exception e)
            {
                OnSendData(false, clientSocket, msgCount, 1, "error", e.Message, "", "");
                Console.WriteLine(e.Message);
            }
        }

        public void OnSendData(bool isSendall, Socket clientSocket, int msgCount, int itemCount, string target, string type, string state, string info)// 전체인가?/보낸사람/보낼내용
        {
            AsyncObject ao = new AsyncObject();

            // 문자열을 바이트 배열으로 변환
            //string message = 
            ao.buffer = Encoding.UTF8.GetBytes('\x01' + msgCount.ToString() + '|' + itemCount.ToString() + '|' + target + '|' + type + '|' + state + '|' + info + '|' + '\x01');

            ao.workingSocket = clientSocket;


            for (int i = connectedClients.Count - 1; i >= 0; i--)
            {
                Socket socket = connectedClients[i].clientSocket;
                if (isSendall)
                {
                    try
                    {
                        socket.BeginSend(ao.buffer, 0, ao.buffer.Length, 0, new AsyncCallback(SendCallback), socket);
                    }
                    catch // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                    {

                        try { socket.Dispose(); } catch { }
                        connectedClients.RemoveAt(i);
                    }
                }
                else
                {
                    if (socket == ao.workingSocket)
                    {
                        try
                        {
                            socket.BeginSend(ao.buffer, 0, ao.buffer.Length, 0, new AsyncCallback(SendCallback), socket);
                        }
                        catch // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                        {

                            try { socket.Dispose(); } catch { }
                            connectedClients.RemoveAt(i);
                        }
                    }
                }
                
            }
            //AppendText(server_log_richtextbox, string.Format("연결된 클라이언트 갯수 : {0}", connectedClients.Count));
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  

                int bytesSent = client.EndSend(ar);
                //Console.WriteLine("{0} : Sent {1} bytes to client.", client.RemoteEndPoint.ToString(), bytesSent);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

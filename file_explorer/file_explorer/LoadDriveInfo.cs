using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace file_explorer
{
    public struct DriveInfo
    {
        public string driveName;
        public string driveLabel;
        public string driveType;
        public string driveTotalsize;
        public string driveFreesize;
    }
    public delegate void DriveInfoToListviewHandler(int msgCount, DriveInfo[] datas);
    public delegate void DriveInfoToTreeviewHandler(int msgCount, string staticPath, string[] dirPaths);
    class LoadDriveInfo
    {
        static event DriveInfoToListviewHandler driveInfotolistviewevent;
        static event DriveInfoToTreeviewHandler driveInfototreeviewevent;

        public void SetDriveInfotoListviewEvnet(DriveInfoToListviewHandler add_event)
        {
            driveInfotolistviewevent += new DriveInfoToListviewHandler(add_event);
        }

        public void SetDriveInfotoTreeviewEvent(DriveInfoToTreeviewHandler add_event)
        {
            driveInfototreeviewevent += new DriveInfoToTreeviewHandler(add_event);
        }

        public void LoadDriverInfoResult(int msgCount, string msg)
        {
            // 3/C:\/ Fixed / 64599998464 / 11458220032 / D:\/ Fixed / 62913507328 / 48704061440 / Z:\/ Fixed / 128033222656 / 59368087552 /
            string[] driveInfo = msg.Split('/');
            int count = 0;
            DriveInfo[] driveInfos=new DriveInfo[Int32.Parse(driveInfo[0])];
            string[] nodes = new string[Int32.Parse(driveInfo[0])];
            for (int driveNum = 0; driveNum < Int32.Parse(driveInfo[0]); driveNum++)
            {
                driveInfos[driveNum].driveName = driveInfo[++count];
                nodes[driveNum] = driveInfos[driveNum].driveName.Split('\\').First(); // 트리뷰에 보낼 내용
                driveInfos[driveNum].driveLabel = driveInfo[++count];
                driveInfos[driveNum].driveType = driveInfo[++count];
                driveInfos[driveNum].driveTotalsize = driveInfo[++count];
                driveInfos[driveNum].driveFreesize = driveInfo[++count];
                Console.WriteLine(driveInfos[driveNum].driveName);
                Console.WriteLine(driveInfos[driveNum].driveType);
                Console.WriteLine(driveInfos[driveNum].driveTotalsize);
                Console.WriteLine(driveInfos[driveNum].driveFreesize);
            }
            driveInfotolistviewevent(msgCount, driveInfos);
            driveInfototreeviewevent(msgCount, "", nodes);
        }
    }
}

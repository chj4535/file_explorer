using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_explorer
{
    public delegate void ChangeStateEventHandler(object[] data);
    //public delegate void ChangeStateEventHandler(string type,string state,object);
    class MakeStateData
    {
        LoginCheck loginCheck = new LoginCheck();
        static List<object[]> dataSet = new List<object[]>();
        static List<string> dataSetIndex = new List<string>();
        static event ChangeStateEventHandler changeStateevent;
        public void SetDriveInfoToCurrentStateEventHandler(ChangeStateEventHandler addEvent)
        {
            changeStateevent += new ChangeStateEventHandler(addEvent);
        }

        public void MakeDataSet(string msg)
        {

            string[] msgs = msg.Split('|');
            string msgCount = msgs[0];
            string itemCount = msgs[1];
            if (msgs[2].Equals("login"))
            {
                loginCheck.LoginResult(msgs[3]);
            }
            else
            {
                if (!dataSetIndex.Contains(msgCount)) //최초로 받은 메시지
                {
                    dataSetIndex.Add(msgCount);
                    dataSet.Add(new object[Int32.Parse(itemCount) + 2]); //0번방에 현재 갯수, 1번방에 경로저장되므로 2칸 더 많아야됨
                    dataSet[dataSet.Count - 1][0] = 0; //현재 개수
                    dataSet[dataSet.Count - 1][1] = Int32.Parse(itemCount);// 총 개수
                }
                int msgIndex = dataSetIndex.IndexOf(msgCount); //메시지 위치 찾음
                int dataCount = (int)(dataSet[msgIndex][0]);
                dataCount++;
                //msgs[2] : type
                //msgs[3] : state
                //msgs[4] : info
                dataSet[msgIndex][dataCount + 1] = msgs[2] + '|' + msgs[3] + '|' + msgs[4] + '|';
                dataSet[msgIndex][0] = dataCount;
                if ((int)dataSet[msgIndex][0] == (int)dataSet[msgIndex][1])//메시지 다 바음
                {
                    changeStateevent(dataSet[msgIndex]);
                    dataSet.RemoveAt(msgIndex);
                    dataSetIndex.RemoveAt(msgIndex);
                }
            }
        }
    }
}
/*
public void LoadDriverInfoResult(int msgCount, string msg)
{
    // 3/C:\/ Fixed / 64599998464 / 11458220032 / D:\/ Fixed / 62913507328 / 48704061440 / Z:\/ Fixed / 128033222656 / 59368087552 /
    string[] driveInfo = msg.Split('/');
    int count = 0;
    DriveInfo[] driveInfos = new DriveInfo[Int32.Parse(driveInfo[0])];
    string[] nodes = new string[Int32.Parse(driveInfo[0])];
    for (int driveNum = 0; driveNum < Int32.Parse(driveInfo[0]); driveNum++)
    {
        driveInfos[driveNum].driveName = driveInfo[++count];
        driveInfos[driveNum].driveLabel = driveInfo[++count];
        driveInfos[driveNum].driveType = driveInfo[++count];
        driveInfos[driveNum].driveTotalsize = driveInfo[++count];
        driveInfos[driveNum].driveFreesize = driveInfo[++count];
        nodes[driveNum] = driveInfos[driveNum].driveName;
        Console.WriteLine(driveInfos[driveNum].driveName);
        Console.WriteLine(driveInfos[driveNum].driveType);
        Console.WriteLine(driveInfos[driveNum].driveTotalsize);
        Console.WriteLine(driveInfos[driveNum].driveFreesize);
    }
    object[] data = new object[2];
    data[0] = nodes;
    data[1] = driveInfos;
    changeStateevent(msgCount, "rootload",data);
}

public void LoadDirSubItemsInfoResult(int msgCount, string staticPath, string filesInfo, string dirsInfo)
{
    if (filesInfo.Equals("error"))
    {
        object[] data = new object[1];
        data[0] = staticPath;
        changeStateevent(msgCount, "error", data); // -1이면 에러라 판단 현재 staticpath에는 에러 담아있음
    }
    else
    {
        //3dirload | 13 / Z:\2019 - 03 - 19_17_23_53.txt / 2019 - 03 - 19_17_23_53.txt /.txt / 2019 - 03 - 19 오후 5:23:56 / 370338 / Z:\2019 - 03 - 19_17_39_54.txt / 2019 - 03 - 19_17_39_54.txt /.txt / 2019 - 03 - 19 오후 5:45:45 / 1982366 / Z:\3DP_Chip_Lite_v1811.exe / 3DP_Chip_Lite_v1811.exe /.exe / 2019 - 01 - 03 오전 9:24:31 / 3025560 / Z:\
        string[] filesInfos = filesInfo.Split('/');
        string[] dirsInfos = dirsInfo.Split('/');
        int count = 0;
        SubItemInfo[] subIteminfos = new SubItemInfo[Int32.Parse(filesInfos[0]) + Int32.Parse(dirsInfos[0])];

        for (int fileNum = 0; fileNum < Int32.Parse(filesInfos[0]); fileNum++)
        {
            subIteminfos[fileNum].isFile = true;
            subIteminfos[fileNum].subItemname = filesInfos[++count];
            subIteminfos[fileNum].subItemtype = filesInfos[++count];
            subIteminfos[fileNum].subItemlastwritetime = filesInfos[++count];
            subIteminfos[fileNum].subItemlength = filesInfos[++count];

            Console.WriteLine(subIteminfos[fileNum].subItemname);
            Console.WriteLine(subIteminfos[fileNum].subItemtype);
            Console.WriteLine(subIteminfos[fileNum].subItemlastwritetime);
            Console.WriteLine(subIteminfos[fileNum].subItemlength);
        }
        count = 0;
        string[] nodes = new string[Int32.Parse(dirsInfos[0])];
        for (int dirNum = Int32.Parse(filesInfos[0]); dirNum < Int32.Parse(filesInfos[0]) + Int32.Parse(dirsInfos[0]); dirNum++)
        {
            subIteminfos[dirNum].isFile = false;
            subIteminfos[dirNum].subItemname = dirsInfos[++count];
            nodes[dirNum - Int32.Parse(filesInfos[0])] = subIteminfos[dirNum].subItemname;
            subIteminfos[dirNum].subItemtype = dirsInfos[++count];
            subIteminfos[dirNum].subItemlastwritetime = dirsInfos[++count];
            subIteminfos[dirNum].subItemlength = null;

            Console.WriteLine(subIteminfos[dirNum].subItemname);
            Console.WriteLine(subIteminfos[dirNum].subItemtype);
            Console.WriteLine(subIteminfos[dirNum].subItemlastwritetime);
            Console.WriteLine(subIteminfos[dirNum].subItemlength);
        }
        object[] data = new object[3];
        data[0] = staticPath;
        data[1] = nodes;
        data[2] = subIteminfos;
        changeStateevent(msgCount, "dirload", data);
    }
}

public void LoadMoveResult()
{

}*/

/*
public struct SubItemInfo
{
public bool isFile;
public string subItemname;
public string subItemtype;
public string subItemlastwritetime;
public string subItemlength;
}

public struct DriveInfo
{
public string driveName;
public string driveLabel;
public string driveType;
public string driveTotalsize;
public string driveFreesize;
}
*/
/*
public void SetDrive(string msgCount,string state,string info)
{
    string[] infos = info.Split('/');

        DriveInfo driveInfo = new DriveInfo();
        string path = infos[0];
        driveInfo.driveName = infos[1];
        driveInfo.driveLabel = infos[2];
        driveInfo.driveType = infos[3];
        driveInfo.driveTotalsize = infos[4];
        driveInfo.driveFreesize = infos[5];
        object[] data = new object[2];
        data[0] = path;
        data[1] = driveInfo;
        changeStateevent("drive", null, data);

}
public void SetFile(string msgCount,string state, string info)
{
    string[] infos = info.Split('/');
    if (!dataSetIndex.Contains(msgCount)) //최초로 받은 메시지
    {
        dataSetIndex.Add(msgCount);
        dataSet.Add(new object[Int32.Parse(infos[0]) + 3]); //0번방에 현재 갯수, 1번방에 경로저장되므로 2칸 더 많아야됨
        dataSet[dataSet.Count - 1][0] = 0; //현재 개수
        dataSet[dataSet.Count - 1][1] = infos[0];// 총 개수
        dataSet[dataSet.Count - 1][2] = infos[1];// 경로
    }
    int msgIndex=dataSetIndex.IndexOf(msgCount); //메시지 위치 찾음
    if(state.Equals("error"))
    {
        //
    }
    else
    {
        SubItemInfo subItem = new SubItemInfo();
        //string path = infos[1];
        subItem.isFile = true;
        subItem.subItemname = infos[2];
        subItem.subItemtype = infos[3];
        subItem.subItemlastwritetime = infos[4];
        subItem.subItemlength = infos[5];
        //object[] data = new object[2];
        //data[0] = path;
        //data[1] = subItem;
        int dataCount = (int)dataSet[msgIndex][0];
        dataCount++;
        dataSet[msgIndex][dataCount+2] = subItem;
    }
    if ((int)dataSet[msgIndex][0] == (int)dataSet[msgIndex][1])//메시지 다 바음
    {
        changeStateevent("file", state, dataSet[msgIndex]);
        dataSet.RemoveAt(msgIndex);
        dataSetIndex.RemoveAt(msgIndex);
    }
}
public void SetDir(string msgCount,string state,string info)
{
    string[] infos = info.Split('/');
    if (state.Equals("error"))
    {
        //
    }
    else
    {
        SubItemInfo subItem = new SubItemInfo();
        string path = infos[1];
        subItem.isFile = false;
        subItem.subItemname = infos[2];
        subItem.subItemtype = infos[3];
        subItem.subItemlastwritetime = infos[4];
        subItem.subItemlength = null;
        object[] data = new object[2];
        data[0] = path;
        data[1] = subItem;
        changeStateevent("dir", state, data);
    }
}
}
}
*/

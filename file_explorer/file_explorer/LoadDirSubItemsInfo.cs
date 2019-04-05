using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_explorer
{
    public struct SubItemInfo
    {
        public bool isFile;
        public string subItempath;
        public string subItemname;
        public string subItemtype;
        public string subItemlastwritetime;
        public string subItemlength;
    }
    public delegate void SubItemEventHandler(int msgCount, SubItemInfo[] data);
    class LoadDirSubItemsInfo
    {
        static event SubItemEventHandler SubItemEvent;
        public void SetSubItemEvnet(SubItemEventHandler add_event)
        {
            SubItemEvent += new SubItemEventHandler(add_event);
        }
        public void LoadDirSubItemsInfoResult(int msgCount, string filesInfo,string dirsInfo)
        {
            //3dirload|13/2019-03-19_17_23_53.txt/.txt/2019-03-19 오후 5:23:56/370338/2019-03-19_17_39_54.txt/.txt/2019-03-19 오후 5:45:45/1982366/3DP_Chip_Lite_v1811.exe/.exe/2019-01-03 오전 9:24:31/3025560/3DP_Net_v1812.exe/.exe/2019-01-03 오후 3:02:45/121367488/asm.txt/.txt/2019-01-16 오전 10:56:25/88/bootmgr//2019-03-06 오후 3:20:18/407706/BOOTNXT//2017-03-19 오전 5:57:38/1/BOOTSECT.BAK/.BAK/2019-03-14 오후 2:01:01/8192/pagefile.sys/.sys/2019-04-03 오전 9:26:00/9126805504/TestTool - 바로 가기.lnk/.lnk/2019-04-02 오후 3:47:38/742/TestTool.exe/.exe/2019-03-07 오후 1:14:52/310718464/TestToolUpdate - 복사본.exe/.exe/2019-03-06 오후 3:09:57/3295744/TestToolUpdate.exe/.exe/2019-03-06 오후 3:09:57/3295744/|12/$RECYCLE.BIN/Hidden, System, Directory/2019-03-04 오전 9:45:55/ASMServer/Directory/2019-03-15 오전 9:15:45/backup/Directory/2019-02-19 오전 9:38:26/Boot/Hidden, System, Directory/2019-03-18 오후 2:27:12/driver/Directory/2019-02-21 오후 2:10:59/Program Files/Directory/2019-04-02 오전 10:16:07/Recovery/Hidden, System, Directory, NotContentIndexed/2019-03-14 오후 2:30:13/study/Directory/2019-04-02 오후 5:07:49/System Volume Information/Hidden, System, Directory/2019-02-01 오전 6:11:10/Tool/Directory/2019-03-28 오후 2:53:27/새 폴더/Directory/2019-04-04 오후 1:49:41/유틸/Directory/2019-03-18 오후 5:54:51/|
            string[] filesInfos = filesInfo.Split('/');
            string[] dirsInfos = dirsInfo.Split('/');
            int count = 0;
            SubItemInfo[] subIteminfos = new SubItemInfo[Int32.Parse(filesInfos[0])+ Int32.Parse(dirsInfos[0])];

            for (int fileNum = 0; fileNum < Int32.Parse(filesInfos[0]); fileNum++)
            {
                subIteminfos[fileNum].isFile = true;
                subIteminfos[fileNum].subItempath = filesInfos[++count];
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
            for (int dirNum = Int32.Parse(filesInfos[0]); dirNum < Int32.Parse(filesInfos[0])+Int32.Parse(dirsInfos[0]); dirNum++)
            {
                subIteminfos[dirNum].isFile = false;
                subIteminfos[dirNum].subItempath = dirsInfos[++count];
                subIteminfos[dirNum].subItemname = dirsInfos[++count];
                subIteminfos[dirNum].subItemtype = dirsInfos[++count];
                subIteminfos[dirNum].subItemlastwritetime = dirsInfos[++count];
                subIteminfos[dirNum].subItemlength = null;

                Console.WriteLine(subIteminfos[dirNum].subItemname);
                Console.WriteLine(subIteminfos[dirNum].subItemtype);
                Console.WriteLine(subIteminfos[dirNum].subItemlastwritetime);
                Console.WriteLine(subIteminfos[dirNum].subItemlength);
            }
            SubItemEvent(msgCount, subIteminfos);
        }
    }
}

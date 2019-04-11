using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_explorer
{
    public delegate void SubItemToCurrentStateEventHandler(int msgCount, string staticPath,string[] currentStateitemsname,SubItemInfo[] data);
    //public delegate void SubItemToListviewEventHandler(int msgCount, SubItemInfo[] data);
    //public delegate void SubItemToTreeviewEventHandler(int msgCount, string staticPath, string[] dirPaths);
    class LoadDirSubItemsInfo
    {
        static event SubItemToCurrentStateEventHandler subitemTocurrentstateevent;

        public void SetSubItemToCurrentStateEvent(SubItemToCurrentStateEventHandler addEvent)
        {
            subitemTocurrentstateevent += new SubItemToCurrentStateEventHandler(addEvent);
        }
        public void LoadDirSubItemsInfoResult(int msgCount, string staticPath, string filesInfo,string dirsInfo)
        {
            if (filesInfo.Equals("error"))
            {
                subitemTocurrentstateevent(-1, staticPath, null,null); // -1이면 에러라 판단 현재 staticpath에는 에러 담아있음
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
                   // subIteminfos[fileNum].isFile = true;
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
                    //subIteminfos[dirNum].isFile = false;
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
                subitemTocurrentstateevent(msgCount, staticPath, nodes,subIteminfos);
                /*
                subItemtolistviewevent(msgCount, subIteminfos);
                if (Int32.Parse(dirsInfos[0]) > 0)
                {
                    string staticPath = dirsInfos[1].Substring(0, dirsInfos[1].LastIndexOf("\\"));
                    subItemtotreeviewevent(msgCount, staticPath, nodes);
                }
                */
            }
        }
    }
}

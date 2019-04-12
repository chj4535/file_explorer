using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace file_explorer
{
    public delegate void UpdateEventHandler(string target);
    class CurrentState
    {
        [DllImport("shell32.dll", EntryPoint = "ExtractIcon")]
        extern static IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);
        const string ShellIconsLib = @"C:\WINDOWS\System32\imageres.dll";
        static public Icon GetIcon(int index)
        {
            IntPtr Hicon = ExtractIcon(
               IntPtr.Zero, ShellIconsLib, index);
            Icon icon = Icon.FromHandle(Hicon);
            return icon;
        }

        static event UpdateEventHandler updateEvent;

        public void SetUpdateEvent(UpdateEventHandler addEvent)
        {
            updateEvent += new UpdateEventHandler(addEvent);
        }
        //public static List<string> currentIconIndex = new List<string>();
        //public static List<Icon> currentIcon = new List<Icon>();
        public static Stack<string> prePathsave = new Stack<string>();
        public static Stack<string> nextPathsave = new Stack<string>();
        public static List<string[]> driveInfo = new List<string[]>();
        //static LoadDriveInfo loadDriveinfo = new LoadDriveInfo();
        //static LoadDirSubItemsInfo loadDirsubiteminfo = new LoadDirSubItemsInfo();
        static MakeData makeStatedata = new MakeData();
        public static string currentStaticpath { get; set; }//현재 경로 저장
        //public static string currentType { get; set; }//현재 받은 정보의 타입
        //public static string currentTypestate { get; set; }//현재 받은 타입의 상태 (exist/error/delete)
        public static object[] currentData { get; set; }//현재 받은 타입의 데이타
        public static bool isClick { get; set; }//버튼이 아닌 클릭이벤트 확인
        public static SendServerEventHandler sendServerEventHandler = new SendServerEventHandler();
        static CurrentState()
        {
            currentStaticpath = "";
            //currentMsgcount = 0;
            makeStatedata.SetDriveInfoToCurrentStateEventHandler(SetStateData);//상태 변경 데이터 받아옴
            //loadDriveinfo.SetDriveInfoToCurrentStateEventHandler(SetDriveInfo);
            //loadDirsubiteminfo.SetSubItemToCurrentStateEvent(SetSubItemInfo);
            
        }
        static void SetStateData(object[] data)
        {
            currentData = data;
            string target = (string)data[2];//타겟확인
            updateEvent(target);
        }

        public void SetFirstLoad()
        {
            currentStaticpath = "root";
            //sendServerEventHandler.LoadSubDir("root", "firstload");
            //MoveDir(false,"root", "form_load");
        }

        public void MoveDir(bool click,string path,string context)
        {
            isClick = click;
            currentStaticpath = path;
            sendServerEventHandler.MoveDir(path, context);
        }
    }
}
        /*
        public static bool isClick { get; set; }//버튼이 아닌 클릭이벤트 확인
        public SendServerEventHandler sendServerEventHandler = new SendServerEventHandler();
        public static bool isDrive;//현재 드라이브인지, 폴더인지 확인
        public static string currentStaticpath { get; set; }//현재 경로 저장
        public static string[] currentStateitemsname { get; set; }//현재 경로의 아이템들 이름저장
        public static SubItemInfo[] currentsubIteminfos { get; set; } //현재 폴더의 하위 아이템 정보
        public static DriveInfo[] currentdriveInfos { get; set; } // 드라이버들 정보
        private static int currentMsgcount { get; set; } // 현재 처리 메시지 번호
        public static Stack<string> prePathsave = new Stack<string>();
        public static Stack<string> nextPathsave = new Stack<string>();
        public static bool isError { get; set; } // 현재상태가 에러인지 판단
        public static string errorMessage { get; set; } // 현재 에러 메시지
        */
        //public static ListView currentListview { get; set; }//현재 리스트뷰
        //public static TreeView currentTreeview { get; set; }//현재 트리뷰
        /*
         * public string GetCurrentPath()
        {
            return currentStaticpath;
        }
        public bool GetErrorstate()
        {
            return isError;
        }
        public string GetErrormessage()
        {
            return errorMessage;
        }
        */

        /*
        switch (type)
        {
            case "drive":
                SetDriveInfo((string)data[0], (DriveInfo)data[1]);
                break;
            case "file":
                SetFileInfo(state, (string)data[0], (SubItemInfo)data[1]);
                break;
            case "dir":
                SetDirInfo(state, (string)data[0], (SubItemInfo)data[1]);
                break;

        }*/
        /*
        static void SetDriveInfo(string path, DriveInfo driveInfo)
        {
            if (currentStaticpath.Equals(path))//root다
            {
                string dataTotalsize = Math.Round((Convert.ToDouble(driveInfo.driveTotalsize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                string dataFreesize = Math.Round((Convert.ToDouble(driveInfo.driveFreesize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                ListViewItem item = new ListViewItem(new[] { driveInfo.driveLabel + '(' + driveInfo.driveName + ')', driveInfo.driveType, dataTotalsize, dataFreesize });
                item.Name = "dir|" + driveInfo.driveName;
                item.ImageKey = driveInfo.driveName;
                currentListview.Items.Add(item);
            }
        }
        static void SetFileInfo(string state, string path,SubItemInfo subItem)
        {
            if (currentStaticpath.Equals(path))//리스트뷰는 현재 위치만 수정하면 됨
            {
                ListViewItem item;
                switch (state)
                {
                    case "delete"://리스트에서 삭제
                        item = currentListview.FindItemWithText(subItem.subItemname);//존재하는지 찾기
                        if (item != null)
                        {
                            currentListview.Items.RemoveAt(currentListview.Items.IndexOf(item));
                        }
                        break;
                    case "exist"://리스트에 추가
                        if (!currentListview.Items.ContainsKey(subItem.subItemname))//현재 포함 안된 멤버면
                        {
                            string dataLength = Math.Round((Convert.ToDouble(subItem.subItemlength) / Math.Pow(2, 10)), 2).ToString() + "KB";
                            item = new ListViewItem(new[] { subItem.subItemname, subItem.subItemlastwritetime, subItem.subItemtype, dataLength });
                            item.Name = "file|" + currentStaticpath + subItem.subItemname;
                            item.ImageKey = currentStaticpath + subItem.subItemname;
                            currentListview.Items.Add(item);
                        }
                        break;
                }
            }
        }
        static void SetDirInfo(string state, string path,SubItemInfo subItem)
        {
            ListViewItem item;
            switch (state)
            {
                case "delete"://리스트, 트리뷰, 경로에서 삭제
                    if (currentStaticpath.Equals(path))//리스트뷰는 현재 위치만 수정하면 됨
                    {
                        item = currentListview.FindItemWithText(subItem.subItemname);
                        if (item != null)//리스트뷰에서 존재하면 삭제
                        {
                            currentListview.Items.RemoveAt(currentListview.Items.IndexOf(item));
                        }
                    }
                    //트리뷰 삭제
                    //경로삭제
                    break;
                case "exist"://리스트에 추가
                    if (currentStaticpath.Equals(path))//리스트뷰는 현재 위치만 수정하면 됨
                    {
                        if (!currentListview.Items.ContainsKey(subItem.subItemname))//현재 포함 안된 멤버면 추가
                        {
                            string dataLength = Math.Round((Convert.ToDouble(subItem.subItemlength) / Math.Pow(2, 10)), 2).ToString() + "KB";
                            item = new ListViewItem(new[] { subItem.subItemname, subItem.subItemlastwritetime, subItem.subItemtype, dataLength });
                            item.Name = "dir|" + currentStaticpath + subItem.subItemname + '\\';
                            item.ImageKey = currentStaticpath + subItem.subItemname + '\\';
                            currentListview.Items.Add(item);
                        }
                    }
                    //트리뷰 추가
                    break;
            }
        }
        /*
        static void SetDriveInfo(int msgCount, string[] items,DriveInfo[] data)
        {
            if (msgCount > currentMsgcount)
            {
                isDrive = true;
                currentMsgcount = msgCount;
                currentStaticpath = "root";
                currentStateitemsname = items;
                currentdriveInfos = data;
                updateEvent();
            }
        }
        static void SetError(string errMessage)
        {
            isError = true;
            errorMessage = errMessage;
            updateEvent();
        }
        static void SetSubItemInfo(int msgCount, string staticPath,string[] items,SubItemInfo[] data)
        {
            if (msgCount > currentMsgcount)
            {
                isDrive = false;
                currentMsgcount = msgCount;
                currentStaticpath = staticPath;
                currentStateitemsname = items;
                currentsubIteminfos = data;//현재 폴더의 하위아이템정보 받기
                updateEvent();
            }
        }
        */

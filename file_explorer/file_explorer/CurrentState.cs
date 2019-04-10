using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_explorer
{
    public delegate void UpdateEventHandler();
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

    class CurrentState
    {
        static event UpdateEventHandler updateEvent;

        public void SetUpdateEvent(UpdateEventHandler addEvent)
        {
            updateEvent += new UpdateEventHandler(addEvent);
        }

        //static LoadDriveInfo loadDriveinfo = new LoadDriveInfo();
        //static LoadDirSubItemsInfo loadDirsubiteminfo = new LoadDirSubItemsInfo();
        static MakeStateData makeStatedata = new MakeStateData();
        static CurrentState()
        {
            currentStaticpath = "";
            currentMsgcount = 0;
            makeStatedata.SetDriveInfoToCurrentStateEventHandler(SetStateData);
            //loadDriveinfo.SetDriveInfoToCurrentStateEventHandler(SetDriveInfo);
            //loadDirsubiteminfo.SetSubItemToCurrentStateEvent(SetSubItemInfo);
        }

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

        public string GetCurrentPath()
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
        static void SetStateData(int msgCount, string commandName, object[] data)
        {
            switch (commandName)
            {
                case "error":
                    SetError((string)data[0]);
                    break;
                case "rootload":
                    SetDriveInfo(msgCount, (string[])data[0], (DriveInfo[])data[1]);
                    break;
                case "dirload":
                    SetSubItemInfo(msgCount, (string)data[0], (string[])data[1], (SubItemInfo[])data[2]);
                    break;
                case "MoveItemToDir":
                    //loadDirsubitemsinfo.LoadDirSubItemsInfoResult(msgCount, msgs[1], msgs[2], msgs[3]);
                    break;
            }
        }
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
    }
}

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
        public static Stack<string> prePathsave = new Stack<string>();
        public static Stack<string> nextPathsave = new Stack<string>();
        public static List<string[]> driveInfo = new List<string[]>();
        static MakeData makeStatedata = new MakeData();
        public static string currentStaticpath { get; set; }//현재 경로 저장
        public static object[] currentData { get; set; }//현재 받은 타입의 데이타
        public static bool isClick { get; set; }//버튼이 아닌 클릭이벤트 확인
        public static SendServerEventHandler sendServerEventHandler = new SendServerEventHandler();
        public static ImageList mainImagelist = new ImageList();
        public static Graphics graphic;
        static CurrentState()
        {
            currentStaticpath = "";
            makeStatedata.SetDriveInfoToCurrentStateEventHandler(SetStateData);//상태 변경 데이터 받아옴
            
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
        }

        public void MoveDir(bool click,string path,string context)
        {
            isClick = click;
            currentStaticpath = path;
            sendServerEventHandler.MoveDir(path, context);
        }
    }
}
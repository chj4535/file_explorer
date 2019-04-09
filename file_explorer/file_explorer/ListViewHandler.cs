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
    class ListViewHandler
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
        static int CurrentMsgcount=0;
        static int listViewcount;
        static ListView mainListview = new ListView();
        static ImageList mainImagelist = new ImageList();
        static LoadDriveInfo loadDriveInfo = new LoadDriveInfo();
        static LoadDirSubItemsInfo loadDirsubitemsinfo = new LoadDirSubItemsInfo();
        public ListViewHandler()
        {
        }

        public void ListViewHandlerSetting(ListView mainFormlistview, ImageList mainFormimagelist)
        {
            listViewcount = 0;
            mainListview = mainFormlistview;
            mainImagelist = mainFormimagelist;
            loadDriveInfo.SetDriveInfotoListviewEvnet(SetDriveListView);
            loadDirsubitemsinfo.SetSubItemToListviewEvnet(SetDirSubItemsListView);
        }

        private void SetDriveListView(int msgCount, DriveInfo[] datas)
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetDriveListView(msgCount, datas);
                });
                return;
            }
            if (CurrentMsgcount >= msgCount)
            {
                return;
            }
            CurrentMsgcount = msgCount;
            Console.WriteLine(listViewcount.ToString() + " vs " + msgCount.ToString());
            if (listViewcount < msgCount) // 현재보다 나중의 요청결과인지 확인
            {
                listViewcount = msgCount;
                mainListview.View = View.Details;
                mainListview.Columns.Clear();//칼럼 초기화
                mainListview.Items.Clear();//내용 초기화
                int columWidth = (mainListview.Width - 2) / 4;
                Console.WriteLine(mainListview.Width);
                mainListview.Columns.Add("이름");
                mainListview.Columns.Add("종류");
                mainListview.Columns.Add("전체 크기");
                mainListview.Columns.Add("사용 가능 공간");
                Icon iconFordriver = GetIcon(30);
                foreach (ColumnHeader header in mainListview.Columns)
                {
                    header.Width = columWidth;
                }
                foreach (DriveInfo data in datas)
                {
                    string dataTotalsize = Math.Round((Convert.ToDouble(data.driveTotalsize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                    string dataFreesize = Math.Round((Convert.ToDouble(data.driveFreesize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                    ListViewItem item = new ListViewItem(new[] { data.driveLabel + '(' + data.driveName + ')', data.driveType, dataTotalsize, dataFreesize });
                    item.Name = "dir|" + data.driveName;
                    //treeViewhandler.AddTreeViewPath(data.driveName);
                    if (!mainImagelist.Images.ContainsKey(data.driveName.Split('\\').First()))
                    {
                        // If not, add the image to the image list.
                        mainImagelist.Images.Add(data.driveName.Split('\\').First(), iconFordriver);
                    }
                    item.ImageKey = data.driveName.Split('\\').First();
                    mainListview.Items.Add(item);
                }
            }
        }

        private void SetDirSubItemsListView(int msgCount, SubItemInfo[] datas) // drive 데이터분류
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetDirSubItemsListView(msgCount, datas);
                });
                return;
            }
            if(CurrentMsgcount >= msgCount)
            {
                return;
            }
            CurrentMsgcount = msgCount;
            Console.WriteLine(listViewcount.ToString() + " vs " + msgCount.ToString());
            if (listViewcount < msgCount) // 현재보다 나중의 요청결과인지 확인
            {
                listViewcount = msgCount;
                mainListview.View = View.Details;
                mainListview.Columns.Clear();//칼럼 초기화
                mainListview.Items.Clear();//내용 초기화
                int columWidth = (mainListview.Width - 2) / 4;
                Console.WriteLine(mainListview.Width);
                mainListview.Columns.Add("이름");
                mainListview.Columns.Add("수정한 날짜");
                mainListview.Columns.Add("유형");
                mainListview.Columns.Add("크기");
                foreach (ColumnHeader header in mainListview.Columns)
                {
                    header.Width = columWidth;
                }
                foreach (SubItemInfo data in datas)
                {
                    string dataLength = Math.Round((Convert.ToDouble(data.subItemlength) / Math.Pow(2, 10)), 2).ToString() + "KB";
                    Icon iconSubitem = GetIcon(1);
                    ListViewItem item=new ListViewItem();
                    if (data.isFile)
                    {
                        item = new ListViewItem(new[] { data.subItemname, data.subItemlastwritetime, data.subItemtype, dataLength });
                        item.Name = "file|" + data.subItempath;
                        iconSubitem = GetIcon(2);
                    }
                    if (!data.isFile)
                    {
                        item = new ListViewItem(new[] { data.subItemname, data.subItemlastwritetime, data.subItemtype, null});
                        item.Name = "dir|" + data.subItempath;
                        iconSubitem = GetIcon(3);
                        //treeViewhandler.AddTreeViewPath(data.subItempath);
                    }
                    if (!mainImagelist.Images.ContainsKey(data.subItempath))
                    {
                        // If not, add the image to the image list.
                        mainImagelist.Images.Add(data.subItempath, iconSubitem);
                    }
                    item.ImageKey = data.subItempath;
                    mainListview.Items.Add(item);
                }
            }
        }
    }
}

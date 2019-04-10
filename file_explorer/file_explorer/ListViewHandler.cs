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
    class ListViewHandler : CurrentState
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
        static ListView mainListview = new ListView();
        static ImageList mainImagelist = new ImageList();
        static Label mainitemscount = new Label();
        static Label mainselectedinfo = new Label();
        public ListViewHandler()
        {
        }

        public void ListViewHandlerSetting(ListView mainFormlistview, ImageList mainFormimagelist,Label mainFormitemscount,Label mainFormselectedinfo)
        {
            mainListview = mainFormlistview;
            mainImagelist = mainFormimagelist;
            mainitemscount = mainFormitemscount;
            mainselectedinfo = mainFormselectedinfo;
            mainFormlistview.SmallImageList = mainFormimagelist;
        }

        public void Update()
        {
            if (isDrive)
            {
                SetDriveListView();
            }
            else
            {
                SetDirSubItemsListView();
            }
        }

        private void SetDriveListView()
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetDriveListView();
                });
                return;
            }
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
            foreach (DriveInfo data in currentdriveInfos)
            {
                string dataTotalsize = Math.Round((Convert.ToDouble(data.driveTotalsize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                string dataFreesize = Math.Round((Convert.ToDouble(data.driveFreesize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                ListViewItem item = new ListViewItem(new[] { data.driveLabel + '(' + data.driveName + ')', data.driveType, dataTotalsize, dataFreesize });
                item.Name = "dir|" + data.driveName;
                //treeViewhandler.AddTreeViewPath(data.driveName);
                if (!mainImagelist.Images.ContainsKey(data.driveName))
                {
                    // If not, add the image to the image list.
                    mainImagelist.Images.Add(data.driveName, iconFordriver);
                }
                item.ImageKey = data.driveName;
                mainListview.Items.Add(item);
            }
        }

        private void SetDirSubItemsListView() // drive 데이터분류
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetDirSubItemsListView();
                });
                return;
            }
            //MessageBox.Show(Form.ActiveForm, errorMessage, "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            foreach (SubItemInfo data in currentsubIteminfos)
            {
                string dataLength = Math.Round((Convert.ToDouble(data.subItemlength) / Math.Pow(2, 10)), 2).ToString() + "KB";
                Icon iconSubitem = GetIcon(1);
                ListViewItem item = new ListViewItem();
                if (data.isFile)
                {
                    item = new ListViewItem(new[] { data.subItemname, data.subItemlastwritetime, data.subItemtype, dataLength });
                    item.Name = "file|" + currentStaticpath + data.subItemname;
                    iconSubitem = GetIcon(2);
                }
                if (!data.isFile)
                {
                    item = new ListViewItem(new[] { data.subItemname, data.subItemlastwritetime, data.subItemtype, null });
                    item.Name = "dir|" + currentStaticpath + data.subItemname + '\\';
                    iconSubitem = GetIcon(3);
                    //treeViewhandler.AddTreeViewPath(data.subItempath);
                }
                if (!mainImagelist.Images.ContainsKey(item.Name.Split('|').Last()))
                {
                    // If not, add the image to the image list.
                    mainImagelist.Images.Add(item.Name.Split('|').Last(), iconSubitem);
                }
                item.ImageKey = item.Name.Split('|').Last();
                mainListview.Items.Add(item);
            }
        }
        public void FirstLoad()
        {
            isClick = true;
            sendServerEventHandler.MoveDir("root", "form_load");
        }
        public void ItemDoubleClick()
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    ItemDoubleClick();
                });
                return;
            }
            string itemName = mainListview.SelectedItems[0].Name;
            string[] itemNames = itemName.Split('|');
            if (itemNames[0].Equals("dir"))
            {
                isClick = true;
                sendServerEventHandler.MoveDir(itemNames[1], "listviewdoubleclick");
            }
        }
        public void MainListViewDragDrop(string targetPath, string dragStaticpath, string[] dragItems, string sendType)
        {
            sendServerEventHandler.MoveItemsToDir(targetPath, dragStaticpath, dragItems, "dnd_listviewtolistview");
        }
    }
}

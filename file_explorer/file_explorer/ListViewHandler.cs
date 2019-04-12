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
        //static List<ListViewItem> listitems = new List<ListViewItem>();//실제 저장된 리스트뷰
        //static ListViewItem[] listiems = new listviewitem
        static int listItemcount = 0;//현재 경로의 아이템 갯수 이와 같은 갯수여야만 출력한다.
        static ListView mainListview = new ListView();
        static ImageList mainImagelist = new ImageList();
        static Label mainitemscount = new Label();
        static Label mainselectedinfo = new Label();
        static string listViewpath = "";//리스트뷰가 보여주는 경로
        static string listViewColum = null;//리스트뷰가 드라이브 column을 보여주는가
        static List<ListViewItem> listViewitems = new List<ListViewItem>();
        static bool isChange;
        public ListViewHandler()
        {
        }

        public void ListViewHandlerSetting(ListView mainFormlistview, ImageList mainFormimagelist, Label mainFormitemscount, Label mainFormselectedinfo)
        {
            mainListview = mainFormlistview;
            mainImagelist = mainFormimagelist;
            mainitemscount = mainFormitemscount;
            mainselectedinfo = mainFormselectedinfo;
            mainFormlistview.SmallImageList = mainFormimagelist;
            mainImagelist.Images.Add("dir", GetIcon(5));
            mainImagelist.Images.Add("dirOpen", GetIcon(4));
            mainImagelist.Images.Add("file", GetIcon(2));
            mainImagelist.Images.Add("drive", GetIcon(30));
            mainImagelist.Images.Add("computer", GetIcon(104));
        }

        public void Update()
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    Update();
                });
                return;
            }
            //mainListview.Hide();
            if (!listViewpath.Equals(currentStaticpath))// 현재 표시된 경로와 currentstate의 경로가 다르면 리스트뷰 초기화
            {
                ResetLiveView();//내용지우기
                if (listViewpath.Equals("root"))//원래 드라이브칼럼인데
                {
                    if (!currentStaticpath.Equals("root"))//최신 경로가 드라이브 칼럼이 아니면
                    {
                        SetColumns(false);//폴더 칼럼으로
                    }
                }
                else if (!listViewpath.Equals("root"))//원래 드라이브칼럼이 아닌데
                {
                    if (currentStaticpath.Equals("root"))//최신 경로가 다라이브 칼럼이면
                    {
                        SetColumns(true);//드라이브 칼럼으로
                    }
                }
                listViewpath = currentStaticpath;//경로 최신화
            }
            //MessageBox.Show(DateTime.Now.ToString("h:mm:ss tt"), "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            isChange = false;//변경사항 여부 확인
            SetListView();//내용 추가 삭제
            if (isChange)
            {
                ListViewItem[] listViewitem = listViewitems.ToArray();
                mainListview.BeginUpdate();
                mainListview.Items.AddRange(listViewitem);
                mainListview.EndUpdate();
            }
            //MessageBox.Show(DateTime.Now.ToString("h:mm:ss tt"), "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //mainListview.Show();

            /*
            if (!listViewpath.Equals(currentStaticpath))// 현재 표시된 경로와 currentstate의 경로가 다르면 리스트뷰 초기화
            {
                ResetLiveView();
                listViewpath = currentStaticpath;
                if (currentType.Equals("drive"))//currentstate이 드라이브인데
                {
                    if (listViewColum==null || !listViewColum.Equals("drive")) // 현재 표시된 열도 드라이브가 아니면
                    {
                        SetColumns(true);
                    }
                }
                else//현재 currentstate이 폴더나 파일인데
                {
                    if (listViewColum.Equals("drive")) // 현재 표시된 열 드라이브면
                    {
                        SetColumns(false);
                    }
                }
            }
            if (currentTypestate.Equals("check"))
            {
                string path = (string)currentTypedata[0];
                if (currentStaticpath.Equals(path))//리스트뷰는 현재 위치만 수정하면 됨
                {
                    listItemcount = (int)currentTypedata[1];
                }
            }
            else
            {
                SetListView();//내용 추가 삭제
            }
            if (listItemcount == listitems.Count)
            {
                PrintListItems();
            }
            */
        }

        private void SetListView()
        {
            int dataLength = (int)currentData[0];
            for (int dataNum = 3; dataNum < dataLength + 3; dataNum++)
            {
                string msg = (string)currentData[dataNum];
                string[] msgs = msg.Split('|');
                string[] infos = msgs[2].Split('/');
                if (currentStaticpath.Equals(infos[0]))
                {
                    switch (msgs[0])
                    {
                        case "file":
                        case "dir":
                            SetSubItemInfo(msgs[0], msgs[1], infos);
                            break;
                        case "drive":
                            SetDriveInfo(msgs[0], msgs[1], infos);
                            break;
                    }
                }
            }
        }
        /*
             * info[0] : path
             * info[1] : name
             * info[2] : extenstion / arrtibute
             * info[3] : lastwritetime
             * info[4] : length(size)
             */
        static void SetSubItemInfo(string type, string state, string[] infos)
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetSubItemInfo(type, state, infos);
                });
                return;
            }
            if (state.Equals("disable")){
                return;
            }
            string path = infos[0];
            ListViewItem item;
            string itemName = infos[1];
            string itemExtenstion = infos[2];
            string itemLastwritetime = infos[3];
            switch (state)
            {
                case "delete"://리스트에서 삭제
                    if (mainListview.Items.ContainsKey(itemName)) //리스트뷰에 있으면
                    {
                        isChange = true;
                        listViewitems.RemoveAt(mainListview.Items.IndexOfKey(itemName));
                        //mainListview.Items.RemoveAt(mainListview.Items.IndexOfKey(itemName));
                    }
                    break;
                case "exist"://리스트에 추가
                    string checkKey ="";
                    if (type.Equals("file"))
                    {
                        checkKey = "file|" + currentStaticpath + itemName;
                    }
                    else
                    {
                        checkKey = "dir|" + currentStaticpath + itemName + '\\';
                    }
                    if (!mainListview.Items.ContainsKey(checkKey)) //리스트뷰에 없으면
                    {
                        isChange = true;
                        string dataLength = null;
                        if (type.Equals("file"))
                        {
                            //Icon iconDir = GetIcon(2);
                            string itemSize = infos[4];
                            dataLength = Math.Round((Convert.ToDouble(itemSize) / Math.Pow(2, 10)), 2).ToString() + "KB";
                            item = new ListViewItem(new[] { itemName, itemLastwritetime, itemExtenstion, dataLength });
                            item.Name = "file|" + currentStaticpath + itemName;
                            //item.ImageKey = currentStaticpath + itemName;
                            item.ImageKey = "file";
                            /*if (!mainImagelist.Images.ContainsKey(item.ImageKey))//이미지리스트에 없으면 추가
                            {
                                // If not, add the image to the image list.
                                mainImagelist.Images.Add(item.ImageKey, iconDir);
                            }*/
                        }
                        else
                        {
                            //Icon iconDir = GetIcon(5);
                            //Icon iconDiropen = GetIcon(4);
                            //iconSubitem = GetIcon(3);
                            item = new ListViewItem(new[] { itemName, itemLastwritetime, itemExtenstion, dataLength });
                            item.Name = "dir|" + currentStaticpath + itemName + '\\';
                            //item.ImageKey = currentStaticpath + itemName + '\\';
                            item.ImageKey = "dir";
                            /*if (!mainImagelist.Images.ContainsKey(item.ImageKey))//이미지리스트에 없으면 추가
                            {
                                // If not, add the image to the image list.
                                mainImagelist.Images.Add(item.ImageKey, iconDir);
                                mainImagelist.Images.Add('\\'+item.ImageKey, iconDiropen);
                            }*/
                        }
                        listViewitems.Add(item);
                        //mainListview.Items.Add(item);
                    }
                    break;
            }
        }
        /*
        driveInfo += d.Name + "/"; //드라이브 이름
                driveInfo += d.VolumeLabel + "/"; //드라이브 라벨
                driveInfo += d.DriveType + "/"; //드라이브 타입
                driveInfo += d.TotalSize.ToString() + "/";//드라이브 전체 크기
                driveInfo += d.TotalFreeSpace.ToString() + "/";//드라이브 여분 크기
                */
        static void SetDriveInfo(string type, string state, string[] infos)
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetDriveInfo(type, state, infos);
                });
                return;
            }
            string path = infos[0];
            ListViewItem item;
            string itemName = infos[1];
            string itemVolumeLabel = infos[2];
            if (itemVolumeLabel.Equals(""))
            {
                itemVolumeLabel = "로컬 디스크";
            }
            string itemType = infos[3];
            string itemTotalsize = infos[4];
            string itemTotalfreespace = infos[5];
            string[] diskNameset = { itemName, itemVolumeLabel + '(' + itemName + ')' };
            if (!driveInfo.Contains(diskNameset))
            {
                driveInfo.Add(diskNameset);
            }
            if (!mainListview.Items.ContainsKey("dir|" + itemName)) //리스트뷰에 없으면
            {
                isChange = true;
                //Icon iconFordriver = GetIcon(30);
                string dataTotalsize = Math.Round((Convert.ToDouble(itemTotalsize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                string dataFreesize = Math.Round((Convert.ToDouble(itemTotalfreespace) / Math.Pow(2, 30)), 2).ToString() + "GB";
                item = new ListViewItem(new[] { itemVolumeLabel + '(' + itemName + ')', itemType, dataTotalsize, dataFreesize });
                item.Name = "dir|" + itemName;
                //item.ImageKey = itemName;
                item.ImageKey = "drive";
                listViewitems.Add(item);
                /*if (!mainImagelist.Images.ContainsKey(itemName))//이미지리스트에 없으면 추가
                {
                    // If not, add the image to the image list.
                    mainImagelist.Images.Add(itemName, iconFordriver);
                }*/
                //mainListview.Items.Add(item);
            }
        }
        /*
        static void SetSubItemInfo()
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetSubItemInfo();
                });
                return;
            }
            string type = currentType;
            string state = currentTypestate;
            string path = (string)currentTypedata[0];
            SubItemInfo subItem = (SubItemInfo)currentTypedata[1];

            if (currentStaticpath.Equals(path))//리스트뷰는 현재 위치만 수정하면 됨
            {
                ListViewItem item;
                switch (state)
                {
                    case "delete"://리스트에서 삭제
                        item = mainListview.FindItemWithText(subItem.subItemname);//존재하는지 찾기
                        if (item != null)
                        {
                            mainListview.Items.RemoveAt(mainListview.Items.IndexOf(item));
                        }
                        break;
                    case "exist"://리스트에 추가
                        if (!mainListview.Items.ContainsKey(subItem.subItemname))//현재 포함 안된 멤버면
                        {
                            string dataLength = Math.Round((Convert.ToDouble(subItem.subItemlength) / Math.Pow(2, 10)), 2).ToString() + "KB";
                            item = new ListViewItem(new[] { subItem.subItemname, subItem.subItemlastwritetime, subItem.subItemtype, dataLength });
                            if (type.Equals("file"))
                            {
                                item.Name = "file|" + currentStaticpath + subItem.subItemname;
                                item.ImageKey = currentStaticpath + subItem.subItemname;
                            }
                            else {
                                item.Name = "dir|" + currentStaticpath + subItem.subItemname + '\\';
                                item.ImageKey = currentStaticpath + subItem.subItemname + '\\';
                            }
                            mainListview.Items.Add(item);
                        }
                        break;
                }
            }
        }

        static void SetDriveInfo()
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetDriveInfo();
                });
                return;
            }
            string path = (string)currentTypedata[0];
            DriveInfo driveInfo = (DriveInfo)currentTypedata[1];
            if (currentStaticpath.Equals(path))//root다
            {
                string dataTotalsize = Math.Round((Convert.ToDouble(driveInfo.driveTotalsize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                string dataFreesize = Math.Round((Convert.ToDouble(driveInfo.driveFreesize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                ListViewItem item = new ListViewItem(new[] { driveInfo.driveLabel + '(' + driveInfo.driveName + ')', driveInfo.driveType, dataTotalsize, dataFreesize });
                item.Name = "dir|" + driveInfo.driveName;
                item.ImageKey = driveInfo.driveName;
                mainListview.Items.Add(item);
            }
        }
        */

        private void ResetLiveView()
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    ResetLiveView();
                });
                return;
            }
            mainListview.Items.Clear();
            listViewitems.Clear();
        }

        private void SetColumns(bool isDrive)
        {

            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetColumns(isDrive);
                });
                return;
            }
            mainListview.View = View.Details;
            mainListview.Columns.Clear();//칼럼 초기화
            mainListview.Items.Clear();//내용 초기화
            int columWidth = (mainListview.Width - 2) / 4;
            Console.WriteLine(mainListview.Width);
            if (isDrive)//드라이브 컬럼으로
            {
                listViewColum = "drive";
                mainListview.Columns.Add("이름");
                mainListview.Columns.Add("종류");
                mainListview.Columns.Add("전체 크기");
                mainListview.Columns.Add("사용 가능 공간");
                foreach (ColumnHeader header in mainListview.Columns)
                {
                    header.Width = columWidth;
                }
            }
            else
            {
                listViewColum = "dir";
                mainListview.Columns.Add("이름");
                mainListview.Columns.Add("수정한 날짜");
                mainListview.Columns.Add("유형");
                mainListview.Columns.Add("크기");
                foreach (ColumnHeader header in mainListview.Columns)
                {
                    header.Width = columWidth;
                }
            }
            /*
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    SetColumns(isDrive);
                });
                return;
            }
            mainListview.View = View.Details;
            mainListview.Columns.Clear();//칼럼 초기화
            mainListview.Items.Clear();//내용 초기화
            int columWidth = (mainListview.Width - 2) / 4;
            Console.WriteLine(mainListview.Width);
            if (isDrive)//드라이브 컬럼으로
            {
                listViewColum = "drive";
                mainListview.Columns.Add("이름");
                mainListview.Columns.Add("종류");
                mainListview.Columns.Add("전체 크기");
                mainListview.Columns.Add("사용 가능 공간");
                foreach (ColumnHeader header in mainListview.Columns)
                {
                    header.Width = columWidth;
                }
            }
            else
            {
                listViewColum = "dir";
                mainListview.Columns.Add("이름");
                mainListview.Columns.Add("수정한 날짜");
                mainListview.Columns.Add("유형");
                mainListview.Columns.Add("크기");
                foreach (ColumnHeader header in mainListview.Columns)
                {
                    header.Width = columWidth;
                }
            }
            */
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
            if (itemNames[0].Equals("dir")) //파일이 아닌 폴더(드라이브) 선택일 시
            {
                MoveDir(true, itemNames[1],"listviewdoubleclick");
                //isClick = true;
                //ResetLieView();
                //currentStaticpath = itemNames[1];
                //sendServerEventHandler.MoveDir(itemNames[1], "listviewdoubleclick");
            }
        }

        public void MainListViewDragDrop(string targetPath, string dragStaticpath, string[] dragItems, string sendType)
        {
            sendServerEventHandler.MoveItemsToDir(targetPath, dragStaticpath, dragItems, "dnd_listviewtolistview");
        }

    }
}
/*
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
*/

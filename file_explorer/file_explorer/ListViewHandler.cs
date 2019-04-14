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
        static ListView mainListview = new ListView();
        static Label mainitemscount = new Label();
        static Label mainselectedinfo = new Label();
        static string listViewpath = "";//리스트뷰가 보여주는 경로
        static ListViewHitTestInfo contextMenuitem;
        static List<ListViewItem> listViewitems = new List<ListViewItem>();
        static bool isChange;
        static List<ListViewItem> copyItems = new List<ListViewItem>();
        public ListViewHandler()
        {
        }
        public void copyItem()
        {
            copyItems.Clear();
            foreach (ListViewItem selectItem in mainListview.SelectedItems)
            {
                copyItems.Add(selectItem);
            }
        }
        public bool IsCopyItems()
        {
            if (copyItems.Count > 0)
            {
                return true;
            }
            return false;
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
            mainImagelist.Images.Add("refresh", GetIcon(224));
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
            //mainListview.Items.Clear();
            listViewitems.Clear();
            SetListView();//내용 추가 삭제
            if (isChange)
            {
                ListViewItem[] listViewitem = listViewitems.ToArray();
                mainListview.BeginUpdate();
                mainListview.Items.AddRange(listViewitem);
                mainListview.EndUpdate();
            }

            //MessageBox.Show(DateTime.Now.ToString("h:mm:ss tt"), "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SetListView()
        {
            int dataLength = (int)currentData[0];
            for (int dataNum = 3; dataNum < dataLength + 3; dataNum++)
            {
                string msg = (string)currentData[dataNum];
                string[] msgs = msg.Split('|');
                string[] infos = msgs[2].Split('/');
                if (msgs[1].Equals("exist") && mainListview.Items.Count > 0)
                {
                    mainListview.Items.Clear();
                }
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
            if (state.Equals("disable"))
            {
                return;
            }
            string path = infos[0];
            ListViewItem item;
            string itemName = infos[1];
            string checkKey = "";
            if (type.Equals("file"))
            {
                checkKey = "file|" + currentStaticpath + itemName;
            }
            else
            {
                checkKey = "dir|" + currentStaticpath + itemName + '\\';
            }
            switch (state)
            {
                case "delete"://리스트에서 삭제
                    if (mainListview.Items.ContainsKey(checkKey)) //리스트뷰에 있으면
                    {
                        mainListview.Items.RemoveAt(mainListview.Items.IndexOfKey(checkKey));
                    }
                    break;
                case "exist"://리스트 체크
                case "add"://리스트 체크
                    string itemExtenstion = infos[2];
                    string itemLastwritetime = infos[3];
                    if (!mainListview.Items.ContainsKey(checkKey)) //리스트뷰에 없으면
                    {
                        isChange = true;
                        string dataLength = null;
                        if (type.Equals("file"))
                        {
                            string itemSize = infos[4];
                            dataLength = Math.Round((Convert.ToDouble(itemSize) / Math.Pow(2, 10)), 2).ToString() + "KB";
                            item = new ListViewItem(new[] { itemName, itemLastwritetime, itemExtenstion, dataLength });
                            item.Name = "file|" + currentStaticpath + itemName;
                            item.ImageKey = "file";
                        }
                        else
                        {
                            item = new ListViewItem(new[] { itemName, itemLastwritetime, itemExtenstion, dataLength });
                            item.Name = "dir|" + currentStaticpath + itemName + '\\';
                            item.ImageKey = "dir";
                        }
                        listViewitems.Add(item);
                    }
                    break;
            }
        }
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
                string dataTotalsize = Math.Round((Convert.ToDouble(itemTotalsize) / Math.Pow(2, 30)), 2).ToString() + "GB";
                string dataFreesize = Math.Round((Convert.ToDouble(itemTotalfreespace) / Math.Pow(2, 30)), 2).ToString() + "GB";
                item = new ListViewItem(new[] { itemVolumeLabel + '(' + itemName + ')', itemType, dataTotalsize, dataFreesize });
                item.Name = "dir|" + itemName;
                item.ImageKey = "drive";
                listViewitems.Add(item);
            }
        }

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
                mainListview.Columns.Add("이름");
                mainListview.Columns.Add("수정한 날짜");
                mainListview.Columns.Add("유형");
                mainListview.Columns.Add("크기");
                foreach (ColumnHeader header in mainListview.Columns)
                {
                    header.Width = columWidth;
                }
            }
        }
        public void ContextMeueItemSelect(Point point)//메뉴가 나올 아이템 저장
        {
            var mousePoint = mainListview.PointToClient(point);
            contextMenuitem = mainListview.HitTest(mousePoint);//메뉴가 나올 위치에 맞는 아이템정보 저장
        }

        public void ContextMeueItemOpen()//메뉴가 나올 아이템 저장
        {
            if (mainListview.InvokeRequired)
            {
                mainListview.Invoke((MethodInvoker)delegate {
                    ContextMeueItemOpen();
                });
                return;
            }
            //ListViewItem selectItem = mainListview.SelectedItems[0];
            string itemName = contextMenuitem.Item.Name;
            string[] itemNames = itemName.Split('|');
            if (itemNames[0].Equals("dir")) //파일이 아닌 폴더(드라이브) 선택일 시
            {
                MoveDir(true, itemNames[1], "listviewdoubleclick");
            }
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
                MoveDir(true, itemNames[1], "listviewdoubleclick");
            }
        }

        public void MainListViewItemDrag(ListViewItem dragItem)
        {
            var items = new List<ListViewItem>();
            items.Add(dragItem);
            foreach (ListViewItem mainFormitem in mainListview.SelectedItems)
            {
                if (!items.Contains(mainFormitem))
                {
                    items.Add(mainFormitem);
                }
            }
            mainListview.DoDragDrop(items, DragDropEffects.Move);
        }

        public void MainListViewDragOver(DragEventArgs e)
        {
            var items = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));
            var mousePoint = mainListview.PointToClient(new Point(e.X, e.Y));
            var hit = mainListview.HitTest(mousePoint);
            if (hit.Item != null && hit.Item.Name.Split('|').First().Equals("dir") && !items.Contains(hit.Item))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void MainListViewDragDrop(DragEventArgs e)
        {
            var items = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));
            var mousePoint = mainListview.PointToClient(new Point(e.X, e.Y));
            var hit = mainListview.HitTest(mousePoint);
            if (hit.Item != null && hit.Item.Name.Split('|').First().Equals("dir") && !items.Contains(hit.Item))
            {
                string targetPath = hit.Item.Name.Split('|').Last();
                targetPath = targetPath.Substring(0, targetPath.LastIndexOf('\\'));
                string dragStaticpath = items[0].Name.Split('|').Last();
                if (items[0].Name.Split('|').First().Equals("dir"))
                {
                    dragStaticpath = dragStaticpath.Substring(0, dragStaticpath.LastIndexOf('\\'));
                }
                dragStaticpath = dragStaticpath.Substring(0, dragStaticpath.LastIndexOf('\\'));
                //string dragStaticpath = firstItempath.Substring(0, firstItempath.LastIndexOf('\\'));
                int count = 0;
                foreach (ListViewItem item in items)
                {
                    string[] itemNames = item.Name.Split('|');


                    string itemType = itemNames[0];
                    string itemPath = itemNames[1];
                    string itemName;
                    if (itemType.Equals("dir"))
                    {
                        string[] itemPaths = itemPath.Split('\\');
                        itemName = "dir" + '/' + itemPaths[itemPaths.Length - 2];
                    }
                    else
                    {
                        itemName = "file" + '/' + itemPath.Split('\\').Last();
                    }
                    sendServerEventHandler.MoveItemsToDir(targetPath, dragStaticpath, itemName, "dnd_listviewtolistview");
                }
                //sendServerEventHandler.MoveItemsToDir(targetPath, dragStaticpath, dragItems, "dnd_listviewtolistview");
                //listViewhandler.MainListViewDragDrop(targetPath, dragStaticpath, itemNames, "dnd_listviewtolistview");
            }
            //sendServerEventHandler.MoveItemsToDir(targetPath, dragStaticpath, dragItems, "dnd_listviewtolistview");
        }

        public void ShowError()
        {
            string msg = (string)currentData[3];
            string[] msgs = msg.Split('|');
            MessageBox.Show(Form.ActiveForm, msgs[0], "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ContextMeueItemRename(int addXpoint)
        {
            int selectIndex = mainListview.Items.IndexOfKey(contextMenuitem.Item.Name);
            ListViewItem saveItem = mainListview.Items[selectIndex];
            TextBox test = new TextBox();
            test.Text = saveItem.Text;
            test.Font= new System.Drawing.Font("굴림", 12F);
            SizeF size = graphic.MeasureString(test.Text, test.Font);
            test.Size= new Size(150,20);
            test.Width = (int)size.Width + 2;
            Point itemPoint = saveItem.Position;
            itemPoint.X = itemPoint.X+ addXpoint+25;
            itemPoint.Y = itemPoint.Y + 70;
            test.Location = itemPoint;
            Form mainForm = mainListview.FindForm();
            mainForm.Controls.Add(test);
            test.BringToFront();
            test.Focus();
            test.Name = contextMenuitem.Item.Name;
            test.Leave += new System.EventHandler(this.ReNameTextBoxDelete);
            test.KeyDown += new System.Windows.Forms.KeyEventHandler(ReNameTextBoxKeyDown);
            //test.Enter += new System.EventHandler(this.ReNameTextBoxDelete);
        }
        public void ReNameTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            TextBox renameTextbox = sender as TextBox;
            if (e.KeyData == Keys.Enter)
            {
                Form mainForm = mainListview.FindForm();
                mainForm.Controls.Remove(renameTextbox);
            }
        }
        public void ReNameTextBoxDelete(object sender, EventArgs e)
        {
            //이름 바꾸는 명령
            TextBox renameTextbox = sender as TextBox;
            string[] itemNames = renameTextbox.Name.Split('|');


            string itemType = itemNames[0];
            string itemFullpath = itemNames[1];
            string itemName;
            string itemPath;
            string type;
            if (itemType.Equals("dir"))
            {
                type = "dir";
                itemPath = itemFullpath.Substring(0, itemFullpath.LastIndexOf('\\'));
                itemPath = itemPath.Substring(0, itemPath.LastIndexOf('\\'))+'\\';
                string[] itemPaths = itemFullpath.Split('\\');
                itemName =itemPaths[itemPaths.Length - 2];
            }
            else
            {
                type = "file";
                itemPath = itemFullpath.Substring(0, itemFullpath.LastIndexOf('\\'))+'\\';
                itemName = itemFullpath.Split('\\').Last();
            }
            string renameName = renameTextbox.Text;
            if (!itemName.Equals(renameName))
            {
                sendServerEventHandler.RenameFileDir(type, itemPath, itemName, renameName);
            }
            
            //sendServerEventHandler.MoveItemsToDir(itemPath, itemPath, itemName, "dnd_listviewtolistview");
            
            //이후 삭제
            Form mainForm = mainListview.FindForm();
            mainForm.Controls.Remove(renameTextbox);
        }

        public void DeleteItem()
        {
            foreach (ListViewItem item in mainListview.SelectedItems)
            {
                string[] itemNames = item.Name.Split('|');
                string itemType = itemNames[0];
                string itemPath = itemNames[1];
                string itemName;
                if (itemType.Equals("dir"))
                {
                    string[] itemPaths = itemPath.Split('\\');
                    //dragStaticpath = dragStaticpath.Substring(0, dragStaticpath.LastIndexOf('\\'));
                    itemName = "dir" + '/' + itemPath.Substring(0,itemPath.LastIndexOf('\\'));
                }
                else
                {
                    itemName = "file" + '/' + itemPath;
                }
                sendServerEventHandler.DeleteFileDir(itemName);
            }
        }
    }
}

/*
public void MainListViewDragDrop(string targetPath, string dragStaticpath, string[] dragItems, string sendType)
{
    sendServerEventHandler.MoveItemsToDir(targetPath, dragStaticpath, dragItems, "dnd_listviewtolistview");
}
*/

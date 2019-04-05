using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace file_explorer
{
    public partial class Main_form : Form
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

        string userId;
        ClientSocketHandler clientSocket = new ClientSocketHandler();
        LoadDirSubItemsInfo loadDirsubitemsinfo = new LoadDirSubItemsInfo();
        LoadDriveInfo loadDriveInfo = new LoadDriveInfo();
        int listViewcount;
        public Main_form(string loginformUserId)
        {
            listViewcount = 0;
            InitializeComponent();
            userId = loginformUserId;
            loadDriveInfo.SetDriveEvnet(SetDriveListView);
            loadDirsubitemsinfo.SetSubItemEvnet(SetDirSubItemsListView);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user_name_label.Text = userId;
            mainFormlistview.SmallImageList = listViewimagelist;
            clientSocket.OnSendData("rootload" + "|", null);
        }
        private void SetDriveListView(int msgCount, DriveInfo[] datas)
        {
            Console.WriteLine(listViewcount.ToString()+" vs "+msgCount.ToString());
            if (listViewcount < msgCount) // 현재보다 나중의 요청결과인지 확인
            {
                if (mainFormlistview.InvokeRequired)
                {
                    mainFormlistview.Invoke((MethodInvoker)delegate ()
                    {
                        listViewcount = msgCount;
                        mainFormlistview.View = View.Details;
                        mainFormlistview.Columns.Clear();//칼럼 초기화
                        mainFormlistview.Items.Clear();//내용 초기화
                        int columWidth = (mainFormlistview.Width - 2) / 4;
                        Console.WriteLine(mainFormlistview.Width);
                        mainFormlistview.Columns.Add("이름");
                        mainFormlistview.Columns.Add("종류");
                        mainFormlistview.Columns.Add("전체 크기");
                        mainFormlistview.Columns.Add("사용 가능 공간");
                        Icon iconFordriver = GetIcon(30);
                        foreach (ColumnHeader header in mainFormlistview.Columns)
                        {
                            header.Width = columWidth;
                        }
                        foreach (DriveInfo data in datas)
                        {
                            string dataTotalsize = Math.Round((Convert.ToDouble(data.driveTotalsize) / Math.Pow(2, 30)), 2).ToString()+"GB";
                            string dataFreesize = Math.Round((Convert.ToDouble(data.driveFreesize) / Math.Pow(2, 30)), 2).ToString()+ "GB";
                            ListViewItem item = new ListViewItem(new[] { data.driveLabel + '(' + data.driveName + ')', data.driveType, dataTotalsize,dataFreesize });
                            item.Name = "dir|"+data.driveName;
                            if (!listViewimagelist.Images.ContainsKey(data.driveName))
                            {
                                // If not, add the image to the image list.
                                listViewimagelist.Images.Add(data.driveName, iconFordriver);
                            }
                            item.ImageKey = data.driveName;
                            mainFormlistview.Items.Add(item);
                        }
                    });
                }
                
            }
        }

        private void SetDirSubItemsListView(int msgCount, SubItemInfo[] datas)
        {
            Console.WriteLine(listViewcount.ToString() + " vs " + msgCount.ToString());
            if (listViewcount < msgCount) // 현재보다 나중의 요청결과인지 확인
            {
                if (mainFormlistview.InvokeRequired)
                {
                    mainFormlistview.Invoke((MethodInvoker)delegate ()
                    {
                        listViewcount = msgCount;
                        mainFormlistview.View = View.Details;
                        mainFormlistview.Columns.Clear();//칼럼 초기화
                        mainFormlistview.Items.Clear();//내용 초기화
                        int columWidth = (mainFormlistview.Width - 2) / 4;
                        Console.WriteLine(mainFormlistview.Width);
                        mainFormlistview.Columns.Add("이름");
                        mainFormlistview.Columns.Add("수정한 날짜");
                        mainFormlistview.Columns.Add("유형");
                        mainFormlistview.Columns.Add("크기");
                        foreach (ColumnHeader header in mainFormlistview.Columns)
                        {
                            header.Width = columWidth;
                        }
                        foreach (SubItemInfo data in datas)
                        {
                            string dataLength = Math.Round((Convert.ToDouble(data.subItemlength) / Math.Pow(2, 10)), 2).ToString() + "KB";
                            ListViewItem item = new ListViewItem(new[] { data.subItemname , data.subItemlastwritetime ,data.subItemtype, dataLength });
                            Icon iconSubitem = GetIcon(1);
                            if (data.isFile)
                            {
                                item.Name = "file|" + data.subItempath;
                                iconSubitem = GetIcon(2);
                            }
                            if (!data.isFile)
                            {
                                item.Name = "dir|" + data.subItempath;
                                iconSubitem = GetIcon(3);
                            }
                            if (!listViewimagelist.Images.ContainsKey(data.subItempath))
                            {
                                // If not, add the image to the image list.
                                listViewimagelist.Images.Add(data.subItempath, iconSubitem);
                            }
                            item.ImageKey = data.subItempath;
                            mainFormlistview.Items.Add(item);
                        }
                    });
                }
            }
        }

        private void ListViewDoubleClick(object sender, MouseEventArgs e)
        {
            //ListViewItem item = sender as ListViewItem;
            //MessageBox.Show(mainFormlistview.SelectedItems[0].Name);
            string itemName = mainFormlistview.SelectedItems[0].Name;
            string[] itemNames = itemName.Split('|');
            if (itemNames[0].Equals("dir"))
            {
                clientSocket.OnSendData("dirload" + "|" + itemNames[1], null);
            }
        }
    }
}

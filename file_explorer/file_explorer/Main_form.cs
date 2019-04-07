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
        Stack<string> prePathsave = new Stack<string>();
        Stack<string> nextPathsave = new Stack<string>();
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
            mainFormtreeview.Nodes.Add("내 PC");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user_name_label.Text = userId;
            mainFormlistview.SmallImageList = listViewimagelist;
            backButton.Enabled = false;
            nextButton.Enabled = false;
            backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            backButton.ForeColor = System.Drawing.Color.Gray;
            nextButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            nextButton.ForeColor = System.Drawing.Color.Gray;
            MoveDir("root", true);
            
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
                            AddTreeViewPath(data.driveName);
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
                                AddTreeViewPath(data.subItempath);
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
        private void MakePathButton(string dirPath)
        {
            Graphics graphic = this.CreateGraphics();
            mainFormcombobox.Controls.Clear();
            //mainFormcombobox.Text = "내 PC";
            Button mainButton = new Button();
            mainButton.Location = new Point(0,0);
            mainButton.Cursor = Cursors.Default;
            mainButton.Text = "내 PC";
            mainButton.Name = "root";
            SizeF size = graphic.MeasureString("내 PC", mainButton.Font);
            mainButton.Width = (int)size.Width+ 20;
            mainButton.Height = mainFormcombobox.Height;


            mainButton.Click += (s, e) => { MoveDir("root", true);};
            //mainButton.BringToFront();
            mainFormcombobox.Controls.Add(mainButton);

            if (!dirPath.Equals("root"))
            {
                string[] dirPaths=dirPath.Split('\\');
                int buttonCount = 0;
                int preButtonpoint = 0;
                string path = "";
                foreach(string dirName in dirPaths)
                {
                    if (dirName.Equals("")) break;
                    preButtonpoint += mainFormcombobox.Controls[buttonCount].Size.Width - 2;
                    Button dirButton = new Button();
                    dirButton.Location = new Point(preButtonpoint, 0);
                    dirButton.Cursor = Cursors.Default;
                    //dirButton.Size = new Size(25, mainFormcombobox.Height-3);
                    //dirButton.AutoSize = true;
                    //dirButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    //dirButton.BringToFront();
                    dirButton.Text = dirName;
                    size = graphic.MeasureString(dirName, mainButton.Font);
                    dirButton.Width = (int)size.Width + 15;
                    dirButton.Height = mainFormcombobox.Height;
                    

                    path+=dirName+"\\";
                    dirButton.Name = path;
                    dirButton.Click += (s,e) => { MoveDir(dirButton.Name, true); };
                    mainFormcombobox.Controls.Add(dirButton);
                    buttonCount += 1;
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
                MoveDir(itemNames[1],true);
            }
        }

        private void MoveDir(string path,bool isClick)
        {
            if (isClick)
            {
                nextPathsave.Clear();
                nextButton.Enabled = false;
                nextButton.BackColor = System.Drawing.SystemColors.ScrollBar;
                nextButton.ForeColor = System.Drawing.Color.Gray;
            }
            if (path.Equals("root"))
            {
                clientSocket.OnSendData("rootload" + "|", null);
                MakePathButton("root");
                if (prePathsave.Count==0 || !prePathsave.Peek().Equals("root"))
                {
                    prePathsave.Push("root");
                    if (prePathsave.Count>1)
                    {
                        backButton.Enabled = true;
                        backButton.BackColor = System.Drawing.SystemColors.Window;
                        backButton.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            else
            {
                clientSocket.OnSendData("dirload" + "|" + path, null);
                MakePathButton(path);
                if (prePathsave.Count==0 || !prePathsave.Peek().Equals(path))
                {
                    prePathsave.Push(path);
                    if (prePathsave.Count > 1)
                    {
                        backButton.Enabled = true;
                        backButton.BackColor = System.Drawing.SystemColors.Window;
                        backButton.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            nextPathsave.Push(prePathsave.Pop()); //현재 경로를 다음스택에 저장해놓음
            if(prePathsave.Count == 1)
            {
                backButton.Enabled = false;
                backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
                backButton.ForeColor = System.Drawing.Color.Gray;
            }
            nextButton.Enabled = true;
            nextButton.BackColor = System.Drawing.SystemColors.Window;
            nextButton.ForeColor = System.Drawing.Color.Black;
            MoveDir(prePathsave.Peek(),false);
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            prePathsave.Push(nextPathsave.Pop()); //현재 경로를 다음스택에 저장해놓음
            if (nextPathsave.Count == 0)
            {
                nextButton.Enabled = false;
                nextButton.BackColor = System.Drawing.SystemColors.ScrollBar;
                nextButton.ForeColor = System.Drawing.Color.Gray;
            }
            backButton.Enabled = true;
            backButton.BackColor = System.Drawing.SystemColors.Window;
            backButton.ForeColor = System.Drawing.Color.Black;
            MoveDir(prePathsave.Peek(), false);
        }

        private void upperButton_Click(object sender, EventArgs e)
        {
            if (prePathsave.Count <2 )//바탕화면으로 이동
            {
                upperButton.Enabled = false;
            }
            else
            {
                MoveDir(prePathsave.ElementAt(1), true);
            }
        }

        private void AddTreeViewPath(string dirPath)
        {
            string[] dirPaths = dirPath.Split('\\');
            TreeNode currentNode = new TreeNode();
            currentNode = mainFormtreeview.Nodes[0];
            string currentdirpath = "";
            foreach (string dirName in dirPaths)
            {
                currentdirpath += dirName + "\\";
                if (dirName.Equals("")) break;
                if (!currentNode.Nodes.ContainsKey(dirName))
                {
                    currentNode.Nodes.Add(new TreeNode(dirName) { Name = dirName });
                }
                currentNode = currentNode.Nodes[dirName];
            }
        }

        private void mainFormtreeview_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currentNode = mainFormtreeview.SelectedNode;
            string dirpath = "";
            while (true)
            {
                if (!currentNode.Parent.Text.Equals("내 PC"))
                {
                    dirpath = currentNode.Name + "\\" + dirpath;
                    currentNode = currentNode.Parent;
                }
                else
                {
                    dirpath = currentNode.Name + "\\" + dirpath;
                    break;
                }
            }
            MoveDir(dirpath, true);
        }
    }
}

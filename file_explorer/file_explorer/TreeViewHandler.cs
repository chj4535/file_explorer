using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace file_explorer
{
    class TreeViewHandler : CurrentState
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
        static bool isAdd;
        static bool isSelect;
        static TreeNode collapseTreenode = new TreeNode();
        static TreeView mainTreeview = new TreeView();
        static TreeNode focusTreenode = new TreeNode();
        //static ImageList mainImagelist = new ImageList();
        static List<TreeNode> treeNodeitems = new List<TreeNode>();
        static TreeNode setTreenode;
        public TreeViewHandler()
        {
        }
        public void Update()
        {
            if (mainTreeview.InvokeRequired)
            {
                mainTreeview.Invoke((MethodInvoker)delegate {
                    Update();
                });
                return;
            }
            isAdd = false;
            mainTreeview.BeginUpdate();
            SetTreeView();
            if (isAdd)
            {
                TreeNode[] treeNodeitem = treeNodeitems.ToArray();
                setTreenode.Nodes.AddRange(treeNodeitem);
            }
            setFocusTreeview();
            mainTreeview.EndUpdate();
        }

        public void TreeViewHandlerSetting(TreeView mainFormtreeview)
        {
            mainTreeview = mainFormtreeview;
            currentStaticpath = "root";
            mainTreeview.Nodes.Add(new TreeNode("내 PC") { Name = "내 PC", ImageKey = "computer", SelectedImageKey = "computer" });
            sendServerEventHandler.LoadSubDir("root", "firstload");
            Icon iconComputer = GetIcon(104);
            if (!mainImagelist.Images.ContainsKey("computer"))
            {
                mainImagelist.Images.Add("computer", iconComputer);
            }
            focusTreenode = mainTreeview.Nodes[0];//색칠한곳 위치 저장
            mainTreeview.Nodes[0].BackColor = System.Drawing.SystemColors.Highlight;
            mainTreeview.Nodes[0].ForeColor = System.Drawing.Color.White;
            mainTreeview.SelectedNode = mainTreeview.Nodes[0];//root선택
            mainTreeview.ImageList = mainImagelist;
            SelectNode(mainTreeview.Nodes[0]);
        }
        public void SetTreeView()
        {
            int dataLength = (int)currentData[0];
            string path = ((string)currentData[3]).Split('|')[2].Split('/')[0];
            SetTreenodelist(path); // 경로에 위치하는 treenode 생성
            for (int dataNum = 3; dataNum < dataLength + 3; dataNum++)
            {
                string msg = (string)currentData[dataNum];
                string[] msgs = msg.Split('|');
                string[] infos = msgs[2].Split('/');
                if (msgs[1].Equals("exist") && setTreenode.Nodes.Count > 0)
                {
                    setTreenode.Nodes.Clear();
                }
                switch (msgs[0])
                {
                    case "dir":
                        SetTreeViewItemInfo(false, msgs[0], msgs[1], infos);
                        break;
                    case "drive":
                        SetTreeViewItemInfo(true, msgs[0], msgs[1], infos);
                        break;
                }
            }
        }
        public void SetTreenodelist(string path)
        {
            if (mainTreeview.InvokeRequired)
            {
                mainTreeview.Invoke((MethodInvoker)delegate {
                    SetTreenodelist(path);
                });
                return;
            }
            treeNodeitems.Clear();
            setTreenode = mainTreeview.Nodes[0];
            string[] staticPaths = path.Split('\\'); //경로 분석
            string currentdirpath = "";
            if (!path.Equals("root"))//폴더
            {
                foreach (string dirName in staticPaths)///경로 추척
                {
                    if (dirName.Equals("")) break;

                    currentdirpath += dirName + '\\';
                    if (!setTreenode.Nodes.ContainsKey(dirName))//경로가 없으면 추가
                    {
                        setTreenode.Nodes.Add(new TreeNode(dirName) { Name = dirName, ImageKey = "dir", SelectedImageKey = "dir" });
                    }
                    setTreenode = setTreenode.Nodes[dirName];
                }
            }
            /*
            foreach(TreeNode node in setTreenode.Nodes)
            {
                treeNodeitems.Add(node);
            }
            */
        }
        public void SetTreeViewItemInfo(bool isDrive, string type, string state, string[] infos)
        {
            if (mainTreeview.InvokeRequired)
            {
                mainTreeview.Invoke((MethodInvoker)delegate {
                    SetTreeViewItemInfo(isDrive, type, state, infos);
                });
                return;
            }
            //TreeNode currentNode = new TreeNode();
            //currentNode = mainTreeview.Nodes[0];
            if (state.Equals("disable"))
            {
                return;
            }
            string[] staticPaths = infos[0].Split('\\'); //경로 분석
            string currentdirpath = "";
            string path = infos[0];
            string itemName = infos[1];
            if (!isDrive) //폴더
            {
                switch (state)
                {
                    case "delete"://트리에서 삭제
                        if (setTreenode.Nodes.ContainsKey(itemName)) //트리에 있으면
                        {
                            //isChange = true;
                            //treeNodeitems.RemoveAt(mainTreeview.Nodes.IndexOfKey(itemName));
                            setTreenode.Nodes.RemoveAt(setTreenode.Nodes.IndexOfKey(itemName));
                        }
                        break;
                    case "exist"://트리에 추가
                    case "add"://트리에 추가
                        if (!setTreenode.Nodes.ContainsKey(itemName)) //트리에 있으면
                        {
                            isAdd = true;
                            //currentdirpath += itemName + '\\';
                            TreeNode items = new TreeNode(itemName) { Name = itemName, ImageKey = "dir", SelectedImageKey = "dir" };
                            /*if (infos[2].Equals("have"))
                            {
                                items.Nodes.Add("");
                            }*/
                            items.Nodes.Add("");
                            treeNodeitems.Add(items);
                            //setTreenode.Nodes.Add(new TreeNode(itemName) { Name = itemName, ImageKey = "dir", SelectedImageKey = "dir" });
                        }
                        break;
                }
            }
            else //드라이브
            {
                if (!setTreenode.Nodes.ContainsKey(itemName.Split('\\').First()))
                {
                    isAdd = true;
                    TreeNode items = new TreeNode(itemName.Split('\\').First()) { Name = itemName.Split('\\').First(), ImageKey = "drive", SelectedImageKey = "drive" };
                    if (infos[3].Equals("have"))
                    {
                        items.Nodes.Add("");
                    }
                    treeNodeitems.Add(items);
                    //setTreenode.Nodes.Add(new TreeNode(itemName.Split('\\').First()) { Name = itemName.Split('\\').First(), ImageKey = "drive", SelectedImageKey = "drive" });
                }
            }
        }

        public void setFocusTreeview()//색칠 최신화
        {
            if (mainTreeview.InvokeRequired)
            {
                mainTreeview.Invoke((MethodInvoker)delegate {
                    setFocusTreeview();
                });
                return;
            }
            string dirPath = currentStaticpath;
            string[] dirPaths = dirPath.Split('\\');
            TreeNode currentNode = new TreeNode();
            currentNode = mainTreeview.Nodes[0];

            //focusTreenode였던곳 색칠 지움
            focusTreenode.BackColor = System.Drawing.Color.White;
            focusTreenode.ForeColor = System.Drawing.Color.Black;

            if (!dirPath.Equals("root"))
            {
                foreach (string dirName in dirPaths)
                {
                    if (dirName.Equals("")) break;
                    if (currentNode.Nodes.Count != 0 && currentNode.IsExpanded)
                    {
                        currentNode = currentNode.Nodes[dirName];
                    }
                    else
                    {
                        break;
                    }
                }
                mainTreeview.SelectedNode = currentNode;
                currentNode.BackColor = System.Drawing.SystemColors.Highlight;
                currentNode.ForeColor = System.Drawing.Color.White;
                focusTreenode = currentNode;
                //focusTreenode = currentNode;
            }
            else
            {
                mainTreeview.SelectedNode = mainTreeview.Nodes[0];
                mainTreeview.Nodes[0].BackColor = System.Drawing.SystemColors.Highlight;
                mainTreeview.Nodes[0].ForeColor = System.Drawing.Color.White;
                focusTreenode = mainTreeview.Nodes[0];
            }
        }
        public void CollapseTreeView(TreeNode Node)
        {
            TreeNode collapseNode = Node;
            collapseTreenode = Node;
            if (collapseNode.ImageKey.Equals("dirOpen"))
            {
                collapseNode.ImageKey = "dir";
                collapseNode.SelectedImageKey = "dir";
            }
            //Node.Collapse();
            setFocusTreeview();//접을때 하이라이트 위치 변경
        }
        public void ExpandTreeView(TreeNode Node)
        {
            TreeNode expandedNode = Node;
            if (expandedNode.Nodes[0].Name == "")
            {
                expandedNode.Nodes.RemoveAt(0);
            }
            if (expandedNode.ImageKey.Equals("dir"))
            {
                expandedNode.ImageKey = "dirOpen";
                expandedNode.SelectedImageKey = "dirOpen";
            }
            string dirpath = "";
            while (true)
            {
                if (expandedNode.Text.Equals("내 PC"))
                {
                    dirpath = "root";
                    break;
                }
                if (!expandedNode.Parent.Text.Equals("내 PC"))
                {
                    dirpath = expandedNode.Name + "\\" + dirpath;
                    expandedNode = expandedNode.Parent;
                }
                else
                {
                    dirpath = expandedNode.Name + "\\" + dirpath;
                    break;
                }
            }
            sendServerEventHandler.LoadSubDir(dirpath, "treeviewafterselect");//확장할때 하위 폴더 불러옴
            //Node.Expand();
            setFocusTreeview();
        }
        public void SelectNode(TreeNode Node)
        {
            TreeNode currentNode = Node;
            focusTreenode.BackColor = System.Drawing.Color.White;
            focusTreenode.ForeColor = System.Drawing.Color.Black;
            currentNode.BackColor = System.Drawing.SystemColors.Highlight;
            currentNode.ForeColor = System.Drawing.Color.White;
            focusTreenode = currentNode;
            string dirpath = "";
            while (true)
            {
                if (currentNode.Text.Equals("내 PC"))
                {
                    dirpath = "root";
                    break;
                }
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
            MoveDir(true, dirpath, "treeviewafterselect");
            setFocusTreeview();
        }
    }
}

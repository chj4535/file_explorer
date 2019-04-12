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
        static bool isChange;
        static TreeView mainTreeview = new TreeView();
        static TreeNode focusTreenode = new TreeNode();
        static ImageList mainImagelist = new ImageList();

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
            isChange = false;
            SetTreeView();
            if (isChange)
            {
                mainTreeview.BeginUpdate();
                TreeNode[] treeNodeitem = treeNodeitems.ToArray();
                setTreenode.Nodes.AddRange(treeNodeitem);
                mainTreeview.EndUpdate();
            }
        }

        public void TreeViewHandlerSetting(TreeView mainFormtreeview, ImageList mainFormimagelist)
        {
            mainImagelist = mainFormimagelist;
            mainTreeview = mainFormtreeview;
            mainTreeview.Nodes.Add(new TreeNode("내 PC") { Name = "내 PC", ImageKey = "computer", SelectedImageKey = "computer" });
            Icon iconComputer = GetIcon(104);
            if (!mainImagelist.Images.ContainsKey("computer"))
            {
                mainImagelist.Images.Add("computer", iconComputer);
            }
            mainTreeview.Nodes[0].BackColor = System.Drawing.SystemColors.Highlight;
            mainTreeview.Nodes[0].ForeColor = System.Drawing.Color.White;
            focusTreenode = mainTreeview.Nodes[0];
            mainTreeview.ImageList = mainImagelist;
        }
        public void SetTreeView()
        {
            int dataLength = (int)currentData[0];
            string path = ((string)currentData[2]).Split('|')[2].Split('/')[0];
            SetTreenodelist(path); // 경로에 위치하는 treenode 생성
            for (int dataNum = 2; dataNum < dataLength + 2; dataNum++)
            {
                string msg = (string)currentData[dataNum];
                string[] msgs = msg.Split('|');
                string[] infos = msgs[2].Split('/');
                if (currentStaticpath.Equals(infos[0]))
                {
                    switch (msgs[0])
                    {
                        case "dir":
                            SetTreeViewItemInfo(false, msgs[0], msgs[1], infos);
                            break;
                        case "drive":
                            SetTreeViewItemInfo(true,msgs[0], msgs[1], infos);
                            break;
                    }
                }
            }
            setFocusTreeview();
            
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
            foreach(TreeNode node in setTreenode.Nodes)
            {
                treeNodeitems.Add(node);
            }
        }
        public void SetTreeViewItemInfo(bool isDrive,string type, string state, string[] infos)
        {
            if (mainTreeview.InvokeRequired)
            {
                mainTreeview.Invoke((MethodInvoker)delegate {
                    SetTreeViewItemInfo(isDrive,type,state,infos);
                });
                return;
            }
            //TreeNode currentNode = new TreeNode();
            //currentNode = mainTreeview.Nodes[0];
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
                            isChange = true;
                            treeNodeitems.RemoveAt(mainTreeview.Nodes.IndexOfKey(itemName));
                            //setTreenode.Nodes.RemoveAt(mainTreeview.Nodes.IndexOfKey(itemName));
                        }
                        break;
                    case "exist"://트리에 추가
                        if (!setTreenode.Nodes.ContainsKey(itemName)) //트리에 있으면
                        {
                            isChange = true;
                            //currentdirpath += itemName + '\\';
                            treeNodeitems.Add(new TreeNode(itemName) { Name = itemName, ImageKey = "dir", SelectedImageKey = "dir" });
                            //setTreenode.Nodes.Add(new TreeNode(itemName) { Name = itemName, ImageKey = "dir", SelectedImageKey = "dir" });
                        }
                        break;
                }
                /*
                foreach (TreeNode dirNode in currentNode.Nodes)//새로 불러왔을때 삭제된 폴더가 있으면 트리에서 삭제
                {
                    int pos = Array.IndexOf(currentStateitemsname, dirNode.Name);
                    if (pos < 0)
                    {
                        dirNode.Remove();
                    }
                }
                foreach (string dirName in currentStateitemsname)//새로 불러왔을때 없는 폴더가 있으면 추가
                {
                    if (!currentNode.Nodes.ContainsKey(dirName))
                    {
                        currentNode.Nodes.Add(new TreeNode(dirName) { Name = dirName, ImageKey = currentStaticpath + dirName+'\\', SelectedImageKey = currentStaticpath + dirName + '\\' });
                    }
                }*/
            }
            else //드라이브
            {
                if (!setTreenode.Nodes.ContainsKey(itemName.Split('\\').First()))
                {
                    isChange = true;
                    treeNodeitems.Add(new TreeNode(itemName.Split('\\').First()) { Name = itemName.Split('\\').First(), ImageKey = "drive", SelectedImageKey = "drive" });
                    //setTreenode.Nodes.Add(new TreeNode(itemName.Split('\\').First()) { Name = itemName.Split('\\').First(), ImageKey = "drive", SelectedImageKey = "drive" });
                }
            }
        }

        public void setFocusTreeview()
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
                currentNode.BackColor = System.Drawing.SystemColors.Highlight;
                currentNode.ForeColor = System.Drawing.Color.White;
                focusTreenode = currentNode;
            }
            else
            {
                focusTreenode = mainTreeview.Nodes[0];
                mainTreeview.Nodes[0].BackColor = System.Drawing.SystemColors.Highlight;
                mainTreeview.Nodes[0].ForeColor = System.Drawing.Color.White;
            }
        }

        public void SelectNode()
        {
            TreeNode currentNode = mainTreeview.SelectedNode;
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
            isClick = true;
            currentStaticpath = dirpath;
            sendServerEventHandler.MoveDir(dirpath,  "treeviewafterselect");
            setFocusTreeview();
        }
    }
}

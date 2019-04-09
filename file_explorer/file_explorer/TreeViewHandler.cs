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
    class TreeViewHandler
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

        static string mainFormcurrentpath = "";
        SendServerEventHandler sendServerEventHandler = new SendServerEventHandler();
        static TreeView mainTreeview = new TreeView();
        static TreeNode focusTreenode = new TreeNode();
        static LoadDriveInfo loadDriveInfo = new LoadDriveInfo();
        static LoadDirSubItemsInfo loadDirsubitemsinfo = new LoadDirSubItemsInfo();
        static ImageList mainImagelist = new ImageList();
        public TreeViewHandler()
        {
        }

        public void TreeViewHandlerSetting(TreeView mainFormtreeview, ImageList mainFormimagelist)
        {
            mainImagelist = mainFormimagelist;
            mainTreeview = mainFormtreeview;
            mainTreeview.Nodes.Add(new TreeNode("내 PC") { Name = "내 PC", ImageKey = "root", SelectedImageKey = "root" });
            mainFormcurrentpath = "내 PC";
            Icon iconComputer = GetIcon(104);
            if (!mainImagelist.Images.ContainsKey("root"))
            {
                // If not, add the image to the image list.
                mainImagelist.Images.Add("root", iconComputer);
            }
            mainTreeview.Nodes[0].BackColor = System.Drawing.SystemColors.Highlight;
            mainTreeview.Nodes[0].ForeColor = System.Drawing.Color.White;
            focusTreenode = mainTreeview.Nodes[0];
            loadDriveInfo.SetDriveInfotoTreeviewEvent(SetTreeView);
            loadDirsubitemsinfo.SetSubItemToTreeviewEvnet(SetTreeView);
            mainTreeview.ImageList = mainImagelist;
        }
        public void setMainformcurrentpath(string path)
        {
            mainFormcurrentpath = path;
        }
        public void SetTreeView(int msgCount, string staticPath, string[] dirPaths)
        {
            if (mainTreeview.InvokeRequired)
            {
                mainTreeview.Invoke((MethodInvoker)delegate {
                    SetTreeView(msgCount, staticPath, dirPaths);
                });
                return;
            }
            TreeNode currentNode = new TreeNode();
            currentNode = mainTreeview.Nodes[0];
            string[] staticPaths = staticPath.Split('\\');
            string currentdirpath = "";
            foreach (string dirName in staticPaths)//현재 온 폴더들의 공통 경로
            {
                if (dirName.Equals("")) break;

                if (!currentNode.Nodes.ContainsKey(dirName))
                {
                    currentNode.Nodes.Add(new TreeNode(dirName) { Name = dirName, ImageKey = currentdirpath + dirName, SelectedImageKey = currentdirpath + dirName });
                }
                currentNode = currentNode.Nodes[dirName];
                currentdirpath += dirName + "\\";
            }
            foreach (TreeNode dirNode in currentNode.Nodes)//새로 불러왔을때 삭제된 폴더가 있으면 트리에서 삭제
            {
                int pos = Array.IndexOf(dirPaths, dirNode.Name);
                if (pos < 0)
                {
                    dirNode.Remove();
                }
            }
            foreach (string dirName in dirPaths)//새로 불러왔을때 없는 폴더가 있으면 추가
            {
                if (!currentNode.Nodes.ContainsKey(dirName))
                {
                    currentNode.Nodes.Add(new TreeNode(dirName) { Name = dirName, ImageKey = currentdirpath + dirName, SelectedImageKey = currentdirpath + dirName });
                }
            }
            setFocusTreeview();
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
            string dirPath = sendServerEventHandler.GetMainFormPath();
            string[] dirPaths = dirPath.Split('\\');
            TreeNode currentNode = new TreeNode();
            currentNode = mainTreeview.Nodes[0];
            focusTreenode.BackColor = System.Drawing.Color.White;
            focusTreenode.ForeColor = System.Drawing.Color.Black;
            if (!dirPath.Equals(""))
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
    }
}

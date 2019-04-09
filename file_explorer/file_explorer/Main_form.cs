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
        TreeViewHandler treeViewhandler = new TreeViewHandler();
        ListViewHandler listViewhandler = new ListViewHandler();
        PathHandler comboBoxhandler = new PathHandler();
        SendServerEventHandler sendServerEventHandler = new SendServerEventHandler();
        ErrorHandler errorHandler = new ErrorHandler();
        Graphics mainFormgraphic;
        ListViewSorter Sorter = new ListViewSorter();
        int listViewcount;
        public Main_form(string loginformUserId)
        {
            listViewcount = 0;
            InitializeComponent();
            userId = loginformUserId;
            this.mainFormrecentcombobox.DisplayMember = "Text";
            this.mainFormrecentcombobox.ValueMember = "Value";
            this.mainFormcombobox.DroppedDown = false;
            errorHandler.SetSendtoMainFormErrorEvent(MainFormErrorEvent);
            treeViewhandler.TreeViewHandlerSetting(mainFormtreeview, mainFormimagelist);
            listViewhandler.ListViewHandlerSetting(mainFormlistview, mainFormimagelist);
            mainFormgraphic = this.CreateGraphics();
            comboBoxhandler.PathHandlerSetting(mainFormpathbutton, mainFormcombobox,mainFormrecentcombobox, mainFormgraphic);
            sendServerEventHandler.SendServerEventHandlerSetting(backButton, nextButton,upperButton);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user_name_label.Text = userId;
            mainFormlistview.SmallImageList = mainFormimagelist;
            backButton.Enabled = false;
            nextButton.Enabled = false;
            upperButton.Enabled = false;
            backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            backButton.ForeColor = System.Drawing.Color.Gray;
            nextButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            nextButton.ForeColor = System.Drawing.Color.Gray;
            sendServerEventHandler.MoveDir("root", true,"form_load");

        }
        private void ListViewDoubleClick(object sender, MouseEventArgs e) // 리스트뷰 더블클릭 시(폴더만) 해당 폴더로 이동
        {
            //ListViewItem item = sender as ListViewItem;
            //MessageBox.Show(mainFormlistview.SelectedItems[0].Name);
            string itemName = mainFormlistview.SelectedItems[0].Name;
            string[] itemNames = itemName.Split('|');
            if (itemNames[0].Equals("dir"))
            {
                sendServerEventHandler.MoveDir(itemNames[1], true,"listviewdoubleclick");
            }
        }
        private void backButton_Click(object sender, EventArgs e) //뒤로가기
        {
            Stack<string> nextPathsave = sendServerEventHandler.attributeNextpathsave;
            Stack<string> prePathsave = sendServerEventHandler.attributePrepathsave;
            nextPathsave.Push(prePathsave.Pop()); //현재 경로를 다음스택에 저장해놓음
            if (prePathsave.Count == 1)
            {
                backButton.Enabled = false;
                backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
                backButton.ForeColor = System.Drawing.Color.Gray;
            }
            nextButton.Enabled = true;
            nextButton.BackColor = System.Drawing.SystemColors.Window;
            nextButton.ForeColor = System.Drawing.Color.Black;
            sendServerEventHandler.attributePrepathsave = prePathsave;
            sendServerEventHandler.attributeNextpathsave = nextPathsave;
            sendServerEventHandler.MoveDir(prePathsave.Peek(), false,"backbutton");
        }

        private void nextButton_Click(object sender, EventArgs e) //앞으로? 가기
        {
            Stack<string> nextPathsave = sendServerEventHandler.attributeNextpathsave;
            Stack<string> prePathsave = sendServerEventHandler.attributePrepathsave;
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
            sendServerEventHandler.attributePrepathsave = prePathsave;
            sendServerEventHandler.attributeNextpathsave = nextPathsave;
            sendServerEventHandler.MoveDir(prePathsave.Peek(), false,"nextbutton");
        }

        private void upperButton_Click(object sender, EventArgs e) // 상위폴더로 이동
        {
            string currentPath = sendServerEventHandler.GetMainFormPath();
            if (currentPath.Split('\\').Last().Equals(""))
            {
                currentPath = "root";
            }
            else
            {
                if (currentPath.Split('\\').Length == 2)
                {
                    currentPath = currentPath.Split('\\').First() + "\\";
                }
                else
                {
                    currentPath = currentPath.Substring(0, currentPath.LastIndexOf("\\"));
                }
                
            }
            sendServerEventHandler.MoveDir(currentPath, true,"upperbutton");
        }

        private void mainFormtreeview_AfterSelect(object sender, TreeViewEventArgs e) //트리상의 폴더 클릭시 이동
        {
            TreeNode currentNode = mainFormtreeview.SelectedNode;
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
            if (dirpath.Split('\\').Length==2)
            {
                sendServerEventHandler.MoveDir(dirpath, true, "treeviewafterselect");
            }
            else
            {
                
                sendServerEventHandler.MoveDir(dirpath.Substring(0, dirpath.LastIndexOf("\\")), true, "treeviewafterselect");
            }
            treeViewhandler.setFocusTreeview();
        }

        private void mainFormtreeview_AfterExpand(object sender, TreeViewEventArgs e)
        {
            treeViewhandler.setFocusTreeview();
        }

        private void mainFormtreeview_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            treeViewhandler.setFocusTreeview();
        }

        private void mainFormpathbutton_Click(object sender, EventArgs e)
        {
            this.mainFormpathbutton.Hide();
            this.mainFormcombobox.Focus();
        }

        private void mainFormcombobox_Leave(object sender, EventArgs e)
        {
            this.mainFormpathbutton.Show();
        }

        private void mainFormcombobox_DropDown(object sender, EventArgs e)
        {
            this.mainFormpathbutton.Hide();
            this.mainFormcombobox.Focus();
        }

        private void mainFormcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currentPath = sendServerEventHandler.GetMainFormPath();
            if (!currentPath.Equals(mainFormcombobox.SelectedItem.ToString()))
            {
                string selectedPath = mainFormcombobox.SelectedItem.ToString();
                sendServerEventHandler.MoveDir(selectedPath, true, "comboindexchange");
            }
        }

        private void mainFormrecentcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPath = (mainFormrecentcombobox.SelectedItem as dynamic).Value;
            sendServerEventHandler.MoveDir(selectedPath, true,"recentcomboindexchange");
        }

        private void mainFormcombobox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string writePath = mainFormcombobox.Text;
                sendServerEventHandler.MoveDir(writePath, true,"comboenter");
                this.mainFormpathbutton.Show();
            }
        }

        public void MainFormErrorEvent(int msgCount,string errorMessage)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate {
                    MainFormErrorEvent(msgCount, errorMessage);
                });
                return;
            }
            Stack<string> prePathsave = sendServerEventHandler.attributePrepathsave;
            string errPath = prePathsave.Pop();
            comboBoxhandler.SetErrorPath(errPath);
            if (prePathsave.Count == 1)
            {
                backButton.Enabled = false;
                backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
                backButton.ForeColor = System.Drawing.Color.Gray;
            }
            sendServerEventHandler.attributePrepathsave = prePathsave;
            sendServerEventHandler.MoveDir(prePathsave.Peek(), false,"error");
            MessageBox.Show(Form.ActiveForm,errorMessage, "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void mainFormlistview_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            mainFormlistview.ListViewItemSorter = Sorter;
            if (!(mainFormlistview.ListViewItemSorter is ListViewSorter))
                return;
            Sorter = (ListViewSorter)mainFormlistview.ListViewItemSorter;

            if (Sorter.LastSort == e.Column)
            {
                if (mainFormlistview.Sorting == SortOrder.Ascending)
                    mainFormlistview.Sorting = SortOrder.Descending;
                else
                    mainFormlistview.Sorting = SortOrder.Ascending;
            }
            else
            {
                mainFormlistview.Sorting = SortOrder.Descending;
            }
            Sorter.ByColumn = e.Column;

            mainFormlistview.Sort();
        }

        
        private void mainFormlistview_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var items = new List<ListViewItem>();
            // create array or collection for all selected items
            // add dragged one first
            items.Add((ListViewItem)e.Item);
            // optionally add the other selected ones
            foreach (ListViewItem mainFormitem in mainFormlistview.SelectedItems)
            {
                if (!items.Contains(mainFormitem))
                {
                    items.Add(mainFormitem);
                }
            }
            // pass the items to move...
            //privateDrag = true;
            mainFormlistview.DoDragDrop(items, DragDropEffects.Move);
            //privateDrag = false;
        }


        private void mainFormlistview_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void mainFormlistview_DragOver(object sender, DragEventArgs e)
        {
            var items = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));
            var pos = mainFormlistview.PointToClient(new Point(e.X, e.Y));
            var hit = mainFormlistview.HitTest(pos);
            //Console.WriteLine(hit.)
            //string[] itemNames = itemName.Split('|');
            //if (itemNames[0].Equals("dir"))
            if (hit.Item != null && hit.Item.Name.Split('|').First().Equals("dir") && !items.Contains(hit.Item))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void mainFormlistview_DragDrop(object sender, DragEventArgs e)
        {
            var items = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));
            var pos = mainFormlistview.PointToClient(new Point(e.X, e.Y));
            var hit = mainFormlistview.HitTest(pos);
            if (hit.Item != null && hit.Item.Name.Split('|').First().Equals("dir") && !items.Contains(hit.Item))
            {
                string targetPath = hit.Item.Name.Split('|').Last();
                string firstItempath = items[0].Name.Split('|').Last();
                string dragStaticpath = firstItempath.Substring(0, firstItempath.LastIndexOf('\\'));
                //string dragStaticpath = dirsInfos[1].Substring(0, dirsInfos[1].LastIndexOf("\\"));
                string[] itemNames = new string[items.Count()];
                int count = 0;
                foreach(ListViewItem item in items)
                {
                    string itemType = item.Name.Split('|').First();
                    string itemPath = item.Name.Split('|').Last();
                    string itemName = itemPath.Split('\\').Last();
                    itemNames[count++] = itemType+"/"+itemName;
                }
                sendServerEventHandler.MoveItemsToDir(targetPath, dragStaticpath, itemNames, "dnd_listviewtolistview");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendServerEventHandler.reload();
        }

        private void mainFormlistview_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (backButton.Enabled.Equals(true))
                {
                    this.backButton_Click(this, null);
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // 전체 단축키
        {
            if (keyData == (Keys.F5))
            {
                sendServerEventHandler.reload();
            }

            if (keyData == (Keys.Alt | Keys.Left))
            {
                if (backButton.Enabled.Equals(true))
                {
                    this.backButton_Click(this, null);
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.Right))
            {
                if (nextButton.Enabled.Equals(true))
                {
                    this.nextButton_Click(this, null);
                }
                return true;
            }

            return true;
        }
    }

    public class ListViewSorter : System.Collections.IComparer
    {
        public int Compare(object o1, object o2)
        {
            if (!(o1 is ListViewItem))
                return (0);
            if (!(o2 is ListViewItem))
                return (0);

            ListViewItem lvi1 = (ListViewItem)o2;
            ListViewItem lvi2 = (ListViewItem)o1;
            string str1;
            string str2;
            if (ByColumn == 0)
            {
                str1 = lvi1.Name;
                str2 = lvi2.Name;
            }
            else
            {
                str1 = lvi1.SubItems[ByColumn].Text;
                str2 = lvi2.SubItems[ByColumn].Text;
            }
            int result;
            if (lvi1.ListView.Sorting == SortOrder.Ascending)
                result = String.Compare(str1, str2);
            else
                result = String.Compare(str2, str1);

            LastSort = ByColumn;
            return (result);
        }
        
        public int ByColumn
        {
            get { return Column; }
            set { Column = value; }
        }
        int Column = 0;

        public int LastSort
        {
            get { return LastColumn; }
            set { LastColumn = value; }
        }
        int LastColumn = 0;
    }
}

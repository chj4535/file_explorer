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
        Graphics mainFormgraphic;
        int listViewcount;
        public Main_form(string loginformUserId)
        {
            listViewcount = 0;
            InitializeComponent();
            userId = loginformUserId;
            this.mainFormrecentcombobox.DisplayMember = "Text";
            this.mainFormrecentcombobox.ValueMember = "Value";
            this.mainFormcombobox.DroppedDown = false;
            treeViewhandler.TreeViewHandlerSetting(mainFormtreeview, mainFormimagelist);
            listViewhandler.ListViewHandlerSetting(mainFormlistview, mainFormimagelist);
            mainFormgraphic = this.CreateGraphics();
            comboBoxhandler.PathHandlerSetting(mainFormpathbutton, mainFormcombobox,mainFormrecentcombobox, mainFormgraphic);
            sendServerEventHandler.SendServerEventHandlerSetting(backButton, nextButton);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user_name_label.Text = userId;
            mainFormlistview.SmallImageList = mainFormimagelist;
            backButton.Enabled = false;
            nextButton.Enabled = false;
            backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            backButton.ForeColor = System.Drawing.Color.Gray;
            nextButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            nextButton.ForeColor = System.Drawing.Color.Gray;
            sendServerEventHandler.MoveDir("root", true);

        }

        private void ListViewDoubleClick(object sender, MouseEventArgs e) // 리스트뷰 더블클릭 시(폴더만) 해당 폴더로 이동
        {
            //ListViewItem item = sender as ListViewItem;
            //MessageBox.Show(mainFormlistview.SelectedItems[0].Name);
            string itemName = mainFormlistview.SelectedItems[0].Name;
            string[] itemNames = itemName.Split('|');
            if (itemNames[0].Equals("dir"))
            {
                sendServerEventHandler.MoveDir(itemNames[1], true);
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
            sendServerEventHandler.MoveDir(prePathsave.Peek(), false);
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
            sendServerEventHandler.MoveDir(prePathsave.Peek(), false);
        }

        private void upperButton_Click(object sender, EventArgs e) // 상위폴더로 이동
        {
            Stack<string> nextPathsave = sendServerEventHandler.attributeNextpathsave;
            Stack<string> prePathsave = sendServerEventHandler.attributePrepathsave;
            if (prePathsave.Count < 2)//바탕화면으로 이동해야하는데 미구현
            {
                upperButton.Enabled = false;
            }
            else
            {
                sendServerEventHandler.MoveDir(prePathsave.ElementAt(1), true);
            }
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
            sendServerEventHandler.MoveDir(dirpath, true);
            treeViewhandler.setFocusTreeview();
        }

        private void mainFormtreeview_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeNode currentNode = e.Node;
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
            treeViewhandler.setFocusTreeview();
            //sendServerEventHandler.MoveDir(dirpath, true);
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

        private void mainFormcombobox_Click(object sender, EventArgs e)
        {
            //this.mainFormpathbutton.Hide();
            //this.mainFormcombobox.Focus();
        }

        private void mainFormcombobox_DropDown(object sender, EventArgs e)
        {
            this.mainFormpathbutton.Hide();
            this.mainFormcombobox.Focus();
        }

        private void mainFormcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPath = mainFormcombobox.SelectedItem.ToString();
            sendServerEventHandler.MoveDir(selectedPath, true);
        }

        private void mainFormrecentcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPath = (mainFormrecentcombobox.SelectedItem as dynamic).Value;
            sendServerEventHandler.MoveDir(selectedPath, true);
        }
    }
}

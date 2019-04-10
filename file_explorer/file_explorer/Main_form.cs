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
        string userId;
        TreeViewHandler treeViewhandler = new TreeViewHandler();
        ListViewHandler listViewhandler = new ListViewHandler();
        PathListHandler pathListhandler = new PathListHandler();
        ButtonHandler buttonHandler = new ButtonHandler();
        Graphics mainFormgraphic;
        ListViewSorter Sorter = new ListViewSorter();
        //int listViewcount;
        public Main_form(string loginformUserId)
        {
            listViewhandler.SetUpdateEvent(UpdateForm);
            InitializeComponent();
            userId = loginformUserId;
            this.mainFormrecentcombobox.DisplayMember = "Text";
            this.mainFormrecentcombobox.ValueMember = "Value";
            //this.mainFormcombobox.DroppedDown = false;
            treeViewhandler.TreeViewHandlerSetting(mainFormtreeview, mainFormimagelist);
            listViewhandler.ListViewHandlerSetting(mainFormlistview, mainFormimagelist,mainFormitemscount,mainFormselectedinfo);
            mainFormgraphic = this.CreateGraphics();
            pathListhandler.PathHandlerSetting(mainFormpathbutton, mainFormcombobox,mainFormrecentcombobox, mainFormgraphic);
            buttonHandler.ButtonHandlerSetting(backButton, nextButton, upperButton);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user_name_label.Text = userId;
            listViewhandler.FirstLoad();

        }

        public void UpdateForm()
        {
            if (listViewhandler.GetErrorstate()) // 에러상태!
            {
                MainFormErrorEvent(listViewhandler.GetErrormessage());
            }
            else
            {
                listViewhandler.Update();
                treeViewhandler.Update();
                pathListhandler.Update();
                buttonHandler.Update();
            }
        }

        public void MainFormErrorEvent(string errorMessage)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate {
                    MainFormErrorEvent(errorMessage);
                });
                return;
            }
            /*
            Stack<string> prePathsave = sendServerEventHandler.attributePrepathsave;
            string errPath = prePathsave.Pop();
            //comboBoxhandler.SetErrorPath(errPath);
            if (prePathsave.Count == 1)
            {
                backButton.Enabled = false;
                backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
                backButton.ForeColor = System.Drawing.Color.Gray;
            }
            sendServerEventHandler.attributePrepathsave = prePathsave;
            sendServerEventHandler.MoveDir(prePathsave.Peek(), false, "error");
            */
            MessageBox.Show(Form.ActiveForm, errorMessage, "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ListViewDoubleClick(object sender, MouseEventArgs e) // 리스트뷰 더블클릭 시(폴더만) 해당 폴더로 이동
        {
            listViewhandler.ItemDoubleClick();
        }
        private void backButton_Click(object sender, EventArgs e) //뒤로가기
        {
            buttonHandler.backButtonclick();
        }

        private void nextButton_Click(object sender, EventArgs e) //앞으로? 가기
        {
            buttonHandler.nextButtonclick();
        }

        private void upperButton_Click(object sender, EventArgs e) // 상위폴더로 이동
        {
            buttonHandler.upperButtonclick();
        }

        private void mainFormtreeview_AfterSelect(object sender, TreeViewEventArgs e) //트리상의 폴더 클릭시 이동
        {
            treeViewhandler.SelectNode();
        }

        private void mainFormtreeview_AfterExpand(object sender, TreeViewEventArgs e)
        {
            treeViewhandler.setFocusTreeview();//펼쳤을때 하이라이트 위치 변경
        }

        private void mainFormtreeview_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            treeViewhandler.setFocusTreeview();//접을때 하이라이트 위치 변경
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
            pathListhandler.MainPathSelectedChange();
        }

        private void mainFormrecentcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pathListhandler.MainRecentPathSelectedIndexChanged();
        }

        private void mainFormcombobox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                pathListhandler.MainComboBoxEnter();
            }
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
            items.Add((ListViewItem)e.Item);
            foreach (ListViewItem mainFormitem in mainFormlistview.SelectedItems)
            {
                if (!items.Contains(mainFormitem))
                {
                    items.Add(mainFormitem);
                }
            }
            mainFormlistview.DoDragDrop(items, DragDropEffects.Move);
            
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
                string[] itemNames = new string[items.Count()];
                int count = 0;
                foreach(ListViewItem item in items)
                {
                    string itemType = item.Name.Split('|').First();
                    string itemPath = item.Name.Split('|').Last();
                    string itemName = itemPath.Split('\\').Last();
                    itemNames[count++] = itemType+"/"+itemName;
                }
                listViewhandler.MainListViewDragDrop(targetPath, dragStaticpath, itemNames, "dnd_listviewtolistview");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonHandler.resetbutton();
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
                buttonHandler.resetbutton();
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

    public class ListViewSorter : System.Collections.IComparer//listview 정렬 하기 위한 클래스
    {
        public int Compare(object listitem1, object listitem2)
        {
            if (!(listitem1 is ListViewItem))
                return (0);
            if (!(listitem2 is ListViewItem))
                return (0);

            ListViewItem changeItem1 = (ListViewItem)listitem2;
            ListViewItem changeItem2 = (ListViewItem)listitem1;
            string str1;
            string str2;
            if (ByColumn == 0) //처음 열(이름) 부분은 앞에 dir_ / file_ 이 숨겨져 있기 때문에 Name으로 정렬한다.
            {
                str1 = changeItem1.Name;
                str2 = changeItem2.Name;
            }
            else
            {
                str1 = changeItem1.SubItems[ByColumn].Text;
                str2 = changeItem2.SubItems[ByColumn].Text;
            }
            int result;
            if (changeItem1.ListView.Sorting == SortOrder.Ascending)
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

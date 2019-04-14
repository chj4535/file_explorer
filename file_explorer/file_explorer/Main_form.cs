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
            listViewhandler.SetUpdateEvent(UpdateForm);  //메인폼을 업데이트 이벤트에 추가
            InitializeComponent();
            userId = loginformUserId;
            this.mainFormrecentcombobox.DisplayMember = "Text";
            this.mainFormrecentcombobox.ValueMember = "Value";
            mainFormgraphic = this.CreateGraphics();
            listViewhandler.ListViewHandlerSetting(mainFormlistview, mainFormimagelist, mainFormlistitemcount, mainFormselectedinfo);
            pathListhandler.PathHandlerSetting(mainFormpathbutton, mainFormcombobox, mainFormrecentcombobox, mainFormgraphic);
            buttonHandler.ButtonHandlerSetting(backButton, nextButton, upperButton);
            treeViewhandler.TreeViewHandlerSetting(mainFormtreeview);


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user_name_label.Text = userId;
            //listViewhandler.SetFirstLoad(); //최소 정보 받기

        }

        public void UpdateForm(string target)
        {
            if (mainFormlistitemcount.InvokeRequired)
            {
                mainFormlistitemcount.Invoke((MethodInvoker)delegate
                {
                    UpdateForm(target);
                });
                return;
            }
            switch (target)
            {
                case "error":
                    listViewhandler.ShowError();
                    break;
                case "all":
                    listViewhandler.Update();
                    treeViewhandler.Update();
                    pathListhandler.Update();
                    buttonHandler.Update();
                    mainFormlistitemcount.Text = mainFormlistview.Items.Count.ToString() + "개의 항목";
                    break;
                case "listView": //리스트뷰 변화
                    listViewhandler.Update();
                    treeViewhandler.setFocusTreeview();//리스트변경시 tree하이라이트 변경
                    pathListhandler.Update();
                    buttonHandler.Update();
                    mainFormlistitemcount.Text = mainFormlistview.Items.Count.ToString() + "개의 항목";
                    break;
                case "treeView": //트리뷰 변화
                    treeViewhandler.Update();
                    break;
            }
        }

        public void MainFormErrorEvent(string errorMessage)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    MainFormErrorEvent(errorMessage);
                });
                return;
            }
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

        private void mainFormlistview_ItemDrag(object sender, ItemDragEventArgs e) //드래그한내용 저장
        {
            listViewhandler.MainListViewItemDrag((ListViewItem)e.Item);
        }

        private void mainFormlistview_DragOver(object sender, DragEventArgs e)
        {
            listViewhandler.MainListViewDragOver(e);
        }

        private void mainFormlistview_DragDrop(object sender, DragEventArgs e)
        {
            listViewhandler.MainListViewDragDrop(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime refreshTime = DateTime.Now;
            buttonHandler.resetbutton(refreshTime);
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
            if (e.KeyCode == Keys.F5)
            {
                DateTime refreshTime = DateTime.Now;
                buttonHandler.resetbutton(refreshTime);
            }
            if (e.KeyCode == Keys.Delete)
            {
                listViewhandler.DeleteItem();
            }
        }

        private void mainFormtreeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var hitTest = e.Node.TreeView.HitTest(e.Location);
            if (hitTest.Location == TreeViewHitTestLocations.PlusMinus)//+ -를 눌렀음
            {
                if (e.Node.IsExpanded)//열려있으면
                    treeViewhandler.ExpandTreeView(e.Node);
                else
                    treeViewhandler.CollapseTreeView(e.Node);
            }
            else
            {
                treeViewhandler.SelectNode(e.Node);
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

        private void Main_form_Resize(object sender, EventArgs e)
        {
            //this.splitContainer1.Size = new System.Drawing.Size(864, 368);
            splitContainer1.Size = new Size(this.Width - 24, this.Height - 132);
            mainFormlistitemcount.Location = new System.Drawing.Point(14, this.Height - 57);
            button1.Location = new System.Drawing.Point(this.Width - 248, 45);
            textBox2.Location = new System.Drawing.Point(this.Width - 210, 45);
            mainFormpathbutton.Size = new Size(this.Width - 358, 23);
            mainFormcombobox.Size = new Size(this.Width - 342, 23);
        }

        private void mainFormlistview_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (mainFormlistview.FocusedItem.Bounds.Contains(e.Location))
                {
                    //열기 case
                    if (mainFormlistview.SelectedItems.Count > 1)
                    {
                        mainFormlistviewcontextmenu.Items[0].Enabled = false;
                    }
                    else
                    {
                        string itemName = mainFormlistview.SelectedItems[0].Name;
                        string[] itemNames = itemName.Split('|');
                        if (itemNames[0].Equals("dir")) //파일이 아닌 폴더(드라이브) 선택일 시
                        {
                            mainFormlistviewcontextmenu.Items[0].Enabled = true;
                        }
                        else
                        {
                            mainFormlistviewcontextmenu.Items[0].Enabled = false;
                        }
                    }
                    listViewhandler.ContextMeueItemSelect(Cursor.Position);
                    mainFormlistviewcontextmenu.Show(Cursor.Position);
                }
            }
        }

        private void mainFormlistviewcontextmenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "열기":
                    listViewhandler.ContextMeueItemOpen();
                    break;
                case "삭제":
                    listViewhandler.DeleteItem();
                    break;
                case "복사":
                    listViewhandler.copyItem();
                    break;
                case "이름 바꾸기":
                    int addXpoint = mainFormtreeview.Width;
                    listViewhandler.ContextMeueItemRename(addXpoint);
                    break;
            }
        }

        private void mainFormlistview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var mousePoint = mainFormlistview.PointToClient(Cursor.Position);
                var hit = mainFormlistview.HitTest(mousePoint);
                if (hit.Item == null) //공백 클릭
                {
                    if (listViewhandler.IsCopyItems()) // 복사한게 존재한다
                    {
                        mainFormlistviewcopycontextmenu.Show(Cursor.Position);
                    }
                }
            }
        }

        private void 복사ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

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

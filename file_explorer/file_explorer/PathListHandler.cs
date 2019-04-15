using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace file_explorer
{
    class PathListHandler : CurrentState
    {
        static ComboBox mainPathcombobox = new ComboBox();
        static ComboBox mainRecentcombobox = new ComboBox();
        static Button mainpathButton = new Button();
        static string[] selectedRecentpahtitem;
        static bool recentPathClick = false;
        static List<string[]> recentPathitem = new List<string[]>(); // 최근 항목 리스트 현재 선택된것 위로9개 아래로9개의 항목을 보여주는 특징 때문에 특정 인덱스를 받으면 그 위, 아래 짤라서 넣는 방식
        int recentPathitemcount = 0;
        static List<string> mainPathitem = new List<string>();//콤보박스 항목이 존재한 상태에서 항목과 일치한 텍스트를 바꾸면 SelectedIndexChanged 이벤트가 발생해서 항목을 임시로 지우고 다시 넣는 방식을 사용
        public PathListHandler()
        {
        }
        public void PathHandlerSetting(Button mainFormpathbutton, ComboBox mainFormcombobox, ComboBox mainFormrecentcombobox, Graphics mainFormgraphic)
        {
            mainpathButton = mainFormpathbutton;
            mainPathcombobox = mainFormcombobox;
            mainRecentcombobox = mainFormrecentcombobox;
            graphic = mainFormgraphic;
        }

        public void Update()
        {
            MakePathButton();
            if (recentPathClick)
            {
                SetRecentComboBox();
                recentPathClick = false;
            }
            else
            {
                AddRecentComboBox();
            }
            AddPathComboBox();
        }
        private void AddRecentComboBox()//최근 접속위치 저장
        {
            if (mainRecentcombobox.InvokeRequired)
            {
                mainRecentcombobox.Invoke((MethodInvoker)delegate {
                    AddRecentComboBox();
                });
                return;
            }
            int currentIndex = recentPathitem.IndexOf(selectedRecentpahtitem);
            while(currentIndex>0 && currentIndex < recentPathitem.Count()-1)
            {
                recentPathitem.RemoveAt(currentIndex + 1);
            }
            recentPathitemcount = recentPathitem.Count();
            if (mainRecentcombobox.InvokeRequired)
            {
                mainRecentcombobox.Invoke((MethodInvoker)delegate {
                    AddRecentComboBox();
                });
                return;
            }
            string[] dirPaths = currentStaticpath.Split('\\');
            string[] item;
            if (dirPaths.Length == 1)//내 PC
            {
                item = new string[] { dirPaths[0], dirPaths[0], recentPathitemcount.ToString() };
            }
            else if(dirPaths.Length==2 && dirPaths[1].Equals(""))//드라이브
            {
                item = new string[] { dirPaths[0], dirPaths[0]+'\\', recentPathitemcount.ToString() };
            }
            else//폴더
            {
                string dirPath = currentStaticpath.Substring(0, currentStaticpath.LastIndexOf('\\'));
                item = new string[] { dirPaths[dirPaths.Length-2], currentStaticpath, recentPathitemcount.ToString() };
            }
            selectedRecentpahtitem = item;
            recentPathitem.Add(item);
            recentPathitemcount++;
            SetRecentComboBox();
        }
        private void SetRecentComboBox()//현재 위치 +9~-9개의 데이터 출력 ( 현재 문제 있음)
        {
            if (mainRecentcombobox.InvokeRequired)
            {
                mainRecentcombobox.Invoke((MethodInvoker)delegate {
                    SetRecentComboBox();
                });
                return;
            }
            mainRecentcombobox.Items.Clear();
            int currentIndex = recentPathitem.IndexOf(selectedRecentpahtitem);
            for (int recentPathindex= currentIndex + 9; recentPathindex>= currentIndex - 9; recentPathindex--)
            {
                if (recentPathindex >= 0 && recentPathindex< recentPathitem.Count())
                {
                    SizeF size = graphic.MeasureString(recentPathitem[recentPathindex][0], mainRecentcombobox.Font);
                    if (mainRecentcombobox.DropDownWidth < size.Width)
                    {
                        mainRecentcombobox.DropDownWidth = (int)size.Width + 2;
                    }
                    mainRecentcombobox.Items.Add(new { Text = recentPathitem[recentPathindex][0],Value= recentPathitem[recentPathindex]});
                }
            }
            currentIndex = recentPathitem.IndexOf(selectedRecentpahtitem);
            mainRecentcombobox.SelectedIndex = recentPathitem.Count-currentIndex-1;
        }

        private void AddPathComboBox() //최신 경로를 경로 콤보박스에 넣음
        {
            if (mainPathcombobox.InvokeRequired)
            {
                mainPathcombobox.Invoke((MethodInvoker)delegate {
                    AddPathComboBox();
                });
                return;
            }
            mainPathcombobox.Items.Clear();
            mainPathcombobox.Text = currentStaticpath;
            if (!mainPathitem.Contains(currentStaticpath))
            {
                mainPathitem.Add(currentStaticpath);
            }
            mainPathitem.Reverse();//역순 출력
            foreach (string item in mainPathitem)
            {
                mainPathcombobox.Items.Add(item);
            }
            mainPathitem.Reverse();
        }
        public void MainComboBoxEnter()//적은 내용으로 이동
        {
            string writePath = mainPathcombobox.Text;
            isClick = true;
            currentStaticpath = writePath;
            sendServerEventHandler.MoveDir(writePath,"comboenter");
            mainpathButton.Show();
        }
        public void MakePathButton() //path경로에 따른 버튼 추가
        {
            if (mainpathButton.InvokeRequired)
            {
                mainpathButton.Invoke((MethodInvoker)delegate {
                    MakePathButton();
                });
                return;
            }
            mainpathButton.Controls.Clear(); // 내용 지우기

            Button mainButton = new Button(); // main 버튼(내 PC)생성
            mainButton.Location = new Point(0, 0);
            mainButton.Cursor = Cursors.Default;
            mainButton.Text = "내 PC";
            mainButton.Name = "root";
            SizeF size = graphic.MeasureString("내 PC", mainButton.Font);
            mainButton.Width = (int)size.Width + 20;
            mainButton.Height = mainpathButton.Height;
            mainButton.Click += (s, e) => { isClick = true; currentStaticpath = "root"; sendServerEventHandler.MoveDir("root", "rootpathbuttonclick"); };
            mainpathButton.Controls.Add(mainButton);

            if (!currentStaticpath.Equals("root"))
            {
                string[] dirPaths = currentStaticpath.Split('\\');
                int buttonCount = 0;
                int preButtonpoint = 0;
                string path = "";
                foreach (string dirName in dirPaths)
                {
                    if (dirName.Equals("")) break;
                    preButtonpoint += mainpathButton.Controls[buttonCount++].Size.Width - 2;
                    Button dirButton = new Button();
                    dirButton.Location = new Point(preButtonpoint, 0);
                    dirButton.Cursor = Cursors.Default;
                    dirButton.Text = dirName;
                    size = graphic.MeasureString(dirName, mainButton.Font);
                    dirButton.Width = (int)size.Width + 15;
                    dirButton.Height = mainpathButton.Height;
                    path += dirName+'\\';
                    dirButton.Name = path;
                    dirButton.Click += (s, e) => { isClick = true; currentStaticpath = dirButton.Name; sendServerEventHandler.MoveDir(dirButton.Name, "dirpathbuttonclick"); };
                    mainpathButton.Controls.Add(dirButton);
                }
            }

        }
        public void MainPathSelectedChange()//경로 바뀜
        {
            if (mainPathcombobox.InvokeRequired)
            {
                mainPathcombobox.Invoke((MethodInvoker)delegate {
                    MainPathSelectedChange();
                });
                return;
            }
            if (!currentStaticpath.Equals(mainPathcombobox.SelectedItem.ToString()))
            {
                string selectedPath = mainPathcombobox.SelectedItem.ToString();
                isClick = true;
                currentStaticpath = selectedPath;
                sendServerEventHandler.MoveDir(selectedPath,  "comboindexchange");
            }
        }
        public void ErrorState()//잘못된 경로 정하면 뒤로 가기
        {
            while (true)
            {
                if (!prePathsave.Peek().Equals(currentStaticpath)) // 현재 경로가 오류이므로 이전 경로를 가져옴 다르면 그 경로로 이동
                {
                    currentStaticpath = prePathsave.Peek();
                    mainPathcombobox.Text = currentStaticpath;
                    MoveDir(true,currentStaticpath,"errorPathState");
                    break;
                }
                else //경로가 같으면 오류 경로 이므로 이전 경로 호출
                {
                    prePathsave.Pop();
                }
            }
        }
        public void MainRecentPathSelectedIndexChanged()//최근 경로 바꿈
        {
            if (mainRecentcombobox.InvokeRequired)
            {
                mainRecentcombobox.Invoke((MethodInvoker)delegate {
                    MainRecentPathSelectedIndexChanged();
                });
                return;
            }
            string[] selectedItem = (mainRecentcombobox.SelectedItem as dynamic).Value;
            if (!selectedItem[2].Equals(selectedRecentpahtitem[2])){
                recentPathClick = true;
                selectedRecentpahtitem = selectedItem;
                isClick = true;
                currentStaticpath = selectedItem[1];
                sendServerEventHandler.MoveDir(selectedItem[1],  "recentcomboindexchange");
            }
        } 
    }
}

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
        static Graphics graphic;
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
        }
        
        /*public void SetComboxPath(string path)
        {
            SetRecentComboBox(path);
            SetPathComboBox(path);
        }*/
        private void AddRecentComboBox()
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
        private void SetRecentComboBox()
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
        private void AddPathComboBox()
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
            mainPathitem.Reverse();
            foreach (string item in mainPathitem)
            {
                mainPathcombobox.Items.Add(item);
            }
            mainPathitem.Reverse();
        }
        public void MainComboBoxEnter()
        {
            string writePath = mainPathcombobox.Text;
            isClick = true;
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
            mainButton.Click += (s, e) => { isClick = true; sendServerEventHandler.MoveDir("root", "rootpathbuttonclick"); };
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
                    dirButton.Click += (s, e) => { isClick = true; sendServerEventHandler.MoveDir(dirButton.Name, "dirpathbuttonclick"); };
                    mainpathButton.Controls.Add(dirButton);
                }
            }

        }
        private void SetErrorRecentComboBox(string path)
        {
            if (mainRecentcombobox.InvokeRequired)
            {
                mainRecentcombobox.Invoke((MethodInvoker)delegate {
                    SetErrorRecentComboBox(path);
                });
                return;
            }
            string recentPath;
            if (path.Split('\\').Last().Equals(""))
            {
                recentPath = path.Split('\\').First();
            }
            else
            {
                recentPath = path.Split('\\').Last();
            }
            int resultindex = mainRecentcombobox.FindStringExact(recentPath);
            if (resultindex != -1)
            {
                mainRecentcombobox.Items.RemoveAt(resultindex);
            }
        }
        public void MainPathSelectedChange()
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
                sendServerEventHandler.MoveDir(selectedPath,  "comboindexchange");
            }
        }

        public void MainRecentPathSelectedIndexChanged()
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
                sendServerEventHandler.MoveDir(selectedItem[1],  "recentcomboindexchange");
            }
        }
        private void SetErrorPathComboBox()
        {
            /*
            if (mainPathcombobox.InvokeRequired)
            {
                mainPathcombobox.Invoke((MethodInvoker)delegate {
                    SetPathComboBox();
                });
                return;
            }
            int resultindex = mainPathcombobox.FindStringExact(path);
            if (resultindex != -1)
            {
                mainPathcombobox.Items.RemoveAt(resultindex);
            }
            mainPathitem.Remove(path);
            */
        }

        
    }
}
